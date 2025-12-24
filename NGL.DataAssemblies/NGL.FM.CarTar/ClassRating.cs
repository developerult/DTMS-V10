using Ngl.FreightMaster.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingMapsRESTToolkit;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SerilogTracing;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;

namespace NGL.FM.CarTar
{
    class ClassRating : TarBaseClass
    {
        #region " Constructors "

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public ClassRating(DAL.WCFParameters oParameters)
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

            this.Logger = Logger.ForContext<ClassRating>();
        }
        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.ClassRating";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
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


        #endregion

        #region methods

        public decimal calculateLTLLineHaul(ref DTO.CarrierCostResults results,
            int nStandardWeightNdx,
            int nWgtBreakNdx,
            ref decimal nLineHaulNoDeficit,
            ref int nBestWeightBreakNdx,
            ref decimal nBestSummedCosts,
            List<decimal> listBreakPoints,
            List<string> lstClasses,
            decimal[,] weightMatrix,
            Dictionary<string, double> shipmentClasses,
            ref ClassRatingLineAllocation LineAllocation)
        {


            decimal nSummedCosts = 0.0M;
            using (var operation = Logger.StartActivity(
                       "calculateLTLLineHaul(results: {results}, nStandardWeightIndex: {nStandardWeightNdx}, nWgtBreakNdx: {nWgtBreakNdx})",
                       results, nStandardWeightNdx, nWgtBreakNdx))
            {
                int intTotalKeys = shipmentClasses.Where(x => x.Value > 0).Count();
                int intIndex = 0;
                //reset the line allocation variable for each iteration
                LineAllocation.nSummedClassWeight = 0;
                LineAllocation.nAcutalHeaviestClassWeight = 0;
                LineAllocation.nAdjustedHeaviestClassWeight = 0;
                LineAllocation.sHeaviestClass = "";
                foreach (KeyValuePair<string, double> pair in shipmentClasses.Where(x => x.Value > 0)
                             .OrderBy(x => x.Value))
                {
                    Logger.Information("Iterating through shipmentClass: {@Pair}", pair);
                    intIndex += 1;
                    if (lstClasses.Contains(pair.Key)) //be sure the listClasses has a reference to this class code
                    {

                        double
                            nClassWeight =
                                Math.Ceiling(pair
                                    .Value); //we round up each total to the next highest whole number for rating

                        Logger.Information("Class code {ClassCode} is in the list of classes, setting nClassWeight to next highest whole number {nClassWeight} (Math.Ceiling({PairValue}))",
                            pair.Key,
                            nClassWeight,
                            pair.Value);


                        LineAllocation.nSummedClassWeight += nClassWeight;
                        Logger.Information("Setting LineAllocation Class Weight Summarized to {SummedClassWeight}", LineAllocation.nSummedClassWeight);


                        if (intIndex >= intTotalKeys)
                        {

                            //this is the heaviest class so we need to make adjustments for any rounding errors caused by Math.Ceiling
                            LineAllocation.nAcutalHeaviestClassWeight = nClassWeight;
                            LineAllocation.nWgtRoundingVariance = Math.Ceiling(LineAllocation.nActualLoadWeight) -
                                                                  LineAllocation.nSummedClassWeight;
                            LineAllocation.nAdjustedHeaviestClassWeight =
                                nClassWeight + LineAllocation.nWgtRoundingVariance;
                            LineAllocation.sHeaviestClass = pair.Key;
                            //adjust the class weight for costing
                            nClassWeight = LineAllocation.nAdjustedHeaviestClassWeight;
                            Logger.Information("Index ({intIndex}) >= TotalKeys ({TotalKeys}), This is the last class in the list of classes, setting LineAllocation.nAcutalHeaviestClassWeight to {nClassWeight}, RoundingVariance: {RoundingVariance} ",
                                intIndex, intTotalKeys,
                                nClassWeight,
                                LineAllocation.nWgtRoundingVariance);
                        }

                        //calculate the cost for the current class at the select weight break 
                        //collect data here for ltl class rating.
                        //bookitemcarrtarEquipDetID is the value 1-10 = nWgtBreakNdx
                        //lstClasses.IndexOf(pair.Key)//class codes
                        //bookitemcarrtarequipdeetvalue = weightMatrix[lstClasses.IndexOf(pair.Key), nWgtBreakNdx]

                        Logger.Information("Setting nSummedCosts ({SummedCosts}) = nWeightClass ({nWeightClass}) * weightMatrix[lstClasses[{PairKey}],{nWgtBreakNdx}] ({WeightMatrix})",
                            nSummedCosts,
                            nClassWeight,
                            pair.Key,
                            nWgtBreakNdx,
                            weightMatrix[lstClasses.IndexOf(pair.Key),
                            nWgtBreakNdx]);

                        nSummedCosts +=
                            Decimal.Round(
                                (decimal)nClassWeight * weightMatrix[lstClasses.IndexOf(pair.Key), nWgtBreakNdx], 2);
                    }
                }

                //Add any costs associated with the difference between nActualLoadWeight and nMinAdjustedLoadWeight
                if (LineAllocation.nWgtAdjWeight > 0)
                {
                    Logger.Information("LineAllocation.WgtAdjWeight ({nWgtAdjWeight}) > 0 - calculating minimum weight adjustment cost and add to nSummedCosts ({nSummedCosts}) using sDeficitClass ({sDeficitClass}) if exists in lstClasses ({ExistsInClass})",
                        LineAllocation.nWgtAdjWeight,
                        nSummedCosts,
                        LineAllocation.sDeficitClass,
                        lstClasses.Contains(LineAllocation.sDeficitClass));

                    //calculate the minimum weight adjustment cost and add it to nSummedCosts using sDeficitClass
                    if (lstClasses.Contains(LineAllocation
                            .sDeficitClass)) //be sure the listClasses has a reference to this class code
                    {
                        nSummedCosts +=
                            Decimal.Round(
                                (decimal)LineAllocation.nWgtAdjWeight *
                                weightMatrix[lstClasses.IndexOf(LineAllocation.sDeficitClass), nWgtBreakNdx], 2);
                    }
                }

                nLineHaulNoDeficit = nSummedCosts;
                double nDeficitWeight = 0.0;
                decimal nDeficitCost = 0.0M;
                //Here we get the total cost of the load
                if (nWgtBreakNdx > nStandardWeightNdx)
                {
                    Logger.Information("nWgtBreakNdx ({nWgtBreakNdx}) > nStandardWeightNdx ({nStandardWeightNdx})",
                        nWgtBreakNdx,
                        nStandardWeightNdx);

                    nDeficitWeight = (double)listBreakPoints[nWgtBreakNdx] -
                                     (LineAllocation.nWgtAdjWeight + LineAllocation.nActualLoadWeight);
                    int nClassNdx = lstClasses.IndexOf(LineAllocation.sDeficitClass);

                    if (nClassNdx != -1)
                    {
                        //calculate the total cost at the deficit weight break rate using the full weight for the weight break
                        nDeficitCost = (decimal)nDeficitWeight * weightMatrix[nClassNdx, nWgtBreakNdx];
                        nDeficitCost = Decimal.Round(nDeficitCost, 2);
                    }

                    nSummedCosts += nDeficitCost;
                }

                if (nBestWeightBreakNdx == -1 || nSummedCosts < nBestSummedCosts)
                {
                    if (nDeficitWeight > 0.0) // We are using a deficit weight.
                    {
                        // Save the fields to return back.
                        LineAllocation.nBestDeficitWeight = nDeficitWeight;
                        LineAllocation.nBestDeficitCost = nDeficitCost;
                        LineAllocation.nBestDeficitWeightBreak = (double)listBreakPoints[nWgtBreakNdx];
                    }

                    LineAllocation.nRatedWeightBreak = (double)listBreakPoints[nWgtBreakNdx];
                    LineAllocation.nLineHaul = nLineHaulNoDeficit;
                    nBestWeightBreakNdx = nWgtBreakNdx;
                    nBestSummedCosts = nSummedCosts;
                }
            }

            return nSummedCosts;

        }

