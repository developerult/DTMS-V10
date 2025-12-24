using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SerilogTracing;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace NGL.FM.CarTar
{
    /// <summary>
    /// BookRev Class
    /// </summary>
    public class CarrierTariffsPivotProcessor : TarBaseClass
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarrierTariffsPivotProcessor(DAL.WCFParameters oParameters)
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
            Logger = Logger.ForContext<CarrierTariffsPivotProcessor>();
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrierTariffsPivotProcessor";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }


        /// <summary>
        /// This property contains the results read from the GetCarrierTariffsPivot() method.
        /// It is persisted here so that we can perform get next carrier operations.
        /// </summary>
        private DTO.CarrierTariffsPivot[] _carrierTariffsPivots = null;
        public DTO.CarrierTariffsPivot[] CarrierTariffsPivots
        {
            get { return _carrierTariffsPivots; }
            set { _carrierTariffsPivots = value; }
        }

        private List<DTO.CarrierTariffsPivot> _carrierTariffsPivotsList = null;
        public List<DTO.CarrierTariffsPivot> CarrierTariffsPivotsList
        {
            get { return _carrierTariffsPivotsList; }
            set { _carrierTariffsPivotsList = value; }
        }

        #endregion

        #region " Methods"

        /// <summary>
        /// This method only works when each item has the same class code so it uses 
        /// the same matrix control number.  When multiple LTL class codes exists we 
        /// need to use a different method (To Be Defined).  
        /// </summary>
        /// <param name="CarrTarEquipMatControl"></param>
        /// <returns></returns>
        public DTO.CarrierTariffsPivot getCarrierTariffsPivot(ref DTO.CarrierCostResults results, int CarrTarEquipMatControl)
        {
            DTO.CarrierTariffsPivot carrierTariffsPivot = null;
            using (var operation =
                   Logger.StartActivity(
                       "getCarrierTariffsPivot(results: {results}, CarrierTarEquipMatControl: {CarrTarEquipMatControl})",
                       results,
                       CarrTarEquipMatControl))
            {
                if (results == null)
                {
                    results = new DTO.CarrierCostResults();
                }


                try
                {
                    results.AddLog("Read specific rate from tariff");
                    carrierTariffsPivot = NGLCarrTarContractData.GetCarrierTariffPivot(CarrTarEquipMatControl);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    if (sqlEx.Message != "E_NoData") // don't throw the exception if no data was found.
                    {
                        results.AddMessage(DTO.CarrierCostResults.MessageEnum.M_SQLFaultCannotReadTariff,
                            sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details);
                        // "E_SQLExceptionMSG", E_UnExpectedMSG
                        string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                            sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                        SaveSysError(errMsg, sourcePath("getCarrierTariffsPivot"));
                        results.AddLog("System error log updated.");
                    }
                }
                catch (Exception ex)
                {
                    results.AddLog(string.Format("Unexpected Error: {0} ", ex.Message));
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    throw;
                }
            }

            return carrierTariffsPivot;
        }

        /// <summary>
        /// Read the Carrier Tariffs Pivots data.
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
        /// <returns></returns>
        public DTO.CarrierTariffsPivot[] getCarrierTariffsPivot(ref DTO.CarrierCostResults results, int bookControl,
                                                                int carrierControl = 0,
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
                                                                int pagesize = 1000)
        {
            List<DTO.CarrierTariffsPivot> finalResult = new List<DTO.CarrierTariffsPivot>();

            using (Logger.StartActivity(
                       "getCarrierTariffsPivot(results: {results}, bookControl: {bookControl}, carrierControl: {carrierControl}, prefered: {prefered}, noLateDelivery: {noLateDelivery}, validated: {validated}, optimizeByCapacity: {optimizeByCapacity}, modeTypeControl: {modeTypeControl}, tempType: {tempType}, tariffTypeControl: {tariffTypeControl}, carrTarEquipMatClass: {carrTarEquipMatClass}, carrTarEquipMatClassTypeControl: {carrTarEquipMatClassTypeControl}, carrTarEquipMatTarRateTypeControl: {carrTarEquipMatTarRateTypeControl}, agentControl: {agentControl}, page: {page}, pagesize: {pagesize})",
                       results, bookControl, carrierControl, prefered, noLateDelivery, validated, optimizeByCapacity,
                       modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass,
                       carrTarEquipMatClassTypeControl, carrTarEquipMatTarRateTypeControl, agentControl, page,
                       pagesize))
            {

                if (results == null)
                {
                    results = new DTO.CarrierCostResults();
                }

                try
                {
                    results.AddLog("Read available tariffs.");
                    Logger.Information("Read available tariffs.");
                    CarrierTariffsPivots = NGLCarrTarContractData.GetCarrierTariffsPivot(bookControl, carrierControl,
                        prefered,
                        noLateDelivery, validated, optimizeByCapacity, modeTypeControl, tempType, tariffTypeControl,
                        carrTarEquipMatClass, carrTarEquipMatClassTypeControl, carrTarEquipMatTarRateTypeControl,
                        agentControl, page, pagesize);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    Logger.Error(sqlEx, "FaultException in getCarrierTariffsPivot.");

                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "General in getCarrierTariffsPivot.");
                    results.AddLog(string.Format("Unexpected Error: {0} ", ex.Message));
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                }

                results.AddLog("Select the most precise tariffs if duplicate rates exist for similar locations.");

                Logger.Information("returnMostPreciseTariffs(CarrierTariffsPivots) {@0}", CarrierTariffsPivots);
                finalResult = returnMostPreciseTariffs(CarrierTariffsPivots);
                Logger.Information("finalResult.ToArray(): {@0}", finalResult);
            }

            var resultToArray = finalResult.ToArray();
            return resultToArray;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <param name="compControl"></param>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <param name="zip"></param>
        /// <param name="modeTypeControl"></param>
        /// <param name="totalWeight"></param>
        /// <param name="totalCases"></param>
        /// <param name="totalPallets"></param>
        /// <param name="totalCubes"></param>
        /// <param name="bookDateLoad"></param>
        /// <param name="inbound"></param>
        /// <param name="BookLoadCom"></param>
        /// <param name="LaneControl"></param>
        /// <param name="carrierControl"></param>
        /// <param name="prefered"></param>
        /// <param name="noLateDelivery"></param>
        /// <param name="validated"></param>
        /// <param name="optimizeByCapacity"></param>
        /// <param name="tempType"></param>
        /// <param name="tariffTypeControl"></param>
        /// <param name="carrTarEquipMatClass"></param>
        /// <param name="carrTarEquipMatClassTypeControl"></param>
        /// <param name="carrTarEquipMatTarRateTypeControl"></param>
        /// <param name="agentControl"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="OrigZip"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        /// </remarks>
        public DTO.CarrierTariffsPivot[] getCarrierTariffsPivot(ref DTO.CarrierCostResults results,
                                                               int compControl,
                                                                string country,
                                                                string state,
                                                                string city,
                                                                string zip,
                                                                int modeTypeControl,
                                                                double totalWeight,
                                                                int totalCases,
                                                                double totalPallets,
                                                                int totalCubes,
                                                                DateTime bookDateLoad,
                                                                bool inbound,
                                                                string BookLoadCom,
                                                                int LaneControl,
                                                               int carrierControl = 0,
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
                                                               string OrigZip = "")
        {
            if (results == null) { results = new DTO.CarrierCostResults(); }

            DTO.CarrierTariffsPivot[] finalResult = null;

            using (var operation = Logger.StartActivity("getCarrierTariffsPivot(CarrierCostResults: {CarrierCostResults}", results))
            {
                try
                {
                    results.AddLog("Read available tariffs.");
                    CarrierTariffsPivots = NGLCarrTarContractData.GetCarrierTariffsPivot(compControl,
                        country,
                        state,
                        city,
                        zip,
                        modeTypeControl,
                        totalWeight,
                        totalCases,
                        totalPallets,
                        totalCubes,
                        bookDateLoad,
                        inbound,
                        BookLoadCom,
                        LaneControl,
                        carrierControl,
                        prefered,
                        noLateDelivery,
                        validated,
                        optimizeByCapacity,
                        modeTypeControl,
                        tempType,
                        tariffTypeControl,
                        carrTarEquipMatClass,
                        carrTarEquipMatClassTypeControl,
                        carrTarEquipMatTarRateTypeControl,
                        agentControl,
                        page,
                        pagesize,
                        OrigZip);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    operation.Complete();
                    Logger.Error(sqlEx, "FaultException in getCarrierTariffsPivot");

                }
                catch (Exception ex)
                {
                    operation.Complete(exception: ex);
                    Logger.Error(ex, "General in getCarrierTariffsPivot");
                    results.AddLog(string.Format("Unexpected Error: {0} ", ex.Message));
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                }

                results.AddLog("Select the most precise tariffs if duplicate rates exist for similar locations.");
                var tempResult = returnMostPreciseTariffs(CarrierTariffsPivots);
                
                Logger.Information("finalResult.ToArray(): {@0}", tempResult);

                finalResult = tempResult.ToArray();
                //results = finalResult;
            }

            return finalResult.ToArray();

        }

        /// <summary>
        /// Returns an array of Pivot records based on address Precision logic
        /// </summary>
        /// <param name="results"></param>
        /// <param name="BookControl"></param>
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
        /// </returns>
        public DTO.CarrierTariffsPivot[] getCarrierTariffsPivotWithPrecision(ref DTO.CarrierCostResults results,
                                                                        int BookControl,
                                                                        int carrierControl = 0,
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
                                                                        int pagesize = 1000)
        {
            if (results == null) { results = new DTO.CarrierCostResults(); }

            using (Logger.StartActivity("getCarrierTariffsPivotWithPrecision, with noLateDelivery: {0}, validated: {1}, optimizeByCapacity: {2}, bookControl: {3}, carrierControl: {4}, prefered: {5}, modeTypeControl: {6}, tempType: {7}, tariffTypeControl: {8}, carrTarEquipMatClass: {9}, carrTarEquipMatClassTypeControl: {10}, carrTarEquipMatTarRateTypeControl: {11}, agentControl: {12}, page: {13}, pagesize: {14}",
                noLateDelivery, validated, optimizeByCapacity, BookControl, carrierControl, prefered, modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass, carrTarEquipMatClassTypeControl, carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize))
            {
                return getCarrierTariffsPivot(ref results, BookControl, carrierControl, prefered, noLateDelivery,
                    validated, optimizeByCapacity, modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass,
                    carrTarEquipMatClassTypeControl,
                    carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize);
            }
        }

        public List<DTO.CarrierTariffsPivot> returnMostPreciseTariffs(DTO.CarrierTariffsPivot[] pivots)
        {

            // iterate through and keep the first record that matches the key fields.
            List<DTO.CarrierTariffsPivot> precPivotsUnique = new List<DTO.CarrierTariffsPivot>(); // Find only the most precise record.
            using (Logger.StartActivity("returnMostPreciseTariffs(pivots: {@pivots})", pivots))
            {
                if (pivots == null || pivots.Count() < 1)
                {
                    return precPivotsUnique;
                }

                bool bFirst = true;
                int nPrevCarrControl = 0;
                int nPrevCarrTarEquipCarrierEquipControl = 0;
                int nPrevCarrTarTariffModeTypeControl = 0;
                int nPrevCarrTarTempType = 0;
                int nPrevCarrTarTariffTypeControl = 0;
                int nPrevCarrTarEquipMatClassTypeControl = 0;
                int nPrevCarrTarEquipMatRateTypeControl = 0;
                string nPrevCarrTarEquipMatClass = String.Empty;

                foreach (DTO.CarrierTariffsPivot dp in pivots)
                {
                    if (bFirst == true ||
                        PivotDifference(dp, nPrevCarrControl, nPrevCarrTarTariffModeTypeControl, nPrevCarrTarTempType,
                            nPrevCarrTarTariffTypeControl,
                            nPrevCarrTarEquipMatClassTypeControl, nPrevCarrTarEquipMatRateTypeControl,
                            nPrevCarrTarEquipMatClass, nPrevCarrTarEquipCarrierEquipControl))
                    {

                        precPivotsUnique.Add(dp);
                        nPrevCarrControl = dp.CarrierControl;
                        nPrevCarrTarEquipCarrierEquipControl = dp.CarrTarEquipCarrierEquipControl;
                        nPrevCarrTarTariffModeTypeControl = dp.CarrTarTariffModeTypeControl;
                        nPrevCarrTarTempType = dp.CarrTarTempType;
                        nPrevCarrTarTariffTypeControl = dp.CarrTarTariffTypeControl;
                        nPrevCarrTarEquipMatClassTypeControl = dp.CarrTarEquipMatClassTypeControl;
                        nPrevCarrTarEquipMatRateTypeControl = dp.CarrTarEquipMatTarRateTypeControl;
                        nPrevCarrTarEquipMatClass = dp.CarrTarEquipMatClass;
                        bFirst = false;
                        Logger.Information("returnMostPreciseTariffs added record to precPivotsUnique.");
                    }
                    // Else, all key fields are the same, so we skip any records which are geographically less precise.
                }

                Logger.Information("returnMostPreciseTariffs precPivotsUnique: {@0}", precPivotsUnique);
            }

            return precPivotsUnique;

        }

        /// <summary>
        /// Determine if the record is for the same tariff or a new one.
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="CarrControl"></param>
        /// <param name="Mode"></param>
        /// <param name="Temp"></param>
        /// <param name="TariffType"></param>
        /// <param name="ClassType"></param>
        /// <param name="RateType"></param>
        /// <param name="Class"></param>
        /// <param name="Equip"></param>
        /// <returns></returns>
        private bool PivotDifference(DTO.CarrierTariffsPivot dp, int CarrControl, int Mode, int Temp, int TariffType, int ClassType, int RateType, string Class, int Equip)
        {
            /****************************************************************
             * Modified by RHR 10/13/13  We need to add the Equipment Type to this filter
             * CarrTarEquipCarrierEquipControl
             * *****************************************************************/
            if (CarrControl != dp.CarrierControl ||
                Equip != dp.CarrTarEquipCarrierEquipControl ||
                Mode != dp.CarrTarTariffModeTypeControl ||
                Temp != dp.CarrTarTempType ||
                TariffType != dp.CarrTarTariffTypeControl ||
                ClassType != dp.CarrTarEquipMatClassTypeControl ||
                RateType != dp.CarrTarEquipMatTarRateTypeControl ||
                Class != dp.CarrTarEquipMatClass)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determine if the record is for the same tariff or a new one, allowing multiple pivot records to handle class rating.
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="CarrControl"></param>
        /// <param name="Mode"></param>
        /// <param name="Temp"></param>
        /// <param name="TariffType"></param>
        /// <param name="ClassType"></param>
        /// <param name="RateType"></param>
        /// <param name="Equip"></param>
        /// <returns></returns>
        private bool PivotDifferenceClass(DTO.CarrierTariffsPivot dp, int CarrControl, int Mode, int Temp, int TariffType, int ClassType, int RateType, int Equip)
        {
            /****************************************************************
            * Modified by RHR 10/13/13  We need to add the Equipment Type to this filter
            * CarrTarEquipCarrierEquipControl
            * *****************************************************************/
            if (CarrControl != dp.CarrierControl ||
                Equip != dp.CarrTarEquipCarrierEquipControl ||
                Mode != dp.CarrTarTariffModeTypeControl ||
                Temp != dp.CarrTarTempType ||
                TariffType != dp.CarrTarTariffTypeControl ||
                ClassType != dp.CarrTarEquipMatClassTypeControl ||
                RateType != dp.CarrTarEquipMatTarRateTypeControl)
            {
                Logger.Information("PivotDifferenceclass returning true because CarrControl: {0} != dp.CarrierControl: {1} || Equip: {2} != dp.CarrTarEquipCarrierEquipControl: {3} || Mode: {4} != dp.CarrTarTariffModeTypeControl: {5} || Temp: {6} != dp.CarrTarTempType: {7} || TariffType: {8} != dp.CarrTarTariffTypeControl: {9} || ClassType: {10} != dp.CarrTarEquipMatClassTypeControl: {11} || RateType: {12} != dp.CarrTarEquipMatTarRateTypeControl: {13}",
                    CarrControl, dp.CarrierControl, Equip, dp.CarrTarEquipCarrierEquipControl, Mode, dp.CarrTarTariffModeTypeControl, Temp, dp.CarrTarTempType, TariffType, dp.CarrTarTariffTypeControl, ClassType, dp.CarrTarEquipMatClassTypeControl, RateType, dp.CarrTarEquipMatTarRateTypeControl);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the most precise carrier tariffs pivot data for the next carrier control after the specified previous carrier control.
        /// </summary>
        /// <param name="bookRevenue"></param>
        /// <param name="previousCarrierControl"></param>
        /// <param name="carrierTariffPivots"></param>
        /// <returns></returns>
        public DTO.CarrierTariffsPivot getNextCarrierTariffsPivotWithPrecision(
            Load load,
            int previousCarrierControl, DTO.CarrierTariffsPivot[] carrierTariffPivots)
        {
            DTO.CarrierTariffsPivot carrierTariffsPivot = null;
            Logger.Information("getNextCarrierTariffsPivotWithPrecision, with previousCarrierControl: {0}", previousCarrierControl);
            // Selects most precise pivot table for the next carrierControl after the specified previousCarrierControl.
            if (previousCarrierControl == -1)
            {
                if (carrierTariffPivots.Length > 0)
                {
                    Logger.Information("getNextCarrierTariffsPivotWithPrecision, with previousCarrierControl: {0}, carrierTariffPivots.Length: {1}", previousCarrierControl, carrierTariffPivots.Length);
                    // just return the first one, we need to all precision logic to this.
                    carrierTariffsPivot = carrierTariffPivots[0];
                }
            }
            else
            {
                // Find the next carrier control
                bool previousFound = false;
                foreach (DTO.CarrierTariffsPivot currentPivot in carrierTariffPivots)
                {
                    if (currentPivot.CarrierControl == previousCarrierControl)
                    {
                        previousFound = true;
                    }
                    else if (previousFound == true)
                    {
                        // just return the first one, we need to add precision logic within each carrier.
                        carrierTariffsPivot = currentPivot;
                        break;
                    }
                }
            }

            return carrierTariffsPivot;
        }

        /// <summary>
        /// Get the most precise carrier tariffs pivot data for the next carrier control after the 
        /// specified previous carrier control.  This logic assumes that the CarrierTariffsPivot array 
        /// is sorted by carrier, tariff, mode,temp, tariff type, class type, rate type,  equipment and class
        /// NOTE:  it does not allow differnt class types to be mixed with the same tariff (which is expected)
        /// </summary>
        /// <param name="bookRevenue"></param>
        /// <param name="previousCarrierControl"></param>
        /// <param name="carrierTariffPivots"></param>
        /// <returns></returns>
        public DTO.CarrierTariffsPivot[] getNextCarrierTariffsPivotWithPrecision(
            ref int nIndex, DTO.CarrierTariffsPivot[] carrierTariffPivots)
        {
            using (Logger.StartActivity(
                       "getNextCarrierTariffsPivotWithPrecision(nIndex: {nIndex}, carrierTariffPivots: {@carrierTariffPivots})",
                       nIndex, carrierTariffPivots))
            {

                // Handles empty as well as end of list.
                if (carrierTariffPivots == null || nIndex >= carrierTariffPivots.Length)
                {
                    return null;
                }

                Logger.Information(
                    "check if carrierTariffPivots[nindex].CarrTarEquipMatTarRateTypeControl ({CarrTarEquipMatTarRateTypeControl}) == (int)Ngl.FreightMaster.Data.Utilities.TariffRateType.ClassRate: {ClassRate}",
                    carrierTariffPivots[nIndex].CarrTarEquipMatTarRateTypeControl,
                    (int)Ngl.FreightMaster.Data.Utilities.TariffRateType.ClassRate);
                // Return more than one record for class rating.
                if (carrierTariffPivots[nIndex].CarrTarEquipMatTarRateTypeControl ==
                    (int)Ngl.FreightMaster.Data.Utilities.TariffRateType.ClassRate)
                {
                    Logger.Information(", with nIndex: {0}, carrierTariffPivots.Length: {1}", nIndex,
                        carrierTariffPivots.Length);
                    // We're still not done. What we have done so far is to sort all the pivot records so that we can determine what is less precise, therefore duplicate.
                    // Now iterate through and keep the first record that matches the key fields.
                    List<DTO.CarrierTariffsPivot>
                        precPivotsForClassRates =
                            new List<DTO.CarrierTariffsPivot>(); // Find only the most precise record.

                    bool bFirst = true;
                    int nPrevCarrControl = 0;
                    int nPrevCarrTarEquipCarrierEquipControl = 0;
                    int nPrevCarrTarTariffModeTypeControl = 0;
                    int nPrevCarrTarTempType = 0;
                    int nPrevCarrTarTariffTypeControl = 0;
                    int nPrevCarrTarEquipMatClassTypeControl = 0;
                    int nPrevCarrTarEquipMatRateTypeControl = 0;
                    string nPrevCarrTarEquipMatClass = String.Empty;
                    /****************************************************************
                     * Modified by RHR 10/13/13  We need to add the Equipment Type to this filter
                     * CarrTarEquipCarrierEquipControl
                     * *****************************************************************/
                    // Build up a list of those records which match all key fields except for class.
                    do
                    {
                        precPivotsForClassRates.Add(carrierTariffPivots[nIndex]);
                        if (bFirst == true)
                        {

                            // Only copy common fields once.
                            bFirst = false;
                            nPrevCarrControl = carrierTariffPivots[nIndex].CarrierControl;
                            nPrevCarrTarEquipCarrierEquipControl =
                                carrierTariffPivots[nIndex].CarrTarEquipCarrierEquipControl;
                            nPrevCarrTarTariffModeTypeControl =
                                carrierTariffPivots[nIndex].CarrTarTariffModeTypeControl;
                            nPrevCarrTarTempType = carrierTariffPivots[nIndex].CarrTarTempType;
                            nPrevCarrTarTariffTypeControl = carrierTariffPivots[nIndex].CarrTarTariffTypeControl;
                            nPrevCarrTarEquipMatClassTypeControl =
                                carrierTariffPivots[nIndex].CarrTarEquipMatClassTypeControl;
                            nPrevCarrTarEquipMatRateTypeControl =
                                carrierTariffPivots[nIndex].CarrTarEquipMatTarRateTypeControl;
                            // nPrevCarrTarEquipMatClass = dp.CarrTarEquipMatClass; Do not add the class.
                            Logger.Information(
                                "bFirst is true so set nPrevCarrControl: {0}, nPrevCarrTarEquipCarrierEquipControl: {1}, nPrevCarrTarTariffModeTypeControl: {2}, nPrevCarrTarTempType: {3}, nPrevCarrTarTariffTypeControl: {4}, nPrevCarrTarEquipMatClassTypeControl: {5}, nPrevCarrTarEquipMatRateTypeControl: {6}",
                                nPrevCarrControl, nPrevCarrTarEquipCarrierEquipControl,
                                nPrevCarrTarTariffModeTypeControl, nPrevCarrTarTempType, nPrevCarrTarTariffTypeControl,
                                nPrevCarrTarEquipMatClassTypeControl, nPrevCarrTarEquipMatRateTypeControl);
                        }

                        nIndex++;
                    } while (nIndex < carrierTariffPivots.Length &&
                             !PivotDifferenceClass(carrierTariffPivots[nIndex], nPrevCarrControl,
                                 nPrevCarrTarTariffModeTypeControl, nPrevCarrTarTempType, nPrevCarrTarTariffTypeControl,
                                 nPrevCarrTarEquipMatClassTypeControl, nPrevCarrTarEquipMatRateTypeControl,
                                 nPrevCarrTarEquipCarrierEquipControl));

                    // Convert it to an array now, since arrays are immutable.
                    return precPivotsForClassRates.ToArray();

                }
                // Return one record since rates are not differentiated by class for any other type.
                else
                {
                    DTO.CarrierTariffsPivot[] pivotList = new DTO.CarrierTariffsPivot[1];
                    pivotList[0] = carrierTariffPivots[nIndex];
                    nIndex++;
                    return pivotList;
                }
            }
        }

        #endregion
    }
}
