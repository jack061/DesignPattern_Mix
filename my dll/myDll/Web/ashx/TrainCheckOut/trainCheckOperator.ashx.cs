using RM.Busines;
using RM.Busines.Util;
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
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.TrainCheckOut
{
    /// <summary>
    /// trainCheckOperator 的摘要说明
    /// </summary>
    public class trainCheckOperator : IHttpHandler, IRequiresSessionState
    {
        RM.Busines.contract.contractBLL contractBll = new Busines.contract.contractBLL();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string module = context.Request["module"];
            bool suc = false;
            string err = "";
            switch (module)
            {
                //添加铁路发货通知
                case "addTrainDelivery":
                    suc = addTrainDelivery(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //编辑铁路发货通知
                case "editTrainDelivery":
                    suc = editTrainDelivery(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //删除铁路发货通知
                case "delTrainDelivery":
                    suc = delTrainDelivery(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //更新打印次数
                case "updatePrintCount":
                    suc = updatePrintCount(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //添加车厢号
                case "addCarriageCode":
                    suc = addCarriageCode(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //更改发货通知表状态为退回，可编辑
                case "updateSaveStatus":
                    suc = updateSaveStatus(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //更新车厢号
                case "updateCarriageCode":
                    suc = updateCarriageCode(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;

            }


        }

        #region 更新车厢号
        private bool updateCarriageCode(ref string err, HttpContext context)
        {
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            string rowsJson = context.Request.Params["rowsJson"]??string.Empty;
            string noticeTag = context.Request.Params["noticeTag"] ?? string.Empty;
            List<Hashtable> carriageList = JsonHelper.DeserializeJsonToList<Hashtable>(rowsJson);
           
            foreach (var item in carriageList)
            {
                Hashtable ht = new Hashtable();
                ht["carriageNumber"] = item["carriageNumber"] ?? string.Empty;
                SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_TRAINDELPAYCODE, "id", item["id"].ToString(), ref sqls, ref objs);
            }
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;

        } 
        #endregion

        #region 更改发货通知表状态为退回，可编辑
        private bool updateSaveStatus(ref string err, HttpContext context)
        {
            string noticeTag = context.Request.Params["noticeTag"] ?? string.Empty;
            Hashtable hs = new Hashtable();
            hs.Add("noticeTag", noticeTag);
            hs.Add("saveStatus", "2");
            int r = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_TRAINDELIVERYNOTICE, "noticeTag", noticeTag, hs);
            return r > 0 ? true : false;
        }
        #endregion

        private bool addCarriageCode(ref string err, HttpContext context)
        {
            int r = 0;
            //获取发货申请唯一标识更新状态
            string createDateTag = context.Request.Params["createDateTag"] ?? string.Empty;
            //获取付费代码详情
            string trainpayCode = context.Request.Params["trainpayCode"];
            List<Hashtable> trainpayCodeTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainpayCode);
            foreach (Hashtable hs in trainpayCodeTable)
            {
                string id = hs["id"].ToString();
                Hashtable htPayCode = new Hashtable();
                htPayCode["carriageNumber"] = hs["carriageNumber"];
                r = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_TRAINDELPAYCODE, "id", id, htPayCode);
            }
            return r > 0 ? true : false;

        }
        //更新打印次数
        private bool updatePrintCount(ref string err, HttpContext context)
        {
            string createDateTag = context.Request.Params["createDateTag"];
            StringBuilder sb = new StringBuilder(@"select printCount from trainDelPayCode where createDateTag=@createDateTag");
            //获取已打印次数
            string printCount = DataFactory.SqlDataBase().getString(sb,
                new SqlParam[1] { new SqlParam("@createDateTag", createDateTag) }, "printCount");
            if (string.IsNullOrEmpty(printCount))
            {
                printCount = "0";
            }
            //更新打印次数
            int count = int.Parse(printCount) + 1;
            Hashtable ht = new Hashtable();
            ht["printCount"] = count;
            int r = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_TRAINDELPAYCODE, "createDateTag", createDateTag, ht);
            return r > 0 ? true : false;

        }
        //删除铁路发货通知
        private bool delTrainDelivery(ref string err, HttpContext context)
        {
            string contractNo = context.Request["contractNo"] == null ? "" : context.Request["contractNo"].ToString();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_TRAINDELIVERYNOTICE, "contractNo", contractNo);
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_TRAINDILIVERYPRODUCT, "contractNo", contractNo);
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_TRAINFROSTATION, "contractNo", contractNo);
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_TRAINDELPAYCODE, "contractNo", contractNo);
                    //更改合同列表发货状态为未完成
                    string sql = "update Econtract set applystatus=0 where contractNo=@contractNo";
                    SqlParameter[] pms = new SqlParameter[]{
                        new SqlParameter("@contractNo",contractNo)
                    };
                    int q = bll.ExecuteNonQuery(sql, pms);
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
        //编辑铁路发货通知
        private bool editTrainDelivery(ref string err, HttpContext context)
        {
            //获取国境口岸站详情
            string trainFrontierStation = context.Request.Params["trainFrontierStation"];
            //获取发货申请唯一标识更新状态
            string createDateTag = context.Request.Params["createDateTag"] ?? string.Empty;
            List<Hashtable> trainFrontierStationTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainFrontierStation);
            //获取付费代码详情
            string trainpayCode = context.Request.Params["trainpayCode"];
            List<Hashtable> trainpayCodeTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainpayCode);
            //获取产品详情
            string trainProduct = context.Request.Params["trainProduct"];
            List<Hashtable> trainProductTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainProduct);
            Hashtable ht = new Hashtable();
            ht["id"] = context.Request["id"] == null ? "" : context.Request["id"].ToString();
            ht["contractNo"] = context.Request["contractNo"] == null ? "" : context.Request["contractNo"].ToString();
            ht["contactContract"] = context.Request["contactContract"] == null ? "" : context.Request["contactContract"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["contactseller"] = context.Request["contactseller"] == null ? "" : context.Request["contactseller"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "0" : context.Request["buyer"].ToString();
            ht["contactbuyer"] = context.Request["contactbuyer"] == null ? "" : context.Request["contactbuyer"].ToString();
            ht["sendMan"] = context.Request["sendManText"] == null ? "" : context.Request["sendManText"].ToString();
            ht["revMan"] = context.Request["revManText"] == null ? "" : context.Request["revManText"].ToString();
            ht["isConsignor"] = context.Request["isConsignor"] == null ? "" : context.Request["isConsignor"].ToString();
            ht["isConsignee"] = context.Request["isConsignee"] == null ? "0" : context.Request["isConsignee"].ToString();
            ht["isContactConsignor"] = context.Request["isContactConsignor"] == null ? "" : context.Request["isContactConsignor"].ToString();
            ht["isContactConsignee"] = context.Request["isContactConsignee"] == null ? "" : context.Request["isContactConsignee"].ToString();
            ht["fromStation"] = context.Request["harborout"] == null ? "" : context.Request["harborout"].ToString();
            //ht["fromStationCode"] = context.Request["fromStationCode"] == null ? "" : context.Request["fromStationCode"].ToString();
            ht["destination"] = context.Request["harborarrive"] == null ? "" : context.Request["harborarrive"].ToString();
            //ht["destinationCode"] = context.Request["destinationCode"] == null ? "" : context.Request["destinationCode"].ToString();
            ht["carrierReport"] = context.Request["carrierReport"] == null ? "0" : context.Request["carrierReport"].ToString();
            ht["carriageCount"] = context.Request["carriageCount"] == null ? "0" : context.Request["carriageCount"].ToString();
            ht["containerSize"] = context.Request["containerSize"] == null ? "0" : context.Request["containerSize"].ToString();
            ht["salesman"] = context.Request["salesman"] == null ? "0" : context.Request["salesman"].ToString();
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createdate"] = DateTime.Now.ToString();
            ht["blockTrain"] = context.Request["blockTrain"] == null ? "0" : context.Request["blockTrain"].ToString();
            ht["countryChn"] = context.Request["countryChn"] == null ? "" : context.Request["countryChn"].ToString();
            ht["countryRus"] = context.Request["countryRus"] == null ? "" : context.Request["countryRus"].ToString();
            ht["palletRequireRus"] = context.Request["palletRequireRus"] == null ? "" : context.Request["palletRequireRus"].ToString();
            JsonHelper.hashTableFormat(ref ht);//转换特殊字符
            bool isOK = contractBll.editTrainDelivery(ref err, ht, trainFrontierStationTable, trainpayCodeTable, trainProductTable, ht["contractNo"].ToString(), ht["contactContract"].ToString(), createDateTag);
            return isOK;
        }
        private bool addTrainDelivery(ref string err, HttpContext context)
        {

            string status = context.Request.QueryString["status"] ?? string.Empty;
            //获取国境口岸站详情
            string trainFrontierStation = context.Request.Params["trainFrontierStation"];
            //获取发货申请唯一标识更新状态
            string createDateTag = context.Request.Params["createDateTag"] ?? string.Empty;
            //获取发货通知标识
            string preString = "FHTZ" + DateTime.Now.ToString("yyyMM");
            string noticeTag = context.Request.Params["noticeTag"] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(noticeTag))
            {
                noticeTag = preString + Util.getSequenceAutoAddZero(preString, 4);//时间戳区分批次
            }

            List<Hashtable> trainFrontierStationTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainFrontierStation);
            //获取付费代码详情
            string trainpayCode = context.Request.Params["trainpayCode"];
            List<Hashtable> trainpayCodeTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainpayCode);
            //获取产品详情
            string trainProduct = context.Request.Params["trainProduct"];
            List<Hashtable> trainProductTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainProduct);
            Hashtable ht = new Hashtable();
            //ht["id"] = context.Request["contractNo"] == null ? "" : context.Request["contractNo"].ToString();
            ht["createDateTag"] = createDateTag;
            ht["contractNo"] = context.Request["contractNo"] == null ? "" : context.Request["contractNo"].ToString();
            ht["contactContract"] = context.Request["contactContract"] == null ? "" : context.Request["contactContract"].ToString();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["contactseller"] = context.Request["contactseller"] == null ? "" : context.Request["contactseller"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["contactbuyer"] = context.Request["contactbuyer"] == null ? "" : context.Request["contactbuyer"].ToString();
            ht["sendMan"] = context.Request["sendManText"] == null ? "" : context.Request["sendManText"].ToString();
            ht["revMan"] = context.Request["revManText"] == null ? "" : context.Request["revManText"].ToString();
            ht["isConsignor"] = context.Request["isConsignor"] == null ? "" : context.Request["isConsignor"].ToString();
            ht["isConsignee"] = context.Request["isConsignee"] == null ? "" : context.Request["isConsignee"].ToString();
            ht["isContactConsignor"] = context.Request["isContactConsignor"] == null ? "" : context.Request["isContactConsignor"].ToString();
            ht["isContactConsignee"] = context.Request["isContactConsignee"] == null ? "" : context.Request["isContactConsignee"].ToString();
            ht["fromStation"] = context.Request["harborout"] == null ? "" : context.Request["harborout"].ToString();
            ht["fromStationCode"] = context.Request["harboroutcode"] == null ? "" : context.Request["harboroutcode"].ToString();
            ht["destination"] = context.Request["harborarrive"] == null ? "" : context.Request["harborarrive"].ToString();
            ht["destinationRemark"] = context.Request["destinationRemark"] == null ? "" : context.Request["destinationRemark"].ToString();
            ht["destinationCode"] = context.Request["harborarrivecode"] == null ? "" : context.Request["harborarrivecode"].ToString();
            ht["salesman"] = context.Request["salesman"] == null ? "" : context.Request["salesman"].ToString();
            ht["carrierReport"] = context.Request["carrierReport"] == null ? "0" : context.Request["carrierReport"].ToString();
            ht["carrierReportEng"] = context.Request["carrierReportEng"] == null ? "0" : context.Request["carrierReportEng"].ToString();
            ht["shipperReport"] = context.Request["shipperReport"] == null ? "0" : context.Request["shipperReport"].ToString();
            ht["shipperReportEng"] = context.Request["shipperReportEng"] == null ? "0" : context.Request["shipperReportEng"].ToString();
            ht["carrierReport"] = context.Request["carrierReport"] == null ? "0" : context.Request["carrierReport"].ToString();
            ht["carriageCount"] = context.Request["carriageCount"] == null ? "0" : context.Request["carriageCount"].ToString();
            ht["containerSize"] = context.Request["containerSize"] == null ? "0" : context.Request["containerSize"].ToString();
            ht["blockTrain"] = context.Request["blockTrain"] == null ? "0" : context.Request["blockTrain"].ToString();
            ht["packagesNumber"] = context.Request["packagesNumber"] == null ? "0" : context.Request["packagesNumber"].ToString();
            ht["palletRequire"] = context.Request["palletRequire"] == null ? "0" : context.Request["palletRequire"].ToString();
            ht["combineProduct"] = context.Request["combineProduct"] == null ? "0" : context.Request["combineProduct"].ToString();
            ht["productReport"] = context.Request["productReport"] == null ? "0" : context.Request["productReport"].ToString();
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createdate"] = DateTimeHelper.ShortDateTimeS;
            ht["noticeTag"] = noticeTag;
            ht["saveStatus"] = status;
            ht["countryChn"] = context.Request["countryChn"] == null ? "" : context.Request["countryChn"].ToString();
            ht["countryRus"] = context.Request["countryRus"] == null ? "" : context.Request["countryRus"].ToString();
            ht["palletRequireRus"] = context.Request["palletRequireRus"] == null ? "" : context.Request["palletRequireRus"].ToString();
            JsonHelper.hashTableFormat(ref ht);//转换特殊字符
            bool isOK = contractBll.addTrainDelivery(ref err, ht, trainFrontierStationTable, trainpayCodeTable, trainProductTable, ht["contractNo"].ToString(), ht["contactContract"].ToString(), createDateTag, status, noticeTag);
            return isOK;
        }

        private bool editCheck(ref string err, HttpContext context)
        {
            string checkId = context.Request.Params["checkId"];

            string strsql = @" update checkoutReport set businessGroup=@businessGroup,saleContract=@saleContract,purchaseContract=@purchaseContract,
buyer=@buyer, seller=@seller, isConsignor=@isConsignor,
isConsignee=@isConsignee, Consignor=@Consignor,Consignee=@Consignee, ConsignorReport=@ConsignorReport, 
fromStation=@fromStation, fromStationCode=@fromStationCode, destination=@destination, destinationCode=@destinationCode,  carrierReport=@carrierReport 
carriageCount=@carriageCount,containerSize=@containerSize  where checkId=@checkId";



            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=checkId,Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessGroup",Value=context.Request.Params["businessGroup"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@saleContract",Value=context.Request.Params["saleContract"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@purchaseContract",Value=context.Request.Params["purchaseContract"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isConsignor",Value=context.Request.Params["isConsignor"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isConsignee",Value=context.Request.Params["isConsignee"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@Consignor",Value=context.Request.Params["Consignor"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@Consignee",Value=context.Request.Params["Consignee"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ConsignorReport",Value=context.Request.Params["ConsignorReport"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@fromStation",Value=context.Request.Params["fromStation"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@fromStationCode",Value=context.Request.Params["fromStationCode"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@destination",Value=context.Request.Params["destination"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@destinationCode",Value=context.Request.Params["destinationCode"],Size=30},
//new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transitPayCode",Value=context.Request.Params["transitPayCode"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@carrierReport",Value=context.Request.Params["carrierReport"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@carriageCount",Value=context.Request.Params["carriageCount"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@containerSize",Value=context.Request.Params["containerSize"],Size=30},
};

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql, mms);

                    #region 国境口岸站修改
                    //先删除后添加
                    string deleteFrontier = "delete from checkFrontierStation where checkId=" + checkId;
                    bll.ExecuteNonQuery(deleteFrontier);

                    // 国境口岸站添加
                    string frontierSql = @" insert into checkFrontierStation(frontierStationCode,frontierStation,checkId)
values(@frontierStationCode,@frontierStation,@checkId);";
                    string trainFrontierStation = context.Request.Params["trainFrontierStation"];
                    List<Hashtable> trainFrontierStationTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainFrontierStation);
                    foreach (var item in trainFrontierStationTable)
                    {
                        SqlParameter[] frontierPms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@frontierStationCode",Value=item["frontierStationCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@frontierStation",Value=item["frontierStation"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=checkId,Size=8},
};
                        bll.ExecuteNonQuery(frontierSql, frontierPms);
                    }



                    #endregion

                    #region 付费代码修改
                    //先删除后添加
                    string deletePaycode = "delete from checkPayCode where checkId=" + checkId;
                    bll.ExecuteNonQuery(deletePaycode);


                    string paySql = @" insert into checkPayCode(payCode,transitPayCode,containerSize,checkId)
values(@payCode,@transitPayCode,@containerSize,@checkId);";
                    string trainpayCode = context.Request.Params["trainpayCode"];
                    List<Hashtable> trainpayCodeTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainpayCode);
                    foreach (var item in trainpayCodeTable)
                    {
                        SqlParameter[] payPms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@payCode",Value=item["payCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transitPayCode",Value=item["transitPayCode"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@containerSize",Value=item["containerSize"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=checkId,Size=8},
};
                        bll.ExecuteNonQuery(paySql, payPms);
                    }


                    #endregion

                    #region 产品修改
                    //先删除后添加
                    string deleteProduct = "delete from checkProduct where checkId=" + checkId;
                    bll.ExecuteNonQuery(deleteProduct);


                    string trainProduct = context.Request.Params["trainProduct"];
                    List<Hashtable> trainProductTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainProduct);
                    string proSql = @" insert into checkoutProduct(productName, packagesType, 
 packagesNumber, weight, checkId)
