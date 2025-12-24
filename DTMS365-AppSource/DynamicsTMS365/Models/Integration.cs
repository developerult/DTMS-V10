using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;
using System.Configuration;
using DynamicsTMS365.Controllers;
using NGL.UTC.Library;

namespace DynamicsTMS365.Models
{
    public class Integration
    {

        public int IntegrationControl { get; set; }

        public int ERPSettingControl { get; set; }

        public int IntegrationTypeControl { get; set; }

        public string TMSURI { get; set; }

        public string TMSAuthUser { get; set; }

        public string TMSAuthPassword { get; set; }

        public string TMSAuthCode { get; set; }

        public string TMSSpec { get; set; } // not supported

        public string TMSNotes { get; set; }

        public string ERPURI { get; set; }

        public string ERPAuthUser { get; set; }

        public string ERPAuthPassword { get; set; }

        public string ERPAuthCode { get; set; }

        public string ERPNotes { get; set; }

        //public DateTime? IntegrationModDate { get; set; }
        public string IntegrationModDate { get; set; }

        public string IntegrationModUser { get; set; }

        public string ERPSpec { get; set; }

        public string TMSAuthURI { get; set; }

        public string TMSScopeURI { get; set; }

        public string TMSActionURI { get; set; }

        public string ERPActionURI { get; set; }

        public string IntegrationType { get; set; }

        private byte[] _IntegrationUpdated;

        public string IntegrationUpdated {
            get
            {
                if (this._IntegrationUpdated != null) { return Convert.ToBase64String(this._IntegrationUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._IntegrationUpdated = null; } else { this._IntegrationUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _IntegrationUpdated = val; }
        public byte[] getUpdated() { return _IntegrationUpdated; }

        

        public static Models.Integration selectModelData(LTS.vIntegration d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.Integration modelRecord = new Models.Integration();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "IntegrationUpdated", "ERPSetting", "Integration", "TMSSpec", "ERPSpec", "IntegrationModDate" };
                string sMsg = "";
                modelRecord = (Models.Integration)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { 
                    modelRecord.setUpdated(d.IntegrationUpdated.ToArray());
                    modelRecord.IntegrationModDate = clsApplication.convertDateToDateTimeString(d.IntegrationModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static Models.Integration selectModelData(DTO.Integration d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.Integration modelRecord = new Models.Integration();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "IntegrationUpdated", "ERPSetting", "Integration", "TMSSpec", "ERPSpec", "IntegrationModDate" };
                string sMsg = "";
                modelRecord = (Models.Integration)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    modelRecord.setUpdated(d.IntegrationUpdated.ToArray());
                    modelRecord.IntegrationModDate = clsApplication.convertDateToDateTimeString(d.IntegrationModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update

                }
            }
            return modelRecord;
        }

        public static DTO.Integration selectDTOData(Models.Integration d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            DTO.Integration dtoRecord = new DTO.Integration();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"IntegrationType", "IntegrationUpdated", "ERPSetting", "Integration", "TMSSpec", "ERPSpec", "IntegrationModDate" };
                string sMsg = "";
                dtoRecord = (DTO.Integration)DTran.CopyMatchingFields(dtoRecord, d, skipObjs, ref sMsg);
                if (dtoRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    dtoRecord.IntegrationUpdated = bupdated == null ? new byte[0] : bupdated;
                    dtoRecord.IntegrationModDate = clsApplication.convertStringToNullDateTime(d.IntegrationModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return dtoRecord;
        }
    }
}