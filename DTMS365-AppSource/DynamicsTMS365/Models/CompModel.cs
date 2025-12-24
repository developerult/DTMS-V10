using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// CompModel cl s for Company
    /// </summary>
    /// <remarks>
    /// Created by SRP on 3/22/18
    /// Deprecated by RHR for v-8.5.4.005 on o4/05/2024
    /// do not use.  use Models.Comp.cs instead
    /// </remarks>
    public class CompModel
    {
        /// <summary>
        /// Property CompControl Control/ID   INT
        /// </summary>
        public int CompControl { get; set; }
        /// <summary>
        /// TPDocEDITControl Property   INT
        /// </summary>
        public int TPDocEDITControl { get; set; }
        /// <summary>
        /// TPDocCCEDIControl Property   INT
        /// </summary>
        public int TPDocCCEDIControl { get; set; }
        /// <summary>
        /// TPDocInbound Property   BOOLEAN
        /// </summary>
        public bool TPDocInbound { get; set; }
        /// <summary>
        /// TPDocPublished Property   BOOLEAN
        /// </summary>
        public bool TPDocPublished { get; set; }
        /// <summary>
        /// TPDocDisabled Property   BOOLEAN
        /// </summary>
        public bool TPDocDisabled { get; set; }
        /// <summary>
        /// TPDocCreateDate Property   DateTime
        /// </summary>
        public DateTime TPDocCreateDate { get; set; }
        private string _TPDocCreateUser; //Modified by SRP on 3/1/18
        /// <summary>
        /// TPDocCreateUser Property   String
        /// </summary>
        public string TPDocCreateUser
        {
            get { return _TPDocCreateUser.Left(100); } //uses extension string method Left
            set { _TPDocCreateUser = value.Left(100); }//Modified by SRP on 3/1/18
        }
        /// <summary>
        /// TPDocModDate Property   DateTime
        /// </summary>
        public DateTime TPDocModDate { get; set; }

        private string _TPDocModUser;
        /// <summary>
        /// TPDocModUser Property   String
        /// </summary>
        public string TPDocModUser
        {
            get { return _TPDocModUser.Left(100); } //uses extension string method Left
            set { _TPDocModUser = value.Left(100); }// Modified by SRP on 3/1/18

        }


        private byte[] _TPDocModUpdated;
        /// <summary>
        /// TPDocModUpdated Property   STRING
        /// </summary>
        public string TPDocModUpdated
        {
            get
            {
                if (this._TPDocModUpdated != null)
                {

                    return Convert.ToBase64String(this._TPDocModUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._TPDocModUpdated = null;

                }

                else
                {

                    this._TPDocModUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _TPDocModUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the TPDocModUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _TPDocModUpdated; }

        /// <summary>
        /// getUpdated Method for getting the TPDocModUpdated
        /// </summary>
        /// <returns></returns>
        public int CompNumber { get; set; }


        public string CompName   { get; set; }


        public int CompNatNumber   { get; set; }


        public string CompNatName   { get; set; }


        public string CompStreetAddress1   { get; set; }


        public string CompStreetAddress2 { get; set; }


        public string CompStreetAddress3   { get; set; }


        public string CompStreetCity   { get; set; }


        public string CompStreetState   { get; set; }


        public string CompStreetCountry   { get; set; }


        public string CompStreetZip   { get; set; }


        public string CompMailAddress1   { get; set; }


        public string CompMailAddress2 { get; set; }


        public string CompMailAddress3   { get; set; }


        public string CompMailCity { get; set; }


        public string CompMailState { get; set; }


        public string CompMailCountry   { get; set; }


        public string CompMailZip { get; set; }


        public string CompWeb { get; set; }


        public string CompEmail   { get; set; }


        public string CompModDate { get; set; }


        public string CompModUser   { get; set; }


        public string CompDirections   { get; set; }


        public string CompAbrev { get; set; }


        public bool CompActive   { get; set; }


        public bool CompNEXTrack { get; set; }


        public string CompNEXTStopAcctNo   { get; set; }


        public string CompNEXTStopPsw   { get; set; }


        public bool CompNextstopSubmitRFP   { get; set; }


        public int CompTypeCategory   { get; set; }


        public string CompFAAShipID   { get; set; }


        public DateTime CompFAAShipDate   { get; set; }


        public bool CompUpdated   { get; set; }


        public string rowguid   { get; set; }

        public int CompBudControl   { get; set; }


        public int CompBudSeasControl   { get; set; }


        public string CompBudSeasDescription   { get; set; }


        public double CompBudSeasMo1   { get; set; }


        public double CompBudSeasMo2   { get; set; }


        public double CompBudSeasMo3   { get; set; }


        public double CompBudSeasMo4   { get; set; }


        public double CompBudSeasMo5   { get; set; }


        public double CompBudSeasMo6   { get; set; }


        public double CompBudSeasMo7   { get; set; }


        public double CompBudSeasMo8   { get; set; }


        public double CompBudSeasMo9 { get; set; }


        public double CompBudSeasMo10   { get; set; }


        public double CompBudSeasMo11   { get; set; }


        public double CompBudSeasMo12   { get; set; }


        public decimal CompBudSlsBudgetMo1   { get; set; }


        public decimal CompBudSlsBudgetMo2   { get; set; }


        public decimal CompBudSlsBudgetMo3   { get; set; }


        public decimal CompBudSlsBudgetMo4   { get; set; }


        public decimal CompBudSlsBudgetMo5   { get; set; }


        public decimal CompBudSlsBudgetMo6   { get; set; }


        public decimal CompBudSlsBudgetMo7 { get; set; }


        public decimal CompBudSlsBudgetMo8 { get; set; }


        public decimal CompBudSlsBudgetMo9 { get; set; }


        public decimal CompBudSlsBudgetMo10 { get; set; }


        public decimal CompBudSlsBudgetMo11 { get; set; }


        public decimal CompBudSlsBudgetMo12 { get; set; }


        public decimal CompBudSlsBudgetMoTotal { get; set; }


        public decimal CompBudSlsActualMo1 { get; set; }


        public decimal CompBudSlsActualMo2 { get; set; }


        public decimal CompBudSlsActualMo3 { get; set; }


        public decimal CompBudSlsActualMo4 { get; set; }


        public decimal CompBudSlsActualMo5 { get; set; }


        public decimal CompBudSlsActualMo6 { get; set; }


        public decimal CompBudSlsActualMo7 { get; set; }


        public decimal CompBudSlsActualMo8 { get; set; }


        public decimal CompBudSlsActualMo9 { get; set; }


        public decimal CompBudSlsActualMo10 { get; set; }


        public decimal CompBudSlsActualMo11 { get; set; }


        public decimal CompBudSlsActualMo12 { get; set; }


        public decimal CompBudSlsActualMoTotal { get; set; }


        public double CompBudSlsMarginBudget   { get; set; }


        public double CompBudSlsMarginActual { get; set; }


        public decimal CompBudCogsBudgetMo1   { get; set; }


        public decimal CompBudCogsBudgetMo2 { get; set; }


        public decimal CompBudCogsBudgetMo3 { get; set; }


        public decimal CompBudCogsBudgetMo4 { get; set; }


        public decimal CompBudCogsBudgetMo5   { get; set; }


        public decimal CompBudCogsBudgetMo6   { get; set; }


        public decimal CompBudCogsBudgetMo7 { get; set; }


        public decimal CompBudCogsBudgetMo8 { get; set; }


        public decimal CompBudCogsBudgetMo9 { get; set; }


        public decimal CompBudCogsBudgetMo10 { get; set; }


        public decimal CompBudCogsBudgetMo11 { get; set; }


        public decimal CompBudCogsBudgetMo12 { get; set; }


        public decimal CompBudCogsBudgetMoTotal { get; set; }


        public decimal CompBudCogsActualMo1 { get; set; }


        public decimal CompBudCogsActualMo2 { get; set; }


        public decimal CompBudCogsActualMo3 { get; set; }


        public decimal CompBudCogsActualMo4 { get; set; }


        public decimal CompBudCogsActualMo5 { get; set; }


        public decimal CompBudCogsActualMo6 { get; set; }


        public decimal CompBudCogsActualMo7 { get; set; }


        public decimal CompBudCogsActualMo8 { get; set; }


        public decimal CompBudCogsActualMo9 { get; set; }


        public decimal CompBudCogsActualMo10 { get; set; }


        public decimal CompBudCogsActualMo11 { get; set; }


        public decimal CompBudCogsActualMo12 { get; set; }


        public decimal CompBudCogsActualMoTotal { get; set; }


        public double CompBudCogsMarginBudget   { get; set; }


        public double CompBudCogsMarginActual { get; set; }


        public decimal CompBudProfitBudgetMo1 { get; set; }


        public decimal CompBudProfitBudgetMo2 { get; set; }


        public decimal CompBudProfitBudgetMo3 { get; set; }


        public decimal CompBudProfitBudgetMo4 { get; set; }


        public decimal CompBudProfitBudgetMo5 { get; set; }


        public decimal CompBudProfitBudgetMo6 { get; set; }


        public decimal CompBudProfitBudgetMo7 { get; set; }


        public decimal CompBudProfitBudgetMo8 { get; set; }


        public decimal CompBudProfitBudgetMo9   { get; set; }


        public decimal CompBudProfitBudgetMo10   { get; set; }


        public decimal CompBudProfitBudgetMo11   { get; set; }


        public decimal CompBudProfitBudgetMo12   { get; set; }


        public decimal CompBudProfitBudgetMoTotal   { get; set; }


        public decimal CompBudProfitActualMo1   { get; set; }


        public decimal CompBudProfitActualMo2   { get; set; }


        public decimal CompBudProfitActualMo3 { get; set; }


        public decimal CompBudProfitActualMo4 { get; set; }


        public decimal CompBudProfitActualMo5 { get; set; }


        public decimal CompBudProfitActualMo6 { get; set; }


        public decimal CompBudProfitActualMo7 { get; set; }


        public decimal CompBudProfitActualMo8 { get; set; }


        public decimal CompBudProfitActualMo9 { get; set; }


        public decimal CompBudProfitActualMo10 { get; set; }


        public decimal CompBudProfitActualMo11 { get; set; }


        public decimal CompBudProfitActualMo12 { get; set; }


        public decimal CompBudProfitActualMoTotal { get; set; }


        public decimal CompBudProfitMarginBudget { get; set; }


        public decimal CompBudProfitMarginActual { get; set; }


        public int CompFinControl   { get; set; }


        public string CompFinDuns   { get; set; }


        public string CompFinTaxID { get; set; }


        public string CompFinPaymentForm   { get; set; }


        public string CompFinSIC   { get; set; }


        public int CompFinPaymentDiscount   { get; set; }


        public int CompFinPaymentDays   { get; set; }


        public int CompFinPaymentNetDays   { get; set; }


        public string CompFinCommTerms   { get; set; }


        public string CompFinCommTermsPer   { get; set; }


        public int CompFinCommCompControl   { get; set; }


        public int CompFinCreditLimit   { get; set; }


        public int CompFinCreditUsed   { get; set; }


        public int CompFinCreditAvail   { get; set; }


        public double CompFinYTDbookedCurr   { get; set; }


        public double CompFinYTDbookedLast   { get; set; }


        public double CompFinYTDcarrierCurr   { get; set; }


        public double CompFinYTDcarrierLast   { get; set; }


        public double CompFinYTDsavingsCurr { get; set; }


        public double CompFinYTDsavingsLast   { get; set; }


        public int CompFinYTDRevenuesCurr   { get; set; }


        public int CompFinYTDRevenuesLast   { get; set; }


        public int CompFinYTDnoLoadsCurr   { get; set; }


        public int CompFinYTDnoLoadsLast   { get; set; }


        public bool CompFinInvPrnCode   { get; set; }


        public bool CompFinInvEMailCode   { get; set; }


        public int CompFinCurType   { get; set; }


        public int CompFinUser1   { get; set; }


        public int CompFinUser2   { get; set; }


        public int CompFinUser3   { get; set; }


        public string CompFinUser4   { get; set; }


        public string CompFinUser5   { get; set; }


        public DateTime CompFinCustomerSince   { get; set; }


        public string CompFinCardType   { get; set; }


        public string CompFinCardName   { get; set; }


        public string CompFinCardExpires   { get; set; }


        public string CompFinCardAuthorizor   { get; set; }


        public string CompFinCardAuthPassword   { get; set; }


        public bool CompFinUseImportFrtCost   { get; set; }


        public decimal CompFinBkhlFlatFee   { get; set; }


        public double CompFinBkhlCostPerc   { get; set; }


        public double CompLatitude   { get; set; }


        public double CompLongitude   { get; set; }


        public string CompMailTo   { get; set; }


        public string CompFDAShipID   { get; set; }


        public bool CompAMS   { get; set; }


        public double CompPayTolPerLo   { get; set; }


        public double CompPayTolPerHi   { get; set; }


        public double CompPayTolCurLo   { get; set; }


        public double CompPayTolCurHi   { get; set; }


        public int CompPayTolWgtFrom   { get; set; }


        public int CompPayTolWgtTo   { get; set; }


        public double CompPayTolTaxPerLo   { get; set; }


        public double CompPayTolTaxPerHi { get; set; }


        public int CompFinBillToCompControl   { get; set; }


        public bool CompSilentTender   { get; set; }

        // Modified by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone default settings
        public string _CompTimeZone = "Central Standard Time";
        public string CompTimeZone
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CompTimeZone))
                {
                    _CompTimeZone = "Central Standard Time";
                }
                return _CompTimeZone.Left(100);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "Central Standard Time";
                }
                _CompTimeZone = value.Left(100);
            }
        }


        public string CompRailStationName   { get; set; }


        public string CompRailSPLC   { get; set; }


        public string CompRailFSAC   { get; set; }


        public string CompRail333   { get; set; }


        public string CompRailR260   { get; set; }


        public bool CompIsTransLoad   { get; set; }


        public string CompUser1   { get; set; }


        public string CompUser2   { get; set; }


        public string CompUser3   { get; set; }


        public string CompUser4 { get; set; }


        public string CompLegalEntity { get; set; }


        public string CompAlphaCode   { get; set; }


        public double CompFinFBToleranceHigh   { get; set; }


        public double CompFinFBToleranceLow   { get; set; }


        public bool CompRestrictCarrierSelection   { get; set; }


        public bool CompWarnOnRestrictedCarrierSelection   { get; set; }


        public string CompNotifyEmail   { get; set; }


        public string CompPhone   { get; set; }


        public string CompFax   { get; set; }


        public int CompCarrierLoadAcceptanceAllowedMinutes  { get; set; }


        public string CompRejectedLoadsTo   { get; set; }


        public string CompRejectedLoadsCc   { get; set; }


        public string CompExpiredLoadsTo   { get; set; }


        public string CompExpiredLoadsCc   { get; set; }


        public string CompAcceptedLoadsTo   { get; set; }


        public string CompAcceptedLoadsCc   { get; set; }


        public string CompHeaderLogo   { get; set; }


        public string CompHeaderLogoLink   { get; set; }
    }
}