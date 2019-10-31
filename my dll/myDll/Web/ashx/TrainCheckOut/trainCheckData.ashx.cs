using RM.Busines;
using RM.Busines.contract;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.TrainCheckOut
{
    /// <summary>
    /// trainCheckData 的摘要说明
    /// </summary>
    public class trainCheckData : IHttpHandler, IRequiresSessionState
    {
        RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request["module"];
            switch (module)
            {
                case "ckpagelist":

                    context.Response.Write(getlist(context));
                    break;
                case "getTrainApplyList":
                    context.Response.Write(getTrainApplyList(context));
                    break;
                case "getTrainCheckedList"://铁路发货通知列表
                    context.Response.Write(getTrainCheckedList(context));
                    break;
                case "getTrainCheckedPrintList"://铁路运单打印列表
                    context.Response.Write(getTrainCheckedPrintList(context));
                    break;
                case "LoadSendOutData"://获取发货申请列表数据填充发货通知表单
                    context.Response.Write(LoadSendOutData(context));
                    break;
                case "LoadSendOutDataByEdit"://修改时获取发货申请列表数据填充发货通知表单
                    context.Response.Write(LoadSendOutDataByEdit(context));
                    break;
                case "getFinishApply":
                    context.Response.Write(getFinishApply(context));
                    break;
                //获取货物产品列表
                case "trainProductByContractNo":
                    context.Response.Write(trainProductByContractNo(context));
                    break;
                //获取国境口岸站列表
                case "checkFrontierStation":

                    context.Response.Write(checkFrontierStation(context));
                    break;
                //获取付费代码和过境代码列表
                case "checkPayCode":
                    context.Response.Write(checkPayCode(context));
                    break;
                //添加付费代码的打印次数
                case "addPrintCount":
                    context.Response.Write(addPrintCount(context));
                    break;

            }
        }


        private string LoadSendOutDataByEdit(HttpContext context)
        {
            string createDateTag = context.Request.QueryString["createDateTag"];
            string noticeTag = context.Request.QueryString["noticeTag"];
            if (string.IsNullOrEmpty(createDateTag))
            {
                throw new Exception("传入参数为空");
            }
            contractBLL bll = new contractBLL();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select shipperReportEng, shipperReport,carrierReport,carrierReportEng,destinationRemark, createDateTag, contractNo, contactContract, buyer, seller, buyercode, sellercode, 
contactbuyer, contactseller, contactbuyercode, contactsellercode,isConsignor, 
isConsignee, isContactConsignor, isContactConsignee, fromStation, fromStationCode, destination, 
destinationCode, carriageCount, containerSize, countryChn,countryRus,palletRequireRus,
createman, createdate, salesman, blockTrain , t1.fromStation as harborout,t1.destination as harborarrive,packagesNumber,palletRequire
from trainDeliveryNotice t1 where noticeTag=@noticeTag");
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@createDateTag",createDateTag),
                new SqlParameter("@noticeTag",noticeTag)
            };
            DataTable dt = bll.GetDataTableBySql(sb.ToString(), pms);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }

        public string LoadSendOutData(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"] ?? string.Empty;
            string createDateTag = context.Request.QueryString["createDateTag"] ?? string.Empty;
            if (string.IsNullOrEmpty(contractNo))
            {
                throw new Exception("传入参数为空");
            }
            contractBLL bll = new contractBLL();
            StringBuilder sb = new StringBuilder();
            //根据合同号判断其关联合同号是否为空
            StringBuilder sv = new StringBuilder(@"select * from Econtract where purchaseCode=@contractNo");
            string purchaseCode = DataFactory.SqlDataBase().getString(sv, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "contractNo");
            //存在关联合同号，根据关联合同号加载发货人收货人
            if (!string.IsNullOrEmpty(purchaseCode))
            {
                sb.Append(@"select t5.rsaddress as revAddress,t5.rsname as revName,t6.rsaddress as sendAddress,t6.rsname as sendName, t3.code as harboroutcode,t4.code as harborarrivecode, t1.salesmanCode as salesman,t1.contractNo,t2.contractNo as contactContract,t1.seller,t1.buyer,t1.sellercode,t1.buyercode
,t2.seller as contactseller,t2.buyer as contactbuyer,t2.buyercode as contactbuyercode,t2.sellercode as contactsellercode,
t2.seller as sendMan,t1.buyer as revMan,
t1.harborout,t1.harborarrive
 from Econtract t1 left join Econtract t2 on t1.contractNo=t2.purchaseCode left join bharbor t3  on t1.harborout=t3.name
left join bharbor t4 on t1.harborarrive=t4.name
left join bcustomer t5 on t5.name=t1.buyer
left join bsupplier t6 on t6.name=t2.seller
where t2.applicationNo=@createDateTag");
            }
            else
            {
                sb.Append(@"select t5.rsaddress as revAddress,t5.rsname as revName,t6.rsaddress as sendAddress,t6.rsname as sendName,
 t1.salesmanCode as salesman,t1.contractNo,t1.seller,t1.buyer,t1.sellercode,t1.buyercode,
 t1.buyer as revMan,t1.seller as sendMan,t1.harborout,t1.harborarrive from Econtract t1
 left join bcustomer t5 on t5.name=t1.buyer
 left join bsupplier t6 on t6.name=t1.seller
where t1.contractNo=@contractNo");
            }
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@contractNo",contractNo),
                new SqlParameter("@createDateTag",createDateTag)
            };
            DataTable dt = bll.GetDataTableBySql(sb.ToString(), pms);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }

        //获取已完成发货申请的铁路运输合同
        private string getFinishApply(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string saleContract = (context.Request["saleContract"] ?? "").ToString().Trim();
            string businessGroup = (context.Request["businessGroup"] ?? "").ToString().Trim();
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            //SqlWhere.Append("1=1");
            if (saleContract.Length > 0)
            {
                SqlWhere.Append(" and  saleContract like '%" + saleContract + "%'");
            }
            if (businessGroup.Length > 0)
            {
                SqlWhere.Append(" and  businessGroup like '%" + businessGroup + "%'");
            }
            //sqldata 为连接字符串
            StringBuilder sqldata = new StringBuilder();
            //sqlcount为数据个数
            StringBuilder sqlcount = new StringBuilder();
            sqldata.AppendFormat(@" select t1.* from Econtract t1 where t1.sendOutStatus='2'and t1.transport='铁路'"
);
            sqlcount.AppendFormat(@"select count(1) from Econtract t1 where t1.sendOutStatus='2'and t1.transport='铁路'");
            SqlParameter[] sqlpps = new SqlParameter[]{
            };
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        //获取铁路待发货通知列表
        private string getTrainApplyList(HttpContext context)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string review = context.Request["review"];
            string flowdirection = context.Request["flowdirection"];
            string transport = context.Request["transport"];
            string isPrint = context.Request["isPrint"]??"";
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t2.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t2.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t2.signedtime<=@signedtime_end ");
            }
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t2.flowdirection=@flowdirection ");
            }
            if (isPrint != null && isPrint == "是")
            {
                sqlshere.Append(" and sendstatus='1'");
            }
            sqldata.AppendFormat(@"select * from (select t3.blockTrain,t3.noticeTag,t3.savestatus,t.* from
(select t2.*,t1.contactStatus ,t1.applystatus as sendstatus, t1.createDateTag,t1.pcode,t1.pname,t1.spec,
t1.quantity,t1.sendQuantity,t1.qunit,t1.packdes,t1.unit,t1.packspec 
from {0} t1,{1} t2   where t1.contractNo=t2.contractNo and t2.transport=@transport
and (t1.contactStatus='{2}' or t1.contactStatus='{3}' ) and t2.createman=@createman
) t left join {4} t3 on t3.createDateTag=t.createDateTag) t4 where 1=1 "
                + sqlshere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT,
                ConstantUtil.STATUS_CONTACT_ED, ConstantUtil.STATUS_CONTACT_SEND, ConstantUtil.TABLE_TRAINDELIVERYNOTICE);
            sqlcount.AppendFormat(@"select count(1) from
(select t2.*,t1.contactStatus ,t1.applystatus as sendstatus, t1.createDateTag,t1.pcode,t1.pname,t1.spec,
t1.quantity,t1.sendQuantity,t1.qunit,t1.packdes,t1.unit,t1.packspec 
from {0} t1,{1} t2   where t1.contractNo=t2.contractNo and t2.transport=@transport
and (t1.contactStatus='{2}' or t1.contactStatus='{3}')  and t2.createman=@createman
) t left join {4} t3 on t3.createDateTag=t.createDateTag"
                + sqlshere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT,
                ConstantUtil.STATUS_CONTACT_ED, ConstantUtil.STATUS_CONTACT_SEND, ConstantUtil.TABLE_TRAINDELIVERYNOTICE);
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                    new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                    new SqlParameter{ParameterName="@transport",Value=transport,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String},
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }

        //获取铁路发货通知列表
        private string getTrainCheckedList(HttpContext context)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string buyer = context.Request["buyer"] ?? "";
            string seller = context.Request["seller"] ?? "";
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and contractNo like '%'+@contractNo+'%' ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and buyer like '%'+@buyer+'%' ");
            }
            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and seller like '%'+@seller+'%' ");
            }
            sqlshere.Append(" and (createman = @createman or adminReviewNumber=@createman) ");
            sqlshere.Append(" and saveStatus='1'");//显示已通知完成的数据
            sqldata.AppendFormat(@"select * from {0} where 1=1 "
                + sqlshere.ToString(), ConstantUtil.VIEW_TRAINNOTICE);
            sqlcount.AppendFormat(@"select count(1) from {0} where 1=1 "
                + sqlshere.ToString(), ConstantUtil.VIEW_TRAINNOTICE);
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String},
                    new SqlParameter{ParameterName="@seller",Value=seller,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String}
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        //获取运单打印列表
        private string getTrainCheckedPrintList(HttpContext context)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string buyer = context.Request["buyer"] ?? "";
            string seller = context.Request["seller"] ?? "";
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and contractNo like '%'+@contractNo+'%' ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and buyer like '%'+@buyer+'%' ");
            }
            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and seller like '%'+@seller+'%' ");
            }
            sqlshere.Append(" and (createman = @createman or adminreview=@createman) ");
            sqldata.AppendFormat(@"select * from (select a.*,b.adminreview from {0} a left join {1} b on a.contractNo=b.contractNo) t where 1=1 "
                + sqlshere.ToString(), ConstantUtil.TABLE_TRAINDELIVERYNOTICE, ConstantUtil.TABLE_ECONTRACT);
            sqlcount.AppendFormat(@"select * from (select a.*,b.adminreview from {0} a left join {1} b on a.contractNo=b.contractNo) t where 1=1 "
                + sqlshere.ToString(), ConstantUtil.TABLE_TRAINDELIVERYNOTICE, ConstantUtil.TABLE_ECONTRACT);
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String},
                    new SqlParameter{ParameterName="@seller",Value=seller,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String}
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }

        //加载铁路发货通知列表
        private string getlist(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            int count = 0;

            string saleContract = (context.Request["saleContract"] ?? "").ToString().Trim();
            string businessGroup = (context.Request["businessGroup"] ?? "").ToString().Trim();
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append("1=1");
            if (saleContract.Length > 0)
            {
                SqlWhere.Append(" and  saleContract like '%" + saleContract + "%'");
            }
            if (businessGroup.Length > 0)
            {
                SqlWhere.Append(" and  businessGroup like '%" + businessGroup + "%'");
            }
            //sqldata 为连接字符串
            StringBuilder sqldata = new StringBuilder();
            //sqlcount为数据个数
            StringBuilder sqlcount = new StringBuilder();

            sqldata.Append(@" select * from checkoutReport  where " + SqlWhere.ToString());

            sqlcount.Append("select count(1) from checkoutReport t1 where " + SqlWhere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[]{
               
            };
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }

        //获取付费代码和过境代码列表
        private string checkPayCode(HttpContext context)
        {
            string createDateTag = context.Request.Params["createDateTag"];
            string noticeTag = context.Request.Params["noticeTag"];
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from trainDelPayCode where createDateTag=@createDateTag and noticeTag=@noticeTag ");
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@createDateTag",createDateTag),
                new SqlParameter("@noticeTag",noticeTag)
            };
            RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
            return jsonh.GetDatatableJsonString(sb, pms).ToString();
        }
        //获取国境口岸站列表
        private string checkFrontierStation(HttpContext context)
        {
            string createDateTag = context.Request.Params["createDateTag"];
            string noticeTag = context.Request.Params["noticeTag"];
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from trainDelFroStation where noticeTag=@noticeTag");
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@noticeTag",noticeTag)
            };
            RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
            return jsonh.GetDatatableJsonString(sb, pms).ToString();
        }

        //获取货物产品列表
        private string trainProduct(HttpContext context)
        {
            string checkId = context.Request.Params["checkId"];
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from checkoutProduct where checkId=@checkId");
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@checkId",checkId)
            };
            RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
            return jsonh.GetDatatableJsonString(sb, pms).ToString();

        }
        private string trainProductByContractNo(HttpContext context)
        {
            string createDateTag = context.Request.Params["createDateTag"];
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from {0} where createDateTag=@createDateTag", ConstantUtil.TABLE_SENDOUTAPPDETAILS);
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@createDateTag",createDateTag)
            };
            RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
            return jsonh.GetDatatableJsonString(sb, pms).ToString();
        }
        //添加付费代码的打印次数
        private int addPrintCount(HttpContext context)
        {
            string paycode = context.Request.Params["paycode"];
            StringBuilder sb = new StringBuilder();
            sb.Append("update trainDelPayCode set printCount = printCount+1  where paycode=@paycode");
            RM.Common.DotNetCode.SqlParam[] pms = new RM.Common.DotNetCode.SqlParam[]{
                new SqlParam("@paycode",paycode)
            };
            int result = DataFactory.SqlDataBase().ExecuteBySql(sb, pms);
            return result;
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