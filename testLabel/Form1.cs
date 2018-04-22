using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using SharpMap;
using SharpMap.Utilities;
using SharpMap.Layers;
using SharpMap.Forms;
using SharpMap.Data;
using NetTopologySuite.Geometries;
using GeoAPI;
using GeoAPI.Geometries;
using Coordinate = GeoAPI.Geometries.Coordinate;

using SharpKml;
using SharpKml.Engine;
using SharpKml.Dom;
using SharpMap.Rendering.Thematics;
using SharpMap.Styles;
using LinearRing = SharpKml.Dom.LinearRing;
using LineString = SharpKml.Dom.LineString;
using Point = SharpKml.Dom.Point;
using Polygon = SharpKml.Dom.Polygon;
using Style = SharpKml.Dom.Style;
using System.Xml;
using System.Diagnostics;


namespace testLabel
{
    public partial class Form1 : Form
    {
        MapBox mbox = new MapBox();
        public Form1()
        {
            InitializeComponent();
            this.Controls.Add(mbox);
            mbox.Dock = DockStyle.Fill;
            mbox.BackColor = Color.White;
        }

        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FeatureDataTable fdt = new FeatureDataTable();
            fdt.Columns.Add("FID", typeof(int));
            fdt.Columns.Add("Rotation", typeof(double));

            GeometryFactory geoFactory = new GeometryFactory();
            Coordinate[] lineCoord01 = new Coordinate[] { new Coordinate(0, 0), new Coordinate(10, 10), new Coordinate(20, 20), new Coordinate(0, 0)};
            Coordinate[] lineCoord02 = new Coordinate[] { new Coordinate(10, 20), new Coordinate(10, 30), new Coordinate(10, 48), new Coordinate(0, 0)};
            ILineString line01 = geoFactory.CreateLineString(lineCoord01);
            ILineString line02 = geoFactory.CreateLineString(lineCoord02);

            Coordinate[] pCoordinate01 = new Coordinate[] { new Coordinate(0, 0), new Coordinate(10, 10), new Coordinate(20, 20), new Coordinate(0, 0) };
            Coordinate[] pCoordinate02 = new Coordinate[] { new Coordinate(10, 20), new Coordinate(10, 30), new Coordinate(10, 48), new Coordinate(10, 20) };

            IPolygon polygon01 = geoFactory.CreatePolygon(pCoordinate01);
            IPolygon polygon02 = geoFactory.CreatePolygon(pCoordinate02);

            FeatureDataRow fdr = fdt.NewRow();
            fdr["FID"] = 3;
            fdr["Rotation"] = 180;
            //fdr.Geometry = line01;
            fdr.Geometry = polygon01;
            fdt.Rows.Add(fdr);

            FeatureDataRow fdr02 = fdt.NewRow();
            fdr02["FID"] = 7;
            fdr02["Rotation"] = 270;
            //fdr02.Geometry = line02;
            fdr02.Geometry = polygon02;
            fdt.Rows.Add(fdr02);

            VectorLayer vlayer = new VectorLayer("1");
            vlayer.Style.Fill = new SolidBrush(Color.FromArgb(0, 255, 255, 255));
            vlayer.Style.EnableOutline = true;
            vlayer.DataSource = new SharpMap.Data.Providers.GeometryFeatureProvider(fdt);
            mbox.Map.Layers.Add(vlayer);

            LabelLayer llayer = new LabelLayer("lab");
            llayer.DataSource = new SharpMap.Data.Providers.GeometryFeatureProvider(fdt);
            llayer.LabelColumn = "FID";
            llayer.Style.Rotation = 0;
            llayer.RotationColumn = "Rotation";
            llayer.Style.IsTextOnPath = false;
            llayer.Style.Font = new Font("宋体", 25);


            mbox.Map.Layers.Add(llayer);
            mbox.Map.ZoomToExtents();
            mbox.Refresh();
        }

