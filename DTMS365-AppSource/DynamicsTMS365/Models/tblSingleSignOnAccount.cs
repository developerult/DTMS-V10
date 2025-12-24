using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DynamicsTMS365.Models
{
    public class tblSingleSignOnAccount
    {
        public int SSOAControl { get; set; }
        public string SSOAName { get; set; }
        public string SSOADesc { get; set; }
        public string SSOAClientID { get; set; }
        public string SSOALoginURL { get; set; }
        public string SSOADataURL { get; set; }
        public string SSOARedirectURL { get; set; }
        public string SSOAClientSecret { get; set; }
        public string SSOAAuthCode { get; set; }
        public int? SSOABidTypeControl { get; set; }
        public DateTime? SSOAModDate { get; set; }

        private string _SSOAModUser;
        public string SSOAModUser
        {
            get { return _SSOAModUser.Left(100); } //uses extension string method Left
            set { _SSOAModUser = value.Left(100); }
        }

        private byte[] _SSOAUpdated;

        /// <summary>
        /// SSOAUpdated should be bound to UI, _SSOAUpdated is only bound on the controller
        /// </summary>
        public string SSOAUpdated
        {
            get
            {
                if (this._SSOAUpdated != null) { return Convert.ToBase64String(this._SSOAUpdated); } return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._SSOAUpdated = null; } else { this._SSOAUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _SSOAUpdated = val; }
        public byte[] getUpdated() { return _SSOAUpdated; }


    }
}