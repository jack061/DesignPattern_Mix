using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WZX.Busines.BLL;
namespace LaoShanWeb.ashx.StaMan
{
    /// <summary>
    /// ReceiveList 的摘要说明
    /// </summary>
    public class ReceiveList : IHttpHandler
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
            if (context.Request["seltype"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["seltype"].ToString()))
                {
                    if (context.Request["seltype"].ToString() == "--全部--")
                    {
                        strWhere += " ";
                    }
                    else if (context.Request["seltype"].ToString() == "涉税信息")
                    {
                        strWhere += " and InfoType='涉税信息' ";
                    }
                    else if (context.Request["seltype"].ToString() == "政策发布")
                    {
                        strWhere += " and InfoType='政策发布' ";
                    }
                    else if (context.Request["seltype"].ToString() == "申报提醒")
                    {
                        strWhere += " and InfoType='申报提醒' ";
                    }
                    else if (context.Request["seltype"].ToString() == "风险提醒")
                    {
                        strWhere += " and InfoType='风险提醒' ";
                    }
                }
            }
            if (context.Request["selIsGet"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["selIsGet"].ToString()))
                {
                    if (context.Request["selIsGet"].ToString() == "--全部--")
                    {
                        strWhere += " ";
                    }
                    else if (context.Request["selIsGet"].ToString() == "已接收")
                    {
                        strWhere += " and IsGet= 1";
                    }
                    else if (context.Request["selIsGet"].ToString() == "未接收")
                    {
                        strWhere += " and IsGet = 0 ";
                    } 
                }
            }
            context.Response.Write(GetReceiveList(row, page, strWhere, order, sort));
        }
        private string GetReceiveList(int row, int page, string strWhere, string order, string sort)
        {
            using (SubInformationBll subBll = new SubInformationBll())
            {

                return subBll.GetReceiveList(row, page, strWhere, order, sort);
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