using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;
using NGL.UTC.Library;
using DynamicsTMS365.Controllers;
using System.Configuration;

namespace DynamicsTMS365.Models
{
    public class CarrierProNumber
    {


        public long? CarrProCheckDigitWeightFactor { get; set; }
        public bool CarrProCheckDigitSplitWeightFactorDigits { get; set; }
        public bool CarrProCheckDigitUseIndexForWeightFactor { get; set; }
        public int CarrProCheckDigitIndexForWeightFactorMin { get; set; }
        public string CarrProCheckDigitErrorCode { get; set; }
        public string CarrProCheckDigit10Code { get; set; }
        public string CarrProCheckDigitOver10Code { get; set; }
        public string CarrProCheckDigitZeroCode { get; set; }
        public string CarrProExp1 { get; set; }
        public string CarrProExp2 { get; set; }
        public string CarrProExp3 { get; set; }
        public string CarrProExp4 { get; set; }
        public string CarrProUser1 { get; set; }
        public string CarrProUser2 { get; set; }
        public string CarrProUser3 { get; set; }
        public string CarrProUser4 { get; set; }
        //public DateTime? CarrProModDate { get; set; }
        public string CarrProModDate { get; set; }
        public string CarrProModUser { get; set; }
        public int CarrProLength { get; set; }
        public bool CarrProCheckDigitUseSubtractionFactor { get; set; }
        public long CarrProSeedWarningSeed { get; set; }
        public long CarrProSeedCurrent { get; set; }
        public int CarrProControl { get; set; }
        public int CarrProCarrierControl { get; set; }
        public int CarrProCompControl { get; set; }
        public int CarrProChkDigAlgControl { get; set; }
        public string CarrProName { get; set; }
        public string CarrProDesc { get; set; }
        public string CarrProPrefix { get; set; }
        public string CarrProPrefixSpacer { get; set; }
        public int CarrProSeedStepFactor { get; set; }
        public string CarrProSuffix { get; set; }
        public string CarrProCheckDigitSpacer { get; set; }
        public bool CarrProPrintCheckDigitOnSeperateBarCode { get; set; }
        public bool CarrProPrintSpacersOnBarCode { get; set; }
        public bool CarrProActive { get; set; }
        public bool CarrProAppendPrefixForCheckDigit { get; set; }
        public bool CarrProAppendSuffixForCheckDigit { get; set; }
        public long CarrProSeedStart { get; set; }
        public long CarrProSeedEnd { get; set; }
        public string CarrProSuffixSpacer { get; set; }
        public int CarrProCheckDigitSubtractionFactor { get; set; }


        private byte[] _CarrProUpdated;

        /// <summary>
        /// CarrProUpdated should be bound to UI, _CarrProUpdated is only bound on the controller
        /// </summary>
        public string CarrProUpdated
        {
            get
            {
                if (this._CarrProUpdated != null) { return Convert.ToBase64String(this._CarrProUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._CarrProUpdated = null; } else { this._CarrProUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _CarrProUpdated = val; }
        public byte[] getUpdated() { return _CarrProUpdated; }


        public static Models.CarrierProNumber selectModelData(DTO.CarrierProNumber d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);


            Models.CarrierProNumber modelRecord = new Models.CarrierProNumber();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrProUpdated", "CarrProModDate" };
                string sMsg = "";
                modelRecord = (Models.CarrierProNumber)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    modelRecord.setUpdated(d.CarrProUpdated.ToArray());
                    modelRecord.CarrProModDate = clsApplication.convertDateToDateTimeString(d.CarrProModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static DTO.CarrierProNumber selectDTOData(Models.CarrierProNumber d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            DTO.CarrierProNumber dtoRecord = new DTO.CarrierProNumber();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrProUpdated", "CarrProModDate" };
                string sMsg = "";
                dtoRecord = (DTO.CarrierProNumber)DTran.CopyMatchingFields(dtoRecord, d, skipObjs, ref sMsg);
                if (dtoRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    dtoRecord.CarrProUpdated = bupdated == null ? new byte[0] : bupdated;
                    dtoRecord.CarrProModDate = clsApplication.convertStringToNullDateTime(d.CarrProModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return dtoRecord;
        }

    }
}