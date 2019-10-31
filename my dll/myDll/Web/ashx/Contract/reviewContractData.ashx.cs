using RM.Busines;
using RM.Busines.contract;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// reviewContractData 的摘要说明
    /// </summary>
    public class reviewContractData : IHttpHandler, IRequiresSessionState
    {
        contractBLL bll = new contractBLL();
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/html";
            string module = context.Request["module"];
            string suc = string.Empty;
            string err = string.Empty;
            switch (module)
            {

                #region 获取审核列表
                case "reviewExportContract"://获取出境进境合同审核列表

                    suc = reviewExportContract(context);
                    context.Response.Write(suc);
                    break;
                case "reviewLogisticsContract"://获取物流合同审核列表

                    suc = reviewLogisticsContract(context);
                    context.Response.Write(suc);
                    break;
                case "reviewServiceContract"://获取服务合同审核列表

                    suc = reviewServiceContract(context);
                    context.Response.Write(suc);
                    break;
                case "reviewManageContract"://获取管理合同审核列表

                    suc = reviewManageContract(context);
                    context.Response.Write(suc);
                    break;
                case "reviewInternalContract"://获取内部清算单审核列表

                    suc = reviewInternalContract(context);
                    context.Response.Write(suc);
                    break; 
                #endregion

                #region 获取废弃合同审核列表
                case "reviewAbandonExportContract"://获取废弃出境合同审核列表
                    suc = reviewAbandonExportContract(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 获取列表信息
                case "LoadTrainContactApp"://获取铁路合同列表数据
                    suc = LoadTrainContactApp(context);
                    context.Response.Write(suc);
                    break;
                case "LoadApp"://获取合同列表数据
                    suc = LoadApp(context);
                    context.Response.Write(suc);
                    break;
                case "LoadEditSendNotice"://获取进境发货通知修改数据
                    suc = LoadEditSendNotice(context);
                    context.Response.Write(suc);
                    break;
                case "LoadReviewApp"://获取审核合同列表数据
                    suc = LoadReviewApp(context);
                    context.Response.Write(suc);
                    break;
                case "LoadContactApp"://获取关联合同列表数据
                    suc = LoadContactApp(context);
                    context.Response.Write(suc);
                    break;
                case "LoadInspectDetails"://获取商检合同信息
                    suc = LoadInspectDetails(context);
                    context.Response.Write(suc);
                    break; 
                #endregion

                #region 根据进出口模板加载数据
                case "LoadDataByExportTemp"://根据出口合同模板加载数据
                    suc = LoadDataByExportTemp(context);
                    context.Response.Write(suc);
                    break;
                case "LoadDataByImportTemp"://根据进口合同模板加载数据
                    suc = LoadDataByImportTemp(context);
                    context.Response.Write(suc);
                    break; 
                #endregion

                #region 加载物流合同信息
                case "LoadLogistics"://获取物流合同列表数据
                    suc = LoadLogistics(context);
                    context.Response.Write(suc);
                    break;
                case "LoadLogisticsFirstItem"://获取物流合同表格头部数据
                    suc = LoadLogisticsFirstItem(context);
                    context.Response.Write(suc);
                    break; 
                #endregion
             
                #region 加载服务合同信息
                case "LoadServiceReviewApp"://获取服务合同审核合同列表数据
                    suc = LoadServiceReviewApp(context);
                    context.Response.Write(suc);
                    break;
                case "LoadServiceData":
                    suc = LoadServiceData(ref err, context);
                    context.Response.Write(suc);
                    break;
                case "LoadServiceAttachData"://加载服务合同框架子合同信息
                    suc = LoadServiceAttachData(ref err, context);
                    context.Response.Write(suc);
                    break;
                case "LoadServiceContactData"://加载服务合同关联合同信息
                    suc = LoadServiceContactData(ref err, context);
                    context.Response.Write(suc);
                    break; 
                #endregion

                #region 加载管理合同信息
                     case "LoadManageReviewApp"://获取服务合同审核合同列表数据
                    suc = LoadManageReviewApp(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 其他
                case "uploadFile":
                    suc = uploadFile(ref err, context);
                    context.Response.Write(suc);
                    break;
                case "getBackContract"://获取业务员退回的合同

                    suc = getBackContract(context);
                    context.Response.Write(suc);
                    break;
                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break; 
                #endregion
            }
        }


        #region 获取废弃合同审核列表数据
          private string reviewAbandonExportContract(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string flowdirection = context.Request["flowdirection"];
            string isDesk = context.Request["isDesk"];
            return bll.GetAbandonReviewContractList(row, page, order, sort, flowdirection, isDesk, signedtime_begin, signedtime_end);
        }
        #endregion


        #region 获取审核列表数据

        #region 获取服务合同审核列表
        private string reviewServiceContract(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string isDesk = context.Request["isDesk"];
            sort = "status2 asc,createdate desc";
            order = "";
            return bll.GetServiceReviewContractList(row, page, order, sort, isDesk);
        }
        #endregion

        #region 获取内部清算单审核列表

        private string reviewInternalContract(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            sort = "status2 asc,createdate desc";
            order = "";
            string isDesk = context.Request["isDesk"];
            return bll.GetInternalContractList(row, page, order, sort, isDesk, signedtime_begin, signedtime_end);
        }
        #endregion

        #region 获取物流合同审核列表
        private string reviewLogisticsContract(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string isDesk = context.Request["isDesk"];
            return bll.GetLogisticsReviewContractList(row, page, order, sort, isDesk);
        }
        #endregion

        #region 获取出境合同审核列表
        //获取出境合同审核列表
        private string reviewExportContract(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string flowdirection = context.Request["flowdirection"];
            string isDesk = context.Request["isDesk"];
            string unReview = context.Request.Params["unReview"];
            //order = "createdate desc";
            sort = "status2 asc,createdate desc";
            order = "";
            return bll.GetReviewContractList(row, page, order, sort, flowdirection, isDesk, signedtime_begin, signedtime_end);
        }
        #endregion

        #region 获取审核合同信息与开会审核详细信息
        private string LoadReviewApp(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            string status1 = getReviewStatus(contractNo);//获取合同上一个节点的状态
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(status1))
            {
                sb.Append(string.Format(@"select t1.*,t2.meetlog,t2.filepath as upMationDetails,t2.meettime,t2.isMeetReview as isMeet from {0} t1 
                                         ,{2} t2  where t1.contractNo=t2.contractNo and t1.contractNo='{1}'",
                                       ConstantUtil.TABLE_ECONTRACT, contractNo, ConstantUtil.TABLE_REVIEWDATA));

            }
            else
            {
                sb.Append(string.Format(@" select t1.*,t2.meetlog,t2.filepath as upMationDetails,t2.meettime,t2.isMeetReview as isMeet
   from (select * from {0} where contractNo='{1}') t1  left join
{2} t2  on t1.contractNo=t2.contractNo  and t2.reviewstatus='{3}'
",
                                                       ConstantUtil.TABLE_ECONTRACT, contractNo, ConstantUtil.TABLE_REVIEWDATA, status1));
            }

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }

            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 获取服务合同审核合同信息与开会审核详细信息
        private string LoadServiceReviewApp(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            string status1 = getServiceReviewStatus(contractNo);//获取合同上一个节点的状态
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(status1))
            {
                sb.Append(string.Format(@"select t1.id,  t1.contractNo,  t1.frameContractNo,  t1.buyerCode,  t1.simpleBuyer,  t1.buyer,
 t1.sellerCode,  t1.simpleSeller,  t1.seller,  t1.signedPlace,  t1.signedTime,  t1.createman,  t1.createdate,  t1.status,  t1.salemanCode, 
 t1.isFrame,  t1.reviewtime,  t1.serviceTag,  t1.frameContract,  t1.salesReviewNumber,  t1.adminReview,  t1.adminReviewNumber,  t1.businessclass,
 t1.salesmanCode, 
 t1.createmanname,  t1.simplePartyC,  t1.partyC,  t1.simplePartyD,  t1.partyD,  t1.partyCCode,  t1.partyDCode,  t1.validity,t2.meetlog,t2.filepath as upMationDetails,
t2.meettime,t2.isMeetReview as isMeet from {0} t1 
                                         ,{2} t2  where t1.contractNo=t2.contractNo and t1.contractNo='{1}'",
                                       ConstantUtil.TABLE_ECONTRACT_SERVICE, contractNo, ConstantUtil.TABLE_REVIEWDATA));

            }
            else
            {
                sb.Append(string.Format(@" select  t1.id,  t1.contractNo,  t1.frameContractNo,  t1.buyerCode,  t1.simpleBuyer,  t1.buyer,
 t1.sellerCode,  t1.simpleSeller,  t1.seller,  t1.signedPlace,  t1.signedTime,  t1.createman,  t1.createdate,  t1.status,  t1.salemanCode, 
 t1.isFrame,  t1.reviewtime,  t1.serviceTag,  t1.frameContract,  t1.salesReviewNumber,  t1.adminReview,  t1.adminReviewNumber,  t1.businessclass,
 t1.salesmanCode, 
 t1.createmanname,  t1.simplePartyC,  t1.partyC,  t1.simplePartyD,  t1.partyD,  t1.partyCCode,  t1.partyDCode, t1.validity,t2.meetlog,t2.filepath as upMationDetails,t2.meettime,t2.isMeetReview as isMeet
   from (select * from {0} where contractNo='{1}') t1  left join
{2} t2  on t1.contractNo=t2.contractNo  and t2.reviewstatus='{3}'
",
                                                       ConstantUtil.TABLE_ECONTRACT_SERVICE, contractNo, ConstantUtil.TABLE_REVIEWDATA, status1));
            }

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }

            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 获取管理合同审核合同信息与开会审核详细信息
        private string LoadManageReviewApp(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            string status1 = getManageReviewStatus(contractNo);//获取合同上一个节点的状态
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(status1))
            {
                sb.Append(string.Format(@"select t1.id, t1.contractNo,
t1.buyerCode, t1.simpleBuyer, t1.buyer, t1.sellerCode, t1.simpleSeller, t1.seller, t1.signedPlace, t1.signedTime,
t1.createman, t1.createdate,
t1.status, t1.salemanCode, t1.isFrame, t1.reviewtime, t1.logisticsTag, t1.frameContract, t1.salesReviewNumber, t1.adminReview, t1.adminReviewNumber,
t1.businessclass, t1.salesmanCode, t1.createmanname, t1.ItemName,t1. ItemAmount, t1.validity, t1.currencyType,t2.meetlog,t2.filepath as upMationDetails,
t2.meettime,t2.isMeetReview as isMeet from {0} t1 
                                         ,{2} t2  where t1.contractNo=t2.contractNo and t1.contractNo='{1}'",
                                       ConstantUtil.TABLE_ECONTRACT_LOGISTICS, contractNo, ConstantUtil.TABLE_REVIEWDATA));

            }
            else
            {
                sb.Append(string.Format(@" select  t1.id, t1.contractNo,
t1.buyerCode, t1.simpleBuyer, t1.buyer, t1.sellerCode, t1.simpleSeller, t1.seller, t1.signedPlace, t1.signedTime,
t1.createman, t1.createdate,
t1.status, t1.salemanCode, t1.isFrame, t1.reviewtime, t1.logisticsTag, t1.frameContract, t1.salesReviewNumber, t1.adminReview, t1.adminReviewNumber,
t1.businessclass, t1.salesmanCode, t1.createmanname, t1.ItemName,t1. ItemAmount, t1.validity, t1.currencyType,t2.meetlog,t2.filepath as upMationDetails,t2.meettime,t2.isMeetReview as isMeet
   from (select * from {0} where contractNo='{1}') t1  left join
{2} t2  on t1.contractNo=t2.contractNo  and t2.reviewstatus='{3}'
",
                                                       ConstantUtil.TABLE_ECONTRACT_LOGISTICS, contractNo, ConstantUtil.TABLE_REVIEWDATA, status1));
            }

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }

            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 获取管理合同审核列表
        private string reviewManageContract(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string isDesk = context.Request["isDesk"];
            sort = "status2 asc,createdate desc";
            order = "";
            return bll.GetManageReviewContractList(row, page, order, sort, isDesk);
        }
        
        #endregion
        #endregion

        #region 获取列表加载信息
        #region 获取商检合同信息
        private string LoadInspectDetails(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(@"select t1.*,t2.revMan,t2.sendMan,t2.sendFactory as sendFactory1,t2.previewCode,t2.facPreviewCode from Econtract t1 left join InspectionCheck t2 on t1.contractNo=t2.contractNo   where t1.contractNo='{0}'",
                                      contractNo));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }

            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 加载服务合同框架子合同信息
        private string LoadServiceAttachData(ref string err, HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            contractBLL bll = new contractBLL();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"select id, contractNo as frameContractNo, buyerCode, simpleBuyer, buyer, 
sellerCode, simpleSeller, seller, signedPlace, signedTime,  createman, 
status, salemanCode, reviewtime, serviceTag, frameContract, salesReviewNumber, adminReview, 
adminReviewNumber, businessclass, salesmanCode, createmanname, simplePartyC, partyC, simplePartyD, partyD, 
partyCCode, partyDCode from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT_SERVICE, contractNo));
            SqlParameter[] pms = new SqlParameter[]{
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }

        #endregion

        #region 加载服务合同关联信息
        private string LoadServiceContactData(ref string err, HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            contractBLL bll = new contractBLL();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"select outsideText,outsideContractNo, id, buyerCode, simpleBuyer, buyer, 
sellerCode, simpleSeller, seller, signedPlace, signedTime,  createman, 
status, salemanCode, reviewtime, serviceTag, frameContract, salesReviewNumber, adminReview, 
adminReviewNumber, businessclass, salesmanCode, createmanname, simplePartyC, partyC, simplePartyD, partyD, 
partyCCode, partyDCode from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT_SERVICE, contractNo));
            SqlParameter[] pms = new SqlParameter[]{
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 加载服务合同信息
        private string LoadServiceData(ref string err, HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            contractBLL bll = new contractBLL();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"select outsideText,outsideContractNo, contractNo, frameContractNo,associateCode, buyerCode, simpleBuyer,
buyer, sellerCode, simpleSeller, seller, signedPlace, signedTime, createman, createdate, 
status, salemanCode, isFrame, reviewtime, serviceTag, frameContract, salesReviewNumber, adminReview, 
adminReviewNumber, businessclass, salesmanCode, createmanname, simplePartyC, partyC, simplePartyD, partyD,
partyCCode, partyDCode, validity from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT_SERVICE, contractNo));
            SqlParameter[] pms = new SqlParameter[]{
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }

        #endregion

        #region 获取进境发货通知修改数据
        private string LoadEditSendNotice(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];

            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(@"select t1.contractNo as sendContractNo,t1.frameContractNo as contractNo,t1.noticeTime,t1.sendTime
,t1.simpleSeller,t1.seller,t1.sellercode,t1.simpleBuyer,t1.buyer,t1.buyercode,t1.flowdirection from {0} t1 
                                         where t1.contractNo='{1}'",
                                     ConstantUtil.TABLE_ECONTRACT, contractNo));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }

            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 获取关联合同列表数据
        private string LoadContactApp(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"select contractNo as contactContractNo,frameCotactContractNo, purchaseCode, salesmanCode, templateno, templatename, 
                contracttype, contracttype2, businessclass, language, signedtime,
signedplace, buyer as seller
, buyercode as sellercode, simpleBuyer as simpleSeller, buyeraddress as selleraddress , currency, pricement1, pricement1per,
pricement2, pricement2per, pvalidity, shipment, transport, tradement, tradeShow, harborout, harborarrive, 
harboroutCountry, deliveryPlace, harborclear, placement, validity, remark, status, createman, createdate,
lastmod, lastmoddate, contractAmount, item1Amount, item2Amount, paidAmount, unpaidAmount, supplemental,
productCategory, shippingmark, overspill, splitShipment, frameContract, paystatus, noticestatus, shipmentstatus, 
declarestatus, singlestatus, taxprintstatus, category, factory, stock, flowdirection, inspectionStatus, sendOutStatus,
harboroutarriveCountry, iscustoms, applystatus, isCancel, adminReview, adminReviewNumber, customMan, bookingstatus,
salesReviewNumber,batchRemark,paymentType, shipDate,freight,commission from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }

            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 获取铁路合同列表数据

        private string LoadTrainContactApp(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();

            //            sb.Append(@"select t1.*,t1.contractNo as exchangeContractNo,t1.simpleSeller as exchangeSimpleBuyer, 
            //t1.seller as exchangeBuyer,t1.sellercode as exchangeBuyerCode,t1.selleraddress as exchangeAddress from Econtract t1 
            // where t1.contractNo=@contractNo");
            sb.Append(@"select t1.*,t1.contractNo as exchangeContractNo,t1.simpleSeller as exchangeSimpleBuyer, 
            t1.seller as exchangeBuyer,t1.sellercode as exchangeBuyerCode,t1.selleraddress as exchangeAddress,t2.sendFactory as sendFactoryInspect from Econtract t1 
            left join Econtract_Inspect t2 on t1.contractNo=t2.purchaseCode where t1.contractNo=@contractNo");
            string ss = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "sellercode");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0);
            //DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);

            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 获取合同列表数据
        private string LoadApp(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            string isCopy = context.Request.QueryString["isCopy"] ?? string.Empty;
            string isattachFrame = context.Request.QueryString["isattachFrame"] ?? string.Empty;
            string isFrameAttach = context.Request.QueryString["isFrameAttach"] ?? string.Empty;
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();
            if (isCopy == "true" || isattachFrame == "true" || isFrameAttach == "true")
            {
                sb.Append(string.Format(@"select contractNo as frameCreNo, purchaseCode, salesmanCode, templateno, templatename, 
                contracttype, contracttype2, businessclass, language, seller, sellercode, simpleSeller, signedtime,
signedplace, buyer, buyercode, simpleBuyer, buyeraddress, selleraddress, currency, pricement1, pricement1per,
pricement2, pricement2per, pvalidity, shipment, transport, tradement, tradeShow, harborout, harborarrive, 
harboroutCountry, deliveryPlace, harborclear, placement, validity, remark, status, createman, createdate,
lastmod, lastmoddate, contractAmount, item1Amount, item2Amount, paidAmount, unpaidAmount, supplemental,
productCategory, shippingmark, overspill, splitShipment, paystatus, noticestatus, shipmentstatus, 
declarestatus, singlestatus, taxprintstatus, category, factory, stock, flowdirection, inspectionStatus, sendOutStatus,
harboroutarriveCountry, iscustoms, applystatus, isCancel, adminReview, adminReviewNumber, customMan, bookingstatus,
salesReviewNumber,freight,commission from {0} where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
            }
            else
            {
                sb.Append(string.Format(@"select t1.* from {0} t1 
                                         where t1.contractNo='{1}'",
                                         ConstantUtil.TABLE_ECONTRACT, contractNo));
            }
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }

            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 获取物流合同表格头部数据
        private string LoadLogisticsFirstItem(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            contractBLL bll = new contractBLL();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where logisticsContractNo='{1}'", ConstantUtil.TABLE_ECONTRACT_LOGISTICSFIRSTITEM, contractNo));
            SqlParameter[] pms = new SqlParameter[]{
            };
            DataTable dt = bll.GetDataTableBySql(sb.ToString(), pms);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            return result.ToLower();
        }
        #endregion

        #region 获取物流合同列表数据
        private string LoadLogistics(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            contractBLL bll = new contractBLL();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where logisticsContractNo='{1}'", ConstantUtil.TABLE_ECONTRACT_LOGISTICS, contractNo));
            SqlParameter[] pms = new SqlParameter[]{
            };
            DataTable dt = bll.GetDataTableBySql(sb.ToString(), pms);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            return result;
        }
        #endregion

        #region 根据出口模板编号获取合同表格数据
        private string LoadDataByExportTemp(HttpContext context)
        {
            string templateno = context.Request.QueryString["templateno"];
            if (string.IsNullOrEmpty(templateno))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where templateno='{1}'", ConstantUtil.TABLE_BTEMPLATE_EXPORTENCONTRACT, templateno));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[0] { }, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 根据进口合同模板加载数据
        private string LoadDataByImportTemp(HttpContext context)
        {
            string templateno = context.Request.QueryString["templateno"];
            if (string.IsNullOrEmpty(templateno))
            {
                throw new Exception("传入参数为空");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where templateno='{1}'", ConstantUtil.TABLE_BTEMPLATE_IMPORTENCONTRACT, templateno));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #endregion

        #region 获取节点状态
        #region 获取合同上一个节点的状态
        //获取合同上一个节点的状态
        private string getReviewStatus(string contractNo)
        {
            string sbStatus = string.Empty;
            StringBuilder sb = new StringBuilder(@"select status from Econtract where contractNo=@contractNo");
            string status = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK3)//待业务主管审核
            {
                sbStatus = status;
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK2;//待合同管理员审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK4)//待财务负责人审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK5)//待财务主管审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK4;//待财务负责人审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK6)//待董事长审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK5;//待财务主管审核
            }
            return sbStatus;
        }
        #endregion

        #region 获取服务合同上一个节点的状态
        //获取合同上一个节点的状态
        private string getServiceReviewStatus(string contractNo)
        {
            string sbStatus = string.Empty;
            StringBuilder sb = new StringBuilder(@"select status from " + ConstantUtil.TABLE_ECONTRACT_SERVICE + " where contractNo=@contractNo");
            string status = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK3)//待业务主管审核
            {
                sbStatus = status;
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK2;//待合同管理员审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK4)//待财务负责人审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK5)//待财务主管审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK4;//待财务负责人审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK6)//待董事长审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK5;//待财务主管审核
            }
            return sbStatus;
        }
        #endregion 

        #region 获取管理合同上一个节点的状态
        //获取合同上一个节点的状态
        private string getManageReviewStatus(string contractNo)
        {
            string sbStatus = string.Empty;
            StringBuilder sb = new StringBuilder(@"select status from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " where contractNo=@contractNo");
            string status = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK3)//待业务主管审核
            {
                sbStatus = status;
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK2;//待合同管理员审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK4)//待财务负责人审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK5)//待财务主管审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK4;//待财务负责人审核
            }
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK6)//待董事长审核
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务主管审核
                //sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK5;//待财务主管审核
            }
            return sbStatus;
        }
        #endregion 
        #endregion

        #region 其他

        #region 获取业务员退回的合同

        private string getBackContract(HttpContext context)
        {
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            sqldata.AppendFormat(@" select t1.*  from " + ConstantUtil.TABLE_ECONTRACT + " t1  where t1.createman=@createman and t1.status='{0}'" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK7);//退回
            sqlcount.AppendFormat(@" select count(1) from " + ConstantUtil.TABLE_ECONTRACT + " t1 where t1.createman=@createman and t1.status='{0}'" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK7);
            SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@createman",createman),
             
            };
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();

        }
        #endregion

        #region 上传文件
        private string uploadFile(ref string err, HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                var file = context.Request.Files[0];
                string path = "/Files/meetReview/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + file.FileName;
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

        #region 实现接口
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion 
        #endregion
    }
}