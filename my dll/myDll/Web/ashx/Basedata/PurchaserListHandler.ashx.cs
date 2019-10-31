using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using System.Data;
using RM.Common;
using RM.Common.DotNetJson;
using RM.Common.DotNetUI;
using System.Collections;
using WZX.Busines.Util;
using RM.Common.DotNetEncrypt;
using RM.Busines;
using System.IO;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using PDA_Service.DataBase.DataBase.SqlServer;
using RM.Common.DotNetBean;
using System.Web.SessionState;
using RM.Common.DotNetData;

namespace RM.Web.ashx.Basedata
{
    /// <summary>
    /// PurchaserListHandler 的摘要说明
    /// </summary>
    public class PurchaserListHandler : IHttpHandler, IRequiresSessionState
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
                case "getCustomerInfo"://获取客户信息
                    context.Response.Write(getCustomerInfo(context));
                    break;
                case "getSubList"://获取通讯信息列表
                    context.Response.Write(getSubList(context));
                    break;
                case "getSubList0"://获取通讯信息列表
                    context.Response.Write(getSubList0(context));
                    break;
                case "getSubList1"://获取收货信息列表
                    context.Response.Write(getSubList1(context));
                    break;
                case "getSubList2"://获取通知人信息列表
                    context.Response.Write(getSubList2(context));
                    break;
                case "GetJobMan":
                    context.Response.Write(GetJobMan(context));
                    break;
                case "GetJobManRole":
                    context.Response.Write(GetJobManRole(context));
                    break;
                case "GetJobManAngency":
                    context.Response.Write(GetJobManAngency (context));
                    break;
                case "add"://添加
                    context.Response.Write(add(context));
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
                case "load"://加载
                    context.Response.Write(load(context));
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
        /// 
        //信息表与营业执照上传
        private string uploadFile(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                var file = context.Request.Files[0];

                string path = "/Files/BaseData/customer/" + DateTime.Now.ToString("yyyyMMddHHmmss") + file.FileName;
                Directory.CreateDirectory(Path.GetDirectoryName(context.Server.MapPath(path)));

                file.SaveAs(context.Server.MapPath(path));

                return path + ":" + file.FileName;
            }
            else
            {
                return "error";
            }
        }
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
                            from bcustomer bs
                            left join Com_UserInfos us on us.Userid=bs.createman  where" + sqlwhere.ToString());
            sqlcount.Append("select count(1) from bcustomer bs where " + sqlwhere.ToString());
            StringBuilder sb = ui.GetDatatablePageJsonString(sqldata, sqlcount, pms, sort, order, page, row);

            return sb.ToString();
        }

        //获取供客户信息
        private string getCustomerInfo(HttpContext context)
        {
            DataTable dt = new DataTable();
            string code = context.Request.Params["code"] ?? "";
            SqlParam[] sqlpps = new SqlParam[]
            {
                new SqlParam("@code", code),
            };
            StringBuilder sql = new StringBuilder();
           // sql.Append("select * from " + ConstantUtil.TABLE_CUSTOMER + " where code=@code");
            sql.Append("select * from (select a.*,b.phone,b.email,b.name as contact_name, b.address as contact_address from " + ConstantUtil.TABLE_CUSTOMER + " a left join " + ConstantUtil.TABLE_BCUSTOMER_CONTACT + " b on a. code = b.code) t where code=@code");

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
            DataTable dt = bd_dal.GetContact(code);
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
            sql.Append(" select * from bcustomer_contact where code=@code ");
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
            DataTable dt = bd_dal.GetCustormerDeliveryList(code);
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
        /// 分页获取通知人信息子表列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getSubList2(HttpContext context)
        {
            int row = int.Parse(context.Request["rows"].ToString());
            int page = int.Parse(context.Request["page"].ToString());
            int count = 0;

            string code = context.Request["code"] == null ? "" : context.Request["code"].ToString();

            StringBuilder sql = new StringBuilder(String.Empty);
            sql.Append(" select * from bcustomer_notice where code=@code ");
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
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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

            #region 基本信息
            ht["code"] = context.Request["CODE"] == null ? "" : context.Request["CODE"].ToString();
            ht["shortname"] = context.Request["SHORTNAME"] == null ? "" : context.Request["SHORTNAME"].ToString();
            ht["name"] = name;
            ht["address"] = address;
            ht["egname"] = egname;
            ht["egaddress"] = egaddress;
            ht["rsname"] =rsname;
            ht["rsaddress"] =rsaddress;
            ht["currency"] = context.Request["CURRENCY"] == null ? "" : context.Request["CURRENCY"].ToString();
            ht["property"] = context.Request["PROPERTY"] == null ? "" : context.Request["PROPERTY"].ToString();
            ht["category"] = context.Request["CATEGORY"] == null ? "" : context.Request["CATEGORY"].ToString();
            //ht["information"] = context.Request["information"] == null ? "" : context.Request["information"].ToString();
            //ht["businesslicense"] = context.Request["businesslicense"] == null ? "" : context.Request["businesslicense"].ToString();
            ht["salesman"] = context.Request["SALESMAN"] == null ? "" : context.Request["SALESMAN"].ToString();
            ht["status"] = "1";
            ht["createman"] = RequestSession.GetSessionUser().UserName;
            ht["createdate"] = DateTimeHelper.ShortDateTimeS;
            ht["classific"] = context.Request["classific"] == null ? "" : context.Request["classific"].ToString();//境内或境外

            ht["informationName"] = informationName;
            ht["informationUrl"] = informationUrl;
            ht["businesslicenseName"] = businesslicenseName;
            ht["businesslicenseUrl"] = businesslicenseUrl;

            #endregion

            #region 服务信息
            ht["icnname"] = context.Request["ICNNAME"] == null ? "" : context.Request["ICNNAME"].ToString();
            ht["icncreditcode"] = context.Request["ICNCREDITCODE"] == null ? "" : context.Request["ICNCREDITCODE"].ToString();
            ht["icnbank"] = icnBank;
            ht["icnaccount"] = context.Request["ICNACCOUNT"] == null ? "" : context.Request["ICNACCOUNT"].ToString();
            ht["icnaddress"] =icnAddress;
            ht["icnphone"] = context.Request["ICNPHONE"] == null ? "" : context.Request["ICNPHONE"].ToString();
            ht["iegname"] = context.Request["IEGNAME"] == null ? "" : context.Request["IEGNAME"].ToString();
            ht["iegcreditcode"] = context.Request["IEGCREDITCODE"] == null ? "" : context.Request["IEGCREDITCODE"].ToString();
            ht["iegbank"] = context.Request["IEGBANK"] == null ? "" : context.Request["IEGBANK"].ToString();
            ht["iegaccount"] = context.Request["IEGACCOUNT"] == null ? "" : context.Request["IEGACCOUNT"].ToString();
            ht["iegaddress"] = context.Request["IEGADDRESS"] == null ? "" : context.Request["IEGADDRESS"].ToString();
            ht["iegphone"] = context.Request["IEGPHONE"] == null ? "" : context.Request["IEGPHONE"].ToString();
            #endregion


            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            List<Hashtable> rows = new List<Hashtable>();
            #region 通讯信息
            var msgInfo = context.Request["datagrid0"];
            if (msgInfo.Length > 0)
            {
                objs.Add(new SqlParam[0]);
                sqls.Add(new StringBuilder("delete bcustomer_contact where code='" + ht["code"] + "'"));//删除发货人
                rows = JsonHelper.DeserializeJsonToList<Hashtable>(msgInfo);
                for (int i = 0; i < rows.Count; i++)
                {
                    sqls.Add(new StringBuilder(@"insert into bcustomer_contact ( code,name,address,phone,email) values(@code,@name,@address,@phone,@email)"));
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
            

            #region 收货信息
            var msgDelivery = context.Request["datagrid1"];
            if (msgDelivery.Length > 0) {
                objs.Add(new SqlParam[0]);
                sqls.Add(new StringBuilder("delete bcustomer_delivery where code='" + ht["code"] + "'"));//删除发货人
                rows = JsonHelper.DeserializeJsonToList<Hashtable>(msgDelivery);
                for (int i = 0; i < rows.Count; i++)
                {
                    //Hashtable row = rows[i];
                    //if (row.ContainsKey("ROWNUM"))
                    //    row.Remove("ROWNUM");
                    sqls.Add(new StringBuilder(@"insert into bcustomer_delivery ( code, name, foreignName,phone, address,foreignAddress,email) values(@code,@name,@foreignName,@phone,@address,@foreignAddress,@email)"));
                    objs.Add(new SqlParam[]{
                        new SqlParam("@code",ht["code"]),
                        new SqlParam("@name",rows[i]["NAME"]??""),
                        new SqlParam("@foreignName",rows[i]["FOREIGNNAME"]??""),
                        new SqlParam("@phone",rows[i]["PHONE"]??""),
                        new SqlParam("@address",rows[i]["ADDRESS"]??""),
                        new SqlParam("@foreignAddress",rows[i]["FOREIGNADDRESS"]??""),
                        new SqlParam("@email",rows[i]["EMAIL"]??""),
                    });
                }
                //SqlUtil.getBatchSqls(rows, "bcustomer_delivery", ref sqls, ref objs);
            }
            #region old 发货人添加
            //List<Hashtable> Json1=JsonHelper.DeserializeJsonToList<Hashtable>(msgDelivery);
            //List<string> listDeilvery = new List<string>();
            //foreach (var item in Json1)
            //{
            //    listDeilvery.Add(item.Value.ToString());
            //}
            //SqlParameter[] pmsDelivery = new SqlParameter[]{
            //    new SqlParameter("@code",ht["code"]),
            //    new SqlParameter("@name",listDeilvery[0]),
            //    new SqlParameter("@foreignName",listDeilvery[1]),
            //    new SqlParameter("@phone",listDeilvery[2]),
            //    new SqlParameter("@address",listDeilvery[3]),
            //    new SqlParameter("@foreignAddress",listDeilvery[4]),
            //    new SqlParameter("@outHarbor",listDeilvery[5]),
            //    new SqlParameter("@clearanceHarbor",listDeilvery[6]),
            //    new SqlParameter("@foreignOutHarbor",listDeilvery[7]),
            //    new SqlParameter("@foreignClearanceHarbor",listDeilvery[8])              
            //};
            
            //string sqlDelivery = @"insert into bcustomer_delivery ( code, name, foreignName,phone, address,foreignAddress, 
            //                        outHarbor, clearanceHarbor,
            //                        foreignOutHarbor, foreignClearanceHarbor)values(@code,@name,@foreignName,@phone,@address,@foreignAddress
            //                        ,@outHarbor,@clearanceHarbor,@foreignOutHarbor,@foreignClearanceHarbor)";
            #endregion
            #endregion
            #region 通知人信息
            var msgNotice = context.Request["datagrid2"];

            if (msgNotice.Length > 0)
            {
                sqls.Add(new StringBuilder("delete bcustomer_notice where code='" + ht["code"] + "'"));//删除发货人
                objs.Add(new SqlParam[0]);
                rows = JsonHelper.DeserializeJsonToList<Hashtable>(msgNotice);
                for (int i = 0; i < rows.Count; i++)
                {
                    sqls.Add(new StringBuilder(@"insert into bcustomer_notice ( phone,code, name, foreignName, address,foreignAddress,email) values(@phone,@code,@name,@foreignName,@address,@foreignAddress,@email)"));
                    objs.Add(new SqlParam[]{
                        new SqlParam("@phone",rows[i]["PHONE"]??""),
                        new SqlParam("@code",ht["code"]),
                        new SqlParam("@name",rows[i]["NAME"]??""),
                        new SqlParam("@foreignName",rows[i]["FOREIGNNAME"]??""),
                        new SqlParam("@address",rows[i]["ADDRESS"]??""),
                        new SqlParam("@foreignAddress",rows[i]["FOREIGNADDRESS"]??""),
                        new SqlParam("@email",rows[i]["EMAIL"]??""),
                    });
                }
            }
            #endregion

            #region 对主表操作
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_CUSTOMER, "code", ht["code"].ToString(), ht);
            if (IsOk)
            {
                bool flag = true;
                if (flag)
                {
                    if (sqls.Count > 0) {
                        flag = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs) >= 0 ? true : false;
                    }
                    if (flag)
                    {
                        ht_result.Add("status", "T");
                        ht_result.Add("msg", "操作成功！");
                    }
                    else
                    {
                        //删除主表
                        DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_CUSTOMER, "code", ht["code"].ToString());
                        //删除子表
                        DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_BCUSTOMER_DELIVERY, "code", ht["code"].ToString());
                        ht_result.Add("status", "F");
                        ht_result.Add("msg", "操作失败！");
                    }
                }
                else
                {
                    //删除主表
                    DataFactory.SqlDataBase().DeleteData(ConstantUtil.TABLE_CUSTOMER, "code", ht["code"].ToString());
                    ht_result.Add("status", "F");
                    ht_result.Add("msg", "操作失败！");
                }

            }
            else
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "操作失败！");
            }
            #endregion

            return JsonHelper.HashtableToJson(ht_result);
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
            name = JsonHelper.StringFormat1(name);
            address = JsonHelper.StringFormat1(address);
            egname = JsonHelper.StringFormat1(egname);
            egaddress = JsonHelper.StringFormat1(egaddress);
            rsname = JsonHelper.StringFormat1(rsname);
            rsaddress = JsonHelper.StringFormat1(rsaddress);

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

            ht["informationName"] = informationName;
            ht["informationUrl"] = informationUrl;
            ht["businesslicenseName"] = businesslicenseName;
            ht["businesslicenseUrl"] = businesslicenseUrl;

            //服务信息
            ht["icnname"] = context.Request["ICNNAME"] == null ? "" : context.Request["ICNNAME"].ToString();
            ht["icncreditcode"] = context.Request["ICNCREDITCODE"] == null ? "" : context.Request["ICNCREDITCODE"].ToString();
            ht["icnbank"] = context.Request["ICNBANK"] == null ? "" : context.Request["ICNBANK"].ToString();
            ht["icnaccount"] = context.Request["ICNACCOUNT"] == null ? "" : context.Request["ICNACCOUNT"].ToString();
            ht["icnaddress"] = context.Request["ICNADDRESS"] == null ? "" : context.Request["ICNADDRESS"].ToString();
            ht["icnphone"] = context.Request["ICNPHONE"] == null ? "" : context.Request["ICNPHONE"].ToString();
            ht["iegname"] = context.Request["IEGNAME"] == null ? "" : context.Request["IEGNAME"].ToString();
            ht["iegcreditcode"] = context.Request["IEGCREDITCODE"] == null ? "" : context.Request["IEGCREDITCODE"].ToString();
            ht["iegbank"] = context.Request["IEGBANK"] == null ? "" : context.Request["IEGBANK"].ToString();
            ht["iegaccount"] = context.Request["IEGACCOUNT"] == null ? "" : context.Request["IEGACCOUNT"].ToString();
            ht["iegaddress"] = context.Request["IEGADDRESS"] == null ? "" : context.Request["IEGADDRESS"].ToString();
            ht["iegphone"] = context.Request["IEGPHONE"] == null ? "" : context.Request["IEGPHONE"].ToString();
            WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll();
            //说明是添加一条通讯信息
            string sqlContact = string.Empty;
            //先删除名下的所有通讯信息，然后添加
            string deleteContract = "delete from bcustomer_contact where code=@code";
            SqlParameter[] pmsDel = new SqlParameter[]{
                new SqlParameter("@code",ht["code"])
            };
            bll.ExecuteNonQuery(deleteContract, pmsDel);
            
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
            List<Hashtable> rows = new List<Hashtable>();
            #region 通讯信息
            var msgInfo = context.Request["datagrid0"];
            if (msgInfo.Length > 0)
            {
                objs.Add(new SqlParam[0]);
                sqls.Add(new StringBuilder("delete bcustomer_contact where code='" + ht["code"] + "'"));//删除发货人
                rows = JsonHelper.DeserializeJsonToList<Hashtable>(msgInfo);
                for (int i = 0; i < rows.Count; i++)
                {
                    sqls.Add(new StringBuilder(@"insert into bcustomer_contact ( code,name,address,phone,email) values(@code,@name,@address,@phone,@email)"));
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

            #region 收货信息
            var msgDelivery = context.Request["datagrid1"];
            if (msgDelivery.Length > 0) {
                sqls.Add(new StringBuilder("delete bcustomer_delivery where code='" + ht["code"] + "'"));//删除发货人
                objs.Add(new SqlParam[0]);
                rows = JsonHelper.DeserializeJsonToList<Hashtable>(msgDelivery);
                for (int i = 0; i < rows.Count; i++)
                {
                    sqls.Add(new StringBuilder(@"insert into bcustomer_delivery ( phone,code, name, foreignName, address,foreignAddress,email) values(@phone,@code,@name,@foreignName,@address,@foreignAddress,@email)"));
                    objs.Add(new SqlParam[]{
                        new SqlParam("@phone",rows[i]["PHONE"]??""),
                        new SqlParam("@code",ht["code"]),
                        new SqlParam("@name",rows[i]["NAME"]??""),
                        new SqlParam("@foreignName",rows[i]["FOREIGNNAME"]??""),
                        new SqlParam("@address",rows[i]["ADDRESS"]??""),
                        new SqlParam("@foreignAddress",rows[i]["FOREIGNADDRESS"]??""),
                        new SqlParam("@email",rows[i]["EMAIL"]??""),
                    });
                }
            }
            #endregion

            #region 通知人信息
            var msgNotice = context.Request["datagrid2"];

            if (msgNotice.Length > 0)
            {
                sqls.Add(new StringBuilder("delete bcustomer_notice where code='" + ht["code"] + "'"));//删除发货人
                objs.Add(new SqlParam[0]);
                rows = JsonHelper.DeserializeJsonToList<Hashtable>(msgNotice);
                for (int i = 0; i < rows.Count; i++)
                {
                    sqls.Add(new StringBuilder(@"insert into bcustomer_notice ( phone,code, name, foreignName, address,foreignAddress,email) values(@phone,@code,@name,@foreignName,@address,@foreignAddress,@email)"));
                    objs.Add(new SqlParam[]{
                        new SqlParam("@phone",rows[i]["PHONE"]??""),
                        new SqlParam("@code",ht["code"]),
                        new SqlParam("@name",rows[i]["NAME"]??""),
                        new SqlParam("@foreignName",rows[i]["FOREIGNNAME"]??""),
                        new SqlParam("@address",rows[i]["ADDRESS"]??""),
                        new SqlParam("@foreignAddress",rows[i]["FOREIGNADDRESS"]??""),
                        new SqlParam("@email",rows[i]["EMAIL"]??""),
                    });
                }
            }
            #endregion
            //对主表操作

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_CUSTOMER, "code", ht["code"].ToString(), ht);
            if (IsOk)
            {
                bool flag = true;
                if (flag)
                {
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
                        ht_result.Add("msg", "操作失败！");
                    }
                }
                else
                {
                    //删除主表
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        ///加载文件
        /// </summary>
        /// <param name="context"></param>
        private string load(HttpContext context)
        {
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
                    string savePath = context.Server.MapPath(ConstantUtil.FILE_CUSTOMER_URL);//Server.MapPath 获得虚拟服务器相对路径
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
                        ht_result.Add("status", "T");
                        ht_result.Add("msg", "上传成功!");
                        return JsonHelper.HashtableToJson(ht_result);
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

        /// <summary>
        /// 获取业务处的人员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetJobMan(HttpContext context)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"select us.*,org.Agency
                                                                from Com_Organization org
                                                                join Com_OrgAddUser orUs on orUs.OrgId=org.Id
                                                                right join Com_UserInfos us on us.Userid=orUs.UserId
                                                                where org.Agency ='" + ConstantUtil.ORG_JOBMAN + "'"));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder ret = new StringBuilder("[");

                foreach (DataRow dr in dt.Rows)
                {
                    ret.Append(JsonHelper.DataRowToJson_(dr) + ",");
                }
                ret = ret.Remove(ret.Length - 1, 1);
                ret.Append("]");
                return ret.ToString();
            }
            return "";
        }

        /// <summary>
        /// 获取业务员角色的人员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetJobManRole(HttpContext context) {
            StringBuilder sql = new StringBuilder(string.Format(@"select ui.* ,org.Agency
                                                                from Com_UserInfos ui 
                                                                join Tb_RolesAddUser ru on ru.UserId=ui.Userid
                                                                join Tb_Roles ro on ro.Id=ru.RolesId
                                                                join Com_OrgAddUser oru on oru.UserId=ui.Userid
                                                                join Com_Organization org on org.Id=oru.OrgId
                                                                where ro.RolesName='" + ConstantUtil.ROLE_JOBMAN + "'"));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder ret = new StringBuilder("[");

                foreach (DataRow dr in dt.Rows)
                {
                    ret.Append(JsonHelper.DataRowToJson_(dr) + ",");
                }
                ret = ret.Remove(ret.Length - 1, 1);
                ret.Append("]");
                return ret.ToString();
            }
            return "";
        }
        /// <summary>
        /// 获取业务组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetJobManAngency(HttpContext context)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"select distinct org.Agency
                                                                from Com_UserInfos ui 
                                                                join Tb_RolesAddUser ru on ru.UserId=ui.Userid
                                                                join Tb_Roles ro on ro.Id=ru.RolesId
                                                                join Com_OrgAddUser oru on oru.UserId=ui.Userid
                                                                join Com_Organization org on org.Id=oru.OrgId
                                                                where ro.RolesName='" + ConstantUtil.ROLE_JOBMAN + "'"));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder ret = new StringBuilder("[");

                foreach (DataRow dr in dt.Rows)
                {
                    ret.Append(JsonHelper.DataRowToJson_(dr) + ",");
                }
                ret = ret.Remove(ret.Length - 1, 1);
                ret.Append("]");
                return ret.ToString();
            }
            return "";
        }
    }
}
