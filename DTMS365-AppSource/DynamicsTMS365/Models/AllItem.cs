using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class AllGridItem
    {
        public int Control { get; set; }
        public string ProNumber { get; set; }
        public string CnsNumber { get; set; }
        public int? StopNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? ScheduledToLoad { get; set; }
        public DateTime? RequestedToArrive { get; set; }
        public string AssignedCarrier { get; set; }
        public string DestinationName { get; set; }
        public string DestinationAddress1 { get; set; }
        public string DestinationAddress2 { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string DestinationZip { get; set; }
        public string DestinationCountry { get; set; }
        public string OrigName { get; set; }
        public string OrigAddress1 { get; set; }
        public string OrigAddress2 { get; set; }
        public string OrigCity { get; set; }
        public string OrigState { get; set; }
        public string OrigZip { get; set; }
        public string OrigCountry { get; set; }
        public string Comments { get; set; }
        public int Status { get; set; }
        public string BookNotes1 { get; set; }
        public string BookNotes2 { get; set; }
        public string BookNotes3 { get; set; }
        public string AssignedProNumber { get; set; }
        public string BookShipCarrierProNumberRaw { get; set; }
        public int? BookShipCarrierProControl { get; set; }
        public string AssignedCarrierNumber { get; set; }
        public string AssignedCarrierName { get; set; }
        public string AssignedCarrierContact { get; set; }
        public string AssignedCarrierContactPhone { get; set; }
        public int BookPickupStopNumber { get; set; }
        public Boolean ApplyToAllDestinations { get; set; }
        public Boolean ApplyToAllPickups { get; set; }
        public Boolean ApplyCommentsToCNS { get; set; }
        public int BookAMSPickupApptControl { get; set; }
        public int BookAMSDeliveryApptControl { get; set; }
        public int BookLoadControl { get; set; }
        public string SHID { get; set; }
        public string CarrierPro { get; set; }
        public DateTime? BookModDate { get; set; }
        private string _BookModUser;
        public string BookModUser
        {
            get { return _BookModUser.Left(100); } //uses extension string method Left
            set { _BookModUser = value.Left(100); }
        }

        //public int BookControl { get; set; }
        //public string BookCarrFBNumber { get; set; }
        //public string BookCarrOrderNumber { get; set; }
        //public string BookCarrBLNumber { get; set; }
        //public DateTime? BookCarrBookDate { get; set; }
        //public DateTime? BookCarrBookTime { get; set; }
        //public string BookCarrBookContact { get; set; }
        public DateTime? BookCarrScheduleDate { get; set; }
        //public DateTime? BookCarrScheduleTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrScheduleTime { get; set; }
        public DateTime? BookCarrActualDate { get; set; }
        //public DateTime? BookCarrActualTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrActualTime { get; set; }
        public DateTime? BookCarrActLoadComplete_Date { get; set; }
        //public DateTime? BookCarrActLoadCompleteTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrActLoadCompleteTime { get; set; }
        public string BookCarrDockPUAssigment { get; set; }
        //public DateTime? BookCarrPODate { get; set; }
        //public DateTime? BookCarrPOTime { get; set; }
        public DateTime? BookCarrApptDate { get; set; }
        //public DateTime? BookCarrApptTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrApptTime { get; set; }
        public DateTime? BookCarrActDate { get; set; }
        //public DateTime? BookCarrActTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrActTime { get; set; }
        public DateTime? BookCarrActUnloadCompDate { get; set; }
        //public DateTime? BookCarrActUnloadCompTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrActUnloadCompTime { get; set; }
        public string BookCarrDockDelAssignment { get; set; }
        //public int BookCarrVarDay { get; set; }
        //public int BookCarrVarHrs { get; set; }
        public string BookCarrTrailerNo { get; set; }
        public string BookCarrSealNo { get; set; }
        public string BookCarrDriverNo { get; set; }
        public string BookCarrDriverName { get; set; }
        ////public string BookCarrRouteNo { get; set; }
        ////public string BookCarrTripNo { get; set; }
        public string BookWhseAuthorizationNo { get; set; }
        public DateTime? BookCarrStartLoadingDate { get; set; }
        //public DateTime? BookCarrStartLoadingTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrStartLoadingTime { get; set; }
        public DateTime? BookCarrFinishLoadingDate { get; set; }
        //public DateTime? BookCarrFinishLoadingTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrFinishLoadingTime { get; set; }
        public DateTime? BookCarrStartUnloadingDate { get; set; }
        //public DateTime? BookCarrStartUnloadingTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrStartUnloadingTime { get; set; }
        public DateTime? BookCarrFinishUnloadingDate { get; set; }
        //public DateTime? BookCarrFinishUnloadingTime { get; set; }
        // Modefied By MA for v-8.5.4.006 on 04-26-2024
        public string BookCarrFinishUnloadingTime { get; set; }
        public int? BookFinAPActWgt { get; set; }
        public string BookCarrBLNumber { get; set; }
        public int? Pages { get; set; }
        public int? RecordCount { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public long? ROW_NUMBER { get; set; }

        //Added By LVV on 7/1/19 for v-8.2
        public string TranCode { get; set; }


        //Added By LVV on 9/19/19 for Bing Maps - CLT aka "Comment Location Tag" (optional location to be tagged as detail with a comment)
        public int CLTStopControl { get; set; }
        public string CLTStopName { get; set; }
        public string CLTStopAddress1 { get; set; }
        public string CLTStopCity { get; set; }
        public string CLTStopState { get; set; }
        public string CLTStopCountry { get; set; }
        public string CLTStopZip { get; set; }
        public double CLTStopLatitude { get; set; }
        public double CLTStopLongitude { get; set; }
        public DateTime? CLTDate { get; set; }
        public DateTime? CLTTime { get; set; }
        public string BookItemOrderNumbers { get; set; } //Added By LVV on 11/7/19 Ticket #201909200751


        public void fillFromRequest(HttpRequest request)
        {
            int intVal = 0;
            short shVal = 0;

            int.TryParse(request["BookPickupStopNumber"] ?? "0", out intVal);
            this.BookPickupStopNumber = intVal;
            intVal = 0;
            int.TryParse(request["BookFinAPActWgt"] ?? "0", out intVal);
            this.BookFinAPActWgt = intVal;
            intVal = 0;

            short.TryParse(request["StopNumber"] ?? "0", out shVal);
            this.StopNumber = shVal;
            shVal = 0;

            this.ScheduledToLoad = string.IsNullOrEmpty(request["ScheduledToLoad"]) ? DateTime.Now : Convert.ToDateTime(request["LOADDATE"]);
            this.RequestedToArrive = string.IsNullOrEmpty(request["RequestedToArrive"]) ? DateTime.Now : Convert.ToDateTime(request["SCHEDULEDATE"]);           
            
            this.BookCarrScheduleDate = string.IsNullOrEmpty(request["BookCarrScheduleDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADToLoadDate"]);
            //this.BookCarrScheduleTime = string.IsNullOrEmpty(request["BookCarrScheduleTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADScheduleTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrScheduleTime = request["BookCarrScheduleTime"]; ;
            this.BookCarrActualDate = string.IsNullOrEmpty(request["BookCarrActualDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADActPickupDate"]);
            //this.BookCarrActualTime = string.IsNullOrEmpty(request["BookCarrActualTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADActPickupTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrActualTime = request["BookCarrActualTime"];
            this.BookCarrStartLoadingDate = string.IsNullOrEmpty(request["BookCarrStartLoadingDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADStartLoadingDate"]);
            //this.BookCarrStartLoadingTime = string.IsNullOrEmpty(request["BookCarrStartLoadingTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADStartLoadingTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrStartLoadingTime = request["BookCarrStartLoadingTime"];
            this.BookCarrFinishLoadingDate = string.IsNullOrEmpty(request["BookCarrFinishLoadingDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADFinishLoadingDate"]);
            //this.BookCarrFinishLoadingTime = string.IsNullOrEmpty(request["BookCarrFinishLoadingTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADFinishLoadingTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrFinishLoadingTime = request["BookCarrFinishLoadingTime"];
            this.BookCarrActLoadComplete_Date = string.IsNullOrEmpty(request["BookCarrActLoadComplete_Date"]) ? DateTime.Now : Convert.ToDateTime(request["LOADActLoadComplete_Date"]);
            //this.BookCarrActLoadCompleteTime = string.IsNullOrEmpty(request["BookCarrActLoadCompleteTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADActLoadCompleteTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrActLoadCompleteTime = request["BookCarrActLoadCompleteTime"];
            this.BookCarrApptDate = string.IsNullOrEmpty(request["BookCarrApptDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADDelScheduleDate"]);
            //this.BookCarrApptTime = string.IsNullOrEmpty(request["BookCarrApptTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADDelScheduleTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrApptTime = request["BookCarrApptTime"];
            this.BookCarrActDate = string.IsNullOrEmpty(request["BookCarrActDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADActDelDate"]);
            //this.BookCarrActTime = string.IsNullOrEmpty(request["BookCarrActTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADActDelTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrActTime = request["BookCarrActTime"];
            this.BookCarrStartUnloadingDate = string.IsNullOrEmpty(request["BookCarrStartUnloadingDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADDelStartUnloadingDate"]);
            //this.BookCarrStartUnloadingTime = string.IsNullOrEmpty(request["BookCarrStartUnloadingTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADDelStartUnloadingTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrStartUnloadingTime = request["BookCarrStartUnloadingTime"] ;
            this.BookCarrFinishUnloadingDate = string.IsNullOrEmpty(request["BookCarrFinishUnloadingDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADDelFinishUnloadingDate"]);
            //this.BookCarrFinishUnloadingTime = string.IsNullOrEmpty(request["BookCarrFinishUnloadingTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADDelFinishUnloadingTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrFinishUnloadingTime = request["BookCarrFinishUnloadingTime"];
            this.BookCarrActUnloadCompDate = string.IsNullOrEmpty(request["BookCarrActUnloadCompDate"]) ? DateTime.Now : Convert.ToDateTime(request["LOADActLoadCompDate"]);
            //this.BookCarrActUnloadCompTime = string.IsNullOrEmpty(request["BookCarrActUnloadCompTime"]) ? DateTime.Now : Convert.ToDateTime(request["LOADActLoadCompTime"]);
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            this.BookCarrActUnloadCompTime = request["BookCarrActUnloadCompTime"];
            this.BookModDate = string.IsNullOrEmpty(request["BookModDate"]) ? DateTime.Now : Convert.ToDateTime(request["BookModDate"]);

            this.BookCarrDockPUAssigment = request["BookCarrDockPUAssigment"] ?? "";
            this.BookCarrTrailerNo = request["BookCarrTrailerNo"] ?? "";
            this.BookCarrSealNo = request["BookCarrSealNo"] ?? "";
            this.BookCarrDriverNo = request["BookCarrDriverNo"] ?? "";
            this.BookCarrDriverName = request["BookCarrDriverName"] ?? "";
            //this.BookWhseAuthorizationNo = request["BookWhseAuthorizationNo"] ?? "";
            this.BookWhseAuthorizationNo = request["LOADWhseAuthorizationNo"] ?? "";
            this.BookCarrDockDelAssignment = request["BookCarrDockDelAssignment"] ?? "";
            this.AssignedProNumber = request["BookShipCarrierProNumber"] ?? "";
            this.AssignedCarrierNumber = request["AssignedCarrierNumber"] ?? "";
            this.AssignedCarrierName = request["AssignedCarrierName "] ?? "";
            this.BookCarrBLNumber = request["BookCarrBLNumber "] ?? "";

        }
    }
}