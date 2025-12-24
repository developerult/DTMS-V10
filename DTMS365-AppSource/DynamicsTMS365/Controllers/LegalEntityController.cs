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

namespace DynamicsTMS365.Controllers
{
    public class LegalEntityController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// // Modified by RHR for v-8.2.1.006 on 04/15/2020 added logic to initializes the Page property by calling the base class constructor
        /// </summary>
        public LegalEntityController()
                : base(Utilities.PageEnum.LegalEntitiyMaint)
	     {
        }

        #endregion


        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LegalEntityController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " REST Services"

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
            return GetLEAdminsFiltered(filter);
        }

        /// <summary>
        /// Get the Legal Entity records visible to the active user
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Modified by RHR for v-8.5.3.007 on 03/09/2023
        ///     added filter by user for LEAdminControl
        /// </remarks>
        [HttpGet, ActionName("GetLEAdminsFiltered")]
        public Models.Response GetLEAdminsFiltered(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "LEAdminGridFltr"); } //save the page filter for the next time the page loads
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
               
                if (Parameters.CatControl != 4)
                {
                    DAL.Models.FilterDetails oDetail = new DAL.Models.FilterDetails();
                    oDetail.filterName = "LEAdminControl";
                    oDetail.filterValueFrom = Parameters.UserLEControl.ToString();
                    f.addFilterDetail(oDetail, true);
                }
                LTS.vLegalEntityAdmin[] oData = new LTS.vLegalEntityAdmin[] { };                
                int RecordCount = 0;
                int count = 0;
                oData = NGLLegalEntityAdminData.GetLEAdminsFiltered(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(oData, count);
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

        [HttpDelete, ActionName("DeleteLegalEntity")]
        public Models.Response DeleteLegalEntity(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLegalEntityAdminData oLEA = new DAL.NGLLegalEntityAdminData(Parameters);

                var blnRet = oLEA.DeleteLegalEntityAdmin(id);

                if (blnRet)
                {
                    Array d = new bool[1] { blnRet };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    //This should never happen because the only reason it would return false is if an exception was thrown and those get caught below
                    //** TODO LVV ** Localize these messages. Also, figure out better way to do this
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the tblLegalEntityAdmin record with Control {0}", id.ToString());
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


        //TODO eventually - Change code that calls this to call the method above
        [HttpGet, ActionName("GetLegalEntityAdmins")]
        public Models.Response GetLegalEntityAdmins()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vLegalEntityAdmin[] retFees = new LTS.vLegalEntityAdmin[] { };
                DAL.NGLLegalEntityAdminData oLEA = new DAL.NGLLegalEntityAdminData(Parameters);
                int count = 0;

                //**TODO LVV** ADD LOGIC TO CHECK IF THE USER HAS ROLE CENTER PERMISSIONS TO RUN THIS PROCEDURE

                retFees = oLEA.GetLegalEntityAdmins();
                if (retFees != null && retFees.Count() > 0)
                {
                    count = retFees.Length;
                }

                response = new Models.Response(retFees, count);
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


        [HttpGet, ActionName("GetLEAdmin")]
        public Models.Response GetLEAdmin(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vLegalEntityAdmin retVal = new LTS.vLegalEntityAdmin { };
                DAL.NGLLegalEntityAdminData oLEA = new DAL.NGLLegalEntityAdminData(Parameters);

                //If LEAdminControl = 0 then use the Users LEControl
                if (id == 0)
                {
                    id = Parameters.UserLEControl;
                }

                retVal = oLEA.GetLegalEntityAdmin(id);

                Array d = new LTS.vLegalEntityAdmin[1] { retVal };
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
        /// Saves chages to Legal Entity Admin details, creates a new record if the primary key is zero
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.2.006 on 03/02/2023 added support for LEAdminSecurityLevel 
        /// </remarks>
        [HttpPost, ActionName("InsertOrUpdateLegalEntity")]
        public Models.Response InsertOrUpdateLegalEntity([System.Web.Http.FromBody]Models.InsertOrUpdateLE g)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.NGLLegalEntityAdminData oLEA = new DAL.NGLLegalEntityAdminData(Parameters);

                if (g.LEAdminControl == 0)
                {
                    //INSERT NEW LEGAL ENTITY
                    if (g.CompControl == 0)
                    {
                        //ON INSERT NEW COMP
                        int compNumber = 0;
                        string validationMsg = "";

                        //Validate Legal Entity is not already being used
                        if (oLEA.DoesLegalEntityAdminExist(g.LegalEntity))
                        {
                            response.StatusCode = HttpStatusCode.InternalServerError;
                            response.Errors = "The Legal Entity '" + g.LegalEntity + "' already exists.";
                            return response;
                        }
                        //ValidateCompBeforeInsert
                        if (!NGLCompData.ValidateCompBeforeInsert(ref compNumber, g.CompName, g.LegalEntity, g.CompAlphaCode, g.CompAbrev, ref validationMsg))
                        {
                            response.StatusCode = HttpStatusCode.InternalServerError;
                            response.Errors = validationMsg;
                            return response;
                        }

                        //Insert the company record
                        LTS.Comp newComp = new LTS.Comp
                        {
                            CompNumber = compNumber,
                            CompActive = g.CompActive,
                            CompName = g.CompName,
                            CompLegalEntity = g.LegalEntity,
                            CompAlphaCode = g.CompAlphaCode,
                            CompAbrev = g.CompAbrev,
                            CompStreetAddress1 = g.CompAddress1,
                            CompStreetAddress2 = g.CompAddress2,
                            CompStreetAddress3 = g.CompAddress3,
                            CompStreetCity = g.CompCity,
                            CompStreetState = g.CompState,
                            CompStreetCountry = g.CompCountry,
                            CompStreetZip = g.CompZip,
                            CompEmail = g.CompEmail,
                            CompWeb = g.CompWebsite,
                            CompFinCurType = 1, //Hardcoded for now
                            CompModDate = DateTime.Now,
                            CompModUser = Parameters.UserName
                        };
                        var ltsRetComp = NGLCompData.InsertComp(newComp);
                        g.CompControl = ltsRetComp.CompControl;

                        //Insert the company contact record
                        try
                        {
                            LTS.CompCont newCompCont = new LTS.CompCont { CompContCompControl = g.CompControl, CompContName = g.CompContName, CompContTitle = g.CompContTitle, CompCont800 = g.CompCont800, CompContPhone = g.CompContPhone, CompContPhoneExt = g.CompContPhoneExt, CompContFax = g.CompContFax, CompContEmail = g.CompContEmail, CompContTender = g.CompContTender };
                            NGLCompContData.InsertOrUpdateCompCont365(newCompCont);
                        }
                        catch (Exception ex)
                        {
                            //log as system alert message but continue processing
                            SaveAppError("CreateNewLEComp Error: " + ex.ToString() + " (source: InsertOrUpdateLegalEntity)");
                        }
                    }
                    else
                    {
                        //Using an Existing Company -- must update that Company's CompLE field
                        NGLCompData.UpdateCompLegalEntity(g.CompControl, g.LegalEntity);
                    }

                    //Insert new tblLegalEntityAdmin record

                    //Modified by RHR for v-8.1.1.1 on 05/10/2018
                    LTS.tblLegalEntityAdmin LEA = new LTS.tblLegalEntityAdmin {
                        LEAdminLegalEntity = g.LegalEntity,
                        LEAdminCompControl = g.CompControl,
                        LEAdminCNSPrefix = g.LEAdminCNSPrefix,
                        LEAdminCNSNumberLow = g.LEAdminCNSNumberLow,
                        LEAdminCNSNumberHigh = g.LEAdminCNSNumberHigh,
                        LEAdminCNSNumber = g.LEAdminCNSNumber,
                        LEAdminPRONumber = g.LEAdminPRONumber,
                        LEAdminAllowCreateOrderSeq = g.LEAdminAllowCreateOrderSeq,
                        LEAdminAutoAssignOrderSeqSeed = g.LEAdminAutoAssignOrderSeqSeed,  
                        LEAdminBOLLegalText = g.LEAdminBOLLegalText,
                        LEAdminDispatchLegalText = g.LEAdminDispatchLegalText,
                        LEAdminCarApptAutomation = false, //This can only be set by a Super User so on insert it must be false
                        LEAdminApptModCutOffMinutes = g.LEAdminApptModCutOffMinutes,
                        LEAdminDefaultLastLoadTime = g.LEAdminDefaultLastLoadTime,
                        LEAdminApptNotSetAlertMinutes = g.LEAdminApptNotSetAlertMinutes,
                        LEAdminAllowApptEdit = false,
                        LEAdminAllowApptDelete = false,
                        LEAdminCarrierAcceptLoadMins = g.LEAdminCarrierAcceptLoadMins,
                        LEAdminExpiredLoadsTo = g.LEAdminExpiredLoadsTo,
                        LEAdminExpiredLoadsCc = g.LEAdminExpiredLoadsCc,
                        LEAdminSecurityLevel = g.LEAdminSecurityLevel //Modified by RHR for v-8.5.2.006 on 03/02/2023 added support for LEAdminSecurityLevel
                    };
                    var LEAControl = oLEA.InsertLEAdmin(LEA);

                    //When we add a LE make sure it creates the user groups automatically by default
                    if (LEAControl != 0)
                    {
                        BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                        bll.CreateUserGroupsForLegalEntityAsync(g.LegalEntity, g.CompControl);
                    }                   
                }
                else
                {
                    //UPDATE LEGAL ENTITY
                    //Modified by RHR for v-8.1.1.1 on 05/10/2018
                    LTS.tblLegalEntityAdmin LEA = new LTS.tblLegalEntityAdmin
                    {
                        LEAdminControl = g.LEAdminControl,
                        LEAdminLegalEntity = g.LegalEntity,
                        LEAdminCompControl = g.CompControl,
                        LEAdminCNSPrefix = g.LEAdminCNSPrefix,
                        LEAdminCNSNumberLow = g.LEAdminCNSNumberLow,
                        LEAdminCNSNumberHigh = g.LEAdminCNSNumberHigh,
                        LEAdminCNSNumber = g.LEAdminCNSNumber,
                        LEAdminPRONumber = g.LEAdminPRONumber,
                        LEAdminAllowCreateOrderSeq = g.LEAdminAllowCreateOrderSeq,
                        LEAdminAutoAssignOrderSeqSeed = g.LEAdminAutoAssignOrderSeqSeed,
                        LEAdminBOLLegalText = g.LEAdminBOLLegalText,
                        LEAdminDispatchLegalText = g.LEAdminDispatchLegalText,
                        LEAdminCarApptAutomation = g.LEAdminCarApptAutomation, //This can only be set by a Super User
                        LEAdminApptModCutOffMinutes = g.LEAdminApptModCutOffMinutes,
                        LEAdminDefaultLastLoadTime = g.LEAdminDefaultLastLoadTime,
                        LEAdminApptNotSetAlertMinutes = g.LEAdminApptNotSetAlertMinutes,
                        LEAdminAllowApptEdit = g.LEAdminAllowApptEdit,
                        LEAdminAllowApptDelete = g.LEAdminAllowApptDelete,
                        LEAdminCarrierAcceptLoadMins = g.LEAdminCarrierAcceptLoadMins,
                        LEAdminExpiredLoadsTo = g.LEAdminExpiredLoadsTo,
                        LEAdminExpiredLoadsCc = g.LEAdminExpiredLoadsCc,
                        LEAdminSecurityLevel = g.LEAdminSecurityLevel //Modified by RHR for v-8.5.2.006 on 03/02/2023 added support for LEAdminSecurityLevel
                    };
                    var spRes = oLEA.UpdateLEAdminLegalEntity(LEA);
                    if(spRes.ErrNumber != 0)
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = spRes.RetMsg;
                        return response;
                    }

                    //Must update that Company's CompLE field
                    //NGLCompData.UpdateCompLegalEntity(g.CompControl, g.LegalEntity);
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



        [HttpPost, ActionName("ChangeOrdersLanesComp")]
        public Models.Response ChangeOrdersLanesComp([System.Web.Http.FromBody]Models.GenericResult g)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.NGLLegalEntityAdminData oLEA = new DAL.NGLLegalEntityAdminData(Parameters);

                int iOldCompControl = g.intField1;
                int iNewCompControl = g.intField2;
                string sRetMsg = "Success!";
                bool bRes = oLEA.UpdateLEChangeOrderLaneComp(iOldCompControl, iNewCompControl, ref sRetMsg);
                g.RetMsg = sRetMsg;
                g.blnField = bRes;
                            

                //return generic results caller call back will handle message
                Array d = new Models.GenericResult[1] { g };
                response = new Models.Response(d, 1);
                if (!bRes) { response.Errors = sRetMsg; }
               
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



        [HttpPost, ActionName("ResetSecuritySettings")]
        public Models.Response ResetSecuritySettings([System.Web.Http.FromBody]Models.GenericResult g)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int iLEControl = g.Control;
                string sRetMsg = "";
                bool bRes = NGLLegalEntityAdminData.ResetSecuritySettings(iLEControl,ref sRetMsg);
                //return generic results caller call back will handle message
                Array d = new Models.GenericResult[1] { g };
                response = new Models.Response(d, 1);
                if (!bRes) { response.Errors = sRetMsg; }

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
        /// Updates the [UserMustChangePassword] flag for all users 
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.2.006 on 03/02/2023 
        /// </remarks>
        [HttpPost, ActionName("AllUsersMustChangePassword")]
        public Models.Response AllUsersMustChangePassword([System.Web.Http.FromBody] Models.GenericResult g)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                bool blnActive = g.blnField1;
                string sRetMsg = " Unable to update all user settings";
               // DAL.NGLSecurityDataProvider oDal = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
                DAL.NGLSecurityDataProvider oDal = new DAL.NGLSecurityDataProvider(Parameters);                
                Array d = new Models.GenericResult[1] { g };
                if (g.Control != Parameters.UserLEControl)
                {
                                       
                    response = new Models.Response(d, 1);
                    response.Errors = "You are not authorized to update user settings. You must be logged in as an administrator for the selected Legal Entity.  Do not run this if you are a Super User.";
                    return response;
                }
                bool bRes = oDal.AllUsersMustChangePassword(blnActive);
                //return generic results caller call back will handle message
                d = new Models.GenericResult[1] { g };
                response = new Models.Response(d, 1);
                if (!bRes) { response.Errors = sRetMsg; }

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


        [HttpPost, ActionName("ChangeAMSCarrierAuto")]
        public Models.Response ChangeAMSCarrierAuto([System.Web.Http.FromBody]Models.InsertOrUpdateLE g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLegalEntityAdminData oLEA = new DAL.NGLLegalEntityAdminData(Parameters);
                var res = oLEA.ChangeAMSCarrierAuto(g.LEAdminControl, g.LEAdminCarApptAutomation);
                if (res.Success)
                {
                    //No errors so return success to caller
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = res.ErrMsg;                  
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



        //******************LECAM (LE Carrier Accesorial Maint)***************

        [HttpPost, ActionName("CopyLECAMConfig")]
        public Models.Response CopyLECAMConfig([System.Web.Http.FromBody]Models.GenericResult g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLookupDataProvider oLook = new DAL.NGLLookupDataProvider(Parameters);

                int CopyToLEControl = g.intField1;
                int CopyFromLEControl = g.intField2;

                LTS.spCopyLECAMConfigResult spRet = oLook.CopyLECAMConfig(CopyToLEControl, CopyFromLEControl);

                if (spRet.ErrNumber == 0)
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    //** TODO LVV ** Localize these messages.
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = spRet.RetMsg;
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

        [HttpGet, ActionName("GetLEWithCAMConfigsToCopy")]
        public Models.Response GetLEWithCAMConfigsToCopy()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //LTS.vGetLEWithCAMConfigsToCopy[] LEs = new LTS.vGetLEWithCAMConfigsToCopy[] { };
                List<DTO.vLookupList> LEs = new List<DTO.vLookupList>();
                DAL.NGLLookupDataProvider oLook = new DAL.NGLLookupDataProvider(Parameters);
                int count = 0;

                var spRet = oLook.GetLEWithCAMConfigsToCopy();

                if (spRet?.Count() > 0)
                {
                    foreach (LTS.vGetLEWithCAMConfigsToCopy r in spRet)
                    {
                        DTO.vLookupList v = new DTO.vLookupList();
                        v.Control = r.LEAdminControl;
                        v.Name = r.LegalEntity;
                        v.Description = r.CompName;
                        LEs.Add(v);                      
                    }
                    count = LEs.Count;
                }
                response = new Models.Response(LEs.ToArray(), count);
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