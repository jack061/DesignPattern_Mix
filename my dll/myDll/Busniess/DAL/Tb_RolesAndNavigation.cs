using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using WZX.Busines;
using System.Collections.Generic;
using RM.Busines;//Please add references
namespace WZX.Busines.DAL
{
	/// <summary>
	/// 数据访问类:Tb_RolesAndNavigation
	/// </summary>
	public partial class Tb_RolesAndNavigation
	{
		public Tb_RolesAndNavigation()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DataFactory.SqlDataBaseExpand().GetMaxID("RolesId", "Tb_RolesAndNavigation"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int RolesId,int NavigationId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Tb_RolesAndNavigation");
			strSql.Append(" where RolesId=@RolesId and NavigationId=@NavigationId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@NavigationId", SqlDbType.Int,4)};
			parameters[0].Value = RolesId;
			parameters[1].Value = NavigationId;

			return DataFactory.SqlDataBaseExpand().Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(WZX.Model.Tb_RolesAndNavigation model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tb_RolesAndNavigation(");
			strSql.Append("RolesId,NavigationId)");
			strSql.Append(" values (");
			strSql.Append("@RolesId,@NavigationId)");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@NavigationId", SqlDbType.Int,4)};
			parameters[0].Value = model.RolesId;
			parameters[1].Value = model.NavigationId;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(WZX.Model.Tb_RolesAndNavigation model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Tb_RolesAndNavigation set ");
            #warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！ 
			strSql.Append("RolesId=@RolesId,");
			strSql.Append("NavigationId=@NavigationId");
			strSql.Append(" where RolesId=@RolesId and NavigationId=@NavigationId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@NavigationId", SqlDbType.Int,4)};
			parameters[0].Value = model.RolesId;
			parameters[1].Value = model.NavigationId;

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
		public bool Delete(int RolesId,int NavigationId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Tb_RolesAndNavigation ");
			strSql.Append(" where RolesId=@RolesId and NavigationId=@NavigationId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@NavigationId", SqlDbType.Int,4)};
			parameters[0].Value = RolesId;
			parameters[1].Value = NavigationId;

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
		/// 得到一个对象实体
		/// </summary>
		public WZX.Model.Tb_RolesAndNavigation GetModel(int RolesId,int NavigationId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RolesId,NavigationId from Tb_RolesAndNavigation ");
			strSql.Append(" where RolesId=@RolesId and NavigationId=@NavigationId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@NavigationId", SqlDbType.Int,4)};
			parameters[0].Value = RolesId;
			parameters[1].Value = NavigationId;

			WZX.Model.Tb_RolesAndNavigation model=new WZX.Model.Tb_RolesAndNavigation();
			DataSet ds=DataFactory.SqlDataBaseExpand().Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["RolesId"]!=null && ds.Tables[0].Rows[0]["RolesId"].ToString()!="")
				{
					model.RolesId=int.Parse(ds.Tables[0].Rows[0]["RolesId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["NavigationId"]!=null && ds.Tables[0].Rows[0]["NavigationId"].ToString()!="")
				{
					model.NavigationId=int.Parse(ds.Tables[0].Rows[0]["NavigationId"].ToString());
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
			strSql.Append("select RolesId,NavigationId ");
			strSql.Append(" FROM Tb_RolesAndNavigation ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
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
			strSql.Append(" RolesId,NavigationId ");
			strSql.Append(" FROM Tb_RolesAndNavigation ");
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
        public List<WZX.Model.Tb_RolesAndNavigation> GetModelList(string strWhere)
        {
            DataSet ds = GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<WZX.Model.Tb_RolesAndNavigation> DataTableToList(DataTable dt)
        {
            List<WZX.Model.Tb_RolesAndNavigation> modelList = new List<WZX.Model.Tb_RolesAndNavigation>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                WZX.Model.Tb_RolesAndNavigation model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new WZX.Model.Tb_RolesAndNavigation();
                    if (dt.Rows[n]["RolesId"] != null && dt.Rows[n]["RolesId"].ToString() != "")
                    {
                        model.RolesId = int.Parse(dt.Rows[n]["RolesId"].ToString());
                    }
                    if (dt.Rows[n]["NavigationId"] != null && dt.Rows[n]["NavigationId"].ToString() != "")
                    {
                        model.NavigationId = int.Parse(dt.Rows[n]["NavigationId"].ToString());
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
			parameters[0].Value = "Tb_RolesAndNavigation";
			parameters[1].Value = "NavigationId";
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

