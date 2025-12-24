using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Web;


namespace NGLTrimbleServices
{
    

    [Serializable()]
    public class clsAllStop
    {
        private short mintStopNumber = 0;
        private string mstrStopName = "";
        private string mstrID1 = "";
        private string mstrID2 = "";
        private double mdblDistToPrev = 0;
        private double mdblTotalRouteCost = 0;
        private short mintSeqNbr = 0;
        private int mlngTruckNumber = 0;
        private string mstrTruckDesignator = "";
        private string mstrConsNumber = "";
        public string ConsNumber
        {
            get
            {
                return mstrConsNumber.Left(20);
            }
            set
            {
                mstrConsNumber = value;
            }
        }

        public double DistToPrev
        {
            get
            {
                return mdblDistToPrev;
            }
            set
            {
                mdblDistToPrev = value;
            }
        }

        public double TotalRouteCost
        {
            get
            {
                return mdblTotalRouteCost;
            }
            set
            {
                mdblTotalRouteCost = value;
            }
        }

        public short SeqNbr
        {
            get
            {
                return mintSeqNbr;
            }
            set
            {
                mintSeqNbr = value;
            }
        }

        public string TruckDesignator
        {
            get
            {
                return mstrTruckDesignator;
            }
            set
            {
                mstrTruckDesignator = value.Left(12);
            }
        }

        public int TruckNumber
        {
            get
            {
                return mlngTruckNumber;
            }
            set
            {
                mlngTruckNumber = value;
            }
        }

        public short StopNumber
        {
            get
            {
                return mintStopNumber;
            }
            set
            {
                mintStopNumber = value;
            }
        }

        public string Stopname
        {
            get
            {
                return mstrStopName;
            }
            set
            {
                mstrStopName = value.Left(20);
            }
        }

        public string ID1
        {
            get
            {
                return mstrID1;
            }
            set
            {
                mstrID1 = value.Left(15);
            }
        }

        public string ID2
        {
            get
            {
                return mstrID2;
            }
            set
            {
                mstrID2 = value.Left(10);
            }
        }
    }


    [Serializable()]
    public class clsAddress
    {
        private int mintBookControl = 0;
        public int BookControl
        {
            get
            {
                return mintBookControl;
            }
            set
            {
                mintBookControl = value;
            }
        }
        private int _StopId = 0;
        public int StopID
        {
            get
            {
                return _StopId;
            }
            set
            {
                _StopId = value;
            }
        }
        private string _strAddress = "";
        public string strAddress
        {
            get
            {
                return _strAddress;
            }
            set
            {
                _strAddress = value;
            }
        }

        private string _strCity = "";
        public string strCity
        {
            get
            {
                return _strCity;
            }
            set
            {
                _strCity = value;
            }
        }
        private string _strState = "";
        public string strState
        {
            get
            {
                return _strState;
            }
            set
            {
                _strState = value;
            }
        }
        private string _strZip = "";
        public string strZip
        {
            get
            {
                return _strZip;
            }
            set
            {
                _strZip = value;
            }
        }

        public string strCountry { get; set; }


        private string _formatedAddress = "";
        public string formatedAddress
        {
            get { return String.Format("{0}, {1}, {2}  {3}", strAddress, strCity, strState, strZip); }
         
        set { _formatedAddress = value; }
       
        }
    }   

    [Serializable()]
    public class clsPCMBadAddress
    {
        private int mintBookControl = 0;
        public int BookControl
        {
            get
            {
                return mintBookControl;
            }
            set
            {
                mintBookControl = value;
            }
        }

        private int mintLaneControl = 0;
        public int LaneControl
        {
            get
            {
                return mintLaneControl;
            }
            set
            {
                mintLaneControl = value;
            }
        }

        private clsAddress moobjOrig = new clsAddress();
        public clsAddress objOrig
        {
            get
            {
                return moobjOrig;
            }
            set
            {
                moobjOrig = value;
            }
        }

        private clsAddress moobjDest = new clsAddress();
        public clsAddress objDest
        {
            get
            {
                return moobjDest;
            }
            set
            {
                moobjDest = value;
            }
        }

        private clsAddress moobjPCMOrig = new clsAddress();
        public clsAddress objPCMOrig
        {
            get
            {
                return moobjPCMOrig;
            }
            set
            {
                moobjPCMOrig = value;
            }
        }

        private clsAddress moobjPCMDest = new clsAddress();
        public clsAddress objPCMDest
        {
            get
            {
                return moobjPCMDest;
            }
            set
            {
                moobjPCMDest = value;
            }
        }

        private string mstrMessage = "";
        public string Message
        {
            get
            {
                return mstrMessage;
            }
            set
            {
                mstrMessage = value;
            }
        }

        private double mdblBatchID = 0;
        public double BatchID
        {
            get
            {
                return mdblBatchID;
            }
            set
            {
                mdblBatchID = value;
            }
        }

        public int Item(int count)
        {
            return count;
        }
    }

    [Serializable()]
    public class clsPCMDataStop
    {
        private int mintBookControl = 0;
        public int BookControl
        {
            get
            {
                return mintBookControl;
            }
            set
            {
                mintBookControl = value;
            }
        }

        private int mintBookCustCompControl = 0;
        public int BookCustCompControl
        {
            get
            {
                return mintBookCustCompControl;
            }
            set
            {
                mintBookCustCompControl = value;
            }
        }

        private int mintBookLoadControl = 0;
        public int BookLoadControl
        {
            get
            {
                return mintBookLoadControl;
            }
            set
            {
                mintBookLoadControl = value;
            }
        }

        private int mintBookODControl = 0;
        public int BookODControl
        {
            get
            {
                return mintBookODControl;
            }
            set
            {
                mintBookODControl = value;
            }
        }

        private int mintBookStopNo = 0;
        public int BookStopNo
        {
            get
            {
                return mintBookStopNo;
            }
            set
            {
                mintBookStopNo = value;
            }
        }

        private int mintRouteType = 0;
        public int RouteType
        {
            get
            {
                return mintRouteType;
            }
            set
            {
                mintRouteType = value;
            }
        }

        private int mintDistType = 0;
        public int DistType
        {
            get
            {
                return mintDistType;
            }
            set
            {
                mintDistType = value;
            }
        }

