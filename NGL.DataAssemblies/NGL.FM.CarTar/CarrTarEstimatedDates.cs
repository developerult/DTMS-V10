using Ngl.FreightMaster.Data;
using SerilogTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using Models = Ngl.FreightMaster.Data.Models;

namespace NGL.FM.CarTar
{
    public  class CarrTarEstimatedDates : TarBaseClass
    {
        #region " Constructors "

        public CarrTarEstimatedDates(DAL.WCFParameters oParameters,
           Load load,
           DTO.CarriersByCost[] carriersByCost,
            List<DTO.CarrTarNoDriveDays> NoDriveDaysList)
            : base()
        {
            if (oParameters == null)
            {
                populateSampleParameterData();  //this will not really be used in production
            }
            else
            {
                Parameters = oParameters;
                this.load = load; 
                this.NoDriveDaysList = NoDriveDaysList;
                this.carriersByCost = carriersByCost;
                this.reqDateSetup = false; //the method setupdata should be called by the caller before calcuating so we limit the number of loops.
                this.loadDateSetup = false; //the method setupdata should be called by the caller before calcuating so we limit the number of loops.
                this.inputsSetup = false;
            }
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrTarEstimatedDates";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        private List<DTO.CarrTarNoDriveDays> NoDriveDaysList { get; set; } 
        private Load load { get; set; }
        private Nullable<DateTime> GreatestLoadShipDate { get; set; }
        private Nullable<DateTime> FinalDestinationRequireDate { get; set; } 
        private bool loadDateSetup { get; set; }
        private bool reqDateSetup { get; set; }
        private bool inputsSetup { get; set; }
        //private bool setupDataCalledResult { get; set; } 
        private DTO.CarriersByCost[] carriersByCost { get; set; }

	    #endregion

        #region methods

        /// <summary>
        /// Calculates the Must Leave By Date BookRev and CarriersByCost objects must be provided in object constructor.  
        /// Throws InvalidOperationException using one of the following localized string variables: 
        /// E_InvalidEstDateSetup, or E_InvalidEstDateReqDate
        /// </summary>
        /// <returns></returns>
        public void CalcMustLeaveBy()
        {
            if (this.inputsSetup == false)
            {
                throw new InvalidOperationException("E_InvalidEstDateSetup");
            }
            if (reqDateSetup == false)
            {
                throw new InvalidOperationException("E_InvalidEstDateReqDate");
            } 
           DoMustLeaveByDateCalculation();  
        }

        /// <summary>
        /// Calculate the Must Leave By Date 
        /// </summary>
        /// <remarks>
        /// Modified by RHR v-7.0.5.102 9/30/20126  
        ///   Hot fix for invalid values when using line haul or when transit days are zero
        /// Modified by RHR for v-8.5.3.006 on 11/29/2022 
        ///     we now use the Holiday Matrix and call the new improved CalculateMustLeaveByDate procedure
        /// </remarks>
        private void DoMustLeaveByDateCalculation()
        {

            // In v-8.5.3.006 the first lane value for LaneTransLeadTimeCalcType are used for all the orders on the load
            //   primarily because we are not supporting multi-stop APIs in this version. 
            //   in the future this may be modified to include multi-stop APIs
            // First get the Lane control from the booking
            int iLaneControl = this.load.BookRevenues[0].BookODControl;
            int? BookLeadTimeAutomationDaysByMile = this.load.BookRevenues[0].BookLeadTimeAutomationDaysByMile;
            int? BookLeadTimeLTLMinimum = this.load.BookRevenues[0].BookLeadTimeLTLMinimum;
            Double BookTotalWgt  = this.load.BookRevenues.Sum( x => x.BookTotalWgt);
            Double? NbrMiles = this.load.BookRevenues.Sum(x => x.BookMilesFrom);
            int iComp = this.load.BookRevenues[0].BookCustCompControl;
           
            if (this.load.LastStopBookRevenue != null && this.load.LastStopBookRevenue.BookControl != 0)
            {
                iLaneControl = this.load.LastStopBookRevenue.BookODControl;
                BookLeadTimeAutomationDaysByMile = this.load.LastStopBookRevenue.BookLeadTimeAutomationDaysByMile;
                BookLeadTimeLTLMinimum = this.load.LastStopBookRevenue.BookLeadTimeLTLMinimum;
                iComp = this.load.BookRevenues[0].BookCustCompControl;
            }
            // next get the LaneTransLeadTimeCalcType  this is used to modify the ship date or required date based on transit hours/days
            int iGetLaneTransLeadTimeCalcType = 0;
            int iLaneTLTBenchmark = 0;
            // in version 8.5.3.006 we use the last stops lane number to get the lane transit time calculated from statistical analysis
            Models.LaneLeadTimeData oLaneLeadtime = NGLLaneData.GetLaneTransLeadTimeData(iLaneControl);
            if (oLaneLeadtime != null && oLaneLeadtime.LaneControl != 0)
            {
                iGetLaneTransLeadTimeCalcType = oLaneLeadtime.LaneTransLeadTimeCalcType ?? 0;
                iLaneTLTBenchmark = oLaneLeadtime.LaneTLTBenchmark ?? 0;
            }

            Models.HolidayMatrix oHolidayM = new Models.HolidayMatrix();
            
            if (!this.GreatestLoadShipDate.HasValue) { this.GreatestLoadShipDate = DateTime.Now; }
            if (!this.FinalDestinationRequireDate.HasValue) { this.FinalDestinationRequireDate = this.GreatestLoadShipDate.Value.AddDays(5); }
            int iYear = this.FinalDestinationRequireDate.Value.Year;
            int iMonth = this.FinalDestinationRequireDate.Value.Month;
            //load HolidayMatrix defaults
            DAL.NGLCompCalData oCompCal = new DAL.NGLCompCalData(this.Parameters);
            oHolidayM.LoadCompDatesList(iComp, oCompCal.GetCompCalsFiltered(iComp).ToList(), iYear);
            Models.DriveDays oCompLoadWeekends = NGLCompData.GetCompWeekendLoadSettings(iComp);
            //Modified by RHR for v-8.5.3.007 on 06/30/2023 fixed null reference exception on oCompLoadWeekends
            if (oCompLoadWeekends == null || ((oCompLoadWeekends.DriveSat ?? false) == false)) { oHolidayM.CompClosedOnSaturday(iMonth, iYear); }
            if (oCompLoadWeekends == null || ((oCompLoadWeekends.DriveSun ?? false) == false)) { oHolidayM.CompClosedOnSunday(iMonth, iYear); }

            foreach (DTO.CarriersByCost cItem in this.carriersByCost)
            {
                // calculate the transit time using lead time workflow settings
                cItem.BookCarrTransitTime = NGLBookRevenueData.CalculateTenderTransitTimes(ref BookLeadTimeAutomationDaysByMile, ref BookLeadTimeLTLMinimum, BookTotalWgt, NbrMiles, iComp, iLaneControl, iLaneTLTBenchmark, cItem.CarrTarEquipMatMaxDays);
                int iTransDays = cItem.BookCarrTransitTime < 24 ? 1 : cItem.BookCarrTransitTime / 24;
                oHolidayM.LoadCarrierNoDriveDays(cItem.CarrierControl, this.NoDriveDaysList, iYear, true);
                oHolidayM.DriveSaturday = cItem.CarrTarWillDriveSaturday;
                oHolidayM.DriveSunday = cItem.CarrTarWillDriveSunday;
                cItem.BookMustLeaveByDateTime = CalculateMustLeaveByDate(this.GreatestLoadShipDate, iTransDays, this.FinalDestinationRequireDate, oHolidayM);
                //Removed by RHR for v-8.5.3.006 on 11/29/2022 
                ////Modified by RHR v-7.0.5.102 9/30/20126  Hot fix for invalid values when using line haul or when transit days are zero
                //if (cItem.CarrTarEquipMatMaxDays == 0)
                //{
                //    continue;
                //}
                //else
                //{

                //    Nullable<DateTime> newMustLeavBy = this.FinalDestinationRequireDate;
                //    for (int i = 0; i < cItem.CarrTarEquipMatMaxDays; i++)
                //    {
                //        newMustLeavBy = newMustLeavBy.Value.AddDays(-1);
                //        //check saturday   
                //        if (newMustLeavBy.Value.DayOfWeek == DayOfWeek.Saturday)
                //        {
                //            //first check NoDriveOnSaturday boolean
                //            if (cItem.CarrTarWillDriveSaturday == false)
                //            {
                //                i--;
                //                continue;
                //            }
                //            //if we made it here, make sure it is not a holiday
                //            if (isHoliday(newMustLeavBy))
                //            {
                //                i--;
                //                continue;
                //            }
                //        }
                //        //check Sunday   
                //        if (newMustLeavBy.Value.DayOfWeek == DayOfWeek.Sunday)
                //        {
                //            //first check NoDriveOnSunday boolean
                //            if (cItem.CarrTarWillDriveSunday == false)
                //            {
                //                i--;
                //                continue;
                //            }
                //            //if we made it here, make sure it is not a holiday
                //            if (isHoliday(newMustLeavBy))
                //            {
                //                i--;
                //                continue;
                //            }
                //        }
                //        //now check the resstr which should be just holidays.,
                //        if (isHoliday(newMustLeavBy))
                //        {
                //            i--;
                //            continue;
                //        }
                //    }
                //    cItem.BookMustLeaveByDateTime = newMustLeavBy;
                //}
            } 
        }

        private static object lockObject = new object();
        /// <summary>
        /// Calculate the new required ship date (Must Leave By) to meet the Required date based on tranit time provided 
        /// adding time for closed pickup days and carrier non-drive days
        /// </summary>
        /// <param name="ShipDate"></param>
        /// <param name="TransDays"></param>
        /// <param name="RequireDate"></param>
        /// <param name="hMatrix"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.006 on 11/29/2022 added new logic to use new Holiday Matrix Model
        /// </remarks>
        public static DateTime? CalculateMustLeaveByDate(DateTime? ShipDate,int TransDays, DateTime? RequireDate,  Models.HolidayMatrix hMatrix )
        {
            lock (lockObject)
            {


                DateTime? dtRet = null;
                dtRet = ShipDate; // set must leave by to ship date as default           

                if (TransDays <= 0) { return dtRet; }

                Nullable<DateTime> newMustLeavBy = RequireDate;
                for (int iDay = 0; iDay < TransDays; iDay++)
                {
                    //each pass through the loop moves the must leave by back one day
                    newMustLeavBy = newMustLeavBy.Value.AddDays(-1);
                    //check saturday                
                    if (newMustLeavBy.Value.DayOfWeek == DayOfWeek.Saturday)
                    {
                        //first check NoDriveOnSaturday boolean
                        if (hMatrix.DriveSaturday == false)
                        {
                            iDay--;
                            continue;
                        }
                        //if we made it here, make sure it is not a holiday
                        if (hMatrix.isHoliday(newMustLeavBy))
                        {
                            iDay--;
                            continue;
                        }
                    }
                    //check Sunday   
                    if (newMustLeavBy.Value.DayOfWeek == DayOfWeek.Sunday)
                    {
                        //first check NoDriveOnSunday boolean
                        if (hMatrix.DriveSunday == false)
                        {
                            iDay--;
                            continue;
                        }
                        //if we made it here, make sure it is not a holiday
                        if (hMatrix.isHoliday(newMustLeavBy))
                        {
                            iDay--;
                            continue;
                        }
                    }
                    //now check the rest, should be just holidays
                    if (hMatrix.isHoliday(newMustLeavBy))
                    {
                        iDay--;
                        continue;
                    }
                }
                // finally we need to check if the pickup date is a drive day and the warehouse is open             
                while (
                        (newMustLeavBy.Value.DayOfWeek == DayOfWeek.Saturday && hMatrix.DriveSaturday == false)
                        ||
                        (newMustLeavBy.Value.DayOfWeek == DayOfWeek.Sunday && hMatrix.DriveSunday == false)
                        ||
                        hMatrix.isHoliday(newMustLeavBy, true, true)
                        || hMatrix.isCompClosed(newMustLeavBy))
                {
                    newMustLeavBy = newMustLeavBy.Value.AddDays(-1);
                }

                return newMustLeavBy;
            }
        }

        /// <summary>
        /// Calculates the Estimated Delivery Dates 
        /// Example:
        ///Load/Ship date: Friday 05/16/2014
        ///No Drive Saturday: true
        ///No Drive Sunday: true
        ///Holidays: None
        ///Transit Days: 3
        ///EDD = Wednesday 05/21/2014.          
        /// Throws InvalidOperationException using one of the following localized string variables: 
        /// E_InvalidEstDateSetup, or E_InvalidEstDateLoadDate
        /// </summary>
        /// <returns>
        /// throws InvalidOperationException if SetupData is not called first</returns>
        public void CalcEstimatedDeliveryDate()
        {
            if (this.inputsSetup == false)
            {
                throw new InvalidOperationException("E_InvalidEstDateSetup");
            } 
            if (loadDateSetup == false)
            {
                throw new InvalidOperationException("E_InvalidEstDateLoadDate");
            } 
            DoEDDCalculation(); 
             
        }

        /// <summary>
        /// Calcualate the Estimated Delivery Date
        /// </summary>
        /// <remarks>
        /// Modified by RHR v-7.0.5.102 9/30/20126  
        ///   Hot fix for invalid values when using line haul or when transit days are zero
        /// </remarks>
        private void DoEDDCalculation()
        {
            using (var operation = Logger.StartActivity("DoEDDCalculation for each carrier cost"))
            {
                foreach (DTO.CarriersByCost cItem in this.carriersByCost)
                {
                    using (var loopOperation = Logger.StartActivity("DoEDDCalculation for {CarrierName}", cItem.CarrierName))
                    {
                        //Modified by RHR v-7.0.5.102 9/30/20126  Hot fix for invalid values when using line haul or when transit days are zero
                        if (cItem.CarrTarEquipMatMaxDays == 0)
                        {
                            Logger.Information("Carrier {CarrierName} has 0 transit days, skipping", cItem.CarrierName);
                            continue;
                        }
                        else
                        {

                            Nullable<DateTime> newEDD = this.GreatestLoadShipDate;
                            for (int i = 0; i < cItem.CarrTarEquipMatMaxDays; i++)
                            {
                                
                                newEDD = newEDD.Value.AddDays(1);
                                Logger.Information("Calculating EDD for {CarrierName} on {GreatestLoadShipDate} + 1 Day = {newEDD} which is a {DayOfWeek}", cItem.CarrierName,this.GreatestLoadShipDate, newEDD, newEDD.Value.DayOfWeek);                                
                                
                                if (newEDD.Value.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    Logger.Information("Checking if {CarrierName} will drive on Saturday ({WillDriveSaturday})", cItem.CarrierName, cItem.CarrTarWillDriveSaturday);
                                    //first check DriveOnSaturday boolean
                                    if (cItem.CarrTarWillDriveSaturday == false)
                                    {
                                        i--;
                                        continue;
                                    }
                                    //if we made it here, make sure it is not a holiday
                                    
                                    if (isHoliday(newEDD))
                                    {
                                        Logger.Information("New EDD is a Holiday");
                                        i--;
                                        continue;
                                    }
                                }
                                //check Sunday   
                                if (newEDD.Value.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    Logger.Information("Checking if {CarrierName} will drive on Saturday ({WillDriveSunday})", cItem.CarrierName, cItem.CarrTarWillDriveSunday);
                                    //first check DriveOnSunday boolean
                                    if (cItem.CarrTarWillDriveSunday == false)
                                    {
                                        i--;
                                        continue;
                                    }
                                    //if we made it here, make sure it is not a holiday
                                    if (isHoliday(newEDD))
                                    {
                                        Logger.Information("New EDD is a Holiday");
                                        i--;
                                        continue;
                                    }
                                }
                                //now check the resstr which should be just holidays.,
                                if (isHoliday(newEDD))
                                {
                                    Logger.Information("New EDD is a Holiday");
                                    i--;
                                    continue;
                                }
                            }
                            cItem.BookExpDelDateTime = newEDD;
                        }

                    }
                }
            }
        }

        public DTO.CarriersByCost[] GetCarriersByCostCalculated()
        {
            return this.carriersByCost;
        }

        public bool setupData()
        { 
            if (validateInputs() == false) {
                this.inputsSetup = false;
                return this.inputsSetup;
            }
            this.setFinalDestinationRequireDate();
            this.setGreatestLoadShipDate(); 
            validateLoadDateSetProperties(); 
            validateReqDateSetProperties();
            this.inputsSetup = true;
            return this.inputsSetup;
        }
       
        private bool validateInputs()
        {
            if (this.carriersByCost == null || this.carriersByCost.Length == 0)
            {
                return false;
            }
            if (this.load.BookRevenues == null || this.load.BookRevenues.Length == 0)
            {
                return false;
            }
            return true;
        }

        private bool validateLoadDateSetProperties()
        {
            if (this.GreatestLoadShipDate == null || this.GreatestLoadShipDate.HasValue == false) { this.loadDateSetup = false; return this.loadDateSetup; }
            this.loadDateSetup = true;
            return this.loadDateSetup;
        }
        private bool validateReqDateSetProperties()
        {
            if (this.FinalDestinationRequireDate == null || this.FinalDestinationRequireDate.HasValue == false) { this.reqDateSetup = false; return this.reqDateSetup; }
            this.reqDateSetup = true;
            return this.reqDateSetup;
        }
       
        private Nullable<DateTime> setGreatestLoadShipDate()
        { 
            var greatest = this.load.BookRevenues.Max(x => x.BookDateLoad); 
            this.GreatestLoadShipDate = greatest;
            return this.GreatestLoadShipDate;
        }

        /// <summary>
        /// for version 6.4
        /// , we will only support Must Leave by Date based on the final destination.  
        /// So we must use the final destination (last stop) required date.
        /// </summary>
        /// <returns></returns>
        private Nullable<DateTime> setFinalDestinationRequireDate()
        {
            DTO.BookRevenue rev = this.load.LastStopBookRevenue;
            if (rev == null || rev.BookDateRequired == null || rev.BookDateRequired.HasValue == false)
            {
                this.FinalDestinationRequireDate = null;
            }
            else
            {
                this.FinalDestinationRequireDate = rev.BookDateRequired;
            }
            return this.FinalDestinationRequireDate;
        }
 
        private bool isHoliday(Nullable<DateTime> day)
        {
            if (day == null) { return false; }
            var item = from d in this.NoDriveDaysList where d.CarrTarNDDNoDrivingDate == day.Value select d;
            if (item == null || item.Count() == 0)
            {
                return false;
            }
            else{
                return true;
            }
        }

        #endregion
    }
    
}
