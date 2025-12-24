using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierDropLoad
    {
        public int CarrierDropControl { get; set; }
        public int CarrierDropNumber { get; set; }
        public string CarrierDropContact { get; set; }
        public string CarrierDropProNumber { get; set; }
        public string CarrierDropReason { get; set; }
        public string CarrierDropReasonLocalized { get; set; }
        public string CarrierDropReasonKeys { get; set; }
        public DateTime? CarrierDropDate { get; set; }
        public DateTime? CarrierDropTime { get; set; }

        private byte[] _CarrierDropLoadUpdated;

        /// <summary>
        /// CarrierDropLoadUpdated should be bound to UI, _CarrierDropLoadUpdated is only bound on the controller
        /// </summary>
        public string CarrierDropLoadUpdated
        {
            get
            {
                if (this._CarrierDropLoadUpdated != null) { return Convert.ToBase64String(this._CarrierDropLoadUpdated); } return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._CarrierDropLoadUpdated = null; } else { this._CarrierDropLoadUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _CarrierDropLoadUpdated = val; }
        public byte[] getUpdated() { return _CarrierDropLoadUpdated; }


    }
}