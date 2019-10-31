using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foreach遍历
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 第一种实现方式
            //Person p = new Person();
            //IEnumerator etor = p.GetEnumerator();
            //while (etor.MoveNext())
            //{
            //    Console.WriteLine(etor.Current);
            //} 
            #endregion

            #region 第二种实现方式
            Person p = new Person();
            foreach (var name in p)
            {
                Console.WriteLine(name);
            }
            #endregion

            Console.ReadKey();

        }
    }

    #region 原始实现foreach
    //class Person : IEnumerable
    //{
    //    public string Name { get; set; }
    //    public int Age { get; set; }

    //    public string[] friends = new string[] { "燕七", "郭大路", "王动", "林太平" };
    //    public IEnumerator GetEnumerator()
    //    {
    //        return new PersonEnumerator(friends);
    //    }
    //}
    //class PersonEnumerator : IEnumerator
    //{
    //    public string[] _frs;
    //    public PersonEnumerator(string[] frs)
    //    {
    //        this._frs = frs;
    //    }
    //    public int index = -1;
    //    public object Current
    //    {
    //        get { return _frs[index]; }
    //    }

    //    public bool MoveNext()
    //    {
    //        if (index < _frs.Length - 1)
    //        {
    //            index++;
    //            return true;
    //        }
    //        return false;
    //    }

    //    public void Reset()
    //    {
    //        index = -1;
    //    }
    //}
    
    #endregion

    #region yiled 实现foreach
    class Person : IEnumerable
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public string[] friends = new string[] { "燕七", "郭大路", "王动", "林太平" };
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < friends.Length; i++)
            {
                yield return friends[i];
            }
        }
    }
    #endregion
}
