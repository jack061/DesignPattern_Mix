using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 数据结构
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "I am a student";
            str = ReverseSentense(str);
            Console.WriteLine(str);
            Console.ReadKey();
        }
        #region 第一个只出现一次的字符
        //复杂度O(n*n)
        private static char GetFirstChar(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                bool isExist = false;
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[i] == str[j] && i != j)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    return str[i];
                }
            }
            return '@';

        }
        //复杂度O(n)
        private static char GetFirstCharHash(string str)
        {
            //hashtable以单个字符为键,出现次数为值
            Hashtable ht = new Hashtable();
            for (int i = 0; i < str.Length; i++)
            {
                if (ht.ContainsKey(str[i]))
                {
                    ht[str[i]] = int.Parse(ht[str[i]].ToString()) + 1;
                }
                else
                {
                    ht[str[i]] = "1";
                }
            }
            //hashtable为无序集合
            for (int i = 0; i < str.Length; i++)
            {
                if (ht[str[i]].ToString() == "1")
                {
                    return str[i];
                }
            }
            return '@';
        }
        #endregion

        #region 反转字符串字符
        public static string ReverseSentense(string sentense)
        {
            if (string.IsNullOrEmpty(sentense))
            {
                return null;
            }
            char[] array = sentense.ToArray();
            int start = 0;
            int end = array.Length - 1;
            array = Reverse(array, start, end);
            start = end = 0;
            while (start<array.Length)
            {
                if (end==array.Length||array[end]==' ')
                {
                    
                }
            }

        }
        public static char[] Reverse(char[] array, int start, int end)
        {
            if (start == null || start < 0 || end > array.Length - 1)
            {
                return null;
            }
            while (start < end)
            {
                char temp = array[end];
                array[end] = array[start];
                array[start] = temp;
                start++;
                end--;
            }
            return array;
        }
        #endregion
    }


}
