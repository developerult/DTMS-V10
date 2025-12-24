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

namespace DynamicsTMS365.Controllers
{
    public class AddressBookController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.AddressBookController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " REST Services"

        [HttpGet, ActionName("GetAddressBookForLE")]
        public Models.Response GetAddressBookForLE()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLaneData oLane = new DAL.NGLLaneData(Parameters);
                DAL.Models.AddressBook[] retZips = new DAL.Models.AddressBook[] { };
                int count = 0;
                int RecordCount = 0;

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 500 : int.Parse(request["take"]);

                string strFilter = request["filter[filters][0][value]"];
                if (string.IsNullOrWhiteSpace(strFilter)) { strFilter = ""; }

                retZips = oLane.GetAddressBookForLE(ref RecordCount, Parameters.UserLEControl, strFilter, 1, 0, skip, take);

                if (retZips != null && retZips.Count() > 0)
                {
                    count = retZips.Length;
                }
                response = new Models.Response(retZips, RecordCount);
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


        [HttpGet, ActionName("GetStopData")]
        public Models.Response GetStopData()
        {
            DAL.NGLCompAMSUserFieldSettingData sDaL = new DAL.NGLCompAMSUserFieldSettingData(Parameters);
            //int compControl = sDaL.GetCompAMSUserFieldSettingFiltered(userControl).CompAMSUserFieldSettingCompControl;


            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;

                DAL.NGLtblStopData aNGLtblStopData = new DAL.NGLtblStopData(Parameters);

                if (aNGLtblStopData.GettblStopsFiltered() != null && aNGLtblStopData.GettblStopsFiltered().Count() > 0)
                {
                    count = aNGLtblStopData.GettblStopsFiltered().Length;
                }

                DAL.NGLCompData aNGLCompData = new DAL.NGLCompData(Parameters);
                response = new Models.Response(aNGLtblStopData.GettblStopsFiltered().OrderByDescending(c => c.StopControl).ToArray(), RecordCount);
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody] Models.tblStop data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblStop aTblStop = new LTS.tblStop();
                aTblStop.StopName = data.StopName;
                aTblStop.StopAddress1 = data.StopAddress1;
                aTblStop.StopCity = data.StopCity;
                aTblStop.StopState = data.StopState;
                aTblStop.StopZip = data.StopZip;

                DAL.NGLtblStopData aNGLtblStopData = new DAL.NGLtblStopData(Parameters);
                bool blnRet = aNGLtblStopData.SaveStopData(aTblStop);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                //Error handler
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