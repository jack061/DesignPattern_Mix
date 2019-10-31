using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using WZX.Busines.Util;
using RM.Common.DotNetBean;
using System.Web.SessionState;
using RM.Common.DotNetData;

namespace RM.Busines.btemplate
{
    public class BtemplateBLL : IRequiresSessionState
    {
        JsonHelperEasyUi jui = new JsonHelperEasyUi();
        public StringBuilder GetTemplateList(string templatename, String man, string begintime, string endtime)
        {
            StringBuilder sb = null;

            String sql = " select * from btemp_main where 1=1 ";
            if (templatename.Length > 0)
            {
                sql += " and templatename like '%'+@templatename+'%' ";
            }
            if (man.Length > 0)
            {
                sql += " and createman like '%'+@createman+'%' ";
            }
            if (begintime.Length > 0)
            {
                sql += " and createdate >= @begintime ";
            }
            if (endtime.Length > 0)
            {
                endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                sql += " and createdate <@endtime ";
            }

            sb = jui.GetDatatableJsonString(
                new StringBuilder(sql),
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@templatename",templatename),
                    new System.Data.SqlClient.SqlParameter("@createman",man),
                    new System.Data.SqlClient.SqlParameter("@begintime",begintime),
                    new System.Data.SqlClient.SqlParameter("@endtime",endtime),
                });

            return sb;
        }

        public StringBuilder GetTemplateDetailList(string templateno)
        {
            StringBuilder sb = null;

            String sql = " select *,case when isinline=1 then '是' else '否' end as inline  from btemp_detail where 1=1 ";

            sql += " and templateno =@templateno ";

            sb = jui.GetDatatableJsonString(
                new StringBuilder(sql),
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@templateno",templateno)
                });

