using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Collections;
using RM.Common.DotNetData;
using System.Data.Common;
using Newtonsoft.Json;
using System.IO;

namespace RM.Common.DotNetJson
{
    /// <summary>
    /// 转换Json格式帮助类
    /// </summary>
    public class JsonHelper
    {


        #region 通过Newtonsoft.json处理json
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"xxx","Name":"xxxx","Age":"xxxx"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            List<T> list = new List<T>();
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
                list = o as List<T>;
            }
            catch (Exception ex)
            {

            }

            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
        #endregion
        /// <summary>
        /// 泛型接口转Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string IListToJson<T>(IList<T> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (T t in list)
            {
                sb.Append(ObjectToJson(t) + ",");
            }
            string _temp = sb.ToString().TrimEnd(',');
            _temp += "]";
            return _temp;
        }
        /// <summary>
        /// 泛型接口转Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="ClassName"></param>
        /// <returns>{"rows":[{"c_id":"21bb6911-af52-42a4-9732-24a6e8384411","eav_id":"4a9fe8ca-112a-47c0-b074-229837cfe6e6","e_id":"cfe929e3-accd-4efb-910b-07705077b6d6","ea_id":"","ea_name":"555","eav_value":"555","eav_memo":"","sud":"0"}]}</returns>
        public static string IListToJson<T>(IList<T> list, string ClassName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"" + ClassName + "\":[");
            foreach (T t in list)
            {
                sb.Append(ObjectToJson(t) + ",");
            }
            string _temp = sb.ToString().TrimEnd(',');
            _temp += "]}";
            return _temp;
        }
        /// <summary>
        /// 对象转Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns>{"SM_ID":"71","SM_Img":"title.gif","SM_Memo":"当前位置：会所服务 ─ 宾客管理 ─ 宾客列表"}</returns>
        public static string ObjectToJson<T>(T t)
        {
            StringBuilder sb = new StringBuilder();
            string json = "";
            if (t != null)
            {
                sb.Append("{");
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    sb.Append("\"" + pi.Name.ToString() + "\"");
                    sb.Append(":");
                    sb.Append("\"" + pi.GetValue(t, null) + "\"");
                    sb.Append(",");
                }
                json = sb.ToString().TrimEnd(',');
                json += "}";
            }
            return json;
        }
        /// <summary>
        /// 对象转Json（重载）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ClassName"></param>
        /// <returns>{"menu":[{"SM_ID":"71","SM_Img":"title.gif","SM_Memo":"当前位置：会所服务 ─ 宾客管理 ─ 宾客列表"}]}</returns>
        public static string ObjectToJson<T>(T t, string ClassName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"" + ClassName + "\":[");
            string json = "";
            if (t != null)
            {
                sb.Append("{");
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    sb.Append("\"" + pi.Name.ToString() + "\"");//.ToLower()
                    sb.Append(":");
                    sb.Append("\"" + pi.GetValue(t, null) + "\"");
                    sb.Append(",");
                }
                json = sb.ToString().TrimEnd(',');
                json += "}]}";
            }
            return json;
        }
        /// <summary>
        /// List转成json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonName"></param>
        /// <param name="IL"></param>
        /// <returns></returns>
        public static string ObjectToJson<T>(IList<T> IL, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    Type type = obj.GetType();
                    PropertyInfo[] pis = type.GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pis.Length; j++)
                    {
                        Json.Append("\"" + pis[j].Name.ToString() + "\":\"" + pis[j].GetValue(IL[i], null) + "\"");
                        if (j < pis.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < IL.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>
        /// 将数组转换为JSON格式的字符串
        /// </summary>
        /// <typeparam name="T">数据类型，如string,int ...</typeparam>
        /// <param name="list">泛型list</param>
        /// <param name="propertyname">JSON的类名</param>
        /// <returns></returns>
        public static string ArrayToJson<T>(List<T> list, string propertyname)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count > 0)
            {
                sb.Append("[{\"");
                sb.Append(propertyname);
                sb.Append("\":[");
                foreach (T t in list)
                {
                    sb.Append("\"");
                    sb.Append(t.ToString());
                    sb.Append("\",");
                }
                string _temp = sb.ToString();
                _temp = _temp.TrimEnd(',');
                _temp += "]}]";
                return _temp;
            }
            else
                return "";
        }

        /// <summary>
        /// DataTable转Json
        /// </summary>
        /// <param name="dt">table数据集</param>
        /// <param name="dtName">json名</param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt, string dtName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"");
            sb.Append(dtName);
            sb.Append("\":[");
            if (DataTableHelper.IsExistRows(dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        sb.Append("\"");
                        sb.Append(dc.ColumnName);
                        sb.Append("\":\"");
                        if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                            sb.Append(dr[dc]).Replace("\\", "/");
                        else
                            sb.Append("&nbsp;");
                        sb.Append("\",");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append("},");
                }
                sb = sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]}");
            return JsonCharFilter(sb.ToString());
        }
        /// <summary>
        /// DataTable转Json
        /// </summary>
        /// <param name="dt">table数据集</param>
        /// <param name="dtName">json名</param>
        /// <returns></returns>
        public static string DataTableToJson_(DataTable dt, string dtName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            sb.Append(dtName);
            sb.Append("\":[");
            if (DataTableHelper.IsExistRows(dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        sb.Append("\"");
                        sb.Append(dc.ColumnName);
                        sb.Append("\":\"");
                        if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                            sb.Append(dr[dc]).Replace("\\", "/");
                        else
                            sb.Append("&nbsp;");
                        sb.Append("\",");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append("},");
                }
                sb = sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            return JsonCharFilter(sb.ToString());
        }

        /// <summary>
        /// 数据行转Json
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <returns></returns>
        public static string DataRowToJson(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (DataColumn dc in dr.Table.Columns)
            {
                sb.Append("\"");
                sb.Append(dc.ColumnName);
                sb.Append("\":\"");
                if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                    sb.Append(StringFormat(dr[dc].ToString()));
                else
                    sb.Append("&nbsp;");
                sb.Append("\",");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("},");
            return sb.ToString();
        }

        /// <summary>
        /// 数据行转Json
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <returns></returns>
        public static string DataRowToJson_(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (DataColumn dc in dr.Table.Columns)
            {
                sb.Append("\"");
                sb.Append(dc.ColumnName);
                sb.Append("\":\"");
                if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                    sb.Append(StringFormat(dr[dc].ToString()));
                else
                    sb.Append("&nbsp;");
                sb.Append("\",");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
       
        /// <summary>
        /// 数组转Json
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string ArrayToJson(string[] strs)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strs.Length; i++)
            {
                sb.AppendFormat("'{0}':'{1}',", i + 1, strs[i]);
            }
            if (sb.Length > 0)
                return "{" + sb.ToString().TrimEnd(',') + "}";
            return "";
        }

        #region ListToJson
        /// <summary>
        /// list 转换json格式
        /// </summary>
        /// <param name="jsonName">类名</param>
        /// <param name="objlist">list集合</param>
        /// <returns></returns>
        public static string ListToJson<T>(List<T> objlist, string jsonName)
        {
            string result = "{";
            //如果没有给定类的名称， 指定一个
            if (jsonName.Equals(string.Empty))
            {
                object o = objlist[0];
                jsonName = o.GetType().ToString();
            }
            result += "\"" + jsonName + "\":[";
            //处理第一行前面不加","号
            bool firstline = true;
            foreach (object oo in objlist)
            {
                if (!firstline)
                {
                    result = result + "," + ObjectToJson(oo);
                }
                else
                {
                    result = result + ObjectToJson(oo) + "";
                    firstline = false;
                }
            }
            return result + "]}";
        }
        /// <summary>
        /// 单个对象转换json
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns></returns>
        private static string ObjectToJson(object o)
        {
            string result = "{";
            List<string> ls_propertys = new List<string>();
            ls_propertys = GetObjectProperty(o);
            foreach (string str_property in ls_propertys)
            {
                if (result.Equals("{"))
                {
                    result = result + str_property;
                }
                else
                {
                    result = result + "," + str_property + "";
                }
            }
            return result + "}";
        }
        /// <summary>
        /// 获取对象属性
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns></returns>
        private static List<string> GetObjectProperty(object o)
        {
            List<string> propertyslist = new List<string>();
            PropertyInfo[] propertys = o.GetType().GetProperties();
            foreach (PropertyInfo p in propertys)
            {
                propertyslist.Add("\"" + p.Name.ToString() + "\":\"" + p.GetValue(o, null) + "\"");

            }
            return propertyslist;
        }


        #endregion

        public static string HashtableToJson(Hashtable data, string dtName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"");
            sb.Append(dtName);
            sb.Append("\":[{");
            foreach (object key in data.Keys)
            {
                object value = data[key];
                sb.Append("\"");
                sb.Append(key);
                sb.Append("\":\"");
                if (!String.IsNullOrEmpty(value.ToString()) && value != DBNull.Value)
                {
                    sb.Append(value).Replace("\\", "/");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append("\",");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("}]}");
            return JsonCharFilter(sb.ToString());
        }

        public static string HashtableToJson(Hashtable data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (object key in data.Keys)
            {
                object value = data[key];
                sb.Append("\"");
                sb.Append(key);
                sb.Append("\":\"");
                if (!String.IsNullOrEmpty(value.ToString()) && value != DBNull.Value)
                {
                    sb.Append(value).Replace("\\", "/");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append("\",");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return JsonCharFilter(sb.ToString());
        }

        /// <summary>  
        /// Json特符字符过滤
        /// </summary>  
        /// <param name="sourceStr">要过滤的源字符串</param>  
        /// <returns>返回过滤的字符串</returns>  
        private static string JsonCharFilter(string sourceStr)
        {
            return sourceStr;
        }

        #region 对象转换为Json
        /// <summary>
        /// 对象转换为json
        /// </summary>
        /// <param name="jsonObject">json对象</param>
        /// <returns>json字符串</returns>
        public static string ToJson(object jsonObject)
        {
            //string jsonString = "{";
            //PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            //for (int i = 0; i < propertyInfo.Length; i++)
            //{
            //    object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
            //    string value = string.Empty;
            //    if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
            //    {
            //        value = "'" + objectValue.ToString() + "'";
            //    }
            //    else if (objectValue is string)
            //    {
            //        value = "'" + ToJson(objectValue.ToString()) + "'";
            //    }
            //    else if (objectValue is IEnumerable)
            //    {
            //        value = ToJson((IEnumerable)objectValue);
            //    }
            //    else
            //    {
            //        value = ToJson(objectValue.ToString());
            //    }
            //    jsonString += "\"" + propertyInfo[i].Name + "\":" + value + ",";
            //}
            //jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            //return jsonString + "}";

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                Type type = propertyInfo[i].PropertyType;
                string strValue = objectValue.ToString();
                strValue = StringFormat(strValue, type);
                sb.Append("\"" + propertyInfo[i].Name + "\":");
                sb.Append(strValue + ",");

            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region 对象集合转换为json
        /// <summary>
        /// 对象集合转换为json
        /// </summary>
        /// <param name="array">对象集合</param>
        /// <returns>json字符串</returns>
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "{";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region 普通集合转换Json
        /// <summary>    
        /// 普通集合转换Json   
        /// </summary>   
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns>  
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region  DataSet转换为Json
        /// <summary>    
        /// DataSet转换为Json   
        /// </summary>    
        /// <param name="dataSet">DataSet对象</param>   
        /// <returns>Json字符串</returns>    
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }
        #endregion

        #region Datatable转换为Json
        /// <summary>     
        /// Datatable转换为Json     
        /// </summary>    
        /// <param name="table">Datatable对象</param>     
        /// <returns>Json字符串</returns>     
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            if (dt.Rows.Count > 0)
            {
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
                        strValue = StringFormat(strValue, type);
                        if (j < dt.Columns.Count - 1)
                        {
                            jsonString.Append(strValue + ",");
                        }
                        else
                        {
                            jsonString.Append(strValue);
                        }
                    }
                    jsonString.Append("},");
                }
                jsonString.Remove(jsonString.Length - 1, 1);
            }
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>    
        /// DataTable转换为Json     
        /// </summary>    
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        #endregion

        #region DataReader转换为Json
        /// <summary>     
        /// DataReader转换为Json     
        /// </summary>     
        /// <param name="dataReader">DataReader对象</param>     
        /// <returns>Json字符串</returns>  
        public static string ToJson(DbDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type == typeof(Guid))
            {
                str = "\"" + str + "\"";
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

        public static string StringFormat(string str)
        {
            //str = str.Replace(">", "&gt;");  
            //str = str.Replace("<", "&lt;");  
            //str = str.Replace(" ", "&nbsp;");  
            str = str.Replace("\"", "&quot;");  
            str = str.Replace("\'", "&#39;");
            str = str.Replace("\\", "\\\\");//对斜线的转义  
            str = str.Replace("\n", "\\n");
            str = str.Replace("\r", "\\r");
            str = str.Replace("\t", "\\t");
            str = str.Replace("\f", "\\f");
            str = str.Replace("\b", "\\b");
            return str;
        }
        public static string StringFormat1(string str)
        {
            str = str.Replace("\"", "&quot;");
            str = str.Replace("\'", "&#39;");
            str = str.Replace("\\", "\\\\");//对斜线的转义  
            str = str.Replace("\n", "\\n");
            str = str.Replace("\r", "\\r");
            str = str.Replace(">", "&gt;");
            str = str.Replace("<", "&lt;");  
            return str;
        }
        public static string StringFormat3(string str)
        {
            //str = str.Replace("\"", "&quot;");
            //str = str.Replace("\'", "&#39;");
            //str = str.Replace("\\", "\\\\");//对斜线的转义  
            //str = str.Replace("\n", "\\n");
            //str = str.Replace("\r", "\\r");
            str = str.Replace(">", "&gt;");
            str = str.Replace("<", "&lt;");
            str = str.Replace("&lt;p&gt;", "<p>");
            str = str.Replace("&lt;/p&gt;", "</p>");
            return str;
        }
   
        /// <summary>
        ///   去除特殊字符  
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static Hashtable hashTableFormat(ref Hashtable ht)
        {
           //string ss="Оплата+за+передачу+на+Хайратон+взыскана."SYSTEM+CAPITAL+INTERNATIONAL""
            Hashtable ht_value = new Hashtable();
            foreach (DictionaryEntry item in ht)
            {
                string val = item.Value.ToString();
                val = item.Value.ToString().Replace("\"","").Replace("\'", "&#39;")
                .Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r");
                ht_value.Add(item.Key,val);
            }
            return ht_value;
        }

        #endregion
    }
}
