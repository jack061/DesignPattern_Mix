using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using WZX.DataBase.Common;
namespace WZX.Busines.DAL  
{
	 	//Tb_UserInfo
		public  class Tb_UserInfoDal:IDisposable
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
        
        public Tb_UserInfoDal (SqlConnection con)
        {
            this.sqlCon = con;
        }
   		
   		#region Function
   		
		public bool Exists()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Tb_UserInfo");
			strSql.Append(" where ");
						SqlParameter[] parameters = {
			};

			return Convert.ToInt32(SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters)) != 0;
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(WZX.Model.Tb_UserInfoMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tb_UserInfo(");			
            strSql.Append("UserCode,Name,RegisterType,HjgPeople,Post,ZfjgSign,UserType,IsFwsk,ComType,address,Phone");
			strSql.Append(") values (");
            strSql.Append("@UserCode,@Name,@RegisterType,@HjgPeople,@Post,@ZfjgSign,@UserType,@IsFwsk,@ComType,@address,@Phone");            
            strSql.Append(") ");            
            		
			SqlParameter[] parameters = {
			            new SqlParameter("@UserCode", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@Name", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@RegisterType", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@HjgPeople", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@Post", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZfjgSign", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@UserType", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@IsFwsk", SqlDbType.Bit,1) ,            
                        new SqlParameter("@ComType", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@address", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Phone", SqlDbType.NVarChar,20)             
              
            };
			            
            parameters[0].Value = model.UserCode;                        
            parameters[1].Value = model.Name;                        
            parameters[2].Value = model.RegisterType;                        
            parameters[3].Value = model.HjgPeople;                        
            parameters[4].Value = model.Post;                        
            parameters[5].Value = model.ZfjgSign;                        
            parameters[6].Value = model.UserType;                        
            parameters[7].Value = model.IsFwsk;                        
            parameters[8].Value = model.ComType;                        
            parameters[9].Value = model.address;                        
            parameters[10].Value = model.Phone;                        
			            SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters);	
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.Tb_UserInfoMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Tb_UserInfo set ");
			                        
            strSql.Append(" UserCode = @UserCode , ");                                    
            strSql.Append(" Name = @Name , ");                                    
            strSql.Append(" RegisterType = @RegisterType , ");                                    
            strSql.Append(" HjgPeople = @HjgPeople , ");                                    
            strSql.Append(" Post = @Post , ");                                    
            strSql.Append(" ZfjgSign = @ZfjgSign , ");                                    
            strSql.Append(" UserType = @UserType , ");                                    
            strSql.Append(" IsFwsk = @IsFwsk , ");                                    
            strSql.Append(" ComType = @ComType , ");                                    
            strSql.Append(" address = @address , ");                                    
            strSql.Append(" Phone = @Phone  ");            			
			strSql.Append(" where  ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@UserCode", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@Name", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@RegisterType", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@HjgPeople", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@Post", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZfjgSign", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@UserType", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@IsFwsk", SqlDbType.Bit,1) ,            
                        new SqlParameter("@ComType", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@address", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Phone", SqlDbType.NVarChar,20)             
              
            };
						            
            parameters[0].Value = model.UserCode;                        
            parameters[1].Value = model.Name;                        
            parameters[2].Value = model.RegisterType;                        
            parameters[3].Value = model.HjgPeople;                        
            parameters[4].Value = model.Post;                        
            parameters[5].Value = model.ZfjgSign;                        
            parameters[6].Value = model.UserType;                        
            parameters[7].Value = model.IsFwsk;                        
            parameters[8].Value = model.ComType;                        
            parameters[9].Value = model.address;                        
            parameters[10].Value = model.Phone;                        
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
			strSql.Append("delete from Tb_UserInfo ");
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
		public WZX.Model.Tb_UserInfoMod GetModel()
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select UserCode, Name, RegisterType, HjgPeople, Post, ZfjgSign, UserType, IsFwsk, ComType, address, Phone  ");			
			strSql.Append("  from Tb_UserInfo ");
			strSql.Append(" where ");
						SqlParameter[] parameters = {
			};

			
			WZX.Model.Tb_UserInfoMod model=new WZX.Model.Tb_UserInfoMod();
			DataSet ds=SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
			
			if(ds.Tables[0].Rows.Count>0)
			{
																model.UserCode= ds.Tables[0].Rows[0]["UserCode"].ToString();
																																model.Name= ds.Tables[0].Rows[0]["Name"].ToString();
																																model.RegisterType= ds.Tables[0].Rows[0]["RegisterType"].ToString();
																																model.HjgPeople= ds.Tables[0].Rows[0]["HjgPeople"].ToString();
																																model.Post= ds.Tables[0].Rows[0]["Post"].ToString();
																																model.ZfjgSign= ds.Tables[0].Rows[0]["ZfjgSign"].ToString();
																																model.UserType= ds.Tables[0].Rows[0]["UserType"].ToString();
																																												if(ds.Tables[0].Rows[0]["IsFwsk"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["IsFwsk"].ToString()=="1")||(ds.Tables[0].Rows[0]["IsFwsk"].ToString().ToLower()=="true"))
					{
					model.IsFwsk= true;
					}
					else
					{
					model.IsFwsk= false;
					}
				}
																				model.ComType= ds.Tables[0].Rows[0]["ComType"].ToString();
																																model.address= ds.Tables[0].Rows[0]["address"].ToString();
																																model.Phone= ds.Tables[0].Rows[0]["Phone"].ToString();
																										
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
			strSql.Append(" FROM Tb_UserInfo ");
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
			strSql.Append(" FROM Tb_UserInfo ");
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

