using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class SystemInfoController : NGLControllerBase
    {
        #region " Constructors "

        public SystemInfoController()
                : base(Utilities.PageEnum.SystemInfo)
        {
        }

        #endregion

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.SystemInfoController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " REST Services"

        /// <summary>
        /// Note: int id parameter is not actually used - it is only here because
        /// the nglRESTCRUDCtrl widget read function requires it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetSystemInfo")]
        public Models.Response GetSystemInfo(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                ////var sSetting = readPageSettings("ReportParentRoleControl", Parameters, Utilities.PageEnum.LEUsers);
                ////int groupControl = 0;
                ////int.TryParse(sSetting[0].UserPSMetaData, out groupControl);

                DTO.vAbout about = NGLSystemData.GetvAbout();

                string webpgsVersion = System.Configuration.ConfigurationManager.AppSettings["webpages:TMSVersion"];
                string webpgsLastModified = System.Configuration.ConfigurationManager.AppSettings["webpages:LastModified"];
                string strDatabase = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string strServer = System.Configuration.ConfigurationManager.AppSettings["DBServer"];

                string strAuthNumber = "Error getting value from database";
                string strVersion = "Error getting value from database";
                string strLastServerSoftwareModified = "Error getting value from database";

                if (about != null)
                {
                    if (about.ServerLastMod.HasValue) { strLastServerSoftwareModified = about.ServerLastMod.Value.ToShortDateString() + " " + about.ServerLastMod.Value.ToLongTimeString(); }
                    strAuthNumber = about.AuthNumber;
                    strVersion = about.version;
                }
              
                Models.SystemInfo info = new Models.SystemInfo {
                    CurrentClientSoftwareRelease = webpgsVersion,
                    LastClientModified = webpgsLastModified,
                    AuthNumber = strAuthNumber,
                    CurrentServerSoftwareRelease = strVersion,
                    LastServerSoftwareModified = strLastServerSoftwareModified,
                    Database = strDatabase,
                    Server = strServer
                };

                Models.SystemInfo[] oRecords = new Models.SystemInfo[1] { info };
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }



        [HttpPost, ActionName("EmailSystemInfo")]
        public Models.Response EmailSystemInfo([System.Web.Http.FromBody]Models.SystemInfo info)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                System.Text.StringBuilder sbBody = new System.Text.StringBuilder();

                string sFrom = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
                string sTo = System.Configuration.ConfigurationManager.AppSettings["NGLTechSupportEmail"]; //NOTE: Is this the correct email we want to send this to?
                if (string.IsNullOrWhiteSpace(sTo)) { sTo = "support@nextgeneration.com"; }              
                string newLine = System.Environment.NewLine + "<br />";
                string subject = "System Info from " + Parameters.UserName;

                sbBody.Append("System Info");
                sbBody.Append(newLine);
                sbBody.Append("Sent by User: " + Parameters.UserName);
                sbBody.Append(newLine);
                sbBody.Append(newLine);
                sbBody.Append("Client Software Version");
                sbBody.Append(newLine);
                sbBody.Append(string.Format("Current Release: {0}, Last Modified: {1}, Auth Number: {2}", info.CurrentClientSoftwareRelease, info.LastClientModified, info.AuthNumber));
                sbBody.Append(newLine);
                sbBody.Append("Server Software Version");
                sbBody.Append(newLine);
                sbBody.Append(string.Format("Current Release: {0}, Last Modified: {1}, Database {2}, Server: {3}", info.CurrentServerSoftwareRelease, info.LastServerSoftwareModified, info.Database, info.Server));
                sbBody.Append(newLine);

                if (!sendEmail(ref response, sFrom, sTo, "", subject, sbBody.ToString())) { return response; }

                //populate reponse data
                bool[] oRecords = new bool[1] { true };
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }



        #endregion



    }
}