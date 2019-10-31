using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace RM.Web.ashx.CostManager
{
    /// <summary>
    /// costList 的摘要说明
    /// </summary>
    public class costList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;

            }
        }

        private string getList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string sort = context.Request["sort"] ?? "contractNo";
            string order = context.Request["order"] ?? "asc";
            //获取查询条件

            string department = context.Request.Params["department"];
            string contractNo = context.Request.Params["contractNo"];
            string applicant = context.Request.Params["applicant"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(department))
            {
                sqlwhere.Append("and department=@department ");
            }
            if (!string.IsNullOrEmpty(contractNo))
            {
                sqlwhere.Append("and contractNo=@contractNo ");
            }
            if (!string.IsNullOrEmpty(applicant))
            {
                sqlwhere.Append("and applicant like '%'+@applicant+'%' ");
            }
            sqlwhere.Append(" and status=1");
            SqlParameter[] pms = new SqlParameter[] 
                {

                    new SqlParameter{ParameterName="@department",Value=department,DbType=DbType.String},
                    new SqlParameter{ParameterName="@contractNo",Value=contractNo,DbType=DbType.String},
                    new SqlParameter{ParameterName="@applicant",Value=applicant,DbType=DbType.String}
                };

            sqldata.Append(@"select * from costManagerment  where" + sqlwhere.ToString());
            sqlcount.Append("select count(1) from costManagerment t2 where " + sqlwhere.ToString());
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