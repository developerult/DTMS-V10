using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class BookPackage
    {
        public int BookPkgBookControl { get; set; }
        public int BookPkgBookHazControl { get; set; }
        public int BookPkgControl { get; set; }
        public int BookPkgCount { get; set; }
        public string BookPkgDescription { get; set; }
        public string BookPkgFAKClass { get; set; }
        public string BookPkgHazmat { get; set; }
        public string BookPkgHazmatTypeCode { get; set; }
        public double BookPkgHeight { get; set; }
        public double BookPkgLength { get; set; }
        public int BookPkgLevelOfDensity { get; set; }
        public DateTime? BookPkgModDate { get; set; }
        public string BookPkgModUser { get; set; }
        public string BookPkgNMFCClass { get; set; }
        public string BookPkgNMFCSubClass { get; set; }
        public int BookPkgPalletTypeID { get; set; }
        public int BookPkgPkgDescControl { get; set; }
        public bool BookPkgStackable { get; set; }
        public double BookPkgWeight { get; set; }
        public double BookPkgWidth { get; set; }
        public string PackageType { get; set; }

        private byte[] _BookPkgUpdated;

        /// <summary>
        /// BookPkgUpdated should be bound to UI, _BookPkgUpdated is only bound on the controller
        /// </summary>
        public string BookPkgUpdated
        {
            get
            {
                if (this._BookPkgUpdated != null)
                {

                    return Convert.ToBase64String(this._BookPkgUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._BookPkgUpdated = null;

                }

                else
                {

                    this._BookPkgUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _BookPkgUpdated = val; }
        public byte[] getUpdated() { return _BookPkgUpdated; }


    }
}