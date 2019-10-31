using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using RM.Busines;

namespace WZX.Busines.DAL
{
    //Com_Organization
    public partial class Com_Organization
    {

        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Com_Organization");
            strSql.Append(" where ");
            strSql.Append(" Id = @Id  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
            parameters[0].Value = Id;

            return DataFactory.SqlDataBaseExpand().Exists(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(WZX.Model.Com_Organization model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Com_Organization(");
            strSql.Append("Agency,ParentId,Sort,Person,Remark");
            strSql.Append(") values (");
            strSql.Append("@Agency,@ParentId,@Sort,@Person,@Remark");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@Agency", SqlDbType.VarChar) ,            
                        new SqlParameter("@ParentId", SqlDbType.Int) ,            
                        new SqlParameter("@Sort", SqlDbType.Int) ,            
                        new SqlParameter("@Person", SqlDbType.Char) ,            
                        new SqlParameter("@Remark", SqlDbType.VarChar)             
              
            };

            parameters[0].Value = model.Agency;
            parameters[1].Value = model.ParentId;
            parameters[2].Value = model.Sort;
            parameters[3].Value = model.Person;
            parameters[4].Value = model.Remark;

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
        public bool Update(WZX.Model.Com_Organization model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Com_Organization set ");

            strSql.Append(" Agency = @Agency , ");
            strSql.Append(" ParentId = @ParentId , ");
            strSql.Append(" Sort = @Sort , ");
            strSql.Append(" Person = @Person , ");
            strSql.Append(" Remark = @Remark  ");
            strSql.Append(" where Id=@Id ");

            SqlParameter[] parameters = {
			            new SqlParameter("@Id", SqlDbType.Int) ,            
                        new SqlParameter("@Agency", SqlDbType.VarChar) ,            
                        new SqlParameter("@ParentId", SqlDbType.Int) ,            
                        new SqlParameter("@Sort", SqlDbType.Int) ,            
                        new SqlParameter("@Person", SqlDbType.Char) ,            
                        new SqlParameter("@Remark", SqlDbType.VarChar)             
              
            };

            parameters[0].Value = model.Id;
            parameters[1].Value = model.Agency;
            parameters[2].Value = model.ParentId;
            parameters[3].Value = model.Sort;
            parameters[4].Value = model.Person;
            parameters[5].Value = model.Remark;
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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Com_Organization ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
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
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string Idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Com_Organization ");
            strSql.Append(" where ID in (" + Idlist + ")  ");
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
        public WZX.Model.Com_Organization GetModel(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id, Agency, ParentId, Sort, Person, Remark  ");
            strSql.Append("  from Com_Organization ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
            parameters[0].Value = Id;


            WZX.Model.Com_Organization model = new WZX.Model.Com_Organization();
            DataSet ds = DataFactory.SqlDataBaseExpand().Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    model.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                model.Agency = ds.Tables[0].Rows[0]["Agency"].ToString();
                if (ds.Tables[0].Rows[0]["ParentId"].ToString() != "")
                {
                    model.ParentId = int.Parse(ds.Tables[0].Rows[0]["ParentId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sort"].ToString() != "")
                {
                    model.Sort = int.Parse(ds.Tables[0].Rows[0]["Sort"].ToString());
                }
                model.Person = ds.Tables[0].Rows[0]["Person"].ToString();
                model.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();

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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM Com_Organization ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DataFactory.SqlDataBaseExpand().Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM Com_Organization ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DataFactory.SqlDataBaseExpand().Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<WZX.Model.Com_Organization> GetModelList(string strWhere)
        {
            DataSet ds = GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<WZX.Model.Com_Organization> DataTableToList(DataTable dt)
        {
            List<WZX.Model.Com_Organization> modelList = new List<WZX.Model.Com_Organization>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                WZX.Model.Com_Organization model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new WZX.Model.Com_Organization();
                    if (dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    model.Agency = dt.Rows[n]["Agency"].ToString();
                    if (dt.Rows[n]["ParentId"].ToString() != "")
                    {
                        model.ParentId = int.Parse(dt.Rows[n]["ParentId"].ToString());
                    }
                    if (dt.Rows[n]["Sort"].ToString() != "")
                    {
                        model.Sort = int.Parse(dt.Rows[n]["Sort"].ToString());
                    }
                    model.Person = dt.Rows[n]["Person"].ToString();
                    model.Remark = dt.Rows[n]["Remark"].ToString();


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
    }
}

