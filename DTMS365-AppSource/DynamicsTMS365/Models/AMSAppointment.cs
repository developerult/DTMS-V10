using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// ASMApptOdrFlg
	/// </summary>
	public class ASMApptOdrFlg
	{
		/// <summary>
		/// Validation
		/// </summary>
		public AMSValidation Validation { get; set; }
		/// <summary>
		/// Appt
		/// </summary>
		public AMSAppointment Appt { get; set; }
		/// <summary>
		/// Ord
		/// </summary>
		public AMSOrder[] Ord { get; set; }
		/// <summary>
		/// Flag
		/// </summary>
		public Boolean Flag { get; set; }
	}
	/// <summary>
	/// Appointments class for SCH-Appointments
	/// </summary>
	public class AMSAppointment
	{
		/// <summary>
		/// Property AMSApptControl Control/ID as INT
		/// </summary>
		public int AMSApptControl { get; set; }
		/// <summary>
		/// Property AMSApptCompControl Control/ID as INT
		/// </summary>
		public int AMSApptCompControl { get; set; }
		/// <summary>
		/// Property AMSApptCarrierControl Control/ID as INT
		/// </summary>
		public int AMSApptCarrierControl { get; set; }
		/// <summary>
		/// AMSApptCarrierSCAC Name Property as STRING
		/// </summary>
		public string AMSApptCarrierSCAC { get; set; }
		/// <summary>
		/// AMSApptCarrierName Name Property as STRING
		/// </summary>
		public string AMSApptCarrierName { get; set; }
		/// <summary>
		/// AMSApptDescription Name Property as STRING
		/// </summary>
		public string AMSApptDescription { get; set; }
		/// <summary>
		/// AMSApptStartDate Property as DateTime
		/// </summary>
		public DateTime AMSApptStartDate { get; set; }
		/// <summary>
		/// AMSApptEndDate Property as DateTime
		/// </summary>
		public DateTime AMSApptEndDate { get; set; }
		/// <summary>
		/// AMSApptTimeZone Name Property as STRING
		/// </summary>
		public string AMSApptTimeZone { get; set; }
		/// <summary>
		/// Property AMSApptRecurrenceParentControl Control/ID as INT
		/// </summary>
		public int? AMSApptRecurrenceParentControl { get; set; }
		/// <summary>
		/// AMSApptRecurrence Name Property as STRING
		/// </summary>
		public string AMSApptRecurrence { get; set; }
		/// <summary>
		/// AMSApptActualDateTime Property as DateTime
		/// </summary>
		public DateTime? AMSApptActualDateTime { get; set; }
		/// <summary>
		/// AMSApptStartLoadingDateTime Property as DateTime
		/// </summary>
		public DateTime? AMSApptStartLoadingDateTime { get; set; }
		/// <summary>
		/// AMSApptFinishLoadingDateTime Property as DateTime
		/// </summary>
		public DateTime? AMSApptFinishLoadingDateTime { get; set; }
		/// <summary>
		/// AMSApptActLoadCompleteDateTime Property as DateTime
		/// </summary>
		public DateTime? AMSApptActLoadCompleteDateTime { get; set; }
		/// <summary>
		/// AMSApptModDate Property as DateTime
		/// </summary>
		public DateTime AMSApptModDate { get; set; }
		/// <summary>
		/// AMSApptModUser Name Property as STRING
		/// </summary>
		public string AMSApptModUser { get; set; }
		/// <summary>
		/// AMSApptNotes Name Property as STRING
		/// </summary>
		public string AMSApptNotes { get; set; }
		/// <summary>
		/// AMSApptDockdoorID Name Property as STRING
		/// </summary>
		public string AMSApptDockdoorID { get; set; }
		/// <summary>
		/// DockDoorName Name Property as STRING
		/// </summary>
		public string DockDoorName { get; set; }
		/// <summary>
		/// Property AMSApptStatusCode Control/ID as INT
		/// </summary>
		public int AMSApptStatusCode { get; set; }
		/// <summary>
		/// AMSApptLabel Name Property as STRING
		/// </summary>
		public string AMSApptLabel { get; set; }
		/// <summary>
		/// AMSApptHover Name Property as STRING
		/// </summary>
		public string AMSApptHover { get; set; }
		/// <summary>
		/// Property AMSApptOrderCount as INT
		/// </summary>
		public int AMSApptOrderCount { get; set; }
		/// <summary>
		/// CompAMSColorCodeSettingColorCode Name Property as STRING
		/// </summary>
		public string CompAMSColorCodeSettingColorCode { get; set; }
		



		private byte[] _AMSApptUpdated;
		/// <summary>
		/// AMSApptUpdate Property as STRING
		/// </summary>
		public string AMSApptUpdated
		{
			get
			{
				if (this._AMSApptUpdated != null)
				{

					return Convert.ToBase64String(this._AMSApptUpdated);

				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{

					this._AMSApptUpdated = null;

				}

				else
				{

					this._AMSApptUpdated = Convert.FromBase64String(value);

				}

			}
		}
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _AMSApptUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the Appointment Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _AMSApptUpdated; }
	}
}