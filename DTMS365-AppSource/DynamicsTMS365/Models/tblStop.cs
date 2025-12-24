using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added By LVV on 9/19/19 for Bing Maps

namespace DynamicsTMS365.Models
{
    public class tblStop
    {
        public int StopControl { get; set; }
        public string StopName { get; set; }
        public string StopAddress1 { get; set; }
        public string StopCity { get; set; }
        public string StopState { get; set; }
        public string StopZip { get; set; }
        public string StopCountry { get; set; }
        public double StopLatitude { get; set; }
        public double StopLongitude { get; set; }
        public DateTime? StopModDate { get; set; }
        private string _StopModUser;
        public string StopModUser
        {
            get { return _StopModUser.Left(100); } //uses extension string method Left
            set { _StopModUser = value.Left(100); }
        }

        private byte[] _StopUpdated;

        /// <summary>
        /// StopUpdated should be bound to UI, _StopUpdated is only bound on the controller
        /// </summary>
        public string StopUpdated
        {
            get
            {
                if (this._StopUpdated != null) { return Convert.ToBase64String(this._StopUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._StopUpdated = null; } else { this._StopUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _StopUpdated = val; }
        public byte[] getUpdated() { return _StopUpdated; }


        private string _AddressString = "";
        public string AddressString
        {
            get { return _AddressString; }
        }

        public void setAddressString()
        {
            string segSep = "", temp = "";
            if (!string.IsNullOrWhiteSpace(StopName)) { temp += StopName + " - "; }
            if (!string.IsNullOrWhiteSpace(StopAddress1)) { temp += segSep + StopAddress1; segSep = ", "; }
            if (!string.IsNullOrWhiteSpace(StopCity)) { temp += segSep + StopCity; segSep = ", "; }
            if (!string.IsNullOrWhiteSpace(StopState)) { temp += segSep + StopState; segSep = ", "; }
            if (!string.IsNullOrWhiteSpace(StopZip)) { temp += segSep + StopZip; segSep = " "; }
            if (!string.IsNullOrWhiteSpace(StopCountry)) { temp += segSep + StopCountry; segSep = " "; }
            if (StopLatitude != 0 || StopLongitude != 0) { temp += segSep + "(" + StopLatitude.ToString() + ", " + StopLongitude.ToString() + ")"; }
            _AddressString = temp;
        }

    }
}