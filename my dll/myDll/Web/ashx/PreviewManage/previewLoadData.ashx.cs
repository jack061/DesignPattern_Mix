using RM.Busines;
using RM.Busines.DAL.Inspect;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.PreviewManage
{
    /// <summary>
    /// previewLoadData 的摘要说明
    /// </summary>
    public class previewLoadData : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "getProductPreMList"://获取产品列表
                    context.Response.Write(getProductPreMList(context));
                    break;
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
        /// 获取产品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string getProductPreMList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;

            //获取查询条件
            string previewCode = (context.Request["previewCode"] ?? "").ToString().Trim();
            string deliveryMan = (context.Request["deliveryMan"] ?? "").ToString().Trim();
            string productName = (context.Request["productName"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (previewCode.Length > 0)
            {
                SqlWhere.Append(" and  previewCode like '%" + previewCode + "%'");
            }
            if (deliveryMan.Length > 0)
            {
                SqlWhere.Append(" and  inspecman like '%" + deliveryMan + "%'");
            }
            if (productName.Length > 0)
            {
                SqlWhere.Append(" and  pname like '%" + productName + "%'");
            }
            SqlWhere.AppendFormat(" and createman='{0}'", RequestSession.GetSessionUser().UserAccount);
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = PreCheck_Dal.GetProductPreMList(SqlWhere, IList_param, page, row, ref count);
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
            string previewCode = (context.Request["previewCode"] ?? "").ToString().Trim();
            string deliveryMan = (context.Request["deliveryMan"] ?? "").ToString().Trim();
            string productName = (context.Request["productName"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (previewCode.Length > 0)
            {
                SqlWhere.Append(" and  previewCode like '%" + previewCode + "%'");
            }
            if (deliveryMan.Length > 0)
            {
                SqlWhere.Append(" and  inspecman like '%" + deliveryMan + "%'");
            }
            if (productName.Length > 0)
            {
                SqlWhere.Append(" and  pname like '%" + productName + "%'");
            }
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = PreCheck_Dal.GetProductList1(SqlWhere, IList_param, page, row, ref count);
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
        public string delProduct(HttpContext context)
        {
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