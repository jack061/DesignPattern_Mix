using RM.Busines.Util;
using RM.Common.DotNetBean;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace RM.Web.ashx.EntryContract
{
    /// <summary>
    /// ChangeContract1 的摘要说明
    /// </summary>
    public class ChangeContract1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string module = context.Request["module"];
            string contractNo = context.Request["contractNo"];
            //module  add or edit 
            if (module == "change")
            {
                string isall = context.Request["isall"];


                string attachmentno = context.Request["attachmentno"] ?? "";


                SqlParameter[] sqlpps = new SqlParameter[]
            {
                new SqlParameter("@contractNo", contractNo),
                new SqlParameter("@attachmentno",attachmentno)
            };
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    //查询出境外合同的信息
                    DataTable dt = new DataTable();
                    //选择了附件编号
                    if (isall == "0")
                    {
                        dt = bll.ExecDatasetSql("select * from Econtract_a where contractNo=@contractNo and attachmentno=@attachmentno", sqlpps).Tables[0];
                    }
                    //未选择附件编号
                    else
                    {
                        dt = bll.ExecDatasetSql(" select * from Econtract where contractNo=@contractNo ", sqlpps).Tables[0];
                    }


                    if (dt.Rows.Count > 0)
                    {


                    }
                    else
                    {
                        dt.Rows.Add(dt.NewRow());
                        dt.AcceptChanges();


                    }

                    SqlParameter[] sqlBuyer = new SqlParameter[]
            {
                new SqlParameter("@code", dt.Rows[0][5]),
              
            };
                    //根据合同号查询出买方的具体信息
                    DataTable buyerSet = bll.ExecDatasetSql("select name,address from bsupplier where code=@code", sqlBuyer).Tables[0];
                    //根据合同号查询出附件信息
                    RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();

                    StringBuilder sb = ui.ToEasyUIComboxJson(dt);
                    StringBuilder buyer = ui.ToEasyUIComboxJson(buyerSet);

                    string sy = sb.ToString();
                    string buyerString = buyer.ToString();


                    buyerString = buyerString.Replace("[{", "").Replace("}]", "");

                    sy = sy.Replace("}]", "," + buyerString + "}]");


                    context.Response.Write(sy);
                }
            }
            else if (module == "copycontract")
            {
                //copy合同
                  string err = "";
                bool suc = copyContract(ref err, context);
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
            //查询合同附件编号
            else if (module == "attach")
            {
                SqlParameter[] sqlpps = new SqlParameter[]
            {
                new SqlParameter("@contractNo", contractNo),
              
            };
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    //根据合同号查询出附件信息
                    RM.Busines.JsonHelperEasyUi ui = new Busines.JsonHelperEasyUi();
                    DataTable attachment = bll.ExecDatasetSql("select attachmentno from Econtract_a where contractNo=@contractNo", sqlpps).Tables[0];
                    StringBuilder attach = ui.ToEasyUIComboxJson(attachment);
                    context.Response.Write(attach);
                }



            }
            else if (module == "addcontract")
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
            else if (module == "deletecontract")
            {
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
            else if (module == "addcontract_ap")
            {
                //保存主表内容
                string err = "";
                bool suc = addContract_ap(ref err, context);
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
            else if (module == "editcontract_ap")
            {
                //保存主表内容
                string err = "";
                bool suc = editContract_ap(ref err, context);
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
            else if (module == "deleteProduct")
            {
                //保存主表内容
                string err = "";
                bool suc = deleteProduct(ref err, context);
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
            else
            {
                context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private bool addcontractAndProduct(ref string errorinfo, HttpContext context)
        {
            return true;
        }

        private bool copyContract(ref string errorinfo, HttpContext context)
        {
            //数据验证
            string status = string.Empty;
            if (context.Request.Params["buyeraddress"].Length == 0)
            {
                errorinfo = "买家地址不能为空!";
                return false;
            }
            if (context.Request.Params["templateno"].Length == 0)
            {
                errorinfo = "模板编号不能为空!";
                return false;
            }
            if (context.Request.Params["seller"].Length == 0)
            {
                errorinfo = "卖家不能为空";
                return false;
            }
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            #region 生成合同编号
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = bll.ExecuteScalar(@"select top 1  contractNo from Icontract  order by contractNo desc");
                if (contractFirst != null)
                {
                    string[] contractArray = contractFirst.ToString().Split('-');
                    contractFirst = contractArray[1];
                    contractOrg = Convert.ToInt32(contractFirst);
                }
                //查询数据表中是否存在买方卖方
                string buyCompany = context.Request.Params["buyer"];
                string sellerCompany = context.Request.Params["seller"];
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



            #endregion

            string attachment = context.Request.Params["attachmentno"];
            string strsql = @"insert into Icontract(contractNo, econtractNo,attachmentNo, businessclass, seller, signedtime, signedplace, buyer,
buyeraddress, currency, pricement1, pricement1per, pricement2, pricement2per, pvalidity, 
shipment, transport, tradement, harborout, harborarrive, harborclear, placement, validity, remark, templateno, status, createman, createdate, lastmod, lastmoddate)
values(@contractNo,@econtractNo,@attachmentno,@businessclass,@seller,@signedtime,@signedplace,@buyer,@buyeraddress,@currency,@pricement1,
@pricement1per,@pricement2,@pricement2per,@pvalidity,@shipment,@transport,@tradement,@harborout,@harborarrive,@harborclear,@placement,
@validity,@remark,@templateno,@status,@createman,@createdate,@lastmod,@lastmoddate);";



            #region mms参数
            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNumber,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@econtractNo",Value=context.Request.Params["econtractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessclass",Value=context.Request.Params["businessclass"],Size=30},

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
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value=DateTime.Now,Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DateTime.Now,Size=8},
};
            #endregion

            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(@"insert into Icontract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");


            bool issuc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    #region 判断商品数量与价格

                    foreach (var item in listtable)
                    {
                        //先拿到用户提交过来的商品的名称，数量与价格
                        var pcode = item["pcode"];
                        int addQuantity = Convert.ToInt32(item["quantity"]);

                        decimal eprice = item["eprice"] == "" ? 0 : Convert.ToDecimal(item["eprice"]);
                        decimal downprice = item["downprice"] == "" ? 0 : Convert.ToDecimal(item["downprice"]);
                        string attachmentno = context.Request["attachmentno"];
                        int existsQuantity = 0;

                        string sqlExists = string.Empty;
                        SqlParameter[] pm = new SqlParameter[]
                    {
                        new SqlParameter("@econtractNo",context.Request.Params["econtractNo"]),
                        new SqlParameter("@pcode",pcode),
                        
                        new SqlParameter("@attachmentno",context.Request.Params["attachmentno"]),
                    };
                        //根据境外合同号和附件编号查询境内产品表中已存在产品的数量
                        if (string.IsNullOrEmpty(attachmentno))
                        {
                            var s = context.Request.Params["econtractNo"];
                            sqlExists = "select contractNo from Icontract where econtractNo=@econtractNo ";
                        }
                        else
                        {
                            sqlExists = "select contractNO from Icontract where econtractNo=@econtractNo and attachmentno=@attachmentNo ";
                        }
                        DataTable dtExists = bll.ExecDatasetSql(sqlExists, pm).Tables[0];

                        if (dtExists.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtExists.Rows.Count; i++)
                            {
                                //根据获得的境内合同号查询境内产品表中该合同号下的数量
                                string ass = dtExists.Rows[i][0].ToString();
                                SqlParameter[] existPms = new SqlParameter[]
                                {
                                    new SqlParameter("@contractNo",dtExists.Rows[i][0].ToString()),
                                    new SqlParameter("@pcode",pcode)
                                };
                                existsQuantity += Convert.ToInt32(bll.ExecuteScalar("select quantity from Icontract_ap where contractNo=@contractNo and pcode=@pcode", existPms));
                            }

                        }
                        //根据境外销售合同号和附件编码和添加产品的编码查询境外表中这次添加商品的名称、数量和价格

                        string sql = string.Empty;
                        if (string.IsNullOrEmpty(attachmentno))
                        {
                            sql = "select  quantity,amount,price from Econtract_ap where contractNo=@econtractNo and pcode=@pcode";
                        }
                        else
                        {
                            sql = "select  quantity,amount,price from Econtract_ap where contractNo=@econtractNo and pcode=@pcode and attachmentno=@attachmentno";
                        }


                        DataTable dtAttach = bll.ExecDatasetSql(sql, pm).Tables[0];
                        //获取境外合同内的产品数量和金额和单价
                        int totalCount = 0;
                        decimal amount = 0;
                        decimal price = 0;

                        if (dtAttach.Rows.Count > 0)
                        {
                            totalCount = Convert.ToInt32(dtAttach.Rows[0][0]);
                            amount = Convert.ToDecimal(dtAttach.Rows[0][1]);
                            price = Convert.ToDecimal(dtAttach.Rows[0][2]);
                        }

                        //判断添加商品的数量和价格
                        if (addQuantity + existsQuantity > totalCount)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;数量超出限制";
                            return false;
                        }
                        //单价的减幅大于商品的单价
                        if (eprice > price)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;单价减幅超出限制";
                            return false;
                        }
                        //总的减价额大于商品总价
                        if (downprice > amount)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;总减价额超出限制";
                            return false;
                        }



                    }

                    #endregion


                    bll.ExecuteNonQuery(strsql, mms);
                    foreach (Hashtable hs in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNumber },
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
                    #region 生成模板
                    //                    var templateNo = context.Request.Params["templateno"];
                    //                    SqlParameter[] sqlpps = new SqlParameter[]
                    //            {
                    //                new SqlParameter("@templateno", templateNo),
                    //                new SqlParameter("@sellerName",context.Request.Params["seller"])

                    //            };
                    //                    //从基础模板中读取数据
                    //                    DataSet ds = bll.ExecDatasetSql(@"select * from btemplate_contract where templateno=@templateno;
                    //select name,address from bsupplier where name=@sellerName", sqlpps);
                    //                    DataTable dt = ds.Tables[0];
                    //                    DataTable dtSeller = ds.Tables[1];
                    //                    string seller = dtSeller.Rows[0][0].ToString();
                    //                    string sellerAddress = dtSeller.Rows[0][1].ToString();
                    //                    string content = dt.Rows[0][6].ToString();
                    //                    string sortno = dt.Rows[0][5].ToString();
                    //                    string validity = context.Request.Params["validity"];
                    //                    StringBuilder sbProduct = new StringBuilder();

                    //                    #region 拼接表单
                    //                    sbProduct.Append(@"<table style='border-collapse:collapse;border:none;' border='1' cellpadding='0' cellspacing='0'>
                    //             <tbody><tr><td style='border:solid black 1.0pt;' valign='top' width='103'>
                    //
                    //                <p class='A0' style='text-align:center;' align='center'>
                    //                    <span style='font-family:宋体;'>序号</span><span></span>
                    //                </p>
                    //            </td>
                    //        
                    //         
                    //            <td style='border:solid black 1.0pt;' valign='top' width='202'>
                    //               
                    //                <p class='A0' style='text-indent:47.25pt;'>
                    //                    <span style='font-family:宋体;'>名称</span><span></span>
                    //                </p>
                    //            </td>
                    //            <td style='border:solid black 1.0pt;' valign='top' width='177'>
                    //              
                    //                <p class='A0' style='text-align:center;' align='center'>
                    //                    <span style='font-family:宋体;'>单价<span>&nbsp; </span></span><span></span>
                    //                </p>
                    //            </td>
                    //            <td style='border:solid black 1.0pt;' valign='top' width='139'>
                    //               
                    //                <p class='A0' style='text-align:center;' align='center'>
                    //                    <span style='font-family:宋体;'>数量</span><span></span>
                    //                </p>
                    //            </td>
                    //            <td style='border:solid black 1.0pt;' valign='top' width='207'>
                    //              
                    //                </p>
                    //                <p class='A0' style='text-align:center;' align='center'>
                    //                    <span style='font-family:宋体;'>总价</span><span></span>
                    //                </p>
                    //            </td>
                    //        </tr>");
                    //                    decimal totalValue = 0;
                    //                    foreach (Hashtable hs in listtable)
                    //                    {
                    //                        decimal quantity = Convert.ToDecimal(hs["quantity"]);
                    //                        decimal total = Convert.ToDecimal(hs["price"]);

                    //                        decimal totalPrice = quantity * total;
                    //                        totalValue += totalPrice;
                    //                        sbProduct.Append(@"<tr><td style='border:solid black 1.0pt;' width='103'>
                    //                <p class='A0' style='text-align:center;' align='center'>
                    //                    <span style='font-family:&quot;'>" + hs["pcode"] + "</span><span></span></p></td>");
                    //                        sbProduct.Append(@"<td style='border:solid black 1.0pt;' width='202'>
                    //              
                    //                    <p class='A0' style='text-align:left;text-indent:21.0pt;' align='left'>
                    //                    <span style='font-family:&quot;'><span>&nbsp;</span>" + hs["pname"] + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td>");
                    //                        sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='177'>
                    //                         <p  style='text-align:cneter;' >
                    //                         <span style='font-family:&quot;'><span>&nbsp;&nbsp;&nbsp; </span>" + hs["price"] + "</span></p>");
                    //                        sbProduct.Append(@" <td style='border:solid black 1.0pt;' width='139'>
                    //                <p class='A0' style='text-indent:5.25pt;'>
                    //                    <span style='font-family:&quot;'>" + hs["quantity"] + "</span></p></td>");
                    //                        sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='207'>
                    //                <p class='A0' style='text-align:center;' align='center'>
                    //                    <span style='font-family:&quot;'>" + totalPrice + "</span></p></td></tr>");
                    //                    }





                    //                    sbProduct.Append(@"<tr style='height:75px; text-align:center'> <td style='border:solid black 1.0pt;' valign='top' width='103'>
                    //              
                    //                <p class='A0'></br>
                    //                    <span style='font-family:&quot;color:windowtext;'><span>&nbsp; </span></span><span style='font-family:宋体;color:windowtext;'>总计</span><span style='color:windowtext;'></span>
                    //                </p>
                    //            </td>
                    //            <td colspan='4' style='border:solid black 1.0pt;' valign='top' width='724'>
                    //              
                    //                <p class='MsoNormal'></br>
                    //                    <span style='font-size:16.5pt;font-family:宋体;color:black;'>总价:" + totalValue + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td></tr></tbody></table>");




                    //                    #endregion



                    //                    var place = context.Request.Params["signedplace"];
                    //                    content = content.Replace("$contractNo", contractNumber)
                    //                            .Replace("$date", context.Request.Params["signedtime"]).Replace("$buyer", context.Request.Params["buyer"])
                    //                            .Replace("$BuyerAddress", context.Request.Params["buyeraddress"]).Replace("$seller", seller)
                    //                            .Replace("$SellerAddress", sellerAddress).Replace("$productTable", sbProduct.ToString())
                    //                            .Replace("$signAddress", context.Request.Params["signedplace"])
                    //                            .Replace("$validity", context.Request.Params["validity"]);
                    //                    //插入境内模板
                    //                    StringBuilder sbEntry = new StringBuilder();
                    //                    sbEntry.Append(@"insert into Icontract_t(contractNo, templateno, language, sortno, content, createman, createdate, lastmod, lastmoddate)
                    //                     values(@contractNo, @templateno, @language, @sortno, @content, @createman, @createdate, @lastmod, @lastmoddate)");
                    //                    SqlParameter[] pms = new SqlParameter[]{
                    //                        new SqlParameter("@contractNo",contractNumber),
                    //                        new SqlParameter("@templateno",context.Request.Params["templateno"]),
                    //                        new SqlParameter("@language","中文"),
                    //                        new SqlParameter("@sortno",sortno),
                    //                        new SqlParameter("@content",content),
                    //                        new SqlParameter("@createman",context.Request.Params["createman"]),
                    //                        new SqlParameter("@createdate",context.Request.Params["createdate"]),
                    //                        new SqlParameter("@lastmod",context.Request.Params["lastmod"]),
                    //                        new SqlParameter("@lastmoddate",context.Request.Params["lastmoddate"]),

                    //                    };
                    //                    bll.ExecuteNonQuery(sbEntry.ToString(), pms);
                    #endregion

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
        private bool addContract(ref string errorinfo, HttpContext context)
        {
            //数据验证
            string status = string.Empty;
            if (context.Request.Params["buyeraddress"].Length == 0)
            {
                errorinfo = "买家地址不能为空!";
                return false;
            }
            if (context.Request.Params["templateno"].Length == 0)
            {
                errorinfo = "模板编号不能为空!";
                return false;
            }
            if (context.Request.Params["seller"].Length == 0)
            {
                errorinfo = "卖家不能为空";
                return false;
            }
            if (context.Request.QueryString["status"]=="0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            #region 生成合同编号
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                var contractFirst = bll.ExecuteScalar(@"select top 1  contractNo from Icontract  order by contractNo desc");
                if (contractFirst!=null)
                {
                    string[] contractArray = contractFirst.ToString().Split('-');
                    contractFirst = contractArray[1];
                    contractOrg = Convert.ToInt32(contractFirst);
                }
                //查询数据表中是否存在买方卖方
                string buyCompany = context.Request.Params["buyer"];
                string sellerCompany = context.Request.Params["seller"];
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



            #endregion

            string attachment = context.Request.Params["attachmentno"];
            string strsql = @"insert into Icontract(contractNo, econtractNo,attachmentNo, businessclass, seller, signedtime, signedplace, buyer,
buyeraddress, currency, pricement1, pricement1per, pricement2, pricement2per, pvalidity, 
shipment, transport, tradement, harborout, harborarrive, harborclear, placement, validity, remark, templateno, status, createman, createdate, lastmod, lastmoddate)
values(@contractNo,@econtractNo,@attachmentno,@businessclass,@seller,@signedtime,@signedplace,@buyer,@buyeraddress,@currency,@pricement1,
@pricement1per,@pricement2,@pricement2per,@pvalidity,@shipment,@transport,@tradement,@harborout,@harborarrive,@harborclear,@placement,
@validity,@remark,@templateno,@status,@createman,@createdate,@lastmod,@lastmoddate);";



            #region mms参数
            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=contractNumber,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@econtractNo",Value=context.Request.Params["econtractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessclass",Value=context.Request.Params["businessclass"],Size=30},

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
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value=DateTime.Now,Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DateTime.Now,Size=8},
};
            #endregion

            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(@"insert into Icontract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");


            bool issuc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    #region 判断商品数量与价格

                    foreach (var item in listtable)
                    {
                        //先拿到用户提交过来的商品的名称，数量与价格
                        var pcode = item["pcode"];
                        int addQuantity = Convert.ToInt32(item["quantity"]);

                        decimal eprice = item["eprice"] == "" ? 0 : Convert.ToDecimal(item["eprice"]);
                        decimal downprice = item["downprice"] == "" ? 0 : Convert.ToDecimal(item["downprice"]);
                        string attachmentno = context.Request["attachmentno"];
                        int existsQuantity = 0;

                        string sqlExists = string.Empty;
                        SqlParameter[] pm = new SqlParameter[]
                    {
                        new SqlParameter("@econtractNo",context.Request.Params["econtractNo"]),
                        new SqlParameter("@pcode",pcode),
                        
                        new SqlParameter("@attachmentno",context.Request.Params["attachmentno"]),
                    };
                        //根据境外合同号和附件编号查询境内产品表中已存在产品的数量
                        if (string.IsNullOrEmpty(attachmentno))
                        {
                            var s = context.Request.Params["econtractNo"];
                            sqlExists = "select contractNo from Icontract where econtractNo=@econtractNo ";
                        }
                        else
                        {
                            sqlExists = "select contractNO from Icontract where econtractNo=@econtractNo and attachmentno=@attachmentNo ";
                        }
                        DataTable dtExists = bll.ExecDatasetSql(sqlExists, pm).Tables[0];

                        if (dtExists.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtExists.Rows.Count; i++)
                            {
                                //根据获得的境内合同号查询境内产品表中该合同号下的数量
                                string ass = dtExists.Rows[i][0].ToString();
                                SqlParameter[] existPms = new SqlParameter[]
                                {
                                    new SqlParameter("@contractNo",dtExists.Rows[i][0].ToString()),
                                    new SqlParameter("@pcode",pcode)
                                };
                                existsQuantity += Convert.ToInt32(bll.ExecuteScalar("select quantity from Icontract_ap where contractNo=@contractNo and pcode=@pcode", existPms));
                            }

                        }
                        //根据境外销售合同号和附件编码和添加产品的编码查询境外表中这次添加商品的名称、数量和价格

                        string sql = string.Empty;
                        if (string.IsNullOrEmpty(attachmentno))
                        {
                            sql = "select  quantity,amount,price from Econtract_ap where contractNo=@econtractNo and pcode=@pcode";
                        }
                        else
                        {
                            sql = "select  quantity,amount,price from Econtract_ap where contractNo=@econtractNo and pcode=@pcode and attachmentno=@attachmentno";
                        }


                        DataTable dtAttach = bll.ExecDatasetSql(sql, pm).Tables[0];
                        //获取境外合同内的产品数量和金额和单价
                        int totalCount = 0;
                        decimal amount = 0;
                        decimal price = 0;

                        if (dtAttach.Rows.Count > 0)
                        {
                            totalCount = Convert.ToInt32(dtAttach.Rows[0][0]);
                            amount = Convert.ToDecimal(dtAttach.Rows[0][1]);
                            price = Convert.ToDecimal(dtAttach.Rows[0][2]);
                        }

                        //判断添加商品的数量和价格
                        if (addQuantity + existsQuantity > totalCount)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;数量超出限制";
                            return false;
                        }
                        //单价的减幅大于商品的单价
                        if (eprice > price)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;单价减幅超出限制";
                            return false;
                        }
                        //总的减价额大于商品总价
                        if (downprice > amount)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;总减价额超出限制";
                            return false;
                        }



                    }

                    #endregion


                    bll.ExecuteNonQuery(strsql, mms);
                    foreach (Hashtable hs in listtable)
                    {
                        SqlParameter[] mms2 = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@contractNo",Value=contractNumber },
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
                    #region 生成模板
//                    var templateNo = context.Request.Params["templateno"];
//                    SqlParameter[] sqlpps = new SqlParameter[]
//            {
//                new SqlParameter("@templateno", templateNo),
//                new SqlParameter("@sellerName",context.Request.Params["seller"])
              
//            };
//                    //从基础模板中读取数据
//                    DataSet ds = bll.ExecDatasetSql(@"select * from btemplate_contract where templateno=@templateno;
//select name,address from bsupplier where name=@sellerName", sqlpps);
//                    DataTable dt = ds.Tables[0];
//                    DataTable dtSeller = ds.Tables[1];
//                    string seller = dtSeller.Rows[0][0].ToString();
//                    string sellerAddress = dtSeller.Rows[0][1].ToString();
//                    string content = dt.Rows[0][6].ToString();
//                    string sortno = dt.Rows[0][5].ToString();
//                    string validity = context.Request.Params["validity"];
//                    StringBuilder sbProduct = new StringBuilder();

//                    #region 拼接表单
//                    sbProduct.Append(@"<table style='border-collapse:collapse;border:none;' border='1' cellpadding='0' cellspacing='0'>
//             <tbody><tr><td style='border:solid black 1.0pt;' valign='top' width='103'>
//
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:宋体;'>序号</span><span></span>
//                </p>
//            </td>
//        
//         
//            <td style='border:solid black 1.0pt;' valign='top' width='202'>
//               
//                <p class='A0' style='text-indent:47.25pt;'>
//                    <span style='font-family:宋体;'>名称</span><span></span>
//                </p>
//            </td>
//            <td style='border:solid black 1.0pt;' valign='top' width='177'>
//              
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:宋体;'>单价<span>&nbsp; </span></span><span></span>
//                </p>
//            </td>
//            <td style='border:solid black 1.0pt;' valign='top' width='139'>
//               
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:宋体;'>数量</span><span></span>
//                </p>
//            </td>
//            <td style='border:solid black 1.0pt;' valign='top' width='207'>
//              
//                </p>
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:宋体;'>总价</span><span></span>
//                </p>
//            </td>
//        </tr>");
//                    decimal totalValue = 0;
//                    foreach (Hashtable hs in listtable)
//                    {
//                        decimal quantity = Convert.ToDecimal(hs["quantity"]);
//                        decimal total = Convert.ToDecimal(hs["price"]);

//                        decimal totalPrice = quantity * total;
//                        totalValue += totalPrice;
//                        sbProduct.Append(@"<tr><td style='border:solid black 1.0pt;' width='103'>
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:&quot;'>" + hs["pcode"] + "</span><span></span></p></td>");
//                        sbProduct.Append(@"<td style='border:solid black 1.0pt;' width='202'>
//              
//                    <p class='A0' style='text-align:left;text-indent:21.0pt;' align='left'>
//                    <span style='font-family:&quot;'><span>&nbsp;</span>" + hs["pname"] + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td>");
//                        sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='177'>
//                         <p  style='text-align:cneter;' >
//                         <span style='font-family:&quot;'><span>&nbsp;&nbsp;&nbsp; </span>" + hs["price"] + "</span></p>");
//                        sbProduct.Append(@" <td style='border:solid black 1.0pt;' width='139'>
//                <p class='A0' style='text-indent:5.25pt;'>
//                    <span style='font-family:&quot;'>" + hs["quantity"] + "</span></p></td>");
//                        sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='207'>
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:&quot;'>" + totalPrice + "</span></p></td></tr>");
//                    }





//                    sbProduct.Append(@"<tr style='height:75px; text-align:center'> <td style='border:solid black 1.0pt;' valign='top' width='103'>
//              
//                <p class='A0'></br>
//                    <span style='font-family:&quot;color:windowtext;'><span>&nbsp; </span></span><span style='font-family:宋体;color:windowtext;'>总计</span><span style='color:windowtext;'></span>
//                </p>
//            </td>
//            <td colspan='4' style='border:solid black 1.0pt;' valign='top' width='724'>
//              
//                <p class='MsoNormal'></br>
//                    <span style='font-size:16.5pt;font-family:宋体;color:black;'>总价:" + totalValue + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td></tr></tbody></table>");




//                    #endregion



//                    var place = context.Request.Params["signedplace"];
//                    content = content.Replace("$contractNo", contractNumber)
//                            .Replace("$date", context.Request.Params["signedtime"]).Replace("$buyer", context.Request.Params["buyer"])
//                            .Replace("$BuyerAddress", context.Request.Params["buyeraddress"]).Replace("$seller", seller)
//                            .Replace("$SellerAddress", sellerAddress).Replace("$productTable", sbProduct.ToString())
//                            .Replace("$signAddress", context.Request.Params["signedplace"])
//                            .Replace("$validity", context.Request.Params["validity"]);
//                    //插入境内模板
//                    StringBuilder sbEntry = new StringBuilder();
//                    sbEntry.Append(@"insert into Icontract_t(contractNo, templateno, language, sortno, content, createman, createdate, lastmod, lastmoddate)
//                     values(@contractNo, @templateno, @language, @sortno, @content, @createman, @createdate, @lastmod, @lastmoddate)");
//                    SqlParameter[] pms = new SqlParameter[]{
//                        new SqlParameter("@contractNo",contractNumber),
//                        new SqlParameter("@templateno",context.Request.Params["templateno"]),
//                        new SqlParameter("@language","中文"),
//                        new SqlParameter("@sortno",sortno),
//                        new SqlParameter("@content",content),
//                        new SqlParameter("@createman",context.Request.Params["createman"]),
//                        new SqlParameter("@createdate",context.Request.Params["createdate"]),
//                        new SqlParameter("@lastmod",context.Request.Params["lastmod"]),
//                        new SqlParameter("@lastmoddate",context.Request.Params["lastmoddate"]),
                     
//                    };
//                    bll.ExecuteNonQuery(sbEntry.ToString(), pms);
                    #endregion

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

        private bool editContract(ref string errorinfo, HttpContext context)
        {
            string status = string.Empty;
            if (context.Request.QueryString["status"] == "0")
            {
                status = "新建";
            }
            else if (context.Request.QueryString["status"] == "1")
            {
                status = "提交";
            }
            #region 生成合同编号
            string contractNumber = string.Empty;
            //获取要添加的买方卖方公司名称和数据库名称相比较生成合同编号

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                //查询境内合同表最后添加的一条数据，获取合同号的后三位。
                int contractOrg = 0;
                string contractFirst = bll.ExecuteScalar(@"select top 1  contractNo from Icontract  order by contractNo desc").ToString();
                if (!string.IsNullOrEmpty(contractFirst))
                {
                    string[] contractArray = contractFirst.Split('-');
                    contractFirst = contractArray[1];
                    contractOrg = Convert.ToInt32(contractFirst);
                }
                //查询数据表中是否存在买方卖方
                string buyCompany = context.Request.Params["buyer"];
                string sellerCompany = context.Request.Params["seller"];
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



            #endregion

            string contractNo = context.Request.Params["contractNo"];
            string strsql = @"update Icontract set contractNo=@contractNumber,econtractNo=@econtractNo,attachmentNo=@attachmentNo, businessclass=@businessclass,
seller=@seller,signedtime=@signedtime,signedplace=@signedplace,buyer=@buyer,
buyeraddress=@buyeraddress,currency=@currency,pricement1=@pricement1,
pricement1per=@pricement1per,pricement2=@pricement2,pricement2per=@pricement2per,
pvalidity=@pvalidity,shipment=@shipment,transport=@transport,tradement=@tradement,
harborout=@harborout,harborarrive=@harborarrive,harborclear=@harborclear,
placement=@placement,validity=@validity,remark=@remark,templateno=@templateno,
lastmod=@lastmod,lastmoddate=@lastmoddate where contractNo=@contractNo ";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNumber",Value=contractNumber,Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@econtractNo",Value=context.Request.Params["econtractNo"]??"",Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentNo",Value=context.Request.Params["attachmentNo"],Size=30},

new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessclass",Value=context.Request.Params["businessclass"],Size=30},

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
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value=DateTime.Now,Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DateTime.Now,Size=8},
};
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();
            sb2.Append(@"insert into Icontract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");
            sb3.Append(@"update Icontract_ap set  pname=@pname,quantity=@quantity,qunit=@qunit,price=@price,amount=@amount,packspec=@packspec,
packing=@packing,pallet=@pallet,ifcheck=@ifcheck,ifplace=@ifplace where contractNo=@contractNo and attachmentno=@attachmentno and pcode=@pcode;");


            bool issuc = false;
            //update 和 insert 产品表
            string oldpcodes = "";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                SqlParameter[] ss2 = new SqlParameter[] { 
                    new SqlParameter{ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30}
                };
                DataTable dt = bll.ExecDatasetSql(" select pcode from Icontract_ap where contractNo=@contractNo and attachmentno='' ", ss2).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    oldpcodes = oldpcodes + "#" + dr["pcode"].ToString();
                }
            }
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {

                    bll.SqlTran = bll.SqlCon.BeginTransaction();

                    #region 判断商品数量与价格

                    foreach (var item in listtable)
                    {
                        //先拿到用户提交过来的商品的名称，数量与价格
                        var pcode = item["pcode"];
                        int addQuantity = Convert.ToInt32(item["quantity"]);
                        decimal eprice = item["eprice"] == "" ? 0 : Convert.ToDecimal(item["eprice"]);
                        decimal downprice = item["downprice"] == "" ? 0 : Convert.ToDecimal(item["downprice"]);
                        int existsQuantity = 0;
                        string attachmentno = context.Request["attachmentno"];
                        SqlParameter[] pm = new SqlParameter[]
                    {
                        new SqlParameter("@econtractNo",context.Request.Params["econtractNo"]),
                        new SqlParameter("@pcode",pcode),
                        new SqlParameter("@contractNo",context.Request.Params["contractNo"]),
                        new SqlParameter("@attachmentno",context.Request.Params["attachmentno"]),
                    };
                        //再拿到用户修改的合同下原有产品的数量
                        int editQuantity = Convert.ToInt32(bll.ExecuteScalar(@"select quantity from Icontract_ap where contractNo=@contractNo and pcode=@pcode", pm));
                        //根据境外合同号和附件编号查询境内产品表中已存在产品的数量
                        string sqlExists = string.Empty;

                        if (string.IsNullOrEmpty(attachmentno))
                        {
                            sqlExists = "select contractNo from Icontract where econtractNo=@econtractNo ";
                        }
                        else
                        {
                            sqlExists = "select contractNO from Icontract where econtractNo=@econtractNo and attachmentno=@attachmentNo ";
                        }
                        DataTable dtExists = bll.ExecDatasetSql(sqlExists, pm).Tables[0];

                        if (dtExists.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtExists.Rows.Count; i++)
                            {
                                //根据获得的境内合同号查询境内产品表中该合同号下的数量
                                string ass = dtExists.Rows[i][0].ToString();
                                SqlParameter[] existPms = new SqlParameter[]
                                {
                                    new SqlParameter("@contractNo",dtExists.Rows[i][0].ToString()),
                                    new SqlParameter("@pcode",pcode)
                                };
                                existsQuantity += Convert.ToInt32(bll.ExecuteScalar("select quantity from Icontract_ap where contractNo=@contractNo and pcode=@pcode", existPms));
                            }

                        }
                        //获取提交过来的境外附件编号，如果选择附件编号，则根据境外号和附件编号查询境外产品表

                        string sql = string.Empty;
                        if (string.IsNullOrEmpty(attachmentno))
                        {
                            sql = "select  quantity,amount,price from Econtract_ap where contractNo=@econtractNo and pcode=@pcode";
                        }
                        else
                        {
                            sql = "select  quantity,amount,price from Econtract_ap where contractNo=@econtractNo and pcode=@pcode and attachmentno=@attachmentno";
                        }


                        DataTable dtAttach = bll.ExecDatasetSql(sql, pm).Tables[0];
                        //获取境外合同内的产品数量和金额和单价
                        int totalCount = Convert.ToInt32(dtAttach.Rows[0][0]);
                        var s = dtAttach.Rows[0][1];
                        decimal amount = Convert.ToDecimal(dtAttach.Rows[0][1]);
                        decimal price = Convert.ToDecimal(dtAttach.Rows[0][2]);
                        //判断添加商品的数量和价格
                        if (addQuantity + existsQuantity - editQuantity > totalCount)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;数量超出限制";
                            return false;
                        }
                        //单价的减幅大于商品的单价
                        if (eprice > price)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;单价减幅超出限制";
                            return false;
                        }
                        //总的减价额大于商品总价
                        if (downprice > amount)
                        {
                            errorinfo = "要添加的商品:&nbsp;" + item["pname"] + "&nbsp;总减价额超出限制";
                            return false;
                        }



                    }







                    #endregion


                    //数据验证

                    if (context.Request.Params["seller"].Length == 0)
                    {
                        errorinfo = "卖家不能为空";
                        return false;
                    }

                    bll.ExecuteNonQuery(strsql, mms);
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
                        if (oldpcodes.Contains(hs["pcode"].ToString()))
                        {
                            bll.ExecuteNonQuery(sb3.ToString(), mms2);
                        }
                        else
                        {
                            bll.ExecuteNonQuery(sb2.ToString(), mms2);
                        }
                    }
                    //删除该合同号下的所有模板然后添加

                    int r = Convert.ToInt32(bll.ExecuteScalar(@"delete  from Icontract_t where contractno=@contractNumber", mms));



                    #region 生成模板
