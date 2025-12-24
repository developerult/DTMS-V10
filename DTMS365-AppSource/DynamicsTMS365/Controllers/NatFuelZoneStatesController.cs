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
using System;

namespace DynamicsTMS365.Controllers
{

    public class NatFuelZoneStatesController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public NatFuelZoneStatesController()
                : base(Utilities.PageEnum.NatFuelZones)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NatFuelZoneStatesController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
       
        private Models.vNatFuelZone selectModelData(LTS.vNatFuelZoneState d)
        {
            Models.vNatFuelZone modelRecord = new Models.vNatFuelZone();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "NatFuelZoneUpdated" };
                string sMsg = "";
                modelRecord = (Models.vNatFuelZone)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
               // if (modelRecord != null) { modelRecord.setUpdated(d.NatFuelZoneUpdated.ToArray()); }
            }

            return modelRecord;
        }

        public static LTS.NatFuelZoneState selectLTSData(Models.NatFuelZone d)
        {
            LTS.NatFuelZoneState ltsRecord = new LTS.NatFuelZoneState();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "NatFuelZoneUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.NatFuelZoneState)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.NatFuelZoneStatesUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"
        //    POST 	/API/objectcontroller{data
        //}  : Create a new object or Update a the current object if the control number exists
        /// 
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"
        /// 

      

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

        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                
                DAL.NGLNatFuelZoneStateData oDAL = new DAL.NGLNatFuelZoneStateData(Parameters);
                Models.vNatFuelZone[] records = new Models.vNatFuelZone[] { };
                LTS.vNatFuelZoneState[] oData1 = new LTS.vNatFuelZoneState[] { };               
                oData1 = oDAL.GetNatFuelZoneStates365( f, ref RecordCount);

                if (oData1 != null && oData1.Count() > 0)
                {
                    count = oData1.Count();
                    records = (from e in oData1 select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.vNatFuelZone data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                DAL.NGLNatFuelZoneStateData oDAL = new DAL.NGLNatFuelZoneStateData(Parameters);
                LTS.NatFuelZoneState record = new LTS.NatFuelZoneState();
                record.NatFuelZoneStatesState = data.NatFuelState;
                record.NatFuelZoneStatesNatFuelZoneID = data.NatFuelZoneID;
                //record.NatFuelZoneStatesNatFuelZoneID = data.NatFuelZoneName;                              
                bool[] oRecords = new bool[1];
                oRecords[0] = oDAL.ValidateupdatedRecord365(record);
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

        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;              
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                DAL.NGLNatFuelZoneStateData oDAL = new DAL.NGLNatFuelZoneStateData(Parameters);
                Models.vNatFuelZone[] records = new Models.vNatFuelZone[] { };
                LTS.vNatFuelZoneState[] oData = new LTS.vNatFuelZoneState[] { };
                oData = oDAL.GetNatFuelZoneStates365(f, ref RecordCount).Where(x => x.NatFuelzoneStateRowID == id).ToArray();
                bool[] oRecords = new bool[1];
                oRecords[0] = oDAL.DeleteNatFuelZone365(oData[0].NatFuelState.ToString());
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