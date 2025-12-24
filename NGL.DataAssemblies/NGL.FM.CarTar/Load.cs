using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Text.Json.Serialization;
using Destructurama.Attributed;
using Serilog;
using Serilog.Core;
using SerilogTracing;

namespace NGL.FM.CarTar
{
    /// <summary>
    /// Load Class.
    /// This class manages the BookRevenue collection that represents a load.
    /// </summary>
    /// <remarks>
    /// Modified by RHR v-7.0.5.100 4/20/2016 
    ///     Added InterlinePoint property
    /// </remarks>
    public class Load : TarBaseClass
    {
        #region " Constructors "

        /// <summary>
        /// Constructor
        /// The constructor actually goes out and gets the BookRevenue collection from data services.
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public Load(int bookControl, DAL.WCFParameters oParameters)
            : base()
        {
            this.Logger = Logger.ForContext<Load>();

            if (oParameters == null)
            {
                //TODO: Remove the test code populateSampleParameterData
                populateSampleParameterData();  //this will not really be used in production
            }
            else
            {
                Parameters = oParameters;
            }

            // Get the list of BookRevenue objects (i.e., each represents a stop on the load).
            Logger.Information("Populating Book Revenues with BookControl: {BookControl}", bookControl);
            _bookRevenues = getBookRevenuesWDetailsFiltered(bookControl);

        }

        /// <summary>
        /// Overloaded Constructor to allow in memory calculations
        /// </summary>
        /// <param name="bookControl"></param>
        /// <param name="oParameters"></param>
        public Load(DTO.BookRevenue[] inBookRevs, DAL.WCFParameters oParameters)
            : base()
        {
            this.Logger = Logger.ForContext<Load>();

            if (oParameters == null)
            {
                
                populateSampleParameterData();  //this will not really be used in production
            }
            else
            {
                Parameters = oParameters;
            }

            // Get the list of BookRevenue objects (i.e., each represents a stop on the load).
            _bookRevenues = inBookRevs; //getBookRevenuesWDetailsFiltered(bookControl);

        }

