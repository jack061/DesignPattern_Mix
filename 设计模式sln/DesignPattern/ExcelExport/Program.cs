using RM.Busines;
using RM.Common.DotNetData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WZX.Busines.Util;

namespace ExcelExport
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(" input the tableName:");
                string tableName = Console.ReadLine();
                //1.获取文件夹下的excel文件
                string path = @"G:\execlToSqlserver";
                string[] fileArray = Directory.GetFiles(path, "*.xls");
                if (fileArray.Length<=0)
                {
                    Console.WriteLine("没有找到对应的.xls文件");
                    continue;
                }
                foreach (var file in fileArray)
                {
                    //2.创建文件流,读入excel文件
                    DataTable dt = NPOIHelper.FormatToDatatable(file, "Sheet1");
                    List<StringBuilder> sqls = new List<StringBuilder>();
                    List<object> objs = new List<object>();
                    StringBuilder sql_insert = new StringBuilder();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        sql_insert.Append(@" create table " + tableName + " ( id int primary key identity(1,1),");
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (i == dt.Columns.Count - 1)
                            {
                                sql_insert.Append(dt.Columns[i].ColumnName + " varchar(2000)");
                            }
                            else
                            {
                                sql_insert.Append(dt.Columns[i].ColumnName + " varchar(2000),");
                            }
                        }
                        sql_insert.Append(");");
                        List<Hashtable> list = DataTableHelper.DataTableToList2(dt);
                        sqls.Add(sql_insert);
                        objs.Add(null);
                        SqlUtil.getBatchSqls(list, tableName, ref sqls, ref objs);
                    }
                    string err = string.Empty;
                    int r = DataFactory.SqlDataBase().BatchExecuteByListSql(sqls, objs, ref err);
                    if (r > 0)
                    {
                        Console.WriteLine("创建成功");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine(err);
                        Console.ReadKey();
                    }
                }
            }
   

        }
    }
}
