using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PDA_Service.DataBase.DataBase.SqlServer;
using RM.Busines;
using WZX.Busines.Util;
using System.Data;
using RM.Common.DotNetJson;
using System.Text;
using System.Collections;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using System.Data.SqlClient;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using System.Web.SessionState;

namespace RM.Web.ashx.ContractPayment
{
    /// <summary>
    /// paymentCredit 的摘要说明
    /// </summary>
    public class paymentCredit : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action) { 
                case "getContractList"://获取所有的合同
                    context.Response.Write(getContractList(context));
                    break;
                case "GetContractBS"://根据收款户名、合同客户、销售员、销售组确定合同范围
                    context.Response.Write(GetContractBS(context));
                    break;
                case "GetBuyContract"://获取采购合同
                    context.Response.Write(GetBuyContract(context));
                    break;
                case "add"://新增信用证开立
                    context.Response.Write(add(context));
                    break;
                case "AddPart":
                    context.Response.Write(AddPart(context));
                    break;
                case "GetPayRecieve"://获取付款项
                    context.Response.Write(GetPayRecieve(context));
                    break;
                case "GetSubTable": //获取合同表
                    context.Response.Write(GetSubTable(context));
                    break;
                //信用证开立列表list
                case "GetList":
                    context.Response.Write(GetList(context));
                    break;
                case "del":
                    context.Response.Write(del(context));
                    break;

                //信用证收款
                case "GetCredit":
                    context.Response.Write(GetCredit(context));
                    break;
                case "save":
                    context.Response.Write(save(context));
                    break;
                case "GetContract":
                    context.Response.Write(GetContract(context));
                    break;
                case "GetContractOne":
                    context.Response.Write(GetContractOne(context));
                    break;
                case "addCredit":
                    context.Response.Write(addCredit(context));
                    break;
                default: 
                    context.Response.Write("");
                    break;
            }
        }

        #region FORM
        // 根据payNo获取特定的PayRecieve行
        private string GetPayRecieve(HttpContext context) {
            string payNo = context.Request.QueryString["payNo"];
            string sk=context.Request.Params["sk"]??"";
            string sql = string.Empty;
            if (string.IsNullOrWhiteSpace(sk))
            {
                sql = string.Format(@"
                                        select businessclass,seller,buyer,salesmanCode,{0}.* from {0}
                                        left join 
                                        (select payNo,businessclass,seller,buyer,salesmanCode from Econtract  
                                        join {1} on {1}.contractNo=Econtract.contractNo                                        
                                        ) con on  con.payNo={0}.payNo where {0}.payNo='{2}'",
                                             ConstantUtil.TALBE_PAYCREDIT, ConstantUtil.TALBE_PAYCREDITDETAILS, payNo);
            }
            else {
                sql = string.Format(@"
                                        select businessclass,seller,buyer,salesmanCode,{0}.* from {0}
                                        left join 
                                        (select payNo,businessclass,seller,buyer,salesmanCode from Econtract  
                                        join {1} on {1}.contractNo=Econtract.contractNo                                        
                                        ) con on  con.payNo={0}.payNo where {0}.payNo='{2}'",
                                             ConstantUtil.TABLE_PAY_Receive, ConstantUtil.TABLE_PAY_ReceiveDetails, payNo);
            }
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            string result = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            }
            else {
                result = "{}";
            }
            return result;
        }
        // 获取到款选中的表
        private string GetSubTable(HttpContext context) {
            string payNo = context.Request.QueryString["payNo"];
            string sk = context.Request.Params["sk"] ?? "";//如果sk非空，那么查询的是收款明细
            string sql = string.Empty;
            if (string.IsNullOrWhiteSpace(sk))
            {
                sql = string.Format(@"select * from Econtract  
                            where contractNo in
                            (select contractNo 
                            from {0}
                            where payNo='{1}'
                            )", ConstantUtil.TALBE_PAYCREDITDETAILS, payNo);
            }
            else {
                sql = string.Format(@"select * from Econtract  
                            where contractNo in
                            (select contractNo 
                            from {0}
                            where payNo='{1}'
                            )", ConstantUtil.TABLE_PAY_ReceiveDetails, payNo);
            }
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            string result = string.Empty;
            if (dt == null || dt.Rows.Count == 0)
            {
                result="{\"total\":0,\"rows\":[]}";
            }
            else
            {
                result += "{\"total\":" + dt.Rows.Count + ",";
                result +=JsonHelper.DataTableToJson_(dt, "rows");
                result +="}";
            }
            return result;
        }
        //保存到信用证开立表&信用证到款？
        private string add(HttpContext context) {
            string result = string.Empty;
            string err = "";
            bool suc = addPayment(context, ref err);

            //返回json
            result = suc == true ? "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}" : "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            return result;
        }
        //保存逻辑处理
        private bool addPayment(HttpContext context, ref string err)
        {
            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["action"];
            string sk = context.Request.Params["sk"] ?? "";

            ht["payNo"] = context.Request["PAYNO"] == null ? "" : context.Request["PAYNO"].ToString();
            ht["businessType"] = context.Request["BUSINESSTYPE"] == null ? "" : context.Request["BUSINESSTYPE"].ToString();
            ht["accountSimplyName"] = context.Request["ACCOUNTSIMPLYNAME"] == null ? "" : context.Request["ACCOUNTSIMPLYNAME"].ToString();
            ht["accountName"] = context.Request["ACCOUNTNAME"] == null ? "" : context.Request["ACCOUNTNAME"].ToString();
            ht["bankName"] = context.Request["BANKNAME"] == null ? "" : context.Request["BANKNAME"].ToString();
            ht["saleman"] = context.Request["SALEMAN"] == null ? "" : context.Request["SALEMAN"].ToString();
            ht["paydate"] = context.Request["CREATEDATE"] == null ? "" : context.Request["CREATEDATE"].ToString();
            ht["payamount"] = context.Request["PAYAMOUNT"] == null ? "0" : context.Request["PAYAMOUNT"].ToString();
            ht["payAccount"] = context.Request["PAYACCOUNT"] == null ? "0" : context.Request["PAYACCOUNT"].ToString();
            ht["currency"] = context.Request["CURRENCY"] == null ? "" : context.Request["CURRENCY"].ToString();
            ht["contractClient"] = context.Request["BUYER"] == null ? "" : context.Request["BUYER"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["partInfo"] = "是";
            ht["status"] = "0";// context.Request["status"].ToString();

            string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            string payno = ht["payNo"].ToString();
            if ("edit".Equals(context.Request["type"] == null ? "" : context.Request["type"].ToString()))
            {
                ht["lastmod"] = "管理员";// RequestSession.GetSessionUser().UserName;
                ht["lastmoddate"] = time;
            }
            else
            {
                ht["createman"] = "管理员";//RequestSession.GetSessionUser().UserName;
                ht["createdate"] = time;
                ht["lastmod"] = "管理员";//RequestSession.GetSessionUser().UserName;
                ht["lastmoddate"] = time;
            }

            string str = context.Request["ttdatagrid"].ToString().ToLower();
            List<Hashtable> list = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //更新合同列表，把本次付款金额更新为合同中的已付金额,未付金额更新

            #region 更新合同表
            foreach (var item in list)
            {
                //校验金额，如果本次付款金额大于未付金额，返回false 
                if (Convert.ToDecimal(item["creditamount"]) > Convert.ToDecimal(item["unpaidamount"]))
                {
                    err = "合同" + item["contractno"] + "的付款金额大于未付款金额";
                    return false;
                }
                string contractNo = item["contractno"].ToString();
                //到款管理中本次付款金额
                decimal payingAmount = Convert.ToDecimal(item["creditamount"]);
                //到款管理中已付金额
                decimal paidAmount = Convert.ToDecimal(item["paidamount"]);
                //已付金额等于本次付款金额加上合同中已付金额
                decimal contractAmount = Convert.ToDecimal(item["contractamount"]);
                decimal unpaidAmount = contractAmount - payingAmount - paidAmount;
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    SqlParameter[] pms = new SqlParameter[]{
                        new  SqlParameter("@paidAmount",payingAmount+paidAmount),
                        new  SqlParameter("@contractNo",contractNo),
                        new SqlParameter("@unpaidAmount",unpaidAmount)
                      };
                    bll.ExecuteNonQuery(@"update Econtract set paidAmount=@paidAmount,unpaidAmount=@unpaidAmount where contractNo=@contractNo", pms);
                }
            }
            #endregion
            StringBuilder[] sqls = new StringBuilder[list.Count];
            object[] objs = new object[list.Count];

            bool IsOk = false;
            if (string.IsNullOrWhiteSpace(sk))
            {
                //信用证开立
                IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TALBE_PAYCREDIT, "payNo", ht["payNo"].ToString(), ht);
            }
            else
            {   //信用证收款
                IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_PAY_Receive, "payNo", ht["payNo"].ToString(), ht);
            }

            if (IsOk)
            {
                //循环添加到款详情
                Hashtable htDetails = new Hashtable();
                bool flag = false;
                foreach (var item in list)
                {
                    htDetails["payNo"] = ht["payNo"];
                    htDetails["contractNo"] = item["contractno"].ToString();
                    htDetails["payer"] = ht["contractClient"].ToString();
                    htDetails["contractAmount"] = item["contractamount"].ToString();
                    htDetails["item1Amount"] = item["item1amount"].ToString();
                    htDetails["item2Amount"] = item["item2amount"].ToString();
                    htDetails["paidAmount"] = item["paidamount"].ToString();
                    htDetails["unpaidAmount"] = item["unpaidamount"].ToString();
                    htDetails["payingAmount"] = !string.IsNullOrWhiteSpace(sk) ? item["payingamount"].ToString() : item["creditamount"].ToString();
                    if (string.IsNullOrWhiteSpace(sk))
                    {
                        //信用证开立
                        flag = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TALBE_PAYCREDITDETAILS, "payNo", htDetails["payNo"].ToString(), "contractNo", htDetails["contractNo"].ToString(), htDetails);
                    }
                    else
                    {
                        //信用证收款
                        flag = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_PAY_ReceiveDetails, "payNo", htDetails["payNo"].ToString(), "contractNo", htDetails["contractNo"].ToString(), htDetails);
                    }
                    
                }

                if (flag)
                {
                    return true;
                }
                else
                {
                    //删除主表
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_PAY_DOMESTIC, "payNo", ht["payNo"].ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 信用证收款
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string addCredit(HttpContext context){
            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["action"];

            //收款信息
            ht["payNo"] = context.Request["PAYNO"] == null ? "" : context.Request["PAYNO"].ToString();
            ht["businessType"] = context.Request["BUSINESSTYPE"] == null ? "" : context.Request["BUSINESSTYPE"].ToString();
            ht["accountSimplyName"] = context.Request["ACCOUNTSIMPLYNAME"] == null ? "" : context.Request["ACCOUNTSIMPLYNAME"].ToString();
            ht["accountName"] = context.Request["ACCOUNTNAME"] == null ? "" : context.Request["ACCOUNTNAME"].ToString();
            ht["bankName"] = context.Request["BANKNAME"] == null ? "" : context.Request["BANKNAME"].ToString();
            ht["saleman"] = context.Request["SALEMAN"] == null ? "" : context.Request["SALEMAN"].ToString();
            ht["paydate"] = context.Request["CREATEDATE"] == null ? "" : context.Request["CREATEDATE"].ToString();
            ht["payamount"] = context.Request["PAYAMOUNT"] == null ? "0" : context.Request["PAYAMOUNT"].ToString();
            ht["payAccount"] = context.Request["PAYACCOUNT"] == null ? "0" : context.Request["PAYACCOUNT"].ToString();
            ht["currency"] = context.Request["CURRENCY"] == null ? "" : context.Request["CURRENCY"].ToString();
            ht["contractClient"] = context.Request["BUYER"] == null ? "" : context.Request["BUYER"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["partInfo"] = "是";
            ht["status"] = "0";// context.Request["status"].ToString();

            string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            string payno = ht["payNo"].ToString();
            if ("edit".Equals(context.Request["type"] == null ? "" : context.Request["type"].ToString()))
            {
                ht["lastmod"] = RequestSession.GetSessionUser().UserName;
                ht["lastmoddate"] = time;
            }
            else
            {
                ht["createman"] = RequestSession.GetSessionUser().UserName;
                ht["createdate"] = time;
                ht["lastmod"] = RequestSession.GetSessionUser().UserName;
                ht["lastmoddate"] = time;
            }

            //收款信息            
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_PAY_Receive, "payNo", ht["payNo"].ToString(), ht);
            //更新信用证收款信息已状态
            List<StringBuilder> sqls = new List<StringBuilder>();
            sqls.Add(new StringBuilder("update PayCredit set revMoney=" + ht["payamount"] + " ,revDate=GETDATE() where payAccount='" + ht["payAccount"] + "'"));
            string status = context.Request["finish"] ?? "";
            if (status == "yes") {
                sqls.Add(new StringBuilder("update PayCredit set payStatus=1 where payAccount='" + ht["payAccount"] + "'"));
            }

            if (IsOk) {
                int n=DataFactory.SqlDataBase().BatchExecuteBySql(sqls);
                return n>0 ? "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}" : "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"错误\"}";
            }
            return "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"错误\"}";
        }
        //保存信用证
        private bool addPaymentCredit(HttpContext context, ref string err)
        {

            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["action"];
            string sk = context.Request.Params["sk"] ?? "";

            ht["payNo"] = context.Request["PAYNO"] == null ? "" : context.Request["PAYNO"].ToString();
            ht["businessType"] = context.Request["BUSINESSTYPE"] == null ? "" : context.Request["BUSINESSTYPE"].ToString();
            ht["accountSimplyName"] = context.Request["ACCOUNTSIMPLYNAME"] == null ? "" : context.Request["ACCOUNTSIMPLYNAME"].ToString();
            ht["accountName"] = context.Request["ACCOUNTNAME"] == null ? "" : context.Request["ACCOUNTNAME"].ToString();
            ht["bankName"] = context.Request["BANKNAME"] == null ? "" : context.Request["BANKNAME"].ToString();
            ht["saleman"] = context.Request["SALEMAN"] == null ? "" : context.Request["SALEMAN"].ToString();
            ht["paydate"] = context.Request["CREATEDATE"] == null ? "" : context.Request["CREATEDATE"].ToString();
            ht["payamount"] = context.Request["PAYAMOUNT"] == null ? "0" : context.Request["PAYAMOUNT"].ToString();
            ht["payAccount"] = context.Request["PAYACCOUNT"] == null ? "0" : context.Request["PAYACCOUNT"].ToString();
            ht["currency"] = context.Request["CURRENCY"] == null ? "" : context.Request["CURRENCY"].ToString();
            ht["contractClient"] = context.Request["BUYER"] == null ? "" : context.Request["BUYER"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["partInfo"] = "是";
            ht["status"] = "0";// context.Request["status"].ToString();

            string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            string payno = ht["payNo"].ToString();
            if ("edit".Equals(context.Request["type"] == null ? "" : context.Request["type"].ToString()))
            {
                ht["lastmod"] = "管理员";// RequestSession.GetSessionUser().UserName;
                ht["lastmoddate"] = time;
            }
            else
            {
                ht["createman"] = "管理员";//RequestSession.GetSessionUser().UserName;
                ht["createdate"] = time;
                ht["lastmod"] = "管理员";//RequestSession.GetSessionUser().UserName;
                ht["lastmoddate"] = time;
            }

            string str = context.Request["ttdatagrid"].ToString().ToLower();
            List<Hashtable> list = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //更新合同列表，把本次付款金额更新为合同中的已付金额,未付金额更新

            #region 更新合同表
            foreach (var item in list)
            {
                //校验金额，如果本次付款金额大于未付金额，返回false 
                if (Convert.ToDecimal(item["creditamount"]) > Convert.ToDecimal(item["unpaidamount"]))
                {
                    err = "合同" + item["contractno"] + "的付款金额大于未付款金额";
                    return false;
                }
                string contractNo = item["contractno"].ToString();
                //到款管理中本次付款金额
                decimal payingAmount = Convert.ToDecimal(item["creditamount"]);
                //到款管理中已付金额
                decimal paidAmount = Convert.ToDecimal(item["paidamount"]);
                //已付金额等于本次付款金额加上合同中已付金额
                decimal contractAmount = Convert.ToDecimal(item["contractamount"]);
                decimal unpaidAmount = contractAmount - payingAmount - paidAmount;
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    SqlParameter[] pms = new SqlParameter[]{
                        new  SqlParameter("@paidAmount",payingAmount+paidAmount),
                        new  SqlParameter("@contractNo",contractNo),
                        new SqlParameter("@unpaidAmount",unpaidAmount)
                      };
                    bll.ExecuteNonQuery(@"update Econtract set paidAmount=@paidAmount,unpaidAmount=@unpaidAmount where contractNo=@contractNo", pms);
                }
            }
            #endregion
            StringBuilder[] sqls = new StringBuilder[list.Count];
            object[] objs = new object[list.Count];

            bool IsOk = false;//信用证收款
            IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_PAY_Receive, "payNo", ht["payNo"].ToString(), ht);

            if (IsOk)
            {
                //循环添加到款详情
                Hashtable htDetails = new Hashtable();
                bool flag = false;
                foreach (var item in list)
                {
                    htDetails["payNo"] = ht["payNo"];
                    htDetails["contractNo"] = item["contractno"].ToString();
                    htDetails["payer"] = ht["contractClient"].ToString();
                    htDetails["contractAmount"] = item["contractamount"].ToString();
                    htDetails["item1Amount"] = item["item1amount"].ToString();
                    htDetails["item2Amount"] = item["item2amount"].ToString();
                    htDetails["paidAmount"] = item["paidamount"].ToString();
                    htDetails["unpaidAmount"] = item["unpaidamount"].ToString();
                    htDetails["payingAmount"] = !string.IsNullOrWhiteSpace(sk) ? item["payingamount"].ToString() : item["creditamount"].ToString();
                    if (string.IsNullOrWhiteSpace(sk))
                    {
                        //信用证开立
                        flag = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TALBE_PAYCREDITDETAILS, "payNo", htDetails["payNo"].ToString(), "contractNo", htDetails["contractNo"].ToString(), htDetails);
                    }
                    else
                    {
                        //信用证收款
                        flag = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_PAY_ReceiveDetails, "payNo", htDetails["payNo"].ToString(), "contractNo", htDetails["contractNo"].ToString(), htDetails);
                    }

                }

                if (flag)
                {
                    return true;
                }
                else
                {
                    //删除主表
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_PAY_DOMESTIC, "payNo", ht["payNo"].ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        //部分保存-暂时不用
        private string AddPart(HttpContext context) {
            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["action"];
            string sk = context.Request.Params["sk"] ?? "";//确定修改PAYCREDIT表还是PAY_Receive

            ht["payNo"] = context.Request["PAYNO"] == null ? "" : context.Request["PAYNO"].ToString();
            ht["businessType"] = context.Request["BUSINESSTYPE"] == null ? "" : context.Request["BUSINESSTYPE"].ToString();
            ht["accountSimplyName"] = context.Request["ACCOUNTSIMPLYNAME"] == null ? "" : context.Request["ACCOUNTSIMPLYNAME"].ToString();
            ht["accountName"] = context.Request["ACCOUNTNAME"] == null ? "" : context.Request["ACCOUNTNAME"].ToString();
            ht["bankName"] = context.Request["BANKNAME"] == null ? "" : context.Request["BANKNAME"].ToString();
            ht["saleman"] = context.Request["SALEMAN"] == null ? "" : context.Request["SALEMAN"].ToString();
            ht["paydate"] = context.Request["CREATEDATE"] == null ? "" : context.Request["CREATEDATE"].ToString();
            ht["payamount"] = context.Request["PAYAMOUNT"] == null ? "0" : context.Request["PAYAMOUNT"].ToString();
            ht["payAccount"] = context.Request["PAYACCOUNT"] == null ? "0" : context.Request["PAYACCOUNT"].ToString();
            ht["currency"] = context.Request["CURRENCY"] == null ? "" : context.Request["CURRENCY"].ToString();
            ht["contractClient"] = context.Request["BUYER"] == null ? "" : context.Request["BUYER"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["partInfo"] = "未对应合同";
            ht["status"] = 1;
            string time = DateTimeHelper.GetToday("yyyy-MM-dd");
            string payno = ht["payNo"].ToString();
            if ("edit".Equals(context.Request["type"] == null ? "" : context.Request["type"].ToString()))
            {
                ht["lastmod"] = "管理员";
                ht["lastmoddate"] = time;
            }
            else
            {
                ht["createman"] = "管理员";
                ht["createdate"] = time;
                ht["lastmod"] = "管理员";
                ht["lastmoddate"] = time;
            }

            bool IsOk = false;
            if (string.IsNullOrWhiteSpace(sk))
            {
                IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TALBE_PAYCREDIT, "payNo", ht["payNo"].ToString(), ht);
            }
            else {
                IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_PAY_Receive, "payNo", ht["payNo"].ToString(), ht);
            }
            return IsOk == true ? "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}" : "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"error\"}"; 
        }

        //获取合同信息
        private string getContractList(HttpContext context)
        {
            string result = string.Empty;
            string type = context.Request["type"] == null ? "" : context.Request["type"].ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where pricement1 like '%{1}%'", ConstantUtil.TABLE_ECONTRACT, type));
            DataTable contractTable = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
            result = contractTable == null || contractTable.Rows.Count == 0 ? "" : JsonHelper.DataTableToJson(contractTable,"DATA") ;
            return result;
        }
        // 根据收款户名、合同客户、销售员、销售组确定合同范围
        private string GetContractBS(HttpContext context) {
            string result = string.Empty;

            string seller = context.Request["seller"].ToString();
            string buyer = context.Request["buyer"].ToString();
            string businessclass = context.Request["businessclass"].ToString();
            string saleman = context.Request["saleman"].ToString();

            string WHERE = string.Empty;
            if (!string.IsNullOrWhiteSpace(buyer))
            {
                WHERE += string.Format(" and buyer='{0}'", buyer);
            }
            if (!string.IsNullOrWhiteSpace(seller))
            {
                WHERE += string.Format(" and seller='{0}'", seller);
            }
            if (!string.IsNullOrWhiteSpace(businessclass))
            {
                WHERE += string.Format(" and businessclass='{0}'", businessclass);
            }
            if (!string.IsNullOrWhiteSpace(saleman))
            {
                WHERE += string.Format(" and salesmanCode='{0}'", saleman);
            }
            //过滤信用证的合同
            WHERE += @" and (pricement1 like '%信用证%' or pricement2 like '%信用证%')";

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"select pricement1,pricement2,item1Amount,item2Amount, et.contractNo,
	                                    case 
		                                    when charindex('信用证',pricement1)>0 then item1Amount
		                                    when charindex('信用证',pricement2)>0 then item2Amount
		                                    when charindex('信用证',pricement1)>0 and charindex('信用证',pricement2)>0 then item1Amount+item2Amount
	                                    end as Amount,
	                                    contractAmount,paidAmount,unpaidAmount,pt.payingAmount
                                    from Econtract et
                                    join (
	                                    select contractNo,SUM(payingAmount) as payingAmount
	                                    from PayCreditDetails
	                                    group by contractNo
                                    ) pt on pt.contractNo=et.contractNo
                                    where et.pricement1 like '%信用证%' or et.pricement2 like '%信用证%'" + WHERE));
            DataTable contractTable = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
           
            string[] money = context.Request["money"] == null ? null : context.Request["money"].ToString().Split('=');
            if (contractTable != null && contractTable.Rows.Count > 0 && money!=null) {
                contractTable.Columns.Add(money[0],typeof(decimal));
                decimal payamount = 0;
                foreach (DataRow dr in contractTable.Rows)
                {
                    dr[money[0]] = payamount;

                    dr["PAIDAMOUNT"] = dr["PAYINGAMOUNT"];
                    dr["UNPAIDAMOUNT"] = Convert.ToDouble(dr["AMOUNT"]) - Convert.ToDouble(dr["PAYINGAMOUNT"]);
                }
            }
            result = contractTable == null || contractTable.Rows.Count == 0 ? "{\"total\":0,\"rows\":[]}" : "{\"total\":" + contractTable.Rows.Count + "," + JsonHelper.DataTableToJson_(contractTable, "rows") + "}";
            return result;
        }
        //获取采购合同
        private string GetBuyContract(HttpContext context) {
            string result = string.Empty;
            string contractNo = context.Request["contractno"] == null ? "" : context.Request["contractno"].ToString();
            string payAmount = context.Request["payamount"] == null ? "" : context.Request["payamount"].ToString();
            string column = context.Request["collumn"] == null ? "" : context.Request["collumn"].ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where purchaseCode='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
            DataTable contractTable = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
            if (contractTable != null && contractTable.Rows.Count > 0 && payAmount != null)
            {
                contractTable.Columns.Add(column, typeof(string));
                decimal payamount = Convert.ToDecimal(payAmount) / contractTable.Rows.Count;
                foreach (DataRow dr in contractTable.Rows)
                    dr[column] = payamount;
            }
            result = contractTable == null || contractTable.Rows.Count == 0 ? "{\"total\":0,\"rows\":[]}" : "{\"total\":" + contractTable.Rows.Count + "," + JsonHelper.DataTableToJson_(contractTable, "rows") + "}";
            return result;
        }
        #endregion

        #region List
        //信用证开立列表
        public string GetList(HttpContext context) {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;

            string contractClient = (context.Request["contractClient"] ?? "").ToString().Trim();
            string payAccount = (context.Request["payAccount"] ?? "").ToString().Trim();//信用证号
            string beginTime = (context.Request["beginTime"] ?? "").ToString().Trim();
            string endTime = (context.Request["endTime"] ?? "").ToString().Trim();
            
            //收款状态更新
            StringBuilder sql = new StringBuilder("update PayCredit set payStatus=1 where revMoney>=payamount");
            DataFactory.SqlDataBase().ExecuteBySql(sql);
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(@"select ISNULL(pd.cnt,0) as contracts,pc.* from PayCredit pc
                            left join (
	                            select COUNT(*) as cnt,payNo from PayCreditDetails group by payNo
                            ) pd on pd.payNo=pc.payNo
                            where 1=1 ");
            if (contractClient.Length > 0)
            {
                SqlWhere.Append(" and  payno like '%" + contractClient + "%'");
            }
            if (payAccount.Length > 0)
            {
                SqlWhere.Append(" and  payAccount like '%" + payAccount + "%'");
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
            DataTable dt = DataFactory.SqlDataBase().GetPageList(SqlWhere.ToString(), IList_param.ToArray(), "createdate", "Desc", page, row, ref count);
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
        //信用证删除
        private string del(HttpContext context)
        {
            bool r = false;
            string payNo = context.Request.Params["payNo"].ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("delete from {0} where payNo=@payNo;", ConstantUtil.TALBE_PAYCREDIT));
            sb.Append(string.Format("delete from {0} where payNo=@payNo;", ConstantUtil.TALBE_PAYCREDITDETAILS));
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(sb.ToString(),
                        new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@payNo", payNo) });
                    bll.SqlTran.Commit();
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }
            if (r)
            {
                return "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            }
            else
            {
                return "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"error\"}";
            }
        }
        #endregion

        #region 信用证收款
        public string GetCredit(HttpContext context) {
            string no = context.Request.Params["no"].ToString();
            string sql = string.Format("select * from {0} where payNo='{1}'", ConstantUtil.TALBE_PAYCREDIT, no);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            if (dt != null && dt.Rows.Count > 0)
            {
                return JsonHelper.DataRowToJson_(dt.Rows[0]);
            }
            else {
                return "{}";
            }
        }
        public string save(HttpContext context) {
            string err = "";
            bool suc = addCashPayment(context, ref err);

            //返回json
            string r = "";
            if (suc == true)
            {
                r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            }
            else
            {
                r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            }
            return r;
        }
        //添加
        private bool addCashPayment(HttpContext context, ref string err)
        {
            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["action"];

            ht["payNo"] = context.Request["payno"] == null ? "" : context.Request["payno"].ToString();
            ht["businessType"] = "信用证";
            ht["accountSimplyName"] = "";
            ht["accountName"] = "";
            ht["bankName"] = "";
            ht["saleman"] = "";
            ht["paydate"] = context.Request["date"] == null ? "" : context.Request["date"].ToString();
            ht["payamount"] = context.Request["amount"] == null ? "0" : context.Request["amount"].ToString();
            ht["payAccount"] = context.Request["creditno"] == null ? "0" : context.Request["creditno"].ToString();
            ht["currency"] = "";
            ht["payrate"] = "0.0";
            ht["status"] = "1";
            ht["remark"] = "";
            ht["contractClient"] = "";

            string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            string payno = ht["payNo"].ToString();
            ht["createman"] = "管理员";
            ht["createdate"] = time;
            ht["lastmod"] = "管理员";
            ht["lastmoddate"] = time;

            string str = context.Request["datagrid"].ToString().ToLower();
            List<Hashtable> list = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //更新合同列表，把本次付款金额更新为合同中的已付金额,未付金额更新

            #region 更新合同表
            //foreach (var item in list)
            //{
            //    //校验金额，如果本次付款金额大于未付金额，返回false 
            //    if (Convert.ToDecimal(item["payingamount"]) > Convert.ToDecimal(item["unpaidamount"]))
            //    {
            //        err = "合同" + item["contractno"] + "的付款金额大于未付款金额";
            //        return false;
            //    }
            //    string contractNo = item["contractno"].ToString();
            //    //到款管理中本次付款金额
            //    decimal payingAmount = Convert.ToDecimal(item["payingamount"]);
            //    //到款管理中已付金额
            //    decimal paidAmount = Convert.ToDecimal(item["paidamount"]);
            //    //已付金额等于本次付款金额加上合同中已付金额
            //    decimal contractAmount = Convert.ToDecimal(item["contractamount"]);
            //    decimal unpaidAmount = contractAmount - payingAmount - paidAmount;
            //    using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            //    {
            //        SqlParameter[] pms = new SqlParameter[]{
            //            new  SqlParameter("@paidAmount",payingAmount+paidAmount),
            //            new  SqlParameter("@contractNo",contractNo),
            //            new SqlParameter("@unpaidAmount",unpaidAmount)
            //          };
            //        bll.ExecuteNonQuery(@"update Econtract set paidAmount=@paidAmount,unpaidAmount=@unpaidAmount where contractNo=@contractNo", pms);
            //    }
            //}
            #endregion
            StringBuilder[] sqls = new StringBuilder[list.Count];
            object[] objs = new object[list.Count];

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_PAY_Receive, "payNo", ht["payNo"].ToString(), ht);
            if (IsOk)
            {
                //循环添加到款详情
                Hashtable htDetails = new Hashtable();
                bool flag = false;
                foreach (var item in list)
                {
                    htDetails["payNo"] = ht["payNo"];
                    htDetails["contractNo"] = item["contractno"].ToString();
                    htDetails["payer"] = ht["contractClient"].ToString();
                    htDetails["contractAmount"] = item["contractamount"].ToString();
                    htDetails["item1Amount"] = item["item1amount"].ToString();
                    htDetails["item2Amount"] = item["item2amount"].ToString();
                    htDetails["paidAmount"] = item["paidamount"].ToString();
                    htDetails["unpaidAmount"] = item["unpaidamount"].ToString();
                    htDetails["payingAmount"] = ht["payamount"];
                    flag = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_PAY_ReceiveDetails, "payNo", htDetails["payNo"].ToString(), "contractNo", htDetails["contractNo"].ToString(), htDetails);
                }

                if (flag)
                {
                    return true;
                }
                else
                {
                    //删除主表
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_PAY_DOMESTIC, "payNo", ht["payNo"].ToString());
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        public string GetContract(HttpContext context) {
            string no = context.Request.Params["no"].ToString();
            string sql = string.Format(@"select pd.payingAmount,et.*
                                        from Econtract et
                                        join PayCreditDetails pd on pd.contractNo=et.contractNo
                                        join PayCredit pc on pc.payNo=pd.payNo
                                        where pc.payAccount='{0}'",
                no);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb=new StringBuilder();
            if (dt==null||dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                sb.Append("{\"total\":" + dt.Rows.Count + ",");
                sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                sb.Append("}");
            }
            return sb.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetContractOne(HttpContext context) {
            string no = context.Request.Params["no"].ToString();
            string sql = string.Format(@"select et.salesmanCode as SALEMAN,
                                                et.businessclass as BUSINESSCLASS,
                                                pc.contractClient as BUYER,
                                                pc.createdate as XYZDATE,
                                                pc.payamount as XYZAMOUNT,
                                                et.currency as MCURRENCY,
                                                et.overspill as OVERSPILL,
                                                pc.revMoney as REVMONEY,
                                                pc.payamount-pc.revMoney as REMAINMONEY
                                        from Econtract et
                                        join PayCreditDetails on PayCreditDetails.contractNo=et.contractNo
                                        join PayCredit pc on pc.payNo=PayCreditDetails.payNo
                                        where pc.payAccount='{0}'",
                no);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0)
            {
                return "{}";
            }
            else
            { 
                return JsonHelper.DataRowToJson_(dt.Rows[0]);
            }
            
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