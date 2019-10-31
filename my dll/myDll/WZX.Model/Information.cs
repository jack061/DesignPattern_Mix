using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace WZX.Model{
	 	//Information
		public class InformationMod
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
		/// InfoType
 
        /// </summary>		
		  		private string _infotype;
 
        public string InfoType
        {
            get{ return _infotype; }
            set{ _infotype = value; }
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
		/// Title
 
        /// </summary>		
		  		private string _title;
 
        public string Title
        {
            get{ return _title; }
            set{ _title = value; }
        }     
						  
			
	 
		/// <summary>
		/// ContentRtf
 
        /// </summary>		
		  		private string _contentrtf;
 
        public string ContentRtf
        {
            get{ return _contentrtf; }
            set{ _contentrtf = value; }
        }     
						  
			
	 
		/// <summary>
		/// AttachmentCount
 
        /// </summary>		
		  		private int _attachmentcount;
 
        public int AttachmentCount
        {
            get{ return _attachmentcount; }
            set{ _attachmentcount = value; }
        }     
						  
			
	 
		/// <summary>
		/// AttachmentData
 
        /// </summary>		
		  		private byte[] _attachmentdata;
 
        public byte[] AttachmentData
        {
            get{ return _attachmentdata; }
            set{ _attachmentdata = value; }
        }     
						  
			
	 
		/// <summary>
		/// SendTime
 
        /// </summary>		
		  		private  Nullable<DateTime>_sendtime;
        public Nullable<DateTime> SendTime
        {
            get{ return (Nullable<DateTime>)_sendtime; }
            set{ _sendtime = value; }
        }        
						  
		   
	}
}