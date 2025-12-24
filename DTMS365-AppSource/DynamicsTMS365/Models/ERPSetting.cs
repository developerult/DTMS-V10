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
    public class ERPSetting
    {

        public int ERPSettingControl { get; set; }

        public string LegalEntity{ get; set; }

        public int ERPTypeControl{ get; set; }

        public string Description{ get; set; }

        public string Version{ get; set; }

        public int ERPAuth{ get; set; }

        public string ERPUser{ get; set; }

        public string ERPPassword{ get; set; }

        public string ERPCertificate{ get; set; }

        //public DateTime? ERPSettingModDate{ get; set; }
        public string ERPSettingModDate{ get; set; }

        public string ERPSettingModUser{ get; set; }

        public int ERPExportMaxRetry{ get; set; }

        public int ERPExportRetryMinutes{ get; set; }

        public int ERPExportMaxRowsReturned{ get; set; }

        public bool ERPExportAutoConfirmation{ get; set; }

        public string ERPAuthURI{ get; set; }

        public string ERPTypeName { get; set; }

        private byte[] _ERPSettingUpdated;

        public string ERPSettingUpdated{
            get
            {
                if (this._ERPSettingUpdated != null) { return Convert.ToBase64String(this._ERPSettingUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._ERPSettingUpdated = null; } else { this._ERPSettingUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _ERPSettingUpdated = val; }
        public byte[] getUpdated() { return _ERPSettingUpdated; }


        public static Models.ERPSetting selectModelData(DTO.ERPSetting d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.ERPSetting modelRecord = new Models.ERPSetting();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ERPSettingUpdated", "ERPSetting", "ERPSettingModDate" };
                string sMsg = "";
                modelRecord = (Models.ERPSetting)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    modelRecord.setUpdated(d.ERPSettingUpdated.ToArray());
                    modelRecord.ERPSettingModDate = clsApplication.convertDateToDateTimeString(d.ERPSettingModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static Models.ERPSetting selectModelData(LTS.vERPSetting d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.ERPSetting modelRecord = new Models.ERPSetting();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ERPSettingUpdated", "ERPSetting"};
                string sMsg = "";
                modelRecord = (Models.ERPSetting)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    modelRecord.setUpdated(d.ERPSettingUpdated.ToArray());
                    modelRecord.ERPSettingModDate = clsApplication.convertDateToDateTimeString(d.ERPSettingModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static DTO.ERPSetting selectDTOData(Models.ERPSetting d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            DTO.ERPSetting dtoRecord = new DTO.ERPSetting();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"ERPTypeName", "ERPSettingUpdated", "ERPSetting", "ERPSettingModDate" };
                string sMsg = "";
                dtoRecord = (DTO.ERPSetting)DTran.CopyMatchingFields(dtoRecord, d, skipObjs, ref sMsg);
                if (dtoRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    dtoRecord.ERPSettingUpdated = bupdated == null ? new byte[0] : bupdated;
                    dtoRecord.ERPSettingModDate = clsApplication.convertStringToNullDateTime(d.ERPSettingModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return dtoRecord;
        }


    }
}