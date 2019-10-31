using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using RM.Common.DotNetEncrypt;
using WZX.Busines.DAL;
using RM.Common.DotNetBean;

namespace HBISProject.ashx.Login
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string Action = context.Request["action"];                      //提交动作
            string user_Account = context.Request["user_Account"];          //账户
            string userPwd = context.Request["userPwd"];                    //密码
            string code = context.Request["code"];                          //验证码
            switch (Action)
            {
                case "login":
                    //TODO:暂时屏蔽验证码
                    if (code.ToLower() != context.Session["dt_session_code"].ToString().ToLower())
                    {
                        context.Response.Write("1");//验证码输入不正确！
                        context.Response.End();
                    }
                    string name = user_Account.Trim();
                    string pass = DESEncrypt.Encrypt( userPwd.Trim());
                    Com_UserLogin bll = new Com_UserLogin();
                    string UserId = bll.GetUserId(name, pass);

                    if (UserId != null)
                    {
                        WZX.Model.Com_UserLogin model = bll.GetModel(UserId);
                        model.LastLoginDate = DateTime.Now;
                        model.LastLoginIP = context.Request.UserHostAddress;
                        bll.Update(model);
                        WZX.Model.View_Users item = new WZX.Model.View_Users();
                        View_Users vbll = new View_Users();
                        item = vbll.GetModel(UserId);
                        context.Session["User"] = item;
                        context.Session["UserId"] = UserId;
                        Com_OrgAddUser orgbll = new Com_OrgAddUser();
                        WZX.Model.Com_OrgAddUser orgmodel = orgbll.GetModelList(" userid=" + UserId)[0];
                        context.Session["OrgId"] = orgmodel.OrgId;
                        context.Session["username"] = item.UserRealName;
                        if (item.Sex == Convert.ToString(1))
                        {
                            context.Session["sex"] = "先生";
                        }
                        else
                        {
                            context.Session["sex"] = "女士";
                        }
                        //--begin
                        //--兼容PageBase类
                        SessionUser sessionUser = new SessionUser(item.Userid, item.LoginName, item.LoginPassword, item.UserRealName);
                        RequestSession.AddSessionUser(sessionUser);
                        //--end
                        Com_LoginLog lbll = new Com_LoginLog();
                        WZX.Model.Com_LoginLog lmodel = new WZX.Model.Com_LoginLog();
                        lmodel.LoginDate = DateTime.Now;
                        lmodel.LoginIP = context.Request.UserHostAddress;
                        lmodel.Status = "0";
                        lmodel.Userid = UserId;
                        lbll.Add(lmodel);
                        context.Response.Write("3");//验证成功
                        context.Response.End();
                    }
                    else 
                    {
                        context.Response.Write("4");//账户或者密码有错误！
                        context.Response.End();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 同一账号不能同时登陆
        /// </summary>
        /// <param name="context"></param>
        /// <param name="User_Account">账户</param>
        /// <returns></returns>
        public bool Islogin(HttpContext context, string User_Account)
        {
            //将Session转换为Arraylist数组
            //ArrayList list = context.Session["GLOBAL_USER_LIST"] as ArrayList;
            //if (list == null)
            //{
            //    list = new ArrayList();
            //}
            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (User_Account == (list[i] as string))
            //    {
            //        //已经登录了，提示错误信息 
            //        return false; ;
            //    }
            //}
            ////将用户信息添加到list数组中
            //list.Add(User_Account);
            ////将数组放入Session
            //context.Session.Add("GLOBAL_USER_LIST", list);
            return true;
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