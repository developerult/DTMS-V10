using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vUserSecurityCarrier
    {
        public int USCControl { get; set; }
        public int USCUserSecurityControl { get; set; }
        public int USCCarrierControl { get; set; }
        public int USCCarrierNumber { get; set; }
        public string CarrierName { get; set; }
        public string CarrierSCAC { get; set; }
        public int USCCarrierContControl { get; set; }
        public string CarrierContName { get; set; }
        public string CarrierContactEMail { get; set; }
        public string CarrierContactPhone { get; set; }
        public string CarrierContPhoneExt { get; set; }
        public string USCCarrierAccounting { get; set; }
        public string USCModDate { get; set; }
        private string _USCModUser;
        public string USCModUser
        {
            get { return _USCModUser.Left(100); } //uses extension string method Left
            set { _USCModUser = value.Left(100); }
        }

        private byte[] _USCUpdated;

        /// <summary>
        /// USCUpdated should be bound to UI, _USCUpdated is only bound on the controller
        /// </summary>
        public string USCUpdated
        {
            get
            {
                if (this._USCUpdated != null) { return Convert.ToBase64String(this._USCUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._USCUpdated = null; } else { this._USCUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _USCUpdated = val; }
        public byte[] getUpdated() { return _USCUpdated; }


    }
}