using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace RM.Web.ashx.PurchaseContract
{
    /// <summary>
    /// LoadData 的摘要说明
    /// </summary>
    public class LoadData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string module = context.Request["module"];

            #region 销售合同附件编号
            if (module == "attach")
            {
                string contractNo = context.Request["contractNo"];

                SqlParameter[] sqlpps = new SqlParameter[]
            {
                new SqlParameter("@contractNo", contractNo),
              
            };
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                    DataTable attachment = bll.ExecDatasetSql("select attachmentno from Econtract_a where contractNo=@contractNo", sqlpps).Tables[0];
                    StringBuilder attach = ui.ToEasyUIComboxJson(attachment);
                    context.Response.Write(attach.ToString());

                }

            }
            #endregion

            #region 合同列表
            if (module == "htpagelist")  //合同列表
            {
                //获取分页变量
                int row = int.Parse(context.Request["rows"].ToString());
                int page = int.Parse(context.Request["page"].ToString());
                string order = context.Request["order"].ToString();
                string sort = context.Request["sort"].ToString();

                string contractNo = context.Request.Params["contractNo"];
                string purchaseCode = context.Request.Params["purchaseCode"];
                string signedtime_begin = context.Request.Params["signedtime_begin"];
                string signedtime_end = context.Request.Params["signedtime_end"];

                if (signedtime_end != null && signedtime_end.Trim().Length > 0)
                {
                    signedtime_end = Convert.ToDateTime(Convert.ToDateTime(signedtime_end).ToString("yyyy-MM-dd")).AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                StringBuilder sqldata = new StringBuilder();
                StringBuilder sqlcount = new StringBuilder();
                StringBuilder sqlshere = new StringBuilder(" 1=1 ");
                if (contractNo != null && contractNo.Trim().Length > 0)
                {
                    sqlshere.Append(" and t2.contractNo like '%'+@contractNo+'%' ");
                }
                if (purchaseCode != null && purchaseCode.Trim().Length > 0)
                {
                    sqlshere.Append(" and t1.purchaseCode like '%'+@purchaseCode+'%' ");
                }
                if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
                {
                    sqlshere.Append(" and t2.signedtime>=@signedtime_begin");
                }
                if (signedtime_end != null && signedtime_end.Trim().Length > 0)
                {
                    sqlshere.Append(" and t2.signedtime<=@signedtime_end ");
                }
                if (context.Request["review"] == "review")
                {
                    sqlshere.Append("and t2.status!='新建'");
                }
                sqldata.Append(@"select  contractNo, saleCode, saleAttachCode, contracttype, contracttype2, businessclass, language, seller, signedtime, signedplace, buyer, buyeraddress, currency, pricement1, pricement1per, pricement2, pricement2per, pvalidity, shipment, transport, tradement, harborout, harborarrive, harborclear, placement, validity, remark, templateno, status, createman, createdate, lastmod, lastmoddate
from  PurchaseContract t2 

where " + sqlshere.ToString());
                sqlcount.Append(@"select count(1)from  PurchaseContract t2  where " + sqlshere.ToString());
                SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@purchaseCode",Value=purchaseCode,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String}
                };

                StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            #endregion

            #region 产品列表
            if (module == "cplist")
            {
                //获取分页变量
                int row = int.Parse(context.Request["rows"].ToString());
                int page = int.Parse(context.Request["page"].ToString());
                string order = context.Request["order"].ToString();
                string sort = context.Request["sort"].ToString();

                //where 参数

                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                StringBuilder sqldata = new StringBuilder();
                StringBuilder sqlcount = new StringBuilder();
                sqldata.Append(" select * from bproduct where 1=1");
                sqlcount.Append("select count(1) from bproduct where 1=1");
                SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    //new SqlParameter("@contractNo",contarctno),
                };

                StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            #endregion

            #region 合同内产品列表
            if (module == "htcppagelist")   //合同附件产品列表
            {
                //where 参数
                string contarctno = context.Request.Params["contractNo"];
                string attachmentno = context.Request.Params["attachmentno"];
                string isall = context.Request.Params["isall"];

                string where = "";
                if (isall == "1")
                {
                    where = " contractNo=@contractNo and attachmentno='' ";
                }
                else
                {
                    where = " contractNo=@contractNo and attachmentno=@attachmentno ";
                }

                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                StringBuilder sqldata = new StringBuilder();
                StringBuilder sqlcount = new StringBuilder();
                sqldata.Append(" select * from PurchaseContract_ap where " + where);
                SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contarctno),
                    new SqlParameter("@attachmentno",attachmentno),
                };

                StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            #endregion

            #region 合同附件列表
            else if (module=="htpagelistfj")
            {
                  //获取分页变量
                int row = int.Parse(context.Request["rows"].ToString());
                int page = int.Parse(context.Request["page"].ToString());
                string order = context.Request["order"].ToString();
                string sort = context.Request["sort"].ToString();

                string contractNo = context.Request.Params["contractNo"];
                string attachmentno = context.Request.Params["attachmentno"];
                string signedtime_begin = context.Request.Params["signedtime_begin"];
                string signedtime_end = context.Request.Params["signedtime_end"];

                if (signedtime_end!=null && signedtime_end.Trim().Length > 0)
                {
                    signedtime_end = Convert.ToDateTime(Convert.ToDateTime(signedtime_end).ToString("yyyy-MM-dd")).AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                StringBuilder sqldata = new StringBuilder();
                StringBuilder sqlcount = new StringBuilder();
                StringBuilder sqlshere = new StringBuilder(" 1=1 ");
                if (contractNo != null && contractNo.Trim().Length > 0)
                {
                    sqlshere.Append(" and t2.contractNo like '%'+@contractNo+'%' ");
                }
                if (attachmentno != null && attachmentno.Trim().Length > 0)
                {
                    sqlshere.Append(" and t1.attachmentno like '%'+@attachmentno+'%' ");
                }
                if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
                {
                    sqlshere.Append(" and t1.signedtime>=@signedtime_begin");                    
                }
                if (signedtime_end != null && signedtime_end.Trim().Length > 0)
                {
                    sqlshere.Append(" and t1.signedtime<=@signedtime_end ");
                }
                sqldata.Append(@" select t1.status as status1,t1.*,t2.contracttype,t2.contracttype2,t2.businessclass,
t3.cname as currency1,t4.cname as transport1,t5.name as harborout1,t6.name as harborarrive1,t7.name as harborclear1 
from PurchaseContract_a t1 left join PurchaseContract t2 on t1.contractNo=t2.contractNo
                left join bdicdate t3 on t1.currency=t3.code and t3.classname='货币'
                left join bdicdate t4 on t1.transport=t4.code and t4.classname='运输方式'
                left join bharbor t5 on t1.harborout=t5.code 
                left join bharbor t6 on t1.harborarrive=t6.code 
                left join bharbor t7 on t1.harborclear=t7.code
where " + sqlshere.ToString());
                sqlcount.Append(@"select count(1)  from PurchaseContract_a t1 left join PurchaseContract t2 on t1.contractNo=t2.contractNo  where " + sqlshere.ToString());
                SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@attachmentno",Value=attachmentno,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String}
                };

                StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            #endregion

            #region 合同附件加载合同编号
            else if (module == "htlist")
            {
                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                StringBuilder sqldata = new StringBuilder();
                sqldata.Append(" select contractNo,language,seller,buyer from PurchaseContract where 1=1");
                SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    //123
                };
                StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            #endregion

            #region 合同附件产品列表
            else if (module == "htcppagelistfj")   //合同附件产品列表
            {
                //where 参数
                string contarctno = context.Request.Params["contractNo"];
                string attachmentno = context.Request.Params["attachmentno"];

                string where = "";
                where = " contractNo=@contractNo and attachmentno=@attachmentno ";

                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                StringBuilder sqldata = new StringBuilder();
                StringBuilder sqlcount = new StringBuilder();
                sqldata.Append(" select * from PurchaseContract_ap where " + where);
                SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contarctno),
                    new SqlParameter("@attachmentno",attachmentno),
                };

                StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            #endregion
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