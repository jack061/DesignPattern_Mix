using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 多态
{
    class Program
    {
        static void Main(string[] args)
        {
            //Animal cat = new Cat();
            //Animal dog = new Dog();
            //Animal smallDog = new SmallDog();
            //cat.Eat();
            //dog.Eat();
            //smallDog.Eat();
            IList<Action> actions = new List<Action>();
            for (int i = 0; i < 5; i++)
            {
                actions.Add(() => Console.WriteLine(i));
            }
            foreach (var item in actions)
            {
                item();
            }
            Console.ReadKey();
        }
    }
    class Animal
    {
  
        public virtual void Eat()
        {
            Console.WriteLine("Animal Eat");
        }
    }
    class Cat : Animal
    {
        public new void Eat()
        {
            Console.WriteLine("Cat Eat");
        }
    }
 
    class Dog : Animal
    {
        public override void Eat()
        {
            Console.WriteLine("Dog Eat");
        }
    }
    class SmallDog : Dog
    {
        public override void Eat()
        {
            Console.WriteLine("SmallDog Eat");
        }
    }
}
