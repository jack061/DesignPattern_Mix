using IEditorInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefelectionDll
{
    public class Class1:IEditor
    {
        public string Name
        {
            get
            {
                return "成都";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Run(System.Windows.Forms.TextBox text)
        {
            text.Text = "陪我到成都的街头走一走，直到所有的灯都熄灭了也不停留。你会挽着我的衣袖，我会把手揣进裤兜。走到玉林路的尽头，走过小酒馆的门口。分别总是在九月，回忆是思念的愁。深秋嫩绿的垂柳，亲吻着我的额头。";
        }



        
    }
}
