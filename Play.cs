using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Linq;
using System;

namespace Gobang
{
    abstract public class Piece:PictureBox
    {
        public Piece(int x, int y)
        {
            this.BackColor = Color.Transparent;
            this.Location = new Point(x, y);
            this.Size = new Size(50, 50);
        }
    }
    public class BlackPiece:Piece
    {
        public BlackPiece(int x, int y): base(x, y)
        {
            this.Image = Properties.Resources.black;
        }
    }
    public class WhitePiece : Piece
    {
        public WhitePiece(int x, int y) : base(x, y)
        {
            this.Image = Properties.Resources.white;
        }
    }
    abstract class Play
    {
        public static bool onGame = false;
        public static int mode = -1;//初始为-1，为0时人机对战；为1时双人离线；为2时双人在线；为3时残局保存
        public static bool player = true;//true为黑，false为白
        public static int bX;
        public static int bY;
        public static bool sender = false;
        public static bool start = true;
        public static Dictionary<string, int> dict = new Dictionary<string, int>();//人机执白棋时，存储枚举分数，对不同的棋型进行估计；前面是棋型，后面是分值
        public static Dictionary<string, int> dict_black = new Dictionary<string, int>();//人机执黑棋时，存储枚举分数，对不同的棋型进行估计；前面是棋型，后面是分值

        #region AI相关
        public static Point AI_Data_Process()
        {
            Point point = new Point(0, 0);
            if (global::Gobang.settings.setting_level == 0)
                point = easy();
            else if (global::Gobang.settings.setting_level == 1)
                point = medium();
            return point;
        }
        public static Point easy()//入门计算：找与前一个点最近的点，并避免禁手
        {
            Point point = new Point(0, 0);
            int CurrentColor;//1代表黑色，2代表白色；与棋盘一致
            //先把棋盘复制一下
            int[,] temp = new int[15, 15];
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    temp[i, j] = MainWnd.ChessBack[i, j];
            if (global::Gobang.settings.setting_color == 1&&start)//玩家执白棋，此时AI先行，这里默认放到中间
            {
                point.X = 17 + 53 * 7; point.Y = 17 + 51 * 7;
                start = !start;
                return point;
            }
            double[,] distance = new double[15, 15];
            //搜索
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    MainWnd.temp[i, j] = MainWnd.ChessBack[i, j];
            for (int i = 0; i <= 14; i++)
                for (int j = 0; j <= 14; j++)
                {
                    if (temp[i, j] == 0)
                    {
                        if (global::Gobang.settings.setting_color == 1 && i!= 0 && j!=0 & MainWnd.ForbiddenCheck(i, j) != 0)
                            distance[i, j] = 100;
                        else
                            distance[i, j] = System.Math.Sqrt((i - bX) * (i - bX) + (j - bY) * (j - bY));
                    }
                    else
                        distance[i, j] = 100;
                }
            double minimum = 50;
            for (int i = 0; i <= 14; i++)
                for (int j = 0; j <= 14; j++)
                {
                    if (minimum >= distance[i, j])
                    {
                        minimum = distance[i, j];
                        point.X = i;
                        point.Y = j;
                    }
                }
            point.X = point.X * 53 + 17; point.Y = point.Y * 51 + 17;
            return point;
        }

