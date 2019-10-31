using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MVC_Create
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region 方式一：伪静态方式实现路由映射服务
            // 获得当前请求的URL地址
            var executePath = Request.AppRelativeCurrentExecutionFilePath;
            // 获得当前请求的参数数组
            var paraArray = executePath.Substring(2).Split('/');
            // 如果没有参数则执行默认配置
            if (string.IsNullOrEmpty(executePath) || executePath.Equals("~/") ||
                paraArray.Length == 0)
            {
                return;
            }

            string controllerName = "home";
            if (paraArray.Length > 0)
            {
                controllerName = paraArray[0];
            }
            string actionName = "index";
            if (paraArray.Length > 1)
            {
                actionName = paraArray[1];
            }

            // 入口一：单一入口 Index.ashx
            Context.RewritePath(string.Format("/Controllers/Index.ashx?controller={0}&action={1}", controllerName, actionName));
            // 入口二：指定MvcHandler进行后续处理
            //Context.RemapHandler(new MvcHandler());
            #endregion
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}