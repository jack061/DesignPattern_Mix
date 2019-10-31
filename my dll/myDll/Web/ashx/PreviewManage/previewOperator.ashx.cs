using PDA_Service.DataBase.DataBase.SqlServer;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Web.ashx.PreviewManage
{
    /// <summary>
    /// previewOperator 的摘要说明
    /// </summary>
    public class previewOperator : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request["module"] == null ? "" : context.Request["module"].ToString();
            switch (module)
            {
                case "addPreview"://添加预验
                    context.Response.Write(addPreview(context));
                    break;
                case "editPreview"://修改预验
                    context.Response.Write(editPreview(context));
                    break;
                case "delPreview"://删除预验信息
                    context.Response.Write(delPreview(context));
                    break;

                default://默认
                    context.Response.Write("");
                    break;
            }
        }

        #region 删除预验
        private string delPreview(HttpContext context)
        {
            string previewCode=context.Request.Params["previewCode"];
            string sql = "delete from previewManage where previewCode=@previewCode";
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@previewCode",previewCode)
            };
            string packSql = "delete from previewPackManage where previewMaCode=@previewCode";
            string columisionSql = "delete from previewConclusionManage where previewCode=@previewCode ";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(sql, pms);
                    if (!string.IsNullOrEmpty(context.Request.Params["packCode"]))
                    {
                        bll.ExecuteNonQuery(packSql, pms);
                        bll.ExecuteNonQuery(columisionSql, pms);
                    }
                   
                    bll.SqlTran.Commit();
                    return "ok";

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
        }
        #endregion

        #region 编辑预验信息
        private string editPreview(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            //预验添加
            //是否为空校验
            var preCode = context.Request.Params["previewCode"];
            var packCode = context.Request.Params["packCode"];

            if (string.IsNullOrEmpty(preCode))
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "预验编号不能为空！");
                return JsonHelper.HashtableToJson(ht_result);
            }
            if (string.IsNullOrEmpty(packCode))
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "性能检验结果单号不能为空！");
                return JsonHelper.HashtableToJson(ht_result);
            }

            #region 原来实现方式
            /*
            string strsql = @" insert into previewManage(previewCode,deliveryMan,productName,HSCode,weight,unit,reportAmount,
                            currency,unitProduction,batchNumber,validity,productionDate,transport,ton)
                            values(@previewCode,@deliveryMan,@productName,@HSCode,@weight,@unit,@reportAmount,@currency,
                            @unitProduction,@batchNumber,@validity,@productionDate,@transport,@ton);";


            SqlParameter[] mms = new SqlParameter[]
                {
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@deliveryMan",Value=context.Request.Params["deliveryMan"],Size=1000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@productName",Value=context.Request.Params["productName"],Size=8},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@HSCode",Value=context.Request.Params["HSCode"],Size=200},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@weight",Value=context.Request.Params["weight"],Size=1000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@unit",Value=context.Request.Params["unit"],Size=1000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@reportAmount",Value=context.Request.Params["reportAmount"],Size=2000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@unitProduction",Value=context.Request.Params["unitProduction"],Size=30},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@batchNumber",Value=context.Request.Params["batchNumber"],Size=30},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=30},
                    new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@productionDate",Value=context.Request.Params["productionDate"],Size=30},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ton",Value=context.Request.Params["ton"]?? "0",Size=60}
                };

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql, mms);
                    if (!string.IsNullOrEmpty(context.Request.Params["packCode"]))
                    {
                        //包装添加

                        string packSql = @" insert into previewPackManage(previewMaCode,packCode,validite,status)
                                                values(@previewMaCode,@packCode,@validite,@status);";
                        string[] pack = context.Request.Form.GetValues("packCode");
                        //循环获得要添加的性能检验单
                        for (int i = 0; i < pack.Length; i++)
                        {


                            SqlParameter[] pack1Pms = new SqlParameter[]
                                {
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewMaCode",Value=context.Request.Params["previewCode"],Size=1000},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packCode",Value=pack[i],Size=1000},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validite",Value=context.Request.Params["PackValidity1"],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value="1",Size=200},

                                };
                            bll.ExecuteNonQuery(packSql, pack1Pms);
                            //包装鉴定结果单号添加
                            string ConclusionSql = @"insert into previewConclusionManage(previewCode,packCode,conclusionCode1,conclusionCode2,conclusionCode3,conclusionCode4,conclusionCode5)
                                            values(@previewCode,@packCode,@conclusionCode1,@conclusionCode2,@conclusionCode3,@conclusionCode4,@conclusionCode5);";

                            string[] conclusionCode1 = context.Request.Params.GetValues("conclusionCode1");
                            string[] conclusionCode2 = context.Request.Params.GetValues("conclusionCode2");
                            string[] conclusionCode3 = context.Request.Params.GetValues("conclusionCode3");
                            string[] conclusionCode4 = context.Request.Params.GetValues("conclusionCode4");
                            string[] conclusionCode5 = context.Request.Params.GetValues("conclusionCode5");

                            SqlParameter[] Conclusion1Pms = new SqlParameter[]
                                {
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packCode",Value=pack[i],Size=1000},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode1",Value=conclusionCode1[i],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode2",Value=conclusionCode2[i],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode3",Value=conclusionCode3[i],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode4",Value=conclusionCode4[i],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode5",Value=conclusionCode5[i],Size=8},

                                };
                            bll.ExecuteNonQuery(ConclusionSql, Conclusion1Pms);


                        }

                    }

                    bll.SqlTran.Commit();
                    return "ok";

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
            */
            #endregion

            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();

            //1、组织主表数据
            string previewCode = (context.Request["previewCode"] ?? "").ToString().Trim();
            string deliveryMan = (context.Request["deliveryMan"] ?? "").ToString().Trim();
            string productName = (context.Request["productName"] ?? "").ToString().Trim();
            string HSCode = (context.Request["HSCode"] ?? "").ToString().Trim();
            string weight = (context.Request["weight"] ?? "").ToString().Trim();
            string unit = (context.Request["unit"] ?? "").ToString().Trim();
            string reportAmount = (context.Request["reportAmount"] ?? "").ToString().Trim();
            string currency = (context.Request["currency"] ?? "").ToString().Trim();
            string unitProduction = (context.Request["unitProduction"] ?? "").ToString().Trim();
            string unitName = (context.Request["unitName"] ?? "").ToString().Trim();
            string batchNumber = (context.Request["batchNumber"] ?? "").ToString().Trim();
            string validity = (context.Request["validity"] ?? "").ToString().Trim();
            string productionDate = (context.Request["productionDate"] ?? "").ToString().Trim();
            string transport = (context.Request["transport"] ?? "").ToString().Trim();
            string ton = (context.Request["ton"] ?? "0").ToString().Trim();

            Hashtable ht_main = new Hashtable();
            ht_main.Add("previewCode", previewCode);
            ht_main.Add("deliveryMan", deliveryMan);
            ht_main.Add("productName", productName);
            ht_main.Add("HSCode", HSCode);
            ht_main.Add("weight", weight);
            ht_main.Add("unit", unit);
            ht_main.Add("reportAmount", reportAmount);
            ht_main.Add("currency", currency);
            ht_main.Add("unitProduction", unitProduction);
            ht_main.Add("unitName", unitName);
            ht_main.Add("batchNumber", batchNumber);
            ht_main.Add("validity", validity);
            ht_main.Add("productionDate", productionDate);
            ht_main.Add("transport", transport);
            ht_main.Add("ton", ton);
            ht_main.Add("createman", RequestSession.GetSessionUser().UserAccount);
          
            //2、生成主表sql
            SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_PreviewManage, "previewCode", previewCode, ref sqls, ref objs);

            //3、组织子表数据1
            List<Hashtable> list = new List<Hashtable>();
            string[] pack = context.Request.Form.GetValues("packCode");
            string[] PackValidity1 = context.Request.Form.GetValues("PackValidity1");
            for (int i = 0; i < pack.Length; i++)
            {
                Hashtable ht_temp = new Hashtable();
                ht_temp.Add("previewMaCode", previewCode);
                ht_temp.Add("packCode", pack[i]);
                ht_temp.Add("validite", PackValidity1[i]);
                ht_temp.Add("status", "1");
                list.Add(ht_temp);
            }
            //4、生成子表sql
            sqls.Add(new StringBuilder("delete " + ConstantUtil.TABLE_PREVIEWPACKMANAGE + " where previewMaCode=@previewMaCode"));
            objs.Add(new SqlParam[] { new SqlParam("@previewMaCode", previewCode) });
            SqlUtil.getBatchSqls(list, ConstantUtil.TABLE_PREVIEWPACKMANAGE, ref sqls, ref objs);

            //5、组织子表数据2
            List<Hashtable> list1 = new List<Hashtable>();

            for (int i = 0; i < pack.Length; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    string[] conclusionCode = context.Request.Params.GetValues("conclusionCode" + j);
                    //for (int m = 0; m < conclusionCode.Length; m++)
                    //{
                    //    Hashtable ht_temp = new Hashtable();
                    //    ht_temp.Add("previewCode", previewCode);
                    //    ht_temp.Add("packCode", pack[i]);
                    //    ht_temp.Add("conclusionCode", conclusionCode[m]);
                    //    list1.Add(ht_temp);
                    //}
                    Hashtable ht_temp = new Hashtable();
                    ht_temp.Add("previewCode", previewCode);
                    ht_temp.Add("packCode", pack[i]);
                    //ht_temp.Add("conclusionCode", (conclusionCode[i] == "&nbsp;" || conclusionCode[i] == " ") ? "" : conclusionCode[i]);
                    ht_temp.Add("conclusionCode", (conclusionCode[i] == "&nbsp;") ? "" : conclusionCode[i].Trim());
                    list1.Add(ht_temp);
                }
            }
            //6、生成子表sql
            sqls.Add(new StringBuilder("delete " + ConstantUtil.TABLE_PREVIEWCONCLUSIONMANAGE + " where previewCode=@previewCode"));
            objs.Add(new SqlParam[] { new SqlParam("@previewCode", previewCode) });
            SqlUtil.getBatchSqls(list1, ConstantUtil.TABLE_PREVIEWCONCLUSIONMANAGE, ref sqls, ref objs);

            //7、执行sql
            int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

            if (result >= 0)
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
            #region 原先实现
            /*
            //预验添加
            string strsql = @"update previewManage set deliveryMan=@deliveryMan,productName=@productName,HSCode=@HSCode,weight=@weight,
unit=@unit,reportAmount=@reportAmount,currency=@currency,unitProduction=@unitProduction,batchNumber=@batchNumber,validity=@validity,
productionDate=@productionDate,transport=@transport where previewCode=@previewCode;";
            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@deliveryMan",Value=context.Request.Params["deliveryMan"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@productName",Value=context.Request.Params["productName"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@HSCode",Value=context.Request.Params["HSCode"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@weight",Value=context.Request.Params["weight"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@unit",Value=context.Request.Params["unit"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@reportAmount",Value=context.Request.Params["reportAmount"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@unitProduction",Value=context.Request.Params["unitProduction"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@batchNumber",Value=context.Request.Params["batchNumber"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=30},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@productionDate",Value=context.Request.Params["productionDate"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
};
            //previewMaCode, packCode, validite, status
            //包装添加
            string packSql = @" update previewPackManage set validite=@validite,status=@status where previewMaCode=@previewCode and packCode=@packCode";
            SqlParameter[] pack1Pms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packCode",Value=context.Request.Params["packCode1"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validite",Value=context.Request.Params["PackValidity1"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value="1",Size=200},

};

            SqlParameter[] pack2Pms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packCode",Value=context.Request.Params["packCode2"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validite",Value=context.Request.Params["PackValidity2"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value="1",Size=200},

};
            //previewCode, packCode, conclusionCode1, conclusionCode2, conclusionCode3, conclusionCode4, conclusionCode5
            //包装鉴定结果单号添加
            string ConclusionSql = @"update previewConclusionManage set conclusionCode1=@conclusionCode1,conclusionCode2=@conclusionCode2,
conclusionCode3=@conclusionCode3,conclusionCode4=@conclusionCode4,conclusionCode5=@conclusionCode1 where previewCode=@previewCode and 
packCode=@packCode;";
            SqlParameter[] Conclusion1Pms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packCode",Value=context.Request.Params["packCode1"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode1",Value=context.Request.Params["conclusionCode1"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode2",Value=context.Request.Params["conclusionCode2"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode3",Value=context.Request.Params["conclusionCode3"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode4",Value=context.Request.Params["conclusionCode4"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode5",Value=context.Request.Params["conclusionCode5"],Size=8},

};

            SqlParameter[] Conclusion2Pms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packCode",Value=context.Request.Params["packCode2"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode1",Value=context.Request.Params["conclusionCode6"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode2",Value=context.Request.Params["conclusionCode7"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode3",Value=context.Request.Params["conclusionCode8"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode4",Value=context.Request.Params["conclusionCode9"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode5",Value=context.Request.Params["conclusionCode10"],Size=8},

};



            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql, mms);
                    if (!string.IsNullOrEmpty(context.Request.Params["packCode1"]))
                    {
                        bll.ExecuteNonQuery(packSql, pack1Pms);
                        bll.ExecuteNonQuery(ConclusionSql, Conclusion1Pms);
                    }
                    if (!string.IsNullOrEmpty(context.Request.Params["packCode2"]))
                    {
                        bll.ExecuteNonQuery(packSql, pack2Pms);
                        bll.ExecuteNonQuery(ConclusionSql, Conclusion2Pms);
                    }
                    bll.SqlTran.Commit();
                    return "ok";

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }*/

            #endregion

        }

        #endregion

        #region 添加预验信息
        private string addPreview(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            //预验添加
            //是否为空校验
            var preCode = context.Request.Params["previewCode"];
            var packCode = context.Request.Params["packCode"];

            if (string.IsNullOrEmpty(preCode))
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "预验编号不能为空！");
                return JsonHelper.HashtableToJson(ht_result);
            }
            if (string.IsNullOrEmpty(packCode))
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "性能检验结果单号不能为空！");
                return JsonHelper.HashtableToJson(ht_result);
            }

            #region 原来实现方式
            /*
            string strsql = @" insert into previewManage(previewCode,deliveryMan,productName,HSCode,weight,unit,reportAmount,
                            currency,unitProduction,batchNumber,validity,productionDate,transport,ton)
                            values(@previewCode,@deliveryMan,@productName,@HSCode,@weight,@unit,@reportAmount,@currency,
                            @unitProduction,@batchNumber,@validity,@productionDate,@transport,@ton);";


            SqlParameter[] mms = new SqlParameter[]
                {
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@deliveryMan",Value=context.Request.Params["deliveryMan"],Size=1000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@productName",Value=context.Request.Params["productName"],Size=8},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@HSCode",Value=context.Request.Params["HSCode"],Size=200},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@weight",Value=context.Request.Params["weight"],Size=1000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@unit",Value=context.Request.Params["unit"],Size=1000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@reportAmount",Value=context.Request.Params["reportAmount"],Size=2000},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@unitProduction",Value=context.Request.Params["unitProduction"],Size=30},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@batchNumber",Value=context.Request.Params["batchNumber"],Size=30},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=30},
                    new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@productionDate",Value=context.Request.Params["productionDate"],Size=30},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
                    new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ton",Value=context.Request.Params["ton"]?? "0",Size=60}
                };

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql, mms);
                    if (!string.IsNullOrEmpty(context.Request.Params["packCode"]))
                    {
                        //包装添加

                        string packSql = @" insert into previewPackManage(previewMaCode,packCode,validite,status)
                                                values(@previewMaCode,@packCode,@validite,@status);";
                        string[] pack = context.Request.Form.GetValues("packCode");
                        //循环获得要添加的性能检验单
                        for (int i = 0; i < pack.Length; i++)
                        {


                            SqlParameter[] pack1Pms = new SqlParameter[]
                                {
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewMaCode",Value=context.Request.Params["previewCode"],Size=1000},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packCode",Value=pack[i],Size=1000},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validite",Value=context.Request.Params["PackValidity1"],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value="1",Size=200},

                                };
                            bll.ExecuteNonQuery(packSql, pack1Pms);
                            //包装鉴定结果单号添加
                            string ConclusionSql = @"insert into previewConclusionManage(previewCode,packCode,conclusionCode1,conclusionCode2,conclusionCode3,conclusionCode4,conclusionCode5)
                                            values(@previewCode,@packCode,@conclusionCode1,@conclusionCode2,@conclusionCode3,@conclusionCode4,@conclusionCode5);";

                            string[] conclusionCode1 = context.Request.Params.GetValues("conclusionCode1");
                            string[] conclusionCode2 = context.Request.Params.GetValues("conclusionCode2");
                            string[] conclusionCode3 = context.Request.Params.GetValues("conclusionCode3");
                            string[] conclusionCode4 = context.Request.Params.GetValues("conclusionCode4");
                            string[] conclusionCode5 = context.Request.Params.GetValues("conclusionCode5");

                            SqlParameter[] Conclusion1Pms = new SqlParameter[]
                                {
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@previewCode",Value=context.Request.Params["previewCode"],Size=1000},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packCode",Value=pack[i],Size=1000},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode1",Value=conclusionCode1[i],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode2",Value=conclusionCode2[i],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode3",Value=conclusionCode3[i],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode4",Value=conclusionCode4[i],Size=8},
                                new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@conclusionCode5",Value=conclusionCode5[i],Size=8},

                                };
                            bll.ExecuteNonQuery(ConclusionSql, Conclusion1Pms);


                        }

                    }

                    bll.SqlTran.Commit();
                    return "ok";

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
            */
            #endregion
            
            List<StringBuilder> sqls = new List<StringBuilder>();
            List<object> objs = new List<object>();
           
            //1、组织主表数据
            string previewCode = (context.Request["previewCode"] ?? "").ToString().Trim();
            string deliveryMan = (context.Request["deliveryMan"] ?? "").ToString().Trim();
            string productName = (context.Request["productName"] ?? "").ToString().Trim();
            string HSCode = (context.Request["HSCode"] ?? "").ToString().Trim();
            string weight = (context.Request["weight"] ?? "").ToString().Trim();
            string unit = (context.Request["unit"] ?? "").ToString().Trim();
            string reportAmount = (context.Request["reportAmount"] ?? "").ToString().Trim();
            string currency = (context.Request["currency"] ?? "").ToString().Trim();
            string unitProduction = (context.Request["unitProduction"] ?? "").ToString().Trim();
            string unitName = (context.Request["unitName"] ?? "").ToString().Trim();
            string batchNumber = (context.Request["batchNumber"] ?? "").ToString().Trim();
            string validity = (context.Request["validity"] ?? "").ToString().Trim();
            string productionDate = (context.Request["productionDate"] ?? "").ToString().Trim();
            string transport = (context.Request["transport"] ?? "").ToString().Trim();
            string ton = (context.Request["ton"] ?? "0").ToString().Trim();

            Hashtable ht_main = new Hashtable();
            ht_main.Add("previewCode", previewCode);
            ht_main.Add("deliveryMan", deliveryMan);
            ht_main.Add("productName", productName);
            ht_main.Add("HSCode", HSCode);
            ht_main.Add("weight", weight);
            ht_main.Add("unit", unit);
            ht_main.Add("reportAmount", reportAmount);
            ht_main.Add("currency", currency);
            ht_main.Add("unitProduction", unitProduction);
            ht_main.Add("unitName", unitName);
            ht_main.Add("batchNumber", batchNumber);
            ht_main.Add("validity", validity);
            ht_main.Add("productionDate", productionDate);
            ht_main.Add("transport", transport);
            ht_main.Add("ton", ton);
            ht_main.Add("createman", RequestSession.GetSessionUser().UserAccount);
            ht_main.Add("createDate", DateTime.Now.ToString());

            //2、生成主表sql
            SqlUtil.getBatchSqls(ht_main, ConstantUtil.TABLE_PreviewManage, "previewCode", previewCode, ref sqls, ref objs);

            //3、组织子表数据1
            List<Hashtable> list = new List<Hashtable>();
            string[] pack = context.Request.Form.GetValues("packCode");
            string[] PackValidity1 = context.Request.Form.GetValues("PackValidity1");
            for (int i = 0; i < pack.Length; i++)
            {
                Hashtable ht_temp = new Hashtable();
                ht_temp.Add("previewMaCode", previewCode);
                ht_temp.Add("packCode", pack[i]);
                ht_temp.Add("validite", PackValidity1[i]);
                ht_temp.Add("status", "1");
                list.Add(ht_temp);
            }
            //4、生成子表sql
            SqlUtil.getBatchSqls(list, ConstantUtil.TABLE_PREVIEWPACKMANAGE, ref sqls, ref objs);

            //5、组织子表数据2
            List<Hashtable> list1 = new List<Hashtable>();

            for (int i = 0; i < pack.Length; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    string[] conclusionCode = context.Request.Params.GetValues("conclusionCode" + j);
                    //for (int m = 0; m < conclusionCode.Length; m++)
                    //{
                    //    Hashtable ht_temp = new Hashtable();
                    //    ht_temp.Add("previewCode", previewCode);
                    //    ht_temp.Add("packCode", pack[i]);
                    //    ht_temp.Add("conclusionCode", conclusionCode[m]);
                    //    list1.Add(ht_temp);
                    //}
                    Hashtable ht_temp = new Hashtable();
                    ht_temp.Add("previewCode", previewCode);
                    ht_temp.Add("packCode", pack[i]);
                    ht_temp.Add("conclusionCode", conclusionCode[i]);
                    list1.Add(ht_temp);
                }
            }
            //6、生成子表sql
            SqlUtil.getBatchSqls(list1, ConstantUtil.TABLE_PREVIEWCONCLUSIONMANAGE, ref sqls, ref objs);

            //7、执行sql
            int result = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs);

            if (result >= 0)
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