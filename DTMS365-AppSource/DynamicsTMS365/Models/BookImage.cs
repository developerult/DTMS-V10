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
    /// <summary>
    /// Model used to map the LTS BookImage to the Web version used by the JSON REST Services 
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.4.003 on 10/26/2023
    ///     for future use.  the current logic does not support using the 
    ///     Full CRUD functionality  use the LTS.vBookImageSummary logic instead
    ///     the vBookImageSummary is read only and the page provides a link to open the image or the file
    /// </remarks>
    public class BookImage
    {

        public int BookImageControl { get; set; }

        public int BookImageBookControl { get; set; }

        public int BookImageTypeCode { get; set; }

        public string BookImageTypeDesc { get; set; }

        public string BookImageModDate { get; set; }

        public string BookImageModUser { get; set; }

        private byte[] _BookImageOLE;

        /// <summary>
        /// BookImageOLE should be bound to UI, _BookImageOLE is only bound on the controller
        /// </summary>
        public string BookImageOLE
        {
            get
            {
                if (this._BookImageOLE != null) { return Convert.ToBase64String(this._BookImageOLE); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._BookImageOLE = null; } else { this._BookImageOLE = Convert.FromBase64String(value); }
            }
        }

        public void setBookImageOLE(byte[] val) { _BookImageOLE = val; }
        public byte[] getBookImageOLE() { return _BookImageOLE; }



        private byte[] _BookImageUpdated;

        /// <summary>
        /// BookImageUpdated should be bound to UI, _BookImageUpdated is only bound on the controller
        /// </summary>
        public string BookImageUpdated
        {
            get
            {
                if (this._BookImageUpdated != null) { return Convert.ToBase64String(this._BookImageUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._BookImageUpdated = null; } else { this._BookImageUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _BookImageUpdated = val; }
        public byte[] getUpdated() { return _BookImageUpdated; }


        public static Models.BookImage selectModelData(LTS.BookImage d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.BookImage modelRecord = new Models.BookImage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookImageUpdated", "BookImageOLE", "BookImage", "Book", "UTC Mod Date Update", "BookImageModDate", "_BookImage", "_Book" };
                string sMsg = "";
                modelRecord = (Models.BookImage)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { 
                    modelRecord.setUpdated(d.BookImageUpdated.ToArray());
                    modelRecord.setBookImageOLE(d.BookImageOLE.ToArray());
                    modelRecord.BookImageModDate = clsApplication.convertDateToDateTimeString(d.BookImageModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static LTS.BookImage selectDTOData(Models.BookImage d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            LTS.BookImage ltsRecord = new LTS.BookImage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookImageUpdated", "BookImageOLE", "BookImage", "BookImageModDate", "Book", "_BookImage", "_Book" };
                string sMsg = "";
                ltsRecord = (LTS.BookImage)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BookImageUpdated = bupdated == null ? new byte[0] : bupdated;

                    byte[] bBookImageOLE = d.getBookImageOLE();
                    ltsRecord.BookImageOLE = bBookImageOLE == null ? new byte[0] : bBookImageOLE;
                    ltsRecord.BookImageModDate = clsApplication.convertStringToNullDateTime(d.BookImageModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return ltsRecord;
        }


    }
}