using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace WZX.Model{
	 	//UserEventLog
		public class UserEventLogMod
	{
   		     
      		
	 
		/// <summary>
		/// AutoID
 
        /// </summary>		
		  		private int _autoid;
 
        public int AutoID
        {
            get{ return _autoid; }
            set{ _autoid = value; }
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
		/// EventType
 
        /// </summary>		
		  		private string _eventtype;
 
        public string EventType
        {
            get{ return _eventtype; }
            set{ _eventtype = value; }
        }     
						  
			
	 
		/// <summary>
		/// EventTime
 
        /// </summary>		
		  		private  Nullable<DateTime>_eventtime;
        public Nullable<DateTime> EventTime
        {
            get{ return (Nullable<DateTime>)_eventtime; }
            set{ _eventtime = value; }
        }        
						  
		   
	}
}