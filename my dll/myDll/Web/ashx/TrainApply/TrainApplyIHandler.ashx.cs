using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.SessionState;
using System.Data;
using System.Collections;
using RM.Busines.DAL;
using RM.Busines;
using RM.Common.DotNetEncrypt;
using RM.Common.DotNetJson;
using WZX.Busines.DAL;
using WZX.Busines.Util;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;

namespace RM.Web.ashx.TrainApply
{
    /// <summary>
    /// UsersHandler 的摘要说明
    /// </summary>
    public class TrainApplyIHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "saveList"://保存申请列表
                    context.Response.Write(saveList(context));
                    break;
                case "loadList"://获取列表
                    context.Response.Write(loadList(context));
                    break;
                case "saveSum"://添加申请汇总
                    context.Response.Write(saveSum(context));
                    break;
                case "delSum"://删除申请汇总
                    context.Response.Write(delSum(context));
                    break;
                case "getListSum"://获取申请汇总的申请表项
                    context.Response.Write(getListSum(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }
        
        /// <summary>
        /// 保存申请单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string saveList(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();

            ht["applyNo"] = string.IsNullOrEmpty(context.Request["APPLYNO"]) ? "" : context.Request["APPLYNO"].ToString();
            ht["customer"] = string.IsNullOrEmpty(context.Request["CUSTOMER"]) ? "" : context.Request["CUSTOMER"].ToString();
            ht["country"] = string.IsNullOrEmpty(context.Request["COUNTRY"]) ? "" : context.Request["COUNTRY"].ToString();
            ht["pcode"] = string.IsNullOrEmpty(context.Request["PCODE"]) ? "" : context.Request["PCODE"].ToString();
            ht["pname"] = string.IsNullOrEmpty(context.Request["PNAME"]) ? "" : context.Request["PNAME"].ToString();
            ht["harbor"] = string.IsNullOrEmpty(context.Request["HARBOR"]) ? "" : context.Request["HARBOR"].ToString();
            ht["quantity"] = string.IsNullOrEmpty(context.Request["QUANTITY"]) ? "" : context.Request["QUANTITY"].ToString();
            ht["remark"] = string.IsNullOrEmpty(context.Request["REMARK"]) ? "" : context.Request["REMARK"].ToString();
            ht["status"] = string.IsNullOrEmpty(context.Request["STATUS"]) ? "" : context.Request["STATUS"].ToString();
            ht["createman"] = string.IsNullOrEmpty(context.Request["CREATEMAN"]) ? "" : context.Request["CREATEMAN"].ToString();
            ht["createdate"] = string.IsNullOrEmpty(context.Request["CREATEDATE"]) ? "" : context.Request["CREATEDATE"].ToString();
            ht["lastmod"] = string.IsNullOrEmpty(context.Request["LASTMOD"]) ? "" : context.Request["LASTMOD"].ToString();
            ht["lastmoddate"] = string.IsNullOrEmpty(context.Request["LASTMODDATE"]) ? "" : context.Request["LASTMODDATE"].ToString();


            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_TRAINAPPLYINEW, "applyNo", ht["applyNo"].ToString(), ht);
            
