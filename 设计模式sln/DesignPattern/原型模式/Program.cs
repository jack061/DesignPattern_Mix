using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 原型模式
{
    class Program
    {
        static void Main(string[] args)
        {
            ConcretePrototype1 p1=new ConcretePrototype1("L");
            ConcretePrototype1 c1=(ConcretePrototype1)p1.Clone();
            ConcretePrototype1 d2 = (ConcretePrototype1)c1.Clone();
            c1.Id = "LBJ";
            Console.WriteLine(d2.Id);
            Console.ReadKey();
        }
    }

    abstract class Prototype
    {
        private string id;
        public Prototype(string id)
        {
            this.id = id;
        }
        public string Id { get { return id; } set { id = value; } }
        public abstract Prototype Clone();
    }
    class ConcretePrototype1 : Prototype
    {
        public ConcretePrototype1(string id):base(id)
        {

        }

        public override Prototype Clone()
        {
            return (Prototype)this.MemberwiseClone();
        }
    }
    class ConcretePrototype2 : Prototype
    {
        public ConcretePrototype2(string id)
            : base(id)
        {

        }

        public override Prototype Clone()
        {
            return (Prototype)this.MemberwiseClone();
        }
    }


}
