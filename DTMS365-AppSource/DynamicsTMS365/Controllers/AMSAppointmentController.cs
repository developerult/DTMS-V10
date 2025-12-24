using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DModel = Ngl.FreightMaster.Data.Models;


namespace DynamicsTMS365.Controllers
{
	/// <summary>
	/// AppointmentsController for All Appointments Rest API Controls
	/// </summary>
	/// <remarks>
	/// Created By SK on 29-05-18
	/// </remarks>
	public class AMSAppointmentController : NGLControllerBase
    {

		#region " Properties"
		/// <summary>
		/// This property is used for logging and error tracking.
		/// </summary>
		private string _SourceClass = "DynamicsTMS365.Controllers.AMSAppointmentsController";
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
		private Models.AMSAppointment selectModelDataAppt(DTO.AMSAppointment d)
		{
			Models.AMSAppointment modelAppointments = new Models.AMSAppointment();
			List<string> skipAppointments = new List<string> { "AMSApptUpdated" };
			string sMsg = "";
			modelAppointments = (Models.AMSAppointment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelAppointments, d, skipAppointments, ref sMsg);
			if (modelAppointments != null) { modelAppointments.setUpdated(d.AMSApptUpdated.ToArray()); }
			return modelAppointments;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">DocumentType Model</param>
		/// <returns>returns tblEDIDocumentType table data</returns>
		/// Modified by SN on 2/21/18
		private DTO.AMSAppointment selectDTODataAppt(Models.AMSAppointment d)
		{
			DTO.AMSAppointment dtoAppointments = new DTO.AMSAppointment();
			if (d != null)
			{
				List<string> skipAppointments = new List<string> { "AMSApptUpdated" };
				string sMsg = "";
				dtoAppointments = (DTO.AMSAppointment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(dtoAppointments, d, skipAppointments, ref sMsg);
				if (dtoAppointments != null)
				{
					byte[] bupdated = d.getUpdated();
					dtoAppointments.AMSApptUpdated = bupdated == null ? new byte[0] : bupdated;
				}
			}
			return dtoAppointments;
		}

		/// <summary>
		/// Selecting a Appointments Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns Appointments</returns>
		private Models.AMSOrder selectModelDataOrd(DTO.AMSOrderList d)
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
		private DTO.AMSOrderList selectDTODataOrd(Models.AMSOrder d)
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
		/// Selecting a Appointments Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns Appointment Tracking Details</returns>
		private Models.AMSApptTracking selectModelDataApptTracking(DTO.AMSAppointmentTracking d)
		{
			Models.AMSApptTracking modelApptTrackingDetails = new Models.AMSApptTracking();
			List<string> skipAppointments = new List<string> { "AMSApptTrackingUpdated" };
			string sMsg = "";
			modelApptTrackingDetails = (Models.AMSApptTracking)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelApptTrackingDetails, d, skipAppointments, ref sMsg);
			if (modelApptTrackingDetails != null) { modelApptTrackingDetails.setUpdated(d.AMSApptTrackingUpdated.ToArray()); }
			return modelApptTrackingDetails;
		}

		/// <summary>
		/// Selecting a Appointments Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns Appointment User Field Details</returns>
		private Models.AMSApptUserFields selectModelDataApptUserFields(DTO.AMSAppointmentUserFieldData d)
		{
			Models.AMSApptUserFields modelApptTrackingDetails = new Models.AMSApptUserFields();
			List<string> skipAppointments = new List<string> { "AMSApptUFDUpdated" };
			string sMsg = "";
			modelApptTrackingDetails = (Models.AMSApptUserFields)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelApptTrackingDetails, d, skipAppointments, ref sMsg);
			if (modelApptTrackingDetails != null) { modelApptTrackingDetails.setUpdated(d.AMSApptUFDUpdated.ToArray()); }
			return modelApptTrackingDetails;
		}

