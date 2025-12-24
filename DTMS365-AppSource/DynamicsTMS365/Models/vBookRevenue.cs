using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vBookRevenue
    {

        public double BookAcutalHeaviestClassWeight { get; set; }
        public bool BookAllowInterlinePoints { get; set; }
        public decimal BookBestDeficitCost { get; set; }
        public double BookBestDeficitWeight { get; set; }
        public double BookBestDeficitWeightBreak { get; set; }
        public double BookBilledLoadWeight { get; set; }
        public int BookBookRevHistRevision { get; set; }
        public string BookCarrierContact { get; set; }
        public string BookCarrierContactPhone { get; set; }
        public int? BookCarrierContControl { get; set; }
        public int BookCarrierControl { get; set; }
        public string BookCarrOrderNumber { get; set; }
        public int BookCarrTarControl { get; set; }
        public int BookCarrTarEquipControl { get; set; }
        public int BookCarrTarEquipMatControl { get; set; }
        public int BookCarrTarEquipMatDetControl { get; set; }
        public int BookCarrTarEquipMatDetID { get; set; }
        public decimal? BookCarrTarEquipMatDetValue { get; set; }
        public string BookCarrTarEquipMatName { get; set; }
        public string BookCarrTarEquipName { get; set; }
        public string BookCarrTarName { get; set; }
        public int BookCarrTarRevisionNumber { get; set; }
        public int BookCarrTruckControl { get; set; }
        public string BookComCode { get; set; }
        public string BookConsPrefix { get; set; }
        public int BookControl { get; set; }
        public bool BookCreditHold { get; set; }
        public int BookCustCompControl { get; set; }
        public DateTime? BookDateLoad { get; set; }
        public DateTime? BookDateRequired { get; set; }
        public int BookDefaultRouteSequence { get; set; }
        public string BookDestAddress1 { get; set; }
        public string BookDestAddress2 { get; set; }
        public string BookDestAddress3 { get; set; }
        public string BookDestCity { get; set; }
        public int? BookDestCompControl { get; set; }
        public string BookDestCountry { get; set; }
        public string BookDestName { get; set; }
        public string BookDestState { get; set; }
        public string BookDestZip { get; set; }
        public DateTime? BookExpDelDateTime { get; set; }
        public decimal? BookFinAPActCost { get; set; }
        public decimal? BookFinAPPayAmt { get; set; }
        public decimal? BookFinAPStdCost { get; set; }
        public decimal? BookFinARBookFrt { get; set; }
        public decimal? BookFinCommStd { get; set; }
        public decimal? BookFinServiceFee { get; set; }
        public string BookHeaviestClass { get; set; }       
        public bool BookLockAllCosts { get; set; }
        public bool BookLockBFCCost { get; set; }
        public double? BookMilesFrom { get; set; }
        public double BookMinAdjustedLoadWeight { get; set; }
        public DateTime? BookModDate { get; set; }
        public int BookModeTypeControl { get; set; }
        public string BookModUser { get; set; }
        public DateTime? BookMustLeaveByDateTime { get; set; }
        public int BookODControl { get; set; }
        public int BookOrderSequence { get; set; }
        public string BookOrigAddress1 { get; set; }
        public string BookOrigAddress2 { get; set; }
        public string BookOrigAddress3 { get; set; }
        public string BookOrigCity { get; set; }
        public int? BookOrigCompControl { get; set; }
        public string BookOrigCountry { get; set; }
        public string BookOrigName { get; set; }
        public string BookOrigState { get; set; }
        public string BookOrigZip { get; set; }
        public double BookOutOfRouteMiles { get; set; }
        public string BookPayCode { get; set; }
        public int BookPickupStopNumber { get; set; }
        public string BookProNumber { get; set; }
        public double BookRatedWeightBreak { get; set; }
        public decimal? BookRevBilledBFC { get; set; }
        public decimal? BookRevCarrierCost { get; set; }
        public decimal? BookRevCommCost { get; set; }
        public double? BookRevCommPercent { get; set; }
        public decimal BookRevDiscount { get; set; }
        public decimal BookRevDiscountMin { get; set; }
        public decimal BookRevDiscountRate { get; set; }
        public decimal BookRevFreightTax { get; set; }
        public decimal? BookRevGrossRevenue { get; set; }
        public double? BookRevLaneBenchMiles { get; set; }
        public decimal BookRevLineHaul { get; set; }
        public double? BookRevLoadMiles { get; set; }
        public decimal? BookRevLoadSavings { get; set; }
        public int? BookRevNegRevenue { get; set; }
        public decimal BookRevNetCost { get; set; }
        public decimal BookRevNonTaxable { get; set; }
        public decimal? BookRevOtherCost { get; set; }
        public decimal? BookRevStopCost { get; set; }
        public int? BookRevStopQty { get; set; }
        public decimal? BookRevTotalCost { get; set; }
        public bool BookRouteConsFlag { get; set; }
        public string BookRouteFinalCode { get; set; }
        public DateTime? BookRouteFinalDate { get; set; }
        public bool BookRouteFinalFlag { get; set; }
        public int BookRouteGuideControl { get; set; }
        public int BookRouteTypeCode { get; set; }
        public string BookSHID { get; set; }
        public string BookShipCarrierDetails { get; set; }
        public string BookShipCarrierName { get; set; }
        public string BookShipCarrierNumber { get; set; }
        public int? BookShipCarrierProControl { get; set; }
        public string BookShipCarrierProNumber { get; set; }
        public string BookShipCarrierProNumberRaw { get; set; }
        public int BookSpotRateAllocationFormula { get; set; }
        public bool BookSpotRateAutoCalcBFC { get; set; }
        public int BookSpotRateBFCAllocationFormula { get; set; }
        public decimal BookSpotRateTotalUnallocatedBFC { get; set; }
        public decimal BookSpotRateTotalUnallocatedLineHaul { get; set; }
        public bool BookSpotRateUseCarrierFuelAddendum { get; set; }
        public bool BookSpotRateUseFuelAddendum { get; set; }
        public short? BookStopNo { get; set; }
        public double BookSummedClassWeight { get; set; }
        public decimal? BookTotalBFC { get; set; }
        public int? BookTotalCases { get; set; }
        public int? BookTotalCube { get; set; }
        public double? BookTotalPL { get; set; }
        public int? BookTotalPX { get; set; }
        public double? BookTotalWgt { get; set; }
        public string BookTranCode { get; set; }
        public string BookTransType { get; set; }
        public string BookTypeCode { get; set; }
        public decimal BookWgtAdjCost { get; set; }
        public double BookWgtAdjWeight { get; set; }
        public double BookWgtAdjWeightBreak { get; set; }
        public double BookWgtRoundingVariance { get; set; }
        public string CompanyName { get; }
        public int CompanyNumber { get; }
        public bool CompFinUseImportFrtCost { get; }
        public bool LaneOriginAddressUse { get; }
        
        private byte[] _BookUpdated;

        /// <summary>
        /// BookUpdated should be bound to UI, _BookUpdated is only bound on the controller
        /// </summary>
        public string BookUpdated
        {
            get
            {
                if (this._BookUpdated != null)
                {

                    return Convert.ToBase64String(this._BookUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._BookUpdated = null;

                }

                else
                {

                    this._BookUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _BookUpdated = val; }
        public byte[] getUpdated() { return _BookUpdated; }
    }
}