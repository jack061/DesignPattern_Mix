using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using RM.Common.DotNetJson;
using RM.Busines.Busines;
using System.Data;

namespace RM.Busines.Util
{
    public class Util
    {
        /// <summary>
        /// json 转 hashtable  list
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<Hashtable> getListFromJson(string json)
        {
            List<Hashtable> list = new List<Hashtable>();
            list = JsonHelper.DeserializeJsonToList<Hashtable>(json);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceName">序列号key</param>
        /// <param name="num">位数  不足左补零</param>
        /// <returns></returns>
        public static String getSequenceAutoAddZero(String sequenceName, int num)
        {
            String sequenceValue = Sequence.getSequence(sequenceName).ToString();
            for (int i = sequenceValue.Trim().Length; i < num; i++)
            {
                sequenceValue = "0" + sequenceValue;
            }
            return sequenceValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceValue">传递的值</param>
        /// <param name="num">位数  不足左补零</param>
        /// <returns></returns>
        public static String getNumberAddZero(String sequenceValue, int num)
        {
          
            for (int i = sequenceValue.Trim().Length; i < num; i++)
            {
                sequenceValue = "0" + sequenceValue;
            }
            return sequenceValue;
        }

        /// <summary>
        /// 数字转化为英文表示
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NumberToEnglishString(int number)
        {
            String strNumber = null;
            if (number < 0)
            {
                strNumber = "error";
                return strNumber;
            }
            if (number < 20)
            {
                switch (number)
                {
                    case 0:
                        strNumber = "zero";
                        return strNumber;
                    case 1:
                        strNumber = "one";
                        return strNumber;
                    case 2:
                        strNumber = "two";
                        return strNumber;
                    case 3:
                        strNumber = "three";
                        return strNumber;
                    case 4:
                        strNumber = "four";
                        return strNumber;
                    case 5:
                        strNumber = "five";
                        return strNumber;
                    case 6:
                        strNumber = "six";
                        return strNumber;
                    case 7:
                        strNumber = "seven";
                        return strNumber;
                    case 8:
                        strNumber = "eight";
                        return strNumber;
                    case 9:
                        strNumber = "nine";
                        return strNumber;
                    case 10:
                        strNumber = "ten";
                        return strNumber;
                    case 11:
                        strNumber = "eleven";
                        return strNumber;
                    case 12:
                        strNumber = "twelve";
                        return strNumber;
                    case 13:
                        strNumber = "thirteen";
                        return strNumber;
                    case 14:
                        strNumber = "fourteen";
                        return strNumber;
                    case 15:
                        strNumber = "fifteen";
                        return strNumber;
                    case 16:
                        strNumber = "sixteen";
                        return strNumber;
                    case 17:
                        strNumber = "seventeen";
                        return strNumber;
                    case 18:
                        strNumber = "eighteen";
                        return strNumber;
                    case 19:
                        strNumber = "nineteen";
                        return strNumber;
                    default:
                        strNumber = "error";
                        return strNumber;
                }
            }

            if (number < 100)
            {
                if (number % 10 == 0)
                {
                    switch (number)
                    {
                        case 20:
                            strNumber = "twenty";
                            return strNumber;
                        case 30:
                            strNumber = "thirty";
                            return strNumber;
                        case 40:
                            strNumber = "forty";
                            return strNumber;
                        case 50:
                            strNumber = "fifty";
                            return strNumber;
                        case 60:
                            strNumber = "sixty";
                            return strNumber;
                        case 70:
                            strNumber = "seventy";
                            return strNumber;
                        case 80:
                            strNumber = "eighty";
                            return strNumber;
                        case 90:
                            strNumber = "ninety";
                            return strNumber;
                        default:
                            strNumber = "error";
                            return strNumber;
                    }
                }
                else
                {
                    strNumber = NumberToEnglishString(number / 10 * 10) + ' '
                            + NumberToEnglishString(number % 10);
                    return strNumber;
                }
            }

            if (number < 1000)
            {
                if (number % 100 == 0)
                {
                    strNumber = NumberToEnglishString(number / 100) + " hundred";
                    return strNumber;
                }
                else
                {
                    strNumber = NumberToEnglishString(number / 100) + " hundred and "
                            + NumberToEnglishString(number % 100);
                    return strNumber;
                }
            }

            if (number < 1000000)
            {
                if (number % 1000 == 0)
                {
                    strNumber = NumberToEnglishString(number / 1000) + " thousand";
                    return strNumber;
                }
                else
                {
                    strNumber = NumberToEnglishString(number / 1000) + " thousand "
                            + NumberToEnglishString(number % 1000);
                    return strNumber;
                }
            }

            if (number < 1000000000)
            {
                if (number % 1000000 == 0)
                {
                    strNumber = NumberToEnglishString(number / 1000000) + " million";
                    return strNumber;
                }
                else
                {
                    strNumber = NumberToEnglishString(number / 1000000) + " million "
                            + NumberToEnglishString(number % 1000000);
                    return strNumber;
                }
            }

            if (number < 999999999)
            {
                if (number % 1000000000 == 0)
                {
                    strNumber = NumberToEnglishString(number / 1000000000) + " billion";
                    return strNumber;
                }
                else
                {
                    strNumber = NumberToEnglishString(number / 1000000000) + " billion "
                            + NumberToEnglishString(number % 1000000000);
                    return strNumber;
                }
            }

            if (number > 999999999)
            {
                strNumber = "error";
                return strNumber;
            }
            return strNumber.ToUpper();
        }


        /// <summary>
        /// 向0行的datatable插入一个空行
        /// </summary>
        /// <param name="dt"></param>
        public static void AddNullRow(ref DataTable dt) {
            if (dt.Rows.Count == 0) {
                dt.Rows.Add(dt.NewRow());
                dt.AcceptChanges();
            }
        }
    }
}
