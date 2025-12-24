using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NGL.DTMS.CMDLine.BCIntegration.Carrier
{
    public class clsCarriers
    {

        public static async Task<Envelope> Read(string bearerToken, oAuth2Settings sSettings, string sLegal, bool readAllHistory = true, bool markAsRead = true)
        {
            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":GetCarriers");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetCarriers xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\"><dynamicsTMSCarriers /><readAllHistory> " + readAllHistory.ToString().ToLower() + "</readAllHistory><flagRead>" + markAsRead.ToString().ToLower() + "</flagRead></GetCarriers></soap:Body></soap:Envelope>\r\n", null, "text/xml");
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

        public static bool Save(Envelope oCarriers)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oCarriers != null && oCarriers.Body != null && oCarriers.Body.GetCarriers_Result != null && oCarriers.Body.GetCarriers_Result.dynamicsTMSCarriers != null && oCarriers.Body.GetCarriers_Result.dynamicsTMSCarriers.Length > 0)
            {
                foreach (Carrier item in oCarriers.Body.GetCarriers_Result.dynamicsTMSCarriers)
                {
                    Console.WriteLine("Name: " + item.CarrierName.ToString());
                }
                blnRet = true;
                Console.WriteLine("Save Carrier complete");
            }
            else
            {
                Console.WriteLine("Save Carrier data failed, data not available");
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

        private GetCarriers_Result getCarriers_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public GetCarriers_Result GetCarriers_Result
        {
            get
            {
                return this.getCarriers_ResultField;
            }
            set
            {
                this.getCarriers_ResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    public partial class GetCarriers_Result
    {

        private Carrier[] dynamicsTMSCarriersField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Carrier", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCarrier", IsNullable = false)]
        public Carrier[] dynamicsTMSCarriers
        {
            get
            {
                return this.dynamicsTMSCarriersField;
            }
            set
            {
                this.dynamicsTMSCarriersField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCarrier")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSCarrier", IsNullable = false)]
    public partial class Carrier
    {

        private string carrierNumberField;

        private string carrierNameField;

        private string carrierStreetAddress1Field;

        private string carrierStreetAddress2Field;

        private string carrierStreetAddress3Field;

        private string carrierStreetCityField;

        private string carrierStreetStateField;

        private string carrierStreetCountryField;

        private string carrierStreetZipField;

        private string carrierMailAddress1Field;

        private string carrierMailAddress2Field;

        private string carrierMailAddress3Field;

        private string carrierMailCityField;

        private string carrierMailStateField;

        private string carrierMailCountryField;

        private string carrierMailZipField;

        private string carrierTypeCodeField;

        private string carrierSCACField;

        private string carrierWebsiteField;

        private string carrierEmailField;

        private string carrierQualInsturanceDateField;

        private bool carrierQualQualifiedField;

        private string carrierQualAuthorityField;

        private bool carrierQualContractField;

        private string carrierQualSignedDateField;

        private string carrierQualContractExpiresDateField;

        private string carrierLegalEntityField;

        private string carrierAlphaCodeField;

        private string carrierCurrencyTypeField;

        /// <remarks/>
        public string CarrierNumber
        {
            get
            {
                return this.carrierNumberField;
            }
            set
            {
                this.carrierNumberField = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierNameField;
            }
            set
            {
                this.carrierNameField = value;
            }
        }

        /// <remarks/>
        public string CarrierStreetAddress1
        {
            get
            {
                return this.carrierStreetAddress1Field;
            }
            set
            {
                this.carrierStreetAddress1Field = value;
            }
        }

        /// <remarks/>
        public string CarrierStreetAddress2
        {
            get
            {
                return this.carrierStreetAddress2Field;
            }
            set
            {
                this.carrierStreetAddress2Field = value;
            }
        }

        /// <remarks/>
        public string CarrierStreetAddress3
        {
            get
            {
                return this.carrierStreetAddress3Field;
            }
            set
            {
                this.carrierStreetAddress3Field = value;
            }
        }

        /// <remarks/>
        public string CarrierStreetCity
        {
            get
            {
                return this.carrierStreetCityField;
            }
            set
            {
                this.carrierStreetCityField = value;
            }
        }

        /// <remarks/>
        public string CarrierStreetState
        {
            get
            {
                return this.carrierStreetStateField;
            }
            set
            {
                this.carrierStreetStateField = value;
            }
        }

        /// <remarks/>
        public string CarrierStreetCountry
        {
            get
            {
                return this.carrierStreetCountryField;
            }
            set
            {
                this.carrierStreetCountryField = value;
            }
        }

        /// <remarks/>
        public string CarrierStreetZip
        {
            get
            {
                return this.carrierStreetZipField;
            }
            set
            {
                this.carrierStreetZipField = value;
            }
        }

        /// <remarks/>
        public string CarrierMailAddress1
        {
            get
            {
                return this.carrierMailAddress1Field;
            }
            set
            {
                this.carrierMailAddress1Field = value;
            }
        }

        /// <remarks/>
        public string CarrierMailAddress2
        {
            get
            {
                return this.carrierMailAddress2Field;
            }
            set
            {
                this.carrierMailAddress2Field = value;
            }
        }

        /// <remarks/>
        public string CarrierMailAddress3
        {
            get
            {
                return this.carrierMailAddress3Field;
            }
            set
            {
                this.carrierMailAddress3Field = value;
            }
        }

        /// <remarks/>
        public string CarrierMailCity
        {
            get
            {
                return this.carrierMailCityField;
            }
            set
            {
                this.carrierMailCityField = value;
            }
        }

        /// <remarks/>
        public string CarrierMailState
        {
            get
            {
                return this.carrierMailStateField;
            }
            set
            {
                this.carrierMailStateField = value;
            }
        }

        /// <remarks/>
        public string CarrierMailCountry
        {
            get
            {
                return this.carrierMailCountryField;
            }
            set
            {
                this.carrierMailCountryField = value;
            }
        }

        /// <remarks/>
        public string CarrierMailZip
        {
            get
            {
                return this.carrierMailZipField;
            }
            set
            {
                this.carrierMailZipField = value;
            }
        }

        /// <remarks/>
        public string CarrierTypeCode
        {
            get
            {
                return this.carrierTypeCodeField;
            }
            set
            {
                this.carrierTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string CarrierSCAC
        {
            get
            {
                return this.carrierSCACField;
            }
            set
            {
                this.carrierSCACField = value;
            }
        }

        /// <remarks/>
        public string CarrierWebsite
        {
            get
            {
                return this.carrierWebsiteField;
            }
            set
            {
                this.carrierWebsiteField = value;
            }
        }

        /// <remarks/>
        public string CarrierEmail
        {
            get
            {
                return this.carrierEmailField;
            }
            set
            {
                this.carrierEmailField = value;
            }
        }

        /// <remarks/>
        public string CarrierQualInsturanceDate
        {
            get
            {
                return this.carrierQualInsturanceDateField;
            }
            set
            {
                this.carrierQualInsturanceDateField = value;
            }
        }

        /// <remarks/>
        public bool CarrierQualQualified
        {
            get
            {
                return this.carrierQualQualifiedField;
            }
            set
            {
                this.carrierQualQualifiedField = value;
            }
        }

        /// <remarks/>
        public string CarrierQualAuthority
        {
            get
            {
                return this.carrierQualAuthorityField;
            }
            set
            {
                this.carrierQualAuthorityField = value;
            }
        }

        /// <remarks/>
        public bool CarrierQualContract
        {
            get
            {
                return this.carrierQualContractField;
            }
            set
            {
                this.carrierQualContractField = value;
            }
        }

        /// <remarks/>
        public String CarrierQualSignedDate
        {
            get
            {
                return this.carrierQualSignedDateField;
            }
            set
            {
                this.carrierQualSignedDateField = value;
            }
        }

        /// <remarks/>
        public string CarrierQualContractExpiresDate
        {
            get
            {
                return this.carrierQualContractExpiresDateField;
            }
            set
            {
                this.carrierQualContractExpiresDateField = value;
            }
        }

        /// <remarks/>
        public string CarrierLegalEntity
        {
            get
            {
                return this.carrierLegalEntityField;
            }
            set
            {
                this.carrierLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string CarrierAlphaCode
        {
            get
            {
                return this.carrierAlphaCodeField;
            }
            set
            {
                this.carrierAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public string CarrierCurrencyType
        {
            get
            {
                return this.carrierCurrencyTypeField;
            }
            set
            {
                this.carrierCurrencyTypeField = value;
            }
        }
    }



}
