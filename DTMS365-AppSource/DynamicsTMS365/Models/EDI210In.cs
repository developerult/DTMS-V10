using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class EDI210In
    {
        public int EDI210InControl { get; set; }
        public string APPONumber { get; set; }
        public string APPRONumber { get; set; }
        public string APCNSNumber { get; set; }
        public int APCarrierNumber { get; set; }
        public string APBillNumber { get; set; }
        public DateTime? APBillDate { get; set; }
        public string APCustomerID { get; set; }
        public string APCostCenterNumber { get; set; }
        public double APTotalCost { get; set; }
        public string APBLNumber { get; set; }
        public int APBilledWeight { get; set; }
        public double APTotalTax { get; set; }
        public double APFee1 { get; set; }
        public double APFee2 { get; set; }
        public double APFee3 { get; set; }
        public double APFee4 { get; set; }
        public double APFee5 { get; set; }
        public double APFee6 { get; set; }
        public double APOtherCosts { get; set; }
        public double APCarrierCost { get; set; }
        public int APOrderSequence { get; set; }
        public bool EDI210InReceived { get; set; }
        public DateTime? EDI210InReceivedDate { get; set; }
        public int EDI210InStatusCode { get; set; }
        public string EDI210InMessage { get; set; }
        public bool Archived { get; set; }
        public string EDI210InFileName { get; set; }
        public string CarrierName { get; set; }
        public string CompName { get; set; }
        public string APFeeDesc1 { get; set; }
        public string APFeeDesc2 { get; set; }
        public string APFeeDesc3 { get; set; }
        public string APFeeDesc4 { get; set; }
        public string APFeeDesc5 { get; set; }
        public string APFeeDesc6 { get; set; }
        public DateTime? EDI210InModDate { get; set; }

        private string _EDI210InModUser;
        public string EDI210InModUser
        {
            get { return _EDI210InModUser.Left(100); } //uses extension string method Left
            set { _EDI210InModUser = value.Left(100); }
        }

        private byte[] _EDI210InUpdated;

        /// <summary>
        /// EDI210InUpdated should be bound to UI, _EDI210InUpdated is only bound on the controller
        /// </summary>
        public string EDI210InUpdated
        {
            get
            {
                if (_EDI210InUpdated != null) { return Convert.ToBase64String(_EDI210InUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { _EDI210InUpdated = null; } else { _EDI210InUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _EDI210InUpdated = val; }
        public byte[] getUpdated() { return _EDI210InUpdated; }
    }
}