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
using DynamicsTMS365.Controllers;
using System.Configuration;
using NGL.UTC.Library;

namespace DynamicsTMS365.Models
{
    public class vBookMultiPick
    {


        public int? MultiPickCompanyNumber { get; set; }
        public string MultiPickCompanyName { get; set; }
        public bool? MultiPickLockBFCCost { get; set; }
        public bool? MultiPickLockAllCosts { get; set; }
        public double? MultiPickRouteMiles { get; set; }
        public decimal? MultiPickRouteNonTaxable { get; set; }
       
        public decimal? MultiPickRouteGrossRevenue { get; set; }
       
        public decimal? MultiPickRouteLoadSavings { get; set; }
       
        public decimal? MultiPickRouteTotalCost { get; set; }
       
        public decimal? MultiPickRouteOtherCost { get; set; }
       
        public decimal? MultiPickRouteCarrierCost { get; set; }
       
        public decimal? MultiPickRouteBFC { get; set; }
       
        public decimal? MultiPickNonTaxable { get; set; }
       
        public decimal? MultiPickGrossRevenue { get; set; }
       
        public decimal? MultiPickLoadSavings { get; set; }
       
        public decimal? MultiPickTotalCost { get; set; }
       
        public decimal? MultiPickOtherCost { get; set; }
       
        public decimal? MultiPickCarrierCost { get; set; }
       
        public decimal? MultiPickBilledBFC { get; set; }
       
        public bool? MultiPickUseImportFrtCost { get; set; }
       
        public string MultiPickLaneNumber { get; set; }
       
        public string MultiPickLaneName { get; set; }
       
        public string MultiPickCarrierName { get; set; }
       
        public int? MultiPickBookShipCarrierProControl { get; set; }
       
        public int MultiPickBookCarrTarEquipControl { get; set; }
       
        public string MultiPickBookCarrTarName { get; set; }
       
        public int MultiPickBookCarrTarControl { get; set; }
       
        public double? MultiPickBookRevLoadMiles { get; set; }
       
        public double? MultiPickBookRevLaneBenchMiles { get; set; }
       
        public bool MultiPickBookSpotRateUseFuelAddendum { get; set; }
       
        public decimal MultiPickBookSpotRateTotalUnallocatedLineHaul { get; set; }
       
        public decimal MultiPickBookSpotRateTotalUnallocatedBFC { get; set; }
       
        public bool? MultiPickIsPickUpFlag { get; set; }
       
        public int MultiPickBookSpotRateBFCAllocationFormula { get; set; }
       
        public bool MultiPickBookSpotRateAutoCalcBFC { get; set; }
       
        public int MultiPickBookSpotRateAllocationFormula { get; set; }
       
        public double MultiPickBookOutOfRouteMiles { get; set; }
        public string MultiPickBookMustLeaveByDateTime { get; set; }
        public string MultiPickBookExpDelDateTime { get; set; }
      
        public string MultiPickBookSHID { get; set; }
       
        public string MultiPickBookFinARInvoiceDate { get; set; }
       
        public int? MultiPickPickupStopNumber { get; set; }
       
        public string MultiPickCarrierNumber { get; set; }
       
        public bool MultiPickBookSpotRateUseCarrierFuelAddendum { get; set; }
       
        public bool MultiPickBookLockAllCosts { get; set; }
       
        public bool? MultiPickRouteConsFlag { get; set; }
       
        public bool? MultiPickRouteFinalFlag { get; set; }
       
        public string MultiPickFax { get; set; }
       
        public string MultiPickPhone { get; set; }
       
        public string MultiPickZip { get; set; }
       
        public string MultiPickCountry { get; set; }
       
        public string MultiPickState { get; set; }
       
        public string MultiPickCity { get; set; }
       
        public string MultiPickAddress3 { get; set; }
       
        public string MultiPickAddress2 { get; set; }
       
        public string MultiPickAddress1 { get; set; }
       
        public string MultiPickName { get; set; }
       
        public bool? MultiPickLocationisOrigin { get; set; }
       
        public int? MultiPickLocationControl { get; set; }
       
        public int? MultiPickCarrierControl { get; set; }
       
        public int? MultiPickODControl { get; set; }
       
        public int? MultiPickCustCompControl { get; set; }
       
        public string MultiPickConsPrefix { get; set; }
       
        public string MultiPickProNumber { get; set; }
       
        public int MultiPickBookControl { get; set; }
       
        public int MultiPickControl { get; set; }
       
        public int? MultiPickStopNumber { get; set; }
       
        public string MultiPickTransType { get; set; }
       
        public double? MultiPickMiles { get; set; }
       
        public string MultiPickDateOrdered { get; set; }
       
        public string MultiPickRouteFinalCode { get; set; }
       
        public string MultiPickRouteFinalDate { get; set; }
       
        public double? MultiPickTotalOrderMiles { get; set; }
       
        public int? MultiPickOrderSequence { get; set; }
       
        public string MultiPickOrderNumber { get; set; }
       
        public string MultiPickModUser { get; set; }
       
        public string MultiPickModDate { get; set; }
       
        public short? MultiPickDeliveryStopNumber { get; set; }
       
        public int? MultiPickPickNumber { get; set; }
       
