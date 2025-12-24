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
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;
using BLL = NGL.FM.BLL;

//Added By LVV on 8/19/20 for v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility

namespace DynamicsTMS365.Controllers
{
    public class NGLSysModuleController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary> Initializes the Page property by calling the base class constructor </summary>
        public NGLSysModuleController() : base(Utilities.PageEnum.NGLSystemMaint) { }

        #endregion

        #region " Properties"

        /// <summary> This property is used for logging and error tracking. </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NGLSysModuleController";
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
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.LicenseModuleBLL oBLL = new BLL.LicenseModuleBLL(Parameters);
                int count = 0;
                DModel.SelectableGridItem[] oData = new DModel.SelectableGridItem[] { };
                oData = oBLL.GetListOfModules();
                if (oData?.Count() > 0) { count = oData.Count(); }
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

        //Not Currently Used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        //Not Currently Used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        //Not Currently Used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                ////if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "FormConfigGridFltr"); } //save the page filter for the next time the page loads
                ////DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                ////int RecordCount = 0;
                ////int count = 0;
                ////DModel.SecurityTypeConfig[] oData = new DModel.SecurityTypeConfig[] { };
                ////oData = NGLSecurityData.GetFormSecTypeXTabData(ref RecordCount, f);
                ////if (oData?.Count() > 0)
                ////{
                ////    count = oData.Count();
                ////    if (RecordCount > count) { count = RecordCount; }
                ////}
                ////response = new Models.Response(oData, count);
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
        public Models.Response Post([FromBody]DModel.SelectableGridSave data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.LicenseModuleBLL oBLL = new BLL.LicenseModuleBLL(Parameters);
                long key = oBLL.GenerateModuleLicenceKey(data);
                long[] oRecords = new long[] { key };
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

        //Not Currently Used
        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                //bool blnRet = oDAL.DeleteBookLoadBoard(id);
                //bool[] oRecords = new bool[1];
                //oRecords[0] = blnRet;
                //response = new Models.Response(oRecords, 1);
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