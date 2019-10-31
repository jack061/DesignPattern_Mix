using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using WZX.DataBase.Common;

namespace WZX.Busines.DAL
{
    public class SqlCommandExDal : IDisposable
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
            set { sqlTran = value;}
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

        public SqlCommandExDal(SqlConnection con)
        {
            this.sqlCon = con;
        }
   		
   		#region Function

		/// <summary>
		/// 执行sql语句
		/// </summary>
        public DataSet ExecDatasetSql(string strSql, SqlParameter[] sqlPars)
		{
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), sqlPars);
		}
       
        public int ExecuteNonQuery(string strSql, SqlParameter[] sqlPars)
        {
            return SqlHelper.ExecuteNonQuery(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), sqlPars);
        }
       
        public object ExecuteScalar(string strSql, SqlParameter[] sqlPars)
        {
            object obj = SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), sqlPars);
            return obj;
        }
        #endregion
    }
}