        /// <summary>
        /// updates the LineAllocation.nWgtAdjWeight and LineAllocation.nMinAdjustedLoadWeight
        /// </summary>
        /// <param name="results"></param>
        /// <param name="ratedLoad"></param>
        /// <param name="InterlineList"></param>
        /// <param name="carrierTariffsPivots"></param>
        /// <param name="LineAllocation"></param>
        /// <param name="minWeights"></param>
        public void updateMinimumWeightRequirements(ref DTO.CarrierCostResults results,
            Load ratedLoad,
            List<int> InterlineList,
            DTO.CarrierTariffsPivot[] carrierTariffsPivots,
            ref ClassRatingLineAllocation LineAllocation,
            ref DTO.CarrTarMinWeight[] minWeights)
        {
            /******************* Begin New Code Added for Minimum Weight **************************/
            //get the most precise min weight record.
            DTO.CarrTarMinWeight minWeight = null;
            if (ratedLoad.BookRevenues.Count() > 0)
            {
                results.AddLog("Get most precise min weights");
                //TODO need to know how to filter using the class codes in the min weight records?
                minWeight = MinWeight.getMostPreciseMinWeightForBookRev(
                    ratedLoad.BookRevenues[0],
                    minWeights,
                    InterlineList,
                    carrierTariffsPivots[0].CarrTarOutbound);
                if (minWeight != null)
                {
                    results.AddLog("MinWeight found CarrTarMinWeightPerLoad: " +
                        minWeight.CarrTarMinWeightPerLoad +
                        " and CarrTarMinWeightPerPallet: " +
                        minWeight.CarrTarMinWeightPerPallet);
                }
                else
                {
                    results.AddLog("Get Min Weight Precise returned null");
                }
            }

            // At this point we need to adjust the weight for LTL minimum weight 
            //by calculating the difference between the minimum weight per pallet and the actual total weight
            //Example:  if LineAllocation.nActualLoadWeight = 3000 lbs and we have two pallets (we need a way to read the total pallets)
            //          and the adjusted minimum weight is greater than 3000 like 3400 we have a phantom weight (pw) of 400 lbs.
            //          the adjust minimum weight (d) is the largest value based on the absolute minimum (a) and 
            //          the weight per pallet (w) times number of pallets (p)  so if w * p > a then d = w * p else d = a
            //          pw = d - LineAllocation.nActualLoadWeight.  If pw > 0 add pw to lowest class code in shipmentClasses
            // NOTE:  if the shipmentClasses has been update we need to call GetClassesTotalWeight again so that LineAllocation.nActualLoadWeight is correct
            //Due to minimum weight requirements in selected tariff, freight charges reflect an additional charge for 3000lbs
            //above shipped cargo at a cost of x dollars.
            //new fields in database.  
            //  BookRevMinWgtAdjCost mapped to LineAllocation.nWgtAdjCost
            //  BookRevMinWgtAdjWeight mapped to LineAllocation.nWgtAdjWeight
            //  BookRevMinWgtAdjWeightBreak mapped to LineAllocation.nWgtAdjWeightBreak
            //  BookRevActualLoadWeight mapped to LineAllocation.nActualLoadWeight
            //  BookRevBilledLoadWeight mapped to LineAllocation.nBilledLoadWeight
            //  BookRevMinWgtAdjLoadWeight mapped to LineAllocation.nMinAdjustedLoadWeight           
            if (minWeight != null)
            {
                LineAllocation.nMinAdjustedLoadWeight = MinWeight.selectHighestWeight(minWeight.CarrTarMinWeightPerPallet,
                    ratedLoad.TotalPL, minWeight.CarrTarMinWeightPerLoad, LineAllocation.nActualLoadWeight);
            }
            if (LineAllocation.nMinAdjustedLoadWeight > LineAllocation.nActualLoadWeight)
            {
                LineAllocation.nWgtAdjWeight = LineAllocation.nMinAdjustedLoadWeight - LineAllocation.nActualLoadWeight;
            }
            else
            {
                LineAllocation.nWgtAdjWeight = 0;
                LineAllocation.nMinAdjustedLoadWeight = LineAllocation.nActualLoadWeight; //no weight adjustment needed so adjusted weight = actual weight
            }




            //TODO need to allocate the weight back to item detail records according to their percentages, class codes and 
            //update the rated weight fields.
            /******************* End New Code Added for Minimum Weight **************************/
        }

        public void addItemToMatrixes(double itemWeight,
            string ClassUsed,
            string itemClass,
            int classType,
            int BookControl,
            ref Dictionary<string, double> shipmentClasses,
            ref Dictionary<Tuple<int, string>, double> AllocateShipmentClasses,
            ref ClassRatingLineAllocation LineAllocation)
        {
            if (shipmentClasses == null) { shipmentClasses = new Dictionary<string, double>(); }
            if (!shipmentClasses.ContainsKey(ClassUsed))
            {
                // Create dictionary element for this rated class.Add the weight to the total weight for this rated class.
                shipmentClasses.Add(ClassUsed, itemWeight);
            }
            else
            {
                // Add the weight to the total weight for this rated class.
                shipmentClasses[ClassUsed] += itemWeight;
            }
            if (LineAllocation == null) { LineAllocation = new ClassRatingLineAllocation(); }
            if (LineAllocation.FAKMapping == null) { LineAllocation.FAKMapping = new Dictionary<Tuple<string, int>, string>(); }
            // Add to our FAK mapping dictionary.
            if (!LineAllocation.FAKMapping.ContainsKey(Tuple.Create(itemClass, classType)))
            {
                LineAllocation.FAKMapping.Add(Tuple.Create(itemClass, classType), ClassUsed);
            }

            if (LineAllocation.WeightMapping == null) { LineAllocation.WeightMapping = new Dictionary<Tuple<string, int>, double>(); }
            if (!LineAllocation.WeightMapping.ContainsKey(Tuple.Create(ClassUsed, classType)))
            {
                LineAllocation.WeightMapping.Add(Tuple.Create(ClassUsed, classType), itemWeight);
            }
            else
            {
                LineAllocation.WeightMapping[Tuple.Create(ClassUsed, classType)] += itemWeight;
            }
            if (AllocateShipmentClasses == null) { AllocateShipmentClasses = new Dictionary<Tuple<int, string>, double>(); }
            if (!AllocateShipmentClasses.ContainsKey(Tuple.Create(BookControl, ClassUsed)))
            {
                AllocateShipmentClasses.Add(Tuple.Create(BookControl, ClassUsed), itemWeight);
            }
            else
            {
                AllocateShipmentClasses[Tuple.Create(BookControl, ClassUsed)] += itemWeight;
            }
        }

        public string getFAKMapping(string itemClass, int classType, DAL.Utilities.PointType eInterlineStatus, ref DTO.CarrTarClassXref[] faks, ref bool bMatchedType)
        {
            string ClassUsed = itemClass;
            if ((faks != null) && faks.Count() > 0)
            {
                foreach (DTO.CarrTarClassXref xref in faks)
                {
                    double nClassFrom;
                    Double.TryParse(xref.CarrTarClassXrefActualFrom, out nClassFrom);
                    double nClassTo;
                    Double.TryParse(xref.CarrTarClassXrefActualTo, out nClassTo);
                    double nItemClass;
                    //Double.TryParse(itemClass, out nItemClass);
                    string numbersOnly = new string(itemClass.Where(char.IsDigit).ToArray());
                    Double.TryParse(numbersOnly, out nItemClass);

                    // Must be in the range, and also the right type of xref
                    if (classType == xref.CarrTarClassXrefClassTypeControl &&
                        nClassFrom <= nItemClass &&
                        nClassTo >= nItemClass)
                    {
                        if (xref.CarrTarClassXrefPointTypeControl == (int)Utilities.PointType.Any)
                        {
                            if (bMatchedType == false)
                            {
                                ClassUsed = xref.CarrTarClassXrefRated;
                            }
                        }
                        else if (xref.CarrTarClassXrefPointTypeControl == (int)eInterlineStatus)
                        {
                            bMatchedType = true;
                            ClassUsed = xref.CarrTarClassXrefRated;
                        }
                    }
                }
            }
            return ClassUsed;
        }

