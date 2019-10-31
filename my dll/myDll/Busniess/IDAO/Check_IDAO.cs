using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.IDAO
{
    public interface Check_IDAO
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
        DataTable GetPreCheckPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);

         /// <summary>
        /// 预检列表明细，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        DataTable GetPreCheckPage_D(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 预检
        /// </summary>
        /// <param name="inspectionNo">预检单号</param>
        /// <returns></returns>
        DataTable GetPreCheck(String inspectionNo);

        /// <summary>
        /// 商检列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        DataTable GetBusinessCheckPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);

        /// <summary>
        /// 商检列表明细，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        DataTable GetBusinessCheckPage_D(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 商检
        /// </summary>
        /// <param name="inspectionNo">商检单号</param>
        /// <returns></returns>
        DataTable GetBusinessCheck(String inspectionNo);
    }
}