//                    var templateNo = context.Request.Params["templateno"];
//                    SqlParameter[] sqlpps = new SqlParameter[]
//            {
//                new SqlParameter("@templateno", templateNo),
//                new SqlParameter("@sellerName",context.Request.Params["seller"])
              
//            };
//                    //从基础模板中读取数据
//                    DataSet ds = bll.ExecDatasetSql(@"select * from btemplate_contract where templateno=@templateno;
//select name,address from bcustomer where name=@sellerName", sqlpps);
//                    DataTable dt = ds.Tables[0];
//                    DataTable dtSeller = ds.Tables[1];
//                    string seller = dtSeller.Rows[0][0].ToString();
//                    string sellerAddress = dtSeller.Rows[0][1].ToString();
//                    string content = dt.Rows[0][6].ToString();
//                    string sortno = dt.Rows[0][5].ToString();
//                    StringBuilder sbProduct = new StringBuilder();
//                    #region 拼接表单
//                    sbProduct.Append(@"<table style='border-collapse:collapse;border:none;' border='1' cellpadding='0' cellspacing='0'>
//             <tbody><tr><td style='border:solid black 1.0pt;' valign='top' width='103'>
//              
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:宋体;'>序号</span><span></span>
//                </p>
//            </td>
//        
//         
//            <td style='border:solid black 1.0pt;' valign='top' width='202'>
//               
//                <p class='A0' style='text-indent:47.25pt;'>
//                    <span style='font-family:宋体;'>名称</span><span></span>
//                </p>
//            </td>
//            <td style='border:solid black 1.0pt;' valign='top' width='177'>
//            
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:宋体;'>单价<span>&nbsp; </span></span><span></span>
//                </p>
//            </td>
//            <td style='border:solid black 1.0pt;' valign='top' width='139'>
//             
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:宋体;'>数量</span><span></span>
//                </p>
//            </td>
//            <td style='border:solid black 1.0pt;' valign='top' width='207'>
//              
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:宋体;'>总价</span><span></span>
//                </p>
//            </td>
//        </tr>");
//                    decimal totalValue = 0;
//                    foreach (Hashtable hs in listtable)
//                    {
//                        decimal quantity = Convert.ToDecimal(hs["quantity"]);
//                        decimal total = Convert.ToDecimal(hs["price"]);

