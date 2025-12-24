using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;
using LTS = Ngl.FreightMaster.Data.LTS;
using Serilog;
using Serilog.Context;
using SerilogTracing;

namespace NGL.FM.CarTar
{
    /// <summary>
    /// BookRev Class
    /// </summary>
    public class BookRev : TarBaseClass
    {
        #region " Constructors "

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public BookRev(DAL.WCFParameters oParameters)
            : base()
        {
            Logger = Logger.ForContext<BookRev>();
            if (oParameters == null)
            {
                ////populateSampleParameterData();  //this will not really be used in production
            }
            else
            {
                Parameters = oParameters;
            }
        }

        #endregion

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.BookRev";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        /// <summary>
        /// This property is used for accessing the instance of the CarrierTariffsPivotProcessor class.
        /// </summary>
        private CarrierTariffsPivotProcessor _carrierTariffsPivotProcessorInstance = null;
        public CarrierTariffsPivotProcessor CarrierTariffsPivotProcessorInstance
        {
            get
            {
                if (_carrierTariffsPivotProcessorInstance == null)
                {
                    // Create an instance of the CarrierTariffsPivotProcessor class.
                    _carrierTariffsPivotProcessorInstance = new CarrierTariffsPivotProcessor(Parameters);
                }
                return _carrierTariffsPivotProcessorInstance;
            }

        }

        /// <summary>
        /// This property is used for accessing the instance of the RateCarrierTariff class.
        /// </summary>
        private RateCarrierTariff _rateCarrierTariffInstance = null;
        public RateCarrierTariff RateCarrierTariffInstance
        {
            get
            {
                if (Parameters != null)
                {

                    if (_rateCarrierTariffInstance == null)
                    {
                        // Create and instance of the RateCarrierTariff class.
                        _rateCarrierTariffInstance = new RateCarrierTariff(Parameters);
                    }
                }

                return _rateCarrierTariffInstance;
            }
        }

        #endregion

        #region " Tariff Engine Entry Methods

        /// <summary>
        /// THis overloaded method allows for calculations without using the database and is in memory. Typically used for SpotRates.
        /// </summary>
        /// <param name="bookControl"></param>
        /// <param name="carrierControl"></param>
        /// <param name="CalculationType"></param>
        /// <param name="CarrTarEquipControl"></param>
        /// <param name="prefered"></param>
        /// <param name="noLateDelivery"></param>
        /// <param name="validated"></param>
        /// <param name="optimizeByCapacity"></param>
        /// <param name="modeTypeControl"></param>
        /// <param name="tempType"></param>
        /// <param name="tariffTypeControl"></param>
        /// <param name="carrTarEquipMatClass"></param>
        /// <param name="carrTarEquipMatClassTypeControl"></param>
        /// <param name="carrTarEquipMatTarRateTypeControl"></param>
        /// <param name="agentControl"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        protected bool assignCarrierNoSave(
            ref DTO.CarrierCostResults results,
            DTO.BookRevenue[] inBookRevs,
            int carrierControl = 0,
            DAL.Utilities.AssignCarrierCalculationType CalculationType =
                DAL.Utilities.AssignCarrierCalculationType.RecalcuateSpotRate,
            bool autoCalculateBFC = true)
        {
            bool blnRet = false;
            using (var operation = Logger.StartActivity(
                       "assignCarrierNoSave(CarrierCostResults: {CarrierCostResults}, BookRevenues: {@BookRevenues}, CarrierControl: {CarrierControl}, AssignCarrierCalculationType: {AssignCarrierCalulationType}, AutoCalculateBFC: {AutoCalculateBFC}",
                       results, inBookRevs, carrierControl, CalculationType, autoCalculateBFC))
            {

                if (results == null)
                {
                    results = new DTO.CarrierCostResults();
                }

                Load load = new Load(inBookRevs, Parameters);
                results.AddLog("Validate Load Tran Code");

                bool validLoad = load.validLoad(ref results, CalculationType);
                if (validLoad)
                {

                    Load ratedLoad = null;

                    switch (CalculationType)
                    {
                        case DAL.Utilities.AssignCarrierCalculationType.RecalcuateNoBFC:
                            results.AddLog("Recalculate costs using current line haul but do not update the BFC.");
                            ratedLoad = recalculateUsingLineHaul(ref results, load, autoCalculateBFC);
                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.RecalcuateSpotRate:
                            ratedLoad = recalculateUsingLineHaul(ref results, load, autoCalculateBFC);
                            break;
                        default:
                            break;
                    }

                    if (results.Success)
                    {
                        results.AddLog("Update the Tran Code for each booking object.");
                        UpdateBookRevDefaultsBeforeSave(load, ref ratedLoad);
                        if (ratedLoad != null)
                        {
                            results.BookRevs = ratedLoad.BookRevenues.ToList();
                        }

                        blnRet = true;
                    }
                }
            }

            return blnRet;

        }



