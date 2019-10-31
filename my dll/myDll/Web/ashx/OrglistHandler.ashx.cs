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
    /// OrglistHandler 的摘要说明
    /// </summary>
    public class OrglistHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.QueryString["type"] == "edit")//获取部门信息
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                Com_Organization bll = new Com_Organization();
                WZX.Model.Com_Organization model = bll.GetModel(Id);
                if (model != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(model.Agency+",");
                    //WZX.Model.Com_Organization pmodel = bll.GetModel(model.ParentId);
                    //if (pmodel != null)
                    //{
                    //    sb.Append(pmodel.Agency);
                    //}
                    sb.Append(model.ParentId+",");
                    sb.Append(model.Sort+",");
                    sb.Append(model.Person+",");
                    sb.Append(model.Remark);
                    context.Response.Write(sb.ToString());
                }
            }
            else if (context.Request.QueryString["type"] == "del")//删除部门信息
            {
                string Id = context.Request.QueryString["Id"];
                Com_Organization bll = new Com_Organization();
                string[] str = Id.Split(',');
                int oId = int.Parse(str[str.Length - 1]);
                WZX.Model.Com_Organization model = bll.GetModel(oId);
                List<string> listSql = new List<string>();
                if (model != null)
                {
                    string sql = "update Com_Organization set Sort=Sort-1 where ParentId=" + model.ParentId + " and Sort>" +model.Sort;
                    listSql.Add(sql);
                }
                listSql.Add(" delete Com_Organization where Id in(" + Id + ")");
                listSql.Add(" update Com_OrgAddUser set OrgId=(select top 1 Id from Com_Organization where ParentId=0) where OrgId in (" +Id + ")");
                if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(listSql) > 0)
                {
                    context.Response.Write("true");
                }
            }
            else if (context.Request.QueryString["type"] == "save")//保存修改或添加部门信息
            {
                Com_Organization bll = new Com_Organization();
                WZX.Model.Com_Organization model = null;
                string name=context.Request.QueryString["name"];
                string remark = context.Request.QueryString["remark"];
                string person = context.Request.QueryString["person"];
                int sort = int.Parse(context.Request.QueryString["sort"]);
                int parent = 0;
                if (context.Request.QueryString["parentId"] != null&&context.Request.QueryString["parentId"]!="")
                {
                    parent = int.Parse(context.Request.QueryString["parentId"]);
                }
                List<WZX.Model.Com_Organization> list = bll.GetModelList(" ParentId=" + parent);
                List<string> listSql = new List<string>();
                if (context.Request.QueryString["Id"] != null && context.Request.QueryString["Id"] != "")
                {
                    int Id = int.Parse(context.Request.QueryString["Id"]);
                    model = bll.GetModel(Id);
                    if (model.ParentId == parent)
                    {
                        if (sort > list.Count)
                        {
                            sort = list.Count;
                        }
                        if (model.Sort > sort)
                        {
                            string sql = "update Com_Organization set Sort=Sort+1 where ParentId=" + parent + " and Sort>=" + sort + " and Sort<" + model.Sort;
                            listSql.Add(sql);
                        }
                        else if (model.Sort < sort)
                        {
                            string sql = "update Com_Organization set Sort=Sort-1 where ParentId=" + parent + " and Sort<=" + sort + " and Sort>" + model.Sort;
                            listSql.Add(sql);
                        }
                    }
                    else
                    {
                        if (sort > list.Count + 1)
                        {
                            sort = list.Count + 1;
                        }
                        else
                        {
                            string sql = "update Com_Organization set Sort=Sort+1 where ParentId=" + parent + " and Sort>=" + sort;
                            listSql.Add(sql);
                            string sql2 = "update Com_Organization set Sort=Sort-1 where ParentId=" + model.ParentId + " and Sort>" + model.Sort;
                            listSql.Add(sql2);
                        }
                    }
                    if (listSql.Count > 0)
                    {
                        DataFactory.SqlDataBaseExpand().ExecuteSqlTran(listSql);
                    }
                    model.Agency = name;
                    model.Person = person;
                    model.Remark = remark;
                    model.Sort = sort;
                    model.ParentId = parent;
                    bll.Update(model);
                }
                else
                {
                    model = new WZX.Model.Com_Organization();
                    model.Agency = name;
                    model.Person = person;
                    model.Remark = remark;
                    if (sort > list.Count + 1)
                    {
                        sort = list.Count + 1;
                    }
                    else
                    {
                        string sql = "update Com_Organization set Sort=Sort+1 where ParentId=" + parent + " and Sort>=" + sort;
                        listSql.Add(sql);
                        string sql2 = "update Com_Organization set Sort=Sort-1 where ParentId=" + model.ParentId + " and Sort>" + model.Sort;
                        listSql.Add(sql2);
                        DataFactory.SqlDataBaseExpand().ExecuteSqlTran(listSql);
                    }
                    model.Sort = sort;
                    model.ParentId = parent;
                    bll.Add(model);
                }
               
                context.Response.Write("true");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                Com_Organization bll = new Com_Organization();
                //Com_UserInfos ubll = new Com_UserInfos();
                //WZX.Model.Com_UserInfos user = null;
                DataSet ds = new DataSet();
                if (context.Request["Id"] != null)
                {
                    ds=bll.GetList(" Id!=" + context.Request.QueryString["Id"]);
                }else{
                    ds = bll.GetAllList();
                }
                if (ds.Tables.Count > 0)
                {
                    sb.Append("[");
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "ParentId=0";
                    dv.Sort = " Sort ";
                    for (int i = 0; i < dv.Count; i++)
                    {
                        sb.Append("{");
                        sb.Append("\"id\":" + dv[i]["Id"] + ",");
                        sb.Append("\"text\":\"" + dv[i]["Agency"] + "\",");
                        sb.Append("\"Sort\":\"" + dv[i]["Sort"] + "\",");
                        sb.Append("\"Person\":\"" + dv[i]["Person"] + "\",");
                        sb.Append("\"Remark\":\"" + dv[i]["Remark"] + "\"");
                        DataView dv2 = new DataView(ds.Tables[0]);
                        dv2.RowFilter = "ParentId=" + dv[i]["Id"];
                        dv2.Sort = " Sort ";
                        if (dv2.Count > 0)
                        {
                            sb.Append(GetChlid(dv2, ds));
                        }
                        sb.Append("},");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("]");
                }
                context.Response.Write(sb.ToString());
            }
        }
        protected StringBuilder GetChlid(DataView dv,DataSet ds)
        {
            StringBuilder sb = new StringBuilder();
            //Com_UserInfos ubll = new Com_UserInfos();
            //WZX.Model.Com_UserInfos user = null;
            sb.Append(",\"children\":[");

            for (int i = 0; i < dv.Count; i++)
            {
                sb.Append("{");
                sb.Append("\"id\":" + dv[i]["Id"] + ",");
                sb.Append("\"text\":\"" + dv[i]["Agency"] + "\",");
                sb.Append("\"Sort\":\"" + dv[i]["Sort"] + "\",");
                //user = new WZX.Model.Com_UserInfos();
                //user = ubll.GetModel(dv[i]["Person"].ToString());
                //if (user != null)
                //{
                //    sb.Append("\"Person\":\"" + user.UserRealName + "\",");
                //}
                //else
                //{
                    sb.Append("\"Person\":\"" + dv[i]["Person"] + "\",");
                //}
                sb.Append("\"Remark\":\"" + dv[i]["Remark"] + "\"");
                DataView dv2 = new DataView(ds.Tables[0]);
                dv2.RowFilter = "ParentId=" + dv[i]["Id"];
                dv2.Sort = " Sort ";
                if (dv2.Count > 0)
                {
                    sb.Append(GetChlid(dv2, ds));
                }
                sb.Append("},");
            }
            sb.Remove(sb.Length-1,1);
            sb.Append("]");
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