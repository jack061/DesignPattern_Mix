using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.SessionState;
using System.Data;
using RM.Busines.DAL;
using RM.Busines;
using RM.Common.DotNetEncrypt;
using RM.Common.DotNetJson;
using WZX.Busines.DAL;

namespace Survey.ashx
{
    /// <summary>
    /// UsersHandler 的摘要说明
    /// </summary>
    public class UsersHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            View_Users bll = new View_Users();
            if (context.Request.QueryString["type"] == "edit")//获取要编辑的用户信息
            {
                string Userid = context.Request.QueryString["Id"];
                DataSet ds = DataFactory.SqlDataBaseExpand().Query("select Id,RolesName from Tb_Roles where Id in (select RolesId from Tb_RolesAddUser where UserId=" + Userid + ")");
                string IdList = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (IdList != "")
                        IdList += ",";
                    IdList += dr["Id"].ToString();
                }
                context.Response.Write(IdList);
            }
            else if (context.Request.QueryString["type"] == "chakan")
            {
                Com_UserLogin userbll = new Com_UserLogin();
                string userlist = JsonHelper.ToJson(userbll.GetList("").Tables[0]);
                context.Response.Write(userlist);
            }
            else if (context.Request.QueryString["type"] == "sel")//浏览用户信息
            {
                string Userid = context.Request.QueryString["Id"];
                DataSet ds = DataFactory.SqlDataBaseExpand().Query("select Id,RolesName from Tb_Roles where Id in (select RolesId from Tb_RolesAddUser where UserId=" + Userid + ")");
                string IdList = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (IdList != "")
                        IdList += ",";
                    IdList += dr["RolesName"].ToString();
                }
                context.Response.Write(IdList);
            }
            else if (context.Request.QueryString["type"] == "del")//删除用户信息，同时删除用户角色表，用户部门表
            {
                string Userid = context.Request.QueryString["Id"];
                string sql = "delete Com_UserInfos where Userid='" + Userid + "'";
                string sql2 = "delete Com_UserLogin where Userid='" + Userid + "'";
                string sql3 = "delete Com_OrgAddUser where UserId='" + Userid + "'";
                string sql4 = "delete Tb_RolesAddUser where UserId='" + Userid + "'";
                List<string> list = new List<string>();
                list.Add(sql);
                list.Add(sql2);
                list.Add(sql3);
                list.Add(sql4);
                if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0)
                {
                    context.Response.Write("true");
                }
                else
                {
                    context.Response.Write("false");
                }
            }
            else if (context.Request.QueryString["type"] == "pass")//修改密码
            {
                string Userid = context.Request.QueryString["UserId"];
                string pass = context.Request.QueryString["pass"];
                Com_UserLogin bll2 = new Com_UserLogin();
                WZX.Model.Com_UserLogin model = new WZX.Model.Com_UserLogin();
                model = bll2.GetModel(Userid);
                model.LoginPassword = DESEncrypt.Encrypt(pass);
                if (bll2.Update(model))
                {
                    context.Response.Write("true");
                }
                else
                {
                    context.Response.Write("false");
                }
            }
            else if (context.Request.QueryString["type"] == "save")//添加和修改
            {
                string login = context.Request.QueryString["login"];
                string pass = context.Request.QueryString["pass"];
                string name = context.Request.QueryString["name"];
                string sex = context.Request.QueryString["sex"];
                string email = context.Request.QueryString["email"];
                string tel = context.Request.QueryString["tel"];
                string mobile = context.Request.QueryString["mobile"];
                string status = context.Request.QueryString["status"];
                string org = "";
                string methods = context.Request.QueryString["methods"];
                if (context.Request["flag"].ToString() == "0")//0：修改，1：添加
                {
                     
                     Com_Organization Orgbll = new Com_Organization();
                     
                    DataTable dt= Orgbll.GetList(" Agency='" + context.Request["org"].ToString() + "'").Tables[0];
                    org = dt.Rows[0]["Id"].ToString();
                }
                else 
                {
                    org = context.Request.QueryString["org"];
                }
                
                string role = context.Request.QueryString["role"];
                Com_UserInfos bll1 = new Com_UserInfos();
                Com_UserLogin bll2 = new Com_UserLogin();
                WZX.Model.Com_UserInfos item1 = new WZX.Model.Com_UserInfos();
                WZX.Model.Com_UserLogin model = new WZX.Model.Com_UserLogin();
                string Userid = context.Request.QueryString["Userid"];
                if (methods=="0")
                {
                    item1 = bll1.GetModel(Userid);
                    model = bll2.GetModel(Userid);
                }
                else
                {
                    Userid = bll1.GetMaxId().ToString();
                    if (Userid == "1")
                    {
                        Userid = "1000000000";
                    }
                    item1.AddDate = DateTime.Now;
                }
                item1.Email = email;
                item1.Mobile = mobile;
                item1.Sex = sex;
                item1.Tel = tel;
                item1.Userid = Userid;
                item1.UserRealName = name;

                model.LoginName = login;
                model.LoginPassword =DESEncrypt.Encrypt(pass);
                model.Status = int.Parse(status);
                model.UserId = Userid;
                Com_OrgAddUser obll = new Com_OrgAddUser();
               
               
                if (methods=="0")
                {
                    bll1.Update(item1);
                    bll2.Update(model);
                }
                else
                {
                    if (bll2.Exists(login))
                    {

                        context.Response.Write("false"); context.Response.End();
                    }
                    else
                    {
                        bll1.Add(item1);
                        bll2.Add(model);
                    }
                } 
                if (org != null && org != "")
                {   obll.Delete(Userid);
                    WZX.Model.Com_OrgAddUser omodel = new WZX.Model.Com_OrgAddUser();
                    omodel.OrgId = int.Parse(org);
                    omodel.UserId = Userid;
                    obll.Add(omodel);
                }
                saveRole(Userid, role);
                if (methods == "0")
                {
                    context.Response.Write("0"); context.Response.End();
                }
                if (methods == "1") {//添加
                    context.Response.Write("1"); context.Response.End();
                }
            }
        }
        public bool saveRole(string Userid, string role)
        {
            List<string> list = new List<string>();
            string sql2 = "delete Tb_RolesAddUser where UserId='" + Userid + "'";
            list.Add(sql2);
            if (role != null && role != "")
            {
                string[] str = role.Split(',');
                foreach (string s in str)
                {
                    string sql = "insert into Tb_RolesAddUser(RolesId,UserId) values(" + s + ",'" + Userid + "')";
                    list.Add(sql);
                }
            }
            if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0)
                return true;
            else
                return false;
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