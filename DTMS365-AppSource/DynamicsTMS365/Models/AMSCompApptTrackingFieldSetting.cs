using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSCompUserFieldSetting class for Company user field settings
	/// </summary>
	public class AMSCompApptTrackingFieldSetting
	{
		/// <summary>		  
		/// PropertyCompAMSApptTrackingSettingControl/ID as INT	 
		/// </summary>									 
		public int CompAMSApptTrackingSettingControl { get; set; }

		/// <summary>
		/// PropertyCompAMSApptTrackingSettingCompControl as INT	  
		/// </summary>												 
		public int CompAMSApptTrackingSettingCompControl { get; set; }

		/// <summary>		  
		/// PropertyCompAMSApptTrackingSettingModDate as DateTime	
		/// </summary>											 
		public DateTime CompAMSApptTrackingSettingModDate { get; set; }	 
		private string _CompAMSApptTrackingSettingModDate;		

		/// <summary>										  
		/// PropertyCompAMSApptTrackingSettingName as STRING	
		/// </summary>										 
		public string CompAMSApptTrackingSettingName { get; set; }	 

		/// <summary>										  
		/// PropertyCompAMSApptTrackingSettingDesc as STRING  
		/// </summary>		   
		public string CompAMSApptTrackingSettingDesc { get; set; }	  

		/// <summary>	  
		/// PropertyCompAMSApptTrackingSettingModUser as STRING	 
		/// </summary>			
		public string CompAMSApptTrackingSettingModUser { get; set; }	  


		private byte[] _CompAMSApptTrackingSettingUpdated;
		/// <summary>
		/// CompAMSUserFieldSettingUpdated Property as STRING
		/// </summary>
		public string CompAMSApptTrackingSettingUpdated
		{
			get
			{
				if (this._CompAMSApptTrackingSettingUpdated != null)
				{

					return Convert.ToBase64String(this._CompAMSApptTrackingSettingUpdated);

				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{

					this._CompAMSApptTrackingSettingUpdated = null;

				}

				else
				{

					this._CompAMSApptTrackingSettingUpdated = Convert.FromBase64String(value);

				}

			}
		}
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _CompAMSApptTrackingSettingUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the Appointment Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _CompAMSApptTrackingSettingUpdated; }
	}
}