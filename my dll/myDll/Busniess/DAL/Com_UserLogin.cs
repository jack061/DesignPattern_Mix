using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using RM.Busines;
namespace WZX.Busines.DAL
{
	/// <summary>
	/// 数据访问类:Com_UserLogin
	/// </summary>
	public partial class Com_UserLogin
	{
		public Com_UserLogin()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
            return DataFactory.SqlDataBaseExpand().GetMaxID("Id", "Com_UserLogin"); 
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string LoginName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Com_UserLogin");
            strSql.Append(" where LoginName=@LoginName");
            SqlParameter[] parameters = {
					new SqlParameter("@LoginName", SqlDbType.VarChar)
};
            parameters[0].Value = LoginName;

            return DataFactory.SqlDataBaseExpand().Exists(strSql.ToString(), parameters);
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Com_UserLogin");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int)
};
			parameters[0].Value = Id;

            return DataFactory.SqlDataBaseExpand().Exists(strSql.ToString(), parameters);
		}
        /// <summary>
        /// 登录
        /// </summary>
        public string GetUserId(string LoginName, string LoginPassword)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId,LoginName,LoginPassword from Com_UserLogin ");
            strSql.Append(" where LoginName=@LoginName and LoginPassword=@LoginPassword and Status=1");
            SqlParameter[] parameters = {
					new SqlParameter("@LoginName", SqlDbType.VarChar),
                    new SqlParameter("@LoginPassword", SqlDbType.VarChar)
};
            parameters[0].Value = LoginName;
            parameters[1].Value = LoginPassword;

            DataSet ds = DataFactory.SqlDataBaseExpand().Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                 return ds.Tables[0].Rows[0]["UserId"].ToString();
            }
            else
            {
                return null;
            }
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
        public int Add(WZX.Model.Com_UserLogin model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Com_UserLogin(");
			strSql.Append("UserId,LoginName,LoginPassword,Status,LastLoginIP,LastLoginDate)");
			strSql.Append(" values (");
			strSql.Append("@UserId,@LoginName,@LoginPassword,@Status,@LastLoginIP,@LastLoginDate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Char),
					new SqlParameter("@LoginName", SqlDbType.VarChar),
					new SqlParameter("@LoginPassword", SqlDbType.VarChar),
					new SqlParameter("@Status", SqlDbType.Int),
					new SqlParameter("@LastLoginIP", SqlDbType.Char),
					new SqlParameter("@LastLoginDate", SqlDbType.DateTime)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.LoginName;
			parameters[2].Value = model.LoginPassword;
			parameters[3].Value = model.Status;
			parameters[4].Value = model.LastLoginIP;
			parameters[5].Value = model.LastLoginDate;

            object obj = DataFactory.SqlDataBaseExpand().GetSingle(strSql.ToString(), parameters);
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
		public bool Update(Model.Com_UserLogin model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Com_UserLogin set ");
			strSql.Append("UserId=@UserId,");
			strSql.Append("LoginName=@LoginName,");
			strSql.Append("LoginPassword=@LoginPassword,");
			strSql.Append("Status=@Status,");
			strSql.Append("LastLoginIP=@LastLoginIP,");
			strSql.Append("LastLoginDate=@LastLoginDate");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Char),
					new SqlParameter("@LoginName", SqlDbType.VarChar),
					new SqlParameter("@LoginPassword", SqlDbType.VarChar),
					new SqlParameter("@Status", SqlDbType.Int),
					new SqlParameter("@LastLoginIP", SqlDbType.Char),
					new SqlParameter("@LastLoginDate", SqlDbType.DateTime),
					new SqlParameter("@Id", SqlDbType.Int)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.LoginName;
			parameters[2].Value = model.LoginPassword;
			parameters[3].Value = model.Status;
			parameters[4].Value = model.LastLoginIP;
			parameters[5].Value = model.LastLoginDate;
			parameters[6].Value = model.Id;

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
		public bool Delete(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Com_UserLogin ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int)
};
			parameters[0].Value = Id;

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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Com_UserLogin ");
			strSql.Append(" where Id in ("+Idlist + ")  ");
            int rows = DataFactory.SqlDataBaseExpand().ExecuteSql(strSql.ToString());
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
        public Model.Com_UserLogin GetModel(string Userid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Id,UserId,LoginName,LoginPassword,Status,LastLoginIP,LastLoginDate from Com_UserLogin ");
            strSql.Append(" where UserId=@UserId");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Char,10)
};
            parameters[0].Value = Userid;

            Model.Com_UserLogin model = new Model.Com_UserLogin();
            DataSet ds = DataFactory.SqlDataBaseExpand().Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"] != null && ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    model.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"] != null && ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LoginName"] != null && ds.Tables[0].Rows[0]["LoginName"].ToString() != "")
                {
                    model.LoginName = ds.Tables[0].Rows[0]["LoginName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LoginPassword"] != null && ds.Tables[0].Rows[0]["LoginPassword"].ToString() != "")
                {
                    model.LoginPassword = ds.Tables[0].Rows[0]["LoginPassword"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Status"] != null && ds.Tables[0].Rows[0]["Status"].ToString() != "")
                {
                    model.Status = int.Parse(ds.Tables[0].Rows[0]["Status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LastLoginIP"] != null && ds.Tables[0].Rows[0]["LastLoginIP"].ToString() != "")
                {
                    model.LastLoginIP = ds.Tables[0].Rows[0]["LastLoginIP"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LastLoginDate"] != null && ds.Tables[0].Rows[0]["LastLoginDate"].ToString() != "")
                {
                    model.LastLoginDate = DateTime.Parse(ds.Tables[0].Rows[0]["LastLoginDate"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.Com_UserLogin GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,UserId,LoginName,LoginPassword,Status,LastLoginIP,LastLoginDate from Com_UserLogin ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
			parameters[0].Value = Id;

			Model.Com_UserLogin model=new Model.Com_UserLogin();
            DataSet ds = DataFactory.SqlDataBaseExpand().Query(strSql.ToString(), parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Id"]!=null && ds.Tables[0].Rows[0]["Id"].ToString()!="")
				{
					model.Id=int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["UserId"]!=null && ds.Tables[0].Rows[0]["UserId"].ToString()!="")
				{
					model.UserId=ds.Tables[0].Rows[0]["UserId"].ToString();
				}
				if(ds.Tables[0].Rows[0]["LoginName"]!=null && ds.Tables[0].Rows[0]["LoginName"].ToString()!="")
				{
					model.LoginName=ds.Tables[0].Rows[0]["LoginName"].ToString();
				}
				if(ds.Tables[0].Rows[0]["LoginPassword"]!=null && ds.Tables[0].Rows[0]["LoginPassword"].ToString()!="")
				{
					model.LoginPassword=ds.Tables[0].Rows[0]["LoginPassword"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Status"]!=null && ds.Tables[0].Rows[0]["Status"].ToString()!="")
				{
					model.Status=int.Parse(ds.Tables[0].Rows[0]["Status"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LastLoginIP"]!=null && ds.Tables[0].Rows[0]["LastLoginIP"].ToString()!="")
				{
					model.LastLoginIP=ds.Tables[0].Rows[0]["LastLoginIP"].ToString();
				}
				if(ds.Tables[0].Rows[0]["LastLoginDate"]!=null && ds.Tables[0].Rows[0]["LastLoginDate"].ToString()!="")
				{
					model.LastLoginDate=DateTime.Parse(ds.Tables[0].Rows[0]["LastLoginDate"].ToString());
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
			strSql.Append("select Id,UserId,LoginName,LoginPassword,Status,LastLoginIP,LastLoginDate ");
			strSql.Append(" FROM Com_UserLogin ");
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
			strSql.Append(" Id,UserId,LoginName,LoginPassword,Status,LastLoginIP,LastLoginDate ");
			strSql.Append(" FROM Com_UserLogin ");
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
			parameters[0].Value = "Com_UserLogin";
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

