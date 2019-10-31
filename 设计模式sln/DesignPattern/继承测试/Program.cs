using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 继承测试
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new BasketBallPlayer("燕七");
            p.Talk();
            p.Say();
            Console.ReadKey();
        }
    }
    public class Person
    {
        protected string name;
        public Person(string name)
        {
            this.name = name;
        }
        public  void Say()
        {
            Console.WriteLine("一个人{0}说话", name);
        }
        public virtual void Talk()
        {
            Console.WriteLine("这是人{0}",name);
        }
    }
    public class Player:Person
    {
        public Player(string name):base(name)
        {

        }
        public override void Talk()
        {
            Console.WriteLine("这是运动员:{0}",name);
        }
    }

    public class BasketBallPlayer : Player
    {
        public BasketBallPlayer(string name)
            : base(name)
        {

        }
        public override void Talk()
        {
            base.Talk();
            Console.WriteLine("这是篮球运动员:{0}",name);
        }
    }
}
