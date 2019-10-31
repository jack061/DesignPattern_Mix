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
	 	//TaxConfiguration
		public  class TaxConfigurationDal:IDisposable
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
        
        public TaxConfigurationDal (SqlConnection con)
        {
            this.sqlCon = con;
        }
   		
   		#region Function
   		
		public bool Exists(string ItemKey)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from TaxConfiguration");
			strSql.Append(" where ");
			                                       strSql.Append(" ItemKey = @ItemKey  ");
                            			SqlParameter[] parameters = {
					new SqlParameter("@ItemKey", SqlDbType.NVarChar,50)			};
			parameters[0].Value = ItemKey;

			return Convert.ToInt32(SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters)) != 0;
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(WZX.Model.TaxConfigurationMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into TaxConfiguration(");			
            strSql.Append("ItemKey,ItemValue");
			strSql.Append(") values (");
            strSql.Append("@ItemKey,@ItemValue");            
            strSql.Append(") ");            
            		
			SqlParameter[] parameters = {
			            new SqlParameter("@ItemKey", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ItemValue", SqlDbType.NVarChar,50)             
              
            };
			            
            parameters[0].Value = model.ItemKey;                        
            parameters[1].Value = model.ItemValue;                        
			            SqlHelper.ExecuteScalar(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), parameters);	
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.TaxConfigurationMod model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update TaxConfiguration set ");
			                        
            strSql.Append(" ItemKey = @ItemKey , ");                                    
            strSql.Append(" ItemValue = @ItemValue  ");            			
			strSql.Append(" where ItemKey=@ItemKey  ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@ItemKey", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ItemValue", SqlDbType.NVarChar,50)             
              
            };
						            
            parameters[0].Value = model.ItemKey;                        
            parameters[1].Value = model.ItemValue;                        
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
		public bool Delete(string ItemKey)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TaxConfiguration ");
			strSql.Append(" where ItemKey=@ItemKey ");
						SqlParameter[] parameters = {
					new SqlParameter("@ItemKey", SqlDbType.NVarChar,50)			};
			parameters[0].Value = ItemKey;


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
		public WZX.Model.TaxConfigurationMod GetModel(string ItemKey)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ItemKey, ItemValue  ");			
			strSql.Append("  from TaxConfiguration ");
			strSql.Append(" where ItemKey=@ItemKey ");
						SqlParameter[] parameters = {
					new SqlParameter("@ItemKey", SqlDbType.NVarChar,50)			};
			parameters[0].Value = ItemKey;

			
			WZX.Model.TaxConfigurationMod model=new WZX.Model.TaxConfigurationMod();
			DataSet ds=SqlHelper.ExecuteDataset(this.sqlCon, this.sqlTran, CommandType.Text, strSql.ToString(), null);
			
			if(ds.Tables[0].Rows.Count>0)
			{
																model.ItemKey= ds.Tables[0].Rows[0]["ItemKey"].ToString();
																																model.ItemValue= ds.Tables[0].Rows[0]["ItemValue"].ToString();
																										
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
			strSql.Append(" FROM TaxConfiguration ");
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
			strSql.Append(" FROM TaxConfiguration ");
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
        public string GetAllConList(int row, int page, string strWhere, string order, string sort)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemKey, CONVERT(varchar(11) , ItemValue, 120 ) as ItemValue from TaxConfiguration ");
            StringBuilder strCount = new StringBuilder();
            strCount.Append("select count(*) from TaxConfiguration");
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

        //修改申报时间
        public string editCon(string ItemKey, DateTime EItemValue)
        {
            string sql = "update TaxConfiguration  set ItemValue='" + EItemValue + "' where ItemKey='" + ItemKey + "'";
            if (SqlHelper.ExecuteNonQuery(sqlCon, CommandType.Text, sql) > 0)
            {
                return "{\"flag\":true,\"msg\":\"申报时间修改成功！\"}";
            }
            else return "{\"flag\":false,\"msg\":\"申报时间修改失败！\"}";
        }
        #endregion
   
	}
}

