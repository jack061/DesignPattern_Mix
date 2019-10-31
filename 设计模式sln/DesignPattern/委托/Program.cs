using RM.Busines;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 委托
{
    class Program
    {
        static void Main(string[] args)
        {
            DelegateEcontract ec = new DelegateEcontract();
            List<string> list = ec.FindAll("Econtract", u => u.Contains("HK"));
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
    class DelegateEcontract
    {
        public List<string> FindAll(string tableName, Predicate<string> match)
        {
            List<string> list = new List<string>();
            StringBuilder sqldata = new StringBuilder();
            sqldata.Append(@" select contractNo from " + tableName);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sqldata, 0);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (match(dt.Rows[i]["contractNo"].ToString()))
                {
                    list.Add(dt.Rows[i]["contractNo"].ToString());
                }
            }
            return list;
        }
    }
}
