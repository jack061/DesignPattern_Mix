using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WZX.Busines.Util;

namespace RM.Web.ashx.CostManager
{
    /// <summary>
    /// costChange 的摘要说明
    /// </summary>
    public class costChange : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request["module"];
            switch (module)
            {
                case "addManager":

                    context.Response.Write(addManager(context));
                    break;
                case "deletecontract":
                    context.Response.Write(deleteManager(context));
                    break;
                case "editManager":
                    context.Response.Write(editManager(context));
                    break;

            }
        }
        //删除
        private bool deleteManager(HttpContext context)
        {
            string contractNo = context.Request.Params["contractNo"];
          
                string sql = "update  costManagerment set status=0 where contractNo=@contractNo";
               SqlParameter[] mms = new SqlParameter[]
               {
                   new SqlParameter("@contractNo",contractNo),
               };
                 using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
                 {
                   int r=  bll.ExecuteNonQuery(sql, mms);
                   if (r>0)
                   {
                       return true;
                   }
                   else
                   {
                       return false;
                   }
                 }
            
        }
        //添加
        private bool addManager(HttpContext context)
        {
            string strsql = @"insert into costManagerment(contractNo, attachmentNo, department, pruseApplication,
amountName, contractAmount, amountPaid, foreignAmount, amount, payingBank, account, bankCode, bankAddress, 
payment,status,applicationDate,applicant,departmentCharge,departmentLeader, financialManager, financialLeader, chairMan)
values(@contractNo,@attachmentNo,@department,@pruseApplication,@amountName,@contractAmount,@amountPaid,@foreignAmount,@amount,@payingBank,@account,
@bankCode,@bankAddress,@payment,@status,@applicationDate,@applicant,@departmentCharge,@departmentLeader,@financialManager,@financialLeader,@chairMan);";

            #region mms参数
            SqlParameter[] mms = new SqlParameter[]
{
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@contractNo",Value=context.Request.Params["contractNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@attachmentNo",Value=context.Request.Params["attachmentNo"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@department",Value=context.Request.Params["department"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@pruseApplication",Value=context.Request.Params["pruseApplication"],Size=30},

new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@amountName",Value=context.Request.Params["amountName"],Size=1000},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@contractAmount",Value=context.Request.Params["contractAmount"],Size=8},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@amountPaid",Value=context.Request.Params["amountPaid"],Size=200},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@foreignAmount",Value=context.Request.Params["foreignAmount"],Size=1000},
new SqlParameter{ DbType=DbType.Decimal,IsNullable=true,ParameterName="@amount",Value=context.Request.Params["amount"],Size=2000},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@payingBank",Value=context.Request.Params["payingBank"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@account",Value=context.Request.Params["account"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@bankCode",Value=context.Request.Params["bankCode"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@bankAddress",Value=context.Request.Params["bankAddress"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@payment",Value=context.Request.Params["payment"],Size=30},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@status",Value="1",Size=30},
new SqlParameter{ DbType=DbType.DateTime,IsNullable=true,ParameterName="@applicationDate",Value=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@applicant",Value=context.Request.Params["applicant"],Size=300},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@departmentCharge",Value=context.Request.Params["departmentCharge"],Size=60},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@departmentLeader",Value=context.Request.Params["departmentLeader"],Size=200},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@financialManager",Value=context.Request.Params["financialManager"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@financialLeader",Value=context.Request.Params["financialLeader"],Size=100},
new SqlParameter{ DbType=DbType.String,IsNullable=true,ParameterName="@chairMan",Value=context.Request.Params["chairMan"],Size=100},

};
            #endregion
            var svn = context.Request.Params["applicationDate"];
            bool issuc = false;
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                int r = bll.ExecuteNonQuery(strsql, mms);
                if (r > 0)
                {
                    issuc = true;
                    return issuc;
                }
                else
                {
                    return issuc;
                }


            }

        }
        //修改
        private bool editManager(HttpContext context)
        {
            Hashtable ht_result = new Hashtable();
            Hashtable ht = new Hashtable();
            //contractNo, attachmentNo, department, pruseApplication, amountName, contractAmount, amountPaid, 
            //foreignAmount, amount, payingBank, account, bankCode, bankAddress, payment, 
            //status, applicationDate, applicant, departmentCharge, departmentLeader, financialManager, financialLeader, chairMan
            ht["contractNo"] = string.IsNullOrEmpty(context.Request["contractNo"]) ? "" : context.Request["contractNo"].ToString();
            ht["attachmentNo"] = string.IsNullOrEmpty(context.Request["attachmentNo"]) ? "" : context.Request["attachmentNo"].ToString();
            ht["department"] = string.IsNullOrEmpty(context.Request["department"]) ? "" : context.Request["department"].ToString();
            //ht["pcode"] = string.IsNullOrEmpty(context.Request["PCODE"]) ? "" : context.Request["PCODE"].ToString();
            ht["pruseApplication"] = string.IsNullOrEmpty(context.Request["pruseApplication"]) ? "" : context.Request["pruseApplication"].ToString();
            ht["amountName"] = string.IsNullOrEmpty(context.Request["amountName"]) ? "" : context.Request["amountName"].ToString();
            ht["contractAmount"] = string.IsNullOrEmpty(context.Request["contractAmount"]) ? new Decimal(0.0) : Convert.ToDecimal(context.Request["contractAmount"].ToString());
            ht["amountPaid"] = string.IsNullOrEmpty(context.Request["amountPaid"]) ? "" : context.Request["amountPaid"].ToString();
            //ht["quantityA"] = string.IsNullOrEmpty(context.Request["QUANTITYA"]) ? "" : context.Request["QUANTITYA"].ToString();
            //ht["quantityB"] = string.IsNullOrEmpty(context.Request["QUANTITYB"]) ? "" : context.Request["QUANTITYB"].ToString();
            ht["foreignAmount"] = string.IsNullOrEmpty(context.Request["foreignAmount"]) ? "" : context.Request["foreignAmount"].ToString();
            ht["amount"] = string.IsNullOrEmpty(context.Request["amount"]) ? "" : context.Request["amount"].ToString();
            ht["payingBank"] = string.IsNullOrEmpty(context.Request["payingBank"]) ? "" : context.Request["payingBank"].ToString();
            ht["account"] = string.IsNullOrEmpty(context.Request["account"]) ? "" : context.Request["account"].ToString();
            ht["bankCode"] = string.IsNullOrEmpty(context.Request["bankCode"]) ? "" : context.Request["bankCode"].ToString();
            ht["payment"] = string.IsNullOrEmpty(context.Request["payment"]) ? "" : context.Request["payment"].ToString();
            ht["status"] = string.IsNullOrEmpty(context.Request["status"]) ? "" : context.Request["status"].ToString();
            ht["applicationDate"] = string.IsNullOrEmpty(context.Request["applicationDate"]) ? "" : context.Request["applicationDate"].ToString();
            ht["applicant"] = string.IsNullOrEmpty(context.Request["applicant"]) ? "" : context.Request["applicant"].ToString();
            ht["departmentCharge"] = string.IsNullOrEmpty(context.Request["departmentCharge"]) ? "" : context.Request["departmentCharge"].ToString();
            ht["departmentLeader"] = string.IsNullOrEmpty(context.Request["departmentLeader"]) ? "" : context.Request["departmentLeader"].ToString();
            ht["financialManager"] = string.IsNullOrEmpty(context.Request["financialManager"]) ? "" : context.Request["financialManager"].ToString();
            ht["financialLeader"] = string.IsNullOrEmpty(context.Request["financialLeader"]) ? "" : context.Request["financialLeader"].ToString();
            ht["chairMan"] = string.IsNullOrEmpty(context.Request["chairMan"]) ? "" : context.Request["chairMan"].ToString();


            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit_1(ConstantUtil.TABLE_COSTMANAGER, "contractNo", ht["contractNo"].ToString(), ht);
            if (IsOk)
            {
                return true;
                //ht_result.Add("status", "T");
                //ht_result.Add("msg", "操作成功！");
            }
            else
            {
                return false;
                //ht_result.Add("status", "F");
                //ht_result.Add("msg", "操作失败！");
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