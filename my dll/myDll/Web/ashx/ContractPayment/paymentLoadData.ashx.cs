using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WZX.Busines.Util;

namespace RM.Web.ashx.ContractPayment
{
    /// <summary>
    /// paymentLoadData 的摘要说明
    /// </summary>
    public class paymentLoadData : IHttpHandler
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
                case "getHTList1"://获取合同列表
                    context.Response.Write(getHTList1(context));
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
                case "getContactCode"://加载销售合同
                    context.Response.Write(getContactCode(context));
                    break;
                case "getPurchaseCode"://加载采购合同
                    context.Response.Write(getPurchaseCode(context));
                    break;

                case "GetCashPayment":
                    context.Response.Write(GetCashPayment(context));
                    break;
                case "GetBussinessMan"://获取业务员
                    context.Response.Write(GetBussinessMan(context));
                    break;
                #region 绑定数据到form上
                case "LoadPayMent":
                    context.Response.Write(LoadPayMent(context));
                    break;
                case "LoadContract":
                    context.Response.Write(LoadContract(context));
                    break;
                #endregion
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        private string LoadPayMent(HttpContext context) {
            return null;
        }
        private string LoadContract(HttpContext context)
        {
            return "";
        }
        private string getContactCode(HttpContext context)
        {
            //根据客户名和收款户名加载销售合同和采购合同
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"]??"desc";
            string sort = context.Request["sort"] ?? "contractNo";
            string accountName = (context.Request["accountName"] ?? "").ToString().Trim();
            string contractClient = (context.Request["contractClient"] ?? "").ToString().Trim();
            string saleman = (context.Request["saleman"] ?? "").ToString().Trim();

            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder();
            if (!string.IsNullOrEmpty(accountName))
            {
                sqlwhere.Append(" and seller=@seller ");
            }
            if (!string.IsNullOrEmpty(contractClient))
            {
                sqlwhere.Append(" and buyer=@buyer ");
            } 
            if (!string.IsNullOrEmpty(saleman))
            {
                sqlwhere.Append(" and salesmanCode=@saleman ");
            }
            SqlParameter[] pms = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@seller",Value=accountName,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=contractClient,DbType=DbType.String},
                    new SqlParameter{ParameterName="@saleman",Value=saleman,DbType=DbType.String},
                };

