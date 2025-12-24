using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Address
    {

        public int BookControl { get; set; }
        public string ShipKey { get; set; }
        public string BookProNumber { get; set; }
        public string BookConsPrefix { get; set; }
        public Boolean LaneOriginAddressUse { get; set; }
        public string strAddress { get; set; }
        public string strCity { get; set; }
        public string strState { get; set; }
        public string strZip { get; set; }
        public DateTime? BookDateLoad { get; set; }
        public DateTime? BookDateRequired { get; set; }
        public Int16? BookStopNo { get; set; }
        public Int16? BookPickNo { get; set; }
        public Int16? BookRouteSequenceNo { get; set; }
        public string BookCarrOrderNumber { get; set; }
        public string BookSHID { get; set; }
    }
}