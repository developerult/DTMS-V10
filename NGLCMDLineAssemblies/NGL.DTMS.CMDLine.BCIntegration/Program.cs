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

namespace NGL.DTMS.CMDLine.BCIntegration
{
    public class Program
    {


        static string clientId = "8fc2abbe-9e9b-4496-8e67-bd08d62dad47";    // Application (client) ID
        static string secret = "pnJ8Q~fM_2V1NZJ5kfjX.Y481vilbvjX.iANobdW";  // Secret value
        static string tenantId = "2518be7e-c933-4905-af64-24ad0157202f";    // Directory (tenant) ID
        static string sLegal = "Production/WS/CRONUS%20USA%2C%20Inc.";
        static string url = "https://login.microsoftonline.com/" + tenantId + "/oauth2/v2.0/token";

        //https://login.microsoftonline.com/2518be7e-c933-4905-af64-24ad0157202f/oauth2/v2.0/token";
        //https://api.businesscentral.dynamics.com/v2.0/2518be7e-c933-4905-af64-24ad0157202f/Production/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices
        //https://api.businesscentral.dynamics.com/v2.0/" + tenantId + "/" + sLegal + "/Codeunit/DynamicsTMSWebServices


        public static async Task  Main(string[] args)
        {
            string bearerToken = await getBearerAccessToken();
            oAuth2Settings sSettings = new oAuth2Settings(clientId, secret,sLegal, "https://api.businesscentral.dynamics.com/.default", "https://api.businesscentral.dynamics.com/v2.0/2518be7e-c933-4905-af64-24ad0157202f/Production/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices", "microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", url);
            Comp.Envelope oCompanies = await Comp.clsCompanies.Read(bearerToken, sSettings, sLegal, false, true);
            Comp.clsCompanies.Save(oCompanies);

            Carr.Envelope oCarriers = await Carr.clsCarriers.Read(bearerToken, sSettings, sLegal, true, false);
            Carr.clsCarriers.Save(oCarriers);

            Lane.Envelope oLanes = await Lane.clsLanes.Read(bearerToken, sSettings, sLegal, true, false);
            Lane.clsLanes.Save(oLanes);

            Haz.Envelope oHazmats = await Haz.clsHazmats.Read(bearerToken, sSettings, sLegal);
            Haz.clsHazmats.Save(oHazmats);

            Book.Envelope oBookings = await Book.clsBookings.Read(bearerToken, sSettings, sLegal, true, false);
            Book.clsBookings.Save(oBookings);

            Pallet.Envelope oPalletTypes = await Pallet.clsPalletTypes.Read(bearerToken, sSettings, sLegal);
            Pallet.clsPalletTypes.Save(oPalletTypes);

            Payable.Envelope oPayables = await Payable.clsPayables.Read(bearerToken, sSettings, sLegal);
            Payable.clsPayables.Save(oPayables);

            int[] iPLControls = new int[] { 20, 21 };
            Pick.SendPicks oSendPicks = Pick.clsPickLists.generateSamplePickData(iPLControls);
            string sPLBody = Pick.clsPickLists.GetSOAPBody(oSendPicks);
            Pick.Envelope oPicks = await Pick.clsPickLists.Send(bearerToken, sSettings, sLegal, sPLBody);
            Pick.clsPickLists.Save(oPicks);

            int[] iAPControls = new int[] { 14, 15 };
            AP.SendAP oSendAP = AP.clsAPExports.generateSampleAPData(iAPControls);
            string sAPXML = AP.clsAPExports.GetSOAPBody(oSendAP);
            Console.WriteLine(sAPXML);
            AP.Envelope oAPs = await AP.clsAPExports.Read(bearerToken, sSettings, sLegal, sAPXML);
            AP.clsAPExports.Save(oAPs);

            Console.WriteLine("break");
            //string sUri = "https://api.businesscentral.dynamics.com/v2.0/" + tenantId + "/Production/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices";
            //string xUri = "https://api.businesscentral.dynamics.com/v2.0/" + tenantId + "/Production/ODataV4/Company('CRONUS%20USA%2C%20Inc.')/Chart_of_Accounts";
            //string nUri = "https://api.businesscentral.dynamics.com/v2.0/" + tenantId + "/Production/ODataV4/Company('CRONUS%20USA%2C%20Inc.')/DynamicsTMSWebServices";

            ////var oRes =  getToken();
            //await sendRequest("https://api.businesscentral.dynamics.com/v2.0/" + tenantId + "/Production/ODataV4/Company('CRONUS%20USA%2C%20Inc.')/Chart_of_Accounts");

            //var oRet = getRestSharpWSDL(oRes);
            //var oRet = getWSDL(oRes);
        }

