using System;
namespace WZX.Model
{
	/// <summary>
	/// Tb_RolesAndNavigation:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Tb_RolesAndNavigation
	{
		public Tb_RolesAndNavigation()
		{}
		#region Model
		private int _rolesid;
		private int _navigationid;
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
		public int NavigationId
		{
			set{ _navigationid=value;}
			get{return _navigationid;}
		}
		#endregion Model

	}
}

