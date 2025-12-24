using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;

namespace NGL.DTMS.CMDLine.BCIntegration.AP
{
    public class clsAPExports
    {
        public static SendAP generateSampleAPData(int[] iAPControls)
        {
            SendAP oSendAP = new SendAP();
            List<AP> lAPs = new List<AP>();
            foreach (int iControl in iAPControls)
            {
                AP oAP = new AP() { 
                    APControl = iControl,
                    CarrierNumber = 0, // CarrierNumber >
                    BookFinAPBillNumber = "Test", // BookFinAPBillNumber >
                    BookFinAPBillInvDate = "11/07/2023", // BookFinAPBillInvDate >
                    BookCarrOrderNumber = "Test", // BookCarrOrderNumber >
                    LaneNumber = "Test", // LaneNumber >
                    BookItemCostCenterNumber = "Test", // BookItemCostCenterNumber >
                    BookFinAPActCost = 0, // BookFinAPActCost >
                    BookCarrBLNumber = "Test", // BookCarrBLNumber >
                    BookFinAPActWgt = 0, // BookFinAPActWgt >
                    BookFinAPBillNoDate = "11/07/2023", // BookFinAPBillNoDate >
                    BookFinAPActTax = 0, // BookFinAPActTax >
                    BookProNumber = "Test", // BookProNumber >
                    BookFinAPExportRetry = 0, // BookFinAPExportRetry >
                    BookFinAPExportDate = "11/07/2023", // BookFinAPExportDate >
                    PrevSentDate = "11/07/2023", // PrevSentDate >
                    CompanyNumber = "Test", // CompanyNumber >
                    BookOrderSequence = 0, // BookOrderSequence >
                    CarrierEquipmentCodes = "Test", // CarrierEquipmentCodes >
                    BookCarrierTypeCode = "Test", // BookCarrierTypeCode >
                    APFee1 = 0, // APFee1 >
                    APFee2 = 0, // APFee2 >
                    APFee3 = 0, // APFee3 >
                    APFee4 = 0, // APFee4 >
                    APFee5 = 0, // APFee5 >
                    APFee6 = 0, // APFee6 >
                    OtherCosts = 0, // OtherCosts >
                    BookWarehouseNumber = "Test", // BookWarehouseNumber >
                    BookMilesFrom = 0, // BookMilesFrom >
                    CompNatNumber = 0, // CompNatNumber >
                    BookReasonCode = "Test", // BookReasonCode >
                    BookTransType = 0, // BookTransType >
                    BookShipCarrierProNumber = "Test", // BookShipCarrierProNumber >
                    BookShipCarrierNumber = "Test", // BookShipCarrierNumber >
                    APTaxDetail1 = 0, // APTaxDetail1 >
                    APTaxDetail2 = 0, // APTaxDetail2 >
                    APTaxDetail3 = 0, // APTaxDetail3 >
                    APTaxDetail4 = 0, // APTaxDetail4 >
                    APTaxDetail5 = 0, // APTaxDetail5 >
                    CompLegalEntity = "Test", // CompLegalEntity >
                    CompAlphaCode = "Test", // CompAlphaCode >
                    BookSHID = "Test", // BookSHID >
                    CarrierAlphaCode = "Test", // CarrierAlphaCode >
                    CarrierLegalEntity = "Test", // CarrierLegalEntity >
                    LaneLegalEntity = "Test", // LaneLegalEntity >
                    BookConsPrefix = "Test", // BookConsPrefix >
                    BookRouteConsFlag = false, // BookRouteConsFlag >
                    BookShipCarrierName = "Test", // BookShipCarrierName >
                    BookFinAPStdCost = 0, // BookFinAPStdCost >
                    BookFinAPTotalTaxableFees = 0, // BookFinAPTotalTaxableFees >
                    BookFinAPTotalTaxes = 0, // BookFinAPTotalTaxes >
                    BookFinAPNonTaxableFees = 0, // BookFinAPNonTaxableFees >
                    BookWhseAuthorizationNo = "Test", // BookWhseAuthorizationNo >
                    BookFinAPGLNumber = "Test" // BookFinAPGLNumber >
                };
                List<APDetails> lDetails = new List<APDetails>();
                lDetails.Add(new APDetails()
                {
                    OrderNumber = "Test", // OrderNumber >
                    ItemNumber = "abc-00", // ItemNumber >
                    QtyOrdered = 0, // QtyOrdered >
                    FreightCost = 0, // FreightCost >
                    ItemCost = 0, // ItemCost >
                    Weight = 0, // Weight >
                    Cube = 0, // Cube >
                    Pack = 0, // Pack >
                    Size = "Test", // Size >
                    Description = "Test", // Description >
                    CustomerItemNumber = "Test", // CustomerItemNumber >
                    CustomerNumber = "Test", // CustomerNumber >
                    OrderSequence = 0, // OrderSequence >
                    Hazmat = "N", // Hazmat >
                    Brand = "Test", // Brand >
                    CostCenter = "Test", // CostCenter >
                    LotNumber = "Test", // LotNumber >
                    LotExpirationDate = "11/07/2023", // LotExpirationDate >
                    GTIN = "Test", // GTIN >
                    CountryOfOrigin = "Test", // CountryOfOrigin >
                    CustomerPONumber = "Test", // CustomerPONumber >
                    BFC = 0, // BFC >
                    HST = "Test", // HST >
                    BookProNumber = "Test", // BookProNumber >
                    PalletType = "Test", // PalletType >
                    APControl = iControl, //APControl >
                    CompNatNumber = 0, // CompNatNumber >
                    CompLegalEntity = "Test", // CompLegalEntity >
                    CompAlphaCode = "Test", // CompAlphaCode >
                    BookItemDiscount = 0, // BookItemDiscount >
                    BookItemLineHaul = 0, // BookItemLineHaul >
                    BookItemTaxableFees = 0, // BookItemTaxableFees >
                    BookItemTaxes = 0, // BookItemTaxes >
                    BookItemNonTaxableFees = 0, // BookItemNonTaxableFees >
                    BookItemWeightBreak = 0, // BookItemWeightBreak >
                    BookItemRated49CFRCode = "Test", // BookItemRated49CFRCode >
                    BookItemRatedIATACode = "Test", // BookItemRatedIATACode >
                    BookItemRatedDOTCode = "Test", // BookItemRatedDOTCode >
                    BookItemRatedMArineCode = "Test", // BookItemRatedMArineCode >
                    BookItemRatedNMFCClass = "Test", // BookItemRatedNMFCClass >
                    BookItemRatedNMFCSubClass = "Test", // BookItemRatedNMFCSubClass >
                    BookItemRatedFAKClass = "Test", // BookItemRatedFAKClass >
                    HazmatTypeCode = "Test", // HazmatTypeCode >
                    Hazmat49CFRCode = "Test", // Hazmat49CFRCode >
                    IATACode = "Test", // IATACode >
                    DOTCode = "Test", // DOTCode >
                    MarineCode = "Test", // MarineCode >
                    NMFCClass = "Test", // NMFCClass >
                    FAKClass = "Test", // FAKClass >
                    LimitedQtyFlag = true, // LimitedQtyFlag >
                    Pallets = 0, // Pallets >
                    Ties = 0, // Ties >
                    Highs = 0, // Highs >
                    QtyPalletPercentage = 0, // QtyPalletPercentage >
                    QtyLength = 0, // QtyLength >
                    QtyWidth = 0, // QtyWidth >
                    QtyHeight = 0, // QtyHeight >
                    Stackable = false, // Stackable >
                    LevelOfDensity = 0, // LevelOfDensity >
                    BookItemOrderNumber = "Test" // BookItemOrderNumber >

                });
                lDetails.Add(new APDetails()
                {
                    OrderNumber = "Test", // OrderNumber >
                    ItemNumber = "abc-01", // ItemNumber >
                    QtyOrdered = 0, // QtyOrdered >
                    FreightCost = 0, // FreightCost >
                    ItemCost = 0, // ItemCost >
                    Weight = 0, // Weight >
                    Cube = 0, // Cube >
                    Pack = 0, // Pack >
                    Size = "Test", // Size >
                    Description = "Test", // Description >
                    CustomerItemNumber = "Test", // CustomerItemNumber >
                    CustomerNumber = "Test", // CustomerNumber >
                    OrderSequence = 0, // OrderSequence >
                    Hazmat = "N", // Hazmat >
                    Brand = "Test", // Brand >
                    CostCenter = "Test", // CostCenter >
                    LotNumber = "Test", // LotNumber >
                    LotExpirationDate = "11/07/2023", // LotExpirationDate >
                    GTIN = "Test", // GTIN >
                    CountryOfOrigin = "Test", // CountryOfOrigin >
                    CustomerPONumber = "Test", // CustomerPONumber >
                    BFC = 0, // BFC >
                    HST = "Test", // HST >
                    BookProNumber = "Test", // BookProNumber >
                    PalletType = "Test", // PalletType >
                    APControl = iControl, //APControl >
                    CompNatNumber = 0, // CompNatNumber >
                    CompLegalEntity = "Test", // CompLegalEntity >
                    CompAlphaCode = "Test", // CompAlphaCode >
                    BookItemDiscount = 0, // BookItemDiscount >
                    BookItemLineHaul = 0, // BookItemLineHaul >
                    BookItemTaxableFees = 0, // BookItemTaxableFees >
                    BookItemTaxes = 0, // BookItemTaxes >
                    BookItemNonTaxableFees = 0, // BookItemNonTaxableFees >
                    BookItemWeightBreak = 0, // BookItemWeightBreak >
                    BookItemRated49CFRCode = "Test", // BookItemRated49CFRCode >
                    BookItemRatedIATACode = "Test", // BookItemRatedIATACode >
                    BookItemRatedDOTCode = "Test", // BookItemRatedDOTCode >
                    BookItemRatedMArineCode = "Test", // BookItemRatedMArineCode >
                    BookItemRatedNMFCClass = "Test", // BookItemRatedNMFCClass >
                    BookItemRatedNMFCSubClass = "Test", // BookItemRatedNMFCSubClass >
                    BookItemRatedFAKClass = "Test", // BookItemRatedFAKClass >
                    HazmatTypeCode = "Test", // HazmatTypeCode >
                    Hazmat49CFRCode = "Test", // Hazmat49CFRCode >
                    IATACode = "Test", // IATACode >
                    DOTCode = "Test", // DOTCode >
                    MarineCode = "Test", // MarineCode >
                    NMFCClass = "Test", // NMFCClass >
                    FAKClass = "Test", // FAKClass >
                    LimitedQtyFlag = true, // LimitedQtyFlag >
                    Pallets = 0, // Pallets >
                    Ties = 0, // Ties >
                    Highs = 0, // Highs >
                    QtyPalletPercentage = 0, // QtyPalletPercentage >
                    QtyLength = 0, // QtyLength >
                    QtyWidth = 0, // QtyWidth >
                    QtyHeight = 0, // QtyHeight >
                    Stackable = false, // Stackable >
                    LevelOfDensity = 0, // LevelOfDensity >
                    BookItemOrderNumber = "Test" // BookItemOrderNumber >

                });
                oAP.Details = lDetails.ToArray();
                lAPs.Add(oAP);
            }
            oSendAP.dynamicsTMSAPs = lAPs.ToArray();
            return oSendAP;
        }

