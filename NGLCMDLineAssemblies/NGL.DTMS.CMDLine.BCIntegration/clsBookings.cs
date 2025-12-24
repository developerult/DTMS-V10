using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NGL.DTMS.CMDLine.BCIntegration.Book
{
    public class clsBookings
    {


        public static async Task<Envelope> Read(string bearerToken, oAuth2Settings sSettings, string sLegal, bool readAllHistory = true, bool markAsRead = true)
        {
            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":GetBookings");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetBookings xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\"><dynamicsTMSBookings /><readAllHistory> " + readAllHistory.ToString().ToLower() + "</readAllHistory><flagRead>" + markAsRead.ToString().ToLower() + "</flagRead></GetBookings></soap:Body></soap:Envelope>\r\n", null, "text/xml");
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

        public static bool Save(Envelope oBookings)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oBookings != null && oBookings.Body != null && oBookings.Body.GetBookings_Result != null && oBookings.Body.GetBookings_Result.dynamicsTMSBookings != null && oBookings.Body.GetBookings_Result.dynamicsTMSBookings.Length > 0)
            {
                foreach (Booking item in oBookings.Body.GetBookings_Result.dynamicsTMSBookings)
                {
                    Console.WriteLine("Name: " + item.PONumber.ToString());
                }
                blnRet = true;
                Console.WriteLine("Save Booking complete");
            }
            else
            {
                Console.WriteLine("Save Booking data failed, data not available");
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

        private GetBookings_Result getBookings_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public GetBookings_Result GetBookings_Result
        {
            get
            {
                return this.getBookings_ResultField;
            }
            set
            {
                this.getBookings_ResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    public partial class GetBookings_Result
    {

        private Booking[] dynamicsTMSBookingsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Booking", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSShipment", IsNullable = false)]
        public Booking[] dynamicsTMSBookings
        {
            get
            {
                return this.dynamicsTMSBookingsField;
            }
            set
            {
                this.dynamicsTMSBookingsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSShipment")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSShipment", IsNullable = false)]
    public partial class Booking
    {

        private byte typeField;

        private string noField;

        private byte changeNoField;

        private string pONumberField;

        private string pOVendorField;

        private string pOdateField;

        private string pOShipdateField;

        private string pOBuyerField;

        private byte pOFrtField;

        private decimal pOTotalFrtField;

        private decimal pOTotalCostField;

        private decimal pOWgtField;

        private byte pOCubeField;

        private byte pOQtyField;

        private byte pOPalletsField;

        private decimal pOLinesField;

        private bool pOConfirmField;

        private string pODefaultCustomerField;

        private string pODefaultCarrierField;

        private string pOReqDateField;

        private string pOShipInstructionsField;

        private bool pOCoolerField;

        private bool pOFrozenField;

        private bool pODryField;

        private string pOTempField;

        private string pOCarTypeField;

        private string pOShipViaField;

        private string pOShipViaTypeField;

        private string pOConsigneeNumberField;

        private string pOCustomerPOField;

        private decimal pOOtherCostsField;

        private byte pOStatusFlagField;

        private byte pOOrderSequenceField;

        private string pOChepGLIDField;

        private string pOCarrierEquipmentCodesField;

        private string pOCarrierTypeCodeField;

        private string pOPalletPositionsField;

        private string pOSchedulePUDateField;

        private string pOSchedulePUTimeField;

        private string pOScheduleDelDateField;

        private string pOScheduleDelTimeField;

        private string pOActPUDateField;

        private string pOActPUTimeField;

        private string pOActDelDateField;

        private string pOActDelTimeField;

        private string pOOrigCompNumberField;

        private string pOOrigNameField;

        private string pOOrigAddress1Field;

        private string pOOrigAddress2Field;

        private string pOOrigAddress3Field;

        private string pOOrigCityField;

        private string pOOrigStateField;

        private string pOOrigCountryField;

        private string pOOrigZipField;

        private string pOOrigContactPhoneField;

        private string pOOrigContactPhoneExtField;

        private string pOOrigContactFaxField;

        private string pOOrigContactNumberField;

        private string pODestNameField;

        private string pODestAddress1Field;

        private string pODestAddress2Field;

        private string pODestAddress3Field;

        private string pODestCityField;

        private string pODestStateField;

        private string pODestCountryField;

        private string pODestZipField;

        private string pODestContactPhoneField;

        private string pODestContactPhoneExtField;

        private string pODestContactFaxField;

        private bool pOPalletExchangeField;

        private string pOPalletTypeField;

        private string pOCommentsField;

        private string pOCommentsConfidentialField;

        private bool pOInboundField;

        private int pODefaultRouteSequenceField;

        private string pORouteGudieNumberField;

        private string pOCompAlphaCodeField;

        private string pOCompLegalEntityField;

        private string pOOrigContactEmailField;

        private string pODestContactEmailField;

        private string pOWhseAuthorizationNoField;

        private string pOWhseReleaseNoField;

        private string pOUser1Field;

        private string pOUser2Field;

        private string pOUser3Field;

        private string pOUser4Field;

        private BookingItems[] itemsField;

        /// <remarks/>
        public byte Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string No
        {
            get
            {
                return this.noField;
            }
            set
            {
                this.noField = value;
            }
        }

        /// <remarks/>
        public byte ChangeNo
        {
            get
            {
                return this.changeNoField;
            }
            set
            {
                this.changeNoField = value;
            }
        }

        /// <remarks/>
        public string PONumber
        {
            get
            {
                return this.pONumberField;
            }
            set
            {
                this.pONumberField = value;
            }
        }

        /// <remarks/>
        public string POVendor
        {
            get
            {
                return this.pOVendorField;
            }
            set
            {
                this.pOVendorField = value;
            }
        }

        /// <remarks/>
        public string POdate
        {
            get
            {
                return this.pOdateField;
            }
            set
            {
                this.pOdateField = value;
            }
        }

        /// <remarks/>
        public string POShipdate
        {
            get
            {
                return this.pOShipdateField;
            }
            set
            {
                this.pOShipdateField = value;
            }
        }

        /// <remarks/>
        public string POBuyer
        {
            get
            {
                return this.pOBuyerField;
            }
            set
            {
                this.pOBuyerField = value;
            }
        }

        /// <remarks/>
        public byte POFrt
        {
            get
            {
                return this.pOFrtField;
            }
            set
            {
                this.pOFrtField = value;
            }
        }

        /// <remarks/>
        public decimal POTotalFrt
        {
            get
            {
                return this.pOTotalFrtField;
            }
            set
            {
                this.pOTotalFrtField = value;
            }
        }

        /// <remarks/>
        public decimal POTotalCost
        {
            get
            {
                return this.pOTotalCostField;
            }
            set
            {
                this.pOTotalCostField = value;
            }
        }

        /// <remarks/>
        public decimal POWgt
        {
            get
            {
                return this.pOWgtField;
            }
            set
            {
                this.pOWgtField = value;
            }
        }

        /// <remarks/>
        public byte POCube
        {
            get
            {
                return this.pOCubeField;
            }
            set
            {
                this.pOCubeField = value;
            }
        }

        /// <remarks/>
        public byte POQty
        {
            get
            {
                return this.pOQtyField;
            }
            set
            {
                this.pOQtyField = value;
            }
        }

        /// <remarks/>
        public byte POPallets
        {
            get
            {
                return this.pOPalletsField;
            }
            set
            {
                this.pOPalletsField = value;
            }
        }

        /// <remarks/>
        public decimal POLines
        {
            get
            {
                return this.pOLinesField;
            }
            set
            {
                this.pOLinesField = value;
            }
        }

        /// <remarks/>
        public bool POConfirm
        {
            get
            {
                return this.pOConfirmField;
            }
            set
            {
                this.pOConfirmField = value;
            }
        }

        /// <remarks/>
        public string PODefaultCustomer
        {
            get
            {
                return this.pODefaultCustomerField;
            }
            set
            {
                this.pODefaultCustomerField = value;
            }
        }

        /// <remarks/>
        public string PODefaultCarrier
        {
            get
            {
                return this.pODefaultCarrierField;
            }
            set
            {
                this.pODefaultCarrierField = value;
            }
        }

        /// <remarks/>
        public string POReqDate
        {
            get
            {
                return this.pOReqDateField;
            }
            set
            {
                this.pOReqDateField = value;
            }
        }

        /// <remarks/>
        public string POShipInstructions
        {
            get
            {
                return this.pOShipInstructionsField;
            }
            set
            {
                this.pOShipInstructionsField = value;
            }
        }

        /// <remarks/>
        public bool POCooler
        {
            get
            {
                return this.pOCoolerField;
            }
            set
            {
                this.pOCoolerField = value;
            }
        }

        /// <remarks/>
        public bool POFrozen
        {
            get
            {
                return this.pOFrozenField;
            }
            set
            {
                this.pOFrozenField = value;
            }
        }

        /// <remarks/>
        public bool PODry
        {
            get
            {
                return this.pODryField;
            }
            set
            {
                this.pODryField = value;
            }
        }

        /// <remarks/>
        public string POTemp
        {
            get
            {
                return this.pOTempField;
            }
            set
            {
                this.pOTempField = value;
            }
        }

        /// <remarks/>
        public string POCarType
        {
            get
            {
                return this.pOCarTypeField;
            }
            set
            {
                this.pOCarTypeField = value;
            }
        }

        /// <remarks/>
        public string POShipVia
        {
            get
            {
                return this.pOShipViaField;
            }
            set
            {
                this.pOShipViaField = value;
            }
        }

        /// <remarks/>
        public string POShipViaType
        {
            get
            {
                return this.pOShipViaTypeField;
            }
            set
            {
                this.pOShipViaTypeField = value;
            }
        }

        /// <remarks/>
        public string POConsigneeNumber
        {
            get
            {
                return this.pOConsigneeNumberField;
            }
            set
            {
                this.pOConsigneeNumberField = value;
            }
        }

        /// <remarks/>
        public string POCustomerPO
        {
            get
            {
                return this.pOCustomerPOField;
            }
            set
            {
                this.pOCustomerPOField = value;
            }
        }

        /// <remarks/>
        public decimal POOtherCosts
        {
            get
            {
                return this.pOOtherCostsField;
            }
            set
            {
                this.pOOtherCostsField = value;
            }
        }

        /// <remarks/>
        public byte POStatusFlag
        {
            get
            {
                return this.pOStatusFlagField;
            }
            set
            {
                this.pOStatusFlagField = value;
            }
        }

        /// <remarks/>
        public byte POOrderSequence
        {
            get
            {
                return this.pOOrderSequenceField;
            }
            set
            {
                this.pOOrderSequenceField = value;
            }
        }

        /// <remarks/>
        public string POChepGLID
        {
            get
            {
                return this.pOChepGLIDField;
            }
            set
            {
                this.pOChepGLIDField = value;
            }
        }

        /// <remarks/>
        public string POCarrierEquipmentCodes
        {
            get
            {
                return this.pOCarrierEquipmentCodesField;
            }
            set
            {
                this.pOCarrierEquipmentCodesField = value;
            }
        }

        /// <remarks/>
        public string POCarrierTypeCode
        {
            get
            {
                return this.pOCarrierTypeCodeField;
            }
            set
            {
                this.pOCarrierTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string POPalletPositions
        {
            get
            {
                return this.pOPalletPositionsField;
            }
            set
            {
                this.pOPalletPositionsField = value;
            }
        }

        /// <remarks/>
        public string POSchedulePUDate
        {
            get
            {
                return this.pOSchedulePUDateField;
            }
            set
            {
                this.pOSchedulePUDateField = value;
            }
        }

        /// <remarks/>
        public string POSchedulePUTime
        {
            get
            {
                return this.pOSchedulePUTimeField;
            }
            set
            {
                this.pOSchedulePUTimeField = value;
            }
        }

        /// <remarks/>
        public string POScheduleDelDate
        {
            get
            {
                return this.pOScheduleDelDateField;
            }
            set
            {
                this.pOScheduleDelDateField = value;
            }
        }

        /// <remarks/>
        public string POScheduleDelTime
        {
            get
            {
                return this.pOScheduleDelTimeField;
            }
            set
            {
                this.pOScheduleDelTimeField = value;
            }
        }

        /// <remarks/>
        public string POActPUDate
        {
            get
            {
                return this.pOActPUDateField;
            }
            set
            {
                this.pOActPUDateField = value;
            }
        }

        /// <remarks/>
        public string POActPUTime
        {
            get
            {
                return this.pOActPUTimeField;
            }
            set
            {
                this.pOActPUTimeField = value;
            }
        }

        /// <remarks/>
        public string POActDelDate
        {
            get
            {
                return this.pOActDelDateField;
            }
            set
            {
                this.pOActDelDateField = value;
            }
        }

        /// <remarks/>
        public string POActDelTime
        {
            get
            {
                return this.pOActDelTimeField;
            }
            set
            {
                this.pOActDelTimeField = value;
            }
        }

        /// <remarks/>
        public string POOrigCompNumber
        {
            get
            {
                return this.pOOrigCompNumberField;
            }
            set
            {
                this.pOOrigCompNumberField = value;
            }
        }

        /// <remarks/>
        public string POOrigName
        {
            get
            {
                return this.pOOrigNameField;
            }
            set
            {
                this.pOOrigNameField = value;
            }
        }

        /// <remarks/>
        public string POOrigAddress1
        {
            get
            {
                return this.pOOrigAddress1Field;
            }
            set
            {
                this.pOOrigAddress1Field = value;
            }
        }

        /// <remarks/>
        public string POOrigAddress2
        {
            get
            {
                return this.pOOrigAddress2Field;
            }
            set
            {
                this.pOOrigAddress2Field = value;
            }
        }

        /// <remarks/>
        public string POOrigAddress3
        {
            get
            {
                return this.pOOrigAddress3Field;
            }
            set
            {
                this.pOOrigAddress3Field = value;
            }
        }

        /// <remarks/>
        public string POOrigCity
        {
            get
            {
                return this.pOOrigCityField;
            }
            set
            {
                this.pOOrigCityField = value;
            }
        }

        /// <remarks/>
        public string POOrigState
        {
            get
            {
                return this.pOOrigStateField;
            }
            set
            {
                this.pOOrigStateField = value;
            }
        }

        /// <remarks/>
        public string POOrigCountry
        {
            get
            {
                return this.pOOrigCountryField;
            }
            set
            {
                this.pOOrigCountryField = value;
            }
        }

        /// <remarks/>
        public string POOrigZip
        {
            get
            {
                return this.pOOrigZipField;
            }
            set
            {
                this.pOOrigZipField = value;
            }
        }

        /// <remarks/>
        public string POOrigContactPhone
        {
            get
            {
                return this.pOOrigContactPhoneField;
            }
            set
            {
                this.pOOrigContactPhoneField = value;
            }
        }

        /// <remarks/>
        public string POOrigContactPhoneExt
        {
            get
            {
                return this.pOOrigContactPhoneExtField;
            }
            set
            {
                this.pOOrigContactPhoneExtField = value;
            }
        }

        /// <remarks/>
        public string POOrigContactFax
        {
            get
            {
                return this.pOOrigContactFaxField;
            }
            set
            {
                this.pOOrigContactFaxField = value;
            }
        }

        /// <remarks/>
        public string POOrigContactNumber
        {
            get
            {
                return this.pOOrigContactNumberField;
            }
            set
            {
                this.pOOrigContactNumberField = value;
            }
        }

        /// <remarks/>
        public string PODestName
        {
            get
            {
                return this.pODestNameField;
            }
            set
            {
                this.pODestNameField = value;
            }
        }

        /// <remarks/>
        public string PODestAddress1
        {
            get
            {
                return this.pODestAddress1Field;
            }
            set
            {
                this.pODestAddress1Field = value;
            }
        }

        /// <remarks/>
        public string PODestAddress2
        {
            get
            {
                return this.pODestAddress2Field;
            }
            set
            {
                this.pODestAddress2Field = value;
            }
        }

        /// <remarks/>
        public string PODestAddress3
        {
            get
            {
                return this.pODestAddress3Field;
            }
            set
            {
                this.pODestAddress3Field = value;
            }
        }

        /// <remarks/>
        public string PODestCity
        {
            get
            {
                return this.pODestCityField;
            }
            set
            {
                this.pODestCityField = value;
            }
        }

        /// <remarks/>
        public string PODestState
        {
            get
            {
                return this.pODestStateField;
            }
            set
            {
                this.pODestStateField = value;
            }
        }

        /// <remarks/>
        public string PODestCountry
        {
            get
            {
                return this.pODestCountryField;
            }
            set
            {
                this.pODestCountryField = value;
            }
        }

        /// <remarks/>
        public string PODestZip
        {
            get
            {
                return this.pODestZipField;
            }
            set
            {
                this.pODestZipField = value;
            }
        }

        /// <remarks/>
        public string PODestContactPhone
        {
            get
            {
                return this.pODestContactPhoneField;
            }
            set
            {
                this.pODestContactPhoneField = value;
            }
        }

        /// <remarks/>
        public string PODestContactPhoneExt
        {
            get
            {
                return this.pODestContactPhoneExtField;
            }
            set
            {
                this.pODestContactPhoneExtField = value;
            }
        }

        /// <remarks/>
        public string PODestContactFax
        {
            get
            {
                return this.pODestContactFaxField;
            }
            set
            {
                this.pODestContactFaxField = value;
            }
        }

        /// <remarks/>
        public bool POPalletExchange
        {
            get
            {
                return this.pOPalletExchangeField;
            }
            set
            {
                this.pOPalletExchangeField = value;
            }
        }

        /// <remarks/>
        public string POPalletType
        {
            get
            {
                return this.pOPalletTypeField;
            }
            set
            {
                this.pOPalletTypeField = value;
            }
        }

        /// <remarks/>
        public string POComments
        {
            get
            {
                return this.pOCommentsField;
            }
            set
            {
                this.pOCommentsField = value;
            }
        }

        /// <remarks/>
        public string POCommentsConfidential
        {
            get
            {
                return this.pOCommentsConfidentialField;
            }
            set
            {
                this.pOCommentsConfidentialField = value;
            }
        }

        /// <remarks/>
        public bool POInbound
        {
            get
            {
                return this.pOInboundField;
            }
            set
            {
                this.pOInboundField = value;
            }
        }

        /// <remarks/>
        public int PODefaultRouteSequence
        {
            get
            {
                return this.pODefaultRouteSequenceField;
            }
            set
            {
                this.pODefaultRouteSequenceField = value;
            }
        }

        /// <remarks/>
        public string PORouteGudieNumber
        {
            get
            {
                return this.pORouteGudieNumberField;
            }
            set
            {
                this.pORouteGudieNumberField = value;
            }
        }

        /// <remarks/>
        public string POCompAlphaCode
        {
            get
            {
                return this.pOCompAlphaCodeField;
            }
            set
            {
                this.pOCompAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public string POCompLegalEntity
        {
            get
            {
                return this.pOCompLegalEntityField;
            }
            set
            {
                this.pOCompLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string POOrigContactEmail
        {
            get
            {
                return this.pOOrigContactEmailField;
            }
            set
            {
                this.pOOrigContactEmailField = value;
            }
        }

        /// <remarks/>
        public string PODestContactEmail
        {
            get
            {
                return this.pODestContactEmailField;
            }
            set
            {
                this.pODestContactEmailField = value;
            }
        }

        /// <remarks/>
        public string POWhseAuthorizationNo
        {
            get
            {
                return this.pOWhseAuthorizationNoField;
            }
            set
            {
                this.pOWhseAuthorizationNoField = value;
            }
        }

        /// <remarks/>
        public string POWhseReleaseNo
        {
            get
            {
                return this.pOWhseReleaseNoField;
            }
            set
            {
                this.pOWhseReleaseNoField = value;
            }
        }

        /// <remarks/>
        public string POUser1
        {
            get
            {
                return this.pOUser1Field;
            }
            set
            {
                this.pOUser1Field = value;
            }
        }

        /// <remarks/>
        public string POUser2
        {
            get
            {
                return this.pOUser2Field;
            }
            set
            {
                this.pOUser2Field = value;
            }
        }

        /// <remarks/>
        public string POUser3
        {
            get
            {
                return this.pOUser3Field;
            }
            set
            {
                this.pOUser3Field = value;
            }
        }

        /// <remarks/>
        public string POUser4
        {
            get
            {
                return this.pOUser4Field;
            }
            set
            {
                this.pOUser4Field = value;
            }
        }

        /// <remarks/>
        /// 
        [XmlElement]
        public BookingItems[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSShipment")]
    public partial class BookingItems
    {

        private object itemPONumberField;

        private decimal fixOffInvAllowField;

        private decimal fixFrtAllowField;

        private string itemNumberField;

        private byte qtyOrderedField;

        private decimal freightCostField;

        private decimal itemCostField;

        private decimal weightField;

        private byte cubeField;

        private byte packField;

        private string sizeField;

        private string descriptionField;

        private byte hazmatField;

        private string brandField;

        private string costCenterField;

        private string lotNumberField;

        private string lotExpirationDateField;

        private string gTINField;

        private string custItemNumberField;

        private string customerNumberField;

        private byte pOOrderSequenceField;

        private string palletTypeField;

        private string pOItemHazmatTypeCodeField;

        private string pOItem49CFRCodeField;

        private string pOItemIATACodeField;

        private string pOItemDOTCodeField;

        private string pOItemMarineCodeField;

        private string pOItemNMFCCClassField;

        private string pOItemFAKClassField;

        private bool pOItemLimitedQtyFlagField;

        private decimal pOItemPalletsField;

        private decimal pOItemTiesField;

        private decimal pOItemHighsField;

        private decimal pOItemQtyPalletPercentageField;

        private decimal pOItemQtyLengthField;

        private decimal pOItemQtyWidthField;

        private decimal pOItemQtyHeightField;

        private bool pOItemStackableField;

        private byte pOItemLevelOfDensityField;

        private string pOItemCompAlphaCodeField;

        private string pOItemCompLegalEntityField;

        private string pOItemNMFCSubClassField;

        private string pOItemWeightUnitOfMeasureField;

        private string pOItemCubeUnitOfMeasureField;

        private string pOItemDimensionUnitOfMeasureField;

        private string pOItemUser1Field;

        private string pOItemUser2Field;

        private string pOItemUser3Field;

        private string pOItemUser4Field;

        private string pOItemOrderNumberField;

        /// <remarks/>
        public object ItemPONumber
        {
            get
            {
                return this.itemPONumberField;
            }
            set
            {
                this.itemPONumberField = value;
            }
        }

        /// <remarks/>
        public decimal FixOffInvAllow
        {
            get
            {
                return this.fixOffInvAllowField;
            }
            set
            {
                this.fixOffInvAllowField = value;
            }
        }

        /// <remarks/>
        public decimal FixFrtAllow
        {
            get
            {
                return this.fixFrtAllowField;
            }
            set
            {
                this.fixFrtAllowField = value;
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
        public byte QtyOrdered
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
        public byte Cube
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
        public byte Pack
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
        public byte Hazmat
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
        public string CustItemNumber
        {
            get
            {
                return this.custItemNumberField;
            }
            set
            {
                this.custItemNumberField = value;
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
        public byte POOrderSequence
        {
            get
            {
                return this.pOOrderSequenceField;
            }
            set
            {
                this.pOOrderSequenceField = value;
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
        public string POItemHazmatTypeCode
        {
            get
            {
                return this.pOItemHazmatTypeCodeField;
            }
            set
            {
                this.pOItemHazmatTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string POItem49CFRCode
        {
            get
            {
                return this.pOItem49CFRCodeField;
            }
            set
            {
                this.pOItem49CFRCodeField = value;
            }
        }

        /// <remarks/>
        public string POItemIATACode
        {
            get
            {
                return this.pOItemIATACodeField;
            }
            set
            {
                this.pOItemIATACodeField = value;
            }
        }

        /// <remarks/>
        public string POItemDOTCode
        {
            get
            {
                return this.pOItemDOTCodeField;
            }
            set
            {
                this.pOItemDOTCodeField = value;
            }
        }

        /// <remarks/>
        public string POItemMarineCode
        {
            get
            {
                return this.pOItemMarineCodeField;
            }
            set
            {
                this.pOItemMarineCodeField = value;
            }
        }

        /// <remarks/>
        public string POItemNMFCCClass
        {
            get
            {
                return this.pOItemNMFCCClassField;
            }
            set
            {
                this.pOItemNMFCCClassField = value;
            }
        }

        /// <remarks/>
        public string POItemFAKClass
        {
            get
            {
                return this.pOItemFAKClassField;
            }
            set
            {
                this.pOItemFAKClassField = value;
            }
        }

        /// <remarks/>
        public bool POItemLimitedQtyFlag
        {
            get
            {
                return this.pOItemLimitedQtyFlagField;
            }
            set
            {
                this.pOItemLimitedQtyFlagField = value;
            }
        }

        /// <remarks/>
        public decimal POItemPallets
        {
            get
            {
                return this.pOItemPalletsField;
            }
            set
            {
                this.pOItemPalletsField = value;
            }
        }

        /// <remarks/>
        public decimal POItemTies
        {
            get
            {
                return this.pOItemTiesField;
            }
            set
            {
                this.pOItemTiesField = value;
            }
        }

        /// <remarks/>
        public decimal POItemHighs
        {
            get
            {
                return this.pOItemHighsField;
            }
            set
            {
                this.pOItemHighsField = value;
            }
        }

        /// <remarks/>
        public decimal POItemQtyPalletPercentage
        {
            get
            {
                return this.pOItemQtyPalletPercentageField;
            }
            set
            {
                this.pOItemQtyPalletPercentageField = value;
            }
        }

        /// <remarks/>
        public decimal POItemQtyLength
        {
            get
            {
                return this.pOItemQtyLengthField;
            }
            set
            {
                this.pOItemQtyLengthField = value;
            }
        }

        /// <remarks/>
        public decimal POItemQtyWidth
        {
            get
            {
                return this.pOItemQtyWidthField;
            }
            set
            {
                this.pOItemQtyWidthField = value;
            }
        }

        /// <remarks/>
        public decimal POItemQtyHeight
        {
            get
            {
                return this.pOItemQtyHeightField;
            }
            set
            {
                this.pOItemQtyHeightField = value;
            }
        }

        /// <remarks/>
        public bool POItemStackable
        {
            get
            {
                return this.pOItemStackableField;
            }
            set
            {
                this.pOItemStackableField = value;
            }
        }

        /// <remarks/>
        public byte POItemLevelOfDensity
        {
            get
            {
                return this.pOItemLevelOfDensityField;
            }
            set
            {
                this.pOItemLevelOfDensityField = value;
            }
        }

        /// <remarks/>
        public string POItemCompAlphaCode
        {
            get
            {
                return this.pOItemCompAlphaCodeField;
            }
            set
            {
                this.pOItemCompAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public string POItemCompLegalEntity
        {
            get
            {
                return this.pOItemCompLegalEntityField;
            }
            set
            {
                this.pOItemCompLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string POItemNMFCSubClass
        {
            get
            {
                return this.pOItemNMFCSubClassField;
            }
            set
            {
                this.pOItemNMFCSubClassField = value;
            }
        }

        /// <remarks/>
        public string POItemWeightUnitOfMeasure
        {
            get
            {
                return this.pOItemWeightUnitOfMeasureField;
            }
            set
            {
                this.pOItemWeightUnitOfMeasureField = value;
            }
        }

        /// <remarks/>
        public string POItemCubeUnitOfMeasure
        {
            get
            {
                return this.pOItemCubeUnitOfMeasureField;
            }
            set
            {
                this.pOItemCubeUnitOfMeasureField = value;
            }
        }

        /// <remarks/>
        public string POItemDimensionUnitOfMeasure
        {
            get
            {
                return this.pOItemDimensionUnitOfMeasureField;
            }
            set
            {
                this.pOItemDimensionUnitOfMeasureField = value;
            }
        }

        /// <remarks/>
        public string POItemUser1
        {
            get
            {
                return this.pOItemUser1Field;
            }
            set
            {
                this.pOItemUser1Field = value;
            }
        }

        /// <remarks/>
        public string POItemUser2
        {
            get
            {
                return this.pOItemUser2Field;
            }
            set
            {
                this.pOItemUser2Field = value;
            }
        }

        /// <remarks/>
        public string POItemUser3
        {
            get
            {
                return this.pOItemUser3Field;
            }
            set
            {
                this.pOItemUser3Field = value;
            }
        }

        /// <remarks/>
        public string POItemUser4
        {
            get
            {
                return this.pOItemUser4Field;
            }
            set
            {
                this.pOItemUser4Field = value;
            }
        }

        /// <remarks/>
        public string POItemOrderNumber
        {
            get
            {
                return this.pOItemOrderNumberField;
            }
            set
            {
                this.pOItemOrderNumberField = value;
            }
        }
    }


}
