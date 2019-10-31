using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using RM.Busines;
namespace WZX.Busines.DAL
{
	/// <summary>
	/// 数据访问类:Com_LoginLog
	/// </summary>
	public partial class Com_LoginLog
	{
		public Com_LoginLog()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DataFactory.SqlDataBaseExpand().GetMaxID("Id", "Com_LoginLog"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Com_LoginLog");
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
		public int Add(WZX.Model.Com_LoginLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Com_LoginLog(");
			strSql.Append("Userid,LoginIP,LoginDate,Status)");
			strSql.Append(" values (");
			strSql.Append("@Userid,@LoginIP,@LoginDate,@Status)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Userid", SqlDbType.Char,10),
					new SqlParameter("@LoginIP", SqlDbType.Char,15),
					new SqlParameter("@LoginDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Char,1)};
			parameters[0].Value = model.Userid;
			parameters[1].Value = model.LoginIP;
			parameters[2].Value = model.LoginDate;
			parameters[3].Value = model.Status;

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
		public bool Update(WZX.Model.Com_LoginLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Com_LoginLog set ");
			strSql.Append("Userid=@Userid,");
			strSql.Append("LoginIP=@LoginIP,");
			strSql.Append("LoginDate=@LoginDate,");
			strSql.Append("Status=@Status");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Userid", SqlDbType.Char,10),
					new SqlParameter("@LoginIP", SqlDbType.Char,15),
					new SqlParameter("@LoginDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Char,1),
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = model.Userid;
			parameters[1].Value = model.LoginIP;
			parameters[2].Value = model.LoginDate;
			parameters[3].Value = model.Status;
			parameters[4].Value = model.Id;

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
			strSql.Append("delete from Com_LoginLog ");
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
			strSql.Append("delete from Com_LoginLog ");
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
		public WZX.Model.Com_LoginLog GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,Userid,LoginIP,LoginDate,Status from Com_LoginLog ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
			parameters[0].Value = Id;

			WZX.Model.Com_LoginLog model=new WZX.Model.Com_LoginLog();
			DataSet ds=DataFactory.SqlDataBaseExpand().Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Id"]!=null && ds.Tables[0].Rows[0]["Id"].ToString()!="")
				{
					model.Id=int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Userid"]!=null && ds.Tables[0].Rows[0]["Userid"].ToString()!="")
				{
					model.Userid=ds.Tables[0].Rows[0]["Userid"].ToString();
				}
				if(ds.Tables[0].Rows[0]["LoginIP"]!=null && ds.Tables[0].Rows[0]["LoginIP"].ToString()!="")
				{
					model.LoginIP=ds.Tables[0].Rows[0]["LoginIP"].ToString();
				}
				if(ds.Tables[0].Rows[0]["LoginDate"]!=null && ds.Tables[0].Rows[0]["LoginDate"].ToString()!="")
				{
					model.LoginDate=DateTime.Parse(ds.Tables[0].Rows[0]["LoginDate"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Status"]!=null && ds.Tables[0].Rows[0]["Status"].ToString()!="")
				{
					model.Status=ds.Tables[0].Rows[0]["Status"].ToString();
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
			strSql.Append("select Id,Userid,LoginIP,LoginDate,Status ");
			strSql.Append(" FROM Com_LoginLog ");
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
			strSql.Append(" Id,Userid,LoginIP,LoginDate,Status ");
			strSql.Append(" FROM Com_LoginLog ");
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
			parameters[0].Value = "Com_LoginLog";
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

