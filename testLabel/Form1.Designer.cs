namespace testLabel
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.绘制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.处理KMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.测试9交模型ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.绘制ToolStripMenuItem,
            this.处理KMLToolStripMenuItem,
            this.测试9交模型ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 绘制ToolStripMenuItem
            // 
            this.绘制ToolStripMenuItem.Name = "绘制ToolStripMenuItem";
            this.绘制ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.绘制ToolStripMenuItem.Text = "绘制";
            this.绘制ToolStripMenuItem.Click += new System.EventHandler(this.绘制ToolStripMenuItem_Click);
            // 
            // 处理KMLToolStripMenuItem
            // 
            this.处理KMLToolStripMenuItem.Name = "处理KMLToolStripMenuItem";
            this.处理KMLToolStripMenuItem.Size = new System.Drawing.Size(70, 21);
            this.处理KMLToolStripMenuItem.Text = "处理KML";
            this.处理KMLToolStripMenuItem.Click += new System.EventHandler(this.处理KMLToolStripMenuItem_Click);
            // 
            // 测试9交模型ToolStripMenuItem
            // 
            this.测试9交模型ToolStripMenuItem.Name = "测试9交模型ToolStripMenuItem";
            this.测试9交模型ToolStripMenuItem.Size = new System.Drawing.Size(87, 21);
            this.测试9交模型ToolStripMenuItem.Text = "测试9交模型";
            this.测试9交模型ToolStripMenuItem.Click += new System.EventHandler(this.测试9交模型ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 绘制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 处理KMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 测试9交模型ToolStripMenuItem;
    }
}