        public string BookOrigName { get; set; }
        public string BookDestName { get; set; }

        public string BookOrigID { get; set; }
        public string BookDestID { get; set; }

        public string BookOrigRegion { get; set; }
        public string BookDestRegion { get; set; }

        public string BookOrigStartTime { get; set; }
        public string BookDestStartTime { get; set; }

        public string BookOrigEndTime { get; set; }
        public string BookDestEndTime { get; set; }


        private string mstrBookOrigZip = "";
        public string BookOrigZip
        {
            get
            {
                return mstrBookOrigZip;
            }
            set
            {
                mstrBookOrigZip = value;
            }
        }

        private string mstrBookDestZip = "";
        public string BookDestZip
        {
            get
            {
                return mstrBookDestZip;
            }
            set
            {
                mstrBookDestZip = value;
            }
        }

        private string mstrBookOrigAddress1 = "";
        public string BookOrigAddress1
        {
            get
            {
                return mstrBookOrigAddress1;
            }
            set
            {
                mstrBookOrigAddress1 = value;
            }
        }

        private string mstrBookDestAddress1 = "";
        public string BookDestAddress1
        {
            get
            {
                return mstrBookDestAddress1;
            }
            set
            {
                mstrBookDestAddress1 = value;
            }
        }

        private string mstrBookOrigCity = "";
        public string BookOrigCity
        {
            get
            {
                return mstrBookOrigCity;
            }
            set
            {
                mstrBookOrigCity = value;
            }
        }

        private string mstrBookDestCity = "";
        public string BookDestCity
        {
            get
            {
                return mstrBookDestCity;
            }
            set
            {
                mstrBookDestCity = value;
            }
        }

        private string mstrBookOrigState = "";
        public string BookOrigState
        {
            get
            {
                return mstrBookOrigState;
            }
            set
            {
                mstrBookOrigState = value;
            }
        }

        private string mstrBookDestState = "";
        public string BookDestState
        {
            get
            {
                return mstrBookDestState;
            }
            set
            {
                mstrBookDestState = value;
            }
        }

        private string mstrBookProNumber = "";
        public string BookProNumber
        {
            get
            {
                return mstrBookProNumber;
            }
            set
            {
                mstrBookProNumber = value;
            }
        }

        private bool mblnLaneOriginAddressUse = true;
        public bool LaneOriginAddressUse
        {
            get
            {
                return mblnLaneOriginAddressUse;
            }
            set
            {
                mblnLaneOriginAddressUse = value;
            }
        }
    }

    [Serializable()]
    public class clsPCMStop
    {
        private int mintBookLoadControl = 0;
        public int BookLoadControl
        {
            get
            {
                return mintBookLoadControl;
            }
            set
            {
                mintBookLoadControl = value;
            }
        }

        private int mintBookODControl = 0;
        public int BookODControl
        {
            get
            {
                return mintBookODControl;
            }
            set
            {
                mintBookODControl = value;
            }
        }

        private short mshtLoopCt = 0;
        public short LoopCt
        {
            get
            {
                return mshtLoopCt;
            }
            set
            {
                mshtLoopCt = value;
            }
        }

        private short mshtStopNumber = 0;
        public short StopNumber
        {
            get
            {
                return mshtStopNumber;
            }
            set
            {
                mshtStopNumber = value;
            }
        }

        private short mshtStopSeq = 0;
        public short StopSeq
        {
            get
            {
                return mshtStopSeq;
            }
            set
            {
                mshtStopSeq = value;
            }
        }

        private double mdblLegMiles = 0;
        public double LegMiles
        {
            get
            {
                return mdblLegMiles;
            }
            set
            {
                mdblLegMiles = value;
            }
        }

        private double mdblTotalMiles = 0;
        public double TotalMiles
        {
            get
            {
                return mdblTotalMiles;
            }
            set
            {
                mdblTotalMiles = value;
            }
        }

        private double mdblLegCost = 0;
        public double LegCost
        {
            get
            {
                return mdblLegCost;
            }
            set
            {
                mdblLegCost = value;
            }
        }

        private double mdblTotalCost = 0;
        public double TotalCost
        {
            get
            {
                return mdblTotalCost;
            }
            set
            {
                mdblTotalCost = value;
            }
        }

        private string mstrZip = "";
        public string Zip
        {
            get
            {
                return mstrZip;
            }
            set
            {
                mstrZip = value;
            }
        }

        private string mstrCity = "";
        public string City
        {
            get
            {
                return mstrCity;
            }
            set
            {
                mstrCity = value;
            }
        }

        private string mstrState = "";
        public string State
        {
            get
            {
                return mstrState;
            }
            set
            {
                mstrState = value;
            }
        }

        private string mstrStreet = "";
        public string Street
        {
            get
            {
                return mstrStreet;
            }
            set
            {
                mstrStreet = value;
            }
        }

        private string mstrStopName = "";
        public string StopName
        {
            get
            {
                return mstrStopName;
            }
            set
            {
                mstrStopName = value;
            }
        }

        private string mstrLegTime = "";
        public string LegTime
        {
            get
            {
                return mstrLegTime;
            }
            set
            {
                mstrLegTime = value;
            }
        }

        private string mstrTotalTime = "";
        public string TotalTime
        {
            get
            {
                return mstrTotalTime;
            }
            set
            {
                mstrTotalTime = value;
            }
        }

        private string mstrPCMilerStreet = "";
        public string PCMilerStreet
        {
            get
            {
                return mstrPCMilerStreet;
            }
            set
            {
                mstrPCMilerStreet = value;
            }
        }

        private string mstrPCMilerState = "";
        public string PCMilerState
        {
            get
            {
                return mstrPCMilerState;
            }
            set
            {
                mstrPCMilerState = value;
            }
        }

        private string mstrPCMilerCity = "";
        public string PCMilerCity
        {
            get
            {
                return mstrPCMilerCity;
            }
            set
            {
                mstrPCMilerCity = value;
            }
        }

        private bool mblnMatched = false;
        public bool Matched
        {
            get
            {
                return mblnMatched;
            }
            set
            {
                mblnMatched = value;
            }
        }
    }

