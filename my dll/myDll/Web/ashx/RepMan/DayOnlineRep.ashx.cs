using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WZX.Busines.BLL;
using RM.Common.DotNetJson;

namespace LaoShanWeb.ashx.RepMan
{
    /// <summary>
    /// DayOnlineRep 的摘要说明
    /// </summary>
    public class DayOnlineRep : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string strWhere = "";
            if (context.Request["starttime"] != null )
            {
                if (!string.IsNullOrEmpty(context.Request["starttime"].ToString()))
                {
                    strWhere += " and EventTime>='" + context.Request["starttime"].ToString() + " 00:00:00" + "' and EventTime<='" + context.Request["starttime"].ToString() + " 23:59:59" + "'";
                }
            }

            using (UserEventLogBll bll = new UserEventLogBll())
            {
                context.Response.Write(JsonHelper.ToJson(bll.GetDayOnline(strWhere).Tables[0]));
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