namespace Gobang
{
    partial class MainWnd
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
            this.AI_Mode = new System.Windows.Forms.Button();
            this.Locally = new System.Windows.Forms.Button();
            this.Remotely = new System.Windows.Forms.Button();
            this.Continue = new System.Windows.Forms.Button();
            this.help = new System.Windows.Forms.Button();
            this.settings = new System.Windows.Forms.Button();
            this.about = new System.Windows.Forms.Button();
            this.info = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnListen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxSendtoIp = new System.Windows.Forms.TextBox();
            this.txblocalip = new System.Windows.Forms.TextBox();
            this.lblocalIp = new System.Windows.Forms.Label();
            this.lstbxMessageView = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbxMessageSend = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // AI_Mode
            // 
            this.AI_Mode.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AI_Mode.Location = new System.Drawing.Point(841, 12);
            this.AI_Mode.Name = "AI_Mode";
            this.AI_Mode.Size = new System.Drawing.Size(159, 46);
            this.AI_Mode.TabIndex = 0;
            this.AI_Mode.Text = "人机对战";
            this.AI_Mode.UseVisualStyleBackColor = true;
            this.AI_Mode.Click += new System.EventHandler(this.AI_Mode_Click);
            // 
            // Locally
            // 
            this.Locally.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Locally.Location = new System.Drawing.Point(841, 64);
            this.Locally.Name = "Locally";
            this.Locally.Size = new System.Drawing.Size(159, 46);
            this.Locally.TabIndex = 1;
            this.Locally.Text = "双人模式(离线)";
            this.Locally.UseVisualStyleBackColor = true;
            this.Locally.Click += new System.EventHandler(this.Locally_Click);
            // 
            // Remotely
            // 
            this.Remotely.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Remotely.Location = new System.Drawing.Point(841, 116);
            this.Remotely.Name = "Remotely";
            this.Remotely.Size = new System.Drawing.Size(159, 46);
            this.Remotely.TabIndex = 2;
            this.Remotely.Text = "双人模式(在线)";
            this.Remotely.UseVisualStyleBackColor = true;
            this.Remotely.Click += new System.EventHandler(this.Remotely_Click);
            // 
            // Continue
            // 
            this.Continue.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Continue.Location = new System.Drawing.Point(841, 291);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(159, 46);
            this.Continue.TabIndex = 3;
            this.Continue.Text = "残局继续";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // help
            // 
            this.help.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.help.Location = new System.Drawing.Point(841, 343);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(159, 46);
            this.help.TabIndex = 4;
            this.help.Text = "帮助";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // settings
            // 
            this.settings.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.settings.Location = new System.Drawing.Point(841, 395);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(159, 46);
            this.settings.TabIndex = 5;
            this.settings.Text = "设置";
            this.settings.UseVisualStyleBackColor = true;
            this.settings.Click += new System.EventHandler(this.settings_Click);
            // 
            // about
            // 
            this.about.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.about.Location = new System.Drawing.Point(841, 447);
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(159, 46);
            this.about.TabIndex = 6;
            this.about.Text = "关于";
            this.about.UseVisualStyleBackColor = true;
            this.about.Click += new System.EventHandler(this.about_Click);
            // 
            // info
            // 
            this.info.AutoSize = true;
            this.info.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.info.ForeColor = System.Drawing.Color.Red;
            this.info.Location = new System.Drawing.Point(785, 682);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(88, 25);
            this.info.TabIndex = 8;
            this.info.Text = "黑棋先行";
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(936, 247);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(94, 38);
            this.btnConnect.TabIndex = 20;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnListen
            // 
            this.btnListen.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnListen.Location = new System.Drawing.Point(809, 247);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(94, 38);
            this.btnListen.TabIndex = 19;
            this.btnListen.Text = "监听";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(829, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 25);
            this.label2.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(800, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 25);
            this.label1.TabIndex = 17;
            this.label1.Text = "目的IP:";
            // 
            // tbxSendtoIp
            // 
            this.tbxSendtoIp.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxSendtoIp.Location = new System.Drawing.Point(879, 208);
            this.tbxSendtoIp.Name = "tbxSendtoIp";
            this.tbxSendtoIp.Size = new System.Drawing.Size(151, 33);
            this.tbxSendtoIp.TabIndex = 16;
            // 
            // txblocalip
            // 
            this.txblocalip.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txblocalip.Location = new System.Drawing.Point(879, 168);
            this.txblocalip.Name = "txblocalip";
            this.txblocalip.Size = new System.Drawing.Size(151, 33);
            this.txblocalip.TabIndex = 15;
            this.txblocalip.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txblocalip_KeyPress);
            // 
            // lblocalIp
            // 
            this.lblocalIp.AutoSize = true;
            this.lblocalIp.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblocalIp.Location = new System.Drawing.Point(800, 172);
            this.lblocalIp.Name = "lblocalIp";
            this.lblocalIp.Size = new System.Drawing.Size(73, 25);
            this.lblocalIp.TabIndex = 14;
            this.lblocalIp.Text = "本地IP:";
            // 
            // lstbxMessageView
            // 
            this.lstbxMessageView.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstbxMessageView.Location = new System.Drawing.Point(771, 499);
            this.lstbxMessageView.Multiline = true;
            this.lstbxMessageView.Name = "lstbxMessageView";
            this.lstbxMessageView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lstbxMessageView.Size = new System.Drawing.Size(298, 142);
            this.lstbxMessageView.TabIndex = 23;
            this.lstbxMessageView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstbxMessageView_KeyPress);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSend.Location = new System.Drawing.Point(982, 682);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 31);
            this.btnSend.TabIndex = 22;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbxMessageSend
            // 
            this.tbxMessageSend.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxMessageSend.Location = new System.Drawing.Point(771, 647);
            this.tbxMessageSend.Name = "tbxMessageSend";
            this.tbxMessageSend.Size = new System.Drawing.Size(298, 29);
            this.tbxMessageSend.TabIndex = 21;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Gobang.Properties.Resources.Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1069, 738);
            this.Controls.Add(this.lstbxMessageView);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbxMessageSend);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxSendtoIp);
            this.Controls.Add(this.txblocalip);
            this.Controls.Add(this.lblocalIp);
            this.Controls.Add(this.info);
            this.Controls.Add(this.about);
            this.Controls.Add(this.settings);
            this.Controls.Add(this.help);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.Remotely);
            this.Controls.Add(this.Locally);
            this.Controls.Add(this.AI_Mode);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWnd";
            this.Text = "Gobang";
            this.Load += new System.EventHandler(this.MainWnd_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainWnd_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.FormClosing += MainWnd_FormClosing;
        }



        #endregion

        private System.Windows.Forms.Button AI_Mode;
        private System.Windows.Forms.Button Locally;
        private System.Windows.Forms.Button Remotely;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button settings;
        private System.Windows.Forms.Button about;
        private System.Windows.Forms.Label info;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxSendtoIp;
        private System.Windows.Forms.TextBox txblocalip;
        private System.Windows.Forms.Label lblocalIp;
        private System.Windows.Forms.TextBox lstbxMessageView;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbxMessageSend;
    }
}

