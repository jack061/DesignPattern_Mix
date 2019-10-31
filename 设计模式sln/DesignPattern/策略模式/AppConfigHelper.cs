using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 策略模式222
{
    class AppConfigHelper
    {
        public static string GetPropertyName()
        {
            string name = null;
            try
            {
                name = System.Configuration.ConfigurationManager.AppSettings["strategyName"];
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return name;
        }
        public static object GetInstance()
        {
            string assemblyName = System.Configuration.ConfigurationManager.AppSettings["strategyName"];
            Type type = Type.GetType(assemblyName);
            var instance = Activator.CreateInstance(type);
            return instance;
        }
    }
}
