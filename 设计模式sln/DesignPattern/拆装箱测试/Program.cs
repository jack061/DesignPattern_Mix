using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 拆装箱测试
{
   public  class Program
    {
        static void Main(string[] args)
        {
            #region old
            //string err = "ss";
            //OutTest.getOutArgs(ref err);
            //Console.WriteLine(err); 
            #endregion
            string str = "郭大路";
           str= str.getStr();
            Console.WriteLine(str);
            Console.ReadKey();
        }
    }
   public static class OutTest
    {
        public  static void getOutArgs(ref string err)
        {
            Console.WriteLine(err);
        }
        public static string getStr(this string s)
        {
            return "燕七";
        }
    }

   
   
}
