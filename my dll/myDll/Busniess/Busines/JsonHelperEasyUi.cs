using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RM.Common;
using RM.Common.DotNetJson;

namespace RM.Busines
{
    public class JsonHelperEasyUi
    {
        public StringBuilder ToEasyUIDataGridJson(DataTable dt,List<String> outField)
        {
            StringBuilder sb = new StringBuilder();

            int count = dt.Rows.Count;
            sb.Append("{\"total\":" + count + ",\"rows\":[");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("{");
                foreach (string s in outField)
                {
                    sb.Append("\""+s+"\":\"" + dr[s].ToString() + "\",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");
            return sb;
        }

        public StringBuilder ToEasyUIComboxJson(DataTable dt, List<String> outField)
        {
            StringBuilder sb = new StringBuilder();

            int count = dt.Rows.Count;
            if (count > 0)
            {
                sb.Append("[");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    foreach (string s in outField)
                    {
                        sb.Append("\"" + s + "\":\"" + dr[s].ToString() + "\",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            else
            {
                sb.Append("[]");
            }
            return sb;
        }

        public StringBuilder ToEasyUIComboxJson(DataTable dt)
        {
            List<String> liscols = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                liscols.Add(col.ColumnName);
            }
            return ToEasyUIComboxJson(dt, liscols);
        }

        public StringBuilder ToEasyUIDataGridJson(DataTable dt)
        {
            //获取全部列
            List<String> liscols = new List<string>();
            foreach(DataColumn col in dt.Columns)
            {
                liscols.Add(col.ColumnName);
            }
            return ToEasyUIDataGridJson(dt,liscols);
        }

        public StringBuilder GetDatatableJsonString(StringBuilder sqlBuilder, SqlParameter[] sqlpps)
        {
            StringBuilder sb = new StringBuilder();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds1 = bll.ExecDatasetSql(sqlBuilder.ToString(),sqlpps);
                DataTable dt = ds1.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    sb.Append("{\"total\":0,\"rows\":[]}");
                }
                else
                {
                    int count = dt.Rows.Count;
                    sb.Append(JsonHelper.ToJson(dt, "rows"));
                    sb.Insert(1, "\"total\":" + count + ",");
                }
            }
            return sb;
        }

        public StringBuilder GetDatatablePageJsonString(StringBuilder sqlBuilder,StringBuilder sqlCount, SqlParameter[] sqlpps,string sort,string order,int page,int row)
        {
            StringBuilder sb = new StringBuilder();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds1 = bll.ExecDatasetSql(getPageSql1(sqlBuilder.ToString(), sort, order, page, row),sqlpps);
                DataTable dt = ds1.Tables[0];
               
                if (dt.Rows.Count == 0)
                {
                    sb.Append("{\"total\":0,\"rows\":[]}");
                }
                else
                {
                    int count = int.Parse(bll.ExecDatasetSql(sqlCount.ToString(),sqlpps).Tables[0].Rows[0][0].ToString());
                    sb.Append(JsonHelper.ToJson(dt, "rows"));
                    sb.Insert(1, "\"total\":" + count + ",");
                }
            }
            return sb;
        }

        public StringBuilder GetDatatablePageJsonString(StringBuilder sqlBuilder, StringBuilder sqlCount, SqlParameter[] sqlpps, string sort, int page, int row)
        {
            StringBuilder sb = new StringBuilder();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds1 = bll.ExecDatasetSql(getPageSql1(sqlBuilder.ToString(), sort, page, row), sqlpps);
                DataTable dt = ds1.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    sb.Append("{\"total\":0,\"rows\":[]}");
                }
                else
                {
                    int count = int.Parse(bll.ExecDatasetSql(sqlCount.ToString(), sqlpps).Tables[0].Rows[0][0].ToString());
                    sb.Append(JsonHelper.ToJson(dt, "rows"));
                    sb.Insert(1, "\"total\":" + count + ",");
                }
            }
            return sb;
        }

        /// <summary>
        /// 通用拼接分页查询字符串的方法
        /// </summary>
        /// <param name="oldSql"></param>
        /// <param name="sort"></param>
        /// <param name="orderType">asc desc</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private string getPageSql1(string oldSql, string sort, string orderType, int pageIndex, int pageSize)
        {
            StringBuilder sbNewSql = new StringBuilder(string.Empty);

            sbNewSql.Append(string.Format(@"select * from (  select tbZC321.*,Row_Number() over(order by {0} {1}) as rownum  
from  ({2}) tbZC321 )  tbAJK2 where tbAJK2.rownum>{3} and tbAJK2.rownum<={4} ", sort, orderType, oldSql, (pageIndex - 1) * pageSize, (pageIndex) * pageSize));

            return sbNewSql.ToString();
        }
        /// <summary>
        /// 通用拼接分页查询字符串的方法
        /// </summary>
        /// <param name="oldSql"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private string getPageSql1(string oldSql, string sort, int pageIndex, int pageSize)
        {
            StringBuilder sbNewSql = new StringBuilder(string.Empty);

            sbNewSql.Append(string.Format(@"select * from (  select tbZC321.*,Row_Number() over(order by {0}) as rownum  
from  ({1}) tbZC321 )  tbAJK2 where tbAJK2.rownum>{2} and tbAJK2.rownum<={3} order by {0}", sort, oldSql, (pageIndex - 1) * pageSize, (pageIndex) * pageSize));

            return sbNewSql.ToString();
        }

        public StringBuilder ToXml(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<xml>");
            sb.Append("<tables>");
            sb.Append("<table name=\""+dt.TableName+"\">");
            sb.Append("<rows>");

            int rowidnex = 0;
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("<row index={0}>", rowidnex);
                int j = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    sb.AppendFormat("<coldata index={0} name=\"{1}\">", j,col.ColumnName);
                    sb.Append("<![CDATA[" + dr[col].ToString() + "]]>");
                    sb.Append("</coldata>");
                    j++;
                }
                sb.Append("</row>");
                rowidnex++;
            }
            sb.Append("</rows>");
            sb.Append("</table>");
            sb.Append("</tables>");
            sb.Append("</xml>");

            return sb;
        }
    }
}
