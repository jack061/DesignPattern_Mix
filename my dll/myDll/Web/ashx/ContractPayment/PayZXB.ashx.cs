using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using WZX.Busines.Util;
using RM.Busines;
using System.Data;
using RM.Common.DotNetJson;
using System.Collections;
using System.Data.SqlClient;
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using RM.Busines.DAL;

namespace RM.Web.ashx.ContractPayment
{
    /// <summary>
    /// PayZXB 的摘要说明
    /// </summary>
    public class PayZXB : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                #region FORM
                case "GetContractList"://获取中信保的合同
                    context.Response.Write(GetContractList(context));
                    break;
                case "GetContractQueryList"://查询得到特定的合同
                    context.Response.Write(GetContractQueryList(context));
                    break;
                case "GetZXB"://获取中信保信息
                    context.Response.Write(GetZXB(context));
                    break;
                case "add":
                    context.Response.Write(add(context));
                    break;
                case "GetPayRecieve":
                    context.Response.Write(GetPayRecieve(context));
                    break;
                case "GetSubTable":
                    context.Response.Write(GetSubTable(context));
                    break;
                case "GetContractInfo":
                    context.Response.Write(GetContractInfo(context));
                    break;
                case "GetZXBS"://获取中信保信息
                    context.Response.Write(GetZXBS(context));
                    break;
                #endregion

                #region list
                case "GetList":
                    context.Response.Write(GetList(context));
                    break;
                case "del":
                    context.Response.Write(del(context));
                    break;
                #endregion