//                        decimal totalPrice = quantity * total;
//                        totalValue += totalPrice;
//                        sbProduct.Append(@"<tr><td style='border:solid black 1.0pt;' width='103'>
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:&quot;'>" + hs["pcode"] + "</span><span></span></p></td>");
//                        sbProduct.Append(@"<td style='border:solid black 1.0pt;' width='202'>
//              
//                    <p class='A0' style='text-align:left;text-indent:21.0pt;' align='left'>
//                    <span style='font-family:&quot;'><span>&nbsp;</span>" + hs["pname"] + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td>");
//                        sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='177'>
//                         <p  style='text-align:cneter;' >
//                         <span style='font-family:&quot;'><span>&nbsp;&nbsp;&nbsp; </span>" + hs["price"] + "</span></p>");
//                        sbProduct.Append(@" <td style='border:solid black 1.0pt;' width='139'>
//                <p class='A0' style='text-indent:5.25pt;'>
//                    <span style='font-family:&quot;'>" + hs["quantity"] + "</span></p></td>");
//                        sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='207'>
//                <p class='A0' style='text-align:center;' align='center'>
//                    <span style='font-family:&quot;'>" + totalPrice + "</span></p></td></tr>");
//                    }





//                    sbProduct.Append(@"<tr style='height:75px; text-align:center'> <td style='border:solid black 1.0pt;' valign='top' width='103'>
//              
//                <p class='A0'></br>
//                    <span style='font-family:&quot;color:windowtext;'><span>&nbsp; </span></span><span style='font-family:宋体;color:windowtext;'>总计</span><span style='color:windowtext;'></span>
//                </p>
//            </td>
//            <td colspan='4' style='border:solid black 1.0pt;' valign='top' width='724'>
//              
//                <p class='MsoNormal'></br>
//                    <span style='font-size:16.5pt;font-family:宋体;color:black;'>总价:" + totalValue + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td></tr></tbody></table>");




