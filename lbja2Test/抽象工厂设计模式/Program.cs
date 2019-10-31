using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 抽象工厂设计模式
{
    class Program
    {
        static void Main(string[] args)
        {
            ISkinFactory factory = (ISkinFactory)configHelper.GetSkinFactoryInstance();
            IButton btm = factory.CreateButton();
            btm.Display();
            Console.ReadKey();
        }
    }
}
