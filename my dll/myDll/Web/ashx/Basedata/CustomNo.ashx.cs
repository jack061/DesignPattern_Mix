using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Busines;
using System.Data;
using RM.Common.DotNetJson;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// CustomNo 的摘要说明
    /// </summary>
    public class CustomNo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] ?? "";
            switch (action)
            {
                case "GetPreFix"://获取收款公司和公司简称
                    context.Response.Write(GetPreFix(context));
                    break;
                default:
                    context.Response.Write("");
                    break;
            }
        }
        
        /// <summary>
        /// 获取公司和简称
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPreFix(HttpContext context)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select distinct NAME,ID,CODE from dbo.BASE_DICTIONARY where PARENTID=154");

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                return "[]";
            }
            else
            {
                StringBuilder sb = new StringBuilder("[");
                foreach (DataRow dr in dt.Rows) {
                    sb.Append(JsonHelper.DataRowToJson_(dr)+",");
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                return sb.ToString();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}