using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.DAL
{
    /// <summary>
    /// 西出铁路数据库操作类
    /// </summary>
    public class Bus_WestRail_Dal:IDAO.BUS_WestRail_IDAO
    {
        public DataTable GetAboradContracts(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select t1.*,case when t1.check1=1 then '已审核' else '未审核' end as checkname1,case when t1.check2=1 then '已审核' else '未审核' end as checkname2,t2.cname as paymentcname,t3.attachmentno from Econtract t1 left join bdicdate t2 on t1.payment=t2.code and t2.classname='付款方式' left join Econtract_a t3 on t1.contractNo=t3.contractNo");
            strSql.Append(" where t1.status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "lastmoddate", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetHomeContracts(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select t1.*,case when t1.check1=1 then '已审核' else '未审核' end as checkname1,case when t1.check2=1 then '已审核' else '未审核' end as checkname2,t2.cname as paymentcname,t3.attachmentno from Icontract t1 left join bdicdate t2 on t1.payment=t2.code and t2.classname='付款方式' left join Icontract_a t3 on t1.contractNo=t3.contractNo");
            strSql.Append(" where t1.status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "lastmoddate", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetAboradContractsAtt(String ccode, string attachmentno)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from Econtract_a where contractNo=@contractNo and attachmentno=@attachmentno ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] 
            { 
                new Common.DotNetCode.SqlParam("@contractNo", ccode), 
                new Common.DotNetCode.SqlParam("@attachmentno", attachmentno) 
            });
        }

        public DataTable GetHomeContractsAtt(String ccode, string attachmentno)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from Icontract_a where contractNo=@contractNo and attachmentno=@attachmentno ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] 
            { 
                new Common.DotNetCode.SqlParam("@contractNo", ccode), 
                new Common.DotNetCode.SqlParam("@attachmentno", attachmentno) 
            });
        }

        public DataTable GetAboradContracts(String ccode)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from Econtract where contractNo=@contractNo ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@contractNo", ccode) });
        }

        public DataTable GetHomeContracts(String ccode)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from Icontract where contractNo=@contractNo ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@contractNo", ccode) });
        }

        public DataTable GetAttachProduct(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select t1.* from Econtract_ap t1 ");
            strSql.Append(" where 1=1  ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "pcode", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetHomeAttachProduct(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select t1.* from Icontract_ap t1 ");
            strSql.Append(" where 1=1  ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "pcode", "Desc", pageIndex, pageSize, ref count);
        }

        public void AddProductToAttach(StringBuilder sqlExec,IList<SqlParam> lisParam)
        {
            DataFactory.SqlDataBase().ExecuteBySql(sqlExec, lisParam.ToArray());
        }

        public DataTable GetHtModle(int type)
        {
            if (type == 1)
            {
                //获取模版列表
                StringBuilder sb = new StringBuilder(String.Empty);
                sb.Append("select  templatetype+'---'+templatename as tempname,templateno from btemplate_contract ");
                return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { });
            }
            else if(type==2)
            {
                StringBuilder sb = new StringBuilder(String.Empty);
                sb.Append("select templatename as tempname,templateno from dbo.btemplate_attach ");
                return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { });
            }
            return null;
        }
    }
}
