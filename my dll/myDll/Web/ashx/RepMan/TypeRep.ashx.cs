using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Common.DotNetJson;
using WZX.Busines.BLL;

namespace LaoShanWeb.ashx.RepMan
{
    /// <summary>
    /// TypeRep 的摘要说明
    /// </summary>
    public class TypeRep : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string strWhere = "";
            if (context.Request["starttime"] != null && context.Request["endtime"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["starttime"].ToString()) && (!string.IsNullOrEmpty(context.Request["endtime"].ToString())))
                {
                    strWhere += " and users.EventTime>='" + context.Request["starttime"].ToString() + " 00:00:00" + "' and users.EventTime<='" + context.Request["endtime"].ToString() + " 23:59:59" + "'";
                }
            }
            using (UserEventLogBll bll = new UserEventLogBll())
            {
                context.Response.Write(JsonHelper.ToJson(bll.GetTypeOnline(strWhere).Tables[0]));
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