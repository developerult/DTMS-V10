using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vLECarrierAccessorial
    {
        public int LECAControl { get; set; }
        public int LECALECarControl { get; set; }
        public int LEAdminControl { get; set; }
        public string LegalEntity { get; set; }
        public int LECompControl { get; set; }
        public string CompName { get; set; }
        public int CompNumber { get; set; }
        public int CarrierControl { get; set; }
        public string CarrierName { get; set; }
        public int CarrierNumber { get; set; }
        public string CarrierAlphaCode { get; set; }
        public int AccessorialCode { get; set; }
        public string AccessorialName { get; set; }
        public string AccessorialDesc { get; set; }
        public string Caption { get; set; }
        public string EDICode { get; set; }
        public bool AutoApprove { get; set; }
        public bool AllowCarrierUpdates { get; set; }
        public bool AccessorialVisible { get; set; }
        public bool DynamicAverageValue { get; set; }
        public decimal AverageValue { get; set; }
        public double ApproveToleranceLow { get; set; }
        public double ApproveToleranceHigh { get; set; }
        public double ApproveTolerancePerLow { get; set; }
        public double ApproveTolerancePerHigh { get; set; }
        public string ModUser { get; set; }
        public DateTime? ModDate { get; set; }

        private byte[] _LECAUpdated;

        /// <summary>
        /// LECAUpdated should be bound to UI, _LECAUpdated is only bound on the controller
        /// </summary>
        public string LECAUpdated
        {
            get
            {
                if (this._LECAUpdated != null) { return Convert.ToBase64String(this._LECAUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._LECAUpdated = null; } else { this._LECAUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _LECAUpdated = val; }
        public byte[] getUpdated() { return _LECAUpdated; }


    }
}