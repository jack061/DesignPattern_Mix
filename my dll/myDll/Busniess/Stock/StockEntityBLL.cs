using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using RM.Common.DotNetBean;
using WZX.Busines.Util;
using System.Data;

namespace RM.Busines.Stock
{
    public class StockEntityBLL
    {
        JsonHelperEasyUi jui = new JsonHelperEasyUi();
        public void AddModel()
        {

        }

        public void EditModel()
        {

        }

        #region 库存结余
        //出库时获取库存结余数量
        public StringBuilder GetJsonStockSwiftList(string wcode, string owner, string ownercode, string mcode, string mname)
        {
            StringBuilder sb = null;

            String sql = @" select * from StockEntitySwift where 1=1 and quantity > 0 ";
            if (wcode.Length > 0)
            {
                sql += " and wcode like '%'+@wcode+'%' ";
            }
            if (owner.Length > 0)
            {
                sql += " and owner like '%'+@owner+'%' ";
            }
            if (ownercode.Length > 0)
            {
                sql += " and ownercode like '%'+@ownercode+'%' ";
            }
            if (mcode.Length > 0)
            {
                sql += " and and mcode like '%'+@mcode+'%' ";
            }
            if (mname.Length > 0)
            {
                sql += " and mname like '%'+@mname+'%' ";
            }

            sb = jui.GetDatatableJsonString(
                new StringBuilder(sql),
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@wcode",wcode),
                    new System.Data.SqlClient.SqlParameter("@owner",owner),
                    new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                    new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                    new System.Data.SqlClient.SqlParameter("@mname",mname),
                });

