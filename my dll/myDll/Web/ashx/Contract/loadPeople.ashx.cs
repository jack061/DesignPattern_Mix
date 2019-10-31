using RM.Common.DotNetJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RM.Web.ashx.Contract
{
    /// <summary>
    /// loadPeople 的摘要说明
    /// </summary>
    public class loadPeople : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string module = context.Request.Params["module"];
            //if (module=="seller")
            //{
            string seller = context.Request.Params["simpleSeller"];
            if (!string.IsNullOrEmpty(seller))
            {
                //加载卖方全称
                context.Response.Write(loadSeller(seller));
            }
            //}
            //if (module=="buyer")
            {
                string buyer = context.Request.Params["simpleBuyer"];

                if (!string.IsNullOrEmpty(buyer))
                {
                    context.Response.Write(loadBuyer(buyer));
                }
                //}

            }
        }

        private string loadSeller(string seller)
        {
               using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
               {
                   SqlParameter[]pms=new SqlParameter[]{
                       new SqlParameter("@seller",seller)
                   };
                   DataRow dr = bll.ExecDatasetSql(@"select shortname, code, name,address from  bsupplier t2 where t2.shortname like '%'+@seller+'%'", pms).Tables[0].Rows[0];
                 string totalSeller= JsonHelper.DataRowToJson_(dr);
                   return totalSeller.ToString();
               }
        }
        private string loadBuyer(string buyer)
        {
            using (WZX.BLL.SqlCommandExBll bll = new WZX.BLL.SqlCommandExBll())
            {
                SqlParameter[] pms = new SqlParameter[]{
                       new SqlParameter("@buyer",buyer)
                   };
                DataRow dr = bll.ExecDatasetSql(@"select shortname,code, name,address from  bcustomer t2 where t2.shortname like '%'+@buyer+'%'", pms).Tables[0].Rows[0];
                string totalBuyer = JsonHelper.DataRowToJson_(dr);
                return totalBuyer.ToString();
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