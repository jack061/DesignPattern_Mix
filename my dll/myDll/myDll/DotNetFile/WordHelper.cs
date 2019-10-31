using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Common.DotNetFile
{
    public class WordHelper
    {
        /// <summary>
        /// 导出word(文件)
        /// FileName：文件名称
        /// content：导出html片段
        /// flag：是否删除 默认0：不删；1：:删除
        /// </summary>
        /// <param name="FileName">文件名字</param>
        /// <param name="content">导出的html判断</param>
        /// <param name="flag">导出后服务器临时文件是否删除0：不删，1：删除</param>
        public static void HtmlToWord(string FileName, string content,int flag=0)
        {
            string filename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + FileName + ".doc"; //Execle文件重命名
            string savePath = HttpContext.Current.Server.MapPath("\\Files\\Word\\");//Server.MapPath 获得虚拟服务器相对路径
            string saveFullPath = savePath + filename;//文件路径
            if (!(Directory.Exists(savePath)))
            {//判断路径是否存在---不存在创建路径
                Directory.CreateDirectory(savePath);
            }
            if ((File.Exists(saveFullPath)))
            {//判断文件是否已经存在，存在删除
                File.Delete(saveFullPath);
            }
            //String word_content = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" " +
            //                 "xmlns:o=\"urn:schemas-microsoft-com:office:office\" " +
            //                 "xmlns:w=\"urn:schemas-microsoft-com:office:word\" " +
            //                 "xmlns=\"http://www.w3.org/TR/REC-html40\">" +
            //                 "<head><meta charset=\"utf-8\"/></head>";
            String word_content = "<head><meta charset=\"utf-8\"/></head>";
            word_content += content;
            StreamWriter writer = new StreamWriter(saveFullPath);//初始化写入
            writer.WriteLine(word_content);//写入内容
            writer.Close();
            FileDownHelper.DownLoadold("\\Files\\Word\\" + filename,flag);
        }


        /// <summary>
        /// 导出word
        /// </summary>
        /// <param name="FileName">文件名字</param>
        public static void ExportWord(string FileName, MemoryStream ms)
        {
            string filename = FileName;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Charset = "Utf-8";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename + ".doc", System.Text.Encoding.UTF8));
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 导出word
        /// </summary>
        /// <param name="FileName">文件名字</param>
        public static void ExportWord(string FileName, string content)
        {
            string filename = FileName;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Charset = "gb2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename + ".doc", System.Text.Encoding.UTF8));
            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.End();
        }
    }
}