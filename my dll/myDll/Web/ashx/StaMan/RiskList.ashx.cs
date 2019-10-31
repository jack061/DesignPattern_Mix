using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WZX.Busines.BLL;

namespace LaoShanWeb.ashx.StaMan
{
    /// <summary>
    /// RiskList 的摘要说明
    /// </summary>
    public class RiskList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string strWhere = "inf.InfoType = '风险提醒'";
            if (context.Request["starttime"] != null && context.Request["endtime"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["starttime"].ToString()) && (!string.IsNullOrEmpty(context.Request["endtime"].ToString())))
                {
                    strWhere += " and CreateTime>='" + context.Request["starttime"].ToString() + " 00:00:00" + "' and CreateTime<='" + context.Request["endtime"].ToString() + " 23:59:59" + "'";
                }
            }
            if (context.Request["txtCusNo"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["txtCusNo"].ToString()))
                {
                    strWhere += " and  tax.UserCode = '" + context.Request["txtCusNo"].ToString() + "'";
                }
            }
            if (context.Request["selIsDone"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["selIsDone"].ToString()))
                {
                    if (context.Request["selIsDone"].ToString() == "--全部--")
                    {
                        strWhere += " ";
                    }
                    else if (context.Request["selIsDone"].ToString() == "已完成")
                    {
                        strWhere += " and IsDone= 1";
                    }
                    else if (context.Request["selIsDone"].ToString() == "未完成")
                    {
                        strWhere += " and IsDone = 0 ";
                    }
                }
            }
            context.Response.Write(GetRiskList(row, page, strWhere, order, sort));
        }
        private string GetRiskList(int row, int page, string strWhere, string order, string sort)
        {
            using (SubInformationBll subBll = new SubInformationBll())
            {

                return subBll.GetRiskList(row, page, strWhere, order, sort);
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