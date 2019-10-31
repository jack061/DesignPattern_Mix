using System;
namespace WZX.Model
{
	/// <summary>
	/// Tb_Navigation:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Tb_Navigation
	{
		public Tb_Navigation()
		{}
		#region Model
		private int _id;
		private string _menuname;
		private string _pagelogo;
        private int _parentid = 0;
		private string _linkaddress;
		private string _icon;
		private int? _sort;
		private int? _isshow=0;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 菜单名称
		/// </summary>
		public string MenuName
		{
			set{ _menuname=value;}
			get{return _menuname;}
		}
		/// <summary>
		/// 页面标识
		/// </summary>
		public string Pagelogo
		{
			set{ _pagelogo=value;}
			get{return _pagelogo;}
		}
		/// <summary>
		/// 上级菜单,默认0 即根节点
		/// </summary>
		public int ParentId
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 链接地址
		/// </summary>
		public string LinkAddress
		{
			set{ _linkaddress=value;}
			get{return _linkaddress;}
		}
		/// <summary>
		/// 图标
		/// </summary>
		public string Icon
		{
			set{ _icon=value;}
			get{return _icon;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int? Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}
		/// <summary>
		/// 是否显示，默认0显示
		/// </summary>
		public int? IsShow
		{
			set{ _isshow=value;}
			get{return _isshow;}
		}
		#endregion Model

	}
}

