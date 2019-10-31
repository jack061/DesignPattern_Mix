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
    /// EditAuthority 的摘要说明
    /// </summary>
    public class EditAuthority : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder sb = new StringBuilder();
            if (context.Request.QueryString["type"] == "save")//保存修改的菜单
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                string Name = context.Request.QueryString["Name"];
                string Code = context.Request.QueryString["Code"];
                string Icon = context.Request.QueryString["Icon"];
                int sort = int.Parse(context.Request.QueryString["sort"]);
                string remark = context.Request.QueryString["remark"];
                Com_ButtonGroup bll = new Com_ButtonGroup();
                WZX.Model.Com_ButtonGroup model = bll.GetModel(Id);
                model.ButtonName = Name;
                model.BtnCode = Code;
                model.Remark = remark;
                int maxSort = bll.GetMaxId()-1;//得到最大的排序
                string sql = "";
                if (model.Sort > sort) {
                    sql = "update Com_ButtonGroup set Sort=Sort+1 where Sort<" + model.Sort + " and Sort>=" + sort;
                }
                else if (model.Sort < sort) {
                    sql = "update Com_ButtonGroup set Sort=Sort-1 where Sort>" + model.Sort + " and Sort<=" + sort;
                }
                if(sql!="")
                    DataFactory.SqlDataBaseExpand().ExecuteSql(sql);
                model.Icon = Icon;
                if (sort > maxSort)
                {
                    model.Sort = maxSort;
                }
                else
                {
                    model.Sort = sort;
                }
                bll.Update(model);
            }
            else if (context.Request.QueryString["type"] == "add")//添加菜单
            {
                string Name = context.Request.QueryString["Name"];
                string Code = context.Request.QueryString["Code"];
                string Icon = context.Request.QueryString["Icon"];
                int sort = int.Parse(context.Request.QueryString["sort"]);
                string remark = context.Request.QueryString["remark"];
                Com_ButtonGroup bll = new Com_ButtonGroup();
                WZX.Model.Com_ButtonGroup model = new WZX.Model.Com_ButtonGroup();
                model.ButtonName = Name;
                model.BtnCode = Code;
                model.Remark = remark;
                int maxSort = bll.GetMaxId();//得到最大的排序
                string sql = "update Com_ButtonGroup set Sort=Sort+1 where Sort>=" + sort;
                DataFactory.SqlDataBaseExpand().ExecuteSql(sql);
                model.Icon = Icon;
                if (sort > maxSort)
                {
                    model.Sort = maxSort;
                }
                else
                {
                    model.Sort = sort;
                }
                bll.Add(model);
            }
            else {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                int sort = int.Parse(context.Request.QueryString["sort"]);
                Com_ButtonGroup bll = new Com_ButtonGroup();
                string sql = "update Com_ButtonGroup set Sort=Sort-1 where Sort>" + sort;
                DataFactory.SqlDataBaseExpand().ExecuteSql(sql);
                bll.Delete(Id);
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