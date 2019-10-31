using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Common.DotNetJson;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;
using RM.Busines;

namespace RM.Web.ashx.InvoiceAndPacking
{
    /// <summary>
    /// InvoiceAndPackingHandler 的摘要说明
    /// </summary>
    public class InvoiceAndPackingHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "getSubInvoice"://获取列表
                    context.Response.Write(getSubInvoice(context));
                    break;
                case "getSubPacking"://获取列表
                    context.Response.Write(getSubPacking(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        /// <summary>
        /// 获取商业发票子表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubInvoice(HttpContext context)
        {

            //获取合同号
            string no = (context.Request["no"] ?? "").ToString().Trim();
            StringBuilder sb = new StringBuilder();
            if (no.Length > 0)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from Econtract_ap where contractNo = '" + no + "'");
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
                
                if (dt.Rows.Count == 0)
                {
                    sb.Append("{\"total\":0,\"rows\":[]}");
                }
                else
                {
                    DataTable dt_count = new DataTable();
                    dt_count = getInvoiceGridFooterTable(dt);
                    sb.Append("{\"total\":" + dt.Rows.Count + ",");
                    sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                    sb.Append(",");
                    sb.Append(JsonHelper.DataTableToJson_(dt_count, "footer"));
                    sb.Append("}");
                }
            }
            else 
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }

           
            return sb.ToString();
        }

        /// <summary>
        /// 获取箱单子表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubPacking(HttpContext context)
        {
            //获取合同号
            string no = (context.Request["no"] ?? "").ToString().Trim();
            StringBuilder sb = new StringBuilder();
            if (no.Length > 0)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from Econtract_ap where contractNo = '" + no + "'");
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

                if (dt.Rows.Count == 0)
                {
                    sb.Append("{\"total\":0,\"rows\":[]}");
                }
                else
                {
                    DataTable dt_count = new DataTable();
                    dt_count = getPackingGridFooterTable(dt);
                    sb.Append("{\"total\":" + dt.Rows.Count + ",");
                    sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                    sb.Append(",");
                    sb.Append(JsonHelper.DataTableToJson_(dt_count, "footer"));
                    sb.Append("}");
                }
            }
            else
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }


            return sb.ToString();
        }

        #region 获取报表底部数据

        /// <summary>
        /// 获取发票griddata footer 统计表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable getInvoiceGridFooterTable(DataTable dt)
        {
            DataTable dt_count = dt.Clone();
            dt_count.Clear();
            DataRow dr = dt_count.NewRow();
            dr["PCODE"] = "总计";
            dr["QUANTITY"] = string.IsNullOrEmpty(dt.Compute("Sum(QUANTITY)","").ToString()) ? 0 : dt.Compute("Sum(QUANTITY)","");
            dr["AMOUNT"] = string.IsNullOrEmpty(dt.Compute("Sum(AMOUNT)", "").ToString()) ? 0 : dt.Compute("Sum(AMOUNT)", "");
            dt_count.Rows.Add(dr);
            return dt_count;
        }
        /// <summary>
        /// 获取箱单griddata footer 统计表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable getPackingGridFooterTable(DataTable dt)
        {
            DataTable dt_count = dt.Clone();
            dt_count.Clear();
            DataRow dr = dt_count.NewRow();

            dr["PCODE"] = "总计";
            dr["QUANTITY"] = string.IsNullOrEmpty(dt.Compute("Sum(QUANTITY)", "").ToString()) ? 0 : dt.Compute("Sum(QUANTITY)", "");
            //件数
            dr["XXXXXX"] = string.IsNullOrEmpty(dt.Compute("Sum(XXXXXX)", "").ToString()) ? 0 : dt.Compute("Sum(XXXXXX)", "");
            //毛重
            dr["XXXXXX"] = string.IsNullOrEmpty(dt.Compute("Sum(XXXXXX)", "").ToString()) ? 0 : dt.Compute("Sum(XXXXXX)", "");
            //净重
            dr["XXXXXX"] = string.IsNullOrEmpty(dt.Compute("Sum(XXXXXX)", "").ToString()) ? 0 : dt.Compute("Sum(XXXXXX)", "");
            dt_count.Rows.Add(dr);
            return dt_count;
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