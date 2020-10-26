using System;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Gobang
{
    public partial class MainWnd : Form
    {
        public static MainWnd mainwnd = new MainWnd();//复制一个引用，以便Play.cs中的AI调用
        public const int CB_GAP_X = 53;
        public const int CB_GAP_Y = 51;
        public const int CB_OFFSET = 17;//棋盘左、上存在17单位的间隙
        public readonly SoundPlayer backgroundplayer = new SoundPlayer(Properties.Resources.ResourceManager.GetStream("bgm"));
        public static int[,] ChessBack = new int[15, 15];//棋盘
        public static int[,] temp = new int[15, 15];//用在禁手判断中，是棋盘的副本
        public static int setting_restriction;//禁手设置
        public static bool game_over = false;
        //网络相关
        public static ControlInternet CI = new ControlInternet(); //实例化的联网对战类
        //网络绘图用
        public delegate void Black_Piece_Invoke(BlackPiece blackpiece);
        public delegate void White_Piece_Invoke(WhitePiece whitepiece);
        public delegate void info_update(string str);
        public MainWnd()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            FileStream file = new FileStream(@"settings.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader file_read = new StreamReader(file);
            int isPlayMusic = Convert.ToInt32(file_read.ReadLine());
            file_read.Close();
            file.Close();
            backgroundplayer.Load();//加载音乐
            if (isPlayMusic == 1)
            {
                backgroundplayer.Play();
                backgroundplayer.PlayLooping();
            }
        }
        /// <summary>
        /// 该函数用于初始化游戏，包括初始化棋盘、按钮等等
        /// </summary>
        public void Initialize()
        {
            game_over = false;
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    ChessBack[i, j] = 0;
            Play.onGame = false;
            Play.player = true;
            Play.mode = -1;
            Play.start = true;
            AI_Mode.Enabled = true;
            Locally.Enabled = true;
            Remotely.Enabled = true;
            Continue.Enabled = true;
            btnConnect.Enabled = false;
            btnListen.Enabled = false;
            txblocalip.Enabled = false;
            tbxSendtoIp.Enabled = false;
            tbxMessageSend.Enabled = false;
            lstbxMessageView.Enabled = false;
            btnSend.Enabled = false;
            lstbxMessageView.Text = "";
        }
        
        //网络绘制时使用，调用了BeginInvoke
        /// <summary>
        /// 该函数负责网络对战时，绘制黑棋
        /// </summary>
        /// <param name="blackpiece"></param>
        public void DrawBlackPiece(BlackPiece blackpiece)
        {
            this.Controls.Add(blackpiece);
        }
        
        /// <summary>
        /// 该函数负责网络对战时，绘制白棋
        /// </summary>
        /// <param name="whitepice"></param>
        public void DrawWhitePiece(WhitePiece whitepice)
        {
            this.Controls.Add(whitepice);
        }
        
        /// <summary>
        /// 该函数负责网络对战时，更新提示信息
        /// </summary>
        /// <param name="str"></param>
        public void UpdateInfo(string str)
        {
            this.info.Text = str;
        }
        public Point point_formal = new Point(0, 0);

        /// <summary>
        /// 该函数用于网络对战中绘制棋子和维护游戏过程
        /// </summary>
        /// <param name="X">棋子横坐标</param>
        /// <param name="Y">棋子纵坐标</param>
        public void GameOn_NetWork(int X, int Y)
        {
            if (Play.onGame)
            {
                if (Play.mode == 2 && Play.sender == true && Play.start == true)
                    return;
                else
                {
                    //计算落下位置
                    Play.bX = X;
                    Play.bY = Y;
                    //防止在同一个位置落子
                    if (ChessBack[Play.bX, Play.bY] != 0)
                        return;

                    if (setting_restriction == 0)//无禁手
                    {
                        if (Play.player)
                        {
                            BlackPiece black = new BlackPiece((int)(CB_OFFSET + (Play.bX - 0.5) * CB_GAP_X), (int)(17 + (Play.bY - 0.5) * CB_GAP_Y));
                            Black_Piece_Invoke black_piece_invoke = new Black_Piece_Invoke(DrawBlackPiece);
                            this.BeginInvoke(black_piece_invoke, new object[] { black });
                        }
                        else
                        {
                            WhitePiece white = new WhitePiece((int)(CB_OFFSET + (Play.bX - 0.5) * CB_GAP_X), (int)(17 + (Play.bY - 0.5) * CB_GAP_Y));
                            White_Piece_Invoke white_piece_invoke = new White_Piece_Invoke(DrawWhitePiece);
                            this.BeginInvoke(white_piece_invoke, new object[] { white });
                        }
                        ChessBack[Play.bX, Play.bY] = Play.player ? 1 : 2;
                        string information;
                        if (Play.player)
                            information = "该白棋了";
                        else
                            information = "该黑棋了";
                        info_update infoupdate = new info_update(UpdateInfo);
                        this.BeginInvoke(infoupdate, new object[] { information });

                        //判断棋盘是否满了
                        if (IsFull() && !Victory(Play.bX, Play.bY))
                        {
                            if (MessageBox.Show("平局，游戏结束。") == DialogResult.OK)
                            {
                                CI.close();
                                Initialize();
                            }
                            return;
                        }

                        //判断胜利，每有一枚棋子落下(无论黑、白)，均判断一次输赢
                        if (Victory(Play.bX, Play.bY))
                        {
                            string Vic = Play.player ? "执黑棋者" : "执白棋者";
                            if (MessageBox.Show(Vic + "胜利！") == DialogResult.OK)
                            {
                                foreach (Control control in this.Controls)
                                {
                                    if (control is WhitePiece)
                                        control.Dispose();
                                }
                                int Black_chess_count = 0;
                                for (int i = 0; i < 14; i++)
                                    for (int j = 0; j < 14; j++)
                                    {
                                        if (ChessBack[i, j] == 1)
                                            Black_chess_count++;
                                    }
                                for (int i = 0; i < Black_chess_count; i++)
                                    foreach (Control control in this.Controls)
                                    {
                                        if (control is BlackPiece)
                                            control.Dispose();
                                    }
                                CI.close();
                                Initialize();
                            }
                            return;
                        }

                        //换人
                        Play.player = !Play.player;
                    }

                    else//设置有禁手的时候
                    {
                        //先判断落点是否属于禁手
                        for (int i = 0; i < 15; i++)
                            for (int j = 0; j < 15; j++)
                                temp[i, j] = ChessBack[i, j];
                        int restriction = ForbiddenCheck(Play.bX, Play.bY);
                        ChessBack[Play.bX, Play.bY] = Play.player ? 1 : 2;
                        if (Play.player && Victory(Play.bX, Play.bY) && restriction != 3 && restriction != 0)//非长连禁手成立和五连同时成立，黑方胜
                        {
                            if (MessageBox.Show("执黑棋者胜利！") == DialogResult.OK)
                            {
                                //移除在游戏中建立的picturebox对象
                                foreach (Control control in this.Controls)
                                {
                                    if (control is WhitePiece)
                                        control.Dispose();
                                }
                                int Black_chess_count = 0;
                                for (int i = 0; i < 14; i++)
                                    for (int j = 0; j < 14; j++)
                                    {
                                        if (ChessBack[i, j] == 1)
                                            Black_chess_count++;
                                    }
                                for (int i = 0; i < Black_chess_count; i++)
                                    foreach (Control control in this.Controls)
                                    {
                                        if (control is BlackPiece)
                                            control.Dispose();
                                    }
                                CI.close();
                                Initialize();
                            }
                            return;
                        }
                        else if (Play.player && restriction != 0)//禁手成立，白棋获胜
                        {
                            if (MessageBox.Show("出现禁手，执白棋者胜利！") == DialogResult.OK)
                            {
                                //移除在游戏中建立的picturebox对象
                                foreach (Control control in this.Controls)
                                {
                                    if (control is WhitePiece)
                                        control.Dispose();
                                }
                                int Black_chess_count = 0;
                                for (int i = 0; i < 14; i++)
                                    for (int j = 0; j < 14; j++)
                                    {
                                        if (ChessBack[i, j] == 1)
                                            Black_chess_count++;
                                    }
                                for (int i = 0; i < Black_chess_count; i++)
                                    foreach (Control control in this.Controls)
                                    {
                                        if (control is BlackPiece)
                                            control.Dispose();
                                    }
                                CI.close();
                                Initialize();
                            }
                            return;
                        }

                        else if (Play.player && restriction == 0)
                        {
                            BlackPiece black = new BlackPiece((int)(CB_OFFSET + (Play.bX - 0.5) * CB_GAP_X), (int)(17 + (Play.bY - 0.5) * CB_GAP_Y));
                            Black_Piece_Invoke black_piece_invoke = new Black_Piece_Invoke(DrawBlackPiece);
                            this.BeginInvoke(black_piece_invoke, new object[] { black });
                        }
                        else
                        {
                            WhitePiece white = new WhitePiece((int)(CB_OFFSET + (Play.bX - 0.5) * CB_GAP_X), (int)(17 + (Play.bY - 0.5) * CB_GAP_Y));
                            White_Piece_Invoke white_piece_invoke = new White_Piece_Invoke(DrawWhitePiece);
                            this.BeginInvoke(white_piece_invoke, new object[] { white });
                        }

                        string information;
                        if (Play.player)
                            information = "该白棋了";
                        else
                            information = "该黑棋了";
                        info_update infoupdate = new info_update(UpdateInfo);
                        this.BeginInvoke(infoupdate, new object[] { information });

                        //判断棋盘是否满了
                        if (IsFull() && !Victory(Play.bX, Play.bY))
                        {
                            if (MessageBox.Show("平局，游戏结束。") == DialogResult.OK)
                            {
                                CI.close();
                                Initialize();
                            }
                            return;
                        }
                        //判断胜利，每有一枚棋子落下(无论黑、白)，均判断一次输赢
                        if (Victory(Play.bX, Play.bY))
                        {
                            string Vic = Play.player ? "执黑棋者" : "执白棋者";
                            if (MessageBox.Show(Vic + "胜利！") == DialogResult.OK)
                            {
                                //移除在游戏中建立的picturebox对象
                                foreach (Control control in this.Controls)
                                {
                                    if (control is WhitePiece)
                                        control.Dispose();
                                }
                                int Black_chess_count = 0;
                                for (int i = 0; i < 14; i++)
                                    for (int j = 0; j < 14; j++)
                                    {
                                        if (ChessBack[i, j] == 1)
                                            Black_chess_count++;
                                    }
                                for (int i = 0; i < Black_chess_count; i++)
                                    foreach (Control control in this.Controls)
                                    {
                                        if (control is BlackPiece)
                                            control.Dispose();
                                    }
                                CI.close();
                                Initialize();
                            }
                            return;
                        }

                        //换人
                        Play.player = !Play.player;
                    }
                }
                Play.start = !Play.start;
            }
            else
                return;
        }

        /// <summary>
        /// 该函数用于双人离线对战和人机对战中绘制棋子和维护游戏过程
        /// </summary>
        /// <param name="X">棋子横坐标</param>
        /// <param name="Y">棋子纵坐标</param>
        public void GameOn(int X, int Y)//绘制棋子和维护游戏过程
        {
            if (Play.onGame)
            {
                if ((Play.mode == 2 && Play.sender == true && Play.player == false) || Play.mode == 2 && Play.sender == false && Play.start == true)
                    return;
                else
                {
                    //计算落下位置
                    Play.bX = (int)((X + CB_GAP_X / 2) / CB_GAP_X);
                    Play.bY = (int)((Y + CB_GAP_Y / 2) / CB_GAP_Y);
                    //防止在同一个位置落子
                    if (ChessBack[Play.bX, Play.bY] != 0)
                        return;

                    if (setting_restriction == 0)//无禁手
                    {
                        if (Play.player)
                            this.Controls.Add(new BlackPiece((int)(CB_OFFSET + (Play.bX - 0.5) * CB_GAP_X), (int)(17 + (Play.bY - 0.5) * CB_GAP_Y)));
                        else
                            this.Controls.Add(new WhitePiece((int)(CB_OFFSET + (Play.bX - 0.5) * CB_GAP_X), (int)(17 + (Play.bY - 0.5) * CB_GAP_Y)));
                        ChessBack[Play.bX, Play.bY] = Play.player ? 1 : 2;
                        if (Play.player)
                            info.Text = "该白棋了";
                        else
                            info.Text = "该黑棋了";
                        //判断棋盘是否满了
                        if (IsFull() && !Victory(Play.bX, Play.bY))
                        {
                            if (MessageBox.Show("平局，游戏结束。") == DialogResult.OK)
                                Initialize();
                            return;
                        }
                        //判断胜利，每有一枚棋子落下(无论黑、白)，均判断一次输赢
                        if (Victory(Play.bX, Play.bY))
                        {
                            game_over = true;
                            string Vic = Play.player ? "执黑棋者" : "执白棋者";
                            if (MessageBox.Show(Vic + "胜利！") == DialogResult.OK)
                            {
                                //移除在游戏中建立的picturebox对象
                                foreach (Control control in this.Controls)
                                {
                                    if (control is WhitePiece)
                                        control.Dispose();
                                }
                                int Black_chess_count = 0;
                                for (int i = 0; i < 14; i++)
                                    for (int j = 0; j < 14; j++)
                                    {
                                        if (ChessBack[i, j] == 1)
                                            Black_chess_count++;
                                    }
                                for (int i = 0; i < Black_chess_count; i++)
                                    foreach (Control control in this.Controls)
                                    {
                                        if (control is BlackPiece)
                                            control.Dispose();
                                    }
                                Initialize();
                            }
                            return;
                        }

                        //换人
                        Play.player = !Play.player;
                    }

                    else//设置有禁手的时候
                    {
                        int restriction = 0;
                        if (Play.mode != 0 && Play.player)
                        {
                            //先判断落点是否属于禁手
                            for (int i = 0; i < 15; i++)
                                for (int j = 0; j < 15; j++)
                                    temp[i, j] = ChessBack[i, j];
                            restriction = ForbiddenCheck(Play.bX, Play.bY);
                        }
                        ChessBack[Play.bX, Play.bY] = Play.player ? 1 : 2;
                        if (Play.player && Victory(Play.bX, Play.bY) && restriction != 3 && restriction != 0)//非长连禁手成立和五连同时成立，黑方胜
                        {
                            game_over = true;
                            if (MessageBox.Show("执黑棋者胜利！") == DialogResult.OK)
                            {
                                //移除在游戏中建立的picturebox对象
                                foreach (Control control in this.Controls)
                                {
                                    if (control is WhitePiece)
                                        control.Dispose();
                                }
                                int Black_chess_count = 0;
                                for (int i = 0; i < 14; i++)
                                    for (int j = 0; j < 14; j++)
                                    {
                                        if (ChessBack[i, j] == 1)
                                            Black_chess_count++;
                                    }
                                for (int i = 0; i < Black_chess_count; i++)
                                    foreach (Control control in this.Controls)
                                    {
                                        if (control is BlackPiece)
                                            control.Dispose();
                                    }
                                Initialize();
                            }
                            return;
                        }
                        else if (Play.player && restriction != 0)//禁手成立，白棋获胜
                        {
                            game_over = true;
                            if (MessageBox.Show("出现禁手，执白棋者胜利！") == DialogResult.OK)
                            {
                                //移除在游戏中建立的picturebox对象
                                foreach (Control control in this.Controls)
                                {
                                    if (control is WhitePiece)
                                        control.Dispose();
                                }
                                int Black_chess_count = 0;
                                for (int i = 0; i < 14; i++)
                                    for (int j = 0; j < 14; j++)
                                    {
                                        if (ChessBack[i, j] == 1)
                                            Black_chess_count++;
                                    }
                                for (int i = 0; i < Black_chess_count; i++)
                                    foreach (Control control in this.Controls)
                                    {
                                        if (control is BlackPiece)
                                            control.Dispose();
                                    }
                                Initialize();
                            }
                            return;
                        }
                        else if (Play.player && restriction == 0)//禁手不成立，绘制
                            this.Controls.Add(new BlackPiece((int)(CB_OFFSET + (Play.bX - 0.5) * CB_GAP_X), (int)(17 + (Play.bY - 0.5) * CB_GAP_Y)));
                        else
                            this.Controls.Add(new WhitePiece((int)(CB_OFFSET + (Play.bX - 0.5) * CB_GAP_X), (int)(17 + (Play.bY - 0.5) * CB_GAP_Y)));
                        //ChessBack[Play.bX, Play.bY] = Play.player ? 1 : 2;
                        if (Play.player)
                            info.Text = "该白棋了";
                        else
                            info.Text = "该黑棋了";
                        //判断棋盘是否满了
                        if (IsFull() && !Victory(Play.bX, Play.bY))
                        {
                            if (MessageBox.Show("平局，游戏结束。") == DialogResult.OK)
                                Initialize();
                            return;
                        }
                        //判断胜利，每有一枚棋子落下(无论黑、白)，均判断一次输赢
                        if (Victory(Play.bX, Play.bY))
                        {
                            string Vic = Play.player ? "执黑棋者" : "执白棋者";
                            if (MessageBox.Show(Vic + "胜利！") == DialogResult.OK)
                            {
                                //移除在游戏中建立的picturebox对象
                                foreach (Control control in this.Controls)
                                {
                                    if (control is WhitePiece)
                                        control.Dispose();
                                }
                                int Black_chess_count = 0;
                                for (int i = 0; i < 14; i++)
                                    for (int j = 0; j < 14; j++)
                                    {
                                        if (ChessBack[i, j] == 1)
                                            Black_chess_count++;
                                    }
                                for (int i = 0; i < Black_chess_count; i++)
                                    foreach (Control control in this.Controls)
                                    {
                                        if (control is BlackPiece)
                                            control.Dispose();
                                    }
                                Initialize();
                            }
                            return;
                        }

                        //换人
                        Play.player = !Play.player;
                    }
                }
                Play.start = !Play.start;
            }

            else
                return;
        }
        public void MainWnd_MouseClick(object sender, MouseEventArgs e)//在本地模式中，当鼠标click时，会调用GameOn函数
        {
            if (e.X <= 768)
            {
                int bX = (int)((e.X + CB_GAP_X / 2) / CB_GAP_X);
                int bY = (int)((e.Y + CB_GAP_Y / 2) / CB_GAP_Y);
                if (Play.mode == 2 && Play.sender == true && Play.player == true)
                {
                    CI.sendMsg(0, Convert.ToByte(setting_restriction), (bX + bY * 15).ToString());
                }
                else if(Play.mode == 2 && Play.sender == false && Play.player == false)
                {
                    CI.sendMsg(2, Convert.ToByte(setting_restriction), (bX + bY * 15).ToString());
                }
                GameOn(e.X, e.Y);
                if (Play.mode == 0)//人机模式，玩家执黑棋
                {
                    point_formal = Play.AI_Data_Process();
                    GameOn(point_formal.X, point_formal.Y);
                }
            }
            else//////////////需要现在屏幕非游戏区的空白部分单击以激活AI
            {
                if (Play.mode == 0 && Gobang.settings.setting_color == 1)//人机模式，玩家执白棋
                {
                    point_formal = Play.AI_Data_Process();
                    GameOn(point_formal.X, point_formal.Y);
                }
            }
        }
        
        /// <summary>
        /// 判断棋盘是否满
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            bool full = true;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (ChessBack[i, j] == 0)
                        full = false;
                }
            }
            game_over = full;
            return full;
        }

        #region 判断胜利

        /// <summary>
        /// 判断是否胜利
        /// </summary>
        /// <param name="bX">当前落子的横坐标</param>
        /// <param name="bY">当前落子的纵坐标</param>
        /// <returns></returns>
        public bool Victory(int bX, int bY)
        {
            if (HorVic(bX, bY) || VerVic(bX, bY) || Vic45(bX, bY))//Horizon, 水平; Vertical, 竖直; 45°倾斜
                return true;
            else
                return false;
        }

        public bool Vic45(int bX, int bY)//倾斜方向
        {
            int b1 = (bX - 4) > 0 ? bX - 4 : 0;
            int b2 = (bY - 4) > 0 ? bY - 4 : 0;
            int b3 = (bX + 4) < 15 ? bX + 4 : 14;
            int b4 = (bY + 4) < 15 ? bY + 4 : 14;
            int val = ChessBack[bX, bY];
            for (int i = b3, j = b4; i > 4 && j > 4; i--, j--)//左上判断
            {
                if (ChessBack[i, j] == val && ChessBack[i - 1, j - 1] == val &&
                    ChessBack[i - 2, j - 2] == val && ChessBack[i - 3, j - 3] == val
                    && ChessBack[i - 4, j - 4] == val)
                    return true;
            }
            for (int i = b1, j = b2; i < 11 && j < 11; i++, j++)//右下判断
            {
                if (ChessBack[i, j] == val && ChessBack[i + 1, j + 1] == val &&
                    ChessBack[i + 2, j + 2] == val && ChessBack[i + 3, j + 3] == val
                    && ChessBack[i + 4, j + 4] == val)
                    return true;
            }
            for (int i = b3, j = b2; i > 4 && j < 11; i--, j++)//左下判断
            {
                if (ChessBack[i, j] == val && ChessBack[i - 1, j + 1] == val &&
                    ChessBack[i - 2, j + 2] == val && ChessBack[i - 3, j + 3] == val
                    && ChessBack[i - 4, j + 4] == val)
                    return true;
            }
            for (int i = b1, j = b4; i < 11 && j > 4; i++, j--)//右上判断
            {
                if (ChessBack[i, j] == val && ChessBack[i + 1, j - 1] == val &&
                    ChessBack[i + 2, j - 2] == val && ChessBack[i + 3, j - 3] == val
                    && ChessBack[i + 4, j - 4] == val)
                    return true;
            }
            return false;
        }

        public bool VerVic(int bX, int bY)//自上而下判断
        {
            int top = (bY - 4) > 0 ? bY - 4 : 0;
            int val = ChessBack[bX, bY];
            for (int i = top; i < 11; i++)
            {
                if (ChessBack[bX, i] == val && ChessBack[bX, i + 1] == val &&
                    ChessBack[bX, i + 2] == val && ChessBack[bX, i + 3] == val
                    && ChessBack[bX, i + 4] == val)
                    return true;
            }
            return false;
        }

        public bool HorVic(int bX, int bY)//自左而右判断
        {
            int left = (bX - 4) > 0 ? bX - 4 : 0;
            int val = ChessBack[bX, bY];
            for (int i = left; i < 11; i++)
            {
                if (ChessBack[i, bY] == val && ChessBack[i + 1, bY] == val &&
                    ChessBack[i + 2, bY] == val && ChessBack[i + 3, bY] == val
                    && ChessBack[i + 4, bY] == val)
                    return true;
            }
            return false;
        }
        #endregion


        #region 判断禁手
        const int NO_FORBIDDEN = 0;
        const int THREE_THREE_FORBIDDEN = 1;
        const int FOUR_FOUR_FORBIDDEN = 2;
        const int LONG_FORBIDDEN = 3;
        const int NONE = 0;
        const int BLACK = 1;
        /// <summary>
        /// 通过递归调用，判断有无禁手
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="adjsame"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static int KeyPointForbiddenCheck(int x, int y, int adjsame, int direction)//辅助检查
        {
            int i, j;//关键点坐标(i, j)
            adjsame++;
            if (direction >= 4)
                adjsame = -adjsame;

            //计算关键点坐标
            switch (direction % 4)
            {
                case 0: i = x; j = y - adjsame; break;
                case 1: i = x + adjsame; j = y - adjsame; break;
                case 2: i = x + adjsame; j = y; break;
                default: i = x + adjsame; j = y + adjsame; break;
            }
            //向棋盘中放入棋子
            temp[x, y] = BLACK;
            temp[i, j] = BLACK;

            //检查关键点
            int result = ForbiddenCheck(i, j);
            //还原棋盘
            temp[i, j] = NONE;
            temp[x, y] = NONE;
            return result;
        }
        public static int ForbiddenCheck(int x, int y)
        {
            int[] adjsame = new int[8];//记录与(x, y)相邻连续黑色棋子数
            int[] adjempty = new int[8];//记录adjsame后相邻连续空子数
            int[] jumpsame = new int[8];//记录adjempty后的连续黑色棋子数
            int[] jumpempty = new int[8];//记录jumpsame后的空位数
            int[] jumpjumpsame = new int[8];//记录jumpempty后的连续黑色棋子数
            //初始化
            for (int i = 0; i < 8; i++)
            {
                adjsame[i] = adjempty[i] = jumpsame[i] = 0;
                jumpempty[i] = jumpjumpsame[i] = 0;
            }
            //将待检测位置(x, y)放入黑色棋子
            temp[x, y] = BLACK;

            //下面进行搜索
            int _x, _y;
            //向上搜索
            for (_y = y - 1; _y >= 0 && temp[x, _y] == BLACK; _y--, adjsame[0]++) ;
            for (; _y >= 0 && temp[x, _y] == NONE; _y--, adjempty[0]++) ;
            for (; _y >= 0 && temp[x, _y] == BLACK; _y--, jumpsame[0]++) ;
            for (; _y >= 0 && temp[x, _y] == NONE; _y--, jumpempty[0]++) ;
            for (; _y >= 0 && temp[x, _y] == BLACK; _y--, jumpjumpsame[0]++) ;

            //右上搜索
            for (_x = x + 1, _y = y - 1; _x < 15 && _y >= 0 && temp[_x, _y] == BLACK; _x++, _y--, adjsame[1]++) ;
            for (; _x < 15 && _y >= 0 && temp[_x, _y] == NONE; _x++, _y--, adjempty[1]++) ;
            for (; _x < 15 && _y >= 0 && temp[_x, _y] == BLACK; _x++, _y--, jumpsame[1]++) ;
            for (; _x < 15 && _y >= 0 && temp[_x, _y] == NONE; _x++, _y--, jumpempty[1]++) ;
            for (; _x < 15 && _y >= 0 && temp[_x, _y] == BLACK; _x++, _y--, jumpjumpsame[1]++) ;

            //向右搜索
            for (_x = x + 1; _x < 15 && temp[_x, y] == BLACK; _x++, adjsame[2]++) ;
            for (; _x < 15 && temp[_x, y] == NONE; _x++, adjempty[2]++) ;
            for (; _x < 15 && temp[_x, y] == BLACK; _x++, jumpsame[2]++) ;
            for (; _x < 15 && temp[_x, y] == NONE; _x++, jumpempty[2]++) ;
            for (; _x < 15 && temp[_x, y] == BLACK; _x++, jumpjumpsame[2]++) ;

            //右下搜索
            for (_x = x + 1, _y = y + 1; _x < 15 && _y < 15 && temp[_x, _y] == BLACK; _x++, _y++, adjsame[3]++) ;
            for (; _x < 15 && _y < 15 && temp[_x, _y] == NONE; _x++, _y++, adjempty[3]++) ;
            for (; _x < 15 && _y < 15 && temp[_x, _y] == BLACK; _x++, _y++, jumpsame[3]++) ;
            for (; _x < 15 && _y < 15 && temp[_x, _y] == NONE; _x++, _y++, jumpempty[3]++) ;
            for (; _x < 15 && _y < 15 && temp[_x, _y] == BLACK; _x++, _y++, jumpjumpsame[3]++) ;

            //向下搜索
            for (_y = y + 1; _y < 15 && temp[x, _y] == BLACK; _y++, adjsame[4]++) ;
            for (; _y < 15 && temp[x, _y] == NONE; _y++, adjempty[4]++) ;
            for (; _y < 15 && temp[x, _y] == BLACK; _y++, jumpsame[4]++) ;
            for (; _y < 15 && temp[x, _y] == NONE; _y++, jumpempty[4]++) ;
            for (; _y < 15 && temp[x, _y] == BLACK; _y++, jumpjumpsame[4]++) ;

            //左下搜索
            for (_x = x - 1, _y = y + 1; _x >= 0 && _y < 15 && temp[_x, _y] == BLACK; _x--, _y++, adjsame[5]++) ;
            for (; _x >= 0 && _y < 15 && temp[_x, _y] == NONE; _x--, _y++, adjempty[5]++) ;
            for (; _x >= 0 && _y < 15 && temp[_x, _y] == BLACK; _x--, _y++, jumpsame[5]++) ;
            for (; _x >= 0 && _y < 15 && temp[_x, _y] == NONE; _x--, _y++, jumpempty[5]++) ;
            for (; _x >= 0 && _y < 15 && temp[_x, _y] == BLACK; _x--, _y++, jumpjumpsame[5]++) ;

            //向左搜索
            for (_x = x - 1; _x < 15 && temp[_x, y] == BLACK; _x--, adjsame[6]++) ;
            for (; _x >= 0 && temp[_x, y] == NONE; _x--, adjempty[6]++) ;
            for (; _x >= 0 && temp[_x, y] == BLACK; _x--, jumpsame[6]++) ;
            for (; _x >= 0 && temp[_x, y] == NONE; _x--, jumpempty[6]++) ;
            for (; _x >= 0 && temp[_x, y] == BLACK; _x--, jumpjumpsame[6]++) ;

            //左上搜索
            for (_x = x - 1, _y = y - 1; _x >= 0 && _y >= 0 && temp[_x, _y] == BLACK; _x--, _y--, adjsame[7]++) ;
            for (; _x >= 0 && _y >= 0 && temp[_x, _y] == NONE; _x--, _y--, adjempty[7]++) ;
            for (; _x >= 0 && _y >= 0 && temp[_x, _y] == BLACK; _x--, _y--, jumpsame[7]++) ;
            for (; _x >= 0 && _y >= 0 && temp[_x, _y] == NONE; _x--, _y--, jumpempty[7]++) ;
            for (; _x >= 0 && _y >= 0 && temp[_x, _y] == BLACK; _x--, _y--, jumpjumpsame[7]++) ;

            //搜索结束，将棋盘还原
            temp[x, y] = NONE;

            //禁手分析
            int threecount = 0, fourcount = 0;//棋型统计数
            for (int i = 0; i < 4; i++)
            {
                if (adjsame[i] + adjsame[i + 4] >= 5)//五子以上相连
                    return LONG_FORBIDDEN;//长连禁手
                else if (adjsame[i] + adjsame[i + 4] == 3)//四子相连
                {
                    //活四冲四判断
                    bool isFour = false;
                    if (adjempty[i] > 0)
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i], i) == NO_FORBIDDEN)
                            isFour = true;
                    }

                    if (adjempty[i + 4] > 0)
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i + 4], i + 4) == NO_FORBIDDEN)
                            isFour = true;
                    }
                    if (isFour)
                        fourcount++;
                }
                else if (adjsame[i] + adjsame[i + 4] == 2) //三子相连
                {
                    //活四、冲四检查
                    if (adjempty[i] == 1 && jumpsame[i] == 1)
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i], i) == NO_FORBIDDEN)
                            fourcount++;
                    }
                    if (adjempty[i + 4] == 1 && jumpsame[i + 4] == 1)
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i + 4], i + 4) == NO_FORBIDDEN)
                            fourcount++;
                    }

                    //活三检查
                    bool isThree = false;

                    if ((adjempty[i] > 2 || adjempty[i] == 2 && jumpsame[i] == 0) && (adjempty[i + 4] > 1 || adjempty[i + 4] == 1 && jumpsame[i + 4] == 0))
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i], i) == NO_FORBIDDEN)
                            isThree = true;
                    }
                    if ((adjempty[i + 4] > 2 || adjempty[i + 4] == 2 && jumpsame[i + 4] == 0) && (adjempty[i] > 1 || adjempty[i] == 1 && jumpsame[i] == 0))
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i + 4], i + 4) == NO_FORBIDDEN)
                            isThree = true;
                    }
                    if (isThree)
                        threecount++;
                }
                else if (adjsame[i] + adjsame[i + 4] == 1)//两子相连
                {
                    //活四冲四判断
                    if (adjempty[i] == 1 && jumpsame[i] == 2)
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i], i) == NO_FORBIDDEN)
                            fourcount++;
                    }
                    if (adjempty[i + 4] == 1 && jumpsame[i + 4] == 2)
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i + 4], i + 4) == NO_FORBIDDEN)
                            fourcount++;
                    }
                    //活三判断
                    if (adjempty[i] == 1 && jumpsame[i] == 1 && (jumpempty[i] > 1 || jumpempty[i] == 1 && jumpjumpsame[i] == 0) &&
                        (adjempty[i + 4] > 1 || adjempty[i + 4] == 1 && jumpsame[i + 4] == 0))
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i], i) == NO_FORBIDDEN)
                            threecount++;
                    }

                    if (adjempty[i + 4] == 1 && jumpsame[i + 4] == 1 && (jumpempty[i + 4] > 1 || jumpempty[i + 4] == 1 && jumpjumpsame[i + 4] == 0) &&
                        (adjempty[i] > 1 || adjempty[i] == 1 && jumpsame[i] == 0))
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i + 4], i + 4) == NO_FORBIDDEN)
                            threecount++;
                    }
                }
                else if (adjsame[i] + adjsame[i + 4] == 0)//单独一子
                {
                    //活四冲四判断
                    if (adjempty[i] == 1 && jumpsame[i] == 3)
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i], i) == NO_FORBIDDEN)
                            fourcount++;
                    }
                    if (adjempty[i + 4] == 1 && jumpsame[i + 4] == 3)
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i + 4], i + 4) == NO_FORBIDDEN)
                            fourcount++;
                    }
                    //活三判断
                    if (adjempty[i] == 1 && jumpsame[i] == 2 && (jumpempty[i] > 1 || jumpempty[i] == 1 && jumpjumpsame[i] == 0) &&
                        (adjempty[i + 4] > 1 || adjempty[i + 4] == 1 && jumpsame[i + 4] == 0))
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i], i) == NO_FORBIDDEN)
                            threecount++;
                    }

                    if (adjempty[i + 4] == 1 && jumpsame[i + 4] == 2 && (jumpempty[i + 4] > 1 || jumpempty[i + 4] == 1 && jumpjumpsame[i + 4] == 0) &&
                        (adjempty[i] > 1 || adjempty[i] == 1 && jumpsame[i] == 0))
                    {
                        if (KeyPointForbiddenCheck(x, y, adjsame[i + 4], i + 4) == NO_FORBIDDEN)
                            threecount++;
                    }
                }
            }
            //禁手分析结束
            if (fourcount > 1)
                return FOUR_FOUR_FORBIDDEN;
            if (threecount > 1)
                return THREE_THREE_FORBIDDEN;

            return NO_FORBIDDEN;
        }

        #endregion


        private void about_Click(object sender, EventArgs e)
        {
            //创建about窗体，并设置初始位置为屏幕居中
            about about_wnd = new about
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            about_wnd.ShowDialog(this);
        }

        private void settings_Click(object sender, EventArgs e)
        {
            //创建settings窗体，并设置初始位置为屏幕居中
            settings settings_wnd = new settings
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            settings_wnd.ShowDialog(this);
        }

        private void AI_Mode_Click(object sender, EventArgs e)
        {
            //将restriction设置提前取出
            FileStream file = new FileStream(@"settings.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader file_read = new StreamReader(file);
            file_read.ReadLine();
            file_read.ReadLine();
            file_read.ReadLine();//跳过3个read
            global::Gobang.settings.setting_color = Convert.ToInt32(file_read.ReadLine());
            file_read.Close();
            file.Close();

            Play.onGame = true;
            Play.mode = 0;
            Locally.Enabled = false;
            Remotely.Enabled = false;
            Continue.Enabled = false;
        }

        private void Locally_Click(object sender, EventArgs e)
        {
            Play.onGame = true;
            Play.mode = 1;
            AI_Mode.Enabled = false;
            Remotely.Enabled = false;
            Continue.Enabled = false;
        }

        private void Remotely_Click(object sender, EventArgs e)
        {
            Play.onGame = false;
            Play.mode = 2;
            AI_Mode.Enabled = false;
            Locally.Enabled = false;
            Continue.Enabled = false;
            btnListen.Enabled = true;
            btnConnect.Enabled = true;
            txblocalip.Enabled = true;
            tbxSendtoIp.Enabled = true;
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            Play.onGame = true;
            Play.mode = 3;
            AI_Mode.Enabled = false;
            Locally.Enabled = false;
            Remotely.Enabled = false;

            //读入游戏数据

            //先读取设置
            FileStream file = new FileStream(@"settings.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader file_read = new StreamReader(file);
            var setting_bgm_temp = file_read.ReadLine();//跳过1个read
            global::Gobang.settings.setting_level = Convert.ToInt32(file_read.ReadLine());
            setting_restriction = Convert.ToInt32(file_read.ReadLine());
            var setting_color_temp = Convert.ToInt32(file_read.ReadLine());
            file_read.Close();
            file.Close();

            //再读取游戏进度文件
            FileStream file_game = new FileStream(@"game.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader file_read_game = new StreamReader(file_game);
            Play.mode = Convert.ToInt32(file_read_game.ReadLine());
            var setting_level_game = Convert.ToInt32(file_read_game.ReadLine());
            file_read_game.ReadLine();//关于Play.player

            //如果发现两个文件里关于level的设置不同，则以残局保存中的为准
            if(global::Gobang.settings.setting_level != setting_level_game)
            {
                //输出到文件
                FileStream file_temp = new FileStream(@"settings.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter file_write = new StreamWriter(file_temp, Encoding.Default);
                file_write.WriteLine(Convert.ToString(setting_bgm_temp));
                file_write.WriteLine(Convert.ToString(setting_level_game));
                file_write.WriteLine(Convert.ToString(setting_restriction));
                file_write.WriteLine(Convert.ToString(setting_color_temp));
                file_write.Close();
                file_temp.Close();
            }
            int[] black_pieces = new int[113];
            int[] white_pieces = new int[113];
            int k = 0, m = 0;
            for (int i = 0; i <= 14; i++)
                for (int j = 0; j <= 14; j++)
                {
                    var temp = Convert.ToInt32(file_read_game.ReadLine());
                    if (temp == 1)
                    {
                        black_pieces[k] = 15 * i + j;
                        k++;
                    }
                    else if(temp == 2)
                    {
                        white_pieces[m] = 15 * i + j;
                        m++;
                    }
                }
            file_read_game.Close();
            file_game.Close();
            //有k个黑棋，m个白棋，其中k>=m恒成立（即要么k=m，要么k=m+1）
            for(int i = 0; i < m; i++)
            {
                GameOn_NetWork(black_pieces[i] / 15, black_pieces[i] % 15);
                GameOn_NetWork(white_pieces[i] / 15, white_pieces[i] % 15);
            }
            if (k > m)
                GameOn_NetWork(black_pieces[k - 1] / 15, black_pieces[k - 1] % 15);
        }

        private void MainWnd_Load(object sender, EventArgs e)
        {
            Initialize();
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            txblocalip.Text = AddressIP;
            //初始化分数字典
            //5个连子
            Play.dict.Add("22222", 122222);
            Play.dict_black.Add("11111", 122222);
            //4个连子
            Play.dict.Add("022220", 12222);
            Play.dict.Add("122220", 5222);
            Play.dict.Add("20222", 5222);
            Play.dict.Add("22022", 5222);
            Play.dict_black.Add("011110", 12222);
            Play.dict_black.Add("211110", 5222);
            Play.dict_black.Add("10111", 5222);
            Play.dict_black.Add("11011", 5222);
            //3个连子
            Play.dict.Add("02220", 2522);
            Play.dict.Add("122200", 1222);
            Play.dict.Add("120220", 1222);
            Play.dict.Add("020221", 1222);
            Play.dict.Add("122020", 1222);
            Play.dict.Add("022021", 1222);
            Play.dict_black.Add("01110", 2522);
            Play.dict_black.Add("211100", 1222);
            Play.dict_black.Add("210110", 1222);
            Play.dict_black.Add("010112", 1222);
            Play.dict_black.Add("211010", 1222);
            Play.dict_black.Add("011012", 1222);
            //2个连子
            Play.dict.Add("00220", 522);
            Play.dict.Add("122000", 522);
            Play.dict.Add("120200", 322);
            Play.dict_black.Add("00110", 522);
            Play.dict_black.Add("211000", 522);
            Play.dict_black.Add("210100", 322);
            //1个子
            Play.dict.Add("211112", 52222);
            Play.dict.Add("11121", 52222);
            Play.dict.Add("11211", 52222);
            Play.dict.Add("21110", 4222);
            Play.dict.Add("12110", 4222);
            Play.dict.Add("11210", 4222);
            Play.dict.Add("21100", 822);
            Play.dict.Add("12100", 822);
            Play.dict.Add("11200", 822);
            Play.dict.Add("21000", 222);
            Play.dict.Add("12000", 222);
            Play.dict_black.Add("122221", 52222);
            Play.dict_black.Add("22212", 52222);
            Play.dict_black.Add("22122", 52222);
            Play.dict_black.Add("12220", 4222);
            Play.dict_black.Add("21220", 4222);
            Play.dict_black.Add("22120", 4222);
            Play.dict_black.Add("11200", 822);
            Play.dict_black.Add("21200", 822);
            Play.dict_black.Add("22100", 822);
            Play.dict_black.Add("12000", 222);
            Play.dict_black.Add("21000", 222);
            //无连子
            Play.dict.Add("00000", 0);
            Play.dict_black.Add("00000", 0);
        }
        
        /// <summary>
        /// 关闭时保存进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWnd_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (Play.mode != 2 && !game_over)
            {
                FileStream file = new FileStream(@"game.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter file_write = new StreamWriter(file);
                file_write.WriteLine(Convert.ToString(Play.mode));
                file_write.WriteLine(Convert.ToString(global::Gobang.settings.setting_level));
                file_write.WriteLine(Convert.ToString(Play.player));
                for (int i = 0; i <= 14; i++)
                    for (int j = 0; j <= 14; j++)
                        file_write.WriteLine(Convert.ToString(ChessBack[i, j]));
                file_write.Close();
                file.Close();
            }
        }

        #region 网络相关
        private void txblocalip_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void tbxlocalPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void lstbxMessageView_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 判断监听按钮是否被点击，如果被点击，新建线程，启动listen函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListen_Click(object sender, EventArgs e)
        {
            Play.player = true;
            Play.sender = true;
            tbxMessageSend.Enabled = true;
            lstbxMessageView.Enabled = true;
            btnSend.Enabled = true;

            //将restriction设置提前取出
            FileStream file = new FileStream(@"settings.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader file_read = new StreamReader(file);
            file_read.ReadLine();
            file_read.ReadLine();//跳过两个read
            setting_restriction = Convert.ToInt32(file_read.ReadLine());
            file_read.Close();
            file.Close();

            Thread listenThread = new Thread(new ThreadStart(listen));
            listenThread.Start();
        }
        
        /// <summary>
        /// 监听函数，用作服务器；异常处理是连接失败或使用了与IPv4协议不匹配的IP地址等等
        /// </summary>
        private void listen()
        {
            try
            {
                Thread.CurrentThread.IsBackground = true;
                Control.CheckForIllegalCrossThreadCalls = false;
                btnConnect.Enabled = false;
                btnListen.Enabled = false;
                tbxSendtoIp.Enabled = false;
                CI.listen();
                Play.onGame = true;   //连接后才允许落子。
                CI.OnReceiveMsg += new ChessEventHander(manageChessEvent);
            }
            catch (Exception ex)
            {
                lstbxMessageView.Text += "listen:\r\n" + ex.Message + "\r\n";
            }
        }

        /// <summary>
        /// 判断连接按钮是否被按下：如果按下，按照目的IP框中的地址，发起连接请求；
        /// 调用的connect函数新建线程，启动receiveMsg函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                CI.connect(tbxSendtoIp.Text);
                Play.onGame = true;
                //Play.player = false;
                Play.sender = false;
                CI.OnReceiveMsg += new ChessEventHander(manageChessEvent);
                btnConnect.Enabled = false;
                btnListen.Enabled = false;
                tbxSendtoIp.Enabled = false;
                tbxMessageSend.Enabled = true;
                lstbxMessageView.Enabled = true;
                btnSend.Enabled = true;
            }
            catch (Exception ex)
            {
                tbxSendtoIp.Text += "btn_connect_Click:\r\n" + ex.Message + "\r\n";
            }
        }
        
        /// <summary>
        /// 判断发送按钮是否被按下；如果按下，则发送一条信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string player;
                if (Play.sender == true)
                    player = "黑子玩家：\r\n";
                else
                    player = "白子玩家：\r\n";
                CI.sendMsg(1, 0, player + tbxMessageSend.Text);
                lstbxMessageView.Text += player + tbxMessageSend.Text + "\r\n";
                tbxMessageSend.Text = "";
            }
            catch (Exception ex)
            {
                lstbxMessageView.Text += "btnSendMs_Click:\r\n" + ex.Message + "\r\n";
            }
        }

        /// <summary>
        /// 处理接收信息事件的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">自定义棋子事件（即接收的信息）</param>
        public void manageChessEvent(object sender, ChessEvent e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            switch (e.Iclass)
            {
                case "0":   //棋子信息
                    {
                        int _x, _y;
                        _x = int.Parse(e.content) % 15;
                        _y = (int)(int.Parse(e.content) / 15);
                        setting_restriction = int.Parse(e.flag);
                        Play.onGame = true;
                        GameOn_NetWork(_x, _y);
                        break;
                    }
                case "1":   //聊天
                    lstbxMessageView.Text += e.content + "\r\n";
                    break;
                case "2":   //不解析restriction字段
                    {
                        int _x, _y;
                        _x = int.Parse(e.content) % 15;
                        _y = (int)(int.Parse(e.content) / 15);
                        Play.onGame = true;
                        GameOn_NetWork(_x, _y);
                        break;
                    }
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            //创建help窗体，并设置初始位置为屏幕居中
            help help_wnd = new help
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            help_wnd.ShowDialog(this);
        }
    }
}