values(@productName, @packagesType, @packagesNumber, @weight, @checkId);";
                    foreach (var item in trainProductTable)
                    {
                        SqlParameter[] proPms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@productName",Value=item["productName"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packagesType",Value=item["packagesType"],Size=8},

new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packagesNumber",Value=item["packagesNumber"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@weight",Value=item["weight"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=checkId,Size=8},
};
                        bll.ExecuteNonQuery(proSql, proPms);
                    }

                    #endregion

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

        //根据合同号获取卖方，买方

        private string getPeople(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                SqlParameter[] pms = new SqlParameter[]{
                    new SqlParameter("@contractNo",contractNo)
                };
                DataTable dt = bll.ExecDatasetSql(@"select buyer,seller,pname,quantity from Econtract a  left join Econtract_ap b on a.contractNo=b.contractNo  where a.contractNo=@contractNo", pms).Tables[0];
                string sbPeople = jsonh.ToEasyUIComboxJson(dt).ToString();
                return sbPeople;

            }
        }

        private bool addCheck(ref string err, HttpContext context)
        {


            //添加sql,获取自增列的最后添加的id
            string strsql = @" insert into checkoutReport(businessGroup, saleContract, purchaseContract, buyer, seller, isConsignor,
isConsignee, Consignor,Consignee, ConsignorReport, fromStation, fromStationCode, destination, destinationCode, carrierReport,carriageCount,containerSize)
values(@businessGroup, @saleContract, @purchaseContract, @buyer, @seller, @isConsignor,
@isConsignee, @Consignor,@Consignee,@ConsignorReport, @fromStation, @fromStationCode,
@destination, @destinationCode, @carrierReport,@carriageCount,@containerSize);select @@IDENTITY";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessGroup",Value=context.Request.Params["businessGroup"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@saleContract",Value=context.Request.Params["saleContract"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@purchaseContract",Value=context.Request.Params["purchaseContract"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isConsignor",Value=context.Request.Params["isConsignor"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isConsignee",Value=context.Request.Params["isConsignee"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@Consignor",Value=context.Request.Params["Consignor"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@Consignee",Value=context.Request.Params["Consignee"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ConsignorReport",Value=context.Request.Params["ConsignorReport"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@fromStation",Value=context.Request.Params["fromStation"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@fromStationCode",Value=context.Request.Params["fromStationCode"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@destination",Value=context.Request.Params["destination"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@destinationCode",Value=context.Request.Params["destinationCode"],Size=30},
//new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transitPayCode",Value=context.Request.Params["transitPayCode"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@carrierReport",Value=context.Request.Params["carrierReport"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@carriageCount",Value=context.Request.Params["carriageCount"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@containerSize",Value=context.Request.Params["containerSize"],Size=30},
};

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    //获取铁路运输表自增列的id
                    int id = int.Parse(bll.ExecuteScalar(strsql, mms).ToString());



                    #region 国境口岸站添加
                    string frontierSql = @" insert into checkFrontierStation(frontierStationCode,frontierStation,checkId)
values(@frontierStationCode,@frontierStation,@checkId);";
                    string trainFrontierStation = context.Request.Params["trainFrontierStation"];
                    List<Hashtable> trainFrontierStationTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainFrontierStation);
                    foreach (var item in trainFrontierStationTable)
                    {
                        SqlParameter[] frontierPms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@frontierStationCode",Value=item["frontierStationCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@frontierStation",Value=item["frontierStation"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=id,Size=8},
};
                        bll.ExecuteNonQuery(frontierSql, frontierPms);
                    }
                    #endregion

                    #region 付费代码和过境付费代码添加

                    string paySql = @" insert into checkPayCode(payCode,transitPayCode,containerSize,checkId)
values(@payCode,@transitPayCode,@containerSize,@checkId);";
                    string trainpayCode = context.Request.Params["trainpayCode"];
                    List<Hashtable> trainpayCodeTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainpayCode);
                    foreach (var item in trainpayCodeTable)
                    {
                        SqlParameter[] payPms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@payCode",Value=item["payCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transitPayCode",Value=item["transitPayCode"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@containerSize",Value=item["containerSize"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=id,Size=8},
};
                        bll.ExecuteNonQuery(paySql, payPms);
                    }



                    #endregion


                    #region 产品添加
                    string trainProduct = context.Request.Params["trainProduct"];
                    List<Hashtable> trainProductTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(trainProduct);
                    string proSql = @" insert into checkoutProduct(productName, packagesType, 
 packagesNumber, weight, checkId)
values(@productName, @packagesType, @packagesNumber, @weight, @checkId);";
                    foreach (var item in trainProductTable)
                    {
                        SqlParameter[] proPms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@productName",Value=item["productName"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packagesType",Value=item["packagesType"],Size=8},

new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packagesNumber",Value=item["packagesNumber"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@weight",Value=item["weight"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=id,Size=8},
};
                        bll.ExecuteNonQuery(proSql, proPms);
                    }
                    #endregion


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

        private bool deleteCheck(ref string err, HttpContext context)
        {

            string checkId = context.Request.Params["checkId"];
            string reportSql = "delete from checkoutReport where checkId=@checkId";
            string productSql = "delete from checkoutProduct where checkId=@checkId";
            string paySql = "delete from checkPayCode where checkId=@checkId";
            string fromtierSql = "delete from checkFrontierStation where checkId=@checkId";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@checkId",checkId)
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(productSql, pms);
                    bll.ExecuteNonQuery(paySql, pms);
                    bll.ExecuteNonQuery(fromtierSql, pms);
                    bll.ExecuteNonQuery(reportSql, pms);

                    bll.SqlTran.Commit();
                    return true;

                }
                catch (Exception ex)
                {

                    err = ex.Message;
                    return false;
                }

            }

        }

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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}