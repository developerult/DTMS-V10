using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NGL.DTMS.CMDLine.BCIntegration.Pay
{
    public class clsPayables
    {

        public static async Task<Envelope> Read(string bearerToken, oAuth2Settings sSettings, string sLegal, bool readAllHistory = true, bool markAsRead = true)
        {
            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":GetPayables");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetPayables xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\"><dynamicsTMSPayables /><readAllHistory> " + readAllHistory.ToString().ToLower() + "</readAllHistory><flagRead>" + markAsRead.ToString().ToLower() + "</flagRead></GetPayables></soap:Body></soap:Envelope>\r\n", null, "text/xml");
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


        public static bool Save(Envelope oPayables)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oPayables != null && oPayables.Body != null && oPayables.Body.GetPayables_Result != null && oPayables.Body.GetPayables_Result.dynamicsTMSPayables != null && oPayables.Body.GetPayables_Result.dynamicsTMSPayables.Length > 0)
            {
                foreach (Payment item in oPayables.Body.GetPayables_Result.dynamicsTMSPayables)
                {
                    Console.WriteLine("Name: " + item.BookCarrOrderNumber.ToString());
                }
                blnRet = true;
                Console.WriteLine("Save Payables complete");
            }
            else
            {
                Console.WriteLine("Save Payables data failed, data not available");
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

        private GetPayables_Result getPayables_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public GetPayables_Result GetPayables_Result
        {
            get
            {
                return this.getPayables_ResultField;
            }
            set
            {
                this.getPayables_ResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    public partial class GetPayables_Result
    {

        private Payment[] dynamicsTMSPayablesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Payment", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPayment", IsNullable = false)]
        public Payment[] dynamicsTMSPayables
        {
            get
            {
                return this.dynamicsTMSPayablesField;
            }
            set
            {
                this.dynamicsTMSPayablesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPayment")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPayment", IsNullable = false)]
    public partial class Payment
    {

        private string bookCarrOrderNumberField;

        private decimal bookFinAPPayAmtField;

        private decimal bookFinAPActWgtField;

        private string bookFinAPCheckField;

        private string bookFinAPPayDateField;

        private string bookFinAPBillNumberField;

        private string bookFinAPGLNumberField;

        private string aPGLDescriptionField;

        private string compNumberField;

        private string bookPRONumberField;

        private byte bookOrderSequenceField;

        private string compLegalEntityField;

        private string compAlphaCodeField;

        private string bookFinAPBillInvDateField;

        /// <remarks/>
        public string BookCarrOrderNumber
        {
            get
            {
                return this.bookCarrOrderNumberField;
            }
            set
            {
                this.bookCarrOrderNumberField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinAPPayAmt
        {
            get
            {
                return this.bookFinAPPayAmtField;
            }
            set
            {
                this.bookFinAPPayAmtField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinAPActWgt
        {
            get
            {
                return this.bookFinAPActWgtField;
            }
            set
            {
                this.bookFinAPActWgtField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPCheck
        {
            get
            {
                return this.bookFinAPCheckField;
            }
            set
            {
                this.bookFinAPCheckField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPPayDate
        {
            get
            {
                return this.bookFinAPPayDateField;
            }
            set
            {
                this.bookFinAPPayDateField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPBillNumber
        {
            get
            {
                return this.bookFinAPBillNumberField;
            }
            set
            {
                this.bookFinAPBillNumberField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPGLNumber
        {
            get
            {
                return this.bookFinAPGLNumberField;
            }
            set
            {
                this.bookFinAPGLNumberField = value;
            }
        }

        /// <remarks/>
        public string APGLDescription
        {
            get
            {
                return this.aPGLDescriptionField;
            }
            set
            {
                this.aPGLDescriptionField = value;
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
        public string BookPRONumber
        {
            get
            {
                return this.bookPRONumberField;
            }
            set
            {
                this.bookPRONumberField = value;
            }
        }

        /// <remarks/>
        public byte BookOrderSequence
        {
            get
            {
                return this.bookOrderSequenceField;
            }
            set
            {
                this.bookOrderSequenceField = value;
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
        public string BookFinAPBillInvDate
        {
            get
            {
                return this.bookFinAPBillInvDateField;
            }
            set
            {
                this.bookFinAPBillInvDateField = value;
            }
        }
    }


}