//                    #endregion

//                    content = content.Replace("$contractNo", contractNumber)
//                            .Replace("$date", context.Request.Params["signedtime"]).Replace("$buyer", context.Request.Params["buyer"])
//                            .Replace("$BuyerAddress", context.Request.Params["buyeraddress"]).Replace("$seller", seller)
//                            .Replace("$SellerAddress", sellerAddress).Replace("$productTable", sbProduct.ToString())
//                            .Replace("$validity", context.Request.Params["validity"])
//                            .Replace("$signAddress", context.Request.Params["signedplace"]);
//                    //插入境内模板
//                    StringBuilder sbEntry = new StringBuilder();
//                    sbEntry.Append(@"insert into Icontract_t(contractNo, templateno, language, sortno, content, createman, createdate, lastmod, lastmoddate)
//                     values(@contractNo, @templateno, @language, @sortno, @content, @createman, @createdate, @lastmod, @lastmoddate)");

//                    SqlParameter[] pms = new SqlParameter[]{
//                        new SqlParameter("@contractNo",contractNumber),
//                        new SqlParameter("@templateno",context.Request.Params["templateno"]),
//                        new SqlParameter("@language","中文"),
//                        new SqlParameter("@sortno",sortno),
//                        new SqlParameter("@content",content),
//                        new SqlParameter("@createman",context.Request.Params["createman"]),
//                        new SqlParameter("@createdate",context.Request.Params["createdate"]),
//                        new SqlParameter("@lastmod",context.Request.Params["lastmod"]),
//                        new SqlParameter("@lastmoddate",context.Request.Params["lastmoddate"]),
                     
