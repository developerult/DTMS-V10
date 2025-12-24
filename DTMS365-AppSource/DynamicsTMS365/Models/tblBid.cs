using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added By LVV on 1/4/17 for v-8.0 Next Stop

namespace DynamicsTMS365.Models
{
    public class tblBid
    {
        public int BidControl { get; set; }
        public int BidLoadTenderControl { get; set; }
        public int BidBidTypeControl { get; set; }
        public int BidCarrierControl { get; set; }
        public int BidCarrierNumber { get; set; }
        public string BidCarrierName { get; set; }
        public string BidCarrierSCAC { get; set; }
        public string BidSHID { get; set; }
        public decimal BidLineHaul { get; set; }
        public decimal BidFuelTotal { get; set; }
        public decimal BidFuelVariable { get; set; }
        public string BidFuelUOM { get; set; }
        public double BidTotalMiles { get; set; }
        public string BidOrigState { get; set; }
        public string BidDestState { get; set; }
        public int BidTotalStops { get; set; }
        public DateTime? BidPosted { get; set; }
        public DateTime? BidExpires { get; set; }
        public int BidStatusCode { get; set; }
        public Boolean BidArchived { get; set; }
        public string BidComments { get; set; }
        public string BidMode { get; set; }
        public int BidErrorCount { get; set; }
        public string BidErrors { get; set; }
        public string BidWarnings { get; set; }
        public string BidInfos { get; set; }
        public Boolean BidInterLine { get; set; }
        public string BidQuoteNumber { get; set; }
        public decimal BidTotalCost { get; set; }
        public double? BidTransitTime { get; set; }
        public DateTime? BidDeliveryDate { get; set; }
        public DateTime? BidQuoteDate { get; set; }
        public double? BidTotalWeight { get; set; }
        public decimal BidDetailTotal { get; set; }
        public double? BidDetailTransitTime { get; set; }
        public decimal BidAdjustments { get; set; }
        public int BidAdjustmentCount { get; set; }
        public int BidBookCarrTarEquipMatControl { get; set; }
        public int BidBookCarrTarEquipControl { get; set; }
        public int BidBookModeTypeControl { get; set; }
        public string BidVendor { get; set; }
        public string BidContractID { get; set; }
        public string BidServiceType { get; set; }
        public int BidTotalPlts { get; set; }
        public int BidTotalQty { get; set; }
        public DateTime? BidModDate { get; set; }
        private string _BidModUser;
        public string BidModUser
        {
            get { return _BidModUser.Left(100); } //uses extension string method Left
            set { _BidModUser = value.Left(100); }
        }
        //Modified by RHR for v-8.5.4.001 on 07/13/2023 added new bid columns
        public decimal? BidCustLineHaul { get; set; }


        public decimal? BidCustTotalCost { get; set; }

        public bool? BidSelectedForExport { get; set; }

        private byte[] _BidUpdated;

