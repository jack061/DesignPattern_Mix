using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using WZX.DataBase.Common;
namespace WZX.Busines.DAL  
{
	 	//Information
		public  class InformationDal:IDisposable
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
        
        public InformationDal (SqlConnection con)
        {
            this.sqlCon = con;
        }
   		
   		#region Function
   		
		public bool Exists(long ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Information");
			strSql.Append(" where ");
			                                       strSql.Append(" ID = @ID  ");
                            			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt)
			};
			parameters[0].Value = ID;

			return Convert.ToInt32(SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters)) != 0;
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(WZX.Model.InformationMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Information(");			
            strSql.Append("InfoType,UserType,Title,ContentRtf,AttachmentCount,AttachmentData,SendTime");
			strSql.Append(") values (");
            strSql.Append("@InfoType,@UserType,@Title,@ContentRtf,@AttachmentCount,@AttachmentData,@SendTime");            
            strSql.Append(") ");            
            strSql.Append(";select @@IDENTITY");		
			SqlParameter[] parameters = {
			            new SqlParameter("@InfoType", SqlDbType.NVarChar,10) ,            
                        new SqlParameter("@UserType", SqlDbType.NVarChar,10) ,            
                        new SqlParameter("@Title", SqlDbType.NVarChar,30) ,            
                        new SqlParameter("@ContentRtf", SqlDbType.NVarChar,-1) ,            
                        new SqlParameter("@AttachmentCount", SqlDbType.Int,4) ,            
                        new SqlParameter("@AttachmentData", SqlDbType.Image) ,            
                        new SqlParameter("@SendTime", SqlDbType.DateTime)             
              
            };
			            
            parameters[0].Value = model.InfoType;                        
            parameters[1].Value = model.UserType;                        
            parameters[2].Value = model.Title;                        
            parameters[3].Value = model.ContentRtf;                        
            parameters[4].Value = model.AttachmentCount;                        
            parameters[5].Value = model.AttachmentData;                        
            parameters[6].Value = model.SendTime;                        
			   
			object obj = SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters);	
			if (obj == null)
			{
				return 0;
			}
			else
			{
				                                    
            	return Convert.ToInt64(obj);
                                                  
			}			   
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.InformationMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Information set ");
			                                                
            strSql.Append(" InfoType = @InfoType , ");                                    
            strSql.Append(" UserType = @UserType , ");                                    
            strSql.Append(" Title = @Title , ");                                    
            strSql.Append(" ContentRtf = @ContentRtf , ");                                    
            strSql.Append(" AttachmentCount = @AttachmentCount , ");                                    
            strSql.Append(" AttachmentData = @AttachmentData , ");                                    
            strSql.Append(" SendTime = @SendTime  ");            			
			strSql.Append(" where ID=@ID ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@ID", SqlDbType.BigInt,8) ,            
                        new SqlParameter("@InfoType", SqlDbType.NVarChar,10) ,            
                        new SqlParameter("@UserType", SqlDbType.NVarChar,10) ,            
                        new SqlParameter("@Title", SqlDbType.NVarChar,30) ,            
                        new SqlParameter("@ContentRtf", SqlDbType.NVarChar,-1) ,            
                        new SqlParameter("@AttachmentCount", SqlDbType.Int,4) ,            
                        new SqlParameter("@AttachmentData", SqlDbType.Image) ,            
                        new SqlParameter("@SendTime", SqlDbType.DateTime)             
              
            };
						            
            parameters[0].Value = model.ID;                        
            parameters[1].Value = model.InfoType;                        
            parameters[2].Value = model.UserType;                        
            parameters[3].Value = model.Title;                        
            parameters[4].Value = model.ContentRtf;                        
            parameters[5].Value = model.AttachmentCount;                        
            parameters[6].Value = model.AttachmentData;                        
            parameters[7].Value = model.SendTime;                        
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
		public bool Delete(long ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Information ");
			strSql.Append(" where ID=@ID");
						SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt)
			};
			parameters[0].Value = ID;


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
		/// 批量删除一批数据
		/// </summary>
		public bool DeleteList(string IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Information ");
			strSql.Append(" where ID in ("+IDlist + ")  ");
			int rows=SqlHelper.ExecuteNonQuery(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
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
		public WZX.Model.InformationMod GetModel(long ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ID, InfoType, UserType, Title, ContentRtf, AttachmentCount, AttachmentData, SendTime  ");			
			strSql.Append("  from Information ");
			strSql.Append(" where ID=@ID");
						SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt)
			};
			parameters[0].Value = ID;

			
			WZX.Model.InformationMod model=new WZX.Model.InformationMod();
			DataSet ds=SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
			
			if(ds.Tables[0].Rows.Count>0)
			{
												if(ds.Tables[0].Rows[0]["ID"].ToString()!="")
				{
					model.ID=long.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
				}
																																				model.InfoType= ds.Tables[0].Rows[0]["InfoType"].ToString();
																																model.UserType= ds.Tables[0].Rows[0]["UserType"].ToString();
																																model.Title= ds.Tables[0].Rows[0]["Title"].ToString();
																																model.ContentRtf= ds.Tables[0].Rows[0]["ContentRtf"].ToString();
																												if(ds.Tables[0].Rows[0]["AttachmentCount"].ToString()!="")
				{
					model.AttachmentCount=int.Parse(ds.Tables[0].Rows[0]["AttachmentCount"].ToString());
				}
																																								if(ds.Tables[0].Rows[0]["AttachmentData"].ToString()!="")
				{
					model.AttachmentData= (byte[])ds.Tables[0].Rows[0]["AttachmentData"];
				}
																								if(ds.Tables[0].Rows[0]["SendTime"].ToString()!="")
				{
					model.SendTime=DateTime.Parse(ds.Tables[0].Rows[0]["SendTime"].ToString());
				}
																														
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
			strSql.Append(" FROM Information ");
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
			strSql.Append(" FROM Information ");
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

