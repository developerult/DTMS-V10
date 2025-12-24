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
    /// Model for Second Child Display for Workflow Setup page links to tblParProcessOption
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.3.006 on 10/20/2022
    /// </remarks>
    public class tblParProcessOption
    {

        public int ParProcOptControl { get; set; }
        public int ParProcOptParProcControl { get; set; }
        public string ParProcOptParKey { get; set; }
        public int ParProcOptParKeyValueType { get; set; }
        public int ParProcOptParKeyTextType { get; set; }
        public string ParProcOptDescription { get; set; }
        public string ParProcOptName { get; set; }
        //public DateTime? ParProcOptModDate { get; set; }
        public string ParProcOptModDate { get; set; }
        public string ParProcOptModUser { get; set; }

        private byte[] _ParProcOptUpdated;

        /// <summary>
        /// ParProcOptUpdated should be bound to UI, _ParProcOptUpdated is only bound on the controller
        /// </summary>
        public string ParProcOptUpdated
        {
            get
            {
                if (this._ParProcOptUpdated != null) { return Convert.ToBase64String(this._ParProcOptUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._ParProcOptUpdated = null; } else { this._ParProcOptUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _ParProcOptUpdated = val; }
        public byte[] getUpdated() { return _ParProcOptUpdated; }


        public static Models.tblParProcessOption selectModelData(LTS.tblParProcessOption d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            Models.tblParProcessOption modelRecord = new Models.tblParProcessOption();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"ParProcOptUpdated", "tblParProcOptTxtItems", "tblParProcOptValItems", "tblParProcess", "ParProcOptModDate" };
                string sMsg = "";
                modelRecord = (Models.tblParProcessOption)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    modelRecord.setUpdated(d.ParProcOptUpdated.ToArray());
                    modelRecord.ParProcOptModDate = clsApplication.convertDateToDateTimeString(d.ParProcOptModDate, userData.UserCultureInfo, serverTimeZone, userData.UserTimeZone); //UTC Mod Date Update
                }
            }
            return modelRecord;
        }

        public static LTS.tblParProcessOption selectLTSData(Models.tblParProcessOption d)
        {
            string serverTimeZone = ConfigurationManager.AppSettings["ServerTimeZone"];
            UsersController userController = new UsersController(); // Create an instance of UserController
            var userSettings = userController.GetUserSettings();
            var userData = (Models.User)userSettings.Data.GetValue(0);

            LTS.tblParProcessOption LTSRecord = new LTS.tblParProcessOption();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ParProcOptUpdated", "tblParProcOptTxtItems", "tblParProcOptValItems", "tblParProcess", "ParProcOptModDate" };
                string sMsg = "";
                LTSRecord = (LTS.tblParProcessOption)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.ParProcOptUpdated = bupdated == null ? new byte[0] : bupdated;
                    LTSRecord.ParProcOptModDate = clsApplication.convertStringToNullDateTime(d.ParProcOptModDate, userData.UserCultureInfo, userData.UserTimeZone, serverTimeZone); //UTC Mod Date Update
                }
            }
            return LTSRecord;
        }

    }
}