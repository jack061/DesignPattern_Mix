using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 命令模式
{
    class Program
    {
        static void Main(string[] args)
        {

            #region old
            ////开店前的准备
            //Barbecuer boy = new Barbecuer();
            //Command bakeMuttonCommand1 = new BakeMuttonCommand(boy);
            //Command bakeMuttonCommand2 = new BakeMuttonCommand(boy);
            //Command bakeChickenWingCommand1 = new BakeChickenWingCommand(boy);
            //Waiter girl = new Waiter();
            ////开门营业，顾客点菜
            //girl.SetOrder(bakeMuttonCommand1);
            //girl.SetOrder(bakeMuttonCommand2);
            //girl.SetOrder(bakeChickenWingCommand1);
            ////点菜完毕，通知厨房
            //girl.Notify(); 
            #endregion

            BarbecueMan receiver = new BarbecueMan();
            Waiter w = new Waiter();
            Command c1 = new BakeMuttonCommand(receiver);
            Command c2 = new BakeChickenWingsCommand(receiver);
            w.Add(c1);
            w.Add(c2);
            w.Notify();
            Console.ReadKey();
        }
    }
    class Waiter
    {
        public List<Command> children = new List<Command>();
        public void Add(Command c)
        {
            children.Add(c);

        }
        public void Remove(Command c)
        {
            children.Remove(c);
        }
        public void Notify()
        {
            foreach (var item in children)
            {
                item.ExcuteCommand();
            }
        }

    }
    abstract class Command
    {
        public BarbecueMan receiver;
        public Command(BarbecueMan receiver)
        {
            this.receiver = receiver;
        }
        public abstract void ExcuteCommand();
    }
    class BakeMuttonCommand : Command
    {
        public BakeMuttonCommand(BarbecueMan receiver)
            : base(receiver)
        {

        }
        public override void ExcuteCommand()
        {
            receiver.BakeMutton();
        }
    }
    class BakeChickenWingsCommand : Command
    {
        public BakeChickenWingsCommand(BarbecueMan receiver):base(receiver)
        {

        }
        public override void ExcuteCommand()
        {
            receiver.BakeChickenWings();
        }
    }

    class BarbecueMan
  {
      public void BakeMutton()
      {
          Console.WriteLine("烤羊肉串");
      }
      public void BakeChickenWings()
      {
          Console.WriteLine("烤鸡翅");
      }
  }

    #region old
    ////服务员
    //class Waiter
    //{
    //    private IList<Command> orders = new List<Command>();
    //    //设置订单
    //    public void SetOrder(Command command)
    //    {
    //        orders.Add(command);
    //        Console.WriteLine("增加订单:"+command.ToString());
    //    }
    //    //取消订单
    //    public void CancelOrder(Command command)
    //    {
    //        orders.Remove(command);
    //        Console.WriteLine("取消订单:"+command.ToString());
    //    }
    //    //通知全部执行
    //    public void Notify()
    //    {
    //        foreach (var item in orders)
    //        {
    //            item.ExcuteCommand();
    //        }
    //    }
    //}
    ////抽象命令类
    //abstract class Command
    //{
    //    protected Barbecuer receiver;
    //    public Command(Barbecuer receiver)
    //    {
    //        this.receiver = receiver;
    //    }
    //    public abstract void ExcuteCommand();
    //}
    ////烤羊肉串命令
    //class BakeMuttonCommand : Command
    //{
    //    public BakeMuttonCommand(Barbecuer receiver)
    //        : base(receiver)
    //    {

    //    }
    //    public override void ExcuteCommand()
    //    {
    //        receiver.BakeMutton();
    //    }
    //}
    ////烤鸡翅命令
    //class BakeChickenWingCommand : Command
    //{
    //    public BakeChickenWingCommand(Barbecuer receiver)
    //        : base(receiver)
    //    {

    //    }
    //    public override void ExcuteCommand()
    //    {
    //        receiver.BakeChickenWing();
    //    }
    //}
    ////烤肉串者
    //public class Barbecuer
    //{
    //    public void BakeMutton()
    //    {
    //        Console.WriteLine("烤羊肉串");
    //    }
    //    public void BakeChickenWing()
    //    {
    //        Console.WriteLine("烤鸡翅");
    //    }
    //} 
    #endregion

}
