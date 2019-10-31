
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 抽象工厂模式
{
    class AppConfigHelper
    {
        public static string GetFactoryName()
        {
            string factoryName = null;
            try
            {
                factoryName = System.Configuration.ConfigurationManager.AppSettings["factoryName"];
                return factoryName;
            }
            catch (Exception ex)
            {
                return factoryName;
            }
            
        }
        public static object GetFactoryInstance()
        {
            string assemblyName = AppConfigHelper.GetFactoryName();
            Type type = Type.GetType(assemblyName);
            var instance = Activator.CreateInstance(type);
            return instance;
        }
    }
}
