using System;
namespace WZX.Model
{
	/// <summary>
	/// Com_LoginLog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Com_LoginLog
	{
		public Com_LoginLog()
		{}
		#region Model
		private int _id;
		private string _userid;
		private string _loginip;
		private DateTime _logindate;
		private string _status;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户编号
		/// </summary>
		public string Userid
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 登录或注销Ip
		/// </summary>
		public string LoginIP
		{
			set{ _loginip=value;}
			get{return _loginip;}
		}
		/// <summary>
		/// 登录注销时间
		/// </summary>
		public DateTime LoginDate
		{
			set{ _logindate=value;}
			get{return _logindate;}
		}
		/// <summary>
		/// 0登录1注销
		/// </summary>
		public string Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

