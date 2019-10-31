using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.DAL;
using RM.Common.DotNetEncrypt;

namespace Survey.ashx
{
    /// <summary>
    /// editpassword 的摘要说明
    /// </summary>
    public class editpassword : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string pass = context.Request.QueryString["newpass"];
            WZX.Model.View_Users viewUser = context.Session["User"] as WZX.Model.View_Users;
            Com_UserLogin bll = new Com_UserLogin();
            WZX.Model.Com_UserLogin model = bll.GetModel(viewUser.Userid);
            model.LoginPassword = DESEncrypt.Encrypt(pass);
            if (bll.Update(model))
            {
                context.Response.Write("true");

            }
            else {
                context.Response.Write("false");
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