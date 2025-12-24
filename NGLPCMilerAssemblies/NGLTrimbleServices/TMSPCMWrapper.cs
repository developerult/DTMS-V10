using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.EnterpriseServices;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using Microsoft.VisualBasic;

namespace NGLTrimbleServices
{
    public class TMSPCMWrapper
    {
        #region " constructors"


        public TMSPCMWrapper(bool bUseTLs12 = true)
        {
            blnUseTLs12 = bUseTLs12;            
            _dtObjectCreateDate = DateTime.Now;
        }

        #endregion

        #region " CONSTANTS"

        // Routing calculation types
        private const short CALC_PRACTICAL = 0;
        private const short CALC_SHORTEST = 1;
        private const short CALC_NATIONAL = 2;
        private const short CALC_AVOIDTOLL = 3;
        private const short CALC_AIR = 4;
        private const short CALC_53FOOT = 6;

        // Report types
        private const short RPT_DETAIL = 0;
        private const short RPT_STATE = 1;
        private const short RPT_MILEAGE = 2;

        // Distance types
        private const short DIST_TYPE_MILES = 0;
        private const short DIST_TYPE_KILO = 1;


        #endregion

        #region " Enums"

        // Note:  these should be made public (maybe iside of Interfaces?)
        public enum PCMEX_Route_Type : int
        {
            //ROUTE_TYPE_PRACTICAL = 0,
            //ROUTE_TYPE_SHORTEST = 1,
            //ROUTE_TYPE_FASTEST = 2
            //ROUTE_TYPE_NATIONAL = 2,
            //ROUTE_TYPE_AVOIDTOLL = 3,
            //ROUTE_TYPE_AIR = 4,
            //ROUTE_TYPE_53FOOT = 6

            Practical = 0,// (Default)
            Shortest = 1,
            Fastest = 2
        }

        /// <summary>
        /// Extended Routing Calculation Types
        /// </summary>
        /// <remarks>
        ///Created by RHR for v-7.0.6.101 on 2/9/2016
        ///Replaces PCMEX_Route_Type when not CALCTYPE_NONE
        ///</remarks>
        public enum PCMEX_CALCTYPE : int
        {
            CALCTYPE_NONE = 0,
            CALCTYPE_PRACTICAL = 1,
            CALCTYPE_SHORTEST = 2,
            CALCTYPE_AIR = 4
        }

        /// <summary>
        ///     ''' PCMSSetCalcTypeEx specific Routing Options
        /// </summary>
        ///<remarks>
        ///     ''' Created by RHR for v-7.0.6.101 on 2/9/2016
        ///     '''   Support For PCMSSetCalcTypeEx see NGL_Opt_Flags for 
        ///     '''   parameter conversion reference
        /// </remarks>
        public enum PCMEX_Opt_Flags : int
        {
            //OPT_NONE = 0,
            //OPT_AVOIDTOLL = 256,
            //OPT_NATIONAL = 512,
            //OPT_FIFTYTHREE = 1024
            None = 0,//(Default)
            FiftyThreeFoot = 1,
            NationalNetwork = 2,
            Both = 3
        }


        /// <summary>
        ///     ''' PCMSSetCalcTypeEx specific Vehicle Options
        ///</summary>
        ///<remarks>
        ///     ''' Created by RHR for v-7.0.6.101 on 2/9/2016
        ///     '''   Support For PCMSSetCalcTypeEx as of 
        ///     '''   v-7.0.6.101 we only support trucks
        ///     '''   CALCEX_VEH_TRUCK
        ///</remarks>
        public enum PCMEX_Veh_Type : int
        {
            //CALCEX_VEH_TRUCK = 0,            
            ////CALCEX_VEH_AUTO = 16777216
            //CALCEX_VEH_AUTO = 2
            Truck = 0,// (Default)
            LightTruck = 1,
            Auto = 2
        }

        /// <summary>
        ///     ''' Support for PCMSSetCalcTypeEx Routing Options.  M be converted to PCMEX_Opt_Flags value before sending to PCMiler
        /// </summary>
        ///<remarks>
        ///     ''' Created by RHR for v-7.0.6.101 on 2/9/2016
        ///     '''   References the NGL PCMilerRouteOption global
        ///     '''   parameter.  must be converted to PCMEX_Opt_Flags
        ///     '''   before sending to PCMiler
        ///</remarks>
        public enum NGL_Opt_Flags : int
        {
            NGL_NONE = 0,
            //NGL_AVOIDTOLL = 1,
            //NGL_NATIONAL = 2,
            //NGL_FIFTYTHREE = 3
            NGL_FIFTYTHREE = 1,
            NGL_NATIONAL = 2,
            BOTH = 3//National and fiftythree
        }

        public enum HazMatType : int
        {
            None = 0,//(Default)
            General = 1,
            Caustic = 2,
            Explosives = 3,
            Flammable = 4,
            Inhalants = 5,
            Radioactive = 6,
            HarmfulToWater = 7,
            Tunnel = 8
        }

        public enum region : int
        {
            Unknown = 0,
            AF = 1,
            AS = 2,
            EU = 3,
            NA = 4,//(Default)
            OC = 5,
            SA = 6,
            ME = 7
        }

        public enum TruckStyle : int
        {
            None = 0,//(Default)
            TwentyEightDoubleTrailer = 1,
            FortyStraightTruck = 2,
            FortyEightSemiTrailer = 3,
            FiftyThreeSemiTrailer = 4,
            FullSizeVan = 5,
            TwentySixStraightTruck = 6,
            ConventionalSchoolBus = 7,//(North America only)
            SmallSchoolBus = 8//(North America only)
        }
        public enum Language : int
        {
            ENUS = 0,//(U.S. English)
            ENGB = 1,//Great Britain English)
            DE = 2,//German
            FR = 3,//French
            ES = 4,//Spanish
            IT = 5,//Italian            
        }
        public enum vehDimUnits : int
        {
            English = 0,//Default
            Metric = 1,
        }
        public enum RouteOptimization : int
        {
            None = 0,//(Default)
            ThruAll = 1,
            DestinationFixed = 2

        }
        public enum PCMEX_Dist_Type : int
        {
            Miles = 0, //(Default)
            Km = 1
        }



        #endregion

        #region " Properties"

        public bool blnUseTLs12 = true;

        public string ConnectionString { get; set; }
        public short ServerID { get; set; }

        private bool _blnDebug = false;
        public bool Debug
        {
            get
            {
                return _blnDebug;
            }
            set
            {
                _blnDebug = value;
            }
        }

        private string _strLastError = "";
        public string LastError
        {
            get
            {
                return _strLastError + GlobalSettings.gLastError;
            }
        }

        private bool _blnLoggingOn = false;
        public bool LoggingOn
        {
            get
            {
                return _blnLoggingOn;
            }
            set
            {
                _blnLoggingOn = value;
            }
        }

        private System.IO.StreamWriter _IOLogStream;
        private clsLog _oLog;
        public clsLog oLog
        {
            get
            {
                if (_oLog == null)
                {
                    _oLog = new clsLog();
                }
                return _oLog;
            }
            set
            {
                _oLog = value;
            }
        }

