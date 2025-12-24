using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSCompDockdoor class for Company Dockdoor
	/// </summary>
	public class AMSCompDockdoor
	{
        //Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement - Added field CompDockInbound

        /// <summary>
        /// Property CompDockContol Control/ID as INT
        /// </summary>
        public int CompDockControl { get; set; }
		/// <summary>
		/// Property CompDockCompControl Control/ID as INT	  
		/// </summary>
		public int CompDockCompControl { get; set; }
		/// <summary>
		/// Property AvgApptTime Time as INT
		/// </summary>
		public int AvgApptTime { get; set; }
		/// <summary>
		/// DockDoorID ID Property as STRING
		/// </summary>
		public string CompDockDockDoorID { get; set; }
		/// <summary>
		/// DockDoorName Name Property as STRING
		/// </summary>
		public string CompDockDockDoorName { get; set; }

		/// <summary>
		/// PropertyCompDockBookingSeq as INT
		/// </summary>					 
		public int CompDockBookingSeq { get; set; }

		/// <summary>
		/// PropertyCompDockValidation as bool
		/// </summary>						  
		public bool CompDockValidation { get; set; }

		/// <summary>
		/// PropertyCompDockOverrideAlert as bool
		/// </summary>							 
		public bool CompDockOverrideAlert { get; set; }

		/// <summary>
		/// PropertyCompDockNotificationAlert as bool
		/// </summary>								 
		public bool CompDockNotificationAlert { get; set; }

		/// <summary>
		/// PropertyCompDockNotificationEmail as STRING
		/// </summary>								   
		public string CompDockNotificationEmail { get; set; }

        public bool CompDockInbound { get; set; } //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement

        /// <summary>
        /// CompDockdoorModDate Property as DateTime
        /// </summary>
        public DateTime CompDockDoorModDate { get; set; }
		/// <summary>
		/// CompDockdoorModUser Name Property as STRING
		/// </summary>
		public string CompDockDoorModUser { get; set; }

		private byte[] _CompDockdoorUpdated;
		/// <summary>
		/// AMSApptUpdate Property as STRING
		/// </summary>
		public string CompDockdoorUpdated
		{
			get
			{
				if (this._CompDockdoorUpdated != null)
				{

					return Convert.ToBase64String(this._CompDockdoorUpdated);

				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{

					this._CompDockdoorUpdated = null;

				}

				else
				{

					this._CompDockdoorUpdated = Convert.FromBase64String(value);

				}

			}
		}
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _CompDockdoorUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the Appointment Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _CompDockdoorUpdated; }
	}
}