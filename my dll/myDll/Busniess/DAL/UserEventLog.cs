using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using WZX.DataBase.Common;
using RM.Common.DotNetJson;
using WZX.Busines;
using RM.Busines;

namespace WZX.Busines.DAL  
{
	 	//UserEventLog
		public  class UserEventLogDal:IDisposable
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
        
        public UserEventLogDal (SqlConnection con)
        {
            this.sqlCon = con;
        }
   		
   		#region Function
   		
		public bool Exists(int AutoID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from UserEventLog");
			strSql.Append(" where ");
			                                       strSql.Append(" AutoID = @AutoID  ");
                            			SqlParameter[] parameters = {
					new SqlParameter("@AutoID", SqlDbType.Int,4)
			};
			parameters[0].Value = AutoID;

			return Convert.ToInt32(SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters)) != 0;
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(WZX.Model.UserEventLogMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into UserEventLog(");			
            strSql.Append("UserCode,EventType,EventTime");
			strSql.Append(") values (");
            strSql.Append("@UserCode,@EventType,@EventTime");            
            strSql.Append(") ");            
            strSql.Append(";select @@IDENTITY");		
			SqlParameter[] parameters = {
			            new SqlParameter("@UserCode", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@EventType", SqlDbType.NVarChar,10) ,            
                        new SqlParameter("@EventTime", SqlDbType.DateTime)             
              
            };
			            
            parameters[0].Value = model.UserCode;                        
            parameters[1].Value = model.EventType;                        
            parameters[2].Value = model.EventTime;                        
			   
			object obj = SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters);	
			if (obj == null)
			{
				return 0;
			}
			else
			{
				                    
            	return Convert.ToInt32(obj);
                                                                  
			}			   
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.UserEventLogMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UserEventLog set ");
			                                                
            strSql.Append(" UserCode = @UserCode , ");                                    
            strSql.Append(" EventType = @EventType , ");                                    
            strSql.Append(" EventTime = @EventTime  ");            			
			strSql.Append(" where AutoID=@AutoID ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@AutoID", SqlDbType.Int,4) ,            
                        new SqlParameter("@UserCode", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@EventType", SqlDbType.NVarChar,10) ,            
                        new SqlParameter("@EventTime", SqlDbType.DateTime)             
              
            };
						            
            parameters[0].Value = model.AutoID;                        
            parameters[1].Value = model.UserCode;                        
            parameters[2].Value = model.EventType;                        
            parameters[3].Value = model.EventTime;                        
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
		public bool Delete(int AutoID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from UserEventLog ");
			strSql.Append(" where AutoID=@AutoID");
						SqlParameter[] parameters = {
					new SqlParameter("@AutoID", SqlDbType.Int,4)
			};
			parameters[0].Value = AutoID;


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
		public bool DeleteList(string AutoIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from UserEventLog ");
			strSql.Append(" where ID in ("+AutoIDlist + ")  ");
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
		public WZX.Model.UserEventLogMod GetModel(int AutoID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select AutoID, UserCode, EventType, EventTime  ");			
			strSql.Append("  from UserEventLog ");
			strSql.Append(" where AutoID=@AutoID");
						SqlParameter[] parameters = {
					new SqlParameter("@AutoID", SqlDbType.Int,4)
			};
			parameters[0].Value = AutoID;

			
			WZX.Model.UserEventLogMod model=new WZX.Model.UserEventLogMod();
			DataSet ds=SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
			
			if(ds.Tables[0].Rows.Count>0)
			{
												if(ds.Tables[0].Rows[0]["AutoID"].ToString()!="")
				{
					model.AutoID=int.Parse(ds.Tables[0].Rows[0]["AutoID"].ToString());
				}
																																				model.UserCode= ds.Tables[0].Rows[0]["UserCode"].ToString();
																																model.EventType= ds.Tables[0].Rows[0]["EventType"].ToString();
																												if(ds.Tables[0].Rows[0]["EventTime"].ToString()!="")
				{
					model.EventTime=DateTime.Parse(ds.Tables[0].Rows[0]["EventTime"].ToString());
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
			strSql.Append(" FROM UserEventLog ");
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
			strSql.Append(" FROM UserEventLog ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
		}
		
        #endregion

        #region

        /// <summary>
        /// 按照月份统计
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public DataSet getMongthRep(string whereString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select count(*) as qiye,month(EventTime) as shijian from dbo.UserEventLog {0} group by month(EventTime) order by month(EventTime)", whereString));
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, sb.ToString(), null);
        }
        /// <summary>
        /// 按照周获取查询统计结果
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public DataSet getWeekRep(string whereString)
        {
            //该方法中，补充的wherestring为：where DATEPART(week, chargetime)>=DATEPART(week, '2013-01-01') and DATEPART(week, chargetime) <=DATEPART(week, '2013-01-15') 
            StringBuilder sb = new StringBuilder();
            //新语句cast(year(chargetime)as char(4))+'-'+cast(DATEPART(week, chargetime) as char)
            sb.Append(string.Format("select count(*) as qiye,cast(year(EventTime)as char(4))+'-'+cast(DATEPART(week, EventTime) as char) as shijian from dbo.UserEventLog {0} group by cast(year(EventTime)as char(4))+'-'+cast(DATEPART(week, EventTime) as char) order by cast(year(EventTime)as char(4))+'-'+cast(DATEPART(week, EventTime) as char)", whereString));
            //sb.Append(string.Format("select sum([money]) as shuliang,DATEPART(week, chargetime) as shijian from tb_charge {0} group by DATEPART(week, chargetime) ", whereString));
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, sb.ToString(), null);
        }
        /// <summary>
        /// 按照天获取查询统计结果
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>

        public DataSet getDayRep(string whereString)
        {
            StringBuilder sb = new StringBuilder();
            //注意，此处还需要修改，现在的语句只是能按照不跨年的月份进行查询，若跨年，需要进一步修改语句convert(char(7),getdate(),120)
            sb.Append(string.Format("select count(*) as qiye,convert(char(10),EventTime,120) as shijian from dbo.UserEventLog {0}  group by convert(char(10),EventTime,120) order by convert(char(10),EventTime,120) ", whereString));
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, sb.ToString(), null);
        }

        //当日上线率
        public DataSet GetDayOnline(string strWhere)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select t1.zongshu,t2.shangxian,t3.xiaxian from(select COUNT(*) as zongshu from dbo.TaxUser)t1,(select COUNT(*) as shangxian from dbo.UserEventLog where EventType ='上线' {0})t2,(select COUNT(*) as xiaxian from dbo.UserEventLog where EventType ='下线' {0})t3", strWhere));
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, sb.ToString(), null);
        }

        //上线企业类型分析
        public DataSet GetTypeOnline(string strWhere)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select tax.UserType, count(*) as reshu from dbo.UserEventLog users left join dbo.TaxUser tax on users.UserCode = tax.UserCode where tax.IsValid = '0' and  users.EventType='上线' {0}  group by tax.UserType", strWhere));
            return SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, sb.ToString(), null);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public string GetOnlineList(int row, int page, string strWhere, string order, string sort)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select tax.UserCode,tax.Name,tax.UserType,users.AutoID ,users.EventType,users.EventTime from dbo.UserEventLog users left join dbo.TaxUser tax on users.UserCode = tax.UserCode ");
            StringBuilder strCount = new StringBuilder();
            strCount.Append("select count(*) from dbo.UserEventLog users left join dbo.TaxUser tax on users.UserCode = tax.UserCode");
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

