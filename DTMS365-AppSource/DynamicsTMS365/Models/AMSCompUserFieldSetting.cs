using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSCompUserFieldSetting class for Company user field settings
	/// </summary>
	public class AMSCompUserFieldSetting
	{
		/// <summary>								 
		/// PropertyCompAMSUserFieldSettingControl/ID as INT
		/// </summary>										
		public int CompAMSUserFieldSettingControl { get; set; }

		/// <summary>
		/// PropertyCompAMSUserFieldSettingCompControl as INT
		/// </summary>										 
		public int CompAMSUserFieldSettingCompControl { get; set; }

		/// <summary>										 
		/// PropertyCompAMSUserFieldSettingModDate as DateTime
		/// </summary>										  
		public DateTime CompAMSUserFieldSettingModDate { get; set; }
		private string _CompAMSUserFieldSettingModDate;				

		/// <summary>												
		/// PropertyCompAMSUserFieldSettingFieldName as STRING		
		/// </summary>												
		public string CompAMSUserFieldSettingFieldName { get; set; }

		/// <summary>												
		/// PropertyCompAMSUserFieldSettingFieldDesc as STRING		
		/// </summary>												
		public string CompAMSUserFieldSettingFieldDesc { get; set; }		   

		/// <summary>		 
		/// PropertyCompAMSUserFieldSettingModUser as STRING			 
		/// </summary>						
		public string CompAMSUserFieldSettingModUser { get; set; }	   


		private byte[] _CompAMSUserFieldSettingUpdated;
		/// <summary>
		/// CompAMSUserFieldSettingUpdated Property as STRING
		/// </summary>
		public string CompDockdoorUpdated
		{
			get
			{
				if (this._CompAMSUserFieldSettingUpdated != null)
				{

					return Convert.ToBase64String(this._CompAMSUserFieldSettingUpdated);

				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{

					this._CompAMSUserFieldSettingUpdated = null;

				}

				else
				{

					this._CompAMSUserFieldSettingUpdated = Convert.FromBase64String(value);

				}

			}
		}
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _CompAMSUserFieldSettingUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the Appointment Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _CompAMSUserFieldSettingUpdated; }
	}
}