using System;
using System.Collections.Generic;
using System.Linq;
using NGLTrimbleServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static NGLTrimbleServices.TMSPCMWrapper;


namespace TrimbleServicesUnitTest
{
    
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CityToLatLong()
        {
            PCMiles objPCM = new PCMiles();
            //CityToLatLong            
            string zipcode = "35582";
            //string LatlngVal = objPCM.CityToLatLong(ref zipcode, true, "");
            //End of CityToLatLong */   

            ////AddStop
            //clsAddress objOrig = new clsAddress();
            //objOrig.strAddress = "Sunshine Mills, Inc.,500 6th Street SW";
            //objOrig.strCity = "Red Bay";
            //objOrig.strState = "AL";
            //objOrig.strZip = "35582";
            //clsAddress objDest = new clsAddress();
            //objDest.strAddress = "DTMS Test Location,1611 Colonial Parkway";
            //objDest.strCity = "Inverness";
            //objDest.strState = "IL";
            //objDest.strZip = "60067";
            //clsAddress objPCMOrig = new clsAddress();
            //clsAddress objPCMDest = new clsAddress();
            //bool blnAddOrigin = true;
            //bool blnAddressValid = false;
            //clsGlobalStopData objGlobalStopData = new clsGlobalStopData();
            //string strOrigStopName = string.Empty;
            //string strDestStopName = string.Empty;
            //int intTrip1 = 1;
            //string strItemNumber = string.Empty;
            //string strItemType = string.Empty;
            //int intBookControl = 1;
            //int intLaneControl = 1;
            //clsPCMBadAddress[] arrBaddAddresses = null;
            //bool isproperStop = objPCM.AddStop(ref objOrig, ref objDest, ref objPCMOrig, ref objPCMDest, ref blnAddOrigin, ref objGlobalStopData, ref blnAddressValid, ref strOrigStopName, ref strDestStopName, intTrip1, strItemNumber, strItemType, intBookControl, intLaneControl, ref arrBaddAddresses);
            ////End of AddStop

          

           

            

        }

        [TestMethod]
        public void getGeoCode()
        {
            PCMiles objPCM = new PCMiles();
            //getGeoCode by ZIP
            string sAddress = "37726";
            double dblat = 0.0, dblng = 0.0;
            bool Isgeocode = objPCM.getGeoCode(sAddress, ref dblat, ref dblng);
            System.Diagnostics.Debug.WriteLine("Address {2}: Lat: {0} Long {1} ", dblat, dblng, sAddress);
            //street address
            sAddress = "127 Bos Way, Deer Lodge, TN  37726 US";
            Isgeocode = objPCM.getGeoCode(sAddress, ref dblat, ref dblng);
            System.Diagnostics.Debug.WriteLine("Address {2}: Lat: {0} Long {1} ", dblat, dblng, sAddress);
            string sResults = dblat.ToString() + "," + dblng.ToString();
            //End of getGeoCode  by Zip*/
        }

        [TestMethod]
        public void getGeoCodeByAddress()
        {
            PCMiles objPCM = new PCMiles();
            //getGeoCode by address
            double dblctnlat = 0.0, dblctnlng = 0.0;
            string location = "500 6th Street SW,Red Bay,AL,35582";
            bool IsgeocodebyAdrs = objPCM.getGeoCode(location, ref dblctnlat, ref dblctnlng, true, null, true);
            //End of getGeoCode  by address*/
        }

