using IEditorInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefelectionDll
{
   public class Class2:IEditor
    {
        public string Name
        {
            get
            {
                return "丽江";
            }
            set
            {
                throw new NotImplementedException();
            }
     
        }

        public void Run(System.Windows.Forms.TextBox text)
        {
            text.Text = "再也不会去丽江，再也不会走在那路上";
        }

      
    }
}
