using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.IDAO.TrainApply
{
    public interface TrainApplyE_IDAO
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
        DataTable GetTrainApplyIPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 请车单
        /// </summary>
        /// <param name="payNo">申请单号</param>
        /// <returns></returns>
        DataTable GetTrainApplyI(String applyNo);

        
    }
}
