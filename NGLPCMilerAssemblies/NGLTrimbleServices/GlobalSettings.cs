using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace NGLTrimbleServices
{
  
    static class GlobalSettings
    {
        public static short gServerID = 0;
        public static string gLastError = "";
        public static bool gProcessRunning = false;

               
        public static void AddBaddAddressToArray(int BookControl, int LaneControl, clsAddress objOrig, clsAddress objDest, clsAddress objPCMOrig, clsAddress objPCMDest, string Message, double BatchID, ref clsPCMBadAddress[] arrBaddAddresses)
        {
            clsPCMBadAddress oBadAddress = new clsPCMBadAddress();
            {
                var withBlock = oBadAddress;
                withBlock.BookControl = BookControl;
                withBlock.LaneControl = LaneControl;
                withBlock.objOrig = objOrig;
                withBlock.objDest = objDest;
                withBlock.objPCMOrig = objPCMOrig;
                withBlock.objPCMDest = objPCMDest;
                withBlock.Message = Message;
                withBlock.BatchID = BatchID;
            }
            if (arrBaddAddresses == null)
            {
                var oldArrBaddAddresses = arrBaddAddresses;
                arrBaddAddresses = new clsPCMBadAddress[1];
                if (oldArrBaddAddresses != null)
                    Array.Copy(oldArrBaddAddresses, arrBaddAddresses, Math.Min(1, oldArrBaddAddresses.Length));
            }
            else
            {
                var oldArrBaddAddresses = arrBaddAddresses;
                arrBaddAddresses = new clsPCMBadAddress[arrBaddAddresses.Length + 1];
                if (oldArrBaddAddresses != null)
                    Array.Copy(oldArrBaddAddresses, arrBaddAddresses, Math.Min(arrBaddAddresses.Length + 1, oldArrBaddAddresses.Length));
            }
            arrBaddAddresses[arrBaddAddresses.Length - 1] = oBadAddress;
        }

        public static void AddStopToArray(short shtStopNumber, string strStopName, string strID1, string strID2, string strTruckName, int intTruckNumber, short shtSeqNbr, double dblDistToPrev, double dblTotalRouteCost, string strConsNumber, ref clsAllStop[] arrAllStops)
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

        public static void AddPCMDataStopToArray(int BookControl, int BookCustCompControl, int BookLoadControl, int BookODControl, int BookStopNo, int RouteType, int DistType, string BookOrigZip, string BookDestZip, string BookOrigAddress1, string BookDestAddress1, string BookOrigCity, string BookDestCity, string BookOrigState, string BookDestState, string BookProNumber, bool LaneOriginAddressUse, ref clsPCMDataStop[] arrPCMDataStops)
        {
            // create a new object
            clsPCMDataStop oPCMDataStop = new clsPCMDataStop();
            {
                var withBlock = oPCMDataStop;
                withBlock.BookControl = BookControl;
                withBlock.BookCustCompControl = BookCustCompControl;
                withBlock.BookLoadControl = BookLoadControl;
                withBlock.BookODControl = BookODControl;
                withBlock.BookStopNo = BookStopNo;
                withBlock.RouteType = RouteType;
                withBlock.DistType = DistType;
                withBlock.BookOrigZip = BookOrigZip;
                withBlock.BookDestZip = BookDestZip;
                withBlock.BookOrigAddress1 = BookOrigAddress1;
                withBlock.BookDestAddress1 = BookDestAddress1;
                withBlock.BookOrigCity = BookOrigCity;
                withBlock.BookDestCity = BookDestCity;
                withBlock.BookOrigState = BookOrigState;
                withBlock.BookDestState = BookDestState;
                withBlock.BookProNumber = BookProNumber;
                withBlock.LaneOriginAddressUse = LaneOriginAddressUse;
            }
            if (arrPCMDataStops == null)
            {
                var oldArrPCMDataStops = arrPCMDataStops;
                arrPCMDataStops = new clsPCMDataStop[1];
                if (oldArrPCMDataStops != null)
                    Array.Copy(oldArrPCMDataStops, arrPCMDataStops, Math.Min(1, oldArrPCMDataStops.Length));
            }
            else
            {
                var oldArrPCMDataStops = arrPCMDataStops;
                arrPCMDataStops = new clsPCMDataStop[arrPCMDataStops.Length + 1];
                if (oldArrPCMDataStops != null)
                    Array.Copy(oldArrPCMDataStops, arrPCMDataStops, Math.Min(arrPCMDataStops.Length + 1, oldArrPCMDataStops.Length));
            }
            arrPCMDataStops[arrPCMDataStops.Length - 1] = oPCMDataStop;
        }




}
}
