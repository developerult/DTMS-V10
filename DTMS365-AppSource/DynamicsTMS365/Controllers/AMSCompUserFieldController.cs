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
	/// AMSCompUserFieldController for All User Fields Rest API Controls
	/// </summary>
	/// <remarks>
	/// Created By PKN on 15-06-18
	/// </remarks>
	public class AMSCompUserFieldController : NGLControllerBase
	{
		#region " Properties"
		/// <summary>
		/// This property is used for logging and error tracking.
		/// </summary>
		private string _SourceClass = "DynamicsTMS365.Controllers.AMSCompUserFieldController";
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
		private Models.AMSCompUserFieldSetting selectModelData(DTO.CompAMSUserFieldSetting d)
		{
			Models.AMSCompUserFieldSetting modelCompUserFieldSetting = new Models.AMSCompUserFieldSetting();
			List<string> skipCompUserFieldSetting = new List<string> { "CompAMSUserFieldSettingUpdated" };
			string sMsg = "";
			modelCompUserFieldSetting = (Models.AMSCompUserFieldSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelCompUserFieldSetting, d, skipCompUserFieldSetting, ref sMsg);
			if (modelCompUserFieldSetting != null) { modelCompUserFieldSetting.setUpdated(d.CompAMSUserFieldSettingUpdated.ToArray()); }
			return modelCompUserFieldSetting;
		}

		/// <summary>
		/// select table data by passing Model
		/// </summary>
		/// <param name="d">DocumentType Model</param>
		/// <returns>returns tblEDIDocumentType table data</returns>
		/// Modified by PKN on 15/06/18
		private DTO.CompAMSUserFieldSetting selectLTSData(Models.AMSCompUserFieldSetting d)
		{
			DTO.CompAMSUserFieldSetting dtoCompUserField = new DTO.CompAMSUserFieldSetting();
			if (d != null)
			{
				List<string> skipCompUserField = new List<string> { "CompAMSUserFieldSettingUpdated" };
				string sMsg = "";
				dtoCompUserField = (DTO.CompAMSUserFieldSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(dtoCompUserField, d, skipCompUserField, ref sMsg);
				if (dtoCompUserField != null)
				{
					byte[] bupdated = d.getUpdated();
					dtoCompUserField.CompAMSUserFieldSettingUpdated = bupdated == null ? new byte[0] : bupdated;
				}
			}
			return dtoCompUserField;
		}


		#endregion

		#region "Rest Services"
		/// <summary>
		/// This is used to return a collection of Model record of Comp user field settings
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns>An enumerable list of records</returns>
		/// <remarks>The following data is also recognized: filters.</remarks>
		[HttpGet, ActionName("GetCompUserFieldSettingRecords")]
		public Models.Response GetCompUserFieldSettingRecords(string filter)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
				int count = 0;
				int RecordCount = 0;
				Models.AMSCompUserFieldSetting[] amsUserFieldSetting = new Models.AMSCompUserFieldSetting[] { };
				DAL.NGLCompAMSUserFieldSettingData oAn = new DAL.NGLCompAMSUserFieldSettingData(Parameters);
				DTO.CompAMSUserFieldSetting[] ltsRet = oAn.GetCompAMSUserFieldSettingsFiltered(f.CompControlFrom);
				if (ltsRet?.Count() > 0)
				{
					count = ltsRet.Count();
					amsUserFieldSetting = (from e in ltsRet select selectModelData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsUserFieldSetting, count);
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



        [HttpGet, ActionName("GetCompUserFieldMaping")]
        public Models.Response GetCompUserFieldMaping(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                List<DynamicsTMS365.Models.CompUserFieldMap> mapTo = new List<DynamicsTMS365.Models.CompUserFieldMap>
                    {
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc =  "Custom",settingFieldMapName = "Custom"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrierContact",settingFieldMapName = "Carrier Contact"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrierContactPhone",settingFieldMapName = "Carrier Phone"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrVarDay",settingFieldMapName = "Carrier Drive Days"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrVarHrs",settingFieldMapName = "Carrier Drive Hours"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookWhseAuthorizationNo",settingFieldMapName = "Auth No"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrSealNo",settingFieldMapName = "Trailer Seal"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrDriverNo",settingFieldMapName = "Driver No"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrDriverName",settingFieldMapName = "Driver Name"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrRouteNo",settingFieldMapName = "Carrier Rt No"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookCarrTripNo",settingFieldMapName = "Carrier Trip No"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookChepGLID",settingFieldMapName = "Chep GL ID"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookNotesBookUser1",settingFieldMapName = "User 1"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookNotesBookUser2",settingFieldMapName = "User 2"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookNotesBookUser3",settingFieldMapName = "User 3"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookNotesBookUser4",settingFieldMapName = "User 4"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookOrigContactName",settingFieldMapName = "Orig Contact Name"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookOrigContactEmail",settingFieldMapName = "Orig Contact Email"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookOrigEmergencyContactPhone",settingFieldMapName = "Orig Emergency Phone"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookOrigEmergencyContactName",settingFieldMapName = "Orig Emergency Name"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookDestContactName",settingFieldMapName = "Dest Contact Name"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookDestContactEmail",settingFieldMapName = "Dest Contact Email"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookDestEmergencyContactPhone",settingFieldMapName = "Dest Emergency Phone"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookDestEmergencyContactName",settingFieldMapName = "Dest Emergency Name"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookShipCarrierProNumber",settingFieldMapName = "Carrier Pro Number"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookShipCarrierName",settingFieldMapName = "Assigned Carrier Name"},
                        new DynamicsTMS365.Models.CompUserFieldMap{ settingFieldMapDesc = "BookShipCarrierNumber",settingFieldMapName = "Assigned Carrier SCAC"}
                    };


                DynamicsTMS365.Models.CompUserFieldMap[] records = mapTo.ToArray();
                int count = records.Count();
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
        /// This is used to return a collection of Model record of user field setting using GetRecordsByID
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetUserFieldSettingDetails")]
		public Models.Response GetUserFieldSettingDetails(string filter)
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

				Models.AMSCompUserFieldSetting[] amsUserFieldSetting = new Models.AMSCompUserFieldSetting[] { };
				DAL.NGLCompAMSUserFieldSettingData oAn = new DAL.NGLCompAMSUserFieldSettingData(Parameters);
				DTO.CompAMSUserFieldSetting[] ltsRet = new DTO.CompAMSUserFieldSetting[1];
				if(Fvalue != "")
				{
					ltsRet[0] = oAn.GetCompAMSUserFieldSettingFiltered(int.Parse(Fvalue));
				}
				
				if (ltsRet[0] != null && ltsRet.Count() > 0)
				{
					count = ltsRet.Count();
					amsUserFieldSetting = (from e in ltsRet
									   select selectModelData(e)).ToArray();
					if (RecordCount > count) { count = RecordCount; }
				}
				response = new Models.Response(amsUserFieldSetting, count);
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
		/// This api method adds new user field record to tblDockSetting table
		/// and returns the model record
		/// </summary>
		/// <param name="data">The Model.</param>
		/// <returns>Insert Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("AddUserField")]
		public Models.Response AddUserField([System.Web.Http.FromBody]Models.AMSCompUserFieldSetting data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLCompAMSUserFieldSettingData dalData = new DAL.NGLCompAMSUserFieldSettingData(Parameters);
				DTO.CompAMSUserFieldSetting oData = selectLTSData(data);

				oData = (DTO.CompAMSUserFieldSetting)dalData.CreateRecord(oData);
				Models.AMSCompUserFieldSetting[] oUserFieldRecords = new Models.AMSCompUserFieldSetting[1];
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
		[HttpPost, ActionName("UpdateUserField")]
		public Models.Response UpdateUserField([System.Web.Http.FromBody]Models.AMSCompUserFieldSetting data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			try
			{
				DAL.NGLCompAMSUserFieldSettingData dalData = new DAL.NGLCompAMSUserFieldSettingData(Parameters);
				DTO.CompAMSUserFieldSetting oData = selectLTSData(data);

				oData = (DTO.CompAMSUserFieldSetting)dalData.UpdateRecord(oData);
				Models.AMSCompUserFieldSetting[] oUserFieldRecords = new Models.AMSCompUserFieldSetting[1];
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
		[HttpPost, ActionName("DeleteUserField")]
		public Models.Response DeleteUserField([System.Web.Http.FromBody]Models.AMSCompUserFieldSetting data)
		{
			var response = new Models.Response();
			if (!authenticateController(ref response)) { return response; }
			var result = true;
			try
			{
				DAL.NGLCompAMSUserFieldSettingData dalCompUserField = new DAL.NGLCompAMSUserFieldSettingData(Parameters);
				DTO.CompAMSUserFieldSetting oData = selectLTSData(data);
				if (oData != null && oData.CompAMSUserFieldSettingControl > 0) { dalCompUserField.DeleteRecord(oData); } else { result = false; }					
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
