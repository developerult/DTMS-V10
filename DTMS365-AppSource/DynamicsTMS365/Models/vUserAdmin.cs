using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vUserAdmin
    {
        public int UserAdminControl { get; set; }
        public int UserSecurityControl { get; set; }
        public int CompNumber { get; set; }
        public int CompControl { get; set; }
        public string CompName { get; set; }
       

        private byte[] _UserAdminUpdated;

        /// <summary>
        /// UserAdminUpdated should be bound to UI, _UserAdminUpdated is only bound on the controller
        /// </summary>
        public string UserAdminUpdated
        {
            get
            {
                if (this._UserAdminUpdated != null) { return Convert.ToBase64String(this._UserAdminUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._UserAdminUpdated = null; } else { this._UserAdminUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _UserAdminUpdated = val; }
        public byte[] getUpdated() { return _UserAdminUpdated; }


    }
}