    [Serializable()]
    public class clsPCMReturn 
    {
        private int _intRetVal = 0;
        public int RetVal
        {
            get
            {
                return _intRetVal;
            }
            set
            {
                _intRetVal = value;
            }
        }
        private string _strMessage = "";
        public string Message
        {
            get
            {
                return _strMessage;
            }
            set
            {
                _strMessage = value;
            }
        }
    }

    [Serializable()]
    public class clsFMStopData
    {
        private int mintBookControl = 0;
        public int BookControl
        {
            get
            {
                return mintBookControl;
            }
            set
            {
                mintBookControl = value;
            }
        }

        private int mintBookCustCompControl = 0;
        public int BookCustCompControl
        {
            get
            {
                return mintBookCustCompControl;
            }
            set
            {
                mintBookCustCompControl = value;
            }
        }

        private int mintBookLoadControl = 0;
        public int BookLoadControl
        {
            get
            {
                return mintBookLoadControl;
            }
            set
            {
                mintBookLoadControl = value;
            }
        }

        private int mintBookODControl = 0;
        public int BookODControl
        {
            get
            {
                return mintBookODControl;
            }
            set
            {
                mintBookODControl = value;
            }
        }
        
        private string mstrBookProNumber = "";
        public string BookProNumber
        {
            get
            {
                return mstrBookProNumber;
            }
            set
            {
                mstrBookProNumber = value;
            }
        }

        private bool mblnLaneOriginAddressUse = true;
        public bool LaneOriginAddressUse
        {
            get
            {
                return mblnLaneOriginAddressUse;
            }
            set
            {
                mblnLaneOriginAddressUse = value;
            }
        }
        private int mintRouteType = 0;
        public int RouteType
        {
            get
            {
                return mintRouteType;
            }
            set
            {
                mintRouteType = value;
            }
        }

        private int mintDistType = 0;
        public int DistType
        {
            get
            {
                return mintDistType;
            }
            set
            {
                mintDistType = value;
            }
        }

        private int mintStopNumber = 0;
        public int StopNumber
        {
            get
            {
                return mintStopNumber;
            }
            set
            {
                mintStopNumber = value;
            }
        }

        private int mintSeqNumber = 0;
        public int SeqNumber
        {
            get
            {
                return mintSeqNumber;
            }
            set
            {
                mintSeqNumber = value;
            }
        }

        private double mdblLegMiles = 0;
        public double LegMiles
        {
            get
            {
                return mdblLegMiles;
            }
            set
            {
                mdblLegMiles = value;
            }
        }

        private double mdblTotalMiles = 0;
        public double TotalMiles
        {
            get
            {
                return mdblTotalMiles;
            }
            set
            {
                mdblTotalMiles = value;
            }
        }

        private double mdblLegCost = 0;
        public double LegCost
        {
            get
            {
                return mdblLegCost;
            }
            set
            {
                mdblLegCost = value;
            }
        }

        private double mdblTotalCost = 0;
        public double TotalCost
        {
            get
            {
                return mdblTotalCost;
            }
            set
            {
                mdblTotalCost = value;
            }
        }

        private string mstrLegTime = "";
        public string LegTime
        {
            get
            {
                return mstrLegTime;
            }
            set
            {
                mstrLegTime = value;
            }
        }

        private string mstrTotalTime = "";
        public string TotalTime
        {
            get
            {
                return mstrTotalTime;
            }
            set
            {
                mstrTotalTime = value;
            }
        }

        private double mdblLegTolls = 0;
        public double LegTolls
        {
            get
            {
                return mdblLegTolls;
            }
            set
            {
                mdblLegTolls = value;
            }
        }

        private double mdblTotalTolls = 0;
        public double TotalTolls
        {
            get
            {
                return mdblTotalTolls;
            }
            set
            {
                mdblTotalTolls = value;
            }
        }

        private double mdblLegESTCHG = 0;
        public double LegESTCHG
        {
            get
            {
                return mdblLegESTCHG;
            }
            set
            {
                mdblLegESTCHG = value;
            }
        }

        private double mdblTotalESTCHG = 0;
        public double TotalESTCHG
        {
            get
            {
                return mdblTotalESTCHG;
            }
            set
            {
                mdblTotalESTCHG = value;
            }
        }

        private string mstrZip = "";
        public string Zip
        {
            get
            {
                return mstrZip;
            }
            set
            {
                mstrZip = value;
            }
        }

        private string mstrCity = "";
        public string City
        {
            get
            {
                return mstrCity;
            }
            set
            {
                mstrCity = value;
            }
        }

        private string mstrState = "";
        public string State
        {
            get
            {
                return mstrState;
            }
            set
            {
                mstrState = value;
            }
        }

        private string mstrStreet = "";
        public string Street
        {
            get
            {
                return mstrStreet;
            }
            set
            {
                mstrStreet = value;
            }
        }

        //Modified by RHR for v-8.5.0.001 on 11/19/2021 added country property
        private string mstrCountry = "";
        public string Country
        {
            get
            {
                return mstrCountry;
            }
            set
            {
                mstrCountry = value;
            }
        }

        private string mstrStopName = "";
        public string StopName
        {
            get
            {
                return mstrStopName;
            }
            set
            {
                mstrStopName = value;
            }
        }

        private string mstrPCMilerStreet = "";
        public string PCMilerStreet
        {
            get
            {
                return mstrPCMilerStreet;
            }
            set
            {
                mstrPCMilerStreet = value;
            }
        }

        private string mstrPCMilerState = "";
        public string PCMilerState
        {
            get
            {
                return mstrPCMilerState;
            }
            set
            {
                mstrPCMilerState = value;
            }
        }

        private string mstrPCMilerCity = "";
        public string PCMilerCity
        {
            get
            {
                return mstrPCMilerCity;
            }
            set
            {
                mstrPCMilerCity = value;
            }
        }

        private string mstrPCMilerZip = "";
        public string PCMilerZip
        {
            get
            {
                return mstrPCMilerZip;
            }
            set
            {
                mstrPCMilerZip = value;
            }
        }

