using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using WZX.Busines.DAL;

namespace Survey.ashx
{
    /// <summary>
    /// RolesListHandler 的摘要说明
    /// </summary>
    public class RolesListHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Tb_Roles bll = new Tb_Roles();
            DataSet ds = bll.GetAllList();
            string str = ToJson(ds);
            context.Response.Write(str);
        }
        public static string ToJson(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            StringBuilder jsonString = new StringBuilder();
            //
            //TODO:total表示记录的总数
            //
            jsonString.Append("{\"total\":150,\"rows\":[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = String.Format(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append("\"" + strValue + "\",");
                    }
                    else
                    {
                        jsonString.Append("\"" + strValue + "\"");
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]}");
            return jsonString.ToString();
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