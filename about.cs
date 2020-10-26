using System;
using System.Windows.Forms;

namespace Gobang
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        private void about_Load(object sender, EventArgs e)
        {
            MainWnd mainwnd = (MainWnd)this.Owner;//将主窗体设置为子窗体的父窗体
        }
    }
}