        //Modified by RHR for v-8.5.0.001 on 11/19/2021 added country property
        private string mstrPCMilerCountry = "";
        public string PCMilerCountry
        {
            get
            {
                return mstrPCMilerCountry;
            }
            set
            {
                mstrPCMilerCountry = value;
            }
        }

        private bool mblnMatched = false;
        public bool Matched
        {
            get
            {
                return mblnMatched;
            }
            set
            {
                mblnMatched = value;
            }
        }

        private bool mblnLocationisOrigin = false;
        public bool LocationisOrigin
        {
            get
            {
                return mblnLocationisOrigin;
            }
            set
            {
                mblnLocationisOrigin = value;
            }
        }

        private bool mblnAddressValid = false;
        public bool AddressValid
        {
            get
            {
                return mblnAddressValid;
            }
            set
            {
                mblnAddressValid = value;
            }
        }

        private bool mblnLogBadAddress = false;
        public bool LogBadAddress
        {
            get
            {
                return mblnLogBadAddress;
            }
            set
            {
                mblnLogBadAddress = value;
            }
        }

        private string mstrWarning = "";
        public string Warning
        {
            get
            {
                return mstrWarning;
            }
            set
            {
                mstrWarning = value;
            }
        }
    }
    
    [Serializable()]
    public class clsPCMReportRecord
    {
        private int mintStopNumber = 0;
        public int StopNumber
        {
            get
            {
                return mintStopNumber;
            }
            set
            {
                mintStopNumber = value;
            }
        }

        private int mintSeqNumber = 0;
        public int SeqNumber
        {
            get
            {
                return mintSeqNumber;
            }
            set
            {
                mintSeqNumber = value;
            }
        }

        private double mdblLegMiles = 0;
        public double LegMiles
        {
            get
            {
                return mdblLegMiles;
            }
            set
            {
                mdblLegMiles = value;
            }
        }

        private double mdblTotalMiles = 0;
        public double TotalMiles
        {
            get
            {
                return mdblTotalMiles;
            }
            set
            {
                mdblTotalMiles = value;
            }
        }

        private double mdblLegCost = 0;
        public double LegCost
        {
            get
            {
                return mdblLegCost;
            }
            set
            {
                mdblLegCost = value;
            }
        }

        private double mdblTotalCost = 0;
        public double TotalCost
        {
            get
            {
                return mdblTotalCost;
            }
            set
            {
                mdblTotalCost = value;
            }
        }

        private string mstrLegTime = "";
        public string LegTime
        {
            get
            {
                return mstrLegTime;
            }
            set
            {
                mstrLegTime = value;
            }
        }

        private string mstrTotalTime = "";
        public string TotalTime
        {
            get
            {
                return mstrTotalTime;
            }
            set
            {
                mstrTotalTime = value;
            }
        }

        private double mdblLegTolls = 0;
        public double LegTolls
        {
            get
            {
                return mdblLegTolls;
            }
            set
            {
                mdblLegTolls = value;
            }
        }

        private double mdblTotalTolls = 0;
        public double TotalTolls
        {
            get
            {
                return mdblTotalTolls;
            }
            set
            {
                mdblTotalTolls = value;
            }
        }

        private double mdblLegESTCHG = 0;
        public double LegESTCHG
        {
            get
            {
                return mdblLegESTCHG;
            }
            set
            {
                mdblLegESTCHG = value;
            }
        }

        private double mdblTotalESTCHG = 0;
        public double TotalESTCHG
        {
            get
            {
                return mdblTotalESTCHG;
            }
            set
            {
                mdblTotalESTCHG = value;
            }
        }


        private string mstrZip = "";
        public string Zip
        {
            get
            {
                return mstrZip;
            }
            set
            {
                mstrZip = value;
            }
        }

        private string mstrCity = "";
        public string City
        {
            get
            {
                return mstrCity;
            }
            set
            {
                mstrCity = value;
            }
        }

        private string mstrState = "";
        public string State
        {
            get
            {
                return mstrState;
            }
            set
            {
                mstrState = value;
            }
        }

        private string mstrStreet = "";
        public string Street
        {
            get
            {
                return mstrStreet;
            }
            set
            {
                mstrStreet = value;
            }
        }

        private string mstrStopName = "";
        public string StopName
        {
            get
            {
                return mstrStopName;
            }
            set
            {
                mstrStopName = value;
            }
        }
    }

    [Serializable()]
    public class clsPCMStopEx
    {
        private int mintBookControl = 0;
        public int BookControl
        {
            get
            {
                return mintBookControl;
            }
            set
            {
                mintBookControl = value;
            }
        }

        private int mintBookLoadControl = 0;
        public int BookLoadControl
        {
            get
            {
                return mintBookLoadControl;
            }
            set
            {
                mintBookLoadControl = value;
            }
        }

        private int mintBookODControl = 0;
        public int BookODControl
        {
            get
            {
                return mintBookODControl;
            }
            set
            {
                mintBookODControl = value;
            }
        }


        private int mintStopNumber = 0;
        public int StopNumber
        {
            get
            {
                return mintStopNumber;
            }
            set
            {
                mintStopNumber = value;
            }
        }

        private int mintSeqNumber = 0;
        public int SeqNumber
        {
            get
            {
                return mintSeqNumber;
            }
            set
            {
                mintSeqNumber = value;
            }
        }

        private double mdblLegMiles = 0;
        public double LegMiles
        {
            get
            {
                return mdblLegMiles;
            }
            set
            {
                mdblLegMiles = value;
            }
        }

        private double mdblTotalMiles = 0;
        public double TotalMiles
        {
            get
            {
                return mdblTotalMiles;
            }
            set
            {
                mdblTotalMiles = value;
            }
        }

        private double mdblLegCost = 0;
        public double LegCost
        {
            get
            {
                return mdblLegCost;
            }
            set
            {
                mdblLegCost = value;
            }
        }

        private double mdblTotalCost = 0;
        public double TotalCost
        {
            get
            {
                return mdblTotalCost;
            }
            set
            {
                mdblTotalCost = value;
            }
        }

        private string mstrLegTime = "";
        public string LegTime
        {
            get
            {
                return mstrLegTime;
            }
            set
            {
                mstrLegTime = value;
            }
        }