        /// <summary>
        /// Assign a carrier and calculate the book revenue costs.
        /// </summary>
        /// <param name="bookControl"></param>
        /// <param name="carrierControl"></param>
        /// <param name="CalculationType"></param>
        /// <param name="CarrTarEquipMatControl"></param>
        /// <param name="CarrTarEquipControl"></param>
        /// <param name="prefered"></param>
        /// <param name="noLateDelivery"></param>
        /// <param name="validated"></param>
        /// <param name="optimizeByCapacity"></param>
        /// <param name="modeTypeControl"></param>
        /// <param name="tempType"></param>
        /// <param name="tariffTypeControl"></param>
        /// <param name="carrTarEquipMatClass"></param>
        /// <param name="carrTarEquipMatClassTypeControl"></param>
        /// <param name="carrTarEquipMatTarRateTypeControl"></param>
        /// <param name="agentControl"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="CarrContact">Optional carrier contact data used if provided for the carrier contact information</param>
        /// <param name="lRestrictedCarriers">Optional a list of carriers not to use when assigning the carrier. 
        /// If provided these carriers and all their tariffs will be ignored.
        /// This parameter is only used when the CalculationType is DAL.Utilities.AssignCarrierCalculationType.Normal.
        /// </param>
        /// <returns>
        /// NOTE:  uses the CarrTarEquipControl to return a list of pivot records if provided. 
        /// If CarrTarEquipMatControl is provided it will be used if the Class Type and Class Code match.  
        /// If Class Type and Class Code match do not match the system will select the lowest cost match available.
        /// If CarrTarEquipControl is not provided lowest cost carrier is selected based on other parameters.
        /// 
        /// </returns>
        /// <remarks>
        /// Modified By RHR on 02/21/2020 for v-8.2.1.004
        ///     fixed bug where existing Carrier Contact information was replaced with default 
        /// </remarks>
        public DTO.CarrierCostResults assignCarrier(
            DTO.BookRevenue[] oBookRevs = null,
                                  bool prefered = true,
                                  bool noLateDelivery = false,
                                  bool validated = true,
                                  bool optimizeByCapacity = true,
                                  int tempType = 0,
                                  int tariffTypeControl = 0,
                                  string carrTarEquipMatClass = null,
                                  int carrTarEquipMatClassTypeControl = 0,
                                  int carrTarEquipMatTarRateTypeControl = 0,
                                  int agentControl = 0,
                                  int page = 1,
                                  int pagesize = 1000,
                                  DTO.CarrierCont CarrContact = null,
                                  List<int> lRestrictedCarriers = null,
                                  bool OptmisticConcurrencyOn = true)
        {
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();
            using (Logger.StartActivity("assignCarrier(oBookRevs: {BookRevs}, prefered: {prefered}", oBookRevs, prefered))
            {
                if (oBookRevs == null || oBookRevs.Count() < 1 || oBookRevs[0].BookControl == 0)
                {
                    results.Success = false;
                    results.AddMessage(Ngl.FreightMaster.Data.DataTransferObjects.CarrierCostResults.MessageEnum.M_NoOrdersFound);
                    return results;
                }

                int bookControl = oBookRevs[0].BookControl;
                DAL.Utilities.AssignCarrierCalculationType CalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal;
                int carrierControl = oBookRevs[0].BookCarrierControl;
                int CarrTarEquipMatControl = oBookRevs[0].BookCarrTarEquipMatControl;
                int CarrTarEquipControl = oBookRevs[0].BookCarrTarEquipControl;
                int modeTypeControl = oBookRevs[0].BookModeTypeControl;
                // Modified By RHR on 02/21/2020 for v-8.2.1.004
                int? existingCarrierContactControl = oBookRevs[0].BookCarrierContControl;

                if (carrierControl != 0 && CarrTarEquipControl != 0)
                {
                    Logger.Information("CarrierControl nad CarrTarEquipControl are not 0, setting Calculation type to UpdateCarrier");
                    CalculationType = DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier;
                }

                try
                {
                    results.AddLog("Read the booking records.");
                    // Get the list of BookRevenue objects and store them in a Load object.
                    Load load = new Load(oBookRevs, Parameters);
                    results.AddLog("Validate the Book Tran Code.");
                    using (LogContext.PushProperty("CalculationType", CalculationType))
                    {
                        bool validLoad =
                            load.validLoad(ref results,
                                CalculationType); //updates the load's CalculationType property on success


                        if (validLoad)
                        {

                            // Rate results will be stored on a cloned ratedLoad object.
                            Load ratedLoad = null;
                            //if UpdateCarrier and CarrTarEquipControl = 0 or any BookCarrTarEquipControl <> CarrTarEquipControl 
                            //we cannot just update the carrier costs we need to use normal processing

                            Logger.Information("CalculationType: {CalculationType}, CarrTarEquipControl: {CarrTarEquipControl}, BookRevenues: {@BookRevenues}", CalculationType, CarrTarEquipControl, load.BookRevenues);

                            if (CalculationType == DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier
                                &&
                                (
                                    CarrTarEquipControl == 0
                                    ||
                                    (
                                        load.BookRevenues != null
                                        && load.BookRevenues.Count() > 0
                                        && (load.BookRevenues.Any(x => x.BookCarrTarEquipControl != CarrTarEquipControl))
                                    )
                                )
                               )
                            {
                                Logger.Information("CalculationType is UpdateCarrier and CarrTarEquipControl is 0 or BookCarrTarEquipControl does not match CarrTarEquipControl, setting CalculationType to Normal");
                                CalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal;
                            }

                            // Perform the specified rating logic.
                            switch (CalculationType)
                            {
                                case DAL.Utilities.AssignCarrierCalculationType.Normal:
                                    Logger.Information("Normal Calculation - Assign lowest cost carrier.");
                                    results.AddLog("Assign lowest cost carrier.");
                                    // Get the Carrier Tariffs Pivot data using the filter data passed into this method.
                                    DTO.CarrierTariffsPivot[] carrierTariffsPivots =
                                        CarrierTariffsPivotProcessorInstance.getCarrierTariffsPivotWithPrecision(
                                            ref results,
                                            bookControl, carrierControl, prefered, noLateDelivery, validated,
                                            optimizeByCapacity,
                                            modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass,
                                            carrTarEquipMatClassTypeControl,
                                            carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize);

                                    Logger.Information("CarrierTariffsPivots: {@CarrierTariffsPivots}", carrierTariffsPivots);

                                    if (carrierTariffsPivots == null || carrierTariffsPivots.Count() < 1)
                                    {
                                        //Modified by RHR v-7.0.5.100 05/17/2016
                                        //added logic to change the message based on if we already have a carrier assigned
                                        if (carrierControl != 0)
                                        {
                                            results.AddLog(
                                                "No valid tariffs found.  Check carrier qualifications, equipment capacity, tariff settings, and rates for the last stop");
                                            Logger.Warning(
                                                "No valid tariffs found.  Check carrier qualifications, equipment capacity, tariff settings, and rates for the last stop");
                                        }
                                        else
                                        {
                                            Logger.Warning("No tariffs found");
                                            results.AddLog("No tariffs found.");
                                        }

                                        results.Success = false;
                                        return results; // There are no carriers to assign
                                    }

                                    if (lRestrictedCarriers != null && lRestrictedCarriers.Count() > 0)
                                    {
                                        carrierTariffsPivots =
                                            (from d in carrierTariffsPivots
                                             where !lRestrictedCarriers.Contains(d.CarrierControl)
                                             select d).ToArray();
                                        if (carrierTariffsPivots == null || carrierTariffsPivots.Count() < 1)
                                        {
                                            //Modified by RHR v-7.0.5.100 05/17/2016
                                            //modified the message to indicate that carrier restrictions are being implemented                                
                                            results.AddLog("No tariffs found due to carrier restrictions.");
                                            results.Success = false;
                                            return results; // There are no carriers to assign
                                        }
                                    }

                                    results.AddLog("Select the lowest cost tariff.");
                                    ratedLoad = selectLowestCostCarrier(ref results, load, carrierTariffsPivots);
                                    break;
                                case DAL.Utilities.AssignCarrierCalculationType.RecalcuateNoBFC:
                                    results.AddLog("Recalculate costs using current line haul but do not update the BFC.");
                                    Logger.Information("Recalculate costs using current line haul but do not update the BFC.");
                                    ratedLoad = recalculateUsingLineHaul(ref results, load, false);
                                    break;
                                case DAL.Utilities.AssignCarrierCalculationType.Recalculate:
                                    results.AddLog("Recalculate costs using current line haul.");
                                    Logger.Information("Recalculate costs using current line haul.");
                                    ratedLoad = recalculateUsingLineHaul(ref results, load);
                                    break;
                                case DAL.Utilities.AssignCarrierCalculationType.UpdateAssignedCarrier:
                                    results.AddLog(
                                        "Cannot Update the Assigned Carrier the functionality is not supported.");
                                    Logger.Warning("Cannot Update the Assigned Carrier the functionality is not supported.");
                                    results.Success = false;
                                    break;
                                case DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier:
                                    Logger.Information(
                                        "Update the carrier cost using the current tariff selected.");
                                    results.AddLog("Update the carrier cost using the current tariff selected.");
                                    ratedLoad = recalculateUsingCarrTarEquipControl(ref results, load, bookControl,
                                        CarrTarEquipControl, CarrTarEquipMatControl);
                                    break;
                            }

                            if (results.Success)
                            {

                                results.AddLog("Update the Book Tran Code on each of the Booking records.");
                                //check for carrier contact and save it to the database
                                //Modified By RHR on 02/21/2020 fixed bug where existing Carrier Contact information was replaced with default 
                                if (CarrContact == null)
                                {
                                    if (ratedLoad != null)
                                    {
                                        if (ratedLoad.LastStopBookRevenue.BookCarrierControl == carrierControl &&
                                            existingCarrierContactControl.HasValue &&
                                            existingCarrierContactControl.Value != 0)
                                        {
                                            CarrContact = NGLCarrierContData.GetDefaultContactForCarrier(carrierControl,
                                                existingCarrierContactControl
                                                    .Value); //Modified By RHR on 02/21/2020 fixed bug where existing Carrier Contact information was replaced with default 
                                        }
                                    }
                                }

                                if (CarrContact != null)
                                {
                                    setCarrierCont(CarrContact);
                                }



                                //DTO.CarrierCont CarrContact = null,
                                UpdateBookRevDefaultsBeforeSave(load, ref ratedLoad);
                                results.AddLog("Save the changes to the database.");
                                // Save the BookRevenues back to the DB.
                                //TODO:  manage save exceptions !!!!! RHR 6/9/14
                                if (ratedLoad != null)
                                {

                                    List<string> sFaultInfo = null;
                                    results.Success =
                                        ratedLoad.saveBookRevenuesWDetailsFiltered(ref sFaultInfo, OptmisticConcurrencyOn);

                                    Logger.Information("saveBookRevenuesWDetailsFiltered - Success: {Success}", results.Success);

                                    if (sFaultInfo != null && sFaultInfo.Count() > 0)
                                    {
                                        //add the error to the log
                                        results.AddLog(
                                            DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotSaveCarrierAssignment,
                                            sFaultInfo);
                                    }
                                    else
                                    {

                                    }

                                    if (results.Success)
                                    {
                                        results.AddLog("Success.");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "Something failed in CarTarBookRev");
                    throw;
                }
                catch (Exception ex)
                {
                    NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.assignCarrier");

                }


            }
            return results;

        }

        /// <summary>
        /// Assign a carrier and calculate the book revenue costs.
        /// </summary>
        /// <param name="bookControl"></param>
        /// <param name="carrierControl"></param>
        /// <param name="CalculationType"></param>
        /// <param name="CarrTarEquipMatControl"></param>
        /// <param name="CarrTarEquipControl"></param>
        /// <param name="prefered"></param>
        /// <param name="noLateDelivery"></param>
        /// <param name="validated"></param>
        /// <param name="optimizeByCapacity"></param>
        /// <param name="modeTypeControl"></param>
        /// <param name="tempType"></param>
        /// <param name="tariffTypeControl"></param>
        /// <param name="carrTarEquipMatClass"></param>
        /// <param name="carrTarEquipMatClassTypeControl"></param>
        /// <param name="carrTarEquipMatTarRateTypeControl"></param>
        /// <param name="agentControl"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="CarrContact">Optional carrier contact data used if provided for the carrier contact information</param>
        /// <param name="lRestrictedCarriers">Optional a list of carriers not to use when assigning the carrier. 
        /// If provided these carriers and all their tariffs will be ignored.
        /// This parameter is only used when the CalculationType is DAL.Utilities.AssignCarrierCalculationType.Normal.
        /// </param>
        /// <returns>
        /// NOTE:  uses the CarrTarEquipControl to return a list of pivot records if provided. 
        /// If CarrTarEquipMatControl is provided it will be used if the Class Type and Class Code match.  
        /// If Class Type and Class Code match do not match the system will select the lowest cost match available.
        /// If CarrTarEquipControl is not provided lowest cost carrier is selected based on other parameters.
        /// 
        /// </returns>
        /// <remarks>
        /// Modified By RHR on 02/21/2020 for v-8.2.1.004
        ///     fixed bug where existing Carrier Contact information was replaced with default 
        /// </remarks>
        public DTO.CarrierCostResults assignCarrier(int bookControl,
                                  int carrierControl = 0,
                                  DAL.Utilities.AssignCarrierCalculationType CalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal,
                                  int CarrTarEquipMatControl = 0,
                                  int CarrTarEquipControl = 0,
                                  bool prefered = true,
                                  bool noLateDelivery = false,
                                  bool validated = true,
                                  bool optimizeByCapacity = true,
                                  int modeTypeControl = 0,
                                  int tempType = 0,
                                  int tariffTypeControl = 0,
                                  string carrTarEquipMatClass = null,
                                  int carrTarEquipMatClassTypeControl = 0,
                                  int carrTarEquipMatTarRateTypeControl = 0,
                                  int agentControl = 0,
                                  int page = 1,
                                  int pagesize = 1000,
                                  DTO.CarrierCont CarrContact = null,
                                  List<int> lRestrictedCarriers = null)
        {
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();
            using (var operation = Logger.StartActivity("NGL.FM.CarTar.BookRev.assignCarrier - carrierControl: {carrierControl}, calculationType: {CalculationType}, CarrTarEquipControl: {CarrTarEquipControl}, prefered: {prefered}, noLateDelivery: {noLateDelivery}, validated: {validated}, optimizeByCapacity: {optimizeByCapacity}, modeTypeControl: {modeTypeControl}, tempType: {tempType}, tariffTypeControl: {tariffTypeControl}, carrTarEquipMatClass: {carrTarEquipMatClass}, carrTarEquipMatClassTypeControl: {carrTarEquipMatClassTypeControl}, carrTarEquipMatTarRateTypeControl: {carrTarEquipMatTarRateTypeControl}, agentControl: {agentControl}, page: {page}, pagesize: {pagesize}, CarrContact: {CarrContact}, lRestrictedCarriers: {lRestrictedCarriers}", carrierControl, CalculationType, CarrTarEquipControl, prefered, noLateDelivery, validated, optimizeByCapacity, modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass, carrTarEquipMatClassTypeControl, carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize, CarrContact, lRestrictedCarriers))
            {
                try
                {
                    results.AddLog("Read the booking records.");
                    // Get the list of BookRevenue objects and store them in a Load object.
                    Load load = new Load(bookControl, Parameters);
                    if (load.BookRevenues == null || load.BookRevenues.Count() < 1)
                    {
                        results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoOrdersFound);
                        return results;
                    }
                    // Modified By RHR on 02/21/2020 for v-8.2.1.004
                    int? existingCarrierContactControl = load.BookRevenues[0].BookCarrierContControl;
                    int existingCarrierControl = load.BookRevenues[0].BookCarrierControl;

                    results.AddLog("Validate the Book Tran Code.");
                    bool validLoad = load.validLoad(ref results, CalculationType); //updates the load's CalculationType property on success
                    if (validLoad)
                    {

                        // Rate results will be stored on a cloned ratedLoad object.
                        Load ratedLoad = null;
                        //if UpdateCarrier and CarrTarEquipControl = 0 or any BookCarrTarEquipControl <> CarrTarEquipControl 
                        //we cannot just update the carrier costs we need to use normal processing
                        if (CalculationType == DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier
                                &&
                                (
                                   CarrTarEquipControl == 0
                                   ||
                                   (
                                       load.BookRevenues != null
                                       && load.BookRevenues.Count() > 0
                                       && (load.BookRevenues.Any(x => x.BookCarrTarEquipControl != CarrTarEquipControl))
                                   )
                               )
                            )
                        {
                            CalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal;
                        }
                        // Perform the specified rating logic.
                        switch (CalculationType)
                        {
                            case DAL.Utilities.AssignCarrierCalculationType.Normal:
                                results.AddLog("Assign lowest cost carrier.");
                                Logger.Information("NGL.FM.CarTar.BookRev.assignCarrier - Assign lowest cost carrier");
                                // Get the Carrier Tariffs Pivot data using the filter data passed into this method.
                                DTO.CarrierTariffsPivot[] carrierTariffsPivots =
                                    CarrierTariffsPivotProcessorInstance.getCarrierTariffsPivotWithPrecision(ref results,
                                        bookControl, carrierControl, prefered, noLateDelivery, validated, optimizeByCapacity,
                                        modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass, carrTarEquipMatClassTypeControl,
                                        carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize);

                                if (carrierTariffsPivots == null || carrierTariffsPivots.Count() < 1)
                                {
                                    //Modified by RHR v-7.0.5.100 05/17/2016
                                    //added logic to change the message based on if we already have a carrier assigned
                                    if (carrierControl != 0)
                                    {
                                        results.AddLog("No valid tariffs found.  Check carrier qualifications, equipment capacity, tariff settings, and rates for the last stop");
                                    }
                                    else
                                    {
                                        results.AddLog("No tariffs found.");
                                    }
                                    results.Success = false;
                                    return results;        // There are no carriers to assign
                                }

                                if (lRestrictedCarriers != null && lRestrictedCarriers.Count() > 0)
                                {
                                    carrierTariffsPivots = (from d in carrierTariffsPivots where !lRestrictedCarriers.Contains(d.CarrierControl) select d).ToArray();
                                    if (carrierTariffsPivots == null || carrierTariffsPivots.Count() < 1)
                                    {
                                        //Modified by RHR v-7.0.5.100 05/17/2016
                                        //modified the message to indicate that carrier restrictions are being implemented                                
                                        results.AddLog("No tariffs found due to carrier restrictions.");
                                        results.Success = false;
                                        return results;        // There are no carriers to assign
                                    }
                                }

                                results.AddLog("Select the lowest cost tariff.");
                                ratedLoad = selectLowestCostCarrier(ref results, load, carrierTariffsPivots);
                                break;
                            case DAL.Utilities.AssignCarrierCalculationType.RecalcuateNoBFC:
                                Logger.Information("NGL.FM.CarTar.BookRev.assignCarrier - Recalculate costs using current line haul but do not update the BFC");
                                results.AddLog("Recalculate costs using current line haul but do not update the BFC.");
                                ratedLoad = recalculateUsingLineHaul(ref results, load, false);
                                break;
                            case DAL.Utilities.AssignCarrierCalculationType.Recalculate:
                                Logger.Information("NGL.FM.CarTar.BookRev.assignCarrier - Recalculate costs using current line haul");
                                results.AddLog("Recalculate costs using current line haul.");
                                ratedLoad = recalculateUsingLineHaul(ref results, load);
                                break;
                            case DAL.Utilities.AssignCarrierCalculationType.UpdateAssignedCarrier:
                                Logger.Warning("NGL.FM.CarTar.BookRev.assignCarrier - Cannot Update the Assigned Carrier the functionality is not supported.");
                                results.AddLog("Cannot Update the Assigned Carrier the functionality is not supported.");
                                results.Success = false;
                                break;
                            case DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier:
                                Logger.Information("NGL.FM.CarTar.BookRev.assignCarrier - Update the carrier cost using the current tariff selected");
                                results.AddLog("Update the carrier cost using the current tariff selected.");
                                ratedLoad = recalculateUsingCarrTarEquipControl(ref results, load, bookControl, CarrTarEquipControl, CarrTarEquipMatControl);
                                break;
                        }
                        if (results.Success)
                        {
                            Logger.Information("NGL.FM.CarTar.BookRev.assignCarrier - Update the Book Tran Code on each of the Booking records");
                            results.AddLog("Update the Book Tran Code on each of the Booking records.");
                            //check for carrier contact and save it to the database


                            //Modified By RHR on 02/21/2020 fixed bug where existing Carrier Contact information was replaced with default 
                            if (CarrContact == null)
                            {
                                if (ratedLoad != null)
                                {
                                    if (ratedLoad.LastStopBookRevenue.BookCarrierControl == existingCarrierControl && existingCarrierContactControl.HasValue && existingCarrierContactControl.Value != 0)
                                    {

                                        CarrContact = NGLCarrierContData.GetDefaultContactForCarrier(existingCarrierControl, existingCarrierContactControl.Value); //Modified By RHR on 02/21/2020 fixed bug where existing Carrier Contact information was replaced with default 
                                        Logger.Information("NGL.FM.CarTar.BookRev.assignCarrier - GetDefaultContactForCarrier - CarrierControl: {existingCarrierControl}, CarrierContactControl: {existingCarrierContactControl}", existingCarrierControl, existingCarrierContactControl);
                                    }
                                }
                            }
                            if (CarrContact != null) { setCarrierCont(CarrContact); }
                            Logger.Information("NGL.FM.CarTar.BookRev.assignCarrier - UpdateBookRevDefaultsBeforeSave");
                            UpdateBookRevDefaultsBeforeSave(load, ref ratedLoad);
                            results.AddLog("Save the changes to the database.");
                            // Save the BookRevenues back to the DB.
                            //TODO:  manage save exceptions !!!!! RHR 6/9/14
                            if (ratedLoad != null)
                            {

                                List<string> sFaultInfo = null;

                                results.Success = ratedLoad.saveBookRevenuesWDetailsFiltered(ref sFaultInfo);
                                Logger.Information("NGL.FM.CarTar.BookRev.assignCarrier - saveBookRevenuesWDetailsFiltered - Success: {Success}", results.Success);
                                if (sFaultInfo != null && sFaultInfo.Count() > 0)
                                {
                                    //add the error to the log
                                    Logger.Error("NGL.FM.CarTar.BookRev.assignCarrier - SQLFaultCannotSaveCarrierAssignment - sFaultInfo: {sFaultInfo}", sFaultInfo);
                                    results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotSaveCarrierAssignment, sFaultInfo);
                                }
                                else
                                {

                                }
                                if (results.Success) { results.AddLog("Success."); }
                            }
                        }
                    }
                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "NGL.FM.CarTar.BookRev.assignCarrier - FaultException");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "NGL.FM.CarTar.BookRev.assignCarrier - Exception");
                    NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.assignCarrier");

                }
            }
            return results;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookControl"></param>
        /// <param name="ShipcarrierControl"></param>
        /// <param name="CalculationType"></param>
        /// <param name="CarrTarEquipMatControl"></param>
        /// <param name="CarrTarEquipControl"></param>
        /// <param name="prefered"></param>
        /// <param name="noLateDelivery"></param>
        /// <param name="validated"></param>
        /// <param name="optimizeByCapacity"></param>
        /// <param name="modeTypeControl"></param>
        /// <param name="tempType"></param>
        /// <param name="tariffTypeControl"></param>
        /// <param name="carrTarEquipMatClass"></param>
        /// <param name="carrTarEquipMatClassTypeControl"></param>
        /// <param name="carrTarEquipMatTarRateTypeControl"></param>
        /// <param name="agentControl"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="CarrContact"></param>
        /// <param name="lRestrictedCarriers"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By RHR on 02/21/2020 for v-8.2.1.004
        ///     fixed bug where existing Carrier Contact information was replaced with default 
        /// </remarks>
        public DTO.CarrierCostResults assignShipCarrier(int bookControl,
                                  int ShipcarrierControl = 0,
                                  DAL.Utilities.AssignCarrierCalculationType CalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal,
                                  int CarrTarEquipMatControl = 0,
                                  int CarrTarEquipControl = 0,
                                  bool prefered = true,
                                  bool noLateDelivery = false,
                                  bool validated = true,
                                  bool optimizeByCapacity = true,
                                  int modeTypeControl = 0,
                                  int tempType = 0,
                                  int tariffTypeControl = 0,
                                  string carrTarEquipMatClass = null,
                                  int carrTarEquipMatClassTypeControl = 0,
                                  int carrTarEquipMatTarRateTypeControl = 0,
                                  int agentControl = 0,
                                  int page = 1,
                                  int pagesize = 1000,
                                  DTO.CarrierCont CarrContact = null,
                                  List<int> lRestrictedCarriers = null)
        {
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();

            try
            {
                results.AddLog("Read the booking records.");
                // Get the list of BookRevenue objects and store them in a Load object.
                Load load = new Load(bookControl, Parameters);
                if (load.BookRevenues == null || load.BookRevenues.Count() < 1)
                {
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoOrdersFound);
                    return results;
                }
                // Modified By RHR on 02/21/2020 for v-8.2.1.004
                int? existingCarrierContactControl = load.BookRevenues[0].BookCarrierContControl;
                int existingCarrierControl = load.BookRevenues[0].BookCarrierControl;

                results.AddLog("Validate the Book Tran Code.");
                bool validLoad = load.validLoad(ref results, CalculationType); //updates the load's CalculationType property on success
                if (validLoad)
                {

                    // Rate results will be stored on a cloned ratedLoad object.
                    Load ratedLoad = null;
                    //if UpdateCarrier and CarrTarEquipControl = 0 or any BookCarrTarEquipControl <> CarrTarEquipControl 
                    //we cannot just update the carrier costs we need to use normal processing
                    if (CalculationType == DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier
                            &&
                            (
                               CarrTarEquipControl == 0
                               ||
                               (
                                   load.BookRevenues != null
                                   && load.BookRevenues.Count() > 0
                                   && (load.BookRevenues.Any(x => x.BookCarrTarEquipControl != CarrTarEquipControl))
                               )
                           )
                        )
                    {
                        CalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal;
                    }
                    // Perform the specified rating logic.
                    switch (CalculationType)
                    {
                        case DAL.Utilities.AssignCarrierCalculationType.Normal:
                            results.AddLog("Assign lowest cost carrier.");
                            // Get the Carrier Tariffs Pivot data using the filter data passed into this method.
                            DTO.CarrierTariffsPivot[] carrierTariffsPivots =
                                CarrierTariffsPivotProcessorInstance.getCarrierTariffsPivotWithPrecision(ref results,
                                    bookControl, ShipcarrierControl, prefered, noLateDelivery, validated, optimizeByCapacity,
                                    modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass, carrTarEquipMatClassTypeControl,
                                    carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize);

                            if (carrierTariffsPivots == null || carrierTariffsPivots.Count() < 1)
                            {
                                //Modified by RHR v-7.0.5.100 05/17/2016
                                //added logic to change the message based on if an alternate shipping carrier is assigned
                                if (ShipcarrierControl != 0)
                                {
                                    results.AddLog("No valid tariffs found for the alternate shipping carrier.  Check carrier qualifications, equipment capacity, tariff settings, and rates for the last stop");
                                }
                                else
                                {
                                    results.AddLog("No tariffs found.");
                                }
                                results.Success = false;
                                return results;        // There are no carriers to assign
                            }

                            if (lRestrictedCarriers != null && lRestrictedCarriers.Count() > 0)
                            {
                                carrierTariffsPivots = (from d in carrierTariffsPivots where !lRestrictedCarriers.Contains(d.CarrierControl) select d).ToArray();
                                if (carrierTariffsPivots == null || carrierTariffsPivots.Count() < 1)
                                {
                                    //Modified by RHR v-7.0.5.100 05/17/2016
                                    //modified the message to indicate that carrier restrictions are being implemented                                
                                    results.AddLog("No tariffs found due to carrier restrictions.");
                                    results.Success = false;
                                    return results;        // There are no carriers to assign
                                }
                            }

                            results.AddLog("Select the lowest cost tariff.");
                            ratedLoad = selectLowestCostCarrier(ref results, load, carrierTariffsPivots);
                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.RecalcuateNoBFC:
                            results.AddLog("Recalculate costs using current line haul but do not update the BFC.");
                            ratedLoad = recalculateUsingLineHaul(ref results, load, false);
                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.Recalculate:
                            results.AddLog("Recalculate costs using current line haul.");
                            ratedLoad = recalculateUsingLineHaul(ref results, load);
                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.UpdateAssignedCarrier:
                            results.AddLog("Cannot Update the Assigned Carrier the functionality is not supported.");
                            results.Success = false;
                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier:
                            results.AddLog("Update the carrier cost using the current tariff selected.");
                            ratedLoad = recalculateUsingCarrTarEquipControl(ref results, load, bookControl, CarrTarEquipControl, CarrTarEquipMatControl);
                            break;
                    }
                    if (results.Success)
                    {

                        results.AddLog("Update the Book Tran Code on each of the Booking records.");
                        //check for carrier contact and save it to the database

                        //Modified By RHR on 02/21/2020 fixed bug where existing Carrier Contact information was replaced with default 
                        if (CarrContact == null)
                        {
                            if (ratedLoad != null)
                            {
                                if (ratedLoad.LastStopBookRevenue.BookCarrierControl == existingCarrierControl && existingCarrierContactControl.HasValue && existingCarrierContactControl.Value != 0)
                                {
                                    CarrContact = NGLCarrierContData.GetDefaultContactForCarrier(existingCarrierControl, existingCarrierContactControl.Value); //Modified By RHR on 02/21/2020 fixed bug where existing Carrier Contact information was replaced with default 
                                }
                            }
                        }
                        if (CarrContact != null) { setCarrierCont(CarrContact); }
                        UpdateBookRevDefaultsBeforeSave(load, ref ratedLoad);
                        results.AddLog("Save the changes to the database.");
                        // Save the BookRevenues back to the DB.
                        //TODO:  manage save exceptions !!!!! RHR 6/9/14
                        if (ratedLoad != null)
                        {

                            List<string> sFaultInfo = null;
                            results.Success = ratedLoad.saveBookRevenuesWDetailsFiltered(ref sFaultInfo);
                            if (sFaultInfo != null && sFaultInfo.Count() > 0)
                            {
                                //add the error to the log
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotSaveCarrierAssignment, sFaultInfo);
                            }
                            else
                            {

                            }
                            if (results.Success) { results.AddLog("Success."); }
                        }
                    }
                }
            }
            catch (FaultException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.assignCarrier");

            }
            return results;

        }




        /// <summary>
        /// Default estimated carriers by cost method.  
        /// For automated lowest cost carrier selection be sure to set the validated parameter to true.
        /// </summary>
        /// <param name="bookControl"></param>
        /// <param name="carrierControl"></param>
        /// <param name="prefered"></param>
        /// <param name="noLateDelivery"></param>
        /// <param name="validated"></param>
        /// <param name="optimizeByCapacity"></param>
        /// <param name="modeTypeControl"></param>
        /// <param name="tempType"></param>
        /// <param name="tariffTypeControl"></param>
        /// <param name="carrTarEquipMatClass"></param>
        /// <param name="carrTarEquipMatClassTypeControl"></param>
        /// <param name="carrTarEquipMatTarRateTypeControl"></param>
        /// <param name="agentControl"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns>
        /// 
        /// </returns>
        /// <remarks>
        /// 
        /// </remarks>
        public DTO.CarrierCostResults estimatedCarriersByCost(int bookControl,
            int carrierControl = 0,
            bool prefered = true,
            bool noLateDelivery = false,
            bool validated = false,
            bool optimizeByCapacity = true,
            int modeTypeControl = 0,
            int tempType = 0,
            int tariffTypeControl = 0,
            string carrTarEquipMatClass = null,
            int carrTarEquipMatClassTypeControl = 0,
            int carrTarEquipMatTarRateTypeControl = 0,
            int agentControl = 0,
            int page = 1,
            int pagesize = 1000)
        {
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();
            using (var operation = Logger.StartActivity("estimatedCarriersByCost(BookControl: {BookControl}, carrierControl: {CarrierControl}, prefered: {Preferred}, noLateDelivery: {NoLateDelivery}, validated: {Validated}, optimizeByCapacity: {OptimizeByCapacity}, modeTypeControl: {ModeTypeControl}, tempType: {TempType}, tariffTypeControl: {TariffTypeControl}, carrTarEquipMatClass: {CarrTarEquipMatClass}, carrTarEquipMatClassTypeControl: {CarrTarEquipMatClassTypeControl}, carrTarEquipMatTarRateTypeControl: {CarrTarEquipMatTarRateTypeControl}, agentControl: {AgentControl}, page: {Page}, pagesize: {PageSize}", bookControl, carrierControl, prefered, noLateDelivery, validated, optimizeByCapacity, modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass, carrTarEquipMatClassTypeControl, carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize))
            {
                try
                {

                    results.AddLog("Get a list of Book Revenue objects and store them in a Load object.");

                    Load load = new Load(bookControl, Parameters);

                    results.AddLog("Validate Book Tran Code.");
                    bool validLoad = load.validLoad(ref results, DAL.Utilities.AssignCarrierCalculationType.Normal);

                    Logger.Information("Is Load Valid: {validLoad}", validLoad);

                    if (validLoad)
                    {

                        results.AddLog("Get the Carrier Tariffs Pivot data using the selected filters.");


                        Logger.Information("Get the Carrier Tariffs Pivot data using the selected filters");

                        DTO.CarrierTariffsPivot[] carrierTariffsPivots =
                            CarrierTariffsPivotProcessorInstance.getCarrierTariffsPivotWithPrecision(ref results,
                                bookControl, carrierControl, prefered, noLateDelivery, validated, optimizeByCapacity,
                                modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass,
                                carrTarEquipMatClassTypeControl,
                                carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize);

                        Logger.Information("Checking if any Pivots were returned {PivotsReturned}", carrierTariffsPivots?.Any() ?? false);

                        if (!(carrierTariffsPivots?.Any()) ?? false)
                        {

                            //Modified by RHR v-7.0.5.100 05/17/2016
                            //added logic to change the message based on if we already have a carrier assigned
                            if (carrierControl != 0)
                            {
                                Logger.Warning(
                                    "No valid tariffs found.  Check carrier qualifications, equipment capacity, tariff settings, and rates for the last stop");
                                results.AddLog(
                                    "No valid tariffs found.  Check carrier qualifications, equipment capacity, tariff settings, and rates for the last stop");
                            }
                            else
                            {
                                Logger.Warning("No Tariffs Found: {@results}", results);
                                results.AddLog("No tariffs found.");
                            }

                            results.Success = false;

                            Logger.Warning("No Carriers Found", results);

                            return results; // There are no carriers to assign
                        }

                        results.AddLog("Estimate the cost for each tariff.");

                        _iCounterLog++;
                        Logger.Information(
                            $"Recursion Counter: {_iCounterLog} - calling estimatedCarriersByCost again", _iCounterLog);

                        results.Success = estimatedCarriersByCost(ref results, load, carrierTariffsPivots);

                        if (results.Success)
                        {
                            Logger.Information("Success {CarriersByCost} tariff rates are available.", results.CarriersByCost.Count);
                            results.AddLog("Success " + results.CarriersByCost.Count.ToString() +
                                           " tariff rates are available.");
                        }
                    }

                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.estimatedCarriersByCost - Fault Exception");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.estimatedCarriersByCost - Unexpected Error");
                }

                Logger.Information("CarTar.BookRev.estimatedCarriersByCost - Complete", results);
                return results;
            }

        }

        private int _iCounterLog = 0;

        /// <summary>
        /// Overloaded estimate carriers by cost method. Expects an array of bookrevenue objects.
        /// For automated lowest cost carrier selection be sure to set the validated parameter to true.
        /// </summary>
        /// <param name="books"></param>
        /// <param name="carrierControl"></param>
        /// <param name="prefered"></param>
        /// <param name="noLateDelivery"></param>
        /// <param name="validated"></param>
        /// <param name="optimizeByCapacity"></param>
        /// <param name="modeTypeControl"></param>
        /// <param name="tempType"></param>
        /// <param name="tariffTypeControl"></param>
        /// <param name="carrTarEquipMatClass"></param>
        /// <param name="carrTarEquipMatClassTypeControl"></param>
        /// <param name="carrTarEquipMatTarRateTypeControl"></param>
        /// <param name="agentControl"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR v-7.0.5.110 9/6/2016 expected to be used by Dynamics TMS 365
        /// </remarks>
        public DTO.CarrierCostResults estimatedCarriersByCost(DTO.BookRevenue[] books,
                                                          bool inBound = false,
                                                          int carrierControl = 0,
                                                          bool prefered = true,
                                                          bool noLateDelivery = false,
                                                          bool validated = false,
                                                          bool optimizeByCapacity = true,
                                                          int modeTypeControl = 0,
                                                          int tempType = 0,
                                                          int tariffTypeControl = 0,
                                                          string carrTarEquipMatClass = null,
                                                          int carrTarEquipMatClassTypeControl = 0,
                                                          int carrTarEquipMatTarRateTypeControl = 0,
                                                          int agentControl = 0,
                                                          int page = 1,
                                                          int pagesize = 1000,
                                                          int skip = 0,
                                                          int take = 0)
        {
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();
            try
            {
                Logger.Information("CarTar.BookRev.estimatedCarriersByCost - Start");
                results.AddLog("Get a list of Book Revenue objects and store them in a Load object.");
                Load load = new Load(books, Parameters);

                results.AddLog("Validate Book Tran Code.");
                bool validLoad = load.validLoad(ref results, DAL.Utilities.AssignCarrierCalculationType.Normal);
                if (validLoad)
                {

                    results.AddLog("Get the Carrier Tariffs Pivot data using the selected filters.");
                    //Modified by RHR v-7.0.5.110 added logic to use rate shop procedure.
                    //Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                    DTO.CarrierTariffsPivot[] carrierTariffsPivots = CarrierTariffsPivotProcessorInstance.getCarrierTariffsPivot(ref results,
                        load.RatedBookRevenue.BookCustCompControl,
                        inBound == true ? load.RatedBookRevenue.BookOrigCountry : load.RatedBookRevenue.BookDestCountry,
                        inBound == true ? load.RatedBookRevenue.BookOrigState : load.RatedBookRevenue.BookDestState,
                        inBound == true ? load.RatedBookRevenue.BookOrigCity : load.RatedBookRevenue.BookDestCity,
                        inBound == true ? load.RatedBookRevenue.BookOrigZip : load.RatedBookRevenue.BookDestZip,
                        load.RatedBookRevenue.BookModeTypeControl,
                        load.TotalWgt,
                        load.TotalCases,
                        load.TotalPL,
                        load.TotalCube,
                        load.RatedBookRevenue.BookDateLoad ?? DateTime.Now,
                        inBound,
                        "",//temp should be a string bookloadcom
                        0,
                       carrierControl,
                        prefered,
                        noLateDelivery,
                        validated,
                        optimizeByCapacity,
                        tempType: tempType,
                        tariffTypeControl: tariffTypeControl,
                        carrTarEquipMatClass: carrTarEquipMatClass,
                        carrTarEquipMatClassTypeControl: carrTarEquipMatClassTypeControl,
                        carrTarEquipMatTarRateTypeControl: carrTarEquipMatTarRateTypeControl,
                        page: page,
                        pagesize: pagesize,
                        OrigZip: (inBound == true ? load.RatedBookRevenue.BookDestZip : load.RatedBookRevenue.BookOrigZip));


                    if (carrierTariffsPivots == null || carrierTariffsPivots.Count() < 1)
                    {
                        //Modified by RHR v-7.0.5.100 05/17/2016
                        //added logic to change the message based on if we already have a carrier assigned
                        if (carrierControl != 0)
                        {
                            results.AddLog("No valid tariffs found.  Check carrier qualifications, equipment capacity, tariff settings, and rates for the last stop");
                        }
                        else
                        {
                            results.AddLog("No tariffs found.");
                        }
                        results.Success = false;
                        return results;        // There are no carriers to assign
                    }
                    results.AddLog("Found " + carrierTariffsPivots.Count().ToString() + " possible carrier tariff rates.");
                    results.AddLog("Estimate the cost for each tariff.");
                    results.Success = estimatedCarriersByCost(ref results, load, carrierTariffsPivots);

                    if (results.Success) { results.AddLog("Success " + results.CarriersByCost.Count.ToString() + " tariff rates are available."); }
                }
            }
            catch (FaultException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.estimatedCarriersByCost");
            }

            return results;
        }

        /// <summary>
        /// for rate shop get the list of carriers and their rates
        /// </summary>
        /// <param name="rateShop"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        /// </remarks>
        public DTO.CarrierCostResults estimatedCarriersByCost(DTO.RateShop rateShop)
        {
            Logger.Information("CarTar.BookRev.estimatedCarriersByCost - RateShop");
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();

            using (var operation = Logger.StartActivity("estimatedCarriersByCost(RateShop: {@RateShop})", rateShop))
            {
                try
                {
                    if (rateShop.BookRevs == null)
                    {
                        Logger.Information("CarTar.BookRev.estimatedCarriersByCost - No Orders Found");
                        results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoOrdersFound);
                        operation.Complete();
                        return results;
                    }

                    ///Get the list of BookRevenue objects and store them in a Load object.
                    Logger.Information(
                        "RateShop - Get a list of Book Revenue objects and store them in a Load object.");

                    Load load = new Load(rateShop.BookRevs.ToArray(), Parameters);


                    results.AddLog("Validate the Book Tran Code.");
                 
                    bool validLoad = load.validLoad(ref results,
                        DAL.Utilities.AssignCarrierCalculationType.RateShopOnly);
                    
                    bool validRateShopLoad = load.ValidRateShopLoad(ref results, rateShop);

                    Logger.Information(
                        "RateShop - Valid Load: {validLoad}, Valid Rate Shop Load: {validRateShopLoad}",
                        validLoad, validRateShopLoad);

                    if (validLoad && validRateShopLoad)
                    {
                        // Removed by RHR 10/30/2014 not sure why we enter 100 as the default
                        //if (!string.IsNullOrWhiteSpace(rateShop.CarrTarEquipMatClass)) { Util.RateShopClassCode = rateShop.CarrTarEquipMatClass; }
                        //Modified by RHR 10/30/14 added class data for rate shoping to load object
                        load.RateShopMatClass = rateShop.CarrTarEquipMatClass;
                        load.RateShopMatClassTypeControl = rateShop.CarrTarEquipMatClassTypeControl;

                        // Get the Carrier Tariffs Pivot data using the filter data passed into this method.
                        DTO.BookRevenue rev = rateShop.BookRevs[0];
                        string country = "";
                        string city = "";
                        string state = "";
                        string zip = "";
                        string OrigZip = "";
                        int compControl = 0;
                        if (rateShop.Outbound)
                        {
                            Logger.Information(
                                "Outbound, setting compControl to {OrigBookControl}, city: {DestCity}, state: {DestState}, zip: {DestZip}, origZip: {OrigZip}",
                                rev.BookOrigCompControl, rev.BookDestCity, rev.BookDestState, rev.BookDestZip,
                                rev.BookOrigZip);
                            compControl = rev.BookOrigCompControl; //not sure about this yet?
                            city = rev.BookDestCity;
                            country = rev.BookDestCountry;
                            state = rev.BookDestState;
                            zip = rev.BookDestZip;
                            OrigZip = rev.BookOrigZip;
                        }
                        else
                        {
                            Logger.Information(
                                "Inbound, setting compControl to {DestBookControl}, city: {OrigCity}, state: {OrigState}, zip: {OrigZip}, origZip: {OrigZip}",
                                rev.BookDestCompControl, rev.BookOrigCity, rev.BookOrigState, rev.BookOrigZip,
                                rev.BookDestZip);
                            compControl = rev.BookDestCompControl; //not sure about this yet?
                            city = rev.BookOrigCity;
                            country = rev.BookOrigCountry;
                            state = rev.BookOrigState;
                            zip = rev.BookOrigZip;
                            OrigZip = rev.BookDestZip;
                        }

                        DateTime bookdateload = DateTime.Today;
                        if (rev.BookDateLoad != null && rev.BookDateLoad.HasValue)
                        {
                            Logger.Information(
                                "BookDateLoad {BookDateLoad} is not null and has value",
                                rev.BookDateLoad.Value);
                            bookdateload = rev.BookDateLoad.Value;
                        }

                        //add the fees to the load object.
                        rateShop.BookFees?.ForEach(bookFee => Logger.Information(
                            " Adding Fee {BookFeesCaption} ({BookFeesAccessorialCode}) to Load Object with value {BookFeesValue}",
                            bookFee.BookFeesCaption, bookFee.BookFeesAccessorialCode, bookFee.BookFeesValue));

                        load.ProfileFees = rateShop.BookFees;

                        Logger.Information(
                            "CarTar.BookRev.estimatedCarriersByCost - Get the Carrier Tariffs Pivot data using the selected filters. compControl={compControl}, country={country}, city={city}, state={state}, zip={zip}, modeTypeControl={modeTypeControl}, BookTotalWgt={BookTotalWgt}, BookTotalCases={BookTotalCases}, BookTotalPL={BookTotalPL}, BookTotalCube={BookTotalCube}, CarrierControl={CarrierControl}, CarrTarEquipMatClass={CarrTarEquipMatClass}, CarrTarEquipMatClassTypeControl={CarrTarEquipMatClassTypeControl}, CarrTarEquipMatTarRateTypeControl={CarrTarEquipMatTarRateTypeControl}",
                            compControl,
                            country,
                            city,
                            state,
                            zip,
                            rev.BookModeTypeControl,
                            rev.BookTotalWgt,
                            rev.BookTotalCases,
                            rev.BookTotalPL,
                            rev.BookTotalCube,
                            rateShop.CarrierControl,
                            rateShop.CarrTarEquipMatClass,
                            rateShop.CarrTarEquipMatClassTypeControl,
                            rateShop.CarrTarEquipMatTarRateTypeControl);


                        DTO.CarrierTariffsPivot[] carrierTariffsPivots =
                            CarrierTariffsPivotProcessorInstance.getCarrierTariffsPivot(ref results,
                                compControl,
                                country,
                                state,
                                city,
                                zip,
                                rev.BookModeTypeControl,
                                rev.BookTotalWgt,
                                rev.BookTotalCases,
                                rev.BookTotalPL,
                                rev.BookTotalCube,
                                bookdateload,
                                !rateShop.Outbound,
                                "", //temp should be a string bookloadcom
                                0,
                                rateShop.CarrierControl,
                                rateShop.Prefered,
                                rateShop.NoLateDelivery,
                                rateShop.Validated,
                                rateShop.OptimizeByCapacity,
                                tempType: rateShop.TempType,
                                tariffTypeControl: 0,
                                carrTarEquipMatClass: rateShop.CarrTarEquipMatClass,
                                carrTarEquipMatClassTypeControl: rateShop.CarrTarEquipMatClassTypeControl,
                                carrTarEquipMatTarRateTypeControl: rateShop.CarrTarEquipMatTarRateTypeControl,
                                page: rateShop.Page,
                                pagesize: rateShop.PageSize,
                                OrigZip: OrigZip);

                        Logger.Information(
                            "Carrier Tariffs Pivot Data:, {@carrierTariffsPivots}", carrierTariffsPivots);

                        if (carrierTariffsPivots == null)
                        {
                            Logger.Information("No CarrierTariffPivots Found");
                            results.AddLog("No tariffs found.");
                            results.Success = false;
                            operation.Complete();
                            return results; // There are no carriers to assign
                        }

                        results.AddLog("Estimate cost for each tariff found.");

                        results.Success = estimatedCarriersByCost(ref results, load, carrierTariffsPivots);

                        if (results.Success)
                        {
                            Logger.Information("CarTar.BookRev.estimatedCarriersByCost - Success");
                            results.AddLog("Success.");
                        }
                    }
                    else
                    {
                        //show error messages.
                        Logger.Information(
                            "Cannot estimate carrier costs the load information may not be valid");
                        results.AddLog("Cannot estimate carrier costs the load information may not be valid.");
                    }
                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.estimatedCarriersByCost - Fault Exception");
                    //throw;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.estimatedCarriersByCost - Unexpected Error");
                    //NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.estimatedCarriersByCost");
                }
            }

            return results;
        }

        private void validateBFCTolerances(DTO.CarrierCostResults results, DTO.BookRevenue rev)
        {
            using (var operation = Logger.StartActivity("validateBFCTolerances(results: {@results}, rev: {@rev}", results, rev))
            {
                decimal BFCTolerance = 1;
                decimal BFCHIgh = 0;
                BFCHIgh = (decimal)NGLCompParameterData.GetParValue("BFCHighValue", rev.BookCustCompControl);
                BFCTolerance = (decimal)NGLCompParameterData.GetParValue("BFCTolerance", rev.BookCustCompControl) / 100;


                Logger.Information("validateBFCTolerances - BFCTolerance: {BFCTolerance}, BFCHigh: {BFCHIgh}", BFCTolerance, BFCHIgh);


                if ((rev.BookRevBilledBFC > (BFCTolerance * (decimal)rev.BookRevCarrierCost)) && (rev.BookRevCarrierCost != 0))
                {
                    Logger.Information("CarTar.BookRev.validateBFCTolerances - Billed BFC exceeds tolerance");
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.MSGRevExceedsTolerance, new List<string>(), true);
                }
                else if (rev.BookRevBilledBFC > BFCHIgh)
                {
                    Logger.Information("CarTar.BookRev.validateBFCTolerances - Billed BFC exceeds high value");
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.MSGBFCHighValueWarning, new List<string>(), true);
                }
            }
        }

        public DTO.CarrierCostResults AdjustBFCNoSave(DTO.AdjustBFC adjustBFC)
        {
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();
            using (Logger.StartActivity("AdjustBFCNoSave(AdjustBFC: {AdjustBFC}", adjustBFC))
            {
                try
                {
                    //DTO.BookRevenue[] revs = null;
                    if (adjustBFC.BookRevs != null && adjustBFC.BookRevs.Count > 0)
                    {
                        Load load = new Load(adjustBFC.BookRevs.ToArray(), Parameters);

                        //do bfc allocation manual user configured in client.
                        load.allocateBFCCostsByAllocationMode(adjustBFC.TotalBFC, adjustBFC.AllocationBFCFormula, adjustBFC.AutoCalculateBFC);

                        //unLock All Costs before recalculating
                        //Assign carrier to bookrecords
                        foreach (DTO.BookRevenue rev in load.BookRevenues)
                        {
                            //TODO add fields to store bfc calculation fields so they are saved for next time.
                            rev.BookLockBFCCost = false;
                        }
                        decimal totLineHaul = load.sumTotalLineHaul();
                        if (totLineHaul > 0)
                        {
                            results.Success = this.assignCarrierNoSave(ref results, load.BookRevenues, 0, DAL.Utilities.AssignCarrierCalculationType.RecalcuateNoBFC, adjustBFC.AutoCalculateBFC);
                        }
                        else
                        {
                            results.Success = true;
                            results.BookRevs = load.BookRevenues.ToList();
                            //do nothing the bfc values should already set to 0.
                        }

                        if (results.Success)
                        {
                            foreach (DTO.BookRevenue rev in results.BookRevs)
                            {
                                rev.BookLockBFCCost = true;
                                validateBFCTolerances(results, rev);
                                //TODO add fields to store bfc calculation fields so they are saved for next time.
                                //also if line haul is 0, make sure the BFC values are also 0.
                                if (totLineHaul == 0)
                                {
                                    rev.BookTotalBFC = 0;
                                    rev.BookRevBilledBFC = 0;
                                }
                            }
                        }
                    }
                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.AdjustBFC - Fault Exception");
                    throw;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.AdjustBFC - Unexpected Error");
                    NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.AdjustBFC");
                }
            }
            return results;
        }

        /// <summary>
        /// To do a spot rate, the user needs to know an estimate before saving so all calculations do not save in this method.
        /// We remove the fees then add the order fees that user passes in then allocate the line haul, then assign carrier and recalculate
        /// </summary>
        /// <param name="inBookRevs"></param>
        /// <param name="allocation"></param>
        /// <param name="userLineHaulCost"></param>
        /// <param name="inBookFees"></param>
        /// <returns>
        /// Modified by RHR v-7.0.5.100 5/6/2016 we no longer reset the TranCode we use the current value
        /// this allows for updates to existing loads; 
        /// Also: We now only reset to N Status if the SHID has not been assigned
        /// </returns>
        public DTO.CarrierCostResults DoSpotRateNoSave(DTO.SpotRate spotRate)
        {

            DTO.CarrierCostResults results = new DTO.CarrierCostResults();
            using (var operation = Logger.StartActivity("DoSpotRateNoSave(SpotRate: {@SpotRate}", spotRate))
            {
                try
                {
                    //DTO.BookRevenue[] revs = null;
                    if (spotRate.BookRevs != null)
                    {
                        Load load = new Load(spotRate.BookRevs.ToArray(), Parameters);
                        //Modified by RHR v-7.0.5.100 5/10/2016
                        //We now only reset to N Status if the SHID has not been assigned
                        //this supports the ability to enter a negotiated rate on an existing load
                        if (string.IsNullOrWhiteSpace(spotRate.BookRevs[0].BookSHID))
                        {
                            Logger.Information("CarTar.BookRev.DoSpotRateNoSave - ResetToNStatus");
                            load.ResetToNStatus();
                        }

                        Logger.Information("DoSpotRateNoSave - Manage Spot Rate Fees");
                        load = manageSpotRateFees(load, spotRate);
                        //do line haul calcs here.
                        Logger.Information("DoSpotRateNoSave - Calculate Line Haul {Formula}", spotRate.allocationFormula.TarBracketTypeName);
                        load.calculateBookRevLineHaul(spotRate.totalLineHaulCost, spotRate.allocationFormula);
                        //do bfc allocation manual user configured in client.
                        Logger.Information("DoSpotRateNoSave - Allocate BFC Costs");
                        load.allocateBFCCostsByAllocationMode(spotRate.TotalBFC, spotRate.AllocationBFCFormula, spotRate.AutoCalculateBFC);

                        //unLock All Costs before recalculating
                        //Assign carrier to bookrecords
                        //assign new spot rate fields.
                        foreach (DTO.BookRevenue rev in load.BookRevenues)
                        {
                            Logger.Information("DoSpotRateNoSave - Assign Spot Rate Fields to BookRevenue[{BookControl}]", rev.BookControl);
                            rev.BookLockAllCosts = false;
                            rev.BookLockBFCCost = false;
                            rev.BookCarrierControl = spotRate.CarrierControl;
                            rev.BookSpotRateAllocationFormula = spotRate.allocationFormula.TarBracketTypeControl;
                            rev.BookSpotRateAutoCalcBFC = spotRate.AutoCalculateBFC;
                            rev.BookSpotRateBFCAllocationFormula = spotRate.AllocationBFCFormula.TarBracketTypeControl;
                            rev.BookSpotRateTotalUnallocatedBFC = spotRate.TotalBFC;
                            rev.BookSpotRateTotalUnallocatedLineHaul = spotRate.totalLineHaulCost;
                            rev.BookSpotRateUseCarrierFuelAddendum = spotRate.UseCarrierFuelAddendum;

                        }
                        //now calculate if the linehaul if greater than 0
                        if (spotRate.totalLineHaulCost > 0)
                        {
                            Logger.Information("Total line haul cost is greater than 0.  Assign Carrier.");
                            //use the tariff engine when linehaul > 0
                            results.Success = this.assignCarrierNoSave(ref results, load.BookRevenues, spotRate.CarrierControl, DAL.Utilities.AssignCarrierCalculationType.RecalcuateSpotRate, spotRate.AutoCalculateBFC);
                        }
                        else
                        {   //zero cost carrier, just set it successfull.
                            results.BookRevs = load.BookRevenues.ToList();
                            results.Success = true;
                            Logger.Information("Total line haul cost is 0.  Zero Cost Carrier set to sucessful.");
                            //Need to set TranCode to P status because we skip the tariff engine. 
                            //do this in the loop below.                        
                        }

                        //assign the Revenue Reason Code
                        if (results.Success)
                        {
                            //close out the process with a final loop.
                            foreach (DTO.BookRevenue rev in results.BookRevs)
                            {
                                rev.BookLockAllCosts = spotRate.LockAllCost;
                                rev.BookLockBFCCost = spotRate.LockBFCCost;
                                rev.BookRevNegRevenue = spotRate.BookRevNegRevenueValue;
                                validateBFCTolerances(results, rev);
                                //Modified by RHR v-7.0.5.100 5/6/2016 to inlcude logic
                                //previously executed by the ResetToNStatus call
                                //reset Tariff Information
                                rev.BookCarrTarControl = 0;
                                rev.BookCarrTarEquipControl = 0;
                                rev.BookCarrTarEquipMatControl = 0;
                                rev.BookCarrTarEquipMatDetControl = 0;
                                rev.BookCarrTarEquipMatDetID = 0;
                                rev.BookCarrTarEquipMatDetValue = 0;
                                rev.BookCarrTarEquipMatName = null;
                                rev.BookCarrTarEquipName = null;
                                rev.BookCarrTarName = null;
                                rev.BookCarrTarRevisionNumber = 0;
                                rev.BookCarrTruckControl = 0;
                                //tariff engine fields
                                rev.BookBestDeficitCost = 0;
                                rev.BookBestDeficitWeight = 0;
                                rev.BookBestDeficitWeightBreak = 0;
                                rev.BookRatedWeightBreak = 0;
                                rev.BookWgtAdjCost = 0;
                                rev.BookWgtAdjWeight = 0;
                                rev.BookWgtAdjWeightBreak = 0;
                                rev.BookBilledLoadWeight = 0;
                                rev.BookMinAdjustedLoadWeight = 0;
                                rev.BookSummedClassWeight = 0;
                                rev.BookWgtRoundingVariance = 0;
                                rev.BookHeaviestClass = "";
                                rev.BookAcutalHeaviestClassWeight = 0;
                                rev.BookRevDiscountRate = 0;
                                rev.BookRevDiscountMin = 0;
                                //when the line haul is 0, we skip the tariff engine.
                                //which means we have to set everything manually including the TranCode.
                                //NOTE: RHR 5/20/2016  It is not clear why totalLineHaulCost would be zero
                                //      on a spot rate at this point; Paul M. designed this logic so we need
                                //      to be careful about the actions taken based on this condition.
                                if (spotRate.totalLineHaulCost == 0)//make sure if the bfc are 0 if line haul is 0. and we assign to P status
                                {
                                    Logger.Information("Total line haul cost is 0.  Set BFC to 0 and TranCode to P.");
                                    rev.BookTotalBFC = 0;
                                    rev.BookRevBilledBFC = 0;
                                    //Modified by RHR v-7.0.5.100 5/6/2016 we no longer reset the 
                                    //TranCode to P we use the current value unless it is N
                                    //then we set it to P
                                    //this allows for updates to existing loads
                                    if (rev.BookTranCode == "N") { rev.BookTranCode = "P"; }
                                    //rev.BookTranCode = "P";

                                }
                            }
                        }
                        Logger.Information("Set DecParameter to the average fuel price {AvgFuelPrice}.", spotRate.AvgFuelPrice);
                        results.DecParameter = spotRate.AvgFuelPrice;
                    }
                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.DoSpotRateNoSave - Fault Exception");
                    //throw;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.DoSpotRateNoSave - Unexpected Error");
                    //NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.DoSpotRate");
                }
            }
            return results;
        }

        /// <summary>
        /// To do a spot rate, the user needs to know an estimate before saving so all calculations do not save in this method.
        /// We remove the fees then add the order fees that user passes in then allocate the line haul, then assign carrier and recalculate
        /// </summary>
        /// <param name="inBookRevs"></param>
        /// <param name="allocation"></param>
        /// <param name="userLineHaulCost"></param>
        /// <param name="inBookFees"></param>
        /// <returns>
        /// Modified by RHR v-7.0.5.100 5/6/2016 we no longer reset the TranCode we use the current value
        /// this allows for updates to existing loads; 
        /// Also: We now only reset to N Status if the SHID has not been assigned
        /// </returns>
        /// <remarks>
        /// Added by LVV On 2/17/17 For v-8.0 Next Stop
        /// </remarks>
        public DTO.CarrierCostResults DoSpotRateSave(DTO.SpotRate spotRate, DTO.CarrierCont CarrContact)
        {
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();
            using (var operation = Logger.StartActivity("DoSpotRateSave(SpotRate: {@SpotRate}, CarrContact: {@CarrContact})", spotRate, CarrContact))
            {


                try
                {
                    //DTO.BookRevenue[] revs = null;
                    if (spotRate.BookRevs != null)
                    {
                        Load load = new Load(spotRate.BookRevs.ToArray(), Parameters);
                        //Modified by RHR v-7.0.5.100 5/10/2016
                        //We now only reset to N Status if the SHID has not been assigned
                        //this supports the ability to enter a negotiated rate on an existing load
                        if (string.IsNullOrWhiteSpace(spotRate.BookRevs[0].BookSHID)) { load.ResetToNStatus(); }

                        load = manageSpotRateFees(load, spotRate);
                        //do line haul calcs here.
                        load.calculateBookRevLineHaul(spotRate.totalLineHaulCost, spotRate.allocationFormula);
                        //do bfc allocation manual user configured in client.
                        load.allocateBFCCostsByAllocationMode(spotRate.TotalBFC, spotRate.AllocationBFCFormula, spotRate.AutoCalculateBFC);

                        //unLock All Costs before recalculating
                        //Assign carrier to bookrecords
                        //assign new spot rate fields.
                        foreach (DTO.BookRevenue rev in load.BookRevenues)
                        {
                            rev.BookLockAllCosts = false;
                            rev.BookLockBFCCost = false;
                            rev.BookCarrierControl = spotRate.CarrierControl;
                            rev.BookSpotRateAllocationFormula = spotRate.allocationFormula.TarBracketTypeControl;
                            rev.BookSpotRateAutoCalcBFC = spotRate.AutoCalculateBFC;
                            rev.BookSpotRateBFCAllocationFormula = spotRate.AllocationBFCFormula.TarBracketTypeControl;
                            rev.BookSpotRateTotalUnallocatedBFC = spotRate.TotalBFC;
                            rev.BookSpotRateTotalUnallocatedLineHaul = spotRate.totalLineHaulCost;
                            rev.BookSpotRateUseCarrierFuelAddendum = spotRate.UseCarrierFuelAddendum;

                        }
                        //now calculate if the linehaul if greater than 0
                        if (spotRate.totalLineHaulCost > 0)
                        {
                            //use the tariff engine when linehaul > 0
                            results.Success = this.assignCarrierNoSave(ref results, load.BookRevenues, spotRate.CarrierControl, DAL.Utilities.AssignCarrierCalculationType.RecalcuateSpotRate, spotRate.AutoCalculateBFC);
                        }
                        else
                        {   //zero cost carrier, just set it successfull.
                            results.BookRevs = load.BookRevenues.ToList();
                            results.Success = true;
                            //Need to set TranCode to P status because we skip the tariff engine. 
                            //do this in the loop below.                        
                        }

                        //assign the Revenue Reason Code
                        if (results.Success)
                        {
                            //close out the process with a final loop.
                            foreach (DTO.BookRevenue rev in results.BookRevs)
                            {
                                rev.BookLockAllCosts = spotRate.LockAllCost;
                                rev.BookLockBFCCost = spotRate.LockBFCCost;
                                rev.BookRevNegRevenue = spotRate.BookRevNegRevenueValue;
                                validateBFCTolerances(results, rev);
                                //Modified by RHR v-7.0.5.100 5/6/2016 to inlcude logic
                                //previously executed by the ResetToNStatus call
                                //reset Tariff Information
                                rev.BookCarrTarControl = 0;
                                rev.BookCarrTarEquipControl = 0;
                                rev.BookCarrTarEquipMatControl = 0;
                                rev.BookCarrTarEquipMatDetControl = 0;
                                rev.BookCarrTarEquipMatDetID = 0;
                                rev.BookCarrTarEquipMatDetValue = 0;
                                if (!spotRate.FromAPI)
                                {
                                    rev.BookCarrTarEquipMatName = null;
                                    rev.BookCarrTarEquipName = null;
                                }



                                //rev.BookCarrTarName = null;
                                rev.BookCarrTarRevisionNumber = 0;
                                rev.BookCarrTruckControl = 0;
                                //tariff engine fields
                                rev.BookBestDeficitCost = 0;
                                rev.BookBestDeficitWeight = 0;
                                rev.BookBestDeficitWeightBreak = 0;
                                rev.BookRatedWeightBreak = 0;
                                rev.BookWgtAdjCost = 0;
                                rev.BookWgtAdjWeight = 0;
                                rev.BookWgtAdjWeightBreak = 0;
                                rev.BookBilledLoadWeight = 0;
                                rev.BookMinAdjustedLoadWeight = 0;
                                rev.BookSummedClassWeight = 0;
                                rev.BookWgtRoundingVariance = 0;
                                rev.BookHeaviestClass = "";
                                rev.BookAcutalHeaviestClassWeight = 0;
                                rev.BookRevDiscountRate = 0;
                                rev.BookRevDiscountMin = 0;
                                //when the line haul is 0, we skip the tariff engine.
                                //which means we have to set everything manually including the TranCode.
                                //NOTE: RHR 5/20/2016  It is not clear why totalLineHaulCost would be zero
                                //      on a spot rate at this point; Paul M. designed this logic so we need
                                //      to be careful about the actions taken based on this condition.
                                if (spotRate.totalLineHaulCost == 0)//make sure if the bfc are 0 if line haul is 0. and we assign to P status
                                {
                                    rev.BookTotalBFC = 0;
                                    rev.BookRevBilledBFC = 0;
                                    //Modified by RHR v-7.0.5.100 5/6/2016 we no longer reset the 
                                    //TranCode to P we use the current value unless it is N
                                    //then we set it to P
                                    //this allows for updates to existing loads
                                    if (rev.BookTranCode == "N") { rev.BookTranCode = "P"; }
                                    //rev.BookTranCode = "P";

                                }
                            }
                            //Add Save code here
                            //check for carrier contact and save it to the database
                            if (CarrContact != null) { setCarrierCont(CarrContact); }
                            load.BookRevenues = results.BookRevs.ToArray();
                            // Save the BookRevenues back to the DB.
                            //TODO:  manage save exceptions !!!!! RHR 6/9/14
                            if (load != null)
                            {
                                List<string> sFaultInfo = null;
                                results.Success = load.saveBookRevenuesWDetailsFiltered(ref sFaultInfo, false);
                                if (sFaultInfo != null && sFaultInfo.Count() > 0)
                                {
                                    //add the error to the log
                                    results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotSaveCarrierAssignment, sFaultInfo);
                                }
                                else
                                {

                                }
                                if (results.Success) { results.AddLog("Success."); }
                            }
                        }
                        results.DecParameter = spotRate.AvgFuelPrice;
                    }
                }
                catch (FaultException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.DoSpotRateSave");
                }
            }
            return results;
        }

        /// <summary>
        /// allocates linehaul cost based on allocation method
        /// </summary>
        /// <param name="inBookRevs"></param>
        /// <returns>
        /// this method may not be used
        /// </returns>
        public DTO.BookRevenue[] allocateLineHaulCost(DTO.BookRevenue[] inBookRevs,
            DTO.tblTarBracketType allocation,
            decimal userLineHaulCost)
        {

            using (Logger.StartActivity("allocateLineHaulCost(BookRevs: {@BookRevs}, Allocation: {@Allocation}, UserLineHaulCost: {UserLineHaulCost})", inBookRevs, allocation, userLineHaulCost))
            {

                try
                {
                    Logger.Information("CarTar.BookRev.allocateLineHaulCost - Load Object Created");
                    Load load = new Load(inBookRevs, Parameters);
                    Logger.Information("CarTar.BookRev.allocateLineHaulCost - Calculate Line Haul with userLineHaulCost: {userLineHaulCost}, {allocation}", userLineHaulCost, allocation);
                    load.calculateBookRevLineHaul(userLineHaulCost, allocation);
                    Logger.Information("CarTar.BookRev.allocateLineHaulCost - Calculated Line Haul");
                    return load.BookRevenues;
                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.allocateLineHaulCost - Fault Exception");
                    // throw;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "CarTar.BookRev.allocateLineHaulCost - Unexpected Error");
                    //NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.allocateLineHaulCost");
                }
            }
            return null;
        }


        public DTO.BookRevenue[] allocateBFCCostsByAllocationMode(DTO.BookRevenue[] inBookRevs,
            DTO.tblTarBracketType allocation,
            decimal userBFC,
            bool autoCalcBFC)
        {
            try
            {
                Logger.Information("CarTar.BookRev.allocateBFCCostsByAllocationMode");
                Load load = new Load(inBookRevs, Parameters);
                load.allocateBFCCostsByAllocationMode(userBFC, allocation, autoCalcBFC);
                return load.BookRevenues;
            }
            catch (FaultException ex)
            {
                Logger.Error(ex, "CarTar.BookRev.allocateBFCCostsByAllocationMode - Fault Exception");
                //throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CarTar.BookRev.allocateBFCCostsByAllocationMode - Unexpected Error");
                //NGLBookRevenueData.throwUnExpectedFaultException(ex, "NGL.FM.CarTar.BookRev.allocateBFCCostsByAllocationMode");
            }

            return null;
        }

        #endregion

        #region " Protected and Private Methods"

        /// <summary>
        /// Select the lowest cost carrier for the specified BookRevenue object by looping through the carrier tariffs pivot data,
        /// computing the cost, and selecting the lowest cost carrier.
        /// </summary>
        /// <param name="load">Load that is being recalculated.</param>
        /// <param name="carrierTariffsPivot">Collection of carrier tariffs pivot data.</param>
        /// <returns>A clone of the input load, that has been updated with rating results of the lowest cost carrier selected.</returns>
        protected Load selectLowestCostCarrier(
            ref DTO.CarrierCostResults results,
            Load load,
            DTO.CarrierTariffsPivot[] carrierTariffsPivot)
        {

            Load lowestCostLoad = null;

            using (Logger.StartActivity("selectLowestCostCarrier(results: {Results}, Load: {Load}, carrierTariffsPivot: {CarrierTariffPivot}", results, load, carrierTariffsPivot))
            {

                Logger.Information("CarTar.BookRev.selectLowestCostCarrier");
                if (load == null)
                {
                    Logger.Warning("CarTar.BookRev.selectLowestCostCarrier - Load is null.");
                    results.AddLog("Not a valid Load.");
                    return null;
                }

                decimal lowestTotalCost = -1;

                int nIndex = 0; // Index into carrier pivot list, handled by getNextCarrierTariffsPivotWithPrecision;
                int nBestIndex = -1;

                results.AddLog("Get the next carrier tariffs pivot table.");

                Logger.Information("CarTar.BookRev.selectLowestCostCarrier - Get the next carrier tariffs pivot table.");

                DTO.CarrierTariffsPivot[] currentCarrierTariffsPivots =
                    CarrierTariffsPivotProcessorInstance.getNextCarrierTariffsPivotWithPrecision(ref nIndex, carrierTariffsPivot);

                ClassRatingLineAllocation LineAllocation = new ClassRatingLineAllocation();
                // Loop through the carrier tariffs pivots
                while (currentCarrierTariffsPivots != null)
                {
                    Logger.Information("CarTar.BookRev.selectLowestCostCarrier - Rate Carrier Tariff Instance.");
                    // Rate using the current carrier tariffs pivot data.
                    Load ratedLoad = null;

                    LineAllocation.Init();
                    // Pass the list in, regardless of it being one or multiple records.
                    var rctiInstance = RateCarrierTariffInstance;
                    rctiInstance.Parameters = Parameters;
                    decimal currentTotalCost = rctiInstance.rateCarrier(ref results, load, currentCarrierTariffsPivots, ref ratedLoad, ref LineAllocation);

                    Logger.Information("CarTar.BookRev.selectLowestCostCarrier - Rate Carrier Tariff Instance - Current Total Cost: {0}", currentTotalCost);

                    // Check if this is the lowest cost carrier so far.
                    if ((currentTotalCost > 0) &&
                        ((lowestTotalCost == -1) || (currentTotalCost < lowestTotalCost)))
                    {
                        // Save the fields we need for eventual allocation.
                        Logger.Information("CarTar.BookRev.selectLowestCostCarrier - Save the fields we need for eventual allocation.");
                        lowestTotalCost = currentTotalCost;
                        lowestCostLoad = ratedLoad;
                        lowestCostLoad.LineAllocation = LineAllocation.Copy();
                        /*********************************************************************
                        * Modified by RHR 10/16/13 added new logic to subtract one from the nIndex 
                        * counter; the getNextCarrierTariffsPivotWithPrecision always increases the  
                        * value of nIndex even after a match is determined so the value of nIndex is 
                        * 1 greater than the selected index. this can cause an index outside the 
                        * bounds of the array error and will cause the caller to select the wrong rate
                        * *******************************************************************/
                        nBestIndex = (nIndex - 1);
                    }

                    // Get the next carrier tariffs pivot table.
                    Logger.Information("CarTar.BookRev.selectLowestCostCarrier - Get the next carrier tariffs pivot table.  Index: {0}", nIndex);
                    currentCarrierTariffsPivots =
                        CarrierTariffsPivotProcessorInstance.getNextCarrierTariffsPivotWithPrecision(
                            ref nIndex, carrierTariffsPivot);
                }
                if (nBestIndex >= 0) // Allocate only if we had a rated load.
                {
                    Logger.Information("CarTar.BookRev.selectLowestCostCarrier - Allocate costs to each item (linehaul, discount, deficit).");
                    results.AddLog("Allocate costs to each item (linehaul, discount, deficit).");

                    DoLineAllocation(lowestCostLoad, carrierTariffsPivot[nBestIndex]);
                    Logger.Information("CarTar.BookRev.selectLowestCostCarrier - Allocate fees back to each item separately.");
                    results.AddLog("Allocate fees back to each item separately.");
                    DoFeeLineAllocation(lowestCostLoad);
                    results.Success = true;
                }
                else
                {
                    Logger.Warning("CarTar.BookRev.selectLowestCostCarrier - No tariffs found.");
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoTariffsFound);
                    results.Success = false;
                }
                Logger.Information("CarTar.BookRev.selectLowestCostCarrier - Lowest Cost Load: {0}", lowestCostLoad);
            }
            return lowestCostLoad;
        }

        /// <summary>
        /// Allocate fees back from the book revenue objects to the lines, based on weight.
        /// </summary>
        /// <param name="lowestCostLoad"></param>
        /// <param name="LineAllocation"></param>
        protected void DoFeeLineAllocation(Load lowestCostLoad)
        {

            using (Logger.StartActivity("CarTar.BookRev.DoFeeLineAllocation - Load: {LowestCostLoad}", lowestCostLoad))
            {

                double nItemWeight = 0.0;
                double nTotalWeight = 0.0;
                DTO.BookItem bookItemLargest;
                double nHeaviestWeight;
                // Save most expensive book revenue.
                decimal nTotalTaxes, nTotalTaxableFees, nTotalNonTaxableFees;
                decimal nTaxes, nTaxableFees, nNonTaxableFees;
                /*********************************************************************
                 * New code added to test for null object reference by RHR 10/14/13
                 * *******************************************************************/
                if (lowestCostLoad == null)
                {
                    return;
                }

                foreach (DTO.BookRevenue bookRev in lowestCostLoad.BookRevenues)
                {
                    // Get weight for book revenue object.
                    bookItemLargest = null;
                    nHeaviestWeight = -1.0;
                    nTotalTaxableFees = 0.0M;
                    nTotalTaxes = 0.0M;
                    nTotalNonTaxableFees = 0.0M;
                    nTotalWeight = bookRev.BookTotalWgt;

                    Logger.Information(
                        "CarTar.BookRev.DoFeeLineAllocation - BookRev: {BookControl}, Total Weight: {TotalWeight}",
                        bookRev.BookControl, nTotalWeight);
                    using (LogContext.PushProperty("BookControl", bookRev.BookControl))
                    {
                        foreach (DTO.BookLoad bookLoad in bookRev.BookLoads)
                        {
                            bookLoad.TrackingState =
                                NGL.Core.ChangeTracker.TrackingInfo
                                    .Updated; //code change pfm 8/26/2015. Item Details where not saving cuz the state was not set to updated.

                            using (Logger.StartActivity("CarTar.BookRev.DoFeeLineAllocation - BookLoad: {BookLoadControl}", bookLoad.BookLoadControl))
                            {
                                foreach (DTO.BookItem bookItem in bookLoad.BookItems)
                                {

                                    bookItem.TrackingState =
                                        NGL.Core.ChangeTracker.TrackingInfo
                                            .Updated; //code change pfm 8/26/2015. Item Details where not saving cuz the state was not set to updated.
                                                      // Get the weight for each line.
                                    nItemWeight = bookItem.BookItemWeight;
                                    if (nItemWeight > nHeaviestWeight)
                                    {
                                        nHeaviestWeight = nItemWeight;
                                        bookItemLargest = bookItem;
                                        Logger.Information(
                                            "CarTar.BookRev.DoFeeLineAllocation - ItemWeight {ItemWeight} > HeaviestWeight {HeaviestWeight}",
                                            nItemWeight, nHeaviestWeight);
                                    }

                                    int nItemCount = bookLoad.BookItems.Count;
                                    // Taxes
                                    if (nTotalWeight <= 0.0)
                                    {
                                        nTaxes = decimal.Round(bookRev.BookRevFreightTax / nItemCount);
                                        Logger.Information(
                                            "CarTar.BookRev.DoFeeLineAllocation - Total Weight <= 0.0, Taxes: {Taxes}",
                                            nTaxes);
                                    }
                                    else
                                    {
                                        nTaxes = decimal.Round(
                                            bookRev.BookRevFreightTax * (decimal)nItemWeight / (decimal)nTotalWeight, 2);
                                        Logger.Information(
                                            "CarTar.BookRev.DoFeeLineAllocation - Total Weight > 0.0 - Taxes: {Taxes}",
                                            nTaxes);
                                    }

                                    bookItem.BookItemTaxes = nTaxes;
                                    nTotalTaxes += nTaxes;
                                    // Taxable fees
                                    if (nTotalWeight <= 0.0)
                                    {
                                        nTaxableFees = decimal.Round(bookRev.BookRevOtherCost / nItemCount);
                                        Logger.Information(
                                            "CarTar.BookRev.DoFeeLineAllocation - Total Weight <= 0.0, Taxable Fees: {TaxableFees}",
                                            nTaxableFees);
                                    }
                                    else
                                    {
                                        nTaxableFees =
                                            decimal.Round(
                                                bookRev.BookRevOtherCost * (decimal)nItemWeight / (decimal)nTotalWeight, 2);
                                        Logger.Information(
                                            "CarTar.BookRev.DoFeeLineAllocation - Total Weight > 0.0 - Taxable Fees: {TaxableFees}",
                                            nTaxableFees);
                                    }

                                    bookItem.BookItemTaxableFees = nTaxableFees;
                                    nTotalTaxableFees += nTaxableFees;
                                    // Non-taxable fees
                                    if (nTotalWeight <= 0.0)
                                    {
                                        nNonTaxableFees = decimal.Round(bookRev.BookRevNonTaxable / nItemCount);
                                        Logger.Information(
                                            "CarTar.BookRev.DoFeeLineAllocation - Total Weight <= 0.0, Non-Taxable Fees: {NonTaxableFees}",
                                            nNonTaxableFees);
                                    }
                                    else
                                    {
                                        nNonTaxableFees =
                                            decimal.Round(
                                                bookRev.BookRevNonTaxable * (decimal)nItemWeight / (decimal)nTotalWeight,
                                                2);
                                        Logger.Information(
                                            "CarTar.BookRev.DoFeeLineAllocation - Total Weight > 0.0 - Non-Taxable Fees: {NonTaxableFees}",
                                            nNonTaxableFees);
                                    }

                                    bookItem.BookItemNonTaxableFees = nNonTaxableFees;
                                    nTotalNonTaxableFees += nNonTaxableFees;
                                    Logger.Information(
                                        "CarTar.BookRev.DoFeeLineAllocation - BookItem: {BookItemControl}, Taxes: {Taxes}, Taxable Fees: {TaxableFees}, Non-Taxable Fees: {NonTaxableFees}",
                                        bookItem.BookItemControl, nTaxes, nTaxableFees, nNonTaxableFees);
                                }
                            }
                        }

                        if (bookItemLargest != null)
                        {
                            if (bookRev.BookRevFreightTax != nTotalTaxes)
                            {
                                bookItemLargest.BookItemTaxes -= (nTotalTaxes - bookRev.BookRevFreightTax);
                                Logger.Information(
                                    "CarTar.BookRev.DoFeeLineAllocation - BookRevFreightTax ({BookRevFreightTax}) != TotalTaxes ({TotalTaxes}, Adjusting Taxes: {BookItemTaxes}",
                                    bookRev.BookRevFreightTax, nTotalTaxes, bookItemLargest.BookItemTaxes);
                            }

                            if (bookRev.BookRevNonTaxable != nTotalNonTaxableFees)
                            {
                                bookItemLargest.BookItemNonTaxableFees -=
                                    (nTotalNonTaxableFees - bookRev.BookRevNonTaxable);
                                Logger.Information(
                                    "CarTar.BookRev.DoFeeLineAllocation - BookRevNonTaxable ({BookRevNonTaxable}) != TotalNonTaxableFees ({TotalNonTaxableFees}, Adjusting Non-Taxable Fees: {BookItemNonTaxableFees}",
                                    bookRev.BookRevNonTaxable, nTotalNonTaxableFees,
                                    bookItemLargest.BookItemNonTaxableFees);
                            }

                            if (bookRev.BookRevOtherCost != nTotalTaxableFees)
                            {
                                bookItemLargest.BookItemTaxableFees -= (nTotalTaxableFees - bookRev.BookRevOtherCost);
                                Logger.Information(
                                    "CarTar.BookRev.DoFeeLineAllocation - BookRevOtherCost ({BookRevOtherCost}) != TotalTaxableFees ({TotalTaxableFees}, Adjusting Taxable Fees: {BookItemTaxableFees}",
                                    bookRev.BookRevOtherCost, nTotalTaxableFees, bookItemLargest.BookItemTaxableFees);

                            }
                        }
                    }
                }
            }
        }

        protected void DoNonClassRatingLineAllocation(Load lowestCostLoad, DTO.CarrierTariffsPivot Pivot)
        {
            using (Logger.StartActivity("DoNonClassRatingLineAllocation(Load: {@LowestCostLoad}, Pivot: {@Pivot})", lowestCostLoad, Pivot))
            {
                if (lowestCostLoad == null)
                {
                    Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - Load is null.");
                    return;
                }
                double nTotalWeight = 0.0;
                double nItemWeight = 0.0;
                // Save heaviest line.
                DTO.BookItem bookItemLargest = null;
                double nMaxLineWeight = -1.0;

                decimal nDiscount, nTotalDiscount = 0.0M;
                decimal nLineHaul, nTotalLineHaul = 0.0M;
                nTotalWeight = lowestCostLoad.TotalWgt;
                foreach (DTO.BookRevenue bookRev in lowestCostLoad.BookRevenues)
                {
                    bookRev.BookBestDeficitCost = lowestCostLoad.LineAllocation.nBestDeficitCost;
                    bookRev.BookBestDeficitWeight = lowestCostLoad.LineAllocation.nBestDeficitWeight;
                    bookRev.BookBestDeficitWeightBreak = lowestCostLoad.LineAllocation.nBestDeficitWeightBreak;
                    bookRev.BookRatedWeightBreak = lowestCostLoad.LineAllocation.nRatedWeightBreak;
                    bookRev.BookWgtAdjCost = lowestCostLoad.LineAllocation.nWgtAdjCost;
                    bookRev.BookWgtAdjWeight = lowestCostLoad.LineAllocation.nWgtAdjWeight;
                    bookRev.BookWgtAdjWeightBreak = lowestCostLoad.LineAllocation.nWgtAdjWeightBreak;
                    bookRev.BookBilledLoadWeight = lowestCostLoad.LineAllocation.nBilledLoadWeight;
                    bookRev.BookMinAdjustedLoadWeight = lowestCostLoad.LineAllocation.nMinAdjustedLoadWeight;
                    bookRev.BookSummedClassWeight = lowestCostLoad.LineAllocation.nSummedClassWeight;
                    bookRev.BookWgtRoundingVariance = lowestCostLoad.LineAllocation.nWgtRoundingVariance;
                    bookRev.BookHeaviestClass = lowestCostLoad.LineAllocation.sHeaviestClass;
                    bookRev.BookAcutalHeaviestClassWeight = lowestCostLoad.LineAllocation.nAcutalHeaviestClassWeight;
                    bookRev.BookRevDiscountRate = lowestCostLoad.DiscountRate;
                    bookRev.BookRevDiscountMin = lowestCostLoad.DiscountMin;
                    Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - BookRev: {0}, BestDeficitCost: {1}, BestDeficitWeight: {2}, BestDeficitWeightBreak: {3}, RatedWeightBreak: {4}, WgtAdjCost: {5}, WgtAdjWeight: {6}, WgtAdjWeightBreak: {7}, BilledLoadWeight: {8}, MinAdjustedLoadWeight: {9}, SummedClassWeight: {10}, WgtRoundingVariance: {11}, HeaviestClass: {12}, AcutalHeaviestClassWeight: {13}, DiscountRate: {14}, DiscountMin: {15}",
                        bookRev.BookControl, bookRev.BookBestDeficitCost, bookRev.BookBestDeficitWeight, bookRev.BookBestDeficitWeightBreak, bookRev.BookRatedWeightBreak, bookRev.BookWgtAdjCost, bookRev.BookWgtAdjWeight, bookRev.BookWgtAdjWeightBreak, bookRev.BookBilledLoadWeight, bookRev.BookMinAdjustedLoadWeight, bookRev.BookSummedClassWeight, bookRev.BookWgtRoundingVariance, bookRev.BookHeaviestClass, bookRev.BookAcutalHeaviestClassWeight, bookRev.BookRevDiscountRate, bookRev.BookRevDiscountMin);
                    //Save the two new MinChargeFlags here
                    foreach (DTO.BookLoad bookLoad in bookRev.BookLoads)
                    {
                        bookLoad.TrackingState = NGL.Core.ChangeTracker.TrackingInfo.Updated;//code change pfm 8/26/2015. Item Details where not saving cuz the state was not set to updated.
                        foreach (DTO.BookItem bookItem in bookLoad.BookItems)
                        {
                            bookItem.TrackingState = NGL.Core.ChangeTracker.TrackingInfo.Updated;//code change pfm 8/26/2015. Item Details where not saving cuz the state was not set to updated.
                                                                                                 // Find the heaviest line as we go.
                            if (bookItem.BookItemWeight > nMaxLineWeight)
                            {
                                nMaxLineWeight = bookItem.BookItemWeight;
                                bookItemLargest = bookItem;
                            }
                            // Set all to zero except for the one that matches the tariff.
                            bookItem.BookItemDeficit49CFRCode = String.Empty;
                            bookItem.BookItemDeficitDOTCode = String.Empty;
                            bookItem.BookItemDeficitFAKClass = String.Empty;
                            bookItem.BookItemDeficitIATACode = String.Empty;
                            bookItem.BookItemDeficitMarineCode = String.Empty;
                            bookItem.BookItemDeficitNMFCClass = String.Empty;
                            bookItem.BookItemRated49CFRCode = String.Empty;
                            bookItem.BookItemRatedDOTCode = String.Empty;
                            bookItem.BookItemRatedFAKClass = String.Empty;
                            bookItem.BookItemRatedIATACode = String.Empty;
                            bookItem.BookItemRatedMarineCode = String.Empty;
                            bookItem.BookItemRatedNMFCClass = String.Empty;

                            nItemWeight = bookItem.BookItemWeight;
                            bookItem.BookItemDeficitCostAdjustment = 0.0M;
                            bookItem.BookItemDeficitWeightAdjustment = 0.0M;
                            bookItem.BookItemWeightBreak = 0.0M;
                            // Now apply it. Discount, linehaul. No deficit rating.
                            double nAllocatedPercent;
                            if (nTotalWeight <= 0.0)
                            {
                                nAllocatedPercent = 0.0;
                                Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - Total Weight is 0.0.");
                            }
                            else
                            {
                                nAllocatedPercent = nItemWeight / nTotalWeight;
                                Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - Allocated Percent: {0}", nAllocatedPercent);
                            }

                            nDiscount = decimal.Round(lowestCostLoad.TotalDiscount * (decimal)nAllocatedPercent, 2);
                            bookItem.BookItemDiscount = nDiscount;
                            nTotalDiscount += nDiscount;
                            Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - BookItem: {0}, Discount: {1}", bookItem.BookItemControl, nDiscount);
                            nLineHaul = decimal.Round(lowestCostLoad.TotalLineHaul * (decimal)nAllocatedPercent, 2);
                            bookItem.BookItemLineHaul = nLineHaul;
                            nTotalLineHaul += nLineHaul;
                            Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - BookItem: {0}, Line Haul: {1}", bookItem.BookItemControl, nLineHaul);
                            // Get Pivot fields.
                            bookItem.BookItemCarrTarEquipMatControl = Pivot.CarrTarEquipMatControl;
                            bookItem.BookItemCarrTarEquipMatName = Pivot.CarrTarEquipMatName;
                            Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - BookItem: {0}, CarrTarEquipMatControl: {1}, CarrTarEquipMatName: {2}", bookItem.BookItemControl, bookItem.BookItemCarrTarEquipMatControl, bookItem.BookItemCarrTarEquipMatName);
                            //                            bookItem.BookItemCarrTarEquipMatDetID = Pivot.CarrTarEquipMatDetID;
                            //                            bookItem.BookItemCarrTarEquipMatDetValue = Pivot.CarrTarEquipMatDetValue;
                        }
                    }
                }
                // Penny allocation.
                if (bookItemLargest != null) // We found something
                {
                    Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - BookItemLargest: {0}", bookItemLargest.BookItemControl);
                    if (lowestCostLoad.TotalDiscount != nTotalDiscount)
                    {
                        bookItemLargest.BookItemDiscount -= (nTotalDiscount - lowestCostLoad.TotalDiscount);
                        Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - Adjusting Discount: {0}", bookItemLargest.BookItemDiscount);
                    }
                    if (lowestCostLoad.TotalLineHaul != nTotalLineHaul)
                    {
                        bookItemLargest.BookItemLineHaul -= (nTotalLineHaul - lowestCostLoad.TotalLineHaul);
                        Logger.Information("CarTar.BookRev.DoNonClassRatingLineAllocation - Adjusting Line Haul: {0}", bookItemLargest.BookItemLineHaul);
                    }
                }
            }
        }

        /// <summary>
        /// Allocate costs to each item detail record on the load
        /// </summary>
        /// <param name="lowestCostLoad"></param>
        /// <param name="Pivot"></param>
        /// <remarks>
        /// Modified by RHR for v-8.2.0.118 on 09/06/2019
        /// applied changes to support new bookpackage logic
        /// where weights an classes may not exist in the item details.
        /// we now set the default itemClass to sDeficitClass when available
        /// </remarks>
        protected void DoLineAllocation(Load lowestCostLoad, DTO.CarrierTariffsPivot Pivot)
        {
            /*********************************************************************
             * New code added to test for null object reference by RHR 10/14/13
             * *******************************************************************/

            if (lowestCostLoad == null)
            {
                return;
            }
            // Find each line, and allocate accordingly.
            // Each time keep an object of the most expensive line. That line gets the additional pennies.
            // Either use a percent from class rating or percent of total shipment weight, class rating if available.
            bool bClassRating;
            using (var operation = Logger.StartActivity("DoLineAllocation(Load: {Load}, Pivot: {Pivot})", lowestCostLoad, Pivot))
            {
                Logger.Information(
                    "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.WeightMapping.Count: {0}",
                    lowestCostLoad.LineAllocation.WeightMapping.Count);
                if (lowestCostLoad.LineAllocation.WeightMapping.Count == 0)
                {
                    bClassRating = false;
                }
                else
                {
                    bClassRating = true;
                }

                Logger.Information("CarTar.BookRev.DoLineAllocation - Class Rating: {0}", bClassRating);

                // Do allocation that's specific to class rating
                if (bClassRating == false)
                {
                    Logger.Information("CarTar.BookRev.DoLineAllocation - Do Non-Class Rating Line Allocation.");
                    DoNonClassRatingLineAllocation(lowestCostLoad, Pivot);
                }
                else // class rating is true
                {
                    Logger.Information("CarTar.BookRev.DoLineAllocation - Do Class Rating Line Allocation.");
                    int classType = Pivot.CarrTarEquipMatClassTypeControl;
                    string itemClass;
                    DTO.BookItem itemLargest = null;
                    double nPercentLargest = -1.0;
                    decimal nTotalDiscount = 0.0M, nTotalLineHaul = 0.0M, nTotalDeficit = 0.0M;
                    decimal nDiscount, nLineHaul, nDeficit;
                    string s49CFRDefault = "100";
                    string sIATADefault = "100";
                    string sDOTDefault = "100";
                    string sMarineDefault = "100";
                    string sNMFCDefault = "100";
                    string sFAKDefault = "100";

                    List<string> sFaultInfo = new List<string>();


                    Logger.Information("CarTar.BookRev.DoLineAllocation - populateDefaultClassCodes.");
                    populateDefaultClassCodes(lowestCostLoad.RatedBookRevenue.BookCustCompControl, ref sFaultInfo,
                        ref s49CFRDefault, ref sIATADefault, ref sDOTDefault, ref sMarineDefault, ref sNMFCDefault,
                        ref sFAKDefault);
                    Logger.Information(
                        "CarTar.BookRev.DoLineAllocation - populateDefaultClassCodes completed, values: s49CFRDefault: {0}, sIATADefault: {1}, sDOTDefault: {2}, sMarineDefault: {3}, sNMFCDefault: {4}, sFAKDefault: {5}",
                        s49CFRDefault, sIATADefault, sDOTDefault, sMarineDefault, sNMFCDefault, sFAKDefault);
                    if ((sFaultInfo != null && sFaultInfo.Count > 0))
                    {
                        Logger.Warning("CarTar.BookRev.DoLineAllocation - populateDefaultClassCodes failed.");
                        //results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
                        return;
                    }

                    // Find class information.
                    foreach (DTO.BookRevenue bookRev in lowestCostLoad.BookRevenues)
                    {

                        Logger.Information(
                            "CarTar.BookRev.DoLineAllocation - foreach (DTO.BookRevenue bookRev in lowestCostLoad.BookRevenues)");

                        bookRev.BookBestDeficitCost = lowestCostLoad.LineAllocation.nBestDeficitCost;
                        bookRev.BookBestDeficitWeight = lowestCostLoad.LineAllocation.nBestDeficitWeight;
                        bookRev.BookBestDeficitWeightBreak = lowestCostLoad.LineAllocation.nBestDeficitWeightBreak;
                        bookRev.BookRatedWeightBreak = lowestCostLoad.LineAllocation.nRatedWeightBreak;
                        bookRev.BookWgtAdjCost = lowestCostLoad.LineAllocation.nWgtAdjCost;
                        bookRev.BookWgtAdjWeight = lowestCostLoad.LineAllocation.nWgtAdjWeight;
                        bookRev.BookWgtAdjWeightBreak = lowestCostLoad.LineAllocation.nWgtAdjWeightBreak;
                        bookRev.BookBilledLoadWeight = lowestCostLoad.LineAllocation.nBilledLoadWeight;
                        bookRev.BookMinAdjustedLoadWeight = lowestCostLoad.LineAllocation.nMinAdjustedLoadWeight;
                        bookRev.BookSummedClassWeight = lowestCostLoad.LineAllocation.nSummedClassWeight;
                        bookRev.BookWgtRoundingVariance = lowestCostLoad.LineAllocation.nWgtRoundingVariance;
                        bookRev.BookHeaviestClass = lowestCostLoad.LineAllocation.sHeaviestClass;
                        bookRev.BookAcutalHeaviestClassWeight =
                            lowestCostLoad.LineAllocation.nAcutalHeaviestClassWeight;
                        bookRev.BookRevDiscountRate = lowestCostLoad.DiscountRate;
                        bookRev.BookRevDiscountMin = lowestCostLoad.DiscountMin;
                        //Save the two new MinChargeFlags here

                        Logger.Information(
                            "CarTar.BookRev.DoLineAllocation - Check if bookRev.BookLoads != null && bookRev.BookLoads.Count() > 0");
                        if (bookRev.BookLoads != null && bookRev.BookLoads.Count() > 0)
                        {
                            foreach (DTO.BookLoad bookLoad in bookRev.BookLoads)
                            {
                                Logger.Information(
                                    "CarTar.BookRev.DoLineAllocation - check if bookLoad.BookItems != null && bookLoad.BookItems.Count() > 0");
                                bookLoad.TrackingState =
                                    NGL.Core.ChangeTracker.TrackingInfo
                                        .Updated; //code change pfm 8/26/2015. Item Details where not saving cuz the state was not set to updated.
                                if (bookLoad.BookItems != null && bookLoad.BookItems.Count() > 0)
                                {
                                    foreach (DTO.BookItem bookItem in bookLoad.BookItems)
                                    {
                                        bookItem.TrackingState =
                                            NGL.Core.ChangeTracker.TrackingInfo
                                                .Updated; //code change pfm 8/26/2015. Item Details where not saving cuz the state was not set to updated.
                                        itemClass = ""; // Blank if unknown.
                                        //Modified by RHR for v-8.2.0.118 on 09/06/2019
                                        // for bookpackage logic we now set the default itemClass to sDeficitClass if it exists
                                        Logger.Information("CarTar.BookRev.DoLineAllocation - switch (classType) ({0})",
                                            classType);
                                        switch (classType)
                                        {
                                            case (int)DAL.Utilities.TariffClassType.class49CFR:
                                                {
                                                    if (string.IsNullOrEmpty(bookItem.BookItem49CFRCode) ||
                                                        bookItem.BookItem49CFRCode.Trim().Length < 1)
                                                    {
                                                        if (string.IsNullOrWhiteSpace(lowestCostLoad.LineAllocation
                                                                .sDeficitClass))
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - s49CFRDefault: {0}",
                                                                s49CFRDefault);
                                                            itemClass = s49CFRDefault;
                                                        }
                                                        else
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.sDeficitClass: {0}",
                                                                lowestCostLoad.LineAllocation.sDeficitClass);
                                                            itemClass = lowestCostLoad.LineAllocation.sDeficitClass;
                                                        }


                                                    }
                                                    else
                                                    {
                                                        Logger.Information(
                                                            "CarTar.BookRev.DoLineAllocation - bookItem.BookItem49CFRCode: {0}",
                                                            bookItem.BookItem49CFRCode);
                                                        itemClass = bookItem.BookItem49CFRCode;
                                                    }

                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classDOT:
                                                {

                                                    if (string.IsNullOrEmpty(bookItem.BookItemDOTCode) ||
                                                        bookItem.BookItemDOTCode.Trim().Length < 1)
                                                    {

                                                        if (string.IsNullOrWhiteSpace(lowestCostLoad.LineAllocation
                                                                .sDeficitClass))
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - sDOTDefault: {0}",
                                                                sDOTDefault);
                                                            itemClass = sDOTDefault;
                                                        }
                                                        else
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.sDeficitClass: {0}",
                                                                lowestCostLoad.LineAllocation.sDeficitClass);
                                                            itemClass = lowestCostLoad.LineAllocation.sDeficitClass;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.Information(
                                                            "CarTar.BookRev.DoLineAllocation - bookItem.BookItemDOTCode: {0}",
                                                            bookItem.BookItemDOTCode);
                                                        itemClass = bookItem.BookItemDOTCode;
                                                    }

                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classFAK:
                                                {
                                                    if (string.IsNullOrEmpty(bookItem.BookItemFAKClass) ||
                                                        bookItem.BookItemFAKClass.Trim().Length < 1)
                                                    {
                                                        if (string.IsNullOrWhiteSpace(lowestCostLoad.LineAllocation
                                                                .sDeficitClass))
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - sFAKDefault: {0}",
                                                                sFAKDefault);
                                                            itemClass = sFAKDefault;
                                                        }
                                                        else
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.sDeficitClass: {0}",
                                                                lowestCostLoad.LineAllocation.sDeficitClass);
                                                            itemClass = lowestCostLoad.LineAllocation.sDeficitClass;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.Information(
                                                            "CarTar.BookRev.DoLineAllocation - bookItem.BookItemFAKClass: {0}",
                                                            bookItem.BookItemFAKClass);
                                                        itemClass = bookItem.BookItemFAKClass;
                                                    }

                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classIATA:
                                                {
                                                    if (string.IsNullOrEmpty(bookItem.BookItemIATACode) ||
                                                        bookItem.BookItemIATACode.Trim().Length < 1)
                                                    {
                                                        if (string.IsNullOrWhiteSpace(lowestCostLoad.LineAllocation
                                                                .sDeficitClass))
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - sIATADefault: {0}",
                                                                sIATADefault);
                                                            itemClass = sIATADefault;
                                                        }
                                                        else
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.sDeficitClass: {0}",
                                                                lowestCostLoad.LineAllocation.sDeficitClass);
                                                            itemClass = lowestCostLoad.LineAllocation.sDeficitClass;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.Information(
                                                            "CarTar.BookRev.DoLineAllocation - bookItem.BookItemIATACode: {0}",
                                                            bookItem.BookItemIATACode);
                                                        itemClass = bookItem.BookItemIATACode;
                                                    }

                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classMarine:
                                                {
                                                    if (string.IsNullOrEmpty(bookItem.BookItemMarineCode) ||
                                                        bookItem.BookItemMarineCode.Trim().Length < 1)
                                                    {
                                                        if (string.IsNullOrWhiteSpace(lowestCostLoad.LineAllocation
                                                                .sDeficitClass))
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - sMarineDefault: {0}",
                                                                sMarineDefault);
                                                            itemClass = sMarineDefault;
                                                        }
                                                        else
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.sDeficitClass: {0}",
                                                                lowestCostLoad.LineAllocation.sDeficitClass);
                                                            itemClass = lowestCostLoad.LineAllocation.sDeficitClass;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.Information(
                                                            "CarTar.BookRev.DoLineAllocation - bookItem.BookItemMarineCode: {0}",
                                                            bookItem.BookItemMarineCode);
                                                        itemClass = bookItem.BookItemMarineCode;
                                                    }

                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classNMFC:
                                                {
                                                    if (string.IsNullOrEmpty(bookItem.BookItemNMFCClass) ||
                                                        bookItem.BookItemNMFCClass.Trim().Length < 1)
                                                    {
                                                        if (string.IsNullOrWhiteSpace(lowestCostLoad.LineAllocation
                                                                .sDeficitClass))
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - sNMFCDefault: {0}",
                                                                sNMFCDefault);
                                                            itemClass = sNMFCDefault;
                                                        }
                                                        else
                                                        {
                                                            Logger.Information(
                                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.sDeficitClass: {0}",
                                                                lowestCostLoad.LineAllocation.sDeficitClass);
                                                            itemClass = lowestCostLoad.LineAllocation.sDeficitClass;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.Information(
                                                            "CarTar.BookRev.DoLineAllocation - bookItem.BookItemNMFCClass: {0}",
                                                            bookItem.BookItemNMFCClass);
                                                        itemClass = bookItem.BookItemNMFCClass;
                                                    }

                                                    break;
                                                }
                                            default:
                                                {
                                                    if (string.IsNullOrWhiteSpace(lowestCostLoad.LineAllocation
                                                            .sDeficitClass))
                                                    {
                                                        Logger.Warning(
                                                            "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.sDeficitClass: {0} which shold never happen",
                                                            lowestCostLoad.LineAllocation.sDeficitClass);
                                                        itemClass = ""; // this should never happen
                                                    }
                                                    else
                                                    {
                                                        Logger.Information(
                                                            "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.sDeficitClass: {0}",
                                                            lowestCostLoad.LineAllocation.sDeficitClass);
                                                        itemClass = lowestCostLoad.LineAllocation.sDeficitClass;
                                                    }

                                                    break;
                                                }
                                        }

                                        if (string.IsNullOrEmpty(itemClass) || itemClass.Trim().Length < 1)
                                        {
                                            Logger.Information(
                                                "CarTar.BookRev.DoLineAllocation - itemClass: {0} - setting to 100",
                                                itemClass);
                                            itemClass = "100";
                                        }

                                        // Modified by RHR for v-8.2.0.118 on 09/06/2019
                                        // for bookpackage logic we now confirm that the itemClass
                                        // exists in the FAKMapping dictionary,  
                                        //if not check if lowestCostLoad.LineAllocation.sDeficitClass exists and use that if possible
                                        // if not possible use the first 
                                        // item in the dictionary for  classused.
                                        String sClassUsed = itemClass;

                                        Logger.Information(
                                            "CarTar.BookRev.DoLineAllocation - check if lowestCostLoad.LineAllocation.FAKMapping.ContainsKey(Tuple.Create(itemClass, classType))");
                                        if (lowestCostLoad.LineAllocation.FAKMapping != null &&
                                            lowestCostLoad.LineAllocation.FAKMapping.ContainsKey(Tuple.Create(itemClass,
                                                classType)))
                                        {
                                            sClassUsed =
                                                lowestCostLoad.LineAllocation.FAKMapping[
                                                    Tuple.Create(itemClass, classType)];
                                            Logger.Information(
                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.FAKMapping.ContainsKey(Tuple.Create(itemClass, classType)) - sClassUsed: {0}",
                                                sClassUsed);
                                        }
                                        else if (lowestCostLoad.LineAllocation.FAKMapping != null &&
                                                 lowestCostLoad.LineAllocation.FAKMapping.ContainsKey(
                                                     Tuple.Create(lowestCostLoad.LineAllocation.sDeficitClass,
                                                         classType)))
                                        {
                                            sClassUsed = lowestCostLoad.LineAllocation.FAKMapping[
                                                Tuple.Create(lowestCostLoad.LineAllocation.sDeficitClass, classType)];
                                            Logger.Information(
                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.FAKMapping.ContainsKey(Tuple.Create(lowestCostLoad.LineAllocation.sDeficitClass, classType)) - sClassUsed: {0}",
                                                sClassUsed);
                                        }
                                        else
                                        {
                                            var firstElement =
                                                lowestCostLoad.LineAllocation.FAKMapping.FirstOrDefault();
                                            sClassUsed = firstElement.Value;
                                            Logger.Information(
                                                "CarTar.BookRev.DoLineAllocation - lowestCostLoad.LineAllocation.FAKMapping.FirstOrDefault() - sClassUsed: {0}",
                                                sClassUsed);
                                        }


                                        // Set all to zero except for the one that matches the tariff.
                                        bookItem.BookItemDeficit49CFRCode = String.Empty;
                                        bookItem.BookItemDeficitDOTCode = String.Empty;
                                        bookItem.BookItemDeficitFAKClass = String.Empty;
                                        bookItem.BookItemDeficitIATACode = String.Empty;
                                        bookItem.BookItemDeficitMarineCode = String.Empty;
                                        bookItem.BookItemDeficitNMFCClass = String.Empty;
                                        bookItem.BookItemRated49CFRCode = String.Empty;
                                        bookItem.BookItemRatedDOTCode = String.Empty;
                                        bookItem.BookItemRatedFAKClass = String.Empty;
                                        bookItem.BookItemRatedIATACode = String.Empty;
                                        bookItem.BookItemRatedMarineCode = String.Empty;
                                        bookItem.BookItemRatedNMFCClass = String.Empty;

                                        // Modified by RHR for v-8.2.0.118 on 09/06/2019
                                        // we now update the item with the identified itemClass

                                        Logger.Information("CarTar.BookRev.DoLineAllocation - switch (classType) ({0})",
                                            classType);
                                        switch (classType)
                                        {
                                            case (int)DAL.Utilities.TariffClassType.class49CFR:
                                                {
                                                    bookItem.BookItemDeficit49CFRCode =
                                                        lowestCostLoad.LineAllocation.sDeficitClass;
                                                    bookItem.BookItemRated49CFRCode = sClassUsed;
                                                    bookItem.BookItem49CFRCode = itemClass;
                                                    Logger.Information(
                                                        "CarTar.BookRev.DoLineAllocation - bookItem.BookItem49CFRCode: {0}",
                                                        bookItem.BookItem49CFRCode);
                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classDOT:
                                                {
                                                    bookItem.BookItemDeficitDOTCode =
                                                        lowestCostLoad.LineAllocation.sDeficitClass;
                                                    bookItem.BookItemDOTCode = sClassUsed;
                                                    bookItem.BookItemDOTCode = itemClass;
                                                    Logger.Information(
                                                        "CarTar.BookRev.DoLineAllocation - bookItem.BookItemDOTCode: {0}",
                                                        bookItem.BookItemDOTCode);
                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classFAK:
                                                {
                                                    bookItem.BookItemDeficitFAKClass =
                                                        lowestCostLoad.LineAllocation.sDeficitClass;
                                                    bookItem.BookItemRatedFAKClass = sClassUsed;
                                                    bookItem.BookItemFAKClass = itemClass;
                                                    Logger.Information(
                                                        "CarTar.BookRev.DoLineAllocation - bookItem.BookItemFAKClass: {0}",
                                                        bookItem.BookItemFAKClass);
                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classIATA:
                                                {
                                                    bookItem.BookItemDeficitIATACode =
                                                        lowestCostLoad.LineAllocation.sDeficitClass;
                                                    bookItem.BookItemRatedIATACode = sClassUsed;
                                                    bookItem.BookItemIATACode = itemClass;
                                                    Logger.Information(
                                                        "CarTar.BookRev.DoLineAllocation - bookItem.BookItemIATACode: {0}",
                                                        bookItem.BookItemIATACode);
                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classMarine:
                                                {
                                                    bookItem.BookItemDeficitMarineCode =
                                                        lowestCostLoad.LineAllocation.sDeficitClass;
                                                    bookItem.BookItemRatedMarineCode = sClassUsed;
                                                    bookItem.BookItemMarineCode = itemClass;
                                                    Logger.Information(
                                                        "CarTar.BookRev.DoLineAllocation - bookItem.BookItemMarineCode: {0}",
                                                        bookItem.BookItemMarineCode);
                                                    break;
                                                }
                                            case (int)DAL.Utilities.TariffClassType.classNMFC:
                                                {
                                                    bookItem.BookItemDeficitNMFCClass =
                                                        lowestCostLoad.LineAllocation.sDeficitClass;
                                                    bookItem.BookItemRatedNMFCClass = sClassUsed;
                                                    bookItem.BookItemNMFCClass = itemClass;
                                                    Logger.Information(
                                                        "CarTar.BookRev.DoLineAllocation - bookItem.BookItemNMFCClass: {0}",
                                                        bookItem.BookItemNMFCClass);
                                                    break;
                                                }
                                            default:
                                                {
                                                    itemClass = String.Empty;
                                                    Logger.Warning(
                                                        "CarTar.BookRev.DoLineAllocation - classType: {0} which shold never happen",
                                                        classType);
                                                    break;
                                                }
                                        }

                                        double nClassWeight =
                                            lowestCostLoad.LineAllocation.WeightMapping[
                                                Tuple.Create(sClassUsed, classType)];
                                        double nItemWeight = bookItem.BookItemWeight;
                                        double nClassPercent =
                                            lowestCostLoad.LineAllocation.PercentMapping[
                                                Tuple.Create(sClassUsed, classType)];

                                        Logger.Information(
                                            "Set nClassWeight: {0}, nItemWeight: {1}, nClassPercent: {2}", nClassWeight,
                                            nItemWeight, nClassPercent);

                                        // Now apply it. Discount, linehaul, and deficit. Linehaul should be the linehaul without deficit rating.
                                        double nAllocatedPercent;

                                        if (nClassWeight == 0.0)
                                        {
                                            Logger.Information(
                                                "nClassweight is zero so setting nAllocatedPercent to 0.0");
                                            nAllocatedPercent = 0.0;
                                        }
                                        else
                                        {
                                            nAllocatedPercent = nItemWeight / nClassWeight * nClassPercent;
                                            Logger.Information(
                                                "nClassweight is not zero so setting nAllocatedPercent to nItemWeight / nClassWeight * nClassPercent: {0}",
                                                nAllocatedPercent);
                                        }

                                        if (nAllocatedPercent > nPercentLargest)
                                        {
                                            nPercentLargest = nAllocatedPercent;
                                            itemLargest = bookItem;
                                            Logger.Information("nAllocatedPercent [{0}] > nPercentLargest [{1}]",
                                                nAllocatedPercent, nPercentLargest);
                                        }

                                        // Calculate line discount and summed total.
                                        nDiscount = decimal.Round(lowestCostLoad.TotalDiscount *
                                                                  (decimal)nAllocatedPercent);
                                        Logger.Information(
                                            "Calculate nDiscount which is lowestCostLoad.TotalDiscount * (decimal)nAllocatedPercent: {0}",
                                            nDiscount);
                                        bookItem.BookItemDiscount = nDiscount;
                                        nTotalDiscount += nDiscount;
                                        Logger.Information(
                                            "CarTar.BookRev.DoLineAllocation - BookItem: {0}, Discount: {1}",
                                            bookItem.BookItemControl, nDiscount);
                                        // Calculate line's line haul and summed total.
                                        nLineHaul = decimal.Round(
                                            lowestCostLoad.LineAllocation.nLineHaul * (decimal)nAllocatedPercent, 2);
                                        bookItem.BookItemLineHaul = nLineHaul;
                                        nTotalLineHaul += nLineHaul;
                                        Logger.Information(
                                            "CarTar.BookRev.DoLineAllocation - BookItem: {0}, Line Haul: {1}",
                                            bookItem.BookItemControl, nLineHaul);
                                        // Calculane line deficit cost and summed total.
                                        nDeficit = decimal.Round(
                                            lowestCostLoad.LineAllocation.nBestDeficitCost * (decimal)nAllocatedPercent,
                                            2);
                                        bookItem.BookItemDeficitCostAdjustment = nDeficit;
                                        nTotalDeficit += nDeficit;
                                        Logger.Information(
                                            "CarTar.BookRev.DoLineAllocation - BookItem: {0}, Deficit: {1}",
                                            bookItem.BookItemControl, nDeficit);
                                        // FAK/discount fields.
                                        // Get Pivot fields.
                                        bookItem.BookItemCarrTarEquipMatControl = Pivot.CarrTarEquipMatControl;
                                        bookItem.BookItemCarrTarEquipMatName = Pivot.CarrTarEquipMatName;
                                        Logger.Information(
                                            "CarTar.BookRev.DoLineAllocation - BookItem: {0}, CarrTarEquipMatControl: {1}, CarrTarEquipMatName: {2}",
                                            bookItem.BookItemControl, bookItem.BookItemCarrTarEquipMatControl,
                                            bookItem.BookItemCarrTarEquipMatName);
                                        //bookItem.BookItemCarrTarEquipMatDetID = Pivot.CarrTarEquipMatDetID;
                                        //bookItem.BookItemCarrTarEquipMatDetValue = Pivot.CarrTarEquipMatDetValue;
                                        //bookItem.BookItemCarrTarEquipMatDetValue = lowestCostLoad.LineAllocation.newobject.GetValue(bookitemcontrol);
                                        // Get Weight fields.
                                        bookItem.BookItemWeightBreak =
                                            (decimal)lowestCostLoad.LineAllocation.nRatedWeightBreak;
                                        bookItem.BookItemDeficitWeightAdjustment =
                                            (decimal)lowestCostLoad.LineAllocation.nBestDeficitWeight;
                                        Logger.Information(
                                            "CarTar.BookRev.DoLineAllocation - BookItem: {0}, WeightBreak: {1}, DeficitWeightAdjustment: {2}",
                                            bookItem.BookItemControl, bookItem.BookItemWeightBreak,
                                            bookItem.BookItemDeficitWeightAdjustment);
                                    }
                                }
                            }
                        }
                    }

                    Logger.Information("CarTar.BookRev.DoLineAllocation - Check if itemLargest != null");
                    if (itemLargest != null)
                    {
                        Logger.Information("CarTar.BookRev.DoLineAllocation - itemLargest != null, so adjusting");
                        if (nTotalLineHaul != lowestCostLoad.LineAllocation.nLineHaul)
                        {
                            Logger.Information(
                                "CarTar.BookRev.DoLineAllocation - nTotalLineHaul != lowestCostLoad.LineAllocation.nLineHaul, so adjusting to nTotalLineHaul - lowestCostLoad.LineAllocation.nLineHaul");
                            itemLargest.BookItemLineHaul -= nTotalLineHaul - lowestCostLoad.LineAllocation.nLineHaul;
                        }

                        Logger.Information(
                            "CarTar.BookRev.DoLineAllocation - check if nTotalDeficit != lowestCostLoad.LineAllocation.nBestDeficitCost");
                        if (nTotalDeficit != lowestCostLoad.LineAllocation.nBestDeficitCost)
                        {
                            Logger.Information(
                                "CarTar.BookRev.DoLineAllocation - nTotalDeficit != lowestCostLoad.LineAllocation.nBestDeficitCost, so adjusting to nTotalDeficit - lowestCostLoad.LineAllocation.nBestDeficitCost");
                            itemLargest.BookItemDeficitCostAdjustment -=
                                nTotalDeficit - lowestCostLoad.LineAllocation.nBestDeficitCost;
                        }

                        Logger.Information(
                            "CarTar.BookRev.DoLineAllocation - check if nTotalDiscount != lowestCostLoad.TotalDiscount");
                        if (nTotalDiscount != lowestCostLoad.TotalDiscount)
                        {
                            Logger.Information(
                                "CarTar.BookRev.DoLineAllocation - nTotalDiscount != lowestCostLoad.TotalDiscount, so adjusting to nTotalDiscount - lowestCostLoad.TotalDiscount");
                            itemLargest.BookItemDiscount -= nTotalDiscount - lowestCostLoad.TotalDiscount;
                        }

                        Logger.Information("CarTar.BookRev.DoLineAllocation - Adjusted Line: {0}",
                            itemLargest.BookItemControl);
                    }
                }
            }
        }

        /// <summary>
        /// Recalculate the accessorial fees for the currently assigned carrier and tariff using the
        /// existing Line Haul costs (i.e., don't recalculate the Line Haul, minimum charge and discounts. )
        /// </summary>
        /// <param name="load">Load that is being recalculated.</param>
        /// <returns>A clone of the input load, that has been updated with rating results.</returns>
        protected Load recalculateUsingLineHaul(ref DTO.CarrierCostResults results, Load load, bool autocalculateBFC = true)
        {
            Load ratedLoad = null;

            using (var operation = Logger.StartActivity("recalculateUsingLineHaul(Results: {Results}, autocalculateBFC: {AutoCalculateBFC})", results, autocalculateBFC))
            {


                if (results == null) { results = new DTO.CarrierCostResults(); }

                results.AddLog("Recalculate Using Line Haul");
                Logger.Information("RateCarrierTariffInstance.recalculateUsingLineHaul, autoCalculateBFC = {0}", autocalculateBFC);

                var rctInstance = new RateCarrierTariff(Parameters);

                decimal currentTotalCost = rctInstance.recalculateUsingLineHaul(ref results, load, ref ratedLoad, autocalculateBFC);
                Logger.Information("Total Cost = {0}", currentTotalCost);
                results.AddLog(string.Format("Total Cost = {0}", currentTotalCost));
                if (currentTotalCost > 0.0M)
                {
                    Logger.Information("Allocate costs to each item (linehaul, discount, deficit).");
                    // Allocate fees back to the lines separately.
                    if (ratedLoad != null)
                    {
                        Logger.Information("ratedLoad is not null, calling DoLineAllocation");
                        results.AddLog("Allocate Fees");
                        DoFeeLineAllocation(ratedLoad);
                        results.Success = true;
                        Logger.Information("Allocation successful.");
                    }
                    else
                    {
                        Logger.Information("ratedLoad is null, allocation failed.");
                        results.Success = false;
                    }
                }
            }
            return ratedLoad;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <param name="load"></param>
        /// <param name="CarrTarEquipControl"></param>
        /// <returns></returns>
        protected Load recalculateUsingCarrTarEquipControl(
            ref DTO.CarrierCostResults results,
            Load load,
            int BookControl,
            int carrTarEquipControl,
            int carrTarEquipMatControl)
        {
            Load ratedLoad = null;
            using (var operation = Logger.StartActivity("RateCarrierTariffInstance.recalculateUsingCarrTarEquipControl with BookControl={BookControl}, carrTarEquipControl={carrTarEquipControl}, carrTarEquipMatControl={carrTarEquipControl}", BookControl, carrTarEquipControl, carrTarEquipMatControl))
            {
                results.AddLog("Rate using the current carrier tariff equipment assigned.");


                DTO.CarrierTariffsPivot[] carrierTariffsPivots = null;
                try
                {
                    results.AddLog("Read tariff rates by current equipment");

                    carrierTariffsPivots = NGLCarrTarContractData.GetCarrierTariffsPivotByEquip(BookControl, carrTarEquipControl);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    Logger.Error(sqlEx, "FaultException in getCarrierTariffsPivot.");
                    if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                    {
                        Logger.Error(sqlEx, "FaultException in getCarrierTariffsPivot.");
                        results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details);
                        // "E_SQLExceptionMSG", E_UnExpectedMSG
                        string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                            sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                        SaveSysError(errMsg, sourcePath("getCarrierTariffsPivot"));
                        results.AddLog("System error log updated.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "General in getCarrierTariffsPivot.");
                    results.AddLog(string.Format("Unexpected Error: {0} ", ex.Message));
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    throw;
                }
                //Validate that we have a tariff
                Logger.Information("Validate that we have a tariff");
                if (carrierTariffsPivots == null || carrierTariffsPivots.Count() < 1)
                {
                    Logger.Warning("No Tariffs Found", results);
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoTariffsFound);
                    results.Success = false;
                    return ratedLoad;        // There are no carriers to assign (NOTE: this will return a null value because ratedLoad is null at this point.  I am not sure this is the desired result RHR 6/11/14)
                }
                ClassRatingLineAllocation LineAllocation = new ClassRatingLineAllocation(); // Need to pass this in but do not use for estimated results.
                LineAllocation.Init();
                DTO.CarrierTariffsPivot[] filteredPivots = null;
                //Determine if we are processing an LTL tariff
                //BugFixed by RHR 9/23/14 we need to test the RateType != 3 rather than the TariffType the Tariff Type is public or private
                //  Also replace the number 3 with the TariffRateType enum            
                //if (carrierTariffsPivots[0].CarrTarTariffTypeControl != 3 &&  carrierTariffsPivots.Any(x => x.CarrTarEquipMatControl == carrTarEquipMatControl)  ) //Class

                Logger.Information("Determine if we are processing an LTL tariff by checking if carrierTariffsPivots[0].CarrTarEquipMatTarRateTypeControl ({0}) != (int)DAL.Utilities.TariffRateType.ClassRate ({1}) && carrierTariffsPivots.Any(x => x.CarrTarEquipMatControl == {0})", carrTarEquipMatControl, (int)DAL.Utilities.TariffRateType.ClassRate);
                if (carrierTariffsPivots[0].CarrTarEquipMatTarRateTypeControl != (int)DAL.Utilities.TariffRateType.ClassRate && carrierTariffsPivots.Any(x => x.CarrTarEquipMatControl == carrTarEquipMatControl)) //Class
                {
                    //we use the matrix record that was selected.
                    Logger.Information("FilteredPivots = carrierTariffsPivots.Where(x => x.CarrTarEquipMatControl == carrTarEquipMatControl).ToArray()");
                    filteredPivots = carrierTariffsPivots.Where(x => x.CarrTarEquipMatControl == carrTarEquipMatControl).ToArray();
                    Logger.Information("filteredPivots.Count() = {0}", filteredPivots?.Count());
                }
                else
                {
                    //use all the records returned
                    Logger.Information("filteredPivots = carrierTariffsPivots");
                    filteredPivots = carrierTariffsPivots;
                }
                // Pass the list in, regardless of it being one or multiple records.
                Logger.Information("RateCarrierTariffInstance.rateCarrier(ref results, load, filteredPivots, ref ratedLoad, ref LineAllocation)");

                var rctInstance = RateCarrierTariffInstance;
                rctInstance.Parameters = Parameters;
                decimal currentTotalCost = rctInstance.rateCarrier(ref results, load, filteredPivots, ref ratedLoad, ref LineAllocation);

                Logger.Information("Check to see if currentTotalCost {0} > 0.0M", currentTotalCost);
                if (currentTotalCost > 0.0M) // Allocate if rating using the pivot record was successful.
                {
                    // Save the fields we need for  allocation.  
                    Logger.Information("ratedLoad.LineAllocation = LineAllocation.Copy();");
                    ratedLoad.LineAllocation = LineAllocation.Copy();
                    results.AddLog("Allocate costs to each item (linehaul, discount, deficit).");

                    Logger.Information("DoLineAllocation(ratedLoad, carrierTariffsPivots[0])");
                    DoLineAllocation(ratedLoad, carrierTariffsPivots[0]);
                    results.AddLog("Allocate fees back to each item separately.");

                    Logger.Information("DoFeeLineAllocation(ratedLoad)");
                    DoFeeLineAllocation(ratedLoad);
                    Logger.Information("results.Success = true, ratedLoad: {@0}", ratedLoad);
                    results.Success = true;
                }
                else
                {
                    Logger.Warning("No Tariffs Found, results: {@0}", results);
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoTariffsFound);
                    results.Success = false;
                }
            }
            return ratedLoad;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <param name="load"></param>
        /// <param name="carrierTariffsPivot"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-7.0.6.0 on 11/7/2016
        ///  added new logic to test for preferred carrier settings
        /// </remarks>
        /// 

        protected bool estimatedCarriersByCost(ref DTO.CarrierCostResults results,
              Load load,
              DTO.CarrierTariffsPivot[] carrierTariffsPivot)
        {
            bool blnTestPreferredCarriers = false;
            bool blnRet = false;
            int nIndex = 0; // Index into carrier pivot list, handled by getNextCarrierTariffsPivotWithPrecision;            
            using (var operation = Logger.StartActivity("RateCarrierTariffInstance.estimatedCarriersByCost"))
            {

                Logger.Information("Determing preferred carriers for load based on RatedBookRevenue.BookODControl which is {BookODControl}", load.RatedBookRevenue.BookODControl);

                DTO.LimitLaneToCarrier[] preferredCarriers = NGLLaneData.GetLanePreferredCarriers(load.RatedBookRevenue.BookODControl);

                // Logger.Information("Preferred carriers found: {0}.  Setting blnTestPreferredCarriers = true", preferredCarriers.Count());


                if (preferredCarriers != null && preferredCarriers.Count() > 0)
                {
                    Logger.Information("Preferred carriers found: {0}.  Setting blnTestPreferredCarriers = true", preferredCarriers.Count());
                    blnTestPreferredCarriers = true;
                }

                Logger.Information("Getting currentCarrierTariffsPivots for nIndex: {0} via CarrierTariffsPivotProcessorInstance.getNextCarrierTariffsPivotWithPrecision", nIndex);

                DTO.CarrierTariffsPivot[] currentCarrierTariffsPivots =
                    CarrierTariffsPivotProcessorInstance.getNextCarrierTariffsPivotWithPrecision(ref nIndex, carrierTariffsPivot);

                Logger.Information("currentCarrierTariffsPivots found: {0}", currentCarrierTariffsPivots?.Count());

                ClassRatingLineAllocation LineAllocation = new ClassRatingLineAllocation(); // Need to pass this in but do not use for estimated results.

                // Loop through the carrier tariffs pivots
                while (currentCarrierTariffsPivots != null)
                {

                    Logger.Information("Processing {0} rate(s) for {CarrierName} tariff ID {CarrTarName}", currentCarrierTariffsPivots?.Count(), currentCarrierTariffsPivots[0].CarrierName, currentCarrierTariffsPivots[0].CarrTarName);
                    //Modified by RHR for v-7.0.6.0 on 11/7/2016
                    bool blnPreferred = false;
                    bool blnLimitToPreferred = false;
                    bool blnWarnOnRestrictCarrier = false;
                    if (blnTestPreferredCarriers == true)
                    {
                        if (preferredCarriers[0].CompRestrictCarrierSelection == true || preferredCarriers[0].LaneRestrictCarrierSelection == true)
                        {
                            Logger.Information("Preferred Carriers are on the carrier ({CompRestrictCarrierSelection}) or lane ({1}) restriction list...Setting blnLimitToPreferred=true", preferredCarriers[0].CompRestrictCarrierSelection, preferredCarriers[0].LaneRestrictCarrierSelection);
                            blnLimitToPreferred = true;
                        }

                        if (
                                (preferredCarriers[0].CompRestrictCarrierSelection == false && preferredCarriers[0].CompWarnOnRestrictedCarrierSelection == true)
                                ||
                                (
                                    preferredCarriers[0].CompRestrictCarrierSelection == false
                                    &&
                                    preferredCarriers[0].LaneRestrictCarrierSelection == false
                                    &&
                                    preferredCarriers[0].LaneWarnOnRestrictedCarrierSelection == true
                                 )
                            )
                        {
                            blnWarnOnRestrictCarrier = true;
                            Logger.Information("Preferred Carriers are on the carrier ({0}) or lane ({1}) warning list...Setting blnWarnOnRestrictCarrier=true", preferredCarriers[0].CompWarnOnRestrictedCarrierSelection, preferredCarriers[0].LaneWarnOnRestrictedCarrierSelection);
                        }
                        //Modified by RHR for v-8.5.3.003 on 06/23/2022 we now allow API as the Tariff name in the preferre carrier list. 
                        Logger.Information("Checking if carrier {CarrierName} is a preferred carrier for this load which checks if x.LLTCCarrierControl == currentCarrierTariffsPivots[0].CarrierControl & (x.LLTCIgnoreTariff == true || (x.LLTCTariffName == currentCarrierTariffsPivots[0].CarrTarName))", currentCarrierTariffsPivots[0].CarrierName);
                        if (preferredCarriers.Any(x =>
                                                    x.LLTCCarrierControl == currentCarrierTariffsPivots[0].CarrierControl
                                                    &
                                                    (
                                                        x.LLTCIgnoreTariff == true
                                                        ||
                                                        (x.LLTCTariffName == "API" || x.LLTCTariffName == currentCarrierTariffsPivots[0].CarrTarName)
                                                    ))
                            )
                        {
                            Logger.Information("Carrier {0} is a preferred carrier for this load", currentCarrierTariffsPivots[0].CarrierControl);
                            blnPreferred = true;
                        }
                    }
                    if (blnLimitToPreferred == false || blnLimitToPreferred == true && blnPreferred == true)
                    {
                        Logger.Information("blnLimitToPreferred is false or blnLimitToPreferred is true and blnPreferred is true");
                        // Rate using the current carrier tariffs pivot data.
                        Load ratedLoad = null;

                        results.AddLog("**************** Processing " + currentCarrierTariffsPivots.Count().ToString() + " rate(s) for " + currentCarrierTariffsPivots[0].CarrierName + " tariff ID " + currentCarrierTariffsPivots[0].CarrTarID + " ****************", currentCarrierTariffsPivots[0].CarrTarControl, "Tariff", DAL.Utilities.NGLMessageKeyRef.CarrierTariff);
                        // Pass the list in, regardless of it being one or multiple records.

                        Logger.Information("RateCarrierTariffInstance.rateCarrier for {0} rate(s) for {CarrierName} tariff ID {2}, TariffName: {TariffName}:{TariffRevision} - Equipment: {CarrierEquipment}/{EquipmentClass}", currentCarrierTariffsPivots?.Count(), currentCarrierTariffsPivots[0].CarrierName, currentCarrierTariffsPivots[0].CarrTarID, currentCarrierTariffsPivots[0].CarrTarName, currentCarrierTariffsPivots[0].CarrTarRevisionNumber, currentCarrierTariffsPivots[0].CarrTarEquipDescription, currentCarrierTariffsPivots[0].CarrTarEquipMatClass);

                        DTO.CarriersByCost carriersByCost = new DTO.CarriersByCost();

                        var rctInstance = RateCarrierTariffInstance;
                        rctInstance.Parameters = Parameters;
                        decimal currentTotalCost = rctInstance.rateCarrier(ref results, load, currentCarrierTariffsPivots, ref ratedLoad, ref LineAllocation, ref carriersByCost);
                        Logger.Information("RateCarrierTariffInstance.rateCarrier returned {0} for {CarrierName} tariff ID {2}, TariffName: {TariffName}:{TariffRevision} - Equipment: {CarrierEquipment}/{EquipmentClass}", currentTotalCost, currentCarrierTariffsPivots[0].CarrierName, currentCarrierTariffsPivots[0].CarrTarID, currentCarrierTariffsPivots[0].CarrTarName, currentCarrierTariffsPivots[0].CarrTarRevisionNumber, currentCarrierTariffsPivots[0].CarrTarEquipDescription, currentCarrierTariffsPivots[0].CarrTarEquipMatClass);
                        if (carriersByCost != null)
                        {
                            carriersByCost.WarnOnRestrictedCarrierSelection = blnWarnOnRestrictCarrier;
                            carriersByCost.BookRevPreferredCarrier = blnPreferred;
                            Logger.Information("CarriersByCost not null, setting BookRevPreferredCarrier to {0} and BookRevPreferredCarrier to {1}", blnWarnOnRestrictCarrier, blnPreferred);
                        }
                    }

                    // Get the next carrier tariffs pivot table.
                    Logger.Information("Getting next carrier tariffs pivot table from CarrierTariffsPivotProcessorInstance.getNextCarrierTariffsPivotWithPrecision({0}, {@1}).", nIndex, carrierTariffsPivot);

                    currentCarrierTariffsPivots =
                        CarrierTariffsPivotProcessorInstance.getNextCarrierTariffsPivotWithPrecision(
                            ref nIndex, carrierTariffsPivot);

                    Logger.Information("currentCarrierTariffsPivots found: {0}, nIndex: {1}", currentCarrierTariffsPivots?.Count(), nIndex);
                }
                //Populate the results with meta data.
                //TODO move this loop into the above loop instead of afterwards.  need to have the rateCarrier method result a result object instead of a decimal
                if (results.CarriersByCost != null && results.CarriersByCost.Count() > 0)
                {
                    Logger.Information("Carriers by Cost found, looping through to set HasAlert, HasInfo, AllowSelect, HasMessages");
                    foreach (DTO.CarriersByCost item in results.CarriersByCost)
                    {
                        if (item != null)
                        {
                            item.HasAlert = false;
                            item.HasInfo = false;
                            item.AllowSelect = false;
                            if (item.CarrierCost == 0)
                            {
                                //something is wrong this with record and no cost is returned.  notifiy the caller.
                                Logger.Information("CarrierCost is 0, setting HasAlert to true and AllowSelect to false");
                                item.AllowSelect = false;
                                item.HasAlert = true;
                            }
                            else
                            {
                                Logger.Information("CarrierCost is not 0, setting AllowSelect to true");
                                item.AllowSelect = true;
                                //has a carrier cost, but there are some messages
                                if (item.Messages != null && item.Messages.Count > 0)
                                {
                                    Logger.Information("Messages found, setting HasMessages to true");
                                    item.HasInfo = true;
                                }
                            }
                            if (item.Messages != null && item.Messages.Count > 0)
                            {
                                Logger.Information("Messages found, setting HasMessages to true");
                                item.HasMessages = true;
                            }
                        }
                    }
                }
                if (results.CarriersByCost != null && results.CarriersByCost.Count() > 0)
                {
                    Logger.Information("Carriers by Cost found, setting blnRet to true and sorting the results by cost.");
                    blnRet = true;
                    results.CarriersByCost = results.CarriersByCost.OrderBy(x => x.CarrierCost).ToList();
                }
            }
            return blnRet;
        }

        /// <summary>
        /// TODO:  compare this logic with the new Load Tender Accept Reject logic and the old logic in 6.0.x
        ///        not sure if this logic works as expected.
        /// TODO: add logic to update carrier contacts if available.
        /// TODO: if carrier contract obj is null, select carrier contact data from carrier contact data.
        /// </summary>
        /// <param name="load"></param>
        /// <param name="ratedLoad"></param>
        protected void UpdateBookRevDefaultsBeforeSave(Load load, ref Load ratedLoad)
        {
            using (var operation = Logger.StartActivity("UpdateBookRevDefaultsBeforeSave(Load: {@Load}, RatedLoad: {@RatedLoad})", load, ratedLoad))
            {
                if (ratedLoad == null)
                {
                    ratedLoad = DeepCopy(load);
                    ratedLoad.Parameters = load.Parameters;
                    foreach (DTO.BookRevenue ratedLoadBookRev in ratedLoad.BookRevenues)
                    {
                        Logger.Information("ratedLoadBookRev.BookCarrierControl: {0}", ratedLoadBookRev.BookCarrierControl);
                        ratedLoadBookRev.ResetToNStatus();

                    }
                }
                else
                {
                    List<string> sFaultInfo = new List<string>();

                    DTO.CarrierCont CarrierCont = null;
                    if (ratedLoad.BookRevenues[0].BookCarrierControl != 0)
                    {
                        CarrierCont = getCarrierCont(ratedLoad.BookRevenues[0].BookCarrierControl, ref sFaultInfo);
                    }

                    foreach (DTO.BookRevenue ratedLoadBookRev in ratedLoad.BookRevenues)
                    {
                        if (CarrierCont != null)
                        {
                            ratedLoadBookRev.BookCarrierContControl = CarrierCont.CarrierContControl;
                            ratedLoadBookRev.BookCarrierContact = CarrierCont.CarrierContName;
                            if (string.IsNullOrWhiteSpace(CarrierCont.CarrierContact800))
                            {
                                ratedLoadBookRev.BookCarrierContactPhone = CarrierCont.CarrierContactPhone;
                            }
                            else
                            {
                                ratedLoadBookRev.BookCarrierContactPhone = CarrierCont.CarrierContact800;
                            }
                        }
                        else
                        {
                            ratedLoadBookRev.BookCarrierContControl = 0;
                            ratedLoadBookRev.BookCarrierContact = null;
                            ratedLoadBookRev.BookCarrierContactPhone = null;
                        }
                        DTO.BookRevenue originalBookRev = load.BookRevenues.Where(x => x.BookControl == ratedLoadBookRev.BookControl).First();
                        if ((ratedLoadBookRev.BookCarrierControl != originalBookRev.BookCarrierControl) &&
                            (ratedLoadBookRev.BookCarrierControl == 0))
                        {
                            ratedLoadBookRev.ResetToNStatus();
                        }
                        else if ((ratedLoadBookRev.BookCarrierControl != originalBookRev.BookCarrierControl) &&
                                 (ratedLoadBookRev.BookCarrierControl != 0))
                        {
                            ratedLoadBookRev.BookTranCode = "P";
                        }
                        else if ((ratedLoadBookRev.BookCarrierControl == originalBookRev.BookCarrierControl) &&
                                 (originalBookRev.BookCarrierControl != 0) &&
                                 (originalBookRev.BookTranCode == "N"))
                        {
                            ratedLoadBookRev.BookTranCode = "P";
                        }
                    }
                }
            }
        }

        protected void SetAssignedCarrier(ref Load load, int carrierControl)
        {
            foreach (DTO.BookRevenue bookrev in load.BookRevenues)
            {
                bookrev.BookCarrierControl = carrierControl;
            }
        }

        /// <summary>
        /// Select the assigned carrier and look up the cost information using rate precision.
        /// </summary>
        /// <param name="bookRevenue">BookRevenue object we are assigning the carrier to.</param>
        /// <param name="carrierTariffsPivot">Collection of carrier tariffs pivot data.</param>
        /// <returns>Collection of fees for the selected carrier.</returns>
        protected Load selectAssignedCarrier(
            Load load,
            int carrierControl,
            DTO.CarrierTariffsPivot[] carrierTariffsPivot)
        {
            Load ratedLoad = null;

            // TODO Add code to assign the carrier.

            return ratedLoad;
        }


        public DTO.BookRevenue getBookRevData(int bookControl)
        {
            DTO.BookRevenue result = null;      // nothing found.

            try
            {
                result = NGLBookRevenueData.GetBookRevenueFiltered(bookControl);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getBookRevData"), bookControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return result;
        }



        private DAL.LTS.spGetFuelSurchargeResult GetFuelSurcharge(int CarrierControl, int CarrTarControl, int CarrTarEquipControl, string STATE, DateTime EffectiveDate)
        {
            using (var operation = Logger.StartActivity("GetFuelSurcharge(CarrierControl: {CarrierControl}, CarrTarControl: {CarrTarControl}, CarrTarEquipControl: {CarrTarEquipControl}, STATE: {STATE}, EffectiveDate: {EffectiveDate})", CarrierControl, CarrTarControl, CarrTarEquipControl, STATE, EffectiveDate))
            {
                try
                {

                    Logger.Information("GetFuelSurcharge for CarrierControl: {CarrierControl}, CarrTarControl: {CarrTarControl}, CarrTarEquipControl: {CarrTarEquipControl}, STATE: {STATE}, EffectiveDate: {EffectiveDate}", CarrierControl, CarrTarControl, CarrTarEquipControl, STATE, EffectiveDate);
                    //Logger.Information("GetFuelSurcharge", "CarrierControl: " + CarrierControl.ToString() + " CarrTarControl: " + CarrTarControl.ToString() + " CarrTarEquipControl: " + CarrTarEquipControl.ToString() + " STATE: " + STATE + " EffectiveDate: " + EffectiveDate.ToString());
                    return NGLCarrierFuelAddendumData.GetFuelSurCharge(CarrierControl, CarrTarControl, CarrTarEquipControl, STATE, EffectiveDate);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    Logger.Error(sqlEx, "FaultException in GetFuelSurcharge.");
                    //most WCF services throw a FaultException these are typically data access errors
                    //caller will need to handle a return value of null or more logic can be added to this method
                    //string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    //SaveSysError(errMsg, sourcePath("GetFuelSurcharge"), CarrierControl.ToString());
                }
                catch (FaultException<DAL.ConflictFaultInfo> sqlEX)
                {
                    Logger.Error(sqlEX, "FaultException in GetFuelSurcharge.");
                    //Optimistic Concurrancy Conflit Exceptions
                    //we need to determine what to do here for unattended execution.  
                    //as special method may be needed to clone the current data,  get the latest version and try again
                    // for now we just log the error and return nothing
                    //   SaveAppError(string.Concat(sourcePath("CarrierControl"), sqlEX.Reason.ToString(), sqlEX.Detail.Message));
                }
                catch (InvalidOperationException ex)
                {
                    Logger.Error(ex, "InvalidOperationException in GetFuelSurcharge.");
                    //this is an example of saving the application error then fixing the problem and trying again
                    //SaveAppError(ex.Message);
                    //if (ex.Message == "Parameters Are Required")
                    //{

                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Exception in GetFuelSurcharge.");
                    //the caller will need to handle the unhandled exceptions
                    // throw;
                }
                Logger.Warning("GetFuelSurcharge returning null.");
            }
            return null;

        }

        /// <summary>
        /// Allocate the Load BFC by weight.
        /// This method calls the spAllocateLoadBFCByWgt stored procedure.
        /// </summary>
        /// <param name="bookControl"></param>
        /// <returns>true if successful, false if not.</returns>
        private bool execspAllocateLoadBFCByWgt(int bookControl)
        {
            try
            {
                List<DTO.NGLStoredProcedureParameter> oSPPars = new List<DTO.NGLStoredProcedureParameter>();
                oSPPars.Add(new DTO.NGLStoredProcedureParameter { ParName = "@BookControl", ParValue = bookControl.ToString() });
                return executeNGLStoredProcedure("spAllocateLoadBFCByWgt", oSPPars);
            }
            catch (InvalidOperationException ex)
            {
                //this is an example of saving the application error then fixing the problem and trying again
                if (ex.Message == "Parameters Are Required")
                {
                    SaveAppError(ex.Message);
                    //this needs to be replace by code to correct the problem with the parameter data
                    populateSampleParameterData();
                    execspAllocateLoadBFCByWgt(bookControl);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }


        /// <summary>
        /// Allocate the Load Total Cost by weight.
        /// This method calls the spAllocateLoadTotalCostByWgt stored procedure.
        /// </summary>
        /// <param name="bookControl"></param>
        /// <returns>true if successful, false if not.</returns>
        private bool execspAllocateLoadTotalCostByWgt(int bookControl)
        {
            using (var operation = Logger.StartActivity("execspAllocateLoadTotalCostByWgt(BookControl: {BookControl})", bookControl))
            {
                try
                {
                    List<DTO.NGLStoredProcedureParameter> oSPPars = new List<DTO.NGLStoredProcedureParameter>();
                    oSPPars.Add(new DTO.NGLStoredProcedureParameter { ParName = "@BookControl", ParValue = bookControl.ToString() });
                    return executeNGLStoredProcedure("spAllocateLoadTotalCostByWgt", oSPPars);
                }
                catch (InvalidOperationException ex)
                {
                    //this is an example of saving the application error then fixing the problem and trying again
                    if (ex.Message == "Parameters Are Required")
                    {
                        SaveAppError(ex.Message);
                        //this needs to be replace by code to correct the problem with the parameter data
                        populateSampleParameterData();
                        execspAllocateLoadTotalCostByWgt(bookControl);
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// this is a simple example of calling a WCF method to read data into DTO object.
        /// the BookRevenue object has all the data fields needed for the spCalcBookRevBFC procedure
        /// once you have the data object you can update the data fields and save the changes 
        /// (see saveBookRevData method
        /// </summary>
        /// <param name="BookControl"></param>
        /// <returns></returns>
        protected DTO.BookRevenue saveBookRevData(DTO.BookRevenue oData)
        {
            DTO.BookRevenue result = null;
            using (var operation = Logger.StartActivity("saveBookRevData"))
            {

                try
                {
                    Logger.Information("NGL.FM.CarTar.BookRev.saveBookRevData({@oData})", oData);
                    result = (DTO.BookRevenue)NGLBookRevenueData.UpdateRecord(oData);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    Logger.Error(sqlEx, "FaultException in saveBookRevData.");
                    //most WCF services throw a FaultException these are typically data access errors
                    //caller will need to handle a return value of null or more logic can be added to this method
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getBookRevData"), oData.BookControl.ToString());
                }
                catch (FaultException<DAL.ConflictFaultInfo> sqlEX)
                {
                    Logger.Error(sqlEX, "FaultException in saveBookRevData.");
                    //Optimistic Concurrancy Conflit Exceptions
                    //we need to determine what to do here for unattended execution.  
                    //as special method may be needed to clone the current data,  get the latest version and try again
                    // for now we just log the error and return nothing
                    SaveAppError(string.Concat(sourcePath("getBookRevData"), sqlEX.Reason.ToString(), sqlEX.Detail.Message));
                }
                catch (InvalidOperationException ex)
                {
                    //this is an example of saving the application error then fixing the problem and trying again
                    if (ex.Message == "Parameters Are Required")
                    {
                        Logger.Fatal(ex, "This is bad.");
                        SaveAppError(ex.Message);
                        //this needs to be replace by code to correct the problem with the parameter data
                        populateSampleParameterData();
                        saveBookRevData(oData);
                    }
                    else
                    {
                        Logger.Error(ex, "InvalidOperationException in saveBookRevData.");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    //the caller will need to handle the unhandled exceptions
                    throw;
                }
            }
            return result;

        }

        /// <summary>
        /// deletes appropriate fees, add the fees as needed based on the fee
        /// the rule is we add the Flat Fuel Charge fee to all of the orders
        /// if Use Carrier Fuel Addendum, it is a tariff/carrier fee and all other fuel fees should not be present.
        /// 
        /// </summary>
        /// <param name="load"></param>
        /// <param name="spotRate"></param>
        /// <returns></returns>
        private Load manageSpotRateFees(Load load, DTO.SpotRate spotRate)
        {
            Logger.Information("manageSpotRateFees(Load: {@Load}, SpotRate {@spotRate}", load, spotRate);
            //first delete all fees from load
            if (spotRate.DeleteOrderFees)
            {
                Logger.Information("Deleting Order Accessorial Fees on Order...");
                load.deleteFees(DAL.Utilities.AccessorialFeeType.Order);
            }

            DTO.BookFee carFuelFee = null;
            //if they want to use carrier fuel addendum, then we need to create a fuel fee for them, and remove any other fuel fees they have in the list.

            Logger.Information("Check if Use Carrier Fuel Addendum ({UseCarrierFuelAddendum}) is true...", spotRate.UseCarrierFuelAddendum);
            if (spotRate.UseCarrierFuelAddendum)
            {
                try
                {
                    Logger.Warning("Using Carrier Fuel Addendum... so remove other Fuel Surcharge Fees.");
                    //Remove any other fuel fees that they have put in there.
                    spotRate.BookFees.RemoveAll(i => i.BookFeesAccessorialCode == 9 || i.BookFeesAccessorialCode == 2 || i.BookFeesAccessorialCode == 15);

                    // DTO.BookRevenue lastStop = load.BookRevenues.OrderByDescending(i => i.BookStopNo).FirstOrDefault();
                    LTS.udfGetTariffSelectionKeysResult lastStop = NGLBookData.GetLastStopData(load.BookRevenues[0].BookControl);
                    if (lastStop == null)
                    {
                        Logger.Error("Unable to Get Last Stop in manageSpotRateFees");
                        throw new FaultException("Unable to Get Last Stop");
                    }
                    DTO.BookRevenue earliestShipDate = load.BookRevenues.OrderBy(i => i.BookDateLoad).FirstOrDefault();


                    Logger.Information("Checking if LastStop ({LastStop}) has RatedBookControl != 0 ({LastStopHasBookControl}) and if Earliest Ship Date {EarliestShipDate} has BookControl ({EarlistShipDateBookControl}) != 0 ", lastStop, lastStop?.RatedBookControl != 0, earliestShipDate, earliestShipDate?.BookControl);

                    if (lastStop != null && lastStop.RatedBookControl != 0 && earliestShipDate != null && earliestShipDate.BookControl != 0)
                    {
                        spotRate.State = lastStop.State;
                        spotRate.EffectiveDate = earliestShipDate.BookDateLoad.Value;
                        Logger.Information("Getting Fuel Surcharge from Addendum for CarrierControl: {CarrierControl}, CarrTarControl: 0, CarrierEquipment: 0, State: {State}, EffectiveDate: {EffectiveDate}", spotRate.CarrierControl, spotRate.State, spotRate.EffectiveDate);
                        DAL.LTS.spGetFuelSurchargeResult result = GetFuelSurcharge(spotRate.CarrierControl, 0, 0, spotRate.State, spotRate.EffectiveDate);
                        if (result.ErrNumber != 0)
                        {
                            Logger.Error("Error in manageSpotRateFees: {ErrorNumber} - {ErrorMessage}", result.ErrNumber, result.RetMsg);
                            //there is a problem
                            throw new FaultException<DAL.SqlFaultInfo>(new DAL.SqlFaultInfo(), new FaultReason(result.RetMsg));
                        }
                        ////success
                        //create the new fuel fee 
                        DTO.tblAccessorial fuelFee = null;
                        if (result.UseRatePerMile.Value)
                        {
                            fuelFee = NGLtblAccessorialData.GettblAccessorialFiltered(9);
                            Logger.Information("Using Rate Per Mile for Fuel Surcharge, setting Fuel Fee to {Fuel}", fuelFee);
                        }
                        else
                        {
                            fuelFee = NGLtblAccessorialData.GettblAccessorialFiltered(2);
                            Logger.Information("Using Flat Rate for Fuel Surcharge, setting Fuel Fee to {Fuel}", fuelFee);
                        }

                        carFuelFee = DAL.NGLtblAccessorialData.ConvertToBookFee(fuelFee, (int)DAL.Utilities.AccessorialFeeType.Tariff);

                        carFuelFee.BookFeesVariable = (double)result.FuelSurcharge.Value;
                        carFuelFee.BookFeesBookControl = load.BookRevenues[0].BookControl;//set it to the first record

                        Logger.Information("Creating BookFee for Fuel: {@BookFee}", carFuelFee);
                        //we will add the fee to the book record last.
                        //spotRate.BookFees.Add(carFuelFee);
                        //set the avg fuel price to display for the users
                        spotRate.AvgFuelPrice = (decimal)result.FuelSurcharge.Value;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error in manageSpotRateFees");
                    throw ex;
                }
            }

            Logger.Information("Adding Fees to Orders...");
            if (spotRate.BookFees != null && spotRate.BookFees.Count() > 0 && load.BookRevenues.Count() > 0)
            {
                //now add the book fees they want.                
                //delete all other fuel fees becasue they are using the carrier fuel addendem
                for (int i = 0; i < spotRate.BookFees.Count; i++)
                {
                    //set the fee type contol on the fees. all fees are order fee except for Use Carrier Fuel Addendum(which is the flat charge)?
                    //if (spotRate.BookFees[i].BookFeesAccessorialCode == 9 || spotRate.BookFees[i].BookFeesAccessorialCode == 2)//need a fee for Carrier Fuel Addendum
                    //{
                    //    spotRate.BookFees[i].BookFeesAccessorialFeeTypeControl = (int)DAL.Utilities.AccessorialFeeType.Tariff;//set it as tariff/carrier specific 
                    //}
                    //else
                    //{
                    //every fee should be order specfic except for the Carrier Fuel Addendum fee, which we will add last after the loop.
                    spotRate.BookFees[i].BookFeesAccessorialFeeTypeControl = (int)DAL.Utilities.AccessorialFeeType.Order;//set as order specific
                                                                                                                         //}

                    //if it is a flat fuel change(new fee), Add the fee to all orders (do not worry about allocation)
                    if (spotRate.BookFees[i].BookFeesAccessorialCode == 15)//flat fuel fee code: 15
                    {
                        foreach (DTO.BookRevenue bookRevenue in load.BookRevenues)
                        {
                            bookRevenue.BookFees.Add(spotRate.BookFees[i]);
                        }
                    }
                    else
                    {
                        //all other fees should be assocaited with the bookcontrol the user has selected.                            
                        foreach (DTO.BookRevenue bookRevenue in load.BookRevenues)
                        {//find the bookrevenue object with the correct book control and add it.
                            if (bookRevenue.BookControl == spotRate.BookFees[i].BookFeesBookControl)
                            {
                                bookRevenue.BookFees.Add(spotRate.BookFees[i]);
                            }
                        }
                    }
                }
            }
            //Finally if they want a carrier Fuel Addendum fee, we add it here becasue it is tariff specfic it was easier to add it after the loop.
            if (spotRate.BookFees != null && load.BookRevenues.Count() > 0 && spotRate.UseCarrierFuelAddendum && carFuelFee != null)
            {
                Logger.Information("Adding Carrier Fuel Addendum Fee to First Order.");
                //Add it to the first one as we set the book control for it earier.
                load.BookRevenues[0].BookFees.Add(carFuelFee);
            }
            return load;
        }


        #endregion

        #region " Test Methods"

        /// <summary>
        /// Test method to show how to throw the following exceptons
        /// InvalidTranCodeException  was called Not Allowed Exception
        /// NoTariffAvailableException
        /// CostLockedException was called Not Allowed Exception
        /// previous documentation If CarrierControl = 0 throw Not Allowed Exception and return has been changed to 
        /// NoTariffAvailableException
        /// </summary>
        public void testThrowExceptions()
        {
            try
            {
                int errType = 0;
                switch (errType)
                {
                    case 0:
                        //shortcut overload with example of how to skip applicaiton error log (non-typical). 
                        //was called Not Allowed Exception      
                        NGLBookRevenueData.throwInvalidTranCodeException("IC", false);
                        break;
                    case 1:
                        //shortcut overload
                        NGLBookRevenueData.throwNoTariffAvailableException("CNS1234");
                        break;
                    case 2:
                        //shortcut overload.  Was called Not Allowed Exception
                        NGLBookRevenueData.throwCostLockedException("PRO12345");
                        break;
                    case 3:
                        //full exception method with system error logging
                        List<string> msgDetails = new List<string>();
                        msgDetails.Add("CNS1234");
                        int LineNbr = 225;
                        int ErrNbr = 456;
                        NGLBookRevenueData.throwFaultException(DAL.SqlFaultInfo.FaultInfoMsgs.E_AssignCarrierFailed,
                                               DAL.SqlFaultInfo.FaultDetailsKey.E_NoTariffAvailable,
                                               msgDetails,
                                                  DAL.SqlFaultInfo.FaultReasons.E_DataValidationFailure,
                                                  "sysMessage",
                                                  "Procedure Name",
                                                  "Record or Data Information",
                                                  DAL.sysErrorParameters.sysErrorSeverity.Critical,
                                                  DAL.sysErrorParameters.sysErrorState.SystemLevelFault,
                                                  LineNbr,
                                                  ErrNbr);
                        break;
                    case 4:
                        //full exception method with just an application error log 
                        NGLBookRevenueData.throwFaultException(DAL.SqlFaultInfo.FaultInfoMsgs.E_ApplicationException,
                            DAL.SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails,
                            "Message String",
                            DAL.SqlFaultInfo.FaultReasons.E_InvalidOperationException,
                            true);
                        break;
                    default:
                        //sample unexpected error exception with a message
                        NGLBookRevenueData.throwUnExpectedFaultException("Message");
                        break;
                }
                return;
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                //this is critical because the above error codes will throw a FaultException
                throw;
            }
            catch (Exception ex)
            {
                // Example of how to throw an unexpected system exeception (Normally we just throw this back to the caller)
                //but there may be some reasons to use this
                NGLBookRevenueData.throwUnExpectedFaultException(ex,
                    "Procedure Name",
                    "Record or Data Info",
                    DAL.sysErrorParameters.sysErrorSeverity.Unexpected,
                    DAL.sysErrorParameters.sysErrorState.UserLevelFault);
                return;
            }
        }

        /// <summary>
        /// This method will need to be modified in WCF to call the new NGL.FM.CarTar.CalcBookRev method because it uses the stored procedure.
        /// </summary>
        /// <param name="oData"></param>
        /// <param name="AllocateBFC"></param>
        /// <param name="UpdateBFC"></param>
        /// <returns></returns>
        public DTO.BookRevenue saveAndCalcBookRev(DTO.BookRevenue oData, bool AllocateBFC = true, bool UpdateBFC = true)
        {
            try
            {
                return NGLBookRevenueData.SaveAndCalcRevenue(oData, AllocateBFC, UpdateBFC);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                //most WCF services throw a FaultException these are typically data access errors
                //caller will need to handle a return value of null or more logic can be added to this method
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                SaveSysError(errMsg, sourcePath("getBookRevData"), oData.BookControl.ToString());
            }
            catch (InvalidOperationException ex)
            {
                //this is an example of saving the application error then fixing the problem and trying again
                if (ex.Message == "Parameters Are Required")
                {
                    SaveAppError(ex.Message);
                    //this needs to be replace by code to correct the problem with the parameter data
                    populateSampleParameterData();
                    saveAndCalcBookRev(oData, AllocateBFC, UpdateBFC);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                //the caller will need to handle the unhandled exceptions
                throw;
            }
            return null;
        }


        /// <summary>
        /// When calling executeNGLStoredProcedure the following parameters
        /// are required by the stored procedure but are implemented in the WCF service
        /// so they should not be included in the ProcPars array:
        /// @UserName NVARCHAR (100), @RetMsg NVARCHAR (500) OUTPUT, @ErrNumber INT OUTPUT 
        /// </summary>
        /// <param name="BookControl"></param>
        /// <param name="UpdateBFC"></param>
        /// <returns></returns>
        public bool execspCalcBookRevBFC(int BookControl, bool UpdateBFC)
        {
            try
            {
                List<DTO.NGLStoredProcedureParameter> oSPPars = new List<DTO.NGLStoredProcedureParameter>();
                oSPPars.Add(new DTO.NGLStoredProcedureParameter { ParName = "@BookControl", ParValue = BookControl.ToString() });
                string sUpdateBFC = "0";
                if (UpdateBFC == true) { sUpdateBFC = "1"; }
                oSPPars.Add(new DTO.NGLStoredProcedureParameter { ParName = "@UpdateBFC", ParValue = sUpdateBFC });
                return executeNGLStoredProcedure("spCalcBookRevBFC", oSPPars);
            }
            catch (InvalidOperationException ex)
            {
                //this is an example of saving the application error then fixing the problem and trying again
                if (ex.Message == "Parameters Are Required")
                {
                    SaveAppError(ex.Message);
                    //this needs to be replace by code to correct the problem with the parameter data
                    populateSampleParameterData();
                    execspCalcBookRevBFC(BookControl, UpdateBFC);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;


        }

        #endregion
    }
}
