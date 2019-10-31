using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// loadOther 的摘要说明
    /// </summary>
    public class loadOther : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "application/json";
            string module = context.Request["module"];
            string suc = string.Empty;
            switch (module)
            {
                case "loadClassific"://获取买卖双方为境内境外
                    suc = loadClassific(context);
                    context.Response.Write(suc);
                    break;
                case "changeStatusNew"://更改合同状态为新建
                    suc = changeStatusNew(context);
                    context.Response.Write(suc);
                    break;
                case "changeConAbandon"://废弃合同
                    suc = changeConAbandon(context);
                    context.Response.Write(suc);
                    break;
                case "changeConDisContinue"://中止合同
                    suc = changeConDisContinue(context);
                    context.Response.Write(suc);
                    break;
                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;
            }
        }
        //中止合同
        private string changeConDisContinue(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string abandonReson = context.Request.Params["disContinueArea"];
            Hashtable ht = new Hashtable();
            ht["abandonStatus"] = ConstantUtil.STATUS_CHECK7;//中止_待直线经理审核
            ht["typeStatus"] = ConstantUtil.STATUS_DISCONTINUE;//中止
            ht["abandonReson"] = abandonReson;//废弃中止原因
            //向合同审核表中插入审批数据
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT, "contractNo", contractNo, ref sqls, ref objs);
            ht.Clear();
            List<Hashtable> list_review = new List<Hashtable>();
            ht["contractNo"] = contractNo;
            ht["reviewstatus"] = ConstantUtil.STATUS_CHECK7;//废弃_待直线经理审核
            ht["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;//提交
            ht["reviewman"] = RequestSession.GetSessionUser().UserAccount;
            ht["reviewdata"] = DateTime.Now;
            list_review.Add(ht);
            SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_ECONTRACT, ref sqls, ref objs);
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? "ok" : "false";
        }
        //废弃合同
        private string changeConAbandon(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string abandonReson = context.Request.Params["abandonArea"];
            Hashtable ht = new Hashtable();
            ht["abandonStatus"] = ConstantUtil.STATUS_CHECK1;//废弃_待直线经理审核
            ht["typeStatus"] = ConstantUtil.STATUS_ABANDON;//废弃
            ht["abandonReson"] = abandonReson;//废弃中止原因
            //向合同审核表中插入审批数据
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_ECONTRACT, "contractNo", contractNo, ref sqls, ref objs);
            ht.Clear();
            List<Hashtable> list_review = new List<Hashtable>();
            ht["contractNo"] = contractNo;
            ht["reviewstatus"] = ConstantUtil.STATUS_CHECK1;//废弃_待直线经理审核
            ht["status"] = ConstantUtil.STATUS_STOCKIN_SUBMIT;//提交
            ht["reviewman"] = RequestSession.GetSessionUser().UserAccount;
            ht["reviewdata"] = DateTime.Now;
            list_review.Add(ht);
            SqlUtil.getBatchSqls(list_review, ConstantUtil.TABLE_ECONTRACT, ref sqls, ref objs);
            int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);
            return r > 0 ? "ok" : "false";
        }

        //更改合同状态为新建
        private string changeStatusNew(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            Hashtable ht = new Hashtable();
            ht["status"] = ConstantUtil.STATUS_STOCKIN_NEW;
            int r= DataFactory.SqlDataBase().UpdateByHashtable(ConstantUtil.TABLE_ECONTRACT, "contractNo", contractNo, ht);
            return r > 0 ? "ok" : "false";

        }
        //获取买卖双方为境内境外
        private string loadClassific(HttpContext context)
        {
            string buyercode = context.Request.Params["buyercode"];
            string sellercode = context.Request.Params["sellercode"];
            StringBuilder sb = new StringBuilder(string.Format(@"select t1.classific as customFic,t2.classific as supplierFic
                                                    from {0} t1,{1} t2 where t1.code=@buyercode and t2.code=@sellercode"
                                                    , ConstantUtil.TABLE_CUSTOMER, ConstantUtil.TABLE_SUPPLIER));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[2]{new SqlParam("@buyercode",buyercode),new 
            SqlParam("sellercode",sellercode)}, 0);
            string jsonRow = JsonHelper.DataRowToJson_(dt.Rows[0]);
            return jsonRow;

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