        private string mstrTotalTime = "";
        public string TotalTime
        {
            get
            {
                return mstrTotalTime;
            }
            set
            {
                mstrTotalTime = value;
            }
        }

        private double mdblLegTolls = 0;
        public double LegTolls
        {
            get
            {
                return mdblLegTolls;
            }
            set
            {
                mdblLegTolls = value;
            }
        }

        private double mdblTotalTolls = 0;
        public double TotalTolls
        {
            get
            {
                return mdblTotalTolls;
            }
            set
            {
                mdblTotalTolls = value;
            }
        }

        private double mdblLegESTCHG = 0;
        public double LegESTCHG
        {
            get
            {
                return mdblLegESTCHG;
            }
            set
            {
                mdblLegESTCHG = value;
            }
        }

        private double mdblTotalESTCHG = 0;
        public double TotalESTCHG
        {
            get
            {
                return mdblTotalESTCHG;
            }
            set
            {
                mdblTotalESTCHG = value;
            }
        }

        private string mstrZip = "";
        public string Zip
        {
            get
            {
                return mstrZip;
            }
            set
            {
                mstrZip = value;
            }
        }

        private string mstrCity = "";
        public string City
        {
            get
            {
                return mstrCity;
            }
            set
            {
                mstrCity = value;
            }
        }

        private string mstrState = "";
        public string State
        {
            get
            {
                return mstrState;
            }
            set
            {
                mstrState = value;
            }
        }

        private string mstrStreet = "";
        public string Street
        {
            get
            {
                return mstrStreet;
            }
            set
            {
                mstrStreet = value;
            }
        }

        private string mstrStopName = "";
        public string StopName
        {
            get
            {
                return mstrStopName;
            }
            set
            {
                mstrStopName = value;
            }
        }


        private string mstrPCMilerStreet = "";
        public string PCMilerStreet
        {
            get
            {
                return mstrPCMilerStreet;
            }
            set
            {
                mstrPCMilerStreet = value;
            }
        }

        private string mstrPCMilerState = "";
        public string PCMilerState
        {
            get
            {
                return mstrPCMilerState;
            }
            set
            {
                mstrPCMilerState = value;
            }
        }

        private string mstrPCMilerCity = "";
        public string PCMilerCity
        {
            get
            {
                return mstrPCMilerCity;
            }
            set
            {
                mstrPCMilerCity = value;
            }
        }

        private bool mblnMatched = false;
        public bool Matched
        {
            get
            {
                return mblnMatched;
            }
            set
            {
                mblnMatched = value;
            }
        }

        private bool mblnLocationisOrigin = false;
        public bool LocationisOrigin
        {
            get
            {
                return mblnLocationisOrigin;
            }
            set
            {
                mblnLocationisOrigin = value;
            }
        }
    }

    //[Serializable()]
    //public class ReportType
    //{
    //    public string __type { get; set; }
    //    public bool THoursWithSeconds { get; set; }
    //}

    [Serializable()]
    public class clsSimpleStop 
    {
        private string _strAddress = "";
        public string Address
        {
            get
            {
                return _strAddress;
            }
            set
            {
                _strAddress = value;
            }
        }
        private int _intStopNumber = 0;
        public int StopNumber
        {
            get
            {
                return _intStopNumber;
            }
            set
            {
                _intStopNumber = value;
            }
        }
        private double _dblTotalMiles = 0;
        public double TotalMiles
        {
            get
            {
                return _dblTotalMiles;
            }
            set
            {
                _dblTotalMiles = value;
            }
        }
        private double _dblLegMiles = 0;
        public double LegMiles
        {
            get
            {
                return _dblLegMiles;
            }
            set
            {
                _dblLegMiles = value;
            }
        }


        private double _dbllegCost = 0;
        public double LegCost
        {
            get
            {
                return _dbllegCost;
            }
            set
            {
                _dbllegCost = value;
            }
        }
        private double _dblTotalCost = 0;
        public double TotalCost
        {
            get
            {
                return _dblTotalCost;
            }
            set
            {
                _dblTotalCost = value;
            }
        }
        private double _dbllegHours = 0;
        public double LegHours
        {
            get
            {
                return _dbllegHours;
            }
            set
            {
                _dbllegHours = value;
            }
        }
        private double _dblTotalHours = 0;
        public double TotalHours
        {
            get
            {
                return _dblTotalHours;
            }
            set
            {
                _dblTotalHours = value;
            }
        }
    }


    [Serializable()]
    public class clsTrimbleReportParams
    {
        #region "Utilities"

        public static string buildBoolJSONProperty(string sKey, bool bValue)
        {
           if (bValue == true) { return string.Format("\"{0}\": {1}", sKey, "true"); } else { return string.Format("\"{0}\": {1}", sKey, "false"); }
        }
        #endregion

        #region "Typical Routing Options"
        public  string buildReportOptions()
        {
            StringBuilder sbOpt = new StringBuilder();
            sbOpt.Append("\"Options\": {");
            sbOpt.Append(buildBoolJSONProperty("HubRouting", HubRouting));
            sbOpt.Append(",");
            sbOpt.Append(buildBoolJSONProperty("UseTollData", UseTollData));
            sbOpt.AppendFormat(",\"FuelUnits\": {0} ",FuelUnits);
            sbOpt.AppendFormat(",\"RouteOptimization\": {0}", RouteOptimization);
            sbOpt.AppendFormat(",\"RoutingType\": {0}", RouteType);
            sbOpt.AppendFormat(",\"TruckStyle\": {0}", (int)TruckStyle);
            sbOpt.AppendFormat(",\"DistanceUnits\": {0}", DistanceUnits);
            sbOpt.Append(",");
            sbOpt.Append(buildBoolJSONProperty("BordersOpen", BordersOpen));
            sbOpt.Append("}");
                      
         
            return sbOpt.ToString();
        }


        private bool _hubRouting = false;
        public bool HubRouting
        {
            get
            {
                return _hubRouting;
            }
            set
            {
                _hubRouting = value;
            }
        }

        private int _fuelUnits = 0;
        /// <summary>
        /// Gallons = 0,  Liters = 1,
        /// </summary>
        public int FuelUnits
        {
            get
            {
                return _fuelUnits;
            }
            set
            {
                _fuelUnits = value;
            }
        }

