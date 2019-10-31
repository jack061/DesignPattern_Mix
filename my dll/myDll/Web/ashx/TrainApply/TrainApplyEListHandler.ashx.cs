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

namespace RM.Web.ashx.TrainApply
{
    /// <summary>
    /// UserListHandler 的摘要说明
    /// </summary>
    public class TrainApplyEListHandler : IHttpHandler
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

            DataTable dt = TrainApplyE_Dal.GetTrainApplyPage(SqlWhere, IList_param, page, row, ref count);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}