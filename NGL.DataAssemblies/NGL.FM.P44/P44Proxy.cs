using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
//using RestSharp;
using System.Web;
using System.IO;
using P44M = P44SDK.V4.Model;


namespace NGL.FM.P44
{
    public class P44Proxy
    {
       
        public string P44WebServiceUrl  {  get; set;  }

        public string P44WebServiceLogin { get; set; }

        public string P44WebServicePassword { get; set; }

        public P44Proxy(string sP44WebServiceUrl,string sP44WebServiceLogin,string sP44WebServicePassword, bool bUseTLs12 = true)
        {
            if (bUseTLs12)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            }
            P44WebServiceUrl = sP44WebServiceUrl;

            P44WebServiceLogin = sP44WebServiceLogin;

            P44WebServicePassword = sP44WebServicePassword;
        }


        public static string getMessageNotLocalized(NGL.FM.P44.MessageEnum key, string sDefault)
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
                case MessageEnum.E_InvalidAddressInfo:
                    sRet = "Invalid Address Info";
                    break;
                case MessageEnum.E_InvalidPackageInfo:
                    sRet = "Invalid Package Info";
                    break;
                default:
                    break;
            }
            return sRet;
        }



        /// <summary>
        /// Method that calls the P44 API to get the Rate Quotes
        /// Entry point for all methods trying to get a Rate Quote
        /// Calls either P44 API v1 (XML) or P44 API v4 (REST) to do 
        /// the quoting based on the parameter intVersion
        /// The caller should look up the value of the Global Parameter
        /// APIRateQuoteVersion to get the version, otherwise the default
        /// is to use v1 (because we know it is stable - v4 in test still as of 2/15/19)
        /// </summary>
        /// <param name="rateQuoteRequest"></param>
        /// <param name="intVersion"></param>
        /// <returns></returns>
        public List<rateQuoteResponse> GetRateQuotes(RateRequest rateQuoteRequest, int intVersion = 1)
        {
            List<rateQuoteResponse> P44RetErrors = new List<rateQuoteResponse>();
            rateQuoteResponse P44RetMsg = new rateQuoteResponse()
            {
                mode = translateMode("LTL"), //lvv - currently we only do LTL
                scac = "TEMP",
                vendor = "TEMP"
            };
            try
            {
                if (intVersion == 1) //use API v1 (XML) else use API v4 (REST)
                {
                    string xml = buildXMLOrderWStream(rateQuoteRequest);
                    return restServicesXML(xml);
                }
                else
                {
                    return buildP44RateRequest(rateQuoteRequest);
                }
            }
            catch (System.ApplicationException aex)
            {
                if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                if (P44RetErrors.Count() > 0)
                {
                    P44RetErrors[0].postMessagesOnly = true;
                    P44RetErrors[0].AddMessage(MessageEnum.E_UnExpected, aex.Message, "An Unexpected Error Has Occurred!", "unknown");
                }
                else
                {
                    P44RetMsg.postMessagesOnly = true;
                    P44RetMsg.AddMessage(MessageEnum.E_UnExpected, aex.Message, "An Unexpected Error Has Occurred!", "unknown");
                    P44RetErrors.Add(P44RetMsg);
                }
            }
            catch (P44SDK.V4.Client.ApiException pex)
            {
                string sMsg = " Rate Quote Request Error: " + pex.Message;
                if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                if (P44RetErrors.Count() > 0)
                {
                    P44RetErrors[0].postMessagesOnly = true;
                    P44RetErrors[0].AddMessage(MessageEnum.E_UnExpected, sMsg, "An Unexpected Error Has Occurred!", "unknown");
                }
                else
                {
                    P44RetMsg.postMessagesOnly = true;
                    P44RetMsg.AddMessage(MessageEnum.E_UnExpected, sMsg, "An Unexpected Error Has Occurred!", "unknown");
                    P44RetErrors.Add(P44RetMsg);
                }               
                
            }
            catch (Exception ex)
            {
                string sMsg = " System Error: " + ex.Message;
                if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                if (P44RetErrors.Count() > 0)
                {
                    P44RetErrors[0].postMessagesOnly = true;
                    P44RetErrors[0].AddMessage(MessageEnum.E_UnExpected, sMsg, "An Unexpected Error Has Occurred!", "unknown");
                }
                else
                {
                    P44RetMsg.postMessagesOnly = true;
                    P44RetMsg.AddMessage(MessageEnum.E_UnExpected, sMsg, "An Unexpected Error Has Occurred!", "unknown");
                    P44RetErrors.Add(P44RetMsg);
                }
            }
            // if we get here we have errors
            return P44RetErrors;


        }

        #region "P44 XML (v1)"

        /* Removed GetRateQuotesXML by RHR on 1/31/2017 because RestSharp is not signed and we are not using it */

        //public RateQuotes GetRateQuotesXML(string xml)
        //{
        //    //var encodedxml = System.Web.HttpUtility.UrlEncode(xml);
        //    var client = new RestClient(P44WebServiceUrl);
        //    //var request = new RestRequest("/xml/multirate/quote.xml", Method.POST);
        //    //var request = new RestRequest("/xml/xmlrate/quote.xml", Method.POST);
        //    var request = new RestRequest("/xml/xmlrate/quote.xml", Method.GET);
        //    //string sBody = string.Format("p44Login={0}&p44Password={1}]&xml={2}", System.Web.HttpUtility.UrlEncode(P44WebServiceLogin), System.Web.HttpUtility.UrlEncode(P44WebServicePassword), encodedxml);
        //    request.AddParameter("p44Login", P44WebServiceLogin, ParameterType.QueryString);
        //    request.AddParameter("p44Password", P44WebServicePassword, ParameterType.QueryString);
        //    //request.AddParameter("p44Login", System.Web.HttpUtility.UrlEncode(P44WebServiceLogin), ParameterType.RequestBody);
        //    //request.AddParameter("p44Password", System.Web.HttpUtility.UrlEncode(P44WebServicePassword), ParameterType.RequestBody);
        //    request.AddParameter("xml", xml, ParameterType.QueryString);
        //    //request.AddParameter("xml", System.Web.HttpUtility.UrlEncode(xml), ParameterType.RequestBody);
        //    //request.AddParameter("text/xml", xml, ParameterType.RequestBody);
        //    //request.AddBody(sBody); //, "http://www.p-44.com/xml");
        //    //IRestResponse<RateQuotes> oResponse = client.Execute<RateQuotes>(request);
        //    var response = client.Execute<RateQuotes>(request);
        //    var oRData1 = response.Data;
        //  return null;
        //}

        public void writeNodeIfNotEmpty(ref XmlWriter writer, string node, string val)
        {
            if (!string.IsNullOrWhiteSpace(val)) { writer.WriteElementString(node, val); }
        }

        public void writeNodeIfNotEmpty(ref XmlWriter writer, string node, string val, string sdefault)
        {
            if (string.IsNullOrWhiteSpace(sdefault))
            {
                writeNodeIfNotEmpty(ref writer, node, val);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(val))
                {
                    writer.WriteElementString(node, val);
                }
                else
                {
                    writer.WriteElementString(node, sdefault);
                }
            }

        }

        /// <summary>
        /// One
        /// </summary>
        /// <param name="oData"></param>
        /// <returns></returns>
        public string buildXMLOrderWStream(RateRequest oData)
        {
            System.Text.StringBuilder xml = new StringBuilder();
            string sXML = "";
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.CloseOutput = false;
            //MemoryStream strm = new MemoryStream();
            StringWriter strm = new StringWriter();
            XmlWriter writer = XmlWriter.Create(strm, settings);
            writer.WriteStartElement("RateRequest");

            writer.WriteElementString("timeout", oData.timeout.ToString());
            writer.WriteElementString("deliveryDate", oData.deliveryDate,DateTime.Now.AddDays(3).ToShortDateString());
            writer.WriteElementString("shipDate",  oData.shipDate, DateTime.Now.AddDays(1).ToShortDateString());
            writer.WriteElementString("fetchAllGuaranteed", "true");
            writer.WriteElementString("returnMultiple", "true");
            if (string.IsNullOrWhiteSpace(oData.loginGroupKey))
            {
                writer.WriteElementString("loginGroupKey", "NGLOPS");
            } else
            {
                writer.WriteElementString("loginGroupKey", oData.loginGroupKey);
            }            
            writer.WriteStartElement("destination");
            writeNodeIfNotEmpty(ref writer, "address1", oData.destination.address1);
            writeNodeIfNotEmpty(ref writer, "companyName", oData.destination.companyName);
            writeNodeIfNotEmpty(ref writer, "city", oData.destination.city);
            writeNodeIfNotEmpty(ref writer, "stateName", oData.destination.stateName);
            writeNodeIfNotEmpty(ref writer, "country", oData.destination.country, "US");
            writeNodeIfNotEmpty(ref writer, "postalCode", oData.destination.postalCode);
            writer.WriteEndElement();
            writer.WriteStartElement("origin");
            writeNodeIfNotEmpty(ref writer, "address1", oData.origin.address1);
            writeNodeIfNotEmpty(ref writer, "companyName", oData.origin.companyName);
            writeNodeIfNotEmpty(ref writer, "city", oData.origin.city);
            writeNodeIfNotEmpty(ref writer, "stateName", oData.origin.stateName);
            writeNodeIfNotEmpty(ref writer, "country", oData.origin.country, "US");
            writeNodeIfNotEmpty(ref writer, "postalCode", oData.origin.postalCode);
            writer.WriteEndElement();

            //xml.Append("<accountNumbers><entry><key>SCAC1</key><value>SEFL</value></entry><entry><key>SCAC2</key><value>AACT</value></entry></accountNumbers>");
            //xml.Append("<accountConfigs>");
            //xml.Append("<account><vendorCode>SCAC1</vendorCode><siteLoginName>DEMO</siteLoginName><credentials>DEMO</credentials></account>");
            //xml.Append("<account><vendorCode>SCAC2</vendorCode><credentials>7e2c61c4-8b4c-4d8b-b47f-ed033c6f4307</credentials><paymentType>P</paymentType><direction>S</direction></account>");
            //xml.Append("</accountConfigs>");

            if (oData.lineItems != null && oData.lineItems.Length > 0)
            {
                writer.WriteStartElement("items");
                foreach (rateQuoteLineImpl i in oData.lineItems)
                {
                    writer.WriteStartElement("item");
                    //writeNodeIfNotEmpty(ref writer, "code", i.code);
                    //writeNodeIfNotEmpty(ref writer, "quantity", i.quantity);
                    writeNodeIfNotEmpty(ref writer, "weight", i.weight.ToString());
                    //writeNodeIfNotEmpty(ref writer, "hazmatId", i.hazmatId);
                    //writeNodeIfNotEmpty(ref writer, "shippingName", i.shippingName);
                    //writeNodeIfNotEmpty(ref writer, "hazmatClass", i.hazmatClass);
                    //writeNodeIfNotEmpty(ref writer, "packagingGroup", i.packagingGroup);
                    writeNodeIfNotEmpty(ref writer, "weightUnit", i.weightUnit, "lbs");
                    writeNodeIfNotEmpty(ref writer, "freightClass", i.freightClass, "70");
                    writeNodeIfNotEmpty(ref writer, "description", i.description);
                    //writeNodeIfNotEmpty(ref writer, "hazmat", i.hazmat.ToString());
                    writeNodeIfNotEmpty(ref writer, "pieces", i.numPieces.ToString());
                    writeNodeIfNotEmpty(ref writer, "packageType", i.packageType);
                    writeNodeIfNotEmpty(ref writer, "length", i.length.ToString());
                    writeNodeIfNotEmpty(ref writer, "width", i.width.ToString());
                    writeNodeIfNotEmpty(ref writer, "height", i.height.ToString());
                    //writeNodeIfNotEmpty(ref writer, "density", i.density.ToString());
                    //writeNodeIfNotEmpty(ref writer, "nmfcItem", i.nmfcItem);
                    //writeNodeIfNotEmpty(ref writer, "nmfcSub", i.nmfcSub.ToString());
                    //writeNodeIfNotEmpty(ref writer, "stackable", i.stackable);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            if (oData.accessorials != null && oData.accessorials.Length > 0)
            {
                writer.WriteStartElement("accessorials");
                foreach (string s in oData.accessorials)
                {
                    if( s != "FSC" )
                    {
                        writeNodeIfNotEmpty(ref writer, "code", s);
                    }
                    
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

            sXML = strm.ToString();
            return sXML;
            //return xml.ToString();

        }

        public List<rateQuoteResponse> restServicesXML(string xml)
        {

            string endPoint = string.Format("{0}/xml/xmlrate/quote.xml?p44Login={1}&p44Password={2}&xml={3}", P44WebServiceUrl, System.Web.HttpUtility.UrlEncode(P44WebServiceLogin), System.Web.HttpUtility.UrlEncode(P44WebServicePassword), System.Web.HttpUtility.UrlEncode(xml));
            //string endPoint = string.Format("http://test.p-44.com/xml/xmlrate/quote.xml?p44Login={0}&p44Password={1}&xml={2}", System.Web.HttpUtility.UrlEncode("rramsey@nextgeneration.com"), System.Web.HttpUtility.UrlEncode("NGL2016!"), System.Web.HttpUtility.UrlEncode(xml));
            //string postData = xml;
            var client = new P44RestClient(endpoint: endPoint,
                            method: HttpVerb.GET);

            //var client = new P44RestClient(endpoint: endPoint,
            //                method: HttpVerb.POST,
            //                postData: "{'xml': '" + xml + "'}");
            XDocument xDoc = client.getXDocRESTResponse("");
            return ProcessResponse(xDoc);
            
        }

        public List<rateQuoteResponse> ProcessResponse(XDocument xDoc)
        {
            XElement root = xDoc.Root;

            //List<rateQuoteResponse> tinfo = (from e in xDoc.Descendants("rateQuote")
            //                                    select new rateQuoteResponse()
            //                                    {
            //                                        mode = e.Element("mode") != null ? (string)e.Element("mode").Value : "",
            //                                        rateDetail =
            //                                                 new rateDetail()
            //                                                 {
            //                                                     total = e.Element("rateDetail").Element("total") != null ? decimal.Parse((string)e.Element("rateDetail").Element("total").Value) : 0

            //                                                 }

            //                                    }).ToList();

            //List<rateQuoteResponse> tstResults = (from e in xDoc.Descendants("rateQuote")
            //                                         select new rateQuoteResponse() {
            //                                             mode = e.Element("mode") != null ? (string)e.Element("mode").Value : "",
            //                                             accessorialsNotPriced = e.Descendants("accessorialsNotPriced").Elements("code") != null ?
            //                                                  (from aNotPriced in e.Descendants("accessorialsNotPriced").Elements("code")
            //                                                   select aNotPriced.Value).ToArray()
            //                                                  : null
            //                                         }

            ////                           ).ToList();

            //Console.WriteLine(tstResults.Count());

            List<rateQuoteResponse> results = (from e in xDoc.Descendants("rateQuote")
                                                  select new rateQuoteResponse()
                                                  {
                                                      mode = translateMode( e.Element("mode") != null ? (string)e.Element("mode").Value : ""),
                                                      scac = e.Element("scac") != null ? (string)e.Element("scac").Value : "",
                                                      vendor = e.Element("vendor") != null ? (string)e.Element("vendor").Value : "",
                                                      interLine = e.Element("interLine") != null ? (string)e.Element("interLine").Value : "",
                                                      quoteNumber = e.Element("quoteNumber") != null ? (string)e.Element("quoteNumber").Value : "",
                                                      carrierNote = e.Element("carrierNote") != null ? (string)e.Element("carrierNote").Value : "",
                                                      deliveryDate = e.Element("deliveryDate") != null ? (string)e.Element("deliveryDate").Value : "",
                                                      expirationDate = e.Element("expirationDate") != null ? (string)e.Element("expirationDate").Value : "",
                                                      quoteDate = e.Element("quoteDate") != null ? (string)e.Element("quoteDate").Value : DateTime.Now.ToShortDateString(),
                                                      serviceType = e.Element("serviceType") != null ? (string)e.Element("serviceType").Value : "",
                                                      totalPallets = e.Element("totalPallets") != null ? int.Parse(e.Element("totalPallets").Value) : 0,
                                                      totalPieces = e.Element("totalPieces") != null ? int.Parse(e.Element("totalPieces").Value) : 0,
                                                      totalWeight = e.Element("totalWeight") != null ? int.Parse(e.Element("totalWeight").Value) : 0,
                                                      transitTime = e.Element("transitTime") != null ? int.Parse(e.Element("transitTime").Value) : 0,
                                                      originTerminal =
                                                        e.Element("originTerminal") != null ?
                                                        new terminalInfo()
                                                        {
                                                            address1 = e.Element("originTerminal").Element("address1") != null ? (string)e.Element("originTerminal").Element("address1").Value : "",
                                                            address2 = e.Element("originTerminal").Element("address2") != null ? (string)e.Element("originTerminal").Element("address2").Value : "",
                                                            address3 = e.Element("originTerminal").Element("address3") != null ? (string)e.Element("originTerminal").Element("address3").Value : "",
                                                            companyName = e.Element("originTerminal").Element("companyName") != null ? (string)e.Element("originTerminal").Element("companyName").Value : "",
                                                            contactName = e.Element("originTerminal").Element("contactName") != null ? (string)e.Element("originTerminal").Element("contactName").Value : "",
                                                            city = e.Element("originTerminal").Element("city") != null ? (string)e.Element("originTerminal").Element("city").Value : "",
                                                            stateName = e.Element("originTerminal").Element("stateName") != null ? (string)e.Element("originTerminal").Element("stateName").Value : "",
                                                            country = e.Element("originTerminal").Element("country") != null ? (string)e.Element("originTerminal").Element("country").Value : "",
                                                            postalCode = e.Element("originTerminal").Element("postalCode") != null ? (string)e.Element("originTerminal").Element("postalCode").Value : "",
                                                            phoneNumber = e.Element("originTerminal").Element("phoneNumber") != null ? (string)e.Element("originTerminal").Element("phoneNumber").Value : "",
                                                            phoneNumber2 = e.Element("originTerminal").Element("phoneNumber2") != null ? (string)e.Element("originTerminal").Element("phoneNumber2").Value : "",
                                                            email = e.Element("originTerminal").Element("email") != null ? (string)e.Element("originTerminal").Element("email").Value : "",
                                                            faxNumber = e.Element("originTerminal").Element("faxNumber") != null ? (string)e.Element("originTerminal").Element("faxNumber").Value : "",
                                                            terminalId = e.Element("originTerminal").Element("terminalId") != null ? (string)e.Element("originTerminal").Element("terminalId").Value : "",
                                                            terminalName = e.Element("originTerminal").Element("terminalName") != null ? (string)e.Element("originTerminal").Element("terminalName").Value : "",
                                                            terminalNumber = e.Element("originTerminal").Element("terminalNumber") != null ? (string)e.Element("originTerminal").Element("terminalNumber").Value : "",
                                                            terminalCarrier = e.Element("originTerminal").Element("terminalCarrier") != null ? (string)e.Element("originTerminal").Element("terminalCarrier").Value : ""
                                                        } : null,
                                                      destinationTerminal =
                                                      e.Element("destinationTerminal") != null ?
                                                      new terminalInfo()
                                                      {
                                                          address1 = e.Element("destinationTerminal").Element("address1") != null ? (string)e.Element("destinationTerminal").Element("address1").Value : "",
                                                          address2 = e.Element("destinationTerminal").Element("address2") != null ? (string)e.Element("destinationTerminal").Element("address2").Value : "",
                                                          address3 = e.Element("destinationTerminal").Element("address3") != null ? (string)e.Element("destinationTerminal").Element("address3").Value : "",
                                                          companyName = e.Element("destinationTerminal").Element("companyName") != null ? (string)e.Element("destinationTerminal").Element("companyName").Value : "",
                                                          contactName = e.Element("destinationTerminal").Element("contactName") != null ? (string)e.Element("destinationTerminal").Element("contactName").Value : "",
                                                          city = e.Element("destinationTerminal").Element("city") != null ? (string)e.Element("destinationTerminal").Element("city").Value : "",
                                                          stateName = e.Element("destinationTerminal").Element("stateName") != null ? (string)e.Element("destinationTerminal").Element("stateName").Value : "",
                                                          country = e.Element("destinationTerminal").Element("country") != null ? (string)e.Element("destinationTerminal").Element("country").Value : "",
                                                          postalCode = e.Element("destinationTerminal").Element("postalCode") != null ? (string)e.Element("destinationTerminal").Element("postalCode").Value : "",
                                                          phoneNumber = e.Element("destinationTerminal").Element("phoneNumber") != null ? (string)e.Element("destinationTerminal").Element("phoneNumber").Value : "",
                                                          phoneNumber2 = e.Element("destinationTerminal").Element("phoneNumber2") != null ? (string)e.Element("destinationTerminal").Element("phoneNumber2").Value : "",
                                                          email = e.Element("destinationTerminal").Element("email") != null ? (string)e.Element("destinationTerminal").Element("email").Value : "",
                                                          faxNumber = e.Element("destinationTerminal").Element("faxNumber") != null ? (string)e.Element("destinationTerminal").Element("faxNumber").Value : "",
                                                          terminalId = e.Element("destinationTerminal").Element("terminalId") != null ? (string)e.Element("destinationTerminal").Element("terminalId").Value : "",
                                                          terminalName = e.Element("destinationTerminal").Element("terminalName") != null ? (string)e.Element("destinationTerminal").Element("terminalName").Value : "",
                                                          terminalNumber = e.Element("destinationTerminal").Element("terminalNumber") != null ? (string)e.Element("destinationTerminal").Element("terminalNumber").Value : "",
                                                          terminalCarrier = e.Element("destinationTerminal").Element("terminalCarrier") != null ? (string)e.Element("destinationTerminal").Element("terminalCarrier").Value : ""
                                                      } : null,
                                                      rateDetail =
                                                      e.Element("rateDetail") != null ?
                                                      new rateDetail()
                                                      {
                                                          total = e.Element("rateDetail").Element("total") != null ? decimal.Parse((string)e.Element("rateDetail").Element("total").Value) : 0,

                                                          rateAdjustments = e.Element("rateDetail").Descendants("rateAdjustment") != null ?
                                                          (from a in e.Element("rateDetail").Descendants("rateAdjustment")
                                                           select new rateAdjustment()
                                                           {
                                                               freightClass = a.Element("freightClass") != null ? (string)a.Element("freightClass").Value : "",
                                                               weight = a.Element("weight") != null ? int.Parse((string)a.Element("weight").Value) : 0,
                                                               pieces = a.Element("pieces") != null ? int.Parse((string)a.Element("pieces").Value) : 0,
                                                               description = a.Element("description") != null ? (string)a.Element("description").Value : "",
                                                               descriptionCode = a.Element("descriptionCode") != null ? (string)a.Element("descriptionCode").Value : "",
                                                               amount = a.Element("amount") != null ? decimal.Parse((string)a.Element("amount").Value) : 0,
                                                               rate = a.Element("rate") != null ? decimal.Parse((string)a.Element("rate").Value) : 0
                                                           }).ToArray()
                                                          : null
                                                      }
                                                      : null,
                                                      errors = e.Descendants("error") != null ?
                                                          (from svsErr in e.Descendants("error")
                                                           select new ServiceError()
                                                           {
                                                               errorCode = svsErr.Element("errorCode") != null ? (string)svsErr.Element("errorCode").Value : "",
                                                               errorMessage = svsErr.Element("errorMessage") != null ? (string)svsErr.Element("errorMessage").Value : "",
                                                               vendorErrorCode = svsErr.Element("vendorErrorCode") != null ? (string)svsErr.Element("vendorErrorCode").Value : "",
                                                               vendorErrorMessage = svsErr.Element("vendorErrorMessage") != null ? (string)svsErr.Element("vendorErrorMessage").Value : "",
                                                               fieldName = svsErr.Element("fieldName") != null ? (string)svsErr.Element("fieldName").Value : "",
                                                               message = svsErr.Element("message") != null ? (string)svsErr.Element("message").Value : ""
                                                           }).ToArray()
                                                          : null,
                                                      warnings = e.Descendants("warning") != null ?
                                                          (from svsErr in e.Descendants("warning")
                                                           select new serviceWarning()
                                                           {
                                                               warningCode = svsErr.Element("warningCode") != null ? (string)svsErr.Element("warningCode").Value : "",
                                                               warningMessage = svsErr.Element("warningMessage") != null ? (string)svsErr.Element("warningMessage").Value : ""
                                                           }).ToArray()
                                                          : null,
                                                      infos = e.Descendants("info") != null ?
                                                          (from svsErr in e.Descendants("info")
                                                           select new serviceInfo()
                                                           {
                                                               infoCode = svsErr.Element("infoCode") != null ? (string)svsErr.Element("infoCode").Value : "",
                                                               infoMessage = svsErr.Element("infoMessage") != null ? (string)svsErr.Element("infoMessage").Value : ""
                                                           }).ToArray()
                                                          : null,
                                                      alternateRates =
                                                        e.Descendants("alternateRate") != null ?
                                                        (from aRates in e.Descendants("alternateRate")
                                                         select new rateDetail()
                                                         {
                                                             total = aRates.Element("total") !=null ?  decimal.Parse((string)aRates.Element("total").Value) : 0,
                                                             subtotal = aRates.Element("subtotal") !=null ? decimal.Parse((string)aRates.Element("subtotal").Value) : 0,
                                                             deliveryDate = aRates.Element("deliveryDate") != null ? (string)aRates.Element("deliveryDate").Value : "",
                                                             rateAdjustments = aRates.Descendants("rateAdjustment") != null ?
                                                           (from a in aRates.Descendants("rateAdjustment")
                                                            select new rateAdjustment()
                                                            {
                                                                freightClass = a.Element("freightClass") != null ? (string)a.Element("freightClass").Value : "",
                                                                weight = a.Element("weight") != null ? int.Parse((string)a.Element("weight").Value) : 0,
                                                                pieces = a.Element("pieces") != null ? int.Parse((string)a.Element("pieces").Value) : 0,
                                                                description = a.Element("description") != null ? (string)a.Element("description").Value : "",
                                                                descriptionCode = a.Element("descriptionCode") != null ? (string)a.Element("descriptionCode").Value : "",
                                                                amount = a.Element("amount") != null ? decimal.Parse((string)a.Element("amount").Value) : 0,
                                                                rate = a.Element("rate") != null ? decimal.Parse((string)a.Element("rate").Value) : 0
                                                            }).ToArray()
                                                           : null
                                                         }).ToArray()
                                                           : null,
                                                      accessorialsNotPriced = e.Descendants("accessorialsNotPriced").Elements("code") != null ?
                                                          (from aNotPriced in e.Descendants("accessorialsNotPriced").Elements("code")
                                                           select aNotPriced.Value).ToArray()
                                                     : null,
                                                      missingChargeCodes = e.Descendants("missingChargeCodes").Elements("code") != null ?
                                                          (from aNotPriced in e.Descendants("missingChargeCodes").Elements("code")
                                                           select aNotPriced.Value).ToArray()
                                                     : null

                                                  }).ToList();

            return results;

        }

        #endregion

        #region "P44 REST (v4)"

        /// <summary>
        /// Builds the object to send to P44 API REST method, calls the P44 REST method, and calls the method to process the response
        /// </summary>
        /// <param name="oData"></param>
        /// <returns></returns>
        public List<rateQuoteResponse> buildP44RateRequest(RateRequest oData)
        {


            List<rateQuoteResponse> P44RetErrors = new List<rateQuoteResponse>();
            rateQuoteResponse P44RetMsg = new rateQuoteResponse()
            {
                mode = translateMode("LTL"), //lvv - currently we only do LTL
                scac = "TEMP",
                vendor = "TEMP"
            };

            //CapacityProviderAccountGroup
            string P44AccountGroup = string.IsNullOrWhiteSpace(oData.loginGroupKey) ? "NGLOPS" : oData.loginGroupKey;
            var capacityProviderAccountGroup = new P44M.CapacityProviderAccountGroup(P44AccountGroup, null);

            //PickupWindow yyyy-MM-dd
            string sDate = string.IsNullOrWhiteSpace(oData.shipDate) ? DateTime.Now.AddDays(1).ToShortDateString() : oData.shipDate;
            string shipDate = Utilities.formatDateStringForAPI(sDate, DateTime.Now.AddDays(1));
            string startTime = Utilities.formatDispatchTimeStringForAPI(oData.shipDate, "08:00");
            string endTime = Utilities.formatDispatchTimeStringForAPI(oData.shipDate, "17:00");
            var pickupWindow = new P44M.LocalDateTimeWindow(shipDate, startTime, endTime);

            //DeliveryWindow
            string strDate = string.IsNullOrWhiteSpace(oData.deliveryDate) ? DateTime.Now.AddDays(3).ToShortDateString() : oData.deliveryDate;
            string deliveryDate = Utilities.formatDateStringForAPI(strDate, DateTime.Now.AddDays(3));
            string startTimeDel = Utilities.formatDispatchTimeStringForAPI(oData.deliveryDate, "08:00");
            string endTimeDel = Utilities.formatDispatchTimeStringForAPI(oData.deliveryDate, "17:00");
            var deliveryWindow = new P44M.LocalDateTimeWindow(deliveryDate, startTimeDel, endTimeDel);

            //TotalLinearFeet - Only used for Volume LTL
            int? totalLinearFeet = null; //oData.totalLinearFeet

            //PaymentTermsOverride
            P44M.RateQuoteQuery.PaymentTermsOverrideEnum paymentTermsOverride = getPaymentTermsOverrideEnum("");

            //DirectionOverride
            P44M.RateQuoteQuery.DirectionOverrideEnum directionOverride = getDirectionOverrideEnum("");

            //ApiConfiguration
            bool fetchAllGuaranteed = true;
            bool fetchAllServiceLevels = true;
            bool fetchAllInsideDelivery = false;
            bool allowUnacceptedAccessorials = true;
            bool enableUnitConversion = true;
            bool fallBackToDefaultAccountGroup = false;
            var accessorialServiceConfiguration = new P44M.AccessorialServiceConfiguration(fetchAllServiceLevels, fetchAllGuaranteed, fetchAllInsideDelivery, allowUnacceptedAccessorials);
            var apiConfiguration = new P44M.RateQuoteApiConfiguration(oData.timeout, enableUnitConversion, accessorialServiceConfiguration, fallBackToDefaultAccountGroup);


            //OriginAddress
            if (string.IsNullOrEmpty(oData.origin.postalCode) || oData.origin.postalCode.Length > 10)
            {

                if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                if (P44RetErrors.Count() > 0)
                {
                    P44RetErrors[0].postMessagesOnly = true;
                    P44RetErrors[0].AddMessage(MessageEnum.E_InvalidAddressInfo, "Origin Postal/Zip Code", "Invalid Address Information", "postalCode");
                }
                else
                {
                    P44RetMsg.postMessagesOnly = true;
                    P44RetMsg.AddMessage(MessageEnum.E_InvalidAddressInfo, "Origin Postal/Zip Code", "Invalid Address Information", "postalCode");
                    P44RetErrors.Add(P44RetMsg);
                }

            }

            // check state.

            //string[] sStates = new string[] { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY", "AS", "DC", "FM", "GU", "MH", "MP", "PW", "PR", "VI" };
            //if ( string.IsNullOrEmpty(oData.origin.stateName) || oData.origin.stateName.Length != 2)
            //{
            //    if (oData.origin.postalCode.Length > 2)
            //    {
            //        oData.origin.postalCode = oData.origin.postalCode.Substring(0, 2);
            //    } 
            //    if (oData.origin.stateName.Length != 2 || !sStates.Contains(oData.origin.stateName))
            //    {
            //        throw new System.ApplicationException("Invalid Origin State.");
            //    }
            //}
            //if (!sStates.Contains(oData.origin.stateName))
            //{
            //    throw new System.ApplicationException("Invalid Origin State.");
            //}
            var origAddress = new P44M.Address(oData.origin.postalCode, new List<string> { oData.origin.address1, oData.origin.companyName }, oData.origin.city, oData.origin.stateName, Utilities.getCountryEnum(oData.origin.country));

            //DeliveryWindow
            if (string.IsNullOrEmpty(oData.destination.postalCode) || oData.destination.postalCode.Length > 10)
            {
                if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                if (P44RetErrors.Count() > 0)
                {
                    P44RetErrors[0].postMessagesOnly = true;
                    P44RetErrors[0].AddMessage(MessageEnum.E_InvalidAddressInfo, "Destination Postal/Zip Code", "Invalid Address Information", "postalCode");
                }
                else
                {
                    P44RetMsg.postMessagesOnly = true;
                    P44RetMsg.AddMessage(MessageEnum.E_InvalidAddressInfo, "Destination Postal/Zip Code", "Invalid Address Information", "postalCode");
                    P44RetErrors.Add(P44RetMsg);
                }
            }
            //if (string.IsNullOrEmpty(oData.destination.postalCode) || oData.destination.postalCode.Length != 5)
            //{
            //    if (oData.destination.postalCode.Length > 5)
            //    {
            //        oData.destination.postalCode = oData.destination.postalCode.Substring(0, 5);
            //    }
            //    else
            //    {
            //        throw new System.ApplicationException("Invalid Destination Postal/Zip Code");
            //    }
            //}
            //if (string.IsNullOrEmpty(oData.destination.stateName) || oData.destination.stateName.Length != 2)
            //{
            //    if (oData.destination.postalCode.Length > 2)
            //    {
            //        oData.destination.postalCode = oData.destination.postalCode.Substring(0, 2);
            //    }
            //    if (oData.destination.stateName.Length != 2 || !sStates.Contains(oData.destination.stateName))
            //    {
            //        throw new System.ApplicationException("Invalid Destination State.");
            //    }
            //}
            //if (!sStates.Contains(oData.destination.stateName))
            //{
            //    throw new System.ApplicationException("Invalid Destination State.");
            //}
            if (P44RetErrors == null || (P44RetErrors.Count() < 1 || P44RetErrors[0].postMessagesOnly == false))
            {
                var destAddress = new P44M.Address(oData.destination.postalCode, new List<string> { oData.destination.address1, oData.destination.companyName }, oData.destination.city, oData.destination.stateName, Utilities.getCountryEnum(oData.destination.country));

                //LineItems
                List<P44M.LineItem> lineItems = new List<P44M.LineItem>();
                List<P44M.AccessorialService> accessorialServices = new List<P44M.AccessorialService>();
                P44M.RateQuoteQuery.WeightUnitEnum weightUnit = P44M.RateQuoteQuery.WeightUnitEnum.LB;
                P44M.RateQuoteQuery.LengthUnitEnum lengthUnit = P44M.RateQuoteQuery.LengthUnitEnum.IN;
                if (oData.lineItems?.Length > 0)
                {
                    foreach (rateQuoteLineImpl i in oData.lineItems)
                    {
                        if (P44RetErrors == null || (P44RetErrors.Count() < 1 || P44RetErrors[0].postMessagesOnly == false))
                        {
                            decimal wgt = 0;
                            Decimal.TryParse(i.weight, out wgt);
                            if (wgt <= 0)
                            {
                                if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                                if (P44RetErrors.Count() > 0)
                                {
                                    P44RetErrors[0].postMessagesOnly = true;
                                    P44RetErrors[0].AddMessage(MessageEnum.E_InvalidPackageInfo, "weight," + i.weight + ", is not valid", "Invalid Package Info", "weight");
                                }
                                else
                                {
                                    P44RetMsg.postMessagesOnly = true;
                                    P44RetMsg.AddMessage(MessageEnum.E_InvalidPackageInfo, "weight," + i.weight + ", is not valid", "Invalid Package Info", "weight");
                                    P44RetErrors.Add(P44RetMsg);
                                }
                            }
                            else if (i.length <= 0 || i.width <= 0 || i.height <= 0)
                            {
                                if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                                if (P44RetErrors.Count() > 0)
                                {
                                    P44RetErrors[0].postMessagesOnly = true;
                                    P44RetErrors[0].AddMessage(MessageEnum.E_InvalidPackageInfo, "length: " + i.length.ToString() + ", width:" + i.width.ToString() + ", or height: " + i.height.ToString() + " is not valid", "Invalid Package Info", "dimensions");
                                }
                                else
                                {
                                    P44RetMsg.postMessagesOnly = true;
                                    P44RetMsg.AddMessage(MessageEnum.E_InvalidPackageInfo, "length: " + i.length.ToString() + ", width:" + i.width.ToString() + ", or height: " + i.height.ToString() + " is not valid", "Invalid Package Info", "dimensions");
                                    P44RetErrors.Add(P44RetMsg);
                                }
                            }
                            else if (i.palletCount <= 0)
                            {
                                if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                                if (P44RetErrors.Count() > 0)
                                {
                                    P44RetErrors[0].postMessagesOnly = true;
                                    P44RetErrors[0].AddMessage(MessageEnum.E_InvalidPackageInfo, "pallet Count: " + i.palletCount.ToString() + " is not valid", "Invalid Package Info", "palletCount");
                                }
                                else
                                {
                                    P44RetMsg.postMessagesOnly = true;
                                    P44RetMsg.AddMessage(MessageEnum.E_InvalidPackageInfo, "pallet Count: " + i.palletCount.ToString() + " is not valid", "Invalid Package Info", "palletCount");
                                    P44RetErrors.Add(P44RetMsg);
                                }
                            }
                            if (P44RetErrors == null || (P44RetErrors.Count() < 1 || P44RetErrors[0].postMessagesOnly == false))
                            {
                                var dimensions = new P44M.CubicDimension(i.length, i.width, i.height);

                                var freightClass = Utilities.getFreightClassEnum(i.freightClass, P44M.LineItem.FreightClassEnum._70);
                                var packageType = Utilities.getPackageTypeEnum((string.IsNullOrWhiteSpace(i.packageType) ? "PLT" : i.packageType));

                                int totalPackages = i.palletCount;
                                P44M.LineItemHazmatDetail hazmatDetail = null;

                                var lineItem = new P44M.LineItem(wgt, dimensions, freightClass, packageType, totalPackages, i.numPieces, i.description, i.stackable, i.nmfcItem, i.nmfcSub, hazmatDetail);

                                lineItems.Add(lineItem);
                            }
                        } else
                        {
                            break;
                        }
                    }
                    if (P44RetErrors == null || (P44RetErrors.Count() < 1 || P44RetErrors[0].postMessagesOnly == false))
                    {
                        string wgtUnit = string.IsNullOrWhiteSpace(oData.lineItems[0].weightUnit) ? "" : oData.lineItems[0].weightUnit;
                        string dimUnit = string.IsNullOrWhiteSpace(oData.lineItems[0].dimUnit) ? "" : oData.lineItems[0].dimUnit;
                        weightUnit = getWeightUnitEnum(wgtUnit); //WeightUnit               
                        lengthUnit = getLengthUnitEnum(dimUnit); //LengthUnit
                    }
                }
                else
                {
                    if (P44RetErrors == null) { P44RetErrors = new List<rateQuoteResponse>(); }
                    if (P44RetErrors.Count() > 0)
                    {
                        P44RetErrors[0].postMessagesOnly = true;
                        P44RetErrors[0].AddMessage(MessageEnum.E_InvalidPackageInfo, "Invalid Item or Package Count", "Invalid Package Info", "dimensions");
                    }
                    else
                    {
                        P44RetMsg.postMessagesOnly = true;
                        P44RetMsg.AddMessage(MessageEnum.E_InvalidPackageInfo, "Invalid Item or Package Count", "Invalid Package Info", "dimensions");
                        P44RetErrors.Add(P44RetMsg);
                    }
                }
                if (P44RetErrors == null || (P44RetErrors.Count() < 1 || P44RetErrors[0].postMessagesOnly == false))
                {
                    //AccessorialServices
                    if (oData.accessorials?.Length > 0)
                    {
                        foreach (string s in oData.accessorials)
                        {
                            if (s != "FSC")
                            {
                                var accessorial = new P44M.AccessorialService(s);
                                accessorialServices.Add(accessorial);
                            }
                           
                        }
                    }
                    //RateQuoteQuery
                    var query = new P44M.RateQuoteQuery(origAddress, destAddress, lineItems, capacityProviderAccountGroup, accessorialServices, pickupWindow, deliveryWindow, totalLinearFeet, weightUnit, lengthUnit, paymentTermsOverride, directionOverride, apiConfiguration);

                    P44SDK.V4.Api.LTLQuotesApi apiClient = new P44SDK.V4.Api.LTLQuotesApi(P44WebServiceUrl);
                    apiClient.Configuration.Username = P44WebServiceLogin;
                    apiClient.Configuration.Password = P44WebServicePassword;

                    P44M.RateQuoteCollection rateQuoteCollection = apiClient.QueryRateQuotes(query);
                    return ProcessRateQuoteResponse(rateQuoteCollection);
                }
                else
                {
                    return P44RetErrors;
                }
            } else
            {
                return P44RetErrors;
            }
        }

        /// <summary>
        /// Proccess the response from the P44 REST API For Rate Quotes
        /// Currently we only support LTL Rate Quotes (this is hardcoded currently)
        /// Note: Honestly I am not sure what the difference is between LTL and VLTL
        /// quoting - do they call the same P44 method? Is there a flag in the object? Not sure
        /// </summary>
        /// <param name="rateQuoteCollection"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created By LVV on 2/8/2019
        /// P44 no longer supporting old version so this new method uses P44 v4 API 
        /// and no longer uses old XML stuff from I think v1
        /// </remarks>
        /// <!--
        /// NOTES
        /// Stuff from old XML that we no longer map to rateQuoteResponse obj
        ///  totalWeight, totalPallets, totalPieces, carrierNote
        /// -->
        public List<rateQuoteResponse> ProcessRateQuoteResponse(P44M.RateQuoteCollection rateQuoteCollection)
        {
            List<rateQuoteResponse> results =
                (from q in rateQuoteCollection.RateQuotes
                 select new rateQuoteResponse()
                 {
                     mode = translateMode("LTL"), //lvv - currently we only do LTL
                     scac = q.CarrierCode != null ? q.CarrierCode : (q.CapacityProviderAccountGroup != null ? (q.CapacityProviderAccountGroup.Accounts?.Count > 0 ? q.CapacityProviderAccountGroup.Accounts[0].Code : "") : ""),
                     vendor = q.ContractId != null ? q.ContractId : "",
                     interLine = q.LaneType.HasValue ? q.LaneType.Value.ToString() : "", //LVV TODO -- does this work as expected??
                     quoteNumber = q.CapacityProviderQuoteNumber != null ? q.CapacityProviderQuoteNumber : "",
                     deliveryDate = q.DeliveryDateTime.HasValue ? q.DeliveryDateTime.Value.ToString() : "",
                     expirationDate = q.QuoteExpirationDateTime.HasValue ? q.QuoteExpirationDateTime.Value.ToString() : "",
                     quoteDate = q.QuoteEffectiveDateTime.HasValue ? q.QuoteEffectiveDateTime.Value.ToShortDateString() : DateTime.Now.ToShortDateString(),
                     serviceType = q.ServiceLevel != null ? (!string.IsNullOrWhiteSpace(q.ServiceLevel.Description) ? q.ServiceLevel.Description : q.ServiceLevel.Code.ToString()) : "", //LVV TODO -- does this work as expected??
                     transitTime = q.TransitDays.HasValue ? q.TransitDays.Value : 0, //lvv - is this correct?
                     //carrierNote = e.Element("carrierNote") != null ? (string)e.Element("carrierNote").Value : "",
                     //totalPallets = e.Element("totalPallets") != null ? int.Parse(e.Element("totalPallets").Value) : 0,
                     //totalPieces = e.Element("totalPieces") != null ? int.Parse(e.Element("totalPieces").Value) : 0,
                     //totalWeight = e.Element("totalWeight") != null ? int.Parse(e.Element("totalWeight").Value) : 0,
                     rateDetail =
                     q.RateQuoteDetail != null ?
                     new rateDetail()
                     {
                         total = q.RateQuoteDetail.Total.HasValue ? q.RateQuoteDetail.Total.Value : 0,
                         subtotal = q.RateQuoteDetail.Subtotal.HasValue ? q.RateQuoteDetail.Subtotal.Value : 0, //added by lvv
                         rateAdjustments = q.RateQuoteDetail.Charges != null ?
                         (
                             (from ch in q.RateQuoteDetail.Charges
                              select new rateAdjustment()
                              {
                                  freightClass = ch.ItemFreightClass,
                                  weight = ch.ItemWeight.HasValue ? (int)ch.ItemWeight.Value : 0,
                                  description = ch.Description,
                                  descriptionCode = ch.Code, //LVV - is this correct??
                                  amount = ch.Amount.HasValue ? ch.Amount.Value : 0,
                                  rate = ch.Rate.HasValue ? ch.Rate.Value : 0
                              }).ToArray().Concat(
                                 q.RequestedAccessorialServices != null ?
                                 (from a in q.RequestedAccessorialServices
                                  where a.Status != P44M.RequestedAccessorialService.StatusEnum.ACCEPTED
                                  select new rateAdjustment()
                                  {
                                      //freightClass = "",
                                      weight = 0,
                                      description = a.Status.HasValue ? Utilities.getStatusEnumStringDesc(a.Status.Value) : "",
                                      descriptionCode = a.Code, //LVV - is this correct??
                                      amount = 0,
                                      rate = 0
                                  }).ToArray() : new rateAdjustment[] { }
                                  ).ToArray()
                         )
                         : new rateAdjustment[] { } //rateAdjustments
                     }
                     : new rateDetail(), //rateDetail
                     errors = q.ErrorMessages != null ?
                     (from svsErr in q.ErrorMessages
                      select new ServiceError()
                      {
                          errorCode = svsErr.Severity.HasValue ? Utilities.getSeverityEnumString(svsErr.Severity.Value) : "",
                          errorMessage = !string.IsNullOrWhiteSpace(svsErr._Message) ? svsErr._Message : "",
                          vendorErrorCode = "", //no longer mapped
                          vendorErrorMessage = !string.IsNullOrWhiteSpace(svsErr.Diagnostic) ? svsErr.Diagnostic : "",
                          fieldName = "",//no longer mapped
                          message = svsErr.Source.HasValue ? Utilities.getSourceEnumString(svsErr.Source.Value) : ""
                      }).ToArray()
                      : new ServiceError[] {}, //errors
                     warnings = q.InfoMessages != null ?
                         (from svsErr in q.InfoMessages
                          where svsErr.Severity == P44M.Message.SeverityEnum.WARNING
                          select new serviceWarning()
                          {
                              warningCode = svsErr.Source.HasValue ? Utilities.getSourceEnumString(svsErr.Source.Value) : "",
                              warningMessage = svsErr.Source.Value == P44M.Message.SourceEnum.SYSTEM ? svsErr._Message : svsErr.Diagnostic
                          }).ToArray()
                         : new serviceWarning[] { }, //warnings
                     infos = q.InfoMessages != null ?
                         (from svsErr in q.InfoMessages
                          where svsErr.Severity == P44M.Message.SeverityEnum.INFO
                          select new serviceInfo()
                          {
                              infoCode = svsErr.Source.HasValue ? Utilities.getSourceEnumString(svsErr.Source.Value) : "",
                              infoMessage = svsErr.Source.Value == P44M.Message.SourceEnum.SYSTEM ? svsErr._Message : svsErr.Diagnostic
                          }).ToArray()
                         : new serviceInfo[] { }, //infos
                     alternateRates =
                     q.AlternateRateQuotes != null ?
                     (from aRates in q.AlternateRateQuotes
                      select new rateDetail()
                      {
                          total = aRates.RateQuoteDetail.Total.HasValue ? aRates.RateQuoteDetail.Total.Value : 0,
                          subtotal = aRates.RateQuoteDetail.Subtotal.HasValue ? aRates.RateQuoteDetail.Subtotal.Value : 0,
                          deliveryDate = aRates.DeliveryDateTime.HasValue ? aRates.DeliveryDateTime.Value.ToString() : "",
                          transitTime = aRates.TransitDays.HasValue ? aRates.TransitDays.Value : 0, //added by lvv - is this correct?
                          rateAdjustments = aRates.RateQuoteDetail.Charges != null ?
                          (from a in aRates.RateQuoteDetail.Charges
                           select new rateAdjustment()
                           {
                               freightClass = a.ItemFreightClass,
                               weight = a.ItemWeight.HasValue ? (int)a.ItemWeight.Value : 0,
                               description = a.Description,
                               descriptionCode = a.Code, //LVV - is this correct??
                               amount = a.Amount.HasValue ? a.Amount.Value : 0,
                               rate = a.Rate.HasValue ? a.Rate.Value : 0
                           }).ToArray()
                           : new rateAdjustment[] { } //rateAdjustments
                      }).ToArray()
                      : new rateDetail[] { }, //alternateRates
                     // accessorialsNotPriced = e.Descendants("accessorialsNotPriced").Elements("code") != null ?
                     //     (from aNotPriced in e.Descendants("accessorialsNotPriced").Elements("code")
                     //      select aNotPriced.Value).ToArray()
                     //: null,
                     // missingChargeCodes = e.Descendants("missingChargeCodes").Elements("code") != null ?
                     //     (from aNotPriced in e.Descendants("missingChargeCodes").Elements("code")
                     //      select aNotPriced.Value).ToArray()
                     //: null
                 }).ToList();
            return results;
        }

        #endregion


        private P44M.RateQuoteQuery.WeightUnitEnum getWeightUnitEnum(string s)
        {
            var strWeightUnitEnum = s.Trim().ToUpper();
            P44M.RateQuoteQuery.WeightUnitEnum retVal = P44M.RateQuoteQuery.WeightUnitEnum.LB; //Set return value to LB by default           
            if (string.IsNullOrWhiteSpace(strWeightUnitEnum)) { return retVal; } //If strWeightUnitEnum is null return LB by default
            if (strWeightUnitEnum == "KG") { retVal = P44M.RateQuoteQuery.WeightUnitEnum.KG; }
            return retVal;
        }

        private P44M.RateQuoteQuery.LengthUnitEnum getLengthUnitEnum(string s)
        {
            var strLengthUnitEnum = s.Trim().ToUpper();
            P44M.RateQuoteQuery.LengthUnitEnum retVal = P44M.RateQuoteQuery.LengthUnitEnum.IN; //Set return value to IN by default 
            if (string.IsNullOrWhiteSpace(strLengthUnitEnum)) { return retVal; } //If strLengthUnitEnum is null return IN by default
            if (strLengthUnitEnum == "CM") { retVal = P44M.RateQuoteQuery.LengthUnitEnum.CM; }
            return retVal;
        }

        private P44M.RateQuoteQuery.PaymentTermsOverrideEnum getPaymentTermsOverrideEnum(string s)
        {
            var strPayment = s.Trim().ToUpper();
            P44M.RateQuoteQuery.PaymentTermsOverrideEnum retVal = P44M.RateQuoteQuery.PaymentTermsOverrideEnum.PREPAID; //Set return value to PREPAID by default   
            switch (strPayment)
            {
                case "PREPAID":
                    retVal = P44M.RateQuoteQuery.PaymentTermsOverrideEnum.PREPAID;
                    break;
                case "COLLECT":
                    retVal = P44M.RateQuoteQuery.PaymentTermsOverrideEnum.COLLECT;
                    break;
                case "THIRDPARTY":
                    retVal = P44M.RateQuoteQuery.PaymentTermsOverrideEnum.THIRDPARTY;
                    break;
            }
            return retVal;
        }

        private P44M.RateQuoteQuery.DirectionOverrideEnum getDirectionOverrideEnum(string s)
        {
            var strDirection = s.Trim().ToUpper();
            P44M.RateQuoteQuery.DirectionOverrideEnum retVal = P44M.RateQuoteQuery.DirectionOverrideEnum.THIRDPARTY; //Set return value to THIRDPARTY by default    
            switch (strDirection)
            {
                case "THIRDPARTY":
                    retVal = P44M.RateQuoteQuery.DirectionOverrideEnum.THIRDPARTY;
                    break;
                case "SHIPPER":
                    retVal = P44M.RateQuoteQuery.DirectionOverrideEnum.SHIPPER;
                    break;
                case "CONSIGNEE":
                    retVal = P44M.RateQuoteQuery.DirectionOverrideEnum.CONSIGNEE;
                    break;
            }
            return retVal;
        }

        //LVV -- this is for in case enum.ToString() does not work as expected -- also I guess we could add localization here?
        private string getLaneTypeEnumString(P44M.RateQuote.LaneTypeEnum e)
        {
            string strRet = "";
            switch (e)
            {
                case P44M.RateQuote.LaneTypeEnum.DIRECT:
                    strRet = "DIRECT";
                    break;
                case P44M.RateQuote.LaneTypeEnum.INTERLINE:
                    strRet = "INTERLINE";
                    break;
                case P44M.RateQuote.LaneTypeEnum.UNSPECIFIED:
                    strRet = "UNSPECIFIED";
                    break;
                default:
                    strRet = "";
                    break;
            }
            return strRet;
        }

        public static transportationMode translateMode(string mode)
        {
            transportationMode result = transportationMode.ALL;
            switch (mode.ToUpper())
            {
                case "PARCEL":
                    result = transportationMode.PARCEL;
                    break;
                case "LTL":
                    result = transportationMode.LTL;
                    break;
                case "VOLUME":
                    result = transportationMode.VOLUME;
                    break;
            }
            return result;
        }

        /// <summary>
        /// translates the translation mode to english text: caller is responsible for translation to other languages
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR on 02/06/2018 for v-8.1
        ///   we now call the new static method to maintain code integrity when changes are made to the translation.
        ///   
        /// </remarks>
        public string translateModeToString(transportationMode mode)
        {
            return transportationModeTranslation(mode);
        }

        /// <summary>
        /// translates the translation mode to english text: caller is responsible for translation to other languages
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <remarks>
        /// Added by RHR on 02/06/2018 for v-8.1
        ///   new static method so instance of class is not required
        /// </remarks>
        public static string transportationModeTranslation(transportationMode mode)
        {
            string result = "ALL";
            switch (mode)
            {
                case transportationMode.PARCEL:
                    result = "PARCEL";
                    break;
                case transportationMode.LTL:
                    result = "LTL";
                    break;
                case transportationMode.VOLUME:
                    result = "VOLUME";
                    break;
            }
            return result;
        }

    }
}