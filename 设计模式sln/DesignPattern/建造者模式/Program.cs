using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 建造者模式
{
    class Program
    {
        static void Main(string[] args)
        {
            Director director = new Director();
            Builder b1 = new ConcreteBuilder1();
            Builder b2 = new ConcreteBuilder2();
            director.Construt(b1);
            Product p1 = b1.GetResult();
            p1.Show();
            director.Construt(b2);
            Product p2 = b2.GetResult();
            p2.Show();
            Console.ReadKey();
        }
    }
    //指挥者
    class Director
    {
        public void Construt(Builder builder)
        {
            builder.BuildPartA();
            builder.BuildPartB();
        }
    }
    //抽象建造者类，确定产品由两个部件构成PartA和PartB组成，并声明一个得到产品建造后结果的方法GetResult
    abstract class Builder
    {
        public abstract void BuildPartA();
        public abstract void BuildPartB();
        public abstract Product GetResult();
    }
    //具体建造者类1
    class ConcreteBuilder1 : Builder
    {
        private Product product = new Product();

        public override void BuildPartA()
        {
            product.Add("部件A");
        }

        public override void BuildPartB()
        {
            product.Add("部件B");
        }

        public override Product GetResult()
        {
            return product;
        }
    }
    //具体建造者2
    class ConcreteBuilder2 : Builder
    {
        private Product product = new Product();

        public override void BuildPartA()
        {
            product.Add("部件X");
        }

        public override void BuildPartB()
        {
            product.Add("部件Y");
        }

        public override Product GetResult()
        {
            return product;
        }
    }
    //产品类，由多个部件构成
    class Product
    {
        IList<string> parts = new List<string>();
        public void Add(string part)
        {
            parts.Add(part);
        }
        public void Show()
        {
            Console.WriteLine("\n 产品 创建=---");
            foreach (var part in parts)
            {
                Console.WriteLine(part);
            }
        }

    }
}
