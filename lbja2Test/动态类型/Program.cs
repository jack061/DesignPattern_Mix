
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 动态类型
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic i;
            i = 10;
            i = "曾梦想仗剑走天涯";
            i=new string[]{"ss","sdfs"};
            Console.WriteLine(i);
            Console.ReadKey();
        }
    }
}
