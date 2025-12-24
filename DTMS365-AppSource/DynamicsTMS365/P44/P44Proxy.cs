using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;
using System.Web;
using System.IO;


namespace DynamicsTMS365.P44
{
    public class P44Proxy
    {
        public string P44WebServiceUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["P44WebServiceUrl"]; }
        }

        public string P44WebServiceLogin
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["P44WebServiceLogin"]; }
        }

        public string P44WebServicePassword
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["P44WebServicePassword"]; }
        }


        public List<rateQuoteResponse> GetRateQuotes(RateRequest rateQuoteRequest)
        {
            string xml = buildXMLOrderWStream(rateQuoteRequest);
            return restServicesXML(xml);
            //return GetRateQuotesXML(xml);           
        }

        public RateQuotes GetRateQuotesXML(string xml)
        {
            //var encodedxml = System.Web.HttpUtility.UrlEncode(xml);


            var client = new RestClient(P44WebServiceUrl);
            //var request = new RestRequest("/xml/multirate/quote.xml", Method.POST);
            //var request = new RestRequest("/xml/xmlrate/quote.xml", Method.POST);
            var request = new RestRequest("/xml/xmlrate/quote.xml", Method.GET);
            //string sBody = string.Format("p44Login={0}&p44Password={1}]&xml={2}", System.Web.HttpUtility.UrlEncode(P44WebServiceLogin), System.Web.HttpUtility.UrlEncode(P44WebServicePassword), encodedxml);
            request.AddParameter("p44Login", P44WebServiceLogin, ParameterType.QueryString);
            request.AddParameter("p44Password", P44WebServicePassword, ParameterType.QueryString);
            //request.AddParameter("p44Login", System.Web.HttpUtility.UrlEncode(P44WebServiceLogin), ParameterType.RequestBody);
            //request.AddParameter("p44Password", System.Web.HttpUtility.UrlEncode(P44WebServicePassword), ParameterType.RequestBody);
            request.AddParameter("xml", xml, ParameterType.QueryString);
            //request.AddParameter("xml", System.Web.HttpUtility.UrlEncode(xml), ParameterType.RequestBody);
            //request.AddParameter("text/xml", xml, ParameterType.RequestBody);
            //request.AddBody(sBody); //, "http://www.p-44.com/xml");


            //IRestResponse<RateQuotes> oResponse = client.Execute<RateQuotes>(request);
            var response = client.Execute<RateQuotes>(request);

            var oRData1 = response.Data;

          return null;
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

            writer.WriteElementString("returnMultiple", "true");
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
                    writeNodeIfNotEmpty(ref writer, "code", s);
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

        public  void writeNodeIfNotEmpty(ref XmlWriter writer, string node, string val)
        {
            if (!string.IsNullOrWhiteSpace(val)) { writer.WriteElementString(node, val); }
        }

        public  void writeNodeIfNotEmpty(ref XmlWriter writer, string node, string val, string sdefault)
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


        public List<rateQuoteResponse>  restServicesXML(string xml)
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
                                                               weight = a.Element("weight") != null ? int.Parse(a.Element("weight").Value) : 0,
                                                               pieces = a.Element("pieces") != null ? int.Parse(a.Element("pieces").Value) : 0,
                                                               description = a.Element("description") != null ? (string)a.Element("description").Value : "",
                                                               descriptionCode = a.Element("descriptionCode") != null ? (string)a.Element("descriptionCode").Value : "",
                                                               amount = a.Element("amount") != null ? decimal.Parse(a.Element("amount").Value) : 0,
                                                               rate = a.Element("rate") != null ? decimal.Parse(a.Element("rate").Value) : 0
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
                                                             total = aRates.Element("total") != null ? decimal.Parse(aRates.Element("infoCode").Value) : 0,
                                                             subtotal = aRates.Element("subtotal") != null ? decimal.Parse(aRates.Element("subtotal").Value) : 0,
                                                             deliveryDate = aRates.Element("deliveryDate") != null ? (string)aRates.Element("deliveryDate").Value : "",
                                                             rateAdjustments = aRates.Descendants("rateAdjustment") != null ?
                                                                  (from a in aRates.Descendants("rateAdjustment")
                                                                   select new rateAdjustment()
                                                                   {
                                                                       freightClass = a.Element("freightClass") != null ? (string)a.Element("freightClass").Value : "",
                                                                       weight = a.Element("weight") != null ? int.Parse(a.Element("weight").Value) : 0,
                                                                       pieces = a.Element("pieces") != null ? int.Parse(a.Element("pieces").Value) : 0,
                                                                       description = a.Element("description") != null ? (string)a.Element("description").Value : "",
                                                                       descriptionCode = a.Element("descriptionCode") != null ? (string)a.Element("descriptionCode").Value : "",
                                                                       amount = a.Element("amount") != null ? decimal.Parse(a.Element("amount").Value) : 0,
                                                                       rate = a.Element("rate") != null ? decimal.Parse(a.Element("rate").Value) : 0
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

        public DynamicsTMS365.P44.transportationMode translateMode(string mode)
        {
            DynamicsTMS365.P44.transportationMode result = transportationMode.ALL;
            switch (mode.ToUpper()){
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

        public string translateModeToString(DynamicsTMS365.P44.transportationMode mode)
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

        public string formatShortDateTime(DateTime? dtVal)
        {
            if (dtVal.HasValue)
            { return string.Format("{0} {1}", dtVal.Value.ToShortDateString(), dtVal.Value.ToShortTimeString()); }
            else { return ""; }
        }

    }
}