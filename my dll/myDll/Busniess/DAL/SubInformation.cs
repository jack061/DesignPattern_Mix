using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using WZX.DataBase.Common;
using RM.Common.DotNetJson;
using RM.Busines;
namespace WZX.Busines.DAL  
{
	 	//SubInformation
		public  class SubInformationDal:IDisposable
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
        
        public SubInformationDal (SqlConnection con)
        {
            this.sqlCon = con;
        }
   		
   		#region Function
   		
		public bool Exists(long ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from SubInformation");
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
		public long Add(WZX.Model.SubInformationMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into SubInformation(");			
            strSql.Append("InformationID,UserCode,FengxianContent,FengxianDate,IsGet,IsDone,CreateTime");
			strSql.Append(") values (");
            strSql.Append("@InformationID,@UserCode,@FengxianContent,@FengxianDate,@IsGet,@IsDone,@CreateTime");            
            strSql.Append(") ");            
            strSql.Append(";select @@IDENTITY");		
			SqlParameter[] parameters = {
			            new SqlParameter("@InformationID", SqlDbType.BigInt,8) ,            
                        new SqlParameter("@UserCode", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@FengxianContent", SqlDbType.NVarChar,-1) ,            
                        new SqlParameter("@FengxianDate", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@IsGet", SqlDbType.Bit,1) ,            
                        new SqlParameter("@IsDone", SqlDbType.Bit,1) ,            
                        new SqlParameter("@CreateTime", SqlDbType.DateTime)             
              
            };
			            
            parameters[0].Value = model.InformationID;                        
            parameters[1].Value = model.UserCode;                        
            parameters[2].Value = model.FengxianContent;                        
            parameters[3].Value = model.FengxianDate;                        
            parameters[4].Value = model.IsGet;                        
            parameters[5].Value = model.IsDone;                        
            parameters[6].Value = model.CreateTime;                        
			   
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
		public bool Update(WZX.Model.SubInformationMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update SubInformation set ");
			                                                
            strSql.Append(" InformationID = @InformationID , ");                                    
            strSql.Append(" UserCode = @UserCode , ");                                    
            strSql.Append(" FengxianContent = @FengxianContent , ");                                    
            strSql.Append(" FengxianDate = @FengxianDate , ");                                    
            strSql.Append(" IsGet = @IsGet , ");                                    
            strSql.Append(" IsDone = @IsDone , ");                                    
            strSql.Append(" CreateTime = @CreateTime  ");            			
			strSql.Append(" where ID=@ID ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@ID", SqlDbType.BigInt,8) ,            
                        new SqlParameter("@InformationID", SqlDbType.BigInt,8) ,            
                        new SqlParameter("@UserCode", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@FengxianContent", SqlDbType.NVarChar,-1) ,            
                        new SqlParameter("@FengxianDate", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@IsGet", SqlDbType.Bit,1) ,            
                        new SqlParameter("@IsDone", SqlDbType.Bit,1) ,            
                        new SqlParameter("@CreateTime", SqlDbType.DateTime)             
              
            };
						            
            parameters[0].Value = model.ID;                        
            parameters[1].Value = model.InformationID;                        
            parameters[2].Value = model.UserCode;                        
            parameters[3].Value = model.FengxianContent;                        
            parameters[4].Value = model.FengxianDate;                        
            parameters[5].Value = model.IsGet;                        
            parameters[6].Value = model.IsDone;                        
            parameters[7].Value = model.CreateTime;                        
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
			strSql.Append("delete from SubInformation ");
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
			strSql.Append("delete from SubInformation ");
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
		public WZX.Model.SubInformationMod GetModel(long ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ID, InformationID, UserCode, FengxianContent, FengxianDate, IsGet, IsDone, CreateTime  ");			
			strSql.Append("  from SubInformation ");
			strSql.Append(" where ID=@ID");
						SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt)
			};
			parameters[0].Value = ID;

			
			WZX.Model.SubInformationMod model=new WZX.Model.SubInformationMod();
			DataSet ds=SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
			
			if(ds.Tables[0].Rows.Count>0)
			{
												if(ds.Tables[0].Rows[0]["ID"].ToString()!="")
				{
					model.ID=long.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
				}
																																if(ds.Tables[0].Rows[0]["InformationID"].ToString()!="")
				{
					model.InformationID=long.Parse(ds.Tables[0].Rows[0]["InformationID"].ToString());
				}
																																				model.UserCode= ds.Tables[0].Rows[0]["UserCode"].ToString();
																																model.FengxianContent= ds.Tables[0].Rows[0]["FengxianContent"].ToString();
																																model.FengxianDate= ds.Tables[0].Rows[0]["FengxianDate"].ToString();
																																												if(ds.Tables[0].Rows[0]["IsGet"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["IsGet"].ToString()=="1")||(ds.Tables[0].Rows[0]["IsGet"].ToString().ToLower()=="true"))
					{
					model.IsGet= true;
					}
					else
					{
					model.IsGet= false;
					}
				}
																																if(ds.Tables[0].Rows[0]["IsDone"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["IsDone"].ToString()=="1")||(ds.Tables[0].Rows[0]["IsDone"].ToString().ToLower()=="true"))
					{
					model.IsDone= true;
					}
					else
					{
					model.IsDone= false;
					}
				}
																if(ds.Tables[0].Rows[0]["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
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
			strSql.Append(" FROM SubInformation ");
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
			strSql.Append(" FROM SubInformation ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
		}
		
        #endregion

        #region 自定义
        //消息接收完成表
        public DataSet GetMessage(string strWhere)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select t1.IsGetT,t2.IsGetF,t3.IsDoneT,t4.IsDoneF from (select COUNT(*) as IsGetT from dbo.SubInformation  where IsGet =1 {0}) t1, (select COUNT(*) as IsGetF from dbo.SubInformation  where IsGet =0 {0}) t2, (select COUNT(*) as IsDoneT from dbo.SubInformation  where IsDone =1 {0}) t3, (select COUNT(*) as IsDoneF from dbo.SubInformation  where IsDone =0 {0}) t4 ", strWhere));
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, sb.ToString(), null);
        }
        //消息接收表
        public DataSet GetReceive(string strWhere)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select t1.IsGetT,t2.IsGetF from(select COUNT(*) as IsGetT from dbo.SubInformation sub left join dbo.Information inf on sub.InformationID = inf.ID where IsGet =1 {0}) t1 ,(select COUNT(*) as IsGetF from dbo.SubInformation sub left join dbo.Information inf on sub.InformationID = inf.ID where IsGet =0 {0})t2 ", strWhere));
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, sb.ToString(), null);
        }
        //风险完成率
        public DataSet GetRisk(string strWhere)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select t1.IsDoneT,t2.IsDoneF from (select COUNT(*) as IsDoneT from  dbo.SubInformation sub left join dbo.Information inf on sub.InformationID = inf.ID where inf.InfoType='风险提醒' and sub.IsDone=1 {0})t1, (select COUNT(*) as IsDoneF from  dbo.SubInformation sub left join dbo.Information inf on sub.InformationID = inf.ID where inf.InfoType='风险提醒' and sub.IsDone=0 {0})t2 ", strWhere));
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, sb.ToString(), null);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public string GetReceiveList(int row, int page, string strWhere, string order, string sort)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select tax.UserCode,tax.Name,tax.UserType,inf.InfoType,inf.Title,sub.ID,sub.CreateTime,sub.IsGet from dbo.SubInformation sub left join dbo.TaxUser tax on sub.UserCode=tax.UserCode  left join dbo.Information inf on sub.InformationID = inf.ID ");
            StringBuilder strCount = new StringBuilder();
            strCount.Append("select count(*) from  dbo.SubInformation sub left join dbo.TaxUser tax on sub.UserCode=tax.UserCode  left join dbo.Information inf on sub.InformationID = inf.ID ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
                strCount.Append(" where " + strWhere);
            }
            DataTable dt = DataFactory.SqlDataBaseExpand().ExesqlDT(getPageSql1(strSql.ToString(), sort, order, page, row));
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                int count = int.Parse(DataFactory.SqlDataBaseExpand().ExesqlDT(strCount.ToString()).Rows[0][0].ToString());
                sb.Append(JsonHelper.ToJson(dt, "rows"));
                sb.Insert(1, "\"total\":" + count + ",");
            }
            return sb.ToString();

        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public string GetRiskList(int row, int page, string strWhere, string order, string sort)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select tax.UserCode,tax.Name,tax.UserType,inf.InfoType,inf.Title,sub.ID,sub.CreateTime,sub.IsDone from dbo.SubInformation sub left join dbo.TaxUser tax on sub.UserCode=tax.UserCode  left join dbo.Information inf on sub.InformationID = inf.ID ");
            StringBuilder strCount = new StringBuilder();
            strCount.Append("select count(*) from  dbo.SubInformation sub left join dbo.TaxUser tax on sub.UserCode=tax.UserCode  left join dbo.Information inf on sub.InformationID = inf.ID ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
                strCount.Append(" where " + strWhere);
            }
            DataTable dt = DataFactory.SqlDataBaseExpand().ExesqlDT(getPageSql1(strSql.ToString(), sort, order, page, row));
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count == 0)
            {
                sb.Append("{\"total\":0,\"rows\":[]}");
            }
            else
            {
                int count = int.Parse(DataFactory.SqlDataBaseExpand().ExesqlDT(strCount.ToString()).Rows[0][0].ToString());
                sb.Append(JsonHelper.ToJson(dt, "rows"));
                sb.Insert(1, "\"total\":" + count + ",");
            }
            return sb.ToString();

        }
        //通用拼接分页查询字符串的方法
        public string getPageSql1(string oldSql, string sort, string orderType, int page, int rows)
        {
            StringBuilder sbNewSql = new StringBuilder(string.Empty);

            sbNewSql.Append(string.Format(@"select * from (  select tb1.*,Row_Number() over(order by {0} {1}) as rownum  
from  ({2}) tb1 )  tb2 where tb2.rownum>{3} and tb2.rownum<={4} ", sort, orderType, oldSql, (page - 1) * rows, (page) * rows));

            return sbNewSql.ToString();
        }
        #endregion

    }
}

