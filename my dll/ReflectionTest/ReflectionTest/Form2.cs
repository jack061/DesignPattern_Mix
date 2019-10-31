using IEditorInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReflectionTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //获取文件夹路径
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Addons");
            //获取路径下的所有dll文件
            string[] dlls = Directory.GetFiles(path, "*.dll");
            //遍历所有的dll文件，获取其type
            foreach (var dll in dlls)
            {
                Assembly ass = Assembly.LoadFile(dll);
                Type[] types = ass.GetExportedTypes();
                //获取接口type，判断该dll文件是否实现其接口
                Type IType = typeof(IEditor);
                foreach (var type in types)
                {
                    if (IType.IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        IEditor etor = (IEditor)Activator.CreateInstance(type);
                        ToolStripItem item = tools.DropDownItems.Add(etor.Name);
                        item.Click += item_Click;
                        item.Tag = etor;
                    }
                }
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item!=null)
            {
                IEditor etor = item.Tag as IEditor;
                if (etor!=null)
                {
                    etor.Run(this.textBox1);
                }
            }
            
            
        }

    
    }
}
