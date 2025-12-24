using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{

    /// <summary>CarrierModel cl s for Carrier</summary>
    /// <remarks>
    /// Created by SRP on 3/22/18
    /// Modified By LVV on 10/19/20 for v-8.3.0.001 Task #Task #20201020161708 - Add Master Carrier Page
    /// </remarks>
    public class CarrierModel
    {      
        public int CarrierControl { get; set; }
        public int CarrierNumber { get; set; }
        public string CarrierName { get; set; }
        public string CarrierSCAC { get; set; }

        public string CarrierStreetAddress1 { get; set; }
        public string CarrierStreetAddress2 { get; set; }
        public string CarrierStreetAddress3 { get; set; }
        public string CarrierStreetCity { get; set; }
        public string CarrierStreetState { get; set; }
        public string CarrierStreetCountry { get; set; }
        public string CarrierStreetZip { get; set; }

        public string CarrierMailAddress1 { get; set; }
        public string CarrierMailAddress2 { get; set; }
        public string CarrierMailAddress3 { get; set; }
        public string CarrierMailCity { get; set; }
        public string CarrierMailState { get; set; }
        public string CarrierMailCountry { get; set; }
        public string CarrierMailZip { get; set; }

        public string CarrierAccountNo { get; set; }
        public string CarrierTypeCode { get; set; }
        public string CarrierGeneralInfo { get; set; }
        public bool CarrierActive { get; set; }
        public string CarrierWebSite { get; set; }
        public string CarrierEmail { get; set; }
        public string CarrierNEXTStopAcct { get; set; }
        public string CarrierNEXTStopPwd { get; set; }

        public string rowguid { get; set; }

        public int CarrierGradControl { get; set; }
        public int CarrierGradReliability { get; set; }
        public int CarrierGradBillingAccuracy { get; set; }
        public int CarrierGradFinancialStrength { get; set; }
        public int CarrierGradEquipmentCondition { get; set; }
        public int CarrierGradContactAttitude { get; set; }
        public int CarrierGradDriverAttitude { get; set; }
        public int CarrierGradClaimFrequency { get; set; }
        public int CarrierGradClaimPayment { get; set; }
        public int CarrierGradGeographicCoverage { get; set; }
        public int CarrierGradCustomerService { get; set; }
        public int CarrierGradPriceChangeNotification { get; set; }
        public int CarrierGradPriceChangeFrequency { get; set; }
        public int CarrierGradPriceAggressiveness { get; set; }
        public double CarrierGradAverage { get; set; }

        public int CarrierQualControl { get; set; }
        public DateTime? CarrierQualInsuranceDate { get; set; }
        public bool CarrierQualQualified { get; set; }
        public string CarrierQualAuthority { get; set; }
        public bool CarrierQualContract { get; set; }
        public DateTime? CarrierQualSignedDate { get; set; }
        public DateTime? CarrierQualContractExpiresDate { get; set; }
        public decimal CarrierQualMaxPerShipment { get; set; }
        public decimal CarrierQualMaxAllShipments { get; set; }
        public decimal CarrierQualCurAllExposure { get; set; }

        public int CarrierCodeVal1 { get; set; }
        public int CarrierCodeVal2 { get; set; }
        public int CarrierTruckDefault { get; set; }
        public bool CarrierAllowWebTender { get; set; }
        public bool CarrierIgnoreTariff { get; set; }
        public bool CarrierAutoFinalize { get; set; }

        public string CarrierSmartWayPartnerType { get; set; }
        public decimal CarrierSmartWayScore { get; set; }
        public bool CarrierSmartWayPartner { get; set; }
        
        public bool CarrierUseStdFuelAddendum { get; set; }
        public bool CarrierAutoAssignProNumber { get; set; }
        public string CarrierQualUSDot { get; set; }
        public string CarrierQualCSAScore { get; set; }
        public decimal CarrierCalcOnTimeServiceLevel { get; set; }
        public decimal CarrierCalcOnTimeNoMonthsUsed { get; set; }
        public decimal CarrierAssignedOnTimeServiceLevel { get; set; }

        public string CarrierUser1 { get; set; }
        public string CarrierUser2 { get; set; }
        public string CarrierUser3 { get; set; }
        public string CarrierUser4 { get; set; }
        public string CarrierLegalEntity { get; set; }
        public string CarrierAlphaCode { get; set; }

        public string CarrierModUser { get; set; }
        public DateTime? CarrierModDate { get; set; }

        public byte[] _CarrierUpdated { get; set; }
        public string CarrierUpdated
        {
            get
            {
                if (this._CarrierUpdated != null) { return Convert.ToBase64String(this._CarrierUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._CarrierUpdated = null; }
                else { this._CarrierUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _CarrierUpdated = val; }
        public byte[] getUpdated() { return _CarrierUpdated; }

    }
}