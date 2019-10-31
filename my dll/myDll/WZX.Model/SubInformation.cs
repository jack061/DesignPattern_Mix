using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace WZX.Model{
	 	//SubInformation
		public class SubInformationMod
	{
   		     
      		
	 
		/// <summary>
		/// ID
 
        /// </summary>		
		  		private long _id;
 
        public long ID
        {
            get{ return _id; }
            set{ _id = value; }
        }     
						  
			
	 
		/// <summary>
		/// InformationID
 
        /// </summary>		
		  		private long _informationid;
 
        public long InformationID
        {
            get{ return _informationid; }
            set{ _informationid = value; }
        }     
						  
			
	 
		/// <summary>
		/// UserCode
 
        /// </summary>		
		  		private string _usercode;
 
        public string UserCode
        {
            get{ return _usercode; }
            set{ _usercode = value; }
        }     
						  
			
	 
		/// <summary>
		/// FengxianContent
 
        /// </summary>		
		  		private string _fengxiancontent;
 
        public string FengxianContent
        {
            get{ return _fengxiancontent; }
            set{ _fengxiancontent = value; }
        }     
						  
			
	 
		/// <summary>
		/// FengxianDate
 
        /// </summary>		
		  		private string _fengxiandate;
 
        public string FengxianDate
        {
            get{ return _fengxiandate; }
            set{ _fengxiandate = value; }
        }     
						  
			
	 
		/// <summary>
		/// IsGet
 
        /// </summary>		
		  		private bool _isget;
 
        public bool IsGet
        {
            get{ return _isget; }
            set{ _isget = value; }
        }     
						  
			
	 
		/// <summary>
		/// IsDone
 
        /// </summary>		
		  		private bool _isdone;
 
        public bool IsDone
        {
            get{ return _isdone; }
            set{ _isdone = value; }
        }     
						  
			
	 
		/// <summary>
		/// CreateTime
 
        /// </summary>		
		  		private  Nullable<DateTime>_createtime;
        public Nullable<DateTime> CreateTime
        {
            get{ return (Nullable<DateTime>)_createtime; }
            set{ _createtime = value; }
        }        
						  
		   
	}
}