using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;
using WZX.Busines.Util;

namespace RM.Busines.DAL.Inspect
{
    public class PreCheck_Dal
    {
        /// <summary>
        /// 产品预验信息
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="IList_param"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static DataTable GetProductList1(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            //select * from previewManage t1 left join previewPackManage t2 on  t1.previewCode=t2.previewMaCode
            strSql.Append(@"select * from previewManage t1 left join previewPackManage t2 on  t1.previewCode=t2.previewMaCode ");
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "previewCode", "DESC", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="IList_param"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static DataTable GetProductPreMList(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from previewManage ");
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "previewCode", "DESC", pageIndex, pageSize, ref count);
        }
        public static DataTable GetProductList(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_INSPECTIONRE);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "inspectionNo", "DESC", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 获取包装预验信息
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="IList_param"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static DataTable GetPackList(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_INSPECTIONREPACK);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "inspectionNo", "DESC", pageIndex, pageSize, ref count);
        }

    }
}
