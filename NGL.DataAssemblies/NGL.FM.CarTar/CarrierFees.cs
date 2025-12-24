using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;
using Destructurama;
using Destructurama.Attributed;
using Serilog.Context;
using SerilogTracing;

namespace NGL.FM.CarTar
{
    public class CarrierFees : TarBaseClass
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarrierFees(DAL.WCFParameters oParameters)
            : base()
        {
            if (oParameters == null)
            {
                populateSampleParameterData();  //this will not really be used in production
            }
            else
            {
                Parameters = oParameters;
            }

            Logger = Logger.ForContext<CarrierFees>();
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrierFees";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        private Dictionary<Tuple<int, int>, List<DTO.CarrTarFee>> _dictCarrTarFeesForLane = null;
        /// <summary>
        /// Stores a list of DTO.CarrTarFee in a Dictionary using a Tuple<LaneControl, CarrTarControl> as the key
        /// </summary>
        public Dictionary<Tuple<int, int>, List<DTO.CarrTarFee>> DictCarrTarFeesForLane
        {
            get
            {
                if (_dictCarrTarFeesForLane == null)
                {
                    // Create a new dictionary.
                    _dictCarrTarFeesForLane = new Dictionary<Tuple<int, int>, List<DTO.CarrTarFee>>();
                }
                return _dictCarrTarFeesForLane;
            }
            set { _dictCarrTarFeesForLane = value; }
        }


        #endregion

        #region " Methods"

        /// <summary>
        /// Calculate all tariff and non-tariff fees where BookFeesIsTax == false (0) and allocate them back to the BookRevenue objects.
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number for current carrier tariff.</param>
        /// <param name="ratedLoad">Load information and rating results.</param>
        /// <remarks>
        /// Modified by RHR v-7.0.5.102 8/9/2016
        ///     Added logic to apply filter where BookFeesIsTax == false (0)
        ///     we now calculate the taxes later after all other fees have been updated.
        /// Modified by RHR for v-7.0.5.103 on 1/23/2016 
        ///  we now replace all the lanes when we recalculate carreir costs
        /// </remarks>

        public void calculateAndAllocateAllFees(int carrTarControl, ref Load ratedLoad, ref DTO.CarrierCostResults results, ref DTO.CarriersByCost carriersByCost)
        {

            using (var operation = Logger.StartActivity("calculateAndAllocateAllFees(carrTarControl: {CarrTarControl}, RatedLoad: {@RatedLoad}, Results: {@Results}, CarriersByCost: {@CarriersByCost})", carrTarControl, ratedLoad, results, carriersByCost))
            {
                if (ratedLoad == null)
                {
                    operation.Complete();
                    return;
                }

                // Add the CarrTarFee data, for the current carrier, to the BookFee collection of the BookRevenue objects.
                //Modified by RHR 9/19/14 we only update the tariff fees when assigning or updating the tariff not when we
                //are recalculating the costs
                Logger.Information("Checking if ratedLoad.CalculationType == DAL.Utilities.AssignCarrierCalculationType.Normal || ratedLoad.CalculationType == DAL.Utilities.AssignCarrierCalculationType.UpdateAssignedCarrier || ratedLoad.CalculationType == DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier");
                if (ratedLoad.CalculationType == DAL.Utilities.AssignCarrierCalculationType.Normal || ratedLoad.CalculationType == DAL.Utilities.AssignCarrierCalculationType.UpdateAssignedCarrier || ratedLoad.CalculationType == DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier || ratedLoad.CalculationType == DAL.Utilities.AssignCarrierCalculationType.RateShopOnly)
                {
                    Logger.Information("Adding Tariff Fees to BookFees");
                    addTariffFeeDataToBookFees(carrTarControl, ref ratedLoad, ref results, ref carriersByCost);
                    //Modified by RHR for v-7.0.5.103 on 1/23/2016 
                    //  we now replace all the lanes when we recalculate carreir costs
                    Logger.Information("Adding Lane Fees to BookFees");
                    addLaneFeeDataToBookFees(ref ratedLoad, ref results, ref carriersByCost);
                }
                // Validate and adjust the BookFee information for conflicts.
                List<DTO.BookRevenue> bookRevenueList = ratedLoad.BookRevenues.ToList();
                bool result = NGLBookRevenueData.AccessorialFeeValidation(ref bookRevenueList);
                ratedLoad.BookRevenues = bookRevenueList.ToArray();

                // Clear out the value on all the BookFee objects so we know which ones have been recalculated.
                foreach (DTO.BookRevenue bookRevenue in ratedLoad.BookRevenues)
                {
                    foreach (DTO.BookFee bookFee in bookRevenue.BookFees)
                    {
                        if (bookFee.BookFeesOverRidden == false)
                        {
                            bookFee.BookFeesValue = 0;
                        }
                    }
                }

                // Loop through each of the BookFee objects, recalculate the fee and allocate it back to the orders.
                foreach (DTO.BookRevenue bookRevenue in ratedLoad.BookRevenues)
                {
                    //Modified by RHR v-7.0.5.102 8/9/2016 BookFeesIsTax == false
                    foreach (DTO.BookFee bookFee in bookRevenue.BookFees.Where(x => x.BookFeesIsTax == false))
                    {
                        Logger.Information("Checking if BookFeesOverRidden ({BookFeesOverRidden}) == false and BookFeesValue ({BookFeesValue}) == 0 and BookFeesAccessorialCode ({BookFeesAccessorialCode}) > 9 and BookFeesAccessorialCode ({BookFeesAccessorialCode}) < 20 or BookFeesAccessorialCode ({BookFeesAccessorialCode}) > 28", bookFee.BookFeesOverRidden, bookFee.BookFeesValue, bookFee.BookFeesAccessorialCode, bookFee.BookFeesAccessorialCode);
                        if ((bookFee.BookFeesOverRidden == false)
                            && (bookFee.BookFeesValue == 0)
                            && ((bookFee.BookFeesAccessorialCode > 9 && bookFee.BookFeesAccessorialCode < 20) || bookFee.BookFeesAccessorialCode > 28) //exclude fuel and pick and stop charges
                           )
                        {
                            Logger.Information("Calculating and Allocating Fee - EXCLUDING anything 9 > and < 20");
                            calculateAndAllocateFee(bookRevenue, bookFee, ref ratedLoad);
                        }
                        else
                        {
                            Logger.Information("Checking if BookFeesValue ({BookFeesValue}) == 0 and BookFeesAccessorialCode ({BookFeesAccessorialCode}) == 2 or BookFeesAccessorialCode ({BookFeesAccessorialCode}) == 9 and BookFeesOverRidden ({BookFeesOverRidden}) == false", bookFee.BookFeesValue, bookFee.BookFeesAccessorialCode, bookFee.BookFeesOverRidden);
                            if (bookFee.BookFeesValue == 0 && (bookFee.BookFeesAccessorialCode == 2 || bookFee.BookFeesAccessorialCode == 9) && bookRevenue.BookFees.Any(x => x.BookFeesAccessorialCode == 15 && x.BookFeesOverRidden == false))
                            {
                                Logger.Information("Setting BookFeesOverRidden to true");
                                bookFee.BookFeesOverRidden = true;
                            }
                        }
                    }
                }

            }
        }


        /// <summary>
        /// Calculate Tax tariff and non-tariff fees where BookFeesIsTax == true (1) and allocate them back to the BookRevenue objects.
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number for current carrier tariff.</param>
        /// <param name="ratedLoad">Load information and rating results.</param>
        /// <remarks>
        /// Created by RHR v-7.0.5.102 8/9/2016
        ///     Added logic to apply filter where BookFeesIsTax == true (1)
        ///     we now calculate the taxes after all other fees have been updated.
        /// </remarks>
        public void calculateAndAllocateTaxFees(int carrTarControl, ref Load ratedLoad, ref DTO.CarrierCostResults results, ref DTO.CarriersByCost carriersByCost)
        {
            using (Logger.StartActivity("calculateAndAllocateTaxFees(carrTarControl: {CarrTarControl}, Load: {@RatedLoad}, Results: {@Results}, CarriersByCost: {@CarriersByCost}", carrTarControl, ratedLoad, results, carriersByCost))
            {
                if (ratedLoad == null)
                {
                    return;
                }

                // Loop through each of the BookFee objects, recalculate the fee and allocate it back to the orders.
                foreach (DTO.BookRevenue bookRevenue in ratedLoad.BookRevenues)
                {
                    //Modified by RHR v-7.0.5.102 8/9/2016 BookFeesIsTax == true
                    foreach (DTO.BookFee bookFee in bookRevenue.BookFees.Where(x => x.BookFeesIsTax == true))
                    {
                        Logger.Information("Checking if BookFeesOverriden not ({BookFeesOverRidden}) AND BookFeesValue ({BookFeesValue}) = 0 AND (BookFeesAccessorialCode (20 < {BookFeesAccessorialCode} > 9) OR BookFeesAccesorialCode ({BookFeesAccessorialCode} > 28",
                            !bookFee.BookFeesOverRidden,
                            bookFee.BookFeesValue,
                            bookFee.BookFeesAccessorialCode,
                            bookFee.BookFeesAccessorialCode);

                        if ((bookFee.BookFeesOverRidden == false)
                            && (bookFee.BookFeesValue == 0)
                            && ((bookFee.BookFeesAccessorialCode > 9 && bookFee.BookFeesAccessorialCode < 20) || bookFee.BookFeesAccessorialCode > 28) //exclude fuel and pick and stop charges
                           )
                        {
                            calculateAndAllocateFee(bookRevenue, bookFee, ref ratedLoad);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Get the tariff fee data, for all applicable tariff fees, and add that data as BookFee objects on the BookRevenue objects.
        /// however if is spot rate: do not use tariffs, so lets skip this. spot rates handle fees differently.
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number for current carrier tariff.</param>
        /// <param name="ratedLoad">Load data and rating results.</param>
        /// <remarks>
        /// 
        /// </remarks>
        private void addTariffFeeDataToBookFees(int carrTarControl, ref Load ratedLoad, ref DTO.CarrierCostResults results, ref DTO.CarriersByCost carriersByCost)
        {


            using (var operation = Logger.StartActivity("addTariffFeeDataToBookFees(carrTarControl: {CarrTarControl}, RatedLoad: {RatedLoad}, Results: {Results}, CarriersByCost: {CarriersByCost})", carrTarControl, ratedLoad, results, carriersByCost))

            {
                if (ratedLoad == null || carrTarControl == 0) //spot rates do not use tariffs, so lets skip this.
                {
                    operation.Complete();
                    return;
                }

                // Remove all existing tariff fees from the load, leaving only the order and lane fees.
                ratedLoad.deleteCarrierFees();

                //modified by RHR 9/19/14  we now check the ratedLoad.ProfileFees property and call getCarrTarFeesWithProfileFees if possible.  typically used for Rate Shopping
                foreach (DTO.BookRevenue bookRevenue in ratedLoad.BookRevenues)
                {
                    using (Logger.StartActivity("Iterating through BookRevenue: {BookRevenue} and checking if ratedload has ProfileFees ({ProfileFeeCount})", bookRevenue, ratedLoad.ProfileFees?.Count()))
                    {
                        List<string> sFaultInfo = new List<string>();
                        List<DTO.CarrTarFee> carrTarFees = new List<DTO.CarrTarFee>();
                        if (ratedLoad.ProfileFees != null && ratedLoad.ProfileFees.Count() > 0)
                        {
                            carrTarFees =
                                getCarrTarFeesWithProfileFees(ratedLoad.ProfileFees, carrTarControl, ref sFaultInfo);

                            Logger.Information("getCarrTarFeesWithProfileFees returned {CarrTarFeesCount} records", carrTarFees?.Count());
                        }
                        else
                        {

                            carrTarFees = getCarrTarFeesByLane(bookRevenue.BookODControl, carrTarControl, ref sFaultInfo,
                                bookRevenue.BookControl);

                            Logger.Information("getCarrTarFeesByLane returned {CarrTarFeesCount} records", carrTarFees?.Count());
                        }

                        if (sFaultInfo != null && sFaultInfo.Count() > 0)
                        {
                            Logger.Error("getCarrTarFeesByLane returned a fault: {@FaultInfo}", sFaultInfo);
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_SQLFaultCannotReadCarrierFees,
                                sFaultInfo);
                            results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
                        }
                        else
                        {
                            if (carrTarFees != null && carrTarFees.Count() > 0)
                            {
                                Logger.Information("Checking if any tariff fees are not supported ({AnyFeesNotSupported})", carrTarFees.Any(x => x.NotSupported == true));

                                if (carrTarFees.Any(x => x.NotSupported == true))
                                {
                                    carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_LaneProfileNotSupported);
                                    Logger.Warning("Lane Profile not supported");

                                    carriersByCost.HasCarrierBeenInvalidated = true;
                                }

                                Logger.Information("Creating BookFees from CarrTarFees");

                                List<DTO.BookFee> bookFees = (from d in carrTarFees
                                                              select SelectBookFeeFromCarrTarFee(d, bookRevenue.BookControl)).ToList();

                                bookRevenue.BookFees.AddRange(bookFees);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Refresh the lane fees for each order on the load
        /// </summary>
        /// <param name="ratedLoad"></param>
        /// <param name="results"></param>
        /// <param name="carriersByCost"></param>
        /// <remarks>
        /// Created by RHR for v-7.0.5.103 on 1/23/2016
        ///   We now replace all lane fees when we calculate carrier costs
        /// </remarks>
        private void addLaneFeeDataToBookFees(ref Load ratedLoad, ref DTO.CarrierCostResults results, ref DTO.CarriersByCost carriersByCost)
        {
            using (var operation = Logger.StartActivity("addLaneFeeDataToBookFees(RatedLoad: {RatedLoad}, Results: {Results}, CarriersByCost: {CarriersByCost})", ratedLoad, results, carriersByCost))
            {
                if (ratedLoad == null)
                {
                    operation.Complete();
                    return;
                }
                Logger.Information("Deleting Lane Fees from RatedLoad");
                ratedLoad.deleteLaneFees();

                foreach (DTO.BookRevenue bookRevenue in ratedLoad.BookRevenues)
                {
                    List<string> sFaultInfo = new List<string>();
                    List<DTO.LaneFee> LaneFees = new List<DTO.LaneFee>();
                    Logger.Information("Getting Lane Fees for BookRevenue: {BookRevenueODControl}", bookRevenue.BookODControl);

                    LaneFees = NGLLaneFeeData.GetLaneFeesFiltered(bookRevenue.BookODControl).ToList();

                    Logger.Information("GetLaneFeesFiltered returned {LaneFeesCount} records", LaneFees?.Count());

                    if (sFaultInfo != null && sFaultInfo.Count() > 0)
                    {
                        // save error message to log;
                        carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_SQLFaultCannotReadCarrierFees, sFaultInfo);
                        //add the error to the log
                        results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
                    }
                    else
                    {
                        if (LaneFees != null && LaneFees.Count() > 0)
                        {

                            Logger.Information("Creating BookFees from LaneFees");
                            // Create a new BookFee object and populate it with data from the LaneFee object.
                            List<DTO.BookFee> bookFees = (from d in LaneFees select SelectBookFeeFromLaneFee(d, bookRevenue.BookControl)).ToList();

                            // Add the update lane fees to the BookFees list of the BookRevenue object.
                            bookRevenue.BookFees.AddRange(bookFees);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Get a list of carrier tariff fees including Lane and Order specific profile fees
        /// </summary>
        /// <param name="LaneControl"></param>
        /// <param name="carrTarControl"></param>
        /// <param name="sFaultInfo"></param>
        /// <param name="BookControl"></param>
        /// <returns></returns>
        /// <remarks>
        ///  Modified by RHR on 4/22/2019 for v-8.2 
        ///     Added logic to use records in BookAccessorial table as order specific profile fees
        ///     So we need to pass in a BookControl number
        /// </remarks>
        private List<DTO.CarrTarFee> getCarrTarFeesByLane(int LaneControl, int carrTarControl, ref List<string> sFaultInfo, int BookControl)
        {
            List<DTO.CarrTarFee> carrTarFees = new List<DTO.CarrTarFee>();      // empty list if nothing found.

            using (Logger.StartActivity("getCarrTarFeesByLane(LaneControl: {LaneControl}, carrTarControl: {CarrTarControl}, BookControl: {BookControl})", LaneControl, carrTarControl, BookControl))
            {

                Tuple<int, int> dictKey = Tuple.Create(LaneControl, carrTarControl);
                //check for a stored reference for this carrTarControl
                if (DictCarrTarFeesForLane.ContainsKey(dictKey))
                {
                    return DictCarrTarFeesForLane[dictKey];
                }

                try
                {
                    carrTarFees =
                        NGLCarrTarFeeData.GetCarrTarFeesFilteredByLaneProfile(LaneControl, carrTarControl, BookControl);
                    if (carrTarFees != null && carrTarFees.Count() > 0)
                    {
                        DictCarrTarFeesForLane.Add(dictKey, carrTarFees);
                    }
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    if (sqlEx.Message != "E_NoData") // don't throw the exception if no data was found.
                    {
                        sFaultInfo = new List<string>
                            { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                        // "E_SQLExceptionMSG", E_UnExpectedMSG
                        string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                            sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                        SaveSysError(errMsg, sourcePath("getCarrTarFeesByLane"), carrTarControl.ToString());
                    }
                    else
                    {
                        DictCarrTarFeesForLane.Add(dictKey, carrTarFees);
                    }
                }
                catch (Exception ex)
                {
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    throw;
                }
            }

            return carrTarFees;
        }


        private List<DTO.CarrTarFee> getCarrTarFeesWithProfileFees(List<DTO.BookFee> profileFees, int carrTarControl, ref List<string> sFaultInfo)
        {
            List<DTO.CarrTarFee> carrTarFees = new List<DTO.CarrTarFee>();      // empty list if nothing found.            
            using (Logger.StartActivity("getCarrTarFeesWithProfileFees(ProfileFees: {ProfileFees}, carrTarControl: {CarrTarControl})", profileFees, carrTarControl))
            {
                try
                {
                    carrTarFees = NGLCarrTarFeeData.GetCarrTarFeesWithProfileFees(profileFees, carrTarControl);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    Logger.Error(sqlEx, "FaultException caught in getCarrTarFeesWithProfileFees");
                    if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                    {
                        sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                        // "E_SQLExceptionMSG", E_UnExpectedMSG
                        string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                            sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                        SaveSysError(errMsg, sourcePath("getCarrTarFeesWithProfileFees"), carrTarControl.ToString());
                    }
                }
                catch (Exception ex)
                {
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    throw;
                }
            }
            return carrTarFees;
        }


        /// <summary>
        /// Used inside of a linq query to Create a new BookFee object and populate it with data from the CarrTarFee object.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="BookControl"></param>
        /// <returns></returns>
        private DTO.BookFee SelectBookFeeFromCarrTarFee(DTO.CarrTarFee d, int BookControl)
        {
            return new DTO.BookFee
            {
                BookFeesControl = 0,
                BookFeesBookControl = BookControl,
                BookFeesMinimum = d.CarrTarFeesMinimum,
                BookFeesValue = 0,
                BookFeesVariable = d.CarrTarFeesVariable,
                BookFeesAccessorialCode = d.CarrTarFeesAccessorialCode,
                BookFeesAccessorialFeeTypeControl = (int)DAL.Utilities.AccessorialFeeType.Tariff,
                BookFeesOverRidden = false,
                BookFeesVariableCode = d.CarrTarFeesVariableCode,
                BookFeesVisible = d.CarrTarFeesVisible,
                BookFeesAutoApprove = d.CarrTarFeesAutoApprove,
                BookFeesAllowCarrierUpdates = d.CarrTarFeesAllowCarrierUpdates,
                BookFeesCaption = d.CarrTarFeesCaption,
                BookFeesEDICode = d.CarrTarFeesEDICode,
                BookFeesTaxable = d.CarrTarFeesTaxable,
                BookFeesIsTax = d.CarrTarFeesIsTax,
                BookFeesTaxSortOrder = d.CarrTarFeesTaxSortOrder,
                BookFeesBOLText = d.CarrTarFeesBOLText,
                BookFeesBOLPlacement = d.CarrTarFeesBOLPlacement,
                BookFeesAccessorialFeeAllocationTypeControl = d.CarrTarFeesAccessorialFeeAllocationTypeControl,
                BookFeesTarBracketTypeControl = d.CarrTarFeesTarBracketTypeControl,
                BookFeesAccessorialFeeCalcTypeControl = d.CarrTarFeesAccessorialFeeCalcTypeControl,
                BookFeesModDate = d.CarrTarFeesModDate,
                BookFeesModUser = d.CarrTarFeesModUser
            };

        }

        /// <summary>
        /// Used inside of a linq query to Create a new BookFee object and populate it with data fronm the LaneFee object
        /// </summary>
        /// <param name="d"></param>
        /// <param name="BookControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-7.0.5.103 on 1/23/2017
        /// </remarks>
        private DTO.BookFee SelectBookFeeFromLaneFee(DTO.LaneFee d, int BookControl)
        {
            return new DTO.BookFee
            {
                BookFeesControl = 0,
                BookFeesBookControl = BookControl,
                BookFeesMinimum = d.LaneFeesMinimum,
                BookFeesValue = 0,
                BookFeesVariable = d.LaneFeesVariable,
                BookFeesAccessorialCode = d.LaneFeesAccessorialCode,
                BookFeesAccessorialFeeTypeControl = (int)DAL.Utilities.AccessorialFeeType.Lane,
                BookFeesOverRidden = false,
                BookFeesVariableCode = d.LaneFeesVariableCode,
                BookFeesVisible = d.LaneFeesVisible,
                BookFeesAutoApprove = d.LaneFeesAutoApprove,
                BookFeesAllowCarrierUpdates = d.LaneFeesAllowCarrierUpdates,
                BookFeesCaption = d.LaneFeesCaption,
                BookFeesEDICode = d.LaneFeesEDICode,
                BookFeesTaxable = d.LaneFeesTaxable,
                BookFeesIsTax = d.LaneFeesIsTax,
                BookFeesTaxSortOrder = d.LaneFeesTaxSortOrder,
                BookFeesBOLText = d.LaneFeesBOLText,
                BookFeesBOLPlacement = d.LaneFeesBOLPlacement,
                BookFeesAccessorialFeeAllocationTypeControl = d.LaneFeesAccessorialFeeAllocationTypeControl,
                BookFeesTarBracketTypeControl = d.LaneFeesTarBracketTypeControl,
                BookFeesAccessorialFeeCalcTypeControl = d.LaneFeesAccessorialFeeCalcTypeControl,
                BookFeesModDate = d.LaneFeesModDate,
                BookFeesModUser = d.LaneFeesModUser
            };

        }

        /// <summary>
        /// Calculate the fee and allocate it back to the orders.
        /// </summary>
        /// <param name="bookRevenue">Current BookRevenue object containing the BookFee object used as input to calculate the fee.</param>
        /// <param name="bookFeeInput">BookFee object containing the data being used to calculate the fee.</param>
        /// <param name="ratedLoad">Load data and rating results.</param>
        /// <remarks>
        /// Modified by RHR for v-7.0.5.103 on 01/24/2017
        ///     add logic to be sure the address matches even if the stop or pickup numbers are the same
        ///     stop and pickup numbers may not have been assigned properly yet.
        /// </remarks>
        public void calculateAndAllocateFee(DTO.BookRevenue bookRevenue, DTO.BookFee bookFeeInput, ref Load ratedLoad)
        {

            using (var operation = Logger.StartActivity("calculateAndAllocateFee(BookRevenue: {BookRevenue}, BookFee: {@BookFee}, Load: {RatedLoad}", bookRevenue, bookFeeInput, ratedLoad))

            {
                if (ratedLoad == null)
                {
                    Logger.Warning("RatedLoad is null");
                    operation.Complete();
                    return;
                }

                decimal totalFee = -1;

                Logger.Information("Checking if BookFeesAccessorialFeeAllocationTypeControl ({BookFeesAccessorialFeeAllocationTypeControl}) == Origin ({Origin}), Destination ({Destination}), Load ({Load}), or None ({None})",
                    bookFeeInput.BookFeesAccessorialFeeAllocationTypeControl,
                    (int)DAL.Utilities.FeeAllocationType.Origin,
                    (int)DAL.Utilities.FeeAllocationType.Destination,
                    (int)DAL.Utilities.FeeAllocationType.Load,
                    (int)DAL.Utilities.FeeAllocationType.None);

                switch (bookFeeInput.BookFeesAccessorialFeeAllocationTypeControl)
                {
                    case (int)DAL.Utilities.FeeAllocationType.Origin:
                        {
                            // Select the subgroup of BookRevenue objects for the Pickup Stop Number.
                            // Modified by RHR for v-7.0.5.103 on 01/24/2017
                            var subGroupBookRevenues = ratedLoad.BookRevenues.Where
                            (
                                x => x.BookPickupStopNumber == bookRevenue.BookPickupStopNumber
                                     && x.BookOrigAddress1 == bookRevenue.BookOrigAddress1
                                     && x.BookOrigCity == bookRevenue.BookOrigCity
                                     && x.BookOrigState == bookRevenue.BookOrigState
                            ).ToArray();

                            // Calculate fee (i.e., total fee for the subgroup of BookRevenue objects.
                            totalFee = calculateFee(bookFeeInput, subGroupBookRevenues);
                            Logger.Information("FeeAllocationType.Origin - Checking if totalFee ({TotalFee}) != -1 ({TotalFeeEqualNegative1}), if true, allocateFeeToBookFees", totalFee, totalFee != -1);
                            if (totalFee != -1)
                            {

                                // Allocate the fee to the individual BookRevenue.BookFees.
                                allocateFeeToBookFees(totalFee, bookFeeInput, subGroupBookRevenues);
                            }
                        }
                        break;

                    case (int)DAL.Utilities.FeeAllocationType.Destination:
                        {
                            // Select the subgroup of BookRevenue objects for the Pickup Stop Number.
                            // Modified by RHR for v-7.0.5.103 on 01/24/2017
                            var subGroupBookRevenues = ratedLoad.BookRevenues.Where
                            (
                                x => x.BookStopNo == bookRevenue.BookStopNo
                                     && x.BookDestAddress1 == bookRevenue.BookDestAddress1
                                     && x.BookDestCity == bookRevenue.BookDestCity
                                     && x.BookDestState == bookRevenue.BookDestState
                            ).ToArray();

                            // Calculate fee (i.e., total fee for the subgroup of BookRevenue objects.
                            totalFee = calculateFee(bookFeeInput, subGroupBookRevenues);
                            Logger.Information("FeeAllocationType.Destination - Checking if totalFee ({TotalFee}) != -1 ({TotalFeeEqualNegative1}), if true, allocateFeeToBookFees", totalFee, totalFee != -1);

                            if (totalFee != -1)
                            {
                                // Allocate the fee to the individual BookRevenue.BookFees.
                                allocateFeeToBookFees(totalFee, bookFeeInput, subGroupBookRevenues);
                            }
                        }
                        break;

                    case (int)DAL.Utilities.FeeAllocationType.Load:
                        // Calculate the carrier fee for the entire load and allocate the fee back to the BookRevenue objects.
                        // Calculate fee (i.e., total fee for the subgroup of BookRevenue objects.
                        totalFee = calculateFee(bookFeeInput, ratedLoad.BookRevenues);

                        Logger.Information("FeeAllocationType.Load - Checking if totalFee ({TotalFee}) != -1 ({TotalFeeEqualNegative1}), if true, allocateFeeToBookFees", totalFee, totalFee != -1);
                        if (totalFee != -1)
                        {
                            // Allocate the fee to the individual BookRevenue.BookFees.
                            allocateFeeToBookFees(totalFee, bookFeeInput, ratedLoad.BookRevenues);
                        }

                        break;

                    case (int)DAL.Utilities.FeeAllocationType.None:
                        {
                            // Select the BookRevenue object containing this BookFee.
                            var subGroupBookRevenues = ratedLoad.BookRevenues
                                .Where(x => x.BookControl == bookFeeInput.BookFeesBookControl).ToArray();

                            // Calculate the carrier fee for the current BookRevenue object and allocate the fee back to the BookRevenue objects.
                            totalFee = calculateFee(bookFeeInput, subGroupBookRevenues);
                            Logger.Information("FeeAllocationType.None - Checking if totalFee ({TotalFee}) != -1 ({TotalFeeEqualNegative1}), if true, allocateFeeToBookFees", totalFee, totalFee != -1);
                            if (totalFee != -1)
                            {
                                // Allocate the fee to the individual BookRevenue.BookFees.
                                allocateFeeToBookFees(totalFee, bookFeeInput, subGroupBookRevenues);
                            }
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Calculate the total fee for the subgroup of BookRevenue objects (selected based on AccessorialFeeAllocationTypeControl).
        /// </summary>
        /// <param name="bookFeeInput">BookFee object containing the data being used to calculate the fee.</param>
        /// <param name="subGroupBookRevenues">Subgroup of BookRevenue objects selected based on AccessorialFeeAllocationTypeControl</param>
        /// <returns>Total fee.</returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.102 8/9/2016
        ///     We now check the BookFeeisTax flag when the BookFeesVariableCode
        ///     is CarrierCost and we use the BookRevNetCost instead of CarrierCost
        /// </remarks>
        private decimal calculateFee(DTO.BookFee bookFeeInput, DTO.BookRevenue[] subGroupBookRevenues)
        {

            decimal fee = -1;
            // Calculate the fee based on the VariableCode (i.e., algorithm)

            using (var operation = Logger.StartActivity("calculateFee(BookFee: {BookFee}, SubGroupBookRevenues: {SubGroupBookRevenues})", bookFeeInput, subGroupBookRevenues))

            {
                Logger.Information("Checking BookFeeVariableCode: {BookFeesVariableCode}", bookFeeInput.BookFeesVariableCode);
                switch (bookFeeInput.BookFeesVariableCode)
                {
                    case (int)DAL.Utilities.FeeVariableCode.LoadWeight:

                        fee = (decimal)(bookFeeInput.BookFeesVariable * subGroupBookRevenues.Sum(x => x.BookTotalWgt));

                        Logger.Information("{BookFee} - LoadWeight: setting fee = BookFeesVariable ({BookFeesVariable}) * Sum of BookTotalWgt ({SumBookTotalWgt})",
                            bookFeeInput,
                            bookFeeInput.BookFeesVariable,
                            subGroupBookRevenues.Sum(x => x.BookTotalWgt));

                        break;

                    case (int)DAL.Utilities.FeeVariableCode.NumberPallets:

                        fee = (decimal)(bookFeeInput.BookFeesVariable * subGroupBookRevenues.Sum(x => x.BookTotalPL));

                        Logger.Information("{BookFee} - NumberPallets: setting fee = BookFeesVariable ({BookFeesVariable}) * Sum of BookTotalPL ({SumBookTotalPL})",
                            bookFeeInput,
                            bookFeeInput.BookFeesVariable,
                            subGroupBookRevenues.Sum(x => x.BookTotalPL));

                        break;

                    case (int)DAL.Utilities.FeeVariableCode.CarrierCost:

                        if (bookFeeInput.BookFeesIsTax == true)
                        {
                            fee = (decimal)bookFeeInput.BookFeesVariable * subGroupBookRevenues.Sum(x => x.BookRevNetCost);
                            Logger.Information("{BookFee} - CarrierCost: setting fee = BookFeesVariable ({BookFeesVariable}) * Sum of BookRevNetCost ({SumBookRevNetCost})",
                                bookFeeInput,
                                bookFeeInput.BookFeesVariable,
                                subGroupBookRevenues.Sum(x => x.BookRevNetCost));
                        }
                        else
                        {
                            fee = (decimal)bookFeeInput.BookFeesVariable * subGroupBookRevenues.Sum(x => x.BookRevCarrierCost);
                            Logger.Information("{BookFee} - CarrierCost: setting fee = BookFeesVariable ({BookFeesVariable}) * Sum of BookRevCarrierCost ({SumBookRevCarrierCost})",
                                bookFeeInput,
                                bookFeeInput.BookFeesVariable,
                                subGroupBookRevenues.Sum(x => x.BookRevCarrierCost));
                        }
                        break;

                    case (int)DAL.Utilities.FeeVariableCode.PerMile:

                        fee = (decimal)(bookFeeInput.BookFeesVariable * (double)subGroupBookRevenues.Sum(x => x.BookMilesFrom));

                        Logger.Information("{BookFee} - PerMile: setting fee = BookFeesVariable ({BookFeesVariable}) * Sum of BookMilesFrom ({SumBookMilesFrom})",
                            bookFeeInput,
                            bookFeeInput.BookFeesVariable,
                            subGroupBookRevenues.Sum(x => x.BookMilesFrom));

                        break;

                    case (int)DAL.Utilities.FeeVariableCode.ByVolume:

                        fee = (decimal)(bookFeeInput.BookFeesVariable * (double)subGroupBookRevenues.Sum(x => x.BookTotalCube));

                        Logger.Information("{BookFee} - ByVolume: setting fee = BookFeesVariable ({BookFeesVariable}) * Sum of BookTotalCube ({SumBookTotalCube})",
                            bookFeeInput,
                            bookFeeInput.BookFeesVariable,
                            subGroupBookRevenues.Sum(x => x.BookTotalCube));

                        break;

                    case (int)DAL.Utilities.FeeVariableCode.ByQuantity:

                        fee = (decimal)(bookFeeInput.BookFeesVariable * (double)subGroupBookRevenues.Sum(x => x.BookTotalCases));

                        Logger.Information("{BookFee} - ByQuantity: setting fee = BookFeesVariable ({BookFeesVariable}) * Sum of BookTotalCases ({SumBookTotalCases})",
                            bookFeeInput,
                            bookFeeInput.BookFeesVariable,
                            subGroupBookRevenues.Sum(x => x.BookTotalCases));

                        break;
                }

                Logger.Information("{BookFee} - Checking if fee ({Fee}) < BookFeeMinimum ({BookFeesMinimum})",
                    bookFeeInput,
                    fee,
                    bookFeeInput.BookFeesMinimum);

                if (fee < bookFeeInput.BookFeesMinimum)
                {
                    fee = bookFeeInput.BookFeesMinimum;
                    Logger.Information("{BookFee} Setting fee to BookFeesMinimum ({BookFeesMinimum})", bookFeeInput, bookFeeInput.BookFeesMinimum);
                }

                fee = Decimal.Round(fee, 2);

            }
            return fee;
        }


        /// <summary>
        /// Allocate the total fee for the subgroup across all BookRevenue objects in the subgroup.
        /// </summary>
        /// <param name="totalFee">Total fee to be allocated across all BookRevenue objects in subgroup.</param>
        /// <param name="bookFeeInput">BookFee object containing the data being used to calculate the fee.</param>
        /// <param name="subGroupBookRevenues">Subgroup of BookRevenue objects selected based on AccessorialFeeAllocationTypeControl</param>
        private void allocateFeeToBookFees(Decimal totalFee, DTO.BookFee bookFeeInput, DTO.BookRevenue[] subGroupBookRevenues)
        {
            using (var operation = Logger.StartActivity("allocateFeeToBookFees(TotalFee: {TotalFee}, BookFee: {BookFee}, SubGroupBookRevenues: {SubGroupBookRevenues})", totalFee, bookFeeInput, subGroupBookRevenues))
            {
                // Calculate the allocated fee for each BookRevenue object in the subgroup.
                decimal remainingTotalFee = totalFee;
                int count = 0;
                foreach (DTO.BookRevenue bookRevenue in subGroupBookRevenues)
                {
                    Logger.Information(
                        "{BookRevenue} - {BookFee} - Calculating allocated fee  - checking if this is the last record in the subgroup ({LastCount})",
                        bookRevenue,
                        bookFeeInput,
                        count == subGroupBookRevenues.Length - 1);

                    decimal allocatedFee = 0;

                    if (count == subGroupBookRevenues.Length - 1)
                    {
                        Logger.Information(
                            "{BookRevenue} - {BookFee} - setting allocated fee to remaining total fee ({RemainingTotalFee})",
                            bookRevenue,
                            bookFeeInput,
                            remainingTotalFee);
                        allocatedFee = remainingTotalFee;
                    }
                    else
                    {
                        // Calculate the allocated fee based on the Bracket Type.
                        Logger.Information(
                            "{BookRevenue} - {BookFee} - Calculating allocated fee based on Bracket Type: {BookFeesTarBracketType}",
                            bookRevenue,
                            bookFeeInput,
                            bookFeeInput.BookFeesTarBracketTypeControl);

                        allocatedFee = calcAllocatedFee(totalFee, bookRevenue, subGroupBookRevenues, bookFeeInput.BookFeesTarBracketTypeControl);

                        Logger.Information(
                            "{BookRevenue} - {BookFee} - Calculated Allocated fee: {AllocatedFee} based on Bracket: {BookFeesTarBraketType} setting remainingTotalFee {RemainingTotalFee} -= allocatedFee",
                            bookRevenue,
                            bookFeeInput,
                            allocatedFee,
                            bookFeeInput.BookFeesTarBracketTypeControl,
                            remainingTotalFee);

                        remainingTotalFee -= allocatedFee;
                        count++;
                    }

                    // Update the BookFees collection with the allocated fee amount.
                    UpdateBookFeesWithAllocatedFee(allocatedFee, bookFeeInput, bookRevenue);
                }
            }
        }


        /// <summary>
        /// Calculate the allocated fee for the specified BookRevenue object.
        /// </summary>
        /// <param name="totalFee">Total fee</param>
        /// <param name="bookRevenue">BookRevenue (order) object that is to be updated with the allocated fee.</param>
        /// <param name="subGroupBookRevenues">Subgroup of BookRevenue objects selected based on AccessorialFeeAllocationTypeContro</param>
        /// <param name="bracketTypeControl">BracketType specifying the formula for allocating the fee.</param>
        /// <returns>Allocated fee for the specified BookRevenue (order) object.</returns>
        private decimal calcAllocatedFee(decimal totalFee, DTO.BookRevenue bookRevenue, DTO.BookRevenue[] subGroupBookRevenues, int bracketTypeControl)
        {
            decimal allocatedFee = 0;
            using (var operation =
                        Logger.StartActivity(
                                "calcAllocatedFee(TotalFee: {TotalFee}, BookRevenue: {BookRevenue}, SubGroupBookRevenues: {SubGroupBookRevenues}, BracketTypeControl: {BracketTypeControl})",
                            totalFee,
                                               bookRevenue,
                                               subGroupBookRevenues,
                                               bracketTypeControl))
            {
                // Calculate the allocated fee based on the Bracket Type.
                /* ***********************************************************
                 * Modified by RHR 5/1/2015 v-7.0.2
                 *  Corrected divide by zero errors
                 *  Cole like this:
                 *      allocatedFee = totalFee * (decimal)bookRevenue.BookTotalWgt / (decimal)subGroupBookRevenues.Sum(x => x.BookTotalWgt);
                 *   Does not work if all weights are zero (miles pl etc cause issues)
                 *   Added new logic to allocage by number of booking records if totals are zero
                 *  **********************************************************/
                decimal SumTotals = (decimal)0;
                decimal totalBookings = (decimal)subGroupBookRevenues.Count();
                if (totalBookings < 1) { totalBookings = 1; }

                Logger.Information("Calculating allocated fee based on Bracket Type: {BracketTypeControl}", bracketTypeControl);
                switch (bracketTypeControl)
                {
                    case (int)DAL.Utilities.BracketType.Pallets:        // Break or Allocate by number of pallets                    
                        SumTotals = (decimal)subGroupBookRevenues.Sum(x => x.BookTotalPL);
                        if (SumTotals > 0)
                        {
                            allocatedFee = totalFee * (decimal)bookRevenue.BookTotalPL / SumTotals;
                            Logger.Information("Pallets - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) * BookTotalPL ({BookTotalPL}) / Sum of BookTotalPL ({SumBookTotalPL})",
                                bookRevenue,
                                allocatedFee,
                                totalFee,
                                bookRevenue.BookTotalPL,
                                SumTotals);
                        }
                        else
                        {
                            allocatedFee = totalFee / totalBookings;
                            Logger.Information("Pallets - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) / total bookings ({TotalBookings})",
                                allocatedFee,
                                totalFee,
                                totalBookings);
                        }

                        break;
                    case (int)DAL.Utilities.BracketType.FlatPallet:        // Break or Allocate by number of pallets                    
                                                                           //Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
                        SumTotals = (decimal)subGroupBookRevenues.Sum(x => x.BookTotalPL);
                        if (SumTotals > 0)
                        {
                            allocatedFee = totalFee * (decimal)bookRevenue.BookTotalPL / SumTotals;
                            Logger.Information("FlatPallet - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) * BookTotalPL ({BookTotalPL}) / Sum of BookTotalPL ({SumBookTotalPL})",
                                allocatedFee,
                                totalFee,
                                bookRevenue.BookTotalPL,
                                SumTotals);
                        }
                        else
                        {
                            allocatedFee = totalFee / totalBookings;
                            Logger.Information("FlatPallet - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) / total bookings ({TotalBookings})",
                                allocatedFee,
                                totalFee,
                                totalBookings);
                        }
                        break;
                    case (int)DAL.Utilities.BracketType.Volume:         // Break or Allocate by number of Cubes                    
                        SumTotals = (decimal)subGroupBookRevenues.Sum(x => x.BookTotalCube);
                        if (SumTotals > 0)
                        {
                            allocatedFee = totalFee * (decimal)bookRevenue.BookTotalCube / SumTotals;
                            Logger.Information("Volume - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) * BookTotalCube ({BookTotalCube}) / Sum of BookTotalCube ({SumBookTotalCube})",
                                allocatedFee,
                                totalFee,
                                bookRevenue.BookTotalCube,
                                SumTotals);
                        }
                        else
                        {
                            allocatedFee = totalFee / totalBookings;
                            Logger.Information("Volume - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) / total bookings ({TotalBookings})",
                                allocatedFee,
                                totalFee,
                                totalBookings);
                        }
                        break;
                    case (int)DAL.Utilities.BracketType.Quantity:       // Break or Allocate by number of Cases
                        SumTotals = (decimal)subGroupBookRevenues.Sum(x => x.BookTotalCases);
                        if (SumTotals > 0)
                        {
                            allocatedFee = totalFee * (decimal)bookRevenue.BookTotalCases / SumTotals;
                            Logger.Information("Quantity - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) * BookTotalCases ({BookTotalCases}) / Sum of BookTotalCases ({SumBookTotalCases})",
                                allocatedFee,
                                totalFee,
                                bookRevenue.BookTotalCases,
                                SumTotals);
                        }
                        else
                        {
                            allocatedFee = totalFee / totalBookings;
                            Logger.Information("Quantity - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) / total bookings ({TotalBookings})",
                                allocatedFee,
                                totalFee,
                                totalBookings);
                        }
                        break;
                    case (int)DAL.Utilities.BracketType.Lbs:            // Break or Allocate by number of Lbs                    
                        SumTotals = (decimal)subGroupBookRevenues.Sum(x => x.BookTotalWgt);
                        if (SumTotals > 0)
                        {
                            allocatedFee = totalFee * (decimal)bookRevenue.BookTotalWgt / SumTotals;
                            Logger.Information("Lbs - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) * BookTotalWgt ({BookTotalWgt}) / Sum of BookTotalWgt ({SumBookTotalWgt})",
                                allocatedFee,
                                totalFee,
                                bookRevenue.BookTotalWgt,
                                SumTotals);
                        }
                        else
                        {
                            allocatedFee = totalFee / totalBookings;
                            Logger.Information("Lbs - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) / total bookings ({TotalBookings})",
                                allocatedFee,
                                totalFee,
                                totalBookings);
                        }
                        break;
                    case (int)DAL.Utilities.BracketType.Cwt:            // Break or Allocate by number of Cwt
                        SumTotals = (decimal)subGroupBookRevenues.Sum(x => x.BookTotalWgt);
                        if (SumTotals > 0)
                        {
                            allocatedFee = totalFee * (decimal)bookRevenue.BookTotalWgt / SumTotals;
                            Logger.Information("Cwt - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) * BookTotalWgt ({BookTotalWgt}) / Sum of BookTotalWgt ({SumBookTotalWgt})",
                                allocatedFee,
                                totalFee,
                                bookRevenue.BookTotalWgt,
                                SumTotals);
                        }
                        else
                        {
                            allocatedFee = totalFee / totalBookings;
                            Logger.Information("Cwt - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) / total bookings ({TotalBookings})",
                                allocatedFee,
                                totalFee,
                                totalBookings);
                        }
                        break;
                    case (int)DAL.Utilities.BracketType.Distance:       // Break or Allocate by Distance like Per Kilometer
                        SumTotals = (decimal)subGroupBookRevenues.Sum(x => x.BookMilesFrom);
                        if (SumTotals > 0)
                        {
                            allocatedFee = totalFee * (decimal)bookRevenue.BookMilesFrom / SumTotals;
                            Logger.Information("Distance - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) * BookMilesFrom ({BookMilesFrom}) / Sum of BookMilesFrom ({SumBookMilesFrom})",
                                allocatedFee,
                                totalFee,
                                bookRevenue.BookMilesFrom,
                                SumTotals);
                        }
                        else
                        {
                            allocatedFee = totalFee / totalBookings;
                            Logger.Information("Distance - Allocated fee: {AllocatedFee}, setting allocated fee to total fee ({TotalFee}) / total bookings ({TotalBookings})",
                                allocatedFee,
                                totalFee,
                                totalBookings);
                        }
                        break;
                }
                allocatedFee = Decimal.Round(allocatedFee, 2);
            }
            return allocatedFee;
        }


        /// <summary>
        /// Update the BookRevenue's BookFee collection with the allocated fee.
        /// </summary>
        /// <param name="allocatedFee">Portion of the total fee that is allocated to the specified BookRevenue (order) object.</param>
        /// <param name="bookFeeInput">BookFee object containing the data being used to calculate the fee.</param>
        /// <param name="bookRevenue">BookRevenue (order) object that is to be updated with the allocated fee.</param>
        private void UpdateBookFeesWithAllocatedFee(Decimal allocatedFee, DTO.BookFee bookFeeInput, DTO.BookRevenue bookRevenue)
        {
            // If the BookFee already exists for the fee, update it, otherwise add it.

            using (var operation = Logger.StartActivity("UpdateBookFeesWithAllocatedFee(AllocatedFee: {AllocatedFee}, BookFee: {BookFee}, BookRevenue: {BookRevenue})", allocatedFee, bookFeeInput, bookRevenue))
            {
                // Find the BookFee object in the BookRevenue's BookFees collection.
                DTO.BookFee bookFeeOutput = bookRevenue.BookFees
                                                       .FirstOrDefault(x => x.BookFeesAccessorialCode == bookFeeInput.BookFeesAccessorialCode &&
                                                                        x.BookFeesOverRidden == false);

                Logger.Information("Finding First Book Fee {BookFee} where BookFeesOverRidden == false && x.BookFeesAccessorialCode == bookFeeInput.BookFeesAccessorialCode ({BookFeesAccessorialCode})", bookFeeOutput, bookFeeInput.BookFeesAccessorialCode);

                if (bookFeeOutput != null)
                {
                    // Update the existing BookFee object with the allocated fee.
                    Logger.Information("Updating BookFee {BookFeeInput} with {BookFeeOutput} and allocated fee: {AllocatedFee}",
                        bookFeeInput,
                        bookFeeOutput,
                        allocatedFee);

                    updateBookFee(allocatedFee, bookFeeInput, bookFeeOutput, bookRevenue);
                }
                else
                {
                    // Add a new BookFee object for the allocated fee.
                    // Create a new BookFee object.
                    Logger.Information("Adding new BookFee based on {BookFeeInput} with allocated fee: {AllocatedFee}", bookFeeInput, allocatedFee);
                    DTO.BookFee bookFee = new DTO.BookFee();
                    updateBookFee(allocatedFee, bookFeeInput, bookFee, bookRevenue);
                    bookRevenue.BookFees.Add(bookFee);
                }
            }
        }


        /// <summary>
        /// Update the BookFee object with the allocated fee and the data used to compute the fee.
        /// </summary>
        /// <param name="allocatedFee">Portion of the total fee that is allocated to the specified BookRevenue (order) object.</param>
        /// <param name="bookFeeInput">BookFee object containing the data being used to calculate the fee.</param>
        /// <param name="bookFeeOutput">BookFee object being updated.</param>
        /// <param name="ratedBookRevenue">BookRevenue (order) object that is to be updated with the allocated fee.</param>
        private void updateBookFee(Decimal allocatedFee, DTO.BookFee bookFeeInput, DTO.BookFee bookFeeOutput, DTO.BookRevenue ratedBookRevenue)
        {
            using (var operation = Logger.StartActivity("updateBookFee(AllocatedFee: {AllocatedFee}, BookFeeInput: {BookFeeInput}, BookFeeOutput: {BookFeeOutput}, RatedBookRevenue: {RatedBookRevenue})",
                allocatedFee,
                bookFeeInput,
                bookFeeOutput,
                ratedBookRevenue))
            {
                // Populate the BookFee object with the carrTarFee info and calculated fee.
                bookFeeOutput.BookFeesAccessorialCode = bookFeeInput.BookFeesAccessorialCode;
                bookFeeOutput.BookFeesAccessorialFeeAllocationTypeControl = bookFeeInput.BookFeesAccessorialFeeAllocationTypeControl;
                bookFeeOutput.BookFeesAccessorialFeeCalcTypeControl = bookFeeInput.BookFeesAccessorialFeeCalcTypeControl;
                bookFeeOutput.BookFeesAccessorialFeeTypeControl = bookFeeInput.BookFeesAccessorialFeeTypeControl;
                bookFeeOutput.BookFeesAllowCarrierUpdates = bookFeeInput.BookFeesAllowCarrierUpdates;
                bookFeeOutput.BookFeesAutoApprove = bookFeeInput.BookFeesAutoApprove;
                bookFeeOutput.BookFeesBOLPlacement = bookFeeInput.BookFeesBOLPlacement;
                bookFeeOutput.BookFeesBOLText = bookFeeInput.BookFeesBOLText;
                bookFeeOutput.BookFeesBookControl = ratedBookRevenue.BookControl;
                bookFeeOutput.BookFeesCaption = bookFeeInput.BookFeesCaption;
                bookFeeOutput.BookFeesControl = bookFeeInput.BookFeesControl;
                bookFeeOutput.BookFeesEDICode = bookFeeInput.BookFeesEDICode;
                bookFeeOutput.BookFeesIsTax = bookFeeInput.BookFeesIsTax;
                bookFeeOutput.BookFeesMinimum = bookFeeInput.BookFeesMinimum;
                bookFeeOutput.BookFeesModDate = bookFeeInput.BookFeesModDate;
                bookFeeOutput.BookFeesModUser = bookFeeInput.BookFeesModUser;
                bookFeeOutput.BookFeesOverRidden = false;
                bookFeeOutput.BookFeesTarBracketTypeControl = bookFeeInput.BookFeesTarBracketTypeControl;
                bookFeeOutput.BookFeesTaxable = bookFeeInput.BookFeesTaxable;
                bookFeeOutput.BookFeesUpdated = bookFeeInput.BookFeesUpdated;
                bookFeeOutput.BookFeesValue = allocatedFee;
                bookFeeOutput.BookFeesVariable = bookFeeInput.BookFeesVariable;
                bookFeeOutput.BookFeesVariableCode = bookFeeInput.BookFeesVariableCode;
                bookFeeOutput.BookFeesVisible = bookFeeInput.BookFeesVisible;
            }
        }


        #endregion
    }
}
