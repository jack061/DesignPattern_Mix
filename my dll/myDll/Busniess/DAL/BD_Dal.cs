using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.DAL
{
    public class BD_Dal:IDAO.BD_IDAO
    {
        public DataTable GetSuppliersPage(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select bcustomer.*,t2.UserRealName as user_name,t3.UserRealName as  modname from bcustomer 
left join Com_UserInfos t2 on bcustomer.createman= t2.Userid
left join Com_UserInfos t3 on bcustomer.lastmod= t3.Userid  ");
            strSql.Append(" where status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "lastmoddate", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 客户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustormerComboxData()
        {
            DataTable dt = new DataTable();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(" select code,shortname,name from bcustomer ");
                dt = ds.Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// 供应商下拉列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSupplierComboxData()
        {
            DataTable dt = new DataTable();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(" select code,shortname,name from bsupplier ");
                dt = ds.Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// 供应商列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSupplyComboxData()
        {
            DataTable dt = new DataTable();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(" select code,shortname,name from bcustomer ");
                dt = ds.Tables[0];
            }

            return dt;
        }

        public DataTable GetDicdata(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select * from bdicdate  ");
            strSql.Append(" where status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "classname", "Desc", pageIndex, pageSize, ref count);
        }
        //新增查找客户管理--收货信息表中的信息
        public DataTable GetCustormerDeliveryList(string code)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bcustomer_delivery where code=@code ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@code", code) });
        }

        public DataTable GetCustomerDeliveryPager(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select * from bcustomer_delivery  ");
            strSql.Append(" where 1=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "code", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetDictionary(String strtable)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select code,cname from bdicdate where classname='" + strtable + "' and status=1 ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb);
        }

        public DataTable GetContact(string cuscode)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bcustomer_contact where code=@code ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@code", cuscode) });
        }
        public DataTable GetCustormerDelivery(string id)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bcustomer_delivery where id=@id ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@id", id) });
        }
        public DataTable GetSupplierDelivery(string code)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bsupplier_delivery where code=@code ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@code", code) });
        }
        public DataTable GetFacContact(string fcode)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bsupplier_contact where code=@fcode ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@fcode", fcode) });
        }

        public DataTable GetProductList(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select * from bproduct  ");
            strSql.Append(" where status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "pcode", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetProducthssList(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select * from bproducthss  ");
            strSql.Append(" where status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "pcode", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetProduct(String pcode)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bproduct where pcode=@pcode ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@pcode", pcode) });
        }

        public DataTable GetProductPrice(string pcode)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bproduct_price where pcode=@pcode ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@pcode", pcode) },0);
        }

        public DataTable GetProducthss(String pcode)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bproducthss where pcode=@pcode ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@pcode", pcode) });
        }

        public DataTable GetFactoryList(StringBuilder SqlWhere, IList<Common.DotNetCode.SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"  select bsupplier.*,t2.UserRealName as user_name,t3.UserRealName as modname from bsupplier 
left join Com_UserInfos t2 on bsupplier.createman= t2.Userid
left join Com_UserInfos t3 on bsupplier.lastmod= t3.Userid  ");
            strSql.Append(" where status=1 ");
            if (SqlWhere.ToString().Trim().Length > 0)
            {
                strSql.Append(" and ");
                strSql.Append(SqlWhere);
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "lastmoddate", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetFactory(String fcode)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            sb.Append(" select * from bsupplier where code=@fcode ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(sb, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@fcode", fcode) });
        }

        /// <summary>
        /// 更新客户的联系人
        /// </summary>
        /// <param name="supcode"></param>
        /// <param name="dtContact"></param>
        /// <returns></returns>
        public int UpdateContact(String supcode,DataTable dtContact)
        {
            if (dtContact == null || dtContact.Rows.Count == 0)
            {
                return 0;
            }
            if (dtContact.Columns.Contains("code")==false)
            {
                dtContact.Columns.Add("code", typeof(String));
            }
            dtContact.TableName = "bcustomer_contact";
            dtContact.AcceptChanges();

            List<SqlParam[]> lis = new List<SqlParam[]>();
            List<StringBuilder> lisstr = new List<StringBuilder>();
            StringBuilder sb = new StringBuilder(" delete bcustomer_contact where code=@supcode ");
            lisstr.Add(sb);
            lis.Add(new SqlParam[] { new SqlParam("@supcode",supcode )});

            foreach (DataRow dr in dtContact.Rows)
            {
                sb = new StringBuilder(" insert into bcustomer_contact(language,country,city,address,linkman,phone,mail,code) values(@language,@country,@city,@address,@linkman,@phone,@mail,@supcode);");
                lisstr.Add(sb);
                lis.Add(new SqlParam[] 
                { 
                    new SqlParam("@language",dr["language"]),
                    new SqlParam("@country",dr["country"]),
                    new SqlParam("@city",dr["city"]),
                    new SqlParam("@address",dr["address"]),
                    new SqlParam("@linkman",dr["linkman"]),
                    new SqlParam("@phone",dr["phone"]),
                    new SqlParam("@mail",dr["mail"]),
                    new SqlParam("@supcode",supcode),
                });
            }

            return DataFactory.SqlDataBase().BatchExecuteBySql(lisstr.ToArray(), lis.ToArray());
        }

        /// <summary>
        /// 更新采购商的联系人
        /// </summary>
        /// <param name="supcode"></param>
        /// <param name="dtContact"></param>
        /// <returns></returns>
        public int UpdateFacContact(String fcode, DataTable dtContact)
        {
            if (dtContact == null || dtContact.Rows.Count == 0)
            {
                return 0;
            }
            if (dtContact.Columns.Contains("code") == false)
            {
                dtContact.Columns.Add("code", typeof(String));
            }
            dtContact.TableName = "bsupplier_contact";
            dtContact.AcceptChanges();

            List<SqlParam[]> lis = new List<SqlParam[]>();
            List<StringBuilder> lisstr = new List<StringBuilder>();
            StringBuilder sb = new StringBuilder(" delete bsupplier_contact where code=@supcode ");
            lisstr.Add(sb);
            lis.Add(new SqlParam[] { new SqlParam("@supcode", fcode) });

            foreach (DataRow dr in dtContact.Rows)
            {
                sb = new StringBuilder(" insert into bsupplier_contact(language,country,city,address,linkman,phone,mail,code) values(@language,@country,@city,@address,@linkman,@phone,@mail,@supcode);");
                lisstr.Add(sb);
                lis.Add(new SqlParam[] 
                { 
                    new SqlParam("@language",dr["language"]),
                    new SqlParam("@country",dr["country"]),
                    new SqlParam("@city",dr["city"]),
                    new SqlParam("@address",dr["address"]),
                    new SqlParam("@linkman",dr["linkman"]),
                    new SqlParam("@phone",dr["phone"]),
                    new SqlParam("@mail",dr["mail"]),
                    new SqlParam("@supcode",fcode),
                });
            }

            return DataFactory.SqlDataBase().BatchExecuteBySql(lisstr.ToArray(), lis.ToArray());
        }

        public int Virtualdelete(string keyfield, string keyvalue, string tablename)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format(" update {0} set status=0 where {1}=@val ", tablename, keyfield));

            SqlParam[] mms = new SqlParam[] { new SqlParam("@val",keyvalue )};

            return DataFactory.SqlDataBase().ExecuteBySql(sb, mms);
        }
    }
}
