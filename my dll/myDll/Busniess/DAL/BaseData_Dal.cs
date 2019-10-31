using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.Busines.IDAO;
using System.Data;
using RM.Common.DotNetCode;
using WZX.Busines.Util;

namespace RM.Busines.DAL
{
    public class BaseData_Dal
    {
        /// <summary>
        /// 港口列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetHarborPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from bharbor where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "createdate", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 获取列表 ，不分页
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="isroot">是否只获取根节点</param>
        /// <returns></returns>
        public static DataTable GetHarborList(StringBuilder SqlWhere, IList<SqlParam> IList_param)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from bharbor where 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, IList_param.ToArray());
        }

        /// <summary>
        /// 获取铁路发货人资质
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetRailShipperPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from "+ConstantUtil.TABLE_BRAILSHIPPERQUALI+" where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "ID", "Desc", pageIndex, pageSize, ref count);
        }
        
        /// <summary>
        /// 港口
        /// </summary>
        /// <param name="payNo">港口编号</param>
        /// <returns></returns>
        public DataTable GetHarborOne(String code) 
        {
            DataTable dt;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from bharbor where code=@code");
            SqlParam[] para = { new SqlParam("@code", code) };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, para);
            return dt;
        }

        /// <summary>
        /// 工厂产品列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetProductPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from bproduct where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "createdate", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 产品销售指导价格列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetProductPricePage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from bproductprice where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "createdate", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 汇率列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetRatePage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from brate where 1=1 ");
            strSql.Append(SqlWhere);
            strSql.Append(" ");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "createdate", "Desc", pageIndex, pageSize, ref count);
        }

        #region 数据字典操作

        /// <summary>
        /// 数据字典列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <param name="isroot">是否只获取根节点</param>
        /// <returns></returns>
        public static DataTable GetDicListPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_DICTRONARY);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "ID", "DESC", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 获取列表 ，不分页
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="isroot">是否只获取根节点</param>
        /// <returns></returns>
        public static DataTable GetDicList(StringBuilder SqlWhere, IList<SqlParam> IList_param)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_DICTRONARY);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, IList_param.ToArray());
        }
        /// <summary>
        /// 获取仓库关联组织 ，不分页
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="isroot">是否只获取根节点</param>
        /// <returns></returns>
        public static DataTable GetWarehouseList(StringBuilder SqlWhere, IList<SqlParam> IList_param)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from View_ORG_USER  where PARENTID='68'");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, IList_param.ToArray());
        }

        /// <summary>
        /// 获取数据字典列表（原）
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="IList_param"></param>
        /// <returns></returns>
        public static DataTable GetDicList_1(StringBuilder SqlWhere, IList<SqlParam> IList_param)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_DICTRONARY_1);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, IList_param.ToArray());
        }

        #endregion

        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="IList_param"></param>
        /// <returns></returns>
        public static DataTable GetCustomerList(StringBuilder SqlWhere, IList<SqlParam> IList_param)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM ");
            strSql.Append(ConstantUtil.TABLE_CUSTOMER);
            strSql.Append(@" WHERE 1=1 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, IList_param.ToArray());
        }
    }
}