        public string getItemClass(ref DTO.CarrierCostResults results,
            Load ratedLoad,
            int classType,
            DTO.BookItem bookItem,
            bool blnRateShopping = false)
        {
            string itemClass = "100";
            string s49CFRDefault = "100";
            string sIATADefault = "100";
            string sDOTDefault = "100";
            string sMarineDefault = "100";
            string sNMFCDefault = "100";
            string sFAKDefault = "100";

            List<string> sFaultInfo = new List<string>();
            //Reads the default class codes for this company
            populateDefaultClassCodes(ratedLoad.RatedBookRevenue.BookCustCompControl, ref sFaultInfo, ref s49CFRDefault, ref sIATADefault, ref sDOTDefault, ref sMarineDefault, ref sNMFCDefault, ref sFAKDefault);
            if ((sFaultInfo != null && sFaultInfo.Count > 0))
            {
                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
            }

            switch (classType)
            {
                case (int)DAL.Utilities.TariffClassType.class49CFR:
                    {
                        if ((blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.class49CFR) && string.IsNullOrWhiteSpace(ratedLoad.RateShopMatClass)) || bookItem == null || string.IsNullOrWhiteSpace(bookItem.BookItem49CFRCode))
                        {
                            itemClass = s49CFRDefault;
                        }
                        else if (blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.class49CFR))
                        {
                            itemClass = ratedLoad.RateShopMatClass;
                        }
                        else
                        {
                            itemClass = bookItem.BookItem49CFRCode;
                        }
                        break;
                    }
                case (int)DAL.Utilities.TariffClassType.classDOT:
                    {
                        if ((blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classDOT) && string.IsNullOrWhiteSpace(ratedLoad.RateShopMatClass)) || bookItem == null || string.IsNullOrWhiteSpace(bookItem.BookItemDOTCode))
                        {
                            itemClass = sDOTDefault;
                        }
                        else if (blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classDOT))
                        {
                            itemClass = ratedLoad.RateShopMatClass;
                        }
                        else
                        {
                            itemClass = bookItem.BookItemDOTCode;
                        }
                        break;
                    }
                case (int)DAL.Utilities.TariffClassType.classFAK:
                    {
                        if ((blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classFAK) && string.IsNullOrWhiteSpace(ratedLoad.RateShopMatClass)) || bookItem == null || string.IsNullOrWhiteSpace(bookItem.BookItemFAKClass))
                        {
                            itemClass = sFAKDefault;
                        }
                        else if (blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classFAK))
                        {
                            itemClass = ratedLoad.RateShopMatClass;
                        }
                        else
                        {
                            itemClass = bookItem.BookItemFAKClass;
                        }
                        break;
                    }
                case (int)DAL.Utilities.TariffClassType.classIATA:
                    {
                        if ((blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classIATA) && string.IsNullOrWhiteSpace(ratedLoad.RateShopMatClass)) || bookItem == null || string.IsNullOrWhiteSpace(bookItem.BookItemIATACode))
                        {
                            itemClass = sIATADefault;
                        }
                        else if (blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classIATA))
                        {
                            itemClass = ratedLoad.RateShopMatClass;
                        }
                        else
                        {
                            itemClass = bookItem.BookItemIATACode;
                        }
                        break;
                    }
                case (int)DAL.Utilities.TariffClassType.classMarine:
                    {
                        if ((blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classMarine) && string.IsNullOrWhiteSpace(ratedLoad.RateShopMatClass)) || bookItem == null || string.IsNullOrWhiteSpace(bookItem.BookItemMarineCode))
                        {
                            itemClass = sMarineDefault;
                        }
                        else if (blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classMarine))
                        {
                            itemClass = ratedLoad.RateShopMatClass;
                        }
                        else
                        {
                            itemClass = bookItem.BookItemMarineCode;
                        }
                        break;
                    }
                case (int)DAL.Utilities.TariffClassType.classNMFC:
                    {
                        if ((blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classNMFC) && string.IsNullOrWhiteSpace(ratedLoad.RateShopMatClass)) || bookItem == null || string.IsNullOrWhiteSpace(bookItem.BookItemNMFCClass))
                        {
                            itemClass = sNMFCDefault;
                        }
                        else if (blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classNMFC))
                        {
                            itemClass = ratedLoad.RateShopMatClass;
                        }
                        else
                        {
                            itemClass = bookItem.BookItemNMFCClass;
                        }

                        break;
                    }
                case (int)DAL.Utilities.TariffClassType.None:
                    {
                        itemClass = "";  //This should not happen because we should not get this far if 
                        return itemClass;
                    }
            }
            if (string.IsNullOrEmpty(itemClass) || itemClass.Trim().Length < 1)
            {
                itemClass = "100";
            }

            return itemClass;
        }

        /// <summary>
        /// Get the class data from bookpackage settings
        /// </summary>
        /// <param name="results"></param>
        /// <param name="ratedLoad"></param>
        /// <param name="classType"></param>
        /// <param name="Pkg"></param>
        /// <param name="blnRateShopping"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2.0.118 on 09/06/2019 for bookpackage logic
        /// </remarks>
        public string getPkgClass(ref DTO.CarrierCostResults results,
         Load ratedLoad,
         int classType,
         LTS.BookPackage Pkg,
         bool blnRateShopping = false)
        {
            string itemClass = "100";
            string s49CFRDefault = "100";
            string sIATADefault = "100";
            string sDOTDefault = "100";
            string sMarineDefault = "100";
            string sNMFCDefault = "100";
            string sFAKDefault = "100";

            List<string> sFaultInfo = new List<string>();
            //Reads the default class codes for this company
            populateDefaultClassCodes(ratedLoad.RatedBookRevenue.BookCustCompControl, ref sFaultInfo, ref s49CFRDefault, ref sIATADefault, ref sDOTDefault, ref sMarineDefault, ref sNMFCDefault, ref sFAKDefault);
            if ((sFaultInfo != null && sFaultInfo.Count > 0))
            {
                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
            }

            switch (classType)
            {

                case (int)DAL.Utilities.TariffClassType.classFAK:
                    {
                        if ((blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classFAK) && string.IsNullOrWhiteSpace(ratedLoad.RateShopMatClass)) || Pkg == null || string.IsNullOrWhiteSpace(Pkg.BookPkgFAKClass))
                        {
                            itemClass = sFAKDefault;
                        }
                        else if (blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classFAK))
                        {
                            itemClass = ratedLoad.RateShopMatClass;
                        }
                        else
                        {
                            itemClass = Pkg.BookPkgFAKClass;
                        }
                        break;
                    }

                case (int)DAL.Utilities.TariffClassType.classNMFC:
                    {
                        if ((blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classNMFC) && string.IsNullOrWhiteSpace(ratedLoad.RateShopMatClass)) || Pkg == null || string.IsNullOrWhiteSpace(Pkg.BookPkgNMFCClass))
                        {
                            itemClass = sNMFCDefault;
                        }
                        else if (blnRateShopping && (ratedLoad.RateShopMatClassTypeControl == 0 || ratedLoad.RateShopMatClassTypeControl == (int)DAL.Utilities.TariffClassType.classNMFC))
                        {
                            itemClass = ratedLoad.RateShopMatClass;
                        }
                        else
                        {
                            itemClass = Pkg.BookPkgNMFCClass;
                        }

                        break;
                    }
                case (int)DAL.Utilities.TariffClassType.None:
                    {
                        itemClass = "";  //This should not happen because we should not get this far if 
                        return itemClass;
                    }
            }
            if (string.IsNullOrEmpty(itemClass) || itemClass.Trim().Length < 1)
            {
                itemClass = "100";
            }

            return itemClass;
        }


        /// <summary>
        /// Build a cost matrix for LTL shipments based on class codes
        /// </summary>
        /// <param name="results"></param>
        /// <param name="ratedLoad"></param>
        /// <param name="carrierTariffsPivots"></param>
        /// <param name="eInterlineStatus"></param>
        /// <param name="shipmentClasses"></param>
        /// <param name="AllocateShipmentClasses"></param>
        /// <param name="LineAllocation"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2.0.118 on 9/6/19
        ///  added logic to look up bookpackage data
        /// </remarks>
        public bool buildLTLRateMatrix(ref DTO.CarrierCostResults results,
            Load ratedLoad,
            DTO.CarrierTariffsPivot[] carrierTariffsPivots,
            DAL.Utilities.PointType eInterlineStatus,
            ref Dictionary<string, double> shipmentClasses,
            ref Dictionary<Tuple<int, string>, double> AllocateShipmentClasses,
            ref ClassRatingLineAllocation LineAllocation
            )
        {
            bool blnRet = false;

            // Get the FAK information "ClassXrefData. 
            //There's no guarantee that we can handle the precision the way we want to using a query, 
            // so we need to scan through the list each time to make the best match on a class-by-class basis.
            DTO.CarrTarClassXref[] faks = ClassXrefInstance.GetCarrTarClassXrefsFiltered(carrierTariffsPivots[0].CarrTarControl);
            string s49CFRDefault = "100";
            string sIATADefault = "100";
            string sDOTDefault = "100";
            string sMarineDefault = "100";
            string sNMFCDefault = "100";
            string sFAKDefault = "100";

            List<string> sFaultInfo = new List<string>();
            //Reads the default class codes for this company
            populateDefaultClassCodes(ratedLoad.RatedBookRevenue.BookCustCompControl, ref sFaultInfo, ref s49CFRDefault, ref sIATADefault, ref sDOTDefault, ref sMarineDefault, ref sNMFCDefault, ref sFAKDefault);
            if ((sFaultInfo != null && sFaultInfo.Count > 0))
            {
                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff, sFaultInfo);
            }

            // This determines which type of class to use.
            int classType = carrierTariffsPivots[0].CarrTarEquipMatClassTypeControl;
            if (classType == (int)DAL.Utilities.TariffClassType.None)
            {
                // this is not an LTL tariff so we cannot rate using this Pivot record.                
                results.AddLog(DTO.CarrierCostResults.MessageEnum.M_InvalidClassTypeForLTLTariff, new List<string> { carrierTariffsPivots[0].CarrierName, carrierTariffsPivots[0].CarrTarID });
                return false;
            }

            string itemClass;
            double itemWeight;
            // Find class information.
            foreach (DTO.BookRevenue bookRev in ratedLoad.BookRevenues)
            {
                // Modified by RHR for v-8.2.0.118 on 9/6/19
                // added logic to look up bookpackage data
                LTS.BookPackage[] oLtsPkgs = NGLBookPackageData.GetBookPackages(bookRev.BookControl);
                if (oLtsPkgs != null && oLtsPkgs.Count() > 0)
                {
                    foreach (LTS.BookPackage pkg in oLtsPkgs)
                    {
                        itemClass = getPkgClass(ref results, ratedLoad, classType, pkg);
                        if (string.IsNullOrEmpty(itemClass)) { itemWeight = 0; itemClass = "100"; } else { itemWeight = pkg.BookPkgWeight; }
                        bool bMatchedType = false;
                        // Now we should have an item with a weight, class and potentially map it to an FAK class.
                        string ClassUsed = getFAKMapping(itemClass, classType, eInterlineStatus, ref faks, ref bMatchedType);
                        addItemToMatrixes(itemWeight, ClassUsed, itemClass, classType, bookRev.BookControl, ref shipmentClasses, ref AllocateShipmentClasses, ref LineAllocation);
                        //after we add item to matix, call LineAllocation.newObject.CreateBookItem(bookitemcontrol,itemClass,classType,ClassUsed)
                    }
                }
                else
                {

                    if (bookRev.BookLoads != null && bookRev.BookLoads.Count() > 0)
                    {
                        foreach (DTO.BookLoad bookLoad in bookRev.BookLoads)
                        {
                            if (bookLoad.BookItems != null && bookLoad.BookItems.Count() > 0)
                            {
                                foreach (DTO.BookItem bookItem in bookLoad.BookItems)
                                {
                                    itemClass = getItemClass(ref results, ratedLoad, classType, bookItem);
                                    if (string.IsNullOrEmpty(itemClass)) { itemWeight = 0; itemClass = "100"; } else { itemWeight = bookItem.BookItemWeight; }
                                    bool bMatchedType = false;
                                    // Now we should have an item with a weight, class and potentially map it to an FAK class.
                                    string ClassUsed = getFAKMapping(itemClass, classType, eInterlineStatus, ref faks, ref bMatchedType);
                                    addItemToMatrixes(itemWeight, ClassUsed, itemClass, classType, bookRev.BookControl, ref shipmentClasses, ref AllocateShipmentClasses, ref LineAllocation);
                                    //after we add item to matix, call LineAllocation.newObject.CreateBookItem(bookitemcontrol,itemClass,classType,ClassUsed)
                                }
                            }
                            else
                            {
                                //no items 
                                itemClass = getItemClass(ref results, ratedLoad, classType, null);
                                if (string.IsNullOrWhiteSpace(itemClass)) { itemWeight = 0; itemClass = "100"; } else { itemWeight = bookLoad.BookLoadWgt; }
                                bool bMatchedType = false;
                                // Now we should have an item with a weight, class and potentially map it to an FAK class.
                                string ClassUsed = getFAKMapping(itemClass, classType, eInterlineStatus, ref faks, ref bMatchedType);
                                addItemToMatrixes(itemWeight, ClassUsed, itemClass, classType, bookRev.BookControl, ref shipmentClasses, ref AllocateShipmentClasses, ref LineAllocation);
                            }
                        }
                    }
                    else
                    {
                        //we are Rate Shopping    
                        //Modified by RHR 10/30/14 we now use the Rate Shop class data stored in the ratedLoad object 
                        itemClass = getItemClass(ref results, ratedLoad, classType, null, true);
                        if (string.IsNullOrWhiteSpace(itemClass)) { itemWeight = 0; itemClass = Util.RateShopClassCode; } else { itemWeight = bookRev.BookTotalWgt; }
                        bool bMatchedType = false;
                        // Now we should have an item with a weight, class and potentially map it to an FAK class.
                        string ClassUsed = getFAKMapping(itemClass, classType, eInterlineStatus, ref faks, ref bMatchedType);
                        addItemToMatrixes(itemWeight, ClassUsed, itemClass, classType, bookRev.BookControl, ref shipmentClasses, ref AllocateShipmentClasses, ref LineAllocation);
                    }
                }
            }
            blnRet = true;
            return blnRet;
        }

        /// <summary>
        /// Calculates a charge based on class rating
        /// </summary>
        /// <param name="results">DTO.CarrierCostResults returned by the Tariff Engine to the caller</param>
        /// <param name="ratedLoad">Current load we are rating</param>
        /// <param name="InterlineList">List bookcontrol nunmbers identified as interline points for load.</param>
        /// <param name="carrierTariffsPivots">Contains a list of pivot records broken down by class.</param>
        /// <param name="DictAlloc"></param>
        /// <param name="LineAllocation"></param>
        /// <param name="ratedPivots">list of tariff pivots used to rate the LTL Load</param>
        /// <returns>Cost for class rating</returns>
        public decimal calcClassRatingCharge(ref DTO.CarrierCostResults results,
            Load ratedLoad,
            List<int> InterlineList,
            DTO.CarrierTariffsPivot[] carrierTariffsPivots,
            ref Dictionary<int, double> DictAlloc,
            ref ClassRatingLineAllocation LineAllocation,
            ref List<DTO.CarrierTariffsPivot> ratedPivots,
            DTO.CarrTarMinWeight[] minWeights)
        {


            decimal nBestSummedCosts = 0.0M; // Dummy value

            using (var operation = Logger.StartActivity("calcClassRatingCharge for Load: {Load}", ratedLoad))
            {
                List<string> lstClasses = new List<string>();

                foreach (DTO.CarrierTariffsPivot cls in carrierTariffsPivots)
                {
                    Logger.Information("Adding EquipmentMatrixClass: {CarrTarEquipMatClass} Class lookup", cls.CarrTarEquipMatClass);
                    lstClasses.Add(cls.CarrTarEquipMatClass.Trim());
                }
                //Set up defaults for break point pricing
                int nBestWeightBreakNdx = -1; // We don't yet have a cheapest weight break.
                decimal nLineHaulNoDeficit = 0.0M; // The linehaul portion without deficit rating.
                int nCountClasses = lstClasses.Count();
                DAL.Utilities.PointType eInterlineStatus = DAL.Utilities.PointType.Direct;

                //Represents a list of ItemWeight summarized and grouped by class
                Dictionary<string, double> shipmentClasses = new Dictionary<string, double>();

                // Class/weight mapping by Book Revenue objects.
                //Represents a list of ItemWeight summarized and grouped by bookcontrol and class
                var AllocateShipmentClasses = new Dictionary<Tuple<int, string>, double>();


                // Determine what we are looking for, interline or directs.
                Logger.Information("Checking for Interline points based on InterlineList != null and Count({InterLineListCount}) > 0", InterlineList?.Count);
                if (InterlineList != null && InterlineList.Count > 0)
                {
                    // At least one point is interline.
                    eInterlineStatus = DAL.Utilities.PointType.Interline;
                }

                // Build up the list of break points.
                //this code is a bug waiting to happen if the users skip a weight break the indexs are all wrong because GetBreakPoints skips values
                //this code must be re-written

                Logger.Information("Get list breakpoints for carrierTariffsPivots[0] ({CarrierTariffPivot})", carrierTariffsPivots[0]);

                List<decimal> listBreakPoints = GetBreakPoints(carrierTariffsPivots[0]);
                int nCountWeightBreaks = listBreakPoints.Count;

                if (nCountWeightBreaks == 0) // If there are no weight breaks (null values) then do not rate.
                {
                    Logger.Warning("No weight breaks found in tariff pivot record {CarrierTariffPivot}", carrierTariffsPivots[0]);
                    operation.Complete();
                    return 0.0M;
                }

                // Fixed multi-dimensional array.
                // Create a matrix of rates based on the classes/weight breaks in tariff. Commonly, this will map everything to one class, but it

                //This is a bug waiting to happen.  If the users leave a breakpoint blank the rates will have the wrong reference in the weightMatrix index
                //this needs to be re-written to use a fixed list of rates and adjust below for zero values where needed.
                //We need to determine what to do if the users leave a value blank or zero for both the weight breaks and the rate value
                decimal[,] weightMatrix = new decimal[nCountClasses, nCountWeightBreaks];

                for (int i = 0; i < nCountClasses; i++)
                {
                    GetRatesForClassRecord(carrierTariffsPivots[i], weightMatrix, nCountWeightBreaks, i);
                }

                if (!buildLTLRateMatrix(ref results, ratedLoad, carrierTariffsPivots, eInterlineStatus,
                        ref shipmentClasses, ref AllocateShipmentClasses, ref LineAllocation))
                {
                    operation.Complete();
                    return -1;
                }

                //Validate that every item class (shipmentClasses) is supported in the tariff pivot record (lstClasses)
                if (ValidateClasses(shipmentClasses, lstClasses) == false)
                {
                    // TODO log that a shipment or FAK class could not be mapped.
                    Logger.Warning("Shipment or FAK class could not be mapped");
                    operation.Complete();
                    return -1;
                }

                // Get total weight. Add up all the weights in the dictionary.

                LineAllocation.nActualLoadWeight = shipmentClasses.Sum(x => x.Value);
                Logger.Information("Calculate LineAllocation Actual Load Weight: {ActualLoadWeight}", LineAllocation.nActualLoadWeight);


                //code added by RHR 5/2/15 we now calculate the line haul cost on the original weight to determine the upcharge based on minimum weight requirements
                //First use the LineAllocation.nActualLoadWeight to get the first available weight class 

                int nStandardWeightNdx = GetStandardWeightBreak(listBreakPoints, LineAllocation.nActualLoadWeight);
                LineAllocation.sDeficitClass =
                    DeficitClass(shipmentClasses, weightMatrix, lstClasses, nStandardWeightNdx);

                Logger.Information("Set LineAllocation.sDeficitClass to {@DeficitClass}", LineAllocation.sDeficitClass);

                // The discount information could be looked up here, but that would require that the structure of the discounts to vary by weight break.
                // Since the discounts at the current time do not vary by weight break, then they are independent of deficit rating. So do not handle discounts
                // until a time when the discount structure reflects weight breaks.

                // In this loop, we will need to know the maximum weight break to use. 
                // Consider deficiting up only one break point but stop once we reach 
                //the last break point to avoid an index out of range error. 
                int nDeficitIndex = Math.Min((Math.Max(nStandardWeightNdx, 0) + 1), nCountWeightBreaks - 1);

                Logger.Information("Determine DeficitIndex ({DeficitIndex}) = Math.Min((Math.Max(nStandardWeightNdx[{nStandardWeightNdx}],0) + 1), nCountWeightBreaks[{nCountWeightBreaks}] - 1)",
                    nDeficitIndex,
                    nStandardWeightNdx,
                    nCountWeightBreaks);


                Logger.Information("Iterating through weight breaks from nStandardWeightNdx[{nStandardWeightNdx}] to nDeficitIndex[{nDeficitIndex}]",
                    nStandardWeightNdx,
                    nDeficitIndex);

                for (int nWgtBreakNdx = Math.Max(nStandardWeightNdx, 0); nWgtBreakNdx <= nDeficitIndex; nWgtBreakNdx++)
                {
                    // Stop when the rate for a class is 0. Then we know we have fewer rates than weight breaks.
                    if (weightMatrix[0, nWgtBreakNdx] == 0)
                    {
                        Logger.Warning("Rate for class is 0.  Fewer rates than weight breaks");
                        // Ok, the rates just don't go that high.
                        break;
                    }

                    // Go through the dictionary, using the weight at each class to calculate the line haul cost
                    //logic included to add minimum weight component and deficit weight as needed based on the location in the loop
                    //this process could be optimized ABOVE to limit deficit weight comparison to just the next weight break instead of all.
                    decimal nSummedCosts = calculateLTLLineHaul(ref results, nStandardWeightNdx, nWgtBreakNdx,
                        ref nLineHaulNoDeficit, ref nBestWeightBreakNdx, ref nBestSummedCosts, listBreakPoints,
                        lstClasses, weightMatrix, shipmentClasses, ref LineAllocation);

                    Logger.Information("Calculate Line Haul for nStandardWeightNdx[{nStandardWeightNdx}] and nWgtBreakNdx[{nWgtBreakNdx}] = {SummedCosts}",
                        nStandardWeightNdx,
                        nWgtBreakNdx,
                        nSummedCosts);

                }

                //nWgtAdjWeightBreak
                //updates the LineAllocation.nWgtAdjWeight and LineAllocation.nMinAdjustedLoadWeight using minWeights xref

                Logger.Information("Update Minimum Weight Requirements");

                updateMinimumWeightRequirements(ref results, ratedLoad, InterlineList, carrierTariffsPivots,
                    ref LineAllocation, ref minWeights);

                Logger.Information("Check if LineAllocation.nMinAdjustedLoadWeight ({nMinAdjustedLoadWeight}) > LineAllocation.nActualLoadWeight ({nActualLoadWeight})",
                    LineAllocation.nMinAdjustedLoadWeight,
                    LineAllocation.nActualLoadWeight);

                if (LineAllocation.nMinAdjustedLoadWeight > LineAllocation.nActualLoadWeight)
                {
                    /* ******************  We need to adjust the cost based on the new adjusted weight ***********************************/
                    //save the previous cost
                    LineAllocation.nWgtAdjCost = nBestSummedCosts;
                    //Now use the LineAllocation.nMinAdjustedLoadWeight to get the first available weight class  
                    //because we have not calculated any Deficit Weight at this time (must be called after updateMinimumWeightRequirements)
                    nStandardWeightNdx = GetStandardWeightBreak(listBreakPoints, LineAllocation.nMinAdjustedLoadWeight);
                    LineAllocation.sDeficitClass =
                        DeficitClass(shipmentClasses, weightMatrix, lstClasses, nStandardWeightNdx);
                    //Save the Minimum Weight Adjusted Weight Break
                    LineAllocation.nWgtAdjWeightBreak = (double)listBreakPoints[nStandardWeightNdx];
                    //Reset the values for for break point pricing
                    nBestWeightBreakNdx = -1; // We don't yet have a cheapest weight break.
                    nBestSummedCosts = 0.0M; // Dummy value
                    nLineHaulNoDeficit = 0.0M; // The linehaul portion without deficit rating.

                    // The discount information could be looked up here, but that would require that the structure of the discounts to vary by weight break.
                    // Since the discounts at the current time do not vary by weight break, then they are independent of deficit rating. So do not handle discounts
                    // until a time when the discount structure reflects weight breaks.

                    // In this loop, we will need to know the maximum weight break to use. 
                    // Consider deficiting up only one break point but stop once we reach 
                    //the last break point to avoid an index out of range error. 
                    nDeficitIndex = Math.Min((Math.Max(nStandardWeightNdx, 0) + 1), nCountWeightBreaks - 1);
                    for (int nWgtBreakNdx = Math.Max(nStandardWeightNdx, 0);
                         nWgtBreakNdx <= nDeficitIndex;
                         nWgtBreakNdx++)
                    {
                        // Stop when the rate for a class is 0. Then we know we have fewer rates than weight breaks.
                        if (weightMatrix[0, nWgtBreakNdx] == 0)
                        {
                            // Ok, the rates just don't go that high.
                            break;
                        }

                        // Go through the dictionary, using the weight at each class to calculate the line haul cost
                        //logic included to add minimum weight component and deficit weight as needed based on the location in the loop
                        //this process could be optimized ABOVE to limit deficit weight comparison to just the next weight break instead of all.
                        decimal nSummedCosts = calculateLTLLineHaul(ref results, nStandardWeightNdx, nWgtBreakNdx,
                            ref nLineHaulNoDeficit, ref nBestWeightBreakNdx, ref nBestSummedCosts, listBreakPoints,
                            lstClasses, weightMatrix, shipmentClasses, ref LineAllocation);

                        Logger.Information("Calculate Line Haul for nStandardWeightNdx[{nStandardWeightNdx}] and nWgtBreakNdx[{nWgtBreakNdx}] = {SummedCosts}",
                            nStandardWeightNdx,
                            nWgtBreakNdx,
                            nSummedCosts);

                    }
                }

                // We should have at least one rate. If not, don't use class rating.
                if (nBestSummedCosts == 0.0M)
                {
                    Logger.Warning("No rate found for class rating");
                    operation.Complete();
                    return -1;
                }

                //calculate the minimum weight adjusted cost value            
                LineAllocation.nWgtAdjCost = nBestSummedCosts - LineAllocation.nWgtAdjCost;
                LineAllocation.nBilledLoadWeight = LineAllocation.nActualLoadWeight + LineAllocation.nWgtAdjWeight +
                                                   LineAllocation
                                                       .nBestDeficitWeight; //Total weight used to calculate costs 
                Logger.Information("Calculate Billed Load Weight: {BilledLoadWeight} = ActualTotalWeight ({ActualTotalWeight}) + WgtAdjWeight ({WgtAdjWeight}) + BestDeficitWeight ({BestDeficitWeight})",
                    LineAllocation.nBilledLoadWeight,
                    LineAllocation.nActualLoadWeight,
                    LineAllocation.nWgtAdjWeight,
                    LineAllocation.nBestDeficitWeight);

                // Book revenue to cost mapping
                Dictionary<int, decimal> bookRevTotals = new Dictionary<int, decimal>();
                // Book revenue to percentage mapping.
                Dictionary<int, double> bookRevPercentage = new Dictionary<int, double>();

                // Go through all the classes by book revenue object, to calculate its portion of the freight.
                int bookControl;
                string sClass;
                decimal nTotalLineHaulCosts = 0.0M; // Costs by each linehaul item excluding deficit weight.

                Logger.Information("Iterating through AllocateShipmentClasses");
                foreach (var pair in AllocateShipmentClasses)
                {
                    // This gives us the tuple that represents book revenue object/class.
                    var Key = pair.Key;
                    bookControl = Key.Item1;
                    sClass = Key.Item2;
                    Logger.Information("Calculate Line Haul Cost for BookControl: {BookControl} and Class: {Class}",
                        bookControl,
                        sClass);
                    // Calculate the cost for one class and one book revenue object.
                    double
                        nClassWeight =
                            Math.Ceiling(pair
                                .Value); //total weight for this bookcontrol and Class rounded up to the next highest whole value
                    int nClassNdx = lstClasses.IndexOf(sClass); // determine what index to use in RateMatrix


                    decimal nCost = (decimal)nClassWeight *
                                    weightMatrix[nClassNdx,
                                        Math.Max(nBestWeightBreakNdx,
                                            0)]; //calculate the cost for this class using the summarized weight by bookcontrol and class
                    nCost = Decimal.Round(nCost, 2);

                    Logger.Information("Calculating Allocation {AllocationShipmentClass}: Cost ({AllocationShipmentClassCost}) = ClassWeight({ClassWeight}) * WeightMatrix[{ClassIndex},Math.Max({nBestWeightBreakNdx},0)] ({WeightMatrixValue}) and adding to TotalLineHaulCost({TotalLineHaulCost})",
                        sClass,
                        nCost,
                        nClassWeight,
                        nClassNdx,
                        nBestWeightBreakNdx,
                        weightMatrix[nClassNdx, Math.Max(nBestWeightBreakNdx, 0)],
                        nTotalLineHaulCosts);


                    nTotalLineHaulCosts +=
                        nCost; // Create a total based on each individual book revenue/class combination.


                    // We now know the class, the weight, the weight break for the rate and the matrix itself. Calculate the cost for one item and add it to the book
                    // revenue object's cost. At the end, build another list by percentages.
                    if (!bookRevTotals.ContainsKey(bookControl))
                    {
                        Logger.Information("bookRevTotals does not contain a key for BookControl: {BookControl}, adding with Cost: {Cost}", bookControl, nCost);
                        bookRevTotals.Add(bookControl, nCost);
                    }
                    else
                    {
                        Logger.Information("bookRevTotals contains a key for BookControl: {BookControl}, adding Cost: {Cost} to {CurrentTotal}",
                            bookControl,
                            nCost,
                            bookRevTotals[bookControl]);

                        bookRevTotals[bookControl] += nCost;
                    }
                }

                // Now look at the line-level allocation logic.
                // Now set up allocation dictionary by class (which will be used by line item).
                String sClassUsed;
                int nClassType;
                // Book revenue to cost mapping
                Dictionary<Tuple<string, int>, decimal> lClassCosts = new Dictionary<Tuple<string, int>, decimal>();
                // Book revenue to percentage mapping.
                Dictionary<Tuple<string, int>, double> lClassPercentage = new Dictionary<Tuple<string, int>, double>();


                Logger.Information("Iterating through LineAllocation.WeightMapping");
                foreach (var pair in LineAllocation.WeightMapping)
                {
                    // This gives us the tuple that represents book revenue object/class.
                    var Key = pair.Key;
                    sClassUsed = Key.Item1;
                    nClassType = Key.Item2;

                    // Calculate the cost for one class and one book revenue object.
                    double nClassWeight = pair.Value;
                    int nClassNdx = lstClasses.IndexOf(sClassUsed);
                    decimal nCost = 0;
                    if ((nClassNdx > -1) && (weightMatrix.GetLength(0) > nClassNdx))
                    {
                        //At this point we know that the weightMatrix will contain the ratedvalue for nClassNdx at nBestWeightBreakNdx position.
                        //nBestWeightBreakNdx this property is the bookitemcarrtarequipmatdetID.
                        //weightMatrix[nClassNdx, Math.Max(nBestWeightBreakNdx, 0)] this is the bookitemcarrtarequipmatdetvalue
                        //call the update method of the LineAllocation.NewObject.UpdateRate(sClassUsed, nBestWeightBreakNdx, weightMatrix[nClassNdx, Math.Max(nBestWeightBreakNdx, 0)])
                        //this UpdateRate will look up any bookitemcontrol that has a matching sClassUsed. it will update ratedvalue bookitemcarrtarequipmatdetvalue and bookitemcarrtarequipmatdetID.
                        nCost = (decimal)nClassWeight * weightMatrix[nClassNdx, Math.Max(nBestWeightBreakNdx, 0)];
                        nCost = Decimal.Round(nCost, 2);
                        Logger.Information("Calculating Allocation {AllocationShipmentClass}: Cost ({AllocationShipmentClassCost}) = ClassWeight({ClassWeight}) * WeightMatrix[{ClassIndex},Math.Max({nBestWeightBreakNdx},0)] ({WeightMatrixValue})",
                            sClassUsed,
                            nCost,
                            nClassWeight,
                            nClassNdx,
                            nBestWeightBreakNdx,
                            weightMatrix[nClassNdx, Math.Max(nBestWeightBreakNdx, 0)]);
                    }

                    // We now know the class, the weight, the weight break for the rate and the matrix itself. Calculate the cost for one item and add it to the book
                    // revenue object's cost. At the end, build another list by percentages.
                    if (!lClassCosts.ContainsKey(Key))
                    {
                        lClassCosts.Add(Key, nCost);
                    }
                    else
                    {
                        lClassCosts[Key] += nCost;
                    }
                }

                // Now take this and make a dictionary by percentage only. This is for book rev level allocation.
                // We divide the book's cost by the total cost.
                foreach (KeyValuePair<int, decimal> pair in bookRevTotals)
                {
                    bookControl = pair.Key;
                    decimal nCost = pair.Value;
                    if (nBestWeightBreakNdx >= 0)
                    {
                        if (nTotalLineHaulCosts == 0.0M)
                        {
                            bookRevPercentage.Add(bookControl, 0.0);
                        }
                        else
                        {
                            bookRevPercentage.Add(bookControl, (double)(nCost / nTotalLineHaulCosts));
                        }
                    }
                }


                // Now take this and make a dictionary by percentage only. This is for line level allocation.
                // We divide the class's cost by the total cost.
                foreach (KeyValuePair<Tuple<string, int>, decimal> pair in lClassCosts)
                {
                    var Key = pair.Key;
                    decimal nCost = pair.Value;
                    if (nBestWeightBreakNdx >= 0)
                    {
                        // Handle the case where the costs are 0.
                        if (nTotalLineHaulCosts == 0.0M)
                        {
                            lClassPercentage.Add(Key, 0.0);
                        }
                        else
                        {
                            Logger.Information("Calculating Allocation {AllocationShipmentClass}: Percentage ({AllocationShipmentClassPercentage}) = Cost ({Cost}) / TotalLineHaulCosts ({TotalLineHaulCosts})",
                                Key.Item1,
                                (double)(nCost / nTotalLineHaulCosts),
                                nCost,
                                nTotalLineHaulCosts);

                            lClassPercentage.Add(Key, (double)(nCost / nTotalLineHaulCosts));
                        }
                    }
                }

                // Finally, adjust for minimum charges.
                if (carrierTariffsPivots[0].CarrTarEquipMatMin != null &&
                    nBestSummedCosts < carrierTariffsPivots[0].CarrTarEquipMatMin)
                {
                    Logger.Information("Adjusting for minimum charges: {MinimumCharge} < {BestSummedCosts} then nBestSummedCosts = {CarrTarEquipMatMinValue}",
                        carrierTariffsPivots[0].CarrTarEquipMatMin,
                        nBestSummedCosts,
                        carrierTariffsPivots[0].CarrTarEquipMatMin.Value);

                    nBestSummedCosts = carrierTariffsPivots[0].CarrTarEquipMatMin.Value;
                }

                // Return this as an argument.
                DictAlloc = bookRevPercentage;
                // Return the class percentage mapping
                LineAllocation.PercentMapping = lClassPercentage;
                Logger.Information("LineAllocation.PercentMapping = {@PercentMapping}", LineAllocation.PercentMapping);
            }

            return nBestSummedCosts;
        }

        /// <summary>
        /// Calculate line haul based on unit of measure rates
        /// </summary>
        /// <param name="ratedLoad"></param>
        /// <param name="InterlineList"></param>
        /// <param name="carrierTariffsPivot"></param>
        /// <param name="LineAllocation"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
        /// </remarks>
        public decimal calcClassUnitOfMeasureRatingCharge(Load ratedLoad, List<int> InterlineList, DTO.CarrierTariffsPivot[] carrierTariffsPivot, ref ClassRatingLineAllocation LineAllocation)
        {
            decimal nBestSummedCosts = 0.0M;
            // Line allocation values by unit of measure (for testing we assume pallet code must be added to suport different break points like cases)
            using (var operation = Logger.StartActivity("calcClassUnitOfMeasureRatingCharge for Load: {Load}", ratedLoad))
            {

                // Interline status
                DAL.Utilities.PointType eInterlineStatus = DAL.Utilities.PointType.Direct;
                // Determine what we are looking for, interline or directs.
                if (InterlineList != null && InterlineList.Count > 0)
                {
                    // At least one point is interline.
                    eInterlineStatus = DAL.Utilities.PointType.Interline;
                }

                DTO.CarrierTariffsPivot pivot = carrierTariffsPivot[0];
                decimal[] listBreakPoints = new decimal[10];
                if (pivot.BPPivot == null)
                {
                    listBreakPoints[0] = 0; // set the first one to be 0.
                }
                else
                {
                    //Add all the break points to the beak point list using zero if NULL
                    listBreakPoints[0] = (pivot.BPPivot.BPVal1 ?? 0);
                    listBreakPoints[1] = (pivot.BPPivot.BPVal2 ?? 0);
                    listBreakPoints[2] = (pivot.BPPivot.BPVal3 ?? 0);
                    listBreakPoints[3] = (pivot.BPPivot.BPVal4 ?? 0);
                    listBreakPoints[4] = (pivot.BPPivot.BPVal5 ?? 0);
                    listBreakPoints[5] = (pivot.BPPivot.BPVal6 ?? 0);
                    listBreakPoints[6] = (pivot.BPPivot.BPVal7 ?? 0);
                    listBreakPoints[7] = (pivot.BPPivot.BPVal8 ?? 0);
                    listBreakPoints[8] = (pivot.BPPivot.BPVal9 ?? 0);
                    listBreakPoints[9] = (pivot.BPPivot.BPVal10 ?? 0);

                }

                int nCountWeightBreaks = 10;

                decimal[] RateMatrix = new decimal[10];
                RateMatrix[0] = (pivot.Val1 ?? 0);
                RateMatrix[1] = (pivot.Val2 ?? 0);
                RateMatrix[2] = (pivot.Val3 ?? 0);
                RateMatrix[3] = (pivot.Val4 ?? 0);
                RateMatrix[4] = (pivot.Val5 ?? 0);
                RateMatrix[5] = (pivot.Val6 ?? 0);
                RateMatrix[6] = (pivot.Val7 ?? 0);
                RateMatrix[7] = (pivot.Val8 ?? 0);
                RateMatrix[8] = (pivot.Val9 ?? 0);
                RateMatrix[9] = (pivot.Val10 ?? 0);

                /***************************************************************************
                 * the logic below is for LTL Class Rating based on weight break points.
                 * For Unit of Measure there are no Class codes and the break points have a
                 * type code assigned like pallets.  All Items are included so we can use the
                 * BookTotalxxx value for costing.
                 *  ************************************************************************/
                double dblTotalUnits = 0;
                //Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
                switch (pivot.BPPivot.CarrTarMatBPTarBracketTypeControl)
                {
                    case (int)DAL.Utilities.BracketType.Pallets:
                        {
                            dblTotalUnits = (double)ratedLoad.TotalPL;
                            break;
                        }
                    case (int)DAL.Utilities.BracketType.FlatPallet:
                        {
                            dblTotalUnits = (double)ratedLoad.TotalPL;
                            break;
                        }
                    case (int)DAL.Utilities.BracketType.Quantity:
                        {
                            dblTotalUnits = (double)ratedLoad.TotalCases;
                            break;
                        }
                    case (int)DAL.Utilities.BracketType.Volume:
                        {
                            dblTotalUnits = (double)ratedLoad.TotalCube;
                            break;
                        }
                    case 0: // enum is wrong and does not support DAL.Utilities.BracketType.Lbs:
                        {
                            dblTotalUnits = (double)ratedLoad.TotalWgt;
                            break;
                        }
                    case (int)DAL.Utilities.BracketType.Cwt:
                        {
                            dblTotalUnits = (double)ratedLoad.TotalWgt;
                            if (dblTotalUnits > 100)
                            {
                                dblTotalUnits = (double)Math.Ceiling(dblTotalUnits / 100);
                            }
                            else
                            {
                                dblTotalUnits = 1;
                            }

                            break;
                        }
                    default:
                        {
                            dblTotalUnits = 0;
                            break;
                        }
                }

                //get the breakpoint index
                int nStandardWeightNdx = GetStandardWeightBreakUOM(listBreakPoints, dblTotalUnits);
                if (nStandardWeightNdx < 0 || nStandardWeightNdx > 9)
                {
                    operation.Complete();
                    return -1; //No supported weight breaks
                }

                double nStandardCost = 0;
                if (nStandardWeightNdx >= 0 && nStandardWeightNdx < 10)
                {
                    //Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
                    if (pivot.BPPivot.CarrTarMatBPTarBracketTypeControl == (int)DAL.Utilities.BracketType.FlatPallet)
                    {
                        nStandardCost = (double)RateMatrix[nStandardWeightNdx];
                    }
                    else if (pivot.BPPivot.CarrTarMatBPTarBracketTypeControl == 0)
                    {
                        nStandardCost = (double)RateMatrix[nStandardWeightNdx];
                    }
                    else if (pivot.BPPivot.CarrTarMatBPTarBracketTypeControl == (int)DAL.Utilities.BracketType.Cwt)
                    {
                        nStandardCost = (double)RateMatrix[nStandardWeightNdx];
                    }
                    else
                    {
                        nStandardCost = dblTotalUnits * (double)RateMatrix[nStandardWeightNdx];
                    }

                }

                double nDeficitCost = 0;
                double dblDeficitUnits = 0;
                if (nStandardWeightNdx >= 0 && nStandardWeightNdx < 9)
                {
                    dblDeficitUnits = (double)listBreakPoints[nStandardWeightNdx + 1];
                    //Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
                    if (pivot.BPPivot.CarrTarMatBPTarBracketTypeControl == (int)DAL.Utilities.BracketType.FlatPallet)
                    {
                        nDeficitCost = (double)RateMatrix[nStandardWeightNdx + 1];
                    }
                    else if (pivot.BPPivot.CarrTarMatBPTarBracketTypeControl == 0)
                    {
                        nDeficitCost = (double)RateMatrix[nStandardWeightNdx + 1];
                    }
                    else
                    {
                        nDeficitCost = dblDeficitUnits * (double)RateMatrix[nStandardWeightNdx + 1];
                    }


                }

                int nBestWeightBreakNdx = -1;
          
                nBestSummedCosts = Decimal.Round((decimal)nStandardCost, 2);
                nBestWeightBreakNdx = nStandardWeightNdx;
                LineAllocation.nLineHaul = nBestSummedCosts; // The linehaul portion without deficit rating.
                if (nDeficitCost > 0 && nDeficitCost < nStandardCost)
                {
                    nBestSummedCosts = Decimal.Round((decimal)nDeficitCost, 2);
                    nBestWeightBreakNdx = nStandardWeightNdx + 1;
                    // Save the fields to return back.
                    LineAllocation.nBestDeficitWeight = dblDeficitUnits;
                    LineAllocation.nBestDeficitCost = nBestSummedCosts;
                    if (nBestWeightBreakNdx < 0)
                    {
                        nBestWeightBreakNdx = 0;
                    }
                    else if (nBestWeightBreakNdx > 10)
                    {
                        nBestWeightBreakNdx = 10;
                    }

                    LineAllocation.nBestDeficitWeightBreak = (double)listBreakPoints[nBestWeightBreakNdx];


                }

                LineAllocation.nRatedWeightBreak = (double)listBreakPoints[nBestWeightBreakNdx];

                // We should have at least one rate. If not, don't use UOM rating.
                if (nBestSummedCosts == 0.0M)
                {
                    operation.Complete();
                    return -1;
                }
            }

            return nBestSummedCosts;
        }


        /// <summary>
        /// Return  what would be the deficit class if there is one.
        /// </summary>
        /// <param name="shipmentClasses">List mapping classes to weights.</param>
        /// <returns>
        /// deficit class or empty if none exists.
        /// Modified by RHR 2/12/2015 v-7.0
        ///   Rules for Deficit Class selection were not being applied
        ///   correctly.  Previously the software was selecting the 
        ///   Deficit class using the lowest weight of any class shipped.
        ///   What it should be doing is selecting the lowest rated class where 
        ///   weight is > 0. To do this we need to use the weightMatrix array which lists 
        ///   the classIndex and its associated rate.
        ///   So we can select the class with the lowest cost rate.
        ///   lstClasses provides the index used in the weightMatrix array for each rate.
        ///   nWgtBreakNdx is the Weight Break index being used for the default weight break       
        /// </returns>
        private string DeficitClass(Dictionary<string, double> shipmentClasses, decimal[,] weightMatrix, List<string> lstClasses, int nWgtBreakNdx)
        {
            String sDefClass = String.Empty;
            //Old code removed by RHR 2/10/15 because nClass is referencing weight not class
            //double nLowest = -1.0;
            //foreach (KeyValuePair<string, double> pair in shipmentClasses)
            //{
            //    double nClass = pair.Value;
            //    if (sDefClass.Length == 0 ||
            //        nClass < nLowest)
            //    {
            //        sDefClass = pair.Key;
            //        nLowest = pair.Value;
            //    }
            //}

            //Start new code
            //decimal[,] weightMatrix = new decimal[nCountClasses, nCountWeightBreaks];
            //List<string> lstClasses = new List<string>();
            decimal nLowestRate = -1;
            foreach (KeyValuePair<string, double> pair in shipmentClasses.Where(x => x.Value > 0))
            {
                if (lstClasses.Contains(pair.Key))
                {
                    decimal nRate = weightMatrix[lstClasses.IndexOf(pair.Key), nWgtBreakNdx];
                    if (nLowestRate == -1 || nRate < nLowestRate)
                    {
                        nLowestRate = nRate;
                        sDefClass = pair.Key;
                    }

                }
            }
            return sDefClass;
        }

        /// <summary>
        /// Get weight break points from pivot record.
        /// </summary>
        /// <param name="pivot">Carrier Tariff Pivot record.</param>
        /// <returns></returns>
        List<decimal> GetBreakPoints(DTO.CarrierTariffsPivot pivot)
        {
            List<decimal> bpList = new List<decimal>();
            using (Logger.StartActivity("GetBreakPoints for Pivot {Pivot}", pivot))
            {
                if (pivot.BPPivot == null)
                {
                    bpList.Add(0); // set the first one to be 0.
                }
                else
                {
                    if (pivot.BPPivot.BPVal1.HasValue && pivot.BPPivot.BPVal1 >= 0) // Allow the first one to be 0.
                        bpList.Add(pivot.BPPivot.BPVal1.Value);
                    if (pivot.BPPivot.BPVal2 > 0)
                        bpList.Add(pivot.BPPivot.BPVal2.Value);
                    if (pivot.BPPivot.BPVal3 > 0)
                        bpList.Add(pivot.BPPivot.BPVal3.Value);
                    if (pivot.BPPivot.BPVal4 > 0)
                        bpList.Add(pivot.BPPivot.BPVal4.Value);
                    if (pivot.BPPivot.BPVal5 > 0)
                        bpList.Add(pivot.BPPivot.BPVal5.Value);
                    if (pivot.BPPivot.BPVal6 > 0)
                        bpList.Add(pivot.BPPivot.BPVal6.Value);
                    if (pivot.BPPivot.BPVal7 > 0)
                        bpList.Add(pivot.BPPivot.BPVal7.Value);
                    if (pivot.BPPivot.BPVal8 > 0)
                        bpList.Add(pivot.BPPivot.BPVal8.Value);
                    if (pivot.BPPivot.BPVal9 > 0)
                        bpList.Add(pivot.BPPivot.BPVal9.Value);
                    if (pivot.BPPivot.BPVal10 > 0)
                        bpList.Add(pivot.BPPivot.BPVal10.Value);
                }
            }

            return bpList;
        }
        #region ClassRatingComments
        // Special cases such as the following need to be considered at some point, but based on the data that's currently available, they may not yet apply.

        // Determine the minimum and maximum weight breaks we are allowed to use. Fot the time being, we say we cannot deficit backwards and
        // can deficit one weight break up. This would be a separate method, which has no external inputs at this point. If we want to change this
        // behavior, then the data will have to stored someplace and we need to be able to access it. I will not worry about it currently.
        // But the information would be comprised of the following data: A TL weight break, flags for deficiting up past weight break, 
        // deficiting down past weight break, number of deficits up, and number of deficits down. Since we don't have this information currently,
        // the logic is simplly to deficit one weight break up and stop.
        // Loop through each allowable weight break:
        // Determine the weight break type. Either standard or deficit. 
        // If deficit, determine the deficit weight, and the deficit class (the lowest numerical class).
        // For each class, multiply the weight times the cost for that class at the current weight break.
        // If less than the minimum, use this instead. Also the discount must have a provision for a minimum separate from weight-break
        // based. If this is the case, then we can discount tariff minimum charges. Otherwise, it's not flexible for all scenarios.
        // Do the deficit weight separately at the lowest class.
        // Round the cost for the class to two decimal places.
        // if the cost has Notyet been determined oris less than the best cost, save the following items:
        // Weight break used for rating.
        // Linehl costs.
        // Discount amount.
        // Deficit weight if any
        // Deficit class if any
        // Return back the various pieces of information. Create a Class Rating return object which has this., including discount.

        #endregion


        /// <summary>
        /// Get the non-deficit weight break for the total weight.
        /// </summary>
        /// <param name="WeightBreaks">Number of weight breaks in the BP pivots</param>
        /// <param name="totalWeight">Total weight summed by class</param>
        /// <returns>
        /// Index of standard weight break 
        /// Modified by RHR 1/28/14 l-1 if weight is less than or equal to 0 or lowest weight break
        /// 
        /// </returns>
        int GetStandardWeightBreak(List<decimal> WeightBreaks, double totalWeight)
        {
            int nStandardNdx = -1;
            using (Logger.StartActivity(
                       "GetStandardWeightBreak for WeightBreaks {WeightBreaks} and TotalWeight {TotalWeight}",
                       WeightBreaks, totalWeight))
            {

                if (totalWeight >= 0)
                {
                    nStandardNdx = 0;
                }

                int nCount = 0;
                foreach (decimal nWeight in WeightBreaks)
                {
                    // Take the last weight break
                    if (totalWeight >= (double)nWeight)
                    {
                        nStandardNdx = nCount;
                    }

                    nCount++;
                }
            }

            return nStandardNdx;
        }

        int GetStandardWeightBreakUOM(decimal[] BreakPoints, double totalUnits)
        {
            int nStandardNdx = -1; // Nothing matches exactly, (lower than first weight break)
            using (Logger.StartActivity("GetStandardWeightBreakUOM by units {TotalUnits} with BreakPoints: {BreakPoints}", totalUnits, BreakPoints))
            {
                int nCount = 0;
                foreach (decimal nWeight in BreakPoints)
                {
                    // Take the last breakpoint that is > 0 as long as totalUnits is >= than breakpoint
                    if (nWeight > 0 && totalUnits >= (double)nWeight)
                    {
                        nStandardNdx = nCount;
                    }

                    nCount++;
                }
            }
            return nStandardNdx;
        }

        /// <summary>
        /// Validate that every class can map to a class in the tariff. If this is not the case, we cannot rate using this tariff.
        /// </summary>
        /// <param name="shipmentClasses">Dictionary mapping classes to weights</param>
        /// <param name="lstClasses">List of classes</param>
        /// <returns>true if valid, false if a class could not be mapped to a tariff rate record</returns>
        private Boolean ValidateClasses(Dictionary<string, double> shipmentClasses, List<string> lstClasses)
        {
            foreach (KeyValuePair<string, double> pair in shipmentClasses)
            {
                if (!lstClasses.Contains(pair.Key.Trim()))
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Retrieve rates from the pivot records and store into matrix.
        /// </summary>
        /// <param name="pivot">Tariff Pivot record</param>
        /// <param name="weightMatrix">Matrix of rates by weight break and class.</param>
        /// <param name="nCountWeightBreaks">Number of weight breaks in BP pivots.</param>
        /// <param name="nClassNdx">Class index</param>
        private void GetRatesForClassRecord(DTO.CarrierTariffsPivot pivot, decimal[,] weightMatrix, int nCountWeightBreaks, int nClassNdx)
        {
            // Populate each weight break from the rates. Right now, assume the break points are in sync with the rates.
            // TODO: Verify that we don't go over the number of break points and issue and error if we do. This is a data error and 
            // should be reported.

            if (pivot.Val1 > 0)
                SetRateValue(weightMatrix, 0, pivot.Val1.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val2 > 0)
                SetRateValue(weightMatrix, 1, pivot.Val2.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val3 > 0)
                SetRateValue(weightMatrix, 2, pivot.Val3.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val4 > 0)
                SetRateValue(weightMatrix, 3, pivot.Val4.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val5 > 0)
                SetRateValue(weightMatrix, 4, pivot.Val5.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val6 > 0)
                SetRateValue(weightMatrix, 5, pivot.Val6.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val7 > 0)
                SetRateValue(weightMatrix, 6, pivot.Val7.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val8 > 0)
                SetRateValue(weightMatrix, 7, pivot.Val8.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val9 > 0)
                SetRateValue(weightMatrix, 8, pivot.Val9.Value, nCountWeightBreaks, nClassNdx);
            if (pivot.Val10 > 0)
                SetRateValue(weightMatrix, 9, pivot.Val10.Value, nCountWeightBreaks, nClassNdx);
        }

        /// <summary>
        /// Set an element of the rate matrix.
        /// </summary>
        /// <param name="weightMatrix">Matrix of rates by class and weight break.</param>
        /// <param name="nWeightIndex">Weight index</param>
        /// <param name="rateValue">Rate value to set</param>
        /// <param name="nCountWeightBreaks">Number of weight breaks</param>
        /// <param name="nClassNdx">Class index</param>
        private void SetRateValue(decimal[,] weightMatrix, int nWeightIndex, decimal rateValue, int nCountWeightBreaks, int nClassNdx)
        {
            if (nWeightIndex < nCountWeightBreaks)
            {
                weightMatrix[nClassNdx, nWeightIndex] = rateValue;
            }
            else
            {
                // TODO SRL Logthiserror;
                Logger.Error("SetRateValue: Weight Index {WeightIndex} is greater than the number of weight breaks {CountWeightBreaks}", nWeightIndex, nCountWeightBreaks);
            }
        }
        #endregion
    }
    public class ClassRatingLineAllocation
    {
        public Dictionary<Tuple<string, int>, string> FAKMapping;
        public Dictionary<Tuple<string, int>, double> WeightMapping;
        public Dictionary<Tuple<string, int>, double> PercentMapping;
        public String sDeficitClass = ""; //Class used for deficit and minimum weight adjustment calculations
        public decimal nLineHaul = 0; //Line Haul Cost not including deficit rating and before minimum charge adjustment
        public decimal nBestDeficitCost = 0; //Cost for Deficit Weight Adjustment 
        public double nBestDeficitWeight = 0;//Difference between nBilledLoadWeight and nMinAdjustedLoadWeight
        public double nBestDeficitWeightBreak = 0; //Break Point Value used to calculate Deficit Weight Adjustment Cost
        public double nRatedWeightBreak = 0;//Break Point Value used to calculate all costs
        public decimal nWgtAdjCost = 0; //Cost for Minimum Weight Adjustment 
        public double nWgtAdjWeight = 0; //Difference between nActualLoadWeight and nMinAdjustedLoadWeight
        public double nWgtAdjWeightBreak = 0;//Break Point Value used to calculate Minimum Weight Adjustment Cost
        public double nActualLoadWeight = 0; //Actual total weight of the load
        public double nBilledLoadWeight = 0; //Total weight used to calculate costs nActualLoadWeight + nWgtAdjWeight + nBestDeficitWeight
        public double nMinAdjustedLoadWeight = 0; //Total weight used to calculate costs before Deficit Weight is applied nActualLoadWeight + nWgtAdjWeight
        /* ************************************* Weight Variance Adjustments ********************
         * An Adjustment may be needed for cost calculations when rounding adjustments are required 
         * to meet the total weight of a shipment when items with different class codes are mixed together.  
         * The weight of the heaviest class will be modified match the total weight if a variance is detected;
         * so if we have 5 lbs over after rounding The system will subtract 5 lbs from the heaviest class 
         * then calculate the cost for that class.  A reference to the weight used to calculate the cost 
         * for each class will be saved with the load so users will be able to understand how the total 
         * cost was calculated.  Each BookRevenue record will store the load totals for these types of adjustments.
         * **************************************************************************************/
        public double nSummedClassWeight; //the sum total of all class code weights after rounding  
        public double nWgtRoundingVariance; //the difference between nSummedClassWeight and nActualLoadWeight
        public string sHeaviestClass;// the heaviest class used to adjust for rounding variance
        public double nAdjustedHeaviestClassWeight; //Weight adjusted rounding variance for heaviest class = nClassWeight + LineAllocation.nWgtRoundingVariance
        public double nAcutalHeaviestClassWeight; //Weight rounded to whole number for actual weight for heaviest class 

        public override string ToString()
        {
            return
                $@"ClassRatingAllocation: LineHaul: {this.nLineHaul}, BestDeficitWeight: {nBestDeficitWeight} BestDeficitCost: {nBestDeficitCost}, BestDeficitWeightBreak: {nBestDeficitWeightBreak}, RatedWeightBreak: {nRatedWeightBreak}, WgtAdjCost: {nWgtAdjCost} WgtAdjWeight: {nWgtAdjWeight}, WgtAdjWeightBreak: {nWgtAdjWeightBreak}
                ActualLoadWeight: {nActualLoadWeight}, BilledLoadWeight: {nBilledLoadWeight},
                MinAdjustedLoadWeight: {nMinAdjustedLoadWeight}, SummedClassWeight: {nSummedClassWeight},
                WgtRoundingVariance: {nWgtRoundingVariance}, HeaviestClass: {sHeaviestClass},
                AdjustedHeaviestClassWeight: {nAdjustedHeaviestClassWeight}, AcutalHeaviestClassWeight: {nAcutalHeaviestClassWeight};";
        }

        public ClassRatingLineAllocation()
        {
            Init();
        }
        public void Init()
        {
            FAKMapping = new Dictionary<Tuple<string, int>, string>();
            WeightMapping = new Dictionary<Tuple<string, int>, double>();
            PercentMapping = new Dictionary<Tuple<string, int>, double>();
            sDeficitClass = String.Empty;
            nLineHaul = 0.0M;
            nBestDeficitCost = 0.0M;
            nBestDeficitWeight = 0.0;
            nBestDeficitWeightBreak = 0.0;
            nRatedWeightBreak = 0.0;
            nWgtAdjCost = 0.0M;
            nWgtAdjWeight = 0.0;
            nWgtAdjWeightBreak = 0.0;
            nActualLoadWeight = 0.0;
            nBilledLoadWeight = 0.0;
            nMinAdjustedLoadWeight = 0.0;
        }
        public ClassRatingLineAllocation Copy()
        {
            ClassRatingLineAllocation copy = new ClassRatingLineAllocation();
            copy.FAKMapping = new Dictionary<Tuple<string, int>, string>(this.FAKMapping);
            copy.WeightMapping = new Dictionary<Tuple<string, int>, double>(this.WeightMapping);
            copy.PercentMapping = new Dictionary<Tuple<string, int>, double>(this.PercentMapping);
            copy.sDeficitClass = this.sDeficitClass;
            copy.nLineHaul = this.nLineHaul;
            copy.nBestDeficitCost = this.nBestDeficitCost;
            copy.nBestDeficitWeight = this.nBestDeficitWeight;
            copy.nBestDeficitWeightBreak = this.nBestDeficitWeightBreak;
            copy.nRatedWeightBreak = this.nRatedWeightBreak;
            copy.nWgtAdjCost = this.nWgtAdjCost;
            copy.nWgtAdjWeight = this.nWgtAdjWeight;
            copy.nWgtAdjWeightBreak = this.nWgtAdjWeightBreak;
            copy.nActualLoadWeight = this.nActualLoadWeight;
            copy.nBilledLoadWeight = this.nBilledLoadWeight;
            copy.nMinAdjustedLoadWeight = this.nMinAdjustedLoadWeight;
            copy.nSummedClassWeight = this.nSummedClassWeight;
            copy.nWgtRoundingVariance = this.nWgtRoundingVariance;
            copy.sHeaviestClass = this.sHeaviestClass;
            copy.nAdjustedHeaviestClassWeight = this.nAdjustedHeaviestClassWeight;
            copy.nAcutalHeaviestClassWeight = this.nAcutalHeaviestClassWeight;
            return copy;
        }
    }
}
