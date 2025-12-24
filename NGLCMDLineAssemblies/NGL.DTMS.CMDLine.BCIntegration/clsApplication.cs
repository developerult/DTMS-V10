using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
//using NGLNAV = NGL.FM.CMDLine.NAVIntegration;
using Comp = NGL.DTMS.CMDLine.BCIntegration.Company;
using Carr = NGL.DTMS.CMDLine.BCIntegration.Carrier;
using Lane = NGL.DTMS.CMDLine.BCIntegration.Lane;
using Haz = NGL.DTMS.CMDLine.BCIntegration.Haz;
using Book = NGL.DTMS.CMDLine.BCIntegration.Book;
using Pallet = NGL.DTMS.CMDLine.BCIntegration.Plt;
using Payable = NGL.DTMS.CMDLine.BCIntegration.Pay;
using Pick = NGL.DTMS.CMDLine.BCIntegration.Pick;
using AP = NGL.DTMS.CMDLine.BCIntegration.AP;
using Ngl.FreightMaster.Core;
using System.Net.Sockets;


namespace NGL.DTMS.CMDLine.BCIntegration
{
    public class clsApplication : Ngl.FreightMaster.Core.NGLCommandLineBaseClass
    {
        #region "Properties"

        // default values are for NGL Prod BC only
        public oAuth2Settings Settings { get; set; }
        public DateTime TokenExpires { get; set; }
        public string BearerToken {  get; set; }
        #endregion

        #region "Public Methods"

        public Comp.Envelope getCompanies(oAuth2Settings sSettings, bool readAllHistory = false, bool markAsRead = true)
        {
            Comp.Envelope oRet = null;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);
            }

