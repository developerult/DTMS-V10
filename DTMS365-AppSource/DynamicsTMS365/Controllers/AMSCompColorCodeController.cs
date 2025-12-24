using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.Controllers
{
	/// <summary>
	/// AMSCompUserFieldController for All color code setting Rest API Controls
	/// </summary>
	/// <remarks>
	/// Created By PKN on 15-06-18
	/// </remarks>
	public class AMSCompColorCodeController : NGLControllerBase
	{
		#region " Properties"
		/// <summary>
		/// This property is used for logging and error tracking.
		/// </summary>
		private string _SourceClass = "DynamicsTMS365.Controllers.AMSCompColorCodeController";
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
		/// <returns>Returns User Field Settings</returns>
		private Models.AMSCompColorCodeSetting selectModelData(DTO.CompAMSColorCodeSetting d)
		{
			Models.AMSCompColorCodeSetting modelCompColorCodeSetting = new Models.AMSCompColorCodeSetting();
			List<string> skipCompColorCodeSetting = new List<string> { "CompAMSColorCodeSettingUpdated" };
			string sMsg = "";
			modelCompColorCodeSetting = (Models.AMSCompColorCodeSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompColorCodeSetting, d, skipCompColorCodeSetting, ref sMsg);
			if (modelCompColorCodeSetting != null) { modelCompColorCodeSetting.setUpdated(d.CompAMSColorCodeSettingUpdated.ToArray()); }
			return modelCompColorCodeSetting;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">DocumentType Model</param>
		/// <returns>returns tblEDIDocumentType table data</returns>
		/// Modified by PKN on 15/06/18
		private DTO.CompAMSColorCodeSetting selectLTSData(Models.AMSCompColorCodeSetting d)
		{
			DTO.CompAMSColorCodeSetting dtoCompColorCodeField = new DTO.CompAMSColorCodeSetting();
			if (d != null)
			{
				List<string> skipCompColorCodeSetting = new List<string> { "CompAMSColorCodeSettingUpdated" };
				string sMsg = "";
				dtoCompColorCodeField = (DTO.CompAMSColorCodeSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(dtoCompColorCodeField, d, skipCompColorCodeSetting, ref sMsg);
				if (dtoCompColorCodeField != null)
				{
					byte[] bupdated = d.getUpdated();
					dtoCompColorCodeField.CompAMSColorCodeSettingUpdated = bupdated == null ? new byte[0] : bupdated;
				}
			}
			return dtoCompColorCodeField;
		}


		#endregion

		#region "Rest Services"
		/// <summary>
		/// This is used to return a collection of Model record of Comp color code settings
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetCompColorCodeSettingRecords")]
		public Models.Response GetCompColorCodeSettingRecords(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{				
				DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                int count = 0;
                int RecordCount = 0;
                var Fname = f.filterName;
				var Fvalue = f.filterValue;
				var colorType = 0;
				if (Fname == "colorType") { colorType = Int32.Parse(Fvalue); }				
				Models.AMSCompColorCodeSetting[] amsColorCodeSetting = new Models.AMSCompColorCodeSetting[] { };
				DAL.NGLCompAMSColorCodeSettingData oAn = new DAL.NGLCompAMSColorCodeSettingData(Parameters);
				DTO.CompAMSColorCodeSetting[] ltsRet = oAn.GetCompAMSColorCodeSettingsFiltered(f.CompControlFrom, colorType);
				if (ltsRet != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsColorCodeSetting = (from e in ltsRet select selectModelData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsColorCodeSetting, count);
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
		/// This is used to return a collection of Model record of color code setting using GetRecordsByID
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetColorCodeSettingDetails")]
		public Models.Response GetColorCodeSettingDetails(string filter)
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

				Models.AMSCompColorCodeSetting[] amsColorCodeFieldSetting = new Models.AMSCompColorCodeSetting[] { };
				DAL.NGLCompAMSColorCodeSettingData oAn = new DAL.NGLCompAMSColorCodeSettingData(Parameters);
				DTO.CompAMSColorCodeSetting[] ltsRet = new DTO.CompAMSColorCodeSetting[1];
				if(Fvalue != "")
				{
					ltsRet[0] = oAn.GetCompAMSColorCodeSettingFiltered(int.Parse(Fvalue));
				}
				
				if (ltsRet[0] != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsColorCodeFieldSetting = (from e in ltsRet
									   select selectModelData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsColorCodeFieldSetting, count);
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
		/// This api method adds new appt status/order color field record to CompAMSColorCodeSetting table
		/// and returns the model record
		/// </summary>
		/// <param name="data">The Model.</param>
		/// <returns>Insert Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("AddApptColorCodeSetting")]
		public Models.Response AddApptColorCodeSetting([System.Web.Http.FromBody]Models.AMSCompColorCodeSetting data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLCompAMSColorCodeSettingData dalData = new DAL.NGLCompAMSColorCodeSettingData(Parameters);
				DTO.CompAMSColorCodeSetting oData = selectLTSData(data);

				oData = (DTO.CompAMSColorCodeSetting)dalData.CreateRecord(oData);
				Models.AMSCompColorCodeSetting[] oApptColorCodeRecords = new Models.AMSCompColorCodeSetting[1];
				oApptColorCodeRecords[0] = selectModelData(oData);

				response = new Models.Response(oApptColorCodeRecords, 1);
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
		[HttpPost, ActionName("UpdateApptColorCodeSetting")]
		public Models.Response UpdateApptColorCodeSetting([System.Web.Http.FromBody]Models.AMSCompColorCodeSetting data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLCompAMSColorCodeSettingData dalData = new DAL.NGLCompAMSColorCodeSettingData(Parameters);
				DTO.CompAMSColorCodeSetting oData = selectLTSData(data);

				oData = (DTO.CompAMSColorCodeSetting)dalData.UpdateRecord(oData);
				Models.AMSCompColorCodeSetting[] oApptColorCodeRecords = new Models.AMSCompColorCodeSetting[1];
				oApptColorCodeRecords[0] = selectModelData(oData);

				response = new Models.Response(oApptColorCodeRecords, 1);
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
		[HttpPost, ActionName("DeleteApptColorCodeSetting")]
		public Models.Response DeleteApptColorCodeSetting([System.Web.Http.FromBody]Models.AMSCompColorCodeSetting data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				DAL.NGLCompAMSColorCodeSettingData dalCompApptColorSetting = new DAL.NGLCompAMSColorCodeSettingData(Parameters);
				DTO.CompAMSColorCodeSetting oData = selectLTSData(data);
				if (oData?.CompAMSColorCodeSettingControl > 0) { dalCompApptColorSetting.DeleteRecord(oData); } else { result = false; }
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