            return sb;
        }

        public StringBuilder GetVarList(string flowdirection)
        {
            DataTable dtvariable = new DataTable();
            if (flowdirection == ConstantUtil.IMPORT)//进境
            {
                dtvariable.Columns.Add("id");
                dtvariable.Columns.Add("text");
                dtvariable.Rows.Add(ConstantUtil.TEMP_PRODUCT, ConstantUtil.TEMP_PRODUCT);//表格
                dtvariable.Rows.Add(ConstantUtil.TEMP_TRADEMENT, ConstantUtil.TEMP_TRADEMENT);//贸易条款 
                dtvariable.Rows.Add(ConstantUtil.TEMP_TRANSPORT, ConstantUtil.TEMP_TRANSPORT);//运输方式
                dtvariable.Rows.Add(ConstantUtil.TEMP_OVERSPILL, ConstantUtil.TEMP_OVERSPILL);//溢出率
                dtvariable.Rows.Add(ConstantUtil.TEMP_IMPORTHARBOR, ConstantUtil.TEMP_IMPORTHARBOR);//进口口岸
                dtvariable.Rows.Add(ConstantUtil.TEMP_ARRIVEHARBOR, ConstantUtil.TEMP_ARRIVEHARBOR);//到货口岸
                dtvariable.Rows.Add(ConstantUtil.TEMP_PRICEMENT1, ConstantUtil.TEMP_PRICEMENT1);//价格条款1
                dtvariable.Rows.Add(ConstantUtil.TEMP_PRICEMENT2, ConstantUtil.TEMP_PRICEMENT2);//价格条款2
                dtvariable.Rows.Add(ConstantUtil.TEMP_PVALIDITY, ConstantUtil.TEMP_PVALIDITY);//价格有效期
                dtvariable.Rows.Add(ConstantUtil.TEMP_VALIDITY, ConstantUtil.TEMP_VALIDITY);//合同有效期
                dtvariable.Rows.Add(ConstantUtil.TEMP_PAYLASTDATE, ConstantUtil.TEMP_PAYLASTDATE);//付款截止日
                dtvariable.Rows.Add(ConstantUtil.TEMP_PLACEMENT, ConstantUtil.TEMP_PLACEMENT);//产地条款
                dtvariable.Rows.Add(ConstantUtil.TEMP_PAYMENTTYPE, ConstantUtil.TEMP_PAYMENTTYPE);//付款方式
                dtvariable.Rows.Add(ConstantUtil.TEMP_SHIPDATE, ConstantUtil.TEMP_SHIPDATE);//发运日期

            }
            else
            {
                dtvariable.Columns.Add("id");
                dtvariable.Columns.Add("text");
                dtvariable.Rows.Add(ConstantUtil.TEMP_PRODUCT, ConstantUtil.TEMP_PRODUCT);//表格
                dtvariable.Rows.Add(ConstantUtil.TEMP_TRADEMENT, ConstantUtil.TEMP_TRADEMENT);//贸易条款 
                dtvariable.Rows.Add(ConstantUtil.TEMP_TRANSPORT, ConstantUtil.TEMP_TRANSPORT);//运输方式
                dtvariable.Rows.Add(ConstantUtil.TEMP_OVERSPILL, ConstantUtil.TEMP_OVERSPILL);//溢出率
                dtvariable.Rows.Add(ConstantUtil.TEMP_EXPORTHARBOR, ConstantUtil.TEMP_EXPORTHARBOR);//出口口岸
                dtvariable.Rows.Add(ConstantUtil.TEMP_ARRIVEHARBOR, ConstantUtil.TEMP_ARRIVEHARBOR);//到货口岸
                dtvariable.Rows.Add(ConstantUtil.TEMP_PRICEMENT1, ConstantUtil.TEMP_PRICEMENT1);//价格条款1
                dtvariable.Rows.Add(ConstantUtil.TEMP_PRICEMENT2, ConstantUtil.TEMP_PRICEMENT2);//价格条款2
                dtvariable.Rows.Add(ConstantUtil.TEMP_PVALIDITY, ConstantUtil.TEMP_PVALIDITY);//价格有效期
                dtvariable.Rows.Add(ConstantUtil.TEMP_VALIDITY, ConstantUtil.TEMP_VALIDITY);//合同有效期
                dtvariable.Rows.Add(ConstantUtil.TEMP_SHIPMENT, ConstantUtil.TEMP_SHIPMENT);//发运条款
                dtvariable.Rows.Add(ConstantUtil.TEMP_PLACEMENT, ConstantUtil.TEMP_PLACEMENT);//产地条款
                dtvariable.Rows.Add(ConstantUtil.TEMP_PAYMENTTYPE, ConstantUtil.TEMP_PAYMENTTYPE);//付款方式
                dtvariable.Rows.Add(ConstantUtil.TEMP_SHIPDATE, ConstantUtil.TEMP_SHIPDATE);//发运日期
            }


            dtvariable.AcceptChanges();
            return jui.ToEasyUIComboxJson(dtvariable);
        }

        public bool Savedata(Hashtable main, List<Hashtable> lisdetail)
        {
            bool r = false;

            //判断是新增还是修改
            bool isedit = false;
            string templateno = main["templateno"].ToString();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var o = bll.ExecuteScalar(" select count(1) from btemp_main where templateno=@templateno ",
                    new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@templateno",templateno)
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

            int m = 0;
            foreach (Hashtable hh in lisdetail)
            {
                hh["templateno"] = templateno;
                hh["sortno"] = (m + 1).ToString();
                if (hh.ContainsKey("inline"))
                {
                    if (hh["inline"].ToString() == "是")
                    {
                        hh["isinline"] = 1;
                    }
                    else
                    {
                        hh["isinline"] = 0;
                    }
                    hh.Remove("inline");
                }
                m++;
            }

            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "btemp_detail", ref sqls, ref objs);

            List<Hashtable> lisMain = new List<Hashtable>();
            lisMain.Add(main);
            StringBuilder[] sqls2 = new StringBuilder[lisMain.Count];
            object[] objs2 = new object[lisMain.Count];

            if (isedit == false)
            {
                //新增
                SqlUtil.getBatchFromListStandard(lisMain, "btemp_main", ref sqls2, ref objs2);
            }
            else
            {
                //修改
                SqlUtil.getBatchFromListStandardUpdate(lisMain, "btemp_main", " templateno=@templateno ", ref sqls2, ref objs2);
            }

            String deleteSub = "delete btemp_detail where templateno=@templateno;";

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(deleteSub, new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@templateno",templateno)
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
        //保存出境模板
        public bool SaveExportTemplatedata(Hashtable main, List<Hashtable> lisdetail)
        {
            bool r = false;

            //判断是新增还是修改
            bool isedit = false;
            string templateno = main["templateno"].ToString();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var o = bll.ExecuteScalar(" select count(1) from btemplate_exportEcontract where templateno=@templateno ",
                    new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@templateno",templateno)
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

            int m = 0;
            foreach (Hashtable hh in lisdetail)
            {
                hh["templateno"] = templateno;
                hh["sortno"] = (m + 1).ToString();
                if (hh.ContainsKey("inline"))
                {
                    if (hh["inline"].ToString() == "是")
                    {
                        hh["isinline"] = 1;
                    }
                    else
                    {
                        hh["isinline"] = 0;
                    }
                    hh.Remove("inline");
                }
                m++;
            }

            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "btemp_detail", ref sqls, ref objs);

            List<Hashtable> lisMain = new List<Hashtable>();
            lisMain.Add(main);
            StringBuilder[] sqls2 = new StringBuilder[lisMain.Count];
            object[] objs2 = new object[lisMain.Count];

            if (isedit == false)
            {
                //新增
                SqlUtil.getBatchFromListStandard(lisMain, "btemplate_exportEcontract", ref sqls2, ref objs2);
            }
            else
            {
                //修改
                SqlUtil.getBatchFromListStandardUpdate(lisMain, "btemplate_exportEcontract", " templateno=@templateno ", ref sqls2, ref objs2);
            }

            String deleteSub = "delete btemp_detail where templateno=@templateno;";

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(deleteSub, new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@templateno",templateno)
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
        //保存进境模板
        public bool SaveImportTemplatedata(Hashtable main, List<Hashtable> lisdetail)
        {
            bool r = false;

            //判断是新增还是修改
            bool isedit = false;
            string templateno = main["templateno"].ToString();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                var o = bll.ExecuteScalar(" select count(1) from btemplate_importEcontract where templateno=@templateno ",
                    new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@templateno",templateno)
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

            int m = 0;
            foreach (Hashtable hh in lisdetail)
            {
                hh["templateno"] = templateno;
                hh["sortno"] = (m + 1).ToString();
                if (hh.ContainsKey("inline"))
                {
                    if (hh["inline"].ToString() == "是")
                    {
                        hh["isinline"] = 1;
                    }
                    else
                    {
                        hh["isinline"] = 0;
                    }
                    hh.Remove("inline");
                }
                m++;
            }

            StringBuilder[] sqls = new StringBuilder[lisdetail.Count];
            object[] objs = new object[lisdetail.Count];
            SqlUtil.getBatchFromListStandard(lisdetail, "btemp_detail", ref sqls, ref objs);

            List<Hashtable> lisMain = new List<Hashtable>();
            lisMain.Add(main);
            StringBuilder[] sqls2 = new StringBuilder[lisMain.Count];
            object[] objs2 = new object[lisMain.Count];

            if (isedit == false)
            {
                //新增
                SqlUtil.getBatchFromListStandard(lisMain, "btemplate_importEcontract", ref sqls2, ref objs2);
            }
            else
            {
                //修改
                SqlUtil.getBatchFromListStandardUpdate(lisMain, "btemplate_importEcontract", " templateno=@templateno ", ref sqls2, ref objs2);
            }

            String deleteSub = "delete btemp_detail where templateno=@templateno;";

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    bll.ExecuteNonQuery(deleteSub, new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@templateno",templateno)
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
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="templateNo"></param>
        public void DelTemplate(string templateNo)
        {
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    string sql = " delete btemp_main where templateno=@templateno;delete btemp_detail where templateno=@templateno;";
                    bll.ExecuteNonQuery(sql, new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@templateno",templateNo)
                    });
                    bll.SqlTran.Commit();
                }
                catch
                {
                    bll.SqlTran.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 返回合同条款内容
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetContractTerms(String lans, String templateNo)
        {
            StringBuilder sb = new StringBuilder(String.Empty);

            DataTable dtmain = null;
            DataTable dtdetail = null;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                string sql = "select * from btemp_main where templateno=@templateno;select * from btemp_detail where  templateno=@templateno;";
                DataSet ds = bll.ExecDatasetSql(sql,
                    new System.Data.SqlClient.SqlParameter[]{
                        new System.Data.SqlClient.SqlParameter("templateno",templateNo)
                    });
                dtmain = ds.Tables[0];
                dtdetail = ds.Tables[1];
            }

            //以段落分段
            bool isShowChn = lans.Contains('中');
            bool isShowEng = lans.Contains('英');
            bool isShowRus = lans.Contains('俄');
            foreach (DataRow dr in dtdetail.Rows)
            {
                if (dr["isinline"].ToString().Length == 0)
                {
                    dr["isinline"] = false;
                }
                bool isinline = Convert.ToBoolean(dr["isinline"]);
                string chn = dr["chncontent"].ToString();
                string eng = dr["engcontent"].ToString();
                eng = eng.Replace("@", "@1");
                string rus = dr["ruscontent"].ToString();
                rus = rus.Replace("@", "@2");
                string sortno = dr["sortno"].ToString();
                string s = "";
                if (isinline)
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<span>" + sortno + "、" + chn + "</span>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<span>" + eng + "</span>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<span>" + rus + "</span>";
                    }
                    sb.AppendLine("<p>" + s + "</p>");
                }
                else
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<p>" + sortno + "、" + chn + "</p>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<p>" + eng + "</p>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<p>" + rus + "</p>";
                    }
                    sb.AppendLine(s);
                }
            }

            return sb;
        }

        public StringBuilder GetContractTermsByJson(String lans, List<Hashtable> templateTable)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            //以段落分段
            bool isShowChn = lans.Contains('中');
            bool isShowEng = lans.Contains('英');
            bool isShowRus = lans.Contains('俄');
            int r = 0;
            string sortno = string.Empty;

            foreach (Hashtable hs in templateTable)
            {
                r++;
                string s = "";
                if (hs["inline"].ToString() == "否")
                {
                    hs["isinline"] = false;
                }
                if (string.IsNullOrWhiteSpace(hs["sortno"].ToString()))
                {
                   sortno = r.ToString();
                }
                else
                {
                    sortno = hs["sortno"].ToString();
                }
                bool isinline = Convert.ToBoolean(hs["isinline"]);
                string chn = hs["chncontent"].ToString();
                string eng = hs["engcontent"].ToString();
                eng = eng.Replace("@", "@1");
                string rus = hs["ruscontent"].ToString();
                rus = rus.Replace("@", "@2");
         
                if (isinline)
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<span>" + sortno + "、" + chn + "</span>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<span>" + eng + "</span>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<span>" + rus + "</span>";
                    }
                    sb.AppendLine("<p>" + s + "</p>");

                }
                else
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<p>" + sortno + "、" + chn + "</p>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<p>" + eng + "</p>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<p>" + rus + "</p>";
                    }
                    sb.AppendLine(s);

                }
            }

            return sb;
        }

        public StringBuilder GetContractTermsByJson(String lans, List<HashTableExp> templateTable)
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            //以段落分段
            bool isShowChn = lans.Contains('中');
            bool isShowEng = lans.Contains('英');
            bool isShowRus = lans.Contains('俄');
            int r = 0;
            foreach (Hashtable hs in templateTable)
            {
                r++;
                string s = "";
                if (hs["inline"].ToString() == "否")
                {
                    hs["isinline"] = false;
                }
                bool isinline = Convert.ToBoolean(hs["isinline"]);
                string chn = hs["chncontent"].ToString();
                string eng = hs["engcontent"].ToString();
                eng = eng.Replace("@", "@1");
                string rus = hs["ruscontent"].ToString();
                rus = rus.Replace("@", "@2");
                string sortno = r.ToString();
                if (isinline)
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<span>" + sortno + "、" + chn + "</span>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<span>" + eng + "</span>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<span>" + rus + "</span>";
                    }
                    sb.AppendLine("<p>" + s + "</p>");

                }
                else
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<p>" + sortno + "、" + chn + "</p>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<p>" + eng + "</p>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<p>" + rus + "</p>";
                    }
                    sb.AppendLine(s);

                }
            }

            return sb;
        }

        /// <summary>
        /// 根据合同号获取合同条款
        /// </summary>
        /// <param name="lans">中英俄</param>
        /// <param name="contractNo"></param>
        /// <returns></returns>
        public StringBuilder GetContractTermsByCNo(String lans, String contractNo)
        {
            StringBuilder sb = new StringBuilder(String.Empty);

            DataTable dtdetail = null;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                string sql = "select * from Econtract_template where  contractno=@contractno order by sortno asc;";
                DataSet ds = bll.ExecDatasetSql(sql,
                    new System.Data.SqlClient.SqlParameter[]{
                        new System.Data.SqlClient.SqlParameter("@contractno",contractNo)
                    });
                dtdetail = ds.Tables[0];
            }

            //以段落分段
            bool isShowChn = lans.Contains('中');
            bool isShowEng = lans.Contains('英');
            bool isShowRus = lans.Contains('俄');
            foreach (DataRow dr in dtdetail.Rows)
            {
                if (dr["isinline"].ToString().Length == 0)
                {
                    dr["isinline"] = false;
                }
                bool isinline = Convert.ToBoolean(dr["isinline"]);
                string chn = dr["chncontent"].ToString();
                string eng = dr["engcontent"].ToString();
                eng = eng.Replace("@", "@1");
                string rus = dr["ruscontent"].ToString();
                rus = rus.Replace("@", "@2");
                string sortno = dr["sortno"].ToString();
                string s = "";
                if (isinline)
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<span>" + sortno + "、" + chn + "</span>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<span>" + eng + "</span>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<span>" + rus + "</span>";
                    }
                    sb.AppendLine("<p>" + s + "</p>");
                }
                else
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<p>" + sortno + "、" + chn + "</p>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<p>" + eng + "</p>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<p>" + rus + "</p>";
                    }
                    sb.AppendLine(s);
                }
            }

            return sb;
        }
        #region 获取商检合同模板条款
        public StringBuilder GetInspectContractTermsByCNo(String lans, String contractNo)
        {
            StringBuilder sb = new StringBuilder(String.Empty);

            DataTable dtdetail = null;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                string sql = "select * from Econtract_Inspect_template where  contractno=@contractno order by sortno asc;";
                DataSet ds = bll.ExecDatasetSql(sql,
                    new System.Data.SqlClient.SqlParameter[]{
                        new System.Data.SqlClient.SqlParameter("@contractno",contractNo)
                    });
                dtdetail = ds.Tables[0];
            }

            //以段落分段
            bool isShowChn = lans.Contains('中');
            bool isShowEng = lans.Contains('英');
            bool isShowRus = lans.Contains('俄');
            foreach (DataRow dr in dtdetail.Rows)
            {
                if (dr["isinline"].ToString().Length == 0)
                {
                    dr["isinline"] = false;
                }
                bool isinline = Convert.ToBoolean(dr["isinline"]);
                string chn = dr["chncontent"].ToString();
                string eng = dr["engcontent"].ToString();
                eng = eng.Replace("@", "@1");
                string rus = dr["ruscontent"].ToString();
                rus = rus.Replace("@", "@2");
                string sortno = dr["sortno"].ToString();
                string s = "";
                if (isinline)
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<span>" + sortno + "、" + chn + "</span>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<span>" + eng + "</span>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<span>" + rus + "</span>";
                    }
                    sb.AppendLine("<p>" + s + "</p>");
                }
                else
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<p>" + sortno + "、" + chn + "</p>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<p>" + eng + "</p>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<p>" + rus + "</p>";
                    }
                    sb.AppendLine(s);
                }
            }

            return sb;
        } 
        #endregion
        /// <summary>
        /// 根据模板号获取模板中的条款
        /// </summary>
        /// <param name="lans">中英俄</param>
        /// <param name="contractNo"></param>
        /// <returns></returns>
        public StringBuilder GetTemplateTermsByNo(String lans, String templateno)
        {
            StringBuilder sb = new StringBuilder(String.Empty);

            DataTable dtdetail = null;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                string sql = "select * from btemp_detail where  templateno=@templateno order by sortno asc;";
                DataSet ds = bll.ExecDatasetSql(sql,
                    new System.Data.SqlClient.SqlParameter[]{
                        new System.Data.SqlClient.SqlParameter("@templateno",templateno)
                    });
                dtdetail = ds.Tables[0];
            }

            //以段落分段
            bool isShowChn = lans.Contains('中');
            bool isShowEng = lans.Contains('英');
            bool isShowRus = lans.Contains('俄');
            foreach (DataRow dr in dtdetail.Rows)
            {
                if (dr["isinline"].ToString().Length == 0)
                {
                    dr["isinline"] = false;
                }
                bool isinline = Convert.ToBoolean(dr["isinline"]);
                string chn = dr["chncontent"].ToString();
                string eng = dr["engcontent"].ToString();
                eng = eng.Replace("@", "@1");
                string rus = dr["ruscontent"].ToString();
                rus = rus.Replace("@", "@2");
                string sortno = dr["sortno"].ToString();
                string s = "";
                if (isinline)
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<span>" + sortno + "、" + chn + "</span>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<span>" + eng + "</span>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<span>" + rus + "</span>";
                    }
                    sb.AppendLine("<p>" + s + "</p>");
                }
                else
                {
                    if (isShowChn && chn.Length > 0)
                    {
                        s += "<p>" + sortno + "、" + chn + "</p>";
                    }
                    if (isShowEng && eng.Length > 0)
                    {
                        s += "<p>" + eng + "</p>";
                    }
                    if (isShowRus && rus.Length > 0)
                    {
                        s += "<p>" + rus + "</p>";
                    }
                    sb.AppendLine(s);
                }
            }

            return sb;
        }

       
    }
}
