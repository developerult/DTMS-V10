using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NGL.DTMS.CMDLine.BCIntegration.Haz
{
    public class clsHazmats
    {

        public static async Task<Envelope> Read(string bearerToken, oAuth2Settings sSettings, string sLegal)
        {
            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":GetHazmat");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetHazmat xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\"><dynamicsTMSHazmats /></GetHazmat></soap:Body></soap:Envelope>\r\n", null, "text/xml");
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

        public static bool Save(Envelope oHazmats)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oHazmats != null && oHazmats.Body != null && oHazmats.Body.GetHazmat_Result != null && oHazmats.Body.GetHazmat_Result.dynamicsTMSHazmats != null && oHazmats.Body.GetHazmat_Result.dynamicsTMSHazmats.Length > 0)
            {
                foreach (Hazmat item in oHazmats.Body.GetHazmat_Result.dynamicsTMSHazmats)
                {
                    Console.WriteLine("Name: " + item.HazClass.ToString());
                }
                blnRet = true;
                Console.WriteLine("Save Hazmat complete");
            }
            else
            {
                Console.WriteLine("Save Hazmat data failed, data not available");
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

        private GetHazmat_Result getHazmat_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public GetHazmat_Result GetHazmat_Result
        {
            get
            {
                return this.getHazmat_ResultField;
            }
            set
            {
                this.getHazmat_ResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    public partial class GetHazmat_Result
    {

        private Hazmat[] dynamicsTMSHazmatsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Hazmat", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSHazmat", IsNullable = false)]
        public Hazmat[] dynamicsTMSHazmats
        {
            get
            {
                return this.dynamicsTMSHazmatsField;
            }
            set
            {
                this.dynamicsTMSHazmatsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSHazmat")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSHazmat", IsNullable = false)]
    public partial class Hazmat
    {

        private string hazRegulationField;

        private string hazItemField;

        private string hazClassField;

        private string hazIDField;

        private string hazDesc01Field;

        private string hazDesc02Field;

        private string hazDesc03Field;

        private string hazUnitField;

        private string hazPackingGroupField;

        private string hazPackingDescField;

        private string hazShipInstField;

        private bool hazLtdQField;

        private bool hazMarPollField;

        private string hazMarStorCatField;

        private byte hazNMFCSubField;

        private byte hazNMFCField;

        private byte hazFrtClassField;

        private bool hazFdxGndOKField;

        private bool hazFdxAirOKField;

        private bool hazUPSgndOKField;

        private bool hazUPSAirOKField;

        /// <remarks/>
        public string HazRegulation
        {
            get
            {
                return this.hazRegulationField;
            }
            set
            {
                this.hazRegulationField = value;
            }
        }

        /// <remarks/>
        public string HazItem
        {
            get
            {
                return this.hazItemField;
            }
            set
            {
                this.hazItemField = value;
            }
        }

        /// <remarks/>
        public string HazClass
        {
            get
            {
                return this.hazClassField;
            }
            set
            {
                this.hazClassField = value;
            }
        }

        /// <remarks/>
        public string HazID
        {
            get
            {
                return this.hazIDField;
            }
            set
            {
                this.hazIDField = value;
            }
        }

        /// <remarks/>
        public string HazDesc01
        {
            get
            {
                return this.hazDesc01Field;
            }
            set
            {
                this.hazDesc01Field = value;
            }
        }

        /// <remarks/>
        public string HazDesc02
        {
            get
            {
                return this.hazDesc02Field;
            }
            set
            {
                this.hazDesc02Field = value;
            }
        }

        /// <remarks/>
        public string HazDesc03
        {
            get
            {
                return this.hazDesc03Field;
            }
            set
            {
                this.hazDesc03Field = value;
            }
        }

        /// <remarks/>
        public string HazUnit
        {
            get
            {
                return this.hazUnitField;
            }
            set
            {
                this.hazUnitField = value;
            }
        }

        /// <remarks/>
        public string HazPackingGroup
        {
            get
            {
                return this.hazPackingGroupField;
            }
            set
            {
                this.hazPackingGroupField = value;
            }
        }

        /// <remarks/>
        public string HazPackingDesc
        {
            get
            {
                return this.hazPackingDescField;
            }
            set
            {
                this.hazPackingDescField = value;
            }
        }

        /// <remarks/>
        public string HazShipInst
        {
            get
            {
                return this.hazShipInstField;
            }
            set
            {
                this.hazShipInstField = value;
            }
        }

        /// <remarks/>
        public bool HazLtdQ
        {
            get
            {
                return this.hazLtdQField;
            }
            set
            {
                this.hazLtdQField = value;
            }
        }

        /// <remarks/>
        public bool HazMarPoll
        {
            get
            {
                return this.hazMarPollField;
            }
            set
            {
                this.hazMarPollField = value;
            }
        }

        /// <remarks/>
        public string HazMarStorCat
        {
            get
            {
                return this.hazMarStorCatField;
            }
            set
            {
                this.hazMarStorCatField = value;
            }
        }

        /// <remarks/>
        public byte HazNMFCSub
        {
            get
            {
                return this.hazNMFCSubField;
            }
            set
            {
                this.hazNMFCSubField = value;
            }
        }

        /// <remarks/>
        public byte HazNMFC
        {
            get
            {
                return this.hazNMFCField;
            }
            set
            {
                this.hazNMFCField = value;
            }
        }

        /// <remarks/>
        public byte HazFrtClass
        {
            get
            {
                return this.hazFrtClassField;
            }
            set
            {
                this.hazFrtClassField = value;
            }
        }

        /// <remarks/>
        public bool HazFdxGndOK
        {
            get
            {
                return this.hazFdxGndOKField;
            }
            set
            {
                this.hazFdxGndOKField = value;
            }
        }

        /// <remarks/>
        public bool HazFdxAirOK
        {
            get
            {
                return this.hazFdxAirOKField;
            }
            set
            {
                this.hazFdxAirOKField = value;
            }
        }

        /// <remarks/>
        public bool HazUPSgndOK
        {
            get
            {
                return this.hazUPSgndOKField;
            }
            set
            {
                this.hazUPSgndOKField = value;
            }
        }

        /// <remarks/>
        public bool HazUPSAirOK
        {
            get
            {
                return this.hazUPSAirOKField;
            }
            set
            {
                this.hazUPSAirOKField = value;
            }
        }
    }





}
