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
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;
using Ngl.FreightMaster.Integration;
//using ProcessDataReturnValues = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues;


namespace DynamicsTMS365.Controllers
{
    public class EDILogController : NGLControllerBase
    {

        #region " Constructors "

        /// <summary> Initializes the Page property by calling the base class constructor </summary>
        public EDILogController() : base(Utilities.PageEnum.EDILogs){ }

        #endregion

        #region " Properties"

        /// <summary> This property is used for logging and error tracking. </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDILogController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"


        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        //Not Used
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {                
                Models.PackageDescription[] records = new Models.PackageDescription[] { };
                int count = 0;
                response = new Models.Response(records, count);
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

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        //Not Used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {                             
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); } //save the page filter for the next time the page loads
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.PackageDescription[] records = new Models.PackageDescription[] { };
                int count = 0;
                response = new Models.Response(records, count);
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

        //Not Used
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.PackageDescription data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
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

        //Not Used
        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
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

        /// <summary>
        /// Input Parameter data:
        ///   GenericResult.strField = EDI doc text
        ///   GenericResult.blnField = CarrierEDISend997 (bool - always true for now but is in case ever need to add it to UI in future)
        /// Return Results Object:
        ///   GenericResult.strArray = array of log strings
        ///   GenericResult.strField = EDI997Response (string of generated 997)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ProcessEDI")]
        public Models.Response ProcessEDI([System.Web.Http.FromBody]Models.GenericResult data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.GenericResult gr = ProcessDataManual(data.strField, data.blnField);
                Models.GenericResult[] oRecords = new Models.GenericResult[1] { gr };
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

        public Models.GenericResult ProcessDataManual(string sEDIData, bool CarrierEDISend997)
        {
            List<string> logs = new List<string>();
            Models.GenericResult gr = new Models.GenericResult() { strArray = logs.ToArray(), strField = "" };          
            if (string.IsNullOrWhiteSpace(sEDIData.Trim())) { return gr; }
            sEDIData = sEDIData.Replace("\n", ""); //strip out line breaks in case the user added them via paste
            string smtpFromAddress = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
            ////var enumResults = ProcessDataReturnValues.nglDataIntegrationComplete;
            var oEDIInput = new clsEDIInput() {
                Debug = System.Diagnostics.Debugger.IsAttached ? true : false,
                Source = "Manual EDI Process",
                //KeepLogDays = 1,
                //LogFile = @"C:\Data\TMSLog.txt",
                //SaveOldLog = false,
                AdminEmail = Parameters.UserEmail,
                FromEmail = !string.IsNullOrWhiteSpace(smtpFromAddress) ? smtpFromAddress : "DoNotReply@nextgeneration.com",
                GroupEmail = Parameters.UserEmail,
                Retry = 0,
                //SMTPServer = "nglMail.nextgeneration.com",//Me.SMTPServer,
                DBServer = Parameters.DBServer,
                Database = Parameters.Database,
                AuthorizationCode = Parameters.WCFAuthCode, //Me.AuthorizationCode,  
                ConnectionString = Parameters.ConnectionString
            };

            //GET THE CARRIER CONFIGURATION BASED ON THE CARRIER PARTNER CODE AND EDIACTION OF THE FIRST FILE
            string strISA = sEDIData.Left(106);
            string strSegSep = strISA.Substring(strISA.Length - 1);           
            string[] sISAs = System.Text.RegularExpressions.Regex.Split(sEDIData, @"\" + strSegSep + @"ISA\*"); //split any ISA sections           
            string strISAHeader = ""; //now we have a list of ISAs so split each ISAs by GS
            clsEDIISA oISA;
            foreach (var s in sISAs)
            {                
                if (s.Left(4) == "ISA*") { strISAHeader = s.Left(105); } else { strISAHeader = "ISA*" + s.Left(101); } //Add the ISA to the array we have to add the ISA* string back in if it does not exist
                oISA = new clsEDIISA(strISAHeader);
                string CarrierEDIPartnerCode = oISA.ISA06; //Get the Partner Code
                oISA.SegmentTerminator = strSegSep;               
                string[] sIEAs = System.Text.RegularExpressions.Regex.Split(s, @"\" + strSegSep + @"IEA\*"); //strip off the IEA segment               
                string[] sGSs = System.Text.RegularExpressions.Regex.Split(sIEAs[0], @"\" + strSegSep + @"GS\*"); //get the GS data
                string EDIXaction = "";
                for (int i = 1; i <= sGSs.Length - 1; i++) //we skip the first item because it holds the ISA data
                {
                    string sg = sGSs[i];
                    if(sg.Left(2) == "FA") { EDIXaction = "997"; }  
                    else if(sg.Left(2) == "QM") { EDIXaction = "214"; }
                    else if(sg.Left(2) == "GF") { EDIXaction = "990"; }
                    else if(sg.Left(2) == "IM") { EDIXaction = "210"; }
                    else if(sg.Left(2) == "RA") { EDIXaction = "820"; }
                    else if(sg.Left(2) == "SM") { EDIXaction = "204"; }
                }
                DAL.NGLCarrierEDIData oDAL = new DAL.NGLCarrierEDIData(Parameters);
                var carrierEDI = oDAL.GetCarrierEDIByPartnerCode(CarrierEDIPartnerCode, EDIXaction);
                if (carrierEDI != null) { CarrierEDISend997 = carrierEDI.CarrierEDIAcknowledgementRequested; } //Get CarrierEDISend997 from config
                break; //we only need to do this for the first record - we assume that all files are for the same carrier (*This is a requirement)
            }
            logs = oEDIInput.ProcessDataManual(sEDIData, Parameters.ConnectionString, DateProcessed: DateTime.Now, CarrierEDIAcknowledgementRequested: CarrierEDISend997);
            if (!string.IsNullOrWhiteSpace(oEDIInput.LastError)) { logs.Add(oEDIInput.LastError); }
            gr = new Models.GenericResult() { strArray = logs.ToArray(), strField = oEDIInput.EDI997Response };
            return gr; 
        }



        #endregion
    }
}