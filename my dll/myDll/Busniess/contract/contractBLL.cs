using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using WZX.Busines.Util;
using RM.Common.DotNetBean;
using System.Web.SessionState;
using System.Data.SqlClient;
using RM.Common.DotNetCode;
using System.Text.RegularExpressions;
using RM.Common.DotNetJson;
using System.Web;
namespace RM.Busines.contract
{
    public class contractBLL : IRequiresSessionState
    {
        JsonHelperEasyUi ui = new JsonHelperEasyUi();



        #region 获取合同列表
        /// <summary>
        /// 获取合同列表,根据review的值判断是所有还是审核列表
        /// </summary>
        /// <param name="contractNo">合同号</param>
        /// <param name="signedtime_begin">签订时间</param>
        /// <param name="signedtime_end">终止时间</param>
        /// <param name="row">条数</param>
        /// <param name="page">页码</param>
        /// <param name="order">排序</param>
        /// <param name="sort">排序依据</param>
        /// <param name="review">是否审核</param>
        /// <returns></returns>
        public StringBuilder GetContractList(string contractNo, string signedtime_begin, string signedtime_end, int row, int page, string order, string sort, string review, string flowdirection, string businessclass, string isConManage)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
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
            if (businessclass != null && businessclass.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.businessclass=@businessclass ");
            }
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }

