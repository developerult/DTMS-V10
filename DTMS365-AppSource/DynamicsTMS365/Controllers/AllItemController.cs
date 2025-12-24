using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class AllItemController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public AllItemController()
                : base(Utilities.PageEnum.All)
	    {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.AllItemController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.AllGridItem selectModelData(LTS.spNGLLOAD_SORTEDWPages_New7052Result d)
        {
            Models.AllGridItem all = new Models.AllGridItem();

            all.Control = d.LOADBookControl;
            all.ProNumber = d.LOADProNumber;
            all.CnsNumber = d.LOADCONSPREFIX;
            all.StopNumber = d.LOADStopNo;
            all.BookPickupStopNumber = d.BookPickupStopNumber;
            all.PurchaseOrderNumber = d.LOADPO;
            all.OrderNumber = d.LOADOrderNumber;
            all.SHID = d.BookSHID;
            all.CarrierPro = d.BookShipCarrierProNumber;
            all.ScheduledToLoad = d.LOADDATE;
            all.RequestedToArrive = d.SCHEDULEDATE;
            all.AssignedCarrier = d.LOADCarrierName;

            all.DestinationName = d.LOADDestName;
            all.DestinationAddress1 = d.LOADDestAdd1;
            all.DestinationAddress2 = d.LOADDestAdd2;
            all.DestinationCity = d.LOADDestCity;
            all.DestinationState = d.LOADDestState;
            all.DestinationZip = d.LOADDestZip;
            all.DestinationCountry = d.LOADDestCountry;

            all.OrigName = d.LOADPickUpName;
            all.OrigAddress1 = d.LOADPickUpAdd1;
            all.OrigAddress2 = d.LOADPickUPAdd2;
            all.OrigCity = d.LOADPickUpCity;
            all.OrigState = d.LOADPickUpState;
            all.OrigZip = d.LOADPickUpZip;
            all.OrigCountry = d.LoadPickUpCountry;

            all.BookCarrScheduleDate = d.LOADToLoadDate;
            //all.BookCarrScheduleTime = d.LOADScheduleTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrScheduleTime = (d.LOADScheduleTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrActualDate = d.LOADActPickupDate;
            //all.BookCarrActualTime = d.LOADActPickupTime;
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            all.BookCarrActualTime = (d.LOADActPickupTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrStartLoadingDate = d.LOADStartLoadingDate;
            //all.BookCarrStartLoadingTime = d.LOADStartLoadingTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrStartLoadingTime = (d.LOADStartLoadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrFinishLoadingDate = d.LOADFinishLoadingDate;
            //all.BookCarrFinishLoadingTime = d.LOADFinishLoadingTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrFinishLoadingTime = (d.LOADFinishLoadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrActLoadComplete_Date = d.LOADActLoadComplete_Date;
            //all.BookCarrActLoadCompleteTime = d.LOADActLoadCompleteTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrActLoadCompleteTime = (d.LOADActLoadCompleteTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrDockPUAssigment = d.LOADDockPUAssigment;
            all.BookCarrApptDate = d.LOADDelScheduleDate;
            //all.BookCarrApptTime = d.LOADDelScheduleTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrApptTime = (d.LOADDelScheduleTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrActDate = d.LOADActDelDate;
            //all.BookCarrActTime = d.LOADActDelTime;
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            all.BookCarrActTime = (d.LOADActDelTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrStartUnloadingDate = d.LOADDelStartUnloadingDate;
            //all.BookCarrStartUnloadingTime = d.LOADDelStartUnloadingTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrStartUnloadingTime = (d.LOADDelStartUnloadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrFinishUnloadingDate = d.LOADDelFinishUnloadingDate;
            //all.BookCarrFinishUnloadingTime = d.LOADDelFinishUnloadingTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrFinishUnloadingTime = (d.LOADDelFinishUnloadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrActUnloadCompDate = d.LOADActLoadCompDate;
            //all.BookCarrActUnloadCompTime = d.LOADActLoadCompTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrActUnloadCompTime = (d.LOADActLoadCompTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrDockDelAssignment = d.LOADDelDock;
            all.BookCarrTrailerNo = d.LOADTrailerNo;
            all.BookCarrSealNo = d.LOADSealNo;
            all.BookCarrDriverNo = d.LOADDriverNo;
            all.BookCarrDriverName = d.LOADDriverName;
            //all.BookCarrTripNo = d.LOADTripNo;
            //all.BookCarrRouteNo = d.LOADRouteNo;
            all.BookWhseAuthorizationNo = d.LOADWhseAuthorizationNo;
            all.Comments = d.Comments;
            all.BookNotes1 = d.BookNotesVisable1;
            all.BookNotes2 = d.BookNotesVisable2;
            all.BookNotes3 = d.BookNotesVisable3;
            all.AssignedProNumber = d.BookShipCarrierProNumber;
            all.BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw;
            all.BookShipCarrierProControl = d.BookShipCarrierProControl;
            all.AssignedCarrierName = d.BookShipCarrierName;
            all.AssignedCarrierNumber = d.BookShipCarrierNumber;
            all.AssignedCarrierContact = d.BookCarrierContact;
            all.AssignedCarrierContactPhone = d.BookCarrierContactPhone;
            all.BookModDate = d.BookModDate;
            all.BookModUser = d.BookModUser;
            all.BookAMSPickupApptControl = d.BookAMSPickupApptControl ?? 0;
            all.BookAMSDeliveryApptControl = d.BookAMSDeliveryApptControl ?? 0;
            all.BookLoadControl = d.BookLoadControl;
            all.BookFinAPActWgt = d.BookFinAPActWgt;
            all.BookCarrBLNumber = d.BookCarrBLNumber;
            all.Page = d.Page;
            all.Pages = d.Pages;
            all.RecordCount = d.RecordCount;
            all.ROW_NUMBER = d.ROW_NUMBER;

            all.TranCode = d.LOADTRANSCODE; //Added By LVV on 7/1/19 for v-8.2

            return all;
        }

        private Models.AllGridItem selectModelData(LTS.vBookAllItem d, int page, int pages, int RecordCount )
        {
            Models.AllGridItem all = new Models.AllGridItem();

            all.Control = d.LOADBookControl;
            all.ProNumber = d.LOADProNumber;
            all.CnsNumber = d.LOADCONSPREFIX;
            all.StopNumber = d.LOADStopNo;
            all.BookPickupStopNumber = d.BookPickupStopNumber;
            all.PurchaseOrderNumber = d.LOADPO;
            all.OrderNumber = d.LOADOrderNumber;
            all.SHID = d.BookSHID;
            all.CarrierPro = d.BookShipCarrierProNumber;
            all.ScheduledToLoad = d.LOADDATE;
            all.RequestedToArrive = d.SCHEDULEDATE;
            all.AssignedCarrier = d.LOADCarrierName;

            all.DestinationName = d.LOADDestName;
            all.DestinationAddress1 = d.LOADDestAdd1;
            all.DestinationAddress2 = d.LOADDestAdd2;
            all.DestinationCity = d.LOADDestCity;
            all.DestinationState = d.LOADDestState;
            all.DestinationZip = d.LOADDestZip;
            all.DestinationCountry = d.LOADDestCountry;

            all.OrigName = d.LOADPickUpName;
            all.OrigAddress1 = d.LOADPickUpAdd1;
            all.OrigAddress2 = d.LOADPickUPAdd2;
            all.OrigCity = d.LOADPickUpCity;
            all.OrigState = d.LOADPickUpState;
            all.OrigZip = d.LOADPickUpZip;
            all.OrigCountry = d.LoadPickUpCountry;

            all.BookCarrScheduleDate = d.LOADToLoadDate;
            //all.BookCarrScheduleTime = d.LOADScheduleTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrScheduleTime = (d.LOADScheduleTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrActualDate = d.LOADActPickupDate;
            //all.BookCarrActualTime = d.LOADActPickupTime;
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            all.BookCarrActualTime = (d.LOADActPickupTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrStartLoadingDate = d.LOADStartLoadingDate;
            //all.BookCarrStartLoadingTime = d.LOADStartLoadingTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrStartLoadingTime = (d.LOADStartLoadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrFinishLoadingDate = d.LOADFinishLoadingDate;
            //all.BookCarrFinishLoadingTime = d.LOADFinishLoadingTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrFinishLoadingTime = (d.LOADFinishLoadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrActLoadComplete_Date = d.LOADActLoadComplete_Date;
            //all.BookCarrActLoadCompleteTime = d.LOADActLoadCompleteTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrActLoadCompleteTime = (d.LOADActLoadCompleteTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrDockPUAssigment = d.LOADDockPUAssigment;
            all.BookCarrApptDate = d.LOADDelScheduleDate;
            //all.BookCarrApptTime = d.LOADDelScheduleTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrApptTime = (d.LOADDelScheduleTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrActDate = d.LOADActDelDate;
            //all.BookCarrActTime = d.LOADActDelTime;
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            all.BookCarrActTime = (d.LOADActDelTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrStartUnloadingDate = d.LOADDelStartUnloadingDate;
            //all.BookCarrStartUnloadingTime = d.LOADDelStartUnloadingTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrStartUnloadingTime = (d.LOADDelStartUnloadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrFinishUnloadingDate = d.LOADDelFinishUnloadingDate;
            //all.BookCarrFinishUnloadingTime = d.LOADDelFinishUnloadingTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrFinishUnloadingTime = (d.LOADDelFinishUnloadingTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrActUnloadCompDate = d.LOADActLoadCompDate;
            //all.BookCarrActUnloadCompTime = d.LOADActLoadCompTime;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            all.BookCarrActUnloadCompTime = (d.LOADActLoadCompTime ?? DateTime.Parse("01/01/2021 00:01")).ToString("HH:mm");
            all.BookCarrDockDelAssignment = d.LOADDelDock;
            all.BookCarrTrailerNo = d.LOADTrailerNo;
            all.BookCarrSealNo = d.LOADSealNo;
            all.BookCarrDriverNo = d.LOADDriverNo;
            all.BookCarrDriverName = d.LOADDriverName;
            //all.BookCarrTripNo = d.LOADTripNo;
            //all.BookCarrRouteNo = d.LOADRouteNo;
            all.BookWhseAuthorizationNo = d.LOADWhseAuthorizationNo;
            all.Comments = d.Comments;
            all.BookNotes1 = d.BookNotesVisable1;
            all.BookNotes2 = d.BookNotesVisable2;
            all.BookNotes3 = d.BookNotesVisable3;
            all.AssignedProNumber = d.BookShipCarrierProNumber;
            all.BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw;
            all.BookShipCarrierProControl = d.BookShipCarrierProControl;
            all.AssignedCarrierName = d.BookShipCarrierName;
            all.AssignedCarrierNumber = d.BookShipCarrierNumber;
            all.AssignedCarrierContact = d.BookCarrierContact;
            all.AssignedCarrierContactPhone = d.BookCarrierContactPhone;
            all.BookModDate = d.BookModDate;
            all.BookModUser = d.BookModUser;
            all.BookAMSPickupApptControl = d.BookAMSPickupApptControl;
            all.BookAMSDeliveryApptControl = d.BookAMSDeliveryApptControl;
            all.BookLoadControl = d.BookLoadControl;
            all.BookFinAPActWgt = d.BookFinAPActWgt;
            all.BookCarrBLNumber = d.BookCarrBLNumber;  
             all.Page = page;
            all.Pages = pages;
            all.RecordCount = RecordCount;
            all.ROW_NUMBER = d.LOADBookControl;
            all.TranCode = d.LOADTRANSCODE; //Added By LVV on 7/1/19 for v-8.2
            all.BookItemOrderNumbers = d.BookItemOrderNumbers; //Added By LVV on 11/7/19 Ticket #201909200751
            return all;
        }

        
        private DTO.AllItem selectDTOData(Models.AllGridItem m)
        {
            //TODO format the dates using NT methods
            //only assign the ones we care about -- comment out the rest
            DTO.AllItem dto = new DTO.AllItem();
            dto.ApplyCommentsToCNS = m.ApplyCommentsToCNS;
            dto.ApplyToAllDestinations = m.ApplyToAllDestinations;
            dto.ApplyToAllPickups = m.ApplyToAllPickups;
            dto.AssignedCarrier = m.AssignedCarrier;
            dto.AssignedCarrierContact = m.AssignedCarrierContact;
            dto.AssignedCarrierContactPhone = m.AssignedCarrierContactPhone;
            dto.AssignedCarrierName = m.AssignedCarrierName;
            dto.AssignedCarrierNumber = m.AssignedCarrierNumber;
            dto.AssignedProNumber = m.AssignedProNumber;
            dto.BookPickupStopNumber = m.BookPickupStopNumber;
            dto.CnsNumber = m.CnsNumber;
            dto.Comments = m.Comments;
            dto.Control = m.Control;
            dto.DestinationName = m.DestinationName;
            dto.OrderNumber = m.OrderNumber;
            dto.ProNumber = m.ProNumber;
            dto.PurchaseOrderNumber = m.PurchaseOrderNumber;
            dto.SHID = m.SHID;
            dto.Status = m.Status;
            dto.StopNumber = m.StopNumber;
            //dto.DestinationCity = m.DestinationCity;
            //dto.DestinationState = m.DestinationState;
            //dto.RequestedToArrive = m.RequestedToArrive;
            //dto.ScheduledToLoad = m.ScheduledToLoad;
            //dto.BookAMSDeliveryApptControl = m.BookAMSDeliveryApptControl;
            //dto.BookAMSPickupApptControl = m.BookAMSPickupApptControl;
            //dto.BookLoadControl = m.BookLoadControl;
            //dto.BookNotes1 = m.BookNotes1;
            //dto.BookNotes2 = m.BookNotes2;
            //dto.BookNotes3 = m.BookNotes3;
            //dto.BookShipCarrierProControl = m.BookShipCarrierProControl;
            //dto.BookShipCarrierProNumberRaw = m.BookShipCarrierProNumberRaw;
            //dto.CarrierPro = m.CarrierPro;
            //dto.BookModDate = m.BookModDate;
            //dto.BookModUser = m.BookModUser; 

            dto.CarrierData = new DTO.BookCarrier();
            dto.CarrierData.BookCarrActDate = m.BookCarrActDate.HasValue ? m.BookCarrActDate.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrActTime = m.BookCarrActTime.HasValue ? m.BookCarrActTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-26-2024
            dto.CarrierData.BookCarrActTime = Utilities.convertStringTimeToDate(m.BookCarrActTime, dto.CarrierData.BookCarrActDate);
            dto.CarrierData.BookCarrActLoadComplete_Date = m.BookCarrActLoadComplete_Date.HasValue ? m.BookCarrActLoadComplete_Date.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrActLoadCompleteTime = m.BookCarrActLoadCompleteTime.HasValue ? m.BookCarrActLoadCompleteTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrActLoadCompleteTime = Utilities.convertStringTimeToDate(m.BookCarrActLoadCompleteTime, dto.CarrierData.BookCarrActLoadComplete_Date);
            dto.CarrierData.BookCarrActualDate = m.BookCarrActualDate.HasValue ? m.BookCarrActualDate.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrActualTime = m.BookCarrActualTime.HasValue ? m.BookCarrActualTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrActualTime = Utilities.convertStringTimeToDate(m.BookCarrActualTime, dto.CarrierData.BookCarrActualDate);
            dto.CarrierData.BookCarrActUnloadCompDate = m.BookCarrActUnloadCompDate.HasValue ? m.BookCarrActUnloadCompDate.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrActUnloadCompTime = m.BookCarrActUnloadCompTime.HasValue ? m.BookCarrActUnloadCompTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrActUnloadCompTime = Utilities.convertStringTimeToDate(m.BookCarrActUnloadCompTime, dto.CarrierData.BookCarrActUnloadCompDate);
            dto.CarrierData.BookCarrApptDate = m.BookCarrApptDate.HasValue ? m.BookCarrApptDate.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrApptTime = m.BookCarrApptTime.HasValue ? m.BookCarrApptTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrApptTime = Utilities.convertStringTimeToDate(m.BookCarrApptTime, dto.CarrierData.BookCarrApptDate);
            dto.CarrierData.BookCarrDockDelAssignment = GetDockDoorAssignment(m.BookCarrDockDelAssignment);
            dto.CarrierData.BookCarrDockPUAssigment = GetDockDoorAssignment(m.BookCarrDockPUAssigment);
            dto.CarrierData.BookCarrFinishLoadingDate = m.BookCarrFinishLoadingDate.HasValue ? m.BookCarrFinishLoadingDate.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrFinishLoadingTime = m.BookCarrFinishLoadingTime.HasValue ? m.BookCarrFinishLoadingTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrFinishLoadingTime = Utilities.convertStringTimeToDate(m.BookCarrFinishLoadingTime, dto.CarrierData.BookCarrFinishLoadingDate);
            dto.CarrierData.BookCarrFinishUnloadingDate = m.BookCarrFinishUnloadingDate.HasValue ? m.BookCarrFinishUnloadingDate.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrFinishUnloadingTime = m.BookCarrFinishUnloadingTime.HasValue ? m.BookCarrFinishUnloadingTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrFinishUnloadingTime = Utilities.convertStringTimeToDate(m.BookCarrFinishUnloadingTime, dto.CarrierData.BookCarrFinishUnloadingDate);
            dto.CarrierData.BookCarrScheduleDate = m.BookCarrScheduleDate.HasValue ? m.BookCarrScheduleDate.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrScheduleTime = m.BookCarrScheduleTime.HasValue ? m.BookCarrScheduleTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrScheduleTime = Utilities.convertStringTimeToDate(m.BookCarrScheduleTime, dto.CarrierData.BookCarrScheduleDate);
            dto.CarrierData.BookCarrTrailerNo = m.BookCarrTrailerNo;
            dto.CarrierData.BookCarrSealNo = m.BookCarrSealNo;
            dto.CarrierData.BookCarrDriverNo = m.BookCarrDriverNo;
            dto.CarrierData.BookCarrDriverName = m.BookCarrDriverName;
            dto.CarrierData.BookWhseAuthorizationNo = m.BookWhseAuthorizationNo;
            dto.CarrierData.BookCarrStartLoadingDate = m.BookCarrStartLoadingDate.HasValue ? m.BookCarrStartLoadingDate.Value : DateTime.MinValue;
            //dto.CarrierData.BookCarrStartLoadingTime = m.BookCarrStartLoadingTime.HasValue ? m.BookCarrStartLoadingTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrStartLoadingTime = Utilities.convertStringTimeToDate(m.BookCarrStartLoadingTime, dto.CarrierData.BookCarrStartLoadingDate);
            dto.CarrierData.BookCarrStartUnloadingDate = m.BookCarrStartUnloadingDate.HasValue ? m.BookCarrStartUnloadingDate.Value : DateTime.MinValue; 
            //dto.CarrierData.BookCarrStartUnloadingTime = m.BookCarrStartUnloadingTime.HasValue ? m.BookCarrStartUnloadingTime.Value : DateTime.MinValue;
            // Modefied By MA for v-8.5.4.006 on 04-29-2024
            dto.CarrierData.BookCarrStartUnloadingTime = Utilities.convertStringTimeToDate(m.BookCarrStartUnloadingTime, dto.CarrierData.BookCarrStartUnloadingDate);
            dto.CarrierData.BookFinAPActWgt = m.BookFinAPActWgt.HasValue ? m.BookFinAPActWgt.Value : 0;
            dto.CarrierData.BookCarrBLNumber = m.BookCarrBLNumber;
            //dto.CarrierData.BookAMSDeliveryApptControl = m.BookAMSDeliveryApptControl;
            //dto.CarrierData.BookAMSPickupApptControl = m.BookAMSPickupApptControl;
            //dto.CarrierData.BookModDate = m.BookModDate;
            //dto.CarrierData.BookModUser = m.BookModUser;

            //Added By LVV on 9/19/19 for Bing Maps
            DTO.tblStop clt = new DTO.tblStop()
            {
                StopControl = m.CLTStopControl,
                StopName = m.CLTStopName,
                StopAddress1 = m.CLTStopAddress1,
                StopCity = m.CLTStopCity,
                StopState = m.CLTStopState,
                StopCountry = m.CLTStopCountry,
                StopZip = m.CLTStopZip,
                StopLatitude = m.CLTStopLatitude,
                StopLongitude = m.CLTStopLongitude
            };     
            dto.setCommentLocation(clt);
            dto.setCommentDate(m.CLTDate);
            dto.setCommentTime(m.CLTTime);

            return dto;
        }


        #endregion

        #region " REST Services"

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllItems(filter);
        }

        [HttpGet, ActionName("GetAllItems")]
        public Models.Response GetAllItems(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if(!authenticateController(ref response)) { return response; }
            try
            {
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }

                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                Models.AllGridItem[] records = new Models.AllGridItem[] { };
                LTS.vBookAllItem[] oData = new LTS.vBookAllItem[] { };
                DAL.NGLAllItemData oDAL = new DAL.NGLAllItemData(Parameters);
                int count = 0;

                int page = f.page;
                int pagesize = f.take;
                int pages = 0;
                int RecordCount = 0;
                oData = oDAL.GetAllItemsFiltered365(f,ref RecordCount);

                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (pagesize < 1) { pagesize = 1; } //avoid divide by 0 error
                    pages = count / pagesize;
                    if (pages < 1 ) { pages = 1; }
                    if (string.IsNullOrWhiteSpace(f.sortName))
                    {
                        records = (from e in oData
                                   orderby e.LOADCONSPREFIX descending, e.LOADStopNo ascending
                                   select selectModelData(e,page,pages,count)).ToArray();
                    } else { records = (from e in oData select selectModelData(e, page, pages, count)).ToArray(); }
                    if (RecordCount > count) { count = RecordCount; }
                }                           
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.AllGridItem item)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                DTO.AllItem dtoRecord = selectDTOData(item);
                dtoRecord.AssignedCarrierContactPhone = Utilities.removeNonNumericText(dtoRecord.AssignedCarrierContactPhone);
                DTO.WCFResults wcfResults = bll.AllTabSave(dtoRecord);
                //Check for messages
                if (wcfResults != null)
                {
                    string strMsg = "";
                    if (wcfResults.Warnings != null && wcfResults.Warnings.Count() > 0) { strMsg = wcfResults.concatWarnings() + " "; }
                    if (wcfResults.Errors != null && wcfResults.Errors.Count() > 0) { strMsg += wcfResults.concatErrors(); }
                    if (!string.IsNullOrWhiteSpace(strMsg)) { response.Errors = strMsg; }
                    //TODO -- FIGURE OUT HOW TO POPULATE STATUS BAR WITH MESSAGES
                    //Set the status bar message to the log
                    var strStatusMsg = wcfResults.getLogAsSingleStr(".", false);
                }               
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }


        protected string Truncate(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) {  return ""; }
            if (value.Length <= length) { return value; }
            return value.Substring(0, length);
        }
        
        protected string GetDockDoorAssignment(string value)
        {           
            if (string.IsNullOrEmpty(value)) { return ""; } //validate          
            //try to parse
            if (value.Length <= 10)
            {
                return value;
            }
            else
            {
                return Truncate(value, 10);
                //woops - they didn't put in a good format - - should be less than 10 characters
                //throw new Exception("The dock door assignment " + value + " should be less than 10 characters.");
            }
        }


        #endregion
    }
}