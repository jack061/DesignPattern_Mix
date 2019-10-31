using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.IDAO
{
    public interface ITemplateDB
    {
        DataTable GetTemplateContractPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        DataTable GetTemplateAttachPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);

        DataTable GetTemplateContract(String id);
        DataTable GetTemplateAttach(String id);
        System.Data.DataTable GetTemplateContractByNo(string templateno);
    }
}
