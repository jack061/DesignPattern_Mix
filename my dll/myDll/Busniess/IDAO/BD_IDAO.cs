using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.IDAO
{
    public interface BD_IDAO
    {
        DataTable GetSuppliersPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        DataTable GetDictionary(String strtable);
        DataTable GetContact(string supcode);
        DataTable GetFacContact(string fcode);
        int UpdateContact(String supcode, DataTable dtContact);
        int UpdateFacContact(String fcode, DataTable dtContact);
        int Virtualdelete(string keyfield, string keyvalue, string tablename);
        DataTable GetDicdata(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        DataTable GetProductList(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        DataTable GetProducthssList(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        DataTable GetFactoryList(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        DataTable GetProduct(String pcode);
        DataTable GetFactory(String pcode);


    }
}