        public static APITokenData getToken()
        {
            APITokenResponse oRet = new APITokenResponse();
            // object tmp = null;
            try
            {

                var client = new RestClient("https://login.microsoftonline.com/2518be7e-c933-4905-af64-24ad0157202f/oauth2/v2.0/token");
                
                RestRequest request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("scope", "https://api.businesscentral.dynamics.com/.default", ParameterType.GetOrPost);
                request.AddParameter("client_id", "8fc2abbe-9e9b-4496-8e67-bd08d62dad47", ParameterType.GetOrPost);
                request.AddParameter("client_secret", "pnJ8Q~fM_2V1NZJ5kfjX.Y481vilbvjX.iANobdW", ParameterType.GetOrPost);
                //request.AddParameter("audience", saudience, ParameterType.GetOrPost); //https://api.businesscentral.dynamics.com/.default
                request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);
                oRet.response = client.Execute(request);

                APITokenData oTokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<APITokenData>(oRet.response.Content);
                return oTokenData;
             
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return null;
        }

        public static  bool getRestSharpWSDL(APITokenData oTokenData)
        {
            bool bRet = false;
            var options = new RestClientOptions("https://api.businesscentral.dynamics.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/v2.0/2518be7e-c933-4905-af64-24ad0157202f/Production/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices", Method.Get);
            request.AddHeader("Authorization", "Bearer " + oTokenData.access_token);
            //request.AddHeader("Content-Type", "text/plain");
            //var body = "";
            //request.AddParameter("text/plain", body, ParameterType.RequestBody);
            RestResponse response =  client.Execute(request);
            Console.WriteLine(response.Content);
            bRet = true;
            return bRet;
        }

