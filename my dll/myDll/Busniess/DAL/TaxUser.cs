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
	 	//TaxUser
		public  class TaxUserDal:IDisposable
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
        
        public TaxUserDal (SqlConnection con)
        {
            this.sqlCon = con;
        }
   		
   		#region Function
   		
		public bool Exists(string UserCode)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from TaxUser");
			strSql.Append(" where ");
			                                       strSql.Append(" UserCode = @UserCode  ");
                            			SqlParameter[] parameters = {
					new SqlParameter("@UserCode", SqlDbType.VarChar,30)			};
			parameters[0].Value = UserCode;

			return Convert.ToInt32(SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters)) != 0;
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(WZX.Model.TaxUserMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into TaxUser(");			
            strSql.Append("UserCode,Name,Pwd,UserType,IsValid");
			strSql.Append(") values (");
            strSql.Append("@UserCode,@Name,@Pwd,@UserType,@IsValid");            
            strSql.Append(") ");            
            		
			SqlParameter[] parameters = {
			            new SqlParameter("@UserCode", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@Name", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Pwd", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@UserType", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@IsValid", SqlDbType.Bit,1)             
              
            };
			            
            parameters[0].Value = model.UserCode;                        
            parameters[1].Value = model.Name;                        
            parameters[2].Value = model.Pwd;                        
            parameters[3].Value = model.UserType;                        
            parameters[4].Value = model.IsValid;                        
			            SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters);	
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.TaxUserMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update TaxUser set ");
			                        
            strSql.Append(" UserCode = @UserCode , ");                                    
            strSql.Append(" Name = @Name , ");                                    
            strSql.Append(" Pwd = @Pwd , ");                                    
            strSql.Append(" UserType = @UserType , ");                                    
            strSql.Append(" IsValid = @IsValid  ");            			
			strSql.Append(" where UserCode=@UserCode  ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@UserCode", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@Name", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Pwd", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@UserType", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@IsValid", SqlDbType.Bit,1)             
              
            };
						            
            parameters[0].Value = model.UserCode;                        
            parameters[1].Value = model.Name;                        
            parameters[2].Value = model.Pwd;                        
            parameters[3].Value = model.UserType;                        
            parameters[4].Value = model.IsValid;                        
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
		public bool Delete(string UserCode)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TaxUser ");
			strSql.Append(" where UserCode=@UserCode ");
						SqlParameter[] parameters = {
					new SqlParameter("@UserCode", SqlDbType.VarChar,30)			};
			parameters[0].Value = UserCode;


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
		public WZX.Model.TaxUserMod GetModel(string UserCode)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select UserCode, Name, Pwd, UserType, IsValid  ");			
			strSql.Append("  from TaxUser ");
			strSql.Append(" where UserCode=@UserCode ");
						SqlParameter[] parameters = {
					new SqlParameter("@UserCode", SqlDbType.VarChar,30)			};
			parameters[0].Value = UserCode;

			
			WZX.Model.TaxUserMod model=new WZX.Model.TaxUserMod();
			DataSet ds=SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
			
			if(ds.Tables[0].Rows.Count>0)
			{
																model.UserCode= ds.Tables[0].Rows[0]["UserCode"].ToString();
																																model.Name= ds.Tables[0].Rows[0]["Name"].ToString();
																																model.Pwd= ds.Tables[0].Rows[0]["Pwd"].ToString();
																																model.UserType= ds.Tables[0].Rows[0]["UserType"].ToString();
																																												if(ds.Tables[0].Rows[0]["IsValid"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["IsValid"].ToString()=="1")||(ds.Tables[0].Rows[0]["IsValid"].ToString().ToLower()=="true"))
					{
					model.IsValid= true;
					}
					else
					{
					model.IsValid= false;
					}
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
			strSql.Append(" FROM TaxUser ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
		}
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList1(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct UserType ");
            strSql.Append(" FROM TaxUser ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
			strSql.Append(" FROM TaxUser ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
		}
		
        #endregion

        #region 自定义
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public string GetAllComList(int row, int page, string strWhere, string order, string sort)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TaxUser ");
            StringBuilder strCount = new StringBuilder();
            strCount.Append("select count(*) from TaxUser");
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
        /// 获取客户上线次数
        /// </summary>
        public string GetLivenessList(int row, int page, string strWhere, string strWhere1,string strWhere2, string order, string sort)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select * from (select t1.UserCode,t1.Name,t1.UserType, users.HjgPeople, (case when t2.counts>0 then t2.counts else 0 end) as counts from (select * from dbo.TaxUser  where IsValid =0 {0})t1 left join (select UserCode, COUNT(*) as  counts from  dbo.UserEventLog where EventType='上线' {1}  group by UserCode)t2 on t1.UserCode = t2.UserCode left join dbo.Tb_UserInfo users on t1.UserCode = users.UserCode) t3 where 1=1 {2}", strWhere, strWhere1, strWhere2));
            StringBuilder strCount = new StringBuilder();
            strCount.Append(string.Format("select count(*) from (select t1.UserCode,t1.Name,t1.UserType,  users.HjgPeople, (case when t2.counts>0 then t2.counts else 0 end) as counts from (select * from dbo.TaxUser  where IsValid =0 {0})t1 left join (select UserCode, COUNT(*) as  counts from  dbo.UserEventLog where EventType='上线' {1}  group by UserCode)t2 on t1.UserCode = t2.UserCode left join dbo.Tb_UserInfo users on t1.UserCode = users.UserCode) t3 where 1=1 {2}", strWhere, strWhere1, strWhere2));
           
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
        //注销企业
        public string cancel(string UserCode)
        {
            string sql = "update TaxUser  set IsValid=1 where UserCode='" + UserCode + "'";
            if (SqlHelper.ExecuteNonQuery(sqlCon, CommandType.Text, sql) > 0)
            {
                return "{\"flag\":true,\"msg\":\"企业注销成功！\"}";
            }
            else return "{\"flag\":false,\"msg\":\"企业注销失败！\"}";
        }
        //批量注销企业
        public string cancelD(string UserCode)
        {
            string sql = "update TaxUser  set IsValid=1 where UserCode in (" + UserCode + ")";
            if (SqlHelper.ExecuteNonQuery(sqlCon, CommandType.Text, sql) > 0)
            {
                return "{\"flag\":true,\"msg\":\"企业注销成功！\"}";
            }
            else return "{\"flag\":false,\"msg\":\"企业注销失败！\"}";
        }
        //批量启用企业
        public string startD(string UserCode)
        {
            string sql = "update TaxUser  set IsValid=0 where UserCode in (" + UserCode + ")";
            if (SqlHelper.ExecuteNonQuery(sqlCon, CommandType.Text, sql) > 0)
            {
                return "{\"flag\":true,\"msg\":\"企业启用成功！\"}";
            }
            else return "{\"flag\":false,\"msg\":\"企业启用失败！\"}";
        }
            //启用企业
        public string start(string UserCode)
        {
            string sql = "update TaxUser  set IsValid=0 where UserCode='" + UserCode + "'";
            if (SqlHelper.ExecuteNonQuery(sqlCon, CommandType.Text, sql) > 0)
            {
                return "{\"flag\":true,\"msg\":\"企业启用成功！\"}";
            }
            else return "{\"flag\":false,\"msg\":\"企业启用失败！\"}";
        }

        //重置企业密码
        public string Reset(string UserCode)
        {
            string sql = "update TaxUser  set Pwd='c4ca4238a0b92382dcc509a6f75849b' where UserCode='" + UserCode + "'";
            if (SqlHelper.ExecuteNonQuery(sqlCon, CommandType.Text, sql) > 0)
            {
                return "{\"flag\":true,\"msg\":\"密码重置成功！\"}";
            }
            else return "{\"flag\":false,\"msg\":\"密码重置失败！\"}";
        }
        #endregion
    }
}

