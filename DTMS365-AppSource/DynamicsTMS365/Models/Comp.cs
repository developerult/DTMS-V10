using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DynamicsTMS365.Models
{
    public class Comp
    {
        public int CompControl { get; set; }
        public int LEAdminControl { get; set; }
        public int CompNumber { get; set; }
        public string CompName { get; set; }
        public string CompLegalEntity { get; set; }
        public string CompAlphaCode { get; set; }
        public string CompAbrev { get; set; }
        public string CompWeb { get; set; }
        public string CompEmail { get; set; }
        public Boolean CompActive { get; set; }
        public string CompStreetAddress1 { get; set; }
        public string CompStreetAddress2 { get; set; }
        public string CompStreetAddress3 { get; set; }
        public string CompStreetCity { get; set; }
        public string CompStreetState { get; set; }
        public string CompStreetZip { get; set; }
        public string CompStreetCountry { get; set; }

        public string CompModDate { get; set; }

      
        public double CompPayTolCurHi { get; set; }//added for Company Migration
        public double CompPayTolCurLo { get; set; } //added for Company Migration
        public double CompPayTolPerHi { get; set; }//added for Company Migration
        public double CompPayTolPerLo { get; set; }//added for Company Migration

        public int CompNatNumber { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompNatName { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompFAAShipID { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompFDAShipID { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public double CompLatitude { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public double CompLongitude { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public short CompTypeCategory { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompNEXTStopAcctNo { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompNEXTStopPsw { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public bool CompNextstopSubmitRFP { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public bool CompNEXTrack { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public bool CompAMS { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public bool CompFinUseImportFrtCost { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public bool CompSilentTender { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public bool CompRestrictCarrierSelection { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public bool CompWarnOnRestrictedCarrierSelection { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompMailAddress1 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompMailAddress2 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompMailAddress3 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompMailCity { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompMailState { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompMailZip { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompMailCountry { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompRailStationName { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompRailSPLC { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompRailFSAC { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompRail333 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompRailR260 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompUser1 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompUser2 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompUser3 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string CompUser4 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint

        public string NewContName { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string NewContTitle { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string NewCont800 { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string NewContPhone { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string NewContPhoneExt { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string NewContFax { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public string NewContEmail { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        public bool NewContTendered { get; set; } //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint


        private string _CompModUser;
        public string CompModUser
        {
            get { return _CompModUser.Left(100); } //uses extension string method Left
            set { _CompModUser = value.Left(100); }
        }

        // Modified by RHR for v-8.5.4.005 on 04/04/2024 added missing data fields from vLEComp365
        public string CompNotifyEmai { get; set; }
        public string CompPhone { get; set; }
        public string CompFax { get; set; }
        public int? CompCarrierLoadAcceptanceAllowedMinutes { get; set; }
        public string CompRejectedLoadsTo { get; set; }
        public string CompRejectedLoadsCc { get; set; }
        public string CompExpiredLoadsTo { get; set; }
        public string CompExpiredLoadsCc { get; set; }
        public string CompAcceptedLoadsTo { get; set; }
        public string CompAcceptedLoadsCc { get; set; }
        public int CompAMSApptDetFieldsVisible { get; set; }
        public bool CompWillLoadOnSunday { get; set; }
        public bool CompWillLoadOnSaturday { get; set; }

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

        // Modified by RHR for v-8.4.0.003 on 07/17/2021 added comp header logCompHeaderLogoLink and CompHeaderLogo properties
        public string CompHeaderLogoLink { get; set; }
        public string CompHeaderLogo { get; set; }

        private byte[] _CompUpdated;

        /// <summary>
        /// CompUpdated should be bound to UI, _CompUpdated is only bound on the controller
        /// </summary>
        public string CompUpdated
        {
            get
            {
                if (this._CompUpdated != null) { return Convert.ToBase64String(this._CompUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._CompUpdated = null; } else { this._CompUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _CompUpdated = val; }
        public byte[] getUpdated() { return _CompUpdated; }


    }
}