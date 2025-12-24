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
    public class vtblSSOAConfig
    {

        public int SSOACControl { get; set; }

        public int SSOACSSOALEControl { get; set; }

        public int SSOACCompControl{ get; set; }

        public int SSOALEAdminControl { get; set; }

        public string SSOACName{ get; set; }

        public string SSOACDesc{ get; set; }

        public string SSOACValue{ get; set; }

        public string SSOACModUser{ get; set; }

        private byte[] _SSOACUpdated { get; set; }

        // Modified by RHR for v-8.5.4.004 on 12/27/2023
        public String SSOACModDate { get; set; }

        public string SSOALEClientID { get; set; }

        public string SSOATypeName { get; set; }

        public string SSOATypeDesc { get; set; }

        public string CompName { get; set; }

        public int? CompNumber { get; set; }

        public int? SSOALESSOATypeControl { get; set; }



        /// <summary>
        /// SSOACUpdated should be bound to UI, _SSOACUpdated is only bound on the controller
        /// </summary>
        public string SSOACUpdated
        {
            get
            {
                if (this._SSOACUpdated != null) { return Convert.ToBase64String(this._SSOACUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._SSOACUpdated = null; } else { this._SSOACUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _SSOACUpdated = val; }
        public byte[] getUpdated() { return _SSOACUpdated; }


        public static Models.vtblSSOAConfig selectModelData(LTS.vtblSSOAConfig d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.vtblSSOAConfig modelRecord = new Models.vtblSSOAConfig();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"SSOACModDate", "SSOACUpdated", "tblSSOAConfig", "tblSSOALEConfig" };
                string sMsg = "";
                modelRecord = (Models.vtblSSOAConfig)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { 
                    modelRecord.setUpdated(d.SSOACUpdated.ToArray());
                    //modelRecord.SSOACModDate = Utilities.convertDateToDateTimeString(d.SSOACModDate);
                    modelRecord.SSOACModDate = clsApplication.convertDateToDateTimeString(d.SSOACModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static LTS.tblSSOAConfig selectLTSData(Models.vtblSSOAConfig d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            LTS.tblSSOAConfig LTSRecord = new LTS.tblSSOAConfig();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "SSOACModDate","SSOALEClientID","SSOATypeName","SSOATypeDesc","CompName","CompNumber", "SSOALESSOATypeControl", "SSOACUpdated", "tblSSOAConfig", "tblSSOALEConfig" };
                string sMsg = "";
                LTSRecord = (LTS.tblSSOAConfig)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.SSOACUpdated = bupdated == null ? new byte[0] : bupdated;
                    //LTSRecord.SSOACModDate = Utilities.convertStringToDateTime(d.SSOACModDate);
                    LTSRecord.SSOACModDate = clsApplication.convertStringToDateTime(d.SSOACModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return LTSRecord;
        }



    }
}