using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Create.Controllers
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // 获取Controller名称
            var controllerName = context.Request.QueryString["c"];
            // 声明IControoler接口-根据Controller Name找到对应的Controller
            IController controller = null;

            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = "home";
            }

            switch (controllerName.ToLower())
            {
                case "home":
                    controller = new HomeController();
                    break;
                default:
                    controller = new HomeController();
                    break;
            }

            controller.Execute(context);
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