using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// ASMCarrierAviblApptInfo
	/// </summary>
	public class ASMCarrierAviblApptInfo
	{
        //Added by LVV on 6/7/19
        public int BookOrigCompControl { get; set; }
        public int BookDestCompControl { get; set; }

        /// <summary>
        /// Appt
        /// </summary>
        public AMSCarrierAvailableSlots Appt { get; set; }
		/// <summary>
		/// CarrierControl
		/// </summary>
		public int CarrierControl { get; set; }
		/// <summary>
		/// CompControl
		/// </summary>
		public int CompControl { get; set; }
		/// <summary>
		/// BookControl
		/// </summary>
		public int BookControl { get; set; }
		/// <summary>
		/// SHID
		/// </summary>
		public string SHID { get; set; }
		/// <summary>
		/// EquipmentID
		/// </summary>
		public string EquipmentID { get; set; }
		/// <summary>
		/// BookDateLoad
		/// </summary>
		public DateTime BookDateLoad { get; set; }
		/// <summary>
		/// BookDateLoad
		/// </summary>
		public DateTime BookDateRequired { get; set; }
		/// <summary>
		/// IsPickup
		/// </summary>
		public bool IsPickup { get; set; }
		/// <summary>
		/// Inbound
		/// </summary>
		public bool Inbound { get; set; }
		/// <summary>
		/// Warehouse
		/// </summary>
		public string Warehouse { get; set; }
		/// <summary>
		/// CarrierName
		/// </summary>
		public string CarrierName { get; set; }
		/// <summary>
		/// IsDelete
		/// </summary>
		public bool IsDelete { get; set; }
		/// <summary>
		/// BookAMSPickupApptControl
		/// </summary>
		public int BookAMSPickupApptControl { get; set; }
		/// <summary>
		/// BookAMSDeliveryApptControl
		/// </summary>
		public int BookAMSDeliveryApptControl { get; set; }
        /// <summary>
        /// CarrierNumber
        /// </summary>
        public int CarrierNumber { get; set; }

        /// <summary>								  
        /// ScheduledDate		  
        /// </summary>								  
        public DateTime? ScheduledDate { get; set; }

        /// <summary>								  
        /// ScheduledTime 		
        /// </summary>								
        public DateTime? ScheduledTime { get; set; }
    }

	/// <summary>
	/// AMSCarrierResults
	/// </summary>
	public class AMSCarrierResults
	{
		/// <summary>
		/// Appt
		/// </summary>
		public AMSCarrierAvailableSlots AvailableSlots { get; set; }
		/// <summary>
		/// Warehouse
		/// </summary>
		public bool blnMustRequestAppt { get; set; }
		/// <summary>
		/// Warehouse
		/// </summary>
		public string RequestSendToEmail { get; set; }
		/// <summary>
		/// Warehouse
		/// </summary>
		public string Subject { get; set; }
		/// <summary>
		/// Warehouse
		/// </summary>
		public string Body { get; set; }
		/// <summary>
		/// Warehouse
		/// </summary>
		public string Message { get; set; }
	}

	/// <summary>
	/// Appointments class for SCH-Appointments
	/// </summary>
	public class AMSCarrierAvailableSlots
	{
		/// <summary>
		/// Property Date Control/ID as DateTime
		/// </summary>
		public DateTime Date { get; set; }
		/// <summary>
		/// Property StartTime Control/ID as DateTime
		/// </summary>
		public DateTime StartTime { get; set; }
		/// <summary>
		/// Property EndTime Control/ID as DateTime
		/// </summary>
		public DateTime EndTime { get; set; }
		/// <summary>
		/// Docks Name Property as STRING
		/// </summary>
		public string Docks { get; set; }
		/// <summary>
		/// Warehouse Property as STRING
		/// </summary>
		public string Warehouse { get; set; }
		/// <summary>
		/// Books Property as STRING
		/// </summary>
		public string Books { get; set; }
		/// <summary>
		/// CarrierNumber Property as INT
		/// </summary>
		public int CarrierNumber { get; set; }
		/// <summary>
		/// CarrierName Property as STRING
		/// </summary>
		public string CarrierName { get; set; }
	}
}