using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 闭包测试
{
    class Program
    {
        static void Main(string[] args)
        {
            int copy;
            List<Action> actions = new List<Action>();
            for (int counter = 0; counter < 10; counter++)
            {
                copy = counter;
                actions.Add(() => Console.WriteLine(copy));
            }
            foreach (Action action in actions)
            {
                action();
            }
            Console.ReadKey();
        }
    }
}
