using RM.Busines;
using RM.Busines.contract;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.BLL;
using WZX.Busines.Util;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// reviewContractOperater 的摘要说明
    /// </summary>
    public class reviewContractOperater : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "application/json";
            string module = context.Request.QueryString["module"];
            string err = string.Empty;
            bool suc = false;
            switch (module)
            {
                // 更新出口进口审核表
                case "reviewData":
                    suc = reviewData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                // 更新出口进口废弃中止审核表
                case "reviewAbandonData":
                    suc = reviewAbandonData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                // 更新开会审核表
                case "reviewMeetData":
                    suc = reviewMeetData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                // 更新服务合同开会审核表
                case "reviewServiceMeetData":
                    suc = reviewServiceMeetData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                case "reviewLogisticsData":
                    suc = reviewLogisticsData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //更新服务合同审核表
                case "reviewServiceData":
                    suc = reviewServiceData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                    //更新管理合同审核表
                case "reviewManageData":
                    suc = reviewManageData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //更新内结合同审核表
                case "reviewInternalData":
                    suc = reviewInternalData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                //内结合同开会审核表
                case "reviewInternalMeetData":
                    suc = reviewInternalMeetData(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;

            }
        }

       

        #region 内结合同开会审核表
        private bool reviewInternalMeetData(ref string err, HttpContext context)
        {
              if (RequestSession.GetSessionUser().UserId == null)
            {
                err = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + "登录信息过期，请重新登录系统！" + "\"}";

                return false;
            }
            //获取审核记录、会议记录
            string reviewmeetlog = context.Request.Params["reviewmeetlog"] ?? string.Empty;
            string meetlog = context.Request.Params["meetlog"] ?? string.Empty;
            string meettime = context.Request.Params["meettime"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"] ?? string.Empty;
            //是否开会审核
            string isMeetReview = context.Request.Params["isMeetReview"] ?? string.Empty;
            //获取合同原本的状态
            string contractStatus = context.Request.Params["contractStatus"] ?? string.Empty;
            //获取上传文件路径
            var filepath = context.Request.Params["filepath"];
            string contractNo = context.Request.Params["contractNo"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string sbStatus = string.Empty;
            //判断合同状态进行更新
            if (isapprove == "undefined" || string.IsNullOrEmpty(isapprove))
            {
                err = "请选择审核状态";
                return false;
            }
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            if (isapprove == "不通过")//审核不通过删除合同审批记录
            {
                sbStatus = "退回";
                //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
            }
            else
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
            }
            //添加更新审核状态表
            Hashtable ht = new Hashtable();
            Hashtable ht_prime = new Hashtable();
            ht_prime.Add("contractNo", contractNo);
            ht_prime.Add("reviewstatus", sbStatus);
            ht["reviewstatus"] = sbStatus;
            ht["reviewlog"] = reviewmeetlog;
            ht["status"] = isapprove;
            ht["reviewman"] = user;
            ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
            ht["contractNo"] = contractNo;
            ht["meetlog"] = meetlog;
            ht["filepath"] = filepath;
            ht["meettime"] = meettime;
            ht["isMeetReview"] = isMeetReview;
            SqlUtil.getBatchSqls(ht,ht_prime, ConstantUtil.TABLE_REVIEWDATA,ref sqls, ref objs);
            //更新合同状态
            sqls.Add(new StringBuilder(string.Format(@"update {0} set status=@reviewstatus where contractNo=@contractNo",ConstantUtil.TABLE_ECONTRACT_INTERNAL)));
            objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        } 
        #endregion

        #region 更新内结审核表
        private bool reviewInternalData(ref string err, HttpContext context)
        {
               string log = context.Request.Params["log"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            string contractNo = context.Request.Params["contractNo"];
            string createman = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(string.Format(@"select createman from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_INTERNAL)), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0).Rows[0][0].ToString();
            //获取合同原本的状态
            string contractStatus = DataFactory.SqlDataBase().getString(new StringBuilder(string.Format(@"select status from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_INTERNAL)), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            //context.Request.Params["contractStatus"];
            //根据登录用户判断approal表中flag是否为1，为1则直接审批通过
            string flag = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract_approal where useraccount=@useraccount"), new SqlParam[1] { new SqlParam("@useraccount", user) }, "flag");
            if (isapprove == "undefined" || string.IsNullOrEmpty(isapprove))
            {
                err = "请选择审核状态";
                return false;
            }
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            if (flag == "1")
            {
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                Hashtable htTime = new Hashtable();
                htTime.Add("reviewtime", datetime);
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT_INTERNAL + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });

                #region 添加更新审核状态表,先删除后添加
                //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
                List<Hashtable> list_review = new List<Hashtable>();
                Hashtable htmanager = new Hashtable();
                htmanager["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK5;//财务主管审核
                htmanager["reviewlog"] = log;
                htmanager["status"] = isapprove;
                htmanager["reviewman"] = user;
                htmanager["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htmanager["contractNo"] = contractNo;
                Hashtable htPresident = new Hashtable();
                htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                htPresident["reviewlog"] = log;
                htPresident["status"] = isapprove;
                htPresident["reviewman"] = user;
                htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htPresident["contractNo"] = contractNo;
                Hashtable ht = new Hashtable();
                ht["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                ht["reviewlog"] = log;
                ht["status"] = isapprove;
                ht["reviewman"] = user;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                //list_review.Add(htmanager);
                list_review.Add(htPresident);
                list_review.Add(ht);
                SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
                #endregion

                #region 更新合同表状态
                string sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                sqls.Add(new StringBuilder(@"update" + ConstantUtil.TABLE_ECONTRACT_INTERNAL + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion
            }

            else
            {
                //获取要更新的状态,查询
                string sbStatus = getInternalStatusByContract(contractStatus);
                if (string.IsNullOrEmpty(sbStatus))
                {

                    err = "审核流程错误";
                    return false;
                }
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT_INTERNAL + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });
                #region 添加审核状态表
                List<Hashtable> list_ht = new List<Hashtable>();
                Hashtable ht = new Hashtable();
                ht["reviewstatus"] = sbStatus;
                ht["reviewlog"] = log;
                ht["status"] = isapprove;
                ht["reviewman"] = user;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                list_ht.Add(ht);
                SqlUtil.getBatchSqls(list_ht, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
                if (isapprove == "不通过")//审核不通过删除合同审批记录
                {
                    sbStatus = "退回";
                    //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                    //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
                }
                #endregion

                #region 更新合同表状态
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT_INTERNAL + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion
            }
            //批量执行sql
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        } 
        #endregion

        #region 更新管理合同审核表
        private bool reviewManageData(ref string err, HttpContext context)
        {
            string log = context.Request.Params["log"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            string contractNo = context.Request.Params["contractNo"];
            string createman = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(string.Format(@"select createman from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_LOGISTICS)), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0).Rows[0][0].ToString();
            //获取合同原本的状态
            string contractStatus = DataFactory.SqlDataBase().getString(new StringBuilder(string.Format(@"select status from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_LOGISTICS)), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            //context.Request.Params["contractStatus"];
            //根据登录用户判断approal表中flag是否为1，为1则直接审批通过
            string flag = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract_approal where useraccount=@useraccount"), new SqlParam[1] { new SqlParam("@useraccount", user) }, "flag");
            if (isapprove == "undefined" || string.IsNullOrEmpty(isapprove))
            {
                err = "请选择审核状态";
                return false;
            }
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            if (flag == "1")
            {
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                Hashtable htTime = new Hashtable();
                htTime.Add("reviewtime", datetime);
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });

                #region 添加更新审核状态表,先删除后添加
                //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
                List<Hashtable> list_review = new List<Hashtable>();
                Hashtable htmanager = new Hashtable();
                htmanager["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK5;//财务主管审核
                htmanager["reviewlog"] = log;
                htmanager["status"] = isapprove;
                htmanager["reviewman"] = user;
                htmanager["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htmanager["contractNo"] = contractNo;
                Hashtable htPresident = new Hashtable();
                htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                htPresident["reviewlog"] = log;
                htPresident["status"] = isapprove;
                htPresident["reviewman"] = user;
                htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htPresident["contractNo"] = contractNo;
                Hashtable ht = new Hashtable();
                ht["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                ht["reviewlog"] = log;
                ht["status"] = isapprove;
                ht["reviewman"] = user;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                //list_review.Add(htmanager);
                list_review.Add(htPresident);
                list_review.Add(ht);
                SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
                #endregion

                #region 更新合同表状态
                string sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                sqls.Add(new StringBuilder(@"update" + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion
            }

            else
            {
                //获取要更新的状态,查询
                string sbStatus = getStatusByContract(contractStatus);
                if (string.IsNullOrEmpty(sbStatus))
                {

                    err = "审核流程错误";
                    return false;
                }
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });
                #region 添加审核状态表
                List<Hashtable> list_ht = new List<Hashtable>();
                Hashtable ht = new Hashtable();
                ht["reviewstatus"] = sbStatus;
                ht["reviewlog"] = log;
                ht["status"] = isapprove;
                ht["reviewman"] = user;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                list_ht.Add(ht);
                SqlUtil.getBatchSqls(list_ht, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
                if (isapprove == "不通过")//审核不通过删除合同审批记录
                {
                    sbStatus = "退回";
                    //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                    //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
                }
                #endregion

                #region 更新合同表状态
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion
         
            }
            //批量执行sql
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        } 
        #endregion

        #region 更新服务合同审核表
        private bool reviewServiceData(ref string err, HttpContext context)
        {
            string log = context.Request.Params["log"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            string contractNo = context.Request.Params["contractNo"];
            string createman = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(string.Format(@"select createman from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_SERVICE)), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0).Rows[0][0].ToString();
            //获取合同原本的状态
            string contractStatus = DataFactory.SqlDataBase().getString(new StringBuilder(string.Format(@"select status from {0} where contractNo=@contractNo", ConstantUtil.TABLE_ECONTRACT_SERVICE)), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            //context.Request.Params["contractStatus"];
            //根据登录用户判断approal表中flag是否为1，为1则直接审批通过
            string flag = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract_approal where useraccount=@useraccount"), new SqlParam[1] { new SqlParam("@useraccount", user) }, "flag");
            if (isapprove == "undefined" || string.IsNullOrEmpty(isapprove))
            {
                err = "请选择审核状态";
                return false;
            }
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            if (flag == "1")
            {
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                Hashtable htTime = new Hashtable();
                htTime.Add("reviewtime", datetime);
                sqls.Add(new StringBuilder(@"update "+ConstantUtil.TABLE_ECONTRACT_SERVICE+@" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });

                #region 添加更新审核状态表,先删除后添加
                //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
                List<Hashtable> list_review = new List<Hashtable>();
                Hashtable htmanager = new Hashtable();
                htmanager["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK5;//财务主管审核
                htmanager["reviewlog"] = log;
                htmanager["status"] = isapprove;
                htmanager["reviewman"] = user;
                htmanager["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htmanager["contractNo"] = contractNo;
                Hashtable htPresident = new Hashtable();
                htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                htPresident["reviewlog"] = log;
                htPresident["status"] = isapprove;
                htPresident["reviewman"] = user;
                htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htPresident["contractNo"] = contractNo;
                Hashtable ht = new Hashtable();
                ht["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                ht["reviewlog"] = log;
                ht["status"] = isapprove;
                ht["reviewman"] = user;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                //list_review.Add(htmanager);
                list_review.Add(htPresident);
                list_review.Add(ht);
                SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs); 
                #endregion

                #region 更新合同表状态
                string sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                sqls.Add(new StringBuilder(@"update" + ConstantUtil.TABLE_ECONTRACT_SERVICE + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion

                #region old
                //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                //{
                //    try
                //    {
                //        bll.SqlTran = bll.SqlCon.BeginTransaction();
                //        //添加更新审核状态表
                //        Hashtable htmanager = new Hashtable();
                //        htmanager["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK5;//财务主管审核
                //        htmanager["reviewlog"] = log;
                //        htmanager["status"] = isapprove;
                //        htmanager["reviewman"] = user;
                //        htmanager["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        htmanager["contractNo"] = contractNo;
                //        Hashtable htPresident = new Hashtable();
                //        htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                //        htPresident["reviewlog"] = log;
                //        htPresident["status"] = isapprove;
                //        htPresident["reviewman"] = user;
                //        htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        htPresident["contractNo"] = contractNo;
                //        Hashtable ht = new Hashtable();
                //        ht["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                //        ht["reviewlog"] = log;
                //        ht["status"] = isapprove;
                //        ht["reviewman"] = user;
                //        ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        ht["contractNo"] = contractNo;
                //        if (isapprove == "不通过")//审核不通过删除合同审批记录
                //        {
                //            sbStatus = "退回";
                //        }
                //        //添加审核日志
                //        DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo);
                //        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, htmanager);
                //        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, htPresident);
                //        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, ht);
                //        string strSql = "update" + ConstantUtil.TABLE_ECONTRACT_SERVICE + " set status=@reviewstatus where contractNo=@contractNo";
                //        SqlParameter[] pms = new SqlParameter[]{
                //        new SqlParameter("@reviewstatus",sbStatus),
                //        new SqlParameter("@contractNo",contractNo),
                //    };

                //        bll.ExecuteNonQuery(strSql, pms);

                //        bll.SqlTran.Commit();

                //        return true;
                //    }
                //    catch (Exception ex)
                //    {
                //        bll.SqlTran.Rollback();
                //        err = ex.Message;
                //        return false;
                //    }

                //} 
                #endregion
            }

            else
            {
                //获取要更新的状态,查询
                string sbStatus = getStatusByContract(contractStatus);

                if (string.IsNullOrEmpty(sbStatus))
                {
                    err = "审核流程错误";
                    return false;
                }
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT_SERVICE + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });
             
                #region 添加审核状态表
                List<Hashtable> list_ht = new List<Hashtable>();
                Hashtable ht = new Hashtable();
                ht["reviewstatus"] = sbStatus;
                ht["reviewlog"] = log;
                ht["status"] = isapprove;
                ht["reviewman"] = user;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                list_ht.Add(ht);
                SqlUtil.getBatchSqls(list_ht, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
                if (isapprove == "不通过")//审核不通过删除合同审批记录
                {
                    sbStatus = "退回";
                    //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                    //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
                } 
                #endregion

                #region 更新合同表状态
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT_SERVICE + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion

                #region old
                //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                //{
                //    try
                //    {
                //        bll.SqlTran = bll.SqlCon.BeginTransaction();
                //        //添加更新审核状态表
                //        Hashtable ht = new Hashtable();
                //        ht["reviewstatus"] = sbStatus;
                //        ht["reviewlog"] = log;
                //        ht["status"] = isapprove;
                //        ht["reviewman"] = user;
                //        ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        ht["contractNo"] = contractNo;
                //        if (isapprove == "不通过")//审核不通过删除合同审批记录
                //        {
                //            sbStatus = "退回";
                //            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo);
                //        }
                //        bool b = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo, "reviewstatus", sbStatus, ht);
                //        string strSql = "update " + ConstantUtil.TABLE_ECONTRACT_SERVICE + " set status=@reviewstatus where contractNo=@contractNo";
                //        SqlParameter[] pms = new SqlParameter[]{
                //        new SqlParameter("@reviewstatus",sbStatus),
                //        new SqlParameter("@contractNo",contractNo),
                //    };

                //        bll.ExecuteNonQuery(strSql, pms);

                //        bll.SqlTran.Commit();

                //        return true;
                //    }
                //    catch (Exception ex)
                //    {
                //        bll.SqlTran.Rollback();
                //        err = ex.Message;
                //        return false;
                //    }

                //} 
                #endregion
            }

            //批量执行sql
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        } 
        #endregion

        #region 更新出口进口审核表
      
        private bool reviewData(ref string err, HttpContext context)
        {
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            string log = context.Request.Params["log"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string rolesName = string.Empty;
            string contractNo = context.Request.Params["contractNo"];
            string buyer = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(@"select buyer from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0).Rows[0][0].ToString();
            string seller = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(@"select seller from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0).Rows[0][0].ToString();
            string createman = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(@"select createman from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0).Rows[0][0].ToString();
            //获取合同原本的状态
            string contractStatus = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            //context.Request.Params["contractStatus"];
            //根据登录用户判断approal表中flag是否为1，为1则直接审批通过
            string flag = DataFactory.SqlDataBase().getString(new StringBuilder(@"select t1.flag from Econtract_approal t1,Econtract t2 where  t1.useraccount=@useraccount and t1.flowdirection=t2.flowdirection and t1.approvalnode=t2.status and (t1.companyname=t2.buyer or t1.companyname=t2.seller) and  t2.contractNo like t1.prioritycode+'%'"), new SqlParam[1] { new SqlParam("@useraccount", user) }, "flag");

            #region flag为1直接审批通过
            if (flag == "1")
            {
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });

                #region 添加更新审核状态表,先删除后添加
                //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
                List<Hashtable> list_review = new List<Hashtable>();
                Hashtable htmanager = new Hashtable();
                htmanager["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK5;//财务主管审核
                htmanager["reviewlog"] = log;
                htmanager["status"] = isapprove;
                htmanager["reviewman"] = user;
                htmanager["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htmanager["contractNo"] = contractNo;
                Hashtable htPresident = new Hashtable();
                htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                htPresident["reviewlog"] = log;
                htPresident["status"] = isapprove;
                htPresident["reviewman"] = user;
                htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htPresident["contractNo"] = contractNo;
                Hashtable ht = new Hashtable();
                ht["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                ht["reviewlog"] = log;
                ht["status"] = isapprove;
                ht["reviewman"] = user;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                //list_review.Add(htmanager);
                list_review.Add(htPresident);
                list_review.Add(ht);
                SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
                #endregion

                #region 更新合同表状态
                string sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion

                #region old
                //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                //{
                //    try
                //    {
                //        bll.SqlTran = bll.SqlCon.BeginTransaction();
                //        //添加更新审核状态表
                //        Hashtable htmanager = new Hashtable();
                //        htmanager["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK5;//财务主管审核
                //        htmanager["reviewlog"] = log;
                //        htmanager["status"] = isapprove;
                //        htmanager["reviewman"] = user;
                //        htmanager["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        htmanager["contractNo"] = contractNo;
                //        Hashtable htPresident = new Hashtable();
                //        htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                //        htPresident["reviewlog"] = log;
                //        htPresident["status"] = isapprove;
                //        htPresident["reviewman"] = user;
                //        htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        htPresident["contractNo"] = contractNo;
                //        Hashtable ht = new Hashtable();
                //        ht["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                //        ht["reviewlog"] = log;
                //        ht["status"] = isapprove;
                //        ht["reviewman"] = user;
                //        ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        ht["contractNo"] = contractNo;
                //        if (isapprove == "不通过")//审核不通过删除合同审批记录
                //        {
                //            sbStatus = "退回";
                //        }
                //        //添加审核日志
                //        DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_REVIEWDATA, contractNo, contractNo);
                //        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, htmanager);
                //        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, htPresident);
                //        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, ht);
                //        string strSql = "update Econtract set status=@reviewstatus where contractNo=@contractNo";
                //        SqlParameter[] pms = new SqlParameter[]{
                //        new SqlParameter("@reviewstatus",sbStatus),
                //        new SqlParameter("@contractNo",contractNo),
                //    };

                //        bll.ExecuteNonQuery(strSql, pms);

                //        bll.SqlTran.Commit();

                //        return true;
                //    }
                //    catch (Exception ex)
                //    {
                //        bll.SqlTran.Rollback();
                //        err = ex.Message;
                //        return false;
                //    }

                //} 
                #endregion
            } 
            #endregion

            #region flag为2进出口财务主管审批后直接审批通过
            else if (flag == "2")
            {
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });
                string sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                #region 添加更新审核状态表
                List<Hashtable> list_review = new List<Hashtable>();
                Hashtable htPresident = new Hashtable();
                htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                htPresident["reviewlog"] = log;
                htPresident["status"] = isapprove;
                htPresident["reviewman"] = user;
                htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                htPresident["contractNo"] = contractNo;
                list_review.Add(htPresident);
                SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
                #endregion
                if (isapprove == "不通过")//审核不通过删除合同审批记录
                {
                    sbStatus = "退回";
                }
                #region 更新合同表状态
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion

                #region old
                //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                //{
                //    try
                //    {
                //        bll.SqlTran = bll.SqlCon.BeginTransaction();
                //        //添加更新审核状态表
                //        Hashtable htPresident = new Hashtable();
                //        htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                //        htPresident["reviewlog"] = log;
                //        htPresident["status"] = isapprove;
                //        htPresident["reviewman"] = user;
                //        htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        htPresident["contractNo"] = contractNo;
                //        //Hashtable ht = new Hashtable();
                //        //ht["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                //        //ht["reviewlog"] = log;
                //        //ht["status"] = isapprove;
                //        //ht["reviewman"] = user;
                //        //ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                //        //ht["contractNo"] = contractNo;
                //        if (isapprove == "不通过")//审核不通过删除合同审批记录
                //        {
                //            sbStatus = "退回";
                //        }
                //        //添加审核日志
                //        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, htPresident);
                //        //DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, ht);
                //        string strSql = "update Econtract set status=@reviewstatus where contractNo=@contractNo";
                //        SqlParameter[] pms = new SqlParameter[]{
                //        new SqlParameter("@reviewstatus",sbStatus),
                //        new SqlParameter("@contractNo",contractNo),
                //    };

                //        bll.ExecuteNonQuery(strSql, pms);

                //        bll.SqlTran.Commit();

                //        return true;
                //    }
                //    catch (Exception ex)
                //    {
                //        bll.SqlTran.Rollback();
                //        err = ex.Message;
                //        return false;
                //    }

                //} 
                #endregion
            } 
            #endregion

            #region 正常审核
            else
            {
                string sbStatus = string.Empty;
                //获取要更新的状态,查询
                sbStatus = getStatusByContract(contractStatus);
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });
                #region 审批通过时更新价格有效期列表显示时间
                if (sbStatus == ConstantUtil.STATUS_STOCKIN_CHECK1)//审批通过
                {
                    //更新价格有效期列表显示时间
                    string showpvalidity = getShowPvalidity(contractNo, datetime);
                    Hashtable ht = new Hashtable();
                    ht["showpvalidity"] = showpvalidity;
                    SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT, "contractNo", contractNo, ref sqls, ref objs);
                    //DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", contractNo, ht);
                } 
                #endregion
                if (string.IsNullOrEmpty(sbStatus))
                {
                    err = "审核流程错误";
                    return false;
                }
                if (isapprove == "不通过")//审核不通过删除合同审批记录
                {
                    sbStatus = "退回";
                    //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                    //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
                    //DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_REVIEWDATA, contractNo, contractNo);
                }
                //添加更新审核状态表
                List<Hashtable>list_review=new List<Hashtable>();
                Hashtable htreview = new Hashtable();
                htreview["reviewstatus"] = sbStatus;
                htreview["reviewlog"] = log;
                htreview["status"] = isapprove;
                htreview["reviewman"] = user;
                htreview["reviewdate"] = datetime;
                htreview["contractNo"] = contractNo;
                list_review.Add(htreview);
                SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);

                #region 更新合同表状态
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT + " set status=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion

                #region old
                //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                //{
                //    try
                //    {
                //        bll.SqlTran = bll.SqlCon.BeginTransaction();
                //        //添加更新审核状态表
                //        Hashtable ht = new Hashtable();
                //        ht["reviewstatus"] = sbStatus;
                //        ht["reviewlog"] = log;
                //        ht["status"] = isapprove;
                //        ht["reviewman"] = user;
                //        ht["reviewdate"] = datetime;
                //        ht["contractNo"] = contractNo;
                //        if (isapprove == "不通过")//审核不通过删除合同审批记录
                //        {
                //            sbStatus = "退回";
                //            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_REVIEWDATA, contractNo, contractNo);
                //        }
                //        bool b = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo, "reviewstatus", sbStatus, ht);
                //        string strSql = "update Econtract set status=@reviewstatus where contractNo=@contractNo";
                //        SqlParameter[] pms = new SqlParameter[]{
                //        new SqlParameter("@reviewstatus",sbStatus),
                //        new SqlParameter("@contractNo",contractNo),
                //    };

                //        bll.ExecuteNonQuery(strSql, pms);

                //        bll.SqlTran.Commit();

                //        return true;
                //    }
                //    catch (Exception ex)
                //    {
                //        bll.SqlTran.Rollback();
                //        err = ex.Message;
                //        return false;
                //    }

                //} 
                #endregion
            } 
            #endregion

            //批处理sql
           int r= DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
           return r > 0 ? true : false;


        } 
        #endregion

        #region 更新进出口废弃合同审核表
        private bool reviewAbandonData(ref string err, HttpContext context)
        {
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            string log = context.Request.Params["log"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string rolesName = string.Empty;
            string contractNo = context.Request.Params["contractNo"];
            string createman = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(@"select createman from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0).Rows[0][0].ToString();
            //获取合同原本的状态
            string contractStatus = DataFactory.SqlDataBase().getString(new StringBuilder(@"select abandonStatus from Econtract where contractNo=@contractNo"), new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "abandonStatus");

            #region 正常审核
        
                string sbStatus = string.Empty;
                //获取要更新的状态,查询
                sbStatus = getAbandonStatusByContract(contractStatus);
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT + @" set reviewtime=@reviewtime where 
                contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@reviewtime", datetime), new SqlParam("@contractNo", contractNo) });

                #region 审批通过时更新合同表状态列status为废弃或中止审批通过
                if (sbStatus == ConstantUtil.STATUS_ABANDONPASS || sbStatus == ConstantUtil.STATUS_DISCONTINUEPASS)//废弃审批通过，中止审批通过
                {
                    sqls.Add(new StringBuilder(@"update Econtract set status=@sbStatus where contractNo=@contractNo"));
                    objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@sbStatus", sbStatus) });
                }
                #endregion

                if (string.IsNullOrEmpty(sbStatus))
                {
                    err = "审核流程错误";
                    return false;
                }
                if (isapprove == "不通过")//审核不通过删除合同审批记录
                {
                    sbStatus = "退回";
                
                }
                //添加更新审核状态表
                List<Hashtable> list_review = new List<Hashtable>();
                Hashtable htreview = new Hashtable();
                htreview["reviewstatus"] = sbStatus;
                htreview["reviewlog"] = log;
                htreview["status"] = isapprove;
                htreview["reviewman"] = user;
                htreview["reviewdate"] = datetime;
                htreview["contractNo"] = contractNo;
                list_review.Add(htreview);
                SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);

                #region 更新合同表状态
                sqls.Add(new StringBuilder(@"update " + ConstantUtil.TABLE_ECONTRACT + " set abandonStatus=@reviewstatus where contractNo=@contractNo"));
                objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
                #endregion

            #endregion

            //批处理sql
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;

        }
        #endregion

        #region 获取价格有效期列表显示时间

        private string getShowPvalidity(string contractNo, string datetime)
        {
            StringBuilder sb = new StringBuilder(@"select pvalidity from Econtract where contractNo=@contractNo");
            DateTime dt = Convert.ToDateTime(datetime);
            int i = 0;
            string pvalidity = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "pvalidity");
            int.TryParse(pvalidity, out i);
            string ss = dt.AddDays(i).ToString();
            return ss;
        } 
        #endregion

        #region 获取审核节点下一节点状态
        public string getStatusByContract(string contractStatus)
        {
            string sbStatus = string.Empty;
            //判断合同状态进行更新
            if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK)//提交
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK2;//业务直线审核
            }
            else if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK2)
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//合同管理员审核
            }
            else if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK4;//业务处总监审核
            }
            else if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK4)
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK5;//财务人员审核
            }
            else if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK5)
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK6;//财务总监审核
            }
            else if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK6)
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK1;//销售总监审核
            }
            return sbStatus;
        } 
        #endregion

        #region 获取废弃中止合同审核节点下一节点状态
        public string getAbandonStatusByContract(string contractStatus)
        {
            string sbStatus = string.Empty;
            switch (contractStatus)
            {
                case ConstantUtil.STATUS_CHECK1://废弃_待直线经理审核
                    sbStatus =ConstantUtil.STATUS_CHECK2;
                    break;
                case ConstantUtil.STATUS_CHECK2://废弃_待合同管理员审核
                    sbStatus = ConstantUtil.STATUS_CHECK3;
                    break;
                case ConstantUtil.STATUS_CHECK3://废弃_待业务处主管审核
                    sbStatus = ConstantUtil.STATUS_CHECK4;
                    break;
                case ConstantUtil.STATUS_CHECK4://废弃_待财务负责人审核
                    sbStatus = ConstantUtil.STATUS_CHECK5;
                    break;
                case ConstantUtil.STATUS_CHECK5://废弃_待财务主管审核
                    sbStatus = ConstantUtil.STATUS_CHECK6;
                    break;
                case ConstantUtil.STATUS_CHECK6://废弃_待董事长审核
                    sbStatus = ConstantUtil.STATUS_ABANDONPASS;
                    break;
                case ConstantUtil.STATUS_CHECK7://中止_待直线经理审核
                    sbStatus = ConstantUtil.STATUS_CHECK8;
                    break;
                case ConstantUtil.STATUS_CHECK8://中止_待合同管理员审核
                    sbStatus = ConstantUtil.STATUS_CHECK9;
                    break;
                case ConstantUtil.STATUS_CHECK9://中止_待业务处主管审核
                    sbStatus = ConstantUtil.STATUS_CHECK10;
                    break;
                case ConstantUtil.STATUS_CHECK10://中止_待财务负责人审核
                    sbStatus = ConstantUtil.STATUS_CHECK11;
                    break;
                case ConstantUtil.STATUS_CHECK11://中止_待财务主管审核
                    sbStatus = ConstantUtil.STATUS_CHECK12;
                    break;
                case ConstantUtil.STATUS_CHECK12://中止_待董事长审核
                    sbStatus = ConstantUtil.STATUS_DISCONTINUEPASS;
                    break;
             
                default:
                    break;
            }
         
            return sbStatus;
        }
        #endregion

        #region 获取内结审核节点下一节点状态
        public string getInternalStatusByContract(string contractStatus)
        {
            string sbStatus = string.Empty;
            //判断合同状态进行更新
            if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK)//提交
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK2;//业务直线审核
            }
            else if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK2)
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//合同管理员审核
            }
            else if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK4;//业务处总监审核
            }
            else if (contractStatus == ConstantUtil.STATUS_STOCKIN_CHECK4)
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
            }
          
            return sbStatus;
        }
        #endregion

        #region 开会审核
        private bool reviewMeetData(ref string err, HttpContext context)
        {
            if (RequestSession.GetSessionUser().UserId == null)
            {
                err = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + "登录信息过期，请重新登录系统！" + "\"}";

                return false;
            }
            //获取审核记录、会议记录
            string reviewmeetlog = context.Request.Params["reviewmeetlog"] ?? string.Empty;
            string meetlog = context.Request.Params["meetlog"] ?? string.Empty;
            string meettime = context.Request.Params["meettime"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"] ?? string.Empty;
            //是否开会审核
            string isMeetReview = context.Request.Params["isMeetReview"] ?? string.Empty;
            //获取合同原本的状态
            string contractStatus = context.Request.Params["contractStatus"] ?? string.Empty;
            //获取上传文件路径
            var filepath = context.Request.Params["filepath"];
            string contractNo = context.Request.Params["contractNo"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string sbStatus = string.Empty;
            //判断合同状态进行更新
            if (isapprove == "undefined" || string.IsNullOrEmpty(isapprove))
            {
                err = "请选择审核状态";
                return false;
            }
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            if (isapprove == "不通过")//审核不通过删除合同审批记录
            {
                sbStatus = "退回";
                sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
            }
            else
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
            }
            //添加更新审核状态表
            Hashtable ht = new Hashtable();
            Hashtable ht_prime = new Hashtable();
            ht_prime.Add("contractNo", contractNo);
            ht_prime.Add("reviewstatus", sbStatus);
            ht["reviewstatus"] = sbStatus;
            ht["reviewlog"] = reviewmeetlog;
            ht["status"] = isapprove;
            ht["reviewman"] = user;
            ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
            ht["contractNo"] = contractNo;
            ht["meetlog"] = meetlog;
            ht["filepath"] = filepath;
            ht["meettime"] = meettime;
            ht["isMeetReview"] = isMeetReview;
            SqlUtil.getBatchSqls(ht,ht_prime, ConstantUtil.TABLE_REVIEWDATA,ref sqls, ref objs);
            //更新合同状态
            sqls.Add(new StringBuilder(@"update Econtract set status=@reviewstatus where contractNo=@contractNo"));
            objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
           int r= DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
           return r > 0 ? true : false;
            #region old
            //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            //{
            //    try
            //    {
            //        bll.SqlTran = bll.SqlCon.BeginTransaction();
            //        if (isapprove == "不通过")//审核不通过删除合同审批记录
            //        {
            //            sbStatus = "退回";
            //            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_REVIEWDATA, contractNo, contractNo);
            //        }
            //        else
            //        {
            //            sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
            //        }
            //        //添加更新审核状态表
            //        Hashtable ht = new Hashtable();
            //        ht["reviewstatus"] = sbStatus;
            //        ht["reviewlog"] = reviewmeetlog;
            //        ht["status"] = isapprove;
            //        ht["reviewman"] = user;
            //        ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
            //        ht["contractNo"] = contractNo;
            //        ht["meetlog"] = meetlog;
            //        ht["filepath"] = filepath;
            //        ht["meettime"] = meettime;
            //        ht["isMeetReview"] = isMeetReview;
            //        bool b = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo, "reviewstatus", sbStatus, ht);
            //        string strSql = "update Econtract set status=@reviewstatus where contractNo=@contractNo";
            //        SqlParameter[] pms = new SqlParameter[]{
            //            new SqlParameter("@reviewstatus",sbStatus),
            //            new SqlParameter("@contractNo",contractNo),
            //        };
            //        bll.ExecuteNonQuery(strSql, pms);

            //        bll.SqlTran.Commit();

            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        err = ex.Message;
            //        return false;
            //    }

            //} 
            #endregion




        } 
        #endregion

        #region 服务合同开会审核
        private bool reviewServiceMeetData(ref string err, HttpContext context)
        {
            if (RequestSession.GetSessionUser().UserId == null)
            {
                err = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + "登录信息过期，请重新登录系统！" + "\"}";

                return false;
            }
            //获取审核记录、会议记录
            string reviewmeetlog = context.Request.Params["reviewmeetlog"] ?? string.Empty;
            string meetlog = context.Request.Params["meetlog"] ?? string.Empty;
            string meettime = context.Request.Params["meettime"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"] ?? string.Empty;
            //是否开会审核
            string isMeetReview = context.Request.Params["isMeetReview"] ?? string.Empty;
            //获取合同原本的状态
            string contractStatus = context.Request.Params["contractStatus"] ?? string.Empty;
            //获取上传文件路径
            var filepath = context.Request.Params["filepath"];
            string contractNo = context.Request.Params["contractNo"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string sbStatus = string.Empty;
            //判断合同状态进行更新
            if (isapprove == "undefined" || string.IsNullOrEmpty(isapprove))
            {
                err = "请选择审核状态";
                return false;
            }
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            if (isapprove == "不通过")//审核不通过删除合同审批记录
            {
                sbStatus = "退回";
                //sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_REVIEWDATA + " where contractNo=@contractNo"));
                //objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
            }
            else
            {
                sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK3;//待业务处主管审核
            }
            //添加更新审核状态表
            Hashtable ht = new Hashtable();
            Hashtable ht_prime = new Hashtable();
            ht_prime.Add("contractNo", contractNo);
            ht_prime.Add("reviewstatus", sbStatus);
            ht["reviewstatus"] = sbStatus;
            ht["reviewlog"] = reviewmeetlog;
            ht["status"] = isapprove;
            ht["reviewman"] = user;
            ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
            ht["contractNo"] = contractNo;
            ht["meetlog"] = meetlog;
            ht["filepath"] = filepath;
            ht["meettime"] = meettime;
            ht["isMeetReview"] = isMeetReview;
            SqlUtil.getBatchSqls(ht,ht_prime, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
            //更新合同状态
            sqls.Add(new StringBuilder(string.Format(@"update {0} set status=@reviewstatus where contractNo=@contractNo",ConstantUtil.TABLE_ECONTRACT_SERVICE)));
            objs.Add(new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@reviewstatus", sbStatus) });
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;

        }
        #endregion

        #region 更新物流审核表
    
        private bool reviewLogisticsData(ref string err, HttpContext context)
        {
            string log = context.Request.Params["log"] ?? string.Empty;
            //判断是否通过
            string isapprove = context.Request.Params["isapprove"];
            string user = RequestSession.GetSessionUser().UserAccount.ToString();
            string rolesName = string.Empty;
            string companyName = string.Empty;
            string logisticsContractNo = context.Request.Params["contractNo"];
            string createman = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(@"select createman from Econtract_logistics where logisticsContractNo=@logisticsContractNo"), new SqlParam[1] { new SqlParam("@logisticsContractNo", logisticsContractNo) }, 0).Rows[0][0].ToString();
            //获取合同原本的状态
            string contractStatus = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract_logistics where logisticsContractNo=@logisticsContractNo"), new SqlParam[1] { new SqlParam("@logisticsContractNo", logisticsContractNo) }, "status");
            //context.Request.Params["contractStatus"];
            //根据登录用户判断approal表中flag是否为1，为1则直接审批通过
            string flag = DataFactory.SqlDataBase().getString(new StringBuilder(@"select status from Econtract_approal where useraccount=@useraccount"), new SqlParam[1] { new SqlParam("@useraccount", user) }, "flag");
            if (flag == "1")
            {
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                Hashtable htTime = new Hashtable();
                htTime.Add("reviewtime", datetime);
                DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT_LOGISTICS, "contractNo", logisticsContractNo, htTime);
                string sbStatus = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                if (isapprove == "undefined" || string.IsNullOrEmpty(isapprove))
                {
                    err = "请选择审核状态";
                    return false;
                }
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    try
                    {
                        bll.SqlTran = bll.SqlCon.BeginTransaction();
                        //添加更新审核状态表
                        Hashtable htmanager = new Hashtable();
                        htmanager["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK5;//财务主管审核
                        htmanager["reviewlog"] = log;
                        htmanager["status"] = isapprove;
                        htmanager["reviewman"] = user;
                        htmanager["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                        htmanager["contractNo"] = logisticsContractNo;
                        Hashtable htPresident = new Hashtable();
                        htPresident["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK6;//董事长审核
                        htPresident["reviewlog"] = log;
                        htPresident["status"] = isapprove;
                        htPresident["reviewman"] = user;
                        htPresident["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                        htPresident["contractNo"] = logisticsContractNo;
                        Hashtable ht = new Hashtable();
                        ht["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK1;//审批通过
                        ht["reviewlog"] = log;
                        ht["status"] = isapprove;
                        ht["reviewman"] = user;
                        ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                        ht["contractNo"] = logisticsContractNo;
                        if (isapprove == "不通过")//审核不通过删除合同审批记录
                        {
                            sbStatus = "退回";
                        }
                        //添加审核日志
                        DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_REVIEWDATA, "contractNo", logisticsContractNo);
                        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, htmanager);
                        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, htPresident);
                        DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_REVIEWDATA, ht);
                        string strSql = "update Econtract_logistics set status=@reviewstatus where logisticsContractNo=@contractNo";
                        SqlParameter[] pms = new SqlParameter[]{
                        new SqlParameter("@reviewstatus",sbStatus),
                        new SqlParameter("@contractNo",logisticsContractNo),
                    };

                        bll.ExecuteNonQuery(strSql, pms);

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

            else
            {
                //获取要更新的状态,查询
                string sbStatus = getStatusByContract(contractStatus);
                //更新合同审批时间,每个节点的时间
                var datetime = DateTimeHelper.ShortDateTimeS;
                Hashtable htTime = new Hashtable();
                htTime.Add("reviewtime", datetime);
                DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT_LOGISTICS, "logisticsContractNo", logisticsContractNo, htTime);
                if (isapprove == "undefined" || string.IsNullOrEmpty(isapprove))
                {
                    err = "请选择审核状态";
                    return false;
                }
                if (string.IsNullOrEmpty(sbStatus))
                {
                    err = "审核流程错误";
                    return false;
                }
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    try
                    {
                        bll.SqlTran = bll.SqlCon.BeginTransaction();
                        //添加更新审核状态表
                        Hashtable ht = new Hashtable();
                        ht["reviewstatus"] = sbStatus;
                        ht["reviewlog"] = log;
                        ht["status"] = isapprove;
                        ht["reviewman"] = user;
                        ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                        ht["contractNo"] = logisticsContractNo;
                        if (isapprove == "不通过")//审核不通过删除合同审批记录
                        {
                            sbStatus = "退回";
                            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_REVIEWDATA, "contractNo", logisticsContractNo);
                        }
                        bool b = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_REVIEWDATA, "contractNo", logisticsContractNo, "reviewstatus", sbStatus, ht);
                        string strSql = "update Econtract_logistics set status=@reviewstatus where logisticsContractNo=@contractNo";
                        SqlParameter[] pms = new SqlParameter[]{
                        new SqlParameter("@reviewstatus",sbStatus),
                        new SqlParameter("@contractNo",logisticsContractNo),
                    };

                        bll.ExecuteNonQuery(strSql, pms);

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
        } 
        #endregion

        #region 返回json
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
    }
}