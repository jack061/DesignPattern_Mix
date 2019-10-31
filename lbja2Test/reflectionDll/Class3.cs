using interfaceDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reflectionDll
{
    public class Class3 : IEditor
    {
        public string Name
        {
            get { return "丽江"; }
        }

        public void Run(System.Windows.Forms.TextBox textbox)
        {
            MessageBox.Show("这是一场没有结局的表演,你的电脑内存已满，请及时清理！！！");
            textbox.Text = "丽江的樱桃花正开，丽江的春让你解开了腰带！";
             
        }
    }
}
