using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using WZX.Busines.Util;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using System.Web.SessionState;
using RM.Common.DotNetData;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// BFactory 的摘要说明
    /// </summary>
    public class BFactory : IHttpHandler, IRequiresSessionState
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
                case "getFactoryInfo"://获取供应商信息
                    context.Response.Write(getFactoryInfo(context));
                    break;
                case "add"://添加
                    context.Response.Write(add(context));
                    break;
                case "getSubList0"://获取通讯信息列表
                    context.Response.Write(getSubList0(context));
                    break;
                case "getSubList1"://获取收货信息列表
                    context.Response.Write(getSubList1(context));
                    break;
                case "edit"://修改
                    context.Response.Write(edit(context));
                    break;
                case "upload"://上传
                    context.Response.Write(uploadFile(context));
                    break;
                case "del"://删除
                    context.Response.Write(del(context));
                    break;
                default://默认
                    context.Response.Write("");
                    break;
            }
        }
        //获取列表
        private string getList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            string sort = context.Request["sort"] ?? "code";
            string order = context.Request["order"] ?? "asc";
            //获取查询条件

            string classific = context.Request.Params["classific"];
            string code = context.Request.Params["code"];
            string name = context.Request.Params["name"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();
            StringBuilder sqlcount = new StringBuilder();
            StringBuilder sqlwhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(classific))
            {
                sqlwhere.Append("and bs.classific like '%'+@classific+'%' ");
            }
            if (!string.IsNullOrEmpty(code))
            {
                sqlwhere.Append("and bs.code like '%'+@code+'%' ");
            }
            if (!string.IsNullOrEmpty(name))
            {
                sqlwhere.Append("and bs.name like '%'+@name+'%' ");
            }
            sqlwhere.Append(" and bs.status=1");
            SqlParameter[] pms = new SqlParameter[] 
                {

                      new SqlParameter{ParameterName="@classific",Value=classific,DbType=DbType.String},
                    new SqlParameter{ParameterName="@code",Value=code,DbType=DbType.String},
                    new SqlParameter{ParameterName="@name",Value=name,DbType=DbType.String}
                };

            sqldata.Append(@"select us.UserRealName,bs.* 
                            from bsupplier bs
                            left join Com_UserInfos us on us.Userid=bs.createman  where" + sqlwhere.ToString());
            sqlcount.Append("select count(1) from bsupplier bs where " + sqlwhere.ToString());
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);

            return sb.ToString();
        }

        //获取供应商信息
        private string getFactoryInfo(HttpContext context)
        {
            DataTable dt = new DataTable();
            string code = context.Request.Params["code"]??"";
            SqlParam[] sqlpps = new SqlParam[]
            {
                new SqlParam("@code", code),
            };
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from (select a.*,b.phone,b.email,b.name as contact_name, b.address as contact_address from " + ConstantUtil.TABLE_SUPPLIER + " a left join " + ConstantUtil.TABLE_SUPPLIER_CONTACT + " b on a. code = b.code) t where code=@code");
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, sqlpps);
            StringBuilder sb = new StringBuilder();
            if (dt == null || !DataTableHelper.IsExistRows(dt))
            {
                sb.Append("{}");
            }
            else
            {
                DataRow dr = dt.Rows[0];
                sb.Append(JsonHelper.DataRowToJson_(dr));
            }
            return sb.ToString();
        }

        //添加
        private string add(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();

            string informationName = (context.Request["informationName"] ?? "").ToString().Trim();
            string informationUrl = (context.Request["informationUrl"] ?? "").ToString().Trim();
            string businesslicenseName = (context.Request["businesslicenseName"] ?? "").ToString().Trim();
            string businesslicenseUrl = (context.Request["businesslicenseUrl"] ?? "").ToString().Trim();

            #region 去除字段中的换行，引号等特殊字符
            string name = context.Request["NAME"] == null ? "" : context.Request["NAME"].ToString();
            string address = context.Request["ADDRESS"] == null ? "" : context.Request["ADDRESS"].ToString();
            string egname = context.Request["EGNAME"] == null ? "" : context.Request["EGNAME"].ToString();
            string egaddress = context.Request["EGADDRESS"] == null ? "" : context.Request["EGADDRESS"].ToString();
            string rsname = context.Request["RSNAME"] == null ? "" : context.Request["RSNAME"].ToString();
            string rsaddress = context.Request["RSADDRESS"] == null ? "" : context.Request["RSADDRESS"].ToString();
            string icnBank = context.Request["ICNBANK"] == null ? "" : context.Request["ICNBANK"].ToString();
            string icnAddress = context.Request["ICNADDRESS"] == null ? "" : context.Request["ICNADDRESS"].ToString();
            name = JsonHelper.StringFormat1(name);
            address = JsonHelper.StringFormat1(address);
            egname = JsonHelper.StringFormat1(egname);
            egaddress = JsonHelper.StringFormat1(egaddress);
            rsname = JsonHelper.StringFormat1(rsname);
            rsaddress = JsonHelper.StringFormat1(rsaddress);
            icnBank = JsonHelper.StringFormat1(icnBank);
            icnAddress = JsonHelper.StringFormat1(icnAddress);
            #endregion
            //基本信息
            ht["code"] = context.Request["CODE"] == null ? "" : context.Request["CODE"].ToString();
            ht["shortname"] = context.Request["SHORTNAME"] == null ? "" : context.Request["SHORTNAME"].ToString();
            ht["name"] = name;
            ht["address"] = address;
            ht["egname"] = egname;
            ht["egaddress"] = egaddress;
            ht["rsname"] = rsname;
            ht["rsaddress"] = rsaddress;
            ht["currency"] = context.Request["currency"] == null ? "" : context.Request["currency"].ToString();
            ht["property"] = context.Request["property"] == null ? "" : context.Request["property"].ToString();
            ht["category"] = context.Request["CATEGORY"] == null ? "" : context.Request["CATEGORY"].ToString();
            //ht["information"] = context.Request["information"] == null ? "" : context.Request["information"].ToString();
            //ht["businesslicense"] = context.Request["businesslicense"] == null ? "" : context.Request["businesslicense"].ToString();
            ht["salesman"] = context.Request["SALESMAN"] == null ? "" : context.Request["SALESMAN"].ToString();
            ht["status"] = "1";
            ht["classific"] = context.Request["classific"] ?? "" ;//供应商类型(境内或境外)

            ht["informationName"] = informationName;
            ht["informationUrl"] = informationUrl;
            ht["businesslicenseName"] = businesslicenseName;
            ht["businesslicenseUrl"] = businesslicenseUrl;

            //服务信息
            ht["icnname"] = context.Request["ICNNAME"] == null ? "" : context.Request["ICNNAME"].ToString();
            ht["icncreditcode"] = context.Request["ICNCREDITCODE"] == null ? "" : context.Request["ICNCREDITCODE"].ToString();
            ht["icnbank"] = icnBank;
            ht["icnaccount"] = context.Request["ICNACCOUNT"] == null ? "" : context.Request["ICNACCOUNT"].ToString();
            ht["icnaddress"] = icnAddress;
            ht["icnphone"] = context.Request["ICNPHONE"] == null ? "" : context.Request["ICNPHONE"].ToString();
            ht["iegname"] = context.Request["IEGNAME"] == null ? "" : context.Request["IEGNAME"].ToString();
            ht["iegcreditcode"] = context.Request["IEGCREDITCODE"] == null ? "" : context.Request["IEGCREDITCODE"].ToString();
            ht["iegbank"] = context.Request["IEGBANK"] == null ? "" : context.Request["IEGBANK"].ToString();
            ht["iegaccount"] = context.Request["IEGACCOUNT"] == null ? "" : context.Request["IEGACCOUNT"].ToString();
            ht["iegaddress"] = context.Request["IEGADDRESS"] == null ? "" : context.Request["IEGADDRESS"].ToString();
            ht["iegphone"] = context.Request["IEGPHONE"] == null ? "" : context.Request["IEGPHONE"].ToString();
            ht["createman"] = RequestSession.GetSessionUser().UserName;
            ht["createdate"] = DateTimeHelper.ShortDateTimeS;
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            List<Hashtable> rows = new List<Hashtable>();
            #region 通讯信息
            var msgInfo = context.Request["datagrid0"];
            if (msgInfo.Length > 0)
            {
                objs.Add(new SqlParam[0]);
                sqls.Add(new StringBuilder("delete bsupplier_contact where code='" + ht["code"] + "'"));//删除发货人
                rows = JsonHelper.DeserializeJsonToList<Hashtable>(msgInfo);
                for (int i = 0; i < rows.Count; i++)
                {
                    sqls.Add(new StringBuilder(@"insert into bsupplier_contact ( code,name,address,phone,email) values(@code,@name,@address,@phone,@email)"));
                    objs.Add(new SqlParam[]{
                        new SqlParam("@code",ht["code"]),
                        new SqlParam("@name",rows[i]["NAME"]??""),
                        new SqlParam("@address",rows[i]["ADDRESS"]??""),
                        new SqlParam("@phone",rows[i]["PHONE"]??""),
                        new SqlParam("@email",rows[i]["EMAIL"]??""),
                        //new SqlParam("@foreignAddress",rows[i]["FOREIGNADDRESS"]??""),
                        //new SqlParam("@email",rows[i]["EMAIL"]??""),
                    });
                }
            }
            #endregion


            //对主表操作
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_SUPPLIER, "code", ht["code"].ToString(), ht);
            if (IsOk)
            { 
                    bool flag = true;
                    if (sqls.Count > 0)
                        flag = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs) >= 0 ? true : false;

                    if (flag)
                    {
                        ht_result.Add("status", "T");
                        ht_result.Add("msg", "操作成功！");
                    }
                    else
                    {
                        ht_result.Add("status", "F");
                        ht_result.Add("msg", "主表操作成功，子表操作失败！");
                    }
            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "主表操作失败！");
            }
            return JsonHelper.HashtableToJson(ht_result);
        }
        /// <summary>
        /// 分页获取通讯信息子表列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubList(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            string code = context.Request["code"] == null ? "" : context.Request["code"].ToString();

            BD_Dal bd_dal = new BD_Dal();
            DataTable dt = bd_dal.GetFacContact(code);
            StringBuilder sb = new StringBuilder();
            count = dt.Rows.Count;
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
        /// 分页获取通讯信息子表列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubList0(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            string code = context.Request["code"] == null ? "" : context.Request["code"].ToString();

            StringBuilder sql = new StringBuilder(String.Empty);
            sql.Append(" select * from bsupplier_contact where code=@code ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, new Common.DotNetCode.SqlParam[] { new Common.DotNetCode.SqlParam("@code", code) });

            count = dt.Rows.Count;
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
        /// 分页获取收货信息子表列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubList1(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;
            string code = context.Request["code"] == null ? "" : context.Request["code"].ToString();

            BD_Dal bd_dal = new BD_Dal();
            DataTable dt = bd_dal.GetSupplierDelivery(code);
            count = dt.Rows.Count;
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
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string edit(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();

            string informationName = (context.Request["informationName"] ?? "").ToString().Trim();
            string informationUrl = (context.Request["informationUrl"] ?? "").ToString().Trim();
            string businesslicenseName = (context.Request["businesslicenseName"] ?? "").ToString().Trim();
            string businesslicenseUrl = (context.Request["businesslicenseUrl"] ?? "").ToString().Trim();
            #region 去除字段中的换行，引号等特殊字符
            string name = context.Request["NAME"] == null ? "" : context.Request["NAME"].ToString();
            string address = context.Request["ADDRESS"] == null ? "" : context.Request["ADDRESS"].ToString();
            string egname = context.Request["EGNAME"] == null ? "" : context.Request["EGNAME"].ToString();
            string egaddress = context.Request["EGADDRESS"] == null ? "" : context.Request["EGADDRESS"].ToString();
            string rsname = context.Request["RSNAME"] == null ? "" : context.Request["RSNAME"].ToString();
            string rsaddress = context.Request["RSADDRESS"] == null ? "" : context.Request["RSADDRESS"].ToString();
            string icnBank = context.Request["ICNBANK"] == null ? "" : context.Request["ICNBANK"].ToString();
            string icnAddress = context.Request["ICNADDRESS"] == null ? "" : context.Request["ICNADDRESS"].ToString();
            name = JsonHelper.StringFormat1(name);
            address = JsonHelper.StringFormat1(address);
            egname = JsonHelper.StringFormat1(egname);
            egaddress = JsonHelper.StringFormat1(egaddress);
            rsname = JsonHelper.StringFormat1(rsname);
            rsaddress = JsonHelper.StringFormat1(rsaddress);
            icnBank = JsonHelper.StringFormat1(icnBank);
            icnAddress = JsonHelper.StringFormat1(icnAddress);
            #endregion
            //基本信息
            ht["code"] = context.Request["CODE"] == null ? "" : context.Request["CODE"].ToString();
            ht["shortname"] = context.Request["SHORTNAME"] == null ? "" : context.Request["SHORTNAME"].ToString();
            ht["name"] = name;
            ht["address"] = address;
            ht["egname"] = egname;
            ht["egaddress"] = egaddress;
            ht["rsname"] = rsname;
            ht["rsaddress"] = rsaddress;
            ht["currency"] = context.Request["CURRENCY"] == null ? "" : context.Request["CURRENCY"].ToString();
            ht["property"] = context.Request["PROPERTY"] == null ? "" : context.Request["PROPERTY"].ToString();
            ht["category"] = context.Request["CATEGORY"] == null ? "" : context.Request["CATEGORY"].ToString();
            //ht["information"] = context.Request["information"] == null ? "" : context.Request["information"].ToString();
            //ht["businesslicense"] = context.Request["businesslicense"] == null ? "" : context.Request["businesslicense"].ToString();
            ht["salesman"] = context.Request["SALESMAN"] == null ? "" : context.Request["SALESMAN"].ToString();
            ht["classific"] = context.Request["classific"] ?? "";//供应商类型(境内或境外)
            ht["createman"] = RequestSession.GetSessionUser().UserAccount;
            ht["createdate"] = DateTimeHelper.ShortDateTimeS;
            ht["informationName"] = informationName;
            ht["informationUrl"] = informationUrl;
            ht["businesslicenseName"] = businesslicenseName;
            ht["businesslicenseUrl"] = businesslicenseUrl;


            //服务信息
            ht["icnname"] = context.Request["ICNNAME"] == null ? "" : context.Request["ICNNAME"].ToString();
            ht["icncreditcode"] = context.Request["ICNCREDITCODE"] == null ? "" : context.Request["ICNCREDITCODE"].ToString();
            ht["icnbank"] = icnBank;
            ht["icnaccount"] = context.Request["ICNACCOUNT"] == null ? "" : context.Request["ICNACCOUNT"].ToString();
            ht["icnaddress"] = icnAddress;
            ht["icnphone"] = context.Request["ICNPHONE"] == null ? "" : context.Request["ICNPHONE"].ToString();
            ht["iegname"] = context.Request["IEGNAME"] == null ? "" : context.Request["IEGNAME"].ToString();
            ht["iegcreditcode"] = context.Request["IEGCREDITCODE"] == null ? "" : context.Request["IEGCREDITCODE"].ToString();
            ht["iegbank"] = context.Request["IEGBANK"] == null ? "" : context.Request["IEGBANK"].ToString();
            ht["iegaccount"] = context.Request["IEGACCOUNT"] == null ? "" : context.Request["IEGACCOUNT"].ToString();
            ht["iegaddress"] = context.Request["IEGADDRESS"] == null ? "" : context.Request["IEGADDRESS"].ToString();
            ht["iegphone"] = context.Request["IEGPHONE"] == null ? "" : context.Request["IEGPHONE"].ToString();

            WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll();
            var msgContact = context.Request["datagrid"];
            if (msgContact == "null")
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "通讯信息不能为空！");

                return JsonHelper.HashtableToJson(ht_result);

            }

            string sqlContact = string.Empty;
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            List<Hashtable> rows = new List<Hashtable>();
            #region 通讯信息
            var msgInfo = context.Request["datagrid0"];
            if (msgInfo.Length > 0)
            {
                objs.Add(new SqlParam[0]);
                sqls.Add(new StringBuilder("delete bsupplier_contact where code='" + ht["code"] + "'"));//删除发货人
                rows = JsonHelper.DeserializeJsonToList<Hashtable>(msgInfo);
                for (int i = 0; i < rows.Count; i++)
                {
                    sqls.Add(new StringBuilder(@"insert into bsupplier_contact ( code,name,address,phone,email) values(@code,@name,@address,@phone,@email)"));
                    objs.Add(new SqlParam[]{
                        new SqlParam("@code",ht["code"]),
                        new SqlParam("@name",rows[i]["NAME"]??""),
                        new SqlParam("@address",rows[i]["ADDRESS"]??""),
                        new SqlParam("@phone",rows[i]["PHONE"]??""),
                        new SqlParam("@email",rows[i]["EMAIL"]??""),
                        //new SqlParam("@foreignAddress",rows[i]["FOREIGNADDRESS"]??""),
                        //new SqlParam("@email",rows[i]["EMAIL"]??""),
                    });
                }
            }
            #endregion
        
            //对主表操作
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_SUPPLIER, "code", ht["code"].ToString(), ht);
            if (IsOk)
            {
                bool flag = true;
                if (sqls.Count > 0)
                    flag = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs) >= 0 ? true : false;

                if (flag)
                {
                    ht_result.Add("status", "T");
                    ht_result.Add("msg", "操作成功！");
                }
                else
                {
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "主表操作成功，子表操作失败！");
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
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string del(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            string pkName = context.Request["pkName"];              //字段主键
            string pkVal = context.Request["pkVal"];
            string tableName = context.Request["tableName"];        //数据库表
            string code = context.Request.QueryString["code"];
            RM_System_IDAO systemidao = new RM_System_Dal();
            BD_IDAO basedataDao = new BD_Dal();
            int a = basedataDao.Virtualdelete(pkName, pkVal, tableName);
            if (a > 0)
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
        //信息表与营业执照上传
        private string uploadFile(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                var file = context.Request.Files[0];

                string path = "/Files/BaseData/bsupplier/" + DateTime.Now.ToString("yyyyMMddHHmmss") + file.FileName;
                Directory.CreateDirectory(Path.GetDirectoryName(context.Server.MapPath(path)));

                file.SaveAs(context.Server.MapPath(path));

                return path + ":" + file.FileName;
            }
            else
            {
                return "error";
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