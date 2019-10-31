using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace WZX.Model{
	 	//Com_Organization
		public class Com_Organization
	{
   		     
      	/// <summary>
		/// Id
        /// </summary>		
		private int _id;
        public int Id
        {
            get{ return _id; }
            set{ _id = value; }
        }        
		/// <summary>
		/// 部门名称
        /// </summary>		
		private string _agency;
        public string Agency
        {
            get{ return _agency; }
            set{ _agency = value; }
        }        
		/// <summary>
		/// 上级部门
        /// </summary>		
		private int _parentid;
        public int ParentId
        {
            get{ return _parentid; }
            set{ _parentid = value; }
        }        
		/// <summary>
		/// 排序
        /// </summary>		
		private int _sort;
        public int Sort
        {
            get{ return _sort; }
            set{ _sort = value; }
        }        
		/// <summary>
		/// 负责人Id
        /// </summary>		
		private string _person;
        public string Person
        {
            get{ return _person; }
            set{ _person = value; }
        }        
		/// <summary>
		/// 备注
        /// </summary>		
		private string _remark;
        public string Remark
        {
            get{ return _remark; }
            set{ _remark = value; }
        }        
		   
	}
}

