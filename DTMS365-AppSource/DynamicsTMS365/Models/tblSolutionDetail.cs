using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class tblSolutionDetail
    {
        public int SolutionAttributeControl { get; set; }
        public int SolutionAttributeTypeControl { get; set; }
        public int SolutionDetailAlternateAddressLaneControl { get; set; }
        public string SolutionDetailAlternateAddressLaneNumber { get; set; }
        public int SolutionDetailBookCarrTarControl { get; set; }
        public int SolutionDetailBookCarrTarEquipControl { get; set; }
        public string SolutionDetailBookCarrTarName { get; set; }
        public int SolutionDetailBookControl { get; set; }
        public DateTime? SolutionDetailBookExpDelDateTime { get; set; }
        public int SolutionDetailBookLoadControl { get; set; }
        public bool SolutionDetailBookLockAllCosts { get; set; }
        public bool SolutionDetailBookLockBFCCost { get; set; }
        public DateTime? SolutionDetailBookModDate { get; set; }
        public string SolutionDetailBookModUser { get; set; }
        public DateTime? SolutionDetailBookMustLeaveByDateTime { get; set; }
        public string SolutionDetailBookNotes { get; set; }
        public double SolutionDetailBookOutOfRouteMiles { get; set; }
        public double? SolutionDetailBookRevLaneBenchMiles { get; set; }
        public double? SolutionDetailBookRevLoadMiles { get; set; }
        public string SolutionDetailBookSHID { get; set; }
        public int? SolutionDetailBookShipCarrierProControl { get; set; }
        public int SolutionDetailCarrierControl { get; set; }
        public string SolutionDetailCarrierEquipmentCodes { get; set; }
        public string SolutionDetailCarrierName { get; set; }
        public int SolutionDetailCarrierNumber { get; set; }
        public string SolutionDetailCom { get; set; }
        public int SolutionDetailCompControl { get; set; }
        public string SolutionDetailCompName { get; set; }
        public string SolutionDetailCompNatName { get; set; }
        public int SolutionDetailCompNatNumber { get; set; }
        public int SolutionDetailCompNumber { get; set; }
        public string SolutionDetailConsPrefix { get; set; }
        public long SolutionDetailControl { get; set; }
        public bool SolutionDetailCustomerApprovalRecieved { get; set; }
        public bool SolutionDetailCustomerApprovalTransmitted { get; set; }
        public DateTime? SolutionDetailDateLoad { get; set; }
        public DateTime? SolutionDetailDateOrdered { get; set; }
        public DateTime? SolutionDetailDateRequested { get; set; }
        public DateTime? SolutionDetailDateRequired { get; set; }
        public int SolutionDetailDefaultRouteSequence { get; set; }
        public string SolutionDetailDestAddress1 { get; set; }
        public string SolutionDetailDestAddress2 { get; set; }
        public string SolutionDetailDestAddress3 { get; set; }
        public string SolutionDetailDestCity { get; set; }
        public int SolutionDetailDestCompControl { get; set; }
        public string SolutionDetailDestCountry { get; set; }
        public double SolutionDetailDestMiles { get; set; }
        public string SolutionDetailDestName { get; set; }
        public double SolutionDetailDestPCMCost { get; set; }
        public double SolutionDetailDestPCMESTCHG { get; set; }
        public string SolutionDetailDestPCMTime { get; set; }
        public decimal SolutionDetailDestPCMTolls { get; set; }
        public string SolutionDetailDestState { get; set; }
        public int SolutionDetailDestStopControl { get; set; }
        public int SolutionDetailDestStopNumber { get; set; }
        public string SolutionDetailDestZip { get; set; }
        public decimal SolutionDetailFinAPActTax { get; set; }
        public decimal SolutionDetailFinServiceFee { get; set; }
        public int SolutionDetailHoldLoad { get; set; }
        public bool SolutionDetailInbound { get; set; }
        public bool SolutionDetailIsHazmat { get; set; }
        public string SolutionDetailLaneName { get; set; }
        public string SolutionDetailLaneNumber { get; set; }
        public bool SolutionDetailLockAllCosts { get; set; }
        public bool SolutionDetailLockBFCCost { get; set; }
        public double SolutionDetailMilesFrom { get; set; }
        public DateTime? SolutionDetailModDate { get; set; }
        public string SolutionDetailModUser { get; set; }
        public int SolutionDetailODControl { get; set; }
        public string SolutionDetailOrderNumber { get; set; }
        public int SolutionDetailOrderSequence { get; set; }
        public string SolutionDetailOrigAddress1 { get; set; }
        public string SolutionDetailOrigAddress2 { get; set; }
        public string SolutionDetailOrigAddress3 { get; set; }
        public string SolutionDetailOrigCity { get; set; }
        public int SolutionDetailOrigCompControl { get; set; }
        public string SolutionDetailOrigCountry { get; set; }
        public double SolutionDetailOrigMiles { get; set; }
        public string SolutionDetailOrigName { get; set; }
        public double SolutionDetailOrigPCMCost { get; set; }
        public double SolutionDetailOrigPCMESTCHG { get; set; }
        public string SolutionDetailOrigPCMTime { get; set; }
        public decimal SolutionDetailOrigPCMTolls { get; set; }
        public string SolutionDetailOrigState { get; set; }
        public int SolutionDetailOrigStopControl { get; set; }
        public int SolutionDetailOrigStopNumber { get; set; }
        public string SolutionDetailOrigZip { get; set; }
        public string SolutionDetailPayCode { get; set; }
        public int SolutionDetailPickNumber { get; set; }
        public int SolutionDetailPickupStopNumber { get; set; }
        public string SolutionDetailPOHdrControl { get; set; }
        public string SolutionDetailPONumber { get; set; }
        public string SolutionDetailProNumber { get; set; }
        public decimal SolutionDetailRevBilledBFC { get; set; }
        public decimal SolutionDetailRevCarrierCost { get; set; }
        public decimal SolutionDetailRevCommCost { get; set; }
        public double SolutionDetailRevCommPercent { get; set; }
        public decimal SolutionDetailRevFreightTax { get; set; }
        public decimal SolutionDetailRevGrossRevenue { get; set; }
        public decimal SolutionDetailRevLoadSavings { get; set; }
        public int SolutionDetailRevNegRevenue { get; set; }
        public decimal SolutionDetailRevNetCost { get; set; }
        public decimal SolutionDetailRevNonTaxable { get; set; }
        public decimal SolutionDetailRevOtherCost { get; set; }
        public decimal SolutionDetailRevTotalCost { get; set; }
        public bool SolutionDetailRouteConsFlag { get; set; }
        public int SolutionDetailRouteGuideControl { get; set; }
        public string SolutionDetailRouteGuideNumber { get; set; }
        public int SolutionDetailRouteTypeCode { get; set; }
        public long SolutionDetailSolutionTruckControl { get; set; }
        public int SolutionDetailStopNo { get; set; }
        public decimal SolutionDetailTotalBFC { get; set; }
        public int SolutionDetailTotalCases { get; set; }
        public int SolutionDetailTotalCube { get; set; }
        public double SolutionDetailTotalPL { get; set; }
        public int SolutionDetailTotalPX { get; set; }
        public double SolutionDetailTotalWgt { get; set; }
        public string SolutionDetailTranCode { get; set; }
        public string SolutionDetailTransType { get; set; }
        public string SolutionDetailTypeCode { get; set; }
        public byte[] _SolutionDetailUpdated { get; set; }
        public tblSolutionTruck tblSolutionTruck { get; set; }

        /// <summary>
        /// tblSolutionDetailsUpdated Property as STRING
        /// </summary>
        public string SolutionDetailUpdated
        {
            get
            {
                if (this._SolutionDetailUpdated != null)
                {

                    return Convert.ToBase64String(this._SolutionDetailUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._SolutionDetailUpdated = null;

                }

                else
                {

                    this._SolutionDetailUpdated = Convert.FromBase64String(value);

                }

            }
        }
        
        public void setUpdated(byte[] val) { _SolutionDetailUpdated = val; }
        
        public byte[] getUpdated() { return _SolutionDetailUpdated; }
    }
}