using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vUserLane
    {

        public int LaneControl { get; set; }
        public string LaneName { get; set; }
        public string LaneNumber { get; set; }
        public int USLControl { get; set; }
        public int USLLaneControl { get; set; }
        public DateTime USLModDate { get; set; }
        public string USLModUser { get; set; }
        public int USLUserSecurityControl { get; set; }

        private byte[] _USLUpdated;

        /// <summary>
        /// USLUpdated should be bound to UI, _USLUpdated is only bound on the controller
        /// </summary>
        public string USLUpdated
        {
            get
            {
                if (this._USLUpdated != null) { return Convert.ToBase64String(this._USLUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._USLUpdated = null; } else { this._USLUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _USLUpdated = val; }
        public byte[] getUpdated() { return _USLUpdated; }
    }
}