using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;
using System.EnterpriseServices.CompensatingResourceManager;
using DynamicsTMS365.Controllers;
using System.Configuration;
using NGL.UTC.Library;

namespace DynamicsTMS365.Models
{
    public class vtblSSOALEConfig
    {
        
        public int SSOALEControl { get; set; }

        public int SSOALESSOAControl { get; set; }

        public int SSOALELEAdminControl { get; set; }

        public string SSOALEClientID { get; set; } // maps to Account Number

        public string SSOALELoginURL { get; set; }

        public string SSOALEDataURL { get; set; }

        public string SSOALERedirectURL { get; set; }

        public string SSOALEClientSecret { get; set; } // maps to Token Secret

        public string SSOALEAuthCode { get; set; } // maps to Token Client ID

        public string SSOALEModUser { get; set; }

        public int SSOALESSOATypeControl { get; set; }

        // Modified by RHR for v-8.5.4.004 on 12/27/2023
        public String SSOALEModDate { get; set; }

        public string SSOAName { get; set; }

        public string SSOATypeName { get; set; }

        public string SSOATypeDesc { get; set; }

        public string LEAdminLegalEntity { get; set; }


        private byte[] _SSOALEUpdated;


        /// <summary>
        /// SSOALEUpdated should be bound to UI, _SSOALEUpdated is only bound on the controller
        /// </summary>
        public string SSOALEUpdated
        {
            get
            {
                if (this._SSOALEUpdated != null) { return Convert.ToBase64String(this._SSOALEUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._SSOALEUpdated = null; }
                else if (value == "0")
                {
                    this._SSOALEUpdated = null;
                }
                else
                {
                    try
                    {
                        this._SSOALEUpdated = Convert.FromBase64String(value);
                    }
                    catch
                    {
                        this._SSOALEUpdated = null;
                    }
                    
                }
            }
        }

        public void setUpdated(byte[] val) { _SSOALEUpdated = val; }
        public byte[] getUpdated() { return _SSOALEUpdated; }


        public static Models.vtblSSOALEConfig selectModelData(LTS.vtblSSOALEConfig d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.vtblSSOALEConfig modelRecord = new Models.vtblSSOALEConfig();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"SSOALEModDate", "SSOALEUpdated", "tblSSOAConfig", "tblSingleSignOnAccount", "tblSSOALEConfig" };
                string sMsg = "";
                modelRecord = (Models.vtblSSOALEConfig)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { 
                    modelRecord.setUpdated(d.SSOALEUpdated.ToArray());
                    //modelRecord.SSOALEModDate = Utilities.convertDateToDateTimeString(d.SSOALEModDate);
                    modelRecord.SSOALEModDate = clsApplication.convertDateToDateTimeString(d.SSOALEModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static LTS.tblSSOALEConfig selectLTSData(Models.vtblSSOALEConfig d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            LTS.tblSSOALEConfig LTSRecord = new LTS.tblSSOALEConfig();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LEAdminLegalEntity", "SSOATypeDesc", "SSOATypeName", "SSOAName", "SSOALEModDate", "SSOALEUpdated", "tblSSOAConfig", "tblSingleSignOnAccount", "tblSSOALEConfig" };
                string sMsg = "";
                LTSRecord = (LTS.tblSSOALEConfig)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.SSOALEUpdated = bupdated == null ? new byte[0] : bupdated;
                    //LTSRecord.SSOALEModDate = Utilities.convertStringToDateTime(d.SSOALEModDate);
                    LTSRecord.SSOALEModDate = clsApplication.convertStringToDateTime(d.SSOALEModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return LTSRecord;
        }

    }
}