using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using RM.Busines;
using WZX.Busines.Util;
using System.Text;
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using System.Data;
using System.Data.SqlClient;
using RM.Common.DotNetJson;

namespace RM.Web.ashx.ContractPayment
{
    /// <summary>
    /// ZXBApply 的摘要说明
    /// </summary>
    public class ZXBApply : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"].ToString();
            switch (action)
            {
                #region list
                case "GetList"://获取列表
                    context.Response.Write(GetList(context));
                    break;
                case "delApply"://删除
                    context.Response.Write(delApply(context));
                    break;
                case "StopZXB"://停用中信保
                    context.Response.Write(StopZXB(context));
                    break;
                #endregion
                #region Form
                case "add"://新增&修改时保存
                    context.Response.Write(add(context));
                    break;
                case "GetTable":
                    context.Response.Write(GetTable(context));
                    break;
                case "GetData":
                    context.Response.Write(GetData(context));
                    break;
                #endregion
                
                default: context.Response.Write(""); break;
            }
        }
        #region list
        //列表
        public string GetList(HttpContext context)
        {
            //更新过期内容
            StringBuilder upsql = new StringBuilder();
            upsql.Append("update PayZXBApply set status=3  where GETDATE()>Deadline");
            DataFactory.SqlDataBase().ExecuteBySql(upsql);

            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string sort = context.Request["sort"]??"";
            string order = context.Request["order"]??"";

            string client = (context.Request["client"] ?? "").ToString().Trim();
            string unit = (context.Request["unit"] ?? "").ToString().Trim();
            string beginTime = (context.Request["beginTime"] ?? "").ToString().Trim();
            string endTime = (context.Request["endTime"] ?? "").ToString().Trim();

            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(client))
            {
                SqlWhere.Append(" and client like '%"+client+"%'");
            }
            if (!string.IsNullOrEmpty(unit))
            {
                SqlWhere.Append(" and unit like '%" + unit + "%'");
            }
            if (!string.IsNullOrEmpty(beginTime))
            {
                SqlWhere.Append(" and createdate > '" + beginTime + "'");
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                SqlWhere.Append(" and createdate < '" + endTime + "'");
            }

            sqldata.Append(string.Format(@"	select case
	                            when status=2 then 2
	                            when status=3 then 3 
	                            when ISNULL(pd.usedAmountpd,0)=0 then 0
	                            when ISNULL(pd.usedAmountpd,0)>0 and ISNULL(pd.usedAmountpd,0)<pa.applyAmount then 4
	                            when ISNULL(pd.usedAmountpd,0)>=pa.applyAmount then 1
	                            end as status1
	                            ,ISNULL(pd.usedAmountpd,0) as usedAmount1,pa.applyAmount-ISNULL(pd.usedAmountpd,0) as unusedAmount1,pa.*
                                from PayZXBApply pa
                                left join (select pz.payAccount as payAccount,ISNULL(SUM(payingAmount),0) as usedAmountpd 
									from PayZXBDetails pd
									left join PayZXB pz on pd.payNo=pz.payNo                                
									group by pz.payAccount
                                ) pd 
                                on  pd.payAccount=pa.applyNo
                                where 1=1 {0}
                                ", SqlWhere));//order by pa.status asc, pa.createdate desc

            sqlcount.Append("select COUNT(1) from PayZXBApply where 1=1 "+SqlWhere.ToString());
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, null, sort, order, page, row);

            return sb.ToString();            
        }
        //删除
        private string delApply(HttpContext context)
        {
            var applyNo = context.Request.Params["applyNo"];
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                string sql = " delete  from PayZXBApply where applyNo=@applyNo";
                SqlParameter[] pms = new SqlParameter[]{
                    new SqlParameter("@applyNo",applyNo)
                };

                int r = bll.ExecuteNonQuery(sql, pms);

                if (r > 0)
                {
                   return "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    return "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + "删除失败" + "\"}";
                }
            }
        }
        /// <summary>
        /// 停用中信保
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string StopZXB(HttpContext context) {
            string applyNo = context.Request.QueryString["applyNo"];
            StringBuilder sql=new StringBuilder();
            sql.Append("update PayZXBApply set status=3 where applyNo='"+applyNo+"' and status=0");
            int cnt = DataFactory.SqlDataBase().ExecuteBySql(sql);

            string result="";
            result = cnt > 0 ? "{\"res\":\"ok\",\"msg\":\"\"}" : "{\"res\":\"fail\",\"msg\":\"\"}";
            return result;
        }
        #endregion
        #region FORM
        //保存
        private string add(HttpContext context) {
            string result = string.Empty;

            Hashtable ht = new Hashtable();

            ht["applyNo"] = context.Request["APPLYNO"] == null ? "" : context.Request["APPLYNO"].ToString();
            ht["applyMan"] = context.Request["APPLYMAN"] == null ? "" : context.Request["APPLYMAN"].ToString();
            ht["applyDate"] = context.Request["APPLYDATE"] == null ? "" : context.Request["APPLYDATE"].ToString();
            ht["client"] = context.Request["CLIENT"] == null ? "" : context.Request["CLIENT"].ToString();
            ht["currency"] = context.Request["CURRENCY"] == null ? "" : context.Request["CURRENCY"].ToString();
            ht["applyAmount"] = context.Request["APPLYAMOUNT"] == null ? "" : context.Request["APPLYAMOUNT"].ToString();
            ht["XZDays"] = context.Request["XZDAYS"] == null ? "" : context.Request["XZDAYS"].ToString();
            ht["usedAmount"] = '0';
            ht["unusedAmount"] = ht["applyAmount"];
            ht["unit"] = context.Request["UNIT"] == null ? "" : context.Request["UNIT"].ToString();
            ht["lastUseDate"] = ht["applyDate"];
            ht["deadline"] = Convert.ToDateTime(ht["applyDate"]).AddDays(Convert.ToInt32(ht["XZDays"])).ToString("yyyy-MM-dd hh:mm:ss");
            ht["simpleClient"] = context.Request["SIMPLECLIENT"] == null ? "" : context.Request["SIMPLECLIENT"].ToString();

            string type = context.Request.QueryString["type"];
            if (type == "add")
            {               
                ht["createman"] = context.Request["APPLYMAN"] == null ? "" : context.Request["APPLYMAN"].ToString();
                ht["createdate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            }
            else
            {
                if(context.Request["STATUS"] != null)
                    ht["status"] = 2;

                ht["lastmod"] = context.Request["APPLYMAN"] == null ? "" : context.Request["APPLYMAN"].ToString();
                ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            }

            bool suc = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_PAYZXBAPPLY, "applyNo", ht["applyNo"].ToString(), ht);

            //返回json
            result = suc == true ? "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}" : "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"操作失败\"}";
            return result;
        }
        //获取一个申请信息
        public string GetData(HttpContext context) {
            string applyNo = context.Request.QueryString["applyNo"];

            StringBuilder sql = new StringBuilder();

            sql.Append(@"select * ,ISNULL(pd.usedAmountpd,0) as usedAmount1,(pa.applyAmount-ISNULL(pd.usedAmountpd,0)) as unusedAmount1
                                from PayZXBApply pa
                                left join (select payNo,ISNULL(SUM(payingAmount),0) as usedAmountpd from PayZXBDetails group by payNo) pd 
                                on  pd.payNo=pa.applyNo where pa.applyNo='"+applyNo+"'");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            if(dt == null || dt.Rows.Count==0){
                return "{}";
            }else{
                return JsonHelper.DataRowToJson_(dt.Rows[0]);
            }
        }
        //获取申请相关的合同信息
        public string GetTable(HttpContext context)
        {
            string applyNo = context.Request.QueryString["applyNo"];
            string sql = string.Format(@"select PayZXBDetails.payingAmount, Econtract.*
                                        from {0}
                                        join {1} on {1}.applyNo={0}.payAccount
                                        join {2} on {2}.payNo={0}.payNo
                                        join Econtract on {2}.contractNo=Econtract.contractNo
                                        where {1}.applyNo='{3}'",
                ConstantUtil.TABLE_PAYZXB,ConstantUtil.TABLE_PAYZXBAPPLY,ConstantUtil.TABLE_PAYZXBDETAILS,applyNo);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            if (dt == null || dt.Rows.Count == 0)
            {
                return "{\"total\":{0},\"rows\":[]";
            }
            else
            {
               return "{\"total\":"+dt.Rows.Count +","+JsonHelper.DataTableToJson_(dt, "rows")+"}";
            }
        }
        #endregion
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}