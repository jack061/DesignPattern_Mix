using System;
namespace WZX.Model
{
	/// <summary>
	/// Tb_RolesAddUser:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Tb_RolesAddUser
	{
		public Tb_RolesAddUser()
		{}
		#region Model
		private int _rolesid;
		private string _userid;
		/// <summary>
		/// 
		/// </summary>
		public int RolesId
		{
			set{ _rolesid=value;}
			get{return _rolesid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		#endregion Model

	}
}

