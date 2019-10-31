using RM.Common.DotNetBean;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace RM.Web.ashx.PurchaseContract
{
    /// <summary>
    /// changePurchaseContract 的摘要说明
    /// </summary>
    public class changePurchaseContract : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string module = context.Request["module"];

            //添加采购合同
            if (module == "addcontract")
            {
                //保存主表内容
                string err = "";
                bool suc = addContract(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
                }
                context.Response.Write(r);
            }
            //修改采购合同
            else if (module == "editcontract")
            {
                //保存主表内容
                string err = "";
                bool suc = editContract(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
                }
                context.Response.Write(r);
            }
                //删除采购合同
            else if (module == "deletecontract")
            {
                //保存主表内容
                string err = "";
                bool suc = deleteContract(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
                }
                context.Response.Write(r);
            }
            //修改状态
            else if (module == "tjht")
            {
                //提交合同
                string err = "";
                bool suc = this.modifyContractStatus(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
                }
                context.Response.Write(r);
            }
            else if (module == "addcontract_a")
            {
                //保存主表内容
                string err = "";
                bool suc = addContract_a(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
                }
                context.Response.Write(r);
            }
            else if (module == "editcontract_a")
            {
                //保存主表内容
                string err = "";
                bool suc = editContract_a(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
                }
                context.Response.Write(r);
            }
            else if (module == "tjhtfj")
            {
                //提交合同附件
                string err = "";
                bool suc = modifyContractFStatus(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
                }
                context.Response.Write(r);
            }
                //删除合同附件
            else if (module == "deletecontractfj")
            {
                string err = "";
                bool suc = deleteContractfj(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
                }
                else
                {
                    r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
                }
                context.Response.Write(r);
            }
                //附件异步请求加载采购合同信息
            else if (module == "loadPurchaseData")
            {
                string err = "";
                string suc = loadPurchaseData(ref err, context);
                //返回json
               
                context.Response.Write(suc);
            }
            //根据销售合同编号加载基本信息
            else if (module == "attach")
            {
                string err = "";
                string suc = loadSaleDataToPurchase(ref err, context);
                //返回json

                context.Response.Write(suc);
            }
                


            #region old
            //else if (module == "copycontract")
            //{
            //    string err = "";
            //    bool suc = copyContract(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "deletecontract")
            //{
            //    string err = "";
            //    bool suc = deleteContract(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "deletecontractfj")
            //{
            //    string err = "";
            //    bool suc = deleteContractfj(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "editcontract")
            //{
            //    //保存主表内容
            //    string err = "";
            //    bool suc = editContract(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "addcontract_a")
            //{
            //    //保存主表内容
            //    string err = "";
            //    bool suc = addContract_a(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "editcontract_a")
            //{
            //    //保存主表内容
            //    string err = "";
            //    bool suc = editContract_a(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "addcontract_ap")
            //{
            //    //保存主表内容
            //    string err = "";
            //    bool suc = addContract_ap(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "editcontract_ap")
            //{
            //    //保存主表内容
            //    string err = "";
            //    bool suc = editContract_ap(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "tjht")
            //{
            //    //提交合同
            //    string err = "";
            //    bool suc = this.modifyContractStatus(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //}
            //else if (module == "tjhtfj")
            //{
            //    //提交合同附件
            //    string err = "";
            //    bool suc = modifyContractFStatus(ref err, context);
            //    //返回json
            //    string r = "";
            //    if (suc == true)
            //    {
            //        r = "{\"sucess\": 1,\"warnmsg\": \"\",\"errormsg\": \"\"}";
            //    }
            //    else
            //    {
            //        r = "{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"" + err + "\"}";
            //    }
            //    context.Response.Write(r);
            //} 
            #endregion

            else
            {
                context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
            }
        }

        #region 根据销售合同编号加载基本信息
        private string loadSaleDataToPurchase(ref string err, HttpContext context)
        {
              var contractNo = context.Request.Params["contractNo"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();

            sqldata.Append(" select * from Econtract where contractNo=@contractNo ");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
               
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        
        #endregion


        #region 异步加载采购合同信息
        private string loadPurchaseData(ref string err, HttpContext context)
        {
            
            var contractNo = context.Request.Params["contractNo"];
            RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
            StringBuilder sqldata = new StringBuilder();

            sqldata.Append(" select * from PurchaseContract where contractNo=@contractNo ");
            SqlParameter[] sqlpps = new SqlParameter[] 
                {
                    new SqlParameter("@contractNo",contractNo),
               
                };

            StringBuilder sb = ui.GetDatatableJsonString(sqldata, sqlpps);
            return sb.ToString();
        }
        
        #endregion
      

        #region 删除采购合同
        private bool deleteContract(ref string err, HttpContext context)
        {
           
            StringBuilder strsql = new StringBuilder(" delete PurchaseContract where contractNo=@contractNo;");
            strsql.Append("  delete PurchaseContract_a where contractNo=@contractNo; ");
            strsql.Append("  delete PurchaseContract_ap where contractNo=@contractNo; ");
            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
             
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql.ToString(), mms);
                    bll.SqlTran.Commit();
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;
                }
            }
            return true;
        } 
        #endregion

        #region 添加采购合同
        private bool addContract(ref string errorinfo, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
          
            contractNo = generalContractNo(context.Request.Params["buyername"], context.Request.Params["sellername"]);
            string status = string.Empty;
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            string strsql = @"insert into PurchaseContract(contractNo, saleCode, saleAttachCode, contracttype, contracttype2, businessclass, 
language, seller, signedtime, signedplace, buyer, buyeraddress, currency, pricement1, pricement1per, pricement2, pricement2per, pvalidity,
shipment, transport, tradement, harborout, harborarrive, harborclear, placement, validity, remark, templateno, status, createman,
createdate, lastmod, lastmoddate)
values(@contractNo,@saleCode,@saleAttachCode,@contracttype,@contracttype2,@businessclass,@language,@seller,@signedtime,@signedplace,@buyer,
@buyeraddress,@currency,@pricement1,
@pricement1per,@pricement2,@pricement2per,@pvalidity,@shipment,@transport,@tradement,@harborout,@harborarrive,@harborclear,@placement,
@validity,@remark,@templateno,@status,@createman,@createdate,@lastmod,@lastmoddate);";

            string lan = context.Request.Params["language"];

            #region mms参数
            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNo,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@saleCode",Value=context.Request.Params["saleCode"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@saleAttachCode",Value=context.Request.Params["saleAttachCode"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contracttype",Value=context.Request.Params["contracttype"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contracttype2",Value=context.Request.Params["contracttype2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessclass",Value=context.Request.Params["businessclass"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@language",Value=lan,Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@signedtime",Value=context.Request.Params["signedtime"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@signedplace",Value=context.Request.Params["signedplace"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyeraddress",Value=context.Request.Params["buyeraddress"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1",Value=context.Request.Params["pricement1"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1per",Value=context.Request.Params["pricement1per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2",Value=context.Request.Params["pricement2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2per",Value=context.Request.Params["pricement2per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pvalidity",Value=context.Request.Params["pvalidity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipment",Value=context.Request.Params["shipment"],Size=300},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradement",Value=context.Request.Params["tradement"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborout",Value=context.Request.Params["harborout"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborarrive",Value=context.Request.Params["harborarrive"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborclear",Value=context.Request.Params["harborclear"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@placement",Value=context.Request.Params["placement"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@remark",Value=context.Request.Params["remark"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@templateno",Value=context.Request.Params["templateno"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value=status,Size=10},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DBNull.Value,Size=8},
};
            #endregion
            //保存模板
            StringBuilder sbmb = new StringBuilder("");
            sbmb.Append(@"delete PurchaseContract_t where contractNo=@contractNo;
insert into PurchaseContract_t(templateno,templatename,language,sortno,content,contractNo) 
select templateno,templatename,language,sortno,content,@contractNo from btemplate_contract where templateno=@templateno  and status=1 ; ");

            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(@" insert into PurchaseContract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");

            bool issuc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    //数据验证
                    if (context.Request.Params["buyeraddress"].Length == 0)
                    {
                        throw new Exception("买家地址不能为空!");
                    }
                    bll.ExecuteNonQuery(strsql, mms);

                    if (context.Request.Params["templateno"].Trim().Length > 0)
                    {
                        SqlParameter[] mms1 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            new SqlParameter{ParameterName="@templateno",Value=context.Request.Params["templateno"]  }
                    };
                        bll.ExecuteNonQuery(sbmb.ToString(), mms1);
                    }

                    foreach (Hashtable hs in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            //默认为空字符串（属于合同的默认产品）
                            new SqlParameter{ParameterName="@attachmentno",Value=""  },
                            new SqlParameter{ParameterName="@pcode",Value=hs["pcode"]  },
                            new SqlParameter{ParameterName="@pname",Value=hs["pname"]  },
                            new SqlParameter{ParameterName="@quantity",Value=hs["quantity"]  },
                            new SqlParameter{ParameterName="@qunit",Value=hs["qunit"]  },
                            new SqlParameter{ParameterName="@price",Value=hs["price"]  },
                            new SqlParameter{ParameterName="@amount",Value=hs["amount"]  },
                            new SqlParameter{ParameterName="@packspec",Value=hs["packspec"]  },
                            new SqlParameter{ParameterName="@packing",Value=hs["packing"]  },
                            new SqlParameter{ParameterName="@pallet",Value=hs["pallet"]  },
                            new SqlParameter{ParameterName="@ifcheck",Value=hs["ifcheck"]  },
                            new SqlParameter{ParameterName="@ifplace",Value=hs["ifplace"]  }
                        };
                        bll.ExecuteNonQuery(sb2.ToString(), mms2);
                    }
                    bll.SqlTran.Commit();
                    issuc = true;
                }
                catch (Exception ex)
                {
                    errorinfo = ex.Message;
                    issuc = false;
                }
            }
            return issuc;
        }
        #endregion


        #region 修改采购合同
        private bool editContract(ref string errorinfo, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string strsql = @"update PurchaseContract set contracttype=@contracttype,contracttype2=@contracttype2,businessclass=@businessclass,language=@language,seller=@seller,signedtime=@signedtime,signedplace=@signedplace,buyer=@buyer,buyeraddress=@buyeraddress,currency=@currency,status=@status,pricement1=@pricement1,pricement1per=@pricement1per,pricement2=@pricement2,pricement2per=@pricement2per,pvalidity=@pvalidity,shipment=@shipment,transport=@transport,tradement=@tradement,harborout=@harborout,harborarrive=@harborarrive,harborclear=@harborclear,placement=@placement,validity=@validity,remark=@remark,templateno=@templateno,lastmod=@lastmod,lastmoddate=@lastmoddate where  contractNo=@contractNo ";
            string status = string.Empty;
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=false,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contracttype",Value=context.Request.Params["contracttype"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contracttype2",Value=context.Request.Params["contracttype2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessclass",Value=context.Request.Params["businessclass"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@language",Value=context.Request.Params["language"].TrimStart(','),Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@signedtime",Value=context.Request.Params["signedtime"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@signedplace",Value=context.Request.Params["signedplace"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyeraddress",Value=context.Request.Params["buyeraddress"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1",Value=context.Request.Params["pricement1"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1per",Value=context.Request.Params["pricement1per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2",Value=context.Request.Params["pricement2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2per",Value=context.Request.Params["pricement2per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pvalidity",Value=context.Request.Params["pvalidity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipment",Value=context.Request.Params["shipment"],Size=300},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradement",Value=context.Request.Params["tradement"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborout",Value=context.Request.Params["harborout"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborarrive",Value=context.Request.Params["harborarrive"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborclear",Value=context.Request.Params["harborclear"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@placement",Value=context.Request.Params["placement"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@remark",Value=context.Request.Params["remark"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@templateno",Value=context.Request.Params["templateno"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value=status,Size=10},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},

};
            //保存模板
            StringBuilder sbmb = new StringBuilder("");
            sbmb.Append(@"delete PurchaseContract_t where contractNo=@contractNo;
insert into PurchaseContract_t(templateno,templatename,language,sortno,content,contractNo) 
select templateno,templatename,language,sortno,content,@contractNo from btemplate_contract where templateno=@templateno   and status=1 ; ");

            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();
            sb2.Append(@" insert into PurchaseContract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");
            sb3.Append(@" update PurchaseContract_ap set  pname=@pname,quantity=@quantity,qunit=@qunit,price=@price,amount=@amount,packspec=@packspec,
packing=@packing,pallet=@pallet,ifcheck=@ifcheck,ifplace=@ifplace where contractNo=@contractNo and attachmentno=@attachmentno and pcode=@pcode;
");

            //update 和 insert 产品表
            string oldpcodes = "";
            string olddel = "";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                SqlParameter[] ss2 = new SqlParameter[] { 
                    new SqlParameter{ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30}
                };
                DataTable dt = bll.ExecDatasetSql(" select pcode from PurchaseContract_ap where contractNo=@contractNo and attachmentno='' ", ss2).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    oldpcodes = oldpcodes + "#" + dr["pcode"].ToString();
                    string oldpcode = dr["pcode"].ToString();
                    if (listtable.Exists(a => a["pcode"].ToString().Equals(oldpcode)) == false)
                    {
                        olddel += "'" + oldpcode + "',";
                    }
                }
                olddel = olddel.TrimEnd(',');
            }
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    bll.ExecuteNonQuery(strsql, mms);

                    if (context.Request.Params["templateno"].Trim().Length > 0)
                    {
                        //保存模板
                        SqlParameter[] mms1 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            new SqlParameter{ParameterName="@templateno",Value=context.Request.Params["templateno"]  }
                        };
                        bll.ExecuteNonQuery(sbmb.ToString(), mms1);
                    }

                    foreach (var a in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNo,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value="",Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pcode",Value=a["pcode"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pname",Value=a["pname"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@quantity",Value=a["quantity"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@qunit",Value=a["qunit"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@price",Value=a["price"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@amount",Value=a["amount"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packspec",Value=a["packspec"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packing",Value=a["packing"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pallet",Value=a["pallet"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifcheck",Value=a["ifcheck"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifplace",Value=a["ifplace"],Size=20},

};
                        if (oldpcodes.Contains(a["pcode"].ToString()))
                        {
                            bll.ExecuteNonQuery(sb3.ToString(), mms2);
                        }
                        else
                        {
                            bll.ExecuteNonQuery(sb2.ToString(), mms2);
                        }
                    }
                    //删除某些项目
                    if (olddel.Length > 0)
                    {
                        SqlParameter[] mms3 = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNo,Size=30}
};
                        bll.ExecuteNonQuery(" delete PurchaseContract_ap where contractNo=@contractNo and attachmentno='' and pcode in (" + olddel + ")", mms3);
                    }
                    bll.SqlTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;
                    return false;
                }
            }
        }
        #endregion


        #region 生成合同编号
        private string generalContractNo(string buyer, string seller)
        {
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;

                var contractFirst = bll.ExecuteScalar(@"select top 1  contractNo from PurchaseContract  order by id desc");
                if (contractFirst == null)
                {
                    contractFirst = "001";
                }
                else
                {
                    string[] contractArray = contractFirst.ToString().Split('-');
                    contractFirst = contractArray[1];
                    contractOrg = Convert.ToInt32(contractFirst);
                }


                //查询数据表中是否存在买方卖方
                string buyCompany = buyer;
                string sellerCompany = seller;
                SqlParameter[] company = new SqlParameter[]
                    {
                        new SqlParameter("@buyer",buyCompany),
                        new SqlParameter("@seller",sellerCompany)
                    };

                DataSet dsCompany = bll.ExecDatasetSql(@"select * from EncodingRules where @buyer like '%'+companyName+'%';
                        select * from EncodingRules where @seller  like '%'+companyName+'%'", company);
                DataTable dtBuyCompany = dsCompany.Tables[0];
                DataTable dtSellerCompany = dsCompany.Tables[1];

                //如果卖方买方都存在，比较其优先级，生成合同编号
                string buyCode = string.Empty;
                string sellerCode = string.Empty;

                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string month = DateTime.Now.Month.ToString();
                string random = string.Empty;

                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                if (dtBuyCompany.Rows.Count > 0 && dtSellerCompany.Rows.Count > 0)
                {
                    //如果买方的优先级大于卖方，生成采购合同
                    if (Convert.ToInt32(dtBuyCompany.Rows[0][0]) > Convert.ToInt32(dtSellerCompany.Rows[0][0]))
                    {
                        buyCode = dtBuyCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = buyCode + "GY" + year + month + "-" + random;

                    }
                    //买方优先级小于卖方，生成销售合同
                    else
                    {
                        sellerCode = dtSellerCompany.Rows[0][2].ToString();
                        random = String.Format("{0:D3}", contractOrg + 1);
                        contractNumber = sellerCode + "XS" + year + month + "-" + random;
                    }


                }
                //如果存在卖方，不存在买方，生成销售合同
                else if (dtSellerCompany.Rows.Count > 0 && dtBuyCompany.Rows.Count <= 0)
                {
                    sellerCode = dtSellerCompany.Rows[0][2].ToString();

                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = sellerCode + "XS" + year + month + '-' + random;
                }
                //存在买方，不存在卖方生成采购合同
                else
                {
                    buyCode = dtBuyCompany.Rows[0][2].ToString();
                    random = String.Format("{0:D3}", contractOrg + 1);
                    contractNumber = buyCode + "GY" + year + month + '-' + random;
                }


            }

            return contractNumber;
        }
        #endregion

        #region 改变合同状态

        private bool modifyContractStatus(ref string err, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder("");
            strsql.Append("  update  PurchaseContract set status=@status where contractNo=@contractNo ; ");
            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] },
                new SqlParameter{ ParameterName="@status",Value=context.Request.Params["status"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql.ToString(), mms);
                    bll.SqlTran.Commit();
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 改变合同附件状态
        private bool modifyContractFStatus(ref string errorinfo, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder("");
            strsql.Append("  update  PurchaseContract_a set status=@status where contractNo=@contractNo and attachmentno=@attachmentno; ");
            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] },
                new SqlParameter{ ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"] },
                new SqlParameter{ ParameterName="@status",Value=context.Request.Params["status"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql.ToString(), mms);
                    bll.SqlTran.Commit();
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 添加采购合同附件
        private bool addContract_a(ref string errorinfo, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string attachmentno = context.Request.Params["attachmentno"];

            attachmentno = generalAttachNo(contractNo);
            string status = string.Empty;
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            string strsql = @" insert into PurchaseContract_a(contractNo, attachmentno, language, seller, signedtime, signedplace,
buyer, buyeraddress, currency, pricement1, pricement1per, pricement2, pricement2per, pvalidity, shipment, transport, tradement, 
harborout, harborarrive, harborclear, placement, validity, remark, templateno, status, createman, createdate, lastmod, lastmoddate)
values(@contractNo,@attachmentno,@language,@seller,@signedtime,@signedplace,@buyer,@buyeraddress,@currency,@pricement1,
@pricement1per,@pricement2,@pricement2per,@pvalidity,@shipment,@transport,@tradement,@harborout,@harborarrive,@harborclear,@placement,
@validity,@remark,@templateno,@status,@createman,@createdate,@lastmod,@lastmoddate);";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=attachmentno,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@language",Value=context.Request.Params["language"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@signedtime",Value=context.Request.Params["signedtime"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@signedplace",Value=context.Request.Params["signedplace"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyeraddress",Value=context.Request.Params["buyeraddress"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1",Value=context.Request.Params["pricement1"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1per",Value=context.Request.Params["pricement1per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2",Value=context.Request.Params["pricement2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2per",Value=context.Request.Params["pricement2per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pvalidity",Value=context.Request.Params["pvalidity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipment",Value=context.Request.Params["shipment"],Size=300},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradement",Value=context.Request.Params["tradement"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborout",Value=context.Request.Params["harborout"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborarrive",Value=context.Request.Params["harborarrive"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborclear",Value=context.Request.Params["harborclear"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@placement",Value=context.Request.Params["placement"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@remark",Value=context.Request.Params["remark"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@templateno",Value=context.Request.Params["templateno"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value=status,Size=10},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DBNull.Value,Size=8},
};
            //保存模板
            StringBuilder sbmb = new StringBuilder("");
            sbmb.Append(@"delete PurchaseContract_at where contractNo=@contractNo and attachmentno=@attachmentno;
insert into PurchaseContract_at(attachmentno,templateno,templatename,language,sortno,content,contractNo) 
select @attachmentno,templateno,templatename,language,sortno,content,@contractNo from btemplate_attach where templateno=@templateno and status=1 ; ");

            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(@" insert into PurchaseContract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");

            bool issuc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql, mms);

                    if (context.Request.Params["templateno"].Trim().Length > 0)
                    {
                        //保存模板
                        SqlParameter[] mms1 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            new SqlParameter{ParameterName="@templateno",Value=context.Request.Params["templateno"]  },
                            new SqlParameter{ParameterName="@attachmentno",Value=attachmentno  }
                        };
                        bll.ExecuteNonQuery(sbmb.ToString(), mms1);
                    }
                    //保存产品
                    foreach (Hashtable hs in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            new SqlParameter{ParameterName="@attachmentno",Value=attachmentno },
                            new SqlParameter{ParameterName="@pcode",Value=hs["pcode"]  },
                            new SqlParameter{ParameterName="@pname",Value=hs["pname"]  },
                            new SqlParameter{ParameterName="@quantity",Value=hs["quantity"]  },
                            new SqlParameter{ParameterName="@qunit",Value=hs["qunit"]  },
                            new SqlParameter{ParameterName="@price",Value=hs["price"]  },
                            new SqlParameter{ParameterName="@amount",Value=hs["amount"]  },
                            new SqlParameter{ParameterName="@packspec",Value=hs["packspec"]  },
                            new SqlParameter{ParameterName="@packing",Value=hs["packing"]  },
                            new SqlParameter{ParameterName="@pallet",Value=hs["pallet"]  },
                            new SqlParameter{ParameterName="@ifcheck",Value=hs["ifcheck"]  },
                            new SqlParameter{ParameterName="@ifplace",Value=hs["ifplace"]  }
                        };
                        bll.ExecuteNonQuery(sb2.ToString(), mms2);
                    }
                    bll.SqlTran.Commit();
                    issuc = true;
                }
                catch (Exception ex)
                {
                    errorinfo = ex.Message.Replace("\r\n", "");
                    issuc = false;
                }
            }
            return issuc;
        } 
        #endregion

        #region 编辑采购合同附件
        private bool editContract_a(ref string errorinfo, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string attachmentno = context.Request.Params["attachmentno"];
            string status = string.Empty;
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            string strsql = @"update PurchaseContract_a set language=@language,seller=@seller,signedtime=@signedtime,signedplace=@signedplace,
buyer=@buyer,buyeraddress=@buyeraddress,currency=@currency,pricement1=@pricement1,pricement1per=@pricement1per,pricement2=@pricement2,
pricement2per=@pricement2per,pvalidity=@pvalidity,shipment=@shipment,transport=@transport,tradement=@tradement,harborout=@harborout,
harborarrive=@harborarrive,harborclear=@harborclear,placement=@placement,validity=@validity,status=@status,remark=@remark,templateno=@templateno,
lastmod=@lastmod,lastmoddate=@lastmoddate where  contractNo=@contractNo and attachmentno=@attachmentno";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=false,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=false,ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@language",Value=context.Request.Params["language"].TrimStart(','),Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@signedtime",Value=context.Request.Params["signedtime"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@signedplace",Value=context.Request.Params["signedplace"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyeraddress",Value=context.Request.Params["buyeraddress"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@currency",Value=context.Request.Params["currency"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1",Value=context.Request.Params["pricement1"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement1per",Value=context.Request.Params["pricement1per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2",Value=context.Request.Params["pricement2"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pricement2per",Value=context.Request.Params["pricement2per"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pvalidity",Value=context.Request.Params["pvalidity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipment",Value=context.Request.Params["shipment"],Size=300},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@transport",Value=context.Request.Params["transport"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@tradement",Value=context.Request.Params["tradement"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborout",Value=context.Request.Params["harborout"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborarrive",Value=context.Request.Params["harborarrive"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@harborclear",Value=context.Request.Params["harborclear"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@placement",Value=context.Request.Params["placement"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@validity",Value=context.Request.Params["validity"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@remark",Value=context.Request.Params["remark"],Size=16},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@templateno",Value=context.Request.Params["templateno"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value=status,Size=10},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},

};
            //保存模板
            StringBuilder sbmb = new StringBuilder("");
            sbmb.Append(@"delete PurchaseContract_at where contractNo=@contractNo;
insert into PurchaseContract_at(attachmentno,templateno,templatename,language,sortno,content,contractNo) 
select @attachmentno,templateno,templatename,language,sortno,content,@contractNo from btemplate_attach where templateno=@templateno  and status=1  ; ");

            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();
            sb2.Append(@" insert into PurchaseContract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");
            sb3.Append(@" update PurchaseContract_ap set  pname=@pname,quantity=@quantity,qunit=@qunit,price=@price,amount=@amount,packspec=@packspec,
packing=@packing,pallet=@pallet,ifcheck=@ifcheck,ifplace=@ifplace where contractNo=@contractNo and attachmentno=@attachmentno and pcode=@pcode;
");

            //update 和 insert 产品表
            string oldpcodes = "";
            string olddel = "";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                SqlParameter[] ss2 = new SqlParameter[] { 
                    new SqlParameter{ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30}
                };
                DataTable dt = bll.ExecDatasetSql(" select pcode from PurchaseContract_ap where contractNo=@contractNo and attachmentno='' ", ss2).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    oldpcodes = oldpcodes + "#" + dr["pcode"].ToString();
                    string oldpcode = dr["pcode"].ToString();
                    if (listtable.Exists(a => a["pcode"].ToString().Equals(oldpcode)) == false)
                    {
                        olddel += "'" + oldpcode + "',";
                    }
                }
                olddel = olddel.TrimEnd(',');
            }
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    bll.ExecuteNonQuery(strsql, mms);

                    if (context.Request.Params["templateno"].Trim().Length > 0)
                    {
                        //保存模板
                        SqlParameter[] mms1 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNo  },
                            new SqlParameter{ParameterName="@templateno",Value=context.Request.Params["templateno"]  },
                            new SqlParameter{ParameterName="@attachmentno",Value=attachmentno  }
                        };
                        bll.ExecuteNonQuery(sbmb.ToString(), mms1);
                    }

                    foreach (var a in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[]
                        {
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=a["contractNo"],Size=30},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=a["attachmentno"],Size=30},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pcode",Value=a["pcode"],Size=60},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pname",Value=a["pname"],Size=200},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@quantity",Value=a["quantity"],Size=9,Precision=18   ,Scale=2    },
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@qunit",Value=a["qunit"],Size=20},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@price",Value=a["price"],Size=9,Precision=18   ,Scale=2    },
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@amount",Value=a["amount"],Size=9,Precision=18   ,Scale=2    },
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packspec",Value=a["packspec"],Size=200},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packing",Value=a["packing"],Size=200},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pallet",Value=a["pallet"],Size=200},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifcheck",Value=a["ifcheck"],Size=20},
                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifplace",Value=a["ifplace"],Size=20},
                        };
                        if (oldpcodes.Contains(a["pcode"].ToString()))
                        {
                            bll.ExecuteNonQuery(sb3.ToString(), mms2);
                        }
                        else
                        {
                            bll.ExecuteNonQuery(sb2.ToString(), mms2);
                        }
                    }
                    //删除某些项目
                    if (olddel.Length > 0)
                    {
                        SqlParameter[] mms3 = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNo,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=attachmentno,Size=30}
};
                        bll.ExecuteNonQuery(" delete Econtract_ap where contractNo=@contractNo and @attachmentno=@attachmentno and pcode in (" + olddel + ")", mms3);
                    }
                    bll.SqlTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;
                    return false;
                }
            }
        } 
        #endregion

        #region 生成合同附件编号
        private string generalAttachNo(string contractNo)
        {
            string attno = contractNo + "-001";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                DataSet ds = bll.ExecDatasetSql(" select max(attachmentno) from PurchaseContract_a where attachmentno like @contractNo+'-%' ",
                    new SqlParameter[] { new SqlParameter("@contractNo", contractNo) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string tmp = ds.Tables[0].Rows[0][0].ToString();
                    string[] tt = tmp.Split('-');
                    if (tt.Length > 1)
                    {
                        attno = contractNo + "-" + (Convert.ToInt32(tt[tt.Length - 1]) + 1).ToString().PadLeft(3, '0');
                    }
                }
            }
            return attno;
        }
        #endregion

        #region 删除采购合同附件
        private bool deleteContractfj(ref string errorinfo, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder("");
            strsql.Append("  delete PurchaseContract_a where contractNo=@contractNo and attachmentno=@attachmentno; ");
            strsql.Append("  delete PurchaseContract_ap where contractNo=@contractNo and attachmentno=@attachmentno; ");
            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] },
                new SqlParameter{ ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql.ToString(), mms);
                    bll.SqlTran.Commit();
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    errorinfo = ex.Message;
                    return false;
                }
            }
            return true;
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