            Task<Comp.Envelope> Comptask = Task.Run<Comp.Envelope>(async () => await Comp.clsCompanies.Read(this.BearerToken, sSettings, sSettings.Legal, readAllHistory, markAsRead));
            oRet = Comptask.Result;            
            return oRet;
        }

        public Carr.Envelope getCarriers(oAuth2Settings sSettings, bool readAllHistory = false, bool markAsRead = true)
        {
            Carr.Envelope oRet = null;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }

            Task<Carr.Envelope> Carrtask = Task.Run<Carr.Envelope>(async () => await Carr.clsCarriers.Read(this.BearerToken, sSettings, sSettings.Legal, readAllHistory, markAsRead));
            oRet = Carrtask.Result;
            return oRet;
        }

        public Lane.Envelope getLanes(oAuth2Settings sSettings, bool readAllHistory = false, bool markAsRead = true)
        {
            Lane.Envelope oRet = null;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }

            Task<Lane.Envelope> Lanetask = Task.Run<Lane.Envelope>(async () => await Lane.clsLanes.Read(this.BearerToken, sSettings, sSettings.Legal, readAllHistory, markAsRead));
            oRet = Lanetask.Result;
            return oRet;
        }

        public Haz.Envelope getHazmats(oAuth2Settings sSettings)
        {
            Haz.Envelope oRet = null;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }

            Task<Haz.Envelope> Haztask = Task.Run<Haz.Envelope>(async () => await Haz.clsHazmats.Read(this.BearerToken, sSettings, sSettings.Legal));
            oRet = Haztask.Result;
            return oRet;
        }

        public Book.Envelope getBooks(oAuth2Settings sSettings, bool readAllHistory = false, bool markAsRead = true)
        {
            Book.Envelope oRet = null;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }

            Task<Book.Envelope> Booktask = Task.Run<Book.Envelope>(async () => await Book.clsBookings.Read(this.BearerToken, sSettings, sSettings.Legal, readAllHistory, markAsRead));
            oRet = Booktask.Result;
            return oRet;
        }

        public Pallet.Envelope getPalletTypes(oAuth2Settings sSettings)
        {
            Pallet.Envelope oRet = null;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }

            Task<Pallet.Envelope> Pallettask = Task.Run<Pallet.Envelope>(async () => await Pallet.clsPalletTypes.Read(this.BearerToken, sSettings, sSettings.Legal));
            oRet = Pallettask.Result;
            return oRet;
        }

        public Payable.Envelope getPayables(oAuth2Settings sSettings, bool readAllHistory = false, bool markAsRead = true)
        {
            Payable.Envelope oRet = null;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }

            Task<Payable.Envelope> Payabletask = Task.Run<Payable.Envelope>(async () => await Payable.clsPayables.Read(this.BearerToken, sSettings, sSettings.Legal, readAllHistory, markAsRead));
            oRet = Payabletask.Result;
            return oRet;
        }

        public bool sendPicks(oAuth2Settings sSettings, Pick.SendPicks oSendPicks,ref string sMsg)
        {
            bool blnRet = false;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }
            string sPLBody = Pick.clsPickLists.GetSOAPBody(oSendPicks);
            Task<Pick.Envelope> Picktask = Task.Run<Pick.Envelope>(async () => await Pick.clsPickLists.Send(this.BearerToken, sSettings, sSettings.Legal, sPLBody));
            Pick.Envelope oPickListResult = Picktask.Result;
            //Pick.clsPickLists.Save(oPicks);
            if (oPickListResult != null && oPickListResult.Body != null)
            {
                if (oPickListResult.Body.Fault != null && oPickListResult.Body.Fault.detail != null)
                {
                    sMsg = "Update Picklist Failed Message: " + oPickListResult.Body.Fault.detail.@string;
                    blnRet = false;
                }
                else
                {

                    blnRet = true;
                    sMsg = "Update Picklist data Complete";
                }
            }
            else
            {
                sMsg = "Update Picklist data failed, data not available";
            }

            return blnRet;
        }


        public bool sendPicksNoPost(oAuth2Settings sSettings, Pick.SendPicks oSendPicks, ref string sMsg)
        {
            bool blnRet = false;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings));
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }
            string sPLBody = Pick.clsPickLists.GetSOAPBody(oSendPicks);
            Task<Pick.Envelope> Picktask = Task.Run<Pick.Envelope>(async () => await Pick.clsPickLists.SendNoPost(this.BearerToken, sSettings, sSettings.Legal, sPLBody));
            Pick.Envelope oPickListResult = Picktask.Result;
            //Pick.clsPickLists.Save(oPicks);
            if (oPickListResult != null && oPickListResult.Body != null)
            {
                if (oPickListResult.Body.Fault != null && oPickListResult.Body.Fault.detail != null)
                {
                    sMsg = "Update Picklist Failed Message: " + oPickListResult.Body.Fault.detail.@string;
                    blnRet = false;
                }
                else
                {

                    blnRet = true;
                    sMsg = "Update Picklist data Complete";
                }
            }
            else
            {
                sMsg = "Update Picklist data failed, data not available";
            }

            return blnRet;
        }

        public bool sendAPs(oAuth2Settings sSettings, AP.SendAP oSendAP, ref string sMsg)
        {
            bool blnRet = false;
            if (DateTime.Now > TokenExpires)
            {
                Task<string> task = Task.Run<string>(async () => await getBearerAccessToken(sSettings)); 
                this.BearerToken = task.Result;
                this.TokenExpires = DateTime.Now.AddMinutes(30);

            }
            string sAPBody = AP.clsAPExports.GetSOAPBody(oSendAP);
            Task<AP.Envelope> APtask = Task.Run<AP.Envelope>(async () => await AP.clsAPExports.Read(this.BearerToken, sSettings, sSettings.Legal, sAPBody));
            AP.Envelope oAPListResult = APtask.Result;
            //AP.clsAPLists.Save(oAPs);
            if (oAPListResult != null && oAPListResult.Body != null)
            {
                if (oAPListResult.Body.Fault != null && oAPListResult.Body.Fault.detail != null)
                {
                    sMsg = "Update AP Data Failed Message: " + oAPListResult.Body.Fault.detail.ToString();
                    blnRet = false;
                }
                else
                {

                    blnRet = true;
                    sMsg = "Update AP Data Complete";
                }
            }
            else
            {
                sMsg = "Update AP Data failed, data not available";
            }

            return blnRet;
        }



        public async Task<string> getBearerAccessToken(oAuth2Settings sSettings )
        {
            string sRet = "";
            try
            {
                string sClientId = sSettings.ClientId;
                string sSecret = sSettings.Secret;
                

                HttpClient httpClient = new HttpClient();
                var content = new StringContent("grant_type=client_credentials&scope="
                    + sSettings.ScopeUrl + "&client_id="
                + HttpUtility.UrlEncode(sClientId) + "&client_secret=" + HttpUtility.UrlEncode(sSecret));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await httpClient.PostAsync(sSettings.AuthUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());
                    sRet =  result["access_token"].ToString();
                    TokenExpires = DateTime.Now.AddMinutes(30);
                    BearerToken = sRet;
                }
                else
                {
                   throw new System.ApplicationException("Error getting access token: " + response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                //Todo: add better exception handler
                throw;
            }
            return sRet;
        }

        #endregion

    }

    public class oAuth2Settings
    {
        public oAuth2Settings() { } 
        public oAuth2Settings(string sClientId, 
            string sSecret, 
            string sLegal, 
            string sScopeUrl = "https://api.businesscentral.dynamics.com/.default",
            string sDataUrl = "https://api.businesscentral.dynamics.com/v2.0/2518be7e-c933-4905-af64-24ad0157202f/Production/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices",
            string sActionUrl = "microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices",
            string sAuthUrl = "https://login.microsoftonline.com/2518be7e-c933-4905-af64-24ad0157202f/oauth2/v2.0/token"
            )
        {
            this.ClientId = sClientId;
            this.Secret = sSecret;
            this.DataUrl = sDataUrl;
            this.ScopeUrl = sScopeUrl;
            this.ActionUrl = sActionUrl;
            this.AuthUrl = sAuthUrl;
            this.Legal = sLegal;
        }


        public string ClientId { get; set; } = "8fc2abbe-9e9b-4496-8e67-bd08d62dad47";    // Application (client) ID
        public string Secret { get; set; } = "pnJ8Q~fM_2V1NZJ5kfjX.Y481vilbvjX.iANobdW";  // Secret value
        public string Legal { get; set; } = "Cronus USA, Inc.";
        public string DataUrl { get; set; } = "https://api.businesscentral.dynamics.com/v2.0/2518be7e-c933-4905-af64-24ad0157202f/Production/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices";
        public string ScopeUrl { get; set; } = "https://api.businesscentral.dynamics.com/.default";
        public string ActionUrl { get; set; } = "microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices";
        public string AuthUrl { get; set; } = "https://login.microsoftonline.com/2518be7e-c933-4905-af64-24ad0157202f/oauth2/v2.0/token";

    }
}
