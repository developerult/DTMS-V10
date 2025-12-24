using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using BLL = NGL.FM.BLL;
using System;
using AuthenticaionService;
using Microsoft.SqlServer.Server;

namespace DynamicsTMS365.Controllers
{
    public class ReceiverController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ReceiverController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        #endregion

        #region " REST Services" 

        [HttpGet, ActionName("GetToken")]
        public string GetToken()
        {
             
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!AuthenticateClientCredentials(ref response)) { 
                return "Unauthorized"; }  else
            {
                return response.TokenAuthCode;
            }
            
        }

        [HttpPost, ActionName("GetAToken")]
        public string GetAToken()
        {

            var response = new Models.Response(); //new HttpResponseMessage();
            if (!AuthenticateClientCredentials(ref response))
            {
                return "Unauthorized";
            }
            else
            {
                string sAccountNumber = this.GetSSOAAccountNumber(response.TokenAuthCode, response.TokenSecrectCode);
                AuthToken oAuthData = new AuthToken();
                string sAuth = response.TokenAuthCode; // "NGLWSPROD2023"; // oAuthData.ComputeContentHash("NGLWSPROD2023");
                                                       //string sKey = System.Text.Encoding.UTF8.GetBytes("NGLEngageLane").ToString();
                                                       //Console.WriteLine("Key: " + sKey);
                                                       //Console.WriteLine("Encolded Key: " + Encoding.UTF8.GetBytes("NGLEngageLane"));
                                                       //Console.WriteLine("new key:  " + oAuthData.ComputeContentHash("NGLEngageLane"));
                                                       //string sKey = "5e48871a432c4cac84283a2515a54c67";
                                                       //string sSecret = "BC0AA12E-EDBC-4C63-ABD5-F8337A398389".Replace("-", "");
                string sSecret = response.TokenSecrectCode; //Guid.NewGuid().ToString().Replace("-", "");

                //Console.WriteLine("Secrect: " + sSecret);
                string sKey = oAuthData.ComputeContentHash(sSecret);
                //Console.WriteLine("Key: " + sKey);

                string sName = "NGL Ops";
                string sEmail = "rramsey@nextgeneration.com";
                string sAccount = sAccountNumber;
                string sUserToken = oAuthData.getToken(sAuth, sKey, sAccount, sName, sEmail);

                return sUserToken;
            }

        }

        [HttpPost, ActionName("Post990")]
        public Models.Response Post900([FromBody] Models.API990 data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            string sAccountNumber = "";
            string sToken = AuthenticateReadAccountFromToken(ref sAccountNumber);
            LTS.tblSSOALEConfig oConfig = GettblSSOALEConfigFromToken(sAccountNumber, 1);

            //oConfig.SSOALEClientSecret

            //AuthenticationService.Managers.IAuthService authService = new AuthenticationService.Managers.JWTService(sTempKey);
            //List<Claim> claims = authService.ReadTokenClaims(sUserToken).ToList();
            //Console.WriteLine("Auth: " + claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Authentication)).Value);

            //authService.SecretKey = sKey;
            //if (!authService.IsTokenValid(sUserToken)) { throw new UnauthorizedAccessException(); }
            //else
            //{
            //    List<Claim> lClaims = authService.GetTokenClaims(sUserToken).ToList();
            //    string sAuthenticationCode = lClaims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Authentication)).Value;
            //    Console.WriteLine("Account: " + lClaims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Authentication)).Value);
            //    Console.WriteLine("Name: " + lClaims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
            //    Console.WriteLine("Email: " + lClaims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Email)).Value);
            //}

            //if (!authenticateController(ref response)) { return response; }
            //try
            //{
            //    bool blnRet = true;
            //    LTS.tblSSOAConfig tblSSOAConfig = Models.tblSSOAConfig.selectLTSData(data);
            //    LTS.tblSSOAConfig oResults = NGLtblSingleSignOnAccountData.InsertOrUpdatetblSSOAConfig(tblSSOAConfig);
            //    if (oResults == null || oResults.SSOACControl == 0) { blnRet = false; }
            //    bool[] oRecords = new bool[1];

            //    oRecords[0] = blnRet;
            //    response = new Models.Response(oRecords, 1);
            //}
            //catch (Exception ex)
            //{
            //    FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
            //    response.StatusCode = fault.StatusCode;
            //    response.Errors = fault.formatMessage();
            //    return response;
            //}
            // = "Success";
            return response;
        }

        #endregion

        #region " Private Methods" 


        private String GetSSOAAccountNumber(string sSSOALEAuthCode , string sSSOALEClientSecret )
        {
            string sRet = null;
            try
            {

                sRet = NGLtblSingleSignOnAccountData.GetFirsttblSSOALEAccountNumber(sSSOALEAuthCode, sSSOALEClientSecret);
               
            }
            catch (Exception ex)
            {
                return null;
            }
            return sRet;
        }

        private LTS.tblSSOALEConfig GettblSSOALEConfigFromToken(string sSSOALEClientID, int iSSOALESSOATypeControl)
        {
            LTS.tblSSOALEConfig oRet = new LTS.tblSSOALEConfig();
            try
            {
                oRet = NGLtblSingleSignOnAccountData.GettblSSOALEConfigFromToken(sSSOALEClientID, iSSOALESSOATypeControl);

            }
            catch (Exception ex)
            {
                return null;
            }
            return oRet;
        }

        #endregion

    }
}