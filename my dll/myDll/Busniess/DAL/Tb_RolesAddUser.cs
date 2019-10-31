using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using WZX.Busines;
using RM.Busines;
namespace WZX.Busines.DAL
{
	/// <summary>
	/// 数据访问类:Tb_RolesAddUser
	/// </summary>
	public partial class Tb_RolesAddUser
	{
		public Tb_RolesAddUser()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DataFactory.SqlDataBaseExpand().GetMaxID("RolesId", "Tb_RolesAddUser"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int RolesId,string UserId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Tb_RolesAddUser");
			strSql.Append(" where RolesId=@RolesId and UserId=@UserId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Char,10)};
			parameters[0].Value = RolesId;
			parameters[1].Value = UserId;

			return DataFactory.SqlDataBaseExpand().Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(WZX.Model.Tb_RolesAddUser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tb_RolesAddUser(");
			strSql.Append("RolesId,UserId)");
			strSql.Append(" values (");
			strSql.Append("@RolesId,@UserId)");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Char,10)};
			parameters[0].Value = model.RolesId;
			parameters[1].Value = model.UserId;

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
		public bool Update(WZX.Model.Tb_RolesAddUser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Tb_RolesAddUser set ");
#warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！ 
			strSql.Append("RolesId=@RolesId,");
			strSql.Append("UserId=@UserId");
			strSql.Append(" where RolesId=@RolesId and UserId=@UserId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Char,10)};
			parameters[0].Value = model.RolesId;
			parameters[1].Value = model.UserId;

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
		public bool Delete(int RolesId,string UserId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Tb_RolesAddUser ");
			strSql.Append(" where RolesId=@RolesId and UserId=@UserId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Char,10)};
			parameters[0].Value = RolesId;
			parameters[1].Value = UserId;

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
        public bool Delete(int RolesId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tb_RolesAddUser ");
            strSql.Append(" where RolesId=@RolesId");
            SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4)};
            parameters[0].Value = RolesId;

            int rows = DataFactory.SqlDataBaseExpand().ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(string UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tb_RolesAddUser ");
            strSql.Append(" where RolesId=@RolesId and UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Char,10)};
            parameters[0].Value = UserId;

            int rows = DataFactory.SqlDataBaseExpand().ExecuteSql(strSql.ToString(), parameters);
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
		public WZX.Model.Tb_RolesAddUser GetModel(int RolesId,string UserId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RolesId,UserId from Tb_RolesAddUser ");
			strSql.Append(" where RolesId=@RolesId and UserId=@UserId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RolesId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Char,10)};
			parameters[0].Value = RolesId;
			parameters[1].Value = UserId;

			WZX.Model.Tb_RolesAddUser model=new WZX.Model.Tb_RolesAddUser();
			DataSet ds=DataFactory.SqlDataBaseExpand().Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["RolesId"]!=null && ds.Tables[0].Rows[0]["RolesId"].ToString()!="")
				{
					model.RolesId=int.Parse(ds.Tables[0].Rows[0]["RolesId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["UserId"]!=null && ds.Tables[0].Rows[0]["UserId"].ToString()!="")
				{
					model.UserId=ds.Tables[0].Rows[0]["UserId"].ToString();
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
			strSql.Append("select RolesId,UserId ");
			strSql.Append(" FROM Tb_RolesAddUser ");
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
			strSql.Append(" RolesId,UserId ");
			strSql.Append(" FROM Tb_RolesAddUser ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DataFactory.SqlDataBaseExpand().Query(strSql.ToString());
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
			parameters[0].Value = "Tb_RolesAddUser";
			parameters[1].Value = "UserId";
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