        public string MultiPickTypeCode { get; set; }
       
        public string MultiPickTranCode { get; set; }
       
        public int? MultiPickTotalPX { get; set; }
       
        public int? MultiPickTotalCube { get; set; }
       
        public double? MultiPickTotalPL { get; set; }
       
        public double? MultiPickTotalWgt { get; set; }
       
        public int? MultiPickTotalCases { get; set; }
       
        public string MultiPickDateDelivered { get; set; }
       
        public string MultiPickDateRequired { get; set; }
       
        public string MultiPickDateLoad { get; set; }
       
        public string MultiPickPayCode { get; set; }
       
        public bool MultiPickBookLockBFCCost { get; set; }


        /// <summary>
        /// Transform DTO data to 365 Model data
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-03-09 added logic to send dates as strings to UI
        ///     this is changed to support multiple time zones and the issues where the JASON data is 
        ///     converted to Universal Time for Dates
        /// </remarks>
        public static Models.vBookMultiPick selectModelData(DTO.vBookMultiPick d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.vBookMultiPick modelRecord = new Models.vBookMultiPick();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "MultiPickBookMustLeaveByDateTime", "MultiPickBookExpDelDateTime", "MultiPickBookFinARInvoiceDate", "MultiPickDateOrdered", "MultiPickRouteFinalDate", "MultiPickModDate", "MultiPickDateDelivered", "MultiPickDateRequired", "MultiPickDateLoad" };
                string sMsg = "";
                modelRecord = (Models.vBookMultiPick)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null)
                {
                    //modelRecord.MultiPickModDate = Utilities.convertDateToDateTimeString(d.MultiPickModDate);
                    modelRecord.MultiPickModDate = clsApplication.convertDateToDateTimeString(d.MultiPickModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                    modelRecord.MultiPickBookMustLeaveByDateTime = Utilities.convertDateToShortDateString(d.MultiPickBookMustLeaveByDateTime);
                    modelRecord.MultiPickBookExpDelDateTime = Utilities.convertDateToShortDateString(d.MultiPickBookExpDelDateTime);
                    modelRecord.MultiPickBookFinARInvoiceDate = Utilities.convertDateToShortDateString(d.MultiPickBookFinARInvoiceDate);
                    modelRecord.MultiPickDateOrdered = Utilities.convertDateToShortDateString(d.MultiPickDateOrdered);
                    modelRecord.MultiPickRouteFinalDate = Utilities.convertDateToShortDateString(d.MultiPickRouteFinalDate);
                    modelRecord.MultiPickDateDelivered = Utilities.convertDateToShortDateString(d.MultiPickDateDelivered);
                    modelRecord.MultiPickDateRequired = Utilities.convertDateToShortDateString(d.MultiPickDateRequired);
                    modelRecord.MultiPickDateLoad = Utilities.convertDateToShortDateString(d.MultiPickDateLoad);
                }
            }

            return modelRecord;
        }

        /// <summary>
        /// Translate 365 Model data to DTO Model data
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        ///  Created by RHR for v-8.5.3.007 on 2023-03-09 added logic to convert strings to dates
        ///     used to fix issues with date conversion in the JSON data
        /// </remarks>
        public static DTO.vBookMultiPick selectLTSData(Models.vBookMultiPick d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            DTO.vBookMultiPick ltsRecord = new DTO.vBookMultiPick();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "MultiPickBookMustLeaveByDateTime", "MultiPickBookExpDelDateTime", "MultiPickBookFinARInvoiceDate", "MultiPickDateOrdered", "MultiPickRouteFinalDate", "MultiPickModDate", "MultiPickDateDelivered", "MultiPickDateRequired", "MultiPickDateLoad" };
                string sMsg = "";
                ltsRecord = (DTO.vBookMultiPick)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {                   

                    //ltsRecord.MultiPickModDate = Utilities.convertStringToNullDateTime(d.MultiPickModDate);
                    ltsRecord.MultiPickModDate = clsApplication.convertStringToNullDateTime(d.MultiPickModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                    ltsRecord.MultiPickBookMustLeaveByDateTime = Utilities.convertStringToNullDateTime(d.MultiPickBookMustLeaveByDateTime);
                    ltsRecord.MultiPickBookExpDelDateTime = Utilities.convertStringToNullDateTime(d.MultiPickBookExpDelDateTime);
                    ltsRecord.MultiPickBookFinARInvoiceDate = Utilities.convertStringToNullDateTime(d.MultiPickBookFinARInvoiceDate);
                    ltsRecord.MultiPickDateOrdered = Utilities.convertStringToNullDateTime(d.MultiPickDateOrdered);
                    ltsRecord.MultiPickRouteFinalDate = Utilities.convertStringToNullDateTime(d.MultiPickRouteFinalDate);
                    ltsRecord.MultiPickDateDelivered = Utilities.convertStringToNullDateTime(d.MultiPickDateDelivered);
                    ltsRecord.MultiPickDateRequired = Utilities.convertStringToNullDateTime(d.MultiPickDateRequired);
                    ltsRecord.MultiPickDateLoad = Utilities.convertStringToNullDateTime(d.MultiPickDateLoad);

                }
            }

            return ltsRecord;
        }


    }
}