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
using NGL.UTC.Library;
using DynamicsTMS365.Controllers;
using System.Configuration;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// Web model of Load Board Data object
    /// </summary>
    /// //<remarks>
    /// Modified by RHR for v-8.4 on 06/22/2021 added model for vBookLoadBoard to support time only fields
    ///   BookDestStartHrs,BookDestStopHrs,BookOriginStartHrs,BookOriginStopHrs
    /// Modified By RHR for v-8.5.0.001 on 11/29/2021 Added  BookOrderSequence 
    /// Modified by RHR for v-8.5.3.007 on 2023-02-15 added logic to use strings for dates
    ///     used to fix issues with date conversion in the JSON data   
    /// Modified by RHR for v-8.5.4.006 on 05/13/2024 added new fields for BookNotesVisable4 & 5
    /// Modified by RHR for v-8.5.4.006 on 05/13/2024 added new fields for Time Zones

    /// </remarks>
    public class vBookLoadBoard
    {
            public bool? DAT { get; set; }
            public bool? NEXTStop { get; set; }
            public string TransType { get; set; }
            public string BookItemOrderNumbers { get; set; }
            public double? BookMilesFrom { get; set; }
            public bool BookRouteConsFlag { get; set; }
            public int? CarrierNumber { get; set; }
            public string CarrierName { get; set; }
            public int BookCarrierControl { get; set; }
            public int? CompNumber { get; set; }
            public string CompName { get; set; }
            public int UserSecurityControl { get; set; }
            public int BookCustCompControl { get; set; }
            public bool? LaneOriginAddressUse { get; set; }
            public string LaneNumber { get; set; }
            public int BookODControl { get; set; }
            public string BookNotesBookUser4 { get; set; }
            public string BookNotesBookUser3 { get; set; }
            public string BookNotesBookUser2 { get; set; }
            public string BookNotesBookUser1 { get; set; }
            //Modified by RHR for v-8.5.4.006 on 05/13/2024 added new fields for BookNotesVisable4 & 5
            public string BookNotesVisable5 { get; set; }
            public string BookNotesVisable4 { get; set; }
            public string BookNotesVisable3 { get; set; }
            public string BookNotesVisable2 { get; set; }
            public string BookNotesVisable1 { get; set; }
            public string BookLoadCom { get; set; }
            public string BookLoadPONumber { get; set; }
            public int BookLoadControl { get; set; }
            public decimal? BookRevTotalCost { get; set; }
            public decimal? BookRevBilledBFC { get; set; }
            public string BookCarrBLNumber { get; set; }
            public string BookRouteFinalCode { get; set; }
            public int BookAMSDeliveryApptControl { get; set; }
            public string txtConfirmed { get; set; }
            public int? CreditAvailable { get; set; }
            public string BookCarrActTime { get; set; }
            public string BookDestStopHrs { get; set; }
            public string BookDestStartHrs { get; set; }
            public bool? BookDestApptReq { get; set; }
            public string BookDestFax { get; set; }
            public string BookDestAddress3 { get; set; }
            public int? BookDestCompControl { get; set; }
            public string BookOriginStopHrs { get; set; }
            public string BookOriginStartHrs { get; set; }
            public bool? BookExportDocCreated { get; set; }
            public bool? BookOriginApptReq { get; set; }
            public string BookOrigFax { get; set; }
            public string BookOrigAddress3 { get; set; }
            public int? BookOrigCompControl { get; set; }
            public int BookRouteTypeCode { get; set; }
            public int BookDefaultRouteSequence { get; set; }
            public int BookRouteGuideControl { get; set; }
            public int? BookTotalPX { get; set; }
            public bool BookBOLCode { get; set; }
            public bool BookDoNotInvoice { get; set; }
            public DateTime? BookFinARInvoiceDate { get; set; }
            public bool? BookHotLoad { get; set; }
            public int BookAPAdjReasonControl { get; set; }
            public string BookCarrierEquipmentCodes { get; set; }
            public string BookTypeCode { get; set; }
            public int BookCommCompControl { get; set; }
            public string BookFinAPBillNumber { get; set; }
            public int BookOriginalLaneControl { get; set; }
            public bool? OnCreditHold { get; set; }
            public string lblConfirmed { get; set; }
            public int BookAMSPickupApptControl { get; set; }
            public string BookModUser { get; set; }
            public string BookModDate { get; set; }
            public string BookDestCountry { get; set; }
            public string BookDestState { get; set; }
            public string BookDestCity { get; set; }
            public string BookDestAddress2 { get; set; }
            public string BookDestAddress1 { get; set; }
            public string BookDestName { get; set; }
            public string DestCSZ { get; set; }
            public string DestAddressShort { get; set; }
            public string DestAddressLong { get; set; }
            public string BookOrigEmergencyContactName { get; set; }
            public string BookOrigEmergencyContactPhone { get; set; }
            public string BookOrigPhone { get; set; }
            public string BookOrigContactEmail { get; set; }
            public string BookOrigContactName { get; set; }
            public string BookOrigZip { get; set; }
            public string BookOrigCountry { get; set; }
            public string BookOrigState { get; set; }
            public string BookOrigCity { get; set; }
            public string BookOrigAddress2 { get; set; }
            public string BookOrigAddress1 { get; set; }
            public string BookOrigName { get; set; }
            public string OrigCSZ { get; set; }
            public string OrigAddressShort { get; set; }
            public string OrigAddressLong { get; set; }
            public string BookConsPrefix { get; set; }
            public string BookSHID { get; set; }
            public string BookCarrOrderNumber { get; set; }
            public string BookProNumber { get; set; }
            public int BookControl { get; set; }
            public string BookDestZip { get; set; }
            public string BookDestContactName { get; set; }
            public string BookDestContactEmail { get; set; }
            public string BookDestPhone { get; set; }
            public int BookPickupStopNumber { get; set; }
            public string BookCarrierContactPhone { get; set; }
            public string BookCarrierContact { get; set; }
            public string BookShipCarrierName { get; set; }
            public string BookShipCarrierNumber { get; set; }
            public int? BookShipCarrierProControl { get; set; }
            public string BookShipCarrierProNumberRaw { get; set; }
            public string BookShipCarrierProNumber { get; set; }
            public short? BookStopNo { get; set; }
            public int? BookCarrierContControl { get; set; }
            public string BookPayCode { get; set; }
            public string BookTransType { get; set; }
            public string BookTranCode { get; set; }
            public string BookWarehouseNumber { get; set; }
            public string BookRouteFinalDate { get; set; }
            public string BookWhseAuthorizationNo { get; set; }
            public string BookCarrTripNo { get; set; }
            public string BookExpDelDateTime { get; set; }
            public string BookMustLeaveByDateTime { get; set; }
            public string BookDateRequested { get; set; }
            public string BookDateOrdered { get; set; }
            public string BookCarrActDate { get; set; }
            public string BookDateRequired { get; set; }
            public string BookDateLoad { get; set; }
            public int? BookTotalCube { get; set; }
            public double? BookTotalPL { get; set; }
            public double? BookTotalWgt { get; set; }
            public int? BookTotalCases { get; set; }
            public string BookDestEmergencyContactName { get; set; }
            public string BookDestEmergencyContactPhone { get; set; }
            public string BookCarrRouteNo { get; set; }
            public int? BookHoldLoad { get; set; }
            public int BookModeTypeControl { get; set; } //Added By RHR on 08/31/21 for v-8.4.0.003
            public string ModeTypeName { get; set; } //Added By RHR on 08/31/21 for v-8.4.0.003
            public int BookOrderSequence { get; set; } //Added By RHR for v-8.5.0.001 on 11/29/2021
            
            //Modified by RHR for v-8.5.4.006 on 05/13/2024 added new fields for Time Zones
            public string LaneOrigTimeZone { get; set; }
            public string LaneDestTimeZone { get; set; }
            public string CompTimeZone { get; set; }
        /// <summary>
        /// Transform LTS data to 365 Model data
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 2023-02-15 added logic to send dates as strings to UI
        ///     this is changed to support multiple time zones and the issues where the JASON data is 
        ///     converted to Universal Time for Dates
        /// </remarks>
        public static Models.vBookLoadBoard selectModelData(LTS.vBookLoadBoard d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.vBookLoadBoard modelRecord = new Models.vBookLoadBoard();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"BookCarrActTime", "BookRouteFinalDate" , "BookExpDelDateTime" , "BookMustLeaveByDateTime" , "BookDateRequested" , "BookDateOrdered" , "BookCarrActDate" , "BookDateRequired" , "BookDateLoad" , "BookDestStopHrs", "BookDestStartHrs", "BookOriginStopHrs", "BookOriginStartHrs", "vBookLoadBoard", "BookModDate" };
                string sMsg = "";
                modelRecord = (Models.vBookLoadBoard)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null)
                {
                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic from ToShortTimeString to ToString("HH:mm") to support new UI Requirements
                    modelRecord.BookCarrActTime = (d.BookCarrActTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed logic from ToShortTimeString to ToString("HH:mm") to support new UI Requirements
                    modelRecord.BookDestStartHrs =   (d.BookDestStartHrs ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm"); 
                    modelRecord.BookDestStopHrs = (d.BookDestStopHrs ?? DateTime.Parse("01/01/2021 23:59")).ToString("HH:mm"); 
                    modelRecord.BookOriginStartHrs = (d.BookOriginStartHrs ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm"); 
                    modelRecord.BookOriginStopHrs = (d.BookOriginStopHrs ?? DateTime.Parse("01/01/2021 23:59")).ToString("HH:mm");
                    // Modified by RHR for v-8.5.3.007 on 2023-02-15
                    //modelRecord.BookModDate =  (d.BookModDate ?? DateTime.Now).ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                    //var test = Utilities.convertDateToDateTimeString(d.BookModDate); old functionality
                    modelRecord.BookModDate = clsApplication.convertDateToDateTimeString(d.BookModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                    modelRecord.BookRouteFinalDate = Utilities.convertDateToShortDateString(d.BookRouteFinalDate);
                    modelRecord.BookExpDelDateTime = Utilities.convertDateToShortDateString(d.BookExpDelDateTime);
                    modelRecord.BookMustLeaveByDateTime = Utilities.convertDateToShortDateString(d.BookMustLeaveByDateTime);
                    modelRecord.BookDateRequested = Utilities.convertDateToShortDateString(d.BookDateRequested);
                    modelRecord.BookDateOrdered = Utilities.convertDateToShortDateString(d.BookDateOrdered);
                    modelRecord.BookCarrActDate = Utilities.convertDateToShortDateString(d.BookCarrActDate);
                    modelRecord.BookDateRequired = Utilities.convertDateToShortDateString(d.BookDateRequired);
                    modelRecord.BookDateLoad  = Utilities.convertDateToShortDateString(d.BookDateLoad);
                }
            }
            // Note : When Lane origin Address Use is false The Book Date Load Timezone is the origin time zone.
            // When Lane origin Address Use is trure The Book Date Load Timezone is the destination time zone

            // Same rule applies to BookRouteFinalDate , BookMustLeaveByDateTime, BookDateOrdered, BookOriginStartHrs, BookOriginStopHrs

            // New Rule : When Lane origin Address Use is false then following fields use the destination time zone
            return modelRecord;
        }

        /// <summary>
        /// Translate 365 Model data to LTS View data
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        ///  Modified by RHR for v-8.5.3.007 on 2023-02-15 added logic to convert strings to dates
        ///     used to fix issues with date conversion in the JSON data
        /// </remarks>
        public static LTS.vBookLoadBoard selectLTSData(Models.vBookLoadBoard d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            LTS.vBookLoadBoard ltsRecord = new LTS.vBookLoadBoard();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookCarrActTime", "BookRouteFinalDate", "BookExpDelDateTime", "BookMustLeaveByDateTime", "BookDateRequested", "BookDateOrdered", "BookCarrActDate", "BookDateRequired", "BookDateLoad", "BookDestStopHrs", "BookDestStartHrs", "BookOriginStopHrs", "BookOriginStartHrs", "vBookLoadBoard", "BookModDate" };
                string sMsg = "";
                ltsRecord = (LTS.vBookLoadBoard)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {

                    //Modified by RHR for v-8.5.4.003 on 10/28/2023 added new convertStringTimeToDate parser now support 24 hour time format
                    // includes logic for a default value if the user enters bad data in the text box
                    DateTime dtDefault = DateTime.Today; 
                    ltsRecord.BookDestStartHrs = Utilities.convertStringTimeToDate( d.BookDestStartHrs, dtDefault);
                    ltsRecord.BookDestStopHrs = Utilities.convertStringTimeToDate(d.BookDestStopHrs, dtDefault.AddHours(11).AddMinutes(59));
                    ltsRecord.BookOriginStartHrs = Utilities.convertStringTimeToDate(d.BookOriginStartHrs, dtDefault);
                    ltsRecord.BookOriginStopHrs = Utilities.convertStringTimeToDate(d.BookOriginStopHrs, dtDefault.AddHours(11).AddMinutes(59));
                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic from ToShortTimeString to ToString("HH:mm") to support new UI Requirements
                    ltsRecord.BookCarrActTime = Utilities.convertStringTimeToDate(d.BookCarrActTime, dtDefault);
                    // Modified by RHR for v-8.5.3.007 on 2023-02-15
                    ltsRecord.BookRouteFinalDate = Utilities.convertStringToNullDateTime(d.BookRouteFinalDate);
                    ltsRecord.BookExpDelDateTime = Utilities.convertStringToNullDateTime(d.BookExpDelDateTime);
                    ltsRecord.BookMustLeaveByDateTime = Utilities.convertStringToNullDateTime(d.BookMustLeaveByDateTime);
                    ltsRecord.BookDateRequested = Utilities.convertStringToNullDateTime(d.BookDateRequested);
                    ltsRecord.BookDateOrdered = Utilities.convertStringToNullDateTime(d.BookDateOrdered);
                    ltsRecord.BookCarrActDate = Utilities.convertStringToNullDateTime(d.BookCarrActDate);
                    ltsRecord.BookDateRequired = Utilities.convertStringToNullDateTime(d.BookDateRequired);
                    ltsRecord.BookDateLoad = Utilities.convertStringToNullDateTime(d.BookDateLoad);
                    // Note:: Function convertStringToDateTime is opposite of convertDateToDateTimeString so Input Time zone of this function is output time zone for convertStringToDateTime.
                    //var test = Utilities.convertStringToDateTime(d.BookModDate); //Previous Functionality
                    ltsRecord.BookModDate = clsApplication.convertStringToDateTime(d.BookModDate,userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update

                }
            }

            return ltsRecord;
        }


    }
}