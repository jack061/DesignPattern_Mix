using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using WZX.Busines;
using System.Collections.Generic;
using RM.Busines;
namespace WZX.Busines.DAL
{
	/// <summary>
	/// 数据访问类:Tb_Navigation
	/// </summary>
	public partial class Tb_Navigation
	{
		public Tb_Navigation()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DataFactory.SqlDataBaseExpand().GetMaxID("Id", "Tb_Navigation"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Tb_Navigation");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
			parameters[0].Value = Id;

			return DataFactory.SqlDataBaseExpand().Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(WZX.Model.Tb_Navigation model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tb_Navigation(");
			strSql.Append("MenuName,Pagelogo,ParentId,LinkAddress,Icon,Sort,IsShow)");
			strSql.Append(" values (");
			strSql.Append("@MenuName,@Pagelogo,@ParentId,@LinkAddress,@Icon,@Sort,@IsShow)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@MenuName", SqlDbType.VarChar,50),
					new SqlParameter("@Pagelogo", SqlDbType.VarChar,50),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@LinkAddress", SqlDbType.VarChar,100),
					new SqlParameter("@Icon", SqlDbType.VarChar,50),
					new SqlParameter("@Sort", SqlDbType.Int,4),
					new SqlParameter("@IsShow", SqlDbType.Int,4)};
			parameters[0].Value = model.MenuName;
			parameters[1].Value = model.Pagelogo;
			parameters[2].Value = model.ParentId;
			parameters[3].Value = model.LinkAddress;
			parameters[4].Value = model.Icon;
			parameters[5].Value = model.Sort;
			parameters[6].Value = model.IsShow;

			object obj = DataFactory.SqlDataBaseExpand().GetSingle(strSql.ToString(),parameters);
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
		public bool Update(WZX.Model.Tb_Navigation model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Tb_Navigation set ");
			strSql.Append("MenuName=@MenuName,");
			strSql.Append("Pagelogo=@Pagelogo,");
			strSql.Append("ParentId=@ParentId,");
			strSql.Append("LinkAddress=@LinkAddress,");
			strSql.Append("Icon=@Icon,");
			strSql.Append("Sort=@Sort,");
			strSql.Append("IsShow=@IsShow");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@MenuName", SqlDbType.VarChar,50),
					new SqlParameter("@Pagelogo", SqlDbType.VarChar,50),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@LinkAddress", SqlDbType.VarChar,100),
					new SqlParameter("@Icon", SqlDbType.VarChar,50),
					new SqlParameter("@Sort", SqlDbType.Int,4),
					new SqlParameter("@IsShow", SqlDbType.Int,4),
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = model.MenuName;
			parameters[1].Value = model.Pagelogo;
			parameters[2].Value = model.ParentId;
			parameters[3].Value = model.LinkAddress;
			parameters[4].Value = model.Icon;
			parameters[5].Value = model.Sort;
			parameters[6].Value = model.IsShow;
			parameters[7].Value = model.Id;

			int rows=DataFactory.SqlDataBaseExpand().ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Tb_Navigation ");
            strSql.Append(" where Id=@Id or ParentId = @Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
			parameters[0].Value = Id;

			int rows=DataFactory.SqlDataBaseExpand().ExecuteSql(strSql.ToString(),parameters);
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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Tb_Navigation ");
			strSql.Append(" where Id in ("+Idlist + ")  ");
			int rows=DataFactory.SqlDataBaseExpand().ExecuteSql(strSql.ToString());
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
		public WZX.Model.Tb_Navigation GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,MenuName,Pagelogo,ParentId,LinkAddress,Icon,Sort,IsShow from Tb_Navigation ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
			parameters[0].Value = Id;

			WZX.Model.Tb_Navigation model=new WZX.Model.Tb_Navigation();
			DataSet ds=DataFactory.SqlDataBaseExpand().Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Id"]!=null && ds.Tables[0].Rows[0]["Id"].ToString()!="")
				{
					model.Id=int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["MenuName"]!=null && ds.Tables[0].Rows[0]["MenuName"].ToString()!="")
				{
					model.MenuName=ds.Tables[0].Rows[0]["MenuName"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Pagelogo"]!=null && ds.Tables[0].Rows[0]["Pagelogo"].ToString()!="")
				{
					model.Pagelogo=ds.Tables[0].Rows[0]["Pagelogo"].ToString();
				}
				if(ds.Tables[0].Rows[0]["ParentId"]!=null && ds.Tables[0].Rows[0]["ParentId"].ToString()!="")
				{
					model.ParentId=int.Parse(ds.Tables[0].Rows[0]["ParentId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LinkAddress"]!=null && ds.Tables[0].Rows[0]["LinkAddress"].ToString()!="")
				{
					model.LinkAddress=ds.Tables[0].Rows[0]["LinkAddress"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Icon"]!=null && ds.Tables[0].Rows[0]["Icon"].ToString()!="")
				{
					model.Icon=ds.Tables[0].Rows[0]["Icon"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Sort"]!=null && ds.Tables[0].Rows[0]["Sort"].ToString()!="")
				{
					model.Sort=int.Parse(ds.Tables[0].Rows[0]["Sort"].ToString());
				}
				if(ds.Tables[0].Rows[0]["IsShow"]!=null && ds.Tables[0].Rows[0]["IsShow"].ToString()!="")
				{
					model.IsShow=int.Parse(ds.Tables[0].Rows[0]["IsShow"].ToString());
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
			strSql.Append("select Id,MenuName,Pagelogo,ParentId,LinkAddress,Icon,Sort,IsShow ");
			strSql.Append(" FROM Tb_Navigation ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            strSql.Append(" order by sort asc ");
			return DataFactory.SqlDataBaseExpand().Query(strSql.ToString());
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
			strSql.Append(" Id,MenuName,Pagelogo,ParentId,LinkAddress,Icon,Sort,IsShow ");
			strSql.Append(" FROM Tb_Navigation ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DataFactory.SqlDataBaseExpand().Query(strSql.ToString());
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<WZX.Model.Tb_Navigation> GetModelList(string strWhere)
        {
            DataSet ds = GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<WZX.Model.Tb_Navigation> DataTableToList(DataTable dt)
        {
            List<WZX.Model.Tb_Navigation> modelList = new List<WZX.Model.Tb_Navigation>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                WZX.Model.Tb_Navigation model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new WZX.Model.Tb_Navigation();
                    if (dt.Rows[n]["Id"] != null && dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    if (dt.Rows[n]["MenuName"] != null && dt.Rows[n]["MenuName"].ToString() != "")
                    {
                        model.MenuName = dt.Rows[n]["MenuName"].ToString();
                    }
                    if (dt.Rows[n]["Pagelogo"] != null && dt.Rows[n]["Pagelogo"].ToString() != "")
                    {
                        model.Pagelogo = dt.Rows[n]["Pagelogo"].ToString();
                    }
                    if (dt.Rows[n]["ParentId"] != null && dt.Rows[n]["ParentId"].ToString() != "")
                    {
                        model.ParentId = int.Parse(dt.Rows[n]["ParentId"].ToString());
                    }
                    if (dt.Rows[n]["LinkAddress"] != null && dt.Rows[n]["LinkAddress"].ToString() != "")
                    {
                        model.LinkAddress = dt.Rows[n]["LinkAddress"].ToString();
                    }
                    if (dt.Rows[n]["Icon"] != null && dt.Rows[n]["Icon"].ToString() != "")
                    {
                        model.Icon = dt.Rows[n]["Icon"].ToString();
                    }
                    if (dt.Rows[n]["Sort"] != null && dt.Rows[n]["Sort"].ToString() != "")
                    {
                        model.Sort = int.Parse(dt.Rows[n]["Sort"].ToString());
                    }
                    if (dt.Rows[n]["IsShow"] != null && dt.Rows[n]["IsShow"].ToString() != "")
                    {
                        model.IsShow = int.Parse(dt.Rows[n]["IsShow"].ToString());
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

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "Tb_Navigation";
			parameters[1].Value = "Id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DataFactory.SqlDataBaseExpand().RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  Method
	}
}