                #region 中信保到款
                case "GetContract":
                    context.Response.Write(GetContract(context));
                    break;
                case "save":
                    context.Response.Write(save(context));
                    break;
                case "GetContractNo":
                    context.Response.Write(GetContractNo(context));
                    break;
                #endregion
                default:
                    context.Response.Write("");
                    break;
            }
        }
        #region form
        /// <summary>
        /// 根据客户名获取所有中信保
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetZXBS(HttpContext context) {
            //更新过期项目
            StringBuilder upsql = new StringBuilder();
            upsql.Append("update PayZXBApply set status=3  where GETDATE()>Deadline");
            DataFactory.SqlDataBase().ExecuteBySql(upsql);

            //选择占用和未占用的中信保申请
            string client = context.Request.QueryString["client"];
            string sql = string.Format(@"select ISNULL(pd.usedAmountpd,0) as usedAmount1,pa.applyAmount-ISNULL(pd.usedAmountpd,0) as unusedAmount1,pa.*
                                        from PayZXBApply pa
                                        left join (select pz.payAccount as payAccount,ISNULL(SUM(payingAmount),0) as usedAmountpd 
									        from PayZXBDetails pd
									        left join PayZXB pz on pd.payNo=pz.payNo                                
									        group by pz.payAccount
                                        ) pd 
                                        on  pd.payAccount=pa.applyNo where pa.client like '%{0}%' and (pa.status=0 or pa.status=1)",
                                        client);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            string result = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                result += "[";
                foreach (DataRow row in dt.Rows)
                {
                    result += JsonHelper.DataRowToJson_(row)+",";
                }

                result = result.Substring(0, result.Length - 1) + "]";
            }
            else
            {
                result = "[]";
            }
            return result;
        }
        /// <summary>
        /// 获取合同信息绑定到添加页面上
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetContractInfo(HttpContext context) {
            string payNo = context.Request.QueryString["payNo"];
            string sql = string.Format(@"
                                        select simpleSeller,seller,buyer,salesmanCode as saleman,businessclass,sp.icnbank as bankname
                                        from Econtract et
                                        join bsupplier sp on sp.code=et.sellercode
                                        where et.contractNo='{0}'",
                                        payNo);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            string result = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            }
            else
            {
                result = "{}";
            }
            return result;
        }
        /// <summary>
        /// 根据payNo获取特定的PayRecieve行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPayRecieve(HttpContext context)
        {
            string payNo = context.Request.QueryString["payNo"];
            string sql = string.Format(@"
                                        select PayZXBDetails.payNo,simpleSeller,bankName,seller,buyer,salesmanCode as saleman,businessclass,applyNo,applyDate,PayZXBApply.currency,applyAmount,usedAmount,unusedAmount,xzdays -(DATEDIFF(DAY,applyDate,GETDATE())) as xzdays 
                                        from {0}
                                        join PayZXBApply on PayZXBApply.applyNo={0}.payAccount
                                        join {1} on {1}.payNo={0}.payNo
                                        join Econtract on {1}.contractNo=Econtract.contractNo
                                        where Econtract.contractNo='{2}'", //and Econtract.currency={0}.currency",
                                        ConstantUtil.TABLE_PAYZXB,ConstantUtil.TABLE_PAYZXBDETAILS,payNo);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            string result = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            }
            else
            {
                result = "{}";
            }
            return result;
        }
        /// <summary>
        /// 获取销售合同表数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSubTable(HttpContext context)
        {
            /*
             * 1、获取得到合同的中信保金额
             * 2、根据中信保明细动态得到未付金额              
             */
            string payNo = context.Request.QueryString["payNo"];
            StringBuilder sqlbuilder = new StringBuilder();
            //查询合同信息
            sqlbuilder.Append(string.Format(@"select * 
                                          from Econtract  
                                          where contractNo='{0}'
                            ",payNo));
            sqlbuilder.Append(string.Format(@"select item1Amount as itemAmount
                            from Econtract
                            where contractNo='{0}' and pricement1 like '%信保%'

                            select item2Amount as itemAmount
                            from Econtract
                            where contractNo='{0}' and pricement2 like '%信保%'
                            
                            select * from PayZXBDetails where contractNo='{0}'", payNo));
            DataSet ds = DataFactory.SqlDataBase().GetDataSetBySQL(sqlbuilder);
            DataTable dt = ds.Tables[0];
            DataTable tpayDetails = ds.Tables[3];

            //合同中中信保的总金额
            decimal zxbAmount = new decimal();
            if ((ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0))
            {
                zxbAmount += Convert.ToDecimal(ds.Tables[1].Rows[0]["itemAmount"]);
            }
            if ((ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0))
            {
                zxbAmount += Convert.ToDecimal(ds.Tables[2].Rows[0]["itemAmount"]);
            }
            //已收金额和未收金额
            decimal payAmount = new decimal(0);
            decimal unpayAmount = new decimal(0);
            if (tpayDetails != null && tpayDetails.Rows.Count > 0) {
                foreach (DataRow row in tpayDetails.Rows) {
                    payAmount += Convert.ToDecimal(row["payingAmount"]);
                }
            }
            unpayAmount = zxbAmount - payAmount;
            
            string result = string.Empty;
            if (dt == null || dt.Rows.Count == 0)
            {
                result = "{\"total\":0,\"rows\":[]}";
            }
            else
            {
                //中信保的总额和应收款金额
                dt.Columns.Add("ZXBAMOUNT", typeof(decimal));
                dt.Columns.Add("creditAmount", typeof(decimal));
                dt.Rows[0]["ZXBAMOUNT"] = zxbAmount;
                dt.Rows[0]["paidAmount"] = payAmount;
                dt.Rows[0]["unpaidAmount"] = unpayAmount;
                dt.Rows[0]["creditAmount"] = unpayAmount;

                result += "{\"total\":" + dt.Rows.Count + ",";
                result += JsonHelper.DataTableToJson_(dt, "rows");
                result += "}";
            }
            return result;
        }
        /// <summary>
        /// 获取合同列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetContractList(HttpContext context)
        {
            string result = string.Empty;
            string type = "中信保";
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where pricement1 like '%{1}%'", ConstantUtil.TABLE_ECONTRACT, type));
            DataTable contractTable = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
            result = contractTable == null || contractTable.Rows.Count == 0 ? "{}" : JsonHelper.DataTableToJson(contractTable, "DATA");
            return result;
        }

        /// <summary>
        /// 根据收款户名、合同客户、销售员、销售组确定合同范围
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetContractQueryList(HttpContext context)
        {
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

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where pricement1 like '%中信保%' {1}",
                ConstantUtil.TABLE_ECONTRACT, WHERE));
            DataTable contractTable = DataFactory.SqlDataBase().GetDataTableBySQL(sb);

            string[] money = context.Request["money"] == null ? null : context.Request["money"].ToString().Split('=');
            if (contractTable != null && contractTable.Rows.Count > 0 && money != null)
            {
                contractTable.Columns.Add(money[0], typeof(decimal));
                decimal payamount = Convert.ToDecimal(money[1]) / contractTable.Rows.Count;
                foreach (DataRow dr in contractTable.Rows)
                    dr[money[0]] = payamount;
            }
            result = contractTable == null || contractTable.Rows.Count == 0 ? "{\"total\":0,\"rows\":[]}" : "{\"total\":" + contractTable.Rows.Count + "," + JsonHelper.DataTableToJson_(contractTable, "rows") + "}";
            return result;
        }

        /// <summary>
        /// 查询中信保
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetZXB(HttpContext context) {
            string result = string.Empty;

            string applyNo = context.Request["applyNo"].ToString();
            string sql = string.Format("select * from {0} where applyNo='{1}'", ConstantUtil.TABLE_PAYZXBAPPLY, applyNo);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql)) ;
            if (dt != null && dt.Rows.Count > 0)
            {
                result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            }
            else {
                result = "{}";
            }
            return result;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string add(HttpContext context) {
            string result = string.Empty;
            string err = "";
            bool suc = addPayment(context, ref err);

            //返回json
            result = suc == true ? "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}" : "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            return result;
        }
        private bool addPayment(HttpContext context, ref string err)
        {
            Hashtable ht = new Hashtable();
            string action = context.Request.QueryString["type"];

            List<StringBuilder> sqls = new List<StringBuilder>();//命令
            List<object> objs = new List<object>();

            #region 中信保基本信息
            ht["payNo"] = context.Request["PAYNO"] == null ? "" : context.Request["PAYNO"].ToString();
            ht["businessType"] = context.Request["BUSINESSTYPE"] == null ? "" : context.Request["BUSINESSTYPE"].ToString();
            ht["accountSimplyName"] = context.Request["sellerSimple"] == null ? "" : context.Request["sellerSimple"].ToString();
            ht["accountName"] = context.Request["SELLER"] == null ? "" : context.Request["SELLER"].ToString();
            ht["bankName"] = context.Request["BANKNAME"] == null ? "" : context.Request["BANKNAME"].ToString();
            ht["saleman"] = context.Request["SALEMAN"] == null ? "" : context.Request["SALEMAN"].ToString();
            ht["contractClient"] = context.Request["BUYER"] == null ? "" : context.Request["BUYER"].ToString();
            ht["payAccount"] = context.Request["APPLYNO"] == null ? "0" : context.Request["APPLYNO"].ToString();
            ht["paydate"] = context.Request["APPLYDATE"] == null ? "" : context.Request["APPLYDATE"].ToString();//中信保申请日期
            ht["payamount"] = context.Request["UNUSEDAMOUNT"] == null ? "0" : context.Request["UNUSEDAMOUNT"].ToString();
            ht["currency"] = context.Request["CURRENCY"] == null ? "" : context.Request["CURRENCY"].ToString();
            //ht["unit"] = context.Request["UNIT"] == null ? "" : context.Request["UNIT"].ToString();//授信单位

            string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            string payno = ht["payNo"].ToString();
            if ("add".Equals(context.Request["type"] == null ? "" : context.Request["type"].ToString()))
            {
                ht["createman"] = "admin";
                ht["createdate"] = time;
                ht["lastmod"] = "admin";
                ht["lastmoddate"] = time;
            }
            else
            {
                ht["lastmod"] = "admin";
                ht["lastmoddate"] = time;
            }
            #endregion

            SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_PAYZXB, "payNo", ht["payNo"].ToString(), ref sqls, ref objs);//中信保基本信息

            string str = context.Request["ttdatagrid"].ToString().ToLower();
            List<Hashtable> list = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            decimal ZXBRemaind = Convert.ToDecimal(ht["payamount"].ToString()); ;
            //更新合同列表，把本次付款金额更新为合同中的已付金额,未付金额更新
            #region 更新合同表
            foreach (var item in list)
            {
                //校验金额，如果本次付款金额大于未付金额，返回false 
                if (Convert.ToDecimal(item["creditamount"]) > decimal.Multiply(Convert.ToDecimal(item["unpaidamount"]),(decimal)(1+0.1)))
                {
                    err = "合同" + item["contractno"] + "的付款金额大于可用金额的10%";
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
                    SqlParam[] pms = new SqlParam[]{
                        new  SqlParam("@paidAmount",payingAmount+paidAmount),
                        new  SqlParam("@contractNo",contractNo),
                        new SqlParam("@unpaidAmount",unpaidAmount)
                      };
                    //bll.ExecuteNonQuery(@"update Econtract set paidAmount=@paidAmount,unpaidAmount=@unpaidAmount where contractNo=@contractNo", pms);
                    sqls.Add(new StringBuilder("update Econtract set paidAmount=@paidAmount,unpaidAmount=@unpaidAmount where contractNo=@contractNo"));
                    objs.Add(pms);
                }
                ZXBRemaind -= payingAmount;
            }
            #endregion
            #region 更新中信保申请
            if (ZXBRemaind >= decimal.Multiply(Convert.ToDecimal(ht["payamount"].ToString()),(decimal)(-0.1))) //
            {
                sqls.Add(new StringBuilder("update PayZXBApply set unusedAmount=@ZXBRemaind,usedAmount=(applyAmount-@ZXBRemaind),lastUseDate=@lastUseDate where applyNo=@applyNo"));
                SqlParam[] pms = new SqlParam[]{
                        new  SqlParam("@ZXBRemaind",ZXBRemaind),
                        new SqlParam("@lastUseDate",DateTime.Now),
                        new SqlParam("@applyNo",ht["payAccount"].ToString())
                      };
                objs.Add(pms);
            }
            else {
                //事务回滚
                err = "中信保月超支";
                return false;
            }
            #endregion
            
            //循环添加到款详情
            foreach (var item in list)
            {
                Hashtable htDetails = new Hashtable();
                htDetails["payNo"] = ht["payNo"];
                htDetails["contractNo"] = item["contractno"].ToString();
                htDetails["payer"] = ht["contractClient"].ToString();
                htDetails["contractAmount"] = item["contractamount"].ToString();
                htDetails["item1Amount"] = item["item1amount"].ToString();
                htDetails["item2Amount"] = item["item2amount"].ToString();
                htDetails["paidAmount"] = item["paidamount"].ToString();
                htDetails["unpaidAmount"] = item["unpaidamount"].ToString();
                htDetails["payingAmount"] = item["creditamount"].ToString();
                //flag = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_PAYZXBDETAILS, "payNo", htDetails["payNo"].ToString(), "contractNo", htDetails["contractNo"].ToString(), htDetails);
                SqlUtil.getBatchSqls(htDetails, ConstantUtil.TABLE_PAYZXBDETAILS, "payNo", htDetails["payNo"].ToString(), ref sqls, ref objs);//中信保基本信息
            }
            //更新中信保占用状态和到期时间
            StringBuilder updateZXBsql = new StringBuilder();
            updateZXBsql.Append("update PayZXBApply set status=1,lastUseDate=GETDATE(),Deadline=dateadd(day,xzdays,getdate()) where applyNo='" + ht["payAccount"].ToString() + "'");
            sqls.Add(updateZXBsql);
            objs.Add(new SqlParam[0]);

            bool flag = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs) >= 0 ? true : false;
            if (!flag)
            {
                err= "操作失败!";
            }
            return flag;
        }
        #endregion

        #region list
        private string GetList(HttpContext context) {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string sort = context.Request["sort"] ?? "";
            string order = context.Request["order"] ?? "";
            int count = 0;

            string buyer = (context.Request["buyer"] ?? "").ToString().Trim();
            string seller = (context.Request["seller"] ?? "").ToString().Trim();
            string beginTime = (context.Request["beginTime"] ?? "").ToString().Trim();
            string endTime = (context.Request["endTime"] ?? "").ToString().Trim();

            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            if (buyer.Length > 0)
            {
                SqlWhere.Append(" and  buyer like '%" + buyer + "%'");//买家
            }
            if (seller.Length > 0)
            {
                SqlWhere.Append(" and  seller like '%" + seller + "%'");//卖家
            }
            if (beginTime.Length > 0)
            {
                SqlWhere.Append(" and  signedtime >= '" + beginTime + "'");//签署时间
            }
            if (endTime.Length > 0)
            {
                SqlWhere.Append(" and  signedtime <= '" + endTime + "'");
            }
            SqlWhere.Append(" and status <> '" + ConstantUtil.STATUS_STOCKIN_NEW + "' ")
                .Append(" and status <> '" + ConstantUtil.STATUS_STOCKIN_CHECK7 + "' ");

            sqldata.Append(string.Format(@"select t.contractNo as flag,  
										 CASE   
		                                     WHEN charindex('信保',pricement1)>0 THEN item1Amount   
		                                     WHEN charindex('信保',pricement1)>0 and charindex('信保',pricement2)>0 THEN item1Amount+item2Amount   
		                                     ELSE item2Amount  
	                                     END as zxbAmount,ISNULL(t.payAmount,0) as zxbPayment,
									tm.* 
								from Econtract tm
								left join 
								(   select et.contractNo,pd.payingAmount as payAmount
									from Econtract et
									join PayZXBDetails pd on pd.contractNo=et.contractNo									
								) t on t.contractNo = tm.contractNo 
								where (tm.pricement1 like '%信保%' or tm.pricement2 like '%信保%') {0} ",SqlWhere));


            sqlcount.Append("select COUNT(1) from Econtract tm where (tm.pricement1 like '%信保%' or tm.pricement2 like '%信保%') " + SqlWhere.ToString());
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, null, sort, order, page, row);

            return sb.ToString(); 
        }
        private string del(HttpContext context) {
            string payNo = context.Request.QueryString["payNo"];
            string sql = string.Format("delete {0} where payNo='{1}'", ConstantUtil.TABLE_PAYZXB, payNo);

            int cnt = DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sql));

            return cnt > 0 ? "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}" : "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"error\"}";
        }
        #endregion

        #region 中信保到款
        private string GetContract(HttpContext context) {
            string no = context.Request.Params["no"].ToString();
            string sql = string.Format(@"select *
                                        from Econtract
                                        where contractNo='{0}'",
                no);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0)
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

        public string save(HttpContext context)
        {
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
            ht["businessType"] = "中信保";
            ht["accountSimplyName"] = "";
            ht["accountName"] = "";
            ht["bankName"] = "";
            ht["saleman"] = "";
            ht["paydate"] = context.Request["date"] == null ? "" : context.Request["date"].ToString();
            ht["payamount"] = context.Request["amount"] == null ? "0" : context.Request["amount"].ToString();
            ht["payAccount"] = context.Request["zxbno"] == null ? "0" : context.Request["zxbno"].ToString();
            ht["currency"] = "";
            ht["payrate"] = "0.0";
            ht["status"] = "1";
            ht["remark"] = "";
            ht["contractClient"] = context.Request["contract"] == null ? "0" : context.Request["contract"].ToString();

            string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            string payno = ht["payNo"].ToString();
            ht["createman"] = "管理员";
            ht["createdate"] = time;
            ht["lastmod"] = "管理员";
            ht["lastmoddate"] = time;

            string str = context.Request["datagrid"].ToString().ToLower();
            List<Hashtable> list = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //更新合同列表，把本次付款金额更新为合同中的已付金额,未付金额更新
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
        private string GetContractNo(HttpContext context)
        {
            string no = context.Request.Params["no"].ToString();
            string sql = string.Format(@"select Econtract.* 
                                        from Econtract
                                        join PayZXBDetails on PayZXBDetails.contractNo=Econtract.contractNo
                                        join PayZXB on PayZXB.payNo=PayZXBDetails.payNo
                                        where PayZXB.payNo='{0}'",
                no);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0)
            {
                sb.Append("{[]}");
            }
            else
            {
                sb.Append("[");
                foreach (DataRow row in dt.Rows) {
                    sb.Append(JsonHelper.DataRowToJson_(row)+",");
                }
                sb.Remove(sb.Length-1,1);
                sb.Append("]");
            }
            return sb.ToString();
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