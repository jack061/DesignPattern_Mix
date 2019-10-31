using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using RM.Busines;
using WZX.Busines.Util;
using RM.Common.DotNetJson;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines.DAL.Inspect;
using RM.Common.DotNetData;
using RM.Common.DotNetBean;

namespace RM.Web.ashx.Inspect
{
    /// <summary>
    /// PreCheckHandler 的摘要说明
    /// </summary>
    public class PreCheckHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "addProduct"://添加
                    context.Response.Write(addProduct(context));
                    break;
                case "addPack":
                    context.Response.Write(addPack(context));
                    break;
                case "editProduct"://添加
                    context.Response.Write(editProduct(context));
                    break;
                case "editPack":
                    context.Response.Write(editPack(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        /// <summary>
        /// 添加产品预验信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string addProduct(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();

            ht["inspectionNo"] = string.IsNullOrEmpty(context.Request["INSPECTIONNO"]) ? "" : context.Request["INSPECTIONNO"].ToString();
            ht["useabledate"] = string.IsNullOrEmpty(context.Request["USEABLEDATE"]) ? "" : context.Request["USEABLEDATE"].ToString();
            ht["inspecman"] = string.IsNullOrEmpty(context.Request["INSPECMAN"]) ? "" : context.Request["INSPECMAN"].ToString();
            //ht["pcode"] = string.IsNullOrEmpty(context.Request["PCODE"]) ? "" : context.Request["PCODE"].ToString();
            ht["pname"] = string.IsNullOrEmpty(context.Request["PNAME"]) ? "" : context.Request["PNAME"].ToString();
            ht["pcodehs"] = string.IsNullOrEmpty(context.Request["PCODEHS"]) ? "" : context.Request["PCODEHS"].ToString();
            ht["quantity"] = string.IsNullOrEmpty(context.Request["QUANTITY"]) ? new Decimal(0.0) : Convert.ToDecimal(context.Request["QUANTITY"].ToString());
            ht["qunit"] = string.IsNullOrEmpty(context.Request["QUNIT"]) ? "" : context.Request["QUNIT"].ToString();
            //ht["quantityA"] = string.IsNullOrEmpty(context.Request["QUANTITYA"]) ? "" : context.Request["QUANTITYA"].ToString();
            //ht["quantityB"] = string.IsNullOrEmpty(context.Request["QUANTITYB"]) ? "" : context.Request["QUANTITYB"].ToString();
            ht["amount"] = string.IsNullOrEmpty(context.Request["AMOUNT"]) ? "" : context.Request["AMOUNT"].ToString();
            ht["currency"] = string.IsNullOrEmpty(context.Request["CURRENCY"]) ? "" : context.Request["CURRENCY"].ToString();
            ht["produce"] = string.IsNullOrEmpty(context.Request["PRODUCE"]) ? "" : context.Request["PRODUCE"].ToString();
            ht["batchno"] = string.IsNullOrEmpty(context.Request["BATCHNO"]) ? "" : context.Request["BATCHNO"].ToString();
            ht["producedate"] = string.IsNullOrEmpty(context.Request["PRODUCEDATE"]) ? "" : context.Request["PRODUCEDATE"].ToString();
            ht["transport"] = string.IsNullOrEmpty(context.Request["TRANSPORT"]) ? "" : context.Request["TRANSPORT"].ToString();
            ht["status"] = string.IsNullOrEmpty(context.Request["statush"]) ? "" : context.Request["statush"].ToString();
            ht["createman"] = string.IsNullOrEmpty(context.Request["CREATEMAN"]) ? "" : context.Request["CREATEMAN"].ToString();
            ht["createdate"] = string.IsNullOrEmpty(context.Request["CREATEDATE"]) ? "" : context.Request["CREATEDATE"].ToString();
            ht["lastmod"] = RequestSession.GetSessionUser().UserName.ToString();
            ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); 

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_INSPECTIONRE, "inspectionNo", ht["inspectionNo"].ToString(), ht);
            if (IsOk)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "操作成功！");
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);
        }

        /// <summary>
        /// 添加包装预验信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string addPack(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();

            List<Hashtable> list = new List<Hashtable>();
            createList(context, list);

            Hashtable ht = new Hashtable();
            ht["pname"] = string.IsNullOrEmpty(context.Request["PNAME"]) ? "" : context.Request["PNAME"].ToString();
            ht["hscode"] = string.IsNullOrEmpty(context.Request["HSCODE"]) ? "" : context.Request["HSCODE"].ToString();
            ht["pname"] = string.IsNullOrEmpty(context.Request["PNAME"]) ? "" : context.Request["PNAME"].ToString();
            ht["inspectionNo"] = string.IsNullOrEmpty(context.Request["INSPECTIONNO"]) ? "" : context.Request["INSPECTIONNO"].ToString();
            ht["status"] = string.IsNullOrEmpty(context.Request["statush"]) ? "" : context.Request["statush"].ToString();
            
            ht["createman"] = string.IsNullOrEmpty(context.Request["CREATEMAN"]) ? "" : context.Request["CREATEMAN"].ToString();
            ht["createdate"] = string.IsNullOrEmpty(context.Request["CREATEDATE"]) ? "" : context.Request["CREATEDATE"].ToString();
            ht["lastmod"] = RequestSession.GetSessionUser().UserName.ToString();
            ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); 


            StringBuilder[] sqls = new StringBuilder[list.Count+1];
            object[] objs = new object[list.Count+1];

            SqlUtil.getBatchFromList(list, ConstantUtil.TABLE_INSPECTIONREPACK2, "inspectionNo", ht["inspectionNo"].ToString(), ref sqls, ref objs);

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_INSPECTIONREPACK, "inspectionNo", ht["inspectionNo"].ToString(), ht);
            if (IsOk)
            { 
                bool flag=DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0 ? true : false;
                if (flag)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_INSPECTIONREPACK, "inspectionNo", ht["inspectionNo"].ToString());
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }
                return JsonHelper.HashtableToJson(ht_result);
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！");
                return JsonHelper.HashtableToJson(ht_result);
            }
        }

        /// <summary>
        /// 编辑预验产品信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string editProduct(HttpContext context) {
            DataTable dt = new DataTable();
            string result = "";
            string inspectionNo = context.Request["inspectionNo"] == null ? "" : context.Request["inspectionNo"].ToString();
            SqlParam[] sqls = new SqlParam[]
            {
                new SqlParam("@inspectionNo", inspectionNo),
            };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from " + ConstantUtil.TABLE_INSPECTIONRE + " where inspectionNo=@inspectionNo");
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, sqls);
            if (DataTableHelper.IsExistRows(dt))
            {
                DataRow dr = dt.Rows[0];
                result = JsonHelper.DataRowToJson_(dr);
            }
            return result;
        }
        /// <summary>
        /// 编辑预验包装信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string editPack(HttpContext context){
            DataTable dtPack = new DataTable();
            string result = "";
            string inspectionNo = context.Request["inspectionNo"] == null ? "" : context.Request["inspectionNo"].ToString();
            SqlParam[] sqls = new SqlParam[]
            {
                new SqlParam("@inspectionNo", inspectionNo),
            };
            StringBuilder sqlPack = new StringBuilder();
            sqlPack.Append("select * from " + ConstantUtil.TABLE_INSPECTIONREPACK + " where inspectionNo=@inspectionNo");
            dtPack = DataFactory.SqlDataBase().GetDataTableBySQL(sqlPack, sqls);

            StringBuilder sqlPack2 = new StringBuilder();
            sqlPack2.Append("select * from " + ConstantUtil.TABLE_INSPECTIONREPACK2 + " where inspectionNo=@inspectionNo");
            DataTable dtPack2 = DataFactory.SqlDataBase().GetDataTableBySQL(sqlPack2, sqls);

            if (DataTableHelper.IsExistRows(dtPack))
            {
                dtPack.Columns.Add("PACKUSENO1");
                dtPack.Columns.Add("PACKUSENO2");
                dtPack.Columns.Add("PACKUSENO3");
                dtPack.Columns.Add("PACKUSENO4");
                dtPack.Columns.Add("PACKUSENO5");
                DataRow dr = dtPack.Rows[0];
                
                for (int i = 0; i < dtPack2.Rows.Count; i++) {
                    dr["PACKUSENO" + (i + 1)] = dtPack2.Rows[i]["packuseno"];
                }
                    result = JsonHelper.DataRowToJson_(dr);
            }
            return result;
        }

        /// <summary>
        /// 私有方法生成预验包装结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="list"></param>
        private static void createList(HttpContext context, List<Hashtable> list)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (!string.IsNullOrEmpty(context.Request["PACKUSENO" + i]))
                {
                    Hashtable ht = new Hashtable();
                    ht["inspectionNo"] = string.IsNullOrEmpty(context.Request["INSPECTIONNO"]) ? "" : context.Request["INSPECTIONNO"].ToString();
                    ht["useabledate"] = string.IsNullOrEmpty(context.Request["USEABLEDATE"]) ? "" : context.Request["USEABLEDATE"].ToString();
                    ht["packuseno"] = string.IsNullOrEmpty(context.Request["PACKUSENO" + i]) ? "" : context.Request["PACKUSENO" + i].ToString();
                    list.Add(ht);
                }

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