using System;
namespace WZX.Model
{
	/// <summary>
	/// Com_NavigationAndButton:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Com_NavigationAndButton
	{
		public Com_NavigationAndButton()
		{}
		#region Model
		private int _navigationid;
		private int _buttonid;
		/// <summary>
		/// 
		/// </summary>
		public int NavigationId
		{
			set{ _navigationid=value;}
			get{return _navigationid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ButtonId
		{
			set{ _buttonid=value;}
			get{return _buttonid;}
		}
		#endregion Model

	}
}