        /// <summary>
        /// BidUpdated should be bound to UI, _BidUpdated is only bound on the controller
        /// </summary>
        public string BidUpdated
        {
            get
            {
                if (this._BidUpdated != null)
                {

                    return Convert.ToBase64String(this._BidUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._BidUpdated = null;

                }

                else
                {

                    this._BidUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _BidUpdated = val; }
        public byte[] getUpdated() { return _BidUpdated; }


        public void fillFromRequestInsert(HttpRequest request)
        {
            int intVal = 0;
            decimal decVal = 0;
            float floatVal = 0;
            //Boolean blnVal = true;

            int.TryParse(request["BidControl"] ?? "0", out intVal);
            this.BidControl = intVal;
            intVal = 0;
            int.TryParse(request["BidLoadTenderControl"] ?? "0", out intVal);
            this.BidLoadTenderControl = intVal;
            //intVal = 0;
            //int.TryParse(request["BidBidTypeControl"] ?? "1", out intVal);
            //this.BidBidTypeControl = intVal;
            intVal = 0;
            int.TryParse(request["BidCarrierControl"] ?? "0", out intVal);
            this.BidCarrierControl = intVal;
            //intVal = 0;
            //int.TryParse(request["BidCarrierNumber"] ?? "0", out intVal);
            //this.BidCarrierNumber = intVal;
            //this.BidCarrierName = request["BidCarrierName"] ?? "";
            //this.BidCarrierSCAC = request["BidCarrierSCAC"] ?? "";
            this.BidSHID = request["BidSHID"] ?? "";
            decimal.TryParse(request["BidLineHaul"] ?? "0", out decVal);
            this.BidLineHaul = decVal;
            decVal = 0;
            decimal.TryParse(request["BidFuelVariable"] ?? "0", out decVal);
            this.BidFuelVariable = decVal;
            this.BidFuelUOM = request["BidFuelUOM"] ?? "";
            this.BidOrigState = request["BidOrigState"] ?? "";
            this.BidDestState = request["BidDestState"] ?? "";
            this.BidComments = request["BidComments"] ?? "";
            intVal = 0;
            int.TryParse(request["BidTotalStops"] ?? "0", out intVal);
            this.BidTotalStops = intVal;

            float.TryParse(request["BidTotalMiles"] ?? "0", out floatVal);
            this.BidTotalMiles = floatVal;

            //this.BidPosted = string.IsNullOrEmpty(request["BidPosted"]) ? DateTime.Now : Convert.ToDateTime(request["BidPosted"]);
            //intVal = 0;
            //int.TryParse(request["BidStatusCode"] ?? "1", out intVal);
            //this.BidStatusCode = intVal;
            //Boolean.TryParse(request["BidArchived"] ?? "true", out blnVal);
            //this.BidArchived = blnVal;
            //this.BidModDate = string.IsNullOrEmpty(request["BidModDate"]) ? DateTime.Now : Convert.ToDateTime(request["BidModDate"]);
            //this.BidModUser = request["BidModUser"] ?? "";

            //this.BidUpdated = request["BidUpdated"] ?? "";

        }

        public void fillFromRequest(HttpRequest request)
        {
            int intVal = 0;
            decimal decVal = 0;
            float floatVal = 0;
            //Boolean blnVal = true;
 
            int.TryParse(request["BidControl"] ?? "0", out intVal);
            this.BidControl = intVal;
            intVal = 0;
            int.TryParse(request["BidLoadTenderControl"] ?? "0", out intVal);
            this.BidLoadTenderControl = intVal;
            intVal = 0;
            int.TryParse(request["BidBidTypeControl"] ?? "1", out intVal);
            this.BidBidTypeControl = intVal;
            intVal = 0;
            int.TryParse(request["BidCarrierControl"] ?? "0", out intVal);
            this.BidCarrierControl = intVal;
            intVal = 0;
            int.TryParse(request["BidTotalStops"] ?? "0", out intVal);
            this.BidTotalStops = intVal;
            intVal = 0;
            int.TryParse(request["BidBookCarrTarEquipMatControl"] ?? "0", out intVal);
            this.BidBookCarrTarEquipMatControl = intVal;
            intVal = 0;
            int.TryParse(request["BidBookCarrTarEquipControl"] ?? "0", out intVal);
            this.BidBookCarrTarEquipControl = intVal;
            intVal = 0;
            int.TryParse(request["BidBookModeTypeControl"] ?? "0", out intVal);
            this.BidBookModeTypeControl = intVal;

            this.BidSHID = request["BidSHID"] ?? "";
            decimal.TryParse(request["BidLineHaul"] ?? "0", out decVal);
            this.BidLineHaul = decVal;
            decVal = 0;
            decimal.TryParse(request["BidFuelVariable"] ?? "0", out decVal);
            this.BidFuelVariable = decVal;
            decVal = 0;
            decimal.TryParse(request["BidFuelTotal"] ?? "0", out decVal);
            this.BidFuelTotal = decVal;
            decVal = 0;
            decimal.TryParse(request["BidTotalCost"] ?? "0", out decVal);
            this.BidTotalCost = decVal;

            this.BidFuelUOM = request["BidFuelUOM"] ?? "";
            this.BidOrigState = request["BidOrigState"] ?? "";
            this.BidDestState = request["BidDestState"] ?? "";
            this.BidComments = request["BidComments"] ?? "";

            float.TryParse(request["BidTotalMiles"] ?? "0", out floatVal);
            this.BidTotalMiles = floatVal;

        }



    }
}