            sqldata.Append(@"select pricement1,pricement2,contractNo,contractAmount,item1Amount,item2Amount,paidAmount,unpaidAmount,unpaidAmount as payingAmount,paystatus from Econtract where paystatus='未收款' " + sqlwhere.ToString());
            sqlcount.Append("select count(1) from Econtract t2 where 1=1 " + sqlwhere.ToString());
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);
            return sb.ToString();
        }

        private string getPurchaseCode(HttpContext context)
        {
            //根据关联合同号加载采购合同
            int row = int.Parse((context.Request["rows"]??"10").ToString());
            int page = int.Parse((context.Request["page"]??"0").ToString());
            string order = context.Request["order"] ?? "desc";
            string sort = context.Request["sort"] ?? "contractNo";
            //string accountName = (context.Request["accountName"] ?? "").ToString().Trim();
            string purchaseCode = context.Request["saleContractNo"].ToString();
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(purchaseCode))
            {
                sqlwhere.Append("and purchaseCode=@purchaseCode ");
            }
            SqlParameter[] pms = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@purchaseCode",Value=purchaseCode,DbType=DbType.String},
                };

            sqldata.Append(@"select * from Econtract  where" + sqlwhere.ToString());
            sqlcount.Append("select count(1) from Econtract t2 where " + sqlwhere.ToString());
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);
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

            string contractClient = (context.Request["contractClient"] ?? "").ToString().Trim();
            string saleman = (context.Request["saleman"] ?? "").ToString().Trim();
            string partInfo = (context.Request["partInfo"] ?? "").ToString().Trim();
            string beginTime = (context.Request["beginTime"] ?? "").ToString().Trim();
            string endTime = (context.Request["endTime"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            //SqlWhere.Append(" and status=1");
            if (contractClient.Length > 0)
            {
                SqlWhere.Append(" and  contractClient like '%" + contractClient + "%'");
            }
            if (saleman.Length > 0)
            {
                SqlWhere.Append(" and  saleman like '%" + saleman + "%'");
            }
            if (partInfo.Length > 0)
            {
                SqlWhere.Append(" and  partInfo like '%" + partInfo + "%'");
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

            DataTable dt = payment_idao.GetPaymentPage(SqlWhere, IList_param, page, row, ref count);
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
        /// 欢迎页面上显示的信息 待匹配合同现汇收款
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCashPayment(HttpContext context)
        {
            int row = 10;
            int page = 1;
            int count = 0;

            //查询条件
            StringBuilder SqlWhere = new StringBuilder(" and partInfo='否' ");
            IList<SqlParam> IList_param = new List<SqlParam>();

            Payment_IDAO payment_idao = new Payment_Dal();

            DataTable dt = payment_idao.GetPaymentPage(SqlWhere, IList_param, page, row, ref count);
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
            int row = int.Parse(context.Request["rows"]??"10");
            int page = int.Parse(context.Request["page"]??"1");
            int count = 0;
            string payno = context.Request["payNo"] == null ? "" : context.Request["payNo"].ToString();

            //查询条件

            StringBuilder SqlWhere = new StringBuilder();

            SqlWhere.Append(" and payNo=@payNo ");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@payNo", payno));

            Payment_IDAO payment_idao = new Payment_Dal();

            DataTable dt = payment_idao.GetPaymentSubPage(SqlWhere, IList_param, page, row, ref count);
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    dt.Rows[i]["UNPAIDAMOUNT"] = Convert.ToDouble(dt.Rows[i]["UNPAIDAMOUNT"]) - Convert.ToDouble(dt.Rows[i]["PAYINGAMOUNT"]);
                    dt.Rows[i]["PAIDAMOUNT"] = Convert.ToDouble(dt.Rows[i]["PAIDAMOUNT"]) + Convert.ToDouble(dt.Rows[i]["PAYINGAMOUNT"]);
                }

                dt.Columns.Remove(dt.Columns["ROWNUM"]); 
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
        private string getHTList1(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            string payerName = context.Request["payerName"] == null ? "" : context.Request["payerName"].ToString();
            string contractCode = context.Request["contractCode"] == null ? "" : context.Request["contractCode"].ToString();
            string payerCode = context.Request["payerCode"] == null ? "" : context.Request["payerCode"].ToString();
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            //根据付款方筛选合同,如果不存在付款方，加载全部合同
            SqlWhere.Append(" where 1=1");
            if (payerName.Length > 0)
            {

                SqlWhere.Append(" and a.buyer like '%" + payerName + "%'");
            }
            //根据合同编号和付款方筛选合同
            if (contractCode.Length > 0)
            {

                SqlWhere.Append(" and a.contractNO like '%" + contractCode + "%'");
            }
            if (payerCode.Length > 0)
            {
                SqlWhere.Append(" and a.buyer like '%" + payerCode + "%'");
            }
            IList<SqlParam> IList_param = new List<SqlParam>();

            Payment_IDAO payment_idao = new Payment_Dal();
            //DataTable dt = payment_idao.GetAbroadHTPage1(SqlWhere, IList_param, page, row, ref count);
            string sb = "";
            sb = payment_idao.GetAbroadHTPage1(SqlWhere, IList_param, page, row, ref count);
            //if (!(DataTableHelper.IsExistRows(dt)))
            //{
            //    //如果不存在，加载全部的合同
            //    StringBuilder SqlThere= new StringBuilder();
            //    //根据合同编号和付款方筛选合同
            //    if (contractCode.Length > 0)
            //    {

            //        SqlThere.Append(" and a.contractNO like '%" + contractCode + "%'");
            //    }
            //    if (payerCode.Length > 0)
            //    {
            //        SqlThere.Append(" and a.buyer like '%" + payerCode + "%'");
            //    }
            //  sb = payment_idao.GetAbroadHTPage1(SqlThere, IList_param, page, row, ref count);
            //  return sb;
            //    //sb.Append("{\"total\":" + count + ",");
            //    //sb.Append(JsonHelper.DataTableToJson_(ds, "rows"));
            //    //sb.Append("}");
            
          
            //}
            //else
            //{
            //    sb.Append("{\"total\":" + count + ",");
            //    sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
            //    sb.Append("}");
            //}
            return sb.ToString();
        }

        private string getHTList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            string payerName = context.Request["payerName"] == null ? "" : context.Request["payerName"].ToString();
            string contractCode = context.Request["contractCode"] == null ? "" : context.Request["contractCode"].ToString();
            string payerCode = context.Request["payerCode"] == null ? "" : context.Request["payerCode"].ToString();
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            //根据付款方筛选合同
            if (payerName.Length > 0)
            {

                SqlWhere.Append(" and a.buyer like '%" + payerName + "%'");
            }
            //根据合同编号和付款方筛选合同
            if (contractCode.Length > 0)
            {

                SqlWhere.Append(" and a.contractNO like '%" + contractCode + "%'");
            }
            if (payerCode.Length > 0)
            {
                SqlWhere.Append(" and a.buyer like '%" + payerCode + "%'");
            }
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
            else
            {
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


        private string GetBussinessMan(HttpContext context)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"select us.*,org.Agency
                                                                from Com_Organization org
                                                                join Com_OrgAddUser orUs on orUs.OrgId=org.Id
                                                                right join Com_UserInfos us on us.Userid=orUs.UserId
                                                                where org.Agency like '%业务%'"));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder ret = new StringBuilder("[");

                foreach (DataRow dr in dt.Rows)
                {
                    ret.Append(JsonHelper.DataRowToJson_(dr) + ",");
                }
                ret = ret.Remove(ret.Length - 1, 1);
                ret.Append("]");
                return ret.ToString();
            }
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