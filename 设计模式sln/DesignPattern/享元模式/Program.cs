using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 享元模式
{
    class Program
    {
        static void Main(string[] args)
        {
            #region old
            //int extrinsicstate = 22;

            //FlyweightFactory f = new FlyweightFactory();

            //Flyweight fx = f.GetFlyweight("X");
            //fx.Operation(--extrinsicstate);

            //Flyweight fy = f.GetFlyweight("Y");
            //fy.Operation(--extrinsicstate);

            //Flyweight fz = f.GetFlyweight("Z");
            //fz.Operation(--extrinsicstate);

            //UnsharedConcreteFlyweight uf = new UnsharedConcreteFlyweight();

            //uf.Operation(--extrinsicstate); 
            #endregion

            #region old1
            //WebSiteFactory facotory = new WebSiteFactory();
            //WebSite w1 = facotory.GetWetSite("产品展示");
            //w1.Use(new User("燕七"));
            //WebSite w2 = facotory.GetWetSite("博客");
            //w2.Use(new User("郭大路"));
            //WebSite w3 = facotory.GetWetSite("产品展示");
            //w3.Use(new User("王动"));
            //WebSite w4 = facotory.GetWetSite("博客");
            //w4.Use(new User("林太平"));
            //Console.WriteLine("网站总数为{0}", facotory.GetWebCount()); 
            #endregion

            WebSiteFactory factory = new WebSiteFactory();
            WebSite w1 = factory.GetWebSite("产品展示");
            w1.Use(new User("燕七"));
            WebSite w2 = factory.GetWebSite("预览");
            w2.Use(new User("郭大路"));
            WebSite w3 = factory.GetWebSite("产品展示");
            w3.Use(new User("林太平"));
            Console.WriteLine("网站个数为{0}",factory.GetWebSiteCount());
            Console.ReadKey();
        }
    }
    public class WebSiteFactory
    {
        Hashtable flyweights = new Hashtable();
        public WebSite GetWebSite(string key)
        {
            if (!flyweights.ContainsKey(key))
            {
                flyweights.Add(key, new ConcreteWebSite(key));
            }
            return (WebSite)flyweights[key];
        }
        public int GetWebSiteCount()
        {
            return flyweights.Count;
        }

    }
    public class User
    {
        public string name;
        public User(string name)
        {
            this.name = name;
        }
    }
    public abstract class WebSite
    {
        public string name;
        public WebSite(string name)
        {
            this.name = name;
        }
        public abstract void Use(User user);
    }
    public class ConcreteWebSite : WebSite
    {
        public ConcreteWebSite(string name):base(name)
        {

        }
        public override void Use(User user)
        {
            Console.WriteLine("网站名称{0},用户名:{1}",name,user.name);
        }
    }



    #region old
    //public class WebSiteFactory
    //{
    //    Hashtable flyweights = new Hashtable();
    //    public WebSite GetWetSite(string key)
    //    {
    //        if (!flyweights.ContainsKey(key))
    //        {
    //            flyweights.Add(key, new ConcreteWebSite(key));
    //        }
    //        return ((WebSite)flyweights[key]);
    //    }
    //    public int GetWebCount()
    //    {
    //        return flyweights.Count;
    //    }
    //}
    //public class User
    //{
    //    public string name;
    //    public User(string name)
    //    {
    //        this.name = name;
    //    }
    //}
    //public abstract class WebSite
    //{
    //    public abstract void Use(User user);
    //}
    //public class ConcreteWebSite : WebSite
    //{
    //    public string name;
    //    public ConcreteWebSite(string name)
    //    {
    //        this.name = name;
    //    }
    //    public override void Use(User user)
    //    {
    //        Console.WriteLine("网站分类：" + name + " 用户：" + user.name);
    //    }
    //}

    #endregion

    #region region
    //class FlyweightFactory
    //{
    //    private Hashtable flyweights = new Hashtable();
    //    public FlyweightFactory()
    //    {
    //        flyweights.Add("X", new ConcreteFlyweight());
    //        flyweights.Add("Y", new ConcreteFlyweight());
    //        flyweights.Add("Z", new ConcreteFlyweight());
    //    }
    //    public Flyweight GetFlyweight(string key)
    //    {
    //        return (Flyweight)flyweights[key];
    //    }
    //}
    //abstract class Flyweight
    //{
    //    public abstract void Operation(int extrinsicstate);  
    //}
    //class ConcreteFlyweight : Flyweight
    //{
    //    public override void Operation(int extrinsicstate)
    //    {
    //        Console.WriteLine("具体Flyweight:"+extrinsicstate);
    //    }
    //}
    //class UnsharedConcreteFlyweight : Flyweight
    //{
    //    public override void Operation(int extrinsicstate)
    //    {
    //        Console.WriteLine("不共享的具体Flyweight:"+extrinsicstate);
    //    }
    //} 
    #endregion


}
