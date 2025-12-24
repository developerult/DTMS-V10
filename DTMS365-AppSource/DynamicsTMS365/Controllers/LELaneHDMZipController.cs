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

namespace DynamicsTMS365.Controllers
{
    public class LELaneHDMZipController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 10/09/2018 
        /// initializes the Page property by calling the base class constructor
        /// Not sure if Utilities.PageEnum.CompanyMaint for the child data 
        /// Comp Contact will cause any trouble.  Some testing may be required
        /// </summary>
        public LELaneHDMZipController()
                : base(Utilities.PageEnum.LELaneHDMMaint)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LELaneHDMZipController";
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

        /// <summary>
        ///  Gets one vtblHDMZip record using the vtblHDMZipControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/09/2018
        ///   previous code to get the first contact record filterd by the vtblHDMZipvtblHDMZiprol
        ///   this method was modified to use the vtblHDMZipControl for id 
        ///   to return theselected record
        /// </remarks>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "HDMZipControl", filterValue = id.ToString() };

                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                Models.vtblHDMZip[] records = new Models.vtblHDMZip[] { };
                LTS.vtblHDMZip[] oData = new LTS.vtblHDMZip[] { };
                oData = oDAL.GetLEHDMZip(f,ref RecordCount );
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.vtblHDMZip.selectModelData(e)).ToArray();
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

        //
        /// <summary>
        /// Gets All the Child vtblHDMZip Records filtered by HDMZipHDMControl passed in as  id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/09/2018
        ///   new name for Get method renamed to support Edit Widget
        /// </remarks>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id == 0)
                {
                    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LELaneHDMMaint);
                }
                f.ParentControl = id;
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                Models.vtblHDMZip[] records = new Models.vtblHDMZip[] { };
                LTS.vtblHDMZip[] oData = new LTS.vtblHDMZip[] { };
                int RecordCount = 0;
                int count = 0;
                oData = oDAL.GetLEHDMZip(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = oData.Count();
                    records = (from e in oData select Models.vtblHDMZip.selectModelData(e)).ToArray();
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

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters); 
                Models.vtblHDMZip[] records = new Models.vtblHDMZip[] { };

                LTS.vtblHDMZip[] oData = new LTS.vtblHDMZip[] { };
                int RecordCount = 0;
                int count = 0;
                oData = oDAL.GetLEHDMZip( f, ref RecordCount);

                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.vtblHDMZip.selectModelData(e)).ToArray();
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
        public Models.Response Post([System.Web.Http.FromBody] Models.vtblHDMZip data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                LTS.vtblHDMZip record = Models.vtblHDMZip.selectLTSData(data);
                oDAL.InsertOrUpdateTariffHDMZip(record);
                bool[] oRecords = new bool[1];                
                oRecords[0] = true; //always returns true unless we have an error
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

        [HttpPost, ActionName("PostSpreadSheet")]
        public Models.Response PostSpreadSheet([System.Web.Http.FromBody] Models.vtblHDMZip[] data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                int iHDMControlDefault = readPagePrimaryKey(Parameters, Utilities.PageEnum.LELaneHDMMaint);
                int iUpdated = 0;
                int iProessed = 0;
                int iEntered = 0;
                List<DTO.NGLMessage> errs = new List<DTO.NGLMessage>();
                if (data != null && data.Count() > 0)
                {
                    iEntered = data.Count();
                    LTS.vtblHDMZip record = new LTS.vtblHDMZip();
                    
                    foreach (Models.vtblHDMZip idata in data)
                    {
                        iProessed++;
                        try
                        {
                            record = Models.vtblHDMZip.selectLTSData(idata);
                            if (record.HDMZipHDMControl == 0)
                            {
                                record.HDMZipHDMControl = iHDMControlDefault;
                            }
                            oDAL.InsertOrUpdateTariffHDMZip(record);
                            iUpdated++;

                        } catch(Exception e)
                        {
                            DTO.NGLMessage zipE = new DTO.NGLMessage();
                            zipE.ErrorMessage = string.Format("Failed to save record {0} out of {1} in the batch; Counry: {2} State: {3} City: {4} From Zip: {5} To Zip: {6}", iProessed, iEntered, idata.HDMZipCountry, idata.HDMZipState, idata.HDMZipCity, idata.HDMZipFrom, idata.HDMZipTo);
                            zipE.ErrorDetails = e.Message;
                            errs.Add(zipE);                           
                        }
                    }
                }
                //LTS.vtblHDMZip record = Models.vtblHDMZip.selectLTSData(data);
                //oDAL.InsertOrUpdateTariffHDMZip(record);
                bool[] oRecords = new bool[1];
                oRecords[0] = true; //always returns true unless we have an error
                response = new Models.Response(oRecords, 1);
                response.Err = errs.ToArray();
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
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                oDAL.DeleteHDMZipRange(id, Parameters.UserName);
                bool[] oRecords = new bool[1];
                oRecords[0] = true; //always returns true unless we have an error
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
        /// Delete all zip/location data for the provided HDMControl number passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.0.001 on 11/09/2021 delete all locations for HDM
        ///     support for HDM Zip Maintenance
        /// </remarks>
        [HttpDelete, ActionName("DELETEAll")]
        public Models.Response DELETEAll(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                oDAL.DeleteAllHDMZip(id);
                bool[] oRecords = new bool[1];
                oRecords[0] = true; //always returns true unless we have an error
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


        #region "Private Methods"


        #endregion
    }
}