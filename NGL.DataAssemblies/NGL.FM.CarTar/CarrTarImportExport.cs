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
using IG = Infragistics.Excel; 
using System.Collections;
using LOC = Ngl.FreightMaster.Data.Utilities.TariffLocalizationTypesEnum;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;

namespace NGL.FM.CarTar
{
   
    public class CarTarImportExportBase : TarBaseClass 
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarTarImportExportBase(DAL.WCFParameters oParameters)
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
            //this.Errors = new List<string>();
            this.Errors = new List<Dictionary<string, ArrayList>>();
         
        }

        #endregion

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarTarImportExportBase";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        //export
        //public string LocalDirectory { get { return System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "bin\\ExcelTemplates\\"; } }

        //import
        //public string LocalDirectory { get { return System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\ExcelImports\\Target\\"; } }
        //public string LocalDirectory { get { return System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "bin\\ExcelImports\\"; } }
        public string LocalDirectory { get; set; }
        public List<Dictionary<string, ArrayList>> Errors { get; set; }

        private List<DTO.CarrTarEquipMatNode> _carrierTariffsPivotsList = null;
        public List<DTO.CarrTarEquipMatNode> CarrierTariffsPivotsList
        {
            get { return _carrierTariffsPivotsList; }
            set { _carrierTariffsPivotsList = value; }
        }

        private IG.Workbook _wbResult = null;
        public IG.Workbook WorkBookResult
        {
            get
            {

                return _wbResult;
            }
            set { _wbResult = value; }
        }

        public List<DTO.vLookupList> ModeTypes { get; set; }
        public List<DTO.vLookupList> Lanes { get; set; }
        public List<DTO.vLookupList> BracketTypes { get; set; }
        public List<DTO.vLookupList> ClassTypes { get; set; }
        public List<DTO.vLookupList> RateTypes { get; set; }
        public List<DTO.vLookupList> TempTypes { get; set; }

        public string SavedFileName { get; set; }
        
        #endregion

        #region "Methods"

        #region "data methods"

        /// <summary>
        /// throws exceptions
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool ImportCSVToTempTblData(string filePath, Util.ImportExportTypes type)
        {
            bool success = false; 
            string tableName = "";
            switch (type)
            {
                case Util.ImportExportTypes.ImportFromCSVRates:
                    tableName = Util.ImportType_tmpCSVCarrierRates;
                    break; 
                case Util.ImportExportTypes.ImportFromCSVInterline:
                    tableName = Util.ImportType_tmpCSVInterlinePoints;
                    break;
                case Util.ImportExportTypes.ImportFromCSVNonService:
                    tableName = Util.ImportType_tmpCSVNonServicePoints;
                    break; 
            }
            success = NGLFlatFileImport.ProcessTMPCSVCarrierRates(tableName, filePath, true, false);
            return success;

        }

        public bool getLaneControl(string lanenumber, ref int inLaneControl)
        {
            bool success = false;
            if (string.IsNullOrEmpty(lanenumber) || lanenumber.Equals("0")) { return true; }
            try
            {
                var lane = (from DTO.vLookupList d in this.Lanes where d.Description.Equals(lanenumber) select d).FirstOrDefault();
                if (lane.Control == 0) { return false; }
                inLaneControl = lane.Control;
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        public DTO.CarrTarMatBPPivot getMatBPData(int matbpcontrol)
        {
            try
            {
                //get a list of equipment controls.
                DTO.CarrTarMatBPPivot bppivot = NGLCarrTarMatBPData.GetCarrTarMatBPPivot(matbpcontrol);
                return bppivot;
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getMatBPData"), matbpcontrol.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                return null;
            }
            return null;
        }

        public List<DTO.vLookupList> GetvLookupList(int sortKey, DAL.Utilities.LookUpListEnum item)
        {
            DTO.vLookupList[] list = null;
            try
            {
                //get a list of equipment controls.
                list = NGLLookupData.GetViewLookupList(item.ToString(), sortKey);
                if (list == null || list.Length == 0) { return null; }
                return list.ToList();
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrTarEquipMatNodes"), item.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }
            return null;

        }
        public void LoadModeTypeList()
        {
            List<DTO.vLookupList> list = GetvLookupList(1, DAL.Utilities.LookUpListEnum.tblModeType);
            if (list == null) { return; }
            this.ModeTypes = list;
        }
        public void LoadLaneList()
        {
            List<DTO.vLookupList> list = GetvLookupList(1, DAL.Utilities.LookUpListEnum.Lane);
            if (list == null) { return; }
            this.Lanes = list;
        }
        public void LoadBracketTypesList()
        {
            List<DTO.vLookupList> list = GetvLookupList(1, DAL.Utilities.LookUpListEnum.tblTarBracketType);
            if (list == null) { return; }
            this.BracketTypes = list;
        }
        public void LoadClassTypesList()
        {
            List<DTO.vLookupList> list = GetvLookupList(1, DAL.Utilities.LookUpListEnum.tblClassType);
            if (list == null) { return; }
            this.ClassTypes = list;
        }
        public void LoadRateTypesList()
        {
            List<DTO.vLookupList> list = GetvLookupList(1, DAL.Utilities.LookUpListEnum.tblTarRateType);
            if (list == null) { return; }
            this.RateTypes = list;
        }
        public void LoadTempTypesList()
        {
            List<DTO.vLookupList> list = GetvLookupList(1, DAL.Utilities.LookUpListEnum.TariffTempType);
            if (list == null) { return; }
            this.TempTypes = list;
        }
        public DTO.vLookupList getvLookupItem(DAL.Utilities.LookUpListEnum item, int control)
        {
            DTO.vLookupList resultItem = null;
            switch (item)
            {
                case DAL.Utilities.LookUpListEnum.tblTarRateType:
                    resultItem = (from d in this.RateTypes where d.Control == control select d).FirstOrDefault();
                    break;
                case DAL.Utilities.LookUpListEnum.tblTarBracketType:
                    resultItem = (from d in this.BracketTypes where d.Control == control select d).FirstOrDefault();
                    break;
                case DAL.Utilities.LookUpListEnum.tblClassType:
                    resultItem = (from d in this.ClassTypes where d.Control == control select d).FirstOrDefault();
                    break;
                case DAL.Utilities.LookUpListEnum.tblModeType:
                    resultItem = (from d in this.ModeTypes where d.Control == control select d).FirstOrDefault();
                    break;
                case DAL.Utilities.LookUpListEnum.TariffTempType:
                    resultItem = (from d in this.TempTypes where d.Control == control select d).FirstOrDefault();
                    break;
                case DAL.Utilities.LookUpListEnum.Lane:
                    resultItem = (from d in this.Lanes where d.Control == control select d).FirstOrDefault();
                    break;
            }
            return resultItem;
        }
        public DTO.Comp getComp(int control)
        {
            DTO.Comp comp = NGLCompData.GetCompFiltered(control);
            return comp;
        }
        public DTO.Carrier getCarrier(int control)
        {
            DTO.Carrier item = NGLCarrierData.GetCarrierFiltered(control);
            return item;
        }
        public DTO.CarrTarEquip getEquip(int control)
        {
            DTO.CarrTarEquip item = NGLCarrTarEquipData.GetCarrTarEquipFiltered(control);
            return item;
        }
        public void UpdateCarrTarMatPivotBreakPointsNoReturn(int CarrTarMatBPControl,
           decimal BPVal1, decimal BPVal2, decimal BPVal3, decimal BPVal4, decimal BPVal5, decimal BPVal6, decimal BPVal7, decimal BPVal8, decimal BPVal9, decimal BPVal10)
        {
            DTO.CarrTarMatBPPivot item = NGLCarrTarMatBPData.GetCarrTarMatBPPivot(CarrTarMatBPControl);
            if (item == null) { return; };
            if (item == null) { return; }
            if (item.CarrTarMatBPControl == 0) { return; }
            item.BPVal1 = BPVal1;
            item.BPVal2 = BPVal2;
            item.BPVal3 = BPVal3;
            item.BPVal4 = BPVal4;
            item.BPVal5 = BPVal5;
            item.BPVal6 = BPVal6;
            item.BPVal7 = BPVal7;
            item.BPVal8 = BPVal8;
            item.BPVal9 = BPVal9;
            item.BPVal10 = BPVal10;
            NGLCarrTarMatBPData.SaveCarrTarMatBPPivot(item);
        }
        /// <summary>
        ///  Tests if any records exist in the carriertariff table where the CarrTarPreCloneControl = CarrTarControl
        /// true indicates that this tariff has already been cloned at lease once.
        /// </summary>
        /// <param name="CarrTarControl"></param>
        /// <returns></returns>
        public bool HasTariffBeenCloned(int CarrTarControl)
        {

            try
            {
                return NGLCarrTarContractData.HasTariffBeenCloned(CarrTarControl);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("HasTariffBeenCloned"), CarrTarControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }
            return false;

        }
        /// <summary>
        /// Detemines if the contracts have already been cloned before.  If so, then thier contracts are out of date and should find the latest versions.
        /// </summary>
        /// <returns></returns>
        public bool validatCloning()
        {
            foreach (DTO.CarrTarEquipMatNode node in this.CarrierTariffsPivotsList)
            {
                if (node.CarrTarEquipMatCarrTarControl == 0)
                {
                    this.AddErrors(LOC.ETariffWorkSheetContractDataNotFound.ToString(), new[] { node.CarrTarEquipMatName });
                    return false;
                }
                try
                {
                    //if (this.HasTariffBeenCloned(node.CarrTarEquipMatCarrTarControl))
                    //{
                    //    this.AddErrors(LOC.ETariffContractClonedPrevously.ToString(), new[]{node.CarrTarEquipMatName});
                    //    return false;
                    //}
                }
                catch (Exception ex)
                {
                    this.AddErrors(LOC.ETariffContractClonedUnknown.ToString(), new[] { node.CarrTarEquipMatName });
                    return false;
                }
            }
            return true;
        }
        public DTO.GenericResults CloneContract(int CarrTarControl, Nullable<DateTime> effDateFrom, Nullable<DateTime> effDateTo, Util.ImportExportTypes cloneType)
        {

            try
            {
                DTO.GenericResults results = null;
                switch(cloneType)
                {
                    case Util.ImportExportTypes.ImportFromExcel:
                        results = NGLCarrTarContractData.CloneTariff(CarrTarControl,
                                                            effDateFrom,
                                                            effDateTo,
                                                            true,
                                                            null,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            false,
                                                            true);
                   break;
                    case Util.ImportExportTypes.ImportFromCSVRates:
                        //CSV does not clone Break Point Data.
                         results = NGLCarrTarContractData.CloneTariff(CarrTarControl,
                                                            effDateFrom,
                                                            effDateTo,
                                                            true,
                                                            null,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            true,
                                                            false,
                                                            true);
                   break;
                   default:
                    //fail the process
                   break;
                } 
                if (results.Control == 0 || results.ErrNumber != 0)
                {
                    //something is way wrong.
                }
                return results;
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("HasTariffBeenCloned"), CarrTarControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }
            return null;

        }
 
        public int getNewestEquipControlUsingPreClonedValue(int CarrTarEquipPreCloneControl)
        {

            try
            {
                int newEquipControl = NGLCarrTarEquipData.getNewestEquipControlUsingPreClonedValue(CarrTarEquipPreCloneControl);
                return newEquipControl;
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getNewestEquipControlUsingPreClonedValue"), CarrTarEquipPreCloneControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }
            return 0;

        } 
        public int getNewestCarrTarMatBPControlUsingPreClonedValue(int CarrTarMatBPPreCloneControl)
        {

            try
            {
                int newControl = NGLCarrTarEquipData.getNewestCarrTarMatBPControlUsingPreClonedValue(CarrTarMatBPPreCloneControl);
                return newControl;
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getNewestCarrTarMatBPControlUsingPreClonedValue"), CarrTarMatBPPreCloneControl.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }
            return 0;

        }

        /// <summary>
        /// Returns list dictionary so it is easily used for localization.  the Key in the dictionary is the key for the localization table.
        /// the value in the dictionary is the array of custom values to be formated with the location string result.
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, ArrayList>> ImportRates(Util.ImportExportTypes importExportType)
        {
            if (this.CarrierTariffsPivotsList == null || this.CarrierTariffsPivotsList.Count == 0 || this.Errors.Count > 0)
            {
                return this.Errors;
            }
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(this.SavedFileName);
             
            //loop through each record 
            //select the rates for the alt datakey          
            //also save the break point fields. 
            foreach (DTO.CarrTarEquipMatNode node in this.CarrierTariffsPivotsList)
            {
                //skip the BP records if we are importing from CSV
                if (importExportType != Util.ImportExportTypes.ImportFromCSVRates)
                {
                    //lets make sure the update the bp record.
                    //node.CarrTarMatBPPivot should not be null, maybe use these values.
                    UpdateCarrTarMatPivotBreakPointsNoReturn(node.CarrTarMatBPPivot.CarrTarMatBPControl,
                        node.CarrTarMatBPPivot.BPVal1.HasValue ? node.CarrTarMatBPPivot.BPVal1.Value : 0,
                         node.CarrTarMatBPPivot.BPVal2.HasValue ? node.CarrTarMatBPPivot.BPVal2.Value : 0,
                         node.CarrTarMatBPPivot.BPVal3.HasValue ? node.CarrTarMatBPPivot.BPVal3.Value : 0,
                         node.CarrTarMatBPPivot.BPVal4.HasValue ? node.CarrTarMatBPPivot.BPVal4.Value : 0,
                         node.CarrTarMatBPPivot.BPVal5.HasValue ? node.CarrTarMatBPPivot.BPVal5.Value : 0,
                         node.CarrTarMatBPPivot.BPVal6.HasValue ? node.CarrTarMatBPPivot.BPVal6.Value : 0,
                         node.CarrTarMatBPPivot.BPVal7.HasValue ? node.CarrTarMatBPPivot.BPVal7.Value : 0,
                         node.CarrTarMatBPPivot.BPVal8.HasValue ? node.CarrTarMatBPPivot.BPVal8.Value : 0,
                         node.CarrTarMatBPPivot.BPVal9.HasValue ? node.CarrTarMatBPPivot.BPVal9.Value : 0,
                         node.CarrTarMatBPPivot.BPVal10.HasValue ? node.CarrTarMatBPPivot.BPVal10.Value : 0);
                }
                //we are going to change this to use a stored procedure/
                //move the ratelist loop in the sp.
                //get the first record to create the Break Point.

                foreach (DTO.CarrTarEquipMatPivot pivot in node.RatesList)
                {
                    //loop thru the rates, set the new cloned controls and create a new rate record to import.
                    pivot.CarrTarEquipMatCarrTarControl = node.CarrTarEquipMatCarrTarControl;
                    pivot.CarrTarEquipMatCarrTarEquipControl = node.CarrTarEquipMatCarrTarEquipControl;
                    if (node.CarrTarMatBPPivot != null)
                    {
                        pivot.CarrTarEquipMatCarrTarMatBPControl = node.CarrTarMatBPPivot.CarrTarMatBPControl;
                    }
                    else
                    {
                        pivot.CarrTarEquipMatCarrTarMatBPControl = 0;//should never get here.
                    }
                    pivot.CarrTarEquipMatClassTypeControl = node.CarrTarEquipMatClassTypeControl;
                    pivot.CarrTarEquipMatName = fileNameWithoutExt;//this name should be useful for the users.
                    pivot.CarrTarEquipMatTarBracketTypeControl = node.CarrTarEquipMatTarBracketTypeControl;
                    pivot.CarrTarEquipMatTarRateTypeControl = node.CarrTarEquipMatTarRateTypeControl;
                    pivot.CarrTarEquipMatModUser = this.Parameters.UserName;
                    try
                    {
                        NGLCarrTarEquipMatData.CreateCarrTarEquipMatPivot(pivot);
                    }
                    catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                    {
                        if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                        {
                            // "E_SQLExceptionMSG", E_UnExpectedMSG
                            string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                                sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                            SaveSysError(errMsg, sourcePath("ImportRates"), "0");
                            this.AddErrors(LOC.EImportTariffRateSqlProblem.ToString(), new[] { sqlEx.Message });
                        }
                    }
                    catch (Exception ex)
                    {
                        // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                        this.AddErrors(LOC.EImportTariffUnknownProblem.ToString(), new[] { ex.Message });
                        throw;
                    }
                }
            }
            return this.Errors;
        }

     
        
        public DAL.Utilities.TariffRateType getRateType(int rateType)
        {
            switch (rateType)
            {
                case (int)DAL.Utilities.TariffRateType.DistanceM:
                    return DAL.Utilities.TariffRateType.DistanceM;
                case (int)DAL.Utilities.TariffRateType.ClassRate:
                    return DAL.Utilities.TariffRateType.ClassRate;
                case (int)DAL.Utilities.TariffRateType.FlatRate:
                    return DAL.Utilities.TariffRateType.FlatRate;
                case (int)DAL.Utilities.TariffRateType.UnitOfMeasure:
                    return DAL.Utilities.TariffRateType.UnitOfMeasure;
                default:
                    return DAL.Utilities.TariffRateType.DistanceM;//should never get here.
            }
        }
        #endregion

        /// <summary>
        /// converts the error list a json string
        /// </summary>
        /// <returns></returns>
        public string ErrorsToJsonString()
        {
            string result = "";
            if (this.Errors == null) { return result; }
            string output = JsonConvert.SerializeObject(this.Errors);
            return output;
        }

        public void clearErrors()
        {
            this.Errors.Clear();
        }
        public void AddErrors(string key, string[] array)
        {
            this.Errors.Add(new Dictionary<string, ArrayList> { { key, array == null ? null : new ArrayList(array) } });
        }

        public string fileExists(string filename)
        {
            string strPath = LocalDirectory + filename;
            if (!System.IO.File.Exists(strPath))
            {
                throw new ApplicationException("Cannot read configuration settings in application folder or application bin folder.");

            }
            return strPath;
        }

        public IG.Workbook openWorkBook(string filename)
        {
            return Infragistics.Excel.Workbook.Load(fileExists(filename));
        }

        public void CleanUp()
        {
        }

        #endregion

    }
  
    public class CarrTarExportExcel : CarTarImportExportBase
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarrTarExportExcel(DAL.WCFParameters oParameters)
            : base(oParameters)
        {


        }
        
        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrTarExportExcel";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }
        //public string LocalDirectory { get { return System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "bin\\ExcelTemplates\\"; } }
        //public string LocalDirectory { get; set; }
        //private List<DTO.CarrTarEquipMatNode> _carrierTariffsPivotsList = null;
        //public List<DTO.CarrTarEquipMatNode> CarrierTariffsPivotsList
        //{
        //    get { return _carrierTariffsPivotsList; }
        //    set { _carrierTariffsPivotsList = value; }
        //}

     
        private IG.Workbook _wbTemplate = null;
        public IG.Workbook wbTemplate
        {
            get
            {
                if (_wbTemplate == null) { _wbTemplate = new IG.Workbook(); }
                return _wbTemplate;
            }
            set { _wbTemplate = value; }
        }

        //private IG.Workbook _wbResult = null;
        //public IG.Workbook WorkBookResult
        //{
        //    get
        //    {

        //        return _wbResult;
        //    }
        //    set { _wbResult = value; }
        //}

        //public List<DTO.vLookupList> ModeTypes { get; set; }
        //public List<DTO.vLookupList> Lanes { get; set; }
        //public List<DTO.vLookupList> BracketTypes { get; set; }
        //public List<DTO.vLookupList> ClassTypes { get; set; }
        //public List<DTO.vLookupList> RateTypes { get; set; }
        //public List<DTO.vLookupList> TempTypes { get; set; }

        /// <summary>
        /// errors delimited by ;
        /// </summary>
        private List<String> _Errors = null;
        public List<String> Errors
        {
            get
            {
                if (_Errors == null) { _Errors = new List<string>(); }
                return _Errors;
            }
            set { _Errors = value; }
        }
        
        #endregion

        #region "Constants & Enums"

        #endregion

        #region " Methods"

        #region "Data Methods"

        public void LoadCarrTarEquipMatNodes(ArrayList carrtarControls)
        {
            List<DTO.CarrTarEquipMatNode> equipmatNodes = null;
            try
            {
                //get a list of equipment controls.
                equipmatNodes = NGLCarrTarEquipData.GetCarrTarEquipFlatNodesList(carrtarControls);
                if (equipmatNodes == null || equipmatNodes.Count == 0) {
                    this.Errors.Add("Export Carrier Tariff Failure:  " +
                        "Unable to get equipment for contracts, need at least one rate.");
                    return;
                }
                foreach (DTO.CarrTarEquipMatNode node in equipmatNodes)
                {
                    node.Contract = NGLCarrTarContractData.GetCarrTarContractFiltered(node.CarrTarEquipMatCarrTarControl);
                    node.RatesList = (NGLCarrTarEquipMatData.GetCarrTarEquipMatPivots(node.CarrTarEquipMatCarrTarEquipControl,
                        node.CarrTarEquipMatTarRateTypeControl,
                        node.CarrTarEquipMatClassTypeControl,
                        node.CarrTarEquipMatTarBracketTypeControl,
                        1, 1000000, "")).ToList<DTO.CarrTarEquipMatPivot>();//we want to select all the records
                }
                this.CarrierTariffsPivotsList = equipmatNodes;
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrTarEquipMatNodes"), carrtarControls.ToString());
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

        }
         
        #endregion

        public void SetDeploymentState(bool webOrStandardDeployment)
        {
            if (webOrStandardDeployment)
            {
                
                this.LocalDirectory=System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "ExcelTemplates\\";   
            }
            else
            { 
                this.LocalDirectory=System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "bin\\ExcelTemplates\\"; 
            }
        }

        public void GenerateBlankExcelForErrorResponse()
        { 
            try
            {
                wbTemplate = openWorkBook("Blank" + Util.XLSXFileType);
                string newWbName = saveTemplateAsNew(Util.XLSXFileType);
                wbTemplate = openWorkBook(newWbName);  
                IG.Worksheet newWorkSheet =null; 
                
                //add a new sheet to the workbook.
                newWorkSheet = wbTemplate.Worksheets[0];
                int i = 1;
                foreach (string item in this.Errors)
                {
                    newWorkSheet.GetCell("A" + 1 + i).Value = item;
                    i++;
                }
                wbTemplate.Save(LocalDirectory + newWbName);
                this.SavedFileName = LocalDirectory + newWbName;
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("ExportCarrTarRatesToExcel"), "0");
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }
            finally
            {  
                CleanUp();
            }

            return;
        }

        /// <summary>
        /// Each sheet will be a unique Rate Type for a contract and equipment.
        /// So for a contract with 2 equipments and 1 TL rate type on each equipment
        /// there should be 2 sheets.
        /// 
        /// Another example:
        /// 2 Equipments with 1 TL rate type and 1 LTL rate type on one equipment
        /// and 1 TL rate type on the other equipment = 3 sheets total.
        /// </summary>
        public void GenerateCarrTarRatesToExcel()
        {
            if (this.CarrierTariffsPivotsList == null || this.CarrierTariffsPivotsList.Count == 0) { return; }
            try
            { 
                wbTemplate = openWorkBook("Carrier_Tariff_Template" + Util.XLSXFileType);
                string newWbName = saveTemplateAsNew(Util.XLSXFileType);
                WorkBookResult = openWorkBook(newWbName);
                wbTemplate = openWorkBook("Carrier_Tariff_Template" + Util.XLSXFileType);
                WorkBookResult.Worksheets.Clear();//start fresh
                //'create variables   
                foreach (DTO.CarrTarEquipMatNode mat in this.CarrierTariffsPivotsList)
                { 
                   IG.Worksheet newsheet = generateWorkSheet(mat);
                   ProtectSheet(newsheet,mat.CarrTarEquipMatTarRateTypeControl);
                } 
                this.SavedFileName = LocalDirectory + newWbName;
                WorkBookResult.Save(this.SavedFileName);
              
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("ExportCarrTarRatesToExcel"), "0");
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }
            finally
            { 
                CleanUp();
            }

            return;
        }
        public IG.Worksheet copyFromtemplate(IG.Worksheet sourceWorksheet, IG.Workbook workbook_dest)
        {  
            IG.Worksheet destination = workbook_dest.Worksheets.Add(sourceWorksheet.Name);
            sourceWorksheet.Protected = false;
            destination.Protected = false;
            foreach (IG.WorksheetColumn sourceColumn in sourceWorksheet.Columns)
            {
                IG.WorksheetColumn destinationColumn = destination.Columns[sourceColumn.Index];
                destinationColumn.CellFormat.SetFormatting(CreateFormatCopy(workbook_dest, sourceColumn.CellFormat));
                destinationColumn.Width = sourceColumn.Width;
                destinationColumn.Hidden = sourceColumn.Hidden;
            }

            MergeHeaders(destination);
            foreach (IG.WorksheetRow sourceRow in sourceWorksheet.Rows)
            {
                IG.WorksheetRow destinationRow = destination.Rows[sourceRow.Index];
                destinationRow.CellFormat.SetFormatting(CreateFormatCopy(workbook_dest, sourceRow.CellFormat));
                destinationRow.Height = sourceRow.Height;
                destinationRow.Hidden = sourceRow.Hidden;

                foreach (IG.WorksheetCell sourceCell in sourceRow.Cells)
                {
                    IG.WorksheetCell destinationCell = destinationRow.Cells[sourceCell.ColumnIndex];
                    destinationCell.CellFormat.SetFormatting(CreateFormatCopy(workbook_dest, sourceCell.CellFormat));                    
                    destinationCell.Value = sourceCell.Value;
                    if (sourceCell.Formula != null)
                    {
                        destinationCell.ApplyFormula(sourceCell.Formula.ToString());
                    }
                }
            }
            
            return destination;
        }
        public IG.IWorksheetCellFormat CreateFormatCopy(IG.Workbook workbook, IG.IWorksheetCellFormat sourceCellFormat)
        {
         IG.IWorksheetCellFormat copy = workbook.CreateNewWorksheetCellFormat();
         copy.SetFormatting(sourceCellFormat);
         return copy;
        }
        public void MergeHeaders(IG.Worksheet sheet)
        {
            sheet.MergedCellsRegions.Add(0, 0, 0, 3);
           sheet.MergedCellsRegions.Add(1, 0, 1, 3);
           sheet.MergedCellsRegions.Add(2, 0, 2, 3);
           sheet.MergedCellsRegions.Add(3, 0, 3, 3);
        }
        /// <summary>
        /// protects the sheet and locks the top 17 rows.
        /// </summary>
        /// <param name="sheet"></param>
        public void ProtectSheet(IG.Worksheet sheet, int rateType)
        {
            sheet.Protected = true;
            try
            {
                sheet.Rows[0].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[1].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[2].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[3].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[4].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[5].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[6].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[7].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[8].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[9].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[10].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[11].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[12].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[13].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[14].CellFormat.Locked = IG.ExcelDefaultableBoolean.True;
                sheet.Rows[15].CellFormat.Locked = IG.ExcelDefaultableBoolean.True; 
                sheet.Rows[16].CellFormat.Locked = IG.ExcelDefaultableBoolean.False;//row 17 should be editable.
                sheet.Rows[17].CellFormat.Locked = IG.ExcelDefaultableBoolean.False;//row 17 should be editable.
                sheet.Rows[18].CellFormat.Locked = IG.ExcelDefaultableBoolean.False;//row 17 should be editable.
                sheet.Rows[19].CellFormat.Locked = IG.ExcelDefaultableBoolean.False;//row 17 should be editable.
                sheet.Rows[20].CellFormat.Locked = IG.ExcelDefaultableBoolean.False;//row 17 should be editable.
                sheet.Rows[21].CellFormat.Locked = IG.ExcelDefaultableBoolean.False;//row 17 should be editable.

                if (rateType == (int)DAL.Utilities.TariffRateType.ClassRate ||
                    rateType == (int)DAL.Utilities.TariffRateType.UnitOfMeasure)
                {
                    //now need to unprotect the BreakPoint Headers 
                    sheet.GetCell(Util.BPColBP1 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                    sheet.GetCell(Util.BPColBP2 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                    sheet.GetCell(Util.BPColBP3 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                    sheet.GetCell(Util.BPColBP4 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                    sheet.GetCell(Util.BPColBP5 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                    sheet.GetCell(Util.BPColBP6 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                    sheet.GetCell(Util.BPColBP7 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                    sheet.GetCell(Util.BPColBP8 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                    sheet.GetCell(Util.BPColBP9 + Util.BPHeaderRow.ToString()).CellFormat.Locked = IG.ExcelDefaultableBoolean.False;
                }
               
            }
            catch (Exception ex)
            {
            } 
        }
        private IG.Worksheet generateWorkSheet(DTO.CarrTarEquipMatNode mat)
        {
            if (mat == null || mat.Contract == null) { return null; }

            IG.Worksheet newWorkSheet =null ;
            IG.Worksheet wsTemplate = null;

            try
            {
                if (WorkBookResult == null) { return null; }
                

                //get the template worksheet
                wsTemplate = getTemplateWorksheet(mat.CarrTarEquipMatTarRateTypeControl, this.wbTemplate);
                newWorkSheet = copyFromtemplate(wsTemplate, WorkBookResult);
                newWorkSheet.Protected = false;
              //  newWorkSheet.Name = mat.CarrTarEquipMatName;
                newWorkSheet.Protected = false;
                //setup the defaults. 
                newWorkSheet.GetCell(Util.TariffNameCell).Value = mat.Contract.CarrTarName;
                newWorkSheet.GetCell(Util.TariffControlCell).Value = mat.Contract.CarrTarControl;

                newWorkSheet.GetCell(Util.CompanyControlCell).Value = mat.Contract.CarrTarCompControl;
                DTO.Comp comp = getComp(mat.Contract.CarrTarCompControl);
                if (comp != null)
                {  
                    newWorkSheet.GetCell(Util.CompanyNameCell).Value = comp.CompName;
                    newWorkSheet.GetCell(Util.CompanyCityCell).Value = comp.CompStreetCity;
                    newWorkSheet.GetCell(Util.CompanyStateCell).Value = comp.CompStreetState;
                    newWorkSheet.GetCell(Util.CompCountryCell).Value = comp.CompStreetCountry;
                }

                newWorkSheet.GetCell(Util.CarrierControlCell).Value = mat.Contract.CarrTarCarrierControl;
                DTO.Carrier carrier = getCarrier(mat.Contract.CarrTarCarrierControl);
                if (carrier != null)
                {
                    newWorkSheet.GetCell(Util.CarrierNameCell).Value = carrier.CarrierName;
                }

                newWorkSheet.GetCell(Util.DefWgtCell).Value = mat.Contract.CarrTarDefWgt;
                newWorkSheet.GetCell(Util.CarrTarBPBracketTypeCell).Value = mat.Contract.CarrTarBPBracketType;
                newWorkSheet.GetCell(Util.CarrTarOutboundCell).Value = mat.Contract.CarrTarOutbound;
                newWorkSheet.GetCell(Util.CarrTarTariffModeTypeControlCell).Value = mat.Contract.CarrTarTariffModeTypeControl;
                newWorkSheet.GetCell(Util.CarrTarTariffTypeControlCell).Value = mat.Contract.CarrTarTariffTypeControl;
                newWorkSheet.GetCell(Util.CarrTarTariffTypeCell).Value = (mat.Contract.CarrTarTariffType.Equals("\0") ? "" : mat.Contract.CarrTarTariffType);
                newWorkSheet.GetCell(Util.CarrTarTempTypeCell).Value = mat.Contract.CarrTarTempType;
                newWorkSheet.GetCell(Util.CarrTarEquipMatClassTypeControlCell).Value = mat.CarrTarEquipMatClassTypeControl;
                newWorkSheet.GetCell(Util.CarrTarEquipMatTarBracketTypeControlCell).Value = mat.CarrTarEquipMatTarBracketTypeControl;
                newWorkSheet.GetCell(Util.CarrTarEquipMatTarRateTypeControlCell).Value = mat.CarrTarEquipMatTarRateTypeControl;
                newWorkSheet.GetCell(Util.CarrTarEquipMatCarrTarEquipControlCell).Value = mat.CarrTarEquipMatCarrTarEquipControl;
                newWorkSheet.GetCell(Util.CarrTarEquipMatNameCell).Value = mat.CarrTarEquipMatName;

                DTO.CarrTarEquip equip = getEquip(mat.CarrTarEquipMatCarrTarEquipControl);
                if (equip != null)
                {
                    newWorkSheet.GetCell(Util.CarrTarEquipControlNameCell).Value = equip.CarrTarEquipName;
                }

                //setup the name cells
                DTO.vLookupList item = null;
                item = this.getvLookupItem(DAL.Utilities.LookUpListEnum.TariffTempType, mat.Contract.CarrTarTempType);
                if (item != null)
                {
                    newWorkSheet.GetCell(Util.CarrTarTempTypeNameCell).Value = item.Name;
                }

                item = this.getvLookupItem(DAL.Utilities.LookUpListEnum.tblModeType, mat.Contract.CarrTarTariffModeTypeControl);
                if (item != null)
                {
                    newWorkSheet.GetCell(Util.CarrTarTariffModeTypeControlNameCell).Value = item.Name;
                }

                item = this.getvLookupItem(DAL.Utilities.LookUpListEnum.tblTarRateType, mat.CarrTarEquipMatTarRateTypeControl);
                if (item != null)
                {
                    newWorkSheet.GetCell(Util.CarrTarEquipMatTarRateTypeControlNameCell).Value = item.Name;
                }

                item = this.getvLookupItem(DAL.Utilities.LookUpListEnum.tblTarBracketType, mat.CarrTarEquipMatTarBracketTypeControl);
                if (item != null)
                {
                    newWorkSheet.GetCell(Util.CarrTarEquipMatTarBracketTypeControlNameCell).Value = item.Name;
                }

                item = this.getvLookupItem(DAL.Utilities.LookUpListEnum.tblClassType, mat.CarrTarEquipMatClassTypeControl);
                if (item != null)
                {
                    newWorkSheet.GetCell(Util.CarrTarEquipMatClassTypeControlNameCell).Value = item.Name;
                }

                //now set the readable fields.
                newWorkSheet.GetCell(Util.HeaderCell).Value = "Carrier Rating Tariff For " + carrier.CarrierName + " Origin " + comp.CompName;
                newWorkSheet.GetCell(Util.HeaderCompanyInfoCell).Value = comp.CompStreetCity + ", " + comp.CompStreetState + "  " + comp.CompStreetCountry;
                newWorkSheet.GetCell(Util.HeaderTariffNameCell).Value = "Tariff Name " + mat.Contract.CarrTarName;
                newWorkSheet.GetCell(Util.HeaderTariffName1Cell).Value = "";
                newWorkSheet.GetCell(Util.DefWgtEnglishCell).Value = mat.Contract.CarrTarDefWgt ? "Yes" : "No";
                newWorkSheet.GetCell(Util.DirectionEnglishCell).Value = mat.Contract.CarrTarOutbound ? "Outbound" : "Inbound";

                int intLastRow = mat.RatesList.Count - 1;
                if (mat.RatesList.Count > 0)
                {
                    //set the worksheet name to the rate record name.
                    newWorkSheet.Name = mat.RatesList[0].CarrTarEquipMatName;
                    setBreakPointHeaders(mat.RatesList[0], mat.CarrTarEquipMatTarRateTypeControl, newWorkSheet);
                }
                object[,] saRet = getRateRangeArray(mat.CarrTarEquipMatTarRateTypeControl, mat.RatesList.Count);

                //scroll through the rates and add then to the array.
                for (int r = 0; r < mat.RatesList.Count; r++)
                {
                    DTO.CarrTarEquipMatPivot rate = (DTO.CarrTarEquipMatPivot)mat.RatesList[r];
                    DTO.vLookupList lane = null;
                    lane = this.getvLookupItem(DAL.Utilities.LookUpListEnum.Lane, rate.CarrTarEquipMatLaneControl);
                    saRet = setRatesArray(rate, saRet, r, lane, newWorkSheet, Util.RateStartRow);
                }
                
            }
            catch (System.Runtime.InteropServices.COMException oCOMException)
            {
                this.Errors.Add("Export Carrier Tariff Failure:  " +
                    "A communication error occurred while working with a Excel. " +
                "You data may not have been saved.  Please open the file in Excel and verify that the data is valid." +
                 Util.NEWLINE +
                "The Error was: " +
                oCOMException.Message.ToString());
            }
            catch (Exception ex)
            {
                this.Errors.Add("Export Carrier Tariff Failure:  " +
                    ex.Message +
                    Util.NEWLINE +
                    "Please correct the problem if possible and try again.");
            }
           
            return newWorkSheet;
        }

        private object[,] generateRateRangeArray(DTO.CarrTarEquipMatPivot rate, object[,] saRet, int row, DTO.vLookupList lane)
        {
            switch (rate.CarrTarEquipMatTarRateTypeControl)
            {
                case (int)DAL.Utilities.TariffRateType.DistanceM:
                    saRet[row, (int)Util.distanceRateColumns.country] = rate.CarrTarEquipMatCountry;
                    saRet[row, (int)Util.distanceRateColumns.state] = rate.CarrTarEquipMatState;
                    saRet[row, (int)Util.distanceRateColumns.city] = rate.CarrTarEquipMatCity;
                    saRet[row, (int)Util.distanceRateColumns.fromZip] = rate.CarrTarEquipMatFromZip;
                    saRet[row, (int)Util.distanceRateColumns.toZip] = rate.CarrTarEquipMatToZip;
                    if (lane == null && rate.CarrTarEquipMatLaneControl != 0)
                    {
                        saRet[row, (int)Util.distanceRateColumns.lane] = "Could Not Find Lane";
                    }
                    else if (rate.CarrTarEquipMatLaneControl == 0)
                    {
                        saRet[row, (int)Util.distanceRateColumns.lane] = rate.CarrTarEquipMatLaneControl;
                    }
                    else
                    {
                        saRet[row, (int)Util.distanceRateColumns.lane] = lane.Description;
                    }
                    saRet[row, (int)Util.distanceRateColumns.minval] = rate.CarrTarEquipMatMin;
                    saRet[row, (int)Util.distanceRateColumns.transDays] = rate.CarrTarEquipMatMaxDays;
                    saRet[row, (int)Util.distanceRateColumns.rate] = rate.Val10;
                    break;
                case (int)DAL.Utilities.TariffRateType.ClassRate:
                    saRet[row, (int)Util.ClassRateColumns.country] = rate.CarrTarEquipMatCountry;
                    saRet[row, (int)Util.ClassRateColumns.state] = rate.CarrTarEquipMatState;
                    saRet[row, (int)Util.ClassRateColumns.city] = rate.CarrTarEquipMatCity;
                    saRet[row, (int)Util.ClassRateColumns.fromZip] = rate.CarrTarEquipMatFromZip;
                    saRet[row, (int)Util.ClassRateColumns.toZip] = rate.CarrTarEquipMatToZip;
                    if (lane == null && rate.CarrTarEquipMatLaneControl != 0)
                    {
                        saRet[row, (int)Util.ClassRateColumns.lane] = "Could Not Find Lane";
                    }
                    else if (rate.CarrTarEquipMatLaneControl == 0)
                    {
                        saRet[row, (int)Util.ClassRateColumns.lane] = rate.CarrTarEquipMatLaneControl;
                    }
                    else
                    {
                        saRet[row, (int)Util.ClassRateColumns.lane] = lane.Description;
                    }
                    saRet[row, (int)Util.ClassRateColumns.classCol] = rate.CarrTarEquipMatClass;
                    saRet[row, (int)Util.ClassRateColumns.minval] = rate.CarrTarEquipMatMin;
                    saRet[row, (int)Util.ClassRateColumns.transDays] = rate.CarrTarEquipMatMaxDays;
                    saRet[row, (int)Util.ClassRateColumns.BP1] = rate.Val1;
                    saRet[row, (int)Util.ClassRateColumns.BP2] = rate.Val2;
                    saRet[row, (int)Util.ClassRateColumns.BP3] = rate.Val3;
                    saRet[row, (int)Util.ClassRateColumns.BP4] = rate.Val4;
                    saRet[row, (int)Util.ClassRateColumns.BP5] = rate.Val5;
                    saRet[row, (int)Util.ClassRateColumns.BP6] = rate.Val6;
                    saRet[row, (int)Util.ClassRateColumns.BP7] = rate.Val7;
                    saRet[row, (int)Util.ClassRateColumns.BP8] = rate.Val8;
                    saRet[row, (int)Util.ClassRateColumns.BP9] = rate.Val9;
                    break;
                case (int)DAL.Utilities.TariffRateType.FlatRate:
                    saRet[row, (int)Util.flatRateColumns.country] = rate.CarrTarEquipMatCountry;
                    saRet[row, (int)Util.flatRateColumns.state] = rate.CarrTarEquipMatState;
                    saRet[row, (int)Util.flatRateColumns.city] = rate.CarrTarEquipMatCity;
                    saRet[row, (int)Util.flatRateColumns.fromZip] = rate.CarrTarEquipMatFromZip;
                    saRet[row, (int)Util.flatRateColumns.toZip] = rate.CarrTarEquipMatToZip;
                    if (lane == null && rate.CarrTarEquipMatLaneControl != 0)
                    {
                        saRet[row, (int)Util.flatRateColumns.lane] = "Could Not Find Lane";
                    }
                    else if (rate.CarrTarEquipMatLaneControl == 0)
                    {
                        saRet[row, (int)Util.flatRateColumns.lane] = rate.CarrTarEquipMatLaneControl;
                    }
                    else
                    {
                        saRet[row, (int)Util.flatRateColumns.lane] = lane.Description;
                    }
                    saRet[row, (int)Util.flatRateColumns.minval] = rate.CarrTarEquipMatMin;
                    saRet[row, (int)Util.flatRateColumns.transDays] = rate.CarrTarEquipMatMaxDays;
                    saRet[row, (int)Util.flatRateColumns.rate] = rate.Val10;
                    break;
                case (int)DAL.Utilities.TariffRateType.UnitOfMeasure:
                    saRet[row, (int)Util.UOMRateColumns.country] = rate.CarrTarEquipMatCountry;
                    saRet[row, (int)Util.UOMRateColumns.state] = rate.CarrTarEquipMatState;
                    saRet[row, (int)Util.UOMRateColumns.city] = rate.CarrTarEquipMatCity;
                    saRet[row, (int)Util.UOMRateColumns.fromZip] = rate.CarrTarEquipMatFromZip;
                    saRet[row, (int)Util.UOMRateColumns.toZip] = rate.CarrTarEquipMatToZip;
                    if (lane == null && rate.CarrTarEquipMatLaneControl != 0)
                    {
                        saRet[row, (int)Util.UOMRateColumns.lane] = "Could Not Find Lane";
                    }
                    else if (rate.CarrTarEquipMatLaneControl == 0)
                    {
                        saRet[row, (int)Util.UOMRateColumns.lane] = rate.CarrTarEquipMatLaneControl;
                    }
                    else
                    {
                        saRet[row, (int)Util.UOMRateColumns.lane] = lane.Description;
                    }
                    saRet[row, (int)Util.UOMRateColumns.classCol] = rate.CarrTarEquipMatClass;
                    saRet[row, (int)Util.UOMRateColumns.minval] = rate.CarrTarEquipMatMin;
                    saRet[row, (int)Util.UOMRateColumns.transDays] = rate.CarrTarEquipMatMaxDays;
                    saRet[row, (int)Util.UOMRateColumns.BP1] = rate.Val1;
                    saRet[row, (int)Util.UOMRateColumns.BP2] = rate.Val2;
                    saRet[row, (int)Util.UOMRateColumns.BP3] = rate.Val3;
                    saRet[row, (int)Util.UOMRateColumns.BP4] = rate.Val4;
                    saRet[row, (int)Util.UOMRateColumns.BP5] = rate.Val5;
                    saRet[row, (int)Util.UOMRateColumns.BP6] = rate.Val6;
                    saRet[row, (int)Util.UOMRateColumns.BP7] = rate.Val7;
                    saRet[row, (int)Util.UOMRateColumns.BP8] = rate.Val8;
                    saRet[row, (int)Util.UOMRateColumns.BP9] = rate.Val9;
                    break;
            }
            return saRet;
        }
        private object[,] setRatesArray(DTO.CarrTarEquipMatPivot rate, object[,] saRet, int row, DTO.vLookupList lane, IG.Worksheet sheet,int startRow)
        {
            switch (rate.CarrTarEquipMatTarRateTypeControl)
            {
                case (int)DAL.Utilities.TariffRateType.DistanceM:
                    sheet.GetCell(Util.DistColCountry + (row+startRow).ToString()).Value  = rate.CarrTarEquipMatCountry;
                    sheet.GetCell(Util.DistColState + (row + startRow).ToString()).Value = rate.CarrTarEquipMatState;
                    sheet.GetCell(Util.DistColCity + (row + startRow).ToString()).Value = rate.CarrTarEquipMatCity;
                    sheet.GetCell(Util.DistColFromZip + (row + startRow).ToString()).Value = rate.CarrTarEquipMatFromZip;
                    sheet.GetCell(Util.DistColToZip + (row + startRow).ToString()).Value = rate.CarrTarEquipMatToZip;
                    if (lane == null && rate.CarrTarEquipMatLaneControl != 0)
                    {
                        sheet.GetCell(Util.DistColLane + (row + startRow).ToString()).Value = "Could Not Find Lane";
                    }
                    else if (rate.CarrTarEquipMatLaneControl == 0)
                    {
                        sheet.GetCell(Util.DistColLane + (row + startRow).ToString()).Value = rate.CarrTarEquipMatLaneControl;
                    }
                    else
                    {
                        sheet.GetCell(Util.DistColLane + (row + startRow).ToString()).Value = lane.Description;
                    }
                    sheet.GetCell(Util.DistColMinval + (row + startRow).ToString()).Value  = rate.CarrTarEquipMatMin;
                    sheet.GetCell(Util.DistColTransDays + (row + startRow).ToString()).Value = rate.CarrTarEquipMatMaxDays;
                    sheet.GetCell(Util.DistColRate + (row + startRow).ToString()).Value = rate.Val1;
                    break;
                case (int)DAL.Utilities.TariffRateType.ClassRate:
                    sheet.GetCell(Util.ClassColCountry + (row + startRow).ToString()).Value = rate.CarrTarEquipMatCountry;
                    sheet.GetCell(Util.ClassColState+ (row + startRow).ToString()).Value = rate.CarrTarEquipMatState;
                    sheet.GetCell(Util.ClassColCity + (row + startRow).ToString()).Value = rate.CarrTarEquipMatCity;
                    sheet.GetCell(Util.ClassColFromZip + (row + startRow).ToString()).Value = rate.CarrTarEquipMatFromZip;
                    sheet.GetCell(Util.ClassColToZip + (row + startRow).ToString()).Value = rate.CarrTarEquipMatToZip;
                    if (lane == null && rate.CarrTarEquipMatLaneControl != 0)
                    {
                        sheet.GetCell(Util.ClassColLane + (row + startRow).ToString()).Value = "Could Not Find Lane";
                    }
                    else if (rate.CarrTarEquipMatLaneControl == 0)
                    {
                        sheet.GetCell(Util.ClassColLane + (row + startRow).ToString()).Value = rate.CarrTarEquipMatLaneControl;
                    }
                    else
                    {
                        sheet.GetCell(Util.ClassColLane + (row + startRow).ToString()).Value  = lane.Description;
                    }
                    sheet.GetCell(Util.ClassColClass + (row + startRow).ToString()).Value = rate.CarrTarEquipMatClass;
                    sheet.GetCell(Util.ClassColMinval + (row + startRow).ToString()).Value = rate.CarrTarEquipMatMin;
                    sheet.GetCell(Util.ClassColTransDays + (row + startRow).ToString()).Value = rate.CarrTarEquipMatMaxDays;
                    sheet.GetCell(Util.ClassColBP1 + (row + startRow).ToString()).Value = rate.Val1;
                    sheet.GetCell(Util.ClassColBP2 + (row + startRow).ToString()).Value = rate.Val2;
                    sheet.GetCell(Util.ClassColBP3 + (row + startRow).ToString()).Value = rate.Val3;
                    sheet.GetCell(Util.ClassColBP4 + (row + startRow).ToString()).Value = rate.Val4;
                    sheet.GetCell(Util.ClassColBP5 + (row + startRow).ToString()).Value = rate.Val5;
                    sheet.GetCell(Util.ClassColBP6 + (row + startRow).ToString()).Value = rate.Val6;
                    sheet.GetCell(Util.ClassColBP7 + (row + startRow).ToString()).Value = rate.Val7;
                    sheet.GetCell(Util.ClassColBP8 + (row + startRow).ToString()).Value = rate.Val8;
                    sheet.GetCell(Util.ClassColBP9 + (row + startRow).ToString()).Value = rate.Val9;
                    break;
                case (int)DAL.Utilities.TariffRateType.FlatRate:
                    sheet.GetCell(Util.FlatColCountry + (row + startRow).ToString()).Value = rate.CarrTarEquipMatCountry;
                    sheet.GetCell(Util.FlatColState + (row + startRow).ToString()).Value = rate.CarrTarEquipMatState;
                    sheet.GetCell(Util.FlatColCity + (row + startRow).ToString()).Value = rate.CarrTarEquipMatCity;
                    sheet.GetCell(Util.FlatColFromZip + (row + startRow).ToString()).Value  = rate.CarrTarEquipMatFromZip;
                    sheet.GetCell(Util.FlatColToZip + (row + startRow).ToString()).Value = rate.CarrTarEquipMatToZip;
                    if (lane == null && rate.CarrTarEquipMatLaneControl != 0)
                    {
                        sheet.GetCell(Util.FlatColLane + (row + startRow).ToString()).Value = "Could Not Find Lane";
                    }
                    else if (rate.CarrTarEquipMatLaneControl == 0)
                    {
                        sheet.GetCell(Util.FlatColLane + (row + startRow).ToString()).Value = rate.CarrTarEquipMatLaneControl;
                    }
                    else
                    {
                        sheet.GetCell(Util.FlatColLane + (row + startRow).ToString()).Value = lane.Description;
                    }
                    sheet.GetCell(Util.FlatColMinval + (row + startRow).ToString()).Value = rate.CarrTarEquipMatMin;
                    sheet.GetCell(Util.FlatColTransDays + (row + startRow).ToString()).Value = rate.CarrTarEquipMatMaxDays;
                    sheet.GetCell(Util.FlatColRate + (row + startRow).ToString()).Value = rate.Val1;
                    break;
                case (int)DAL.Utilities.TariffRateType.UnitOfMeasure:
                    sheet.GetCell(Util.UOMColCountry + (row + startRow).ToString()).Value  = rate.CarrTarEquipMatCountry;
                    sheet.GetCell(Util.UOMColState + (row + startRow).ToString()).Value = rate.CarrTarEquipMatState;
                    sheet.GetCell(Util.UOMColCity + (row + startRow).ToString()).Value = rate.CarrTarEquipMatCity;
                    sheet.GetCell(Util.UOMColFromZip + (row + startRow).ToString()).Value = rate.CarrTarEquipMatFromZip;
                    sheet.GetCell(Util.UOMColToZip + (row + startRow).ToString()).Value = rate.CarrTarEquipMatToZip;
                    if (lane == null && rate.CarrTarEquipMatLaneControl != 0)
                    {
                        sheet.GetCell(Util.UOMColLane + (row + startRow).ToString()).Value = "Could Not Find Lane";
                    }
                    else if (rate.CarrTarEquipMatLaneControl == 0)
                    {
                        sheet.GetCell(Util.UOMColLane + (row + startRow).ToString()).Value = rate.CarrTarEquipMatLaneControl;
                    }
                    else
                    {
                        sheet.GetCell(Util.UOMColLane + (row + startRow).ToString()).Value = lane.Description;
                    }
                    sheet.GetCell(Util.UOMColClass + (row + startRow).ToString()).Value = rate.CarrTarEquipMatClass;
                    sheet.GetCell(Util.UOMColMinval + (row + startRow).ToString()).Value = rate.CarrTarEquipMatMin;
                    sheet.GetCell(Util.UOMColTransDays + (row + startRow).ToString()).Value = rate.CarrTarEquipMatMaxDays;
                    sheet.GetCell(Util.UOMColBP1 + (row + startRow).ToString()).Value = rate.Val1;
                    sheet.GetCell(Util.UOMColBP2 + (row + startRow).ToString()).Value = rate.Val2;
                    sheet.GetCell(Util.UOMColBP3 + (row + startRow).ToString()).Value = rate.Val3;
                    sheet.GetCell(Util.UOMColBP4 + (row + startRow).ToString()).Value = rate.Val4;
                    sheet.GetCell(Util.UOMColBP5 + (row + startRow).ToString()).Value = rate.Val5;
                    sheet.GetCell(Util.UOMColBP6 + (row + startRow).ToString()).Value = rate.Val6;
                    sheet.GetCell(Util.UOMColBP7 + (row + startRow).ToString()).Value  = rate.Val7;
                    sheet.GetCell(Util.UOMColBP8 + (row + startRow).ToString()).Value = rate.Val8;
                    sheet.GetCell(Util.UOMColBP9 + (row + startRow).ToString()).Value = rate.Val9;
                    break;
            }
            return saRet;
        }
        private void deleteTemplateSheets()
        {
            WorkBookResult.Worksheets.Remove(WorkBookResult.Worksheets["Distance"]);
            WorkBookResult.Worksheets.Remove(WorkBookResult.Worksheets["LTL"]);
            WorkBookResult.Worksheets.Remove(WorkBookResult.Worksheets["UOM"]);
            WorkBookResult.Worksheets.Remove(WorkBookResult.Worksheets["Flat"]);
            WorkBookResult.Worksheets.Remove(WorkBookResult.Worksheets["Sheet2"]);
            WorkBookResult.Worksheets.Remove(WorkBookResult.Worksheets["Sheet3"]);  
        }
        public IG.Worksheet getTemplateWorksheet(int rateType, IG.Workbook templatebook)
        {
            switch (rateType)
            {
                case (int)DAL.Utilities.TariffRateType.DistanceM:
                    return templatebook.Worksheets["Distance"];
                case (int)DAL.Utilities.TariffRateType.ClassRate:
                    return templatebook.Worksheets["LTL"];
                case (int)DAL.Utilities.TariffRateType.FlatRate:
                    return templatebook.Worksheets["Flat"];
                case (int)DAL.Utilities.TariffRateType.UnitOfMeasure:
                    return templatebook.Worksheets["UOM"];
            }
            return null;
        }
        private IG.Worksheet getCopiedWorksheet(int rateType)
        {
            switch (rateType)
            {
                case (int)DAL.Utilities.TariffRateType.DistanceM:
                    return WorkBookResult.Worksheets["Distance (2)"];
                case (int)DAL.Utilities.TariffRateType.ClassRate:
                    return WorkBookResult.Worksheets["LTL (2)"];
                case (int)DAL.Utilities.TariffRateType.FlatRate:
                    return WorkBookResult.Worksheets["Flat (2)"];
                case (int)DAL.Utilities.TariffRateType.UnitOfMeasure:
                    return WorkBookResult.Worksheets["UOM (2)"];
            }
            return null;
        }
        private IG.WorksheetRegion getworksheetRange(int rateType, IG.Worksheet ws, int intLastRow)
        {
            IG.WorksheetRegion range = null;
            switch (rateType)
            {
                case (int)DAL.Utilities.TariffRateType.DistanceM:
                    range = ws.GetRegion(Util.RatesStartCell + Util.DistanceEndCellLetter + (Util.RateStartRow + intLastRow).ToString());
                    break;
                case (int)DAL.Utilities.TariffRateType.ClassRate:
                    range = ws.GetRegion(Util.RatesStartCell + Util.ClassEndCellLetter + (Util.RateStartRow + intLastRow).ToString());
                    break;
                case (int)DAL.Utilities.TariffRateType.FlatRate:
                    range = ws.GetRegion(Util.RatesStartCell + Util.FlatEndCellLetter + (Util.RateStartRow + intLastRow).ToString());
                    break;
                case (int)DAL.Utilities.TariffRateType.UnitOfMeasure:
                    range = ws.GetRegion(Util.RatesStartCell + Util.UOMEndCellLetter + (Util.RateStartRow + intLastRow).ToString());
                    break;
            }
            return range;
        }
        private object[,] getRateRangeArray(int rateType, int rateListLength)
        {
            object[,] saRet = null;
            switch (rateType)
            {
                case (int)DAL.Utilities.TariffRateType.DistanceM:
                    saRet = new object[rateListLength, Util.DistanceEndColumnNum];
                    return saRet;
                case (int)DAL.Utilities.TariffRateType.ClassRate:
                    saRet = new object[rateListLength, Util.ClassEndColumnNum];
                    return saRet;
                case (int)DAL.Utilities.TariffRateType.FlatRate:
                    saRet = new object[rateListLength, Util.FlatEndColumnNum];
                    return saRet;
                case (int)DAL.Utilities.TariffRateType.UnitOfMeasure:
                    saRet = new object[rateListLength, Util.UOMEndColumnNum];
                    return saRet;
            }
            return saRet;
        }
        private void setBreakPointHeaders(DTO.CarrTarEquipMatPivot rate, int rateType, IG.Worksheet newWorkSheet)
        {
            //if (rate.CarrTarEquipMatCarrTarMatBPControl == 0) { return; }
            DTO.CarrTarMatBPPivot bppivot = getMatBPData(rate.CarrTarEquipMatCarrTarMatBPControl);
            string result = "";
            if (bppivot == null) { result = "not found"; }
            switch (rateType)
            {
                case (int)DAL.Utilities.TariffRateType.DistanceM:
                    newWorkSheet.GetCell(Util.CarrTarEquipMatCarrTarMatBPControlCell).Value = rate.CarrTarEquipMatCarrTarMatBPControl;
                    break;
                case (int)DAL.Utilities.TariffRateType.ClassRate:
                    if (bppivot == null || bppivot.CarrTarMatBPControl == 0)
                    {
                        newWorkSheet.GetCell(Util.ClassColBP1 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.ClassColBP2 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.ClassColBP3 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.ClassColBP4 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.ClassColBP5 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.ClassColBP6 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.ClassColBP7 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.ClassColBP8 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.ClassColBP9 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.CarrTarEquipMatCarrTarMatBPControlCell).Value = 0;
                    }
                    else
                    {
                        newWorkSheet.GetCell(Util.ClassColBP1 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal1;
                        newWorkSheet.GetCell(Util.ClassColBP2 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal2;
                        newWorkSheet.GetCell(Util.ClassColBP3 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal3;
                        newWorkSheet.GetCell(Util.ClassColBP4 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal4;
                        newWorkSheet.GetCell(Util.ClassColBP5 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal5;
                        newWorkSheet.GetCell(Util.ClassColBP6 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal6;
                        newWorkSheet.GetCell(Util.ClassColBP7 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal7;
                        newWorkSheet.GetCell(Util.ClassColBP8 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal8;
                        newWorkSheet.GetCell(Util.ClassColBP9 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal9;
                        newWorkSheet.GetCell(Util.CarrTarEquipMatCarrTarMatBPControlCell).Value = rate.CarrTarEquipMatCarrTarMatBPControl;
                    }
                    break;
                case (int)DAL.Utilities.TariffRateType.FlatRate:
                    newWorkSheet.GetCell(Util.CarrTarEquipMatCarrTarMatBPControlCell).Value  = rate.CarrTarEquipMatCarrTarMatBPControl;
                    break;
                case (int)DAL.Utilities.TariffRateType.UnitOfMeasure:
                    if (bppivot == null || bppivot.CarrTarMatBPControl == 0)
                    {
                        newWorkSheet.GetCell(Util.UOMColBP1 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.UOMColBP2 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.UOMColBP3 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.UOMColBP4 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.UOMColBP5 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.UOMColBP6 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.UOMColBP7 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.UOMColBP8 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.UOMColBP9 + Util.BPHeaderRow.ToString()).Value = result;
                        newWorkSheet.GetCell(Util.CarrTarEquipMatCarrTarMatBPControlCell).Value  = 0;
                    }
                    else
                    {
                        newWorkSheet.GetCell(Util.UOMColBP1 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal1;
                        newWorkSheet.GetCell(Util.UOMColBP2 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal2;
                        newWorkSheet.GetCell(Util.UOMColBP3 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal3;
                        newWorkSheet.GetCell(Util.UOMColBP4 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal4;
                        newWorkSheet.GetCell(Util.UOMColBP5 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal5;
                        newWorkSheet.GetCell(Util.UOMColBP6 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal6;
                        newWorkSheet.GetCell(Util.UOMColBP7 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal7;
                        newWorkSheet.GetCell(Util.UOMColBP8 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal8;
                        newWorkSheet.GetCell(Util.UOMColBP9 + Util.BPHeaderRow.ToString()).Value = bppivot.BPVal9;
                        newWorkSheet.GetCell(Util.CarrTarEquipMatCarrTarMatBPControlCell).Value = rate.CarrTarEquipMatCarrTarMatBPControl;
                    }
                    break;
            }
        }
        public void CleanUp()
        {
            try
            {
               // xlApp.Quit();
            }
            catch (Exception ex) { }
            try
            {
                wbTemplate = null;
               // xlApp = null;
            }
            catch (Exception ex) { }
        } 
        private string saveTemplateAsNew(string filetype)
        {
            string filename = Guid.NewGuid().ToString() + filetype;
            string strPath = LocalDirectory + filename;
            wbTemplate.Save(strPath);
            return filename;
        }
         
        #endregion
    }

    public class CarrTarImportExcel : CarTarImportExportBase
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarrTarImportExcel(DAL.WCFParameters oParameters)
            : base(oParameters)
        { 
            this.Errors = new List<Dictionary<string, ArrayList>>(); 
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrTarImportExcel";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }
            
        public bool ImportSuccess { get; set; }

        #endregion

        #region "Constants & Enums"
         
        
        #endregion

        #region " Methods"
         
        public void SetDeploymentState(bool webOrStandardDeployment)
        {
            if (webOrStandardDeployment)
            {//web deployment
                //this one does not include the bin folder.
                string direct = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "ExcelImports\\Target\\";
                string bin = "bin\\";
                if (direct.Contains(bin))
                {
                    int startind = direct.IndexOf(bin);
                    direct.Remove(startind, 5);
                }
                this.LocalDirectory = direct;
            }
            else
            {
                //this one includes the bin folder.
                this.LocalDirectory = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\ExcelImports\\Target\\";
            }
        }
         
        #region "Date Methods"
          
        #endregion

        /// <summary>
        /// Each sheet will be a unique Rate Type for a contract and equipment.
        /// So for a contract with 2 equipments and 1 TL rate type on each equipment
        /// there should be 2 sheets.
        /// 
        /// Another example:
        /// 2 Equipments with 1 TL rate type and 1 LTL rate type on one equipment
        /// and 1 TL rate type on the other equipment = 3 sheets total.
        /// </summary>
        public void ExtractCarrTarRatesFromExcel()
        {
            //if (this.CarrierTariffsPivotsList == null) {
                this.CarrierTariffsPivotsList = new List<DTO.CarrTarEquipMatNode>();
            //}
            try
            {
                this.WorkBookResult = openWorkBook(this.SavedFileName);
                // Xls.Worksheet worksheet = this.WorkBookResult.Sheets[1];
                //for each workbook 
                //create contract
                int numSheetsRejected = 0; //if more than 1 sheet is rejected, reject the whole document.
                //for each row in sheet.
                //create rates
                foreach (IG.Worksheet worksheet in this.WorkBookResult.Worksheets)
                {
                    if (worksheet.Name.Equals("Sheet2") || worksheet.Name.Equals("Sheet3"))
                    {
                        continue;
                    }
                    bool sheetRejected = false;
                    DTO.CarrTarEquipMatNode node = new DTO.CarrTarEquipMatNode();
                    node.Contract = new DTO.CarrTarContract();

                    if (numSheetsRejected > 1)
                    {
                        this.AddErrors(LOC.ImportWorkBookRejected.ToString(), null);
                        ImportSuccess = false;
                        return;
                    }
                    sheetRejected = extractKeyValues(worksheet, ref node);
                    if (sheetRejected)
                    {
                        this.AddErrors(LOC.ImportTariffCouldNotParseKeyData.ToString(), new[] { worksheet.Name });
                        numSheetsRejected += 1;
                        continue;
                    }

                    //made it this far, lets go ahead and grab the contract and the breakpoint data objects from the db.
                    DTO.CarrTarMatBPPivot bpPivot = null;
                    DTO.CarrTarContract contract = null;
                    contract = NGLCarrTarContractData.GetCarrTarContractFiltered(node.CarrTarEquipMatCarrTarControl);
                    if (node.CarrTarMatBPPivot.CarrTarMatBPControl == 0){
                        bpPivot = new DTO.CarrTarMatBPPivot();
                    }
                    else
                    {
                        bpPivot = NGLCarrTarMatBPData.GetCarrTarMatBPPivot(node.CarrTarMatBPPivot.CarrTarMatBPControl);                
                    } 
                    if (bpPivot == null)
                    {
                        sheetRejected = false;
                        this.AddErrors(LOC.ImportTariffCouldNotParseBreakPoints.ToString(), new[] { worksheet.Name });
                        numSheetsRejected += 1;
                        continue;
                    }
                    sheetRejected = validateKeyValues(node, worksheet.Name, contract);
                    if (sheetRejected)
                    {
                        this.AddErrors(LOC.ImportTariffCouldNotKeyDataInvalid.ToString(), new[] { worksheet.Name });
                        numSheetsRejected += 1;
                        continue;
                    }

                    //looks like the control number are valid.
                    //lets fill in the objects from the db.
                    node.Contract = contract;
                    node.CarrTarMatBPPivot = bpPivot;

                    sheetRejected = extractBreakPoints(worksheet, ref node);
                    if (sheetRejected)
                    {
                        this.AddErrors(LOC.ImportTariffCouldNotParseBreakPoints.ToString(), new[] { worksheet.Name });
                        numSheetsRejected += 1;
                        continue;
                    }
                    sheetRejected = extractRates(worksheet, ref node);
                    if (sheetRejected)
                    {
                        this.AddErrors(LOC.ImportTariffCouldNotInvalidRates.ToString(), new[] { worksheet.Name });
                        numSheetsRejected += 1;
                        continue;
                    }
                    this.CarrierTariffsPivotsList.Add(node);
                }
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("ImportCarrTarRatesToExcel"), "0");                    
                }
                this.AddErrors(LOC.tariffSqlFaultProblem.ToString(), new[] { sqlEx.Message });
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                this.AddErrors(LOC.tariffUnknownProblem.ToString(), new[] { ex.Message });
            }
            finally
            {
                //if (WorkBookResult != null) { WorkBookResult.Close(); }
                CleanUp();
            }

            return;
        }
        
        public bool validateKeyValues(DTO.CarrTarEquipMatNode node, string messageReference, DTO.CarrTarContract dbContract)
        { 
            if (node == null) { return true; }
            bool sheetRejected = false;
            if (node.CarrTarEquipMatCarrTarControl == 0)
            {
                this.AddErrors(LOC.ETariffWorkSheetContractDataNotFound.ToString(), new[] { messageReference });
                sheetRejected = true;
                return sheetRejected;
            } 
            if (node.CarrTarEquipMatCarrTarEquipControl == 0)
            {
                this.AddErrors(LOC.ETariffWorkSheetEquipDataNotFound.ToString(), new[] { messageReference });
                sheetRejected = true;
                return sheetRejected;
            }
            if (dbContract == null || dbContract.CarrTarControl == 0 || dbContract.CarrTarControl != node.CarrTarEquipMatCarrTarControl)
            {
                this.AddErrors(LOC.ETariffWorkSheetContractNotFound.ToString(), new[] { messageReference });
                sheetRejected = true;
                return sheetRejected;
            } 
            if (string.IsNullOrEmpty(node.CarrTarEquipMatName))
            {
                this.AddErrors(LOC.ETariffWorkSheetContractRateNotFound.ToString(), new[] { messageReference });
                sheetRejected = true;
                return sheetRejected;
            } 
            
            return sheetRejected;
        }

        private bool extractKeyValues(IG.Worksheet worksheet, ref DTO.CarrTarEquipMatNode node)
        {
            if (worksheet == null) { return true; }
            if (node == null) { return true; }
            bool sheetRejected = false;
            GetCellResult getCellResult = null;
            worksheet.Protected = false;

            //Parse rate Type
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CarrTarEquipMatTarRateTypeControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateTypeNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.CarrTarEquipMatTarRateTypeControl = Convert.ToInt32(getCellResult.Value);
            }
            getCellResult = null;

            //Parse contract control
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.TariffControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTariffControlNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.CarrTarEquipMatCarrTarControl = Convert.ToInt32(getCellResult.Value);
                node.Contract.CarrTarControl = node.CarrTarEquipMatCarrTarControl;
            }
            getCellResult = null;
             
            //Parse CarrTarTempType
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CarrTarTempTypeCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTTempTypeNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.Contract.CarrTarTempType = Convert.ToInt32(getCellResult.Value);
            }
            getCellResult = null;

            //Parse carrier control
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CarrierControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTCarrierControlNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.Contract.CarrTarCarrierControl = Convert.ToInt32(getCellResult.Value);
            }
            getCellResult = null;

            //Parse DefWgt
            getCellResult = new GetCellResult(typeof(bool), worksheet, Util.DefWgtCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, "DefWgt"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.Contract.CarrTarDefWgt = Convert.ToBoolean(getCellResult.Value);
            }
            getCellResult = null;

             
            //Parse CarrTarEquipMatTarBracketTypeControl
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CarrTarEquipMatTarBracketTypeControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, "CarrTarEquipMatTarBracketTypeControl"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.CarrTarEquipMatTarBracketTypeControl = Convert.ToInt32(getCellResult.Value);
                node.Contract.CarrTarBPBracketType = node.CarrTarEquipMatTarBracketTypeControl;
            }
            getCellResult = null;

            //Parse CarrTarOutbound
            getCellResult = new GetCellResult(typeof(bool), worksheet, Util.CarrTarOutboundCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, "CarrTarOutbound"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.Contract.CarrTarOutbound = Convert.ToBoolean(getCellResult.Value); 
            }
            getCellResult = null;


            //Parse CarrTarTariffTypeControl
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CarrTarTariffTypeControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, "CarrTarTariffTypeControl"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.Contract.CarrTarTariffTypeControl = Convert.ToInt32(getCellResult.Value);
            }
            getCellResult = null;

            //Parse CarrTarEquipMatClassTypeControl
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CarrTarEquipMatClassTypeControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, "CarrTarEquipMatClassTypeControl"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.CarrTarEquipMatClassTypeControl = Convert.ToInt32(getCellResult.Value);
            }
            getCellResult = null;

            //Parse CarrTarEquipMatCarrTarEquipControl
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CarrTarEquipMatCarrTarEquipControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,  "CarrTarEquipMatCarrTarEquipControl"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.CarrTarEquipMatCarrTarEquipControl = Convert.ToInt32(getCellResult.Value);                
            }
            getCellResult = null;

            //Parse CompanyControl
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CompanyControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,  "CompanyControl"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.Contract.CarrTarCompControl = Convert.ToInt32(getCellResult.Value);
            }
            getCellResult = null;

            //Parse CarrTarEquipMatName
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.CarrTarEquipMatNameCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, "CarrTarEquipMatName"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.CarrTarEquipMatName = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            
            //Parse CarrTarEquipMatCarrTarMatBPControl
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.CarrTarEquipMatCarrTarMatBPControlCell);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, "CarrTarEquipMatCarrTarMatBPControl"});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                node.CarrTarMatBPPivot.CarrTarMatBPControl = Convert.ToInt32(getCellResult.Value);
            }
            getCellResult = null;
              
            return sheetRejected;
        }

        private bool extractBreakPoints(IG.Worksheet worksheet, ref DTO.CarrTarEquipMatNode node)
        { 
            if (worksheet == null) { return true; }
            if (node == null) { return true; }
            bool sheetRejected = false;
            GetCellResult getCellResult = null;
            worksheet.Protected = false;
            //first determine what rate templete we should use.               
            DAL.Utilities.TariffRateType rateType = getRateType(node.CarrTarEquipMatTarRateTypeControl);

            if (rateType == DAL.Utilities.TariffRateType.ClassRate || 
                rateType == DAL.Utilities.TariffRateType.UnitOfMeasure)
            {
                //Parse BP_1
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP1 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP1 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal1 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;

                //Parse BP_2
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP2 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP2 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal2 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;

                //Parse BP_3
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP3 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP3 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal3 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;

                //Parse BP_4
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP4 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP4 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal4 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;

                //Parse BP_5
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP5 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP5 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal5 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;

                //Parse BP_6
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP6 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP6 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal6 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;

                //Parse BP_7
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP7 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP7 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal7 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;

                //Parse BP_8
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP8 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP8 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal8 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;

                //Parse BP_9
                getCellResult = new GetCellResult(typeof(double), worksheet, Util.BPColBP9 + Util.BPHeaderRow.ToString());
                getCellResult.ParseValue();
                if (getCellResult.Success == false)
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffTItemNotvalid.ToString(), new[] { getCellResult.Message, Util.NEWLINE, Util.BPColBP9 + Util.BPHeaderRow.ToString() });
                    sheetRejected = true;
                    return sheetRejected;
                }
                else
                {
                    node.CarrTarMatBPPivot.BPVal9 = Convert.ToDecimal(getCellResult.Value);
                }
                getCellResult = null;
            }
              
            return sheetRejected;
        }
        
        private bool extractRates(IG.Worksheet worksheet,ref DTO.CarrTarEquipMatNode node)
        {
            if (worksheet == null) { return true; }
            if (node == null) { return true; }
            bool sheetRejected = false;
            try
            {
                //first determine what rate templete we should use.               
                DAL.Utilities.TariffRateType rateType = getRateType(node.CarrTarEquipMatTarRateTypeControl);
                if (worksheet == null) { return true; }
                int totalRowInSheet = worksheet.Rows.Count();
                DTO.CarrTarEquipMatPivot newrec = null;
                for (int i = Util.RateStartRow; i <= totalRowInSheet; i++)
                {
                    newrec = new DTO.CarrTarEquipMatPivot();                
                    switch (rateType)
                    {
                        case DAL.Utilities.TariffRateType.DistanceM:
                            sheetRejected = getDistanceRateFromRow(i, worksheet, ref newrec);
                            break;
                        case DAL.Utilities.TariffRateType.ClassRate:
                            sheetRejected = getClassRateFromRow(i, worksheet, ref newrec);
                            break;
                        case DAL.Utilities.TariffRateType.FlatRate:
                            sheetRejected = getFlatRateFromRow(i, worksheet, ref newrec);
                            break;
                        case DAL.Utilities.TariffRateType.UnitOfMeasure:
                            sheetRejected = getUOMRateFromRow(i, worksheet, ref newrec);
                            break;
                    }
                    if (sheetRejected) { break; }
                    node.RatesList.Add(newrec);
                }
                return sheetRejected;
                 
            }
            catch (System.Runtime.InteropServices.COMException oCOMException)
            {
                this.AddErrors(LOC.EImportTariffComFailure.ToString(), new[]{Util.NEWLINE, oCOMException.Message.ToString()});
            }
            catch (Exception ex)
            {
                this.AddErrors(LOC.EImportTariffExtractRatesUnknown.ToString(), new[]{ex.Message});
            }
             
            return sheetRejected;
        }

        public bool CloneContracts(Nullable < DateTime > effDateFrom, Nullable < DateTime > effDateTo)
        {
            foreach (DTO.CarrTarEquipMatNode node in this.CarrierTariffsPivotsList)
            {
                try
                {
                    DTO.GenericResults result = this.CloneContract(node.CarrTarEquipMatCarrTarControl, effDateFrom, effDateTo, Util.ImportExportTypes.ImportFromExcel);
                    if (result == null || result.Control == 0 || result.ErrNumber != 0)
                    {
                        if (result == null)
                        {
                            this.AddErrors(LOC.EImportCloneFailedEmptyResult.ToString(), new[] { node.CarrTarEquipMatName });
                            return false;
                        }
                        else
                        {
                            this.AddErrors(LOC.EImportCloneFailedUnknown.ToString(), new[] { node.CarrTarEquipMatName, result.RetMsg });
                            return false;
                        }
                    }
                    else
                    {
                        //success
                        //update the contract control numbers.
                        //first get the equipment controls
                        //GetEquipPreCloneControl
                        int newEquipControl = 0;
                        int preCloneEquipControl = node.CarrTarEquipMatCarrTarEquipControl;
                        newEquipControl = this.getNewestEquipControlUsingPreClonedValue(node.CarrTarEquipMatCarrTarEquipControl);
                        if (newEquipControl == 0)
                        {
                            this.AddErrors(LOC.EImportCloneFailedNewestEquipControl.ToString(), new[] { node.CarrTarEquipMatName });
                            return false;
                        }
                        //set the new equipcontrols and contract controls
                        node.CarrTarEquipMatCarrTarEquipControl = newEquipControl;
                        node.CarrTarEquipMatCarrTarControl = result.Control;
                        node.CarrTarMatBPPivot.CarrTarMatBPCarrTarControl = result.Control;
                        node.Contract.CarrTarControl = result.Control;
                        //get the cloned breakpoint.
                        if (node.RatesList != null && node.RatesList.Count > 0)
                        {
                            //all of these rates should have the same BPcontrol because they
                            //are on the same excel sheet and have the same rate type parmaeters.
                            int preCloneBPControl = node.CarrTarMatBPPivot.CarrTarMatBPControl;
                            int newBPControl = 0;
                            if (preCloneBPControl > 0)
                            {
                                newBPControl = this.getNewestCarrTarMatBPControlUsingPreClonedValue(preCloneBPControl);
                                if (newBPControl == 0)
                                {
                                    this.AddErrors(LOC.EImportCloneFailedNewestBPControl.ToString(), new[] { node.CarrTarEquipMatName });
                                    return false;
                                }
                            }
                            node.CarrTarMatBPPivot.CarrTarMatBPControl = newBPControl;
                            foreach (DTO.CarrTarEquipMatPivot rate in node.RatesList)
                            {
                                rate.CarrTarEquipMatCarrTarMatBPControl = newBPControl;
                                rate.CarrTarEquipMatCarrTarControl = result.Control;
                                rate.CarrTarEquipMatCarrTarEquipControl = newEquipControl;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    this.AddErrors(LOC.EImportCloneFailedUnknownException.ToString(), new[] { node.CarrTarEquipMatName, ex.Message });
                    return false;
                }
            }
            return true;
        }
         
        private bool getDistanceRateFromRow(int row, IG.Worksheet worksheet, ref DTO.CarrTarEquipMatPivot newrec)
        {
            if (worksheet == null) { return true; } 
            string laneNumber = "";
            bool sheetRejected = false;
            GetCellResult getCellResult = null;

            //Parse country
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.DistColCountry + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.DistColCountry + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCountry = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse state
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.DistColState + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.DistColState + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatState = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse city
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.DistColCity + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.DistColCity + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCity = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse from zip
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.DistColFromZip + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.DistColFromZip + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatFromZip = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse to zip
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.DistColToZip + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.DistColToZip + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatToZip = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse lane
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.DistColLane + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.DistColLane + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                laneNumber = Convert.ToString(getCellResult.Value);
                int laneControl = 0;
                if (getLaneControl(laneNumber, ref laneControl))
                {
                    newrec.CarrTarEquipMatLaneControl = laneControl;
                }
                else
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffRateFromRowLaneNotFound.ToString(), new[]{Util.NEWLINE , Util.DistColLane + row});
                    sheetRejected = true;
                    return sheetRejected;
                } 
            }
            getCellResult = null;

            //Parse min
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.DistColMinval + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.DistColMinval + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMin = Convert.ToDecimal(getCellResult.Value);
                
            }
            getCellResult = null;

            //Parse transit days
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.DistColTransDays + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.DistColTransDays + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMaxDays = Convert.ToInt32(getCellResult.Value);

            }
            getCellResult = null;

            //Parse rate
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.DistColRate + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.DistColRate + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val1 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            return sheetRejected;
        }
        private bool getClassRateFromRow(int row, IG.Worksheet worksheet,ref DTO.CarrTarEquipMatPivot newrec)
        {
            if (worksheet == null) { return true; } 
            string laneNumber = "";
            bool sheetRejected = false;
            GetCellResult getCellResult = null;

            //Parse country
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.ClassColCountry + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColCountry + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCountry = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse state
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.ClassColState + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColState + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatState = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse city
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.ClassColCity + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.ClassColCity + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCity = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse from zip
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.ClassColFromZip + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.ClassColFromZip + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatFromZip = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse to zip
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.ClassColToZip + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.ClassColToZip + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatToZip = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse lane
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.ClassColLane + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColLane + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                laneNumber = Convert.ToString(getCellResult.Value);
                int laneControl = 0;
                if (getLaneControl(laneNumber, ref laneControl))
                {
                    newrec.CarrTarEquipMatLaneControl = laneControl;
                }
                else
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffRateFromRowLaneNotFound.ToString(), new[]{Util.NEWLINE , Util.ClassColLane + row});
                    sheetRejected = true;
                    return sheetRejected;
                } 
            }
            getCellResult = null;

            //Parse class
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.ClassColClass + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColClass + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatClass = Convert.ToString(getCellResult.Value);

            }
            getCellResult = null;

            //Parse min
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColMinval + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColMinval + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMin = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse transit days
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColTransDays + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColTransDays + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMaxDays = Convert.ToInt32(getCellResult.Value);
                 
            }
            getCellResult = null;

            //Parse bp1
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP1 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.ClassColBP1 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val1 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp2
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP2 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColBP2 + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val2 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp3
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP3 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColBP3 + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val3 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp4
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP4 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.ClassColBP4 + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val4 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp5
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP5 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColBP5 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val5 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp6
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP6 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.ClassColBP6 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val6 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp7
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP7 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColBP7 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val7 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp8
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP8 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColBP8 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val8 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp9
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.ClassColBP9 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.ClassColBP9 + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val9 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            return sheetRejected;
        }
        private bool getFlatRateFromRow(int row, IG.Worksheet worksheet,ref DTO.CarrTarEquipMatPivot newrec)
        {
            if (worksheet == null) { return true; }
            string laneNumber = "";
            bool sheetRejected = false;
            GetCellResult getCellResult = null;

            //Parse country
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.FlatColCountry + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColCountry + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCountry = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse state
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.FlatColState + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColState + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatState = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse city
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.FlatColCity + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColCity + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCity = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse from zip
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.FlatColFromZip + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColFromZip + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatFromZip = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse to zip
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.FlatColToZip + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColToZip + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatToZip = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse lane
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.FlatColLane + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColLane + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                laneNumber = Convert.ToString(getCellResult.Value);
                int laneControl = 0;
                if (getLaneControl(laneNumber, ref laneControl))
                {
                    newrec.CarrTarEquipMatLaneControl = laneControl;
                }
                else
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffRateFromRowLaneNotFound.ToString(), new[]{Util.NEWLINE , Util.FlatColLane + row });
                    sheetRejected = true;
                    return sheetRejected;
                } 
            }
            getCellResult = null;

            //Parse min
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.FlatColMinval + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColMinval + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMin = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse transit days
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.FlatColTransDays + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColTransDays + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMaxDays = Convert.ToInt32(getCellResult.Value);

            }
            getCellResult = null;

            //Parse rate
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.FlatColRate + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.FlatColRate + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val1 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            return sheetRejected;
              
        }
        private bool getUOMRateFromRow(int row, IG.Worksheet worksheet,ref DTO.CarrTarEquipMatPivot newrec)
        {
            if (worksheet == null) { return true; }
            string laneNumber = "";
            bool sheetRejected = false;
            GetCellResult getCellResult = null;

            //Parse country
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.UOMColCountry + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.UOMColCountry + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCountry = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse state
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.UOMColState + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.UOMColState + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatState = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse city
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.UOMColCity + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.UOMColCity + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCity = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse from zip
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.UOMColFromZip + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.UOMColFromZip + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatFromZip = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse to zip
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.UOMColToZip + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.UOMColToZip + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatToZip = Convert.ToString(getCellResult.Value);
            }
            getCellResult = null;

            //Parse lane
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.UOMColLane + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.UOMColLane + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                laneNumber = Convert.ToString(getCellResult.Value);
                int laneControl = 0;
                if (getLaneControl(laneNumber, ref laneControl))
                {
                    newrec.CarrTarEquipMatLaneControl = laneControl;
                }
                else
                {
                    //we have to reject this sheet because the import control numbers are missing.
                    this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.NEWLINE + Util.UOMColLane + row});
                    sheetRejected = true;
                    return sheetRejected;
                } 
            }
            getCellResult = null;

            //Parse class
            getCellResult = new GetCellResult(typeof(string), worksheet, Util.UOMColClass + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColClass + row });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatClass = Convert.ToString(getCellResult.Value);

            }
            getCellResult = null;

            //Parse min
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColMinval + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColMinval + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMin = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse transit days
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColTransDays + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.UOMColTransDays + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMaxDays = Convert.ToInt32(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp1
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP1 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE,Util.UOMColBP1 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val1 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp2
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP2 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColBP2 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val2 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp3
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP3 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColBP3 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val3 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp4
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP4 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColBP4 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val4 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp5
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP5 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColBP5 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val5 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp6
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP6 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColBP6 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val6 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp7
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP7 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColBP7 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val7 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp8
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP8 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColBP8 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val8 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            //Parse bp9
            getCellResult = new GetCellResult(typeof(double), worksheet, Util.UOMColBP9 + row);
            getCellResult.ParseValue();
            if (getCellResult.Success == false)
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[]{getCellResult.Message, Util.NEWLINE, Util.UOMColBP9 + row});
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.Val9 = Convert.ToDecimal(getCellResult.Value);

            }
            getCellResult = null;

            return sheetRejected;
        }
          
        public void CleanUp()
        { 
            try
            {
                System.IO.File.Delete(fileExists(this.SavedFileName));
            }
            catch (Exception ex) { }
        }
         
        #endregion
    }

    public class CarrTarImportRatesCSV : CarTarImportExportBase
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarrTarImportRatesCSV(DAL.WCFParameters oParameters)
            : base(oParameters)
        {
            this.Errors = new List<Dictionary<string, ArrayList>>();
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrTarImportRatesCSV";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        public bool ImportSuccess { get; set; }

        #endregion

        #region "Constants & Enums"


        #endregion

        #region " Methods"

        #region "Data Methods"

        public Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate GetFirstRecordTMPCSVCarrierRates()
        {
            return NGLFlatFileImport.GetFirstRecordTMPCSVCarrierRates();  
        }
         
        public Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate[] GetAllTMPCSVCarrierRates()
        { 
            return NGLFlatFileImport.GetAllTMPCSVCarrierRates();  
        }

        #endregion

        public void SetDeploymentState(bool webOrStandardDeployment)
        {
            if (webOrStandardDeployment)
            {//web deployment 
                //Not supported
                throw new NotImplementedException("Not Supported");
                //this one does not include the bin folder.
                ////string direct = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "ExcelImports\\Target\\";
                ////string bin = "bin\\";
                ////if (direct.Contains(bin))
                ////{
                ////    int startind = direct.IndexOf(bin);
                ////    direct.Remove(startind, 5);
                ////}
                ////this.LocalDirectory = direct;
            }
            else
            {
                this.LocalDirectory = "C:\\Data\\CSVImport\\";
            }
        }
          
        public bool ImportCSVToTempTbl()
        {
            bool success = false;
            try
            {
                string filePathAndName = LocalDirectory + this.SavedFileName;
                //since the path must be on the database server we cant really check if file exists on the web server
               //string filePathAndName = this.fileExists(this.SavedFileName);
               success = ImportCSVToTempTblData(filePathAndName, Util.ImportExportTypes.ImportFromCSVRates);
               if (success == false)
               {
                   this.AddErrors(LOC.EImportCSVCarrierRatesFailed.ToString(), new[] { this.SavedFileName, "" });
               }
            }
            catch (FaultException<Ngl.FreightMaster.Data.SqlFaultInfo> fex)
            {
                this.AddErrors(LOC.EImportCSVCarrierRatesFailed.ToString(), new[] { this.SavedFileName, fex.ToString() });
                success = false;
            }
            catch (Exception ex)
            {
                this.AddErrors(LOC.EImportCSVCarrierRatesFailed.ToString(), new[] { this.SavedFileName, ex.ToString() });
                success = false;
            }
            return success;
        }
         
        public void ExtractCarrTarFirstRateFromTempTbl()
        {
          
            //if (this.CarrierTariffsPivotsList == null)
            //{
             this.CarrierTariffsPivotsList = new List<DTO.CarrTarEquipMatNode>();
            ////}
            Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate[] rates = null;
            try
            {
                try
                {
                    Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate rate = GetFirstRecordTMPCSVCarrierRates();
                    if (rate == null || string.IsNullOrEmpty(rate.EquipName) || string.IsNullOrEmpty(rate.TarID))
                    {
                        throw new Exception("Rates not available or are blank");
                    }
                    rates = new Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate[] { rate };
                    if (rates == null || rates.Length == 0)
                    {
                        throw new Exception("Rates not available or are blank");
                    }
                }
                catch (Exception ex)
                {
                    this.AddErrors(LOC.EUnableToReadTMPCarRates.ToString(), new[] { ex.ToString() });
                    return;
                }
                if (rates == null || rates.Length == 0)
                {
                    throw new Exception("Rates not available or are blank");
                }
                int numRecordsRejected = 0; //if more than 1 record is rejected, reject the whole process.

                bool contractDataSetup = false;//only need to do this once. - however must validate the rest of the records are the same.
                DTO.CarrTarEquipMatNode node = new DTO.CarrTarEquipMatNode();
                node.Contract = new DTO.CarrTarContract();
                for (int intI = 0; intI < rates.Length; intI++)
                {
                    Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate rateRow = rates[intI]; 
                    bool csvRejcted = false;
                    if (rateRow == null)
                    {
                        numRecordsRejected += 1;
                        this.AddErrors(LOC.EImportRateRejectedRateNull.ToString(), null);
                        ImportSuccess = false;
                        return;
                    }
                    if (contractDataSetup == false)
                    {



                        csvRejcted = extractKeyValues(rateRow, ref node);
                        if (csvRejcted)
                        {
                            this.AddErrors(LOC.EImportCSVTariffCouldNotParseKeyData.ToString(), new[] { rateRow.TarID, intI.ToString() });
                            numRecordsRejected += 1;
                            ImportSuccess = false;
                            return;
                        }

                        //save of the from and To dates
                        Nullable<DateTime> fromDate = node.Contract.CarrTarEffDateFrom;
                        Nullable<DateTime> toDate = node.Contract.CarrTarEffDateTo;

                        DTO.CarrTarContract contract = null;
                        contract = NGLCarrTarContractData.GetCarrTarContractFiltered(node.Contract.CarrTarID.ToUpper());
                        contract.CarrTarEffDateFrom = fromDate;
                        contract.CarrTarEffDateTo = toDate;
                        node.CarrTarEquipMatCarrTarControl = contract.CarrTarControl;
                        DTO.CarrTarEquip equip = NGLCarrTarEquipData.GetCarrTarEquipFiltered(contract.CarrTarControl, node.CarrTarEquipMatName);
                        node.CarrTarEquipMatCarrTarEquipControl = equip.CarrTarEquipControl;
                        // node.CarrTarMatBPPivot
                        node.CarrTarMatBPPivot = NGLCarrTarMatBPData.GetCarrTarMatBPPivotByRef(node.CarrTarEquipMatCarrTarControl,
                            node.CarrTarEquipMatTarRateTypeControl,
                            node.CarrTarEquipMatClassTypeControl,
                            node.CarrTarEquipMatTarBracketTypeControl);
                        csvRejcted = validateKeyValues(node, contract.CarrTarID, contract);
                        if (csvRejcted)
                        {
                            this.AddErrors(LOC.ImportTariffCouldNotKeyDataInvalid.ToString(), new[] { contract.CarrTarID });
                            numRecordsRejected += 1;
                            ImportSuccess = false;
                            return;
                        }

                        ////looks like the control number are valid.
                        ////lets fill in the objects from the db.
                        node.Contract = contract;
                        this.CarrierTariffsPivotsList.Add(node);
                        contractDataSetup = true;
                    }
                    else
                    {
                        //validate the rest of the records to make sure they are for the same tariff and rates.
                        //TODO validation
                    }

                    csvRejcted = extractRate(rateRow, ref node, intI);
                    if (csvRejcted)
                    {
                        this.AddErrors(LOC.ImportTariffCouldNotInvalidRates.ToString(), new[] { node.Contract.CarrTarID });
                        numRecordsRejected += 1;
                        ImportSuccess = false;
                        return;
                    }
                   
                }
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("ImportCarrTarRatesToExcel"), "0");
                }
                this.AddErrors(LOC.tariffSqlFaultProblem.ToString(), new[] { sqlEx.Message });
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                this.AddErrors(LOC.tariffUnknownProblem.ToString(), new[] { ex.Message });
            }
            finally
            {
                //if (WorkBookResult != null) { WorkBookResult.Close(); }
                CleanUp();
            }

            return;
        }
        
        public bool validateKeyValues(DTO.CarrTarEquipMatNode node, string messageReference, DTO.CarrTarContract dbContract)
        {
            if (node == null) { return true; }
            bool sheetRejected = false;
            //TODO
            if (node.CarrTarEquipMatCarrTarControl == 0)
            {
                this.AddErrors(LOC.ETariffWorkSheetContractDataNotFound.ToString(), new[] { messageReference });
                sheetRejected = true;
                return sheetRejected;
            }
            if (node.CarrTarEquipMatCarrTarEquipControl == 0)
            {
                this.AddErrors(LOC.ETariffWorkSheetEquipDataNotFound.ToString(), new[] { messageReference });
                sheetRejected = true;
                return sheetRejected;
            }
            if (dbContract == null || dbContract.CarrTarControl == 0 || dbContract.CarrTarControl != node.CarrTarEquipMatCarrTarControl)
            {
                this.AddErrors(LOC.ETariffWorkSheetContractNotFound.ToString(), new[] { messageReference });
                sheetRejected = true;
                return sheetRejected;
            }
            if (string.IsNullOrEmpty(node.CarrTarEquipMatName))
            {
                this.AddErrors(LOC.ETariffWorkSheetContractRateNotFound.ToString(), new[] { messageReference });
                sheetRejected = true;
                return sheetRejected;
            }

            return sheetRejected;
        }

        private bool extractKeyValues(Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate rateRow, ref DTO.CarrTarEquipMatNode node)
        {
            if (rateRow == null) { return true; }
            if (node == null) { return true; }
            if (node.Contract == null) { return true; }
            bool rowRejected = false;

            //CarID
            if (string.IsNullOrEmpty(rateRow.TarID))
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffTarIDNotvalid.ToString(), new[] { rateRow.TarID });
                rowRejected = true;
                return rowRejected;
            }
            else
            {
                node.Contract.CarrTarID = rateRow.TarID; 
            }
            //Equip Name
            if (string.IsNullOrEmpty(rateRow.EquipName))
            {
                this.AddErrors(LOC.EImportTariffEquipNameNotvalid.ToString(), new[] { rateRow.EquipName, });
                rowRejected = true;
                return rowRejected;
            }
            else
            {
                node.CarrTarEquipMatName = rateRow.EquipName;
            }
            //Bracket Type Control
            if (rateRow.BracketTypeControl == null ||
                rateRow.BracketTypeControl.HasValue == false)
            { 
                this.AddErrors(LOC.EImportTariffBracketTypeNotvalid.ToString(), new[] { rateRow.BracketTypeControl.Value.ToString() });
                rowRejected = true;
                return rowRejected;
            }
            else
            {
                node.CarrTarEquipMatTarBracketTypeControl = rateRow.BracketTypeControl.Value;
                node.Contract.CarrTarBPBracketType = rateRow.BracketTypeControl.Value;
            }
            //Rate Type control
            if (rateRow.RateTypeControl == null ||
                rateRow.RateTypeControl.HasValue == false ||
                rateRow.RateTypeControl.Value == 0)
            {
                this.AddErrors(LOC.EImportTariffRateTypeNotvalid.ToString(), new[] { rateRow.RateTypeControl.Value.ToString() });
                rowRejected = true;
                return rowRejected;
            }
            else
            {
                node.CarrTarEquipMatTarRateTypeControl = rateRow.RateTypeControl.Value; 
            }

            //Class Type control
            if (rateRow.ClassTypeControl == null ||
                rateRow.ClassTypeControl.HasValue == false)
            {
                this.AddErrors(LOC.EImportTariffClassTypeeNotvalid.ToString(), new[] { rateRow.ClassTypeControl.Value.ToString() });
                rowRejected = true;
                return rowRejected;
            }
            else
            {
                node.CarrTarEquipMatClassTypeControl = rateRow.ClassTypeControl.Value;
            }

            //Eff From date control
            if (rateRow.EffDateFrom == null ||
                rateRow.EffDateFrom.HasValue == false)
            {
                this.AddErrors(LOC.EImportTariffClassTypeeNotvalid.ToString(), new[] { rateRow.EffDateFrom.Value.ToString() });
                rowRejected = true;
                return rowRejected;
            }
            else
            {
                node.Contract.CarrTarEffDateFrom = rateRow.EffDateFrom;
            }

            //Eff To date control
            node.Contract.CarrTarEffDateTo = rateRow.EffDateTo; 
             
            return rowRejected;
        }
         
        private bool extractRate(Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate rateRow, ref DTO.CarrTarEquipMatNode node, int index)
        {
            if (rateRow == null) { return true; }
            if (node == null) { return true; }
            bool sheetRejected = false;
             
            try
            {
                DAL.Utilities.TariffRateType rateType = getRateType(node.CarrTarEquipMatTarRateTypeControl);
                DTO.CarrTarEquipMatPivot newrec = null;
                newrec = new DTO.CarrTarEquipMatPivot();
                switch (rateType)
                {
                    case DAL.Utilities.TariffRateType.DistanceM:
                        sheetRejected = getDistanceRateFromRow(index, rateRow, ref newrec);
                        break;
                    case DAL.Utilities.TariffRateType.ClassRate:
                        sheetRejected = getClassRateFromRow(index, rateRow, ref newrec);
                        break;
                    case DAL.Utilities.TariffRateType.FlatRate:
                        sheetRejected = getFlatRateFromRow(index, rateRow, ref newrec);
                        break;
                    case DAL.Utilities.TariffRateType.UnitOfMeasure:
                        sheetRejected = getUOMRateFromRow(index, rateRow, ref newrec);
                        break;
                }
                node.RatesList.Add(newrec);
                 
                return sheetRejected;

            }
            catch (System.Runtime.InteropServices.COMException oCOMException)
            {
                this.AddErrors(LOC.EImportTariffComFailure.ToString(), new[] { Util.NEWLINE, oCOMException.Message.ToString() });
            }
            catch (Exception ex)
            {
                this.AddErrors(LOC.EImportTariffExtractRatesUnknown.ToString(), new[] { ex.Message });
            }

            return sheetRejected;
        }

        public bool CloneContracts()
        {
            foreach (DTO.CarrTarEquipMatNode node in this.CarrierTariffsPivotsList)
            {
                try
                {
                    DTO.GenericResults result = this.CloneContract(node.CarrTarEquipMatCarrTarControl, node.Contract.CarrTarEffDateFrom, node.Contract.CarrTarEffDateTo, Util.ImportExportTypes.ImportFromCSVRates);
                    if (result == null || result.Control == 0 || result.ErrNumber != 0)
                    {
                        if (result == null)
                        {
                            this.AddErrors(LOC.EImportCloneFailedEmptyResult.ToString(), new[] { node.CarrTarEquipMatName });
                            return false;
                        }
                        else
                        {
                            this.AddErrors(LOC.EImportCloneFailedUnknown.ToString(), new[] { node.CarrTarEquipMatName, result.RetMsg });
                            return false;
                        }
                    }
                    else
                    {
                        //success
                        //update the contract control numbers.
                        //first get the equipment controls
                        //GetEquipPreCloneControl
                        int newEquipControl = 0;//CarrTarEquipMatCarrTarEquipControl
                        int preCloneEquipControl = node.CarrTarEquipMatCarrTarEquipControl;
                        newEquipControl = this.getNewestEquipControlUsingPreClonedValue(node.CarrTarEquipMatCarrTarEquipControl);
                        if (newEquipControl == 0)
                        {
                            this.AddErrors(LOC.EImportCloneFailedNewestEquipControl.ToString(), new[] { node.CarrTarEquipMatName });
                            return false;
                        }
                        //set the new equipcontrols and contract controls
                        node.CarrTarEquipMatCarrTarEquipControl = newEquipControl;
                        node.CarrTarEquipMatCarrTarControl = result.Control; 
                        node.Contract.CarrTarControl = result.Control;

                        if (node.RatesList != null && node.RatesList.Count > 0 && node.RatesList[0] != null)
                        {
                            //get the cloned breakpoint.
                            //DTO.CarrTarEquipMatPivot rate1 = node.RatesList[0];
                            DAL.Utilities.TariffRateType rateType = this.getRateType(node.CarrTarEquipMatTarRateTypeControl);
                            if (rateType == DAL.Utilities.TariffRateType.ClassRate ||
                                 rateType == DAL.Utilities.TariffRateType.UnitOfMeasure)
                            {
                                //get the new bp stuff from the rate controls                             
                                //node.CarrTarMatBPPivot = new DTO.CarrTarMatBPPivot();
                                //node.CarrTarMatBPPivot.CarrTarMatBPCarrTarControl = result.Control;
                                //node.CarrTarMatBPPivot = NGLCarrTarMatBPData.GetCarrTarMatBPPivotByRef(
                                //     result.Control,
                                //    node.CarrTarEquipMatTarRateTypeControl,
                                //    node.CarrTarEquipMatClassTypeControl,
                                //    node.CarrTarEquipMatTarBracketTypeControl);

                                 

                                int preCloneBPControl = node.CarrTarMatBPPivot.CarrTarMatBPControl;
                                int newBPControl = 0;
                                if (preCloneBPControl > 0)
                                {
                                    newBPControl = this.getNewestCarrTarMatBPControlUsingPreClonedValue(preCloneBPControl);
                                    if (newBPControl == 0)
                                    {
                                        this.AddErrors(LOC.EImportCloneFailedNewestBPControl.ToString(), new[] { node.CarrTarEquipMatName });
                                        return false;
                                    }
                                }
                                node.CarrTarMatBPPivot.CarrTarMatBPControl = newBPControl;
                            }
                            foreach (DTO.CarrTarEquipMatPivot rate 
                                in node.RatesList)  //there should be only one in here for csv import.
                            {
                                rate.CarrTarEquipMatCarrTarMatBPControl = node.CarrTarMatBPPivot.CarrTarMatBPControl;
                                rate.CarrTarEquipMatCarrTarControl = result.Control;
                                rate.CarrTarEquipMatCarrTarEquipControl = newEquipControl;
                                rate.CarrTarEquipMatTarBracketTypeControl = node.CarrTarEquipMatTarBracketTypeControl;
                                rate.CarrTarEquipMatTarRateTypeControl = node.CarrTarEquipMatTarRateTypeControl;
                                rate.CarrTarEquipMatClassTypeControl = node.CarrTarEquipMatClassTypeControl; 
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    this.AddErrors(LOC.EImportCloneFailedUnknownException.ToString(), new[] { node.CarrTarEquipMatName, ex.Message });
                    return false;
                }
            }
            return true;
        }

        public List<Dictionary<string, ArrayList>> ImportRatesByCSV(Util.ImportExportTypes importExportType, string CarrTarEquipMatName)
        {
            if (this.CarrierTariffsPivotsList == null || this.CarrierTariffsPivotsList.Count == 0 || this.Errors.Count > 0)
            {
                return this.Errors;
            }
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(this.SavedFileName);
             
            foreach (DTO.CarrTarEquipMatNode node in this.CarrierTariffsPivotsList)//there should be only 1 in this list
            {
                if (node.RatesList.Count > 0)//there should be only 1 in this list too.
                {  
                    try
                    {
                      LTS.spImportCSVRatesFromTmpTblResult result =  NGLFlatFileImport.ImportCSVRatesFromTmpTbl(node.CarrTarEquipMatCarrTarEquipControl,
                            node.CarrTarEquipMatCarrTarControl,
                            node.CarrTarMatBPPivot.CarrTarMatBPControl,
                            Parameters.UserName,
                            CarrTarEquipMatName);
                      if (result == null)
                      {
                          this.AddErrors(LOC.EImportTariffUnknownProblem.ToString(), new[] { "spImportCSVRatesFromTmpTblResult is null" });
                          this.ImportSuccess = false;
                      }
                      if (result.Success == false)
                      {
                          this.AddErrors(LOC.EImportTariffUnknownProblem.ToString(), new[] { result.RetMsg }); 
                          this.ImportSuccess = false;
                      }
                      else
                      {
                          this.ImportSuccess = true;
                          //TODO need to add better messaging to show user how many records were processes and to show any errors.
                          return this.Errors;
                      }
                    }
                    catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                    {
                        if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                        {
                            // "E_SQLExceptionMSG", E_UnExpectedMSG
                            string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                                sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                            SaveSysError(errMsg, sourcePath("ImportRates"), "0");
                            this.AddErrors(LOC.EImportTariffRateSqlProblem.ToString(), new[] { sqlEx.Message });
                        }
                    }
                    catch (Exception ex)
                    {
                        // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                        this.AddErrors(LOC.EImportTariffUnknownProblem.ToString(), new[] { ex.Message });
                        throw;
                    }
                } 
            }
            return this.Errors;
        }


        private bool getDistanceRateFromRow(int row, Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate rateRow, ref DTO.CarrTarEquipMatPivot newrec)
        {
            if (rateRow == null) { return true; } 
            bool sheetRejected = false;

            //Parse country  
            if (string.IsNullOrEmpty(rateRow.Country))
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[] { rateRow.Country, Util.NEWLINE, row.ToString() });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCountry = rateRow.Country;
            } 

            //Parse state 
            newrec.CarrTarEquipMatState = rateRow.State;
            newrec.CarrTarEquipMatCity = rateRow.City;
            newrec.CarrTarEquipMatFromZip = rateRow.FromZip;
            newrec.CarrTarEquipMatToZip = rateRow.ToZip;
            newrec.CarrTarEquipMatToZip = rateRow.ToZip; 
              
            //Parse lane
            if (rateRow.Lane == null || rateRow.Lane.HasValue == false)
            {
                newrec.CarrTarEquipMatLaneControl = 0;
            }
            if (rateRow.Lane.Value == 0)
            {
                newrec.CarrTarEquipMatLaneControl = 0;
            }
            else
            {
                newrec.CarrTarEquipMatLaneControl = rateRow.Lane.Value; 
            }

            if (rateRow.Min == null || rateRow.Min.HasValue == false)
            {
                this.AddErrors(LOC.EImportTariffMinvalueNotvalid.ToString(), new[] { "Min", row.ToString() });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMin = rateRow.Min.Value;
            }

            //sometimes they dont care about accurate must leave by and estimated delivery dates, so max days can be null or 0.
            if (rateRow.MaxDays == null || rateRow.MaxDays.HasValue == false)
            {
                newrec.CarrTarEquipMatMaxDays = 0;
            }
            else
            {
                newrec.CarrTarEquipMatMaxDays = rateRow.MaxDays.Value;
            } 

            //get the rate.
            newrec.Val1 = rateRow.Val1;
          

            return sheetRejected;
        }
        private bool getClassRateFromRow(int row, Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate rateRow, ref DTO.CarrTarEquipMatPivot newrec)
        {
             
            if (rateRow == null) { return true; }
            bool sheetRejected = false;

            //Parse country  
            if (string.IsNullOrEmpty(rateRow.Country))
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[] { rateRow.Country, Util.NEWLINE, row.ToString() });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCountry = rateRow.Country;
            }

            //Parse state 
            newrec.CarrTarEquipMatState = rateRow.State;
            newrec.CarrTarEquipMatCity = rateRow.City;
            newrec.CarrTarEquipMatFromZip = rateRow.FromZip;
            newrec.CarrTarEquipMatToZip = rateRow.ToZip;
            newrec.CarrTarEquipMatToZip = rateRow.ToZip;

            //Parse lane
            if (rateRow.Lane == null || rateRow.Lane.HasValue == false)
            {
                newrec.CarrTarEquipMatLaneControl = 0;
            }
            if (rateRow.Lane.Value == 0)
            {
                newrec.CarrTarEquipMatLaneControl = 0;
            }
            else
            {
                newrec.CarrTarEquipMatLaneControl = rateRow.Lane.Value;
            }

            if (rateRow.Min == null || rateRow.Min.HasValue == false)
            {
                //this.AddErrors(LOC.EImportTariffMinvalueNotvalid.ToString(), new[] { "Min", row.ToString() });
                //sheetRejected = true;
                //return sheetRejected;
                //they must want the min value to be 0.
                newrec.CarrTarEquipMatMin = 0;
            }
            else
            {
                newrec.CarrTarEquipMatMin = rateRow.Min.Value;
            }
              
            //sometimes they dont care about accurate must leave by and estimated delivery dates, so max days can be null or 0.
            if (rateRow.MaxDays == null || rateRow.MaxDays.HasValue == false)
            {
                newrec.CarrTarEquipMatMaxDays = 0;
            }
            else
            {
                newrec.CarrTarEquipMatMaxDays = rateRow.MaxDays.Value;
            } 

            //get the rate.
            newrec.Val1 = rateRow.Val1;
            newrec.Val2 = rateRow.Val2;
            newrec.Val3 = rateRow.Val3;
            newrec.Val4 = rateRow.Val4;
            newrec.Val5 = rateRow.Val5;
            newrec.Val6 = rateRow.Val6;
            newrec.Val7 = rateRow.Val7;
            newrec.Val8 = rateRow.Val8;
            newrec.Val9 = rateRow.Val9;
            newrec.CarrTarEquipMatClass = rateRow.Class;
       
       
             return sheetRejected;
        }
        private bool getFlatRateFromRow(int row, Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate rateRow, ref DTO.CarrTarEquipMatPivot newrec)
        {
            if (rateRow == null) { return true; }
            bool sheetRejected = false;

            //Parse country  
            if (string.IsNullOrEmpty(rateRow.Country))
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[] { rateRow.Country, Util.NEWLINE, row.ToString() });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCountry = rateRow.Country;
            }

            //Parse state 
            newrec.CarrTarEquipMatState = rateRow.State;
            newrec.CarrTarEquipMatCity = rateRow.City;
            newrec.CarrTarEquipMatFromZip = rateRow.FromZip;
            newrec.CarrTarEquipMatToZip = rateRow.ToZip;
            newrec.CarrTarEquipMatToZip = rateRow.ToZip;

            //Parse lane
            if (rateRow.Lane == null || rateRow.Lane.HasValue == false)
            {
                newrec.CarrTarEquipMatLaneControl = 0;
            }
            if (rateRow.Lane.Value == 0)
            {
                newrec.CarrTarEquipMatLaneControl = 0;
            }
            else
            {
                newrec.CarrTarEquipMatLaneControl = rateRow.Lane.Value;
            }

            if (rateRow.Min == null || rateRow.Min.HasValue == false)
            {
                this.AddErrors(LOC.EImportTariffMinvalueNotvalid.ToString(), new[] { "Min", row.ToString() });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMin = rateRow.Min.Value;
            }

            //sometimes they dont care about accurate must leave by and estimated delivery dates, so max days can be null or 0.
            if (rateRow.MaxDays == null || rateRow.MaxDays.HasValue == false)
            {
                newrec.CarrTarEquipMatMaxDays = 0;
            }
            else
            {
                newrec.CarrTarEquipMatMaxDays = rateRow.MaxDays.Value;
            } 

            //get the rate.
            newrec.Val1 = rateRow.Val1; 
            newrec.CarrTarEquipMatClass = rateRow.Class;
             

            //final business rule: Some times the flat rate is stored in
            //Min Charge and not in the Val1.  
            //Check the val1, if it is 0 put the min charge in to Val1.
            if (newrec.Val1 == 0)
            {
                newrec.Val1 = newrec.CarrTarEquipMatMin;
            }

            return sheetRejected;

        }
        private bool getUOMRateFromRow(int row, Ngl.FreightMaster.Data.LTS.tmpCSVCarrierRate rateRow, ref DTO.CarrTarEquipMatPivot newrec)
        {
            if (rateRow == null) { return true; }
            bool sheetRejected = false;

            //Parse country  
            if (string.IsNullOrEmpty(rateRow.Country))
            {
                //we have to reject this sheet because the import control numbers are missing.
                this.AddErrors(LOC.EImportTariffRateFromRowCellNotValid.ToString(), new[] { rateRow.Country, Util.NEWLINE, row.ToString() });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatCountry = rateRow.Country;
            }

            //Parse state 
            newrec.CarrTarEquipMatState = rateRow.State;
            newrec.CarrTarEquipMatCity = rateRow.City;
            newrec.CarrTarEquipMatFromZip = rateRow.FromZip;
            newrec.CarrTarEquipMatToZip = rateRow.ToZip;
            newrec.CarrTarEquipMatToZip = rateRow.ToZip;

            //Parse lane
            if (rateRow.Lane == null || rateRow.Lane.HasValue == false)
            {
                newrec.CarrTarEquipMatLaneControl = 0;
            }
            if (rateRow.Lane.Value == 0)
            {
                newrec.CarrTarEquipMatLaneControl = 0;
            }
            else
            {
                newrec.CarrTarEquipMatLaneControl = rateRow.Lane.Value;
            }

            if (rateRow.Min == null || rateRow.Min.HasValue == false)
            {
                this.AddErrors(LOC.EImportTariffMinvalueNotvalid.ToString(), new[] { "Min", row.ToString() });
                sheetRejected = true;
                return sheetRejected;
            }
            else
            {
                newrec.CarrTarEquipMatMin = rateRow.Min.Value;
            }

            //sometimes they dont care about accurate must leave by and estimated delivery dates, so max days can be null or 0.
            if (rateRow.MaxDays == null || rateRow.MaxDays.HasValue == false)
            {
                newrec.CarrTarEquipMatMaxDays = 0;
            }
            else
            {
                newrec.CarrTarEquipMatMaxDays = rateRow.MaxDays.Value;
            } 

            //get the rate.
            newrec.Val1 = rateRow.Val1;
            newrec.Val2 = rateRow.Val2;
            newrec.Val3 = rateRow.Val3;
            newrec.Val4 = rateRow.Val4;
            newrec.Val5 = rateRow.Val5;
            newrec.Val6 = rateRow.Val6;
            newrec.Val7 = rateRow.Val7;
            newrec.Val8 = rateRow.Val8;
            newrec.Val9 = rateRow.Val9;
            newrec.CarrTarEquipMatClass = rateRow.Class;


            return sheetRejected;
        }

        public void CleanUp()
        {
            try
            {
                System.IO.File.Delete(fileExists(this.SavedFileName));
            }
            catch (Exception ex) { }
        }

        #endregion
    }

    public class CarrTarImportInterlinePointsCSV : CarTarImportExportBase
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarrTarImportInterlinePointsCSV(DAL.WCFParameters oParameters)
            : base(oParameters)
        {
            this.Errors = new List<Dictionary<string, ArrayList>>();
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrTarImportInterlinePointsCSV";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        public bool ImportSuccess { get; set; }

        #endregion

        #region "Constants & Enums"


        #endregion

        #region " Methods"

        #region "Date Methods"
         
        private bool ImportTempTblInterlinePointsData()
        {
            bool success = false;
            success = NGLFlatFileImport.ImportAllTMPCSVInterlinePoints();
            return success; 
        }
         
        #endregion

        public void SetDeploymentState(bool webOrStandardDeployment)
        {
            if (webOrStandardDeployment)
            {//web deployment 
                //Not supported
                throw new NotImplementedException("Not Supported");
                //this one does not include the bin folder.
                ////string direct = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "ExcelImports\\Target\\";
                ////string bin = "bin\\";
                ////if (direct.Contains(bin))
                ////{
                ////    int startind = direct.IndexOf(bin);
                ////    direct.Remove(startind, 5);
                ////}
                ////this.LocalDirectory = direct;
            }
            else
            {
                this.LocalDirectory = "C:\\Data\\CSVImport\\";
            }
        }

        public bool ImportCSVToTempTbl()
        {
            bool success = false;
            try
            {
                string filePathAndName = LocalDirectory + this.SavedFileName;
                success = ImportCSVToTempTblData(filePathAndName,Util.ImportExportTypes.ImportFromCSVInterline);
                if (success == false)
                {
                    this.AddErrors(LOC.EImportCSVCarTarInterlineFailed.ToString(), new[] { this.SavedFileName, "" });
                }
            }
            catch (Exception ex)
            {
                this.AddErrors(LOC.EImportCSVCarTarInterlineFailed.ToString(), new[] { this.SavedFileName, ex.ToString() });
                success = false;
            }
            return success;
        }

        public bool ImportInterlinePoints()
        {
            bool success = false;
            try
            {
                success = ImportTempTblInterlinePointsData();
                if (success == false)
                {
                    this.AddErrors(LOC.EImportCSVCarTarInterlineFailed.ToString(), new[] { this.SavedFileName, "" });
                }
            }
            catch (Exception ex)
            {
                this.AddErrors(LOC.EImportCSVCarTarInterlineFailed.ToString(), new[] { this.SavedFileName, ex.ToString() });
                success = false;
            }
            return success;
        }
          
        public void CleanUp()
        {
            try
            {
                System.IO.File.Delete(fileExists(this.SavedFileName));
            }
            catch (Exception ex) { }
        }

        #endregion
    }

    public class CarrTarImportNonServicePointsCSV : CarTarImportExportBase
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarrTarImportNonServicePointsCSV(DAL.WCFParameters oParameters)
            : base(oParameters)
        {
            this.Errors = new List<Dictionary<string, ArrayList>>();
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrTarImportNonServicePointsCSV";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        public bool ImportSuccess { get; set; }

        #endregion

        #region "Constants & Enums"


        #endregion

        #region " Methods"

        #region "Date Methods"

        private bool ImportTempTblNonServicePointsData()
        {
            bool success = false;
            success = NGLFlatFileImport.ImportAllTMPCSVNonServicePoints();
            return success;
        }

        #endregion

        public void SetDeploymentState(bool webOrStandardDeployment)
        {
            if (webOrStandardDeployment)
            {//web deployment 
                //Not supported
                throw new NotImplementedException("Not Supported");
                //this one does not include the bin folder.
                ////string direct = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "ExcelImports\\Target\\";
                ////string bin = "bin\\";
                ////if (direct.Contains(bin))
                ////{
                ////    int startind = direct.IndexOf(bin);
                ////    direct.Remove(startind, 5);
                ////}
                ////this.LocalDirectory = direct;
            }
            else
            {
                this.LocalDirectory = "C:\\Data\\CSVImport\\";
            }
        }

        public bool ImportCSVToTempTbl()
        {
            bool success = false;
            try
            {
                string filePathAndName = LocalDirectory + this.SavedFileName;
                success = ImportCSVToTempTblData(filePathAndName, Util.ImportExportTypes.ImportFromCSVNonService);
                if (success == false)
                {
                    this.AddErrors(LOC.EImportCSVCarTarNonServFailed.ToString(), new[] { this.SavedFileName, "" });
                }
            }
            catch (Exception ex)
            {
                this.AddErrors(LOC.EImportCSVCarTarNonServFailed.ToString(), new[] { this.SavedFileName, ex.ToString() });
                success = false;
            }
            return success;
        }

        public bool ImportNonServicePoints()
        {
            bool success = false;
            try
            {
                success = ImportTempTblNonServicePointsData();
                if (success == false)
                {
                    this.AddErrors(LOC.EImportCSVCarTarNonServFailed.ToString(), new[] { this.SavedFileName, "" });
                }
            }
            catch (Exception ex)
            {
                this.AddErrors(LOC.EImportCSVCarTarNonServFailed.ToString(), new[] { this.SavedFileName, ex.ToString() });
                success = false;
            }
            return success;
        }

        public void CleanUp()
        {
            try
            {
                System.IO.File.Delete(fileExists(this.SavedFileName));
            }
            catch (Exception ex) { }
        }

        #endregion
    }

    public class CarrTarCopyContract : CarTarImportExportBase
    {
        #region " Constructors "


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oParameters">Data Access Layer parameters (i.e., WCF parameters).</param>
        public CarrTarCopyContract(DAL.WCFParameters oParameters)
            : base(oParameters)
        {
            this.Errors = new List<Dictionary<string, ArrayList>>();
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "NGL.FM.CarTar.CarrTarImportNonServicePointsCSV";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        public bool CopySuccess { get; set; }

        #endregion

        #region "Methods"

        public  bool CopyContract(int CarrTarControl,  
                               DateTime? EffDateFrom   ,  
                               DateTime? EffDateTo   ,  
                               bool AutoApprove ,  
                               DateTime? IssuedDate ,
                               bool CopyClassXrefData ,
                               bool CopyNoDriveDays ,
                               bool CopyDiscountData ,
                               bool CopyFeeData ,
                               bool CopyInterlinePointData ,
                               bool CopyMinChargeData ,
                               bool CopyMinWeightData ,
                               bool CopyNonServicePointData ,
                               bool CopyMatrixBPData ,
                               bool CopyEquipmentData ,
                               bool CopyEquipmentRateData,
                               bool CopyFuelData ,
                               int newCompControl,
                                string newContractName)
        {

             DTO.GenericResults results = null;
             try
             {


                 results = NGLCarrTarContractData.CopyTariff(CarrTarControl,
                                                     EffDateFrom,
                                                     EffDateTo,
                                                     AutoApprove,
                                                     IssuedDate,
                                                     CopyClassXrefData,
                                                     CopyNoDriveDays,
                                                     CopyDiscountData,
                                                     CopyFeeData,
                                                     CopyInterlinePointData,
                                                     CopyMinChargeData,
                                                     CopyMinWeightData,
                                                     CopyNonServicePointData,
                                                     CopyMatrixBPData,
                                                     CopyEquipmentData,
                                                     CopyEquipmentRateData,
                                                     CopyFuelData,
                                                     newCompControl,
                                                     newContractName);
                 if (results == null || results.Control == 0 || results.ErrNumber != 0)
                 {
                     if (results == null)
                     {
                         this.AddErrors(LOC.EImportCloneFailedEmptyResult.ToString(), new[] { "ErrorNumber: " + results.ErrNumber + " CarrTarControl: " + CarrTarControl });
                         return false;
                     }
                     else
                     {
                         switch (results.ErrNumber)
                         {
                             case 0:
                                 this.AddErrors(LOC.ECopyConFailedUnknown.ToString(), new[] { "ErrorNumber: " + results.ErrNumber + " CarrTarControl: " + CarrTarControl, results.RetMsg });
                                 break;
                             case 1:
                                 this.AddErrors(LOC.ECopyConFailedNoCompany.ToString(), null);
                                 break;
                             case 2:
                                 this.AddErrors(LOC.ECopyConFailedNoTariffFound.ToString(), null);
                                 break;
                             case 3:
                                 this.AddErrors(LOC.ECopyConFailedContractExists.ToString(), null);
                                 break;
                             case 4:
                                 this.AddErrors(LOC.ECopyConFailedNoNameProvided.ToString(), null);
                                 break;
                             default:
                                 this.AddErrors(LOC.ECopyConFailedUnknown.ToString(), new[] { "ErrorNumber: " + results.ErrNumber + " CarrTarControl: " + CarrTarControl, results.RetMsg });
                                 break;
                         } 
                         return false;
                     }
                 }
                 if (results.Control > 0)
                 {
                     //success 
                     return true;
                 }
             }
             catch (Exception ex)
             {
                 this.AddErrors(LOC.EImportCloneFailedUnknownException.ToString(), new[] { "ErrorNumber: " + results.ErrNumber + " CarrTarControl: " + CarrTarControl,  ex.Message });
                 return false;
             }
             return false;
        }

        #endregion

    }

    /// <summary>
    /// This class is used to parse the cell value and give results if the parse was successfull based on the data type expectded.
    /// </summary>
    public class GetCellResult
    {
        private Type ExpectedType { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public object Value { get; set; }
        private IG.Worksheet worksheet {get; set;}
        private string CellRange { get; set; } 
        private string messeageStart { get{ return "Unable to parse cell " + CellRange;}  }

        public GetCellResult(Type expectedType, IG.Worksheet worksheet, string cellRange)
        {
            this.ExpectedType= expectedType;
            this.worksheet =worksheet;
            this.CellRange=cellRange;
        }

        public void ParseValue(){
            if (this.worksheet == null){
                this.Success = false;
                this.Message= messeageStart + ", the worksheet is not available.";
                return;
            }
            if (string.IsNullOrEmpty(this.CellRange)){
                this.Success = false;
                this.Message= messeageStart + ", the cell range provided is not valid.";
                return;
            }
            try 
	        {	      
                bool typeFound  = false; 
		        if (this.ExpectedType == typeof(double))
                {
                    this.Value = Convert.ToDouble(worksheet.GetCell(this.CellRange).Value);
                    this.Success = true;
                    typeFound = true;
                    return;
                }
                if (this.ExpectedType == typeof(string))
                {
                    this.Value = Convert.ToString(worksheet.GetCell(this.CellRange).Value);
                    this.Success = true;
                    typeFound = true; 
                    return;
                }
                if (this.ExpectedType == typeof(int))
                {
                    this.Value = Convert.ToInt32(worksheet.GetCell(this.CellRange).Value);
                    this.Success = true;
                    typeFound = true;
                    return;
                }
                if (this.ExpectedType == typeof(bool))
                {
                    this.Value =  Convert.ToBoolean(worksheet.GetCell(this.CellRange).Value);
                    this.Success = true;
                    typeFound = true;
                    return;
                }
                if (typeFound==false)
                {
                    this.Success = false;
                    this.Message= messeageStart + ", type provided was not valid.";
                    return;
                }
	        }
	        catch (Exception ex)
	        {
		        this.Success = false;
                this.Message= messeageStart + ", could not convert cell value to " + this.ExpectedType + " type. " + ex.Message;		         
	        }

        }

    }
    

}
