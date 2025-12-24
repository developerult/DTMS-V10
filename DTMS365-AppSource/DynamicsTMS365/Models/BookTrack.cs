using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class BookTrack
    {
        public int BookTrackControl { get; set; }
        public int BookTrackBookControl { get; set; }
        public int BookTrackStatus { get; set; }
        public string BookTrackContact { get; set; }
        public string BookTrackComment { get; set; }
        public string BookTrackCommentLocalized { get; set; }
        public string BookTrackCommentKeys { get; set; }
        public DateTime? BookTrackDate { get; set; }
        public DateTime? BookTrackModDate { get; set; }

        private string _BookTrackModUser;
        public string BookTrackModUser
        {
            get { return _BookTrackModUser.Left(100); } //uses extension string method Left
            set { _BookTrackModUser = value.Left(100); }
        }

        private byte[] _BookTrackUpdated;

        /// <summary>
        /// BookTrackUpdated should be bound to UI, _BookTrackUpdated is only bound on the controller
        /// </summary>
        public string BookTrackUpdated
        {
            get
            {
                if (this._BookTrackUpdated != null) { return Convert.ToBase64String(this._BookTrackUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._BookTrackUpdated = null; } else { this._BookTrackUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _BookTrackUpdated = val; }
        public byte[] getUpdated() { return _BookTrackUpdated; }

        //Added fields for cm (Booking Menu Reject)
        public string BookProNumber { get; set; }
        public int BookCarrierControl { get; set; }
        public int CarrierNumber { get; set; }        
        public string CarrierName { get; set; }
        public string BookSHID { get; set; }
        public string BookConsPrefix { get; set; }
        public string BookCarrOrderNumber { get; set; }
        public string LoadStatusDesc { get; set; }
    }
}