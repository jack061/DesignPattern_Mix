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
using RM.Common.DotNetBean;
using System.Web.SessionState;

namespace RM.Web.ashx.PaymentManage
{
    /// <summary>
    /// PaymentAbroadHandler 的摘要说明
    /// </summary>
    public class PaymentAbroadHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (type)
            {
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;
                case "getSubList"://获取子表列表
                    context.Response.Write(getSubList(context));
                    break;
                case "getHTList"://获取合同列表
                    context.Response.Write(getHTList(context));
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

            string payno = (context.Request["payno"] ?? "").ToString().Trim();
            string payer = (context.Request["payer"] ?? "").ToString().Trim();
            string beginTime = (context.Request["beginTime"] ?? "").ToString().Trim();
            string endTime = (context.Request["endTime"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            if (payno.Length > 0)
            {
                SqlWhere.Append(" and  payno like '%" + payno + "%'");
            }
            if (payer.Length > 0)
            {
                SqlWhere.Append(" and  payer like '%" + payer + "%'");
            }
            if (beginTime.Length > 0)
            {
                SqlWhere.Append(" and  paydate >= '" + beginTime + "'");
            }
            if (endTime.Length > 0)
            {
                SqlWhere.Append(" and  paydate <= '" + endTime + "'");
            }
            IList<SqlParam> IList_param = new List<SqlParam>();

            Payment_IDAO payment_idao = new Payment_Dal();

            DataTable dt = payment_idao.GetAbroadPaymentPage(SqlWhere, IList_param, page, row, ref count);
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

            DataTable dt = payment_idao.GetAbroadPaymentSubPage(SqlWhere, IList_param, page, row, ref count);
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
        /// 分页获取合同选择列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getHTList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();

            Payment_IDAO payment_idao = new Payment_Dal();

            DataTable dt = payment_idao.GetAbroadHTPage(SqlWhere, IList_param, page, row, ref count);
            StringBuilder sb = new StringBuilder();
            if (!(DataTableHelper.IsExistRows(dt)))
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
            ht["payamount"] = context.Request["payamount"] == null ? "0" : context.Request["payamount"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "" : context.Request["currency"].ToString();
            ht["payrate"] = context.Request["payrate"] == null ? "0" : context.Request["payrate"].ToString();
            ht["status"] = context.Request["status"] == null ? "" : context.Request["status"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            if ("edit".Equals(context.Request["action"] == null ? "" : context.Request["action"].ToString()))
            {
                ht["createman"] = context.Request["createman"] == null ? RequestSession.GetSessionUser().UserName : context.Request["createman"].ToString();
                ht["createdate"] = context.Request["createdate"] == null ? time : context.Request["createdate"].ToString();
            }
            else {
                ht["createman"] = RequestSession.GetSessionUser().UserName;
                ht["createdate"] = time;
                ht["lastmod"] = RequestSession.GetSessionUser().UserName;
                ht["lastmoddate"] = time; 
            }
            
            string subDataJson = context.Request["datagrid"] == null ? "" : context.Request["datagrid"].ToString();
            List<Hashtable> list = new List<Hashtable>();
            list = JsonHelper.DeserializeJsonToList<Hashtable>(subDataJson);
            StringBuilder[] sqls = new StringBuilder[list.Count];
            object[] objs = new object[list.Count];
            SqlUtil.getBatchFromList(list, ConstantUtil.TABLE_PAY_ABROAD_D, ref sqls, ref objs);

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_PAY_ABROAD, "payNo", ht["payNo"].ToString(), ht);
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