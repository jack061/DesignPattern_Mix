using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RM.Common.DotNetData
{
    /// <summary>
    /// Hashtable 比较器
    ///
    /// </summary>
    public class HashtableComparer:IComparer<Hashtable>
    {
        private string sort;
        private string sorttype;
        private string datatype;
        public HashtableComparer() 
        {
            this.sort = "sortno";
            this.sorttype = "asc";
            datatype = "string";
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="sort">排序字段</param>
        /// <param name="sorttype">排序类型：asc/desc</param>
        /// <param name="datatype">排序字段数据类型：NUM/STRING</param>
        public HashtableComparer(string sort, string sorttype,string datatype)
        {
            this.sort = sort;
            this.sorttype = sorttype;
            this.datatype = datatype;
        }

        public int Compare(Hashtable x, Hashtable y) 
        {
            int result = 0;
            try
            {
                if ("NUM".Equals(datatype.ToUpper()))
                {
                    result = sorttype.ToLower().Equals("asc") ? float.Parse(x[sort].ToString()).CompareTo(float.Parse(y[sort].ToString())) : float.Parse(y[sort].ToString()).CompareTo(float.Parse(x[sort].ToString()));
                }
                else 
                {
                    result = sorttype.ToLower().Equals("asc") ? x[sort].ToString().CompareTo(y[sort].ToString()) : y[sort].ToString().CompareTo(x[sort].ToString());
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}
