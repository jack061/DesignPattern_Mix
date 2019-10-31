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
    public class TrainApplyI_Dal
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
        public DataTable GetSubmitPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from "+ConstantUtil.TABLE_TRAINAPPLYINEW+" where 1=1 ");//TrainapplyINew
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "createdate", "Desc", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 请车单
        /// </summary>
        /// <param name="payNo">申请单号</param>
        /// <returns></returns>
        public DataTable GetTrainApplyI(String applyNo) 
        {
            DataTable dt;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from TrainapplyI where applyNo=@applyNo");
            SqlParam[] para = { new SqlParam("@applyNo", applyNo) };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, para);
            return dt;
        }
        /// <summary>
        /// 中泰请车计划列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetSumPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from " + ConstantUtil.TABLE_TRAINAPPLYISUM + " where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "applyNo", "desc", pageIndex, pageSize, ref count);
        }


        public DataTable GetTrainApplyIPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int page, int row, ref int count)
        {
            throw new NotImplementedException();
        }
    }
}
