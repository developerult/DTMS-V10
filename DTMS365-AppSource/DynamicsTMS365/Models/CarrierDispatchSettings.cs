using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierDispatchSettings
    {
        public int LECarControl { get; set; } //REPLACE *
        public int LEAdminControl { get; set; }
        public int CarrierControl { get; set; }
        public int DispatchTypeControl { get; set; }
        public bool RateShopOnly { get; set; }
        public bool APIDispatching { get; set; }
        public bool APIStatusUpdates { get; set; }
        public bool ShowAuditFailReason { get; set; }
        public bool ShowPendingFeeFailReason { get; set; }
        public int BillToCompControl { get; set; }
        public string CarrierAccountRef { get; set; }
        public bool LECarUseDefault { get; set; }
        public string LECarExpiredLoadsTo { get; set; }
        public string LECarExpiredLoadsCc { get; set; }
        public int LECarCarrierAcceptLoadMins { get; set; }
        public string LECarBillingAddress1 { get; set; }
        public string LECarBillingAddress2 { get; set; }
        public string LECarBillingAddress3 { get; set; }
        public string LECarBillingCity { get; set; }
        public string LECarBillingState { get; set; }
        public string LECarBillingZip { get; set; }
        public string LECarBillingCountry { get; set; }
        public bool LECarAllowLTLConsolidation { get; set; }
        //Start Modified by RHR for v-8.4 on 4/22/2021
        public bool LECarAllowCarrierAcceptRejectByEmail { get; set; } //Turn Carrier Accept/Reject With Token via Email on Or off
         public bool LECarCarrierAuthCarrierAcceptRejectByEmail { get; set; } //Require Carrier Username And Password for Carrier Accept/Reject in addition to Token
        public int LECarCarrierAuthCarrierAcceptRejectExpMin { get; set; } //Carrier Accept/Reject With Token Expiration Minutes When Null Or zero use Global Parameter AutoExpireTenderTokenMin
        //End Modified by RHR for v-8.4 on 4/22/2021
        // Start Modified by RHR for v-8.5.4.001 on 07/06/2023 added new fields from Table
        public bool LECarWillDriveSunday { get; set; }
        public bool LECarWillDriveSaturday { get; set; }
        public bool LECarUpliftUseCarrierSpecific { get; set; }
        public decimal LECarCarrierSpecificUpliftPerc { get; set; }

    // End Modified by RHR for v-8.5.4.001 on 07/06/2023 added new fields from Table

}
}