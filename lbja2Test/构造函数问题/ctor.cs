using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 构造函数问题
{
   public class ctor
    {
       public ctor(int _age,string _name,string _address)
       {
           this.Age = _age;
           this.Name = _name;
           this.Address = _address;
       }
       public ctor(int _age,string _name):this(_age,_name,"杨过")
       {

       }
       public ctor(int age):this(age,"杨过","英雄山庄")
       {

       }
       public ctor(string name)
           : this(12, name, "英雄山庄")
       {

       }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
