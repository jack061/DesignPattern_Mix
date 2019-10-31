using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 状态模式
{
    class AppConfigHelper
    {
        public static string GetPropertyName()
        {
            string propertyName = null;
            try
            {
                propertyName = System.Configuration.ConfigurationManager.AppSettings["propertyName"];
            }
            catch (Exception x)
            {

                Console.WriteLine(x.Message);
            }
            return propertyName;
        }
        public static object GetInstance()
        {
            string assemblyName = AppConfigHelper.GetPropertyName();
            Type t = Type.GetType(assemblyName);
            var instance = Activator.CreateInstance(t);
            return instance;
        }
    }
}
