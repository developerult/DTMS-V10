using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class APMassEntry
    {
        public int APControl { get; set; }
        public int APCarrierNumber { get; set; }
        public string APBillNumber { get; set; }
        public DateTime? APBillDate { get; set; }
        public string APPONumber { get; set; }
        public string APCustomerID { get; set; }
        public string APCostCenterNumber { get; set; }
        public decimal APTotalCost { get; set; }
        public string APPRONumber { get; set; }
        public string APBLNumber { get; set; }
        public int APBilledWeight { get; set; }
        public string APCNSNumber { get; set; }
        public DateTime? APReceivedDate { get; set; }
        public string APPayCode { get; set; }
        public bool APElectronicFlag { get; set; }
        public bool APApprovedFlag { get; set; }
        public string APMessage { get; set; }
        public decimal APTotalTax { get; set; }
        public decimal APFee1 { get; set; }
        public decimal APFee2 { get; set; }
        public decimal APFee3 { get; set; }
        public decimal APFee4 { get; set; }
        public decimal APFee5 { get; set; }
        public decimal APFee6 { get; set; }
        public decimal APOtherCosts { get; set; }
        public decimal APCarrierCost { get; set; }
        public bool APExportFlag { get; set; }
        public int APOrderSequence { get; set; }
        public decimal APTaxDetail1 { get; set; }
        public decimal APTaxDetail2 { get; set; }
        public decimal APTaxDetail3 { get; set; }
        public decimal APTaxDetail4 { get; set; }
        public decimal APTaxDetail5 { get; set; }
        public string APSHID { get; set; }
        public string APShipCarrierProNumber { get; set; }
        public decimal APReduction { get; set; } //Added By LVV 3/24/20 v-8.2.1.006
        public int APReductionReason { get; set; } //Added By LVV 3/24/20 v-8.2.1.006
        public decimal APReductionAdjustedCost { get; set; } //Added By LVV 3/24/20 v-8.2.1.006
        public string APModUser { get; set; }
        public DateTime? APModDate { get; set; }
        private byte[] _APUpdated;

        /// <summary>
        /// APUpdated should be bound to UI, _APUpdated is only bound on the controller
        /// </summary>
        public string APUpdated
        {
            get
            {
                if (this._APUpdated != null) { return Convert.ToBase64String(this._APUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._APUpdated = null; } else { this._APUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _APUpdated = val; }
        public byte[] getUpdated() { return _APUpdated; }
    }
}