using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NGL.DTMS.CMDLine.BCIntegration.Plt
{
    public  class clsPalletTypes
    {


        public static async Task<Envelope> Read(string bearerToken, oAuth2Settings sSettings, string sLegal)
        {
            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":GetPalletType");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetPalletType xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\"><dynamicsTMSPalletTypes /></GetPalletType></soap:Body></soap:Envelope>\r\n", null, "text/xml");
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

        public static bool Save(Envelope oPalletTypes)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oPalletTypes != null && oPalletTypes.Body != null && oPalletTypes.Body.GetPalletType_Result != null && oPalletTypes.Body.GetPalletType_Result.dynamicsTMSPalletTypes != null && oPalletTypes.Body.GetPalletType_Result.dynamicsTMSPalletTypes.Length > 0)
            {
                foreach (PalletType item in oPalletTypes.Body.GetPalletType_Result.dynamicsTMSPalletTypes)
                {
                    Console.WriteLine("Name: " + item.PalletType1.ToString());
                }
                blnRet = true;
                Console.WriteLine("Save PalletType complete");
            }
            else
            {
                Console.WriteLine("Save PalletType data failed, data not available");
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

        private GetPalletType_Result getPalletType_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public GetPalletType_Result GetPalletType_Result
        {
            get
            {
                return this.getPalletType_ResultField;
            }
            set
            {
                this.getPalletType_ResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    public partial class GetPalletType_Result
    {

        private PalletType[] dynamicsTMSPalletTypesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PalletType", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPalletType", IsNullable = false)]
        public PalletType[] dynamicsTMSPalletTypes
        {
            get
            {
                return this.dynamicsTMSPalletTypesField;
            }
            set
            {
                this.dynamicsTMSPalletTypesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPalletType")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPalletType", IsNullable = false)]
    public partial class PalletType
    {

        private string palletType1Field;

        private string palletTypeDescriptionField;

        private decimal palletTypeWeightField;

        private decimal palletTypeWidthField;

        private decimal palletTypeDepthField;

        private decimal palletTypeVolumeField;

        private bool palletTypeContainerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PalletType")]
        public string PalletType1
        {
            get
            {
                return this.palletType1Field;
            }
            set
            {
                this.palletType1Field = value;
            }
        }

        /// <remarks/>
        public string PalletTypeDescription
        {
            get
            {
                return this.palletTypeDescriptionField;
            }
            set
            {
                this.palletTypeDescriptionField = value;
            }
        }

        /// <remarks/>
        public decimal PalletTypeWeight
        {
            get
            {
                return this.palletTypeWeightField;
            }
            set
            {
                this.palletTypeWeightField = value;
            }
        }

        /// <remarks/>
        public decimal PalletTypeWidth
        {
            get
            {
                return this.palletTypeWidthField;
            }
            set
            {
                this.palletTypeWidthField = value;
            }
        }

        /// <remarks/>
        public decimal PalletTypeDepth
        {
            get
            {
                return this.palletTypeDepthField;
            }
            set
            {
                this.palletTypeDepthField = value;
            }
        }

        /// <remarks/>
        public decimal PalletTypeVolume
        {
            get
            {
                return this.palletTypeVolumeField;
            }
            set
            {
                this.palletTypeVolumeField = value;
            }
        }

        /// <remarks/>
        public bool PalletTypeContainer
        {
            get
            {
                return this.palletTypeContainerField;
            }
            set
            {
                this.palletTypeContainerField = value;
            }
        }
    }


}
