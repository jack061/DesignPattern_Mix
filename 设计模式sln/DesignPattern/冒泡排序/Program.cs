using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 冒泡排序
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int> { 8, 5, 23, 2, 54 };
            QuickSort(list, 0, list.Count - 1);
            int[] array = new int[] { 8, 5, 23, 2, 54 };
            //QuickSort(array);
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        #region 冒泡排序
        public static void BubbleSort(int[] array)
        {
            bool flag = true;
            for (int i = 0; i < array.Length && flag; i++)
            {
                flag = false;
                for (int j = array.Length - 2; j >=i; j--)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    break;
                }
            }

        }
        #endregion

        #region 简单选择排序
        public static void SimpleChooseSort(int[] array)
        {
            int i, j, min;
            for (i = 0; i < array.Length; i++)
            {
                min = i;
                for (j = i + 1; j < array.Length; j++)
                {
                    if (array[min] > array[j])
                    {
                        min = j;
                    }
                }
                if (i != min)
                {
                    Swap(array, i, min);
                }
            }
        }
        public static void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        #endregion

        #region 快速排序
        //获取按枢轴值左右分流后枢轴的位置
        private static int Division(List<int> list, int left, int right)
        {
            while (left < right)
            {
                int pivot = left;
                if (list[left] > list[left + 1])
                {
                    int temp = list[left];
                    list[left] = list[left + 1];
                    list[left + 1] = temp;
                    left++;
                }
                else
                {
                    int temp2 = list[right];
                    list[right] = list[left + 1];
                    list[left + 1] = temp2;
                    right--;
                }
            }
            return left;
        }

        private static void QuickSort(List<int> list, int left, int right)
        {
            if (left < right)
            {
                int i = Division(list, left, right);
                //中枢轴左边排序
                QuickSort(list, left, i - 1);
                //中枢轴右边排序
                QuickSort(list, i + 1, right);
            }
        }
        #endregion

        #region 直接插入排序
        public static void InsertSort(int[] arry)
        {
            //直接插入排序是将待比较的数值与它的前一个数值进行比较，所以外层循环是从第二个数值开始的
            for (int i = 1; i < arry.Length; i++)
            {
                //如果当前元素小于其前面的元素
                if (arry[i] < arry[i - 1])
                {
                    //用一个变量来保存当前待比较的数值，因为当一趟比较完成时，我们要将待比较数值置入比它小的数值的后一位 
                    int temp = arry[i];
                    int j = 0;
                    for (j = i - 1; j >= 0 && temp < arry[j]; j--)
                    {
                        arry[j + 1] = arry[j];
                    }
                    arry[j + 1] = temp;
                }
            }
        }
        #endregion
    }
}
