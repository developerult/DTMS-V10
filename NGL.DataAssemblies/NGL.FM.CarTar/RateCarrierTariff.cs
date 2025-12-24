using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;
using Ngl.FreightMaster.Data;
using SerilogTracing;
using Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.CarTar
{
    /// <summary>
    /// RateCarrierTariff Class
    /// This class is responsible for all rating activities.
    /// </summary>
    public class RateCarrierTariff : TarBaseClass
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public RateCarrierTariff(DAL.WCFParameters oParameters)
            : base()
        {
            Logger.Information("RateCarrierTariff - Parameters: {@0}", Parameters);

            if (oParameters == null)
            {
                // populateSampleParameterData();  //this will not really be used in production
            }
            else
            {
                Parameters = oParameters;
            }
        }

        #endregion

        #region " Properties"


        /// <summary>
        /// flag used to indicate if the M_DistanceRateRestricted results message has already been added to the result object (only one is expected)
        /// </summary>
        public bool DistRateRstrMsgAdded { get; set; }
        /// <summary>
        /// flag used to indicate if the M_FlatRateRestricted results message has already been added to the result object (only one is expected)
        /// </summary>
        public bool FlatRateRstrMsgAdded { get; set; }
        /// <summary>
        /// flag used to indicate if the M_LTLRateRestricted results message has already been added to the result object (only one is expected)
        /// </summary>
        public bool LTLRateRstrMsgAdded { get; set; }
        /// <summary>
        /// flag used to indicate if the M_UOMRateRestricted results message has already been added to the result object (only one is expected)
        /// </summary>
        public bool UOMRateRstrMsgAdded { get; set; }

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.RateCarrierTariff";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        /// <summary>
        /// This property is used for accessing the instance of the CarrierFees class.
        /// </summary>
        private CarrierFees _carrierFeesInstance = null;
        public CarrierFees CarrierFeesInstance
        {
            get
            {
                if (_carrierFeesInstance == null)
                {
                    // Create and instance of the CarrierFees class.
                    _carrierFeesInstance = new CarrierFees(Parameters);
                }
                return _carrierFeesInstance;
            }
        }

        private NGLCarrTarDiscountData _discountInstance = null;
        public NGLCarrTarDiscountData DiscountInstance
        {
            get
            {
                if (_discountInstance == null)
                {
                    // Create an instance of the Discount class.
                    _discountInstance = new NGLCarrTarDiscountData(Parameters);
                }
                return _discountInstance;
            }
        }

        private NGLCarrTarClassXrefData _classXrefInstance = null;
        public NGLCarrTarClassXrefData ClassXrefInstance
        {
            get
            {
                if (_classXrefInstance == null)
                {
                    // Create an instance of the ClassXRef class.
                    _classXrefInstance = new NGLCarrTarClassXrefData(Parameters);
                }
                return _classXrefInstance;
            }
        }

        private NGLCarrTarMinChargeData _minChargeInstance = null;
        public NGLCarrTarMinChargeData MinChargeInstance
        {
            get
            {
                if (_minChargeInstance == null)
                {
                    // Create an instance of the Discount class.
                    _minChargeInstance = new NGLCarrTarMinChargeData(Parameters);
                }
                return _minChargeInstance;
            }
        }

        private NGLCarrTarMinWeightData _minWeightInstance = null;
        public NGLCarrTarMinWeightData MinWeightInstance
        {
            get
            {
                if (_minWeightInstance == null)
                {
                    _minWeightInstance = new NGLCarrTarMinWeightData(Parameters);
                }
                return _minWeightInstance;
            }
        }

        private NGLCarrTarInterlineData _interlineInstance = null;
        public NGLCarrTarInterlineData InterlineInstance
        {
            get
            {
                if (_interlineInstance == null)
                {
                    // Create an instance of the Interline class.
                    _interlineInstance = new NGLCarrTarInterlineData(Parameters);
                }
                return _interlineInstance;
            }
        }

        private NGLCarrTarNonServData _nonServInstance = null;
        public NGLCarrTarNonServData NonServInstance
        {
            get
            {
                if (_nonServInstance == null)
                {
                    // Create an instance of the Interline class.
                    _nonServInstance = new NGLCarrTarNonServData(Parameters);
                }
                return _nonServInstance;
            }
        }


        private Dictionary<int, DTO.CarrTarNonServ[]> _dictNonServicePoints = null;
        public Dictionary<int, DTO.CarrTarNonServ[]> DictNonServicePoints
        {
            get
            {
                if (_dictNonServicePoints == null)
                {
                    // Create a new dictionary.
                    _dictNonServicePoints = new Dictionary<int, DTO.CarrTarNonServ[]>();
                }
                return _dictNonServicePoints;
            }
            set { _dictNonServicePoints = value; }
        }


        private Dictionary<int, DTO.CarrTarInterline[]> _dictTarInterlineList = null;
        public Dictionary<int, DTO.CarrTarInterline[]> DictTarInterlineList
        {
            get
            {
                if (_dictTarInterlineList == null)
                {
                    // Create a new dictionary.
                    _dictTarInterlineList = new Dictionary<int, DTO.CarrTarInterline[]>();
                }
                return _dictTarInterlineList;
            }
            set { _dictTarInterlineList = value; }
        }

        private Dictionary<int, DTO.CarrTarMinCharge[]> _dictTarMinCharge = null;
        public Dictionary<int, DTO.CarrTarMinCharge[]> DictTarMinCharge
        {
            get
            {
                if (_dictTarMinCharge == null)
                {
                    // Create a new dictionary.
                    _dictTarMinCharge = new Dictionary<int, DTO.CarrTarMinCharge[]>();
                }
                return _dictTarMinCharge;
            }
            set { _dictTarMinCharge = value; }
        }

        private Dictionary<int, DTO.CarrTarMinWeight[]> _dictTarMinWeight = null;
        public Dictionary<int, DTO.CarrTarMinWeight[]> DictTarMinWeight
        {
            get
            {
                if (_dictTarMinWeight == null)
                {
                    // Create a new dictionary.
                    _dictTarMinWeight = new Dictionary<int, DTO.CarrTarMinWeight[]>();
                }
                return _dictTarMinWeight;
            }
            set { _dictTarMinWeight = value; }
        }

        private Dictionary<int, DTO.CarrTarDiscount[]> _dictTarDiscount = null;
        public Dictionary<int, DTO.CarrTarDiscount[]> DictTarDiscount
        {
            get
            {
                if (_dictTarDiscount == null)
                {
                    // Create a new dictionary.
                    _dictTarDiscount = new Dictionary<int, DTO.CarrTarDiscount[]>();
                }
                return _dictTarDiscount;
            }
            set { _dictTarDiscount = value; }
        }

        #endregion

        #region " Methods"

        public decimal rateCarrier(ref DTO.CarrierCostResults results, Load load, DTO.CarrierTariffsPivot[] carrierTariffsPivots, ref Load ratedLoad,
            ref ClassRatingLineAllocation LineAllocation)
        {
            Logger.Information("RateCarrierTariff.rateCarrier(\nresults:{@0},\nload:{@1},\npivots:{@2},\nratedLoad:{@3},\nLineAllocation:{@4}", results, load, carrierTariffsPivots, ratedLoad, LineAllocation);
            DTO.CarriersByCost carriersByCost = new DTO.CarriersByCost();
            return rateCarrier(ref results, load, carrierTariffsPivots, ref ratedLoad, ref LineAllocation, ref carriersByCost);
        }

        /// <summary>
        /// Rate a load with the carrier tariff pivot data.  The caller must filter the records by equipment or unique tariff before calling this method. 
        /// </summary>
        /// <param name="load">Load being rated (contains BookRevenue collection)</param>
        /// <param name="carrierTariffsPivot">Carrier tariff info to rate with.</param>
        /// <param name="carrierCosts">Rating results cost information.</param>
        /// <returns>
        /// Generally carrierTariffsPivots will contain one record but may hold multiple in the case of LTL or address precision.
        /// All records will belong to the same tariff/equipment 
        /// </returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.100 4/20/2016 
        ///     Added InterlinePoint property
        /// Modified by RHR v-7.0.5.102 8/9/2016 
        ///     Added new parameters to UpdateBookRevenues method
        /// </remarks>
        public decimal rateCarrier(ref DTO.CarrierCostResults results, Load load, DTO.CarrierTariffsPivot[] carrierTariffsPivots, ref Load ratedLoad,
            ref ClassRatingLineAllocation LineAllocation, ref DTO.CarriersByCost newCarriersByCost)
        {



            decimal totalCost = -1;
            ratedLoad = null;
            List<string> sFaultInfo = null;
            // Make a deep copy of the load so that we can update the copy with the rating results.
            //Logger.Information("RateCarrierTariff.rateCarrier(\nresults:{@0},\nload:{@1},\npivots:{@2},\nratedLoad:{@3},\nLineAllocation:{@4},\nnewCarriersByCost:{@5}", results, load, carrierTariffsPivots, ratedLoad, LineAllocation, newCarriersByCost);

            ratedLoad = DeepCopy(load);

            ratedLoad.Parameters = load.Parameters;
            ratedLoad.CarrTarEquipMatMaxDays = carrierTariffsPivots[0].CarrTarEquipMatMaxDays;
            ratedLoad.CarrTarOutbound = carrierTariffsPivots[0].CarrTarOutbound;
            ratedLoad.CarrTarWillDriveSaturday = carrierTariffsPivots[0].CarrTarWillDriveSaturday;
            ratedLoad.CarrTarWillDriveSunday = carrierTariffsPivots[0].CarrTarWillDriveSunday;
            Logger.Information("Assign rated load to carrier...{0}", carrierTariffsPivots[0].CarrierName);
            ratedLoad.assignRatedBookRevenue(carrierTariffsPivots[0].RatedBookControl);
            Logger.Information("RateCarrierTariff.rateCarrier.AfterAssignRatedBookRevenue: CarrTarEquipMatMaxDays: {CarrTarEquipMatMaxDays}, CarrTarOutbound: {CarrTarOutbound}, CarrTarWillDriveSaturday: {CarrTarWillDriveSaturday}, CarrTarWillDriveSunday: {CarrTarWillDriveSunday}", ratedLoad.CarrTarEquipMatMaxDays, ratedLoad.CarrTarOutbound, ratedLoad.CarrTarWillDriveSaturday, ratedLoad.CarrTarWillDriveSunday);



            // Modified by RHR v-7.0.5.101 6/2/2016  
            //   we now query the db directly using isBookRevNonServicePoint instead of storing the non-service information in memory
            //   there are too many records and the system performance would be affected.
            foreach (DTO.BookRevenue bookRev in load.BookRevenues)
            {
                Logger.Information("RateCarrierTariff.rateCarrier.ForEachBookRev:");
                if (isBookRevNonServicePoint(carrierTariffsPivots[0].CarrTarControl, bookRev, ref sFaultInfo))
                {
                    Logger.Information("RateCarrierTariff.rateCarrier.ForEachBookRev.IsBookRevNonServicePoint CarrTarControl:{0} FaultInfo:{@1}", carrierTariffsPivots[0].CarrTarName, sFaultInfo);
                    if (sFaultInfo != null && sFaultInfo.Count() > 0)
                    {
                        //this is a fault exception we need to return a message to the user.
                        //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                        //and a message about why the tariff cannot be selected.

                        Logger.Information("RateCarrierTariff.rateCarrier.ForEachBookRev.IsBookRevNonServicePoint - Add Invalid Carrier - 343");
                        Logger.Information("Creating invalid carrier and adding to return with total cost: {TotalCost}", totalCost);

                        results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);

                        return totalCost;
                    }

                    //If any one of the orders is part of the non service points this tariff is not allowed
                    Logger.Information("RateCarrierTariff.rateCarrier.ForEachBookRev.IsBookRevNonServicePoint - Add Invalid Carrier - 351");
                    results.AddLog("Non-service point found for carrier " + carrierTariffsPivots[0].CarrierName);

                    return totalCost;
                }
            }


            // Modified by RHR v-7.0.5.101 6/2/2016  
            //   we now query the db directly using isBookRevInterline instead of storing the interline information in memory
            //   there are too many records and the system performance would be affected.
            List<int> interlineList = new List<int>();
            foreach (DTO.BookRevenue bookRev in load.BookRevenues)
            {
                //Modified by RHR v-7.0.5.100 4/20/2016 Added Logic to always capture InterlinePoint flag
                Logger.Information("RateCarrierTariff.rateCarrier.isBookRevInterline", bookRev);
                if (isBookRevInterline(carrierTariffsPivots[0].CarrTarControl, bookRev, ref interlineList, ref sFaultInfo))
                {
                    Logger.Information("RateCarrierTariff.rateCarrier.isBookRevInterline");
                    if (sFaultInfo != null && sFaultInfo.Count() > 0)
                    {
                        //this is a fault exception we need to return a message to the user.
                        //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                        //and a message about why the tariff cannot be selected.                
                        //add the error to the log
                        results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
                        return totalCost;
                    }
                    if (bookRev.BookAllowInterlinePoints == false)
                    {
                        //If any one of the orders has an interline point restriction this tariff is not allowed
                        results.AddLog("Interline point restriction for carrier " + carrierTariffsPivots[0].CarrierName);
                        return totalCost;
                    }
                    else
                    {
                        ratedLoad.InterlinePoint = true;
                    }
                }
            }
            int recCount = 0;

            // Get an allocation percentage list from the linehaul charge calculation.
            Dictionary<int, double> DictAlloc = new Dictionary<int, double>();
            results.AddLog("Calculate the total line haul charge.");
            Logger.Information("RateCarrierTariff.rateCarrier - Calculate total line haul charge");
            List<DTO.CarrierTariffsPivot> ratedPivots = new List<DTO.CarrierTariffsPivot>();
            ratedLoad.TotalLineHaul = calcLineHaulCharge(ref results,
                ratedLoad, interlineList,
                carrierTariffsPivots,
                ref DictAlloc,
                ref LineAllocation,
                ref ratedPivots);
            if (ratedLoad.TotalLineHaul <= 0)
            {
                // Unable to rate.
                Logger.Warning("RateCarrierTariff.rateCarrier - Unable to calculate linehaul charge\n{@0}", ratedLoad);
                return totalCost;
            }

            results.AddLog("create a new Carriers by Cost item");
            Logger.Information("RateCarrierTariff.rateCarrier - create a new Carriers by Cost Item");
            DTO.CarriersByCost carriersByCost = new DTO.CarriersByCost();
            if (ratedLoad.TotalLineHaul < 1)
            {
                carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_ZeroMilesUsingMinCharge);
            }
            //Modified by RHR 6/9/2014 v-6.4.  New logic approved for how discounts and minimum charges are applied.
            //1.if the NegotiatedMinimum charge exists it will replace the published tariff minimum charge.
            //2.if the NegotiatedMinimum charge is less than the line haul minus the discount then we replace 
            //  the line haul with the NegotiatedMinimum charge and set the discount to zero. 
            //3.if the NegotiatedMinimum charge is empty or zero we use the published tariff minimum charge.
            //4.If a discount exists we apply the discount to the minimum charge and the line haul.
            //5.If the discounted minimum charge is less than the discounted line haul we repalce the line haul with 
            //  the minimumn charge and set the discount based on the discount applied to the minimum charge.
            //6.If the line haul is not modified by a minimum charge we set the discount based on the disctoun applied to the line haul.
            //7.The Carrier Cost will always be the LineHaul - the calculated discount.
            //8.Fees and taxes will be applied after the Carrier Cost has been calculated.
            results.AddLog("Check the contract for discounts.");
            Logger.Information("RateCarrierTariff.rateCarrier.getCarrTarDiscountsFiltered - Check contract for discounts");
            DTO.CarrTarDiscount[] discounts = getCarrTarDiscountsFiltered(carrierTariffsPivots[0].CarrTarControl, ref sFaultInfo);
            if (sFaultInfo != null && sFaultInfo.Count() > 0)
            {
                //this is a fault exception we may need to return a message to the user.
                //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                //and a message about why the tariff cannot be selected.                
                //add the error to the log
                Logger.Warning("RateCarrierTariff.rateCarrier.getCarrTarDiscountsFiltered Couldn't read discounts");
                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
                return -1;
            }
            decimal LineHaulDiscount = 0;
            decimal rate = 0;
            decimal minimumRate = 0;
            bool blnUsingMinDiscount = false;
            Logger.Information("Calculate the line haul discount amount");
            results.AddLog("Calculate the line haul discount amount for this load.");
            if (discounts != null)
            {
                // Get the most applicable discount record based on geographic criteria and interline.
                Logger.Information("RateCarrierTariff.rateCarrier.GetMostPreciseDiscount");
                DTO.CarrTarDiscount discount = GetMostPreciseDiscount(ratedLoad.RatedBookRevenue, discounts, interlineList, ratedLoad.TotalWgt, carrierTariffsPivots[0].CarrTarOutbound);

                if (discount != null)
                {
                    Logger.Information("RateCarrierTariff.rateCarrier.GetMostPreciseDiscount:{@0}", discount);
                    rate = discount.CarrTarDiscountRateValue;
                    minimumRate = discount.CarrTarDiscountMinValue;
                    ratedLoad.DiscountMin = minimumRate;
                    ratedLoad.DiscountRate = rate;
                    /******  Debug code added to test rounding issues ************
                    decimal dLHD = ratedLoad.TotalLineHaul * rate;
                    decimal dRounded = decimal.Round(dLHD, 2);
                    System.Diagnostics.Debug.WriteLine("Total Line Haul {0}", dLHD);
                    System.Diagnostics.Debug.WriteLine("Rounded Total Line Haul {0}", dRounded);
                    ****************************************************************************/

                    if (ratedLoad.TotalLineHaul > 0.0M)
                    {
                        LineHaulDiscount = Decimal.Round(ratedLoad.TotalLineHaul * rate, 2);
                        Logger.Information("LineHaulDiscount = Decimal.Round(ratedLoad.TotalLineHaul * rate, 2) = {0}", LineHaulDiscount);
                    }
                    if (LineHaulDiscount < minimumRate)
                    {
                        blnUsingMinDiscount = true;
                        LineHaulDiscount = minimumRate;
                        Logger.Information("LineHaulDiscount < minimumRate = true, LineHaulDiscount = minimumRate = {0}", LineHaulDiscount);
                    }
                }
                Logger.Information("No discounts were found...");
            }
            Logger.Information("Read the negotiated minimum charge table for the contract");
            results.AddLog("Read the negotiated minimum charge table for the contract");
            DTO.CarrTarMinCharge[] minCharges = getCarrTarMinChargeFiltered(carrierTariffsPivots[0].CarrTarControl, ref sFaultInfo);
            Logger.Information("Read the negotiated minimum charge table for the contract:{0}", minCharges);

            if (sFaultInfo != null && sFaultInfo.Count() > 0)
            {
                //this is a fault exception we may need to return a message to the user.
                //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                //and a message about why the tariff cannot be selected.
                Logger.Warning("RateCarrierTariff.rateCarrier.getCarrTarMinChargeFiltered Couldn't read min charges");
                //add the error to the log
                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
                return -1;
            }
            results.AddLog("Calculate the negotiated minimum charge for this load");
            decimal NegotiatedMinCharge = calcAbsoluteMinChargeForBookRev(ratedLoad.RatedBookRevenue, minCharges, interlineList, carrierTariffsPivots[0].CarrTarOutbound);
            Logger.Information("RateCarrierTariff.rateCarrier.calcAbsoluteMinChargeForBookRev = NegotiatedMinCharge = {0}", NegotiatedMinCharge);
            if (NegotiatedMinCharge > 0)
            {
                if (NegotiatedMinCharge > (ratedLoad.TotalLineHaul - LineHaulDiscount))
                {
                    results.AddLog("Using negotiaged minimum charge, published minimum charge is ignored.");
                    ratedLoad.TotalLineHaul = NegotiatedMinCharge;
                    ratedLoad.TotalDiscount = 0;
                    Logger.Information("Using negotiaged minimum charge, published minimum charge is ignored. ratedLoad.TotalLineHaul = NegotiatedMinCharge = {0}", ratedLoad.TotalLineHaul);
                    //Update ratedLoad.NegotiatedMinChargeUsed Flag
                    carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_LineHaulAdjustedByNegotiatedMinCharge, new List<string> { NegotiatedMinCharge.ToString("C2", System.Globalization.CultureInfo.CurrentCulture) });
                }
                else
                {
                    if (LineHaulDiscount != 0)
                    {
                        Logger.Information("Using published line haul with discount.");
                        results.AddLog("Using published line haul with discount.");
                    }
                    ratedLoad.TotalDiscount = LineHaulDiscount;
                    Logger.Information("Using published line haul with discount. ratedLoad.TotalDiscount = LineHaulDiscount = {0}", ratedLoad.TotalDiscount);
                    if (blnUsingMinDiscount)
                    {
                        Logger.Information("Discount min value not exceeded.");
                        results.AddLog(DTO.CarriersByCost.getMessageNotLocalizedString(DTO.CarriersByCost.MessageEnum.M_DiscountMinValueNotExceeded));
                        carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_DiscountMinValueNotExceeded);
                    }
                }
            }
            else
            {
                decimal RateMinValue = ratedPivots.Max(x => x.CarrTarEquipMatMin) ?? 0;
                decimal MinChargeDiscount = 0;
                bool blnUsingMinDiscountForMinCharge = false;
                if (RateMinValue > 0)
                {
                    Logger.Information("Calculating the minimum charge discount.  RateMinValue: {0}", RateMinValue);
                    results.AddLog("Calculating the minimum charge discount.");
                    MinChargeDiscount = Decimal.Round(RateMinValue * rate, 2);
                    results.AddLog("Checking the minimum charge discount against the minimum discount allowed.");
                    Logger.Information("Checking the minimum charge discount against the minimum discount allowed. MinChargeDiscount: {0}", MinChargeDiscount);
                    if (MinChargeDiscount < minimumRate)
                    {
                        Logger.Information("Using minimum charge discount. {0}", MinChargeDiscount);
                        blnUsingMinDiscountForMinCharge = true;
                        MinChargeDiscount = minimumRate;
                    }
                    //A minimum charge exists so check if the line haul - discount is less than the minimum charge - discount
                    Logger.Information("Checking if the line haul - discount is less than the minimum charge - discount. LineHaulDiscount: {0}, MinChargeDiscount: {1}", LineHaulDiscount, MinChargeDiscount);
                    if ((ratedLoad.TotalLineHaul - LineHaulDiscount) < (RateMinValue - MinChargeDiscount))
                    {
                        results.AddLog("Using published minimum charge for line haul.");
                        ratedLoad.TotalLineHaul = RateMinValue;
                        ratedLoad.TotalDiscount = MinChargeDiscount;
                        //Update ratedLoad.PublishedMinChargeUsed Flag
                        Logger.Information("Using published minimum charge for line haul. ratedLoad.TotalLineHaul = RateMinValue = {0}", ratedLoad.TotalLineHaul);
                        carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_LineHaulAdjustedByMinCharge, new List<string> { RateMinValue.ToString("C2", System.Globalization.CultureInfo.CurrentCulture) });
                        if (blnUsingMinDiscountForMinCharge)
                        {
                            Logger.Information("Discount min value not exceeded.");
                            results.AddLog(DTO.CarriersByCost.getMessageNotLocalizedString(DTO.CarriersByCost.MessageEnum.M_DiscountMinValueNotExceeded));
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_DiscountMinValueNotExceeded);
                        }
                    }
                    else
                    {
                        if (LineHaulDiscount != 0)
                        {
                            results.AddLog("Using published line haul with discount.");
                            Logger.Information("Using published line haul with discount. LineHaulDiscount: {0}", LineHaulDiscount);
                        }
                        ratedLoad.TotalDiscount = LineHaulDiscount;
                        if (blnUsingMinDiscount)
                        {
                            Logger.Information("Discount min value not exceeded.");
                            results.AddLog(DTO.CarriersByCost.getMessageNotLocalizedString(DTO.CarriersByCost.MessageEnum.M_DiscountMinValueNotExceeded));
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_DiscountMinValueNotExceeded);
                        }
                    }
                }
                else
                {
                    results.AddLog("Line haul minus discount exceeds minimum.");
                    if (LineHaulDiscount != 0)
                    {
                        results.AddLog("Using published line haul with discount.");
                        Logger.Information("Using published line haul with discount. LineHaulDiscount: {0}", LineHaulDiscount);
                    }
                    ratedLoad.TotalDiscount = LineHaulDiscount;
                    if (blnUsingMinDiscount)
                    {
                        Logger.Information("Discount min value not exceeded.");
                        results.AddLog(DTO.CarriersByCost.getMessageNotLocalizedString(DTO.CarriersByCost.MessageEnum.M_DiscountMinValueNotExceeded));
                        carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_DiscountMinValueNotExceeded);
                    }
                }
            }


            // Allocate the line haul charge back to each BookRevenue object.
            ///Modified by RHR 8/27/14 moved DictAlloc condition inside for loop
            //if (DictAlloc != null && DictAlloc.Count > 0)
            //{
            int nHighIndex = -1;
            double nHighPct = -1.0;
            decimal nSum = 0.0M;
            decimal allocatedLineHaulCharge;
            double pct = 0;
            double dbllLoadWeight = 0;
            double dblOrderWeight = 0;
            for (int i = 0; i < ratedLoad.BookRevenues.Length; i++)
            {
                Logger.Information("RateCarrierTariff.rateCarrier.AllocateLineHaulCharge for bookRevenue[{0}]", i);
                // Find the percentage of the total freight this load should have.                
                if (DictAlloc != null && DictAlloc.Count > 0)
                {
                    pct = DictAlloc[ratedLoad.BookRevenues[i].BookControl];
                    Logger.Information("RateCarrierTariff.rateCarrier.AllocateLineHaulCharge - DictAlloc: {0}", pct);
                }
                else
                {
                    dbllLoadWeight = ratedLoad.TotalWgt;
                    dblOrderWeight = ratedLoad.BookRevenues[i].BookTotalWgt;
                    if (dbllLoadWeight <= 0 || dblOrderWeight < 0)
                    {

                        pct = 0;
                        Logger.Information("RateCarrierTariff.rateCarrier.AllocateLineHaulCharge - dbllLoadWeight <= 0 || dblOrderWeight < 0");
                    }
                    else
                    {
                        pct = dblOrderWeight / dbllLoadWeight;
                        Logger.Information("RateCarrierTariff.rateCarrier.AllocateLineHaulCharge - pct: {0}", pct);
                    }

                }
                if (pct > nHighPct)
                {
                    nHighIndex = i;
                    nHighPct = pct;
                    Logger.Information("RateCarrierTariff.rateCarrier.AllocateLineHaulCharge - nHighIndex: {0}, nHighPct: {1}", nHighIndex, nHighPct);
                }
                allocatedLineHaulCharge = decimal.Round((ratedLoad.TotalLineHaul * (decimal)pct), 2);

                ratedLoad.BookRevenues[i].BookRevLineHaul = allocatedLineHaulCharge;
                nSum += allocatedLineHaulCharge;
                Logger.Information("RateCarrierTariff.rateCarrier.AllocateLineHaulCharge - allocatedLineHaulCharge: {0}, nSum: {1}", allocatedLineHaulCharge, nSum);
            }
            if (nHighIndex >= 0 && nSum != ratedLoad.TotalLineHaul)
            {
                ratedLoad.BookRevenues[nHighIndex].BookRevLineHaul += (ratedLoad.TotalLineHaul - nSum);
                Logger.Information("RateCarrierTariff.rateCarrier.AllocateLineHaulCharge - nHighIndex [{2}] >= 0 && nSum [{0}] != ratedLoad.TotalLineHaul [{1}]", nSum, ratedLoad.TotalLineHaul, nHighIndex);
            }


            // Discount allocation.
            ///Modified by RHR 8/27/14 moved DictAlloc condition inside for loop
            //if (DictAlloc != null && DictAlloc.Count > 0)
            //{
            // We have information from class rating (LTL) to help refine the discounts.
            nHighIndex = -1;
            nHighPct = -1.0;
            nSum = 0.0M;
            decimal allocatedDiscount;
            for (int i = 0; i < ratedLoad.BookRevenues.Length; i++)
            {
                // Find the percentage of the total freight this load should have.
                if (DictAlloc != null && DictAlloc.Count > 0)
                {
                    pct = DictAlloc[ratedLoad.BookRevenues[i].BookControl];
                    Logger.Information("RateCarrierTariff.rateCarrier.AllocateDiscount - DictAlloc: {0}", pct);
                }
                else
                {
                    dbllLoadWeight = ratedLoad.TotalWgt;
                    dblOrderWeight = ratedLoad.BookRevenues[i].BookTotalWgt;
                    if (dbllLoadWeight <= 0 || dblOrderWeight < 0)
                    {
                        pct = 0;
                        Logger.Information("RateCarrierTariff.rateCarrier.AllocateDiscount - dbllLoadWeight [{0}] <= 0 || dblOrderWeight [{1}] < 0", dbllLoadWeight, dblOrderWeight);
                    }
                    else
                    {
                        pct = dblOrderWeight / dbllLoadWeight;
                        Logger.Information("RateCarrierTariff.rateCarrier.AllocateDiscount - pct: {0}", pct);
                    }
                }
                if (pct > nHighPct)
                {
                    nHighIndex = i;
                    nHighPct = pct;
                    Logger.Information("RateCarrierTariff.rateCarrier.AllocateDiscount - nHighIndex: {0}, nHighPct: {1}", nHighIndex, nHighPct);
                }
                allocatedDiscount = decimal.Round((ratedLoad.TotalDiscount * (decimal)pct), 2);

                ratedLoad.BookRevenues[i].BookRevDiscount = allocatedDiscount;
                nSum += allocatedDiscount;
                Logger.Information("RateCarrierTariff.rateCarrier.AllocateDiscount - allocatedDiscount: {0}, nSum: {1}", allocatedDiscount, nSum);
            }
            if (nHighIndex >= 0 && nSum != ratedLoad.TotalDiscount)
            {
                ratedLoad.BookRevenues[nHighIndex].BookRevDiscount += (ratedLoad.TotalDiscount - nSum);
                Logger.Information("RateCarrierTariff.rateCarrier.AllocateDiscount - nHighIndex [{2}] >= 0 && nSum [{0}] != ratedLoad.TotalDiscount [{1}]", nSum, ratedLoad.TotalDiscount, nHighIndex);
            }


            results.AddLog("Update the carrier costs.");
            for (int i = 0; i < ratedLoad.BookRevenues.Length; i++)
            {
                ratedLoad.BookRevenues[i].BookRevCarrierCost = ratedLoad.BookRevenues[i].BookRevLineHaul - ratedLoad.BookRevenues[i].BookRevDiscount;
                Logger.Information("RateCarrierTariff.rateCarrier.UpdateCarrierCost - BookRevCarrierCost[{1}]: {0}", ratedLoad.BookRevenues[i].BookRevCarrierCost, i);
            }

            results.AddLog("Calculate all tariff and non-tariff fees then allocate them back to each order.");
            //add ref to DTO.CarrierCostResults results and DTO.CarriersByCost carriersByCost 
            Logger.Information("RateCarrierTariff.rateCarrier.CalculateAndAllocateAllFees");
            CarrierFeesInstance.calculateAndAllocateAllFees(carrierTariffsPivots[0].CarrTarControl, ref ratedLoad, ref results, ref carriersByCost);
            Logger.Information("RateCarrierTariff.rateCarrier.CalculateAndAllocateAllFees - Done");

            Logger.Information("Calculate the pickup, stop off, and fuel surcharges.");
            results.AddLog("Calculate the pickup, stop off, and fuel surcharges.");
            // NGLBookRevenueData.getLegacyStopPickandFuelChargesForLoad(ratedLoad.TotalLineHaul, ratedLoad.BookRevenues);
            string message = String.Empty;
            DTO.BookRevenue[] bookRevenues = ratedLoad.BookRevenues;
            decimal totalStopCharges = 0, totalPickCharges = 0, totalFuelCost = 0;
            NGLBookRevenueData.getLegacyStopPickandFuelChargesForLoad(ref bookRevenues,
                ref totalStopCharges, ref totalPickCharges, ref totalFuelCost,
                (ratedLoad.TotalLineHaul - ratedLoad.TotalDiscount), carrierTariffsPivots[0].CarrierControl, true, carrierTariffsPivots[0].CarrTarControl, carrierTariffsPivots[0].CarrTarEquipControl, ref message);
            ratedLoad.TotalStopCharges = totalStopCharges;
            ratedLoad.TotalPickCharges = totalPickCharges;

            Logger.Information("Legacy Stop and Fuel Charges: TotalStopCharges: {totalStopCharges}, TotalPickCharges: {totalPickCharges}, TotalFuelCost: {totalFuelCost}", totalStopCharges, totalPickCharges, totalFuelCost);

            if (totalFuelCost <= 0)
            {
                Logger.Information("totalFuelCost {0} is less than or equal to zero.  Calculating fuel cost.", totalFuelCost);
                int[] iCodes = { 2, 9, 15 };

                foreach (DTO.BookRevenue b in bookRevenues)
                {
                    Logger.Information("Calculating fuel cost for BookControl: {BookControl}", b.BookControl);
                    totalFuelCost += (from f in b.BookFees
                                      where iCodes.Contains(f.BookFeesAccessorialCode)
                                      select f.BookFeesValue).Sum();
                    Logger.Information("Total Fuel Cost: {0}", totalFuelCost);
                }


            }

            ratedLoad.TotalFuelCost = totalFuelCost;
            NGLLECarrierAccessorialData leCarrierAccessorials = new NGLLECarrierAccessorialData(Parameters);
            Logger.Information("Checking to see if any book fees exist that are not associated with tblLECarrAccessorials");
            foreach (BookRevenue bookRev in load.BookRevenues)
            {
                foreach (var bookfee in bookRev.BookFees)
                {
                    var foundAccessorials = leCarrierAccessorials.GetLECarrierAccessorialsByCarrierControl(carrierTariffsPivots[0].CarrierControl);
                    if (foundAccessorials != null && foundAccessorials.Any())
                    {
                        Logger.Information("Validating Accessorials for {AccessorialCode}", bookfee.BookFeesAccessorialCode);
                        var found = foundAccessorials.Any(x => x.LECAAccessorialCode == bookfee.BookFeesAccessorialCode);

                        if (!found)
                        {
                            Logger.Information("Invalid book fee found for carrier {CarrierName}, Fee: {BookFee}", carrierTariffsPivots[0].CarrierName, bookfee);
                            //var invalidCarrier = createInvalidCarrier(carrierTariffsPivots[0], ratedLoad);
                            //invalidCarrier.AddMessage("m_InvalidFee", $"BookFee {bookfee.BookFeesAccessorialCode} is not supported by this carrier.  If this is a mistake, update the Carrier to include", true);
                            //results.CarriersByCost.Add(invalidCarrier);

                            results.Success = false;
                            results.AddLog("Invalid book fee found for carrier " + carrierTariffsPivots[0].CarrierName);
                            return totalCost;
                        }
                    }

                    //if (!isBookFeeValid(bookfee))
                    //{
                    //    results.CarriersByCost.Add(createInvalidCarrier(carrierTariffsPivots[0], ratedLoad, DTO.CarriersByCost.MessageEnum.M_InvalidBookFee, new List<string> { bookRev.BookCarrOrderNumber }));
                    //    results.AddLog("Invalid book fee found for carrier " + carrierTariffsPivots[0].CarrierName);
                    //    return totalCost;
                    //}
                }
            }


            results.AddLog("Compute the total cost of the load.");
            //Modified by RHR v-7.0.5.102 9/9/2016 fixed bug where pivot data was not being passed to UpdateBookRevenues

            totalCost = UpdateBookRevenues(ratedPivots[0], ratedLoad, ref results, ref carriersByCost);
            Logger.Information("RateCarrierTariff.rateCarrier.UpdateBookRevenues - TotalCost: {totalCost}", totalCost);
            results.AddLog("update the list of available Carriers.");
            newCarriersByCost = updateCarriersByCost(ref carriersByCost, ratedPivots[0], ratedLoad);
            if (!carriersByCost.HasCarrierBeenInvalidated)
            {
                Logger.Information("RateCarrierTariff.rateCarrier.updateCarriersByCost - newCarriersByCost: {@0}", newCarriersByCost);
                results.CarriersByCost.Add(newCarriersByCost);
            }
            return totalCost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <param name="load"></param>
        /// <param name="ratedLoad"></param>
        /// <param name="autocalculateBFC"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.102 8/9/2016 
        ///     Added new parameters to UpdateBookRevenues method
        /// </remarks>
        public decimal recalculateUsingLineHaul(ref DTO.CarrierCostResults results, Load load, ref Load ratedLoad, bool autocalculateBFC = true)
        {
            decimal totalCost = -1;
            using (var operation = Logger.StartActivity("RateCarrierTariff.recalculateUsingLineHaul - Load: {@0}, RatedLoad: {@1}, AutoCalculateBFC: {2}", load, ratedLoad, autocalculateBFC))
            {
                if (results == null) { results = new DTO.CarrierCostResults(); }

                ratedLoad = null;

                // Make a deep copy of the load so that we can update the copy with the rating results.
                ratedLoad = DeepCopy(load);
                ratedLoad.Parameters = load.Parameters;

                DTO.CarriersByCost carriersByCost = new DTO.CarriersByCost();
                // No need to calculate the total line haul charge.  We just want to recalculate the accessorial fees.
                ratedLoad.TotalLineHaul = ratedLoad.sumTotalLineHaul();
                if (ratedLoad.TotalLineHaul <= 0 && (ratedLoad.RatedBookRevenue.BookCarrTarName != "SPOT RATE" || getCompanyLevelParameterValue(ratedLoad.RatedBookRevenue.BookCustCompControl, "AllowZeroLineHaulOnSpotRate") == 0))
                {
                    Logger.Warning("Total Line Haul is less than or equal to zero.  Cannot rate the load.");
                    operation.Complete();
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_InvalidLineHaulCannotRateLoad);
                    return totalCost;
                }
                results.AddLog(string.Format("Total Line Haul = {0}", ratedLoad.TotalLineHaul));
                results.AddLog("Calculate and Allocate All Fees");
                // Calculate all tariff and non-tariff fees and allocate them back to the BookRevenue objects.
                //add ref to DTO.CarrierCostResults results and DTO.CarriersByCost carriersByCost = new DTO.CarriersByCost();
                CarrierFeesInstance.calculateAndAllocateAllFees(ratedLoad.BookRevenues[0].BookCarrTarControl, ref ratedLoad, ref results, ref carriersByCost);

                // Calculate the pickup, stop off, and fuel surcharges.
                // NGLBookRevenueData.getLegacyStopPickandFuelChargesForLoad(ratedLoad.TotalLineHaul, ratedLoad.BookRevenues);
                string message = String.Empty;
                DTO.BookRevenue[] bookRevenues = ratedLoad.BookRevenues;
                decimal totalStopCharges = 0, totalPickCharges = 0, totalFuelCost = 0;
                results.AddLog("Get Legacy Stop, Pick and Fuel Charges for Load");
                //Modified by RHR 9/19/14 we now use TotalCarrierCost (automatically sums totals) replacing (ratedLoad.TotalLineHaul - ratedLoad.TotalDiscount) formula which may not be correct because the values are not being set
                NGLBookRevenueData.getLegacyStopPickandFuelChargesForLoad(ref bookRevenues,
                    ref totalStopCharges, ref totalPickCharges, ref totalFuelCost,
                    (ratedLoad.TotalCarrierCost),
                    ratedLoad.BookRevenues[0].BookCarrierControl, true,
                    ratedLoad.BookRevenues[0].BookCarrTarControl,
                    ratedLoad.BookRevenues[0].BookCarrTarEquipControl, ref message);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_LegacyStopPickandFuelProblem, false, null);
                    results.AddLog(message);
                }
                ratedLoad.TotalStopCharges = totalStopCharges;
                ratedLoad.TotalPickCharges = totalPickCharges;
                if (totalFuelCost <= 0)
                {
                    int[] iCodes = { 2, 9, 15 };
                    foreach (DTO.BookRevenue b in bookRevenues)
                    {
                        Logger.Information("Calculating fuel cost for BookRevenue: {@0}, BookFees: {@1}", b, b.BookFees);
                        totalFuelCost += (from f in b.BookFees
                                          where iCodes.Contains(f.BookFeesAccessorialCode)
                                          select f.BookFeesValue).Sum();
                        Logger.Information("Total Fuel Cost: {0}", totalFuelCost);
                    }


                }
                ratedLoad.TotalFuelCost = totalFuelCost;
                results.AddLog("Update total cost and revenue for load");
                // Compute the total cost of the load and update the BookRevenue objects.
                //Modified by RHR v-7.0.5.102 8/9/2016 
                Logger.Information("UpdateBookRevenues - recalculateUsingLineHaul, ratedLoad: {@0}, results: {@1}", ratedLoad, results);
                totalCost = UpdateBookRevenues(null, ratedLoad, ref results, ref carriersByCost, autocalculateBFC);

                results.AddLog("update the list of selected Carriers.");
                results.CarriersByCost.Add(updateCarriersByCost(ref carriersByCost, ratedLoad));
            }
            return totalCost;
        }


        /// <summary>
        /// Deprecated: do not use this method  we now query the db directly via isBookRevInterline method
        /// Previously used to Return true or false if the bookrev is an interline point 
        /// </summary>
        /// <param name="bookRev">Current book revenue object.</param>
        /// <param name="interlines">List of interline records.</param>
        /// <returns> 
        /// TODO: we need to test the level of precision to be sure we have the best match for interline points.
        /// </returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.100 6/2/2016  
        ///   we now query the db directly using isBookRevInterline instead of storing the interline information in memory
        ///   there are too many records and the system performance would be affected.
        /// </remarks>
        public bool GetInterlineForBookRev(DTO.BookRevenue bookRev, DTO.CarrTarInterline[] interlines, ref List<int> interlineList)
        {
            bool blnRet = false;
            if (bookRev == null || interlines == null || interlines.Count() < 1) { return false; }
            //check the origin address for interline (generally used for inbound
            blnRet =
                (from d in interlines
                 where
                 (
                     (string.IsNullOrWhiteSpace(d.CarrTarInterlineCountry) || d.CarrTarInterlineCountry.ToUpper() == bookRev.BookOrigCountry.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarInterlineState) || d.CarrTarInterlineState.ToUpper() == bookRev.BookOrigState.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarInterlineCity) || d.CarrTarInterlineCity.ToUpper() == bookRev.BookOrigCity.ToUpper())
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarInterlineEffDateFrom.HasValue || d.CarrTarInterlineEffDateFrom.Value.Date <= bookRev.BookDateLoad.Value.Date))
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarInterlineEffDateTo.HasValue || d.CarrTarInterlineEffDateTo.Value.Date >= bookRev.BookDateLoad.Value.Date))
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarInterlineZip) || string.IsNullOrWhiteSpace(bookRev.BookOrigZip) || bookRev.BookOrigZip.Trim().Substring(0, d.CarrTarInterlineZip.Trim().Length).ToUpper().CompareTo(d.CarrTarInterlineZip.Trim().ToUpper()) == 0)
                 )
                 select d).Any();
            //if the origin is ok check the destination.  
            if (!blnRet)
            {
                blnRet =
                (from d in interlines
                 where
                 (
                     (string.IsNullOrWhiteSpace(d.CarrTarInterlineCountry) || d.CarrTarInterlineCountry.ToUpper() == bookRev.BookDestCountry.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarInterlineState) || d.CarrTarInterlineState.ToUpper() == bookRev.BookDestState.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarInterlineCity) || d.CarrTarInterlineCity.ToUpper() == bookRev.BookDestCity.ToUpper())
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarInterlineEffDateFrom.HasValue || d.CarrTarInterlineEffDateFrom.Value.Date <= bookRev.BookDateLoad.Value.Date))
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarInterlineEffDateTo.HasValue || d.CarrTarInterlineEffDateTo.Value.Date >= bookRev.BookDateLoad.Value.Date))
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarInterlineZip) || string.IsNullOrWhiteSpace(bookRev.BookDestZip) || bookRev.BookDestZip.Trim().Substring(0, d.CarrTarInterlineZip.Trim().Length).ToUpper().CompareTo(d.CarrTarInterlineZip.Trim().ToUpper()) == 0)
                 )
                 select d).Any();
            }
            if (blnRet)
            {
                if (interlineList == null) { interlineList = new List<int>(); }
                interlineList.Add(bookRev.BookControl);
            }
            return blnRet;
        }

        /// <summary>
        /// returns true or false if the booking revenue data is an interline point.  
        /// Updates interlinelist with the book control if the result is false.
        /// Replaces both GetInterlineForBookRev and getCarrTarInterlinesFiltered methods
        /// </summary>
        /// <param name="CarrTarControl"></param>
        /// <param name="bookRev"></param>
        /// <param name="interlineList"></param>
        /// <param name="sFaultInfo"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.100 6/2/2016  
        ///   we now query the db directly via IsInterline instead of storing the interline information in memory
        ///   there are too many records and the system performance would be affected
        /// </remarks>
        public bool isBookRevInterline(int CarrTarControl, DTO.BookRevenue bookRev, ref List<int> interlineList, ref List<string> sFaultInfo)
        {
            bool blnRet = false;
            if (bookRev == null) { return false; }
            string Country = bookRev.BookOrigCountry.ToUpper();
            string City = bookRev.BookOrigCity.ToUpper();
            string State = bookRev.BookOrigState.ToUpper();
            string Zip = bookRev.BookOrigZip;
            DateTime? FromDate = bookRev.BookDateLoad;
            DateTime? ToDate = bookRev.BookDateLoad;
            //check the origin address for interline (generally used for inbound)
            try
            {
                blnRet = InterlineInstance.IsInterline(CarrTarControl, Country, State, City, FromDate, ToDate, Zip);
                //if the origin is ok check the destination.  
                if (!blnRet)
                {
                    Country = bookRev.BookDestCountry.ToUpper();
                    City = bookRev.BookDestCity.ToUpper();
                    State = bookRev.BookDestState.ToUpper();
                    Zip = bookRev.BookDestZip;
                    blnRet = InterlineInstance.IsInterline(CarrTarControl, Country, State, City, FromDate, ToDate, Zip);
                }

            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                string errMsg = Util.formatFaultInfo(sqlEx, ref sFaultInfo);
                if (!string.IsNullOrWhiteSpace(errMsg))
                {
                    SaveSysError(errMsg, sourcePath("isBookRevInterline"), CarrTarControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            if (blnRet)
            {
                if (interlineList == null) { interlineList = new List<int>(); }
                interlineList.Add(bookRev.BookControl);
            }
            return blnRet;
        }

        /// <summary>
        /// Returns the value of the minimum charge for the array of minimum charges provided (typically filtered by tariff) or zero if none exist.
        /// </summary>
        /// <param name="bookRev">Current book revenue object.</param>
        /// <param name="minCharges">List of minimum charges</param>
        /// <param name="minCharges">List of interlineList bookcontrol nunmbers</param>
        /// <returns> 
        /// TODO: we need to test the level of precision to be sure we have the best match for interline points.
        /// </returns>
        public decimal calcAbsoluteMinChargeForBookRev(DTO.BookRevenue bookRev,
            DTO.CarrTarMinCharge[] minCharges,
            List<int> interlineList,
            bool Outbound)
        {
            decimal decRet = 0;
            Logger.Information("calcAbsoluteMinChargeForBookRev - bookRev: {@0}, minCharges: {@1}, interlineList: {@2}, Outbound: {@3}", bookRev, minCharges, interlineList, Outbound);
            if (bookRev == null || minCharges == null || minCharges.Count() < 1) { return 0; }
            bool blnInterline = interlineList.Contains(bookRev.BookControl);
            string Country;
            string State;
            string City;
            string Zip;
            if (Outbound)
            {
                Country = bookRev.BookDestCountry;
                State = bookRev.BookDestState;
                City = bookRev.BookDestCity;
                Zip = bookRev.BookDestZip;
                Logger.Information("calcAbsoluteMinChargeForBookRev - Outbound: Country: {0}, State: {1}, City: {2}, Zip: {3}", Country, State, City, Zip);

            }
            else
            {
                Country = bookRev.BookOrigCountry;
                State = bookRev.BookOrigState;
                City = bookRev.BookOrigCity;
                Zip = bookRev.BookOrigZip;
                Logger.Information("calcAbsoluteMinChargeForBookRev - Inbound: Country: {0}, State: {1}, City: {2}, Zip: {3}", Country, State, City, Zip);
            }

            //get the mincharge

            Logger.Information("calcAbsoluteMinChargeForBookRev - minCharges: {@0}", minCharges);
            decRet =
                (from d in minCharges
                 where
                 (
                    (
                        (blnInterline == true && d.CarrTarMinChargePointTypeControl == (int)DAL.Utilities.PointType.Interline)
                        ||
                        (blnInterline == false && d.CarrTarMinChargePointTypeControl == (int)DAL.Utilities.PointType.Direct)
                        ||
                        d.CarrTarMinChargePointTypeControl == (int)DAL.Utilities.PointType.Any
                    )
                    &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinChargeCountry) || d.CarrTarMinChargeCountry.ToUpper() == Country.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinChargeState) || d.CarrTarMinChargeState.ToUpper() == State.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinChargeCity) || d.CarrTarMinChargeCity.ToUpper() == City.ToUpper())
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarMinChargeEffDateFrom.HasValue || d.CarrTarMinChargeEffDateFrom.Value.Date <= bookRev.BookDateLoad.Value.Date))
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarMinChargeEffDateTo.HasValue || d.CarrTarMinChargeEffDateTo.Value.Date >= bookRev.BookDateLoad.Value.Date))
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinChargeZipFrom) || string.IsNullOrWhiteSpace(Zip) || Zip.Trim().Substring(0, d.CarrTarMinChargeZipFrom.Trim().Length).ToUpper().CompareTo(d.CarrTarMinChargeZipFrom.Trim().ToUpper()) >= 0) //zip is equal to or follows CarrTarMinChargeZipFrom
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinChargeZipTo) || string.IsNullOrWhiteSpace(Zip) || Zip.Trim().Substring(0, d.CarrTarMinChargeZipTo.Trim().Length).ToUpper().CompareTo(d.CarrTarMinChargeZipTo.Trim().ToUpper()) <= 0) //zip is equal to or preceeds CarrTarDiscountZipTo
                 )
                 orderby
                    d.CarrTarMinChargeCity descending,
                    d.CarrTarMinChargeState descending,
                    d.CarrTarMinChargeCountry descending,
                    d.CarrTarMinChargeZipFrom descending,
                    d.CarrTarMinChargeZipTo ascending,
                    d.CarrTarMinChargeValue descending
                 select d.CarrTarMinChargeValue).FirstOrDefault();

            return decRet;
        }






        /// <summary>
        /// Deprecated: do not use this method  we now query the db directly via isBookRevNonServicePoint method
        /// Previously used to Return True or false if the bookrev is a non service point
        /// </summary>
        /// <param name="bookRev">Current book revenue object</param>
        /// <param name="nonServs">List of non-service objects.</param>
        /// <returns>
        /// Modified by RHR v-7.0.5.100 6/2/2016  
        ///  we now query the db directly using isBookRevNonServicePoint instead of storing the non-service information in memory
        ///   there are too many records and the system performance would be affected.
        /// </returns>
        public bool GetNonServForBookRev(DTO.BookRevenue bookRev, DTO.CarrTarNonServ[] nonServs)
        {
            bool blnRet = false;
            if (bookRev == null || nonServs == null || nonServs.Count() < 1) { return false; }
            //check th origin address (we cannot pickup from non service points)
            blnRet =
                (from d in nonServs
                 where
                 (
                     (string.IsNullOrWhiteSpace(d.CarrTarNonServState) || d.CarrTarNonServState.ToUpper() == bookRev.BookOrigState.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarNonServCountry) || d.CarrTarNonServCountry.ToUpper() == bookRev.BookOrigCountry.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarNonServCity) || d.CarrTarNonServCity.ToUpper() == bookRev.BookOrigCity.ToUpper())
                     &&
                     (!d.CarrTarNonServEffDateFrom.HasValue || d.CarrTarNonServEffDateFrom.Value.Date <= bookRev.BookDateLoad.Value.Date)
                     &&
                     (!d.CarrTarNonServEffDateTo.HasValue || d.CarrTarNonServEffDateTo.Value.Date >= bookRev.BookDateLoad.Value.Date)
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarNonServZip) || string.IsNullOrWhiteSpace(bookRev.BookOrigZip) || bookRev.BookOrigZip.Trim().Substring(0, d.CarrTarNonServZip.Trim().Length).ToUpper().CompareTo(d.CarrTarNonServZip.Trim().ToUpper()) == 0)
                 )
                 select d).Any();
            //if the origin is ok check the destination.  cannot deliver to non service point.
            if (!blnRet)
            {
                blnRet =
                    (from d in nonServs
                     where
                     (
                         (string.IsNullOrWhiteSpace(d.CarrTarNonServState) || d.CarrTarNonServState.ToUpper() == bookRev.BookDestState.ToUpper())
                         &&
                         (string.IsNullOrWhiteSpace(d.CarrTarNonServCountry) || d.CarrTarNonServCountry.ToUpper() == bookRev.BookDestCountry.ToUpper())
                         &&
                         (string.IsNullOrWhiteSpace(d.CarrTarNonServCity) || d.CarrTarNonServCity.ToUpper() == bookRev.BookDestCity.ToUpper())
                         &&
                         (!d.CarrTarNonServEffDateFrom.HasValue || d.CarrTarNonServEffDateFrom.Value.Date <= bookRev.BookDateLoad.Value.Date)
                         &&
                         (!d.CarrTarNonServEffDateTo.HasValue || d.CarrTarNonServEffDateTo.Value.Date >= bookRev.BookDateLoad.Value.Date)
                         &&
                         (string.IsNullOrWhiteSpace(d.CarrTarNonServZip) || string.IsNullOrWhiteSpace(bookRev.BookDestZip) || bookRev.BookDestZip.Trim().Substring(0, d.CarrTarNonServZip.Trim().Length).ToUpper().CompareTo(d.CarrTarNonServZip.Trim().ToUpper()) == 0)
                     )
                     select d).Any();
            }
            return blnRet;
        }

        /// <summary>
        /// returns true or false if the booking revenue data is an interline point.
        /// Replaces both GetNonServForBookRev and getCarrTarNonServsFiltered methods
        /// </summary>
        /// <param name="CarrTarControl"></param>
        /// <param name="bookRev"></param>
        /// <param name="sFaultInfo"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.100 6/2/2016  
        ///   we now query the db directly via IsNonServicePoint instead of storing the non-service information in memory
        ///   there are too many records and the system performance would be affected
        /// </remarks>
        public bool isBookRevNonServicePoint(int CarrTarControl, DTO.BookRevenue bookRev, ref List<string> sFaultInfo)
        {
            Logger.Information("RateCarrierTariff.isBookRevNonServicePoint");
            bool blnRet = false;
            if (bookRev == null) { return false; }
            string Country = bookRev.BookOrigCountry.ToUpper();
            string City = bookRev.BookOrigCity.ToUpper();
            string State = bookRev.BookOrigState.ToUpper();
            string Zip = bookRev.BookOrigZip;
            DateTime? FromDate = bookRev.BookDateLoad;
            DateTime? ToDate = bookRev.BookDateLoad;
            //check the origin address for Non-Service Points (generally used for inbound)
            try
            {
                Logger.Information("RateCarrierTariff.isBookRevNonServicePoint - Call NonServInstance.IsNonServicePoint");
                blnRet = NonServInstance.IsNonServicePoint(CarrTarControl, Country, State, City, FromDate, ToDate, Zip);
                //if the origin is ok check the destination.  
                if (!blnRet)
                {
                    Country = bookRev.BookDestCountry.ToUpper();
                    City = bookRev.BookDestCity.ToUpper();
                    State = bookRev.BookDestState.ToUpper();
                    Zip = bookRev.BookDestZip;
                    Logger.Information("RateCarrierTariff.isBookRevNonServicePoint - blnRet: {@blnRet}", blnRet);
                    blnRet = NonServInstance.IsNonServicePoint(CarrTarControl, Country, State, City, FromDate, ToDate, Zip);
                    Logger.Information("RateCarrierTariff.isBookRevNonServicePoint - blnRet: {@blnRet}", blnRet);
                }
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                Logger.Error(sqlEx, "RateCarrierTariff.isBookRevNonServicePoint");
                string errMsg = Util.formatFaultInfo(sqlEx, ref sFaultInfo);
                if (!string.IsNullOrWhiteSpace(errMsg))
                {
                    SaveSysError(errMsg, sourcePath("isBookRevNonServicePoint"), CarrTarControl.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "RateCarrierTariff.isBookRevNonServicePoint");
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                //throw;
            }

            return blnRet;
        }


        ///// <summary>
        ///// Returns absolute(floor) minimum charge.
        ///// </summary>
        ///// <param name="carrierTariffsPivots">Tariff pivot records</param>
        ///// <param name="interlineList">List of interline status by book revenue object.</param>
        ///// <param name="ratedLoad">Current load we are rating.</param>
        ///// <returns></returns>
        //private decimal calcMinimumCharge(DTO.CarrierTariffsPivot[] carrierTariffsPivots, List<int> interlineList, Load ratedLoad)
        //{
        //    decimal nMinimum = 0.0M;
        //    DTO.CarrTarMinCharge[] minCharges = getCarrTarMinChargesFiltered(carrierTariffsPivots[0].CarrTarControl);
        //    if (minCharges != null)
        //    {
        //        // Get the most applicalbe discount record based on geographic criteria and interline.
        //        DTO.CarrTarMinCharge minCharge = GetMostPreciseMinCharge(ratedLoad, minCharges, interlineList, carrierTariffsPivots[0]);
        //        if (minCharge != null)
        //        {
        //           nMinimum = minCharge.CarrTarMinChargeValue;
        //        }
        //    }
        //    return nMinimum;
        //}

        ///// <summary>
        ///// Get the most precise minimum charge record.
        ///// </summary>
        ///// <param name="load">Load we are rating.</param>
        ///// <param name="minCharges">List of partially filtered minimum charge records.</param>
        ///// <param name="InterlineList">List mapping book revenue objects to interline status.</param>
        ///// <param name="pivot">Tariff pivot record.</param>
        ///// <returns></returns>
        //public DTO.CarrTarMinCharge GetMostPreciseMinCharge(Load load, DTO.CarrTarMinCharge[] minCharges, List<int> InterlineList, DTO.CarrierTariffsPivot pivot)
        //{
        //    DTO.CarrTarMinCharge MinCharge = null;
        //    DAL.Utilities.PointType eInterlineStatus = DAL.Utilities.PointType.Direct;
        //    DTO.BookRevenue lastStop = load.LastStopBookRevenue;

        //    if (lastStop == null) // Shouldn't happen.
        //    {
        //        return MinCharge;
        //    }
        //    // Determine what we are looking for, interline or directs.
        //    if (InterlineList != null && InterlineList.Count > 0)
        //    {
        //        // At least one point is interline.
        //        eInterlineStatus = DAL.Utilities.PointType.Interline;
        //    }

        //    try
        //    {
        //        // Look up minimum charge using last stop's book revenue.
        //        MinCharge =
        //            (from d in minCharges
        //             where ((d.CarrTarMinChargeState == lastStop.BookDestState ||
        //                     d.CarrTarMinChargeState.Length == 0) &&
        //                    (d.CarrTarMinChargeCountry == lastStop.BookDestCountry ||
        //                     d.CarrTarMinChargeCountry.Length == 0) &&
        //                    (d.CarrTarMinChargeCity == lastStop.BookDestCity ||
        //                     d.CarrTarMinChargeCity.Length == 0) &&
        //                     (d.CarrTarMinChargePointTypeControl == (int)DAL.Utilities.PointType.Any ||
        //                      d.CarrTarMinChargePointTypeControl == (int)eInterlineStatus) &&
        //                    (!d.CarrTarMinChargeEffDateFrom.HasValue ||
        //                     d.CarrTarMinChargeEffDateFrom.Value.Date <= lastStop.BookDateLoad.Value.Date) &&
        //                    (!d.CarrTarMinChargeEffDateTo.HasValue ||
        //                     d.CarrTarMinChargeEffDateTo.Value.Date >= lastStop.BookDateLoad.Value.Date) &&
        //                    (d.CarrTarMinChargeZipFrom.Length == 0 ||
        //                     lastStop.BookDestZip.TrimEnd().Substring(0, d.CarrTarMinChargeZipFrom.Length).CompareTo(d.CarrTarMinChargeZipFrom) >= 0) &&
        //                    (d.CarrTarMinChargeZipTo.Length == 0 ||
        //                     lastStop.BookDestZip.TrimEnd().Substring(0, d.CarrTarMinChargeZipTo.Length).CompareTo(d.CarrTarMinChargeZipTo) <= 0))
        //             // These fields are not unique, so we must get the most precise record.
        //             orderby
        //              d.CarrTarMinChargeZipTo,
        //              d.CarrTarMinChargeCity,
        //              d.CarrTarMinChargeState,
        //              d.CarrTarMinChargeCountry,
        //              d.CarrTarMinChargeEffDateFrom,
        //              d.CarrTarMinChargePointTypeControl
        //              descending
        //             select d).FirstOrDefault();
        //            }
        //    catch (FaultException<DAL.SqlFaultInfo> sqlEx)
        //    {
        //        if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
        //        {
        //            // "E_SQLExceptionMSG", E_UnExpectedMSG
        //            string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
        //                sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
        //            SaveSysError(errMsg, sourcePath("GetMostPreciseMinCharge"), pivot.CarrTarControl.ToString());
        //        }
        //    }

        //    // Just return if there is an applicable record.
        //    return MinCharge;
        //}

        /// <summary>
        /// Get the most precise discount record.
        /// </summary>
        /// <param name="bookRev">book record used to rate this load</param>
        /// <param name="discounts">List of partially filtered discount records.</param>
        /// <param name="InterlineList">List of Interline book controls for this load</param>
        /// <param name="totalWeight">Total weight for the load</param>
        /// <returns>
        /// TODO: add logic to filter or sort using [CarrTarDiscountMinValue]?  not sure how this works
        /// </returns>
        public DTO.CarrTarDiscount GetMostPreciseDiscount(DTO.BookRevenue bookRev,
            DTO.CarrTarDiscount[] discounts,
            List<int> InterlineList,
            double totalWeight,
            bool Outbound)
        {
            DTO.CarrTarDiscount Discount = null;
            bool blnInterline = false;
            if (bookRev == null || discounts == null || discounts.Count() < 1) { return Discount; }

            // Determine what we are looking for, interline or directs.
            if (InterlineList != null && InterlineList.Count > 0)
            {
                // At least one point is interline.
                blnInterline = true;
            }
            string Country;
            string State;
            string City;
            string Zip;
            if (Outbound)
            {
                Country = bookRev.BookDestCountry;
                State = bookRev.BookDestState;
                City = bookRev.BookDestCity;
                Zip = bookRev.BookDestZip;

            }
            else
            {
                Country = bookRev.BookOrigCountry;
                State = bookRev.BookOrigState;
                City = bookRev.BookOrigCity;
                Zip = bookRev.BookOrigZip;
            }

            // Look up discount using last stop's book revenue.
            Discount =
                (from d in discounts
                 where
                 (
                     (d.CarrTarDiscountWgtLimit == 0 || d.CarrTarDiscountWgtLimit >= totalWeight)
                     &&
                     (
                         (blnInterline == true && d.CarrTarDiscountPointTypeControl == (int)DAL.Utilities.PointType.Interline)
                         ||
                         (blnInterline == false && d.CarrTarDiscountPointTypeControl == (int)DAL.Utilities.PointType.Direct)
                         ||
                         d.CarrTarDiscountPointTypeControl == (int)DAL.Utilities.PointType.Any
                     )
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarDiscountCountry) || d.CarrTarDiscountCountry.ToUpper() == Country.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarDiscountState) || d.CarrTarDiscountState.ToUpper() == State.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarDiscountCity) || d.CarrTarDiscountCity.ToUpper() == City.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarDiscountZipFrom) || string.IsNullOrWhiteSpace(Zip) || Zip.Trim().Substring(0, d.CarrTarDiscountZipFrom.Trim().Length).ToUpper().CompareTo(d.CarrTarDiscountZipFrom.Trim().ToUpper()) >= 0) //zip is equal to or follows CarrTarDiscountZipFrom
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarDiscountZipTo) || string.IsNullOrWhiteSpace(Zip) || Zip.Trim().Substring(0, d.CarrTarDiscountZipTo.Trim().Length).ToUpper().CompareTo(d.CarrTarDiscountZipTo.Trim().ToUpper()) <= 0) //zip is equal to or preceeds CarrTarDiscountZipTo
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarDiscountEffDateFrom.HasValue || d.CarrTarDiscountEffDateFrom.Value.Date <= bookRev.BookDateLoad.Value.Date))
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarDiscountEffDateTo.HasValue || d.CarrTarDiscountEffDateTo.Value.Date >= bookRev.BookDateLoad.Value.Date))
                 )
                 orderby
                 d.CarrTarDiscountCity descending,
                 d.CarrTarDiscountState descending,
                 d.CarrTarDiscountCountry descending,
                 d.CarrTarDiscountZipFrom descending,
                 d.CarrTarDiscountZipTo ascending,
                 d.CarrTarDiscountWgtLimit ascending,
                 d.CarrTarDiscountRateValue descending
                 select d).FirstOrDefault();


            // Just return if there is an applicable record.
            return Discount;
        }

        /// <summary>
        /// Get discount records. Calls NGL library and requires further work to get the most precise record.
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number</param>
        /// <returns></returns>
        public DTO.CarrTarDiscount[] getCarrTarDiscountsFiltered(int carrTarControl, ref List<string> sFaultInfo)
        {
            DTO.CarrTarDiscount[] discounts = null;      // nothing found.
            //check for a stored reference for this carrTarControl
            if (this.DictTarDiscount.ContainsKey(carrTarControl)) { return this.DictTarDiscount[carrTarControl]; }
            try
            {
                discounts = DiscountInstance.GetCarrTarDiscountsFiltered(carrTarControl);
                this.DictTarDiscount.Add(carrTarControl, discounts);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrTarDiscountsFiltered"), carrTarControl.ToString());
                }
                else
                {
                    this.DictTarDiscount.Add(carrTarControl, null);
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return discounts;
        }

        /// <summary>
        /// Get the FAK (Class Reference) records using the NGL library. Later it gets the most precise information from these results.
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number</param>
        /// <returns></returns>
        public DTO.CarrTarClassXref[] getCarrTarClassXRefsFiltered(int carrTarControl)
        {
            DTO.CarrTarClassXref[] xrefs = null;      // nothing found.

            try
            {
                xrefs = ClassXrefInstance.GetCarrTarClassXrefsFiltered(carrTarControl);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrClassXrefsFiltered"), carrTarControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return xrefs;
        }

        /// <summary>
        /// Get the minimum charge records from the NGL library (later filtered for precision).
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number</param>
        /// <returns></returns>
        public DTO.CarrTarMinCharge[] getCarrTarMinChargesFiltered(int carrTarControl)
        {
            DTO.CarrTarMinCharge[] minCharges = null;      // nothing found.

            try
            {
                minCharges = MinChargeInstance.GetCarrTarMinChargesFiltered(carrTarControl);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrMinChargesFiltered"), carrTarControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return minCharges;
        }

        public DTO.CarrTarMinWeight[] getCarrTarMinWeightsFiltered(int carrTarControl)
        {
            DTO.CarrTarMinWeight[] minWeights = null;      // nothing found.

            try
            {
                minWeights = MinWeightInstance.GetCarrTarMinWeightsFiltered(carrTarControl);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrMinWeightsFiltered"), carrTarControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return minWeights;
        }


        /// <summary>
        /// Deprecated: do not use this method  we now query the db directly via isBookRevInterline method
        /// Previously used to Get the interline records for tariff pivot using NGL library. Requires subsequent filter to get most precise record.
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number.</param>
        /// <returns></returns>
        /// <remarks>
        ///  Modified by RHR v-7.0.5.100 6/2/2016  
        ///  we now query the db directly using isBookRevInterline instead of storing the non-service information in memory
        ///   there are too many records and the system performance would be affected.
        /// </remarks>
        public DTO.CarrTarInterline getCarrTarInterlineFiltered(int carrTarControl)
        {
            DTO.CarrTarInterline interline = null;      // nothing found.

            try
            {
                interline = InterlineInstance.GetCarrTarInterlineFiltered(carrTarControl);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrInterlineFiltered"), carrTarControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return interline;
        }

        /// <summary>
        /// Deprecated: do not use this method  we now query the db directly via IsInterline method
        /// Previously used to Read the interline status for the tariff using the NGL library. Requires subsequent filtering to obtain the most precise record.
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number</param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.100 6/2/2016  
        ///   we now query the db directly instead of storing the interline information in memory
        ///   there are too many records and the system performance would be affected.
        /// </remarks>
        public DTO.CarrTarInterline[] getCarrTarInterlinesFiltered(int carrTarControl, ref List<string> sFaultInfo)
        {
            DTO.CarrTarInterline[] interlines = null;      // nothing found.
            //check for a stored reference for this carrTarControl
            if (this.DictTarInterlineList.ContainsKey(carrTarControl)) { return this.DictTarInterlineList[carrTarControl]; }
            //read the data from the database
            try
            {
                interlines = InterlineInstance.GetCarrTarInterlinesFiltered(carrTarControl);
                this.DictTarInterlineList.Add(carrTarControl, interlines);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrTarInterlinesFiltered"), carrTarControl.ToString());
                }
                else
                {
                    this.DictTarInterlineList.Add(carrTarControl, null);
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return interlines;
        }

        /// <summary>
        /// Deprecated: do not use this method  we now query the db directly via IsNonServicePoint method
        /// Previously used to Gets the non-service points for the tariff pivot record using the NGL library. Required further filtering to obtain the most precise record.
        /// </summary>
        /// <param name="carrTarControl">Carrier Tariff Control number</param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.100 6/2/2016  
        ///   we now query the db directly instead of storing the non-service point information in memory
        ///   there are too many records and the system performance would be affected.
        /// </remarks>
        public DTO.CarrTarNonServ[] getCarrTarNonServsFiltered(int carrTarControl, ref List<string> sFaultInfo)
        {
            DTO.CarrTarNonServ[] nonServs = null;      // nothing found.
                                                       //check for a stored reference for this carrTarControl
            if (DictNonServicePoints.ContainsKey(carrTarControl)) { return DictNonServicePoints[carrTarControl]; }
            //read the data from the database
            try
            {
                nonServs = NonServInstance.GetCarrTarNonServsFiltered(carrTarControl);
                DictNonServicePoints.Add(carrTarControl, nonServs);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrTarNonServsFiltered"), carrTarControl.ToString());
                }
                else
                {
                    DictNonServicePoints.Add(carrTarControl, null);
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return nonServs;
        }

        /// <summary>
        /// Reads the interline status for the tariff using the NGL library. Requires subsequent filtering to obtain the most precise record.
        /// </summary>
        /// <param name="carrTarControl"></param>
        /// <param name="sFaultInfo"></param>
        /// <returns></returns>
        public DTO.CarrTarMinCharge[] getCarrTarMinChargeFiltered(int carrTarControl, ref List<string> sFaultInfo)
        {
            DTO.CarrTarMinCharge[] minCharges = null;      // nothing found.
            //check for a stored reference for this carrTarControl
            if (this.DictTarMinCharge.ContainsKey(carrTarControl)) { return this.DictTarMinCharge[carrTarControl]; }
            //read the data from the database
            try
            {
                minCharges = MinChargeInstance.GetCarrTarMinChargesFiltered(carrTarControl);
                this.DictTarMinCharge.Add(carrTarControl, minCharges);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrTarMinChargeFiltered"), carrTarControl.ToString());
                }
                else
                {
                    this.DictTarMinCharge.Add(carrTarControl, null);
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return minCharges;
        }

        public DTO.CarrTarMinWeight[] getCarrTarMinWeightFiltered(int carrTarControl, ref List<string> sFaultInfo)
        {
            DTO.CarrTarMinWeight[] minWeights = null;      // nothing found.
            //check for a stored reference for this carrTarControl
            if (this.DictTarMinWeight.ContainsKey(carrTarControl)) { return this.DictTarMinWeight[carrTarControl]; }
            //read the data from the database
            try
            {
                minWeights = MinWeightInstance.GetCarrTarMinWeightsFiltered(carrTarControl);
                this.DictTarMinWeight.Add(carrTarControl, minWeights);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrTarMinWeightFiltered"), carrTarControl.ToString());
                }
                else
                {
                    this.DictTarMinWeight.Add(carrTarControl, null);
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return minWeights;
        }


        /// <summary>
        /// Calculate the line haul charge (i.e., the base charge) for each rate type.
        /// </summary>
        /// <param name="ratedLoad">Load being rated (contains BookRevenue collection)</param>
        /// <param name="InterlineList">List mapping book revenue objects to interline status.</param>
        /// <param name="carrierTariffsPivot">Carrier tariff info to rate with.</param>
        /// <returns>Line haul charge (i.e., base charge).
        /// </returns>
        private decimal calcLineHaulCharge(ref DTO.CarrierCostResults results,
            Load ratedLoad,
            List<int> InterlineList,
            DTO.CarrierTariffsPivot[] carrierTariffsPivots,
            ref Dictionary<int, double> DictAlloc,
            ref ClassRatingLineAllocation LineAllocation,
            ref List<DTO.CarrierTariffsPivot> ratedPivots)

        {

            Logger.Information("RateCarrierTariff.calcLineHaulCharge with current results {@carrierTariffsPivots}, and DictAlloc {@DictAlloc}, and LineAlloc {@LineAllocation}", results, DictAlloc, LineAllocation);

            if (ratedPivots == null) { ratedPivots = new List<DTO.CarrierTariffsPivot>(); }
            decimal lineHaulCharge = -1;
            using (var operation = Logger.StartActivity(
                       "calcLineHaulCharge(Results: {results}, RatedLoad: {RatedLoad}, InterlineList: {InterlineList}, CarrierTariffPivots: {CarrierTariffPivots}, LineAllocation: {LineAllocation}, RatedPivots: {RatedPivots}",
                       results, ratedLoad, InterlineList, carrierTariffsPivots, LineAllocation, ratedPivots))
            {

                ratedLoad.CarrierCubeRate = 0;
                ratedLoad.CarrierMileRate = 0;
                ratedLoad.CarrierLbsRate = 0;
                ratedLoad.CarrierPltRate = 0;
                ratedLoad.CarrierCaseRate = 0;

                switch (carrierTariffsPivots[0].CarrTarEquipMatTarRateTypeControl)
                {
                    case (int)DAL.Utilities.TariffRateType.DistanceM: // Distance based rates (miles)
                        Logger.Information("RateCarrierTariff.calcLineHaulCharge - Check for Allow Distance Rate ({0})",
                            carrierTariffsPivots[0].AllowDistanceRate);
                        if (carrierTariffsPivots[0].AllowDistanceRate)
                        {

                            ratedLoad.CarrierMileRate = (carrierTariffsPivots[0].Val1 == null)
                                ? 0
                                : (decimal)carrierTariffsPivots[0].Val1;
                            Logger.Information("RateCarrierTariff.calcLineHaulCharge - Mile Rate ({0})",
                                ratedLoad.CarrierMileRate);
                            ratedPivots.Add(carrierTariffsPivots[0]);
                            decimal totalMiles = (decimal)ratedLoad.TotalMiles;
                            //PFM Code Change 2/4/2015 - New Requirements to Round the miles 
                            //to nearest integer before we calculate
                            decimal roundedMiles = Math.Round(totalMiles);
                            lineHaulCharge = Decimal.Round((roundedMiles * ratedLoad.CarrierMileRate), 2);
                            Logger.Information(
                                "RateCarrierTariff.calcLineHaulCharge - Rounded Miles: {1}, Line Haul Charge ({0}) * CarrierMileRate ({2})",
                                lineHaulCharge, roundedMiles, ratedLoad.CarrierMileRate);
                            if (lineHaulCharge > 0)
                            {
                                Logger.Information("RateCarrierTariff.calcLineHaulCharge - Distance Rate Found");
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_DistanceRateFound,
                                    new List<string> { carrierTariffsPivots[0].CarrTarID });
                            }
                            else
                            {
                                lineHaulCharge = 0.1M;
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - Invalid distance rate or miles; using minimum cost .1M.");
                                results.AddLog("Invalid distance rate or miles; using minimum cost.");
                            }
                        }
                        else
                        {
                            if (!DistRateRstrMsgAdded)
                            {
                                Logger.Information("RateCarrierTariff.calcLineHaulCharge - Distance Rate Restricted");
                                DistRateRstrMsgAdded = true;
                                results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_DistanceRateRestricted);
                            }
                            else
                            {
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - Distance Rate Restricted - For some weird reason this else is the same as the if");
                                //just add it to the log
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_DistanceRateRestricted, null);
                            }
                        }

                        break;
                    case (int)DAL.Utilities.TariffRateType.DistanceK: // Distance based rates (kilometers)
                        break;
                    case (int)DAL.Utilities.TariffRateType.ClassRate: // Class and weight break based rates
                        //if (ratedLoad.CanGoLTL && carrierTariffsPivots[0].AllowLTLRate) // Rate only if is not a multi-pick or multi-stop load.
                        //Modified by RHR for v-8.2.0.117 on 07/18/2019 
                        //  Includes logic to check for Multi-Pick LTL Settings by comp And carrier
                        //  Note: for this to work the carrierTariffsPivots object's AllowLTLRate property may need to be modified in SQL queries
                        Logger.Information("RateCarrierTariff.calcLineHaulCharge - Check for Allow LTL Rate ({0})",
                            carrierTariffsPivots[0].AllowLTLRate);
                        if (ratedLoad.CanLoadGoLTL(ratedLoad.RatedBookRevenue.BookControl,
                                carrierTariffsPivots[0]
                                    .CarrierControl)) // Rate only if is not a multi-pick or multi-stop load.
                        {
                            Logger.Information("RateCarrierTariff.calcLineHaulCharge - LTL Rate Allowed");
                            results.AddLog("Looking up the min weights for the tariff");

                            Logger.Information("RateCarrierTariff.calcLineHaulCharge - Get Min Weights for tariff");
                            DTO.CarrTarMinWeight[] minWeights =
                                MinWeightInstance.GetCarrTarMinWeightsFiltered(
                                    carrierTariffsPivots[0].CarrTarControl);
                            if (minWeights != null)
                            {
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - Found {0} min weights from tariff",
                                    minWeights.Length);
                                results.AddLog("Found " + minWeights.Length +
                                               " min weights from tariff");
                            }
                            else
                            {
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - Get Min Weights for tariff returned null");
                                results.AddLog("Get Min Weights for tariff returned null.");
                            }

                            //Add code to read the array of minWeight objects.
                            //We should add one new field to the database in the book table which
                            //hold the minimum weight adjustment value.  Similar to rated weight.
                            //pass minWeight array to calcClassRatingCharge
                            ratedPivots.Add(
                                carrierTariffsPivots
                                    [0]); // Note:  this should be added inside the calcClassRatingCharge method

                            ClassRating cr = new ClassRating(Parameters);

                            Logger.Information("RateCarrierTariff.calcLineHaulCharge - Calculate Class Rating Charge");
                            lineHaulCharge = cr.calcClassRatingCharge(ref results,
                                ratedLoad,
                                InterlineList,
                                carrierTariffsPivots,
                                ref DictAlloc,
                                ref LineAllocation,
                                ref ratedPivots,
                                minWeights);
                            Logger.Information(
                                "RateCarrierTariff.calcLineHaulCharge.calcClassRatingCharge - Line Haul Charge: {0}",
                                lineHaulCharge);
                            // Set the CarrierLbsRate to the average LTL rate.
                            //Old Code removed by RHR 10/9/13 'ratedLoad.CarrierLbsRate = lineHaulCharge / (decimal)ratedLoad.TotalWgt;
                            //Modified to correct divide by zero error if TotalWgt is zero
                            if ((ratedLoad.TotalWgt > 0))
                            {
                                ratedLoad.CarrierLbsRate = lineHaulCharge / (decimal)ratedLoad.TotalWgt;
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - Rated Load Total Weight: {0}, Carrier Lbs Rate: {1}",
                                    ratedLoad.TotalWgt, ratedLoad.CarrierLbsRate);
                            }
                            else
                            {
                                ratedLoad.CarrierLbsRate = 0;
                            }

                            if (lineHaulCharge > 0)
                            {
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - LTL Rate Found with carTarId: {0}",
                                    carrierTariffsPivots[0].CarrTarID);
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_LTLRateFound,
                                    new List<string> { carrierTariffsPivots[0].CarrTarID });
                            }
                            else
                            {
                                Logger.Warning(
                                    "RateCarrierTariff.calcLineHaulCharge - Invalid class, LTL rate or load weight; cost is zero");
                                results.AddLog("Invalid class, LTL rate or load weight; cost is zero.");
                            }
                        }
                        else
                        {
                            if (!LTLRateRstrMsgAdded)
                            {
                                LTLRateRstrMsgAdded = true;
                                results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_LTLRateRestricted);
                                Logger.Information("RateCarrierTariff.calcLineHaulCharge - LTL Rate Restricted");
                            }
                            else
                            {
                                //just add it to the log
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - LTL Rate Restricted - For some weird reason this else is the same as the if");
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_LTLRateRestricted, null);
                            }
                        }

                        break;
                    case (int)DAL.Utilities.TariffRateType.FlatRate: // Flat rate   
                        Logger.Information("RateCarrierTariff.calcLineHaulCharge - Check for Allow Flat Rate ({0})",
                            carrierTariffsPivots[0].AllowFlatRate);
                        if (carrierTariffsPivots[0].AllowFlatRate)
                        {
                            ratedPivots.Add(carrierTariffsPivots[0]);
                            lineHaulCharge =
                                Decimal.Round(
                                    (carrierTariffsPivots[0].Val1 == null) ? 0 : (decimal)carrierTariffsPivots[0].Val1,
                                    2);
                            //ratedPivots.Add(carrierTariffsPivots[0]);
                            if (lineHaulCharge > 0)
                            {
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - Flat Rate Found with carTarId: {0}",
                                    carrierTariffsPivots[0].CarrTarID);
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_FlatRateFound,
                                    new List<string> { carrierTariffsPivots[0].CarrTarID });
                            }
                            else
                            {
                                Logger.Warning(
                                    "RateCarrierTariff.calcLineHaulCharge - Invalid Flat rate; cost is zero");
                                results.AddLog("Invalid Flat rate; cost is zero.");
                            }
                        }
                        else
                        {
                            if (!FlatRateRstrMsgAdded)
                            {
                                Logger.Information("RateCarrierTariff.calcLineHaulCharge - Flat Rate Restricted");
                                FlatRateRstrMsgAdded = true;
                                results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_FlatRateRestricted);
                            }
                            else
                            {
                                //just add it to the log
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_FlatRateRestricted, null);
                            }
                        }

                        break;
                    case (int)DAL.Utilities.TariffRateType.UnitOfMeasure: // Unit of measure rates
                        //calcClassUnitOfMeasureRatingCharge
                        Logger.Information("RateCarrierTariff.calcLineHaulCharge - Check for Allow UOM Rate ({0})",
                            carrierTariffsPivots[0].AllowUOMRate);
                        if (ratedLoad.CanGoLTL &&
                            carrierTariffsPivots[0]
                                .AllowUOMRate) // Rate only if is not a multi-pick or multi-stop load.
                        {
                            ratedPivots.Add(
                                carrierTariffsPivots
                                    [0]); // Note:  this shold be added inside the calcClassUnitOfMeasureRatingCharge method
                            ClassRating cr = new ClassRating(Parameters);

                            lineHaulCharge = cr.calcClassUnitOfMeasureRatingCharge(ratedLoad, InterlineList,
                                carrierTariffsPivots, ref LineAllocation);

                            Logger.Information(
                                "RateCarrierTariff.calcLineHaulCharge - UOM Rate Found with carTarId: {0}",
                                carrierTariffsPivots[0].CarrTarID);

                            switch (carrierTariffsPivots[0].BPPivot.CarrTarMatBPTarBracketTypeControl)
                            {
                                case (int)DAL.Utilities.BracketType.Pallets:
                                    {
                                        Logger.Information("RateCarrierTariff.calcLineHaulCharge - Bracket Type: Pallets");
                                        if ((ratedLoad.TotalPL > 0))
                                        {

                                            ratedLoad.CarrierPltRate = lineHaulCharge / (decimal)ratedLoad.TotalPL;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierPltRate to lineHaulCharge / TotalPL which equals {0}/{1}={2}",
                                                lineHaulCharge, ratedLoad.TotalPL, ratedLoad.CarrierPltRate);
                                        }
                                        else
                                        {
                                            ratedLoad.CarrierPltRate = 0;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierPltRate to 0");
                                        }

                                        break;
                                    }
                                case (int)DAL.Utilities.BracketType.FlatPallet:
                                    {
                                        //Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
                                        Logger.Information(
                                            "RateCarrierTariff.calcLineHaulCharge - Bracket Type: FlatPallet");
                                        if ((ratedLoad.TotalPL > 0))
                                        {
                                            ratedLoad.CarrierPltRate = lineHaulCharge / (decimal)ratedLoad.TotalPL;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierPltRate to lineHaulCharge / TotalPL which equals {0}/{1}={2}",
                                                lineHaulCharge, ratedLoad.TotalPL, ratedLoad.CarrierPltRate);
                                        }
                                        else
                                        {
                                            ratedLoad.CarrierPltRate = 0;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierPltRate to 0");
                                        }

                                        break;
                                    }
                                case (int)DAL.Utilities.BracketType.Quantity:
                                    {
                                        if ((ratedLoad.TotalCases > 0))
                                        {
                                            ratedLoad.CarrierCaseRate = lineHaulCharge / (decimal)ratedLoad.TotalCases;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierCaseRate to lineHaulCharge / TotalCases which equals {0}/{1}={2}",
                                                lineHaulCharge, ratedLoad.TotalCases, ratedLoad.CarrierCaseRate);
                                        }
                                        else
                                        {
                                            ratedLoad.CarrierCaseRate = 0;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierCaseRate to 0");
                                        }

                                        break;
                                    }
                                case (int)DAL.Utilities.BracketType.Volume:
                                    {
                                        if ((ratedLoad.TotalCube > 0))
                                        {
                                            ratedLoad.CarrierCubeRate = lineHaulCharge / (decimal)ratedLoad.TotalCube;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierCubeRate to lineHaulCharge / TotalCube which equals {0}/{1}={2}",
                                                lineHaulCharge, ratedLoad.TotalCube, ratedLoad.CarrierCubeRate);
                                        }
                                        else
                                        {
                                            ratedLoad.CarrierCubeRate = 0;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierCubeRate to 0");
                                        }

                                        break;
                                    }
                                case (int)DAL.Utilities.BracketType.Cwt:
                                    {
                                        if ((ratedLoad.TotalWgt > 0))
                                        {
                                            ratedLoad.CarrierLbsRate = lineHaulCharge / (decimal)ratedLoad.TotalWgt;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierLbsRate to lineHaulCharge / TotalWgt which equals {0}/{1}={2}",
                                                lineHaulCharge, ratedLoad.TotalWgt, ratedLoad.CarrierLbsRate);
                                        }
                                        else
                                        {
                                            ratedLoad.CarrierLbsRate = 0;
                                            Logger.Information(
                                                "RateCarrierTariff.calcLineHaulCharge - Setting CarrierLbsRate to 0");
                                        }

                                        break;

                                    }
                                default:
                                    {
                                        ratedLoad.CarrierPltRate = 0;
                                        ratedLoad.CarrierCaseRate = 0;
                                        ratedLoad.CarrierCubeRate = 0;
                                        ratedLoad.CarrierLbsRate = 0;
                                        Logger.Information(
                                            "RateCarrierTariff.calcLineHaulCharge - Setting CarrierPltRate, CarrierCaseRate, CarrierCubeRate, and CarrierLbsRate to 0");
                                        break;
                                    }
                            }

                            if (lineHaulCharge > 0)
                            {
                                Logger.Information(
                                    "RateCarrierTariff.calcLineHaulCharge - UOM Rate Found with carTarId: {0}",
                                    carrierTariffsPivots[0].CarrTarID);
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_UOMRateFound,
                                    new List<string> { carrierTariffsPivots[0].CarrTarID });
                            }
                            else
                            {
                                Logger.Warning(
                                    "RateCarrierTariff.calcLineHaulCharge - Invalid unit of measure rate or unit amount check quantity, weight, pallets or volume values; cost is zero.");
                                results.AddLog(
                                    "Invalid unit of measure rate or unit amount check quantity, weight, pallets or volume values; cost is zero.");
                            }
                        }
                        else
                        {
                            if (!UOMRateRstrMsgAdded)
                            {
                                Logger.Information("RateCarrierTariff.calcLineHaulCharge - UOM Rate Restricted");
                                UOMRateRstrMsgAdded = true;
                                results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_UOMRateRestricted);
                            }
                            else
                            {
                                //just add it to the log
                                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_UOMRateRestricted, null);
                            }
                        }

                        break;
                    case (int)DAL.Utilities.TariffRateType.CzarLite
                        : // CzarLite rates (class and weight break based rate).
                        results.AddLog("CzarLite tariff is not configured; cost is zero.");
                        break;
                }
            }

            return lineHaulCharge;
        }

        /// <summary>
        /// Update the book revenue object costs.
        /// </summary>
        /// <param name="carrierTariffsPivot">Carrier tariff pivot record. This parameter may be null when we are
        /// recalculating using the existing Line Haul cost.</param>
        /// <param name="load">Current load we are rating.</param>
        /// <returns>Total cost</returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.102 8/9/2016
        ///     Added logic to calculate the tax fees after all other costs are updated.  
        /// Modified by RHR v-7.0.5.103 2/15/2017
        ///    Fixed bug where taxes were being added to the Non-taxable cost for an order (HST etc.)   
        /// </remarks>
        private decimal UpdateBookRevenues(DTO.CarrierTariffsPivot carrierTariffsPivot, Load load, ref DTO.CarrierCostResults results, ref DTO.CarriersByCost carriersByCost, bool autocalculateBFC = true)
        {
            decimal totalCost = 0;
            using (var operatio = Logger.StartActivity(
                       "UpdateBookRevenues(CarrierTariffsPivot: {@carrierTariffsPivot}, Load: {@load}, Results: {@results}, CarriersByCost: {@carriersByCost}, AutoCalculateBFC: {autocalculateBFC}",
                       carrierTariffsPivot, load, results, carriersByCost, autocalculateBFC))
            {

                //Modified by RHR v-7.0.5.102 8/9/2016
                // We must update all of the BookRevNetCost values before we can calculate taxes
                foreach (DTO.BookRevenue br in load.BookRevenues)
                {
                    using (Logger.StartActivity("Processing BookRev: {@BookRevenue}", br))
                    {
                        br.BookRevCarrierCost = br.BookRevLineHaul - br.BookRevDiscount;
                        br.BookRevOtherCost = br.BookFees.Where(x => x.BookFeesTaxable == true).Sum(x => x.BookFeesValue);
                        br.BookRevNetCost = br.BookRevCarrierCost + br.BookRevOtherCost;
                        Logger.Information(
                            "UpdateBookRevenues - BookRevNetCost: {BookRevNetCost}, BookRevCarrierCost: {BookRevCarrierCost}, BookRevOtherCost: {BookRevOtherCost}",
                            br.BookRevNetCost, br.BookRevCarrierCost, br.BookRevOtherCost);
                    }
                }

                foreach (DTO.BookRevenue br in load.BookRevenues)
                {
                    br.BookCarrTarInterlinePoint = load.InterlinePoint;

                    CarrierFeesInstance.calculateAndAllocateTaxFees(
                        load.BookRevenues[0].BookCarrTarControl,
                        ref load,
                        ref results,
                        ref carriersByCost);

                    Logger.Information(
                        "UpdateBookRevenues - BookRevFreightTax: {BookRevFreightTax}, BookRevNonTaxable: {BookRevNonTaxable}, BookRevTotalCost: {BookRevTotalCost}",
                        br.BookRevFreightTax,
                        br.BookRevNonTaxable,
                        br.BookRevTotalCost);




                    br.BookRevFreightTax = br.BookFees.Where(x => x.BookFeesIsTax == true).Sum(x => x.BookFeesValue);

                    Logger.Information("UpdateBookRevenues - BookRevFreightTax: {0}", br.BookRevFreightTax);

                    br.BookRevNonTaxable = br.BookFees
                        .Where(x => x.BookFeesTaxable == false && x.BookFeesIsTax == false).Sum(x => x.BookFeesValue);

                    Logger.Information("UpdateBookRevenues - BookRevNonTaxable: {0}", br.BookRevNonTaxable);

                    br.BookRevTotalCost = br.BookRevNetCost + br.BookRevFreightTax + br.BookRevNonTaxable;

                    Logger.Information("UpdateBookRevenues - BookRevTotalCost: {0}", br.BookRevTotalCost);
                    /************************************************************************************
                     * Modified by RHR 10/14/13  the old method GetBFCWithTotalCost is no longer valid
                     * We now must call CalculateBFC which does not depend on saving the book record first
                     * Old Code: br.BookTotalBFC = NGLBookData.GetBFCWithTotalCost(br.BookControl, br.BookRevTotalCost);
                     * **********************************************************************************/
                    //New Code:


                    Logger.Information("Check if BookControl ({BookControl}) > 0 && AutocalculateBFC ({AutoCalculateBFC}) == true && BookLockBFCCost ({BookLockBFCCost}) == false ", br.BookControl, autocalculateBFC, br.BookLockBFCCost);

                    if (br.BookControl > 0 &&
                        autocalculateBFC &&
                        br.BookLockBFCCost ==
                        false) //PFM code change, added parameter autocalculate.  In the spotrate scencario, we may calculate manually.
                    {
                        Logger.Information(
                            "UpdateBookRevenues - Calculate BFC with br.BookMilesFrom: {0}, br.BookTotalWgt: {1}, br.BookTotalPL: {2}, br.BookTotalCube: {3}, br.BookTotalCases: {4}, br.BookTotalBFC: {5}, br.BookCustCompControl: {6}, br.BookODControl: {7}, br.BookRevTotalCost: {8}, br.BookRevOtherCost: {9}",
                            br.BookMilesFrom, br.BookTotalWgt, br.BookTotalPL, br.BookTotalCube, br.BookTotalCases,
                            br.BookTotalBFC, br.BookCustCompControl, br.BookODControl, br.BookRevTotalCost,
                            br.BookRevOtherCost);



                        br.BookTotalBFC = NGLBookData.CalculateBFC(
                            (br.BookMilesFrom ?? 0),
                            br.BookTotalWgt,
                            br.BookTotalPL,
                            br.BookTotalCube,
                            br.BookTotalCases,
                            br.BookTotalBFC,
                            br.BookCustCompControl,
                            br.BookODControl,
                            br.BookRevTotalCost,
                            br.BookRevOtherCost);




                        Logger.Information("UpdateBookRevenues - Calculate BFC returned: {BookTotalBFC}", br.BookTotalBFC);
                    }

                    Logger.Information("Check if BookLockBFCCost ({BookLockBFCCost}) == false", br.BookLockBFCCost);

                    if (br.BookLockBFCCost == false) //code change PFM 8/26/2015 BFC was getting reset even when locked.
                    {
                        Logger.Information("BFC Cost is not locked.  setting BookRevBilledBFC ({BookRevBilledBFC}) =  BookRevTotalCost: {BookRevTotalCost}",
                            br.BookRevBilledBFC,
                            br.BookRevTotalCost);

                        br.BookRevBilledBFC = br.BookTotalBFC;
                    }

                    if (br.BookControl > 0)
                    {
                        Logger.Information(
                            "Book Control > 0 - GetServiceFee with br.BookControl: {BookControl}, br.BookFinAPPayAmt: {BookFinAPPayAmt}, br.BookFinAPActCost: {BookFinAPActCost}, br.BookRevTotalCost: {BookRevTotalCost}",
                            br.BookControl,
                            br.BookFinAPPayAmt,
                            br.BookFinAPActCost,
                            br.BookRevTotalCost);

                        br.BookFinServiceFee = NGLBookData.GetServiceFee(br.BookControl, br.BookFinAPPayAmt,
                            br.BookFinAPActCost, br.BookRevTotalCost);

                    }

                    br.BookRevLoadSavings = br.BookRevBilledBFC - br.BookRevTotalCost - br.BookFinServiceFee;

                    Logger.Information(
                        "UpdateBookRevenues - BookRevLoadSavings which equals BookRevBilledBFC ({BookRevBilledBFC}) - BookRevTotalCost ({BookRevTotalCost}) - BookFinServiceFee ({BookFinServiceFee}) = {BookRevLoadSavings}",
                        br.BookRevBilledBFC,
                        br.BookRevTotalCost,
                         br.BookFinServiceFee,
                        br.BookRevLoadSavings);

                    br.BookRevCommCost = (br.BookRevCommPercent == 0)
                        ? 0
                        : Decimal.Round(((decimal)br.BookRevCommPercent * br.BookRevLoadSavings) / (decimal)100, 2);

                    Logger.Information(
                        "UpdateBookRevenues - BookRevCommCost which equals BookRevCommPercent ({BookRevCommPercent}) * BookRevLoadSavings ({BookRevLoadSavings}) / 100 = {BookRevCommCost}",
                        br.BookRevCommPercent,
                        br.BookRevLoadSavings,
                        br.BookRevCommCost);

                    br.BookRevGrossRevenue = br.BookRevLoadSavings - br.BookRevCommCost;

                    Logger.Information(
                        "UpdateBookRevenues - BookRevGrossRevenue which equals BookRevLoadSavings ({BookRevLoadSavings}) - BookRevCommCost ({BookRevCommCost}) = {BookRevGrossRevenue}",
                         br.BookRevLoadSavings,
                         br.BookRevCommCost,
                         br.BookRevGrossRevenue);

                    br.BookFinAPStdCost = br.BookRevTotalCost;

                    Logger.Information(
                        "UpdateBookRevenues - BookFinAPStdCost which equals BookRevTotalCost ({BookRevTotalCost}) = {BookFinAPStdCost}",
                         br.BookRevTotalCost,
                         br.BookFinAPStdCost);

                    br.BookFinARBookFrt = br.BookRevBilledBFC;
                    br.BookFinCommStd = br.BookRevCommCost;

                    // If the carrierTariffsPivot is null, then we must be recalculating using the existing Line Haul cost.
                    // In this case, we've already rated with the selected carrier and the carrier tariff data has already 
                    // been updated to the BookRevenue object, so we don't need to do it here.
                    if (carrierTariffsPivot != null)
                    {
                        Logger.Information(
                            "carrierTariffsPivot is not null, so we are updating the BookRevenue object with the carrierTariffsPivot data");
                        br.BookCarrTarControl = carrierTariffsPivot.CarrTarControl;
                        br.BookCarrTarRevisionNumber = carrierTariffsPivot.CarrTarRevisionNumber;
                        br.BookCarrTarName = carrierTariffsPivot.CarrTarName;
                        br.BookCarrTarEquipControl = carrierTariffsPivot.CarrTarEquipControl;
                        br.BookCarrTarEquipName = carrierTariffsPivot.CarrTarEquipName;
                        br.BookCarrTarEquipMatControl = carrierTariffsPivot.CarrTarEquipMatControl;
                        br.BookCarrTarEquipMatName = carrierTariffsPivot.CarrTarEquipMatName;
                        br.BookModeTypeControl = carrierTariffsPivot.CarrTarTariffModeTypeControl;
                        //br.BookCarrTarEquipMatDetControl = carrierTariffsPivot.CarrTarEquipMatDetControl;
                        // TODO how do we set the followint two fields for LTL?
                        br.BookCarrTarEquipMatDetID = 1;
                        br.BookCarrTarEquipMatDetValue = carrierTariffsPivot.Val1;
                        br.BookRevLoadMiles = load.TotalMiles;
                        br.BookBilledLoadWeight = load.TotalWgt;
                        br.BookTotalPL = load.TotalPL;
                        br.BookTotalCube = load.TotalCube;
                        br.BookTotalCases = load.TotalCases;
                        br.BookTotalPX = load.TotalPX;
                        br.BookCarrierControl = carrierTariffsPivot.CarrierControl;
                    }
                    // TODO need to call new procedure to populate the following fields.
                    //br.BookCarrierContact
                    //br.BookCarrierContactPhone
                    //br.BookCarrierContControl


                    Logger.Information("UpdateBookRevenues - set totalCost ({totalCost}) += br.BookRevTotalCost: {BookRevTotalCost}",
                        totalCost, br.BookRevTotalCost);
                    totalCost += br.BookRevTotalCost;
                }
            }

            Logger.Information("UpdateBookRevenues - totalCost: {totalCost}", totalCost);
            return totalCost;
        }

        #endregion
    }
}
