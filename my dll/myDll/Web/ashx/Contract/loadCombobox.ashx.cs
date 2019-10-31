using RM.Busines;
using RM.Common.DotNetCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// loadCombobox 的摘要说明
    /// </summary>
    public class loadCombobox : IHttpHandler
    {
        RM.Busines.JsonHelperEasyUi jsonh = new Busines.JsonHelperEasyUi();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request["module"];
            StringBuilder suc = new StringBuilder();

            switch (module)
            {

                case "product"://获取产品大类

                    suc = product(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "shipment"://获取发运条款

                    suc = shipment(context);
                    context.Response.Write(suc.ToString());
                    break;

                case "transport"://获取运输方式

                    suc = transport(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "currency"://获取货币

                    suc = currency(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "tradement"://获取贸易条款

                    suc = tradement(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "validity"://获取有效期

                    suc = validity(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "pricement1"://获取价格条款1

                    suc = pricement1(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "pricement2"://获取价格条款2

                    suc = pricement2(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "seller"://获取供应商
                    suc = seller(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "buyer"://获取客户

                    suc = buyer(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "exportTemplate"://获取出口模板

                    suc = exportTemplate(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "importTemplate"://获取出口模板

                    suc = importTemplate(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "logisticsTemplate"://获取出口模板

                    suc = logisticsTemplate(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "harborout"://获取

                    suc = harborout(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "harborarrive"://获取

                    suc = harborarrive(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "harborclear"://获取

                    suc = harborclear(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "businessclass"://获取

                    suc = businessclass(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "containerSize"://获取集装箱规格

                    suc = containerSize(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "fromStation"://获取发站

                    suc = fromStation(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "destination"://获取到站

                    suc = destination(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "adminReview"://获取合同审核人

                    suc = adminReview(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "salesmanCode"://获取业务员编号

                    suc = salesmanCode(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "simpleseller"://获取卖方简称

                    suc = simpleseller(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "simplebuyer"://获取买方简称

                    suc = simplebuyer(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "customeDelivery"://客户收货人货人
                    suc = customeDelivery(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "buspplierDelivery"://发货人
                    suc = buspplierDelivery(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "customeNotice"://客户通知人
                    suc = customeNotice(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "previewCode"://根据发货人获取预验单号
                    suc = previewCode(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "facpreviewCode"://根据发货人获取预验单号
                    suc = facpreviewCode(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "getPaymentType"://获取付款方式
                    suc = getPaymentType(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "getBankAccount"://根据所选银行加载收款银行账户
                    suc = getBankAccount(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "palletRequire"://获取托盘要求
                    suc = palletRequire(context);
                    context.Response.Write(suc.ToString());
                    break;
                case "internalCompany"://获取内部公司
                    suc = internalCompany(context);
                    context.Response.Write(suc.ToString());
                    break;
                default://默认
                    context.Response.Write("{\"sucess\": 0,\"warnmsg\": \"\",\"errormsg\": \"未找到所请求的服务\"}");
                    break;
            }
        }

        #region 获取内部公司
        private StringBuilder internalCompany(HttpContext context)
        {
            StringBuilder sb = new StringBuilder(@"select code,shortname,name from bcustomer where len(code)<5");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        
        #endregion
        #region 托盘要求
        private StringBuilder palletRequire(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select * from BASE_DICTIONARY where PARENTID='231';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        } 
        #endregion

        #region 根据所选银行加载收款银行账户
     
        private StringBuilder getBankAccount(HttpContext context)
        {
            string bankName = context.Request.Params["bankName"];
            StringBuilder sb = new StringBuilder(" select * from BASE_DICTIONARY where ENGLISH=@bankName;");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[1] { new SqlParam("@bankName", bankName) }, 0);
            return jsonh.ToEasyUIComboxJson(dt);

        } 
        #endregion

        //获取付款方式
        private StringBuilder getPaymentType(HttpContext context)
        {
            StringBuilder sb = new StringBuilder(" select code,name as cname from BASE_DICTIONARY where PARENTID='25';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
   

        private StringBuilder previewCode(HttpContext context)
        {
            string man = context.Request.Params["man"];
            string transport = context.Request.Params["transport"];
            //StringBuilder sb = new StringBuilder("select * from previewManage where deliveryMan=@man; ");
            StringBuilder sb = new StringBuilder(@"select * from View_Inspection a1 where a1.deliveryMan =@sendMan and a1.transport=@transport");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[2] { new SqlParam("@sendMan", man),new SqlParam("@transport",transport) }, 0);
            return jsonh.ToEasyUIDataGridJson(dt);
        }

        private StringBuilder facpreviewCode(HttpContext context)
        {
            string man = context.Request.Params["man"];
            //StringBuilder sb = new StringBuilder("select * from previewManage where deliveryMan=@man; ");
            StringBuilder sb = new StringBuilder(@"select t1.quantity as advanceOccupy from Econtract_Inspect_ap t1,Econtract_Inspect t2,previewManage t3 where t1.inspectContractNo=
 t2.contractNo and t2.facpreviewCode=t3.previewCode and t3.HSCode=(select hssCode from bproduct t4 where t4.pcode=t1.pcode) 
 and t2.sendMan='@sendMan'");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, new SqlParam[1] { new SqlParam("@sendMan", man) }, 0);
            return jsonh.ToEasyUIDataGridJson(dt);
        }

        private StringBuilder simpleseller(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select * from bsupplier where status=1; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder simplebuyer(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select * from bcustomer where status=1; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        //发货人
        private StringBuilder buspplierDelivery(HttpContext context)
        {
            StringBuilder sqlWhere = new StringBuilder();
            string customername = context.Request.Params["customername"] ?? "";
            if (customername != "")
            {
                sqlWhere.Append(" and customername='" + customername + "'");
            }
            StringBuilder sb = new StringBuilder("select * from (select a.shortname,a.name as customername, b.* from bsupplier a,bsupplier_delivery b  where status=1 and a.code=b.code) t where 1=1  ");
            sb.Append(sqlWhere);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        //客户收货人
        private StringBuilder customeDelivery(HttpContext context)
        {
            StringBuilder sqlWhere = new StringBuilder();
            string customername = context.Request.Params["customername"] ?? "";
            if(customername != "")
            {
                sqlWhere.Append(" and customername='" + customername + "'");
            }
            StringBuilder sb = new StringBuilder("select * from (select a.shortname,a.name as customername, b.* from bcustomer a,bcustomer_delivery b  where status=1 and a.code=b.code) t where 1=1  ");
            sb.Append(sqlWhere);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        //客户通知人
        private StringBuilder customeNotice(HttpContext context)
        {
            StringBuilder sqlWhere = new StringBuilder();
            string customername = context.Request.Params["customername"] ?? "";
            if (customername != "")
            {
                sqlWhere.Append(" and customername='" + customername + "'");
            }
            StringBuilder sb = new StringBuilder("select * from (select a.shortname,a.name as customername,b.* from bcustomer a,bcustomer_notice b  where status=1 and a.code=b.code) t where 1=1 ");
            sb.Append(sqlWhere);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        //获取业务员编号
        private StringBuilder salesmanCode(HttpContext context)
        {
            StringBuilder sb = new StringBuilder(@"select t3.UserRealName as cname,t4.LoginName as code from Tb_RolesAddUser t1 left join Tb_Roles t2 
 on t1.rolesId=t2.Id left join Com_UserInfos t3 on t3.Userid=t1.UserId left join Com_UserLogin t4 on t4.UserId=t1.UserId  where t2.RolesName='合同管理员';");
          DataTable dt=  DataFactory.SqlDataBase().GetDataTableBySQL(sb,0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        //获取合同审核人
        private StringBuilder adminReview(HttpContext context)
        {
            StringBuilder sb = new StringBuilder(@"select t3.UserRealName as cname,t4.LoginName as code from Tb_RolesAddUser t1 left join Tb_Roles t2 
 on t1.rolesId=t2.Id left join Com_UserInfos t3 on t3.Userid=t1.UserId left join Com_UserLogin t4 on t4.UserId=t1.UserId  where t2.RolesName='合同管理员';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        //获取到站
        private StringBuilder destination(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select code,name as cname from BASE_DICTIONARY where PARENTID='102';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        //获取发站
        private StringBuilder fromStation(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select code,name as cname from BASE_DICTIONARY where PARENTID='101';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        //获取集装箱规格
        private StringBuilder containerSize(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select code,name as cname from BASE_DICTIONARY where PARENTID='103';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder businessclass(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select code,name as cname from BASE_DICTIONARY where PARENTID='18';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder harborclear(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select code,country,name,egname,runame from bharbor order by country,name; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder harborarrive(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select code,country,name,egname,runame from bharbor order by country,name; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder harborout(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = new DataTable();
            string country = context.Request.Params["country"];
            if (string.IsNullOrEmpty(country))
            {
                sb = new StringBuilder(" select code,country,name,egname,runame from bharbor order by country,name; ");
                dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            }
            else
            {
                sb = new StringBuilder(" select code,country,name,egname,runame from bharbor where country=@country order by country,name; ");
                dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb,new SqlParam[1]{new SqlParam("@country",country)}, 0);
            }
            
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder exportTemplate(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select distinct templatename,templateno  from btemplate_exportEcontract  ;");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        private StringBuilder importTemplate(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select distinct templatename,templateno  from btemplate_importEcontract  ;");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }
        private StringBuilder logisticsTemplate(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select distinct logisticsTemplateName as templatenname,logisticsTemplateno as templateno from btemplate_logistics  ;");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder buyer(HttpContext context)
        {

            StringBuilder sb = new StringBuilder("select * from bcustomer where status=1; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            //return jsonh.ToEasyUIDataGridJson(dt);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder seller(HttpContext context)
        {

            StringBuilder sb = new StringBuilder("select * from bsupplier where status=1; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIDataGridJson(dt);
        }

        private StringBuilder pricement2(HttpContext context)
        {

            StringBuilder sb = new StringBuilder("select code,name as cname from BASE_DICTIONARY where PARENTID='64';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder pricement1(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select code,name as cname from BASE_DICTIONARY where PARENTID='64'; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder validity(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select code,name as cname from BASE_DICTIONARY where PARENTID='84';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder tradement(HttpContext context)
        {

            StringBuilder sb = new StringBuilder("select code,name as cname from BASE_DICTIONARY where PARENTID='114';");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder currency(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select code,name as cname from BASE_DICTIONARY where PARENTID='22';  ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder transport(HttpContext context)
        {

            StringBuilder sb = new StringBuilder(" select code,name as cname from BASE_DICTIONARY where PARENTID='14'; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder shipment(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select code,name as cname from BASE_DICTIONARY where PARENTID='26'; ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
        }

        private StringBuilder product(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("select distinct productcategory from bproduct;");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            return jsonh.ToEasyUIComboxJson(dt);
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