        [TestMethod]
        public void LatLongToCity()
        {
            PCMiles objPCM = new PCMiles();
            ////LatLongToCity
            string latlong = "-75.163244,40.958188";
            string scity = objPCM.LatLongToCity(latlong, true, null);
            ////End of LatLongToCity
        }
        [TestMethod]
        public void FullName()
        {
            PCMiles objPCM = new PCMiles();
            //FullName
            string CityNameOrZipCode = "1 independence way";
            string scity = objPCM.FullName(CityNameOrZipCode, true, "");
            //End of FullName
        }
        [TestMethod]
        public void cityStateZipLookup()
        {
            PCMiles objPCM = new PCMiles();
            //cityStateZipLookup
            string postalCode = "60067";
            clsAddress[] cityobj = objPCM.cityStateZipLookup(postalCode, true, "");
            //End of cityStateZipLookup

        }
        [TestMethod]
        public void PCMValidateAddress()
        {
            PCMiles objPCM = new PCMiles();
            //PCMValidateAddress
            string strAddress = "500 6th Street SW";
            bool isValidAddrs = objPCM.PCMValidateAddress(strAddress);
            //End of PCMValidateAddress
        }
        [TestMethod]
        public void PCMTestMiles()
        {
            PCMiles objPCM = new PCMiles();
            //PCMValidateAddress
            string strOrigin = "60067";
            string strDest = "37726";
            double dMiles = objPCM.Miles(strOrigin, strDest);
            string sREt = dMiles.ToString();
            //End of PCMValidateAddress
        }
        [TestMethod]
        public void getPracticalMiles()
        {
            PCMiles objPCM = new PCMiles();
            //getPracticalMiles
            clsAddress objOrig = new clsAddress();
            objOrig.strAddress = "500 6th Street SW";
            objOrig.strCity = "Red Bay";
            objOrig.strState = "AL";
            objOrig.strZip = "35582";
            clsAddress objDest = new clsAddress();
            objDest.strAddress = "1611 Colonial Parkway";
            objDest.strCity = "Inverness";
            objDest.strState = "IL";
            objDest.strZip = "60067";
            clsAddress objPCMOrig = new clsAddress();
            clsAddress objPCMDest = new clsAddress();
            bool blnAddOrigin = true;
            bool blnAddressValid = false;
            clsGlobalStopData objGlobalStopData = new clsGlobalStopData();
            string strOrigStopName = string.Empty;
            string strDestStopName = string.Empty;
            int intTrip1 = 1;
            PCMEX_Route_Type Route_Type = PCMEX_Route_Type.Practical;
            PCMEX_Dist_Type Dist_Type = PCMEX_Dist_Type.Miles;
            PCMEX_CALCTYPE EXCALC_Type = PCMEX_CALCTYPE.CALCTYPE_NONE;
            PCMEX_Opt_Flags EXOpt_Flags = PCMEX_Opt_Flags.None;
            PCMEX_Veh_Type EXVeh_Type = PCMEX_Veh_Type.Truck;
            int intCompControl = 2;
            int intBookControl1 = 0;
            int intLaneControl1 = 8;
            string strItemNumber1 = string.Empty;
            string strItemType1 = string.Empty;
            double dblAutoCorrectBadLaneZipCodes = 0.0;
            double dblBatchID = 0.0;
            bool blnBatch = true;
            clsPCMBadAddress[] arrBaddAddresses1 = new clsPCMBadAddress[2];
            clsAllStops isproperStop = objPCM.getPracticalMiles("C36349D0A5F5D440AAC0CB8A0287F02C",objOrig, objDest, Route_Type, Dist_Type, EXCALC_Type, EXOpt_Flags, EXVeh_Type, intCompControl, intBookControl1, intLaneControl1, strItemNumber1, strItemType1, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, ref arrBaddAddresses1);
            //End of getPracticalMiles
        }
        [TestMethod]
        public void PCMReSyncMultiStop()
        {
            PCMiles objPCM = new PCMiles();
            //PCMReSyncMultiStop
            clsFMStopData[] arrFMStops = new clsFMStopData[3];
            clsFMStopData objstop = new clsFMStopData();
            arrFMStops[0] = new clsFMStopData();
            arrFMStops[0].Street = "Red Warehouse,4400 Sharon Rd";
            arrFMStops[0].PCMilerStreet = "Red Warehouse,4400 Sharon Rd";
            arrFMStops[0].State = "NC";
            arrFMStops[0].PCMilerState = "NC";
            arrFMStops[0].Zip = "28211";
            arrFMStops[0].PCMilerZip = "28211";
            arrFMStops[0].PCMilerCity = "CHARLOTTE";
            arrFMStops[1] = new clsFMStopData();
            arrFMStops[1].Street = "101 Wood Avenue South #900";
            arrFMStops[1].PCMilerStreet = "101 Wood Avenue South #900";
            arrFMStops[1].State = "NJ";
            arrFMStops[1].PCMilerState = "NJ";
            arrFMStops[1].Zip = "08830";
            arrFMStops[1].PCMilerZip = "08830";
            arrFMStops[1].PCMilerCity = "ISELIN";
            arrFMStops[2] = new clsFMStopData();
            arrFMStops[2].Street = "4825 Creekstone Dr #190";
            arrFMStops[2].PCMilerStreet = "4825 Creekstone Dr #190";
            arrFMStops[2].State = "NC";
            arrFMStops[2].PCMilerState = "NC";
            arrFMStops[2].Zip = "27703";
            arrFMStops[2].PCMilerZip = "27703";
            arrFMStops[2].PCMilerCity = "DURHAM";
            double dblBatchID = 0.0;
            bool blnKeepStopNumbers = false;
            clsPCMReportRecord[] arrPCMReportRecords = new clsPCMReportRecord[3];
            clsGlobalStopData objResync = objPCM.PCMReSyncMultiStop(ref arrFMStops, dblBatchID, blnKeepStopNumbers, ref arrPCMReportRecords);
            //End of PCMReSyncMultiStop
        }
        [TestMethod]
        public void getRouteMiles()
        {
            PCMiles objPCM = new PCMiles();

            //getRouteMiles
            clsSimpleStop[] arrSimpStops = new clsSimpleStop[3];
            arrSimpStops[0] = new clsSimpleStop();
            arrSimpStops[0].Address = "Red Warehouse,4400 Sharon Rd,NC,28211,CHARLOTTE";
            arrSimpStops[1] = new clsSimpleStop();
            arrSimpStops[1].Address = "White Warehouse,101 Wood Avenue South #900,NJ,08830,ISELIN";
            arrSimpStops[2] = new clsSimpleStop();
            arrSimpStops[2].Address = "Yellow Warehouse,4825 Creekstone Dr #190,NC,27703,DURHAM";
            clsPCMReturn objGetRouteMiles = objPCM.getRouteMiles(ref arrSimpStops);
            //End of getRouteMiles
        }