        private string _strLogFileName = @"C:\Data\TMSLogs\NGL-Service-PCMiler-Log.txt";
        public string LogFileName
        {
            get
            {
                return _strLogFileName;
            }
            set
            {
                _strLogFileName = value;
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
        private DateTime _dtObjectCreateDate;

        private string[] _PCMBuffers;
        public string[] PCMBuffers
        {
            get
            {
                return _PCMBuffers;
            }
            set
            {
                _PCMBuffers = value;
            }
        }

        private static string _Routeoptimization;
        public static string Routeoptimization
        {
            get { if (string.IsNullOrEmpty(_Routeoptimization)) { return RouteOptimization.None.ToString(); } else { return _Routeoptimization; } }
            set { _Routeoptimization = value; }
        }
        private TrimbleAPI _oTrimbleAPI = null; // new TrimbleAPI();
        public TrimbleAPI oTrimbleAPI
        {
            get
            {
                if (_oTrimbleAPI == null) { _oTrimbleAPI = new TrimbleAPI(blnUseTLs12); _oTrimbleAPI.APIKey = this.APIKey; }
                
                return _oTrimbleAPI;
            }
            set
            {
                _oTrimbleAPI = value;
            }
        }

        private static string _VechType;
        public static string VechicleType
        {
            get { if (string.IsNullOrEmpty(_VechType)) { return PCMEX_Veh_Type.Truck.ToString(); } else { return _VechType; } }
            set { _VechType = value; }
        }
        private static string _RouteType;
        public static string RouteType
        {
            get { if (string.IsNullOrEmpty(_RouteType)) { return PCMEX_Route_Type.Practical.ToString(); } else { return _RouteType; } }
            set { _RouteType = value; }
        }

        private static string _DistType;
        public static string DistanceType
        {
            get { if (string.IsNullOrEmpty(_DistType)) { return PCMEX_Dist_Type.Miles.ToString(); } else { return _DistType; } }
            set { _DistType = value; }
        }
        private static string _CalcType;
        public static string CalcType
        {
            get { if (string.IsNullOrEmpty(_CalcType)) { return PCMEX_CALCTYPE.CALCTYPE_NONE.ToString(); } else { return _CalcType; } }
            set { _CalcType = value; }
        }

        private static string _OptFlag;
        public static string OptFlag
        {
            get { if (string.IsNullOrEmpty(_OptFlag)) { return PCMEX_Opt_Flags.None.ToString(); } else { return _OptFlag; } }
            set { _OptFlag = value; }
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

        #endregion

        #region " Methods"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string About()
        {
            string strAbout;
            strAbout = "Component Title: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                + ", Version: "
                + System.Convert.ToString(System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion)
                + ", Module Name: " + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).InternalName;
            return strAbout;
        }
        /// <summary>
        /// 
        /// </summary>
        public void openLog()
        {
            if (!string.IsNullOrEmpty(LogFileName))
            {
                _oLog = new clsLog();
                _oLog.Debug = _blnDebug;
                _IOLogStream = _oLog.Open(LogFileName, KeepLogDays, SaveOldLog);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intReturn"></param>
        public void closeLog(int intReturn = 0)
        {
            if (!this.LoggingOn) return;
            try
            {
                if (_oLog == null) return;
                // log the number of seconds the app was running
                TimeSpan tsSpan = DateTime.Now.Subtract(this._dtObjectCreateDate);
                string strSeconds = tsSpan.Seconds.ToString();
                Log("TMS PCM Wrapper was running for " + strSeconds + " seconds.");
                if (intReturn != 0)
                {
                    _oLog.closeLog(intReturn, ref _IOLogStream);
                }
                else
                {
                    _oLog.closeLog(ref _IOLogStream);
                }
            }
            catch (Exception ex)
            {
                //ignore any errors when closing the log file
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessage"></param>
        public void Log(string logMessage)
        {
            if (!this.LoggingOn)
            {
                // If Me.Debug Then Console.WriteLine("Logging is off.  The log message is: " & logMessage)
                return;
            }
            // Write to log file
            try
            {
                if (_oLog == null) openLog();
                _oLog.Write(logMessage, ref _IOLogStream);
            }
            catch (Exception ex)
            {
                // ignore any errors while writing to the log
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="e"></param>
        private void LogError(string strMsg, Exception e = null)
        {
            string strErr = "";
            if (e != null)
            {
                if (this.Debug)
                    this._strLastError = strMsg + " " + e.ToString();
                else
                    this._strLastError = strMsg + " " + e.Message;
                strMsg += " " + e.ToString();
            }
            else
                this._strLastError = strMsg;
            if (this.LoggingOn)
                Log(strMsg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        private string CityName(string zipcode)
        {
            string buffer = "";
            string Zip = "";
            try
            {
                buffer = FirstMatch(zipcode);
            }
            catch (Exception ex)
            {
                LogError("Cannot execute CityName. ", ex);
            }
            return buffer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sAddress"></param>
        /// <returns></returns>
        public string CityToLatLong(ref string sAddress)
        {
            // Note:  see convertLatLongToDbl for one possible format
            //          the new methods may use a different way to read lat/long
            string sResults = "";
            short Ret = 0;
            _strLastError = "";
            double dblLat = 0;
            double dblLong = 0;
            try
            {
                if (getGeoCode(sAddress, ref  dblLat, ref dblLong))
                {
                    sResults = dblLat.ToString() + "," + dblLong.ToString();
                }
                else
                {
                    LogError("Cannot get Lat Long for address: " + sAddress );
                }
            }
            catch (Exception ex)
            {
                LogError("Cannot get Lat Long for address: " + sAddress, ex);
            }
            return sResults;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saddress"></param>
        /// <param name="dblLat"></param>
        /// <param name="dblLong"></param>
        /// <returns></returns>
        public string CityToLatLong(ref string saddress, ref double dblLat, ref double dblLong)
        {
            // Note:  see convertLatLongToDbl for one possible format
            //          the new methods may use a different way to read lat/long
            string sResults = "";
            short Ret = 0;
            _strLastError = "";            
            try
            {
                string[] lctnarr = saddress.Split(',');
                clsAddress objAdrs = new clsAddress();
                objAdrs.strAddress = lctnarr[0];
                objAdrs.strCity = lctnarr[1];
                objAdrs.strState = lctnarr[2];
                objAdrs.strZip = lctnarr[3];
                Ret = oTrimbleAPI.PCMSAddressToLatlong(objAdrs, ref dblLat, ref dblLong);
                sResults = dblLong + "," + dblLat;
                // Check for errors
                if (Ret == -1) LogError("Cannot execute CityToLatLong the address," + saddress + ", is not valid");

            }
            catch (Exception ex)
            {
                LogError("Cannot execute CityToLatLong. ", ex);
            }
            return sResults;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sZipOrAddress"></param>
        /// <returns></returns>
        public clsAddress[] cityStateZipLookup(string sZipOrAddress)
        {

            short numMatches = 0;
            string sStreet = "";
            string sCity = "";
            string sState = "";
            string sZip = "";
            string sCountry = "";
            int Ret = 0;
            _strLastError = "";
            List<clsAddress> result = new List<clsAddress>();
            try
            {


                // Lookup and get the first match
                numMatches = oTrimbleAPI.PCMSLookup(sZipOrAddress);
                // the legacy logc for PCMSLookup would add an array of addresses to the trip
                //then we would loop through all the address on the trip and create an array of 
                // clsAddress objects for each address in teh trip's array
                //
                // The new logic should be able to make one call to get all matching address for sZipOrAddress
                // and parse the Json object adding the results to the result
                //
                // TODO:   both TrimbleAPI.PCMSLookup and  TrimbleAPI.PCMSGetFmtMatch2 with new logic 
                //         to generate a list of matching locations
                if (numMatches >= 1)
                {
                    for (int i = 0; i <= numMatches - 1; i++)
                    {
                        Ret = oTrimbleAPI.PCMSGetFmtMatch2(i, ref sStreet, ref sCity, ref sState, ref sZip, ref sCountry);
                        if (Ret >= 1)
                        {
                            clsAddress addr = new clsAddress();
                            if (sStreet != null && sStreet.Length > 0)
                                addr.strAddress = sStreet.ToString();
                            if (sCity != null && sCity.Length > 0)
                                addr.strCity = sCity.ToString();
                            if (sState != null && sState.Length > 0)
                                addr.strState = sState.ToString();
                            if (sZip != null && sZip.Length > 0)
                                addr.strZip = sZip.ToString();
                            result.Add(addr);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogError("Cannot execute FirstMatch. ", ex);
            }
            if (result != null && result.Count > 0)
                return result.ToArray();
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CityZip"></param>
        /// <returns></returns>
        private string FirstMatch(string CityZip)
        {

            short numMatches = 0;
            string strRet = "";
            try
            {

                // Lookup and get the first match
                numMatches = oTrimbleAPI.PCMSLookup(CityZip);
                if (numMatches >= 1)
                {
                    strRet = oTrimbleAPI.PCMSGetMatch();
                }

            }
            catch (Exception ex)
            {
                LogError("Cannot execute FirstMatch. ", ex);
            }
            return strRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CityNameOrZipCode"></param>
        /// <returns></returns>
        public string FullName(string CityNameOrZipCode)
        {
            return FirstMatch(CityNameOrZipCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strLatLong"></param>
        /// <returns></returns>
        private double convertLatLongToDbl(string strLatLong)
        {
            // renamed to convertLatLongToDbl from convertLatLongToDec
            //Note:  this logic is for the Legacy conversion of string lat long to 
            // double,  the new methods may not need this method.
            string strhemisphere = "W";
            int intdegrees = 0;
            double dblminutes = 0;
            double dblseconds = 0;
            short intmulitplier = 0;
            string strDegrees = "";
            string strMinutes = "";
            string strSeconds = "";
            if (strLatLong.Length > 2)
                strDegrees = strLatLong.Left(3);
            if (strLatLong.Length > 5)
                strMinutes = strLatLong.Mid(4, 2);
            if (strLatLong.Length > 7)
                strSeconds = strLatLong.Mid(6, 2);
            if (strLatLong.Length > 8)
                strhemisphere = strLatLong.Right(1);
            int.TryParse(strDegrees, out intdegrees);
            double.TryParse(strMinutes, out dblminutes);
            double.TryParse(strSeconds, out dblseconds);


            // intdegrees = CShort(Left(strLatLong, 3))
            // dblminutes = CDbl(Mid(strLatLong, 4, 2))
            // dblseconds = CDbl(Mid(strLatLong, 6, 2))
            strhemisphere = strLatLong.Right(1);
            if (dblminutes != 0) { dblminutes = dblminutes / 60.0; }
            if (dblseconds != 0) { dblseconds = dblseconds / 3600.0; }

            if (strhemisphere == "W" | strhemisphere == "S")
            {
                intmulitplier = -1;
            }
            else
            {
                intmulitplier = 1;
            }

            return (intdegrees + dblminutes + dblseconds) * intmulitplier;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sAddress"></param>
        /// <param name="dblLat"></param>
        /// <param name="dblLong"></param>
        /// <returns></returns>
        public bool getGeoCode(string sAddress, ref double dblLat, ref double dblLong)
        {
            bool blnRet = false;
            string strRet = "";
            try
            {
                TrimbleServiceReference.Coordinates sCoords = null;
                List<TrimbleServiceReference.Location> lOrig = oTrimbleAPI.TRMSearch(sAddress);
                if (lOrig != null && lOrig.Count() > 0)
                {

                    sCoords = lOrig[0].Coords;
                    if (!double.TryParse(lOrig[0].Coords.Lat, out dblLat))
                    {
                        _strLastError = "Cannot get lat long for location: " + sAddress;
                        return false;
                    }
                    if (!double.TryParse(lOrig[0].Coords.Lon, out dblLong))
                    {
                        _strLastError = "Cannot get lat long for location: " + sAddress;
                        return false;
                    }
                    blnRet = true;

                }

            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot get lat long for location: " + sAddress, ex);
            }
            catch (Exception ex)
            {
                LogError("Cannot get lat long for location: " + sAddress, ex);
            }

            return blnRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dblLat"></param>
        /// <param name="dblLong"></param>
        /// <returns></returns>
        public bool getGeoCodeByAddress(string location, ref double dblLat, ref double dblLong)
        {
            bool blnRet = false;
            string strRet = "";
            try
            {

                strRet = CityToLatLong(ref location, ref dblLat, ref dblLong);
                if (!string.IsNullOrWhiteSpace(strRet))
                {
                    string[] latlng = strRet.Split(',');
                    if (latlng.Length > 0)
                    {
                        dblLong = Convert.ToDouble(latlng[0]);
                        dblLat = Convert.ToDouble(latlng[1]);
                        blnRet = true;
                    }
                    else
                    {
                        _strLastError = "Cannot get lat long from location data: " + strRet.Trim();
                        blnRet = false;
                    }
                }
                else
                    blnRet = false;
            }

            catch (AccessViolationException ex)
            {
                LogError("Cannot execute getGeoCode: PC Miler is no longer running.", ex);
            }
            catch (Exception ex)
            {
                LogError("Cannot execute getGeoCode. ", ex);
            }

            return blnRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="latlong"></param>
        /// <returns></returns>
        public string LatLongToCity(string latlong)
        {
            StringBuilder buffer = new StringBuilder(100);
            short Ret = 0;
            string sCity = "-1";
            try
            {
                Ret = oTrimbleAPI.PCMSLatLongToCity(latlong, ref sCity);
                // Check for errors
                if (Ret == -1) { sCity = "-1"; }
            }
            catch (Exception ex)
            {
                LogError("Cannot execute LatLongToCity. ", ex);
            }
            return sCity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strAddressType"></param>
        /// <param name="strItemNumber"></param>
        /// <param name="strItemType"></param>
        /// <param name="objSource"></param>
        /// <param name="objPCM"></param>
        /// <param name="strStopName"></param>
        /// <param name="strWarnings"></param>
        /// <returns></returns>
        private bool validateAddress(string strAddressType, string strItemNumber, string strItemType, ref clsAddress objSource, ref clsAddress objPCM, ref string strStopName, ref string strWarnings)
        {
            bool blnRet = false;
            bool blnMatchFound = false;
            string[] strPCMilerCityState;
            string sAddressFound = "";
            string strMatchZip = "";
            string strZip = "";
            int intDash = 0;
            try
            {


                intDash = Strings.InStr(1, objSource.strZip, "-");
                //return just the first 5 digits of the us zip code
                if (intDash > 0)
                {
                    strZip = objSource.strZip.Left(intDash - 1);
                }
                else
                {
                    strZip = objSource.strZip;
                }
                strStopName = objSource.strZip;
                if (oTrimbleAPI.PCMSCheckPlaceName(strZip) > 0)
                { blnMatchFound = true; }
                else
                {
                    blnMatchFound = false;

                    //Note: the Legacy error handler for PC Miler is no longer supported
                    //we must implement a new level of error handler for the API?
                    // this is not clear at the moment
                    //int intErr = PCMSGetError();
                    //if (intErr != 0)
                    //    strWarnings += "PC Miler Check Place Name Failure: " + getPCMError(intErr) + ".  Please try again later.  ";
                    //else
                    //    strWarnings += "The " + strAddressType + " postal code, " + strZip + " ,for " + strItemType + " " + strItemNumber + " is not valid.  All postal codes are required." + Constants.vbCrLf;
                    //return false;

                    // for now just return a generic message if a match is not found
                    strWarnings += "The " + strAddressType + " postal code, " + strZip + " ,for " + strItemType + " " + strItemNumber + " is not valid.  All postal codes are required." + Constants.vbCrLf;
                    return false;
                }
                objPCM.strAddress = ""; // objSource.strAddress
                objPCM.strCity = "";
                objPCM.strState = "";
                objPCM.strZip = "";
                if (blnMatchFound) { objPCM.strZip = strZip; }
                if (UseZipOnly) { return true; } // no more valiation is required

                // Get the PCmiler street address
                string strSource =  objSource.strCity + "," + objSource.strState + "," + objSource.strAddress+","+ strZip;
                int intRetVal = oTrimbleAPI.PCMSLookup(strSource);
                if (intRetVal < 1) { intRetVal = oTrimbleAPI.PCMSLookup(strZip + "," + objSource.strAddress); }
                if (intRetVal > 0)
                {
                    if (oTrimbleAPI.PCMSGetMatch(0, ref sAddressFound))
                    {
                        
                        

                                // --------------------------------------------
                                // End Modifiy by RHR 7.0.5.100 05/19/2016
                                // --------------------------------------------
                                strPCMilerCityState = Strings.Split(sAddressFound, ";");
                                objPCM.strCity = Strings.Trim(strPCMilerCityState[1]);
                                objPCM.strState = Strings.Trim(strPCMilerCityState[2]);
                                objPCM.strAddress = strPCMilerCityState[0].Trim();
                        // Compare Our Address with PC Milers Best Match.
                        // If issues we return a value in strMessage to be logged in the database
                        if (blnMatchFound)
                        {
                            if (objSource.strZip.ToLower() != objPCM.strZip.ToLower())
                            {
                                // the zip code does not match so log a warning that the
                                // address may not match PCMiler Database using postal code
                                // for origin
                                strWarnings = strWarnings + "The " + strAddressType + " postal code for " + strItemType + " " + strItemNumber + " does not match the PCMiler postal code. Using FreightMaster postal code for routing." + Constants.vbCrLf;
                                // set stop name back to zip code
                                strStopName = objSource.strZip;
                            }
                            else
                            {
                                // check if the state matches we only use full addressing if states match
                                if (objPCM.strState.ToLower() != objSource.strState.ToLower())
                                {
                                    strWarnings = strWarnings + "The " + strAddressType + " State for " + strItemType + " " + strItemNumber + " does not match the PCMiler State. Using FreightMaster postal code for routing." + Constants.vbCrLf;
                                    // set stop name back to zip code
                                    strStopName = objSource.strZip;
                                }
                                else if (objPCM.strAddress.ToLower() != objSource.strAddress.ToLower())
                                {
                                    strWarnings = strWarnings + "The " + strAddressType + " Street Address for " + strItemType + " " + strItemNumber + " does not match the PCMiler Street Address. Using PCMiler Street Address for routing." + Constants.vbCrLf;
                                    blnMatchFound = true;
                                }
                                else
                                {
                                    blnMatchFound = true;
                                }

                            }
                        }
                        else
                        {
                            // the postal code could not be found we assume user error on
                            // postal code so check city and state for match (if they do not match
                            // the entire address fails and falls into the bad postal code trap farther down)
                            if (objPCM.strCity == objSource.strCity | objPCM.strState == objSource.strState)
                            {
                                strWarnings = strWarnings + "The " + strAddressType + " Postal Code Is Not Valid Using Closest Match (Please Correct) for " + strItemType + " " + strItemNumber + ".  Using PCMiler address for routing." + Constants.vbCrLf;
                                blnMatchFound = true;
                            }
                            else
                            {
                                // set stop name back to zip code
                                strStopName = objSource.strZip;
                                strWarnings = strWarnings + "The " + strAddressType + " Address and postal code for " + strItemType + " " + strItemNumber + " is not valid.  The load could not be routed." + Constants.vbCrLf;
                            }
                        }
                              
                        }
                        else
                            strWarnings = strWarnings + "There was a problem with the " + strAddressType + " address for " + strItemType + " " + strItemNumber + " . Using FreightMaster postal code for routing." + Constants.vbCrLf;
                    
                }
                else
                {
                    //Note: the Legacy error handler for PC Miler is no longer supported
                    //we must implement a new level of error handler for the API?
                    // this is not clear at the moment
                    //int intErr = PCMSGetError();
                    //string strPCMilerError = "cannot access PC Miler; the system may be busy; please try again later.";
                    //if (intErr != 0)
                    //    strPCMilerError = getPCMError(intErr);
                    //if (blnMatchFound)
                    //    strWarnings += " The " + strAddressType + " Street Address for " + strItemType + " " + strItemNumber + " cannot be found for the postal code provided. Using FreightMaster postal code for routing." + Constants.vbCrLf;
                    //else
                    //{
                    //    strWarnings += "PC Miler Lookup Address Failure: " + strPCMilerError;
                    //    return false;
                    //}

                    //For now just return a message

                    if (blnMatchFound)
                        strWarnings += " The " + strAddressType + " Street Address for " + strItemType + " " + strItemNumber + " cannot be found for the postal code provided. Using FreightMaster postal code for routing." + Constants.vbCrLf;
                    else
                    {
                        strWarnings += "PC Miler Lookup Address Failure: ";
                        return false;
                    }

                }
                blnRet = blnMatchFound;
            }
            catch (Exception ex)
            {
                //add error handler?
            }

            return blnRet;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objOrig"></param>
        /// <param name="objDest"></param>
        /// <param name="objPCMOrig"></param>
        /// <param name="objPCMDest"></param>
        /// <param name="blnAddOrigin"></param>
        /// <param name="objGlobalStopData"></param>
        /// <param name="blnAddressValid"></param>
        /// <param name="strOrigStopName"></param>
        /// <param name="strDestStopName"></param>
        /// <param name="intTrip1"></param>
        /// <param name="strItemNumber"></param>
        /// <param name="strItemType"></param>
        /// <param name="intBookControl"></param>
        /// <param name="intLaneControl"></param>
        /// <param name="arrBaddAddresses"></param>
        /// <returns></returns>
        public bool AddStop(ref clsAddress objOrig, ref clsAddress objDest, ref clsAddress objPCMOrig, ref clsAddress objPCMDest, ref bool blnAddOrigin, ref clsGlobalStopData objGlobalStopData, ref bool blnAddressValid, ref string strOrigStopName, ref string strDestStopName, int intTrip1, string strItemNumber, string strItemType, int intBookControl, int intLaneControl, ref clsPCMBadAddress[] arrBaddAddresses)
        {
            bool blnRet = false;
            string strOriginAddressWarnings = "";
            string strWarnings = "";
            bool blnLogBadAddress = false;
            bool blnOriginAddressValid = true;
            long Ret = 0;
            try
            {


                blnAddressValid = false;
                // ********** Validate and Add the Origin Address *********************
                strOriginAddressWarnings = "";
                if (validateAddress("Origin", strItemNumber, strItemType, ref objOrig, ref objPCMOrig, ref strOrigStopName, ref strOriginAddressWarnings))
                {
                    // NOTE: Warnings are not logged until we process the Destination Address Below

                    if (Strings.Len(Strings.Trim(strOriginAddressWarnings)) > 0)
                    {
                        if (Strings.InStr(1, strWarnings, "(Please Correct)") > 0 & objGlobalStopData.AutoCorrectBadLaneZipCodes == 1)
                        {
                            if (Strings.Len(Strings.Trim(objPCMOrig.strZip)) > 0)
                            {
                                objGlobalStopData.OriginZip = objPCMOrig.strZip;
                                strOriginAddressWarnings = "";
                            }
                            else
                                blnLogBadAddress = true;
                        }
                        else
                            blnLogBadAddress = true;
                    }
                }
                else
                {
                    blnLogBadAddress = true;
                    if (blnAddOrigin)
                    {
                        blnOriginAddressValid = false;
                        objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + strOriginAddressWarnings;
                    }
                }
                if (blnAddOrigin)
                {
                    // Note: stop name references the formatted address created in validateAddress
                    //       this will be the address used for the Route,  
                    //       In the API version we also pass in the full address object to assist with building the route
                    Ret = oTrimbleAPI.PCMSAddStop(strOrigStopName, objOrig);
                    if (Ret < 1)  // this should not fail because validate should already return a valid address?
                    {
                        // The stopname cannot be found so reset to zipcode only
                        if (strOrigStopName == objOrig.strZip)
                        {
                            // This is a total failure
                            blnOriginAddressValid = false;
                            objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The origin address cannot be found and the postal code is not valid.  Cannot route load.";
                            objPCMOrig.strAddress = "** Address Does Not Exist **";
                            objPCMOrig.strCity = "";
                            objPCMOrig.strState = "";
                            objPCMOrig.strZip = "";
                        }
                        else
                        {
                            // try to use the zip code
                            strOrigStopName = objOrig.strZip;
                            objPCMOrig.strAddress = "** Address Does Not Exist **";
                            objPCMOrig.strCity = "";
                            objPCMOrig.strState = "";
                            objPCMOrig.strZip = objOrig.strZip;
                            Ret = oTrimbleAPI.PCMSAddStop(strOrigStopName, objOrig);
                            if (Ret < 1)
                            {
                                // Give up
                                objPCMOrig.strZip = "";
                                blnOriginAddressValid = false;
                                objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The origin address cannot be found and the postal code is not valid.  Cannot route load.";
                            }
                            else
                            {
                                blnOriginAddressValid = true;
                                blnAddOrigin = false;
                            }
                        }
                    }
                    else
                    {
                        blnOriginAddressValid = true;
                        blnAddOrigin = false;
                    }
                }
                strWarnings = "";
                if (validateAddress("Destination", strItemNumber, strItemType, ref objDest, ref objPCMDest, ref strDestStopName, ref strWarnings))
                {
                    if (blnOriginAddressValid)
                    {
                        Ret = oTrimbleAPI.PCMSAddStop(strDestStopName, objDest);
                        if (Ret < 1)
                        {
                            if (strDestStopName == objDest.strZip)
                            {
                                objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The destination address cannot be found and the postal code is not valid.  Cannot route load.";
                                objPCMDest.strAddress = "** Address Does Not Exist **";
                                objPCMDest.strCity = "";
                                objPCMDest.strState = "";
                                objPCMDest.strZip = "";
                            }
                            else
                            {
                                // Try to use the zip code
                                strDestStopName = objDest.strZip;
                                objPCMDest.strAddress = "** Address Does Not Exist **";
                                objPCMDest.strCity = "";
                                objPCMDest.strState = "";
                                objPCMDest.strZip = objDest.strZip;
                                Ret = oTrimbleAPI.PCMSAddStop(strDestStopName, objDest);
                                if (Ret < 1)
                                {
                                    // Give up
                                    objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The destination address cannot be found and the postal code is not valid.  Cannot route load.";
                                    objPCMDest.strZip = "";
                                }
                                else
                                    blnAddressValid = true;
                            }
                        }
                        else
                            blnAddressValid = true;
                    }
                    // We need clarificaiton on what is being done here!!!
                    if (Strings.Len(Strings.Trim(strWarnings)) > 0)
                    {
                        if (Strings.InStr(1, strWarnings, "(Please Correct)") > 0 & objGlobalStopData.AutoCorrectBadLaneZipCodes == 1)
                        {
                            if (Strings.Len(Strings.Trim(objPCMDest.strZip)) > 0)
                            {
                                objGlobalStopData.DestZip = objPCMDest.strZip;
                                strWarnings = "";
                            }
                            else
                                blnLogBadAddress = true;
                        }
                        else
                            blnLogBadAddress = true;
                    }
                }
                else
                {
                    objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + strWarnings;
                    blnLogBadAddress = true;
                }
                if (blnLogBadAddress)
                    // add the bad address to the array
                    GlobalSettings.AddBaddAddressToArray(intBookControl, intLaneControl, objOrig, objDest, objPCMOrig, objPCMDest, strOriginAddressWarnings + strWarnings, objGlobalStopData.BatchID, ref arrBaddAddresses);
                blnRet = true;
            }
            catch (Exception ex)
            {
                //add error handler?
            }
            return blnRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objOrig"></param>
        /// <param name="objDest"></param>
        /// <param name="objPCMOrig"></param>
        /// <param name="objPCMDest"></param>
        /// <param name="blnAddOrigin"></param>
        /// <param name="objGlobalStopData"></param>
        /// <param name="blnAddressValid"></param>
        /// <param name="strOrigStopName"></param>
        /// <param name="strDestStopName"></param>
        /// <param name="intTrip1"></param>
        /// <param name="intBookControl"></param>
        /// <param name="intLaneControl"></param>
        /// <param name="arrBaddAddresses"></param>
        /// <returns></returns>
        public bool AddStop(ref clsAddress objOrig, ref clsAddress objDest, ref clsAddress objPCMOrig, ref clsAddress objPCMDest,
            ref bool blnAddOrigin, ref clsGlobalStopData objGlobalStopData, ref bool blnAddressValid,
            ref string strOrigStopName, ref string strDestStopName, int intTrip1, int intBookControl, int intLaneControl,
             ref clsPCMBadAddress[] arrBaddAddresses)
        {
            bool blnRet = false;
            string strOriginAddressWarnings = "";
            string strWarnings = "";
            bool blnLogBadAddress = false;
            bool blnOriginAddressValid = true;
            long Ret = 0;
            try
            {


                blnAddressValid = false;
                string strItemNumber = string.Empty;
                string strItemType = string.Empty;
                // ********** Validate and Add the Origin Address *********************
                strOriginAddressWarnings = "";
                if (validateAddress("Origin", strItemNumber, strItemType, ref objOrig, ref objPCMOrig, ref strOrigStopName, ref strOriginAddressWarnings))
                {
                    // NOTE: Warnings are not logged until we process the Destination Address Below

                    if (Strings.Len(Strings.Trim(strOriginAddressWarnings)) > 0)
                    {
                        if (Strings.InStr(1, strWarnings, "(Please Correct)") > 0 & objGlobalStopData.AutoCorrectBadLaneZipCodes == 1)
                        {
                            if (Strings.Len(Strings.Trim(objPCMOrig.strZip)) > 0)
                            {
                                objGlobalStopData.OriginZip = objPCMOrig.strZip;
                                strOriginAddressWarnings = "";
                            }
                            else
                                blnLogBadAddress = true;
                        }
                        else
                            blnLogBadAddress = true;
                    }
                }
                else
                {
                    blnLogBadAddress = true;
                    if (blnAddOrigin)
                    {
                        blnOriginAddressValid = false;
                        objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + strOriginAddressWarnings;
                    }
                }
                if (blnAddOrigin)
                {
                    // Note: stop name references the formatted address created in validateAddress
                    //       this will be the address used for the Route,  
                    //       In the API version we also pass in the full address object to assist with building the route
                    Ret = oTrimbleAPI.PCMSAddStop(strOrigStopName, objOrig);
                    if (Ret < 1)  // this should not fail because validate should already return a valid address?
                    {
                        // The stopname cannot be found so reset to zipcode only
                        if (strOrigStopName == objOrig.strZip)
                        {
                            // This is a total failure
                            blnOriginAddressValid = false;
                            objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The origin address cannot be found and the postal code is not valid.  Cannot route load.";
                            objPCMOrig.strAddress = "** Address Does Not Exist **";
                            objPCMOrig.strCity = "";
                            objPCMOrig.strState = "";
                            objPCMOrig.strZip = "";
                        }
                        else
                        {
                            // try to use the zip code
                            strOrigStopName = objOrig.strZip;
                            objPCMOrig.strAddress = "** Address Does Not Exist **";
                            objPCMOrig.strCity = "";
                            objPCMOrig.strState = "";
                            objPCMOrig.strZip = objOrig.strZip;
                            Ret = oTrimbleAPI.PCMSAddStop(strOrigStopName, objOrig);
                            if (Ret < 1)
                            {
                                // Give up
                                objPCMOrig.strZip = "";
                                blnOriginAddressValid = false;
                                objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The origin address cannot be found and the postal code is not valid.  Cannot route load.";
                            }
                            else
                            {
                                blnOriginAddressValid = true;
                                blnAddOrigin = false;
                            }
                        }
                    }
                    else
                    {
                        blnOriginAddressValid = true;
                        blnAddOrigin = false;
                    }
                }
                strWarnings = "";
                if (validateAddress("Destination", strItemNumber, strItemType, ref objDest, ref objPCMDest, ref strDestStopName, ref strWarnings))
                {
                    if (blnOriginAddressValid)
                    {
                        Ret = oTrimbleAPI.PCMSAddStop(strDestStopName, objDest);
                        if (Ret < 1)
                        {
                            if (strDestStopName == objDest.strZip)
                            {
                                objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The destination address cannot be found and the postal code is not valid.  Cannot route load.";
                                objPCMDest.strAddress = "** Address Does Not Exist **";
                                objPCMDest.strCity = "";
                                objPCMDest.strState = "";
                                objPCMDest.strZip = "";
                            }
                            else
                            {
                                // Try to use the zip code
                                strDestStopName = objDest.strZip;
                                objPCMDest.strAddress = "** Address Does Not Exist **";
                                objPCMDest.strCity = "";
                                objPCMDest.strState = "";
                                objPCMDest.strZip = objDest.strZip;
                                Ret = oTrimbleAPI.PCMSAddStop(strDestStopName, objDest);
                                if (Ret < 1)
                                {
                                    // Give up
                                    objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The destination address cannot be found and the postal code is not valid.  Cannot route load.";
                                    objPCMDest.strZip = "";
                                }
                                else
                                    blnAddressValid = true;
                            }
                        }
                        else
                            blnAddressValid = true;
                    }
                    // We need clarificaiton on what is being done here!!!
                    if (Strings.Len(Strings.Trim(strWarnings)) > 0)
                    {
                        if (Strings.InStr(1, strWarnings, "(Please Correct)") > 0 & objGlobalStopData.AutoCorrectBadLaneZipCodes == 1)
                        {
                            if (Strings.Len(Strings.Trim(objPCMDest.strZip)) > 0)
                            {
                                objGlobalStopData.DestZip = objPCMDest.strZip;
                                strWarnings = "";
                            }
                            else
                                blnLogBadAddress = true;
                        }
                        else
                            blnLogBadAddress = true;
                    }
                }
                else
                {
                    objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + strWarnings;
                    blnLogBadAddress = true;
                }
                if (blnLogBadAddress)
                    // add the bad address to the array
                    GlobalSettings.AddBaddAddressToArray(intBookControl, intLaneControl, objOrig, objDest, objPCMOrig, objPCMDest, strOriginAddressWarnings + strWarnings, objGlobalStopData.BatchID, ref arrBaddAddresses);
                blnRet = true;
            }
            catch (Exception ex)
            {
                //add error handler?
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
        /// <param name="logfile"></param>
        /// <returns></returns>
        public clsGlobalStopData getPracticalMiles(clsAddress objOrig, clsAddress objDest, PCMEX_Route_Type Route_Type, PCMEX_Dist_Type Dist_Type,
            int intCompControl, int intBookControl, int intLaneControl, string strItemNumber, string strItemType,
            double dblAutoCorrectBadLaneZipCodes,
            double dblBatchID, bool blnBatch, ref clsPCMBadAddress[] arrBaddAddresses, bool loggin, string logfile)
        {
            return getPracticalMilesEx(objOrig, objDest, Route_Type, Dist_Type,
                intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType,
                dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, ref arrBaddAddresses, loggin, logfile);
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
        /// <param name="logfile"></param>
        /// <returns></returns>
        public clsGlobalStopData getPracticalMilesEx(clsAddress objOrig, clsAddress objDest, PCMEX_Route_Type Route_Type, PCMEX_Dist_Type Dist_Type,
            int intCompControl, int intBookControl, int intLaneControl, string strItemNumber, string strItemType, double dblAutoCorrectBadLaneZipCodes,
            double dblBatchID, bool blnBatch, ref clsPCMBadAddress[] arrBaddAddresses, bool loggin, string logfile)
        {
            int Ret = 0;
            double mi = 0;  // converted to double to support new API
            int intTrip1 = 0;
            int intDash = 0;
            string strOrigStopName = "";
            string strDestStopName = "";
            // Dim Ret As Long
            // Dim objAllStops As clsGlobalStopData
            clsGlobalStopData objGlobalStopData = new clsGlobalStopData();
            clsAddress objPCMOrig = new clsAddress();
            clsAddress objPCMDest = new clsAddress();
            TrimbleServiceReference.Coordinates origCoords = new TrimbleServiceReference.Coordinates();
            TrimbleServiceReference.Coordinates destCoords = new TrimbleServiceReference.Coordinates();
            bool blnAddressValid = true;
            _strLastError = "";
            try
            {

                objGlobalStopData.BadAddressCount = 0;
                objGlobalStopData.FailedAddressMessage = "";
                objGlobalStopData.BatchID = dblBatchID;
                blnAddressValid = true;

                // calc type was part of the Legacy Trip logic
                //we must find a new way to set the calc type
                //using the new route reports API
                //if (EXCALC_Type != PCMEX_CALCTYPE.CALCTYPE_NONE)
                //    PCMSSetCalcTypeEx(intTrip1, EXCALC_Type, EXOpt_Flags, EXVeh_Type);
                //else
                //    PCMSSetCalcType(intTrip1, Route_Type);

                // miles type was part of the Legacy Trip logic
                //we must find a new way to set the miles type
                //using the new route reports API
                //PCMSSetMiles(intTrip1);
                //if (Dist_Type == INGL_Service_PCMiler.PCMEX_Dist_Type.DIST_TYPE_KILO)
                //    PCMSSetKilometers(intTrip1);
                RouteType = Route_Type.ToString();
                DistanceType = Dist_Type.ToString();
                objPCMOrig = new clsAddress();
                objPCMDest = new clsAddress();
                objGlobalStopData.AutoCorrectBadLaneZipCodes = dblAutoCorrectBadLaneZipCodes;
                this._strLastError = "";
                bool addOrig = true;
                AddStop(ref objOrig, ref objDest, ref objPCMOrig, ref objPCMDest, ref addOrig, ref objGlobalStopData, ref blnAddressValid,
                    ref strOrigStopName, ref strDestStopName, intTrip1, strItemNumber, strItemType, intBookControl, intLaneControl,
                    ref arrBaddAddresses);
                if (!string.IsNullOrEmpty(this._strLastError))
                {
                    LogError("Cannot execute getPracticalMiles. " + this._strLastError);
                    objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
                }
                else if (blnAddressValid)
                {
                    // We need to determine if we need two methods or just one to optimize the route
                    Ret = oTrimbleAPI.PCMSOptimize();
                    mi = oTrimbleAPI.PCMSCalculate();
                    // Check for errors before converting tenths to miles
                    // the Legacy API miles had to be converted
                    // the new API we are expecting a double to be returned so this code has been removed
                    //if (-1 != mi) { dblTotalMiles = mi / 10.0; }
                    //objGlobalStopData.TotalMiles = dblTotalMiles;
                    objGlobalStopData.TotalMiles = mi;
                }
            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot get miles for " + strOrigStopName + " To " + strDestStopName + ": PC Miler is no longer running.", ex);
                objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            catch (Exception ex)
            {
                LogError("Cannot get miles for " + strOrigStopName + " To " + strDestStopName + ": ", ex);
                objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                oTrimbleAPI.PCMSClearStops();
            }
            return objGlobalStopData;
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
        /// <param name="loggin"></param>
        /// <param name="logfile"></param>
        /// <returns></returns>
        public clsGlobalStopData getPracticalMiles(clsAddress objOrig, clsAddress objDest, PCMEX_Route_Type Route_Type, PCMEX_Dist_Type Dist_Type,
            PCMEX_CALCTYPE EXCALC_Type, PCMEX_Opt_Flags EXOpt_Flags, PCMEX_Veh_Type EXVeh_Type, int intCompControl,
            int intBookControl, int intLaneControl, string strItemNumber, string strItemType, double dblAutoCorrectBadLaneZipCodes,
            double dblBatchID, bool blnBatch, ref clsPCMBadAddress[] arrBaddAddresses, bool loggin, string logfile)
        {
            return getPracticalMilesEx(objOrig, objDest, Route_Type, Dist_Type, PCMEX_CALCTYPE.CALCTYPE_NONE, PCMEX_Opt_Flags.None, PCMEX_Veh_Type.Truck, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, ref arrBaddAddresses, loggin, logfile);
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
        /// <param name="loggin"></param>
        /// <param name="logfile"></param>
        /// <returns></returns>
        public clsGlobalStopData getPracticalMilesEx(clsAddress objOrig, clsAddress objDest, PCMEX_Route_Type Route_Type,
            PCMEX_Dist_Type Dist_Type, PCMEX_CALCTYPE EXCALC_Type, PCMEX_Opt_Flags EXOpt_Flags, PCMEX_Veh_Type EXVeh_Type,
            int intCompControl, int intBookControl, int intLaneControl, string strItemNumber, string strItemType,
            double dblAutoCorrectBadLaneZipCodes, double dblBatchID, bool blnBatch, ref clsPCMBadAddress[] arrBaddAddresses,
            bool loggin, string logfile)
        {
            int Ret = 0;
            double mi = 0;  // converted to double to support new API
            int intTrip1 = 0;
            int intDash = 0;
            string strOrigStopName = "";
            string strDestStopName = "";
            // Dim Ret As Long
            // Dim objAllStops As clsGlobalStopData
            clsGlobalStopData objGlobalStopData = new clsGlobalStopData();
            clsAddress objPCMOrig = new clsAddress();
            clsAddress objPCMDest = new clsAddress();
            TrimbleServiceReference.Coordinates origCoords = new TrimbleServiceReference.Coordinates();
            TrimbleServiceReference.Coordinates destCoords = new TrimbleServiceReference.Coordinates();
            bool blnAddressValid = true;
            _strLastError = "";
            try
            {

                objGlobalStopData.BadAddressCount = 0;
                objGlobalStopData.FailedAddressMessage = "";
                objGlobalStopData.BatchID = dblBatchID;
                blnAddressValid = true;

                // calc type was part of the Legacy Trip logic
                //we must find a new way to set the calc type
                //using the new route reports API
                //if (EXCALC_Type != PCMEX_CALCTYPE.CALCTYPE_NONE)
                //    PCMSSetCalcTypeEx(intTrip1, EXCALC_Type, EXOpt_Flags, EXVeh_Type);
                //else
                //    PCMSSetCalcType(intTrip1, Route_Type);

                // miles type was part of the Legacy Trip logic
                //we must find a new way to set the miles type
                //using the new route reports API
                //PCMSSetMiles(intTrip1);
                //if (Dist_Type == INGL_Service_PCMiler.PCMEX_Dist_Type.DIST_TYPE_KILO)
                //    PCMSSetKilometers(intTrip1);
                RouteType = Route_Type.ToString();
                DistanceType = Dist_Type.ToString();
                OptFlag = EXOpt_Flags.ToString();
                VechicleType = EXVeh_Type.ToString();
                CalcType = EXCALC_Type.ToString();

                objPCMOrig = new clsAddress();
                objPCMDest = new clsAddress();
                objGlobalStopData.AutoCorrectBadLaneZipCodes = dblAutoCorrectBadLaneZipCodes;
                this._strLastError = "";
                bool addOrig = true;
                AddStop(ref objOrig, ref objDest, ref objPCMOrig, ref objPCMDest, ref addOrig, ref objGlobalStopData, ref blnAddressValid, ref strOrigStopName, ref strDestStopName, intTrip1, strItemNumber, strItemType, intBookControl, intLaneControl, ref arrBaddAddresses);
                if (!string.IsNullOrEmpty(this._strLastError))
                {
                    LogError("Cannot execute getPracticalMiles. " + this._strLastError);
                    objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
                }
                else if (blnAddressValid)
                {
                    // We need to determine if we need two methods or just one to optimize the route
                    Ret = oTrimbleAPI.PCMSOptimize();
                    mi = oTrimbleAPI.PCMSCalculate();
                    // Check for errors before converting tenths to miles
                    // the Legacy API miles had to be converted
                    // the new API we are expecting a double to be returned so this code has been removed
                    //if (-1 != mi) { dblTotalMiles = mi / 10.0; }
                    //objGlobalStopData.TotalMiles = dblTotalMiles;
                    objGlobalStopData.TotalMiles = mi;
                }
            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot get miles for " + strOrigStopName + " To " + strDestStopName + ": PC Miler is no longer running.", ex);
                objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            catch (Exception ex)
            {
                LogError("Cannot get miles for " + strOrigStopName + " To " + strDestStopName + ": ", ex);
                objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                oTrimbleAPI.PCMSClearStops();
            }
            return objGlobalStopData;
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
        /// <param name="dblAutoCorrectBadLaneZipCodes"></param>
        /// <param name="dblBatchID"></param>
        /// <param name="blnBatch"></param>
        /// <param name="arrBaddAddresses"></param>
        /// <param name="Login"></param>
        /// <param name="logfile"></param>
        /// <returns></returns>

        public clsGlobalStopData getPracticalMilesEx(clsAddress objOrig, clsAddress objDest, PCMEX_Route_Type Route_Type, PCMEX_Dist_Type Dist_Type, PCMEX_CALCTYPE EXCALC_Type, PCMEX_Opt_Flags EXOpt_Flags, PCMEX_Veh_Type EXVeh_Type,
            int intCompControl, int intBookControl, int intLaneControl, double dblAutoCorrectBadLaneZipCodes, double dblBatchID, bool blnBatch, ref clsPCMBadAddress[] arrBaddAddresses, string Login, string logfile)
        {
            int Ret = 0;
            double mi = 0;  // converted to double to support new API
            int intTrip1 = 0;
            int intDash = 0;
            string strOrigStopName = "";
            string strDestStopName = "";
            // Dim Ret As Long
            // Dim objAllStops As clsGlobalStopData
            clsGlobalStopData objGlobalStopData = new clsGlobalStopData();
            clsAddress objPCMOrig = new clsAddress();
            clsAddress objPCMDest = new clsAddress();
            TrimbleServiceReference.Coordinates origCoords = new TrimbleServiceReference.Coordinates();
            TrimbleServiceReference.Coordinates destCoords = new TrimbleServiceReference.Coordinates();
            bool blnAddressValid = true;
            _strLastError = "";
            try
            {

                objGlobalStopData.BadAddressCount = 0;
                objGlobalStopData.FailedAddressMessage = "";
                objGlobalStopData.BatchID = dblBatchID;
                blnAddressValid = true;

                // calc type was part of the Legacy Trip logic
                //we must find a new way to set the calc type
                //using the new route reports API
                //if (EXCALC_Type != PCMEX_CALCTYPE.CALCTYPE_NONE)
                //    PCMSSetCalcTypeEx(intTrip1, EXCALC_Type, EXOpt_Flags, EXVeh_Type);
                //else
                //    PCMSSetCalcType(intTrip1, Route_Type);

                // miles type was part of the Legacy Trip logic
                //we must find a new way to set the miles type
                //using the new route reports API
                //PCMSSetMiles(intTrip1);
                //if (Dist_Type == INGL_Service_PCMiler.PCMEX_Dist_Type.DIST_TYPE_KILO)
                //    PCMSSetKilometers(intTrip1);

                objPCMOrig = new clsAddress();
                objPCMDest = new clsAddress();
                objGlobalStopData.AutoCorrectBadLaneZipCodes = dblAutoCorrectBadLaneZipCodes;
                this._strLastError = "";
                bool addOrig = true;
                string strItemNumber = string.Empty;
                string strItemType = string.Empty;
                AddStop(ref objOrig, ref objDest, ref objPCMOrig, ref objPCMDest, ref addOrig, ref objGlobalStopData, ref blnAddressValid, ref strOrigStopName, ref strDestStopName, intTrip1, strItemNumber, strItemType, intBookControl, intLaneControl, ref arrBaddAddresses);
                if (!string.IsNullOrEmpty(this._strLastError))
                {
                    LogError("Cannot execute getPracticalMiles. " + this._strLastError);
                    objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
                }
                else if (blnAddressValid)
                {
                    // We need to determine if we need two methods or just one to optimize the route
                    Ret = oTrimbleAPI.PCMSOptimize();
                    mi = oTrimbleAPI.PCMSCalculate();
                    // Check for errors before converting tenths to miles
                    // the Legacy API miles had to be converted
                    // the new API we are expecting a double to be returned so this code has been removed
                    //if (-1 != mi) { dblTotalMiles = mi / 10.0; }
                    //objGlobalStopData.TotalMiles = dblTotalMiles;
                    objGlobalStopData.TotalMiles = mi;
                }
            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot get miles for " + strOrigStopName + " To " + strDestStopName + ": PC Miler is no longer running.", ex);
                objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            catch (Exception ex)
            {
                LogError("Cannot get miles for " + strOrigStopName + " To " + strDestStopName + ": ", ex);
                objGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {

            }
            return objGlobalStopData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Origin"></param>
        /// <param name="Destination"></param>
        /// <returns></returns>
        public double Miles(string Origin, string Destination)
        {
            double dMiles = 0;
            try
            {
                dMiles = oTrimbleAPI.PCMSCalcDistance(Origin, Destination);

            }
            catch (AccessViolationException ex)
            {
                LogError(string.Format("Cannot get miles for {0} To {1} : PC Miler is not available right now.", Origin, Destination), ex);
            }
            catch (Exception ex)
            {
                LogError("Cannot not calculate miles. Please check your results for errors.", ex);
            }
            finally
            {

            }
            return dMiles;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CityName"></param>
        /// <returns></returns>
        public string zipcode(string CityName)
        {
            string strRet = "";
            strRet = FirstMatch(CityName);
            return strRet.Substring(0, 5);
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
            return PCMReSyncMultiStopEx(ref arrFMStops, PCMEX_CALCTYPE.CALCTYPE_NONE, PCMEX_Opt_Flags.None, PCMEX_Veh_Type.Truck, dblBatchID, blnKeepStopNumbers, ref arrPCMReportRecords);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrFMStops"></param>
        /// <param name="EXCALC_Type"></param>
        /// <param name="EXOpt_Flags"></param>
        /// <param name="EXVeh_Type"></param>
        /// <param name="dblBatchID"></param>
        /// <param name="blnKeepStopNumbers"></param>
        /// <param name="arrPCMReportRecords"></param>
        /// <returns></returns>
        public clsGlobalStopData PCMReSyncMultiStopEx(ref clsFMStopData[] arrFMStops, PCMEX_CALCTYPE EXCALC_Type, PCMEX_Opt_Flags EXOpt_Flags, PCMEX_Veh_Type EXVeh_Type, double dblBatchID, bool blnKeepStopNumbers, ref clsPCMReportRecord[] arrPCMReportRecords)
        {
            List<clsFMStopData> oFMStops = new List<clsFMStopData>(arrFMStops);
            List<clsPCMReportRecord> oPCMReportRecords = new List<clsPCMReportRecord>();
            var oGlobalStopData = PCMReSyncMultiStop(ref oFMStops, EXCALC_Type, EXOpt_Flags, EXVeh_Type, dblBatchID, blnKeepStopNumbers, ref oPCMReportRecords);
            if (oFMStops != null && oFMStops.Count > 0)
                arrFMStops = oFMStops.ToArray();
            if (oPCMReportRecords != null && oPCMReportRecords.Count > 0)
                arrPCMReportRecords = oPCMReportRecords.ToArray();

            return oGlobalStopData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oFMStops"></param>
        /// <param name="EXCALC_Type"></param>
        /// <param name="EXOpt_Flags"></param>
        /// <param name="EXVeh_Type"></param>
        /// <param name="dblBatchID"></param>
        /// <param name="blnKeepStopNumbers"></param>
        /// <param name="oPCMReportRecords"></param>
        /// <returns></returns>
        private clsGlobalStopData PCMReSyncMultiStop(ref List<clsFMStopData> oFMStops, PCMEX_CALCTYPE EXCALC_Type, PCMEX_Opt_Flags EXOpt_Flags, PCMEX_Veh_Type EXVeh_Type, double dblBatchID, bool blnKeepStopNumbers, ref List<clsPCMReportRecord> oPCMReportRecords)
        {
            int Ret = 0;
            int i = 0;
            int intTrip1 = 0;
            string strOrigStopName = "";
            string strDestStopName = "";
            short shtDupStreetsCt = 0;
            short shtLoopCt = 0;
            bool blnAddOrigin = true;
            clsGlobalStopData oGlobalStopData = new clsGlobalStopData();
            string strMatchedStreet = "";
            clsAddress objOrig = new clsAddress();
            clsAddress objDest = new clsAddress();
            clsAddress objPCMOrig = new clsAddress();
            clsAddress objPCMDest = new clsAddress();
            bool blnAddressValid = true;
            int Reseq_Type = 1;

            try
            {
                if (oFMStops == null || oFMStops.Count < 1)
                    return null/* TODO Change to default(_) if this is not a reference type */;

                oGlobalStopData.BadAddressCount = 0;
                oGlobalStopData.FailedAddressMessage = "";
                oGlobalStopData.BatchID = dblBatchID;

                // calc type was part of the Legacy Trip logic
                //we must find a new way to set the calc type
                ////using the new route reports API
                //if (EXCALC_Type != PCMEX_CALCTYPE.CALCTYPE_NONE)
                //    PCMSSetCalcTypeEx(intTrip1, EXCALC_Type, EXOpt_Flags, EXVeh_Type);
                //else
                //    PCMSSetCalcType(intTrip1, oFMStops[0].RouteType);

                // miles type was part of the Legacy Trip logic
                //we must find a new way to set the miles type
                //using the new route reports API
                //PCMSSetMiles(intTrip1);
                //if (oFMStops[0].DistType == 1)
                //    // we are using Kilometers
                //    PCMSSetKilometers(intTrip1)


                if (blnKeepStopNumbers)
                    Routeoptimization = RouteOptimization.ThruAll.ToString();
                else
                    Routeoptimization = RouteOptimization.None.ToString();
                // TODO:  need to add new logic to determine what to do when we keep stop number
                if (!blnKeepStopNumbers)
                {
                    List<System.Collections.Generic.List<clsPCMReportRecord>> oReports = new List<System.Collections.Generic.List<clsPCMReportRecord>>();
                    int intOrigins = oFMStops.Where(x => x.LocationisOrigin == true).Count();
                    if (intOrigins == 0)
                    {
                        // just add the stops normally
                        // TODO: buildTrip is not finished
                        if (!buildTrip(oFMStops, ref oPCMReportRecords, ref oGlobalStopData, blnKeepStopNumbers))
                            return null/* TODO Change to default(_) if this is not a reference type */;
                    }
                    else
                    {
                        bool blnTripAdded = false;
                        List<string> lOriginStopKeys = new List<string>();
                        // create an array of the original stops so we have an immutable index
                        // sorted by LocationisOrigin
                        clsFMStopData[] StopIndexed = (from s in oFMStops
                                                       orderby s.LocationisOrigin
                                                       select s).ToArray();
                        if (StopIndexed != null && StopIndexed.Count() > 0)
                        {
                            for (int index = 0; index <= StopIndexed.Count() - 1; index++)
                            {
                                var oStop = StopIndexed[index];
                                if (oStop.LocationisOrigin)
                                {
                                    // we reprocess each origin to get optimal miles
                                    string strStopKey = string.Concat(oStop.Street, oStop.City, oStop.State, oStop.Zip).ToLower();
                                    if (!lOriginStopKeys.Contains(strStopKey))
                                    {
                                        // we have not processed this origin so save the key and build a trip
                                        lOriginStopKeys.Add(strStopKey);
                                        List<clsFMStopData> oSortedStops = new List<clsFMStopData>();
                                        List<clsPCMReportRecord> oSortedReportRecords = new List<clsPCMReportRecord>();
                                        clsGlobalStopData oSortedGlobalStopData = new clsGlobalStopData();
                                        oSortedStops.Add(oStop); // add this origin first
                                        for (int ii = 0; ii <= StopIndexed.Count() - 1; ii++)
                                        {
                                            if (ii != index)
                                                oSortedStops.Add(StopIndexed[ii]);
                                        }
                                        // build the trim
                                        var blnBuildTrip = buildTrip(oSortedStops, ref oSortedReportRecords, ref oSortedGlobalStopData, blnKeepStopNumbers);
                                        if (blnBuildTrip)
                                        {
                                            blnTripAdded = true;
                                            if (oSortedReportRecords != null && oSortedReportRecords.Count() > 0)
                                            {
                                                if (oPCMReportRecords == null)
                                                {
                                                    oPCMReportRecords = oSortedReportRecords;
                                                    oGlobalStopData = oSortedGlobalStopData;
                                                }
                                                else if (oPCMReportRecords.Count() > 0)
                                                {
                                                    if (oSortedReportRecords[oSortedReportRecords.Count() - 1].TotalMiles < oPCMReportRecords[oPCMReportRecords.Count() - 1].TotalMiles)
                                                    {
                                                        oPCMReportRecords = oSortedReportRecords;
                                                        oGlobalStopData = oSortedGlobalStopData;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            // if we could not load any stop and if we have an error just return nothing
                            if (!blnTripAdded && !string.IsNullOrEmpty(_strLastError))
                                return null/* TODO Change to default(_) if this is not a reference type */;
                        }
                    }
                }
                // Modified by RHR v-7.0.5.100 05/25/2016
                // included a default routing option and calls the new method buildTrip
                if (oPCMReportRecords == null || oPCMReportRecords.Count() < 1)
                {
                    if (blnKeepStopNumbers)
                        Routeoptimization = RouteOptimization.ThruAll.ToString();
                    else
                        Routeoptimization = RouteOptimization.None.ToString();
                    // we do not have a solution so just add the stops normally (this always happens when blnKeepStopNumbers is true)
                    if (!(buildTrip(oFMStops, ref oPCMReportRecords, ref oGlobalStopData, blnKeepStopNumbers)))
                        return null/* TODO Change to default(_) if this is not a reference type */;
                }
                // We now loop through each stop and lookup the correct match in the PCMS report list                           
                strMatchedStreet = "";
                List<string> strMatchedStops = new List<string>();
                foreach (clsFMStopData oPCMStop in oFMStops)
                {
                    var MStreet = oPCMStop.PCMilerStreet;
                    var MCity = oPCMStop.PCMilerCity;
                    var MState = oPCMStop.PCMilerState;
                    var MZip = oPCMStop.Zip;
                    var MStopSeq = oPCMStop.SeqNumber;
                    var MStopNbr = oPCMStop.StopNumber;
                    List<clsPCMReportRecord> oMatch;
                    if (blnKeepStopNumbers)
                        Routeoptimization = RouteOptimization.ThruAll.ToString();
                    else
                        Routeoptimization = RouteOptimization.None.ToString();
                    if (blnKeepStopNumbers)
                    {
                        // We do not change the sequence so we lookup the data based on the sequence number
                        oMatch = (from d in oPCMReportRecords
                                  where d.Street == MStreet & d.State == MState & d.City == MCity & d.SeqNumber == MStopSeq
                                  select d).ToList();
                        // If no street match fall back to zip code
                        if (oMatch == null || oMatch.Count < 1)
                            oMatch = (from d in oPCMReportRecords
                                      where d.Zip == MZip & d.SeqNumber == MStopSeq
                                      select d).ToList();
                        // If no zip code match fall back to city and state
                        if (oMatch == null || oMatch.Count < 1)
                            oMatch = (from d in oPCMReportRecords
                                      where d.State == MState & d.City == MCity & d.SeqNumber == MStopSeq
                                      select d).ToList();
                        if (oMatch == null || oMatch.Count < 1)
                        {
                            // there is a problem and we cannot get the miles for this stop so set the miles and stop number to zero
                            oPCMStop.StopNumber = 0;
                            oPCMStop.LegMiles = 0;
                            oPCMStop.TotalMiles = 0;
                            oPCMStop.LegCost = 0;
                            oPCMStop.TotalCost = 0;
                            oPCMStop.LegTime = "";
                            oPCMStop.TotalTime = "";
                        }
                        else
                        {
                            // update the stop data with the first match found
                            // oPCMStop.StopNumber = oMatch(0).StopNumber
                            oPCMStop.LegMiles = oMatch[0].LegMiles;
                            oPCMStop.TotalMiles = oMatch[0].TotalMiles;
                            oPCMStop.LegCost = oMatch[0].LegCost;
                            oPCMStop.TotalCost = oMatch[0].TotalCost;
                            oPCMStop.LegTime = oMatch[0].LegTime;
                            oPCMStop.TotalTime = oMatch[0].TotalTime;
                            oPCMStop.Matched = true;
                        }
                    }
                    else
                    {
                        // Stops have been changed so we need to determine if duplicate stops exist for the same address and which one gets the miles
                        // remembering that miles for the second load at the same stop are zero. Currently we make this decision in the order they are 
                        // transmitted to us
                        oMatch = (from d in oPCMReportRecords
                                  where d.Street == MStreet & d.State == MState & d.City == MCity
                                  select d).ToList();
                        // If no street match fall back to zip code
                        if (oMatch == null || oMatch.Count < 1)
                            oMatch = (from d in oPCMReportRecords
                                      where d.Zip == MZip
                                      select d).ToList();
                        // If no zip code match fall back to city and state
                        if (oMatch == null || oMatch.Count < 1)
                            oMatch = (from d in oPCMReportRecords
                                      where d.State == MState & d.City == MCity
                                      select d).ToList();
                        if (oMatch == null || oMatch.Count < 1)
                        {
                            // there is a problem and we cannot get the miles for this stop so set the miles and stop number to zero
                            oPCMStop.StopNumber = 0;
                            oPCMStop.LegMiles = 0;
                            oPCMStop.TotalMiles = 0;
                            oPCMStop.LegCost = 0;
                            oPCMStop.TotalCost = 0;
                            oPCMStop.LegTime = "";
                            oPCMStop.TotalTime = "";
                        }
                        else
                        {
                            // Check if the stop has already been assigned miles
                            strMatchedStreet = oMatch[0].Street + oMatch[0].City + oMatch[0].State + oMatch[0].Zip;
                            if (strMatchedStops != null && strMatchedStops.Count > 0 && strMatchedStops.Contains(strMatchedStreet))
                            {
                                // a duplicate is found so all costs and miles are zero
                                oPCMStop.StopNumber = oMatch[0].StopNumber + 1;
                                oPCMStop.LegMiles = 0;
                                oPCMStop.TotalMiles = oMatch[0].TotalMiles;
                                oPCMStop.LegCost = 0;
                                oPCMStop.TotalCost = oMatch[0].TotalCost;
                                oPCMStop.LegTime = "";
                                oPCMStop.TotalTime = oMatch[0].TotalTime;
                                oPCMStop.Matched = true;
                            }
                            else
                            {
                                oPCMStop.StopNumber = oMatch[0].StopNumber + 1;
                                oPCMStop.LegMiles = oMatch[0].LegMiles;
                                oPCMStop.TotalMiles = oMatch[0].TotalMiles;
                                oPCMStop.LegCost = oMatch[0].LegCost;
                                oPCMStop.TotalCost = oMatch[0].TotalCost;
                                oPCMStop.LegTime = oMatch[0].LegTime;
                                oPCMStop.TotalTime = oMatch[0].TotalTime;
                                oPCMStop.Matched = true;
                            }
                            if (strMatchedStops == null)
                                strMatchedStops = new List<string>();
                            strMatchedStops.Add(strMatchedStreet);
                        }
                    }
                }
            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot execute PCMReSyncMultiStop: PC Miler is no longer running.", ex);
                oGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            catch (Exception ex)
            {
                LogError("Cannot execute PCMReSyncMultiStop. ", ex);
                oGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                oTrimbleAPI.PCMSClearStops();
            }
            return oGlobalStopData;
        }

        #region "Test API Methods"

        public void TimeWindowRouting(clsPCMDataStop[] arrStopData)
        {
            //https://pcmiler.alk.com/apis/rest/v1.0/service.svc/route/optimize?region={region}&dataset={dataset}
            try
            {

                oTrimbleAPI.PCMSClearStops();
               //string sRet =  oTrimbleAPI.TimeWindowOptimizer();

            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot execute PCMResync: PC Miler is no longer running.", ex);
                //oGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            catch (Exception ex)
            {
                LogError("Cannot execute PCMResync. ", ex);
                //oGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                oTrimbleAPI.PCMSClearStops();
            }
        }


        #endregion



        #region "TODO: migrate logic from NGL.Service.PCMiler64.PCMiles.vb"

        //public clsGlobalStopData PCMReSync(clsPCMDataStop[] arrStopData, string strConsNumber, double dblBatchID, bool blnKeepStopNumbers, clsAllStop[] arrAllStops, ref clsPCMBadAddress[] arrBaddAddresses)
        //{
        //    clsGlobalStopData oStopData = null;
        //    //Copy logic here from NGL.Service.PCMiler64.PCMiles.vb
        //    return oStopData;
        //}
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
            int Ret = 0;
            int i = 0;
            short lines = 0;
            int intTrip1 = 0;
            StringBuilder buff = new StringBuilder(255);
            List<clsPCMStop> colPCMStops;
            clsPCMStop objPCMStop;
            string strZip = "";
            string strStreet = "";
            string strCity = "";
            string strState = "";
            string strStopName = "";
            string[] strCityState;
            string strOrigStopName = "";
            string strDestStopName = "";
            bool blnInbound = false;
            string[] strDupZips;
            string[] strDupStreets;
            Collection colDupStops;
            short shtDupStreetsCt = 0;
            short shtLoopCt = 0;
            bool blnAddOrigin = true;
            bool blnSkip = false;
            short shtPos = 0;
            clsGlobalStopData oGlobalStopData = new clsGlobalStopData();
            int shtStopCT = 0;
            bool blnMatchFound = false;
            double dblLegMiles = 0;
            double dblTotalMiles = 0;
            double dblLegCost = 0;
            double dblTotalCost = 0;
            string strLegTime = "";
            string strTotalTime = "";
            string strZipFound = "";
            string strStreetsFound = "";
            short shtFieldCt = 0;
            bool blnAddToClass = true;
            int shtStopNbr = 0;
            string strMatchedStreet = "";
            clsAddress objOrig = new clsAddress();
            clsAddress objDest = new clsAddress();
            clsAddress objPCMOrig = new clsAddress();
            clsAddress objPCMDest = new clsAddress();
            bool blnAddressValid = true;
            int Reseq_Type = 1;
            // Dim EvtLog As New System.Diagnostics.EventLog
            // EvtLog.Log = "Application"
            // EvtLog.Source = "NGL.Service.PCMiler"
            // EvtLog.WriteEntry("PCMReSync Running", EventLogEntryType.Information)
            _strLastError = "";
            try
            {
                // Set global parameters
                //gProcessRunning = true;
                oTrimbleAPI.PCMSClearStops();
                oGlobalStopData.BadAddressCount = 0;
                oGlobalStopData.FailedAddressMessage = "";
                oGlobalStopData.BatchID = dblBatchID;


                // Create a new stops collection
                colPCMStops = new List<clsPCMStop>();
                if (arrStopData.Length > 0)
                {
                    // set up parameters
                    //PCMSSetCalcType(intTrip1, arrStopData[0].RouteType);
                    //PCMSSetMiles(intTrip1);
                    //if (arrStopData[0].DistType == 1)
                    //    // we are using Kilometers
                    //    PCMSSetKilometers(intTrip1);
                    blnInbound = arrStopData[0].LaneOriginAddressUse;
                }
                // Process the stops
                for (int intStops = 0; intStops <= arrStopData.Length - 1; intStops++)
                {
                    shtStopCT = shtStopCT + 1;
                    // loop through each stop data record
                    clsPCMDataStop oStop = arrStopData[intStops];
                    shtStopNbr = oStop.BookStopNo;
                    // set up the address objects
                    objOrig = new clsAddress();
                    objDest = new clsAddress();
                    if (oStop.LaneOriginAddressUse)
                    {
                        objOrig.strZip = oStop.BookDestZip;
                        objOrig.strAddress = oStop.BookDestAddress1;
                        objOrig.strCity = oStop.BookDestCity;
                        objOrig.strState = oStop.BookDestState;
                        objDest.strZip = oStop.BookOrigZip;
                        objDest.strAddress = oStop.BookOrigAddress1;
                        objDest.strCity = oStop.BookOrigCity;
                        objDest.strState = oStop.BookOrigState;
                        RouteType = Enum.GetName(typeof(PCMEX_Route_Type), oStop.RouteType);
                    }
                    else
                    {
                        objOrig.strZip = oStop.BookOrigZip;
                        objOrig.strAddress = oStop.BookOrigAddress1;
                        objOrig.strCity = oStop.BookOrigCity;
                        objOrig.strState = oStop.BookOrigState;
                        objDest.strZip = oStop.BookDestZip;
                        objDest.strAddress = oStop.BookDestAddress1;
                        objDest.strCity = oStop.BookDestCity;
                        objDest.strState = oStop.BookDestState;
                        RouteType = Enum.GetName(typeof(PCMEX_Route_Type), oStop.RouteType);
                    }
                    // initialize the PC Miler Address Objects
                    objPCMOrig = new clsAddress();
                    objPCMDest = new clsAddress();
                    // Add the Stop
                    AddStop(ref objOrig, ref objDest, ref objPCMOrig, ref objPCMDest, ref blnAddOrigin, ref oGlobalStopData, ref blnAddressValid, ref strOrigStopName, ref strDestStopName, intTrip1, oStop.BookProNumber, "PRO Number", oStop.BookControl, 0, ref arrBaddAddresses);
                    // If the address is valid then add the stop to the collection
                    if (blnAddressValid)
                    {
                        if (blnKeepStopNumbers)
                            Routeoptimization = RouteOptimization.ThruAll.ToString();
                        else
                            Routeoptimization = RouteOptimization.None.ToString();
                        // add to collection
                        objPCMStop = new clsPCMStop();
                        {
                            var withBlock = objPCMStop;
                            withBlock.BookLoadControl = oStop.BookLoadControl;
                            withBlock.Zip = objDest.strZip;
                            withBlock.City = objOrig.strCity;
                            withBlock.State = objDest.strState;
                            withBlock.Street = objDest.strAddress;
                            if (strDestStopName == objDest.strZip)
                                // There was a problem with the address so clear the street (we are using zip code)
                                withBlock.PCMilerStreet = "";
                            else
                                withBlock.PCMilerStreet = objPCMDest.strAddress;
                            withBlock.PCMilerCity = objPCMDest.strCity;
                            withBlock.PCMilerState = objPCMDest.strState;
                            if (blnKeepStopNumbers)
                                //withBlock.StopNumber = shtStopNbr;
                                withBlock.LoopCt = shtLoopCt;
                            withBlock.BookODControl = oStop.BookODControl;
                        }
                        // add the stop to the collection
                        // colPCMStops.Add(objPCMStop, "k" + oStop.BookLoadControl.ToString());
                        colPCMStops.Add(objPCMStop);
                    }
                }
                // test for bad zips if any exist we cannot continue
                if (oGlobalStopData.FailedAddressMessage.Trim() != "")
                {
                    _strLastError += oGlobalStopData.FailedAddressMessage + Constants.vbCrLf + "The requested operation has been canceled!";
                    return null/* TODO Change to default(_) if this is not a reference type */;
                }
                //if (blnKeepStopNumbers)
                //    Routeoptimization = RouteOptimization.ThruAll.ToString();
                //else
                //    Routeoptimization = RouteOptimization.None.ToString();
                // If we are not using manual stop number call the PCM Resequence and Optimize routine
                if (!blnKeepStopNumbers)
                {
                    // resequence  and optimize the stops
                    // PCMSSetResequence(intTrip1, Reseq_Type);                   
                    Ret = oTrimbleAPI.PCMSOptimize();
                }
                //Ret = oTrimbleAPI.PCMSCalculate();
                double Ret1 = oTrimbleAPI.PCMSCalculate();
                clsPCMReportRecord[] array = new clsPCMReportRecord[10];
                oTrimbleAPI.PCMSNumRptLines(ref array, blnKeepStopNumbers);
                //returns no of stops in the mileage report.
                //Replace PCMNumberReportLines with a Method in API Class to return list of stops from the report
                //below were the reports for loop to use the list of stops in foreach loop.
                //there fore the PCMSGetRptLine no loner needed,because we already have the lines in the stop Array from the report.

                shtStopCT = 0;
                shtDupStreetsCt = 0;
                strDupZips = new string[1];
                strDupStreets = new string[1];
                colDupStops = new Collection();
                // we skip the first line because it is the origin.


                if (blnInbound & !blnKeepStopNumbers)
                {
                    // We need to reverse the stop numbers.
                    //objPCMStop = new clsPCMStop();

                    // now add all values to the AllStops Collection
                    foreach (var objPCMStops in colPCMStops)
                        AddStopToArray(objPCMStops.LoopCt, objPCMStops.StopName, objPCMStops.BookLoadControl.ToString(), objPCMStops.BookODControl.ToString(), "0", 1, objPCMStops.StopNumber, objPCMStops.LegMiles, objPCMStops.TotalCost, strConsNumber, ref arrAllStops);
                }
            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot execute PCMResync: PC Miler is no longer running.", ex);
                oGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            catch (Exception ex)
            {
                LogError("Cannot execute PCMResync. ", ex);
                oGlobalStopData = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                oTrimbleAPI.PCMSClearStops();
            }
            return oGlobalStopData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shtStopNumber"></param>
        /// <param name="strStopName"></param>
        /// <param name="strID1"></param>
        /// <param name="strID2"></param>
        /// <param name="strTruckName"></param>
        /// <param name="intTruckNumber"></param>
        /// <param name="shtSeqNbr"></param>
        /// <param name="dblDistToPrev"></param>
        /// <param name="dblTotalRouteCost"></param>
        /// <param name="strConsNumber"></param>
        /// <param name="arrAllStops"></param>
        public void AddStopToArray(short shtStopNumber, string strStopName, string strID1, string strID2, string strTruckName, int intTruckNumber, short shtSeqNbr, double dblDistToPrev, double dblTotalRouteCost, string strConsNumber, ref clsAllStop[] arrAllStops)
        {
            // create a new object
            clsAllStop oStop = new clsAllStop();
            {
                var withBlock = oStop;
                withBlock.StopNumber = shtStopNumber;
                withBlock.Stopname = strStopName;
                withBlock.ID1 = strID1;
                withBlock.ID2 = strID2;
                withBlock.TruckDesignator = strTruckName;
                withBlock.TruckNumber = intTruckNumber;
                withBlock.SeqNbr = shtSeqNbr;
                withBlock.DistToPrev = dblDistToPrev;
                withBlock.TotalRouteCost = dblTotalRouteCost;
                withBlock.ConsNumber = strConsNumber;
            }
            if (arrAllStops == null)
            {
                var oldArrAllStops = arrAllStops;
                arrAllStops = new clsAllStop[1];
                if (oldArrAllStops != null)
                    Array.Copy(oldArrAllStops, arrAllStops, Math.Min(1, oldArrAllStops.Length));
            }
            else
            {
                var oldArrAllStops = arrAllStops;
                arrAllStops = new clsAllStop[arrAllStops.Length + 1];
                if (oldArrAllStops != null)
                    Array.Copy(oldArrAllStops, arrAllStops, Math.Min(arrAllStops.Length + 1, oldArrAllStops.Length));
            }
            arrAllStops[arrAllStops.Length - 1] = oStop;
        }

        //public clsPCMReturn getRouteMiles(ref clsSimpleStop[] sRoute)
        //{
        //    clsPCMReturn oPCMReturn = new clsPCMReturn();
        //    //Copy logic here from NGL.Service.PCMiler64.PCMiles.vb
        //    return oPCMReturn;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRoute"></param>
        /// <returns></returns>
        public clsPCMReturn getRouteMiles(ref clsSimpleStop[] sRoute)
        {
            clsPCMReturn oPCMReturn = new clsPCMReturn();
            try
            {
                clsAddress oadrs = new clsAddress();
                oTrimbleAPI.PCMSClearStops();
                for (int i = 0; i <= sRoute.Length - 1; i++)
                {
                    // Test if data has been added to the array @ i
                    if (sRoute[i] != null)
                    {
                        // Check if the address info is valid.
                        if (oTrimbleAPI.PCMSCheckPlaceName(sRoute[i].Address) > 0)
                        {
                            if (oTrimbleAPI.PCMSAddStop(sRoute[i].Address, oadrs) < 1)
                            {
                                // The address cannot be found so return an error                    
                                oPCMReturn.RetVal = 0;
                                oPCMReturn.Message = "Cannot get route miles! The address " + sRoute[i].Address + " cannot be added to the route or it does not exists in the PCMiler address database.";
                                break;
                            }
                            else
                                oPCMReturn.RetVal = i;
                        }
                        else
                        {
                            oPCMReturn.RetVal = 0;
                            oPCMReturn.Message = "Cannot get route miles! The address " + sRoute[i].Address + " does not exists in the PCMiler address database.";
                            break;
                        }
                    }
                }
                int intLegID = 0;
                if (oPCMReturn.RetVal > 0)
                {
                    // oTrimbleAPI.PCMSCalculate();
                    // Now Get the Data Back
                    oTrimbleAPI.PCMSGetLegInfo(ref sRoute);
                    for (int i = 0; i <= sRoute.Length - 1; i++)
                    {
                        // Test if data has been added to the array @ i
                        if (sRoute[i] != null && sRoute[i].StopNumber > 0)
                        {


                            // update the leg info
                            // legInfoType sLegData = new legInfoType();
                            //oTrimbleAPI.PCMSGetLegInfo(ref sRoute);
                            //{
                            //    var withBlock = sRoute[i];
                            //    withBlock.LegMiles = double.Parse(sLegData.legMiles);
                            //    withBlock.LegCost = double.Parse(sLegData.legCost);
                            //    withBlock.LegHours = double.Parse(sLegData.legHours);
                            //    withBlock.TotalMiles = double.Parse(sLegData.totMiles);
                            //    withBlock.TotalCost = double.Parse(sLegData.totCost);
                            //    withBlock.TotalHours = double.Parse(sLegData.totHours);
                            //}
                            //intLegID += 1;
                        }
                    }
                }
            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot get Route Miles: PC Miler is no longer running.", ex);
                oPCMReturn.RetVal = 0;
                oPCMReturn.Message = "Error while getting Route Miles: " + ex.Message + ". PC Miler is no longer running.";
            }
            catch (Exception ex)
            {
                LogError("Cannot get Route Miles: ", ex);
                oPCMReturn.RetVal = 0;
                oPCMReturn.Message = "Error while getting Route Miles: " + ex.Message + ".";
            }
            finally
            {
                oTrimbleAPI.PCMSClearStops();
            }

            return oPCMReturn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strAddress"></param>
        /// <returns></returns>
        public bool PCMValidateAddress(string strAddress)
        {
            bool bRet = false;
            //Copy logic here from NGL.Service.PCMiler64.PCMiles.vb
            try
            {                
                short numMatches = oTrimbleAPI.PCMSLookup(strAddress);
                if (numMatches > 0) bRet = true;
            }
            catch (AccessViolationException ex)
            {
                LogError("Cannot execute PCMValidateAddress: PC Miler is no longer running.", ex);
            }
            catch (Exception ex)
            {
                LogError("Cannot execute PCMValidateAddress.", ex);
            }
            return bRet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intErr"></param>
        /// <returns></returns>
        private string getPCMError(int intErr = 0)
        {
            StringBuilder buff = new StringBuilder(255);
            string strError = "";
            try
            {                
                if (intErr == 0)
                    intErr = oTrimbleAPI.PCMSGetError();
                if (intErr != 0)
                {
                    var ret = oTrimbleAPI.PCMSGetErrorString(intErr, buff, 255);
                    strError = buff.ToString();
                }
            }
            catch (Exception ex)
            {
            }

            return strError;
        }

        //private bool validateAddressEx(ref clsFMStopData oSource)
        //{
        //    bool bRet = false;
        //    //Copy logic here from NGL.Service.PCMiler64.PCMiles.vb
        //    return bRet;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oSource"></param>
        /// <returns></returns>
        private bool validateAddressEx(ref clsFMStopData oSource)
        {
            bool blnMatchFound = false;
            string[] strPCMilerCityState;
            StringBuilder buffer = new StringBuilder(256);
            string buff = "";
            string strMatchZip = "";
            string strZip = "";
            int intDash = 0;
            string strAddressType = "Destination";


            var withBlock = oSource;
            if (withBlock.LocationisOrigin)
                strAddressType = "Origin";
            string[] ziparr = withBlock.Zip.Split('-');
            if (ziparr != null && ziparr.Count() > 0)
            {
                if (ziparr.Count() == 1)
                    strZip = ziparr[0].ToString();
                else
                    strZip = ziparr[1].ToString();
            }
            //intDash = InStr(1, withBlock.Zip, "-");
            //if (intDash)
            //    strZip = Left(withBlock.Zip, intDash - 1);
            else
                strZip = withBlock.Zip;
            withBlock.StopName = withBlock.Zip;
            if (oTrimbleAPI.PCMSCheckPlaceName(strZip) > 0)
                blnMatchFound = true;
            else
            {
                blnMatchFound = false;
                // Modified by RHR v-7.0.5.100 05/25/2016
                int intErr = oTrimbleAPI.PCMSGetError();
                if (intErr != 0)
                    withBlock.Warning += "PC Miler Check Place Name Failure: " + getPCMError(intErr) + ".  Please try again later.  ";
                else
                    withBlock.Warning += "The " + strAddressType + " postal code, " + strZip + " ,for PRO Number " + withBlock.BookProNumber + " is not valid.  All postal codes are required." + Constants.vbCrLf;
                return false;
            }
            withBlock.PCMilerState = "** Address Does Not Exist **"; // objSource.strAddress
            withBlock.PCMilerCity = "";
            withBlock.PCMilerState = "";
            withBlock.PCMilerZip = "";
            withBlock.PCMilerCountry = "";
            if (blnMatchFound)
                withBlock.Zip = strZip;
            // public configuration property UseZipOnly is configured by the caller typically in the parameter table
            // If true we do not attempt to lookup the street address
            if (UseZipOnly)
                return true;
            // Get the PCmiler street address
            string strSource;
            strSource = strZip + " " + withBlock.City + ", " + withBlock.State + ";  " + withBlock.Street;
            int intRetVal;
            intRetVal = oTrimbleAPI.PCMSLookup(strSource);
            if (intRetVal < 1)
                intRetVal = oTrimbleAPI.PCMSLookup(strZip + ";" + withBlock.Street);
            if (intRetVal > 0)
            {
                string buffer1 = string.Empty;
                buffer1 = oTrimbleAPI.PCMSGetMatch(0);
                if (buffer1.Count() > 0)

                {
                    buff = buffer1.ToString();
                    // Debug.Print buff
                    savePCMBuffer(buff);
                    //        if (buffer.Insert(1, ";", 1))
                    //        {
                    //            withBlock.PCMilerStreet = Trim(simpleStreetScrubber(Strings.Replace(Strings.Mid(buff, Strings.InStr(1, buff, ";") + 1, Strings.Len(buff)), Strings.Chr(0), "")));
                    //            if (InStr(1, withBlock.PCMilerStreet, "&"))
                    //            {
                    //                withBlock.PCMilerStreet = "** Address Does Not Exist **";
                    //                withBlock.Warning = withBlock.Warning + "The " + strAddressType + " address for PRO Number " + withBlock.BookProNumber + " does not exist in PCMiler. Using FreightMaster postal code for routing." + Constants.vbCrLf;
                    //            }
                    //            else
                    //            {
                    //                withBlock.StopName = Strings.Trim(Strings.Replace(buff, Strings.Chr(0), ""));
                    //                // --------------------------------------------
                    //                // Start Modifiy by RHR 7.0.5.100 05/19/2016
                    //                // --------------------------------------------
                    //                // test for US vs Canadian postal codes
                    //                // PC Miler will sometimes return a postal code as the first portion fo the buffer before the city name so 
                    //                // we need to use this if available and split it out from the city
                    //                string strTestZip = buff.Split(" ")(0);
                    //                // strTestZip = Trim(Left(buff, 5))
                    //                string strNewCAZip = "";
                    //                if (Information.IsNumeric(Strings.Left(Strings.Trim(strTestZip), 5)))
                    //                {
                    //                    // this is a standard US zip code so use the returned value
                    //                    withBlock.PCMilerZip = Strings.Left(Strings.Trim(strTestZip), 5); // Trim(Left(buff, 6))
                    //                    strMatchZip = Strings.Left(strZip, 5);
                    //                    buff = splitCityStateCounty(buff, strTestZip.Length() + 1);
                    //                }
                    //                else
                    //                {
                    //                    // check for canadian postal code --Modifiy by RHR for 7.0.5.102 on 12/21/2016
                    //                    strTestZip = Strings.Left(buff, 7);
                    //                    if (isValidCAZip(strTestZip, strNewCAZip))
                    //                    {
                    //                        withBlock.PCMilerZip = strNewCAZip;
                    //                        strMatchZip = strZip.Trim();
                    //                        buff = splitCityStateCounty(buff, strTestZip.Length() + 1);
                    //                    }
                    //                    else
                    //                    {
                    //                        strMatchZip = strZip;
                    //                        withBlock.PCMilerZip = strZip;
                    //                        buff = splitCityStateCounty(buff, 1);
                    //                    }
                    //                }

                    //                // --------------------------------------------
                    //                // End Modifiy by RHR 7.0.5.100 05/19/2016
                    //                // --------------------------------------------
                    //                strPCMilerCityState = Strings.Split(buff, ",");
                    //                withBlock.PCMilerCity = Strings.Trim(strPCMilerCityState[0]);
                    //                withBlock.PCMilerState = Strings.Trim(strPCMilerCityState[1]);
                    //                // Compare Our Address with PC Milers Best Match.
                    //                // If issues we return a value in strMessage to be logged in the database
                    //                if (blnMatchFound)
                    //                {
                    //                    if (strMatchZip.ToLower() != withBlock.PCMilerZip.ToLower())
                    //                    {
                    //                        // the zip code does not match so log a warning that the
                    //                        // address may not match PCMiler Database using postal code
                    //                        // for origin
                    //                        withBlock.Warning += " The " + strAddressType + " postal code for PRO Number " + withBlock.BookProNumber + " does not match the PCMiler postal code. Using FreightMaster postal code for routing." + Constants.vbCrLf;
                    //                        // set stop name back to zip code
                    //                        withBlock.StopName = withBlock.Zip;
                    //                    }
                    //                    else
                    //                    // check if the state matches we only use full addressing if states match
                    //                    if (withBlock.PCMilerState.ToLower() != withBlock.State.ToLower())
                    //                    {
                    //                        withBlock.Warning += " The " + strAddressType + " State for PRO Number " + withBlock.BookProNumber + " does not match the PCMiler State. Using FreightMaster postal code for routing." + Constants.vbCrLf;
                    //                        // set stop name back to zip code
                    //                        withBlock.StopName = withBlock.Zip;
                    //                    }
                    //                    else if (withBlock.PCMilerStreet.ToLower() != withBlock.Street.ToLower())
                    //                    {
                    //                        withBlock.Warning += " The " + strAddressType + " Street Address for PRO Number" + withBlock.BookProNumber + " does not match the PCMiler Street Address. Using PCMiler Street Address for routing." + Constants.vbCrLf;
                    //                        blnMatchFound = true;
                    //                    }
                    //                    else
                    //                        blnMatchFound = true;
                    //                }
                    //                else
                    //                    // the postal code could not be found we assume user error on
                    //                    // postal code so check city and state for match (if they do not match
                    //                    // the entire address fails and falls into the bad postal code trap farther down)
                    //                    if (withBlock.PCMilerCity == withBlock.City | withBlock.PCMilerState == withBlock.State)
                    //                {
                    //                    withBlock.Warning += " The " + strAddressType + " Postal Code Is Not Valid Using Closest Match (Please Correct) for PRO Number " + withBlock.BookProNumber + ".  Using PCMiler address for routing." + Constants.vbCrLf;
                    //                    blnMatchFound = true;
                    //                }
                    //                else
                    //                {
                    //                    // set stop name back to zip code
                    //                    withBlock.StopName = withBlock.Zip;
                    //                    withBlock.Warning += " The " + strAddressType + " Address and postal code for PRO Number" + withBlock.BookProNumber + " is not valid.  The load could not be routed." + Constants.vbCrLf;
                    //                }
                    //            }
                    //        }
                    //        else
                    //            withBlock.Warning += " There was a problem with the " + strAddressType + " address for PRO Number " + withBlock.BookProNumber + " . Using FreightMaster postal code for routing." + Constants.vbCrLf;
                    //    }
                    //}
                    //else
                    //{
                    //    // Modified by RHR v-7.0.5.100 06/03/2016
                    //    int intErr = PCMSGetError();
                    //    string strPCMilerError = "cannot access PC Miler; the system may be busy; please try again later.";
                    //    if (intErr != 0)
                    //        strPCMilerError = getPCMError(intErr);
                    //    if (blnMatchFound)
                    //        withBlock.Warning += " The " + strAddressType + " Street Address for PRO Number " + withBlock.BookProNumber + " cannot be found for the postal code provided. Using FreightMaster postal code for routing." + Constants.vbCrLf;
                    //    else
                    //    {
                    //        withBlock.Warning += "PC Miler Lookup Address Failure: " + strPCMilerError;
                    //        return false;
                    //    }

                }
            }
            return blnMatchFound;
        }

        ////private bool AddStopEx(ref clsFMStopData oFMStop, ref clsGlobalStopData objGlobalStopData)
        ////{
        ////    bool bRet = false;
        ////    //Copy logic here from NGL.Service.PCMiler64.PCMiles.vb
        ////    return bRet;
        ////}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oFMStop"></param>
        /// <param name="objGlobalStopData"></param>
        /// <returns></returns>
        private bool AddStopEx(ref clsFMStopData oFMStop, ref clsGlobalStopData objGlobalStopData)
        {
            bool blnRet = true;
            long Ret = 0;
            string strAddressType = "Destination";
            {
                var withBlock = oFMStop;
                if (withBlock.LocationisOrigin)
                    strAddressType = "Origin";
                withBlock.AddressValid = false;
                withBlock.Warning = "";
                clsAddress oAddress = new clsAddress();
                if (validateAddressEx(ref oFMStop))
                {
                    Ret = oTrimbleAPI.PCMSAddStop(withBlock.StopName, oAddress);//Need to check
                    if (Ret < 1)
                    {
                        // The stopname cannot be found so reset to zipcode only
                        if (withBlock.StopName == withBlock.Zip)
                        {
                            // This is a total failure
                            blnRet = false;
                            objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The " + strAddressType + " address cannot be found and the postal code is not valid.  Cannot route load.";
                            withBlock.PCMilerStreet = "** Address Does Not Exist **";
                            withBlock.PCMilerCity = "";
                            withBlock.PCMilerState = "";
                            withBlock.PCMilerZip = "";
                            withBlock.PCMilerCountry = "";
                        }
                        else
                        {
                            // try to use the zip code
                            withBlock.StopName = withBlock.Zip;
                            withBlock.PCMilerStreet = "** Address Does Not Exist **";
                            withBlock.PCMilerCity = "";
                            withBlock.PCMilerState = "";
                            withBlock.PCMilerZip = withBlock.Zip;
                            withBlock.PCMilerCountry = "";
                            Ret = oTrimbleAPI.PCMSAddStop(withBlock.StopName, withBlock);
                            if (Ret < 1)
                            {
                                // Give up
                                withBlock.PCMilerZip = "";
                                blnRet = false;
                                objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage + "The " + strAddressType + " address cannot be found and the postal code is not valid.  Cannot route load.";
                            }
                            else
                                withBlock.AddressValid = true;
                        }
                    }
                    else
                        withBlock.AddressValid = true;
                    if ((withBlock.Warning.Trim()).Length > 0)
                    {
                        if (Strings.InStr(1, withBlock.Warning, "(Please Correct)") > 0 & objGlobalStopData.AutoCorrectBadLaneZipCodes == 1)
                        {
                            if ((withBlock.PCMilerZip.Trim()).Length > 0)
                            {
                                if (withBlock.LocationisOrigin)
                                    objGlobalStopData.OriginZip = withBlock.PCMilerZip;
                                else
                                    objGlobalStopData.DestZip = withBlock.PCMilerZip;
                                withBlock.Warning = "";
                            }
                            else
                                withBlock.LogBadAddress = true;
                        }
                        else
                            withBlock.LogBadAddress = true;
                    }
                }
                else
                {
                    withBlock.LogBadAddress = true;
                    withBlock.AddressValid = false;
                    blnRet = false;
                    objGlobalStopData.FailedAddressMessage += withBlock.Warning;
                }
            }
            return blnRet;
        }

        ////private bool buildTrip(List<clsFMStopData> oFMStops, ref List<clsPCMReportRecord> oPCMReportRecords, ref clsGlobalStopData oGlobalStopData, bool blnKeepStopNumbers, int Reseq_Type = 1)
        ////{
        ////    bool bRet = false;
        ////    //Copy logic here from NGL.Service.PCMiler64.PCMiles.vb
        ////    return bRet;
        ////}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oFMStops"></param>
        /// <param name="oPCMReportRecords"></param>
        /// <param name="oGlobalStopData"></param>
        /// <param name="blnKeepStopNumbers"></param>
        /// <param name="Reseq_Type"></param>
        /// <returns></returns>
        private bool buildTrip(List<clsFMStopData> oFMStops, ref List<clsPCMReportRecord> oPCMReportRecords, ref clsGlobalStopData oGlobalStopData, bool blnKeepStopNumbers, int Reseq_Type = 1)
        {
            double Ret = 0.0;
            bool blnSuccess = false;
            // clear all stops and reload
            //Ret = PCMSClearStops(intTrip1);
            oTrimbleAPI.PCMSClearStops();
            // We use a for to loop to be sure we process the data in the correct order
            for (int intStops = 0; intStops <= oFMStops.Count - 1; intStops++)
            {
                // loop through each stop data record
                clsFMStopData oStop = oFMStops[intStops];
                oStop.SeqNumber = intStops;
                // Add the Stop
                AddStopEx(ref oStop, ref oGlobalStopData);
            }
            // test for bad zips if any exist we cannot continue
            if (oGlobalStopData.FailedAddressMessage.Trim() != "")
            {
                _strLastError += oGlobalStopData.FailedAddressMessage + Constants.vbCrLf + "The requested operation has been canceled!";
                return blnSuccess;
            }
            if (blnKeepStopNumbers)
                Routeoptimization = RouteOptimization.ThruAll.ToString();
            else
                Routeoptimization = RouteOptimization.None.ToString();
            // Determine if we need to optimize the route (resync)
            if (!blnKeepStopNumbers)
            {
                // resequence  and optimize the stops
                // Calculate the miles
                //Ret = PCMSCalculate(intTrip1);
                Ret = oTrimbleAPI.PCMSCalculate();
                // We now store the report data in a list that can be referenced by the caller.
                oPCMReportRecords = getPCMSReport(blnKeepStopNumbers);
            }

            return blnSuccess;
        }


        //private List<clsPCMReportRecord> getPCMSReport(int intTrip1)
        //{
        //    List<clsPCMReportRecord> oRet = new List<clsPCMReportRecord>();
        //    //Copy logic here from NGL.Service.PCMiler64.PCMiles.vb
        //    return oRet;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blnKeepStopNumbers"></param>
        /// <returns></returns>
        private List<clsPCMReportRecord> getPCMSReport(bool blnKeepStopNumbers)
        {
            clsPCMReportRecord[] oReports = null;
            if (blnKeepStopNumbers)
                Routeoptimization = RouteOptimization.ThruAll.ToString();
            else
                Routeoptimization = RouteOptimization.None.ToString();
            oTrimbleAPI.PCMSNumRptLines(ref oReports, blnKeepStopNumbers);
            List<clsPCMReportRecord> oReports1 = new List<clsPCMReportRecord>();
            // **** PCMSGetRptLine Record Layout for PC Miler 24 Tab delimited record ****'
            // NOTE tabs are represented by the |  this value does not actually exist in the data; also additional spaces have been
            // added arround the | to make it easier to read. Tolls are also possible if purchased they would show up betweed Total Time and Leg Est CHG
            // If a valid street address is used (10 columns):
            // City, State |         Street         |Leg Miles|Total Miles|Leg  Cost|Total Cost|Leg Time|Total Time|Leg EST CHG|Total EST CHG|
            // Grandview, WA | 171 North Forsell Road | 1064.5  |   1792.8  | 1266.39 |  2245.31 |  16:51 |   30:26  |   3820.3  |    6633.3   |
            // If defaulting to zip code (10 columns)
            // Zip |  City, State, County  |Leg Miles|Total Miles|Leg  Cost|Total Cost|Leg Time|Total Time|Leg EST CHG|Total EST CHG|
            // 98930 | Grandview, WA, Yakima |  1040.0 |  1784.2   | 1279.02 |  2238.52 |  17:04 |  30:23   |  3847.8   |   6601.5    |            

            oReports1.AddRange(oReports.ToList());
            return oReports1;
        }


        #endregion


        #endregion





        #region " Private Utility Funtions "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strStreet"></param>
        /// <returns></returns>
        private string simpleStreetScrubber(string strStreet)
        {
            try
            {
                strStreet = " " + Strings.LCase(strStreet) + " ";
                if (Strings.InStr(1, strStreet, " dr ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " dr ", " drive ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " ave ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " ave ", " avenue ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " blvd ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " blvd ", " boulevard ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " st ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " st ", " street ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " e ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " e ", " east ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " w ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " w ", " west ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " s ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " s ", " south ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " n ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " n ", " north ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " cir ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " cir ", " circle ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " ct ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " ct ", " court ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " ne ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " ne ", " northeast ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " nw ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " nw ", " northwest ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " pkwy ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " pkwy ", " parkway ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " rd ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " rd ", " road ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " trl ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " trl ", " trail ", 1, 1, Constants.vbTextCompare);
                if (Strings.InStr(1, strStreet, " sq ", Constants.vbTextCompare) > 0)
                    strStreet = Strings.Replace(strStreet, " sq ", " square ", 1, 1, Constants.vbTextCompare);
            }
            catch (Exception ex)
            {
            }
            return strStreet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buff"></param>
        private void savePCMBuffer(string buff)
        {
            if (string.IsNullOrEmpty(buff))
                return;
            List<string> lBuff = new List<string>();
            if (PCMBuffers != null) { lBuff = PCMBuffers.ToList(); }
            lBuff.Add(buff);
            PCMBuffers = lBuff.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="intStart"></param>
        /// <returns></returns>
        private string splitCityStateCounty(string buff, int intStart)
        {
            string sRet = buff;
            try
            {
                if (intStart >= buff.Length)
                    intStart = buff.Length - 1;
                if (intStart < 1)
                    intStart = 1;
                if (Strings.InStr(1, buff, ";") > 0)
                    buff = buff.Split(';')[0];
                sRet = Strings.Mid(buff, intStart);
            }
            catch (Exception ex)
            {
                //do nothing
            }
            return sRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipOriginal"></param>
        /// <param name="zipNew"></param>
        /// <returns></returns>
        private bool isValidCAZip(string zipOriginal, ref string zipNew)
        {
            bool blnRet = false;
            try
            {
                if (string.IsNullOrEmpty(zipOriginal))
                    return false;
                string strTest = Strings.Trim(zipOriginal);
                if (strTest.Length < 6 | strTest.Length > 7)
                    return false;
                int intCt = 0;
                string strNew = "";
                if (strTest.Length < 7)
                    // add a space after the first 3 characters
                    strTest = Strings.Left(strTest, 3) + " " + Strings.Right(strTest, 3);
                // all character tests expect upper case letters
                strTest = strTest.ToUpper();
                for (int i = 0; i <= strTest.Length - 1; i++)
                {
                    char c = strTest[i];
                    switch (i)
                    {
                        case 0:
                            {
                                if (!isCharCAZipAlpha(c))
                                    return false;
                                break;
                            }

                        case 1:
                            {
                                if (!isCharCAZipNumeric(c))
                                    return false;
                                break;
                            }

                        case 2:
                            {
                                if (!isCharCAZipAlpha(c))
                                    return false;
                                break;
                            }

                        case 3:
                            {
                                if (c != ' ')
                                    return false;
                                break;
                            }

                        case 4:
                            {
                                if (!isCharCAZipNumeric(c))
                                    return false;
                                break;
                            }

                        case 5:
                            {
                                if (!isCharCAZipAlpha(c))
                                    return false;
                                break;
                            }

                        case 6:
                            {
                                if (!isCharCAZipNumeric(c))
                                    return false;
                                break;
                            }

                        default:
                            {
                                return false;
                            }
                    }
                }
                // if we get here the zip is a valid CA Postal Code
                blnRet = true;
                zipNew = strTest;
            }
            catch (Exception ex)
            {
            }

            return blnRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool isCharCAZipAlpha(char c)
        {
            char cStart = 'A';
            char cEnd = 'Z';
            char cLowerStart = 'a';
            char cLowerEnd = 'z';

            if ((int)c >= (int)cLowerStart && (int)c <= (int)cLowerEnd)
            {
                //Note:  should always be upper case so this may not be a valid test
                return true; // lower case found
            }
            else if (((int)c >= (int)cStart && (int)c <= (int)cEnd))
            {
                return true; // upper case found
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool isCharCAZipNumeric(char c)
        {
            int intVal = -1;
            if (!int.TryParse(c.ToString(), out intVal))
                return false;
            if (intVal >= 0 && intVal <= 9)
                return true;
            else
                return false;
        }


        #endregion
    }
}
