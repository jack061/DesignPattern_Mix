using RM.Common.DotNetBean;
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

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// chosseContractData 的摘要说明
    /// </summary>
    public class chosseContractData : IHttpHandler, IRequiresSessionState
    {
        RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
        public void ProcessRequest(HttpContext context)
        {
            //从后台获取数据
            context.Response.ContentType = "application/json";
            string module = context.Request["module"];
            string suc = string.Empty;
            switch (module)
            {
                case "getImConCopyList"://独立创建下关联创建复制关联合同号
                    suc = getImConCopyList(context);
                    context.Response.Write(suc);
                    break;
                case "serviceContactContract"://服务合同主合同创建关联复制筛选合同
                    suc = serviceContactContract(context);
                    context.Response.Write(suc);
                    break;
                case "acceptUploadFile"://承兑excel文件上传
                    suc = acceptUploadFile(context);
                    context.Response.Write(suc);
                    break;
                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;
            }
        }

        #region 承兑excel文件上传
        private string acceptUploadFile(HttpContext context)
        {
            string virPath = ConstantUtil.FILE_ACCEPT_URL + DateTime.Now.Day;
            RM.Common.DotNetFile.ExcelHelper excel = new Common.DotNetFile.ExcelHelper();
            Hashtable ht_result = new Hashtable();
            DataTable dt = excel.upExcelToDatatable(context, virPath, ref ht_result);
            string json = ui.ToEasyUIDataGridJson(dt).ToString(); 
            return json;
        } 
        #endregion

        #region 服务合同主合同创建关联复制筛选合同，不限制创建人
        private string serviceContactContract(HttpContext context)
        {
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder(" 1=1 ");
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
          
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlwhere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlwhere.Append(" and t1.createdate<=@signedtime_end ");
            }
            //sqlwhere.Append(" and datediff(day,createdate,getdate())<365");
            sqlwhere.AppendFormat(" and t1.serviceTag='{0}'", ConstantUtil.CONTRACT_SERVICE);
            sqldata.AppendFormat(@"select t1.* from {0} t1 where " + sqlwhere.ToString(),ConstantUtil.TABLE_ECONTRACT_SERVICE);
            SqlParameter[] sqlpps = new SqlParameter[]{
                new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String},
            };
              StringBuilder sb=ui.GetDatatableJsonString(sqldata,sqlpps);
              return sb.ToString();

        } 
        #endregion

        #region 独立创建下关联创建复制关联合同号,只筛选主合同
        private string getImConCopyList(HttpContext context)
        {
            string flowdirection = context.Request.Params["flowdirection"];
            string createman = RequestSession.GetSessionUser().UserAccount.ToString();
       
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlshere = new StringBuilder(" 1=1 ");
            string signedtime_begin = context.Request.Params["signedtime_begin"];
            string signedtime_end = context.Request.Params["signedtime_end"];
            if (flowdirection != null && flowdirection.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.flowdirection=@flowdirection ");
            }
            if (signedtime_begin != null && signedtime_begin.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate>=@signedtime_begin");
            }
            if (signedtime_end != null && signedtime_end.Trim().Length > 0)
            {
                sqlshere.Append(" and t1.createdate<=@signedtime_end ");
            }
            sqlshere.AppendFormat(" and t1.contractTag={0} and t1.frameContract='否'", ConstantUtil.CONTRACTTAG_MAINCON);
            //sqlshere.Append(" and datediff(day, createdate, GETDATE()) <365 ");
            sqlshere.Append(" and t1.createman=@createman ");
            sqldata.Append(@" select t1.*
                              from Econtract t1              
                              where " + sqlshere.ToString());
        
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                   new SqlParameter{ParameterName="@flowdirection",Value=flowdirection,DbType=DbType.String},
                     new SqlParameter{ParameterName="@createman",Value=createman,DbType=DbType.String},
                        new SqlParameter{ParameterName="@signedtime_begin",Value=signedtime_begin,DbType=DbType.String},
                         new SqlParameter{ParameterName="@signedtime_end",Value=signedtime_end,DbType=DbType.String},
                     
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        
        #endregion

        #region 实现接口
        public bool IsReusable
        {
            get
            {
                return false;
            }
        } 
        #endregion
    }
}