using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;
using RM.Busines;
using System.Collections;

namespace WZX.Busines.Util
{
    public class SqlUtil
    {
        /// <summary>
        /// 根据DataTable获取批处理sql语句以及参数
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dataTable">数据表</param>
        /// <param name="key">主键</param>
        /// <param name="sqls">sql语句数组</param>
        /// <param name="objs">参数数组</param>
        public static void getBatchFromDataTable(DataTable dt,string dataTable, string key, ref StringBuilder[] sqls, ref object[] objs) 
        {
            if(dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count;i++ ) 
                {
                    string keyValue=dt.Rows[i][key].ToString();
                    int flag = DataFactory.SqlDataBase().IsExist(dataTable, key, keyValue);
                    if (flag == 0)
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("Insert into " + dataTable + "(");
                        for (int j = 0; j < dt.Columns.Count - 1; j++)
                        {
                            sbadd.Append(getColumn(dt.Columns[j].ColumnName) + ",");
                        }
                        sbadd.Append(getColumn(dt.Columns[dt.Columns.Count - 1].ColumnName) + ")");
                        sbadd.Append(" values(");
                        for (int j = 0; j < dt.Columns.Count - 1; j++)
                        {
                            sbadd.Append("@" + getColumn(dt.Columns[j].ColumnName) + ",");
                        }
                        sbadd.Append("@" + getColumn(dt.Columns[dt.Columns.Count - 1].ColumnName) + ")");
                        sqls[i] = sbadd;

                        //参数
                        SqlParam[] parmAdd = new SqlParam[dt.Columns.Count];
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            parmAdd[j] = new SqlParam("@" + getColumn(dt.Columns[j].ColumnName), getValue(dt.Rows[i][getColumn(dt.Columns[j].ColumnName)].ToString()));
                        }
                        objs[i] = parmAdd;
                    }
                    else 
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("update " + dataTable + " set ");
                        for (int j = 0; j < dt.Columns.Count - 1; j++)
                        {
                            sbadd.Append(getColumn(dt.Columns[j].ColumnName) + "= @" + getColumn(dt.Columns[j].ColumnName)+",");
                        }
                        sbadd.Append(getColumn(dt.Columns[dt.Columns.Count - 1].ColumnName) + "= @" + getColumn(dt.Columns[dt.Columns.Count - 1].ColumnName));
                        sbadd.Append(" where "+ key +"=@"+key);
                        
                        sqls[i] = sbadd;

