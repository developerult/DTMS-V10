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
    public class TariffFuelAddendumController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public TariffFuelAddendumController()
                : base(Utilities.PageEnum.TariffFuel)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.TariffFuelAddendumController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.CarrierFuelAddendum selectModelData(LTS.CarrierFuelAddendum d)
        {
            Models.CarrierFuelAddendum modelRecord = new Models.CarrierFuelAddendum();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrFuelAdUpdated","CarrierFuelAdExes","CarrierFuelAdRates", "Carrier" };
                string sMsg = "";
                modelRecord = (Models.CarrierFuelAddendum)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
               if (modelRecord != null) { modelRecord.setUpdated(d.CarrFuelAdUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.CarrierFuelAddendum selectLTSData(Models.CarrierFuelAddendum d)
        {
            LTS.CarrierFuelAddendum ltsRecord = new LTS.CarrierFuelAddendum();
            if (d != null)
            {
                //List<string> skipObjs = new List<string> { "CarrTarUpdated", "CarrierTariffBreakPoints", "CarrierTariffMatrixes", "CarrierTariffDiscounts", "CarrierTariffFees", "CarrierTariffInterlines", "CarrierTariffMinCharges", "CarrierTariffNonServices", "CarrierTariffEquipments", "CarrierTariffMatrixBPs", "CarrierTariffClassXrefs", "CarrierTariffNoDriveDays", "CarrierTariffMinWeights", "CompRefCarrier", "Carrier" };
                List<string> skipObjs = new List<string> { "CarrFuelAdUpdated", "CarrierFuelAdExes", "CarrierFuelAdRates","Carrier" };
                string sMsg = "";
                ltsRecord = (LTS.CarrierFuelAddendum)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrFuelAdUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }

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
            //Note: The id must always match a CarrFuelAdControl associated with the select tariff 
            //the system looks up the last saved tariff pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "CarrFuelAdControl";
                    f.filterValue = id.ToString();
                }
                //get the parent control
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DAL.NGLCarrierFuelAddendumData oDAL = new DAL.NGLCarrierFuelAddendumData(Parameters);
                Models.CarrierFuelAddendum[] records = new Models.CarrierFuelAddendum[] { };
                LTS.CarrierFuelAddendum[] oData = new LTS.CarrierFuelAddendum[] { };
                oData = oDAL.GetCarrierTariffFuelAddendum(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {                   
                    //This controller only returns one record for each tariff so just save the first records control number as
                    //the primary key,  to be used by dependent actions later
                    bool blnSavePK = savePagePrimaryKey(Parameters, this.Page, oData[0].CarrFuelAdControl);
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
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
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DAL.NGLCarrierFuelAddendumData oDAL = new DAL.NGLCarrierFuelAddendumData(Parameters);
                Models.CarrierFuelAddendum[] records = new Models.CarrierFuelAddendum[] { };
                LTS.CarrierFuelAddendum[] oData = new LTS.CarrierFuelAddendum[] { };
                oData = oDAL.GetCarrierTariffFuelAddendum(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
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

        //[HttpGet, ActionName("CreateNewFuelAddendumFromTemplate")]
        //public Models.Response CreateNewFuelAddendumFromTemplate(string filter)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.3.0.001 on 09/21/2020 
        ///   fixed bug where the wrong value for new carrier control is being passed to the DAL/stored procedure
        ///   Note that iCarrierControl represents the active carrier selected and not the source addendum 
        ///   When updating the data from the Tariff the filter holds the CarrTarControl and iCarrierControl
        ///   is zero,  Note that an update to the stored procedure will lookup hte correct value for iCarrierControl
        ///   When we are using the CarrTarControl and iCarrierControl is zero.
        /// </remarks>
        [HttpPost, ActionName("CreateNewFuelAddendumFromTemplate")]
        public Models.Response CreateNewFuelAddendumFromTemplate([System.Web.Http.FromBody]string filter)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.GenericResult oGResult = new Models.GenericResult() { Control = 0, strField = "undefined" };
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
               DAL.NGLCarrierFuelAddendumData oDLL = new DAL.NGLCarrierFuelAddendumData(Parameters);
                int iCarrTarControl = 0;
                int iSourceCarrierControl = 0;
                int iSourceCarrFuelAdControl = 0; //Modified by RHR for v-8.3.0.001 on 09/21/2020 
                int iCarrierControl = 0; //Modified by RHR for v-8.3.0.001 on 09/21/2020 
                int.TryParse(filter, out iCarrTarControl);
                if (iCarrTarControl == 0)
                {
                    iCarrTarControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                }
                bool blnUserDefaultTempalate = false;
                //get the work flow settings
                LTS.tblUserPageSetting[] wdgtUPS = oUPS.GetPageSettingsForCurrentUser((int)Utilities.PageEnum.Tariff, "wdgtNewFuelAdTemplWorkFlowOptionCtrlEdit");
                if (wdgtUPS != null && wdgtUPS.Count() > 0){                
                    Models.WorkFlowSetting[] wfs = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.WorkFlowSetting[]>(wdgtUPS[0].UserPSMetaData);
                    //find out which thing is checked - perform the action based on that
                    //Json OBJECT LOOKS LIKE '{"fieldID":"3273","fieldName":"RateITWorkFlowSwitchSpotRate","fieldDefaultValue":"true","fieldVisible":"true","fieldReadOnly":"false"}'
                    //NGLWorkFlowGroups must be visible Switches must have a default value of "true" text
                    if (wfs != null)
                    {
                        foreach (Models.WorkFlowSetting stg in wfs)
                        {
                            if (stg.fieldName == "ynSelectCarrierFuelAsTemplate")
                            {
                                if (!string.IsNullOrWhiteSpace(stg.fieldDefaultValue) && stg.fieldDefaultValue.ToString().ToLower() == "true")
                                {
                                    blnUserDefaultTempalate = false;
                                }
                                else
                                {
                                    blnUserDefaultTempalate = true;
                                }
                                break;
                            }
                        }
                    }

                }
                if (blnUserDefaultTempalate == false)
                {
                    //use the FuelAdTemplOptions data for carriercontrol stored in NumericValue2
                    LTS.tblUserPageSetting[] oFuelAdOPtUPS = oUPS.GetPageSettingsForCurrentUser((int)Utilities.PageEnum.TariffFuel, "FuelAdTemplOptions");
                    Models.vGenericPageSettingTemplate oGPageSet = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.vGenericPageSettingTemplate>(oFuelAdOPtUPS[0].UserPSMetaData);
                    if (oGPageSet != null) { 
                        int.TryParse(oGPageSet.NumericValue2.ToString(), out iSourceCarrierControl);
                    }
                    //Modified by RHR for v-8.3.0.001 on 09/21/2020 lookup the source fuel addendum using the selected carrier control
                    //If UseGlobalStdFuelAddendumCarrier = False And NewCarrierControl <> 0 And SourceCarrFuelAdControl = 0 Then
                    //'look up the fuel addendum using the carriercontrol
                    DTO.CarrierFuelAddendum[] oFAs = oDLL.GetCarrierFuelAddendumsFiltered(iSourceCarrierControl);
                    if (oFAs != null && oFAs.Count() > 0)
                    {
                        iSourceCarrFuelAdControl = oFAs[0].CarrFuelAdControl;
                    } else
                    {
                        response.Errors = Utilities.getLocalizedMsg("FillFromTemplateInvalid");
                        return response; 
                    }
                        
                    
                }
                int iRet = oDLL.CreateNewFuelAddendumFromTemplate(iCarrierControl, iCarrTarControl, 0, blnUserDefaultTempalate, iSourceCarrFuelAdControl, Parameters.UserName);
                bool[] oRecords = new bool[1] { true };
                if (iRet == 0)
                {
                    response.Errors = Utilities.getLocalizedMsg("E_CannotSaveNoData");
                    oRecords[0] = false;
                }          
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

        [HttpPost, ActionName("UpdateCarrierFuelFeesByContract")]
        public Models.Response UpdateCarrierFuelFeesByContract([System.Web.Http.FromBody]string filter)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.GenericResult oGResult = new Models.GenericResult() { Control = 0, strField = "undefined" };
                int iCarrTarControl = 0;
                int.TryParse(filter, out iCarrTarControl);
                if (iCarrTarControl == 0)
                {
                    iCarrTarControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                }

                BLL.NGLCarrierBLL oBLL = new BLL.NGLCarrierBLL(Parameters);
                DTO.WCFResults oRet = oBLL.UpdateCarrierFuelFeesByContract(iCarrTarControl);
              
                response.Errors = Utilities.formatWCFResultErrors(oRet);
                response.Warnings = Utilities.formatWCFResultWarnings(oRet);
                response.Messages = Utilities.formatWCFResultMessages(oRet);
                response.StatusCode = HttpStatusCode.OK;
                bool[] oRecords = new bool[1] { false };
                if (oRet.Success)
                {
                    oRecords[0] = true;
                }              
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


        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.CarrierFuelAddendum data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrierFuelAddendumData oDAL = new DAL.NGLCarrierFuelAddendumData(Parameters);
                if (data.CarrFuelAdCarrTarControl == 0)
                {
                    //get the parent control
                    data.CarrFuelAdCarrTarControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);

                }
                LTS.CarrierFuelAddendum oChanges = selectLTSData(data);
                bool blnRet = oDAL.SaveCarrierTariffFuelAddendum(oChanges);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
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


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrierFuelAddendumData oDAL = new DAL.NGLCarrierFuelAddendumData(Parameters);
                bool blnRet = oDAL.DeleteCarrierFuelAddendum(id);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
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