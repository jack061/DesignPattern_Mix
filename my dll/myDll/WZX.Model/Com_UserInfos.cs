using System;
namespace WZX.Model
{
	/// <summary>
	/// Com_UserInfos:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Com_UserInfos
	{
		public Com_UserInfos()
		{}
		#region Model
		private string _userid;
		private string _userrealname;
		private string _sex;
		private string _email;
		private string _tel;
		private string _mobile;
		private string _adduser;
		private DateTime? _adddate;
		/// <summary>
		/// 用户编号
		/// </summary>
        public string Userid
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 姓名
		/// </summary>
		public string UserRealName
		{
			set{ _userrealname=value;}
			get{return _userrealname;}
		}
		/// <summary>
		/// 性别 1男 2女
		/// </summary>
		public string Sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// 邮箱
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 电话
		/// </summary>
		public string Tel
		{
			set{ _tel=value;}
			get{return _tel;}
		}
		/// <summary>
		/// 手机
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 添加人
		/// </summary>
		public string AddUser
		{
			set{ _adduser=value;}
			get{return _adduser;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime? AddDate
		{
			set{ _adddate=value;}
			get{return _adddate;}
		}
		#endregion Model

	}
}

