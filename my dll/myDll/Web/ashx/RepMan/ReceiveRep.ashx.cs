using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WZX.Busines.BLL;
using RM.Common.DotNetJson;

namespace LaoShanWeb.ashx.RepMan
{
    /// <summary>
    /// ReceiveRep 的摘要说明
    /// </summary>
    public class ReceiveRep : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string strWhere = "";
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
                    strWhere += " and UserCode = '" + context.Request["txtCusNo"].ToString() + "'";
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
            using (SubInformationBll bll = new SubInformationBll())
            {
                context.Response.Write(JsonHelper.ToJson(bll.GetReceive(strWhere).Tables[0]));
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