using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Controllers
{
    public class TariffHDMController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public TariffHDMController()
               : base(Utilities.PageEnum.TariffHDMs)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.TariffHDMController";
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
        //    POST 	/API/objectcontroller{data
        //}  : Create a new object or Update a the current object if the control number exists
        /// 
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"
        /// 

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a HDMControl associated with the select Record 
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "HDMTariffXrefControl";
                    f.filterValue = id.ToString();
                }
                f.LEAdminControl = Parameters.UserLEControl;
                LTS.vtblHDMTariffXref[] oData = new LTS.vtblHDMTariffXref[] { };
                oData = oDAL.GetTariffHDM(f, ref RecordCount);
               
                if (oData != null)
                {
                    count = 1;
                }
                response = new Models.Response(oData, count);

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
                f.LEAdminControl = Parameters.UserLEControl;

                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);                
                LTS.vtblHDMTariffXref[] oData = new LTS.vtblHDMTariffXref[] { };
                oData = oDAL.GetTariffHDM(f, ref RecordCount);

                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                   
                }
                response = new Models.Response(oData, count);
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
        public Models.Response Post([System.Web.Http.FromBody] LTS.vtblHDMTariffXref data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                data.HDMLEAdminControl = Parameters.UserLEControl;
                int iCarrTarControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                int iHDMControl = data.HDMTariffXrefHDMControl;
                bool[] oRecords = new bool[1];
                oRecords[0] = false;
                if (iCarrTarControl != 0 && iHDMControl != 0)
                {
                    int iRet = oDAL.InsertTariffHDM(iHDMControl,iCarrTarControl);
                    if (iRet != 0)
                    {
                        oRecords[0] = true;
                    }
                }               

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
                DAL.NGLHDMData oDAL = new DAL.NGLHDMData(Parameters);
                int iCarrTarControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                bool[] oRecords = new bool[1];
                oRecords[0] = false;
                if (iCarrTarControl != 0 && id != 0)
                {
                    int iRet = oDAL.DeleteTariffHDM(id, iCarrTarControl);
                    if (iRet != 0)
                    {
                        oRecords[0] = true;
                    }
                }                
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