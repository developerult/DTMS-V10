using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class NSAcceptBid
    {
        public int LTBookControl { get; set; }
        public int BidControl { get; set; }
        public int BidLoadTenderControl { get; set; }
        public int BidBidTypeControl { get; set; }
        public int BidCarrierControl { get; set; }
        public decimal BidLineHaul { get; set; }
        public decimal BidFuelTotal { get; set; }
        public decimal BidFuelVariable { get; set; }
        public string BidFuelUOM { get; set; }
        public decimal BidTotalCost { get; set; }
        public int BidBookCarrTarEquipMatControl { get; set; }
        public int BidBookCarrTarEquipControl { get; set; }
        public int BidBookModeTypeControl { get; set; }
    }
}