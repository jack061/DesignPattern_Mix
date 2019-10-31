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
using RM.Common.DotNetData;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// UserListHandler 的摘要说明
    /// </summary>
    public class HarborListHandler : IHttpHandler, IRequiresSessionState
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
                case "getCombList"://获取列表
                    context.Response.Write(getCombList(context));
                    break;
                case "getHarborInfo"://获取港口信息
                    context.Response.Write(getHarborInfo(context));
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
            string country = context.Request.Params["country"] ?? "";
            string name = context.Request.Params["name"]??"";
            string code = context.Request.Params["code"]??"";


            StringBuilder SqlWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(country))
            {
                SqlWhere.Append("and country like '%" + country + "%' ");
            }
            if (!string.IsNullOrEmpty(name))
            {
                SqlWhere.Append("and name like '" + name + "%' ");
            }
            if (!string.IsNullOrEmpty(code))
            {
                SqlWhere.Append("and code like '" + code + "%' ");
            }
            SqlWhere.Append(" ");
            IList<SqlParam> IList_param = new List<SqlParam>();

            BaseData_Dal harbo_idao = new BaseData_Dal();

            DataTable dt = harbo_idao.GetHarborPage(SqlWhere, IList_param, page, row, ref count);
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

        /// <summary>
        /// 获取口岸
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getCombList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            DataTable dt = BaseData_Dal.GetHarborList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
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

        //获取港口信息
        private string getHarborInfo(HttpContext context)
        {
            DataTable dt = new DataTable();
            string code = context.Request.Params["code"] ?? "";
            string name = context.Request.Params["name"] ?? "";
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (code.Length > 0)
            {
                SqlWhere.Append(" and  code = '" + code + "'");
            }
            if (name.Length > 0)
            {
                SqlWhere.Append(" and  name = '" + name + "'");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from bharbor where 1=1 ")
                .Append(SqlWhere);

            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            StringBuilder sb = new StringBuilder();
            if (dt == null || !DataTableHelper.IsExistRows(dt))
            {
                sb.Append("{}");
            }
            else
            {
                DataRow dr = dt.Rows[0];
                sb.Append(JsonHelper.DataRowToJson_(dr));
            }
            return sb.ToString();
        }

        private string Save(HttpContext context)
        {
            string code = context.Request.QueryString["code"];
            string country = context.Request.QueryString["country"];
            string name = context.Request.QueryString["name"];
            string egname = context.Request.QueryString["egname"];
            string runame = context.Request.QueryString["runame"];
            
            string status = context.Request.QueryString["status"];
            string methods = context.Request.QueryString["methods"];

            string sql = "";

            if (methods == "1")
            {
                sql = "Insert into bharbor( code, country, name, egname, runame, status, createman, lastmod) values('" + code + "', '" + country + "', '" + name + "', '" + egname + "', '" + runame + "', '" + status + "','" + RequestSession.GetSessionUser().UserId + "', '" + RequestSession.GetSessionUser().UserId + "')";
            }
            else if (methods == "0")
            {
                sql = "Update bharbor set country='" + country + "', name='" + name + "', egname='" + egname + "', runame='" + runame + "',  lastmod='" + RequestSession.GetSessionUser().UserId + "', lastmoddate=getdate(), status='" + status + "' where  code = '" + code + "'";
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
            string sql = "delete from bharbor where code='" + code + "'";

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