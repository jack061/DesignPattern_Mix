using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 构造函数问题
{
    class Program
    {
        static void Main(string[] args)
        {
            //ctor ct = new ctor("燕七");
            //Console.WriteLine(ct.Name);
            //Console.WriteLine(ct.Address);
            //Console.WriteLine(ct.Age);
            ctor[] ctArray = new ctor[2] { new ctor("郭大路"),new ctor("王动") };
            for (int i = 0; i < ctArray.Length; i++)
            {
                Console.WriteLine(ctArray[i].Name);
            }
            Console.ReadKey();
        }
    }
}
