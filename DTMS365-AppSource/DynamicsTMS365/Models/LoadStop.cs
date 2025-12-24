using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LoadStop
    {

        public string BookControl { get; set; }
        public string ShipKey { get; set; }
        public string BookProNumber { get; set; }
        public string BookConsPrefix { get; set; }
        public string BookSHID { get; set; }
        public string OrderNumber { get; set; }
        public string LaneOriginAddressUse { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string BookDateLoad { get; set; }
        public string BookDateRequired { get; set; }
        public string BookTotalCases { get; set; }
        public string BookTotalWgt { get; set; }
        public string BookTotalPL { get; set; }
        public string BookTotalCube { get; set; }
        public int SequenceNo { get; set; }
        public int StopNo { get; set; }
        public int PickNo { get; set; }
        public string isPickup { get; set; }
        public string PickupOrDelivery { get; set; }
    }
}