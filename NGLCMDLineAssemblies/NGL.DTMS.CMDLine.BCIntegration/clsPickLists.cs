using NGL.DTMS.CMDLine.BCIntegration.AP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
namespace NGL.DTMS.CMDLine.BCIntegration.Pick
{
    public class clsPickLists
    {
        public static SendPicks generateSamplePickData(int[] iPLControls)
        {
            SendPicks oSendPicks = new SendPicks();
            List<Pick> lPicks = new List<Pick>();
            foreach (int iControl in iPLControls)
            {
                Pick oPick = new Pick()
                {
                    PLControl = iControl,
                   BookCarrOrderNumber= "Test", // BookCarrOrderNumber >
                   BookConsPrefix= "Test", // BookConsPrefix >
                   CarrierNumber = 0, // CarrierNumber >
                   BookRevTotalCost = 0, // BookRevTotalCost >
                   LoadOrder = 0, // LoadOrder >
                   BookDateLoad = "11/07/2023" , // BookDateLoad >
                   BookDateRequired = "11/07/2023" , // BookDateRequired >
                   BookLoadCom = "D" , // BookLoadCom >
                   BookProNumber= "Test", // BookProNumber >
                   BookRouteFinalCode = "NS" , // BookRouteFinalCode >
                   BookRouteFinalDate = "11/07/2023" , // BookRouteFinalDate >
                   BookTotalCases = 0, // BookTotalCases >
                   BookTotalWgt = 0, // BookTotalWgt >
                   BookTotalPL = 0, // BookTotalPL >
                   BookTotalCube = 0, // BookTotalCube >
                   BookTotalBFC = 0, // BookTotalBFC >
                   BookStopNo = 0, // BookStopNo >
                   CompName= "Test", // CompName >
                   CompNumber= "Test", // CompNumber >
                   BookTypeCode = "N" , // BookTypeCode >
                   BookDateOrdered = "11/07/2023" , // BookDateOrdered >
                   BookOrigName= "Test", // BookOrigName >
                   BookOrigAddress1= "Test", // BookOrigAddress1 >
                   BookOrigAddress2= "Test", // BookOrigAddress2 >
                   BookOrigAddress3= "Test", // BookOrigAddress3 >
                   BookOrigCity= "Test", // BookOrigCity >
                   BookOrigState = "TN" , // BookOrigState >
                   BookOrigCountry= "Test", // BookOrigCountry >
                   BookOrigZip= "Test", // BookOrigZip >
                   BookDestName= "Test", // BookDestName >
                   BookDestAddress1= "Test", // BookDestAddress1 >
                   BookDestAddress2= "Test", // BookDestAddress2 >
                   BookDestAddress3= "Test", // BookDestAddress3 >
                   BookDestCity= "Test", // BookDestCity >
                   BookDestState = "TN" , // BookDestState >
                   BookDestCountry= "Test", // BookDestCountry >
                   BookDestZip= "Test", // BookDestZip >
                   BookLoadPONumber= "Test", // BookLoadPONumber >
                   CarrierName= "Test", // CarrierName >
                   LaneNumber= "Test", // LaneNumber >
                   CommCodeDescription= "Test", // CommCodeDescription >
                   BookMilesFrom = "0", // BookMilesFrom >
                   BookCommCompControl = 0, // BookCommCompControl >
                   BookRevCommCost = 0, // BookRevCommCost >
                   BookRevGrossRevenue = 0, // BookRevGrossRevenue >
                   BookFinCommStd = 0, // BookFinCommStd >
                   BookDoNotInvoice = false , // BookDoNotInvoice >
                   BookOrderSequence = 0, // BookOrderSequence >
                   CarrierEquipmentCodes= "Test", // CarrierEquipmentCodes >
                   BookCarrierTypeCode= "Test", // BookCarrierTypeCode >
                   BookWarehouseNumber= "Test", // BookWarehouseNumber >
                   CompNatNumber = 0, // CompNatNumber >
                   BookTransType= "Test", // BookTransType >
                   BookShipCarrierProNumber= "Test", // BookShipCarrierProNumber >
                   BookShipCarrierNumber= "Test", // BookShipCarrierNumber >
                   LaneComments= "Test", // LaneComments >
                   FuelSurCharge = 0, // FuelSurCharge >
                   BookRevCarrierCost = 0, // BookRevCarrierCost >
                   BookRevOtherCost = 0, // BookRevOtherCost >
                   BookRevNetCost = 0, // BookRevNetCost >
                   BookRevFreightTax = 0, // BookRevFreightTax >
                   BookFinServiceFee = 0, // BookFinServiceFee >
                   BookRevLoadSavings = 0, // BookRevLoadSavings >
                   TotalNonFuelFees = 0, // TotalNonFuelFees >
                   BookPickNumber = 0, // BookPickNumber >
                   BookPickupStopNumber = 0, // BookPickupStopNumber >
                   PLExportRetry = 0, // PLExportRetry >
                   PLExportDate = "11/07/2023" , // PLExportDate >
                   PLExported = true, // PLExported >
                   CarrierAlphaCode= "Test", // CarrierAlphaCode >
                   CarrierLegalEntity= "Test", // CarrierLegalEntity >
                   CompLegalEntity= "Test", // CompLegalEntity >
                   CompAlphaCode= "Test", // CompAlphaCode >
                   LaneLegalEntity= "Test", // LaneLegalEntity >
                   BookOriginalLaneNumber= "Test", // BookOriginalLaneNumber >
                   BookAlternateAddressLaneNumber= "Test", // BookAlternateAddressLaneNumber >
                   BookSHID= "Test", // BookSHID >
                   BookOriginalLaneLegalEntity= "Test", // BookOriginalLaneLegalEntity >
                   BookWhseAuthorizationNo= "Test", // BookWhseAuthorizationNo >
                   BookShipCarrierName= "Test", // BookShipCarrierName >
                   BookRevNonTaxable = 0, // BookRevNonTaxable >
                   BookFinAPGLNumber= "Test", // BookFinAPGLNumber >
                };
                List<PickLines> lLines = new List<PickLines>();
                lLines.Add(new PickLines()
                {
                    PLControl = iControl, //PLControl >
                    BookCarrOrderNumber = "Test", // BookCarrOrderNumber >
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
                    BookItemRatedMarineCode = "Test", // BookItemRatedMarineCode >
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
                    Stackable = 0, // Stackable >
                    LevelOfDensity = 0, // LevelOfDensity >
                    BookItemOrderNumber = "Test" // BookItemOrderNumber >

                });
                lLines.Add(new PickLines()
                {
                    PLControl = iControl, //PLControl >
                    BookCarrOrderNumber = "Test", // BookCarrOrderNumber >
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
                    BookItemRatedMarineCode = "Test", // BookItemRatedMarineCode >
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
                    Stackable = 0, // Stackable >
                    LevelOfDensity = 0, // LevelOfDensity >
                    BookItemOrderNumber = "Test" // BookItemOrderNumber >

                });
                oPick.Lines = lLines.ToArray();
                lPicks.Add(oPick);
            }
            oSendPicks.dynamicsTMSPicks = lPicks.ToArray();
            return oSendPicks;
        }

