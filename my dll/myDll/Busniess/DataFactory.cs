using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WZX.DataBase;
using WZX.Common.DotNetConfig;
using WZX.DataBase.SqlServer;
using RM.DataBase;
using PDA_Service.DataBase.DataBase.SqlServer;
using RM.Common.DotNetConfig;

namespace RM.Busines
{
    /// <summary>
    /// 连接数据库服务工厂
    /// </summary>
    public class DataFactory
    {
        /// <summary>
        /// 链接 SqlServer 数据库
        /// </summary>
        /// <returns></returns>
        public static IDbHelper SqlDataBase()
        {
            return new SqlServerHelper(ConfigHelper.GetAppSettings("SqlServer_RM_DB"));
        }

        /// <summary>
        /// 链接 SqlServer 数据库
        /// </summary>
        /// <returns></returns>
        public static SqlServerHelperExpand SqlDataBaseExpand()
        {
            return new SqlServerHelperExpand(ConfigHelper.GetAppSettings("SqlServer_RM_DB"));
        }
    }
}
