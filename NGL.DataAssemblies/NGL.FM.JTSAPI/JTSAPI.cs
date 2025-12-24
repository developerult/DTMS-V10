using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Web;

namespace NGL.FM.JTSAPI
{
    public class JTSAPI
    {
        public JTSAPI()
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        }

        public JTSAPI(bool bUseTLs12)
        {
            if (bUseTLs12)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            }
        }


        public enum MessageEnum
        {
            None,
            E_UnExpected, //"An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes."
            E_OptionalCharge, //"Additional charges if required
            E_NoRatesFound, //No Rates Found
            E_InvalidCarrierNumber, // "Invalid Carrier Configuration for API";
            E_CommunicationFailure, // "Communication with API Service Failed"
            E_InvalidShipDate, // "Invalid Ship Date"
            E_WeightTooLowForLTL, //"The weight is too low for LTL
            E_WeightTooHighForLTL, //"The weight is too high for LTL and too low for truckload
            E_WeightTooLowForTL, //"The weight is too low for Truckload
            E_WeightTooHighForTL //"The weight is too high for truckload
        }


        public static string getMessageNotLocalized(JTSAPI.MessageEnum key, string sDefault)
        {
            string sRet = sDefault;
            switch (key)
            {
                case MessageEnum.None:
                    sRet = "";
                    break;
                case MessageEnum.E_UnExpected:
                    sRet = "An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes.";
                    break;
                case MessageEnum.E_OptionalCharge:
                    sRet = "Additional charges if required";
                    break;
                case MessageEnum.E_NoRatesFound:
                    sRet = "No Rates Found";
                    break;
                case MessageEnum.E_InvalidCarrierNumber:
                    sRet = "Invalid Carrier Configuration for API";
                    break;
                case MessageEnum.E_CommunicationFailure:
                    sRet = "Communication with API Service Failed";
                    break;
                case MessageEnum.E_InvalidShipDate:
                    sRet = "Invalid Ship Date";
                    break;
                case MessageEnum.E_WeightTooLowForLTL:
                    sRet = "The weight is too low for LTL";
                    break;
                case MessageEnum.E_WeightTooHighForLTL:
                    sRet = "The weight is too high for LTL and too low for truckload";
                    break;
                case MessageEnum.E_WeightTooLowForTL:
                    sRet = "The weight is too low for Truckload";
                    break;
                case MessageEnum.E_WeightTooHighForTL:
                    sRet = "The weight is too high for truckload";
                    break;
                default:
                    break;
            }
            return sRet;
        }


        public enum RefNumbers
        {
            SHID, // - Shipment ID #,
            MBOL, // - Master Bill of Lading #,
            PO, //- Purchase Order #,
            BOL, // - Bill of Lading #,
            CRID, // - Customer Specific Ref Number,
            CUSTPO, // - Customer PO Number,
            DEL, // - Delivery #, 
            JNO, // - Job Number, 
            PU, // - Pickup #, 
            CON, // - Customer Order Number,
            APPT, // - Appointment Number
        }

        public enum TransMode
        {
            LTL,
            TL,
            Air,
            Ocean,
            Bulk,
            Consol,
            Flatbed

        }

        public enum EquipType { Van, Reefer, Flatbed, Lcl, Container20, Container40, Container40HighCube, Container45HighCube, Container20FlatRack, Container40FlatRack, Container40HighCubeFlatRack, Container45FlatRack, Container20OpenTop, Container40OpenTop, Container20Reefer, Container40Reefer, Container40HighCubeReefer, Container45Reefer, Container20ReeferDryUsage, Container40ReeferDryUsage, Container40HighCubeReeferDryUsage, Container45ReeferDryUsage, Container20Platform }

        public enum SpecialRequirement
        {
            accessorialNeedsAppointmentNotification,
            accessorialNeedsLiftGateRequired,
            accessorialNeedsInsideDelivery,
            accessorialNeedsLimitedAccess,
            accessorialNeedsResidentialDelivery,
            accessorialNeedsConventionOrTradeShowDelivery,
            accessorialNeedsHazardousShipment,
            accessorialNeedsGuaranteeBy5pm,
            accessorialNeedsGuaranteeByNoon,
            accessorialNeedsPupRequired,
            accessorialNeedsMineDelivery,
            accessorialNeedsFreezeProtect,
            accessorialExtremeLength8To10Feet,
            accessorialExtremeLength11To12Feet,
            accessorialExtremeLength13To14Feet,
            accessorialExtremeLength15To16Feet,
            accessorialExtremeLength17To18Feet,
            accessorialExtremeLength19To20Feet
        }

        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string audience { get; set; }
        public string grant_type { get; set; }
        public JTSTokenData getToken(string sclient_id = "0oa9kmcecjIeC1Rgn357", string sclient_secret = "Wy9DFX_AQRoVlC4rMIcqUdgVcFJ0SpgcvoRnolKY", string saudience = "https://inavisphere.JTSobinson.com", string sgrant_type = "client_credentials")
        {
            JTSTokenResponse oRet = new JTSTokenResponse();
            // object tmp = null;
            try
            {
                ////For testing purposes only added on 24 - 09 - 2025
                //return null;

                // Commented out for testing purposes only added on 24-09-2025

                var client = new RestClient("https://sandbox-api.navisphere.com/v1/oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("client_id", sclient_id, ParameterType.GetOrPost);
                request.AddParameter("client_secret", sclient_secret, ParameterType.GetOrPost);
                request.AddParameter("audience", saudience, ParameterType.GetOrPost);
                request.AddParameter("grant_type", sgrant_type, ParameterType.GetOrPost);
                //var response = client.Get<MarketplaceSearJTSesponse>(request);
                oRet.response = client.Execute(request);
                //List<object> result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(oRet.response.Content).ToList();
                JTSTokenData oTokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<JTSTokenData>(oRet.response.Content);
                return oTokenData;


                //var result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(oRet.response.Content);
                //System.Diagnostics.Debug.WriteLine(oRet.ToString());
                //var sToken = result.access_token;
                //if (result.Count > 0)
                //{
                //    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result[0].ToString());
                //    if (values == null)
                //    {
                //        oRet.success = false;
                //    }
                //    oRet.tokenresponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JTSGetTokenResponse>(values["content"].ToString());
                //    //Newtonsoft.Json.JsonConvert.DeserializeObject<JTSGetTokenResponse>(values["content"].ToString()); values["access_token"].ToString();
                //    //            oRet.expires_in = values["expires_in"].ToString();
                //    //            oRet.token_type = values["token_type"].ToString();

                //    //lstPoints = Newtonsoft.Json.JsonConvert.DeserializeObject<TrimbleServiceReference.Coordinates>(values["Coords"].ToString());
                //    //            if (lstPoints != null)
                //    //                sResults = lstPoints.Lat + "," + lstPoints.Lon;
                //    //            else
                //    //                return -1;

                //}
                //else
                //{
                //    oRet.success = false;
                //}
                // tmp = result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return null;
        }

        public string getTestOrigin()
        {


            JTSAddress oAddress = new JTSAddress()
            {
                locationName = "Origin Location",
                address1 = "123 Main St",
                address2 = "Building 1",
                address3 = "Room 212",
                city = "WILMINGTON",
                stateProvinceCode = "MA",
                countryCode = "US",
                postalCode = "01887",
                latitude = 0,
                longitude = 0,
                specialRequirements = null,
                isPort = "false",
                unLocode = "US AC8",
                iata = "ACB",
                customerLocationId = "W541849",
                referenceNumbers = null

            };
            //string sRet = string.Format("\"fromCity\":\"{0}\",\"fromState\":\"{1}\",\"fromPostalCode\":\"{2}\"", oAddress.city, oAddress.stateProvinceCode, oAddress.postalCode);
            string sRet = "\"fromCity\":\"New Orleans\",\"fromState\":\"LA\",\"fromPostalCode\":\"70126\"";
            return sRet;
        }

        public string getTestDestination()
        {
            //JTSSpecialRequirement oSpecialReq = new JTSSpecialRequirement()
            //{
            //    liftGate = "false",
            //    insidePickup = "false",
            //    insideDelivery = "false",
            //    residentialNonCommercial = "false",
            //    limitedAccess = "false",
            //    tradeShoworConvention = "false",
            //    constructionSite = "false",
            //    dropOffAtCarrierTerminal = "false",
            //    pickupAtCarrierTerminal = "false"
            //};
            //JTSReferenceNumbers[] oRefs = new JTSReferenceNumbers[1];
            //oRefs[0] = new JTSReferenceNumbers { type = "DEL", value = "DeliverNumber1" };

            JTSAddress oAddress = new JTSAddress()
            {
                locationName = "Destination Location",
                address1 = "123 North AVE",
                address2 = "Building 1",
                address3 = "Suite 550",
                city = "ABERDEEN",
                stateProvinceCode = "SD",
                countryCode = "US",
                postalCode = "57402",
                latitude = 0,
                longitude = 0,
                specialRequirements = null,
                isPort = "false",
                unLocode = "US AC8",
                iata = "ACB",
                customerLocationId = "W541849",
                referenceNumbers = null
            };
            //string sRet = string.Format("\"toCity\":\"{0}\",\"toState\":\"{1}\",\"toPostalCode\":\"{2}\"", oAddress.city, oAddress.stateProvinceCode, oAddress.postalCode);
            string sRet = "\"toCity\":\"Oklahoma City\",\"toState\":\"OK\",\"toPostalCode\":\"73131\"";
            return sRet;
        }

        public string getTestMode(string sType, int iQty, string sMode)
        {

            JTSEquipment[] oEquipments = new JTSEquipment[1];
            oEquipments[0] = new JTSEquipment()
            {
                equipmentType = sType,
                quantity = iQty
            };

            JTSTransportMode oMode = new JTSTransportMode()
            {
                mode = sMode,
                equipments = oEquipments
            };
            string jMode = JsonConvert.SerializeObject(oMode);
            return jMode;
        }

        public string getTestRateReferenceNumbers()
        {

            JTSReferenceNumbers[] oRefs = new JTSReferenceNumbers[1];
            oRefs[0] = new JTSReferenceNumbers { type = "MBOL", value = "MBOL12345" };
            string jRefs = JsonConvert.SerializeObject(oRefs);
            return jRefs;
        }

        public string getTestItems()
        {
            JTSItem[] aItems = new JTSItem[1];
            ////,\n    \\"items\\": [\n        {{\n            \\"itemInvRecNo\\": 1089,\n            \\"height\\": \\"70\\",\n            \\"itemClass\\": \\"70\\",\n            \\"weight\\": \\"2000\\",\n            \\"palletSizeId\\": 0,\n            \\"palletCount\\": 0\n        }}\n    ]\n}}\"
            ////,\n    \\"items\\": [\n        {{\n            \\"
            ///"itemInvRecNo\\": 1089,\n            
            ///\\"height\\": \\"70\\",\n            
            ///\\"itemClass\\": \\"70\\",\n            
            ///\\"weight\\": \\"2000\\",\n            
            ///\\"palletSizeId\\": 0,\n            
            ///\\"palletCount\\": 0\n        }}\n    ]\n}}\"
            //itemInvRecNo = 64432 1089,
            //aItems[0] = new JTSItem
            //{

            //    itemInvRecNo = 64432,
            //    itemClass = "70",
            //    height = "48",
            //    weight = "2000",
            //    palletSizeId = 0,
            //    palletCount = 1
            //};

            //aItems[0] = new JTSItem
            //{

            //    itemInvRecNo = 64432,
            //    itemClass = "70",
            //    height = "48",
            //    weight = "1071.6",
            //    palletSizeId = 0,
            //    palletCount = 1
            //};

            //1071.6

            //aItems[0] = new JTSItem
            //{

            //    itemInvRecNo = 64436,
            //    itemClass = "100",
            //    height = "48",
            //    weight = "1072",
            //    palletSizeId = 0,
            //    palletCount = 1
            //};
            aItems[0] = new JTSItem
            {
                itemInvRecNo = 64435,
                itemClass = "92.5",
                height = "48",
                weight = "3150",
                palletSizeId = 0,
                palletCount = 5
            };
    //64436

    //aItems[0] = new JTSItem
    //{

    //    itemInvRecNo = 64432,
    //    itemClass = "70",
    //    height = "48",
    //    weight = "1071.6",
    //    palletSizeId = 0,
    //    palletCount = 1
    //};



            string jItems = JsonConvert.SerializeObject(aItems);
            return jItems;
        }



        public JTSQuoteResponse getTestHTTPRateRequest(string sToken)
        {
            //var request = (HttpWebRequest)WebRequest.Create("https://test.johansontrans.com/Rates/v2/LTL");
            var request = (HttpWebRequest)WebRequest.Create("https://www.johansontrans.com/Rates/v2/LTL");
            //string postData = "{ \"items\": " + getTestItems() + ",\"origin\":" + getTestOrigin() + ",\"destination\":" + getTestDestination() + ",\"shipDate\": \"2022-01-20T20:30:00.0000000Z\", \"customerCode\": \"C377465\",\"declaredValue\": 50000,\"transportModes\": [" + getTestMode("Van", 1, "LTL") + "],\"referenceNumbers\": " + getTestRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";
            bool isRefrigeratedLoad = false;
            bool getTranistDays = true;            
            JTSSpecialRequirement oSpecial = new JTSSpecialRequirement();
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("{{{0},{1},\"isRefrigeratedLoad\":{2},\"getTranistDays\":{3},\"accessorials\":{{{4}}},\"items\":{5}}}", getTestOrigin(),getTestDestination(), isRefrigeratedLoad.ToString().ToLower(), getTranistDays.ToString().ToLower(), oSpecial.getSpecialRequirementsString(), getTestItems());
            
            string postData = sbPostData.ToString();
            var data = Encoding.ASCII.GetBytes(postData);
            request.Headers.Add("Authorization", "Bearer " + sToken);
            //request.Headers.Add("auth", "Bearer " + sToken);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseJSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string sRet = responseJSON.ToString();
            JTSQuoteResponse oData = Newtonsoft.Json.JsonConvert.DeserializeObject<JTSQuoteResponse>(sRet);

            return oData;
        }


        public JTSQuoteResponse getTestHTTPRateRequestOld(string sToken)
        {
            //var request = (HttpWebRequest)WebRequest.Create("https://test.johansontrans.com/Rates/v2/LTL");
            ////string postData = "{ \"items\": " + getTestItems() + ",\"origin\":" + getTestOrigin() + ",\"destination\":" + getTestDestination() + ",\"shipDate\": \"2022-01-20T20:30:00.0000000Z\", \"customerCode\": \"C377465\",\"declaredValue\": 50000,\"transportModes\": [" + getTestMode("Van", 1, "LTL") + "],\"referenceNumbers\": " + getTestRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";
            //bool isRefrigeratedLoad = false;
            //bool getTranistDays = false;
            //JTSAddress oOrigin = getTestOrigin();
            //JTSAddress oDest = getTestDestination();
            //JTSSpecialRequirements oSpecial = new JTSSpecialRequirements();
            //StringBuilder sbPostData = new StringBuilder();
            //sbPostData.AppendFormat("{{name:\"{0}\",\"request\":{{\"auth\":{{\"type\": \"bearer\",\"bearer\":[{{\"key\":\"token\",\"value\":\"{1}\",\"type\":\"string\"}}]}},", "https://www.johansontrans.com//Rates/v2/LTL", "d711d36b-f01b-40fd-8445-5f0730fe4db6");
            //sbPostData.AppendFormat("\"method\":\"POST\",\"header\":[{{\"key\": \"Content-Type\",\"value\": \"application/json\"}}],");
            //sbPostData.AppendFormat("\"body\":{{\"mode\": \"raw\",\"raw\":\"{{");
            //sbPostData.AppendFormat("\\n\\\"fromCity\\\" : \\\"WILMINGTON\\\",\\n \\\"fromState\\\":\\\"MA\\\",\\n\\\"fromPostalCode\\\": \\\"01887\\\",");
            //sbPostData.AppendFormat("\\n\\\"toCity\\\":\\\"ABERDEEN\\\",\\n\\\"toState\\\":\\\"SD\\\",\\n\\\"toPostalCode\\\":\\\"57402\\\",");
            //sbPostData.AppendFormat("\\n\\\"isRefrigeratedLoad\\\": false,\\n\\\"getTranistDays\\\": false,");
            //string sSpecial = oSpecial.getSpecialRequirements(true);
            //sbPostData.AppendFormat("\\n\\\"accessorials\\\":\\n{{{0}\\n}},", sSpecial);
            ////sbPostData.AppendFormat("\\n\\\"accessorials\\\":\\n{{\\n\\\"accessorialNeedsAppointmentNotification\\": false,\n        \\"accessorialNeedsLiftGateRequired\\": false,\n        \\"accessorialNeedsInsideDelivery\\": false,\n        \\"accessorialNeedsLimitedAccess\\": false,\n        \\"accessorialNeedsResidentialDelivery\\": false,\n        \\"accessorialNeedsConventionOrTradeShowDelivery\\": false,\n        \\"accessorialNeedsHazardousShipment\\": false,\n        \\"accessorialNeedsGuaranteeBy5pm\\": false,\n        \\"accessorialNeedsGuaranteeByNoon\\": false,\n        \\"accessorialNeedsPupRequired\\": false,\n        \\"accessorialNeedsMineDelivery\\": false,\n        \\"accessorialNeedsFreezeProtect\\": false,\n        \\"accessorialExtremeLength8To10Feet\\": false,\n        \\"accessorialExtremeLength11To12Feet\\": false,\n        \\"accessorialExtremeLength13To14Feet\\": false,\n        \\"accessorialExtremeLength15To16Feet\\": false,\n        \\"accessorialExtremeLength17To18Feet\\": false,\n        \\"accessorialExtremeLength19To20Feet\\": false\n    }}
            //string sItems = getTestItems();
            //sbPostData.AppendFormat("\\n\\\"items\\\":[{0}]\"

            //             }},
            //	\"url\": {{
            //                     \"raw\": \"https://test.johansontrans.com/Rates/v2/LTL\",
            //		\"protocol\": \"https\",
            //		\"host\": [

            //                     \"test\",
            //			\"johansontrans\",
            //			\"com\"
            //		],
            //		\"path\": [

            //                     \"Rates\",
            //			\"v2\",
            //			\"LTL\"
            //		]
            //	}}
            //             }},
            //\"response\": []

            //     }},
            //string postData = sbPostData.ToString();
            //var data = Encoding.ASCII.GetBytes(postData);
            //request.Headers.Add("Authorization", "Bearer " + sToken);
            //request.Method = "POST";
            //request.ContentType = "application/json";
            //request.ContentLength = data.Length;
            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}
            //var response = (HttpWebResponse)request.GetResponse();
            //var responseJSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //string sRet = responseJSON.ToString();
            //JTSQuoteResponse oData = Newtonsoft.Json.JsonConvert.DeserializeObject<JTSQuoteResponse>(sRet);

            return null; // oData;
        }

        public JTSQuoteResponse getHTTPRateRequest(string sToken, RateRequest oRequest, string sDataURL)
        {
            var request = (HttpWebRequest)WebRequest.Create(sDataURL);
            string postData = oRequest.postData();
            var data = Encoding.ASCII.GetBytes(postData);
            JTSQuoteResponse oData = new JTSQuoteResponse();
            request.Headers.Add("Authorization", "Bearer " + sToken);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseJSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
                try
                {
                    Stream iData = response.GetResponseStream();
                    using (var reader = new StreamReader(iData))
                    {
                        string sRawMsg = reader.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine(sRawMsg);
                    }
                } catch
                {
                    //do nothing
                }
               
                    string sRet = responseJSON.ToString();
                oData = Newtonsoft.Json.JsonConvert.DeserializeObject<JTSQuoteResponse>(sRet);
                if (oRequest.oFees != null && oData != null)
                {
                    oData.oFees = oRequest.oFees;
                }

            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream eData = response.GetResponseStream())
                    using (var reader = new StreamReader(eData))
                    {
                        oData = new JTSQuoteResponse();
                        oData.postMessagesOnly = true;
                        string sTxtMsg = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(sTxtMsg))
                        {
                            oData.AddMessage(JTSAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid.  Please contact your API Vendor.  The actual Error is: " +  e.Message, "", "");
                        }
                        else
                        {
                            oData.AddMessage(JTSAPI.MessageEnum.E_NoRatesFound, sTxtMsg, "", "");
                        }

                    }
                }
                
            }
            
            return oData;
        }

    }


    public class JTSMessage
    {
        public string Message { get; set; }
        public string MessageLocalCode { get; set; }
        public string VendorMessage { get; set; }
        public string VendorErrorCode { get; set; }
        public string FieldName { get; set; }
        public string Details { get; set; }
        public bool bLogged { get; set; } = false;


    }


    public class JTSQuoteResponse
    {
        public JTSQuoteSummary[] quoteSummaries { get; set; }
        //TODo: map Rates to quote Summaries for JTS
        public JTSQuoteSummary[] rates { get; set; }


        public bool postMessagesOnly { get; set; } = false;
        private List<JTSMessage> messages { get; set; }

        public void AddMessage(JTSAPI.MessageEnum key, string sDetails, string sDefault, string sFieldName)
        {
            JTSMessage msg = new JTSMessage();
            msg.MessageLocalCode = key.ToString();
            msg.Message = JTSAPI.getMessageNotLocalized(key, sDefault);
            msg.Details = sDetails;
            msg.FieldName = sFieldName;
            msg.bLogged = false;
            if (messages == null) { messages = new List<JTSMessage>(); }
            messages.Add(msg);
        }


        public void AddMessage(JTSMessage msg)
        {
            if (messages == null) { messages = new List<JTSMessage>(); }
            msg.bLogged = false;
            messages.Add(msg);
        }

        public List<JTSMessage> GetMessages()
        {
            if (messages == null) { messages = new List<JTSMessage>(); }
            return messages;
        }

        public string concateMessages()
        {
            string sRet = "";
            if (messages != null && messages.Count() > 0)
            {
                foreach (JTSMessage m in messages)
                {
                    sRet += m.Message + ": " + m.Details + " ";
                }
            }

            return sRet;
        }
        /// <summary>
        /// Array of JTSFees used to Maps to BookFees
        /// </summary>
        public JTSFees[] oFees { get; set; }

    }
    public class JTSQuoteSummary
    {

        public string carrierSCAC{ get; set; } // { get; set; } //": "string",
        public string carrierName { get; set; } //": "string",
        public bool isBestMethod { get; set; } //": true,
        public double baseRate { get; set; } //": 0,
        public double accessorialTotal { get; set; } //": 0,
        public double fuelSurcharge { get; set; } //": 0,
        public double totalRate { get; set; } //": 0,
        public string directOrInterline { get; set; } //": "string",
        public string toPostalCode { get; set; } //": "string",
        public int transitDays{ get; set; } //": 0,
        public string quoteId { get; set; } //": "string",
        public bool isFTL { get; set; } //": true
        //public Int64 quoteId { get; set; } //":2159912944
        //public JTSCarrier carrier { get; set; }
        //public JTSCustomer customer { get; set; }
        //public double totalCharge { get; set; } //":599.35
        //public double totalFreightCharge { get; set; } //":499.46
        //public double totalAccessorialCharge { get; set; } //":99.89
        //public JTSTransit transit { get; set; } //":{"minimumTransitDays":1,"maximumTransitDays":2},"
        //public JTSRate[] rates { get; set; } //":[{"rateId":216096619,"totalRate":499.46,"unitRate":499.46,"quantity":1,"rateCode":"400","rateCodeValue":"Line Haul","currencyCode":"USD","isOptional":false},{ "rateId":216096620,"totalRate":99.89,"unitRate":99.89,"quantity":499.46,"rateCode":"405","rateCodeValue":"Fuel Surcharge","currencyCode":"USD","isOptional":false},{ "rateId":216096621,"totalRate":150,"unitRate":150,"quantity":1,"rateCode":"EXL","rateCodeValue":"Excessive Length","currencyCode":"USD","isOptional":true}]
        //public string transportModeType { get; set; } //":"LTL"
        //public string equipmentType { get; set; } //":"Van"
        //public JTSCargoLiability cargoLiability { get; set; }
        //public double distance { get; set; }
        ////
        //public string quoteSource { get; set; } //Contractual"
    }

    public class JTSCargoLiability
    {
        public int perPound { get; set; } //integer Carrier’s liability per pound based on product attributes.

        public int max { get; set; } //integer Carrier’s maximum liability based on product attributes.

        public int amount { get; set; } //integer Calculated liability coverage for specific quote based on product attributes.

        public string currencyCode { get; set; } //string (currencyCodeEnum)
    }

    public class JTSRate
    {
        public double rateId { get; set; } //number<float> ID of the rate that is provided on successfully calling the Rating API.

        public double totalRate { get; set; } //number <float> Currency amount of the total rate

        public double unitRate { get; set; } //number <float> Currency amount of the unit rate

        public double quantity { get; set; } //number <float> Defines how many units there are
        public string rateCode { get; set; }
        public string rateCodeValue { get; set; } //string (rateCodeEnum)
        public string currencyCode { get; set; } //string (currencyCodeEnum)
        public bool isOptional { get; set; } //Defines if the rate is optional. If the isOptional flag is set to false, the currency amount of the assecorial will be included in the totalCharge.
    }
    public class JTSTransit
    {
        public double minimumTransitDays { get; set; } // number<float> Minimum transit days.
        public double maximumTransitDays { get; set; } // number <float> Maximum transit days.
        public string minimumDeliveryDate { get; set; } // string <date-time> An ISO8601 UTC date-time used to indicate the minimum delivery date.
        public string maximumDeliveryDate { get; set; } //string <date-time> An ISO8601 UTC date-time used to indicate the maximum delivery date.
    }

    public class JTSCustomer
    {
        public string customerName { get; set; }
    }

    public class JTSCarrier
    {
        public string carrierName { get; set; }
        public string scac { get; set; }
    }

    public class JTSTokenResponse
    {
        public JTSTokenData tokenresponse { get; set; }

        public string error_code { get; set; }
        public string error_msg { get; set; }
        public bool success { get; set; }
        public IRestResponse response { get; set; }

    }

    public class JTSTokenData
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }

    public class JTSItem
    {
        public int itemInvRecNo { get; set; }
        public string height { get; set; }
        public string itemClass { get; set; }
        public string weight { get; set; }
        public int palletSizeId { get; set; }
        public int palletCount { get; set; }
        //public string description { get; set; } // "widgets",
        //public int freightClass { get; set; } // 400,
        //public int actualWeight { get; set; } // 250,
        //public string weightUnit { get; set; } // "Pounds",
        //public int length { get; set; } // 24,
        //public int width { get; set; } // 10,
        //public int height { get; set; } // 10,
        //public string linearUnit { get; set; } // "Inches",
        //public int pallets { get; set; } // 1,
        //public int pieces { get; set; } // 1,
        //public int palletSpaces { get; set; } // 1,
        //public int volume { get; set; } // 3,
        //public string volumeUnit { get; set; } // "CubicFeet",
        //public int density { get; set; } // 25,
        //public int linearSpace { get; set; } // 8,
        //public int declaredValue { get; set; } // 50000,
        //public string packagingCode { get; set; } // "BIN",
        //public string productCode { get; set; } // "wdgt",
        //public string productName { get; set; } // "widgets",
        //public string temperatureSensitive { get; set; } // "Dry",
        //public string temperatureUnit { get; set; } // "Fahrenheit",
        //public int requiredTemperatureHigh { get; set; } // 85,
        //public int requiredTemperatureLow { get; set; } // 35,
        //public int unitsPerPallet { get; set; } // 36,
        //public int unitWeight { get; set; } // 14,
        //public int unitVolume { get; set; } // 3,
        //public string isStackable { get; set; } // "true",
        //public string isOverWeightOverDimensional { get; set; } // "false",
        //public string isUsedGood { get; set; } // "false",
        //public string isHazardous { get; set; } // "false",
        //public string hazardousDescription { get; set; } // "Car Battery",
        //public string hazardousEmergencyPhone { get; set; } // "5555555555",
        //public string nmfc { get; set; } // "156600",
        //public string upc { get; set; } // "1234567890",
        //public string sku { get; set; } // "01234-001-F10-6",
        //public string plu { get; set; } // "4026",
        //public int pickupSequenceNumber { get; set; }
        //public int dropSequenceNumber { get; set; }
        //public JTSReferenceNumbers[] referenceNumbers { get; set; }

    }

    /// <summary>
    /// Accessorial Fee Mapping to BookFees
    /// </summary>
    public class JTSFees
    {
        public int BookAcssControl { get; set; }
        public int BookAcssNACControl { get; set; }
        public decimal BookAcssValue { get; set; }
        public string NACCode { get; set; }
        public string NACName { get; set; }
        public int AccessorialCode { get; set; }
        public string AccessorialName { get; set; }
      
    }

    public class JTSReferenceNumbers
    {
        public string type { get; set; } //"PO",
        public string value { get; set; } //"PO12345"

        public static void addReferenceNumber(JTSAPI.RefNumbers etype, string sVal, ref List<JTSReferenceNumbers> lref)
        {
            if (lref != null)
            {
                lref = new List<JTSReferenceNumbers>();
            }
            lref.Add(new JTSReferenceNumbers() { type = etype.ToString(), value = sVal });
        }

        public static void addReferenceNumber(JTSAPI.RefNumbers etype, string sVal, ref JTSReferenceNumbers[] referenceNumbers)
        {
            List<JTSReferenceNumbers> lref = new List<JTSReferenceNumbers>();
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref = referenceNumbers.ToList();
            }
            lref.Add(new JTSReferenceNumbers() { type = etype.ToString(), value = sVal });

            referenceNumbers = lref.ToArray();
        }

        public static void addReferenceNumber(JTSReferenceNumbers oRefNbr, ref JTSReferenceNumbers[] referenceNumbers)
        {
            List<JTSReferenceNumbers> lref = new List<JTSReferenceNumbers>();
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref = referenceNumbers.ToList();
            }
            lref.Add(oRefNbr);

            referenceNumbers = lref.ToArray();
        }

        public static void addReferenceNumbers(List<JTSReferenceNumbers> lref, ref JTSReferenceNumbers[] referenceNumbers)
        {
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref.AddRange(referenceNumbers.ToList());
            }

            referenceNumbers = lref.ToArray();
        }

        public static void repklaceReferenceNumbers(List<JTSReferenceNumbers> lref, ref JTSReferenceNumbers[] referenceNumbers)
        {
            referenceNumbers = lref.ToArray();
        }
    }

    public class JTSAddress
    {

        public string locationName { get; set; } // "Origin Location",
        public string address1 { get; set; } // "14800 Charlson Rd",
        public string address2 { get; set; } // "Building 1",
        public string address3 { get; set; } // "Room 212",
        public string city { get; set; } // "Eden Prairie",
        public string stateProvinceCode { get; set; } // "MN",
        public string countryCode { get; set; } // "US",
        public string postalCode { get; set; } // "55347",
        public double latitude { get; set; } //31.717096,
        public double longitude { get; set; } //-99.132553,
        public JTSSpecialRequirement specialRequirements { get; set; }
        public string isPort { get; set; } // "false",
        public string unLocode { get; set; } // "US AC8",
        public string iata { get; set; } // "ACB",
        public string customerLocationId { get; set; } // "W541849",
        public JTSReferenceNumbers[] referenceNumbers { get; set; }



    }

    public class JTSSpecialRequirement
    {
        public bool accessorialNeedsAppointmentNotification { get; set; } = false;
        public bool accessorialNeedsLiftGateRequired { get; set; } = false;
        public bool accessorialNeedsInsideDelivery { get; set; } = false;
        public bool accessorialNeedsLimitedAccess { get; set; } = false;
        public bool accessorialNeedsResidentialDelivery { get; set; } = false;
        public bool accessorialNeedsConventionOrTradeShowDelivery { get; set; } = false;
        public bool accessorialNeedsHazardousShipment { get; set; } = false;
        public bool accessorialNeedsGuaranteeBy5pm { get; set; } = false;
        public bool accessorialNeedsGuaranteeByNoon { get; set; } = false;
        public bool accessorialNeedsPupRequired { get; set; } = false;
        public bool accessorialNeedsMineDelivery { get; set; } = false;
        public bool accessorialNeedsFreezeProtect { get; set; } = false;
        public bool accessorialExtremeLength8To10Feet { get; set; } = false;
        public bool accessorialExtremeLength11To12Feet { get; set; } = false;
        public bool accessorialExtremeLength13To14Feet { get; set; } = false;
        public bool accessorialExtremeLength15To16Feet { get; set; } = false;
        public bool accessorialExtremeLength17To18Feet { get; set; } = false;
        public bool accessorialExtremeLength19To20Feet { get; set; } = false;

        public string getSpecialRequirements(bool bformatRaw)
        {
            StringBuilder sbRet = new StringBuilder();
            string sRawBegin = "";
            string sRawStart = "";
            string sRawEnd = "";
            if (bformatRaw == true)
            {
                sRawBegin = "\\n";
                sRawStart = "\\";
                sRawEnd = "\\";
            }
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsAppointmentNotification{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsAppointmentNotification);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsLiftGateRequired{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsLiftGateRequired);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsInsideDelivery{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsInsideDelivery);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsLimitedAccess{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsLimitedAccess);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsResidentialDelivery{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsResidentialDelivery);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsConventionOrTradeShowDelivery{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsConventionOrTradeShowDelivery);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsHazardousShipment{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsHazardousShipment);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsGuaranteeBy5pm{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsGuaranteeBy5pm);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsGuaranteeByNoon{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsGuaranteeByNoon);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsPupRequired{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsPupRequired);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsMineDelivery{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsMineDelivery);
            sbRet.AppendFormat("{0}{1}\"accessorialNeedsFreezeProtect{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialNeedsFreezeProtect);
            sbRet.AppendFormat("{0}{1}\"accessorialExtremeLength8To10Feet{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialExtremeLength8To10Feet);
            sbRet.AppendFormat("{0}{1}\"accessorialExtremeLength11To12Feet{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialExtremeLength11To12Feet);
            sbRet.AppendFormat("{0}{1}\"accessorialExtremeLength13To14Feet{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialExtremeLength13To14Feet);
            sbRet.AppendFormat("{0}{1}\"accessorialExtremeLength15To16Feet{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialExtremeLength15To16Feet);
            sbRet.AppendFormat("{0}{1}\"accessorialExtremeLength17To18Feet{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialExtremeLength17To18Feet); ;
            sbRet.AppendFormat("{0}{1}\"accessorialExtremeLength19To20Feet{1}\":{3},", sRawBegin, sRawStart, sRawEnd, this.accessorialExtremeLength19To20Feet);



            //},\n    \\"items\\": [\n        {{\n            \\"itemInvRecNo\\": 1089,\n            \\"height\\": \\"70\\",\n            \\"itemClass\\": \\"70\\",\n            \\"weight\\": \\"2000\\",\n            \\"palletSizeId\\": 0,\n            \\"palletCount\\": 0\n        }}\n    ]\n}}\"


            return sbRet.ToString();
        }

        public string getSpecialRequirementsString()
        {
            StringBuilder sbRet = new StringBuilder();
            
            sbRet.AppendFormat("\"accessorialNeedsAppointmentNotification\":{0},", this.accessorialNeedsAppointmentNotification.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsLiftGateRequired\":{0},", this.accessorialNeedsLiftGateRequired.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsInsideDelivery\":{0},", this.accessorialNeedsInsideDelivery.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsLimitedAccess\":{0},", this.accessorialNeedsLimitedAccess.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsResidentialDelivery\":{0},", this.accessorialNeedsResidentialDelivery.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsConventionOrTradeShowDelivery\":{0},", this.accessorialNeedsConventionOrTradeShowDelivery.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsHazardousShipment\":{0},", this.accessorialNeedsHazardousShipment.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsGuaranteeBy5pm\":{0},", this.accessorialNeedsGuaranteeBy5pm.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsGuaranteeByNoon\":{0},", this.accessorialNeedsGuaranteeByNoon.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsPupRequired\":{0},", this.accessorialNeedsPupRequired.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsMineDelivery\":{0},", this.accessorialNeedsMineDelivery.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialNeedsFreezeProtect\":{0},", this.accessorialNeedsFreezeProtect.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialExtremeLength8To10Feet\":{0},", this.accessorialExtremeLength8To10Feet.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialExtremeLength11To12Feet\":{0},", this.accessorialExtremeLength11To12Feet.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialExtremeLength13To14Feet\":{0},", this.accessorialExtremeLength13To14Feet.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialExtremeLength15To16Feet\":{0},", this.accessorialExtremeLength15To16Feet.ToString().ToLower());
            sbRet.AppendFormat("\"accessorialExtremeLength17To18Feet\":{0},", this.accessorialExtremeLength17To18Feet.ToString().ToLower()); 
            sbRet.AppendFormat("\"accessorialExtremeLength19To20Feet\":{0}", this.accessorialExtremeLength19To20Feet.ToString().ToLower());





            return sbRet.ToString();
        }



        public void setSpecialRequrement(JTSAPI.SpecialRequirement eReq)
    {
            switch (eReq)
            {
                case JTSAPI.SpecialRequirement.accessorialNeedsAppointmentNotification:
                    this.accessorialNeedsAppointmentNotification = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsLiftGateRequired:
                    this.accessorialNeedsLiftGateRequired = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsInsideDelivery:
                    this.accessorialNeedsInsideDelivery = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsLimitedAccess:
                    this.accessorialNeedsLimitedAccess = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsResidentialDelivery:
                    this.accessorialNeedsResidentialDelivery = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsConventionOrTradeShowDelivery:
                    this.accessorialNeedsConventionOrTradeShowDelivery = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsHazardousShipment:
                    this.accessorialNeedsHazardousShipment = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsGuaranteeBy5pm:
                    this.accessorialNeedsGuaranteeBy5pm = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsGuaranteeByNoon:
                    this.accessorialNeedsGuaranteeByNoon = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsPupRequired:
                    this.accessorialNeedsPupRequired = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsMineDelivery:
                    this.accessorialNeedsMineDelivery = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialNeedsFreezeProtect:
                    this.accessorialNeedsFreezeProtect = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialExtremeLength8To10Feet:
                    this.accessorialExtremeLength8To10Feet = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialExtremeLength11To12Feet:
                    this.accessorialExtremeLength11To12Feet = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialExtremeLength13To14Feet:
                    this.accessorialExtremeLength13To14Feet = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialExtremeLength15To16Feet:
                    this.accessorialExtremeLength15To16Feet = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialExtremeLength17To18Feet:
                    this.accessorialExtremeLength17To18Feet = true;
                    break;
                case JTSAPI.SpecialRequirement.accessorialExtremeLength19To20Feet:
                    this.accessorialExtremeLength19To20Feet = true;
                    break;
                default:                    
                    break;
            }


        }
    }


    public class JTSTransportMode
    {
        public string mode { get; set; } //"LTL",
        public JTSEquipment[] equipments { get; set; }

    }

    public class JTSEquipment
    {
        public string equipmentType { get; set; } //"Van",
        public int quantity { get; set; } //1
    }


    /// <summary>
    /// Configure the Load to be shipped.  then call JTSAPI.getHTTPRateRequest with a valid token and a RateRequest Object
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.1.002 on 01/31/2022
    ///     Instructions:
    ///     populate the following properties
    ///     oAccessorials
    ///     sSpecial
    ///     oOrigin
    ///     lStops (one stop for now)
    ///     lItems
    ///     customerCode
    ///     declaredValue (value of items)
    ///     oRefs
    ///  Methods
    ///     use setShipDate to populate sShipDate
    ///     use setMode to populate oMode
    ///     
    /// </remarks>
    public class RateRequest
    {

        public JTSTransportMode tMode { get; set; }
        public JTSSpecialRequirement oSpecial { get; set; } = new JTSSpecialRequirement();
        public bool isRefrigeratedLoad { get; set; } = false;
        public bool getTranistDays { get; set; } = true;


        //note: caller must populate  JTSSpecialRequirement oOrigSpecialReq = new JTSSpecialRequirement()
        //        {
        //            liftGate = "false",
        //            insidePickup = "false",
        //            insideDelivery = "false",
        //            residentialNonCommercial = "false",
        //            limitedAccess = "false",
        //            tradeShoworConvention = "false",
        //            constructionSite = "false",
        //            dropOffAtCarrierTerminal = "false",
        //            pickupAtCarrierTerminal = "false"
        //        };
        //JTSReferenceNumbers[] oOrigRefs = new JTSReferenceNumbers[1];
        //oOrigRefs[0] = new JTSReferenceNumbers { type = "PU", value = "PickUpNumber1" };
        public JTSAddress oOrigin { get; set; }

        public List<JTSAddress> lStops { get; set; }

        //note: caller must populate  Item JTSReferenceNumbers[] oRefs 
        // example oRefs[0] = new JTSReferenceNumbers { type = "PO", value = "PO12345" };
        public List<JTSItem> lItems { get; set; }

        public string sShipDate { get; set; }
        private DateTime? dtNGLShipDate { get; set; }
        public void setShipDate(DateTime dShipDate)
        {
            dtNGLShipDate = dShipDate;
            sShipDate = convertDatetoUTCWebFormat(dShipDate);

        }

        public DateTime? getNGLShipDate()
        {
            return dtNGLShipDate;
        }

        public string customerCode { get; set; }  //\": \"C377465\",\"
        public double declaredValue { get; set; } //\": 50000

        public JTSTransportMode oMode { get; set; }

        public JTSReferenceNumbers[] oRefs { get; set; }

        /// <summary>
        /// String Array of NAC Accessorial Codes Used by TMS mapping
        /// </summary>
        public string[] oAccessorials { get; set; }
        /// <summary>
        /// Array of JTSFees used to Maps to BookFees
        /// </summary>
        public JTSFees[] oFees { get; set; }


        public string postData()
        {
            if (oSpecial == null)
            {
                oSpecial = new JTSSpecialRequirement();
            }
            string sReturn =  string.Format("{{{0},{1},\"isRefrigeratedLoad\":{2},\"getTranistDays\":{3},\"accessorials\":{{{4}}},\"items\":{5}}}", getOrigin(), getDestination(), isRefrigeratedLoad.ToString().ToLower(), getTranistDays.ToString().ToLower(), oSpecial.getSpecialRequirementsString(), getItems());

            //string sReturn = "{ \"items\": " + getItems() + ",\"origin\":" + getOrigin() + ",\"destination\":" + getDestination() + ",\"shipDate\": \"" + sShipDate + "\", \"customerCode\": \"" + customerCode + "\",\"declaredValue\": " + declaredValue.ToString() + ",\"transportModes\": [" + getMode() + "],\"referenceNumbers\": " + getRateReferenceNumbers() + ", \"optionalAccessorials\": " + getAccessorials() + "}";
            return sReturn;
        }

        public string convertDatetoUTCWebFormat(DateTime dtVal)
        {
            return string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", dtVal.ToUniversalTime());
        }

        public string getItems()
        {
            string jItems = JsonConvert.SerializeObject(lItems.ToArray());
            return jItems;
        }


        public string getOrigin()
        {

            string sRet = string.Format("\"fromCity\":\"{0}\",\"fromState\":\"{1}\",\"fromPostalCode\":\"{2}\"", oOrigin.city, oOrigin.stateProvinceCode, oOrigin.postalCode);
            return sRet;
            //string jOrigin = JsonConvert.SerializeObject(oOrigin);
            //return jOrigin;
        }

        public string getDestination()
        {
            // for now we only allow single stop
            JTSAddress oDest = lStops[0];
            string sRet = string.Format("\"toCity\":\"{0}\",\"toState\":\"{1}\",\"toPostalCode\":\"{2}\"", oDest.city, oDest.stateProvinceCode, oDest.postalCode);

            //string sRet = string.Format("\"fromCity\":\"{0}\",\"fromState\":\"{1}\",\"fromPostalCode\":\"{2}\"", oDest.city, oDest.stateProvinceCode, oDest.postalCode);
            return sRet;

            //string jDest = JsonConvert.SerializeObject(oDest);
            //return jDest;
        }

        public void setMode(JTSAPI.EquipType eEquipType, int iEquipQty, JTSAPI.TransMode eMode)
        {
            JTSEquipment[] oEquipments = new JTSEquipment[1];
            oEquipments[0] = new JTSEquipment()
            {
                equipmentType = eEquipType.ToString(),
                quantity = iEquipQty
            };

            oMode = new JTSTransportMode()
            {
                mode = eMode.ToString(),
                equipments = oEquipments
            };
        }

        public void setMode(string sEquipType, int iEquipQty, string  sMode)
        {
            JTSEquipment[] oEquipments = new JTSEquipment[1];
            oEquipments[0] = new JTSEquipment()
            {
                equipmentType = sEquipType,
                quantity = iEquipQty
            };

            oMode = new JTSTransportMode()
            {
                mode = sMode,
                equipments = oEquipments
            };
        }

        public string getMode()
        {
            string jMode = JsonConvert.SerializeObject(oMode);
            return jMode;
        }



        public string getRateReferenceNumbers()

        {
            string jRefs = "[{}]";
            if (oRefs != null && oRefs.Count() > 0)
            {
                jRefs = JsonConvert.SerializeObject(oRefs);
            }

            //JTSReferenceNumbers[] oRefs = new JTSReferenceNumbers[1];
            //oRefs[0] = new JTSReferenceNumbers { type = "MBOL", value = "MBOL12345" };

            return jRefs;
        }

        public string getAccessorials()

        {
            string jRefs = "[]";
            if (oAccessorials != null && oAccessorials.Count() > 0)
            {
                jRefs = JsonConvert.SerializeObject(oAccessorials);
            }

            //JTSReferenceNumbers[] oRefs = new JTSReferenceNumbers[1];
            //oRefs[0] = new JTSReferenceNumbers { type = "MBOL", value = "MBOL12345" };

            return jRefs;
        }




        private bool postMessagesOnly { get; set; } = false;
        private List<JTSMessage> messages { get; set; }

        public bool getPostMessageOnlyFlag()
        {
            return postMessagesOnly;
        }

        public void setPostMessageOnlyFlag(bool bval)
        {
            postMessagesOnly = bval;
        }

        public void AddMessage(JTSAPI.MessageEnum key, string sDetails, string sDefault, string sFieldName)
        {
            JTSMessage msg = new JTSMessage();
            msg.MessageLocalCode = key.ToString();
            msg.Message = JTSAPI.getMessageNotLocalized(key, sDefault);
            msg.Details = sDetails;
            msg.FieldName = sFieldName;
            msg.bLogged = false;
            if (messages == null) { messages = new List<JTSMessage>(); }
            messages.Add(msg);
        }

        public List<JTSMessage> GetMessages()
        {
            if (messages == null) { messages = new List<JTSMessage>(); }
            return messages;
        }
    }

}
