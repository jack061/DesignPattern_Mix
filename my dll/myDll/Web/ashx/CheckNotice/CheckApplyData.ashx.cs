using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WZX.Busines.Util;

namespace RM.Web.ashx.CheckNotice
{
    /// <summary>
    /// CheckNoticeData 的摘要说明
    /// </summary>
    public class CheckApplyData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request["module"] == null ? "" : context.Request["module"].ToString();
            switch (module)
            {
                case "bookingRequestList"://加载海运订舱申请列表
                    context.Response.Write(bookingRequestList(context));
                    break;
                case "cnpagelist"://加载出库通知数据
                    context.Response.Write(cnpagelist(context));
                    break;
                case"shipproduct":
                    context.Response.Write(shipproduct(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
         
        }
        //加载海运订舱申请列表
        private string bookingRequestList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string productname = context.Request.Params["productname"];
            string buyer = context.Request.Params["buyer"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder();
            if (!string.IsNullOrEmpty(contractNo))
            {
                sqlwhere.Append("and contractNo=@contractNo ");
            }
            if (!string.IsNullOrEmpty(productname))
            {
                sqlwhere.Append("and productName=@productname ");
            }
            if (!string.IsNullOrEmpty(buyer))
            {
                sqlwhere.Append("and buyer like '%'+@buyer+'%' ");
            }

            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@productName",Value=productname,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String}
                };
            //加载发货申请已完成，并且运输方式为海运的合同数据
            sqldata.AppendFormat(@"select * from {0}  where sendOutStatus=2 and transport=海运" + sqlwhere.ToString(), ConstantUtil.TABLE_ECONTRACT);
            sqlcount.AppendFormat("select count(1) from {0} where sendOutStatus=2 and transport=海运" + sqlwhere.ToString(), ConstantUtil.TABLE_ECONTRACT);
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);
            return sb.ToString();
        }
        private string shipproduct(HttpContext context)
        {
         
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string checkId = context.Request["checkId"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@checkId",Value=checkId,DbType=DbType.String},
                 
                };
            sqldata.Append(@"select * from checkProduct  where checkId=@checkId");
            sqlcount.Append("select count(1) from checkProduct  where checkId=@checkId ");
            StringBuilder sb = ui.GetDatatableJsonString(sqldata, pms);
          
            return sb.ToString();

        }
        //获取出库通知数据
        private string cnpagelist(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string order = context.Request["order"].ToString();
            string sort = context.Request["sort"].ToString();
            string contractNo = context.Request.Params["contractNo"];
            string productname = context.Request.Params["productname"];
            string buyer = context.Request.Params["buyer"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(contractNo))
            {
                sqlwhere.Append("and contractNo=@contractNo ");
            }
            if (!string.IsNullOrEmpty(productname))
            {
                sqlwhere.Append("and productName=@productname ");
            }
            if (!string.IsNullOrEmpty(buyer))
            {
                sqlwhere.Append("and buyer like '%'+@buyer+'%' ");
            }

            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@productName",Value=productname,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String}
                };

            sqldata.Append(@"select * from checkoutNotice  where" + sqlwhere.ToString());
            sqlcount.Append("select count(1) from checkoutNotice t2 where " + sqlwhere.ToString());
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);
            return sb.ToString();
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