        public static string GetSOAPBody(SendAP oSendAP )
        {
            string sSOAP = "";
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UnicodeEncoding(bigEndian: false, byteOrderMark: false);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(oSendAP.GetType());
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, settings);
            serializer.Serialize(writer, oSendAP);
            sSOAP = Encoding.Unicode.GetString(ms.ToArray());
           
            return sSOAP;

        }

        public static async Task<Envelope> Read(string bearerToken, oAuth2Settings sSettings, string sLegal, string sSOAPBody)
        {

            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":SendAP");
                //request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSIsImtpZCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSJ9.eyJhdWQiOiJodHRwczovL2FwaS5idXNpbmVzc2NlbnRyYWwuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMjUxOGJlN2UtYzkzMy00OTA1LWFmNjQtMjRhZDAxNTcyMDJmLyIsImlhdCI6MTY5ODg1NjY2MSwibmJmIjoxNjk4ODU2NjYxLCJleHAiOjE2OTg4NjA1NjEsImFpbyI6IkUyRmdZTmd6NWVJRFM5WEVTRzhKd1pLYXNySkNBQT09IiwiYXBwaWQiOiI4ZmMyYWJiZS05ZTliLTQ0OTYtOGU2Ny1iZDA4ZDYyZGFkNDciLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yNTE4YmU3ZS1jOTMzLTQ5MDUtYWY2NC0yNGFkMDE1NzIwMmYvIiwiaWR0eXAiOiJhcHAiLCJvaWQiOiI4NmVmMzA1My04OTdkLTRhYzctODljNC1mODkwZWM5ZGJjNWYiLCJyaCI6IjAuQVc0QWZyNFlKVFBKQlVtdlpDU3RBVmNnTHozdmJabHNzMU5CaGdlbV9Ud0J1Sjl1QUFBLiIsInJvbGVzIjpbIkF1dG9tYXRpb24uUmVhZFdyaXRlLkFsbCIsImFwcF9hY2Nlc3MiLCJBUEkuUmVhZFdyaXRlLkFsbCJdLCJzdWIiOiI4NmVmMzA1My04OTdkLTRhYzctODljNC1mODkwZWM5ZGJjNWYiLCJ0aWQiOiIyNTE4YmU3ZS1jOTMzLTQ5MDUtYWY2NC0yNGFkMDE1NzIwMmYiLCJ1dGkiOiJYZGRYSFkySEMweWNROV9JdWZjMEFBIiwidmVyIjoiMS4wIn0.atbwWAfYnYEhSt_cyP7Jv5q7tsljv5JQIAoDzV17BAxBqYEykuJYsEEKK_Is1vkztnLSuADoOaEUiChtWGRS4PwAN_AHGieBH47V50yMS1tC2PbYI0CMCIqsgw-0yp5sAQBFHH_SXhuR9Kg7ya2ERmD35Hy_1uKv6wXFIBM27R3LC74bSx_GmcZu-sjfib-uoLyGArPPswa17oBQmnJcv22j3I8UhCH8WJev0fDn9ujFBqqOImz9YnrSKt8fT6uL_59Omej36bqPPwZqxZPvFX4n1u4CJgNTc0mVWVjHqBCw6z0GS8lOIjTrLTQ2skuSZ_b-yoNFZ1c4-x9GTOwTVw");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body>" + sSOAPBody + "</soap:Body></soap:Envelope>\r\n", null, "text/xml");
                request.Content = content;
                var response = await client.SendAsync(request);
                //removed by RHR so we can capture the fault response //response.EnsureSuccessStatusCode();
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