		/// <summary>
		/// select table data by passing Appointment User Field Model
		/// </summary>
		/// <param name="d">AMSApptUserFields Model</param>
		/// <returns>returns AMSAppointmentUserFieldData table data</returns>
		private LTS.AMSAppointmentUserFieldData selectLTSData(Models.AMSApptUserFields d)
		{
			LTS.AMSAppointmentUserFieldData ltsApptUserField = new LTS.AMSAppointmentUserFieldData();
			if (d != null)
			{
				List<string> skipApptUserField = new List<string> { "AMSApptUFDUpdated" };
				string sMsg = "";
				ltsApptUserField = (LTS.AMSAppointmentUserFieldData)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsApptUserField, d, skipApptUserField, ref sMsg);
				if (ltsApptUserField != null)
				{
					byte[] bupdated = d.getUpdated();
					ltsApptUserField.AMSApptUFDUpdated = bupdated == null ? new byte[0] : bupdated;
				}
			}
			return ltsApptUserField;
		}

		/// <summary>
		/// select table data by passing Appointment Tracking Data Model
		/// </summary>
		/// <param name="d">AMSApptTracking Model</param>
		/// <returns>returns AMSAppointmentTracking table data</returns>
		private LTS.AMSAppointmentTracking selectLTSTrackingData(Models.AMSApptTracking d)
		{
			LTS.AMSAppointmentTracking ltsApptTrackingData = new LTS.AMSAppointmentTracking();
			if (d != null)
			{
				List<string> skipApptTrackingField = new List<string> { "AMSApptTrackingUpdated" };
				string sMsg = "";
				ltsApptTrackingData = (LTS.AMSAppointmentTracking)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsApptTrackingData, d, skipApptTrackingField, ref sMsg);
				if (ltsApptTrackingData != null)
				{
					byte[] bupdated = d.getUpdated();
					ltsApptTrackingData.AMSApptTrackingUpdated = bupdated == null ? new byte[0] : bupdated;
				}
			}
			return ltsApptTrackingData;
		}

		/// <summary>
		/// Selecting a Appointment Validation Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns Appointment validation</returns>
		private Models.AMSValidation selectModelDataApptValidation(DAL.Models.AMSValidation d)
		{
			Models.AMSValidation modelApptValidation = new Models.AMSValidation();
			List<string> skipAppointments = new List<string> { "" };
			string sMsg = "";
			modelApptValidation = (Models.AMSValidation)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelApptValidation, d, skipAppointments, ref sMsg);
			return modelApptValidation;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">AMSValidation Model</param>
		private DAL.Models.AMSValidation selectDALDataApptValidation(Models.AMSValidation d)
		{
			DAL.Models.AMSValidation dalApptValidation = new DAL.Models.AMSValidation();
			if (d != null)
			{
				List<string> skipAppointments = new List<string> { "" };
				string sMsg = "";
				dalApptValidation = (DAL.Models.AMSValidation)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(dalApptValidation, d, skipAppointments, ref sMsg);
			}
			return dalApptValidation;
		}

		/// <summary>
		/// Selecting a Appointment Validation Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns Appointment validation</returns>
		private DTO.AMSOrderList selectAMSOrderList(Models.AMSOrder d)
		{
			DTO.AMSOrderList amsOrderList = new DTO.AMSOrderList();
			List<string> skipAMSOrderList = new List<string> { "" };
			string sMsg = "";
			amsOrderList = (DTO.AMSOrderList)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(amsOrderList, d, skipAMSOrderList, ref sMsg);
			return amsOrderList;
		}

		#endregion

		#region "Rest Services"

		[HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                DAL.NGLDockBlockOutPeriodData oBlock = new DAL.NGLDockBlockOutPeriodData(Parameters);
                List<Models.AMSAppointment> amsAppointments = new List<Models.AMSAppointment>();
                Models.AMSAppointment[] amsAppointmentFromDB = new Models.AMSAppointment[] { };
                int count = 0;
                int RecordCount = 0;
                DModel.RecurrenceEvent[] blocks = oBlock.GetDockBlockOutPeriodsByComp(f.CompControlFrom);                
                foreach (DModel.RecurrenceEvent b in blocks)
                {
                    Models.AMSAppointment appt = new Models.AMSAppointment();
                    int intRecurrenceId = 0;
                    int.TryParse(b.recurrenceId, out intRecurrenceId);
                    appt.AMSApptCompControl = f.CompControlFrom;
                    appt.AMSApptDockdoorID = b.DockDoorID;
                    appt.DockDoorName = b.DockDoorName;
                    appt.AMSApptStartDate = b.StartDate;
                    appt.AMSApptEndDate = b.EndDate;
                    appt.AMSApptRecurrence = b.recurrenceRule;
                    appt.AMSApptDescription = b.Description;
                    appt.AMSApptLabel = b.Title;
                    appt.AMSApptHover = b.Title;
                    appt.AMSApptStatusCode = intRecurrenceId; //Map DockBlockRecurrenceType to recurrenceID
                    appt.CompAMSColorCodeSettingColorCode = b.RecTypeColorCode;
                    appt.AMSApptCarrierName = "test";
                    appt.AMSApptCarrierSCAC = "TTTT";
                    //appt.AMSApptControl = 111111;
                    appt.AMSApptControl = b.Id;             
                    //appt.AMSApptControl = 0;
                    appt.AMSApptRecurrenceParentControl = b.Id; //this indicates a BlockOutPeriod recurrence event
                    amsAppointments.Add(appt);
                }                              
                DTO.AMSAppointment[] ltsRet = NGLAMSAppointmentData.GetAMSAppointmentsFilteredOptimized(f.CompControlFrom, DateTime.Parse(f.filterName), DateTime.Parse(f.filterValue));
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    amsAppointmentFromDB = (from e in ltsRet select selectModelDataAppt(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                if (amsAppointmentFromDB != null && amsAppointmentFromDB.Count() > 0)
                {
                    count += amsAppointments.Count;
                    foreach (Models.AMSAppointment a in amsAppointmentFromDB)
                    {
                        amsAppointments.Add(a);
                    }
                }
                response = new Models.Response(amsAppointments.ToArray(), count);
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
        /// This is used to return a collection of Model record of Appointments using GetRecordsByOrder
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

                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AMSAppointment[] amsAppointments = new Models.AMSAppointment[] { };
                int RecordCount = 0;
                int count = 0;
                var Fname = f.filterName;
				var Fvalue = f.filterValue;
                bool bound;
                string appt = "", ord = "", pro = "", po = "", cns = "", shid = "";
                if (Fname == "ApptNumber") { appt = Fvalue; }
                if (Fname == "OrdNumber") { ord = Fvalue; }
                if (Fname == "ProNumner") { pro = Fvalue; }
                if (Fname == "PoNumber") { po = Fvalue; }
                if (Fname == "CNS") { cns = Fvalue; }
                if (Fname == "SHID") { shid = Fvalue; }
                if (f.page == 0) { bound = false; } else { bound = true; }		
				DTO.AMSAppointment[] ltsRet = new DTO.AMSAppointment[1];
				if(appt != "")
				{
					ltsRet[0] = NGLAMSAppointmentData.GetAMSAppointmentFiltered(int.Parse(appt));
				}
				else
				{
					ltsRet[0] = NGLAMSAppointmentData.FindAMSApptByOrder(f.CompControlFrom, bound, ord, pro, po, cns, shid);
				}				
				if (ltsRet[0] != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsAppointments = (from e in ltsRet select selectModelDataAppt(e)).ToArray();
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
		/// This is used to return a Model record of Document Type which has updated
		/// </summary>
		/// <param name="AOF">all Models Object.</param>
		/// <returns>Updated Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("SaveUpdateAppointmentOrders")]
		public Models.Response SaveUpdateAppointmentOrders([System.Web.Http.FromBody]Models.ASMApptOdrFlg AOF)
		{
            bool flag = AOF.Flag;
			Models.AMSAppointment dataAppt = AOF.Appt;
			Models.AMSOrder[] dataOrd = AOF.Ord;
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DTO.AMSOrderList[] aOrder = new DTO.AMSOrderList[] { };
                DModel.AMSValidation oValidation = new DModel.AMSValidation();
                DTO.AMSAppointment oAppointment = selectDTODataAppt(dataAppt);			
				aOrder = (from e in dataOrd select selectDTODataOrd(e)).ToArray();             
				//Here you will need to populate the oValidation object with whatever was passed in from the javascript
				oValidation = selectDALDataApptValidation(AOF.Validation);
				DTO.AMSAppointmentResult oData = NGLAMSAppointmentData.CreateOrUpdateAppointment(ref oValidation, aOrder, oAppointment,flag);
                //First we must check the Success Flag
                if (!oValidation.Success)
                {
					Models.AMSValidation[] oAMSValidation = new Models.AMSValidation[1];
					oAMSValidation[0] = selectModelDataApptValidation(oValidation);
					response = new Models.Response(oAMSValidation, 1);
					//If oValidation.Success is false here then that means that oData WILL BE NULL (because the save did not run because validation failed)
					//So this situation needs to be handled here in terms of the results returned to the javascript caller
					//Also, oValidation must be returned as part of those results to the javascript (the result will indicate what actions need to occur next) 
				}
				else
				{
					Models.AMSAppointment[] oApptRecords = new Models.AMSAppointment[1];
					Models.AMSOrder[] oOrdRecords = new Models.AMSOrder[] { };
					oApptRecords[0] = selectModelDataAppt(oData.Appointment);
					oOrdRecords = (from e in oData.AppointmentOrders select selectModelDataOrd(e)).ToArray();
					response = new Models.Response(oApptRecords, 1);
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
		/// This is used to return true or false record of EDIMasterDocStructSegment which has deleted or not
		/// based on MDSSegControl using teststructloop class
		/// </summary>
		/// <param name="data">The testMstructsegment.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("DeleteAppointment")]
		public Models.Response DeleteAppointment([System.Web.Http.FromBody]Models.AMSAppointment data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				DTO.AMSAppointment oData = selectDTODataAppt(data);
                if (data != null && data.AMSApptControl > 0) { NGLAMSAppointmentData.DeleteAMSAppointment(oData.AMSApptControl, false); } else { result = false; }
				Array d = new bool[1] { result };
				response = new Models.Response(d, 1);
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
		/// This is used to return true or false record after removing booking from appointment
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("RemoveBookingFromAppointment")]
		public Models.Response RemoveBookingFromAppointment([FromBody]string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
            DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
			try
			{
				int ApptControl = f.ApptControl;
				int BookControl = f.BookControl;
                if (BookControl > 0) { NGLAMSAppointmentData.RemoveAMSBooking(ApptControl, BookControl); } else { result = false; }
				Array d = new bool[1] { result };
				response = new Models.Response(d, 1);
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
		/// This is used to return a collection of Model record of Appointments
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetAMSAppointmentTrackingDetails")]
		public Models.Response GetAMSAppointmentTrackingDetails(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AMSApptTracking[] records = new Models.AMSApptTracking[] { };
                DAL.NGLAMSAppointmentTrackingData oAn = new DAL.NGLAMSAppointmentTrackingData(Parameters);
                int RecordCount = 0;
                int count = 0;                			
				DTO.AMSAppointmentTracking[] oData = oAn.GetAMSAppointmentTrackingsFiltered(f.ApptControl);
				if (oData?.Count() > 0)
				{
					count = oData.Count();
                    records = (from e in oData select selectModelDataApptTracking(e)).ToArray();
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
		/// This api method is used to update tracking data and return a Model record
		/// </summary>
		/// <param name="data">The Model.</param>
		/// <returns>Insert Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("UpdateAMSAppointmentTrackingFieldRecord")]
		public Models.Response UpdateAMSAppointmentTrackingFieldRecord([System.Web.Http.FromBody]Models.AMSApptTracking data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				DAL.NGLAMSAppointmentTrackingData dalData = new DAL.NGLAMSAppointmentTrackingData(Parameters);
				LTS.AMSAppointmentTracking oData = selectLTSTrackingData(data);
                if (oData != null) { dalData.InsertOrUpdateAMSAppointmentTracking(oData); } else { result = false; }
				Array d = new bool[1] { result };
				response = new Models.Response(d, 1);
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
		/// This is used to return a collection of Model record of Appointments
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetAMSAppointmentUserFieldDetails")]
		public Models.Response GetAMSAppointmentUserFieldDetails(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AMSApptUserFields[] records = new Models.AMSApptUserFields[] { };
                DAL.NGLAMSAppointmentUserFieldDataData oAn = new DAL.NGLAMSAppointmentUserFieldDataData(Parameters);
                int RecordCount = 0;
                int count = 0;               				
				DTO.AMSAppointmentUserFieldData[] oData = oAn.GetAMSAppointmentUserFieldDatasFiltered(f.ApptControl);
				if (oData?.Count() > 0)
				{
					count = oData.Count();
                    records = (from e in oData select selectModelDataApptUserFields(e)).ToArray();
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
		/// This api method is used to update user field details and return a Model record
		/// </summary>
		/// <param name="data">The Model.</param>
		/// <returns>Insert Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("UpdateAMSAppointmentUserFieldRecord")]
		public Models.Response UpdateAMSAppointmentUserFieldRecord([System.Web.Http.FromBody]Models.AMSApptUserFields data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				DAL.NGLAMSAppointmentUserFieldDataData dalData = new DAL.NGLAMSAppointmentUserFieldDataData(Parameters);
				LTS.AMSAppointmentUserFieldData oData = selectLTSData(data);
                if (oData != null) { dalData.InsertOrUpdateAMSAppointmentUserFieldData(oData); } else { result = false; }
				Array d = new bool[1] { result };
				response = new Models.Response(d, 1);
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
		/// This is used to return a collection of Model record of appointments for Warehouse
		/// </summary>
		/// <param name="Orders">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpPost, ActionName("GetWarehouseAvailableAppointments")]
		public Models.Response GetWarehouseAvailableAppointments([System.Web.Http.FromBody]Models.AMSOrder[] Orders)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLDockSettingData dalData = new DAL.NGLDockSettingData(Parameters);
				DTO.AMSOrderList[] OrdersList = new DTO.AMSOrderList[] { };
				if(Orders.Length > 0)
				{
					OrdersList = (from e in Orders select selectAMSOrderList(e)).ToArray();
					DModel.AMSCarrierAvailableSlots[] amsCASlots = dalData.GetWarehouseAvailableAppointments(OrdersList);
					var retVals = amsCASlots.ToArray();
					response = new Models.Response(retVals, retVals.Length);
				}
				else
				{
					DModel.AMSCarrierAvailableSlots[] noAMSCASlots = new DModel.AMSCarrierAvailableSlots[] { };
					response = new Models.Response(noAMSCASlots, noAMSCASlots.Length);
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
		/// This is used to update the already booked carrier appointment
		/// </summary>
		/// <param name="data">The testMstructsegment.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("BookWarehouseAppointment")]
		public Models.Response BookWarehouseAppointment([System.Web.Http.FromBody]DModel.AMSCarrierAvailableSlots data)
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
				string resp = NGLAMSAppointmentData.BookWarehouseAppointment(data);
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

		#endregion

	}
}