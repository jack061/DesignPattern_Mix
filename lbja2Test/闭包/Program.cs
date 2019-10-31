using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 闭包
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 第一种闭包
            //List<Action> actions = new List<Action>();
            //for (int counter = 0; counter < 10; counter++)
            //{
            //    int copy;
            //    copy = counter;
            //    actions.Add(() => { Console.WriteLine(copy); });

            //}
            //foreach (Action action in actions)
            //{
            //    action();
            //} 
            #endregion

            #region 第二种闭包
            //List<Action> actions = new List<Action>();
            //for (int counter = 0; counter < 10; counter++)
            //{
            //    TempClass tc = new TempClass();
            //    tc.copy = counter;
            //    actions.Add(tc.TempMethod);
            //}
            //foreach (Action action in actions)
            //{
            //    action();
            //}
            #endregion

            #region 第三种闭包
            List<Action> actions = new List<Action>();
            for (int counter = 0; counter < 10; counter++)
            {
                actions.Add(() => Console.WriteLine(counter));
            }
            foreach (Action action in actions)
            {
                action();
            }
            #endregion

            #region 第四种闭包
            //List<Action> actions = new List<Action>();
            //TempClass tc = new TempClass();
            //for ( tc.copy = 0; tc.copy < 10; tc.copy++)
            //{
            //    actions.Add(tc.TempMethod);
            //}
            //foreach (var action in actions)
            //{
            //    action();
            //}
            #endregion
            Console.ReadKey();
        }
    }
    class TempClass
    {
        public int copy;
        public void TempMethod()
        {
            Console.WriteLine(copy);
        }
    }
}
