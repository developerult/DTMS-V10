using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSCompUserFieldSetting class for Company user field settings
	/// </summary>
	public class AMSCompColorCodeSetting
	{
		/// <summary>	
		/// PropertyCompAMSColorCodeSettingControl/ID as INT   
		/// </summary>										  
		public int CompAMSColorCodeSettingControl { get; set; }

		/// <summary>	 
		/// PropertyCompAMSColorCodeSettingCompControl as INT	 
		/// </summary>							   
		public int CompAMSColorCodeSettingCompControl { get; set; }

		/// <summary>		 
		/// PropertyCompAMSColorCodeSettingType as INT	  
		/// </summary>			
		public int CompAMSColorCodeSettingType { get; set; }

		/// <summary>	
		/// PropertyCompAMSColorCodeSettingKey as INT	  
		/// </summary>					 
		public int CompAMSColorCodeSettingKey { get; set; }

		/// <summary>						  
		/// PropertyCompAMSColorCodeSettingFieldName as STRING	
		/// </summary>	
		public string CompAMSColorCodeSettingFieldName { get; set; } 

		/// <summary>				
		/// PropertyCompAMSColorCodeSettingFieldDesc as STRING			 
		/// </summary>	  
		public string CompAMSColorCodeSettingFieldDesc { get; set; }

		/// <summary>						  
		/// PropertyCompAMSColorCodeSettingColorCode as STRING	
		/// </summary>	
		public string CompAMSColorCodeSettingColorCode { get; set; }

		/// <summary>  
		/// PropertyCompAMSColorCodeSettingModDate as DateTime	
		/// </summary>	   
		public DateTime CompAMSColorCodeSettingModDate { get; set; }
		private string _CompAMSColorCodeSettingModDate;

		/// <summary>	  
		/// PropertyCompAMSColorCodeSettingModUser as STRING
		/// </summary>	 
		public string CompAMSColorCodeSettingModUser { get; set; }		   


		private byte[] _CompAMSColorCodeSettingUpdated;
		/// <summary>
		/// CompAMSUserFieldSettingUpdated Property as STRING
		/// </summary>
		public string CompAMSColorCodeSettingUpdated
		{
			get
			{
				if (this._CompAMSColorCodeSettingUpdated != null)
				{

					return Convert.ToBase64String(this._CompAMSColorCodeSettingUpdated);

				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{

					this._CompAMSColorCodeSettingUpdated = null;

				}

				else
				{

					this._CompAMSColorCodeSettingUpdated = Convert.FromBase64String(value);

				}

			}
		}
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _CompAMSColorCodeSettingUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the Appointment Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _CompAMSColorCodeSettingUpdated; }
	}
}