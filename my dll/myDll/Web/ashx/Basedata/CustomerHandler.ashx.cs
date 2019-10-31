using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines.DAL;
using System.Text;
using RM.Busines;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// CustomerHandler 的摘要说明
    /// </summary>
    public class CustomerHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (type)
            {
                case "getComboList"://获取列表
                    context.Response.Write(getComboList(context));
                    break;
                case "GetSendmanList":
                    context.Response.Write(GetSendmanList(context));
                    break;
                case "add"://添加

                    break;
                case "edit"://修改

                    break;
                case "del"://删除

                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        private string getComboList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            DataTable dt = BaseData_Dal.GetCustomerList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\",");
                    sb.Append("\"SHORTNAME\":\"" + dt.Rows[i]["SHORTNAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据合同号获取客户发货人
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSendmanList(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];

            StringBuilder SqlWhere = new StringBuilder();
            StringBuilder sql = new StringBuilder(@"select bc.code,bd.name from bcustomer bc
                                                    join bcustomer_delivery bd on bd.code=bc.code 
                                                    join Econtract et on et.buyercode = bc.code or et.buyer=bc.name
                                                    where et.contractNo='" + contractNo+"'");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            StringBuilder sb = new StringBuilder("");
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
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