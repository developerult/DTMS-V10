using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added By LVV on 3/1/17 for v-8.0 Next Stop

namespace DynamicsTMS365.Models
{
    public class tblBidCostAdj
    {
        public int BidCostAdjControl { get; set; }
        public int BidCostAdjBidControl { get; set; }
        public string BidCostAdjFreightClass { get; set; }
        public float BidCostAdjWeight { get; set; }
        public string BidCostAdjDesc { get; set; }
        public string BidCostAdjDescCode { get; set; }
        public decimal BidCostAdjAmount { get; set; }
        public decimal BidCostAdjRate { get; set; }
        public string BidCostAdjUOM { get; set; }
        public DateTime? BidCostAdjModDate { get; set; }
        //Modified by RHR for v-8.5.4.005 on 02/14/2024
        //  added BidCostAdjTypeControl
        public int? BidCostAdjTypeControl { get; set; }
        private string _BidCostAdjModUser;
        public string BidCostAdjModUser
        {
            get { return _BidCostAdjModUser.Left(100); } //uses extension string method Left
            set { _BidCostAdjModUser = value.Left(100); }
        }

        private byte[] _BidCostAdjUpdated;

        /// <summary>
        /// BidCostAdjUpdated should be bound to UI, _BidCostAdjUpdated is only bound on the controller
        /// </summary>
        public string BidCostAdjUpdated
        {
            get
            {
                if (this._BidCostAdjUpdated != null)
                {

                    return Convert.ToBase64String(this._BidCostAdjUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._BidCostAdjUpdated = null;

                }

                else
                {

                    this._BidCostAdjUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _BidCostAdjUpdated = val; }
        public byte[] getUpdated() { return _BidCostAdjUpdated; }



    }
}