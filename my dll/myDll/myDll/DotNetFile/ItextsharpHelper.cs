using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.tool.xml;
using RM.Common.Util;
using RM.Common.DotNetCode;
using System.Web;
using System.Text.RegularExpressions;

namespace RM.Common.DotNetFile
{
    public class ItextsharpHelper
    {
        /// <summary>
        /// 将Html文字 输出到PDF档里
        /// </summary>
        /// <param name="htmlText"></param>
        /// <param name="flag">0:纵向;非0：横向</param>
        /// <returns></returns>
        public static byte[] ConvertHtmlTextToPDF(string htmlText,int flag = 0)
        {
            byte[] result = null;
            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }
            //避免当htmlText无任何html tag标签的纯文字时，转PDF时会挂掉，所以一律加上<p>标签
            //htmlText = "<p>" + htmlText + "</p>";

            MemoryStream outputStream = new MemoryStream();//PDF输出流
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串转成byte[]
            MemoryStream msInput = new MemoryStream(data);//输入流
            Document doc = null;
            if (flag != 0)
            {
                //设置纸张的大小对象
                Rectangle rectangle = new Rectangle(PageSize.A4);
                //Rectangle rectangle = new Rectangle(842f,595f);
                // 创建word 文档,并旋转，使其横向
                doc = new Document(rectangle);
            }
            else 
            {
                doc = new Document();//要写PDF的文件，样式默认A4
            }
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            //指定文件预设开档时的缩放为100%（文档设置）
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //开启Document文件 
            doc.Open();
            //使用XMLWorkerHelper把Html parse到PDF档里
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8);
            //将pdfDest设定的资料写到PDF档
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);
            doc.Close();
            msInput.Close();
            result = outputStream.ToArray();
            outputStream.Close();
            //回传PDF档案 
            return result;

        }

        /// <summary>
        /// 导出pdf(文件)
        /// FileName：文件名称
        /// content：导出html片段
        /// isWaterMark：是否添加水印 0：不添加，1：添加
        /// flag：是否删除 默认0：不删；1：:删除
        /// </summary>
        /// <param name="FileName">文件名字</param>
        /// <param name="content">导出的html判断</param>
        /// <param name="isWaterMark">是否添加水印 0：不添加，1：添加</param>
        /// <param name="flag">导出后服务器临时文件是否删除0：不删，1：删除</param>
        /// 纵向
        public static void HtmlToPdf(string FileName, string content,string imgpath,int isWaterMark, int flag = 0)
        {
            string filename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + FileName + ".pdf"; //Execle文件重命名
            string savePath = HttpContext.Current.Server.MapPath("\\Files\\PDF\\");//Server.MapPath 获得虚拟服务器相对路径
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
            //String word_content = "<head><meta charset=\"utf-8\"/></head>";
            //word_content += content;
            //StreamWriter writer = new StreamWriter(saveFullPath);//初始化写入
            //writer.WriteLine(new BinaryWriter(new MemoryStream(ConvertHtmlTextToPDF(content))));//写入内容
            //writer.Close();
            File.WriteAllBytes(saveFullPath, ConvertHtmlTextToPDF(content));



            if (isWaterMark == 1)
            {
                #region 添加水印
                string newfilename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + FileName + ".pdf";
                string newsaveFullPath = HttpContext.Current.Server.MapPath("\\Files\\PDF\\") + newfilename;
                if (!string.IsNullOrEmpty(imgpath))
                {
                    //int pages = document.PageNumber;
                    //int pages = GetPageCount(saveFullPath);
                    PdfReader reader = new PdfReader(saveFullPath);
                    int n = reader.NumberOfPages;
                    PdfStamper stamper = new PdfStamper(reader, new FileStream(newsaveFullPath, FileMode.Create));
                    int j = 0;
                    PdfContentByte contentByte;
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("\\") + imgpath);
                    img.ScalePercent(15000f / img.Width);
                    img.SetAbsolutePosition(225, 350);
                    while (j < n)
                    {
                        j++;
                        contentByte = stamper.GetOverContent(j);
                        contentByte.AddImage(img);
                    }
                    stamper.Close();
                    reader.Close();

                }
                FileDownHelper.DownLoadold("\\Files\\PDF\\" + newfilename, flag);
                #endregion
            }
            else 
            {
                FileDownHelper.DownLoadold("\\Files\\PDF\\" + filename, flag);
            }           
           

            
        }
        //横向
        public static void HtmlToPdf1(string FileName, string content, string imgpath, int isWaterMark, int flag = 0)
        {
            string filename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + FileName + ".pdf"; //Execle文件重命名
            string savePath = HttpContext.Current.Server.MapPath("\\Files\\PDF\\");//Server.MapPath 获得虚拟服务器相对路径
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
            //String word_content = "<head><meta charset=\"utf-8\"/></head>";
            //word_content += content;
            //StreamWriter writer = new StreamWriter(saveFullPath);//初始化写入
            //writer.WriteLine(new BinaryWriter(new MemoryStream(ConvertHtmlTextToPDF(content))));//写入内容
            //writer.Close();
            File.WriteAllBytes(saveFullPath, ConvertHtmlTextToPDF(content, 1));

            if (isWaterMark == 1)
            {
                #region 添加水印
                string newfilename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + FileName + ".pdf";
                string newsaveFullPath = HttpContext.Current.Server.MapPath("\\Files\\PDF\\") + newfilename;
                if (!string.IsNullOrEmpty(imgpath))
                {
                    //int pages = document.PageNumber;
                    //int pages = GetPageCount(saveFullPath);
                    PdfReader reader = new PdfReader(saveFullPath);
                    int n = reader.NumberOfPages;
                    PdfStamper stamper = new PdfStamper(reader, new FileStream(newsaveFullPath, FileMode.Create));
                    int j = 0;
                    PdfContentByte contentByte;
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("\\") + imgpath);
                    img.ScalePercent(15000f / img.Width);
                    img.SetAbsolutePosition(225, 350);
                    while (j < n)
                    {
                        j++;
                        contentByte = stamper.GetOverContent(j);
                        contentByte.AddImage(img);
                    }
                    stamper.Close();
                    reader.Close();

                }
                FileDownHelper.DownLoadold("\\Files\\PDF\\" + newfilename, flag);
                #endregion
            }
            else
            {
                FileDownHelper.DownLoadold("\\Files\\PDF\\" + filename, flag);
            }



        }
        /// <summary>
        /// 获取pdf文件的页数
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static int GetPageCount(string filepath) { 
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader r = new StreamReader(fs);
            string pdfText = r.ReadToEnd();
            Regex rx1 = new Regex(@"/Type\s*/Page[^s]");
            MatchCollection matches = rx1.Matches(pdfText);
            r.Close();
            return  matches.Count;
        }
    }
}
