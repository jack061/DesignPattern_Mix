using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using RM.Common.DotNetCode;
using WZX.Busines.Util;

namespace WZX.Busines.Busines
{
    /// <summary>
    /// 收入预测业务
    /// </summary>
    public class IncomeForecastBusiness
    {
        /// <summary>
        /// 收入数据导入预处理
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public void preDealIncomeExcel( ref DataTable dt,string filePath,string location)
        {
            HSSFWorkbook hssfworkbook;
            DateTime dateTime = DateTime.Now;
            int year = dateTime.Year;
            int month = 0;
            int week = 0;
            int interval = 0;
            int indexer = 0;
            string dateTimeString = "";
            string impDateTime = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            if(File.Exists(filePath))
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                //获得sheet
                ISheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                //处理单元数据
                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    if (row.LastCellNum > 40)
                    {
                        indexer++;
                        ICell cell = row.GetCell(1);
                        if (cell != null && "物料专用号".Equals(cell.ToString()))
                        {//第一行
                            cell = row.GetCell(20);
                            string temp = getCellString(cell);
                            if (!(string.IsNullOrEmpty(temp)))
                            {
                                string[] tempDateArray = temp.Split('/');
                                if (tempDateArray.Length == 2)
                                {
                                    month = int.Parse(tempDateArray[0]);
                                    dateTimeString = dateTime.Year + "-" + addZero(tempDateArray[0]) + "-" + addZero(tempDateArray[1]);
                                    dateTime = DateTime.Parse(dateTimeString);
                                    week = DateTimeHelper.GetWeekOfYear(dateTime);
                                    int tempWeek = DateTimeHelper.GetWeekNumberOfDay(dateTime);
                                    interval = 7 - tempWeek + 7 +1;//当前到下个周的周末
                                }
                            }
                        }
                        else 
                        {//第二行之后
                            indexer++;
                            if (indexer == 2)
                            {//第二行为合计行
                                cell = row.GetCell(1);//物料专用号
                                dr["NUMBER"] = getCellString(cell);
                                cell = row.GetCell(2);//描述
                                dr["DESCRIPTION"] = getCellString(cell);
                                cell = row.GetCell(3);//BU
                                dr["BU"] = getCellString(cell);
                                cell = row.GetCell(4);//PL
                                dr["PL"] = getCellString(cell);
                                cell = row.GetCell(5);//PL工厂
                                dr["PL_FACTORY"] = getCellString(cell);
                                cell = row.GetCell(11);//单位
                                dr["UNIT"] = getCellString(cell);
                                cell = row.GetCell(19);//T-小计
                                dr["WEEK0"] = getCellNum(cell);
                                cell = row.GetCell(19 + interval + 1);//T+小计
                                dr["WEEK1"] = getCellNum(cell);
                                for (int i = 1; i < 13; i++)
                                {//T+2~T+13小计
                                    cell = row.GetCell(19 + interval + 1 + i);//T+小计
                                    dr["WEEK" + (1 + i)] = getCellNum(cell);
                                }

                                dr["AREA"] = location;
                                dr["IMPTIME"] = impDateTime;
                                dr["FORECASTTIME"] = dateTimeString;
                                dr["YEAR"] = year;
                                dr["MONTH"] = month;
                                dr["WEEK"] = week;
                                dr["FLAG"] = ConstantUtil.INCOME_IMP_SUM_FLAG;
                            }
                            else 
                            {
                                cell = row.GetCell(1);//物料专用号
                                dr["NUMBER"] = getCellString(cell);
                                cell = row.GetCell(2);//描述
                                dr["DESCRIPTION"] = getCellString(cell);
                                cell = row.GetCell(3);//BU
                                dr["BU"] = getCellString(cell);
                                cell = row.GetCell(4);//PL
                                dr["PL"] = getCellString(cell);
                                cell = row.GetCell(5);//PL工厂
                                dr["PL_FACTORY"] = getCellString(cell);
                                cell = row.GetCell(11);//单位
                                dr["UNIT"] = getCellString(cell);
                                cell = row.GetCell(19);//T-小计
                                dr["WEEK0"] = getCellNum(cell);
                                cell = row.GetCell(19 + interval + 1);//T+小计
                                dr["WEEK1"] = getCellNum(cell);
                                for (int i = 1; i < 13; i++)
                                {//T+2~T+13小计
                                    cell = row.GetCell(19 + interval + 1 + i);//T+小计
                                    dr["WEEK" + (1 + i)] = getCellNum(cell);
                                }

                                dr["AREA"] = location;
                                dr["IMPTIME"] = impDateTime;
                                dr["FORECASTTIME"] = dateTimeString;
                                dr["YEAR"] = year;
                                dr["MONTH"] = month;
                                dr["WEEK"] = week;
                                dr["FLAG"] = ConstantUtil.INCOME_IMP_UNSUM_FLAG;
                            }

                            dt.Rows.Add(dr);
                        }
                    }
                    
                }
            }
           
        }

        /// <summary>
        /// 收入数据导入预处理（自然周）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public void preDealIncomeExcel1(ref DataTable dt, string filePath, string location)
        {
            HSSFWorkbook hssfworkbook;
            DateTime dateTime = DateTime.Now;
            int year = dateTime.Year;
            int month = 0;
            int week = 0;
            int interval = 0;
            int indexer = 0;
            int indexweek = 0;
            string dateTimeString = "";
            string impDateTime = DateTimeHelper.GetToday("yyyy-MM-dd HH:mm:ss");
            if (File.Exists(filePath))
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                //获得sheet
                ISheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                //处理单元数据
                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    if (row.LastCellNum > 40)
                    {
                        indexer++;
                        ICell cell = row.GetCell(1);
                        if (cell != null && "物料专用号".Equals(cell.ToString()))
                        {//第一行
                            cell = row.GetCell(20);
                            string temp = getCellString(cell);
                            if (!(string.IsNullOrEmpty(temp)))
                            {
                                string[] tempDateArray = temp.Split('/');
                                if (tempDateArray.Length == 2)
                                {
                                    month = int.Parse(tempDateArray[0]);
                                    dateTimeString = dateTime.Year + "-" + addZero(tempDateArray[0]) + "-" + addZero(tempDateArray[1]);
                                    dateTime = DateTime.Parse(dateTimeString);
                                    week = DateTimeHelper.GetWeekOfYear(dateTime);
                                    indexweek = DateTimeHelper.GetWeekNumberOfDay(dateTime);
                                    interval = 7 - indexweek + 7 + 1;//当前到下个周的周末
                                }
                            }
                        }
                        else
                        {//第二行之后
                            indexer++;
                            if (indexer == 2)
                            {//第二行为合计行
                                cell = row.GetCell(1);//物料专用号
                                dr["NUMBER"] = getCellString(cell);
                                cell = row.GetCell(2);//描述
                                dr["DESCRIPTION"] = getCellString(cell);
                                cell = row.GetCell(3);//BU
                                dr["BU"] = getCellString(cell);
                                cell = row.GetCell(4);//PL
                                dr["PL"] = getCellString(cell);
                                cell = row.GetCell(5);//PL工厂
                                dr["PL_FACTORY"] = getCellString(cell);
                                cell = row.GetCell(11);//单位
                                dr["UNIT"] = getCellString(cell);

                                #region 开始计算小计
                                /*
                                cell = row.GetCell(19);//T-小计
                                dr["WEEK0"] = getCellNum(cell);
                                cell = row.GetCell(19 + interval + 1);//T+小计
                                dr["WEEK1"] = getCellNum(cell);
                                 */
                                double week0 = 0.0;
                                double week1 = 0.0;
                                getSum(row, 19, indexweek, ref week0, ref week1);
                                dr["WEEK0"] = week0;
                                dr["WEEK1"] = week1;

                                for (int i = 1; i < 13; i++)
                                {//T+2~T+13小计
                                    cell = row.GetCell(19 + interval + 1 + i);//T+小计
                                    dr["WEEK" + (1 + i)] = getCellNum(cell);
                                }
                                #endregion

                                dr["AREA"] = location;
                                dr["IMPTIME"] = impDateTime;
                                dr["FORECASTTIME"] = dateTimeString;
                                dr["YEAR"] = year;
                                dr["MONTH"] = month;
                                dr["WEEK"] = week;
                                dr["FLAG"] = ConstantUtil.INCOME_IMP_SUM_FLAG;
                            }
                            else
                            {
                                cell = row.GetCell(1);//物料专用号
                                dr["NUMBER"] = getCellString(cell);
                                cell = row.GetCell(2);//描述
                                dr["DESCRIPTION"] = getCellString(cell);
                                cell = row.GetCell(3);//BU
                                dr["BU"] = getCellString(cell);
                                cell = row.GetCell(4);//PL
                                dr["PL"] = getCellString(cell);
                                cell = row.GetCell(5);//PL工厂
                                dr["PL_FACTORY"] = getCellString(cell);
                                cell = row.GetCell(11);//单位
                                dr["UNIT"] = getCellString(cell);
                                #region 开始计算小计
                                /*
                                cell = row.GetCell(19);//T-小计
                                dr["WEEK0"] = getCellNum(cell);
                                cell = row.GetCell(19 + interval + 1);//T+小计
                                dr["WEEK1"] = getCellNum(cell);
                                 */
                                double week0 = 0.0;
                                double week1 = 0.0;
                                getSum(row, 19, indexweek, ref week0, ref week1);
                                dr["WEEK0"] = week0;
                                dr["WEEK1"] = week1;

                                for (int i = 1; i < 13; i++)
                                {//T+2~T+13小计
                                    cell = row.GetCell(19 + interval + 1 + i);//T+小计
                                    dr["WEEK" + (1 + i)] = getCellNum(cell);
                                }
                                #endregion

                                dr["AREA"] = location;
                                dr["IMPTIME"] = impDateTime;
                                dr["FORECASTTIME"] = dateTimeString;
                                dr["YEAR"] = year;
                                dr["MONTH"] = month;
                                dr["WEEK"] = week;
                                dr["FLAG"] = ConstantUtil.INCOME_IMP_UNSUM_FLAG;
                            }

                            dt.Rows.Add(dr);
                        }
                    }

                }
            }

        }

        #region 辅助
        public string getCellString(ICell cell)
        {
            string temp = "";
            if (cell == null)
            {
                temp = "";
            }
            else
            {
                temp = cell.ToString();
            }

            return temp;
        }
        /// <summary>
        /// 获取cell数值型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public double getCellNum(ICell cell)
        {
            double  temp = 0.0;
            if (cell == null || string.IsNullOrEmpty(cell.ToString()))
            {
                temp = 0.0;
            }
            else
            {
                temp = double.Parse(cell.ToString());
            }

            return temp;
        }
        /// <summary>
        /// 前部补0
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public string addZero(string _string)
        {
            int temp = int.Parse(_string);
            if (temp >= 0 && temp < 10)
            {
                return "0" + temp;
            }
            else
            {
                return temp + "";
            }
        }
        /// <summary>
        /// 计算前两自然周
        /// </summary>
        /// <param name="row">excel行</param>
        /// <param name="columnIndex">起始列索引</param>
        /// <param name="weekIndex">当前周内索引</param>
        /// <param name="week0">返回第一周</param>
        /// <param name="week1">返回第二周</param>
        public void getSum(IRow row,int columnIndex,int weekIndex,ref double week0,ref double week1)
        {
            if(weekIndex > 1)
            {
                double week0_1 = 0.0;
                double week0_2 = 0.0;
                double week1_sum = 0.0;
                int flag = 0;
                flag = weekIndex;
                for (int i = columnIndex - (flag - 1); (flag - 1) > 0; flag--)
                {
                    ICell cell = row.GetCell(i);
                    week0_1 += getCellNum(cell);
                }
                flag = weekIndex;
                for (int i = columnIndex + 1; flag < 8; flag++, i++)
                 {
                     ICell cell = row.GetCell(i);
                     week0_2 += getCellNum(cell);
                 }
                 week0 = week0_1 + week0_2;//第一自然周

                 int startIndex = columnIndex + (7 - weekIndex + 1);
                 flag = 1;
                 for (int i = startIndex + flag; flag < 8; flag++)
                 {
                     ICell cell = row.GetCell(startIndex + flag);
                     week1_sum += getCellNum(cell);
                 }
                 week1 = week1_sum;
                
            }
            
        }
        #endregion
    }
}
