using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class BookSpotRateData
    {

       
        public string AllocationFormula { get; set; }
        
        public string BFCAllocationFormula { get; set; }
        
        public int BookSpotRateAllocationBFCFormulaControl { get; set; }
       
        public int BookSpotRateAllocationFormulaControl { get; set; }
        
        public bool BookSpotRateAutoCalculateBFC { get; set; }
        
        public decimal BookSpotRateAvgFuelPrice { get; set; }
       
        public int BookSpotRateBookControl { get; set; }
        
        public int BookSpotRateBookReasonCode { get; set; }
        
        public string BookSpotRateBookSHID { get; set; }
        
        public int BookSpotRateCarrierControl { get; set; }
        
        public int BookSpotRateControl { get; set; }
       
        public bool BookSpotRateDeleteLaneFees { get; set; }
       
        public bool BookSpotRateDeleteOrderFees { get; set; }
        
        public bool BookSpotRateDeleteTariffFees { get; set; }
        
        public DateTime? BookSpotRateEffectiveDate { get; set; }
       
        public DateTime? BookSpotRateModDate { get; set; }
       
        public string BookSpotRateModUser { get; set; }
       
        public string BookSpotRateState { get; set; }
       
        public decimal BookSpotRateTotalBFC { get; set; }
       
        public decimal BookSpotRateTotalLineHaulCost { get; set; }
        private byte[] _BookSpotRateUpdated;
        public string BookSpotRateUpdated
        {
            get
            {
                if (this._BookSpotRateUpdated != null)
                {

                    return Convert.ToBase64String(this._BookSpotRateUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._BookSpotRateUpdated = null;

                }

                else
                {

                    this._BookSpotRateUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _BookSpotRateUpdated = val; }
        public byte[] getUpdated() { return _BookSpotRateUpdated; }

        public bool BookSpotRateUserCarrierFuelAddendum { get; set; }
       
        public string CarrierName { get; set; }
        
        public int? CarrierNumber { get; set; }
       
        public string Reason { get; set; }
    }
}