using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.Busines.IDAO;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.DAL
{
    public class Check_Dal:Check_IDAO
    {
        /// <summary>
        /// 预检列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetPreCheckPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from InspectionRe where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "inspecdate", "Desc", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 预检列表明细，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetPreCheckPage_D(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from InspectionReProduct where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "inspectionNo", "Desc", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 预检
        /// </summary>
        /// <param name="inspectionNo">预检单号</param>
        /// <returns></returns>
        public  DataTable GetPreCheck(String inspectionNo) 
        {
            DataTable dt;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from InspectionRe where inspectionNo=@inspectionNo");
            SqlParam[] para = { new SqlParam("@inspectionNo", inspectionNo) };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, para);
            return dt;
        }

        /// <summary>
        /// 商检列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetBusinessCheckPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from Inspection where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "inspecdate", "Desc", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 商检列表明细，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetBusinessCheckPage_D(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from InspectionProduct where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "inspectionNo", "Desc", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 商检
        /// </summary>
        /// <param name="inspectionNo">商检单号</param>
        /// <returns></returns>
        public  DataTable GetBusinessCheck(String inspectionNo) 
        {
            DataTable dt;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from Inspection where inspectionNo=@inspectionNo");
            SqlParam[] para = { new SqlParam("@inspectionNo", inspectionNo) };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, para);
            return dt;
        }
    }
}
