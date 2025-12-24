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
	/// AMSCompApptTrackingFieldController for All appointment tracking Fields Rest API Controls
	/// </summary>
	/// <remarks>
	/// Created By PKN on 15-06-18
	/// </remarks>
	public class AMSCompApptTrackingFieldController : NGLControllerBase
	{
		#region " Properties"
		/// <summary>
		/// This property is used for logging and error tracking.
		/// </summary>
		private string _SourceClass = "DynamicsTMS365.Controllers.AMSCompApptTrackingFieldController";
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
		/// Selecting a user field setting Model data by passing table records
		/// </summary>
		/// <param name="d">The table data.</param>
		/// <returns>Returns Appointment Tracking Field Settings</returns>
		private Models.AMSCompApptTrackingFieldSetting selectModelData(DTO.CompAMSApptTrackingSetting d)
		{
			Models.AMSCompApptTrackingFieldSetting modelCompApptTrackingFieldSetting = new Models.AMSCompApptTrackingFieldSetting();
			List<string> skipCompApptTrackingFieldSetting = new List<string> { "CompAMSApptTrackingSettingUpdated" };
			string sMsg = "";
			modelCompApptTrackingFieldSetting = (Models.AMSCompApptTrackingFieldSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompApptTrackingFieldSetting, d, skipCompApptTrackingFieldSetting, ref sMsg);
			if (modelCompApptTrackingFieldSetting != null) { modelCompApptTrackingFieldSetting.setUpdated(d.CompAMSApptTrackingSettingUpdated.ToArray()); }
			return modelCompApptTrackingFieldSetting;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">DocumentType Model</param>
		/// <returns>returns tblEDIDocumentType table data</returns>
		/// Modified by PKN on 15/06/18
		private DTO.CompAMSApptTrackingSetting selectLTSData(Models.AMSCompApptTrackingFieldSetting d)
		{
			DTO.CompAMSApptTrackingSetting dtoCompApptTrackingField = new DTO.CompAMSApptTrackingSetting();
			if (d != null)
			{
				List<string> skipCompApptTrackingField = new List<string> { "CompAMSUserFieldSettingUpdated" };
				string sMsg = "";
				dtoCompApptTrackingField = (DTO.CompAMSApptTrackingSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(dtoCompApptTrackingField, d, skipCompApptTrackingField, ref sMsg);
				if (dtoCompApptTrackingField != null)
				{
					byte[] bupdated = d.getUpdated();
					dtoCompApptTrackingField.CompAMSApptTrackingSettingUpdated = bupdated == null ? new byte[0] : bupdated;
				}
			}
			return dtoCompApptTrackingField;
		}

		private Models.AMSCompApptTrackingFieldSetting selectBModelData(DTO.DTOBaseClass d)
		{
			Models.AMSCompApptTrackingFieldSetting modelCompDockApptTrackingSetting = new Models.AMSCompApptTrackingFieldSetting();
			List<string> skipCompDockdoor = new List<string> { "DockSettingUpdated" };
			string sMsg = "";
			modelCompDockApptTrackingSetting = (Models.AMSCompApptTrackingFieldSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompDockApptTrackingSetting, d, skipCompDockdoor, ref sMsg);
			return modelCompDockApptTrackingSetting;
		}


		#endregion

		#region "Rest Services"
		/// <summary>
		/// This is used to return a collection of Model record of Comp appointment tracking field settings
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetCompApptTrackingFieldSettingRecords")]
		public Models.Response GetCompApptTrackingFieldSettingRecords(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{		
				DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                int count = 0;
                int RecordCount = 0;
                Models.AMSCompApptTrackingFieldSetting[] amsApptTrackingFieldSetting = new Models.AMSCompApptTrackingFieldSetting[] { };
				DAL.NGLCompAMSApptTrackingSettingData oAn = new DAL.NGLCompAMSApptTrackingSettingData(Parameters);
				DTO.CompAMSApptTrackingSetting[] ltsRet = oAn.GetCompAMSApptTrackingSettingsFiltered(f.CompControlFrom);
				if (ltsRet != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsApptTrackingFieldSetting = (from e in ltsRet select selectModelData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsApptTrackingFieldSetting, count);
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
		/// This is used to return a collection of Model record of appointment tracking field setting using GetRecordsByID
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetApptTrackingFieldSettingDetails")]
		public Models.Response GetApptTrackingFieldSettingDetails(string filter)
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

				Models.AMSCompApptTrackingFieldSetting[] amsApptTrackingFieldSetting = new Models.AMSCompApptTrackingFieldSetting[] { };
				DAL.NGLCompAMSApptTrackingSettingData oAn = new DAL.NGLCompAMSApptTrackingSettingData(Parameters);
				DTO.CompAMSApptTrackingSetting[] ltsRet = new DTO.CompAMSApptTrackingSetting[1];
				if(Fvalue != "")
				{
					ltsRet[0] = oAn.GetCompAMSApptTrackingSettingFiltered(int.Parse(Fvalue));
				}
				
				if (ltsRet[0] != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsApptTrackingFieldSetting = (from e in ltsRet
									   select selectModelData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsApptTrackingFieldSetting, count);
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
		/// This api method adds new appt tracking field record to tblDockSetting table
		/// and returns the model record
		/// </summary>
		/// <param name="data">The Model.</param>
		/// <returns>Insert Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("AddApptTrackingField")]
		public Models.Response AddApptTrackingField([System.Web.Http.FromBody]Models.AMSCompApptTrackingFieldSetting data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLCompAMSApptTrackingSettingData dalData = new DAL.NGLCompAMSApptTrackingSettingData(Parameters);
				DTO.CompAMSApptTrackingSetting oData = selectLTSData(data);

				oData = (DTO.CompAMSApptTrackingSetting)dalData.CreateRecord(oData);
				Models.AMSCompApptTrackingFieldSetting[] oUserFieldRecords = new Models.AMSCompApptTrackingFieldSetting[1];
				oUserFieldRecords[0] = selectModelData(oData);

				response = new Models.Response(oUserFieldRecords, 1);
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
		/// This api method is used to update user field details and return a Model record
		/// </summary>
		/// <param name="data">The Model.</param>
		/// <returns>Insert Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("UpdateApptTrackingField")]
		public Models.Response UpdateApptTrackingField([System.Web.Http.FromBody]Models.AMSCompApptTrackingFieldSetting data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLCompAMSApptTrackingSettingData dalData = new DAL.NGLCompAMSApptTrackingSettingData(Parameters);
				DTO.CompAMSApptTrackingSetting oData = selectLTSData(data);

				oData = (DTO.CompAMSApptTrackingSetting)dalData.UpdateRecord(oData);
				Models.AMSCompApptTrackingFieldSetting[] oUserFieldRecords = new Models.AMSCompApptTrackingFieldSetting[1];
				oUserFieldRecords[0] = selectModelData(oData);

				response = new Models.Response(oUserFieldRecords, 1);
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
		/// This is used to delete user field and return true or false indicating status
		/// </summary>
		/// <param name="data">The testMstructsegment.</param>
		/// <returns>deleted Single record</returns>
		[HttpPost, ActionName("DeleteApptTrackingField")]
		public Models.Response DeleteApptTrackingField([System.Web.Http.FromBody]Models.AMSCompApptTrackingFieldSetting data)
		{
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				DAL.NGLCompAMSApptTrackingSettingData dalCompUserField = new DAL.NGLCompAMSApptTrackingSettingData(Parameters);
				DTO.CompAMSApptTrackingSetting oData = selectLTSData(data);
				if (oData?.CompAMSApptTrackingSettingControl > 0) { dalCompUserField.DeleteRecord(oData); } else { result = false; }				
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