        public static Point medium()//思路：根据空格，对一个可走的空位子进行打分；
                //如果AI落在这个空位置的分数越高，说明这个位置就越好，
                //每次ai走棋就找到一个最好的空位置就行了。
                //在计算分数时，分四个方向找，四个方向上的分数累加
                //在搜索时，只使用进行索引，最后转为相对坐标
        {
            Point point = new Point(0, 0);
            if (global::Gobang.settings.setting_color == 1 && start)//玩家执白棋，此时AI先行，这里默认放到中间
            {
                point.X = 17 + 53 * 7; point.Y = 17 + 51 * 7;
                start = !start;
                return point;
            }
            var dict_score = new Dictionary<Point, int>();
            for(int i = 0; i <= 14; i++)
                for(int j = 0; j <= 14; j++)
                {
                    if(global::Gobang.MainWnd.ChessBack[i,j] == 0)//无子
                    {
                        Point pos = new Point(i, j);
                        dict_score.Add(pos, Get_point_score(pos));
                    }
                }
            var temp_dict_score = dict_score.OrderByDescending(o => o.Value).ToDictionary(p => p.Key, o => o.Value);
            point = temp_dict_score.Keys.First();
            //最后对索引进行处理
            point.X = point.X * 53 + 17; point.Y = point.Y * 51 + 17;
            return point;
        }
        private static string Get_Hor_Point_Str(Point point)//水平
        {
            int head_x = point.X, trail_x = point.X, head_count = 0, trail_count = 0;
            while (head_count < 5 && head_x - 1 > 0)
            {
                head_count++;
                head_x--;
            }
            while (trail_count < 5 && trail_x + 1 < 13)
            {
                trail_count++;
                trail_x++;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = point.X - head_count; i <= point.X + trail_count; i++)
                sb.Append(global::Gobang.MainWnd.ChessBack[i, point.Y].ToString());
            return sb.ToString();
        }
        private static string Get_Ver_Point_Str(Point point)//竖直
        {
            int head_y = point.Y, trail_y = point.Y, head_count = 0, trail_count = 0;
            while (head_count < 5 && head_y - 1 > 0)
            {
                head_count++;
                head_y--;
            }
            while (trail_count < 5 && trail_y + 1 < 13)
            {
                trail_count++;
                trail_y++;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = point.Y - head_count; i <= point.Y + trail_count; i++)
                sb.Append(global::Gobang.MainWnd.ChessBack[point.X, i].ToString());
            return sb.ToString();
        }
        private static string Get_Leftup_Point_Str(Point point)//左上
        {
            int head_x = point.X, head_y = point.Y, trail_x = point.X, trail_y = point.Y, head_count = 0, trail_count = 0;
            while (head_count < 5 && head_y - 1 > 0 && head_x - 1 > 0)
            {
                head_count++;
                head_y--;
                head_x--;
            }
            while (trail_count < 5 && trail_y + 1 < 13 && trail_x + 1 < 13)
            {
                trail_count++;
                trail_y++;
                trail_x++;
            }
            var step = point.X - head_count;
            StringBuilder sb = new StringBuilder();
            for (int i = point.Y - head_count; i <= point.Y + trail_count; i++)
            {
                sb.Append(global::Gobang.MainWnd.ChessBack[step, i].ToString());
                step++;
            }
            return sb.ToString();
        }
        private static string Get_Rightup_Point_Str(Point point)//右上
        {
            int head_x = point.X, head_y = point.Y, trail_x = point.X, trail_y = point.Y, head_count = 0, trail_count = 0;
            while (head_count < 5 && head_y + 1 < 13 && head_x - 1 > 0)
            {
                head_count++;
                head_y++;
                head_x--;
            }
            while (trail_count < 5 && trail_y - 1 > 0 && trail_x + 1 < 13)
            {
                trail_count++;
                trail_y--;
                trail_x++;
            }
            var step = point.Y + head_count;
            StringBuilder sb = new StringBuilder();
            for (int i = point.X - head_count; i <= point.X + trail_count; i++)
            {
                sb.Append(global::Gobang.MainWnd.ChessBack[i, step].ToString());
                step--;
            }
            return sb.ToString();
        }
        private static int Get_point_score(Point point)
        {
            int value = 0;
            string left_up = Get_Leftup_Point_Str(point);
            string right_up = Get_Rightup_Point_Str(point);
            string hor = Get_Hor_Point_Str(point);
            string ver = Get_Ver_Point_Str(point);
            if (global::Gobang.settings.setting_color == 0)//玩家执黑子，人机执白子
            {
                foreach (KeyValuePair<string, int> pair in dict)
                {
                    Array temp_1;
                    temp_1 = pair.Key.ToArray();
                    Array.Reverse(temp_1);
                    string temp = temp_1.ToString();
                    if (left_up.Contains(pair.Key) || left_up.Contains(temp))
                        value += pair.Value;
                    if (right_up.Contains(pair.Key) || right_up.Contains(temp))
                        value += pair.Value;
                    if (hor.Contains(pair.Key) || hor.Contains(temp))
                        value += pair.Value;
                    if (ver.Contains(pair.Key) || ver.Contains(temp))
                        value += pair.Value;
                    if (point.X != 0 && point.Y != 0 && global::Gobang.MainWnd.ForbiddenCheck(point.X, point.Y) != 0)
                        value = -100000;
                }
            }
            else//玩家执白子，人机执黑子
            {
                foreach (KeyValuePair<string, int> pair in dict_black)
                {
                    Array temp_1;
                    temp_1 = pair.Key.ToArray();
                    Array.Reverse(temp_1);
                    string temp = temp_1.ToString();
                    if (left_up.Contains(pair.Key) || left_up.Contains(temp))
                        value += pair.Value;
                    if (right_up.Contains(pair.Key) || right_up.Contains(temp))
                        value += pair.Value;
                    if (hor.Contains(pair.Key) || hor.Contains(temp))
                        value += pair.Value;
                    if (ver.Contains(pair.Key) || ver.Contains(temp))
                        value += pair.Value;
                    if (point.X != 0 && point.Y != 0 && global::Gobang.MainWnd.ForbiddenCheck(point.X, point.Y) != 0)
                        value = -100000;
                }
            }
            return value;
        }
        #endregion
    }
}