using System;
using System.Collections.Generic;
 
using System.Text;
using System.Data.SqlClient;

using System.Data;
using WZX.DataBase.Common;
namespace WZX.Busines.DAL
{
    public class ExecuteSql : IDisposable
    {
        SqlConnection sqlCon = null;

        public SqlConnection SqlCon
        {
            get { return sqlCon; }
            set
            {
                if (value != null)
                    this.sqlCon = value;
                else
                    throw new ArgumentNullException("数据连接没有被实例化！");
            }
        }
        SqlTransaction sqlTran = null;

        public SqlTransaction SqlTran
        {
            get { return sqlTran; }
            set { sqlTran = value; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
        }

        public ExecuteSql(SqlConnection con)
        {
            this.sqlCon = con;
        }

        public DataSet ExecuteDataSet(string sql)
        {
            return SqlHelper.ExecuteDataset(sqlCon, sqlTran, CommandType.Text, sql);
        }
        public int ExecuteNoQuery(string sql)
        {
            return SqlHelper.ExecuteNonQuery(sqlCon, sqlTran, CommandType.Text, sql);
        }
        public object ExecuteScale(string sql)
        {
            return SqlHelper.ExecuteScalar(sqlCon, sqlTran, CommandType.Text, sql);
        }

    }
}