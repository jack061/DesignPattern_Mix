using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace RM.Web.ashx.CheckNotice
{
    /// <summary>
    /// CheckNoticeOperator 的摘要说明
    /// </summary>
    public class CheckNoticeOperator : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request.QueryString["module"];

            if (module == "delCheckNotice")
            {
                string err = "";
                bool suc = delCheckNotice(ref err, context);
                if (suc == true)
                {
                    context.Response.Write("ok");
                }
                else
                {
                    context.Response.Write(err);
                }

            }
            else if (module == "addCheckNotice")
            {
                //保存主表内容
                string err = "";
          
                bool suc = addCheckNotice(ref err, context);

                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "ok";
                }
                else
                {
                    r = err;
                }
                context.Response.Write(r);
            }
            else if (module == "editCheckNotice")
            {
                string err = "";
                bool suc = editCheckNotice(ref err, context);
                //返回json
                string r = "";
                if (suc == true)
                {
                    r = "ok";
                }
                else
                {
                    r = err;
                }
                context.Response.Write(r);
            }
         
            else
            {
                context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
            }
        }

        private bool editCheckNotice(ref string err, HttpContext context)
        {
            string checkId = context.Request.Params["checkId"];

            string strsql = @" update checkoutNotice set saleContract=@saleContract,purchaseContract=@purchaseContract,busniessGroup=@busniessGroup,
buyer=@buyer,seller=@seller,isConsignor=@isConsignor,isConsignee=@isConsignee,Consignor=@Consignor,	Consignee=@Consignee,	destinationPort=@destinationPort,
billNumbers=@billNumbers,shippingdate=@shippingdate,shipname=@shipname,voyages=@voyages,carrier=@carrier,consign=@consign,	wharf=@wharf,
isDT=@isDT,isTM=@isTM,isCM=@isCM,isTZMD=@isTZMD,isTDD=@isTDD,costbear=@costbear,containerCount=@containerCount  where checkId=@checkId ;";



            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@busniessGroup",Value=context.Request.Params["businessGroup"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@saleContract",Value=context.Request.Params["saleContract"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@purchaseContract",Value=context.Request.Params["purchaseContract"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isConsignor",Value=context.Request.Params["isConsignor"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isConsignee",Value=context.Request.Params["isConsignee"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@Consignor",Value=context.Request.Params["Consignor"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@Consignee",Value=context.Request.Params["Consignee"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@destinationPort",Value=context.Request.Params["destinationPort"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@billNumbers",Value=context.Request.Params["billNumbers"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shippingdate",Value=context.Request.Params["shippingdate"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipname",Value=context.Request.Params["shipname"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@voyages",Value=context.Request.Params["voyages"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@carrier",Value=context.Request.Params["carrier"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@consign",Value=context.Request.Params["consign"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@wharf",Value=context.Request.Params["wharf"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isDT",Value=context.Request.Params["isDT"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isTM",Value=context.Request.Params["isTM"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isCM",Value=context.Request.Params["isCM"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isTZMD",Value=context.Request.Params["isTZMD"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isTDD",Value=context.Request.Params["isTDD"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@costbear",Value=context.Request.Params["costbear"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@containerCount",Value=context.Request.Params["containerCount"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=checkId,Size=30},
};

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(strsql, mms);
                  

                    #region 产品添加
                    if (!string.IsNullOrEmpty(context.Request.Params["productName"]))
                    {
                        //付费代码添加
                        //productName, packages, netWeight, fullWeight, mass, checkId
                        string proSql = @" update checkProduct set productName=@productName, packages=@packages, 
netWeight=@netWeight, fullWeight=@fullWeight, mass=@mass  where checkId=@checkId;";
                        string[] productName = context.Request.Form.GetValues("productName");
                        string[] packages = context.Request.Form.GetValues("packages");
                        string[] netWeight = context.Request.Form.GetValues("netWeight");
                        string[] fullWeight = context.Request.Form.GetValues("fullWeight");
                        string[] mass = context.Request.Form.GetValues("mass");

                      
                        for (int i = 0; i < productName.Length; i++)
                        {


                            SqlParameter[] proPms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@productName",Value=productName[i],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packages",Value=packages[i],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@netWeight",Value=netWeight[i],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@fullWeight",Value=fullWeight[i],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@mass",Value=mass[i],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=checkId,Size=8},
};
                            bll.ExecuteNonQuery(proSql, proPms);
                        }


                    #endregion



                    }

                    bll.SqlTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;
                }

            }
        }

        private bool addCheckNotice(ref string err, HttpContext context)
        {
            //添加sql,获取自增列的最后添加的id

            string strsql = @" insert into checkoutNotice(busniessGroup, saleContract, purchaseContract, buyer, seller, isConsignor,
isConsignee, Consignor,Consignee, destinationPort, billNumbers, shippingdate, shipname, voyages, carrier, consign,
wharf, isDT, isTM, isCM, isTZMD, isTDD, costbear, containerCount)
values(@businessGroup, @saleContract, @purchaseContract, @buyer, @seller, @isConsignor,
@isConsignee, @Consignor,@Consignee,@destinationPort, @billNumbers, @shippingdate,
@shipname, @voyages, @carrier, @consign,@wharf, @isDT,@isTM,@isCM,@isTZMD, @isTDD,@costbear, @containerCount);select @@IDENTITY";


            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@businessGroup",Value=context.Request.Params["businessGroup"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@saleContract",Value=context.Request.Params["saleContract"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@purchaseContract",Value=context.Request.Params["purchaseContract"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@buyer",Value=context.Request.Params["buyer"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@seller",Value=context.Request.Params["seller"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isConsignor",Value=context.Request.Params["isConsignor"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isConsignee",Value=context.Request.Params["isConsignee"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@Consignor",Value=context.Request.Params["Consignor"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@Consignee",Value=context.Request.Params["Consignee"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@destinationPort",Value=context.Request.Params["destinationPort"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@billNumbers",Value=context.Request.Params["billNumbers"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shippingdate",Value=context.Request.Params["shippingdate"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@shipname",Value=context.Request.Params["shipname"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@voyages",Value=context.Request.Params["voyages"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@carrier",Value=context.Request.Params["carrier"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@consign",Value=context.Request.Params["consign"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@wharf",Value=context.Request.Params["wharf"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isDT",Value=context.Request.Params["isDT"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isTM",Value=context.Request.Params["isTM"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isCM",Value=context.Request.Params["isCM"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isTZMD",Value=context.Request.Params["isTZMD"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@isTDD",Value=context.Request.Params["isTDD"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@costbear",Value=context.Request.Params["costbear"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@containerCount",Value=context.Request.Params["containerCount"],Size=30},

};

            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    //获取铁路运输表自增列的id
                    int id = int.Parse(bll.ExecuteScalar(strsql, mms).ToString());

                    //productName, packages, netWeight, fullWeight, mass, checkId

                    #region 产品添加
                    string shipProduct = context.Request.Params["shipProduct"];
                    List<Hashtable> shipProductTable = RM.Common.DotNetJson.JsonHelper.DeserializeJsonToList<Hashtable>(shipProduct);

                    string proSql = @" insert into checkProduct(productName, packages, 
netWeight, fullWeight,mass, checkId)
values(@productName, @packages, @netWeight, @fullWeight,@mass, @checkId);";
                    foreach (var item in shipProductTable)
                    {
                        SqlParameter[] frontierPms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@productName",Value=item["productName"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packages",Value=item["packages"],Size=8},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@netWeight",Value=item["netWeight"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@fullWeight",Value=item["fullWeight"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@mass",Value=item["mass"],Size=1000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=id,Size=8},
};
                        bll.ExecuteNonQuery(proSql, frontierPms);
                    }
                    
                    #endregion

                    #region 产品添加 old
//                    if (!string.IsNullOrEmpty(context.Request.Params["productName"]))
//                    {
//                        string proSql = @" insert into checkProduct(productName, packages, 
//netWeight, fullWeight,mass, checkId)
//values(@productName, @packages, @netWeight, @fullWeight,@mass, @checkId);";
//                        string[] productName = context.Request.Form.GetValues("productName");
//                        string[] packages = context.Request.Form.GetValues("packages");
//                        string[] netWeight = context.Request.Form.GetValues("netWeight");
//                        string[] fullWeight = context.Request.Form.GetValues("fullWeight");
//                        string[] mass = context.Request.Form.GetValues("mass");
                    

//                              for (int i = 0; i < productName.Length; i++)
//                        {


//                            SqlParameter[] proPms = new SqlParameter[]
//{
//new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@productName",Value=productName[i],Size=1000},
//new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@packages",Value=packages[i],Size=8},
//new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@netWeight",Value=netWeight[i],Size=1000},
//new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@fullWeight",Value=fullWeight[i],Size=1000},
//new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@mass",Value=mass[i],Size=1000},
//new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@checkId",Value=id,Size=8},
//};
//                            bll.ExecuteNonQuery(proSql, proPms);
//                        }

//                    }
                    #endregion

                    bll.SqlTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    bll.SqlTran.Rollback();
                    err = ex.Message;
                    return false;
                }

            }
        }

        private bool delCheckNotice(ref string err, HttpContext context)
        {
            string checkId = context.Request.Params["checkId"];
            string reportSql = "delete from checkoutNotice where checkId=@checkId";
            string productSql = "delete from checkProduct where checkId=@checkId";
         
            SqlParameter[] pms = new SqlParameter[]{
                new SqlParameter("@checkId",checkId)
            };
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                try
                {
                    bll.SqlTran = bll.SqlCon.BeginTransaction();
                    bll.ExecuteNonQuery(productSql, pms);
             
                    bll.ExecuteNonQuery(reportSql, pms);

                    bll.SqlTran.Commit();
                    return true;

                }
                catch (Exception ex)
                {

                    err = ex.Message;
                    return false;
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