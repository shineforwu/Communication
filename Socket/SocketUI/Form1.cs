using SocketCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketUI
{
    public partial class Form1 : Form
    {
        SocketServer S = new SocketServer();
        SocketClient c = new SocketClient();
        public delegate void delegateStr(string mes);
        public event delegateStr eventStr;
        public Form1()
        {
            InitializeComponent();
            S.Show += Get;
            eventStr += Get;
        }
        void Get(string mess)
        {
            if (!lb_list.InvokeRequired)
            {
                lb_list.Items.Insert(0, mess);
            }
            else
            {
                lb_list.Invoke(eventStr, mess);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(S.Start));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            c.SendMes(DateTime.Now.ToString());
            c.SendMes("Hi");
        }
    }
}