        public bool UseTollData { get; set; }
        /// <summary>
        /// 0 none (keep stop numbers) , 1 ThroughALL (sequence) ,  new 2 Keep Last Stop 
        /// </summary>
        public int RouteOptimization { get; set; }

        private int _routeType = 0;
        /// <summary>
        /// 0 Practical, 1 Shortest, 2 Fastest  parameter  PCMilerRouteType  Use 0 = Practical, 1 = Shortest, 2 = Fastest 
        /// 4 = Air routing is not supported?  convert to 2 
        /// 
        /// </summary>
        public int RouteType
        {
            get
            {
                return _routeType;
            }
            set
            {
                if (value < 0) { value = 0; }
                if (value > 2) { value = 2; }
                _routeType = value;
            }
        }

        private NGLTrimbleServices.TrimbleServiceReference.TruckStyle _TruckStyle = TrimbleServiceReference.TruckStyle.FiftyThreeSemiTrailer;
        /// <summary>
        /// Default is 4  FiftyThreeSemiTrailer
        /// </summary>
        public NGLTrimbleServices.TrimbleServiceReference.TruckStyle TruckStyle { get { return _TruckStyle; } set { _TruckStyle = value; } }

        //default is the test key
        private string _APIKey = "C36349D0A5F5D440AAC0CB8A0287F02C"; 
        public string APIKey{get{return _APIKey;}set {_APIKey = value; }}

        public int DistanceUnits { get; set; }
        // "DistanceUnits": 0,
        //"TollDiscourage": true,
        //"BordersOpen": true,
        public bool BordersOpen { get; set; }
        #endregion

        #region "Non-Typical Routing Options"

        #endregion

        #region "Deprecated"

        #endregion

        private int _StopId = 0;
        public int StopID
        {
            get
            {
                return _StopId;
            }
            set
            {
                _StopId = value;
            }
        }
       

        private int[] _afSetIDs;
        public int[] afSetIDs
        {
            get
            {
                if (_afSetIDs == null) { _afSetIDs = new int[] { 1, 2, 3 }; }
                return _afSetIDs;
            }
            set
            {
                _afSetIDs = value;
            }
        }
        private bool _hwyOnly = false;
        public bool hwyOnly
        {
            get
            {
                return _hwyOnly;
            }
            set
            {
                _hwyOnly = value;
            }
        }
        private bool _custRdSpeeds = false;
        public bool CustRdSpeeds
        {
            get
            {
                return _custRdSpeeds;
            }
            set
            {
                _custRdSpeeds = value;
            }
        }
        private bool _avoidFavors = false;
        public bool AvoidFavors
        {
            get
            {
                return _avoidFavors;
            }
            set
            {
                _avoidFavors = value;
            }
        }

        private int _overrideClass = 0;
        public int OverrideClass
        {
            get
            {
                return _overrideClass;
            }
            set
            {
                _overrideClass = value;
            }
        }

        private int _distUnits = 0;
        public int DistUnits
        {
            get
            {
                return _distUnits;
            }
            set
            {
                _distUnits = value;
            }
        }
        
        private bool _avoidTolls = false;
        public bool AvoidTolls
        {
            get
            {
                return _avoidTolls;
            }
            set
            {
                _avoidTolls = value;
            }
        }
        private bool _inclFerryDist = false;
        public bool InclFerryDist
        {
            get
            {
                return _inclFerryDist;
            }
            set
            {
                _inclFerryDist = value;
            }
        }
        private bool _openBorders = false;
        public bool OpenBorders
        {
            get
            {
                return _openBorders;
            }
            set
            {
                _openBorders = value;
            }
        }
        private bool _restrOverrides = false;
        public bool RestrOverrides
        {
            get
            {
                return _restrOverrides;
            }
            set
            {
                _restrOverrides = value;
            }
        }

        private int _hazMat = 0;
        public int HazMat
        {
            get
            {
                return _hazMat;
            }
            set
            {
                _hazMat = value;
            }
        }
        
        private string _routeOpt = "";
        public string RouteOpt
        {
            get
            {
                return _routeOpt;
            }
            set
            {
                _routeOpt = value;
            }
        }
        private int _lang = 0;
        public int Language
        {
            get
            {
                return _lang;
            }
            set
            {
                _lang = value;
            }
        }


        /// <summary>
        /// 0 Truck, 1 LightTruck, 2 Auto
        /// </summary>
        public int VehicleType { get; set; }
        private int _vehDimUnits = 0;
        public int VehDimUnits
        {
            get
            {
                return _vehDimUnits;
            }
            set
            {
                _vehDimUnits = value;
            }
        }

        private string _vehHeight = "";//Need to check default Height
        public string VehHeight
        {
            get
            {
                return _vehHeight;
            }
            set
            {
                _vehHeight = value;
            }
        }
        private string _vehLength = ""; //Need to check default Length
        public string VehLength
        {
            get
            {
                return _vehLength;
            }
            set
            {
                _vehLength = value;
            }
        }
        private string _vehWidth = ""; //Need to check default width
        public string VehWidth
        {
            get
            {
                return _vehWidth;
            }
            set
            {
                _vehWidth = value;
            }
        }
        private string _vehWeight = ""; //Need to check default weight
        public string VehWeight
        {
            get
            {
                return _vehWeight;
            }
            set
            {
                _vehWeight = value;
            }
        }
        private int _axles = 5;
        public int Axles
        {
            get
            {
                return _axles;
            }
            set
            {
                _axles = value;
            }
        }
        private int _truckConfig = 0;
        public int TruckConfig
        {
            get
            {
                return _truckConfig;
            }
            set
            {
                _truckConfig = value;
            }
        }
       


        private bool _LCV = false;
        public bool LCV
        {
            get
            {
                return _LCV;
            }
            set
            {
                _LCV = value;
            }
        }

        private bool _inclTollData = false;
        public bool InclTollData
        {
            get
            {
                return _inclTollData;
            }
            set
            {
                _inclTollData = value;
            }
        }

