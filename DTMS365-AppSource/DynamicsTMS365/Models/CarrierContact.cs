using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierContact
    {
        public int CarrierContControl { get; set; }
        public int CarrierContLECarControl { get; set; }
        public int CarrierContCarrierControl { get; set; }
        public string CarrierContName { get; set; }
        public string CarrierContTitle { get; set; }
        public string CarrierContactPhone { get; set; }
        public string CarrierContPhoneExt { get; set; }
        public string CarrierContactFax { get; set; }
        public string CarrierContact800 { get; set; }
        public string CarrierContactEMail { get; set; }
        public Boolean CarrierContactDefault { get; set; }
        public Boolean CarrierContSchedContact { get; set; }

        private byte[] _CarrierContUpdated;

        /// <summary>
        /// CarrierContUpdated should be bound to UI, _CarrierContUpdated is only bound on the controller
        /// </summary>
        public string CarrierContUpdated
        {
            get
            {
                if (this._CarrierContUpdated != null) { return Convert.ToBase64String(this._CarrierContUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._CarrierContUpdated = null; } else { this._CarrierContUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _CarrierContUpdated = val; }
        public byte[] getUpdated() { return _CarrierContUpdated; }

    }
}