        public static string GetSOAPBody(SendPicks oSendPicks)
        {
            string sSOAP = "";
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UnicodeEncoding(bigEndian: false, byteOrderMark: false);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(oSendPicks.GetType());
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, settings);
            serializer.Serialize(writer, oSendPicks);
            sSOAP = Encoding.Unicode.GetString(ms.ToArray());

            return sSOAP;

        }


        public static string GetSOAPBody(int[] iPLControls)
        {
            string sSOAP = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(" <SendPicks xmlns=\"urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices\" >");
            sb.Append(" <dynamicsTMSPicks>");
            foreach (int iControl in iPLControls)
            {          
                sb.Append(" <Pick xmlns=\"urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPick\" >");
                sb.AppendFormat("<PLControl>{0}</PLControl>",iControl);
                sb.Append("<BookCarrOrderNumber>Test</BookCarrOrderNumber>");
                sb.Append("<BookConsPrefix>Test</BookConsPrefix>");
                sb.Append("<CarrierNumber>0</CarrierNumber>");
                sb.Append("<BookRevTotalCost>100</BookRevTotalCost>");
                sb.Append("<LoadOrder>0</LoadOrder>");
                sb.Append("<BookDateLoad>11/07/2023</BookDateLoad>");
                sb.Append("<BookDateRequired>11/07/2023</BookDateRequired>");
                sb.Append("<BookLoadCom>D</BookLoadCom>");
                sb.Append("<BookProNumber>Test</BookProNumber>");
                sb.Append("<BookRouteFinalCode>NS</BookRouteFinalCode>");
                sb.Append("<BookRouteFinalDate>11/07/2023</BookRouteFinalDate>");
                sb.Append("<BookTotalCases>0</BookTotalCases>");
                sb.Append("<BookTotalWgt>0</BookTotalWgt>");
                sb.Append("<BookTotalPL>0</BookTotalPL>");
                sb.Append("<BookTotalCube>0</BookTotalCube>");
                sb.Append("<BookTotalBFC>0</BookTotalBFC>");
                sb.Append("<BookStopNo>0</BookStopNo>");
                sb.Append("<CompName>Test</CompName>");
                sb.Append("<CompNumber>Test</CompNumber>");
                sb.Append("<BookTypeCode>N</BookTypeCode>");
                sb.Append("<BookDateOrdered>11/07/2023</BookDateOrdered>");
                sb.Append("<BookOrigName>Test</BookOrigName>");
                sb.Append("<BookOrigAddress1>Test</BookOrigAddress1>");
                sb.Append("<BookOrigAddress2>Test</BookOrigAddress2>");
                sb.Append("<BookOrigAddress3>Test</BookOrigAddress3>");
                sb.Append("<BookOrigCity>Test</BookOrigCity>");
                sb.Append("<BookOrigState>TN</BookOrigState>");
                sb.Append("<BookOrigCountry>Test</BookOrigCountry>");
                sb.Append("<BookOrigZip>Test</BookOrigZip>");
                sb.Append("<BookDestName>Test</BookDestName>");
                sb.Append("<BookDestAddress1>Test</BookDestAddress1>");
                sb.Append("<BookDestAddress2>Test</BookDestAddress2>");
                sb.Append("<BookDestAddress3>Test</BookDestAddress3>");
                sb.Append("<BookDestCity>Test</BookDestCity>");
                sb.Append("<BookDestState>TN</BookDestState>");
                sb.Append("<BookDestCountry>Test</BookDestCountry>");
                sb.Append("<BookDestZip>Test</BookDestZip>");
                sb.Append("<BookLoadPONumber>Test</BookLoadPONumber>");
                sb.Append("<CarrierName>Test</CarrierName>");
                sb.Append("<LaneNumber>Test</LaneNumber>");
                sb.Append("<CommCodeDescription>Test</CommCodeDescription>");
                sb.Append("<BookMilesFrom>0</BookMilesFrom>");
                sb.Append("<BookCommCompControl>0</BookCommCompControl>");
                sb.Append("<BookRevCommCost>0</BookRevCommCost>");
                sb.Append("<BookRevGrossRevenue>0</BookRevGrossRevenue>");
                sb.Append("<BookFinCommStd>0</BookFinCommStd>");
                sb.Append("<BookDoNotInvoice>false</BookDoNotInvoice>");
                sb.Append("<BookOrderSequence>0</BookOrderSequence>");
                sb.Append("<CarrierEquipmentCodes>Test</CarrierEquipmentCodes>");
                sb.Append("<BookCarrierTypeCode>Test</BookCarrierTypeCode>");
                sb.Append("<BookWarehouseNumber>Test</BookWarehouseNumber>");
                sb.Append("<CompNatNumber>0</CompNatNumber>");
                sb.Append("<BookTransType>Test</BookTransType>");
                sb.Append("<BookShipCarrierProNumber>Test</BookShipCarrierProNumber>");
                sb.Append("<BookShipCarrierNumber>Test</BookShipCarrierNumber>");
                sb.Append("<LaneComments>Test</LaneComments>");
                sb.Append("<FuelSurCharge>0</FuelSurCharge>");
                sb.Append("<BookRevCarrierCost>0</BookRevCarrierCost>");
                sb.Append("<BookRevOtherCost>0</BookRevOtherCost>");
                sb.Append("<BookRevNetCost>0</BookRevNetCost>");
                sb.Append("<BookRevFreightTax>0</BookRevFreightTax>");
                sb.Append("<BookFinServiceFee>0</BookFinServiceFee>");
                sb.Append("<BookRevLoadSavings>0</BookRevLoadSavings>");
                sb.Append("<TotalNonFuelFees>0</TotalNonFuelFees>");
                sb.Append("<BookPickNumber>0</BookPickNumber>");
                sb.Append("<BookPickupStopNumber>0</BookPickupStopNumber>");
                sb.Append("<PLExportRetry>0</PLExportRetry>");
                sb.Append("<PLExportDate>11/07/2023</PLExportDate>");
                sb.Append("<PLExported>true</PLExported>");
                sb.Append("<CarrierAlphaCode>Test</CarrierAlphaCode>");
                sb.Append("<CarrierLegalEntity>Test</CarrierLegalEntity>");
                sb.Append("<CompLegalEntity>Test</CompLegalEntity>");
                sb.Append("<CompAlphaCode>Test</CompAlphaCode>");
                sb.Append("<LaneLegalEntity>Test</LaneLegalEntity>");
                sb.Append("<BookOriginalLaneNumber>Test</BookOriginalLaneNumber>");
                sb.Append("<BookAlternateAddressLaneNumber>Test</BookAlternateAddressLaneNumber>");
                sb.Append("<BookSHID>Test</BookSHID>");
                sb.Append("<BookOriginalLaneLegalEntity>Test</BookOriginalLaneLegalEntity>");
                sb.Append("<BookWhseAuthorizationNo>Test</BookWhseAuthorizationNo>");
                sb.Append("<BookShipCarrierName>Test</BookShipCarrierName>");
                sb.Append("<BookRevNonTaxable>0</BookRevNonTaxable>");
                sb.Append("<BookFinAPGLNumber>Test</BookFinAPGLNumber>");
                sb.Append("<Lines>");
                sb.Append("<BookCarrOrderNumber>Test</BookCarrOrderNumber>");
                sb.Append("<ItemNumber>Test</ItemNumber>");
                sb.Append("<QtyOrdered>0</QtyOrdered>");
                sb.Append("<FreightCost>0</FreightCost>");
                sb.Append("<ItemCost>0</ItemCost>");
                sb.Append("<Weight>0</Weight>");
                sb.Append("<Cube>0</Cube>");
                sb.Append("<Pack>0</Pack>");
                sb.Append("<Size>Test</Size>");
                sb.Append("<Description>Test</Description>");
                sb.Append("<CustomerItemNumber>Test</CustomerItemNumber>");
                sb.Append("<CustomerNumber>Test</CustomerNumber>");
                sb.Append("<OrderSequence>0</OrderSequence>");
                sb.Append("<Hazmat>N</Hazmat>");
                sb.Append("<Brand>Test</Brand>");
                sb.Append("<CostCenter>Test</CostCenter>");
                sb.Append("<LotNumber>Test</LotNumber>");
                sb.Append("<LotExpirationDate>11/07/2023</LotExpirationDate>");
                sb.Append("<GTIN>Test</GTIN>");
                sb.Append("<BFC>0</BFC>");
                sb.Append("<CountryOfOrigin>Test</CountryOfOrigin>");
                sb.Append("<CustomerPONumber>Test</CustomerPONumber>");
                sb.Append("<HST>Test</HST>");
                sb.Append("<BookProNumber>Test</BookProNumber>");
                sb.Append("<PalletType>Test</PalletType>");
                sb.AppendFormat("<PLControl>{0}</PLControl>", iControl);
                sb.Append("<CompNatNumber>0</CompNatNumber>");
                sb.Append("<BookRouteConsFlag>true</BookRouteConsFlag>");
                sb.Append("<BookAlternateAddressLaneNumber>Test</BookAlternateAddressLaneNumber>");
                sb.Append("<CompLegalEntity>Test</CompLegalEntity>");
                sb.Append("<CompAlphaCode>Test</CompAlphaCode>");
                sb.Append("<BookItemDiscount>0</BookItemDiscount>");
                sb.Append("<BookItemLineHaul>0</BookItemLineHaul>");
                sb.Append("<BookItemTaxableFees>0</BookItemTaxableFees>");
                sb.Append("<BookItemTaxes>0</BookItemTaxes>");
                sb.Append("<BookItemNonTaxableFees>0</BookItemNonTaxableFees>");
                sb.Append("<BookItemWeightBreak>0</BookItemWeightBreak>");
                sb.Append("<BookItemRated49CFRCode>Test</BookItemRated49CFRCode>");
                sb.Append("<BookItemRatedIATACode>Test</BookItemRatedIATACode>");
                sb.Append("<BookItemRatedDOTCode>Test</BookItemRatedDOTCode>");
                sb.Append("<BookItemRatedMarineCode>Test</BookItemRatedMarineCode>");
                sb.Append("<BookItemRatedNMFCClass>Test</BookItemRatedNMFCClass>");
                sb.Append("<BookItemRatedNMFCSubClass>Test</BookItemRatedNMFCSubClass>");
                sb.Append("<BookItemRatedFAKClass>Test</BookItemRatedFAKClass>");
                sb.Append("<HazmatTypeCode>Test</HazmatTypeCode>");
                sb.Append("<Hazmat49CFRCode>Test</Hazmat49CFRCode>");
                sb.Append("<IATACode>Test</IATACode>");
                sb.Append("<DOTCode>Test</DOTCode>");
                sb.Append("<MarineCode>Test</MarineCode>");
                sb.Append("<NMFCClass>Test</NMFCClass>");
                sb.Append("<FAKClass>Test</FAKClass>");
                sb.Append("<LimitedQtyFlag>true</LimitedQtyFlag>");
                sb.Append("<Pallets>0</Pallets>");
                sb.Append("<Ties>0</Ties>");
                sb.Append("<Highs>0</Highs>");
                sb.Append("<QtyPalletPercentage>0</QtyPalletPercentage>");
                sb.Append("<QtyLength>0</QtyLength>");
                sb.Append("<QtyWidth>0</QtyWidth>");
                sb.Append("<QtyHeight>0</QtyHeight>");
                sb.Append("<Stackable>0</Stackable>");
                sb.Append("<LevelOfDensity>0</LevelOfDensity>");
                sb.Append("<BookItemOrderNumber>Test</BookItemOrderNumber>");
                sb.Append("</Lines>");
                sb.Append("</Pick>");
            }

            sb.Append("</dynamicsTMSPicks>");
            sb.Append("</SendPicks>");


            sSOAP = sb.ToString();
            return sSOAP;

        }

