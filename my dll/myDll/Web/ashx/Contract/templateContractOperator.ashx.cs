using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// templateContractOperator 的摘要说明
    /// </summary>
    public class templateContractOperator : IHttpHandler, IRequiresSessionState
    {
        RM.Busines.btemplate.BtemplateBLL templBLL = new Busines.btemplate.BtemplateBLL();
        public void ProcessRequest(HttpContext context)
        {
            string module = context.Request.QueryString["module"];
            string err = "";
            bool suc = false;
            switch (module)
            {
                //获取模板管理列表
                case "saveTemplate":

                    suc = saveTemplate(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;
                case "deleteTemplate":

                    suc = DelTemplate(ref err, context);
                    context.Response.Write(returnData(suc, err));
                    break;

                case "addLogisticsTemplate":

                    suc = addLogisticsTemplate(ref err, context);//保存物流合同模板
                    context.Response.Write(returnData(suc, err));
                    break;

                case "deleteLogisticsTemplate":

                    suc = deleteLogisticsTemplate(ref err, context);//删除物流合同模板
                    context.Response.Write(returnData(suc, err));
                    break;

                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;
            }
        }
        //添加编辑物流合同模板
        private bool addLogisticsTemplate(ref string err, HttpContext context)
        {
            bool suc = false;
            var logisticsTemplateno = context.Request.Params["logisticsTemplateno"];
            if (string.IsNullOrEmpty(logisticsTemplateno))
            {
                logisticsTemplateno = Guid.NewGuid().ToString();
            }
            //logisticsTemplateno, logisticsTemplateName, buyerCode, simpleBuyer, buyer, sellerCode, simpleSeller, seller, signedPlace, signedTime, contractText, createman, createdate
            //获取主模板信息
            Hashtable ht = new Hashtable();
            ht["seller"] = context.Request["seller"] == null ? "" : context.Request["seller"].ToString();
            ht["sellerCode"] = context.Request["sellercode"] == null ? "" : context.Request["sellercode"].ToString();
            ht["simpleSeller"] = context.Request["simpleSeller"] == null ? "" : context.Request["simpleSeller"].ToString();
            ht["signedtime"] = context.Request["signedtime"] == null ? "" : context.Request["signedtime"].ToString();
            ht["signedplace"] = context.Request["signedplace"] == null ? "" : context.Request["signedplace"].ToString();
            ht["buyer"] = context.Request["buyer"] == null ? "" : context.Request["buyer"].ToString();
            ht["buyerCode"] = context.Request["buyercode"] == null ? "" : context.Request["buyercode"].ToString();
            ht["simpleBuyer"] = context.Request["simpleBuyer"] == null ? "" : context.Request["simpleBuyer"].ToString();
            ht["contractText"] = context.Request["htmlcontent"] == null ? "" : context.Request["htmlcontent"].ToString();
            ht["logisticsTemplateno"] = logisticsTemplateno;
            ht["logisticsTemplateName"] = context.Request["logisticsTemplateName"] == null ? "0" : context.Request["logisticsTemplateName"].ToString();
            ht["createman"] = RequestSession.GetSessionUser().UserName;
            ht["createdate"] = DateTime.Now.ToString();
            //获取第一列表名信息
            Hashtable htFirstItem = new Hashtable();
            htFirstItem["logisticsTemplateno"] = logisticsTemplateno;
            htFirstItem["item1"] = context.Request["item1"] == null ? "" : context.Request["item1"].ToString();
            htFirstItem["item2"] = context.Request["item2"] == null ? "" : context.Request["item2"].ToString();
            htFirstItem["item3"] = context.Request["item3"] == null ? "" : context.Request["item3"].ToString();
            htFirstItem["item4"] = context.Request["item4"] == null ? "" : context.Request["item4"].ToString();
            htFirstItem["item5"] = context.Request["item5"] == null ? "" : context.Request["item5"].ToString();
            htFirstItem["item6"] = context.Request["item6"] == null ? "" : context.Request["item6"].ToString();
            htFirstItem["item7"] = context.Request["item7"] == null ? "" : context.Request["item7"].ToString();
            htFirstItem["item8"] = context.Request["item8"] == null ? "" : context.Request["item8"].ToString();
            htFirstItem["item1width"] = context.Request["item1width"] == null ? "" : context.Request["item1width"].ToString();
            htFirstItem["item2width"] = context.Request["item2width"] == null ? "" : context.Request["item2width"].ToString();
            htFirstItem["item3width"] = context.Request["item3width"] == null ? "" : context.Request["item3width"].ToString();
            htFirstItem["item4width"] = context.Request["item4width"] == null ? "" : context.Request["item4width"].ToString();
            htFirstItem["item5width"] = context.Request["item5width"] == null ? "" : context.Request["item5width"].ToString();
            htFirstItem["item6width"] = context.Request["item6width"] == null ? "" : context.Request["item6width"].ToString();
            htFirstItem["item7width"] = context.Request["item7width"] == null ? "" : context.Request["item7width"].ToString();
            htFirstItem["item8width"] = context.Request["item8width"] == null ? "" : context.Request["item8width"].ToString();
            //获取模板动态表格
            string logisticsItem = context.Request.Params["logisticsItem"];
            List<Hashtable> logisticsItemTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(logisticsItem);
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    //保存主表
                    suc = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_BTEMPLATE_LOGISTICS, "logisticsTemplateno", ht["logisticsTemplateno"].ToString(), ht);
                    if (suc)
                    {
                        //保存表名
                        suc = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_BTEMPLATE_LOGISTICSFIRSTITEM, "logisticsTemplateno", htFirstItem["logisticsTemplateno"].ToString(), htFirstItem);
                        //保存表格内容
                        //先删除，再添加
                        int r = DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_BTEMPLATE_LOGISTICSITEMS, "logisticsTemplateno", logisticsTemplateno);
                        foreach (var hs in logisticsItemTable)
                        {
                            Hashtable htLogistics = new Hashtable();
                            htLogistics["item1"] = hs["item1"];
                            htLogistics["item2"] = hs["item2"];
                            htLogistics["item3"] = hs["item3"];
                            htLogistics["item4"] = hs["item4"];
                            htLogistics["item5"] = hs["item5"];
                            htLogistics["item6"] = hs["item6"];
                            htLogistics["item7"] = hs["item7"];
                            htLogistics["item8"] = hs["item8"];
                            htLogistics["logisticsTemplateno"] = logisticsTemplateno;
                            DataFactory.SqlDataBase().InsertByHashtable(ConstantUtil.TABLE_BTEMPLATE_LOGISTICSITEMS, htLogistics);
                        }
                    }
                    bll.SqlTran.Commit();

                }

                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    err = ex.Message;

                }
                return suc;

            }

        }
        //添加或编辑
        private bool saveTemplate(ref string err, HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            bool suc = false;
            string templateno = (context.Request.Params["templateno"] ?? "").ToString();
            string flowdirection = context.Request.Params["flowdirection"];
            if (templateno.Trim().Length == 0)
            {
                templateno = Guid.NewGuid().ToString();
            }

            Hashtable main = new Hashtable();
            main.Add("templateno", templateno);
            main.Add("status", "新建");
            main.Add("remark", (context.Request.Params["remark"] ?? "").ToString());
            main.Add("templatename", (context.Request.Params["templatename"] ?? "").ToString());
            main.Add("language", (context.Request.Params["language"] ?? "").ToString());
            main.Add("seller", (context.Request.Params["seller"] ?? "").ToString());
            main.Add("sellercode", (context.Request.Params["sellercode"] ?? "").ToString());
            main.Add("simpleSeller", (context.Request.Params["simpleSeller"] ?? "").ToString());
            main.Add("buyer", (context.Request.Params["buyer"] ?? "").ToString());
            main.Add("buyercode", (context.Request.Params["buyercode"] ?? "").ToString());
            main.Add("simpleBuyer", (context.Request.Params["simpleBuyer"] ?? "").ToString());
            main.Add("signedtime", (context.Request.Params["signedtime"] ?? "").ToString());
            main.Add("signedplace", (context.Request.Params["signedplace"] ?? "").ToString());
            main.Add("buyeraddress", (context.Request.Params["buyeraddress"] ?? "").ToString());
            main.Add("selleraddress", (context.Request.Params["selleraddress"] ?? "").ToString());
            main.Add("currency", (context.Request.Params["currency"] ?? "").ToString());
            main.Add("pricement1", (context.Request.Params["pricement1"] ?? "").ToString());
            main.Add("pricement1per", (context.Request.Params["pricement1per"] ?? "").ToString());
            main.Add("pricement2", (context.Request.Params["pricement2"] ?? "").ToString());
            main.Add("pricement2per", (context.Request.Params["pricement2per"] ?? "").ToString());
            main.Add("pvalidity", (context.Request.Params["pvalidity"] ?? "").ToString());
            main.Add("shipment", (context.Request.Params["shipment"] ?? "").ToString());
            main.Add("transport", (context.Request.Params["transport"] ?? "").ToString());
            main.Add("tradement", (context.Request.Params["tradement"] ?? "").ToString());
            main.Add("tradeShow", (context.Request.Params["tradeShow"] ?? "").ToString());
            main.Add("harborout", (context.Request.Params["harborout"] ?? "").ToString());
            main.Add("harborarrive", (context.Request.Params["harborarrive"] ?? "").ToString());
            main.Add("harboroutCode", (context.Request.Params["harboroutCode"] ?? "").ToString());
            main.Add("harboroutCountry", (context.Request.Params["harboroutCountry"] ?? "").ToString());
            main.Add("harboroutarriveCountry", (context.Request.Params["harboroutarriveCountry"] ?? "").ToString());
            main.Add("harborarriveCode", (context.Request.Params["harborarriveCode"] ?? "").ToString());
            main.Add("deliveryPlace", (context.Request.Params["deliveryPlace"] ?? "").ToString());
            main.Add("harborclear", (context.Request.Params["harborclear"] ?? "").ToString());
            main.Add("placement", (context.Request.Params["placement"] ?? "").ToString());
            main.Add("validity", (context.Request.Params["validity"] ?? "").ToString());
            main.Add("productCategory", (context.Request.Params["productCategory"] ?? "").ToString());
            main.Add("flowdirection", (context.Request.Params["flowdirection"] ?? "").ToString());
            main.Add("templateCategory", (context.Request.Params["templateCategory"] ?? "").ToString());
            main.Add("shippingmark", (context.Request.Params["shippingmark"] ?? "").ToString());
            main.Add("overspill", (context.Request.Params["overspill"] ?? "").ToString());
            main.Add("paymentType", (context.Request.Params["paymentType"] ?? "").ToString());
            main.Add("shipDate", (context.Request.Params["shipDate"] ?? "").ToString());
            List<Hashtable> lisdetail = new List<Hashtable>();
            string subDataJson = context.Request["datagrid"] == null ? "" : context.Request["datagrid"].ToString();
            lisdetail = JsonHelper.DeserializeJsonToList<Hashtable>(subDataJson);
            try
            {
                if (flowdirection == ConstantUtil.IMPORT)//进境
                {
                    suc = SaveImportTemplatedata(main, lisdetail);
                }
                else if (flowdirection == ConstantUtil.EXPORT)//出境
                {
                    suc = SaveExportTemplatedata(main, lisdetail);
                }


                return suc;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }

        }
        //删除进境出境合同模板
        public bool DelTemplate(ref string err, HttpContext context)
        {
            string templateNo = context.Request.Params["templateno"];
            string flowdirection = context.Request.Params["flowdirection"];
            string sql = string.Empty;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {
                    if (flowdirection ==ConstantUtil.IMPORT)
                    {
                        sql = " delete btemplate_importEcontract where templateno=@templateno;delete btemp_detail where templateno=@templateno;";
                    }
                    else if (flowdirection ==ConstantUtil.EXPORT)
                    {
                        sql = " delete btemplate_exportEcontract where templateno=@templateno;delete btemp_detail where templateno=@templateno;";
                    }


                    bll.ExecuteNonQuery(sql, new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@templateno",templateNo)
                    });
                    bll.SqlTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {

                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;

                }
            }
        }
        //删除物流合同模板
        public bool deleteLogisticsTemplate(ref string err, HttpContext context)
        {
            string logisticsTemplateno = context.Request.Params["logisticsTemplateno"];
            string sql = string.Empty;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                bll.SqlTran = bll.SqlCon.BeginTransaction();
                try
                {

                    sql = @" delete btemplate_logistics where logisticsTemplateno=@logisticsTemplateno;
                             delete btemplate_logisticsItems where logisticsTemplateno=@logisticsTemplateno;
                             delete btemplate_logisticsFirstItem where logisticsTemplateno=@logisticsTemplateno;";
                    int r = bll.ExecuteNonQuery(sql, new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("@logisticsTemplateno",logisticsTemplateno)
                    });
                    bll.SqlTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {

                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;

                }
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string returnData(bool isok, string err)
        {
            string r = "";
            if (isok)
            {
                r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            }
            else
            {
                r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
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
                if (string.IsNullOrWhiteSpace(hh["sortno"].ToString()))
                {
                    hh["sortno"] = (m + 1).ToString();
                }
            
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
                if (string.IsNullOrWhiteSpace(hh["sortno"].ToString()))
                {
                    hh["sortno"] = (m + 1).ToString();
                }
            
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
    }
}