using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added By LVV on 12/23/16 for v-8.0 Next Stop

//***** Depreciated by LVV on 4/6/17 *****
//All these methods were moved to the NextStopLoadController so that all NextStop Load tender grids
//would use the same model. This is to consolidate the places we need to make changed if fields are added etc.
//It didn't make sense to have two models with exactly the same fields
//Eventually the goal would be to depreciate the views and have all methods return LTS.tblLoadTender objects since
//the only thing that is different is the Where clause
//I am leaving these controller classes where they are and have only commented out the lines in the aspx files in case we ever need
//to use them again (for the automated code builder from the database -- I think there was something about naming conventions that this new model might break)


namespace DynamicsTMS365.Models
{
    public class vNSAvailablePendingLoads
    {
        public int LoadTenderControl { get; set; }
        public int LTLoadTenderTypeControl { get; set; }
        public int LTBookControl { get; set; }
        public string LTBookProNumber { get; set; }
        public string LTBookConsPrefix { get; set; }
        public string LTBookSHID { get; set; }
        public string LTBookCarrOrderNumber { get; set; }
        public int? LTBookOrderSequence { get; set; }
        public int? LTBookTotalCases { get; set; }
        public double? LTBookTotalWgt { get; set; }
        public double? LTBookTotalPL { get; set; }
        public int? LTBookTotalCube { get; set; }
        public double? LTBookTotalMiles { get; set; }
        public DateTime? LTBookDateLoad { get; set; }
        public DateTime? LTBookDateRequired { get; set; }
        public string LTBookOrigName { get; set; }
        public string LTBookOrigAddress1 { get; set; }
        public string LTBookOrigAddress2 { get; set; }
        public string LTBookOrigAddress3 { get; set; }
        public string LTBookOrigCity { get; set; }
        public string LTBookOrigState { get; set; }
        public string LTBookOrigCountry { get; set; }
        public string LTBookOrigZip { get; set; }
        public string LTBookDestName { get; set; }
        public string LTBookDestAddress1 { get; set; }
        public string LTBookDestAddress2 { get; set; }
        public string LTBookDestAddress3 { get; set; }
        public string LTBookDestCity { get; set; }
        public string LTBookDestState { get; set; }
        public string LTBookDestCountry { get; set; }
        public string LTBookDestZip { get; set; }
        public int? LTStatusCode { get; set; }
        public string LTMessage { get; set; }
        public Boolean LTArchived { get; set; }
        public DateTime? LTTenderedDate { get; set; }
        public DateTime? LTExpires { get; set; }
        public Boolean LTExpired { get; set; }
    }
}