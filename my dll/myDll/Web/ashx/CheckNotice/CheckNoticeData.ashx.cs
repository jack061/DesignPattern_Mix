using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using RM.Common.DotNetBean;
using RM.Busines;

namespace RM.Web.ashx.CheckNotice
{
    /// <summary>
    /// CheckNoticeData 的摘要说明
    /// </summary>
    public class CheckNoticeData : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request["module"] == null ? "" : context.Request["module"].ToString();
            switch (module)
            {
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
            string noticeStatus = context.Request.Params["noticeStatus"]?? "";
            string distributeStatus = context.Request.Params["distributeStatus"] ?? "";
            string confirmStatus = context.Request.Params["confirmStatus"] ?? "";
            string sendOutNoticeStatus = context.Request.Params["sendOutNoticeStatus"] ?? "";
            string submodule = context.Request.Params["submodule"] ?? "";
            string checkNoticeNumber = context.Request.Params["checkNoticeNumber"] ?? "";
            
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(checkNoticeNumber))
            {
                sqlwhere.Append("and checkNoticeNumber like '%'+@checkNoticeNumber+'%' ");
            }
            if (!string.IsNullOrEmpty(contractNo))
            {
                sqlwhere.Append("and saleContract  like '%'+@contractNo+'%' ");
            }
            if (!string.IsNullOrEmpty(productname))
            {
                sqlwhere.Append("and productName=@productname ");
            }
            if (!string.IsNullOrEmpty(buyer))
            {
                sqlwhere.Append("and buyer like '%'+@buyer+'%' ");
            }
            //状态
            if (!string.IsNullOrEmpty(noticeStatus))
            {
                sqlwhere.Append("and noticeStatus=@noticeStatus  ");
            }
            if (!string.IsNullOrEmpty(distributeStatus))
            {
                sqlwhere.Append("and distributeStatus=@distributeStatus  ");
            }
            if (!string.IsNullOrEmpty(confirmStatus))
            {
                sqlwhere.Append("and confirmStatus=@confirmStatus  ");
            }
            if (!string.IsNullOrEmpty(sendOutNoticeStatus))
            {
                sqlwhere.Append("and sendOutNoticeStatus=@sendOutNoticeStatus  ");
            }

            string agency = "";
            if (submodule == "订舱通知")
            {
                StringBuilder sql_agency = new StringBuilder();
                sql_agency.Append(" select * from View_ORG_USER where LoginName='" + RequestSession.GetSessionUser().UserAccount.ToString() + "'");
                agency = DataFactory.SqlDataBase().getString(sql_agency, "PERSON");
                sqlwhere.Append(" and distributeAgency=@distributeAgency");
            }

            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@checkNoticeNumber",Value=checkNoticeNumber,DbType=DbType.String},
                    new SqlParameter{ParameterName="@productName",Value=productname,DbType=DbType.String},
                    new SqlParameter{ParameterName="@noticeStatus",Value=noticeStatus,DbType=DbType.String},
                    new SqlParameter{ParameterName="@distributeStatus",Value=distributeStatus,DbType=DbType.String},
                    new SqlParameter{ParameterName="@confirmStatus",Value=confirmStatus,DbType=DbType.String},
                    new SqlParameter{ParameterName="@sendOutNoticeStatus",Value=sendOutNoticeStatus,DbType=DbType.String},
                    new SqlParameter{ParameterName="@buyer",Value=buyer,DbType=DbType.String},
                    new SqlParameter{ParameterName="@distributeAgency",Value=agency,DbType=DbType.String}
                };

            //sqldata.Append(@"select * from checkoutNotice  where" + sqlwhere.ToString());
            sqldata.Append(@"select * from (select a.*,b.paystatus from checkoutNotice a, Econtract b  where a.saleContract = b.contractNo) t where " + sqlwhere.ToString());
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