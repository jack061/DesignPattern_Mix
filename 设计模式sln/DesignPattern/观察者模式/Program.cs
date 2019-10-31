using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 观察者模式
{
    public delegate void mydelegate();

    class Program
    {



        static void Main(string[] args)
        {
            ISubject sub = new Secretary();
            sub.SubjectState = "胡汉三回来了";
       
            //sub.action += obj.Update;
            sub.Notify();

            #region old
            //ISubject subject = new Secretary();
            //subject.SubjectState = "老板回来了";
            //subject.Attach(new NBAObserver("燕七", subject));
            //subject.Attach(new FootballObserver("燕七", subject));
            //subject.Notify();
            #endregion
            Console.ReadKey();
        }
    }
    //通知者接口
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
        string SubjectState { get; set; }
        event Action action;
    }
    //通知者-秘书
    public class Secretary : ISubject
    {
        public event Action action;
        IList<IObserver> observers = new List<IObserver>();
        public void Attach(IObserver observer)
        {

            observers.Add(observer);

        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }
        public void Notify()
        {
            IObserver obj = new NBAObserver("燕七", this);
            obj.Update(this);
            action();
            //foreach (IObserver item in observers)
            //{
            //    item.Update(this);
            //}
        }

        public string SubjectState
        {
            get;
            set;
        }
     
    }


    //观察者接口
    public abstract class IObserver
    {
        public ISubject sub;
        public string name;
        public IObserver(string name, ISubject sub)
        {
            this.sub = sub;
            this.name = name;
        }
       
        public abstract void Update(ISubject sub);
    }

    //观察者-NBA
    class NBAObserver : IObserver
    {

        public NBAObserver(string name, ISubject sub)
            : base(name, sub)
        {
        }
        public override void Update(ISubject sub)
        {
            sub.action += sub_action;
         
        }

        void sub_action()
        {
            Console.WriteLine("{0}{1}快关闭NBA直播", sub.SubjectState, name);
        }
    }
    class FootballObserver : IObserver
    {
        public FootballObserver(string name, ISubject sub)
            : base(name, sub)
        {

        }
        public override void Update(ISubject sub)
        {
            sub.action += sub_action2;
       
        }

        void sub_action2()
        {
            Console.WriteLine("{0}{1}快关闭足球直播", sub.SubjectState, name);
        }
    }



    #region old
    ////通知者接口
    //interface ISubject
    //{
    //    void Attach(IObserver observer);
    //    void Detach(IObserver observer);
    //    void Notify();
    //    string SubjectState
    //    {
    //        get;
    //        set;
    //    }
    //}
    ////具体通知者-秘书
    //class Secretary : ISubject
    //{
    //    IList<IObserver> observers = new List<IObserver>();
    //    public void Attach(IObserver observer)
    //    {
    //        observers.Add(observer);
    //    }

    //    public void Detach(IObserver observer)
    //    {
    //        observers.Remove(observer);
    //    }

    //    public void Notify()
    //    {
    //        foreach (IObserver o in observers)
    //        {
    //            o.Update();
    //        }
    //    }

    //    public string SubjectState
    //    {
    //        get;
    //        set;
    //    }
    //}

    ////观察者接口
    //interface IObserver
    //{
    //    void Update();
    //}

    ////具体观察者-NBA
    //class NBAObserver : IObserver
    //{
    //    private string name;
    //    private ISubject sub;
    //    public NBAObserver(string name, ISubject sub)
    //    {
    //        this.name = name;
    //        this.sub = sub;
    //    }
    //    public void Update()
    //    {
    //        Console.WriteLine("{0}{1} 关闭NBA直播", name, sub.SubjectState);
    //    }
    //} 
    #endregion

}
