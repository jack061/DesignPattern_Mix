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
    /// NavigationList 的摘要说明
    /// </summary>
    public class NavigationList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder sb = new StringBuilder();
            if (context.Request.QueryString["type"] == "edit")//获取要修改的菜单
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                Tb_Navigation bll = new Tb_Navigation();
                WZX.Model.Tb_Navigation model= bll.GetModel(Id);
                if (model != null)
                {
                    sb.Append(model.MenuName+",");
                    sb.Append(model.Pagelogo+",");
                    sb.Append(model.ParentId + ",");
                    sb.Append(model.LinkAddress+",");
                    sb.Append(model.Icon+",");
                    List<WZX.Model.Tb_Navigation> list = bll.GetModelList(" ParentId="+model.ParentId);
                    string str = "";
                    for (int i = 1; i <= list.Count; i++)
                    {
                        if (i == model.Sort)
                        {
                            str += "<option value='" + i + "'  selected='selected'>" + i + "</option>";
                        }
                        else
                        {
                            str += "<option value='" + i + "'>" + i + "</option>";
                        }
                    }
                    sb.Append(str + ",");
                    sb.Append(model.IsShow);
                }
                context.Response.Write(sb.ToString());
            }
            else if (context.Request.QueryString["type"] == "Parent")//获取所有根节点 并生成选项
            {
                Tb_Navigation bll = new Tb_Navigation();
                List<WZX.Model.Tb_Navigation> list = bll.GetModelList(" ParentId=0");
                sb.Append("<option value='0'></option>");
                foreach (WZX.Model.Tb_Navigation model in list)
                {
                    sb.Append("<option value='" + model.Id + "'>" + model.MenuName+ "</option>");
                }
                context.Response.Write(sb.ToString());
            }
            else if (context.Request.QueryString["type"] == "child")//获取所有子节点数量并生成排序选项
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                Tb_Navigation bll = new Tb_Navigation();
                List<WZX.Model.Tb_Navigation> list = bll.GetModelList(" ParentId="+Id);
                int i = 1;
                for ( ; i <= list.Count; i++)
                {
                    sb.Append("<option value='" + i + "'>" + i + "</option>");
                }
                sb.Append("<option value='" + i + "' selected='selected'>" + i + "</option>");
                sb.Append(",<option value='" + i + "' selected='selected'>" + i + "</option>");
                context.Response.Write(sb.ToString());
            }
            else if (context.Request.QueryString["type"] == "save")//保存修改的菜单
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                string MenuName = context.Request.QueryString["Name"];
                string Logo = context.Request.QueryString["Name"];
                int Parent = int.Parse(context.Request.QueryString["Parent"]);
                string Link = context.Request.QueryString["Link"];
                string icon = context.Request.QueryString["icon"];
                int sort = int.Parse(context.Request.QueryString["sort"]);
                int Isshow = int.Parse(context.Request.QueryString["Isshow"]);
                Tb_Navigation bll = new Tb_Navigation();
                WZX.Model.Tb_Navigation model = bll.GetModel(Id);
                model.MenuName = MenuName;
                model.Pagelogo = Logo;
                List<string> listSql = new List<string>();
                if (model.ParentId == Parent)
                {
                    if (model.Sort > sort)
                    {
                        string sql = "update Tb_Navigation set Sort=Sort+1 where ParentId=" + Parent + " and Sort>=" + sort + " and Sort<" + model.Sort;
                        listSql.Add(sql);
                    }
                    else if(model.Sort<sort)
                    {
                        string sql = "update Tb_Navigation set Sort=Sort-1 where ParentId=" + Parent + " and Sort<=" + sort + " and Sort>" + model.Sort;
                        listSql.Add(sql);
                    }
                }
                else
                {
                    string sql = "update Tb_Navigation set Sort=Sort+1 where ParentId=" + Parent + " and Sort>=" + sort;
                    listSql.Add(sql);
                    string sql2 = "update Tb_Navigation set Sort=Sort-1 where ParentId=" + model.ParentId + " and Sort>" + model.Sort;
                    listSql.Add(sql2);
                }
                if (listSql.Count>0)
                {
                    DataFactory.SqlDataBaseExpand().ExecuteSqlTran(listSql);
                }
                if (Parent == 0)
                {
                    model.LinkAddress = null;
                }
                else
                {
                    model.LinkAddress = Link;
                }
                model.ParentId = Parent;
                model.Icon = icon;
                List<WZX.Model.Tb_Navigation> list = bll.GetModelList(" ParentId=" + Parent);
                if (sort > list.Count + 1)
                {
                    model.Sort = list.Count + 1;
                }
                else
                {
                    model.Sort = sort;
                }
                model.IsShow = Isshow;
                bll.Update(model);
            }
            else if (context.Request.QueryString["type"] == "add")//添加菜单
            {
                string MenuName = context.Request.QueryString["Name"];
                string Logo = context.Request.QueryString["Name"];
                int Parent = int.Parse(context.Request.QueryString["Parent"]);
                string Link = context.Request.QueryString["Link"];
                string icon = context.Request.QueryString["icon"];
                int sort = int.Parse(context.Request.QueryString["sort"]);
                int Isshow = int.Parse(context.Request.QueryString["Isshow"]);
                Tb_Navigation bll = new Tb_Navigation();
                WZX.Model.Tb_Navigation model =new WZX.Model.Tb_Navigation();
                model.MenuName = MenuName;
                model.Pagelogo = Logo;
                string sql = "update Tb_Navigation set Sort=Sort+1  where ParentId=" + Parent + " and Sort>=" + sort;
                DataFactory.SqlDataBaseExpand().ExecuteSql(sql);
                if (Parent == 0)
                {
                    model.LinkAddress = null;
                }
                model.LinkAddress = Link;
                model.ParentId = Parent;
                model.Icon = icon;
                model.Sort = sort;
                model.IsShow = Isshow;
                bll.Add(model);
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