using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            DelegateEventArgs del = new DelegateEventArgs();
            del.invokeDelegate();
            Console.ReadKey();
        }
        class DelegateEventArgs
        {
            public delegate void MyDelegate(int x, int y);
            public MyDelegate mydelegate;
            public void invokeDelegate()
            {
                mydelegate = new MyDelegate(getInt);
                mydelegate.Invoke(4, 3);

            }
            public void getInt(int x, int y)
            {
                Console.WriteLine("this is "+x+","+y);
            }
        }

        class Foo
        {
            public static string Field = GetString("Initialize the static field");
            public static string GetString(string s)
            {
                Console.WriteLine(s);
                return s;
            }
        }

        #region 递归
        public static void Permutation(char[] str, char[] begin, int startIndex)
        {
            if (startIndex == str.Length)
            {
                Console.WriteLine(str);
            }
            else
            {
                for (int i = startIndex; i < str.Length; i++)
                {
                    char temp = begin[i];
                    begin[i] = begin[startIndex];
                    begin[startIndex] = temp;

                    Permutation(str, begin, startIndex + 1);

                    temp = begin[i];
                    begin[i] = begin[startIndex];
                    begin[startIndex] = temp;
                }
            }
        }
        public static int Fibonacci(int n)
        {
            if (n < 0) return -1;
            if (n == 0) return 0;
            if (n == 1) return 1;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        public static void Permutation(string[] nums, int m, int n)
        {
            string t;
            if (m < n - 1)
            {
                Permutation(nums, m + 1, n);
                for (int i = m + 1; i < n; i++)
                {
                    //可抽取Swap方法
                    t = nums[m];
                    nums[m] = nums[i];
                    nums[i] = t;

                    Permutation(nums, m + 1, n);

                    //可抽取Swap方法
                    t = nums[m];
                    nums[m] = nums[i];
                    nums[i] = t;
                }
            }
            else
            {
                for (int j = 0; j < nums.Length; j++)
                {
                    Console.Write(nums[j]);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
