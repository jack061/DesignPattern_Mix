using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using WZX.Busines.DAL;
using WZX.Busines;
using RM.Busines;

namespace Survey.ashx
{
    /// <summary>
    /// EditRoles 的摘要说明
    /// </summary>
    public class EditRoles : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.QueryString["type"] == "edit")//获取角色信息
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                Tb_Roles bll = new Tb_Roles();
                WZX.Model.Tb_Roles model= bll.GetModel(Id);
                StringBuilder sb = new StringBuilder();
                sb.Append(model.RolesName+",");
                sb.Append(model.Remark+",");
                View_Users ubll = new View_Users();
                List<WZX.Model.View_Users> list = ubll.GetModelList(" Userid in(select UserId from Tb_RolesAddUser where RolesId="+Id+")");
                foreach(WZX.Model.View_Users item in list)
                {
                    sb.Append("<div onclick='ss(" + item.Userid + ")' height='23px'><input type='hidden' value='" + item.Userid + "' />" + item.LoginPassword + "|" + item.UserRealName + "</div>");
                }
                context.Response.Write(sb.ToString());
            }
            else if (context.Request.QueryString["type"] == "getUser")//获取未绑定到当前角色的用户
            {
                StringBuilder sb = new StringBuilder();
                View_Users ubll = new View_Users();
                List<WZX.Model.View_Users> list = new List<WZX.Model.View_Users>();
                //if (context.Request.QueryString["Id"] != null && context.Request.QueryString["Id"] != "")
                //{
                //    int Id = int.Parse(context.Request.QueryString["Id"]);
                //    list = ubll.GetModelList(" Userid not in(select UserId from Tb_RolesAddUser where RolesId=" + Id + ")");
                //}
                //else
                //{
                    list = ubll.GetModelList("");
               // }
                foreach (WZX.Model.View_Users item in list)
                {
                    sb.Append("<div><input name=\"chkItem\" value=\"<div onclick='ss(" + item.Userid + ")' height='23px'><input type='hidden' value='" + item.Userid + "'  />" + item.LoginPassword + "|" + item.UserRealName + "</div>\" type=\"checkbox\" /> ");
                    sb.Append(item.LoginPassword + "|" + item.UserRealName + "</div>");
                }
                context.Response.Write(sb.ToString());
            }
            else if (context.Request.QueryString["type"] == "save")//保存角色信息
            {
                string name = context.Request.QueryString["name"];
                string remark = context.Request.QueryString["remark"];
                int Id = int.Parse(context.Request.QueryString["Id"]);
                Tb_Roles bll = new Tb_Roles();
                WZX.Model.Tb_Roles model = bll.GetModel(Id);
                model.Remark = remark;
                model.RolesName = name;
                context.Response.Write(bll.Update(model));
            }
            else if (context.Request.QueryString["type"] == "add")//添加
            {
                string name = context.Request.QueryString["name"];
                string remark = context.Request.QueryString["remark"];
                WZX.Model.Tb_Roles model = new WZX.Model.Tb_Roles();
                Tb_Roles bll = new Tb_Roles();
                model.Remark = remark;
                model.RolesName = name;
                if(bll.Add(model)>0)
                    context.Response.Write("true");
                else
                    context.Response.Write("false");

            }
            else if (context.Request.QueryString["type"] == "delRole")//删除角色
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                Tb_Roles bll = new Tb_Roles();
               
                Tb_RolesAddUser rbll = new Tb_RolesAddUser();
                rbll.Delete(Id);
                context.Response.Write( bll.Delete(Id));
            }
            else if (context.Request.QueryString["type"] == "Distri")//获取已分配的权限
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                Tb_RolesAndNavigation bll = new Tb_RolesAndNavigation();
                List<WZX.Model.Tb_RolesAndNavigation> list = new List<WZX.Model.Tb_RolesAndNavigation>();
                list = bll.GetModelList(" RolesId=" + Id);
                StringBuilder sb = new StringBuilder();
                foreach (WZX.Model.Tb_RolesAndNavigation model in list)
                {
                    sb.Append(model.NavigationId + ",");
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                context.Response.Write(sb.ToString());
            }
            else if (context.Request.QueryString["type"] == "saveDistri")
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                string nav = context.Request.QueryString["nav"];
                string[] str = nav.Split(',');
                List<string> list = new List<string>();
                string sql = "delete Tb_RolesAndNavigation where RolesId=" + Id;
                foreach (string ss in str)
                {
                    Tb_Navigation bll = new Tb_Navigation();
                    WZX.Model.Tb_Navigation model = bll.GetModel(int.Parse(ss));
                    if (model.ParentId != 0)
                    {
                        string sql3 = "delete Tb_RolesAndNavigation where RolesId=" + Id + " and NavigationId=" + model.ParentId;
                        string sql1 = "insert into Tb_RolesAndNavigation (RolesId,NavigationId) values(" + Id + "," + model.ParentId + ")";
                        list.Add(sql3);
                        list.Add(sql1);
                    }
                    string sql2 = "insert into Tb_RolesAndNavigation (RolesId,NavigationId) values(" + Id + "," + ss + ")";
                    list.Add(sql2);
                }
                DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list);
            }
           // context.Response.Write("Hello World");
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