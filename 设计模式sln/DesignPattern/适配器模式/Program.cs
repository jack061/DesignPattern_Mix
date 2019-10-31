using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 适配器模式
{
    class Program
    {
        static void Main(string[] args)
        {

            #region old
            //Player b = new Forwards("詹姆斯");
            //b.Attack();
            //Player m = new PointGuards("保罗");
            //m.Attack();
            //Player ym = new Translator("姚明");
            //ym.Attack();
            //ym.Defense(); 
            #endregion

            Player sf = new Forwards("詹姆斯");
            sf.Attack();
            Player c = new Translator("姚明");
            c.Attack();
            Console.Read();
        }
    }

    #region old
    //abstract class Player
    //{
    //    protected string name;
    //    public Player(string name)
    //    {
    //        this.name = name;
    //    }
    //    public abstract void Attack();
    //    public abstract void Defense();
    //}
    //class Forwards : Player
    //{
    //    public Forwards(string name)
    //        : base(name)
    //    {

    //    }
    //    public override void Attack()
    //    {
    //        Console.WriteLine("前锋{0} 进攻", name);
    //    }

    //    public override void Defense()
    //    {
    //        Console.WriteLine("前锋{0} 防守", name);
    //    }
    //}
    //class Center : Player
    //{
    //    public Center(string name)
    //        : base(name)
    //    {

    //    }
    //    public override void Attack()
    //    {
    //        Console.WriteLine("中锋{0} 进攻", name);
    //    }

    //    public override void Defense()
    //    {
    //        Console.WriteLine("中锋{0} 防守", name);
    //    }
    //}
    //class PointGuards : Player
    //{
    //    public PointGuards(string name)
    //        : base(name)
    //    {

    //    }
    //    public override void Attack()
    //    {
    //        Console.WriteLine("后卫{0} 进攻", name);
    //    }

    //    public override void Defense()
    //    {
    //        Console.WriteLine("后卫{0} 防守", name);
    //    }
    //}
    ////外籍中锋
    //class ForeignCenter
    //{
    //    private string name;
    //    public string Name
    //    {
    //        get { return name; }
    //        set { name = value; }
    //    }
    //    public void 进攻()
    //    {
    //        Console.WriteLine("外籍中锋{0}进攻", name);
    //    }
    //    public void 防守()
    //    {
    //        Console.WriteLine("外籍中锋{0}防守", name);
    //    }
    //}
    ////翻译者
    //class Translator : Player
    //{
    //    private ForeignCenter wjzf = new ForeignCenter();
    //    public Translator(string name)
    //        : base(name)
    //    {
    //        wjzf.Name = name;
    //    }
    //    public override void Attack()
    //    {
    //        wjzf.进攻();
    //    }

    //    public override void Defense()
    //    {
    //        wjzf.防守();
    //    }
    //}

    #endregion

    abstract class Player
    {
        protected string name;
        public Player(string name)
        {
            this.name = name;
        }
        public abstract void Attack();
        public abstract void Defence();
    }
    class Forwards : Player
    {
        public Forwards(string name)
            : base(name)
        {


        }
        public override void Attack()
        {
            Console.WriteLine("前锋{0}进攻", name);
        }

        public override void Defence()
        {
            Console.WriteLine("前锋{0}防守", name);
        }
    }
    //外籍中锋
    class ForeignCenter
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public void 进攻()
        {
            Console.WriteLine("外籍中锋{0}进攻", name);
        }
        public void 防守()
        {
            Console.WriteLine("外籍中锋{0}防守", name);
        }
    }
    class Translator : Player
    {
        ForeignCenter center = new ForeignCenter();
        public Translator(string name)
            : base(name)
        {
            center.Name = name;
        }
        public override void Attack()
        {
            center.进攻();
        }


        public override void Defence()
        {
            center.防守();
        }
    }




}
