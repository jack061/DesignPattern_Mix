using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 扩展方法
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = string.Empty;
            s.ToInt();
        }


    }
    public static class EString
    {
        /// <summary>
        /// 将字符串转换为Int
        /// </summary>
        /// <param name="t"></param>
        /// <returns>当转换失败时返回0</returns>
        public static int ToInt(this string t)
        {
            int id;
            int.TryParse(t, out id);//这里当转换失败时返回的id为0
            return id;
        }
        public static int IsRange(this DateTime t, DateTime startTime, DateTime endTime)
        {
            if (((startTime - t).TotalSeconds > 0))
            {
                return -1;
            }

            if (((endTime - t).TotalSeconds < 0))
            {
                return 1;
            }

            return 0;
        }
    }
}
