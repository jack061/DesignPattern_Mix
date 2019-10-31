using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.SessionState;
using System.Data;
using RM.Busines.DAL;
using RM.Busines;
using RM.Common.DotNetEncrypt;
using RM.Common.DotNetJson;
using WZX.Busines.DAL;
using System.Collections;
using WZX.Busines.Util;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Busines.DAL.Inspect;
using RM.Busines.DAL.TrainApply;
using RM.Common.DotNetData;

namespace RM.Web.ashx.TrainApply
{
    /// <summary>
    /// UsersHandler 的摘要说明
    /// </summary>
    public class TrainApplyEHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            View_Users bll = new View_Users();

            string action = context.Request.QueryString["action"];
            switch (action)
            {
                case "edit":
                    context.Response.Write(edit(context));
                    break;
                case "getSubList":
                    context.Response.Write(getSubList(context));
                    break;
                case "del":
                     context.Response.Write(del(context));
                    break;
                case "add":
                    context.Response.Write(add(context));
                    break;
                default:
                    context.Response.Write("");
                    break;
            }
        }

        private static string add(HttpContext context)
        {
            Hashtable test = RequestHelper.getDataFromRequestForm(context);
            Hashtable test1 = RequestHelper.getDataFromRequestForm_1(context);

            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["applyNo"] = string.IsNullOrEmpty(context.Request["APPLYNO"]) ? "" : context.Request["APPLYNO"].ToString();
            ht["customer"] = string.IsNullOrEmpty(context.Request["CUSTOMER"]) ? "" : context.Request["CUSTOMER"].ToString();
            ht["sendman"] = string.IsNullOrEmpty(context.Request["SENDMAN"]) ? "" : context.Request["SENDMAN"].ToString();
            ht["departure"] = string.IsNullOrEmpty(context.Request["DEPARTURE"]) ? "" : context.Request["DEPARTURE"].ToString();
            ht["senddeclare"] = string.IsNullOrEmpty(context.Request["SENDDECLARE"]) ? "" : context.Request["SENDDECLARE"].ToString();
            ht["receiveman"] = string.IsNullOrEmpty(context.Request["RECEIVEMAN"]) ? "" : context.Request["RECEIVEMAN"].ToString();
            ht["arrival"] = string.IsNullOrEmpty(context.Request["ARRIVAL"]) ? "" : context.Request["ARRIVAL"].ToString();
            ht["harborstation"] = string.IsNullOrEmpty(context.Request["HARBORSTATION"]) ? "" : context.Request["HARBORSTATION"].ToString();
            ht["whereload"] = string.IsNullOrEmpty(context.Request["WHERELOAD"]) ? "" : context.Request["WHERELOAD"].ToString();
            ht["determwght"] = string.IsNullOrEmpty(context.Request["DETERMWGHT"]) ? "" : context.Request["DETERMWGHT"].ToString();
            ht["costpay"] = string.IsNullOrEmpty(context.Request["COSTPAY"]) ? "" : context.Request["COSTPAY"].ToString();
            ht["senddoc"] = string.IsNullOrEmpty(context.Request["SENDDOC"]) ? "" : context.Request["SENDDOC"].ToString();
            ht["contractno"] = string.IsNullOrEmpty(context.Request["CONTRACTNO"]) ? "" : context.Request["CONTRACTNO"].ToString();
            ht["transcontractdate"] = string.IsNullOrEmpty(context.Request["TRANSCONTRACTDATE"]) ? "" : context.Request["TRANSCONTRACTDATE"].ToString();
            ht["arrivaldate"] = string.IsNullOrEmpty(context.Request["ARRIVALDATE"]) ? "" : context.Request["ARRIVALDATE"].ToString();
            ht["procedures"] = string.IsNullOrEmpty(context.Request["PROCEDURES"]) ? "" : context.Request["PROCEDURES"].ToString();
            ht["batchno"] = string.IsNullOrEmpty(context.Request["BATCHNO"]) ? "" : context.Request["BATCHNO"].ToString();
            ht["remark"] = string.IsNullOrEmpty(context.Request["REMARK"]) ? "" : context.Request["REMARK"].ToString();
            ht["status"] = string.IsNullOrEmpty(context.Request["STATUS"]) ? "" : context.Request["STATUS"].ToString();
            ht["createman"] = string.IsNullOrEmpty(context.Request["CREATEMAN"]) ? "" : context.Request["CREATEMAN"].ToString();
            ht["createdate"] = string.IsNullOrEmpty(context.Request["CREATEDATE"]) ? "" : context.Request["CREATEDATE"].ToString();
            ht["lastmod"] = string.IsNullOrEmpty(context.Request["LASTMOD"]) ? "" : context.Request["LASTMOD"].ToString();
            ht["lastmoddate"] = string.IsNullOrEmpty(context.Request["LASTMODDATE"]) ? "" : context.Request["LASTMODDATE"].ToString();

            StringBuilder[] sqls;
            object[] objs;
            doSubList(context, ht, out sqls, out objs);

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_TRAINAPPLYE, "applyNo", ht["applyNo"].ToString(), ht);
            if (IsOk)
            {
                bool flag = true;
                if (sqls.Length > 0 && objs.Length > 0)
                {
                    flag = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0 ? true : false;
                }
                if (flag)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    //删除主表
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_TRAINAPPLYE, "applyNo", ht["applyNo"].ToString());
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);
        }

        /// <summary>
        /// 处理字表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ht"></param>
        /// <param name="subDataJson1"></param>
        /// <param name="sqls1"></param>
        /// <param name="objs1"></param>
        private static void doSubList(HttpContext context, Hashtable ht, out StringBuilder[] sqls1, out object[] objs1)
        {
            sqls1 = null;
            objs1 = null;

            List<StringBuilder> sqlList = new List<StringBuilder>();
            List<object> objList = new List<object>();

            for (int i = 0; i < 4; i++)
            {
                //处理子表信息
                string subDataJson1 = context.Request["datagrid" + (i + 1)] == null ? "" : context.Request["datagrid" + (i + 1)].ToString();
                List<Hashtable> list = new List<Hashtable>();
                if (!(string.IsNullOrEmpty(subDataJson1)) && subDataJson1.Length > 2)
                {
                    list = JsonHelper.DeserializeJsonToList<Hashtable>(subDataJson1);
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (!list[j].Contains("SORTNO"))
                            list[j].Add("SORTNO", j + "");

                        if (!(list[j].Contains("APPLYNO")) )
                        {
                            list[j].Add("APPLYNO", ht["applyNo"].ToString());
                        }
                    }
                    StringBuilder[] sqlitem = new StringBuilder[list.Count + 1];
                    object[] objitem = new object[list.Count + 1];

                    if (i == 0)
                        SqlUtil.getBatchFromList(list, ConstantUtil.TABLE_TRAINAPPLYE1, "applyNo", ht["applyNo"].ToString(), ref sqlitem, ref objitem);
                    else if (i == 1)
                    {
                        SqlUtil.getBatchFromList(list, ConstantUtil.TABLE_TRAINAPPLYE2, "applyNo", ht["applyNo"].ToString(), ref sqlitem, ref objitem);
                    }
                    else if (2 == i)
                    {
                        SqlUtil.getBatchFromList(list, ConstantUtil.TABLE_TRAINAPPLYE3, "applyNo", ht["applyNo"].ToString(), ref sqlitem, ref objitem);
                    }
                    else
                    {
                        SqlUtil.getBatchFromList(list, ConstantUtil.TABLE_TRAINAPPLYE4, "applyNo", ht["applyNo"].ToString(), ref sqlitem, ref objitem);
                    }
                    sqlList.AddRange(sqlitem);
                    objList.AddRange(objitem);
                }
            }
            sqls1 = sqlList.ToArray();
            objs1 = objList.ToArray();
        }


        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string del(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            string applyNo = context.Request.QueryString["applyNo"];

            List<string> list = new List<string>();
            list.Add("delete from "+ConstantUtil.TABLE_TRAINAPPLYE+" where applyNo='" + applyNo + "'");
            list.Add("delete from " + ConstantUtil.TABLE_TRAINAPPLYE1 + " where applyNo='" + applyNo + "'");
            list.Add("delete from " + ConstantUtil.TABLE_TRAINAPPLYE2 + " where applyNo='" + applyNo + "'");
            list.Add("delete from " + ConstantUtil.TABLE_TRAINAPPLYE3 + " where applyNo='" + applyNo + "'");
            list.Add("delete from " + ConstantUtil.TABLE_TRAINAPPLYE4 + " where applyNo='" + applyNo + "'");
            if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list) > 0)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "操作成功");
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);
        }

        //private static void sel(HttpContext context)
        //{
        //    string applyNo = context.Request.QueryString["applyNo"];
        //    DataSet ds = DataFactory.SqlDataBaseExpand().Query("select * from TrainapplyI where applyNo ='" + applyNo + "'");

        //    string IdList = "";
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        if (IdList != "")
        //            IdList += ",";
        //        IdList += dr["applyNo"].ToString();
        //    }
        //    context.Response.Write(IdList);
        //}

        /// <summary>
        /// 获取子表信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string getSubList(HttpContext context) {

            int table = int.Parse(context.Request["table"].ToString());
            int count = 0;

            //获取查询条件
            string applyNo = (context.Request["applyNo"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and  applyNo = '" + applyNo + "'");
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = TrainApplyE_Dal.GetSubList(ConstantUtil.TABLE_TRAINAPPLYE+table,applyNo);
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                sb.Append("{\"total\":" + count + ",");
                sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                sb.Append("}");
            }
            return sb.ToString();
        }
       
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        private static string edit(HttpContext context)
        {
            DataTable dt = new DataTable();
            string result = "";
            string applyNo = context.Request["applyNo"] == null ? "" : context.Request["applyNo"].ToString();
            SqlParam[] sqls = new SqlParam[]
            {
                new SqlParam("@applyNo", applyNo),
            };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from " + ConstantUtil.TABLE_TRAINAPPLYE + " where applyNo=@applyNo");
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, sqls);
            if (DataTableHelper.IsExistRows(dt))
            {
                DataRow dr = dt.Rows[0];
                result = JsonHelper.DataRowToJson_(dr);
            }
            return result;
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