        public static bool Save(Envelope oAPResult)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oAPResult != null && oAPResult.Body != null)
            {
                if (oAPResult.Body.Fault != null && oAPResult.Body.Fault.detail != null)
                {
                    Console.WriteLine("Update AP Data Failed Message: " + oAPResult.Body.Fault.detail.@string);
                    blnRet = false;
                }
                else
                {

                    blnRet = true;
                    Console.WriteLine("Update AP Data Complete");
                }
            }
            else
            {
                Console.WriteLine("Update AP Data failed, data not available");
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

        private object sendAP_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public object SendAP_Result
        {
            get
            {
                return this.sendAP_ResultField;
            }
            set
            {
                this.sendAP_ResultField = value;
            }
        }

        private EnvelopeBodyFault faultField;

        /// <remarks/>
        public EnvelopeBodyFault Fault
        {
            get
            {
                return this.faultField;
            }
            set
            {
                this.faultField = value;
            }
        }
    }



    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeBodyFault
    {

        private string faultcodeField;

        private faultstring faultstringField;

        private detail detailField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public string faultcode
        {
            get
            {
                return this.faultcodeField;
            }
            set
            {
                this.faultcodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public faultstring faultstring
        {
            get
            {
                return this.faultstringField;
            }
            set
            {
                this.faultstringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public detail detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class faultstring
    {

        private string langField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class detail
    {

        private string stringField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/")]
        public string @string
        {
            get
            {
                return this.stringField;
            }
            set
            {
                this.stringField = value;
            }
        }
    }




    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    public partial class SendAP
    {

        private AP[] dynamicsTMSAPsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AP", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSAP", IsNullable = false)]
        public AP[] dynamicsTMSAPs
        {
            get
            {
                return this.dynamicsTMSAPsField;
            }
            set
            {
                this.dynamicsTMSAPsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSAP")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSAP", IsNullable = false)]
    public partial class AP
    {

        private long aPControlField;

        private int carrierNumberField;

        private string bookFinAPBillNumberField;

        private string bookFinAPBillInvDateField;

        private string bookCarrOrderNumberField;

        private string laneNumberField;

        private string bookItemCostCenterNumberField;

        private decimal bookFinAPActCostField;

        private string bookCarrBLNumberField;

        private int bookFinAPActWgtField;

        private string bookFinAPBillNoDateField;

        private decimal bookFinAPActTaxField;

        private string bookProNumberField;

        private int bookFinAPExportRetryField;

        private string bookFinAPExportDateField;

        private string prevSentDateField;

        private string companyNumberField;

        private int bookOrderSequenceField;

        private string carrierEquipmentCodesField;

        private string bookCarrierTypeCodeField;

        private decimal aPFee1Field;

        private decimal aPFee2Field;

        private decimal aPFee3Field;

        private decimal aPFee4Field;

        private decimal aPFee5Field;

        private decimal aPFee6Field;

        private decimal otherCostsField;

        private string bookWarehouseNumberField;

        private decimal bookMilesFromField;

        private int compNatNumberField;

        private string bookReasonCodeField;

        private int bookTransTypeField;

        private string bookShipCarrierProNumberField;

        private string bookShipCarrierNumberField;

        private decimal aPTaxDetail1Field;

        private decimal aPTaxDetail2Field;

        private decimal aPTaxDetail3Field;

        private decimal aPTaxDetail4Field;

        private decimal aPTaxDetail5Field;

        private string compLegalEntityField;

        private string compAlphaCodeField;

        private string bookSHIDField;

        private string carrierAlphaCodeField;

        private string carrierLegalEntityField;

        private string laneLegalEntityField;

        private string bookConsPrefixField;

        private bool bookRouteConsFlagField;

        private string bookShipCarrierNameField;

        private decimal bookFinAPStdCostField;

        private decimal bookFinAPTotalTaxableFeesField;

        private decimal bookFinAPTotalTaxesField;

        private decimal bookFinAPNonTaxableFeesField;

        private string bookWhseAuthorizationNoField;

        private string bookFinAPGLNumberField;

        private APDetails[] detailsField;

        /// <remarks/>
        public long APControl
        {
            get
            {
                return this.aPControlField;
            }
            set
            {
                this.aPControlField = value;
            }
        }

        /// <remarks/>
        public int CarrierNumber
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
        public string LaneNumber
        {
            get
            {
                return this.laneNumberField;
            }
            set
            {
                this.laneNumberField = value;
            }
        }

        /// <remarks/>
        public string BookItemCostCenterNumber
        {
            get
            {
                return this.bookItemCostCenterNumberField;
            }
            set
            {
                this.bookItemCostCenterNumberField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinAPActCost
        {
            get
            {
                return this.bookFinAPActCostField;
            }
            set
            {
                this.bookFinAPActCostField = value;
            }
        }

        /// <remarks/>
        public string BookCarrBLNumber
        {
            get
            {
                return this.bookCarrBLNumberField;
            }
            set
            {
                this.bookCarrBLNumberField = value;
            }
        }

        /// <remarks/>
        public int BookFinAPActWgt
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
        public string BookFinAPBillNoDate
        {
            get
            {
                return this.bookFinAPBillNoDateField;
            }
            set
            {
                this.bookFinAPBillNoDateField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinAPActTax
        {
            get
            {
                return this.bookFinAPActTaxField;
            }
            set
            {
                this.bookFinAPActTaxField = value;
            }
        }

        /// <remarks/>
        public string BookProNumber
        {
            get
            {
                return this.bookProNumberField;
            }
            set
            {
                this.bookProNumberField = value;
            }
        }

        /// <remarks/>
        public int BookFinAPExportRetry
        {
            get
            {
                return this.bookFinAPExportRetryField;
            }
            set
            {
                this.bookFinAPExportRetryField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPExportDate
        {
            get
            {
                return this.bookFinAPExportDateField;
            }
            set
            {
                this.bookFinAPExportDateField = value;
            }
        }

        /// <remarks/>
        public string PrevSentDate
        {
            get
            {
                return this.prevSentDateField;
            }
            set
            {
                this.prevSentDateField = value;
            }
        }

        /// <remarks/>
        public string CompanyNumber
        {
            get
            {
                return this.companyNumberField;
            }
            set
            {
                this.companyNumberField = value;
            }
        }

        /// <remarks/>
        public int BookOrderSequence
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
        public string CarrierEquipmentCodes
        {
            get
            {
                return this.carrierEquipmentCodesField;
            }
            set
            {
                this.carrierEquipmentCodesField = value;
            }
        }

        /// <remarks/>
        public string BookCarrierTypeCode
        {
            get
            {
                return this.bookCarrierTypeCodeField;
            }
            set
            {
                this.bookCarrierTypeCodeField = value;
            }
        }

        /// <remarks/>
        public decimal APFee1
        {
            get
            {
                return this.aPFee1Field;
            }
            set
            {
                this.aPFee1Field = value;
            }
        }

        /// <remarks/>
        public decimal APFee2
        {
            get
            {
                return this.aPFee2Field;
            }
            set
            {
                this.aPFee2Field = value;
            }
        }

        /// <remarks/>
        public decimal APFee3
        {
            get
            {
                return this.aPFee3Field;
            }
            set
            {
                this.aPFee3Field = value;
            }
        }

        /// <remarks/>
        public decimal APFee4
        {
            get
            {
                return this.aPFee4Field;
            }
            set
            {
                this.aPFee4Field = value;
            }
        }

        /// <remarks/>
        public decimal APFee5
        {
            get
            {
                return this.aPFee5Field;
            }
            set
            {
                this.aPFee5Field = value;
            }
        }

        /// <remarks/>
        public decimal APFee6
        {
            get
            {
                return this.aPFee6Field;
            }
            set
            {
                this.aPFee6Field = value;
            }
        }

        /// <remarks/>
        public decimal OtherCosts
        {
            get
            {
                return this.otherCostsField;
            }
            set
            {
                this.otherCostsField = value;
            }
        }

        /// <remarks/>
        public string BookWarehouseNumber
        {
            get
            {
                return this.bookWarehouseNumberField;
            }
            set
            {
                this.bookWarehouseNumberField = value;
            }
        }

        /// <remarks/>
        public decimal BookMilesFrom
        {
            get
            {
                return this.bookMilesFromField;
            }
            set
            {
                this.bookMilesFromField = value;
            }
        }

        /// <remarks/>
        public int CompNatNumber
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
        public string BookReasonCode
        {
            get
            {
                return this.bookReasonCodeField;
            }
            set
            {
                this.bookReasonCodeField = value;
            }
        }

        /// <remarks/>
        public int BookTransType
        {
            get
            {
                return this.bookTransTypeField;
            }
            set
            {
                this.bookTransTypeField = value;
            }
        }

        /// <remarks/>
        public string BookShipCarrierProNumber
        {
            get
            {
                return this.bookShipCarrierProNumberField;
            }
            set
            {
                this.bookShipCarrierProNumberField = value;
            }
        }

        /// <remarks/>
        public string BookShipCarrierNumber
        {
            get
            {
                return this.bookShipCarrierNumberField;
            }
            set
            {
                this.bookShipCarrierNumberField = value;
            }
        }

        /// <remarks/>
        public decimal APTaxDetail1
        {
            get
            {
                return this.aPTaxDetail1Field;
            }
            set
            {
                this.aPTaxDetail1Field = value;
            }
        }

        /// <remarks/>
        public decimal APTaxDetail2
        {
            get
            {
                return this.aPTaxDetail2Field;
            }
            set
            {
                this.aPTaxDetail2Field = value;
            }
        }

        /// <remarks/>
        public decimal APTaxDetail3
        {
            get
            {
                return this.aPTaxDetail3Field;
            }
            set
            {
                this.aPTaxDetail3Field = value;
            }
        }

        /// <remarks/>
        public decimal APTaxDetail4
        {
            get
            {
                return this.aPTaxDetail4Field;
            }
            set
            {
                this.aPTaxDetail4Field = value;
            }
        }

        /// <remarks/>
        public decimal APTaxDetail5
        {
            get
            {
                return this.aPTaxDetail5Field;
            }
            set
            {
                this.aPTaxDetail5Field = value;
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
        public string BookSHID
        {
            get
            {
                return this.bookSHIDField;
            }
            set
            {
                this.bookSHIDField = value;
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
        public string LaneLegalEntity
        {
            get
            {
                return this.laneLegalEntityField;
            }
            set
            {
                this.laneLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string BookConsPrefix
        {
            get
            {
                return this.bookConsPrefixField;
            }
            set
            {
                this.bookConsPrefixField = value;
            }
        }

        /// <remarks/>
        public bool BookRouteConsFlag
        {
            get
            {
                return this.bookRouteConsFlagField;
            }
            set
            {
                this.bookRouteConsFlagField = value;
            }
        }

        /// <remarks/>
        public string BookShipCarrierName
        {
            get
            {
                return this.bookShipCarrierNameField;
            }
            set
            {
                this.bookShipCarrierNameField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinAPStdCost
        {
            get
            {
                return this.bookFinAPStdCostField;
            }
            set
            {
                this.bookFinAPStdCostField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinAPTotalTaxableFees
        {
            get
            {
                return this.bookFinAPTotalTaxableFeesField;
            }
            set
            {
                this.bookFinAPTotalTaxableFeesField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinAPTotalTaxes
        {
            get
            {
                return this.bookFinAPTotalTaxesField;
            }
            set
            {
                this.bookFinAPTotalTaxesField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinAPNonTaxableFees
        {
            get
            {
                return this.bookFinAPNonTaxableFeesField;
            }
            set
            {
                this.bookFinAPNonTaxableFeesField = value;
            }
        }

        /// <remarks/>
        public string BookWhseAuthorizationNo
        {
            get
            {
                return this.bookWhseAuthorizationNoField;
            }
            set
            {
                this.bookWhseAuthorizationNoField = value;
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
        /// <System.Xml.Serialization.XmlElementAttribute("Details")>  
        /// 
        [XmlElement]
        public APDetails[] Details
        {
            get
            {
                return this.detailsField;
            }
            set
            {
                this.detailsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSAP")]
    public partial class APDetails
    {

        private string orderNumberField;

        private string itemNumberField;

        private int qtyOrderedField;

        private decimal freightCostField;

        private decimal itemCostField;

        private decimal weightField;

        private int cubeField;

        private int packField;

        private string sizeField;

        private string descriptionField;

        private string customerItemNumberField;

        private string customerNumberField;

        private int orderSequenceField;

        private string hazmatField;

        private string brandField;

        private string costCenterField;

        private string lotNumberField;

        private string lotExpirationDateField;

        private string gTINField;

        private string countryOfOriginField;

        private string customerPONumberField;

        private decimal bFCField;

        private string hSTField;

        private string bookProNumberField;

        private string palletTypeField;

        private long aPControlField;

        private int compNatNumberField;

        private string compLegalEntityField;

        private string compAlphaCodeField;

        private decimal bookItemDiscountField;

        private decimal bookItemLineHaulField;

        private decimal bookItemTaxableFeesField;

        private decimal bookItemTaxesField;

        private decimal bookItemNonTaxableFeesField;

        private decimal bookItemWeightBreakField;

        private string bookItemRated49CFRCodeField;

        private string bookItemRatedIATACodeField;

        private string bookItemRatedDOTCodeField;

        private string bookItemRatedMArineCodeField;

        private string bookItemRatedNMFCClassField;

        private string bookItemRatedNMFCSubClassField;

        private string bookItemRatedFAKClassField;

        private string hazmatTypeCodeField;

        private string hazmat49CFRCodeField;

        private string iATACodeField;

        private string dOTCodeField;

        private string marineCodeField;

        private string nMFCClassField;

        private string fAKClassField;

        private bool limitedQtyFlagField;

        private decimal palletsField;

        private decimal tiesField;

        private decimal highsField;

        private decimal qtyPalletPercentageField;

        private decimal qtyLengthField;

        private decimal qtyWidthField;

        private decimal qtyHeightField;

        private bool stackableField;

        private int levelOfDensityField;

        private string bookItemOrderNumberField;

        /// <remarks/>
        public string OrderNumber
        {
            get
            {
                return this.orderNumberField;
            }
            set
            {
                this.orderNumberField = value;
            }
        }

        /// <remarks/>
        public string ItemNumber
        {
            get
            {
                return this.itemNumberField;
            }
            set
            {
                this.itemNumberField = value;
            }
        }

        /// <remarks/>
        public int QtyOrdered
        {
            get
            {
                return this.qtyOrderedField;
            }
            set
            {
                this.qtyOrderedField = value;
            }
        }

        /// <remarks/>
        public decimal FreightCost
        {
            get
            {
                return this.freightCostField;
            }
            set
            {
                this.freightCostField = value;
            }
        }

        /// <remarks/>
        public decimal ItemCost
        {
            get
            {
                return this.itemCostField;
            }
            set
            {
                this.itemCostField = value;
            }
        }

        /// <remarks/>
        public decimal Weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }

        /// <remarks/>
        public int Cube
        {
            get
            {
                return this.cubeField;
            }
            set
            {
                this.cubeField = value;
            }
        }

        /// <remarks/>
        public int Pack
        {
            get
            {
                return this.packField;
            }
            set
            {
                this.packField = value;
            }
        }

        /// <remarks/>
        public string Size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string CustomerItemNumber
        {
            get
            {
                return this.customerItemNumberField;
            }
            set
            {
                this.customerItemNumberField = value;
            }
        }

        /// <remarks/>
        public string CustomerNumber
        {
            get
            {
                return this.customerNumberField;
            }
            set
            {
                this.customerNumberField = value;
            }
        }

        /// <remarks/>
        public int OrderSequence
        {
            get
            {
                return this.orderSequenceField;
            }
            set
            {
                this.orderSequenceField = value;
            }
        }

        /// <remarks/>
        public string Hazmat
        {
            get
            {
                return this.hazmatField;
            }
            set
            {
                this.hazmatField = value;
            }
        }

        /// <remarks/>
        public string Brand
        {
            get
            {
                return this.brandField;
            }
            set
            {
                this.brandField = value;
            }
        }

        /// <remarks/>
        public string CostCenter
        {
            get
            {
                return this.costCenterField;
            }
            set
            {
                this.costCenterField = value;
            }
        }

        /// <remarks/>
        public string LotNumber
        {
            get
            {
                return this.lotNumberField;
            }
            set
            {
                this.lotNumberField = value;
            }
        }

        /// <remarks/>
        public string LotExpirationDate
        {
            get
            {
                return this.lotExpirationDateField;
            }
            set
            {
                this.lotExpirationDateField = value;
            }
        }

        /// <remarks/>
        public string GTIN
        {
            get
            {
                return this.gTINField;
            }
            set
            {
                this.gTINField = value;
            }
        }

        /// <remarks/>
        public string CountryOfOrigin
        {
            get
            {
                return this.countryOfOriginField;
            }
            set
            {
                this.countryOfOriginField = value;
            }
        }

        /// <remarks/>
        public string CustomerPONumber
        {
            get
            {
                return this.customerPONumberField;
            }
            set
            {
                this.customerPONumberField = value;
            }
        }

        /// <remarks/>
        public decimal BFC
        {
            get
            {
                return this.bFCField;
            }
            set
            {
                this.bFCField = value;
            }
        }

        /// <remarks/>
        public string HST
        {
            get
            {
                return this.hSTField;
            }
            set
            {
                this.hSTField = value;
            }
        }

        /// <remarks/>
        public string BookProNumber
        {
            get
            {
                return this.bookProNumberField;
            }
            set
            {
                this.bookProNumberField = value;
            }
        }

        /// <remarks/>
        public string PalletType
        {
            get
            {
                return this.palletTypeField;
            }
            set
            {
                this.palletTypeField = value;
            }
        }

        /// <remarks/>
        public long APControl
        {
            get
            {
                return this.aPControlField;
            }
            set
            {
                this.aPControlField = value;
            }
        }

        /// <remarks/>
        public int CompNatNumber
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
        public decimal BookItemDiscount
        {
            get
            {
                return this.bookItemDiscountField;
            }
            set
            {
                this.bookItemDiscountField = value;
            }
        }

        /// <remarks/>
        public decimal BookItemLineHaul
        {
            get
            {
                return this.bookItemLineHaulField;
            }
            set
            {
                this.bookItemLineHaulField = value;
            }
        }

        /// <remarks/>
        public decimal BookItemTaxableFees
        {
            get
            {
                return this.bookItemTaxableFeesField;
            }
            set
            {
                this.bookItemTaxableFeesField = value;
            }
        }

        /// <remarks/>
        public decimal BookItemTaxes
        {
            get
            {
                return this.bookItemTaxesField;
            }
            set
            {
                this.bookItemTaxesField = value;
            }
        }

        /// <remarks/>
        public decimal BookItemNonTaxableFees
        {
            get
            {
                return this.bookItemNonTaxableFeesField;
            }
            set
            {
                this.bookItemNonTaxableFeesField = value;
            }
        }

        /// <remarks/>
        public decimal BookItemWeightBreak
        {
            get
            {
                return this.bookItemWeightBreakField;
            }
            set
            {
                this.bookItemWeightBreakField = value;
            }
        }

        /// <remarks/>
        public string BookItemRated49CFRCode
        {
            get
            {
                return this.bookItemRated49CFRCodeField;
            }
            set
            {
                this.bookItemRated49CFRCodeField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedIATACode
        {
            get
            {
                return this.bookItemRatedIATACodeField;
            }
            set
            {
                this.bookItemRatedIATACodeField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedDOTCode
        {
            get
            {
                return this.bookItemRatedDOTCodeField;
            }
            set
            {
                this.bookItemRatedDOTCodeField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedMArineCode
        {
            get
            {
                return this.bookItemRatedMArineCodeField;
            }
            set
            {
                this.bookItemRatedMArineCodeField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedNMFCClass
        {
            get
            {
                return this.bookItemRatedNMFCClassField;
            }
            set
            {
                this.bookItemRatedNMFCClassField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedNMFCSubClass
        {
            get
            {
                return this.bookItemRatedNMFCSubClassField;
            }
            set
            {
                this.bookItemRatedNMFCSubClassField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedFAKClass
        {
            get
            {
                return this.bookItemRatedFAKClassField;
            }
            set
            {
                this.bookItemRatedFAKClassField = value;
            }
        }

        /// <remarks/>
        public string HazmatTypeCode
        {
            get
            {
                return this.hazmatTypeCodeField;
            }
            set
            {
                this.hazmatTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string Hazmat49CFRCode
        {
            get
            {
                return this.hazmat49CFRCodeField;
            }
            set
            {
                this.hazmat49CFRCodeField = value;
            }
        }

        /// <remarks/>
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        public string DOTCode
        {
            get
            {
                return this.dOTCodeField;
            }
            set
            {
                this.dOTCodeField = value;
            }
        }

        /// <remarks/>
        public string MarineCode
        {
            get
            {
                return this.marineCodeField;
            }
            set
            {
                this.marineCodeField = value;
            }
        }

        /// <remarks/>
        public string NMFCClass
        {
            get
            {
                return this.nMFCClassField;
            }
            set
            {
                this.nMFCClassField = value;
            }
        }

        /// <remarks/>
        public string FAKClass
        {
            get
            {
                return this.fAKClassField;
            }
            set
            {
                this.fAKClassField = value;
            }
        }

        /// <remarks/>
        public bool LimitedQtyFlag
        {
            get
            {
                return this.limitedQtyFlagField;
            }
            set
            {
                this.limitedQtyFlagField = value;
            }
        }

        /// <remarks/>
        public decimal Pallets
        {
            get
            {
                return this.palletsField;
            }
            set
            {
                this.palletsField = value;
            }
        }

        /// <remarks/>
        public decimal Ties
        {
            get
            {
                return this.tiesField;
            }
            set
            {
                this.tiesField = value;
            }
        }

        /// <remarks/>
        public decimal Highs
        {
            get
            {
                return this.highsField;
            }
            set
            {
                this.highsField = value;
            }
        }

        /// <remarks/>
        public decimal QtyPalletPercentage
        {
            get
            {
                return this.qtyPalletPercentageField;
            }
            set
            {
                this.qtyPalletPercentageField = value;
            }
        }

        /// <remarks/>
        public decimal QtyLength
        {
            get
            {
                return this.qtyLengthField;
            }
            set
            {
                this.qtyLengthField = value;
            }
        }

        /// <remarks/>
        public decimal QtyWidth
        {
            get
            {
                return this.qtyWidthField;
            }
            set
            {
                this.qtyWidthField = value;
            }
        }

        /// <remarks/>
        public decimal QtyHeight
        {
            get
            {
                return this.qtyHeightField;
            }
            set
            {
                this.qtyHeightField = value;
            }
        }

        /// <remarks/>
        public bool Stackable
        {
            get
            {
                return this.stackableField;
            }
            set
            {
                this.stackableField = value;
            }
        }

        /// <remarks/>
        public int LevelOfDensity
        {
            get
            {
                return this.levelOfDensityField;
            }
            set
            {
                this.levelOfDensityField = value;
            }
        }

        /// <remarks/>
        public string BookItemOrderNumber
        {
            get
            {
                return this.bookItemOrderNumberField;
            }
            set
            {
                this.bookItemOrderNumberField = value;
            }
        }
    }


}
