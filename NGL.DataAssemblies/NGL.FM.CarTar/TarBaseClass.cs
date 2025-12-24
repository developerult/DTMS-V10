using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Reflection;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;
using Serilog.Core;
using Serilog;
using Destructurama;
using Destructurama.Attributed;
using SerilogTracing;


namespace NGL.FM.CarTar
{
    public abstract class TarBaseClass
    {

        #region " Constructors "
        public TarBaseClass()
            : base()
        {
        }

        #endregion

        #region " Properties"

        public abstract string SourceClass
        {
            get;
            set;
        }
        private ILogger _Logger;
        [NotLogged]
        public ILogger Logger
        {
            get
            {
                if (_Logger == null)
                {
                    _Logger = Log.Logger.ForContext<TarBaseClass>();
                }
                return _Logger;
            }
            set { _Logger = value; }
        }
        private DAL.WCFParameters _Parameters;
        public DAL.WCFParameters Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; }
        }

        private string _ConnectionString = "";
        [NotLogged]
        public string ConnectionString
        {
            get
            {
                if (_ConnectionString.Trim().Length < 5)
                {

                    if ((Parameters != null) && Parameters.ConnectionString.Trim().Length > 5)

                    {
                        _ConnectionString = _Parameters.ConnectionString;

                    }
                    else
                    {
                        _ConnectionString = string.Format("Server={0}; Database={1}; Integrated Security=SSPI", Parameters.DBServer.Trim(), Parameters.Database.Trim());
                    }
                }
                return _ConnectionString;
            }
            set { _ConnectionString = value; }
        }

        private Dictionary<int, DTO.CarrierCont> _dictCarrierConts = null;
        [NotLogged]
        public Dictionary<int, DTO.CarrierCont> DictCarrierConts
        {
            get
            {
                if (_dictCarrierConts == null)
                {
                    // Create a new dictionary.
                    _dictCarrierConts = new Dictionary<int, DTO.CarrierCont>();
                }
                return _dictCarrierConts;
            }
            set { _dictCarrierConts = value; }
        }

        private Dictionary<int, DTO.CarrTarContract> _dictCarrTarContracts = null;
        [NotLogged]
        public Dictionary<int, DTO.CarrTarContract> DictCarrTarContracts
        {
            get
            {
                if (_dictCarrTarContracts == null)
                {
                    // Create a new dictionary.
                    _dictCarrTarContracts = new Dictionary<int, DTO.CarrTarContract>();
                }
                return _dictCarrTarContracts;
            }
            set { _dictCarrTarContracts = value; }
        }

        private Dictionary<int, DAL.LTS.vCarrierQual> _dictCarrierQuals = null;
        [NotLogged]
        public Dictionary<int, DAL.LTS.vCarrierQual> DictCarrierQuals
        {
            get
            {
                if (_dictCarrierQuals == null)
                {
                    // Create a new dictionary.
                    _dictCarrierQuals = new Dictionary<int, DAL.LTS.vCarrierQual>();
                }
                return _dictCarrierQuals;
            }
            set { _dictCarrierQuals = value; }
        }

        private Dictionary<int, List<DTO.CarrTarNoDriveDays>> _dictCarrTarNoDriveDays = null;
        public Dictionary<int, List<DTO.CarrTarNoDriveDays>> DictCarrTarNoDriveDays
        {
            get
            {
                if (_dictCarrTarNoDriveDays == null)
                {
                    // Create a new dictionary.
                    _dictCarrTarNoDriveDays = new Dictionary<int, List<DTO.CarrTarNoDriveDays>>();
                }
                return _dictCarrTarNoDriveDays;
            }
            set { _dictCarrTarNoDriveDays = value; }
        }

        private Dictionary<Tuple<int, string>, double> _dictCompanyLevelParameterValue = null;
        [NotLogged]
        public Dictionary<Tuple<int, string>, double> DictCompanyLevelParameterValue
        {
            get
            {
                if (_dictCompanyLevelParameterValue == null)
                {
                    // Create a new dictionary.
                    _dictCompanyLevelParameterValue = new Dictionary<Tuple<int, string>, double>();
                }
                return _dictCompanyLevelParameterValue;
            }
            set { _dictCompanyLevelParameterValue = value; }
        }

        private Dictionary<int, List<DAL.LTS.udfGetDefaultClassCodesResult>> _dictDefaultClassCodes = null;
        [NotLogged]
        public Dictionary<int, List<DAL.LTS.udfGetDefaultClassCodesResult>> DictDefaultClassCodes
        {
            get
            {
                if (_dictDefaultClassCodes == null)
                {
                    // Create a new dictionary.
                    _dictDefaultClassCodes = new Dictionary<int, List<DAL.LTS.udfGetDefaultClassCodesResult>>();
                }
                return _dictDefaultClassCodes;
            }
            set { _dictDefaultClassCodes = value; }
        }

        private bool _FromAPI = false;
        public bool FromAPI
        {
            get { return _FromAPI; }
            set { _FromAPI = value; }
        }


        #endregion

        #region " NGL Data Object Properties"

        private DAL.NGLSystemDataProvider _NGLSystemData;
        public DAL.NGLSystemDataProvider NGLSystemData
        {
            get
            {
                if (_NGLSystemData == null)
                {
                    if ((Parameters != null))
                    {
                        bool validateAccess = Parameters.ValidateAccess;
                        Parameters.ValidateAccess = false;
                        _NGLSystemData = new DAL.NGLSystemDataProvider(Parameters);
                        Parameters.ValidateAccess = validateAccess;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Parameters Are Required");
                    }

                }
                return _NGLSystemData;
            }
            set
            {
                _NGLSystemData = value;
            }
        }

        private DAL.NGLBookData _NGLBookData;
        [NotLogged]
        public DAL.NGLBookData NGLBookData
        {
            get { if (_NGLBookData == null) { _NGLBookData = new DAL.NGLBookData(Parameters); } return _NGLBookData; }
            set { _NGLBookData = value; }
        }

        private DAL.NGLBookItemData _NGLBookItemData;
        [NotLogged]
        public DAL.NGLBookItemData NGLBookItemData
        {
            get { if (_NGLBookItemData == null) { _NGLBookItemData = new DAL.NGLBookItemData(Parameters); } return _NGLBookItemData; }
            set { _NGLBookItemData = value; }
        }

        private DAL.NGLBookPackage _NGLBookPackageData;
        /// <summary>
        /// reference to instance of NGL Book Package DAL class object
        /// </summary>
        /// <remarks>
        /// Modified by RHR for v-8.2.0.118 on 09/06/2019 for bookpackage logic
        /// </remarks>
        [NotLogged]
        public DAL.NGLBookPackage NGLBookPackageData
        {
            get
            {
                if (_NGLBookPackageData == null)
                {
                    _NGLBookPackageData = new DAL.NGLBookPackage(Parameters);
                }
                return _NGLBookPackageData;
            }
            set { _NGLBookPackageData = value; }
        }

        private DAL.NGLBookTrackData _NGLBookTrackData;
        [NotLogged]
        public DAL.NGLBookTrackData NGLBookTrackData
        {
            get { if (_NGLBookTrackData == null) { _NGLBookTrackData = new DAL.NGLBookTrackData(Parameters); } return _NGLBookTrackData; }
            set { _NGLBookTrackData = value; }
        }

        private DAL.NGLBookNoteData _NGLBookNoteData;
        [NotLogged]
        public DAL.NGLBookNoteData NGLBookNoteData
        {
            get { if (_NGLBookNoteData == null) { _NGLBookNoteData = new DAL.NGLBookNoteData(Parameters); } return _NGLBookNoteData; }
            set { _NGLBookNoteData = value; }
        }

        private DAL.NGLBookLoadData _NGLBookLoadData;
        [NotLogged]
        public DAL.NGLBookLoadData NGLBookLoadData
        {
            get { if (_NGLBookLoadData == null) { _NGLBookLoadData = new DAL.NGLBookLoadData(Parameters); } return _NGLBookLoadData; }
            set { _NGLBookLoadData = value; }
        }

        private DAL.NGLBookRevenueData _NGLBookRevenueData;
        [NotLogged]
        public DAL.NGLBookRevenueData NGLBookRevenueData
        {
            get { if (_NGLBookRevenueData == null) { _NGLBookRevenueData = new DAL.NGLBookRevenueData(Parameters); } return _NGLBookRevenueData; }
            set { _NGLBookRevenueData = value; }
        }

        private DAL.NGLCarrTarEquipMatData _NGLCarrTarEquipMatData;
        [NotLogged]
        public DAL.NGLCarrTarEquipMatData NGLCarrTarEquipMatData
        {
            get { if (_NGLCarrTarEquipMatData == null) { _NGLCarrTarEquipMatData = new DAL.NGLCarrTarEquipMatData(Parameters); } return _NGLCarrTarEquipMatData; }
            set { _NGLCarrTarEquipMatData = value; }
        }
        private DAL.NGLCarrTarMatBPData _NGLCarrTarMatBPData;
        [NotLogged]
        public DAL.NGLCarrTarMatBPData NGLCarrTarMatBPData
        {
            get { if (_NGLCarrTarMatBPData == null) { _NGLCarrTarMatBPData = new DAL.NGLCarrTarMatBPData(Parameters); } return _NGLCarrTarMatBPData; }
            set { _NGLCarrTarMatBPData = value; }
        }

        private DAL.NGLBookCarrierData _NGLBookCarrierData;
        [NotLogged]
        public DAL.NGLBookCarrierData NGLBookCarrierData
        {
            get { if (_NGLBookCarrierData == null) { _NGLBookCarrierData = new DAL.NGLBookCarrierData(Parameters); } return _NGLBookCarrierData; }
            set { _NGLBookCarrierData = value; }
        }

        private DAL.NGLBookFinancialData _NGLBookFinancialData;
        [NotLogged]
        public DAL.NGLBookFinancialData NGLBookFinancialData
        {
            get { if (_NGLBookFinancialData == null) { _NGLBookFinancialData = new DAL.NGLBookFinancialData(Parameters); } return _NGLBookFinancialData; }
            set { _NGLBookFinancialData = value; }
        }

        private DAL.NGLBookLoadDetailData _NGLBookLoadDetailData;
        [NotLogged]
        public DAL.NGLBookLoadDetailData NGLBookLoadDetailData
        {
            get { if (_NGLBookLoadDetailData == null) { _NGLBookLoadDetailData = new DAL.NGLBookLoadDetailData(Parameters); } return _NGLBookLoadDetailData; }
            set { _NGLBookLoadDetailData = value; }
        }

        private DAL.NGLBookFeeData _NGLBookFeeData;
        [NotLogged]
        public DAL.NGLBookFeeData NGLBookFeeData
        {
            get { if (_NGLBookFeeData == null) { _NGLBookFeeData = new DAL.NGLBookFeeData(Parameters); } return _NGLBookFeeData; }
            set { _NGLBookFeeData = value; }
        }

        private DAL.NGLCarrierData _NGLCarrierData;
        [NotLogged]
        public DAL.NGLCarrierData NGLCarrierData
        {
            get { if (_NGLCarrierData == null) { _NGLCarrierData = new DAL.NGLCarrierData(Parameters); } return _NGLCarrierData; }
            set { _NGLCarrierData = value; }
        }

        private DAL.NGLCarrierEquipCodeData _NGLCarrierEquipCodeData;
        [NotLogged]
        public DAL.NGLCarrierEquipCodeData NGLCarrierEquipCodeData
        {
            get { if (_NGLCarrierEquipCodeData == null) { _NGLCarrierEquipCodeData = new DAL.NGLCarrierEquipCodeData(Parameters); } return _NGLCarrierEquipCodeData; }
            set { _NGLCarrierEquipCodeData = value; }
        }

        private DAL.NGLCarrierFuelData _NGLCarrierFuelData;
        [NotLogged]
        public DAL.NGLCarrierFuelData NGLCarrierFuelData
        {
            get { if (_NGLCarrierFuelData == null) { _NGLCarrierFuelData = new DAL.NGLCarrierFuelData(Parameters); } return _NGLCarrierFuelData; }
            set { _NGLCarrierFuelData = value; }
        }

        private DAL.NGLCarrierFuelAddendumData _NGLCarrierFuelAddendumData;
        [NotLogged]
        public DAL.NGLCarrierFuelAddendumData NGLCarrierFuelAddendumData
        {
            get { if (_NGLCarrierFuelAddendumData == null) { _NGLCarrierFuelAddendumData = new DAL.NGLCarrierFuelAddendumData(Parameters); } return _NGLCarrierFuelAddendumData; }
            set { _NGLCarrierFuelAddendumData = value; }
        }

        private DAL.NGLCarrierFuelAdExData _NGLCarrierFuelAdExData;
        [NotLogged]
        public DAL.NGLCarrierFuelAdExData NGLCarrierFuelAdExData
        {
            get { if (_NGLCarrierFuelAdExData == null) { _NGLCarrierFuelAdExData = new DAL.NGLCarrierFuelAdExData(Parameters); } return _NGLCarrierFuelAdExData; }
            set { _NGLCarrierFuelAdExData = value; }
        }

        private DAL.NGLCarrierFuelAdRateData _NGLCarrierFuelAdRateData;
        [NotLogged]
        public DAL.NGLCarrierFuelAdRateData NGLCarrierFuelAdRateData
        {
            get { if (_NGLCarrierFuelAdRateData == null) { _NGLCarrierFuelAdRateData = new DAL.NGLCarrierFuelAdRateData(Parameters); } return _NGLCarrierFuelAdRateData; }
            set { _NGLCarrierFuelAdRateData = value; }
        }

        private DAL.NGLCarrierFuelStateData _NGLCarrierFuelStateData;
        [NotLogged]
        public DAL.NGLCarrierFuelStateData NGLCarrierFuelStateData
        {
            get { if (_NGLCarrierFuelStateData == null) { _NGLCarrierFuelStateData = new DAL.NGLCarrierFuelStateData(Parameters); } return _NGLCarrierFuelStateData; }
            set { _NGLCarrierFuelStateData = value; }
        }

        private DAL.NGLCarrFeeData _NGLCarrFeeData;
        [NotLogged]
        public DAL.NGLCarrFeeData NGLCarrFeeData
        {
            get { if (_NGLCarrFeeData == null) { _NGLCarrFeeData = new DAL.NGLCarrFeeData(Parameters); } return _NGLCarrFeeData; }
            set { _NGLCarrFeeData = value; }
        }

        private DAL.NGLCarrTarContractData _NGLCarrTarContractData;
        [NotLogged]
        public DAL.NGLCarrTarContractData NGLCarrTarContractData
        {
            get { if (_NGLCarrTarContractData == null) { _NGLCarrTarContractData = new DAL.NGLCarrTarContractData(Parameters); } return _NGLCarrTarContractData; }
            set { _NGLCarrTarContractData = value; }
        }


        private DAL.NGLCarrierContData _NGLCarrierContData;
        [NotLogged]
        public DAL.NGLCarrierContData NGLCarrierContData
        {
            get { if (_NGLCarrierContData == null) { _NGLCarrierContData = new DAL.NGLCarrierContData(Parameters); } return _NGLCarrierContData; }
            set { _NGLCarrierContData = value; }
        }


        private DAL.NGLCarrierTariffNoDriveDays _NGLCarrierTariffNoDriveDays;
        [NotLogged]
        public DAL.NGLCarrierTariffNoDriveDays NGLCarrierTariffNoDriveDays
        {
            get { if (_NGLCarrierTariffNoDriveDays == null) { _NGLCarrierTariffNoDriveDays = new DAL.NGLCarrierTariffNoDriveDays(Parameters); } return _NGLCarrierTariffNoDriveDays; }
            set { _NGLCarrierTariffNoDriveDays = value; }
        }
        private DAL.NGLCarrTarFeeData _NGLCarrTarFeeData;
        [NotLogged]
        public DAL.NGLCarrTarFeeData NGLCarrTarFeeData
        {
            get { if (_NGLCarrTarFeeData == null) { _NGLCarrTarFeeData = new DAL.NGLCarrTarFeeData(Parameters); } return _NGLCarrTarFeeData; }
            set { _NGLCarrTarFeeData = value; }
        }

        private DAL.NGLCarrTarEquipData _NGLCarrTarEquipData;
        [NotLogged]
        public DAL.NGLCarrTarEquipData NGLCarrTarEquipData
        {
            get { if (_NGLCarrTarEquipData == null) { _NGLCarrTarEquipData = new DAL.NGLCarrTarEquipData(Parameters); } return _NGLCarrTarEquipData; }
            set { _NGLCarrTarEquipData = value; }
        }

        private DAL.NGLCompData _NGLCompData;
        [NotLogged]
        public DAL.NGLCompData NGLCompData
        {
            get { if (_NGLCompData == null) { _NGLCompData = new DAL.NGLCompData(Parameters); } return _NGLCompData; }
            set { _NGLCompData = value; }
        }

        private DAL.NGLCompParameterData _NGLCompParameterData;
        [NotLogged]
        public DAL.NGLCompParameterData NGLCompParameterData
        {
            get { if (_NGLCompParameterData == null) { _NGLCompParameterData = new DAL.NGLCompParameterData(Parameters); } return _NGLCompParameterData; }
            set { _NGLCompParameterData = value; }
        }

        private DAL.NGLCompContData _NGLCompContData;
        [NotLogged]
        public DAL.NGLCompContData NGLCompContData
        {
            get { if (_NGLCompContData == null) { _NGLCompContData = new DAL.NGLCompContData(Parameters); } return _NGLCompContData; }
            set { _NGLCompContData = value; }
        }

        private DAL.NGLBatchProcessDataProvider _NGLBatchProcessData;
        [NotLogged]
        public DAL.NGLBatchProcessDataProvider NGLBatchProcessData
        {
            get { if (_NGLBatchProcessData == null) { _NGLBatchProcessData = new DAL.NGLBatchProcessDataProvider(Parameters); } return _NGLBatchProcessData; }
            set { _NGLBatchProcessData = value; }
        }

        private DAL.NGLLaneData _NGLLaneData;
        [NotLogged]
        public DAL.NGLLaneData NGLLaneData
        {
            get { if (_NGLLaneData == null) { _NGLLaneData = new DAL.NGLLaneData(Parameters); } return _NGLLaneData; }
            set { _NGLLaneData = value; }
        }

        private DAL.NGLLaneFeeData _NGLLaneFeeData;
        [NotLogged]
        public DAL.NGLLaneFeeData NGLLaneFeeData
        {
            get { if (_NGLLaneFeeData == null) { _NGLLaneFeeData = new DAL.NGLLaneFeeData(Parameters); } return _NGLLaneFeeData; }
            set { _NGLLaneFeeData = value; }
        }

        private DAL.NGLtblAccessorialData _NGLtblAccessorialData;
        [NotLogged]
        public DAL.NGLtblAccessorialData NGLtblAccessorialData
        {
            get { if (_NGLtblAccessorialData == null) { _NGLtblAccessorialData = new DAL.NGLtblAccessorialData(Parameters); } return _NGLtblAccessorialData; }
            set { _NGLtblAccessorialData = value; }
        }

        private DAL.NGLFlatFileImport _NGLFlatFileImport;
        [NotLogged]
        public DAL.NGLFlatFileImport NGLFlatFileImport
        {
            get { if (_NGLFlatFileImport == null) { _NGLFlatFileImport = new DAL.NGLFlatFileImport(Parameters); } return _NGLFlatFileImport; }
            set { _NGLFlatFileImport = value; }
        }

        private DAL.NGLLookupDataProvider _NGLLookupData;
        [NotLogged]
        public DAL.NGLLookupDataProvider NGLLookupData
        {
            get
            {
                if (_NGLLookupData == null)
                {
                    if ((Parameters != null))
                    {
                        _NGLLookupData = new DAL.NGLLookupDataProvider(Parameters);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Parameters Are Required");
                    }

                }
                return _NGLLookupData;
            }
            set
            {
                _NGLLookupData = value;
            }
        }

        private DAL.NGLCompParameterRefSystemData _NGLCompParameterRefSystemData;
        [NotLogged]
        public DAL.NGLCompParameterRefSystemData NGLCompParameterRefSystemData
        {
            get
            {
                if (_NGLCompParameterRefSystemData == null)
                {
                    if ((Parameters != null))
                    {
                        _NGLCompParameterRefSystemData = new DAL.NGLCompParameterRefSystemData(Parameters);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Parameters Are Required");
                    }

                }
                return _NGLCompParameterRefSystemData;
            }
            set
            {
                _NGLCompParameterRefSystemData = value;
            }
        }

        #endregion

        #region " Methods"

        protected DAL.NDPBaseClass NDPBaseClassFactory(string source)
        {
            Logger.Verbose("TarBaseClass.NDPBaseClassFactory - Source:{0}", source);
            if ((Parameters != null))
            {
                try
                {
                    string typename = "Ngl.FreightMaster.Data." + source;
                    Logger.Verbose("TarBaseClass.NDPBaseClassFactory - TypeName:{0}", typename);
                    Type t = typeof(Ngl.FreightMaster.Data.NDPBaseClass).Assembly.GetType(typename);

                    DAL.NDPBaseClass newC = Activator.CreateInstance(t, new object[] { Parameters }) as DAL.NDPBaseClass;
                    Logger.Verbose("TarBaseClass.NDPBaseClassFactory - NewC:{0}", newC);
                    if (newC == null)
                    {
                        //throw new System.InvalidCastException("The class " + source + " is not a valid NDPBaseClass");
                        Logger.Error("TarBaseClass.NDPBaseClassFactory - The class {0} is not a valid NDPBaseClass", source);
                    }

                    return newC;

                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "TarBaseClass.NDPBaseClassFactory - FaultException");
                    //throw;
                }
                catch (System.NullReferenceException ex)
                {
                    Logger.Error(ex, "TarBaseClass.NDPBaseClassFactory - NullReferenceException");
                    //throw new System.InvalidCastException("The class " + source + " is not a valid NDPBaseClass");
                }
                catch (System.ArgumentNullException ex)
                {
                    Logger.Error(ex, "TarBaseClass.NDPBaseClassFactory - ArgumentNullException");
                    //throw new System.InvalidCastException("The class " + source + " cannot be found or is not a valid NDPBaseClass");
                }
                catch (System.MissingMethodException ex)
                {
                    Logger.Error(ex, "TarBaseClass.NDPBaseClassFactory - MissingMethodException");
                    //throw new System.InvalidCastException("The class " + source + " does not support the required constructor.  It may not be a valid NDPBaseClass");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "TarBaseClass.NDPBaseClassFactory - Exception");
                    //throw;
                }

            }
            else
            {
                Logger.Error("TarBaseClass.NDPBaseClassFactory - Parameters are required");
                //throw new System.InvalidOperationException("Parameters Are Required");
            }
            return null;
        }




        protected void populateSampleParameterData()
        {
            Logger.Fatal("TarBaseClass.populateSampleParameterData - Parameters:{0}", Parameters);
            throw new System.ApplicationException("Invalid WCF Parameter Data in constructor");

            //if (Parameters == null) { Parameters = new DAL.WCFParameters(); }
            //Parameters.Database = "NGLMAS2013DEV";
            //Parameters.DBServer = "NGLRDP06D";
            //Parameters.UserName = "ngl\rramsey";
            //Parameters.WCFAuthCode = "WCFDEV";
            //Parameters.WCFServiceURL = "http://nglwcfdev70.nextgeneration.com";
            //Parameters.ConnectionString = "Data Source=nglrdp06d;Initial Catalog=NGLMAS2013DEV;User ID=nglweb;Password=5529";
            //Parameters.ProcedureControl = 0;
            //Parameters.ProcedureName = "";
            //Parameters.FormControl = 0;
            //Parameters.FormName = "";
        }

        public void SaveAppError(string Message)
        {
            try
            {
                Logger.Error("TarBaseClass.SaveAppError - {0}", Message);
                // NGLSystemData.CreateAppErrorByMessage(Message, Parameters.UserName);
            }
            catch (Exception ex)
            {
                //we ignore all errors while saving application error data
            }

        }


        public void SaveSysError(string Message, string errorProcedure, string record = "", int errorNumber = 0, int errorSeverity = 0, int errorState = 0, int errorLineNber = 0)
        {

            try
            {
                Logger.Error("Message:{Message} record:{record} errorNumber:{errorNumber} errorLineNumber:{3}", Message, record, errorNumber, errorLineNber);
                // NGLSystemData.CreateSystemErrorByMessage(Message, Parameters.UserName, errorProcedure, record, errorNumber, errorSeverity, errorState, errorLineNber);
            }
            catch (Exception ex)
            {
                //we ignore all errors while saving application error data
            }


        }

        /// <summary>
        /// When calling executeNGLStoredProcedure the following parameters
        /// are required by the stored procedure but are implemented in the WCF service
        /// so they should not be included in the ProcPars array:
        /// @UserName NVARCHAR (100), @RetMsg NVARCHAR (4000) OUTPUT, @ErrNumber INT OUTPUT
        /// </summary>
        /// <param name="BatchName"></param>
        /// <param name="ProcName"></param>
        /// <param name="ProcPars"></param>
        /// <returns></returns>
        protected bool executeNGLStoredProcedure(string BatchName, string ProcName, DTO.NGLStoredProcedureParameter[] ProcPars)
        {
            try
            {
                return NGLBatchProcessData.executeNGLStoredProcedure(BatchName, ProcName, ProcPars);

            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                Logger.Error(sqlEx, "TarBaseClass.executeNGLStoredProcedure - {0}", errMsg);
                SaveSysError(errMsg, sourcePath("executeNGLStoredProcedure"), ProcName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "TarBaseClass.executeNGLStoredProcedure - {0}", ex.Message);
                //throw;
            }
            return false;
        }

        /// <summary>
        /// When calling executeNGLStoredProcedure the following parameters
        /// are required by the stored procedure but are implemented in the WCF service
        /// so they should not be included in the ProcPars array:
        /// @UserName NVARCHAR (100), @RetMsg NVARCHAR (4000) OUTPUT, @ErrNumber INT OUTPUT
        /// </summary>
        /// <param name="BatchName"></param>
        /// <param name="ProcName"></param>
        /// <param name="ProcPars"></param>
        /// <returns></returns>
        protected bool executeNGLStoredProcedure(string ProcName, List<DTO.NGLStoredProcedureParameter> ProcPars)
        {
            using (var operation = Logger.StartActivity("executeNGLStoredProcedure(ProcName: {ProcName}, ProcPars: {ProcPars})", ProcName, ProcPars))
            {
                try
                {
                    return NGLBatchProcessData.executeNGLStoredProcedure(ProcName, ProcName, ProcPars.ToArray());

                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {

                    Logger.Error(sqlEx, "TarBaseClass.executeNGLStoredProcedure");
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("executeNGLStoredProcedure"), ProcName);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        protected bool executeSQL(string strSQL)
        {
            try
            {
                return NGLBatchProcessData.executeSQL(strSQL);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());

                SaveSysError(errMsg, sourcePath("executeSQL"), strSQL);
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

        protected int getScalarInteger(string strSQL)
        {
            int intRet = 0;
            try
            {
                intRet = NGLBatchProcessData.returnScalarInteger(strSQL);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());

                SaveSysError(errMsg, sourcePath("getScalarInteger"), strSQL);
            }
            catch (Exception ex)
            {
                throw;
            }
            return intRet;
        }

        protected string getScalarString(string strSQL)
        {
            string strRet = "";
            try
            {
                strRet = NGLBatchProcessData.returnScalarString(strSQL);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());

                SaveSysError(errMsg, sourcePath("getScalarString"), strSQL);
            }
            catch (Exception ex)
            {
                throw;
            }
            return strRet;
        }

        protected string sourcePath(string caller)
        {
            return string.Concat(SourceClass, '.', caller);
        }

        /// <summary>
        /// This method produces a deep copy of the passed in object.
        /// Notes: 
        /// 1. This method only works if all the class types contained in the object
        /// have default constructors.  Any classes that normally have parameters passed
        /// into their constructor, will have to deal with the parameters after the deep
        /// copy has been made.
        /// 2. This method only works if the object contains classes that are in the 
        /// current assembly or the Ngl.FreightMaster.Data assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Object cannot be null");
            lock (obj)
            {
                return (T)Process(obj);
            }
        }

        private static object Process(object obj)
        {
            if (obj == null)
                return null;

            Type type = obj.GetType();

            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                String arrayElementType = type.FullName.Replace("[]", string.Empty);
                Type elementType = Type.GetType(arrayElementType);
                if (elementType == null)
                {
                    // See if the type is in the DTO assembly.
                    elementType = Type.GetType(arrayElementType + ",Ngl.FreightMaster.Data");
                }

                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(Process(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {
                object toret = Activator.CreateInstance(obj.GetType());
                FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                            BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(obj);
                    if (fieldValue == null)
                        continue;
                    field.SetValue(toret, Process(fieldValue));
                }
                return toret;
            }
            else
                throw new ArgumentException("Unknown type");
        }

        /// <summary>
        /// Creates a new CarriersByCost record and updates the values based on the selected tariff, the last stop or the entier load as required
        /// </summary>
        /// <param name="currentCarrierTariffsPivot"></param>
        /// <param name="ratedLoad"></param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.100 4/20/2016
        /// Added InterlinePoint flag to results
        /// Modified by RHR v-7.0.5.102 9/30/20126  
        ///   Hot fix for invalid values when using line haul or when transit days are zero
        ///   the default Expected Delivery Date should be the
        ///   required date not the load date
        /// </remarks>
        protected DTO.CarriersByCost updateCarriersByCost(ref DTO.CarriersByCost carriersByCost, DTO.CarrierTariffsPivot currentCarrierTariffsPivot, Load ratedLoad)
        {
            DTO.CarriersByCost[] pivots = null;
            using (Logger.StartActivity("{CarrierName}, calculating cost with tariff {TariffName}:{RevisionNumber}, {EquipmentType} - Matrix: {MatrixName}, DefaultWgt: {DefaultWgt}, Distance: {AllowDistanceRate}, Flat: {AllowFlatRate}, LTL: {AllowLTLRate}, UOM: {AllowUOMRate}",
                                         currentCarrierTariffsPivot.CarrierName,
                                         currentCarrierTariffsPivot.CarrTarName,
                                         currentCarrierTariffsPivot.CarrTarRevisionNumber,
                                         currentCarrierTariffsPivot.CarrTarEquipName,
                                         currentCarrierTariffsPivot.CarrTarEquipMatName,
                                         currentCarrierTariffsPivot.CarrTarDefWgt,
                                         currentCarrierTariffsPivot.AllowDistanceRate,
                                         currentCarrierTariffsPivot.AllowFlatRate,
                                         currentCarrierTariffsPivot.AllowLTLRate,
                                         currentCarrierTariffsPivot.AllowUOMRate))
            {
                DTO.BookRevenue ratedStop = ratedLoad.RatedBookRevenue;

                carriersByCost.BookAllowInterlinePoints = ratedStop.BookAllowInterlinePoints;
                carriersByCost.BookCarrTarControl = ratedStop.BookCarrTarControl;
                carriersByCost.BookCarrTarEquipControl = currentCarrierTariffsPivot.CarrTarEquipControl;
                //For LTL (Class Type Rates) we could have multiple Matrix Rates for each order because each item 
                //May have a differrent rate.  so the values of 
                //  CarrTarEquipMatControl, 
                //  BookCarrTarEquipMatDetControl, 
                //  and BookCarrTarEquipMatDetID 
                //may not be 100% accurate
                carriersByCost.BookCarrTarEquipMatControl = currentCarrierTariffsPivot.CarrTarEquipMatControl;
                carriersByCost.BookCarrTarEquipMatDetControl = ratedStop.BookCarrTarEquipMatDetControl;
                carriersByCost.BookCarrTarEquipMatDetID = 0;  //reserved for future use
                carriersByCost.BookCarrTarEquipMatDetValue = 0; //reserved for future use                
                carriersByCost.BookCarrTarEquipMatName = currentCarrierTariffsPivot.CarrTarEquipMatName;
                carriersByCost.BookCarrTarEquipName = currentCarrierTariffsPivot.CarrTarEquipName;
                carriersByCost.BookCarrTarName = currentCarrierTariffsPivot.CarrTarName;
                carriersByCost.BookCarrTarRevisionNumber = currentCarrierTariffsPivot.CarrTarRevisionNumber;
                carriersByCost.BookCarrTruckControl = ratedStop.BookCarrTruckControl;
                carriersByCost.setModeType(currentCarrierTariffsPivot.CarrTarTariffModeTypeControl);
                carriersByCost.BookRevDiscount = ratedLoad.TotalDiscount;
                carriersByCost.BookRevLineHaul = ratedLoad.TotalLineHaul;
                carriersByCost.BookRevLoadMiles = ratedLoad.TotalMiles;
                carriersByCost.BookTransType = ratedStop.BookTransType;


                Logger.Information("{CarrierName} - Check if TariffMatrixRateType ({CarrTarEquipMatTarRateTypeControl}) is DistanceM {DistanceM} or DistanceK {DistanceK}",
                                    currentCarrierTariffsPivot.CarrierName,
                                                      currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl,
                                                      (int)DAL.Utilities.TariffRateType.DistanceM,
                                                      (int)DAL.Utilities.TariffRateType.DistanceK);

                if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.DistanceM) ||
                    (currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.DistanceK))
                {
                    carriersByCost.CarrierMileRate = ratedLoad.CarrierMileRate;     // Distance rate per mile from pivot table if using distance rates.
                    Logger.Information("{CarrierName} Setting CarrierMileRate: {CarrierMileRate}", carriersByCost.CarrierName, carriersByCost.CarrierMileRate);
                }


                Logger.Information("{CarrierName} - Check if MatrixRateType {MatrixRateType} is (ClassRate({ClassRate}) || CzarLite({CzarLite}) || UnitOfMeasure({UnitOfMeasure}) AND (MatrixBracketType {MatrixBracketType} is Lbs ({LbsBracketType}) or Cwt ({CwtBracketType})",
                    currentCarrierTariffsPivot.CarrierName,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl,
                    (int)DAL.Utilities.TariffRateType.ClassRate,
                    (int)DAL.Utilities.TariffRateType.CzarLite,
                    (int)DAL.Utilities.TariffRateType.UnitOfMeasure,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl,
                    (int)DAL.Utilities.BracketType.Lbs,
                    (int)DAL.Utilities.BracketType.Cwt);

                if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.ClassRate) ||
                    (currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.CzarLite) ||
                        ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.UnitOfMeasure) &&
                            ((currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl == (int)DAL.Utilities.BracketType.Lbs) ||
                             (currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl == (int)DAL.Utilities.BracketType.Cwt))))
                            {
                                carriersByCost.CarrierLbsRate = ratedLoad.CarrierLbsRate;       // Averate LTL rate per pound from the pivot table data if using class rates.
                                Logger.Information("{CarrierName} choosing CarrierLbsRate {CarrierLbsRate}", currentCarrierTariffsPivot.CarrierName, carriersByCost.CarrierLbsRate);
                            }

                Logger.Information("{CarrierName} - Check if TariffMatrixRateType ({CarrTarEquipMatTarRateTypeControl}) is UnitOfMeasure {UnitOfMeasure} AND MatrixBracketType {MatrixBracketType} is Volume {VolumeBracketType}",
                    currentCarrierTariffsPivot.CarrierName,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl,
                    (int)DAL.Utilities.TariffRateType.UnitOfMeasure,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl,
                    (int)DAL.Utilities.BracketType.Volume);

                if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.UnitOfMeasure) &&
                    (currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl == (int)DAL.Utilities.BracketType.Volume))
                {
                    carriersByCost.CarrierCubeRate = ratedLoad.CarrierCubeRate;     // Average rate per cube (volume) from the pivot table data if using Volume unit-of-measure rate.
                    Logger.Information("{CarrierName} choosing CarrierCubeRate: {CarrierCubeRate}", currentCarrierTariffsPivot.CarrierName, carriersByCost.CarrierCubeRate);
                }


                Logger.Information("{CarrierName} - Check if TariffMatrixRateType ({CarrTarEquipMatTarRateTypeControl}) is UnitOfMeasure {UnitOfMeasure} AND MatrixBracketType {MatrixBracketType} is Pallets {PalletsBracketType}",
                    currentCarrierTariffsPivot.CarrierName,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl,
                    (int)DAL.Utilities.TariffRateType.UnitOfMeasure,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl,
                    (int)DAL.Utilities.BracketType.Pallets);

                if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.UnitOfMeasure) &&
                    (currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl == (int)DAL.Utilities.BracketType.Pallets))
                {
                    carriersByCost.CarrierPltRate = ratedLoad.CarrierPltRate;       // Average rate per pallet from the pivot table data if using Pallet unit-of-measure rate.
                    Logger.Information("{CarrierName} choosing CarrierPltRate: {CarrierPltRate}", currentCarrierTariffsPivot.CarrierName, carriersByCost.CarrierPltRate);
                }


                Logger.Information("{CarrierName} - Check if TariffMatrixRateType ({CarrTarEquipMatTarRateTypeControl}) is UnitOfMeasure {UnitOfMeasure} AND MatrixBracketType {MatrixBracketType} is FlatPallet {FlatPalletBracketType}",
                    currentCarrierTariffsPivot.CarrierName,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl,
                    (int)DAL.Utilities.TariffRateType.UnitOfMeasure,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl,
                    (int)DAL.Utilities.BracketType.FlatPallet);

                if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.UnitOfMeasure) &&
                   (currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl == (int)DAL.Utilities.BracketType.FlatPallet))
                {
                    carriersByCost.CarrierPltRate = ratedLoad.CarrierPltRate;       // Average rate per pallet from the pivot table data if using Pallet unit-of-measure rate.
                    Logger.Information("{CarrierName} Choose By Flat Pallet  CarrierPltRate: {CarrierPltRate}", currentCarrierTariffsPivot.CarrierName, carriersByCost.CarrierPltRate);
                }


                Logger.Information("{CarrierName} - Check if TariffMatrixRateType ({CarrTarEquipMatTarRateTypeControl}) is UnitOfMeasure {UnitOfMeasure} AND MatrixBracketType {MatrixBracketType} is Quantity {QuantityBracketType}",
                    currentCarrierTariffsPivot.CarrierName,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl,
                    (int)DAL.Utilities.TariffRateType.UnitOfMeasure,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl,
                    (int)DAL.Utilities.BracketType.Quantity);

                if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.UnitOfMeasure) &&
                    (currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl == (int)DAL.Utilities.BracketType.Quantity))
                {
                    carriersByCost.CarrierCaseRate = ratedLoad.CarrierCaseRate;     // Average rate per case (quantity) from the pivot table data if using Quantity unit-of-measure rate.
                    Logger.Information("{CarrierName} Choose By Quantity CarrierCaseRate: {CarrierCaseRate}", currentCarrierTariffsPivot.CarrierName, carriersByCost.CarrierCaseRate);
                }

                carriersByCost.CarrierControl = currentCarrierTariffsPivot.CarrierControl;
                carriersByCost.CarrierCost = ratedLoad.SumOfEstTotalCost;
                carriersByCost.CarrierEquipment = currentCarrierTariffsPivot.CarrTarEquipName;
                carriersByCost.CarrierMinCost = (decimal)(currentCarrierTariffsPivot.CarrTarEquipMatMin ?? 0);
                carriersByCost.CarrierName = currentCarrierTariffsPivot.CarrierName;
                carriersByCost.CarrierNumber = currentCarrierTariffsPivot.CarrierNumber;
                carriersByCost.CarrierRateExpires = currentCarrierTariffsPivot.CarrTarEffDateTo;
                Logger.Information("{CarrierName} - Equipment: {Equipment}, CarrierRateExpires: {CarrierRateExpires}", currentCarrierTariffsPivot.CarrierName, currentCarrierTariffsPivot.CarrTarEquipName, carriersByCost.CarrierRateExpires);

                Logger.Information("{CarrierName} - Check if TariffMatrixRateType ({CarrTarEquipMatTarRateTypeControl}) is DistanceM {DistanceM} or DistanceK {DistanceK} OR FlatRate {FlatRate}",
                    currentCarrierTariffsPivot.CarrierName,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl,
                    (int)DAL.Utilities.TariffRateType.DistanceM,
                    (int)DAL.Utilities.TariffRateType.DistanceK,
                    (int)DAL.Utilities.TariffRateType.FlatRate);

                if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.DistanceM) ||
                    (currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.DistanceK) ||
                    (currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.FlatRate))
                {

                    carriersByCost.CarrierTLCost = ratedLoad.TotalLineHaul - ratedLoad.TotalDiscount;

                    Logger.Information("{CarrierName} is using Distance or FlatRate, setting TLCost: {CarrierTLCost} = TotalLineHaul({TotalLineHaul}) - TotalDiscount({TotalDiscount})",
                        currentCarrierTariffsPivot.CarrierName,
                        carriersByCost.CarrierTLCost, 
                        ratedLoad.TotalLineHaul,
                        ratedLoad.TotalDiscount);

                }
                //TODO:  logic needs to be added to calcualte the Total Fuel, Stop and Pick charges using 
                //values other than the legacy charge calculator because new fees may be added that represent 
                //these values and they may be associated with the tblFormula based on formula type
                carriersByCost.FuelCost = ratedLoad.TotalFuelCost;
                carriersByCost.OtherFees = ratedLoad.TotalAccessorial - ratedLoad.TotalStopCharges - ratedLoad.TotalPickCharges - ratedLoad.TotalFuelCost;
                carriersByCost.PickCharges = ratedLoad.TotalPickCharges;
                carriersByCost.StopCharges = ratedLoad.TotalStopCharges;
                carriersByCost.TotalAccessorial = ratedLoad.TotalAccessorial;
                carriersByCost.TrackingState = currentCarrierTariffsPivot.TrackingState;
                carriersByCost.setRateType(currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl);
                carriersByCost.setClassType(currentCarrierTariffsPivot.CarrTarEquipMatClassTypeControl);
                //TODO: review code to get the correct bracket type like CWT based on the tariff settings
                //the enum may not be accurate for Distance (not sure)  It is not clear why we do not
                //always use the current tariff bracket type
                
                
                Logger.Information("{CarrierName} - Check if TariffMatrixRateType ({CarrTarEquipMatTarRateTypeControl}) is DistanceM {DistanceM} or DistanceK {DistanceK}",
                    currentCarrierTariffsPivot.CarrierName,
                    currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl,
                    (int)DAL.Utilities.TariffRateType.DistanceM,
                    (int)DAL.Utilities.TariffRateType.DistanceK);

                if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.DistanceM) ||
                    (currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.DistanceK))
                {
                    Logger.Information("{CarrierName} - Distance Bracket", currentCarrierTariffsPivot.CarrierName);
                    carriersByCost.setBracketType((int)DAL.Utilities.BracketType.Distance);
                }
                else if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.FlatRate))
                {
                    Logger.Information("{CarrierName} - Flat Rate Bracket", currentCarrierTariffsPivot.CarrierName);
                    carriersByCost.setBracketType(0);
                }
                else if ((currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl == (int)DAL.Utilities.TariffRateType.ClassRate))
                {
                    Logger.Information("{CarrierName} - Class Rate Bracket", currentCarrierTariffsPivot.CarrierName);
                    carriersByCost.setBracketType((int)DAL.Utilities.BracketType.Lbs);
                }
                else
                {
                    Logger.Information("{CarrierName} - Other Bracket based on CarrTarEquipMatTarRatetypeControl = {MatrixTarRateControl}",
                        currentCarrierTariffsPivot.CarrierName,
                        currentCarrierTariffsPivot.CarrTarEquipMatTarRateTypeControl);

                    carriersByCost.setBracketType(currentCarrierTariffsPivot.CarrTarEquipMatTarBracketTypeControl);
                }

                carriersByCost.setTempType(currentCarrierTariffsPivot.CarrTarTempType);
                //check if this carrier is qualified for selection
                List<string> sFaultInfo = new List<string>();
                DAL.LTS.vCarrierQual oCarQual = getCarrierQual(currentCarrierTariffsPivot.CarrierControl, ref sFaultInfo);

                Logger.Information("updateCarriersByCost.getCarrierQual: {@oCarQual} FaultInfo: {@sFaultInfo}", oCarQual, sFaultInfo);

                if (sFaultInfo != null && sFaultInfo.Count() > 0)
                {
                    //this is a fault exception we may need to return a message to the user.
                    //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                    //and a message about why the tariff cannot be selected.                
                    carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_SQLFaultCannotReadCarrierQual, sFaultInfo);
                }
                else
                {
                    if (oCarQual != null)
                    {
                        System.DateTime dtNow = System.DateTime.Now;
                        //if (oCarQual.CarrierQualQualified ?? true == false) { carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.W_CarrierNotQualified); }
                        if (!(oCarQual.CarrierQualQualified ?? true))
                        {
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.W_CarrierNotQualified);
                            Logger.Warning("TarBaseClass.updateCarriersByCost - Carrier Not Qualified: {CarrierName}", currentCarrierTariffsPivot.CarrierName);
                        }
                        if (oCarQual.CarrierQualInsuranceDate == null || oCarQual.CarrierQualInsuranceDate.Value < dtNow)
                        {
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.W_CarrierInsExpired);
                            Logger.Warning("TarBaseClass.updateCarriersByCost - Carrier Insurance Expired: {CarrierName}", currentCarrierTariffsPivot.CarrierName);
                        }
                        if ((oCarQual.CarrierQualContract ?? false) && (oCarQual.CarrierQualContractExpiresDate == null || oCarQual.CarrierQualContractExpiresDate.Value < dtNow))
                        {
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.W_CarrierContractExpired);
                            Logger.Warning("TarBaseClass.updateCarriersByCost - Carrier Contract Expired: {CarrierName}", currentCarrierTariffsPivot.CarrierName);
                        }
                    }
                }
                //New fields added by RHR 5/19/14 for transit time calculations
                carriersByCost.CarrTarWillDriveSaturday = currentCarrierTariffsPivot.CarrTarWillDriveSaturday;
                carriersByCost.CarrTarWillDriveSunday = currentCarrierTariffsPivot.CarrTarWillDriveSunday;
                carriersByCost.CarrTarOutbound = currentCarrierTariffsPivot.CarrTarOutbound;
                carriersByCost.CarrTarEquipMatMaxDays = currentCarrierTariffsPivot.CarrTarEquipMatMaxDays;
                carriersByCost.BookOutOfRouteMiles = ratedLoad.TotalOutOfRouteMiles;
                //Modified by RHR v-7.0.5.100 4/20/2016
                //Added InterlinePoint flag to results
                carriersByCost.InterlinePoint = ratedLoad.InterlinePoint;
                //Get the no drive days
                sFaultInfo = new List<string>();
                List<DTO.CarrTarNoDriveDays> lNoDriveDays = this.getCarrTarNoDriveDays(currentCarrierTariffsPivot.CarrTarControl, ref sFaultInfo);
                
                if (sFaultInfo != null && sFaultInfo.Count() > 0)
                {
                    //this is a fault exception we may need to return a message to the user.
                    //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                    //and a message about why the tariff cannot be selected.                
                    carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_SQLFaultCannotReadCarrierTariffNoDriveDays, sFaultInfo);
                }
                pivots = new DTO.CarriersByCost[1] { carriersByCost };

                try
                {
                    NGL.FM.CarTar.CarrTarEstimatedDates oEstDates =
                        new NGL.FM.CarTar.CarrTarEstimatedDates(this.Parameters,
                            ratedLoad,
                            pivots,
                            lNoDriveDays);
                    oEstDates.setupData();
                    Logger.Information("{CarrierName} - Calculating Estimated Dates CalcMustLeaveBy", currentCarrierTariffsPivot.CarrierName);
                    oEstDates.CalcMustLeaveBy();
                    Logger.Information("{CarrierName} - Calculating Estimated Dates CalcEstimatedDeliveryDate", currentCarrierTariffsPivot.CarrierName);
                    oEstDates.CalcEstimatedDeliveryDate();
                    Logger.Information("{CarrierName} - Calculating Estimated Dates GetCarriersByCostCalculated", currentCarrierTariffsPivot.CarrierName);
                    pivots = oEstDates.GetCarriersByCostCalculated();
                    foreach (DTO.BookRevenue b in ratedLoad.BookRevenues)
                    {
                        // Modified by RHr for v-8.5.3.006 on 12/07/2022  added BookCarrTransitTime                        
                        b.BookCarrTransitTime = pivots[0].BookCarrTransitTime;

                        //Modified by RHR v-7.0.5.102 9/30/20126  Hot fix for invalid values when using line haul or when transit days are zero
                        if (pivots[0].BookMustLeaveByDateTime != null && pivots[0].BookMustLeaveByDateTime.HasValue)
                        {
                            b.BookMustLeaveByDateTime = pivots[0].BookMustLeaveByDateTime;
                        }


                        if (pivots[0].BookExpDelDateTime != null && pivots[0].BookExpDelDateTime.HasValue)
                        {
                            b.BookExpDelDateTime = pivots[0].BookExpDelDateTime;
                        }
                        //Ari Requirement for TransitDay calculations. 8/19/2014.
                        //If either date is null set them to the BookDateLoad.
                        if (b.BookMustLeaveByDateTime == null || b.BookMustLeaveByDateTime.HasValue == false)
                        {
                            b.BookMustLeaveByDateTime = b.BookDateLoad;
                        }
                        else
                        {
                            //if we are not adjusting load date because of production lead time issues
                            //and the must leave by date is before the current ship date
                            //and BookMustLeaveByDateTime is greater than today
                            //and the lane is configured to update the ship date based on transit time
                            // We change the ship date to the Must Leave by Date and Time
                            // rules:
                            // 1. if BookProductionLeadTimeApplied is null update BookDateLoad
                            // 2. if BookProductionLeadTimeApplied is not null and is false update BookDateLoad
                            // 3. if BookProductionLeadTimeApplied is true do not update BookDateLoad
                            // result:  if nullable BookProductionLeadTimeApplied not true then  update BookDateLoad
                            Logger.Information("{CarrierName} - Calculating Estimated Dates check to see if BookProductionLeadTimeApplied is null or false", currentCarrierTariffsPivot.CarrierName);
                            if (!(b.BookProductionLeadTimeApplied ?? false))
                            {
                                Logger.Information("Checking if BookMustLeaveByDateTime ({BookMustLeaveByDateTime} is before BookDateLoad ({BookDateLoad}) and BookMustLeaveByDateTime ({BookMustLeaveByDateTime}) is greater than today", b.BookMustLeaveByDateTime, b.BookDateLoad, b.BookMustLeaveByDateTime);
                                if (b.BookMustLeaveByDateTime < b.BookDateLoad && b.BookMustLeaveByDateTime > DateTime.Now)
                                {
                                    Logger.Information("{CarrierName} - BookMustLeaveByDateTime is before BookDateLoad and BookMustLeaveByDateTime is greater than today", currentCarrierTariffsPivot.CarrierName);
                                    int iLaneControl = b.BookODControl;
                                    // next get the LaneTransLeadTimeCalcType  this is used to modify the ship date or required date based on transit hours/days
                                    Logger.Information("{CarrierName} - Calculating LaneTransLeadTimeCalcType for LaneControl: {iLaneControl}", currentCarrierTariffsPivot.CarrierName, iLaneControl);
                                    int iGetLaneTransLeadTimeCalcType = NGLLaneData.GetLaneTransLeadTimeCalcType(iLaneControl);
                                    if (iGetLaneTransLeadTimeCalcType == 1)
                                    {
                                        Logger.Information("{CarrierName} LaneTransLeadTimeCalcType is 1 so we will update the BookDateLoad to BookMustLeaveByDateTime", currentCarrierTariffsPivot.CarrierName);
                                        b.BookDateLoad = b.BookMustLeaveByDateTime;
                                    }
                                }
                            }
                        }
                        Logger.Information("{CarrierName} - Calculating Estimated Dates check to see if BookExpDelDateTime is null or false", currentCarrierTariffsPivot.CarrierName);
                        if (b.BookExpDelDateTime == null || b.BookExpDelDateTime.HasValue == false)
                        {
                            //Modified by RHR v-7.0.5.102  9/30/2016  
                            //  the default Expected Delivery Date should be the 
                            //  required date not the load date
                            Logger.Information("BookExpDelDateTime is null or false so we will update the BookExpDelDateTime to BookDateRequired ({BookDateRequired})", b.BookDateRequired);
                            b.BookExpDelDateTime = b.BookDateRequired;
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    string sMsg = ex.Message;
                    if (sMsg == "E_InvalidEstDateSetup" || sMsg == "E_InvalidEstDateReqDate" || sMsg == "E_InvalidEstDateLoadDate")
                    {
                        Logger.Error(ex, "Error calculating Estimated Dates");
                        carriersByCost.AddMessage(ex.Message);
                    }
                    else
                    {
                        Logger.Error(ex, "Error calculating Estimated Dates");
                        carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.E_CannotCalcEstDates, new List<string> { ex.Message });
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error calculating Estimated Dates");
                    carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.E_CannotCalcEstDates, new List<string> { ex.Message });
                }
                Logger.Information("{CarrierName} End carriersByCost: {@carriersByCost} pivots[0]: {@pivots}", currentCarrierTariffsPivot.CarrierName, carriersByCost, pivots[0]);
            }
            return pivots[0]; //there should be only one
        }

        /// <summary>
        /// Overload reads tariff data using ratedload CarrTarControl used when a pivot record is not avaliable like when recalculating costs and fees.
        /// </summary>
        /// <param name="carriersByCost"></param>
        /// <param name="currentCarrierTariffsPivot"></param>
        /// <param name="ratedLoad"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR v-7.0.5.100 4/20/2016
        /// Added InterlinePoint flag to results
        /// Modified by RHR v-7.0.5.102 9/30/20126  
        ///   Hot fix for invalid values when using line haul or when transit days are zero
        ///   the default Expected Delivery Date should be the
        ///   required date not the load date
        /// </remarks>
        protected DTO.CarriersByCost updateCarriersByCost(ref DTO.CarriersByCost carriersByCost, Load ratedLoad)
        {
            DTO.CarriersByCost[] pivots = null;
            using (var operation = Logger.StartActivity("Start carriersByCost: {@carriersByCost} Load: {@ratedLoad}", carriersByCost, ratedLoad))
            {
                List<string> sFaultInfo = new List<string>();
                List<DTO.CarrTarNoDriveDays> lNoDriveDays = new List<DTO.CarrTarNoDriveDays>();
                bool blnUseCarrTarContract = false;
                DTO.BookRevenue ratedStop = ratedLoad.RatedBookRevenue;
                Logger.Information("Check to see if BookCarTarControl isn't 0");
                if (ratedStop.BookCarrTarControl != 0)
                {

                    Logger.Information("Get CarrTarContract");
                    DTO.CarrTarContract CarrTarContract = getCarrTarContract(ratedStop.BookCarrTarControl, ref sFaultInfo);
                    if (sFaultInfo != null && sFaultInfo.Count() > 0)
                    {
                        //this is a fault exception we may need to return a message to the user.
                        //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                        //and a message about why the tariff cannot be selected.                
                        Logger.Error("Error getting CarrTarContract", sFaultInfo);
                        carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_SQLFaultCannotReadCarrTarContract, sFaultInfo);
                    }
                    if (CarrTarContract != null && CarrTarContract.CarrTarControl != 0)
                    {

                        Logger.Information("Set values from CarrTarContract", CarrTarContract);

                        blnUseCarrTarContract = true;
                        carriersByCost.BookCarrTarName = CarrTarContract.CarrTarName;
                        carriersByCost.BookCarrTarRevisionNumber = CarrTarContract.CarrTarRevisionNumber;
                        carriersByCost.CarrierRateExpires = CarrTarContract.CarrTarEffDateTo;
                        carriersByCost.CarrTarWillDriveSaturday = CarrTarContract.CarrTarWillDriveSaturday;
                        carriersByCost.CarrTarWillDriveSunday = CarrTarContract.CarrTarWillDriveSunday;
                        carriersByCost.CarrTarOutbound = CarrTarContract.CarrTarOutbound;
                        carriersByCost.setTempType(CarrTarContract.CarrTarTempType);
                        carriersByCost.setModeType(CarrTarContract.CarrTarTariffModeTypeControl);
                        //Get the no drive days

                        Logger.Information("Get CarrTarNoDriveDays");

                        sFaultInfo = new List<string>();

                        lNoDriveDays = this.getCarrTarNoDriveDays(CarrTarContract.CarrTarControl, ref sFaultInfo);
                        if (sFaultInfo != null && sFaultInfo.Count() > 0)
                        {
                            //this is a fault exception we may need to return a message to the user.
                            //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                            //and a message about why the tariff cannot be selected.                
                            Logger.Error("Error getting CarrTarNoDriveDays", sFaultInfo);
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_SQLFaultCannotReadCarrierTariffNoDriveDays, sFaultInfo);
                        }

                    }
                }
                Logger.Information("{CarrierName} - check for if !blnUseCarrTarContract ({blnUseCarrTarContract})", carriersByCost.CarrierName, !blnUseCarrTarContract);
                if (!blnUseCarrTarContract)
                {
                    Logger.Information("!blnUseCarrTarContract is true.. so settign carrier to rated stop carrier tariff ({TariffName}:{TariffRevision})", ratedStop.BookCarrTarName, ratedStop.BookCarrTarRevisionNumber);
                    carriersByCost.BookCarrTarName = ratedStop.BookCarrTarName;
                    carriersByCost.BookCarrTarRevisionNumber = ratedStop.BookCarrTarRevisionNumber;
                    carriersByCost.CarrierRateExpires = DateTime.Now.AddDays(1);
                    carriersByCost.CarrTarWillDriveSaturday = true;
                    carriersByCost.CarrTarWillDriveSunday = true;
                    carriersByCost.CarrTarOutbound = true;
                    carriersByCost.setTempType(0);
                    carriersByCost.setModeType(3);

                }
                carriersByCost.BookAllowInterlinePoints = ratedStop.BookAllowInterlinePoints;
                carriersByCost.BookCarrTarControl = ratedStop.BookCarrTarControl;
                carriersByCost.BookCarrTarEquipControl = ratedStop.BookCarrTarEquipControl;
                //For LTL (Class Type Rates) we could have multiple Matrix Rates for each order because each item 
                //May have a differrent rate.  so the values of 
                //  CarrTarEquipMatControl, 
                //  BookCarrTarEquipMatDetControl, 
                //  and BookCarrTarEquipMatDetID 
                //may not be 100% accurate
                carriersByCost.BookCarrTarEquipMatControl = ratedStop.BookCarrTarEquipMatControl;
                carriersByCost.BookCarrTarEquipMatDetControl = ratedStop.BookCarrTarEquipMatDetControl;
                carriersByCost.BookCarrTarEquipMatDetID = 0;  //reserved for future use
                carriersByCost.BookCarrTarEquipMatDetValue = 0; //reserved for future use                
                carriersByCost.BookCarrTarEquipMatName = ratedStop.BookCarrTarEquipMatName;
                carriersByCost.BookCarrTarEquipName = ratedStop.BookCarrTarEquipName;
                carriersByCost.BookCarrTruckControl = ratedStop.BookCarrTruckControl;
                carriersByCost.BookRevDiscount = ratedLoad.TotalDiscount;
                carriersByCost.BookRevLineHaul = ratedLoad.TotalLineHaul;
                carriersByCost.BookRevLoadMiles = ratedLoad.TotalMiles;
                carriersByCost.BookTransType = ratedStop.BookTransType;
                carriersByCost.CarrierMileRate = 0;
                carriersByCost.CarrierLbsRate = 0;
                carriersByCost.CarrierCubeRate = 0;
                carriersByCost.CarrierPltRate = 0;
                carriersByCost.CarrierCaseRate = 0;
                carriersByCost.CarrierControl = ratedStop.BookCarrierControl;
                carriersByCost.CarrierCost = ratedLoad.SumOfEstTotalCost;
                //Removed by RHR for v-8.5.4.001 on 07/06/2023 we now use this value differetly as part of the Bid data
                //carriersByCost.setUpcharge(getCompanyLevelParameterValue(ratedStop.BookCustCompControl, "CarrierCostUpcharge"));
                carriersByCost.CarrierEquipment = "";
                carriersByCost.CarrierMinCost = 0;
                carriersByCost.CarrierName = "";
                carriersByCost.CarrierNumber = 0;
                carriersByCost.CarrierTLCost = 0;
                //TODO:  logic needs to be added to calcualte the Total Fuel, Stop and Pick charges using 
                //values other than the legacy charge calculator because new fees may be added that represent 
                //these values and they may be associated with the tblFormula based on formula type
                carriersByCost.FuelCost = ratedLoad.TotalFuelCost;
                carriersByCost.OtherFees = ratedLoad.TotalAccessorial - ratedLoad.TotalStopCharges - ratedLoad.TotalPickCharges - ratedLoad.TotalFuelCost;
                carriersByCost.PickCharges = ratedLoad.TotalPickCharges;
                carriersByCost.StopCharges = ratedLoad.TotalStopCharges;
                carriersByCost.TotalAccessorial = ratedLoad.TotalAccessorial;

                Logger.Information("Setting ratetype ({OriginalRateType}) to 0, class type ({OriginalClassType}) to 0, and bracket type ({OriginalBracketType}) to 0",
                    carriersByCost.RateTypeName,
                    carriersByCost.ClassTypeName,
                    carriersByCost.BracketTypeName);

                carriersByCost.setRateType(0);
                carriersByCost.setClassType(0);
                carriersByCost.setBracketType(0);
                //TODO: when recalculating we need to determin the level of qualification validation (check old code . 6.0.x)
                //check if this carrier is qualified for selection
                sFaultInfo = new List<string>();
                DAL.LTS.vCarrierQual oCarQual = getCarrierQual(ratedStop.BookCarrierControl, ref sFaultInfo);
                if (sFaultInfo != null && sFaultInfo.Count() > 0)
                {
                    //this is a fault exception we may need to return a message to the user.
                    //the createInvalidCarrier is used to show the carrier tariff info on the screen with zero cost
                    //and a message about why the tariff cannot be selected.                
                    Logger.Error("Error getting CarrierQual", sFaultInfo);
                    carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.M_SQLFaultCannotReadCarrierQual, sFaultInfo);
                }
                else
                {
                    if (oCarQual != null)
                    {

                        Logger.Information("Check CarrierQual for {SCAC}, {@oCarQual}", oCarQual.CarrierQualAuthority, oCarQual);
                        System.DateTime dtNow = System.DateTime.Now;
                        if (oCarQual.CarrierQualQualified ?? true == false) { 
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.W_CarrierNotQualified);
                            Logger.Warning("{CarrierName} - Carrier Not Qualified: ", carriersByCost.CarrierName);
                        }
                        if (oCarQual.CarrierQualInsuranceDate == null || oCarQual.CarrierQualInsuranceDate.Value < dtNow) { 
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.W_CarrierInsExpired);
                            Logger.Warning("{CarrierName} - Carrier Insurance Expired: ", carriersByCost.CarrierName);
                        }
                        if ((oCarQual.CarrierQualContract ?? false) && (oCarQual.CarrierQualContractExpiresDate == null || oCarQual.CarrierQualContractExpiresDate.Value < dtNow)) { 
                            Logger.Warning("{CarrierName} - Carrier Contract Expired: ", carriersByCost.CarrierName);
                            carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.W_CarrierContractExpired);
                        }
                    }
                }
                carriersByCost.CarrTarEquipMatMaxDays = 0;
                carriersByCost.BookOutOfRouteMiles = ratedLoad.TotalOutOfRouteMiles;
                carriersByCost.InterlinePoint = ratedLoad.InterlinePoint;

                pivots = new DTO.CarriersByCost[1] { carriersByCost };
                try
                {
                    Logger.Information("Create CarrTarEstimatedDates - RatedStop: {@ratedStop} Carriers: {@CarriersByCost}", ratedStop, carriersByCost);
                    NGL.FM.CarTar.CarrTarEstimatedDates oEstDates =
                        new NGL.FM.CarTar.CarrTarEstimatedDates(this.Parameters,
                            ratedLoad,
                            pivots,
                            lNoDriveDays);
                    Logger.Information("Setup Data - oEstDates: {@oEstDates}", oEstDates);
                    oEstDates.setupData();
                    Logger.Information("Calc Must Leave By {@oEstDates}", oEstDates);
                    oEstDates.CalcMustLeaveBy();
                    Logger.Information("Calc Estimated Delivery Date {@oEstDates}", oEstDates);
                    oEstDates.CalcEstimatedDeliveryDate();
                    Logger.Information("Get Carriers By Cost Calculated {@oEstDates}", oEstDates);
                    pivots = oEstDates.GetCarriersByCostCalculated();
                    foreach (DTO.BookRevenue b in ratedLoad.BookRevenues)
                    {
                        Logger.Information("Update Book Revenue with Pivot Data {Book}, {Pivot}", b, pivots[0]);
                        // Modified by RHr for v-8.5.3.006 on 12/07/2022  added BookCarrTransitTime                        
                        b.BookCarrTransitTime = pivots[0].BookCarrTransitTime;

                        //Modified by RHR v-7.0.5.102 9/30/20126  Hot fix for invalid values when using line haul or when transit days are zero

                        if (pivots[0].BookMustLeaveByDateTime != null && pivots[0].BookMustLeaveByDateTime.HasValue)
                        {
                            b.BookMustLeaveByDateTime = pivots[0].BookMustLeaveByDateTime;
                        }
                        Logger.Information("Check BookExpDelDateTime {BookExpDelDateTime}", pivots[0]);
                        if (pivots[0].BookExpDelDateTime != null && pivots[0].BookExpDelDateTime.HasValue)
                        {
                            b.BookExpDelDateTime = pivots[0].BookExpDelDateTime;
                        }
                        //Ari Requirement for TransitDay calculations. 8/19/2014.
                        //If either date is null set them to the BookDateLoad.
                        Logger.Information("Check BookMustLeaveByDateTime and BookExpDelDateTime", pivots[0]);
                        if (b.BookMustLeaveByDateTime == null || b.BookMustLeaveByDateTime.HasValue == false)
                        {
                            b.BookMustLeaveByDateTime = b.BookDateLoad;
                        }
                        else
                        {
                            //if we are not adjusting load date because of production lead time issues
                            //and the must leave by date is before the current ship date
                            //and BookMustLeaveByDateTime is greater than today
                            //and the lane is configured to update the ship date based on transit time
                            // We change the ship date to the Must Leave by Date and Time
                            // rules:
                            // 1. if BookProductionLeadTimeApplied is null update BookDateLoad
                            // 2. if BookProductionLeadTimeApplied is not null and is false update BookDateLoad
                            // 3. if BookProductionLeadTimeApplied is true do not update BookDateLoad
                            // result:  if nullable BookProductionLeadTimeApplied not true then  update BookDateLoad
                            Logger.Information("Check BookProductionLeadTimeApplied ({0})", b.BookProductionLeadTimeApplied);
                            if (!(b.BookProductionLeadTimeApplied ?? false))
                            {
                                Logger.Information("Checking if BookMustLeaveByDateTime {BookMustLeaveByDateTime} is before BookDateLoad and {BookDateLoad} BookMustLeaveByDateTime {BookMustLeaveByDateTime} is greater than Now {Now}", b.BookMustLeaveByDateTime, b.BookDateLoad, b.BookMustLeaveByDateTime, DateTime.Now);
                                if (b.BookMustLeaveByDateTime < b.BookDateLoad && b.BookMustLeaveByDateTime > DateTime.Now)
                                {
                                    Logger.Information("BookMustLeaveByDateTime is before BookDateLoad and BookMustLeaveByDateTime is greater than today");
                                    int iLaneControl = b.BookODControl;
                                    // next get the LaneTransLeadTimeCalcType  this is used to modify the ship date or required date based on transit hours/days
                                    Logger.Information("Calculating LaneTransLeadTimeCalcType for LaneControl: {iLaneControl}", iLaneControl);
                                    int iGetLaneTransLeadTimeCalcType = NGLLaneData.GetLaneTransLeadTimeCalcType(iLaneControl);
                                    if (iGetLaneTransLeadTimeCalcType == 1)
                                    {
                                        Logger.Information("LaneTransLeadTimeCalcType is 1 so we will update the BookDateLoad to BookMustLeaveByDateTime");
                                        b.BookDateLoad = b.BookMustLeaveByDateTime;
                                    }
                                }
                            }

                        }
                        Logger.Information("Check BookExpDelDateTime");
                        if (b.BookExpDelDateTime == null || b.BookExpDelDateTime.HasValue == false)
                        {
                            //Modified by RHR v-7.0.5.102  9/30/2016  
                            //  the default Expected Delivery Date should be the 
                            //  required date not the load date
                            Logger.Information("BookExpDelDateTime is null or false so we will update the BookExpDelDateTime to BookDateRequired");
                            b.BookExpDelDateTime = b.BookDateRequired;
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Logger.Error(ex, "Error calculating Estimated Dates");
                    string sMsg = ex.Message;
                    if (sMsg == "E_InvalidEstDateSetup" || sMsg == "E_InvalidEstDateReqDate" || sMsg == "E_InvalidEstDateLoadDate")
                    {
                        carriersByCost.AddMessage(ex.Message);
                    }
                    else
                    {
                        carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.E_CannotCalcEstDates, new List<string> { ex.Message });
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error calculating Estimated Dates");
                    carriersByCost.AddMessage(DTO.CarriersByCost.MessageEnum.E_CannotCalcEstDates, new List<string> { ex.Message });
                }

                Logger.Information("End carriersByCost: {@carriersByCost} and Pivots[0] {@Pivot}", carriersByCost, pivots[0]);
            }
            return pivots[0];  //there should be only one
        }

        /// <summary>
        /// Checks a static dictionary of Carrier Qual Data for CarrierControl and returns the data object; if the dictionary does not contain CarrierControl the data is retrieved from the database.
        /// </summary>
        /// <param name="CarrierControl"></param>
        /// <param name="sFaultInfo"></param>
        /// <returns></returns>
        protected DAL.LTS.vCarrierQual getCarrierQual(int CarrierControl, ref List<string> sFaultInfo)
        {
            DAL.LTS.vCarrierQual oData = null;      // nothing found.
                                                    //check for a stored reference for this carrTarControl
            using (var operation = Logger.StartActivity("getCarrierQual(CarrierControl: {CarrierControl}, sFaultInfo)", CarrierControl))
            {
                if (DictCarrierQuals.ContainsKey(CarrierControl)) { return DictCarrierQuals[CarrierControl]; }
                //read the data from the database
                try
                {
                    oData = NGLCarrierData.GetCarrierQual(CarrierControl);
                    DictCarrierQuals.Add(CarrierControl, oData);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                    {
                        Logger.Error(sqlEx, "TarBaseClass.getCarrierQual - Error getting CarrierQual");
                        sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                        // "E_SQLExceptionMSG", E_UnExpectedMSG
                        
                    }
                    else
                    {
                        Logger.Error(sqlEx, "TarBaseClass.getCarrierQual - Error getting CarrierQual, but I'm adding it to the CarrierQual???");
                        DictCarrierQuals.Add(CarrierControl, null);
                    }
                }
                catch (Exception ex)
                {
                    operation.Complete(Serilog.Events.LogEventLevel.Error, ex);
                    Logger.Error(ex, "TarBaseClass.getCarrierQual - Error getting CarrierQual");
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    //throw;
                }
            }
            return oData;

        }

        /// <summary>
        /// Checks a static dictionary of Carrier Tariff Contract information for CarrTarControl and returns the data; 
        /// if the dictionary does not contain CarrTarControl the data is retrieved from the database.
        /// </summary>
        /// <param name="CarrTarControl"></param>
        /// <param name="sFaultInfo"></param>
        /// <returns></returns>
        protected DTO.CarrTarContract getCarrTarContract(int CarrTarControl, ref List<string> sFaultInfo)
        {
            DTO.CarrTarContract oData = null;      // nothing found.
                                                   //check for a stored reference for this carrTarControl
            using (var operation = Logger.StartActivity("getCarrTarContract(CarrTarControl: {CarrTarControl})", CarrTarControl))
            {
                if (DictCarrTarContracts.ContainsKey(CarrTarControl))
                {
                    Logger.Information("CarrTarConrol ({CarrTarControl}) already found in Dictionary", CarrTarControl);
                    return DictCarrTarContracts[CarrTarControl];
                }
                //read the data from the database
                try
                {
                    oData = this.NGLCarrTarContractData.GetCarrTarContractFiltered(CarrTarControl);
                    DictCarrTarContracts.Add(CarrTarControl, oData);
                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    Logger.Error(sqlEx, "TarBaseClass.getCarrTarContract - Error getting CarrTarContract");
                    if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                    {
                        sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                        // "E_SQLExceptionMSG", E_UnExpectedMSG
                        string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                            sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                        
                    }
                    else
                    {
                        DictCarrTarContracts.Add(CarrTarControl, null);
                    }
                }
                catch (Exception ex)
                {
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    throw;
                }
            }
            return oData;

        }

        protected DTO.CarrierCont getCarrierCont(int CarrierControl, ref List<string> sFaultInfo)
        {
            DTO.CarrierCont oData = null;      // nothing found.
                                               //check for a stored reference for this carrTarControl
            if (DictCarrierConts.ContainsKey(CarrierControl)) { return DictCarrierConts[CarrierControl]; }
            //read the data from the database
            try
            {
                oData = this.NGLCarrierContData.GetFirstCarrierContForCarrier(CarrierControl);
                DictCarrierConts.Add(CarrierControl, oData);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                Logger.Error(sqlEx, "TarBaseClass.getCarrierCont - Error getting CarrierCont");
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCarrierCont"), CarrierControl.ToString());
                }
                else
                {
                    DictCarrierConts.Add(CarrierControl, null);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "TarBaseClass.getCarrierCont - Error getting CarrierCont");
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
            }

            return oData;

        }

        protected void setCarrierCont(DTO.CarrierCont CarrContact)
        {
            if (CarrContact == null || CarrContact.CarrierContControl == 0) { return; }
            int CarrierControl = CarrContact.CarrierContCarrierControl;
            //check for a stored reference for this carrTarControl
            if (DictCarrierConts.ContainsKey(CarrierControl))
            {
                //update the data with the current contact object.
                DictCarrierConts[CarrierControl] = CarrContact;
            }
            else
            {
                DictCarrierConts.Add(CarrierControl, CarrContact);
            }
        }

        /// <summary>
        /// Checks a static dictionary of Carrier Tariff Contract information for CarrTarControl and returns the data; 
        /// if the dictionary does not contain CarrTarControl the data is retrieved from the database.
        /// </summary>
        /// <param name="CarrTarControl"></param>
        /// <param name="sFaultInfo"></param>
        /// <returns></returns>
        protected List<DTO.CarrTarNoDriveDays> getCarrTarNoDriveDays(int CarrTarControl, ref List<string> sFaultInfo)
        {
            List<DTO.CarrTarNoDriveDays> noDriveDays = new List<DTO.CarrTarNoDriveDays>();      // nothing found.
            using (var operation = Logger.StartActivity("getCarrTarNoDriveDays - Start CarrTarControl: {CarrTarControl}", CarrTarControl))
            {
                if (DictCarrTarNoDriveDays.ContainsKey(CarrTarControl))
                {
                    Logger.Information("CarrTarConrol ({CarrTarControl}) already found in Dictionary", CarrTarControl);
                    operation.Complete();

                    return DictCarrTarNoDriveDays[CarrTarControl];
                }

                try
                {
                    DTO.CarrTarNoDriveDays[] oData = this.NGLCarrierTariffNoDriveDays.GetCarrTarNDDsFiltered(CarrTarControl);
                    if (oData != null)
                    {

                        noDriveDays = oData.ToList();
                        string ndds = String.Join(",", noDriveDays.Select(nd => nd.CarrTarNDDNoDrivingDate));
                        DictCarrTarNoDriveDays.Add(CarrTarControl, noDriveDays);
                    }


                }
                catch (FaultException<DAL.SqlFaultInfo> sqlEx)
                {
                    Logger.Error(sqlEx, "TarBaseClass.getCarrTarNoDriveDays - Error getting CarrTarNoDriveDays");
                    if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                    {
                        sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                        // "E_SQLExceptionMSG", E_UnExpectedMSG
                        string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                            sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                        SaveSysError(errMsg, sourcePath("getCarrTarNoDriveDays"), CarrTarControl.ToString());
                    }
                    else
                    {
                        string ndds = String.Join(",", noDriveDays.Select(nd => nd.CarrTarNDDNoDrivingDate));
                        Logger.Information("Adding No Driving Days {NoDriveDays} to {CarrTarControl}", CarrTarControl, ndds);
                        DictCarrTarNoDriveDays.Add(CarrTarControl, noDriveDays);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "getCarrTarNoDriveDays - Error getting CarrTarNoDriveDays");
                    // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                    //throw;
                }
            }
            return noDriveDays;

        }

        /// <summary>
        /// Checks a static dictionary of Default Class Codes for CompControl and returns the list; if the dictionary does not contain CompControl the data is retrieved from the database.
        /// </summary>
        /// <param name="CompControl"></param>
        /// <param name="sFaultInfo">List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };</param>
        /// <returns></returns>
        protected List<DAL.LTS.udfGetDefaultClassCodesResult> getDefaultClassCodes(int CompControl, ref List<string> sFaultInfo)
        {
            List<DAL.LTS.udfGetDefaultClassCodesResult> oData = new List<DAL.LTS.udfGetDefaultClassCodesResult>();      // nothing found.
                                                                                                                        //check for a stored reference for this carrTarControl
            if (DictDefaultClassCodes.ContainsKey(CompControl)) { return DictDefaultClassCodes[CompControl]; }
            //read the data from the database
            try
            {
                oData = NGLCarrTarContractData.GetDefaultClassCodes(CompControl);
                DictDefaultClassCodes.Add(CompControl, oData);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {
                    sFaultInfo = new List<string> { sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details };
                    // "E_SQLExceptionMSG", E_UnExpectedMSG
                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getDefaultClassCodes"), CompControl.ToString());
                }
                else
                {
                    DictDefaultClassCodes.Add(CompControl, oData);
                }
            }
            catch (Exception ex)
            {
                // This should never happen, the Exception and InvalidOperationExceptions should have been rethrown as FaultExceptions.
                throw;
            }

            return oData;

        }

        protected double getCompanyLevelParameterValue(int CompControl, string parKey)
        {
            double dblRet = 0;
            Tuple<int, string> dictKey = Tuple.Create(CompControl, parKey);
            if (DictCompanyLevelParameterValue.ContainsKey(dictKey)) { return DictCompanyLevelParameterValue[dictKey]; }
            //read the data from the database
            try
            {
                dblRet = this.NGLCompParameterData.GetParValue(parKey, CompControl);
                DictCompanyLevelParameterValue.Add(dictKey, dblRet);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                if (sqlEx.Message != "E_NoData")    // don't throw the exception if no data was found.
                {

                    string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",
                        sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                    SaveSysError(errMsg, sourcePath("getCompanyLevelParameterValue"), CompControl.ToString());
                }
                else
                {
                    DictCompanyLevelParameterValue.Add(dictKey, 0);
                }
            }
            return dblRet;

        }


        /// <summary>
        /// Checks a static dictionary of Default Class Codes for CompControl and returns the list; 
        /// if the dictionary does not contain CompControl the data is retrieved from the database 
        /// then it populates the default value strings with data.
        /// </summary>
        /// <param name="CompControl"></param>
        /// <param name="sFaultInfo"></param>
        /// <param name="s49CFRDefault"></param>
        /// <param name="sIATADefault"></param>
        /// <param name="sDOTDefault"></param>
        /// <param name="sMarineDefault"></param>
        /// <param name="sNMFCDefault"></param>
        /// <param name="sFAKDefault"></param>
        protected void populateDefaultClassCodes(int CompControl,
            ref List<string> sFaultInfo,
            ref string s49CFRDefault,
            ref string sIATADefault,
            ref string sDOTDefault,
            ref string sMarineDefault,
            ref string sNMFCDefault,
            ref string sFAKDefault)
        {

            List<DAL.LTS.udfGetDefaultClassCodesResult> oDefClassCodes = getDefaultClassCodes(CompControl, ref sFaultInfo);
            if ((sFaultInfo == null || sFaultInfo.Count() < 1) && (oDefClassCodes != null && oDefClassCodes.Count() > 0))
            {

                foreach (DAL.LTS.udfGetDefaultClassCodesResult c in oDefClassCodes)
                {
                    switch (c.ParKey)
                    {
                        case "Use49CFRDefault":
                            if (c.ParValue != 0)
                            {
                                s49CFRDefault = c.ParText;
                            }
                            break;
                        case "UseIATADefault":
                            if (c.ParValue != 0)
                            {
                                sIATADefault = c.ParText;
                            }
                            break;
                        case "UseDOTDefault":
                            if (c.ParValue != 0)
                            {
                                sDOTDefault = c.ParText;
                            }
                            break;
                        case "UseMarineDefault":
                            if (c.ParValue != 0)
                            {
                                sMarineDefault = c.ParText;
                            }
                            break;
                        case "UseNMFCDefault":
                            if (c.ParValue != 0)
                            {
                                sNMFCDefault = c.ParText;
                            }
                            break;
                        case "UseFAKDefault":
                            if (c.ParValue != 0)
                            {
                                sFAKDefault = c.ParText;
                            }
                            break;
                    }
                }
            }

        }
        #endregion

    }
}