        private double _fuelEconLoad = 0.0;
        public double FuelEconLoad
        {
            get
            {
                return _fuelEconLoad;
            }
            set
            {
                _fuelEconLoad = value;
            }
        }
        private double _fuelEconEmpty = 0.0;
        public double FuelEconEmpty
        {
            get
            {
                return _fuelEconEmpty;
            }
            set
            {
                _fuelEconEmpty = value;
            }
        }

        private double _costPerFuelUnit = 0.0;
        public double CostPerFuelUnit
        {
            get
            {
                return _costPerFuelUnit;
            }
            set
            {
                _costPerFuelUnit = value;
            }
        }
        private double _costGHG = 0.0;
        public double CostGHG
        {
            get
            {
                return _costGHG;
            }
            set
            {
                _costGHG = value;
            }
        }
        private double _costMaintLoad = 0.0;
        public double CostMaintLoad
        {
            get
            {
                return _costMaintLoad;
            }
            set
            {
                _costMaintLoad = value;
            }
        }
        private double _costMaintEmpty = 0.0;
        public double CostMaintEmpty
        {
            get
            {
                return _costMaintEmpty;
            }
            set
            {
                _costMaintEmpty = value;
            }
        }
        private double _costTimeLoad = 0.0;
        public double CostTimeLoad
        {
            get
            {
                return _costTimeLoad;
            }
            set
            {
                _costTimeLoad = value;
            }
        }
        private double _costTimeEmpty = 0.0;
        public double CostTimeEmpty
        {
            get
            {
                return _costTimeEmpty;
            }
            set
            {
                _costTimeEmpty = value;
            }
        }
        private double _exchangeRate = 0.0;
        public double ExchangeRate
        {
            get
            {
                return _exchangeRate;
            }
            set
            {
                _exchangeRate = value;
            }
        }

