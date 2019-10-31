using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using WZX.Busines.DAL;
using WZX.Busines;
using RM.Busines;

namespace Survey.ashx
{
    /// <summary>
    /// NavigationRole 的摘要说明
    /// </summary>
    public class NavigationRole : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string RoleId="0";
            if (context.Request["RoleId"] != null) {
                RoleId = context.Request["RoleId"].ToString();
            }
            Tb_Navigation bll = new Tb_Navigation();
            DataSet ds = bll.GetList("  IsShow=0");
            DataSet ds2 = DataFactory.SqlDataBaseExpand().Query("select Id,ButtonName from Com_ButtonGroup");
            DataSet ds4 = DataFactory.SqlDataBaseExpand().Query("select [RolesId],[NavigationId],ButtonId  FROM [Tb_RolesAndNavigation] where RolesId=" + RoleId);
            DataSet ds3 = DataFactory.SqlDataBaseExpand().Query("select NavigationId,ButtonId from Com_NavigationAndButton");
           //0菜单没有按钮,1菜单有按钮角色没按钮，2菜单有按钮并且角色也有按钮
            StringBuilder sb = new StringBuilder();
            string strId = "";
            foreach (DataRow dr in ds2.Tables[0].Rows) {
                sb.Append(dr["ButtonName"]+",");
                strId += dr["Id"] + ",";
            }
            sb.Append("|");
            sb.Append(strId+"|");
            foreach (DataRow dr in ds4.Tables[0].Rows)
            {
                sb.Append(dr["NavigationId"] + ",");
            }
            sb.Append("|");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = " ParentId=0";
                dv.Sort = "Sort";
                for (int i = 0; i < dv.Count; i++)
                {
                    //DataView dv5 = new DataView(ds4.Tables[0]);
                    //dv5.RowFilter = " NavigationId=" + dv[i]["Id"];
                    //if (dv5.Count > 0)
                    //    sb.Append("1,");
                    //else
                    //    sb.Append("0,");
                    for (int k = 0; k < ds2.Tables[0].Rows.Count;k++ )
                    {
                        sb.Append("0,");
                    }
                    sb.Append("|");
                    DataView dv2 = new DataView(ds.Tables[0]);
                    dv2.RowFilter = " ParentId=" + dv[i]["Id"];
                    dv2.Sort = " Sort";
                    if (dv2.Count > 0)
                    {
                        for (int j = 0; j < dv2.Count; j++)
                        {
                            //DataView dv6 = new DataView(ds4.Tables[0]);
                            //dv6.RowFilter = " NavigationId=" + dv2[j]["Id"];
                            //if (dv6.Count > 0)
                            //    sb.Append("1,");
                            //else
                            //    sb.Append("0,");
                            foreach (DataRow dr in ds2.Tables[0].Rows)
                            {
                                DataView dv3 = new DataView(ds3.Tables[0]);
                                dv3.RowFilter = " NavigationId=" + dv2[j]["Id"] + " and ButtonId=" + dr["Id"];
                                if (dv3.Count > 0)
                                {
                                    DataView dv4 = new DataView(ds4.Tables[0]);
                                    dv4.RowFilter = " NavigationId=" + dv2[j]["Id"] + " and ButtonId=" + dr["Id"];
                                    if (dv4.Count > 0)
                                        sb.Append("2,");
                                    else
                                        sb.Append("1,");
                                }
                                else
                                    sb.Append("0,");
                            }
                            sb.Append("|");
                        }
                    }
                    
                }
            }
            string text = sb.ToString();
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