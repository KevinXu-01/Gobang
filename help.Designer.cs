namespace Gobang
{
    partial class help
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(help));
            this.about_tip = new System.Windows.Forms.Label();
            this.about_info = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // about_tip
            // 
            this.about_tip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.about_tip.AutoSize = true;
            this.about_tip.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.about_tip.Location = new System.Drawing.Point(243, 50);
            this.about_tip.Name = "about_tip";
            this.about_tip.Size = new System.Drawing.Size(80, 26);
            this.about_tip.TabIndex = 2;
            this.about_tip.Text = "帮     助";
            // 
            // about_info
            // 
            this.about_info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.about_info.AutoSize = true;
            this.about_info.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.about_info.Location = new System.Drawing.Point(96, 100);
            this.about_info.Name = "about_info";
            this.about_info.Size = new System.Drawing.Size(373, 125);
            this.about_info.TabIndex = 3;
            this.about_info.Text = "五子棋是全国智力运动会竞技项目之一，\r\n是一种两人对弈的纯策略型棋类游戏。\r\n通常双方分别使用黑白两色的棋子，\r\n下在棋盘直线与横线的交叉点上，\r\n先在四个方向之" +
    "一上形成五子连线者获胜。";
            this.about_info.Click += new System.EventHandler(this.about_info_Click);
            // 
            // help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 289);
            this.Controls.Add(this.about_info);
            this.Controls.Add(this.about_tip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "help";
            this.Text = "帮助";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label about_tip;
        private System.Windows.Forms.Label about_info;
    }
}