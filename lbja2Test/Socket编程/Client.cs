using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Socket编程
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string ip = this.textBox1.Text;
            string point = this.textBox2.Text;
            sk.Connect(ip, int.Parse(point));
            string msg = this.textBox3.Text;
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            int r = sk.Send(buffer);
            if (r > 0)
            {
                this.Text = "发送成功";
            }
        }
    }
}
