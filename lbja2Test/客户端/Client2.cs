using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 客户端
{
    public partial class Client2 : Form
    {
        public Client2()
        {
            InitializeComponent();
        }
        Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private void button1_Click(object sender, EventArgs e)
        {
            string ip = this.textBox1.Text;
            string point = this.textBox2.Text;
            sk.Connect(IPAddress.Parse(ip), int.Parse(point));
            this.listBox1.Items.Add("连接成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string msg = this.textBox3.Text;
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            int r = sk.Send(buffer);
            if (r > 0)
            {
                this.listBox1.Items.Add("发送成功");
            }
        }
    }
}
