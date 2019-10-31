using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace WZX.DataBase.Common
{

    /// <summary>
    /// 
    /// </summary>
    public class SqlHelper
    {
        private enum SqlConnectionOwnership
        {
            Internal = 0,
            External = 1,
        }

        // METHODS
        private SqlHelper()
        {
        }

        /// <summary>
        /// add sqlPararmeters array to the SqlCommand
        /// if the value is empty ,the equal to <c>DBNull.Value</c>
        /// </summary>
        /// <param name="command">type SqlCommand </param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc or query string</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            SqlParameter[] sqlParameters = commandParameters;
            foreach (SqlParameter sqlParameter in sqlParameters)
            {
                if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && (sqlParameter.Value == null))
                {
                    sqlParameter.Value = DBNull.Value;
                }
                command.Parameters.Add(sqlParameter);
            }
        }

        /// <summary>
        /// create a new <c>SqlConnection</c>
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <returns>return a newly instantiated  SqlConnection instance</returns>
        public static SqlConnection InitializingSqlConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// dispose the  SqlConnection instance
        /// </summary>
        /// <param name="connection"></param>
        public static void DisposeSqlConnection(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// get the SqlTransaction object 
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <returns>Newly instantiated SqlTransaction instance</returns>
        public static SqlTransaction InitializingSqlTransaction(string connectionString)
        {
            SqlConnection connection = InitializingSqlConnection(connectionString);
            SqlTransaction transaction = connection.BeginTransaction();
            return transaction;
        }

        /// <summary>
        /// get the SqlTransaction object
        /// </summary>
        /// <param name="connection">a SqlConnection object</param>
        /// <returns>Newly instantiated SqlTransaction instance</returns>
        public static SqlTransaction InitializingSqlTransaction(SqlConnection connection)
        {
            SqlTransaction transaction = connection.BeginTransaction();
            return transaction;
        }

        /// <summary>
        /// 将对象值赋给参数
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc or query string</param>
        /// <param name="parameterValues"></param>		/// 
        /// <returns>完成,返回TRUE</returns>		
        private static bool AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            bool bolresult;
            bolresult = !(commandParameters == null) || (parameterValues == null);
            if (bolresult)
            {
                if (commandParameters.Length != parameterValues.Length)
                {
                    //在向方法提供的其中一个参数无效时引发的异常。 
                    throw new ArgumentException("Parameter count does not match Parameter Value count.");
                }
                int i = 0;
                int j = commandParameters.Length;
                while (i < j)
                {
                    commandParameters[i].Value = parameterValues[i];
                    i++;
                }
            }
            return bolresult;
        }

        /// <summary>
        /// 准备<c>command</c>参数
        /// 不包括对于Exception 的处理
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        private static void PrepareCommand(SqlCommand command, SqlConnection connection
            , SqlTransaction transaction, CommandType commandType, string commandText
            , SqlParameter[] commandParameters)
        {

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                SqlHelper.AttachParameters(command, commandParameters);
            }
        }

        /// <summary>
        ///  准备<c>command</c>参数,建议不使用该函数,本函数不包括<c>SqlTransaction</c>参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        private static void PrepareCommand(SqlCommand command, SqlConnection connection
            , CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// run  a query sets ,and return the effected rows count 
        /// </summary>
        /// <param name="connectionString">connection String </param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// run  a query sets ,and return the effected rows count 
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <param name="commandType">The value of enum <c>CommandType</c></param>
        /// <param name="commandText">Name of the stored procedure in the DB, eg. sp_DoTask or the sql query which may be contained parameters</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc or command text</param>
        /// <returns>An integer indicating return value of executing result</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            int i = -1;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandTimeout = 1800;

                SqlHelper.PrepareCommand(sqlCommand, connection, commandType, commandText, commandParameters);
                i = sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return i;
        }

        /// <summary>
        ///  run  a query sets ,and return the effected rows count 
        /// </summary>
        /// <param name="connection">an SqlConnection object</param>		
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <returns>An integer indicating return value of executing result</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery(connection, commandType, commandText, null);
        }

        /// <summary>
        /// run  a query sets ,and return the effected rows count 
        /// </summary>
        /// <param name="connection">an SqlConnection object</param>		
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of parameters to be passed to the procedure or query</param>
        /// <returns>the effected rows count</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters);
            int j = sqlCommand.ExecuteNonQuery();
            sqlCommand.Parameters.Clear();
            return j;
        }

        /// <summary>
        /// run  a query sets ,and return the effected rows count 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>		
        /// <returns>the effected rows count</returns>		
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// run a sql query,and return the effected rows count 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery(connection, transaction, commandType, commandText, null);
        }

        /// <summary>
        ///  run a sql query,and return the effected rows count 
        /// </summary>
        /// <param name="transaction">the SqlTransaction object</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of parameters to be passed to the procedure or query</param>
        /// <returns>the effected rows count</returns>		
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(transaction.Connection, transaction, commandType, commandText, commandParameters);
        }

        /// <summary>
        ///  run a sql query,and return the effected rows count 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, commandParameters);
            int j = sqlCommand.ExecuteNonQuery();
            sqlCommand.Parameters.Clear();
            return j;
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <returns>Newly instantiated DataSet instance</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc or query string</param>
        /// <returns>Newly instantiated DataSet instance</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            DataSet dataSet;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                dataSet = SqlHelper.ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return dataSet;
        }

        /// <summary>
        ///  there's no parrameters in the query string or stored proceduce
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset(connection, commandType, commandText, null);
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>
        /// <returns>Newly instantiated DataSet instance</returns>
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {

            //			SqlCommand sqlCommand = new SqlCommand ();
            //			SqlHelper.PrepareCommand (sqlCommand, connection, null, commandType, commandText, commandParameters);
            //			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter (sqlCommand);
            //			DataSet dataSet1 = new DataSet ();
            //			sqlDataAdapter.Fill (dataSet1);
            //			sqlCommand.Parameters.Clear ();				
            //			return dataSet1;
            return ExecuteDataset(connection, null, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, transaction, commandType, commandText, null);
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="transaction">the SqlTransaction object,by it we get the SqlConnection object</param>
        /// <param name="connection"></param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>
        /// <returns>Newly instantiated DataSet instance</returns>
        public static DataSet ExecuteDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, commandParameters);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            sqlCommand.Parameters.Clear();
            return dataSet;
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename. 
        /// this is used in the transaction
        /// </summary>
        /// <param name="transaction">the SqlTransaction object,by it we get the SqlConnection object</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>
        /// <returns>Newly instantiated DataSet instance</returns>
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            //			SqlCommand sqlCommand = new SqlCommand ();
            //			SqlHelper.PrepareCommand (sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters);
            //			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter (sqlCommand);
            //			DataSet dataSet = new DataSet ();
            //			sqlDataAdapter.Fill (dataSet);
            //			sqlCommand.Parameters.Clear ();			
            //			return dataSet;
            return ExecuteDataset(transaction.Connection, transaction, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// there's no parrameters in the query string or stored proceduce
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// Creates a DataTable by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <returns>Newly instantiated DataTable instance</returns>
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataTable(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// Creates a DataTable by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc or query string</param>
        /// <returns>Newly instantiated DataTable instance</returns>
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {

            DataTable dataTable;
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                dataTable = SqlHelper.ExecuteDataTable(connection, commandType, commandText, commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return dataTable;
        }

        /// <summary>
        ///  there's no parrameters in the query string or stored proceduce
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataTable(connection, commandType, commandText, null);
        }

        /// <summary>
        /// Creates a DataTable by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionString">Connection String to the associated database</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>
        /// <returns>Newly instantiated DataTable instance</returns>
        public static DataTable ExecuteDataTable(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            sqlCommand.Parameters.Clear();
            return dataTable;
        }

        /// <summary>
        /// there's no parrameters in the query string or stored proceduce
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataTable(transaction, commandType, commandText, null);
        }

        public static DataTable ExecuteDataTable(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataTable(connection, transaction, commandType, commandText, null);
        }

        /// <summary>
        /// Creates a DataTable by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename. 
        /// this is used in the transaction
        /// </summary>
        /// <param name="transaction">the SqlTransaction object,by it we get the SqlConnection object</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>
        /// <returns>Newly instantiated DataTable instance</returns>
        public static DataTable ExecuteDataTable(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            sqlCommand.Parameters.Clear();
            return dataTable;
            //ExecuteDataTable(transaction.Connection,transaction,commandType,commandText,commandParameters);
        }

        /// <summary>
        /// Creates a DataTable by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename. 
        /// this is used in the transaction
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, commandParameters);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            sqlCommand.Parameters.Clear();
            return dataTable;
        }

        /// <summary>
        /// Creates a SqlDataReader by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename. 
        /// this is used in the transaction
        /// </summary>
        /// <param name="connection">a SqlConnection instance<</param>
        /// <param name="transaction">the SqlTransaction object,by it we get the SqlConnection object</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>
        /// <param name="connectionOwnership">the value of enum <c>SqlConnectionOwnership</c></param>
        /// <returns>Newly instantiated SqlDataReader instance</returns>
        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText
            , SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            SqlDataReader sqlDataReader;
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, commandParameters);
            if (connectionOwnership == SqlConnectionOwnership.External)
            {
                sqlDataReader = sqlCommand.ExecuteReader();
            }
            else
            {
                sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            sqlCommand.Parameters.Clear();
            return sqlDataReader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteReader(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlDataReader sqlDataReader;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            try
            {
                sqlDataReader = SqlHelper.ExecuteReader(connection, null, commandType, commandText, commandParameters, 0);
            }
            catch
            {
                connection.Close();
                throw;
            }
            return sqlDataReader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteReader(connection, commandType, commandText, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteReader(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteScalar(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            object obj;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                obj = SqlHelper.ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteScalar(connection, commandType, commandText, null);
        }

        /// <summary>
        /// get the first object by running the stored procedure or query 
        /// </summary>
        /// <param name="connection">a SqlConnection instance<</param>		
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>		
        /// <returns>the first object of the query array value</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters);
            object obj = sqlCommand.ExecuteScalar();
            sqlCommand.Parameters.Clear();
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteScalar(transaction, commandType, commandText, null);
        }

        public static object ExecuteScalar(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteScalar(connection, transaction, commandType, commandText, null);
        }

        /// <summary>
        /// get the first object by running the stored procedure or query 
        /// </summary>
        /// <param name="transaction">the SqlTransaction object</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>		
        /// <returns>the first object of the query array value</returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters);
            object obj = sqlCommand.ExecuteScalar();
            sqlCommand.Parameters.Clear();
            return obj;
        }

        public static object ExecuteScalar(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, commandParameters);
            object obj = sqlCommand.ExecuteScalar();
            sqlCommand.Parameters.Clear();
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteXmlReader(connection, commandType, commandText, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a XmlReader by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename. 		
        /// </summary>
        /// <param name="connection">a SqlConnection instance<</param>		
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>		
        /// <returns>Newly instantiated XmlReader instance</returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters);
            XmlReader xmlReader = sqlCommand.ExecuteXmlReader();
            sqlCommand.Parameters.Clear();
            return xmlReader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteXmlReader(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// Creates a XmlReader by running the stored procedure or query  and placing the results
        /// of the query/proc into the given tablename. 
        /// this is used in the transaction
        /// </summary>		
        /// <param name="transaction">the SqlTransaction object,by it we get the SqlConnection object</param>
        /// <param name="commandType">the value of enum <c>CommandType</c></param>
        /// <param name="commandText">the query string or the stored procedure</param>
        /// <param name="commandParameters">Array of SqlParameter objects containing parameters to the stored proc</param>		
        /// <returns>Newly instantiated XmlReader instance</returns>
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 1800;

            SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters);
            XmlReader xmlReader = sqlCommand.ExecuteXmlReader();
            sqlCommand.Parameters.Clear();
            return xmlReader;
        }

        /// <summary>
        /// Builds a SqlCommand 
        /// </summary>
        /// <param name="storedProcName">  the stored procedure name</param>
        /// <param name="parameters">Array of IDataParameter objects</param>
        /// <returns>A newly instantiated SqlCommand</returns>
        public static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            return BuildQueryCommand(connection, CommandType.StoredProcedure, storedProcName, parameters);
        }

        /// <summary>
        /// Builds a SqlCommand
        /// </summary>
        /// <param name="connection">the sqlConnection</param>
        /// <param name="commandType">the <c>CommandType</c></param>
        /// <param name="comandText">the stored procedure name</param>
        /// <param name="parameters">Array of IDataParameter objects</param>
        /// <returns>A newly instantiated SqlCommand</returns>
        public static SqlCommand BuildQueryCommand(SqlConnection connection, CommandType commandType, string comandText, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(comandText, connection);
            command.CommandTimeout = 1800;
            command.CommandType = commandType;
            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }

        /// <summary>
        /// Create the new instance of the SqlCommand	
        /// </summary>
        /// <param name="storedProcName">Name of the stored procedure in the DataBase</param>
        /// <param name="parameters">IDataParameter Array</param>
        /// <returns>the SqlCommand instance</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);

            command.Parameters.Add(new SqlParameter("@ReturnValue",
                SqlDbType.Int,
                4, /* Size */
                ParameterDirection.ReturnValue,
                false, /* is nullable */
                0, /* byte precision */
                0, /* byte scale */
                string.Empty,
                DataRowVersion.Default,
                null));

            return command;
        }

        /// <summary>
        /// Runs a stored procedure, which  returns an integer indicating the return value of the
        /// stored procedure, and also returns the value of the RowsAffected by the stored procedure
        /// </summary>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="parameters">Array of IDataParameter objects</param>
        /// <param name="rowsAffected">Number of rows affected by the stored procedure.</param>
        /// <returns>  return an integer value that indicate the stored procedure state </returns>
        public static int RunProcedure(SqlConnection connection, string storedProcName, IDataParameter[] parameters, out int affectedRows)
        {
            SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
            affectedRows = command.ExecuteNonQuery();
            int result = (int)command.Parameters["@ReturnValue"].Value;
            return result;
        }

        public static int RunProcedure(SqlConnection connection, SqlTransaction trans, string storedProcName, IDataParameter[] parameters, out int affectedRows)
        {
            SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
            command.Transaction = trans;
            affectedRows = command.ExecuteNonQuery();
            int result = (int)command.Parameters["@ReturnValue"].Value;
            return result;
        }

        /// <summary>
        ///create a SqlDataReader containing the result of the stored procedure.
        /// </summary>
        /// <param name="storedProcName">name of the stored procedure</param>
        /// <param name="parameters">array of parameters to be passed to the procedure</param>
        /// <returns>a newly instantiated SqlDataReader</returns>
        public static SqlDataReader RunProcedure(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlDataReader returnReader;
            if (ConnectionState.Open != connection.State)
                connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;

            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }

        /// <summary>
        ///create a SqlDataReader containing the result of the stored procedure.
        /// </summary>
        /// <param name="selectsql">select condition</param>
        /// <returns>SqlDataReader</returns>		
        public static SqlDataReader RunProcedure(SqlConnection connection, string selectsql)
        {
            SqlDataReader returnReader;
            if (ConnectionState.Open != connection.State)
                connection.Open();
            SqlCommand command = new SqlCommand(selectsql, connection);
            command.CommandTimeout = 1800;
            command.CommandType = CommandType.Text;

            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }

        /// <summary>
        /// create a new dataset which have been exists in the class ,
        /// and named use the <c>tableName</c>,
        /// </summary>
        /// <param name="storedProcName">the Stored procedure in db </param>
        /// <param name="parameters">the parameters of the procedure </param>
        /// <param name="tableName"></param>
        /// <returns>a newly instantiated DataSet object</returns>
        public static DataSet RunProcedure(SqlConnection connection, string storedProcName, IDataParameter[] parameters, string tableName)
        {
            DataSet dataSet = new DataSet();
            if (ConnectionState.Open != connection.State)
                connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);
            connection.Close();
            return dataSet;
        }

        public static DataSet RunProcedure(SqlConnection connection, SqlTransaction trans, string storedProcName, IDataParameter[] parameters, string tableName)
        {
            DataSet dataSet = new DataSet();
            //			if(ConnectionState.Open  !=connection.State )
            //				connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDA.SelectCommand.Transaction = trans;
            sqlDA.Fill(dataSet, tableName);
            //connection.Close();
            return dataSet;
        }

        /// <summary>
        /// fill the dataRows to a dataset which have been exists in the class ,
        /// and named use the <c>tableName</c>
        /// </summary>
        /// <param name="storedProcName">the Stored procedure in db </param>
        /// <param name="parameters">the parameters of the procedure </param>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        public static void RunProcedure(SqlConnection connection, string storedProcName, IDataParameter[] parameters, DataSet dataSet, string tableName)
        {
            if (ConnectionState.Open != connection.State)
                connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildIntCommand(connection, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);
            connection.Close();
        }

        /// <summary>
        /// 填充记录集
        /// </summary>
        /// <param name="ds">dataSet</param>
        /// <param name="strSql">查询语句</param>
        /// <param name="tbNm">数据表名</param>
        public static void FillData(SqlConnection connection, ref DataSet ds, string strSql, string tbNm)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strSql, connection);
            sqlDataAdapter.Fill(ds, tbNm);
        }
    }

}
