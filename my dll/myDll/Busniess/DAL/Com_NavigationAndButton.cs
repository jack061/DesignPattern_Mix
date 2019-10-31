using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using RM.Busines;
namespace WZX.Busines.DAL
{
	/// <summary>
	/// 数据访问类:Com_NavigationAndButton
	/// </summary>
	public partial class Com_NavigationAndButton
	{
		public Com_NavigationAndButton()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DataFactory.SqlDataBaseExpand().GetMaxID("NavigationId", "Com_NavigationAndButton"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int NavigationId,int ButtonId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Com_NavigationAndButton");
			strSql.Append(" where NavigationId=@NavigationId and ButtonId=@ButtonId ");
			SqlParameter[] parameters = {
					new SqlParameter("@NavigationId", SqlDbType.Int,4),
					new SqlParameter("@ButtonId", SqlDbType.Int,4)};
			parameters[0].Value = NavigationId;
			parameters[1].Value = ButtonId;

			return DataFactory.SqlDataBaseExpand().Exists(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(WZX.Model.Com_NavigationAndButton model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Com_NavigationAndButton(");
			strSql.Append("NavigationId,ButtonId)");
			strSql.Append(" values (");
			strSql.Append("@NavigationId,@ButtonId)");
			SqlParameter[] parameters = {
					new SqlParameter("@NavigationId", SqlDbType.Int,4),
					new SqlParameter("@ButtonId", SqlDbType.Int,4)};
			parameters[0].Value = model.NavigationId;
			parameters[1].Value = model.ButtonId;

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
		public bool Update(WZX.Model.Com_NavigationAndButton model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Com_NavigationAndButton set ");
#warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！ 
			strSql.Append("NavigationId=@NavigationId,");
			strSql.Append("ButtonId=@ButtonId");
			strSql.Append(" where NavigationId=@NavigationId and ButtonId=@ButtonId ");
			SqlParameter[] parameters = {
					new SqlParameter("@NavigationId", SqlDbType.Int,4),
					new SqlParameter("@ButtonId", SqlDbType.Int,4)};
			parameters[0].Value = model.NavigationId;
			parameters[1].Value = model.ButtonId;

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
		public bool Delete(int NavigationId,int ButtonId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Com_NavigationAndButton ");
			strSql.Append(" where NavigationId=@NavigationId and ButtonId=@ButtonId ");
			SqlParameter[] parameters = {
					new SqlParameter("@NavigationId", SqlDbType.Int,4),
					new SqlParameter("@ButtonId", SqlDbType.Int,4)};
			parameters[0].Value = NavigationId;
			parameters[1].Value = ButtonId;

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
		public WZX.Model.Com_NavigationAndButton GetModel(int NavigationId,int ButtonId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 NavigationId,ButtonId from Com_NavigationAndButton ");
			strSql.Append(" where NavigationId=@NavigationId and ButtonId=@ButtonId ");
			SqlParameter[] parameters = {
					new SqlParameter("@NavigationId", SqlDbType.Int,4),
					new SqlParameter("@ButtonId", SqlDbType.Int,4)};
			parameters[0].Value = NavigationId;
			parameters[1].Value = ButtonId;

			WZX.Model.Com_NavigationAndButton model=new WZX.Model.Com_NavigationAndButton();
			DataSet ds=DataFactory.SqlDataBaseExpand().Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["NavigationId"]!=null && ds.Tables[0].Rows[0]["NavigationId"].ToString()!="")
				{
					model.NavigationId=int.Parse(ds.Tables[0].Rows[0]["NavigationId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ButtonId"]!=null && ds.Tables[0].Rows[0]["ButtonId"].ToString()!="")
				{
					model.ButtonId=int.Parse(ds.Tables[0].Rows[0]["ButtonId"].ToString());
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
			strSql.Append("select NavigationId,ButtonId ");
			strSql.Append(" FROM Com_NavigationAndButton ");
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
			strSql.Append(" NavigationId,ButtonId ");
			strSql.Append(" FROM Com_NavigationAndButton ");
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
			parameters[0].Value = "Com_NavigationAndButton";
			parameters[1].Value = "ButtonId";
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

