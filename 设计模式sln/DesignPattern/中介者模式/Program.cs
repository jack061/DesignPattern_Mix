using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 中介者模式
{
    class Program
    {
        static void Main(string[] args)
        {

            #region old
            //UniteNationsSecurityCouncil UNSC = new UniteNationsSecurityCouncil();
            //USA c1 = new USA(UNSC);
            //Iraq c2 = new Iraq(UNSC);
            //UNSC.colleague1 = c1;
            //UNSC.colleague2 = c2;
            //c1.Declare("不准研发核武器，否则发动战争");
            //c2.Declare("我们没有核武器，不怕侵略"); 
            #endregion

            #region old1
            //Mediator mediator = new ConcreteMediator();
            //ConcreteColleage1 c1 = new ConcreteColleage1(mediator);
            //ConcreteColleage2 c2 = new ConcreteColleage2(mediator);
            //mediator.colleage1 = c1;
            //mediator.colleage2 = c2;
            //c1.Send("同事1收到消息");
            //c2.Send("同事2收到消息"); 
            #endregion

            Mediator m = new ConcreteMediator();
            Colleage c1 = new ConcreteColleage1(m);
            Colleage c2 = new ConcreteColleage2(m);
            m.colleage1 = c1;
            m.colleage2 = c2;
            c1.Send("同事1发出消息");
            c2.Send("同事2发出消息");
          
            Console.ReadKey();
        }
    }

    abstract class Mediator
    {
        public Colleage colleage1;
        public Colleage colleage2;
        public abstract void Send(string message,Colleage colleage);
    }
    class ConcreteMediator : Mediator
    {
        public override void Send(string message, Colleage colleage)
        {
            if (colleage==colleage1)
            {
                colleage2.Notify(message);
            }
            else
            {
                colleage1.Notify(message);
            }
        }
    }

    abstract class Colleage
    {
        public Mediator mediator;
        public Colleage(Mediator mediator)
        {
            this.mediator = mediator;
        }
        public abstract void Send(string message);
        public abstract void Notify(string message);
    }
    class ConcreteColleage1 : Colleage
    {
        public ConcreteColleage1(Mediator mediator):base(mediator)
        {

        }
        public override void Send(string message)
        {
            mediator.Send(message, this);
        }

        public override void Notify(string message)
        {
            Console.WriteLine("同事1收到消息:{0}",message);
        }
    }
    class ConcreteColleage2 : Colleage
    {
        public ConcreteColleage2(Mediator mediator):base(mediator)
        {

        }
        public override void Send(string message)
        {
            mediator.Send(message, this);
        }

        public override void Notify(string message)
        {
            Console.WriteLine("同事2收到消息:{0}",message);
        }
    }



    #region old1
    //public abstract class Mediator
    //{
    //    public ConcreteColleage1 colleage1;
    //    public ConcreteColleage2 colleage2;
    //    public abstract void Send(string message,Colleage colleage);

    //}
    //public class ConcreteMediator : Mediator
    //{
    //    public override void Send(string message, Colleage colleage)
    //    {
    //        if (colleage==colleage1)
    //        {
    //            colleage2.Notify(message);
    //        }
    //        else
    //        {
    //            colleage1.Notify(message);
    //        }
    //    }
    //}

    //public abstract class Colleage
    //{
    //    public Mediator mediator;
    //    public Colleage(Mediator mediator)
    //    {
    //        this.mediator = mediator;
    //    }
    //    public abstract void Send(string message);
    //    public abstract void Notify(string message);
    //}
    //public class ConcreteColleage1 : Colleage
    //{
    //    public ConcreteColleage1(Mediator mediator):base(mediator)
    //    {

    //    }
    //    public override void Send(string message)
    //    {
    //        mediator.Send(message, this);
    //    }

    //    public override void Notify(string message)
    //    {
    //        Console.WriteLine("同事1收到消息:{0}",message);
    //    }
    //}
    //public class ConcreteColleage2 : Colleage
    //{
    //    public ConcreteColleage2(Mediator mediator)
    //        : base(mediator)
    //    {

    //    }
    //    public override void Send(string message)
    //    {
    //        mediator.Send(message, this);
    //    }

    //    public override void Notify(string message)
    //    {
    //        Console.WriteLine("同事2收到消息:{0}", message);
    //    }
    //} 
    #endregion

    #region old2
    //abstract class Mediator
    //{
    //    public abstract void Send(string message, Colleage colleage);
    //}
    //class ConcreteMediator : Mediator
    //{
    //    public ConcreteColleage1 colleage1 { get; set; }
    //    public ConcreteColleage2 colleage2 { get; set; }

    //    public override void Send(string message, Colleage colleage)
    //    {
    //        if (colleage == colleage1)
    //        {
    //            colleage2.ShowMessage(message);
    //        }
    //        else
    //        {
    //            colleage1.ShowMessage(message);
    //        }
    //    }
    //}

    //abstract class Colleage
    //{
    //    public Mediator mediator;
    //    public Colleage(Mediator mediator)
    //    {
    //        this.mediator = mediator;
    //    }
    //}
    //class ConcreteColleage1 : Colleage
    //{
    //    public ConcreteColleage1(Mediator mediator)
    //        : base(mediator)
    //    {

    //    }
    //    public void Send(string message)
    //    {
    //        mediator.Send(message, this);
    //    }
    //    public void ShowMessage(string message)
    //    {
    //        Console.WriteLine("同事1收到消息:{0}", message);
    //    }
    //}
    //class ConcreteColleage2 : Colleage
    //{
    //    public ConcreteColleage2(Mediator mediator)
    //        : base(mediator)
    //    {

    //    }
    //    public void Send(string message)
    //    {
    //        mediator.Send(message, this);
    //    }
    //    public void ShowMessage(string message)
    //    {
    //        Console.WriteLine("同事2收到消息:{0}", message);
    //    }
    //}

    #endregion

    #region old
    //abstract class UniteNations
    //{
    //    public abstract void Declare(string message, Country colleague);
    //}
    //class UniteNationsSecurityCouncil : UniteNations
    //{
    //    public USA colleague1 { get; set; }
    //    public Iraq colleague2 { get; set; }

    //    public override void Declare(string message, Country colleague)
    //    {
    //        if (colleague == colleague1)
    //        {
    //            colleague2.GetMessage(message);
    //        }
    //        else
    //        {
    //            colleague1.GetMessage(message);
    //        }
    //    }
    //}


    //abstract class Country
    //{
    //    protected UniteNations mediator;
    //    public Country(UniteNations mediator)
    //    {
    //        this.mediator = mediator;
    //    }
    //}
    //class USA : Country
    //{
    //    public USA(UniteNations mediator)
    //        : base(mediator)
    //    {

    //    }
    //    public void Declare(string message)
    //    {
    //        mediator.Declare(message, this);
    //    }
    //    public void GetMessage(string message)
    //    {
    //        Console.WriteLine("美国获得对方信息:" + message);
    //    }
    //}
    //class Iraq : Country
    //{
    //    public Iraq(UniteNations mediator)
    //        : base(mediator)
    //    {

    //    }
    //    public void Declare(string message)
    //    {
    //        mediator.Declare(message, this);
    //    }
    //    public void GetMessage(string message)
    //    {
    //        Console.WriteLine("伊拉克获得对方信息:" + message);
    //    }

    //} 
    #endregion

}
