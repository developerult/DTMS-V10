using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;
using DynamicsTMS365.Controllers;
using System.Configuration;
using NGL.UTC.Library;

namespace DynamicsTMS365.Models
{
    public class vBookFinancial
    {
        public int BookControl { get; set; }

        public int vBookFinancialControl { get; set; }

        public decimal? BookFinServiceFee { get; set; }

        public decimal? BookFinARBookFrt { get; set; }

        public decimal? BookFinARInvoiceAmt { get; set; }

        public decimal? BookFinARPayAmt { get; set; }

        public string BookFinARCheck { get; set; }

        public string BookFinARGLNumber { get; set; }

        public decimal? BookFinARBalance { get; set; }

        public int? BookFinARCurType { get; set; }

        public string BookFinAPBillNumber { get; set; }

        public int? BookFinAPActWgt { get; set; }

        public decimal? BookFinAPStdCost { get; set; }

        public decimal? BookFinAPActCost { get; set; }

        public decimal? BookFinAPPayAmt { get; set; }

        public string BookFinAPCheck { get; set; }

        public string BookFinAPGLNumber { get; set; }

        public int? BookFinAPCurType { get; set; }

        public decimal? BookFinCommStd { get; set; }

        public decimal? BookFinCommAct { get; set; }

        public decimal? BookFinCommPayAmt { get; set; }

        public string BookFinCommtCheck { get; set; }

        public decimal? BookFinCommCreditAmt { get; set; }

        public int? BookFinCommLoadCount { get; set; }

        public string BookFinCommGLNumber { get; set; }

        public string BookFinCheckClearedNumber { get; set; }

        public decimal? BookFinCheckClearedAmt { get; set; }

        public string BookFinCheckClearedDesc { get; set; }

        public string BookFinCheckClearedAcct { get; set; }

        public string BookModUser { get; set; }

        public bool BookDoNotInvoice { get; set; }

        public string BookRouteFinalCode { get; set; }

        public string BookConsPrefix { get; set; }

        public int? BookFinAPExportRetry { get; set; }

        public bool BookFinAPExportFlag { get; set; }

        public string BookSHID { get; set; }

        public string ARCustomerText { get; set; }

        public string APCarrierText { get; set; }

        public string APCommissionsText { get; set; }

        //Date to string properties

        public string BookFinARInvoiceDate { get; set; }

        public string BookFinARPayDate { get; set; }

        public string BookFinAPBillNoDate { get; set; }

        public string BookFinAPBillInvDate { get; set; }

        public string BookFinAPPayDate { get; set; }

        public string BookFinAPLastViewed { get; set; }

        public string BookFinCommPayDate { get; set; }

        public string BookFinCommCreditPayDate { get; set; }

        public string BookFinCheckClearedDate { get; set; }

        public string BookModDate { get; set; }

        public string BookDateLoad { get; set; }

        public string BookFinAPExportDate { get; set; }

        public string BookModDateString { get; set; }


        //Time to string properties

        public string BookFinAPExportTimeString { get; set; }

        public string BookDateLoadTimeString { get; set; }

        public string BookFinCheckClearedTimeString { get; set; }

        public string BookFinCommCreditPayTimeString { get; set; }

        public string BookFinCommPayTimeString { get; set; }

        public string BookFinAPPayTimeString { get; set; }

        public string BookFinAPBillInvTimeString { get; set; }

        public string BookFinAPBillNoTimeString { get; set; }

        public string BookFinARPayTimeString { get; set; }

        public string BookFinARInvoiceTimeString { get; set; }

