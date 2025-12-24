using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RestSharp;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using static NGLTrimbleServices.TMSPCMWrapper;


namespace NGLTrimbleServices
{

    public class TrimbleAPI
    {

        public TrimbleAPI(bool bUseTLs12 = true)
        {
            if (bUseTLs12)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            }
        }

        public string _strLastError = "";
        public string gLastError = "";
        //temporary holder of possible address matches.
        private string[] _sAddressData;
        public string[] sAddressData
        {
            get
            {
                return _sAddressData;
            }
            set
            {
                _sAddressData = value;
            }
        }
        private bool _blnUseZipOnly = false;

        public bool UseZipOnly
        {
            get
            {
                return _blnUseZipOnly;
            }

            set
            {
                _blnUseZipOnly = value;
            }
        }

        public string LastError
        {
            get
            {
                return _strLastError + gLastError;
            }
            set { }
        }

        private bool _blnDebug = false;

        public bool Debug
        {
            get
            {
                bool DebugRet = false;
                DebugRet = _blnDebug;
                return DebugRet;
            }

            set
            {
                _blnDebug = value;
            }
        }

        private int _intKeepLogDays = 7;

        public int KeepLogDays
        {
            get
            {
                return _intKeepLogDays;
            }

            set
            {
                _intKeepLogDays = value;
            }
        }

        private bool _blnSaveOldLog = false;

        public bool SaveOldLog
        {
            get
            {
                return _blnSaveOldLog;
            }

            set
            {
                _blnSaveOldLog = value;
            }
        }

        private string getErrorMessage(ref Exception ex)
        {
            string strRet = "";
            if (this.Debug)
                strRet = ex.ToString();
            else
                strRet = ex.Message;
            return strRet;
        }
        private List<clsTrimbleReportParams> _sRouteTrimbleReports;
        public List<clsTrimbleReportParams> TrimbleRouteReports
        {
            get
            {
                return _sRouteTrimbleReports;
            }
            set
            {
                _sRouteTrimbleReports = value;
            }
        }
        private string _strWebServiceURL = "https://pcmiler.alk.com/apis/rest/v1.0/Service.svc/";

        public string WebServiceURL
        {
            get
            {
                return _strWebServiceURL;
            }

            set
            {
                _strWebServiceURL = value;
            }
        }

        private string _SingleSearchWebServiceURL = "https://singlesearch.alk.com/";

        public string SingleSearchWebServiceURL
        {
            get
            {
                return _SingleSearchWebServiceURL;
            }

            set
            {
                _SingleSearchWebServiceURL = value;
            }
        }
        private string _strAPIKey = "C36349D0A5F5D440AAC0CB8A0287F02C";

        public string APIKey
        {
            get
            {
                return _strAPIKey;
            }

            set
            {
                _strAPIKey = value;
            }
        }
        //convert this to the correct object type for this sample we just use string
        public List<string> APIRouteData { get; set; }



