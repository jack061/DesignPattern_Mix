using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WZX.Busines.Util
{
    public class ConstantUtil
    {
        #region 数据表f
        //基础数据表
        public const string TABLE_DICTRONARY = "BASE_DICTIONARY";//数据字典表
        public const string TABLE_BUS_PRODUCTHSS = "bproducthss";//海关产品信息
        public const string TABLE_BCUSTOMER_CONTACT = "bcustomer_contact";//通讯信息表
        public const string TABLE_BCUSTOMER_DELIVERY = "bcustomer_delivery";//收货信息表
        public const string TABLE_CUSTOMER = "bcustomer";//客户列表
        public const string TABLE_SUPPLIER = "bsupplier";//供应商列表
        public const string TABLE_SUPPLIER_CONTACT = "bsupplier_contact";//供应商通讯表
        public const string TABLE_DICTRONARY_1 = "bdicdate";//数据字典表(原)
        public const string TABLE_BTEMPLATE_CONTRACT = "btemplate_contract";//合同票据模板数据字典
        public const string TABLE_BRAILSHIPPERQUALI = "bRailShipperQuali";//铁路发货人资质
        public const string TABLE_INSUREDPRICE = "InsuredPrice";//货物保价表
        
        //合同
        public const string TABLE_ECONTRACT = "Econtract";//境外合同
        public const string TABLE_ECONTRACT_AP = "Econtract_ap";//境外合同产品表
        public const string TABLE_ECONTRACT_A = "Econtract_a";//境外合同附件
        public const string TABLE_ECONTRACT_A_Item = "Econtract_a";//合同附件模板条款
        public const string TABLE_ECONTRACT_SELLERCODE = "160912";//卖方编码
        public const string TABLE_ECONTRACT_TEMPLATE = "Econtract_template";//合同模板
        public const string TABLE_ECONTRACT_SPLIT = "Econtract_split";//合同批次表
        public const string TABLE_ECONTRACT_LOGISTICS = "Econtract_logistics";//物流合同表
        public const string TABLE_ECONTRACT_LOGISTICSFIRSTITEM = "Econtract_logisticsFirstItem";//物流合同表名
        public const string TABLE_ECONTRACT_LOGISTICSITEMS = "Econtract_logisticsItems";//物流合同条款
        public const string TABLE_ECONTRACT_INSPECT = "Econtract_Inspect";//商检合同列表
        public const string TABLE_ECONTRACT_INSPECT_TEMPLATE = "Econtract_Inspect_template";//商检合同模板
        public const string TABLE_ECONTRACT_INSPECT_AP = "Econtract_Inspect_ap";//商检合同产品列表
        public const string STATUS_INSPECTSENDFLOWDIRECTION = "发货通知";//进境合同创建发货通知业务流向
        public const string TABLE_ECONTRACT_SENDNOTICE = "Econtract_SendNotice";//进境发货通知表
        //服务合同
        public const string TABLE_ECONTRACT_SERVICE = "Econtract_Service";//服务合同表
        public const string TABLE_ECONTRACT_SERVICEITEMS = "Econtract_ServiceItems";//服务合同条款表
        public const string TABLE_ECONTRACT_SERVICECOSTCATEGORY = "Econtract_SerCostCategory";//服务合同费用类别表
        
      
        //内部结算单
        public const string TABLE_ECONTRACT_INTERNAL = "Econtract_Internal";//内部结算单
        public const string TABLE_ECONTRACT_INTERNAL_AP = "Econtract_Internal_ap";//内部结算单产品表
        //关联合同
        public const string STATUS_CONTACT_ED = "已创关联合同";//已创关联合同
        public const string STATUS_CONTACT_SEND = "已直接发货";//已直接发货
        //商检合同
        public const string STATUS_CONTRACTINSPECT_STATUS1 = "部分创建";
        public const string STATUS_CONTRACTINSPECT_STATUS2= "已创建";
        //合同审核
        public const string TABLE_REVIEWDATA = "reviewData";//合同审核表
        //合同模板
        public const string TABLE_BTEMPLATE_EXPORTENCONTRACT = "btemplate_exportEcontract";//出口合同模板
        public const string TABLE_BTEMPLATE_IMPORTENCONTRACT = "btemplate_importEcontract";//进口合同模板
        public const string TABLE_BTEMPLATE_LOGISTICS = "btemplate_logistics";//物流合同模板
        public const string TABLE_BTEMPLATE_LOGISTICSFIRSTITEM = "btemplate_logisticsFirstItem";//物流合同模板表头
        public const string TABLE_BTEMPLATE_LOGISTICSITEMS = "btemplate_logisticsItems";//物流合同模板条款
        //费用管理
        public const string TABLE_COSTMANAGER = "costManagerment";//境外合同
        //商检
        public const string TABLE_INSPECTION = "Inspection";
        public const string TABLE_INSPECTIONPRODUCT = "InspectionProduct";
        public const string TABLE_INSPECTIONRE = "InspectionRe";
        public const string TABLE_INSPECTIONREPACK = "InspectionRePack";
        public const string TABLE_INSPECTIONREPACK2 = "InspectionRePack2";
        public const string TABLE_PreviewManage = "previewManage";
        public const string TABLE_PREVIEWPACKMANAGE = "previewPackManage";
        public const string TABLE_PREVIEWCONCLUSIONMANAGE = "previewConclusionManage";
        public const string TABLE_INSPECTIONAPP = "InspectionApp";//商检申请
        public const string TABLE_INSPECTIONAPPDETAILS = "InspectionAppDetails";//商检申请详细表
        public const string TABLE_INSPECTIONCHECKDETAILS = "InspectionCheckDetails";//商检确定详细表
        public const string TABLE_INSPECTIONCHECK = "InspectionCheck";//商检确定表
        public const string TABLE_SENDOUTAPPDETAILS = "SendoutAppDetails";//发货申请
        public const string TABLE_ECONTRACT_INSPECT_SENDOUT = "Econtract_Inspect_sendout";//发货申请
        public const string TABLE_SENDOUTCHKDETAILS = "SendoutCHKDetails";//发货确认
        public const string TABLE_INSPECTION_TRAINCONFIRM = "Inspection_TrainConfirm";//铁路商检确认
        //铁路发货通知
        public const string VIEW_TRAINNOTICE = "trainNotice";//发货通知视图
        public const string TABLE_TRAINDELIVERYNOTICE = "trainDeliveryNotice";//发货通知表
        public const string TABLE_TRAINDILIVERYPRODUCT = "trainDeliveryProduct";//发货通知产品表
        public const string TABLE_TRAINDELPAYCODE = "trainDelPayCode";//发货通知付费代码表
        public const string TABLE_TRAINFROSTATION = "trainDelFroStation";//发货通知国境口岸站表

        //到款表
        public const string TABLE_PAY_DOMESTIC = "PayReceiveI";//境内到款表
        public const string TABLE_PAY_DOMESTIC_D = "PayReceiveIDetail";//境内到款表明细
        public const string TABLE_PAY_ABROAD = "PayReceiveE";//境外到款表
        public const string TABLE_PAY_ABROAD_D = "PayReceiveEDetail";//境外到款表明细
        public const string TABLE_PAY_Receive = "PayReceive";
        public const string TABLE_PAY_ReceiveDetails = "PayReceiveDetails";
        public const string TALBE_PAYCREDIT = "PayCredit";
        public const string TALBE_PAYCREDITDETAILS = "PayCreditDetails";
        public const string TABLE_PAYZXBAPPLY = "PayZXBApply";
        public const string TABLE_PAYZXB = "PayZXB";
        public const string TABLE_PAYZXBDETAILS = "PayZXBDetails";
        public const string TABLE_PAYACCEPT = "PayAccept";//承兑文件上传
        
        
        //财务表
        public const string TABLE_BANK_WATER = "Finbankwater";//银行流水
        public const string TABLE_BANK_WATER_D = "Finbankwaterd";//银行流水明细


        //请车管理
        public const string TABLE_TRAINAPPLYE = "TrainapplyE"; //基本信息
        public const string TABLE_TRAINAPPLYE1 = "TrainapplyE1";//车辆信息
        public const string TABLE_TRAINAPPLYE2 = "TrainapplyE2";//货物明细
        public const string TABLE_TRAINAPPLYE3 = "TrainapplyE3";//封印
        public const string TABLE_TRAINAPPLYE4 = "TrainapplyE4";//承运信息
        public const string STATUS_CHECKED = "已汇总";
        public const string STATUS_NEW = "新建";
        public const string TABLE_TRAINAPPLYINEW = "TrainapplyINew";
        public const string TABLE_TRAINAPPLYISUM = "TrainapplyISum";

        //报关单校对
        public const string TABLE_CUSTOMSDECLARATIONDETAILS = "CustomsDeclarationDetails";//报关单子表
        public const string TABLE_CUSTOMSDECLARATION = "CustomsDeclaration";//报关单主表

        //海运订舱
        public const string TABLE_CHECKOUTNOTICE = "CHECKOUTNOTICE";//海运订舱表
        public const string TABLE_CHECKOUTNOTICE_D = "CHECKOUTNOTICE_D";//海运订舱子表
        public const string VIEW_CheckOutNotice = "View_CheckOutNotice";//海运订舱视图

        //发货
        public const string TABLE_STOCKENTITYSWIFT = "StockEntitySwift";//仓库表
        public const string TABLE_STOCKOUTENTITY = "StockOutEntity";//发货表
        public const string TABLE_STOCKOUTENTITY_D = "StockOutEntity_D";//发货表-子表
        public const string VIEW_SENDAPPSTOCK = "View_SendAppStock";//发货通知视图
        public const string VIEW_SENDAPPSTOCK1 = "View_Stock";//发货通知视图

        public const string STATUS_APPLY_UN = "待订舱";//待订舱
        public const string STATUS_APPLY_ED = "已订舱";//已订舱

        public const string STATUS_HY_APPLY_UN = "待申请";//待申请
        public const string STATUS_HY_APPLY_ED = "已申请";//已申请
        public const string STATUS_HY_BACK = "退回";//退回

        public const string STATUS_DISTRIBUTE_UN = "待分配";//待分配
        public const string STATUS_DISTRIBUTE_BACK = "退回";//退回
        public const string STATUS_DISTRIBUTE_ED = "已分配";//已分配

        public const string STATUS_CONFIRM_UN = "待确认";//待确认
        public const string STATUS_CONFIRM_RECEIVE = "已接收";//已接收
        public const string STATUS_CONFIRM_ED = "已确认";//已确认

        public const string STATUS_SENDOUTNOTICE_UN = "待通知";//待通知
        public const string STATUS_SENDOUTNOTICE_ED = "已通知";//已通知



        #endregion

        #region 文件路径
        public const string FILE_CUSTOMER_URL = "";//客户信息上传路径
        public const string FILE_MODEL = "\\Files\\Model\\";//文件模板路径
        public const string FILE_FINANCE_URL = "\\Files\\FinanceFiles\\";//上传财务excel存储路径
        public const string FILE_ACCEPT_URL = "/upload/acceptance/";//承兑excel上传存储路径
        #endregion

        #region 模板编号
        public const string NoPackingInvoice = "1603-094";//箱单和发票头部模板编号
        public const string PriceList = "20161111";//价格单
        public const string NoInvoice = "5801";//报关发票
        #endregion

        #region 合同状态常量
        //public const string STATUS_STOCKIN_ABANDON= "废弃";//废弃
        public const string STATUS_STOCKIN_NEW = "新建";//入库新建
        public const string STATUS_STOCKIN_SUBMIT = "提交";//
        public const string STATUS_STOCKIN_CHECK = "待直线经理审核";//提交
        public const string STATUS_STOCKIN_CHECK1 = "审批通过";//销售总监审核
        public const string STATUS_STOCKIN_CHECK2 = "待合同管理员审核";//业务直线审核
        public const string STATUS_STOCKIN_CHECK3 = "待业务处主管审核";//合同管理员审核
        public const string STATUS_STOCKIN_CHECK4 = "待财务负责人审核";//业务处总监审核
        public const string STATUS_STOCKIN_CHECK5 = "待财务主管审核";//财务人员审核
        public const string STATUS_STOCKIN_CHECK6 = "待董事长审核";//财务总监审核
        public const string STATUS_STOCKIN_CHECK7 = "退回";//退回
        public const string STATUS_STOCKIN_CHECK8 = "业务员";//退回
        public const string STATUS_STOCKIN_CHECK9 = "直线经理审核";//退回
        public const string STATUS_STOCKIN_CHECK10 = "合同管理员审核";//退回
        public const string STATUS_STOCKIN_CHECK11= "业务处主管审核";//退回
        public const string STATUS_STOCKIN_CHECK12= "财务负责人审核";//退回
        public const string STATUS_STOCKIN_CHECK13= "财务主管审核";//退回
        public const string STATUS_STOCKIN_CHECK14= "董事长审核";//退回
   
        #endregion

        #region 废弃/中止合同状态常量
        public const string STATUS_ABANDON = "废弃";//废弃
        public const string STATUS_DISCONTINUE = "中止";
        public const string STATUS_CHECK1 = "废弃_待直线经理审核";
        public const string STATUS_CHECK2 = "废弃_待合同管理员审核";
        public const string STATUS_CHECK3= "废弃_待业务处主管审核";
        public const string STATUS_CHECK4 = "废弃_待财务负责人审核";
        public const string STATUS_CHECK5 = "废弃_待财务主管审核";
        public const string STATUS_CHECK6 = "废弃_待董事长审核";
        public const string STATUS_ABANDONPASS = "废弃_审批通过";
        public const string STATUS_DISCONTINUEPASS = "中止_审批通过";
        public const string STATUS_CHECK7 = "中止_待直线经理审核";
        public const string STATUS_CHECK8 = "中止_待合同管理员审核";
        public const string STATUS_CHECK9 = "中止_待业务处主管审核";
        public const string STATUS_CHECK10 = "中止_待财务负责人审核";
        public const string STATUS_CHECK11 = "中止_待财务主管审核";
        public const string STATUS_CHECK12 = "中止_待董事长审核";
        #endregion

        #region 角色或组名角色
        public const string ROLE_JOBMAN = "业务员";//业务员角色名
        public const string ROLE_SALESREVIEW = "直线经理";//直线经理
        public const string ORG_JOBMAN = "业务处";//组织业务处
        public const string ROLE_CONTRACTMAN = "合同管理员";//合同管理员
        #endregion

        #region 业务流向常量
        public const string IMPORT = "进境";//进境
        public const string EXPORT = "出境";//出境
        public const string LOGISTICS = "物流";//进境
        public const string INTERNALCLEARING = "内部清算单";
        public const string CLASSIFICIMPORT = "境内";//客户供应商境内经外
        public const string CLASSIFICEXPORT = "境外";
        #endregion

        #region 模板常量
        public const string TEMP_TRADEMENT = "@贸易条款@";
        public const string TEMP_TRADEMENTENG = "@1贸易条款@1";
        public const string TEMP_TRADEMENTRUS = "@2贸易条款@2";
        public const string TEMP_TRANSPORT = "@运输方式@";
        public const string TEMP_TRANSPORTENG = "@1运输方式@1";
        public const string TEMP_TRANSPORTRUS = "@2运输方式@2";
        public const string TEMP_OVERSPILL = "@溢出率@";
        public const string TEMP_OVERSPILLENG = "@1溢出率@1";
        public const string TEMP_OVERSPILLRUS = "@2溢出率@2";
        public const string TEMP_IMPORTHARBOR = "@进口口岸@";
        public const string TEMP_IMPORTHARBORENG = "@1进口口岸@1";
        public const string TEMP_IMPORTHARBORRUS = "@2进口口岸@2";
        public const string TEMP_ARRIVEHARBOR = "@到货口岸@";
        public const string TEMP_ARRIVEHARBORENG = "@1到货口岸@1";
        public const string TEMP_ARRIVEHARBORRUS = "@2到货口岸@2";
        public const string TEMP_PRICEMENT1 = "@价格条款1@";
        public const string TEMP_PRICEMENT1ENG = "@1价格条款1@1";
        public const string TEMP_PRICEMENT1RUS = "@2价格条款1@2";
        public const string TEMP_PRICEMENT2 = "@价格条款2@";
        public const string TEMP_PRICEMENT2ENG = "@1价格条款2@1";
        public const string TEMP_PRICEMENT2RUS = "@2价格条款2@2";
        public const string TEMP_PVALIDITY = "@价格有效期@";
        public const string TEMP_PVALIDITYENG = "@1价格有效期@1";
        public const string TEMP_PVALIDITYRUS = "@2价格有效期@2";
        public const string TEMP_VALIDITY = "@合同有效期@";
        public const string TEMP_VALIDITYENG = "@1合同有效期@1";
        public const string TEMP_VALIDITYRUS = "@2合同有效期@2";
        public const string TEMP_PAYLASTDATE = "@回款截止日@";
        public const string TEMP_PAYLASTDATEENG = "@1回款截止日@1";
        public const string TEMP_PAYLASTDATERUS = "@2回款截止日@2";
        public const string TEMP_PLACEMENT = "@产地条款@";
        public const string TEMP_PLACEMENTENG = "@1产地条款@1";
        public const string TEMP_PLACEMENTRUS = "@2产地条款@2";
        public const string TEMP_EXPORTHARBOR = "@出口口岸@";
        public const string TEMP_EXPORTHARBORENG = "@1出口口岸@1";
        public const string TEMP_EXPORTHARBORRUS = "@2出口口岸@2";
        public const string TEMP_SHIPMENT = "@发运条款@";
        public const string TEMP_SHIPMENTENG = "@1发运条款@1";
        public const string TEMP_SHIPMENTRUS = "@2发运条款@2";
        public const string TEMP_SHIPDATE = "@发运日期@";
        public const string TEMP_SHIPDATEENG = "@1发运日期@1";
        public const string TEMP_SHIPDATERUS = "@2发运日期@2";
        public const string TEMP_PAYMENTTYPE = "@付款方式@";
        public const string TEMP_PAYMENTTYPEENG = "@1付款方式@1";
        public const string TEMP_PAYMENTTYPERUS = "@2付款方式@2";
        public const string TEMP_PRODUCT = "@ProductSheet@";
        public const string TEMP_PRODUCTENG = "@1ProductSheet@1";
        public const string TEMP_PRODUCTRUS = "@2ProductSheet@2";
        #endregion

        #region 数据字典
        public const int PRICEMENTCODE = 64;//价格条款
        public const int PAYMENTCODE = 25;//付款方式
        public const String TRANSFORTRAIL = "铁路";//运输方式铁路
        public const String TRANSFORTSHIP = "海运";//运输方式海运
        #endregion
   
        #region 货币常量
        public const string CURRENCY_CNY = "CNY";//人民币
        public const string CURRENCY_UCD = "USD";//美元
        public const string CURRENCY_EUR = "EUR";//欧元
        public const string CURRENCY_JPY = "JPY";//日元
        public const string CURRENCY_GBP = "GBP";//英镑
        public const string CURRENCY_KRW = "KRW";//韩元
        public const string CURRENCY_HKD = "HKD";//港元
        public const string CURRENCY_AUD = "AUD";//澳元
        public const string CURRENCY_CAD = "CAD";//加元
        #endregion

        #region 日期常量
        public const string DATE_DAY = "日";

        #endregion

        #region 合同类型标识
        public const string CONTRACTTAG_MAINCON = "11";//进出境合同
        public const string CONTRACTTAG_SENDNOTICE = "22";//进境发货通知
        public const string CONTRACTTAG_SENDNOTICENEW = "222";//保存时进境发货通知
        public const string CONTRACTTAG_CONATTACH = "33";//框架合同附件
        public const string CONTRACTTAG_INTERNALTEMP = "44_a";//保存时内部结算单
        public const string CONTRACTTAG_INTERNAL = "44";//内部结算单
        public const string LOGINSTICSTAG = "55";//物流合同
        public const string CONTRACT_OUTSIDE = "66";//外部文本合同
        public const string CONTRACT_OUTSIDESAVE = "00";//保存时外部文本合同
        public const string CONTRACT_SERVICE = "88";//服务物流合同
        public const string CONTRACT_SERVICEATTACH = "88_0";//服务物流合同子合同
        public const string CONTRACTTAG_MAINCON_IMMEDIATE = "11_A";//直接发货时创建的关联合同标识
        #endregion

        #region 客户供应商编码
        public const string HK_OLD = "3200";
        public const string HK_NEW = "HK";
        #endregion
        public const String IFYES = "是";//状态是
        public const String IFNO = "否";//状态否

    }
}