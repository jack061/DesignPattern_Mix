using System;
namespace WZX.Model
{
	/// <summary>
	/// Tb_Roles:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Tb_Roles
	{
		public Tb_Roles()
		{}
		#region Model
		private int _id;
		private string _rolesname;
		private string _remark;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RolesName
		{
			set{ _rolesname=value;}
			get{return _rolesname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