//                    };
//                    bll.ExecuteNonQuery(sbEntry.ToString(), pms);
                    #endregion




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


        private bool addContract_a(ref string errorinfo, HttpContext context)
        {


            string contractNo = context.Request.Params["contractNo"];
            string attachmentno = context.Request.Params["attachmentno"];
            string strsql = @"insert into Icontract_a
(contractNo,attachmentno,seller,signedtime,signedplace,buyer,
buyeraddress,currency,pricement1,pricement1per,pricement2,pricement2per,pvalidity,shipment,transport,tradement,harborout,harborarrive,harborclear,
placement,validity,remark,templateno,status,createman,createdate,lastmod,lastmoddate)
values(@contractNo,@attachmentno,@seller,@signedtime,@signedplace,@buyer,@buyeraddress,@currency,@pricement1,
@pricement1per,@pricement2,@pricement2per,@pvalidity,@shipment,@transport,@tradement,@harborout,@harborarrive,@harborclear,@placement,
@validity,@remark,@templateno,@status,@createman,@createdate,@lastmod,@lastmoddate);";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"],Size=30},



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
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value="新建",Size=10},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value=DateTime.Now,Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DateTime.Now,Size=8},
};
            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(@" insert into Icontract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
                        @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");

            bool issuc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql, mms);
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

                    #region 生成模板
                    var templateNo = context.Request.Params["templateno"];
                    SqlParameter[] sqlpps = new SqlParameter[]
            {
                new SqlParameter("@templateno", templateNo),
                new SqlParameter("@code",context.Request.Params["seller"])
              
            };
                    //从基础模板中读取数据
                    DataSet ds = bll.ExecDatasetSql(@"select * from btemplate_attach where templateno=@templateno and templatename='境内附件模板';
select name,address from bcustomer where code=@code", sqlpps);
                    DataTable dt = ds.Tables[0];
                    DataTable dtSeller = ds.Tables[1];
                    string seller = dtSeller.Rows[0][0].ToString();
                    string sellerAddress = dtSeller.Rows[0][1].ToString();
                    string content = dt.Rows[0][6].ToString();
                    string sortno = dt.Rows[0][5].ToString();
                    StringBuilder sbProduct = new StringBuilder();
                    #region 拼接表单
                    sbProduct.Append(@"<table style='border-collapse:collapse;border:none;' border='1' cellpadding='0' cellspacing='0'>
             <tbody><tr><td style='border:solid black 1.0pt;' valign='top' width='103'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Item</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>序号</span><span></span>
                </p>
            </td>
        
         
            <td style='border:solid black 1.0pt;' valign='top' width='202'>
                <p class='A0' style='text-indent:10.5pt;'>
                    <span style='font-family:&quot;'>Name of Commodity</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-indent:47.25pt;'>
                    <span style='font-family:宋体;'>名称</span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='177'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Unit Price</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>单价<span>&nbsp; </span></span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='139'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Quantity</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>数量</span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='207'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Total Amount</span><span style='font-family:宋体;'>（</span><span style='font-family:&quot;'>USD</span><span style='font-family:宋体;'>）</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>总价</span><span></span>
                </p>
            </td>
        </tr>'");
                    decimal totalValue = 0;
                    foreach (Hashtable hs in listtable)
                    {
                        decimal quantity = Convert.ToDecimal(hs["quantity"]);
                        decimal total = Convert.ToDecimal(hs["price"]);

                        decimal totalPrice = quantity * total;
                        totalValue += totalPrice;
                        sbProduct.Append(@"<tr><td style='border:solid black 1.0pt;' width='103'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>" + hs["pcode"] + "</span><span></span></p></td>");
                        sbProduct.Append(@"<td style='border:solid black 1.0pt;' width='202'>
              
                    <p class='A0' style='text-align:left;text-indent:21.0pt;' align='left'>
                    <span style='font-family:&quot;'><span>&nbsp;</span>" + hs["pname"] + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td>");
                        sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='177'>
                         <p  style='text-align:cneter;' >
                         <span style='font-family:&quot;'><span>&nbsp;&nbsp;&nbsp; </span>" + hs["price"] + "</span></p>");
                        sbProduct.Append(@" <td style='border:solid black 1.0pt;' width='139'>
                <p class='A0' style='text-indent:5.25pt;'>
                    <span style='font-family:&quot;'>" + hs["quantity"] + "</span></p></td>");
                        sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='207'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>" + totalPrice + "</span></p></td></tr>");
                    }





                    sbProduct.Append(@"<tr style='height:75px; text-align:center'> <td style='border:solid black 1.0pt;' valign='top' width='103'>
              
                <p class='A0'></br>
                    <span style='font-family:&quot;color:windowtext;'><span>&nbsp; </span></span><span style='font-family:宋体;color:windowtext;'>总计</span><span style='color:windowtext;'></span>
                </p>
            </td>
            <td colspan='4' style='border:solid black 1.0pt;' valign='top' width='724'>
              
                <p class='MsoNormal'></br>
                    <span style='font-size:16.5pt;font-family:宋体;color:black;'>总价:" + totalValue + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td></tr></tbody></table>");




                    #endregion

                    content = content.Replace("$contractNo", context.Request.Params["contractNo"])
                            .Replace("$date", context.Request.Params["signedtime"]).Replace("$buyer", context.Request.Params["buyer"])
                            .Replace("$BuyerAddress", context.Request.Params["buyeraddress"]).Replace("$seller", seller)
                            .Replace("$SellerAddress", sellerAddress).Replace("$productTable", sbProduct.ToString())
                            .Replace("$validity", context.Request.Params["validity"]);
                    //插入境内附件模板
                    StringBuilder sbEntry = new StringBuilder();
                    sbEntry.Append(@"insert into Icontract_at(contractNo,attachmentno ,templateno, language, sortno, content, createman, createdate, lastmod, lastmoddate)
                     values(@contractNo, @attachmentno,@templateno, @language, @sortno, @content, @createman, @createdate, @lastmod, @lastmoddate)");
                    SqlParameter[] pms = new SqlParameter[]{
                        new SqlParameter("@contractNo",context.Request.Params["contractNo"]),
                        new SqlParameter("@templateno",context.Request.Params["templateno"]),
                        new SqlParameter("@language","中文"),
                        new SqlParameter("@sortno",sortno),
                        new SqlParameter("@content",content),
                        new SqlParameter("@createman",context.Request.Params["createman"]),
                        new SqlParameter("@createdate",context.Request.Params["createdate"]),
                        new SqlParameter("@lastmod",context.Request.Params["lastmod"]),
                        new SqlParameter("@lastmoddate",context.Request.Params["lastmoddate"]),
                        new SqlParameter("@attachmentno",context.Request.Params["attachmentno"]),
                     
                    };
                    bll.ExecuteNonQuery(sbEntry.ToString(), pms);
                    #endregion

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
        private bool editContract_a(ref string errorinfo, HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
            string attachmentno = context.Request.Params["attachmentno"];
            string strsql = @"update Icontract_a set seller=@seller,signedtime=@signedtime,signedplace=@signedplace,
buyer=@buyer,buyeraddress=@buyeraddress,currency=@currency,pricement1=@pricement1,pricement1per=@pricement1per,pricement2=@pricement2,
pricement2per=@pricement2per,pvalidity=@pvalidity,shipment=@shipment,transport=@transport,tradement=@tradement,harborout=@harborout,
harborarrive=@harborarrive,harborclear=@harborclear,placement=@placement,validity=@validity,remark=@remark,templateno=@templateno,
lastmod=@lastmod,lastmoddate=@lastmoddate where  contractNo=@contractNo and attachmentno=@attachmentno";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"],Size=30},



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
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value="新建",Size=10},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@createman",Value=DateTime.Now,Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@createdate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@lastmod",Value="",Size=20},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@lastmoddate",Value=DateTime.Now,Size=8},
};

            //获取产品列表
            string str = context.Request.Params["htcplistStr"];
            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);

            //保存产品
            //contractNo	attachmentno	pcode	pname	quantity	qunit	price	amount	packspec	packing	pallet	ifcheck	ifplace
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();
            sb2.Append(@" insert into Icontract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
                        @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");
            sb3.Append(@" update Icontract_ap set  pname=@pname,quantity=@quantity,qunit=@qunit,price=@price,amount=@amount,packspec=@packspec,
packing=@packing,pallet=@pallet,ifcheck=@ifcheck,ifplace=@ifplace where contractNo=@contractNo and attachmentno=@attachmentno and pcode=@pcode;
");
            bool issuc = false;
            string oldpcodes = "";
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                SqlParameter[] ss2 = new SqlParameter[] { 
                    new SqlParameter{ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30}
                };
                DataTable dt = bll.ExecDatasetSql(" select pcode from Icontract_ap where contractNo=@contractNo  ", ss2).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    oldpcodes = oldpcodes + "#" + dr["pcode"].ToString();
                }
            }
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql, mms);
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
                        if (oldpcodes.Contains(hs["pcode"].ToString()))
                        {
                            bll.ExecuteNonQuery(sb3.ToString(), mms2);
                        }
                        else
                        {
                            bll.ExecuteNonQuery(sb2.ToString(), mms2);
                        }
                    }
                    //根据合同号和模板编号查询表中是否存在模板，没有添加，有则更新

                    DataTable dtTemp = bll.ExecDatasetSql(@"select * from Icontract_at where contractno=@contractNo and templateno=@templateNo and attachmentno=@attachmentno", mms).Tables[0];
                    if (dtTemp.Rows.Count <= 0)
                    {
                        #region 生成模板
                        var templateNo = context.Request.Params["templateno"];

                        SqlParameter[] sqlpps = new SqlParameter[]
            {
                new SqlParameter("@templateno", templateNo),
                new SqlParameter("@code",context.Request.Params["seller"]),
            
              
            };
                        //从基础模板中读取数据
                        DataSet ds = bll.ExecDatasetSql(@"select * from btemplate_attach where templateno=@templateno and templatename='境内附件模板';
select name,address from bcustomer where code=@code", sqlpps);
                        DataTable dt = ds.Tables[0];
                        DataTable dtSeller = ds.Tables[1];
                        string seller = dtSeller.Rows[0][0].ToString();
                        string sellerAddress = dtSeller.Rows[0][1].ToString();
                        string content = dt.Rows[0][6].ToString();
                        string sortno = dt.Rows[0][5].ToString();
                        StringBuilder sbProduct = new StringBuilder();
                        #region 拼接表单
                        sbProduct.Append(@"<table style='border-collapse:collapse;border:none;' border='1' cellpadding='0' cellspacing='0'>
             <tbody><tr><td style='border:solid black 1.0pt;' valign='top' width='103'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Item</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>序号</span><span></span>
                </p>
            </td>
        
         
            <td style='border:solid black 1.0pt;' valign='top' width='202'>
                <p class='A0' style='text-indent:10.5pt;'>
                    <span style='font-family:&quot;'>Name of Commodity</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-indent:47.25pt;'>
                    <span style='font-family:宋体;'>名称</span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='177'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Unit Price</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>单价<span>&nbsp; </span></span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='139'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Quantity</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>数量</span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='207'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Total Amount</span><span style='font-family:宋体;'>（</span><span style='font-family:&quot;'>USD</span><span style='font-family:宋体;'>）</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>总价</span><span></span>
                </p>
            </td>
        </tr>'");
                        decimal totalValue = 0;
                        foreach (Hashtable hs in listtable)
                        {
                            decimal quantity = Convert.ToDecimal(hs["quantity"]);
                            decimal total = Convert.ToDecimal(hs["price"]);

                            decimal totalPrice = quantity * total;
                            totalValue += totalPrice;
                            sbProduct.Append(@"<tr><td style='border:solid black 1.0pt;' width='103'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>" + hs["pcode"] + "</span><span></span></p></td>");
                            sbProduct.Append(@"<td style='border:solid black 1.0pt;' width='202'>
              
                    <p class='A0' style='text-align:left;text-indent:21.0pt;' align='left'>
                    <span style='font-family:&quot;'><span>&nbsp;</span>" + hs["pname"] + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td>");
                            sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='177'>
                         <p  style='text-align:cneter;' >
                         <span style='font-family:&quot;'><span>&nbsp;&nbsp;&nbsp; </span>" + hs["price"] + "</span></p>");
                            sbProduct.Append(@" <td style='border:solid black 1.0pt;' width='139'>
                <p class='A0' style='text-indent:5.25pt;'>
                    <span style='font-family:&quot;'>" + hs["quantity"] + "</span></p></td>");
                            sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='207'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>" + totalPrice + "</span></p></td></tr>");
                        }





                        sbProduct.Append(@"<tr style='height:75px; text-align:center'> <td style='border:solid black 1.0pt;' valign='top' width='103'>
              
                <p class='A0'></br>
                    <span style='font-family:&quot;color:windowtext;'><span>&nbsp; </span></span><span style='font-family:宋体;color:windowtext;'>总计</span><span style='color:windowtext;'></span>
                </p>
            </td>
            <td colspan='4' style='border:solid black 1.0pt;' valign='top' width='724'>
              
                <p class='MsoNormal'></br>
                    <span style='font-size:16.5pt;font-family:宋体;color:black;'>总价:" + totalValue + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td></tr></tbody></table>");




                        #endregion

                        content = content.Replace("$contractNo", context.Request.Params["contractNo"])
                                .Replace("$date", context.Request.Params["signedtime"]).Replace("$buyer", context.Request.Params["buyer"])
                                .Replace("$BuyerAddress", context.Request.Params["buyeraddress"]).Replace("$seller", seller)
                                .Replace("$SellerAddress", sellerAddress).Replace("$productTable", sbProduct.ToString())
                                .Replace("$validity", context.Request.Params["validity"]);
                        //插入境内模板
                        StringBuilder sbEntry = new StringBuilder();
                        sbEntry.Append(@"insert into Icontract_at(contractNo,attachmentno, templateno, language, sortno, content, createman, createdate, lastmod, lastmoddate)
                     values(@contractNo,@attachmentno, @templateno, @language, @sortno, @content, @createman, @createdate, @lastmod, @lastmoddate)");

                        SqlParameter[] pms = new SqlParameter[]{
                        new SqlParameter("@contractNo",context.Request.Params["contractNo"]),
                        new SqlParameter("@templateno",context.Request.Params["templateno"]),
                        new SqlParameter("@language","中文"),
                        new SqlParameter("@sortno",sortno),
                        new SqlParameter("@content",content),
                        new SqlParameter("@createman",context.Request.Params["createman"]),
                        new SqlParameter("@createdate",context.Request.Params["createdate"]),
                        new SqlParameter("@lastmod",context.Request.Params["lastmod"]),
                        new SqlParameter("@lastmoddate",context.Request.Params["lastmoddate"]),
                        new SqlParameter("@attachmentno",context.Request.Params["attachmentno"]),
                     
                    };
                        bll.ExecuteNonQuery(sbEntry.ToString(), pms);
                        #endregion
                    }
                    else
                    {
                        #region 更新模板
                        var templateNo = context.Request.Params["templateno"];
                        SqlParameter[] sqlpps = new SqlParameter[]
            {
                new SqlParameter("@templateno", templateNo),
                new SqlParameter("@code",context.Request.Params["seller"])
              
            };
                        //从基础模板中读取数据
                        DataSet ds = bll.ExecDatasetSql(@"select * from btemplate_attach where templateno=@templateno and templatename='境内附件模板';
select name,address from bcustomer where code=@code", sqlpps);
                        DataTable dt = ds.Tables[0];
                        DataTable dtSeller = ds.Tables[1];
                        string seller = dtSeller.Rows[0][0].ToString();
                        string sellerAddress = dtSeller.Rows[0][1].ToString();
                        string content = dt.Rows[0][6].ToString();
                        string sortno = dt.Rows[0][5].ToString();
                        StringBuilder sbProduct = new StringBuilder();
                        #region 拼接表单
                        sbProduct.Append(@"<table style='border-collapse:collapse;border:none;' border='1' cellpadding='0' cellspacing='0'>
             <tbody><tr><td style='border:solid black 1.0pt;' valign='top' width='103'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Item</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>序号</span><span></span>
                </p>
            </td>
        
         
            <td style='border:solid black 1.0pt;' valign='top' width='202'>
                <p class='A0' style='text-indent:10.5pt;'>
                    <span style='font-family:&quot;'>Name of Commodity</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-indent:47.25pt;'>
                    <span style='font-family:宋体;'>名称</span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='177'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Unit Price</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>单价<span>&nbsp; </span></span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='139'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Quantity</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>数量</span><span></span>
                </p>
            </td>
            <td style='border:solid black 1.0pt;' valign='top' width='207'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>Total Amount</span><span style='font-family:宋体;'>（</span><span style='font-family:&quot;'>USD</span><span style='font-family:宋体;'>）</span><span style='font-family:&quot;'></span>
                </p>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:宋体;'>总价</span><span></span>
                </p>
            </td>
        </tr>'");
                        decimal totalValue = 0;
                        foreach (Hashtable hs in listtable)
                        {
                            decimal quantity = Convert.ToDecimal(hs["quantity"]);
                            decimal total = Convert.ToDecimal(hs["price"]);

                            decimal totalPrice = quantity * total;
                            totalValue += totalPrice;
                            sbProduct.Append(@"<tr><td style='border:solid black 1.0pt;' width='103'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>" + hs["pcode"] + "</span><span></span></p></td>");
                            sbProduct.Append(@"<td style='border:solid black 1.0pt;' width='202'>
              
                    <p class='A0' style='text-align:left;text-indent:21.0pt;' align='left'>
                    <span style='font-family:&quot;'><span>&nbsp;</span>" + hs["pname"] + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td>");
                            sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='177'>
                         <p  style='text-align:cneter;' >
                         <span style='font-family:&quot;'><span>&nbsp;&nbsp;&nbsp; </span>" + hs["price"] + "</span></p>");
                            sbProduct.Append(@" <td style='border:solid black 1.0pt;' width='139'>
                <p class='A0' style='text-indent:5.25pt;'>
                    <span style='font-family:&quot;'>" + hs["quantity"] + "</span></p></td>");
                            sbProduct.Append(@"  <td style='border:solid black 1.0pt;' width='207'>
                <p class='A0' style='text-align:center;' align='center'>
                    <span style='font-family:&quot;'>" + totalPrice + "</span></p></td></tr>");
                        }





                        sbProduct.Append(@"<tr style='height:75px; text-align:center'> <td style='border:solid black 1.0pt;' valign='top' width='103'>
              
                <p class='A0'></br>
                    <span style='font-family:&quot;color:windowtext;'><span>&nbsp; </span></span><span style='font-family:宋体;color:windowtext;'>总计</span><span style='color:windowtext;'></span>
                </p>
            </td>
            <td colspan='4' style='border:solid black 1.0pt;' valign='top' width='724'>
              
                <p class='MsoNormal'></br>
                    <span style='font-size:16.5pt;font-family:宋体;color:black;'>总价:" + totalValue + "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span></span></p></td></tr></tbody></table>");




                        #endregion

                        content = content.Replace("$contractNo", context.Request.Params["contractNo"])
                                .Replace("$date", context.Request.Params["signedtime"]).Replace("$buyer", context.Request.Params["buyer"])
                                .Replace("$BuyerAddress", context.Request.Params["buyeraddress"]).Replace("$seller", seller)
                                .Replace("$SellerAddress", sellerAddress).Replace("$productTable", sbProduct.ToString())
                                .Replace("$validity", context.Request.Params["validity"]);
                        //插入境内模板
                        StringBuilder sbEntry = new StringBuilder();
                        sbEntry.Append(@"update Icontract_at set content=@content,createman=@createman,createdate=@createdate,lastmod=@lastmod,lastmoddate=@lastmoddate
where contractNo=@contractNo and templateno=@templateno and language=@language and sortno=@sortno and attachmentno=@attachmentno");

                        SqlParameter[] pms = new SqlParameter[]{
                        new SqlParameter("@contractNo",context.Request.Params["contractNo"]),
                          new SqlParameter("@attachmentno",context.Request.Params["attachmentno"]),
                        new SqlParameter("@templateno",context.Request.Params["templateno"]),
                        new SqlParameter("@language","中文"),
                        new SqlParameter("@sortno",sortno),
                        new SqlParameter("@content",content),
                        new SqlParameter("@createman",context.Request.Params["createman"]),
                        new SqlParameter("@createdate",context.Request.Params["createdate"]),
                        new SqlParameter("@lastmod",context.Request.Params["lastmod"]),
                        new SqlParameter("@lastmoddate",context.Request.Params["lastmoddate"]),
                     
                    };
                        bll.ExecuteNonQuery(sbEntry.ToString(), pms);
                        #endregion
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
            #region 更新产品
            //            string str = context.Request.Params["htcplistStr"];
            //            List<Hashtable> listtable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(str);
            //            StringBuilder sb2 = new StringBuilder();
            //            StringBuilder sb3 = new StringBuilder();
            //            sb2.Append(@" insert into Icontract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values (
            //            @contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace)");
            //            sb3.Append(@" update Icontract_ap set  pname=@pname,quantity=@quantity,qunit=@qunit,price=@price,amount=@amount,packspec=@packspec,
            //packing=@packing,pallet=@pallet,ifcheck=@ifcheck,ifplace=@ifplace where contractNo=@contractNo and attachmentno=@attachmentno and pcode=@pcode;
            //");

            //            //update 和 insert 产品表
            //            string oldpcodes = "";
            //            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            //            {
            //                SqlParameter[] ss2 = new SqlParameter[] { 
            //                    new SqlParameter{ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30}
            //                };
            //                DataTable dt = bll.ExecDatasetSql(" select pcode from Icontract_ap where contractNo=@contractNo and attachmentno='' ", ss2).Tables[0];
            //                foreach (DataRow dr in dt.Rows)
            //                {
            //                    oldpcodes = oldpcodes + "#" + dr["pcode"].ToString();
            //                }
            //            }
            //            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            //            {
            //                try
            //                {
            //                    bll.SqlTran = bll.SqlCon.BeginTransaction();

            //                    bll.ExecuteNonQuery(strsql, mms);
            //                    foreach (var a in listtable)
            //                    {
            //                        SqlParameter[] mms2 = new SqlParameter[]
            //                        {
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=a["contractNo"],Size=30},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=a["attachmentno"],Size=30},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pcode",Value=a["pcode"],Size=60},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pname",Value=a["pname"],Size=200},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@quantity",Value=a["quantity"],Size=9,Precision=18   ,Scale=2    },
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@qunit",Value=a["qunit"],Size=20},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@price",Value=a["price"],Size=9,Precision=18   ,Scale=2    },
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@amount",Value=a["amount"],Size=9,Precision=18   ,Scale=2    },
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packspec",Value=a["packspec"],Size=200},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packing",Value=a["packing"],Size=200},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pallet",Value=a["pallet"],Size=200},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifcheck",Value=a["ifcheck"],Size=20},
            //                            new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifplace",Value=a["ifplace"],Size=20},
            //                        };
            //                        if (oldpcodes.Contains(a["pcode"].ToString()))
            //                        {
            //                            bll.ExecuteNonQuery(sb3.ToString(), mms2);
            //                        }
            //                        else
            //                        {
            //                            bll.ExecuteNonQuery(sb2.ToString(), mms2);
            //                        }
            //                    }
            //                    bll.SqlTran.Commit();
            //                    return true;
            //                }
            //                catch (Exception ex)
            //                {
            //                    bll.SqlTran.Rollback();
            //                    errorinfo = ex.Message;
            //                    return false;
            //                }
            //            } 
            #endregion
        }

        private bool addContract_ap(ref string errorinfo, HttpContext context)
        {
            string strsql = @"
  insert into Econtract_ap(contractNo,attachmentno,pcode,pname,quantity,qunit,price,amount,packspec,packing,pallet,ifcheck,ifplace) values(@contractNo,@attachmentno,@pcode,@pname,@quantity,@qunit,@price,@amount,@packspec,@packing,@pallet,@ifcheck,@ifplace);";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pcode",Value=context.Request.Params["pcode"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pname",Value=context.Request.Params["pname"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@quantity",Value=context.Request.Params["quantity"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@qunit",Value=context.Request.Params["qunit"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@price",Value=context.Request.Params["price"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@amount",Value=context.Request.Params["amount"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packspec",Value=context.Request.Params["packspec"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packing",Value=context.Request.Params["packing"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pallet",Value=context.Request.Params["pallet"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifcheck",Value=context.Request.Params["ifcheck"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifplace",Value=context.Request.Params["ifplace"],Size=20},

};


            try
            {
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    bll.ExecuteNonQuery(strsql, mms);
                }
                return true;
            }
            catch (Exception ex)
            {
                errorinfo = ex.Message;
                return false;
            }
        }

        private bool editContract_ap(ref string errorinfo, HttpContext context)
        {
            string strsql = @"
   update Econtract_ap set pname=@pname,quantity=@quantity,qunit=@qunit,price=@price,amount=@amount,packspec=@packspec,packing=@packing,pallet=@pallet,ifcheck=@ifcheck,ifplace=@ifplace where contractNo=@contractNo and attachmentno=@attachmentno and pcode=@pcode";

            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentno",Value=context.Request.Params["attachmentno"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pcode",Value=context.Request.Params["pcode"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pname",Value=context.Request.Params["pname"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@quantity",Value=context.Request.Params["quantity"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@qunit",Value=context.Request.Params["qunit"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@price",Value=context.Request.Params["price"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@amount",Value=context.Request.Params["amount"],Size=9,Precision=18   ,Scale=2    },
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packspec",Value=context.Request.Params["packspec"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packing",Value=context.Request.Params["packing"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pallet",Value=context.Request.Params["pallet"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifcheck",Value=context.Request.Params["ifcheck"],Size=20},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@ifplace",Value=context.Request.Params["ifplace"],Size=20},

};


            try
            {
                using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                {
                    bll.ExecuteNonQuery(strsql, mms);
                }
                return true;
            }
            catch (Exception ex)
            {
                errorinfo = ex.Message;
                return false;
            }
        }
        private bool deleteProduct(ref string errorinfo, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder(" delete from Icontract_ap  where contractNo=@contractNo;");

            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {

                    bll.ExecuteNonQuery(strsql.ToString(), mms);

                }
                catch (Exception ex)
                {

                    errorinfo = ex.Message;
                    return false;
                }
            }
            return true;
        }
        private bool deleteContract(ref string errorinfo, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder(@" delete from Icontract  where contractNo=@contractNo;delete from Icontract_t where contractNo=@contractNo
delete from Icontract_ap where contractNo=@contractNo ");

            SqlParameter[] mms = new SqlParameter[] { 
                new SqlParameter{ ParameterName="@contractNo",Value=context.Request.Params["contractNo"] }
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {

                    bll.ExecuteNonQuery(strsql.ToString(), mms);

                }
                catch (Exception ex)
                {

                    errorinfo = ex.Message;
                    return false;
                }
            }
            return true;
        }
        private bool deleteContractfj(ref string errorinfo, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder("");
            strsql.Append("  delete Icontract_a where contractNo=@contractNo and attachmentno=@attachmentno; ");
            //strsql.Append("  delete Econtract_ap where contractNo=@contractNo and attachmentno=@attachmentno; ");
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

        private bool modifyContractFStatus(ref string errorinfo, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder("");
            strsql.Append("  update  Icontract_a set status=@status where contractNo=@contractNo and attachmentno=@attachmentno; ");
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

        private bool modifyContractStatus(ref string errorinfo, HttpContext context)
        {
            StringBuilder strsql = new StringBuilder("");
            strsql.Append("  update  Icontract set status=@status where contractNo=@contractNo ; ");
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
                    errorinfo = ex.Message;
                    return false;
                }
            }
            return true;
        }
    }
}