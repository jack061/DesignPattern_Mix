using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using WZX.Busines.Util;
using RM.Common.DotNetBean;

namespace RM.Busines.Stock
{
    public class StockBLL
    {
        JsonHelperEasyUi jui = new JsonHelperEasyUi();
        public void AddModel()
        { 
            
        }

        public void EditModel()
        { 
            
        }

        public StringBuilder GetJsonStockInList(string indocno,string man,string begintime,string endtime)
        {
            StringBuilder sb = null;

            String sql = @"  select *,createmanname as man1,t2.mcode,t2.mname,t2.unit,t2.inquantity,t2.price,t2.amount  from dbo.sotckmindoc t1
 left join sotckminmatr t2 on t1.indocno=t2.indocno where 1=1  ";

            if (indocno.Length > 0)
            {
                sql += " and t1.indocno like '%'+@indocno+'%' ";
            }
            if (man.Length > 0)
            {
                sql += " and t1.createmanname like '%'+@man+'%' ";
            }
            if (begintime.Length > 0)
            {
                sql += " and t1.indate >= @begintime ";
            }
            if (endtime.Length > 0)
            {
                endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                sql += " and t1.indate <@endtime ";
            }

            sb=jui.GetDatatableJsonString(
                new StringBuilder(sql), 
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@indocno",indocno),
                    new System.Data.SqlClient.SqlParameter("@man",man),
                    new System.Data.SqlClient.SqlParameter("@begintime",begintime),
                    new System.Data.SqlClient.SqlParameter("@endtime",endtime),
                });

            return sb;
        }

        public StringBuilder GetJsonStockOutList(string indocno, string man, string begintime, string endtime)
        {
            StringBuilder sb = null;

            String sql =@" select *,createmanname as man1,t2.mcode,t2.mname,t2.unit,t2.outquantity,t2.price,t2.amount  from dbo.sotckmout t1 
  left join sotckmoutmatr t2 on t1.outdocno=t2.outdocno where 1=1 ";
            if (indocno.Length > 0)
            {
                sql += " and t1.outdocno like '%'+@outdocno+'%' ";
            }
            if (man.Length > 0)
            {
                sql += " and t1.createmanname like '%'+@man+'%' ";
            }
            if (begintime.Length > 0)
            {
                sql += " and t1.outdate >= @begintime ";
            }
            if (endtime.Length > 0)
            {
                endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                sql += " and t1.outdate <@endtime ";
            }

            sb = jui.GetDatatableJsonString(
                new StringBuilder(sql),
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@outdocno",indocno),
                    new System.Data.SqlClient.SqlParameter("@man",man),
                    new System.Data.SqlClient.SqlParameter("@begintime",begintime),
                    new System.Data.SqlClient.SqlParameter("@endtime",endtime),
                });

