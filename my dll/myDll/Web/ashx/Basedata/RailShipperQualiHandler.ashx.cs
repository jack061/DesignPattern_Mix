using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using RM.Busines.DAL;
using System.Data;
using RM.Common.DotNetJson;
using RM.Busines;
using WZX.Busines.Util;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// RailShipperQualiHandler 的摘要说明
    /// </summary>
    public class RailShipperQualiHandler : IHttpHandler
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
               // case "edit":
               //     context.Response.Write(getList(context));
               //     break;
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

            BaseData_Dal harbo_idao = new BaseData_Dal();

            DataTable dt = harbo_idao.GetRailShipperPage(SqlWhere, IList_param, page, row, ref count);
            StringBuilder sb = new StringBuilder();
            if (dt== null || dt.Rows.Count == 0)
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
            string sendPerson = context.Request.QueryString["sendPerson"];
            string adress1 = context.Request.QueryString["adress1"];
            string adress2 = context.Request.QueryString["adress2"];
            string adress3 = context.Request.QueryString["adress3"];
            string rusInfo = context.Request.QueryString["rusInfo"];
            string engInfo = context.Request.QueryString["engInfo"];
            string type = context.Request.QueryString["method"].Trim();

            string sql = "";
            if (string.Equals(type,"add"))
            {
                sql = string.Format("INSERT INTO {0}(SendPerson,Adress1,Adress2,Adress3,RusInfo,EngInfo) VALUES('{1}','{2}','{3}','{4}','{5}','{6}')",
                    ConstantUtil.TABLE_BRAILSHIPPERQUALI, sendPerson, adress1, adress2, adress3, rusInfo, engInfo);
            }
            else
            {
                string id = context.Request.QueryString["id"];
                sql = string.Format("UPDATE {0} SET SendPerson='{1}',Adress1='{2}',Adress2='{3}',Adress3='{4}',RusInfo='{5}',EngInfo='{6}' WHERE ID={7}",
                    ConstantUtil.TABLE_BRAILSHIPPERQUALI, sendPerson, adress1, adress2, adress3, rusInfo, engInfo, id);
            }

            List<string> list = new List<string>();
            list.Add(sql);

            return DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0 ? "true" : "false";
        }

        private string Del(HttpContext context)
        {
            string id = context.Request.QueryString["id"];
            string sql = string.Format("DELETE FROM {0} WHERE ID={1}",ConstantUtil.TABLE_BRAILSHIPPERQUALI,id);

            List<string> list = new List<string>();
            list.Add(sql);
            return DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0?"true":"false";
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