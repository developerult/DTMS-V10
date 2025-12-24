
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class RateRequestOrder
    {
        public int ID { get; set; }
        public string ShipKey { get; set; }
        public string BookConsPrefix { get; set; }
        public string ShipDate { get; set; }
        public string DeliveryDate { get; set; }
        public int BookCustCompControl { get; set; }
        public string CompName { get; set; }
        public int CompNumber { get; set; }
        public string CompAlphaCode { get; set; }
        public int BookCarrierControl { get; set; }
        public string CarrierName { get; set; }
        public int CarrierNumber { get; set; }
        public string CarrierAlphaCode { get; set; }
        public int TotalCases { get; set; }
        public double TotalWgt { get; set; }
        public double TotalPL { get; set; }
        public int TotalCube { get; set; }
        public int TotalStops { get; set; }
        public RateRequestStop Pickup { get; set; }
        public RateRequestStop[] Stops { get; set; }
        public string[] Accessorials { get; set; } //LVV ADD
              
    }

    public class RateRequestStop
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int BookControl { get; set; }
        public string BookProNumber { get; set; }
        public int StopIndex { get; set; }
        public string BookCarrOrderNumber { get; set; }
        public int CompControl { get; set; }
        public string CompName { get; set; }
        public string CompAddress1 { get; set; }
        public string CompAddress2 { get; set; }
        public string CompAddress3 { get; set; }
        public string CompCity { get; set; }
        public string CompState { get; set; }
        public string CompCountry { get; set; }
        public string CompPostalCode { get; set; }
        public bool IsPickup { get; set; }
        public int StopNumber { get; set; }
        public int TotalCases { get; set; }
        public double TotalWgt { get; set; }
        public double TotalPL { get; set; }
        public int TotalCube { get; set; }
        public string LoadDate { get; set; }
        public string SHID { get; set; }
        public string RequiredDate { get; set; }
        public RateRequestItem[] Items { get; set; }
    }

    public class RateRequestItem
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int LoadID { get; set; }
        public int ItemStopIndex { get; set; }
        public int ItemControl { get; set; }
        public string ItemNumber { get; set; }
        public decimal Weight { get; set; }
        //"lbs"
        public string WeightUnit { get; set; }
        //70
        public string FreightClass { get; set; }
        public int PalletCount { get; set; }
        private int _numPieces;
        public int NumPieces {
            get{
                int.TryParse(this.Quantity, out _numPieces);
                return _numPieces;
            }

            set {
                _numPieces = value;               
            }
        }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string HazmatId { get; set; }
        public string Code { get; set; }
        public string HazmatClass { get; set; }
        public bool IsHazmat { get; set; }
        public string Pieces { get; set; }
        public string PackageType { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Density { get; set; }
        public string NMFCItem { get; set; }
        public string NMFCSub { get; set; }
        public bool Stackable { get; set; }
    }

}