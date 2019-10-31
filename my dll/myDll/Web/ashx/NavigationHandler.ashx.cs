using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;
using WZX.Busines.DAL;

namespace Survey.ashx
{
    /// <summary>
    /// NavigationHandler 的摘要说明
    /// </summary>
    public class NavigationHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"]?? "";
            switch (action) 
            {
                case "treeMenu"://获取树形菜单
                    context.Response.Write(treeMenu(context));
                    break;
                default://默认
                    context.Response.Write(defaultMenu(context));
                    break;
            }
        }

        private string treeMenu(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            Tb_Navigation bll = new Tb_Navigation();
            List<WZX.Model.Tb_Navigation> list = new List<WZX.Model.Tb_Navigation>();
            WZX.Model.View_Users item = context.Session["User"] as WZX.Model.View_Users;
            string strWhere = null;
            if (item.UserRealName == "管理员")
            {
                strWhere = " IsShow=0";
            }
            else
            {
                strWhere = "Id in(select NavigationId from Tb_RolesAndNavigation where RolesId in(select RolesId from Tb_RolesAddUser where UserId='" + item.Userid + "')) and IsShow=0";
            }
            DataSet ds = bll.GetList(strWhere);
            sb.Append(" [");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = " ParentId=0";
                dv.Sort = "Sort";
                for (int i = 0; i < dv.Count; i++)
                {
                    if (i == 0)
                    {
                        //sb.Append("{\"id\":\"" + dv[i]["Id"] + "\",\"iconCls\":\"" + dv[i]["Icon"] + "\",\"text\":\"" + dv[i]["Pagelogo"] + "\",\"state\": \"open\",");
                        sb.Append("{\"id\":\"" + dv[i]["Id"] + "\",\"text\":\"" + dv[i]["Pagelogo"] + "\",\"state\": \"open\",");
                    }
                    else 
                    {
                        //sb.Append("{\"id\":\"" + dv[i]["Id"] + "\",\"iconCls\":\"" + dv[i]["Icon"] + "\",\"text\":\"" + dv[i]["Pagelogo"] + "\",\"state\": \"closed\",");
                        sb.Append("{\"id\":\"" + dv[i]["Id"] + "\",\"text\":\"" + dv[i]["Pagelogo"] + "\",\"state\": \"closed\",");
                    }
                    
                    sb.Append("\"children\":[");
                    //sb.Append("{" + dv[i]["Pagelogo"] + "," + dv[i]["Icon"] + ",");
                    //sb.Append("<ul>");
                    DataView dv2 = new DataView(ds.Tables[0]);
                    dv2.RowFilter = " ParentId=" + dv[i]["Id"];
                    dv2.Sort = " Sort";
                    for (int j = 0; j < dv2.Count; j++)
                    {
                        sb.Append("{\"id\":\"" + dv2[j]["Id"] + "\",\"text\":\"" + dv2[j]["Pagelogo"] + "\",\"iconCls\":\"" + dv2[j]["Icon"].ToString() + "\",\"attributes\":{\"url\":\"" + dv2[j]["LinkAddress"] + "\"}},");
                        //sb.Append("<li><div><a ref=\"" + dv2[j]["Pagelogo"] + "\" href=\"javascript:void(0)\" rel=\"" + dv2[j]["LinkAddress"] + "\" ><span class=\"" + dv2[j]["Icon"].ToString() + "\" >&nbsp;</span><span class=\"nav\">" + dv2[j]["Pagelogo"] + "</span></a></div></li>");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("]},");
                    // sb.Append("</ul>}");
                }
                //sb.Remove(0, 1);
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }

        private string defaultMenu(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            Tb_Navigation bll = new Tb_Navigation();
            List<WZX.Model.Tb_Navigation> list = new List<WZX.Model.Tb_Navigation>();
            WZX.Model.View_Users item = context.Session["User"] as WZX.Model.View_Users;
            string strWhere = null;
            if (item.UserRealName == "管理员")
            {
                strWhere = " IsShow=0";
            }
            else
            {
                strWhere = "Id in(select NavigationId from Tb_RolesAndNavigation where RolesId in(select RolesId from Tb_RolesAddUser where UserId='" + item.Userid + "')) and IsShow=0";
            }
            DataSet ds = bll.GetList(strWhere);
            sb.Append(" {\"menus\":[");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = " ParentId=0";
                dv.Sort = "Sort";
                for (int i = 0; i < dv.Count; i++)
                {
                    sb.Append("{\"menuid\":\"" + dv[i]["Id"] + "\",\"icon\":\"" + dv[i]["Icon"] + "\",\"menuname\":\"" + dv[i]["Pagelogo"] + "\",");
                    sb.Append("\"menus\":[");
                    //sb.Append("{" + dv[i]["Pagelogo"] + "," + dv[i]["Icon"] + ",");
                    //sb.Append("<ul>");
                    DataView dv2 = new DataView(ds.Tables[0]);
                    dv2.RowFilter = " ParentId=" + dv[i]["Id"];
                    dv2.Sort = " Sort";
                    for (int j = 0; j < dv2.Count; j++)
                    {
                        sb.Append("{\"menuid\":\"" + dv2[j]["Id"] + "\",\"menuname\":\"" + dv2[j]["Pagelogo"] + "\",\"icon\":\"" + dv2[j]["Icon"].ToString() + "\",\"url\":\"" + dv2[j]["LinkAddress"] + "\"},");
                        //sb.Append("<li><div><a ref=\"" + dv2[j]["Pagelogo"] + "\" href=\"javascript:void(0)\" rel=\"" + dv2[j]["LinkAddress"] + "\" ><span class=\"" + dv2[j]["Icon"].ToString() + "\" >&nbsp;</span><span class=\"nav\">" + dv2[j]["Pagelogo"] + "</span></a></div></li>");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("]},");
                    // sb.Append("</ul>}");
                }
                //sb.Remove(0, 1);
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
            }
           return sb.ToString();
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