        private void 处理KMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                NetTopologySuite.Geometries.GeometryFactory gFactory = new GeometryFactory();
                Dictionary<int, FeatureDataTable> fdtDic = new Dictionary<int, FeatureDataTable>();
                XmlDocument doc = new XmlDocument();
                doc.Load(ofd.FileName);
                XmlNode root = doc.DocumentElement;
                string nameUri = root.NamespaceURI;
                XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);
                xnm.AddNamespace("pre", nameUri);
                XmlNodeList folderName = root.SelectNodes("pre:Document/pre:Folder/pre:Folder/pre:name", xnm);
                XmlNode oddNode = null;
                XmlNode evenNode = null;
                for (int i = 0; i < folderName.Count; i++)
                {
                    if (folderName[i].InnerText == "odd num")
                    {
                        oddNode = folderName[i].ParentNode;
                    }

                    if (folderName[i].InnerText == "even num")
                    {
                        evenNode = folderName[i].ParentNode;
                    }
                }
                XmlNodeList folders_Odd = null;
                XmlNodeList folders_Even = null;
                if (oddNode != null)
                {
                    folders_Odd = oddNode.SelectNodes("pre:Folder", xnm);
                }
                if (evenNode != null)
                {
                    folders_Even = evenNode.SelectNodes("pre:Folder", xnm);
                }


                if (folders_Odd != null)
                {
                    for (int i = 0; i < folders_Odd.Count; i++)
                    {
                        FeatureDataTable srcfdt = null;
                        string flihgtNameStr = folders_Odd[i].SelectSingleNode("pre:name", xnm).InnerText.Trim();
                        //寻找偶数
                        XmlNode sameFlightEven = null;
                        if (folders_Even != null)
                        {
                            for (int j = 0; j < folders_Even.Count; j++)
                            {
                                XmlNode flightN = folders_Odd[i].SelectSingleNode("pre:name", xnm);
                                string tempFlightName = flightN.InnerText.Trim();
                                if (tempFlightName == flihgtNameStr)
                                {
                                    sameFlightEven = flightN.ParentNode;
                                }
                            }
                        }

                        XmlNodeList placemarks_Odd = folders_Odd[i].SelectNodes("pre:Placemark", xnm);
                        XmlNodeList placemarks_even = null;
                        if (sameFlightEven != null)
                        {
                            placemarks_even = folders_Even[i].SelectNodes("pre:Placemark", xnm);
                        }

                        string[] flightName = flihgtNameStr.Split(' ');
                        int flihgtId = Convert.ToInt32(flightName[1]);
                        //if (fdtDic.Keys.Contains(flihgtId))
                        //{
                        //    srcfdt = fdtDic[flihgtId];
                        //}
                        //else
                        //{
                        //    srcfdt = new FeatureDataTable();
                        //    srcfdt.Columns.Add("FlightId", typeof(int));
                        //    srcfdt.Columns.Add("Name", typeof(string));
                        //    srcfdt.Columns.Add("OverlapCount", typeof(int));
                        //    fdtDic.Add(flihgtId, srcfdt);
                        //}
                        srcfdt = new FeatureDataTable();
                        srcfdt.Columns.Add("FlightId", typeof(int));
                        srcfdt.Columns.Add("Name", typeof(string));
                        srcfdt.Columns.Add("OverlapCount", typeof(int));
                        fdtDic.Add(flihgtId, srcfdt);
                        for (int k = 0; k < placemarks_Odd.Count; k++)
                        {
                            XmlNode coordinatesN_odd = placemarks_Odd[k].SelectSingleNode("pre:Polygon/pre:outerBoundaryIs/pre:LinearRing/pre:coordinates", xnm);
                            string placemarkName_odd = placemarks_Odd[k].SelectSingleNode("pre:name", xnm).InnerText;
                            string coordinateStr = coordinatesN_odd.InnerText.Trim();
                            string[] coordinateArry = coordinateStr.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            Coordinate[] coordinates = new Coordinate[coordinateArry.Length];
                            for (int j = 0; j < coordinateArry.Length; j++)
                            {
                                string[] xyh = coordinateArry[j].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                coordinates[j] = new Coordinate(Convert.ToDouble(xyh[0]), Convert.ToDouble(xyh[1]), Convert.ToDouble(xyh[2]));
                            }
                            if (coordinateArry.Length > 3)
                            {
                                GeoAPI.Geometries.IPolygon polygon = gFactory.CreatePolygon(coordinates);
                                FeatureDataRow fdr = srcfdt.NewRow();
                                fdr["FlightId"] = flihgtId;
                                Debug.WriteLine(flihgtId);
                                fdr["Name"] = placemarkName_odd;
                                fdr["OverlapCount"] = 0;
                                fdr.Geometry = polygon;
                                srcfdt.AddRow(fdr);
                            }

                            if (placemarks_even != null)
                            {
                                if (k >= placemarks_even.Count)
                                {
                                    continue;
                                }
                                XmlNode coordinatesN_even = placemarks_even[k].SelectSingleNode("pre:Polygon/pre:outerBoundaryIs/pre:LinearRing/pre:coordinates", xnm);
                                string placemarkName_even = placemarks_even[k].SelectSingleNode("pre:name", xnm).InnerText;
                                string coordStr = coordinatesN_odd.InnerText.Trim();
                                string[] coordinateA_even = coordStr.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                Coordinate[] coor = new Coordinate[coordinateA_even.Length];
                                for (int j = 0; j < coordinateA_even.Length; j++)
                                {
                                    string[] xyh = coordinateA_even[j].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                    coor[j] = new Coordinate(Convert.ToDouble(xyh[0]), Convert.ToDouble(xyh[1]), Convert.ToDouble(xyh[2]));
                                }
                                if (coor.Length > 3)
                                {
                                    GeoAPI.Geometries.IPolygon polygon = gFactory.CreatePolygon(coor);
                                    FeatureDataRow fdr = srcfdt.NewRow();
                                    fdr["FlightId"] = flihgtId;
                                    Debug.WriteLine(flihgtId);
                                    fdr["Name"] = placemarkName_even;
                                    fdr["OverlapCount"] = 0;
                                    fdr.Geometry = polygon;
                                    srcfdt.AddRow(fdr);
                                }
                            }
                        }
                    }
                }

                //对fdtDic中的所有航线进行旁向重叠计算
                foreach (var item in fdtDic.Values)
                {
                    overlapLR(item);
                }
                
                FeatureDataTable allfdt = null;//所有的航线都放进来了
                if (fdtDic.Count>0)
                {                   
                    foreach (FeatureDataTable item in fdtDic.Values)
                    {
                        if (allfdt==null)
                        {
                            allfdt = new FeatureDataTable();
                            allfdt.Columns.Add("FlightId", typeof(int));
                            allfdt.Columns.Add("Name", typeof(string));
                            allfdt.Columns.Add("OverlapCount", typeof(int));
                        }
                        foreach (var r in item.Rows)
                        {
                            FeatureDataRow srcFdr = r as FeatureDataRow;
                            FeatureDataRow tempfdr = allfdt.NewRow();
                            tempfdr["FlightId"] = srcFdr["FlightId"];
                            tempfdr["Name"] = srcFdr["Name"];
                            tempfdr["OverlapCount"] = srcFdr["OverlapCount"];
                            if (Convert.ToInt32( srcFdr["OverlapCount"])>0)
                            {
                                Debug.WriteLine(Convert.ToInt32(srcFdr["OverlapCount"]));
                            }
                            tempfdr.Geometry = srcFdr.Geometry;
                            allfdt.Rows.Add(tempfdr);
                        }
                    }

                    VectorStyle overlap0 = new VectorStyle();
                    overlap0.Fill = new SolidBrush(Color.FromArgb(90,10,100,30));
                    overlap0.Outline = new Pen(Color.Yellow, 1.0f);
                    overlap0.EnableOutline = true;

                    VectorStyle overlap2 = new VectorStyle();
                    overlap2.Fill = new SolidBrush(Color.FromArgb(90, 10, 130, 30));
                    overlap0.Outline = new Pen(Color.Yellow, 1.0f);
                    overlap0.EnableOutline = true;

                    VectorStyle overlap3 = new VectorStyle();
                    overlap3.Fill = new SolidBrush(Color.FromArgb(90, 10, 160, 30));
                    overlap0.Outline = new Pen(Color.Yellow, 2.0f);
                    overlap0.EnableOutline = true;

                    VectorStyle overlap4 = new VectorStyle();
                    overlap4.Fill = new SolidBrush(Color.FromArgb(90, 10, 190, 30));
                    overlap0.Outline = new Pen(Color.Yellow, 2.0f);
                    overlap0.EnableOutline = true;

                    VectorStyle overlap5 = new VectorStyle();
                    overlap5.Fill = new SolidBrush(Color.FromArgb(90, 10, 220, 30));
                    overlap0.Outline = new Pen(Color.Yellow, 2.0f);
                    overlap0.EnableOutline = true;

                    VectorStyle defualtStyle = new VectorStyle();
                    defualtStyle.Fill = new SolidBrush(Color.FromArgb(200, Color.Red));
                    //overlap0.Outline = new Pen(Color.Yellow, 1.0f);
                    overlap0.EnableOutline = true;

                    Dictionary<int, IStyle> styles = new Dictionary<int, IStyle>();
                    styles.Add(0, overlap0);
                    styles.Add(1, overlap2);
                    styles.Add(2, overlap3);
                    styles.Add(3, overlap4);
                    styles.Add(4, overlap5);


                    VectorLayer vlayer = new VectorLayer("l");
                    vlayer.DataSource = new SharpMap.Data.Providers.GeometryFeatureProvider(allfdt);
                    vlayer.Theme = new UniqueValuesTheme<int>("OverlapCount", styles, defualtStyle);
                    vlayer.Style.Fill=new SolidBrush(Color.FromArgb(30,Color.Green));
                    vlayer.Enabled=true;
                    mbox.Map.Layers.Add(vlayer);
                    mbox.Map.ZoomToExtents();
                    mbox.Refresh();
	            }               
            }
        }

               
                //if (srcfdtAll.Rows.Count > 0)
                //{
                //    VectorLayer vlayer = new VectorLayer("kml");
                //    vlayer.DataSource = new SharpMap.Data.Providers.GeometryFeatureProvider(srcfdtAll);
                //    vlayer.Style.Fill = new SolidBrush(Color.FromArgb(30, Color.Green));
                //    vlayer.Enabled = true;
                //    mbox.Map.Layers.Add(vlayer);
                //    mbox.Map.ZoomToExtents();
                //    mbox.Refresh();
                //}

                   

       /// <summary>
        /// 计算旁向重叠
       /// </summary>
       /// <param name="flight">包含一条航线上的所有图形属性，所有的几何图形存储位置必须是依次连续存储</param>
        private void overlapLR(FeatureDataTable flight)
        {
            if (flight.Rows.Count < 2)
            {
                return;
            }
            List<FeatureDataRow> removeFeature = new List<FeatureDataRow>();
            for (int i = 0; i < flight.Rows.Count-1; i++)
            {
                FeatureDataRow targetFeature = flight.Rows[i] as FeatureDataRow;
                IGeometry p = targetFeature.Geometry;
                for (int j = i + 1; ( j < flight.Rows.Count); j++)
                {
                    FeatureDataRow tempFeature = flight.Rows[i] as FeatureDataRow;
                    if (p.Intersects(tempFeature.Geometry))
                    {
                        IGeometry intersection = p.Intersection(tempFeature.Geometry);
                        IGeometry diff = p.Difference(tempFeature.Geometry);
                        if (intersection.Area > 0)
                        {
                            FeatureDataRow fdr = flight.NewRow();
                            fdr["FlightId"] = targetFeature["FlightId"];
                            fdr["Name"] = targetFeature["Name"];
                            int overlap = 0;
                            if (int.TryParse(targetFeature["OverlapCount"].ToString(), out overlap))
                            {
                                fdr["OverlapCount"] = overlap + 1;
                            }
                            else
                                fdr["OverlapCount"] = 1;
                            fdr.Geometry = intersection;
                            flight.AddRow(fdr);
                        }
                        if (diff.Area > 0)
                        {
                            targetFeature.Geometry = diff;
                        }
                        else
                        {
                            targetFeature.Geometry = diff;
                            removeFeature.Add(targetFeature);
                        }
                    }
                    else
                        break;
                }
                foreach (var item in removeFeature)
                {
                    flight.RemoveRow(item);
                }
                removeFeature.Clear();
            }           
        }



        private void 测试9交模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetTopologySuite.Geometries.GeometryFactory geoFactory = new GeometryFactory();
            Coordinate[] cood01 = new Coordinate[4];
            cood01[0] = new Coordinate(0, 0);
            cood01[1] = new Coordinate(10, 10);
            cood01[2] = new Coordinate(10, 0);
            cood01[3] = cood01[0];

            Coordinate[] cood02 = new Coordinate[4];
            cood02[0] = new Coordinate(0, 0);
            cood02[1] = new Coordinate(10, 10);
            cood02[2] = new Coordinate(10, 0);
            cood02[3] = cood02[0];

            IPolygon p1 = geoFactory.CreatePolygon(cood01);
            IPolygon p2 = geoFactory.CreatePolygon(cood02);
            IGeometry diff = p1.Difference(p2);
            for (int i = 0; i < diff.Coordinates.Length; i++)
            {
                Debug.WriteLine("X:{0}  Y:{1}", diff.Coordinates[i].X, diff.Coordinates[i].Y);
            }


            IGeometry intersection = p1.Intersection(p2);
            for (int i = 0; i < intersection.Coordinates.Length; i++)
            {
                Debug.WriteLine("X:{0}  Y:{1}", intersection.Coordinates[i].X, intersection.Coordinates[i].Y);
            }
        }
    }
}
