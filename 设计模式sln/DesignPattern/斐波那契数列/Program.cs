using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace 斐波那契数列
{
    class Program
    {
        static void Main(string[] args)
        {

            int i = Fibonacci(3);
            Console.WriteLine(i);
            Console.ReadKey();
        }

        public static int Fibonacci(int i)
        {
            if (i < 2)
                return i == 0 ? 0 : 1;
            return Fibonacci(i - 1) + Fibonacci(i - 2);
        }

        public static int Factorial(int n)
        {
            int i = 0;
            int sum = 0;
            if (n == 0)
            {
                return 1;
            }
            sum = n * Factorial(n - 1);
            Console.WriteLine(sum);
            return sum;
        }
        public static int optimizeFibonacci(int first, int second, int n)
        {
            if (n > 0)
            {
                if (n == 1)
                {    // 递归终止条件
                    return first;       // 简单情景
                }
                else if (n == 2)
                {            // 递归终止条件
                    return second;      // 简单情景
                }
                else if (n == 3)
                {         // 递归终止条件
                    return first + second;      // 简单情景
                }
                int ss = optimizeFibonacci(second, first + second, n - 1);  // 相同重复逻辑，缩小问题规模
                return ss;
            }
            return -1;
        }
        public static bool isPalindromeString_recursive(string s)
        {
            int start = 0;
            int end = s.Length - 1;
            if (end > start)
            {   // 递归终止条件:两个指针相向移动，当start超过end时，完成判断
                char[] ch = s.ToCharArray();
                if (ch[start] != ch[end])
                {
                    return false;
                }
                else
                {
                    // 递归调用，缩小问题的规模
                    return isPalindromeString_recursive(s.Substring(start + 1).Substring(0, end - 1));
                }
            }
            return true;
        }

        #region 二分查找
        public static int BinarySearch(int[]arr, int key)
        {
            int low, high, mid;
            low = 1;
            high = arr.Length - 1;
            while (low <= high)
            {
                mid = (low + high) / 2;
                int a = arr[mid];
                if (key < arr[mid])
                {
                    high = mid - 1;
                }
                else if (key > arr[mid])
                {
                    low = mid + 1;
                }
                else
                {
                    return mid;
                }
            }
            return 0;

        }
        #endregion
    }

}
