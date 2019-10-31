using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.Busines.IDAO;
using System.Data;
using RM.Common.DotNetCode;
using WZX.Busines.Util;

namespace RM.Busines.DAL.TrainApply
{
    public class TrainApplyE_Dal
    {
        /// <summary>
        /// 中泰请车计划列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public static DataTable GetTrainApplyPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from TrainapplyE where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "applyNo", "Desc", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 请车单
        /// </summary>
        /// <param name="payNo">申请单号</param>
        /// <returns></returns>
        public static DataTable GetTrainApply(String applyNo) 
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from TrainapplyE where applyNo=@applyNo");
            SqlParam[] para = { new SqlParam("@applyNo", applyNo) };
            return DataFactory.SqlDataBase().GetDataTableBySQL(sql, para);
        }


        public static DataTable GetSubList( string table,string applyNo) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from " + table + " where applyNo=@applyNo");
            SqlParam[] para = { new SqlParam("@applyNo", applyNo) };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql,para);
        }
        
    }
}
