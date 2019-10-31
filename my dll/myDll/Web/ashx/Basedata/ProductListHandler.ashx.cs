using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using WZX.Busines.DAL;
using WZX.Busines;
using RM.Busines;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using WZX.Busines.Util;
using System.Web.SessionState;
using RM.Common.DotNetBean;
using System.Data.SqlClient;
using System.Collections;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// UserListHandler 的摘要说明
    /// </summary>
    public class ProductListHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string err = "";
            bool suc = false;
            context.Response.ContentType = "text/plain";
            string type = context.Request["type"] == null ? "" : context.Request["type"].ToString();
            switch (type)
            {
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;
                case "getSubList"://获取产品价格子表信息
                    context.Response.Write(getSubList(context));
                    break;
                case "edit":
                    context.Response.Write(edit(context));
                    //context.Response.Write(getList(context));
                    break;
                case "save":
                    context.Response.Write(Save(context));
                    break;
                case "add":
                    context.Response.Write(add(context));
                    break;
                case "getTree":
                    context.Response.Write(getTree(context));
                    break;
                case "del":
                    context.Response.Write(Del(context));
                    break;
                case "getCategory":
                    context.Response.Write(getCategory(context));
                    break;
                case "saveImportData"://保存上传excel

                    suc = saveImportData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        private string getCategory(HttpContext context)
        {
            RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
               using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(@"select distinct productCategory as name from bproduct");
              return jsonh.ToEasyUIComboxJson(ds.Tables[0]).ToString();
            }
           
        }

        private string getTree(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            ds = GetList("");

            sb.Append("[");
            DataView dv = new DataView(ds.Tables[0]);

            sb.Append("{");

            sb.Append("\"text\":\"" + "产品大类" + "\"");

            DataView dv2 = new DataView(ds.Tables[0]);

            if (dv2.Count > 0)
            {
                sb.Append(GetChlid(dv2, ds));
            }
            sb.Append("},");

            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            //获取查询条件
            string pcode = (context.Request["code"] ?? "").ToString().Trim();
            string pname = (context.Request["name"] ?? "").ToString().Trim();
            string productCategory = (context.Request["productCategory"] ?? "").ToString().Trim();
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (pcode.Length > 0)
            {
                SqlWhere.Append(" and  pcode like '%" + pcode + "%'");
            }
            if (productCategory.Length > 0)
            {
                SqlWhere.Append(" and  productCategory like '%" + productCategory + "%'");
            }

            if (pname.Length > 0)
            {
                SqlWhere.Append(" and  pname like '%" + pname + "%'");
            }

            IList<SqlParam> IList_param = new List<SqlParam>();

            BaseData_Dal basedata_idao = new BaseData_Dal();

            DataTable dt = basedata_idao.GetProductPage(SqlWhere, IList_param, page, row, ref count);
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                sb.Append("{\"total\":" + count + ",");
                sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                sb.Append("}");
            }
            return sb.ToString();



        }
        private string Save(HttpContext context)
        {        
            string pCategory = context.Request.QueryString["pCategory"];
            string pcode = context.Request.QueryString["pcode"];
            string pname = context.Request.QueryString["pname"];
            string unit = context.Request.QueryString["unit"];
            string spec = context.Request.QueryString["spec"];
            string packdes = context.Request.QueryString["packdes"];
            string packageUnit = context.Request.QueryString["packageUnit"];
            string pallet = context.Request.QueryString["pallet"];
            string hsscode = context.Request.QueryString["hsscode"];
            string ifinspection = context.Request.QueryString["ifinspection"];
            string addStatus = context.Request.QueryString["addStatus"];
            string status = context.Request.QueryString["status"];
            string methods = context.Request.QueryString["methods"];

            string pnameen = context.Request.QueryString["pnameen"];
            string pnameru = context.Request.QueryString["pnameru"];
            string subTable = context.Request.Params["subTable"];
             List<Hashtable> subList = new List<Hashtable>();
             List<StringBuilder> sqls = new List<StringBuilder>();
             List<object> objs = new List<object>();

             StringBuilder sql = new StringBuilder();

             if (addStatus == "add")
            {//pcode, pname, unit, spec, packdes, pallet, hsscode, ifinspection, status, createman, createdate, lastmod, lastmoddate
                sql.Append("Insert into bproduct(packageUnit, pcode,productCategory, pname, unit, spec, packdes, pallet, hsscode, ifinspection, status, createman, lastmod,pnameen,pnameru) values('" + packageUnit + "','" + pcode + "', '" + pCategory + "', '" + pname + "', '" + unit + "', '" + spec + "', '" + packdes + "', '" + pallet + "', '" + hsscode + "', '" + ifinspection + "', '" + status + "','" + RequestSession.GetSessionUser().UserId + "', '" + RequestSession.GetSessionUser().UserId + "', '" + pnameen + "', '" + pnameru + "')");
            }
             else if (addStatus == "edit")
            {
                sql.Append("Update bproduct set packageUnit='" + packageUnit + "',pname='" + pname + "',productCategory='" + pCategory + "', unit='" + unit + "', spec='" + spec + "', packdes='" + packdes + "',pallet='" + pallet + "', hsscode='" + hsscode + "', ifinspection='" + ifinspection + "',  lastmod='" + RequestSession.GetSessionUser().UserId + "', lastmoddate=getdate(), status='" + status + "', pnameen='" + pnameen + "', pnameru='" + pnameru + "' where  pcode = '" + pcode + "'");
            }
            if (!(string.IsNullOrEmpty(subTable)))
            {
                List<Hashtable> pro_list = new List<Hashtable>();
                subList = JsonHelper.DeserializeJsonToList<Hashtable>(subTable);
                foreach (var hs in subList)
                {
                    Hashtable ht_pro = new Hashtable();
                    ht_pro["pcode"] = pcode;
                    ht_pro["pname"] = pname;
                    ht_pro["buyer"] = hs["buyer"];
                    ht_pro["seller"] = hs["seller"];
                    ht_pro["price"] = hs["price"];
                    ht_pro["priceUnit"] = hs["priceUnit"];
                    ht_pro["spec"] = hs["spec"];
                    ht_pro["validityStart"] = hs["validityStart"];
                    ht_pro["validityEnd"] = hs["validityEnd"];
                    pro_list.Add(ht_pro);
                }
                SqlUtil.getBatchSqls(pro_list, "bproduct_price", "pcode", pcode.ToString(), ref sqls, ref objs);
                sqls.Add(sql);
                SqlParam[] tep_para = null;
                objs.Add(tep_para);
            }
            bool IsOk = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs) >= 0 ? true : false;
            if (IsOk)
            {
                if (methods == "0")
                {
                    return "0";
                }
                else if (methods == "1")
                {//添加
                    return "1";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
            /**List<string> list = new List<string>();
            list.Add(sql);
            if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0)
            {
                if (methods == "0")
                {
               return "0";           
                }
                else if (methods == "1")
                {//添加
                    return "1";
                }
                else
                {
                    return "false";
                }

            }
            else
            {
                return "false";
            }**/
        }


        private string add(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();
            ht = RequestHelper.getDataFromRequestForm_1(context);
            //根据操作类型进行相关操作（比如操作人、操作时间）
            if (ht.Contains("action"))
            {
                if ("add".Equals(ht["action"]))
                {//添加

                }
                if ("edit".Equals(ht["action"]))
                {//修改

                }
                ht.Remove("action");
            }
            //获取子表信息
            string subDataJson = "";
            if (ht.Contains("datagrid"))
            {
                subDataJson = ht["datagrid"].ToString();
                ht.Remove("datagrid");
            }
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            //生成主表sql
            SqlUtil.getBatchSqls(ht, "bproduct", "PCODE", ht["PCODE"].ToString(), ref sqls, ref objs);

            List<Hashtable> list = new List<Hashtable>();
            if (!(string.IsNullOrEmpty(subDataJson)))
            {
                list = JsonHelper.DeserializeJsonToList<Hashtable>(subDataJson);
                string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
                string man = RequestSession.GetSessionUser().UserName.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Contains("ROWNUM"))
                    {
                        list[i].Remove("ROWNUM");
                    }
                    if (!(list[i].Contains("DOCNO")))
                    {
                        list[i].Add("DOCNO", ht["DOCNO"].ToString());
                        list[i].Add("CREATEDATE", time);
                        list[i].Add("CREATEMAN", man);
                        list[i].Add("LASTMOD", man);
                        list[i].Add("LASTMODDATE", time);
                    }
                }
                SqlUtil.getBatchSqls(list, "bproduct_price", "DOCNO", ht["DOCNO"].ToString(), ref sqls, ref objs);
            }

            bool IsOk = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs) >= 0 ? true : false;
            if (IsOk)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "操作成功！");
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);
        }

        private string edit(HttpContext context)
        {
            return "";
        }

        private string Del(HttpContext context)
        {
            string pcode = context.Request.QueryString["pcode"];
            string sql = "delete from bproduct where pcode='" + pcode + "'";

            List<string> list = new List<string>();
            list.Add(sql);
            if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0)
            {
                return "true";
                //context.Response.Write("true");
            }
            else
            {
                return "false";
                //context.Response.Write("false");
            }


        }
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct productCategory ");
            strSql.Append(" FROM bproduct ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DataFactory.SqlDataBaseExpand().Query(strSql.ToString());
        }
        protected StringBuilder GetChlid(DataView dv, DataSet ds)
        {
            StringBuilder sb = new StringBuilder();
            //Com_UserInfos ubll = new Com_UserInfos();
            //WZX.Model.Com_UserInfos user = null;
            sb.Append(",\"children\":[");

            for (int i = 0; i < dv.Count; i++)
            {
                sb.Append("{");

                sb.Append("\"text\":\"" + dv[i]["productCategory"] + "\"");



                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb;
        }
        private string getSubList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            string pcode = context.Request["pcode"] == null ? "" : context.Request["pcode"].ToString();

            BD_Dal bd_dal = new BD_Dal();
            DataTable dt = bd_dal.GetProductPrice(pcode);
            StringBuilder sb = new StringBuilder();
            count = dt.Rows.Count;
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                sb.Append("{\"total\":" + count + ",");
                sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                sb.Append("}");
            }
            return sb.ToString();
        }

        private bool saveImportData(ref string err, HttpContext context)
        {
            string datagridJson = context.Request.Params["datagridJson"];
            List<Hashtable> excelList = JsonHelper.DeserializeJsonToList<Hashtable>(datagridJson);
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            SqlUtil.getBatchSqls(excelList, "bproduct", ref sqls, ref objs);
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        }
        #region 返回json格式

        private string returnData(bool isok, string err)
        {
            string r = "";
            if (isok)
            {
                r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            }
            else
            {
                r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            }
            return r;
        }

        private string returnData1(bool isok, string err, string template, string lanugage)
        {
            string r = "";
            if (isok)
            {
                r = "{\"sucess\": 1,\"contract\": \"" + err + "\",\"template\": \"" + template + "\",\"language\": \"" + lanugage + "\",\"warnmsg\": \"\",\"errormsg\": \"\"}";
            }
            else
            {
                r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            }
            return r;
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}