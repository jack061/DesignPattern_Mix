using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using System.Data;
using RM.Common;
using RM.Common.DotNetJson;
using System.Collections;
using WZX.Busines.Util;
using RM.Busines;
using RM.Common.DotNetData;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// ProducthssListHandler 的摘要说明
    /// </summary>
    public class ProducthssListHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;
                case "add"://添加
                    context.Response.Write(add(context));
                    break;
                case "edit"://修改
                    context.Response.Write(edit(context));
                    break;
                case "getTree":
                    context.Response.Write(getTree(context));
                    break;
                case "del"://删除
                    context.Response.Write(del(context));
                    break;
                case "getCategory":
                    context.Response.Write(getCategory(context));
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
                DataSet ds = bll.ExecDatasetSql(@"select distinct productCategory as name from " + ConstantUtil.TABLE_BUS_PRODUCTHSS);
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
            string pcode = (context.Request["pcode"] ?? "").ToString().Trim();
            string pname = (context.Request["pname"] ?? "").ToString().Trim();
            string productCategory = (context.Request["productCategory"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (pcode.Length > 0)
            {
                SqlWhere.Append(" and  pcode like '%" + pcode + "%'");
            }
            if (pname.Length > 0)
            {
                SqlWhere.Append(" and  pname like '%" + pname + "%'");
            }
            if (productCategory.Length > 0)
            {
                SqlWhere.Append(" and  productCategory like '%" + productCategory + "%'");
            }
           
            IList<SqlParam> IList_param = new List<SqlParam>();

            BD_IDAO bd_idao = new BD_Dal();

            DataTable dt = bd_idao.GetProducthssList(SqlWhere, IList_param, page, row, ref count);
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
      
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string add(HttpContext context)
        {
            Hashtable ht = new Hashtable();
            string id = context.Request["id"] == null ? "" : context.Request["id"].ToString();
            ht["productCategory"] = context.Request["productCategory"] == null ? "" : context.Request["productCategory"].ToString();
            ht["pcode"] = context.Request["pcode"] == null ? "" : context.Request["pcode"].ToString();
            ht["pname"] = context.Request["pname"] == null ? "" : context.Request["pname"].ToString();
            ht["condition"] = context.Request["condition"] == null ? "" : context.Request["condition"].ToString();
            ht["unit1"] = context.Request["unit1"] == null ? "" : context.Request["unit1"].ToString();
            ht["unit2"] = context.Request["unit2"] == null ? "" : context.Request["unit2"].ToString();
            ht["rate1"] = context.Request["rate1"] == null ? "" : context.Request["rate1"].ToString();
            ht["rate2"] = context.Request["rate2"] == null ? "" : context.Request["rate2"].ToString();
            ht["rate3"] = context.Request["rate3"] == null ? "" : context.Request["rate3"].ToString();
            ht["rate4"] = context.Request["rate4"] == null ? "" : context.Request["rate4"].ToString();
            ht["rate5"] = context.Request["rate5"] == null ? "" : context.Request["rate5"].ToString();
            ht["rate6"] = context.Request["rate6"] == null ? "" : context.Request["rate6"].ToString();
            ht["rate7"] = context.Request["rate7"] == null ? "" : context.Request["rate7"].ToString();
            ht["rate8"] = context.Request["rate8"] == null ? "" : context.Request["rate8"].ToString();
            ht["priceflag"] = context.Request["priceflag"] == null ? "" : context.Request["priceflag"].ToString();
            ht["rateflag"] = context.Request["rateflag"] == null ? "" : context.Request["rateflag"].ToString();
            ht["inspection"] = context.Request["inspection"] == null ? "" : context.Request["inspection"].ToString();
            ht["pnameen"] = context.Request["pnameen"] == null ? "" : context.Request["pnameen"].ToString();
            ht["pnameru"] = context.Request["pnameru"] == null ? "" : context.Request["pnameru"].ToString();
            ht["ifinspection"] = context.Request["ifinspection"] == null ? "" : context.Request["ifinspection"].ToString();

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_BUS_PRODUCTHSS, "id", id, ht);
            if (IsOk)
            {
                    return "ok";              
            }
            else{
                return "操作失败！";
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string edit(HttpContext context)
        {
            return "";
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string del(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();

            string pcode = context.Request.QueryString["pcode"];
            StringBuilder sb = new StringBuilder();
            SqlParam[] param = { new SqlParam("@pcode", pcode) };
            sb.Append(" delete " + ConstantUtil.TABLE_BUS_PRODUCTHSS + " where pcode  = @pcode");
            if (DataFactory.SqlDataBase().ExecuteBySql(sb, param) > 0)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "操作成功");
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);
        }
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct productCategory ");
            strSql.Append(" FROM  " + ConstantUtil.TABLE_BUS_PRODUCTHSS);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}