            return sb;
        }

        public StringBuilder GetJsonStockList()
        {
            StringBuilder sb = null;



            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.sotckmatrbatch "),
                new System.Data.SqlClient.SqlParameter[] { });

            return sb;
        }

        public StringBuilder GetRkMatrList(string indocno)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.sotckminmatr where indocno=@indocno"),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@indocno",indocno)
                });

            return sb;
        }
        /// <summary>
        /// 货物出库产品列表
        /// </summary>
        /// <param name="indocno"></param>
        /// <returns></returns>
        public StringBuilder GetCkMatrList(string outdocno)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.sotckmoutmatr where outdocno=@outdocno"),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@outdocno",outdocno)
                });

            return sb;
        }

        public StringBuilder GetRkMatrListByMcode(string mcode)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.sotckminmatr where mcode=@mcode"),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@mcode",mcode)
                });

            return sb;
        }

        public StringBuilder GetStockLog(string mcode)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder(@"select * from (
select '入库' as stocktype,t2.seller,t2.buyer,t2.confirmdate,t2.contractNo as contractno, t1.indocno as docno,t1.mname,t1.mcode,t1.mspec,t1.unit,t1.inquantity as quantity,amount,t1.remark from dbo.sotckminmatr t1  
inner join  sotckmindoc t2 on t1.indocno=t2.indocno and t2.status='确认'
union all 
select '出库' as stocktype,t2.seller,t2.buyer,t2.confirmdate,t2.contractNo as contractno, t1.outdocno as docno,t1.mname,t1.mcode,t1.mspec,t1.unit,t1.outquantity as quantity,amount,t1.remark from dbo.sotckmoutmatr t1  
inner join  sotckmout t2 on t1.outdocno=t2.outdocno and t2.status='确认')
tt where mcode=@mcode order by confirmdate desc "),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@mcode",mcode)
                });

            return sb;
        }

        /// <summary>
        /// 货物出库产品列表
        /// </summary>
        /// <param name="indocno"></param>
        /// <returns></returns>
        public StringBuilder GetCkMatrListByMcode(string mcode)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.sotckmoutmatr where mcode=@mcode"),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@mcode",mcode)
                });

            return sb;
        }

        public StringBuilder GetSelectedHtList(string contractNo,string attachmentNo,string begtime,string endtime,String bgroup,String seller,String buyer,string busman)
        {
            StringBuilder sb = null;

            string sbwhere1 = "";
            if (seller.Length > 0)
            {
                sbwhere1 += " and tt1.sellercode=@seller ";
            }
            if (buyer.Length > 0)
            {
                sbwhere1+=" and  tt1.buyercode=@buyer ";
            }
            if (busman.Length > 0)
            {
                sbwhere1 += " and tt1.salesmanCode=@busman";
            }
            String sql = @"   select * from (select tt1.contractNo,tt1.signedtime,tt1.signedplace,tt1.status,
'' as attachmentNo,businessclass,seller,buyer,currency,tt2.quantity,tt2.amount,tt2.pname from dbo.Econtract tt1
 inner join (
	select t1.pname,t1.pcode, t1.contractNo,(t1.quantity-isnull(t2.inquantity,0)) as quantity,(t1.quantity-isnull(t2.inquantity,0))*t1.price as amount
	from Econtract_ap t1
	left join 
	(
		select sotckmindoc.contractNo,mcode,sum(inquantity) as inquantity from dbo.sotckminmatr
		inner join   sotckmindoc on sotckmindoc.indocno=sotckminmatr.indocno and sotckmindoc.status in ('新建','提交','审核','确认')
		group by contractNo,mcode
	) t2 on t1.contractNo=t2.contractNo   and t1.pcode=t2.mcode 
    where (t1.quantity-isnull(t2.inquantity,0))>0) tt2 on tt1.contractNo=tt2.contractNo 
 where  tt1.contractNo like '%GY%' "+sbwhere1+@") ttt1 where 1=1 
            ";
            if (bgroup.Length > 0)
            {
                sql += " and businessclass='" + bgroup + "' ";
            }
            if (contractNo.Length > 0)
            {
                sql += " and contractNo like '%'+@contractNo+'%' ";
            }
            if (attachmentNo.Length > 0)
            {
                sql += " and attachmentno like '%'+@attachmentno+'%' ";
            }
            if (begtime.Length > 0)
            {
                sql += " and signedtime >= @begintime ";
            }
            if (endtime.Length > 0)
            {
                endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                sql += " and signedtime <@endtime ";
            }

            sb = jui.GetDatatableJsonString(
                new StringBuilder(sql),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@contractNo",contractNo),
                    new System.Data.SqlClient.SqlParameter("@attachmentno",attachmentNo),
                    new System.Data.SqlClient.SqlParameter("@begintime",begtime),
                    new System.Data.SqlClient.SqlParameter("@endtime",endtime),
                    new System.Data.SqlClient.SqlParameter("@seller",seller),
                    new System.Data.SqlClient.SqlParameter("@buyer",buyer),
                    new System.Data.SqlClient.SqlParameter("@busman",busman)
                });

            return sb;
        }

        public StringBuilder GetSelectedHtList_XS(string contractNo, string attachmentNo, string begtime, string endtime, String bgroup,string seller,string buyer,string busman)
        {
            StringBuilder sb = null;

            string sbwhere1 = " 1=1 ";
            if (seller.Length > 0)
            {
                sbwhere1 += " and tt1.sellercode=@seller ";
            }
            if (buyer.Length > 0)
            {
                sbwhere1 += " and  tt1.buyercode=@buyer ";
            }
            if (busman.Length > 0)
            {
                sbwhere1 += " and tt1.salesmanCode=@busman";
            }
            String sql = @"select * from (select tt1.contractNo,tt1.signedtime,tt1.signedplace,tt1.status,
'' as attachmentNo,businessclass,seller,buyer,currency,tt2.outquantity,tt2.amount,tt2.pname from dbo.Econtract tt1
inner join (
	select t1.contractNo,t1.pcode,t1.pname,t1.quantity-isnull(t2.outquantity,0) as outquantity,(t1.quantity-isnull(t2.outquantity,0))*t1.price as amount 
	from Econtract_ap t1
	left join 
	(
		select sotckmout.contractNo,mcode,sum(outquantity) as outquantity from dbo.sotckmoutmatr
		inner join   sotckmout on sotckmout.outdocno=sotckmoutmatr.outdocno and sotckmout.status in ('新建','提交','审核','确认')
		group by contractNo,mcode
	) t2 on t1.contractNo=t2.contractNo   and t1.pcode=t2.mcode 
    where (t1.quantity-isnull(t2.outquantity,0))>0 
 ) tt2 on tt1.contractNo=tt2.contractNo 
 where   tt1.contractNo like '%XS%' and "+sbwhere1+") ttt1 where 1=1 ";

            if (bgroup.Length > 0)
            {
                sql += " and businessclass='" + bgroup + "' ";
            }
            if (contractNo.Length > 0)
            {
                sql += " and contractNo like '%'+@contractNo+'%' ";
            }
            if (attachmentNo.Length > 0)
            {
                sql += " and attachmentno like '%'+@attachmentno+'%' ";
            }
            if (begtime.Length > 0)
            {
                sql += " and signedtime >= @begintime ";
            }
            if (endtime.Length > 0)
            {
                endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                sql += " and signedtime <@endtime ";
            }

            sb = jui.GetDatatableJsonString(
                new StringBuilder(sql),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@contractNo",contractNo),
                    new System.Data.SqlClient.SqlParameter("@attachmentno",attachmentNo),
                    new System.Data.SqlClient.SqlParameter("@begintime",begtime),
                    new System.Data.SqlClient.SqlParameter("@endtime",endtime),
                    new System.Data.SqlClient.SqlParameter("@seller",seller),
                    new System.Data.SqlClient.SqlParameter("@buyer",buyer),
                    new System.Data.SqlClient.SqlParameter("@busman",busman)
                });

            return sb;
        }

        public StringBuilder GetSelectedHtcp(string contractNo,string attachmentno)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder(@"select t1.contractNo,t1.pcode,t1.pname,(t1.quantity-isnull(t2.inquantity,0)) as quantity,
t1.qunit,t1.price,t1.priceUnit,(t1.quantity-isnull(t2.inquantity,0))*t1.price as amount,t1.packspec,
t1.packing,t1.pallet,t1.ifcheck,t1.ifplace from Econtract_ap t1
left join 
(
select contractNo,mcode,sum(inquantity) as inquantity from dbo.sotckminmatr
left join sotckmindoc on sotckmindoc.indocno=sotckminmatr.indocno and sotckmindoc.contractNo=@contractNo 
group by contractNo,mcode) t2 on t1.contractNo=t2.contractNo  and t1.pcode=t2.mcode  
where t1.contractNo=@contractNo and t1.quantity-isnull(t2.inquantity,0)>0
            "),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@contractNo",contractNo),
                    new System.Data.SqlClient.SqlParameter("@attachmentno",attachmentno)
                });

            return sb;
        }

        public StringBuilder GetSelectedHtcp_XS(string contractNo, string attachmentno)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder(@"select t1.contractNo,t1.pcode,t1.pname,(t1.quantity-isnull(t2.outquantity,0)) as quantity,
t1.qunit,t1.price,t1.priceUnit,(t1.quantity-isnull(t2.outquantity,0))*t1.price as amount,t1.packspec,
t1.packing,t1.pallet,t1.ifcheck,t1.ifplace from Econtract_ap t1
left join 
(
select contractNo,mcode,sum(outquantity) as outquantity from dbo.sotckmoutmatr
left join sotckmout on sotckmout.outdocno=sotckmoutmatr.outdocno and sotckmout.contractNo=@contractNo 
group by contractNo,mcode) t2 on t1.contractNo=t2.contractNo  and t1.pcode=t2.mcode  
where t1.contractNo=@contractNo and t1.quantity-isnull(t2.outquantity,0)>0
            "),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@contractNo",contractNo),
                    new System.Data.SqlClient.SqlParameter("@attachmentno",attachmentno)
                });

            return sb;
        }

        public bool SaveRkdata(Hashtable main, List<Hashtable> lisdetail)
        {
            bool r = false;

            //判断是新增还是修改
            bool isedit = false;
            string indocno = main["indocno"].ToString();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var o= bll.ExecuteScalar(" select count(1) from sotckmindoc where indocno=@indocno ",
                    new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@indocno",indocno)
                    });
                if (o.ToString().Length > 0)
                {
                    isedit = Convert.ToInt32(o)>=1;
                }
            }

            if (isedit == false)
            {
                main.Add("createman",RequestSession.GetSessionUser().UserId);
                main.Add("createmanname" ,RequestSession.GetSessionUser().UserName);
                main.Add("createdate",DateTime.Now);
                main.Add("lastmod", RequestSession.GetSessionUser().UserId);
                main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
                main.Add("lastmoddate", DateTime.Now);
            }
            else
            {
                main.Add("lastmod", RequestSession.GetSessionUser().UserId);
                main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
                main.Add("lastmoddate", DateTime.Now);
            }
         
            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "sotckminmatr", ref sqls, ref objs);

            List<Hashtable> lisMain = new List<Hashtable>();
            lisMain.Add(main);
            StringBuilder[] sqls2 = new StringBuilder[lisMain.Count];
            object[] objs2 = new object[lisMain.Count];

            if (isedit == false)
            {
                //新增
                SqlUtil.getBatchFromListStandard(lisMain, "sotckmindoc", ref sqls2, ref objs2);
            }
            else
            {
                //修改
                SqlUtil.getBatchFromListStandardUpdate(lisMain, "sotckmindoc"," indocno=@indocno ", ref sqls2, ref objs2);
            }

            String deleteSub = "delete sotckminmatr where indocno=@indocno;";

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(deleteSub, new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@indocno",indocno)
                });
                    for (int i = 0; i < sqls.Count(); i++)
                    {
                        bll.ExecuteNonQuery(sqls[i].ToString(), objs[i] as System.Data.SqlClient.SqlParameter[]);
                    }
                    for (int i = 0; i < sqls2.Count(); i++)
                    {
                        bll.ExecuteNonQuery(sqls2[i].ToString(), objs2[i] as System.Data.SqlClient.SqlParameter[]);
                    }
                    bll.SqlTran.Commit();
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }

            return r;
        }

        public StringBuilder CkTest(List<Hashtable> lisdetail,string sellercode)
        {
            StringBuilder sb = new StringBuilder();
            //判断仓库当前是否有足够库存可以出库
            //查询仓库和当前入库保留
            String sql = @"select sum(quantity) as maxquantity from 
(
select mcode,mname,quantity from dbo.sotckmatrbatch where mcode=@mcode and buyercode=@sellercode 
union all 
select mcode,mname,(0-outquantity) as quantity from sotckmoutmatr where mcode = @mcode 
and exists(select 1 from sotckmout where sotckmoutmatr.outdocno=sotckmout.outdocno 
and sotckmout.status in ('新建','提交','审核'))
) tt  ";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                foreach (Hashtable htmcode in lisdetail)
                {
                    String mcode = htmcode["mcode"].ToString();
                    object ret= bll.ExecuteScalar(sql, new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                        new System.Data.SqlClient.SqlParameter("@sellercode",sellercode)
                    });
                    if (ret.ToString().Length == 0)
                    {
                        ret = "0";
                    }
                    if (Convert.ToDecimal(ret) < Convert.ToDecimal(htmcode["outquantity"]))
                    {
                        sb.AppendLine(htmcode["mname"].ToString() + "  仓库中数量不足，不能出库！");
                    }
                }
            }

            return sb;
        }

        public bool SaveCkdata(Hashtable main, List<Hashtable> lisdetail)
        {
            bool r = false;

            //判断是新增还是修改
            bool isedit = false;
            string outdocno = main["outdocno"].ToString();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var o = bll.ExecuteScalar(" select count(1) from sotckmout where outdocno=@outdocno ",
                    new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@outdocno",outdocno)
                    });
                if (o.ToString().Length > 0)
                {
                    isedit = Convert.ToInt32(o) >= 1;
                }
            }

            if (isedit == false)
            {
                main.Add("createman", RequestSession.GetSessionUser().UserId);
                main.Add("createmanname", RequestSession.GetSessionUser().UserName);
                main.Add("createdate", DateTime.Now);
                main.Add("lastmod", RequestSession.GetSessionUser().UserId);
                main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
                main.Add("lastmoddate", DateTime.Now);
            }
            else
            {
                main.Add("lastmod", RequestSession.GetSessionUser().UserId);
                main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
                main.Add("lastmoddate", DateTime.Now);
            }



            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "sotckmoutmatr", ref sqls, ref objs);

            List<Hashtable> lisMain = new List<Hashtable>();
            lisMain.Add(main);
            StringBuilder[] sqls2 = new StringBuilder[lisMain.Count];
            object[] objs2 = new object[lisMain.Count];

            if (isedit == false)
            {
                //新增
                SqlUtil.getBatchFromListStandard(lisMain, "sotckmout", ref sqls2, ref objs2);
            }
            else
            {
                //修改
                SqlUtil.getBatchFromListStandardUpdate(lisMain, "sotckmout", " outdocno=@outdocno ", ref sqls2, ref objs2);
            }

            String deleteSub = "delete sotckmoutmatr where outdocno=@outdocno;";

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(deleteSub, new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@outdocno",outdocno)
                });
                    for (int i = 0; i < sqls.Count(); i++)
                    {
                        bll.ExecuteNonQuery(sqls[i].ToString(), objs[i] as System.Data.SqlClient.SqlParameter[]);
                    }
                    for (int i = 0; i < sqls2.Count(); i++)
                    {
                        bll.ExecuteNonQuery(sqls2[i].ToString(), objs2[i] as System.Data.SqlClient.SqlParameter[]);
                    }
                    bll.SqlTran.Commit();
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }

            return r;
        }

        public bool DeleteInstock(string indocno)
        {
            bool r = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("delete sotckmindoc where indocno=@indocno;");
            sb.Append("delete sotckminmatr where indocno=@indocno");
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(sb.ToString(),
                        new System.Data.SqlClient.SqlParameter[]{new System.Data.SqlClient.SqlParameter("@indocno",indocno)});
                    bll.SqlTran.Commit();
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }

            return r;
        }

        public bool DeleteOutstock(string outdocno)
        {
            bool r = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("delete sotckmout where outdocno=@outdocno;");
            sb.Append("delete sotckmoutmatr where outdocno=@outdocno");
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(sb.ToString(),
                        new System.Data.SqlClient.SqlParameter[]{new System.Data.SqlClient.SqlParameter("@outdocno",outdocno)});
                    bll.SqlTran.Commit();
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }

            return r;
        }
        /// <summary>
        /// 修改单据状态
        /// </summary>
        /// <param name="docno"></param>
        /// <param name="doctype"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool SubmitBill(string docno, string doctype, string status)
        {
            bool r = false;
            string tableName = "";
            string keyname = "";
            if (doctype == "入库")
            {
                tableName = "sotckmindoc";
                keyname = "indocno";
            }
            else if (doctype == "出库")
            {
                tableName = "sotckmout";
                keyname = "outdocno";
            }
            string sql = " update " + tableName + " set status=@status where " + keyname + "=@docno;";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.ExecuteNonQuery(sql,
                    new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@docno",docno),
                        new System.Data.SqlClient.SqlParameter("@status",status)
                    });
                r = true;
            }

            return r;
        }

        public bool ConfirmInStock(string indocno,string userid)
        {
            bool r = false;

            //货物受入库影响的产品
            string sqlpcode = " select * from sotckmatrbatch t1 where exists(select 1 from sotckminmatr t2,sotckmindoc t3  where t2.indocno=t3.indocno and t1.contractno=t3.contractno  and t2.indocno=@indocno and t1.mcode=t2.mcode );";
            string sqlInstock = @"select sotckmindoc.contractno,mcode,mname,mspec,sum(inquantity) as inquantity,unit,min(price) as price,sum(amount) as amount,sotckmindoc.sellercode,sotckmindoc.buyercode 
 from sotckminmatr 
inner join sotckmindoc on sotckmindoc.indocno=sotckminmatr.indocno 
where sotckmindoc.indocno=@indocno group by sotckmindoc.contractno,mcode,mname,mspec,unit,sotckmindoc.sellercode,sotckmindoc.buyercode ;";
            DataTable dtp = null;
            DataTable dtInstockP = null;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds= bll.ExecDatasetSql(sqlpcode + sqlInstock,
                    new System.Data.SqlClient.SqlParameter[]
                    {
                        new System.Data.SqlClient.SqlParameter("@indocno",indocno)
                    });
                dtp = ds.Tables[0];
                dtInstockP = ds.Tables[1];
            }
            List<string> calcMCode=new List<string>();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    foreach (DataRow dr in dtInstockP.Rows)
                    {
                        string mcode = dr["mcode"].ToString();
                        string mname = dr["mname"].ToString();
                        string contractno = dr["contractno"].ToString();
                        string sellercode = dr["sellercode"].ToString();
                        string buyercode = dr["buyercode"].ToString();
                        if (calcMCode.Contains(mcode))
                        {
                            continue;
                        }
                        decimal inquantity = Convert.ToDecimal(dtInstockP.Compute("sum(inquantity)", "mcode='" + mcode + "' "));
                        decimal amout = Convert.ToDecimal(dtInstockP.Compute("sum(amount)", "mcode='" + mcode + "' "));
                        DataRow[] rr = dtp.Select("mcode='" + mcode + "' ");
                        if (rr.Length > 0)
                        {
                            //update
                            decimal quantity = Convert.ToDecimal(rr[0]["quantity"])+inquantity;
                            decimal amount = Convert.ToDecimal(rr[0]["amount"])+amout;
                            decimal price = 0;
                            if(quantity!=0)
                            {
                                price = Math.Round(amount / quantity, 5);
                            }

                            string sql = " update sotckmatrbatch set quantity=@quantity,amount=@amount,price=@price,lastmoddate=getdate() where mcode=@mcode and wcode=@wcode and batchno=@batchno and contractno=@contractno;";
                            var pms = new System.Data.SqlClient.SqlParameter[]{
                                new System.Data.SqlClient.SqlParameter("@quantity",quantity),
                                new System.Data.SqlClient.SqlParameter("@amount",amount),
                                new System.Data.SqlClient.SqlParameter("@price",price),
                                new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                                new System.Data.SqlClient.SqlParameter("@wcode",""),
                                new System.Data.SqlClient.SqlParameter("@batchno",""),
                                new System.Data.SqlClient.SqlParameter("@contractno",contractno)
                            };
                            bll.ExecuteNonQuery(sql, pms);
                        }
                        else
                        {
                             string sql = @"insert into dbo.sotckmatrbatch(contractno,buyercode,sellercode,wcode,mcode,mname,batchno,mspec,quantity,unit,
position,price,amount,createdate,lastmoddate) 
values(@contractno,@buyercode,@sellercode,@wcode,@mcode,@mname,@batchno,@mspec,@quantity,@unit,'',@price,@amount,getdate(),getdate())
";
                             var pms = new System.Data.SqlClient.SqlParameter[]{
                                new System.Data.SqlClient.SqlParameter("@contractno",contractno),
                                new System.Data.SqlClient.SqlParameter("@buyercode",buyercode),
                                new System.Data.SqlClient.SqlParameter("@sellercode",sellercode),
                                new System.Data.SqlClient.SqlParameter("@wcode",""),
                                new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                                new System.Data.SqlClient.SqlParameter("@mname",mname),
                                new System.Data.SqlClient.SqlParameter("@batchno",""),
                                new System.Data.SqlClient.SqlParameter("@mspec",dr["mspec"]),
                                new System.Data.SqlClient.SqlParameter("@quantity",dr["inquantity"]),
                                new System.Data.SqlClient.SqlParameter("@unit",dr["unit"]),
                                new System.Data.SqlClient.SqlParameter("@price",dr["price"]),
                                new System.Data.SqlClient.SqlParameter("@amount",dr["amount"]),
                            };
                             bll.ExecuteNonQuery(sql, pms);
                        }
                    }
                    bll.ExecuteNonQuery("update sotckmindoc set status='确认',confirmdate=getdate() where indocno=@indocno;", 
                        new System.Data.SqlClient.SqlParameter[]{
                            new System.Data.SqlClient.SqlParameter("@indocno",indocno)
                        });
                    bll.SqlTran.Commit();
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }

            return r;
        }

        public bool ConfirmOutStock(string outdocno, string userid)
        {
            bool r = false;

            string sqlSelect = @"select * from sotckmatrbatch t1 where exists(select 1 from sotckmoutmatr t2,sotckmout t3 where  t2.outdocno=t3.outdocno and t3.sellercode=t1.buyercode and t2.outdocno=@outdocno and t1.mcode=t2.mcode ) order by t1.lastmoddate asc ;select * from sotckmoutmatr where outdocno=@outdocno ;";
            String sql = "update sotckmout set status='确认',confirmdate=getdate() where outdocno=@outdocno;";

            DataTable dtoutMat = null;
            DataTable dtStockMat = null;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds1 = bll.ExecDatasetSql(sqlSelect,
                     new System.Data.SqlClient.SqlParameter[]{
                       new System.Data.SqlClient.SqlParameter("@outdocno",outdocno)});
                dtStockMat = ds1.Tables[0];
                dtoutMat = ds1.Tables[1];
            }

            //更新stock内存表
            dtStockMat.AcceptChanges();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    foreach (DataRow dr in dtoutMat.Rows)
                    {
                        string mcode = dr["mcode"].ToString();
                        string mname = dr["mname"].ToString();
              

                        //需要出的数量
                        decimal outquantity = Convert.ToDecimal(dtoutMat.Compute("sum(outquantity)", "mcode='" + mcode + "' "));
                        decimal amout = Convert.ToDecimal(dtoutMat.Compute("sum(amount)", "mcode='" + mcode + "' "));

                        //判断数量是否足够出
                        decimal stockquantity = Convert.ToDecimal(dtStockMat.Compute("sum(quantity)","mcode='" + mcode + "' "));
                        if (outquantity > stockquantity)
                        {
                            throw new Exception("产品 【" + mname + "】仓库数量为：" + stockquantity + ",合同应出数量为：" + outquantity + ",应出数量大于仓库数量，出库失败！");
                        }
                        DataRow[] rr = dtStockMat.Select("mcode='" + mcode + "' ");
                        if (rr.Length > 0)
                        {
                            foreach (DataRow drstockmatr in rr)
                            {
                                if (outquantity <= 0)
                                {
                                    break;
                                }
                                decimal drquantity = Convert.ToDecimal(drstockmatr["quantity"]);
                                decimal drprice = Convert.ToDecimal(drstockmatr["price"]);
                               
                                if (outquantity > drquantity)
                                {
                                    drstockmatr["quantity"] = 0;
                                    drstockmatr["amount"] = 0;
                                    outquantity -= drquantity;
                                }
                                else
                                {
                                    drstockmatr["quantity"] = drquantity - outquantity;
                                    drstockmatr["amount"] = (drquantity - outquantity) * drprice;
                                }
                            }
                            //根据行状态更新数据库数据
                            foreach (DataRow drstockmatr in rr)
                            {
                                if (drstockmatr.RowState == DataRowState.Modified)
                                {
                                    string contractno = drstockmatr["contractno"].ToString();
                                    //update
                                    decimal quantity = Convert.ToDecimal(drstockmatr["quantity"]);
                                    decimal amount = Convert.ToDecimal(drstockmatr["amount"]);
                                    String c1 = drstockmatr["contractno"].ToString();
                                    String m1 = drstockmatr["mcode"].ToString();
                                    String w1 = drstockmatr["wcode"].ToString();
                                    String b1 = drstockmatr["batchno"].ToString();

                                    var pms = new System.Data.SqlClient.SqlParameter[]{
                                    new System.Data.SqlClient.SqlParameter("@quantity",quantity),
                                    new System.Data.SqlClient.SqlParameter("@amount",amount),
                                    new System.Data.SqlClient.SqlParameter("@mcode",m1),
                                    new System.Data.SqlClient.SqlParameter("@wcode",w1),
                                    new System.Data.SqlClient.SqlParameter("@batchno",b1),
                                    new System.Data.SqlClient.SqlParameter("@contractno",contractno)
                                };
                                    bll.ExecuteNonQuery(@" update sotckmatrbatch set quantity=@quantity,amount=@amount
                                where mcode=@mcode and wcode=@wcode and batchno=@batchno and contractno=@contractno ; ", pms);
                                }
                            }
                        }
                    }
                    bll.ExecuteNonQuery(sql, new System.Data.SqlClient.SqlParameter[]{
                       new System.Data.SqlClient.SqlParameter("@outdocno",outdocno)});
                    bll.SqlTran.Commit();
                    Console.WriteLine("事务提交！");
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }

            return r;
        }

        public StringBuilder GetStockAccount(string mcode,string mname,string beginTime,string endTime)
        {
            StringBuilder sb = null;

            StringBuilder where = new StringBuilder(" quantity>=0  ");
            if (mcode.Trim().Length > 0)
            {
                where.Append(" and mcode like '%'+@mcode+'%'");
            }
            if (mname.Trim().Length > 0)
            {
                where.Append(" and mcode like '%@mname%'");
            }
            if (beginTime.Trim().Length > 0)
            {
                where.Append(" and createdate >=@beginTime ");
            }
            if (endTime.Trim().Length > 0)
            {
                endTime = Convert.ToDateTime(endTime).AddDays(1).ToString("yyyy-MM-dd");
                where.Append(" and createdate < @endTime");
            }

            sb = jui.GetDatatableJsonString(
                new StringBuilder(@"select t1.*,t2.seller,t2.buyer from (select wcode,contractno,position,mcode,mname,mspec,unit,SUM(amount) as amount,SUM(quantity) as quantity,
 min(price) as price,''  as remark 
 from sotckmatrbatch where " +where.ToString()+ @"  group by contractno,wcode,position,mcode,mname,mspec,unit) t1 
left join Econtract t2 on t1.contractno=t2.contractno 
            "),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                    new System.Data.SqlClient.SqlParameter("@mname",mname),
                    new System.Data.SqlClient.SqlParameter("@beginTime",beginTime),
                    new System.Data.SqlClient.SqlParameter("@endTime",endTime)
                });

            return sb;
        }
    }
}
