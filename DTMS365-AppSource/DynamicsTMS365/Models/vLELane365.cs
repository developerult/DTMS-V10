using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;
using System.Security.Policy;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// Web Model of LTL vLELane365 data object
    /// </summary>
    /// //<remarks>
    /// Modified by RHR for v-8.4 on 06/24/2021 added logic to support time only fields
    ///   LaneRecHourStart ,LaneRecHourStop ,LaneDestHourStart ,LaneDestHourStop 
    ///   Added static methods selectModelData and  selectLTSData
    ///   requires new using statements Core and Data Access Layer
    /// </remarks>
    public class vLELane365
    {
        public int LEAdminControl { get; set; }
        public int LTTransType { get; set; }
        public int LaneControl { get; set; }
        public int LaneCompControl { get; set; }
        public string CompName { get; set; }
        public string CompNumber { get; set; }
        public string LaneNumber { get; set; }
        public string LaneName { get; set; }
        public string LaneLegalEntity { get; set; }
        public bool LaneActive { get; set; }
        public bool LaneOriginAddressUse { get; set; }
        public string LaneTransType { get; set; }
        public string TransType { get; set; }
        public string LaneTempType { get; set; }
        public string TempType { get; set; }
        public string LaneComments { get; set; }
        public string LaneCommentsConfidential { get; set; }
        public int LaneOrigCompControl { get; set; }
        public string LaneOrigName { get; set; }
        public string LaneOrigAddress1 { get; set; }
        public string LaneOrigAddress2 { get; set; }
        public string LaneOrigAddress3 { get; set; }
        public string LaneOrigCity { get; set; }
        public string LaneOrigState { get; set; }
        public string LaneOrigZip { get; set; }
        public string LaneOrigCountry { get; set; }
        public string LaneOrigContactName { get; set; }
        public string LaneOrigContactPhone { get; set; }
        public string LaneOrigContactEmail { get; set; }
        public string LaneOrigEmergencyContactPhone { get; set; }
        public string LaneOrigEmergencyContactName { get; set; }
        public bool LaneAppt { get; set; }
        public string LaneRecHourStart { get; set; } // Modified by RHR for v-8.4 on 06/24/2021
        public string LaneRecHourStop { get; set; } // Modified by RHR for v-8.4 on 06/24/2021
        public int LaneDestCompControl { get; set; }
        public string LaneDestName { get; set; }
        public string LaneDestAddress1 { get; set; }
        public string LaneDestAddress2 { get; set; }
        public string LaneDestAddress3 { get; set; }
        public string LaneDestCity { get; set; }
        public string LaneDestState { get; set; }
        public string LaneDestZip { get; set; }
        public string LaneDestCountry { get; set; }
        public string LaneDestContactName { get; set; }
        public string LaneDestContactPhone { get; set; }
        public string LaneDestContactEmail { get; set; }
        public string LaneDestEmergencyContactPhone { get; set; }
        public string LaneDestEmergencyContactName { get; set; }
        public bool LaneAptDelivery { get; set; }
        public string LaneDestHourStart { get; set; } // Modified by RHR for v-8.4 on 06/24/2021
        public string LaneDestHourStop { get; set; } // Modified by RHR for v-8.4 on 06/24/2021
        //Capacity Settings
        public double LaneTLWgt { get; set; } //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        public int LaneTLCases { get; set; } //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        public int LaneTLCube { get; set; } //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        public double LaneTLPL { get; set; } //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365

        //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        public string LaneNumberMaster { get; set; }
        public string LaneNameMaster { get; set; }
        public string LaneCarrierEquipmentCodes { get; set; }
        public string txtLaneDefaultCarrier { get; set; }
        public string LaneDefaultCarrierContact { get; set; }
        public string LaneDefaultCarrierPhone { get; set; }
        public int LaneStops { get; set; }
        public bool LaneDefaultCarrierUse { get; set; }
        public bool LaneAutoTenderFlag { get; set; }
        public bool LaneDoNotInvoice { get; set; }
        public bool LaneAllowInterline { get; set; }
        public bool LaneCascadingDispatchingFlag { get; set; }
        public string LaneUser1 { get; set; }
        public string LaneUser2 { get; set; }
        public string LaneUser3 { get; set; }
        public string LaneUser4 { get; set; }
        public bool LanePalletExchange { get; set; }

        public string _LanePalletType = "N";
        public string LanePalletType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LanePalletType))
                {
                    _LanePalletType = "N";
                }
                return _LanePalletType.Left(100);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "N";
                }
                _LanePalletType = value.Left(100);
            }
        }


        public int LaneOLTBenchmark { get; set; }
        public int LaneTLTBenchmark { get; set; }
        public string LanePrimaryBuyer { get; set; }
        public int LaneRecMinIn { get; set; }
        public int LaneRecMinUnload { get; set; }
        public int LaneRecMinOut { get; set; }
        public string LaneConsigneeNumber { get; set; }
        public double LaneBFC { get; set; }
        public string LaneBFCType { get; set; }
        private int _LaneModeTypeControl = 3;
        public int LaneModeTypeControl { 
            get { if (_LaneModeTypeControl == 0) { _LaneModeTypeControl = 3; } return _LaneModeTypeControl; }
            set { if (value == 0) { value = 3; }  _LaneModeTypeControl = value; }
        }

        public string LanePortofEntry { get; set; }
        public bool LaneRestrictCarrierSelection { get; set; }
        public bool LaneWarnOnRestrictedCarrierSelection { get; set; }
        public double LaneBenchMiles { get; set; }
        public double LaneLatitude { get; set; }
        public double LaneLongitude { get; set; }
        public bool LaneIsTransLoad { get; set; }
        public bool LaneIsCrossDockFacility { get; set; }
        public int LaneRouteGuideControl { get; set; }
        public int LaneDefaultRouteSequence { get; set; }
        public int LaneRouteTypeCode { get; set; }

        //Start Modified by RHR for v-8.4 on 4/22/2021
        public string LaneWeightUnit { get; set; } //Added because it was missig holds values like "LB" "KG" , may not be used?
        public string LaneLengthUnit { get; set; } //Added because it was missig holds values like "IN" "CM" "FT" "M" , may not be used?
        public bool LaneAllowCarrierBookApptByEmail { get; set; } //Turn Carrier Book Appt With Token via Email on or off
        public bool LaneRequireCarrierAuthBookApptByEmail { get; set; } //Require Carrier Username and Password for Carrier Book Appt in addition to Token
        public bool LaneUseCarrieContEmailForBookApptByEmail { get; set; } //Use Default Carrier Contact Email instead of Book Appt Email

        public string LaneCarrierBookApptviaTokenEmail { get; set; } //Default Email when LaneAllowCarrierBookApptByEmail is true	

        public string LaneCarrierBookApptviaTokenFailEmail { get; set; } //Contact Email if Token Fails

        public string LaneCarrierBookApptviaTokenFailPhone { get; set; } //Contact Phone if Token Fails
        public string ReferenceLaneNumber { get; set; }

        //Start Modified by RHR for v-8.4.0.003 on 7/19/2021 Colortech Lead Time Enhancement


        private int? _LaneTransLeadTimeLocationOption;
        public int? LaneTransLeadTimeLocationOption
        {
            get { 
                if (!_LaneTransLeadTimeLocationOption.HasValue)
                {
                    _LaneTransLeadTimeLocationOption = 0;
                }
                return _LaneTransLeadTimeLocationOption; 
            } //uses extension string method Left
            set { _LaneTransLeadTimeLocationOption = value; }
        }

        private bool? _LaneTransLeadTimeUseMasterLane;
        public bool? LaneTransLeadTimeUseMasterLane
        {
            get
            {
                if (!_LaneTransLeadTimeUseMasterLane.HasValue)
                {
                    _LaneTransLeadTimeUseMasterLane = false;
                }
                return _LaneTransLeadTimeUseMasterLane;
            } //uses extension string method Left
            set { _LaneTransLeadTimeUseMasterLane = value; }
        }

        private int? _LaneTransLeadTimeCalcType;
        public int? LaneTransLeadTimeCalcType
        {
            get
            {
                if (!_LaneTransLeadTimeCalcType.HasValue)
                {
                    _LaneTransLeadTimeCalcType = 0;
                }
                return _LaneTransLeadTimeCalcType;
            } 
            set { _LaneTransLeadTimeCalcType = value; }
        }

        //End Modified by RHR for v-8.4.0.003 on 7/19/2021 Colortech Lead Time Enhancement

        public bool LaneRestrictedAtCompLevel { get; set; }
        //End Modified by RHR for v-8.4 on 4/22/2021

        //Begin Modified by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone settings  and new date time settings
        // changed to string 
        public string LaneModDate { get; set; }
        public string _LaneOrigTimeZone = "Central Standard Time";
        public string LaneOrigTimeZone {
            get {
                if (string.IsNullOrWhiteSpace(_LaneOrigTimeZone))
                {
                    _LaneOrigTimeZone = "Central Standard Time";
                }
                return _LaneOrigTimeZone.Left(100); 
            }
            set { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "Central Standard Time";
                }
                _LaneOrigTimeZone = value.Left(100); 
            }
        }

        public string _LaneDestTimeZone = "Central Standard Time";
        public string LaneDestTimeZone
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LaneDestTimeZone))
                {
                    _LaneDestTimeZone = "Central Standard Time";
                }
                return _LaneDestTimeZone.Left(100);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "Central Standard Time";
                }
                _LaneDestTimeZone = value.Left(100);
            }
        }

        //End Modified by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone settings  and new date time settings

        private string _LaneModUser;
        public string LaneModUser
        {
            get { return _LaneModUser.Left(100); } //uses extension string method Left
            set { _LaneModUser = value.Left(100); }
        }

        private byte[] _LaneUpdated;

        /// <summary>
        /// LaneUpdated should be bound to UI, _LaneUpdated is only bound on the controller
        /// </summary>
        public string LaneUpdated
        {
            get
            {
                if (this._LaneUpdated != null) { return Convert.ToBase64String(this._LaneUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._LaneUpdated = null; }
                else { this._LaneUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _LaneUpdated = val; }
        public byte[] getUpdated() { return _LaneUpdated; }

        public static Models.vLELane365 selectModelData(LTS.vLELane365 d)
        {
            Models.vLELane365 modelRecord = new Models.vLELane365();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneRecHourStart", "LaneRecHourStop", "LaneDestHourStart", "LaneDestHourStop", "LaneUpdated", "rowguid", "_Lane", "Lane", "_tblRouteType", "tblRouteType", "LaneModDate" };
                string sMsg = "";
                modelRecord = (Models.vLELane365)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { 
                    modelRecord.setUpdated(d.LaneUpdated.ToArray());
                    //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed logic from ToShortTimeString to ToString("HH:mm") to support new UI Requirements
                    modelRecord.LaneRecHourStart = (d.LaneRecHourStart ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm"); 
                    modelRecord.LaneRecHourStop = (d.LaneRecHourStop ?? DateTime.Parse("01/01/2021 23:59")).ToString("HH:mm");
                    modelRecord.LaneDestHourStart = (d.LaneDestHourStart ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.LaneDestHourStop = (d.LaneDestHourStop ?? DateTime.Parse("01/01/2021 23:59")).ToString("HH:mm");
                    modelRecord.LaneModDate = Utilities.convertDateToDateTimeString(d.LaneModDate);

                }
            }
            return modelRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 03/07/2023
        ///     added  Utilities.convertStringToNullDateTime to allow null dates as empty strings
        /// </remarks>
        public static LTS.vLELane365 selectLTSData(Models.vLELane365 d)
        {
            LTS.vLELane365 ltsRecord = new LTS.vLELane365();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneRecHourStart", "LaneRecHourStop", "LaneDestHourStart", "LaneDestHourStop", "LaneUpdated", "rowguid", "_Lane", "Lane", "_tblRouteType", "tblRouteType", "LaneModDate" };
                string sMsg = "";
                ltsRecord = (LTS.vLELane365)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LaneUpdated = bupdated == null ? new byte[0] : bupdated;
                    //Modified by RHR for v-8.5.4.003 on 10/28/2023 added new convertStringTimeToDate parser now support 24 hour time format
                    // includes logic for a default value if the user enters bad data in the text box
                    DateTime dtDefault = DateTime.Today;
                    ltsRecord.LaneRecHourStart = Utilities.convertStringTimeToDate(d.LaneRecHourStart, dtDefault);
                    ltsRecord.LaneRecHourStop = Utilities.convertStringTimeToDate(d.LaneRecHourStop, dtDefault.AddHours(11).AddMinutes(59));
                    ltsRecord.LaneDestHourStart = Utilities.convertStringTimeToDate(d.LaneDestHourStart, dtDefault);
                    ltsRecord.LaneDestHourStop = Utilities.convertStringTimeToDate(d.LaneDestHourStop, dtDefault.AddHours(11).AddMinutes(59));
                    ltsRecord.LaneModDate = Utilities.convertStringToDateTime(d.LaneModDate);
                    ltsRecord.ReferenceLaneNumber = d.LaneNumber;
                }
            }
            return ltsRecord;
        }



    }
}