        public static async Task<Envelope> Send(string bearerToken, oAuth2Settings sSettings, string sLegal, string sSOAPBody)
        {

            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":SendPicks");
                //request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSIsImtpZCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSJ9.eyJhdWQiOiJodHRwczovL2FwaS5idXNpbmVzc2NlbnRyYWwuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMjUxOGJlN2UtYzkzMy00OTA1LWFmNjQtMjRhZDAxNTcyMDJmLyIsImlhdCI6MTY5ODg1NjY2MSwibmJmIjoxNjk4ODU2NjYxLCJleHAiOjE2OTg4NjA1NjEsImFpbyI6IkUyRmdZTmd6NWVJRFM5WEVTRzhKd1pLYXNySkNBQT09IiwiYXBwaWQiOiI4ZmMyYWJiZS05ZTliLTQ0OTYtOGU2Ny1iZDA4ZDYyZGFkNDciLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yNTE4YmU3ZS1jOTMzLTQ5MDUtYWY2NC0yNGFkMDE1NzIwMmYvIiwiaWR0eXAiOiJhcHAiLCJvaWQiOiI4NmVmMzA1My04OTdkLTRhYzctODljNC1mODkwZWM5ZGJjNWYiLCJyaCI6IjAuQVc0QWZyNFlKVFBKQlVtdlpDU3RBVmNnTHozdmJabHNzMU5CaGdlbV9Ud0J1Sjl1QUFBLiIsInJvbGVzIjpbIkF1dG9tYXRpb24uUmVhZFdyaXRlLkFsbCIsImFwcF9hY2Nlc3MiLCJBUEkuUmVhZFdyaXRlLkFsbCJdLCJzdWIiOiI4NmVmMzA1My04OTdkLTRhYzctODljNC1mODkwZWM5ZGJjNWYiLCJ0aWQiOiIyNTE4YmU3ZS1jOTMzLTQ5MDUtYWY2NC0yNGFkMDE1NzIwMmYiLCJ1dGkiOiJYZGRYSFkySEMweWNROV9JdWZjMEFBIiwidmVyIjoiMS4wIn0.atbwWAfYnYEhSt_cyP7Jv5q7tsljv5JQIAoDzV17BAxBqYEykuJYsEEKK_Is1vkztnLSuADoOaEUiChtWGRS4PwAN_AHGieBH47V50yMS1tC2PbYI0CMCIqsgw-0yp5sAQBFHH_SXhuR9Kg7ya2ERmD35Hy_1uKv6wXFIBM27R3LC74bSx_GmcZu-sjfib-uoLyGArPPswa17oBQmnJcv22j3I8UhCH8WJev0fDn9ujFBqqOImz9YnrSKt8fT6uL_59Omej36bqPPwZqxZPvFX4n1u4CJgNTc0mVWVjHqBCw6z0GS8lOIjTrLTQ2skuSZ_b-yoNFZ1c4-x9GTOwTVw");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body>" + sSOAPBody + "</soap:Body></soap:Envelope>\r\n", null, "text/xml");
                request.Content = content;
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
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


