using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using WZX.Busines.IDAO;
//using WZX.Common.DotNetCode;
using RM.Common.DotNetCode;
using System.Data;
using WZX.Busines.Util;
using WZX.Busines.IDAO;
using RM.Busines;


namespace WZX.Busines.DAL
{
    public class HBIS_Income_Dal:HBIS_Income_IDAO
    {
        /// <summary>
        /// 收入导入记录列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetImpRecordList(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_INCOMEIMP_RECORD);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "IMPTIME", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 收入导入记录明细列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetImpRecordListD(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_INCOMEIMP_RECORD_D);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "NUMBER", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 收入预测列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetIncomeForecastList(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_INCOME_FORECAST_WEEK);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "NUMBER", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 收入预测明细列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetIncomeForecastListD(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_INCOMEIMP_RECORD);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "NUMBER", "Desc", pageIndex, pageSize, ref count);
        }
    }
}
