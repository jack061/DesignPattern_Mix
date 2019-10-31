using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using WZX.Busines.DAL;
using WZX.Busines;
using RM.Busines;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using RM.Busines.IDAO;
using RM.Busines.DAL.TrainApply;
using RM.Common;
using RM.Busines.DAL.Inspect;
using System.Collections;
using WZX.Busines.Util;
using RM.Common.DotNetData;
using RM.Common.DotNetBean;

namespace RM.Web.ashx.Inspect
{
    /// <summary>
    /// InspectHandler 的摘要说明
    /// </summary>
    public class InspectHandler : IHttpHandler,System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "add"://添加
                    context.Response.Write(add(context));
                    break;
                case "edit":
                    context.Response.Write(edit(context));
                    break;
                case "getSubList":
                    context.Response.Write(getSubList(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string edit(HttpContext context)
        {
            DataTable dt = new DataTable();
            string result = "";
            string inspectionNo = context.Request["inspectionNo"] == null ? "" : context.Request["inspectionNo"].ToString();
            SqlParam[] sqls = new SqlParam[]
            {
                new SqlParam("@inspectionNo", inspectionNo),
            };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from " + ConstantUtil.TABLE_INSPECTION + " where inspectionNo=@inspectionNo");
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, sqls);
            if (DataTableHelper.IsExistRows(dt))
            {
                DataRow dr = dt.Rows[0];
                result = JsonHelper.DataRowToJson_(dr);
            }
            return result;
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;

            //获取查询条件
            string inspectionNo = (context.Request["inspectionNo"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and  inspectionNo = '" + inspectionNo + "'");
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = Inspect_Dal.GetSubList(SqlWhere, IList_param, page, row, ref count);
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
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string add(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();

            ht["inspectionNo"] = string.IsNullOrEmpty(context.Request["INSPECTIONNO"]) ? "" : context.Request["INSPECTIONNO"].ToString();
            ht["apasswd"] = string.IsNullOrEmpty(context.Request["APASSWD"]) ? "" : context.Request["APASSWD"].ToString();
            ht["inspectnum"] = string.IsNullOrEmpty(context.Request["INSPECTNUM"]) ? "" : context.Request["INSPECTNUM"].ToString();
            ht["reinspectnum"] = string.IsNullOrEmpty(context.Request["REINSPECTNUM"]) ? "" : context.Request["REINSPECTNUM"].ToString();
            ht["registernum"] = string.IsNullOrEmpty(context.Request["REGISTERNUM"]) ? "" : context.Request["REGISTERNUM"].ToString();
            ht["produce"] = string.IsNullOrEmpty(context.Request["PRODUCE"]) ? "" : context.Request["PRODUCE"].ToString();
            ht["inspectman"] = string.IsNullOrEmpty(context.Request["INSPECTMAN"]) ? "" : context.Request["INSPECTMAN"].ToString();
            ht["property"] = string.IsNullOrEmpty(context.Request["PROPERTY"]) ? "" : context.Request["PROPERTY"].ToString();
            ht["inspecttype"] = string.IsNullOrEmpty(context.Request["INSPECTTYPE"]) ? "" : context.Request["INSPECTTYPE"].ToString();
            ht["inspectdept"] = string.IsNullOrEmpty(context.Request["INSPECTDEPT"]) ? "" : context.Request["INSPECTDEPT"].ToString();
            ht["purposedept"] = string.IsNullOrEmpty(context.Request["PURPOSEDEPT"]) ? "" : context.Request["PURPOSEDEPT"].ToString();
            ht["applydate"] = string.IsNullOrEmpty(context.Request["APPLYDATE"]) ? "" : context.Request["APPLYDATE"].ToString();
            ht["linkman"] = string.IsNullOrEmpty(context.Request["LINKMAN"]) ? "" : context.Request["LINKMAN"].ToString();
            ht["phone"] = string.IsNullOrEmpty(context.Request["PHONE"]) ? "" : context.Request["PHONE"].ToString();
            ht["shipper"] = string.IsNullOrEmpty(context.Request["SHIPPER"]) ? "" : context.Request["SHIPPER"].ToString();
            ht["shippercn"] = string.IsNullOrEmpty(context.Request["SHIPPERCN"]) ? "" : context.Request["SHIPPERCN"].ToString();

            ht["shippereg"] = string.IsNullOrEmpty(context.Request["SHIPPEREG"]) ? "" : context.Request["SHIPPEREG"].ToString();
            ht["receiver"] = string.IsNullOrEmpty(context.Request["RECEIVER"]) ? "" : context.Request["RECEIVER"].ToString();
            ht["receivercn"] = string.IsNullOrEmpty(context.Request["RECEIVERCN"]) ? "" : context.Request["RECEIVERCN"].ToString();
            ht["receivereg"] = string.IsNullOrEmpty(context.Request["RECEIVEREG"]) ? "" : context.Request["RECEIVEREG"].ToString();
            ht["transport"] = string.IsNullOrEmpty(context.Request["TRANSPORT"]) ? "" : context.Request["TRANSPORT"].ToString();
            ht["tocountry"] = string.IsNullOrEmpty(context.Request["TOCOUNTRY"]) ? "" : context.Request["TOCOUNTRY"].ToString();
            ht["fromharbor"] = string.IsNullOrEmpty(context.Request["FROMHARBOR"]) ? "" : context.Request["FROMHARBOR"].ToString();
            ht["exitharbor"] = string.IsNullOrEmpty(context.Request["EXITHARBOR"]) ? "" : context.Request["EXITHARBOR"].ToString();
            ht["location"] = string.IsNullOrEmpty(context.Request["LOCATION"]) ? "" : context.Request["LOCATION"].ToString();
            ht["contractno"] = string.IsNullOrEmpty(context.Request["CONTRACTNO"]) ? "" : context.Request["CONTRACTNO"].ToString();
            ht["attachdoc"] = string.IsNullOrEmpty(context.Request["ATTACHDOC"]) ? "" : context.Request["ATTACHDOC"].ToString();
            ht["needdoc"] = string.IsNullOrEmpty(context.Request["NEEDDOC"]) ? "" : context.Request["NEEDDOC"].ToString();
            ht["attachment"] = string.IsNullOrEmpty(context.Request["ATTACHMENT"]) ? "" : context.Request["ATTACHMENT"].ToString();
            ht["applytype"] = string.IsNullOrEmpty(context.Request["APPLYTYPE"]) ? "" : context.Request["APPLYTYPE"].ToString();
            ht["remark"] = string.IsNullOrEmpty(context.Request["REMARK"]) ? "" : context.Request["REMARK"].ToString();
            ht["status"] = string.IsNullOrEmpty(context.Request["statush"]) ? "" : context.Request["statush"].ToString();

            //ht["createman"] = string.IsNullOrEmpty(context.Request["CREATEMAN"]) ? "" : context.Request["CREATEMAN"].ToString();
            //ht["createdate"] = string.IsNullOrEmpty(context.Request["CREATEDATE"]) ? "" : context.Request["CREATEDATE"].ToString();
            //ht["lastmod"] = string.IsNullOrEmpty(context.Request["LASTMOD"]) ? "" : context.Request["LASTMOD"].ToString();
            //ht["lastmoddate"] = string.IsNullOrEmpty(context.Request["LASTMODDATE"]) ? "" : context.Request["LASTMODDATE"].ToString();

            ht["createman"] = string.IsNullOrEmpty(context.Request["CREATEMAN"]) ? "" : context.Request["CREATEMAN"].ToString();
            ht["createdate"] = string.IsNullOrEmpty(context.Request["CREATEDATE"]) ? "" : context.Request["CREATEDATE"].ToString();
            //ht["lastmod"] = RequestSession.GetSessionUser().UserName.ToString();
            ht["lastmod"] = string.IsNullOrEmpty(context.Request["LASTMOD"]) ? "" : context.Request["LASTMOD"].ToString();
            ht["lastmoddate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); 

            //处理子表信息
            string subDataJson = context.Request["datagrid"] == null ? "" : context.Request["datagrid"].ToString();
            StringBuilder[] sqls = null;
            object[] objs = null;
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
                    if (!(list[i].Contains("INSPECTIONNO")))
                    {
                        list[i].Add("INSPECTIONNO", ht["inspectionNo"].ToString());
                    }
                }
                sqls = new StringBuilder[list.Count + 1];
                objs = new object[list.Count + 1];
                SqlUtil.getBatchFromList(list, ConstantUtil.TABLE_INSPECTIONPRODUCT, "inspectionNo", ht["inspectionNo"].ToString(), ref sqls, ref objs);
            }

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_INSPECTION, "inspectionNo", ht["inspectionNo"].ToString(), ht);
            if (IsOk)
            {
                bool flag = true;
                if (!(string.IsNullOrEmpty(subDataJson)))
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
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_INSPECTION, "inspectionNo", ht["inspectionNo"].ToString());
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}