            return sb;
        }
        //库存结余明细
        public StringBuilder GetStockLog(string mcode, string batchno)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder(@"select * from StockEntityLog where mcode=@mcode and batchno=@batchno order by trandate "),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                    new System.Data.SqlClient.SqlParameter("@batchno",batchno)
                });

            return sb;
        }
        //？
        public StringBuilder GetJsonStockList()
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.StockEntitySwift "),
                new System.Data.SqlClient.SqlParameter[] { });

            return sb;
        }
        #endregion

        #region 入库
        //入库单一个产品信息
        public StringBuilder GetRkMatrListByMcode(string mcode)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.StockInEntity_D where mcode=@mcode"),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@mcode",mcode)
                });

            return sb;
        }
        //入库保存&提交
        public bool SaveRkdata(Hashtable main, List<Hashtable> lisdetail)
        {
            bool r = false;

            //判断是新增还是修改
            bool isedit = false;
            string indocno = main["indocno"].ToString();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var o = bll.ExecuteScalar(" select count(1) from StockInEntity where indocno=@indocno ",
                    new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@indocno",indocno)
                    });
                if (o.ToString().Length > 0)
                {
                    isedit = Convert.ToInt32(o) >= 1;
                }
            }

            if (isedit == false)
            {
                main.Add("createman", RequestSession.GetSessionUser().UserAccount);
                main.Add("createmanname", RequestSession.GetSessionUser().UserName);
                main.Add("createdate", DateTime.Now);
                main.Add("lastmod", RequestSession.GetSessionUser().UserAccount);
                main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
                main.Add("lastmoddate", DateTime.Now);
            }
            else
            {
                main.Add("lastmod", RequestSession.GetSessionUser().UserAccount);
                main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
                main.Add("lastmoddate", DateTime.Now);
            }

            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "StockInEntity_D", ref sqls, ref objs);

            List<Hashtable> lisMain = new List<Hashtable>();
            lisMain.Add(main);
            StringBuilder[] sqls2 = new StringBuilder[lisMain.Count];
            object[] objs2 = new object[lisMain.Count];

            if (isedit == false)
            {
                //新增
                SqlUtil.getBatchFromListStandard(lisMain, "StockInEntity", ref sqls2, ref objs2);
            }
            else
            {
                //修改
                SqlUtil.getBatchFromListStandardUpdate(lisMain, "StockInEntity", " indocno=@indocno ", ref sqls2, ref objs2);
            }

            String deleteSub = "delete StockInEntity_D where indocno=@indocno;";

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
        //库管接收入库保存&提交
        public bool ReceiveRkdata(Hashtable main, List<Hashtable> lisdetail)
        {
            bool r = false;
            string indocno = main["indocno"].ToString();

            main.Add("lastmod", RequestSession.GetSessionUser().UserId);
            main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
            main.Add("lastmoddate", DateTime.Now);

            main.Add("stockmanagercode", RequestSession.GetSessionUser().UserId);
            main.Add("stockmanager", RequestSession.GetSessionUser().UserName);
            main.Add("stockmanagercondate", DateTime.Now);

            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "StockInEntity_D", ref sqls, ref objs);

            List<Hashtable> lisMain = new List<Hashtable>();
            lisMain.Add(main);
            StringBuilder[] sqls2 = new StringBuilder[lisMain.Count];
            object[] objs2 = new object[lisMain.Count];

            //修改
            //SqlUtil.getBatchFromListStandardUpdate(lisMain, "StockInEntity", " indocno=@indocno ", ref sqls2, ref objs2);

            String deleteSub = "delete StockInEntity_D where indocno=@indocno;";

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
                    //for (int i = 0; i < sqls2.Count(); i++)
                    //{
                    //    bll.ExecuteNonQuery(sqls2[i].ToString(), objs2[i] as System.Data.SqlClient.SqlParameter[]);
                    //}
                    bll.SqlTran.Commit();
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }

            if (main["status"].ToString() == "已收货")
            {
                r = false;
                //货物受入库影响的产品
                string sqlpcode = " select * from StockEntitySwift t1 where exists(select 1 from StockInEntity_D t2,StockInEntity t3 where t2.indocno=t3.indocno and t2.indocno=@indocno and t1.wcode=t3.wcode and t1.ownercode=t3.ownercode and t1.mcode=t2.mcode);";

                string sqlInstock = @"select t1.*,t2.carnumber  as carnumberd,t2.ticketdate as ticketdated,t2.mcode,t2.mname,t2.mspec,t2.unit,t2.pack,t2.packunit,t2.packdes,t2.realinquantity,t2.realnumber 
             from StockInEntity t1,StockInEntity_D t2  where t1.indocno=t2.indocno and t1.indocno=@indocno;";
                DataTable dtp = null;
                DataTable dtInstockP = null;
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    DataSet ds = bll.ExecDatasetSql(sqlpcode + sqlInstock,
                        new System.Data.SqlClient.SqlParameter[]
                    {
                        new System.Data.SqlClient.SqlParameter("@indocno",indocno)
                    });
                    dtp = ds.Tables[0];
                    dtInstockP = ds.Tables[1];
                }
                List<string> calcMCode = new List<string>();

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
                            string wcode = dr["wcode"].ToString();
                            string wname = dr["wname"].ToString();
                            string ownercode = dr["ownercode"].ToString();
                            string owner = dr["owner"].ToString();
                            if (calcMCode.Contains(mcode))
                            {
                                continue;
                            }
                            decimal inquantity = Convert.ToDecimal(dtInstockP.Compute("sum(realinquantity)", "mcode='" + mcode + "' and ownercode='" + ownercode + "' "));
                            decimal realnumber = Convert.ToDecimal(dtInstockP.Compute("sum(realnumber)", "mcode='" + mcode + "' and ownercode='" + ownercode + "' "));
                            //DataRow[] rr = dtp.Select("mcode='" + mcode + "' and wcode='" + wcode + "'  and ownercode='" + ownercode + "'");
                            //if (rr.Length > 0)
                            //{
                            //    //update
                            //    decimal quantity = Convert.ToDecimal(rr[0]["quantity"]) + inquantity;
                            //    decimal number = Convert.ToDecimal(rr[0]["number"]) + realnumber;
                            //    string sql = " update StockEntitySwift set quantity=@quantity,number=@number,lastmoddate=getdate() where wcode=@wcode and mcode=@mcode and  ownercode=@ownercode;";
                            //    var pms = new System.Data.SqlClient.SqlParameter[]{
                            //    new System.Data.SqlClient.SqlParameter("@quantity",quantity),
                            //    new System.Data.SqlClient.SqlParameter("@number",number),                          
                            //    new System.Data.SqlClient.SqlParameter("@wcode",wcode),
                            //    new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                            //    new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                            //};
                            //    bll.ExecuteNonQuery(sql, pms);
                            //}
                            //else
                            //{wcode, wname, ownercode, owner, batchno, mcode, mname, mspec, number, quantity, unit,  pack, packunit, packdes, position, remark, supcode, 
                      //docno, doctype, createman, createdate, lastmod, lastmoddate, contractno

                            string sql = @"insert into dbo.StockEntitySwift(ownercode,owner,wcode,wname,mcode,carnumber,ticketdate,mname,batchno,mspec,quantity,number,unit,pack, packunit, packdes,position,docno, doctype, createman, createdate, lastmod, lastmoddate) 