        public static bool getWSDL(APITokenData oTokenData)
        {
            bool bRet = false;
            var request = (HttpWebRequest)WebRequest.Create("https://api.businesscentral.dynamics.com/v2.0/2518be7e-c933-4905-af64-24ad0157202f/Production/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices");
            string postData = ""; // oRequest.postData();
            var data = Encoding.ASCII.GetBytes(postData);
            request.Headers.Add("Authorization", "Bearer " + oTokenData.access_token);
            //request.Headers.Add("Host", "api.businesscentral.dynamics.com");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream eData = response.GetResponseStream())
                    using (var reader = new StreamReader(eData))
                    {
                        string sTxtMsg = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(sTxtMsg))
                        {
                            Console.WriteLine("Failed.  The actual Error is: " + e.Message, "", "");
                        }
                        else
                        {
                            Console.WriteLine("Failed.  The actual Error is: " + sTxtMsg, "", "");
                        }
                    }
                }
            }


            return bRet;
        }

        static async Task<string> getBearerAccessToken()
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent("grant_type=client_credentials&scope=https://api.businesscentral.dynamics.com/.default&client_id="
            + HttpUtility.UrlEncode(clientId) + "&client_secret=" + HttpUtility.UrlEncode(secret));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());
                return result["access_token"].ToString();
            }
            else
            {
                Console.WriteLine("Error from getting access token: " + response.StatusCode.ToString());
                return "";
            }
        }

        static async Task sendRequest(string requestApiUrl)
        {
            string bearerToken = await getBearerAccessToken();
            if (bearerToken == "") return;
            HttpClient requestHttpClient = new HttpClient();
            requestHttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
            var requestResponse = await requestHttpClient.GetAsync(requestApiUrl);
            if (requestResponse.IsSuccessStatusCode)
                Console.WriteLine(await requestResponse.Content.ReadAsStringAsync());
            else
                Console.WriteLine("Error from sending request: " + requestResponse.StatusCode.ToString());
        }

       
        //public bool doW()
        //{
        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.businesscentral.dynamics.com/v2.0/2518be7e-c933-4905-af64-24ad0157202f/Production/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices");
        //    request.Headers.Add("SOAPAction", "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices:GetCompanies");
        //    request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSIsImtpZCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSJ9.eyJhdWQiOiJodHRwczovL2FwaS5idXNpbmVzc2NlbnRyYWwuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMjUxOGJlN2UtYzkzMy00OTA1LWFmNjQtMjRhZDAxNTcyMDJmLyIsImlhdCI6MTY5ODg2MDI5OSwibmJmIjoxNjk4ODYwMjk5LCJleHAiOjE2OTg4NjQxOTksImFpbyI6IkUyRmdZRGg3cHZaaHdOcGpyak0vWExHWm43eWZIUUE9IiwiYXBwaWQiOiI4ZmMyYWJiZS05ZTliLTQ0OTYtOGU2Ny1iZDA4ZDYyZGFkNDciLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yNTE4YmU3ZS1jOTMzLTQ5MDUtYWY2NC0yNGFkMDE1NzIwMmYvIiwiaWR0eXAiOiJhcHAiLCJvaWQiOiI4NmVmMzA1My04OTdkLTRhYzctODljNC1mODkwZWM5ZGJjNWYiLCJyaCI6IjAuQVc0QWZyNFlKVFBKQlVtdlpDU3RBVmNnTHozdmJabHNzMU5CaGdlbV9Ud0J1Sjl1QUFBLiIsInJvbGVzIjpbIkF1dG9tYXRpb24uUmVhZFdyaXRlLkFsbCIsImFwcF9hY2Nlc3MiLCJBUEkuUmVhZFdyaXRlLkFsbCJdLCJzdWIiOiI4NmVmMzA1My04OTdkLTRhYzctODljNC1mODkwZWM5ZGJjNWYiLCJ0aWQiOiIyNTE4YmU3ZS1jOTMzLTQ5MDUtYWY2NC0yNGFkMDE1NzIwMmYiLCJ1dGkiOiJtWWM0WG8zRVZrS2w1NGl0SVZRb0FBIiwidmVyIjoiMS4wIn0.kEV2j6ywfW6Ved6jAH7J_wSBQvxgc7VHGpCMrt5UKnqkvyn5oGvSAxVsxPjwlcjNdhQpWmF8y6k_cJFxOjzjbG9LR61XKMWRvlg70kcj5m2Al-EA9OXcwL1mvgjeboWxnStqtPnKAuM__pT0mh-7v_-PBXsQOmZUt7tQFta9gYUAQeZBgbgHxQ47wMDS2JZIDKkIPjzgOKaGcw6r2_NBuZGfcNxfTNOdv3zJCM5FB9hWKKfGAnNHo5JojNIQdEebO_pSsj8Syj8rK8ZacxXsM0vE9iPOkqmZcBi4zJgc4A983-i737Q88Cpb3Nj2S0hsoEmP8PNwzg9vOnZxMRBoDQ");
        //    var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetCompanies xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\"><dynamicsTMSCompanies /><readAllHistory>false</readAllHistory><flagRead>true</flagRead></GetCompanies></soap:Body></soap:Envelope>\r\n", null, "text/xml");
        //    request.Content = content;
        //    var response = await client.SendAsync(request);
        //    response.EnsureSuccessStatusCode();
        //    Console.WriteLine(await response.Content.ReadAsStringAsync());

        //}
    }

    public class APITokenData
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }

    public class APITokenResponse
    {
        public APITokenData tokenresponse { get; set; }

        public string error_code { get; set; }
        public string error_msg { get; set; }
        public bool success { get; set; }
        public RestSharp.RestResponse response { get; set; }
        

    }



    //// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    //public partial class Envelope
    //{

    //    private EnvelopeBody bodyField;

    //    /// <remarks/>
    //    public EnvelopeBody Body
    //    {
    //        get
    //        {
    //            return this.bodyField;
    //        }
    //        set
    //        {
    //            this.bodyField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    //public partial class EnvelopeBody
    //{

    //    private GetCompanies_Result getCompanies_ResultField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    //    public GetCompanies_Result GetCompanies_Result
    //    {
    //        get
    //        {
    //            return this.getCompanies_ResultField;
    //        }
    //        set
    //        {
    //            this.getCompanies_ResultField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    //public partial class GetCompanies_Result
    //{

    //    private GetCompanies_ResultDynamicsTMSCompanies dynamicsTMSCompaniesField;

    //    /// <remarks/>
    //    public GetCompanies_ResultDynamicsTMSCompanies dynamicsTMSCompanies
    //    {
    //        get
    //        {
    //            return this.dynamicsTMSCompaniesField;
    //        }
    //        set
    //        {
    //            this.dynamicsTMSCompaniesField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    //public partial class GetCompanies_ResultDynamicsTMSCompanies
    //{

    //    private Company companyField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCompany")]
    //    public Company Company
    //    {
    //        get
    //        {
    //            return this.companyField;
    //        }
    //        set
    //        {
    //            this.companyField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCompany")]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCompany", IsNullable = false)]
    //public partial class Company
    //{

    //    private object compAlphaCodeField;

    //    private object compLegalEntityField;

    //    private object compNumberField;

    //    private object compNameField;

    //    private byte compNatNumberField;

    //    private object compNatNameField;

    //    private object compStreetAddress1Field;

    //    private object compStreetAddress2Field;

    //    private object compStreetAddress3Field;

    //    private object compStreetCityField;

    //    private object compStreetStateField;

    //    private object compStreetCountryField;

    //    private object compStreetZipField;

    //    private object compMailAddress1Field;

    //    private object compMailAddress2Field;

    //    private object compMailAddress3Field;

    //    private object compMailCityField;

    //    private object compMailStateField;

    //    private object compMailCountryField;

    //    private object compMailZipField;

    //    private object compWebField;

    //    private object compEmailField;

    //    private object compDirectionsField;

    //    private object compAbrevField;

    //    private bool compActiveField;

    //    private object compNEXTrackField;

    //    private object compNEXTStopAcctNoField;

    //    private object compNEXTStopPswField;

    //    private bool compNEXTStopSubmitRFPField;

    //    private object compFAAShipIDField;

    //    private object compFAAShipDateField;

    //    private object compFinDunsField;

    //    private object compFinTaxIDField;

    //    private byte compFinPaymentFormField;

    //    private object compFinSICField;

    //    private byte compFinPaymentDiscountField;

    //    private byte[] compFinPaymentDaysField;

    //    private object compFinCommTermsField;

    //    private decimal compFinCommTermsPerField;

    //    private byte compFinCreditLimitField;

    //    private byte compFinCreditUsedField;

    //    private bool compFinInvPrnCodeField;

    //    private bool compFinInvEMailCodeField;

    //    private byte compFinCurTypeField;

    //    private decimal compFinFBToleranceHighField;

    //    private decimal compFinFBToleranceLowField;

    //    private object compFinCustomerSinceField;

    //    private object compFinCardTypeField;

    //    private object compFinCardNameField;

    //    private object compFinCardExpiresField;

    //    private object compFinCardAuthorizorField;

    //    private object compFinCardAuthPasswordField;

    //    private bool compFinUseImportFrtCostField;

    //    private decimal compFinBkhlCostPercField;

    //    private decimal compLatitudeField;

    //    private decimal compLongitudeField;

    //    private object compMailToField;

    //    /// <remarks/>
    //    public object CompAlphaCode
    //    {
    //        get
    //        {
    //            return this.compAlphaCodeField;
    //        }
    //        set
    //        {
    //            this.compAlphaCodeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompLegalEntity
    //    {
    //        get
    //        {
    //            return this.compLegalEntityField;
    //        }
    //        set
    //        {
    //            this.compLegalEntityField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompNumber
    //    {
    //        get
    //        {
    //            return this.compNumberField;
    //        }
    //        set
    //        {
    //            this.compNumberField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompName
    //    {
    //        get
    //        {
    //            return this.compNameField;
    //        }
    //        set
    //        {
    //            this.compNameField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public byte CompNatNumber
    //    {
    //        get
    //        {
    //            return this.compNatNumberField;
    //        }
    //        set
    //        {
    //            this.compNatNumberField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompNatName
    //    {
    //        get
    //        {
    //            return this.compNatNameField;
    //        }
    //        set
    //        {
    //            this.compNatNameField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompStreetAddress1
    //    {
    //        get
    //        {
    //            return this.compStreetAddress1Field;
    //        }
    //        set
    //        {
    //            this.compStreetAddress1Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompStreetAddress2
    //    {
    //        get
    //        {
    //            return this.compStreetAddress2Field;
    //        }
    //        set
    //        {
    //            this.compStreetAddress2Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompStreetAddress3
    //    {
    //        get
    //        {
    //            return this.compStreetAddress3Field;
    //        }
    //        set
    //        {
    //            this.compStreetAddress3Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompStreetCity
    //    {
    //        get
    //        {
    //            return this.compStreetCityField;
    //        }
    //        set
    //        {
    //            this.compStreetCityField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompStreetState
    //    {
    //        get
    //        {
    //            return this.compStreetStateField;
    //        }
    //        set
    //        {
    //            this.compStreetStateField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompStreetCountry
    //    {
    //        get
    //        {
    //            return this.compStreetCountryField;
    //        }
    //        set
    //        {
    //            this.compStreetCountryField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompStreetZip
    //    {
    //        get
    //        {
    //            return this.compStreetZipField;
    //        }
    //        set
    //        {
    //            this.compStreetZipField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompMailAddress1
    //    {
    //        get
    //        {
    //            return this.compMailAddress1Field;
    //        }
    //        set
    //        {
    //            this.compMailAddress1Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompMailAddress2
    //    {
    //        get
    //        {
    //            return this.compMailAddress2Field;
    //        }
    //        set
    //        {
    //            this.compMailAddress2Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompMailAddress3
    //    {
    //        get
    //        {
    //            return this.compMailAddress3Field;
    //        }
    //        set
    //        {
    //            this.compMailAddress3Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompMailCity
    //    {
    //        get
    //        {
    //            return this.compMailCityField;
    //        }
    //        set
    //        {
    //            this.compMailCityField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompMailState
    //    {
    //        get
    //        {
    //            return this.compMailStateField;
    //        }
    //        set
    //        {
    //            this.compMailStateField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompMailCountry
    //    {
    //        get
    //        {
    //            return this.compMailCountryField;
    //        }
    //        set
    //        {
    //            this.compMailCountryField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompMailZip
    //    {
    //        get
    //        {
    //            return this.compMailZipField;
    //        }
    //        set
    //        {
    //            this.compMailZipField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompWeb
    //    {
    //        get
    //        {
    //            return this.compWebField;
    //        }
    //        set
    //        {
    //            this.compWebField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompEmail
    //    {
    //        get
    //        {
    //            return this.compEmailField;
    //        }
    //        set
    //        {
    //            this.compEmailField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompDirections
    //    {
    //        get
    //        {
    //            return this.compDirectionsField;
    //        }
    //        set
    //        {
    //            this.compDirectionsField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompAbrev
    //    {
    //        get
    //        {
    //            return this.compAbrevField;
    //        }
    //        set
    //        {
    //            this.compAbrevField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public bool CompActive
    //    {
    //        get
    //        {
    //            return this.compActiveField;
    //        }
    //        set
    //        {
    //            this.compActiveField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompNEXTrack
    //    {
    //        get
    //        {
    //            return this.compNEXTrackField;
    //        }
    //        set
    //        {
    //            this.compNEXTrackField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompNEXTStopAcctNo
    //    {
    //        get
    //        {
    //            return this.compNEXTStopAcctNoField;
    //        }
    //        set
    //        {
    //            this.compNEXTStopAcctNoField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompNEXTStopPsw
    //    {
    //        get
    //        {
    //            return this.compNEXTStopPswField;
    //        }
    //        set
    //        {
    //            this.compNEXTStopPswField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public bool CompNEXTStopSubmitRFP
    //    {
    //        get
    //        {
    //            return this.compNEXTStopSubmitRFPField;
    //        }
    //        set
    //        {
    //            this.compNEXTStopSubmitRFPField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFAAShipID
    //    {
    //        get
    //        {
    //            return this.compFAAShipIDField;
    //        }
    //        set
    //        {
    //            this.compFAAShipIDField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFAAShipDate
    //    {
    //        get
    //        {
    //            return this.compFAAShipDateField;
    //        }
    //        set
    //        {
    //            this.compFAAShipDateField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinDuns
    //    {
    //        get
    //        {
    //            return this.compFinDunsField;
    //        }
    //        set
    //        {
    //            this.compFinDunsField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinTaxID
    //    {
    //        get
    //        {
    //            return this.compFinTaxIDField;
    //        }
    //        set
    //        {
    //            this.compFinTaxIDField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public byte CompFinPaymentForm
    //    {
    //        get
    //        {
    //            return this.compFinPaymentFormField;
    //        }
    //        set
    //        {
    //            this.compFinPaymentFormField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinSIC
    //    {
    //        get
    //        {
    //            return this.compFinSICField;
    //        }
    //        set
    //        {
    //            this.compFinSICField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public byte CompFinPaymentDiscount
    //    {
    //        get
    //        {
    //            return this.compFinPaymentDiscountField;
    //        }
    //        set
    //        {
    //            this.compFinPaymentDiscountField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("CompFinPaymentDays")]
    //    public byte[] CompFinPaymentDays
    //    {
    //        get
    //        {
    //            return this.compFinPaymentDaysField;
    //        }
    //        set
    //        {
    //            this.compFinPaymentDaysField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinCommTerms
    //    {
    //        get
    //        {
    //            return this.compFinCommTermsField;
    //        }
    //        set
    //        {
    //            this.compFinCommTermsField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal CompFinCommTermsPer
    //    {
    //        get
    //        {
    //            return this.compFinCommTermsPerField;
    //        }
    //        set
    //        {
    //            this.compFinCommTermsPerField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public byte CompFinCreditLimit
    //    {
    //        get
    //        {
    //            return this.compFinCreditLimitField;
    //        }
    //        set
    //        {
    //            this.compFinCreditLimitField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public byte CompFinCreditUsed
    //    {
    //        get
    //        {
    //            return this.compFinCreditUsedField;
    //        }
    //        set
    //        {
    //            this.compFinCreditUsedField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public bool CompFinInvPrnCode
    //    {
    //        get
    //        {
    //            return this.compFinInvPrnCodeField;
    //        }
    //        set
    //        {
    //            this.compFinInvPrnCodeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public bool CompFinInvEMailCode
    //    {
    //        get
    //        {
    //            return this.compFinInvEMailCodeField;
    //        }
    //        set
    //        {
    //            this.compFinInvEMailCodeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public byte CompFinCurType
    //    {
    //        get
    //        {
    //            return this.compFinCurTypeField;
    //        }
    //        set
    //        {
    //            this.compFinCurTypeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal CompFinFBToleranceHigh
    //    {
    //        get
    //        {
    //            return this.compFinFBToleranceHighField;
    //        }
    //        set
    //        {
    //            this.compFinFBToleranceHighField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal CompFinFBToleranceLow
    //    {
    //        get
    //        {
    //            return this.compFinFBToleranceLowField;
    //        }
    //        set
    //        {
    //            this.compFinFBToleranceLowField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinCustomerSince
    //    {
    //        get
    //        {
    //            return this.compFinCustomerSinceField;
    //        }
    //        set
    //        {
    //            this.compFinCustomerSinceField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinCardType
    //    {
    //        get
    //        {
    //            return this.compFinCardTypeField;
    //        }
    //        set
    //        {
    //            this.compFinCardTypeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinCardName
    //    {
    //        get
    //        {
    //            return this.compFinCardNameField;
    //        }
    //        set
    //        {
    //            this.compFinCardNameField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinCardExpires
    //    {
    //        get
    //        {
    //            return this.compFinCardExpiresField;
    //        }
    //        set
    //        {
    //            this.compFinCardExpiresField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinCardAuthorizor
    //    {
    //        get
    //        {
    //            return this.compFinCardAuthorizorField;
    //        }
    //        set
    //        {
    //            this.compFinCardAuthorizorField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompFinCardAuthPassword
    //    {
    //        get
    //        {
    //            return this.compFinCardAuthPasswordField;
    //        }
    //        set
    //        {
    //            this.compFinCardAuthPasswordField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public bool CompFinUseImportFrtCost
    //    {
    //        get
    //        {
    //            return this.compFinUseImportFrtCostField;
    //        }
    //        set
    //        {
    //            this.compFinUseImportFrtCostField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal CompFinBkhlCostPerc
    //    {
    //        get
    //        {
    //            return this.compFinBkhlCostPercField;
    //        }
    //        set
    //        {
    //            this.compFinBkhlCostPercField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal CompLatitude
    //    {
    //        get
    //        {
    //            return this.compLatitudeField;
    //        }
    //        set
    //        {
    //            this.compLatitudeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public decimal CompLongitude
    //    {
    //        get
    //        {
    //            return this.compLongitudeField;
    //        }
    //        set
    //        {
    //            this.compLongitudeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    public object CompMailTo
    //    {
    //        get
    //        {
    //            return this.compMailToField;
    //        }
    //        set
    //        {
    //            this.compMailToField = value;
    //        }
    //    }
    //}




}
