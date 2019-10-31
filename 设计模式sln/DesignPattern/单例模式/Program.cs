using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 单例模式
{
    class Program
    {
        static void Main(string[] args)
        {
            Singleton s1 = Singleton.GetInstance();
            Singleton2 s2 = Singleton2.GetInstance();
            Console.WriteLine(s1.GetType().Name);
            Console.WriteLine(s2.GetType().Name);
            Console.ReadKey();
        }
    }

    #region 懒汉式单例
    class Singleton
    {
        private static Singleton instance;
        private static readonly object sync = new object();
        private Singleton()
        {

        }
        public static Singleton GetInstance()
        {
            if (instance == null)
            {
                lock (sync)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                }
            }
            return instance;
        }
    }
    //class Singleton
    //{
    //    private static Singleton instance;
    //    private static readonly object sync = new object();
    //    private Singleton()
    //    {

    //    }
    //    public static Singleton GetInstance()
    //    {
    //        if (instance == null)
    //        {
    //            lock (sync)
    //            {
    //                if (instance == null)
    //                {
    //                    instance = new Singleton();
    //                }
    //            }
    //        }
    //        return instance;
    //    }
    //}
    #endregion

    #region 饿汉式单例
    public class Singleton2
    {
        private static readonly Singleton2 instance = new Singleton2();
        private Singleton2()
        {

        }
        public static Singleton2 GetInstance()
        {
            return instance;
        }
    }
    //class Singleton2
    //{
    //    private static readonly Singleton2 instance = new Singleton2();
    //    private Singleton2()
    //    {

    //    }
    //    public static Singleton2 GetInstance()
    //    {
    //        return instance;
    //    }
    //}
    #endregion
}
