using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LECarrierAccessorial
    {
        public int LECAControl { get; set; }
        public int LECALECarControl { get; set; }
        public int LECAAccessorialCode { get; set; }
        public string LECACaption { get; set; }
        public string LECAEDICode { get; set; }
        public bool LECAAutoApprove { get; set; }
        public bool LECAAllowCarrierUpdates { get; set; }
        public bool LECAAccessorialVisible { get; set; }
        public bool LECADynamicAverageValue { get; set; }
        public decimal LECAAverageValue { get; set; }
        public double LECAApproveToleranceLow { get; set; }
        public double LECAApproveToleranceHigh { get; set; }
        public double LECAApproveTolerancePerLow { get; set; }
        public double LECAApproveTolerancePerHigh { get; set; }
        public string LECAModUser { get; set; }
        public DateTime? LECAModDate { get; set; }

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