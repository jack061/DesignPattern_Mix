using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RM.Busines.DAL.TrainApply;
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using RM.Common;
using RM.Busines.DAL;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetJson;
using WZX.Busines.Util;

namespace RM.Web.ashx.TrainApply
{
    /// <summary>
    /// UserListHandler 的摘要说明
    /// </summary>
    public class TrainApplyIListHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "application/json";
            context.Response.ContentType = "text/plain";
            string type = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (type)
            {
                case "getSubmitList"://获取列表
                    context.Response.Write(getSubmitList(context));
                    break;
                case "getSumList"://获取列表
                    context.Response.Write(getSumList(context));
                    break;
                case "delSubmitList"://获取列表
                    context.Response.Write(delSubmitList(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        /// <summary>
        /// 分页获取获取提交的列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubmitList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string filter = context.Request["filter"] == null ? null : context.Request["filter"].ToString();
            int count = 0;
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            if(!string.IsNullOrEmpty(filter))
                SqlWhere.Append(" and status='" + ConstantUtil.STATUS_NEW + "' ");
            IList<SqlParam> IList_param = new List<SqlParam>();

            TrainApplyI_Dal trainapply_idao = new TrainApplyI_Dal();

            DataTable dt = trainapply_idao.GetSubmitPage(SqlWhere, IList_param, page, row, ref count);

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
        /// 分页获取汇总列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSumList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = new TrainApplyI_Dal().GetSumPage(SqlWhere, IList_param, page, row, ref count);
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
        /// 删除提交列表的项
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string delSubmitList(HttpContext context) {
            Hashtable ht_result = new Hashtable();

            string applyNo = string.IsNullOrEmpty(context.Request["applyNo"]) ? "" : context.Request["applyNo"].ToString();
            StringBuilder sb = new StringBuilder();
            SqlParam[] param = { new SqlParam("@applyNo", applyNo) };
            sb.Append(" delete " + ConstantUtil.TABLE_TRAINAPPLYINEW + " where applyNo  = @applyNo and status='"+ConstantUtil.STATUS_NEW+"'");
            if (DataFactory.SqlDataBase().ExecuteBySql(sb, param) > 0)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "操作成功");
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！注意已汇总的提交单无法删除！");
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