/// <summary>
/// 自定义一个事件，该事件继承自预设类ChessEvent
/// </summary>
public class ChessEvent : EventArgs
{
    public string Iclass;
    public string content;
    public string flag;
    public ChessEvent(string _class, string _flag, string _content)
    {
        Iclass = _class;
        content = _content;
        flag = _flag;
    }
}

/// <summary>
/// 委托，用于处理ChessEvent
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public delegate void ChessEventHander(object sender, ChessEvent e);

/// <summary>
/// 联网对战接口
/// </summary>
public interface ISocket
{
    void listen();
    void connect(string ipStr);
    void sendMsg(byte @class, byte flag, string content);
    void receiveMsg(object obj);
    void close();
}

/// <summary>
/// 联网对战类，包含listen、connect、sendMsg、receiveMsg和close函数
/// </summary>
public class ControlInternet : ISocket
{
    private Socket skRec = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private IPEndPoint ipeRec;
    private Socket skSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private IPEndPoint ipeSend;
    public event ChessEventHander OnReceiveMsg;
    public ControlInternet()
    {

    }

    public void listen()
    {
        try
        {
            IPAddress[] ip_list = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            IPAddress myIp = ip_list[ip_list.Length - 1];//获取IP地址
            skRec.Bind(new IPEndPoint(IPAddress.Parse(myIp.ToString()), 8880));
            skRec.Listen(0);
            Socket clientRec = skRec.Accept();
            Thread receiveMsgThread = new Thread(new ParameterizedThreadStart(receiveMsg));
            receiveMsgThread.Start(clientRec);
            skSend.Bind(new IPEndPoint(IPAddress.Parse(myIp.ToString()), 8881));
            skSend.Listen(0);
            skSend = skSend.Accept();
        }
        catch (Exception ex)
        {
            MessageBox.Show("listen:" + ex.Message);
        }
    }

