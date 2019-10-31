using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using WZX.Busines.DAL;

namespace Survey.ashx
{
    /// <summary>
    /// UserRoleList 的摘要说明
    /// </summary>
    public class UserRoleList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Tb_Roles bll = new Tb_Roles();
            List<WZX.Model.Tb_Roles> list = bll.GetModelList("");
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (WZX.Model.Tb_Roles item in list)
            {
                sb.Append("{\"Id\":" + item.Id + ",");
                sb.Append("\"Name\":\"" + item.RolesName + "\",");
                sb.Append("\"Remark\":\"" + item.Remark + "\"},");
            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
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