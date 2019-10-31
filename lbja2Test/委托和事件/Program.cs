using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 委托和事件
{
    class Program
    {
        public delegate void testDelegate(string name);
        
        static void Main(string[] args)
        {
            manager mg = new manager();
            mg.testEvent += EnglishGreeting;
            mg.testEvent += ChineseGreeting;
            mg.GreetPeople("燕七");

        }
        public static void EnglishGreeting(string name)
        {
            Console.WriteLine("Morning, " + name);
            Console.ReadKey();
        }
        public static void ChineseGreeting(string name)
        {
            Console.WriteLine("早上好, " + name);
            Console.ReadKey();
        }
    }
    class manager
    {
        public event Action<string> testEvent;
        public  void GreetPeople(string name)
        {
            testEvent(name);
        }
    }
}
