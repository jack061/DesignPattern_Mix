using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace RM.Common.Util
{
    class UnicodeFontFactory : FontFactoryImp
    {
        private static readonly string arialuniFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            "arialuni.ttf");//arial unicode MS是完整的unicode字型。
        private static readonly string DFKaiPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
          "KAIU.TTF");//标楷体
        private static readonly string simfangPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
          "simfang.ttf");//仿宋
        private static readonly string simTimesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            "times.ttf");//罗马  不支持中文
        private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            "Arial.ttf");//Arial 不支持中文
        private static readonly string simYouPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
       "simsun.ttc");//宋体

        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
            bool cached)
        {
            //可用Arial或标楷体，选一个
            //BaseFont baseFont = BaseFont.CreateFont(arialuniFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            BaseFont baseFont = BaseFont.CreateFont(arialuniFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style, color);
        }

    }
}