values(@ownercode,@owner,@wcode,@wname,@mcode,@carnumber,@ticketdate,@mname,@batchno,@mspec,@quantity,@number,@unit,@pack, @packunit, @packdes,'',@docno, @doctype, @createman,getdate(),@lastmod,getdate())";
                                var pms = new System.Data.SqlClient.SqlParameter[]{
                                new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                                new System.Data.SqlClient.SqlParameter("@owner",owner),
                                new System.Data.SqlClient.SqlParameter("@wcode",wcode),
                                new System.Data.SqlClient.SqlParameter("@wname",wname),
                                new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                                new System.Data.SqlClient.SqlParameter("@carnumber",dr["carnumberd"]),
                                new System.Data.SqlClient.SqlParameter("@ticketdate",dr["ticketdated"]),
                                new System.Data.SqlClient.SqlParameter("@mname",mname),
                                new System.Data.SqlClient.SqlParameter("@batchno",dr["indocno"]),
                                new System.Data.SqlClient.SqlParameter("@mspec",dr["mspec"]),
                                new System.Data.SqlClient.SqlParameter("@quantity",dr["realinquantity"]),
                                new System.Data.SqlClient.SqlParameter("@number",dr["realnumber"]),
                                new System.Data.SqlClient.SqlParameter("@unit",dr["unit"]),
                                new System.Data.SqlClient.SqlParameter("@pack",dr["pack"]),
                                new System.Data.SqlClient.SqlParameter("@packunit",dr["packunit"]),
                                new System.Data.SqlClient.SqlParameter("@packdes",dr["packdes"]),
                                new System.Data.SqlClient.SqlParameter("@docno",dr["indocno"]),
                                new System.Data.SqlClient.SqlParameter("@doctype","入库通知"),
                                new System.Data.SqlClient.SqlParameter("@createman",RequestSession.GetSessionUser().UserName),
                                new System.Data.SqlClient.SqlParameter("@lastmod",RequestSession.GetSessionUser().UserName)
                            };
                                bll.ExecuteNonQuery(sql, pms);

                            //插入入库日志记录
                                sql = @"insert into dbo.StockEntityLog(wcode, wname, ownercode, owner, mcode,carnumber,ticketdate, mname, batchno, mspec, pack, packunit, packdes, number, quantity, unit, position,  doctype, docno, trandate, stockmanagercode, stockmanager, stockmanagercondate, createman, createdate, lastmod, lastmoddate) 
