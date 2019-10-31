using interfaceDll;
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

namespace reflectionTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region old
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Addons");
            //string[] dlls = Directory.GetFiles(path,"*.dll");
            //foreach (var dll in dlls)
            //{
            //   Assembly ass = Assembly.LoadFile(dll);
            //   Type[] types = ass.GetExportedTypes();
            //   Type IType = typeof(IEditor);
            //   foreach (var type in types)
            //   {
            //       if (IType.IsAssignableFrom(type)&&!type.IsAbstract)
            //       {
            //           IEditor etor = (IEditor)Activator.CreateInstance(type);
            //           tools.DropDownItems.Add(etor.Name);
            //           tools.Click += new EventHandler(tools_Click);
            //           tools.Tag = etor;
            //       }
            //   }
            //}

            #endregion

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Addons");
            string[] dlls = Directory.GetFiles("*.dll", path);
            foreach (var dll in dlls)
            {
                Assembly ass = Assembly.LoadFile(dll);
                Type[] types = ass.GetExportedTypes();
                Type Itype = typeof(IEditor);
                foreach (var type in types)
                {
                    if (Itype.IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        IEditor etor =(IEditor)Activator.CreateInstance(type);
                        ToolStripItem item = tools.DropDownItems.Add(etor.Name);
                        item.Tag = etor;
                        item.Click+=s;
                    }
                }

            }
        }
        void items_Click(object sender, EventArgs e)
        {
            ToolStripItem tools = sender as ToolStripItem;
            IEditor etor = tools.Tag as IEditor;
            if (etor != null)
            {

                etor.Run(this.textBox1);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            ToolStripItem tools = sender as ToolStripItem;
            IEditor etor = tools.Tag as IEditor;
            if (etor != null)
            {
                etor.Run(this.textBox1);
            }
        }

        void tools_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tools = sender as ToolStripItem;
            IEditor etor = tools.Tag as IEditor;
            if (etor != null)
            {
                etor.Run(this.textBox1);
            }
        }

        private void tools_click2(object sender, EventArgs e)
        {
            ToolStripItem tools = sender as ToolStripItem;
            IEditor etor = tools.Tag as IEditor;
            if (etor != null)
            {
                etor.Run(this.textBox1);
            }
        }
        void tools_Click(object sender, EventArgs e)
        {
            ToolStripItem tools = sender as ToolStripItem;
            IEditor etor = tools.Tag as IEditor;
            if (etor != null)
            {
                etor.Run(this.textBox1);
            }
            //ToolStripItem tools = sender as ToolStripItem;
            //IEditor etor = tools.Tag as IEditor;
            //if (etor != null)
            //{
            //    etor.Run(this.textBox1);
            //}
        }


    }
}
