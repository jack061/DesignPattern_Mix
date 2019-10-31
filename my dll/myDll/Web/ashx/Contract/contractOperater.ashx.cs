using RM.Busines;
using RM.Busines.contract;
using RM.Busines.Util;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// contractOperater 的摘要说明
    /// </summary>
    public class contractOperater : IHttpHandler, IRequiresSessionState
    {
        RM.Busines.contract.contractBLL contractBll = new Busines.contract.contractBLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string module = context.Request.QueryString["module"];
            string err = "";
            bool suc = false;
            string template = "";
            string lanugage = "";
            switch (module)
            {
                #region 添加合同
                //添加编辑合同
                case "addContractTest":
                    suc = addContractTest(ref err, context);
                    context.Response.Write(returnData1(suc, err, template, lanugage));
                    break;
                //添加进境关联合同
                case "addImContactContract":
                    suc = addImContactContract(ref err, context);
                    context.Response.Write(returnData1(suc, err, template, lanugage));
                    break;
                //添加外部文本合同
                case "addOutSideContractTest":
                    suc = addOutSideContractTest(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //添加外部文本合同
                case "addInspectContract":
                    suc = addInspectContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //添加服务合同
                case "addServiceContract":
                    suc = addServiceContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //添加管理合同
                case "addManageContract":
                    suc = addManageContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //添加编辑合同附件
                case "addAttachContract":

                    suc = addAttachContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //添加编辑框架合同附件
                case "addFrameAttachContractTest":
                    suc = addFrameAttachContractTest(ref err, context);
                    context.Response.Write(returnData1(suc, err, template, lanugage));
                    break;
                // 添加编辑进境创建发货通知
                case "addImportSendContract":
                    suc = addImportSendContract(ref err, context);
                    context.Response.Write(returnData1(suc, err, template, lanugage));
                    break;
                //添加编辑内部清算单
                case "addInternalContract":
                    suc = addInternalContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;

                //添加编辑物流合同
                case "addLogisticsContract":

                    suc = addLogisticsContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                #endregion

                #region 删除合同
                //删除服务合同
                case "deleteServiceContract":
                    suc = deleteServiceContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //删除合同
                case "deletecontract":

                    suc = deleteContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //删除合同附件
                case "deletecontractfj":

                    suc = deleteContractfj(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //删除物流合同
                case "deleteLogisticsContract":

                    suc = deleteLogisticsContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                #endregion

                #region 其他
                //附件异步请求加载采购合同信息
                case "loadSaleData":

                    suc = checkContract(ref err, context);
                    context.Response.Write(suc);
                    break;
                //校验是否已关联合同
                case "checkContact":
                    suc = checkContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                // 更新审核表
                case "reviewData":

                    suc = reviewData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //第一次添加分批产品表
                case "addReviewFirst":

                    suc = addReviewFirst(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;



                //添加关联合同
                //case "saveCotactContract":

                //    suc = saveCotactContract(ref err, context);
                //    context.Response.Write(returnData(suc, err));
                //    break;
                //添加铁路关联合同
                case "saveCotactTrainContract":

                    suc = saveCotactTrainContract(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //更改发着申请表状态为直接发货
                case "sendimmediate":

                    suc = sendimmediate(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //更新合同表保费运费
                case "updateCost":

                    suc = updateCost(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //上传文件
                case "uploadExportFile":

                    string path = uploadExportFile(context);
                    context.Response.Write(path);
                    break;
                //服务合同外部文本上传文件
                case "uploadServiceFile":

                    string path1 = uploadServiceFile(context);
                    context.Response.Write(path1);
                    break;
                case "saveAcceptData"://保存承兑上传excel

                    suc = saveAcceptData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                case "deleteAcceptFile"://删除承兑上传excel

                    suc = deleteAcceptFile(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                case "updatePrintStatus"://更新合同打印状态
                    suc = updatePrintStatus(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;

                #endregion


            }
        }


        #region 删除服务合同
        private bool deleteServiceContract(ref string err, HttpContext context)
        {
            //判断合同的状态，如果不是新建状态，提示不能删除
            StringBuilder strsql = new StringBuilder(" delete Econtract_Service where contractNo=@contractNo;");
            strsql.Append("  delete Econtract_ServiceItems where contractNo=@contractNo; ");
            strsql.Append("  delete reviewdata where contractNo=@contractNo; ");
            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {

                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql.ToString(), mms);
                    bll.SqlTran.Commit();
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 保存商检合同
        private bool addInspectContract(ref string err, HttpContext context)
        {
            //Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            string contractNo = context.Request.Params["contractNo"];
            string htcplistStr = context.Request.Params["htcplistStr"];
            List<Hashtable> productListTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(htcplistStr);

            #region 主合同信息
            Hashtable ht = new Hashtable();
            ht["contractNo"] = context.Request.Params["contractNo"];
            ht["consignee"] = context.Request.Params["consignee"] ?? string.Empty;
            ht["sendFactory"] = context.Request.Params["sendFactory"] ?? string.Empty;
            ht["language"] = context.Request.Params["language"] ?? string.Empty;
            ht["sendMan"] = context.Request.Params["sendMan"] ?? string.Empty;
            ht["buyer"] = context.Request.Params["buyer"] ?? string.Empty;
            ht["simpleBuyer"] = context.Request.Params["simpleBuyer"] ?? string.Empty;
            ht["buyeraddress"] = context.Request.Params["buyeraddress"] ?? string.Empty;
            ht["buyercode"] = context.Request.Params["buyercode"] ?? string.Empty;
            ht["seller"] = context.Request.Params["seller"] ?? string.Empty;
            ht["simpleSeller"] = context.Request.Params["simpleSeller"] ?? string.Empty;
            ht["selleraddress"] = context.Request.Params["selleraddress"] ?? string.Empty;
            ht["sellercode"] = context.Request.Params["sellercode"] ?? string.Empty;
            ht["signedtime"] = context.Request.Params["signedtime"] ?? string.Empty;
            ht["signedplace"] = context.Request.Params["signedplace"] ?? string.Empty;
            ht["currency"] = context.Request.Params["currency"] ?? string.Empty;
            ht["pricement1"] = context.Request.Params["pricement1"] ?? string.Empty;
            ht["pricement1per"] = context.Request.Params["pricement1per"] ?? string.Empty;
            ht["pricement2"] = context.Request.Params["pricement2"] ?? string.Empty;
            ht["pricement2per"] = context.Request.Params["pricement2per"] ?? string.Empty;
            ht["pvalidity"] = context.Request.Params["pvalidity"] ?? string.Empty;
            ht["shipment"] = context.Request.Params["shipment"] ?? string.Empty;
            ht["transport"] = context.Request.Params["transport"] ?? string.Empty;
            ht["tradement"] = context.Request.Params["tradement"] ?? string.Empty;
            ht["tradeShow"] = context.Request.Params["tradeShow"] ?? string.Empty;
            ht["harborout"] = context.Request.Params["harborout"] ?? string.Empty;
            ht["harborarrive"] = context.Request.Params["harborarrive"] ?? string.Empty;
            ht["harboroutCountry"] = context.Request.Params["harboroutCountry"] ?? string.Empty;
            ht["harboroutarriveCountry"] = context.Request.Params["harboroutarriveCountry"] ?? string.Empty;
            ht["deliveryPlace"] = context.Request.Params["deliveryPlace"] ?? string.Empty;
            ht["harborclear"] = context.Request.Params["harborclear"] ?? string.Empty;
            ht["placement"] = context.Request.Params["placement"] ?? string.Empty;
            ht["validity"] = context.Request.Params["validity"] ?? string.Empty;
            ht["supplemental"] = context.Request.Params["supplemental"] ?? string.Empty;
            ht["remark"] = context.Request.Params["remark"] ?? string.Empty;
            ht["templateno"] = context.Request.Params["templateno"] ?? string.Empty;
            ht["templatename"] = context.Request.Params["templatename"] ?? string.Empty;
            ht["status"] = context.Request.Params["status"] ?? string.Empty;
            ht["shippingmark"] = context.Request.Params["shippingmark"] ?? string.Empty;
            ht["overspill"] = context.Request.Params["overspill"] ?? string.Empty;
            ht["splitShipment"] = context.Request.Params["splitShipment"] ?? string.Empty;
            ht["frameContract"] = context.Request.Params["frameContract"] ?? string.Empty;
            ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
            ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
            ht["productCategory"] = context.Request.Params["productCategory"] ?? string.Empty;
            ht["flowdirection"] = context.Request.Params["flowdirection"] ?? string.Empty;
            ht["previewCode"] = context.Request.Params["previewCode"] ?? string.Empty;
            ht["facPreviewCode"] = context.Request.Params["facPreviewCode"] ?? string.Empty;
            ht["batchRemark"] = context.Request.Params["batchRemark"] ?? string.Empty;
            #endregion

            #region 重构实现
            //1、主数据
            //2、生成主表sql
            SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT_INSPECT, "contractNo", ht["contractNo"].ToString(), ref sqls, ref objs);

            //保存合同产品,先删除后添加
            sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_ECONTRACT_INSPECT_AP + " where contractNo=@contractNo"));
            objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
            List<Hashtable> list_pro = new List<Hashtable>();
            foreach (Hashtable hs in productListTable)
            {
                Hashtable htProduct = new Hashtable();
                htProduct["inspectContractNo"] = contractNo;//商检合同号
                //htProduct["contractNo"] = contractNo;//合同号
                htProduct["contractNo"] = contractNo;
                htProduct["pcode"] = hs["pcode"];
                htProduct["pname"] = hs["pname"];
                htProduct["quantity"] = hs["quantity"];
                htProduct["qunit"] = hs["qunit"];
                htProduct["price"] = hs["price"];
                htProduct["priceUnit"] = hs["priceUnit"];
                htProduct["amount"] = hs["amount"];
                htProduct["packspec"] = hs["packspec"];
                htProduct["packing"] = hs["packing"];
                htProduct["packdes"] = hs["packdes"];
                htProduct["spec"] = hs["spec"];
                htProduct["pallet"] = hs["pallet"];
                htProduct["unit"] = hs["unit"];
                htProduct["ifcheck"] = hs["ifcheck"];
                htProduct["ifplace"] = hs["ifplace"];
                htProduct["hsCode"] = hs["hsCode"];
                htProduct["pnameen"] = hs["pnameen"];
                htProduct["pnameru"] = hs["pnameru"];
                htProduct["packagesNumber"] = hs["packagesNumber"] ?? string.Empty;
                htProduct["priceAdd"] = hs["priceAdd"] ?? string.Empty;
                htProduct["amountfloat"] = hs["amountfloat"] ?? string.Empty;
                list_pro.Add(htProduct);
            }
            SqlUtil.getBatchSqls(list_pro, ConstantUtil.TABLE_ECONTRACT_INSPECT_AP, ref sqls, ref objs);
            //执行sql
            int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return result >= 0 ? true : false;
            #endregion
        }
        #endregion

        #region 各种类型合同的添加编辑


        #region 合同添加编辑
        //添加编辑合同
        private bool addContractTest(ref string errorinfo, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string originalContractNo = string.Empty;
            string templateno = context.Request.Params["templateno"];
            string status = string.Empty;

            #region 判断是否为合同管理员, 合同管理员修改是否报关，是否框架
            string isConManage = context.Request.Params["isConManage"];
            if (isConManage == "True")
            {
                //判断合同创建人是否和登陆人为同一人，同一人说明是合同管理员修改自己的合同。
                string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
                string createman = DataFactory.SqlDataBase().getString(new StringBuilder(@"select createman from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "createman");
                string status2 = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
                if (!userAccount.Equals(createman))
                {
                    string conManage_contractNo = context.Request.Params["conManage_contractNo"] ?? string.Empty;
                    string isCustoms_Manage = context.Request["iscustoms"] ?? string.Empty;
                    string frameContract_Manage = context.Request["frameContract"] ?? string.Empty;
                    Hashtable ht_manage = new Hashtable();
                    ht_manage.Add("iscustoms", isCustoms_Manage);
                    ht_manage.Add("frameContract", frameContract_Manage);
                    int b_manage = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", conManage_contractNo, ht_manage);
                    return b_manage >= 0 ? true : false;
                }
                else//合同管理员修改自己的合同，状态为新建或退回则继续审核，否则只修改是否报关和是否框架
                {
                    if (status2 != ConstantUtil.STATUS_NEW && status2 != ConstantUtil.STATUS_HY_BACK)
                    {
                        string conManage_contractNo = context.Request.Params["conManage_contractNo"] ?? string.Empty;
                        string isCustoms_Manage = context.Request["iscustoms"] ?? string.Empty;
                        string frameContract_Manage = context.Request["frameContract"] ?? string.Empty;
                        Hashtable ht_manage = new Hashtable();
                        ht_manage.Add("iscustoms", isCustoms_Manage);
                        ht_manage.Add("frameContract", frameContract_Manage);
                        int b_manage = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", conManage_contractNo, ht_manage);
                        return b_manage >= 0 ? true : false;
                    }

                }

            }
            #endregion

            //string validity = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            //string showvalidity = getShowPvalidity(validity);//生成显示在列表中的合同有效期
            #region 必填项为空校验与复制合同号选择，确认新建或提交
            var isview = context.Request.QueryString["isview"] ?? "";
            var isattach = context.Request.QueryString["isattach"] ?? "";
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            //判断生成合同号
            //获取合同的状态 
            StringBuilder sb = new StringBuilder(@"select status from Econtract where contractNo=@contractNo");
            string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (context.Request.QueryString["status"] == "0")
            {

                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                    if (contractNo == "自动编号")
                    {
                        contractNo = getSerialNumber();//生成流水编号
                    }
                }

                //contractBll.deleteEcontract(contractNo);

            }
            else if (context.Request.QueryString["status"] == "1")
            {
                #region 框架合同验证
                //框架合同验证
                string frameContract = context.Request.Params["frameContract"];
                if (frameContract == "否")
                {
                    if (listtable.Count == 0)
                    {
                        errorinfo = "非框架合同必须有产品";
                        return false;
                    }
                }
                #endregion

                //提交时校验必填项
                bool isValidate = validateInput(context, ref errorinfo);
                if (!isValidate)
                {
                    return false;
                }
                if (contractStatus != ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    //删除新建合同，生成提交合同
                    contractBll.deleteEcontract(contractNo);
                    //contractNo = generalContractNo(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
                    contractNo = RM.Busines.contract.getContractCode.getContractNumber(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
                }
                else
                {
                    #region 退回时校验买卖方与原合同买卖方是否相同，相同则不变，否则更换买卖方，序号保持不变

                    string orginalBuyerCode = string.Empty;
                    string orginalSellerCode = string.Empty;
                    string buyerCode = context.Request.Params["buyercode"];
                    string sellerCode = context.Request.Params["sellercode"];
                    StringBuilder sbBack = new StringBuilder(string.Format(@"select * from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
                    //获取原合同买卖双方
                    orginalBuyerCode = DataFactory.SqlDataBase().getString(sbBack, "buyercode");
                    orginalSellerCode = DataFactory.SqlDataBase().getString(sbBack, "sellercode");
                    //买卖方不相同生成新的合同号
                    if (buyerCode.Trim() != orginalBuyerCode.Trim() || sellerCode.Trim() != orginalSellerCode.Trim())
                    {
                        originalContractNo = contractNo;
                        ////删除退回合同，生成新合同
                        //contractBll.deleteEcontract(contractNo);
                        contractNo = getNewContractNo(contractNo, buyerCode, sellerCode, "CONTRACTNO");
                    }
                    #endregion
                }

                if (confirmContractMan(ConstantUtil.ROLE_CONTRACTMAN, RequestSession.GetSessionUser().UserAccount.ToString()))
                {
                    //登录人角色为合同管理员，状态改为待业务处主管审核
                    status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK;//提交
                }

            }
            #endregion

            #region 合同付款、未付款金额确认

            //根据产品金额计算出合同总金额，条款1金额，条款2金额，已付金额
            decimal totalAmount = 0;
            decimal item1Amount = 0;
            decimal item2Amount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;
            decimal item1Per= ConvertHelper.ToDecimal<object>(context.Request.Params["pricement1per"], 0);
            decimal item2Per = ConvertHelper.ToDecimal<object>(context.Request.Params["pricement2per"], 0);
            //decimal item1Per = context.Request.Params["pricement1per"] == null ? 0 : Convert.ToDecimal(context.Request.Params["pricement1per"]);
            //string item2PerString = context.Request.Params["pricement2per"];
            //if (!string.IsNullOrEmpty(item2PerString))
            //{
            //    item2Per = Convert.ToDecimal(item2PerString);
            //}
            foreach (var item in listtable)
            {
                var ss = item["amount"];
                var bb = Convert.ToDecimal(item["amount"]);
                totalAmount += ConvertHelper.ToDecimal<string>(item["amount"].ToString(), 0);
            }
            unpaidAmount = totalAmount - paidAmount;
            item1Amount = totalAmount * item1Per / 100;
            item2Amount = totalAmount * item2Per / 100;

            #endregion

            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["purchaseCode"] = context.Request["purchaseCode"] == null ? "" : context.Request["purchaseCode"].ToString();
            ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            ht["language"] = context.Request["language"] == null ? "0" : context.Request["language"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellercode"] = context.Request["sellercode"] == null ? "" : context.Request["sellercode"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "0" : context.Request["simpleSeller"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyercode"] = context.Request["buyercode"] == null ? "" : context.Request["buyercode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["buyeraddress"] = context.Request["buyeraddress"] == null ? "" : context.Request["buyeraddress"].ToString();
            ht["selleraddress"] = context.Request["selleraddress"] == null ? "" : context.Request["selleraddress"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "0" : context.Request["currency"].ToString();
            ht["pricement1"] = context.Request["pricement1"] == null ? "" : context.Request["pricement1"].ToString();
            ht["pricement1per"] = context.Request["pricement1per"] == null ? "0" : context.Request["pricement1per"].ToString();
            ht["pricement2"] = context.Request["pricement2"] == null ? "" : context.Request["pricement2"].ToString();
            ht["pricement2per"] = context.Request["pricement2per"] == null ? "" : context.Request["pricement2per"].ToString();
            ht["pvalidity"] = context.Request["pvalidity"] == null ? "" : context.Request["pvalidity"].ToString();
            ht["shipment"] = context.Request["shipment"] == null ? "" : context.Request["shipment"].ToString();
            ht["paymentType"] = context.Request["paymentType"] == null ? "" : context.Request["paymentType"].ToString();
            ht["shipDate"] = context.Request["shipDate"] == null ? "" : context.Request["shipDate"].ToString();
            ht["transport"] = context.Request["transport"] == null ? "" : context.Request["transport"].ToString();
            ht["tradement"] = context.Request["tradement"] == null ? "0" : context.Request["tradement"].ToString();
            ht["tradeShow"] = context.Request["tradeShow"] == null ? "" : context.Request["tradeShow"].ToString();
            ht["harborout"] = context.Request["harborout"] == null ? "0" : context.Request["harborout"].ToString();
            ht["harborarrive"] = context.Request["harborarrive"] == null ? "" : context.Request["harborarrive"].ToString();
            ht["harboroutCountry"] = context.Request["harboroutCountry"] == null ? "" : context.Request["harboroutCountry"].ToString();
            ht["harboroutarriveCountry"] = context.Request["harboroutarriveCountry"] == null ? "" : context.Request["harboroutarriveCountry"].ToString();
            ht["deliveryPlace"] = context.Request["deliveryPlace"] == null ? "" : context.Request["deliveryPlace"].ToString();
            ht["harborclear"] = context.Request["harborclear"] == null ? "" : context.Request["harborclear"].ToString();
            ht["placement"] = context.Request["placement"] == null ? "" : context.Request["placement"].ToString();
            ht["validity"] = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            ht["supplemental"] = context.Request["supplemental"] == null ? "0" : context.Request["supplemental"].ToString();
            ht["batchRemark"] = context.Request["batchRemark"] == null ? "" : context.Request["batchRemark"].ToString();
            ht["templateno"] = context.Request["templateno"] == null ? "0" : context.Request["templateno"].ToString();
            ht["templatename"] = context.Request["templatename"] == null ? "0" : context.Request["templatename"].ToString();
            ht["status"] = status;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["shippingmark"] = context.Request["shippingmark"] == null ? "" : context.Request["shippingmark"].ToString();
            //ht["showvalidity"] = context.Request["showvalidity"] == null ? "" : context.Request["showvalidity"].ToString();
            ht["overspill"] = context.Request["overspill"] == "" ? 0 : decimal.Parse(context.Request["overspill"]);
            ht["splitShipment"] = context.Request["splitShipment"] == null ? "0" : context.Request["splitShipment"].ToString();
            ht["frameContract"] = context.Request["frameContract"] == null ? "" : context.Request["frameContract"].ToString();
            var createdate = context.Request.Params["createDate"];
            string isEdit = context.Request.Params["isEdit"] ?? string.Empty;
            if (isEdit == "true")//说明为编辑，创建时间不改变
            {
                ht["createdate"] = createdate;
            }
            else
            {
                ht["createdate"] = DateTime.Now.ToString();
            }

            ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
            ht["lastmoddate"] = DateTimeHelper.ShortDateTimeS;
            ht["contractAmount"] = totalAmount;
            ht["item1Amount"] = item1Amount;
            ht["item2Amount"] = item2Amount;
            ht["paidAmount"] = paidAmount;
            ht["unpaidAmount"] = unpaidAmount;
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                ht["contractTag"] = ConstantUtil.CONTRACTTAG_MAINCON;//标识进出境合同
            }
            ht["productCategory"] = context.Request["productCategory"] == null ? "" : context.Request["productCategory"].ToString();
            ht["flowdirection"] = context.Request["flowdirection"] == null ? "" : context.Request["flowdirection"].ToString();
            ht["iscustoms"] = context.Request["iscustoms"] == null ? "" : context.Request["iscustoms"].ToString();
            ht["adminReview"] = context.Request["adminreview"] == null ? "" : context.Request["adminreview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminreviewnumber"] == null ? "" : context.Request["adminreviewnumber"].ToString();
            //获取业务员的直线审核经理名称
            ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());
            ht["freight"] = context.Request["freight"] == null ? "" : context.Request["freight"].ToString();
            ht["commission"] = context.Request["commission"] == null ? "" : context.Request["commission"].ToString();
            string splitstr = context.Request.Params["splitStr"];
            List<Hashtable> splitListTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(splitstr);
            string datagrid = context.Request.Params["datagrid"];//获取模板列表
            List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagrid);
            bool isOK = contractBll.addOrEditContract(ref errorinfo, contractNo, ht, listtable, splitListTable, templateno, status, ht["createman"].ToString(), templateTable, originalContractNo);
            return isOK;
        }

        #endregion

        #region 添加管理合同
        private bool addManageContract(ref string err, HttpContext context)
        {
            //获取服务合同编号
            var contractNo = context.Request.Params["contractNo"].ToString().Trim();
            string status = string.Empty;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            string originalContractNo = string.Empty;
            #region 获取合同状态与合同号
            StringBuilder sb = new StringBuilder(string.Format(@"select status from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_LOGISTICS));
            string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (context.Request.QueryString["status"] == "0")
            {

                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                    if (string.IsNullOrEmpty(contractNo) || contractNo == "自动编号")
                    {
                        contractNo = getSerialNumber();
                    }
                }

            }
            else if (context.Request.QueryString["status"] == "1")
            {
                if (contractStatus != ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    //删除新建合同，生成提交合同
                    contractBll.deleteManageContract(contractNo);
                    //contractNo = generalContractNoByManage(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
                    contractNo = generalContractNoByMannage(context.Request.Params["buyercode"], context.Request.Params["sellercode"]
                           , context.Request.Params["simplePartyC"], context.Request.Params["simplePartyD"]);

                }
                else
                {
                    #region 退回时校验买卖方与原合同买卖方是否相同，相同则不变，否则更换买卖方，序号保持不变
                    string orginalBuyerCode = string.Empty;
                    string orginalSellerCode = string.Empty;
                    string buyerCode = context.Request.Params["buyercode"];
                    string sellerCode = context.Request.Params["sellercode"];
                    StringBuilder sbBack = new StringBuilder(string.Format(@"select * from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT_SERVICEITEMS, contractNo));
                    //获取原合同买卖双方
                    orginalBuyerCode = DataFactory.SqlDataBase().getString(sbBack, "buyercode");
                    orginalSellerCode = DataFactory.SqlDataBase().getString(sbBack, "sellercode");
                    //买卖方不相同生成新的合同号
                    if (buyerCode.Trim() != orginalBuyerCode.Trim() || sellerCode.Trim() != orginalSellerCode.Trim())
                    {
                        //退回时用新合同号更新旧合同号
                        originalContractNo = contractNo;
                        //contractBll.deleteManageContract(contractNo);
                        contractNo = getNewContractNo(contractNo, buyerCode, sellerCode, "GL");
                    }
                    #endregion
                }
                if (confirmAngency(ConstantUtil.ORG_JOBMAN, userId))
                {
                    //用户所属部门为业务处，状态改为待业务处主管审核
                    status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK;//待直线经理审核
                }

            }
            #endregion

            #region 获取主模板信息
            string text = context.Request["htmlcontent"] == null ? "" : context.Request["htmlcontent"].ToString();
            //text = Regex.Replace(text, @"\s", "");
            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["frameContractNo"] = context.Request["frameContractNo"] == null ? "" : context.Request["frameContractNo"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellerCode"] = context.Request["sellerCode"] == null ? "" : context.Request["sellerCode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "" : context.Request["simpleSeller"].ToString();
            ht["simplePartyC"] = context.Request["simplePartyC"] == null ? "" : context.Request["simplePartyC"].ToString();
            ht["partyC"] = context.Request["partyC"] == null ? "" : context.Request["partyC"].ToString();
            ht["simplePartyD"] = context.Request["simplePartyD"] == null ? "" : context.Request["simplePartyD"].ToString();
            ht["partyD"] = context.Request["partyD"] == null ? "" : context.Request["partyD"].ToString();
            ht["partyCCode"] = context.Request["partyCCode"] == null ? "" : context.Request["partyCCode"].ToString();
            ht["partyDCode"] = context.Request["partyDCode"] == null ? "" : context.Request["partyDCode"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyerCode"] = context.Request["buyerCode"] == null ? "" : context.Request["buyerCode"].ToString();
            ht["validity"] = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            ht["contractText"] = text;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            var createdate = context.Request.Params["createDate"];
            string isEdit = context.Request.Params["isEdit"] ?? string.Empty;
            if (isEdit == "true")//说明为编辑，创建时间不改变
            {
                ht["createdate"] = createdate;
            }
            else
            {
                ht["createdate"] = DateTime.Now.ToString();
            }
            ht["adminReview"] = context.Request["adminReview"] == null ? "" : context.Request["adminReview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminReviewNumber"] == null ? "" : context.Request["adminReviewNumber"].ToString();
            ht["ItemName"] = context.Request["ItemName"] == null ? "" : context.Request["ItemName"].ToString();
            ht["ItemAmount"] = context.Request["ItemAmount"] == null ? "" : context.Request["ItemAmount"].ToString();
            //获取业务员的直线审核经理名称
            ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                ht["logisticsTag"] = ConstantUtil.LOGINSTICSTAG;
            }
            ht["status"] = status;
            #endregion

            //生出主合同sql
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            if (string.IsNullOrEmpty(originalContractNo))
            {
                //说明不是退回的合同
                SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT_LOGISTICS, "contractNo", contractNo, ref sqls, ref objs);
            }
            else
            {
                //获取原合同审核数据
                StringBuilder sqlUpdateReview = new StringBuilder(@"update reviewdata set contractNo=@originalContractNo where contractNo=@contractNo");
                SqlParam[] pms = new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@originalContractNo", originalContractNo) };
                sqls.Add(sqlUpdateReview);
                objs.Add(pms);
                //退回的合同，根据旧合同号为主键更新合同
                SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT_LOGISTICS, "contractNo", originalContractNo, ref sqls, ref objs);
            }


            //提交时,合同管理员创建时向审核记录表中插入提交状态
            List<Hashtable> list_review = new List<Hashtable>();
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                Hashtable htReview = new Hashtable();
                htReview["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK;//待直线经理审核
                htReview["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;
                htReview["reviewman"] = RequestSession.GetSessionUser().UserAccount;
                htReview["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htReview["contractNo"] = contractNo;
                list_review.Add(htReview);
                SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
            }
            //批量执行sql
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        }

        #endregion

        #region 添加服务合同
        private bool addServiceContract(ref string err, HttpContext context)
        {
            //获取服务合同编号
            var contractNo = context.Request.Params["contractNo"];
            string status = string.Empty;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            string isFrameAttach = context.Request.Params["isFrameAttach"] ?? string.Empty;//是否为框架子合同
            string frameContractNo = context.Request.Params["frameContractNo"] ?? string.Empty;//框架合同号
            string originalContractNo = string.Empty;
            #region 获取合同状态与合同号
            StringBuilder sb = new StringBuilder(string.Format(@"select status from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_SERVICE));
            string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (context.Request.QueryString["status"] == "0")
            {

                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                    if (string.IsNullOrEmpty(contractNo) || contractNo == "自动编号")
                    {
                        contractNo = getSerialNumber();
                    }
                }

            }
            else if (context.Request.QueryString["status"] == "1")
            {
                if (contractStatus != ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    //删除新建合同，生成提交合同
                    contractBll.deleteServiceContract(contractNo);
                    if (isFrameAttach == "true" || !string.IsNullOrEmpty(frameContractNo))//生成框架子合同号
                    {
                        contractNo = getServiceFrameContractNo(frameContractNo.Trim());
                    }
                    else
                    {
                        contractNo = generalContractNoByService(context.Request.Params["buyercode"], context.Request.Params["sellercode"]
                            , context.Request.Params["simplePartyC"], context.Request.Params["simplePartyD"]);
                    }
                }
                else
                {
                    #region 退回时校验买卖方与原合同买卖方是否相同，相同则不变，否则更换买卖方，序号保持不变
                    string orginalBuyerCode = string.Empty;
                    string orginalSellerCode = string.Empty;
                    string buyerCode = context.Request.Params["buyercode"];
                    string sellerCode = context.Request.Params["sellercode"];
                    StringBuilder sbBack = new StringBuilder(string.Format(@"select * from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT_SERVICE, contractNo));
                    //获取原合同买卖双方
                    orginalBuyerCode = DataFactory.SqlDataBase().getString(sbBack, "buyercode");
                    orginalSellerCode = DataFactory.SqlDataBase().getString(sbBack, "sellercode");
                    //买卖方不相同生成新的合同号
                    if (buyerCode.Trim() != orginalBuyerCode.Trim() || sellerCode.Trim() != orginalSellerCode.Trim())
                    {
                        //删除退回合同，生成新合同
                        //contractBll.deleteServiceContract(contractNo);
                        originalContractNo = contractNo;
                        contractNo = getNewContractNo(contractNo, buyerCode, sellerCode, "WL");
                    }
                    #endregion
                }

                status = ConstantUtil.STATUS_STOCKIN_CHECK2;//待合同管理员审核
                //if (confirmContractMan(ConstantUtil.ROLE_CONTRACTMAN, userAccount) || confirmAngency( ConstantUtil.ORG_JOBMAN,userId))
                // {
                //     //登录人角色为合同管理员或者用户所属部门为业务处，状态改为待业务处主管审核
                //     status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
                // }
                // else
                // {
                //     status = ConstantUtil.STATUS_STOCKIN_CHECK;//提交
                // }
            }
            #endregion

            #region 获取主模板信息
            string text = context.Request["htmlcontent"] == null ? "" : context.Request["htmlcontent"].ToString();
            //text = Regex.Replace(text, @"\s", "");
            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["isFrame"] = context.Request["isFrame"] == null ? "" : context.Request["isFrame"].ToString();
            ht["frameContractNo"] = context.Request["frameContractNo"] == null ? "" : context.Request["frameContractNo"].ToString();
            ht["outsideText"] = context.Request["outsideText"] == null ? "" : context.Request["outsideText"].ToString();
            ht["outsideContractNo"] = context.Request["outsideContractNo"] == null ? "" : context.Request["outsideContractNo"].ToString();
            ht["associateCode"] = context.Request["associateCode"] == null ? "" : context.Request["associateCode"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellerCode"] = context.Request["sellerCode"] == null ? "" : context.Request["sellerCode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "" : context.Request["simpleSeller"].ToString();
            ht["simplePartyC"] = context.Request["simplePartyC"] == null ? "" : context.Request["simplePartyC"].ToString();
            ht["partyC"] = context.Request["partyC"] == null ? "" : context.Request["partyC"].ToString();
            ht["simplePartyD"] = context.Request["simplePartyD"] == null ? "" : context.Request["simplePartyD"].ToString();
            ht["partyD"] = context.Request["partyD"] == null ? "" : context.Request["partyD"].ToString();
            ht["partyCCode"] = context.Request["partyCCode"] == null ? "" : context.Request["partyCCode"].ToString();
            ht["partyDCode"] = context.Request["partyDCode"] == null ? "" : context.Request["partyDCode"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyerCode"] = context.Request["buyerCode"] == null ? "" : context.Request["buyerCode"].ToString();
            ht["validity"] = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            ht["contractText"] = text;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            var createdate = context.Request.Params["createDate"];
            string isEdit = context.Request.Params["isEdit"] ?? string.Empty;
            if (isEdit == "true")//说明为编辑，创建时间不改变
            {
                ht["createdate"] = createdate;
            }
            else
            {
                ht["createdate"] = DateTime.Now.ToString();
            }
            ht["adminReview"] = context.Request["adminReview"] == null ? "" : context.Request["adminReview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminReviewNumber"] == null ? "" : context.Request["adminReviewNumber"].ToString();
            //获取业务员的直线审核经理名称
            //ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK2 || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                if (isFrameAttach == "true" || !string.IsNullOrEmpty(frameContractNo))
                {
                    ht["serviceTag"] = ConstantUtil.CONTRACT_SERVICEATTACH;//服务子合同标识
                }
                else
                {
                    ht["serviceTag"] = ConstantUtil.CONTRACT_SERVICE;
                }

            }
            ht["status"] = status;
            #endregion

            //生出主合同sql
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            if (string.IsNullOrEmpty(originalContractNo))
            {
                //不是退回
                SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT_SERVICE, "contractNo", contractNo, ref sqls, ref objs);
            }
            else
            {
                //获取原合同审核数据
                StringBuilder sqlUpdateReview = new StringBuilder(@"update reviewdata set contractNo=@originalContractNo where contractNo=@contractNo");
                SqlParam[] pms = new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@originalContractNo", originalContractNo) };
                sqls.Add(sqlUpdateReview);
                objs.Add(pms);
                SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT_SERVICE, "contractNo", originalContractNo, ref sqls, ref objs);
            }

            //获取模板动态表格
            string categoryList = context.Request.Params["categoryList"];
            List<Hashtable> categoryListmTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(categoryList);
            //先删除，再添加
            sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_ECONTRACT_SERVICEITEMS + " where contractNo=@contractNo"));
            objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
            List<Hashtable> list_category = new List<Hashtable>();
            foreach (var hs in categoryListmTable)
            {
                Hashtable htCategory = new Hashtable();
                htCategory["costCategory"] = hs["costCategory"];
                htCategory["project"] = hs["project"];
                htCategory["currency"] = hs["currency"];
                htCategory["amount"] = hs["amount"];
                htCategory["priceUnit"] = hs["priceUnit"];
                htCategory["remark"] = hs["remark"];
                htCategory["projectDescribe"] = hs["projectDescribe"];
                htCategory["price"] = hs["price"];
                htCategory["quantity"] = hs["quantity"];
                htCategory["rate"] = hs["rate"];
                htCategory["contractNo"] = contractNo;
                list_category.Add(htCategory);
            }
            SqlUtil.getBatchSqls(list_category, ConstantUtil.TABLE_ECONTRACT_SERVICEITEMS, ref sqls, ref objs);
            //提交时,合同管理员创建时向审核记录表中插入提交状态
            List<Hashtable> list_review = new List<Hashtable>();
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK2 || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                Hashtable htReview = new Hashtable();
                htReview["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK;//待直线经理审核
                htReview["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;
                htReview["reviewman"] = RequestSession.GetSessionUser().UserAccount;
                htReview["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htReview["contractNo"] = contractNo;
                list_review.Add(htReview);
            }
            SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
            //批量执行sql
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r >= 0 ? true : false;

            #region old
            //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            //{
            //    try
            //    {
            //        bll.SqlTran = bll.SqlCon.BeginTransaction();
            //        //保存主表
            //        suc = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT_SERVICE, "contractNo", ht["contractNo"].ToString(), ht);
            //        if (suc)
            //        {
            //            //先删除，再添加
            //            int r = DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_SERVICEITEMS, "contractNo", contractNo);
            //            foreach (var hs in categoryListmTable)
            //            {
            //                Hashtable htCategory = new Hashtable();
            //                htCategory["costCategory"] = hs["costCategory"];
            //                htCategory["project"] = hs["project"];
            //                htCategory["currency"] = hs["currency"];
            //                htCategory["amount"] = hs["amount"];
            //                htCategory["priceUnit"] = hs["priceUnit"];
            //                htCategory["remark"] = hs["remark"];
            //                htCategory["contractNo"] = contractNo;
            //                suc=  DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_ECONTRACT_SERVICEITEMS, htCategory)>0?true:false;
            //            }
            //            //提交时,合同管理员创建时向审核记录表中插入提交状态
            //            if (status == ConstantUtil.STATUS_STOCKIN_CHECK2 || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            //            {
            //                Hashtable htReview = new Hashtable();
            //                htReview["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK8;//业务员
            //                htReview["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;
            //                htReview["reviewman"] = RequestSession.GetSessionUser().UserAccount;
            //                htReview["reviewdate"] = DateTimeHelper.ShortDateTimeS;
            //                htReview["contractNo"] = contractNo;
            //                suc=DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo, "reviewstatus", status, htReview);
            //            }
            //        }
            //        bll.SqlTran.Commit();
            //    }

            //    catch (Exception ex)
            //    {
            //        bll.SqlTran.Rollback();
            //        err = ex.Message;

            //    }
            //    return suc;

            //} 
            #endregion
        }
        #endregion

        #region 外部文本合同添加编辑
        //添加编辑合同
        private bool addOutSideContractTest(ref string errorinfo, HttpContext context)
        {
            string originalContractNo = string.Empty;
            string contractNo = context.Request.Params["contractNo"];
            string templateno = context.Request.Params["templateno"];
            string status = string.Empty;
            //string validity = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            //string showvalidity = getShowPvalidity(validity);//生成显示在列表中的合同有效期
            #region 必填项为空校验与复制合同号选择，确认新建或提交
            var isview = context.Request.QueryString["isview"] ?? "";
            var isattach = context.Request.QueryString["isattach"] ?? "";
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            //判断生成合同号
            //获取合同的状态 
            StringBuilder sb = new StringBuilder(@"select status from Econtract where contractNo=@contractNo");
            string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (context.Request.QueryString["status"] == "0")
            {

                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                }
                if (contractNo == "自动编号")
                {
                    contractNo = getSerialNumber();//生成流水编号
                }
                //contractBll.deleteEcontract(contractNo);

            }
            else if (context.Request.QueryString["status"] == "1")
            {
                if (contractStatus != ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    //删除新建合同，生成提交合同
                    contractBll.deleteEcontract(contractNo);
                    //contractNo = generalContractNo(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
                    contractNo = RM.Busines.contract.getContractCode.getContractNumber(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
                }
                else
                {
                    #region 退回时校验买卖方与原合同买卖方是否相同，相同则不变，否则更换买卖方，序号保持不变
                    string orginalBuyerCode = string.Empty;
                    string orginalSellerCode = string.Empty;
                    string buyerCode = context.Request.Params["buyercode"];
                    string sellerCode = context.Request.Params["sellercode"];
                    StringBuilder sbBack = new StringBuilder(string.Format(@"select * from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
                    //获取原合同买卖双方
                    orginalBuyerCode = DataFactory.SqlDataBase().getString(sbBack, "buyercode");
                    orginalSellerCode = DataFactory.SqlDataBase().getString(sbBack, "sellercode");
                    //买卖方不相同生成新的合同号
                    if (buyerCode.Trim() != orginalBuyerCode.Trim() || sellerCode.Trim() != orginalSellerCode.Trim())
                    {
                        originalContractNo = contractNo;
                        //删除退回合同，生成新合同
                        //contractBll.deleteEcontract(contractNo);
                        contractNo = getNewContractNo(contractNo, buyerCode, sellerCode, "CONTRACTNO");
                    }
                    #endregion
                }

                #region 框架合同验证
                //框架合同验证
                string frameContract = context.Request.Params["frameContract"];
                if (frameContract == "否")
                {
                    if (listtable.Count == 0)
                    {
                        errorinfo = "非框架合同必须有产品";
                        return false;
                    }
                }
                #endregion

                //提交时校验必填项
                bool isValidate = validateInput(context, ref errorinfo);
                if (!isValidate)
                {
                    return false;
                }
                if (confirmContractMan(ConstantUtil.ROLE_CONTRACTMAN, RequestSession.GetSessionUser().UserAccount.ToString()))
                {
                    //登录人角色为合同管理员，状态改为待业务处主管审核
                    status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK;//提交
                }

            }
            #endregion

            #region 合同付款、未付款金额确认

            //根据产品金额计算出合同总金额，条款1金额，条款2金额，已付金额
            decimal totalAmount = 0;
            decimal item1Amount = 0;
            decimal item2Amount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;
            decimal item1Per = ConvertHelper.ToDecimal<object>(context.Request.Params["pricement1per"], 0);
            decimal item2Per = ConvertHelper.ToDecimal<object>(context.Request.Params["pricement2per"], 0);
            foreach (var item in listtable)
            {
                var ss = item["amount"];
                var bb = Convert.ToDecimal(item["amount"]);
                totalAmount += ConvertHelper.ToDecimal<string>(item["amount"].ToString(), 0);
            }
            unpaidAmount = totalAmount - paidAmount;
            item1Amount = totalAmount * item1Per / 100;
            item2Amount = totalAmount * item2Per / 100;

            #endregion

            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["purchaseCode"] = context.Request["purchaseCode"] == null ? "" : context.Request["purchaseCode"].ToString();
            ht["outsideContractNo"] = context.Request["outsideContractNo"] == null ? "" : context.Request["outsideContractNo"].ToString();//外部文本合同号
            ht["outsideText"] = context.Request["outsideText"] == null ? "" : context.Request["outsideText"].ToString();//外部文本附件路径
            ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            ht["language"] = context.Request["language"] == null ? "0" : context.Request["language"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellercode"] = context.Request["sellercode"] == null ? "" : context.Request["sellercode"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "0" : context.Request["simpleSeller"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyercode"] = context.Request["buyercode"] == null ? "" : context.Request["buyercode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["buyeraddress"] = context.Request["buyeraddress"] == null ? "" : context.Request["buyeraddress"].ToString();
            ht["selleraddress"] = context.Request["selleraddress"] == null ? "" : context.Request["selleraddress"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "0" : context.Request["currency"].ToString();
            ht["pricement1"] = context.Request["pricement1"] == null ? "" : context.Request["pricement1"].ToString();
            ht["pricement1per"] = context.Request["pricement1per"] == null ? "0" : context.Request["pricement1per"].ToString();
            ht["pricement2"] = context.Request["pricement2"] == null ? "" : context.Request["pricement2"].ToString();
            ht["pricement2per"] = context.Request["pricement2per"] == null ? "" : context.Request["pricement2per"].ToString();
            ht["pvalidity"] = context.Request["pvalidity"] == null ? "" : context.Request["pvalidity"].ToString();
            ht["shipment"] = context.Request["shipment"] == null ? "" : context.Request["shipment"].ToString();
            ht["paymentType"] = context.Request["paymentType"] == null ? "" : context.Request["paymentType"].ToString();
            ht["shipDate"] = context.Request["shipDate"] == null ? "" : context.Request["shipDate"].ToString();
            ht["transport"] = context.Request["transport"] == null ? "" : context.Request["transport"].ToString();
            ht["tradement"] = context.Request["tradement"] == null ? "0" : context.Request["tradement"].ToString();
            ht["tradeShow"] = context.Request["tradeShow"] == null ? "" : context.Request["tradeShow"].ToString();
            ht["harborout"] = context.Request["harborout"] == null ? "0" : context.Request["harborout"].ToString();
            ht["harborarrive"] = context.Request["harborarrive"] == null ? "" : context.Request["harborarrive"].ToString();
            ht["harboroutCountry"] = context.Request["harboroutCountry"] == null ? "" : context.Request["harboroutCountry"].ToString();
            ht["harboroutarriveCountry"] = context.Request["harboroutarriveCountry"] == null ? "" : context.Request["harboroutarriveCountry"].ToString();
            ht["deliveryPlace"] = context.Request["deliveryPlace"] == null ? "" : context.Request["deliveryPlace"].ToString();
            ht["harborclear"] = context.Request["harborclear"] == null ? "" : context.Request["harborclear"].ToString();
            ht["placement"] = context.Request["placement"] == null ? "" : context.Request["placement"].ToString();
            ht["validity"] = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            ht["supplemental"] = context.Request["supplemental"] == null ? "0" : context.Request["supplemental"].ToString();
            ht["batchRemark"] = context.Request["batchRemark"] == null ? "" : context.Request["batchRemark"].ToString();
            ht["templateno"] = context.Request["templateno"] == null ? "0" : context.Request["templateno"].ToString();
            ht["templatename"] = context.Request["templatename"] == null ? "0" : context.Request["templatename"].ToString();
            ht["outTag"] = ConstantUtil.CONTRACT_OUTSIDESAVE;
            ht["status"] = status;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["shippingmark"] = context.Request["shippingmark"] == null ? "" : context.Request["shippingmark"].ToString();
            ht["overspill"] = context.Request["overspill"] == "" ? 0 : decimal.Parse(context.Request["overspill"]);
            ht["splitShipment"] = context.Request["splitShipment"] == null ? "0" : context.Request["splitShipment"].ToString();
            ht["frameContract"] = context.Request["frameContract"] == null ? "" : context.Request["frameContract"].ToString();
            var createdate = context.Request.Params["createDate"];
            string isEdit = context.Request.Params["isEdit"] ?? string.Empty;
            if (isEdit == "true")//说明为编辑，创建时间不改变
            {
                ht["createdate"] = createdate;
            }
            else
            {
                ht["createdate"] = DateTime.Now.ToString();
            }
            ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
            ht["lastmoddate"] = DateTimeHelper.ShortDateTimeS;
            ht["contractAmount"] = totalAmount;
            ht["item1Amount"] = item1Amount;
            ht["item2Amount"] = item2Amount;
            ht["paidAmount"] = paidAmount;
            ht["unpaidAmount"] = unpaidAmount;
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3 || status == ConstantUtil.STATUS_STOCKIN_CHECK7)
            {
                ht["contractTag"] = ConstantUtil.CONTRACT_OUTSIDE;
            }

            ht["productCategory"] = context.Request["productCategory"] == null ? "" : context.Request["productCategory"].ToString();
            ht["flowdirection"] = context.Request["flowdirection"] == null ? "" : context.Request["flowdirection"].ToString();
            ht["iscustoms"] = context.Request["iscustoms"] == null ? "" : context.Request["iscustoms"].ToString();
            ht["adminReview"] = context.Request["adminreview"] == null ? "" : context.Request["adminreview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminreviewnumber"] == null ? "" : context.Request["adminreviewnumber"].ToString();
            //获取业务员的直线审核经理名称
            ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());
            string splitstr = context.Request.Params["splitStr"];
            List<Hashtable> splitListTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(splitstr);
            string datagrid = context.Request.Params["datagrid"];//获取模板列表
            List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagrid);
            bool isOK = contractBll.addOrEditContract(ref errorinfo, contractNo, ht, listtable, splitListTable, templateno, status, ht["createman"].ToString(), templateTable, originalContractNo);
            return isOK;
        }

        #endregion

        #region 框架合同附件添加编辑
        //添加编辑合同附件
        private bool addFrameAttachContractTest(ref string errorinfo, HttpContext context)
        {
            string originalContractNo = string.Empty;
            string frameContractNo = string.Empty;
            string mainContractNo = context.Request.Params["mainContractNo"];
            string frameCotactContractNo = context.Request.Params["frameCotactContractNo"] ?? string.Empty;
            string contractNo = context.Request.Params["contractNo"];
            string templateno = context.Request.Params["templateno"];
            string status = string.Empty;

            #region 判断是否为合同管理员, 合同管理员修改是否报关，是否框架
            string isConManage = context.Request.Params["isConManage"];
            if (isConManage == "True")
            {
                //判断合同创建人是否和登陆人为同一人，同一人说明是合同管理员修改自己的合同。
                string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
                string createman = DataFactory.SqlDataBase().getString(new StringBuilder(@"select createman from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "createman");
                string status2 = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
                if (!userAccount.Equals(createman))
                {
                    string conManage_contractNo = context.Request.Params["conManage_contractNo"] ?? string.Empty;
                    string isCustoms_Manage = context.Request["iscustoms"] ?? string.Empty;
                    string frameContract_Manage = context.Request["frameContract"] ?? string.Empty;
                    Hashtable ht_manage = new Hashtable();
                    ht_manage.Add("iscustoms", isCustoms_Manage);
                    ht_manage.Add("frameContract", frameContract_Manage);
                    int b_manage = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", conManage_contractNo, ht_manage);
                    return b_manage >= 0 ? true : false;
                }
                else//合同管理员修改自己的合同，状态为新建或退回则继续审核，否则只修改是否报关和是否框架
                {
                    if (status2 != ConstantUtil.STATUS_NEW && status2 != ConstantUtil.STATUS_HY_BACK)
                    {
                        string conManage_contractNo = context.Request.Params["conManage_contractNo"] ?? string.Empty;
                        string isCustoms_Manage = context.Request["iscustoms"] ?? string.Empty;
                        string frameContract_Manage = context.Request["frameContract"] ?? string.Empty;
                        Hashtable ht_manage = new Hashtable();
                        ht_manage.Add("iscustoms", isCustoms_Manage);
                        ht_manage.Add("frameContract", frameContract_Manage);
                        int b_manage = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", conManage_contractNo, ht_manage);
                        return b_manage >= 0 ? true : false;
                    }

                }

            }
            #endregion

            if (string.IsNullOrEmpty(mainContractNo))
            {
                var frameCreNo = context.Request.Params["frameCreNo"] ?? string.Empty;
                frameContractNo = frameCreNo == "" ? context.Request.Params["frameAttachContractNo"] : frameCreNo;//依据的框架合同号
                if (string.IsNullOrEmpty(frameContractNo))
                {
                    frameContractNo = frameCotactContractNo;
                }
            }
            else
            {
                frameContractNo = mainContractNo;
            }



            #region 必填项为空校验与复制合同号选择，确认新建或提交

            var isview = context.Request.QueryString["isview"] ?? "";
            var isattach = context.Request.QueryString["isattach"] ?? "";
            //获取合同的状态 
            StringBuilder sb = new StringBuilder(@"select status from Econtract where contractNo=@contractNo");
            string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (context.Request.QueryString["status"] == "0")
            {

                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                    if (contractNo == "自动编号")
                    {
                        contractNo = getSerialNumber();//生成流水编号
                    }
                }


                //contractBll.deleteEcontract(contractNo);
                //contractNo = getSerialNumber();//生成流水编号
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                if (contractStatus != ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    //删除新建合同，生成提交合同
                    contractBll.deleteEcontract(contractNo);

                    contractNo = generalAttachNo1(frameContractNo);//生成附件编号
                }
                else
                {
                    #region 退回时校验买卖方与原合同买卖方是否相同，相同则不变，否则更换买卖方，序号保持不变
                    string orginalBuyerCode = string.Empty;
                    string orginalSellerCode = string.Empty;
                    string buyerCode = context.Request.Params["buyercode"];
                    string sellerCode = context.Request.Params["sellercode"];
                    StringBuilder sbBack = new StringBuilder(string.Format(@"select * from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
                    //获取原合同买卖双方
                    orginalBuyerCode = DataFactory.SqlDataBase().getString(sbBack, "buyercode");
                    orginalSellerCode = DataFactory.SqlDataBase().getString(sbBack, "sellercode");
                    //买卖方不相同生成新的合同号
                    if (buyerCode.Trim() != orginalBuyerCode.Trim() || sellerCode.Trim() != orginalSellerCode.Trim())
                    {
                        originalContractNo = contractNo;
                        //contractBll.deleteEcontract(contractNo);
                        contractNo = getNewContractNo(contractNo, buyerCode, sellerCode, "CONTRACTNO");
                    }
                    #endregion
                }


                if (confirmContractMan(ConstantUtil.ROLE_CONTRACTMAN, RequestSession.GetSessionUser().UserAccount.ToString()))
                {
                    //登录人角色为合同管理员，状态改为待业务处主管审核
                    status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK;//提交
                }
            }
            #endregion

            #region 获取产品列表以及合同付款、未付款金额确认
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            #region 框架合同验证
            //框架合同验证
            string frameContract = context.Request.Params["frameContract"];
            if (frameContract == "否")
            {
                if (listtable.Count == 0)
                {
                    errorinfo = "非框架合同必须有产品";
                    return false;
                }
            }
            #endregion


            //根据产品金额计算出合同总金额，条款1金额，条款2金额，已付金额
            decimal totalAmount = 0;
            decimal item1Amount = 0;
            decimal item2Amount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;
            decimal item1Per = ConvertHelper.ToDecimal<object>(context.Request.Params["pricement1per"], 0);
            decimal item2Per = ConvertHelper.ToDecimal<object>(context.Request.Params["pricement2per"], 0);
            foreach (var item in listtable)
            {
                var ss = item["amount"];
                var bb = Convert.ToDecimal(item["amount"]);
                totalAmount += ConvertHelper.ToDecimal<string>(item["amount"].ToString(), 0);
            }
            unpaidAmount = totalAmount - paidAmount;
            item1Amount = totalAmount * item1Per / 100;
            item2Amount = totalAmount * item2Per / 100;

            #endregion

            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["frameAttachContractNo"] = frameContractNo;
            ht["purchaseCode"] = context.Request["purchaseCode"] == null ? "" : context.Request["purchaseCode"].ToString();
            ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            ht["language"] = context.Request["language"] == null ? "0" : context.Request["language"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellercode"] = context.Request["sellercode"] == null ? "" : context.Request["sellercode"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "0" : context.Request["simpleSeller"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyercode"] = context.Request["buyercode"] == null ? "" : context.Request["buyercode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["buyeraddress"] = context.Request["buyeraddress"] == null ? "" : context.Request["buyeraddress"].ToString();
            ht["selleraddress"] = context.Request["selleraddress"] == null ? "" : context.Request["selleraddress"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "0" : context.Request["currency"].ToString();
            ht["pricement1"] = context.Request["pricement1"] == null ? "" : context.Request["pricement1"].ToString();
            ht["pricement1per"] = context.Request["pricement1per"] == null ? "0" : context.Request["pricement1per"].ToString();
            ht["pricement2"] = context.Request["pricement2"] == null ? "" : context.Request["pricement2"].ToString();
            ht["pricement2per"] = context.Request["pricement2per"] == null ? "" : context.Request["pricement2per"].ToString();
            ht["pvalidity"] = context.Request["pvalidity"] == null ? "" : context.Request["pvalidity"].ToString();
            ht["shipment"] = context.Request["shipment"] == null ? "" : context.Request["shipment"].ToString();
            ht["paymentType"] = context.Request["paymentType"] == null ? "" : context.Request["paymentType"].ToString();
            ht["shipDate"] = context.Request["shipDate"] == null ? "" : context.Request["shipDate"].ToString();
            ht["transport"] = context.Request["transport"] == null ? "" : context.Request["transport"].ToString();
            ht["tradement"] = context.Request["tradement"] == null ? "0" : context.Request["tradement"].ToString();
            ht["tradeShow"] = context.Request["tradeShow"] == null ? "" : context.Request["tradeShow"].ToString();
            ht["harborout"] = context.Request["harborout"] == null ? "0" : context.Request["harborout"].ToString();
            ht["harborarrive"] = context.Request["harborarrive"] == null ? "" : context.Request["harborarrive"].ToString();
            ht["harboroutCountry"] = context.Request["harboroutCountry"] == null ? "" : context.Request["harboroutCountry"].ToString();
            ht["harboroutarriveCountry"] = context.Request["harboroutarriveCountry"] == null ? "" : context.Request["harboroutarriveCountry"].ToString();
            ht["deliveryPlace"] = context.Request["deliveryPlace"] == null ? "" : context.Request["deliveryPlace"].ToString();
            ht["harborclear"] = context.Request["harborclear"] == null ? "" : context.Request["harborclear"].ToString();
            ht["placement"] = context.Request["placement"] == null ? "" : context.Request["placement"].ToString();
            ht["validity"] = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            ht["supplemental"] = context.Request["supplemental"] == null ? "0" : context.Request["supplemental"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["templateno"] = context.Request["templateno"] == null ? "0" : context.Request["templateno"].ToString();
            ht["templatename"] = context.Request["templatename"] == null ? "0" : context.Request["templatename"].ToString();
            ht["status"] = status;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["shippingmark"] = context.Request["shippingmark"] == null ? "" : context.Request["shippingmark"].ToString();
            ht["overspill"] = context.Request["overspill"] == "" ? 0 : decimal.Parse(context.Request["overspill"]);
            ht["splitShipment"] = context.Request["splitShipment"] == null ? "0" : context.Request["splitShipment"].ToString();

            ht["batchRemark"] = context.Request["batchRemark"] == null ? "" : context.Request["batchRemark"].ToString();
            var createdate = context.Request.Params["createDate"];
            string isEdit = context.Request.Params["isEdit"] ?? string.Empty;
            if (isEdit == "true")//说明为编辑，创建时间不改变
            {
                ht["createdate"] = createdate;
            }
            else
            {
                ht["createdate"] = DateTime.Now.ToString();
            }
            ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
            ht["lastmoddate"] = DateTimeHelper.ShortDateTimeS;
            ht["contractAmount"] = totalAmount;
            ht["item1Amount"] = item1Amount;
            ht["item2Amount"] = item2Amount;
            ht["paidAmount"] = paidAmount;
            ht["unpaidAmount"] = unpaidAmount;
            ht["frameContract"] = "否";
            ht["isCustoms"] = "否";
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                ht["contractTag"] = ConstantUtil.CONTRACTTAG_CONATTACH;//标识框架合同附件
            }

            ht["productCategory"] = context.Request["productCategory"] == null ? "" : context.Request["productCategory"].ToString();
            ht["flowdirection"] = context.Request["flowdirection"] == null ? "" : context.Request["flowdirection"].ToString();

            ht["adminReview"] = context.Request["adminreview"] == null ? "" : context.Request["adminreview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminreviewnumber"] == null ? "" : context.Request["adminreviewnumber"].ToString();
            //获取业务员的直线审核经理名称
            ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());
            string splitstr = context.Request.Params["splitStr"];
            List<Hashtable> splitListTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(splitstr);
            string datagrid = context.Request.Params["datagrid"];//获取模板列表
            List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagrid);
            bool isOK = contractBll.addOrEditContract(ref errorinfo, ht["contractNo"].ToString(), ht, listtable, splitListTable, templateno, status, ht["createman"].ToString(), templateTable, originalContractNo);
            return isOK;
        }
        #endregion

        #region 内部结算单添加编辑
        //添加编辑内部清算单
        private bool addInternalContract(ref string err, HttpContext context)
        {
            string status = string.Empty;
            string originalContractNo = string.Empty;
            #region 获取合同状态与合同号
            var contractNo = context.Request.Params["contractNo"];
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            StringBuilder sb = new StringBuilder(string.Format(@"select status from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_INTERNAL));
            string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (context.Request.QueryString["status"] == "0")
            {

                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                    if (string.IsNullOrEmpty(contractNo) || contractNo == "自动编号")
                    {
                        contractNo = getSerialNumber();
                    }
                }

            }
            else if (context.Request.QueryString["status"] == "1")
            {
                if (contractStatus != ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    //删除新建合同，生成提交合同
                    contractBll.deleteInternalContract(contractNo);
                    contractNo = InternalContractNo(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
                }
                else
                {
                    #region 退回时校验买卖方与原合同买卖方是否相同，相同则不变，否则更换买卖方，序号保持不变
                    string orginalBuyerCode = string.Empty;
                    string orginalSellerCode = string.Empty;
                    string buyerCode = context.Request.Params["buyercode"];
                    string sellerCode = context.Request.Params["sellercode"];
                    StringBuilder sbBack = new StringBuilder(string.Format(@"select * from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT_INTERNAL, contractNo));
                    //获取原合同买卖双方
                    orginalBuyerCode = DataFactory.SqlDataBase().getString(sbBack, "buyercode");
                    orginalSellerCode = DataFactory.SqlDataBase().getString(sbBack, "sellercode");
                    //买卖方不相同生成新的合同号
                    if (buyerCode.Trim() != orginalBuyerCode.Trim() || sellerCode.Trim() != orginalSellerCode.Trim())
                    {
                        originalContractNo = contractNo;
                        contractNo = getNewContractNo(contractNo, buyerCode, sellerCode, "NJ");
                    }
                    #endregion
                }
                if (confirmAngency(ConstantUtil.ORG_JOBMAN, userId))
                {
                    //用户所属部门为业务处，状态改为待业务处主管审核
                    status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK;//待直线经理审核
                }
            }
            #endregion
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            #region 添加合同sql执行块
            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["purchaseCode"] = string.IsNullOrEmpty(context.Request["purchaseCode"]) ? "" : context.Request["purchaseCode"].ToString();
            ht["simpleSeller"] = string.IsNullOrEmpty(context.Request["simpleSeller"]) ? "" : context.Request["simpleSeller"].ToString();
            ht["seller"] = string.IsNullOrEmpty(context.Request["seller"]) ? "" : context.Request["seller"].ToString();
            ht["simpleBuyer"] = string.IsNullOrEmpty(context.Request["simpleBuyer"]) ? "" : context.Request["simpleBuyer"].ToString();
            ht["buyer"] = string.IsNullOrEmpty(context.Request["buyer"]) ? "" : context.Request["buyer"].ToString();
            ht["buyercode"] = string.IsNullOrEmpty(context.Request["buyercode"]) ? "" : context.Request["buyercode"].ToString().Trim();
            ht["sellercode"] = string.IsNullOrEmpty(context.Request["sellercode"]) ? "" : context.Request["sellercode"].ToString().Trim();
            ht["signedtime"] = string.IsNullOrEmpty(context.Request["signedtime"]) ? "" : context.Request["signedtime"].ToString();
            //ht["category"] = string.IsNullOrEmpty(context.Request["category"]) ? "" : context.Request["category"].ToString();
            var createdate = context.Request.Params["createDate"];
            string isEdit = context.Request.Params["isEdit"] ?? string.Empty;
            if (isEdit == "true")//说明为编辑，创建时间不改变
            {
                ht["createdate"] = createdate;
            }
            else
            {
                ht["createdate"] = DateTime.Now.ToString();
            }
            ht["status"] = status;
            ht["signedplace"] = string.IsNullOrEmpty(context.Request["signedplace"]) ? "" : context.Request["signedplace"].ToString();
            ht["contractText"] = string.IsNullOrEmpty(context.Request["htmlContent"]) ? "" : context.Request["htmlContent"].ToString();
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            ht["adminReview"] = context.Request["adminReview"] == null ? "" : context.Request["adminReview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminReviewNumber"] == null ? "" : context.Request["adminReviewNumber"].ToString();
            ht["itemProName"] = context.Request["itemProName"] == null ? "" : context.Request["itemProName"].ToString();
            ht["Organizer"] = context.Request["Organizer"] == null ? "" : context.Request["Organizer"].ToString();
            ht["startDate"] = context.Request["startDate"] == null ? "" : context.Request["startDate"].ToString();
            ht["createTableName"] = context.Request["createTableName"] == null ? "" : context.Request["createTableName"].ToString();
            ht["endDate"] = context.Request["endDate"] == null ? "" : context.Request["endDate"].ToString();
            ht["text1"] = context.Request["hideText1"] == null ? "" : context.Request["hideText1"].ToString();
            ht["text2"] = context.Request["hideText2"] == null ? "" : context.Request["hideText2"].ToString();
            ht["text3"] = context.Request["hideText3"] == null ? "" : context.Request["hideText3"].ToString();
            ht["text4"] = context.Request["hideText4"] == null ? "" : context.Request["hideText4"].ToString();
            //获取业务员的直线审核经理名称
            ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());
            if (status != ConstantUtil.STATUS_STOCKIN_NEW && status != ConstantUtil.STATUS_STOCKIN_CHECK7)
            {
                ht["contractTag"] = ConstantUtil.CONTRACTTAG_INTERNAL;//标识内部结算单
            }
            else
            {
                ht["contractTag"] = ConstantUtil.CONTRACTTAG_INTERNALTEMP;//保存时内部结算单
            }
            if (string.IsNullOrEmpty(originalContractNo))
            {
                //说明不是退回后重新提交的合同，生成主合同sql
                SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT_INTERNAL, "contractNo", contractNo, ref sqls, ref objs);
            }
            else
            {
                //获取原合同审核数据,更新审核表合同号
                StringBuilder sqlUpdateReview = new StringBuilder(@"update reviewdata set contractNo=@originalContractNo where contractNo=@contractNo");
                SqlParam[] pms = new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@originalContractNo", originalContractNo) };
                sqls.Add(sqlUpdateReview);
                objs.Add(pms);
                //为退回后重新提交，根据旧合同号更新主键
                SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT_INTERNAL, "contractNo", originalContractNo, ref sqls, ref objs);
            }

            //保存产品
            #endregion

            #region 产品sql执行块
            foreach (Hashtable hs in listtable)
            {
                var quantity = hs["quantity"].ToString() == "" ? "0" : hs["quantity"].ToString();
                //var interingQuantity = hs["interingQuantity"].ToString() == "" ? "0" : hs["interingQuantity"].ToString();
                //var unInterQuantity = hs["unInterQuantity"].ToString() == "" ? "0" : hs["unInterQuantity"].ToString();
                //var interquantity = hs["interquantity"].ToString() == "" ? "0" : hs["interquantity"].ToString();
                Hashtable ht_prime = new Hashtable();
                ht_prime.Add("contractNo", contractNo);
                ht_prime.Add("pcode", hs["pcode"]);
                Hashtable htProduct = new Hashtable();
                htProduct["purchaseCode"] = ht["purchaseCode"];
                htProduct["contractNo"] = contractNo;
                htProduct["attachmentno"] = "";
                htProduct["pcode"] = hs["pcode"];
                htProduct["pname"] = hs["pname"];
                htProduct["spec"] = hs["spec"];
                htProduct["quantity"] = quantity;
                //htProduct["interingQuantity"] = interingQuantity;
                //htProduct["unInterQuantity"] = unInterQuantity;
                //htProduct["interquantity"] = interquantity;
                htProduct["qunit"] = hs["qunit"];
                htProduct["price"] = hs["price"];
                htProduct["amount"] = hs["amount"];
                htProduct["rate"] = hs["rate"];
                htProduct["SAPNumber"] = hs["SAPNumber"];
                SqlUtil.getBatchSqls(htProduct, ht_prime, ConstantUtil.TABLE_ECONTRACT_INTERNAL_AP, ref sqls, ref objs);
            }
            #endregion

            #region 提交时,合同管理员创建时向审核记录表中插入提交状态

            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                Hashtable ht_submit = new Hashtable();
                Hashtable ht_review_prime = new Hashtable();
                ht_submit["reviewstatus"] = status;
                ht_submit["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;
                ht_submit["reviewman"] = userAccount;
                ht_submit["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht_submit["contractNo"] = contractNo;
                ht_review_prime.Add("contractNo", contractNo);
                ht_review_prime.Add("reviewstatus", status);
                SqlUtil.getBatchSqls(ht_submit, ht_review_prime, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
            }
            #endregion

            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;

        }


        #endregion

        #region 进境关联合同添加编辑
        //添加进境关联合同
        private bool addImContactContract(ref string errorinfo, HttpContext context)
        {
            string originalContractNo = string.Empty;
            string contractNo = context.Request.Params["contractNo"];
            string templateno = context.Request.Params["templateno"];
            string status = string.Empty;
            string frameCotactContractNo = context.Request.QueryString["frameCotactContractNo"];
            string isCopyContact = context.Request.QueryString["isCopyContact"];//复制创建关联合同框架合同附件
            string mainContractNo = context.Request.QueryString["mainContractNo"];//复制创建关联合同框架合同附件时关联合同号
            frameCotactContractNo = context.Request.QueryString["frameCotactContractNo"] == "" ? context.Request.Form["frameCotactContractNo"] : context.Request.QueryString["frameCotactContractNo"];
            string purchaseCode = context.Request.Params["purchaseCode"];

            #region 判断是否为合同管理员, 合同管理员修改是否报关，是否框架
            string isConManage = context.Request.Params["isConManage"];
            if (isConManage == "True")
            {
                //判断合同创建人是否和登陆人为同一人，同一人说明是合同管理员修改自己的合同。
                string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
                string createman = DataFactory.SqlDataBase().getString(new StringBuilder(@"select createman from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "createman");
                string status2 = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
                if (!userAccount.Equals(createman))
                {
                    string conManage_contractNo = context.Request.Params["conManage_contractNo"] ?? string.Empty;
                    string isCustoms_Manage = context.Request["iscustoms"] ?? string.Empty;
                    string frameContract_Manage = context.Request["frameContract"] ?? string.Empty;
                    Hashtable ht_manage = new Hashtable();
                    ht_manage.Add("iscustoms", isCustoms_Manage);
                    ht_manage.Add("frameContract", frameContract_Manage);
                    int b_manage = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", conManage_contractNo, ht_manage);
                    return b_manage >= 0 ? true : false;
                }
                else//合同管理员修改自己的合同，状态为新建或退回则继续审核，否则只修改是否报关和是否框架
                {
                    if (status2 != ConstantUtil.STATUS_NEW && status2 != ConstantUtil.STATUS_HY_BACK)
                    {
                        string conManage_contractNo = context.Request.Params["conManage_contractNo"] ?? string.Empty;
                        string isCustoms_Manage = context.Request["iscustoms"] ?? string.Empty;
                        string frameContract_Manage = context.Request["frameContract"] ?? string.Empty;
                        Hashtable ht_manage = new Hashtable();
                        ht_manage.Add("iscustoms", isCustoms_Manage);
                        ht_manage.Add("frameContract", frameContract_Manage);
                        int b_manage = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", conManage_contractNo, ht_manage);
                        return b_manage >= 0 ? true : false;
                    }

                }

            }
            #endregion

            #region 必填项为空校验与复制合同号选择，确认新建或提交

            var isview = context.Request.QueryString["isview"] ?? "";
            var isattach = context.Request.QueryString["isattach"] ?? "";
            //获取合同的状态 
            StringBuilder sb = new StringBuilder(@"select status from Econtract where contractNo=@contractNo");
            string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (context.Request.QueryString["status"] == "0")
            {

                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                    if (contractNo == "自动编号")
                    {
                        contractNo = getSerialNumber();//生成流水编号
                    }
                }


            }
            else if (context.Request.QueryString["status"] == "1")
            {
                //提交时校验必填项
                bool isValidate = validateInput(context, ref errorinfo);
                if (!isValidate)
                {
                    return false;
                }
                if (contractStatus != ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    //删除新建合同，生成提交合同
                    contractBll.deleteEcontract(contractNo);
                    if (!string.IsNullOrEmpty(frameCotactContractNo) && isCopyContact != "true")
                    {
                        contractNo = generalAttachNo1(frameCotactContractNo);
                    }
                    else
                    {
                        //contractNo = generalContractNo(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
                        contractNo = RM.Busines.contract.getContractCode.getContractNumber(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
                    }
                }
                else
                {
                    #region 退回时校验买卖方与原合同买卖方是否相同，相同则不变，否则更换买卖方，序号保持不变
                    string orginalBuyerCode = string.Empty;
                    string orginalSellerCode = string.Empty;
                    string buyerCode = context.Request.Params["buyercode"];
                    string sellerCode = context.Request.Params["sellercode"];
                    StringBuilder sbBack = new StringBuilder(string.Format(@"select * from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
                    //获取原合同买卖双方
                    orginalBuyerCode = DataFactory.SqlDataBase().getString(sbBack, "buyercode");
                    orginalSellerCode = DataFactory.SqlDataBase().getString(sbBack, "sellercode");
                    //买卖方不相同生成新的合同号
                    if (buyerCode.Trim() != orginalBuyerCode.Trim() || sellerCode.Trim() != orginalSellerCode.Trim())
                    {
                        originalContractNo = contractNo;
                        //contractBll.deleteEcontract(contractNo);
                        contractNo = getNewContractNo(contractNo, buyerCode, sellerCode, "CONTRACTNO");
                    }
                    #endregion
                }

                if (confirmContractMan(ConstantUtil.ROLE_CONTRACTMAN, RequestSession.GetSessionUser().UserAccount.ToString()))
                {
                    //登录人角色为合同管理员，状态改为待业务处主管审核
                    status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK;//提交
                }
            }
            #endregion

            #region 获取产品列表以及合同付款、未付款金额确认
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            #region 框架合同验证
            //框架合同验证
            string frameContract = context.Request.Params["frameContract"];
            if (frameContract == "否")
            {
                if (listtable.Count == 0)
                {
                    errorinfo = "非框架合同必须有产品";
                    return false;
                }
            }
            #endregion


            //根据产品金额计算出合同总金额，条款1金额，条款2金额，已付金额
            decimal totalAmount = 0;
            decimal item1Amount = 0;
            decimal item2Amount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;
            decimal item1Per = ConvertHelper.ToDecimal<object>(context.Request.Params["pricement1per"], 0);
            decimal item2Per = ConvertHelper.ToDecimal<object>(context.Request.Params["pricement2per"], 0);
            foreach (var item in listtable)
            {
                var ss = item["amount"];

                var bb = Convert.ToDecimal(item["amount"]);
                totalAmount += ConvertHelper.ToDecimal<string>(item["amount"].ToString(), 0);
            }
            unpaidAmount = totalAmount - paidAmount;
            item1Amount = totalAmount * item1Per / 100;
            item2Amount = totalAmount * item2Per / 100;

            #endregion

            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            if (isCopyContact == "true")
            {
                ht["purchaseCode"] = context.Request["frameCotactContractNo"] == "" ? context.Request["purchaseCode"].ToString() : context.Request["frameCotactContractNo"];
            }
            else
            {
                ht["purchaseCode"] = context.Request["mainContractNo"] == "" ? context.Request["purchaseCode"].ToString() : context.Request["mainContractNo"];
            }

            ht["frameCotactContractNo"] = context.Request["frameCotactContractNo"] == null ? "" : context.Request["frameCotactContractNo"].ToString();
            ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            ht["language"] = context.Request["language"] == null ? "0" : context.Request["language"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellercode"] = context.Request["sellercode"] == null ? "" : context.Request["sellercode"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "0" : context.Request["simpleSeller"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyercode"] = context.Request["buyercode"] == null ? "" : context.Request["buyercode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["buyeraddress"] = context.Request["buyeraddress"] == null ? "" : context.Request["buyeraddress"].ToString();
            ht["selleraddress"] = context.Request["selleraddress"] == null ? "" : context.Request["selleraddress"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "0" : context.Request["currency"].ToString();
            ht["pricement1"] = context.Request["pricement1"] == null ? "" : context.Request["pricement1"].ToString();
            ht["pricement1per"] = context.Request["pricement1per"] == null ? "0" : context.Request["pricement1per"].ToString();
            ht["pricement2"] = context.Request["pricement2"] == null ? "" : context.Request["pricement2"].ToString();
            ht["pricement2per"] = context.Request["pricement2per"] == null ? "" : context.Request["pricement2per"].ToString();
            ht["pvalidity"] = context.Request["pvalidity"] == null ? "" : context.Request["pvalidity"].ToString();
            ht["shipment"] = context.Request["shipment"] == null ? "" : context.Request["shipment"].ToString();
            ht["paymentType"] = context.Request["paymentType"] == null ? "" : context.Request["paymentType"].ToString();
            ht["shipDate"] = context.Request["shipDate"] == null ? "" : context.Request["shipDate"].ToString();
            ht["transport"] = context.Request["transport"] == null ? "" : context.Request["transport"].ToString();
            ht["tradement"] = context.Request["tradement"] == null ? "0" : context.Request["tradement"].ToString();
            ht["tradeShow"] = context.Request["tradeShow"] == null ? "" : context.Request["tradeShow"].ToString();
            ht["harborout"] = context.Request["harborout"] == null ? "0" : context.Request["harborout"].ToString();
            ht["harborarrive"] = context.Request["harborarrive"] == null ? "" : context.Request["harborarrive"].ToString();
            ht["harboroutCountry"] = context.Request["harboroutCountry"] == null ? "" : context.Request["harboroutCountry"].ToString();
            ht["harboroutarriveCountry"] = context.Request["harboroutarriveCountry"] == null ? "" : context.Request["harboroutarriveCountry"].ToString();
            ht["deliveryPlace"] = context.Request["deliveryPlace"] == null ? "" : context.Request["deliveryPlace"].ToString();
            ht["harborclear"] = context.Request["harborclear"] == null ? "" : context.Request["harborclear"].ToString();
            ht["placement"] = context.Request["placement"] == null ? "" : context.Request["placement"].ToString();
            ht["validity"] = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            ht["supplemental"] = context.Request["supplemental"] == null ? "0" : context.Request["supplemental"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["templateno"] = context.Request["templateno"] == null ? "0" : context.Request["templateno"].ToString();
            ht["templatename"] = context.Request["templatename"] == null ? "0" : context.Request["templatename"].ToString();
            ht["status"] = status;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["shippingmark"] = context.Request["shippingmark"] == null ? "" : context.Request["shippingmark"].ToString();
            ht["overspill"] = context.Request["overspill"] == "" ? 0 : decimal.Parse(context.Request["overspill"]);
            ht["splitShipment"] = context.Request["splitShipment"] == null ? "0" : context.Request["splitShipment"].ToString();
            ht["frameContract"] = context.Request["frameContract"] == null ? "" : context.Request["frameContract"].ToString();
            ht["batchRemark"] = context.Request["batchRemark"] == null ? "" : context.Request["batchRemark"].ToString();
            var createdate = context.Request.Params["createDate"];
            string isEdit = context.Request.Params["isEdit"] ?? string.Empty;
            if (isEdit == "true")//说明为编辑，创建时间不改变
            {
                ht["createdate"] = createdate;
            }
            else
            {
                ht["createdate"] = DateTime.Now.ToString();
            }
            ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
            ht["lastmoddate"] = DateTimeHelper.ShortDateTimeS;
            ht["contractAmount"] = totalAmount;
            ht["item1Amount"] = item1Amount;
            ht["item2Amount"] = item2Amount;
            ht["paidAmount"] = paidAmount;
            ht["unpaidAmount"] = unpaidAmount;
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                //if (!string.IsNullOrEmpty(frameCotactContractNo) && isCopyContact != "true")
                if (!string.IsNullOrEmpty(frameCotactContractNo) && isCopyContact != "true")
                {
                    ht["contractTag"] = ConstantUtil.CONTRACTTAG_CONATTACH;//标识进出境合同为附件
                }
                else
                {
                    ht["contractTag"] = ConstantUtil.CONTRACTTAG_MAINCON;//标识进出境合同
                }

            }
            ht["productCategory"] = context.Request["productCategory"] == null ? "" : context.Request["productCategory"].ToString();
            ht["flowdirection"] = context.Request["flowdirection"] == null ? "" : context.Request["flowdirection"].ToString();
            ht["iscustoms"] = context.Request["iscustoms"] == null ? "" : context.Request["iscustoms"].ToString();
            ht["adminReview"] = context.Request["adminreview"] == null ? "" : context.Request["adminreview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminreviewnumber"] == null ? "" : context.Request["adminreviewnumber"].ToString();
            //获取业务员的直线审核经理名称
            ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());
            string splitstr = context.Request.Params["splitStr"];
            List<Hashtable> splitListTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(splitstr);
            string datagrid = context.Request.Params["datagrid"];//获取模板列表
            List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagrid);
            bool isOK = contractBll.addOrEditContract(ref errorinfo, contractNo, ht, listtable, splitListTable, templateno, status, ht["createman"].ToString(), templateTable, originalContractNo);
            return isOK;
        }
        #endregion

        #endregion

        #region 进境创建发货通知添加编辑

        // 添加编辑进境创建发货通知
        private bool addImportSendContract(ref string errorinfo, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string sendContractNo = context.Request.Params["sendContractNo"];
            string templateno = context.Request.Params["templateno"];
            string status = string.Empty;
            #region 必填项为空校验与复制合同号选择，确认新建或提交
            var isview = context.Request.QueryString["isview"] ?? "";
            var isattach = context.Request.QueryString["isattach"] ?? "";
            //判断生成合同号
            string sbcontractNo = sendContractNo;

            if (context.Request.QueryString["status"] == "0")
            {
                status = ConstantUtil.STATUS_STOCKIN_NEW;//新建
                if (sendContractNo == "自动编号")
                {
                    sbcontractNo = getSerialNumber();//生成流水编号
                }
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                //删除新建合同，生成提交合同
                contractBll.deleteEcontract(sendContractNo);

                sbcontractNo = generalImportSendContractNo(contractNo);
                status = ConstantUtil.STATUS_STOCKIN_CHECK1;//状态更改为审批通过
            }
            #endregion

            #region 获取产品列表以及合同付款、未付款金额确认
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //根据产品金额计算出合同总金额，条款1金额，条款2金额，已付金额
            decimal totalAmount = 0;
            decimal item1Amount = 0;
            decimal item2Amount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;
            decimal item1Per = context.Request.Params["pricement1per"] == null ? 0 : Convert.ToDecimal(context.Request.Params["pricement1per"]);
            decimal item2Per = context.Request.Params["pricement2per"] == null ? 0 : Convert.ToDecimal(context.Request.Params["pricement2per"]);
            foreach (var item in listtable)
            {
                var ss = item["amount"];

                var bb = Convert.ToDecimal(item["amount"]);
                totalAmount += ConvertHelper.ToDecimal<string>(item["amount"].ToString(), 0);
            }
            unpaidAmount = totalAmount - paidAmount;
            item1Amount = totalAmount * item1Per / 100;
            item2Amount = totalAmount * item2Per / 100;

            #endregion

            string text = context.Request["htmlcontent"] == null ? "" : context.Request["htmlcontent"].ToString();
            text = Regex.Replace(text, @"\s", "");
            Hashtable ht = new Hashtable();
            ht["contractNo"] = sbcontractNo;
            ht["frameContractNo"] = contractNo;
            ht["language"] = context.Request["language"] == null ? "" : context.Request["language"].ToString();
            ht["flowdirection"] = context.Request["flowdirection"] == null ? "" : context.Request["flowdirection"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellercode"] = context.Request["sellercode"] == null ? "" : context.Request["sellercode"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "" : context.Request["simpleSeller"].ToString();
            ht["sendTime"] = context.Request["sendTime"] == null ? "" : context.Request["sendTime"].ToString();
            ht["noticeTime"] = context.Request["noticeTime"] == null ? "" : context.Request["noticeTime"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyercode"] = context.Request["buyercode"] == null ? "" : context.Request["buyercode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["status"] = status;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            var createdate = context.Request.Params["createDate"];
            if (string.IsNullOrEmpty(createdate))
            {
                createdate = DateTime.Now.ToString();
            }
            ht["createdate"] = createdate;
            ht["contractText"] = text;
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK1)
            {
                ht["contractTag"] = ConstantUtil.CONTRACTTAG_SENDNOTICE;//标识进境发货通知
            }
            else
            {
                ht["contractTag"] = ConstantUtil.CONTRACTTAG_SENDNOTICENEW;//保存时标识进境发货通知
            }
            string splitstr = context.Request.Params["splitStr"];
            List<Hashtable> splitListTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(splitstr);
            string datagrid = context.Request.Params["datagrid"];//获取模板列表
            List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagrid);
            bool isOK = contractBll.addOrEditContractSendNotice(ref errorinfo, ht["contractNo"].ToString(), ht, listtable, splitListTable, templateno, status, ht["createman"].ToString(), templateTable);
            return isOK;
        }
        #endregion

        #region 物流合同添加编辑
        //添加编辑物流合同
        private bool addLogisticsContract(ref string err, HttpContext context)
        {
            bool suc = false;
            //获取物流合同编号
            var logisticsContractNo = context.Request.Params["logisticsContractNo"];
            var logisticsTemplateno = context.Request.Params["logisticsTemplateno"];
            if (string.IsNullOrEmpty(logisticsTemplateno))
            {
                logisticsTemplateno = Guid.NewGuid().ToString();
            }
            string status = string.Empty;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            if (context.Request.QueryString["status"] == "0")
            {
                //获取合同的状态 
                StringBuilder sb = new StringBuilder(@"select status from Econtract_logistics where logisticsContractNo=@logisticsContractNo");
                string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@logisticsContractNo", logisticsContractNo) }, "status");
                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                }
                if (string.IsNullOrEmpty(logisticsContractNo) || logisticsContractNo == "自动编号")
                {
                    logisticsContractNo = getSerialNumber();
                }
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                //删除新建合同，生成提交合同
                contractBll.deleteEcontract_logistics(logisticsContractNo);
                logisticsContractNo = generalContractNoBylogistics(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);

                if (confirmContractMan(ConstantUtil.ROLE_CONTRACTMAN, userAccount) || confirmAngency(ConstantUtil.ORG_JOBMAN, userId))
                {
                    //登录人角色为合同管理员或者用户所属部门为业务处，状态改为待业务处主管审核
                    status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK;//提交
                }
            }
            //获取主模板信息
            Hashtable ht = new Hashtable();
            ht["logisticsContractNo"] = logisticsContractNo;
            ht["logisticsTemplateName"] = context.Request["logisticsTemplateName"] == null ? "0" : context.Request["logisticsTemplateName"].ToString();
            ht["logisticsTemplateno"] = context.Request["logisticsTemplateno"] == null ? "0" : context.Request["logisticsTemplateno"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellerCode"] = context.Request["sellercode"] == null ? "" : context.Request["sellercode"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "" : context.Request["simpleSeller"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyerCode"] = context.Request["buyercode"] == null ? "" : context.Request["buyercode"].ToString();
            ht["contractText"] = context.Request["htmlcontent"] == null ? "" : context.Request["htmlcontent"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["isFrame"] = context.Request["isFrame"] == null ? "" : context.Request["isFrame"].ToString();
            ht["frameContractNo"] = context.Request["frameContractNo"] == null ? "" : context.Request["frameContractNo"].ToString();
            ht["frameContract"] = context.Request["frameContract"] == null ? "" : context.Request["frameContract"].ToString();
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            ht["createdate"] = context.Request["createdate"] == null ? DateTime.Now.ToString() : context.Request["createdate"].ToString();
            ht["adminReview"] = context.Request["adminReview"] == null ? "" : context.Request["adminReview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminReviewNumber"] == null ? "" : context.Request["adminReviewNumber"].ToString();
            //获取业务员的直线审核经理名称
            ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());

            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                ht["logisticsTag"] = ConstantUtil.LOGINSTICSTAG;
            }
            ht["status"] = status;
            //获取第一列表名信息
            Hashtable htFirstItem = new Hashtable();
            htFirstItem["logisticsContractNo"] = logisticsContractNo;
            htFirstItem["item1"] = context.Request["item1"] == null ? "" : context.Request["item1"].ToString();
            htFirstItem["item2"] = context.Request["item2"] == null ? "" : context.Request["item2"].ToString();
            htFirstItem["item3"] = context.Request["item3"] == null ? "" : context.Request["item3"].ToString();
            htFirstItem["item4"] = context.Request["item4"] == null ? "" : context.Request["item4"].ToString();
            htFirstItem["item5"] = context.Request["item5"] == null ? "" : context.Request["item5"].ToString();
            htFirstItem["item6"] = context.Request["item6"] == null ? "" : context.Request["item6"].ToString();
            htFirstItem["item7"] = context.Request["item7"] == null ? "" : context.Request["item7"].ToString();
            htFirstItem["item8"] = context.Request["item8"] == null ? "" : context.Request["item8"].ToString();
            htFirstItem["item1width"] = context.Request["item1width"] == null ? "" : context.Request["item1width"].ToString();
            htFirstItem["item2width"] = context.Request["item2width"] == null ? "" : context.Request["item2width"].ToString();
            htFirstItem["item3width"] = context.Request["item3width"] == null ? "" : context.Request["item3width"].ToString();
            htFirstItem["item4width"] = context.Request["item4width"] == null ? "" : context.Request["item4width"].ToString();
            htFirstItem["item5width"] = context.Request["item5width"] == null ? "" : context.Request["item5width"].ToString();
            htFirstItem["item6width"] = context.Request["item6width"] == null ? "" : context.Request["item6width"].ToString();
            htFirstItem["item7width"] = context.Request["item7width"] == null ? "" : context.Request["item7width"].ToString();
            htFirstItem["item8width"] = context.Request["item8width"] == null ? "" : context.Request["item8width"].ToString();
            //获取模板动态表格
            string logisticsItem = context.Request.Params["logisticsItem"];
            List<Hashtable> logisticsItemTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(logisticsItem);

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    //保存主表
                    suc = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT_LOGISTICS, "logisticsContractNo", ht["logisticsContractNo"].ToString(), ht);
                    if (suc)
                    {
                        //保存表名
                        suc = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT_LOGISTICSFIRSTITEM, "logisticsContractNo", htFirstItem["logisticsContractNo"].ToString(), htFirstItem);
                        //保存表格内容
                        //先删除，再添加
                        int r = DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_LOGISTICSITEMS, "logisticsContractNo", logisticsContractNo);
                        foreach (var hs in logisticsItemTable)
                        {
                            Hashtable htLogistics = new Hashtable();
                            htLogistics["item1"] = hs["item1"];
                            htLogistics["item2"] = hs["item2"];
                            htLogistics["item3"] = hs["item3"];
                            htLogistics["item4"] = hs["item4"];
                            htLogistics["item5"] = hs["item5"];
                            htLogistics["item6"] = hs["item6"];
                            htLogistics["item7"] = hs["item7"];
                            htLogistics["item8"] = hs["item8"];
                            htLogistics["logisticsContractNo"] = logisticsContractNo;

                            DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_ECONTRACT_LOGISTICSITEMS, htLogistics);
                        }
                    }
                    bll.SqlTran.Commit();

                }




                catch (Exception ex)
                {
                    err = ex.Message;

                }
                return suc;

            }
        }
        #endregion

        #region 获取进境发货通知合同号
        //获取进口发货通知合同号
        private string generalImportSendContractNo(string contractNo)
        {
            if (contractNo.Contains("FH"))
            {
                contractNo = contractNo.Substring(0, contractNo.Length - 6);
            }
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询境内发货通知合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;

                var contractFirst = bll.ExecuteScalar(string.Format(@"select top 1  contractNo from Econtract where contractTag={0}  order by id desc ", ConstantUtil.CONTRACTTAG_SENDNOTICE));

                if (contractFirst == null)
                {
                    contractFirst = "001";
                }

                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string strYear = contractFirst.ToString().Substring(contractFirst.ToString().Length - 9, 2);//获取合同中的年份
                    string monthNow = DateTime.Now.Month.ToString();
                    string yearNow = DateTime.Now.Year.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "000";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[2];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }

                }

                string random = string.Empty;
                random = String.Format("{0:D3}", contractOrg + 1);

                contractNumber = contractNo + "FH-" + random;

            }

            return contractNumber;
        }
        #endregion

        #region 获取服务合同框架子合同编号
        //获取进口发货通知合同号
        private string getServiceFrameContractNo(string frameContractNo)
        {
            string attno = frameContractNo + "-001";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(string.Format(@"select max(contractNo) from {0} where contractNo like @contractNo+'-%'", ConstantUtil.TABLE_ECONTRACT_SERVICE),
                    new SqlParameter[] { new SqlParameter("@contractNo", frameContractNo) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string tmp = ds.Tables[0].Rows[0][0].ToString();
                    string[] tt = tmp.Split('-');
                    if (tt.Length > 1)
                    {
                        attno = frameContractNo + "-" + (Convert.ToInt32(tt[tt.Length - 1]) + 1).ToString().PadLeft(3, '0');
                    }
                }
            }
            return attno;
        }
        #endregion

        #region 保存合同时生成自动编号
        //生成流水编号
        private string getSerialNumber()
        {
            Random random = new Random();
            string strRandom = random.Next(1, 10).ToString(); //生成编号 
            string code = "temp" + DateTime.Now.Year +Util.getNumberAddZero(DateTime.Now.Month.ToString(),2);//形如
            code = code+ Util.getSequenceAutoAddZero(code, 3);
            return code.Trim();
        }
        #endregion

        #region 获取生成合同附件编号
        private string generalAttachNo1(string contractNo)
        {
            string attno = contractNo + "-001";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(" select max(contractNo) from Econtract where contractNo like @contractNo+'-%' ",
                    new SqlParameter[] { new SqlParameter("@contractNo", contractNo) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string tmp = ds.Tables[0].Rows[0][0].ToString();
                    string[] tt = tmp.Split('-');
                    if (tt.Length > 1)
                    {
                        attno = contractNo + "-" + (Convert.ToInt32(tt[tt.Length - 1]) + 1).ToString().PadLeft(3, '0');
                    }
                }
            }
            return attno.Trim();
        }
        #endregion

        #region 提交合同时生成编号
        private string generalContractNo(string buyercode, string sellercode)
        {
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var ss = sellercode.Trim();
                //查询出买方卖方具体的名称
                var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where code=" + buyercode.Trim());
                var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + sellercode.Trim());
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = DataFactory.SqlDataBase().getString(new StringBuilder(@"select top 1  contractNo from Econtract where contractTag=@contractTag  order by id desc "),
                    new SqlParam[1] { new SqlParam("@contractTag", ConstantUtil.CONTRACTTAG_MAINCON) }, "contractNo");
                //var contractFirst = bll.ExecuteScalar(string.Format(@"select top 1  contractNo from Econtract where contractTag={0}  order by id desc ", ConstantUtil.CONTRACTTAG_MAINCON));
                contractFirst = contractFirst.ToString().Trim();
                if (contractFirst == null)
                {
                    contractFirst = "001";
                }

                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string strYear = contractFirst.ToString().Substring(contractFirst.ToString().Length - 9, 2);//获取合同中的年份
                    string monthNow = DateTime.Now.Month.ToString();
                    string yearNow = DateTime.Now.Year.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "000";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[1];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }

                }


                //查询数据表中是否存在买方卖方
                string buyCompany = buyerName.ToString();
                string sellerCompany = sellerName.ToString();
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];

                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;

                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = buyCode + "GY" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = sellerCode + "XS" + year + month + "-" + random;
                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = sellerCode + "XS" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = buyCode + "GY" + year + month + '-' + random;
                }


            }

            return contractNumber;
        }

        private string generalContractNo1(string buyercode, string sellercode)
        {
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询出买方卖方具体的名称
                SqlParameter[] pms = new SqlParameter[]{
                    new SqlParameter("@buyer",buyercode),
                    new SqlParameter("@seller",sellercode)
                };
                var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where name=@buyer", pms);
                var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where name=@seller", pms);
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;

                var contractFirst = bll.ExecuteScalar(@"select top 1  contractNo from Econtract  order by id desc");

                if (contractFirst == null)
                {
                    contractFirst = "001";
                }

                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string monthNow = DateTime.Now.Month.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "001";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[1];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                }


                //查询数据表中是否存在买方卖方
                string buyCompany = buyerName.ToString();
                string sellerCompany = sellerName.ToString();
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];

                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;

                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = buyCode + "GY" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = sellerCode + "XS" + year + month + "-" + random;
                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = sellerCode + "XS" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = buyCode + "GY" + year + month + '-' + random;
                }


            }

            return contractNumber;
        }
        #endregion

        #region 根据买卖方编码获取优先级前6位编码
        public string getNewContractNo(string contractNo, string buyercode, string sellercode, string contractType)
        {
            string[] contractArray = contractNo.Split('-');
            string contractNumber = string.Empty;
            string buyer = string.Empty;
            string seller = string.Empty;

            #region 获取合同号前六位
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where code=" + buyercode.Trim());
                var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + sellercode.Trim());
                string buyCompany = buyerName.ToString();
                string sellerCompany = sellerName.ToString();
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];


                #region 获取合同号前6位
                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyer = dtBuyCompany.Rows[0][2].ToString();
                        if (contractType == "WL" || contractType == "GL")
                        {
                            contractNumber = buyer;
                        }
                        else
                        {
                            contractNumber = buyer + "GY";
                        }


                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        seller = dtSellerCompany.Rows[0][2].ToString();
                        if (contractType == "WL" || contractType == "GL")
                        {
                            contractNumber = seller;
                        }
                        else
                        {
                            contractNumber = seller + "XS";
                        }

                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    seller = dtSellerCompany.Rows[0][2].ToString();
                    if (contractType == "WL" || contractType == "GL")
                    {
                        contractNumber = seller;
                    }
                    else
                    {
                        contractNumber = seller + "XS";
                    }
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyer = dtBuyCompany.Rows[0][2].ToString();
                    if (contractType == "WL" || contractType == "GL")
                    {
                        contractNumber = buyer;
                    }
                    else
                    {
                        contractNumber = buyer + "GY";
                    }
                }
                #endregion
            }
            #endregion

            string contractOne = contractArray[0];
            contractOne = contractOne.Substring(contractOne.Length - 4);
            string returnNumber = string.Empty;
            switch (contractType)
            {
                case "NJ":
                    if (contractArray.Length == 3)//说明为附件合同
                    {

                        returnNumber = "NJ" + contractNumber + contractOne + "-" + contractArray[contractArray.Length - 2] + "-" + contractArray[contractArray.Length - 1];
                    }
                    else
                    {
                        returnNumber = "NJ" + contractNumber + contractOne + "-" + contractArray[contractArray.Length - 1];
                    }
                    break;
                case "WL":
                    if (contractArray.Length == 3)//说明为附件合同
                    {
                        contractNumber = contractNumber.Substring(0, contractNumber.Length - 3);
                        returnNumber = contractNumber + "WL" + contractOne + "-" + contractArray[contractArray.Length - 2] + "-" + contractArray[contractArray.Length - 1];
                    }
                    else
                    {
                        //contractNumber = contractNumber.Substring(0,);
                        returnNumber = contractNumber + "WL" + contractOne + "-" + contractArray[contractArray.Length - 1];
                    }
                    break;
                case "GL":
                    if (contractArray.Length == 3)//说明为附件合同
                    {
                        contractNumber = contractNumber.Substring(0, contractNumber.Length - 3);
                        returnNumber = contractNumber + "GL" + contractOne + "-" + contractArray[contractArray.Length - 2] + "-" + contractArray[contractArray.Length - 1];
                    }
                    else
                    {
                        returnNumber = contractNumber + "GL" + contractOne + "-" + contractArray[contractArray.Length - 1];
                    }
                    break;
                case "CONTRACTNO":
                    if (contractArray.Length == 3)//说明为附件合同
                    {
                        returnNumber = contractNumber + contractOne + "-" + contractArray[contractArray.Length - 2] + "-" + contractArray[contractArray.Length - 1];
                    }
                    else
                    {
                        returnNumber = contractNumber + contractOne + "-" + contractArray[contractArray.Length - 1];
                    }
                    break;
                default:
                    break;
            }
            return returnNumber.Trim();

        }
        #endregion

        #region 获取内部结算单编号
        //获取内部结算单编号
        private string InternalContractNo(string buyercode, string sellercode)
        {
            string contractNumber = "NJ";
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询出买方卖方具体的名称
                var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where code=" + buyercode.Trim());
                var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + sellercode.Trim());
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = bll.ExecuteScalar(string.Format(@"select top 1  contractNo from {0} where contractTag='{1}' order by id desc", ConstantUtil.TABLE_ECONTRACT_INTERNAL, ConstantUtil.CONTRACTTAG_INTERNAL));
                if (contractFirst == null)
                {
                    contractFirst = "000";
                }

                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string strYear = contractFirst.ToString().Substring(contractFirst.ToString().Length - 9, 2);//获取合同中的年份
                    string monthNow = DateTime.Now.Month.ToString();
                    string yearNow = DateTime.Now.Month.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "000";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[1];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                }


                //查询数据表中是否存在买方卖方
                string buyCompany = buyerName.ToString();
                string sellerCompany = sellerName.ToString();
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];

                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;

                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber += buyCode + "GY" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber += sellerCode + "XS" + year + month + "-" + random;
                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber += sellerCode + "XS" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber += buyCode + "GY" + year + month + '-' + random;
                }


            }

            return contractNumber;
        }
        #endregion

        #region 发货申请后添加关联合同
        private bool saveCotactTrainContract(ref string err, HttpContext context)
        {
            string ifcheck = context.Request.Params["ifcheck"] ?? string.Empty;
            string contractNo = context.Request.Params["associatecontractNo"];
            string createDateTag = context.Request.Params["createDateTag"] ?? string.Empty;//获取发货申请标识
            string status = string.Empty;
            if (context.Request.QueryString["status"] == "0")
            {
                //获取合同的状态 
                StringBuilder sb = new StringBuilder(@"select status from Econtract where contractNo=@contractNo");
                string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
                if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
                {
                    status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
                }
                else
                {
                    status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
                }
                if (contractNo == "自动编号")
                {
                    contractNo = getSerialNumber();//生成流水编号
                }
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                contractBll.deleteEcontract(contractNo);
                //获取自动编号
                contractNo = getContractCode.getContractNumber(context.Request.Params["exchangeBuyerCode"], context.Request.Params["exchangeSellerCode"]);
                //contractNo = generalContractNo(context.Request.Params["exchangeBuyerCode"], context.Request.Params["exchangeSellerCode"]);
                //提交时校验必填项
                //bool isValidate = validateInput(context, ref err);
                //if (!isValidate)
                //{
                //    return false;
                //}
                status = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务总监审核

            }
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            string datagrid = context.Request.Params["datagrid"];//获取模板列表
            List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagrid);
            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["purchaseCode"] = context.Request.Params["exchangeContractNo"];
            ht["salesmanCode"] = context.Request.Params["salesmanCode"];
            ht["businessclass"] = context.Request.Params["businessclass"];
            ht["language"] = context.Request.Params["language"];
            ht["seller"] = context.Request.Params["exchangeSeller"];
            ht["sellercode"] = context.Request.Params["exchangeSellerCode"];
            ht["simpleSeller"] = context.Request.Params["exchangeSimpleSeller"];
            ht["selleraddress"] = context.Request.Params["exchangeSellerAddress"];
            ht["buyer"] = context.Request.Params["exchangeBuyer"];
            ht["buyercode"] = context.Request.Params["exchangeBuyerCode"];
            ht["simpleBuyer"] = context.Request.Params["exchangeSimpleBuyer"];
            ht["buyeraddress"] = context.Request.Params["exchangeAddress"];
            ht["signedtime"] = context.Request.Params["signedtime"];
            ht["signedplace"] = context.Request.Params["signedplace"];
            ht["currency"] = context.Request.Params["currency"];
            ht["pricement1"] = context.Request.Params["pricement1"];
            ht["pricement1per"] = context.Request.Params["pricement1per"];
            ht["pricement2"] = context.Request.Params["pricement2"];
            ht["pricement2per"] = context.Request.Params["pricement2per"];
            ht["pvalidity"] = context.Request.Params["pvalidity"];
            ht["shipment"] = context.Request.Params["shipment"];
            ht["paymentType"] = context.Request["paymentType"] == null ? "" : context.Request["paymentType"].ToString();
            ht["shipDate"] = context.Request["shipDate"] == null ? "" : context.Request["shipDate"].ToString();
            ht["transport"] = context.Request.Params["transport"];
            ht["tradement"] = context.Request.Params["tradement"];
            ht["tradeShow"] = context.Request.Params["tradeShow"];
            ht["harborout"] = context.Request.Params["harborout"];
            ht["harborarrive"] = context.Request.Params["harborarrive"];
            ht["harboroutCountry"] = context.Request.Params["harboroutCountry"];
            ht["harboroutarriveCountry"] = context.Request.Params["harboroutarriveCountry"];
            ht["deliveryPlace"] = context.Request.Params["deliveryPlace"];
            ht["harborclear"] = context.Request.Params["harborclear"];
            ht["placement"] = context.Request.Params["placement"];
            ht["validity"] = context.Request.Params["validity"];
            ht["supplemental"] = context.Request.Params["supplemental"];
            ht["remark"] = context.Request.Params["remark"];
            ht["templateno"] = context.Request.Params["templateno"];
            ht["templatename"] = context.Request.Params["templatename"];
            ht["status"] = status;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createmanname"] = RequestSession.GetSessionUser().UserName;
            ht["shippingmark"] = context.Request.Params["shippingmark"];
            ht["overspill"] = context.Request.Params["overspill"];
            ht["splitShipment"] = context.Request.Params["splitShipment"];
            ht["frameContract"] = context.Request.Params["frameContract"];
            ht["createdate"] = context.Request["createdate"] == null ? DateTime.Now.ToString() : context.Request["createdate"].ToString();
            ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
            ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd"); ;
            ht["contractAmount"] = context.Request.Params["contractAmount"];
            ht["item1Amount"] = context.Request.Params["item1Amount"];
            ht["item2Amount"] = context.Request.Params["item2Amount"];
            ht["paidAmount"] = context.Request.Params["paidAmount"];
            ht["unpaidAmount"] = context.Request.Params["unpaidAmount"];
            ht["productCategory"] = context.Request.Params["productCategory"];
            ht["flowdirection"] = context.Request.Params["flowdirection"];
            ht["applicationNo"] = createDateTag ?? string.Empty;
            ht["sendFactory"] = context.Request.Params["sendFactoryInspect"];
            ht["sendFactoryCode"] = context.Request.Params["sendFactoryCode"];
            ht["iscustoms"] = "是";
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                ht["contractTag"] = ConstantUtil.CONTRACTTAG_MAINCON;//标识进出境合同
            }
            ht["adminReview"] = context.Request["adminreview"] == null ? "" : context.Request["adminreview"].ToString();
            ht["adminReviewNumber"] = context.Request["adminreviewnumber"] == null ? "" : context.Request["adminreviewnumber"].ToString();
            //获取业务员的直线审核经理名称
            ht["salesReviewNumber"] = getSalesReviewNumber(RequestSession.GetSessionUser().UserAccount.ToString(), ht["businessclass"].ToString());
            bool isOK = contractBll.addOrEditContactContract(ref err, contractNo, ht, ht["purchaseCode"].ToString(), listtable,
             context.Request.Params["templateno"], createDateTag, templateTable, status, ht["createman"].ToString(), ifcheck);
            return isOK;
        }
        //添加关联合同
        //private bool saveCotactContract(ref string errorinfo, HttpContext context)
        //{
        //    //根据关联合同号校验其卖方与商检合同的卖方是否相同
        //    bool suc = checkSellerByInspect(context.Request.Params["seller"], context.Request.Params["purchaseCode"], ref  errorinfo);
        //    if (!suc)
        //    {
        //        return false;
        //    }
        //    string contractNo = context.Request.Params["contractNo"];
        //    string createDateTag = context.Request.Params["createDateTag"] ?? string.Empty;//获取发货申请标识
        //    string status = string.Empty;
        //    if (contractNo == "自动编号")
        //    {
        //        //获取自动编号
        //        contractNo = generalContractNo(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
        //    }
        //    if (context.Request.QueryString["status"] == "0")
        //    {
        //        //获取合同的状态 
        //        StringBuilder sb = new StringBuilder(@"select status from Econtract where contractNo=@contractNo");
        //        string contractStatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
        //        if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK7)//退回
        //        {
        //            status = ConstantUtil.STATUS_STOCKIN_CHECK7;//退回
        //        }
        //        else
        //        {
        //            status = ConstantUtil.STATUS_STOCKIN_NEW;//新建  
        //        }
        //        contractBll.deleteEcontract(contractNo);
        //        contractNo = getSerialNumber();//生成流水编号
        //    }
        //    else if (context.Request.QueryString["status"] == "1")
        //    {
        //        contractBll.deleteEcontract(contractNo);
        //        //获取自动编号
        //        contractNo = generalContractNo(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
        //        //提交时校验必填项
        //        bool isValidate = validateInput(context, ref errorinfo);
        //        if (!isValidate)
        //        {
        //            return false;
        //        }

        //        status = ConstantUtil.STATUS_STOCKIN_CHECK3;
        //    }
        //    string str = context.Request.Params["htcplistStr"];
        //    List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
        //    string datagrid = context.Request.Params["datagrid"];//获取模板列表
        //    List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagrid);
        //    Hashtable ht = new Hashtable();
        //    ht["contractNo"] = contractNo;
        //    ht["purchaseCode"] = context.Request.Params["purchaseCode"];
        //    ht["salesmanCode"] = context.Request.Params["salesmanCode"];
        //    ht["businessclass"] = context.Request.Params["businessclass"];
        //    ht["language"] = context.Request.Params["language"];
        //    ht["seller"] = context.Request.Params["seller"];
        //    ht["sellercode"] = context.Request.Params["sellercode"];
        //    ht["simpleSeller"] = context.Request.Params["simpleseller"];
        //    ht["buyer"] = context.Request.Params["buyer"];
        //    ht["buyercode"] = context.Request.Params["buyercode"];
        //    ht["simpleBuyer"] = context.Request.Params["simpleBuyer"];
        //    ht["buyeraddress"] = context.Request.Params["buyeraddress"];
        //    ht["signedtime"] = context.Request.Params["signedtime"];
        //    ht["signedplace"] = context.Request.Params["signedplace"];
        //    ht["currency"] = context.Request.Params["currency"];
        //    ht["pricement1"] = context.Request.Params["pricement1"];
        //    ht["pricement1per"] = context.Request.Params["pricement1per"];
        //    ht["pricement2"] = context.Request.Params["pricement2"];
        //    ht["pricement2per"] = context.Request.Params["pricement2per"];
        //    ht["pvalidity"] = context.Request.Params["pvalidity"];
        //    ht["shipment"] = context.Request.Params["shipment"];
        //    ht["transport"] = context.Request.Params["transport"];
        //    ht["tradement"] = context.Request.Params["tradement"];
        //    ht["tradeShow"] = context.Request.Params["tradeShow"];
        //    ht["harborout"] = context.Request.Params["harborout"];
        //    ht["harborarrive"] = context.Request.Params["harborarrive"];
        //    ht["harboroutCountry"] = context.Request.Params["harboroutCountry"];
        //    ht["harboroutarriveCountry"] = context.Request.Params["harboroutarriveCountry"];
        //    ht["deliveryPlace"] = context.Request.Params["deliveryPlace"];
        //    ht["harborclear"] = context.Request.Params["harborclear"];
        //    ht["placement"] = context.Request.Params["placement"];
        //    ht["validity"] = context.Request.Params["validity"];
        //    ht["supplemental"] = context.Request.Params["supplemental"];
        //    ht["remark"] = context.Request.Params["remark"];
        //    ht["templateno"] = context.Request.Params["templateno"];
        //    ht["templatename"] = context.Request.Params["templatename"];
        //    ht["status"] = status;
        //    ht["createman"] = RequestSession.GetSessionUser().UserAccount;
        //    ht["createmanname"] = RequestSession.GetSessionUser().UserName;
        //    ht["shippingmark"] = context.Request.Params["shippingmark"];
        //    ht["overspill"] = context.Request.Params["overspill"];
        //    ht["splitShipment"] = context.Request.Params["splitShipment"];
        //    ht["frameContract"] = context.Request.Params["frameContract"];
        //    ht["createdate"] = context.Request["createdate"] == null ? DateTime.Now.ToString() : context.Request["createdate"].ToString();
        //    ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
        //    ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd "); ;
        //    ht["contractAmount"] = context.Request.Params["contractAmount"];
        //    ht["item1Amount"] = context.Request.Params["item1Amount"];
        //    ht["item2Amount"] = context.Request.Params["item2Amount"];
        //    ht["paidAmount"] = context.Request.Params["paidAmount"];
        //    ht["unpaidAmount"] = context.Request.Params["unpaidAmount"];
        //    ht["productCategory"] = context.Request.Params["productCategory"];
        //    ht["flowdirection"] = context.Request.Params["flowdirection"];
        //    if (status == ConstantUtil.STATUS_STOCKIN_CHECK3)
        //    {
        //        ht["contractTag"] = ConstantUtil.CONTRACTTAG_MAINCON;//标识进出境合同 
        //    }
        //    bool isOK = contractBll.addOrEditContactContract(ref errorinfo, contractNo, ht, listtable, context.Request.Params["templateno"], createDateTag, templateTable, status, ht["createman"].ToString());
        //    return isOK;
        //}
        //根据关联合同号校验其卖方与商检合同的卖方是否相同

        private bool checkSellerByInspect(string seller, string purchasecode, ref string err)
        {
            bool suc = false;
            string sql = "select seller from Econtract_Inspect where purchaseCode=@purchasecode";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@purchasecode",purchasecode)
            };
            string code = contractBll.getScalarString(sql, pms).ToString();
            if (code.Equals(seller))
            {
                suc = true;
            }
            else
            {
                err = "关联合同卖方与商检合同卖方不匹配";
            }

            return suc;

        }
        #endregion

        #region 合同提交时校验必填项
        //提交时校验必填项
        private bool validateInput(HttpContext context, ref string errorinfo)
        {
            bool b = true;
            string isCustoms = context.Request.Params["isCustoms"] ?? string.Empty;//是否为报关合同
            string pvalidity = context.Request.Params["pvalidity"] ?? string.Empty;//价格有效期
            string validity = context.Request.Params["validity"] ?? string.Empty;//合同有效期
            string adminReview = context.Request.Params["adminReview"] ?? string.Empty;//合同审核人
            string salesmanCode = context.Request.Params["salesmanCode"] ?? string.Empty;//业务员
            string pricement1per = context.Request.Params["pricement1per"] ?? string.Empty;//价格条款1占比
            string buyercode = context.Request.Params["buyercode"] ?? string.Empty;//买方编码
            string sellercode = context.Request.Params["sellercode"] ?? string.Empty;//卖方编码
            if (string.IsNullOrEmpty(isCustoms) || string.IsNullOrEmpty(pvalidity) || string.IsNullOrEmpty(validity) ||
                string.IsNullOrEmpty(adminReview) || string.IsNullOrEmpty(salesmanCode) || string.IsNullOrEmpty(pricement1per) || string.IsNullOrEmpty(buyercode) || string.IsNullOrEmpty(sellercode))
            {
                errorinfo = "请校验必填项";
                b = false;
            }
            return b;
        }
        #endregion

        #region 删除物流合同
        //删除物流合同
        private bool deleteLogisticsContract(ref string err, HttpContext context)
        {
            //判断合同的状态，如果不是新建状态，提示不能删除
            StringBuilder strsql = new StringBuilder(" delete Econtract_logistics where logisticsContractNo=@logisticsContractNo;");
            strsql.Append("  delete Econtract_logisticsItems where logisticsContractNo=@logisticsContractNo; ");
            strsql.Append("  delete Econtract_logisticsFirstItem where logisticsContractNo=@logisticsContractNo; ");
            strsql.Append("  delete reviewdata where contractNo=@logisticsContractNo; ");
            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@logisticsContractNo",Value=context.Request.Params["logisticsContractNo"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {

                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql.ToString(), mms);
                    bll.SqlTran.Commit();
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 更改发货申请表状态为直接发货
        //更改发着申请表状态为直接发货
        private bool sendimmediate(ref string err, HttpContext context)
        {
            #region 校验商检
            //string ifcheck = context.Request.Params["ifcheck"] ?? string.Empty;
            //string transport = context.Request.Params["transport"] ?? string.Empty;
            //if (transport == "海运")
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendFormat(@"select * from {0} where purchaseCode=@purchaseCode", ConstantUtil.TABLE_ECONTRACT_INSPECT);
            //    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[1] { new SqlParam("@purchaseCode", context.Request.Params["contractNo"]) }, 0);
            //    if (dt.Rows.Count <= 0)//未创商检合同
            //    {
            //        if (ifcheck == "是")
            //        {
            //            err = "请先商检";
            //            return false;
            //        }
            //    }

            //}//海运时校验商检
            #endregion

            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            string createDateTag = context.Request.Params["createDateTag"] ?? string.Empty;
            string sendMan = context.Request.Params["sendMan"] ?? string.Empty;
            string sendManCode = context.Request.Params["sendManCode"] ?? string.Empty;
            string sendFactory = context.Request.Params["sendFactory"] ?? string.Empty;
            string sendFactoryCode = context.Request.Params["sendFactoryCode"] ?? string.Empty;
            string inspectApplyNo = context.Request.Params["inspectApplyNo"] ?? string.Empty;//商检编号
            string contractNo = context.Request.Params["contractNo"] ?? string.Empty;
            string sendStyle = context.Request.Params["sendStyle"] ?? string.Empty;//发货类型
            string pcode = context.Request.Params["pcode"] ?? string.Empty;
            string pname = context.Request.Params["pname"] ?? string.Empty;
            string quantity = context.Request.Params["quantity"] ?? string.Empty;
            string qunit = context.Request.Params["qunit"] ?? string.Empty;
            string createConVal = context.Request.Params["createContract"] ?? string.Empty;
            #region 根据用户选择是否创建关联报关合同创建合同
            if (createConVal == "1")
            {
                //生成新的合同编号
                string newContractNo = contractNo + "-A" + Util.getSequenceAutoAddZero(contractNo, 2);//税务发票流水号
                //创建关联报关合同，执行存储过程

                Hashtable ht_proc = new Hashtable();
                ht_proc["newContractNo"] = newContractNo;
                ht_proc["contractNo"] = contractNo;
                ht_proc["createDateTag"] = createDateTag;
                //根据发货申请单号查询发货申请表中的数量。
                string sendQuantity = DataFactory.SqlDataBase().getString(new StringBuilder(string.Format(@"select sendQuantity from {0} where createDateTag=@createDateTag")), new SqlParam[1] { new SqlParam("@createDateTag", createDateTag) }, "sendQuantity");
                string amount = DataFactory.SqlDataBase().getString(new StringBuilder(string.Format(@"select amount from {0} where createDateTag=@createDateTag")), new SqlParam[1] { new SqlParam("@createDateTag", createDateTag) }, "amount");
                ht_proc["quantity"] = sendQuantity;
                ht_proc["amount"] = amount;
                int i = DataFactory.SqlDataBase().ExecuteByProc("Econtract_proc", ht_proc);
                if (i <= 0)
                {
                    err = "提交失败";
                    return false;
                }

            }
            #endregion
            Hashtable hb = new Hashtable();
            hb["contactStatus"] = ConstantUtil.STATUS_CONTACT_SEND;//状态更改为已直接发货
            hb["sendMan"] = sendMan;
            hb["sendManCode"] = sendManCode;
            hb["sendFactory"] = sendFactory;
            hb["sendFactoryCode"] = sendFactoryCode;
            hb["sendStyle"] = sendStyle;
            SqlUtil.getBatchSqls(hb, ConstantUtil.TABLE_SENDOUTAPPDETAILS, "createDateTag", createDateTag, ref sqls, ref objs);
            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["applyNo"] = createDateTag;
            ht["inspectContractNo"] = inspectApplyNo;
            ht["pcode"] = pcode;
            ht["pname"] = pname;
            ht["quantity"] = quantity;
            ht["qunit"] = qunit;
            List<Hashtable> list_hb = new List<Hashtable>();
            list_hb.Add(ht);
            SqlUtil.getBatchSqls(list_hb, ConstantUtil.TABLE_ECONTRACT_INSPECT_SENDOUT, ref sqls, ref objs);
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        }
        #endregion

        #region 获取业务员的直线经理审核人
        //获取业务员的直线审核经理名称
        private string getSalesReviewNumber(string username, string angency)
        {
            string salesReviewNumber = string.Empty;
            StringBuilder sql = new StringBuilder(string.Format(@"select ui.UserRealName from Com_UserInfos ui 
join Tb_RolesAddUser ru on ru.UserId=ui.Userid  join Tb_Roles ro on ro.Id=ru.RolesId  
join Com_OrgAddUser oru on oru.UserId=ui.Userid  join Com_Organization org on org.Id=oru.OrgId
 where org.Agency='{0}' and ro.RolesName='{1}'", angency, ConstantUtil.ROLE_SALESREVIEW));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                salesReviewNumber = dt.Rows[0][0].ToString();
            }
            return salesReviewNumber;
        }
        #endregion

        #region 删除合同
        //删除合同
        private bool deleteContract(ref string errorinfo, HttpContext context)
        {
            //判断合同的状态，如果不是新建状态，提示不能删除
            StringBuilder strsql = new StringBuilder(" delete Econtract where contractNo=@contractNo;");
            //strsql.Append("  delete Econtract_a where contractNo=@contractNo; ");
            strsql.Append("  delete Econtract_ap where contractNo=@contractNo; ");
            strsql.Append("  delete Econtract_template where contractNo=@contractNo; ");
            strsql.Append("  delete reviewdata where contractNo=@contractNo; ");
            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                string a = bll.ExecuteScalar(" select status from  Econtract where contractNo=@contractNo;", mms).ToString();
                if (a.Equals("新建") == false)
                {
                    errorinfo = "合同已提交，不能删除！";
                    return false;
                }
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql.ToString(), mms);
                    bll.SqlTran.Commit();
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;
                    return false;
                }
            }
            return true;
        }


        #endregion

        #region 异步加载采购合同信息
        private string loadSaleData(ref string err, HttpContext context)
        {

            var contractNo = context.Request.Params["contractNo"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();

            sqldata.Append(" select * from Econtract where contractNo=@contractNo ");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }

        #endregion

        #region 校验是否已关联合同
        //校验是否已关联合同
        private bool checkContract(ref string errorinfo, HttpContext context)
        {
            string contactCode = context.Request.Params["contactCode"];
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //判断表中是否存在已关联合同号
                DataTable dt = bll.ExecDatasetSql(@"select purchaseCode from Econtract where 1=1").Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() == contactCode)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        #endregion

        #region 获取物流合同编号
        private string generalContractNoBylogistics(string buyercode, string sellercode)
        {
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询出买方卖方具体的名称
                var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where code=" + buyercode);
                var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + sellercode);
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = bll.ExecuteScalar(string.Format(@"select top 1  logisticsContractNo from Econtract_logistics where logisticsTag='{0}'order by id desc", ConstantUtil.LOGINSTICSTAG));
                if (contractFirst == null)
                {
                    contractFirst = "001";
                }
                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string strYear = contractFirst.ToString().Substring(contractFirst.ToString().Length - 9, 2);//获取合同中的年份
                    string monthNow = DateTime.Now.Month.ToString();
                    string yearNow = DateTime.Now.Year.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "000";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[1];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                }
                //查询数据表中是否存在买方卖方
                string buyCompany = buyerName.ToString();
                string sellerCompany = sellerName.ToString();
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];

                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;

                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = buyCode + "GL" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = sellerCode + "GL" + year + month + "-" + random;
                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = sellerCode + "GL" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = buyCode + "GL" + year + month + '-' + random;
                }
            }

            return contractNumber;
        }
        #endregion

        #region 获取服务合同编号,根据甲乙丙丁四方判断
        private string generalContractNoByService(string buyercode, string sellercode)
        {
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询出买方卖方具体的名称
                var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where code=" + buyercode);
                var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + sellercode);
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = bll.ExecuteScalar(string.Format(@"select top 1  contractNo from {0} where serviceTag='{1}'order by id desc", ConstantUtil.TABLE_ECONTRACT_SERVICE, ConstantUtil.CONTRACT_SERVICE));
                contractFirst = contractFirst.ToString().Trim();
                if (contractFirst == null)
                {
                    contractFirst = "001";
                }
                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string strYear = contractFirst.ToString().Substring(contractFirst.ToString().Length - 9, 2);//获取合同中的年份
                    string monthNow = DateTime.Now.Month.ToString();
                    string yearNow = DateTime.Now.Year.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "000";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[1];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                }
                //查询数据表中是否存在买方卖方
                string buyCompany = buyerName.ToString();
                string sellerCompany = sellerName.ToString();
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];

                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;
                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = buyCode + "WL" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = sellerCode + "WL" + year + month + "-" + random;
                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = sellerCode + "WL" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = buyCode + "WL" + year + month + '-' + random;
                }
            }
            return contractNumber.Trim();
        }
        private string generalContractNoByService(string buyercode, string sellercode, string partCName, string partDName)
        {
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询出买方卖方具体的名称
                var buyerName = DataFactory.SqlDataBase().getString(new StringBuilder(@"select shortname from bcustomer where code=@code"), new SqlParam[1] { new SqlParam("@code", buyercode) }, "shortname");
                var sellerName = DataFactory.SqlDataBase().getString(new StringBuilder(@"select shortname from bsupplier where code=@code"), new SqlParam[1] { new SqlParam("@code", sellercode) }, "shortname");
                //var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where code=" + buyercode);
                //var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + sellercode);
                //var partCName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + partCCode);
                //var partDName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + partDCode);

                #region 生成后三位随机编号
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = bll.ExecuteScalar(string.Format(@"select top 1  contractNo from {0} where serviceTag='{1}'order by id desc", ConstantUtil.TABLE_ECONTRACT_SERVICE, ConstantUtil.CONTRACT_SERVICE));
                contractFirst = contractFirst.ToString().Trim();
                if (contractFirst == null)
                {
                    contractFirst = "001";
                }
                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string strYear = contractFirst.ToString().Substring(contractFirst.ToString().Length - 9, 2);//获取合同中的年份
                    string monthNow = DateTime.Now.Month.ToString();
                    string yearNow = DateTime.Now.Year.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "000";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[1];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                }
                #endregion

                //查询数据表中是否存在买方卖方
                string buyCompany = buyerName ?? string.Empty;
                string sellerCompany = sellerName ?? string.Empty;
                partCName = partCName ?? string.Empty;
                partDName = partDName ?? string.Empty;
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany),
                        new SqlParameter("@partCName",partCName),
                        new SqlParameter("@partDName",partDName)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%';
                        select * from EncodingRules where @partCName  like '%'+companyName+'%';
                        select * from EncodingRules where @partDName  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];
                DataTable dtPartC = dsCompany.Tables[2];
                DataTable dtPartD = dsCompany.Tables[3];
                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;
                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                #region 根据优先级生成前几位

                //四方都存在根据优先级最高者生成编号
                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0 && dtPartC.Rows.Count > 0 && dtPartD.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partB = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partC = Convert.ToInt32(dtPartC.Rows[0][0]);
                    int partD = Convert.ToInt32(dtPartD.Rows[0][0]);
                    random = String.Format("{0:D3}", contractOrg + 1);
                    int[] Numbers = new int[] { partA, partB, partC, partD };
                    int max = Numbers.Max();
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "WL" + year + month + "-" + random;
                }
                //存在甲乙丙三方
                else if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0 && dtPartC.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partB = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partC = Convert.ToInt32(dtPartC.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partB, partC };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "WL" + year + month + "-" + random;
                }
                //存在甲乙丁三方
                else if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0 && dtPartD.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partB = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partD = Convert.ToInt32(dtPartD.Rows[0][0]);
                    random = String.Format("{0:D3}", contractOrg + 1);
                    int[] Numbers = new int[] { partA, partB, partD };
                    int max = Numbers.Max();
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "WL" + year + month + "-" + random;
                }
                //存在甲丙两方
                else if (dtBuyCompany.Rows.Count > 0 && dtPartC.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partC = Convert.ToInt32(dtPartC.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partC };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "WL" + year + month + "-" + random;
                }
                //存在甲丁两方
                else if (dtBuyCompany.Rows.Count > 0 && dtPartD.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partD = Convert.ToInt32(dtPartD.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partD };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "WL" + year + month + "-" + random;
                }
                //存在乙丙两方
                else if (dtSellerCompany.Rows.Count > 0 && dtPartC.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partC = Convert.ToInt32(dtPartC.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partC };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "WL" + year + month + "-" + random;
                }
                //存在乙丁两方
                else if (dtSellerCompany.Rows.Count > 0 && dtPartD.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partD = Convert.ToInt32(dtPartD.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partD };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "WL" + year + month + "-" + random;
                }
                else if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = buyCode + "WL" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = sellerCode + "WL" + year + month + "-" + random;
                    }
                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = sellerCode + "WL" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = buyCode + "WL" + year + month + '-' + random;
                }
                #endregion
            }
            return contractNumber.Trim();
        }
        #endregion

        #region 获取管理合同编号,根据甲乙丙丁四方判断

        private string generalContractNoByMannage(string buyercode, string sellercode, string partCName, string partDName)
        {
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询出买方卖方具体的名称
                var buyerName = DataFactory.SqlDataBase().getString(new StringBuilder(@"select shortname from bcustomer where code=@code"), new SqlParam[1] { new SqlParam("@code", buyercode) }, "shortname");
                var sellerName = DataFactory.SqlDataBase().getString(new StringBuilder(@"select shortname from bsupplier where code=@code"), new SqlParam[1] { new SqlParam("@code", sellercode) }, "shortname");
                //var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where code=" + buyercode);
                //var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + sellercode);
                //var partCName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + partCCode);
                //var partDName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + partDCode);

                #region 生成后三位随机编号
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = bll.ExecuteScalar(string.Format(@"select top 1  contractNo from {0} where logisticsTag='{1}'order by id desc", ConstantUtil.TABLE_ECONTRACT_LOGISTICS, ConstantUtil.LOGINSTICSTAG));
                contractFirst = contractFirst.ToString().Trim();
                if (contractFirst == null)
                {
                    contractFirst = "001";
                }
                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string strYear = contractFirst.ToString().Substring(contractFirst.ToString().Length - 9, 2);//获取合同中的年份
                    string monthNow = DateTime.Now.Month.ToString();
                    string yearNow = DateTime.Now.Year.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "000";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[1];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                }
                #endregion

                //查询数据表中是否存在买方卖方
                string buyCompany = buyerName ?? string.Empty;
                string sellerCompany = sellerName ?? string.Empty;
                partCName = partCName ?? string.Empty;
                partDName = partDName ?? string.Empty;
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany),
                        new SqlParameter("@partCName",partCName),
                        new SqlParameter("@partDName",partDName)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%';
                        select * from EncodingRules where @partCName  like '%'+companyName+'%';
                        select * from EncodingRules where @partDName  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];
                DataTable dtPartC = dsCompany.Tables[2];
                DataTable dtPartD = dsCompany.Tables[3];
                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;
                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                #region 根据优先级生成前几位

                //四方都存在根据优先级最高者生成编号
                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0 && dtPartC.Rows.Count > 0 && dtPartD.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partB = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partC = Convert.ToInt32(dtPartC.Rows[0][0]);
                    int partD = Convert.ToInt32(dtPartD.Rows[0][0]);
                    random = String.Format("{0:D3}", contractOrg + 1);
                    int[] Numbers = new int[] { partA, partB, partC, partD };
                    int max = Numbers.Max();
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "GL" + year + month + "-" + random;
                }
                //存在甲乙丙三方
                else if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0 && dtPartC.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partB = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partC = Convert.ToInt32(dtPartC.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partB, partC };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "GL" + year + month + "-" + random;
                }
                //存在甲乙丁三方
                else if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0 && dtPartD.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partB = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partD = Convert.ToInt32(dtPartD.Rows[0][0]);
                    random = String.Format("{0:D3}", contractOrg + 1);
                    int[] Numbers = new int[] { partA, partB, partD };
                    int max = Numbers.Max();
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "GL" + year + month + "-" + random;
                }
                //存在甲丙两方
                else if (dtBuyCompany.Rows.Count > 0 && dtPartC.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partC = Convert.ToInt32(dtPartC.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partC };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "GL" + year + month + "-" + random;
                }
                //存在甲丁两方
                else if (dtBuyCompany.Rows.Count > 0 && dtPartD.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtBuyCompany.Rows[0][0]);
                    int partD = Convert.ToInt32(dtPartD.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partD };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "GL" + year + month + "-" + random;
                }
                //存在乙丙两方
                else if (dtSellerCompany.Rows.Count > 0 && dtPartC.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partC = Convert.ToInt32(dtPartC.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partC };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "GL" + year + month + "-" + random;
                }
                //存在乙丁两方
                else if (dtSellerCompany.Rows.Count > 0 && dtPartD.Rows.Count > 0)
                {
                    int partA = Convert.ToInt32(dtSellerCompany.Rows[0][0]);
                    int partD = Convert.ToInt32(dtPartD.Rows[0][0]);
                    int[] Numbers = new int[] { partA, partD };
                    int max = Numbers.Max();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    //根据最大值的id获取其编号
                    StringBuilder sb = new StringBuilder(@"select code from EncodingRules where priority=@priority");
                    string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@priority", max) }, "code");
                    contractNumber = code + "GL" + year + month + "-" + random;
                }
                else if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = buyCode + "GL" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = sellerCode + "GL" + year + month + "-" + random;
                    }
                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = sellerCode + "GL" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = buyCode + "GL" + year + month + '-' + random;
                }
                #endregion
            }
            return contractNumber.Trim();
        }
        #endregion

        #region 获取管理合同编号
        private string generalContractNoByManage(string buyercode, string sellercode)
        {
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询出买方卖方具体的名称
                var buyerName = bll.ExecuteScalar(@"select shortname from bcustomer where code=" + buyercode);
                var sellerName = bll.ExecuteScalar(@"select shortname from bsupplier where code=" + sellercode);
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = bll.ExecuteScalar(string.Format(@"select top 1  contractNo from {0} where logisticsTag='{1}'order by id desc", ConstantUtil.TABLE_ECONTRACT_LOGISTICS, ConstantUtil.LOGINSTICSTAG));
                contractFirst = contractFirst.ToString().Trim();
                if (contractFirst == null)
                {
                    contractFirst = "001";
                }
                else
                {
                    //如果月份不相同，随机编号变为001
                    string str = contractFirst.ToString().Substring(contractFirst.ToString().Length - 6, 2);//获取合同中的月份
                    string strYear = contractFirst.ToString().Substring(contractFirst.ToString().Length - 9, 2);//获取合同中的年份
                    string monthNow = DateTime.Now.Month.ToString();
                    string yearNow = DateTime.Now.Year.ToString();
                    if (monthNow.Length == 1)
                    {
                        monthNow = "0" + monthNow;
                    }

                    if (!str.Equals(monthNow))
                    {
                        contractFirst = "000";
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                    else
                    {
                        string[] contractArray = contractFirst.ToString().Split('-');
                        contractFirst = contractArray[1];
                        contractOrg = Convert.ToInt32(contractFirst);
                    }
                }
                //查询数据表中是否存在买方卖方
                string buyCompany = buyerName.ToString();
                string sellerCompany = sellerName.ToString();
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];

                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;

                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = buyCode + "GL" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = sellerCode + "GL" + year + month + "-" + random;
                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = sellerCode + "GL" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = buyCode + "GL" + year + month + '-' + random;
                }
            }
            return contractNumber.Trim();
        }
        #endregion

        #region unuse
        //添加编辑合同附件
        private bool addAttachContract(ref string err, HttpContext context)
        {
            //获取主合同号
            string MaincontractNo = context.Request.Params["MaincontractNo"];
            //获取附件编号
            string contractNo = context.Request.Params["contractNo"];
            if (contractNo == "自动编号")
            {
                contractNo = generalAttachNo1(MaincontractNo);
            }
            string status = string.Empty;

            #region 必填项为空校验与复制合同号选择，确认新建或提交

            var isview = context.Request.QueryString["isview"] ?? "";
            var isattach = context.Request.QueryString["isattach"] ?? "";
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            #endregion

            #region 获取产品列表
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            #endregion
            var purchasecode = context.Request["purchaseCode"];
            Hashtable ht = new Hashtable();
            ht["contractNo"] = contractNo;
            ht["purchaseCode"] = context.Request["purchaseCode"] == null ? "" : context.Request["purchaseCode"].ToString();
            ht["language"] = context.Request["language"] == null ? "0" : context.Request["language"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellercode"] = context.Request["sellercode"] == null ? "" : context.Request["sellercode"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "0" : context.Request["simpleSeller"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyercode"] = context.Request["buyercode"] == null ? "" : context.Request["buyercode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["buyeraddress"] = context.Request["buyeraddress"] == null ? "" : context.Request["buyeraddress"].ToString();
            ht["currency"] = context.Request["currency"] == null ? "0" : context.Request["currency"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            ht["status"] = status;
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createdate"] = context.Request["createdate"] == null ? DateTime.Now.ToString() : context.Request["createdate"].ToString();
            ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
            ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd "); ;
            ht["productCategory"] = context.Request["productCategory"] == null ? "" : context.Request["productCategory"].ToString();
            ht["flowdirection"] = context.Request["flowdirection"] == null ? "" : context.Request["flowdirection"].ToString();
            Hashtable htTemplateItem = new Hashtable();
            htTemplateItem["id"] = context.Request.Params["id"] == null ? "" : context.Request["id"].ToString();
            htTemplateItem["item1"] = context.Request.Params["item1"] == null ? "" : context.Request["item1"].ToString();
            htTemplateItem["item2"] = context.Request.Params["item2"] == null ? "" : context.Request["item2"].ToString();
            htTemplateItem["item3"] = context.Request.Params["item3"] == null ? "" : context.Request["item3"].ToString();
            htTemplateItem["item4"] = context.Request.Params["item4"] == null ? "" : context.Request["item4"].ToString();
            htTemplateItem["item5"] = context.Request.Params["item5"] == null ? "" : context.Request["item5"].ToString();
            htTemplateItem["item1foreign"] = context.Request.Params["item1foreign"] == null ? "" : context.Request["item1foreign"].ToString();
            htTemplateItem["item2foreign"] = context.Request.Params["item2foreign"] == null ? "" : context.Request["item2foreign"].ToString();
            htTemplateItem["item3foreign"] = context.Request.Params["item3foreign"] == null ? "" : context.Request["item3foreign"].ToString();
            htTemplateItem["item4foreign"] = context.Request.Params["item4foreign"] == null ? "" : context.Request["item4foreign"].ToString();
            htTemplateItem["item5foreign"] = context.Request.Params["item5foreign"] == null ? "" : context.Request["item5foreign"].ToString();
            bool isOK = contractBll.addOrEditContractAttach(ref err, contractNo, ht, listtable, htTemplateItem);
            return isOK;
        }
        // 第一次添加分批产品表
        private bool addReviewFirst(ref string err, HttpContext context)
        {
            string contractNo = context.Request["contractNo"];
            string pname = context.Request["pname"];
            string pcode = context.Request["pcode"];
            int number = Convert.ToInt32(context.Request["number"]);
            int r = 0;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //id, contractNo, pcode, pname, amount
                //string strsql = "select * from Econtract_split where contractNo=@contractNo and pname=@pname";
                string sql = "insert into Econtract_split_a( pcode, pname)values(@pcode, @pname)";
                string delsql = "delete from Econtract_split_a ";
                SqlParameter[] pms = new SqlParameter[]{
                    new SqlParameter("@contractNo",contractNo),
                    new SqlParameter("@pcode",pcode),
                    new SqlParameter("@pname",pname),
                    new SqlParameter("@amount",""),
                };
                bll.ExecuteNonQuery(delsql, pms);
                //DataTable dt = bll.ExecDatasetSql(strsql, pms).Tables[0];
                //if (dt.Rows.Count>0)
                //{
                //    return "ok";
                //}
                for (int i = 0; i < number; i++)
                {
                    r = bll.ExecuteNonQuery(sql, pms);
                }
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
        //更新审核表
        private bool reviewData(ref string err, HttpContext context)
        {


            if (RequestSession.GetSessionUser().UserId == null)
            {
                err = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + "登录信息过期，请重新登录系统！" + "\"}";

                return false;
            }

            string log = context.Request.Params["log"];
            string status = context.Request.Params["status"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string contractNo = context.Request.Params["contractNo"];
            //获取合同原本的状态
            string contractStatus = context.Request.Params["contractStatus"];
            //获取工厂和仓库信息
            string factory = context.Request.Params["factory"] ?? string.Empty;
            string stock = context.Request.Params["stock"] ?? string.Empty;
            string sbStatus = string.Empty;
            //判断合同状态进行更新
            if (contractStatus == "提交")
            {
                sbStatus = "业务直线审核";
            }
            else if (contractStatus == "业务直线审核")
            {
                sbStatus = "合同管理员审核";
            }
            else if (contractStatus == "合同管理员审核")
            {
                sbStatus = "业务处总监审核";
            }
            else if (contractStatus == "业务处总监审核")
            {
                sbStatus = "财务人员审核";
            }
            else if (contractStatus == "财务人员审核")
            {
                sbStatus = "财务总监审核";
            }
            else if (contractStatus == "财务总监审核")
            {
                sbStatus = "销售总监审核";
            }
            if (status == "undefined" || string.IsNullOrEmpty(status))
            {
                err = "请选择审核状态";
                return false;
            }
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    //更新审核状态表
                    //id,
                    string sql = "insert into reviewdata( reviewstatus, reviewlog, status, reviewdate,reviewman, contractNo)values( @reviewstatus, @reviewlog, @status, @reviewdate,@reviewman, @contractNo)";
                    SqlParameter[] mms = new SqlParameter[]{
                           new SqlParameter("@reviewstatus",sbStatus),
                           new SqlParameter("@reviewlog",log),
                           new SqlParameter("@status",status),
                              new SqlParameter("@reviewman",user),
                           new SqlParameter("@reviewdate",DateTime.Now.ToString("yyyy-MM-dd ")),
                           new SqlParameter("@contractNo",contractNo),
                           
                       };

                    string strSql = "update Econtract set status=@reviewstatus,factory=@factory,stock=@stock where contractNo=@contractNo";
                    SqlParameter[] pms = new SqlParameter[]{
                        new SqlParameter("@reviewstatus",sbStatus),
                        new SqlParameter("@contractNo",contractNo),
                        new SqlParameter("@factory",factory),
                        new SqlParameter("@stock",stock),
                    };
                    int r = bll.ExecuteNonQuery(sql, mms);
                    if (r > 0)
                    {
                        bll.ExecuteNonQuery(strSql, pms);
                    }
                    bll.SqlTran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;
                }

            }




        }

        private string generalAttachNo(string contractNo)
        {
            string attno = contractNo + "-001";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(" select max(attachmentno) from Econtract_a where attachmentno like @contractNo+'-%' ",
                    new SqlParameter[] { new SqlParameter("@contractNo", contractNo) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string tmp = ds.Tables[0].Rows[0][0].ToString();
                    string[] tt = tmp.Split('-');
                    if (tt.Length > 1)
                    {
                        attno = contractNo + "-" + (Convert.ToInt32(tt[tt.Length - 1]) + 1).ToString().PadLeft(3, '0');

                    }
                }
            }
            return attno;
        }




        #region old
        //添加合同
        private bool addContract(ref string errorinfo, ref string template, ref string language, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];

            string purchaseCode = context.Request["contactCode"];
            string templateno = context.Request["templateno"];
            string seller = context.Request.Params["seller"];
            string buyer = context.Request.Params["buyer"];
            string status = string.Empty;
            var currency = context.Request.Params["currency"];
            var tradement = context.Request.Params["tradement"];
            errorinfo = contractNo;
            language = context.Request.Params["language"];
            template = context.Request.Params["templateno"];

            #region 必填项为空校验与复制合同号选择，确认新建或提交


            if (string.IsNullOrEmpty(templateno))
            {
                errorinfo = "模版编号不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(seller))
            {
                errorinfo = "卖方不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(buyer))
            {
                errorinfo = "买方不能为空";
                return false;
            }
            var isview = context.Request.QueryString["isview"] ?? "";
            //说明是复制创建
            if (isview == "true")
            {
                contractNo = generalContractNo1(context.Request.Params["buyername"], context.Request.Params["sellername"]);
            }
            else
            {
                //获取自动编号
                //contractNo = generalContractNo(context.Request.Params["buyername"], context.Request.Params["sellername"]);
                contractNo = RM.Busines.contract.getContractCode.getContractNumber(context.Request.Params["buyercode"], context.Request.Params["sellercode"]);
            }
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            #endregion

            #region 获取产品列表以及合同付款、未付款金额确认
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            #region 框架合同验证
            //框架合同验证
            string frameContract = context.Request.Params["frameContract"];
            if (frameContract == "否")
            {
                if (listtable.Count == 0)
                {
                    errorinfo = "非框架合同必须有产品";
                    return false;
                }
            }
            #endregion

            //根据产品金额计算出合同总金额，条款1金额，条款2金额，已付金额
            decimal totalAmount = 0;
            decimal item1Amount = 0;
            decimal item2Amount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;
            decimal item1Per = Convert.ToDecimal(context.Request.Params["pricement1per"]);
            decimal item2Per = Convert.ToDecimal(context.Request.Params["pricement2per"]);

            foreach (var item in listtable)
            {
                var ss = item["amount"];
                var bb = Convert.ToDecimal(item["amount"]);
                totalAmount += ConvertHelper.ToDecimal<string>(item["amount"].ToString(), 0);
            }
            unpaidAmount = totalAmount - paidAmount;
            item1Amount = totalAmount * item1Per / 100;
            item2Amount = totalAmount * item2Per / 100;

            #endregion

            #region 添加合同sql执行块
            #region old


            //Hashtable ht = new Hashtable();
            //ht["contractNo"] = contractNo;
            //ht["purchaseCode"] = context.Request["purchaseCode"] == null ? "" : context.Request["purchaseCode"].ToString();
            //ht["salesmanCode"] = context.Request["salesmanCode"] == null ? "" : context.Request["salesmanCode"].ToString();
            //ht["businessclass"] = context.Request["businessclass"] == null ? "" : context.Request["businessclass"].ToString();
            //ht["language"] = context.Request["language"] == null ? "0" : context.Request["language"].ToString();
            //ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            //ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "0" : context.Request["simpleSeller"].ToString();
            //ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            //ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            //ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            //ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            //ht["buyeraddress"] = context.Request["buyeraddress"] == null ? "" : context.Request["buyeraddress"].ToString();
            //ht["currency"] = context.Request["currency"] == null ? "0" : context.Request["currency"].ToString();
            //ht["pricement1"] = context.Request["pricement1"] == null ? "" : context.Request["pricement1"].ToString();
            //ht["pricement1per"] = context.Request["pricement1per"] == null ? "0" : context.Request["pricement1per"].ToString();
            //ht["pricement2"] = context.Request["pricement2"] == null ? "" : context.Request["pricement2"].ToString();
            //ht["pricement2per"] = context.Request["pricement2per"] == null ? "" : context.Request["pricement2per"].ToString();
            //ht["pvalidity"] = context.Request["pvalidity"] == null ? "" : context.Request["pvalidity"].ToString();
            //ht["shipment"] = context.Request["shipment"] == null ? "" : context.Request["shipment"].ToString();
            //ht["transport"] = context.Request["transport"] == null ? "" : context.Request["transport"].ToString();
            //ht["tradement"] = context.Request["tradement"] == null ? "0" : context.Request["tradement"].ToString();
            //ht["tradeShow"] = context.Request["tradeShow"] == null ? "" : context.Request["tradeShow"].ToString();
            //ht["harborout"] = context.Request["harborout"] == null ? "0" : context.Request["harborout"].ToString();
            //ht["harborarrive"] = context.Request["harborarrive"] == null ? "" : context.Request["harborarrive"].ToString();
            //ht["deliveryPlace"] = context.Request["deliveryPlace"] == null ? "" : context.Request["deliveryPlace"].ToString();
            //ht["harborclear"] = context.Request["harborclear"] == null ? "" : context.Request["harborclear"].ToString();
            //ht["placement"] = context.Request["placement"] == null ? "" : context.Request["placement"].ToString();
            //ht["validity"] = context.Request["validity"] == null ? "" : context.Request["validity"].ToString();
            //ht["supplemental"] = context.Request["supplemental"] == null ? "0" : context.Request["supplemental"].ToString();
            //ht["remark"] = context.Request["remark"] == null ? "" : context.Request["remark"].ToString();
            //ht["templateno"] = context.Request["templateno"] == null ? "0" : context.Request["templateno"].ToString();
            //ht["status"] = status;
            //ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            //ht["shippingmark"] = context.Request["shippingmark"] == null ? "" : context.Request["shippingmark"].ToString();
            //ht["overspill"] = context.Request["overspill"] == null ? "" : context.Request["overspill"].ToString();
            //ht["splitShipment"] = context.Request["splitShipment"] == null ? "0" : context.Request["splitShipment"].ToString();
            //ht["frameContract"] = context.Request["frameContract"] == null ? "" : context.Request["frameContract"].ToString();
            //ht["createdate"] = DateTime.Now.ToString("yyyy-MM-dd ");
            //ht["lastmod"] = RequestSession.GetSessionUser().UserAccount;
            //ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd "); ;
            //ht["contractAmount"] = totalAmount;
            //ht["item1Amount"] = context.Request["item1Amount"] == null ? "" : context.Request["item1Amount"].ToString();
            //ht["item2Amount"] = context.Request["item2Amount"] == null ? "" : context.Request["item2Amount"].ToString();
            //ht["paidAmount"] = context.Request["paidAmount"] == null ? "0" : context.Request["paidAmount"].ToString();
            //ht["unpaidAmount"] = context.Request["unpaidAmount"] == null ? "" : context.Request["unpaidAmount"].ToString();
            //bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT, "contractNo", ht["contractNo"].ToString(), ht);
            #endregion

            #region sql添加


            string strsql = @" insert into Econtract(contractNo,purchaseCode,salesmanCode,businessclass,language,seller,simpleSeller,selleraddress,signedtime,signedplace,buyer,simpleBuyer,
buyeraddress,currency,pricement1,pricement1per,pricement2,pricement2per,pvalidity,shipment,transport,tradement,tradeShow,harborout,harborarrive,deliveryPlace,harborclear,
placement,validity,supplemental,remark,templateno,status,createman,createdate,lastmod,lastmoddate,contractAmount,item1Amount,item2Amount,paidAmount,unpaidAmount
,shippingmark,overspill,splitShipment,frameContract,buyercode,sellercode)
values(@contractNo,@purchaseCode,@salesmanCode,@businessclass,@language,@seller,@simpleSeller,@selleraddress,@signedtime,@signedplace,@buyer,@simpleBuyer,@buyeraddress,@currency,@pricement1,
@pricement1per,@pricement2,@pricement2per,@pvalidity,@shipment,@transport,@tradement,@tradeShow,@harborout,@harborarrive,@deliveryPlace,@harborclear,@placement,@supplemental,
@validity,@remark,@templateno,@status,@createman,@createdate,@lastmod,@lastmoddate,@contractAmount,@item1Amount,@item2Amount,@paidAmount,@unpaidAmount,@shippingmark,
@overspill,@splitShipment,@frameContract,@buyercode,@sellercode);";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNo,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@purchaseCode",Value=purchaseCode,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@salesmanCode",Value=context.Request.Params["salesman"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessclass",Value="西出组",Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@language",Value=context.Request.Params["language"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@simpleSeller",Value=context.Request.Params["simpleSeller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@selleraddress",Value=context.Request.Params["selleraddress"],Size=2000},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@signedtime",Value=context.Request.Params["signedtime"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@signedplace",Value=context.Request.Params["signedplace"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@simpleBuyer",Value=context.Request.Params["simpleBuyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyeraddress",Value=context.Request.Params["buyeraddress"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1",Value=context.Request.Params["pricement1"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1per",Value=context.Request.Params["pricement1per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2",Value=context.Request.Params["pricement2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2per",Value=context.Request.Params["pricement2per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pvalidity",Value=context.Request.Params["pvalidity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipment",Value=context.Request.Params["shipment"],Size=300},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradement",Value=context.Request.Params["tradement"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradeShow",Value=context.Request.Params["tradeShow"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborout",Value=context.Request.Params["harborout"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborarrive",Value=context.Request.Params["harborarrive"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@deliveryPlace",Value=context.Request.Params["deliveryPlace"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborclear",Value=context.Request.Params["harborclear"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@placement",Value=context.Request.Params["placement"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@supplemental",Value=context.Request.Params["supplemental"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@remark",Value=context.Request.Params["remark"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@templateno",Value=context.Request.Params["templateno"],Size=50},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value=status,Size=10},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value="管理员",Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shippingmark",Value=context.Request.Params["shippingmark"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@overspill",Value=context.Request.Params["overspill"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@splitShipment",Value=context.Request.Params["splitShipment"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@frameContract",Value=context.Request.Params["frameContract"],Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd "),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DBNull.Value,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@contractAmount",Value=totalAmount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@item1Amount",Value=item1Amount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@item2Amount",Value=item2Amount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@paidAmount",Value=paidAmount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@unpaidAmount",Value=unpaidAmount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@buyercode",Value=context.Request.Params["buyername"],Size=20},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@sellercode",Value=context.Request.Params["sellername"],Size=20},
            };
            #endregion

            //DataFactory.SqlDataBase().ExecuteBySql(sql,)
            //保存模板
            StringBuilder sbmb = new StringBuilder("");
            sbmb.Append(@"delete Econtract_template where contractNo=@contractNo;
insert into Econtract_template(templateno, sortno, chncontent, engcontent, ruscontent, isinline, contractno) 
select templateno, sortno, chncontent, engcontent, ruscontent, isinline,@contractNo from btemp_detail where templateno=@templateno ; ");
            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(@"insert into Econtract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");
            // 保存分批发货产品
            StringBuilder sb3 = new StringBuilder();
            sb3.Append(@"insert into Econtract_split (contractNo,pcode,pname,amount)values(@contractNo,@pcode,@pname,@amount)");

            #endregion

            bool issuc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    //数据验证
                    if (context.Request.Params["buyeraddress"].Length == 0)
                    {
                        throw new Exception("买家地址不能为空!");
                    }
                    //执行合同主体添加

                    bll.ExecuteNonQuery(strsql, mms);
                    //执行模板添加
                    if (context.Request.Params["templateno"].Trim().Length > 0)
                    {
                        SqlParameter[] mms1 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            new SqlParameter{ParameterName="@templateno",Value=context.Request.Params["templateno"]  }
                    };
                        bll.ExecuteNonQuery(sbmb.ToString(), mms1);
                    }

                    #region 保存分批发货产品
                    //校验产品数量
                    //获取分批发货产品列表
                    string splitstr = context.Request.Params["splitStr"];
                    //当分批发货次数大于1时才进行添加
                    if (splitstr != "0")
                    {
                        List<Hashtable> splitListTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(splitstr);

                        int amount = 0;
                        foreach (var item in splitListTable)
                        {
                            amount += (int)item["amount"];
                        }
                        var produntCount = context.Request.Params["productCount"];
                        if (amount != Convert.ToInt32(produntCount))
                        {
                            errorinfo = "分批发货量与产品总量不符";
                            return false;
                        }
                        foreach (var item in splitListTable)
                        {
                            SqlParameter[] splitPms = new SqlParameter[]{
                new SqlParameter("@contractNo",contractNo),
                new SqlParameter("@pcode",item["pcode"]),
                new SqlParameter("@pname",item["pname"]),
                new SqlParameter("@amount",item["amount"]),
    
            };
                            //分批发货产品列表添加
                            bll.ExecuteNonQuery(sb3.ToString(), splitPms);
                        }
                    }


                    #endregion

                    //合同主产品循环添加
                    foreach (Hashtable hs in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            //默认为空字符串（属于合同的默认产品）
                            new SqlParameter{ParameterName="@attachmentno",Value=""  },
                            new SqlParameter{ParameterName="@pcode",Value=hs["pcode"]  },
                            new SqlParameter{ParameterName="@pname",Value=hs["pname"]  },
                            new SqlParameter{ParameterName="@quantity",Value=hs["quantity"]  },
                            new SqlParameter{ParameterName="@qunit",Value=hs["qunit"]  },
                            new SqlParameter{ParameterName="@price",Value=hs["price"]  },
                            new SqlParameter{ParameterName="@amount",Value=hs["amount"]  },
                            new SqlParameter{ParameterName="@packspec",Value=hs["packspec"]  },
                            new SqlParameter{ParameterName="@packing",Value=hs["packing"]  },
                            new SqlParameter{ParameterName="@pallet",Value=hs["pallet"]  },
                            new SqlParameter{ParameterName="@ifcheck",Value=hs["ifcheck"]  },
                            new SqlParameter{ParameterName="@ifplace",Value=hs["ifplace"]  }
                        };
                        bll.ExecuteNonQuery(sb2.ToString(), mms2);
                    }
                    bll.SqlTran.Commit();
                    issuc = true;
                }
                catch (Exception ex)
                {
                    errorinfo = ex.Message;
                    issuc = false;
                }
            }
            return issuc;
        }

        //添加合同附件

        private bool addContract_attach(ref string errorinfo, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            //生成附件编号
            string attachmentno = generalAttachNo1(contractNo);

            string templateno = context.Request["templateno"];
            string seller = context.Request.Params["seller"];
            string buyer = context.Request.Params["buyer"];
            string status = string.Empty;
            var currency = context.Request.Params["currency"];
            var tradement = context.Request.Params["tradement"];
            if (string.IsNullOrEmpty(templateno))
            {
                errorinfo = "模版编号不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(seller))
            {
                errorinfo = "卖方不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(buyer))
            {
                errorinfo = "买方不能为空";
                return false;
            }


            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            //根据产品金额计算出合同总金额，条款1金额，条款2金额，已付金额
            decimal totalAmount = 0;
            decimal item1Amount = 0;
            decimal item2Amount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;
            decimal item1Per = Convert.ToDecimal(context.Request.Params["pricement1per"]);
            decimal item2Per = Convert.ToDecimal(context.Request.Params["pricement2per"]);
            foreach (var item in listtable)
            {
                totalAmount += Convert.ToDecimal(item["amount"]);
            }
            unpaidAmount = totalAmount - paidAmount;
            item1Amount = totalAmount * item1Per / 100;
            item2Amount = totalAmount * item2Per / 100;
            string strsql = @" insert into Econtract(contractNo,purchaseCode,salesmanCode,businessclass,language,seller,simpleSeller,signedtime,signedplace,buyer,simpleBuyer,
buyeraddress,currency,pricement1,pricement1per,pricement2,pricement2per,pvalidity,shipment,transport,tradement,harborout,harborarrive,harborclear,
placement,validity,remark,templateno,status,createman,createdate,lastmod,lastmoddate,contractAmount,item1Amount,item2Amount,paidAmount,unpaidAmount)
values(@contractNo,@purchaseCode,@salesmanCode,@businessclass,@language,@seller,@simpleSeller,@signedtime,@signedplace,@buyer,@simpleBuyer,@buyeraddress,@currency,@pricement1,
@pricement1per,@pricement2,@pricement2per,@pvalidity,@shipment,@transport,@tradement,@harborout,@harborarrive,@harborclear,@placement,
@validity,@remark,@templateno,@status,@createman,@createdate,@lastmod,@lastmoddate,@contractAmount,@item1Amount,@item2Amount,@paidAmount,@unpaidAmount);";

            string lan = context.Request.Params["language"];
            if (lan[0] == ',')
            {
                lan = lan.TrimStart(',');
            }
            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=attachmentno,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@purchaseCode",Value="",Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@salesmanCode",Value=context.Request.Params["salesman"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessclass",Value="西出组",Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@language",Value=lan,Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@simpleSeller",Value=context.Request.Params["simpleSeller"],Size=1000},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@signedtime",Value=context.Request.Params["signedtime"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@signedplace",Value=context.Request.Params["signedplace"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@simpleBuyer",Value=context.Request.Params["simpleBuyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyeraddress",Value=context.Request.Params["buyeraddress"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1",Value=context.Request.Params["pricement1"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1per",Value=context.Request.Params["pricement1per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2",Value=context.Request.Params["pricement2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2per",Value=context.Request.Params["pricement2per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pvalidity",Value=context.Request.Params["pvalidity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipment",Value=context.Request.Params["shipment"],Size=300},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradement",Value=context.Request.Params["tradement"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborout",Value=context.Request.Params["harborout"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborarrive",Value=context.Request.Params["harborarrive"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborclear",Value=context.Request.Params["harborclear"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@placement",Value=context.Request.Params["placement"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@remark",Value=context.Request.Params["remark"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@templateno",Value=context.Request.Params["templateno"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value=status,Size=10},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value="管理员",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd "),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DBNull.Value,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@contractAmount",Value=totalAmount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@item1Amount",Value=item1Amount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@item2Amount",Value=item2Amount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@paidAmount",Value=paidAmount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@unpaidAmount",Value=unpaidAmount,Size=8},
};
            //保存模板
            StringBuilder sbmb = new StringBuilder("");
            sbmb.Append(@"delete Econtract_t where contractNo=@contractNo;
insert into Econtract_t(templateno,templatename,language,sortno,content,contractNo) 
select templateno,templatename,language,sortno,content,@contractNo from btemplate_contract where templateno=@templateno   and status=1 ; ");


            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(@" insert into Econtract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");

            bool issuc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    //数据验证
                    if (context.Request.Params["buyeraddress"].Length == 0)
                    {
                        throw new Exception("买家地址不能为空!");
                    }
                    bll.ExecuteNonQuery(strsql, mms);

                    if (context.Request.Params["templateno"].Trim().Length > 0)
                    {
                        SqlParameter[] mms1 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=attachmentno},
                            new SqlParameter{ParameterName="@templateno",Value=context.Request.Params["templateno"]  }
                    };
                        bll.ExecuteNonQuery(sbmb.ToString(), mms1);
                    }

                    foreach (Hashtable hs in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=attachmentno  },
                            //默认为空字符串（属于合同的默认产品）
                            new SqlParameter{ParameterName="@attachmentno",Value=""  },
                            new SqlParameter{ParameterName="@pcode",Value=hs["pcode"]  },
                            new SqlParameter{ParameterName="@pname",Value=hs["pname"]  },
                            new SqlParameter{ParameterName="@quantity",Value=hs["quantity"]  },
                            new SqlParameter{ParameterName="@qunit",Value=hs["qunit"]  },
                            new SqlParameter{ParameterName="@price",Value=hs["price"]  },
                            new SqlParameter{ParameterName="@amount",Value=hs["amount"]  },
                            new SqlParameter{ParameterName="@packspec",Value=hs["packspec"]  },
                            new SqlParameter{ParameterName="@packing",Value=hs["packing"]  },
                            new SqlParameter{ParameterName="@pallet",Value=hs["pallet"]  },
                            new SqlParameter{ParameterName="@ifcheck",Value=hs["ifcheck"]  },
                            new SqlParameter{ParameterName="@ifplace",Value=hs["ifplace"]  }
                        };
                        bll.ExecuteNonQuery(sb2.ToString(), mms2);
                    }
                    bll.SqlTran.Commit();
                    issuc = true;
                }
                catch (Exception ex)
                {
                    errorinfo = ex.Message;
                    issuc = false;
                }
            }
            return issuc;
        }

        //编辑合同
        private bool editContract(ref string errorinfo, ref string template, ref string language, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            errorinfo = contractNo;
            language = context.Request.Params["language"];
            template = context.Request.Params["templateno"];
            string frameContract = context.Request.Params["frameContract"];
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            //框架合同验证
            if (frameContract == "否")
            {
                if (listtable.Count == 0)
                {
                    errorinfo = "非框架合同必须有产品";
                    return false;
                }
            }

            //根据产品金额计算出合同总金额，条款1金额，条款2金额，已付金额
            decimal totalAmount = 0;
            decimal item1Amount = 0;
            decimal item2Amount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;
            decimal item1Per = Convert.ToDecimal(context.Request.Params["pricement1per"]);
            decimal item2Per = Convert.ToDecimal(context.Request.Params["pricement2per"]);
            foreach (var item in listtable)
            {
                totalAmount += Convert.ToDecimal(item["amount"]);
            }
            unpaidAmount = totalAmount - paidAmount;
            item1Amount = totalAmount * item1Per / 100;
            item2Amount = totalAmount * item2Per / 100;
            string strsql = @"update Econtract set purchaseCode=@purchaseCode,  simpleSeller=@simpleSeller,selleraddress=@selleraddress,simpleBuyer=@simpleBuyer,salesmanCode=@salesmanCode, businessclass=@businessclass,
language=@language,seller=@seller,signedtime=@signedtime,signedplace=@signedplace,buyer=@buyer,buyeraddress=@buyeraddress,currency=@currency,
status=@status,pricement1=@pricement1,pricement1per=@pricement1per,pricement2=@pricement2,pricement2per=@pricement2per,pvalidity=@pvalidity,
shipment=@shipment,transport=@transport,tradement=@tradement,tradeShow=@tradeShow,harborout=@harborout,harborarrive=@harborarrive,deliveryPlace=@deliveryPlace,harborclear=@harborclear,
placement=@placement,validity=@validity,supplemental=@supplemental,remark=@remark,templateno=@templateno,lastmod=@lastmod,lastmoddate=@lastmoddate,contractAmount=@contractAmount,
item1Amount=@item1Amount,item2Amount=@item2Amount,shippingmark=@shippingmark,overspill=@overspill,splitShipment=@splitShipment,frameContract=@frameContract,buyercode=@buyercode,sellercode=@sellercode
where  contractNo=@contractNo ";
            string status = string.Empty;
            string templateno = context.Request["templateno"];
            string seller = context.Request.Params["seller"];
            string buyer = context.Request.Params["buyer"];

            if (string.IsNullOrEmpty(templateno))
            {
                errorinfo = "模版编号不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(seller))
            {
                errorinfo = "卖方不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(buyer))
            {
                errorinfo = "买方不能为空";
                return false;
            }
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=false,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@purchaseCode",Value=context.Request.Params["purchaseCode"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@salesmanCode",Value=context.Request.Params["salesman"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessclass",Value="西出组",Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@language",Value=context.Request.Params["language"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@simpleSeller",Value=context.Request.Params["simpleSeller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@selleraddress",Value=context.Request.Params["selleraddress"],Size=1000},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@signedtime",Value=context.Request.Params["signedtime"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@signedplace",Value=context.Request.Params["signedplace"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@simpleBuyer",Value=context.Request.Params["simpleBuyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyeraddress",Value=context.Request.Params["buyeraddress"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1",Value=context.Request.Params["pricement1"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1per",Value=context.Request.Params["pricement1per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2",Value=context.Request.Params["pricement2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2per",Value=context.Request.Params["pricement2per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pvalidity",Value=context.Request.Params["pvalidity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipment",Value=context.Request.Params["shipment"],Size=300},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradement",Value=context.Request.Params["tradement"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradeShow",Value=context.Request.Params["tradement"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborout",Value=context.Request.Params["harborout"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborarrive",Value=context.Request.Params["harborarrive"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@deliveryPlace",Value=context.Request.Params["deliveryPlace"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborclear",Value=context.Request.Params["harborclear"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@placement",Value=context.Request.Params["placement"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@supplemental",Value=context.Request.Params["supplemental"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@remark",Value=context.Request.Params["remark"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@templateno",Value=context.Request.Params["templateno"],Size=50},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value=status,Size=10},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value="管理员",Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shippingmark",Value=context.Request.Params["shippingmark"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@overspill",Value=context.Request.Params["overspill"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@splitShipment",Value=context.Request.Params["splitShipment"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@frameContract",Value=context.Request.Params["frameContract"],Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd "),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DBNull.Value,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@contractAmount",Value=totalAmount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@item1Amount",Value=item1Amount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@item2Amount",Value=item2Amount,Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@buyercode",Value=context.Request.Params["buyername"],Size=20},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@sellercode",Value=context.Request.Params["sellername"],Size=20},

};
            //保存模板
            var temp = context.Request.Params["templateno"];
            StringBuilder sbmb = new StringBuilder("");
            sbmb.Append(@"delete Econtract_template where contractNo=@contractNo;
insert into Econtract_template(templateno, sortno, chncontent, engcontent, ruscontent, isinline, contractno) 
select templateno, sortno, chncontent, engcontent, ruscontent, isinline,@contractNo from btemp_detail where templateno=@templateno ; ");

            //删除更新分批产品表
            string delSpltSql = "delete from Econtract_split where contractNo=@contractNo";
            // 保存分批发货产品
            StringBuilder updateSplitSql = new StringBuilder();
            updateSplitSql.Append(@"insert into Econtract_split (contractNo,pcode,pname,amount)values(@contractNo,@pcode,@pname,@amount)");

            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();
            sb2.Append(@" insert into Econtract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");
            sb3.Append(@" update Econtract_ap set  pname=@pname,quantity=@quantity,qunit=@qunit,price=@price,amount=@amount,packspec=@packspec,
packing=@packing,pallet=@pallet,ifcheck=@ifcheck,ifplace=@ifplace where contractNo=@contractNo and attachmentno=@attachmentno and pcode=@pcode;
");


            //update 和 insert 产品表
            string oldpcodes = "";
            string olddel = "";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                SqlParameter[] ss2 = new SqlParameter[] { 
                    new SqlParameter{ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30}
                };
                DataTable dt = bll.ExecDatasetSql(" select pcode from Econtract_ap where contractNo=@contractNo and attachmentno='' ", ss2).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    oldpcodes = oldpcodes + "#" + dr["pcode"].ToString();
                    string oldpcode = dr["pcode"].ToString();
                    if (listtable.Exists(a => a["pcode"].ToString().Equals(oldpcode)) == false)
                    {
                        olddel += "'" + oldpcode + "',";
                    }
                }
                olddel = olddel.TrimEnd(',');
            }
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    #region 保存分批发货产品
                    //获取分批发货产品列表
                    string splitstr = context.Request.Params["splitStr"];
                    //当分批发货次数大于1时才进行添加
                    if (splitstr != "0")
                    {
                        List<Hashtable> splitListTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(splitstr);
                        //删除后添加
                        bll.ExecuteNonQuery(delSpltSql, mms);
                        //校验产品数量
                        int amount = 0;
                        foreach (var item in splitListTable)
                        {
                            amount += Convert.ToInt32(item["amount"]);
                        }
                        var produntCount = context.Request.Params["productCount"];
                        if (amount != Convert.ToInt32(produntCount))
                        {
                            errorinfo = "分批发货量与产品总量不符";
                            return false;
                        }

                        foreach (var item in splitListTable)
                        {
                            SqlParameter[] splitPms = new SqlParameter[]{
                new SqlParameter("@contractNo",contractNo),
                new SqlParameter("@pcode",item["pcode"]),
                new SqlParameter("@pname",item["pname"]),
                new SqlParameter("@amount",item["amount"]),
              
            };

                            bll.ExecuteNonQuery(updateSplitSql.ToString(), splitPms);
                        }
                    }


                    #endregion
                    bll.ExecuteNonQuery(strsql, mms);

                    if (context.Request.Params["templateno"].Trim().Length > 0)
                    {
                        //保存模板
                        SqlParameter[] mms1 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            new SqlParameter{ParameterName="@templateno",Value=context.Request.Params["templateno"]  }
                        };
                        bll.ExecuteNonQuery(sbmb.ToString(), mms1);
                    }

                    foreach (var a in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNo,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value="",Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pcode",Value=a["pcode"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pname",Value=a["pname"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@quantity",Value=a["quantity"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@qunit",Value=a["qunit"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@price",Value=a["price"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@amount",Value=a["amount"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packspec",Value=a["packspec"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packing",Value=a["packing"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pallet",Value=a["pallet"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifcheck",Value=a["ifcheck"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifplace",Value=a["ifplace"],Size=20},

};
                        if (oldpcodes.Contains(a["pcode"].ToString()))
                        {
                            bll.ExecuteNonQuery(sb3.ToString(), mms2);
                        }
                        else
                        {
                            bll.ExecuteNonQuery(sb2.ToString(), mms2);
                        }
                    }
                    //删除某些项目
                    if (olddel.Length > 0)
                    {
                        SqlParameter[] mms3 = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNo,Size=30}
};
                        bll.ExecuteNonQuery(" delete Econtract_ap where contractNo=@contractNo and attachmentno='' and pcode in (" + olddel + ")", mms3);
                    }
                    bll.SqlTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;
                    return false;
                }
            }
        }
        //删除合同附件
        private bool deleteContractfj(ref string errorinfo, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder("");
            //strsql.Append("  delete Econtract_a where contractNo=@contractNo and attachmentno=@attachmentno; ");
            strsql.Append("  delete Econtract_ap where contractNo=@contractNo and attachmentno=@attachmentno; ");
            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] },
                new SqlParameter{ ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql.ToString(), mms);
                    bll.SqlTran.Commit();
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;
                    return false;
                }
            }
            return true;
        }

        #endregion

        #endregion

        #region 文件上传
        //上传文件
        private string uploadExportFile(HttpContext context)
        {


            if (context.Request.Files.Count > 0)
            {
                var file = context.Request.Files[0];

                string path = "/Files/Contract/ExportContract/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "/" + file.FileName;
                Directory.CreateDirectory(Path.GetDirectoryName(context.Server.MapPath(path)));
                file.SaveAs(context.Server.MapPath(path));
                return path + ":" + file.FileName;
            }
            else
            {
                return "error";
            }
        }

        private string uploadServiceFile(HttpContext context)
        {


            if (context.Request.Files.Count > 0)
            {
                var file = context.Request.Files[0];

                string path = "/Files/Contract/ServiceContract/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "/" + file.FileName;
                Directory.CreateDirectory(Path.GetDirectoryName(context.Server.MapPath(path)));
                file.SaveAs(context.Server.MapPath(path));
                return path + ":" + file.FileName;
            }
            else
            {
                return "error";
            }
        }
        #endregion

        #region 返回json格式

        private string returnData(bool isok, string err)
        {
            string r = "";
            if (isok)
            {
                r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            }
            else
            {
                r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            }
            return r;
        }

        private string returnData1(bool isok, string err, string template, string lanugage)
        {
            string r = "";
            if (isok)
            {
                r = "{\"sucess\": 1,\"contract\": \"" + err + "\",\"template\": \"" + template + "\",\"language\": \"" + lanugage + "\",\"warnmsg\": \"\",\"errormsg\": \"\"}";
            }
            else
            {
                r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            }
            return r;
        }
        #endregion

        #region 判断登录用户为何种角色
        //判断登录用户是否为合同管理员
        public bool confirmContractMan(string roles, string user)
        {
            StringBuilder sb = new StringBuilder(@"select t3.UserRealName as cname,t4.LoginName as code from Tb_RolesAddUser t1 left join Tb_Roles t2 
 on t1.rolesId=t2.Id left join Com_UserInfos t3 on t3.Userid=t1.UserId left join Com_UserLogin t4 on t4.UserId=t1.UserId  where t2.RolesName=@roles and t4.LoginName=@user");
            string code = DataFactory.SqlDataBase().getString(sb, new SqlParam[2] { new SqlParam("@user", user), new SqlParam("@roles", roles) }, "code");
            if (!string.IsNullOrWhiteSpace(code))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 判断登录用户所处部门
        public bool confirmAngency(string angency, string userId)
        {
            bool b = false;
            StringBuilder sb = new StringBuilder(@"select Agency from Com_Organization where Id = 
                             (select OrgId from Com_OrgAddUser where UserId=@userId)");
            string angencyName = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@userId", userId) }, "Agency");
            if (string.Equals(angency, angencyName))
            {
                b = true;
            }
            return b;

        }
        #endregion

        #region 获取用户所处部门
        public string getAngency(string userId)
        {
            StringBuilder sb = new StringBuilder(@"select Agency from Com_Organization where Id = 
                             (select OrgId from Com_OrgAddUser where UserId=@userId)");
            string angencyName = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@userId", userId) }, "Agency");
            return angencyName;
        }
        #endregion

        #region 更新合同表保费运费
        private bool updateCost(ref string err, HttpContext context)
        {
            string premium = context.Request.Params["premium"];
            string shipCost = context.Request.Params["shipCost"];
            string contractNo = context.Request.Params["contractNo"];
            Hashtable ht = new Hashtable();
            ht.Add("premium", premium);
            ht.Add("shipCost", shipCost);
            int r = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", contractNo, ht);
            return r > 0 ? true : false;

        }

        #endregion

        #region 上传承兑excel
        private bool saveAcceptData(ref string err, HttpContext context)
        {
            string datagridJson = context.Request.Params["datagridJson"];
            List<Hashtable> excelList = JsonHelper.DeserializeJsonToList<Hashtable>(datagridJson);
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            SqlUtil.getBatchSqls(excelList, ConstantUtil.TABLE_PAYACCEPT, ref sqls, ref objs);
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        }
        #endregion

        #region 删除承兑excel
        private bool deleteAcceptFile(ref string err, HttpContext context)
        {
            var acceptno = context.Request.Params["acceptno"];
            int r = DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_PAYACCEPT, "acceptno", acceptno);
            return r > 0 ? true : false;
        }

        #endregion

        #region 更新合同打印状态
        private bool updatePrintStatus(ref string err, HttpContext context)
        {
            string tableName = context.Request.Params["tableName"] ?? string.Empty;
            string isPrint = context.Request.Params["isPrint"] ?? string.Empty;
            string isDownload = context.Request.Params["isDownload"] ?? string.Empty;
            string contractNo = context.Request.Params["contractNo"] ?? string.Empty;
            Hashtable ht = new Hashtable();
            if (isPrint == "true")
            {
                //已打印状态
                ht.Add("printStatus", "2");

            }
            else if (isDownload == "true")
            {
                //已下载状态
                ht.Add("printStatus", "1");
            }
            int r = DataFactory.SqlDataBase().UpdateByHashtable(tableName, "contractNo", contractNo, ht);
            return r > 0 ? true : false;
        }
        #endregion

        #region 实现接口
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

    }
}