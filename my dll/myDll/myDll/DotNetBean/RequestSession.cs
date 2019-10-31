using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace RM.Common.DotNetBean
{
    /// <summary>
    /// Session 帮助类
    /// </summary>
    public static class RequestSession//: IHttpHandler, IRequiresSessionState
    {
        private static string SESSION_USER = "SESSION_USER";
        private static  int EXPIRE =3*60 ;//分钟为单位

        public static void AddSessionUser(SessionUser user)
        {
            HttpContext.Current.Session[SESSION_USER] = user;
            HttpContext.Current.Session.Timeout = EXPIRE;  
        }
        public static SessionUser GetSessionUser()
        {
            if (HttpContext.Current.Session[SESSION_USER] == null)
            {
                return null;
            }
            else
            {
                return (SessionUser)HttpContext.Current.Session[SESSION_USER];
            }
        }
    }
}
