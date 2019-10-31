using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 值传递引用传递
{
    class Program
    {
        static void Main(string[] args)
        {
            Student xiaohong = new Student("小红", 12);
            //BanZheng(ref xiaohong);
            BanJiaZheng(xiaohong);
            Console.WriteLine(xiaohong.Name + "  " + xiaohong.Age);
            
           
            Console.ReadKey();
        }

        static void BanZheng(ref Student student)
        {
            //student.Name = "红姐";
            //student.Age = 18;

            student = new Student("红姐", 18);
        }

        static void BanJiaZheng(Student student)
        {
            //student.Name = "红姐";
            //student.Age = 18;

            student = new Student("红姐", 18);
        }

        private static bool Print(int number)
        {
            Console.WriteLine(number);
            return number >= 200 || Print(number + 1);
        }
    }

    class Student
    {
        public Student(string name, int age)
        {
            Name = name;
            Age = age;
        }
        public string Name { get; set; }
        public int Age { get; set; }
    }

     
}
