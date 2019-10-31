using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IEditorInterface
{
    public interface IEditor
    {
         string Name { get; set; }
         void Run(TextBox text);
    }
}
