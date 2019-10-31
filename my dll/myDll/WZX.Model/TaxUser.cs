using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace WZX.Model{
	 	//TaxUser
		public class TaxUserMod
	{
   		     
      		
	 
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
		/// Name
 
        /// </summary>		
		  		private string _name;
 
        public string Name
        {
            get{ return _name; }
            set{ _name = value; }
        }     
						  
			
	 
		/// <summary>
		/// Pwd
 
        /// </summary>		
		  		private string _pwd;
 
        public string Pwd
        {
            get{ return _pwd; }
            set{ _pwd = value; }
        }     
						  
			
	 
		/// <summary>
		/// UserType
 
        /// </summary>		
		  		private string _usertype;
 
        public string UserType
        {
            get{ return _usertype; }
            set{ _usertype = value; }
        }     
						  
			
	 
		/// <summary>
		/// IsValid
 
        /// </summary>		
		  		private bool _isvalid;
 
        public bool IsValid
        {
            get{ return _isvalid; }
            set{ _isvalid = value; }
        }     
						  
		   
	}
}