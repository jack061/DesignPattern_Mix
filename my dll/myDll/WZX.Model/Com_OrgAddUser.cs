using System;
namespace WZX.Model
{
	/// <summary>
	/// Com_OrgAddUser:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Com_OrgAddUser
	{
		public Com_OrgAddUser()
		{}
		#region Model
		private int _orgid;
		private string _userid;
		/// <summary>
		/// 
		/// </summary>
		public int OrgId
		{
			set{ _orgid=value;}
			get{return _orgid;}
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

