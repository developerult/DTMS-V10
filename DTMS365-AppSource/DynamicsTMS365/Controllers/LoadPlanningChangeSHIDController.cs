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
    public class LoadPlanningChangeSHIDController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 12/03/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LoadPlanningChangeSHIDController()
                : base(Utilities.PageEnum.LoadPlanningChangeSHID)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadPlanningChangeSHIDController";
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

                //The Load Board Rate It does not currently have any data to Readbeed to read
                // the load board data so for now we just return the parent id
                //in a vLoadBoardChangeSHID record
                //if more data is needed in the future we should read the records
                //from the database
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                LTS.vLoadBoardChangeSHID[] oRecords = new LTS.vLoadBoardChangeSHID[1];
                oRecords[0] = oDAL.GetBookLoadBoardChangeSHID(id);
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
        /// Gets All the Child Records filtered by parent id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/17/2018   
        /// </remarks>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {

            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (id == 0)
                {
                    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);

                }
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                LTS.vLoadBoardChangeSHID[] oRecords = new LTS.vLoadBoardChangeSHID[1];
                oRecords[0] = oDAL.GetBookLoadBoardChangeSHID(id);
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

                int id = 0;
                int.TryParse(filter, out id);
                if (id == 0)
                {
                    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                }
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                LTS.vLoadBoardChangeSHID[] oRecords = new LTS.vLoadBoardChangeSHID[1];
                oRecords[0] = oDAL.GetBookLoadBoardChangeSHID(id);
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 12/17/2018
        ///  we now call the correct BookRevBLL.GenerateQuote based on the selected switch
        ///  added logic to process wcfResponse Message, Warning, and Error dictionary messages
        /// </remarks>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody] LTS.vLoadBoardChangeSHID data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.GenericResult oGResult = new Models.GenericResult() { Control = 0, strField = "undefined" };
                if (data == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = Utilities.getLocalizedMsg("E_CannotSaveNoData");
                    response.Data = new Models.GenericResult[] { oGResult };
                    response.Count = 1;
                    return response;
                }
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                bool blnRet = oDAL.UpdateBookLoadBoardSHID(data);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
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
                ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                ////bool blnRet = oDAL.DeleteBookLoadBoard(id);
                bool[] oRecords = new bool[1];

                ////oRecords[0] = blnRet;

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


        #region "Private Functions"

        /// <summary>
        /// Calculates the spot rate details and creates the load board bid data. returns the bid control of the inserted record  and prepares a response object to return
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="data"></param>
        /// <param name="iBidControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 12/04/2018
        /// </remarks>
        //private Models.Response executeDoSpotRate(int pk, LTS.vLoadBoardChangeSHID data, ref int iBidControl)
        //{
        //    var response = new Models.Response(); //new HttpResponseMessage();           
        //    try
        //    {
        //        BLL.NGLBookRevenueBLL oBLL = new BLL.NGLBookRevenueBLL(this.Parameters);
        //        iBidControl = oBLL.InsertLoadTenderSpotRate(pk);

        //        int count = 1;
        //        int[] aBidControls = new int[] { iBidControl };
        //        response = new Models.Response(aBidControls, count);
        //        response.StatusCode = HttpStatusCode.OK;

        //    }
        //    catch (Exception ex)
        //    {
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    return response;
        //}


        #endregion
    }
}