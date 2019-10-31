using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;

namespace RM.Web.Bus.Lib
{
    public class InsteadString
    {
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\$BEGIN_[^\$]+_END\$)", System.Text.RegularExpressions.RegexOptions.Compiled);
        public void InsteadStringBuilder(StringBuilder sb, Hashtable htdata)
        {
            //处理字符串变换
            var matchs = reg.Matches(sb.ToString());
            //偏移位置，因为有时候替换字符串和原字符串的长度不一致
            int pyindex = 0;
            foreach (System.Text.RegularExpressions.Match mat in matchs)
            {
                if (mat.Success == true)
                {
                    string old = mat.ToString();
                    string[] aaold = old.Split('_');
                    if (aaold.Length == 3)
                    {
                        if (htdata.ContainsKey(aaold[1]))
                        {
                            string value = htdata[aaold[1]].ToString();

                            //删除之后再添加
                            sb.Remove(mat.Index + pyindex, mat.Length);
                            sb.Insert(mat.Index + pyindex, value);

                            pyindex += value.Length - mat.Length;
                        }
                    }
                }
            }
        }

        #region 实现单例模式
        private static readonly object lockobj = new object();
        private static InsteadString insteadStr = null;
        private InsteadString()
        { 
            
        }

        public static InsteadString Singleton
        {
            get
            {
                if (insteadStr == null)
                {
                    lock (lockobj)
                    {
                        if (insteadStr == null)
                        {
                            insteadStr = new InsteadString();
                        }
                    }
                }
                return insteadStr;
            }
        }
        #endregion
    }

    public class InsteadLabelString
    {
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\{[^\{]+\})", System.Text.RegularExpressions.RegexOptions.Compiled);
        /// <summary>
        /// 替换标签（多语言）
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="htdata">键值对</param>
        /// <param name="lans">举例：俄文,中文,英文</param>
        public void InsteadStringBuilder(StringBuilder sb, Hashtable htdata,string lans)
        {
            //处理字符串变换
            var matchs = reg.Matches(sb.ToString());
            //偏移位置，因为有时候替换字符串和原字符串的长度不一致
            int pyindex = 0;
            foreach (System.Text.RegularExpressions.Match mat in matchs)
            {
                if (mat.Success == true)
                {
                    string old = mat.ToString().TrimStart('{').TrimEnd('}');
                    if (old.StartsWith("英文:") || old.StartsWith("中文:") || old.StartsWith("俄文:") || old.StartsWith("不限:"))
                    {
                        string[] aaold = old.Split(':');
                        if (aaold.Length == 2)
                        {
                            if (htdata.ContainsKey(old))
                            {
                                string value = "";

                                if (aaold[0] == "不限" || aaold[0].Length > 0 && lans.Contains(aaold[0]))
                                {
                                    value = htdata[old].ToString();
                                }
                                //删除之后再添加
                                //begin 原子操作
                                sb.Remove(mat.Index + pyindex, mat.Length);
                                sb.Insert(mat.Index + pyindex, value);
                                pyindex += value.Length - mat.Length;
                                //end
                            }
                            else
                            {
                                string value = "";

                                //删除之后再添加
                                //begin 原子操作
                                sb.Remove(mat.Index + pyindex, mat.Length);
                                sb.Insert(mat.Index + pyindex, value);
                                pyindex += value.Length - mat.Length;
                                //end
                            }
                        }
                    }
                    else if (old.StartsWith("英文=") || old.StartsWith("中文=") || old.StartsWith("俄文=") || old.StartsWith("不限="))
                    {
                        string str1 = old.Substring(0, 2);
                        string str2 = old.Substring(3, old.Length - 3);
                        string value = "";
                        //說明是中文合同
                        //if (!lans.Contains("-"))
                        //{
                        //    if (!old.StartsWith("中文="))
                        //    {
                        //        sb.Remove(mat.Index + pyindex, mat.Length);

                        //    }
                        //    else
                        //    {
                        //        value = str2;
                        //    }

                        //}  else 

                      if (lans.Contains(str1) || str1 == "不限")
                        {
                            value = str2;
                          
                        }
                        //删除之后再添加
                        //begin 原子操作
                        sb.Remove(mat.Index + pyindex, mat.Length);
                        sb.Insert(mat.Index + pyindex, value);
                        pyindex += value.Length - mat.Length;
                        //end
                    }
                }
            }
        }

        #region 实现单例模式
        private static readonly object lockobj = new object();
        private static InsteadLabelString insteadStr = null;
        private InsteadLabelString()
        {

        }

        public static InsteadLabelString Singleton
        {
            get
            {
                if (insteadStr == null)
                {
                    lock (lockobj)
                    {
                        if (insteadStr == null)
                        {
                            insteadStr = new InsteadLabelString();
                        }
                    }
                }
                return insteadStr;
            }
        }
        #endregion
    }
}