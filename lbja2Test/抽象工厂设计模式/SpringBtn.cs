using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 抽象工厂设计模式
{
    class SpringBtn:IButton
    {
        public void Display()
        {
            Console.WriteLine("springBtn点击");
        }
    }
}
