using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;

using System.Data.SqlClient;
using RM.Common.DotNetConfig;
using RM.Busines.DAL;
using WZX.Busines.DAL;
using WZX.Common.DotNetConfig;

namespace WZX.Busines.BLL
{
	 	//UserEventLog
		public partial class UserEventLogBll: IDisposable
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
        
		private  UserEventLogDal dal=null;
		public UserEventLogBll()
		{
			this.selfConn = true;
            this.sqlCon = new SqlConnection(ConfigHelperExpand.GetConnectString());
            this.sqlCon.Open();
            this.dal = new UserEventLogDal(this.sqlCon);
		}
		
		 /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据连接</param>
        public UserEventLogBll(SqlConnection con)
        {
            this.sqlCon = con;
            this.dal = new UserEventLogDal(con);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据连接</param>
        /// <param name="transaction">事务</param>
        public UserEventLogBll(SqlConnection con, SqlTransaction tran)
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
		
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int AutoID)
		{
			return dal.Exists(AutoID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(WZX.Model.UserEventLogMod model)
		{
						return dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.UserEventLogMod model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int AutoID)
		{
			
			return dal.Delete(AutoID);
		}
				/// <summary>
		/// 批量删除一批数据
		/// </summary>
		public bool DeleteList(string AutoIDlist )
		{
			return dal.DeleteList(AutoIDlist );
		}
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public WZX.Model.UserEventLogMod GetModel(int AutoID)
		{
			
			return dal.GetModel(AutoID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public WZX.Model.UserEventLogMod GetModelByCache(int AutoID)
		{
			
			string CacheKey = "UserEventLogModel-" + AutoID;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(AutoID);
					if (objModel != null)
					{
						int ModelCache = ConfigHelperExpand.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (WZX.Model.UserEventLogMod)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<WZX.Model.UserEventLogMod> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<WZX.Model.UserEventLogMod> DataTableToList(DataTable dt)
		{
			List<WZX.Model.UserEventLogMod> modelList = new List<WZX.Model.UserEventLogMod>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				WZX.Model.UserEventLogMod model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new WZX.Model.UserEventLogMod();					
													if(dt.Rows[n]["AutoID"].ToString()!="")
				{
					model.AutoID=int.Parse(dt.Rows[n]["AutoID"].ToString());
				}
																																				model.UserCode= dt.Rows[n]["UserCode"].ToString();
																																model.EventType= dt.Rows[n]["EventType"].ToString();
																												if(dt.Rows[n]["EventTime"].ToString()!="")
				{
					model.EventTime=DateTime.Parse(dt.Rows[n]["EventTime"].ToString());
				}
																										
				
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
#endregion

        #region

        public DataSet getMongthRep(string whereString)
        {
            return dal.getMongthRep(whereString);
        }
        public DataSet getWeekRep(string whereString)
        {
            return dal.getWeekRep(whereString);
        }
        public DataSet getDayRep(string whereString)
        {
            return dal.getDayRep(whereString);
        }
        //当日上线率
        public DataSet GetDayOnline(string strWhere)
        {
            return dal.GetDayOnline(strWhere);
        }
        //上线企业类型分析
        public DataSet GetTypeOnline(string strWhere)
        {
            return dal.GetTypeOnline(strWhere);
        }

        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>

        public string GetOnlineList(int row, int page, string strWhere, string order, string sort)
        {

            return dal.GetOnlineList(row, page, strWhere, order, sort);
        }

        #endregion

    }
}