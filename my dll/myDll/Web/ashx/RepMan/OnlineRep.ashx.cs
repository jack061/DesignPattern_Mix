using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Common.DotNetJson;
using WZX.Busines.BLL;

namespace LaoShanWeb.ashx.RepMan
{
    /// <summary>
    /// OnlineRep 的摘要说明
    /// </summary>
    public class OnlineRep : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string ctype = context.Request["ctype"].ToString();
            string strWhere = "where EventType = '上线'";
            if (context.Request["begintime"] != null && context.Request["endtime"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["begintime"].ToString()) && (!string.IsNullOrEmpty(context.Request["endtime"].ToString())))
                {
                    strWhere += " and EventTime>='" + context.Request["begintime"].ToString() + " 00:00:00" + "' and EventTime<='" + context.Request["endtime"].ToString() + " 23:59:59" + "'";
                }
            }
            if (context.Request["sztype"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["sztype"].ToString()))
                {
                    if (context.Request["sztype"].ToString() == "--全部--")
                    {
                        strWhere += " ";
                    }
                    else if (context.Request["sztype"].ToString() == "上线")
                    {
                        strWhere += " and EventType='上线' ";
                    }
                    else if (context.Request["sztype"].ToString() == "下线")
                    {
                        strWhere += " and EventType='下线' ";
                    }
                }
            }
            using (UserEventLogBll bll = new UserEventLogBll())
            {
                if (ctype == "yue")
                {
                    context.Response.Write(JsonHelper.ToJson(bll.getMongthRep(strWhere).Tables[0]));
                }
                else if (ctype == "zhou")
                {
                    context.Response.Write(JsonHelper.ToJson(bll.getWeekRep(strWhere).Tables[0]));
                }
                else if (ctype == "tian")
                {
                    context.Response.Write(JsonHelper.ToJson(bll.getDayRep(strWhere).Tables[0]));
                }

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