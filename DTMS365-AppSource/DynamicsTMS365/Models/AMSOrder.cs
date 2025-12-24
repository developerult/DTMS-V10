using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{   /// <summary>
	/// AMSOrder class for SCH-Appointments
	/// </summary>
	public class AMSOrder
	{

		/// <summary>
		/// Property BookControl Control/ID as INT	  
		/// </summary>
		public int BookControl { get; set; }
		/// <summary>
		/// Property BookAMSDeliveryApptControl Control/ID as INT	
		/// </summary>
		public int BookAMSDeliveryApptControl { get; set; }
		/// <summary>
		/// Property BookAMSPickupApptControl Control/ID as INT	
		/// </summary>
		public int BookAMSPickupApptControl { get; set; }
		/// <summary>
		/// BookItemDetailDescription Name Property as STRING
		/// </summary>
		public string BookItemDetailDescription { get; set; }
		/// <summary>
		/// Property BookCustCompControl Control/ID as INT
		/// </summary>
		public int BookCustCompControl { get; set; }
		/// <summary>
		/// Property AMSCompControl Control/ID as INT
		/// </summary>
		public int AMSCompControl { get; set; }
		/// <summary>
		/// Property BookCarrierControl Control/ID as INT 
		/// </summary>
		public int BookCarrierControl { get; set; }
		/// <summary>
		/// BookCarrierContact Name Property as STRING		 
		/// </summary>
		public string BookCarrierContact { get; set; }
		/// <summary>
		/// BookCarrierContactPhone Name Property as STRING	   BookProNumber
		/// </summary>
		public string BookCarrierContactPhone { get; set; }
		/// <summary>
		/// BookProNumber Name Property as STRING
		/// </summary>
		public string BookProNumber { get; set; }
		/// <summary>
		/// BookConsPrefix Name Property as STRING	   
		/// </summary>
		public string BookConsPrefix { get; set; }
		/// <summary>
		/// BookOrderSequence Name Property as STRING	
		/// </summary>
		public string BookOrderSequence { get; set; }
		/// <summary>
		/// Property BookCarrOrderNumberSeq Control/ID as INT
		/// </summary>
		public int BookCarrOrderNumberSeq { get; set; }
		/// <summary>
		/// BookCarrOrderNumber Name Property as STRING
		/// </summary>
		public string BookCarrOrderNumber { get; set; }
		/// <summary>
		/// BookLoadPONumber Name Property as STRING	
		/// </summary>
		public string BookLoadPONumber { get; set; }
		/// <summary>
		/// Property BookLoadControl Control/ID as INT
		/// </summary>
		public int BookLoadControl { get; set; }
		/// <summary>
		/// Property BookOrigCompControl Control/ID as INT
		/// </summary>
		public int BookOrigCompControl { get; set; }
		/// <summary>
		/// BookOrigName Name Property as STRING	
		/// </summary>
		public string BookOrigName { get; set; }
		/// <summary>
		/// BookOrigAddress1 Name Property as STRING	
		/// </summary>
		public string BookOrigAddress1 { get; set; }
		/// <summary>
		/// BookOrigCity Name Property as STRING	
		/// </summary>
		public string BookOrigCity { get; set; }
		/// <summary>
		/// BookOrigState Name Property as STRING	
		/// </summary>
		public string BookOrigState { get; set; }
		/// <summary>
		/// BookOrigCountry Name Property as STRING	
		/// </summary>
		public string BookOrigCountry { get; set; }
		/// <summary>
		/// BookOrigZip Name Property as STRING	
		/// </summary>
		public string BookOrigZip { get; set; }
		/// <summary>
		/// BookOrigPhone Name Property as STRING	
		/// </summary>
		public string BookOrigPhone { get; set; }
		/// <summary>
		/// Property BookDestCompControl Control/ID as INT
		/// </summary>
		public int BookDestCompControl { get; set; }
		/// <summary>
		/// BookDestName Name Property as STRING
		/// </summary>
		public string BookDestName { get; set; }
		/// <summary>
		/// BookDestAddress1 Name Property as STRING
		/// </summary>
		public string BookDestAddress1 { get; set; }
		/// <summary>
		/// BookDestCity Name Property as STRING
		/// </summary>
		public string BookDestCity { get; set; }
		/// <summary>
		/// BookDestState Name Property as STRING
		/// </summary>
		public string BookDestState { get; set; }
		/// <summary>
		/// BookDestCountry Name Property as STRING
		/// </summary>
		public string BookDestCountry { get; set; }
		/// <summary>
		/// BookDestZip Name Property as STRING
		/// </summary>
		public string BookDestZip { get; set; }
		/// <summary>
		/// BookDestPhone Name Property as STRING
		/// </summary>
		public string BookDestPhone { get; set; }


		/// <summary>
		/// BookDateOrdered, _BookDateOrdered Property as DateTime
		/// </summary>
		public DateTime? BookDateOrdered { get; set; }
		private string _BookDateOrdered;
		/// <summary>
		/// BookDateLoad, _BookDateLoad Property as DateTime
		/// </summary>
		public DateTime? BookDateLoad { get; set; }
		private string _BookDateLoad;
		/// <summary>
		/// BookDateRequired, _BookDateRequired Property as DateTime
		/// </summary>
		public DateTime? BookDateRequired { get; set; }
		private string _BookDateRequired;

		/// <summary>
		/// Property BookTotalCases Control/ID as INT
		/// </summary>
		public int BookTotalCases { get; set; }
		/// <summary>
		/// Property BookTotalWgt Control/ID as Double
		/// </summary>
		public Double BookTotalWgt { get; set; }
		/// <summary>
		/// Property BookTotalPL Control/ID as Double
		/// </summary>
		public Double BookTotalPL { get; set; }
		/// <summary>
		/// Property BookTotalCube Control/ID as Double
		/// </summary>
		public Double BookTotalCube { get; set; }
		/// <summary>
		/// Property BookTotalPX Control/ID as INT
		/// </summary>
		public int BookTotalPX { get; set; }
		/// <summary>
		/// Property BookStopNo Control/ID as INT
		/// </summary>
		public int BookStopNo { get; set; }
		/// <summary>
		/// Property BookRouteConsFlag Control/ID as Boolean
		/// </summary>
		public Boolean BookRouteConsFlag { get; set; }
		/// <summary>
		/// Property BookODControl Control/ID as INT
		/// </summary>
		public int BookODControl { get; set; }
		/// <summary>
		/// BookShipCarrierProNumber Name Property as STRING
		/// </summary>
		public string BookShipCarrierProNumber { get; set; }
		/// <summary>
		/// BookShipCarrierProNumberRaw Name Property as STRING
		/// </summary>
		public string BookShipCarrierProNumberRaw { get; set; }
		/// <summary>
		/// Property BookShipCarrierProControl Control/ID as INT
		/// </summary>
		public int BookShipCarrierProControl { get; set; }
		/// <summary>
		/// BookShipCarrierName Name Property as STRING
		/// </summary>
		public string BookShipCarrierName { get; set; }
		/// <summary>
		/// BookShipCarrierNumber Name Property as STRING
		/// </summary>
		public string BookShipCarrierNumber { get; set; }
		/// <summary>
		/// Property CarrierNumber Control/ID as INT
		/// </summary>
		public int CarrierNumber { get; set; }
		/// <summary>
		/// CarrierName Name Property as STRING
		/// </summary>
		public string CarrierName { get; set; }
		/// <summary>
		/// CarrierSCAC Name Property as STRING
		/// </summary>
		public string CarrierSCAC { get; set; }
		/// <summary>
		/// LaneNumber Name Property as STRING
		/// </summary>
		public string LaneNumber { get; set; }
		/// <summary>
		/// Property LaneOriginAddressUse Control/ID as Boolean
		/// </summary>
		public Boolean LaneOriginAddressUse { get; set; }
		/// <summary>
		/// Property BookTotalPX Control/ID as INT
		/// </summary>
		public int _OrderTypeNotUsed { get; set; }
		/// <summary>
		/// Property BookStopNo Control/ID as INT
		/// </summary>
		public int OrderType { get; set; }
		/// <summary>
		/// BookShipCarrierProNumber Name Property as STRING
		/// </summary>
		public string OrderTypeMsg { get; set; }
        /// <summary>
        /// Added By LVV On 7/19/18 For v-8.3 TMS365 Scheduler
        /// Used to return the color that the grid row should be in the
        /// Search Orders grid
        /// </summary>
        public string OrderColorCode { get; set; }
		/// <summary>
		/// Added By SK On 7/20/18 For v-8.3 TMS365 Scheduler
		/// Used to return the EquipmentID
		/// Search Orders grid
		/// </summary>
		public string EquipmentID { get; set; }



	}
}