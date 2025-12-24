using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class ClaimsController : NGLControllerBase
    {
        #region " Constructors "

        public ClaimsController()
            : base(Utilities.PageEnum.Claims)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ClaimsController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion
        #region " Data Translation"

        private Models.Claim selectModelData(LTS.vLEClaims365 d)
        {
            Models.Claim modelRecord = new Models.Claim();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ClaimUpdated" };
                string sMsg = "";
                modelRecord = (Models.Claim)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.ClaimUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.vLEClaims365 selectLTSData(Models.Claim d)
        {
            LTS.vLEClaims365 ltsRecord = new LTS.vLEClaims365();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ClaimUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vLEClaims365)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.ClaimUpdated = bupdated == null ? new byte[0] : bupdated;

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
            //Note: The id must always match a CarrTarEquipControl associated with the select tariff using CarrTarEquipCarrTarControl
            //the system looks up the last saved tariff pk for this user and return the first Service record found
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new service record.

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                if (id == 0) { id = this.readPagePrimaryKey(); }
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "ClaimControl";
                    f.filterValue = id.ToString();
                }

                DAL.NGLClaimData oDAL = new DAL.NGLClaimData(Parameters);
                Models.Claim[] records = new Models.Claim[] { };
                LTS.vLEClaims365[] oData = new LTS.vLEClaims365[] { };
                oData = oDAL.GetClaimsFiltered(f, ref RecordCount);
                //if (oData != null && oData.Count() > 0)
                //{
                count = oData.Count();
                records = (from e in oData select selectModelData(e)).ToArray();
                if (RecordCount > count) { count = RecordCount; }
                //  }
                response = new Models.Response(records, 1);
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


        [HttpGet, ActionName("GetClaimSummary")]
        public Models.Response GetClaimSummary(int id)
        {

            return this.Get(id);

        }


        [HttpGet, ActionName("GetNewClaim")]
        public Models.Response GetNewClaim(int id)
        {
            //id must be a valid bookcontrol

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 1;
                if (id == 0)
                {
                    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                }
                DAL.NGLClaimData oDAL = new DAL.NGLClaimData(Parameters);
                Models.Claim[] records = new Models.Claim[] { };
                LTS.vLEClaims365 oClaim = new LTS.vLEClaims365();
                LTS.vLEClaims365[] oData = new LTS.vLEClaims365[] { };
                oClaim = oDAL.CreateClaim(id);
                if (oClaim != null && oClaim.ClaimControl != 0)
                {
                    Models.PageSetting psData = new Models.PageSetting();
                    psData.name = "pk";
                    psData.value = oClaim.ClaimControl.ToString();
                    this.PostPageSetting(psData, Utilities.PageEnum.Claims);
                    oData = new LTS.vLEClaims365[] { oClaim };

                }

                count = oData.Count();
                records = (from e in oData select selectModelData(e)).ToArray();
                if (RecordCount > count) { count = RecordCount; }

                response = new Models.Response(records, 1);
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
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                DAL.NGLClaimData oDAL = new DAL.NGLClaimData(Parameters);
                Models.Claim[] records = new Models.Claim[] { };
                LTS.vLEClaims365[] oData = new LTS.vLEClaims365[] { };
                oData = oDAL.GetClaimsFiltered(f, ref RecordCount);
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


        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody] Models.Claim data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (data.ClaimCarrierControl == 0) { data.ClaimCarrierControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LegalEntityCarrierMaint); }
                DAL.NGLClaimData oDAL = new DAL.NGLClaimData(Parameters);

                DAL.NGLLegalEntityCarrierData aNGLLegalEntityCarrierData = new DAL.NGLLegalEntityCarrierData(Parameters);
                int leCarCarrierControl = aNGLLegalEntityCarrierData.GetCarrierControlForLECarControl((int)data.ClaimCarrierControl);

                data.ClaimCarrierControl = leCarCarrierControl;

                LTS.vLEClaims365 oChanges = selectLTSData(data);

                //Grab user control id for comp id - Starts
                //int user = UserControl > 0 ? UserControl :
                //          (HttpContext.Current.Session["user"] as int?) ?? 0;

                //if (user > 0)
                //{
                //    DAL.NGLUserGroupData aNGLUserGroupData = new DAL.NGLUserGroupData(Parameters);
                //    var userSecurityEntities = aNGLUserGroupData.GetUserSecurityLegalEntity(user);

                //    if (userSecurityEntities?.Any() == true)
                //    {
                //        DAL.NGLCompData aNGLCompData = new DAL.NGLCompData(Parameters);
                //        Take the first comp id
                //        var firstComp = userSecurityEntities.First();
                //        oChanges.ClaimCustCompControl = aNGLCompData.GetComp(firstComp.USLECompControl).CompControl;
                //    }
                //}
                //Grab user control id for comp id - Ends

                //Previous code starts - 26/11/2025
                //Grab user control id for comp id -Starts
                int user = 0;
                if (UserControl > 0)
                {
                    user = UserControl;
                }
                else if (HttpContext.Current.Session["user"] != null)
                {
                    user = (int)HttpContext.Current.Session["user"];
                }
                else
                {
                    //Set a default value or handle the error
                    user = 0;
                }

                DAL.NGLUserGroupData aNGLUserGroupData = new DAL.NGLUserGroupData(Parameters);
                aNGLUserGroupData.GetUserSecurityLegalEntity(user);

                int compId = 0;
                DAL.NGLCompData aNGLCompData = new DAL.NGLCompData(Parameters);

                foreach (var comp in aNGLUserGroupData.GetUserSecurityLegalEntity(user))
                {
                    compId = aNGLCompData.GetComp(comp.USLECompControl).CompControl;
                }
                //Grab user control id for comp id -Ends

                oChanges.ClaimCustCompControl = compId;
                //Previous code ends - 26/11/2025

                bool blnRet = oDAL.SaveClaim(oChanges);
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


        // Commented by Ayman on 10-12-2025
        //[HttpPost, ActionName("Post")]
        //public Models.Response Post([System.Web.Http.FromBody]Models.Claim data)
        //{
        //    //create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        if (data.ClaimCarrierControl == 0) { data.ClaimCarrierControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LegalEntityCarrierMaint); }
        //        DAL.NGLClaimData oDAL = new DAL.NGLClaimData(Parameters);

        //        DAL.NGLLegalEntityCarrierData aNGLLegalEntityCarrierData = new DAL.NGLLegalEntityCarrierData(Parameters);
        //        int leCarCarrierControl = aNGLLegalEntityCarrierData.GetCarrierControlForLECarControl((int)data.ClaimCarrierControl);

        //        data.ClaimCarrierControl = leCarCarrierControl;

        //        LTS.vLEClaims365 oChanges = selectLTSData(data);

        //        // Commented by Amit change on 2025-12-01 quick fix for comp control id
        //        ////Grab user control id for comp id - Starts
        //        //int user = UserControl > 0 ? UserControl :
        //        //          (HttpContext.Current.Session["user"] as int?) ?? 0;


        //        //DAL.NGLClaimData oDAL = new DAL.NGLClaimData(Parameters);
        //        //LTS.vLEClaims365 oChanges = selectLTSData(data);

        //        //// Add by Amit on 2025-12-01 quick fix for comp control id
        //        //if (user > 0)
        //        //{
        //        //    DAL.NGLUserGroupData aNGLUserGroupData = new DAL.NGLUserGroupData(Parameters);
        //        //    var userSecurityEntities = aNGLUserGroupData.GetUserSecurityLegalEntity(user);

        //        //    if (userSecurityEntities?.Any() == true)
        //        //    {
        //        //        DAL.NGLCompData aNGLCompData = new DAL.NGLCompData(Parameters);
        //        //        //Take the first comp id
        //        //        var firstComp = userSecurityEntities.First();
        //        //        oChanges.ClaimCustCompControl = aNGLCompData.GetComp(firstComp.USLECompControl).CompControl;
        //        //    }
        //        //}


        //        //Previous code starts - 26/11/2025
        //        //Grab user control id for comp id -Starts
        //        int user = 0;
        //        if (UserControl > 0)
        //        {
        //            user = UserControl;
        //        }
        //        else if (HttpContext.Current.Session["user"] != null)
        //        {
        //            user = (int)HttpContext.Current.Session["user"];
        //        }
        //        else
        //        {
        //            //Set a default value or handle the error
        //            user = 0;
        //        }

        //        DAL.NGLUserGroupData aNGLUserGroupData = new DAL.NGLUserGroupData(Parameters);
        //        aNGLUserGroupData.GetUserSecurityLegalEntity(user);

        //        int compId = 0;
        //        DAL.NGLCompData aNGLCompData = new DAL.NGLCompData(Parameters);

        //        foreach (var comp in aNGLUserGroupData.GetUserSecurityLegalEntity(user))
        //        {
        //            compId = aNGLCompData.GetComp(comp.USLECompControl).CompControl;
        //        }
        //        //Grab user control id for comp id -Ends

        //        oChanges.ClaimCustCompControl = compId;
        //        //Previous code ends - 26/11/2025

        //        bool blnRet = oDAL.SaveClaim(oChanges);
        //        bool[] oRecords = new bool[1];
        //        oRecords[0] = blnRet;
        //        response = new Models.Response(oRecords, 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error handler
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }

        //    return response;
        //}


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLClaimData oDAL = new DAL.NGLClaimData(Parameters);
                bool blnRet = oDAL.DeleteClaim(id);

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