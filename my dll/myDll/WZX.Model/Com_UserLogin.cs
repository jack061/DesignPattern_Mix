using System;
namespace WZX.Model
{
	/// <summary>
	/// Com_UserLogin:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Com_UserLogin
	{
		public Com_UserLogin()
		{}
		#region Model
		private int _id;
		private string _userid;
		private string _loginname;
		private string _loginpassword;
		private int _status;
		private string _lastloginip;
		private DateTime? _lastlogindate;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户Id
		/// </summary>
		public string UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 登录账号
		/// </summary>
		public string LoginName
		{
			set{ _loginname=value;}
			get{return _loginname;}
		}
		/// <summary>
		/// 登录密码，加密
		/// </summary>
		public string LoginPassword
		{
			set{ _loginpassword=value;}
			get{return _loginpassword;}
		}
		/// <summary>
		/// 状态，0未启用 1允许登录 2禁止登录
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 最后登录的IP
		/// </summary>
		public string LastLoginIP
		{
			set{ _lastloginip=value;}
			get{return _lastloginip;}
		}
		/// <summary>
		/// 最后登录的时间
		/// </summary>
		public DateTime? LastLoginDate
		{
			set{ _lastlogindate=value;}
			get{return _lastlogindate;}
		}
		#endregion Model

	}
}

