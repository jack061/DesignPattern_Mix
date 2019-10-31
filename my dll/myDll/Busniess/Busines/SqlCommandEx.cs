using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using RM.Busines.DAL;
using RM.Common.DotNetConfig;
using WZX.Busines.DAL;
using WZX.Common.DotNetConfig;

namespace WZX.BLL
{
    public class SqlCommandExBll:IDisposable
    {
        private bool selfConn = false;


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
            set
            {
                sqlTran = value;
                if (this.dal != null)
                {
                    this.dal.SqlTran = this.SqlTran;
                }
            }
        }

        private SqlCommandExDal dal = null;
        public SqlCommandExBll()
        {
            this.selfConn = true;
            this.sqlCon = new SqlConnection(ConfigHelper.GetAppSettings("SqlServer_RM_DB"));
            this.sqlCon.Open();
            this.dal = new SqlCommandExDal(this.sqlCon);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据连接</param>
        public SqlCommandExBll(SqlConnection con)
        {
            this.sqlCon = con;
            this.dal = new SqlCommandExDal(con);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据连接</param>
        /// <param name="transaction">事务</param>
        public SqlCommandExBll(SqlConnection con, SqlTransaction tran)
            : this(con)
        {
            this.SqlTran = tran;
            this.dal.SqlTran = this.SqlTran;
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

            if (this.selfConn)
            {
                this.sqlCon.Close();
                this.sqlCon.Dispose();
                this.sqlCon = null;
            }
            if (this.dal != null)
            {
                this.dal.Dispose();
                this.dal = null;
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        public DataSet ExecDatasetSql(string strSql, SqlParameter[] sqlPars)
        {
            return this.dal.ExecDatasetSql(strSql, sqlPars);
        }
        public DataSet ExecDatasetSql(string strSql)
        {
            return this.dal.ExecDatasetSql(strSql, null);
        }
        public int ExecuteNonQuery(string strSql, SqlParameter[] sqlPars)
        {
            return this.dal.ExecuteNonQuery(strSql, sqlPars);
        }
        public int ExecuteNonQuery(string strSql)
        {
            return this.dal.ExecuteNonQuery(strSql, null);
        }
        public object ExecuteScalar(string strSql, SqlParameter[] sqlPars)
        {
            object obj = this.dal.ExecuteScalar(strSql, sqlPars);
            return obj;
        }
        public object ExecuteScalar(string strSql)
        {
            object obj = this.dal.ExecuteScalar(strSql, null);
            return obj;
        }
    }
}
