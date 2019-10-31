using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WZX.Busines.Util;
using RM.Common.DotNetJson;
using System.Collections;
using RM.Common.DotNetCode;
using RM.Busines.Busines;
using RM.Busines;
using System.Web.SessionState;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Common.DotNetEamil;
using System.IO;

namespace RM.Web.ashx.CheckNotice
{
    /// <summary>
    /// CheckData 的摘要说明
    /// </summary>
    public class CheckData : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request["module"] == null ? "" : context.Request["module"].ToString();
            switch (module)
            {
                case "bookingRequestList"://加载海运订舱申请列表
                    context.Response.Write(bookingRequestList(context));
                    break;
                case "getAddSubList"://获取添加订舱申请时的子表信息
                    context.Response.Write(getAddSubList(context));
                    break;
                case "getCheckNoticeSubList"://获取添加订舱申请时的子表信息
                    context.Response.Write(getCheckNoticeSubList(context));
                    break;
                case "saveNotice"://添加订舱申请
                    context.Response.Write(saveNotice(context));
                    break;
                case "submitNotice"://添加订舱申请
                    context.Response.Write(submitNotice(context));
                    break;
                case "receiveSubmitApply"://接收提交订舱申请（分配订舱申请）
                    context.Response.Write(receiveSubmitApply(context));
                    break;
                case "backApply"://退回订舱申请
                    context.Response.Write(backApply(context));
                    break;
                case "backConfirm"://退回订舱申请至分配节点
                    context.Response.Write(backConfirm(context));
                    break;
                case "receiveApply"://确认节点接收订舱申请
                    context.Response.Write(receiveApply(context));
                    break;
                case "updateNotice"://更新订舱通知
                    context.Response.Write(updateNotice(context));
                    break;
                case "getCheckNoticeDList"://获取发运通知明细表
                    context.Response.Write(getCheckNoticeDList(context));
                    break;
                case "getCheckNoticeDList_WH"://获取发运通知明细表+库存信息
                    context.Response.Write(getCheckNoticeDList_WH(context));
                    break;
                case "getWarehouse"://获取仓库列表
                    context.Response.Write(getWarehouse(context));
                    break;
                case "addStockOut"://海运发货通知
                    context.Response.Write(addStockOut(context));
                    break;
                case "upload"://上传
                    context.Response.Write(uploadFile(context));
                    break;

                default://默认
                    context.Response.Write("");
                    break;
            }
        }
        //加载海运订舱申请列表
        private string bookingRequestList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"]?? "";
            string createDateTag = context.Request.Params["createDateTag"] ?? "";
            string buyer = context.Request.Params["buyer"]?? "";
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder();
            if (!string.IsNullOrEmpty(contractNo))
            {
                sqlwhere.Append("and contractNo=@contractNo ");
            }
            if (!string.IsNullOrEmpty(createDateTag))
            {
                sqlwhere.Append("and createDateTag like '%'+@createDateTag+'%'  ");
            }
            if (!string.IsNullOrEmpty(buyer))
            {
                sqlwhere.Append("and buyer like '%'+@buyer+'%' ");
            }

            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createDateTag",Value=createDateTag,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String}
                };
            //加载发货申请已完成，并且运输方式为海运的合同数据
            /*
            sqldata.AppendFormat(@"select b.checkNoticeNumber,b.noticeStatus as DCApplyStatus,b.distributeStatus,b.confirmStatus,b.sendOutNoticeStatus,b.distributeMan,b.distributeDatetime,b.createman as applyman,b.createtime as applydate, tt.* from ( select * from (select a.createDateTag,a.pcode,a.pname,a.quantity as sendQuantity,a.qunit,a.price,a.priceUnit,a.amount,a.packspec,a.packing,a.spec,a.pallet,a.packdes,a.unit,a.checkNoticeStatus,a.isDC,b.* from {0} a,{1} b where a.contractNo = b.contractNo  and transport <> '铁路' and isDC in ('中泰订舱','客户订舱') ) t where 1=1 "
                    + sqlwhere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT)
                .Append(") tt left join " + ConstantUtil.TABLE_CHECKOUTNOTICE + " b on tt.createDateTag = b.contractCreateDateTag  where tt.createman='" + RequestSession.GetSessionUser().UserAccount.ToString() + "'");
            sqlcount.AppendFormat("select count(1) from (select b.checkNoticeNumber,b.noticeStatus as DCApplyStatus,b.distributeStatus,b.confirmStatus,b.sendOutNoticeStatus,b.distributeMan,b.distributeDatetime,b.createman as applyman,b.createtime as applydate, tt.* from ( select * from (select a.createDateTag,a.pcode,a.pname,a.quantity as sendQuantity,a.qunit,a.price,a.priceUnit,a.amount,a.packspec,a.packing,a.spec,a.pallet,a.packdes,a.unit,a.checkNoticeStatus,a.isDC,b.* from {0} a,{1} b where a.contractNo = b.contractNo  and transport <> '铁路' and isDC in ('中泰订舱','客户订舱') ) t where 1=1"
                + sqlwhere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT)
            .Append(") tt left join " + ConstantUtil.TABLE_CHECKOUTNOTICE + " b on tt.createDateTag = b.contractCreateDateTag  where tt.createman='" + RequestSession.GetSessionUser().UserAccount.ToString() + "')ttt");
            */
            sqldata.AppendFormat(@"select b.checkNoticeNumber,b.noticeStatus as DCApplyStatus,b.distributeStatus,b.confirmStatus,b.sendOutNoticeStatus,b.distributeMan,b.distributeDatetime,b.createman as applyman,b.createtime as applydate, tt.* from ( select * from (select createDateTag, sendQuantity,checkNoticeStatus,isDC,b.* from ( select contractNo,createDateTag,productCategory,sum(quantity) as sendQuantity,checkNoticeStatus,isDC from {0} group by contractNo,createDateTag,productCategory,checkNoticeStatus,isDC ) a, {1} b where a.contractNo = b.contractNo  and transport <> '铁路' and isDC in ('中泰订舱','客户订舱') ) t where 1=1 "
                    + sqlwhere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT)
                .Append(") tt left join " + ConstantUtil.TABLE_CHECKOUTNOTICE + " b on tt.createDateTag = b.contractCreateDateTag  where tt.createman='" + RequestSession.GetSessionUser().UserAccount.ToString() + "'");
            sqlcount.AppendFormat("select count(1) from (select b.checkNoticeNumber,b.noticeStatus as DCApplyStatus,b.distributeStatus,b.confirmStatus,b.sendOutNoticeStatus,b.distributeMan,b.distributeDatetime,b.createman as applyman,b.createtime as applydate, tt.* from ( select * from (select createDateTag, sendQuantity,checkNoticeStatus,isDC,b.* from ( select contractNo,createDateTag,productCategory,sum(quantity) as sendQuantity,checkNoticeStatus,isDC from {0} group by contractNo,createDateTag,productCategory,checkNoticeStatus,isDC ) a,{1} b where a.contractNo = b.contractNo  and transport <> '铁路' and isDC in ('中泰订舱','客户订舱') ) t where 1=1 "
                    + sqlwhere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT)
            .Append(") tt left join " + ConstantUtil.TABLE_CHECKOUTNOTICE + " b on tt.createDateTag = b.contractCreateDateTag  where tt.createman='" + RequestSession.GetSessionUser().UserAccount.ToString() + "')ttt");

            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, "DCApplyStatus asc,applydate desc", page, row);
            return sb.ToString();
        }


        //获取添加订舱申请时的子表信息
        private string getAddSubList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"]?? "";
            string createDateTag = context.Request.Params["createDateTag"]?? "";
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder();
            sqlwhere.Append("and contractNo=@contractNo and createDateTag=@createDateTag ");

            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createDateTag",Value=createDateTag,DbType=DbType.String}
                };
            sqldata.AppendFormat(@"select contractNo, pcode,pname,spec,qunit,packing,sendQuantity as quantity,sendQuantity as weight from {0}  where 1=1 " + sqlwhere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS);
            sqlcount.AppendFormat("select count(1) from {0} where 1=1 " + sqlwhere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS);
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);
            return sb.ToString();
        }
        //获取订舱申请时的子表信息
        private string getCheckNoticeSubList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string checkNoticeNumber = context.Request.Params["checkNoticeNumber"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder();
            if (!string.IsNullOrEmpty(checkNoticeNumber))
            {
                sqlwhere.Append("and checkNoticeNumber=@checkNoticeNumber ");
            }

            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@checkNoticeNumber",Value=checkNoticeNumber,DbType=DbType.String}
                };
            sqldata.AppendFormat(@"select * from {0}  where 1=1 " + sqlwhere.ToString(), ConstantUtil.TABLE_CHECKOUTNOTICE_D);
            sqlcount.AppendFormat("select count(1) from {0} where 1=1 " + sqlwhere.ToString(), ConstantUtil.TABLE_CHECKOUTNOTICE_D);
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);
            return sb.ToString();
        }

        //添加订舱
        private string saveNotice(HttpContext context) 
        {
            Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            string checkNoticeNumber = (context.Request["checkNoticeNumber"] ?? "").ToString().Trim();
            string contractNo = (context.Request["saleContract"] ?? "").ToString().Trim();
            string createDateTag = (context.Request["createDateTag"] ?? "").ToString().Trim();
            string seller = (context.Request["seller"] ?? "").ToString().Trim();
            string consignor = (context.Request["consignor"] ?? "").ToString().Trim();
            string consignorAddress = (context.Request["consignorAddress"] ?? "").ToString().Trim();
            string consignorPhone = (context.Request["consignorPhone"] ?? "").ToString().Trim();
            string buyer = (context.Request["buyer"] ?? "").ToString().Trim();
            string consignee = (context.Request["consignee"] ?? "").ToString().Trim();
            string consigneeAddress = (context.Request["consigneeAddress"] ?? "").ToString().Trim();
            string consigneePhone = (context.Request["consigneePhone"] ?? "").ToString().Trim();
            string noticeMan1 = (context.Request["noticeMan1"] ?? "").ToString().Trim();
            string noticeManAddress1 = (context.Request["noticeManAddress1"] ?? "").ToString().Trim();
            string noticeManPhone1 = (context.Request["noticeManPhon1e"] ?? "").ToString().Trim();
            string noticeMan2 = (context.Request["noticeMan2"] ?? "").ToString().Trim();
            string noticeManAddress2 = (context.Request["noticeManAddress2"] ?? "").ToString().Trim();
            string noticeManPhone2 = (context.Request["noticeManPhone2"] ?? "").ToString().Trim();
            string boxCount = (context.Request["boxCount"] ?? "").ToString().Trim();
            string shipClause = (context.Request["shipClause"] ?? "").ToString().Trim();
            string deliveryPlace = (context.Request["deliveryPlace"] ?? "").ToString().Trim();
            string departurePort = (context.Request["departurePort"] ?? "").ToString().Trim();
            string departurePort_en = (context.Request["departurePort_en"] ?? "").ToString().Trim();
            string departurePortCountry = (context.Request["departurePortCountry"] ?? "").ToString().Trim();
            string departurePortCountry_en = (context.Request["departurePortCountry_en"] ?? "").ToString().Trim();
            string unloadPort = (context.Request["unloadPort"] ?? "").ToString().Trim();
            string unloadPort_en = (context.Request["unloadPort_en"] ?? "").ToString().Trim();
            string unloadPortCountry = (context.Request["unloadPortCountry"] ?? "").ToString().Trim();
            string unloadPortCountry_en = (context.Request["unloadPortCountry_en"] ?? "").ToString().Trim();
            string shippngcostItem = (context.Request["shippngcostItem"] ?? "").ToString().Trim();
            string boxCountCFS = (context.Request["boxCountCFS"] ?? "0").ToString().Trim();
            string boxCountHC = (context.Request["boxCountHC"] ?? "0").ToString().Trim();
            string boxCountGP4 = (context.Request["boxCountGP4"] ?? "0").ToString().Trim();
            string boxCountGP2 = (context.Request["boxCountGP2"] ?? "0").ToString().Trim();
            string preShipDate = (context.Request["preShipDate"] ?? "").ToString().Trim();
            string preShipCompanyName = (context.Request["preShipCompanyName"] ?? "").ToString().Trim();
            string mark = (context.Request["mark"] ?? "").ToString().Trim();

            //订舱信息
            string applyType = (context.Request["applyType"] ?? "").ToString().Trim();
            string billNumbers = (context.Request["billNumbers"] ?? "").ToString().Trim();
            string shipdate = (context.Request["shipdate"] ?? "").ToString().Trim();
            string shipname = (context.Request["shipname"] ?? "").ToString().Trim();
            string shipnumber = (context.Request["shipnumber"] ?? "").ToString().Trim();
            string containerCount = (context.Request["containerCount"] ?? "").ToString().Trim();
            string wharf = (context.Request["wharf"] ?? "").ToString().Trim();
            string costbear = (context.Request["costbear"] ?? "").ToString().Trim();
            string isDT = (context.Request["isDT"] ?? "").ToString().Trim();
            string isTM = (context.Request["isTM"] ?? "").ToString().Trim();
            string isCM = (context.Request["isCM"] ?? "").ToString().Trim();
            string isTZMD = (context.Request["isTZMD"] ?? "").ToString().Trim();
            string isTDD = (context.Request["isTDD"] ?? "").ToString().Trim();
            string shipcompanyname = (context.Request["shipcompanyname"] ?? "").ToString().Trim();
            string cutoffdate = (context.Request["cutoffdate"] ?? "").ToString().Trim();
            string boxorderName = (context.Request["boxorderName"] ?? "").ToString().Trim();
            string boxorderUrl = (context.Request["boxorderUrl"] ?? "").ToString().Trim();
            string bhName = (context.Request["bhName"] ?? "").ToString().Trim();
            string bhUrl = (context.Request["bhUrl"] ?? "").ToString().Trim();
            string noticeorderName = (context.Request["noticeorderName"] ?? "").ToString().Trim();
            string noticeorderUrl = (context.Request["noticeorderUrl"] ?? "").ToString().Trim();

            string shipProduct = context.Request["shipProduct"] ?? "";
            //判断是否已申请
            /*取消校验
            StringBuilder isApplied_sql = new StringBuilder();
            isApplied_sql.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE)
                .Append(" where saleContract='" + contractNo + "' and contractCreateDateTag='" + createDateTag + "'");
            DataTable dt_isApplied = DataFactory.SqlDataBase().GetDataTableBySQL(isApplied_sql);
            if (DataTableHelper.IsExistRows(dt_isApplied) && (dt_isApplied.Rows[0]["NOTICESTATUS"].ToString()) != ConstantUtil.STATUS_HY_BACK)
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "该发货申请已经申请过订舱信息！");
                
            }
             * */
            if(1==2)
            {

            }
            else 
            {
                /*
                if (DataTableHelper.IsExistRows(dt_isApplied)) 
                {
                    checkNoticeNumber = dt_isApplied.Rows[0]["CHECKNOTICENUMBER"].ToString();
                }
                 * */
                #region 主数据
                //1、组织数据
                if (string.IsNullOrEmpty(checkNoticeNumber))
                {
                    string sequenceKey = "HYDC" + DateTimeHelper.GetToday("yyyyMM");
                    checkNoticeNumber = sequenceKey + Sequence.getSequenceAutoAddZero(sequenceKey, 4);
                }
                Hashtable ht_main = new Hashtable();
                ht_main.Add("checkNoticeNumber", checkNoticeNumber);
                ht_main.Add("seller", seller);
                ht_main.Add("consignor", consignor);
                ht_main.Add("saleContract", contractNo);
                ht_main.Add("contractCreateDateTag", createDateTag);
                ht_main.Add("consignorAddress", consignorAddress);
                ht_main.Add("consignorPhone", consignorPhone);
                ht_main.Add("buyer", buyer);
                ht_main.Add("consignee", consignee);
                ht_main.Add("consigneeAddress", consigneeAddress);
                ht_main.Add("consigneePhone", consigneePhone);
                ht_main.Add("noticeMan1", noticeMan1);
                ht_main.Add("noticeManAddress1", noticeManAddress1);
                ht_main.Add("noticeManPhone1", noticeManPhone1);
                ht_main.Add("noticeMan2", noticeMan2);
                ht_main.Add("noticeManAddress2", noticeManAddress2);
                ht_main.Add("noticeManPhone2", noticeManPhone2);
                ht_main.Add("boxCount", boxCount);
                ht_main.Add("shipClause", shipClause);
                ht_main.Add("deliveryPlace", deliveryPlace);
                ht_main.Add("departurePort", departurePort);
                ht_main.Add("departurePort_en", departurePort_en);
                ht_main.Add("departurePortCountry", departurePortCountry);
                ht_main.Add("departurePortCountry_en", departurePortCountry_en);
                ht_main.Add("unloadPort", unloadPort);
                ht_main.Add("unloadPort_en", unloadPort_en);
                ht_main.Add("unloadPortCountry", unloadPortCountry);
                ht_main.Add("unloadPortCountry_en", unloadPortCountry_en);
                ht_main.Add("shippngcostItem", shippngcostItem);
                ht_main.Add("boxCountCFS", boxCountCFS);
                ht_main.Add("boxCountHC", boxCountHC);
                ht_main.Add("boxCountGP4", boxCountGP4);
                ht_main.Add("boxCountGP2", boxCountGP2);
                ht_main.Add("preShipDate", preShipDate);
                ht_main.Add("preShipCompanyName", preShipCompanyName);
                ht_main.Add("mark", mark);
                ht_main.Add("containerCount", int.Parse(boxCountCFS) + int.Parse(boxCountHC) + int.Parse(boxCountGP4) + int.Parse(boxCountGP2));
                ht_main.Add("noticeStatus", ConstantUtil.STATUS_HY_APPLY_ED);
                
                ht_main.Add("sendOutNoticeStatus", ConstantUtil.STATUS_SENDOUTNOTICE_UN);
                ht_main.Add("createman", RequestSession.GetSessionUser().UserName.ToString());
                ht_main.Add("createtime", DateTimeHelper.ShortDateTimeS);
                 
                if (applyType=="客户订舱")
                {
                    ht_main.Add("billNumbers", billNumbers);
                    ht_main.Add("shipdate", shipdate);
                    ht_main.Add("shipname", shipname);
                    ht_main.Add("shipnumber", shipnumber);
                    ht_main.Add("wharf", wharf);
                    ht_main.Add("costbear", costbear);
                    ht_main.Add("isDT", isDT);
                    ht_main.Add("isTM", isTM);
                    ht_main.Add("isCM", isCM);
                    ht_main.Add("isTZMD", isTZMD);
                    ht_main.Add("isTDD", isTDD);
                    ht_main.Add("shipcompanyname", shipcompanyname);
                    ht_main.Add("cutoffdate", cutoffdate);
                    ht_main.Add("boxorderName", boxorderName);
                    ht_main.Add("boxorderUrl", boxorderUrl);
                    ht_main.Add("bhName", bhName);
                    ht_main.Add("bhUrl", bhUrl);
                    ht_main.Add("noticeorderName", noticeorderName);
                    ht_main.Add("noticeorderUrl", noticeorderUrl);
                    ht_main.Add("confirmStatus", ConstantUtil.STATUS_CONFIRM_ED);
                    ht_main.Add("distributeStatus", ConstantUtil.STATUS_DISTRIBUTE_ED);
                }
                else 
                {
                    ht_main.Add("confirmStatus", ConstantUtil.STATUS_CONFIRM_UN);
                    ht_main.Add("distributeStatus", ConstantUtil.STATUS_DISTRIBUTE_UN);
                }
                //2、生成主表sql
                SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_CHECKOUTNOTICE, "checkNoticeNumber", checkNoticeNumber, ref sqls, ref objs);
                #endregion
                #region 子表数据
                //3、组织子表数据
                List<Hashtable> list_temp = new List<Hashtable>();
                List<Hashtable> list = new List<Hashtable>();
                list_temp = JsonHelper.DeserializeJsonToList<Hashtable>(shipProduct);
                for (int i = 0; i < list_temp.Count; i++)
                {
                    Hashtable ht_temp = new Hashtable();
                    ht_temp.Add("checkNoticeNumber", checkNoticeNumber);
                    ht_temp.Add("mass", list_temp[i]["mass"]);
                    ht_temp.Add("pcode", list_temp[i]["pcode"]);
                    ht_temp.Add("pname", list_temp[i]["pname"]);
                    ht_temp.Add("spec", list_temp[i]["spec"]);
                    //ht_temp.Add("quantity", list_temp[i]["sendQuantity"]);
                    ht_temp.Add("quantity", list_temp[i]["quantity"]);
                    ht_temp.Add("qunit", list_temp[i]["qunit"]);
                    ht_temp.Add("packing", list_temp[i]["packing"]);
                    ht_temp.Add("weight", list_temp[i]["weight"]);
                    ht_temp.Add("volume", list_temp[i]["volume"]);
                    list.Add(ht_temp);
                }
                //4、生成子表sql
                //4.1 先删除子表
                sqls.Add(new StringBuilder(" delete " + ConstantUtil.TABLE_CHECKOUTNOTICE_D +" where checkNoticeNumber = @checkNoticeNumber"));
                objs.Add(new SqlParam[] { new SqlParam("@checkNoticeNumber", checkNoticeNumber) });
                //4.1 生成子表sql
                SqlUtil.getBatchSqls(list, ConstantUtil.TABLE_CHECKOUTNOTICE_D,  ref sqls, ref objs);
                //6、执行sql
                int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

                if (result >= 0)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }


                #endregion
            }
            

            return JsonHelper.HashtableToJson(ht_result);
        }
        //提交订舱
        private string submitNotice(HttpContext context) 
        {
            Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            string checkNoticeNumber = (context.Request["checkNoticeNumber"] ?? "").ToString().Trim();
            string contractNo = (context.Request["saleContract"] ?? "").ToString().Trim();
            string createDateTag = (context.Request["createDateTag"] ?? "").ToString().Trim();
            string seller = (context.Request["seller"] ?? "").ToString().Trim();
            string consignor = (context.Request["consignor"] ?? "").ToString().Trim();
            string consignorAddress = (context.Request["consignorAddress"] ?? "").ToString().Trim();
            string consignorPhone = (context.Request["consignorPhone"] ?? "").ToString().Trim();
            string buyer = (context.Request["buyer"] ?? "").ToString().Trim();
            string consignee = (context.Request["consignee"] ?? "").ToString().Trim();
            string consigneeAddress = (context.Request["consigneeAddress"] ?? "").ToString().Trim();
            string consigneePhone = (context.Request["consigneePhone"] ?? "").ToString().Trim();
            string noticeMan1 = (context.Request["noticeMan1"] ?? "").ToString().Trim();
            string noticeManAddress1 = (context.Request["noticeManAddress1"] ?? "").ToString().Trim();
            string noticeManPhone1 = (context.Request["noticeManPhon1e"] ?? "").ToString().Trim();
            string noticeMan2 = (context.Request["noticeMan2"] ?? "").ToString().Trim();
            string noticeManAddress2 = (context.Request["noticeManAddress2"] ?? "").ToString().Trim();
            string noticeManPhone2 = (context.Request["noticeManPhone2"] ?? "").ToString().Trim();
            string boxCount = (context.Request["boxCount"] ?? "").ToString().Trim();
            string shipClause = (context.Request["shipClause"] ?? "").ToString().Trim();
            string deliveryPlace = (context.Request["deliveryPlace"] ?? "").ToString().Trim();
            string departurePort = (context.Request["departurePort"] ?? "").ToString().Trim();
            string departurePort_en = (context.Request["departurePort_en"] ?? "").ToString().Trim();
            string departurePortCountry = (context.Request["departurePortCountry"] ?? "").ToString().Trim();
            string departurePortCountry_en = (context.Request["departurePortCountry_en"] ?? "").ToString().Trim();
            string unloadPort = (context.Request["unloadPort"] ?? "").ToString().Trim();
            string unloadPort_en = (context.Request["unloadPort_en"] ?? "").ToString().Trim();
            string unloadPortCountry = (context.Request["unloadPortCountry"] ?? "").ToString().Trim();
            string unloadPortCountry_en = (context.Request["unloadPortCountry_en"] ?? "").ToString().Trim();
            string shippngcostItem = (context.Request["shippngcostItem"] ?? "").ToString().Trim();
            string boxCountCFS = (context.Request["boxCountCFS"] ?? "0").ToString().Trim();
            string boxCountHC = (context.Request["boxCountHC"] ?? "0").ToString().Trim();
            string boxCountGP4 = (context.Request["boxCountGP4"] ?? "0").ToString().Trim();
            string boxCountGP2 = (context.Request["boxCountGP2"] ?? "0").ToString().Trim();
            string preShipDate = (context.Request["preShipDate"] ?? "").ToString().Trim();
            string preShipCompanyName = (context.Request["preShipCompanyName"] ?? "").ToString().Trim();
            string mark = (context.Request["mark"] ?? "").ToString().Trim();

            //订舱信息
            string applyType = (context.Request["applyType"] ?? "").ToString().Trim();
            string billNumbers = (context.Request["billNumbers"] ?? "").ToString().Trim();
            string shipdate = (context.Request["shipdate"] ?? "").ToString().Trim();
            string shipname = (context.Request["shipname"] ?? "").ToString().Trim();
            string shipnumber = (context.Request["shipnumber"] ?? "").ToString().Trim();
            string containerCount = (context.Request["containerCount"] ?? "").ToString().Trim();
            string wharf = (context.Request["wharf"] ?? "").ToString().Trim();
            string costbear = (context.Request["costbear"] ?? "").ToString().Trim();
            string isDT = (context.Request["isDT"] ?? "").ToString().Trim();
            string isTM = (context.Request["isTM"] ?? "").ToString().Trim();
            string isCM = (context.Request["isCM"] ?? "").ToString().Trim();
            string isTZMD = (context.Request["isTZMD"] ?? "").ToString().Trim();
            string isTDD = (context.Request["isTDD"] ?? "").ToString().Trim();
            string shipcompanyname = (context.Request["shipcompanyname"] ?? "").ToString().Trim();
            string cutoffdate = (context.Request["cutoffdate"] ?? "").ToString().Trim();
            string boxorderName = (context.Request["boxorderName"] ?? "").ToString().Trim();
            string boxorderUrl = (context.Request["boxorderUrl"] ?? "").ToString().Trim();
            string bhName = (context.Request["bhName"] ?? "").ToString().Trim();
            string bhUrl = (context.Request["bhUrl"] ?? "").ToString().Trim();
            string noticeorderName = (context.Request["noticeorderName"] ?? "").ToString().Trim();
            string noticeorderUrl = (context.Request["noticeorderUrl"] ?? "").ToString().Trim();

            string shipProduct = context.Request["shipProduct"] ?? "";
            //判断是否已申请
            /*取消校验
            StringBuilder isApplied_sql = new StringBuilder();
            isApplied_sql.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE)
                .Append(" where saleContract='" + contractNo + "' and contractCreateDateTag='" + createDateTag + "'");
            DataTable dt_isApplied = DataFactory.SqlDataBase().GetDataTableBySQL(isApplied_sql);
            if (DataTableHelper.IsExistRows(dt_isApplied) && (dt_isApplied.Rows[0]["NOTICESTATUS"].ToString()) != ConstantUtil.STATUS_HY_BACK)
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "该发货申请已经申请过订舱信息！");
                
            }
             * */
            if(1==2)
            {

            }
            else 
            {
                /*
                if (DataTableHelper.IsExistRows(dt_isApplied)) 
                {
                    checkNoticeNumber = dt_isApplied.Rows[0]["CHECKNOTICENUMBER"].ToString();
                }
                 * */
                #region 主数据
                //1、组织数据
                if (string.IsNullOrEmpty(checkNoticeNumber))
                {
                    string sequenceKey = "HYDC" + DateTimeHelper.GetToday("yyyyMM");
                    checkNoticeNumber = sequenceKey + Sequence.getSequenceAutoAddZero(sequenceKey, 4);
                }
                Hashtable ht_main = new Hashtable();
                ht_main.Add("checkNoticeNumber", checkNoticeNumber);
                ht_main.Add("seller", seller);
                ht_main.Add("consignor", consignor);
                ht_main.Add("saleContract", contractNo);
                ht_main.Add("contractCreateDateTag", createDateTag);
                ht_main.Add("consignorAddress", consignorAddress);
                ht_main.Add("consignorPhone", consignorPhone);
                ht_main.Add("buyer", buyer);
                ht_main.Add("consignee", consignee);
                ht_main.Add("consigneeAddress", consigneeAddress);
                ht_main.Add("consigneePhone", consigneePhone);
                ht_main.Add("noticeMan1", noticeMan1);
                ht_main.Add("noticeManAddress1", noticeManAddress1);
                ht_main.Add("noticeManPhone1", noticeManPhone1);
                ht_main.Add("noticeMan2", noticeMan2);
                ht_main.Add("noticeManAddress2", noticeManAddress2);
                ht_main.Add("noticeManPhone2", noticeManPhone2);
                ht_main.Add("boxCount", boxCount);
                ht_main.Add("shipClause", shipClause);
                ht_main.Add("deliveryPlace", deliveryPlace);
                ht_main.Add("departurePort", departurePort);
                ht_main.Add("departurePort_en", departurePort_en);
                ht_main.Add("departurePortCountry", departurePortCountry);
                ht_main.Add("departurePortCountry_en", departurePortCountry_en);
                ht_main.Add("unloadPort", unloadPort);
                ht_main.Add("unloadPort_en", unloadPort_en);
                ht_main.Add("unloadPortCountry", unloadPortCountry);
                ht_main.Add("unloadPortCountry_en", unloadPortCountry_en);
                ht_main.Add("shippngcostItem", shippngcostItem);
                ht_main.Add("boxCountCFS", boxCountCFS);
                ht_main.Add("boxCountHC", boxCountHC);
                ht_main.Add("boxCountGP4", boxCountGP4);
                ht_main.Add("boxCountGP2", boxCountGP2);
                ht_main.Add("preShipDate", preShipDate);
                ht_main.Add("preShipCompanyName", preShipCompanyName);
                ht_main.Add("mark", mark);
                ht_main.Add("containerCount", int.Parse(boxCountCFS) + int.Parse(boxCountHC) + int.Parse(boxCountGP4) + int.Parse(boxCountGP2));
                ht_main.Add("noticeStatus", ConstantUtil.STATUS_HY_APPLY_ED);
                
                ht_main.Add("sendOutNoticeStatus", ConstantUtil.STATUS_SENDOUTNOTICE_UN);
                ht_main.Add("createman", RequestSession.GetSessionUser().UserName.ToString());
                ht_main.Add("createtime", DateTimeHelper.ShortDateTimeS);
 
                if (applyType=="客户订舱")
                {
                    ht_main.Add("billNumbers", billNumbers);
                    ht_main.Add("shipdate", shipdate);
                    ht_main.Add("shipname", shipname);
                    ht_main.Add("shipnumber", shipnumber);
                    ht_main.Add("wharf", wharf);
                    ht_main.Add("costbear", costbear);
                    ht_main.Add("isDT", isDT);
                    ht_main.Add("isTM", isTM);
                    ht_main.Add("isCM", isCM);
                    ht_main.Add("isTZMD", isTZMD);
                    ht_main.Add("isTDD", isTDD);
                    ht_main.Add("shipcompanyname", shipcompanyname);
                    ht_main.Add("cutoffdate", cutoffdate);
                    ht_main.Add("boxorderName", boxorderName);
                    ht_main.Add("boxorderUrl", boxorderUrl);
                    ht_main.Add("bhName", bhName);
                    ht_main.Add("bhUrl", bhUrl);
                    ht_main.Add("noticeorderName", noticeorderName);
                    ht_main.Add("noticeorderUrl", noticeorderUrl);
                    ht_main.Add("confirmStatus", ConstantUtil.STATUS_CONFIRM_ED);
                    ht_main.Add("distributeStatus", ConstantUtil.STATUS_DISTRIBUTE_ED);
                }
                else 
                {
                    ht_main.Add("confirmStatus", ConstantUtil.STATUS_CONFIRM_UN);
                    ht_main.Add("distributeStatus", ConstantUtil.STATUS_DISTRIBUTE_UN);
                }
                //2、生成主表sql
                SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_CHECKOUTNOTICE, "checkNoticeNumber", checkNoticeNumber, ref sqls, ref objs);
                #endregion
                #region 子表数据
                //3、组织子表数据
                List<Hashtable> list_temp = new List<Hashtable>();
                List<Hashtable> list = new List<Hashtable>();
                list_temp = JsonHelper.DeserializeJsonToList<Hashtable>(shipProduct);
                for (int i = 0; i < list_temp.Count; i++)
                {
                    Hashtable ht_temp = new Hashtable();
                    ht_temp.Add("checkNoticeNumber", checkNoticeNumber);
                    ht_temp.Add("mass", list_temp[i]["mass"]);
                    ht_temp.Add("pcode", list_temp[i]["pcode"]);
                    ht_temp.Add("pname", list_temp[i]["pname"]);
                    ht_temp.Add("spec", list_temp[i]["spec"]);
                    //ht_temp.Add("quantity", list_temp[i]["sendQuantity"]);
                    ht_temp.Add("quantity", list_temp[i]["quantity"]);
                    ht_temp.Add("qunit", list_temp[i]["qunit"]);
                    ht_temp.Add("packing", list_temp[i]["packing"]);
                    ht_temp.Add("weight", list_temp[i]["weight"]);
                    ht_temp.Add("volume", list_temp[i]["volume"]);
                    list.Add(ht_temp);
                }
                //4、生成子表sql
                //4.1 先删除子表
                sqls.Add(new StringBuilder(" delete " + ConstantUtil.TABLE_CHECKOUTNOTICE_D +" where checkNoticeNumber = @checkNoticeNumber"));
                objs.Add(new SqlParam[] { new SqlParam("@checkNoticeNumber", checkNoticeNumber) });
                //4.1 生成子表sql
                SqlUtil.getBatchSqls(list, ConstantUtil.TABLE_CHECKOUTNOTICE_D,  ref sqls, ref objs);

                //5、组织更新发货申请中申请订舱状态的sql
                StringBuilder update_sql = new StringBuilder();
                update_sql.Append(" update " + ConstantUtil.TABLE_SENDOUTAPPDETAILS + " set checkNoticeStatus='" + ConstantUtil.STATUS_APPLY_ED + "'")
                    .Append(" where contractNo=@contractNo and createDateTag=@createDateTag");
                sqls.Add(update_sql);
                objs.Add(new SqlParam[] { new SqlParam("@contractNo", contractNo), new SqlParam("@createDateTag", createDateTag) });
                //6、执行sql
                int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

                if (result >= 0)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }


                #endregion
            }
            

            return JsonHelper.HashtableToJson(ht_result);
        }
        #region 分配订舱申请

        //接收提交订舱申请（分配订舱申请）
        private string receiveSubmitApply(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            string checkNoticeNumber = (context.Request["checkNoticeNumber"] ?? "").ToString().Trim();
            string sendEmail = (context.Request["sendEmail"] ?? "").ToString().Trim();
            string customMan = (context.Request["customMan"] ?? "").ToString().Trim();
            string[] distributedInfo = null;
            if (!string.IsNullOrEmpty(customMan)) 
            {
                distributedInfo = customMan.Split('-');
            }
            if (distributedInfo.Length != 2)
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "请检查报关行信息！");
            }
            else 
            {
                //判断是否已分配
                StringBuilder isApplied_sql = new StringBuilder();
                isApplied_sql.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE)
                    .Append(" where checkNoticeNumber='" + checkNoticeNumber + "' and distributeStatus='" + ConstantUtil.STATUS_DISTRIBUTE_ED + "'");
                DataTable dt_isApplied = DataFactory.SqlDataBase().GetDataTableBySQL(isApplied_sql);
                if (DataTableHelper.IsExistRows(dt_isApplied))
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "该订舱申请已经分配完成！");
                }
                else
                {
                    #region 主数据
                    //1、组织数据
                    Hashtable ht_main = new Hashtable();
                    ht_main.Add("checkNoticeNumber", checkNoticeNumber);
                    ht_main.Add("distributeAgency", distributedInfo[0]);
                    ht_main.Add("distributeMan_ED", distributedInfo[1]);
                    ht_main.Add("distributeMan", RequestSession.GetSessionUser().UserName.ToString());
                    ht_main.Add("confirmStatus", ConstantUtil.STATUS_CONFIRM_UN);
                    ht_main.Add("distributeDatetime", DateTimeHelper.ShortDateTimeS);
                    ht_main.Add("distributeStatus", ConstantUtil.STATUS_DISTRIBUTE_ED);
                    //2、生成主表sql
                    SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_CHECKOUTNOTICE, "checkNoticeNumber", checkNoticeNumber, ref sqls, ref objs);
                    #endregion
                    //3、执行sql
                    int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

                    if (result >= 0)
                    {
                        //发送邮件
                        if (sendEmail == "true")
                        {
                            StringBuilder sql1 = new StringBuilder("select * from Com_UserInfos where UserRealName='" + customMan + "'");
                            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql1, 0);

                            if (dt != null && dt.Rows.Count == 1)
                            {
                                string Dep_Email = dt.Rows[0]["Email"].ToString();
                                string Mis_Name = "中泰欣隆业务管理系统";
                                string Mis_Describe = "系统信息：您有一个待确认海运订舱申请，订舱编号：" + checkNoticeNumber + " 请您及时登录系统进行处理！";
                                SMTPManager.MailSending(Dep_Email, Mis_Name, Mis_Describe, "");
                            }
                        }

                        ht_result.Add("status", "T");
                        ht_result.Add("msg", "操作成功！");
                    }
                    else
                    {
                        ht_result.Add("status", "F");
                        ht_result.Add("msg", "操作失败！");
                    }

                }
            }

            return JsonHelper.HashtableToJson(ht_result);
        }
        //退回订舱申请
        private string backApply(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            string checkNoticeNumber = (context.Request["checkNoticeNumber"] ?? "").ToString().Trim();

            //判断是否已退回
            StringBuilder isApplied_sql = new StringBuilder();
            isApplied_sql.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE)
                .Append(" where checkNoticeNumber='" + checkNoticeNumber + "' and noticeStatus='" + ConstantUtil.STATUS_HY_BACK + "'");
            DataTable dt_isApplied = DataFactory.SqlDataBase().GetDataTableBySQL(isApplied_sql);
            if (DataTableHelper.IsExistRows(dt_isApplied))
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "该订舱申请已经分配完成！");
            }
            else
            {
                #region 主数据
                //1、组织数据
                Hashtable ht_main = new Hashtable();
                ht_main.Add("checkNoticeNumber", checkNoticeNumber);
                ht_main.Add("noticeStatus", ConstantUtil.STATUS_HY_BACK);
                ht_main.Add("distributeStatus", "");//分配状态置空
                ht_main.Add("confirmStatus", "");//确认状态置空
                //2、生成主表sql
                SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_CHECKOUTNOTICE, "checkNoticeNumber", checkNoticeNumber, ref sqls, ref objs);
                #endregion
                //3、执行sql
                int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

                if (result >= 0)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }

            }


            return JsonHelper.HashtableToJson(ht_result);
        }

        #endregion

        #region 申请确认

        //退回订舱申请至分配节点
        private string backConfirm(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            string checkNoticeNumber = (context.Request["checkNoticeNumber"] ?? "").ToString().Trim();

            //判断是否已退回
            StringBuilder isApplied_sql = new StringBuilder();
            isApplied_sql.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE)
                .Append(" where checkNoticeNumber='" + checkNoticeNumber + "' and distributeStatus='" + ConstantUtil.STATUS_DISTRIBUTE_BACK + "'");
            DataTable dt_isApplied = DataFactory.SqlDataBase().GetDataTableBySQL(isApplied_sql);
            if (DataTableHelper.IsExistRows(dt_isApplied))
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "该订舱申请已经退回！");
            }
            else
            {
                #region 主数据
                //1、组织数据
                Hashtable ht_main = new Hashtable();
                ht_main.Add("checkNoticeNumber", checkNoticeNumber);
                ht_main.Add("distributeStatus", ConstantUtil.STATUS_DISTRIBUTE_BACK);
                ht_main.Add("confirmStatus", "");//确认状态置空
                //2、生成主表sql
                SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_CHECKOUTNOTICE, "checkNoticeNumber", checkNoticeNumber, ref sqls, ref objs);
                #endregion
                //3、执行sql
                int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

                if (result >= 0)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }

            }


            return JsonHelper.HashtableToJson(ht_result);
        }
        //确认节点接收订舱申请
        private string receiveApply(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            string checkNoticeNumber = (context.Request["checkNoticeNumber"] ?? "").ToString().Trim();

            //判断是否已接收订舱申请
            StringBuilder isApplied_sql = new StringBuilder();
            isApplied_sql.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE)
                .Append(" where checkNoticeNumber='" + checkNoticeNumber + "' and confirmStatus='" + ConstantUtil.STATUS_CONFIRM_RECEIVE + "'");
            DataTable dt_isApplied = DataFactory.SqlDataBase().GetDataTableBySQL(isApplied_sql);
            if (DataTableHelper.IsExistRows(dt_isApplied))
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "该订舱申请已经接收！");
            }
            else
            {
                #region 主数据
                //1、组织数据
                Hashtable ht_main = new Hashtable();
                ht_main.Add("checkNoticeNumber", checkNoticeNumber);
                ht_main.Add("confirmStatus", ConstantUtil.STATUS_CONFIRM_RECEIVE);
                //2、生成主表sql
                SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_CHECKOUTNOTICE, "checkNoticeNumber", checkNoticeNumber, ref sqls, ref objs);
                #endregion
                //3、执行sql
                int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

                if (result >= 0)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }

            }


            return JsonHelper.HashtableToJson(ht_result);
        }

        #endregion

        //更新订舱通知
        private string updateNotice(HttpContext context) 
        {
            Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            string checkNoticeNumber = (context.Request["applyNo"] ?? "").ToString().Trim();
            string billNumbers = (context.Request["billNumbers"] ?? "").ToString().Trim();
            string shipdate = (context.Request["shipdate"] ?? "").ToString().Trim();
            string shipname = (context.Request["shipname"] ?? "").ToString().Trim();
            string shipnumber = (context.Request["shipnumber"] ?? "").ToString().Trim();
            string containerCount = (context.Request["containerCount"] ?? "").ToString().Trim();
            string wharf = (context.Request["wharf"] ?? "").ToString().Trim();
            string costbear = (context.Request["costbear"] ?? "").ToString().Trim();
            string isDT = (context.Request["isDT"] ?? "").ToString().Trim();
            string isTM = (context.Request["isTM"] ?? "").ToString().Trim();
            string isCM = (context.Request["isCM"] ?? "").ToString().Trim();
            string isTZMD = (context.Request["isTZMD"] ?? "").ToString().Trim();
            string isTDD = (context.Request["isTDD"] ?? "").ToString().Trim();
            string shipcompanyname = (context.Request["shipcompanyname"] ?? "").ToString().Trim();
            string cutoffdate = (context.Request["cutoffdate"] ?? "").ToString().Trim();
            string boxorderName = (context.Request["boxorderName"] ?? "").ToString().Trim();
            string boxorderUrl = (context.Request["boxorderUrl"] ?? "").ToString().Trim();
            string bhName = (context.Request["bhName"] ?? "").ToString().Trim();
            string bhUrl = (context.Request["bhUrl"] ?? "").ToString().Trim();
            string noticeorderName = (context.Request["noticeorderName"] ?? "").ToString().Trim();
            string noticeorderUrl = (context.Request["noticeorderUrl"] ?? "").ToString().Trim();

            string shipProduct = context.Request["shipProduct"] ?? "";
            //判断是否已申请
            StringBuilder isApplied_sql = new StringBuilder();
            isApplied_sql.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE)
                .Append(" where checkNoticeNumber='" + checkNoticeNumber + "' and confirmStatus='" + ConstantUtil.STATUS_CONFIRM_ED + "'");
            DataTable dt_isApplied = DataFactory.SqlDataBase().GetDataTableBySQL(isApplied_sql);
            if (DataTableHelper.IsExistRows(dt_isApplied))
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "该发货申请已经申请过订舱信息！");
            }
            else 
            {
                #region 主数据
                //1、组织数据
                Hashtable ht_main = new Hashtable();
                ht_main.Add("checkNoticeNumber", checkNoticeNumber);
                ht_main.Add("billNumbers", billNumbers);
                ht_main.Add("shipdate", shipdate);
                ht_main.Add("shipname", shipname);
                ht_main.Add("shipnumber", shipnumber);
                ht_main.Add("wharf", wharf);
                ht_main.Add("costbear", costbear);
                ht_main.Add("isDT", isDT);
                ht_main.Add("isTM", isTM);
                ht_main.Add("isCM", isCM);
                ht_main.Add("isTZMD", isTZMD);
                ht_main.Add("isTDD", isTDD);
                ht_main.Add("shipcompanyname", shipcompanyname);
                ht_main.Add("cutoffdate", cutoffdate);
                ht_main.Add("boxorderName", boxorderName);
                ht_main.Add("boxorderUrl", boxorderUrl);
                ht_main.Add("bhName", bhName);
                ht_main.Add("bhUrl", bhUrl);
                ht_main.Add("noticeorderName", noticeorderName);
                ht_main.Add("noticeorderUrl", noticeorderUrl);
                ht_main.Add("confirmStatus", ConstantUtil.STATUS_CONFIRM_ED);
                ht_main.Add("updateman", RequestSession.GetSessionUser().UserName.ToString());
                ht_main.Add("updatetime", DateTimeHelper.ShortDateTimeS);
                //2、生成主表sql
                SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_CHECKOUTNOTICE, "checkNoticeNumber", checkNoticeNumber, ref sqls, ref objs);
                #endregion
                //3、执行sql
                int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

                if (result >= 0)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }

            }
            

            return JsonHelper.HashtableToJson(ht_result);
        }

        //获取发运通知明细表
        private string getCheckNoticeDList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"]?? "";
            string contractCreateDateTag = context.Request.Params["createDateTag"] ?? "";
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder();
            sqlwhere.Append("and saleContract=@contractNo and contractCreateDateTag=@contractCreateDateTag ");

            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@contractCreateDateTag",Value=contractCreateDateTag,DbType=DbType.String}
                };
            sqldata.AppendFormat(@"select * from {0}  where 1=1 " + sqlwhere.ToString(), ConstantUtil.VIEW_CheckOutNotice);
            sqlcount.AppendFormat("select count(1) from {0} where 1=1 " + sqlwhere.ToString(), ConstantUtil.VIEW_CheckOutNotice);
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);
            return sb.ToString();
        }

        //获取发运通知明细表+库存信息
        private string getCheckNoticeDList_WH(HttpContext context)
        {
            //int row = int.Parse(context.Request["rows"].ToString());
            //int page = int.Parse(context.Request["page"].ToString());
            //string order = context.Request["order"].ToString();
            //string sort = context.Request["sort"].ToString();
            string wcode = context.Request.Params["wcode"] ?? "";
            string owner_wh = context.Request.Params["owner_wh"] ?? "";
            string contractNo = context.Request.Params["contractNo"] ?? "";
            string contractCreateDateTag = context.Request.Params["createDateTag"] ?? "";
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder();
            //sqlwhere.Append(" and createDateTag=@createDateTag and owner = @owner and wcode =@wcode");
            sqlwhere.Append(" and owner = @owner and wcode =@wcode");
            

            SqlParameter[] pms = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@createDateTag",Value=contractCreateDateTag,DbType=DbType.String},
                    new SqlParameter{ParameterName="@owner",Value=owner_wh,DbType=DbType.String},
                    new SqlParameter{ParameterName="@wcode",Value=wcode,DbType=DbType.String}
                };
            sqldata.AppendFormat(@"select * ,0 as realnumber,0 as realquantity from {0}  where 1=1 " + sqlwhere.ToString(), ConstantUtil.VIEW_SENDAPPSTOCK1);
            sqlcount.AppendFormat("select count(1) from {0}  where 1=1 " + sqlwhere.ToString(), ConstantUtil.VIEW_SENDAPPSTOCK);
            
            //StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, pms);
            return sb.ToString();
        }

        
        //获取仓库列表
        private StringBuilder getWarehouse(HttpContext context)
        {
            RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
            StringBuilder sqlWhere = new StringBuilder();
            string ownercode = context.Request.Params["sellercode"] ?? "";
            string owner = context.Request.Params["seller"] ?? "";
            if (!string.IsNullOrEmpty(ownercode)) 
            {
                sqlWhere.Append(" and ownercode='" + ownercode + "'");
            }
            if (!string.IsNullOrEmpty(owner))
            {
                sqlWhere.Append(" and owner='" + owner + "'");
            }


            StringBuilder sb = new StringBuilder("select distinct wcode,wname,ownercode,owner from StockEntitySwift where 1=1 ");
            sb.Append(sqlWhere);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        
        //海运发货通知
        private string addStockOut(HttpContext context) 
        {
            Hashtable ht_result = new Hashtable();
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            string checkNoticeNumber = (context.Request["checkNoticeNumber"] ?? "").ToString().Trim();
            string createDateTag = context.Request.Params["createDateTag"] ?? "";
            string contractNo = (context.Request["contractNo"] ?? "").ToString().Trim();
            string wcode = (context.Request["wcode"] ?? "").ToString().Trim();
            string wname = (context.Request["wname"] ?? "").ToString().Trim();
            string owner = (context.Request["owner"] ?? "").ToString().Trim();
            string ownercode = (context.Request["ownercode"] ?? "").ToString().Trim();
            string wkeeper = (context.Request["wkeeper"] ?? "").ToString().Trim();
            string wkeepercode = (context.Request["wkeepercode"] ?? "").ToString().Trim();
            string isDT = (context.Request["isDT"] ?? "").ToString().Trim();
            string isTM = (context.Request["isTM"] ?? "").ToString().Trim();
            string isCM = (context.Request["isCM"] ?? "").ToString().Trim();
            string isTZMD = (context.Request["isTZMD"] ?? "").ToString().Trim();
            string costbear = (context.Request["costbear"] ?? "").ToString().Trim();
            string note = (context.Request["note"] ?? "").ToString().Trim();
            string sendDetail = (context.Request["sendDetail"] ?? "").ToString().Trim();
            if (!string.IsNullOrEmpty(checkNoticeNumber))
            {
                //获取合同信息
                StringBuilder getContract_sql = new StringBuilder();
                getContract_sql.Append(" select * from " + ConstantUtil.TABLE_ECONTRACT)
                    .Append(" where contractNo='" + contractNo + "'");
                DataTable dt_Contract = DataFactory.SqlDataBase().GetDataTableBySQL(getContract_sql);
                Hashtable ht_Contract = DataTableHelper.DataTableToHashtable(dt_Contract);

                //获取订舱信息
                StringBuilder getCheckNotice_sql = new StringBuilder();
                getCheckNotice_sql.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE)
                    .Append(" where checkNoticeNumber='" + checkNoticeNumber + "'");
                DataTable dt_checkNotice = DataFactory.SqlDataBase().GetDataTableBySQL(getCheckNotice_sql);
                Hashtable ht_checkNotice = DataTableHelper.DataTableToHashtable(dt_checkNotice);

                //获取订舱申请明细
                StringBuilder sql_getCheckNoticeD = new StringBuilder();
                sql_getCheckNoticeD.Append(" select * from " + ConstantUtil.TABLE_CHECKOUTNOTICE_D)
                    .Append(" where checkNoticeNumber='" + checkNoticeNumber + "'");
                DataTable dt_checkNoticeD = DataFactory.SqlDataBase().GetDataTableBySQL(sql_getCheckNoticeD);
                List<Hashtable> list_checkNoticeD = DataTableHelper.DataTableToList(dt_checkNoticeD);

                #region 更新订舱信息
                Hashtable ht_dc = new Hashtable();
                ht_dc.Add("checkNoticeNumber", checkNoticeNumber);
                ht_dc.Add("isDT", isDT);
                ht_dc.Add("isTM", isTM);
                ht_dc.Add("isCM", isCM);
                ht_dc.Add("isTZMD", isTZMD);
                ht_dc.Add("costbear", costbear);
                ht_dc.Add("note", note);
                //生成主表sql
                SqlUtil.getBatchSqls(ht_dc, ConstantUtil.TABLE_CHECKOUTNOTICE, "checkNoticeNumber", checkNoticeNumber, ref sqls, ref objs);
                #endregion

                #region 主数据
                //1、组织数据
                string sequenceKey = "CK" + DateTimeHelper.GetToday("yyyyMMdd");
                string outdocno = sequenceKey + Sequence.getSequenceAutoAddZero(sequenceKey, 4);
                Hashtable ht_main = new Hashtable();
                ht_main.Add("checkNoticeNumber", checkNoticeNumber);
                ht_main.Add("outdocno", outdocno);
                ht_main.Add("wcode", wcode);
                ht_main.Add("wname", wname);
                ht_main.Add("outtype", "发货通知");
                ht_main.Add("outdate", DateTimeHelper.ShortDateTime);
                ht_main.Add("ownercode", ownercode);
                ht_main.Add("owner", owner);
                ht_main.Add("contractNo", ht_Contract["CONTRACTNO"]);
                ht_main.Add("buyercode", ht_Contract["BUYERCODE"]);
                ht_main.Add("buyer", ht_Contract["BUYER"]);
                ht_main.Add("sellercode", ht_Contract["SELLERCODE"]);
                ht_main.Add("seller", ht_Contract["SELLER"]);

                ht_main.Add("shipcompany", ht_checkNotice["SHIPCOMPANYNAME"]);
                ht_main.Add("shipname", ht_checkNotice["SHIPNAME"]);
                ht_main.Add("shipnum", ht_checkNotice["SHIPNUMBER"]);
                ht_main.Add("status", "待发货");
                ht_main.Add("busman", wkeeper);
                ht_main.Add("busmancode", wkeepercode);
                ht_main.Add("createman", RequestSession.GetSessionUser().UserAccount.ToString());
                ht_main.Add("createmanname", RequestSession.GetSessionUser().UserName.ToString());
                ht_main.Add("createdate", DateTimeHelper.ShortDateTimeS);
                //2、生成主表sql
                SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_STOCKOUTENTITY, "outdocno", outdocno, ref sqls, ref objs);
                #endregion
                #region 子表数据
                //3、组织子表数据
                List<Hashtable> list_temp = new List<Hashtable>();
                List<Hashtable> list = new List<Hashtable>();
                list_temp = JsonHelper.DeserializeJsonToList<Hashtable>(sendDetail);
                /*
                for (int i = 0; i < list_checkNoticeD.Count; i++)
                {
                    Hashtable ht_temp = new Hashtable();
                    ht_temp.Add("outdocno", outdocno);
                    ht_temp.Add("mcode", list_checkNoticeD[i]["PCODE"]);
                    ht_temp.Add("mname", list_checkNoticeD[i]["PNAME"]);
                    ht_temp.Add("mspec", list_checkNoticeD[i]["SPEC"]);
                    ht_temp.Add("unit", list_checkNoticeD[i]["QUNIT"]);
                    ht_temp.Add("pack", list_checkNoticeD[i]["PACKING"]);
                    ht_temp.Add("packdes", "");
                    ht_temp.Add("number", list_checkNoticeD[i]["QUANTITY"]);
                    ht_temp.Add("outquantity", list_checkNoticeD[i]["QUANTITY"]);
                    list.Add(ht_temp);
                }*/
                for (int i = 0; i < list_temp.Count; i++)
                {
                    Hashtable ht_temp = new Hashtable();
                    if (list_temp[i]["realquantity"].ToString() != "0" & list_temp[i]["realquantity"].ToString() != "")
                    {
                        ht_temp.Add("outdocno", outdocno);
                        ht_temp.Add("batchno", list_temp[i]["indocno"]);
                        ht_temp.Add("mcode", list_temp[i]["mcode"]);
                        ht_temp.Add("mname", list_temp[i]["mname"]);
                        ht_temp.Add("mspec", list_temp[i]["mspec"]);
                        ht_temp.Add("unit", list_temp[i]["unit"]);
                        ht_temp.Add("pack", list_temp[i]["pack"]);
                        ht_temp.Add("packunit", list_temp[i]["packunit"]);
                        ht_temp.Add("packdes", list_temp[i]["packdes"]);
                        ht_temp.Add("number", list_temp[i]["realnumber"]);
                        ht_temp.Add("outquantity", list_temp[i]["realquantity"]);
                        list.Add(ht_temp);
                    }
                    
                }
                //4、生成子表sql
                SqlUtil.getBatchSqls(list, ConstantUtil.TABLE_STOCKOUTENTITY_D, ref sqls, ref objs);

                //5、组织更新请中通知状态的sql
                StringBuilder update_sql = new StringBuilder();
                update_sql.Append(" update " + ConstantUtil.TABLE_CHECKOUTNOTICE + " set sendOutNoticeStatus='" + ConstantUtil.STATUS_SENDOUTNOTICE_ED + "'")
                    .Append(",outNoticeman='" + RequestSession.GetSessionUser().UserName.ToString() + "'")
                    .Append(",outNoticetime='" + DateTimeHelper.ShortDateTimeS + "'")
                    .Append(" where checkNoticeNumber=@checkNoticeNumber ");
                sqls.Add(update_sql);
                objs.Add(new SqlParam[] { new SqlParam("@checkNoticeNumber", checkNoticeNumber) });

                #endregion
                //6、执行sql
                int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

                if (result >= 0)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }


            } else 
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "没有可操作数据！");
            }
            

            return JsonHelper.HashtableToJson(ht_result);
        }

        //上传附件
        private string uploadFile(HttpContext context)
        {

            //var status = context.Request["customerStatus"];
            var data = "";

            if (context.Request.Files.Count > 0)
            {
                var file = context.Request.Files[0];

                string path = "/Files/SendOut/"+ DateTime.Now.ToString("yyyyMMddHHmmss") + file.FileName;
                Directory.CreateDirectory(Path.GetDirectoryName(context.Server.MapPath(path)));

                file.SaveAs(context.Server.MapPath(path));

                return path + ":" + file.FileName;
            }
            else
            {
                return "error";
            }
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