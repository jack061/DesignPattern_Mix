using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WZX.Busines;
using RM.Busines;

namespace Survey.ashx
{
    /// <summary>
    /// SaveNavigationRole 的摘要说明
    /// </summary>
    public class SaveNavigationRole : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string RoleId = context.Request["RoleId"].ToString();
            string NavId = context.Request["NavId"].ToString();
            List<string> list=new List<string>();
            list.Add("delete from Tb_RolesAndNavigation where RolesId="+RoleId);
            string[] str = NavId.Split('|');
            for (int i = 0; i < str.Length - 1; i++) {
                string[] str2 = str[i].Split(',');
                if (str2.Length == 2)
                    list.Add(" insert into Tb_RolesAndNavigation(RolesId,NavigationId,ButtonId) values(" + RoleId + "," + str2[0] + ",0)");
                else {
                    for (int j = 1; j < str2.Length - 1; j++) {
                        list.Add(" insert into Tb_RolesAndNavigation(RolesId,NavigationId,ButtonId) values(" + RoleId + "," + str2[0] + ","+str2[j]+")");  
                    }
                }
            }
            DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list);
            context.Response.Write("Hello World");
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