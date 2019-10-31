using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using RM.Common.DotNetJson;
using RM.Common.DotNetCode;
using System.IO;
using System.Data;

namespace RM.Common.DotNetFile
{
    public class ExcelHelper
    {
        /// <summary>
        ///上传excel 文件转化为json(未完成)
        /// </summary>
        /// <param name="context"></param>
        public string upExcelToJSON(HttpContext context,string virSavePath)
        {
            Hashtable ht_result = new Hashtable();
            try
            {
                HttpPostedFile file = context.Request.Files["Filedata"];
                if (file != null)
                {
                    string oldFileName = file.FileName;//原文件名
                    int size = file.ContentLength;//附件大小

                    string extenstion = oldFileName.Substring(oldFileName.LastIndexOf(".") + 1);//后缀名
                    if (extenstion != "xls" && extenstion != "xlsx")
                    {
                        ht_result.Add("status", "F");
                        ht_result.Add("msg", "只可以选择Excel文件");
                        return JsonHelper.HashtableToJson(ht_result);
                    }
                    string filename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + "." + extenstion; //Execle文件重命名
                    string savePath = context.Server.MapPath(virSavePath);//Server.MapPath 获得虚拟服务器相对路径
                    string saveFullPath = savePath + filename;//文件路径
                    bool flag = Directory.Exists(savePath);
                    if (!(Directory.Exists(savePath)))
                    {//判断路径是否存在---不存在创建路径
                        Directory.CreateDirectory(savePath);
                    }
                    if ((File.Exists(saveFullPath)))
                    {//判断文件是否已经存在，存在删除
                        File.Delete(saveFullPath);
                    }

                    file.SaveAs(saveFullPath);
                    bool uploaded = File.Exists(saveFullPath);

                    if (uploaded)
                    {
                        DataTable dt = null;
                        try
                        {
                            //dt = ExcelHelper.ExcelToDataSet("Sheet1", saveFullPath);
                            dt = NPOIHelper.FormatToDatatable(saveFullPath, "Sheet1");
                        }
                        catch
                        {
                            ht_result.Add("status", "F");
                            ht_result.Add("msg", "导入失败，Excel工作表标签名错误，标签名必须是Sheet1，请查证后再导入!");
                            return JsonHelper.HashtableToJson(ht_result);
                        }

                        if (dt != null)
                        {
                            int rowsnum = dt.Rows.Count;
                            int columnnum = dt.Columns.Count;
                            if (rowsnum == 0)
                            {
                                ht_result.Add("status", "F");
                                ht_result.Add("msg", "Excel表为空表,无数据!");
                                return JsonHelper.HashtableToJson(ht_result);
                            }
                            else
                            {
                                for (int i = 0; i < columnnum; i++)
                                {//对列名进行处理
                                    dt.Columns[i].ColumnName = getColumn(dt.Columns[i].ColumnName);
                                }

                                DataRow[] arrayDR = dt.Select("NUMBER is null ");
                                if (arrayDR != null && arrayDR.Length > 0)
                                {
                                    ht_result.Add("status", "F");
                                    ht_result.Add("msg", "导入失败，含有专用号为空的数据，请完善！");
                                    return JsonHelper.HashtableToJson(ht_result);
                                }
                                StringBuilder sblist = new StringBuilder();
                                for (int i = 0; i < rowsnum; i++)
                                {//对数据进行校验

                                }

                                /**可做数据保存操作
                                 * 
                                 */

                                ht_result.Add("status", "T");
                                ht_result.Add("msg", "Excle表加载成功!");
                                return JsonHelper.HashtableToJson(ht_result);

                            }
                        }
                    }

                }
                ht_result.Add("status", "F");
                ht_result.Add("msg", "加载文件失败");
                return JsonHelper.HashtableToJson(ht_result);
            }
            catch (Exception ex)
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "加载文件失败:" + ex.ToString());
                return JsonHelper.HashtableToJson(ht_result);
            }
        }

        /// <summary>
        ///上传excel 文件转化为Datatable
        /// </summary>
        /// <param name="context"></param>
        public DataTable upExcelToDatatable(HttpContext context, string virSavePath, ref Hashtable ht_result)
        {
            DataTable dt = null;
            //Hashtable ht_result = new Hashtable();
            try
            {
                HttpPostedFile file = context.Request.Files["Filedata"];
                if (file != null)
                {
                    string oldFileName = file.FileName;//原文件名
                    int size = file.ContentLength;//附件大小

                    string extenstion = oldFileName.Substring(oldFileName.LastIndexOf(".") + 1);//后缀名
                    if (extenstion != "xls" && extenstion != "xlsx")
                    {
                        ht_result.Add("status", "F");
                        ht_result.Add("msg", "只可以选择Excel文件");
                        return dt;
                    }
                    string filename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + "." + extenstion; //Execle文件重命名
                    string savePath = context.Server.MapPath(virSavePath);//Server.MapPath 获得虚拟服务器相对路径
                    string saveFullPath = savePath + filename;//文件路径
                    bool flag = Directory.Exists(savePath);
                    if (!(Directory.Exists(savePath)))
                    {//判断路径是否存在---不存在创建路径
                        Directory.CreateDirectory(savePath);
                    }
                    if ((File.Exists(saveFullPath)))
                    {//判断文件是否已经存在，存在删除
                        File.Delete(saveFullPath);
                    }

                    file.SaveAs(saveFullPath);
                    bool uploaded = File.Exists(saveFullPath);

                    if (uploaded)
                    {
                        try
                        {
                            //dt = ExcelHelper.ExcelToDataSet("Sheet1", saveFullPath);
                            dt = NPOIHelper.FormatToDatatable(saveFullPath, "Sheet1");
                        }
                        catch
                        {
                            ht_result.Add("status", "F");
                            ht_result.Add("msg", "导入失败，Excel工作表标签名错误，标签名必须是Sheet1，请查证后再导入!");
                            return dt;
                        }

                        if (dt != null)
                        {
                            int rowsnum = dt.Rows.Count;
                            int columnnum = dt.Columns.Count;
                            if (rowsnum == 0)
                            {
                                ht_result.Add("status", "F");
                                ht_result.Add("msg", "Excel表为空表,无数据!");
                                return dt;
                            }
                            else
                            {
                                for (int i = 0; i < columnnum; i++)
                                {//对列名进行处理
                                    dt.Columns[i].ColumnName = getColumn(dt.Columns[i].ColumnName);
                                }

                                //DataRow[] arrayDR = dt.Select("NUMBER is null ");
                                //if (arrayDR != null && arrayDR.Length > 0)
                                //{
                                //    ht_result.Add("status", "F");
                                //    ht_result.Add("msg", "导入失败，含有专用号为空的数据，请完善！");
                                //    return dt;
                                //}
                                StringBuilder sblist = new StringBuilder();
                                for (int i = 0; i < rowsnum; i++)
                                {//对数据进行校验

                                }
                                ht_result.Add("status", "T");
                                ht_result.Add("msg", "Excle表加载成功!");
                                return dt;

                            }
                        }
                    }

                }
                ht_result.Add("status", "F");
                ht_result.Add("msg", "加载文件失败");
                return dt;
            }
            catch (Exception ex)
            {
                ht_result.Add("status", "F");
                ht_result.Add("msg", "加载文件失败:" + ex.ToString());
                return dt;
            }
        }

        #region 辅助
        public static string getColumn(string column)
        {
            string temp = column;
            if (!(string.IsNullOrEmpty(temp)))
            {
                int index = temp.IndexOf("<");
                if (index > 0)
                {
                    temp = temp.Substring(0, index);
                }
            }
            return temp;
        }
        public static string getValue(string value)
        {
            string temp = value;
            if ((string.IsNullOrEmpty(temp)))
            {
                temp = null;
            }
            return temp;
        }
        #endregion
    }
}
