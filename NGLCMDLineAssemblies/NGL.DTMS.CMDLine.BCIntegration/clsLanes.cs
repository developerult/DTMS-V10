using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NGL.DTMS.CMDLine.BCIntegration.Lane
{
    public class clsLanes
    {

        public static async Task<Envelope> Read(string bearerToken, oAuth2Settings sSettings, string sLegal, bool readAllHistory = true, bool markAsRead = true)
        {
            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":GetLanes");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetLanes xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\"><dynamicsTMSLanes /><readAllHistory> " + readAllHistory.ToString().ToLower() + "</readAllHistory><flagRead>" + markAsRead.ToString().ToLower() + "</flagRead></GetLanes></soap:Body></soap:Envelope>\r\n", null, "text/xml");
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


        public static bool Save(Envelope oLanes)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oLanes != null && oLanes.Body != null && oLanes.Body.GetLanes_Result != null && oLanes.Body.GetLanes_Result.dynamicsTMSLanes != null && oLanes.Body.GetLanes_Result.dynamicsTMSLanes.Length > 0)
            {
                foreach (Lane item in oLanes.Body.GetLanes_Result.dynamicsTMSLanes)
                {
                    Console.WriteLine("Name: " + item.LaneName.ToString());
                }
                blnRet = true;
                Console.WriteLine("Save Lane complete");
            }
            else
            {
                Console.WriteLine("Save Lane data failed, data not available");
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

        private GetLanes_Result getLanes_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public GetLanes_Result GetLanes_Result
        {
            get
            {
                return this.getLanes_ResultField;
            }
            set
            {
                this.getLanes_ResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    public partial class GetLanes_Result
    {

        private Lane[] dynamicsTMSLanesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Lane", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSLane", IsNullable = false)]
        public Lane[] dynamicsTMSLanes
        {
            get
            {
                return this.dynamicsTMSLanesField;
            }
            set
            {
                this.dynamicsTMSLanesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSLane")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSLane", IsNullable = false)]
    public partial class Lane
    {

        private byte controlIDField;

        private string laneNumberField;

        private string laneNameField;

        private string laneNumberMasterField;

        private string laneNameMasterField;

        private string laneCompNumberField;

        private bool laneDefaultCarrierUseField;

        private byte laneDefaultCarrierNumberField;

        private string laneOrigCompNumberField;

        private string laneOrigNameField;

        private string laneOrigAddress1Field;

        private string laneOrigAddress2Field;

        private string laneOrigAddress3Field;

        private string laneOrigCityField;

        private string laneOrigStateField;

        private string laneOrigCountryField;

        private string laneOrigZipField;

        private string laneOrigContactPhoneField;

        private string laneOrigContactPhoneExtField;

        private string laneOrigContactFaxField;

        private string laneDestCompNumberField;

        private string laneDestNameField;

        private string laneDestAddress1Field;

        private string laneDestAddress2Field;

        private string laneDestAddress3Field;

        private string laneDestCityField;

        private string laneDestStateField;

        private string laneDestCountryField;

        private string laneDestZipField;

        private string laneDestContactPhoneField;

        private string laneDestContactPhoneExtField;

        private string laneDestContactFaxField;

        private string laneConsigneeNumberField;

        private byte laneRecMinInField;

        private byte laneRecMinUnloadField;

        private byte laneRecMinOutField;

        private bool laneApptField;

        private bool lanePalletExchangeField;

        private string lanePalletTypeField;

        private byte laneBenchMilesField;

        private decimal laneBFCField;

        private string laneBFCTypeField;

        private string laneRecHourStartField;

        private string laneRecHourStopField;

        private string laneDestHourStopField;

        private string laneCommentsField;

        private string laneCommentsConfidentialField;

        private decimal laneLatitudeField;

        private decimal laneLongitudeField;

        private byte laneTempTypeField;

        private string lanePrimairyBuyerField;

        private bool laneAptDeliveryField;

        private string brokerNumberField;

        private string brokerNameField;

        private bool laneOriginAddressUseField;

        private string laneCarrierEquipmentCodesField;

        private string laneChepGLIDField;

        private string laneCarrierTypeCodeField;

        private bool lanePickupMonField;

        private bool lanePickupTueField;

        private bool lanePickupWedField;

        private bool lanePickupThuField;

        private bool lanePickupFriField;

        private bool lanePickupSatField;

        private bool lanePickupSunField;

        private bool laneDropOffMonField;

        private bool laneDropOffTueField;

        private bool laneDropOffWedField;

        private bool laneDropOffThuField;

        private bool laneDropOffFriField;

        private bool laneDropOffSatField;

        private bool laneDropOffSunField;

        private byte landDefaultRouteSequenceField;

        private string laneRouteGuideNumberField;

        private bool laneIsCrossDockFacilityField;

        private string laneCompAlphaCodeField;

        private string laneOrigCompAlphaCodeField;

        private string laneDestCompAlphaCodeField;

        private string laneLegalEntityField;

        private byte laneTransTypeField;

        private string laneOrigContactEmailField;

        private string laneDestContactEmailField;

        private string laneUser1Field;

        private string laneUser2Field;

        private string laneUser3Field;

        private string laneUser4Field;

        /// <remarks/>
        public byte ControlID
        {
            get
            {
                return this.controlIDField;
            }
            set
            {
                this.controlIDField = value;
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
        public string LaneName
        {
            get
            {
                return this.laneNameField;
            }
            set
            {
                this.laneNameField = value;
            }
        }

        /// <remarks/>
        public string LaneNumberMaster
        {
            get
            {
                return this.laneNumberMasterField;
            }
            set
            {
                this.laneNumberMasterField = value;
            }
        }

        /// <remarks/>
        public string LaneNameMaster
        {
            get
            {
                return this.laneNameMasterField;
            }
            set
            {
                this.laneNameMasterField = value;
            }
        }

        /// <remarks/>
        public string LaneCompNumber
        {
            get
            {
                return this.laneCompNumberField;
            }
            set
            {
                this.laneCompNumberField = value;
            }
        }

        /// <remarks/>
        public bool LaneDefaultCarrierUse
        {
            get
            {
                return this.laneDefaultCarrierUseField;
            }
            set
            {
                this.laneDefaultCarrierUseField = value;
            }
        }

        /// <remarks/>
        public byte LaneDefaultCarrierNumber
        {
            get
            {
                return this.laneDefaultCarrierNumberField;
            }
            set
            {
                this.laneDefaultCarrierNumberField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigCompNumber
        {
            get
            {
                return this.laneOrigCompNumberField;
            }
            set
            {
                this.laneOrigCompNumberField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigName
        {
            get
            {
                return this.laneOrigNameField;
            }
            set
            {
                this.laneOrigNameField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigAddress1
        {
            get
            {
                return this.laneOrigAddress1Field;
            }
            set
            {
                this.laneOrigAddress1Field = value;
            }
        }

        /// <remarks/>
        public string LaneOrigAddress2
        {
            get
            {
                return this.laneOrigAddress2Field;
            }
            set
            {
                this.laneOrigAddress2Field = value;
            }
        }

        /// <remarks/>
        public string LaneOrigAddress3
        {
            get
            {
                return this.laneOrigAddress3Field;
            }
            set
            {
                this.laneOrigAddress3Field = value;
            }
        }

        /// <remarks/>
        public string LaneOrigCity
        {
            get
            {
                return this.laneOrigCityField;
            }
            set
            {
                this.laneOrigCityField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigState
        {
            get
            {
                return this.laneOrigStateField;
            }
            set
            {
                this.laneOrigStateField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigCountry
        {
            get
            {
                return this.laneOrigCountryField;
            }
            set
            {
                this.laneOrigCountryField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigZip
        {
            get
            {
                return this.laneOrigZipField;
            }
            set
            {
                this.laneOrigZipField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigContactPhone
        {
            get
            {
                return this.laneOrigContactPhoneField;
            }
            set
            {
                this.laneOrigContactPhoneField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigContactPhoneExt
        {
            get
            {
                return this.laneOrigContactPhoneExtField;
            }
            set
            {
                this.laneOrigContactPhoneExtField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigContactFax
        {
            get
            {
                return this.laneOrigContactFaxField;
            }
            set
            {
                this.laneOrigContactFaxField = value;
            }
        }

        /// <remarks/>
        public string LaneDestCompNumber
        {
            get
            {
                return this.laneDestCompNumberField;
            }
            set
            {
                this.laneDestCompNumberField = value;
            }
        }

        /// <remarks/>
        public string LaneDestName
        {
            get
            {
                return this.laneDestNameField;
            }
            set
            {
                this.laneDestNameField = value;
            }
        }

        /// <remarks/>
        public string LaneDestAddress1
        {
            get
            {
                return this.laneDestAddress1Field;
            }
            set
            {
                this.laneDestAddress1Field = value;
            }
        }

        /// <remarks/>
        public string LaneDestAddress2
        {
            get
            {
                return this.laneDestAddress2Field;
            }
            set
            {
                this.laneDestAddress2Field = value;
            }
        }

        /// <remarks/>
        public string LaneDestAddress3
        {
            get
            {
                return this.laneDestAddress3Field;
            }
            set
            {
                this.laneDestAddress3Field = value;
            }
        }

        /// <remarks/>
        public string LaneDestCity
        {
            get
            {
                return this.laneDestCityField;
            }
            set
            {
                this.laneDestCityField = value;
            }
        }

        /// <remarks/>
        public string LaneDestState
        {
            get
            {
                return this.laneDestStateField;
            }
            set
            {
                this.laneDestStateField = value;
            }
        }

        /// <remarks/>
        public string LaneDestCountry
        {
            get
            {
                return this.laneDestCountryField;
            }
            set
            {
                this.laneDestCountryField = value;
            }
        }

        /// <remarks/>
        public string LaneDestZip
        {
            get
            {
                return this.laneDestZipField;
            }
            set
            {
                this.laneDestZipField = value;
            }
        }

        /// <remarks/>
        public string LaneDestContactPhone
        {
            get
            {
                return this.laneDestContactPhoneField;
            }
            set
            {
                this.laneDestContactPhoneField = value;
            }
        }

        /// <remarks/>
        public string LaneDestContactPhoneExt
        {
            get
            {
                return this.laneDestContactPhoneExtField;
            }
            set
            {
                this.laneDestContactPhoneExtField = value;
            }
        }

        /// <remarks/>
        public string LaneDestContactFax
        {
            get
            {
                return this.laneDestContactFaxField;
            }
            set
            {
                this.laneDestContactFaxField = value;
            }
        }

        /// <remarks/>
        public string LaneConsigneeNumber
        {
            get
            {
                return this.laneConsigneeNumberField;
            }
            set
            {
                this.laneConsigneeNumberField = value;
            }
        }

        /// <remarks/>
        public byte LaneRecMinIn
        {
            get
            {
                return this.laneRecMinInField;
            }
            set
            {
                this.laneRecMinInField = value;
            }
        }

        /// <remarks/>
        public byte LaneRecMinUnload
        {
            get
            {
                return this.laneRecMinUnloadField;
            }
            set
            {
                this.laneRecMinUnloadField = value;
            }
        }

        /// <remarks/>
        public byte LaneRecMinOut
        {
            get
            {
                return this.laneRecMinOutField;
            }
            set
            {
                this.laneRecMinOutField = value;
            }
        }

        /// <remarks/>
        public bool LaneAppt
        {
            get
            {
                return this.laneApptField;
            }
            set
            {
                this.laneApptField = value;
            }
        }

        /// <remarks/>
        public bool LanePalletExchange
        {
            get
            {
                return this.lanePalletExchangeField;
            }
            set
            {
                this.lanePalletExchangeField = value;
            }
        }

        /// <remarks/>
        public string LanePalletType
        {
            get
            {
                return this.lanePalletTypeField;
            }
            set
            {
                this.lanePalletTypeField = value;
            }
        }

        /// <remarks/>
        public byte LaneBenchMiles
        {
            get
            {
                return this.laneBenchMilesField;
            }
            set
            {
                this.laneBenchMilesField = value;
            }
        }

        /// <remarks/>
        public decimal LaneBFC
        {
            get
            {
                return this.laneBFCField;
            }
            set
            {
                this.laneBFCField = value;
            }
        }

        /// <remarks/>
        public string LaneBFCType
        {
            get
            {
                return this.laneBFCTypeField;
            }
            set
            {
                this.laneBFCTypeField = value;
            }
        }

        /// <remarks/>
        public string LaneRecHourStart
        {
            get
            {
                return this.laneRecHourStartField;
            }
            set
            {
                this.laneRecHourStartField = value;
            }
        }

        /// <remarks/>
        public string LaneRecHourStop
        {
            get
            {
                return this.laneRecHourStopField;
            }
            set
            {
                this.laneRecHourStopField = value;
            }
        }

        /// <remarks/>
        public string LaneDestHourStop
        {
            get
            {
                return this.laneDestHourStopField;
            }
            set
            {
                this.laneDestHourStopField = value;
            }
        }

        /// <remarks/>
        public string LaneComments
        {
            get
            {
                return this.laneCommentsField;
            }
            set
            {
                this.laneCommentsField = value;
            }
        }

        /// <remarks/>
        public string LaneCommentsConfidential
        {
            get
            {
                return this.laneCommentsConfidentialField;
            }
            set
            {
                this.laneCommentsConfidentialField = value;
            }
        }

        /// <remarks/>
        public decimal LaneLatitude
        {
            get
            {
                return this.laneLatitudeField;
            }
            set
            {
                this.laneLatitudeField = value;
            }
        }

        /// <remarks/>
        public decimal LaneLongitude
        {
            get
            {
                return this.laneLongitudeField;
            }
            set
            {
                this.laneLongitudeField = value;
            }
        }

        /// <remarks/>
        public byte LaneTempType
        {
            get
            {
                return this.laneTempTypeField;
            }
            set
            {
                this.laneTempTypeField = value;
            }
        }

        /// <remarks/>
        public string LanePrimairyBuyer
        {
            get
            {
                return this.lanePrimairyBuyerField;
            }
            set
            {
                this.lanePrimairyBuyerField = value;
            }
        }

        /// <remarks/>
        public bool LaneAptDelivery
        {
            get
            {
                return this.laneAptDeliveryField;
            }
            set
            {
                this.laneAptDeliveryField = value;
            }
        }

        /// <remarks/>
        public string BrokerNumber
        {
            get
            {
                return this.brokerNumberField;
            }
            set
            {
                this.brokerNumberField = value;
            }
        }

        /// <remarks/>
        public string BrokerName
        {
            get
            {
                return this.brokerNameField;
            }
            set
            {
                this.brokerNameField = value;
            }
        }

        /// <remarks/>
        public bool LaneOriginAddressUse
        {
            get
            {
                return this.laneOriginAddressUseField;
            }
            set
            {
                this.laneOriginAddressUseField = value;
            }
        }

        /// <remarks/>
        public string LaneCarrierEquipmentCodes
        {
            get
            {
                return this.laneCarrierEquipmentCodesField;
            }
            set
            {
                this.laneCarrierEquipmentCodesField = value;
            }
        }

        /// <remarks/>
        public string LaneChepGLID
        {
            get
            {
                return this.laneChepGLIDField;
            }
            set
            {
                this.laneChepGLIDField = value;
            }
        }

        /// <remarks/>
        public string LaneCarrierTypeCode
        {
            get
            {
                return this.laneCarrierTypeCodeField;
            }
            set
            {
                this.laneCarrierTypeCodeField = value;
            }
        }

        /// <remarks/>
        public bool LanePickupMon
        {
            get
            {
                return this.lanePickupMonField;
            }
            set
            {
                this.lanePickupMonField = value;
            }
        }

        /// <remarks/>
        public bool LanePickupTue
        {
            get
            {
                return this.lanePickupTueField;
            }
            set
            {
                this.lanePickupTueField = value;
            }
        }

        /// <remarks/>
        public bool LanePickupWed
        {
            get
            {
                return this.lanePickupWedField;
            }
            set
            {
                this.lanePickupWedField = value;
            }
        }

        /// <remarks/>
        public bool LanePickupThu
        {
            get
            {
                return this.lanePickupThuField;
            }
            set
            {
                this.lanePickupThuField = value;
            }
        }

        /// <remarks/>
        public bool LanePickupFri
        {
            get
            {
                return this.lanePickupFriField;
            }
            set
            {
                this.lanePickupFriField = value;
            }
        }

        /// <remarks/>
        public bool LanePickupSat
        {
            get
            {
                return this.lanePickupSatField;
            }
            set
            {
                this.lanePickupSatField = value;
            }
        }

        /// <remarks/>
        public bool LanePickupSun
        {
            get
            {
                return this.lanePickupSunField;
            }
            set
            {
                this.lanePickupSunField = value;
            }
        }

        /// <remarks/>
        public bool LaneDropOffMon
        {
            get
            {
                return this.laneDropOffMonField;
            }
            set
            {
                this.laneDropOffMonField = value;
            }
        }

        /// <remarks/>
        public bool LaneDropOffTue
        {
            get
            {
                return this.laneDropOffTueField;
            }
            set
            {
                this.laneDropOffTueField = value;
            }
        }

        /// <remarks/>
        public bool LaneDropOffWed
        {
            get
            {
                return this.laneDropOffWedField;
            }
            set
            {
                this.laneDropOffWedField = value;
            }
        }

        /// <remarks/>
        public bool LaneDropOffThu
        {
            get
            {
                return this.laneDropOffThuField;
            }
            set
            {
                this.laneDropOffThuField = value;
            }
        }

        /// <remarks/>
        public bool LaneDropOffFri
        {
            get
            {
                return this.laneDropOffFriField;
            }
            set
            {
                this.laneDropOffFriField = value;
            }
        }

        /// <remarks/>
        public bool LaneDropOffSat
        {
            get
            {
                return this.laneDropOffSatField;
            }
            set
            {
                this.laneDropOffSatField = value;
            }
        }

        /// <remarks/>
        public bool LaneDropOffSun
        {
            get
            {
                return this.laneDropOffSunField;
            }
            set
            {
                this.laneDropOffSunField = value;
            }
        }

        /// <remarks/>
        public byte LandDefaultRouteSequence
        {
            get
            {
                return this.landDefaultRouteSequenceField;
            }
            set
            {
                this.landDefaultRouteSequenceField = value;
            }
        }

        /// <remarks/>
        public string LaneRouteGuideNumber
        {
            get
            {
                return this.laneRouteGuideNumberField;
            }
            set
            {
                this.laneRouteGuideNumberField = value;
            }
        }

        /// <remarks/>
        public bool LaneIsCrossDockFacility
        {
            get
            {
                return this.laneIsCrossDockFacilityField;
            }
            set
            {
                this.laneIsCrossDockFacilityField = value;
            }
        }

        /// <remarks/>
        public string LaneCompAlphaCode
        {
            get
            {
                return this.laneCompAlphaCodeField;
            }
            set
            {
                this.laneCompAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigCompAlphaCode
        {
            get
            {
                return this.laneOrigCompAlphaCodeField;
            }
            set
            {
                this.laneOrigCompAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public string LaneDestCompAlphaCode
        {
            get
            {
                return this.laneDestCompAlphaCodeField;
            }
            set
            {
                this.laneDestCompAlphaCodeField = value;
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
        public byte LaneTransType
        {
            get
            {
                return this.laneTransTypeField;
            }
            set
            {
                this.laneTransTypeField = value;
            }
        }

        /// <remarks/>
        public string LaneOrigContactEmail
        {
            get
            {
                return this.laneOrigContactEmailField;
            }
            set
            {
                this.laneOrigContactEmailField = value;
            }
        }

        /// <remarks/>
        public string LaneDestContactEmail
        {
            get
            {
                return this.laneDestContactEmailField;
            }
            set
            {
                this.laneDestContactEmailField = value;
            }
        }

        /// <remarks/>
        public string LaneUser1
        {
            get
            {
                return this.laneUser1Field;
            }
            set
            {
                this.laneUser1Field = value;
            }
        }

        /// <remarks/>
        public string LaneUser2
        {
            get
            {
                return this.laneUser2Field;
            }
            set
            {
                this.laneUser2Field = value;
            }
        }

        /// <remarks/>
        public string LaneUser3
        {
            get
            {
                return this.laneUser3Field;
            }
            set
            {
                this.laneUser3Field = value;
            }
        }

        /// <remarks/>
        public string LaneUser4
        {
            get
            {
                return this.laneUser4Field;
            }
            set
            {
                this.laneUser4Field = value;
            }
        }
    }


}
