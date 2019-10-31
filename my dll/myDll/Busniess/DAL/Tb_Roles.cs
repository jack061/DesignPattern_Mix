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
	/// 数据访问类:Tb_Roles
	/// </summary>
	public partial class Tb_Roles
	{
		public Tb_Roles()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DataFactory.SqlDataBaseExpand().GetMaxID("Id", "Tb_Roles"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Tb_Roles");
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
		public int Add(WZX.Model.Tb_Roles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tb_Roles(");
			strSql.Append("RolesName,Remark)");
			strSql.Append(" values (");
			strSql.Append("@RolesName,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesName", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,500)};
			parameters[0].Value = model.RolesName;
			parameters[1].Value = model.Remark;

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
		public bool Update(WZX.Model.Tb_Roles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Tb_Roles set ");
			strSql.Append("RolesName=@RolesName,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesName", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,500),
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = model.RolesName;
			parameters[1].Value = model.Remark;
			parameters[2].Value = model.Id;

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
			strSql.Append("delete from Tb_Roles ");
			strSql.Append(" where Id=@Id");
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
			strSql.Append("delete from Tb_Roles ");
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
		public WZX.Model.Tb_Roles GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,RolesName,Remark from Tb_Roles ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
			parameters[0].Value = Id;

			WZX.Model.Tb_Roles model=new WZX.Model.Tb_Roles();
			DataSet ds=DataFactory.SqlDataBaseExpand().Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Id"]!=null && ds.Tables[0].Rows[0]["Id"].ToString()!="")
				{
					model.Id=int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["RolesName"]!=null && ds.Tables[0].Rows[0]["RolesName"].ToString()!="")
				{
					model.RolesName=ds.Tables[0].Rows[0]["RolesName"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Remark"]!=null && ds.Tables[0].Rows[0]["Remark"].ToString()!="")
				{
					model.Remark=ds.Tables[0].Rows[0]["Remark"].ToString();
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
			strSql.Append("select Id,RolesName,Remark ");
			strSql.Append(" FROM Tb_Roles ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DataFactory.SqlDataBaseExpand().Query(strSql.ToString());
		}
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int Selcount(string strWhere)
        {
            string sql = "select count(Id) from Tb_Roles";
            if (strWhere.Trim() != "")
            {
                sql += " where " + strWhere;
            }
            object obj = DataFactory.SqlDataBaseExpand().GetSingle(sql);
            if (obj != null)
            {
                return int.Parse(obj.ToString());
            }
            return 0;
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int row, int page, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(" top " + row);
            strSql.Append(" RolesName,Remark,Id ");
            strSql.Append(" FROM Tb_Roles  where Id not in (select top " + (page - 1) * row + " Id from Tb_Roles ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere + " order by " + filedOrder + ")");
                strSql.Append(" and " + strWhere + " order by " + filedOrder);
            }
            else
            {
                strSql.Append(" order by " + filedOrder + ") order by " + filedOrder);
            }
            return DataFactory.SqlDataBaseExpand().Query(strSql.ToString());
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        public List<WZX.Model.Tb_Roles> GetList_(int row, int page, string strWhere, string filedOrder)
        {
            DataSet ds = GetList(row, page, strWhere, filedOrder);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<WZX.Model.Tb_Roles> GetModelList(string strWhere)
        {
            DataSet ds = GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<WZX.Model.Tb_Roles> DataTableToList(DataTable dt)
        {
            List<WZX.Model.Tb_Roles> modelList = new List<WZX.Model.Tb_Roles>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                WZX.Model.Tb_Roles model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new WZX.Model.Tb_Roles();
                    if (dt.Rows[n]["Id"] != null && dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    if (dt.Rows[n]["RolesName"] != null && dt.Rows[n]["RolesName"].ToString() != "")
                    {
                        model.RolesName = dt.Rows[n]["RolesName"].ToString();
                    }
                    if (dt.Rows[n]["Remark"] != null && dt.Rows[n]["Remark"].ToString() != "")
                    {
                        model.Remark = dt.Rows[n]["Remark"].ToString();
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
		#endregion  Method
	}
}

