using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using System.Data;
using RM.Common;
using RM.Busines.DAL;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetJson;
using WZX.Busines.Util;

namespace RM.Web.ashx.PaymentManage
{
    /// <summary>
    /// PaymentDomesticHandler 的摘要说明
    /// </summary>
    public class PaymentDomesticHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string type = context.Request["type"] == null ? "" : context.Request["type"].ToString();
            switch (type){
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;
                case "getSubList"://获取子表列表
                    context.Response.Write(getSubList(context));
                    break;
                case "add"://添加
                    context.Response.Write(add(context));
                    break;
                case "edit"://修改
                    context.Response.Write(edit(context));
                    break;
                case "del"://删除
                    context.Response.Write(del(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
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
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            IList<SqlParam> IList_param = new List<SqlParam>();

            Payment_IDAO payment_idao = new Payment_Dal();

            DataTable dt = payment_idao.GetDomesticPaymentPage(SqlWhere, IList_param, page, row, ref count);
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
        /// 分页获取子表列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            string payno = context.Request["payNo"] == null ? "" : context.Request["payNo"].ToString();
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and payNo=@payNo ");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@payNo", payno));

            Payment_IDAO payment_idao = new Payment_Dal();

            DataTable dt = payment_idao.GetDomesticPaymentSubPage(SqlWhere, IList_param, page, row, ref count);
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

            ht["payNo"] = context.Request["payNo"] == null ? "" : context.Request["payNo"].ToString();
            ht["receiver"] = context.Request["receiver"] == null ? "" : context.Request["receiver"].ToString();
            ht["payer"] = context.Request["payer"] == null ? "" : context.Request["payer"].ToString();
            ht["paydate"] = context.Request["paydate"] == null ? "" : context.Request["paydate"].ToString();
            ht["payamount"] = context.Request["payamount"] == null ? "" : context.Request["payamount"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "" : context.Request["currency"].ToString();
            ht["payrate"] = context.Request["payrate"] == null ? "" : context.Request["payrate"].ToString();
            ht["applyNo"] = context.Request["applyNo"] == null ? "" : context.Request["applyNo"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();

            string subDataJson = context.Request["datagrid"] == null ? "" : context.Request["datagrid"].ToString();
            List<Hashtable> list = new List<Hashtable>();
            list = JsonHelper.DeserializeJsonToList<Hashtable>(subDataJson);

            StringBuilder[] sqls = new StringBuilder[list.Count];
            object[] objs = new object[list.Count];
            SqlUtil.getBatchFromList(list, ConstantUtil.TABLE_PAY_DOMESTIC_D, ref sqls, ref objs);

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit(ConstantUtil.TABLE_PAY_DOMESTIC, "payNo", ht["payNo"].ToString(), ht);
            if (IsOk)
            {
                bool flag = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0 ? true : false;
                if (flag)
                {
                    return "操作成功！";
                }
                else 
                {
                    //删除主表
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_PAY_DOMESTIC, "payNo", ht["payNo"].ToString());
                    return "操作失败！";
                }
               
            }
            else
            {
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
            return "";
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