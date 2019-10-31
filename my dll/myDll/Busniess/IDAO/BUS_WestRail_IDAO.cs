using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.IDAO
{
    public interface BUS_WestRail_IDAO
    {
        DataTable GetAboradContracts(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        DataTable GetAboradContracts(String ccode);
        DataTable GetAboradContractsAtt(String ccode, string attachmentno);
        DataTable GetAttachProduct(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        void AddProductToAttach(StringBuilder sqlExec, IList<SqlParam> lisParam);
        /// <summary>
        /// 获取合同模版下拉框数据源
        /// </summary>
        /// <param name="type">1：内容2：附件模版</param>
        /// <returns></returns>
        DataTable GetHtModle(int type);
    }
}