    public void connect(string ipStr)
    {
        ipeSend = new IPEndPoint(IPAddress.Parse(ipStr), 8880);
        skSend.Connect(ipeSend);
        ipeRec = new IPEndPoint(IPAddress.Parse(ipStr), 8881);
        skRec.Connect(ipeRec);
        Thread receiveMsgThread = new Thread(new ParameterizedThreadStart(receiveMsg));
        receiveMsgThread.Start(skRec);
    }

    public void sendMsg(byte @class, byte flag, string content)
    {
        try
        {
            byte[] tmpBytes = Encoding.Default.GetBytes(content);
            MessageControl.Message msg = new MessageControl.Message(@class, flag, tmpBytes);
            byte[] sendeBytes = msg.ToBytes();
            skSend.Send(sendeBytes);
        }
        catch (Exception ex)
        {

        }
    }

    public void receiveMsg(object obj)
    {
        Thread.CurrentThread.IsBackground = true;
        Socket clientRec = (Socket)obj;
        MessageControl.Message msg = new MessageControl.Message();
        MessageControl.MessageStream mst = new MessageControl.MessageStream();
        int revb;
        try
        {
            while (clientRec.Connected)
            {
                byte[] recBytes = new byte[512];
                revb = clientRec.Receive(recBytes);
                mst.Write(recBytes, 0, revb);
                if (mst.Read(out msg))
                {
                    OnReceiveMsg(this, new ChessEvent(msg.Class.ToString(), msg.Flag.ToString(), Encoding.Default.GetString(msg.Content)));
                }
            }
        }
        catch (Exception ex)
        {
            //skRec.Close();
            //skSend.Close();
            //clientRec.Close();
        }
    }

    public void close()
    {
        skRec.Close();
        skSend.Close();
        //    //skRec = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    //skSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    ~ControlInternet()
    {
        skRec.Close();
        skSend.Close();
    }
}
#endregion