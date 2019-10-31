using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEquals
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<object, int> dic = new Dictionary<object, int>();
            Coordinates c1 = new Coordinates(5, 5);
            Coordinates c2 = new Coordinates(5, 5);
            dic.Add(c1, 1);
            dic.Add(c2, 2);
            Console.ReadKey();
        }
    }
    class Coordinates
    {
        int x { get; set; }
        int y { get; set; }
        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public override int GetHashCode()
        {
            return (this.x * 5) * (this.x * 10);
        }
        //public override bool Equals(object obj)
        //{
        //    Coordinates coord = obj as Coordinates;
        //    if (coord.x == this.x && coord.y == this.y)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
