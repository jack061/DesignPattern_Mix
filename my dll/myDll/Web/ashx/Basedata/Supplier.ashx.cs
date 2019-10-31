using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using RM.Busines;
using RM.Common.DotNetJson;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// Supplier 的摘要说明
    /// </summary>
    public class Supplier : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] ?? "";
            switch (action) {
                case "GetSupplierInfo"://获取（集团内管理）供应商的简称，全名和开户行
                    context.Response.Write(GetSupplierInfo(context));
                    break;
                case "GetSKInfo"://获取收款公司和公司简称
                    context.Response.Write(GetSKInfo(context));
                    break;
                case "GetSKBank"://根据公司名获取银行
                    context.Response.Write(GetSKBank(context));
                    break;
                case "GetSendSupplier":
                    context.Response.Write(GetSendSupplier(context));
                    break;
                default:
                    context.Response.Write("");
                    break;
            }
        }
        /// <summary>
        /// 废弃，不好使了
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSupplierInfo(HttpContext context) {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select shortname,name,icnbank
                        from bsupplier 
                        where property='集团内管理'");

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                return "[]";
            }
            else
            {
                return JsonHelper.DataTableToJson(dt, "rows");
            }
        }

        /// <summary>
        /// 获取公司和简称
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSKInfo(HttpContext context) {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select distinct NAME from dbo.BASE_DICTIONARY where PARENTID=121");

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                return "[]";
            }
            else
            {
                return JsonHelper.DataTableToJson(dt, "rows");
            }
        }

        /// <summary>
        /// 根据公司名获取银行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSKBank(HttpContext context)
        {
            string name = context.Request.QueryString["name"];

            StringBuilder sql = new StringBuilder();
            sql.Append(@"select distinct ENGLISH from dbo.BASE_DICTIONARY 
where PARENTID=121 and NAME='"+name+"'");

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                return "[]";
            }
            else
            {
                StringBuilder ret = new StringBuilder("[");
                for (int i = 0; i < dt.Rows.Count; i++) {
                    ret.Append(JsonHelper.DataRowToJson_(dt.Rows[i])+",");
                }
                ret = ret.Remove(ret.Length - 1, 1);
                ret.Append("]");
                return ret.ToString();
            }
        }

        /// <summary>
        /// 根据合同号获取发货人信息：发货申请部分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSendSupplier(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            StringBuilder sql = new StringBuilder(@"select code,name from bsupplier
                                                    select bs.code,bs.name 
                                                    from InspectionAppDetails insApp 
                                                    join bsupplier bs on insApp.sendMan=bs.name
                                                    where insApp.contractNo='"+contractNo+"'");
            DataSet ds = DataFactory.SqlDataBase().GetDataSetBySQL(sql);

            DataTable dt = ds.Tables[1].Rows.Count == 0 ? ds.Tables[0] : ds.Tables[1];

            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++){
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
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