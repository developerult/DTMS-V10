using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class APMassEntryFees
    {
        public int APMFeesControl { get; set; }
        public int APMFeesAPControl { get; set; }
        public int APMFeesBookControl { get; set; }
        public int APMFeesAccessorialCode { get; set; }
        public string AccessorialName { get; set; }
        public string AccessorialCaption { get; set; }
        public string AccessorialDescription { get; set; }
        public decimal APMFeesValue { get; set; }
        public string APMFeesEDICode { get; set; }
        public string APMFeesCaption { get; set; }
        public bool APMFeesMissingFee { get; set; }
        public int APMFeesStopSequence { get; set; }
        public string APMFeesOrderNumber { get; set; }
        public string APMFeesModUser { get; set; }
        public DateTime? APMFeesModDate { get; set; }
        private byte[] _APMFeesUpdated;

        /// <summary>
        /// APMFeesUpdated should be bound to UI, _APMFeesUpdated is only bound on the controller
        /// </summary>
        public string APMFeesUpdated
        {
            get
            {
                if (this._APMFeesUpdated != null) { return Convert.ToBase64String(this._APMFeesUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._APMFeesUpdated = null; } else { this._APMFeesUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _APMFeesUpdated = val; }
        public byte[] getUpdated() { return _APMFeesUpdated; }
    }
}