using RM.Busines;
using RM.Busines.btemplate;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;
using RM.Common.DotNetData;
using RM.Common.DotNetFile;
using RM.Web.Bus.Lib;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// templateContractData 的摘要说明
    /// </summary>
    public class templateContractData : IHttpHandler, IRequiresSessionState
    {
        RM.Busines.contract.contractBLL contractBll = new Busines.contract.contractBLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string module = context.Request.QueryString["module"];
            string err = "";
            string suc = string.Empty;
            switch (module)
            {
                //获取模板管理列表
                case "templatelist":
                    suc = templatelist(ref err, context);
                    context.Response.Write(suc);
                    break;
                //获取合同模板信息

                case "contractTempList":
                    suc = contractTempList(ref err, context);
                    context.Response.Write(suc);
                    break;

                //获取物流合同模板列表
                case "logisticsTemplatelist":
                    suc = logisticsTemplatelist(ref err, context);
                    context.Response.Write(suc);
                    break;
                //获取物流合同模板列表
                case "logisticsTemplatelist1":
                    suc = logisticsTemplatelist1(ref err, context);
                    context.Response.Write(suc);
                    break;

                //获取物流合同模板列表中动态表格数据
                case "logisticsItems":
                    suc = logisticsItems(ref err, context);
                    context.Response.Write(suc);
                    break;
                //获取实时模板预览数据
                case "GetTempldatePVC":
                    suc = GetTempldatePVC(ref err, context);
                    context.Response.Write(suc);
                    break;
                //获取实时物流模板预览数据
                case "GetLogisiticsTempldatePVC":
                    suc = GetLogisiticsTempldatePVC(ref err, context);
                    context.Response.Write(suc);
                    break;
                //获取模板填充数据
                case "LoadTemplateData":
                    suc = LoadTemplateData(ref err, context);
                    context.Response.Write(suc);
                    break;
                //导出PDF
                case "ExportPDF":
                    string tableName = context.Request.Params["tableName"] ?? string.Empty;
                    if (string.IsNullOrEmpty(tableName))
                    {
                   
                        context.Response.Write(ExportPDF(context));
                    }
                    else
                    {
                        context.Response.Write(ExportPDF(context,tableName));
                    }
                   
                    break;
                //下载PDF
                case "DownloadPDF":
                    DownloadPDF(context);
                    break;


                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;
            }
        }
        //获取合同模板信息
        private string contractTempList(ref string err, HttpContext context)
        {
            context.Response.ContentType = "application/Json";
            string tableName = string.Empty;
            string isInspect = context.Request.Params["isInspect"];
           string  sql=string.Empty;
            if (!string.IsNullOrEmpty(isInspect))//查询商检模板表
            {
                sql = " select *,case when isinline=1 then '是' else '否' end as inline  from Econtract_Inspect_template where 1=1 ";
            }
            else
            {
             sql = " select *,case when isinline=1 then '是' else '否' end as inline  from Econtract_template where 1=1 ";
            }
            string contractNo = (context.Request.Params["no"] ?? "").Trim();
            StringBuilder sb = new StringBuilder();
          
            sql += " and contractNo =@contractNo ";
            JsonHelperEasyUi js = new JsonHelperEasyUi();
            sb = js.GetDatatableJsonString(
                new StringBuilder(sql),
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@contractNo",contractNo)
                });
            return sb.ToString();
        }
        //获取模板填充数据
        private string LoadTemplateData(ref string err, HttpContext context)
        {
            string templateno = context.Request.QueryString["templateno"];
            string flowdirection = context.Request.QueryString["flowdirection"];
            if (string.IsNullOrEmpty(templateno))
            {
                throw new Exception("传入参数为空");
            }
            StringBuilder sb = new StringBuilder();
            if (flowdirection == ConstantUtil.IMPORT)
            {
                sb.Append(string.Format("select * from {0} where templateno='{1}'", ConstantUtil.TABLE_BTEMPLATE_IMPORTENCONTRACT, templateno));
            }
            else
            {
                sb.Append(string.Format("select * from {0} where templateno='{1}'", ConstantUtil.TABLE_BTEMPLATE_EXPORTENCONTRACT, templateno));
            }

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[0] { }, 0);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "没有找到数据！";
            }
            string result = JsonHelper.DataRowToJson_(dt.Rows[0]);
            result = result.Replace("&nbsp;", "");
            return result;
        }

        private string GetLogisiticsTempldatePVC(ref string err, HttpContext context)
        {
            string buyerCode = context.Request.Params["buyerCode"];
            string language = context.Request.Params["language"];
            string sellerCode = context.Request.Params["sellerCode"];
            string contractText = context.Request.Params["contractText"];
            string templateJson = context.Request.Params["tempJson"];
            string signedTime = context.Request.Params["signedTime"];
            string signedPlace = context.Request.Params["signedPlace"];
            string firstparty = context.Request.Params["firstparty"];
            string secondparty = context.Request.Params["secondparty"];
            string allItems = context.Request.Params["allItems"];
            string allItemswidth = context.Request.Params["allItemswidth"];
            string templatename = context.Request.Params["templatename"];

            string title = string.Empty;
            string bottom = string.Empty;
            string bankMessage = string.Empty;
            string tableName = string.Empty;
            string transporteng = "";
            string transportrus = "";
            string harborouteng = "";
            string harboroutrus = "";
            string harborarriveeng = "";
            string harborarriverus = "";
            string shipmenteng = "";
            string shipmengrus = "";
            string placementeng = "";
            string placementrus = "";
            string pricement1eng = "";
            string pricement1rus = "";
            string pricement2eng = "";
            string pricement2rus = "";

            StringBuilder sb = new StringBuilder();

            //获取模板详细
            System.Data.DataTable dt = new System.Data.DataTable();

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {

                //获取表头表尾
                title = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表头' and templateno='77600'").ToString();
                bottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾'  and templateno='77600'").ToString();
            }
            //替换变量

            Hashtable htdata = new Hashtable();
            if (!string.IsNullOrEmpty(signedTime))
            {
                htdata["中文:签订时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
                htdata["英文:签订时间"] = Convert.ToDateTime(signedTime).ToString("yyyy-MM-dd");
            }
            htdata["中文:签订地点"] = signedPlace.ToString();
            htdata["中文:甲方"] = firstparty.ToString();
            htdata["中文:乙方"] = secondparty.ToString();
            htdata["中文:模板名称"] = templatename.ToString();
            //卖方信息，买方信息，产品信息
            DataSet ds22 = new DataSet();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
             
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyerCode),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellerCode)
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            if (ds22.Tables[0].Rows.Count > 0)
            {
                DataRow dr1 = ds22.Tables[0].Rows[0];
                htdata["俄文:买方名"] = dr1["rsname"].ToString();
                htdata["英文:买方名"] = dr1["egname"].ToString();
                htdata["中文:买方名"] = dr1["name"].ToString();

                htdata["俄文:买方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:买方地址"] = dr1["egaddress"].ToString();
                htdata["中文:买方地址"] = dr1["address"].ToString();
                htdata["中文:开户银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:开户行名称"] = dr1["iegbank"].ToString();
                htdata["中文:开户银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:开户行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["中文:开户银行账号"] = dr1["icnaccount"].ToString();
                htdata["英文:开户银行号"] = dr1["iegaccount"].ToString();
            }
            if (ds22.Tables[1].Rows.Count > 0)
            {
                DataRow dr1 = ds22.Tables[1].Rows[0];
                htdata["俄文:卖方名"] = dr1["rsname"].ToString();
                htdata["英文:卖方名"] = dr1["egname"].ToString();
                htdata["中文:卖方名"] = dr1["name"].ToString();
                htdata["俄文:卖方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:卖方地址"] = dr1["egaddress"].ToString();
                htdata["中文:卖方地址"] = dr1["address"].ToString();
                htdata["英文:收款人银行名称"] = dr1["iegbank"].ToString();
                htdata["中文:收款人银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:收款人银行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:收款人银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:收款人银行账号"] = dr1["iegaccount"].ToString();
                htdata["中文:收款人银行账号"] = dr1["icnaccount"].ToString();
                htdata["中文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["英文:收款人银行行号"] = dr1["icncreditcode"].ToString();
            }
            //客户
            if (ds22.Tables[2].Rows.Count > 0)
            {
                DataRow drcus = ds22.Tables[2].Rows[0];
                htdata["不限:买方电话"] = drcus["phone"].ToString();
            }
            //供应商
            if (ds22.Tables[3].Rows.Count > 0)
            {
                DataRow drsup = ds22.Tables[3].Rows[0];
                htdata["不限:卖方电话"] = drsup["phone"].ToString();
            }
            htdata.Add("中文:合同号", "合同号");
            htdata.Add("英文:合同号", "Contract No.");

            htdata.Add("中文:日期", "日期");
            htdata.Add("英文:日期", "Date");

            htdata.Add("中文:合同", "合同");
            htdata.Add("英文:合同", "CONTRACT");
            htdata.Add("俄文:合同", "КОНТРАКТ");

            htdata.Add("俄文:卖方", "ПРОДАВЕЦ:");
            htdata.Add("英文:卖方", "The Seller:");
            htdata.Add("中文:卖方", "卖方:");

            htdata.Add("中文:银行", "银行:");
            htdata.Add("俄文:银行", "Банк:");

            htdata.Add("俄文:买方", "ПОКУПАТЕЛЬ:");
            htdata.Add("中文:买方", "买方:");
            htdata.Add("英文:买方", "The Buyer: ");

            htdata.Add("俄文:地址", "Реквизиты:");
            htdata.Add("中文:地址", "地址:");
            htdata.Add("英文:地址", "ADD:");
            string lans = "中文";
            //先添加表头
            StringBuilder titleSb = new StringBuilder(title);
            InsteadLabelString.Singleton.InsteadStringBuilder(titleSb, htdata, lans);
            string title1 = titleSb.ToString().Replace("\t", "").Replace("\r\n", "").Replace("<br />", "");
            title1 = title1.Replace("<p></p>", "");
            sb.AppendLine(title1.ToString());
            sb.Append("<br/>");
            sb.AppendLine(contractText.ToString());

            #region 生成表格
            //生成表格
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(templateJson);
            string[] itemsArray = allItems.Split('|');
            string[] itemswidthArray = allItemswidth.Split('|');//获取列宽
            StringBuilder sbpro = new StringBuilder();
            sbpro.Append(@"<table class='prodetail' border='1'  cellpadding=3 style='border-collapse:collapse;width:85%;height:100px;'>");
            //生成表头
            sbpro.Append("<tr>");
            for (int i = 0; i < itemsArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(itemsArray[i].ToString()))
                {
                    sbpro.Append("<td width='" + itemswidthArray[i] + "px'>" + itemsArray[i] + "</td>");
                }

            }
            sbpro.Append("</tr>");

            foreach (Hashtable hs in listtable)
            {
                sbpro.Append("<tr>");
                if (!string.IsNullOrEmpty(hs["item1"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item1"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item2"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item2"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item3"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item3"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item4"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item4"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item5"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item5"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item6"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item6"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item7"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item7"] + "</td>");
                }
                if (!string.IsNullOrEmpty(hs["item8"].ToString()))
                {
                    sbpro.Append("<td>" + hs["item8"] + "</td>");
                }
                sbpro.Append("</tr>");
            }
            sbpro.Append("</table>");
            sb.AppendLine(sbpro.ToString());
            #endregion

            //替换表尾
            StringBuilder bottomSb = new StringBuilder(bottom);
            InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
            sb.AppendLine(bottomSb.ToString());
            return sb.ToString();
        }
        //获取实时模板预览数据

        private string GetTempldatePVC(ref string err, HttpContext context)
        {
            string buyercode = context.Request.Params["buyercode"];
            string sellercode = context.Request.Params["sellercode"];
            string signedPlace = context.Request.Params["signedPlace"];
            string signedTime = "";
            string templateno = context.Request.Params["templateno"];
            string language = context.Request.Params["language"] ?? string.Empty;
            string validity = context.Request.Params["validity"] ?? string.Empty;
            string productlist = context.Request.Params["productlist"];
            string tradement = context.Request.Params["tradement"] ?? string.Empty;
            string transport = context.Request.Params["transport"] ?? string.Empty;
            string harborout = context.Request.Params["harborout"] ?? string.Empty;
            string harborarrive = context.Request.Params["harborarrive"] ?? string.Empty;
            string delivery = context.Request.Params["deliveryPlace"] ?? string.Empty;
            string pricement1 = context.Request.Params["pricement1"] ?? string.Empty;
            string pricement2 = context.Request.Params["pricement2"] ?? string.Empty;
            string pvalidity = context.Request.Params["pvalidity"] ?? string.Empty;
            string shipment = context.Request.Params["shipment"] ?? string.Empty;
            string placement = context.Request.Params["placement"] ?? string.Empty;
            string item1 = context.Request.Params["item1"] ?? string.Empty;
            string item2 = context.Request.Params["item2"] ?? string.Empty;
            string item3 = context.Request.Params["item3"] ?? string.Empty;
            string item4 = context.Request.Params["item4"] ?? string.Empty;
            string item5 = context.Request.Params["item5"] ?? string.Empty;
            string simpleBuyer = context.Request.Params["simpleBuyer"] ?? string.Empty;
            string simpleSeller = context.Request.Params["simpleSeller"] ?? string.Empty;
            string buyeraddress = context.Request.Params["buyeraddress"] ?? string.Empty;
            string selleraddress = context.Request.Params["selleraddress"] ?? string.Empty;
            string moreLanguage = context.Request.Params["moreLanguage"] ?? string.Empty;
            string templatejson = context.Request.Params["tempjson"] ?? string.Empty;
            string overspill = context.Request.Params["overspill"] ?? string.Empty;
            string shipDate = context.Request.Params["shipDate"] ?? string.Empty;
            string paymentType = context.Request.Params["paymentType"] ?? string.Empty;
            string title = string.Empty;
            string bottom = string.Empty;
            string bankMessage = string.Empty;
            string customerClassfic = string.Empty;
            string supplierClassfic = string.Empty;
            string bulling = string.Empty;
            string transporteng = "";
            string transportrus = "";
            string harborouteng = "";
            string harboroutrus = "";
            string harborarriveeng = "";
            string harborarriverus = "";
            string shipmenteng = "";
            string shipmengrus = "";
            string placementeng = "";
            string placementrus = "";
            string pricement1eng = "";
            string pricement1rus = "";
            string pricement2eng = "";
            string pricement2rus = "";
            string paymentTypeeng = "";
            string paymentTyperus = "";
            StringBuilder sb = new StringBuilder();
            //获取合同详细
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtContract = new System.Data.DataTable();
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                System.Data.SqlClient.SqlParameter[] mms = new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@templateno",templateno),
                   
                };
                dt = bll.ExecDatasetSql(@" select templateno, sortno, chncontent, engcontent, ruscontent, isinline from btemp_detail where templateno=@templateno  order by sortno;
                   ", mms).Tables[0];
                title = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表头'  and templateno='0603'").ToString();
                bottom = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='表尾' and templateno='0603'").ToString();
                bankMessage = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='银行信息'").ToString();
                bulling = bll.ExecuteScalar(@"select content from btemplate_contract where templatename='开票信息' and templateno='0603'").ToString();
            }

            #region 填充hashtable
            Hashtable htdata = new Hashtable();

            //htdata["中文:签订时间"] = Convert.ToDateTime(drContract["signedtime"]).ToString("yyyy年MM月dd天");
            htdata["中文:签订时间"] = signedTime;
            htdata["英文:签订时间"] = signedTime;
            htdata["中文:签订地点"] = signedPlace.ToString();
            htdata["不限:签订时间"] = signedTime;
            htdata["不限:签订地点"] = signedPlace.ToString();
            htdata["不限:批次备注"] = "";
            //卖方信息，买方信息，产品信息
            DataSet ds22 = new DataSet();
            string sellerName = string.Empty;
            string sellerChinaAddress = string.Empty;
            string sellerEngAddress = string.Empty;
            //银行信息
            string iegbank = string.Empty;
            string iegaddress = string.Empty;
            string iegname = string.Empty;
            string iegaccount = string.Empty;
            string sellerPhone = string.Empty;
            string swiftCode = string.Empty;

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                StringBuilder sb22 = new StringBuilder();
                sb22.Append(" select * from bcustomer where code=@cuscode; ");
                sb22.Append(" select * from bsupplier where code=@supcode; ");
                //sb22.Append(" select * from Econtract_ap where contractNo=@contractno and attachmentno=''; ");
                sb22.Append(" select * from bcustomer_contact where code=@cuscode; ");
                sb22.Append(" select * from bsupplier_contact where code=@supcode; ");

                System.Data.SqlClient.SqlParameter[] pps = new System.Data.SqlClient.SqlParameter[]{
                   //new  System.Data.SqlClient.SqlParameter("@contractno",contractNo),
                   new  System.Data.SqlClient.SqlParameter("@cuscode",buyercode),
                   new  System.Data.SqlClient.SqlParameter("@supcode",sellercode)
                };
                ds22 = bll.ExecDatasetSql(sb22.ToString(), pps);
            }
            if (ds22.Tables[0].Rows.Count > 0)
            {
                //客户
                DataRow dr1 = ds22.Tables[0].Rows[0];
                htdata["俄文:买方名"] = dr1["rsname"].ToString();
                htdata["英文:买方名"] = dr1["egname"].ToString();
                htdata["中文:买方名"] = dr1["name"].ToString();
                htdata["俄文:买方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:买方地址"] = dr1["egaddress"].ToString();
                htdata["中文:买方地址"] = dr1["address"].ToString();
                htdata["中文:客户地址及电话"] = dr1["icnaddress"].ToString() + dr1["icnphone"].ToString();
                htdata["英文:客户地址及电话"] = dr1["iegaddress"].ToString() + dr1["iegphone"].ToString();
                htdata["中文:开户银行"] = dr1["icnbank"].ToString();
                htdata["英文:开户银行"] = dr1["iegbank"].ToString();
                htdata["中文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:社会信用代码"] = dr1["icncreditcode"].ToString();
                htdata["中文:开户银行账号"] = dr1["icnaccount"].ToString();
                htdata["英文:开户银行号"] = dr1["iegaccount"].ToString();
                customerClassfic = dr1["classific"].ToString();
            }
            if (ds22.Tables[1].Rows.Count > 0)
            {
                //供应商
                DataRow dr1 = ds22.Tables[1].Rows[0];
                htdata["俄文:卖方名"] = dr1["rsname"].ToString();
                htdata["英文:卖方名"] = dr1["egname"].ToString();
                htdata["中文:卖方名"] = dr1["name"].ToString();
                sellerName = dr1["name"].ToString();
                sellerChinaAddress = dr1["address"].ToString();
                sellerEngAddress = dr1["egaddress"].ToString();
                htdata["俄文:卖方地址"] = dr1["rsaddress"].ToString();
                htdata["英文:卖方地址"] = dr1["egaddress"].ToString();
                htdata["中文:卖方地址"] = dr1["address"].ToString();
                htdata["英文:收款银行名称"] = dr1["iegbank"].ToString();
                htdata["中文:收款银行名称"] = dr1["icnbank"].ToString();
                htdata["英文:收款人银行地址"] = dr1["iegaddress"].ToString();
                htdata["中文:收款人银行地址"] = dr1["icnaddress"].ToString();
                htdata["英文:收款人银行账号"] = dr1["iegaccount"].ToString();
                htdata["中文:收款人银行账号"] = dr1["icnaccount"].ToString();
                htdata["中文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["英文:收款人银行行号"] = dr1["icncreditcode"].ToString();
                htdata["中文:卖方信用代码"] = dr1["icncreditcode"].ToString();
                htdata["英文:卖方信用代码"] = dr1["icncreditcode"].ToString();
                iegbank = dr1["iegbank"].ToString();
                iegaddress = dr1["iegaddress"].ToString();
                iegname = dr1["iegname"].ToString();
                iegaccount = dr1["iegaccount"].ToString();
                sellerPhone = dr1["icnphone"].ToString();
                swiftCode = dr1["icncreditcode"].ToString();
                supplierClassfic = dr1["classific"].ToString();
            }
            //客户
            if (ds22.Tables[2].Rows.Count > 0)
            {
                DataRow drcus = ds22.Tables[2].Rows[0];
                htdata["不限:买方电话"] = drcus["phone"].ToString();
            }
            //供应商
            if (ds22.Tables[3].Rows.Count > 0)
            {
                DataRow drsup = ds22.Tables[3].Rows[0];
                htdata["不限:卖方电话"] = drsup["phone"].ToString();
            }
            htdata["中文:收款人银行名称"] = iegbank;
            htdata["中文:编码"] = swiftCode;
            htdata["中文:收款人支行地址"] = iegaddress;
            htdata["中文:支行号"] = iegaccount;
            htdata.Add("中文:合同号", "合同号");
            htdata.Add("英文:合同号", "Contract No.");

            htdata.Add("中文:合同有效期", validity.ToString().Length == 0 ? "" : validity.ToString());
            // Convert.ToDateTime(drContract["validity"]).ToString("yyyy 年 MM 月 dd 日"));
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
            htdata.Add("英文:合同有效期", validity.ToString().Length == 0 ? "" : validity.ToString());
            //Convert.ToDateTime(drContract["validity"]).ToString(cultureinfo));

            htdata.Add("中文:日期", "日期");
            htdata.Add("英文:日期", "Date:");

            htdata.Add("中文:合同", "合同:");
            htdata.Add("英文:合同", "CONTRACT:");
            htdata.Add("俄文:合同", "КОНТРАКТ:");

            htdata.Add("俄文:卖方", "ПРОДАВЕЦ:");
            htdata.Add("英文:卖方", "The Seller:");
            htdata.Add("中文:卖方", "卖方:");

            htdata.Add("中文:银行", "银行:");
            htdata.Add("俄文:银行", "Банк:");

            htdata.Add("俄文:买方", "ПОКУПАТЕЛЬ:");
            htdata.Add("中文:买方", "买方:");
            htdata.Add("英文:买方", "The Buyer:");

            htdata.Add("俄文:地址", "Реквизиты:");
            htdata.Add("中文:地址", "地址:");
            htdata.Add("英文:地址", "ADD:"); 
            #endregion



            string lans = language.ToString();
            if (!string.IsNullOrEmpty(moreLanguage))
            {
                lans = moreLanguage;
            }
            //先添加表头
            StringBuilder titleSb = new StringBuilder(title);
            InsteadLabelString.Singleton.InsteadStringBuilder(titleSb, htdata, lans);
            string title1 = titleSb.ToString().Replace("\t", "").Replace("\r\n", "");
            title1 = title1.Replace("<p></p>", "");
            sb.AppendLine(title1.ToString());
            #region 生成产品明细表
            //动态生成产品表格
            string productTitle = "";
            StringBuilder sbpro = new StringBuilder();
            string th0 = "";
            string th1 = "";
            string th2 = "";
            string th3 = "";
            string th4 = "";
            string th5 = "";
            string th6 = "";
            string th7 = "";
            string th8 = "";
            string totalabel = "";
            if (lans.Contains("英"))
            {
                productTitle = "ProductDetails<br/>";
                th0 = "DESCRIPTION<br/>";
                th1 = "UNIT<br/>";
                th2 = "QUANTITY<br/>";
                th3 = "UNIT PRICE<br/>";
                th4 = "PRICEUNIT<br/>";
                th5 = "AMOUNT<br/>";
                th6 = "Mark<br/>";
                th7 = "Packages<br/>";
                th8 = "Spec<br/>";
            }
            if (lans.Contains("俄"))
            {
                productTitle = "Список продуктов<br/> ";
                th0 = "Описание товаров <br/>";
                th1 = " единица  <br/>";
                th2 = "количество <br/>";
                th3 = " цена за единицу <br/>";
                th4 = "валюты  <br/>";
                th5 = " цена  <br/>";
                th6 = " отгрузочная маркировка <br/>";
                th7 = " упаковка <br/>";
                th8 = "спецификации <br/>";
            }
            if (lans.Contains("中"))
            {
                productTitle += "产品明细";
                th0 += "货物描述";
                th1 += "单位";
                th2 += "数量";
                th3 += "单价";
                th4 += "币种";
                th5 += "总价";
                th6 += "唛头";
                th7 += "包装";
                th8 += "规格";
            }
            sbpro.AppendLine(productTitle);
            sbpro.Append(@"<table class='prodetail' border='1'cellpadding=1 style='width:500px;'>");
            //生成产品明细表

            sbpro.Append("<tr style='height:50px';>");
            sbpro.Append("<td style='width:2px'>" + th6 + "</td>");
            sbpro.Append("<td>" + th0 + "</td>");
            sbpro.Append("<td>" + th8 + "</td>");
            sbpro.Append("<td>" + th7 + "</td>");
            sbpro.Append("<td>" + th1 + "</td>");
            sbpro.Append("<td>" + th2 + "</td>");
            sbpro.Append("<td>" + th3 + "</td>");
            sbpro.Append("<td>" + th4 + "</td>");
            sbpro.Append("<td>" + th5 + "</td>");
            sbpro.Append("</tr>");
            for (int i = 0; i < 2; i++)
            {
                sbpro.Append("<tr style='height:50px'>");
                sbpro.Append("<td>&nbsp;</td>");
                sbpro.Append("<td></td>");
                sbpro.Append("<td></td>");
                sbpro.Append("<td></td>");
                sbpro.Append("<td></td>");
                sbpro.Append("<td></td>");
                sbpro.Append("<td></td>");
                sbpro.Append("<td></td>");
                sbpro.Append("<td></td>");
                sbpro.Append("<td></td>");
                sbpro.Append("</tr>");
            }
            if (lans.Contains("英文"))
            {
                totalabel = "Total Value<br/>";
            }
            if (lans.Contains("俄文"))
            {
                totalabel = "Обшая сумма<br/>";
            }
            if (lans.Contains("中文"))
            {
                totalabel += "总金额";
            }
            sbpro.Append("<tr style='height:50px'>");
            sbpro.Append("<td>" + totalabel + "</td>");
            sbpro.Append("<td colspan='8' ></td>");
            sbpro.Append("</tr>");
            sbpro.Append("</table>");
            //sb.Append(sbpro.ToString());

            #endregion
            BtemplateBLL templateBll = new BtemplateBLL();

            if (!string.IsNullOrEmpty(item1))
            {
                sb.AppendLine("<span style='color:red'>" + item1 + "</span><br/>");
            }
            if (!string.IsNullOrEmpty(item2))
            {
                sb.AppendLine("<span style='color:red'>" + item2 + "</span>  <br />");
            }
            if (!string.IsNullOrEmpty(item3))
            {
                sb.AppendLine("<span style='color:red'>" + item3 + "</span>  <br />");
            }
            if (!string.IsNullOrEmpty(item4))
            {
                sb.AppendLine("<span style='color:red'>" + item4 + "</span>  <br />");
            }
            if (!string.IsNullOrEmpty(item5))
            {
                sb.AppendLine("<span style='color:red'>" + item5 + "</span>  <br />");
            }
            if (!string.IsNullOrEmpty(templatejson))
            {
                /*
                 * 对List<Hashtable>进行排序
                 * 
                 * 方法1：
                 *       List<HashTableExp> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<HashTableExp>(templatejson);
                 *       templateTable.Sort();
                 *       
                 * 方法2：
                 *       List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(templatejson);
                 *       HashtableComparer htComparer = new HashtableComparer(sort,sorttype,datatype);
                 *       templateTable.Sort(htComparer);
                 * */
                List<Hashtable> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(templatejson);
                HashtableComparer htComparer = new HashtableComparer("sortno","asc","NUM");
                templateTable.Sort(htComparer);
                //List<HashTableExp> templateTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<HashTableExp>(templatejson);
                //templateTable.Sort();
                StringBuilder sb33 = templateBll.GetContractTermsByJson(lans, templateTable);
                contractBll.getEngRusSting(transport, harborout, harborarrive, shipment, placement, pricement1, pricement2, paymentType, ref transporteng, ref transportrus,
                  ref harborouteng, ref harboroutrus, ref harborarriveeng, ref harborarriverus, ref shipmenteng, ref shipmengrus, ref placementeng,
              ref placementrus, ref pricement1eng, ref pricement1rus, ref pricement2eng, ref pricement2rus, ref paymentTypeeng, ref paymentTyperus);
                //替换标签文字 变量条款
                InsteadLabelString.Singleton.InsteadStringBuilder(sb33, htdata, lans);
                sb33 = sb33
                   .Replace(ConstantUtil.TEMP_TRADEMENT, tradement).Replace(ConstantUtil.TEMP_TRADEMENTENG, tradement).Replace(ConstantUtil.TEMP_TRADEMENTRUS, tradement)//贸易条款
                   .Replace(ConstantUtil.TEMP_TRANSPORT, transport).Replace(ConstantUtil.TEMP_TRANSPORTENG, transporteng).Replace(ConstantUtil.TEMP_TRANSPORTRUS, transportrus)//运输方式
                   .Replace(ConstantUtil.TEMP_EXPORTHARBOR, harborout).Replace(ConstantUtil.TEMP_EXPORTHARBORENG, harborouteng).Replace(ConstantUtil.TEMP_EXPORTHARBORRUS, harboroutrus)//出口口岸
                   .Replace(ConstantUtil.TEMP_ARRIVEHARBOR, harborarrive).Replace(ConstantUtil.TEMP_ARRIVEHARBORENG, harborarriveeng).Replace(ConstantUtil.TEMP_ARRIVEHARBORRUS, harborarriverus)//到货口岸
                   .Replace(ConstantUtil.TEMP_PRICEMENT1, pricement1).Replace(ConstantUtil.TEMP_PRICEMENT1ENG, pricement1eng).Replace(ConstantUtil.TEMP_PRICEMENT1RUS, pricement1rus)//价格条款1
                   .Replace(ConstantUtil.TEMP_PRICEMENT2, pricement2).Replace(ConstantUtil.TEMP_PRICEMENT2ENG, pricement2eng).Replace(ConstantUtil.TEMP_PRICEMENT2RUS, pricement2rus)//价格条款2
                   .Replace(ConstantUtil.TEMP_PVALIDITY, pvalidity).Replace(ConstantUtil.TEMP_PVALIDITYENG, pvalidity).Replace(ConstantUtil.TEMP_PVALIDITYRUS, pvalidity)//价格有效期
                   .Replace(ConstantUtil.TEMP_VALIDITY, validity).Replace(ConstantUtil.TEMP_VALIDITYENG, validity).Replace(ConstantUtil.TEMP_VALIDITYRUS, validity)//合同有效期
                   .Replace(ConstantUtil.TEMP_SHIPMENT, shipment).Replace(ConstantUtil.TEMP_SHIPMENTENG, shipmenteng).Replace(ConstantUtil.TEMP_SHIPMENTRUS, shipmengrus)//发运条款
                   .Replace(ConstantUtil.TEMP_PLACEMENT, placement).Replace(ConstantUtil.TEMP_PLACEMENTENG, placement).Replace(ConstantUtil.TEMP_PLACEMENT, placement)//产地条款
                   .Replace(ConstantUtil.TEMP_IMPORTHARBOR, harborout).Replace(ConstantUtil.TEMP_IMPORTHARBORENG, harborouteng).Replace(ConstantUtil.TEMP_IMPORTHARBORRUS, harboroutrus)//进口口岸
                   .Replace(ConstantUtil.TEMP_OVERSPILL, overspill).Replace(ConstantUtil.TEMP_OVERSPILLENG, overspill).Replace(ConstantUtil.TEMP_OVERSPILLRUS, overspill)//溢出率
                   .Replace(ConstantUtil.TEMP_PAYLASTDATE, shipment).Replace(ConstantUtil.TEMP_PAYLASTDATEENG, shipment).Replace(ConstantUtil.TEMP_PAYLASTDATERUS, shipment)//付款截止日
                   .Replace(ConstantUtil.TEMP_SHIPDATE, shipDate).Replace(ConstantUtil.TEMP_SHIPDATEENG, shipDate).Replace(ConstantUtil.TEMP_SHIPDATERUS, shipDate)//发运日期
                   .Replace(ConstantUtil.TEMP_PAYMENTTYPE, paymentType).Replace(ConstantUtil.TEMP_PAYMENTTYPEENG, paymentTypeeng).Replace(ConstantUtil.TEMP_PAYMENTTYPERUS, paymentTyperus);//付款方式
                if (lans == "中文-英文")
                {
                    sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, sbpro.ToString()).Replace(ConstantUtil.TEMP_PRODUCTENG, "").Replace(ConstantUtil.TEMP_PRODUCTRUS, "");//产品表格
                }
                if (lans == "中文")
                {
                    sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, sbpro.ToString()).Replace(ConstantUtil.TEMP_PRODUCTENG, "").Replace(ConstantUtil.TEMP_PRODUCTRUS, "");//产品表格
                }
                if (lans == "英文")
                {
                    sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, "").Replace(ConstantUtil.TEMP_PRODUCTENG, sbpro.ToString()).Replace(ConstantUtil.TEMP_PRODUCTRUS, "");//产品表格
                }
                if (lans == "中文-俄文")
                {
                    sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, sbpro.ToString()).Replace(ConstantUtil.TEMP_PRODUCTENG, "").Replace(ConstantUtil.TEMP_PRODUCTRUS, "");//产品表格
                }
                if (lans == "俄文")
                {
                    sb33 = sb33.Replace(ConstantUtil.TEMP_PRODUCT, "").Replace(ConstantUtil.TEMP_PRODUCTENG, "").Replace(ConstantUtil.TEMP_PRODUCTRUS, sbpro.ToString());//产品表格
                }
                sb.AppendLine(sb33.ToString());
            }
            //替换表尾
            if (customerClassfic == ConstantUtil.IMPORT)
            {
                StringBuilder bottomSb = new StringBuilder(bottom);
                InsteadLabelString.Singleton.InsteadStringBuilder(bottomSb, htdata, lans);
                sb.AppendLine(bottomSb.ToString());
            }
            else
            {
                StringBuilder bullingSb = new StringBuilder(bulling);
                InsteadLabelString.Singleton.InsteadStringBuilder(bullingSb, htdata, lans);
                sb.AppendLine(bullingSb.ToString());
            }
            return sb.ToString();
        }

        private string logisticsItems(ref string err, HttpContext context)
        {
            string logisticsTemplateno = context.Request.Params["logisticsTemplateno"];

            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            sqldata.Append(@" select *  from btemplate_logisticsItems where  logisticsTemplateno=@logisticsTemplateno  ");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@logisticsTemplateno",Value=logisticsTemplateno,DbType=DbType.String},
             
                };
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        //获取模板管理列表
        private string templatelist(ref string err, HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string templatename = context.Request.Params["templatename"];
            string templateCategory = context.Request.Params["templateCategory"];
            string category = context.Request.Params["category"];
            string productCategory = context.Request.Params["productCategory"];
            //获取用户名选择加载模板列表,为管理员则加载所有模板列表
            string createman = RequestSession.GetSessionUser().UserName.ToString();
            StringBuilder sqlshere = new StringBuilder("  1=1");
            StringBuilder sqlcount = new StringBuilder();
            if (templatename != null && templatename.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.templatename like '%'+@templatename+'%' ");
            }
            if (category != null && category.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection like '%'+@category+'%' ");
            }
            if (templateCategory != null && templateCategory.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.templateCategory=@templateCategory");
            }
            if (productCategory != null && productCategory.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.productCategory=@productCategory ");
            }
            if (createman != null && createman != "管理员")
            {
                sqlshere.Append(" and createmanname=@createman");
            }

            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            if (category == ConstantUtil.IMPORT)//进境
            {
                sqldata.Append(" select * from btemplate_importEcontract t1 where " + sqlshere);
                sqlcount.Append("select count(1) from btemplate_importEcontract t1 where " + sqlshere.ToString());
            }
            else if (category == ConstantUtil.EXPORT)//出境
            {
                sqldata.Append(" select * from btemplate_exportEcontract t1 where " + sqlshere);
                sqlcount.Append("select count(1) from btemplate_exportEcontract t1 where " + sqlshere.ToString());
            }


            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@createman",createman),
                    new SqlParameter("@templatename",templatename),
                    new SqlParameter("@templateCategory",templateCategory),
                    new SqlParameter("@productCategory",productCategory),
                    new SqlParameter("@category",category),
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }

        private string logisticsTemplatelist(ref string err, HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();

            StringBuilder sqlshere = new StringBuilder("  1=1");
            StringBuilder sqlcount = new StringBuilder();

            sqlshere.Append(" and createman=@createman");
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            sqldata.Append(" select * from btemplate_logistics t1 where " + sqlshere);
            sqlcount.Append("select count(1) from btemplate_logistics t1 where " + sqlshere.ToString());

            string createman = RequestSession.GetSessionUser().UserName.ToString();
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@createman",createman),
                
                };
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            return sb.ToString();
        }
        private string logisticsTemplatelist1(ref string err, HttpContext context)
        {

            StringBuilder sqlshere = new StringBuilder("  1=1");
            StringBuilder sqlcount = new StringBuilder();

            sqlshere.Append(" and createman=@createman");
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            sqldata.Append(" select * from btemplate_logistics t1 where " + sqlshere);
            string createman = RequestSession.GetSessionUser().UserName.ToString();
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@createman",createman),
                
                };
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        //根据语言加载不同的表尾信息
        private string splitDiffInfo(string info, string language)
        {
            //分割条款条数
            string[] infoArray = info.Split('&');
            for (int i = 0; i < infoArray.Length; i++)
            {
                //每一条条款根据语言分割
                if (language == "中文")
                {

                }
                //string[] mationArray = infoArray[i].Split('|');
                //for (int j = 0; j < mationArray.Length; j++)
                //{

                //}

            }
            return "";

        }
        /// <summary>
        /// 导出pdf(文件)
        /// FileName：文件名称
        /// content：导出html片段
        /// isWaterMark：是否添加水印 0：不添加，1：添加
        /// flag：是否删除 默认0：不删；1：:删除
        /// </summary>
       
        private string  ExportPDF(HttpContext context,string tableName=ConstantUtil.TABLE_ECONTRACT) 
        {
           
            string filename = "";
            string html = (context.Request["html"] ?? "").ToString();
            string contractNo = (context.Request["contractNo"] ?? "").ToString();
            string htmlcontent = (context.Request["htmlcontent"] ?? "").ToString();
            //flag 1为纵向，0为横向
            string flag = context.Request.Params["flag"] ?? string.Empty;
            if (string.IsNullOrEmpty(flag))
            {
                flag = "0";
            }
            //htmlcontent = "<div style=margin-top:100px;margin-left:20px;margin-right:20px;><p>" + htmlcontent + "</p></div>";
            htmlcontent = htmlcontent.Replace("<br>", "</br>");
  
            html ="<?xml version=\"1.0\" encoding=\"UTF-8\"?>  <!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">  <html xmlns=\"http://www.w3.org/1999/xhtml\">  <head>   <meta http-equiv=\"Content-Type\" content=\"text/html;charset=UTF-8\"></meta> <title></title>"+html;
            html += "</head><body>" + htmlcontent + "</body></html>";
            html=html.Replace("<br>", "</br>");
            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select * from " +tableName+ " where contractNo='" + contractNo + "'");
            string approvalStatus = DataFactory.SqlDataBase().getString(sb_sql, "status");
            if (ConstantUtil.STATUS_STOCKIN_CHECK1.Equals(approvalStatus))
            {
              
               filename= PdfHelper.HtmlToPdf("", html, "Themes\\Images\\审批通过151.png", 1,1,Convert.ToInt32(flag));
            }
            else
            {
                filename = PdfHelper.HtmlToPdf("", html, "Themes\\Images\\审批通过151.png", 0, 1, Convert.ToInt32(flag));
            }

            return "{\"filename\":\""+ filename +"\",\"flag\":\"1\"}";
        }
        private void DownloadPDF(HttpContext context) 
        {
            string filename = (context.Request["filename"] ?? "").ToString();
            string flag_ = (context.Request["flag"] ?? "0").ToString();
            int flag = int.Parse(flag_);

            FileDownHelper.DownLoadold("\\Files\\PDF\\" + filename, flag);
            
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}