using Ecan;
using RM.Busines;
using RM.Busines.btemplate;
using RM.Busines.contract;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Busines.Util;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetJson;
using RM.Web.Bus.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// contractData 的摘要说明
    /// </summary>
    public class contractData : IHttpHandler, IRequiresSessionState
    {
        RM.Busines.contract.contractBLL contractBll = new Busines.contract.contractBLL();
        JsonHelperEasyUi ui = new JsonHelperEasyUi();
        public void ProcessRequest(HttpContext context)
        {
            //从后台获取数据
            context.Response.ContentType = "application/json";
            string module = context.Request["module"];
            string suc = string.Empty;

            switch (module)
            {
                #region 获取列表
                case "htpagelist"://获取合同列表
                    suc = getContractList(context);
                    context.Response.Write(suc);
                    break;
                case "htcppagelist"://获取合同产品列表
                    suc = GetContractProductList(context);
                    context.Response.Write(suc);
                    break;
                case "getCplistByContact"://获取合同产品列表
                    suc = getCplistByContact(context);
                    context.Response.Write(suc);
                    break;
                case "costCategoryList"://获取服务合同费用列表
                    suc = costCategoryList(context);
                    context.Response.Write(suc);
                    break;
                case "htpagelistByHK"://获取关联合同列表，根据发货申请表获取
                    suc = GetContractListByHK(context);
                    context.Response.Write(suc);
                    break;
                case "cplist"://获取所有产品列表
                    suc = GetProductList(context);
                    context.Response.Write(suc);
                    break;

                case "sendoutProductList"://发货已申请合同产品列表
                    suc = sendoutProductList(context);
                    context.Response.Write(suc);
                    break;
                case "logisticsContractList"://获取物流合同列表
                    suc = logisticsContractList(context);
                    context.Response.Write(suc);
                    break;
                case "logisticsItems"://获取物流合同条款列表
                    suc = logisticsItems(context);
                    context.Response.Write(suc);
                    break;

                case "loadReviewData"://获取分批发货产品列表
                    suc = loadReviewData(context);
                    context.Response.Write(suc);
                    break;
                case "traincplist"://获取火车产品列表
                    suc = traincplist(context);
                    context.Response.Write(suc);
                    break;
                case "manageContractList"://获取管理合同列表
                    suc = manageContractList(context);
                    context.Response.Write(suc);
                    break;
                case "LoadManageData"://加载管理合同数据
                    suc = LoadManageData(context);
                    context.Response.Write(suc);
                    break;
                case "internalClearingList"://获取内部清算单列表
                    suc = internalClearingList(context);
                    context.Response.Write(suc);
                    break;
                case "getInternalContract"://创建内部清算单时要筛选的合同列表
                    suc = getInternalContract(context);
                    context.Response.Write(suc);
                    break;
                case "GetInternalProductList"://修改内部清算单产品列表
                    suc = GetInternalProductList(context);
                    context.Response.Write(suc);
                    break;
                case "createInterProduct"://创建内部清算单产品列表
                    suc = createInterProduct(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 获取模板列表
                case "GetImportTemplateList"://获取进口合同模板列表，根据创建人，模板类型筛选
                    suc = GetImportTemplateList(context);
                    context.Response.Write(suc);
                    break;
                case "getExportTemplate"://获取出口合同模板列表，根据创建人，模板类型筛选
                    suc = GetExportTemplateList(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 获取审核信息
                case "getReviewData"://获取审核信息
                    suc = GetReviewContractList(context);
                    context.Response.Write(suc);
                    break;
                case "GetServiceReviewList"://获取审核信息
                    suc = GetServiceReviewList(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 获取发货通知
                case "getCopyImportSendContract"://进境合同复制创建发货通知筛选创建过的发货通知
                    suc = getCopyImportSendContract(context);
                    context.Response.Write(suc);
                    break;
                case "getImportFrameInCon"://进境合同复制创建发货通知筛选创建过的发货通知
                    suc = getImportFrameInCon(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 服务合同
                case "serviceContractList"://获取服务合同列表
                    suc = serviceContractList(context);
                    context.Response.Write(suc);
                    break;
                case "getServiceContract"://获取新建服务子合同时根据时间业务员筛选框架合同
                    suc = getServiceContract(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 获取预览文本
                case "GetRealTimeContract"://获取实时合同预览文本
                    suc = GetRealTimeContract(context);
                    context.Response.Write(suc);
                    break;
                case "GetRealTimeInvoice"://获取实时箱单发票预览文本
                    suc = GetRealTimeInvoice(context);
                    context.Response.Write(suc);
                    break;
                case "GetLogisticsPreview"://获取实时物流合同预览文本
                    suc = GetLogisticsPreview(context);
                    context.Response.Write(suc);
                    break;
                case "GetServicePreview"://获取实时服务合同预览文本
                    suc = GetServicePreview(context);
                    context.Response.Write(suc);
                    break;
                case "GetRealTimeSendContract"://获取实时发货通知预览文本
                    suc = GetRealTimeSendContract(context);
                    context.Response.Write(suc);
                    break;
                case "GetInternalPreview"://获取实时内部结算单预览文本
                    suc = GetInternalPreview(context);
                    context.Response.Write(suc);
                    break;
                case "GetManagePreview"://获取管理合同实时预览文本
                    suc = GetManagePreview(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 筛选合同
                case "getCopyContract"://复制创建合同时筛选合同
                    suc = getCopyContract(context);
                    context.Response.Write(suc);
                    break;
                case "getContactContract"://进境关联创建合同时筛选合同
                    suc = getContactContract(context);
                    context.Response.Write(suc);
                    break;
                case "getLogisticsContract"://筛选物流合同列表
                    suc = getLogisticsContract(context);
                    context.Response.Write(suc);
                    break;
                case "getImportFrameAttachInCon"://独立创建下创建框架合同附件复制创建筛选框架下的子合同
                    suc = getImportFrameAttachInCon(context);
                    context.Response.Write(suc);
                    break;
                case "getLogisticsFrameInCon"://筛选物流合同框架合同下的子合同列表
                    suc = getLogisticsFrameInCon(context);
                    context.Response.Write(suc);
                    break;
                case "getContract"://新建合同时根据条件筛选合同
                    suc = getContract(context);
                    context.Response.Write(suc);
                    break;
                case "cotractByConManagerEdit"://筛选合同管理员自己的和属于合同管理员审核的合同
                    suc = cotractByConManagerEdit(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 其他
                case "CancelApply"://作废关联合同
                    context.Response.Write(CancelApply(context));
                    break;
                case "GetRailCheck":
                    context.Response.Write(GetRailCheck(context));
                    break;
                case "GetPrdutInfoRemain":
                    context.Response.Write(GetPrdutInfoRemain(context));
                    break;
                case "GetInsPrdutInfoRemain":
                    context.Response.Write(GetInsPrdutInfoRemain(context));
                    break;
                case "getPurchaseCode":  //获取其关联合同的合同号
                    context.Response.Write(getPurchaseCode(context));
                    break;
                case "getValidity":  //获取价格有效期日期加审核通过天数
                    context.Response.Write(getValidity(context));
                    break;
                //创建关联合同直接发货时初始化商检合同信息
                case "initInpectInfo":
                    suc = initInpectInfo(context);
                    context.Response.Write(suc);
                    break;
                //创建承兑上传excel文件列表
                case "getAcceptList":
                    suc = getAcceptList(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                #region 根据权限获取合同查询列表
                //依据角色查询出口合同
                case "getExportListByRoles":
                    suc = getExportListByRoles(context);
                    context.Response.Write(suc);
                    break;
                //依据角色查询进口合同
                case "getImportListByRoles":
                    suc = getImportListByRoles(context);
                    context.Response.Write(suc);
                    break;
                //依据角色查询服务合同
                case "getServiceListByRoles":
                    suc = getServiceListByRoles(context);
                    context.Response.Write(suc);
                    break;
                //依据角色查询内结合同
                case "getInternalListByRoles":
                    suc = getInternalListByRoles(context);
                    context.Response.Write(suc);
                    break;
                //依据角色查询管理合同
                case "getManageListByRoles":
                    suc = getManageListByRoles(context);
                    context.Response.Write(suc);
                    break;
                #endregion

                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;
            }

        }

        #region 筛选合同管理员自己的和属于合同管理员审核的合同
        private string cotractByConManagerEdit(HttpContext context)
        {
             int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string businessclass = context.Request.Params["businessclass"];
            string review = context.Request["review"];
            string flowdirection = context.Request["flowdirection"];
            StringBuilder sb = contractBll.GetConEditByConManager(contractNo, signedtime_begin, signedtime_end, row, page, order, sort, review, flowdirection, businessclass);
            return sb.ToString();
        } 
        #endregion


        #region 依据权限获取合同查询列表
        //进口合同查询列表
        private string getImportListByRoles(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string angencyName = string.Empty;
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");

            #region 查询条件
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
            sqlshere.Append(" and t1.flowdirection=@flowdirection");
            #endregion

            #region 获取用户所属角色,根据角色加载查询条件
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            string userName = RequestSession.GetSessionUser().UserName.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            //获取用户角色
            contractBll.getRolesCompanyByUserId(userId, ref rolesName, ref companyName);

            switch (rolesName)
            {
                case "业务员":
                    //只查看创建人为自己的合同
                    sqlshere.Append(" and t1.createman=@createman");
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "直线经理":
                    //查看其所属部门下所有业务员的合同
                    angencyName = contractBll.loadAngency(userName);
                    sqlshere.Append(" and t1.businessclass=@businessclass");
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "合同管理员":
                    //加载合同审核人为其的合同
                    sqlshere.Append(" and t1.adminReviewNumber=@adminReviewNumber");
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "业务处主管":
                    //加载全部
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "财务负责人":
                    //加载全部
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "财务主管":
                    //加载全部
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "董事长":
                    //加载全部
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                default:
                    break;
            }

            #endregion

            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                    new SqlParameter{ParameterName="@flowdirection",Value=ConstantUtil.IMPORT,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createman",Value=userAccount,DbType=DbType.String},
                    new SqlParameter{ParameterName="@businessclass",Value=angencyName,DbType=DbType.String},
                    new SqlParameter{ParameterName="@adminReviewNumber",Value=userAccount,DbType=DbType.String},
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        //出口合同查询列表
        private string getExportListByRoles(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string angencyName = string.Empty;
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");

            #region 查询条件
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
            sqlshere.Append(" and t1.flowdirection=@flowdirection");
            #endregion

            #region 获取用户所属角色,根据角色加载查询条件
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            string userName = RequestSession.GetSessionUser().UserName.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            //获取用户角色
            contractBll.getRolesCompanyByUserId(userId, ref rolesName, ref companyName);

            switch (rolesName)
            {
                case "业务员":
                    //只查看创建人为自己的合同
                    sqlshere.Append(" and t1.createman=@createman");
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "直线经理":
                    //查看其所属部门下所有业务员的合同
                    angencyName = contractBll.loadAngency(userName);
                    sqlshere.Append(" and t1.businessclass=@businessclass");
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "合同管理员":
                    //加载合同审核人为其的合同
                    sqlshere.Append(" and t1.adminReviewNumber=@adminReviewNumber");
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "业务处主管":
                    //加载全部
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "财务负责人":
                    //加载全部
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "财务主管":
                    //加载全部
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                case "董事长":
                    //加载全部
                    sqldata.Append(@" select *,t1.status as status1 from contractSearch_view t1 where " + sqlshere.ToString());
                    sqlcount.Append("select count(1) from contractSearch_view t1 where " + sqlshere.ToString());
                    break;
                default:
                    break;
            }

            #endregion

            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                    new SqlParameter{ParameterName="@flowdirection",Value=ConstantUtil.EXPORT,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createman",Value=userAccount,DbType=DbType.String},
                    new SqlParameter{ParameterName="@businessclass",Value=angencyName,DbType=DbType.String},
                    new SqlParameter{ParameterName="@adminReviewNumber",Value=userAccount,DbType=DbType.String},
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();

        }
        //管理合同查询列表
        private string getManageListByRoles(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string angencyName = string.Empty;
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();

            #region 查询条件
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
        
            #endregion

            #region 获取用户所属角色,根据角色加载查询条件
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            string userName = RequestSession.GetSessionUser().UserName.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            //获取用户角色
            contractBll.getRolesCompanyByUserId(userId, ref rolesName, ref companyName);

            switch (rolesName)
            {
                case "业务员":
                    //只查看创建人为自己的合同
                    sqlshere.Append(" and t1.createman=@createman");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    break;
                case "直线经理":
                    //查看其所属部门下所有业务员的合同
                    angencyName = contractBll.loadAngency(userName);
                    sqlshere.Append(" and t1.businessclass=@businessclass");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    break;
                case "合同管理员":
                    //加载合同审核人为其的合同
                    sqlshere.Append(" and t1.adminReviewNumber=@adminReviewNumber");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    break;
                case "业务处主管":
                    //加载全部
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    break;
                case "财务负责人":
                    //加载全部
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    break;
                case "财务主管":
                    //加载全部
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    break;
                case "董事长":
                    //加载全部
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                    break;
                default:
                    break;
            }

            #endregion

            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createman",Value=userAccount,DbType=DbType.String},
                    new SqlParameter{ParameterName="@businessclass",Value=angencyName,DbType=DbType.String},
                    new SqlParameter{ParameterName="@adminReviewNumber",Value=userAccount,DbType=DbType.String},
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }

        //服务合同查询列表
        private string getServiceListByRoles(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string angencyName = string.Empty;
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();

            #region 查询条件
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
          
            #endregion

            #region 获取用户所属角色,根据角色加载查询条件
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            string userName = RequestSession.GetSessionUser().UserName.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            //获取用户角色
            contractBll.getRolesCompanyByUserId(userId, ref rolesName, ref companyName);

            switch (rolesName)
            {
                case "业务员":
                    //只查看创建人为自己的合同
                    sqlshere.Append(" and t1.createman=@createman");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    break;
                case "直线经理":
                    //查看其所属部门下所有业务员的合同
                    angencyName = contractBll.loadAngency(userName);
                    sqlshere.Append(" and t1.businessclass=@businessclass");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    break;
                case "合同管理员":
                    //加载合同审核人为其的合同
                    sqlshere.Append(" and t1.adminReviewNumber=@adminReviewNumber");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    break;
                case "业务处主管":
                    //加载全部
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    break;
                case "财务负责人":
                    //加载全部
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    break;
                case "财务主管":
                    //加载全部
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    break;
                case "董事长":
                    //加载全部
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    break;
                default:
                       sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                    break;
            }

            #endregion

            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                    new SqlParameter{ParameterName="@createman",Value=userAccount,DbType=DbType.String},
                    new SqlParameter{ParameterName="@businessclass",Value=angencyName,DbType=DbType.String},
                    new SqlParameter{ParameterName="@adminReviewNumber",Value=userAccount,DbType=DbType.String},
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }

        //内接合同查询列表

        private string getInternalListByRoles(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string angencyName = string.Empty;
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();

            #region 查询条件
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
         
            #endregion

            #region 获取用户所属角色,根据角色加载查询条件
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userId = RequestSession.GetSessionUser().UserId.ToString();
            string userName = RequestSession.GetSessionUser().UserName.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            //获取用户角色
            contractBll.getRolesCompanyByUserId(userId, ref rolesName, ref companyName);

            switch (rolesName)
            {
                case "业务员":
                    //只查看创建人为自己的合同
                    sqlshere.Append(" and t1.createman=@createman");
                    sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    break;
                case "直线经理":
                    //查看其所属部门下所有业务员的合同
                    angencyName = contractBll.loadAngency(userName);
                    sqlshere.Append(" and t1.businessclass=@businessclass");
                    sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    break;
                case "合同管理员":
                    //加载合同审核人为其的合同
                    sqlshere.Append(" and t1.adminReviewNumber=@adminReviewNumber");
                    sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    break;
                case "业务处主管":
                    //加载全部
                    sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    break;
                case "财务负责人":
                    //加载全部
                    sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    break;
                case "财务主管":
                    //加载全部
                    sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    break;
                case "董事长":
                    //加载全部
                    sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    break;
                default:
                       sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
                    sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                    break;
            }

            #endregion

            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                    new SqlParameter("@contractTag",ConstantUtil.CONTRACTTAG_INTERNAL),
                    new SqlParameter("@contractTagTemp",ConstantUtil.CONTRACTTAG_INTERNALTEMP),
                    new SqlParameter{ParameterName="@createman",Value=userAccount,DbType=DbType.String},
                    new SqlParameter{ParameterName="@businessclass",Value=angencyName,DbType=DbType.String},
                    new SqlParameter{ParameterName="@adminReviewNumber",Value=userAccount,DbType=DbType.String},
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }



        #endregion

        #region 创建内部清算单产品列表
        private string createInterProduct(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string buyer = context.Request.Params["buyer"];
            string seller = context.Request.Params["seller"];
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();

            sqldata.Append(@"select t1.spec, t1.contractNo, t1.pcode,t1.pname,t1.quantity,t1.qunit,isnull(t2.price,0) as price,
t2.priceUnit from Econtract_ap t1 left join bproduct_price t2 on t2.buyer=@buyer and t2.seller=@seller and t1.pcode=t2.pcode
   where t1.contractNo=@contractNo");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
                    new SqlParameter("@buyer",buyer),
                    new SqlParameter("@seller",seller),
                };
            DataTable dt = new DataTable();

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 获取修改内部结算单产品列表
        public string GetInternalProductList(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            sqldata.AppendFormat(" select * from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_INTERNAL_AP);
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
                 
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 获取预览文本

        #region 获取管理合同实时预览文本
        private string GetManagePreview(HttpContext context)
        {
            string buyerCode = context.Request.Params["buyerCode"];
            string language = context.Request.Params["language"];
            string sellerCode = context.Request.Params["sellerCode"];
            string contractText = context.Request.Params["contractText"];
            string signedTime = context.Request.Params["signedTime"];
            string signedPlace = context.Request.Params["signedPlace"];
            string validity = context.Request.Params["validity"];
            string contractNo = context.Request.Params["contractNo"];
            string buyer = context.Request.Params["buyer"];//甲方
            string seller = context.Request.Params["seller"];
            string ItemName = context.Request.Params["ItemName"];
            string ItemAmount = context.Request.Params["ItemAmount"];
            string itemProName = context.Request.Params["itemProName"];

            string title = string.Empty;
            string bottom = string.Empty;
            string bulling = string.Empty;
            string bankMessage = string.Empty;
            string tableName = string.Empty;
            string customerClassfic = string.Empty;

            StringBuilder sb = new StringBuilder();

            //获取模板详细
            System.Data.DataTable dt = new System.Data.DataTable();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //获取表头表尾
                title = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表头' and templateno='7290'").ToString();
                bottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾'  and templateno='7290'").ToString();
                bulling = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='开票信息'  and templateno='7290'").ToString();

            }

            #region 替换模板变量
            //替换变量
            Hashtable htdata = new Hashtable();
            htdata["不限:合同编号"] = contractNo;
            if (!string.IsNullOrEmpty(signedTime))
            {
                htdata["中文:签订时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
                htdata["英文:签订时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
            }

            htdata["中文:签订地点"] = signedPlace.ToString();
            htdata["中文:合同有效期"] = validity.ToString();
            htdata["中文:甲方"] = buyer.ToString();
            htdata["中文:乙方"] = seller.ToString();
            htdata["中文:项目名称"] = ItemName.ToString();
            htdata["中文:项目金额"] = ItemAmount.ToString();
            //htdata["中文:项目(生产)名称"] = itemProName.ToString();

            #endregion

            #region 获取卖方买方信息
            //卖方信息，买方信息
            DataSet ds22 = new DataSet();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
             
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyerCode),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellerCode)
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            if (ds22.Tables[0].Rows.Count > 0)
            {
                DataRow dr1 = ds22.Tables[0].Rows[0];
                htdata["俄文:买方名"] = dr1["rsname"].ToString();
                htdata["英文:买方名"] = dr1["egname"].ToString();
                htdata["中文:买方名"] = dr1["name"].ToString();

                htdata["俄文:买方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:买方地址"] = dr1["egaddress"].ToString();
                htdata["中文:买方地址"] = dr1["address"].ToString();
                htdata["中文:开户银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:开户行名称"] = dr1["iegbank"].ToString();
                htdata["中文:开户银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:开户行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["中文:开户银行账号"] = dr1["icnaccount"].ToString();
                htdata["英文:开户银行号"] = dr1["iegaccount"].ToString();
                customerClassfic = dr1["classific"].ToString();
            }
            if (ds22.Tables[1].Rows.Count > 0)
            {
                DataRow dr1 = ds22.Tables[1].Rows[0];
                htdata["俄文:卖方名"] = dr1["rsname"].ToString();
                htdata["英文:卖方名"] = dr1["egname"].ToString();
                htdata["中文:卖方名"] = dr1["name"].ToString();
                htdata["俄文:卖方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:卖方地址"] = dr1["egaddress"].ToString();
                htdata["中文:卖方地址"] = dr1["address"].ToString();
                htdata["英文:收款人银行名称"] = dr1["iegbank"].ToString();
                htdata["中文:收款人银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:收款人银行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:收款人银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:收款人银行账号"] = dr1["iegaccount"].ToString();
                htdata["中文:收款人银行账号"] = dr1["icnaccount"].ToString();
                htdata["中文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["英文:收款人银行行号"] = dr1["icncreditcode"].ToString();
            }
            //客户
            if (ds22.Tables[2].Rows.Count > 0)
            {
                DataRow drcus = ds22.Tables[2].Rows[0];
                htdata["不限:买方电话"] = drcus["phone"].ToString();
            }
            //供应商
            if (ds22.Tables[3].Rows.Count > 0)
            {
                DataRow drsup = ds22.Tables[3].Rows[0];
                htdata["不限:卖方电话"] = drsup["phone"].ToString();
            }
            #endregion

            #region 替换模板常量
            htdata.Add("中文:合同号", "合同号");
            htdata.Add("英文:合同号", "Contract No.");

            htdata.Add("中文:日期", "日期");
            htdata.Add("英文:日期", "Date");

            htdata.Add("中文:合同", "合同");
            htdata.Add("英文:合同", "CONTRACT");
            htdata.Add("俄文:合同", "КОНТРАКТ");

            htdata.Add("俄文:卖方", "ПРОДАВЕЦ:");
            htdata.Add("英文:卖方", "The Seller:");
            htdata.Add("中文:卖方", "卖方:");

            htdata.Add("中文:银行", "银行:");
            htdata.Add("俄文:银行", "Банк:");

            htdata.Add("俄文:买方", "ПОКУПАТЕЛЬ:");
            htdata.Add("中文:买方", "买方:");
            htdata.Add("英文:买方", "The Buyer: ");

            htdata.Add("俄文:地址", "Реквизиты:");
            htdata.Add("中文:地址", "地址:");
            htdata.Add("英文:地址", "ADD:");
            #endregion

            #region 拼接表格

            #endregion

            string lans = "中文";
            //先添加表头
            StringBuilder titleSb = new StringBuilder(title);
            InsteadLabelString.Singleton.InsteadStringBuilder(titleSb, htdata, lans);
            string title1 = titleSb.ToString().Replace("\t", "").Replace("\r\n", "").Replace("<br />", "");
            title1 = title1.Replace("<p></p>", "");
            sb.AppendLine(title1.ToString());
            sb.Append("<br/>");
            sb.AppendLine(contractText.ToString());
            if (customerClassfic == "境内")
            {
                StringBuilder bottomSb = new StringBuilder(bottom);
                InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
                sb.AppendLine(bottomSb.ToString());
            }
            else
            {
                StringBuilder bullingSb = new StringBuilder(bulling);
                InsteadLabelString.Singleton.InsteadStringBuilder(bullingSb, htdata, lans);
                sb.AppendLine(bullingSb.ToString());
            }
            return sb.ToString();
        }
        #endregion

        #region 获取实时箱单发票预览文本
        private string GetRealTimeInvoice(HttpContext context)
        {
            RM.Busines.IDAO.ITemplateDB templateDb = new RM.Busines.DAL.TempldateDB();

            string buyer = context.Request.Params["buyer"];
            string seller = context.Request.Params["seller"];
            string buyername = context.Request.Params["buyername"];
            string sellername = context.Request.Params["sellername"];
            string signedPlace = context.Request.Params["signedPlace"];
            string signedTime = context.Request.Params["signedTime"];
            string templateno = context.Request.Params["templateno"];
            string language = context.Request.Params["language"];
            string validity = context.Request.Params["validity"] ?? string.Empty;
            string productlist = context.Request.Params["productlist"];
            string tradement = context.Request.Params["tradement"] ?? string.Empty;
            string transport = context.Request.Params["transport"] ?? string.Empty;
            string harborout = context.Request.Params["harborout"] ?? string.Empty;
            string harborarrive = context.Request.Params["harborarrive"] ?? string.Empty;
            string delivery = context.Request.Params["deliveryPlace"] ?? string.Empty;
            string pricement1 = context.Request.Params["pricement1"] ?? string.Empty;
            string pricement2 = context.Request.Params["pricement2"] ?? string.Empty;
            string pvalidity = context.Request.Params["pvalidity"] ?? string.Empty;
            string shipment = context.Request.Params["shipment"] ?? string.Empty;
            string placement = context.Request.Params["placement"] ?? string.Empty;
            string currency = context.Request.Params["currency"] ?? string.Empty;
            string no_ = context.Request.Params["contractNo"] ?? string.Empty; ;
            string type_ = string.IsNullOrEmpty(context.Request.Params["type"]) ? "" : context.Request.Params["type"];
            string head = string.Empty;
            string which = type_;
            string contractNo = no_;
            StringBuilder sb = new StringBuilder();
            Hashtable hashTable = new Hashtable();
            DataSet ds22 = new DataSet();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                System.Data.SqlClient.SqlParameter[] mms = new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@templateno",ConstantUtil.NoPackingInvoice)
                };
                //获取箱单模板
                head = bll.ExecuteScalar(string.Format(@"select content from {0} where templateno='{1}'", ConstantUtil.TABLE_BTEMPLATE_CONTRACT, ConstantUtil.NoPackingInvoice)).ToString();
                SqlParameter[] pms = new SqlParameter[]{
                   new SqlParameter("@buyer",buyer),
                       new SqlParameter("@seller",seller),
               };
                buyername = bll.ExecuteScalar(@"select code from bcustomer where name=@buyer", pms).ToString();
                sellername = bll.ExecuteScalar(@"select code from bsupplier where name=@seller", pms).ToString();

            }
            //获取合同信息
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                //sb22.Append(" select * from Econtract_ap where contractNo=@contractno and attachmentno=''; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
                   //new  System.Data.SqlClient.SqlParameter("@contractno",contractNo),
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyername),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellername)
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            //DataTable contractTable = dtContract.Tables[0];//合同表
            //DataTable contractAPTable = dtContract.Tables[1];//产品表
            DataTable buyerInfoTable = ds22.Tables[0];//买方信息
            DataTable sellerInfoTable = ds22.Tables[1];//卖方信息

            string[] buyerInfo = null;
            string[] sellerInfo = null;
            string[] headTitle = null;
            string[] buyOrSell = null;
            string[] timeInvoiceContract = null;//时间\发票号\合同号语言
            string totalCount = string.Empty;
            GetHeadLanguage(language, sellerInfoTable, buyerInfoTable, ref buyerInfo, ref sellerInfo, ref headTitle, ref buyOrSell,
               ref timeInvoiceContract, ref totalCount);
            hashTable["{COMPANY:公司-名称-外文}"] = sellerInfo[2];//contractNo.Contains("GY") ? buyerInfo[2] : sellerInfo[2];//公司-名称-外文
            hashTable["{COMPANY:公司-地址-外文}"] = sellerInfo[3];//contractNo.Contains("GY") ? buyerInfo[3] : sellerInfo[3];//公司-地址-外文
            hashTable["{COMPANY:公司-名称-中文}"] = sellerInfo[0]; //contractNo.Contains("GY") ? buyerInfo[0] : sellerInfo[0];//公司-名称-中文
            hashTable["{COMPANY:公司-地址-中文}"] = sellerInfo[1]; //contractNo.Contains("GY") ? buyerInfo[1] : sellerInfo[1];//公司-地址-中文
            hashTable["{TYPE:箱单发票}"] = which.Equals("invoice") || which.Equals("other") ? headTitle[0] : headTitle[1];//箱单发票
            hashTable["{发票编号}"] = which.Equals("other") ? GetSeqNo() : "";
            hashTable["{TIME:时间}"] = signedTime;//签订时间
            hashTable["{NO:发票号}"] = contractNo;//发票号
            hashTable["{NO:合同号}"] = contractNo;//合同号
            hashTable["{BUYER:买方-中文}"] = buyerInfo[0]; //contractNo.Contains("GY") ? sellerInfo[0] : buyerInfo[0];//买(卖)家名称
            hashTable["{BUYER:买方-外文}"] = buyerInfo[2]; //contractNo.Contains("GY") ? sellerInfo[2] : buyerInfo[2];//买(卖)家名称
            hashTable["{BUYER:SELLER}"] = buyOrSell[1]; //contractNo.Contains("GY") ? buyOrSell[0] : buyOrSell[1];
            hashTable["{BUYER:买方-外文}"] = hashTable["{BUYER:SELLER}"].Equals(" ") ? " " : hashTable["{BUYER:SELLER}"] + ":" + hashTable["{BUYER:买方-外文}"];
            //hashTable["{INFO:简要信息}"] = "ADDRESS:" + buyerInfo[3]; //hashTable["{BUYER:SELLER}"].Equals(" ") ? " " : contractNo.Contains("GY") ? sellerInfo[3] : buyerInfo[3];//买(卖)家地址
            hashTable["{INFO:简要信息}"] = "";
            hashTable["{买方:卖方}"] = "买方"; //contractNo.Contains("GY") ? "卖方" : "买方";
            //hashTable["{BUYER:买方-中文}"] = hashTable["{买方:卖方}"] + ":" + hashTable["{BUYER:买方-中文}"];
            string address = "<br/> " + "地址:" + (contractNo.Contains("GY") ? sellerInfo[1] : buyerInfo[1]);//买(卖)家地址;
            hashTable["{BUYER:买方-中文}"] = hashTable["{BUYER:买方-中文}"].ToString() == "" ? "" : (hashTable["{买方:卖方}"] + ":" + hashTable["{BUYER:买方-中文}"]) + address;
            hashTable["{DATE 时间}"] = timeInvoiceContract[0];
            hashTable["{INVOICE No发票号}"] = timeInvoiceContract[1];
            hashTable["{CONTRACT No合同号}"] = timeInvoiceContract[2];

            head = head.Replace("{COMPANY:公司-名称-外文}", hashTable["{COMPANY:公司-名称-外文}"].ToString());
            head = head.Replace("{COMPANY:公司-地址-外文}", hashTable["{COMPANY:公司-地址-外文}"].ToString());
            head = head.Replace("{COMPANY:公司-名称-中文}", hashTable["{COMPANY:公司-名称-中文}"].ToString());
            head = head.Replace("{COMPANY:公司-地址-中文}", hashTable["{COMPANY:公司-地址-中文}"].ToString());
            head = head.Replace("{TYPE:箱单发票}", hashTable["{TYPE:箱单发票}"].ToString());
            head = head.Replace("{发票编号}", hashTable["{发票编号}"].ToString());
            head = head.Replace("{TIME:时间}", hashTable["{TIME:时间}"].ToString());
            head = head.Replace("{NO:发票号}", hashTable["{NO:发票号}"].ToString());
            head = head.Replace("{NO:合同号}", hashTable["{NO:合同号}"].ToString());
            head = head.Replace("{BUYER:买方-外文}", hashTable["{BUYER:买方-外文}"].ToString());
            head = head.Replace("{INFO:简要信息}", hashTable["{INFO:简要信息}"].ToString());
            head = head.Replace("{BUYER:买方-中文}", hashTable["{BUYER:买方-中文}"].ToString());

            head = head.Replace("{DATE 时间}", hashTable["{DATE 时间}"].ToString());
            head = head.Replace("{INVOICE No发票号}", hashTable["{INVOICE No发票号}"].ToString());
            head = head.Replace("{CONTRACT No合同号}", hashTable["{CONTRACT No合同号}"].ToString());

            sb.Append(head);

            if (which.Equals("invoice") || which.Equals("other"))//发票处理
            {

                string uperTable = GetTableUpSection(ref ds22, tradement, harborout, harborarrive, transport, pricement1, language);

                int sumCnt;

                string table = GetInvoiceTable(productlist, currency, tradement, out sumCnt, language);
                uperTable += string.Format(totalCount, sumCnt);
                sb.Append(uperTable);
                sb.Append(table);

                if (which.Equals("other"))
                {
                    sb.Append(string.Format("<p align='center'><span style='font-size:15px;'>{0}</span></p><p align='center'><span style='font-size:15px;'>{1}</span></p>",
                                    contractNo.Contains("GY") ? buyerInfo[2] : sellerInfo[2],
                                    contractNo.Contains("GY") ? buyerInfo[0] : sellerInfo[0]));
                }
            }
            else
            { //箱单处理
                sb.Append(GetPackingTable(sb, productlist, language));
            }
            return sb.ToString();
        }
        #endregion

        #region 获取发货通知实时预览文本

        private string GetRealTimeSendContract(HttpContext context)
        {

            string contractNo = context.Request.Params["contractNo"] ?? string.Empty;
            string buyer = context.Request.Params["buyer"] ?? string.Empty;
            string seller = context.Request.Params["seller"] ?? string.Empty;
            string buyercode = context.Request.Params["buyercode"] ?? string.Empty;
            string sellercode = context.Request.Params["sellercode"] ?? string.Empty;
            string sendTime = context.Request.Params["sendTime"] ?? string.Empty;
            string noticeTime = context.Request.Params["noticeTime"] ?? string.Empty;
            string items = context.Request.Params["items"] ?? string.Empty;
            string productlist = context.Request.Params["productlist"];

            string title = string.Empty;
            string bottom = string.Empty;
            string bankMessage = string.Empty;
            string customerClassfic = string.Empty;
            string supplierClassfic = string.Empty;
            string bulling = string.Empty;
            StringBuilder sb = new StringBuilder();

            //获取合同详细
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtContract = new System.Data.DataTable();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {

                title = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='发货通知'  and templateno='0603'").ToString();
                bottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾' and templateno='0603'").ToString();
                bankMessage = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='银行信息'").ToString();
                bulling = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='开票信息' and templateno='0603'").ToString();
            }

            Hashtable htdata = new Hashtable();
            DataSet ds22 = new DataSet();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                //sb22.Append(" select * from Econtract_ap where contractNo=@contractno and attachmentno=''; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
                   //new  System.Data.SqlClient.SqlParameter("@contractno",contractNo),
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyercode),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellercode)
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            if (ds22.Tables[0].Rows.Count > 0)
            {
                //客户
                DataRow dr1 = ds22.Tables[0].Rows[0];
                htdata["俄文:买方名"] = dr1["rsname"].ToString();
                htdata["英文:买方名"] = dr1["egname"].ToString();
                htdata["中文:买方名"] = dr1["name"].ToString();
                htdata["俄文:买方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:买方地址"] = dr1["egaddress"].ToString();
                htdata["中文:买方地址"] = dr1["address"].ToString();
                htdata["中文:客户地址及电话"] = dr1["icnaddress"].ToString() + dr1["icnphone"].ToString();
                htdata["英文:客户地址及电话"] = dr1["iegaddress"].ToString() + dr1["iegphone"].ToString();
                htdata["中文:开户银行"] = dr1["icnbank"].ToString();
                htdata["英文:开户银行"] = dr1["iegbank"].ToString();
                htdata["中文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["中文:开户银行账号"] = dr1["icnaccount"].ToString();
                htdata["英文:开户银行号"] = dr1["iegaccount"].ToString();
                customerClassfic = dr1["classific"].ToString();
            }
            if (ds22.Tables[1].Rows.Count > 0)
            {
                //供应商
                DataRow dr1 = ds22.Tables[1].Rows[0];
                htdata["俄文:卖方名"] = dr1["rsname"].ToString();
                htdata["英文:卖方名"] = dr1["egname"].ToString();
                htdata["中文:卖方名"] = dr1["name"].ToString();

                htdata["俄文:卖方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:卖方地址"] = dr1["egaddress"].ToString();
                htdata["中文:卖方地址"] = dr1["address"].ToString();
                htdata["英文:收款银行名称"] = dr1["iegbank"].ToString();
                htdata["中文:收款银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:收款人银行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:收款人银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:收款人银行账号"] = dr1["iegaccount"].ToString();
                htdata["中文:收款人银行账号"] = dr1["icnaccount"].ToString();
                htdata["中文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["英文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["中文:卖方信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:卖方信用代码"] = dr1["icncreditcode"].ToString();
                supplierClassfic = dr1["classific"].ToString();
            }
            //客户
            if (ds22.Tables[2].Rows.Count > 0)
            {
                DataRow drcus = ds22.Tables[2].Rows[0];
                htdata["不限:买方电话"] = drcus["phone"].ToString();
            }
            //供应商
            if (ds22.Tables[3].Rows.Count > 0)
            {
                DataRow drsup = ds22.Tables[3].Rows[0];
                htdata["不限:卖方电话"] = drsup["phone"].ToString();
            }
            htdata["中文:通知时间"] = noticeTime;
            htdata["中文:发货时间"] = sendTime;
            htdata["不限:合同编号"] = contractNo.ToString();
            htdata["中文:卖方名"] = seller.ToString();
            htdata["中文:买方名"] = buyer.ToString();
            string lans = "中文";
            //先添加表头
            StringBuilder titleSb = new StringBuilder(title);
            InsteadLabelString.Singleton.InsteadStringBuilder(titleSb, htdata, lans);
            string title1 = titleSb.ToString().Replace("\t", "").Replace("\r\n", "");
            title1 = title1.Replace("<p></p>", "");
            sb.AppendLine(title1.ToString());

            #region 生成产品明细表
            string productTitle = "";
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(productlist);
            ////动态生成产品表格
            //DataTable dtproduct = ds22.Tables[2];
            StringBuilder sbpro = new StringBuilder();
            string th0 = "";
            string th1 = "";
            string th2 = "";
            string th3 = "";
            string th4 = "";
            string th5 = "";
            string th6 = "";
            //string th6 = "";
            //string th7 = "";
            string th8 = "";
            //string th9 = "";

            if (lans.Contains("英"))
            {
                productTitle = "ProductDetails<br/>";
                th0 = "DESCRIPTION<br/>";
                th1 = "UNIT<br/>";
                th2 = "QUANTITY<br/>";
                th3 = "UNIT PRICE<br/>";
                th4 = "PRICEUNIT<br/>";
                th5 = "AMOUNT<br/>";

            }
            if (lans.Contains("俄"))
            {
                productTitle = "Список продуктов<br/> ";
                th0 = "Описание товаров <br/>";
                th1 = " единица  <br/>";
                th2 = "количество <br/>";
                th3 = " цена за единицу <br/>";
                th4 = "валюты  <br/>";
                th5 = " цена  <br/>";


            }
            if (lans.Contains("中"))
            {
                productTitle += "产品明细";
                th0 += "货物描述";
                th1 += "单位";
                th2 += "数量";
                th3 += "单价";
                th4 += "币种";
                th5 += "总价";
                th8 += "规格";

            }
            sb.AppendLine(productTitle);
            sbpro.Append(@"<table class='prodetail' border='1' bordercolor='#a0c6e5' cellpadding=3 style='border-collapse:collapse;width:500px;height:200px;'>");

            //生成产品明细表

            sbpro.Append("<tr>");
            sbpro.Append("<td>" + th0 + "</td>");
            sbpro.Append("<td>" + th1 + "</td>");
            sbpro.Append("<td>" + th2 + "</td>");
            sbpro.Append("<td>" + th3 + "</td>");
            sbpro.Append("<td>" + th8 + "</td>");
            sbpro.Append("<td>" + th4 + "</td>");
            sbpro.Append("<td>" + th5 + "</td>");
            sbpro.Append("</tr>");


            decimal totalamount = 0;
            string priceUnit = string.Empty;
            foreach (Hashtable hs in listtable)
            {
                //rownum++;
                sbpro.Append("<tr>");
                sbpro.Append("<td>" + hs["pname"] + "</td>");
                sbpro.Append("<td>" + hs["qunit"] + "</td>");
                sbpro.Append("<td>" + hs["quantity"] + "</td>");
                sbpro.Append("<td>" + hs["price"] + "</td>");
                sbpro.Append("<td>" + hs["spec"] + "</td>");
                sbpro.Append("<td>" + hs["priceUnit"] + "</td>");
                sbpro.Append("<td>" + hs["amount"] + "</td>");

                sbpro.Append("</tr>");
                priceUnit = hs["priceUnit"].ToString();
                totalamount += Convert.ToDecimal(hs["amount"]);
            }
            string totalabel = "";
            if (lans.Contains("英文"))
            {
                totalabel = "Total Value<br/>";
            }
            if (lans.Contains("俄文"))
            {
                totalabel = "Обшая сумма<br/>";
            }
            if (lans.Contains("中文"))
            {
                totalabel += "总金额";
            }

            sbpro.Append("<tr>");
            sbpro.Append("<td>" + totalabel + "</td>");
            string total = "";
            int cnt = (int)(Convert.ToDouble(totalamount) == 0 ? 1 : Convert.ToDouble(totalamount));
            if (lans.Contains("英文"))
            {
                total = Util.NumberToEnglishString(cnt).ToUpper() + priceUnit + "<br/>";
            }
            if (lans.Contains("中文") || lans.Contains("俄文"))
            {
                total += contractBll.getCurrency(priceUnit, EcanRMB.CmycurD(totalamount.ToString()));
                //total += EcanRMB.CmycurD(totalamount.ToString()).Substring(0, EcanRMB.CmycurD(totalamount.ToString()).Length - 2) + contractBll.getCurrency(priceUnit);
            }
            sbpro.Append("<td colspan='6' >" + total + "</td>");
            sbpro.Append("</tr>");
            sbpro.Append("<tr>");
            //sbpro.Append("<td colspan='6'>Instructions:说明<br/>T/T Beneficiary  收款人：" + sellerName.ToString() + "<br/>Add:" + sellerEngAddress + "<br/>地址：" + sellerChinaAddress + "<br/>This pval is sold in accordance with the General Trade Rules . 该助剂按照一般贸易规则___10__% more or less allowance on both Credit amount and quantity of Goods is allowed.<br/>货物数量存在10%的溢短装是允许的。<br/>Seller must loading separately for each order quantity of each plant the buyer to provide, should not be mixed.<br/>卖方需按照中买方提供的各工厂采购量来分别装箱，不得混装、拼装。</td></tr>");

            sbpro.Append("</table>");
            sb.Append(sbpro.ToString());

            #endregion

            //添加条款
            sb.AppendLine(items.ToString());
            //替换表尾
            if (customerClassfic == "境内")
            {
                StringBuilder bottomSb = new StringBuilder(bottom);
                InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
                sb.AppendLine(bottomSb.ToString());
            }
            else
            {
                StringBuilder bullingSb = new StringBuilder(bulling);
                InsteadLabelString.Singleton.InsteadStringBuilder(bullingSb, htdata, lans);
                sb.AppendLine(bullingSb.ToString());
            }
            return sb.ToString();
        }
        #endregion

        #region 获取内部结算单实时预览文本
        //获取实时内部结算单预览文本
        private string GetInternalPreview(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"] ?? string.Empty;
            string signedTime = context.Request.Params["signedTime"] ?? string.Empty;
            string signedplace = context.Request.Params["signedplace"] ?? string.Empty;
            string buyer = context.Request.Params["buyer"] ?? string.Empty;
            string seller = context.Request.Params["seller"] ?? string.Empty;
            string buyercode = context.Request.Params["buyercode"] ?? string.Empty;
            string sellercode = context.Request.Params["sellercode"] ?? string.Empty;
            string datagridjson = context.Request.Params["datagridjson"] ?? string.Empty;
            string remark = context.Request.Params["remark"] ?? string.Empty;
            string createTableName = context.Request.Params["createTableName"];
            string itemProName = context.Request.Params["itemProName"];
            string Organizer = context.Request.Params["Organizer"];
            string startDate = context.Request.Params["startDate"];
            string endDate = context.Request.Params["endDate"];
            string ItemAmount = context.Request.Params["ItemAmount"];
            string text1 = context.Request.Params["text1"];
            string text2 = context.Request.Params["text2"];
            string text3 = context.Request.Params["text3"];
            string text4 = context.Request.Params["text4"];
            string title = string.Empty;
            string bottom = string.Empty;
            string bulling = string.Empty;
            string bankMessage = string.Empty;
            string customerClassfic = string.Empty;
            StringBuilder sb = new StringBuilder();

            //获取合同详细
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtContract = new System.Data.DataTable();
            #region 获取表头表尾
            title = @"<p>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:18px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {中文=内部供需结转单}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span> 
</p>
<p>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {中文=编号:}{不限:合同编号}
</p>
<br />
<table class='prodetail' style='border-collapse:collapse;width:1100px;table-layout:fixed;font-family:SimSun;' cellpadding='3' border='1'>
	<tbody>
		<tr>
			<td style='layout:fixed;' width='150px' >
				<span>&nbsp;{中文=项目(生产)名称:}</span> 
			</td>
			<td style='layout:fixed;' width='150px'>
				<span>&nbsp;<span>{中文:项目(生产)名称}</span></span> 
			</td>
			<td style='layout:fixed;' width='120px'>
				<span>&nbsp;{中文=制单日期:}</span> 
			</td>
			<td style='layout:fixed;' width='120px'>
				<span>&nbsp;{中文:制单日期}</span> 
			</td>
			<td style='layout:fixed;' width='120px'>
				<span>&nbsp;{中文=承办单位:}</span> 
			</td>
			<td style='layout:fixed;' width='440px' >
				<span>&nbsp;{中文:承办单位}</span> 
			</td>
		</tr>
		<tr>
			<td >
				<span>&nbsp;{中文=供方单位:}</span> 
			</td>
			<td colspan='2'>
				<span>&nbsp;{中文:供方单位}</span> 
			</td>
			<td>
				<span>&nbsp;{中文=需方单位:}</span> 
			</td>
			<td colspan='2'>
				<span>&nbsp;{中文:需方单位}</span> 
			</td>
		</tr>
	</tbody>
</table>";
            bottom = @"<table class='prodetail' style='border-collapse:collapse;width:1100px;height:200px;font-family:SimSun;' cellpadding='3' border='1';>
	<tbody>
		<tr>
			<td colspan='6'width='1100px'>
				<span>&nbsp;{中文=验收标准:}{中文:验收标准}</span> 
			</td>
		</tr>
		<tr>
			<td colspan='6'width='1100px'>
				<span>&nbsp;{中文=不合格品处理方法:}{中文:不合格品处理方法}</span> 
			</td>
		</tr>
		<tr>
			<td colspan='6' width='1100px'>
				<span>&nbsp;{中文=结算方式:}{中文:结算方式}</span> 
			</td>
		</tr>
		<tr>
			<td width='150'>
				<span>&nbsp;{中文=需方单位领导:}</span> 
			</td>
			<td width='150'>
				<span>&nbsp;{中文:需方单位领导}</span> 
			</td>
			<td width='150'>
				<span>&nbsp;{中文=承办部门主管领导:}</span> 
			</td>
			<td width='150'>
				<span>&nbsp;{中文:承办部门主管领导}</span> 
			</td>
			<td width='150'>
				<span>&nbsp;{中文=承办部门负责人:}</span> 
			</td>
			<td width='350'>
				<span>&nbsp;{中文:承办部门负责人}</span> 
			</td>
		</tr>
		<tr>
			<td width='150'>
				<span>&nbsp;{中文=财务部门:}</span> 
			</td>
			<td width='150'>
				<span>&nbsp;{中文:财务部门}</span> 
			</td>
			<td width='150'>
				<span>&nbsp;{中文=计划/合同专管员:}</span> 
			</td>
			<td width='150'>
				<span>&nbsp;{中文:计划/合同专管员}</span> 
			</td>
			<td width='150'>
				<span>&nbsp;{中文=承办人:}</span> 
			</td>
			<td width='350'>
				<span>&nbsp;{中文:承办人}</span> 
			</td>
		</tr>
	</tbody>
</table>
<span><span style='font-size:18px;'>{中文=填写说明:}</span>&nbsp; {中文:填写说明}<br />
</span>";
            bulling = bottom;
            //title = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表头' and templateno='0601'").ToString();
            //bulling = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾'  and templateno='0601'").ToString();
            //bottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾'  and templateno='0601'").ToString();

            #endregion

            #region 替换模板变量
            Hashtable htdata = new Hashtable();
            htdata["不限:合同编号"] = contractNo;
            htdata["中文:买方名"] = buyer;
            htdata["中文:卖方名"] = seller;
            htdata["中文:签订时间"] = signedTime;
            htdata["中文:签订地点"] = signedplace;
            if (!string.IsNullOrWhiteSpace(createTableName))
            {
                htdata["中文:制单日期"] = createTableName.ToString();
            }
            htdata["中文:项目(生产)名称"] = itemProName.ToString();
            htdata["中文:承办单位"] = Organizer.ToString();
            htdata["中文:供方单位"] = seller.ToString();
            htdata["中文:需方单位"] = buyer.ToString();
            htdata["中文:验收标准"] = text1.ToString();
            htdata["中文:不合格品处理方法"] = text2.ToString();
            htdata["中文:结算方式"] = text3.ToString();
            htdata["中文:填写说明"] = text4.ToString();
            #endregion


            #region 加载买卖方详细信息
            DataSet ds22 = new DataSet();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                //sb22.Append(" select * from Econtract_ap where contractNo=@contractno and attachmentno=''; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
                   //new  System.Data.SqlClient.SqlParameter("@contractno",contractNo),
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyercode.Trim()),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellercode.Trim())
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            if (ds22.Tables[0].Rows.Count > 0)
            {
                //客户
                DataRow dr1 = ds22.Tables[0].Rows[0];
                htdata["俄文:买方名"] = dr1["rsname"].ToString();
                htdata["英文:买方名"] = dr1["egname"].ToString();
                htdata["中文:买方名"] = dr1["name"].ToString();
                htdata["俄文:买方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:买方地址"] = dr1["egaddress"].ToString();
                htdata["中文:买方地址"] = dr1["address"].ToString();
                htdata["中文:客户地址及电话"] = dr1["icnaddress"].ToString() + dr1["icnphone"].ToString();
                htdata["英文:客户地址及电话"] = dr1["iegaddress"].ToString() + dr1["iegphone"].ToString();
                htdata["中文:开户银行"] = dr1["icnbank"].ToString();
                htdata["英文:开户银行"] = dr1["iegbank"].ToString();
                htdata["中文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["中文:开户银行账号"] = dr1["icnaccount"].ToString();
                htdata["英文:开户银行号"] = dr1["iegaccount"].ToString();
                customerClassfic = dr1["classific"].ToString();
            }
            if (ds22.Tables[1].Rows.Count > 0)
            {
                //供应商
                DataRow dr1 = ds22.Tables[1].Rows[0];
                htdata["俄文:卖方名"] = dr1["rsname"].ToString();
                htdata["英文:卖方名"] = dr1["egname"].ToString();
                htdata["中文:卖方名"] = dr1["name"].ToString();

                htdata["俄文:卖方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:卖方地址"] = dr1["egaddress"].ToString();
                htdata["中文:卖方地址"] = dr1["address"].ToString();
                htdata["英文:收款银行名称"] = dr1["iegbank"].ToString();
                htdata["中文:收款银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:收款人银行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:收款人银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:收款人银行账号"] = dr1["iegaccount"].ToString();
                htdata["中文:收款人银行账号"] = dr1["icnaccount"].ToString();
                htdata["中文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["英文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["中文:卖方信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:卖方信用代码"] = dr1["icncreditcode"].ToString();

            }
            //客户
            if (ds22.Tables[2].Rows.Count > 0)
            {
                DataRow drcus = ds22.Tables[2].Rows[0];
                htdata["不限:买方电话"] = drcus["phone"].ToString();
            }
            //供应商
            if (ds22.Tables[3].Rows.Count > 0)
            {
                DataRow drsup = ds22.Tables[3].Rows[0];
                htdata["不限:卖方电话"] = drsup["phone"].ToString();
            }
            #endregion
            //卖方信息，买方信息，产品信息
            string lans = "中文";
            //先添加表头
            StringBuilder titleSb = new StringBuilder(title);
            InsteadLabelString.Singleton.InsteadStringBuilder(titleSb, htdata, lans);
            sb.AppendLine(titleSb.ToString());
            #region 生成产品明细表
            string productTitle = "";
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagridjson);
            ////动态生成产品表格
            //DataTable dtproduct = ds22.Tables[2];
            StringBuilder sbpro = new StringBuilder();
            string th0 = "";
            string th1 = "";
            string th2 = "";
            string th3 = "";
            string th4 = "";
            string th5 = "";
            string th6 = "";
            string th7 = "";
            string th8 = "";
            string th9 = "";
            string th10 = "";
            if (lans.Contains("中"))
            {
                productTitle += "物料编码";
                th0 += "物料编码";
                th1 += "物料名称";
                th2 += "规格型号";
                th3 += "单位";
                th4 += "含税单价";
                th5 += "数量";
                th6 += "金额";
                th7 += "供货起止日期";
                th8 += "税率";
                th9 += "SAP订单类型和编号";

            }
            //sbpro.AppendLine(productTitle);
            sbpro.Append(@"<table class='prodetail' border='1' cellpadding=3 style='border-collapse:collapse;width:1100px;height:200px;'>");

            //生成产品明细表

            sbpro.Append("<tr>");

            sbpro.Append("<td>" + th0 + "</td>");
            sbpro.Append("<td>" + th1 + "</td>");
            sbpro.Append("<td>" + th2 + "</td>");
            sbpro.Append("<td>" + th3 + "</td>");
            sbpro.Append("<td>" + th4 + "</td>");
            sbpro.Append("<td>" + th5 + "</td>");
            sbpro.Append("<td>" + th6 + "</td>");
            sbpro.Append("<td>" + th7 + "</td>");
            sbpro.Append("<td>" + th8 + "</td>");
            sbpro.Append("<td>" + th9 + "</td>");
            sbpro.Append("</tr>");


            decimal totalamount = 0;
            string priceUnit = string.Empty;
            foreach (Hashtable hs in listtable)
            {
                //rownum++;
                sbpro.Append("<tr>");

                sbpro.Append("<td>" + hs["pcode"] + "</td>");
                sbpro.Append("<td>" + hs["pname"] + "</td>");
                sbpro.Append("<td>" + hs["spec"] + "</td>");
                sbpro.Append("<td>" + hs["qunit"] + "</td>");
                sbpro.Append("<td>" + hs["price"] + "</td>");
                sbpro.Append("<td>" + hs["quantity"] + "</td>");
                sbpro.Append("<td>" + hs["amount"] + "</td>");
                sbpro.Append("<td>" + startDate + "-" + endDate + "</td>");
                sbpro.Append("<td>" + hs["rate"] + "</td>");
                sbpro.Append("<td>" + hs["SAPNumber"] + "</td>");
                sbpro.Append("</tr>");
                priceUnit = hs["priceUnit"].ToString() == "" ? ConstantUtil.CURRENCY_CNY : hs["priceUnit"].ToString();
                totalamount += Convert.ToDecimal(hs["amount"]);
            }
            string totalabel = "";

            if (lans.Contains("中文"))
            {
                totalabel += "总金额";
            }
            sbpro.Append("<tr>");
            sbpro.Append("<td>" + totalabel + "</td>");
            string total = "";

            int cnt = (int)(Convert.ToDouble(totalamount) == 0 ? 1 : Convert.ToDouble(totalamount));
            if (lans.Contains("英文"))
            {
                total = Util.NumberToEnglishString(cnt).ToUpper() + priceUnit + "<br/>";
            }
            if (lans.Contains("中文") || lans.Contains("俄文"))
            {
                total += contractBll.getCurrency(priceUnit, EcanRMB.CmycurD(totalamount.ToString()));
                //total += EcanRMB.CmycurD(totalamount.ToString()).Substring(0, EcanRMB.CmycurD(totalamount.ToString()).Length - 2) + contractBll.getCurrency(priceUnit);
            }

            sbpro.Append("<td colspan='10' >" + total + "</td>");
            sbpro.Append("</tr>");
            sbpro.Append("<tr>");
            sbpro.Append("</table>");
            sb.AppendLine(sbpro.ToString());
            #endregion


            sb.AppendLine(remark);
            //替换表尾
            if (customerClassfic == "境内")
            {
                StringBuilder bottomSb = new StringBuilder(bottom);
                InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
                sb.AppendLine(bottomSb.ToString());
            }
            else
            {
                StringBuilder bullingSb = new StringBuilder(bulling);
                InsteadLabelString.Singleton.InsteadStringBuilder(bullingSb, htdata, lans);
                sb.AppendLine(bullingSb.ToString());
            }
            return sb.ToString();
        }
        #endregion

        #region 获取合同实时预览文本
        //获取实时合同预览文本
        private string GetRealTimeContract(HttpContext context)
        {
            ITemplateDB templateDb = new TempldateDB();
            string contractNo = context.Request.Params["contractNo"] ?? string.Empty;
            string buyercode = context.Request.Params["buyercode"];
            string sellercode = context.Request.Params["sellercode"];
            string signedPlace = context.Request.Params["signedPlace"];
            string signedTime = context.Request.Params["signedTime"];
            string templateno = context.Request.Params["templateno"];
            string language = context.Request.Params["language"] ?? string.Empty;
            string validity = context.Request.Params["validity"] ?? string.Empty;
            string productlist = context.Request.Params["productlist"];
            string tradement = context.Request.Params["tradement"] ?? string.Empty;
            string transport = context.Request.Params["transport"] ?? string.Empty;
            string harborout = context.Request.Params["harborout"] ?? string.Empty;
            string harborarrive = context.Request.Params["harborarrive"] ?? string.Empty;
            string delivery = context.Request.Params["deliveryPlace"] ?? string.Empty;
            string pricement1 = context.Request.Params["pricement1"] ?? string.Empty;
            string pricement2 = context.Request.Params["pricement2"] ?? string.Empty;
            string pvalidity = context.Request.Params["pvalidity"] == null ? string.Empty : context.Request.Params["pvalidity"];
            string shipment = context.Request.Params["shipment"] ?? string.Empty;
            string placement = context.Request.Params["placement"] ?? string.Empty;
            string item1 = context.Request.Params["item1"] ?? string.Empty;
            string item2 = context.Request.Params["item2"] ?? string.Empty;
            string item3 = context.Request.Params["item3"] ?? string.Empty;
            string item4 = context.Request.Params["item4"] ?? string.Empty;
            string item5 = context.Request.Params["item5"] ?? string.Empty;
            string simpleBuyer = context.Request.Params["simpleBuyer"] ?? string.Empty;
            string simpleSeller = context.Request.Params["simpleSeller"] ?? string.Empty;
            string buyeraddress = context.Request.Params["buyeraddress"] ?? string.Empty;
            string selleraddress = context.Request.Params["selleraddress"] ?? string.Empty;
            string moreLanguage = context.Request.Params["moreLanguage"] ?? string.Empty;
            string templatejson = context.Request.Params["templatejson"] ?? string.Empty;
            string shppingmark = context.Request.Params["shippingmark"] ?? string.Empty;
            string batchRemark = context.Request.Params["batchRemark"] ?? string.Empty;
            string overspill = context.Request.Params["overspill"] ?? string.Empty;
            string shipDate = context.Request.Params["shipDate"] ?? string.Empty;
            string paymentType = context.Request.Params["paymentType"] ?? string.Empty;
            string transporteng = "";
            string transportrus = "";
            string harborouteng = "";
            string harboroutrus = "";
            string harborarriveeng = "";
            string harborarriverus = "";
            string shipmenteng = "";
            string shipmengrus = "";
            string placementeng = "";
            string placementrus = "";
            string pricement1eng = "";
            string pricement1rus = "";
            string pricement2eng = "";
            string pricement2rus = "";
            string paymentTypeeng = "";
            string paymentTyperus = "";
            string title = string.Empty;
            string bottom = string.Empty;
            string bankMessage = string.Empty;
            string customerClassfic = string.Empty;
            string supplierClassfic = string.Empty;
            string bulling = string.Empty;
            string attachBottom = "";
            string attachBulling = "";
            string noBankMessage = "";
            string noBankBullingMess = "";
            //获取是否为商检查看合同，true 预览时加上收货人
            string isInspect = context.Request.Params["isInspect"] ?? string.Empty;
            StringBuilder sb = new StringBuilder();

            //获取合同详细
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtContract = new System.Data.DataTable();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                System.Data.SqlClient.SqlParameter[] mms = new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@templateno",templateno),
                   
                };
                dt = bll.ExecDatasetSql(@" select templateno, sortno, chncontent, engcontent, ruscontent, isinline from btemp_detail where templateno=@templateno  order by sortno;
                   ", mms).Tables[0];
                title = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表头'  and templateno='0603'").ToString();
                bottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾' and templateno='0603'").ToString();
                bankMessage = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='银行信息'").ToString();
                bulling = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='开票信息' and templateno='0603'").ToString();
                noBankMessage = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾无银行信息 ' and templateno='0603'").ToString();
                noBankBullingMess = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='无银行开票信息  ' and templateno='0603'").ToString();
                attachBulling = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='附件开票信息' and templateno='0604'").ToString();
                attachBottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='附件表尾' and templateno='0604'").ToString();
            }

            Hashtable htdata = new Hashtable();

            //htdata["中文:签订时间"] = Convert.ToDateTime(drContract["signedtime"]).ToString("yyyy年MM月dd天");
            htdata["中文:签订时间"] = signedTime;
            htdata["英文:签订时间"] = signedTime;
            htdata["中文:签订地点"] = signedPlace.ToString();
            htdata["英文:签订地点"] = signedPlace.ToString();
            htdata["不限:合同编号"] = contractNo.ToString();
            htdata["不限:签订时间"] = signedTime;
            htdata["不限:签订地点"] = signedPlace.ToString();
            htdata["不限:批次备注"] = batchRemark.ToString();
            if (isInspect == "true")//添加收货人
            {
                var revMan = context.Request.Params["revMan"] ?? string.Empty;
                htdata["中文:收货"] = "收货人:";
                htdata["中文:收货人"] = revMan;
            }
            if (contractNo.Contains("GY"))
            {
                htdata["不限:买方签名"] = contractBll.getSignedPng(contractNo);
            }
            else
            {
                htdata["不限:卖方签名"] = contractBll.getSignedPng(contractNo);
            }
            //卖方信息，买方信息，产品信息
            DataSet ds22 = new DataSet();
            string sellerName = string.Empty;
            string sellerChinaAddress = string.Empty;
            string sellerEngAddress = string.Empty;
            //银行信息
            string iegbank = string.Empty;
            string iegaddress = string.Empty;
            string iegname = string.Empty;
            string iegaccount = string.Empty;
            string sellerPhone = string.Empty;
            string swiftCode = string.Empty;

            #region 加载买卖方详细信息
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                //sb22.Append(" select * from Econtract_ap where contractNo=@contractno and attachmentno=''; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
                   //new  System.Data.SqlClient.SqlParameter("@contractno",contractNo),
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyercode),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellercode)
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            if (ds22.Tables[0].Rows.Count > 0)
            {
                //客户
                DataRow dr1 = ds22.Tables[0].Rows[0];
                htdata["俄文:买方名"] = dr1["rsname"].ToString();
                htdata["英文:买方名"] = dr1["egname"].ToString();
                htdata["中文:买方名"] = dr1["name"].ToString();
                htdata["俄文:买方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:买方地址"] = dr1["egaddress"].ToString();
                htdata["中文:买方地址"] = dr1["address"].ToString();
                htdata["中文:客户地址及电话"] = dr1["icnaddress"].ToString() + dr1["icnphone"].ToString();
                htdata["英文:客户地址及电话"] = dr1["iegaddress"].ToString() + dr1["iegphone"].ToString();
                htdata["中文:开户银行"] = dr1["icnbank"].ToString();
                htdata["英文:开户银行"] = dr1["iegbank"].ToString();
                htdata["中文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["中文:开户银行账号"] = dr1["icnaccount"].ToString();
                htdata["英文:开户银行号"] = dr1["iegaccount"].ToString();
                customerClassfic = dr1["classific"].ToString();
            }
            if (ds22.Tables[1].Rows.Count > 0)
            {
                //供应商
                DataRow dr1 = ds22.Tables[1].Rows[0];
                htdata["俄文:卖方名"] = dr1["rsname"].ToString();
                htdata["英文:卖方名"] = dr1["egname"].ToString();
                htdata["中文:卖方名"] = dr1["name"].ToString();
                sellerName = dr1["name"].ToString();
                sellerChinaAddress = dr1["address"].ToString();
                sellerEngAddress = dr1["egaddress"].ToString();
                htdata["俄文:卖方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:卖方地址"] = dr1["egaddress"].ToString();
                htdata["中文:卖方地址"] = dr1["address"].ToString();
                htdata["英文:收款银行名称"] = dr1["iegbank"].ToString();
                htdata["中文:收款银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:收款人银行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:收款人银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:收款人银行账号"] = dr1["iegaccount"].ToString();
                htdata["中文:收款人银行账号"] = dr1["icnaccount"].ToString();
                htdata["中文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["英文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["中文:卖方信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:卖方信用代码"] = dr1["icncreditcode"].ToString();
                iegbank = dr1["iegbank"].ToString();
                iegaddress = dr1["iegaddress"].ToString();
                iegname = dr1["iegname"].ToString();
                iegaccount = dr1["iegaccount"].ToString();
                sellerPhone = dr1["icnphone"].ToString();
                swiftCode = dr1["icncreditcode"].ToString();
                supplierClassfic = dr1["classific"].ToString();
            }
            //客户
            if (ds22.Tables[2].Rows.Count > 0)
            {
                DataRow drcus = ds22.Tables[2].Rows[0];
                htdata["不限:买方电话"] = drcus["phone"].ToString();
            }
            //供应商
            if (ds22.Tables[3].Rows.Count > 0)
            {
                DataRow drsup = ds22.Tables[3].Rows[0];
                htdata["不限:卖方电话"] = drsup["phone"].ToString();
            }
            #endregion
            htdata["中文:收款人银行名称"] = iegbank;
            htdata["中文:编码"] = swiftCode;
            htdata["中文:收款人支行地址"] = iegaddress;
            htdata["中文:支行号"] = iegaccount;
            htdata.Add("中文:合同号", "合同号");
            htdata.Add("英文:合同号", "Contract No.");

            htdata.Add("中文:合同有效期", validity.ToString().Length == 0 ? "" : validity.ToString());
            // Convert.ToDateTime(drContract["validity"]).ToString("yyyy 年 MM 月 dd 日"));
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
            htdata.Add("英文:合同有效期", validity.ToString().Length == 0 ? "" : validity.ToString());
            //Convert.ToDateTime(drContract["validity"]).ToString(cultureinfo));

            htdata.Add("中文:日期", "日期");
            htdata.Add("英文:日期", "Date:");

            htdata.Add("中文:合同", "合同:");
            htdata.Add("英文:合同", "CONTRACT:");
            htdata.Add("俄文:合同", "КОНТРАКТ:");

            htdata.Add("俄文:卖方", "ПРОДАВЕЦ:");
            htdata.Add("英文:卖方", "The Seller:");
            htdata.Add("中文:卖方", "卖方:");

            htdata.Add("中文:银行", "银行:");
            htdata.Add("俄文:银行", "Банк:");

            htdata.Add("俄文:买方", "ПОКУПАТЕЛЬ:");
            htdata.Add("中文:买方", "买方:");
            htdata.Add("英文:买方", "The Buyer:");

            htdata.Add("俄文:地址", "Реквизиты:");
            htdata.Add("中文:地址", "地址:");
            htdata.Add("英文:地址", "ADD:");



            string lans = language.ToString();
            if (!string.IsNullOrEmpty(moreLanguage))
            {
                lans = moreLanguage;
            }
            //先添加表头
            StringBuilder titleSb = new StringBuilder(title);
            InsteadLabelString.Singleton.InsteadStringBuilder(titleSb, htdata, lans);
            string title1 = titleSb.ToString().Replace("\t", "").Replace("\r\n", "");
            title1 = title1.Replace("<p></p>", "");
            sb.AppendLine(title1.ToString());

            #region 生成产品明细表
            string productTitle = "";
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(productlist);
            ////动态生成产品表格
            //DataTable dtproduct = ds22.Tables[2];
            StringBuilder sbpro = new StringBuilder();
            string th0 = "";
            string th1 = "";
            string th2 = "";
            string th3 = "";
            string th4 = "";
            string th5 = "";
            string th6 = "";
            string th7 = "";
            string th8 = "";
            //string th6 = "";
            //string th7 = "";
            //string th8 = "";
            //string th9 = "";

            if (lans.Contains("英"))
            {
                productTitle = "ProductDetails<br/>";
                th0 = "DESCRIPTION<br/>";
                th1 = "UNIT<br/>";
                th2 = "QUANTITY<br/>";
                th3 = "UNIT PRICE<br/>";
                th4 = "PRICEUNIT<br/>";
                th5 = "AMOUNT<br/>";
                th6 = "Mark<br/>";
                th7 = "Packages<br/>";
                th8 = "Spec<br/>";
            }
            if (lans.Contains("俄"))
            {
                productTitle = "Список продуктов<br/> ";
                th0 = "Описание товаров <br/>";
                th1 = " единица  <br/>";
                th2 = "количество <br/>";
                th3 = " цена за единицу <br/>";
                th4 = "валюты  <br/>";
                th5 = " цена  <br/>";
                th6 = " Mаркировка <br/>";
                th7 = " упаковка <br/>";
                th8 = "спецификации <br/>";
            }
            if (lans.Contains("中"))
            {
                productTitle += "产品明细";
                th0 += "货物描述";
                th1 += "单位";
                th2 += "数量";
                th3 += "单价";
                th4 += "币种";
                th5 += "总价";
                th6 += "唛头";
                th7 += "包装";
                th8 += "规格";
            }
            sbpro.AppendLine(productTitle);
            sbpro.Append(@"<table class='prodetail' border='1' bordercolor='#a0c6e5' cellpadding=3 style='border-collapse:collapse;width:500px;height:200px;'>");

            //生成产品明细表

            sbpro.Append("<tr>");
            sbpro.Append("<td>" + th6 + "</td>");
            sbpro.Append("<td>" + th0 + "</td>");
            sbpro.Append("<td>" + th8 + "</td>");
            sbpro.Append("<td>" + th7 + "</td>");
            sbpro.Append("<td>" + th1 + "</td>");
            sbpro.Append("<td>" + th2 + "</td>");
            sbpro.Append("<td>" + th3 + "</td>");

            sbpro.Append("<td>" + th4 + "</td>");
            sbpro.Append("<td>" + th5 + "</td>");
            sbpro.Append("</tr>");


            decimal totalamount = 0;
            string priceUnit = string.Empty;
            foreach (Hashtable hs in listtable)
            {
                //rownum++;
                sbpro.Append("<tr>");
                sbpro.Append("<td>" + shppingmark + "</td>");
                if (lans == "中文-英文")
                {
                    sbpro.Append("<td>" + hs["pnameen"] + "<br/>" + hs["pname"] + "</td>");
                }
                if (lans == "中文")
                {
                    sbpro.Append("<td>" + hs["pname"] + "</td>");
                }
                if (lans == "英文")
                {
                    sbpro.Append("<td>" + hs["pnameen"] + "</td>");
                }
                if (lans == "中文-俄文")
                {
                    sbpro.Append("<td>" + hs["pnameru"] + "<br/>" + hs["pname"] + "</td>");
                }
                if (lans == "俄文")
                {
                    sbpro.Append("<td>" + hs["pnameru"] + "</td>");
                }
                sbpro.Append("<td>" + hs["spec"] + "</td>");
                sbpro.Append("<td>" + hs["pallet"] + hs["unit"] + "</td>");
                sbpro.Append("<td>" + hs["qunit"] + "</td>");
                sbpro.Append("<td>" + hs["quantity"] + "</td>");
                string priceAdd = hs["priceAdd"] == null ? string.Empty : hs["priceAdd"].ToString();
                if (!string.IsNullOrEmpty(priceAdd))
                {
                    sbpro.Append("<td>" + priceAdd + "</td>");
                }
                else
                {
                    sbpro.Append("<td>" + hs["price"] + "</td>");
                }

                sbpro.Append("<td>" + hs["priceUnit"] + "</td>");
                sbpro.Append("<td>" + hs["amount"] + "</td>");

                sbpro.Append("</tr>");
                priceUnit = hs["priceUnit"].ToString();
                string amount = hs["amount"].ToString();
                if (string.IsNullOrEmpty(amount))
                {
                    amount = "0";
                }
                totalamount += Convert.ToDecimal(amount);
            }
            string totalabel = "";
            if (lans.Contains("英文"))
            {
                totalabel = "Total Value<br/>";
            }
            if (lans.Contains("俄文"))
            {
                totalabel = "Обшая сумма<br/>";
            }
            if (lans.Contains("中文"))
            {
                totalabel += "总金额";
            }
            sbpro.Append("<tr>");
            sbpro.Append("<td>" + totalabel + "</td>");
            string total = "";
            int cnt = (int)(Convert.ToDouble(totalamount) == 0 ? 1 : Convert.ToDouble(totalamount));
            if (lans.Contains("英文"))
            {
                total = Util.NumberToEnglishString(cnt).ToUpper() + priceUnit + "<br/>";
            }
            if (lans.Contains("中文") || lans.Contains("俄文"))
            {
                total += contractBll.getCurrency(priceUnit, EcanRMB.CmycurD(totalamount.ToString()));
                //total += EcanRMB.CmycurD(totalamount.ToString()).Substring(0, EcanRMB.CmycurD(totalamount.ToString()).Length - 2) + contractBll.getCurrency(priceUnit);
            }

            sbpro.Append("<td colspan='8' >" + total + "</td>");
            sbpro.Append("</tr>");
            sbpro.Append("<tr>");
            sbpro.Append("</table>");
            #endregion
            if (context.Request.Params["isInspect"] == "true")//判断是否为商检合同
            {
                sb.AppendLine(sbpro.ToString());
            }

            //sb.AppendLine(sbpro.ToString());
            BtemplateBLL templateBll = new BtemplateBLL();

            if (!string.IsNullOrEmpty(item1))
            {
                sb.AppendLine("<span style='color:red'>" + item1 + "</span><br/>");
            }
            if (!string.IsNullOrEmpty(item2))
            {
                sb.AppendLine("<span style='color:red'>" + item2 + "</span>  <br />");
            }
            if (!string.IsNullOrEmpty(item3))
            {
                sb.AppendLine("<span style='color:red'>" + item3 + "</span>  <br />");
            }
            if (!string.IsNullOrEmpty(item4))
            {
                sb.AppendLine("<span style='color:red'>" + item4 + "</span>  <br />");
            }
            if (!string.IsNullOrEmpty(item5))
            {
                sb.AppendLine("<span style='color:red'>" + item5 + "</span>  <br />");
            }
            //List<Hashtable> templateTable = JsonHelper.DeserializeJsonToList<Hashtable>(templatejson);
            //List<HashTableExp> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<HashTableExp>(templatejson);
            //templateTable.Sort();
            templatejson = JsonHelper.StringFormat3(templatejson);//特殊字符转义
            List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(templatejson);
            StringBuilder sb33 = new StringBuilder();
            if (templateTable != null)
            {
                HashtableComparer htComparer = new HashtableComparer("sortno", "asc", "NUM");
                templateTable.Sort(htComparer);
                sb33 = templateBll.GetContractTermsByJson(lans, templateTable);
            }
            //StringBuilder sb33 = templateBll.GetContractTerms(lans, templateno);
            //InsteadString.Singleton.InsteadStringBuilder(sb33, htdata);
            contractBll.getEngRusSting(transport, harborout, harborarrive, shipment, placement, pricement1, pricement2, paymentType, ref transporteng, ref transportrus,
                    ref harborouteng, ref harboroutrus, ref harborarriveeng, ref harborarriverus, ref shipmenteng, ref shipmengrus, ref placementeng,
                ref placementrus, ref pricement1eng, ref pricement1rus, ref pricement2eng, ref pricement2rus, ref paymentTypeeng, ref paymentTyperus);
            //替换标签文字 变量条款
            InsteadLabelString.Singleton.InsteadStringBuilder(sb33, htdata, lans);
            sb33 = sb33
               .Replace(ConstantUtil.TEMP_TRADEMENT, tradement).Replace(ConstantUtil.TEMP_TRADEMENTENG, tradement).Replace(ConstantUtil.TEMP_TRADEMENTRUS, tradement)//贸易条款
               .Replace(ConstantUtil.TEMP_TRANSPORT, transport).Replace(ConstantUtil.TEMP_TRANSPORTENG, transporteng).Replace(ConstantUtil.TEMP_TRANSPORTRUS, transportrus)//运输方式
               .Replace(ConstantUtil.TEMP_EXPORTHARBOR, harborout).Replace(ConstantUtil.TEMP_EXPORTHARBORENG, harborouteng).Replace(ConstantUtil.TEMP_EXPORTHARBORRUS, harboroutrus)//出口口岸
               .Replace(ConstantUtil.TEMP_ARRIVEHARBOR, harborarrive).Replace(ConstantUtil.TEMP_ARRIVEHARBORENG, harborarriveeng).Replace(ConstantUtil.TEMP_ARRIVEHARBORRUS, harborarriverus)//到货口岸
               .Replace(ConstantUtil.TEMP_PRICEMENT1, pricement1).Replace(ConstantUtil.TEMP_PRICEMENT1ENG, pricement1eng).Replace(ConstantUtil.TEMP_PRICEMENT1RUS, pricement1rus)//价格条款1
               .Replace(ConstantUtil.TEMP_PRICEMENT2, pricement2).Replace(ConstantUtil.TEMP_PRICEMENT2ENG, pricement2eng).Replace(ConstantUtil.TEMP_PRICEMENT2RUS, pricement2rus)//价格条款2
               .Replace(ConstantUtil.TEMP_PVALIDITY, pvalidity).Replace(ConstantUtil.TEMP_PVALIDITYENG, pvalidity).Replace(ConstantUtil.TEMP_PVALIDITYRUS, pvalidity)//价格有效期
               .Replace(ConstantUtil.TEMP_VALIDITY, validity).Replace(ConstantUtil.TEMP_VALIDITYENG, validity).Replace(ConstantUtil.TEMP_VALIDITYRUS, validity)//合同有效期
               .Replace(ConstantUtil.TEMP_SHIPMENT, shipment).Replace(ConstantUtil.TEMP_SHIPMENTENG, shipmenteng).Replace(ConstantUtil.TEMP_SHIPMENTRUS, shipmengrus)//发运条款
               .Replace(ConstantUtil.TEMP_PLACEMENT, placement).Replace(ConstantUtil.TEMP_PLACEMENTENG, placement).Replace(ConstantUtil.TEMP_PLACEMENT, placement)//产地条款
               .Replace(ConstantUtil.TEMP_IMPORTHARBOR, harborout).Replace(ConstantUtil.TEMP_IMPORTHARBORENG, harborouteng).Replace(ConstantUtil.TEMP_IMPORTHARBORRUS, harboroutrus)//进口口岸
               .Replace(ConstantUtil.TEMP_OVERSPILL, overspill).Replace(ConstantUtil.TEMP_OVERSPILLENG, overspill).Replace(ConstantUtil.TEMP_OVERSPILLRUS, overspill)//溢出率
               .Replace(ConstantUtil.TEMP_PAYLASTDATE, shipment).Replace(ConstantUtil.TEMP_PAYLASTDATEENG, shipment).Replace(ConstantUtil.TEMP_PAYLASTDATERUS, shipment)//付款截止日
               .Replace(ConstantUtil.TEMP_SHIPDATE, shipDate).Replace(ConstantUtil.TEMP_SHIPDATEENG, shipDate).Replace(ConstantUtil.TEMP_SHIPDATERUS, shipDate)//发运日期
               .Replace(ConstantUtil.TEMP_PAYMENTTYPE, paymentType).Replace(ConstantUtil.TEMP_PAYMENTTYPEENG, paymentTypeeng).Replace(ConstantUtil.TEMP_PAYMENTTYPERUS, paymentTyperus);//付款方式
            if (lans == "中文-英文")
            {
                sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, sbpro.ToString()).Replace(ConstantUtil.TEMP_PRODUCTENG, "").Replace(ConstantUtil.TEMP_PRODUCTRUS, "");//产品表格
            }
            if (lans == "中文")
            {
                sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, sbpro.ToString()).Replace(ConstantUtil.TEMP_PRODUCTENG, "").Replace(ConstantUtil.TEMP_PRODUCTRUS, "");//产品表格
            }
            if (lans == "英文")
            {
                sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, "").Replace(ConstantUtil.TEMP_PRODUCTENG, sbpro.ToString()).Replace(ConstantUtil.TEMP_PRODUCTRUS, "");//产品表格
            }
            if (lans == "中文-俄文")
            {
                sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, sbpro.ToString()).Replace(ConstantUtil.TEMP_PRODUCTENG, "").Replace(ConstantUtil.TEMP_PRODUCTRUS, "");//产品表格
            }
            if (lans == "俄文")
            {
                sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, "").Replace(ConstantUtil.TEMP_PRODUCTENG, "").Replace(ConstantUtil.TEMP_PRODUCTRUS, sbpro.ToString());//产品表格
            }
            sb.AppendLine(sb33.ToString());
            //替换表尾,附件与合同区分

            string contractTag = DataFactory.SqlDataBase().getString(new StringBuilder(@"select contractTag from Econtract where contractNo=@contractNo"),
                new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "contractTag");
            if (contractTag == ConstantUtil.CONTRACTTAG_CONATTACH)//为框架合同附件
            {
                if (customerClassfic == "境内")
                {
                    StringBuilder bottomSb = new StringBuilder(attachBottom);
                    InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
                    sb.AppendLine(bottomSb.ToString());
                }
                else
                {
                    StringBuilder bullingSb = new StringBuilder(attachBulling);
                    InsteadLabelString.Singleton.InsteadStringBuilder(bullingSb, htdata, lans);
                    sb.AppendLine(bullingSb.ToString());
                }
            }
            else
            {
                if (customerClassfic == "境内")
                {
                    //判断合同中价格条款中是否含有信用证，有，不显示银行信息
                    if (pricement1.Contains("信用证") || pricement2.Contains("信用证"))
                    {
                        StringBuilder noBankMesSb = new StringBuilder(noBankMessage);
                        InsteadLabelString.Singleton.InsteadStringBuilder(noBankMesSb, htdata, lans);
                        sb.AppendLine(noBankMesSb.ToString());
                    }
                    else
                    {
                        StringBuilder bottomSb = new StringBuilder(bottom);
                        InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
                        sb.AppendLine(bottomSb.ToString());
                    }

                }
                else
                {
                    if (!(pricement1.Contains("信用证") || pricement2.Contains("信用证")))
                    {
                        StringBuilder bullingSb = new StringBuilder(bulling);
                        InsteadLabelString.Singleton.InsteadStringBuilder(bullingSb, htdata, lans);
                        sb.AppendLine(bullingSb.ToString());
                    }
                    else
                    {
                        StringBuilder noBankBullingSb = new StringBuilder(noBankBullingMess);
                        InsteadLabelString.Singleton.InsteadStringBuilder(noBankBullingSb, htdata, lans);
                        sb.AppendLine(noBankBullingSb.ToString());
                    }

                }
            }
            return sb.ToString();
        }
        #endregion

        #region 获取实时物流预览文本
        //获取实时物流合同预览文本
        private string GetLogisticsPreview(HttpContext context)
        {
            string buyerCode = context.Request.Params["buyerCode"];
            string language = context.Request.Params["language"];
            string sellerCode = context.Request.Params["sellerCode"];
            string contractText = context.Request.Params["contractText"];
            string templateJson = context.Request.Params["tempJson"];
            string signedTime = context.Request.Params["signedTime"];
            string signedPlace = context.Request.Params["signedPlace"];
            string firstparty = context.Request.Params["firstparty"];
            string secondparty = context.Request.Params["secondparty"];
            string allItems = context.Request.Params["allItems"];
            string allItemswidth = context.Request.Params["allItemswidth"];
            string templatename = context.Request.Params["templatename"];
            string logisticsContractNo = context.Request.Params["logisticsContractNo"];
            string title = string.Empty;
            string bottom = string.Empty;
            string bankMessage = string.Empty;
            string tableName = string.Empty;
            StringBuilder sb = new StringBuilder();

            //获取模板详细
            System.Data.DataTable dt = new System.Data.DataTable();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {

                //获取表头表尾
                title = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表头' and templateno='77600'").ToString();
                bottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾'  and templateno='77600'").ToString();
            }
            //替换变量

            Hashtable htdata = new Hashtable();
            htdata["不限:合同编号"] = logisticsContractNo;
            if (!string.IsNullOrEmpty(signedTime))
            {
                htdata["中文:签订时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
                htdata["英文:签订时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
            }
            htdata["中文:签订地点"] = signedPlace.ToString();
            htdata["中文:甲方"] = firstparty.ToString();
            htdata["中文:乙方"] = secondparty.ToString();
            htdata["中文:模板名称"] = templatename.ToString();
            //卖方信息，买方信息，产品信息
            DataSet ds22 = new DataSet();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
             
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyerCode),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellerCode)
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            if (ds22.Tables[0].Rows.Count > 0)
            {
                DataRow dr1 = ds22.Tables[0].Rows[0];
                htdata["俄文:买方名"] = dr1["rsname"].ToString();
                htdata["英文:买方名"] = dr1["egname"].ToString();
                htdata["中文:买方名"] = dr1["name"].ToString();

                htdata["俄文:买方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:买方地址"] = dr1["egaddress"].ToString();
                htdata["中文:买方地址"] = dr1["address"].ToString();
                htdata["中文:开户银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:开户行名称"] = dr1["iegbank"].ToString();
                htdata["中文:开户银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:开户行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["中文:开户银行账号"] = dr1["icnaccount"].ToString();
                htdata["英文:开户银行号"] = dr1["iegaccount"].ToString();
            }
            if (ds22.Tables[1].Rows.Count > 0)
            {
                DataRow dr1 = ds22.Tables[1].Rows[0];
                htdata["俄文:卖方名"] = dr1["rsname"].ToString();
                htdata["英文:卖方名"] = dr1["egname"].ToString();
                htdata["中文:卖方名"] = dr1["name"].ToString();
                htdata["俄文:卖方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:卖方地址"] = dr1["egaddress"].ToString();
                htdata["中文:卖方地址"] = dr1["address"].ToString();
                htdata["英文:收款人银行名称"] = dr1["iegbank"].ToString();
                htdata["中文:收款人银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:收款人银行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:收款人银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:收款人银行账号"] = dr1["iegaccount"].ToString();
                htdata["中文:收款人银行账号"] = dr1["icnaccount"].ToString();
                htdata["中文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["英文:收款人银行行号"] = dr1["icncreditcode"].ToString();
            }
            //客户
            if (ds22.Tables[2].Rows.Count > 0)
            {
                DataRow drcus = ds22.Tables[2].Rows[0];
                htdata["不限:买方电话"] = drcus["phone"].ToString();
            }
            //供应商
            if (ds22.Tables[3].Rows.Count > 0)
            {
                DataRow drsup = ds22.Tables[3].Rows[0];
                htdata["不限:卖方电话"] = drsup["phone"].ToString();
            }
            htdata.Add("中文:合同号", "合同号");
            htdata.Add("英文:合同号", "Contract No.");

            htdata.Add("中文:日期", "日期");
            htdata.Add("英文:日期", "Date");

            htdata.Add("中文:合同", "合同");
            htdata.Add("英文:合同", "CONTRACT");
            htdata.Add("俄文:合同", "КОНТРАКТ");

            htdata.Add("俄文:卖方", "ПРОДАВЕЦ:");
            htdata.Add("英文:卖方", "The Seller:");
            htdata.Add("中文:卖方", "卖方:");

            htdata.Add("中文:银行", "银行:");
            htdata.Add("俄文:银行", "Банк:");

            htdata.Add("俄文:买方", "ПОКУПАТЕЛЬ:");
            htdata.Add("中文:买方", "买方:");
            htdata.Add("英文:买方", "The Buyer: ");

            htdata.Add("俄文:地址", "Реквизиты:");
            htdata.Add("中文:地址", "地址:");
            htdata.Add("英文:地址", "ADD:");



            string lans = "中文";
            //先添加表头
            StringBuilder titleSb = new StringBuilder(title);
            InsteadLabelString.Singleton.InsteadStringBuilder(titleSb, htdata, lans);
            string title1 = titleSb.ToString().Replace("\t", "").Replace("\r\n", "").Replace("<br />", "");
            title1 = title1.Replace("<p></p>", "");
            sb.AppendLine(title1.ToString());
            sb.Append("<br/>");
            sb.AppendLine(contractText.ToString());

            #region 生成表格
            //生成表格
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(templateJson);
            string[] itemsArray = allItems.Split('|');
            string[] itemswidthArray = allItemswidth.Split('|');//获取列宽
            StringBuilder sbpro = new StringBuilder();
            sbpro.Append(@"<table class='prodetail' border='1'  cellpadding=3 style='border-collapse:collapse;width:85%;height:100px;'>");
            //生成表头
            sbpro.Append("<tr>");
            for (int i = 0; i < itemsArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(itemsArray[i].ToString()))
                {
                    sbpro.Append("<td width='" + itemswidthArray[i] + "px'>" + itemsArray[i] + "</td>");
                }

            }
            sbpro.Append("</tr>");

            foreach (Hashtable hs in listtable)
            {
                sbpro.Append("<tr>");
                if (!string.IsNullOrEmpty(hs["item1"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item1"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item2"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item2"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item3"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item3"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item4"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item4"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item5"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item5"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item6"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item6"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item7"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item7"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item8"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item8"] + "</td>");
                }
                sbpro.Append("</tr>");
            }
            sbpro.Append("</table>");
            sb.AppendLine(sbpro.ToString());
            #endregion

            //替换表尾
            StringBuilder bottomSb = new StringBuilder(bottom);
            InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
            sb.AppendLine(bottomSb.ToString());
            return sb.ToString();
        }
        #endregion

        #region 获取实时服务预览文本
        //获取实时物流合同预览文本
        private string GetServicePreview(HttpContext context)
        {
            string buyerCode = context.Request.Params["buyerCode"];
            string language = context.Request.Params["language"];
            string sellerCode = context.Request.Params["sellerCode"];
            string contractText = context.Request.Params["contractText"];
            string signedTime = context.Request.Params["signedTime"];
            string signedPlace = context.Request.Params["signedPlace"];
            string validity = context.Request.Params["validity"];
            string contractNo = context.Request.Params["contractNo"];
            string partyA = context.Request.Params["partyA"];//甲方
            string partyB = context.Request.Params["partyB"];
            string partyC = context.Request.Params["partyC"];
            string partyD = context.Request.Params["partyD"];
            string partyCCode = context.Request.Params["partyCCode"];
            string partyDCode = context.Request.Params["partyDCode"];
            string datagrid = context.Request.Params["datagridjson"] ?? string.Empty;
            string title = string.Empty;
            string bottom = string.Empty;
            string bankMessage = string.Empty;
            string tableName = string.Empty;
            StringBuilder sb = new StringBuilder();

            //获取模板详细
            System.Data.DataTable dt = new System.Data.DataTable();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //获取表头表尾
                title = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表头' and templateno='77600'").ToString();
                bottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾'  and templateno='77600'").ToString();

            }
            //替换变量

            Hashtable htdata = new Hashtable();
            htdata["不限:合同编号"] = contractNo;
            if (!string.IsNullOrEmpty(signedTime))
            {
                htdata["中文:签订时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
                htdata["英文:签订时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
            }
            htdata["中文:签订地点"] = signedPlace.ToString();
            htdata["中文:合同有效期"] = validity.ToString();
            htdata["中文:甲方"] = partyA.ToString();
            htdata["中文:乙方"] = partyB.ToString();
            if (!string.IsNullOrEmpty(partyC))
            {
                htdata["中文:丙方"] = partyC;
                htdata["中文:丙方1"] = "丙方:";
                htdata["中文:地址1"] = "地址:";
                htdata["中文:丙方日期"] = "日期:";
                if (!string.IsNullOrEmpty(signedTime))
                {
                    htdata["中文:丙方时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
                }
                htdata["中文:丙方地址"] = DataFactory.SqlDataBase().getString(new StringBuilder(@"select * from bsupplier where code=@code"),
                    new SqlParam[1] { new SqlParam("@code", partyCCode) }, "address");
            }
            if (!string.IsNullOrEmpty(partyD))
            {
                htdata["中文:丁方"] = partyD;
                htdata["中文:丁方2"] = "丁方:";
                htdata["中文:地址2"] = "地址:";
                htdata["中文:丁方日期"] = "日期:";
                if (!string.IsNullOrEmpty(signedTime))
                {
                    htdata["中文:丁方时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
                }

                htdata["中文:丁方地址"] = DataFactory.SqlDataBase().getString(new StringBuilder(@"select * from bsupplier where code=@code"),
                 new SqlParam[1] { new SqlParam("@code", partyDCode) }, "address");
            }
            //卖方信息，买方信息，产品信息
            DataSet ds22 = new DataSet();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
             
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyerCode),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellerCode)
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            if (ds22.Tables[0].Rows.Count > 0)
            {
                DataRow dr1 = ds22.Tables[0].Rows[0];
                htdata["俄文:买方名"] = dr1["rsname"].ToString();
                htdata["英文:买方名"] = dr1["egname"].ToString();
                htdata["中文:买方名"] = dr1["name"].ToString();

                htdata["俄文:买方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:买方地址"] = dr1["egaddress"].ToString();
                htdata["中文:买方地址"] = dr1["address"].ToString();
                htdata["中文:开户银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:开户行名称"] = dr1["iegbank"].ToString();
                htdata["中文:开户银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:开户行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["中文:开户银行账号"] = dr1["icnaccount"].ToString();
                htdata["英文:开户银行号"] = dr1["iegaccount"].ToString();
            }
            if (ds22.Tables[1].Rows.Count > 0)
            {
                DataRow dr1 = ds22.Tables[1].Rows[0];
                htdata["俄文:卖方名"] = dr1["rsname"].ToString();
                htdata["英文:卖方名"] = dr1["egname"].ToString();
                htdata["中文:卖方名"] = dr1["name"].ToString();
                htdata["俄文:卖方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:卖方地址"] = dr1["egaddress"].ToString();
                htdata["中文:卖方地址"] = dr1["address"].ToString();
                htdata["英文:收款人银行名称"] = dr1["iegbank"].ToString();
                htdata["中文:收款人银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:收款人银行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:收款人银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:收款人银行账号"] = dr1["iegaccount"].ToString();
                htdata["中文:收款人银行账号"] = dr1["icnaccount"].ToString();
                htdata["中文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["英文:收款人银行行号"] = dr1["icncreditcode"].ToString();
            }
            //客户
            if (ds22.Tables[2].Rows.Count > 0)
            {
                DataRow drcus = ds22.Tables[2].Rows[0];
                htdata["不限:买方电话"] = drcus["phone"].ToString();
            }
            //供应商
            if (ds22.Tables[3].Rows.Count > 0)
            {
                DataRow drsup = ds22.Tables[3].Rows[0];
                htdata["不限:卖方电话"] = drsup["phone"].ToString();
            }
            htdata.Add("中文:合同号", "合同号");
            htdata.Add("英文:合同号", "Contract No.");

            htdata.Add("中文:日期", "日期");
            htdata.Add("英文:日期", "Date");

            htdata.Add("中文:合同", "合同");
            htdata.Add("英文:合同", "CONTRACT");
            htdata.Add("俄文:合同", "КОНТРАКТ");

            htdata.Add("俄文:卖方", "ПРОДАВЕЦ:");
            htdata.Add("英文:卖方", "The Seller:");
            htdata.Add("中文:卖方", "卖方:");

            htdata.Add("中文:银行", "银行:");
            htdata.Add("俄文:银行", "Банк:");

            htdata.Add("俄文:买方", "ПОКУПАТЕЛЬ:");
            htdata.Add("中文:买方", "买方:");
            htdata.Add("英文:买方", "The Buyer: ");

            htdata.Add("俄文:地址", "Реквизиты:");
            htdata.Add("中文:地址", "地址:");
            htdata.Add("英文:地址", "ADD:");
            string lans = "中文";
            //先添加表头
            StringBuilder titleSb = new StringBuilder(title);
            InsteadLabelString.Singleton.InsteadStringBuilder(titleSb, htdata, lans);
            string title1 = titleSb.ToString().Replace("\t", "").Replace("\r\n", "").Replace("<br />", "");
            title1 = title1.Replace("<p></p>", "");
            sb.AppendLine(title1.ToString());
            sb.Append("<br/>");
            sb.AppendLine(contractText.ToString());

            #region 生成表格
            //生成表格
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(datagrid);

            StringBuilder sbpro = new StringBuilder();
            sbpro.Append(@"<table class='prodetail' border='1'  cellpadding=3 style='border-collapse:collapse;width:100%;height:100px;'>");
            //生成表头
            sbpro.Append("<tr>");
            sbpro.Append("<td width='10%'>费用类别</td>");
            sbpro.Append("<td width='15%'>项目</td>");
            sbpro.Append("<td width='15%'>项目描述</td>");
            sbpro.Append("<td width='8%'>币种</td>");
            sbpro.Append("<td width='8%'>单价</td>");
            sbpro.Append("<td width='8%'>数量</td>");
            sbpro.Append("<td width='10%'>金额</td>");
            sbpro.Append("<td width='10%'>汇率</td>");
            sbpro.Append("<td width='10%'>计价单位</td>");
            sbpro.Append("<td >备注</td>");
            sbpro.Append("</tr>");
            foreach (Hashtable hs in listtable)
            {
                sbpro.Append("<tr>");
                sbpro.Append("<td>" + hs["costCategory"] + "</td>");
                sbpro.Append("<td>" + hs["project"] + "</td>");
                sbpro.Append("<td>" + hs["projectDescribe"] + "</td>");
                sbpro.Append("<td>" + hs["currency"] + "</td>");
                sbpro.Append("<td>" + hs["price"] + "</td>");
                sbpro.Append("<td>" + hs["quantity"] + "</td>");
                sbpro.Append("<td>" + hs["amount"] + "</td>");
                sbpro.Append("<td>" + hs["rate"] + "</td>");
                sbpro.Append("<td>" + hs["priceUnit"] + "</td>");
                sbpro.Append("<td>" + hs["remark"] + "</td>");
                sbpro.Append("</tr>");
            }
            sbpro.Append("</table>");
            if (listtable.Count > 0)
            {
                sb.AppendLine(sbpro.ToString());
            }

            #endregion


            //替换表尾
            StringBuilder bottomSb = new StringBuilder(bottom);
            InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
            sb.AppendLine(bottomSb.ToString());
            return sb.ToString();
        }
        #endregion

        #endregion

        #region 获取列表

        #region 获取管理合同列表
        private string manageContractList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string buyer = context.Request.Params["buyer"];
            string seller = context.Request.Params["seller"];
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.buyer  like '%'+@buyer+'%'  ");
            }
            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.seller  like '%'+@seller+'%' ");
            }
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
               new SqlParameter("@createman",createman),
               new SqlParameter("@buyer",buyer),
               new SqlParameter("@seller",seller),
               new SqlParameter("@signedtime_begin",signedtime_begin),
               new SqlParameter("@signedtime_end",signedtime_end),
               new SqlParameter("@contractNo",contractNo)
                };
            if (createman != null && createman != "admin")
            {
                sqlshere.Append(" and t1.createman=@createman");
            }
            sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
            sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        #endregion

        #region 获取服务合同费用列表
        private string costCategoryList(HttpContext context)
        {
            JsonHelperEasyUi ui = new JsonHelperEasyUi();
            string contractNo = context.Request.Params["contractNo"];
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            sqldata.AppendFormat(" select * from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_SERVICEITEMS);
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
                 
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 获取服务合同列表，不限制创建人
        private string serviceContractList(HttpContext context)
        {
            JsonHelperEasyUi ui = new JsonHelperEasyUi();
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            string buyer = context.Request.Params["buyer"];
            string seller = context.Request.Params["seller"];
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.buyer  like '%'+@buyer+'%'  ");
            }
            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.seller  like '%'+@seller+'%' ");
            }
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                new SqlParameter("@createman",createman),
               new SqlParameter("@buyer",buyer),
               new SqlParameter("@seller",seller),
               new SqlParameter("@signedtime_begin",signedtime_begin),
               new SqlParameter("@signedtime_end",signedtime_end),
               new SqlParameter("@contractNo",contractNo)
                };
            //if (createman != null && createman != "admin")
            //{
            //    sqlshere.Append(" and t1.createman=@createman");
            //}
            sqldata.AppendFormat(" select * from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
            sqlcount.AppendFormat("select count(1) from {0} t1 where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        #endregion

        #region 获取合同列表
        //获取合同列表
        private string getContractList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string businessclass = context.Request.Params["businessclass"];
            string review = context.Request["review"];
            string flowdirection = context.Request["flowdirection"];
            string isConManage = context.Request["isConManage"] ?? string.Empty;
            StringBuilder sb = contractBll.GetContractList(contractNo, signedtime_begin, signedtime_end, row, page, order, sort, review, flowdirection, businessclass, isConManage);
            return sb.ToString();
        }
        #endregion

        #region 获取物流合同列表
        //获取物流合同列表
        private string logisticsContractList(HttpContext context)
        {
            JsonHelperEasyUi ui = new JsonHelperEasyUi();
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string buyer = context.Request.Params["buyer"];
            string seller = context.Request.Params["seller"];
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.logisticsContractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.buyer  like '%'+@buyer+'%'  ");
            }
            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.seller  like '%'+@seller+'%' ");
            }
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                new SqlParameter("@createman",createman),
               new SqlParameter("@buyer",buyer),
               new SqlParameter("@seller",seller),
               new SqlParameter("@signedtime_begin",signedtime_begin),
               new SqlParameter("@signedtime_end",signedtime_end),
               new SqlParameter("@contractNo",contractNo)
                };
            sqlshere.Append("and createman=@createman");
            sqldata.Append(" select * from Econtract_logistics where 1=1" + sqlshere.ToString());
            sqlcount.Append("select count(1) from Econtract_logistics where 1=1" + sqlshere.ToString());
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        #endregion

        #region 获取内部清算单列表
        private string internalClearingList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            string contractNo = context.Request.Params["contractNo"];
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            if (contractNo != null && contractNo.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.contractNo like '%'+@contractNo+'%' ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
            }
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
               new SqlParameter("@createman",createman),
                new SqlParameter("@contractTag",ConstantUtil.CONTRACTTAG_INTERNAL),
                  new SqlParameter("@contractTagTemp",ConstantUtil.CONTRACTTAG_INTERNALTEMP)
                };
            if (createman != null && createman != "admin")
            {
                sqlshere.Append(" and t1.createman=@createman");
            }
            sqlshere.Append(" and (contractTag=@contractTag or contractTag=@contractTagTemp)");
            sqldata.AppendFormat(@" select t1.*,t2.quantity,t2.amount from {0} t1 left join (select sum(quantity)as quantity,sum(amount) as amount ,contractNo from {1} group by contractNo)t2
              on t1.contractNo=t2.contractNo where 1=1 " + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL,ConstantUtil.TABLE_ECONTRACT_INTERNAL_AP);
            sqlcount.AppendFormat("select count(1) from {0} t1  where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        #endregion

        #region 创建进境内部清算单时要筛选的合同列表
        private string getInternalContract(HttpContext context)
        {
            string salesmanCode = context.Request.Params["saleman"];
            string productCategory = context.Request.Params["product"];
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            string flowdirection = context.Request.Params["flowdirection"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            if (productCategory != null && productCategory.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.productCategory=@productCategory ");
            }
            sqlshere.Append(" and datediff(day, createdate, GETDATE()) <365 ");//获取最近一年的数据
            sqlshere.AppendFormat(" and (t1.contractTag={0} or t1.contractTag={1})", ConstantUtil.CONTRACTTAG_MAINCON, ConstantUtil.CONTRACTTAG_CONATTACH);
            if (createman != "admin")
            {
                sqlshere.Append(" and t1.createman=@createman ");
            }

            sqlshere.AppendFormat(" and t1.flowdirection=@flowdirection");
            sqlshere.AppendFormat(" and t1.salesmanCode=@salesmanCode");
            sqlshere.AppendFormat(" and t1.frameContract='否'");

            sqldata.Append(@" select t1.*
                              from Econtract t1              
                              where " + sqlshere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@productCategory",Value=productCategory,DbType=DbType.String},
                     new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String},
                      new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                      new SqlParameter{ParameterName="@salesmanCode",Value=salesmanCode,DbType=DbType.String},
                     
                };
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }

        #endregion

        #endregion

        #region 获取管理合同数据
        private string LoadManageData(HttpContext context)
        {

            string contractNo = context.Request.Params["contractNo"];
            StringBuilder sb = new StringBuilder(string.Format(@"select id, contractNo, logisticsTemplateno,
logisticsTemplateName, frameContractNo, buyerCode, simpleBuyer, buyer, sellerCode, simpleSeller, seller,
signedPlace, signedTime, createman, createdate, status, salemanCode, isFrame, reviewtime, 
logisticsTag, frameContract, salesReviewNumber, adminReview, adminReviewNumber,
businessclass, salesmanCode, createmanname, ItemName, ItemAmount, validity,
currencyType,simplePartyC,partyC,simplePartyD,partyD,partyCCode,partyDCode from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_LOGISTICS));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }
        #endregion

        #region 获取新建服务子合同时根据时间业务员筛选框架合同，不限制创建人
        private string getServiceContract(HttpContext context)
        {
            string saleman = context.Request.Params["saleman"];
            string time = context.Request.Params["time"];
            string attachSaleman = context.Request.Params["attachSaleman"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");

            if (attachSaleman != null && attachSaleman.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.salemanCode=@attachSaleman ");
            }
            if (time != null && time.Trim().Length > 0 && time != "undefined")
            {
                sqlshere.Append(" and datediff(day, createdate, GETDATE()) <@time ");
            }
            sqlshere.Append(" and t1.isFrame=@isFrame ");
            //sqlshere.Append(" and t1.createman=@createman");
            sqlshere.Append(" and t1.serviceTag=@serviceTag");
            sqldata.AppendFormat(@" select t1.*
                              from {0} t1              
                              where " + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                       new SqlParameter{ParameterName="@attachSaleman",Value=attachSaleman,DbType=DbType.String},
                       new SqlParameter{ParameterName="@isFrame",Value='是',DbType=DbType.String},
                       new SqlParameter{ParameterName="@saleman",Value=saleman,DbType=DbType.String},
                       new SqlParameter{ParameterName="@serviceTag",Value=ConstantUtil.CONTRACT_SERVICE,DbType=DbType.String},
                       new SqlParameter{ParameterName="@time",Value=time,DbType=DbType.String},
                       new SqlParameter{ParameterName="@createman",Value=RequestSession.GetSessionUser().UserAccount,DbType=DbType.String},
                  
                };
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 获取工厂产品列表
        //获取所有产品列表
        private string GetProductList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string category = context.Request.QueryString["category"];
            string pcode = context.Request.Params["pcode"];
            string name = context.Request.Params["name"];
            StringBuilder sb = contractBll.GetProductList(pcode, name, category, row, page, order, sort);
            return sb.ToString();
        }
        #endregion

        #region 获取合同产品列表
        //获取合同产品列表
        private string GetContractProductList(HttpContext context)
        {

            string contarctno = context.Request.Params["contractNo"];
            StringBuilder sb = contractBll.GetContractProductList(contarctno);
            return sb.ToString();
        }
        //获取产品列表，根据是否为关联合同加载不同的价格
        private string getCplistByContact(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            //根据合同号判断其是否为关联合同
            StringBuilder sbSql = new StringBuilder(@"select purchaseCode from Econtract where contractNo=@contractNo");
            string purchaseCode = DataFactory.SqlDataBase().getString(sbSql, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "purchaseCode");
            //没有关联合同号，加载price
            if (string.IsNullOrEmpty(purchaseCode))
            {
                sqldata.Append(" select * from Econtract_ap where contractNo=@contractNo");
            }
            else
            {
                sqldata.Append(@"select contractNo, attachmentno, pcode, pname, quantity, qunit, priceUnit, amount, packspec, packing, pallet, spec, packdes, unit, ifcheck, ifplace, productCategory, hsCode, pnameen, pnameru, packagesNumber, priceAdd as price, amountfloat, skinWeight from Econtract_ap where contractNo=@contractNo");
            }


            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
                 
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 获取铁路发货产品列表
        //获取铁路发货产品列表
        private string traincplist(HttpContext context)
        {

            string contractNo = context.Request.Params["contractNo"];
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select pname as productName ,quantity as packagesNumber,qunit as weight,pallet as packagesType  from Econtract_ap 
                where contractNo=@contractNo");
            SqlParameter[] pms = new SqlParameter[]{
                    new SqlParameter("@contractNo",contractNo)
                };
            JsonHelperEasyUi jss = new JsonHelperEasyUi();
            return jss.GetDatatableJsonString(sb, pms).ToString();

        }
        #endregion

        #region 出口合同获取模板列表，根据创建人
        //出口合同获取模板列表，根据创建人
        private string GetExportTemplateList(HttpContext context)
        {

            string templatecategory = context.Request.Params["templatecategory"];
            string createman = RequestSession.GetSessionUser().UserName.ToString();
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            StringBuilder sb = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate<=@signedtime_end ");
            }
            //sb.AppendFormat(@"select * from {0} t1 where createmanname=@createman " + sqlshere.ToString(), ConstantUtil.TABLE_BTEMPLATE_EXPORTENCONTRACT);
            sb.AppendFormat(@"select * from {0} t1 where 1=1 " + sqlshere.ToString(), ConstantUtil.TABLE_BTEMPLATE_EXPORTENCONTRACT);
            return ui.GetDatatableJsonString(sb, new SqlParameter[3] { new SqlParameter("@signedtime_begin", signedtime_begin), new SqlParameter("@signedtime_end", signedtime_end), new SqlParameter("@createman", createman) }).ToString();
        }
        #endregion

        #region 进口合同获取模板列表，根据创建人
        //进口合同获取模板列表，根据创建人，模板类型筛选
        private string GetImportTemplateList(HttpContext context)
        {
            string templatecategory = context.Request.Params["templatecategory"];
            string createman = RequestSession.GetSessionUser().UserName.ToString();
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            StringBuilder sb = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate<=@signedtime_end ");
            }
            //sb.AppendFormat(@"select * from {0} t1 where createmanname=@createman " + sqlshere.ToString(), ConstantUtil.TABLE_BTEMPLATE_IMPORTENCONTRACT);
            sb.AppendFormat(@"select * from {0} t1 where 1=1 " + sqlshere.ToString(), ConstantUtil.TABLE_BTEMPLATE_IMPORTENCONTRACT);
            return ui.GetDatatableJsonString(sb, new SqlParameter[3] { new SqlParameter("@signedtime_begin", signedtime_begin), new SqlParameter("@signedtime_end", signedtime_end),new SqlParameter("@createman",createman) }).ToString();
          
        }

        #endregion

        #region 进境合同复制创建发货通知筛选创建过的发货通知
        //进境合同复制创建发货通知筛选创建过的发货通知
        private string getCopyImportSendContract(HttpContext context)
        {
            string saleman = context.Request.Params["saleman"];
            string time = context.Request.Params["time"];
            string seller = context.Request.Params["seller"];
            string buyer = context.Request.Params["buyer"];
            string productCategory = context.Request.Params["product"];
            string checkAttachTime = context.Request.Params["checkAttachTime"];
            string isFrame = context.Request.Params["isFrame"];
            string flowdirection = context.Request.Params["flowdirection"];
            string attachSaleman = context.Request.Params["attachSaleman"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            //StringBuilder sqlcount = new StringBuilder();

            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            if (attachSaleman != null && attachSaleman.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.salesmanCode=@attachSaleman ");
            }
            if (time != null && time.Trim().Length > 0)
            {
                sqlshere.Append(" and datediff(day, createdate, GETDATE()) <@time ");
            }
            if (isFrame == "true")
            {
                sqlshere.Append(" and t1.frameContract=@frameContract ");
            }
            //if (saleman != null && saleman.Trim().Length > 0)
            //{
            //    sqlshere.Append(" and t1.salesmanCode=@saleman ");
            //}
            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.seller=@seller ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.buyer=@buyer ");
            }
            if (productCategory != null && productCategory.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.productCategory=@productCategory ");
            }

            sqldata.Append(@" select t1.*
                              from Econtract t1              
                              where t1.contractNo like 'FH%' and " + sqlshere.ToString());
            //sqlcount.Append("select count(1) from Econtract t1 where " + sqlshere.ToString());

            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@attachSaleman",Value=attachSaleman,DbType=DbType.String},
                     new SqlParameter{ParameterName="@saleman",Value=saleman,DbType=DbType.String},
                    new SqlParameter{ParameterName="@productCategory",Value=productCategory,DbType=DbType.String},
                    new SqlParameter{ParameterName="@time",Value=time,DbType=DbType.String},
                    new SqlParameter{ParameterName="@seller",Value=seller,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String},
                     new SqlParameter{ParameterName="@frameContract",Value="是",DbType=DbType.String},
                   new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                  
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 筛选物流合同列表
        //筛选物流合同列表
        private string getLogisticsContract(HttpContext context)
        {
            string saleman = context.Request.Params["saleman"];
            string time = context.Request.Params["time"];
            string seller = context.Request.Params["seller"];
            string buyer = context.Request.Params["buyer"];
            string productCategory = context.Request.Params["product"];
            string checkAttachTime = context.Request.Params["checkAttachTime"];
            string isFrame = context.Request.Params["isFrame"];
            string flowdirection = context.Request.Params["flowdirection"];
            string attachSaleman = context.Request.Params["attachSaleman"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            //StringBuilder sqlcount = new StringBuilder();

            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            if (attachSaleman != null && attachSaleman.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.salesmanCode=@attachSaleman ");
            }
            if (time != null && time.Trim().Length > 0 && time != "undefined")
            {
                sqlshere.Append(" and datediff(day, createdate, GETDATE()) <@time ");
            }
            if (isFrame == "true")
            {
                sqlshere.Append(" and t1.frameContract=@frameContract ");
            }
            //if (saleman != null && saleman.Trim().Length > 0)
            //{
            //    sqlshere.Append(" and t1.salesmanCode=@saleman ");
            //}
            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.seller=@seller ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.buyer=@buyer ");
            }
            if (productCategory != null && productCategory.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.productCategory=@productCategory ");
            }
            sqlshere.Append(" and t1.createman=@createman");
            sqlshere.Append(" and t1.logisticsTag=@logisticsTag");
            sqldata.Append(@" select t1.*
                              from Econtract_logistics t1              
                              where " + sqlshere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                       new SqlParameter{ParameterName="@attachSaleman",Value=attachSaleman,DbType=DbType.String},
                     new SqlParameter{ParameterName="@saleman",Value=saleman,DbType=DbType.String},
                    new SqlParameter{ParameterName="@productCategory",Value=productCategory,DbType=DbType.String},
                    new SqlParameter{ParameterName="@time",Value=time,DbType=DbType.String},
                    new SqlParameter{ParameterName="@seller",Value=seller,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String},
                     new SqlParameter{ParameterName="@frameContract",Value="是",DbType=DbType.String},
                     new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                     new SqlParameter{ParameterName="@createman",Value=RequestSession.GetSessionUser().UserAccount,DbType=DbType.String},
                       new SqlParameter{ParameterName="@logisticsTag",Value=ConstantUtil.LOGINSTICSTAG,DbType=DbType.String},
                  
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 筛选进境框架合同下的子合同列表,根据框架合同号筛选
        //筛选进境框架合同下的子合同列表,根据框架合同号筛选
        private string getImportFrameInCon(HttpContext context)
        {

            string frameContractNo = context.Request.Params["frameContractNo"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            sqlshere.Append(" and t1.frameContractNo=@frameaContractNo");
            sqldata.Append(@" select t1.*
                              from Econtract t1              
                              where  " + sqlshere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                     new SqlParameter{ParameterName="@frameaContractNo",Value=frameContractNo,DbType=DbType.String},
                  
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 独立创建下创建框架合同附件复制创建筛选框架下的子合同
        private string getImportFrameAttachInCon(HttpContext context)
        {

            string frameAttachContractNo = context.Request.Params["frameAttachContractNo"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            sqlshere.Append(" and t1.frameAttachContractNo=@frameAttachContractNo");
            sqldata.Append(@" select t1.*
                              from Econtract t1              
                              where  " + sqlshere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                     new SqlParameter{ParameterName="@frameAttachContractNo",Value=frameAttachContractNo,DbType=DbType.String},
                  
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 筛选物流合同框架合同下的子合同列表
        //筛选物流合同框架合同下的子合同列表
        private string getLogisticsFrameInCon(HttpContext context)
        {
            string saleman = context.Request.Params["saleman"];
            string time = context.Request.Params["time"];
            string seller = context.Request.Params["seller"];
            string buyer = context.Request.Params["buyer"];
            string productCategory = context.Request.Params["product"];
            string checkAttachTime = context.Request.Params["checkAttachTime"];
            string isFrame = context.Request.Params["isFrame"];
            string flowdirection = context.Request.Params["flowdirection"];
            string attachSaleman = context.Request.Params["attachSaleman"];
            string logisticsContractNo = context.Request.Params["logisticsContractNo"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            //StringBuilder sqlcount = new StringBuilder();

            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            if (attachSaleman != null && attachSaleman.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.salesmanCode=@attachSaleman ");
            }
            if (time != null && time.Trim().Length > 0 && time != "undefined")
            {
                sqlshere.Append(" and datediff(day, createdate, GETDATE()) <@time ");
            }
            if (isFrame == "true")
            {
                sqlshere.Append(" and t1.frameContract=@frameContract ");
            }
            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.seller=@seller ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.buyer=@buyer ");
            }
            if (productCategory != null && productCategory.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.productCategory=@productCategory ");
            }
            sqlshere.Append(" and t1.frameaContractNo=@frameContractNo");
            sqldata.Append(@" select t1.*
                              from Econtract_logistics t1              
                              where  " + sqlshere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                       new SqlParameter{ParameterName="@attachSaleman",Value=attachSaleman,DbType=DbType.String},
                     new SqlParameter{ParameterName="@saleman",Value=saleman,DbType=DbType.String},
                    new SqlParameter{ParameterName="@productCategory",Value=productCategory,DbType=DbType.String},
                    new SqlParameter{ParameterName="@time",Value=time,DbType=DbType.String},
                    new SqlParameter{ParameterName="@seller",Value=seller,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String},
                   new SqlParameter{ParameterName="@frameContract",Value="是",DbType=DbType.String},
                   new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                     new SqlParameter{ParameterName="@frameContractNo",Value=logisticsContractNo,DbType=DbType.String},
                  
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 筛选最近365天的进境合同数据，根据创建人，时间,状态不为新建和退回

        //筛选最近365天的进境合同数据，根据创建人，时间,状态不为新建和退回，主合同和附件合同
        private string getContactContract(HttpContext context)
        {
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            string flowdirection = context.Request.Params["flowdirection"].ToString();
            string useraccount = RequestSession.GetSessionUser().UserAccount.ToString();
            StringBuilder sb = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate<=@signedtime_end ");
            }
            sb.AppendFormat(@"select * from {0} t1 where flowdirection='{1}' and (createman='{2}' or salesmancode='{2}')
             and status!='{3}'  and frameContract='否' and (contractTag='{4}'or contractTag='{5}'or contractTag='{6}')"+sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT,
               flowdirection, useraccount, ConstantUtil.STATUS_STOCKIN_NEW, ConstantUtil.CONTRACTTAG_MAINCON
               , ConstantUtil.CONTRACTTAG_CONATTACH, ConstantUtil.CONTRACT_OUTSIDE);
            return ui.GetDatatableJsonString(sb, new SqlParameter[2] { new SqlParameter("@signedtime_begin", signedtime_begin), new SqlParameter("@signedtime_end", signedtime_end) }).ToString();
        }
        #endregion

        #region 获取其关联合同的合同号
        //获取其关联合同的合同号
        private string getPurchaseCode(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"] ?? string.Empty;
            StringBuilder sb = new StringBuilder(@"select * from Econtract where purchaseCode=@contractNo");
            return DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "contractNo");
        }
        #endregion

        #region 新建合同时根据条件筛选合同
        //新建合同时根据条件筛选合同
        private string getContract(HttpContext context)
        {
            string saleman = context.Request.Params["saleman"];
            string time = context.Request.Params["time"];
            string seller = context.Request.Params["seller"];
            string buyer = context.Request.Params["buyer"];
            string productCategory = context.Request.Params["product"];
            string checkAttachTime = context.Request.Params["checkAttachTime"];
            string isFrame = context.Request.Params["isFrame"];
            string flowdirection = context.Request.Params["flowdirection"];
            string attachSaleman = context.Request.Params["attachSaleman"];
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            string isattachFrame = context.Request.Params["isattachFrame"];//是否为框架合同附件
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            //StringBuilder sqlcount = new StringBuilder();

            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            if (attachSaleman != null && attachSaleman.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.salesmanCode=@attachSaleman ");
            }
            if (time != null && time.Trim().Length > 0)
            {
                sqlshere.Append(" and datediff(day, createdate, GETDATE()) <@time ");
            }
            if (isFrame == "true")
            {
                sqlshere.Append(" and t1.frameContract=@frameContract ");
            }
            if (isattachFrame == "false")
            {
                sqlshere.Append(" and t1.frameContractNo is null ");
            }

            if (seller != null && seller.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.seller=@seller ");
            }
            if (buyer != null && buyer.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.buyer=@buyer ");
            }
            if (productCategory != null && productCategory.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.productCategory=@productCategory ");
            }
        
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate<=@signedtime_end ");
            }
            sqlshere.AppendFormat(" and t1.contractTag={0}", ConstantUtil.CONTRACTTAG_MAINCON);
            sqlshere.Append(" and t1.createman=@createman ");
            sqldata.Append(@" select t1.*
                              from Econtract t1              
                              where " + sqlshere.ToString());
            //sqlcount.Append("select count(1) from Econtract t1 where " + sqlshere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                       new SqlParameter{ParameterName="@attachSaleman",Value=attachSaleman,DbType=DbType.String},
                     new SqlParameter{ParameterName="@saleman",Value=saleman,DbType=DbType.String},
                    new SqlParameter{ParameterName="@productCategory",Value=productCategory,DbType=DbType.String},
                    new SqlParameter{ParameterName="@time",Value=time,DbType=DbType.String},
                    new SqlParameter{ParameterName="@seller",Value=seller,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String},
                     new SqlParameter{ParameterName="@frameContract",Value="是",DbType=DbType.String},
                   new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                     new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String},
                       new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                         new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                     
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region 发货已申请合同产品列表
        //发货已申请合同产品列表
        private string sendoutProductList(HttpContext context)
        {
            string createDateTag = context.Request.Params["createDateTag"];
            StringBuilder sb = contractBll.GetSendoutProductList(createDateTag);
            return sb.ToString();
        }
        #endregion

        #region 复制创建合同时筛选合同，获取最近30天的数据
        // 复制创建合同时筛选合同，获取最近30天的数据
        private string getCopyContract(HttpContext context)
        {
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            string flowdirection = context.Request.Params["flowdirection"].ToString();
            string useraccount = RequestSession.GetSessionUser().UserAccount.ToString();
            StringBuilder sb = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate<=@signedtime_end ");
            }
            sb.AppendFormat(@"select * from {0} t1 where flowdirection='{1}' and createman='{2}'
            and contractTag='{3}' "+sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT,
               flowdirection, useraccount, ConstantUtil.CONTRACTTAG_MAINCON);
            return ui.GetDatatableJsonString(sb, new SqlParameter[2] { new SqlParameter("@signedtime_begin", signedtime_begin), new SqlParameter("@signedtime_end", signedtime_end) }).ToString();
        }
        #endregion

        #region 获取审核信息

        //获取审核信息
        private string GetReviewContractList(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            StringBuilder sb = contractBll.GetReviewContractList(contractNo);
            return sb.ToString();
        }
        #endregion

        #region 获取服务合同审核信息

        //获取审核信息
        private string GetServiceReviewList(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            StringBuilder sb = contractBll.GetServiceReviewList(contractNo);
            return sb.ToString();
        }
        #endregion

        #region 获取物流条款信息
        private string logisticsItems(HttpContext context)
        {
            string logisticsContractNo = context.Request.Params["logisticsContractNo"];

            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            sqldata.Append(@"select *  from Econtract_logisticsItems where  logisticsContractNo=@logisticsContractNo");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@logisticsContractNo",Value=logisticsContractNo,DbType=DbType.String},
             
                };
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        #endregion

        #region  根据合同号获取产品信息和发货申请剩余量
        private string GetPrdutInfoRemain(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];

            StringBuilder sql = new StringBuilder(@"select ap.*,ISNULL((ap.quantity-t.sendQuantity),ap.quantity) as remain
                                                    from Econtract_ap ap
                                                    left join 
                                                    (
	                                                    select pcode,SUM(sendQuantity)as sendQuantity from SendoutAppDetails
	                                                    where contractNo='" + contractNo + @"'
	                                                    group by pcode
                                                    ) t on t.pcode=ap.pcode
                                                    where contractNo='" + contractNo + "'");

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, 2);
            if (dt != null && dt.Rows.Count > 0)
            {
                return "{\"total\":" + dt.Rows.Count + "," + JsonHelper.DataTableToJson_(dt, "rows") + "}";
            }
            return "[]";
        }
        #endregion

        #region 根据合同号获取产品信息和商检申请剩余量
        private string GetInsPrdutInfoRemain(HttpContext context)
        {
            string contractNo = context.Request.QueryString["contractNo"];

            StringBuilder sql = new StringBuilder(string.Format(@"select ap.*,(ap.quantity-ISNULL(t.sendQuantity,0)) as remain
                                                                    from Econtract_ap ap
                                                                    left join 
                                                                    (
	                                                                    select pcode,SUM(insQuantity)as sendQuantity from InspectionAppDetails
	                                                                    where contractNo='{0}'
	                                                                    group by pcode
                                                                    ) t on t.pcode=ap.pcode
                                                                    where contractNo='{0}'", contractNo));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, 2);
            if (dt != null && dt.Rows.Count > 0)
            {
                return "{\"total\":" + dt.Rows.Count + "," + JsonHelper.DataTableToJson_(dt, "rows") + "}";
            }
            return "[]";
        }
        #endregion

        #region 其他(获取价格有效期)
        private string getValidity(HttpContext context)
        {
            string validity = context.Request.Params["validity"];
            string contractNo = context.Request.Params["contractNo"];
            string status = context.Request.Params["status"];
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK1)//审批通过
            {
                StringBuilder sb = new StringBuilder(@"select reviewtime from Econtract where contractNo=@contractNo");
                //获取合同审批通过时间
                DateTime dt = new DateTime();
                string reviewdate = DataFactory.SqlDataBase().getString(sb, new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", ConstantUtil.STATUS_STOCKIN_CHECK1) }, "reviewtime");
                //return  DateTimeHelper.ToDate(reviewdate).ToString();
                dt = Convert.ToDateTime(reviewdate);
                int i = 0;
                int.TryParse(validity, out i);
                string ss = dt.AddDays(i).ToString();
                return ss;
            }
            else
            {
                return DateTime.Now.AddDays(4).ToString();
            }

        }
        #endregion

        #region 获取分批发货产品表

        //获取分批发货产品表
        private string loadReviewData(HttpContext context)
        {
            //string contractNo = context.Request.Params["contractNo"];
            string pname = context.Request.Params["pname"];
            string pcode = context.Request.Params["pcode"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();


            sqldata.Append(@" select *  from Econtract_split_a where  pname=@pname and pcode=@pcode ");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                     //new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@pname",Value=pname,DbType=DbType.String},
                    new SqlParameter{ParameterName="@pcode",Value=pcode,DbType=DbType.String},
              
                  
                };
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();

        }
        #endregion

        #region 获取适用于生成关联合同的合同列表,发货已完成的合同
        //获取适用于生成关联合同的合同列表,发货已完成的合同
        private string GetContractListByHK(HttpContext context)
        {
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
            //string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            string createman = RequestSession.GetSessionUser().UserName.ToString();
            StringBuilder sb = contractBll.GetContractListByHK(contractNo, signedtime_begin, signedtime_end, row, page, order, sort, review, flowdirection, transport, createman);
            return sb.ToString();
        }
        #endregion

        #region 作废关联合同
        private string CancelApply(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string createDateTag = context.Request.Params["createDateTag"];
            //根据合同号判断发货申请表中是否有多条申请，有多条申请就把合同表中sendoutStatus改为部分申请，否就改为未申请
            StringBuilder sb = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            sb.AppendFormat(@"select count(*) as count from {0} where contractNo='{1}'", ConstantUtil.TABLE_SENDOUTAPPDETAILS, contractNo);
            int count =Convert.ToInt32( DataFactory.SqlDataBase().getString(sb, "count"));
            if (count>1)
            {
                 sql.Append(string.Format("update {0} set sendoutStatus=1 where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
            }
            else
            {
                sql.Append(string.Format("update {0} set sendoutStatus=0 where contractNo='{1}'", ConstantUtil.TABLE_ECONTRACT, contractNo));
            }
            //sql.Append(string.Format("update {0} set isCancel=1 where contractNo='{1}' ", ConstantUtil.TABLE_ECONTRACT, contractNo));
            sql.Append(string.Format("delete {0} where createDateTag='{1}'", ConstantUtil.TABLE_SENDOUTAPPDETAILS, createDateTag));
            int cnt = DataFactory.SqlDataBase().ExecuteBySql(sql);
            return cnt > 0 ? "{\"result\":\"ok\",\"msg\":\"操作成功\"}" : "{\"result\":\"no\",\"msg\":\"操作错误\"}";
        }
        #endregion

        #region 报关单校对 铁路校对
        private string GetRailCheck(HttpContext context)
        {
            string Id = context.Request.QueryString["id"];//支付码id
            string contractNo = context.Request.QueryString["contractNo"];


            StringBuilder sql = new StringBuilder(string.Format(@"select pc.id,pc.productWeight as quantity,eap.pcode,eap.pname,eap.qunit,eap.price,eap.priceUnit,eap.price*pc.productWeight as amount,eap.packspec
                                                    from trainDelPayCode pc
                                                    join trainDeliveryNotice tn on tn.contractNo=pc.contractNo 
                                                    join Econtract_ap eap on eap.contractNo=pc.contractNo
                                                    where pc.contractNo='{0}' and pc.id={1}", contractNo, Id));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, 0);

            if (dt == null || dt.Rows.Count == 0) return "[]";
            else
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
        }
        #endregion

        #region 销售组json data
        private StringBuilder xszComboxData()
        {
            StringBuilder sb = new StringBuilder();

            DataTable dt = new DataTable();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                dt = bll.ExecDatasetSql(" select '不限' as NAME,'' as CODE union all  select NAME,CODE from BASE_DICTIONARY where PARENTID=18 ").Tables[0];
            }
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            return ui.ToEasyUIComboxJson(dt);
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

        #region 箱单发票方法
        private static void GetHeadLanguage(string language, DataTable sellerInfoTable, DataTable buyerInfoTable, ref string[] buyerInfo, ref string[] sellerInfo, ref string[] headTitle, ref string[] buyOrSell, ref string[] timeInvoiceContract, ref string totalCount)
        {
            switch (language)
            {
                #region zh
                case "":
                case "zh":
                    buyerInfo = new string[] {
                                    buyerInfoTable.Rows[0]["name"].ToString(), 
                                    buyerInfoTable.Rows[0]["address"].ToString(), 
                                    " ",
                                    " "
                                };
                    sellerInfo = new string[] {
                                    sellerInfoTable.Rows[0]["name"].ToString(), 
                                    sellerInfoTable.Rows[0]["address"].ToString(), 
                                    "",
                                    "" 
                                };
                    headTitle = new string[]{
                                    "发票",
                                    "箱单"
                                };
                    buyOrSell = new string[]{
                                    " ",
                                    " "
                                };
                    timeInvoiceContract = new string[]{
                                    "时间",
                                    "发票号",
                                    "合同号"
                                };
                    //totalCount = "<p style='font-size:15px;' style='font-size:15px;'>总件数：{0}Bags</p>";
                    totalCount = "";
                    break;
                #endregion
                #region eg
                case "eg":
                    buyerInfo = new string[] {
                                    "",
                                    "",
                                    buyerInfoTable.Rows[0]["egname"].ToString(),
                                    buyerInfoTable.Rows[0]["egaddress"].ToString()
                                };
                    sellerInfo = new string[] {
                                    "" ,
                                    "",
                                    sellerInfoTable.Rows[0]["egname"].ToString(),
                                    sellerInfoTable.Rows[0]["egaddress"].ToString()
                                };
                    headTitle = new string[]{
                                    "INVOICE",
                                    "PACKING LIST"
                                };
                    buyOrSell = new string[]{
                                    "SELLER",
                                    "BUYER"
                                };
                    timeInvoiceContract = new string[]{
                                    "DATE",
                                    "INVOICE No",
                                    "CONTRACT No"
                                };
                    totalCount = "<p style='font-size:15px;' style='font-size:15px;'>Total pakages:{0}Bags</p>";
                    break;
                #endregion
                #region rs
                case "rs":
                    buyerInfo = new string[] { 
                                    "",
                                    "",
                                    buyerInfoTable.Rows[0]["rsname"].ToString(),
                                    buyerInfoTable.Rows[0]["rsaddress"].ToString()
                                };
                    sellerInfo = new string[] {
                                    "",
                                    "" ,
                                    sellerInfoTable.Rows[0]["rsname"].ToString(),
                                    sellerInfoTable.Rows[0]["rsaddress"].ToString()
                                };
                    headTitle = new string[]{
                                    "счет-фактура",
                                    "упаковочный лист"
                                };
                    buyOrSell = new string[]{
                                    "ПРОДАВЕЦ",
                                    "ПОКУПАТЕЛЬ"
                                };
                    timeInvoiceContract = new string[]{
                                    "ДАТА",
                                    "номер накладной",
                                    "номер контракта"
                                };
                    totalCount = "<p style='font-size:15px;' style='font-size:15px;'>Общее число:{0}</p>";
                    break;
                #endregion
                #region zheg
                case "zheg":
                    buyerInfo = new string[] {
                                    buyerInfoTable.Rows[0]["name"].ToString(), 
                                    buyerInfoTable.Rows[0]["address"].ToString(), 
                                    buyerInfoTable.Rows[0]["egname"].ToString(),
                                    buyerInfoTable.Rows[0]["egaddress"].ToString()
                                };
                    sellerInfo = new string[] {
                                    sellerInfoTable.Rows[0]["name"].ToString(), 
                                    sellerInfoTable.Rows[0]["address"].ToString(), 
                                    sellerInfoTable.Rows[0]["egname"].ToString(),
                                    sellerInfoTable.Rows[0]["egaddress"].ToString() 
                                };
                    headTitle = new string[]{
                                    "INVOICE 发票",
                                    "PACKING LIST 箱单"
                                };
                    buyOrSell = new string[]{
                                    "SELLER",
                                    "BUYER"
                                };
                    timeInvoiceContract = new string[]{
                                    "DATE 时间",
                                    "INVOICE No 发票号",
                                    "CONTRACT No 合同号"
                                };
                    //totalCount = "<p style='font-size:15px;' style='font-size:15px;'>Total pakages:{0}Bags &#12288;&#12288;&#12288;&#12288; 总件数：{0}Bags</p>";
                    totalCount = "";
                    break;
                #endregion
                #region zhrs
                case "zhrs":
                    buyerInfo = new string[] {
                                    buyerInfoTable.Rows[0]["name"].ToString(), 
                                    buyerInfoTable.Rows[0]["address"].ToString(), 
                                    buyerInfoTable.Rows[0]["rsname"].ToString(),
                                    buyerInfoTable.Rows[0]["rsaddress"].ToString()
                                };
                    sellerInfo = new string[] {
                                    sellerInfoTable.Rows[0]["name"].ToString(), 
                                    sellerInfoTable.Rows[0]["address"].ToString(), 
                                    sellerInfoTable.Rows[0]["rsname"].ToString(),
                                    sellerInfoTable.Rows[0]["rsaddress"].ToString() 
                                };
                    headTitle = new string[]{
                                    "счет-фактура 发票",
                                    "упаковочный лист 箱单"
                                };
                    buyOrSell = new string[]{
                                    "ПРОДАВЕЦ",
                                    "ПОКУПАТЕЛЬ"
                                };
                    timeInvoiceContract = new string[]{
                                    "ДАТА 时间",
                                    "номер накладной 发票号",
                                    "номер контракта 合同号"
                                };
                    //totalCount = "<p style='font-size:15px;' style='font-size:15px;'>Общее число:{0}Bags &#12288;&#12288;&#12288;&#12288; 总件数：{0}Bags</p>";
                    totalCount = "";
                    break;
                #endregion
                default:
                    break;
            }
        }
        private string GetSeqNo()
        {
            return System.DateTime.Now.ToString("yyyy") + "0010";
        }
        //获取是销售合同还是采购合同
        private string getGYorXS(string buyer, string seller)
        {
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {

                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyer),
                        new SqlParameter("@seller",seller)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];
                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        return "GY";

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        return "XS";
                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    return "XS";
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    return "GY";
                }

            }


        }

        /// <summary>
        /// 获取发票表单上方的信息
        /// </summary>
        /// <param name="dtContract"></param>
        /// <param name="contractTable"></param>
        /// <returns></returns>
        private static string GetTableUpSection(ref DataSet dtContract, string tradement, string harborout, string harborarrive, string transport, string pricement1, string language)
        {
            string FromName = harborout;
            string ToName = harborarrive;
            StringBuilder sb = new StringBuilder(@"select code from bharbor where name=@name");
            string FromCode = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@name", FromName.Trim()) }, "code");
            string ToCode = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@name", ToName.Trim()) }, "code");
            StringBuilder sbTrade = new StringBuilder(@"select * from BASE_DICTIONARY where PARENTID='114' and Name=@name");
            string tradementeng = DataFactory.SqlDataBase().getString(sbTrade, new SqlParam[1] { new SqlParam("@name", tradement.Trim()) }, "ENGLISH");
            string tradementrus = DataFactory.SqlDataBase().getString(sbTrade, new SqlParam[1] { new SqlParam("@name", tradement.Trim()) }, "RUSSIAN");
            string[] shippInfo = null;
            string[] paymentInfo = null;

            string uperTable = "<table class='prodetail ke-zeroborder' style='width:100%;' cellpadding='0' border='0'><tbody>";
            string[] FromToInfo = null;
            if (!string.IsNullOrWhiteSpace(FromCode) && !string.IsNullOrWhiteSpace(ToCode))
            {
                //获取合同信息
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    SqlParameter[] pms = new System.Data.SqlClient.SqlParameter[] { 
                        new SqlParameter("@harbor1",FromCode),
                        new SqlParameter("@harbor2",ToCode),
                        new SqlParameter("@transport",transport),
                        new SqlParameter("@paytype",pricement1)
                        };
                    dtContract = bll.ExecDatasetSql(
                             @"SELECT * FROM bharbor WHERE code=@harbor1;
                              SELECT * FROM bharbor WHERE code=@harbor2;
                              SELECT * FROM BASE_DICTIONARY Where NAME=@transport;
                              SELECT * FROM BASE_DICTIONARY Where CODE =@paytype;",
                            pms);
                }
                FromToInfo = new string[12];
                if (dtContract.Tables[0].Rows.Count > 0)
                {
                    FromToInfo[0] = dtContract.Tables[0].Rows[0]["country"].ToString();//国家
                    FromToInfo[1] = dtContract.Tables[0].Rows[0]["countryeng"].ToString();
                    FromToInfo[2] = dtContract.Tables[0].Rows[0]["countryrus"].ToString();
                    FromToInfo[6] = dtContract.Tables[0].Rows[0]["name"].ToString();//发运港口
                    FromToInfo[7] = dtContract.Tables[0].Rows[0]["egname"].ToString();
                    FromToInfo[8] = dtContract.Tables[0].Rows[0]["runame"].ToString();
                }
                if (dtContract.Tables[1].Rows.Count > 0)
                {
                    FromToInfo[3] = dtContract.Tables[1].Rows[0]["country"].ToString();
                    FromToInfo[4] = dtContract.Tables[1].Rows[0]["countryeng"].ToString();
                    FromToInfo[5] = dtContract.Tables[1].Rows[0]["countryrus"].ToString();
                    FromToInfo[9] = dtContract.Tables[1].Rows[0]["name"].ToString();//目的港口
                    FromToInfo[10] = dtContract.Tables[1].Rows[0]["egname"].ToString();
                    FromToInfo[11] = dtContract.Tables[1].Rows[0]["runame"].ToString();
                }
                shippInfo = new string[3];
                if (dtContract.Tables[2].Rows.Count > 0)
                {
                    shippInfo[0] = dtContract.Tables[2].Rows[0]["NAME"].ToString();
                    shippInfo[1] = dtContract.Tables[2].Rows[0]["ENGLISH"].ToString();
                    shippInfo[2] = dtContract.Tables[2].Rows[0]["RUSSIAN"].ToString();
                };
                paymentInfo = new string[3];
                if (dtContract.Tables[3].Rows.Count > 0)
                {
                    paymentInfo[0] = dtContract.Tables[3].Rows[0]["NAME"].ToString();
                    paymentInfo[1] = dtContract.Tables[3].Rows[0]["ENGLISH"].ToString();
                    paymentInfo[2] = dtContract.Tables[3].Rows[0]["RUSSIAN"].ToString();
                };
            }
            else
            {
                FromToInfo = new string[12];
                shippInfo = new string[3];
                paymentInfo = new string[3];
            }
            string FromTo = string.Empty;
            string ship = string.Empty;
            string payment = string.Empty;
            string tradeMent = string.Empty;
            switch (language)
            {
                case "":
                case "zh":
                    FromTo = string.Format(@"<tr><td><p  style='font-size:15px;'>发运站：{0}&nbsp;{1} </p></td></tr>
                                             <tr><td><p  style='font-size:15px;'>目的地：{2}&nbsp;{3}</p></td></tr>
                                            ",
                        FromToInfo[0], FromToInfo[6], FromToInfo[3], FromToInfo[9]);
                    ship = string.Format(@"<tr><td><p style='font-size:15px;'>运输方式：{0}</p></td></tr>", shippInfo[0]);
                    tradeMent = string.Format("<tr><td><p style='font-size:15px;'>贸易方式:{0}</p></td></tr>", tradement);
                    payment = string.Format("<tr><td><p style='font-size:15px;'>付款方式:{0}</p></td></tr>", paymentInfo[0]);
                    break;
                case "eg":
                    FromTo = string.Format(@"<tr><td><p  style='font-size:15px;'>FROM：{0}&nbsp;{1}  </p></td></tr>
                                             <tr><td><p  style='font-size:15px;'>TO：{2}&nbsp;{3}</p></td></tr>",
                        FromToInfo[1], FromToInfo[7], FromToInfo[4], FromToInfo[10]);
                    ship = string.Format("<tr><td><p style='font-size:15px;'>SHIPPED PER:{0}</p></td></tr>", shippInfo[1]);
                    tradeMent = string.Format("<tr><td><p style='font-size:15px;'>TradeMent:{0} </p></td></tr>", tradementeng);
                    payment = string.Format("<tr><td><p style='font-size:15px;'>Payment:{0} </p></td></tr>", paymentInfo[1]);
                    break;
                case "rs":
                    FromTo = string.Format(@"<tr><td><p  style='font-size:15px;'>станция отправления：{0}&nbsp;{1} </p></td></tr>
                                            <tr><td><p  style='font-size:15px;'>  Прибытие станции：{2}&nbsp;{3}</td></tr> ",
                        FromToInfo[2], FromToInfo[8], FromToInfo[5], FromToInfo[11]);
                    ship = string.Format("<tr><td><p style='font-size:15px;'>транспорт:{0} </p></td></tr>", shippInfo[2]);
                    tradeMent = string.Format("<tr><td><p style='font-size:15px;'>Условия торговли:{0} </p></td></tr>", tradementrus);
                    payment = string.Format("<tr><td><p style='font-size:15px;'>оплата:{0} </p></td></tr>", paymentInfo[2]);
                    break;
                case "zheg":
                    FromTo = string.Format(@"<tr><td><p  style='font-size:15px;'>FROM：{0}&nbsp;{1}</p> </td> <td> <p style='font-size:15px;'> 发运站：{2}&nbsp;{3} </p></td></tr> 
                                             <tr><td><p  style='font-size:15px;'> TO：{4}&nbsp;{5}</p> </td> <td> <p style='font-size:15px;'>目的地：{6}&nbsp;{7}</p></td></tr> ",
                        FromToInfo[1], FromToInfo[7], FromToInfo[0], FromToInfo[6], FromToInfo[4], FromToInfo[10], FromToInfo[3], FromToInfo[9]);
                    ship = string.Format(@"<tr><td><p style='font-size:15px;'>SHIPPED PER:{0}</p> </td> <td> <p style='font-size:15px;'> 运输方式：{1}</p></td></tr>",
                        shippInfo[1], shippInfo[0]);
                    tradeMent = string.Format(@"<tr><td><p style='font-size:15px;'>TradeMent:{0} </p> </td> <td> <p style='font-size:15px;'> 贸易方式:{1}</p></td></tr>",
                       tradementeng, tradement);
                    payment = string.Format(@"<tr><td><p style='font-size:15px;'>Payment:{0} </p> </td> <td> <p style='font-size:15px;'>  付款方式:{1}</p></td></tr>",
                        paymentInfo[1], paymentInfo[0]);
                    break;
                case "zhrs":
                    FromTo = string.Format(@"<tr><td><p  style='font-size:15px;'>станция отправления：{0}&nbsp;{1}</p> </td> <td> <p style='font-size:15px;'> 发运站：{2}&nbsp;{3} </p></td></tr>
                                             <tr><td><p  style='font-size:15px;'>  Прибытие станции：{4}&nbsp;{5} </p> </td> <td> <p style='font-size:15px;'> 目的地：{6}&nbsp;{7}</td></tr>",
                        FromToInfo[2], FromToInfo[8], FromToInfo[0], FromToInfo[6], FromToInfo[5], FromToInfo[11], FromToInfo[3], FromToInfo[9]);
                    ship = string.Format("<tr><td><p style='font-size:15px;'>транспорт:{0} </p> </td> <td> <p style='font-size:15px;'> 运输方式：{1}</p></td></tr>",
                        shippInfo[2], shippInfo[0]);
                    tradeMent = string.Format("<tr><td><p style='font-size:15px;'>Условия торговли:{0} </p> </td> <td> <p style='font-size:15px;'> 贸易方式:{1}</p></td></tr>",
                        tradementrus, tradement);
                    payment = string.Format("<tr><td><p style='font-size:15px;'>оплата:{0}</p> </td> <td> <p style='font-size:15px;'>付款方式:{1}</p></td></tr>",
                        paymentInfo[2], paymentInfo[0]);
                    break;
                default: break;
            }
            uperTable += FromTo + ship + tradeMent + payment + "</tbody></table>";
            return uperTable;
        }

        private static string GetInvoiceTable(string productlist, string currency, string tradement, out int sumCnt, string language)
        {
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(productlist);
            string table = "<table  width='100%'  align='center' class='prodetail' border='1' bordercolor='#a0c6e5' cellpadding=3 style='border-collapse:collapse; font-size:15px;'>";
            string tableHead = string.Empty;
            string rows = string.Empty;

            switch (language)
            {
                case "":
                case "zh":
                    tableHead = @"<tr><td style='font-size:15px;'>唛头</td><td style='font-size:15px;'>货描</td><td style='font-size:15px;'>数量</td><td style='font-size:15px;'>单位</td><td style='font-size:15px;'>单价</td><td style='font-size:15px;'>总金额</td><td style='font-size:15px;'>币制</td><tr>";
                    break;
                case "eg":
                    tableHead = @"<tr><td style='font-size:15px;'>Marks and No.</td><td style='font-size:15px;'>Description </td><td style='font-size:15px;'>Quantity</td><td style='font-size:15px;'>Unit</td><td style='font-size:15px;'>Price</td><td style='font-size:15px;'>Amount</td><td style='font-size:15px;'>Currency</td><tr>";
                    break;
                case "rs":
                    tableHead = @"<tr><td style='font-size:15px;'>маркировка</td><td style='font-size:15px;'>Наименование/Марка</td><td style='font-size:15px;'> количество</td><td style='font-size:15px;'>единица</td><td style='font-size:15px;'> цена за единицу</td><td style='font-size:15px;'>общая сумма</td><td style='font-size:15px;'>валюты</td><tr>";
                    break;
                case "zheg":
                    tableHead = @"<tr><td style='font-size:15px;'>Marks and No.<br/> 唛头</td><td style='font-size:15px;'>Description <br/> 货描</td><td style='font-size:15px;'>Quantity<br/>数量</td><td style='font-size:15px;'>Unit<br/>单位</td><td style='font-size:15px;'>Price<br/>单价</td><td style='font-size:15px;'>Amount <br/>总金额</td><td style='font-size:15px;'>Currency<br/>币制</td><tr>";
                    break;
                case "zhrs":
                    tableHead = @"<tr><td style='font-size:15px;'>маркировка <br/> 唛头</td><td style='font-size:15px;'>Наименование/Марка  <br/>货描</td><td style='font-size:15px;'> количество <br/>数量</td><td style='font-size:15px;'>единица <br/>单位</td><td style='font-size:15px;'> цена за единицу <br/>单价</td><td style='font-size:15px;'>общая сумма <br/>总金额</td><td style='font-size:15px;'>валюты <br/>币制</td><tr>";
                    break;

            }
            table += tableHead;
            double sumAmount = 0;
            sumCnt = 0;//总件数
            table += GetInvoiceTBody(listtable, currency, tradement, ref sumCnt, ref sumAmount, language);
            string moneyCN = Ecan.EcanRMB.CmycurD(sumAmount.ToString());
            contractBLL bll = new contractBLL();
            string tableFoot = "";
            switch (language)
            {
                case "":
                case "zh":
                    tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>总金额：{0}</td><tr>",
                        //moneyCN.Substring(0, moneyCN.Length - 2) + (currency == "美元" ? "美元" : "元"));
                                       bll.getCurrency(currency, moneyCN));
                    //moneyCN.Substring(0, moneyCN.Length - 2) +bll.getCurrency(currency));

                    break;
                case "eg":
                    tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>TOATAL　AMOUNT:SAY {0} ONLY</td><tr>",
                              (currency) + Util.NumberToEnglishString((int)sumAmount).ToUpper());
                    break;
                case "rs":
                    tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>Общая сумма :{0}</td><tr>",
                                sumAmount);
                    break;
                case "zheg":
                    tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>TOATAL　AMOUNT:SAY {0} ONLY<br/>总金额：{1}</td><tr>",
                                 (currency) + Util.NumberToEnglishString((int)sumAmount).ToUpper(),
                                    bll.getCurrency(currency, moneyCN));
                    //moneyCN.Substring(0, moneyCN.Length - 2) + (bll.getCurrency(currency)));
                    break;
                case "zhrs":
                    tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>Общая сумма : {0} <br/>总金额：{1}</td><tr>",
                                 sumAmount,
                                bll.getCurrency(currency, moneyCN));
                    //moneyCN.Substring(0, moneyCN.Length - 2) + (bll.getCurrency(currency) == "美元" ? "美元" : "元"));
                    break;
            }
            //switch (language)
            //{
            //    case "":
            //    case "zh":
            //        tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>总金额：{0}整</td><tr>",
            //            //moneyCN.Substring(0, moneyCN.Length - 2) + (currency == "美元" ? "美元" : "元"));
            //                    moneyCN.Substring(0, moneyCN.Length - 2) + bll.getCurrency(currency));

            //        break;
            //    case "eg":
            //        tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>TOATAL　AMOUNT:SAY {0} ONLY</td><tr>",
            //                    (bll.getCurrency(currency) == "美元" ? "US DOLLARS " : "YUAN ") + Util.NumberToEnglishString((int)sumAmount).ToUpper());
            //        break;
            //    case "rs":
            //        tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>Общая сумма :{0}</td><tr>",
            //                    sumAmount);
            //        break;
            //    case "zheg":
            //        tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>TOATAL　AMOUNT:SAY {0} ONLY<br/>总金额：{1}整</td><tr>",
            //                    (bll.getCurrency(currency) == "美元" ? "US DOLLARS " : "YUAN ") + Util.NumberToEnglishString((int)sumAmount).ToUpper(),
            //                    moneyCN.Substring(0, moneyCN.Length - 2) + (bll.getCurrency(currency)));
            //        break;
            //    case "zhrs":
            //        tableFoot = string.Format("<tr><td colspan='7' style='font-size:15px;'>Общая сумма : {0} <br/>总金额：{1}整</td><tr>",
            //                     sumAmount,
            //                    moneyCN.Substring(0, moneyCN.Length - 2) + (bll.getCurrency(currency)));
            //        break;
            //}
            table += tableFoot + "</table>";
            return table;
        }
        private static string GetInvoiceTBody(List<Hashtable> listtable, string currency, string tradement, ref int sumCnt, ref double sumAmount, string language)
        {
            contractBLL bll = new contractBLL();
            string table = "";
            foreach (Hashtable row in listtable)
            {
                string pname = string.Empty;
                if (language == "zheg")
                {
                    pname = row["pnameen"] + "<br/>" + row["pname"].ToString() + row["spec"].ToString();
                }
                if (language == "zh")
                {
                    pname = row["pname"].ToString() + row["spec"];
                }
                if (language == "eg")
                {
                    pname = row["pnameen"].ToString() + row["spec"].ToString();
                }
                if (language == "zhrs")
                {
                    pname = row["pnameru"] + "<br/>" + row["pname"].ToString() + row["spec"].ToString();
                }
                if (language == "rs")
                {
                    pname = row["pnameru"].ToString() + row["spec"].ToString();
                }
                string unit = row["qunit"].ToString();
                string quantity = row["quantity"].ToString();
                string price = string.Empty;
                string priceAdd = row["priceAdd"] == null ? string.Empty : row["priceAdd"].ToString();
                if (!string.IsNullOrEmpty(priceAdd))
                {
                    price = row["priceAdd"].ToString();
                }
                else
                {
                    price = row["price"].ToString();
                }
                string amount = row["amount"].ToString();
                int cnt = 0;
                string packagesNumber = row["packagesNumber"].ToString();
                int.TryParse(packagesNumber, out cnt);
                sumCnt += cnt;
                sumAmount += Convert.ToDouble(amount);
                table += string.Format(@"<tr>
                                            <td style='font-size:15px;'>{0}</td>
                                            <td style='font-size:15px;'>{1}</td>
                                            <td style='font-size:15px;'>{2}</td>
                                            <td style='font-size:15px;'>{3}</td>
                                            <td style='font-size:15px;'>{4}</td>
                                            <td style='font-size:15px;'>{5}</td>
                                            <td align='center' style='font-size:15px;'>{6}</td>
                                        <tr>",
                    "无",
                     pname,//名称
                     quantity,//数量
                    unit,//单位
                    price,//单价
                    Convert.ToDouble(quantity) * Convert.ToDouble(price),//总价
                    currency
                    );
            }
            return table;
        }

        private static string GetPackingTable(StringBuilder sb, string productlist, string language)
        {
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(productlist);
            string table = "<table  width='100%'  align='center' class='prodetail' border='1' bordercolor='#a0c6e5' cellpadding=3 style='border-collapse:collapse; font-size:15px;'>";
            switch (language)
            {
                case "":
                case "zh":
                    table += @"<tr>
                                    <td style='font-size:15px;'>货描</td>
                                    <td style='font-size:15px;'>规格</td>
                                    <td style='font-size:15px;'>包装描述</td>
                                    <td style='font-size:15px;'>件数</td>
                                    <td style='font-size:15px;'>净重</td>
                                    <td style='font-size:15px;'>毛重</td>
                                <tr>";
                    break;
                case "eg":
                    table += @"<tr>
                                    <td style='font-size:15px;'>Description</td>
                                    <td style='font-size:15px;'>Specifications</td>
                                    <td style='font-size:15px;'>Packing</td>
                                    <td style='font-size:15px;'>Packages</td>
                                    <td style='font-size:15px;'>Net weight</td>
                                    <td style='font-size:15px;'>Gross weight</td>
                                <tr>";
                    break;
                case "rs":
                    table += @"<tr><td style='font-size:15px;'>Наименование/Марка</td>
                                    <td style='font-size:15px;'>спецификации</td>
                                    <td style='font-size:15px;'>описание упаковки</td>
                                     <td style='font-size:15px;'>число</td>
                                    <td style='font-size:15px;'>вес нетто</td>
                                    <td style='font-size:15px;'>вес брутто</td>
                               <tr>";
                    break;
                case "zheg":
                    table += @"<tr>
                                    <td style='font-size:15px;'>Description<br/>货描</td>
                                    <td style='font-size:15px;'>Specifications<br/>规格</td>
                                    <td style='font-size:15px;'>Packing<br/>包装描述</td>
                                     <td style='font-size:15px;'>Packages<br/>件数</td>
                                    <td style='font-size:15px;'>Net weight<br/>净重</td>
                                    <td style='font-size:15px;'>Gross weight<br/>毛重</td>
                                <tr>";
                    break;

                case "zhrs":
                    table += @"<tr><td style='font-size:15px;'>Наименование/Марка<br/>货描</td>
                                    <td style='font-size:15px;'>спецификации<br/>规格</td>
                                    <td style='font-size:15px;'>описание упаковки<br/>包装描述</td>
                                    <td style='font-size:15px;'>число<br/>件数</td>
                                    <td style='font-size:15px;'>вес нетто<br/>净重</td>
                                    <td style='font-size:15px;'>вес брутто<br/>毛重</td>
                               <tr>";
                    break;
            }
            foreach (Hashtable row in listtable)
            {
                int cnt = 0;
                string packagesNumber = row["packagesNumber"].ToString();
                int.TryParse(packagesNumber, out cnt);
                decimal skin = 0.000M;
                string skinWeight = row["skinWeight"].ToString();
                decimal.TryParse(skinWeight, out skin);
                decimal quantity = 0.000M;
                string quantityString = row["quantity"].ToString();
                decimal.TryParse(quantityString, out quantity);
                decimal grossWeight = skin + quantity;
                if (language.Equals("zh") || language.Equals(""))
                {
                    table += string.Format(@"<tr>
                            <td style='font-size:15px;'>{0}</td>
                            <td style='font-size:15px;'>{1}</td>
                            <td style='font-size:15px;'>{2}</td>
                            <td style='font-size:15px;'>{3}</td>
                            <td style='font-size:15px;'>{4}</td>
                             <td style='font-size:15px;'>{5}</td>
                        </tr>",
                        row["pname"].ToString() + row["spec"].ToString(),
                        row["spec"],
                        row["pallet"].ToString() + row["unit"].ToString() + row["packdes"].ToString(),
                        cnt,
                        row["quantity"].ToString() + row["qunit"].ToString(),
                            grossWeight.ToString("#0.000") + row["qunit"].ToString());//毛重，净重+皮重
                }
                if (language.Equals("eg"))
                {
                    table += string.Format(@"<tr>
                            <td style='font-size:15px;'>{0}</td>
                            <td style='font-size:15px;'>{1}</td>
                            <td style='font-size:15px;'>{2}</td>
                            <td style='font-size:15px;'>{3}</td>
                            <td style='font-size:15px;'>{4}</td>
                            <td style='font-size:15px;'>{5}</td>
                        </tr>",
                        row["pnameen"].ToString() + row["spec"].ToString(),
                        row["spec"],
                        row["pallet"].ToString() + row["unit"].ToString() + row["packdes"].ToString(),
                        cnt,
                        row["quantity"].ToString() + row["qunit"].ToString(),
                             grossWeight.ToString("#0.000") + row["qunit"].ToString());//毛重，净重+皮重
                }
                if (language.Equals("rs"))
                {
                    table += string.Format(@"<tr>
                            <td style='font-size:15px;'>{0}</td>
                            <td style='font-size:15px;'>{1}</td>
                            <td style='font-size:15px;'>{2}</td>
                            <td style='font-size:15px;'>{3}</td>
                            <td style='font-size:15px;'>{4}</td>
                            <td style='font-size:15px;'>{5}</td>
                        </tr>",
                        row["pnameru"].ToString() + row["spec"].ToString(),
                        row["spec"],
                        row["pallet"].ToString() + row["unit"].ToString() + row["packdes"].ToString(),
                        cnt,
                        row["quantity"].ToString() + row["qunit"].ToString(),
                              grossWeight.ToString("#0.000") + row["qunit"].ToString());//毛重，净重+皮重
                }
                if (language.Equals("zheg"))
                {
                    table += string.Format(@"<tr>
                            <td style='font-size:15px;'>{0}</td>
                            <td style='font-size:15px;'>{1}</td>
                            <td style='font-size:15px;'>{2}</td>
                            <td style='font-size:15px;'>{3}</td>
                            <td style='font-size:15px;'>{4}</td>
                            <td style='font-size:15px;'>{5}</td>
                        </tr>",
                        row["pnameen"] + "<br/>" + row["pname"].ToString() + row["spec"].ToString(),
                        row["spec"],
                        row["pallet"].ToString() + row["unit"].ToString() + row["packdes"].ToString(),
                        cnt,
                        row["quantity"].ToString() + row["qunit"].ToString(),
                        grossWeight.ToString("#0.000") + row["qunit"].ToString());//毛重，净重+皮重
                }
                if (language.Equals("zhrs"))
                {
                    table += string.Format(@"<tr>
                            <td style='font-size:15px;'>{0}</td>
                            <td style='font-size:15px;'>{1}</td>
                            <td style='font-size:15px;'>{2}</td>
                            <td style='font-size:15px;'>{3}</td>
                            <td style='font-size:15px;'>{4}</td>
                            <td style='font-size:15px;'>{5}</td>
                        </tr>",
                         row["pnameru"] + "<br/>" + row["pname"].ToString() + row["spec"].ToString(),
                        row["spec"],
                        row["pallet"].ToString() + row["unit"].ToString() + row["packdes"].ToString(),
                        cnt,
                        row["quantity"].ToString() + row["qunit"].ToString(),
                        grossWeight.ToString("#0.000") + row["qunit"].ToString());//毛重，净重+皮重
                }
            }

            table += "</table>";
            return table;
        }
        #endregion

        #region 创建关联合同直接发货时初始化商检合同信息
        private string initInpectInfo(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string info = string.Empty;
            //根据合同号查询商检合同表中数据
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"select 
 t1.*,t2.inspectStyle from Econtract_Inspect t1,InspectionAppDetails t2 where  t1.purchaseCode=@contractNo
 and t1.purchaseCode=t2.contractNo");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0);
            if (dt.Rows.Count > 0)
            {
                info = JsonHelper.DataRowToJson_(dt.Rows[0]);
                info = info.Replace("&nbsp;", "");
            }
            else
            {
                info = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}";
            }
            return info;
        }
        #endregion

        #region 创建承兑上传excel文件列表

        private string getAcceptList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            string acceptno = context.Request.Params["acceptno"];
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();

            if (acceptno != null && acceptno.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.acceptno like '%'+@acceptno+'%' ");
            }

            SqlParameter[] sqlpps = new SqlParameter[] 
                {
               new SqlParameter("@createman",createman)
                };
            //sqlshere.Append("and createman=@createman");
            sqldata.AppendFormat(" select * from {0} where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_PAYACCEPT);
            sqlcount.AppendFormat("select count(1) from {0} where 1=1" + sqlshere.ToString(), ConstantUtil.TABLE_PAYACCEPT);
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        #endregion

        #region 执行存储过程
        public void execProcdure(Hashtable ht)
        {
          DataTable dt=  DataFactory.SqlDataBase().GetDataTableProc("Econtract_proc",ht);
        }
        #endregion
    }
}