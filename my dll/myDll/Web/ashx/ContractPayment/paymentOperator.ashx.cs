using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.ContractPayment
{
    /// <summary>
    /// paymentOperator 的摘要说明
    /// </summary>
    public class paymentOperator : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request.QueryString["module"];
            //删除
            if (module == "del")
            {

                string err = "";
                bool suc = delPayment(ref err, context);

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
                context.Response.Write(r);
            }
            //添加
            if (module == "add")
            {
                string err = "";
                bool suc = addPayment(context,ref err);

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
                context.Response.Write(r);
            }
            //付款账户加载付款方
            if (module == "payAccount")
            {

                string suc = GetPayer(context);

                context.Response.Write(suc);
               
            }
            //收款账户简称加载全称和银行信息
            if (module == "getAccount")
            {

                string suc = getAccount(context);

                context.Response.Write(suc);
            }
            //保存部分现金到款
            if (module == "AddPart")
            {
                context.Response.Write(AddPart(context));
            }
            //认领承兑
            if (module == "ClaimAccept")
            {
                string err = "";
                bool suc = ClaimAccept(context, ref err);

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
                context.Response.Write(r);
            }
        }
        //收款账户简称加载全称和银行信息
        private string getAccount(HttpContext context)
        {
            string accountSimplyName = context.Request.Params["accountSimplyName"];
            StringBuilder sql = new StringBuilder("select accountName,bankName from accountMessage where accountSimplyName like '%" + accountSimplyName + "%' ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
            StringBuilder sb = jsonh.ToEasyUIComboxJson(dt);
            return sb.ToString();
        }
        //根据付款账户获得付款方

        private string GetPayer(HttpContext context)
        {
            string payAccount = context.Request.Params["payAccount"];
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                string sql = "select shortname from bcustomer where icnaccount=@payAccount ";
                SqlParameter[] pms = new SqlParameter[]{
                    new SqlParameter("@payAccount",payAccount)
                };
                string data = bll.ExecuteScalar(sql, pms).ToString() ?? string.Empty;
                return data;
            }
            
        }
        
        /// <summary>
        ///添加或修改
        /// </summary>
        /// <param name="context"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private bool addPayment(HttpContext context,ref string err)
        {
            List<StringBuilder> sqls = new List<StringBuilder>();//命令
            List<object> objs = new List<object>();//参数

            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["action"];
            string part = context.Request.QueryString["part"];
            
          
            ht["payNo"] = context.Request["payNo"] == null ? "" : context.Request["payNo"].ToString();
            ht["businessType"] = context.Request["businessType"] == null ? "" : context.Request["businessType"].ToString();
            ht["accountSimplyName"] = context.Request["accountSimplyName"] == null ? "" : context.Request["accountSimplyName"].ToString();
            ht["accountName"] = context.Request["accountName"] == null ? "" : context.Request["accountName"].ToString();
            ht["bankName"] = context.Request["bankName"] == null ? "" : context.Request["bankName"].ToString();
            ht["saleman"] = context.Request["saleman"] == null ? "" : context.Request["saleman"].ToString();
            ht["paydate"] = context.Request["paydate"] == null ? "" : context.Request["paydate"].ToString();
            ht["payamount"] = context.Request["payamount"] == null ? "0" : context.Request["payamount"].ToString();
            ht["payAccount"] = context.Request["payAccount"] == null ? "0" : context.Request["payAccount"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "" : context.Request["currency"].ToString();
            ht["payrate"] = context.Request["payrate"] == null ? "0" : context.Request["payrate"].ToString();
            ht["status"] = context.Request["status"] == null ? "1" : context.Request["status"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["revBankAccount"] = context.Request["revBankAccount"] == null ? "1" : context.Request["revBankAccount"].ToString();
            ht["revAccountType"] = context.Request["revAccountType"] == null ? "" : context.Request["revAccountType"].ToString();
            ht["s2Status"] = part=="part"?"0":"1";//提交状态
            ht["partInfo"] = "是";
            ht["contractClient"] = context.Request["contractClient"] == null ? "" : context.Request["contractClient"].ToString();
            ht["simpleContractClient"] = context.Request["simpleContractClient"] == null ? "" : context.Request["simpleContractClient"].ToString();
            string time = DateTimeHelper.ShortDateTimeS;
            string payno = ht["payNo"].ToString();
            if ("edit".Equals(context.Request["action"] == null ? "" : context.Request["action"].ToString()))
            {
                ht["lastmod"] = RequestSession.GetSessionUser().UserName ;
                ht["lastmoddate"] = time;
            }
            else
            {
                ht["createman"] = RequestSession.GetSessionUser().UserName;
                ht["createdate"] = time;
                //ht["lastmod"] = RequestSession.GetSessionUser().UserName;
                //ht["lastmoddate"] = time;
            }
        
            string str = context.Request["datagrid"].ToString().ToLower();
            List<Hashtable> list = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
         
            //更新合同列表，把本次付款金额更新为合同中的已付金额,未付金额更新
            List<Hashtable> listTmp = new List<Hashtable>();
            foreach (var item in list)
            {
                //校验金额，如果本次付款金额大于未付金额，返回false 
                if (Convert.ToDecimal(item["payingamount"]) > Convert.ToDecimal(item["unpaidamount"]))
                {
                    err = "合同" + item["contractno"] + "的付款金额大于未付款金额";
                    return false;
                }
                string contractNo = item["contractno"].ToString();
                //到款管理中本次付款金额
                decimal payingAmount = Convert.ToDecimal(item["payingamount"]);
                //到款管理中已付金额
                decimal paidAmount = Convert.ToDecimal(item["paidamount"]);
                //已付金额等于本次付款金额加上合同中已付金额
                decimal contractAmount = Convert.ToDecimal(item["contractamount"]);
                decimal unpaidAmount = contractAmount - payingAmount - paidAmount;

                //更新合同表
                #region
                sqls.Add(new StringBuilder("update Econtract set paidAmount=@paidAmount,unpaidAmount=@unpaidAmount where contractNo=@contractNo"));
                objs.Add(new SqlParam[]{
                    new  SqlParam("@paidAmount",payingAmount+paidAmount),
                    new  SqlParam("@contractNo",contractNo),
                    new SqlParam("@unpaidAmount",unpaidAmount)
                });
                #endregion
                //更新收款详情表
                #region
                Hashtable htDetails = new Hashtable();
                htDetails["payNo"] = ht["payNo"];
                htDetails["contractNo"] = item["contractno"].ToString();
                htDetails["payer"] = ht["contractClient"].ToString();
                htDetails["contractAmount"] = item["contractamount"].ToString();
                htDetails["item1Amount"] = item["item1amount"].ToString();
                htDetails["item2Amount"] = item["item2amount"].ToString();
                htDetails["paidAmount"] = item["paidamount"].ToString();
                htDetails["unpaidAmount"] = item["unpaidamount"].ToString();
                htDetails["payingAmount"] = item["payingamount"].ToString();
                htDetails["pricement1"] = item["pricement1"].ToString();
                htDetails["pricement2"] = item["pricement2"].ToString();
                htDetails["chargingAmount"] = item["chargingamount"].ToString();
                listTmp.Add(htDetails);
                #endregion
                //信保归还
                #region
                StringBuilder zxbSql = new StringBuilder();
                zxbSql.Append(@"select contractNo,amount,zxbAmount1+ zxbAmount2 as zxbAmount,releaseAmount
                                from (
	                                select et.paidAmount as paidAmount,
		                                et.item1Amount+et.item2Amount as amount,
		                                et.contractNo as contractNo,
		                                case et.pricement1
			                                when '信保' then et.item1Amount
			                                else 0
			                                end as zxbAmount1,
		                                case et.pricement2
			                                when '信保' then et.item2Amount
			                                else 0
			                                end as zxbAmount2,
		                                ISNULL(t.releaseAmount,0) as  releaseAmount
	                                from Econtract et
	                                left join (
		                                select contractNo,SUM(payingAmount) releaseAmount
		                                from PayZXBDetails
		                                where payingAmount<0
		                                group by contractNo
	                                ) t on t.contractNo=et.contractNo
                                 where contractNo='" + htDetails["contractNo"].ToString() + "') tab");//合同号，合同总金额，信保额度，已释放信保
                DataTable zxbTb = DataFactory.SqlDataBase().GetDataTableBySQL(zxbSql);

                if (zxbTb != null && zxbTb.Rows.Count > 0)
                {
                    DataRow firstRow = zxbTb.Rows[0];
                    double zxbneg = -(Convert.ToDouble(firstRow["amount"]) - Convert.ToDouble(firstRow["zxbAmount"]) - Convert.ToDouble(firstRow["releaseAmount"]));
                    if (zxbneg < 0)
                    {//信保可释放量小于0，可添加一条付款量为负的金额
                        sqls.Add(new StringBuilder("insert into PayZXBDetails(contractNo,payingAmount) values('" + htDetails["contractNo"].ToString() + "'," + zxbneg + ")"));
                        objs.Add(new SqlParam[0]);
                    }
                }
                #endregion
            } 
            SqlUtil.getBatchSqls(listTmp, ConstantUtil.TABLE_PAY_ReceiveDetails,ref sqls,ref objs);

            //插入收款记录
            listTmp.Clear();
            //listTmp.Add(ht);
            SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_PAY_Receive, "payNo", ht["payNo"].ToString(), ref sqls, ref objs);
            int n = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return n>0?true:false;
        }

        /// <summary>
        ///部分添加或修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddPart(HttpContext context) {
            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["action"];
            string part = context.Request.QueryString["part"];

            ht["payNo"] = context.Request["payNo"] == null ? "" : context.Request["payNo"].ToString();
            ht["businessType"] = context.Request["businessType"] == null ? "" : context.Request["businessType"].ToString();
            ht["accountSimplyName"] = context.Request["accountSimplyName"] == null ? "" : context.Request["accountSimplyName"].ToString();
            ht["accountName"] = context.Request["accountName"] == null ? "" : context.Request["accountName"].ToString();
            ht["bankName"] = context.Request["bankName"] == null ? "" : context.Request["bankName"].ToString();
            ht["saleman"] = context.Request["saleman"] == null ? "" : context.Request["businessType"].ToString();
            ht["paydate"] = context.Request["paydate"] == null ? "" : context.Request["paydate"].ToString();
            ht["payamount"] = context.Request["payamount"] == null ? "0" : context.Request["payamount"].ToString();
            ht["payAccount"] = context.Request["payAccount"] == null ? "0" : context.Request["payAccount"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "" : context.Request["currency"].ToString();
            ht["payrate"] = context.Request["payrate"] == null ? "0" : context.Request["payrate"].ToString();
            ht["status"] = context.Request["status"] == null ? "1" : context.Request["status"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["revBankAccount"] = context.Request["revBankAccount"] == null ? "1" : context.Request["revBankAccount"].ToString();
            ht["revAccountType"] = context.Request["revAccountType"] == null ? "" : context.Request["revAccountType"].ToString();
            ht["contractClient"] = context.Request["contractClient"] == null ? "" : context.Request["contractClient"].ToString();
            ht["s2Status"] = part=="part"?"0":"1";//保存状态
            ht["partInfo"] = "否";
            string time = DateTimeHelper.ShortDateTimeS;
            string payno = ht["payNo"].ToString();
            if ("edit".Equals(context.Request["action"] == null ? "" : context.Request["action"].ToString()))
            {
                //ht["createman"] = context.Request["createman"] == null ? RequestSession.GetSessionUser().UserName : context.Request["createman"].ToString();
                //ht["createdate"] = context.Request["createdate"] == null ? time : context.Request["createdate"].ToString();
                //ht["lastmoddate"] = context.Request["lastmoddate"] == null ? time : context.Request["lastmoddate"].ToString();
                ht["lastmod"] = RequestSession.GetSessionUser().UserName;
                ht["lastmoddate"] = time;
            }
            else
            {
                ht["createman"] = RequestSession.GetSessionUser().UserName;
                ht["createdate"] = time;
                //ht["lastmod"] = RequestSession.GetSessionUser().UserName;
                //ht["lastmoddate"] = time;
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_PAY_Receive, "payNo", ht["payNo"].ToString(), ht);

            return IsOk == true ? "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}" : "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"error\"}";
        }
        //删除
        private bool delPayment(ref string err, HttpContext context)
        {
            var payNo = context.Request.Params["payNo"];
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //string sql = " update payReceive set status=0 where payNo=@payNo";
                string sql = " delete payReceive  where payNo=@payNo";
                SqlParameter[] pms = new SqlParameter[]{
                    new SqlParameter("@payNo",payNo)
                };
                int r = bll.ExecuteNonQuery(sql, pms);
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        ///认领承兑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private bool ClaimAccept(HttpContext context, ref string err)
        {
            List<StringBuilder> sqls = new List<StringBuilder>();//命令
            List<object> objs = new List<object>();//参数

            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["action"];
            string part = context.Request.QueryString["part"];
            string acceptno = context.Request["acceptno"] ?? "";
            string contractClient = context.Request["contractClient"] ?? "";
            string saleman = context.Request["saleman"] ?? "";
            string simpleContractClient = context.Request["simpleContractClient"] ?? "";
            ht["acceptno"] = acceptno;
            ht["saleman"] = saleman;
            ht["contractClient"] = contractClient;
            ht["simpleContractClient"] = simpleContractClient;
            ht["claimstatus"] = "是";
            ht["lastmod"] = RequestSession.GetSessionUser().UserName;
            ht["lastmoddate"] = DateTimeHelper.ShortDateTimeS;
        
            string str = context.Request["datagrid"].ToString().ToLower();
            List<Hashtable> list = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
         
            //更新合同列表，把本次付款金额更新为合同中的已付金额,未付金额更新
            List<Hashtable> listTmp = new List<Hashtable>();
            foreach (var item in list)
            {
                //校验金额，如果本次付款金额大于未付金额，返回false 
                if (Convert.ToDecimal(item["payingamount"]) > Convert.ToDecimal(item["unpaidamount"]))
                {
                    err = "合同" + item["contractno"] + "的付款金额大于未付款金额";
                    return false;
                }
                string contractNo = item["contractno"].ToString();
                //到款管理中本次付款金额
                decimal payingAmount = Convert.ToDecimal(item["payingamount"]);
                //到款管理中已付金额
                decimal paidAmount = Convert.ToDecimal(item["paidamount"]);
                //已付金额等于本次付款金额加上合同中已付金额
                decimal contractAmount = Convert.ToDecimal(item["contractamount"]);
                decimal unpaidAmount = contractAmount - payingAmount - paidAmount;

                //更新合同表
                #region
                sqls.Add(new StringBuilder("update Econtract set paidAmount=@paidAmount,unpaidAmount=@unpaidAmount where contractNo=@contractNo"));
                objs.Add(new SqlParam[]{
                    new  SqlParam("@paidAmount",payingAmount+paidAmount),
                    new  SqlParam("@contractNo",contractNo),
                    new SqlParam("@unpaidAmount",unpaidAmount)
                });
                #endregion
                //更新收款详情表
                #region
                Hashtable htDetails = new Hashtable();
                htDetails["payNo"] = acceptno;
                htDetails["contractNo"] = item["contractno"].ToString();
                htDetails["payer"] = contractClient;
                htDetails["contractAmount"] = item["contractamount"].ToString();
                htDetails["item1Amount"] = item["item1amount"].ToString();
                htDetails["item2Amount"] = item["item2amount"].ToString();
                htDetails["paidAmount"] = item["paidamount"].ToString();
                htDetails["unpaidAmount"] = item["unpaidamount"].ToString();
                htDetails["payingAmount"] = item["payingamount"].ToString();
                htDetails["pricement1"] = item["pricement1"].ToString();
                htDetails["pricement2"] = item["pricement2"].ToString();
                htDetails["chargingAmount"] = item["chargingamount"].ToString();
                listTmp.Add(htDetails);
                #endregion
                //信保归还
                #region
                StringBuilder zxbSql = new StringBuilder();
                zxbSql.Append(@"select contractNo,amount,zxbAmount1+ zxbAmount2 as zxbAmount,releaseAmount
                                from (
	                                select et.paidAmount as paidAmount,
		                                et.item1Amount+et.item2Amount as amount,
		                                et.contractNo as contractNo,
		                                case et.pricement1
			                                when '信保' then et.item1Amount
			                                else 0
			                                end as zxbAmount1,
		                                case et.pricement2
			                                when '信保' then et.item2Amount
			                                else 0
			                                end as zxbAmount2,
		                                ISNULL(t.releaseAmount,0) as  releaseAmount
	                                from Econtract et
	                                left join (
		                                select contractNo,SUM(payingAmount) releaseAmount
		                                from PayZXBDetails
		                                where payingAmount<0
		                                group by contractNo
	                                ) t on t.contractNo=et.contractNo
                                 where contractNo='" + htDetails["contractNo"].ToString() + "') tab");//合同号，合同总金额，信保额度，已释放信保
                DataTable zxbTb = DataFactory.SqlDataBase().GetDataTableBySQL(zxbSql);

                if (zxbTb != null && zxbTb.Rows.Count > 0)
                {
                    DataRow firstRow = zxbTb.Rows[0];
                    double zxbneg = -(Convert.ToDouble(firstRow["amount"]) - Convert.ToDouble(firstRow["zxbAmount"]) - Convert.ToDouble(firstRow["releaseAmount"]));
                    if (zxbneg < 0)
                    {//信保可释放量小于0，可添加一条付款量为负的金额
                        sqls.Add(new StringBuilder("insert into PayZXBDetails(contractNo,payingAmount) values('" + htDetails["contractNo"].ToString() + "'," + zxbneg + ")"));
                        objs.Add(new SqlParam[0]);
                    }
                }
                #endregion
            } 
            SqlUtil.getBatchSqls(listTmp, ConstantUtil.TABLE_PAY_ReceiveDetails,ref sqls,ref objs);

            //插入收款记录
            listTmp.Clear();
            //listTmp.Add(ht);
            SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_PAYACCEPT, "acceptno", ht["acceptno"].ToString(), ref sqls, ref objs);
            int n = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return n>0?true:false;
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