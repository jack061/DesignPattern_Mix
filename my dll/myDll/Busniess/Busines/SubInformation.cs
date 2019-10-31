using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using System.Data.SqlClient;
using RM.Busines.DAL;
using WZX.Common.DotNetConfig;
using WZX.Busines.DAL;

namespace WZX.Busines.BLL {
	 	//SubInformation
		public partial class SubInformationBll: IDisposable
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
        
		private  SubInformationDal dal=null;
		public SubInformationBll()
		{
			this.selfConn = true;
            this.sqlCon = new SqlConnection(ConfigHelperExpand.GetConnectString());
            this.sqlCon.Open();
            this.dal = new SubInformationDal(this.sqlCon);
		}
		
		 /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据连接</param>
        public SubInformationBll(SqlConnection con)
        {
            this.sqlCon = con;
            this.dal = new WZX.Busines.DAL.SubInformationDal(con);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据连接</param>
        /// <param name="transaction">事务</param>
        public SubInformationBll(SqlConnection con, SqlTransaction tran)
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
		public bool Exists(long ID)
		{
			return dal.Exists(ID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long  Add(WZX.Model.SubInformationMod model)
		{
						return dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.SubInformationMod model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long ID)
		{
			
			return dal.Delete(ID);
		}
				/// <summary>
		/// 批量删除一批数据
		/// </summary>
		public bool DeleteList(string IDlist )
		{
			return dal.DeleteList(IDlist );
		}
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public WZX.Model.SubInformationMod GetModel(long ID)
		{
			
			return dal.GetModel(ID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public WZX.Model.SubInformationMod GetModelByCache(long ID)
		{
			
			string CacheKey = "SubInformationModel-" + ID;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ID);
					if (objModel != null)
					{
						int ModelCache = ConfigHelperExpand.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (WZX.Model.SubInformationMod)objModel;
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
		public List<WZX.Model.SubInformationMod> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<WZX.Model.SubInformationMod> DataTableToList(DataTable dt)
		{
			List<WZX.Model.SubInformationMod> modelList = new List<WZX.Model.SubInformationMod>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				WZX.Model.SubInformationMod model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new WZX.Model.SubInformationMod();					
													if(dt.Rows[n]["ID"].ToString()!="")
				{
					model.ID=long.Parse(dt.Rows[n]["ID"].ToString());
				}
				if(dt.Rows[n]["InformationID"].ToString()!="")
				{
					model.InformationID=long.Parse(dt.Rows[n]["InformationID"].ToString());
				}
																																				model.UserCode= dt.Rows[n]["UserCode"].ToString();
				model.FengxianContent= dt.Rows[n]["FengxianContent"].ToString();
				model.FengxianDate= dt.Rows[n]["FengxianDate"].ToString();
																																												if(dt.Rows[n]["IsGet"].ToString()!="")
				{
					if((dt.Rows[n]["IsGet"].ToString()=="1")||(dt.Rows[n]["IsGet"].ToString().ToLower()=="true"))
					{
					model.IsGet= true;
					}
					else
					{
					model.IsGet= false;
					}
				}
				if(dt.Rows[n]["IsDone"].ToString()!="")
				{
					if((dt.Rows[n]["IsDone"].ToString()=="1")||(dt.Rows[n]["IsDone"].ToString().ToLower()=="true"))
					{
					model.IsDone= true;
					}
					else
					{
					model.IsDone= false;
					}
				}
																if(dt.Rows[n]["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(dt.Rows[n]["CreateTime"].ToString());
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
            //消息接收完成表
        public DataSet GetMessage(string strWhere)
        {
            return dal.GetMessage(strWhere);
        }
        //消息接收表
        public DataSet GetReceive(string strWhere)
        {
            return dal.GetReceive(strWhere);
        }
        //风险完成表
        public DataSet GetRisk(string strWhere)
        {
            return dal.GetRisk(strWhere);
        }

        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>

        public string GetReceiveList(int row, int page, string strWhere, string order, string sort)
        {

            return dal.GetReceiveList(row, page, strWhere, order, sort);
        }
        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>

        public string GetRiskList(int row, int page, string strWhere, string order, string sort)
        {

            return dal.GetRiskList(row, page, strWhere, order, sort);
        }
        #endregion

    }
}