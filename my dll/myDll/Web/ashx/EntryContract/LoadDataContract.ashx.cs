using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace RM.Web.ashx.EntryContract
{
    /// <summary>
    /// ChangeContract 的摘要说明
    /// </summary>
    public class ChangeContract : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //从后台获取数据
            context.Response.ContentType = "application/json";
            string module = context.Request["module"];

            #region 合同列表
            if (module == "htpagelist")  //合同列表
            {
                //获取分页变量
                int row = int.Parse(context.Request["rows"].ToString());
                int page = int.Parse(context.Request["page"].ToString());
                string order = context.Request["order"].ToString();
                string sort = context.Request["sort"].ToString();

                string contractNo = context.Request.Params["contractNo"];
                string econtractNo = context.Request.Params["econtractNo"];
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
                //if (attachmentno != null && attachmentno.Trim().Length > 0)
                //{
                //    sqlshere.Append(" and t1.attachmentno like '%'+@attachmentno+'%' ");
                //}
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
                sqldata.Append(@"select contractNo, econtractNo,attachmentNo, businessclass, seller, signedtime, signedplace, buyer, buyeraddress, currency, pricement1, pricement1per, pricement2, pricement2per, pvalidity, shipment, transport, tradement, harborout, harborarrive, harborclear, placement, validity, remark, templateno, status, createman, createdate, lastmod, lastmoddate
from  Icontract t2 

where " + sqlshere.ToString());
                sqlcount.Append(@"select count(1)from  Icontract t2  where " + sqlshere.ToString());
                SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@attachmentno",Value=econtractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String}
                };

                StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            #endregion

            #region 境外合同附件编号
            else if (module == "attach")
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

            #region 加载已有产品列表
            else if (module == "htcppagelist")
            {
                //where 参数
                string contarctno = context.Request.Params["contractNo"];
                string attachmentno = context.Request.Params["attachmentno"];
                string isall = context.Request.Params["isall"];

                string where = "";
                if (isall == "1")
                {
                    where = " contractNo=@contractNo";
                }
                else
                {
                    where = " contractNo=@contractNo and attachmentno=@attachmentno ";
                }

                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                StringBuilder sqldata = new StringBuilder();
                StringBuilder sqlcount = new StringBuilder();
                sqldata.Append(" select * from Icontract_ap where " + where);
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

            #region 产品列表
            else if (module == "cplist")   //合同产品列表
            {
                //获取分页变量
                int row = int.Parse(context.Request["rows"].ToString());
                int page = int.Parse(context.Request["page"].ToString());
                string order = context.Request["order"].ToString();
                string sort = context.Request["sort"].ToString();
                string attachmentno = context.Request["attachmentno"]??"";
                string contractNo = context.Request["contractNo"];
                string isall = context.Request["isall"];
                //where 参数

                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                StringBuilder sqldata = new StringBuilder();
                StringBuilder sqlcount = new StringBuilder();
                //从境外产品表中加载产品,不选择附件编号
                if (isall == "1")
                {
                    sqldata.Append(" select * from Econtract_ap where contractNo=@contractNo");
                    sqlcount.Append("select count(1) from Econtract_ap where contractNo=@contractNo");
                }
               //选择附件编号
                else
                {
                    sqldata.Append(" select * from Econtract_ap where contractNo=@contractNo and attachmentno=@attachmentno");
                    sqlcount.Append("select count(1) from Econtract_ap where contractNo=@contractNo and attachmentno=@attachmentno");
                }

               
                SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
                    new SqlParameter("@attachmentno",attachmentno),
                };

                StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            #endregion

            #region 合同附件列表
            //else if (module == "htfjpagelist")   //合同附件列表
            //{
            //获取分页变量
            //int row = int.Parse(context.Request["rows"].ToString());
            //int page = int.Parse(context.Request["page"].ToString());
            //string order = context.Request["order"].ToString();
            //string sort = context.Request["sort"].ToString();

                ////where 参数
            //string contarctno = context.Request.Params["contractNo"];

                //RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            //StringBuilder sqldata = new StringBuilder();
            //StringBuilder sqlcount = new StringBuilder();
            //sqldata.Append(" select *,status as status1 from Icontract_a where contractNo=@contractNo");
            //sqlcount.Append("select count(1) from Icontract_a where contractNo=@contractNo");
            //SqlParameter[] sqlpps = new SqlParameter[] 
            //{
            //    new SqlParameter("@contractNo",contarctno),
            //};

                //StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            //context.Response.Clear();
            //context.Response.Write(sb.ToString());
            //context.Response.End();
            //}
            #endregion

            #region 合同附件列表单独加载
            //            else if (module == "htfjpagelistLonely")   //合同附件列表
            //            {
            //                //获取分页变量
            //                int row = int.Parse(context.Request["rows"].ToString());
            //                int page = int.Parse(context.Request["page"].ToString());
            //                string order = context.Request["order"].ToString();
            //                string sort = context.Request["sort"].ToString();

//                string contractNo = context.Request.Params["contractNo"];
            //                string attachmentno = context.Request.Params["attachmentno"];
            //                string signedtime_begin = context.Request.Params["signedtime_begin"];
            //                string signedtime_end = context.Request.Params["signedtime_end"];

//                if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            //                {
            //                    signedtime_end = Convert.ToDateTime(Convert.ToDateTime(signedtime_end).ToString("yyyy-MM-dd")).AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //                }
            //                RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            //                StringBuilder sqldata = new StringBuilder();
            //                StringBuilder sqlcount = new StringBuilder();
            //                StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            //                if (contractNo != null && contractNo.Trim().Length > 0)
            //                {
            //                    sqlshere.Append(" and t2.contractNo like '%'+@contractNo+'%' ");
            //                }
            //                if (attachmentno != null && attachmentno.Trim().Length > 0)
            //                {
            //                    sqlshere.Append(" and t2.attachmentno like '%'+@attachmentno+'%' ");
            //                }
            //                if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            //                {
            //                    sqlshere.Append(" and t2.signedtime>=@signedtime_begin");
            //                }
            //                if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            //                {
            //                    sqlshere.Append(" and t2.signedtime<=@signedtime_end ");
            //                }
            //                sqldata.Append(@"select  contractNo, attachmentno, seller, signedtime, signedplace, buyer, buyeraddress, currency, pricement1, pricement1per, pricement2, pricement2per, pvalidity, shipment, transport, tradement, harborout, harborarrive, harborclear, placement, validity, remark, templateno, status, createman, createdate, lastmod, lastmoddate
            //from  Icontract_a t2 
            //
            //where " + sqlshere.ToString());
            //                sqlcount.Append(@"select count(1)from  Icontract_a t2  where " + sqlshere.ToString());
            //                SqlParameter[] sqlpps = new SqlParameter[] 
            //                {
            //                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
            //                    new SqlParameter{ParameterName="@attachmentno",Value=attachmentno,DbType=DbType.String},
            //                    new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
            //                    new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String}
            //                };

//                StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, sqlpps, sort, order, page, row);
            //                context.Response.Clear();
            //                context.Response.Write(sb.ToString());
            //                context.Response.End();
            //            }
            #endregion

            else
            {
                StringBuilder sb = new StringBuilder("");
                sb.Append("{\"total\":0,\"rows\":[]}");
                context.Response.Write(sb.ToString());
            }
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