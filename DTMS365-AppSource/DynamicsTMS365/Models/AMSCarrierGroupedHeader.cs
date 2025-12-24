using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{   
    public class AMSCarrierGroupedHeader
    {
        public string BookSHID { get; set; }
        public string BookConsPrefix { get; set; }
        public string BookCarrTrailerNo { get; set; }
        public DateTime? BookDateLoad { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public string Warehouse { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public int BookCarrierControl { get; set; }
        public string CarrierName { get; set; }
        public int CarrierNumber { get; set; }				
        public DateTime? BookDateRequired { get; set; }
        //public string BookCarrOrderNumber { get; set; }							   
        //public string BookProNumber { get; set; }		

        //Added by LVV on 6/7/19
        public int BookOrigCompControl { get; set; }
        public int BookDestCompControl { get; set; }
    }
}