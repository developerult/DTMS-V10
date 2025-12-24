using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;
using DynamicsTMS365.Controllers;
using NGL.UTC.Library;
using System.Configuration;

namespace DynamicsTMS365.Models
{
    public class BookCarrier
    {
        public int BookControl { get; set; }
        public string BookCarrFBNumber { get; set; }
        public string BookCarrOrderNumber { get; set; }
        public string BookCarrBLNumber { get; set; }
        public string BookCarrBookContact { get; set; }        
        //public DateTime? BookCarrActLoadComplete_Date { get; set; }
        public string BookCarrDockPUAssigment { get; set; }        
        public string BookCarrDockDelAssignment { get; set; }
        public int BookCarrVarDay { get; set; }
        public int BookCarrVarHrs { get; set; }
        public string BookCarrTrailerNo { get; set; }
        public string BookCarrSealNo { get; set; }
        public string BookCarrDriverNo { get; set; }
        public string BookCarrDriverName { get; set; }
        public string BookCarrRouteNo { get; set; }
        public string BookCarrTripNo { get; set; }
        public string BookWhseAuthorizationNo { get; set; }        
        public int BookAMSPickupApptControl { get; set; }
        public int BookAMSDeliveryApptControl { get; set; }
        private string _BookModUser;
        public string BookModUser
        {
            get { return _BookModUser.Left(100); } //uses extension string method Left
            set { _BookModUser = value.Left(100); }
        }

        //Modified by RHR for v-8.5.4.004 on 12/07/2023 date properties converted to strings
        public string BookCarrBookDate { get; set; }
        public string BookCarrScheduleDate { get; set; }
        public string BookCarrActualDate { get; set; }
        public string BookCarrActLoadCompleteDate { get; set; }
        public string BookCarrPODate { get; set; }
        public string BookCarrApptDate { get; set; }
        public string BookCarrActDate { get; set; }
        public string BookCarrActUnloadCompDate { get; set; }
        public string BookCarrStartLoadingDate { get; set; }
        public string BookCarrFinishLoadingDate { get; set; }
        public string BookCarrStartUnloadingDate { get; set; }
        public string BookCarrFinishUnloadingDate { get; set; }
        public string BookModDate { get; set; }





        // time properties to convert
        public string BookCarrBookTime { get; set; }
        public string BookCarrScheduleTime { get; set; }
        public string BookCarrActualTime { get; set; }
        public string BookCarrActLoadCompleteTime { get; set; }
        public string BookCarrPOTime { get; set; }
        public string BookCarrApptTime { get; set; }
        public string BookCarrActTime { get; set; }
        public string BookCarrActUnloadCompTime { get; set; }
        public string BookCarrStartLoadingTime { get; set; }
        public string BookCarrFinishLoadingTime { get; set; }
        public string BookCarrStartUnloadingTime { get; set; }
        public string BookCarrFinishUnloadingTime { get; set; }

        public static string FormatDateToTime(DateTime? val) { if (val.HasValue) { return val.Value.ToShortTimeString(); } else { return ""; } }
        public static DateTime? FormatTimeToDate(DateTime? dtParent, string val)
        {
            DateTime dtcurrent = DateTime.Now;
            DateTime? dtRet = null;
            if (string.IsNullOrWhiteSpace(val)) { return dtRet; }
            if (dtParent.HasValue) { dtcurrent = dtParent.Value; }
            DateTime dtKendoTime;
            string sTime = "";
            if (DateTime.TryParse(val, out dtKendoTime))
            {
                sTime = dtKendoTime.ToShortTimeString();
            }
            string dtString = dtcurrent.ToShortDateString() + " " + sTime;
            DateTime newdateTime;
            if (DateTime.TryParse(dtString, out newdateTime))
            {
                dtRet = newdateTime;
            }
            return dtRet;
        }

