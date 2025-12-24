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
    /// Model for First Child Display for Workflow Setup page links to tblParProcess
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.3.006 on 10/20/2022
    /// </remarks>
    public class tblParProcess
    {
        public int ParProcControl { get; set; }
        public int ParProcCategoryControl { get; set; }
        public string ParProcName { get; set; }
        public string ParProcDescription { get; set; }
        public string ParProcModUser { get; set; }
        //public DateTime? ParProcModDate { get; set; }
        public string ParProcModDate { get; set; }

        private byte[] _ParProcUpdated;

        /// <summary>
        /// ParProcUpdated should be bound to UI, _ParProcUpdated is only bound on the controller
        /// </summary>
        public string ParProcUpdated
        {
            get
            {
                if (this._ParProcUpdated != null) { return Convert.ToBase64String(this._ParProcUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._ParProcUpdated = null; } else { this._ParProcUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _ParProcUpdated = val; }
        public byte[] getUpdated() { return _ParProcUpdated; }


        public static Models.tblParProcess selectModelData(LTS.tblParProcess d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.tblParProcess modelRecord = new Models.tblParProcess();
            if (d != null)

            {
                List<string> skipObjs = new List<string> {"ParProcUpdated", "tblParCategory", "tblParProcessOptions", "ParProcModDate" };
                string sMsg = "";
                modelRecord = (Models.tblParProcess)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    modelRecord.setUpdated(d.ParProcUpdated.ToArray()); 
                    modelRecord.ParProcModDate = clsApplication.convertDateToDateTimeString(d.ParProcModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static LTS.tblParProcess selectLTSData(Models.tblParProcess d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            LTS.tblParProcess LTSRecord = new LTS.tblParProcess();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ParProcUpdated", "tblParCategory", "tblParProcessOptions", "ParProcModDate" };
                string sMsg = "";
                LTSRecord = (LTS.tblParProcess)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.ParProcUpdated = bupdated == null ? new byte[0] : bupdated;
                    LTSRecord.ParProcModDate = clsApplication.convertStringToNullDateTime(d.ParProcModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return LTSRecord;
        }
    }
}