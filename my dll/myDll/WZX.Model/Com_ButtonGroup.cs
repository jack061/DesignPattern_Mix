using System;
namespace WZX.Model
{
	/// <summary>
	/// Com_ButtonGroup:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Com_ButtonGroup
	{
		public Com_ButtonGroup()
		{}
		#region Model
		private int _id;
		private string _buttonname;
		private string _btncode;
		private string _icon;
		private int? _sort;
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
		public string ButtonName
		{
			set{ _buttonname=value;}
			get{return _buttonname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BtnCode
		{
			set{ _btncode=value;}
			get{return _btncode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Icon
		{
			set{ _icon=value;}
			get{return _icon;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Sort
		{
			set{ _sort=value;}
			get{return _sort;}
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

