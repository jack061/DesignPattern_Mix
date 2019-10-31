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
using RM.Busines.DAL.TrainApply;
using RM.Common;
using RM.Busines.DAL.Inspect;
using System.Collections;
using WZX.Busines.Util;

namespace RM.Web.ashx.Inspect
{
    /// <summary>
    /// InspectListHandler 的摘要说明
    /// </summary>
    public class InspectListHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;
                case "del"://删除
                    context.Response.Write(del(context));
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

            //获取查询条件
            string inspectionno = (context.Request["INSPECTIONNO"] ?? "").ToString().Trim();
            string inspectnum = (context.Request["INSPECTNUM"] ?? "").ToString().Trim();
            string reinspectnum = (context.Request["REINSPECTNUM"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (inspectionno.Length > 0)
            {
                SqlWhere.Append(" and  inspectionNo like '%" + inspectionno + "%'");
            }
            if (inspectnum.Length > 0)
            {
                SqlWhere.Append(" and  inspectnum like '%" + inspectnum + "%'");
            }
            if (reinspectnum.Length > 0)
            {
                SqlWhere.Append(" and  reinspectnum like '%" + reinspectnum + "%'");
            }
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = Inspect_Dal.GetInspectListPage(SqlWhere, IList_param, page, row, ref count);
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
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string del(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();

            string inspectionNo = string.IsNullOrEmpty(context.Request["inspectionNo"]) ? "" : context.Request["inspectionNo"].ToString();
            StringBuilder sb = new StringBuilder();
            SqlParam[] param = { new SqlParam("@inspectionNo", inspectionNo) };
            sb.Append(" delete " + ConstantUtil.TABLE_INSPECTION + " where inspectionNo  = @inspectionNo");
            sb.Append(" delete " + ConstantUtil.TABLE_INSPECTIONPRODUCT + " where inspectionNo  = @inspectionNo");
            if (DataFactory.SqlDataBase().ExecuteBySql(sb, param) > 0)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "操作成功");
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);
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