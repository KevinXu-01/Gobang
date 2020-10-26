namespace Gobang
{
    partial class settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settings));
            this.BackgroundMusic = new System.Windows.Forms.Label();
            this.Level = new System.Windows.Forms.Label();
            this.level_selection = new System.Windows.Forms.ComboBox();
            this.restriction = new System.Windows.Forms.Label();
            this.colorSelection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.confirm = new System.Windows.Forms.Button();
            this.restriction_select = new Gobang.CheckButton();
            this.bgmusic = new Gobang.CheckButton();
            this.SuspendLayout();
            // 
            // BackgroundMusic
            // 
            this.BackgroundMusic.AutoSize = true;
            this.BackgroundMusic.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BackgroundMusic.Location = new System.Drawing.Point(133, 39);
            this.BackgroundMusic.Name = "BackgroundMusic";
            this.BackgroundMusic.Size = new System.Drawing.Size(124, 25);
            this.BackgroundMusic.TabIndex = 1;
            this.BackgroundMusic.Text = "背  景  音  乐";
            // 
            // Level
            // 
            this.Level.AutoSize = true;
            this.Level.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Level.Location = new System.Drawing.Point(131, 96);
            this.Level.Name = "Level";
            this.Level.Size = new System.Drawing.Size(126, 25);
            this.Level.TabIndex = 2;
            this.Level.Text = "人机对战难度";
            // 
            // level_selection
            // 
            this.level_selection.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.level_selection.FormattingEnabled = true;
            this.level_selection.Items.AddRange(new object[] {
            "易",
            "中"});
            this.level_selection.Location = new System.Drawing.Point(291, 96);
            this.level_selection.Name = "level_selection";
            this.level_selection.Size = new System.Drawing.Size(121, 33);
            this.level_selection.TabIndex = 3;
            this.level_selection.SelectedIndexChanged += new System.EventHandler(this.level_selection_SelectedIndexChanged);
            // 
            // restriction
            // 
            this.restriction.AutoSize = true;
            this.restriction.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.restriction.Location = new System.Drawing.Point(133, 154);
            this.restriction.Name = "restriction";
            this.restriction.Size = new System.Drawing.Size(122, 25);
            this.restriction.TabIndex = 4;
            this.restriction.Text = "禁            手";
            // 
            // colorSelection
            // 
            this.colorSelection.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.colorSelection.FormattingEnabled = true;
            this.colorSelection.Items.AddRange(new object[] {
            "黑棋",
            "白棋"});
            this.colorSelection.Location = new System.Drawing.Point(291, 207);
            this.colorSelection.Name = "colorSelection";
            this.colorSelection.Size = new System.Drawing.Size(121, 33);
            this.colorSelection.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(133, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "人机对战中你执";
            // 
            // confirm
            // 
            this.confirm.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.confirm.Location = new System.Drawing.Point(222, 257);
            this.confirm.Name = "confirm";
            this.confirm.Size = new System.Drawing.Size(106, 39);
            this.confirm.TabIndex = 8;
            this.confirm.Text = "确定";
            this.confirm.UseVisualStyleBackColor = true;
            this.confirm.Click += new System.EventHandler(this.confirm_Click);
            // 
            // restriction_select
            // 
            this.restriction_select.BackColor = System.Drawing.Color.Transparent;
            this.restriction_select.Checked = false;
            this.restriction_select.Cursor = System.Windows.Forms.Cursors.Hand;
            this.restriction_select.Location = new System.Drawing.Point(291, 154);
            this.restriction_select.Name = "restriction_select";
            this.restriction_select.Size = new System.Drawing.Size(87, 27);
            this.restriction_select.TabIndex = 5;
            // 
            // bgmusic
            // 
            this.bgmusic.BackColor = System.Drawing.Color.Transparent;
            this.bgmusic.Checked = false;
            this.bgmusic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bgmusic.Location = new System.Drawing.Point(291, 39);
            this.bgmusic.Name = "bgmusic";
            this.bgmusic.Size = new System.Drawing.Size(87, 27);
            this.bgmusic.TabIndex = 0;
            // 
            // settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 314);
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colorSelection);
            this.Controls.Add(this.restriction_select);
            this.Controls.Add(this.restriction);
            this.Controls.Add(this.level_selection);
            this.Controls.Add(this.Level);
            this.Controls.Add(this.BackgroundMusic);
            this.Controls.Add(this.bgmusic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "settings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckButton bgmusic;
        private System.Windows.Forms.Label BackgroundMusic;
        private System.Windows.Forms.Label Level;
        private System.Windows.Forms.ComboBox level_selection;
        private System.Windows.Forms.Label restriction;
        private CheckButton restriction_select;
        private System.Windows.Forms.ComboBox colorSelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button confirm;
    }
}