using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Models
{

	/// <summary>
	/// Load Planning UpdateBookFromLoadPlanning Model Data
	/// </summary>
	/// <remarks>
	/// Created by RHR for v-8.5.3.006 on 01/10/2023
	/// </remarks>
	public class LPMassUpdate
    {

		public string BookProNumber { get; set; }
		public string BookDateLoad { get; set; }
		public string BookDateRequired { get; set; }
		public string BookCarrActDate { get; set; }
		public string BookShipCarrierProNumber { get; set; }
		public string BookShipCarrierName { get; set; }
		public string BookShipCarrierNumber { get; set; }
		public string BookCustomerApprovalTransmitted { get; set; }
		public string BookCustomerApprovalRecieved { get; set; }
		public string BookCarrTrailerNo { get; set; }
		public string BookCarrSealNo { get; set; }
		public string BookCarrDriverNo { get; set; }
		public string BookCarrDriverName { get; set; }
		public string BookCarrRouteNo { get; set; }
		public string BookCarrTripNo { get; set; }
		public string BookTrackContact { get; set; }
		public string BookTrackComment { get; set; }
		public string BookTrackStatus { get; set; }
		public string BookRouteConsFlag { get; set; }
		public string BookShipCarrierProNumberRaw { get; set; }
		public string BookShipCarrierProControl { get; set; }
		public string BookRouteTypeCode { get; set; }
		public string BookCarrBLNumber { get; set; }
	}
}