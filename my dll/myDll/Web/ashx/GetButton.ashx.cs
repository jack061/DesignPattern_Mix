using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Text;
using WZX.Busines;
using RM.Busines;

namespace Survey.ashx
{
    /// <summary>
    /// GetButton 的摘要说明
    /// </summary>
    public class GetButton : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string pageName = context.Request.QueryString["pageName"];

            if (context.Session["User"] == null)
            {
                context.Response.Write("{\"flag\":true}");
            }
            else
            {
                WZX.Model.View_Users item = context.Session["User"] as WZX.Model.View_Users;
                int index = pageName.IndexOf('?', 0);
                string pageName_sub="";
                if (index > 0) {
                    pageName_sub = pageName.Substring(0, index);
                }
                DataTable dt_tb_navigation = DataFactory.SqlDataBaseExpand().GetDataTableBySQL(new StringBuilder("select Id from Tb_Navigation where LinkAddress like'%" + pageName_sub + "%'"));
                object NagId = null;
                if (dt_tb_navigation != null && dt_tb_navigation.Rows.Count > 1)
                {
                    NagId = DataFactory.SqlDataBaseExpand().GetSingle("select Id from Tb_Navigation where LinkAddress like'%" + pageName + "%'");
                }
                else 
                {
                    NagId = DataFactory.SqlDataBaseExpand().GetSingle("select Id from Tb_Navigation where LinkAddress like'%" + pageName_sub + "%'");
                    
                }
                DataSet ds = new DataSet();
                if (item.LoginName == "admin")
                {
                    ds = DataFactory.SqlDataBaseExpand().Query("select ButtonName,BtnCode,Icon from Com_ButtonGroup where Id in(select ButtonId from Com_NavigationAndButton where NavigationId=" + NagId + ") order by sort");
                }
                else
                {
                    object RoleId = DataFactory.SqlDataBaseExpand().GetSingle("select RolesId from Tb_RolesAddUser where UserId='" + item.Userid + "'");
                    ds = DataFactory.SqlDataBaseExpand().Query("select ButtonName,BtnCode,Icon from Com_ButtonGroup where Id in(select ButtonId from Tb_RolesAndNavigation where RolesId=" + RoleId + " and NavigationId=" + NagId + ") order by sort");
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("{\"ButtonName\":\"" + dr["ButtonName"] + "\",");
                    sb.Append("\"BtnCode\":\"" + dr["BtnCode"] + "\",");
                    sb.Append("\"Icon\":\"" + dr["Icon"] + "\"},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                context.Response.Write(sb.ToString());
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