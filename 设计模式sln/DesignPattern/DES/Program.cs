using RM.Busines;
using RM.Common.DotNetEncrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("input the loginName:");
                var loginName = Console.ReadLine();
                string password = DataFactory.SqlDataBase().getString(new StringBuilder(@" select LoginPassword from Com_UserLogin where loginName='" + loginName + "'"), "LoginPassword");
                try
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = DESEncrypt.Decrypt(password);
                        Console.WriteLine("password:" + password);
                    }
                    else
                    {
                        Console.WriteLine("warning:the user is not existed");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

        }
    }
}
