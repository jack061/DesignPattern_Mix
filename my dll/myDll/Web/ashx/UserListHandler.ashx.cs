using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using WZX.Busines.DAL;
using WZX.Busines;
using RM.Busines;
using RM.Common.DotNetEncrypt;

namespace Survey.ashx
{
    /// <summary>
    /// UserListHandler 的摘要说明
    /// </summary>
    public class UserListHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int orgId = 0;
            if (context.Request["OrgId"] != null)
            {
                orgId = int.Parse(context.Request["OrgId"].ToString());
            }
            string strWhere = "";
            Com_Organization obll = new Com_Organization();
            List<WZX.Model.Com_Organization> olist = obll.GetModelList("");
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(orgId);
            sb2 = GetId(orgId, sb2, olist);
            strWhere = "UserId in (select UserId from Com_OrgAddUser where OrgId in (" + sb2.ToString() + "))";
            View_Users bll = new View_Users();
            List<WZX.Model.View_Users> list = bll.GetLiGetModelListst(row, page, strWhere, " Userid desc");
            StringBuilder sb = new StringBuilder();
            if (list.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                int count = bll.Selcount(strWhere);
                sb.Append("{\"total\":" + count + ",\"rows\":[");
                foreach (WZX.Model.View_Users model in list)
                {
                    sb.Append("{\"Userid\":\"" + model.Userid + "\",");
                    sb.Append("\"LoginName\":\"" + model.LoginName + "\",");
                    sb.Append("\"UserRealName\":\"" + model.UserRealName + "\",");
                    sb.Append("\"Pass\":\"" + DESEncrypt.Decrypt(model.LoginPassword) + "\",");
                    //sb.Append("\"Pass\":\"" + model.LoginPassword + "\",");
                    sb.Append("\"Mail\":\"" + model.Email + "\",");
                    sb.Append("\"Mobile\":\"" + model.Mobile + "\",");
                    sb.Append("\"Tel\":\"" + model.Tel + "\",");
                    sb.Append("\"Sex\":\"" + model.Sex + "\",");
                    sb.Append("\"Status\":\"" + model.Status + "\",");
                    string sql = "select Agency from Com_Organization where Id = (select OrgId from Com_OrgAddUser where UserId=" + model.Userid + ")";
                    object obj = DataFactory.SqlDataBaseExpand().GetSingle(sql);
                    sb.Append("\"OrgId\":\"" + obj + "\",");
                    //string sql2 = "select Id,RolesName from Tb_Roles where Id = (select RolesId from Tb_RolesAddUser where UserId=" + model.Userid + ")";
                    //DataSet ds = DbHelperSQL.Query(sql2);
                    //sb.Append()
                    sb.Append("\"Email\":\"" + model.Email + "\",");
                    if (model.Status == 0)
                        sb.Append("\"Status\":\"禁用\"");
                    else if (model.Status == 1)
                        sb.Append("\"Status\":\"启用\"");
                    //else
                    //    sb.Append("\"Status\":\"禁止登录\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
            }
            context.Response.Write(sb.ToString());
        }
        public StringBuilder GetId(int ParentId, StringBuilder sb, List<WZX.Model.Com_Organization> list)
        {
            foreach (WZX.Model.Com_Organization item in list)
            {
                if (item.ParentId == ParentId)
                {
                    if (sb.Length != 0)
                        sb.Append(",");
                    sb.Append("'" + item.Id + "'");
                    sb = GetId(item.Id, sb, list);
                }
            }
            return sb;
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