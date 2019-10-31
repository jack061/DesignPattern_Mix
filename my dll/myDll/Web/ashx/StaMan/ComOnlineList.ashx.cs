using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WZX.Busines.BLL;

namespace LaoShanWeb.ashx.StaMan
{
    /// <summary>
    /// ComOnlineList 的摘要说明
    /// </summary>
    public class ComOnlineList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string strWhere = "1=1 ";
            if (context.Request["starttime"] != null && context.Request["endtime"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["starttime"].ToString()) && (!string.IsNullOrEmpty(context.Request["endtime"].ToString())))
                {
                    strWhere += " and EventTime>='" + context.Request["starttime"].ToString() + " 00:00:00" + "' and EventTime<='" + context.Request["endtime"].ToString() + " 23:59:59" + "'";
                }
            }
            if (context.Request["CusUserCode"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["CusUserCode"].ToString()))
                {
                    strWhere += " and  tax.UserCode = '" + context.Request["CusUserCode"].ToString() + "'";
                }
            }
            if (context.Request["CustType"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["CustType"].ToString()))
                {
                    strWhere += " and  UserType like '%" + context.Request["CustType"].ToString() + "%'";
                }
            }
            if (context.Request["seltype"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["seltype"].ToString()))
                {
                    if (context.Request["seltype"].ToString() == "--全部--")
                    {
                        strWhere += " ";
                    }
                    else if (context.Request["seltype"].ToString() == "上线")
                    {
                        strWhere += " and EventType='上线' ";
                    }
                    else if (context.Request["seltype"].ToString() == "下线")
                    {
                        strWhere += " and EventType='下线' ";
                    }
                    
                }
            }
            context.Response.Write(GetOnlineList(row, page, strWhere, order, sort));
        }
        private string GetOnlineList(int row, int page, string strWhere, string order, string sort)
        {
            using (UserEventLogBll userBll = new UserEventLogBll())
            {

                return userBll.GetOnlineList(row, page, strWhere, order, sort);
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