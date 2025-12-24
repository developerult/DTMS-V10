using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class EDI214
    {
        public int EDI214Control { get; set; }
        public string BookCarrOrderNumber { get; set; }
        public int BookOrderSequence { get; set; }
        public string BookConsPrefix { get; set; }
        public string CarrierPartnerCode { get; set; }
        public string CompPartnerCode { get; set; }
        public string EventCode { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string BookShipCarrierProNumber { get; set; }
        public string BookShipCarrierNumber { get; set; }
        public string BookShipCarrierName { get; set; }
        public string EventComments { get; set; }
        public string CarrierName { get; set; }
        public string CompName { get; set; }
        public bool EDI214Received { get; set; }
        public DateTime? EDI214ReceivedDate { get; set; }
        public int EDI214StatusCode { get; set; }
        public string EDI214Message { get; set; }
        public bool Archived { get; set; }
        public string EDI214FileName { get; set; }
        public string SHID { get; set; }
        public DateTime? EDI214ModDate { get; set; }

        private string _EDI214ModUser;
        public string EDI214ModUser
        {
            get { return _EDI214ModUser.Left(100); } //uses extension string method Left
            set { _EDI214ModUser = value.Left(100); }
        }

        private byte[] _EDI214Updated;

        /// <summary>
        /// EDI214Updated should be bound to UI, _EDI214Updated is only bound on the controller
        /// </summary>
        public string EDI214Updated
        {
            get
            {
                if(_EDI214Updated != null) { return Convert.ToBase64String(_EDI214Updated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { _EDI214Updated = null; } else { _EDI214Updated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _EDI214Updated = val; }
        public byte[] getUpdated() { return _EDI214Updated; }
    }
}