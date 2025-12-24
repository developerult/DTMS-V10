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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class CompController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 10/09/2018 
        /// initializes the Page property by calling the base class constructor
        /// Not sure if Utilities.PageEnum.CompanyMaint for the child data 
        /// Comp Contact will cause any trouble.  Some testing may be required
        /// </summary>
        public CompController()
                : base(Utilities.PageEnum.CompanyMaint)
	     {
        }

        #endregion
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CompController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        public static Models.Comp selectModelData(LTS.vLEComp365 d)
        {
            Models.Comp modelRecord = new Models.Comp();
            List<string> skipObjs = new List<string> { "CompModDate", "CompUpdated" };
            string sMsg = "";
            modelRecord = (Models.Comp)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
            if (modelRecord != null)
            {
                modelRecord.setUpdated(d.CompUpdated.ToArray());
                //Modified by RHR for v-8.5.4.005
                modelRecord.CompModDate = Utilities.convertDateToDateTimeString(d.CompModDate);
            }
            return modelRecord;
        }

        public static Models.Comp selectModelData(DTO.Comp d)
        {
            Models.Comp modelRecord = new Models.Comp();
            List<string> skipObjs = new List<string> { "CompModDate", "CompUpdated" };
            string sMsg = "";
            modelRecord = (Models.Comp)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
            if (modelRecord != null)
            {
                modelRecord.setUpdated(d.CompUpdated.ToArray());
                //Modified by RHR for v-8.5.4.005
                modelRecord.CompModDate = Utilities.convertDateToDateTimeString(d.CompModDate);
            }
            return modelRecord;
        }

        public static LTS.Comp selectLTSData(Models.Comp d)
        {
            LTS.Comp ltsRecord = new LTS.Comp();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CompModDate", "CompUpdated", "CompCals", "_CompCals", "CompConts", "_CompConts", "CompEDIs", "_CompEDIs", "CompGoals", "_CompGoals", "CompTracks", "_CompTracks", "CompParameters", "_CompParameters", "CompAMSApptTrackingSettings", "_CompAMSApptTrackingSettings", "CompAMSColorCodeSettings", "_CompAMSColorCodeSettings", "CompAMSUserFieldSettings", "_CompAMSUserFieldSettings", "CompDockdoors", "_CompDockdoors", "tblSubscriptionRequestPendings", "_tblSubscriptionRequestPendings"  };
                string sMsg = "";
                ltsRecord = (LTS.Comp)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CompUpdated = bupdated == null ? new byte[0] : bupdated;
                    ltsRecord.CompModDate = Utilities.convertStringToDateTime(d.CompModDate);

                }
            }
            return ltsRecord;
        }

        #endregion


        #region " REST Services"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.Comp[] records = new Models.Comp[] { };
                int count = 0;               
                DTO.Comp dtoComp = NGLCompData.GetCompFiltered(id);
                if (dtoComp != null)
                {
                    var comp = selectModelData(dtoComp);
                    records = new Models.Comp[] { comp };
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
            return GetLECompsFiltered(filter);
        }

        [HttpGet, ActionName("GetLECompsFiltered")]
        public Models.Response GetLECompsFiltered(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.Comp[] retComps = new Models.Comp[] { };
                LTS.vLEComp365[] vcomps = new LTS.vLEComp365[] { };
                int RecordCount = 0;
                int count = 0;
                vcomps = NGLCompData.GetLECompsFiltered(ref RecordCount, f);
                if (vcomps != null && vcomps.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    retComps = (from e in vcomps
                              orderby e.CompName ascending
                              select selectModelData(e)).ToArray();
                }
                response = new Models.Response(retComps, count);
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]Models.Comp data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLegalEntityAdminData oLEA = new DAL.NGLLegalEntityAdminData(Parameters);
                if (data.CompControl == 0)
                {
                    //INSERT NEW COMP
                    int compNumber = 0;
                    string validationMsg = "";
                    //ValidateCompBeforeInsert
                    if (!NGLCompData.ValidateCompBeforeInsert(ref compNumber, data.CompName, data.CompLegalEntity, data.CompAlphaCode, data.CompAbrev, ref validationMsg))
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = validationMsg;
                        return response;
                    }
                    //Insert the company record
                    LTS.Comp newComp = new LTS.Comp
                    {
                        CompNumber = compNumber,
                        CompActive = data.CompActive,
                        CompName = data.CompName,
                        CompLegalEntity = data.CompLegalEntity,
                        CompAlphaCode = data.CompAlphaCode,
                        CompAbrev = data.CompAbrev,
                        CompStreetAddress1 = data.CompStreetAddress1,
                        CompStreetAddress2 = data.CompStreetAddress2,
                        CompStreetAddress3 = data.CompStreetAddress3,
                        CompStreetCity = data.CompStreetCity,
                        CompStreetState = data.CompStreetState,
                        CompStreetCountry = data.CompStreetCountry,
                        CompStreetZip = data.CompStreetZip,
                        CompEmail = data.CompEmail,
                        CompWeb = data.CompWeb,
                        CompNatNumber = data.CompNatNumber, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompNatName = data.CompNatName, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompFAAShipID = data.CompFAAShipID, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompFDAShipID = data.CompFDAShipID, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompLatitude = data.CompLatitude, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompLongitude = data.CompLongitude, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompTypeCategory = data.CompTypeCategory, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompNEXTStopAcctNo = data.CompNEXTStopAcctNo, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompNEXTStopPsw = data.CompNEXTStopPsw, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompNextstopSubmitRFP = data.CompNextstopSubmitRFP, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompNEXTrack = data.CompNEXTrack, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompAMS = data.CompAMS, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompFinUseImportFrtCost = data.CompFinUseImportFrtCost, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompSilentTender = data.CompSilentTender, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompRestrictCarrierSelection = data.CompRestrictCarrierSelection, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompWarnOnRestrictedCarrierSelection = data.CompWarnOnRestrictedCarrierSelection, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompMailAddress1 = data.CompMailAddress1, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompMailAddress2 = data.CompMailAddress2, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompMailAddress3 = data.CompMailAddress3, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompMailCity = data.CompMailCity, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompMailState = data.CompMailState, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompMailZip = data.CompMailZip, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompMailCountry = data.CompMailCountry, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompRailStationName = data.CompRailStationName, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompRailSPLC = data.CompRailSPLC, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompRailFSAC = data.CompRailFSAC, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompRail333 = data.CompRail333, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompRailR260 = data.CompRailR260, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompUser1 = data.CompUser1, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompUser2 = data.CompUser2, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompUser3 = data.CompUser3, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompUser4 = data.CompUser4, //Added By LVV on 10/29/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
                        CompFinCurType = 1, //Hardcoded for now
                        CompModDate = DateTime.Now,
                        CompModUser = Parameters.UserName,
                        CompTimeZone = data.CompTimeZone
                    };
                    var ltsRetComp = NGLCompData.InsertComp(newComp);
                    data.CompControl = ltsRetComp.CompControl;
                    //Insert the company contact record
                    if (data.CompControl != 0)
                    { 
                        try
                        {
                            LTS.CompCont newCompCont = new LTS.CompCont { CompContCompControl = data.CompControl, CompContName = data.NewContName, CompContTitle = data.NewContTitle, CompCont800 = data.NewCont800, CompContPhone = data.NewContPhone, CompContPhoneExt = data.NewContPhoneExt, CompContFax = data.NewContFax, CompContEmail = data.NewContEmail, CompContTender = data.NewContTendered };
                            NGLCompContData.InsertOrUpdateCompCont365(newCompCont);
                            //Added By LVV on 4/15/20
                            var oRet = NGLCompData.AddVisibleCompForLEUsers(data.CompLegalEntity, compNumber, false);
                            if (oRet.Success == false) { SaveAppError("CreateNewLEComp Error: " + oRet.ErrMsg + " (source: InsertOrUpdateCompMaint)"); }
                        }
                        catch (Exception ex)
                        {
                            SaveAppError("CreateNewLEComp Error: " + ex.ToString() + " (source: InsertOrUpdateCompMaint)"); //log as system alert message but continue processing
                        }
                    
                        try
                        {
                            RecalculateLatLong(data.CompControl);
                        }
                        catch (Exception ex)
                        {
                            SaveAppError("RecalculateLatLong Error: " + ex.ToString() + " (source: InsertOrUpdateCompMaint)"); //log as system alert message but continue processing
                        }
                    }
                }
                else
                {
                    //UPDATE COMP
                    string validationMsg = "";
                    //ValidateCompBeforeUpdate
                    if (!NGLCompData.ValidateCompBeforeUpdate(data.CompControl, data.CompNumber, data.CompName, data.CompLegalEntity, data.CompAlphaCode, data.CompAbrev, ref validationMsg))
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = validationMsg;
                        return response;
                    }
                    LTS.Comp ltsComp = selectLTSData(data);
                    ltsComp.CompWeb = data.CompWeb;
                    NGLCompData.UpdateCompMaint365(ltsComp);
                    NGLBatchProcessData.UpdateLaneAddressWithCompany(ltsComp.CompControl);
                    //Added By LVV on 4/15/20
                    int compNumber = ltsComp.CompNumber.HasValue ? ltsComp.CompNumber.Value : 0;
                    var oRet = NGLCompData.AddVisibleCompForLEUsers(ltsComp.CompLegalEntity, compNumber, true);
                    if (oRet.Success == false) { SaveAppError("UpdateLEComp Error: " + oRet.ErrMsg + " (source: InsertOrUpdateCompMaint)"); }
                }
                //No errors so return success to caller
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

        //Depreciated - called by the CompMaintEAWndCtrl() widget which is no longer used.Now we do the Edit/Add in CM(Post method)
        [HttpPost, ActionName("InsertOrUpdateCompMaint")]
        public Models.Response InsertOrUpdateCompMaint([System.Web.Http.FromBody]Models.CompMaint cm)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLegalEntityAdminData oLEA = new DAL.NGLLegalEntityAdminData(Parameters);
                if (cm.Company.CompControl == 0)
                {
                    //INSERT NEW COMP
                    int compNumber = 0;
                    string validationMsg = "";
                    //ValidateCompBeforeInsert
                    if (!NGLCompData.ValidateCompBeforeInsert(ref compNumber, cm.Company.CompName, cm.Company.CompLegalEntity, cm.Company.CompAlphaCode, cm.Company.CompAbrev, ref validationMsg))
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = validationMsg;
                        return response;
                    }
                    //Insert the company record
                    LTS.Comp newComp = new LTS.Comp
                    {
                        CompNumber = compNumber,
                        CompActive = cm.Company.CompActive,
                        CompName = cm.Company.CompName,
                        CompLegalEntity = cm.Company.CompLegalEntity,
                        CompAlphaCode = cm.Company.CompAlphaCode,
                        CompAbrev = cm.Company.CompAbrev,
                        CompStreetAddress1 = cm.Company.CompStreetAddress1,
                        CompStreetAddress2 = cm.Company.CompStreetAddress2,
                        CompStreetAddress3 = cm.Company.CompStreetAddress3,
                        CompStreetCity = cm.Company.CompStreetCity,
                        CompStreetState = cm.Company.CompStreetState,
                        CompStreetCountry = cm.Company.CompStreetCountry,
                        CompStreetZip = cm.Company.CompStreetZip,
                        CompEmail = cm.Company.CompEmail,
                        CompWeb = cm.Company.CompWeb,
                        CompFinCurType = 1, //Hardcoded for now
                        CompModDate = DateTime.Now,
                        CompModUser = Parameters.UserName
                    };
                    var ltsRetComp = NGLCompData.InsertComp(newComp);
                    cm.Company.CompControl = ltsRetComp.CompControl;
                    //Insert the company contact record
                    try
                    {
                        LTS.CompCont newCompCont = new LTS.CompCont { CompContCompControl = cm.Company.CompControl, CompContName = cm.CompanyContact.CompContName, CompContTitle = cm.CompanyContact.CompContTitle, CompCont800 = cm.CompanyContact.CompCont800, CompContPhone = cm.CompanyContact.CompContPhone, CompContPhoneExt = cm.CompanyContact.CompContPhoneExt, CompContFax = cm.CompanyContact.CompContFax, CompContEmail = cm.CompanyContact.CompContEmail, CompContTender = cm.CompanyContact.CompContTender };
                        NGLCompContData.InsertOrUpdateCompCont365(newCompCont);
                        //Added By LVV on 4/15/20
                        var oRet = NGLCompData.AddVisibleCompForLEUsers(cm.Company.CompLegalEntity, compNumber, false);
                        if (oRet.Success == false) { SaveAppError("CreateNewLEComp Error: " + oRet.ErrMsg + " (source: InsertOrUpdateCompMaint)"); }
                    }
                    catch (Exception ex)
                    {                        
                        SaveAppError("CreateNewLEComp Error: " + ex.ToString() + " (source: InsertOrUpdateCompMaint)"); //log as system alert message but continue processing
                    }
                }
                else
                {
                    //UPDATE COMP
                    string validationMsg = "";
                    //ValidateCompBeforeUpdate
                    if (!NGLCompData.ValidateCompBeforeUpdate(cm.Company.CompControl, cm.Company.CompNumber,cm.Company.CompName, cm.Company.CompLegalEntity, cm.Company.CompAlphaCode, cm.Company.CompAbrev, ref validationMsg))
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = validationMsg;
                        return response;
                    }
                    LTS.Comp ltsComp = selectLTSData(cm.Company);
                    ltsComp.CompWeb = cm.Company.CompWeb;
                    NGLCompData.UpdateCompMaint365(ltsComp);
                    NGLBatchProcessData.UpdateLaneAddressWithCompany(ltsComp.CompControl);
                    //Added By LVV on 4/15/20
                    int compNumber = ltsComp.CompNumber.HasValue ? ltsComp.CompNumber.Value : 0;
                    var oRet = NGLCompData.AddVisibleCompForLEUsers(ltsComp.CompLegalEntity, compNumber, true);
                    if (oRet.Success == false) { SaveAppError("UpdateLEComp Error: " + oRet.ErrMsg + " (source: InsertOrUpdateCompMaint)"); }
                }
                //No errors so return success to caller
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

        //Not sure if this is used anywhere?
        //GETS A LOOKUPLIST OF THE LECOMPS
        [HttpGet, ActionName("GetLECompsList")]
        public Models.Response GetLECompsList(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] vcomps = new DTO.vLookupList[] { };
                vcomps = NGLCompData.GetLECompsList(id);
                response = new Models.Response(vcomps, vcomps.Length);
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

        //GETS A LOOKUPLIST OF THE LECOMPS
        [HttpGet, ActionName("GetLEComps")]
        public Models.Response GetLEComps(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters() { LEAdminControl = id };
                Models.Comp[] retComps = new Models.Comp[] { };
                LTS.vLEComp365[] vcomps = new LTS.vLEComp365[] { };
                int RecordCount = 0;
                int count = 0;
                vcomps = NGLCompData.GetLECompsFiltered(ref RecordCount, f);
                if (vcomps != null && vcomps.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    retComps = (from e in vcomps
                                orderby e.CompName ascending
                                select selectModelData(e)).ToArray();
                }
                response = new Models.Response(retComps, count);
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


        [HttpDelete, ActionName("Delete")]
        public Models.Response Delete(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                NGLCompData.DeleteComp365(id);
                bool[] oRecords = new bool[1] { true };
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


        [HttpGet, ActionName("RecalculateLatLong")]
        public Models.Response RecalculateLatLong(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                DAL.Models.ResultObject result = bll.RecalculateCompLatLong(id);
                //DAL.Models.ResultObject result = bll.CalculateAndSaveMilesLatLong(id,1);
                if (result == null) { result = new DAL.Models.ResultObject(); }
                Array d = new DAL.Models.ResultObject[1] { result };
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


        [HttpGet, ActionName("GetCompanySummary")]
        public Models.Response GetCompanySummary(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyMaint); } 
                LTS.vCompFin[] records = new LTS.vCompFin[] { };
                DModel.AllFilters f = new DModel.AllFilters();
                addToFilters(ref f, "CompFinCompControl", id.ToString());
                int RecordCount = 0;
                LTS.vCompFin[] oData = NGLCompCreditData.GetLECompsFins(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    records = new LTS.vCompFin[] { oData[0] };
                }
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




        //Note - I don't think this is being used anywhere as of 10/18/20
        [HttpPost, ActionName("SavePayableTolerances")]
        public Models.Response SavePayableTolerances(Models.Comp toleranceData)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyMaint);
                f.LEAdminControl = Parameters.UserLEControl;
                Models.Comp[] retComps = new Models.Comp[1] ;
                LTS.vLEComp365[] vcomps = new LTS.vLEComp365[] { };
                LTS.Comp compObj = new LTS.Comp();
                int RecordCount = 0;
                int count = 0;
                vcomps = NGLCompData.GetLECompsFiltered(ref RecordCount, f).Where(x => x.CompControl == f.ParentControl).ToArray();
                if (vcomps != null && vcomps.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    retComps = (from e in vcomps
                                orderby e.CompName ascending
                                select selectModelData(e)).ToArray();

                    compObj = (from e in retComps
                               orderby e.CompName ascending
                               select selectLTSData(e)).FirstOrDefault();
                }
                compObj.CompPayTolPerLo = toleranceData.CompPayTolPerLo;
                compObj.CompPayTolPerHi = toleranceData.CompPayTolPerHi;
                compObj.CompPayTolCurLo = toleranceData.CompPayTolCurLo;
                compObj.CompPayTolCurHi = toleranceData.CompPayTolCurHi;
                NGLCompData.SaveCompTolerances(compObj);
                response = new Models.Response(retComps, count);
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