        public static Models.BookCarrier selectModelData(DTO.BookCarrier d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();

            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.BookCarrier modelRecord = new Models.BookCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {
                    "BookCarrBookDate",
                    "BookCarrScheduleDate",
                    "BookCarrActualDate",
                    "BookCarrActLoadCompleteDate",
                    "BookCarrPODate",
                    "BookCarrApptDate",
                    "BookCarrActDate",
                    "BookCarrActUnloadCompDate",
                    "BookCarrStartLoadingDate",
                    "BookCarrFinishLoadingDate",
                    "BookCarrStartUnloadingDate",
                    "BookCarrFinishUnloadingDate",
                    "BookModDate",
                    "BookCarrActLoadComplete_Date", 
                    "BookCarrBookTime", 
                    "BookCarrScheduleTime",
                    "BookCarrActualTime", 
                    "BookCarrActLoadCompleteTime", 
                    "BookCarrPOTime", 
                    "BookCarrApptTime", 
                    "BookCarrActTime", 
                    "BookCarrActUnloadCompTime", 
                    "BookCarrStartLoadingTime", 
                    "BookCarrFinishLoadingTime", 
                    "BookCarrStartUnloadingTime", 
                    "BookCarrFinishUnloadingTime" };
                string sMsg = "";
                modelRecord = (Models.BookCarrier)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic to use strings for dates due to UTC conversion issues
                    modelRecord.BookCarrBookDate = Utilities.convertDateToDateTimeString(d.BookCarrBookDate);
                    modelRecord.BookCarrScheduleDate = Utilities.convertDateToDateTimeString(d.BookCarrScheduleDate);
                    modelRecord.BookCarrActualDate = Utilities.convertDateToDateTimeString(d.BookCarrActualDate);
                    modelRecord.BookCarrPODate = Utilities.convertDateToDateTimeString(d.BookCarrPODate);
                    modelRecord.BookCarrApptDate = Utilities.convertDateToDateTimeString(d.BookCarrApptDate);
                    modelRecord.BookCarrActDate = Utilities.convertDateToDateTimeString(d.BookCarrActDate);
                    modelRecord.BookCarrActUnloadCompDate = Utilities.convertDateToDateTimeString(d.BookCarrStartLoadingDate);
                    modelRecord.BookCarrStartLoadingDate = Utilities.convertDateToDateTimeString(d.BookCarrStartLoadingDate);
                    modelRecord.BookCarrFinishLoadingDate = Utilities.convertDateToDateTimeString(d.BookCarrFinishLoadingDate);
                    modelRecord.BookCarrStartUnloadingDate = Utilities.convertDateToDateTimeString(d.BookCarrStartUnloadingDate);
                    modelRecord.BookCarrFinishUnloadingDate = Utilities.convertDateToDateTimeString(d.BookCarrFinishUnloadingDate);
                    //modelRecord.BookModDate = Utilities.convertDateToDateTimeString(d.BookModDate); Old Functionality
                    modelRecord.BookModDate = clsApplication.convertDateToDateTimeString(d.BookModDate,userData.UserCultureInfo,serverTimeZone,userData.UserTimeZone); //UTC Mod Date Update
                    modelRecord.BookCarrActLoadCompleteDate = Utilities.convertDateToDateTimeString(d.BookCarrActLoadComplete_Date);
                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic from ToShortTimeString to ToString("HH:mm") to support new UI Requirements
                    //convert time strings for text box
                    modelRecord.BookCarrBookTime = (d.BookCarrBookTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrScheduleTime = (d.BookCarrScheduleTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrActualTime = (d.BookCarrActualTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrActLoadCompleteTime = (d.BookCarrActLoadCompleteTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrPOTime = (d.BookCarrPOTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrApptTime = (d.BookCarrApptTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrActTime = (d.BookCarrActTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrActUnloadCompTime = (d.BookCarrActUnloadCompTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrStartLoadingTime = (d.BookCarrStartLoadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrFinishLoadingTime = (d.BookCarrFinishLoadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrStartUnloadingTime = (d.BookCarrStartUnloadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookCarrFinishUnloadingTime = (d.BookCarrFinishUnloadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                }
            }
            return modelRecord;
        }

        public static DTO.BookCarrier selectDTOData(Models.BookCarrier d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            DTO.BookCarrier dtoRecord = new DTO.BookCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {
                    "BookCarrBookDate",
                    "BookCarrScheduleDate",
                    "BookCarrActualDate",
                    "BookCarrActLoadCompleteDate",
                    "BookCarrPODate",
                    "BookCarrApptDate",
                    "BookCarrActDate",
                    "BookCarrActUnloadCompDate",
                    "BookCarrStartLoadingDate",
                    "BookCarrFinishLoadingDate",
                    "BookCarrStartUnloadingDate",
                    "BookCarrFinishUnloadingDate",
                    "BookModDate",
                    "BookCarrActLoadComplete_Date",
                    "BookCarrBookTime",
                    "BookCarrScheduleTime",
                    "BookCarrActualTime",
                    "BookCarrActLoadCompleteTime",
                    "BookCarrPOTime",
                    "BookCarrApptTime",
                    "BookCarrActTime",
                    "BookCarrActUnloadCompTime",
                    "BookCarrStartLoadingTime",
                    "BookCarrFinishLoadingTime",
                    "BookCarrStartUnloadingTime",
                    "BookCarrFinishUnloadingTime"
                };
                string sMsg = "";
                dtoRecord = (DTO.BookCarrier)DTran.CopyMatchingFields(dtoRecord, d, skipObjs, ref sMsg);
                if (dtoRecord != null)
                {
                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic to use strings for dates due to UTC conversion issues
                   dtoRecord.BookCarrBookDate = Utilities.convertStringToNullDateTime(d.BookCarrBookDate);
                   dtoRecord.BookCarrScheduleDate = Utilities.convertStringToNullDateTime(d.BookCarrScheduleDate);
                   dtoRecord.BookCarrActualDate = Utilities.convertStringToNullDateTime(d.BookCarrActualDate);
                   dtoRecord.BookCarrPODate = Utilities.convertStringToNullDateTime(d.BookCarrPODate);
                   dtoRecord.BookCarrApptDate = Utilities.convertStringToNullDateTime(d.BookCarrApptDate);
                   dtoRecord.BookCarrActDate = Utilities.convertStringToNullDateTime(d.BookCarrActDate);
                   dtoRecord.BookCarrActUnloadCompDate = Utilities.convertStringToNullDateTime(d.BookCarrStartLoadingDate);
                   dtoRecord.BookCarrStartLoadingDate = Utilities.convertStringToNullDateTime(d.BookCarrStartLoadingDate);
                   dtoRecord.BookCarrFinishLoadingDate = Utilities.convertStringToNullDateTime(d.BookCarrFinishLoadingDate);
                   dtoRecord.BookCarrStartUnloadingDate = Utilities.convertStringToNullDateTime(d.BookCarrStartUnloadingDate);
                   dtoRecord.BookCarrFinishUnloadingDate = Utilities.convertStringToNullDateTime(d.BookCarrFinishUnloadingDate);
                    //dtoRecord.BookModDate = Utilities.convertStringToNullDateTime(d.BookModDate);
                    dtoRecord.BookModDate = clsApplication.convertStringToNullDateTime(d.BookModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                    dtoRecord.BookCarrActLoadComplete_Date = Utilities.convertStringToNullDateTime(d.BookCarrActLoadCompleteDate);
                    //Modified by RHR for v-8.5.4.003 on 10/28/2023 added new convertStringTimeToDate parser now support 24 hour time format
                    // includes logic for a default value if the user enters bad data in the text box
                    DateTime dtDefault = DateTime.Today;
                    dtoRecord.BookCarrBookTime = Utilities.convertStringTimeToDate(d.BookCarrBookTime,dtoRecord.BookCarrBookDate);
                    dtoRecord.BookCarrScheduleTime = Utilities.convertStringTimeToDate(d.BookCarrScheduleTime,dtoRecord.BookCarrScheduleDate);
                    dtoRecord.BookCarrActualTime = Utilities.convertStringTimeToDate(d.BookCarrActualTime, dtoRecord.BookCarrActualDate);
                    dtoRecord.BookCarrActLoadCompleteTime = Utilities.convertStringTimeToDate(d.BookCarrActLoadCompleteTime, dtoRecord.BookCarrActLoadComplete_Date);
                    dtoRecord.BookCarrPOTime = Utilities.convertStringTimeToDate(d.BookCarrPOTime, dtoRecord.BookCarrPODate);
                    dtoRecord.BookCarrApptTime = Utilities.convertStringTimeToDate(d.BookCarrApptTime,dtoRecord.BookCarrApptDate);
                    dtoRecord.BookCarrActTime = Utilities.convertStringTimeToDate(d.BookCarrActTime, dtoRecord.BookCarrActDate);
                    dtoRecord.BookCarrActUnloadCompTime = Utilities.convertStringTimeToDate(d.BookCarrActUnloadCompTime, dtoRecord.BookCarrActUnloadCompDate);
                    dtoRecord.BookCarrStartLoadingTime = Utilities.convertStringTimeToDate(d.BookCarrStartLoadingTime, dtoRecord.BookCarrStartLoadingDate);
                    dtoRecord.BookCarrFinishLoadingTime = Utilities.convertStringTimeToDate(d.BookCarrFinishLoadingTime, dtoRecord.BookCarrFinishLoadingDate);
                    dtoRecord.BookCarrStartUnloadingTime = Utilities.convertStringTimeToDate(d.BookCarrStartUnloadingTime, dtoRecord.BookCarrStartUnloadingDate);
                    dtoRecord.BookCarrFinishUnloadingTime = Utilities.convertStringTimeToDate(d.BookCarrFinishUnloadingTime, dtoRecord.BookCarrFinishUnloadingDate);
                }
            }
            return dtoRecord;
        }
    }
}