        public static async Task<Envelope> SendNoPost(string bearerToken, oAuth2Settings sSettings, string sLegal, string sSOAPBody)
        {

            Envelope oRet = new Envelope();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, sSettings.DataUrl);
                request.Headers.Add("SOAPAction", "urn:" + sSettings.ActionUrl + ":SendPicksNoPost");
                //request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSIsImtpZCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSJ9.eyJhdWQiOiJodHRwczovL2FwaS5idXNpbmVzc2NlbnRyYWwuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMjUxOGJlN2UtYzkzMy00OTA1LWFmNjQtMjRhZDAxNTcyMDJmLyIsImlhdCI6MTY5ODg1NjY2MSwibmJmIjoxNjk4ODU2NjYxLCJleHAiOjE2OTg4NjA1NjEsImFpbyI6IkUyRmdZTmd6NWVJRFM5WEVTRzhKd1pLYXNySkNBQT09IiwiYXBwaWQiOiI4ZmMyYWJiZS05ZTliLTQ0OTYtOGU2Ny1iZDA4ZDYyZGFkNDciLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yNTE4YmU3ZS1jOTMzLTQ5MDUtYWY2NC0yNGFkMDE1NzIwMmYvIiwiaWR0eXAiOiJhcHAiLCJvaWQiOiI4NmVmMzA1My04OTdkLTRhYzctODljNC1mODkwZWM5ZGJjNWYiLCJyaCI6IjAuQVc0QWZyNFlKVFBKQlVtdlpDU3RBVmNnTHozdmJabHNzMU5CaGdlbV9Ud0J1Sjl1QUFBLiIsInJvbGVzIjpbIkF1dG9tYXRpb24uUmVhZFdyaXRlLkFsbCIsImFwcF9hY2Nlc3MiLCJBUEkuUmVhZFdyaXRlLkFsbCJdLCJzdWIiOiI4NmVmMzA1My04OTdkLTRhYzctODljNC1mODkwZWM5ZGJjNWYiLCJ0aWQiOiIyNTE4YmU3ZS1jOTMzLTQ5MDUtYWY2NC0yNGFkMDE1NzIwMmYiLCJ1dGkiOiJYZGRYSFkySEMweWNROV9JdWZjMEFBIiwidmVyIjoiMS4wIn0.atbwWAfYnYEhSt_cyP7Jv5q7tsljv5JQIAoDzV17BAxBqYEykuJYsEEKK_Is1vkztnLSuADoOaEUiChtWGRS4PwAN_AHGieBH47V50yMS1tC2PbYI0CMCIqsgw-0yp5sAQBFHH_SXhuR9Kg7ya2ERmD35Hy_1uKv6wXFIBM27R3LC74bSx_GmcZu-sjfib-uoLyGArPPswa17oBQmnJcv22j3I8UhCH8WJev0fDn9ujFBqqOImz9YnrSKt8fT6uL_59Omej36bqPPwZqxZPvFX4n1u4CJgNTc0mVWVjHqBCw6z0GS8lOIjTrLTQ2skuSZ_b-yoNFZ1c4-x9GTOwTVw");
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body>" + sSOAPBody + "</soap:Body></soap:Envelope>\r\n", null, "text/xml");
                request.Content = content;
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
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