        private string _tollPlan = "";
        public string TollPlan
        {
            get
            {
                return _tollPlan;
            }
            set
            {
                _tollPlan = value;
            }
        }
        private bool _condenseDirections = false;
        public bool CondenseDirections
        {
            get
            {
                return _condenseDirections;
            }
            set
            {
                _condenseDirections = value;
            }
        }
        private int _region = 4;//NA default
        public int Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
            }
        }

        private string _dataVersion = "Current";
        public string DataVersion
        {
            get
            {
                return _dataVersion;
            }
            set
            {
                _dataVersion = value;
            }
        }

        public int RouteID { get; set; }
        public List<string> Stops { get; set; }
        public string Miles { get; set; }
    }
    [Serializable()]
    public class Address
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public object SPLC { get; set; }
        public int CountryPostalFilter { get; set; }
        public int AbbreviationFormat { get; set; }
        public string StateName { get; set; }
        public string StateAbbreviation { get; set; }
        public string CountryAbbreviation { get; set; }
    }

    [Serializable()]
    public class Coords
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
    }

    [Serializable()]
    public class Stop
    {
        public Address Address { get; set; }
        public Coords Coords { get; set; }
        public int Region { get; set; }
        public string Label { get; set; }
        public string PlaceName { get; set; }
        public string TimeZone { get; set; }
        public List<object> Errors { get; set; }
        public object SpeedLimitInfo { get; set; }
        public string ConfidenceLevel { get; set; }
        public double DistanceFromRoad { get; set; }
        public object CrossStreet { get; set; }
    }

    [Serializable()]
    public class ReportLine
    {
        public Stop Stop { get; set; }
        public string LMiles { get; set; }
        public string TMiles { get; set; }
        public string LCostMile { get; set; }
        public string TCostMile { get; set; }
        public string LHours { get; set; }
        public string THours { get; set; }
        public string LTolls { get; set; }
        public string TTolls { get; set; }
        public string LEstghg { get; set; }
        public string TEstghg { get; set; }
        public object EtaEtd { get; set; }
    }

    [Serializable()]
    public class clsMileage
    {
        public string _type { get; set; }
        public object RouteID { get; set; }
        public List<ReportLine> ReportLines { get; set; }
        public bool TrafficDataUsed { get; set; }
    }

    [Serializable()]
    public class MileageReport
    {
        public List<clsMileage> MileageArray { get; set; }
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Serializable()]
    public class clsAllStops 
    {
        private List<clsAllStop> mcolAllStops;
        // Public ReadOnly Property AllStops() As Collection Implements IclsAllStops.AllStops
        // Get
        // Return mcolAllStops
        // End Get
        // End Property

        public int[] mintBadAddressControls;
      

        private string mstrFailedAddressMessage = "";
        public string FailedAddressMessage
        {
            get
            {
                return mstrFailedAddressMessage;
            }
            set
            {
                mstrFailedAddressMessage = value;
            }
        }

        private int mintBadAddressCount = 0;
        public int BadAddressCount
        {
            get
            {
                return mintBadAddressCount;
            }
            set
            {
                mintBadAddressCount = value;
            }
        }

        private double mdblTotalMiles = 0;
        public double TotalMiles
        {
            get
            {
                return mdblTotalMiles;
            }
            set
            {
                mdblTotalMiles = value;
            }
        }

        private string mstrOriginZip = "";
        public string OriginZip
        {
            get
            {
                return mstrOriginZip;
            }
            set
            {
                mstrOriginZip = value;
            }
        }

        private string mstrDestZip = "";
        public string DestZip
        {
            get
            {
                return mstrDestZip;
            }
            set
            {
                mstrDestZip = value;
            }
        }

        private double mdblAutoCorrectBadLaneZipCodes = 0;
        public double AutoCorrectBadLaneZipCodes
        {
            get
            {
                return mdblAutoCorrectBadLaneZipCodes;
            }
            set
            {
                mdblAutoCorrectBadLaneZipCodes = value;
            }
        }

        private double mdblBatchID = 0;
        public double BatchID
        {
            get
            {
                return mdblBatchID;
            }
            set
            {
                mdblBatchID = value;
            }
        }

        private string mstrLastError = "";
        public string LastError
        {
            get
            {
                return mstrLastError;
            }
        }

        private string mstrConsNumber = "";
        public string ConsNumber
        {
            get
            {
                
                return mstrConsNumber;
            }
            set
            {
                mstrConsNumber = value;
            }
        }
        public clsAllStops() : base()
        {
            mcolAllStops = new List<clsAllStop>();
        }

        
        public string Item(int ItemNo)
        {
            return ItemNo.ToString();
        }
        public clsAllStop Add(short shtStopNumber, string strStopName, string strID1, string strID2, string strTruckName, int intTruckNumber, short shtSeqNbr, double dblDistToPrev, double dblTotalRouteCost, string strConsNumber)
        {
            // create a new object
            clsAllStop objNewMember = new clsAllStop();
            try
            {
                // set the properties passed into the method
                {
                    var withBlock = objNewMember;
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
                if (shtStopNumber == 0)
                    mcolAllStops.Add(objNewMember);
                else
                    //mcolAllStops.Add(objNewMember, "k" + shtStopNumber);
                return objNewMember;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                return null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                objNewMember = null/* TODO Change to default(_) if this is not a reference type */;
            }
            return null/* TODO Change to default(_) if this is not a reference type */;
        }

        public bool Add(clsAllStop objNewMember)
        {
            bool blnRet = false;
            try
            {
                if (objNewMember.StopNumber == 0)
                    mcolAllStops.Add(objNewMember);
                else
                   // mcolAllStops.Add(objNewMember, "k" + objNewMember.StopNumber);
                blnRet = true;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                return false;
            }
            return blnRet;
        }

       
        public int COUNT
        {
            get
            {
                return mcolAllStops.Count;
            }
        }

       
    }
    [Serializable()]
    public struct legInfoType
    {
        public string legMiles;
        public string totMiles;
        public string legCost;
        public string totCost;
        public string legHours;
        public string totHours;
    };
    [Serializable()]
    public class clsPCMBadAddresses 
    {
        private List<clsPCMBadAddress> mcolAddresses;
        // Public ReadOnly Property Addresses() As Collection Implements IclsPCMBadAddresses.Addresses
        // Get
        // Return mcolAddresses
        // End Get
        // End Property

        private string mstrLastError = "";
        public string LastError
        {
            get
            {
                return mstrLastError;
            }
        }


        public clsPCMBadAddresses() 
        {
            mcolAddresses = new List<clsPCMBadAddress>();
        }

       


        public clsPCMBadAddress Add(int BookControl, int LaneControl, clsAddress objOrig, clsAddress objDest, clsAddress objPCMOrig, clsAddress objPCMDest, string Message, double BatchID)
        {
            // create a new object
            clsPCMBadAddress objNewMember = new clsPCMBadAddress();
            try
            {
                // set the properties passed into the method

                {
                    var withBlock = objNewMember;
                    withBlock.BookControl = BookControl;
                    withBlock.LaneControl = LaneControl;
                    withBlock.objOrig = objOrig;
                    withBlock.objDest = objDest;
                    withBlock.objPCMOrig = objPCMOrig;
                    withBlock.objPCMDest = objPCMDest;
                    withBlock.Message = Message;
                    withBlock.BatchID = BatchID;
                }
                if (BookControl == 0)
                    mcolAddresses.Add(objNewMember);
                else
                   // mcolAddresses.Add(objNewMember, "k" + BookControl);
                return objNewMember;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                return null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                objNewMember = null/* TODO Change to default(_) if this is not a reference type */;
            }
            return null/* TODO Change to default(_) if this is not a reference type */;
        }

        public bool Add(clsPCMBadAddress objNewMember)
        {
            bool blnRet = false;
            try
            {
                if (objNewMember.BookControl == 0)
                    mcolAddresses.Add(objNewMember);
                else
                   // mcolAddresses.Add(objNewMember, "k" + objNewMember.BookControl);
                blnRet = true;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                return default(Boolean);
            }
            finally
            {
                objNewMember = null/* TODO Change to default(_) if this is not a reference type */;
            }
            return blnRet;
        }

       
        public int COUNT
        {
            get
            {
                return mcolAddresses.Count;
            }
        }
        public int Item(int Itemno)
        {
            return Itemno;
        } 
     
    }
    
    [Serializable()]
    public class clsPCMReturnEx 
    {
        private int[] mintBadAddressControls;
        public int BadAddressControls
        {
            get
            {
               
                return mintBadAddressControls[0];
            }
            set
            {
                
                mintBadAddressControls[0] = value;
            }
        }

        private string mstrFailedAddressMessage = "";
        public string FailedAddressMessage
        {
            get
            {
                return mstrFailedAddressMessage;
            }
            set
            {
                mstrFailedAddressMessage = value;
            }
        }

        private int mintBadAddressCount = 0;
        public int BadAddressCount
        {
            get
            {
                return mintBadAddressCount;
            }
            set
            {
                mintBadAddressCount = value;
            }
        }

        private double mdblTotalMiles = 0;
        public double TotalMiles
        {
            get
            {
                return mdblTotalMiles;
            }
            set
            {
                mdblTotalMiles = value;
            }
        }

        private string mstrOriginZip = "";
        public string OriginZip
        {
            get
            {
                return mstrOriginZip;
            }
            set
            {
                mstrOriginZip = value;
            }
        }

        private string mstrDestZip = "";
        public string DestZip
        {
            get
            {
                return mstrDestZip;
            }
            set
            {
                mstrDestZip = value;
            }
        }

        private double mdblAutoCorrectBadLaneZipCodes = 0;
        public double AutoCorrectBadLaneZipCodes
        {
            get
            {
                return mdblAutoCorrectBadLaneZipCodes;
            }
            set
            {
                mdblAutoCorrectBadLaneZipCodes = value;
            }
        }

        private double mdblBatchID = 0;
        public double BatchID
        {
            get
            {
                return mdblBatchID;
            }
            set
            {
                mdblBatchID = value;
            }
        }

        private string mstrLastError = "";
        public string LastError
        {
            get
            {
                return mstrLastError;
            }
        }

        private int _intRetVal = 0;
        public int RetVal
        {
            get
            {
                return _intRetVal;
            }
            set
            {
                _intRetVal = value;
            }
        }
        private string _strMessage = "";
        public string Message
        {
            get
            {
                return _strMessage;
            }
            set
            {
                _strMessage = value;
            }
        }

        private object _Results = null;
        public object Results
        {
            get
            {
                return _Results;
            }
            set
            {
                _Results = value;
            }
        }
    }


}
