using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vNatFuelCrossTab
    {
        public decimal NatAverage { get; set; }
        public DateTime NatFuelDate { get; set; }
        public int? NatFuelID { get; set; }
        public decimal? ZoneFuelCost1 { get; set; }
        public decimal? ZoneFuelCost2 { get; set; }
        public decimal? ZoneFuelCost3 { get; set; }
        public decimal? ZoneFuelCost4 { get; set; }
        public decimal? ZoneFuelCost5 { get; set; }
        public decimal? ZoneFuelCost6 { get; set; }
        public decimal? ZoneFuelCost7 { get; set; }
        public decimal? ZoneFuelCost8 { get; set; }
        public decimal? ZoneFuelCost9 { get; set; }
        public string ZoneFuelName1 { get; set; }
       public string ZoneFuelName2 { get; set; }
        public string ZoneFuelName3 { get; set; }
        public string ZoneFuelName4 { get; set; }
        public string ZoneFuelName5 { get; set; }
        public string ZoneFuelName6 { get; set; }
        public string ZoneFuelName7 { get; set; }
        public string ZoneFuelName8 { get; set; }
        public string ZoneFuelName9 { get; set; }
        private byte[] _CarrierFuelUpdated;

       
        public string CarrierFuelUpdated
        {
            get
            {
                if (this._CarrierFuelUpdated != null) { return Convert.ToBase64String(this._CarrierFuelUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._CarrierFuelUpdated = null; }
                else { this._CarrierFuelUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _CarrierFuelUpdated = val; }
        public byte[] getUpdated() { return _CarrierFuelUpdated; }
    }
}