                        //参数
                        SqlParam[] parmAdd = new SqlParam[dt.Columns.Count];
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            parmAdd[j] = new SqlParam("@" + getColumn(dt.Columns[j].ColumnName), getValue(dt.Rows[i][getColumn(dt.Columns[j].ColumnName)].ToString()));
                        }
                        objs[i] = parmAdd;
                    }
                    

                }
            }
        }

        /// <summary>
        /// 根据DataTable获取批处理sql语句以及参数
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dataTable">数据表</param>
        /// <param name="key">主键</param>
        /// <param name="sqls">sql语句数组</param>
        /// <param name="objs">参数数组</param>
        public static void getBatchFromDataTable(DataTable dt, string dataTable, ref StringBuilder[] sqls, ref object[] objs)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StringBuilder sbadd = new StringBuilder();
                    sbadd.Append("Insert into " + dataTable + "(");
                    for (int j = 0; j < dt.Columns.Count - 1; j++)
                    {
                        sbadd.Append(getColumn(dt.Columns[j].ColumnName) + ",");
                    }
                    sbadd.Append(getColumn(dt.Columns[dt.Columns.Count - 1].ColumnName) + ")");
                    sbadd.Append(" values(");
                    for (int j = 0; j < dt.Columns.Count - 1; j++)
                    {
                        sbadd.Append("@" + getColumn(dt.Columns[j].ColumnName) + ",");
                    }
                    sbadd.Append("@" + getColumn(dt.Columns[dt.Columns.Count - 1].ColumnName) + ")");
                    sqls[i] = sbadd;

                    //参数
                    SqlParam[] parmAdd = new SqlParam[dt.Columns.Count];
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        parmAdd[j] = new SqlParam("@" + getColumn(dt.Columns[j].ColumnName), getValue(dt.Rows[i][getColumn(dt.Columns[j].ColumnName)].ToString()));
                    }
                    objs[i] = parmAdd;

                }
            }
        }


        /// <summary>
        /// 根据List获取批处理sql语句以及参数
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dataTable">数据表</param>
        /// <param name="key">主键</param>
        /// <param name="sqls">sql语句数组</param>
        /// <param name="objs">参数数组</param>
        public static void getBatchFromList(List<Hashtable> list, string tableName, ref StringBuilder[] sqls, ref object[] objs)
        {
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Hashtable ht = list[i];
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Insert Into ");
                    sb.Append(tableName);
                    sb.Append("(");
                    StringBuilder sp = new StringBuilder();
                    StringBuilder sb_prame = new StringBuilder();
                    foreach (string key in ht.Keys)
                    {
                        sb_prame.Append("," + key);
                        sp.Append(",@" + key);
                    }
                    sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
                    sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
                    sqls[i] = sb;
                    objs[i] = GetParameter(ht);
                }
            }
        }

        public static void getBatchFromListStandard(List<Hashtable> list, string tableName, ref StringBuilder[] sqls, ref object[] objs)
        {
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Hashtable ht = list[i];
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Insert Into ");
                    sb.Append(tableName);
                    sb.Append("(");
                    StringBuilder sp = new StringBuilder();
                    StringBuilder sb_prame = new StringBuilder();
                    foreach (string key in ht.Keys)
                    {
                        sb_prame.Append("," + key);
                        sp.Append(",@" + key);
                    }
                    sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
                    sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
                    sqls[i] = sb;
                    objs[i] = GetParameterStandrad(ht);
                }
            }
        }

        public static void getBatchFromListStandardUpdate(List<Hashtable> list, string tableName, string strwhere, ref StringBuilder[] sqls, ref object[] objs)
        {
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Hashtable ht = list[i];
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Update ");
                    sb.Append(tableName);
                    sb.Append(" set ");
                    IDictionaryEnumerator id = ht.GetEnumerator();
                    while (id.MoveNext())
                    {
                        sb.Append(id.Key + " = @" + id.Key + ",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" where " + strwhere);

                    sqls[i]=sb;
                    objs[i]=GetParameterStandrad(ht);
                }
            }
        }

        /// <summary>
        /// 根据List获取批处理sql语句以及参数
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dataTable">数据表</param>
        /// <param name="key">主键</param>
        /// <param name="sqls">sql语句数组</param>
        /// <param name="objs">参数数组</param>
        public static void getBatchFromList(List<Hashtable> list, string tableName, string tableKey, string keyValue, ref StringBuilder[] sqls, ref object[] objs)
        {
            if (list.Count > 0)
            {
                StringBuilder sb1 = new StringBuilder();
                sb1.Append("delete ");
                sb1.Append(tableName);
                sb1.Append(" where "+tableKey+" =@");
                sb1.Append(tableKey+" ");
                sqls[0] = sb1;
                objs[0] = new SqlParam[] { new SqlParam("@" + tableKey, keyValue) };
                for (int i = 0; i < list.Count; i++)
                {
                    Hashtable ht = list[i];
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Insert Into ");
                    sb.Append(tableName);
                    sb.Append("(");
                    StringBuilder sp = new StringBuilder();
                    StringBuilder sb_prame = new StringBuilder();
                    foreach (string key in ht.Keys)
                    {
                        sb_prame.Append("," + key);
                        sp.Append(",@" + key);
                    }
                    sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
                    sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
                    sqls[i+1] = sb;
                    objs[i+1] = GetParameter(ht);
                }
            }
        }

        /// <summary>
        /// 根据List获取批处理sql语句以及参数
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dataTable">数据表</param>
        /// <param name="key">主键</param>
        /// <param name="sqls">sql语句数组</param>
        /// <param name="objs">参数数组</param>
        public static void getBatchSqls(List<Hashtable> list, string tableName, string tableKey, string keyValue, ref List<StringBuilder> sqls, ref List<object> objs)
        {
            if (list.Count > 0)
            {
                StringBuilder sb1 = new StringBuilder();
                sb1.Append("delete ");
                sb1.Append(tableName);
                sb1.Append(" where " + tableKey + " =@");
                sb1.Append(tableKey + " ");
                sqls.Add(sb1);
                objs.Add( new SqlParam[] { new SqlParam("@" + tableKey, keyValue) });
                for (int i = 0; i < list.Count; i++)
                {
                    Hashtable ht = list[i];
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Insert Into ");
                    sb.Append(tableName);
                    sb.Append("(");
                    StringBuilder sp = new StringBuilder();
                    StringBuilder sb_prame = new StringBuilder();
                    foreach (string key in ht.Keys)
                    {
                        sb_prame.Append("," + key);
                        sp.Append(",@" + key);
                    }
                    sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
                    sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
                    sqls.Add(sb);
                    objs.Add(GetParameter(ht));
                }
            }
        }
        /// <summary>
        /// 根据List获取批处理sql语句以及参数
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dataTable">数据表</param>
        /// <param name="key">主键</param>
        /// <param name="sqls">sql语句数组</param>
        /// <param name="objs">参数数组</param>
        public static void getBatchSqls(Hashtable ht, string tableName, string tableKey, string keyValue, ref List<StringBuilder> sqls, ref List<object> objs)
        {
            //判断是否存在该条记录

            if (DataFactory.SqlDataBase().IsExist(tableName, tableKey, keyValue) <= 0)
            {//不存在生成插入
                StringBuilder sb = new StringBuilder();
                sb.Append(" Insert Into ");
                sb.Append(tableName);
                sb.Append("(");
                StringBuilder sp = new StringBuilder();
                StringBuilder sb_prame = new StringBuilder();
                foreach (string key in ht.Keys)
                {
                    sb_prame.Append("," + key);
                    sp.Append(",@" + key);
                }
                sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
                sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
                sqls.Add(sb);
                objs.Add(GetParameter(ht));
            }
            else
            {//存在生成更新
                StringBuilder sb = new StringBuilder();
                sb.Append(" Update ");
                sb.Append(tableName);
                sb.Append(" set ");
                IDictionaryEnumerator id = ht.GetEnumerator();
                while (id.MoveNext()) 
                {
                    sb.Append(id.Key+" = @"+id.Key+",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" where " + tableKey + " = '" + keyValue + "'");
                sqls.Add(sb);
                objs.Add(GetParameter(ht));
            }
        }
        public static void getBatchSqls(Hashtable ht, Hashtable ht_prime, string tableName, ref List<StringBuilder> sqls, ref List<object> objs)
        {
            //判断是否存在该条记录
            if (DataFactory.SqlDataBase().IsExist3(tableName,ht_prime) <= 0)
            {//不存在生成插入
                StringBuilder sb = new StringBuilder();
                sb.Append(" Insert Into ");
                sb.Append(tableName);
                sb.Append("(");
                StringBuilder sp = new StringBuilder();
                StringBuilder sb_prame = new StringBuilder();
                foreach (string key in ht.Keys)
                {
                    sb_prame.Append("," + key);
                    sp.Append(",@" + key);
                }
                sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
                sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
                sqls.Add(sb);
                objs.Add(GetParameter(ht));
            }
            else
            {//存在生成更新
                StringBuilder sb = new StringBuilder();
                sb.Append(" Update ");
                sb.Append(tableName);
                sb.Append(" set ");
                IDictionaryEnumerator id = ht.GetEnumerator();
                while (id.MoveNext())
                {
                    sb.Append(id.Key + " = @" + id.Key + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" where 1=1 and ");
             
                foreach (DictionaryEntry item in ht_prime)
                {
                    sb.Append(item.Key + "=@" + item.Key + " and ");
                }
                if (ht_prime.Count > 1)
                {
                    sb.Remove(sb.ToString().LastIndexOf("and"), 3);
                }
                sqls.Add(sb);
                objs.Add(GetParameter(ht));
            }
        }
        /// <summary>
        /// 根据List获取批处理sql语句以及参数,无删除语句
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dataTable">数据表</param>
        /// <param name="key">主键</param>
        /// <param name="sqls">sql语句数组</param>
        /// <param name="objs">参数数组</param>
        public static void getBatchSqls(List<Hashtable> list, string tableName, ref List<StringBuilder> sqls, ref List<object> objs)
        {
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Hashtable ht = list[i];
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Insert Into ");
                    sb.Append(tableName);
                    sb.Append("(");
                    StringBuilder sp = new StringBuilder();
                    StringBuilder sb_prame = new StringBuilder();
                    foreach (string key in ht.Keys)
                    {
                        sb_prame.Append("," + key);
                        sp.Append(",@" + key);
                    }
                    sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
                    sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
                    sqls.Add(sb);
                    objs.Add(GetParameter(ht));
                }
            }
        }

        #region 辅助
        public static string getColumn(string column) 
        {
            string temp = column;
            if(!(string.IsNullOrEmpty(temp)))
            {
                int index = temp.IndexOf("<");
                if (index > 0) 
                {
                    temp = temp.Substring(0,index);
                }
            }
            return temp;
        }
        public static string getValue(string value)
        {
            string temp = value;
            if ((string.IsNullOrEmpty(temp)))
            {
                temp = null;   
            }
            return temp;
        }
        /// <summary>
        /// 对象参数转换
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static SqlParam[] GetParameter(Hashtable ht)
        {
            SqlParam[] _params = new SqlParam[ht.Count];
            int i = 0;
            foreach (string key in ht.Keys)
            {
                _params[i] = new SqlParam("@" + key, ht[key]);
                i++;
            }
            return _params;
        }

        public static System.Data.SqlClient.SqlParameter[] GetParameterStandrad(Hashtable ht)
        {
            System.Data.SqlClient.SqlParameter[] _params = new System.Data.SqlClient.SqlParameter[ht.Count];
            int i = 0;
            foreach (string key in ht.Keys)
            {
                _params[i] = new System.Data.SqlClient.SqlParameter("@" + key, ht[key]);
                i++;
            }
            return _params;
        }

        #endregion
    }
}