            if (createman != null && createman != "admin" && isConManage != "True")
            {
                sqlshere.Append(" and t1.createman=@createman");
            }
            if (isConManage == "True")
            {
                sqlshere.Append(" and (t1.adminReviewNumber=@createman or t1.createman=@createman)");
            }
            sqldata.Append(@" select t1.*,t1.status as status1,t2.quantity  from Econtract t1 left join (select sum(quantity) as quantity,contractNo from Econtract_ap group by(contractNo))t2
on t1.contractNo=t2.contractNo where " + sqlshere.ToString());
            sqlcount.Append("select count(1) from Econtract t1 where " + sqlshere.ToString());
            //sqldata.Append(@" select *,t1.status as status1 from Econtract t1 where " + sqlshere.ToString());
            //sqlcount.Append("select count(1) from Econtract t1 where " + sqlshere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                     new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                    new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                   new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String},
                   new SqlParameter{ParameterName="@businessclass",Value=businessclass,DbType=DbType.String}
                };

            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb;
        }
        //获取合同中发货申请已完成，卖方为香港公司，运输方式为铁路的合同
        public StringBuilder GetContractListByHK(string contractNo, string signedtime_begin, string signedtime_end, int row, int page, string order, string sort, string review, string flowdirection, string transport, string createman)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();

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
            if (createman != null && createman != "管理员")
            {
                sqlshere.Append(" and t2.adminReview=@createman");
            }
            if (transport == "铁路")
            {
                sqldata.AppendFormat(@"select t2.*,t1.contactStatus ,t1.createDateTag,t1.pcode,t1.pname,t1.spec,t1.ifcheck,
t1.quantity,t1.sendQuantity,t1.qunit,t1.pallet,t1.unit,t1.packdes,t1.createmanname as applyman,t1.createdate as applydate from {0} t1,{1} 
t2 where t1.contractNo=t2.contractNo and t2.transport=@transport"
            + sqlshere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT);
                sqlcount.AppendFormat(@"select count(1) from {0} t1,{1} 
t2 where t1.contractNo=t2.contractNo and t2.transport=@transport " + sqlshere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT);
            }
            else if (transport == "海运")
            {
                transport = "铁路";
                sqldata.AppendFormat(@"select t2.*,t1.contactStatus ,t1.createDateTag,t1.pcode,t1.pname,t1.spec,t1.ifcheck,
t1.quantity,t1.sendQuantity,t1.qunit,t1.pallet,t1.unit,t1.packdes,t1.createmanname as applyman,t1.createdate as applydate from {0} t1,{1} 
t2 where t1.contractNo=t2.contractNo and t2.transport!=@transport"
                          + sqlshere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT);
                sqlcount.AppendFormat(@"select count(1) from {0} t1,{1} 
t2 where t1.contractNo=t2.contractNo and t2.transport!=@transport " + sqlshere.ToString(), ConstantUtil.TABLE_SENDOUTAPPDETAILS, ConstantUtil.TABLE_ECONTRACT);
            }


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
            return sb;
        }

        public StringBuilder GetConEditByConManager(string contractNo, string signedtime_begin, string signedtime_end, int row, int page, string order, string sort, string review, string flowdirection, string businessclass)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
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
            if (businessclass != null && businessclass.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.businessclass=@businessclass ");
            }
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            if (createman != null && createman != "admin")
            {
                sqlshere.Append(" and (t1.createman=@createman or t1.adminReviewNumber=@createman) ");
            }
            sqldata.Append(@" select t1.*,t1.status as status1,t2.quantity  from Econtract t1 left join (select sum(quantity) as quantity,contractNo from Econtract_ap group by(contractNo))t2
on t1.contractNo=t2.contractNo where " + sqlshere.ToString());
            sqlcount.Append("select count(1) from Econtract t1 where " + sqlshere.ToString());
            //sqldata.Append(@" select *,t1.status as status1 from Econtract t1 where " + sqlshere.ToString());
            //sqlcount.Append("select count(1) from Econtract t1 where " + sqlshere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                     new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                    new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                   new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String},
                   new SqlParameter{ParameterName="@businessclass",Value=businessclass,DbType=DbType.String}
                };

            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb;
        }


        #endregion

        #region 获取废弃合同审核列表


        public string GetAbandonReviewContractList(int row, int page, string order, string sort, string flowdirection, string isDesk, string signedtime_begin, string signedtime_end)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate<=@signedtime_end ");
            }
            #region 登录人为admin时，看到所有的废弃或中止的合同
            string loginUser = RequestSession.GetSessionUser().UserAccount.ToString();
            if (loginUser.Equals("admin"))
            {
                sqldata.AppendFormat(@"select t1.*,t1.status as status1 from {0} t1 where (t1.typeStatus='{0}' or t1.typeStatus='{1}') " + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT, ConstantUtil.STATUS_DISCONTINUE, ConstantUtil.STATUS_ABANDON);
                sqlcount.AppendFormat(@"select count(*) from {0} t1 where (t1.typeStatus='{0}' or t1.typeStatus='{1}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT, ConstantUtil.STATUS_DISCONTINUE, ConstantUtil.STATUS_ABANDON);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@flowdirection",flowdirection),
                 new SqlParameter("@signedtime_begin",signedtime_begin),
                 new SqlParameter("@signedtime_end",signedtime_end)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            //获取用户所属角色和所属公司名称
            string rolesName = string.Empty;
            string companyName = string.Empty;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userAccountName = RequestSession.GetSessionUser().UserName.ToString();
            string userAccountNumber = RequestSession.GetSessionUser().UserId.ToString();

            #region 业务直线审核
            //如果为业务直线人员审核，即abandonStatus为废弃_待业务直线审核或中止_待业务直线审核，则加载合同中salesReviewNumber为登陆人账号的合同审核
            string salesReviewName = getSalesReviewMan(userAccountName);
            if (salesReviewName.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT + " t1  where t1.salesReviewNumber=@salesReviewNumber and (t1.abandonStatus='{0}' or t1.abandonStatus='{1}')" + sqlshere.ToString(), ConstantUtil.STATUS_CHECK1, ConstantUtil.STATUS_CHECK7);
                sqlcount.AppendFormat(@" select count(1) from " + ConstantUtil.TABLE_ECONTRACT + " t1  where t1.salesReviewNumber=@salesReviewNumber and (t1.abandonStatus='{0}' or t1.abandonStatus='{1}')" + sqlshere.ToString(), ConstantUtil.STATUS_CHECK1, ConstantUtil.STATUS_CHECK7);
                SqlParameter[] sqlpps = new SqlParameter[] {
                  new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection),
                   new SqlParameter("@salesReviewNumber",salesReviewName),
                 //new SqlParameter("@signedtime_begin",signedtime_begin),
                 //new SqlParameter("@signedtime_end",signedtime_end)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 合同管理员审核
            //获取合同审核人
            string name = getReviewMan(userAccountName);
            //如果为合同管理员审核,则加载合同中adminReviewNumber为登陆人账号的合同审核
            if (name.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT + " t1  where  t1.adminReviewNumber=@adminReviewNumber and (t1.abandonStatus='{0}' or t1.abandonStatus='{1}')" + sqlshere.ToString(), ConstantUtil.STATUS_CHECK2, ConstantUtil.STATUS_CHECK8);
                sqlcount.AppendFormat("select count(1)  from " + ConstantUtil.TABLE_ECONTRACT + " t1  where t1.adminReviewNumber=@adminReviewNumber and (t1.abandonStatus='{0}' or t1.abandonStatus='{1}')" + sqlshere.ToString(), ConstantUtil.STATUS_CHECK2, ConstantUtil.STATUS_CHECK8);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection),
                    new SqlParameter("@userAccount",userAccount)
                 //new SqlParameter("@signedtime_begin",signedtime_begin),
                 //new SqlParameter("@signedtime_end",signedtime_end)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 其他审核
            else
            {
                sqldata.Append(@" select t1.*,t1.status as status1 from Econtract t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection = @flowdirection   and t1.abandonStatus like '%'+ t2.approvalnode" + sqlshere.ToString());
                sqlcount.Append(@" select count(1) from Econtract t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection = @flowdirection  and t1.abandonStatus like '%'+ t2.approvalnode" + sqlshere.ToString());
                SqlParameter[] sqlpps = new SqlParameter[] {
                    new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection),
                   new SqlParameter("@userAccount",userAccount),
                 //new SqlParameter("@signedtime_begin",signedtime_begin),
                 //new SqlParameter("@signedtime_end",signedtime_end)
                };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion


        }

        #endregion

        #region 获取审核列表

        #region 获取进境出境审核列表
        //一个人一个角色额加载审核列表
        public string GetReviewContractList(int row, int page, string order, string sort, string flowdirection, string isDesk, string signedtime_begin, string signedtime_end)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate<=@signedtime_end ");
            }

            #region 登录人为admin时，看到所有提交的状态不为新建或退回的待审核的合同
            string loginUser = RequestSession.GetSessionUser().UserAccount.ToString();
            if (loginUser.Equals("admin"))
            {
                sqldata.AppendFormat(@"select t1.*,t1.status as status1 from {0} t1 where status!='{1}' and status!='{2}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT, ConstantUtil.STATUS_NEW, ConstantUtil.STATUS_HY_BACK);
                sqlcount.AppendFormat(@"select count(*) from {0} t1 where status!='{1}' and status!='{2}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT, ConstantUtil.STATUS_NEW, ConstantUtil.STATUS_HY_BACK);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@flowdirection",flowdirection),
                 new SqlParameter("@signedtime_begin",signedtime_begin),
                 new SqlParameter("@signedtime_end",signedtime_end)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            //获取用户所属角色和所属公司名称
            string rolesName = string.Empty;
            string companyName = string.Empty;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userAccountName = RequestSession.GetSessionUser().UserName.ToString();
            string userAccountNumber = RequestSession.GetSessionUser().UserId.ToString();

            #region 业务直线审核
            //如果为业务直线人员审核，即status为待业务直线审核，则加载合同中salesReviewNumber为登陆人账号的合同审核
            string salesReviewName = getSalesReviewMan(userAccountName);
            if (salesReviewName.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select case status
  when '待直线经理审核'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT + " t1  where t1.salesReviewNumber=@salesReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}' or t1.status='{6}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK, ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                sqlcount.AppendFormat(@" select count(1) from " + ConstantUtil.TABLE_ECONTRACT + " t1  where t1.salesReviewNumber=@salesReviewNumber  and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}' or t1.status='{6}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK, ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                SqlParameter[] sqlpps = new SqlParameter[] {
                  new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection),
                   new SqlParameter("@salesReviewNumber",salesReviewName),
                 //new SqlParameter("@signedtime_begin",signedtime_begin),
                 //new SqlParameter("@signedtime_end",signedtime_end)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 合同管理员审核
            //获取合同审核人
            string name = getReviewMan(userAccountName);
            //如果为合同管理员审核，即status为业务直线审核，则加载合同中adminReviewNumber为登陆人账号的合同审核
            if (name.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select case status
  when '待合同管理员审核'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT + " t1  where  t1.adminReviewNumber=@adminReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                sqlcount.AppendFormat("select count(1)  from " + ConstantUtil.TABLE_ECONTRACT + " t1  where t1.adminReviewNumber=@adminReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection),
                    new SqlParameter("@userAccount",userAccount)
                 //new SqlParameter("@signedtime_begin",signedtime_begin),
                 //new SqlParameter("@signedtime_end",signedtime_end)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 其他审核
            else
            {
                //根据登陆用户获取要筛选的条件
                string reviewstatus = string.Empty;
                string strWhere = getStrWhere(userAccount,ref reviewstatus);

                #region old
                //                sqldata.Append(@" select t1.*,t1.status as status1 from Econtract t1,Econtract_approal t2 where (t1.seller = t2.companyname 
                //or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
                //t2.useraccount = @userAccount and t2.flowdirection = @flowdirection   and t1.status = t2.approvalnode" + sqlshere.ToString());
                //                sqlcount.Append(@" select count(1) from Econtract t1,Econtract_approal t2 where (t1.seller = t2.companyname 
                //or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
                //t2.useraccount = @userAccount and t2.flowdirection = @flowdirection  and t1.status = t2.approvalnode" + sqlshere.ToString()); 
                #endregion

                sqldata.Append(@" select case status
  when '"+reviewstatus+@"'then '待审核'
  else '已审核'end as status2,  t1.*,t1.status as status1 from Econtract t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection = @flowdirection  " + sqlshere.ToString()+strWhere.ToString());
                sqlcount.Append(@" select count(1) from Econtract t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection = @flowdirection  " + sqlshere.ToString()+strWhere.ToString());
                SqlParameter[] sqlpps = new SqlParameter[] {
                    new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection),
                   new SqlParameter("@userAccount",userAccount),
                 //new SqlParameter("@signedtime_begin",signedtime_begin),
                 //new SqlParameter("@signedtime_end",signedtime_end)
                };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

        }


        //一个人可以为多个角色加载审核列表?
        public string GetReviewContractListByOther(int row, int page, string order, string sort, string flowdirection)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            //获取用户所属角色和所属公司名称
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userAccountNumber = RequestSession.GetSessionUser().UserId.ToString();
            DataTable dt = getRolesCompanyBySQL(userAccount, flowdirection);
            DataTable ds = new DataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rolesName = dt.Rows[i][0].ToString();
                string companyName = dt.Rows[i][1].ToString();
                string status = loadContractByRolesName(rolesName);
                //如果是审核，则根据sql语句加载相关的合同审核
                sqldata.Append(@" select t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT + " t1  where (t1.buyer=@companyName or t1.seller=@companyName) and t1.status=@status and t1.adminReviewNumber=@adminReviewNumber" + sqlshere.ToString());
                SqlParameter[] sqlpps = new SqlParameter[] {
                new SqlParameter("@companyName",companyName),
                new SqlParameter("@status",status),
                new SqlParameter("@flowdirection",flowdirection),
               new SqlParameter("@adminReviewNumber",userAccountNumber)
            };
                DataTable dv = GetDataTableBySql(sqldata.ToString(), sqlpps);
                ds.Merge(dv);
            }
            sb = ui.ToEasyUIDataGridJson(ds);
            return sb.ToString();
        }

        #region  根据角色名称确定加载那种状态下的合同
        private string loadContractByRolesName(string rolesName)
        {
            string status = string.Empty;
            if (rolesName == ConstantUtil.STATUS_STOCKIN_CHECK2)//业务直线审核
            {
                status = ConstantUtil.STATUS_STOCKIN_CHECK;//提交
            }
            if (rolesName == ConstantUtil.STATUS_STOCKIN_CHECK3)//合同管理员审核
            {
                status = ConstantUtil.STATUS_STOCKIN_CHECK2;
            }
            if (rolesName == ConstantUtil.STATUS_STOCKIN_CHECK4)//业务处总监审核
            {
                status = ConstantUtil.STATUS_STOCKIN_CHECK3;
            }
            if (rolesName == ConstantUtil.STATUS_STOCKIN_CHECK5)//财务人员审核
            {
                status = ConstantUtil.STATUS_STOCKIN_CHECK4;
            }
            if (rolesName == ConstantUtil.STATUS_STOCKIN_CHECK6)//财务总监审核
            {
                status = ConstantUtil.STATUS_STOCKIN_CHECK5;
            }
            if (rolesName == ConstantUtil.STATUS_STOCKIN_CHECK1)//销售总监审核
            {
                status = ConstantUtil.STATUS_STOCKIN_CHECK6;
            }
            return status;
        }
        #endregion
        #endregion

        #region 获取内部清算单审核列表
        public string GetInternalContractList(int row, int page, string order, string sort, string isDesk, string signedtime_begin, string signedtime_end)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            //获取用户所属角色和所属公司名称
            string flowdirection = ConstantUtil.INTERNALCLEARING;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userAccountName = RequestSession.GetSessionUser().UserName.ToString();
            string userAccountNumber = RequestSession.GetSessionUser().UserId.ToString();

            #region 登录人为admin时，看到所有提交的状态不为新建或退回的待审核的合同
            string loginUser = RequestSession.GetSessionUser().UserAccount.ToString();
            if (loginUser.Equals("admin"))
            {

                sqldata.AppendFormat(@"select t1.*,t1.status as status1 from {0} t1 where status!='{1}' and status!='{2}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL, ConstantUtil.STATUS_NEW, ConstantUtil.STATUS_HY_BACK);
                sqlcount.AppendFormat(@"select count(*) from {0} t1 where status!='{1}' and status!='{2}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL, ConstantUtil.STATUS_NEW, ConstantUtil.STATUS_HY_BACK);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@flowdirection",flowdirection),
                 new SqlParameter("@signedtime_begin",signedtime_begin),
                 new SqlParameter("@signedtime_end",signedtime_end)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 业务直线审核
            //如果为业务直线人员审核，即status为待业务直线审核，则加载合同中salesReviewNumber为登陆人账号的合同审核
            string salesReviewName = getSalesReviewMan(userAccountName);
            if (salesReviewName.Equals(userAccountName))
            {
                sqldata.AppendFormat(@"  select case status
  when '待直线经理审核'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_INTERNAL + " t1  where t1.salesReviewNumber=@salesReviewNumber  and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}' or t1.status='{6}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK, ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                sqlcount.AppendFormat(@" select count(1) from " + ConstantUtil.TABLE_ECONTRACT_INTERNAL + " t1  where t1.salesReviewNumber=@salesReviewNumber  and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}' or t1.status='{6}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK, ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@salesReviewNumber",salesReviewName),
                 new SqlParameter("@flowdirection",flowdirection)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 合同管理员审核
            //获取合同审核人
            string name = getReviewMan(userAccountName);
            //如果为合同管理员审核，即status为业务直线审核，则加载合同中adminReviewNumber为登陆人账号的合同审核
            if (name.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select case status
  when '待合同管理员审核'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_INTERNAL + " t1  where  t1.adminReviewNumber=@adminReviewNumber  and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                sqlcount.AppendFormat("select count(1)  from " + ConstantUtil.TABLE_ECONTRACT_INTERNAL + " t1  where t1.adminReviewNumber=@adminReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 其他审核
            else
            {
                string reviewstatus = string.Empty;
                string strWhere = getStrWhere(userAccount, ref reviewstatus);
                sqldata.AppendFormat(@"select case status
  when '" + reviewstatus + @"'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from {0} t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection=@flowdirection  " + sqlshere.ToString()+strWhere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                sqlcount.AppendFormat(@" select count(1) from {0} t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection=@flowdirection  " + sqlshere.ToString() + strWhere.ToString(), ConstantUtil.TABLE_ECONTRACT_INTERNAL);
                SqlParameter[] sqlpps = new SqlParameter[] {
                    new SqlParameter("@userAccount",userAccount),
                       new SqlParameter("@flowdirection",ConstantUtil.INTERNALCLEARING),
                };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion



        }
        #endregion

        #region 获取物流合同审核列表
        public string GetLogisticsReviewContractList(int row, int page, string order, string sort, string isDesk)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            //获取用户所属角色和所属公司名称
            string flowdirection = ConstantUtil.LOGISTICS;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userAccountName = RequestSession.GetSessionUser().UserName.ToString();
            string userAccountNumber = RequestSession.GetSessionUser().UserId.ToString();

            #region 业务直线审核
            //如果为业务直线人员审核，即status为待业务直线审核，则加载合同中salesReviewNumber为登陆人账号的合同审核
            string salesReviewName = getSalesReviewMan(userAccountName);
            if (salesReviewName.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where t1.salesReviewNumber=@salesReviewNumber and t1.status='{0}'" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK);
                sqlcount.AppendFormat(@" select count(1) from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where t1.salesReviewNumber=@salesReviewNumber and t1.status='{0}'" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@salesReviewNumber",salesReviewName),
                 new SqlParameter("@flowdirection",flowdirection)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 合同管理员审核
            //获取合同审核人
            string name = getReviewMan(userAccountName);
            //如果为合同管理员审核，即status为业务直线审核，则加载合同中adminReviewNumber为登陆人账号的合同审核
            if (name.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where  t1.adminReviewNumber=@adminReviewNumber and t1.status='{0}'" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2);
                sqlcount.AppendFormat("select count(1)  from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where t1.adminReviewNumber=@adminReviewNumber and t1.status='{0}'" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 其他审核
            else
            {
                sqldata.Append(@" select t1.*,t1.status as status1 from Econtract_logistics t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.logisticsContractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection=@flowdirection  and t1.status = t2.approvalnode" + sqlshere.ToString());
                sqlcount.Append(@" select count(1) from Econtract_logistics t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.logisticsContractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection=@flowdirection  and t1.status = t2.approvalnode" + sqlshere.ToString());
                SqlParameter[] sqlpps = new SqlParameter[] {
                    new SqlParameter("@userAccount",userAccount),
                       new SqlParameter("@flowdirection",ConstantUtil.LOGISTICS),
                };
                #region old
                //getRolesCompanyBySQL(userAccount, ref rolesName, ref companyName);
                ////根据角色名称确定加载那种状态下的合同
                //string status = loadContractByRolesName(rolesName);
                ////如果是审核，则根据sql语句加载相关的合同审核
                //sqldata.Append(@" select t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where (t1.buyer=@companyName or t1.seller=@companyName) and t1.status=@status" + sqlshere.ToString());
                //SqlParameter[] sqlpps = new SqlParameter[] {
                //    new SqlParameter("@companyName",companyName),
                //    new SqlParameter("@status",status),
                //};
                //sqlcount.Append("select count(1)  from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where (t1.buyer=@companyName or t1.seller=@companyName)  and t1.status=@status" + sqlshere.ToString()); 
                #endregion
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

        }
        #endregion

        #region 获取服务合同审核列表
        public string GetServiceReviewContractList(int row, int page, string order, string sort, string isDesk)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            //获取用户所属角色和所属公司名称
            string flowdirection = ConstantUtil.LOGISTICS;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userAccountName = RequestSession.GetSessionUser().UserName.ToString();
            string userAccountNumber = RequestSession.GetSessionUser().UserId.ToString();

            #region 登录人为admin时，看到所有提交的状态不为新建或退回的待审核的合同
            string loginUser = RequestSession.GetSessionUser().UserAccount.ToString();
            if (loginUser.Equals("admin"))
            {

                sqldata.AppendFormat(@"select t1.*,t1.status as status1 from {0} t1 where status!='{1}' and status!='{2}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE, ConstantUtil.STATUS_NEW, ConstantUtil.STATUS_HY_BACK);
                sqlcount.AppendFormat(@"select count(*) from {0} t1 where status!='{1}' and status!='{2}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE, ConstantUtil.STATUS_NEW, ConstantUtil.STATUS_HY_BACK);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@flowdirection",flowdirection),
              
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 业务直线审核
            //如果为业务直线人员审核，即status为待业务直线审核，则加载合同中salesReviewNumber为登陆人账号的合同审核
            //string salesReviewName = getSalesReviewMan(userAccountName);
            //if (salesReviewName.Equals(userAccountName))
            //{
            //    sqldata.AppendFormat(@" select t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_SERVICE + " t1  where t1.salesReviewNumber=@salesReviewNumber and t1.status='{0}'" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK);
            //    sqlcount.AppendFormat(@" select count(1) from " + ConstantUtil.TABLE_ECONTRACT_SERVICE + " t1  where t1.salesReviewNumber=@salesReviewNumber and t1.status='{0}'" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK);
            //    SqlParameter[] sqlpps = new SqlParameter[] {
            //     new SqlParameter("@salesReviewNumber",salesReviewName),
            //     new SqlParameter("@flowdirection",flowdirection)
            //};
            //    StringBuilder sb = new StringBuilder();
            //    if (isDesk == "true")
            //    {
            //        sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            //    }
            //    else
            //    {
            //        sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            //    }
            //    return sb.ToString();
            //}
            #endregion

            #region 合同管理员审核
            //获取合同审核人
            string name = getReviewMan(userAccountName);
            //如果为合同管理员审核，即status为业务直线审核，则加载合同中adminReviewNumber为登陆人账号的合同审核
            if (name.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select case status
  when '待合同管理员审核'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_SERVICE + " t1  where  t1.adminReviewNumber=@adminReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                sqlcount.AppendFormat("select count(1)  from " + ConstantUtil.TABLE_ECONTRACT_SERVICE + " t1  where t1.adminReviewNumber=@adminReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 其他审核
            else
            {
                //根据登陆用户获取要筛选的条件
                string reviewstatus = string.Empty;
                string strWhere = getStrWhere(userAccount, ref reviewstatus);

                sqldata.AppendFormat(@" select case status
  when '" + reviewstatus + @"'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from {0} t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection=@flowdirection  " + sqlshere.ToString() + strWhere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                sqlcount.AppendFormat(@" select count(1) from {0} t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection=@flowdirection  " + sqlshere.ToString() + strWhere.ToString(), ConstantUtil.TABLE_ECONTRACT_SERVICE);
                SqlParameter[] sqlpps = new SqlParameter[] {
                    new SqlParameter("@userAccount",userAccount),
                       new SqlParameter("@flowdirection",ConstantUtil.LOGISTICS),
                };
                #region old
                //getRolesCompanyBySQL(userAccount, ref rolesName, ref companyName);
                ////根据角色名称确定加载那种状态下的合同
                //string status = loadContractByRolesName(rolesName);
                ////如果是审核，则根据sql语句加载相关的合同审核
                //sqldata.Append(@" select t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where (t1.buyer=@companyName or t1.seller=@companyName) and t1.status=@status" + sqlshere.ToString());
                //SqlParameter[] sqlpps = new SqlParameter[] {
                //    new SqlParameter("@companyName",companyName),
                //    new SqlParameter("@status",status),
                //};
                //sqlcount.Append("select count(1)  from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where (t1.buyer=@companyName or t1.seller=@companyName)  and t1.status=@status" + sqlshere.ToString()); 
                #endregion
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

        }
        #endregion

        #region 获取合同审核列表
        /// <summary>
        /// 获取合同审核列表
        /// </summary>
        /// <param name="contractNo">合同号</param>
        /// <returns></returns>
        public StringBuilder GetReviewContractList(string contractNo)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            sqldata.Append(" select * from reviewData where  contractNo=@contractNo order by reviewdate asc ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sqldata, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK)//待直线经理审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK8;//业务员
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK2)//待合同管理员审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK9;//直线经理审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK3)//待业务处主管审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK10;//合同管理员审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK4)//待财务负责人审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK11;//业务处主管审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK5)//待财务主管审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK12;//财务负责人审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK6)//待董事长审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK13;//财务主管审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK1)//审批通过
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK14;//董事长审核
                }

            }
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                int count = dt.Rows.Count;
                sb.Append(JsonHelper.ToJson(dt, "rows"));
                sb.Insert(1, "\"total\":" + count + ",");
            }
            return sb;

        }

        #endregion

        #region 获取服务合同审核列表
        /// <summary>
        /// 获取服务合同审核列表
        /// </summary>
        /// <param name="contractNo">合同号</param>
        /// <returns></returns>
        public StringBuilder GetServiceReviewList(string contractNo)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            sqldata.Append(" select * from reviewData where  contractNo=@contractNo order by reviewdate asc ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sqldata, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, 0);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK)//待直线经理审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK8;//业务员
                }
                //if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK2)//待合同管理员审核
                //{
                //    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK8;//业务员
                //}
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK3)//待业务处主管审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK10;//合同管理员审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK4)//待财务负责人审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK11;//业务处主管审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK5)//待财务主管审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK12;//财务负责人审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK6)//待董事长审核
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK13;//财务主管审核
                }
                else if (dt.Rows[i]["reviewstatus"].ToString() == ConstantUtil.STATUS_STOCKIN_CHECK1)//审批通过
                {
                    dt.Rows[i]["reviewstatus"] = ConstantUtil.STATUS_STOCKIN_CHECK14;//董事长审核
                }

            }
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                int count = dt.Rows.Count;
                sb.Append(JsonHelper.ToJson(dt, "rows"));
                sb.Insert(1, "\"total\":" + count + ",");
            }
            return sb;

        }

        #endregion

        #region 获取管理合同审核列表
        public string GetManageReviewContractList(int row, int page, string order, string sort, string isDesk)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder();
            //获取用户所属角色和所属公司名称
            string flowdirection = ConstantUtil.LOGISTICS;
            string userAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            string userAccountName = RequestSession.GetSessionUser().UserName.ToString();
            string userAccountNumber = RequestSession.GetSessionUser().UserId.ToString();

            #region 登录人为admin时，看到所有提交的状态不为新建或退回的待审核的合同
            string loginUser = RequestSession.GetSessionUser().UserAccount.ToString();
            if (loginUser.Equals("admin"))
            {

                sqldata.AppendFormat(@"select t1.*,t1.status as status1 from {0} t1 where status!='{1}' and status!='{2}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS, ConstantUtil.STATUS_NEW, ConstantUtil.STATUS_HY_BACK);
                sqlcount.AppendFormat(@"select count(*) from {0} t1 where status!='{1}' and status!='{2}'" + sqlshere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS, ConstantUtil.STATUS_NEW, ConstantUtil.STATUS_HY_BACK);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@flowdirection",flowdirection),
              
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 业务直线审核
            //如果为业务直线人员审核，即status为待业务直线审核，则加载合同中salesReviewNumber为登陆人账号的合同审核
            string salesReviewName = getSalesReviewMan(userAccountName);
            if (salesReviewName.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select case status
  when '待直线经理审核'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where t1.salesReviewNumber=@salesReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}' or t1.status='{6}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK, ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                sqlcount.AppendFormat(@" select count(1) from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where t1.salesReviewNumber=@salesReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}' or t1.status='{6}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK, ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@salesReviewNumber",salesReviewName),
                 new SqlParameter("@flowdirection",flowdirection)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 合同管理员审核
            //获取合同审核人
            string name = getReviewMan(userAccountName);
            //如果为合同管理员审核，即status为业务直线审核，则加载合同中adminReviewNumber为登陆人账号的合同审核
            if (name.Equals(userAccountName))
            {
                sqldata.AppendFormat(@" select case status
  when '待合同管理员审核'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where  t1.adminReviewNumber=@adminReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                sqlcount.AppendFormat("select count(1)  from " + ConstantUtil.TABLE_ECONTRACT_LOGISTICS + " t1  where t1.adminReviewNumber=@adminReviewNumber and (t1.status='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}' or t1.status='{5}')" + sqlshere.ToString(), ConstantUtil.STATUS_STOCKIN_CHECK2, ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                SqlParameter[] sqlpps = new SqlParameter[] {
                 new SqlParameter("@adminReviewNumber",userAccount),
                 new SqlParameter("@flowdirection",flowdirection)
            };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

            #region 其他审核
            else
            {
                string reviewstatus = string.Empty;
                string strWhere = getStrWhere(userAccount, ref reviewstatus);
                sqldata.AppendFormat(@" select case status
  when '" + reviewstatus + @"'then '待审核'
  else '已审核'end as status2, t1.*,t1.status as status1 from {0} t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection=@flowdirection  and t1.status = t2.approvalnode" + sqlshere.ToString() + strWhere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                sqlcount.AppendFormat(@" select count(1) from {0} t1,Econtract_approal t2 where (t1.seller = t2.companyname 
or t1.buyer = t2.companyname) and t1.contractNo like t2.prioritycode+'%' and 
t2.useraccount = @userAccount and t2.flowdirection=@flowdirection  and t1.status = t2.approvalnode" + sqlshere.ToString() + strWhere.ToString(), ConstantUtil.TABLE_ECONTRACT_LOGISTICS);
                SqlParameter[] sqlpps = new SqlParameter[] {
                    new SqlParameter("@userAccount",userAccount),
                       new SqlParameter("@flowdirection",ConstantUtil.LOGISTICS),
                };
                StringBuilder sb = new StringBuilder();
                if (isDesk == "true")
                {
                    sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                }
                else
                {
                    sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                }
                return sb.ToString();
            }
            #endregion

        }
        #endregion

        #endregion

        #region 获取产品列表

        #region 获取所有产品列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcode">产品编码</param>
        /// <param name="pname">产品名称</param>
        /// <param name="category">产品类别</param>
        /// <param name="row"></param>
        /// <param name="page"></param>
        /// <param name="order"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public StringBuilder GetProductList(string pcode, string pname, string category, int row, int page, string order, string sort)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();

            sqldata.Append("select t1.*,isnull( t2.ifinspection,'否')  as customsInspec from bproduct t1 left join bproducthss t2 on t1.hsscode=t2.pcode where 1=1");
            if (!string.IsNullOrEmpty(category))
            {
                sqldata.Append(" and t1.productCategory=@category");
            }
            if (!string.IsNullOrEmpty(pcode))
            {
                sqldata.Append(" and t1.pcode=@pcode");
            }
            if (!string.IsNullOrEmpty(pname))
            {
                sqldata.Append(" and t1.pname=@pname");
            }
            sqlcount.Append("select count(1) from bproduct where 1=1");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@category",category),
                    new SqlParameter("@pname",pname),
                    new SqlParameter("@pcode",pcode),
                };

            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb;

        }
        #endregion

        #region 获取合同产品列表
        /// <summary>
        /// 获取合同产品列表
        /// </summary>
        /// <param name="contractNo">合同号</param>
        /// <returns></returns>
        public StringBuilder GetContractProductList(string contractNo)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();

            sqldata.Append(" select * from Econtract_ap where contractNo=@contractNo");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
                 
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb;
        }
        #endregion



        #region 获取发货已申请合同产品列表
        public StringBuilder GetSendoutProductList(string createDateTag)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            sqldata.AppendFormat(@"select Id, createDateTag, contractNo, buyer, seller, productCategory, pcode, pname, packspec, 
spec, pallet, unit, packdes, qunit, price, priceUnit, ifcheck,
 ifplace, hsCode, checkNoticeStatus, contactStatus, applystatus, createman,(sendQuantity*price) as amount,sendQuantity as quantity from {0} where createDateTag=@createDateTag", ConstantUtil.TABLE_SENDOUTAPPDETAILS);
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@createDateTag",createDateTag),

                };
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb;
        }
        #endregion

        #region 获取物流合同产品列表
        public StringBuilder GetLogisticsContractList(int row, int page, string order, string sort)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
               
                };

            sqldata.Append(" select * from Econtract_logistics where 1=1");
            sqlcount.Append("select count(1) from Econtract_logistics where 1=1");
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb;

        }
        #endregion
        #endregion

        #region 加载模板列表，根据创建人，模板类型筛选
        /// <summary>
        /// 加载模板列表，根据创建人，模板类型筛选
        /// </summary>
        /// <param name="templateCategory">模板类型</param>
        /// <param name="createmanname">创建人</param>
        /// <returns></returns>
        public StringBuilder GetTemplateList(string templateCategory, string createmanname)
        {
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            sqldata.Append(" select * from btemplate_contactEcontract where templateCategory=@templateCategory and createmanname=@createmanname");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@templateCategory",templateCategory),
                    new SqlParameter("@createmanname",createmanname)
                   
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb;
        }
        #endregion

        #region 根据合同号获取产品分批列表
        // public StringBuilder GetSplitProductList(string contractNo)
        //{


        //}
        #endregion

        #region 增删改

        #region 合同添加和编辑
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorinfo"></param>
        /// <param name="contractNo">合同号</param>
        /// <param name="htContract">合同hash表</param>
        /// <param name="productListTable">产品表</param>
        /// <param name="splitListTable">分批发货产品表</param>
        /// <param name="templateno">模板编号</param>
        /// <returns></returns>
        public bool addOrEditContract(ref string errorinfo, string contractNo, Hashtable htContract, List<Hashtable> productListTable,
             List<Hashtable> splitListTable, string templateno, string status, string createman, List<Hashtable> templateTable, string originalContractNo)
        {
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            //获取主合同sql
            if (string.IsNullOrEmpty(originalContractNo))
            {
                //不是退回后重新提交的合同
                SqlUtil.getBatchSqls(htContract, ConstantUtil.TABLE_ECONTRACT, "contractNo", contractNo, ref sqls, ref objs);
            }
            else
            {

                #region 把原合同的审核数据更新到新合同号中
                //获取原合同审核数据
                StringBuilder sqlUpdateReview = new StringBuilder(@"update reviewdata set contractNo=@originalContractNo where contractNo=@contractNo");
                SqlParam[] pms = new SqlParam[2] { new SqlParam("@contractNo", contractNo), new SqlParam("@originalContractNo", originalContractNo) };
                sqls.Add(sqlUpdateReview);
                objs.Add(pms);
                #endregion

                SqlUtil.getBatchSqls(htContract, ConstantUtil.TABLE_ECONTRACT, "contractNo", originalContractNo, ref sqls, ref objs);
            }

            List<Hashtable> pro_list = new List<Hashtable>();
            #region 添加产品sql
            foreach (Hashtable hs in productListTable)
            {
                Hashtable htProduct = new Hashtable();
                Hashtable ht_prime = new Hashtable();
                ht_prime.Add("contractNo", contractNo);
                ht_prime.Add("pcode", hs["pcode"]);
                htProduct["contractNo"] = contractNo;
                htProduct["attachmentno"] = "";
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
                htProduct["skinWeight"] = hs["skinWeight"] ?? string.Empty;
                pro_list.Add(htProduct);
            }
            //先删除后添加
            SqlUtil.getBatchSqls(pro_list, ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", contractNo, ref sqls, ref objs);
            #endregion

            #region 保存模板
            List<Hashtable> pro_temp = new List<Hashtable>();
            if (templateTable != null)
            {
                int a = 0;
                foreach (Hashtable hs in templateTable)
                {
                    a = a + 1;

                    Hashtable htTemplate = new Hashtable();
                    Hashtable ht_temp_prime = new Hashtable();
                    if (string.IsNullOrWhiteSpace(hs["sortno"].ToString()))
                    {
                        htTemplate["sortno"] = a;
                        ht_temp_prime.Add("sortno", a);
                    }
                    else
                    {
                        htTemplate["sortno"] = hs["sortno"];
                        ht_temp_prime.Add("sortno", hs["sortno"]);
                    }
                    htTemplate["templateno"] = templateno;
                    htTemplate["contractno"] = contractNo;
                    htTemplate["chncontent"] = hs["chncontent"];
                    htTemplate["engcontent"] = hs["engcontent"];
                    htTemplate["ruscontent"] = hs["ruscontent"];
                    htTemplate["isinline"] = hs["isinline"];
                    htTemplate["variable"] = hs["variable"];
                    ht_temp_prime.Add("contractno", contractNo);
                    ht_temp_prime.Add("templateno", templateno);
                    pro_temp.Add(htTemplate);

                }
                //先删除后添加
                SqlUtil.getBatchSqls(pro_temp, ConstantUtil.TABLE_ECONTRACT_TEMPLATE, "contractNo", contractNo, ref sqls, ref objs);
            }
            #endregion

            #region 提交时,合同管理员创建时向审核记录表中插入提交状态

            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                Hashtable ht = new Hashtable();
                Hashtable ht_review_prime = new Hashtable();
                ht["reviewstatus"] = status;
                ht["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;
                ht["reviewman"] = createman;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                ht_review_prime.Add("contractNo", contractNo);
                ht_review_prime.Add("reviewstatus", status);
                List<Hashtable> review_list = new List<Hashtable>();
                review_list.Add(ht);
                SqlUtil.getBatchSqls(review_list, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
                //SqlUtil.getBatchSqls(ht, ht_review_prime, ConstantUtil.TABLE_REVIEWDATA, ref sqls, ref objs);
            }
            #endregion
            //批量执行sql
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
            #region old
            //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            //{
            //    try
            //    {
            //        bll.SqlTran = bll.SqlCon.BeginTransaction();
            //        //保存合同主体
            //        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT, "contractNo", htContract["contractNo"].ToString(), htContract);
            //        if (IsOk)
            //        {
            //            //先删除后保存
            //            int b = DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", contractNo);
            //            //保存合同产品
            //            foreach (Hashtable hs in productListTable)
            //            {
            //                Hashtable htProduct = new Hashtable();
            //                htProduct["contractNo"] = contractNo;
            //                htProduct["attachmentno"] = "";
            //                htProduct["pcode"] = hs["pcode"];
            //                htProduct["pname"] = hs["pname"];
            //                htProduct["quantity"] = hs["quantity"];
            //                htProduct["qunit"] = hs["qunit"];
            //                htProduct["price"] = hs["price"];
            //                htProduct["priceUnit"] = hs["priceUnit"];
            //                htProduct["amount"] = hs["amount"];
            //                htProduct["packspec"] = hs["packspec"];
            //                htProduct["packing"] = hs["packing"];
            //                htProduct["packdes"] = hs["packdes"];
            //                htProduct["spec"] = hs["spec"];
            //                htProduct["pallet"] = hs["pallet"];
            //                htProduct["unit"] = hs["unit"];
            //                htProduct["ifcheck"] = hs["ifcheck"];
            //                htProduct["ifplace"] = hs["ifplace"];
            //                htProduct["hsCode"] = hs["hsCode"];
            //                htProduct["pnameen"] = hs["pnameen"];
            //                htProduct["pnameru"] = hs["pnameru"];
            //                htProduct["packagesNumber"] = hs["packagesNumber"] ?? string.Empty;
            //                htProduct["priceAdd"] = hs["priceAdd"] ?? string.Empty;
            //                htProduct["amountfloat"] = hs["amountfloat"] ?? string.Empty;
            //                htProduct["skinWeight"] = hs["skinWeight"] ?? string.Empty;
            //                bool pro = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", htProduct["contractNo"].ToString(), "pcode", htProduct["pcode"].ToString(), htProduct);

            //            }
            //            if (templateTable != null)
            //            {
            //                //保存模板
            //                DataFactory.SqlDataBase().DeleteData("Econtract_template", "contractNo", contractNo);
            //                int a = 0;
            //                foreach (Hashtable hs in templateTable)
            //                {
            //                    a = a + 1;

            //                    Hashtable htTemplate = new Hashtable();
            //                    if (string.IsNullOrWhiteSpace(hs["sortno"].ToString()))
            //                    {
            //                        htTemplate["sortno"] = a;
            //                    }
            //                    else
            //                    {
            //                        htTemplate["sortno"] = hs["sortno"];
            //                    }
            //                    htTemplate["templateno"] = templateno;

            //                    htTemplate["contractno"] = contractNo;
            //                    htTemplate["chncontent"] = hs["chncontent"];
            //                    htTemplate["engcontent"] = hs["engcontent"];
            //                    htTemplate["ruscontent"] = hs["ruscontent"];
            //                    htTemplate["isinline"] = hs["isinline"];
            //                    htTemplate["variable"] = hs["variable"];
            //                    int r = DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_ECONTRACT_TEMPLATE, htTemplate);
            //                }
            //            }

            //            //提交时,合同管理员创建时向审核记录表中插入提交状态
            //            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            //            {
            //                Hashtable ht = new Hashtable();
            //                ht["reviewstatus"] = status;
            //                ht["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;
            //                ht["reviewman"] = createman;
            //                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
            //                ht["contractNo"] = contractNo;
            //                bool ok = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo, "reviewstatus", status, ht);
            //            }

            //        }

            //        bll.SqlTran.Commit();
            //        return IsOk;

            //    }
            //    catch (Exception ex)
            //    {
            //        bll.SqlTran.Rollback();
            //        errorinfo = ex.Message;
            //        return false;

            //    }

            //} 
            #endregion
        }
        //发货通知添加和编辑
        public bool addOrEditContractSendNotice(ref string errorinfo, string contractNo, Hashtable htContract, List<Hashtable> productListTable,
         List<Hashtable> splitListTable, string templateno, string status, string createman, List<Hashtable> templateTable)
        {
            bool suc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    //保存合同主体
                    bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT, "contractNo", htContract["contractNo"].ToString(), htContract);
                    if (IsOk)
                    {
                        //先删除后保存
                        int b = DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", contractNo);
                        //保存合同产品
                        foreach (Hashtable hs in productListTable)
                        {
                            Hashtable htProduct = new Hashtable();
                            htProduct["contractNo"] = contractNo;
                            htProduct["attachmentno"] = "";
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
                            bool pro = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", htProduct["contractNo"].ToString(), "pcode", htProduct["pcode"].ToString(), htProduct);

                        }
                        if (templateTable != null)
                        {
                            //保存模板
                            DataFactory.SqlDataBase().DeleteData("Econtract_template", "contractNo", contractNo);
                            int a = 0;
                            foreach (Hashtable hs in templateTable)
                            {
                                a = a + 1;
                                Hashtable htTemplate = new Hashtable();
                                if (string.IsNullOrWhiteSpace(hs["sortno"].ToString()))
                                {
                                    htTemplate["sortno"] = a;
                                }
                                else
                                {
                                    htTemplate["sortno"] = hs["sortno"];
                                }
                                htTemplate["templateno"] = templateno;
                                htTemplate["sortno"] = a;
                                htTemplate["contractno"] = contractNo;
                                htTemplate["chncontent"] = hs["chncontent"];
                                htTemplate["engcontent"] = hs["engcontent"];
                                htTemplate["ruscontent"] = hs["ruscontent"];
                                htTemplate["isinline"] = hs["isinline"];
                                int r = DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_ECONTRACT_TEMPLATE, htTemplate);
                            }
                        }

                    }

                    bll.SqlTran.Commit();
                    return IsOk;

                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;
                    return false;

                }

            }
        }
        //关联合同添加或编辑
        public bool addOrEditContactContract(ref string errorinfo, string contractNo, Hashtable htContract, string purchaseCode, List<Hashtable> productListTable, string templateno, string createDateTag,
            List<Hashtable> templateTable, string status, string createman, string ifcheck)
        {

            #region 校验商检
            //string transport = htContract["transport"].ToString();
            //string applyNo = string.Empty;
            //if (transport == "海运")//海运校验商检
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendFormat(@"select * from {0} where purchaseCode=@purchaseCode", ConstantUtil.TABLE_ECONTRACT_INSPECT);
            //    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[1] { new SqlParam("@purchaseCode", purchaseCode) }, 0);
            //    if (dt.Rows.Count <= 0)//未创商检合同
            //    {
            //        if (ifcheck == "是")
            //        {
            //            errorinfo = "请先商检";
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        applyNo = dt.Rows[0]["inspectApplyNo"].ToString();
            //    }
            //}

            #endregion

            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            //生成主合同sql
            SqlUtil.getBatchSqls(htContract, ConstantUtil.TABLE_ECONTRACT, "contractNo", htContract["contractNo"].ToString(), ref sqls, ref objs);

            #region 保存产品
            ////先删除后保存
            //先删除，再添加
            sqls.Add(new StringBuilder(@"delete from " + ConstantUtil.TABLE_ECONTRACT_AP + " where contractNo=@contractNo"));
            objs.Add(new SqlParam[1] { new SqlParam("@contractNo", contractNo) });
            //int b = DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", contractNo);
            //保存合同产品
            List<Hashtable> list_hb = new List<Hashtable>();
            List<Hashtable> list_htSend = new List<Hashtable>();
            foreach (Hashtable hs in productListTable)
            {
                Hashtable htProduct = new Hashtable();
                htProduct["contractNo"] = contractNo;
                htProduct["attachmentno"] = "";
                htProduct["pcode"] = hs["pcode"];
                htProduct["pname"] = hs["pname"];
                htProduct["quantity"] = hs["quantity"];
                htProduct["qunit"] = hs["qunit"];
                htProduct["price"] = hs["price"];
                htProduct["priceUnit"] = hs["priceUnit"];
                htProduct["amount"] = hs["amount"];
                htProduct["packspec"] = hs["packspec"];
                htProduct["packing"] = hs["packing"];
                htProduct["pallet"] = hs["pallet"];
                htProduct["ifcheck"] = hs["ifcheck"];
                htProduct["ifplace"] = hs["ifplace"];
                htProduct["unit"] = hs["unit"];
                htProduct["hsCode"] = hs["hsCode"];
                htProduct["packagesNumber"] = hs["packagesNumber"] ?? string.Empty;
                htProduct["priceAdd"] = hs["priceAdd"] ?? string.Empty;
                htProduct["amountfloat"] = hs["amountfloat"] ?? string.Empty;
                htProduct["skinWeight"] = hs["skinWeight"] ?? string.Empty;
                list_hb.Add(htProduct);

                #region 向senout表中添加数据
                //Hashtable htSend = new Hashtable();
                //htSend["contractNo"] = purchaseCode;
                ////htSend["applyNo"] = applyNo;
                //htSend["inspectContractNo"] = createDateTag;
                //htSend["pcode"] = hs["pcode"];
                //htSend["pname"] = hs["pname"];
                //htSend["quantity"] = hs["quantity"];
                //htSend["qunit"] = hs["qunit"];
                //list_htSend.Add(htSend);
                #endregion
            }
            SqlUtil.getBatchSqls(list_hb, ConstantUtil.TABLE_ECONTRACT_AP, ref sqls, ref objs);
            //SqlUtil.getBatchSqls(list_htSend, ConstantUtil.TABLE_ECONTRACT_INSPECT_SENDOUT, ref sqls, ref objs);
            #endregion

            #region 添加模板
            //保存模板
            if (templateTable != null)
            {
                //保存模板
                DataFactory.SqlDataBase().DeleteData("Econtract_template", "contractNo", contractNo);
                int a = 0;
                foreach (Hashtable hs in templateTable)
                {
                    a = a + 1;
                    Hashtable htTemplate = new Hashtable();
                    if (string.IsNullOrWhiteSpace(hs["sortno"].ToString()))
                    {
                        htTemplate["sortno"] = a;
                    }
                    else
                    {
                        htTemplate["sortno"] = hs["sortno"];
                    }
                    htTemplate["templateno"] = templateno;
                    htTemplate["sortno"] = a;
                    htTemplate["contractno"] = contractNo;
                    htTemplate["chncontent"] = hs["chncontent"];
                    htTemplate["engcontent"] = hs["engcontent"];
                    htTemplate["ruscontent"] = hs["ruscontent"];
                    htTemplate["isinline"] = hs["isinline"];
                    List<Hashtable> list_htTemp = new List<Hashtable>();
                    list_htTemp.Add(htTemplate);
                    SqlUtil.getBatchSqls(list_htTemp, ConstantUtil.TABLE_ECONTRACT_TEMPLATE, ref sqls, ref objs);
                    //DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_ECONTRACT_TEMPLATE, htTemplate);
                }
            }
            #endregion

            #region 更改发货申请关联状态,追加发货人发货工厂
            Hashtable hb = new Hashtable();
            hb["contactStatus"] = ConstantUtil.STATUS_CONTACT_ED;
            hb["sendFactory"] = htContract["sendFactory"].ToString();
            hb["sendFactoryCode"] = htContract["sendFactoryCode"].ToString();
            hb["sendMan"] = htContract["seller"].ToString();
            hb["sendManCode"] = htContract["sellercode"].ToString();
            SqlUtil.getBatchSqls(hb, ConstantUtil.TABLE_SENDOUTAPPDETAILS, "createDateTag", createDateTag, ref sqls, ref objs);
            //int r = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_SENDOUTAPPDETAILS, "createDateTag", createDateTag, hb); 
            #endregion

            #region 提交时,合同管理员创建时向审核记录表中插入提交状态
            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            {
                Hashtable ht = new Hashtable();
                ht["reviewstatus"] = status;
                ht["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;
                ht["reviewman"] = createman;
                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
                ht["contractNo"] = contractNo;
                List<Hashtable> htReview = new List<Hashtable>();
                htReview.Add(ht);
                SqlUtil.getBatchSqls(htReview, ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo, ref sqls, ref objs);

                //bool ok = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo, "reviewstatus", status, ht);
            }
            #endregion

            int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return result > 0 ? true : false;

            #region old
            //bool suc = false;
            //using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            //{
            //    try
            //    {
            //        bll.SqlTran = bll.SqlCon.BeginTransaction();

            //        //保存合同主体
            //        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT, "contractNo", htContract["contractNo"].ToString(), htContract);
            //        if (IsOk)
            //        {
            //            //先删除后保存
            //            int b = DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", contractNo);
            //            //保存合同产品
            //            foreach (Hashtable hs in productListTable)
            //            {
            //                Hashtable htProduct = new Hashtable();
            //                htProduct["contractNo"] = contractNo;
            //                htProduct["attachmentno"] = "";
            //                htProduct["pcode"] = hs["pcode"];
            //                htProduct["pname"] = hs["pname"];
            //                htProduct["quantity"] = hs["quantity"];
            //                htProduct["qunit"] = hs["qunit"];
            //                htProduct["price"] = hs["price"];
            //                htProduct["priceUnit"] = hs["priceUnit"];
            //                htProduct["amount"] = hs["amount"];
            //                htProduct["packspec"] = hs["packspec"];
            //                htProduct["packing"] = hs["packing"];
            //                htProduct["pallet"] = hs["pallet"];
            //                htProduct["ifcheck"] = hs["ifcheck"];
            //                htProduct["ifplace"] = hs["ifplace"];
            //                htProduct["unit"] = hs["unit"];
            //                htProduct["hsCode"] = hs["hsCode"];
            //                htProduct["packagesNumber"] = hs["packagesNumber"] ?? string.Empty;
            //                htProduct["priceAdd"] = hs["priceAdd"] ?? string.Empty;
            //                htProduct["amountfloat"] = hs["amountfloat"] ?? string.Empty;

            //                #region 向senout表中添加数据
            //                Hashtable htSend = new Hashtable();
            //                htSend["contractNo"] = purchaseCode;
            //                htSend["applyNo"] = applyNo;
            //                htSend["inspectContractNo"] = createDateTag;
            //                htSend["pcode"] = hs["pcode"];
            //                htSend["pname"] = hs["pname"];
            //                htSend["quantity"] = hs["quantity"];
            //                htSend["qunit"] = hs["qunit"];
            //                DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_ECONTRACT_INSPECT_SENDOUT, htSend);
            //                #endregion

            //                suc = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", htProduct["contractNo"].ToString(), "pcode", htProduct["pcode"].ToString(), htProduct);
            //            }
            //            //保存模板

            //            if (templateTable != null)
            //            {
            //                //保存模板
            //                DataFactory.SqlDataBase().DeleteData("Econtract_template", "contractNo", contractNo);
            //                int a = 0;
            //                foreach (Hashtable hs in templateTable)
            //                {
            //                    a = a + 1;
            //                    Hashtable htTemplate = new Hashtable();
            //                    if (string.IsNullOrWhiteSpace(hs["sortno"].ToString()))
            //                    {
            //          +              htTemplate["sortno"] = a;
            //                    }
            //                    else
            //                    {
            //                        htTemplate["sortno"] = hs["sortno"];
            //                    }
            //                    htTemplate["templateno"] = templateno;
            //                    htTemplate["sortno"] = a;
            //                    htTemplate["contractno"] = contractNo;
            //                    htTemplate["chncontent"] = hs["chncontent"];
            //                    htTemplate["engcontent"] = hs["engcontent"];
            //                    htTemplate["ruscontent"] = hs["ruscontent"];
            //                    htTemplate["isinline"] = hs["isinline"];
            //                    DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_ECONTRACT_TEMPLATE, htTemplate);
            //                }
            //            }

            //            //更改发货申请关联状态
            //            Hashtable hb = new Hashtable();
            //            hb["contactStatus"] = ConstantUtil.STATUS_CONTACT_ED;
            //            hb["sendFactory"] = htContract["sendFactory"].ToString();
            //            int r = DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_SENDOUTAPPDETAILS, "createDateTag", createDateTag, hb);
            //            //提交时,合同管理员创建时向审核记录表中插入提交状态
            //            if (status == ConstantUtil.STATUS_STOCKIN_CHECK || status == ConstantUtil.STATUS_STOCKIN_CHECK3)
            //            {
            //                Hashtable ht = new Hashtable();
            //                ht["reviewstatus"] = status;
            //                ht["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;
            //                ht["reviewman"] = createman;
            //                ht["reviewdate"] = DateTimeHelper.ShortDateTimeS;
            //                ht["contractNo"] = contractNo;
            //                bool ok = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_REVIEWDATA, "contractNo", contractNo, "reviewstatus", status, ht);
            //            }
            //        }


            //        bll.SqlTran.Commit();

            //    }
            //    catch (Exception ex)
            //    {
            //        bll.SqlTran.Rollback();
            //        errorinfo = ex.Message;

            //    }
            //    return suc;
            //} 
            #endregion
        }
        //保存模板
        private bool saveTemplate(string contractNo, string templateno)
        {
            int r = 0;
            StringBuilder sbmb = new StringBuilder("");
            DataFactory.SqlDataBase().DeleteData("Econtract_template", "contractNo", contractNo);
            sbmb.Append(@"insert into Econtract_template(templateno, sortno, chncontent, engcontent, ruscontent, isinline, contractno) 
select templateno, sortno, chncontent, engcontent, ruscontent, isinline,@contractNo from Econtract_template where templateno=@templateno ; ");
            SqlParameter[] mms1 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo},
                            new SqlParameter{ParameterName="@templateno",Value=templateno}
       
             };

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                r = bll.ExecuteNonQuery(sbmb.ToString(), mms1);
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

        #endregion

        #region 合同附件添加和编辑
        public bool addOrEditContractAttach(ref string errorinfo, string contractNo, Hashtable htContract, List<Hashtable> productListTable,
               Hashtable htTemplate)
        {
            bool suc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    //保存合同主体
                    bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT, "contractNo", htContract["contractNo"].ToString(), htContract);
                    if (IsOk)
                    {
                        //保存合同产品
                        foreach (Hashtable hs in productListTable)
                        {
                            Hashtable htProduct = new Hashtable();
                            htProduct["contractNo"] = contractNo;
                            htProduct["attachmentno"] = "";
                            htProduct["pcode"] = hs["pcode"];
                            htProduct["pname"] = hs["pname"];
                            htProduct["quantity"] = hs["quantity"];
                            htProduct["qunit"] = hs["qunit"];
                            htProduct["price"] = hs["price"];
                            htProduct["amount"] = hs["amount"];
                            htProduct["packspec"] = hs["packspec"];
                            htProduct["packing"] = hs["packing"];
                            htProduct["pallet"] = hs["pallet"];
                            htProduct["ifcheck"] = hs["ifcheck"];
                            htProduct["ifplace"] = hs["ifplace"];
                            suc = DataFactory.SqlDataBase().Submit_AddOrEdit_2(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", htProduct["contractNo"].ToString(), "pcode", htProduct["pcode"].ToString(), htProduct);
                        }
                        //保存模板
                        //suc = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT_A_Item, "id", htTemplate["id"].ToString(), htTemplate);
                    }

                    bll.SqlTran.Commit();

                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;

                }
                return suc;
            }
        }
        //保存模板
        private bool saveAttachTemplate(string contractNo, List<Hashtable> templatelisttable)
        {
            //保存分批发货产品
            bool suc = false;
            foreach (var hs in templatelisttable)
            {
                Hashtable httemplate = new Hashtable();
                httemplate["contractNo"] = contractNo;
                httemplate["id"] = hs["id"] ?? string.Empty;
                httemplate["item1"] = hs["item1"];
                httemplate["item2"] = hs["item2"];
                httemplate["item3"] = hs["item3"];
                httemplate["item4"] = hs["item4"];
                httemplate["item5"] = hs["item5"];
                httemplate["item1foreign"] = hs["item1foreign"];
                httemplate["item2foreign"] = hs["item2foreign"];
                httemplate["item3foreign"] = hs["item3foreign"];
                httemplate["item4foreign"] = hs["item4foreign"];
                httemplate["item5foreign"] = hs["item5foreign"];
                suc = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_ECONTRACT_A_Item, "id", httemplate["id"].ToString(), httemplate);
            }
            return suc;
        }
        #endregion

        #region 获取单个combobox列表
        /// <summary>
        /// 获取单个combobox列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public StringBuilder getComboboxList(string tableName, string parameter)
        {
            RM.Busines.JsonHelperEasyUi jsonh = new JsonHelperEasyUi();
            string sql = "select * from " + tableName;
            SqlParameter[] pms = new SqlParameter[]{
            new SqlParameter("@parameter",parameter)
        };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataTable ds = bll.ExecDatasetSql(sql, pms).Tables[0];
                return jsonh.ToEasyUIComboxJson(ds);
            }
        }
        #endregion

        #region 获取单个表列表，不分页
        public string getTableList(string tableName, string pvName, string pvVal)
        {
            RM.Busines.JsonHelperEasyUi jsonh = new JsonHelperEasyUi();
            SqlParameter[] pms = new SqlParameter[]{
            new SqlParameter("@parameter",pvVal)
        };
            string sql = "select * from " + tableName + " where " + pvName + "=@parameter";
            StringBuilder sb = new StringBuilder(sql);
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {

                return jsonh.GetDatatableJsonString(sb, pms).ToString();
            }
        }
        #endregion

        #region 根据字段获取dataTable
        public DataTable GetDataTableByColumn(string tableName, string pvName, string pvVal)
        {
            SqlParameter[] pms = new SqlParameter[]{
            new SqlParameter("@parameter",pvVal)
        };
            string sql = "select * from " + tableName + " where " + pvName + "=@parameter";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                return bll.ExecDatasetSql(sql, pms).Tables[0];
            }
        }
        #endregion

        #region 根据sql语句获取dataTable
        public DataTable GetDataTableBySql(string sql, SqlParameter[] pms)
        {
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                return bll.ExecDatasetSql(sql, pms).Tables[0];
            }
        }
        #endregion

        #region 根据sql语句获取单个返回值
        public object getScalarString(string sql, SqlParameter[] pms)
        {
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                return bll.ExecuteScalar(sql, pms) ?? string.Empty;
            }
        }
        #endregion

        #region 根据用户id获取用户角色和所在公司名
        public void getRolesCompanyByUserId(string userId, ref string rolesName, ref string companyName)
        {
            //根据登录用户获取用户所属公司，用户角色名称
            string sql = @"select RolesName,Remark from Tb_Roles where Id in (select RolesId from Tb_RolesAddUser where UserId=@userId)";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@userId",userId)
            };
            DataTable dt = GetDataTableBySql(sql, pms);
            rolesName = dt.Rows[0][0].ToString();
            companyName = dt.Rows[0][1].ToString();

        }
        public void getRolesCompanyBySQL(string userId, string flowdirection, ref string rolesName, ref string companyName)
        {
            //根据登录用户获取用户所属公司，用户角色名称
            string sql = @"select approvalnode,companyname from Econtract_approal where useraccount=@userId and flowdirection=@flowdirection";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@userId",userId),
                new SqlParameter("@flowdirection",flowdirection)
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataTable dt = bll.ExecDatasetSql(sql, pms).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    rolesName = dt.Rows[0][0].ToString();
                    companyName = dt.Rows[0][1].ToString();
                }

            }


        }
        public DataTable getRolesCompanyBySQL(string userId, string flowdirection)
        {
            //根据登录用户获取用户所属公司，用户角色名称
            string sql = @"select approvalnode,companyname from Econtract_approal where useraccount=@userId and flowdirection=@flowdirection";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@userId",userId),
                new SqlParameter("@flowdirection",flowdirection)
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataTable dt = bll.ExecDatasetSql(sql, pms).Tables[0];

                return dt;
            }

        }
        public void getRolesCompanyBySQL(string userId, ref string rolesName, ref string companyName)
        {
            //根据登录用户获取用户所属公司，用户角色名称
            string sql = @"select approvalnode,companyname from Econtract_approal where useraccount=@userId";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@userId",userId)
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataTable dt = bll.ExecDatasetSql(sql, pms).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    rolesName = dt.Rows[0][0].ToString();
                    companyName = dt.Rows[0][1].ToString();
                }
            }
            //DataTable dt = GetDataTableBySql(sql, pms);

        }
        #endregion

        #region 获取表尾字符串
        public string getTableString()
        {
            string sb = @"<table class='prodetail' rules='none' cellspacing='0' border='1' align='left' height='350px'>
	      <tbody>
		<tr>
			<td style='vertical-align:top;' width='300px'>
				<p>
					{中文=收款人银行名称}:{中文:收款人银行名称}
				</p>
				<p>
					{英文=Beneficiary's Bank Name}{英文:收款人银行名称}
				</p>
			</td>
			<td style='vertical-align:top;' width='300px'>
				<p>
					<span style='font-size:10.0pt;'></span> 
				</p>
				<p>
					{中文=社会信用代码}:{中文:社会信用代码}
				</p>
				<p>
					{英文=Swift Code:}{英文:社会信用代码}
				</p>
			</td>
		</tr>
		<tr>
			<td width='300px'>
				<p>
					{中文=收款人银行地址}:{中文:收款人银行地址}
				</p>
				<p>
					{英文=Beneficiary's Bank Address}{英文:收款人银行地址}
				</p>
			</td>
			<td style='vertical-align:top;' width='300px'>
				<p>
					{中文=客户地址及电话}:{中文:客户地址及电话}
				</p>
				<p>
					{英文=Customer's Address And Phone:}{英文:客户地址及电话}
				</p>
			</td>
		</tr>
		<tr>
			<td style='vertical-align:top;' width='300px'>
				<p>
					{中文=收款人银行行号}:{中文:收款人银行行号}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				</p>
				<p>
					{英文=Beneficiary's Bank Code}{英文:收款人银行行号}&nbsp;&nbsp;
				</p>
			</td>
			<td style='vertical-align:top;' width='300px'>
				<p>
					{中文=开户银行}:{中文:开户银行}
				</p>
				<p>
					{英文=Deposit's Bank :}{英文:开户银行}
				</p>
			</td>
		</tr>
		<tr>
			<td style='vertical-align:top;'>
				<p>
					{中文=收款人银行账号}:{中文:收款人银行账号}
				</p>
				<p>
					{英文=Beneficiary's Bank Account:}{英文:收款人银行账号}
				</p>
			</td>
			<td style='vertical-align:top;background-color:#FFFFFF;'>
				<p>
					{中文=开户银行账号}:{中文:开户银行账号}
				</p>
				<p>
					{英文=Deposit's Bank Account:}{英文:开户银行号}
				</p>
			</td>
		</tr>
	</tbody>
</table>";
            return sb;
        }
        #endregion

        #region 删除合同
        public void deleteEcontract(string contractNo)
        {
           int r= DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT, "contractNo", contractNo);
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_AP, "contractNo", contractNo);
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_TEMPLATE, "contractNo", contractNo);

        }
        #endregion

        #region 删除物流合同
        public void deleteEcontract_logistics(string logisticsContractNo)
        {
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_LOGISTICS, "logisticsContractNo", logisticsContractNo);
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_LOGISTICSFIRSTITEM, "logisticsContractNo", logisticsContractNo);
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_LOGISTICSITEMS, "logisticsContractNo", logisticsContractNo);

        }
        #endregion

        #region 删除管理合同
        public void deleteManageContract(string contractNo)
        {
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_LOGISTICS, "contractNo", contractNo);


        }
        #endregion

        #region 删除服务合同
        public void deleteServiceContract(string contractNo)
        {
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_SERVICE, "contractNo", contractNo);
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_SERVICEITEMS, "contractNo", contractNo);

        }
        #endregion

        #region 删除内部结算单合同
        public void deleteInternalContract(string contractNo)
        {
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_INTERNAL, "contractNo", contractNo);
            DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_ECONTRACT_INTERNAL_AP, "contractNo", contractNo);

        }
        #endregion
        #endregion

        #region 添加铁路发货通知
        public bool addTrainDelivery(ref string errorinfo, Hashtable ht, List<Hashtable> trainFrontierStationTable,
           List<Hashtable> trainpayCodeTable, List<Hashtable> trainProductTable,
            string contractNo, string contactContract, string createDateTag, string status, string noticeTag)
        {

            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            //获取主合同sql
            SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_TRAINDELIVERYNOTICE, "noticeTag", noticeTag, ref sqls, ref objs);

            #region 保存合同产品
            //保存合同产品
            List<Hashtable> list_hash = new List<Hashtable>();
            foreach (Hashtable hs in trainProductTable)
            {
                Hashtable htProduct = new Hashtable();
                htProduct["createDateTag"] = createDateTag;
                htProduct["noticeTag"] = noticeTag;
                htProduct["contractNo"] = contractNo;
                htProduct["pname"] = hs["pname"];
                htProduct["sendQuantity"] = hs["sendQuantity"];
                htProduct["price"] = hs["price"];
                htProduct["qunit"] = hs["qunit"];
                htProduct["priceUnit"] = hs["priceUnit"];
                htProduct["contactContract"] = contactContract;
                htProduct["pallet"] = hs["packdes"];//最小包装
                htProduct["unit"] = hs["unit"];//包装单位
                htProduct["packdes"] = hs["packing"];//包装
                htProduct["spec"] = hs["spec"];//规格
                list_hash.Add(htProduct);

            }
            SqlUtil.getBatchSqls(list_hash, ConstantUtil.TABLE_TRAINDILIVERYPRODUCT, "noticeTag", noticeTag, ref sqls, ref objs);
            #endregion

            #region 保存过境口岸表
            list_hash.Clear();
            foreach (Hashtable hs in trainFrontierStationTable)
            {
                Hashtable htFrontier = new Hashtable();
                htFrontier["noticeTag"] = noticeTag;
                htFrontier["createDateTag"] = createDateTag;
                htFrontier["contractNo"] = contractNo;
                htFrontier["frontierStationCode"] = hs["frontierStationCode"];
                htFrontier["frontierStation"] = hs["frontierStation"];
                htFrontier["contactContract"] = contactContract;
                htFrontier["carrier"] = hs["carrier"];
                list_hash.Add(htFrontier);
            }
            SqlUtil.getBatchSqls(list_hash, ConstantUtil.TABLE_TRAINFROSTATION, "noticeTag", noticeTag, ref sqls, ref objs);
            list_hash.Clear();
            #endregion

            #region 保存付费代码表
            foreach (Hashtable hs in trainpayCodeTable)
            {
                Hashtable htPayCode = new Hashtable();
                htPayCode["noticeTag"] = noticeTag;
                htPayCode["createDateTag"] = createDateTag;
                htPayCode["contractNo"] = contractNo;
                htPayCode["payCode"] = hs["payCode"];
                //htPayCode["transitPayCode"] = hs["transitPayCode"];
                //htPayCode["transitPayCode2"] = hs["transitPayCode2"];
                //htPayCode["transitPayCode3"] = hs["transitPayCode3"];
                //htPayCode["transitPayCode4"] = hs["transitPayCode4"];
                htPayCode["containerSize"] = hs["containerSize"];
                htPayCode["carriageNumber"] = hs["carriageNumber"];
                htPayCode["productWeight"] = hs["productWeight"];
                htPayCode["grossWeight"] = hs["grossWeight"];
                htPayCode["contactContract"] = contactContract;
                list_hash.Add(htPayCode);

            }
            //先删除后添加
            SqlUtil.getBatchSqls(list_hash, ConstantUtil.TABLE_TRAINDELPAYCODE, "noticeTag", noticeTag, ref sqls, ref objs);
            #endregion

            #region 更改发货申请列表发货状态为完成
            if (status == "1")//提交时更改发货申请状态
            {
                //校验发货申请中的产品数量与已做发货通知的产品数量，显示部分发货，发货完成
                string applystatus = string.Empty;
                validateProductCount(createDateTag, trainProductTable, ref applystatus);
                Hashtable ht_send = new Hashtable();
                ht_send.Add("applystatus", applystatus);
                ht_send.Add("savestatus", "1");
                ht_send.Add("createDateTag", createDateTag);
                SqlUtil.getBatchSqls(ht_send, ConstantUtil.TABLE_SENDOUTAPPDETAILS, "createDateTag", createDateTag, ref sqls, ref objs);
            }
            else
            {
                Hashtable ht_send = new Hashtable();
                ht_send.Add("savestatus", "0");
                ht_send.Add("createDateTag", createDateTag);
                SqlUtil.getBatchSqls(ht_send, ConstantUtil.TABLE_SENDOUTAPPDETAILS, "createDateTag", createDateTag, ref sqls, ref objs);
            }

            #endregion

            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? true : false;
        }



        #region 校验发货申请中的产品数量与已做发货通知的产品数量，显示部分发货，发货完成
        private void validateProductCount(string createDateTag, List<Hashtable> trainProductTable, ref string applystatus)
        {
            foreach (Hashtable hs in trainProductTable)
            {
                string pname = hs["pname"].ToString();
                //获取发货申请表中产品数量
                StringBuilder sb = new StringBuilder(string.Format(@"select quantity from {0} where createDateTag=@createDateTag and pname=@pname", ConstantUtil.TABLE_SENDOUTAPPDETAILS));
                string sendQuantity = DataFactory.SqlDataBase().getString(sb, new SqlParam[2] { new SqlParam("@createDateTag", createDateTag), new SqlParam("@pname", pname) }, "quantity");
                //获取发货通知表中产品的数量，和本次提交的数量
                StringBuilder sbNotice = new StringBuilder();
                sbNotice.AppendFormat(@"select sum(sendQuantity) as noticeQuantity from {0} where createDateTag=@createDateTag", ConstantUtil.TABLE_TRAINDILIVERYPRODUCT);
                string noticeQuantity = DataFactory.SqlDataBase().getString(sbNotice, new SqlParam[2] { new SqlParam("@createDateTag", createDateTag), new SqlParam("@pname", pname) }, "sendQuantity");
                string thisQuantity = hs["sendQuantity"].ToString();
                if (Convert.ToDecimal(sendQuantity) == Convert.ToDecimal(noticeQuantity) + Convert.ToDecimal(thisQuantity))
                {
                    applystatus = "1";//发货完成
                }
                else
                {
                    applystatus = "2";//部分发货
                }



            }


        }
        #endregion
        public bool editTrainDelivery(ref string errorinfo, Hashtable ht, List<Hashtable> trainFrontierStationTable,
       List<Hashtable> trainpayCodeTable, List<Hashtable> trainProductTable, string contractNo, string contactContract, string createDateTag)
        {
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    int r = DataFactory.SqlDataBase().UpdateByHashtable3(ConstantUtil.TABLE_TRAINDELIVERYNOTICE, "id", ht["id"].ToString(), ht);
                    if (r > 0)
                    {
                        //保存合同产品
                        DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_TRAINDILIVERYPRODUCT, "contractNo", contractNo);
                        foreach (Hashtable hs in trainProductTable)
                        {
                            Hashtable htProduct = new Hashtable();
                            htProduct["createDateTag"] = createDateTag;
                            htProduct["contractNo"] = contractNo;
                            htProduct["pname"] = hs["pname"];
                            htProduct["sendQuantity"] = hs["sendQuantity"];
                            htProduct["price"] = hs["price"];
                            htProduct["qunit"] = hs["qunit"];
                            htProduct["priceUnit"] = hs["priceUnit"];
                            htProduct["contactContract"] = contactContract;
                            htProduct["pallet"] = hs["packdes"];//最小包装
                            htProduct["unit"] = hs["unit"];//包装单位
                            htProduct["packdes"] = hs["packing"];//包装
                            htProduct["spec"] = hs["spec"];//规格

                            DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_TRAINDILIVERYPRODUCT, htProduct);

                        }
                        //保存国境口岸站表
                        DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_TRAINFROSTATION, "contractNo", contractNo);
                        foreach (Hashtable hs in trainFrontierStationTable)
                        {

                            Hashtable htFrontier = new Hashtable();
                            htFrontier["createDateTag"] = createDateTag;
                            htFrontier["contractNo"] = contractNo;
                            htFrontier["frontierStationCode"] = hs["frontierStationCode"];
                            htFrontier["frontierStation"] = hs["frontierStation"];
                            htFrontier["contactContract"] = contactContract;

                            DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_TRAINFROSTATION, htFrontier);

                        }
                        //保存付费代码表
                        DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_TRAINDELPAYCODE, "contractNo", contractNo);
                        foreach (Hashtable hs in trainpayCodeTable)
                        {
                            Hashtable htPayCode = new Hashtable();
                            htPayCode["createDateTag"] = createDateTag;
                            htPayCode["contractNo"] = contractNo;
                            htPayCode["payCode"] = hs["payCode"];
                            htPayCode["transitPayCode"] = hs["transitPayCode"];
                            htPayCode["containerSize"] = hs["containerSize"];
                            htPayCode["carriageNumber"] = hs["carriageNumber"];
                            htPayCode["productWeight"] = hs["productWeight"];
                            htPayCode["contactContract"] = contactContract;
                            DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_TRAINDELPAYCODE, htPayCode);
                        }
                    }

                    //更改发货申请列表发货状态为完成
                    string sql = "update SendoutAppDetails set applystatus=1 where createDateTag=@createDateTag";
                    SqlParameter[] pms = new SqlParameter[]{
                        new SqlParameter("@createDateTag",createDateTag)
                    };
                    int q = bll.ExecuteNonQuery(sql, pms);
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

        #endregion

        #region 其他
        #region 获取合同审核人
        public string getReviewMan(string userAccount)
        {
            //获取合同审核人，比对登陆人是否为合同审核人
            string name = string.Empty;
            StringBuilder sbreview = new StringBuilder(@"select t3.UserRealName as cname,t4.LoginName as code from Tb_RolesAddUser t1 left join Tb_Roles t2 
 on t1.rolesId=t2.Id left join Com_UserInfos t3 on t3.Userid=t1.UserId left join Com_UserLogin t4 on t4.UserId=t1.UserId  where t2.RolesName='合同管理员' and t3.UserRealName=@username");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sbreview, new SqlParam[1] { new SqlParam("@username", userAccount) }, 0);
            if (dt.Rows.Count > 0)
            {
                name = dt.Rows[0]["cname"].ToString();
            }
            return name;


        }
        #endregion

        #region 获取直线经理审核人
        public string getSalesReviewMan(string userAccount)
        {
            //获取直线经理审核人，比对登陆人是否为直线经理审核人
            string name = string.Empty;
            StringBuilder sbreview = new StringBuilder(@"select t3.UserRealName as cname,t4.LoginName as code from Tb_RolesAddUser t1 left join Tb_Roles t2 
 on t1.rolesId=t2.Id left join Com_UserInfos t3 on t3.Userid=t1.UserId left join Com_UserLogin t4 on t4.UserId=t1.UserId  where t2.RolesName='直线经理' and t3.UserRealName=@username");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sbreview, new SqlParam[1] { new SqlParam("@username", userAccount) }, 0);
            if (dt.Rows.Count > 0)
            {
                name = dt.Rows[0]["cname"].ToString();
            }
            return name;


        }
        #endregion

        #region 获取数据字典中的英文俄文信息
        //public string getEngRussInfo(string parentId, string columnName)
        //{
        //    StringBuilder sb = new StringBuilder(@"select * from BASE_DICTIONARY where PARENTID=@parentId");
        //    return DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@parentId", parentId) }, columnName);
        //}
        public string getEngRussInfo(string name, string columnName)
        {
            StringBuilder sb = new StringBuilder(@"select * from BASE_DICTIONARY where NAME=@name");
            return DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@name", name) }, columnName);
        }
        public string getEngRussInfo1(int parentId, string name, string columnName)
        {
            StringBuilder sb = new StringBuilder(@"select * from BASE_DICTIONARY where NAME=@name and PARENTID=@parentId");
            return DataFactory.SqlDataBase().getString(sb, new SqlParam[2] { new SqlParam("@name", name), new SqlParam("@parentId", parentId) }, columnName);
        }
        public string getEngRussHarbor(string name, string columnName)
        {
            StringBuilder sb = new StringBuilder(@"select * from bharbor where name=@name");
            return DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@name", name) }, columnName);
        }
        #endregion

        #region 获取字符串中某个元素的个数，进行替换操作
        public string getCountInstead(string sb, string language, string transport)
        {
            int r = Regex.Matches(sb.ToString(), transport).Count;
            string tran = string.Empty;
            if (r > 1)
            {
                if (language.Contains("中-英"))
                {
                    tran = getEngRussInfo("14", "ENGLISH");
                }
            }
            return tran;
        }
        #endregion


        #region  获取模板变量的中英文
        public void getEngRusSting(string transport, string harborout, string harborarrive, string shipment, string placement, string pricement1, string pricement2
        , string paymentType, ref string transporteng, ref string transportrus, ref string harborouteng, ref string harboroutrus, ref string harborarriveeng,
        ref string harborarriverus, ref string shipmenteng, ref string shipmengrus, ref string placementeng, ref string placementrus,
        ref string pricement1eng, ref string pricement1rus, ref string pricement2eng, ref string pricement2rus, ref string paymentTypeeng, ref string paymentTyperus)
        {
            transporteng = getEngRussInfo(transport, "ENGLISH");
            transportrus = getEngRussInfo(transport, "RUSSIAN");
            harborouteng = getEngRussHarbor(harborout, "egname");
            harboroutrus = getEngRussHarbor(harborout, "runame");
            harborarriveeng = getEngRussHarbor(harborarrive, "egname");
            harborarriverus = getEngRussHarbor(harborarrive, "runame");
            shipmenteng = getEngRussInfo(shipment, "ENGLISH");
            shipmengrus = getEngRussInfo(shipment, "RUSSIAN");
            pricement1eng = getEngRussInfo1(ConstantUtil.PRICEMENTCODE, pricement1, "ENGLISH");
            pricement1rus = getEngRussInfo1(ConstantUtil.PRICEMENTCODE, pricement1, "RUSSIAN");
            pricement2eng = getEngRussInfo1(ConstantUtil.PRICEMENTCODE, pricement2, "ENGLISH");
            pricement2rus = getEngRussInfo1(ConstantUtil.PRICEMENTCODE, pricement2, "RUSSIAN");
            paymentTypeeng = getEngRussInfo1(ConstantUtil.PAYMENTCODE, paymentType, "ENGLISH");
            paymentTyperus = getEngRussInfo1(ConstantUtil.PAYMENTCODE, paymentType, "RUSSIAN");
        }

        #endregion

        #region 替换币制
        public string getCurrency(string currency, string amount)
        {
            string recurrency = string.Empty;
            switch (currency)
            {
                case ConstantUtil.CURRENCY_CNY://人民币
                    recurrency = amount;
                    break;
                case ConstantUtil.CURRENCY_UCD:
                    amount = amount.Replace("元", "美元");
                    recurrency = amount;
                    break;
                case ConstantUtil.CURRENCY_EUR:
                    amount = amount.Replace("元", "欧元");
                    recurrency = amount;
                    break;
                case ConstantUtil.CURRENCY_JPY:
                    amount = amount.Replace("元", "日元");
                    recurrency = amount;
                    break;
                case ConstantUtil.CURRENCY_GBP:
                    amount = amount.Replace("元", "英镑");
                    recurrency = amount;
                    break;
                case ConstantUtil.CURRENCY_KRW:
                    amount = amount.Replace("元", "韩元");
                    recurrency = amount;

                    break;
                case ConstantUtil.CURRENCY_HKD:
                    amount = amount.Replace("元", "港元");
                    recurrency = amount;

                    break;
                case ConstantUtil.CURRENCY_AUD:
                    amount = amount.Replace("元", "澳元");
                    recurrency = amount;

                    break;
                case ConstantUtil.CURRENCY_CAD:
                    amount = amount.Replace("元", "加元");
                    recurrency = amount;

                    break;

                default:
                    break;
            }
            return recurrency;
        }
        #endregion

        #region 获取审核通过签名图片
        public string getSignedPng(string contractNo)
        {

            HttpContext context = HttpContext.Current;
            string picPath = string.Empty;
            string url = context.Request.Url.Host + ":" + context.Request.Url.Port;
            StringBuilder sb = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK1));
            string reviewman = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");
            //string reviewstatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewstatus");
            if (!string.IsNullOrEmpty(reviewman))//审批通过
            {
                picPath = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman + ".png' alt='' border='0' width='154' height='50' />";
            }
            return picPath;

        }
        public string getSignedPng(string contractNo, ref string reviewman)
        {

            HttpContext context = HttpContext.Current;
            string picPath = string.Empty;
            string url = context.Request.Url.Host + ":" + context.Request.Url.Port;
            StringBuilder sb = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK1));
            reviewman = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");
            //string reviewstatus = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewstatus");
            if (!string.IsNullOrEmpty(reviewman))//审批通过
            {
                picPath = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman + ".png' alt='' border='0' width='154' height='50' />";
            }
            return picPath;

        }
        #endregion

        #region 内结获取审核通过签名图片
        public void getInternalSignedPng(string contractNo, ref string salesMan, ref string salesReviewMan, ref string contractManager,
            ref string financeMan)
        {
            HttpContext context = HttpContext.Current;
            string picPath = string.Empty;
            string url = context.Request.Url.Host + ":" + context.Request.Url.Port;
            //根据合同号判断创建人是否为业务处
            StringBuilder sb = new StringBuilder(@"select * from Econtract_Internal where contractNo=@contractNo");
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();
            StringBuilder sb4 = new StringBuilder();
            StringBuilder sb5 = new StringBuilder();
            string userRealName = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "createmanname");
            string status = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "status");
            if (status != ConstantUtil.STATUS_STOCKIN_CHECK1)//不为审批通过，返回
            {
                return;
            }
            bool b = confirmAngency(ConstantUtil.ORG_JOBMAN, userRealName);
            if (b)//为业务处人员创建,承办人,合同专管员为创建人，承办部门负责人为业务处主管
            {
                sb1 = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and status='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_SUBMIT));//提交
                sb2 = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK4));//待财务负责人审核
                sb3 = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK5));//待财务主管审核
                string reviewman = DataFactory.SqlDataBase().getString(sb1, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");
                string reviewman2 = DataFactory.SqlDataBase().getString(sb2, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");
                string reviewman3 = DataFactory.SqlDataBase().getString(sb3, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");
                salesMan = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman + ".png' alt='' border='0' width='120' height='30' />";
                contractManager = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman + ".png' alt='' border='0' width='120' height='30' />";
                salesReviewMan = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman2 + ".png' alt='' border='0' width='120' height='30' />";
                financeMan = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman3 + ".png' alt='' border='0' width='120' height='30' />";
            }
            else//为业务员创建，承办人为创建人，承办负责人为直线经理，合同管理员为合同管理员，财务部门为财务负责人
            {
                sb1 = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK));//待直线经理审核
                sb2 = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK2));//待合同管理员审核
                sb3 = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK3));//待业务处主管审核
                //sb4 = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK4));//待财务负责人审核
                sb4 = new StringBuilder(string.Format(@"select * from {0} where contractNo=@contractNo and reviewstatus='{1}'", ConstantUtil.TABLE_REVIEWDATA, ConstantUtil.STATUS_STOCKIN_CHECK5));//待财务主管审核
                string reviewman4 = DataFactory.SqlDataBase().getString(sb1, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");//业务员
                string reviewman5 = DataFactory.SqlDataBase().getString(sb2, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");//直线经理
                string reviewman6 = DataFactory.SqlDataBase().getString(sb3, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");//合同管理员
                string reviewman7 = DataFactory.SqlDataBase().getString(sb4, new SqlParam[1] { new SqlParam("@contractNo", contractNo) }, "reviewman");//财务负责人
                salesMan = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman4 + ".png' alt='' border='0' width='154' height='50' />";
                contractManager = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman6 + ".png' alt='' border='0' width='154' height='50' />";
                salesReviewMan = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman5 + ".png' alt='' border='0' width='154' height='50' />";
                financeMan = "<img src='http://" + url + "/Themes/Images/signs/" + reviewman7 + ".png' alt='' border='0' width='154' height='50' />";
            }


        }
        #endregion
        #endregion

        #region 判断登录用户所处部门
        public bool confirmAngency(string angency, string userRealName)
        {
            bool b = false;

            StringBuilder sb = new StringBuilder(@"select Agency from Com_Organization where Id = 
                             (select OrgId from Com_OrgAddUser where UserId=(select Userid from Com_UserInfos where UserRealName=@userRealName))");

            string angencyName = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@userRealName", userRealName) }, "Agency");
            if (string.Equals(angency, angencyName))
            {
                b = true;
            }
            return b;

        }
        //加载登录用户所处部门
        public string loadAngency(string userRealName)
        {

            StringBuilder sb = new StringBuilder(@"select Agency from Com_Organization where Id = 
                             (select OrgId from Com_OrgAddUser where UserId=(select Userid from Com_UserInfos where UserRealName=@userRealName))");

            string angencyName = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@userRealName", userRealName) }, "Agency");
            return angencyName;

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
        //根据用户名判断用户所属角色
        public string getLoginRoles(string userAccount)
        {
            StringBuilder sb = new StringBuilder(@"select t2.rolesName from Tb_RolesAddUser t1 left join Tb_Roles t2 
 on t1.rolesId=t2.Id left join Com_UserInfos t3 on t3.Userid=t1.UserId left join Com_UserLogin t4 on t4.UserId=t1.UserId  where  t4.LoginName=@user");
            string roles = DataFactory.SqlDataBase().getString(sb, new SqlParam[1] { new SqlParam("@user", userAccount) }, "rolesName");
            return roles;
        }
        #endregion

        #region 返回两个数的最大值
        private int getMaxNumber(params int[] arrs)
        {
            int max = arrs[0];
            for (int i = 1; i < arrs.Length; i++)
            {
                if (max < arrs[i])
                {
                    max = arrs[i];
                }
            }
            return max;
        }
        #endregion

        #region 获取合同审核中其他审核的审核条件
        private string getStrWhere(string userAccount,ref string  reviewstatus)
        {
            string strWhere = string.Empty;
            string roles = getLoginRoles(userAccount);
            switch (roles)
            {
                case "业务处主管":
                    reviewstatus = "待业务处主管审核";
                    strWhere = string.Format("  and (t1.status ='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}' or t1.status='{4}') ", ConstantUtil.STATUS_STOCKIN_CHECK3, ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                    break;
                case "财务负责人":
                    reviewstatus = "待财务负责人审核";
                    strWhere = string.Format("  and (t1.status ='{0}' or t1.status='{1}' or t1.status='{2}' or t1.status='{3}') ",  ConstantUtil.STATUS_STOCKIN_CHECK4, ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                    break;
                case "财务主管":
                    reviewstatus = "待财务主管审核";
                    strWhere = string.Format("  and (t1.status ='{0}' or t1.status='{1}' or t1.status='{2}' ) ",  ConstantUtil.STATUS_STOCKIN_CHECK5, ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                    break;
                case "董事长":
                    reviewstatus = "待董事长审核";
                    strWhere = string.Format("  and (t1.status ='{0}' or t1.status='{1}') ",  ConstantUtil.STATUS_STOCKIN_CHECK6, ConstantUtil.STATUS_STOCKIN_CHECK1);
                    break;
           
                default:
                    break;
            }
            return strWhere;
        }
        #endregion

   

    }
}

