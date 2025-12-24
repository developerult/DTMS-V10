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
using DModel = Ngl.FreightMaster.Data.Models;
using NGL.FM.BLL;
using Ngl.FreightMaster.Data.DataTransferObjects;
using System.Runtime.Remoting;
using System.Web.Http.Results;
using System.Data.Entity.Core.Metadata.Edm;

namespace DynamicsTMS365.Controllers
{
    public class LoadBoardController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LoadBoardController()
                : base(Utilities.PageEnum.LoadBoard)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
        // See Models.vBookLoadBoard for static methods

        #endregion

        #region " REST Services"

        // POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        // GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        // PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        // DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        /// <summary>
        /// reads the selected Load Board Data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.4 on 06/22/2021 added model for vBookLoadBoard to support time only fields
        ///   BookDestStartHrs,BookDestStopHrs,BookOriginStartHrs,BookOriginStopHrs
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
                DModel.AllFilters f = new DModel.AllFilters();
                f.filterName = "BookControl";
                f.filterValue = id.ToString();
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                Models.vBookLoadBoard[] oRecords = new Models.vBookLoadBoard[] { };
                LTS.vBookLoadBoard[] oData = new LTS.vBookLoadBoard[] { };
                oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) {  count = RecordCount;  }
                    oRecords = (from e in oData select Models.vBookLoadBoard.selectModelData(e)).ToArray();
                }
                response = new Models.Response(oRecords, count);
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
        

        [HttpGet, ActionName("UpdateCNS")]
        public Models.Response UpdateCNS(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                bool[] oRecords = { new DAL.NGLBookLoadBoard(Parameters).UpdateCNSNumber(id) };                
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
        [HttpGet, ActionName("GetLoadBoardSummary")]
        public Models.Response GetLoadBoardSummary(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vLoadBoardSummary[] records = new LTS.vLoadBoardSummary[] { };

                if (id == 0) {
                    return new Models.Response(records, 0);
                    //id = this.readPagePrimaryKey(); 
                }
                int count = 0;

                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                LTS.vLoadBoardSummary oData = oDAL.GetLoadBoardSummary(id);
                if (oData != null)
                {
                    records = new LTS.vLoadBoardSummary[1] { oData };
                    count = 1;
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
        //GetLoadBoardShipingSummary
        [HttpGet, ActionName("GetLoadBoardShipingSummary")]
        public Models.Response GetLoadBoardShipingSummary(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vLoadBoardShipingSummary[] records = new LTS.vLoadBoardShipingSummary[] { };

                if (id == 0)
                {
                    return new Models.Response(records, 0);
                    //id = this.readPagePrimaryKey(); 
                }
                int count = 0;

                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                LTS.vLoadBoardShipingSummary oData = oDAL.GetLoadBoardShipingSummary(id);
                if (oData != null)
                {
                    records = new LTS.vLoadBoardShipingSummary[1] { oData };
                    count = 1;
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
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                Models.vBookLoadBoard[] records = new Models.vBookLoadBoard[] { };
                LTS.vBookLoadBoard[] oData = new LTS.vBookLoadBoard[] { };
                oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData select Models.vBookLoadBoard.selectModelData(e)).ToArray();
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


        [HttpGet, ActionName("AcceptAllRecords")]
        public Models.Response AcceptAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                LTS.vBookLoadBoard[] oData = new LTS.vBookLoadBoard[] { };
                oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                bool[] oRecords = new bool[1];
                if (oData != null && oData.Count() > 0)
                {
                    //Call updatetrancode for each record in PC status
                    BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);
                    DAL.Models.ResultObject result = new DAL.Models.ResultObject() ;
                    foreach (LTS.vBookLoadBoard d in oData)
                    {
                        if (d.BookTranCode == "PC")
                        {
                            oBLL.UpdateBookTranCode365(ref result, d.BookControl, "PB", d.BookTranCode, d.BookCarrOrderNumber, d.BookOrderSequence);
                        }
                        count ++;
                    }
                   

                    oRecords[0] = true;

                    response = new Models.Response(oRecords, count);
                } else
                {
                    oRecords[0] = true; 
                    response = new Models.Response(oRecords, 1);
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


        [HttpGet, ActionName("GetTransLoad")]
        public Models.Response GetTransLoad(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                int iBookControl = f.BookControl;
                int iTransXControl = f.ParentControl;
                BLL.NGLLaneTransLoadXrefDetBLL oBLL = new BLL.NGLLaneTransLoadXrefDetBLL(Parameters);
                DTO.WCFResults result = oBLL.processSelectedTransLoadConfiguration(iTransXControl,iBookControl);
                bool[] oRecords = new bool[1];

                DAL.Models.ResultObject oRet = new DAL.Models.ResultObject();
                
            

                if (result != null && result.Success == false)
                {
                    oRet.Success = false;
                    response.Errors = Utilities.convertWCFMessagesToString(result);
                    //response.AsyncMessagesPossible = result.isAsyncMsgPossible();
                    //response.AsyncMessageKey = result.getAsyncMessageKey();
                    //response.AsyncTypeKey = result.getAsyncTypeKey();
                    //Utilities.addWCFMessagesToResponse(ref response, ref result, "Load Board");

                } else
                {
                    oRet.Success = true;
                    oRet.SuccessMsg = "Success your load has been routed!";


                }
                Array d = new DAL.Models.ResultObject[1] { oRet };
                response.Data = d;
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




        [HttpGet, ActionName("GetRecordsByCNS")]
        public Models.Response GetRecordsByCNS(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DModel.AllFilters f = new DModel.AllFilters();
                DModel.FilterDetails d = new DModel.FilterDetails();
                d.filterName = "BookConsPrefix";
                d.filterValueFrom = filter;
                d.filterValueTo = filter;
                f.addFilterDetail(d);               
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                Models.vBookLoadBoard[] records = new Models.vBookLoadBoard[] { };
                LTS.vBookLoadBoard[] oData = new LTS.vBookLoadBoard[] { };
                oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData select Models.vBookLoadBoard.selectModelData(e)).ToArray();
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
        /// Reads any messages using the LoadTenderControl passed into the filter data
        /// filters: LoadTenderControl, LoadTenderTypeControl, Caller title like "Rate Quote" 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.3.0.002 on 12/21/2020
        /// </remarks>
        [HttpGet, ActionName("GetLoadTenderMessages")]
        public Models.Response GetLoadTenderMessages(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            int iLoadTenderControl = 0;
            DAL.NGLLoadTenderData oDAL = null;
            try
            {               
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                oDAL = new DAL.NGLLoadTenderData(Parameters);
                iLoadTenderControl = Convert.ToInt32(f.FilterValues.Where(x => x.filterName == "LoadTenderControl").Select(y => y.filterValueFrom).FirstOrDefault());
                int iLoadTenderTypeControl = Convert.ToInt32(f.FilterValues.Where(x => x.filterName == "LoadTenderTypeControl").Select(y => y.filterValueFrom).FirstOrDefault());
                string sCaller = f.FilterValues.Where(x => x.filterName == "Caller").Select(y => y.filterValueFrom).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(sCaller)) { sCaller = "Load Tender"; }
                DTO.WCFResults result = oDAL.readLoadTenderCarrierCostMessages(iLoadTenderControl,(DAL.Utilities.NGLLoadTenderTypes)iLoadTenderTypeControl);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = new int[] { iLoadTenderControl };
                if (result != null)
                {
                    response.AsyncMessagesPossible = result.isAsyncMsgPossible();
                    response.AsyncMessageKey = result.getAsyncMessageKey();
                    response.AsyncTypeKey = result.getAsyncTypeKey();
                    Utilities.addWCFMessagesToResponse(ref response, ref result, sCaller);
                }
               

            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            } finally
            {
                if ( iLoadTenderControl != 0 && oDAL != null)
                {
                    try
                    {
                        bool blnDeleted = oDAL.deleteLoadTenderCarrierCostMessages(iLoadTenderControl);
                    }
                    catch
                    {
                        //do nothing
                    }
                }
                
            }
            return response;
        }
        /// <summary>
        /// Save Load Board Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.4 on 06/22/2021 added model for vBookLoadBoard to support time only fields
        ///   BookDestStartHrs,BookDestStopHrs,BookOriginStartHrs,BookOriginStopHrs
        /// </remarks>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.vBookLoadBoard data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);

                oDAL.DeliveryDateManuallyChanged(Utilities.convertStringToNullDateTime(data.BookCarrActDate), data.BookControl); //Added By LVV on 7/23/20 for v-8.3.0.001 Task #20200609162226 - Load Board Delivered Date

                data.BookDestEmergencyContactPhone = Utilities.removeNonNumericText(data.BookDestEmergencyContactPhone);
                data.BookDestPhone = Utilities.removeNonNumericText(data.BookDestPhone);
                data.BookCarrierContactPhone = Utilities.removeNonNumericText(data.BookCarrierContactPhone);
                data.BookOrigEmergencyContactPhone = Utilities.removeNonNumericText(data.BookOrigEmergencyContactPhone);
                data.BookOrigPhone = Utilities.removeNonNumericText(data.BookOrigPhone);
                LTS.vBookLoadBoard oChanges = new LTS.vBookLoadBoard();
                oChanges = Models.vBookLoadBoard.selectLTSData(data);               
                bool blnRet = oDAL.SaveBookLoadBoard(oChanges);

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

        [HttpPost, ActionName("PostGridDrop")]
        public Models.Response PostGridDrop([System.Web.Http.FromBody] Models.vBookLoadBoardDropped data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            bool[] oRecords = new bool[1];
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = true;
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                DAL.NGLBatchProcessDataProvider oDALBatch = new DAL.NGLBatchProcessDataProvider(Parameters);
                //if (data.BookControl == 0)
                //{
                //    blnRet = false;
                //}
                Models.vBookLoadBoard draggedObject = data.draggedObject;
                Models.vBookLoadBoard droppedObject = data.droppedObject;
                bool isDraggingUp = data.isDraggingUp;
                /* rules:
                 * both dragged and dropped must have different book controls non-zero
                 * both dragged and dropped must be in N or P status
                 * If the CNS is the same change the stop sequence for dragged to above dropped and move dropped down one (TODO:  add LoadPlanningReSequenceStopNumbers logic )   
                 * If CNS is different follow stamping rules in saveStampedCNS (TODO: add logic to reassign truck following LoadPlanningItemDropped logic)
                 */

                // begin validation: note, the client should run validation; this is just to ensure we have good data on the server.
                if (draggedObject.BookControl == 0 || droppedObject.BookControl == 0 || draggedObject.BookControl == droppedObject.BookControl)
                {
                    blnRet = false;
                    //'Cannot save changes to {0}.  The following key fields are required: {1}.
                    oDAL.throwInvalidKeyFaultException(DAL.SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyFieldsRequired, new List<string>() { "The Load Board", "Unique non-zero Book Control Key" });
                }
                /*
                 * TODO: add logic to test if the costs are locked
                 */
                if ((droppedObject.BookHoldLoad ?? 0) != 0)
                {
                    //'Cannot save changes to {0}.  The following key fields are required: {1}.
                    oDAL.throwRecordOnHoldException("Dropped Order Number: " + droppedObject.BookCarrOrderNumber);
                }
                if ((draggedObject.BookHoldLoad ?? 0) != 0)
                {
                    //'Cannot save changes to {0}.  The following key fields are required: {1}.
                    oDAL.throwRecordOnHoldException("Dragged Order Number: " + draggedObject.BookCarrOrderNumber);
                }
                if (droppedObject.BookTranCode != "N" & droppedObject.BookTranCode != "P")
                {
                    //'Cannot save changes to {0}.  The following key fields are required: {1}.
                    oDAL.throwInvalidTranCodeException("Dropped " + droppedObject.BookTranCode);
                }

                if (draggedObject.BookTranCode != "N" & draggedObject.BookTranCode != "P")
                {
                    //'Cannot save changes to {0}.  The following key fields are required: {1}.
                    oDAL.throwInvalidTranCodeException("Dragged " + draggedObject.BookTranCode);
                }

                if (draggedObject.BookConsPrefix == droppedObject.BookConsPrefix)
                {
                    //we are changing sequence for now we just replace dragged with dropped sequence and reduce dragged by one
                    //(draggedObject.BookHoldLoad ?? 0)
                    if ( draggedObject.BookStopNo > droppedObject.BookStopNo ) {
                        // going from higher to low
                        draggedObject.BookStopNo = (droppedObject.BookStopNo ?? 0);
                        droppedObject.BookStopNo = (short)((droppedObject.BookStopNo ?? 0) + 1);
                    } else if (draggedObject.BookStopNo < droppedObject.BookStopNo)
                    {
                        // going from lower to higher
                        draggedObject.BookStopNo = (droppedObject.BookStopNo ?? 0);
                        droppedObject.BookStopNo = (short)((droppedObject.BookStopNo ?? 0) -1);
                    } else
                    {
                        if (isDraggingUp)
                        {
                            if (droppedObject.BookStopNo == 1)
                            {
                                // do not change dragged object but move dropped object down
                                droppedObject.BookStopNo = (short)((droppedObject.BookStopNo ?? 0) + 1);
                            } else
                            {
                                //move the dragged object above the dropped object
                                draggedObject.BookStopNo = (short)((droppedObject.BookStopNo ?? 0) - 1);
                            }
                            
                        } else
                        {
                            // we are dragging down 
                            // do not change dropped object but move dragged object down
                            draggedObject.BookStopNo = (short)((droppedObject.BookStopNo ?? 0) + 1);
                        }
                        
                        
                        //oRecords[0] = true;
                        //response = new Models.Response(oRecords, 1);
                        //return response;
                    }
                    
                    // save the changes for both
                    draggedObject.BookDestEmergencyContactPhone = Utilities.removeNonNumericText(draggedObject.BookDestEmergencyContactPhone);
                    draggedObject.BookDestPhone = Utilities.removeNonNumericText(draggedObject.BookDestPhone);
                    draggedObject.BookCarrierContactPhone = Utilities.removeNonNumericText(draggedObject.BookCarrierContactPhone);
                    draggedObject.BookOrigEmergencyContactPhone = Utilities.removeNonNumericText(draggedObject.BookOrigEmergencyContactPhone);
                    draggedObject.BookOrigPhone = Utilities.removeNonNumericText(draggedObject.BookOrigPhone);
                    LTS.vBookLoadBoard oChanges = new LTS.vBookLoadBoard();
                    oChanges = Models.vBookLoadBoard.selectLTSData(draggedObject);
                    blnRet = oDAL.SaveBookLoadBoard(oChanges);
                    if (!blnRet)
                    {
                        oDAL.throwUnExpectedFaultException("The dragged data could not be saved,  please refresh your data and try again.");
                    }
                    droppedObject.BookDestEmergencyContactPhone = Utilities.removeNonNumericText(droppedObject.BookDestEmergencyContactPhone);
                    droppedObject.BookDestPhone = Utilities.removeNonNumericText(droppedObject.BookDestPhone);
                    droppedObject.BookCarrierContactPhone = Utilities.removeNonNumericText(droppedObject.BookCarrierContactPhone);
                    droppedObject.BookOrigEmergencyContactPhone = Utilities.removeNonNumericText(droppedObject.BookOrigEmergencyContactPhone);
                    droppedObject.BookOrigPhone = Utilities.removeNonNumericText(droppedObject.BookOrigPhone);
                    oChanges = new LTS.vBookLoadBoard();
                    oChanges = Models.vBookLoadBoard.selectLTSData(droppedObject);
                    blnRet = oDAL.SaveBookLoadBoard(oChanges);
                } else
                {
                    //same logic as save stamped
                    string sCNS = droppedObject.BookConsPrefix;
                    blnRet = oDALBatch.UpdateConsolidationNumber365(draggedObject.BookControl, sCNS);

                }               

                
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


        [HttpGet , ActionName("GetSeletedCNS")]
        public Models.Response GetSeletedCNS()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                string[] oRecords = new string[1];
                oRecords[0] = readPageCNSKey(Parameters, this.Page);
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
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                bool blnRet = oDAL.DeleteBookLoadBoard(id);
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


        [HttpGet, ActionName("StopResequence")]
        public Models.Response StopResequence(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);

                DModel.ResultObject oData = oBLL.StopResequence(id, false);
                response.Data = new DModel.ResultObject[] { oData };
                response.Count = 1;
                response.AsyncMessagesPossible = false;
                if (oData != null)
                {
                    Utilities.addResultObjectMessagesToResponse(ref response, ref oData, "Stop Resequence",true,true);
                   
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

        [HttpGet, ActionName("GetMiles")]
        public Models.Response GetMiles(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);

                DModel.ResultObject oData = oBLL.StopResequence(id, true);

                response.Data = new DModel.ResultObject[] { oData };
                response.Count = 1;
                response.AsyncMessagesPossible = false;
                if (oData != null)
                {
                    Utilities.addResultObjectMessagesToResponse(ref response, ref oData, "Get Miles");
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

        [HttpGet, ActionName("DuplicatePro")]
        public Models.Response DuplicatePro(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLBookBLL oBLL = new BLL.NGLBookBLL(Parameters); 
                LTS.vBookLoadBoard oData = new LTS.vBookLoadBoard();
                oData = oBLL.DuplicatePro(id);
                Models.vBookLoadBoard[] records = new Models.vBookLoadBoard[1];
                records[0] = Models.vBookLoadBoard.selectModelData(oData);
                response = new Models.Response(records, 1);
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

        [HttpGet, ActionName("createNewSequence")]
        public Models.Response createNewSequence(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLBookBLL oBLL = new BLL.NGLBookBLL(Parameters);
                Models.vBookLoadBoard[] records = new Models.vBookLoadBoard[1];
                LTS.vBookLoadBoard oData = new LTS.vBookLoadBoard();
                oData = oBLL.NewSequence(id);
                records[0] = Models.vBookLoadBoard.selectModelData(oData);
                response = new Models.Response(records, 1);
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

        //NGLLaneTransLoadXrefDetBLL

        [HttpGet, ActionName("createTransLoad")]
        public Models.Response createTransLoad(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLLaneTransLoadXrefDetBLL oBLL = new BLL.NGLLaneTransLoadXrefDetBLL(Parameters);
                bool[] oRecords = new bool[1];
                oRecords[0] = oBLL.processTransLoadFacility(id);
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

        [HttpPost, ActionName("ResetAPExport")]
        public Models.Response ResetAPExport([System.Web.Http.FromBody] string sBookControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                int iBookControl = 0;
                int.TryParse(sBookControl, out iBookControl);
                if (iBookControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.ResetAPExport(iBookControl);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Reset AP Export Flags", "Booking Primary Key.  Please select a record and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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

        //ConfirmShip
        [HttpPost, ActionName("ConfirmShip")]
        public Models.Response ConfirmShip([System.Web.Http.FromBody] string sBookControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                int iBookControl = 0;
                int.TryParse(sBookControl, out iBookControl);
                if (iBookControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.ConfirmShip(iBookControl);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                   // msg = NGLcmLocalizeKeyValuePairObjData.GetLocalizedValueByKey("MSGConfirmShipInvlaidRecord", "Cannot confirm shipment the selected booking record is invalid or cannot be located.")

                    List<string> sDetailList = new List<string> { "Ship Confirmation", "Cannot confirm shipment the selected booking record is invalid or cannot be located." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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
        [HttpPost, ActionName("HoldLoad")]
        public Models.Response HoldLoad([System.Web.Http.FromBody]string sBookControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                int iBookControl = 0;
                int.TryParse(sBookControl, out iBookControl);
                if (iBookControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.UpdateBookHoldLoad(iBookControl, true);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Booking Place Load on Hold", "Booking Primary Key.  Please select a record and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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


        [HttpPost, ActionName("UnHoldLoad")]
        public Models.Response UnHoldLoad([System.Web.Http.FromBody]string sBookControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                int iBookControl = 0;
                int.TryParse(sBookControl, out iBookControl);
                if (iBookControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.UpdateBookHoldLoad(iBookControl, false);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Booking Release Hold on Load", "Booking Primary Key.  Please select a record and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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

        [HttpPost, ActionName("saveStampedCNS")]
        public Models.Response saveStampedCNS([System.Web.Http.FromBody]string sBookControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBatchProcessDataProvider oDAL = new DAL.NGLBatchProcessDataProvider(Parameters);
                string sCNS = readPageCNSKey(Parameters, this.Page);
                int iBookControl = 0;
                int.TryParse(sBookControl, out iBookControl);
                if (iBookControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.UpdateConsolidationNumber365(iBookControl, sCNS);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Consoliation Number", "Booking Primary Key.  Please select a record and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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

        [HttpGet, ActionName("getCNSStamp")]
        public Models.Response getCNSStamp(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
               string sCNS = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string>(filter);

                bool[] oRecords = new bool[1];
                oRecords[0] = savePageCNSKey(Parameters, this.Page, sCNS);
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

        [HttpGet, ActionName("RecalculateUsingLineHaul")]
        public Models.Response RecalculateUsingLineHaul(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLBookRevenueBLL oBLL = new BLL.NGLBookRevenueBLL(Parameters);

                DTO.CarrierCostResults oData = oBLL.RecalculateUsingLineHaul(id);
                response.Data = new bool[] { oData.Success };
                Utilities.addCarrierCostResultMessagesToResponse(ref response, ref oData, "Re-Calc With Line Haul" );
               
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
        //UpdateCarrier
        [HttpGet, ActionName("Calculate")]
        public Models.Response Calculate(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLBookRevenueBLL oBLL = new BLL.NGLBookRevenueBLL(Parameters);

                DTO.CarrierCostResults oData = oBLL.UpdateCarrier(id);
                response.Data = new bool[] { oData.Success };
                Utilities.addCarrierCostResultMessagesToResponse(ref response, ref oData, "Re-Calc With Line Haul");

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