            if (IsOk)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "保存成功！");            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "保存失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);

        }

        /// <summary>
        /// 加载申请单属性
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string loadList(HttpContext context) {
            DataTable dt = new DataTable();
            string result = "";
            string applyNo = context.Request["applyNo"] == null ? "" : context.Request["applyNo"].ToString();
            SqlParam[] sqls = new SqlParam[]
            {
                new SqlParam("@applyNo", applyNo),
            };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from " + ConstantUtil.TABLE_TRAINAPPLYINEW + " where applyNo=@applyNo");
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, sqls);
            if (DataTableHelper.IsExistRows(dt))
            {
                DataRow dr = dt.Rows[0];
                result = JsonHelper.DataRowToJson_(dr);
            }
            return result;
        }

        /// <summary>
        /// 保存汇总表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string saveSum(HttpContext context) {
            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();

            ht["applyNo"] = string.IsNullOrEmpty(context.Request["APPLYNO"]) ? "" : context.Request["APPLYNO"].ToString();
            ht["createman"] = string.IsNullOrEmpty(context.Request["CREATEMAN"]) ? "" : context.Request["CREATEMAN"].ToString();
            //处理子表信息
            string subDataJson = context.Request["datagrid"] == null ? "" : context.Request["datagrid"].ToString();
            List<Hashtable> list = new List<Hashtable>();
            if (!(string.IsNullOrEmpty(subDataJson)))
            {
                list = JsonHelper.DeserializeJsonToList<Hashtable>(subDataJson);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Contains("ROWNUM"))
                    {
                        list[i].Remove("ROWNUM");
                    }
                    if ((list[i].Contains("APPLYNO")))
                    {
                        string applyNo2 = list[i]["APPLYNO"].ToString();
                        list[i]["APPLYNO"] = ht["applyNo"];
                        list[i].Add("APPLYNO2",applyNo2);
                        list[i]["CREATEMAN"] = ht["createman"];
                    }
                }
            }

            List<StringBuilder> sqls = new List<StringBuilder>();
            sqls.Add(new StringBuilder(" update "+ConstantUtil.TABLE_TRAINAPPLYINEW+" set status='" + ConstantUtil.STATUS_NEW + "' where applyNo in(select applyNo2 from "+ConstantUtil.TABLE_TRAINAPPLYISUM+" where applyNo = '" + ht["applyNo"].ToString() + "')"));
            sqls.Add(new StringBuilder("delete from " + ConstantUtil.TABLE_TRAINAPPLYISUM + " where applyNo='" + ht["applyNo"] + "'"));
            for (int i = 0; i < list.Count; i++) {
                sqls.Add(new StringBuilder("update " + ConstantUtil.TABLE_TRAINAPPLYINEW + " set status = '"+ConstantUtil.STATUS_CHECKED+"' where applyNo='" + list[i]["APPLYNO2"] + "'"));
                
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into "+ConstantUtil.TABLE_TRAINAPPLYISUM+ "(applyNo,applyNo2,customer,country,pcode,pname,harbor,quantity,remark,createman) ");
                sql.Append(("values('" + list[i]["APPLYNO"].ToString() + "','" + list[i]["APPLYNO2"].ToString() + "','" +
                    list[i]["CUSTOMER"].ToString() + "','" + list[i]["COUNTRY"].ToString() + "','" + list[i]["PCODE"].ToString() +
                    "','" + list[i]["PNAME"].ToString() + "','" + list[i]["HARBOR"].ToString() + "'," +
                    list[i]["QUANTITY"].ToString() + ",'" + list[i]["REMARK"].ToString() + "','" + list[i]["CREATEMAN"].ToString() + "')").Replace("&nbsp;", ""));
                sqls.Add(sql);
            }


            int n = DataFactory.SqlDataBase().BatchExecuteBySql(sqls);

            if (n>0)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "保存成功！");
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "保存失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);

        }

        /// <summary>
        /// 删除汇总表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string delSum(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            string applyNo = context.Request.QueryString["applyNo"];
            List<string> list = new List<string>();
            list.Add(" update " + ConstantUtil.TABLE_TRAINAPPLYINEW + " set status='" + ConstantUtil.STATUS_NEW + "' where applyNo in(select applyNo2 from " + ConstantUtil.TABLE_TRAINAPPLYISUM + " where applyNo = '" + applyNo + "')");
            list.Add("delete from " + ConstantUtil.TABLE_TRAINAPPLYISUM + " where applyNo='" + applyNo + "'");
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

        private string getListSum(HttpContext context) {
            DataTable dt = new DataTable();
            string applyNo = context.Request["applyNo"] == null ? "" : context.Request["applyNo"].ToString();
            SqlParam[] sqls = new SqlParam[]
            {
                new SqlParam("@applyNo", applyNo),
            };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from TrainapplyINew where applyNo in(select applyNo2 from TrainapplyISum where applyNo=@applyNo)");
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, sqls);
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                sb.Append("{\"total\":" + dt.Rows.Count + ",");
                sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                sb.Append("}");
            }
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