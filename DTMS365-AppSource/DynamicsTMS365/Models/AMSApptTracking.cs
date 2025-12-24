using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSApptTracking
	/// </summary>
	public class AMSApptTracking
	{
		
		/// <summary>
		/// PropertyAMSApptTrackingControl/ID as INT
		/// </summary>
		public int AMSApptTrackingControl { get; set; }

		/// <summary>
		/// PropertyAMSApptTrackingApptControl as INT
		/// </summary>
		public int AMSApptTrackingApptControl { get; set; }

		/// <summary>
		/// PropertyAMSApptTrackingCompAMSApptTrackingSettingControl as INT
		/// </summary>
		public int AMSApptTrackingCompAMSApptTrackingSettingControl { get; set; }

		/// <summary>
		/// PropertyAMSApptTrackingDateTime as DateTime
		/// </summary>
		public DateTime AMSApptTrackingDateTime { get; set; }

		/// <summary>
		/// PropertyAMSApptTrackingModDate as DateTime
		/// </summary>
		public DateTime AMSApptTrackingModDate { get; set; }

		/// <summary>
		/// PropertyAMSApptTrackingName as STRING
		/// </summary>
		public string AMSApptTrackingName { get; set; }

		/// <summary>
		/// PropertyAMSApptTrackingDesc as STRING
		/// </summary>
		public string AMSApptTrackingDesc { get; set; }

		/// <summary>
		/// PropertyAMSApptTrackingModUser as STRING
		/// </summary>
		public string AMSApptTrackingModUser { get; set; }
		
		private byte[] _AMSApptTrackingUpdated;
		/// <summary>
		/// AMSApptTrackingUpdated Property as STRING
		/// </summary>
		public string AMSApptTrackingUpdated
		{
			get
			{
				if (this._AMSApptTrackingUpdated != null)
				{

					return Convert.ToBase64String(this._AMSApptTrackingUpdated);

				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{

					this._AMSApptTrackingUpdated = null;

				}

				else
				{

					this._AMSApptTrackingUpdated = Convert.FromBase64String(value);

				}

			}
		}
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _AMSApptTrackingUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the book item Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _AMSApptTrackingUpdated; }

}
}