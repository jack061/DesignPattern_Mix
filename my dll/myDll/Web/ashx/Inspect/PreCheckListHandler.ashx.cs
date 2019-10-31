using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines.DAL.Inspect;
using RM.Common.DotNetJson;
using System.Collections;
using WZX.Busines.Util;
using RM.Busines;

namespace RM.Web.ashx.Inspect
{
    /// <summary>
    /// CheckManage 的摘要说明
    /// </summary>
    public class PreCheckListHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "getProductList"://获取产品预验信息
                    context.Response.Write(getProductList(context));
                    break;
                case "getPackList"://获取包装预验信息
                    context.Response.Write(getPackList(context));
                    break;
                case "delProduct"://删除产品预验信息
                    context.Response.Write(delProduct(context));
                    break;
                case "delPack"://删除包装预验信息
                    context.Response.Write(delPack(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        /// <summary>
        /// 获取产品预验信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string getProductList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;

            //获取查询条件
            string inspectionno = (context.Request["INSPECTIONNO"] ?? "").ToString().Trim();
            string inspecman = (context.Request["INSPECMAN"] ?? "").ToString().Trim();
            string pname = (context.Request["PNAME"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (inspectionno.Length > 0)
            {
                SqlWhere.Append(" and  inspectionNo like '%" + inspectionno + "%'");
            }
            if (inspecman.Length > 0)
            {
                SqlWhere.Append(" and  inspecman like '%" + inspecman + "%'");
            }
            if (pname.Length > 0)
            {
                SqlWhere.Append(" and  pname like '%" + pname + "%'");
            }
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = PreCheck_Dal.GetProductList(SqlWhere, IList_param, page, row, ref count);
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
        /// 获取包装预验信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string getPackList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;

            //获取查询条件
            string inspectionno = (context.Request["INSPECTIONNO"] ?? "").ToString().Trim();
            string pname = (context.Request["PNAME"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (inspectionno.Length > 0)
            {
                SqlWhere.Append(" and  inspectionNo like '%" + inspectionno + "%'");
            }
            if (pname.Length > 0)
            {
                SqlWhere.Append(" and  pname like '%" + pname + "%'");
            }
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = PreCheck_Dal.GetPackList(SqlWhere, IList_param, page, row, ref count);
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
        /// 删除产品预验信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string delProduct(HttpContext context){
            Hashtable ht_result = new Hashtable();

            string inspectionNo = string.IsNullOrEmpty(context.Request["inspectionNo"]) ? "" : context.Request["inspectionNo"].ToString();
            StringBuilder sb = new StringBuilder();
            SqlParam[] param = { new SqlParam("@inspectionNo", inspectionNo) };
            sb.Append(" delete " + ConstantUtil.TABLE_INSPECTIONRE + " where inspectionNo  = @inspectionNo");
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
        
        /// <summary>
        /// 删除包装预验信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string delPack(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();

            string inspectionNo = string.IsNullOrEmpty(context.Request["inspectionNo"]) ? "" : context.Request["inspectionNo"].ToString();
            StringBuilder sb = new StringBuilder();
            SqlParam[] param = { new SqlParam("@inspectionNo", inspectionNo) };
            sb.Append(" delete " + ConstantUtil.TABLE_INSPECTIONREPACK + " where inspectionNo  = @inspectionNo");
            sb.Append(" delete " + ConstantUtil.TABLE_INSPECTIONREPACK2 + " where inspectionNo  = @inspectionNo");
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}