using interfaceDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace reflectionDll
{
    public class Class1:IEditor
    {
        public string Name
        {
            get { return "成都"; }
        }

        public void Run(System.Windows.Forms.TextBox textbox)
        {
         
            textbox.Text = "陪我到成都的街头走一走，直到所有的灯都熄灭了也不停留";
        }
    }
    public class Class2 : IEditor
    {
        public string Name
        {
            get { return "大理"; }
        }

        public void Run(System.Windows.Forms.TextBox textbox)
        {

            textbox.Text = "不如一路向西去大理";
        }
    }
   
}
