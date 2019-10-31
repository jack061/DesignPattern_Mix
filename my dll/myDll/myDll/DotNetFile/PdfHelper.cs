using iTextSharp.text.pdf;
using Pechkin;
using Pechkin.Synchronized;
using RM.Common.DotNetCode;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace RM.Common.DotNetFile
{
    public class PdfHelper
    {
        #region Pechkin 集成wkhtmltopdf
        /// <summary>
        /// 将Html文字 输出到PDF档里
        /// </summary>
        /// <param name="htmlText"></param>
        /// <param name="flag">0:纵向;非0：横向</param>
        /// <param name="pagesizeFlag">0:不加页码;非0：加页码</param>
        /// <returns></returns>
        public static byte[] ConvertHtmlTextToPDF(string htmlText, int flag = 0,int pagesizeFlag=0)
        {
            byte[] result = null;

            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }
            //1、先生成html页面
            /*
            string host = HttpContext.Current.Request.Url.Host;
            int port = HttpContext.Current.Request.Url.Port;
            string filename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + ".html"; //Execle文件重命名
            string savePath = HttpContext.Current.Server.MapPath("\\Files\\html\\");//Server.MapPath 获得虚拟服务器相对路径
            string saveFullPath = savePath + filename;//文件路径
            if (!(Directory.Exists(savePath)))
            {//判断路径是否存在---不存在创建路径
                Directory.CreateDirectory(savePath);
            }
            if ((File.Exists(saveFullPath)))
            {//判断文件是否已经存在，存在删除
                File.Delete(saveFullPath);
            }
            File.WriteAllBytes(saveFullPath, System.Text.Encoding.UTF8.GetBytes(htmlText));
             * */
            
            //2、生成pdf参数设置
            SynchronizedPechkin sc = new SynchronizedPechkin(new GlobalConfig()
                                            .SetMargins(new Margins() { Left = ConvertToHundredthsInch(10), Right = ConvertToHundredthsInch(10), Top = ConvertToHundredthsInch(15), Bottom = ConvertToHundredthsInch(15) }) //设置边距  
                                            .SetPaperOrientation(flag==0?true:false) //设置纸张方向为横向,true为横向，false 为纵向
                                            .SetPaperSize(ConvertToHundredthsInch(210), ConvertToHundredthsInch(297))); //设置纸张大小210mm * 297mm (A4)
                                            
            ObjectConfig oc = new ObjectConfig();
            oc.SetPrintBackground(true)
                .SetLoadImages(true)
                .SetZoomFactor(1.5);
               // .SetPageUri("http://" + host + ":" + port + "/Files/html/" + filename);//通过链接获取页面内容
            if (pagesizeFlag != 0) 
            {
                oc.Footer.SetCenterText("[page]");//页码
            }
            result = sc.Convert(oc, System.Text.Encoding.UTF8.GetBytes(htmlText));//直接赋值内容
            //result = sc.Convert(oc);
            return result;

        }
       
        /// <summary>
        /// 导出pdf(文件)
        /// FileName：文件名称
        /// content：导出html片段
        /// isWaterMark：是否添加水印 0：不添加，1：添加
        /// flag：是否删除 默认0：不删；1：:删除
        ///oriFlag： 导出为横向或纵向，0为横向，1为纵向
        /// </summary>
        /// <param name="FileName">文件名字</param>
        /// <param name="content">导出的html判断</param>
        /// <param name="isWaterMark">是否添加水印 0：不添加，1：添加</param>
        /// <param name="flag">导出后服务器临时文件是否删除0：不删，1：删除</param>
        /// <param name="oriFlag">导出为横向或纵向，0为横向，1为纵向</param>
        /// 纵向
        public static string HtmlToPdf(string FileName, string content, string imgpath, int isWaterMark, int flag = 0,int oriFlag=0)
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
            File.WriteAllBytes(saveFullPath, ConvertHtmlTextToPDF(content, oriFlag));



            if (isWaterMark == 1)
            {
                #region 添加水印
                 filename = DateTimeHelper.GetToday("yyyyMMddHHmmssfff") + FileName + ".pdf";
                 string newsaveFullPath = HttpContext.Current.Server.MapPath("\\Files\\PDF\\") + filename;
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
                #endregion
            }
            return filename;
        }
        /// <summary>
        /// 获取pdf文件的页数
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static int GetPageCount(string filepath)
        {
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader r = new StreamReader(fs);
            string pdfText = r.ReadToEnd();
            Regex rx1 = new Regex(@"/Type\s*/Page[^s]");
            MatchCollection matches = rx1.Matches(pdfText);
            r.Close();
            return matches.Count;
        }
        #endregion

        private static int ConvertToHundredthsInch(int millimeter)
          {
              return (int)((millimeter * 10.0) / 2.54);
          }
    }
}
