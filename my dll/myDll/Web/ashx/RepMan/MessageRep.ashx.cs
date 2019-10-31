using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Busines;
using RM.Common.DotNetJson;
using WZX.Busines.BLL;

namespace LaoShanWeb.ashx.RepMan
{
    /// <summary>
    /// MessageRep 的摘要说明
    /// </summary>
    public class MessageRep : IHttpHandler
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
            using (SubInformationBll bll = new SubInformationBll())
            {
                //context.Response.Write(JsonHelper.ToJson(bll.GetMessage(strWhere).Tables[0]));
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