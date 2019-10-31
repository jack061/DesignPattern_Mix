using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.Busines.DAL
{
    public class TempldateDB:RM.Busines.IDAO.ITemplateDB
    {

        System.Data.DataTable IDAO.ITemplateDB.GetTemplateContractPage(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select t1.*,t2.cname as lanname from btemplate_contract t1 left join  bdicdate t2 on t1.language=t2.code ");
            strSql.Append(" where t1.status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere); 
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "templatetype,templateno,sortno", "asc", pageIndex, pageSize, ref count);
        }

        System.Data.DataTable IDAO.ITemplateDB.GetTemplateAttachPage(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select t1.*,t2.cname as lanname from btemplate_attach  t1 left join  bdicdate t2 on t1.language=t2.code  ");
            strSql.Append(" where t1.status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "templateno,sortno", "asc", pageIndex, pageSize, ref count);
        }

        System.Data.DataTable IDAO.ITemplateDB.GetTemplateContract(string id)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from btemplate_contract where id=@id  ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] 
            { 
                new Common.DotNetCode.SqlParam("@id", id),
            });
        }

        System.Data.DataTable IDAO.ITemplateDB.GetTemplateContractByNo(string templateno)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from btemplate_contract where templateno=@templateno  ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] 
            { 
                new Common.DotNetCode.SqlParam("@templateno", templateno),
            });
        }

        System.Data.DataTable IDAO.ITemplateDB.GetTemplateAttach(string id)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from btemplate_attach where id=@id ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] 
            { 
                new Common.DotNetCode.SqlParam("@id", id),
            });
        }
    }
}
