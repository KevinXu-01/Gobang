namespace Gobang
{
    partial class about
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(about));
            this.about_tip = new System.Windows.Forms.Label();
            this.about_info = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // about_tip
            // 
            this.about_tip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.about_tip.AutoSize = true;
            this.about_tip.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.about_tip.Location = new System.Drawing.Point(158, 21);
            this.about_tip.Name = "about_tip";
            this.about_tip.Size = new System.Drawing.Size(80, 26);
            this.about_tip.TabIndex = 1;
            this.about_tip.Text = "关     于";
            // 
            // about_info
            // 
            this.about_info.AutoSize = true;
            this.about_info.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.about_info.Location = new System.Drawing.Point(18, 66);
            this.about_info.Name = "about_info";
            this.about_info.Size = new System.Drawing.Size(379, 150);
            this.about_info.TabIndex = 2;
            this.about_info.Text = "本程序系2020年春季软件构造基础大作业，\r\n作者是许静宇（学号2018302060052）。\r\n版本：1.0.0.0。\r\n开发环境：Windows 10 190" +
    "9，\r\nVisual Studio 2019（版本：16.6.0），\r\n.Net Framework 4.8.03752。";
            // 
            // about
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 248);
            this.Controls.Add(this.about_info);
            this.Controls.Add(this.about_tip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "about";
            this.ShowInTaskbar = false;
            this.Text = "关于";
            this.Load += new System.EventHandler(this.about_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label about_tip;
        private System.Windows.Forms.Label about_info;
    }
}