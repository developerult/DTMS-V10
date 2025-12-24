using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vUserGroup
    {       
        public int UserGroupsControl { get; set; }
        public string UserGroupsName { get; set; }
        public int UserGroupsLegalEntityCompControl { get; set; }
        public string LEAdminLegalEntity { get; set; }
        public int LEAdminControl { get; set; }
        public string CompName { get; set; }
        public int CompNumber { get; set; }
        public int UserGroupsUGCControl { get; set; }
        public string UGCName { get; set; }
        public string UGCDesc { get; set; }
        

        private byte[] _UserGroupsUpdated;

        /// <summary>
        /// UserGroupsUpdated should be bound to UI, _UserGroupsUpdated is only bound on the controller
        /// </summary>
        public string UserGroupsUpdated
        {
            get
            {
                if (this._UserGroupsUpdated != null) { return Convert.ToBase64String(this._UserGroupsUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._UserGroupsUpdated = null; } else { this._UserGroupsUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _UserGroupsUpdated = val; }
        public byte[] getUpdated() { return _UserGroupsUpdated; }


    }
}