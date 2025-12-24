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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
//using NGLNAV = NGL.FM.CMDLine.NAVIntegration;

namespace NGL.DTMS.CMDLine.BCIntegration.Company
{
  
    public class clsCompanies
    {
        public static async Task<Envelope> Read(string bearerToken, oAuth2Settings sSettings, string sLegal, bool readAllHistory = false, bool markAsRead = true)
        {

            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":GetCompanies");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetCompanies xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\"><dynamicsTMSCompanies /><readAllHistory>" + readAllHistory.ToString().ToLower() + "</readAllHistory><flagRead>" + markAsRead.ToString().ToLower() + "</flagRead></GetCompanies></soap:Body></soap:Envelope>\r\n", null, "text/xml");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string xml = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(xml);

                XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
                using (StringReader reader = new StringReader(xml))
                {
                    oRet = (Envelope)serializer.Deserialize(reader);
                }
            }
            catch (System.InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return oRet;

        }

        public static bool Save(Envelope oCompanies)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oCompanies != null && oCompanies.Body != null && oCompanies.Body.GetCompanies_Result != null && oCompanies.Body.GetCompanies_Result.dynamicsTMSCompanies != null && oCompanies.Body.GetCompanies_Result.dynamicsTMSCompanies.Length > 0)
            {
                foreach (Company c in oCompanies.Body.GetCompanies_Result.dynamicsTMSCompanies)
                {
                    Console.WriteLine("Name: " + c.CompName.ToString());
                }
                blnRet = true;
                Console.WriteLine("Save Companies Complete");
            }
            else
            {
                Console.WriteLine("Save Company data failed, data not available");
            }

            return blnRet;
        }

    }

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            private EnvelopeBody bodyField;

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            private GetCompanies_Result getCompanies_ResultField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
            public GetCompanies_Result GetCompanies_Result
            {
                get
                {
                    return this.getCompanies_ResultField;
                }
                set
                {
                    this.getCompanies_ResultField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
        public partial class GetCompanies_Result
        {

            private Company[] dynamicsTMSCompaniesField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Company", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCompany", IsNullable = false)]
            public Company[] dynamicsTMSCompanies
            {
                get
                {
                    return this.dynamicsTMSCompaniesField;
                }
                set
                {
                    this.dynamicsTMSCompaniesField = value;
                }
            }
        }


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCompany")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCompany", IsNullable = false)]
    public partial class Company
    {

        private string compAlphaCodeField;

        private string compLegalEntityField;

        private string compNumberField;

        private string compNameField;

        private string compNatNumberField;

        private string compNatNameField;

        private string compStreetAddress1Field;

        private string compStreetAddress2Field;

        private string compStreetAddress3Field;

        private string compStreetCityField;

        private string compStreetStateField;

        private string compStreetCountryField;

        private string compStreetZipField;

        private string compMailAddress1Field;

        private string compMailAddress2Field;

        private string compMailAddress3Field;

        private string compMailCityField;

        private string compMailStateField;

        private string compMailCountryField;

        private string compMailZipField;

        private string compWebField;

        private string compEmailField;

        private string compDirectionsField;

        private string compAbrevField;

        private bool compActiveField;

        private string compNEXTrackField;

        private string compNEXTStopAcctNoField;

        private string compNEXTStopPswField;

        private bool compNEXTStopSubmitRFPField;

        private string compFAAShipIDField;

        private string compFAAShipDateField;

        private string compFinDunsField;

        private string compFinTaxIDField;

        private string compFinPaymentFormField;

        private string compFinSICField;

        private int compFinPaymentDiscountField;

        private int compFinPaymentDaysField;

        private string compFinCommTermsField;

        private decimal compFinCommTermsPerField;

        private int compFinCreditLimitField;

        private int compFinCreditUsedField;

        private bool compFinInvPrnCodeField;

        private bool compFinInvEMailCodeField;

        private int compFinCurTypeField;

        private decimal compFinFBToleranceHighField;

        private decimal compFinFBToleranceLowField;

        private string compFinCustomerSinceField;

        private string compFinCardTypeField;

        private string compFinCardNameField;

        private string compFinCardExpiresField;

        private string compFinCardAuthorizorField;

        private string compFinCardAuthPasswordField;

        private bool compFinUseImportFrtCostField;

        private decimal compFinBkhlCostPercField;

        private decimal compLatitudeField;

        private decimal compLongitudeField;

        private string compMailToField;

        /// <remarks/>
        public string CompAlphaCode
        {
            get
            {
                return this.compAlphaCodeField;
            }
            set
            {
                this.compAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public string CompLegalEntity
        {
            get
            {
                return this.compLegalEntityField;
            }
            set
            {
                this.compLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string CompNumber
        {
            get
            {
                return this.compNumberField;
            }
            set
            {
                this.compNumberField = value;
            }
        }

        /// <remarks/>
        public string CompName
        {
            get
            {
                return this.compNameField;
            }
            set
            {
                this.compNameField = value;
            }
        }

        /// <remarks/>
        public string CompNatNumber
        {
            get
            {
                return this.compNatNumberField;
            }
            set
            {
                this.compNatNumberField = value;
            }
        }

        /// <remarks/>
        public string CompNatName
        {
            get
            {
                return this.compNatNameField;
            }
            set
            {
                this.compNatNameField = value;
            }
        }

        /// <remarks/>
        public string CompStreetAddress1
        {
            get
            {
                return this.compStreetAddress1Field;
            }
            set
            {
                this.compStreetAddress1Field = value;
            }
        }

        /// <remarks/>
        public string CompStreetAddress2
        {
            get
            {
                return this.compStreetAddress2Field;
            }
            set
            {
                this.compStreetAddress2Field = value;
            }
        }

        /// <remarks/>
        public string CompStreetAddress3
        {
            get
            {
                return this.compStreetAddress3Field;
            }
            set
            {
                this.compStreetAddress3Field = value;
            }
        }

        /// <remarks/>
        public string CompStreetCity
        {
            get
            {
                return this.compStreetCityField;
            }
            set
            {
                this.compStreetCityField = value;
            }
        }

        /// <remarks/>
        public string CompStreetState
        {
            get
            {
                return this.compStreetStateField;
            }
            set
            {
                this.compStreetStateField = value;
            }
        }

        /// <remarks/>
        public string CompStreetCountry
        {
            get
            {
                return this.compStreetCountryField;
            }
            set
            {
                this.compStreetCountryField = value;
            }
        }

        /// <remarks/>
        public string CompStreetZip
        {
            get
            {
                return this.compStreetZipField;
            }
            set
            {
                this.compStreetZipField = value;
            }
        }

        /// <remarks/>
        public string CompMailAddress1
        {
            get
            {
                return this.compMailAddress1Field;
            }
            set
            {
                this.compMailAddress1Field = value;
            }
        }

        /// <remarks/>
        public string CompMailAddress2
        {
            get
            {
                return this.compMailAddress2Field;
            }
            set
            {
                this.compMailAddress2Field = value;
            }
        }

        /// <remarks/>
        public string CompMailAddress3
        {
            get
            {
                return this.compMailAddress3Field;
            }
            set
            {
                this.compMailAddress3Field = value;
            }
        }

        /// <remarks/>
        public string CompMailCity
        {
            get
            {
                return this.compMailCityField;
            }
            set
            {
                this.compMailCityField = value;
            }
        }

        /// <remarks/>
        public string CompMailState
        {
            get
            {
                return this.compMailStateField;
            }
            set
            {
                this.compMailStateField = value;
            }
        }

        /// <remarks/>
        public string CompMailCountry
        {
            get
            {
                return this.compMailCountryField;
            }
            set
            {
                this.compMailCountryField = value;
            }
        }

        /// <remarks/>
        public string CompMailZip
        {
            get
            {
                return this.compMailZipField;
            }
            set
            {
                this.compMailZipField = value;
            }
        }

        /// <remarks/>
        public string CompWeb
        {
            get
            {
                return this.compWebField;
            }
            set
            {
                this.compWebField = value;
            }
        }

        /// <remarks/>
        public string CompEmail
        {
            get
            {
                return this.compEmailField;
            }
            set
            {
                this.compEmailField = value;
            }
        }

        /// <remarks/>
        public string CompDirections
        {
            get
            {
                return this.compDirectionsField;
            }
            set
            {
                this.compDirectionsField = value;
            }
        }

        /// <remarks/>
        public string CompAbrev
        {
            get
            {
                return this.compAbrevField;
            }
            set
            {
                this.compAbrevField = value;
            }
        }

        /// <remarks/>
        public bool CompActive
        {
            get
            {
                return this.compActiveField;
            }
            set
            {
                this.compActiveField = value;
            }
        }

        /// <remarks/>
        public string CompNEXTrack
        {
            get
            {
                return this.compNEXTrackField;
            }
            set
            {
                this.compNEXTrackField = value;
            }
        }

        /// <remarks/>
        public string CompNEXTStopAcctNo
        {
            get
            {
                return this.compNEXTStopAcctNoField;
            }
            set
            {
                this.compNEXTStopAcctNoField = value;
            }
        }

        /// <remarks/>
        public string CompNEXTStopPsw
        {
            get
            {
                return this.compNEXTStopPswField;
            }
            set
            {
                this.compNEXTStopPswField = value;
            }
        }

        /// <remarks/>
        public bool CompNEXTStopSubmitRFP
        {
            get
            {
                return this.compNEXTStopSubmitRFPField;
            }
            set
            {
                this.compNEXTStopSubmitRFPField = value;
            }
        }

        /// <remarks/>
        public string CompFAAShipID
        {
            get
            {
                return this.compFAAShipIDField;
            }
            set
            {
                this.compFAAShipIDField = value;
            }
        }

        /// <remarks/>
        public string CompFAAShipDate
        {
            get
            {
                return this.compFAAShipDateField;
            }
            set
            {
                this.compFAAShipDateField = value;
            }
        }

        /// <remarks/>
        public string CompFinDuns
        {
            get
            {
                return this.compFinDunsField;
            }
            set
            {
                this.compFinDunsField = value;
            }
        }

        /// <remarks/>
        public string CompFinTaxID
        {
            get
            {
                return this.compFinTaxIDField;
            }
            set
            {
                this.compFinTaxIDField = value;
            }
        }

        /// <remarks/>
        public string CompFinPaymentForm
        {
            get
            {
                return this.compFinPaymentFormField;
            }
            set
            {
                this.compFinPaymentFormField = value;
            }
        }

        /// <remarks/>
        public string CompFinSIC
        {
            get
            {
                return this.compFinSICField;
            }
            set
            {
                this.compFinSICField = value;
            }
        }

        /// <remarks/>
        public int CompFinPaymentDiscount
        {
            get
            {
                return this.compFinPaymentDiscountField;
            }
            set
            {
                this.compFinPaymentDiscountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CompFinPaymentDays")]
        public int CompFinPaymentDays
        {
            get
            {
                return this.compFinPaymentDaysField;
            }
            set
            {
                this.compFinPaymentDaysField = value;
            }
        }

        /// <remarks/>
        public string CompFinCommTerms
        {
            get
            {
                return this.compFinCommTermsField;
            }
            set
            {
                this.compFinCommTermsField = value;
            }
        }

        /// <remarks/>
        public decimal CompFinCommTermsPer
        {
            get
            {
                return this.compFinCommTermsPerField;
            }
            set
            {
                this.compFinCommTermsPerField = value;
            }
        }

        /// <remarks/>
        public int CompFinCreditLimit
        {
            get
            {
                return this.compFinCreditLimitField;
            }
            set
            {
                this.compFinCreditLimitField = value;
            }
        }

        /// <remarks/>
        public int CompFinCreditUsed
        {
            get
            {
                return this.compFinCreditUsedField;
            }
            set
            {
                this.compFinCreditUsedField = value;
            }
        }

        /// <remarks/>
        public bool CompFinInvPrnCode
        {
            get
            {
                return this.compFinInvPrnCodeField;
            }
            set
            {
                this.compFinInvPrnCodeField = value;
            }
        }

        /// <remarks/>
        public bool CompFinInvEMailCode
        {
            get
            {
                return this.compFinInvEMailCodeField;
            }
            set
            {
                this.compFinInvEMailCodeField = value;
            }
        }

        /// <remarks/>
        public int CompFinCurType
        {
            get
            {
                return this.compFinCurTypeField;
            }
            set
            {
                this.compFinCurTypeField = value;
            }
        }

        /// <remarks/>
        public decimal CompFinFBToleranceHigh
        {
            get
            {
                return this.compFinFBToleranceHighField;
            }
            set
            {
                this.compFinFBToleranceHighField = value;
            }
        }

        /// <remarks/>
        public decimal CompFinFBToleranceLow
        {
            get
            {
                return this.compFinFBToleranceLowField;
            }
            set
            {
                this.compFinFBToleranceLowField = value;
            }
        }

        /// <remarks/>
        public string CompFinCustomerSince
        {
            get
            {
                return this.compFinCustomerSinceField;
            }
            set
            {
                this.compFinCustomerSinceField = value;
            }
        }

        /// <remarks/>
        public string CompFinCardType
        {
            get
            {
                return this.compFinCardTypeField;
            }
            set
            {
                this.compFinCardTypeField = value;
            }
        }

        /// <remarks/>
        public string CompFinCardName
        {
            get
            {
                return this.compFinCardNameField;
            }
            set
            {
                this.compFinCardNameField = value;
            }
        }

        /// <remarks/>
        public string CompFinCardExpires
        {
            get
            {
                return this.compFinCardExpiresField;
            }
            set
            {
                this.compFinCardExpiresField = value;
            }
        }

        /// <remarks/>
        public string CompFinCardAuthorizor
        {
            get
            {
                return this.compFinCardAuthorizorField;
            }
            set
            {
                this.compFinCardAuthorizorField = value;
            }
        }

        /// <remarks/>
        public string CompFinCardAuthPassword
        {
            get
            {
                return this.compFinCardAuthPasswordField;
            }
            set
            {
                this.compFinCardAuthPasswordField = value;
            }
        }

        /// <remarks/>
        public bool CompFinUseImportFrtCost
        {
            get
            {
                return this.compFinUseImportFrtCostField;
            }
            set
            {
                this.compFinUseImportFrtCostField = value;
            }
        }

        /// <remarks/>
        public decimal CompFinBkhlCostPerc
        {
            get
            {
                return this.compFinBkhlCostPercField;
            }
            set
            {
                this.compFinBkhlCostPercField = value;
            }
        }

        /// <remarks/>
        public decimal CompLatitude
        {
            get
            {
                return this.compLatitudeField;
            }
            set
            {
                this.compLatitudeField = value;
            }
        }

        /// <remarks/>
        public decimal CompLongitude
        {
            get
            {
                return this.compLongitudeField;
            }
            set
            {
                this.compLongitudeField = value;
            }
        }

        /// <remarks/>
        public string CompMailTo
        {
            get
            {
                return this.compMailToField;
            }
            set
            {
                this.compMailToField = value;
            }
        }
    }





}
