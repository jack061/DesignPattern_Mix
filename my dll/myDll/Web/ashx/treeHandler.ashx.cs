using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using WZX.Busines.DAL;

namespace Survey.ashx
{
    /// <summary>
    /// treeHandler 的摘要说明
    /// </summary>
    public class treeHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string UserId=context.Request.QueryString["Id"];
            Tb_Roles bll = new Tb_Roles();
            List<WZX.Model.Tb_Roles> list = bll.GetModelList("");
            Tb_RolesAddUser rbll = new Tb_RolesAddUser();
            WZX.Model.Tb_RolesAddUser model = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (WZX.Model.Tb_Roles item in list)
            {
                model = rbll.GetModel(item.Id,UserId);
                sb.Append("{\"id\":"+item.Id+",");
                sb.Append("\"text\":\""+item.RolesName+"\"");
                if (model!=null)
                {
                    sb.Append(",\"checked\":true");
                }
                sb.Append("},");
            }
            sb.Remove(sb.Length-1,1);
            sb.Append("]");
            context.Response.Write(sb.ToString());
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