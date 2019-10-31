using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.Busines.IDAO;
using System.Data;
using RM.Common.DotNetCode;
using WZX.Busines.Util;
using System.Data.SqlClient;

namespace RM.Busines.DAL
{
    public class Payment_Dal:Payment_IDAO
    {
        /// <summary>
        /// 海外付款列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetAbroadPaymentPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from PayReceiveE where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "paydate", "Desc", pageIndex, pageSize, ref count);
        }
        public DataTable GetPaymentPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from PayReceive where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "paydate", "Desc", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 海外付款子表列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetAbroadPaymentSubPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from PayReceiveEDetail where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "contractNo", "Desc", pageIndex, pageSize, ref count);
        }
        public DataTable GetPaymentSubPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from PayReceiveDetails where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "contractNo", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 海外合同列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetAbroadHTPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT a.*,b.attachmentno from " + ConstantUtil.TABLE_ECONTRACT + " a left join ");
            strSql.Append(ConstantUtil.TABLE_ECONTRACT_A);
            strSql.Append(@" b on a.contractNo = b.contractNo  where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "contractNo", "Desc", pageIndex, pageSize, ref count);
        }
        public  string GetAbroadHTPage1(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            //查询出合同总金额
            strSql.Append(@"select * from Econtract d left join (select a.contractNo,sum( b.amount) as price
from Econtract a left join Econtract_ap b on a.contractNo=b.contractNo group by a.contractNo)as v on d.contractNo=v.contractNo"); 
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            sqlcount.Append("select count(1) from Econtract t1 where " + SqlWhere.ToString());
            SqlParameter[] sqlpps = new SqlParameter[]{

            };
            RM.Busines.JsonHelperEasyUi ui = new RM.Busines.JsonHelperEasyUi();
            StringBuilder sb = ui.GetDatatablePageJsonString(strSql, sqlcount, sqlpps,"contractNo", "desc", pageIndex, pageSize);
          return sb.ToString();
        }

        /// <summary>
        /// 海外付款
        /// </summary>
        /// <param name="payNo">付款单号</param>
        /// <returns></returns>
        public DataTable GetAbroadPayment(String payNo) 
        {
            DataTable dt;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from PayReceiveE where payNo=@payNo");
            SqlParam[] para = { new SqlParam("@payNo", payNo) };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, para);
            return dt;
        }

        /// <summary>
        /// 境内付款列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetDomesticPaymentPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from PayReceiveI where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "paydate", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 境内付款子表列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetDomesticPaymentSubPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from PayReceiveIDetail where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "contractNo", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 境内付款
        /// </summary>
        /// <param name="payNo">付款单号</param>
        /// <returns></returns>
        public DataTable GetDomesticPayment(String payNo) 
        {
            DataTable dt;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from PayReceiveI where payNo=@payNo");
            SqlParam[] para = { new SqlParam("@payNo", payNo) };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, para);
            return dt;
        }
    }
}
