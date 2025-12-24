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
    public class BookRevTariffFeesController : NGLControllerBase
    {


        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public BookRevTariffFeesController()
                : base(Utilities.PageEnum.LoadBoardRevenueFees)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookRevTariffFeesController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
        // not needed
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

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {

                Models.vBookFee[] fees = new Models.vBookFee[1];
                //LTS.vBookFee[] fees = new LTS.vBookFee[1];
                DAL.NGLBookFeeData oBookFeesDAL = new DAL.NGLBookFeeData(Parameters);                
                fees[0] = Models.vBookFee.selectModelData(oBookFeesDAL.GetvBookFee(id));
                response = new Models.Response(fees, 1);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
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
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {

                //Modified by RHR for v-8.5.4.004 on 12/06/2023 new BookControl setting
                //int iBookControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                LTS.vBookFee[] fees = new LTS.vBookFee[] { };
                int iBookControl = 0;
                iBookControl = readBookControlPageSetting(iBookControl);
                if (iBookControl == 0)
                {
                    return new Models.Response(fees, 0);
                }

                DAL.NGLBookFeeData oBookFeesDAL = new DAL.NGLBookFeeData(Parameters);
                int count = 0;
                fees = oBookFeesDAL.GetvBookFeesForTariff( iBookControl);
                Models.vBookFee[] records = new Models.vBookFee[] { };

                if (fees != null && fees.Length > 0)
                {
                    count = fees.Length;
                    records = (from e in fees
                               orderby e.BFCaption ascending
                               select Models.vBookFee.selectModelData(e)).ToArray();

                }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody] Models.vBookFee data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            string sPageTitle = "";
            try
            {
                BLL.NGLBookFeesBLL oBLL = new BLL.NGLBookFeesBLL(Parameters);
                bool[] oRecords = new bool[1];
                DTO.CarrierCostResults oData = new DTO.CarrierCostResults();
                oData.Success = true;
                DTO.BookFee dtoData = Models.vBookFee.selectDTOData(data);
                sPageTitle = "Update Tariff Specific Fee";
                oData = oBLL.UpdateBookFeeD365(dtoData);
                response.Data = new bool[] { oData.Success };
                response.Count = 1;
                Utilities.addCarrierCostResultMessagesToResponse(ref response, ref oData, sPageTitle);
              
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


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLBookFeesBLL oBLL = new BLL.NGLBookFeesBLL(Parameters);
                oBLL.DeleteBookFee(id);
                //returns true if no errors
                bool[] oRecords = new bool[1];
                oRecords[0] = true;
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


        #region " public methods"


        #endregion

    }
}