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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class CarrierSchedulerGroupedController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public CarrierSchedulerGroupedController()
                : base(Utilities.PageEnum.CarrierSchedulerGrouped)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CarrierSchedulerGroupedController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.AMSCarrierGroupedHeader selectModelData(LTS.spGetAMSCarrierPickNeedApptGroupedResult d)
        {
            Models.AMSCarrierGroupedHeader modelRecord = new Models.AMSCarrierGroupedHeader();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierGroupedHeader)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.AMSCarrierGroupedHeader selectModelData(LTS.spGetAMSCarrierDelNeedApptGroupedResult d)
        {
            Models.AMSCarrierGroupedHeader modelRecord = new Models.AMSCarrierGroupedHeader();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierGroupedHeader)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.AMSCarrierGroupedHeader selectModelData(LTS.spGetAMSCarrierPickBookedApptGroupedResult d)
        {
            Models.AMSCarrierGroupedHeader modelRecord = new Models.AMSCarrierGroupedHeader();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierGroupedHeader)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.AMSCarrierGroupedHeader selectModelData(LTS.spGetAMSCarrierDelBookedApptGroupedResult d)
        {
            Models.AMSCarrierGroupedHeader modelRecord = new Models.AMSCarrierGroupedHeader();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierGroupedHeader)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.AMSCarrierOrdersChart selectModelData(LTS.spGetCarrierOrderSummaryForChartGroupedResult d)
        {
            Models.AMSCarrierOrdersChart modelRecord = new Models.AMSCarrierOrdersChart();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierOrdersChart)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.AMSCarrierOrders selectModelData(LTS.vAMSCarrierPickNeedAppt d)
        {
            Models.AMSCarrierOrders modelRecord = new Models.AMSCarrierOrders();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierOrders)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.AMSCarrierOrders selectModelData(LTS.vAMSCarrierDelNeedAppt d)
        {
            Models.AMSCarrierOrders modelRecord = new Models.AMSCarrierOrders();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierOrders)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.AMSCarrierOrders selectModelData(LTS.vAMSCarrierPickBookedAppt d)
        {
            Models.AMSCarrierOrders modelRecord = new Models.AMSCarrierOrders();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierOrders)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.AMSCarrierOrders selectModelData(LTS.vAMSCarrierDelBookedAppt d)
        {
            Models.AMSCarrierOrders modelRecord = new Models.AMSCarrierOrders();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.AMSCarrierOrders)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
            }
            return modelRecord;
        }


        #endregion

        #region " REST Services"


        //** Get Methods for the 4 main grids on the Carrier Scheduler Grouped page **//

        /// <summary>
        /// Gets the records for the Unscheduled Orders Pickup grid on the Carrier Scheduler Grouped page
        /// </summary>
        [HttpGet, ActionName("GetAMSCarrierUOPickGrouped")]
        public Models.Response GetAMSCarrierUOPickGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AMSCarrierGroupedHeader[] records = new Models.AMSCarrierGroupedHeader[] { };
                int count = 0;
                int RecordCount = 0;         
                LTS.spGetAMSCarrierPickNeedApptGroupedResult[] oData = NGLAMSAppointmentData.GetAMSCarrierPickNeedApptGrouped(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        /// <summary>
        /// Gets the records for the Unscheduled Orders Delivery grid on the Carrier Scheduler Grouped page
        /// </summary>
        [HttpGet, ActionName("GetAMSCarrierUODelGrouped")]
        public Models.Response GetAMSCarrierUODelGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AMSCarrierGroupedHeader[] records = new Models.AMSCarrierGroupedHeader[] { };
                int count = 0;
                int RecordCount = 0;
                LTS.spGetAMSCarrierDelNeedApptGroupedResult[] oData = NGLAMSAppointmentData.GetAMSCarrierDelNeedApptGrouped(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        /// <summary>
        /// Gets the records for the Booked Appts Pickup grid on the Carrier Scheduler Grouped page
        /// </summary>
        [HttpGet, ActionName("GetAMSCarrierBAPickGrouped")]
        public Models.Response GetAMSCarrierBAPickGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AMSCarrierGroupedHeader[] records = new Models.AMSCarrierGroupedHeader[] { };
                int count = 0;
                int RecordCount = 0;
                LTS.spGetAMSCarrierPickBookedApptGroupedResult[] oData = NGLAMSAppointmentData.GetAMSCarrierPickBookedApptGrouped(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        /// <summary>
        /// Gets the records for the Booked Appts Delivery grid on the Carrier Scheduler Grouped page
        /// </summary>
        [HttpGet, ActionName("GetAMSCarrierBADelGrouped")]
        public Models.Response GetAMSCarrierBADelGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AMSCarrierGroupedHeader[] records = new Models.AMSCarrierGroupedHeader[] { };
                int count = 0;
                int RecordCount = 0;
                LTS.spGetAMSCarrierDelBookedApptGroupedResult[] oData = NGLAMSAppointmentData.GetAMSCarrierDelBookedApptGrouped(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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


        //** Get Methods for the 4 detail grids on the Carrier Scheduler Grouped page **//

        /// <summary>
        /// Gets the detail records for the Unscheduled Orders Pickup grid on the Carrier Scheduler Grouped page
        /// </summary>
        [HttpGet, ActionName("GetUOPickDetailsGrouped")]
        public Models.Response GetUOPickDetailsGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                Models.GenericResult gr = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                string SHID = gr.strField;
                string Warehouse = gr.strField2;
                int BookOrigCompControl = gr.intField1;
                DateTime? BookDateLoad = gr.dtField;
                Models.AMSCarrierOrders[] records = new Models.AMSCarrierOrders[] { };
                int count = 0;
                LTS.vAMSCarrierPickNeedAppt[] oData = NGLAMSAppointmentData.GetAMSCarrierPickNeedApptBySHID(SHID, BookOrigCompControl, BookDateLoad);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        /// <summary>
        /// Gets the detail records for the Unscheduled Orders Delivery grid on the Carrier Scheduler Grouped page
        /// </summary>
        [HttpGet, ActionName("GetUODelDetailsGrouped")]
        public Models.Response GetUODelDetailsGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                Models.GenericResult gr = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                string SHID = gr.strField;
                string Warehouse = gr.strField2;
                int BookDestCompControl = gr.intField1;
                DateTime? BookDateRequired = gr.dtField;
                Models.AMSCarrierOrders[] records = new Models.AMSCarrierOrders[] { };
                int count = 0;
                LTS.vAMSCarrierDelNeedAppt[] oData = NGLAMSAppointmentData.GetAMSCarrierDelNeedApptBySHID(SHID, BookDestCompControl, BookDateRequired);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        /// <summary>
        /// Gets the detail records for the Booked Appts Pickup grid on the Carrier Scheduler Grouped page
        /// </summary>
        [HttpGet, ActionName("GetBAPickDetailsGrouped")]
        public Models.Response GetBAPickDetailsGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                Models.GenericResult gr = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                string SHID = gr.strField;
                string Warehouse = gr.strField2;
                int BookOrigCompControl = gr.intField1;
                DateTime? BookDateLoad = gr.dtField;
                Models.AMSCarrierOrders[] records = new Models.AMSCarrierOrders[] { };
                int count = 0;
                LTS.vAMSCarrierPickBookedAppt[] oData = NGLAMSAppointmentData.GetAMSCarrierPickBookedApptBySHID(SHID, BookOrigCompControl, BookDateLoad);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        /// <summary>
        /// Gets the detail records for the Booked Appts Delivery grid on the Carrier Scheduler Grouped page
        /// </summary>
        [HttpGet, ActionName("GetBADelDetailsGrouped")]
        public Models.Response GetBADelDetailsGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                Models.GenericResult gr = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                string SHID = gr.strField;
                string Warehouse = gr.strField2;
                int BookDestCompControl = gr.intField1;
                DateTime? BookDateRequired = gr.dtField;
                Models.AMSCarrierOrders[] records = new Models.AMSCarrierOrders[] { };
                int count = 0;
                LTS.vAMSCarrierDelBookedAppt[] oData = NGLAMSAppointmentData.GetAMSCarrierDelBookedApptBySHID(SHID, BookDestCompControl, BookDateRequired);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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


        //** Post Method to Save Equipment ID on the Carrier Scheduler Grouped page **//

        /// <summary>
        /// This is used to save the EquipID and return result
        /// </summary>
        [HttpPost, ActionName("SaveEquipmentIDForOrderGrouped")]
        public Models.Response SaveEquipmentIDForOrderGrouped([System.Web.Http.FromBody]Models.UpdateEquipIDCSG data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            try
            {
                if (data != null)
                {
                    int CompControl;
                    DateTime LoadDate;
                    DateTime RequiredDate;
                    bool Inbound;
                    bool IsTransfer;
                    int BookControl;
                    if (data.equipIDValidation.IsPickup)
                    {
                        LTS.vAMSCarrierPickNeedAppt[] d = NGLAMSAppointmentData.GetAMSCarrierPickNeedApptBySHID(data.SHID);
                        if (d == null || d.Count() < 1 || d[0].BookControl == 0)
                        {
                            NGLAMSAppointmentData.throwNoDataFaultException();
                        }
                        CompControl = d[0].BookCustCompControl;
                        LoadDate = d[0].BookDateLoad.Value;
                        RequiredDate = d[0].BookDateRequired.Value;
                        Inbound = d[0].Inbound.Value;
                        IsTransfer = d[0].IsTransfer.Value;
                        BookControl = d[0].BookControl;                    
                    }
                    else
                    {
                        LTS.vAMSCarrierDelNeedAppt[] d = NGLAMSAppointmentData.GetAMSCarrierDelNeedApptBySHID(data.SHID);
                        if (d == null || d.Count() < 1 || d[0].BookControl == 0)
                        {
                            NGLAMSAppointmentData.throwNoDataFaultException();
                        }
                        CompControl = d[0].BookCustCompControl;
                        LoadDate = d[0].BookDateLoad.Value;
                        RequiredDate = d[0].BookDateRequired.Value;
                        Inbound = d[0].Inbound.Value;
                        IsTransfer = d[0].IsTransfer.Value;
                        BookControl = d[0].BookControl;                                           
                    }
                    DModel.EquipIDValidation oData = data.equipIDValidation;
                    oData.BookControl = BookControl; //we have to set this here because we no longer have this value in the grid record
                    NGLBookData.SaveEquipmentID(ref oData, CompControl, LoadDate, RequiredDate, Inbound, IsTransfer);
                    DModel.EquipIDValidation[] eVal = new DModel.EquipIDValidation[] { oData };
                    response = new Models.Response(eVal, 1);
                }
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


        //** Methods for the "View Available Appts" popup window for the Unscheduled Orders grids on the Carrier Scheduler Grouped page **//

        /// <summary>
        /// View available appointment time slots for the selected Unscheduled Orders record on the Carrier Scheduler Grouped page
        /// </summary>
        /// <param name="filter">
        /// filter.strField = SHID
        /// filter.blnField = blnIsPickup
        /// filter.strField2 = Warehouse
        /// filter.intField1 = BookOrigCompControl/BookDestCompControl (based on blnIsPickup)
        /// filter.dtField = BookDateLoad/BookDateRequired             (based on blnIsPickup)
        /// </param>
        /// <returns></returns>
        [HttpGet, ActionName("GetCarAvailableApptsUOGrouped")]
        public Models.Response GetCarAvailableApptsUOGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                Models.GenericResult gr = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                if (gr != null)
                {
                    bool blnIsPickup = gr.blnField;
                    string SHID = gr.strField;
                    string pWarehouse = gr.strField2;
                    int pWarehouseControl = gr.intField1;
                    DateTime? dt = gr.dtField;

                    int BookControl, CarrierControl, CompControl, CarrierNumber;
                    DateTime LoadDate, RequiredDate;
                    DateTime? ScheduledDate, ScheduledTime;
                    bool Inbound;
                    string EquipID, Warehouse, CarrierName;

                    if (blnIsPickup)
                    {
                        LTS.vAMSCarrierPickNeedAppt[] d = NGLAMSAppointmentData.GetAMSCarrierPickNeedApptBySHID(SHID, pWarehouseControl, dt);
                        if (d == null || d.Count() < 1 || d[0].BookControl == 0)
                        {
                            NGLAMSAppointmentData.throwNoDataFaultException();
                        }
                        BookControl = d[0].BookControl;
                        CarrierControl = d[0].BookCarrierControl;
                        //CompControl = d[0].BookCustCompControl;
                        CompControl = d[0].BookOrigCompControl.Value;
                        LoadDate = d[0].BookDateLoad.Value;
                        RequiredDate = d[0].BookDateRequired.Value;
                        EquipID = d[0].BookCarrTrailerNo;
                        Inbound = d[0].Inbound.Value;
                        Warehouse = d[0].Warehouse;
                        CarrierName = d[0].CarrierName;
                        CarrierNumber = d[0].CarrierNumber.Value;
                        ScheduledDate = d[0].ScheduledDate;
                        ScheduledTime = d[0].ScheduledTime;
                    }
                    else
                    {
                        LTS.vAMSCarrierDelNeedAppt[] d = NGLAMSAppointmentData.GetAMSCarrierDelNeedApptBySHID(SHID, pWarehouseControl, dt);
                        if (d == null || d.Count() < 1 || d[0].BookControl == 0)
                        {
                            NGLAMSAppointmentData.throwNoDataFaultException();
                        }
                        BookControl = d[0].BookControl;
                        CarrierControl = d[0].BookCarrierControl;
                        //CompControl = d[0].BookCustCompControl;
                        CompControl = d[0].BookDestCompControl.Value;
                        LoadDate = d[0].BookDateLoad.Value;
                        RequiredDate = d[0].BookDateRequired.Value;
                        EquipID = d[0].BookCarrTrailerNo;
                        Inbound = d[0].Inbound.Value;
                        Warehouse = d[0].Warehouse;
                        CarrierName = d[0].CarrierName;
                        CarrierNumber = d[0].CarrierNumber.Value;
                        ScheduledDate = d[0].ScheduledDate;
                        ScheduledTime = d[0].ScheduledTime;
                    }
                    //DModel.AMSCarrierResults oTData = NGLDockSettingData.GetCarrierAvailableAppointmentsEXPERIMENT(SHID, pWarehouse, blnIsPickup);
                    DModel.AMSCarrierResults oTData = NGLDockSettingData.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID);
                    if (oTData == null )
                    {
                        NGLAMSAppointmentData.throwNoDataFaultException();
                    }
                    if (!oTData.blnMustRequestAppt)
                    {
                        var retVals = oTData.AvailableSlots.ToArray();
                        response = new Models.Response(retVals, retVals.Length);
                    }
                    else
                    {
                        List<DModel.AMSCarrierResults> oApptRecords = new List<DModel.AMSCarrierResults>();
                        oApptRecords.Add(oTData);
                        response = new Models.Response(oApptRecords.ToArray(), oApptRecords.ToArray().Count());
                    }
                }
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

        [HttpGet, ActionName("GetCarAvailableApptsUOToken")]
        public Models.Response GetCarAvailableApptsUOToken(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();           
            try
            {
                if (string.IsNullOrWhiteSpace(filter) )
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = "There was a problem with the data or you are not authorized to execute this action. Please use the contact information on the page for support.";
                    return response;
                }
                

                Models.GenericResult gr = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                if (gr != null)
                {
                    DAL.NGLtblServiceTokenData oTokenDAL = new DAL.NGLtblServiceTokenData(Utilities.DALWCFParameters);
                    DAL.Models.CarrierBookApptWithTokenData oTokenData = oTokenDAL.CarrierBookApptWithTokenData(gr.sToken);
                    if (oTokenData == null)
                    {
                        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                        response.Errors = "There was a problem with the data or you are not authorized to execute this action. Please use the contact information on the page for support.";
                        return response;
                    }
                    if (oTokenData.BookControl == 0)
                    {
                        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                        response.Errors = string.Format("Your Token is not valid.\n<p> please email {0} or call {1} for support.  </ p>", oTokenData.LaneCarrierBookApptviaTokenFailEmail, oTokenData.LaneCarrierBookApptviaTokenFailPhone);
                        return response;
                    }
                    if (oTokenData.ExpirationDate < DateTime.Now)
                    {
                        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                        response.Errors = string.Format("Your Token has expired.\n<p> please email {0} or call {1} for support.  </ p>", oTokenData.LaneCarrierBookApptviaTokenFailEmail, oTokenData.LaneCarrierBookApptviaTokenFailPhone);
                        return response;
                    }
                    this.Parameters.UserCarrierControl = gr.intField2;
                    bool blnIsPickup = gr.blnField;
                    string SHID = gr.strField;
                    string pWarehouse = gr.strField2;
                    int pWarehouseControl = gr.intField1;
                    DateTime? dt = gr.dtField;

                    int BookControl, CarrierControl, CompControl, CarrierNumber;
                    DateTime LoadDate, RequiredDate;
                    DateTime? ScheduledDate, ScheduledTime;
                    bool Inbound;
                    string EquipID, Warehouse, CarrierName;

                    if (blnIsPickup)
                    {
                        LTS.vAMSCarrierPickNeedAppt[] d = NGLAMSAppointmentData.GetAMSCarrierPickNeedApptBySHID(SHID, pWarehouseControl, dt);
                        if (d == null || d.Count() < 1 || d[0].BookControl == 0)
                        {
                            response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                            response.Errors = string.Format("An appointmet is not available or it has already been booked.\n<p> please email {0} or call {1} for support.  </ p>", oTokenData.LaneCarrierBookApptviaTokenFailEmail, oTokenData.LaneCarrierBookApptviaTokenFailPhone);
                            return response;
                           // NGLAMSAppointmentData.throwNoDataFaultException();
                        }
                        BookControl = d[0].BookControl;
                        CarrierControl = d[0].BookCarrierControl;
                        //CompControl = d[0].BookCustCompControl;
                        CompControl = d[0].BookOrigCompControl.Value;
                        LoadDate = d[0].BookDateLoad.Value;
                        RequiredDate = d[0].BookDateRequired.Value;
                        EquipID = d[0].BookCarrTrailerNo;
                        Inbound = d[0].Inbound.Value;
                        Warehouse = d[0].Warehouse;
                        CarrierName = d[0].CarrierName;
                        CarrierNumber = d[0].CarrierNumber.Value;
                        ScheduledDate = d[0].ScheduledDate;
                        ScheduledTime = d[0].ScheduledTime;
                    }
                    else
                    {
                        LTS.vAMSCarrierDelNeedAppt[] d = NGLAMSAppointmentData.GetAMSCarrierDelNeedApptBySHID(SHID, pWarehouseControl, dt);
                        if (d == null || d.Count() < 1 || d[0].BookControl == 0)
                        {
                            response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                            response.Errors = string.Format("An appointmet is not available or it has already been booked.\n<p> please email {0} or call {1} for support.  </ p>", oTokenData.LaneCarrierBookApptviaTokenFailEmail, oTokenData.LaneCarrierBookApptviaTokenFailPhone);
                            return response;

                            //NGLAMSAppointmentData.throwNoDataFaultException();
                        }
                        BookControl = d[0].BookControl;
                        CarrierControl = d[0].BookCarrierControl;
                        //CompControl = d[0].BookCustCompControl;
                        CompControl = d[0].BookDestCompControl.Value;
                        LoadDate = d[0].BookDateLoad.Value;
                        RequiredDate = d[0].BookDateRequired.Value;
                        EquipID = d[0].BookCarrTrailerNo;
                        Inbound = d[0].Inbound.Value;
                        Warehouse = d[0].Warehouse;
                        CarrierName = d[0].CarrierName;
                        CarrierNumber = d[0].CarrierNumber.Value;
                        ScheduledDate = d[0].ScheduledDate;
                        ScheduledTime = d[0].ScheduledTime;
                    }
                    //DModel.AMSCarrierResults oTData = NGLDockSettingData.GetCarrierAvailableAppointmentsEXPERIMENT(SHID, pWarehouse, blnIsPickup);
                    DModel.AMSCarrierResults oTData = NGLDockSettingData.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID);
                    if (oTData == null)
                    {
                        NGLAMSAppointmentData.throwNoDataFaultException();
                    }
                    if (!oTData.blnMustRequestAppt)
                    {
                        var retVals = oTData.AvailableSlots.ToArray();
                        response = new Models.Response(retVals, retVals.Length);
                    }
                    else
                    {
                        List<DModel.AMSCarrierResults> oApptRecords = new List<DModel.AMSCarrierResults>();
                        oApptRecords.Add(oTData);
                        response = new Models.Response(oApptRecords.ToArray(), oApptRecords.ToArray().Count());
                    }
                }
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
        /// Schedules an appoinment (from the view available appts grid) for the Unscheduled Order on the Carrier Scheduler Grouped page
        /// </summary>
        /// <param name="data"></param>
        [HttpPost, ActionName("CarrierScheduleApptForUOGrouped")]
        public Models.Response CarrierScheduleApptForUOGrouped([System.Web.Http.FromBody]DModel.AMSCarrierAvailableSlots data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (data == null || string.IsNullOrWhiteSpace(data.Books))
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = "There was a problem with the data. Please refresh the data and try again. If this does not fix the problem, please contact technical support.";
                    return response;
                }
                //string resp = NGLAMSAppointmentData.BookCarrierAppointment(data);
                string resp = NGLAMSAppointmentData.CarrierAutomationCreateNewAppointment(data); //Modified By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
                if (string.IsNullOrWhiteSpace(resp))
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = resp;
                }
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
        /// Schedules an appoinment (from the view available appts grid) for the Unscheduled Order on the Carrier Scheduler Grouped page
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>
        /// Modified by RHR for v-8.4.0.003 on 11/2/2021
        /// </remarks>
        [HttpPost, ActionName("CarrierScheduleApptForUOGroupedToken")]
        public Models.Response CarrierScheduleApptForUOGroupedToken([System.Web.Http.FromBody] Models.AMSCarrierAvailableSlotsToken tdata)
        {
            var response = new Models.Response();
            try
            {
                if (tdata == null || string.IsNullOrWhiteSpace(tdata.Books) || string.IsNullOrWhiteSpace(tdata.sToken))
                {                  
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = "There was a problem with the data or you are not authorized to execute this action. Please use the contact information on the page for support.";
                    return response;
                }
                DAL.NGLtblServiceTokenData oTokenDAL = new DAL.NGLtblServiceTokenData(Utilities.DALWCFParameters);
                DAL.Models.CarrierBookApptWithTokenData oTokenData = oTokenDAL.CarrierBookApptWithTokenData(tdata.sToken);
                if (oTokenData == null )
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = "There was a problem with the data or you are not authorized to execute this action. Please use the contact information on the page for support.";
                    return response;
                }
                if (oTokenData.BookControl == 0)
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Your Token is not valid.\n<p> please email {0} or call {1} for support.  </ p>", oTokenData.LaneCarrierBookApptviaTokenFailEmail, oTokenData.LaneCarrierBookApptviaTokenFailPhone);
                    return response;
                }
                if (oTokenData.ExpirationDate < DateTime.Now)
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Your Token has expired.\n<p> please email {0} or call {1} for support.  </ p>", oTokenData.LaneCarrierBookApptviaTokenFailEmail, oTokenData.LaneCarrierBookApptviaTokenFailPhone);
                    return response;
                }
                DModel.AMSCarrierAvailableSlots data = new DModel.AMSCarrierAvailableSlots
                {
                    ApptControl = tdata.ApptControl,
                    Date = tdata.Date,
                    StartTime = tdata.StartTime,
                    EndTime = tdata.EndTime,
                    Docks = tdata.Docks,
                    Warehouse = tdata.Warehouse,
                    Books = tdata.Books,
                    CarrierNumber = tdata.CarrierNumber,
                    CarrierName = tdata.CarrierName,
                    CompControl = tdata.CompControl,
                    CarrierControl = tdata.CarrierControl
                };
                
                //string resp = NGLAMSAppointmentData.BookCarrierAppointment(data);
                DModel.ResultObject resp = NGLAMSAppointmentData.CarrierAutomationCreateNewAppointmentWReport(data); //Modified By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
                
                if (resp == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = "Create Appointment Failure.  Please contact technical support";
                }
                
                if (resp.Success)
                {
                    response.Messages = resp.SuccessMsg;
                    response.Data = new bool[1] { true };
                }
                else
                {
                    response.Data = new bool[1] { false };
                    response.Messages = resp.ErrMsg;
                }
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


        //** Methods called from the Booked Appointments grids on the Carrier Scheduler Grouped page **//

        /// <summary>
        /// Called from the Booked Appointments grids on the Carrier Scheduler Grouped page
        /// Checks to see if the user has the correct permissions to either Delete or Edit a previously scheduled appointment. 
        /// The method either allows the user to perform the action, or gives them the option to submit a Change/Delete request
        /// </summary>
        /// <param name="filter">
        /// filter.strField = SHID
        /// filter.blnField = blnIsPickup
        /// filter.blnField1 = blnIsDelete
        /// </param>
        /// <returns>Gets single/list of records</returns>
        [HttpGet, ActionName("GetModifyOptionCarrierBAGrouped")]
        public Models.Response GetModifyOptionCarrierBAGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {              
                Models.GenericResult gr = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                if (gr == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = "There was a problem with the data. Please refresh the data and try again. If this does not fix the problem, please contact technical support.";
                    return response;
                }
                string SHID = gr.strField;
                bool blnIsPickup = gr.blnField;
                bool blnIsDelete = gr.blnField1;
                string pWarehouse = gr.strField2;
                int pWarehouseControl = gr.intField1;
                DateTime? dt = gr.dtField;

                int BookControl, CarrierControl, CompControl, CarrierNumber, AMSPickApptControl, AMSDelApptControl;
                DateTime LoadDate, RequiredDate;
                DateTime? ScheduledDate, ScheduledTime;
                bool Inbound;
                string EquipID, Warehouse, CarrierName;

                if (blnIsPickup)
                {
                    LTS.vAMSCarrierPickBookedAppt[] d = NGLAMSAppointmentData.GetAMSCarrierPickBookedApptBySHID(SHID, pWarehouseControl, dt);
                    if (d == null || d.Count() < 1 || d[0].BookControl == 0)
                    {
                        NGLAMSAppointmentData.throwNoDataFaultException();
                    }
                    BookControl = d[0].BookControl;
                    CarrierControl = d[0].BookCarrierControl;
                    //CompControl = d[0].BookCustCompControl;
                    CompControl = d[0].BookOrigCompControl.Value;
                    LoadDate = d[0].BookDateLoad.Value;
                    RequiredDate = d[0].BookDateRequired.Value;
                    EquipID = d[0].BookCarrTrailerNo;
                    Inbound = d[0].Inbound.Value;
                    Warehouse = d[0].Warehouse;
                    CarrierName = d[0].CarrierName;
                    CarrierNumber = d[0].CarrierNumber.Value;
                    ScheduledDate = d[0].ScheduledDate;
                    ScheduledTime = d[0].ScheduledTime;
                    AMSPickApptControl = d[0].BookAMSPickupApptControl;
                    AMSDelApptControl = d[0].BookAMSDeliveryApptControl;
                }
                else
                {
                    LTS.vAMSCarrierDelBookedAppt[] d = NGLAMSAppointmentData.GetAMSCarrierDelBookedApptBySHID(SHID, pWarehouseControl, dt);
                    if (d == null || d.Count() < 1 || d[0].BookControl == 0)
                    {
                        NGLAMSAppointmentData.throwNoDataFaultException();
                    }
                    BookControl = d[0].BookControl;
                    CarrierControl = d[0].BookCarrierControl;
                    //CompControl = d[0].BookCustCompControl;
                    CompControl = d[0].BookDestCompControl.Value;
                    LoadDate = d[0].BookDateLoad.Value;
                    RequiredDate = d[0].BookDateRequired.Value;
                    EquipID = d[0].BookCarrTrailerNo;
                    Inbound = d[0].Inbound.Value;
                    Warehouse = d[0].Warehouse;
                    CarrierName = d[0].CarrierName;
                    CarrierNumber = d[0].CarrierNumber.Value;
                    ScheduledDate = d[0].ScheduledDate;
                    ScheduledTime = d[0].ScheduledTime;
                    AMSPickApptControl = d[0].BookAMSPickupApptControl;
                    AMSDelApptControl = d[0].BookAMSDeliveryApptControl;                   
                }
                DModel.AMSCarrierResults CAppts = NGLDockSettingData.ModifyCarrierBookedAppointment(CarrierControl, CompControl, BookControl, SHID, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, blnIsDelete, AMSPickApptControl, AMSDelApptControl, CarrierNumber);
                if (CAppts == null)
                {
                    NGLAMSAppointmentData.throwNoDataFaultException();
                }
                if (!CAppts.blnMustRequestAppt && CAppts.AvailableSlots != null)
                {
                    var retVals = CAppts.AvailableSlots.ToArray();
                    response = new Models.Response(retVals, retVals.Length);
                }
                else
                {
                    List<DModel.AMSCarrierResults> oApptRecords = new List<DModel.AMSCarrierResults>();
                    oApptRecords.Add(CAppts);
                    response = new Models.Response(oApptRecords.ToArray(), oApptRecords.ToArray().Count());
                }
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
        /// Attempts to update the Booked Appointment's dock/times on the Carrier Scheduler Grouped page
        /// </summary>
        /// <param name="filter">The testMstructsegment.</param>
        /// <returns>deleted Single record</returns>        
        [HttpPost, ActionName("UpdateCarrierBookedAppointmentGrouped")]
        public Models.Response UpdateCarrierBookedAppointmentGrouped([System.Web.Http.FromBody]Models.AMSCarrierBAWrapper w)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            try
            {
                if (w == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Errors = "There was a problem with the data. Please refresh the data and try again. If this does not fix the problem, please contact technical support.";
                    return response;
                }
                string strRes = "";
                int bookControl = 0;
                DModel.AMSCarrierAvailableSlots data = w.AMSCarrierAvailableSlots;
                bool blnIsPickup = w.blnIsPickup;
                //We have to get this from the db because the caller no longer has access to the fields BookAMSPickupApptControl or BookAMSDeliveryApptControl
                var bookControls = data.Books.Split(',');
                int.TryParse(bookControls[0], out bookControl);
                var b = NGLBookData.GetBookFilteredNoChildren(bookControl);
                //if (b.BookAMSPickupApptControl != 0) { data.ApptControl = b.BookAMSPickupApptControl; } else if (b.BookAMSDeliveryApptControl != 0) { data.ApptControl = b.BookAMSDeliveryApptControl; } else { data.ApptControl = 0; }
                data.ApptControl = 0;
                if (blnIsPickup) { data.ApptControl = b.BookAMSPickupApptControl; } else { data.ApptControl = b.BookAMSDeliveryApptControl; }
                //if (data.ApptControl != 0) { strRes = NGLAMSAppointmentData.UpdateCarrierBookedAppointment(data); } else { strRes = "Appointment Control cannot be 0"; }
                if (data.ApptControl != 0) { strRes = NGLAMSAppointmentData.CarrierAutomationUpdateExistingAppointment(data); } else { strRes = "Appointment Control cannot be 0"; } //Modified By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
                if (string.IsNullOrWhiteSpace(strRes))
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = strRes;
                }
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



        //** Get Method for the Summary Charts on the Carrier Scheduler Grouped page **//

        /// <summary>
        /// This is used to return a collection of orders count for different dates
        /// </summary>
        /// <param name="filter">The filter.</param>
        [HttpGet, ActionName("GetCarrierOrderSummaryForChartGrouped")]
        public Models.Response GetCarrierOrderSummaryForChartGrouped(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AMSCarrierOrdersChart[] records = new Models.AMSCarrierOrdersChart[] { };
                int count = 0;
                LTS.spGetCarrierOrderSummaryForChartGroupedResult[] oData = NGLCarrierData.GetCarrierOrderSummaryForChartGrouped(Parameters.UserCarrierControl, f.take);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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


        #endregion


    }
}