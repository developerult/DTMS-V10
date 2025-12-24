using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RestSharp;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using PCM = NGLTrimbleServices.TMSPCMWrapper;
using static NGLTrimbleServices.TMSPCMWrapper;

namespace NGLTrimbleServices
{
      
    public class PCMiles
    {
        public PCMiles(bool bUseTLs12 = true)
        {
            blnUseTLs12 = bUseTLs12;
            if (bUseTLs12)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            }
        }
        public bool blnUseTLs12 = true;
        public string _strLastError = "";
        public string gLastError = "";

        public string LastError
        {
            get
            {
                return _strLastError + gLastError;
            }
            set { }
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
        private string getErrorMessage(ref Exception ex)
        {
            string strRet = "";
            if (this.Debug)
                strRet = ex.ToString();
            else
                strRet = ex.Message;
            return strRet;
        }

        public NGLTrimbleServices.clsTrimbleReportParams oPar { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="strSource"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.0.002 on 01/03/2022 added new error messages
        /// </remarks>
        private string formatWebException(Exception ex, string strSource)
        {
            if (ex.Message.Contains("400")) {
                return "Your " + strSource + " request is not valid or has bad information.";
            } else
            {        
            return "The PCMiler " + strSource + " procedure is not available." + System.Environment.NewLine +
            "There was a problem connecting to " + this.WebServiceURL + System.Environment.NewLine +
            "Please check your company level parameter settings.  The actual error message is: " + System.Environment.NewLine + getErrorMessage(ref ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityZip"></param>
        /// <param name="LoggingOn"></param>
        /// <param name="LogFileName"></param>
        /// <returns></returns>
        public string CityToLatLong(ref string cityZip, bool LoggingOn, string LogFileName)
        {
            string strRet = "";
            PCM oPCMW = new PCM(blnUseTLs12);
            _strLastError = "";
            gLastError = "";
            try
            {
               
                string strLastError = "";
                strRet = oPCMW.CityToLatLong(ref cityZip);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "CityToLatLong");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute CityToLatLong. " + getErrorMessage(ref ex);
            }
            return strRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sAddress"></param>
        /// <param name="dblLat"></param>
        /// <param name="dblLong"></param>
        /// <param name="LoggingOn"></param>
        /// <param name="LogFileName"></param>
        /// <returns></returns>
        public bool getGeoCode(string sAddress, ref double dblLat, ref double dblLong)
        {           
            bool blnRet = false;
            PCM oPCMW = new PCM(blnUseTLs12);
            _strLastError = "";
            gLastError = "";
            try
            {
               
                string strLastError = "";
                oPCMW.APIKey = APIKey;
                blnRet = oPCMW.getGeoCode(sAddress, ref dblLat, ref dblLong);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "getGeoCode");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute getGeoCode. " + getErrorMessage(ref ex);
            }
            return blnRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dblLat"></param>
        /// <param name="dblLong"></param>
        /// <param name="LoggingOn"></param>
        /// <param name="LogFileName"></param>
        /// <param name="Islocation"></param>
        /// <returns></returns>
        public bool getGeoCode(string location, ref double dblLat, ref double dblLong, bool LoggingOn, string LogFileName,bool Islocation)
        {
            bool blnRet = false;
            PCM oPCMW = new PCM(blnUseTLs12);
            _strLastError = "";
            gLastError = "";
            try
            {
                string strLastError = "";
                oPCMW.APIKey = APIKey;
                blnRet = oPCMW.getGeoCode(location, ref dblLat, ref dblLong);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "getGeoCode");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute getGeoCode. " + getErrorMessage(ref ex);
            }
            return blnRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Latlong"></param>
        /// <param name="LoggingOn"></param>
        /// <param name="LogFileName"></param>
        /// <returns></returns>
        public string LatLongToCity(string Latlong, bool LoggingOn, string LogFileName)
        {
            string strRet = "";
            PCM oPCMW = new PCM(blnUseTLs12);
            _strLastError = "";
            gLastError = "";
            try
            {               
                string strLastError = "";
                strRet = oPCMW.LatLongToCity(Latlong);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "LatLongToCity");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute LatLongToCity. " + getErrorMessage(ref ex);
            }
            return strRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CityNameOrZipCode"></param>
        /// <param name="LoggingOn"></param>
        /// <param name="LogFileName"></param>
        /// <returns></returns>
        public string FullName(string CityNameOrZipCode, bool LoggingOn, string LogFileName)
        {
            string strRet = "";
            PCM oPCMW = new PCM(blnUseTLs12);
            _strLastError = "";
            gLastError = "";
            try
            {
               
                string strLastError = "";
                strRet = oPCMW.FullName(CityNameOrZipCode);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "FullName");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute FullName. " + getErrorMessage(ref ex);
            }
            return strRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postalCode"></param>
        /// <param name="LoggingOn"></param>
        /// <param name="LogFileName"></param>
        /// <returns></returns>
        public clsAddress[] cityStateZipLookup(string postalCode, bool LoggingOn, string LogFileName)
        {            
            List<clsAddress> oRet = new List<clsAddress>();
            PCM oPCMW = new PCM(blnUseTLs12);
            try
            {
               
                clsAddress[] oData = oPCMW.cityStateZipLookup(postalCode);               
                if (oData != null && oData.Length > 0)
                {
                    foreach (clsAddress a in oData)
                        oRet.Add(new clsAddress() { strAddress = a.strAddress, strCity = a.strCity, strState = a.strState, strZip = a.strZip });
                }
            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "cityStateZipLookup");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute cityStateZipLookup. " + getErrorMessage(ref ex);
            }
            if (oRet != null && oRet.Count > 0)
                return oRet.ToArray();
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strAddress"></param>
        /// <returns></returns>
        public bool PCMValidateAddress(string strAddress)
        {
            _strLastError = "";
            gLastError = "";
            bool blnRet = false;
            try
            {
                PCM oPCMW = new PCM(blnUseTLs12);
                oPCMW.APIKey = this.APIKey;
                string strLastError = "";
                blnRet = oPCMW.PCMValidateAddress(strAddress);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            // End If
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "PCMValidateAddress");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute PCMValidateAddress. " + getErrorMessage(ref ex);
            }
            return blnRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objOrig"></param>
        /// <param name="objDest"></param>
        /// <param name="Route_Type"></param>
        /// <param name="Dist_Type"></param>
        /// <param name="EXCALC_Type"></param>
        /// <param name="EXOpt_Flags"></param>
        /// <param name="EXVeh_Type"></param>
        /// <param name="intCompControl"></param>
        /// <param name="intBookControl"></param>
        /// <param name="intLaneControl"></param>
        /// <param name="strItemNumber"></param>
        /// <param name="strItemType"></param>
        /// <param name="dblAutoCorrectBadLaneZipCodes"></param>
        /// <param name="dblBatchID"></param>
        /// <param name="blnBatch"></param>
        /// <param name="arrBaddAddresses"></param>
        /// <returns></returns>
        public clsAllStops getPracticalMiles(string sAPIKey, clsAddress objOrig, clsAddress objDest, PCMEX_Route_Type Route_Type, 
            PCMEX_Dist_Type Dist_Type, PCMEX_CALCTYPE EXCALC_Type, PCMEX_Opt_Flags EXOpt_Flags, PCMEX_Veh_Type EXVeh_Type, 
            int intCompControl, int intBookControl, int intLaneControl, string strItemNumber, string strItemType, 
            double dblAutoCorrectBadLaneZipCodes, double dblBatchID, bool blnBatch, ref clsPCMBadAddress[] arrBaddAddresses)
        {
            clsAllStops oclsAllStops = new clsAllStops();
            //clsGlobalStopData oclsAllStops = new clsGlobalStopData(); // null/* TODO Change to default(_) if this is not a reference type */;            
            oclsAllStops.BadAddressCount = 0;
            oclsAllStops.FailedAddressMessage = "";
            oclsAllStops.BatchID = dblBatchID;
            oclsAllStops.AutoCorrectBadLaneZipCodes = dblAutoCorrectBadLaneZipCodes;
            oclsAllStops.OriginZip = objOrig.strZip;
            oclsAllStops.DestZip = objDest.strZip;
            _strLastError = "";
            gLastError = "";
            PCM oPCMW = new PCM(blnUseTLs12);
            List<NGLTrimbleServices.TrimbleServiceReference.StopLocation> lBadAddresses = new List<NGLTrimbleServices.TrimbleServiceReference.StopLocation>();
            List<NGLTrimbleServices.TrimbleServiceReference.StopLocation> lMatchingAddresses = new List<NGLTrimbleServices.TrimbleServiceReference.StopLocation>();
            
            try
            {              
                //clsPCMBadAddress[] arrBadAddresses;
                TrimbleAPI TRM = new TrimbleAPI();
                NGLTrimbleServices.TrimbleServiceReference.StopLocation[] Stops = new NGLTrimbleServices.TrimbleServiceReference.StopLocation[2];
                bool bInvalidRoute = false;
                string sFailedAddressWarning = "";
                Stops[0] = TRM.PCMCreateStop(objOrig.strAddress, objOrig.strCity, objOrig.strState, objOrig.strZip, objOrig.strCountry, "Orig", "", ref lBadAddresses, ref lMatchingAddresses);
                if (Stops[0] == null) {
                    sFailedAddressWarning = "The origin address is not valid. ";
                    bInvalidRoute = true; 
                }
                Stops[1] = TRM.PCMCreateStop(objDest.strAddress, objDest.strCity, objDest.strState, objDest.strZip, objDest.strCountry, "Dest", "", ref lBadAddresses, ref lMatchingAddresses);
                if (Stops[1] == null) {
                    sFailedAddressWarning = sFailedAddressWarning +  " The destination address is not valid. ";
                    bInvalidRoute = true;
                }
                if (!bInvalidRoute)
                {                    
                    oPar = new NGLTrimbleServices.clsTrimbleReportParams();
                    oPar.HubRouting = false;
                    oPar.UseTollData = true;
                    oPar.FuelUnits = 0;
                    oPar.RouteOptimization = 0;
                    oPar.RouteType = (int)Route_Type;
                    switch (EXOpt_Flags){
                        case PCMEX_Opt_Flags.FiftyThreeFoot:
                            oPar.TruckStyle = NGLTrimbleServices.TrimbleServiceReference.TruckStyle.FiftyThreeSemiTrailer;
                            break;
                        case PCMEX_Opt_Flags.None:
                            oPar.TruckStyle = NGLTrimbleServices.TrimbleServiceReference.TruckStyle.FortyEightSemiTrailer;
                            break;
                        default:
                            oPar.TruckStyle = NGLTrimbleServices.TrimbleServiceReference.TruckStyle.FiftyThreeSemiTrailer;
                            break;
                    }
                    oPar.DistanceUnits = (int)Dist_Type;
                    oPar.APIKey = sAPIKey;
                    
                    NGLTrimbleServices.TrimbleServiceReference.MileageReport oData = TRM.PCMMileageReport(Stops, oPar);
                    
                    if (oData != null)
                    {
                        // we loop through all the lines to get the total miles
                        // for now the StopData is not used but this may be needed 
                        // in the future.  this logic is stil being evaluated.  
                        List<clsFMStopData> StopData = new List<clsFMStopData>();
                        foreach (NGLTrimbleServices.TrimbleServiceReference.StopReportLine line in oData.ReportLines)
                        {
                            double dblBlank = 0;
                            clsFMStopData Stop = new clsFMStopData();
                            Stop.BookControl = intBookControl;
                            Stop.Street = line.Stop.Address.StreetAddress;
                            if (double.TryParse(line.LCostMile, out dblBlank))
                            {
                                Stop.LegCost = dblBlank;
                            }
                            if (double.TryParse(line.LMiles, out dblBlank))
                            {
                                Stop.LegMiles = dblBlank;
                            }
                            Stop.LegTime = line.LHours;
                            if (double.TryParse(line.LTolls, out dblBlank))
                            {
                                Stop.LegTolls = dblBlank;
                            }
                            if (lBadAddresses != null && lBadAddresses.Count() > 0)
                            {
                                Stop.LogBadAddress = true;
                            }else
                            {
                                Stop.LogBadAddress = false;
                            }
                            TrimbleServiceReference.StopLocation pcmStop = lMatchingAddresses.Where(c => c.Label == line.Stop.Label).FirstOrDefault(); 
                            
                            if (pcmStop != null)
                            {
                                Stop.PCMilerCity = pcmStop.Address.City;
                                Stop.PCMilerState = pcmStop.Address.State;
                                Stop.PCMilerStreet = pcmStop.Address.StreetAddress;
                                Stop.PCMilerZip = pcmStop.Address.Zip;
                                Stop.PCMilerCountry = pcmStop.Address.Country;
                            }

                            Stop.TotalCost = double.Parse(line.TCostMile);
                            Stop.TotalMiles = double.Parse(line.TMiles); 
                            Stop.TotalTime = line.THours; 
                            Stop.TotalTolls = double.Parse(line.TTolls);
                            StopData.Add(Stop);
                        }
                        
                        oclsAllStops.TotalMiles = StopData.Sum(c => c.LegMiles);
                    }
                    
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Found " + lBadAddresses.Count().ToString() + " bad addresses");
                }
                if (lBadAddresses != null && lBadAddresses.Count() > 0)
                {                   
                    clsAddress objPCMOrig = new clsAddress();
                    clsAddress objPCMDest = new clsAddress();

                    NGLTrimbleServices.TrimbleServiceReference.StopLocation PCMStop = lMatchingAddresses.Where(c => c.Label == "Orig").FirstOrDefault();
                    if (PCMStop != null && PCMStop.Address != null)
                    {
                        objPCMOrig.strAddress = PCMStop.Address.StreetAddress;
                        objPCMOrig.strCity = PCMStop.Address.City;
                        objPCMOrig.strState = PCMStop.Address.State;
                        objPCMOrig.strZip = PCMStop.Address.Zip;
                        objPCMOrig.strCountry = PCMStop.Address.Country;
                    }
                    NGLTrimbleServices.TrimbleServiceReference.StopLocation PCMStopDest = lMatchingAddresses.Where(c => c.Label == "Dest").FirstOrDefault();
                    if (PCMStopDest != null && PCMStopDest.Address != null)
                    {
                        objPCMDest.strAddress = PCMStopDest.Address.StreetAddress;
                        objPCMDest.strCity = PCMStopDest.Address.City;
                        objPCMDest.strState = PCMStopDest.Address.State;
                        objPCMDest.strZip = PCMStopDest.Address.Zip;
                        objPCMDest.strCountry = PCMStopDest.Address.Country;
                    }
                    sFailedAddressWarning = sFailedAddressWarning + " Using Postal Code for routing";
                    GlobalSettings.AddBaddAddressToArray(intBookControl, intLaneControl, objOrig, objDest, objPCMOrig, objPCMDest, "Using PCM Address or Zip Code for routing", dblBatchID, ref arrBaddAddresses);
                }

                oclsAllStops.FailedAddressMessage = sFailedAddressWarning;
            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "getPracticalMiles");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute getPracticalMiles. " + getErrorMessage(ref ex);
            }
            return oclsAllStops;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objOrig"></param>
        /// <param name="objDest"></param>
        /// <param name="Route_Type"></param>
        /// <param name="Dist_Type"></param>
        /// <param name="intCompControl"></param>
        /// <param name="intBookControl"></param>
        /// <param name="intLaneControl"></param>
        /// <param name="strItemNumber"></param>
        /// <param name="strItemType"></param>
        /// <param name="dblAutoCorrectBadLaneZipCodes"></param>
        /// <param name="dblBatchID"></param>
        /// <param name="blnBatch"></param>
        /// <param name="arrBaddAddresses"></param>
        /// <param name="loggin"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public clsGlobalStopData getPracticalMiles(clsAddress objOrig, clsAddress objDest, PCMEX_Route_Type Route_Type,
            PCMEX_Dist_Type Dist_Type, 
            int intCompControl, int intBookControl, int intLaneControl, string strItemNumber, string strItemType,
            double dblAutoCorrectBadLaneZipCodes, double dblBatchID,
            bool blnBatch, ref clsPCMBadAddress[] arrBaddAddresses,bool loggin,string logFile)
        {
            clsGlobalStopData oclsAllStops = null/* TODO Change to default(_) if this is not a reference type */;
            _strLastError = "";
            gLastError = "";
            PCM oPCMW = new PCM(blnUseTLs12);
            try
            {
                string strLastError = "";
                clsPCMBadAddress[] arrBadAddresses;
                clsAddress oOrig = new clsAddress();
                {
                    var withBlock = oOrig;
                    withBlock.strAddress = objOrig.strAddress;
                    withBlock.strCity = objOrig.strCity;
                    withBlock.strState = objOrig.strState;
                    withBlock.strZip = objOrig.strZip;
                }
                clsAddress oDest = new clsAddress();
                {
                    var withBlock = oDest;
                    withBlock.strAddress = objDest.strAddress;
                    withBlock.strCity = objDest.strCity;
                    withBlock.strState = objDest.strState;
                    withBlock.strZip = objDest.strZip;
                }
                clsGlobalStopData oGlobalStopData = oPCMW.getPracticalMilesEx(objOrig, objDest, Route_Type, Dist_Type, 
                    intCompControl, intBookControl, intLaneControl,  strItemNumber,  strItemType, dblAutoCorrectBadLaneZipCodes,
                    dblBatchID, blnBatch, ref arrBaddAddresses,true,"");
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;


            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "getPracticalMiles");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute getPracticalMiles. " + getErrorMessage(ref ex);
            }
            return oclsAllStops;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrFMStops"></param>
        /// <param name="dblBatchID"></param>
        /// <param name="blnKeepStopNumbers"></param>
        /// <param name="arrPCMReportRecords"></param>
        /// <returns></returns>
        public clsGlobalStopData PCMReSyncMultiStop(ref clsFMStopData[] arrFMStops, double dblBatchID, bool blnKeepStopNumbers, ref clsPCMReportRecord[] arrPCMReportRecords)
        {
            _strLastError = "";
            gLastError = "";
            clsGlobalStopData _clsstopObj = new clsGlobalStopData();
            try
            {
                PCM oPCMW = new PCM(blnUseTLs12);
                string strLastError = "";
                _clsstopObj = oPCMW.PCMReSyncMultiStop(ref arrFMStops, dblBatchID, blnKeepStopNumbers, ref arrPCMReportRecords);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            // End If
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "PCMReSyncMultiStop");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute PCMReSyncMultiStop. " + getErrorMessage(ref ex);
            }
            return _clsstopObj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRoute"></param>
        /// <returns></returns>
        public clsPCMReturn getRouteMiles(ref clsSimpleStop[] sRoute)
        {
            _strLastError = "";
            gLastError = "";
            clsPCMReturn _clsstopObj = new clsPCMReturn();
            try
            {
                PCM oPCMW = new PCM(blnUseTLs12);
                string strLastError = "";
                oPCMW.APIKey = APIKey;
                _clsstopObj = oPCMW.getRouteMiles(ref sRoute);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            // End If
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "getRouteMiles");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute getRouteMiles. " + getErrorMessage(ref ex);
            }
            return _clsstopObj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrStopData"></param>
        /// <param name="strConsNumber"></param>
        /// <param name="dblBatchID"></param>
        /// <param name="blnKeepStopNumbers"></param>
        /// <param name="arrAllStops"></param>
        /// <param name="arrBaddAddresses"></param>
        /// <returns></returns>
        public clsGlobalStopData PCMReSync(clsPCMDataStop[] arrStopData, string strConsNumber, double dblBatchID, bool blnKeepStopNumbers, ref clsAllStop[] arrAllStops, ref clsPCMBadAddress[] arrBaddAddresses)
        {
            _strLastError = "";
            gLastError = "";           
            clsGlobalStopData _clsstopObj = new clsGlobalStopData();
            try
            {
                PCM oPCMW = new PCM(blnUseTLs12);
                string strLastError = "";
                _clsstopObj = oPCMW.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, ref arrAllStops, ref arrBaddAddresses);
                if (!string.IsNullOrEmpty(strLastError))
                    _strLastError = strLastError;
            }
            // End If
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "PCMReSync");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute PCMReSync. " + getErrorMessage(ref ex);
            }
            return _clsstopObj;
        }

        public double Miles(string Origin, string Destination)
        {
            _strLastError = "";
            gLastError = "";
            try
            {
                PCM oPCMW = new PCM(blnUseTLs12);
                oPCMW.APIKey = this.APIKey;
                return oPCMW.Miles(Origin, Destination);
            }
            catch (System.Net.WebException ex)
            {
                _strLastError = formatWebException(ex, "Miles");
            }
            catch (Exception ex)
            {
                _strLastError = "Cannot execute Miles. " + getErrorMessage(ref ex);
            }
            return 0;
            
        }

    }
} 
