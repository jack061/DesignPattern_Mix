using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 代理模式
{
    class Program
    {
        static void Main(string[] args)
        {
            ISubject sub = new Proxy();
            sub.Request();
            Console.ReadKey();
        }
    }
    interface ISubject
    {
        void Request();
    }
    class RealSubject : ISubject
    {
        public void Request()
        {
            Console.WriteLine("这是真实的请求");
        }
    }
    class Proxy : ISubject
    {
        public ISubject sub;
        public void Request()
        {
            sub = new RealSubject();
            sub.Request();
        }
    }

    #region old
    //abstract class Subject
    //{
    //    public abstract void Request();

    //}
    //class RealSubject : Subject
    //{
    //    public override void Request()
    //    {

    //        Console.WriteLine("真实的请求");
    //    }
    //}
    //class Proxy : Subject
    //{
    //    public RealSubject realsubject;
    //    public override void Request()
    //    {
    //        realsubject = new RealSubject();
    //        realsubject.Request();
    //    }
    //} 
    #endregion
}