        public Load()
            : base()
        {
            Logger = Logger.ForContext<Load>();

        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.Load";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        /// <summary>
        /// This is the BookRevenue collection that represents a load.
        /// </summary>
        private DTO.BookRevenue[] _bookRevenues = null;
        public DTO.BookRevenue[] BookRevenues
        {
            get { return _bookRevenues; }
            set { _bookRevenues = value; }
        }

        private DTO.BookRevenue _RatedBookRevenue = null;
        public DTO.BookRevenue RatedBookRevenue
        {
            get
            {
                if (_RatedBookRevenue == null)
                {
                    return LastStopBookRevenue;
                }
                else
                {
                    return _RatedBookRevenue;
                }
            }

            set { _RatedBookRevenue = value; }
        }

        //Modified by RHR 10/30/14 added class data for rate shoping to load object
        public string RateShopMatClass { get; set; }

        public int RateShopMatClassTypeControl
        {
            get
                ; set;
        }



        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool CanGoLTL
        {
            get
            {
                bool blnRet = true;
                if (this.BookRevenues?.Count() > 1)
                {
                    foreach (DTO.BookRevenue b in BookRevenues)
                    {
                        if (b.BookControl != this.RatedBookRevenue.BookControl
                            && b.BookDestAddress1 != this.RatedBookRevenue.BookDestAddress1
                            && b.BookOrigAddress1 != this.RatedBookRevenue.BookOrigAddress1)
                        {
                            //this is a multi-pick or multi-stop load and cannot go LTL
                            blnRet = false;
                            break; //we are done as soon as one address does not match.
                        }
                    }
                }
                return blnRet;
            }
        }

        /// <summary>
        /// Modified by RHr 4/15/14.  bug fixed where changes to the last stop do not 
        /// update the LastStopBookRevenue reference because it is not linked to the bookRevenues list
        /// by removing the local private variable and making the property read only we always get the 
        /// correct reference to the last stop object.
        /// </summary>
        public DTO.BookRevenue LastStopBookRevenue
        {
            get
            {

                if (_bookRevenues != null && _bookRevenues.Length > 0)
                {
                    return _bookRevenues.OrderByDescending(x => x.BookStopNo).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public DTO.BookRevenue FirstPickUpBookRevenue
        {
            get
            {

                if (_bookRevenues != null && _bookRevenues.Length > 0)
                {
                    return _bookRevenues.OrderBy(x => x.BookPickupStopNumber).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// The total miles for the load.
        /// </summary>
        private double _totalMiles = -1;
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public double TotalMiles
        {
            get
            {
                //if (_totalMiles == -1)
                //{
                    try
                    {


                        // Sum the incremental stop distances from the BookMilesFrom field on each BookRevenue object in the load.
                        _totalMiles = (double)_bookRevenues?.Sum(x => x.BookMilesFrom);

                        // Find the largest direct distance (i.e., the largest of the BookRevLaneBenchMiles.
                        DTO.BookRevenue temp = _bookRevenues?.OrderByDescending(x => x.BookRevLaneBenchMiles)?.FirstOrDefault();
                        double? maxLaneBenchMilesq = temp?.BookRevLaneBenchMiles;
                        double maxLaneBenchMiles = (maxLaneBenchMilesq == null) ? 0.0 : (double)maxLaneBenchMilesq;

                        // If we are assigning a carrier and the total incremental miles is less than the largest direct distance,
                        // then use the largest direct distance.
                        DTO.BookRevenue lastStop = LastStopBookRevenue;
                        if ((lastStop == null || lastStop.BookCarrierControl == 0) &&
                            ((_totalMiles == 0) || (_totalMiles < maxLaneBenchMiles)))
                        {
                            _totalMiles = maxLaneBenchMiles;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Error calculating Total Miles...");
                    }
             //   }

                return _totalMiles;
            }
        }

        /// <summary>
        /// The total out of route miles for the load.
        /// </summary>
        private double _totalOutOfRouteMiles = -1;
        public double TotalOutOfRouteMiles
        {
            get
            {
                
                    // Sum the incremental stop distances from the BookOutOfRouteMiles field on each BookRevenue object in the load.
                    _totalOutOfRouteMiles = (double)_bookRevenues.Sum(x => x.BookOutOfRouteMiles);
                
                return _totalOutOfRouteMiles;
            }
        }

        /// <summary>
        /// The total weigt of the load
        /// </summary>
        private double _totalWgt = -1;
        public double TotalWgt
        {
            get
            {
                
                    _totalWgt = (double)_bookRevenues.Sum(x => x.BookTotalWgt);
                
                return _totalWgt;
            }
        }

        /// <summary>
        /// The total cases on the load
        /// </summary>
        private int _totalCases = -1;
        public int TotalCases
        {
            get
            {
                
                    _totalCases = _bookRevenues.Sum(x => x.BookTotalCases);
                
                return _totalCases;
            }
        }

        /// <summary>
        /// The total Cube of the load
        /// </summary>
        private int _totalCube = -1;
        public int TotalCube
        {
            get
            {
               
                    _totalCube = _bookRevenues.Sum(x => x.BookTotalCube);
                
                return _totalCube;
            }
        }

        /// <summary>
        /// The total PL of the load
        /// </summary>
        private double _totalPL = -1;
        public double TotalPL
        {
            get
            {
              
                    _totalPL = (double)_bookRevenues.Sum(x => x.BookTotalPL);
                
               return _totalPL;
            }
        }

        /// <summary>
        /// The total PL of the load
        /// </summary>
        private int _totalPX = -1;
        public int TotalPX
        {
            get
            {
              
                    _totalPX = _bookRevenues.Sum(x => x.BookTotalPX);
                
                return _totalPX;
            }
        }

        //Add two new properties for two new flags NegociatedMinChargeUsed, PublishedMinChargeUsed.

        private decimal _totalLineHaul = 0;
        public decimal TotalLineHaul
        {
            get { return _totalLineHaul; }
            set { _totalLineHaul = value; }
        }

        public decimal sumTotalLineHaul()
        {
            decimal totalLineHaul = 0;

            foreach (DTO.BookRevenue br in BookRevenues)
            {
                totalLineHaul += br.BookRevLineHaul;
            }

            return totalLineHaul;
        }

        public decimal sumTotalBFC()
        {
            decimal totalBFC = 0;

            foreach (DTO.BookRevenue br in BookRevenues)
            {
                totalBFC += br.BookTotalBFC;
            }

            return totalBFC;
        }

        private decimal _totalDiscount = 0;
        public decimal TotalDiscount
        {
            get { return _totalDiscount; }
            set { _totalDiscount = value; }
        }

        private decimal _DiscountRate = 0;
        public decimal DiscountRate
        {
            get { return _DiscountRate; }
            set { _DiscountRate = value; }
        }

        private decimal _DiscountMin = 0;
        public decimal DiscountMin
        {
            get { return _DiscountMin; }
            set { _DiscountMin = value; }
        }

        private decimal _minimumCharge = 0;
        public decimal MinimumCharge
        {
            get { return _minimumCharge; }
            set { _minimumCharge = value; }
        }

        public decimal SumOfEstTotalCost
        {
            get { return _bookRevenues.Sum(x => x.BookRevTotalCost); }
        }

        public decimal TotalCarrierCost
        {
            get { return _bookRevenues.Sum(x => x.BookRevCarrierCost); }
        }

        public decimal TotalAccessorial
        {
            get
            {
                decimal totalAccessorial = 0;
                foreach (DTO.BookRevenue br in BookRevenues)
                {
                    totalAccessorial += (br.BookRevOtherCost + br.BookRevFreightTax + br.BookRevNonTaxable);
                }
                return totalAccessorial;
            }
        }

        private List<DTO.BookFee> _ProfileFees = new List<DTO.BookFee>();
        public List<DTO.BookFee> ProfileFees
        {
            get { return _ProfileFees; }
            set { _ProfileFees = value; }
        }

        private DAL.Utilities.AssignCarrierCalculationType _CalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal;
        public DAL.Utilities.AssignCarrierCalculationType CalculationType
        {
            get { return _CalculationType; }
            set { _CalculationType = value; }
        }

        private decimal _totalStopCharges = 0;
        public decimal TotalStopCharges
        {
            get { return _totalStopCharges; }
            set { _totalStopCharges = value; }
        }

        private decimal _totalPickCharges = 0;
        public decimal TotalPickCharges
        {
            get { return _totalPickCharges; }
            set { _totalPickCharges = value; }
        }

        private decimal _totalFuelCost = 0;
        public decimal TotalFuelCost
        {
            get { return _totalFuelCost; }
            set { _totalFuelCost = value; }
        }

        private decimal _carrierCubeRate = 0;
        public decimal CarrierCubeRate
        {
            get { return _carrierCubeRate; }
            set { _carrierCubeRate = value; }
        }

        private decimal _carrierMileRate = 0;
        public decimal CarrierMileRate
        {
            get { return _carrierMileRate; }
            set { _carrierMileRate = value; }
        }

        private decimal _carrierLbsRate = 0;
        public decimal CarrierLbsRate
        {
            get { return _carrierLbsRate; }
            set { _carrierLbsRate = value; }
        }

        private decimal _carrierPltRate = 0;
        public decimal CarrierPltRate
        {
            get { return _carrierPltRate; }
            set { _carrierPltRate = value; }
        }

        private decimal _carrierCaseRate = 0;
        public decimal CarrierCaseRate
        {
            get { return _carrierCaseRate; }
            set { _carrierCaseRate = value; }
        }
        private ClassRatingLineAllocation _LineAllocation = new ClassRatingLineAllocation();
        public ClassRatingLineAllocation LineAllocation
        {
            get { return _LineAllocation; }
            set { _LineAllocation = value; }
        }

        private int _CarrTarEquipMatMaxDays = 0;
        public int CarrTarEquipMatMaxDays
        {
            get { return _CarrTarEquipMatMaxDays; }
            set { _CarrTarEquipMatMaxDays = value; }
        }

        private bool _CarrTarOutbound = true;
        public bool CarrTarOutbound
        {
            get { return _CarrTarOutbound; }
            set { _CarrTarOutbound = value; }
        }

        private bool _CarrTarWillDriveSunday = true;
        public bool CarrTarWillDriveSunday
        {
            get { return _CarrTarWillDriveSunday; }
            set { _CarrTarWillDriveSunday = value; }
        }

        private bool _CarrTarWillDriveSaturday = true;
        public bool CarrTarWillDriveSaturday
        {
            get { return _CarrTarWillDriveSaturday; }
            set { _CarrTarWillDriveSaturday = value; }
        }

        private bool _InterlinePoint = false;
        /// <summary>
        /// Flag to identify carriers that require an inteline carrier to delivery the load to one or more of the stops on this load
        /// </summary>
        /// <remarks>
        /// Created by RHR v-7.0.5.100 4/20/2016 
        /// </remarks>
        public bool InterlinePoint
        {
            get { return _InterlinePoint; }
            set { _InterlinePoint = value; }
        }

        #endregion

        #region "Public Methods"

        public bool CanLoadGoLTL(int BookControl, int CarrierControl)
        {
            bool bRet = false;

            try
            {
                bRet = NGLBookData.CanLoadGoLTL(BookControl, CarrierControl);
            }
            catch
            {
                //do nothing just return default ( false)
                Logger.Error("Error in NGL.FM.CarTar.Load.CanLoadGoLTL({0},{1})", BookControl, CarrierControl);
            }
            return bRet;
        }

        /// <summary>
        /// Validate the BookRevenue objects
        /// </summary>
        /// <returns>true if valid, else false</returns>
        public bool validLoad(ref DTO.CarrierCostResults results, DAL.Utilities.AssignCarrierCalculationType calculationType)
        {
            using (var operationLog =
                   Logger.StartActivity(
                       "validLoad(CarrierCostResults: {@CarrierCostResults}, AssignCarrierCalculationType: {AssignCarrierCalculationType}",
                       results, calculationType))
            {

                if (results == null)
                    results = new DTO.CarrierCostResults();

                if (_bookRevenues == null)
                {
                    Logger.Warning("No orders found...");
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoOrdersFound);
                    operationLog.Complete();
                    return false;
                }
                else
                {
                    Logger.Information("Processing Calculation Type: {CalculationType}", calculationType);
                    switch (calculationType)
                    {
                        case DAL.Utilities.AssignCarrierCalculationType.RateShopOnly:
                            foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                            {

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookOrigCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigZip))
                                {
                                    Logger.Warning("Origin Country, Zip are not valid on {BookRevenue} ", bookRevenue);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_OrigRateShopCountryZipNotValid, true);
                                    operationLog.Complete();
                                    return false;
                                }


                                if (string.IsNullOrWhiteSpace(bookRevenue.BookDestCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestZip))
                                {

                                    Logger.Warning("Destination Country, Zip are not valid on {BookRevenue} ", bookRevenue);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_DestRateShopCountryZipNotValid, true);

                                    return false;
                                }
                            }

                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.Normal:
                            Logger.Information("Validate Load - Normal Carrier Calculation...");
                            foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                            {
                                Logger.Information(
                                    "Validate Load  - Normal - check if BookRouteFinalCode is NS, SH, or CM ({BookRouteFinalCode})",
                                    bookRevenue.BookRouteFinalCode);
                                if (string.IsNullOrWhiteSpace(bookRevenue.BookRouteFinalCode) ||
                                    bookRevenue.BookRouteFinalCode == "NS" || bookRevenue.BookRouteFinalCode == "SH" ||
                                    bookRevenue.BookRouteFinalCode == "CM")
                                {
                                    // Validate BookTranCode.
                                    Logger.Information(
                                        "Validate Load - Normal - check if bookRevenue.BookTranCode is N, P, or PC ({BookTranCode})",
                                        bookRevenue.BookTranCode);
                                    if ((bookRevenue.BookTranCode != "N") && (bookRevenue.BookTranCode != "P") &&
                                        (bookRevenue.BookTranCode != "PC"))
                                    {
                                        Logger.Warning(
                                            "Validate Load - Normal - Item is finalized and cannot change carrier on Order: {OrderNumber}, PRO Number: {PRONumber}.",
                                            bookRevenue.BookCarrOrderNumber,
                                            bookRevenue.BookProNumber);

                                        results.AddMessage(
                                            DTO.CarrierCostResults.MessageEnum.M_FinalizedCannotChangeCarrier, true,
                                            bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                        return false;
                                    }
                                }
                                else
                                {
                                    Logger.Information(
                                        "Validate Load - Normal - check if bookRevenue.BookTranCode is IC ({BookTranCode})",
                                        bookRevenue.BookTranCode);
                                    // Validate BookTranCode.                                
                                    if (bookRevenue.BookTranCode == "IC")
                                    {
                                        Logger.Warning(
                                            "Item is invoiced and cannot be recalculated for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                            bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                        results.AddMessage(
                                            DTO.CarrierCostResults.MessageEnum.M_InvoicedCannotRecalculate, true,
                                            bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                        return false;
                                    }
                                }

                                Logger.Information(
                                    "Validate Load - Normal - check if bookRevenue.BookLockAllCosts is true ({BookLockAllCosts})",
                                    bookRevenue.BookLockAllCosts);
                                if (bookRevenue.BookLockAllCosts == true)
                                {
                                    Logger.Warning("All Costs are locked for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber,
                                        bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_CostsAreLockedCannotRecalculate, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                Logger.Information(
                                    "Validate Load - Normal - check if Originating Address Information is populated at least one field...");

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookOrigCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigZip))
                                {
                                    Logger.Warning(
                                        "Origin Country, City, State, Zip are not valid for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber,
                                        bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_OrigCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                Logger.Information(
                                    "Validate Load - Normal - check if Destination Address Information is populated at least one field...",
                                    bookRevenue.BookDestCity);
                                if (string.IsNullOrWhiteSpace(bookRevenue.BookDestCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestZip))
                                {
                                    Logger.Information(
                                        "Destination Country, City, State, Zip are not valid for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_DestCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }
                            }

                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.RecalcuateNoBFC:

                            foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                            {
                                Logger.Information("RecalculateNoBFC - Checking if {BookRevenue} has valid origin data ", bookRevenue);

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookOrigCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigZip))
                                {

                                    Logger.Warning("Origin Country, City, State, Zip are not valid for Order Number: {OrderNumber}, PRO Number: { PRONumber}",
                                    bookRevenue.BookCarrOrderNumber,
                                    bookRevenue.BookProNumber);

                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_OrigCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookDestCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestZip))
                                {

                                    Logger.Warning("Destination Country, City, State, Zip are not valid for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber,
                                        bookRevenue.BookProNumber);

                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_DestCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }
                            }

                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.Recalculate:

                            foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                            {
                                Logger.Information("Checking if BookCodeCode is Invoiced...");
                                if (bookRevenue.BookTranCode == "IC")
                                {
                                    Logger.Warning(
                                        "Item is invoiced and cannot be recalculated for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_InvoicedCannotRecalculate,
                                        true, bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                Logger.Information("Checking if All Costs are locked...({AllCostsLocked})", bookRevenue.BookLockAllCosts);
                                if (bookRevenue.BookLockAllCosts == true)
                                {
                                    Logger.Warning("All Costs are locked for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_CostsAreLockedCannotRecalculate, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (bookRevenue.BookCarrierControl == 0)
                                {
                                    Logger.Warning("No Carrier is assigned for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoCarrierCannotRecalculate,
                                        true, bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookOrigCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigZip))
                                {
                                    Logger.Warning(
                                        "Origin Country, City, State, Zip are not valid for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_OrigCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookDestCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestZip))
                                {
                                    Logger.Warning(
                                        "Destination Country, City, State, Zip are not valid for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_DestCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }
                            }

                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.RecalcuateSpotRate:
                            Logger.Information("NGL.FM.CarTar.Load.validLoad() - RecalcuateSpotRate");
                            foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                            {
                                // Validate BookTranCode.
                                if (bookRevenue.BookTranCode == "IC")
                                {
                                    Logger.Warning(
                                        "Item is invoiced and cannot be recalculated for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_InvoicedCannotRecalculate,
                                        true, bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (bookRevenue.BookLockAllCosts == true)
                                {
                                    Logger.Warning("All Costs are locked for Order Number: {OrderNumber}, PRO Number: {PRONumber}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_CostsAreLockedCannotRecalculate, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (bookRevenue.BookCarrierControl == 0)
                                {
                                    Logger.Warning("No Carrier is assigned for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoCarrierCannotRecalculate,
                                        true, bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookOrigCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigZip))
                                {
                                    Logger.Warning(
                                        "Origin Country, City, State, Zip are not valid for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_OrigCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookDestCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestZip))
                                {
                                    Logger.Warning(
                                        "Destination Country, City, State, Zip are not valid for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_DestCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }
                            }

                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.UpdateAssignedCarrier:
                            Logger.Information("NGL.FM.CarTar.Load.validLoad() - UpdateAssignedCarrier");
                            foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                            {
                                if (string.IsNullOrWhiteSpace(bookRevenue.BookOrigCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigZip))
                                {
                                    Logger.Warning(
                                        "Origin Country, City, State, Zip are not valid for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_OrigCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookDestCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestZip))
                                {
                                    Logger.Warning(
                                        "Destination Country, City, State, Zip are not valid for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_DestCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }
                            }

                            break;
                        case DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier:
                            Logger.Information("NGL.FM.CarTar.Load.validLoad() - UpdateCarrier");
                            foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                            {
                                if (string.IsNullOrWhiteSpace(bookRevenue.BookRouteFinalCode) ||
                                    bookRevenue.BookRouteFinalCode == "NS" || bookRevenue.BookRouteFinalCode == "SH" ||
                                    bookRevenue.BookRouteFinalCode == "CM")
                                {
                                    // Validate BookTranCode.
                                    if ((bookRevenue.BookTranCode != "N") && (bookRevenue.BookTranCode != "P") &&
                                        (bookRevenue.BookTranCode != "PC"))
                                    {
                                        Logger.Warning(
                                            "Item is finalized and cannot change carrier for Order Number: {0}, PRO Number: {1}",
                                            bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                        results.AddMessage(
                                            DTO.CarrierCostResults.MessageEnum.M_FinalizedCannotChangeCarrier, true,
                                            bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                        return false;
                                    }
                                }
                                else
                                {
                                    // Validate BookTranCode.                                
                                    if (bookRevenue.BookTranCode == "IC")
                                    {
                                        Logger.Warning(
                                            "Item is invoiced and cannot be recalculated for Order Number: {0}, PRO Number: {1}",
                                            bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                        results.AddMessage(
                                            DTO.CarrierCostResults.MessageEnum.M_InvoicedCannotRecalculate, true,
                                            bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                        return false;
                                    }
                                }

                                if (bookRevenue.BookLockAllCosts == true)
                                {
                                    Logger.Warning("All Costs are locked for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_CostsAreLockedCannotRecalculate, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookOrigCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookOrigZip))
                                {
                                    Logger.Warning(
                                        "Origin Country, City, State, Zip are not valid for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_OrigCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }

                                if (string.IsNullOrWhiteSpace(bookRevenue.BookDestCity) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestState) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestCountry) ||
                                    string.IsNullOrWhiteSpace(bookRevenue.BookDestZip))
                                {
                                    Logger.Warning(
                                        "Destination Country, City, State, Zip are not valid for Order Number: {0}, PRO Number: {1}",
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    results.AddMessage(
                                        DTO.CarrierCostResults.MessageEnum.M_DestCountryCityStZipNotValid, true,
                                        bookRevenue.BookCarrOrderNumber, bookRevenue.BookProNumber);
                                    return false;
                                }
                            }

                            break;
                    }

                    this.CalculationType = calculationType;
                }
            }

            return true;
        }

        public bool validLoad(DAL.Utilities.AssignCarrierCalculationType calculationType)
        {
            DTO.CarrierCostResults results = new DTO.CarrierCostResults();
            return validLoad(ref results, calculationType);

        }
        public bool ValidRateShopLoad(ref DTO.CarrierCostResults results, DTO.RateShop rateShop)
        {
            if (results == null) results = new DTO.CarrierCostResults();
            bool valid = true;
            using (var operationLog =
                   Logger.StartActivity(
                       "ValidRateShopLoad(CarrierCostResults: {@CarrierCostResults}, RateShop: {@RateShop}",
                       results, rateShop))
            {
                if (_bookRevenues == null)
                {
                    Logger.Warning("No orders found...");
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoOrdersFound);
                    valid = false;
                    return valid;
                }

                if (_bookRevenues.Length == 0)
                {
                    Logger.Warning("At least one order is required for Rate Shopping...");
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_AtLeastOneOrderReq);
                    valid = false;
                    return valid;
                }

                if (_bookRevenues[0].BookMilesFrom == -1)
                {
                    Logger.Warning("Distance is required for Rate Shopping...");
                    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_DistanceRequired);
                    valid = false;
                    return valid;
                }

                //if (rateShop.ModeTypeControl == 0)
                //{
                //    results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_ReqFieldMissing);
                //    valid = false;
                //    return valid;
                //}
            }

            return valid;
        }
        /// <summary>
        /// Delete all the Carrier Fees from the load. 
        /// </summary>
        public void deleteCarrierFees()
        {
            Logger.Information("Attempting to Delete Carrier fees...");
            if (_bookRevenues != null)
            {
                foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                {
                    // Select the BookFees that are not tariff fees.
                    Logger.Information("Deleting Tariff BookFees for {OrderNumber}...OriginalBookFeesCount: {OriginalBookFeesCount}", bookRevenue.BookCarrOrderNumber, bookRevenue.BookFees?.Count());
                    bookRevenue.BookFees = bookRevenue.BookFees.Where(x => x.BookFeesAccessorialFeeTypeControl != (int)DAL.Utilities.AccessorialFeeType.Tariff).ToList();
                    
                }
            }
        }

        /// <summary>
        /// Deleter all the Lane Fees from the load.
        /// </summary>
        /// <remarks>
        /// Created by RHR for v-7.0.5.103 on 1/23/2017
        ///   We now replace all lane fees when carrier costs are recalculated
        /// </remarks>
        public void deleteLaneFees()
        {
            if (_bookRevenues != null)
            {
                foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                {
                    Logger.Information("Deleting Lane Fees for {OrderNumber}...", bookRevenue.BookCarrOrderNumber);
                    // Select the BookFees that are not tariff fees.
                    bookRevenue.BookFees = bookRevenue.BookFees.Where(x => x.BookFeesAccessorialFeeTypeControl != (int)DAL.Utilities.AccessorialFeeType.Lane).ToList();
                }
            }
        }

        /// <summary>
        /// Delete all the Non-Carrier Fees from the load. 
        /// </summary>
        public void deleteNonCarrierFees()
        {
            if (_bookRevenues != null)
            {
                foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                {
                    // Just select the BookFees that are tariff (i.e., carrier) fees.
                    bookRevenue.BookFees = bookRevenue.BookFees
                        .Where(x => x.BookFeesAccessorialFeeTypeControl == (int)DAL.Utilities.AccessorialFeeType.Tariff ||
                                    x.BookFeesOverRidden == true)
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Delete fees by parameter
        /// </summary>
        public void deleteFees(DAL.Utilities.AccessorialFeeType feetype)
        {
            if (_bookRevenues != null)
            {

                foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                {
                    //modified by RHR 9/10/14 to use RemoveAll using lambda predicate
                    //modified by RHR 7/9/15 fixed null reference exception where BookFees is Null
                    //  lambda RemoveAll requires a bookfees object
                    if (bookRevenue.BookFees != null)
                    {
                        bookRevenue.BookFees.RemoveAll(x => x.BookFeesAccessorialFeeTypeControl == (int)feetype);
                    }
                    //Removed by RHR 9/10/14 cannot use i to remove because count changes after each RemoveAt
                    //Not all items will be removed
                    //for (int i = 0; i < bookRevenue.BookFees.Count; i++ )
                    //{
                    //    if (bookRevenue.BookFees[i].BookFeesAccessorialFeeTypeControl == (int)feetype)
                    //    {
                    //        bookRevenue.BookFees.RemoveAt(i);
                    //    }
                    //} 
                }
            }
        }

        /// <summary>
        /// disassociateTariff - removes tariff control number from all orders on this load.
        /// This is typically used for the SpotRate calculation
        /// - ResetToNStatus
        /// </summary>
        public void ResetToNStatus()
        {
            if (_bookRevenues != null)
            {
                foreach (DTO.BookRevenue bookRevenue in _bookRevenues)
                {
                    Logger.Information("Resetting BookRevenue ({0}) to N Status...", bookRevenue);
                    bookRevenue.ResetToNStatus();
                }
            }
        }


        /// <summary>
        /// Calculates the linehaul cost for each bookrev object based on the allocation formula.
        /// Updates the fields: BookRevLineHaul,BookRevCarrierCost,BookRevDiscount in the obect only.
        /// </summary>
        /// <param name="userLineHaulCost"></param>
        /// <param name="allocation"></param>
        public void calculateBookRevLineHaul(decimal userLineHaulCost, DTO.tblTarBracketType allocation)
        {
            using (var operation =
                   Logger.StartActivity(
                       "calculateBookRevLineHaul(UserLineHaulCost: {UserLineHaulCost}, Allocation: {@Allocation})",
                       userLineHaulCost, allocation))
            {


                DTO.BookRevenue maxRev = null;
                decimal maxRevlineAlloc = 0;
                foreach (DTO.BookRevenue rev in this.BookRevenues)
                {
                    if (userLineHaulCost == 0)
                    {
                        Logger.Information(
                            "User Line Haul Cost is 0.  Setting BookRevLineHaul, BookRevCarrierCost, BookRevDiscount to 0");
                        rev.BookRevLineHaul = 0;
                        rev.BookRevCarrierCost = 0;
                        rev.BookRevDiscount = 0;
                        continue;
                    }

                    decimal revlinehaulalloc = 0; //reset to zero 
                    switch (allocation.TarBracketTypeControl)
                    {
                        case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Pallets:
                            //decimal(Math.Round(((part == 0.0) ? 1.0 : part) / ((whole == 0.0) ? 1.0 : whole) * Convert.ToDouble((decimal.Compare(cost, 0m) == 0) ? 1m : cost), round));

                            revlinehaulalloc =
                                DTran.AllocateCostByPercentage(rev.BookTotalPL, this.TotalPL, userLineHaulCost);
                            rev.BookRevLineHaul = revlinehaulalloc;
                            rev.BookRevCarrierCost = revlinehaulalloc;
                            rev.BookRevDiscount = 0;

                            Logger.Information("Calculating LineHaul ({LineHaul}) by AllocateCostByPercentage based on Pallets (BookTotal: {BookTotalPL}, TotalPL: {TotalPL}, UserLineHaulCost: {UserLineHaul}",
                                revlinehaulalloc,
                                rev.BookTotalPL,
                                rev.BookTotalPL,
                                userLineHaulCost);
                            break;
                        case (int)Ngl.FreightMaster.Data.Utilities.BracketType.FlatPallet:
                           
                            revlinehaulalloc =
                                DTran.AllocateCostByPercentage(rev.BookTotalPL, this.TotalPL, userLineHaulCost);

                            rev.BookRevLineHaul = revlinehaulalloc;
                            rev.BookRevCarrierCost = revlinehaulalloc;
                            rev.BookRevDiscount = 0;

                            Logger.Information("Calculating LineHaul ({LineHaul}) by AllocateCostByPercentage based on FlatPallets (BookTotal: {BookTotalPL}, TotalPL: {TotalPL}, UserLineHaulCost: {UserLineHaul}",
                                revlinehaulalloc,
                                rev.BookTotalPL,
                                rev.BookTotalPL,
                                userLineHaulCost);

                            break;
                        case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Volume:

                            revlinehaulalloc =
                                DTran.AllocateCostByPercentage(rev.BookTotalCube, this.TotalCube, userLineHaulCost);
                            rev.BookRevLineHaul = revlinehaulalloc;
                            rev.BookRevCarrierCost = revlinehaulalloc;
                            rev.BookRevDiscount = 0;

                            Logger.Information("Calculating LineHaul ({LineHaul}) by AllocateCostByPercentage based on Volume (BookTotal: {BookTotalCube}, TotalCube: {TotalCube}, UserLineHaulCost: {UserLineHaul}",
                                revlinehaulalloc,
                                rev.BookTotalCube,
                                rev.BookTotalCube,
                                userLineHaulCost);

                            break;
                        case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Quantity:
                            
                            revlinehaulalloc = DTran.AllocateCostByPercentage(rev.BookTotalCases, this.TotalCases,
                                userLineHaulCost);
                            rev.BookRevLineHaul = revlinehaulalloc;
                            rev.BookRevCarrierCost = revlinehaulalloc;
                            rev.BookRevDiscount = 0;
                            Logger.Information(
                                "Calculating BookRev Line Haul ({RevLineHaulAlloc}) for Quantity using AllocateCostByPercentage on BookTotalCases: {BookTotalCases}, TotalCases: {TotalCases}, userLineHaulCost:{userLineHaulCost}",
                                revlinehaulalloc,
                                                  rev.BookTotalCases, 
                                                  this.TotalCases, 
                                                  userLineHaulCost);

                            break;
                        case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Lbs:
                         
                            revlinehaulalloc =
                                DTran.AllocateCostByPercentage(rev.BookTotalWgt, this.TotalWgt, userLineHaulCost);
                            rev.BookRevLineHaul = revlinehaulalloc;
                            rev.BookRevCarrierCost = revlinehaulalloc;
                            rev.BookRevDiscount = 0;

                            Logger.Information(
                                "Calculating BookRev Line Haul ({BookRevLineHaul}) for Lbs using AllocateCostByPercentage on BookTotalWgt: {BookTotalWgt}, TotalWgt: {TotalWgt}, userLineHaulCost:{userLineHaulCost}",
                                revlinehaulalloc,
                                rev.BookTotalWgt, 
                                this.TotalWgt,
                                userLineHaulCost);

                            break;
                        case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Cwt:
                                                    
                            revlinehaulalloc =
                                DTran.AllocateCostByPercentage(rev.BookTotalWgt, this.TotalWgt, userLineHaulCost);
                            rev.BookRevLineHaul = revlinehaulalloc;
                            rev.BookRevCarrierCost = revlinehaulalloc;
                            rev.BookRevDiscount = 0;

                            Logger.Information(
                                "Calculating BookRev Line Haul ({BookRevLineHaul}) for Cwt using AllocateCostByPercentage on BookTotalWgt: {BookTotalWgt}, TotalWgt: {TotalWgt}, userLineHaulCost:{userLineHaulCost}",
                                revlinehaulalloc,
                                rev.BookTotalWgt,
                                this.TotalWgt,
                                userLineHaulCost);

                            break;
                        case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Distance:
                          
                            if (rev.BookMilesFrom.HasValue && rev.BookMilesFrom > 0)
                            {
                                revlinehaulalloc = DTran.AllocateCostByPercentage(rev.BookMilesFrom.Value,
                                    this.TotalMiles, userLineHaulCost);
                                rev.BookRevLineHaul = revlinehaulalloc;
                                rev.BookRevCarrierCost = revlinehaulalloc;
                                rev.BookRevDiscount = 0;
                                Logger.Information(
                                    "Calculating BookRev Line Haul ({BookRevLineHaul}) for Distance using AllocateCostByPercentage on BookMilesFrom: {BookMilesFrom}, TotalMiles: {TotalMiles}, userLineHaulCost:{userLineHaulCost}",
                                    revlinehaulalloc,
                                    rev.BookMilesFrom,
                                    this.TotalMiles,
                                    userLineHaulCost);
                            }else
                            {
                                Logger.Warning("BookMilesFrom is null or 0.  Setting BookRevLineHaul, BookRevCarrierCost, BookRevDiscount to 0");
                               
                            }

                            break;
                        case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Even:
                            
                            revlinehaulalloc = Math.Round(userLineHaulCost / this.BookRevenues.Count(), 2);
                            rev.BookRevLineHaul = revlinehaulalloc;
                            rev.BookRevCarrierCost = revlinehaulalloc;
                            rev.BookRevDiscount = 0;

                            Logger.Information(
                                "Calculating BookRev Line Haul ({BookRevLineHaul}) for Even using userLineHaulCost ({UserLineHaulCost}) / BookRevenues.Count() ({BookRevenuesCount})",
                                revlinehaulalloc,
                                userLineHaulCost,
                                this.BookRevenues.Count());

                            break;
                        default:
                            break;
                    }

                    if (revlinehaulalloc > maxRevlineAlloc)
                    {
                        Logger.Information("revlinehaulAllocation ({RevLineHaulAllocation}) > MaxRevLineHaulAllocation ({MaxRevLineAllocation}), Setting MaxRevLineAlloc to {MaxRevLineAllocation} and MaxRev to {RevLineHaulAllocation}",
                            revlinehaulalloc,
                            rev,
                            maxRevlineAlloc,
                            revlinehaulalloc);

                        maxRevlineAlloc = revlinehaulalloc;
                        maxRev = rev;
                    }
                }

                decimal totalLoadLineHaul = this.sumTotalLineHaul();

                Logger.Information("Total Load Line Haul is {TotalLineHaul}", totalLoadLineHaul);
                if (maxRev != null)
                {
                    maxRev.BookRevLineHaul =
                        DTran.AllocationVarianceAdjustment(totalLoadLineHaul, userLineHaulCost, maxRev.BookRevLineHaul);
                    maxRev.BookRevCarrierCost = maxRev.BookRevLineHaul;
                    maxRev.BookRevDiscount = 0;

                    Logger.Information("MaxRev != null, Calculating bookRevLineHaul ({BookRevLineHaul}) = AllocationVarianceAdjustment(totalLoadLineHaul: {TotalLoadLineHaul}, userLineHaulCost: {UserLineHaulCost}, maxRev.BookRevLineHaul: {MaxRevBookRevLineHaul})",
                        maxRev.BookRevLineHaul,
                        totalLoadLineHaul,
                        userLineHaulCost,
                        maxRev.BookRevLineHaul);


                }
            }
        }

        /// <summary>
        /// Calculates the BFC manual as configured by the user from the client.  If autocalcBFC is true, it will skip this method.
        /// </summary>
        /// <param name="usertotalbfc"></param>
        /// <param name="allocation"></param>
        /// <param name="autoCalcBFC"></param>
        public void allocateBFCCostsByAllocationMode(decimal usertotalbfc, DTO.tblTarBracketType allocation, bool autoCalcBFC)
        {
            using (var operation =
                   Logger.StartActivity(
                       "allocateBFCCostsByAllocationMode(usertotalbfc: {usertotalbfc}, allocation: {allocation}, autoCalcBFC: {autoCalcBFC}",
                       usertotalbfc,
                       allocation.TarBracketTypeName, autoCalcBFC))
            {

                if (autoCalcBFC == true)
                {
                    Logger.Information("autoCalcBFC is true, returning nothing.  Which makes you wonder why this is here...");
                    operation.Complete();
                    return;
                }

                DTO.BookRevenue maxRev = null;
                decimal maxRevBFCAlloc = 0;
                foreach (DTO.BookRevenue rev in this.BookRevenues)
                {
                    Logger.Information("Calculating BFC Costs for {BookRevenue}...", rev);

                    if (rev.BookTotalBFC > 0 || rev.BookRevBilledBFC > 0 || rev.BookFinARBookFrt > 0)
                    {
                        Logger.Information($"BookTotalBFC ({rev.BookTotalBFC}), BookRevBilledBFC ({rev.BookRevBilledBFC}), BookFinARBookFrt ({rev.BookFinARBookFrt}) are already set.  Skipping this record.");
                        continue;
                    }

                    //no need to calculate if the bfc is 0 anyways.
                    if (usertotalbfc == 0)
                    {
                        Logger.Information(
                            "User Total BFC is 0.  Setting BookTotalBFC, BookRevBilledBFC, BookFinARBookFrt to 0");
                        rev.BookTotalBFC = 0;
                        rev.BookRevBilledBFC = 0;
                        rev.BookFinARBookFrt = 0;
                        //move to next record.
                        continue;
                    }

                    decimal revBFCalloc = 0; //reset to zero

                    using (Logger.StartActivity("Checking BracketTypeControl ({BracketTypeControl})", allocation.TarBracketTypeControl))
                    {

                        switch (allocation.TarBracketTypeControl)
                        {
                            case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Pallets:

                                revBFCalloc = DTran.AllocateCostByPercentage(rev.BookTotalPL, this.TotalPL, usertotalbfc);
                                rev.BookTotalBFC = revBFCalloc;
                                rev.BookRevBilledBFC = revBFCalloc;
                                rev.BookFinARBookFrt = revBFCalloc;

                                Logger.Information(
                                    "Calculating BFC Costs ({revBFCalloc}) for Pallets using AllocateCostByPercentage on BookTotalPL: {BookTotalPL}, TotalPL: {TotalPL}, usertotalbfc:{usertotalbfc}",
                                    revBFCalloc,
                                    rev.BookTotalPL,
                                    this.TotalPL,
                                    usertotalbfc);

                                break;
                            case (int)Ngl.FreightMaster.Data.Utilities.BracketType.FlatPallet:
                                //Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic

                                revBFCalloc = DTran.AllocateCostByPercentage(rev.BookTotalPL, this.TotalPL, usertotalbfc);
                                rev.BookTotalBFC = revBFCalloc;
                                rev.BookRevBilledBFC = revBFCalloc;
                                rev.BookFinARBookFrt = revBFCalloc;

                                Logger.Information(
                                    "Calculating BFC Costs ({revBFCalloc}) for FlatPallets using AllocateCostByPercentage on BookTotalPL: {BookTotalPL}, TotalPL: {TotalPL}, usertotalbfc:{usertotalbfc}",
                                    revBFCalloc,
                                    rev.BookTotalPL,
                                    this.TotalPL,
                                    usertotalbfc);

                                break;
                            case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Volume:

                                revBFCalloc =
                                    DTran.AllocateCostByPercentage(rev.BookTotalCube, this.TotalCube, usertotalbfc);
                                rev.BookTotalBFC = revBFCalloc;
                                rev.BookRevBilledBFC = revBFCalloc;
                                rev.BookFinARBookFrt = revBFCalloc;
                                rev.BookFinARBookFrt = revBFCalloc;

                                Logger.Information(
                                    "Calculating BFC Costs ({revBFCalloc}) for Volume using AllocateCostByPercentage on BookTotalCube: {BookTotalCube}, TotalCube: {TotalCube}, usertotalbfc:{usertotalbfc}",
                                    revBFCalloc,
                                    rev.BookTotalCube,
                                    this.TotalCube,
                                    usertotalbfc);

                                break;
                            case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Quantity:

                                revBFCalloc =
                                    DTran.AllocateCostByPercentage(rev.BookTotalCases, this.TotalCases, usertotalbfc);
                                rev.BookTotalBFC = revBFCalloc;
                                rev.BookRevBilledBFC = revBFCalloc;
                                rev.BookFinARBookFrt = revBFCalloc;

                                Logger.Information(
                                    "Calculating BFC Costs ({revBFCalloc}) for Quantity using AllocateCostByPercentage on BookTotalCases: {BookTotalCases}, TotalCases: {TotalCases}, usertotalbfc:{usertotalbfc}",
                                    revBFCalloc,
                                    rev.BookTotalCases,
                                    this.TotalCases,
                                    usertotalbfc);

                                break;
                            case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Lbs:

                                revBFCalloc = DTran.AllocateCostByPercentage(rev.BookTotalWgt, this.TotalWgt, usertotalbfc);
                                rev.BookTotalBFC = revBFCalloc;
                                rev.BookRevBilledBFC = revBFCalloc;
                                rev.BookFinARBookFrt = revBFCalloc;

                                Logger.Information(
                                    "Calculating BFC Costs ({revBFCalloc}) for Lbs using AllocateCostByPercentage on BookTotalWgt: {BookTotalWgt}, TotalWgt: {TotalWgt}, usertotalbfc:{usertotalbfc}",
                                    revBFCalloc,
                                    rev.BookTotalWgt,
                                    this.TotalWgt,
                                    usertotalbfc);

                                break;
                            case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Cwt:

                                revBFCalloc = DTran.AllocateCostByPercentage(rev.BookTotalWgt, this.TotalWgt, usertotalbfc);
                                rev.BookTotalBFC = revBFCalloc;
                                rev.BookRevBilledBFC = revBFCalloc;
                                rev.BookFinARBookFrt = revBFCalloc;

                                Logger.Information(
                                    "Calculating BFC Costs ({revBFCalloc}) for Cwt using AllocateCostByPercentage on BookTotalWgt: {BookTotalWgt}, TotalWgt: {TotalWgt}, usertotalbfc:{usertotalbfc}",
                                    revBFCalloc,
                                    rev.BookTotalWgt,
                                    this.TotalWgt,
                                    usertotalbfc);

                                break;
                            case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Distance:

                                if (rev.BookMilesFrom.HasValue && rev.BookMilesFrom > 0)
                                {
                                    revBFCalloc = DTran.AllocateCostByPercentage(rev.BookMilesFrom.Value, this.TotalMiles,
                                        usertotalbfc);
                                    rev.BookTotalBFC = revBFCalloc;
                                    rev.BookRevBilledBFC = revBFCalloc;
                                    rev.BookFinARBookFrt = revBFCalloc;

                                    Logger.Information(
                                        "Calculating BFC Costs ({revBFCalloc}) for Distance using AllocateCostByPercentage on BookMilesFrom: {BookMilesFrom}, TotalMiles: {TotalMiles}, usertotalbfc:{usertotalbfc}",
                                        revBFCalloc,
                                        rev.BookMilesFrom,
                                        this.TotalMiles,
                                        usertotalbfc);

                                }

                                break;
                            case (int)Ngl.FreightMaster.Data.Utilities.BracketType.Even:

                                revBFCalloc = Math.Round(usertotalbfc / this.BookRevenues.Count(), 2);
                                rev.BookTotalBFC = revBFCalloc;
                                rev.BookRevBilledBFC = revBFCalloc;
                                rev.BookFinARBookFrt = revBFCalloc;

                                Logger.Information(
                                    "Calculating BFC Costs ({revBFCalloc}) for Even using usertotalbfc ({usertotalbfc}) / BookRevenues.Count() ({BookRevenuesCount})",
                                    revBFCalloc,
                                    usertotalbfc,
                                    this.BookRevenues.Count());

                                break;
                            default:
                                break;
                        }
                    }
                    if (revBFCalloc > maxRevBFCAlloc)
                    {
                        maxRevBFCAlloc = revBFCalloc;
                        maxRev = rev;

                        Logger.Information(
                            "revBFCalloc ({revBFCalloc}) > maxRevBFCAlloc ({maxRevBFCAlloc}), Setting maxRevBFCAlloc to {maxRevBFCAlloc} and maxRev to {revBFCalloc}",
                            revBFCalloc,
                            maxRevBFCAlloc,
                            revBFCalloc);   
                    }
                }

                decimal totalLoadBFC = this.sumTotalBFC();

                Logger.Information("Total BasicFreightCost: {TotalBFC}", totalLoadBFC);

                if (maxRev != null)
                {
                    maxRev.BookTotalBFC =
                        DTran.AllocationVarianceAdjustment(totalLoadBFC, usertotalbfc, maxRev.BookTotalBFC);
                    maxRev.BookRevBilledBFC = maxRev.BookTotalBFC;
                    maxRev.BookFinARBookFrt = maxRev.BookTotalBFC;
                    
                    Logger.Information(
                        "maxRev != null, Calculating BookRevBilledBFC ({BookRevBilledBFC}) = AllocationVarianceAdjustment(totalLoadBFC: {TotalLoadBFC}, usertotalbfc: {UserTotalBFC}, maxRev.BookTotalBFC: {MaxRevBookTotalBFC})",
                        maxRev.BookRevBilledBFC,
                        totalLoadBFC,
                        usertotalbfc,
                        maxRev.BookTotalBFC);

                }
            }
        }

        /// <summary>
        /// Resets the RatedBookRevenue property based on the current pivot data returned.
        /// the default is the last stop if a value does not exist
        /// </summary>
        /// <param name="intRatedBookControl"></param>
        /// <returns></returns>
        public DTO.BookRevenue assignRatedBookRevenue(int intRatedBookControl)
        {
            RatedBookRevenue = null;
            using (var operation = Logger.StartActivity("Assigning Rated Book Revenue if _bookRevenues isn't null and length > 0 ({0}) and intRatedBookControl {1} != 0 and BookRevenues.Any(BookControl = {1})",
                       (_bookRevenues != null && _bookRevenues.Length > 0),
                       intRatedBookControl))
            {
                if (_bookRevenues != null && _bookRevenues.Length > 0 && intRatedBookControl != 0 &&
                    _bookRevenues.Any(x => x.BookControl == intRatedBookControl))
                {
                    RatedBookRevenue = _bookRevenues.FirstOrDefault(x => x.BookControl == intRatedBookControl);
                    Logger.Information("Rated Book Revenue Assigned: {RatedBookRevenue}", RatedBookRevenue);
                }
            }
            return RatedBookRevenue;
        }
        #endregion

        #region "Helper Methods"

        /// <summary>
        /// Get the collection of BookRevenue objects for the specified bookControl number.
        /// </summary>
        /// <param name="bookControl">Book Control number used for selecting the BookRevenue objects.</param>
        /// <returns>Collection of BookRevenue objects for the specified bookControl number.</returns>
        private DTO.BookRevenue[] getBookRevenuesWDetailsFiltered(int bookControl)
        {
            DTO.BookRevenue[] result = null;    // nothing found
            using (var operation = Logger.StartActivity("getBookRevenuesWDetailsFiltered(BookControl: {BookControl})", bookControl))
            {

                try
                {
                    result = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(bookControl);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    operation.Complete(null, sqlEx);
                    Logger.Error(sqlEx, "getBookRevenuesWDetailsFiltered");
                    //if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                    //{
                    //    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    //    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                    //        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    //    SaveSysError(errMsg, sourcePath("getBookRevenuesWDetailsFiltered"), bookControl.ToString());
                    //}
                }
                catch (Exception ex)
                {
                    operation.Complete(null, ex);
                    Logger.Error(ex, "getBookRevenuesWDetailsFiltered");
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    //throw; // gonna just remove this... CT

                }
            }

            return result;
        }


        /// <summary>
        /// Save the currenbt collection of BookRevenue data.
        /// </summary>
        /// <param name="sFaultInfo"></param>
        /// <returns></returns>
        internal bool saveBookRevenuesWDetailsFiltered(ref List<string> sFaultInfo, bool OptmisticConcurrencyOn = true)
        {
            bool blnRet = false;
            using (var operation = Logger.StartActivity("saveBookRevenuesWDetailsFiltered(sFaultInfo, OptimisticConcurrency: {OptimisticConcurrency})", OptmisticConcurrencyOn))
            {
                try
                {
                    /************************************************************
                     * Modified by RHR 10/17/13 we do not need the data back so we
                     * now call SaveRevenuesNoReturn
                     * ***********************************************************/
                    NGLBookRevenueData.SaveRevenuesNoReturn(this.BookRevenues, true, true, OptmisticConcurrencyOn);
                    blnRet = true;
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    operation.Complete(Serilog.Events.LogEventLevel.Error, sqlEx);
                    Logger.Error(sqlEx, "NGL.FM.CarTar.saveBookRevenuesWDetailsFiltered");
                    sFaultInfo = new List<string>
                        { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    //string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                    //    sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    //SaveSysError(errMsg, sourcePath("saveBookRevenuesWDetailsFiltered"));
                }
                catch (FaultException<DAL.ConflictFaultInfo> sqlEx)
                {
                    operation.Complete(null, sqlEx);
                    Logger.Error(sqlEx, "NGL.FM.CarTar.saveBookRevenuesWDetailsFiltered");
                    sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    //string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                    //    sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    //SaveSysError(errMsg, sourcePath("saveBookRevenuesWDetailsFiltered"));
                }
                catch (Exception ex)
                {
                    operation.Complete(null, ex);
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    //throw;
                    Logger.Error(ex, "NGL.FM.CarTar.saveBookRevenuesWDetailsFiltered");

                }
            }
            return blnRet;
        }
        #endregion
    }

}
