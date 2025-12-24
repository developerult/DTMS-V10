using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSApptUserFields
	/// </summary>
	public class AMSApptUserFields
	{
		
		/// <summary>
		/// PropertyAMSApptUFDControl as INT
		/// </summary>
		public int AMSApptUFDControl { get; set; }

		/// <summary>
		/// PropertyAMSApptUFDApptControl as INT
		/// </summary>
		public int AMSApptUFDApptControl { get; set; }

		/// <summary>
		/// PropertyAMSApptUFDCompAMSUserFieldSettingControl as INT
		/// </summary>
		public int AMSApptUFDCompAMSUserFieldSettingControl { get; set; }

		/// <summary>
		/// PropertyAMSApptUFDModDate as DateTime
		/// </summary>
		public DateTime AMSApptUFDModDate { get; set; }
		private string _AMSApptUFDModDate;

		/// <summary>
		/// PropertyAMSApptUFDData as STRING
		/// </summary>
		public string AMSApptUFDData { get; set; }

		/// <summary>
		/// PropertyAMSApptUFDName as STRING
		/// </summary>
		public string AMSApptUFDName { get; set; }

		/// <summary>
		/// PropertyAMSApptUFDDesc as STRING
		/// </summary>
		public string AMSApptUFDDesc { get; set; }

		/// <summary>
		/// PropertyAMSApptUFDModUser as STRING
		/// </summary>
		public string AMSApptUFDModUser { get; set; }
		
		private byte[] _AMSApptUFDUpdated;
		/// <summary>
		/// AMSApptUFDUpdated Property as STRING
		/// </summary>
		public string AMSApptUFDUpdated
		{
			get
			{
				if (this._AMSApptUFDUpdated != null)
				{

					return Convert.ToBase64String(this._AMSApptUFDUpdated);

				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{

					this._AMSApptUFDUpdated = null;

				}

				else
				{

					this._AMSApptUFDUpdated = Convert.FromBase64String(value);

				}

			}
		}
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _AMSApptUFDUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the book item Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _AMSApptUFDUpdated; }

}
}