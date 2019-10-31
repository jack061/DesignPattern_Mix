using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using WZX.Model;
using System.Data.SqlClient;
using RM.Common.DotNetConfig;
using RM.Busines.DAL;
using WZX.Busines.DAL;
using WZX.Common.DotNetConfig;

namespace WZX.BLL {
	 	//TaxUser
		public partial class TaxUserBll: IDisposable
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
        
		private  TaxUserDal dal=null;
		public TaxUserBll()
		{
			this.selfConn = true;
            this.sqlCon = new SqlConnection(ConfigHelperExpand.GetConnectString());
            this.sqlCon.Open();
            this.dal = new TaxUserDal(this.sqlCon);
		}
		
		 /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据连接</param>
        public TaxUserBll(SqlConnection con)
        {
            this.sqlCon = con;
            this.dal = new TaxUserDal(con);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据连接</param>
        /// <param name="transaction">事务</param>
        public TaxUserBll(SqlConnection con, SqlTransaction tran)
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
		public bool Exists(string UserCode)
		{
			return dal.Exists(UserCode);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void  Add(WZX.Model.TaxUserMod model)
		{
						dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.TaxUserMod model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string UserCode)
		{
			
			return dal.Delete(UserCode);
		}
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public WZX.Model.TaxUserMod GetModel(string UserCode)
		{
			
			return dal.GetModel(UserCode);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public WZX.Model.TaxUserMod GetModelByCache(string UserCode)
		{
			
			string CacheKey = "TaxUserModel-" + UserCode;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(UserCode);
					if (objModel != null)
					{
						int ModelCache = ConfigHelperExpand.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (WZX.Model.TaxUserMod)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList1(string strWhere)
        {
            return dal.GetList1(strWhere);
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
		public List<WZX.Model.TaxUserMod> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<WZX.Model.TaxUserMod> DataTableToList(DataTable dt)
		{
			List<WZX.Model.TaxUserMod> modelList = new List<WZX.Model.TaxUserMod>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				WZX.Model.TaxUserMod model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new WZX.Model.TaxUserMod();					
																	model.UserCode= dt.Rows[n]["UserCode"].ToString();
																																model.Name= dt.Rows[n]["Name"].ToString();
																																model.Pwd= dt.Rows[n]["Pwd"].ToString();
																																model.UserType= dt.Rows[n]["UserType"].ToString();
																																												if(dt.Rows[n]["IsValid"].ToString()!="")
				{
					if((dt.Rows[n]["IsValid"].ToString()=="1")||(dt.Rows[n]["IsValid"].ToString().ToLower()=="true"))
					{
					model.IsValid= true;
					}
					else
					{
					model.IsValid= false;
					}
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

        #region 自定义

        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>

        public string GetAllComList(int row, int page, string strWhere, string order, string sort)
        {

            return dal.GetAllComList(row, page, strWhere, order, sort);
        }

        //注销企业
        public string cancel(string UserCode)
        {
            return dal.cancel(UserCode);
        }
        //批量注销企业
        public string cancelD(string UserCode)
        {
            return dal.cancelD(UserCode);
        }
        //批量启用企业
        public string startD(string UserCode)
        {
            return dal.startD(UserCode);
        }
        //启用企业
        public string start(string UserCode)
        {
            return dal.start(UserCode);
        }
        //重置企业密码
        public string Reset(string UserCode)
        {
            return dal.Reset(UserCode);
        }

        /// <summary>
        /// 获取客户上线次数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>

        public string GetLivenessList(int row, int page, string strWhere, string strWhere1,string strWhere2, string order, string sort)
        {

            return dal.GetLivenessList(row, page, strWhere, strWhere1,strWhere2, order, sort);
        }
        #endregion

    }
}