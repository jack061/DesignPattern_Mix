using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using WZX.DataBase.Common;
namespace WZX.Busines.DAL  
{
	 	//t_qiye
		public  class t_qiyeDal:IDisposable
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
        
        public t_qiyeDal (SqlConnection con)
        {
            this.sqlCon = con;
        }
   		
   		#region Function
   		
		public bool Exists()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from t_qiye");
			strSql.Append(" where ");
						SqlParameter[] parameters = {
			};

			return Convert.ToInt32(SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters)) != 0;
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(WZX.Model.t_qiyeMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into t_qiye(");			
            strSql.Append("qiyemingcheng,zhuangtai");
			strSql.Append(") values (");
            strSql.Append("@qiyemingcheng,@zhuangtai");            
            strSql.Append(") ");            
            		
			SqlParameter[] parameters = {
			            new SqlParameter("@qiyemingcheng", SqlDbType.NChar,100) ,            
                        new SqlParameter("@zhuangtai", SqlDbType.NChar,10)             
              
            };
			            
            parameters[0].Value = model.qiyemingcheng;                        
            parameters[1].Value = model.zhuangtai;                        
			            SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters);	
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.t_qiyeMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update t_qiye set ");
			                        
            strSql.Append(" qiyemingcheng = @qiyemingcheng , ");                                    
            strSql.Append(" zhuangtai = @zhuangtai  ");            			
			strSql.Append(" where  ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@qiyemingcheng", SqlDbType.NChar,100) ,            
                        new SqlParameter("@zhuangtai", SqlDbType.NChar,10)             
              
            };
						            
            parameters[0].Value = model.qiyemingcheng;                        
            parameters[1].Value = model.zhuangtai;                        
            int rows=SqlHelper.ExecuteNonQuery(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from t_qiye ");
			strSql.Append(" where ");
						SqlParameter[] parameters = {
			};


			int rows=SqlHelper.ExecuteNonQuery(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
				
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public WZX.Model.t_qiyeMod GetModel()
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select qiyemingcheng, zhuangtai  ");			
			strSql.Append("  from t_qiye ");
			strSql.Append(" where ");
						SqlParameter[] parameters = {
			};

			
			WZX.Model.t_qiyeMod model=new WZX.Model.t_qiyeMod();
			DataSet ds=SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
			
			if(ds.Tables[0].Rows.Count>0)
			{
																model.qiyemingcheng= ds.Tables[0].Rows[0]["qiyemingcheng"].ToString();
																																model.zhuangtai= ds.Tables[0].Rows[0]["zhuangtai"].ToString();
																										
				return model;
			}
			else
			{
				return null;
			}
		}
		
		
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * ");
			strSql.Append(" FROM t_qiye ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
		}
		
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" * ");
			strSql.Append(" FROM t_qiye ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
		}
		
        #endregion
   
	}
}