        [TestMethod]
        public void PCMReSync()
        {
            PCMiles objPCM = new PCMiles();
            //PCMReSync
            clsPCMDataStop[] arrStopData = new clsPCMDataStop[3];
            arrStopData[0] = new clsPCMDataStop();
            arrStopData[0].BookOrigAddress1 = "500 6th Street SW";
            arrStopData[0].BookOrigCity = "Red Bay";
            arrStopData[0].BookOrigState = "AL";
            arrStopData[0].BookOrigZip = "35582";
            arrStopData[0].BookDestAddress1 = "123 Money Way";
            arrStopData[0].BookDestCity = "Chicago";
            arrStopData[0].BookDestState = "IL";
            arrStopData[0].BookDestZip = "61236";
            arrStopData[0].RouteType = (int)PCMEX_Route_Type.Practical;
            arrStopData[0].BookLoadControl = 1;
            arrStopData[0].BookOrigID = "1-O";
            arrStopData[0].BookDestID = "1-D";
            arrStopData[0].BookOrigRegion = "4";
            arrStopData[0].BookDestRegion = "4";
            arrStopData[0].BookOrigName = "Red Bay Warehouse";
            arrStopData[0].BookDestName = "Chicago Storage";
            arrStopData[0].BookOrigStartTime = "08:00";
            arrStopData[0].BookOrigEndTime = "11:59";
            arrStopData[0].BookDestStartTime = "11:59";
            arrStopData[0].BookDestEndTime = "18:00";
            arrStopData[1] = new clsPCMDataStop();
            arrStopData[1].BookOrigAddress1 = "4400 Sharon Rd";
            arrStopData[1].BookOrigCity = "CHARLOTTE";
            arrStopData[1].BookOrigState = "NC";
            arrStopData[1].BookOrigZip = "28211";
            arrStopData[1].BookDestAddress1 = "Yellow Warehouse,4825 Creekstone Dr #190";
            arrStopData[1].BookDestCity = "DURHAM";
            arrStopData[1].BookDestState = "NC";
            arrStopData[1].BookDestZip = "27703";
            arrStopData[1].RouteType = (int)PCMEX_Route_Type.Practical;
            arrStopData[1].BookLoadControl = 2;
            arrStopData[1].BookOrigID = "2-O";
            arrStopData[1].BookDestID = "2-D";
            arrStopData[1].BookOrigRegion = "4";
            arrStopData[1].BookDestRegion = "4";
            arrStopData[1].BookOrigName = "CHARLOTTE Warehouse";
            arrStopData[1].BookDestName = "Yellow Warehouse";
            arrStopData[1].BookOrigStartTime = "08:00";
            arrStopData[1].BookOrigEndTime = "11:59";
            arrStopData[1].BookDestStartTime = "11:59";
            arrStopData[1].BookDestEndTime = "18:00";
            arrStopData[2] = new clsPCMDataStop();
            arrStopData[2].BookOrigAddress1 = "White Warehouse,101 Wood Avenue South #900";
            arrStopData[2].BookOrigCity = "ISELIN";
            arrStopData[2].BookOrigState = "NJ";
            arrStopData[2].BookOrigZip = "08830";
            arrStopData[2].BookDestAddress1 = "The Cannon Group PLC,192 Market Square";
            arrStopData[2].BookDestCity = "Atlanta";
            arrStopData[2].BookDestState = "GA";
            arrStopData[2].BookDestZip = "31772";
            arrStopData[2].RouteType = (int)PCMEX_Route_Type.Practical;
            arrStopData[2].BookLoadControl = 3;
            arrStopData[2].BookOrigID = "3-O";
            arrStopData[2].BookDestID = "3-D";
            arrStopData[2].BookOrigRegion = "4";
            arrStopData[2].BookDestRegion = "4";
            arrStopData[2].BookOrigName = "White Warehouse";
            arrStopData[2].BookDestName = "Atlanta Warehouse";
            arrStopData[2].BookOrigStartTime = "08:00";
            arrStopData[2].BookOrigEndTime = "11:59";
            arrStopData[2].BookDestStartTime = "11:59";
            arrStopData[2].BookDestEndTime = "18:00";
            string strConsNumber = "";
            double dblBatchID = 0.0;
            bool blnKeepStopNumbers = false;
            clsAllStop[] arrAllStops = new clsAllStop[3];
            clsPCMBadAddress[] arrBaddAddresses = new clsPCMBadAddress[3];
            TrimbleAPI TRM = new TrimbleAPI();
            //string sData = TRM.TimeWindowOptimizer();
            //Console.WriteLine(sData);
            //return;
            clsGlobalStopData objResync = objPCM.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, ref arrAllStops, ref arrBaddAddresses);
            //End of PCMReSync
        }


        [TestMethod]
        public void TimeWindowOptimizer()
        {
            PCMiles objPCM = new PCMiles();
            //PCMReSync
            clsPCMDataStop[] arrStopData = new clsPCMDataStop[3];
            NGLTrimbleServices.TrimbleServiceReference.StopLocation[] Stops = new NGLTrimbleServices.TrimbleServiceReference.StopLocation[2] ;
            
            //List<NGLTrimbleServices.TrimbleServiceReference.ReportRoute> parts = new List<NGLTrimbleServices.TrimbleServiceReference.ReportRoute>();
            arrStopData[0] = new clsPCMDataStop();

            //[{\"RouteId\": \"test\", \"Stops\": [{\"Address\": {\"Zip\": \"08540\"},\"Label\": \"Trimble MAPS\"}, {\"Address\": {\"Zip\": \"90266\"},\"Label\": \"Manhattan Beach\"}],
            Stops[0] = new NGLTrimbleServices.TrimbleServiceReference.StopLocation();
            Stops[0].Address = new NGLTrimbleServices.TrimbleServiceReference.Address();
            Stops[0].Address.Zip = "08540";
            Stops[0].Label = "Trimble MAPS";
            Stops[1] = new NGLTrimbleServices.TrimbleServiceReference.StopLocation();
            Stops[1].Address = new NGLTrimbleServices.TrimbleServiceReference.Address();
            Stops[1].Address.Zip = "90266";
            Stops[1].Label = "Manhattan Beach";
            TrimbleAPI TRM = new TrimbleAPI();
            NGLTrimbleServices.TrimbleServiceReference.MileageReport oData = TRM.TimeWindowOptimizer(Stops);
            Console.WriteLine(oData.RouteID);
            return;

            arrStopData[0].BookOrigAddress1 = "500 6th Street SW";
            arrStopData[0].BookOrigCity = "Red Bay";
            arrStopData[0].BookOrigState = "AL";
            arrStopData[0].BookOrigZip = "35582";
            arrStopData[0].BookDestAddress1 = "123 Money Way";
            arrStopData[0].BookDestCity = "Chicago";
            arrStopData[0].BookDestState = "IL";
            arrStopData[0].BookDestZip = "61236";
            arrStopData[0].RouteType = (int)PCMEX_Route_Type.Practical;
            arrStopData[0].BookLoadControl = 1;
            arrStopData[0].BookOrigID = "1-O";
            arrStopData[0].BookDestID = "1-D";
            arrStopData[0].BookOrigRegion = "4";
            arrStopData[0].BookDestRegion = "4";
            arrStopData[0].BookOrigName = "Red Bay Warehouse";
            arrStopData[0].BookDestName = "Chicago Storage";
            arrStopData[0].BookOrigStartTime = "08:00";
            arrStopData[0].BookOrigEndTime = "11:59";
            arrStopData[0].BookDestStartTime = "11:59";
            arrStopData[0].BookDestEndTime = "18:00";
            arrStopData[1] = new clsPCMDataStop();
            arrStopData[1].BookOrigAddress1 = "4400 Sharon Rd";
            arrStopData[1].BookOrigCity = "CHARLOTTE";
            arrStopData[1].BookOrigState = "NC";
            arrStopData[1].BookOrigZip = "28211";
            arrStopData[1].BookDestAddress1 = "Yellow Warehouse,4825 Creekstone Dr #190";
            arrStopData[1].BookDestCity = "DURHAM";
            arrStopData[1].BookDestState = "NC";
            arrStopData[1].BookDestZip = "27703";
            arrStopData[1].RouteType = (int)PCMEX_Route_Type.Practical;
            arrStopData[1].BookLoadControl = 2;
            arrStopData[1].BookOrigID = "2-O";
            arrStopData[1].BookDestID = "2-D";
            arrStopData[1].BookOrigRegion = "4";
            arrStopData[1].BookDestRegion = "4";
            arrStopData[1].BookOrigName = "CHARLOTTE Warehouse";
            arrStopData[1].BookDestName = "Yellow Warehouse";
            arrStopData[1].BookOrigStartTime = "08:00";
            arrStopData[1].BookOrigEndTime = "11:59";
            arrStopData[1].BookDestStartTime = "11:59";
            arrStopData[1].BookDestEndTime = "18:00";
            arrStopData[2] = new clsPCMDataStop();
            arrStopData[2].BookOrigAddress1 = "White Warehouse,101 Wood Avenue South #900";
            arrStopData[2].BookOrigCity = "ISELIN";
            arrStopData[2].BookOrigState = "NJ";
            arrStopData[2].BookOrigZip = "08830";
            arrStopData[2].BookDestAddress1 = "The Cannon Group PLC,192 Market Square";
            arrStopData[2].BookDestCity = "Atlanta";
            arrStopData[2].BookDestState = "GA";
            arrStopData[2].BookDestZip = "31772";
            arrStopData[2].RouteType = (int)PCMEX_Route_Type.Practical;
            arrStopData[2].BookLoadControl = 3;
            arrStopData[2].BookOrigID = "3-O";
            arrStopData[2].BookDestID = "3-D";
            arrStopData[2].BookOrigRegion = "4";
            arrStopData[2].BookDestRegion = "4";
            arrStopData[2].BookOrigName = "White Warehouse";
            arrStopData[2].BookDestName = "Atlanta Warehouse";
            arrStopData[2].BookOrigStartTime = "08:00";
            arrStopData[2].BookOrigEndTime = "11:59";
            arrStopData[2].BookDestStartTime = "11:59";
            arrStopData[2].BookDestEndTime = "18:00";
            string strConsNumber = "";
            double dblBatchID = 0.0;
            bool blnKeepStopNumbers = false;
            clsAllStop[] arrAllStops = new clsAllStop[3];
            clsPCMBadAddress[] arrBaddAddresses = new clsPCMBadAddress[3];
            
            clsGlobalStopData objResync = objPCM.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, ref arrAllStops, ref arrBaddAddresses);
            //End of PCMReSync
        }


        [TestMethod]
        public void MileageReport()
        {
            PCMiles objPCM = new PCMiles();
            //PCMReSync
            clsPCMDataStop[] arrStopData = new clsPCMDataStop[6];
            TrimbleAPI TRM = new TrimbleAPI();
            NGLTrimbleServices.TrimbleServiceReference.StopLocation[] Stops = new NGLTrimbleServices.TrimbleServiceReference.StopLocation[6];
            List<NGLTrimbleServices.TrimbleServiceReference.StopLocation> lBadAddresses = new List<NGLTrimbleServices.TrimbleServiceReference.StopLocation>();
            List<NGLTrimbleServices.TrimbleServiceReference.StopLocation> lMatchingAddresses = new List<NGLTrimbleServices.TrimbleServiceReference.StopLocation>();
            //List<NGLTrimbleServices.TrimbleServiceReference.ReportRoute> parts = new List<NGLTrimbleServices.TrimbleServiceReference.ReportRoute>();
            arrStopData[0] = new clsPCMDataStop();
            bool bInvalidRoute = false;

            //[{\"RouteId\": \"test\", \"Stops\": [{\"Address\": {\"Zip\": \"08540\"},\"Label\": \"Trimble MAPS\"}, {\"Address\": {\"Zip\": \"90266\"},\"Label\": \"Manhattan Beach\"}],
            //Stops[0] = new NGLTrimbleServices.TrimbleServiceReference.StopLocation();
            //Stops[0].Address = new NGLTrimbleServices.TrimbleServiceReference.Address();
            //Stops[0].Address.Zip = "08540";
            //Stops[0].Label = "Trimble MAPS";
            //Stops[1] = new NGLTrimbleServices.TrimbleServiceReference.StopLocation();
            //Stops[1].Address = new NGLTrimbleServices.TrimbleServiceReference.Address();
            //Stops[1].Address.Zip = "90266";
            //Stops[1].Label = "Manhattan Beach";

            //Stops[0].Address = new NGLTrimbleServices.TrimbleServiceReference.Address();
            //Stops[0].Address.Zip = "08540";
            //Stops[0].Label = "Trimble MAPS";

            //Stops[1].Address = new NGLTrimbleServices.TrimbleServiceReference.Address();
            //Stops[1].Address.Zip = "90266";
            //Stops[1].Label = "Manhattan Beach";
            // arrStopData[0].RouteType = (int)PCMEX_Route_Type.Practical;           
            Stops[0] = TRM.PCMCreateStop("814 4th St NW", "Red Bay", "AL", "35582", "US", "1-O", "", ref lBadAddresses,ref lMatchingAddresses);
            if (Stops[0] == null) { bInvalidRoute = true; }
            //Stops[0] = TRM.PCMCreateStop("500 6th Street SW", "Red Bay", "AL", "35582", "US", "1-O", "Red Bay Warehouse");
            //strZip & " " & objSource.strCity & ", " & objSource.strState & ";  " & objSource.strAddress            
            // not needed PCMCreateStop validates address var oData1 = TRM.PCMSLookup("61244  Moline, IL; 274 21st St ");
            Stops[1] = TRM.PCMCreateStop("274 21st St", "Moline", "IL", "61244", "US", "1-D", "",  ref lBadAddresses, ref lMatchingAddresses);
            if (Stops[1] == null) { bInvalidRoute = true; }
            //274 21st St, East Moline, IL 61244
            //Stops[1] = TRM.PCMCreateStop("123 Money Way", "Chicago", "IL", "61236", "US","1-D", "Chicago Storage");
            //arrStopData[0].BookOrigStartTime = "08:00";
            //arrStopData[0].BookOrigEndTime = "11:59";
            //arrStopData[0].BookDestStartTime = "11:59";
            //arrStopData[0].BookDestEndTime = "18:00";
            Stops[2] = TRM.PCMCreateStop("4400 Sharon Rd", "CHARLOTTE", "NC", "28211", "US", "2-O", "", ref lBadAddresses, ref lMatchingAddresses);
            if (Stops[2] == null) { bInvalidRoute = true; }
            //Stops[2] = TRM.PCMCreateStop("4400 Sharon Rd","CHARLOTTE","NC","28211","US", "2-O", "CHARLOTTE Warehouse");
            Stops[3] = TRM.PCMCreateStop("4825 Creekstone Dr #190", "DURHAM", "NC", "27703", "US", "2-D", "", ref lBadAddresses, ref lMatchingAddresses);
            if (Stops[3] == null) { bInvalidRoute = true; }
            //Stops[3] = TRM.PCMCreateStop("Yellow Warehouse,4825 Creekstone Dr #190","DURHAM","NC","27703","US", "2-D", "Yellow Warehouse");
            //arrStopData[1].BookOrigStartTime = "08:00";
            //arrStopData[1].BookOrigEndTime = "11:59";
            //arrStopData[1].BookDestStartTime = "11:59";
            //arrStopData[1].BookDestEndTime = "18:00";
            Stops[4] = TRM.PCMCreateStop("101 Wood Avenue South #900", "ISELIN", "NJ", "08830", "US", "3-O", "", ref lBadAddresses, ref lMatchingAddresses);
            if (Stops[4] == null) { bInvalidRoute = true; }
            //Stops[4] = TRM.PCMCreateStop("White Warehouse,101 Wood Avenue South #900","ISELIN","NJ", "08830","US", "3-O", "White Warehouse");
            Stops[5] = TRM.PCMCreateStop("192 Market Square", "Atlanta", "GA", "31772", "US", "3-D", "", ref lBadAddresses, ref lMatchingAddresses);
            if (Stops[5] == null) { bInvalidRoute = true; }
            //Stops[5] = TRM.PCMCreateStop("The Cannon Group PLC,192 Market Square", "Atlanta", "GA", "31772", "US", "3-D", "Atlanta Warehouse");
            //arrStopData[2].BookOrigStartTime = "08:00";
            //arrStopData[2].BookOrigEndTime = "11:59";
            //arrStopData[2].BookDestStartTime = "11:59";
            //arrStopData[2].BookDestEndTime = "18:00";

            /*
             *Location:  ID: 1-O Miles: 0.000
Location:  ID: 1-D Miles: 632.959
Location:  ID: 2-O Miles: 1525.177
Location:  ID: 2-D Miles: 1683.408
Location:  ID: 3-O Miles: 2164.245
Location:  ID: 3-D Miles: 3032.710 
             * 
             * 
             * Location:  ID: 1-O Leg Miles: 0.000 Total Miles: 0.000
Location:  ID: 1-D Leg Miles: 633.406 Total Miles: 633.406
Location:  ID: 2-O Leg Miles: 892.219 Total Miles: 1525.625
Location:  ID: 2-D Leg Miles: 158.230 Total Miles: 1683.855
Location:  ID: 3-O Leg Miles: 481.583 Total Miles: 2165.439
Location:  ID: 3-D Leg Miles: 868.465 Total Miles: 3033.904
             * 
             * Location:  ID: 1-O Leg Miles: 0.000 Total Miles: 0.000
Location:  ID: 3-D Leg Miles: 282.091 Total Miles: 282.091
Location:  ID: 2-O Leg Miles: 251.397 Total Miles: 533.489
Location:  ID: 2-D Leg Miles: 158.230 Total Miles: 691.719
Location:  ID: 3-O Leg Miles: 481.583 Total Miles: 1173.303
Location:  ID: 1-D Leg Miles: 933.695 Total Miles: 2106.998


            Location:  ID: 1-O Leg Miles: 0.000 Total Miles: 0.000
Location:  ID: 3-D Leg Miles: 282.091 Total Miles: 282.091
Location:  ID: 2-O Leg Miles: 251.397 Total Miles: 533.489
Location:  ID: 2-D Leg Miles: 158.230 Total Miles: 691.719
Location:  ID: 3-O Leg Miles: 481.583 Total Miles: 1173.303
Location:  ID: 1-D Leg Miles: 933.695 Total Miles: 2106.998

             * */

            bool blnKeepStopNumbers = false;
            if (!bInvalidRoute)
            {
                NGLTrimbleServices.clsTrimbleReportParams oPar = new NGLTrimbleServices.clsTrimbleReportParams();
                oPar.HubRouting = false;
                oPar.UseTollData = true;
                oPar.FuelUnits = 0;
                oPar.RouteOptimization = 0;
                oPar.RouteType = 0;
                oPar.TruckStyle = NGLTrimbleServices.TrimbleServiceReference.TruckStyle.FiftyThreeSemiTrailer;
                oPar.APIKey = "C36349D0A5F5D440AAC0CB8A0287F02C";
                NGLTrimbleServices.TrimbleServiceReference.MileageReport oData = TRM.PCMMileageReport(Stops, oPar);
                foreach (NGLTrimbleServices.TrimbleServiceReference.StopReportLine line in oData.ReportLines)
                {
                    System.Diagnostics.Debug.WriteLine("Location: {0} ID: {2} Leg Miles: {3} Total Miles: {1}",line.Stop.PlaceName,line.TMiles, line.Stop.Label, line.LMiles);
                    //foreach (Stop in line.Stop)
                }
                Console.WriteLine(oData.RouteID);
            } else
            {
                System.Diagnostics.Debug.WriteLine("Found " + lBadAddresses.Count().ToString() + " bad addresses");
            }
            return;
        }
    }
}
