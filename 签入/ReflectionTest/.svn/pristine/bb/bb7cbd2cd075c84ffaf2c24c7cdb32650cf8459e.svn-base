using IEditorInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefelectionDll
{
  public class ChangeColor:IEditor
    {
        public string Name
        {
            get
            {
                return "转换大写";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Run(System.Windows.Forms.TextBox text)
        {
           text.Text= text.Text.ToUpper();
        }
    }
}
