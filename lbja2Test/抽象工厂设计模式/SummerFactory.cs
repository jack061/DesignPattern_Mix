using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 抽象工厂设计模式
{
    class SummerFactory:ISkinFactory
    {
        public IButton CreateButton()
        {
            return new SummerBtn();
        }

    }
}
