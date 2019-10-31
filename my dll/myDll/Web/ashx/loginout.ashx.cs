using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using WZX.Busines.DAL;

namespace Survey.ashx
{
    /// <summary>
    /// loginout 的摘要说明
    /// </summary>
    public class loginout : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Session["User"] == null)
            {
                context.Response.Redirect("../Frame/login.html");
            }
            WZX.Model.View_Users viewUser = context.Session["User"] as WZX.Model.View_Users;
            Com_LoginLog lbll = new Com_LoginLog();
            WZX.Model.Com_LoginLog lmodel = new WZX.Model.Com_LoginLog();
            lmodel.LoginDate = DateTime.Now;
            lmodel.LoginIP =  HttpContext.Current.Request.UserHostAddress;
            lmodel.Status = "1";
            lmodel.Userid = viewUser.Userid;
            lbll.Add(lmodel);
            context.Session["User"] = null;
            context.Response.Redirect("../Frame/login.html");
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