        /// <summary>
        /// Method to Get the City Name based on Zip.
        /// </summary>
        /// <param name="szip">zip</param>
        /// <param name="sResults">concationation of lon,lat along with','</param>
        /// <returns>short value</returns>
        /// <remarks>
        /// Notes by RHR for v-8.4 on 06/07/2021
        ///     TODO: modify the logic to use a full address not Just the postcode  we need to use the street address
        ///     locations?&postcode= needs more information on this service
        ///     We need an overload that returns the data not just a string.
        /// </remarks>
        public short PCMSCityToLatLong(string szip, ref string sResults)
        {
            // the legacy logc for PCMSCityToLatLong returns a string with the results
            //look at the caller to determien the correct format for lat long and 
            //add the results to the string (for backward compatibiliy)
            //a new method may be used to return the actual lat/long.
            //Add code to call API and configure return value.
            IRestResponse response = null;
            TrimbleServiceReference.Coordinates lstPoints = new TrimbleServiceReference.Coordinates();
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var client = new RestSharp.RestClient(WebServiceURL + "locations?&postcode=" + szip + "&dataset=Current&authToken=" + APIKey);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            List<object> result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(response.Content).ToList();
            if (result.Count > 0)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result[0].ToString());
                lstPoints = Newtonsoft.Json.JsonConvert.DeserializeObject<TrimbleServiceReference.Coordinates>(values["Coords"].ToString());
                if (lstPoints != null)
                    sResults = lstPoints.Lat + "," + lstPoints.Lon;
                else
                    return -1;

            }
            else
            {
                return -1;
            }



            return 1;
        }
        /// <summary>
        /// Method to Get the Coordinates(Lat,Lng).
        /// </summary>
        /// <param name="oaddress">Object of clsAddress</param>
        /// <param name="Lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        /// <returns>short value</returns>
        public short PCMSAddressToLatlong(clsAddress oaddress, ref double Lat, ref double lng)
        {
            // the legacy logc for PCMSCityToLatLong returns a string with the results
            //look at the caller to determien the correct format for lat long and 
            //add the results to the string (for backward compatibiliy)
            //a new method may be used to return the actual lat/long.
            //Add code to call API and configure return value.
            IRestResponse response = null;

            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            TrimbleServiceReference.Coordinates lstPoints = new TrimbleServiceReference.Coordinates();
            string Street = HttpUtility.UrlEncode(oaddress.strAddress);
            string City = HttpUtility.UrlEncode(oaddress.strCity);
            string State = HttpUtility.UrlEncode(oaddress.strState);
            string PostCode = HttpUtility.UrlEncode(oaddress.strZip);
            var client = new RestSharp.RestClient(WebServiceURL + "locations?street=" + Street + "&city=" + City + "&state=" + State + "&postcode=" + PostCode + "&dataset=Current&authToken=" + APIKey);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            List<object> result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(response.Content).ToList();
            if (result.Count > 0)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result[0].ToString());
                lstPoints = Newtonsoft.Json.JsonConvert.DeserializeObject<TrimbleServiceReference.Coordinates>(values["Coords"].ToString());
                if (lstPoints != null)
                {
                    Lat = Convert.ToDouble(lstPoints.Lat); lng = Convert.ToDouble(lstPoints.Lon);
                }
                else
                    return -1;

            }
            else
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// Validate Address, returns count of address data found.  if UseZipOnly validate zip code only
        /// return -1 for no match, 0 for invalid address but zip code ok or actual matches found
        /// Caller must handle bad address assignment
        /// </summary>
        /// <param name="oAddress"></param>
        /// <param name="lAddresses"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.4.0.003 on 07/06/2021
        ///    Replaces PCMSLookup for Trimble
        ///    we can now return full address data not just a string.
        /// </remarks>
        public short TRMLookup(NGLTrimbleServices.TrimbleServiceReference.Address oAddress, ref List<TrimbleServiceReference.Address> lAddresses)
        {
            short iRet = -1;
            TrimbleServiceReference.Address adrs = new TrimbleServiceReference.Address();
            if (lAddresses == null) { lAddresses = new List<TrimbleServiceReference.Address>(); }
            List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
            string sAddress = HttpUtility.UrlEncode(string.Format("{0} {1}, {2}; {3}", oAddress.Zip, oAddress.City, oAddress.State, oAddress.StreetAddress));
            IRestResponse response = null;
            object result = null;
            if (this.UseZipOnly)
            {
                var client = new RestSharp.RestClient(SingleSearchWebServiceURL + "na/api/search?authToken=" + APIKey + "&query=" + oAddress.Zip);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                response = client.Execute(request);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);
                if (result != null)
                {
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                    lctn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.Location>>(values["Locations"].ToString());
                    if (lctn != null && lctn.Count > 0)
                    {
                        for (int i = 0; i < lctn.Count; i++)
                        {
                            adrs = lctn[i].Address;
                            //string adrss = adrs.StreetAddress + ";" + adrs.City + ";" + adrs.State + ";" + adrs.Zip + ";" + adrs.Country;
                            lAddresses.Add(adrs);
                        }
                        iRet = (short)lctn.Count;
                    }
                }
                return iRet;
            }  else
            {
                var client = new RestSharp.RestClient(SingleSearchWebServiceURL + "na/api/search?authToken=" + APIKey + "&query=" + sAddress);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                response = client.Execute(request);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);
            }
            if (result != null)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                lctn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.Location>>(values["Locations"].ToString());
                if (lctn != null && lctn.Count > 0)
                {                    
                    for (int i = 0; i < lctn.Count; i++)
                    {
                        adrs = lctn[i].Address;
                        //string adrss = adrs.StreetAddress + ";" + adrs.City + ";" + adrs.State + ";" + adrs.Zip + ";" + adrs.Country;
                        lAddresses.Add(adrs);
                    }
                    iRet = (short)lctn.Count;
                }
            } else if (!this.UseZipOnly)
            {
                var client = new RestSharp.RestClient(SingleSearchWebServiceURL + "na/api/search?authToken=" + APIKey + "&query=" + oAddress.Zip);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                response = client.Execute(request);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);
                if (result != null)
                {
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                    lctn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.Location>>(values["Locations"].ToString());
                    if (lctn != null && lctn.Count > 0)
                    {
                        for (int i = 0; i < lctn.Count; i++)
                        {
                            adrs = lctn[i].Address;
                            //string adrss = adrs.StreetAddress + ";" + adrs.City + ";" + adrs.State + ";" + adrs.Zip + ";" + adrs.Country;
                            lAddresses.Add(adrs);
                        }
                       
                    }
                }
                return 0;
            }


            return iRet;
        }

        public List<TrimbleServiceReference.Location> TRMSearch(string sAddress)
        {           
            IRestResponse response = null;
            List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
            string strAddress = HttpUtility.UrlEncode(sAddress);
            //  https://singlesearch.alk.com/na/api/search?authToken=C36349D0A5F5D440AAC0CB8A0287F02C&query=1%20independence%20way
            var client = new RestSharp.RestClient(SingleSearchWebServiceURL + "na/api/search?authToken=" + APIKey + "&query=" + strAddress);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            object result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);          
            if (result != null)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                lctn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.Location>>(values["Locations"].ToString());
            }
            return lctn;           
        }
        /// <summary>
        /// Method to check Address from API
        /// </summary>
        /// <param name="sAddress">address string</param>
        /// <returns>No Of matches</returns>
        public short PCMSLookup(string sAddress)
        {
            // the legacy logc for PCMSLookup would add an array of addresses to the trip
            // this logic may be replaced to return a list of matching addresses
            //Add code to call API and configure return value
            // One option for For backward compatibliity we could add the address 
            // list to an array of string then PCMSGetFmtMatch2 could 
            // reference each item using the index?  It is not clear how 
            //this will play out in the final release?

            // for now call API and get a list of address data
            // add the list to sAddressData
            //sample:
            IRestResponse response = null;
            short Ret = -1;

            string strAddress = HttpUtility.UrlEncode(sAddress);
            TrimbleServiceReference.Address adrs = new TrimbleServiceReference.Address();
            List<TrimbleServiceReference.Address> lstadrs = new List<TrimbleServiceReference.Address>();
            List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
            //  https://singlesearch.alk.com/na/api/search?authToken=C36349D0A5F5D440AAC0CB8A0287F02C&query=1%20independence%20way
            var client = new RestSharp.RestClient(SingleSearchWebServiceURL + "na/api/search?authToken=" + APIKey + "&query=" + strAddress);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            object result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);
            List<string> _saddress = new List<string>();
            if (result != null)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                lctn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.Location>>(values["Locations"].ToString());
                if (lctn != null && lctn.Count > 0)
                {
                    sAddressData = new string[lctn.Count];
                    for (int i = 0; i < lctn.Count; i++)
                    {
                        adrs = lctn[i].Address;
                        string adrss = adrs.StreetAddress + ";" + adrs.City + ";" + adrs.State + ";" + adrs.Zip + ";" + adrs.Country;
                        _saddress.Add(adrss);
                        sAddressData = _saddress.Distinct().ToArray();
                    }

                    Ret = (short)lctn.Count;
                    return Ret;
                }

            }

            // return (short)result.Count;
            //sAddressData = new string[3] { "street 1,city 1,state 1,zip 1,country 1", "street 2,city 2,state 2,zip 2,country 2", "street 3,city 3,state 3,zip 3,country 3" };
            return Ret; //number of matches
        }
        /// <summary>
        /// Method to the City,Address,State,zip based on format.
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="sStreet">ref Street</param>
        /// <param name="sCity">ref City</param>
        /// <param name="sState">ref State</param>
        /// <param name="sZip">ref Zip</param>
        /// <param name="sCountry">ref Country</param>
        /// <returns>returns int</returns>
        public int PCMSGetFmtMatch2(int index, ref string sStreet, ref string sCity, ref string sState, ref string sZip, ref string sCountry)
        {
            int iRet = -1;
            // the legacy logc for PCMSLookup would add an array of addresses to the trip
            //then we would loop through all the address on the trip and create an array of 
            // clsAddress objects for each address in teh trip's array
            //
            // The new logic should be able to make one call to get all matching address for sZipOrAddress
            // and parse the Json object adding the results to the result

            // One option for For backward compatibliity we could add the address 
            // list to an array of string in PCMSLookup then 
            // reference each item using the index?  It is not clear how 
            //this will play out in the final release
            //Sample:
            //read the array, parse the data and return the results
            if (sAddressData != null && sAddressData.Count() > index)
            {
                string sAddress = sAddressData[index];
                if (!string.IsNullOrEmpty(sAddress))
                {
                    string[] sData = sAddress.Split(';');
                    if (sData.Length > 0)
                    {
                        iRet = 1;
                        for (int i = 0; i < 5; i++)
                        {
                            if (sData.Length > 0)
                            {
                                switch (i)
                                {
                                    case 0:
                                        sStreet = sData[i];
                                        break;
                                    case 1:
                                        sCity = sData[i];
                                        break;

                                    case 2:
                                        sState = sData[i];
                                        break;

                                    case 3:
                                        sZip = sData[i];
                                        break;

                                    case 4:
                                        sCountry = sData[i];
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }

                }
            }
            return iRet;

        }
        /// <summary>
        /// Method to Get the Matched Address
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>address string</returns>
        public string PCMSGetMatch(int index = 0)
        {
            //caller must use PCMSLookup to populate the array of matching addresses
            // note the format of the legacy address may be different than 
            // the sample address stored in sAddressData
            // adjustments will be required
            IRestResponse response = null;
            string sRet = "";

            if (sAddressData != null && sAddressData.Count() > index)
            {
                sRet = sAddressData[index];

            }
            return sRet;

        }
        /// <summary>
        /// Method to check the Matched Address
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="sAddress">ref Address string</param>
        /// <returns>true/false</returns>
        public bool PCMSGetMatch(int index, ref string sAddress)
        {

            bool blnRet = false;

            if (sAddressData != null && sAddressData.Count() > index)
            {
                sAddress = sAddressData[index];
                blnRet = true;

            }
            return blnRet;

        }
        /// <summary>
        /// Method to Get the City Name.
        /// </summary>
        /// <param name="latLong">(lng,lat)</param>
        /// <param name="sCity">ref City</param>
        /// <returns>No of Matches</returns>
        public short PCMSLatLongToCity(string latLong, ref string sCity)
        {
            short Ret = -1; //represents number of matches found
            //add logic to look up lat long using API
            sCity = "API results goes here";
            IRestResponse response = null;

            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string coords = HttpUtility.UrlEncode(latLong);
            TrimbleServiceReference.Address adrs = new TrimbleServiceReference.Address();
            var client = new RestSharp.RestClient(WebServiceURL + "locations/reverse?Coords=" + coords + "&matchNamedRoadsOnly=true&maxCleanupMiles=20&region=NA&dataset=Current&timestamp=&includeTrimblePlaceIds=true&authToken=" + APIKey);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);
            if (result != null)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                adrs = Newtonsoft.Json.JsonConvert.DeserializeObject<TrimbleServiceReference.Address>(values["Address"].ToString());
                if (adrs != null)
                {
                    sCity = adrs.City;
                    return 1;
                }
                else
                    return -1;

            }
            else
            {
                return -1;
            }

        }
        /// <summary>
        /// Method to Check the PlaceNames
        /// </summary>
        /// <param name="sAddress">address string</param>
        /// <returns>No of Matches</returns>
        public int PCMSCheckPlaceName(string sAddress)
        {
            int iRet = 0;
            //add logic to return the number of matches found for sAddress
            IRestResponse response = null;
            string strAddress = HttpUtility.UrlEncode(sAddress);
            TrimbleServiceReference.Address adrs = new TrimbleServiceReference.Address();
            List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
            var client = new RestSharp.RestClient(SingleSearchWebServiceURL + "na/api/search?authToken=" + APIKey + "&query=" + strAddress);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            object result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);
            List<string> _saddress = new List<string>();
            if (result != null)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                lctn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.Location>>(values["Locations"].ToString());
                if (lctn != null && lctn.Count > 0)
                {
                    sAddressData = new string[lctn.Count];
                    for (int i = 0; i < lctn.Count; i++)
                    {
                        adrs = lctn[i].Address;
                        string adrss = adrs.StreetAddress + ";" + adrs.City + ";" + adrs.State + ";" + adrs.Zip + ";" + adrs.Country;
                        _saddress.Add(adrss);
                        sAddressData = _saddress.Distinct().ToArray();
                        iRet = lctn.Count;
                    }
                }
            }
            return iRet;
        }
        /// <summary>
        /// Method to Add the Stop/Addresses
        /// </summary>
        /// <param name="sAddress">address string</param>
        /// <param name="oStop">object of clsAddress</param>
        /// <returns>No of Matches</returns>
        public long PCMSAddStop(string sAddress, clsAddress oStop)
        {
            long lRet = 0;
            // for this example we just had the address to the list
            // in the finished version we need to build the full route object
            // including key values from oStop
            if (APIRouteData == null) { APIRouteData = new List<string>(); }
            // APIRouteData.Add(sAddress);
            if (TrimbleRouteReports == null) { TrimbleRouteReports = new List<clsTrimbleReportParams>(); }
            IRestResponse response = null;
            string strAddress = HttpUtility.UrlEncode(sAddress);
            TrimbleServiceReference.Address adrs = new TrimbleServiceReference.Address();
            TrimbleServiceReference.Coordinates coords = new TrimbleServiceReference.Coordinates();
            List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
            List<clsAddress> lstostops = new List<clsAddress>();
            clsTrimbleReportParams objtrimbleRoute = new clsTrimbleReportParams();
            var client = new RestSharp.RestClient(SingleSearchWebServiceURL + "na/api/search?authToken=" + APIKey + "&query=" + strAddress);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            object result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);
            List<string> _saddress = new List<string>();
            if (result != null)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                lctn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.Location>>(values["Locations"].ToString());
                lctn = lctn.Distinct().ToList();
                if (lctn != null && lctn.Count > 0)
                {
                    sAddressData = new string[lctn.Count];
                    for (int i = 0; i < lctn.Count; i++)
                    {
                        adrs = lctn[i].Address;
                        string adrss = adrs.StreetAddress + ";" + adrs.City + ";" + adrs.State + ";" + adrs.Zip + ";" + adrs.Country;
                        oStop.strAddress = adrs.StreetAddress;
                        oStop.strCity = adrs.City;
                        oStop.strState = adrs.State;
                        oStop.strZip = adrs.Zip;
                        _saddress.Add(adrss);
                        sAddressData = _saddress.Distinct().ToArray();
                        lstostops.Add(oStop);
                        coords = lctn[i].Coords;
                        string scoords = coords.Lon + "," + coords.Lat;
                        APIRouteData.Add(scoords);
                        lRet = lctn.Count;
                        if (TrimbleRouteReports.Count > 0)
                        {
                            objtrimbleRoute.StopID = (TrimbleRouteReports.Count + 1);
                        }
                        else
                        {
                            objtrimbleRoute.StopID = (i + 1);
                        }
                        objtrimbleRoute.RouteOpt = Routeoptimization;
                        if (objtrimbleRoute.Stops == null) objtrimbleRoute.Stops = new List<string>();
                        objtrimbleRoute.Stops.Add(scoords);
                        TrimbleRouteReports.Add(objtrimbleRoute);
                    }
                }
            }
            return lRet;
        }

        /// <summary>
        /// Method to Add the Stop/Addresses
        /// </summary>
        /// <param name="sAddress">address string</param>
        /// <param name="oStop">object of clsFMStopData</param>
        /// <returns>No of Matches</returns>
        public long PCMSAddStop(string sAddress, clsFMStopData oStop)
        {
            long lRet = 0;

            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            // for this example we just had the address to the list
            // in the finished version we need to build the full route object
            // including key values from oStop
            if (APIRouteData == null) { APIRouteData = new List<string>(); }
            // APIRouteData.Add(sAddress);
            if (TrimbleRouteReports == null) { TrimbleRouteReports = new List<clsTrimbleReportParams>(); }
            IRestResponse response = null;
            string strAddress = HttpUtility.UrlEncode(sAddress);
            TrimbleServiceReference.Address adrs = new TrimbleServiceReference.Address();
            TrimbleServiceReference.Coordinates coords = new TrimbleServiceReference.Coordinates();
            List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
            List<clsFMStopData> lststops = new List<clsFMStopData>();
            clsTrimbleReportParams objtrimbleRoute = new clsTrimbleReportParams();
            var client = new RestSharp.RestClient(SingleSearchWebServiceURL + "na/api/search?authToken=" + APIKey + "&query=" + strAddress);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            object result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(response.Content);
            List<string> _saddress = new List<string>();
            if (result != null)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.ToString());
                lctn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.Location>>(values["Locations"].ToString());
                if (lctn != null && lctn.Count > 0)
                {
                    sAddressData = new string[lctn.Count];
                    for (int i = 0; i < lctn.Count; i++)
                    {
                        adrs = lctn[i].Address;
                        string adrss = adrs.StreetAddress + ";" + adrs.City + ";" + adrs.State + ";" + adrs.Zip + ";" + adrs.Country;
                        _saddress.Add(adrss);
                        sAddressData = _saddress.Distinct().ToArray();
                        coords = lctn[i].Coords;
                        string scoords = coords.Lon + "," + coords.Lat;
                        APIRouteData.Add(scoords);
                        lRet = lctn.Count;
                        if (TrimbleRouteReports.Count > 0)
                        {
                            objtrimbleRoute.StopID = (TrimbleRouteReports.Count + 1);
                        }
                        else
                        {
                            objtrimbleRoute.StopID = (i + 1);
                        }
                        objtrimbleRoute.RouteOpt = Routeoptimization;
                        if (objtrimbleRoute.Stops == null) objtrimbleRoute.Stops = new List<string>();
                        objtrimbleRoute.Stops.Add(scoords);
                        TrimbleRouteReports.Add(objtrimbleRoute);
                    }
                }
                if (APIRouteData != null && APIRouteData.Count > 0)
                {
                    APIRouteData = APIRouteData;  //APIRouteData will be optimized by route optimization parameters via API (for example practical
                    string _sCoords = string.Empty;
                    for (int i = 0; i < APIRouteData.Count; i++)
                    {
                        _sCoords += APIRouteData[i] + ";";
                    }
                    IRestResponse response1 = null;
                    string strCoordinates = HttpUtility.UrlEncode(_sCoords.Trim(';'));
                    List<ReportLine> _reportLines = new List<ReportLine>();
                    Address adrs1 = new Address();
                    List<TrimbleServiceReference.Location> lctn1 = new List<TrimbleServiceReference.Location>();
                    clsFMStopData[] stparr = new clsFMStopData[2];
                    clsFMStopData ostop = new clsFMStopData();
                    clsTrimbleReportParams objtrimbleRoute1 = new clsTrimbleReportParams();
                    Stop _stpObj = new Stop();
                    List<Stop> _lststp = new List<Stop>();
                    var client1 = new RestSharp.RestClient(WebServiceURL + "/route/routeReports?stops=" + strCoordinates + "&reports=Mileage&routeOpt" + Routeoptimization + "&routeType=" + RouteType + "&authToken=" + APIKey);//Need to Add Additional parameters.
                    client.Timeout = -1;
                    var request1 = new RestRequest(Method.GET);
                    response = client.Execute(request1);
                    var result1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(response.Content);
                    if (result1 != null)
                    {
                        var values1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(result1[0].ToString());
                        _reportLines = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportLine>>(values["ReportLines"].ToString());
                        if (_reportLines != null && _reportLines.Count > 0)
                        {
                            stparr = new clsFMStopData[_reportLines.Count];
                            for (int i = 0; i < _reportLines.Count; i++)
                            {

                                objtrimbleRoute.StopID = (i + 1);
                                objtrimbleRoute.RouteOpt = Routeoptimization;
                                _stpObj = _reportLines[i].Stop;
                                adrs1 = _stpObj.Address;
                                ostop.City = adrs.City;
                                ostop.SeqNumber = i;
                                ostop.StopNumber = (i + 1);
                                ostop.State = adrs.State;
                                ostop.StopName = adrs.StreetAddress;
                                ostop.Street = adrs.StreetAddress;
                                ostop.Zip = adrs.Zip;
                                _lststp.Add(_stpObj);
                                if (i > 0)
                                {

                                    ostop.LegCost = Convert.ToDouble(_reportLines[i].LCostMile);
                                    ostop.LegESTCHG = Convert.ToDouble(_reportLines[i].LEstghg);
                                    ostop.LegMiles = Convert.ToDouble(_reportLines[i].LMiles);
                                    ostop.LegTime = _reportLines[i].LHours;
                                    ostop.LegTolls = Convert.ToDouble(_reportLines[i].LTolls);
                                    ostop.TotalCost = Convert.ToDouble(_reportLines[i].TCostMile);
                                    ostop.TotalESTCHG = Convert.ToDouble(_reportLines[i].TEstghg);
                                    ostop.TotalMiles = Convert.ToDouble(_reportLines[i].TMiles);
                                    ostop.TotalTime = _reportLines[i].THours;
                                    ostop.TotalTolls = Convert.ToDouble(_reportLines[i].TTolls);

                                }
                                lststops.Add(ostop);
                            }

                            lRet = lststops.ToArray().Count();
                        }
                    }
                }
            }
            return lRet;
        }

        /// <summary>
        /// Creates a new stop object for address,  returns null if the address cannot be found, references to bad stop data and trimble stops found are available to the caller
        /// </summary>
        /// <param name="sStreetAddress"></param>
        /// <param name="sCity"></param>
        /// <param name="sState"></param>
        /// <param name="sZip"></param>
        /// <param name="sCountry"></param>
        /// <param name="sLabel"></param>
        /// <param name="sPlaceName"></param>
        /// <param name="lBadStops"></param>
        /// <param name="lMatchingStops"></param>
        /// <param name="eRegion"></param>
        /// <param name="sCounty"></param>
        /// <param name="sSPLC"></param>
        /// <param name="eCountryPostalFilter"></param>
        /// <param name="eAbbreviationFormat"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.4.0.003 on 07/06/2021
        ///   replaces PCMSAddStop
        /// </remarks>
        public NGLTrimbleServices.TrimbleServiceReference.StopLocation PCMCreateStop(
            string sStreetAddress,
            string sCity,
            string sState,
            string sZip,
            string sCountry,
            string sLabel,
            string sPlaceName,
            ref List<TrimbleServiceReference.StopLocation> lBadStops,
            ref List<TrimbleServiceReference.StopLocation> lMatchingStops,
            NGLTrimbleServices.TrimbleServiceReference.DataRegion eRegion = NGLTrimbleServices.TrimbleServiceReference.DataRegion.NA,
            string sCounty = "",
            string sSPLC = "",
            TrimbleServiceReference.PostCodeType eCountryPostalFilter = TrimbleServiceReference.PostCodeType.US,
            NGLTrimbleServices.TrimbleServiceReference.CountryAbbreviationType eAbbreviationFormat = NGLTrimbleServices.TrimbleServiceReference.CountryAbbreviationType.FIPS
            )
        {
            NGLTrimbleServices.TrimbleServiceReference.Address oAddress = new NGLTrimbleServices.TrimbleServiceReference.Address() ;
            oAddress.StreetAddress = sStreetAddress;
            oAddress.City = sCity;
            oAddress.State = sState;
            oAddress.Zip = sZip;
            oAddress.Country = sCountry;
            oAddress.County = sCounty;
            oAddress.SPLC = sSPLC;
            oAddress.CountryPostalFilter = eCountryPostalFilter;
            //oAddress.CountryAbbreviation = eAbbreviationFormat.ToString();

            return PCMCreateStop(oAddress, sLabel, sPlaceName, ref lBadStops, ref lMatchingStops, eRegion);        
        }

        /// <summary>
        /// Creates a new stop object for address,  returns null if the address cannot be found, references to bad stop data and trimble stops found are available to the caller
        /// </summary>
        /// <param name="oAddress"></param>
        /// <param name="sLabel"></param>
        /// <param name="sPlaceName"></param>
        /// <param name="lBadStops"></param>
        /// <param name="lMatchingStops"></param>
        /// <param name="eRegion"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.4.0.003 on 07/06/2021
        ///   replaces PCMSAddStop
        /// </remarks>
        public NGLTrimbleServices.TrimbleServiceReference.StopLocation PCMCreateStop(NGLTrimbleServices.TrimbleServiceReference.Address oAddress, string sLabel,string sPlaceName,
            ref List<TrimbleServiceReference.StopLocation> lBadStops,
            ref List<TrimbleServiceReference.StopLocation> lMatchingStops, NGLTrimbleServices.TrimbleServiceReference.DataRegion eRegion = NGLTrimbleServices.TrimbleServiceReference.DataRegion.NA)
        {
            NGLTrimbleServices.TrimbleServiceReference.StopLocation Stop = new NGLTrimbleServices.TrimbleServiceReference.StopLocation();
            // validate address
            if (lMatchingStops == null) { lMatchingStops = new List<TrimbleServiceReference.StopLocation>(); }
            List<TrimbleServiceReference.Address> lMatchingAddresses = new List<TrimbleServiceReference.Address>();
            short iValid = TRMLookup(oAddress,ref lMatchingAddresses);
            if (iValid == -1) {
                // no match 
                if (lBadStops == null) { lBadStops = new List<TrimbleServiceReference.StopLocation>(); }
                TrimbleServiceReference.StopLocation oBadStop = new TrimbleServiceReference.StopLocation();
                oBadStop.Address = oAddress;
                oBadStop.Label = sLabel;
                oBadStop.PlaceName = sPlaceName;
                oBadStop.Region = eRegion;
                lBadStops.Add(oBadStop);
                return null;
            } else if (iValid == 0)
            {
                if (lBadStops == null) { lBadStops = new List<TrimbleServiceReference.StopLocation>(); }
                TrimbleServiceReference.StopLocation oBadStop = new TrimbleServiceReference.StopLocation();
                oBadStop.Address = oAddress;
                oBadStop.Label = sLabel;
                oBadStop.PlaceName = sPlaceName;
                oBadStop.Region = eRegion;
                lBadStops.Add(oBadStop);
                TrimbleServiceReference.Address oZipAddress = new TrimbleServiceReference.Address();
                oZipAddress.Zip = oAddress.Zip;
                oZipAddress.CountryPostalFilter = oAddress.CountryPostalFilter;
                Stop.Address = oZipAddress;
            } else
            {
                Stop.Address = oAddress;
            }
            
            Stop.Label = sLabel;
            Stop.PlaceName = sPlaceName;
            Stop.Region = eRegion;
            return Stop; 
        }
        /// <summary>
        /// Method to clear the Address values
        /// </summary>
        public void PCMSClearStops()
        {
            APIRouteData = new List<string>();
            TrimbleRouteReports = new List<clsTrimbleReportParams>();
        }
        /// <summary>
        /// Method to Get the CalMiles based on routeType,RoutOptTypes,DistanceType,Vechicle Type.
        /// </summary>
        /// <returns></returns>
        public int PCMSOptimize()
        {
            int iRet = 0;

            if (APIRouteData != null && APIRouteData.Count > 0)
            {
                APIRouteData = APIRouteData;  //APIRouteData will be optimized by route optimization parameters via API (for example practical
                string _sCoords = string.Empty;
                for (int i = 0; i < APIRouteData.Count; i++)
                {
                    _sCoords += APIRouteData[i] + ";";
                }
                IRestResponse response = null;
                string strCoordinates = HttpUtility.UrlEncode(_sCoords.Trim(';'));
                List<ReportLine> _reportLines = new List<ReportLine>();
                Address adrs = new Address();
                List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
                List<clsAddress> lstostops = new List<clsAddress>();
                clsTrimbleReportParams objtrimbleRoute = new clsTrimbleReportParams();
                Stop _stpObj = new Stop();
                List<Stop> _lststp = new List<Stop>();
                var client = new RestSharp.RestClient(WebServiceURL + "/route/routeReports?stops=" + strCoordinates + "&reports=Mileage&routeOpt" + Routeoptimization + "&vehType="+ VechicleType + "&distUnits=" + DistanceType + "&overrideClass=" + OptFlag + "&routeType="+RouteType+ "&authToken=" + APIKey);//Need to Add Additional parameters.
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                response = client.Execute(request);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(response.Content);
                if (result != null)
                {
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result[0].ToString());
                    _reportLines = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportLine>>(values["ReportLines"].ToString());
                    if (_reportLines != null && _reportLines.Count > 0)
                    {
                        for (int i = 0; i < _reportLines.Count; i++)
                        {
                            if (TrimbleRouteReports.Count > 0)
                            {
                                objtrimbleRoute.StopID = (TrimbleRouteReports.Count + 1);
                            }
                            else
                            {
                                objtrimbleRoute.StopID = (i + 1);
                            }
                            _stpObj = _reportLines[i].Stop;
                            adrs = _stpObj.Address;
                            _lststp.Add(_stpObj);

                            objtrimbleRoute.RouteOpt = Routeoptimization;
                            objtrimbleRoute.Miles = _reportLines[i].TMiles;
                            TrimbleRouteReports.Add(objtrimbleRoute);
                        }
                    }
                    else
                        return -1;

                }
                else
                {
                    return -1;
                }
            }

            return iRet;
        }
        /// <summary>
        /// Method returns TotalMiles
        /// </summary>
        /// <returns>Miles</returns>
        public double PCMSCalculate()
        {
            double dMiles = 0;

            //if (APIRouteData != null && APIRouteData.Count > 0)
            //{
            //    dMiles = 1;  //dMiles will equal the route data returned by the API

            //}
            if (TrimbleRouteReports == null) { TrimbleRouteReports = new List<clsTrimbleReportParams>(); }
            else
            {
                return Convert.ToDouble(TrimbleRouteReports[TrimbleRouteReports.Count - 1].Miles);
            }
            return dMiles;

        }
       

        /// <summary>
        /// Method to Get the Coordinates based on Address
        /// </summary>
        /// <param name="sStreet">Street</param>
        /// <param name="sCity">City</param>
        /// <param name="sState">StateCountry</param>
        /// <param name="sPostCode">Zip Code</param>
        /// <returns></returns>
        private TrimbleServiceReference.Coordinates GetGeoCodingCoords(string sStreet, string sCity, string sState, string sCountry, string sPostCode)
        {
            IRestResponse response = null;
            TrimbleServiceReference.Coordinates lstPoints = new TrimbleServiceReference.Coordinates();

            string Street = HttpUtility.UrlEncode(sStreet);
            string City = HttpUtility.UrlEncode(sCity);
            string State = HttpUtility.UrlEncode(sState);
            var client = new RestSharp.RestClient(WebServiceURL + "locations?street=" + Street + "&city=" + City + "&state=" + State + "&country=" + sCountry + "&postcode=" + sPostCode + "&dataset=Current&authToken=" + APIKey);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            List<object> result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(response.Content).ToList();
            if (result.Count > 0)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result[0].ToString());
                lstPoints = Newtonsoft.Json.JsonConvert.DeserializeObject<TrimbleServiceReference.Coordinates>(values["Coords"].ToString());
            }
            return lstPoints;
        }

        /// <summary>
        /// Method to get the Total Miles based on Stops(Coordinates)
        /// </summary>
        /// <param name="sstops">GeoCoordinates</param>
        /// <returns>Total Miles</returns>
        public double PCMSCalculate(string sstops)

        {
            IRestResponse response = null;
            double miles = 0.0;

            string stops = HttpUtility.UrlEncode(sstops);
            var client = new RestSharp.RestClient(WebServiceURL + "route/routeReports?stops=" + stops + "&reports=CalcMiles&authToken=" + APIKey);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.CalculateMilesReport>>(response.Content);
            if (result.Count > 0)
            {
                miles = result[0].TMiles;
            }

            return miles;
        }
        /// <summary>
        /// Method to Calculate the distance based on origin and Destination
        /// </summary>
        /// <param name="sOrign">Origin Ardress</param>
        /// <param name="sDestination">Destion Address</param>
        /// <returns></returns>
        public double PCMSCalcDistance(string sOrign, string sDestination)
        {
            double dMiles = 0;
            TrimbleServiceReference.Coordinates sorignCoords = null;
            TrimbleServiceReference.Coordinates sdestnCoords = null;
            List <TrimbleServiceReference.Location> lOrig = TRMSearch(sOrign);
            if (lOrig != null && lOrig.Count() > 0)
            {
                sorignCoords = lOrig[0].Coords;
            } else
            {
                return 0;
            }
            List<TrimbleServiceReference.Location> lDest = TRMSearch(sDestination);
            if (lDest != null && lDest.Count() > 0)
            {
                sdestnCoords = lDest[0].Coords;
            }
            else
            {
                return 0;
            }
            //TrimbleServiceReference.Coordinates sorignCoords = GetGeoCodingCoords(sOrign,null,null,null,null);
            //TrimbleServiceReference.Coordinates sdestnCoords = GetGeoCodingCoords(sDestination, null, null, null, null);
            //Add API code here
            IRestResponse response = null;            
            string sstops = sorignCoords.Lon + "," + sorignCoords.Lat + ";" + sdestnCoords.Lon + "," + sdestnCoords.Lat;
            string stops = HttpUtility.UrlEncode(sstops);
            var client = new RestSharp.RestClient(WebServiceURL + "route/routeReports?stops=" + stops + "&reports=CalcMiles&authToken=" + APIKey);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            response = client.Execute(request);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrimbleServiceReference.CalculateMilesReport>>(response.Content);
            if (result.Count > 0)
            {
                dMiles = result[0].TMiles;
            }
            return dMiles;
        }
        /// <summary>
        /// Method to bind leginfo and to get the array of clsSimpleStop
        /// </summary>
        /// <param name="stopsarray">ref array of clsSimpleStop</param>
        /// <returns>legInfoType</returns>
        public legInfoType PCMSGetLegInfo(ref clsSimpleStop[] stopsarray)
        {
            legInfoType oleg = new legInfoType();
            clsSimpleStop oclsStop = new clsSimpleStop();
            int count = 0;
            clsSimpleStop[] oclsStopary = new clsSimpleStop[count];
            TrimbleRouteReports = new List<clsTrimbleReportParams>();
            if (APIRouteData != null && APIRouteData.Count > 0)
            {
                APIRouteData = APIRouteData;  //APIRouteData will be optimized by route optimization parameters via API (for example practical
                string _sCoords = string.Empty;
                for (int i = 0; i < APIRouteData.Count; i++)
                {
                    _sCoords += APIRouteData[i] + ";";
                }
                IRestResponse response = null;
                string strCoordinates = HttpUtility.UrlEncode(_sCoords.Trim(';'));
                List<ReportLine> _reportLines = new List<ReportLine>();
                Address adrs = new Address();
                List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
                List<clsAddress> lstostops = new List<clsAddress>();
                clsTrimbleReportParams objtrimbleRoute = new clsTrimbleReportParams();
                Stop _stpObj = new Stop();
                List<Stop> _lststp = new List<Stop>();
                var client = new RestSharp.RestClient(WebServiceURL + "/route/routeReports?stops=" + strCoordinates + "&reports=Mileage&routeOpt" + Routeoptimization + "&authToken=" + APIKey);//Need to Add Additional parameters?.
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                response = client.Execute(request);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(response.Content);
                if (result != null)
                {
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result[0].ToString());
                    _reportLines = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportLine>>(values["ReportLines"].ToString());
                    if (_reportLines != null && _reportLines.Count > 0)
                    {
                        count = _reportLines.Count;
                        oclsStopary = new clsSimpleStop[count];
                        for (int i = 0; i < _reportLines.Count; i++)
                        {
                            oclsStopary[i] = new clsSimpleStop();
                            objtrimbleRoute.StopID = (i + 1);
                            _stpObj = _reportLines[i].Stop;
                            adrs = _stpObj.Address;
                            _lststp.Add(_stpObj);
                            objtrimbleRoute.RouteOpt = Routeoptimization;
                            oclsStopary[i].Address = adrs.StreetAddress + "," + adrs.City + "," + adrs.State + "," + adrs.Zip + "," + adrs.Country;
                            if (i > 0)
                            {
                                objtrimbleRoute.Miles = _reportLines[i].TMiles;
                                TrimbleRouteReports.Add(objtrimbleRoute);
                                oleg.legMiles = _reportLines[i].LMiles;
                                oleg.legCost = _reportLines[i].LCostMile;
                                oleg.legHours = _reportLines[i].LHours.Replace(':', '.');
                                oleg.totMiles = _reportLines[i].TMiles;
                                oleg.totCost = _reportLines[i].TCostMile;
                                oleg.totHours = _reportLines[i].THours.Replace(':', '.');
                                oclsStopary[i].LegCost = Convert.ToDouble(_reportLines[i].LCostMile);
                                oclsStopary[i].LegHours = Convert.ToDouble(oleg.legHours);
                                oclsStopary[i].LegMiles = Convert.ToDouble(oleg.legMiles);
                                oclsStopary[i].StopNumber = objtrimbleRoute.StopID;
                                oclsStopary[i].TotalCost = Convert.ToDouble(oleg.totCost);
                                oclsStopary[i].TotalHours = Convert.ToDouble(oleg.totHours);
                                oclsStopary[i].TotalMiles = Convert.ToDouble(oleg.totMiles);
                            }
                        }
                        stopsarray = oclsStopary.ToArray();
                    }

                }

            }
            return oleg;
        }
        /// <summary>
        /// Method to Get the Number of Report Lines Array of clsPCMReportRecord
        /// </summary>
        /// <param name="reports">ref Array of clsPCMReportRecord</param>
        /// <param name="stoptype">True/false</param>
        public void PCMSNumRptLines(ref clsPCMReportRecord[] reports, bool stoptype)
        {
            clsPCMReportRecord oclsReport = new clsPCMReportRecord();
            int count = 0;
            clsPCMReportRecord[] oclsStopary = new clsPCMReportRecord[count];
            TrimbleRouteReports = new List<clsTrimbleReportParams>();
            if (APIRouteData != null && APIRouteData.Count > 0)
            {
                APIRouteData = APIRouteData;  //APIRouteData will be optimized by route optimization parameters via API (for example practical
                string _sCoords = string.Empty;
                for (int i = 0; i < APIRouteData.Count; i++)
                {
                    _sCoords += APIRouteData[i] + ";";
                }
                IRestResponse response = null;
                string strCoordinates = HttpUtility.UrlEncode(_sCoords.Trim(';'));
                List<ReportLine> _reportLines = new List<ReportLine>();                           
                Address adrs = new Address();
                List<TrimbleServiceReference.Location> lctn = new List<TrimbleServiceReference.Location>();
                List<clsAddress> lstostops = new List<clsAddress>();
                clsTrimbleReportParams objtrimbleRoute = new clsTrimbleReportParams();
                Stop _stpObj = new Stop();
                List<Stop> _lststp = new List<Stop>();
                var client = new RestSharp.RestClient(WebServiceURL + "/route/routeReports?stops=" + strCoordinates + "&reports=Mileage&routeOpt=" + Routeoptimization + "&routeType=" + RouteType + "&authToken=" + APIKey);//Need to Add Additional parameters.
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                response = client.Execute(request);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(response.Content);
                if (result != null)
                {
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result[0].ToString());
                    _reportLines = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportLine>>(values["ReportLines"].ToString());
                    if (_reportLines != null && _reportLines.Count > 0)
                    {
                        count = _reportLines.Count;
                        oclsStopary = new clsPCMReportRecord[count];
                        for (int i = 0; i < _reportLines.Count; i++)
                        {
                            oclsStopary[i] = new clsPCMReportRecord();
                            objtrimbleRoute.StopID = (i + 1);
                            objtrimbleRoute.RouteOpt = Routeoptimization;
                            _stpObj = _reportLines[i].Stop;
                            adrs = _stpObj.Address;
                            oclsStopary[i].City = adrs.City;
                            oclsStopary[i].SeqNumber = i;
                            if (Routeoptimization ==RouteOptimization.None.ToString()) {
                                oclsStopary[i].StopNumber = (i + 1);
                            }
                            else
                            {
                                oclsStopary[i].StopNumber = (count--);
                            }
                            oclsStopary[i].State = adrs.State;
                            oclsStopary[i].StopName = adrs.StreetAddress;
                            oclsStopary[i].Street = adrs.StreetAddress;
                            oclsStopary[i].Zip = adrs.Zip;
                            _lststp.Add(_stpObj);
                            if (i > 0)
                            {

                                oclsStopary[i].LegCost = Convert.ToDouble(_reportLines[i].LCostMile);
                                oclsStopary[i].LegESTCHG = Convert.ToDouble(_reportLines[i].LEstghg);
                                oclsStopary[i].LegMiles = Convert.ToDouble(_reportLines[i].LMiles);
                                oclsStopary[i].LegTime = _reportLines[i].LHours;
                                oclsStopary[i].LegTolls = Convert.ToDouble(_reportLines[i].LTolls);
                                oclsStopary[i].TotalCost = Convert.ToDouble(_reportLines[i].TCostMile);
                                oclsStopary[i].TotalESTCHG = Convert.ToDouble(_reportLines[i].TEstghg);
                                oclsStopary[i].TotalMiles = Convert.ToDouble(_reportLines[i].TMiles);
                                oclsStopary[i].TotalTime = _reportLines[i].THours;
                                oclsStopary[i].TotalTolls = Convert.ToDouble(_reportLines[i].TTolls);

                            }

                        }

                        reports = oclsStopary.ToArray();
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int PCMSGetError()
        {
            int errorCode = 0;
            return errorCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intErr"></param>
        /// <param name="buff"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string PCMSGetErrorString(int intErr, StringBuilder buff, int count = 255)
        {
            return buff.ToString();
        }

        public NGLTrimbleServices.TrimbleServiceReference.MileageReport TimeWindowOptimizer(NGLTrimbleServices.TrimbleServiceReference.StopLocation[] Stops)
        {
            string sRet = "";
            
            var request = (HttpWebRequest)WebRequest.Create("https://pcmiler.alk.com/APIs/REST/v1.0/Service.svc/route/routeReports?dataVersion=Current");
            //NGLTrimbleServices.TrimbleServiceReference.ReportRequestBody oBody = new NGLTrimbleServices.TrimbleServiceReference.ReportRequestBody();
            ////var ReportRoutes = new NGLTrimbleServices.TrimbleServiceReference.re
            //NGLTrimbleServices.TrimbleServiceReference.ReportRoute[] ReportRoutes = new NGLTrimbleServices.TrimbleServiceReference.ReportRoute[1];
            //ReportRoutes[0] = new NGLTrimbleServices.TrimbleServiceReference.ReportRoute();
            //ReportRoutes[0].RouteId = "test";
            //ReportRoutes[0].Stops = Stops;
            ////string jReportTypes = "\"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]"
            //NGLTrimbleServices.TrimbleServiceReference.ReportType[] oReportTypes = new NGLTrimbleServices.TrimbleServiceReference.ReportType[1];
            //oReportTypes[0] = new NGLTrimbleServices.TrimbleServiceReference.ReportType();
            //oReportTypes[0].__type = "MileageReportType: http://pcmiler.alk.com/APIs/v1.0"; // "MileageReportType: http://pcmiler.alk.com/APIs/v1.0\";
            //oReportTypes[0].THoursWithSeconds = false;
            //ReportRoutes[0].ReportTypes = oReportTypes;
            //oBody.ReportRoutes = ReportRoutes;
            ////oReportTypes[0] = new NGLTrimbleServices.TrimbleServiceReference.ReportType();
            ////oReportTypes[0].
            ////ReportRoutes[0].ReportTypes = new NGLTrimbleServices.TrimbleServiceReference.ReportType();
            //string sTypes = JsonConvert.SerializeObject(ReportRoutes[0].ReportTypes);
            //System.Diagnostics.Debug.WriteLine(sTypes);
            //\"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]}]}";
            //var postData = "{ \"ReportRoutes\": [{\"RouteId\": \"test\", \"Stops\": [{\"Address\": {\"Zip\": \"08540\"},\"Label\": \"Trimble MAPS\"}, {\"Address\": {\"Zip\": \"90266\"},\"Label\": \"Manhattan Beach\"}], \"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]}]}";
            string jStops = JsonConvert.SerializeObject(Stops);
            var postData = "{ \"ReportRoutes\": [{\"RouteId\": \"test\", \"Stops\":"  + jStops + ", \"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]}]}";

            var data = Encoding.ASCII.GetBytes(postData);
            request.Headers.Add("Authorization", APIKey);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseJSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //object result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(responseJSON);
            //NGLTrimbleServices.TrimbleServiceReference.MileageReport oData = new NGLTrimbleServices.TrimbleServiceReference.MileageReport();
            //oData = Newtonsoft.Json.JsonConvert.DeserializeObject<NGLTrimbleServices.TrimbleServiceReference.MileageReport>(values["ReportLines"].ToString());
            sRet = responseJSON.ToString();
            if (sRet.Contains("__type"))
            {
                sRet = sRet.Replace("__type", "type");
            }
                //"__type":"MileageReport:http://pcmiler.alk.com/APIs/v1.0", "ReportLines
                //if (sRet.Contains("__type")){
                //    int istart = sRet.IndexOf("_", 0);
                //    string sFirstpart = sRet.Substring(0, istart - 1);
                //    string sSecondpart = "";
                //    if (sRet.Contains(","))
                //    {
                //        int iEndPart = sRet.IndexOf(",", istart);
                //        sSecondpart = sRet.Substring(iEndPart +1 );
                //    }
                //    sRet = sFirstpart + sSecondpart;
                //}
                NGLTrimbleServices.TrimbleServiceReference.MileageReport[] oData = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<NGLTrimbleServices.TrimbleServiceReference.MileageReport[]>(sRet);
            //sRet = responseJSON.ToString();
            if (oData != null)
            {
                System.Diagnostics.Debug.WriteLine(oData.Count());
                System.Diagnostics.Debug.WriteLine(oData[0].ReportLines.Count());
            }

            return oData[0];
        }


        public NGLTrimbleServices.TrimbleServiceReference.MileageReport PCMMileageReport(NGLTrimbleServices.TrimbleServiceReference.StopLocation[] Stops, clsTrimbleReportParams oPar)
        {
            string sRet = "";

            var request = (HttpWebRequest)WebRequest.Create("https://pcmiler.alk.com/APIs/REST/v1.0/Service.svc/route/routeReports?dataVersion=Current");
            //NGLTrimbleServices.TrimbleServiceReference.ReportRequestBody oBody = new NGLTrimbleServices.TrimbleServiceReference.ReportRequestBody();
            ////var ReportRoutes = new NGLTrimbleServices.TrimbleServiceReference.re
            NGLTrimbleServices.TrimbleServiceReference.ReportRoute[] ReportRoutes = new NGLTrimbleServices.TrimbleServiceReference.ReportRoute[1];

            //ReportRoutes[0] = new NGLTrimbleServices.TrimbleServiceReference.ReportRoute();
            //NGLTrimbleServices.TrimbleServiceReference.ReportOptions opts = new NGLTrimbleServices.TrimbleServiceReference.ReportOptions();

            //opts.type
            //ReportRoutes[0].RouteId = "test";
            //ReportRoutes[0].Stops = Stops;
            ////string jReportTypes = "\"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]"
            //NGLTrimbleServices.TrimbleServiceReference.ReportType[] oReportTypes = new NGLTrimbleServices.TrimbleServiceReference.ReportType[1];
            //oReportTypes[0] = new NGLTrimbleServices.TrimbleServiceReference.ReportType();
            //oReportTypes[0].__type = "MileageReportType: http://pcmiler.alk.com/APIs/v1.0"; // "MileageReportType: http://pcmiler.alk.com/APIs/v1.0\";
            //oReportTypes[0].THoursWithSeconds = false;
            //ReportRoutes[0].ReportTypes = oReportTypes;
            //oBody.ReportRoutes = ReportRoutes;
            ////oReportTypes[0] = new NGLTrimbleServices.TrimbleServiceReference.ReportType();
            ////oReportTypes[0].
            ////ReportRoutes[0].ReportTypes = new NGLTrimbleServices.TrimbleServiceReference.ReportType();
            //string sTypes = JsonConvert.SerializeObject(ReportRoutes[0].ReportTypes);
            //System.Diagnostics.Debug.WriteLine(sTypes);
            //\"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]}]}";
            //var postData = "{ \"ReportRoutes\": [{\"RouteId\": \"test\", \"Stops\": [{\"Address\": {\"Zip\": \"08540\"},\"Label\": \"Trimble MAPS\"}, {\"Address\": {\"Zip\": \"90266\"},\"Label\": \"Manhattan Beach\"}], \"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]}]}";
            string jStops = JsonConvert.SerializeObject(Stops);
            //var postData = "{ \"ReportRoutes\": [{\"RouteId\": \"test\", \"Stops\":" + jStops + ", \"ReportingOptions\": {\"BordersOpen\": false,\"HubRouting\": false,\"RouteOptimization\": 1,\"RoutingType\": 0, }, \"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]}]}";
            //var postData = "{ \"ReportRoutes\": [{\"RouteId\": \"test\", \"Stops\":" + jStops + ", \"ReportingOptions\": {\"BordersOpen\": false,\"HubRouting\": false,\"RouteOptimization\": 1,\"RoutingType\": 0, }, \"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]}]}";
            //string sReportOptions = "\"ReportingOptions\": { \"UseTollData\": true,\"FuelUnits\": 0,\"RouteCosts\": {\"FuelEconomyLoaded\": 8.5,\"FuelEconomyEmpty\": 11.9,\"PricePerFuelUnit\": 13, \"TruckStyle\": 0,\"GreenHouseGas\": 5.2,\"OtherCostPerDistUnitLoaded\": 12.2,\"OtherCostPerDistanceUnitEmpty\": 8.9,\"CostTimeLoaded\": 15.5,\"CostTimeEmpty\": 12.6},\"RouteOptimization\": 1,\"RoutingType\": 0,\"TruckStyle\": 4}";
            //string sReportOptions = "\"ReportingOptions\": { \"UseTollData\": true,\"FuelUnits\": 0,\"RouteOptimization\": 1,\"RoutingType\": 0,\"TruckStyle\": 4}";
            if ( oPar == null) { oPar = new clsTrimbleReportParams(); }
            string sReportOptions = oPar.buildReportOptions(); // "\"Options\": { \"HubRouting\": true,\"UseTollData\": true,\"FuelUnits\": 0,\"RouteOptimization\": 1,\"RoutingType\": 0,\"TruckStyle\": 4 }";

            var postData = "{ \"ReportRoutes\": [{" + sReportOptions + ",\"RouteId\": \"test\", \"Stops\":" + jStops + ", \"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false}]}]}";
            //var postData = "{ \"ReportRoutes\": [{" + sReportOptions + ",\"RouteId\": \"test\", \"Stops\":" + jStops + ", \"ReportTypes\": [{\"__type\": \"MileageReportType:http://pcmiler.alk.com/APIs/v1.0\",\"THoursWithSeconds\": false},{\"__type\": \"CalculateMilesReportType:http://pcmiler.alk.com/APIs/v1.0\"}]}]}";

            // "__type": "CalculateMilesReportType:http://pcmiler.alk.com/APIs/v1.0
            //reports=Mileage&routeType=Practical&overrideClass=FiftyThreeFoot&avoidTolls=true&openBorders=false&routeOpt=1
            var data = Encoding.ASCII.GetBytes(postData);
            request.Headers.Add("Authorization", oPar.APIKey);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseJSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //object result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(responseJSON);
            //NGLTrimbleServices.TrimbleServiceReference.MileageReport oData = new NGLTrimbleServices.TrimbleServiceReference.MileageReport();
            //oData = Newtonsoft.Json.JsonConvert.DeserializeObject<NGLTrimbleServices.TrimbleServiceReference.MileageReport>(values["ReportLines"].ToString());
            sRet = responseJSON.ToString();
            if (sRet.Contains("__type"))
            {
                sRet = sRet.Replace("__type", "type");
            }
            //"__type":"MileageReport:http://pcmiler.alk.com/APIs/v1.0", "ReportLines
            //if (sRet.Contains("__type")){
            //    int istart = sRet.IndexOf("_", 0);
            //    string sFirstpart = sRet.Substring(0, istart - 1);
            //    string sSecondpart = "";
            //    if (sRet.Contains(","))
            //    {
            //        int iEndPart = sRet.IndexOf(",", istart);
            //        sSecondpart = sRet.Substring(iEndPart +1 );
            //    }
            //    sRet = sFirstpart + sSecondpart;
            //}
            NGLTrimbleServices.TrimbleServiceReference.MileageReport[] oData = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<NGLTrimbleServices.TrimbleServiceReference.MileageReport[]>(sRet);
            //sRet = responseJSON.ToString();
            if (oData != null)
            {
                System.Diagnostics.Debug.WriteLine(oData.Count());
                System.Diagnostics.Debug.WriteLine(oData[0].ReportLines.Count());
            }

            return oData[0];
        }


    }

}
