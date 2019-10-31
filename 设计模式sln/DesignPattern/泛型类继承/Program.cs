using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 泛型类继承
{
    class Program
    {
        static void Main(string[] args)
        {
            People<bool> p = new Student<bool>();
            p.loadPeople();
            Console.ReadKey();
        }
    }
    class People<T>
    {
        public virtual void loadPeople()
        {
            Type type = typeof(T);
            Console.WriteLine(type);
        }
    }
    class Student<T> : People<T> where T:new()
    {
        public override void loadPeople()
        {
            Type type = typeof(T);
            Console.WriteLine(type);
        }
        public void GetName<T>() where T : class
        {
            
        }
    }


}
