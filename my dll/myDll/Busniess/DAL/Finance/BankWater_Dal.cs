using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;
using WZX.Busines.Util;

namespace RM.Busines.DAL.Finance
{
    public class BankWater_Dal
    {
        #region 银行流水

        /// <summary>
        /// 银行流水列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public static DataTable GetBankWaterListPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_BANK_WATER);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "docNo", "DESC", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 银行流水列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public static DataTable GetBankWaterListPage_D(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_BANK_WATER_D);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "docNo", "DESC", pageIndex, pageSize, ref count);
        }

        #endregion
    }
}