        public static Models.vBookFinancial selectModelData(LTS.vBookFinancial d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);
            Models.vBookFinancial modelRecord = new Models.vBookFinancial();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {
                    "BookFinARInvoiceDate",
                    "BookFinARPayDate",
                    "BookFinAPBillNoDate",
                    "BookFinAPBillInvDate",
                    "BookFinAPPayDate",
                    "BookFinAPLastViewed",
                    "BookFinCommPayDate",
                    "BookFinCommCreditPayDate",
                    "BookFinCheckClearedDate",
                    "BookModDate",
                    "BookDateLoad",
                    "BookFinAPExportDate",
                    "BookFinAPExportTimeString",
                    "BookDateLoadTimeString",
                    "BookFinCheckClearedTimeString",
                    "BookFinCommCreditPayTimeString",
                    "BookFinCommPayTimeString",
                    "BookFinAPPayTimeString",
                    "BookFinAPBillInvTimeString",
                    "BookFinAPBillNoTimeString",
                    "BookFinARPayTimeString",
                    "BookFinARInvoiceTimeString" };
                string sMsg = "";
                modelRecord = (Models.vBookFinancial)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null)
                {
                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic to use strings for dates due to UTC conversion issues
                    modelRecord.BookFinARInvoiceDate = Utilities.convertDateToDateTimeString(d.BookFinARInvoiceDate);
                    modelRecord.BookFinARPayDate = Utilities.convertDateToDateTimeString(d.BookFinARPayDate);
                    modelRecord.BookFinAPBillNoDate = Utilities.convertDateToDateTimeString(d.BookFinAPBillNoDate);
                    modelRecord.BookFinAPBillInvDate = Utilities.convertDateToDateTimeString(d.BookFinAPBillInvDate);
                    modelRecord.BookFinAPPayDate = Utilities.convertDateToDateTimeString(d.BookFinAPPayDate);
                    modelRecord.BookFinAPLastViewed = Utilities.convertDateToDateTimeString(d.BookFinAPLastViewed);
                    modelRecord.BookFinCommPayDate = Utilities.convertDateToDateTimeString(d.BookFinCommPayDate);
                    modelRecord.BookFinCommCreditPayDate = Utilities.convertDateToDateTimeString(d.BookFinCommCreditPayDate);
                    modelRecord.BookFinCheckClearedDate = Utilities.convertDateToDateTimeString(d.BookFinCheckClearedDate);
                    //modelRecord.BookModDate = Utilities.convertDateToDateTimeString(d.BookModDate);
                    modelRecord.BookModDate = clsApplication.convertDateToDateTimeString(d.BookModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                    modelRecord.BookDateLoad = Utilities.convertDateToDateTimeString(d.BookDateLoad);
                    modelRecord.BookFinAPExportDate = Utilities.convertDateToDateTimeString(d.BookFinAPExportDate);
                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic from ToShortTimeString to ToString("HH:mm") to support new UI Requirements
                    //convert time strings for text box
                    modelRecord.BookFinAPExportTimeString = (d.BookFinAPExportTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookDateLoadTimeString = (d.BookDateLoadTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookFinCheckClearedTimeString = (d.BookFinCheckClearedTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookFinCommCreditPayTimeString = (d.BookFinCommCreditPayTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookFinCommPayTimeString = (d.BookFinCommPayTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookFinAPPayTimeString = (d.BookFinAPPayTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookFinAPBillInvTimeString = (d.BookFinAPBillInvTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookFinAPBillNoTimeString = (d.BookFinAPBillNoTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookFinARPayTimeString = (d.BookFinARPayTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                    modelRecord.BookFinARInvoiceTimeString = (d.BookFinARInvoiceTimeString ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
                }
            }
            return modelRecord;
        }

        public static LTS.vBookFinancial selectLTSData(Models.vBookFinancial d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            LTS.vBookFinancial LTSRecord = new LTS.vBookFinancial();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {

                    "BookFinARInvoiceDate",
                    "BookFinARPayDate",
                    "BookFinAPBillNoDate",
                    "BookFinAPBillInvDate",
                    "BookFinAPPayDate",
                    "BookFinAPLastViewed",
                    "BookFinCommPayDate",
                    "BookFinCommCreditPayDate",
                    "BookFinCheckClearedDate",
                    "BookModDate",
                    "BookDateLoad",
                    "BookFinAPExportDate",
                    "BookFinAPExportTimeString",
                    "BookDateLoadTimeString",
                    "BookFinCheckClearedTimeString",
                    "BookFinCommCreditPayTimeString",
                    "BookFinCommPayTimeString",
                    "BookFinAPPayTimeString",
                    "BookFinAPBillInvTimeString",
                    "BookFinAPBillNoTimeString",
                    "BookFinARPayTimeString",
                    "BookFinARInvoiceTimeString"
                };
                string sMsg = "";
                LTSRecord = (LTS.vBookFinancial)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {

                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic to use strings for dates due to UTC conversion issues
                    LTSRecord.BookFinARInvoiceDate = Utilities.convertStringToNullDateTime(d.BookFinARInvoiceDate);
                    LTSRecord.BookFinARPayDate = Utilities.convertStringToNullDateTime(d.BookFinARPayDate);
                    LTSRecord.BookFinAPBillNoDate = Utilities.convertStringToNullDateTime(d.BookFinAPBillNoDate);
                    LTSRecord.BookFinAPBillInvDate = Utilities.convertStringToNullDateTime(d.BookFinAPBillInvDate);
                    LTSRecord.BookFinAPPayDate = Utilities.convertStringToNullDateTime(d.BookFinAPPayDate);
                    LTSRecord.BookFinAPLastViewed = Utilities.convertStringToNullDateTime(d.BookFinAPLastViewed);
                    LTSRecord.BookFinCommPayDate = Utilities.convertStringToNullDateTime(d.BookFinCommPayDate);
                    LTSRecord.BookFinCommCreditPayDate = Utilities.convertStringToNullDateTime(d.BookFinCommCreditPayDate);
                    LTSRecord.BookFinCheckClearedDate = Utilities.convertStringToNullDateTime(d.BookFinCheckClearedDate);
                    //LTSRecord.BookModDate = Utilities.convertStringToNullDateTime(d.BookModDate);
                    LTSRecord.BookModDate = clsApplication.convertStringToNullDateTime(d.BookModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                    LTSRecord.BookDateLoad = Utilities.convertStringToNullDateTime(d.BookDateLoad);
                    LTSRecord.BookFinAPExportDate = Utilities.convertStringToNullDateTime(d.BookFinAPExportDate);
                    //Modified by RHR for v-8.5.4.004 on 12/07/2023 changed logic from ToShortTimeString to ToString("HH:mm") to support new UI Requirements
                    //convert time strings back to  Dates
                    DateTime dtDefault = DateTime.Today;
                    LTSRecord.BookFinAPExportTimeString = Utilities.convertStringTimeToDate(d.BookFinAPExportTimeString , LTSRecord.BookFinAPExportDate);
                    LTSRecord.BookDateLoadTimeString = Utilities.convertStringTimeToDate(d.BookDateLoadTimeString , LTSRecord.BookDateLoad);
                    LTSRecord.BookFinCheckClearedTimeString = Utilities.convertStringTimeToDate(d.BookFinCheckClearedTimeString, LTSRecord.BookFinCheckClearedDate);
                    LTSRecord.BookFinCommCreditPayTimeString = Utilities.convertStringTimeToDate(d.BookFinCommCreditPayTimeString, LTSRecord.BookFinCommCreditPayDate);
                    LTSRecord.BookFinCommPayTimeString = Utilities.convertStringTimeToDate(d.BookFinCommPayTimeString, LTSRecord.BookFinCommPayDate);
                    LTSRecord.BookFinAPPayTimeString = Utilities.convertStringTimeToDate(d.BookFinAPPayTimeString, LTSRecord.BookFinAPPayDate);
                    LTSRecord.BookFinAPBillInvTimeString = Utilities.convertStringTimeToDate(d.BookFinAPBillInvTimeString, LTSRecord.BookFinAPBillInvDate);
                    LTSRecord.BookFinAPBillNoTimeString = Utilities.convertStringTimeToDate(d.BookFinAPBillNoTimeString, LTSRecord.BookFinAPBillNoDate);
                    LTSRecord.BookFinARPayTimeString = Utilities.convertStringTimeToDate(d.BookFinARPayTimeString, LTSRecord.BookFinARPayDate);
                    LTSRecord.BookFinARInvoiceTimeString = Utilities.convertStringTimeToDate(d.BookFinARInvoiceTimeString, LTSRecord.BookFinARInvoiceDate);

                }
            }
            return LTSRecord;
        }
    }
}