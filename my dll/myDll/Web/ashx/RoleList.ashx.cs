using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using WZX.Busines.DAL;

namespace Survey.ashx
{
    /// <summary>
    /// RoleList 的摘要说明
    /// </summary>
    public class RoleList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string strWhere = "";
            Tb_Roles bll = new Tb_Roles();
            List<WZX.Model.Tb_Roles> list = bll.GetList_(row, page, strWhere, " Id desc");
            StringBuilder sb = new StringBuilder();
            if (list.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                int count = bll.Selcount("");
                sb.Append("{\"total\":" + count + ",\"rows\":[");
                foreach (WZX.Model.Tb_Roles model in list)
                {
                    sb.Append("{\"Id\":" + model.Id + ",");
                    sb.Append("\"Name\":\"" + model.RolesName + "\",");
                    sb.Append("\"Remark\":\"" + model.Remark + "\"},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
            }
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