using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 职责链模式
{
    class Program
    {
        static void Main(string[] args)
        {
            #region old
            //Handler h1 = new ConcreteHandler1();
            //Handler h2 = new ConcreteHandler2();
            //Handler h3 = new ConcreteHandler3();
            //h1.SetSuccessor(h2);
            //h1.SetSuccessor(h3);
            //int[] requests = { 2, 4, 5,12, 44, 28, 3, 52, 27 };
            //foreach (int request in requests)
            //{
            //    h1.HandlerRequest(request);
            //} 
            #endregion

            Handler h1 = new Handler1();
            Handler h2 = new Handler2();
            Handler h3 = new Handler3();
            h1.SetSuccessor(h2);
            h2.SetSuccessor(h3);
            int[] requests = { 1, 33, 44, 22, 233, 1, 556, 43, 422, 21, 34, 446, 3 };
            foreach (var item in requests)
            {
                h1.HandlerRequest(item);
            }
            Console.ReadKey();
        }
    }

    public abstract class Handler
    {
        public Handler successor;

        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }
        public abstract void HandlerRequest(int request);
    }
    public class Handler1 : Handler
    {
        public override void HandlerRequest(int request)
        {
            if (request <= 15)
            {
                Console.WriteLine("{0}待直线经理审核", request);
            }
            else
            {
                successor.HandlerRequest(request);
            }
        }
    }
    public class Handler2 : Handler
    {
        public override void HandlerRequest(int request)
        {
            if (request <= 50)
            {
                Console.WriteLine("{0}待合同管理员审核", request);
            }
            else
            {
                successor.HandlerRequest(request);
            }
        }
    }
    public class Handler3 : Handler
    {
        public override void HandlerRequest(int request)
        {
            Console.WriteLine("{0}业务处主管审核", request);
        }
    }



    #region old
    //abstract class Handler
    //{
    //    protected Handler successor;
    //    public void SetSuccessor(Handler successor)
    //    {
    //        this.successor = successor;
    //    }
    //    public abstract void HandlerRequest(int request);
    //}
    //class ConcreteHandler1 : Handler
    //{
    //    public override void HandlerRequest(int request)
    //    {
    //        if (request>=0&&request<10)
    //        {
    //            Console.WriteLine("{0} 处理请求 {1}",this.GetType().Name,request);
    //        }
    //        else if (successor!=null)
    //        {
    //            successor.HandlerRequest(request);
    //        }
    //    }
    //}
    //class ConcreteHandler2 : Handler
    //{
    //    public override void HandlerRequest(int request)
    //    {
    //        if (request >= 10 && request < 20)
    //        {
    //            Console.WriteLine("{0} 处理请求 {1}", this.GetType().Name, request);
    //        }
    //        else if (successor != null)
    //        {
    //            successor.HandlerRequest(request);
    //        }
    //    }
    //}
    //class ConcreteHandler3 : Handler
    //{
    //    public override void HandlerRequest(int request)
    //    {
    //        if (request >=20&&request<=40)
    //        {
    //            Console.WriteLine("{0} 处理请求 {1}", this.GetType().Name, request);
    //        }
    //        else if (successor != null)
    //        {
    //            successor.HandlerRequest(request);
    //        }
    //    }
    //} 
    #endregion


}
