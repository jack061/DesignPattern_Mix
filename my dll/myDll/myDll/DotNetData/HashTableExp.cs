using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RM.Common.DotNetData
{
    public class HashTableExp : Hashtable,IComparable
    {
        private string sort;
        private string sorttype;
        public HashTableExp() 
        {
            this.sort = "sortno";
            this.sorttype = "asc";
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="sort">排序字段</param>
        /// <param name="sorttype">排序类型：asc/desc</param>
        public HashTableExp(string sort,string sorttype)
        {
            this.sort = sort;
            this.sorttype = sorttype;
        }

        public int CompareTo(object obj)
        {
            int result=0;
            try
            {
                HashTableExp info = obj as HashTableExp;
                result =  sorttype.ToLower().Equals("asc") ? this[sort].ToString().CompareTo(info[sort].ToString()) : info[sort].ToString().CompareTo(this[sort].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}
