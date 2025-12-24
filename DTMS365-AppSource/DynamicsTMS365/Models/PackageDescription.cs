using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class PackageDescription
    {
        public int PkgDescControl { get; set; }
       public string PkgDescDesc { get; set; }
       public string PkgDescFAKClass { get; set; }
        public int PkgDescLEAdminControl { get; set; }
        public string PkgDescName { get; set; }
        public string PkgDescNMFCClass { get; set; }
        public string PkgDescNMFCSubClass { get; set; }
        private string _PkgDescModUser { get; set; }
        public string PkgDescModUser
        {
            get { return _PkgDescModUser.Left(100); } //uses extension string method Left
            set { _PkgDescModUser = value.Left(100); }
        }
        public DateTime? PkgDescModDate { get; set; }
     
        private byte[] _PkgDescUpdated;
        /// <summary>
        /// PkgDescUpdated should be bound to UI, _PkgDescUpdated is only bound on the controller
        /// </summary>
        public string PkgDescUpdated
        {
            get
            {
                if (this._PkgDescUpdated != null)
                {

                    return Convert.ToBase64String(this._PkgDescUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._PkgDescUpdated = null;

                }

                else
                {

                    this._PkgDescUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _PkgDescUpdated = val; }
        public byte[] getUpdated() { return _PkgDescUpdated; }

    }
}