using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WZX.Busines.IDAO;
using System.Data;
using RM.Common.DotNetCode;
using WZX.Busines.Util;
using RM.Busines;

namespace WZX.Busines.DAL
{
    /// <summary>
    /// 基础数据的数据层处理
    /// </summary>
    public class HBIS_Dal:HBIS_IDAO
    {
        #region 物料管理
        /// <summary>
        /// 物料列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetMaterialInfoPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_MATERIAL_BASE);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "NUMBER", "ASC", pageIndex, pageSize, ref count);
        }
        #endregion
    }
}
