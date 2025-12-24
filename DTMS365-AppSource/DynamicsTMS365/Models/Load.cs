using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Load
    {
        public string ShipKey { get; set; }
        public int Orders  { get; set; }
        public string BookConsPrefix { get; set; }
        public Boolean BookRouteConsFlag { get; set; }
        public int BookCarrierControl { get; set; }
        public string CarrierName { get; set; }
        public int? CarrierNumber { get; set; }
        public string CarrierAlphaCode { get; set; }
        public string CarrierSCAC { get; set; }
        public string BookSHID { get; set; }
        public string BookTranCode { get; set; }
        public int? TotalCases { get; set; }
        public double? TotalWgt { get; set; }
        public double? TotalPL { get; set; }
        public int? TotalCube { get; set; }
        public int? MaxLaneTLCases { get; set; }
        public double? MaxLaneTLWGT { get; set; }
        public double? MaxLaneTLPL { get; set; }
        public int? MaxLaneTLCube { get; set; }
        public DateTime? LoadDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}