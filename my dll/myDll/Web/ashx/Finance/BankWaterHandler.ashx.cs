using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines.DAL.Finance;
using RM.Common.DotNetJson;
using System.Collections;
using RM.Common.DotNetBean;
using RM.Busines;
using WZX.Busines.Util;
using System.IO;
using RM.Common.DotNetFile;
using RM.Common.DotNetData;
using System.Web.SessionState;
using RM.Common.DotNetConfig;

namespace RM.Web.ashx.Finance
{
    /// <summary>
    /// BankWaterHandler 的摘要说明
    /// </summary>
    public class BankWaterHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (action)
            {
                case "getList"://获取列表
                    context.Response.Write(getList(context));
                    break;
                case "getSubList"://获取列表
                    context.Response.Write(getSubList(context));
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
                case "load"://加载
                    context.Response.Write(load(context));
                    break;
                case "loadModule"://下载模板
                    loadModule(context);
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;

            //获取查询条件
            string bankname = (context.Request["bankname"] ?? "").ToString().Trim();
            string accountname = (context.Request["accountname"] ?? "").ToString().Trim();
            string account = (context.Request["account"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" ");
            if (bankname.Length > 0)
            {
                SqlWhere.Append(" and  bankname like '%" + bankname + "%'");
            }
            if (accountname.Length > 0)
            {
                SqlWhere.Append(" and  accountname like '%" + accountname + "%'");
            }
            if (account.Length > 0)
            {
                SqlWhere.Append(" and  account like '%" + account + "%'");
            }
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = BankWater_Dal.GetBankWaterListPage(SqlWhere, IList_param, page, row, ref count);
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
            string docno = (context.Request["docno"] ?? "").ToString().Trim();

            //查询条件
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append(" and  docno = '" + docno + "'");
            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = BankWater_Dal.GetBankWaterListPage_D(SqlWhere, IList_param, page, row, ref count);
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
            ht = RequestHelper.getDataFromRequestForm_1(context);
            //根据操作类型进行相关操作（比如操作人、操作时间）
            if (ht.Contains("action")) 
            {
                if("add".Equals(ht["action"]))
                {//添加

                }
                if ("edit".Equals(ht["action"]))
                {//修改

                }
                ht.Remove("action");
            }
            //获取子表信息
            string subDataJson = "";
            if (ht.Contains("datagrid")) 
            {
               subDataJson = ht["datagrid"].ToString();
               ht.Remove("datagrid");
            }
            List<StringBuilder> sqls =new List<StringBuilder>();
            List<object> objs = new List<object>();

            //生成主表sql
            SqlUtil.getBatchSqls(ht, ConstantUtil.TABLE_BANK_WATER, "DOCNO", ht["DOCNO"].ToString(), ref sqls, ref objs);

            List<Hashtable> list = new List<Hashtable>();
            if (!(string.IsNullOrEmpty(subDataJson))) 
            {
                list = JsonHelper.DeserializeJsonToList<Hashtable>(subDataJson);
                string time = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
                string man = RequestSession.GetSessionUser().UserName.ToString();
                for (int i = 0; i < list.Count;i++ )
                {
                    if(list[i].Contains("ROWNUM"))
                    {
                        list[i].Remove("ROWNUM");
                    }
                    if (!(list[i].Contains("DOCNO")))
                    {
                        list[i].Add("DOCNO", ht["DOCNO"].ToString());
                        list[i].Add("INDATE", time);
                        list[i].Add("INMAN", man);
                    }
                }
                SqlUtil.getBatchSqls(list, ConstantUtil.TABLE_BANK_WATER_D, "DOCNO", ht["DOCNO"].ToString(), ref sqls, ref objs);
            }

            bool IsOk = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs) >= 0 ? true : false;
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
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string edit(HttpContext context)
        {
            DataTable dt = new DataTable();
            string result = "";
            string number = context.Request["docno"] == null ? "" : context.Request["docno"].ToString();
            SqlParam[] sqls = new SqlParam[]
            {
                new SqlParam("@docno", number),
            };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from " + ConstantUtil.TABLE_BANK_WATER + " where docno=@docno");
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, sqls);
            if (DataTableHelper.IsExistRows(dt))
            {
                DataRow dr = dt.Rows[0];
                result = JsonHelper.DataRowToJson_(dr);
            }
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string del(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();

            string docno = string.IsNullOrEmpty(context.Request["docno"]) ? "" : context.Request["docno"].ToString();
            StringBuilder sb = new StringBuilder();
            SqlParam[] param = { new SqlParam("@docno", docno) };
            sb.Append(" delete " + ConstantUtil.TABLE_BANK_WATER + " where docno  = @docno");
            sb.Append(" delete " + ConstantUtil.TABLE_BANK_WATER_D + " where docno  = @docno");
            if (DataFactory.SqlDataBase().ExecuteBySql(sb, param) > 0)
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
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private void loadModule(HttpContext context)
        {
            string virPath = ConstantUtil.FILE_MODEL + ConfigHelper.GetAppSettings("BankWatertModel");
            string newName = DateTimeHelper.GetToday("yyyyMMdd") + "_" + ConfigHelper.GetAppSettings("BankWatertModel");
            string filePath = context.Server.MapPath(virPath);//Server.MapPath 获得虚拟服务器相对路径
            if (!(File.Exists(filePath)))
            {
                context.Response.Write("模板文件不存在，请联系系统管理员!");

            }
            FileDownHelper.DownLoadold(virPath, newName);

        }
        /// <summary>
        ///加载文件
        /// </summary>
        /// <param name="context"></param>
        private string load(HttpContext context)
        {
            string docno = (context.Request["docno"] ?? "").ToString().Trim();
            Hashtable ht_result = new Hashtable();
            try
            {
                HttpPostedFile file = context.Request.Files["Filedata"];
                if (file != null)
                {
                    string oldFileName = file.FileName;//原文件名
                    int size = file.ContentLength;//附件大小

                    string extenstion = oldFileName.Substring(oldFileName.LastIndexOf(".") + 1);//后缀名
                    if (extenstion != "xls" && extenstion != "xlsx")
                    {
                        ht_result.Add("status", "F");
                        ht_result.Add("msg", "只可以选择Excel文件");
                        return JsonHelper.HashtableToJson(ht_result);
                    }
                    string filename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + "." + extenstion; //Execle文件重命名
                    string savePath = context.Server.MapPath(ConstantUtil.FILE_FINANCE_URL);//Server.MapPath 获得虚拟服务器相对路径
                    string saveFullPath = savePath + filename;//文件路径
                    bool flag = Directory.Exists(savePath);
                    if (!(Directory.Exists(savePath)))
                    {//判断路径是否存在---不存在创建路径
                        Directory.CreateDirectory(savePath);
                    }
                    if ((File.Exists(saveFullPath)))
                    {//判断文件是否已经存在，存在删除
                        File.Delete(saveFullPath);
                    }

                    file.SaveAs(saveFullPath);
                    bool uploaded = File.Exists(saveFullPath);

                    if (uploaded)
                    {
                        DataTable dt = null;
                        try
                        {
                            //dt = ExcelHelper.ExcelToDataSet("Sheet1", saveFullPath);
                            dt = NPOIHelper.FormatToDatatable(saveFullPath, "Sheet1");
                        }
                        catch
                        {
                            ht_result.Add("status", "F");
                            ht_result.Add("msg", "导入失败，Excel工作表标签名错误，标签名必须是Sheet1，请查证后再导入!");
                            return JsonHelper.HashtableToJson(ht_result);
                        }

                        if (DataTableHelper.IsExistRows(dt))
                        {
                            int rowsnum = dt.Rows.Count;
                            int columnnum = dt.Columns.Count;
                            if (rowsnum == 0)
                            {
                                ht_result.Add("status", "F");
                                ht_result.Add("msg", "Excel表为空表,无数据!");
                                return JsonHelper.HashtableToJson(ht_result);
                            }
                            else
                            {
                                for (int i = 0; i < columnnum; i++)
                                {//对列名进行处理
                                    dt.Columns[i].ColumnName = SqlUtil.getColumn(dt.Columns[i].ColumnName);
                                }
                                StringBuilder sql = new StringBuilder();
                                SqlParam[] param = new SqlParam[]{};
                                for (int i = 0; i < dt.Rows.Count;i++ )
                                {
                                    dt.Rows[i]["DOCNO"] = docno;
                                    sql.Append("select b.CONTRACTNO from ");
                                    sql.Append(ConstantUtil.TABLE_PAY_ABROAD + " a left join " + ConstantUtil.TABLE_PAY_ABROAD_D);
                                    sql.Append(" b on a.payNo = b.payNo where a.paydate = '" + dt.Rows[i]["TRADEDATE"] + "'");
                                    dt.Rows[i]["CONTRACTNO"] = DataFactory.SqlDataBase().getString(sql, param, "CONTRACTNO");
                                    sql.Clear();
                                }
                                StringBuilder sb = new StringBuilder();
                                if (dt.Rows.Count == 0)
                                {
                                    sb.Append("{\"status\":\"T\",\"total\":0,\"rows\":[]}");
                                }
                                else
                                {
                                    sb.Append("{\"status\":\"T\",\"total\":" + dt.Rows.Count + ",");
                                    sb.Append(JsonHelper.DataTableToJson_(dt, "rows"));
                                    sb.Append("}");
                                }
                                return sb.ToString();
                            }
                        }
                    }

                }
                ht_result.Add("status", "F");
                ht_result.Add("msg", "加载文件失败");
                return JsonHelper.HashtableToJson(ht_result);
            }
            catch (Exception ex)
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "加载文件失败:" + ex.ToString());
                return JsonHelper.HashtableToJson(ht_result);
            }
        }

        private bool saveRecord(DataTable dt)
        {
            bool flag = false;
            StringBuilder[] sqls = null;
            object[] objs = null;
            if (dt != null && dt.Rows.Count != 0)
            {
                sqls = new StringBuilder[dt.Rows.Count];
                objs = new object[dt.Rows.Count];
                List<string> list = new List<string>();
                SqlUtil.getBatchFromDataTable(dt, ConstantUtil.TABLE_BANK_WATER_D, "DOCNO", ref sqls, ref objs);
            }
            else
            {
                return flag;

            }

            flag = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0 ? true : false;
            return flag;

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