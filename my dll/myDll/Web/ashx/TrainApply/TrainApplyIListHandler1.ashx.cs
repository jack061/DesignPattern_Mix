using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using System.Data;
using RM.Common;
using RM.Busines.DAL.TrainApply;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetJson;
using WZX.Busines.Util;

namespace RM.Web.ashx.TrainApply
{
    /// <summary>
    /// TrainApplyIListHandler1 的摘要说明
    /// </summary>
    public class TrainApplyIListHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string type = context.Request["type"] == null ? "" : context.Request["type"].ToString();
            switch (type){
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

            TrainApplyI_Dal trainapply_idao = new TrainApplyI_Dal();

            DataTable dt = trainapply_idao.GetTrainApplyIPage(SqlWhere, IList_param, page, row, ref count);

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