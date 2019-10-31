using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 装饰模式
{
    class Program
    {
        static void Main(string[] args)
        {

            #region old
            //Person p = new Person("燕七");
            //TShirts t = new TShirts();
            //BigTrousers b = new BigTrousers();
            //t.Decorator(p);
            //b.Decorator(t);
            //b.show(); 
            #endregion

            Person p = new Person("lbj");
            Finery t = new TShirts();
            Finery b = new BigTrousers();
            t.SetDecorator(p);
            b.SetDecorator(t);
            b.Run();
            Console.ReadKey();
        }
    }

    class Person
    {
        public string name;
        public Person()
        {

        }
        public Person(string name)
        {
            this.name = name;
        }
        public virtual void Run()
        {
            Console.WriteLine("this is decorator for {0}", name);
        }
    }
    class Finery : Person
    {
        public Person p;
        public void SetDecorator(Person p)
        {
            this.p = p;
        }
        public override void Run()
        {
            if (p != null)
            {
                p.Run();
            }
        }
    }
    class BigTrousers : Finery
    {
        public override void Run()
        {
            Console.WriteLine("big trousers");
            base.Run();
        }
    }
    class TShirts : Finery
    {
        public override void Run()
        {
            Console.WriteLine("TShirts");
            base.Run();
        }
    }

    //class Person
    //{
    //    public string name;
    //    public Person(string name)
    //    {
    //        this.name = name;
    //    }
    //    public Person()
    //    {

    //    }
    //    public virtual void Show()
    //    {
    //        Console.WriteLine("这是对{0}的装饰", name);
    //    }
    //}
    //class Finery : Person
    //{

    //    public Person p;
    //    public void SetDecorator(Person p)
    //    {
    //        this.p = p;
    //    }
    //    public override void Show()
    //    {
    //        if (p!=null)
    //        {
    //            p.Show();
    //        }
    //    }
    //}
    //class BigTrousers:Finery
    //{
    //    public override void Show()
    //    {
    //        Console.WriteLine("裤子");
    //        base.Show();
    //    }
    //}
    //class TShirts:Finery
    //{
    //    public override void Show()
    //    {
    //        Console.WriteLine("大体恤");
    //        base.Show();
    //    }
    //}

    #region old
    //class Person
    //{
    //    private string name;
    //    public Person()
    //    {

    //    }
    //    public Person(string name)
    //    {
    //        this.name = name;
    //    }
    //    public virtual void show()
    //    {
    //        Console.WriteLine("这是对{0}的装饰", name);
    //    }
    //}
    //class Finery : Person
    //{
    //    public Person component;
    //    public void Decorator(Person p)
    //    {
    //        this.component = p;

    //    }
    //    public override void show()
    //    {
    //        if (component != null)
    //        {
    //            component.show();
    //        }
    //    }
    //}
    //class TShirts : Finery
    //{
    //    public override void show()
    //    {
    //        Console.WriteLine("大体恤");
    //        base.show();
    //    }
    //}
    //class BigTrousers : Finery
    //{
    //    public override void show()
    //    {
    //        Console.WriteLine("裤子");
    //        base.show();
    //    }
    //} 
    #endregion

}
