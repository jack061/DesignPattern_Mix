using RM.Busines;
using RM.Busines.Util;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// contractAddOrEdit 的摘要说明
    /// </summary>
    public class contractAddOrEdit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string module = context.Request.QueryString["module"];
            string err = "";
            bool suc = false;
            string template = "";
            string lanugage = "";
            switch (module)
            {
                #region 添加合同
                //添加编辑合同
                //case "addContractTest":
                //    suc = addContractTest(ref err, context);
                //    context.Response.Write(returnData1(suc, err, template, lanugage));
                //    break;
                #endregion
                default:
                    break;
            }
        }
     

        #region 返回json格式

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

        private string returnData1(bool isok, string err, string template, string lanugage)
        {
            string r = "";
            if (isok)
            {
                r = "{\"sucess\": 1,\"contract\": \"" + err + "\",\"template\": \"" + template + "\",\"language\": \"" + lanugage + "\",\"warnmsg\": \"\",\"errormsg\": \"\"}";
            }
            else
            {
                r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            }
            return r;
        }

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

