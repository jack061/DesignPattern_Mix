using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using WZX.Busines.DAL;
using WZX.Busines;
using RM.Busines;

namespace Survey.ashx
{
    /// <summary>
    /// Authority 的摘要说明
    /// </summary>
    public class Authority : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.QueryString["type"] == "save")
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                string Autid = context.Request.QueryString["Autid"];
                Com_NavigationAndButton bll = new Com_NavigationAndButton();
                List<string> list = new List<string>();
                list.Add("delete from Com_NavigationAndButton where NavigationId="+Id);
                string[] str = Autid.Split(',');
                if(str.Length>0 && str[0] != "")
                {
                    foreach (string BtnId in str)
                    {
                        list.Add("insert into Com_NavigationAndButton(NavigationId,ButtonId)values(" + Id + "," + BtnId + ")");
                    }
                }
                
                DataFactory.SqlDataBaseExpand().ExecuteSqlTran(list);
            }
            else if (context.Request.QueryString["type"] == "auth") {
                string NavigaId = "0";
                if (context.Request["NavigaId"] != null)
                {
                    NavigaId = context.Request["NavigaId"].ToString();
                }
                DataSet ds=DataFactory.SqlDataBaseExpand().Query("select [ButtonId] from Com_NavigationAndButton where  NavigationId=" + NavigaId);
                string str = "";
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    if (str != "")
                        str += ",";
                    str += dr["ButtonId"];
                }
                if (str.Length > 0)
                {
                    context.Response.Write(str);
                }
                else {
                    context.Response.Write("false");
                }
            }
            else
            {
               
                // Com_NavigationAndButton Nbll = new Com_NavigationAndButton();
                // DataSet ds = Nbll.GetList(" NavigationId=" + NavigaId);
                Com_ButtonGroup bll = new Com_ButtonGroup();
                List<WZX.Model.Com_ButtonGroup> list = bll.GetModelList(" 1=1 order by Sort");
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (WZX.Model.Com_ButtonGroup item in list)
                {
                    sb.Append("{\"Id\":" + item.Id + ",");
                    sb.Append("\"ButtonName\":\"" + item.ButtonName + "\",");
                    sb.Append("\"BtnCode\":\"" + item.BtnCode + "\",");
                    sb.Append("\"Icon\":\"" + item.Icon + "\",");
                    sb.Append("\"Sort\":\"" + item.Sort + "\",");
                    sb.Append("\"Remark\":\"" + item.Remark + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                context.Response.Write(sb.ToString());
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