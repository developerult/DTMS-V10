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
	//Add resource and Copy resource settings by passing this class as parameter
	public class CopyResource
	{
		public int CopyToDockControl { get; set; }
		public int CopyFromDockControl { get; set; }
		public bool CopyToNew { get; set; }	  
		public string DockDoorID { get; set; }	
		public string DockDoorName { get; set; }
	}

	public class ChangeOverridesPwd
	{
		public int DockControl { get; set; }
		public string newPwd { get; set; }
		public string confirmPwd { get; set; }
	}
	/// <summary>
	/// AMSCompDockdoorController for All Dock Doors Rest API Controls
	/// </summary>
	/// <remarks>
	/// Created By SK on 29-05-18
	/// </remarks>
	public class AMSCompDockdoorController : NGLControllerBase
	{
		#region " Properties"
		/// <summary>
		/// This property is used for logging and error tracking.
		/// </summary>
		private string _SourceClass = "DynamicsTMS365.Controllers.AMSCompDockdoorController";
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
		private Models.AMSCompDockdoor selectModelData(DTO.CompDockDoor d)
		{
			Models.AMSCompDockdoor modelCompDockdoor = new Models.AMSCompDockdoor();
			List<string> skipCompDockdoor = new List<string> { "CompDockdoorUpdated" };
			string sMsg = "";
			modelCompDockdoor = (Models.AMSCompDockdoor)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompDockdoor, d, skipCompDockdoor, ref sMsg);
			if (modelCompDockdoor != null) { modelCompDockdoor.setUpdated(d.CompDockDoorUpdated.ToArray()); }
			return modelCompDockdoor;
		}

		private Models.AMSCompDockdoor selectBModelData(DTO.DTOBaseClass d)
		{
			Models.AMSCompDockdoor modelCompDockdoor = new Models.AMSCompDockdoor();
			List<string> skipCompDockdoor = new List<string> { "CompDockdoorUpdated" };
			string sMsg = "";
			modelCompDockdoor = (Models.AMSCompDockdoor)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompDockdoor, d, skipCompDockdoor, ref sMsg);
			return modelCompDockdoor;
		}

		/// <summary>
		/// Selecting a tblDockTimeCalcFactors Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns AMSCompDockTimeCalcFactor Type</returns>
		/// Modified by PKN on 6/20/18
		private Models.AMSCompDockTimeCalcFactor selectFromLTSData(LTS.tblDockTimeCalcFactor d)
		{
			Models.AMSCompDockTimeCalcFactor modelCompDockTimeCalcFactor = new Models.AMSCompDockTimeCalcFactor();
			List<string> skipCompDockTimeCalcFactor = new List<string> { "DockTCFUpdated", "CompDockDoor", "_CompDockDoor" };
			string sMsg = "";
			modelCompDockTimeCalcFactor = (Models.AMSCompDockTimeCalcFactor)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompDockTimeCalcFactor, d, skipCompDockTimeCalcFactor, ref sMsg);
			if (modelCompDockTimeCalcFactor != null) { modelCompDockTimeCalcFactor.setUpdated(d.DockTCFUpdated.ToArray()); }
			return modelCompDockTimeCalcFactor;
		}

		/// <summary>
		/// Selecting a tblDockTimeCalcFactors Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns AMSCompDockApptTimeSetting Type</returns>
		/// Modified by PKN on 6/20/18
		private Models.AMSCompDockApptTimeSetting selectApptTimeSettingFromLTSData(LTS.tblDockSetting d)
		{
			Models.AMSCompDockApptTimeSetting modelCompDockApptTimeSetting = new Models.AMSCompDockApptTimeSetting();
			List<string> skipCompDockApptTimeSetting = new List<string> { "DockSettingUpdated", "CompDockDoor", "_CompDockDoor" };
			string sMsg = "";
			modelCompDockApptTimeSetting = (Models.AMSCompDockApptTimeSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompDockApptTimeSetting, d, skipCompDockApptTimeSetting, ref sMsg);
			//if (modelCompDockApptTimeSetting != null) { modelCompDockApptTimeSetting.setUpdated(d.DockSettingUpdated.ToArray()); }
			return modelCompDockApptTimeSetting;
		}

		/// <summary>
		/// Selecting a tblDockBlockOutPeriod Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns tblDockBlockOutPeriod Type</returns>
		/// Modified by PKN on 7/11/18
		private Models.AMSCompDockBlockOutPeriod selectDockBlockOutPeriodFromLTSData(LTS.tblDockBlockOutPeriod d)
		{
			Models.AMSCompDockBlockOutPeriod modelCompDockBlockOutPeriods = new Models.AMSCompDockBlockOutPeriod();
			List<string> skipCompDockBlockOutPeriod = new List<string> { "DockBlockUpdated", "CompDockDoor", "_CompDockDoor" };
			string sMsg = "";
			modelCompDockBlockOutPeriods = (Models.AMSCompDockBlockOutPeriod)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompDockBlockOutPeriods, d, skipCompDockBlockOutPeriod, ref sMsg);
			//if (modelCompDockApptTimeSetting != null) { modelCompDockApptTimeSetting.setUpdated(d.DockSettingUpdated.ToArray()); }
			return modelCompDockBlockOutPeriods;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">DocumentType Model</param>
		/// <returns>returns tblEDIDocumentType table data</returns>
		/// Modified by SN on 2/21/18
		private DTO.CompDockDoor selectLTSData(Models.AMSCompDockdoor d)
		{
			DTO.CompDockDoor dtoCompDockdoor = new DTO.CompDockDoor();
			if (d != null)
			{
				List<string> skipCompDockdoor = new List<string> { "CompDockdoorUpdated" };
				string sMsg = "";
				dtoCompDockdoor = (DTO.CompDockDoor)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(dtoCompDockdoor, d, skipCompDockdoor, ref sMsg);
				if (dtoCompDockdoor != null)
				{
					byte[] bupdated = d.getUpdated();
					dtoCompDockdoor.CompDockDoorUpdated = bupdated == null ? new byte[0] : bupdated;
				}
			}
			return dtoCompDockdoor;
		}


		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">DocumentType Model</param>
		/// <returns>returns tblEDIDocumentType table data</returns>
		/// Modified by SN on 2/21/18
		private LTS.tblDockTimeCalcFactor selectFromLTSData(Models.AMSCompDockTimeCalcFactor d)
		{
			LTS.tblDockTimeCalcFactor ltsDockTimeCalcFactor = new LTS.tblDockTimeCalcFactor();
			if (d != null)
			{
				List<string> skipDockTimeCalcFactor = new List<string> { "DockTCFUpdated", "CompDockDoor", "_CompDockDoor" };
				string sMsg = "";
				ltsDockTimeCalcFactor = (LTS.tblDockTimeCalcFactor)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsDockTimeCalcFactor, d, skipDockTimeCalcFactor, ref sMsg);
				if (ltsDockTimeCalcFactor != null)
				{
					byte[] bupdated = d.getUpdated();
					ltsDockTimeCalcFactor.DockTCFUpdated = bupdated == null ? new byte[0] : bupdated;
				}
			}
			return ltsDockTimeCalcFactor;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">DockApptTime Model</param>
		/// <returns>returns tblDockSetting table data</returns>
		/// Modified by PKN on 7/4/18
		private Models.AMSCompDockApptTimeSetting selectDALApptTimeModelData(DModel.DockApptSettings d)
		{
			Models.AMSCompDockApptTimeSetting modeDockApptTime = new Models.AMSCompDockApptTimeSetting();
			List<string> skipDockApptTime = new List<string> { };
			string sMsg = "";
			modeDockApptTime = (Models.AMSCompDockApptTimeSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modeDockApptTime, d, skipDockApptTime, ref sMsg);
			//if (modelDocPTTypes != null) { modelDocPTTypes.setUpdated(d.DockTCFUpdated.ToArray()); }
			return modeDockApptTime;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">DocumentType Model</param>
		/// <returns>returns tblDockSetting table data</returns>
		/// Modified by PKN on 6/29/18
		private DModel.DockApptSettings selectFromApptTimeModelData(Models.AMSCompDockApptTimeSetting d)
		{
			DModel.DockApptSettings lstDockApptTimeSettings = new DModel.DockApptSettings();
			List<string> skipDockApptTimeSettings = new List<string> { "DockSettingUpdated" };
			string sMsg = "";
			lstDockApptTimeSettings = (DModel.DockApptSettings)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(lstDockApptTimeSettings, d, skipDockApptTimeSettings, ref sMsg);
			return lstDockApptTimeSettings;
		}


		#endregion

		#region "Rest Services"
		/// <summary>
		/// This is used to return a collection of Model record of Appointments
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recogyenized: filters.</remarks>
		[HttpGet, ActionName("GetRecords")]
		public Models.Response GetRecords(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{		
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                int count = 0;
                int RecordCount = 0;
                Models.AMSCompDockdoor[] amsCompDockdoor = new Models.AMSCompDockdoor[] { };
				DAL.NGLCompDockDoorData oAn = new DAL.NGLCompDockDoorData(Parameters);
				DTO.CompDockDoor[] ltsRet = oAn.GetCompDockDoorsFiltered(int.Parse(f.filterValue));
				if (ltsRet != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsCompDockdoor = (from e in ltsRet select selectModelData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsCompDockdoor, count);
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

        /// Created by PKN on 7/4/18
        /// <summary>
        /// This is used to add a new resource to the company
        /// and returns the dock door record with details
        /// </summary>
        /// <param name="Resource">The dock door record.</param>
        /// <returns>Adds Single record</returns>
        /// <remarks>
        /// The following data is also recognized: Model.
        /// Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
        ///  Added field CompDockInbound
        /// </remarks
        [HttpPost, ActionName("SaveNewCompResource")]
		public Models.Response SaveNewCompResource([System.Web.Http.FromBody]Models.AMSCompDockdoor Resource)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				Models.AMSCompDockdoor[] amsCompDockdoor = new Models.AMSCompDockdoor[] { };
				DAL.NGLCompDockDoorData oAn = new DAL.NGLCompDockDoorData(Parameters);
				DTO.CompDockDoor oData = selectLTSData(Resource);
				int CompControl = int.Parse(oData.CompDockCompControl.ToString());
				string DockDoorID = oData.CompDockDockDoorID.ToString();
				string DockDoorName = oData.CompDockDockDoorName.ToString();
				int BookingSeq = oData.CompDockBookingSeq;
				bool DockValidation = oData.CompDockValidation;
				bool OverrideAlert = oData.CompDockOverrideAlert;
				bool NotificationAlert = oData.CompDockNotificationAlert;
				string NotificationEmail = oData.CompDockNotificationEmail;
                bool DockInbound = oData.CompDockInbound;

				DTO.CompDockDoor oTData = oAn.AddNewResource365(CompControl, DockDoorID, DockDoorName, BookingSeq, DockValidation, OverrideAlert, NotificationAlert, NotificationEmail, DockInbound);
				Models.AMSCompDockdoor[] oRecords = new Models.AMSCompDockdoor[1];
				oRecords[0] = selectModelData(oData);

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

		/// Created by PKN on 7/5/18
		/// <summary>
		/// This is used to update resource details, appt time settings
		/// and returns the dock door record with details
		/// </summary>
		/// <param name="Resource">The dock door record.</param>
		/// <returns>Updates Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks
		[HttpPost, ActionName("UpdateCompResourceDetails")]
		public Models.Response UpdateCompResourceDetails([System.Web.Http.FromBody]Models.AMSCompDockdoor Resource)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				Models.AMSCompDockdoor[] amsCompDockdoor = new Models.AMSCompDockdoor[] { };
				DAL.NGLCompDockDoorData oAn = new DAL.NGLCompDockDoorData(Parameters);
				DTO.CompDockDoor oData = selectLTSData(Resource);
				Models.AMSCompDockdoor[] oRecords = new Models.AMSCompDockdoor[1];
				oRecords[0] = selectBModelData(oAn.UpdateRecord(oData));
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
		/// This is used to return true or false record of EDIMasterDocStructSegment which has deleted or not
		/// based on MDSSegControl using teststructloop class
		/// </summary>
		/// <param name="data">The testMstructsegment.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("DeleteCompResource")]
		public Models.Response DeleteCompResource([System.Web.Http.FromBody]Models.AMSCompDockdoor data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				Models.AMSCompDockdoor[] amsCompDockdoor = new Models.AMSCompDockdoor[] { };
				DAL.NGLCompDockDoorData oAn = new DAL.NGLCompDockDoorData(Parameters);
				DTO.CompDockDoor oData = selectLTSData(data);
				if (oData?.CompDockControl > 0) { oAn.DeleteRecord(oData); } else { result = false; }					
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
		/// This is used to created and/or copy resource settings and return a Model record which is Inserted 
		/// using sp name spCopyResourceSettingsResult
		/// </summary>
		/// <param name="CopyResource">The copyresource class having parameters of .</param>
		/// <param name="CopyToDockControl">The DockControl of resource to which resource settings has to be copied.</param>
		/// <param name="CopyFromDockControl">The DockControl of resource to which resource settings that has to be copied</param>
		/// <param name="CopyToNew">The boolean flag indicates to create new resource</param>
		/// <param name="DockDoorID">The Cock door ID.</param>
		/// <param name="DockDoorName">The Dock door Name.</param>
		/// <returns>Inserted Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("CopyResourceConfig")]
		public Models.Response CopyResourceConfig(CopyResource ResourceDtl)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				int CopyToDockControl = ResourceDtl.CopyToDockControl;
				int CopyFromDockControl = ResourceDtl.CopyFromDockControl;
				bool CopyToNew = ResourceDtl.CopyToNew;
				string DockDoorID = ResourceDtl.DockDoorID;
				string DockDoorName = ResourceDtl.DockDoorName;
				if (CopyToNew) { CopyToDockControl = 0; } else { DockDoorID = ""; DockDoorName = ""; }
				DAL.NGLCompDockDoorData dalData = new DAL.NGLCompDockDoorData(Parameters);
				LTS.spCopyResourceSettingsResult resourceList = dalData.CopyResourceSettings(CopyToDockControl, CopyFromDockControl, CopyToNew, DockDoorID, DockDoorName);
				if (resourceList.ErrNumber != 0) { result = false; }					
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
		/// This is used to return a collection of Model record of dock doors
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recogyenized: filters.</remarks>
		[HttpGet, ActionName("GetCopyToExisingDocks")]
		public Models.Response GetCopyToExisingDocks(string filter)
		{
			var response = new Models.Response(); //new HttpResponseMessage();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				DAL.NGLCompDockDoorData oLook = new DAL.NGLCompDockDoorData(Parameters);
				DTO.vLookupList[] lookup = new DTO.vLookupList[] { };
				int count = 0;
				if (f.CompControlFrom > 0 && !f.filterValue.Equals(""))
				{
					lookup = oLook.GetCopyToExisingDocks(f.CompControlFrom, int.Parse(f.filterValue));
					if (lookup != null && lookup.Count() > 0) { count = lookup.Count(); }
				}
				response = new Models.Response(lookup, count);
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
		/// This is used to return a collection of Model record of dock appt settings
		/// based on the dock door of company passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// Created by PKN on 6/29/18
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetDockApptTimeSettings")]
		public Models.Response GetDockApptTimeSettings(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				Models.AMSCompDockApptTimeSetting[] amsCompDockAppTimeSetting = new Models.AMSCompDockApptTimeSetting[] { };
				DAL.NGLDockSettingData oAn = new DAL.NGLDockSettingData(Parameters);
				DModel.DockApptSettings oTData = oAn.GetDockApptTimeSettings(int.Parse(f.filterValue));
				Models.AMSCompDockApptTimeSetting[] oRecords = new Models.AMSCompDockApptTimeSetting[1];
				oRecords[0] = selectDALApptTimeModelData(oTData);
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

		/// Created by PKN on 7/4/18
		/// <summary>
		/// This is used to save appt time setting details
		/// </summary>
		/// <param name="TimeSettingData">The dock door appt time setting record.</param>
		/// <returns>Adds Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks
		[HttpPost, ActionName("SaveDockApptTimeSettings")]
		public Models.Response SaveDockApptTimeSettings([System.Web.Http.FromBody]Models.AMSCompDockApptTimeSetting TimeSettingData)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				Models.AMSCompDockApptTimeSetting[] amsCompDockdoor = new Models.AMSCompDockApptTimeSetting[] { };
				DAL.NGLDockSettingData oAn = new DAL.NGLDockSettingData(Parameters);
				DModel.DockApptSettings oApptSettingData = selectFromApptTimeModelData(TimeSettingData);
				if(oApptSettingData?.DockControl > 0) { oAn.SaveDockApptTimeSettings(oApptSettingData); }					
				Models.AMSCompDockApptTimeSetting[] oRecords = new Models.AMSCompDockApptTimeSetting[1];
				//oRecords[0] = selectModelData(oData);
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
		/// This is used to return a collection of Model record of time calculation factor
		/// based on the dock door of company passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// Created by PKN on 6/20/18
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetDockTimeCalcFactors")]
		public Models.Response GetDockTimeCalcFactors(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{				
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                int count = 0;
				int RecordCount = 0;
				Models.AMSCompDockTimeCalcFactor[] amsCompDockTimeCalcFactors = new Models.AMSCompDockTimeCalcFactor[] { };
				DAL.NGLDockTimeCalcFactorData oAn = new DAL.NGLDockTimeCalcFactorData(Parameters);
				LTS.tblDockTimeCalcFactor[] ltsTimeCalcFactors = oAn.GetDockTimeCalcFactorsFiltered(ref RecordCount, f);
				if (ltsTimeCalcFactors != null && ltsTimeCalcFactors.Count() > 0)
				{
					count = ltsTimeCalcFactors.Count();
					amsCompDockTimeCalcFactors = (from e in ltsTimeCalcFactors select selectFromLTSData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsCompDockTimeCalcFactors, count);
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

		/// Created by PKN on 6/21/18
		/// <summary>
		/// This is used to return a Model record of time calc factor which has inserted
		/// </summary>
		/// <param name="dt">The dt.</param>
		/// <returns>Inserted Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("SaveDockTimeCalcFactor")]
		public Models.Response SaveDockTimeCalcFactor([System.Web.Http.FromBody]Models.AMSCompDockTimeCalcFactor dt)
		{
			var response = new Models.Response(); //new HttpResponseMessage();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLDockTimeCalcFactorData dalData = new DAL.NGLDockTimeCalcFactorData(Parameters);
				LTS.tblDockTimeCalcFactor oChanges = selectFromLTSData(dt);
				LTS.tblDockTimeCalcFactor oData = dalData.InsertOrUpdateDockTimeCalcFactor(oChanges);
				Models.AMSCompDockTimeCalcFactor[] oRecords = new Models.AMSCompDockTimeCalcFactor[1];
				oRecords[0] = selectFromLTSData(oData);
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
		/// This is used to return true or false record of EDIMasterDocStructSegment which has deleted or not
		/// based on DockTCFControl
		/// </summary>
		/// <param name="data">The testMstructsegment.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("DeleteDockTimeCalcFactor")]
		public Models.Response DeleteDockTimeCalcFactor([System.Web.Http.FromBody]Models.AMSCompDockTimeCalcFactor data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				DAL.NGLDockTimeCalcFactorData dalDockData = new DAL.NGLDockTimeCalcFactorData(Parameters);
				LTS.tblDockTimeCalcFactor oData = selectFromLTSData(data);
				if (oData?.DockTCFControl > 0) { dalDockData.DeleteDockTimeCalcFactor(oData.DockTCFControl); } else { result = false; }			
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
		/// This is used to return a collection of Model record of package types
		/// based on the dock door of company passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// Created by PKN on 6/25/18
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetSupportedPackageTypesForDock")]
		public Models.Response GetSupportedPackageTypesForDock(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{				
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                int count = 0;
				int RecordCount = 0;
                DAL.NGLDockSettingData oAn = new DAL.NGLDockSettingData(Parameters);
                DModel.DockPTType[] supportedDockPTTypes = new DModel.DockPTType[] { };
                supportedDockPTTypes = oAn.GetSupportedPackageTypesForDock(Int32.Parse(f.filterValue));
				if (supportedDockPTTypes != null && supportedDockPTTypes.Count() > 0)
				{
					count = supportedDockPTTypes.Count();
					if (RecordCount > count) { count = RecordCount; }
				}
                else
                {
                    supportedDockPTTypes = new DModel.DockPTType[] { };
                }
				response = new Models.Response(supportedDockPTTypes, count);
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
		/// This is used to return a collection of Model record of package types
		/// based on the dock door of company passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// Created by PKN on 6/25/18
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetEditablePackageTypesForDock")]
		public Models.Response GetEditablePackageTypesForDock(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				int count = 0;
				int RecordCount = 0;
                DAL.NGLDockSettingData oAn = new DAL.NGLDockSettingData(Parameters);
                DModel.DockPTType[] editableDockPTTypes = new DModel.DockPTType[] { };
                editableDockPTTypes = oAn.GetEditablePackageTypesForDock(Int32.Parse(f.filterValue));
				if (editableDockPTTypes != null && editableDockPTTypes.Count() > 0)
				{
					count = editableDockPTTypes.Count();
					if (RecordCount > count) { count = RecordCount; }
				}
                else { editableDockPTTypes = new DModel.DockPTType[] { }; }
				response = new Models.Response(editableDockPTTypes, count);
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

		/// Created by PKN on 6/27/18
		/// <summary>
		/// This is used to return a Model record of time calc factor which has inserted
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>Inserted Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("SaveDockPackageTypes")]
		public Models.Response SaveDockPackageTypes([FromBody]string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
            int[] arrPackageTypes = { 0 };
            var result = true;
			try
			{
				DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				DAL.NGLDockSettingData dalData = new DAL.NGLDockSettingData(Parameters);
                if (!f.Data.Equals("")) { arrPackageTypes = f.Data.Split(',').Select(str => int.Parse(str)).ToArray(); } else { arrPackageTypes = new int[] { }; }
                if (!(f.filterValue).Equals("")) { dalData.SavePackageTypeConfig(int.Parse(f.filterValue), arrPackageTypes); } else { result = false; }                   
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
		/// This is used to return a collection of Model record of package types
		/// based on the dock door of company passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// Created by PKN on 6/25/18
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetEditableTempTypesForDock")]
		public Models.Response GetEditableTempTypesForDock(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				int count = 0;
				int RecordCount = 0;
                DAL.NGLDockSettingData oAn = new DAL.NGLDockSettingData(Parameters);
                DModel.DockPTType[] editableDockETempTypes = new DModel.DockPTType[] { };
                editableDockETempTypes = oAn.GetEditableTempTypesForDock(Int32.Parse(f.filterValue));
				if (editableDockETempTypes != null && editableDockETempTypes.Count() > 0)
				{
					count = editableDockETempTypes.Count();
					if (RecordCount > count) { count = RecordCount; }
				}
                else { editableDockETempTypes = new DModel.DockPTType[] { }; }
				response = new Models.Response(editableDockETempTypes, count);
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
		/// This is used to return a collection of Model record of package types
		/// based on the dock door of company passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// Created by PKN on 6/25/18
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetSupportedTempTypesForDock")]
		public Models.Response GetSupportedTempTypesForDock(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				int count = 0;
				int RecordCount = 0;
                DAL.NGLDockSettingData oAn = new DAL.NGLDockSettingData(Parameters);
                DModel.DockPTType[] editableDockTempTypes = new DModel.DockPTType[] { };
				editableDockTempTypes = oAn.GetSupportedTempTypesForDock(Int32.Parse(f.filterValue));
				if (editableDockTempTypes != null && editableDockTempTypes.Count() > 0)
				{
					count = editableDockTempTypes.Count();
					if (RecordCount > count) { count = RecordCount; }
				}
                else { editableDockTempTypes = new DModel.DockPTType[] { }; }
				response = new Models.Response(editableDockTempTypes, count);
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


		/// Created by PKN on 7/3/18
		/// <summary>
		/// This is used to return a Model record of temp types which has inserted
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>Inserted Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("SaveDockTempTypes")]
		public Models.Response SaveDockTempTypes([FromBody]string filter)
		{
			var response = new Models.Response();
			int[] arrTemmpTypes = { 0 };
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				DAL.NGLDockSettingData dalData = new DAL.NGLDockSettingData(Parameters);
                if (!f.Data.Equals("")) { arrTemmpTypes = f.Data.Split(',').Select(str => int.Parse(str)).ToArray(); } else { arrTemmpTypes = new int[] { }; }                    
                if (!(f.filterValue).Equals("")) { dalData.SaveTempTypeConfig(int.Parse(f.filterValue), arrTemmpTypes); } else { result = false; }						
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
		/// This api method is used to return a collection of Model record for overrides based on
		/// Dock Door and company passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetOverrideSettingsForDock")]
		public Models.Response GetOverrideSettingsForDock(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				int count = 0;
				int RecordCount = 0;
				Models.AMSCompDockApptTimeSetting[] DockApptTimeSettings = new Models.AMSCompDockApptTimeSetting[] { };
				DAL.NGLDockSettingData oAn = new DAL.NGLDockSettingData(Parameters);
				LTS.tblDockSetting[] ltsRet = oAn.GetSettingsForDock(Int32.Parse(f.filterValue));
				if (ltsRet != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					DockApptTimeSettings = (from e in ltsRet select selectApptTimeSettingFromLTSData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(DockApptTimeSettings, count);
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

		/// Created by PKN on 7/4/18
		/// <summary>
		/// This api method is used to save override setting details for dock door and company
		/// </summary>
		/// <param name="OverridesData">The dock door override setting record.</param>
		/// <returns>Adds Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks
		[HttpPost, ActionName("UpdateOverrideSettingsForDock")]
		public Models.Response UpdateOverrideSettingsForDock([System.Web.Http.FromBody]Models.AMSCompDockApptTimeSetting OverridesData)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				Models.AMSCompDockApptTimeSetting[] amsCompDockdoor = new Models.AMSCompDockApptTimeSetting[] { };
				DAL.NGLDockSettingData oAn = new DAL.NGLDockSettingData(Parameters);
				DModel.DockApptSettings oCompDockOverrideSettingData = selectFromApptTimeModelData(OverridesData);
                if (oCompDockOverrideSettingData?.DockSettingControl > 0)
                {
                    oAn.UpdateOverrideSettings(oCompDockOverrideSettingData.DockSettingControl, oCompDockOverrideSettingData.DockSettingDescription, oCompDockOverrideSettingData.DockSettingOn, oCompDockOverrideSettingData.DockSettingRequireReasonCode, oCompDockOverrideSettingData.DockSettingRequireSupervisorPwd);
                }
                else { result = false; }
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

		/// Created by PKN on 7/4/18
		/// <summary>
		/// This api method is used to save override setting details for dock door and company
		/// </summary>
		/// <param name="OverridesData">The dock door override setting record.</param>
		/// <returns>Adds Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks
		[HttpPost, ActionName("ChangeOverridesPwdForDock")]
		public Models.Response ChangeOverridesPwdForDock(ChangeOverridesPwd OverridesPwdData)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				string strMsg = "";
				bool result = false;
				DAL.NGLCompDockDoorData oAn = new DAL.NGLCompDockDoorData(Parameters);
				if (OverridesPwdData != null && (OverridesPwdData.DockControl > 0))
				{
					result = oAn.ChangeOverridesPwd(ref strMsg, OverridesPwdData.DockControl, OverridesPwdData.newPwd, OverridesPwdData.confirmPwd);
					if (!result)
                    {
                        //Return the error message to the user
                        if (string.IsNullOrWhiteSpace(strMsg)) { strMsg = "A problem occured while attempting to change the Supervisor Override Password"; }
                        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                        response.Errors = strMsg;
                        return response;                  
                    }						
				}
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
		/// This api method is used to return a collection of Model record for block out periods based on
		/// Dock Door passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetDockBlockOutPeriodsByDock")]
		public Models.Response GetDockBlockOutPeriodsByDock(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                DAL.NGLDockBlockOutPeriodData oAn = new DAL.NGLDockBlockOutPeriodData(Parameters);
                List<Models.AMSCompDockBlockOutPeriod> list = new List<Models.AMSCompDockBlockOutPeriod>();
                Models.AMSCompDockBlockOutPeriod[] DockApptBlockOutPeriods = new Models.AMSCompDockBlockOutPeriod[] { };
                int count = 0;        
				DModel.RecurrenceEvent[] mRet = oAn.GetDockBlockOutPeriodsByDock(f.ParentControl);
                if (mRet?.Count() > 0)
                {
                    foreach (DModel.RecurrenceEvent evnt in mRet)
                    {
                        Models.AMSCompDockBlockOutPeriod block = new Models.AMSCompDockBlockOutPeriod();
                        int intRecurrenceID = 0;
                        int.TryParse(evnt.recurrenceId, out intRecurrenceID);
                        block.DockBlockControl = evnt.Id; //Map DockBlockControl to ID
                        block.DockControl = evnt.DockControl;
                        block.DockBlockExpired = evnt.DockBlockExpired;
                        block.DockBlockOn = evnt.DockBlockOn;
                        block.RecurrenceType = intRecurrenceID; //Map DockBlockRecurrenceType to recurrenceID
                        block.Title = evnt.Title;
                        block.Description = evnt.Description;
                        block.StartTime = evnt.StartDate; //StartDate = StartDate + StartTime
                        block.EndTime = evnt.EndDate; //EndDate = StartDate + EndTime
                        block.StartDate = evnt.StartDate;
                        block.Until = evnt.Rule.UNTIL;
                        block.Count = evnt.Rule.COUNT;
                        if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.SU)) { block.blnSun = true; } else { block.blnSun = false; }
                        if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.MO)) { block.blnMon = true; } else { block.blnMon = false; }
                        if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.TU)) { block.blnTue = true; } else { block.blnTue = false; }
                        if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.WE)) { block.blnWed = true; } else { block.blnWed = false; }
                        if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.TH)) { block.blnThu = true; } else { block.blnThu = false; }
                        if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.FR)) { block.blnFri = true; } else { block.blnFri = false; }
                        if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.SA)) { block.blnSat = true; } else { block.blnSat = false; }
                        list.Add(block);
                    }
                    count = list.Count;
                    DockApptBlockOutPeriods = list.ToArray();
                }
                response = new Models.Response(DockApptBlockOutPeriods, count);
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
		/// This is used to return a Model record of dock block out period details
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetDockBlockOutPeriodDetails")]
		public Models.Response GetDockBlockOutPeriodDetails(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);

                DAL.NGLDockBlockOutPeriodData oAn = new DAL.NGLDockBlockOutPeriodData(Parameters);
                Models.AMSCompDockBlockOutPeriod[] amsDockBlockOutPeriod = new Models.AMSCompDockBlockOutPeriod[] { };
                DModel.RecurrenceEvent evnt = new DModel.RecurrenceEvent();
                int count = 0;

                if (f.filterValue != "")
					evnt = oAn.GetDockBlockOutPeriod(int.Parse(f.filterValue));

				if (evnt != null)
				{
                    Models.AMSCompDockBlockOutPeriod db = new Models.AMSCompDockBlockOutPeriod();
                    count = 1;
                    int intRecurrenceID = 0;
                    int.TryParse(evnt.recurrenceId, out intRecurrenceID);
                  
                    db.DockBlockControl = evnt.Id; //Map DockBlockControl to ID
                    db.DockControl = evnt.DockControl;
                    db.DockBlockExpired = evnt.DockBlockExpired;
                    db.DockBlockOn = evnt.DockBlockOn;
                    db.RecurrenceType = intRecurrenceID; //Map DockBlockRecurrenceType to recurrenceID
                    db.Title = evnt.Title;
                    db.Description = evnt.Description;
                    db.StartTime = evnt.StartDate; //StartDate = StartDate + StartTime
                    db.EndTime = evnt.EndDate; //EndDate = StartDate + EndTime
                    db.StartDate = evnt.StartDate;
                    db.Until = evnt.Rule.UNTIL;
                    db.Count = evnt.Rule.COUNT;
                    if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.SU)) { db.blnSun = true; } else { db.blnSun = false; }
                    if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.MO)) { db.blnMon = true; } else { db.blnMon = false; }
                    if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.TU)) { db.blnTue = true; } else { db.blnTue = false; }
                    if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.WE)) { db.blnWed = true; } else { db.blnWed = false; }
                    if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.TH)) { db.blnThu = true; } else { db.blnThu = false; }
                    if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.FR)) { db.blnFri = true; } else { db.blnFri = false; }
                    if (evnt.Rule.BYDAY.Contains(DModel.RecurranceWeekday.SA)) { db.blnSat = true; } else { db.blnSat = false; }

                    amsDockBlockOutPeriod[0] = db;
                }
				response = new Models.Response(amsDockBlockOutPeriod, count);
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

		/// Created by PKN on 7/11/18
		/// <summary>
		/// This is used to return a Model record of dock block out period which has inserted
		/// </summary>
		/// <param name="dt">The dt.</param>
		/// <returns>Inserted Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("SaveDockApptBlockOutPeriod")]
		public Models.Response SaveDockApptBlockOutPeriod([System.Web.Http.FromBody]Models.AMSCompDockBlockOutPeriod dt)
		{
			var response = new Models.Response(); //new HttpResponseMessage();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLDockBlockOutPeriodData dalData = new DAL.NGLDockBlockOutPeriodData(Parameters);
                DModel.RecurrenceRule rule = new DModel.RecurrenceRule();
                rule.UNTIL = dt.Until;
                rule.COUNT = dt.Count;
                rule.resetBYDAYs(dt.blnSun, dt.blnMon, dt.blnTue, dt.blnWed, dt.blnThu, dt.blnFri, dt.blnSat);
                var startDate = dt.StartDate.Add(dt.StartTime.TimeOfDay); //StartDate = StartDate + StartTime
                var endDate = dt.StartDate.Add(dt.EndTime.TimeOfDay);     //EndDate = StartDate + EndTime
                DModel.RecurrenceEvent evnt = new DModel.RecurrenceEvent();
                evnt.Id = dt.DockBlockControl;
                evnt.Title = dt.Title;
                evnt.Description = dt.Description;
                evnt.StartDate = startDate;
                evnt.EndDate = endDate;
                evnt.recurrenceId = dt.RecurrenceType.ToString();
                evnt.Rule = rule;
                dalData.InsertOrUpdateDockBlockOutPeriod(dt.DockControl, dt.DockBlockOn, evnt);
                //The only time this sub will fail is if an exception is thrown which will be handled below
				Array d = new bool[1] { true };
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
		/// This is used to return true or false record of Dock Block out Period which has deleted or not
		/// based on DockBlockControl
		/// </summary>
		/// <param name="data">The testMstructsegment.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("DeleteDockApptBlockOutPeriod")]
		public Models.Response DeleteDockApptBlockOutPeriod([System.Web.Http.FromBody]Models.AMSCompDockBlockOutPeriod data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				DAL.NGLDockBlockOutPeriodData dalDockData = new DAL.NGLDockBlockOutPeriodData(Parameters);
				if (data?.DockBlockControl > 0) { dalDockData.DeleteDockBlockOutPeriod(data.DockBlockControl); } else { result = false; }
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
		/// This is used to return a collection of Model record of appt details fields
		/// based on the company passed to the method.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// Created by PKN on 7/16/18
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetApptDetailFieldsForComp")]
		public Models.Response GetApptDetailFieldsForComp(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				int count = 0;
				int RecordCount = 0;
                DAL.NGLCompData oAn = new DAL.NGLCompData(Parameters);
                DModel.DockPTType[] compApptDetailsFields = new DModel.DockPTType[] { };
				compApptDetailsFields = oAn.GetCompAMSApptDetailFields(Int32.Parse(f.filterValue));
				if (compApptDetailsFields?.Count() > 0)
				{
					count = compApptDetailsFields.Count();
					if (RecordCount > count) { count = RecordCount; }
				}
                else { compApptDetailsFields = new DModel.DockPTType[] { }; }
				response = new Models.Response(compApptDetailsFields, count);
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

		/// Created by PKN on 7/16/18
		/// <summary>
		/// This is used to return a Model record of appt details fiels after saving
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>Inserted Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("SaveApptDetailFieldForComp")]
		public Models.Response SaveApptDetailFieldForComp([FromBody]string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
            int[] arrApptDetailsFields = { 0 };
            var result = true;
			try
			{
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
				DAL.NGLCompData dalData = new DAL.NGLCompData(Parameters);
                if (!f.Data.Equals("")) { arrApptDetailsFields = f.Data.Split(',').Select(str => int.Parse(str)).ToArray(); } else { arrApptDetailsFields = new int[] { }; }
                if (!(f.filterValue).Equals("")) { dalData.SaveCompAMSApptDetailFields(int.Parse(f.filterValue), arrApptDetailsFields); } else { result = false; }                   
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

		#endregion
	}
}
