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
    public class BookFee
    {


        public bool NotSupported { get; set; }
        public bool BookFeesAccessorialProfileSpecific { get; set; }
        public string BookFeesModUser { get; set; }
        //public DateTime? BookFeesModDate { get; set; }
        public string BookFeesModDate { get; set; }
        public string BookFeesDependencyKey { get; set; }
        public int BookFeesAccessorialDependencyTypeControl { get; set; }
        public int BookFeesAccessorialOverRideReasonControl { get; set; }
        public int BookFeesAccessorialFeeCalcTypeControl { get; set; }
        public int BookFeesTarBracketTypeControl { get; set; }
        public int BookFeesAccessorialFeeAllocationTypeControl { get; set; }
        public string BookFeesBOLPlacement { get; set; }
        public string BookFeesBOLText { get; set; }
        public int BookFeesTaxSortOrder { get; set; }
        public bool BookFeesIsTax { get; set; }
        
        public bool BookFeesTaxable { get; set; }
        
        public string BookFeesEDICode { get; set; }
        
        public int BookFeesControl { get; set; }
        
        public int BookFeesBookControl { get; set; }
        
        public decimal BookFeesMinimum { get; set; }
        
        public decimal BookFeesValue { get; set; }
        
        public double BookFeesVariable { get; set; }
        public bool AllowOverwrite { get; set; }
        
        public int BookFeesAccessorialCode { get; set; }
        
        public bool BookFeesOverRidden { get; set; }
        
        public int BookFeesVariableCode { get; set; }
        
        public bool BookFeesVisible { get; set; }
        
        public bool BookFeesAutoApprove { get; set; }
        
        public bool BookFeesAllowCarrierUpdates { get; set; }
        
        public string BookFeesCaption { get; set; }
        
        public int BookFeesAccessorialFeeTypeControl { get; set; }
        public bool BookFeesMissingFee { get; set; }

        private byte[] _BookFeesUpdated;

        /// <summary>
        /// BookFeesUpdated should be bound to UI, _BookFeesUpdated is only bound on the controller
        /// </summary>
        public string BookFeesUpdated
        {
            get
            {
                if (this._BookFeesUpdated != null) { return Convert.ToBase64String(this._BookFeesUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._BookFeesUpdated = null; } else { this._BookFeesUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _BookFeesUpdated = val; }
        public byte[] getUpdated() { return _BookFeesUpdated; }


        public static Models.BookFee selectModelData(DTO.BookFee d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.BookFee modelRecord = new Models.BookFee();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookFeesUpdated", "BookFees", "BookFee", "Book", "_BookFees", "_Book", "BookFeesModDate" };
                string sMsg = "";
                modelRecord = (Models.BookFee)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null)
                {
                    modelRecord.setUpdated(d.BookFeesUpdated.ToArray());
                    modelRecord.BookFeesModDate = clsApplication.convertDateToDateTimeString(d.BookFeesModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static DTO.BookFee selectDTOData(Models.BookFee d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            DTO.BookFee dtoRecord = new DTO.BookFee();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookFeesUpdated", "BookFees", "BookFee", "Book", "_BookFees", "_Book", "BookFeesModDate" };
                string sMsg = "";
                dtoRecord = (DTO.BookFee)DTran.CopyMatchingFields(dtoRecord, d, skipObjs, ref sMsg);
                if (dtoRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    dtoRecord.BookFeesUpdated = bupdated == null ? new byte[0] : bupdated;
                    dtoRecord.BookFeesModDate = clsApplication.convertStringToNullDateTime(d.BookFeesModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return dtoRecord;
        }

        
    }
}