        public static bool Save(Envelope oPickListResult)
        {
            bool blnRet = false;
            //add logic to save data to DTMS via Web Services
            if (oPickListResult != null && oPickListResult.Body != null )
            {
                if (oPickListResult.Body.Fault != null && oPickListResult.Body.Fault.detail != null)
                {
                    Console.WriteLine("Update Picklist Failed Message: " + oPickListResult.Body.Fault.detail.@string);
                    blnRet = false;
                }
                else
                {

                    blnRet = true;
                    Console.WriteLine("Update Picklist data Complete");
                }
            }
            else
            {
                Console.WriteLine("Update Picklist data failed, data not available");
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

        private object sendPicks_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public object SendPicks_Result
        {
            get
            {
                return this.sendPicks_ResultField;
            }
            set
            {
                this.sendPicks_ResultField = value;
            }
        }

        private object SendPicksNoPost_ResultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
        public object SendPicksNoPost_Result
        {
            get
            {
                return this.SendPicksNoPost_ResultField;
            }
            set
            {
                this.SendPicksNoPost_ResultField = value;
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
    

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-schemas/codeunit/DynamicsTMSWebServices", IsNullable = false)]
    public partial class SendPicks
    {

        private Pick[] dynamicsTMSPicksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Pick", Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPick", IsNullable = false)]
        public Pick[] dynamicsTMSPicks
        {
            get
            {
                return this.dynamicsTMSPicksField;
            }
            set
            {
                this.dynamicsTMSPicksField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPick")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPick", IsNullable = false)]
    public partial class Pick
    {

        private long pLControlField;

        private string bookCarrOrderNumberField;

        private string bookConsPrefixField;

        private int carrierNumberField;

        private decimal bookRevTotalCostField;

        private int loadOrderField;

        private string bookDateLoadField;

        private string bookDateRequiredField;

        private string bookLoadComField;

        private string bookProNumberField;

        private string bookRouteFinalCodeField;

        private string bookRouteFinalDateField;

        private int bookTotalCasesField;

        private decimal bookTotalWgtField;

        private decimal bookTotalPLField;

        private int bookTotalCubeField;

        private decimal bookTotalBFCField;

        private int bookStopNoField;

        private string compNameField;

        private string compNumberField;

        private string bookTypeCodeField;

        private string bookDateOrderedField;

        private string bookOrigNameField;

        private string bookOrigAddress1Field;

        private string bookOrigAddress2Field;

        private string bookOrigAddress3Field;

        private string bookOrigCityField;

        private string bookOrigStateField;

        private string bookOrigCountryField;

        private string bookOrigZipField;

        private string bookDestNameField;

        private string bookDestAddress1Field;

        private string bookDestAddress2Field;

        private string bookDestAddress3Field;

        private string bookDestCityField;

        private string bookDestStateField;

        private string bookDestCountryField;

        private string bookDestZipField;

        private string bookLoadPONumberField;

        private string carrierNameField;

        private string laneNumberField;

        private string commCodeDescriptionField;

        private string bookMilesFromField;

        private int bookCommCompControlField;

        private decimal bookRevCommCostField;

        private decimal bookRevGrossRevenueField;

        private decimal bookFinCommStdField;

        private bool bookDoNotInvoiceField;

        private int bookOrderSequenceField;

        private string carrierEquipmentCodesField;

        private string bookCarrierTypeCodeField;

        private string bookWarehouseNumberField;

        private int compNatNumberField;

        private string bookTransTypeField;

        private string bookShipCarrierProNumberField;

        private string bookShipCarrierNumberField;

        private string laneCommentsField;

        private decimal fuelSurChargeField;

        private decimal bookRevCarrierCostField;

        private decimal bookRevOtherCostField;

        private decimal bookRevNetCostField;

        private decimal bookRevFreightTaxField;

        private decimal bookFinServiceFeeField;

        private decimal bookRevLoadSavingsField;

        private decimal totalNonFuelFeesField;

        private int bookPickNumberField;

        private int bookPickupStopNumberField;

        private int pLExportRetryField;

        private string pLExportDateField;

        private bool pLExportedField;

        private string carrierAlphaCodeField;

        private string carrierLegalEntityField;

        private string compLegalEntityField;

        private string compAlphaCodeField;

        private string laneLegalEntityField;

        private string bookOriginalLaneNumberField;

        private string bookAlternateAddressLaneNumberField;

        private string bookSHIDField;

        private string bookOriginalLaneLegalEntityField;

        private string bookWhseAuthorizationNoField;

        private string bookShipCarrierNameField;

        private decimal bookRevNonTaxableField;

        private string bookFinAPGLNumberField;

        private PickLines[] linesField;

        /// <remarks/>
        public long PLControl
        {
            get
            {
                return this.pLControlField;
            }
            set
            {
                this.pLControlField = value;
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
        public decimal BookRevTotalCost
        {
            get
            {
                return this.bookRevTotalCostField;
            }
            set
            {
                this.bookRevTotalCostField = value;
            }
        }

        /// <remarks/>
        public int LoadOrder
        {
            get
            {
                return this.loadOrderField;
            }
            set
            {
                this.loadOrderField = value;
            }
        }

        /// <remarks/>
        public string BookDateLoad
        {
            get
            {
                return this.bookDateLoadField;
            }
            set
            {
                this.bookDateLoadField = value;
            }
        }

        /// <remarks/>
        public string BookDateRequired
        {
            get
            {
                return this.bookDateRequiredField;
            }
            set
            {
                this.bookDateRequiredField = value;
            }
        }

        /// <remarks/>
        public string BookLoadCom
        {
            get
            {
                return this.bookLoadComField;
            }
            set
            {
                this.bookLoadComField = value;
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
        public string BookRouteFinalCode
        {
            get
            {
                return this.bookRouteFinalCodeField;
            }
            set
            {
                this.bookRouteFinalCodeField = value;
            }
        }

        /// <remarks/>
        public string BookRouteFinalDate
        {
            get
            {
                return this.bookRouteFinalDateField;
            }
            set
            {
                this.bookRouteFinalDateField = value;
            }
        }

        /// <remarks/>
        public int BookTotalCases
        {
            get
            {
                return this.bookTotalCasesField;
            }
            set
            {
                this.bookTotalCasesField = value;
            }
        }

        /// <remarks/>
        public decimal BookTotalWgt
        {
            get
            {
                return this.bookTotalWgtField;
            }
            set
            {
                this.bookTotalWgtField = value;
            }
        }

        /// <remarks/>
        public decimal BookTotalPL
        {
            get
            {
                return this.bookTotalPLField;
            }
            set
            {
                this.bookTotalPLField = value;
            }
        }

        /// <remarks/>
        public int BookTotalCube
        {
            get
            {
                return this.bookTotalCubeField;
            }
            set
            {
                this.bookTotalCubeField = value;
            }
        }

        /// <remarks/>
        public decimal BookTotalBFC
        {
            get
            {
                return this.bookTotalBFCField;
            }
            set
            {
                this.bookTotalBFCField = value;
            }
        }

        /// <remarks/>
        public int BookStopNo
        {
            get
            {
                return this.bookStopNoField;
            }
            set
            {
                this.bookStopNoField = value;
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
        public string BookTypeCode
        {
            get
            {
                return this.bookTypeCodeField;
            }
            set
            {
                this.bookTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string BookDateOrdered
        {
            get
            {
                return this.bookDateOrderedField;
            }
            set
            {
                this.bookDateOrderedField = value;
            }
        }

        /// <remarks/>
        public string BookOrigName
        {
            get
            {
                return this.bookOrigNameField;
            }
            set
            {
                this.bookOrigNameField = value;
            }
        }

        /// <remarks/>
        public string BookOrigAddress1
        {
            get
            {
                return this.bookOrigAddress1Field;
            }
            set
            {
                this.bookOrigAddress1Field = value;
            }
        }

        /// <remarks/>
        public string BookOrigAddress2
        {
            get
            {
                return this.bookOrigAddress2Field;
            }
            set
            {
                this.bookOrigAddress2Field = value;
            }
        }

        /// <remarks/>
        public string BookOrigAddress3
        {
            get
            {
                return this.bookOrigAddress3Field;
            }
            set
            {
                this.bookOrigAddress3Field = value;
            }
        }

        /// <remarks/>
        public string BookOrigCity
        {
            get
            {
                return this.bookOrigCityField;
            }
            set
            {
                this.bookOrigCityField = value;
            }
        }

        /// <remarks/>
        public string BookOrigState
        {
            get
            {
                return this.bookOrigStateField;
            }
            set
            {
                this.bookOrigStateField = value;
            }
        }

        /// <remarks/>
        public string BookOrigCountry
        {
            get
            {
                return this.bookOrigCountryField;
            }
            set
            {
                this.bookOrigCountryField = value;
            }
        }

        /// <remarks/>
        public string BookOrigZip
        {
            get
            {
                return this.bookOrigZipField;
            }
            set
            {
                this.bookOrigZipField = value;
            }
        }

        /// <remarks/>
        public string BookDestName
        {
            get
            {
                return this.bookDestNameField;
            }
            set
            {
                this.bookDestNameField = value;
            }
        }

        /// <remarks/>
        public string BookDestAddress1
        {
            get
            {
                return this.bookDestAddress1Field;
            }
            set
            {
                this.bookDestAddress1Field = value;
            }
        }

        /// <remarks/>
        public string BookDestAddress2
        {
            get
            {
                return this.bookDestAddress2Field;
            }
            set
            {
                this.bookDestAddress2Field = value;
            }
        }

        /// <remarks/>
        public string BookDestAddress3
        {
            get
            {
                return this.bookDestAddress3Field;
            }
            set
            {
                this.bookDestAddress3Field = value;
            }
        }

        /// <remarks/>
        public string BookDestCity
        {
            get
            {
                return this.bookDestCityField;
            }
            set
            {
                this.bookDestCityField = value;
            }
        }

        /// <remarks/>
        public string BookDestState
        {
            get
            {
                return this.bookDestStateField;
            }
            set
            {
                this.bookDestStateField = value;
            }
        }

        /// <remarks/>
        public string BookDestCountry
        {
            get
            {
                return this.bookDestCountryField;
            }
            set
            {
                this.bookDestCountryField = value;
            }
        }

        /// <remarks/>
        public string BookDestZip
        {
            get
            {
                return this.bookDestZipField;
            }
            set
            {
                this.bookDestZipField = value;
            }
        }

        /// <remarks/>
        public string BookLoadPONumber
        {
            get
            {
                return this.bookLoadPONumberField;
            }
            set
            {
                this.bookLoadPONumberField = value;
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
        public string CommCodeDescription
        {
            get
            {
                return this.commCodeDescriptionField;
            }
            set
            {
                this.commCodeDescriptionField = value;
            }
        }

        /// <remarks/>
        public string BookMilesFrom
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
        public int BookCommCompControl
        {
            get
            {
                return this.bookCommCompControlField;
            }
            set
            {
                this.bookCommCompControlField = value;
            }
        }

        /// <remarks/>
        public decimal BookRevCommCost
        {
            get
            {
                return this.bookRevCommCostField;
            }
            set
            {
                this.bookRevCommCostField = value;
            }
        }

        /// <remarks/>
        public decimal BookRevGrossRevenue
        {
            get
            {
                return this.bookRevGrossRevenueField;
            }
            set
            {
                this.bookRevGrossRevenueField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinCommStd
        {
            get
            {
                return this.bookFinCommStdField;
            }
            set
            {
                this.bookFinCommStdField = value;
            }
        }

        /// <remarks/>
        public bool BookDoNotInvoice
        {
            get
            {
                return this.bookDoNotInvoiceField;
            }
            set
            {
                this.bookDoNotInvoiceField = value;
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
        public string BookTransType
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
        public decimal FuelSurCharge
        {
            get
            {
                return this.fuelSurChargeField;
            }
            set
            {
                this.fuelSurChargeField = value;
            }
        }

        /// <remarks/>
        public decimal BookRevCarrierCost
        {
            get
            {
                return this.bookRevCarrierCostField;
            }
            set
            {
                this.bookRevCarrierCostField = value;
            }
        }

        /// <remarks/>
        public decimal BookRevOtherCost
        {
            get
            {
                return this.bookRevOtherCostField;
            }
            set
            {
                this.bookRevOtherCostField = value;
            }
        }

        /// <remarks/>
        public decimal BookRevNetCost
        {
            get
            {
                return this.bookRevNetCostField;
            }
            set
            {
                this.bookRevNetCostField = value;
            }
        }

        /// <remarks/>
        public decimal BookRevFreightTax
        {
            get
            {
                return this.bookRevFreightTaxField;
            }
            set
            {
                this.bookRevFreightTaxField = value;
            }
        }

        /// <remarks/>
        public decimal BookFinServiceFee
        {
            get
            {
                return this.bookFinServiceFeeField;
            }
            set
            {
                this.bookFinServiceFeeField = value;
            }
        }

        /// <remarks/>
        public decimal BookRevLoadSavings
        {
            get
            {
                return this.bookRevLoadSavingsField;
            }
            set
            {
                this.bookRevLoadSavingsField = value;
            }
        }

        /// <remarks/>
        public decimal TotalNonFuelFees
        {
            get
            {
                return this.totalNonFuelFeesField;
            }
            set
            {
                this.totalNonFuelFeesField = value;
            }
        }

        /// <remarks/>
        public int BookPickNumber
        {
            get
            {
                return this.bookPickNumberField;
            }
            set
            {
                this.bookPickNumberField = value;
            }
        }

        /// <remarks/>
        public int BookPickupStopNumber
        {
            get
            {
                return this.bookPickupStopNumberField;
            }
            set
            {
                this.bookPickupStopNumberField = value;
            }
        }

        /// <remarks/>
        public int PLExportRetry
        {
            get
            {
                return this.pLExportRetryField;
            }
            set
            {
                this.pLExportRetryField = value;
            }
        }

        /// <remarks/>
        public string PLExportDate
        {
            get
            {
                return this.pLExportDateField;
            }
            set
            {
                this.pLExportDateField = value;
            }
        }

        /// <remarks/>
        public bool PLExported
        {
            get
            {
                return this.pLExportedField;
            }
            set
            {
                this.pLExportedField = value;
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
        public string BookOriginalLaneNumber
        {
            get
            {
                return this.bookOriginalLaneNumberField;
            }
            set
            {
                this.bookOriginalLaneNumberField = value;
            }
        }

        /// <remarks/>
        public string BookAlternateAddressLaneNumber
        {
            get
            {
                return this.bookAlternateAddressLaneNumberField;
            }
            set
            {
                this.bookAlternateAddressLaneNumberField = value;
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
        public string BookOriginalLaneLegalEntity
        {
            get
            {
                return this.bookOriginalLaneLegalEntityField;
            }
            set
            {
                this.bookOriginalLaneLegalEntityField = value;
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
        public decimal BookRevNonTaxable
        {
            get
            {
                return this.bookRevNonTaxableField;
            }
            set
            {
                this.bookRevNonTaxableField = value;
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
        /// 
        [XmlElement]
        public PickLines[] Lines
        {
            get
            {
                return this.linesField;
            }
            set
            {
                this.linesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSPick")]
    public partial class PickLines
    {

        private string bookCarrOrderNumberField;

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

        private decimal bFCField;

        private string countryOfOriginField;

        private string customerPONumberField;

        private string hSTField;

        private string bookProNumberField;

        private string palletTypeField;

        private long pLControlField;

        private int compNatNumberField;

        private bool bookRouteConsFlagField;

        private string bookAlternateAddressLaneNumberField;

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

        private string bookItemRatedMarineCodeField;

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

        private int stackableField;

        private int levelOfDensityField;

        private string bookItemOrderNumberField;

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
        public long PLControl
        {
            get
            {
                return this.pLControlField;
            }
            set
            {
                this.pLControlField = value;
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
        public string BookAlternateAddressLaneNumber
        {
            get
            {
                return this.bookAlternateAddressLaneNumberField;
            }
            set
            {
                this.bookAlternateAddressLaneNumberField = value;
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
        public string BookItemRatedMarineCode
        {
            get
            {
                return this.bookItemRatedMarineCodeField;
            }
            set
            {
                this.bookItemRatedMarineCodeField = value;
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
        public int Stackable
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

