using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using RM.Busines;//Please add references
namespace WZX.Busines.DAL
{
	/// <summary>
	/// 数据访问类:Com_UserInfos
	/// </summary>
	public partial class Com_UserInfos
	{
		public Com_UserInfos()
		{}
		#region  Method
        /// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DataFactory.SqlDataBaseExpand().GetMaxID("Userid", "Com_UserLogin"); 
		}
		/// <summary>
		/// 是否存在该记录
		/// </summary>
        public bool Exists(string Userid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Com_UserInfos");
			strSql.Append(" where Userid=@Userid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Userid", SqlDbType.Char,10)};
			parameters[0].Value = Userid;

			return DataFactory.SqlDataBaseExpand().Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(WZX.Model.Com_UserInfos model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Com_UserInfos(");
			strSql.Append("Userid,UserRealName,Sex,Email,Tel,Mobile,AddUser,AddDate)");
			strSql.Append(" values (");
			strSql.Append("@Userid,@UserRealName,@Sex,@Email,@Tel,@Mobile,@AddUser,@AddDate)");
			SqlParameter[] parameters = {
					new SqlParameter("@Userid", SqlDbType.Char,10),
					new SqlParameter("@UserRealName", SqlDbType.VarChar,50),
					new SqlParameter("@Sex", SqlDbType.Char,1),
					new SqlParameter("@Email", SqlDbType.VarChar,100),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@Mobile", SqlDbType.VarChar,20),
					new SqlParameter("@AddUser", SqlDbType.Char,10),
					new SqlParameter("@AddDate", SqlDbType.DateTime)};
			parameters[0].Value = model.Userid;
			parameters[1].Value = model.UserRealName;
			parameters[2].Value = model.Sex;
			parameters[3].Value = model.Email;
			parameters[4].Value = model.Tel;
			parameters[5].Value = model.Mobile;
			parameters[6].Value = model.AddUser;
			parameters[7].Value = model.AddDate;

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
		public bool Update(WZX.Model.Com_UserInfos model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Com_UserInfos set ");
			strSql.Append("UserRealName=@UserRealName,");
			strSql.Append("Sex=@Sex,");
			strSql.Append("Email=@Email,");
			strSql.Append("Tel=@Tel,");
			strSql.Append("Mobile=@Mobile,");
			strSql.Append("AddUser=@AddUser,");
			strSql.Append("AddDate=@AddDate");
			strSql.Append(" where Userid=@Userid ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserRealName", SqlDbType.VarChar,50),
					new SqlParameter("@Sex", SqlDbType.Char,1),
					new SqlParameter("@Email", SqlDbType.VarChar,100),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@Mobile", SqlDbType.VarChar,20),
					new SqlParameter("@AddUser", SqlDbType.Char,10),
					new SqlParameter("@AddDate", SqlDbType.DateTime),
					new SqlParameter("@Userid", SqlDbType.Char,10)};
			parameters[0].Value = model.UserRealName;
			parameters[1].Value = model.Sex;
			parameters[2].Value = model.Email;
			parameters[3].Value = model.Tel;
			parameters[4].Value = model.Mobile;
			parameters[5].Value = model.AddUser;
			parameters[6].Value = model.AddDate;
			parameters[7].Value = model.Userid;

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
        public bool Delete(string Userid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Com_UserInfos ");
			strSql.Append(" where Userid=@Userid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Userid", SqlDbType.Char,10)};
			parameters[0].Value = Userid;

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
		public bool DeleteList(string Useridlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Com_UserInfos ");
			strSql.Append(" where Userid in ("+Useridlist + ")  ");
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
        public WZX.Model.Com_UserInfos GetModel(string Userid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Userid,UserRealName,Sex,Email,Tel,Mobile,AddUser,AddDate from Com_UserInfos ");
			strSql.Append(" where Userid=@Userid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Userid", SqlDbType.Char,10)};
			parameters[0].Value = Userid;

			WZX.Model.Com_UserInfos model=new WZX.Model.Com_UserInfos();
			DataSet ds=DataFactory.SqlDataBaseExpand().Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				model.Userid=ds.Tables[0].Rows[0]["Userid"].ToString();
				if(ds.Tables[0].Rows[0]["UserRealName"]!=null && ds.Tables[0].Rows[0]["UserRealName"].ToString()!="")
				{
					model.UserRealName=ds.Tables[0].Rows[0]["UserRealName"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Sex"]!=null && ds.Tables[0].Rows[0]["Sex"].ToString()!="")
				{
					model.Sex=ds.Tables[0].Rows[0]["Sex"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Email"]!=null && ds.Tables[0].Rows[0]["Email"].ToString()!="")
				{
					model.Email=ds.Tables[0].Rows[0]["Email"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Tel"]!=null && ds.Tables[0].Rows[0]["Tel"].ToString()!="")
				{
					model.Tel=ds.Tables[0].Rows[0]["Tel"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Mobile"]!=null && ds.Tables[0].Rows[0]["Mobile"].ToString()!="")
				{
					model.Mobile=ds.Tables[0].Rows[0]["Mobile"].ToString();
				}
				if(ds.Tables[0].Rows[0]["AddUser"]!=null && ds.Tables[0].Rows[0]["AddUser"].ToString()!="")
				{
					model.AddUser=ds.Tables[0].Rows[0]["AddUser"].ToString();
				}
				if(ds.Tables[0].Rows[0]["AddDate"]!=null && ds.Tables[0].Rows[0]["AddDate"].ToString()!="")
				{
					model.AddDate=DateTime.Parse(ds.Tables[0].Rows[0]["AddDate"].ToString());
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
			strSql.Append("select Userid,UserRealName,Sex,Email,Tel,Mobile,AddUser,AddDate ");
			strSql.Append(" FROM Com_UserInfos ");
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
			strSql.Append(" Userid,UserRealName,Sex,Email,Tel,Mobile,AddUser,AddDate ");
			strSql.Append(" FROM Com_UserInfos ");
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
			parameters[0].Value = "Com_UserInfos";
			parameters[1].Value = "Userid";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DataFactory.SqlDataBaseExpand().RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  Method

        #region 自定义

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public IList<WZX.Model.Com_UserInfos> GetList()
        {
            IList<WZX.Model.Com_UserInfos> lst = new List<WZX.Model.Com_UserInfos>();
            DataTable dt = new DataTable();
            string sql = "select * from Com_UserInfos";
            dt = DataFactory.SqlDataBaseExpand().ExesqlDT(sql);
            foreach (DataRow row in dt.Rows)
            {
                WZX.Model.Com_UserInfos UserMod = new Model.Com_UserInfos();
                UserMod.Userid = row["Userid"].ToString();
                UserMod.UserRealName = row["UserRealName"].ToString();
                lst.Add(UserMod);

            }
            return lst;

        }

        #endregion
	}
}

