using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Gobang
{
    [ToolboxItem(true)]
    public partial class CheckButton : UserControl
    {
        public bool isCheck;
        public int isChanged = 0;
        public CheckButton()
        {
            InitializeComponent();
            //设置Style支持透明背景色并且双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;

            this.Cursor = Cursors.Hand;
            this.Size = new Size(87, 27);
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked
        {
            set { isCheck = value; this.Invalidate(); }
            get { return isCheck; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bitMapOn = Properties.Resources.btncheckon;
            Bitmap bitMapOff = Properties.Resources.btncheckoff;
            Graphics g = e.Graphics;
            Rectangle rec = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            if (isCheck)
            {
                g.DrawImage(bitMapOn, rec);
            }
            else
            {
                g.DrawImage(bitMapOff, rec);
            }
        }
        private void CheckButton_Click(object sender, EventArgs e)
        {
            isChanged++;
            isCheck = !isCheck;
            this.Invalidate();
        }
    }
}