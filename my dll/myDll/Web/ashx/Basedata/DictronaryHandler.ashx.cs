using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using RM.Busines;
using WZX.Busines.DAL;
using RM.Common.DotNetJson;
using System.Collections;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using WZX.Busines.Util;
using RM.Busines.DAL;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// DictronaryHandler 的摘要说明
    /// </summary>
    public class DictronaryHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (type)
            {
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;
                case "add"://添加
                    context.Response.Write(add(context));
                    break;
                case "edit"://修改
                    context.Response.Write(edit(context));
                    break;
                case "del"://删除
                    context.Response.Write(del(context));
                    break;
                case "getCurrencyList"://获取列表
                    context.Response.Write(getCurrencyList(context));
                    break;
                case "getUnitList"://获取列表
                    context.Response.Write(getUnitList(context));
                    break;
                case "getProduceUnitList"://获取生产单位列表
                    context.Response.Write(getProduceUnitList(context));
                    break;
                case "getPackUnitList"://获取最小包装列表
                    context.Response.Write(getPackUnitList(context));
                    break;
                case "getPackdesList"://获取包装描述
                    context.Response.Write(getPackdesList(context));
                    break;
                case "getWarehouseList"://获取仓库列表
                    context.Response.Write(getWarehouseList(context));
                    break;
                case "getWarehouseList1"://获取仓库列表---关联组织
                    context.Response.Write(getWarehouseList1(context));
                    break;
                case "getShippngcostItemList"://获取运费条款列表
                    context.Response.Write(getShippngcostItemList(context));
                    break;
                case "getSendFactoryList"://获取发货工厂列表
                    context.Response.Write(getSendFactoryList(context));
                    break;
                case "getTypeList"://废弃，这个方法使用的是bdicdate表
                    context.Response.Write(getTypeList(context));
                    break;
                case "GetDicByParentID"://根据parentid获取所有元素
                    context.Response.Write(GetDicByParentID(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getList(HttpContext context)
        {
            //int row = int.Parse(context.Request["rows"].ToString());
            //int page = int.Parse(context.Request["page"].ToString());
            int count = 0;

            //获取查询条件
            string Id = (context.Request["Id"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            if (!(string.IsNullOrEmpty(Id)))
            {
                SqlWhere.Append(" and id = " + Id);
            }

            IList<SqlParam> IList_param = new List<SqlParam>();
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                DataView dv = new DataView(dt);
                dv.RowFilter = "PARENTID=0";
                dv.Sort = " ID ";
                for (int i = 0; i < dv.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":" + dv[i]["ID"] + ",");
                    sb.Append("\"text\":\"" + dv[i]["NAME"] + "\",");
                    sb.Append("\"CODE\":\"" + dv[i]["CODE"] + "\",");
                    sb.Append("\"ISVALIDATE\":\"" + dv[i]["ISVALIDATE"] + "\",");
                    sb.Append("\"REMARK\":\"" + dv[i]["REMARK"] + "\",");
                    sb.Append("\"ENGLISH\":\"" + dv[i]["ENGLISH"] + "\",");
                    sb.Append("\"RUSSIAN\":\"" + dv[i]["RUSSIAN"] + "\"");
                    DataView dv2 = new DataView(dt);
                    dv2.RowFilter = "PARENTID=" + dv[i]["ID"];
                    dv2.Sort = " ID ";
                    if (dv2.Count > 0)
                    {
                        sb.Append(GetChlid(dv2, dt));
                    }
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
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
            Hashtable ht = new Hashtable();
            Hashtable ht_result = new Hashtable();
            string id = context.Request["id"] == null ? "" : context.Request["id"].ToString();
            ht["name"] = context.Request["name"] == null ? "" : context.Request["name"].ToString();
            ht["parentId"] = context.Request["parentId"] == null ? "" : context.Request["parentId"].ToString();
            ht["code"] = context.Request["code"] == null ? "" : context.Request["code"].ToString();
            ht["isvalidate"] = context.Request["validate"] == null ? "0" : context.Request["validate"].ToString();
            ht["remark"] = context.Request["remark"] == null ? "0" : context.Request["remark"].ToString();
            ht["english"] = context.Request["english"] == null ? "0" : context.Request["english"].ToString();
            ht["russian"] = context.Request["russian"] == null ? "0" : context.Request["russian"].ToString();

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit(ConstantUtil.TABLE_DICTRONARY, "id", id, ht);
            if (IsOk)
            {
                ht_result.Add("status", "T");
                ht_result.Add("msg", "操作成功");
                return JsonHelper.HashtableToJson(ht_result);

            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败");
                return JsonHelper.HashtableToJson(ht_result); ;

            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string edit(HttpContext context)
        {
            int Id = int.Parse(context.Request.QueryString["Id"]);
            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and id = @id");

            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@id", Id));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            Hashtable ht = DataTableHelper.DataTableToHashtable(dt);
            return JsonHelper.HashtableToJson(ht);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string del(HttpContext context)
        {
            string Id = context.Request.QueryString["Id"];
            Com_Organization bll = new Com_Organization();
            string[] str = Id.Split(',');
            List<string> listSql = new List<string>();

            listSql.Add(" delete " + ConstantUtil.TABLE_DICTRONARY + " where Id in(" + Id + ")");
            if (DataFactory.SqlDataBaseExpand().ExecuteSqlTran(listSql) > 0)
            {
                context.Response.Write("true");
            }
            return "";
        }

        protected StringBuilder GetChlid(DataView dv, DataTable dt)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(",\"children\":[");

            for (int i = 0; i < dv.Count; i++)
            {
                sb.Append("{");
                sb.Append("\"id\":" + dv[i]["ID"] + ",");
                sb.Append("\"text\":\"" + dv[i]["NAME"] + "\",");
                sb.Append("\"CODE\":\"" + dv[i]["CODE"] + "\",");
                sb.Append("\"ISVALIDATE\":\"" + dv[i]["ISVALIDATE"] + "\",");
                sb.Append("\"ENGLISH\":\"" + dv[i]["ENGLISH"] + "\",");
                sb.Append("\"RUSSIAN\":\"" + dv[i]["RUSSIAN"] + "\",");
                sb.Append("\"REMARK\":\"" + dv[i]["REMARK"] + "\"");
                DataView dv2 = new DataView(dt);
                dv2.RowFilter = "PARENTID=" + dv[i]["ID"];
                dv2.Sort = " ID ";
                if (dv2.Count > 0)
                {
                    sb.Append(GetChlid(dv2, dt));
                }
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb;
        }

        /// <summary>
        /// 获取货币列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getCurrencyList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and classname = @classname");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@classname", "货币"));
            DataTable dt = BaseData_Dal.GetDicList_1(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["CNAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 生产单位注册号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getProduceUnitList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and PARENTID = @PARENTID");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@PARENTID", "157"));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取运费条款列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getShippngcostItemList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and PARENTID = @PARENTID");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@PARENTID", "159"));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getWarehouseList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and PARENTID = @PARENTID");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@PARENTID", "115"));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取仓库列表 关联组织
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getWarehouseList1(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            DataTable dt = BaseData_Dal.GetWarehouseList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append(JsonHelper.DataRowToJson_(dt.Rows[i]) + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
              }
            return sb.ToString();
        }

        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getUnitList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and PARENTID = @PARENTID");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@PARENTID", "27"));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }
        private string getPackUnitList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and PARENTID = @PARENTID");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@PARENTID", "147"));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取包装描述
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getPackdesList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and PARENTID = @PARENTID");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@PARENTID", "170"));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取销售组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getTypeList(HttpContext context)
        {
            string type = context.Request["type"].ToString();

            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and classname = @classname");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@classname", type));
            DataTable dt = BaseData_Dal.GetDicList_1(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["CNAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取发货工厂列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSendFactoryList(HttpContext context)
        {
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and PARENTID = @PARENTID");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@PARENTID", "120"));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据PARENTID获取项
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetDicByParentID(HttpContext context) {
            string parentId = context.Request["pid"].ToString();

            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and PARENTID = @pid");
            IList<SqlParam> IList_param = new List<SqlParam>();
            IList_param.Add(new SqlParam("@pid", parentId));
            DataTable dt = BaseData_Dal.GetDicList(SqlWhere, IList_param);
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + dt.Rows[i]["CODE"] + "\",");
                    sb.Append("\"text\":\"" + dt.Rows[i]["NAME"] + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
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