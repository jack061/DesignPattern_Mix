using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using WZX.Busines.DAL;
using WZX.Busines;
using RM.Busines;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common;
using System.Web.SessionState;
using RM.Common.DotNetBean;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// UserListHandler 的摘要说明
    /// </summary>
    public class ProductPriceListHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request["type"] == null ? "" : context.Request["type"].ToString();
            switch (type)
            {
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;
                case "edit":
                    context.Response.Write(getList(context));
                    break;
                case "save":
                    context.Response.Write(Save(context));
                    break;
                case "del":
                    context.Response.Write(Del(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            IList<SqlParam> IList_param = new List<SqlParam>();

            BaseData_Dal base_idao = new BaseData_Dal();

            DataTable dt = base_idao.GetProductPricePage(SqlWhere, IList_param, page, row, ref count);
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                sb.Append("{\"total\":" + count + ",");
                sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                sb.Append("}");
            }
            return sb.ToString();
        }
        private string Save(HttpContext context)
        {
            string code = context.Request.QueryString["code"];
            string name = context.Request.QueryString["name"];
            string price = context.Request.QueryString["price"];
            string pricedate = context.Request.QueryString["pricedate"];
            string des = context.Request.QueryString["des"];
            string status = context.Request.QueryString["status"];
            string methods = context.Request.QueryString["methods"];

            string sql = "";

            if (methods == "1")
            {
                sql = "Insert into bproductprice( code, name, price, pricedate, des, status, createman, lastmod) values('" + code + "', '" + name + "','" + price + "',  '" + pricedate + "', '" + des + "', '" + status + "','" + RequestSession.GetSessionUser().UserId + "', '" + RequestSession.GetSessionUser().UserId + "')";
            }
            else if (methods == "0")
            {
                sql = "Update bproductprice set  name='" + name + "',price='" + price + "', pricedate='" + pricedate + "', des='" + des + "',  lastmod='" + RequestSession.GetSessionUser().UserId + "', lastmoddate=getdate(), status='" + status + "' where  code = '" + code + "'";
            }
            List<string> list = new List<string>();
            list.Add(sql);
            if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0)
            {
                if (methods == "0")
                {
                    return "0";
                }
                else if (methods == "1")
                {//添加
                    return "1";
                }
                else
                {
                    return "false";
                }

            }
            else
            {
                return "false";
            }
        }
        private string Del(HttpContext context)
        {
            string code = context.Request.QueryString["code"];
            string sql = "delete from bproductprice where code='" + code + "'";

            List<string> list = new List<string>();
            list.Add(sql);
            if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0)
            {
                return "true";
                //context.Response.Write("true");
            }
            else
            {
                return "false";
                //context.Response.Write("false");
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