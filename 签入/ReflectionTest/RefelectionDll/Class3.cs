using IEditorInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefelectionDll
{
    public class Class3 : IEditor
    {
        public string Name
        {
            get
            {
                return "戒色";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Run(System.Windows.Forms.TextBox text)
        {
            text.Text = "这是一场永远也不会结束的战争,never never give up!!!";
        }
    }
}
