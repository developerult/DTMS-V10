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
    public class SubscriptionRequestController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.SubscriptionRequestController";
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
            return GetPendingSubscriptionRequests(filter);
        }

        [HttpGet, ActionName("GetPendingSubscriptionRequests")]
        public Models.Response GetPendingSubscriptionRequests(string filter)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                DAL.NGLSubscriptionRequestPendingData oSUBPD = new DAL.NGLSubscriptionRequestPendingData(Parameters);
                LTS.vSubscriptionRequest[] subs = new LTS.vSubscriptionRequest[] { };
                int RecordCount = 0;

                subs = oSUBPD.GetSubscriptionRequestsFiltered(ref RecordCount, f);

                response = new Models.Response(subs, RecordCount);
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


        [HttpPost, ActionName("CreateSubscriptionRequest")]
        public Models.Response CreateSubscriptionRequest([System.Web.Http.FromBody]DAL.Models.FreeTrialComp comp)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                DAL.Models.FreeTrialComp rc = new DAL.Models.FreeTrialComp();

                rc = bll.CreateNewSignUpComp(comp);

                if (!string.IsNullOrWhiteSpace(rc.ValidationMsg))
                {
                    response.StatusCode = HttpStatusCode.NotAcceptable;
                    response.Errors = rc.ValidationMsg;
                    return response;
                }
                else
                {
                    System.Text.StringBuilder sbEmailNGL = new System.Text.StringBuilder();
                    System.Text.StringBuilder sbEmailUser = new System.Text.StringBuilder();
                    
                    string nglSignUpEmailTo = System.Configuration.ConfigurationManager.AppSettings["NGLSignUpEmailTo"];
                    if (string.IsNullOrWhiteSpace(nglSignUpEmailTo)) { nglSignUpEmailTo = "support@nextgeneration.com"; }
                    string sFrom = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
                    string newLine = System.Environment.NewLine + "<br />";

                    //Send an email to the NGL Admin person who handles the sign ups
                    sbEmailNGL.Append("New User Sign Up Request");
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append("A user has requested to Sign Up for a full account with the following information: ");
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append("USER");
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append(string.Format("UserControl: {0}, UserName: {1}, UserEmail: {2}", Parameters.UserControl, Parameters.UserName, Parameters.UserEmail));
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append("COMPANY");
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append(string.Format("CompControl: {0}, CompName: {1}, CompNumber {2}, CompLegalEntity: {3}, CompAbrev: {4}, CompAlphaCode: {5}", rc.CompControl, rc.CompName, rc.CompNumber, rc.CompLegalEntity, rc.CompAbrev, rc.CompAlphaCode));
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append(string.Format("Address1: {0}, Address2: {1}, Address3: {2}, City {3}, State: {4}, Zip: {5}, Country: {6}", rc.ShipFromAddress1, rc.ShipFromAddress2, rc.ShipFromAddress3, rc.ShipFromCity, rc.ShipFromState, rc.ShipFromZip, rc.ShipFromCountry));
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append("CONTACT");
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append(string.Format("Name: {0}, Phone: {1}, Email: {2}", rc.CompContName, rc.CompContPhone, rc.CompContEmail));
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append(string.Format("Title: {0}, 800: {1}, Phone Ext: {2}, Fax: {3}", rc.CompContTitle, rc.CompCont800, rc.CompContPhoneExt, rc.CompContFax));
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append("User Groups for this Legal Entity have also been created for Administrators, Standard, and Carrier.");
                    sbEmailNGL.Append(newLine);
                    if (!string.IsNullOrWhiteSpace(rc.WarningMsg))
                    {
                        sbEmailNGL.Append("The following problems occured while creating records using the information above: ");
                        sbEmailNGL.Append(newLine);
                        sbEmailNGL.Append(rc.WarningMsg);
                        sbEmailNGL.Append(newLine);
                    }
                    try
                    {
                        NGLEmailData.GenerateEmail(sFrom, nglSignUpEmailTo, "", "New User Sign Up Request", sbEmailNGL.ToString(), "", "", "", "");
                    }
                    catch (Exception ex)
                    {
                        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                        string msg = "Error generating New Sign Up email to NGL for User: " + Parameters.UserName + " and CompControl: " + rc.CompControl + ". Err Msg: " + fault.formatMessage();
                        SaveAppError(msg);
                    }

                    //Send an email to the user who requested the sign up
                    sbEmailUser.Append("Thank you for your request to Get More Features!");
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(string.Format("The following information has been sent to Next Generation Logistics, Inc. with the request for access to the full version of the software. If you would like to change any of the information please send an email to {0}", nglSignUpEmailTo));
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append("COMPANY INFO");
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(string.Format("Company Name: {0}, 3 Letter Prefix: {1}, Warehouse Name: {2}, Warehouse ID: {3}", rc.CompLegalEntity, rc.CompAbrev, rc.CompName, rc.CompAlphaCode));
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append("SHIP FROM ADDRESS");
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(string.Format("Address1: {0}, Address2: {1}, Address3: {2}, City {3}, State: {4}, Postal Code: {5}, Country: {6}", rc.ShipFromAddress1, rc.ShipFromAddress2, rc.ShipFromAddress3, rc.ShipFromCity, rc.ShipFromState, rc.ShipFromZip, rc.ShipFromCountry));
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append("COMPANY CONTACT");
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(string.Format("Contact Name: {0}, Title: {1}, Email: {2}", rc.CompContName, rc.CompContTitle, rc.CompContEmail));
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(string.Format("Phone: {0}, Phone Ext: {1}, 800: {2}, Fax: {3}", rc.CompContPhone, rc.CompContPhoneExt, rc.CompCont800, rc.CompContFax));
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append("A Next Generation Logistics Team Member will be in contact soon to complete the Sign Up process.");
                    sbEmailUser.Append(newLine);
                    try
                    {
                        NGLEmailData.GenerateEmail(sFrom, Parameters.UserEmail, "", "TMS 365 Sign Up Request Pending", sbEmailUser.ToString(), "", "", "", "");
                    }
                    catch (Exception ex)
                    {
                        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                        string msg = "Error generating New Sign Up email to User: " + Parameters.UserName + " with CompControl: " + rc.CompControl + ". Err Msg: " + fault.formatMessage();
                        SaveAppError(msg);
                    }

                    //populate reponse data
                    int count = 1;
                    Array d = new DAL.Models.FreeTrialComp[1] { rc };
                    response = new Models.Response(d, count);
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


        [HttpGet, ActionName("GetSubscriptionRequestByUser")]
        public Models.Response GetSubscriptionRequestByUser(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);

                DAL.Models.FreeTrialComp comp = bll.GetPendingSignUpInfo(id);

                if (comp == null || comp.CompControl == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Errors = "No comp found";
                }
                else
                {
                    Array d = new DAL.Models.FreeTrialComp[1] { comp };
                    response = new Models.Response(d, 1);
                } 
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


        /// <summary>
        /// GenericResult.Control = LEAdminControl
        /// GenericResult.intField1 = LEAdminCompControl
        /// GenericResult.intField2 = USC
        /// GenericResult.intField3 = SUBPDControl
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ApproveSubscriptionRequest")]
        public Models.Response ApproveSubscriptionRequest([System.Web.Http.FromBody]Models.GenericResult g)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.NGLSubscriptionRequestPendingData oSUBPD = new DAL.NGLSubscriptionRequestPendingData(Parameters);
                DAL.NGLUserSecurityLegalEntityData oUSLE = new DAL.NGLUserSecurityLegalEntityData(Parameters);
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);

                // ** TODO ** PUT THIS ALL IN A TRANSACTIONAL SP

                //Change the users UserSecurityLegalEntity record from Free Trial to the New LE
                var spRes = oUSLE.InsertOrUpdateUserSecurityLE(g.intField2, g.Control);
                if (spRes != null && spRes.ErrNumber != 0)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = spRes.RetMsg;
                    return response;
                }

                //Move the user to the LE Admin Group for the new LE
                var dtoGroup = oSec.GetUserGroupFiltered(g.intField1, 3);
                if (dtoGroup != null && dtoGroup.UserGroupsControl != 0)
                {
                    oSec.ReplaceUserSecurityWithGroup(dtoGroup.UserGroupsControl, g.intField2);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = "Could not find the Administrators group for this Legal Entity. The user could not be moved from the Free Trial User Group and it will have to be done manually in the Role Center.";
                    return response;
                }

                //Remove the record from tblSubscriptionRequestPending
                bool blnSuccess = oSUBPD.DeleteSubscriptionRequest(g.intField3);

                Array d = new bool[1] { true };
                response = new Models.Response(d, 1);
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

    }
}