values(@wcode, @wname, @ownercode, @owner, @mcode,@carnumber,@ticketdate, @mname, @batchno, @mspec, @pack, @packunit, @packdes, @number, @quantity, @unit,'', @doctype, @docno, getdate(), @stockmanagercode, @stockmanager, getdate(),@createman,getdate(),@lastmod,getdate())";
                                pms = new System.Data.SqlClient.SqlParameter[]{ 
                                new System.Data.SqlClient.SqlParameter("@wcode",wcode),
                                new System.Data.SqlClient.SqlParameter("@wname",wname),
                                new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                                new System.Data.SqlClient.SqlParameter("@owner",owner),
                                new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                                new System.Data.SqlClient.SqlParameter("@carnumber",dr["carnumberd"]),
                                new System.Data.SqlClient.SqlParameter("@ticketdate",dr["ticketdated"]),
                                new System.Data.SqlClient.SqlParameter("@mname",mname),
                                new System.Data.SqlClient.SqlParameter("@batchno",dr["indocno"]),
                                new System.Data.SqlClient.SqlParameter("@mspec",dr["mspec"]),
                                new System.Data.SqlClient.SqlParameter("@pack",dr["pack"]),
                                new System.Data.SqlClient.SqlParameter("@packunit",dr["packunit"]),
                                new System.Data.SqlClient.SqlParameter("@packdes",dr["packdes"]),
                                new System.Data.SqlClient.SqlParameter("@number",dr["realnumber"]),
                                new System.Data.SqlClient.SqlParameter("@quantity",dr["realinquantity"]),
                                new System.Data.SqlClient.SqlParameter("@unit",dr["unit"]),
                                new System.Data.SqlClient.SqlParameter("@doctype","入库通知"),
                                new System.Data.SqlClient.SqlParameter("@docno",dr["indocno"]),
                                new System.Data.SqlClient.SqlParameter("@stockmanagercode",RequestSession.GetSessionUser().UserAccount),
                                new System.Data.SqlClient.SqlParameter("@stockmanager",RequestSession.GetSessionUser().UserName),
                                new System.Data.SqlClient.SqlParameter("@createman",RequestSession.GetSessionUser().UserName),
                                new System.Data.SqlClient.SqlParameter("@lastmod",RequestSession.GetSessionUser().UserName)
                            };
                                bll.ExecuteNonQuery(sql, pms);
                            }
                        //}
                        bll.ExecuteNonQuery("update StockInEntity set status=@status,confirmdate=getdate(),stockmanagercode=@stockmanagercode,stockmanager=@stockmanager,stockmanagercondate=getdate() where indocno=@indocno;",
                        new System.Data.SqlClient.SqlParameter[]{
                            new System.Data.SqlClient.SqlParameter("@status",main["status"].ToString()),
                            new System.Data.SqlClient.SqlParameter("@stockmanagercode",RequestSession.GetSessionUser().UserAccount),
                            new System.Data.SqlClient.SqlParameter("@stockmanager",RequestSession.GetSessionUser().UserName),
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
            }

            return r;
        }
        //删除入库
        public bool DeleteInstock(string indocno)
        {
            bool r = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("delete StockInEntity where indocno=@indocno;");
            sb.Append("delete StockInEntity_D where indocno=@indocno");
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(sb.ToString(),
                        new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@indocno", indocno) });
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
        //确认入库
        public bool ConfirmInStock(string indocno, string userid,string status)
        {
            bool r = false;

            //货物受入库影响的产品
            string sqlpcode = " select * from StockEntitySwift t1 where exists(select 1 from StockInEntity_D t2,StockInEntity t3 where t2.indocno=t3.indocno and t2.indocno=@indocno and t1.wcode=t3.wcode and t1.ownercode=t3.ownercode and t1.mcode=t2.mcode);";
//            string sqlInstock = @"select StockInEntity.contractno,mcode,mname,mspec,sum(inquantity) as inquantity,unit,sum(number) as number,sum(realinquantity) as realinquantity,sum(realnumber) as realnumber,StockInEntity.wcode,StockInEntity.ownercode,StockInEntity.wname,StockInEntity.owner 
// from StockInEntity_D 
//inner join StockInEntity on StockInEntity.indocno=StockInEntity_D.indocno 
//where StockInEntity.indocno=@indocno group by StockInEntity.contractno,mcode,mname,mspec,unit,StockInEntity.wcode,StockInEntity.ownercode,StockInEntity.wname,StockInEntity.owner ;";
            string sqlInstock = @"select t1.*,t2.mcode,t2.mname,t2.mspec,t2.unit,t2.realinquantity,t2.realnumber 
             from StockInEntity t1,StockInEntity_D t2  where t1.indocno=t2.indocno and t1.indocno=@indocno;";
            DataTable dtp = null;
            DataTable dtInstockP = null;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(sqlpcode + sqlInstock,
                    new System.Data.SqlClient.SqlParameter[]
                    {
                        new System.Data.SqlClient.SqlParameter("@indocno",indocno)
                    });
                dtp = ds.Tables[0];
                dtInstockP = ds.Tables[1];
            }
            List<string> calcMCode = new List<string>();

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
                        string wcode = dr["wcode"].ToString();
                        string wname = dr["wname"].ToString();
                        string ownercode = dr["ownercode"].ToString();
                        string owner = dr["owner"].ToString();
                        if (calcMCode.Contains(mcode))
                        {
                            continue;
                        }
                        decimal inquantity = Convert.ToDecimal(dtInstockP.Compute("sum(realinquantity)", "mcode='" + mcode + "' and ownercode='" + ownercode + "' "));
                        decimal realnumber = Convert.ToDecimal(dtInstockP.Compute("sum(realnumber)", "mcode='" + mcode + "' and ownercode='" + ownercode + "' "));
                        DataRow[] rr = dtp.Select("mcode='" + mcode + "' and wcode='" + wcode + "'  and ownercode='" + ownercode + "'");
                        if (rr.Length > 0)
                        {
                            //update
                            decimal quantity = Convert.ToDecimal(rr[0]["quantity"]) + inquantity;
                            decimal number = Convert.ToDecimal(rr[0]["number"]) + realnumber;
                            string sql = " update StockEntitySwift set quantity=@quantity,number=@number,lastmoddate=getdate() where wcode=@wcode and mcode=@mcode and  ownercode=@ownercode;";
                            var pms = new System.Data.SqlClient.SqlParameter[]{
                                new System.Data.SqlClient.SqlParameter("@quantity",quantity),
                                new System.Data.SqlClient.SqlParameter("@number",number),                          
                                new System.Data.SqlClient.SqlParameter("@wcode",wcode),
                                new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                                new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                            };
                            bll.ExecuteNonQuery(sql, pms);
                        }
                        else
                        {
                            string sql = @"insert into dbo.StockEntitySwift(ownercode,owner,wcode,wname,mcode,mname,batchno,mspec,quantity,number,unit,
position,createdate,lastmoddate) 
values(@ownercode,@owner,@wcode,@wname,@mcode,@mname,@batchno,@mspec,@quantity,@number,@unit,'',getdate(),getdate())
";
                            var pms = new System.Data.SqlClient.SqlParameter[]{
                                new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                                new System.Data.SqlClient.SqlParameter("@owner",owner),
                                new System.Data.SqlClient.SqlParameter("@wcode",wcode),
                                new System.Data.SqlClient.SqlParameter("@wname",wname),
                                new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                                new System.Data.SqlClient.SqlParameter("@mname",mname),
                                new System.Data.SqlClient.SqlParameter("@batchno",""),
                                new System.Data.SqlClient.SqlParameter("@mspec",dr["mspec"]),
                                new System.Data.SqlClient.SqlParameter("@quantity",dr["realinquantity"]),
                                new System.Data.SqlClient.SqlParameter("@number",dr["realnumber"]),
                                new System.Data.SqlClient.SqlParameter("@unit",dr["unit"])
                            };
                            bll.ExecuteNonQuery(sql, pms);
                        }
                    }
                    bll.ExecuteNonQuery("update StockInEntity set status=@status,confirmdate=getdate(),busmancode=@busmancode,busman=@busman,busmancondate=getdate() where indocno=@indocno;",
                        new System.Data.SqlClient.SqlParameter[]{
                            new System.Data.SqlClient.SqlParameter("@status",status),
                            new System.Data.SqlClient.SqlParameter("@busmancode",RequestSession.GetSessionUser().UserId),
                            new System.Data.SqlClient.SqlParameter("@busman",RequestSession.GetSessionUser().UserName),
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

        #endregion

        #region 出库
        // 根据海运通知单获取出库列表
        public StringBuilder GetJsonStockOutList_SeaNotice(string indocno, string man, string begintime, string endtime)
        {
            StringBuilder sb = null;

            String sql = @"select t1.*,t2.outdocno,t2.outdate,t2.status as outstatus,t2.createmanname from checkoutNotice t1 left join StockOutEntity t2 on t1.checkid=t2.checkid where 1=1 ";
            if (indocno.Length > 0)
            {
                sql += " and t2.outdocno like '%'+@outdocno+'%' ";
            }
            if (man.Length > 0)
            {
                sql += " and t2.createmanname like '%'+@man+'%' ";
            }
            if (begintime.Length > 0)
            {
                sql += " and t2.outdate >= @begintime ";
            }
            if (endtime.Length > 0)
            {
                endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                sql += " and t2.outdate <@endtime ";
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
        //出库产品列表
        public StringBuilder GetCkMatrList(string outdocno)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.StockOutEntity_D where outdocno=@outdocno"),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@outdocno",outdocno)
                });

            return sb;
        }
        //
        public StringBuilder GetCkMatrListByMcode(string mcode)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder("select * from dbo.StockOutEntity_D where mcode=@mcode"),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@mcode",mcode)
                });

            return sb;
        }
        //出库校验
        public StringBuilder CkTest(List<Hashtable> lisdetail, string wcode, string ownercode)
        {
            StringBuilder sb = new StringBuilder();
            //判断仓库当前是否有足够库存可以出库
            //查询仓库和当前出库保留
//            String sql = @"select sum(quantity) as maxquantity from 
//(
//select mcode,mname,quantity from dbo.StockEntitySwift where mcode=@mcode and ownercode=@wcode 
//union all 
//select mcode,mname,(0-outquantity) as quantity from StockOutEntity_D where mcode = @mcode 
//and exists(select 1 from StockOutEntity where StockOutEntity_D.outdocno=StockOutEntity.outdocno 
//and StockOutEntity.status in ('新建','提交','审核'))
//) tt  ";
            String sql = @"select sum(isnull(quantity,0)) as maxquantity from 
(
select mcode,mname,quantity from dbo.StockEntitySwift where mcode=@mcode and batchno=@batchno and wcode=@wcode and ownercode=@ownercode
union all 
select mcode,mname,(0-outquantity) as quantity from StockOutEntity_D where mcode = @mcode and batchno=@batchno   
and exists(select 1 from StockOutEntity where StockOutEntity_D.outdocno=StockOutEntity.outdocno 
and StockOutEntity.status in ('新建','待发货') and StockOutEntity.ownercode=@ownercode and StockOutEntity.wcode=@wcode)
) tt  ";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                foreach (Hashtable htmcode in lisdetail)
                {
                    String mcode = htmcode["mcode"].ToString();
                    String batchno = htmcode["batchno"].ToString();
                    object ret = bll.ExecuteScalar(sql, new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@mcode",mcode),
                        new System.Data.SqlClient.SqlParameter("@batchno",batchno),
                        new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                        new System.Data.SqlClient.SqlParameter("@wcode",wcode)
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
        //出库保存
        public bool SaveCkdata(Hashtable main, List<Hashtable> lisdetail)
        {
            bool r = false;

            //判断是新增还是修改
            bool isedit = false;
            string outdocno = main["outdocno"].ToString();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var o = bll.ExecuteScalar(" select count(1) from StockOutEntity where outdocno=@outdocno ",
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
                main.Add("createman", RequestSession.GetSessionUser().UserAccount);
                main.Add("createmanname", RequestSession.GetSessionUser().UserName);
                main.Add("createdate", DateTime.Now);
                main.Add("lastmod", RequestSession.GetSessionUser().UserAccount);
                main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
                main.Add("lastmoddate", DateTime.Now);
            }
            else
            {
                main.Add("lastmod", RequestSession.GetSessionUser().UserAccount);
                main.Add("lastmodname", RequestSession.GetSessionUser().UserName);
                main.Add("lastmoddate", DateTime.Now);
            }



            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "StockOutEntity_D", ref sqls, ref objs);

            List<Hashtable> lisMain = new List<Hashtable>();
            lisMain.Add(main);
            StringBuilder[] sqls2 = new StringBuilder[lisMain.Count];
            object[] objs2 = new object[lisMain.Count];

            if (isedit == false)
            {
                //新增
                SqlUtil.getBatchFromListStandard(lisMain, "StockOutEntity", ref sqls2, ref objs2);
            }
            else
            {
                //修改
                SqlUtil.getBatchFromListStandardUpdate(lisMain, "StockOutEntity", " outdocno=@outdocno ", ref sqls2, ref objs2);
            }

            String deleteSub = "delete StockOutEntity_D where outdocno=@outdocno;";

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
        //出库删除
        public bool DeleteOutstock(string outdocno)
        {
            bool r = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("delete StockOutEntity where outdocno=@outdocno;");
            sb.Append("delete StockOutEntity_D where outdocno=@outdocno");
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(sb.ToString(),
                        new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@outdocno", outdocno) });
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
        //出库确认
        public bool ConfirmOutStock(string outdocno, string userid,string username,string status, List<Hashtable> lisdetail)
        {
            bool r = false;

            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "StockOutEntity_D", ref sqls, ref objs);

            String deleteSub = "delete StockOutEntity_D where outdocno=@outdocno;";

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
                    //for (int i = 0; i < sqls2.Count(); i++)
                    //{
                    //    bll.ExecuteNonQuery(sqls2[i].ToString(), objs2[i] as System.Data.SqlClient.SqlParameter[]);
                    //}
                    bll.SqlTran.Commit();
                    r = true;
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }

            if (status == "已发货")
            {
                r = false;
                //string sqlSelect = @"select * from StockEntitySwift t1 where exists(select 1 from StockOutEntity_D t2,StockOutEntity t3 where  t2.outdocno=t3.outdocno and t3.wcode=t1.ownercode and t2.outdocno=@outdocno and t1.mcode=t2.mcode ) order by t1.lastmoddate asc ;select * from StockOutEntity_D where outdocno=@outdocno ;";
                string sqlSelect = @"select t1.* from StockEntitySwift t1 ,StockOutEntity t2 where t1.wcode = t2.wcode and t1.ownercode = t2.ownercode and t2.outdocno = @outdocno and t1.quantity > 0 ;select * from StockOutEntity_D where outdocno=@outdocno ;select * from StockOutEntity where outdocno=@outdocno ;";

                DataTable dtout = null;
                DataTable dtoutMat = null;
                DataTable dtStockMat = null;
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    DataSet ds1 = bll.ExecDatasetSql(sqlSelect,
                         new System.Data.SqlClient.SqlParameter[]{
                       
                       new System.Data.SqlClient.SqlParameter("@outdocno",outdocno)
                     });
                    dtStockMat = ds1.Tables[0];
                    dtoutMat = ds1.Tables[1];
                    dtout = ds1.Tables[2];
                    dtStockMat.Columns.Add("oldnumber",typeof(int));
                    dtStockMat.Columns.Add("oldquantity", typeof(decimal));
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
                            string batchno = dr["batchno"].ToString();
                            string mcode = dr["mcode"].ToString();
                            string mname = dr["mname"].ToString();


                            //需要出的数量
                            decimal outquantity = Convert.ToDecimal(dtoutMat.Compute("sum(realoutquantity)", "mcode='" + mcode + "' and batchno='" + batchno + "' "));
                            decimal outnumber = Convert.ToDecimal(dtoutMat.Compute("sum(realnumber)", "mcode='" + mcode + "'  and batchno='" + batchno + "' "));

                            //判断数量是否足够出
                            decimal stockquantity = Convert.ToDecimal(dtStockMat.Compute("sum(quantity)", "mcode='" + mcode + "'  and batchno='" + batchno + "' "));
                            decimal stocknumber = Convert.ToDecimal(dtStockMat.Compute("sum(number)", "mcode='" + mcode + "'  and batchno='" + batchno + "' "));
                            if (outquantity > stockquantity && outnumber > stocknumber)
                            {
                                throw new Exception("产品 【" + mname + "】仓库数量为：" + stockquantity + ",应出数量为：" + outquantity + ",应出数量大于仓库数量，不能出库！");
                            }
                            DataRow[] rr = dtStockMat.Select("mcode='" + mcode + "'  and batchno='" + batchno + "' ");
                            if (rr.Length > 0)
                            {
                                foreach (DataRow drstockmatr in rr)
                                {
                                    if (outquantity <= 0)
                                    {
                                        break;
                                    }
                                    decimal drquantity = Convert.ToDecimal(drstockmatr["quantity"]);
                                    decimal drnumber = Convert.ToDecimal(drstockmatr["number"]);

                                    if (outquantity > drquantity && outnumber > drnumber)
                                    {
                                        drstockmatr["quantity"] = 0;
                                        drstockmatr["number"] = 0;
                                        outquantity -= drquantity;
                                        outnumber -= drnumber;
                                        drstockmatr["oldquantity"] = -drquantity;
                                        drstockmatr["oldnumber"] = -drnumber;
                                    }
                                    else
                                    {
                                        drstockmatr["quantity"] = drquantity - outquantity;
                                        drstockmatr["number"] = drnumber - outnumber;
                                        drstockmatr["oldquantity"] = -outquantity;
                                        drstockmatr["oldnumber"] = -outnumber;
                                    }
                                }
                                
                                //根据行状态更新数据库数据
                                foreach (DataRow drstockmatr in rr)
                                {
                                    if (drstockmatr.RowState == DataRowState.Modified)
                                    {
                                        //string contractno = drstockmatr["contractno"].ToString();
                                        //update
                                        decimal quantity = Convert.ToDecimal(drstockmatr["quantity"]);
                                        decimal number = Convert.ToDecimal(drstockmatr["number"]);
                                        String ownercode = drstockmatr["ownercode"].ToString();
                                        String mcode1 = drstockmatr["mcode"].ToString();
                                        String wcode = drstockmatr["wcode"].ToString();
                                        String batchno1 = drstockmatr["batchno"].ToString();
                                        String carnumber = drstockmatr["carnumber"].ToString();

                                        String owner = drstockmatr["owner"].ToString();
                                        String mname1 = drstockmatr["mname"].ToString();
                                        String wname = drstockmatr["wname"].ToString();

                                        var pms = new System.Data.SqlClient.SqlParameter[]{
                                    new System.Data.SqlClient.SqlParameter("@quantity",quantity),
                                    new System.Data.SqlClient.SqlParameter("@number",number),
                                    new System.Data.SqlClient.SqlParameter("@mcode",mcode1),
                                    new System.Data.SqlClient.SqlParameter("@wcode",wcode),
                                    new System.Data.SqlClient.SqlParameter("@batchno",batchno1),
                                    new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                                    new System.Data.SqlClient.SqlParameter("@carnumber",carnumber)};

                                        bll.ExecuteNonQuery(@" update StockEntitySwift set quantity=@quantity,number=@number
                                where mcode=@mcode and wcode=@wcode and batchno=@batchno and ownercode=@ownercode and carnumber=@carnumber; ", pms);

                                        //插入出库日志记录 
                                        string sql = @"insert into dbo.StockEntityLog(wcode, wname, ownercode, owner, carnumber,mcode, mname, batchno, mspec, pack, packunit, packdes, number, quantity, unit, position, doctype, docno, trandate, contractno, buyercode, buyer, sellercode, seller, shipcompany, shipname, shipnum, busman, busmancode,stockmanagercode, stockmanager, stockmanagercondate, createman, createdate, lastmod, lastmoddate,indate) 
values(@wcode, @wname, @ownercode, @owner,@carnumber, @mcode, @mname, @batchno, @mspec, @pack, @packunit, @packdes, @number, @quantity, @unit,'', @doctype, @docno, getdate(),@contractno, @buyercode, @buyer, @sellercode, @seller, @shipcompany, @shipname, @shipnum, @busman, @busmancode, @stockmanagercode, @stockmanager, getdate(),@createman,getdate(),@lastmod,getdate(),@indate)";
                                        pms = new System.Data.SqlClient.SqlParameter[]{ 
                                        new System.Data.SqlClient.SqlParameter("@wcode",wcode),
                                        new System.Data.SqlClient.SqlParameter("@wname",wname),
                                        new System.Data.SqlClient.SqlParameter("@ownercode",ownercode),
                                        new System.Data.SqlClient.SqlParameter("@owner",owner),
                                        new System.Data.SqlClient.SqlParameter("@carnumber",carnumber),
                                        new System.Data.SqlClient.SqlParameter("@mcode",mcode1),
                                        new System.Data.SqlClient.SqlParameter("@mname",mname1),
                                        new System.Data.SqlClient.SqlParameter("@batchno",dr["batchno"]),
                                        new System.Data.SqlClient.SqlParameter("@mspec",dr["mspec"]),
                                        new System.Data.SqlClient.SqlParameter("@pack",dr["pack"]),
                                        new System.Data.SqlClient.SqlParameter("@packunit",dr["packunit"]),
                                        new System.Data.SqlClient.SqlParameter("@packdes",dr["packdes"]),
                                        new System.Data.SqlClient.SqlParameter("@number",Convert.ToDecimal(drstockmatr["oldnumber"])),
                                        new System.Data.SqlClient.SqlParameter("@quantity",Convert.ToDecimal(drstockmatr["oldquantity"])),
                                        new System.Data.SqlClient.SqlParameter("@unit",dr["unit"]),
                                        new System.Data.SqlClient.SqlParameter("@doctype","出库通知"),
                                        new System.Data.SqlClient.SqlParameter("@docno",dtout.Rows[0]["outdocno"]),
                                        new System.Data.SqlClient.SqlParameter("@contractno",dtout.Rows[0]["contractno"]),
                                        new System.Data.SqlClient.SqlParameter("@buyercode",dtout.Rows[0]["buyercode"]),
                                        new System.Data.SqlClient.SqlParameter("@buyer",dtout.Rows[0]["buyer"]),
                                        new System.Data.SqlClient.SqlParameter("@sellercode",dtout.Rows[0]["sellercode"]),
                                        new System.Data.SqlClient.SqlParameter("@seller",dtout.Rows[0]["seller"]),
                                        new System.Data.SqlClient.SqlParameter("@shipcompany",dtout.Rows[0]["shipcompany"]),
                                        new System.Data.SqlClient.SqlParameter("@shipname",dtout.Rows[0]["shipname"]),
                                        new System.Data.SqlClient.SqlParameter("@shipnum",dtout.Rows[0]["shipnum"]),
                                        new System.Data.SqlClient.SqlParameter("@busman",dtout.Rows[0]["busman"]),
                                        new System.Data.SqlClient.SqlParameter("@busmancode",dtout.Rows[0]["busmancode"]),
                                        new System.Data.SqlClient.SqlParameter("@stockmanagercode",RequestSession.GetSessionUser().UserAccount),
                                        new System.Data.SqlClient.SqlParameter("@stockmanager",RequestSession.GetSessionUser().UserName),
                                        new System.Data.SqlClient.SqlParameter("@createman",RequestSession.GetSessionUser().UserName),
                                        new System.Data.SqlClient.SqlParameter("@lastmod",RequestSession.GetSessionUser().UserName),
                                        new System.Data.SqlClient.SqlParameter("@indate",drstockmatr["createdate"])};

                                        bll.ExecuteNonQuery(sql, pms);
                                    }
                                }
                            }
                        }
                        //
                        String sql1 = "update StockOutEntity set status=@status,stockmanagercode=@userid,stockmanager=@username,stockmanagercondate=getdate(),confirmdate=getdate() where outdocno=@outdocno;";

                        bll.ExecuteNonQuery(sql1, new System.Data.SqlClient.SqlParameter[]{
                        new System.Data.SqlClient.SqlParameter("@status",status),
                        new System.Data.SqlClient.SqlParameter("@userid",userid),
                       new System.Data.SqlClient.SqlParameter("@username",username),
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
            }

            return r;
        }
        #endregion


        public StringBuilder GetSelectedHtList(string contractNo, string attachmentNo, string begtime, string endtime, String bgroup, String seller, String buyer, string busman)
        {
            StringBuilder sb = null;

            string sbwhere1 = "";
            if (seller.Length > 0)
            {
                sbwhere1 += " and tt1.wcode=@seller ";
            }
            if (buyer.Length > 0)
            {
                sbwhere1 += " and  tt1.ownercode=@buyer ";
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
		select StockInEntity.contractNo,mcode,sum(inquantity) as inquantity from dbo.StockInEntity_D
		inner join   StockInEntity on StockInEntity.indocno=StockInEntity_D.indocno and StockInEntity.status in ('新建','提交','审核','确认')
		group by contractNo,mcode
	) t2 on t1.contractNo=t2.contractNo   and t1.pcode=t2.mcode 
    where (t1.quantity-isnull(t2.inquantity,0))>0) tt2 on tt1.contractNo=tt2.contractNo 
 where  tt1.contractNo like '%GY%' " + sbwhere1 + @") ttt1 where 1=1 
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

        public StringBuilder GetSelectedHtList_XS(string contractNo, string attachmentNo, string begtime, string endtime, String bgroup, string seller, string buyer, string busman)
        {
            StringBuilder sb = null;

            string sbwhere1 = " 1=1 ";
            if (seller.Length > 0)
            {
                sbwhere1 += " and tt1.wcode=@seller ";
            }
            if (buyer.Length > 0)
            {
                sbwhere1 += " and  tt1.ownercode=@buyer ";
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
		select StockOutEntity.contractNo,mcode,sum(outquantity) as outquantity from dbo.StockOutEntity_D
		inner join   StockOutEntity on StockOutEntity.outdocno=StockOutEntity_D.outdocno and StockOutEntity.status in ('新建','提交','审核','确认')
		group by contractNo,mcode
	) t2 on t1.contractNo=t2.contractNo   and t1.pcode=t2.mcode 
    where (t1.quantity-isnull(t2.outquantity,0))>0 
 ) tt2 on tt1.contractNo=tt2.contractNo 
 where   tt1.contractNo like '%XS%' and " + sbwhere1 + ") ttt1 where 1=1 ";

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

        public StringBuilder GetSelectedHtcp(string contractNo, string attachmentno)
        {
            StringBuilder sb = null;

            sb = jui.GetDatatableJsonString(
                new StringBuilder(@"select t1.contractNo,t1.pcode,t1.pname,(t1.quantity-isnull(t2.inquantity,0)) as quantity,
t1.qunit,t1.price,t1.priceUnit,(t1.quantity-isnull(t2.inquantity,0))*t1.price as amount,t1.packspec,
t1.packing,t1.pallet,t1.ifcheck,t1.ifplace from Econtract_ap t1
left join 
(
select contractNo,mcode,sum(inquantity) as inquantity from dbo.StockInEntity_D
left join StockInEntity on StockInEntity.indocno=StockInEntity_D.indocno and StockInEntity.contractNo=@contractNo 
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
select contractNo,mcode,sum(outquantity) as outquantity from dbo.StockOutEntity_D
left join StockOutEntity on StockOutEntity.outdocno=StockOutEntity_D.outdocno and StockOutEntity.contractNo=@contractNo 
group by contractNo,mcode) t2 on t1.contractNo=t2.contractNo  and t1.pcode=t2.mcode  
where t1.contractNo=@contractNo and t1.quantity-isnull(t2.outquantity,0)>0
            "),
                new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@contractNo",contractNo),
                    new System.Data.SqlClient.SqlParameter("@attachmentno",attachmentno)
                });

            return sb;
        }

        // 修改单据状态
        public bool SubmitBill(string docno, string doctype, string status)
        {
            bool r = false;
            string tableName = "";
            string keyname = "";
            if (doctype == "入库")
            {
                tableName = "StockInEntity";
                keyname = "indocno";
            }
            else if (doctype == "出库")
            {
                tableName = "StockOutEntity";
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

        

        

        
    }
}
