using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 策略模式222
{
    class Program
    {
        static void Main(string[] args)
        {
            #region old
            //MovieTicket mt = new MovieTicket();
            //double originalPrice = 60.0;
            //double currentPrice = originalPrice;
            //mt.Price = originalPrice;
            //Console.WriteLine("原始票价:{0}", originalPrice);
            //IDiscount discount = AppConfigHelper.GetInstance() as IDiscount;
            //mt.Discount = discount;
            //Console.WriteLine("折后票价{0}", mt.Price); 
            #endregion

            #region old1
            //MovieTicket ticket = new MovieTicket();
            //double originalPrice = 60;
            //ticket.Price = originalPrice;
            //Console.WriteLine("原始票价为:"+originalPrice);
            //ticket.Discount = (IDiscount)AppConfigHelper.GetInstance();
            //Console.WriteLine("打折后票价为:"+ticket.Price); 
            #endregion

            MovieTicket ticket = new MovieTicket();
            double originalPrice = 60;
            Console.WriteLine("原始票价为{0}",originalPrice);
            ticket.Price = originalPrice;
            ticket.Discount = (IDiscount)AppConfigHelper.GetInstance();
            Console.WriteLine("折后票价为{0}",ticket.Price);
            Console.ReadKey();
        }
    }

    #region old
    ////Context环境类
    //public class MovieTicket
    //{
    //    private double _price;
    //    private IDiscount _discount;
    //    public double Price
    //    {
    //        get
    //        {
    //            return _discount.Calulate(_price);
    //        }
    //        set
    //        {
    //            _price = value;
    //        }
    //    }
    //    public IDiscount Discount
    //    {
    //        set
    //        {
    //            _discount = value;
    //        }
    //    }
    //}
    ////抽象策略类
    //public interface IDiscount
    //{
    //    double Calulate(double price);

    //}
    ////具体策略类：学生票折扣
    //public class StudentDiscount : IDiscount
    //{
    //    public double Calulate(double price)
    //    {
    //        Console.WriteLine("学生票:");
    //        return price * 0.8;
    //    }
    //}
    ////VIP会员票
    //public class VIPDiscount : IDiscount
    //{
    //    public double Calulate(double price)
    //    {
    //        Console.WriteLine("VIP票:");
    //        Console.WriteLine("增加积分");
    //        return price * 0.5;
    //    }
    //}

    #endregion

    #region old1
    //public class MovieTicket
    //{
    //    private double _price;
    //    private IDiscount _discount;
    //    public double Price
    //    {
    //        get { return _discount.Calculate(_price); }
    //        set { _price = value; }
    //    }
    //    public IDiscount Discount
    //    {
    //        set { _discount = value; }
    //    }
    //}
    //public interface IDiscount
    //{
    //    double Calculate(double price);
    //}
    //public class StudentDiscount : IDiscount
    //{
    //    public double Calculate(double price)
    //    {
    //        Console.WriteLine("学生票:");
    //        return price * 0.8;
    //    }
    //}
    //public class VIPDiscount : IDiscount
    //{
    //    public double Calculate(double price)
    //    {
    //        Console.WriteLine("会员票：赚积分");
    //        return price * 0.5;
    //    }
    //} 
    #endregion

    public class MovieTicket
    {
        private double _price;
        private IDiscount _discount;
        public double Price
        {
            get
            {
                return _discount.Calculate(_price);
            }
            set
            {
                _price = value;
            }
        }
        public IDiscount Discount
        {
            set { _discount = value; }
        }
    }
    public interface IDiscount
    {
        double Calculate(double price);
    }
    public class StudentDiscount : IDiscount
    {
        public double Calculate(double price)
        {
            Console.WriteLine("学生票打9折");
            return (price * 0.9);
        }
    }
    public class VIPDiscount : IDiscount
    {
        public double Calculate(double price)
        {
            Console.WriteLine("VIP赚积分");
            return (price * 0.6);
        }
    }



}
