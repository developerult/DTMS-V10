using Ngl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.Controllers
{       /// <summary>
        /// AMSOrderController for All Orders Rest API Controls
        /// </summary>
        /// <remarks>
        /// Created By SK on 29-05-18
        /// </remarks>
    public class AMSOrderController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.AMSOrderController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        /// <value>The source class.</value>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }
        /// <summary>
        /// The request
        /// </summary>
        HttpRequest request = HttpContext.Current.Request;
        #endregion

        #region " Data Translation"

        /// <summary>
        /// Selecting a Appointments Model data by passing table records
        /// </summary>
        /// <param name="d">The table data.</param>
        /// <returns>Returns Appointments</returns>
        private Models.AMSOrder selectModelData(DTO.AMSOrderList d)
        {
            Models.AMSOrder modelOrders = new Models.AMSOrder();
            List<string> skipOrders = new List<string> { "BookUpdated" };
            string sMsg = "";
            modelOrders = (Models.AMSOrder)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelOrders, d, skipOrders, ref sMsg);
            if (modelOrders != null)
            {
                //modelOrders.setUpdated(d.BookUpdated.ToArray()); 
            }
            return modelOrders;
        }

        /// <summary>
        /// select table data by passing Model
        /// </summary>
        /// <param name="d">Orders Model</param>
        /// <returns>returns Books table data</returns>
        /// Modified by SN on 2/21/18
        private DTO.AMSOrderList selectDTOData(Models.AMSOrder d)
        {
            DTO.AMSOrderList dtoOrders = new DTO.AMSOrderList();
            if (d != null)
            {
                List<string> skipOrders = new List<string> { "BookUpdated" };
                string sMsg = "";
                dtoOrders = (DTO.AMSOrderList)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(dtoOrders, d, skipOrders, ref sMsg);
                if (dtoOrders != null)
                {
                    //	byte[] bupdated = d.getUpdated();
                    //	dtoOrders.BookUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return dtoOrders;
        }

        /// <summary>
        /// select table data by passing Model
        /// </summary>
        /// <param name="d">DocumentType Model</param>
        /// <returns>returns BookItem table data</returns>
        /// Modified by SN on 2/21/18
        private Models.AMSBookItem selectBookModelData(DTO.BookItem d)
        {
            Models.AMSBookItem modelBookItems = new Models.AMSBookItem();
            List<string> skipBookItems = new List<string> { "BookItemUpdated" };
            string sMsg = "";
            modelBookItems = (Models.AMSBookItem)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelBookItems, d, skipBookItems, ref sMsg);
            if (modelBookItems != null)
            {
                //modelOrders.setUpdated(d.BookUpdated.ToArray()); 
            }
            return modelBookItems;
        }

        /// <summary>
        /// Selecting Order Validation Model data by passing table records
        /// </summary>
        /// <param name="d">The table data.</param>
        /// <returns>Returns Order validation</returns>
        private Models.AMSValidation selectModelDataOrderValidation(DAL.Models.AMSValidation d)
        {
            Models.AMSValidation modelOrderValidation = new Models.AMSValidation();
            List<string> skipAppointments = new List<string> { "" };
            string sMsg = "";
            modelOrderValidation = (Models.AMSValidation)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelOrderValidation, d, skipAppointments, ref sMsg);
            return modelOrderValidation;
        }

        #endregion

        #region "Rest Services"
        /// <summary>
        /// This is used to return a collection of Model record of Appointments using GetRecordsByOrder
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        /// <remarks>
        ///   The following data is also recognized: filters.
        ///    Modified by RHR for v- 8.2.0.118 on 9/9/2019
        ///    changed carriercontrol filter to carrier filter as a string we now 
        ///    support looking up the carrier name or the carrier number and find 
        ///    the first 
        ///  Modified by RHR for v-7.2.0.008 on 10/08/2020
        ///     we do not change filter from and filter to, these are always the dates because wild card searches return too many records
        /// </remarks>
        [HttpGet, ActionName("GetRecordsByOrdersFilter")]
        public Models.Response GetRecordsByOrdersFilter(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                var Fname = f.filterName;
                var Fvalue = f.filterValue;

                string sCarrier = "";
                var cns = "";
                var ord = "";
                var pro = "";
                var po = "";
                var shid = "";

                if (Fname == "Carrier")
                    sCarrier = Fvalue;
                if (Fname == "CNSNumber")
                    cns = Fvalue;
                if (Fname == "OrdNumber")
                    ord = Fvalue;
                if (Fname == "ProNumner")
                    pro = Fvalue;
                if (Fname == "PoNumber")
                    po = Fvalue;
                if (Fname == "SHID")
                    shid = Fvalue;
                //Modified by RHR for v-7.2.0.008 on 10/08/2020
                //we do not change filter from and filter to, these are always the dates because wild card searches return too many records
                //if (!string.IsNullOrWhiteSpace(Fvalue)){ f.filterFrom = null; f.filterTo = null; } 

                Models.AMSOrder[] amsAppointments = new Models.AMSOrder[] { };
                DAL.NGLAMSAppointmentData oAn = new DAL.NGLAMSAppointmentData(Parameters);
                DTO.AMSOrderList[] ltsRet = oAn.GetAMSOrders(f.CompControlFrom, f.filterFrom, f.filterTo, cns, ord, pro, po, sCarrier, shid);
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    amsAppointments = (from e in ltsRet
                                       select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(amsAppointments, count);
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
        /// This is used to return a collection of Model record of Order using GetGroupedRecords By Order
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetAMSOrdersGroupedByOrder")]
        public Models.Response GetAMSOrdersGroupedByOrder(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.AMSOrder Order = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.AMSOrder>(filter);
                Models.AMSOrder[] amsAppointments = new Models.AMSOrder[] { };
                DAL.NGLAMSAppointmentData oAn = new DAL.NGLAMSAppointmentData(Parameters);
                DTO.AMSOrderList[] ltsRet = oAn.GetAMSOrdersGrouped(Order.BookControl, Order.AMSCompControl, Order.EquipmentID, Order.BookDateLoad, Order.BookDateRequired, Order.LaneOriginAddressUse, Order.BookAMSPickupApptControl, Order.BookAMSDeliveryApptControl, Order.BookOrigCompControl, Order.BookDestCompControl, true);
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    amsAppointments = (from e in ltsRet
                                       select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(amsAppointments, count);
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
        /// This is used to return a collection of Model record of Order using GetRecords By Appointments
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecordsByAppointmentFilter")]
        public Models.Response GetRecordsByAppointmentFilter(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.AMSOrder[] amsAppointments = new Models.AMSOrder[] { };
                DAL.NGLAMSAppointmentData oAn = new DAL.NGLAMSAppointmentData(Parameters);

                DTO.AMSOrderList[] ltsRet = oAn.GetAMSOrdersByAppointment(int.Parse(f.filterValue));
                DAL.NGLCarrierData aNGLCarrierData = new DAL.NGLCarrierData(Parameters);

                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    amsAppointments = (from e in ltsRet
                                       select selectModelData(e)).ToArray();

                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(amsAppointments, count);
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
        /// This is used to return a collection of Model record of Order using GetRecords By BookControl Id
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecordsByBookControl")]
        public Models.Response GetRecordsByBookControl(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                //DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.BookItem[] amsBooks = new Models.BookItem[] { };

                DAL.NGLBookData oAn = new DAL.NGLBookData(Parameters);
                DAL.NGLCarrierData aNGLCarrierData = new DAL.NGLCarrierData(Parameters);

                List<int> aLst = new List<int>();
                aLst.Add(int.Parse(filter));


                Ngl.FreightMaster.Data.LTS.Book[] ltsBooks = oAn.GetBookByControls(aLst);
                DTO.BookItem[] ltsRet = ltsBooks.Select(book => new DTO.BookItem
                {
                    //Map properties manually
                    BookItemControl = book.BookControl,
                    BookItemCarrTarEquipMatControl = book.BookCarrierControl,
                }).ToArray();

                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    amsBooks = (from e in ltsRet
                                select selectModelForBooks(e)).ToArray();

                    foreach (var o in amsBooks)
                    {
                        //Carrier name
                        o.BookItemCarrTarEquipMatName = aNGLCarrierData.GetCarrier(o.BookItemCarrTarEquipMatControl).CarrierName;
                        //Carrier scac
                        o.BookItemNMFCSubClass = aNGLCarrierData.GetCarrier(o.BookItemCarrTarEquipMatControl).CarrierSCAC;
                    }

                    if (RecordCount > count) { count = RecordCount; }
                }

                response = new Models.Response(amsBooks, count);
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

        private Models.BookItem selectModelForBooks(DTO.BookItem d)
        {
            Models.BookItem modelOrders = new Models.BookItem();
            List<string> skipOrders = new List<string> { "BookUpdated" };
            string sMsg = "";
            modelOrders = (Models.BookItem)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelOrders, d, skipOrders, ref sMsg);
            if (modelOrders != null)
            {
                //modelOrders.setUpdated(d.BookUpdated.ToArray()); 
            }
            return modelOrders;
        }

        /// <summary>
        /// This is used to return a Model record of Orders List which has updated
        /// </summary>
        /// <param name="AO">The Model.</param>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("AddOrdersToAppointment")]
        public Models.Response AddOrdersToAppointment([System.Web.Http.FromBody] Models.ASMApptOdrFlg AO)
        {
            AO.Appt.AMSApptRecurrenceParentControl = null;
            Models.AMSAppointment dataAppt = AO.Appt;
            Models.AMSOrder[] dataOrd = AO.Ord;

            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLAMSAppointmentData dalData = new DAL.NGLAMSAppointmentData(Parameters);
                DTO.AMSAppointment oAppointment = new DTO.AMSAppointment();
                DTO.AMSOrderList[] aOrder = new DTO.AMSOrderList[] { };
                aOrder = (from e in dataOrd
                          select selectDTOData(e)).ToArray();

                DAL.Models.AMSValidation oValidation = new DAL.Models.AMSValidation();
                DTO.AMSOrderList[] oData = dalData.AddOrdersToAppointment(ref oValidation, aOrder, oAppointment);

                if (oValidation.Success)
                {
                    Models.AMSOrder[] oOrdRecords = new Models.AMSOrder[] { };
                    oOrdRecords = (from e in oData
                                   select selectModelData(e)).ToArray();
                    response = new Models.Response(oOrdRecords, 1);
                }
                else
                {
                    Models.AMSValidation[] oAMSValidation = new Models.AMSValidation[1];
                    oAMSValidation[0] = selectModelDataOrderValidation(oValidation);
                    response = new Models.Response(oAMSValidation, 1);
                }
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            // return the HTTP Response.
            return response;
        }

        /// <summary>
        /// This is used to return a collection of Model record of book items using book load control
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetBookItemsByBookLoadControl")]
        public Models.Response GetBookItemsByBookLoadControl(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.AMSBookItem[] amsBookItems = new Models.AMSBookItem[] { };
                DAL.NGLBookItemData dalData = new DAL.NGLBookItemData(Parameters);
                DTO.BookItem[] ltsBookItems = dalData.GetBookItemsFiltered(int.Parse(f.filterValue));

                if (ltsBookItems != null && ltsBookItems.Count() > 0)
                {
                    count = ltsBookItems.Count();
                    amsBookItems = (from e in ltsBookItems
                                    select selectBookModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(amsBookItems, count);
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
