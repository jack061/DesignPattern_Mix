using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 外观模式
{
    class Program
    {
        static void Main(string[] args)
        {
            Fund fund = new Fund();
            fund.Buy();
            fund.Sell();
            Console.ReadKey();

        }
    }
    class Fund
    {
        Stock stock1;
        Stock stock2;

        public Fund()
        {
            stock1 = new Stock1("燕七");
            stock2 = new Stock2();
        }
        public void Buy()
        {
            stock1.Buy();
            stock2.Buy();
        }
        public void Sell()
        {
            stock1.Sell();
            stock2.Sell();
        }
    }
    abstract class Stock
    {
        private string name;
        public Stock()
        {

        }
        public Stock(string name)
        {
            this.name = name;
        }

        public abstract void Sell();
        public abstract void Buy();
    }
    class Stock1 : Stock
    {
        public Stock1(string name)
        {
            Console.WriteLine(name);
        }
        public override void Buy()
        {
            Console.WriteLine("股票1买入");
        }

        public override void Sell()
        {
            Console.WriteLine("股票1卖出");
        }
    }
    class Stock2 : Stock
    {
        public override void Buy()
        {
            Console.WriteLine("股票2买入");
        }

        public override void Sell()
        {
            Console.WriteLine("股票2卖出");
        }
    }

}
