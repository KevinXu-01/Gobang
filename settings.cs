using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Gobang
{
    public partial class settings : Form
    {
        public static int setting_bgm;
        public static int setting_level;
        public static int setting_color;
        public settings()
        {
            InitializeComponent();
        }

        private void settings_Load(object sender, EventArgs e)
        {
            int setting_restriction_temp = MainWnd.setting_restriction;
            FileStream file = new FileStream(@"settings.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader file_read = new StreamReader(file);
            setting_bgm = Convert.ToInt32(file_read.ReadLine());//为0关闭，为1打开

            setting_level = Convert.ToInt32(file_read.ReadLine());
            MainWnd.setting_restriction = Convert.ToInt32(file_read.ReadLine());//为0无禁手，为1有禁手
            setting_color = Convert.ToInt32(file_read.ReadLine());
            file_read.Close();
            file.Close();
            if (Play.mode == 2)
                MainWnd.setting_restriction = setting_restriction_temp;
            //还原设置-难度、颜色
            level_selection.SelectedIndex = setting_level;
            colorSelection.SelectedIndex = setting_color;

            //还原设置-播放音乐
            if (setting_bgm == 1)
                this.bgmusic.isCheck = true;
            else
                this.bgmusic.isCheck = false;

            //还原设置-有无禁手
            if (MainWnd.setting_restriction == 1)
                this.restriction_select.isCheck = true;
            else
                this.restriction_select.isCheck = false;
        }

        private void level_selection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void confirm_Click(object sender, EventArgs e)
        {
            MainWnd mainwnd = (MainWnd)this.Owner;//将主窗体设置为子窗体的父窗体

            ////保存设置
            //保存设置-难度、颜色
            if (Play.onGame == false)
            {
                setting_level = level_selection.SelectedIndex;
                setting_color = colorSelection.SelectedIndex;
            }
                        
            //保存设置-播放音乐
            if (this.bgmusic.isCheck)
                setting_bgm = 1;
            else if (!this.bgmusic.isCheck)
                setting_bgm = 0;

            //保存设置-禁手
            if (this.restriction_select.isCheck && Play.onGame == false)
                MainWnd.setting_restriction = 1;
            else if(!this.restriction_select.isCheck && Play.onGame == false)
                MainWnd.setting_restriction = 0;
            else
            {

            }

            if (setting_bgm == 1)
            {
                if (this.bgmusic.isChanged % 2 == 1)//如果确认键按下时，当前设置为播放，且按钮被按
                                                                           //下奇数次，说明之前设置为不播放，于是启动播放
                {
                    mainwnd.backgroundplayer.Play();
                    mainwnd.backgroundplayer.PlayLooping();
                }
                else  //如果确认键按下时，当前设置为播放，且按钮被按
                         //下偶数次，说明之前设置也是播放，于是操作为空,
                         //使之继续播放
                {

                }
            }
            else
                mainwnd.backgroundplayer.Stop();
            //输出到文件
            FileStream file = new FileStream(@"settings.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter file_write = new StreamWriter(file, Encoding.Default);
            file_write.WriteLine(Convert.ToString(setting_bgm));
            file_write.WriteLine(Convert.ToString(setting_level));
            file_write.WriteLine(Convert.ToString(MainWnd.setting_restriction));
            file_write.WriteLine(Convert.ToString(setting_color));
            file_write.Close();
            file.Close();

            this.Close();
        }
    }
}