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
	/// CarrierController for All Carrier Order and Appointments Rest API Controls
	/// </summary>
	/// <remarks>
	/// Created By PKN on 14-08-18
	/// </remarks>
	public class AMSCarrierController : NGLControllerBase
    {

		#region " Properties"
		/// <summary>
		/// This property is used for logging and error tracking.
		/// </summary>
		private string _SourceClass = "DynamicsTMS365.Controllers.AMSCarrierController";
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
		/// Selecting a AMSCarrierOrders Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns AMSCarrierOrders Type</returns>
		/// Modified by PKN on 8/4/18
		private Models.AMSCarrierOrders selectAMSCarrierBookPickupOrderFromLTSData(LTS.vAMSCarrierPickNeedAppt d)
		{
			Models.AMSCarrierOrders modelAMSCarrierBoookPickupOrder = new Models.AMSCarrierOrders();
			List<string> skipAMSCarrierBoookPickupOrder = new List<string> {  };
			string sMsg = "";
			modelAMSCarrierBoookPickupOrder = (Models.AMSCarrierOrders)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelAMSCarrierBoookPickupOrder, d, skipAMSCarrierBoookPickupOrder, ref sMsg);
			//if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
			return modelAMSCarrierBoookPickupOrder;
		}

		/// <summary>
		/// Selecting a AMSCarrierOrders Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns AMSCarrierOrders Type</returns>
		/// Modified by PKN on 8/4/18
		private Models.AMSCarrierOrders selectAMSCarrierBookDelOrderFromLTSData(LTS.vAMSCarrierDelNeedAppt d)
		{
			Models.AMSCarrierOrders modelAMSCarrierBoookPickupOrder = new Models.AMSCarrierOrders();
			List<string> skipAMSCarrierBoookPickupOrder = new List<string> { };
			string sMsg = "";
			modelAMSCarrierBoookPickupOrder = (Models.AMSCarrierOrders)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelAMSCarrierBoookPickupOrder, d, skipAMSCarrierBoookPickupOrder, ref sMsg);
			//if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
			return modelAMSCarrierBoookPickupOrder;
		}

		/// <summary>
		/// Selecting a AMSCarrierOrders Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns AMSCarrierOrders Type</returns>
		/// Modified by PKN on 8/4/18
		private Models.AMSCarrierOrders selectCarrierBookedPickupOrderFromLTSData(LTS.vAMSCarrierPickBookedAppt d)
		{
			Models.AMSCarrierOrders modelAMSCarrierBookedPickupOrders = new Models.AMSCarrierOrders();
			List<string> skipAMSCarrierBoookPickupOrder = new List<string> { };
			string sMsg = "";
			modelAMSCarrierBookedPickupOrders = (Models.AMSCarrierOrders)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelAMSCarrierBookedPickupOrders, d, skipAMSCarrierBoookPickupOrder, ref sMsg);
			//if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
			return modelAMSCarrierBookedPickupOrders;
		}

		/// <summary>
		/// Selecting a AMSCarrierOrders Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns AMSCarrierOrders Type</returns>
		/// Modified by PKN on 8/4/18
		private Models.AMSCarrierOrders selectCarrierBookedDelOrderFromLTSData(LTS.vAMSCarrierDelBookedAppt d)
		{
			Models.AMSCarrierOrders modelAMSCarrierBookedDelOrders = new Models.AMSCarrierOrders();
			List<string> skipAMSCarrierBoookDelOrder = new List<string> { };
			string sMsg = "";
			modelAMSCarrierBookedDelOrders = (Models.AMSCarrierOrders)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelAMSCarrierBookedDelOrders, d, skipAMSCarrierBoookDelOrder, ref sMsg);
			//if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
			return modelAMSCarrierBookedDelOrders;
		}

		/// <summary>
		/// Selecting a Appointment Validation Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns Appointment validation</returns>
		private Models.EquipIDValidation selectModelDataEquipIDValidation(DAL.Models.EquipIDValidation d)
		{
			Models.EquipIDValidation modelEquipIDValidation = new Models.EquipIDValidation();
			List<string> skipEquipID = new List<string> { "" };
			string sMsg = "";
			modelEquipIDValidation = (Models.EquipIDValidation)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEquipIDValidation, d, skipEquipID, ref sMsg);
			return modelEquipIDValidation;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">AMSValidation Model</param>
		private DAL.Models.EquipIDValidation selectDALDataEquipIDValidation(Models.EquipIDValidation d)
		{
			DAL.Models.EquipIDValidation dalEquipIDValidation = new DAL.Models.EquipIDValidation();
			if (d != null)
			{
				List<string> skipEquipID = new List<string> { "" };
				string sMsg = "";
				dalEquipIDValidation = (DAL.Models.EquipIDValidation)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(dalEquipIDValidation, d, skipEquipID, ref sMsg);
			}
			return dalEquipIDValidation;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">AMSValidation Model</param>
		private Models.AMSCarrierOrdersChart selectAMSCarrierOrdersChart(LTS.SPGetCarrierOrderSummaryForChartResult d)
		{
			Models.AMSCarrierOrdersChart amsCarrierOrdersChart = new Models.AMSCarrierOrdersChart();
			if (d != null)
			{
				List<string> skiAMSCarrierOrdersChart = new List<string> { "" };
				string sMsg = "";
				amsCarrierOrdersChart = (Models.AMSCarrierOrdersChart)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(amsCarrierOrdersChart, d, skiAMSCarrierOrdersChart, ref sMsg);
			}
			return amsCarrierOrdersChart;
		}

		#endregion

		#region "Rest Services"
		/// <summary>
		/// This is used to return a collection of orders count for different dates
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recogyenized: filters.</remarks>
		[HttpGet, ActionName("GetCarrierOrderSummaryForChart")]
		public Models.Response GetCarrierOrderSummaryForChart(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				int count = 0;
				DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
				Models.AMSCarrierOrdersChart[] amsCarrierOrdersChart = new Models.AMSCarrierOrdersChart[] { };
				DAL.NGLCarrierData oAn = new DAL.NGLCarrierData(Parameters);
				LTS.SPGetCarrierOrderSummaryForChartResult[] ltsCarrierOrders = oAn.GetCarrierOrderSummaryForChart(Parameters.UserCarrierControl, f.take);
				if (ltsCarrierOrders != null && ltsCarrierOrders.Count() > 0)
				{
					count = ltsCarrierOrders.Count();
					amsCarrierOrdersChart = (from e in ltsCarrierOrders
											  select selectAMSCarrierOrdersChart(e)).ToArray();
				}
				response = new Models.Response(amsCarrierOrdersChart, count);
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
		/// This is used to return a collection of Model record of Pickup Orders
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetCarrierPickUpOrdersForAppt")]
		public Models.Response GetCarrierPickUpOrdersForAppt(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				int count = 0;
				int RecordCount = 0;
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

				Models.AMSCarrierOrders[] amsCarrierPickupOrders = new Models.AMSCarrierOrders[] { };
				DAL.NGLAMSAppointmentData oAn = new DAL.NGLAMSAppointmentData(Parameters);
				LTS.vAMSCarrierPickNeedAppt[] ltsRet = oAn.GetAMSCarrierPickNeedAppt(f, ref RecordCount);

				if (ltsRet != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsCarrierPickupOrders = (from e in ltsRet
									   select selectAMSCarrierBookPickupOrderFromLTSData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsCarrierPickupOrders, count);
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
		/// This is used to return a collection of Model record of Delivery Orders
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetCarrierDeliveryOrdersForAppt")]
		public Models.Response GetCarrierDeliveryOrdersForAppt(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				int count = 0;
				int RecordCount = 0;
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

				Models.AMSCarrierOrders[] amsCarrierDeliveryOrders = new Models.AMSCarrierOrders[] { };
				DAL.NGLAMSAppointmentData oAn = new DAL.NGLAMSAppointmentData(Parameters);
				LTS.vAMSCarrierDelNeedAppt[] ltsRet = oAn.GetAMSCarrierDelNeedAppt(f, ref RecordCount);

				if (ltsRet != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsCarrierDeliveryOrders = (from e in ltsRet
											  select selectAMSCarrierBookDelOrderFromLTSData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsCarrierDeliveryOrders, count);
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
		/// This is used to return a collection of Model record of booked pickup Orders
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetCarrierBookedPickupOrders")]
		public Models.Response GetCarrierBookedPickupOrders(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				int count = 0;
				int RecordCount = 0;
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

				Models.AMSCarrierOrders[] amsCarrierBookedPickupOrders = new Models.AMSCarrierOrders[] { };
				DAL.NGLAMSAppointmentData oAn = new DAL.NGLAMSAppointmentData(Parameters);
				LTS.vAMSCarrierPickBookedAppt[] ltsRet = oAn.GetAMSCarrierPickBookedAppt(f, ref RecordCount);

				if (ltsRet != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsCarrierBookedPickupOrders = (from e in ltsRet
											  select selectCarrierBookedPickupOrderFromLTSData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsCarrierBookedPickupOrders, count);
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
		/// This is used to return a collection of Model record of Delivery Orders
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetCarrierBookedDelOrders")]
		public Models.Response GetCarrierBookedDelOrders(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				int count = 0;
				int RecordCount = 0;
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

				Models.AMSCarrierOrders[] amsCarrierDeliveryOrders = new Models.AMSCarrierOrders[] { };
				DAL.NGLAMSAppointmentData oAn = new DAL.NGLAMSAppointmentData(Parameters);
				LTS.vAMSCarrierDelBookedAppt[] ltsRet = oAn.GetAMSCarrierDelBookedAppt(f, ref RecordCount);

				if (ltsRet != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsCarrierDeliveryOrders = (from e in ltsRet
												select selectCarrierBookedDelOrderFromLTSData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsCarrierDeliveryOrders, count);
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
		/// This is used to return a collection of Model record of appointments for carrier
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetCarrierAvailableAppointments")]
		public Models.Response GetCarrierAvailableAppointments(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				Models.ASMCarrierAviblApptInfo CAI = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.ASMCarrierAviblApptInfo>(filter);
                int compControl = 0;
                if (CAI.IsPickup){ compControl = CAI.BookOrigCompControl; }else { compControl = CAI.BookDestCompControl; }
                DModel.AMSCarrierResults oTData = NGLDockSettingData.GetCarrierAvailableAppointments(CAI.CarrierControl, compControl, CAI.BookControl, CAI.EquipmentID, CAI.BookDateLoad, CAI.BookDateRequired, CAI.IsPickup, CAI.Inbound, CAI.Warehouse, CAI.CarrierName, CAI.CarrierNumber, CAI.ScheduledDate, CAI.ScheduledTime, CAI.SHID);                
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
		/// This is used to return message indicating appointment is booked or not
		/// </summary>
		/// <param name="filter">The testMstructsegment.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("UpdateBookedCarrierAppointment")]
		public Models.Response UpdateBookedCarrierAppointment([FromBody]string filter)
		{
			var response = new Models.Response();
			string result = "";
			if (!authenticateController(ref response)) { return response; }
            try
            {
                Array d;
                DModel.AMSCarrierAvailableSlots f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AMSCarrierAvailableSlots>(filter);
                DAL.NGLAMSAppointmentData dalData = new DAL.NGLAMSAppointmentData(Parameters);

                if (f.ApptControl != 0) { result = dalData.UpdateCarrierBookedAppointment(f); } else {  result = "false"; }
				if (result == "") { d = new bool[1] { true }; } else { d = new bool[1] { false }; }

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
		/// This is used to update the already booked carrier appointment
		/// </summary>
		/// <param name="data">The testMstructsegment.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("BookCarrierAppointment")]
		public Models.Response BookCarrierAppointment([System.Web.Http.FromBody]DModel.AMSCarrierAvailableSlots data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLAMSAppointmentData dalData = new DAL.NGLAMSAppointmentData(Parameters);

				if (data == null || string.IsNullOrWhiteSpace(data.Books))
				{
					response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					response.Errors = "There was a problem with the data. Please refresh the data and try again. If this does not fix the problem, please contact technical support.";
					return response;
				}

                string resp = NGLAMSAppointmentData.CarrierAutomationCreateNewAppointment(data); //Modified By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
                //string resp = dalData.BookCarrierAppointment(data);
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
		/// This is used to get and modify appt slot for already booked carrier appointment
		/// </summary>
		/// <param name="filter">The carrierapptslot.</param>
		/// <returns>Gets single/list of records</returns>
		[HttpGet, ActionName("GetModifiedCarrierAvlblAppointments")]
		public Models.Response GetModifiedCarrierAvlblAppointments(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				Models.ASMCarrierAviblApptInfo data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.ASMCarrierAviblApptInfo>(filter);

				if (data == null)
				{
					response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					response.Errors = "There was a problem with the data. Please refresh the data and try again. If this does not fix the problem, please contact technical support.";
					return response;
				}

				DModel.AMSCarrierResults CAppts = NGLDockSettingData.ModifyCarrierBookedAppointment(data.CarrierControl, data.CompControl, data.BookControl, data.SHID, data.EquipmentID, data.BookDateLoad, data.BookDateRequired, data.IsPickup, data.Inbound, data.Warehouse, data.CarrierName, data.IsDelete, data.BookAMSPickupApptControl, data.BookAMSDeliveryApptControl, data.CarrierNumber);
				if (!CAppts.blnMustRequestAppt && CAppts.AvailableSlots != null )
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
		/// This is used to save the EquipID and return result
		/// </summary>
		/// <param name="data">all Models Object.</param>
		/// <returns>Updated Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("SaveEquipmentIDForOrder")]
		public Models.Response SaveEquipmentIDForOrder([System.Web.Http.FromBody]Models.UpdateEquipID data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLBookData dalData = new DAL.NGLBookData(Parameters);

				if (data != null)
				{
                    ////DateTime DateLoad = data.order.BookDateLoad ?? DateTime.Now;
                    ////DateTime DateRequired = data.order.BookDateRequired ?? DateTime.Now;
                    //Modified By LVV 8/29/2018
                    //We can't set the dates to Date.Now because that will break the business logic. I don't think these dates are ever going to be null.
                    DateTime DateLoad = data.order.BookDateLoad.Value;
                    DateTime DateRequired = data.order.BookDateRequired.Value;

                    DModel.EquipIDValidation oData = selectDALDataEquipIDValidation(data.equipID);

					dalData.SaveEquipmentID(ref oData, data.order.BookCustCompControl, DateLoad, DateRequired, data.order.Inbound, data.order.IsTransfer);

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

		#endregion

	}
}