using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_test
{
    class Program
    {
        static void Main(string[] args)
        {
            ZtTradingEntities context = new ZtTradingEntities();
            Econtract_uploadPath path = new Econtract_uploadPath() { contractNo = "aa", filePath = "111" };
            context.Econtract_uploadPath.Add(path);
            context.SaveChanges();
        }
    }
}
