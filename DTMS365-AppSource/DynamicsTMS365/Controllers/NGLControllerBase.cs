using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;

using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticaionService;



using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DynamicsTMS365.Models;
using Ngl.FreightMaster.Data.Models;
using System.Drawing.Drawing2D;
using AuthenticaionService;

namespace DynamicsTMS365.Controllers
{
    /// <summary>
    /// Sample Default REST Service Controller Methods
    /// These methods are required when using the new NGLEditWindCtrl Widget
    /// The API listed will call 
    ///     Get{id} to read the record
    ///     POST{data} to insert or update a record
    ///     DELETE{id} to delete the record
    ///     PUT not currently used for NGLEditWindCtrl Widget
    /// 
    /// GET 	/API/objectcontroller : Get all objects
    /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
    /// 
    /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
    /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
    /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"
    /// </summary>
    public abstract class NGLControllerBase : ApiController
    {
        
        #region " Constructors "
        public NGLControllerBase()
                : base()
	        {
	        }

        /// <summary>
        /// Configures the Page property 
        /// </summary>
        /// <param name="page"></param>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018 
        /// </remarks>
        public NGLControllerBase(Utilities.PageEnum page)
                : base()
        {
            this.Page = page;
        }

        #endregion

        #region " Properties"

        public abstract string SourceClass { get; set; }

        public int UserControl { get; set; }

        private DAL.WCFParameters _Parameters;
        public  DAL.WCFParameters Parameters
        {
            get
            {
                if (_Parameters == null)
                {
                    //Utilities.DALWCFParameters.CloneParameters(ref _Parameters);
                    _Parameters = Utilities.DALWCFParameters.CloneParameters();
                    //Can call method either way
                    //Note: executeFunc(Utilities.DALWCFParameters.CloneParameters()) -- this function requires a copy of the parameters as arg but we don't have to create an instance to execute the function
                    //Read user info from dictionary and populate other fields
                    if (Utilities.GlobalSSOResultsByUser.ContainsKey(UserControl))
                    {
                        //Modified By LVV on 4/9/20 - bug fix null reference exception
                        DAL.Models.SSOResults ssoa = Utilities.GlobalSSOResultsByUser[UserControl];
                        if (ssoa != null)
                        {
                            _Parameters.UserControl = UserControl;
                            _Parameters.UserName = ssoa.UserName;
                            _Parameters.IsUserCarrier = ssoa.IsUserCarrier;
                            _Parameters.UserCarrierControl = ssoa.UserCarrierControl;
                            _Parameters.UserCarrierContControl = ssoa.UserCarrierContControl;
                            _Parameters.UserLEControl = ssoa.UserLEControl;
                            _Parameters.UserEmail = ssoa.SSOAUserEmail;
                            _Parameters.CatControl = ssoa.CatControl;
                        }
                    }
                }
                return _Parameters;
            }
            set { _Parameters = value; }
        }

        private string _ConnectionString = "";
        public string ConnectionString
        {
            get
            {
                if (_ConnectionString.Trim().Length < 5)
                {
                    if ((Parameters != null) && Parameters.ConnectionString.Trim().Length > 5) { _ConnectionString = _Parameters.ConnectionString; } else { _ConnectionString = string.Format("Server={0}; Database={1}; Integrated Security=SSPI", Parameters.DBServer.Trim(), Parameters.Database.Trim()); }
                }
                return _ConnectionString;
            }
            set { _ConnectionString = value; }
        }

        /// <summary>
        /// Identifies the page used by this controller
        /// to store configuration settings.  Controllers are not always dedicated to show data
        /// on a singl page but settings like filters need to be saved with a page control number
        /// This enum identifies that value.  Use the new constructor to populate the value
        /// </summary>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018
        /// </remarks>
        public Utilities.PageEnum Page { get; set; }

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
        public DAL.NGLBookData NGLBookData
        {
            get{if (_NGLBookData == null) { _NGLBookData = (DAL.NGLBookData)NDPBaseClassFactory("NGLBookData");}return _NGLBookData;}
            set { _NGLBookData = value; }
        }

        private DAL.NGLBookItemData _NGLBookItemData;
        public DAL.NGLBookItemData NGLBookItemData
        {
            get{ if (_NGLBookItemData == null) { _NGLBookItemData = (DAL.NGLBookItemData)NDPBaseClassFactory("NGLBookItemData"); } return _NGLBookItemData;}
            set{ _NGLBookItemData = value;}
        }

        private DAL.NGLBookTrackData _NGLBookTrackData;
        public DAL.NGLBookTrackData NGLBookTrackData
        {
            get{if (_NGLBookTrackData == null) { _NGLBookTrackData = (DAL.NGLBookTrackData)NDPBaseClassFactory("NGLBookTrackData"); } return _NGLBookTrackData;}
            set{_NGLBookTrackData = value;}
        }

        private DAL.NGLBookNoteData _NGLBookNoteData;
        public DAL.NGLBookNoteData NGLBookNoteData
        {
            get { if (_NGLBookNoteData == null) { _NGLBookNoteData = (DAL.NGLBookNoteData)NDPBaseClassFactory("NGLBookNoteData"); } return _NGLBookNoteData; }
            set { _NGLBookNoteData = value; }
        }

        private DAL.NGLBookLoadData _NGLBookLoadData;
        public DAL.NGLBookLoadData NGLBookLoadData
        {
            get { if (_NGLBookLoadData == null) { _NGLBookLoadData = (DAL.NGLBookLoadData)NDPBaseClassFactory("NGLBookLoadData"); } return _NGLBookLoadData; }
            set { _NGLBookLoadData = value; }
        }

        private DAL.NGLBookRevenueData _NGLBookRevenueData;
        public DAL.NGLBookRevenueData NGLBookRevenueData
        {
            get { if (_NGLBookRevenueData == null) { _NGLBookRevenueData = (DAL.NGLBookRevenueData)NDPBaseClassFactory("NGLBookRevenueData"); } return _NGLBookRevenueData; }
            set { _NGLBookRevenueData = value; }
        }
         
        private DAL.NGLCarrTarEquipMatData _NGLCarrTarEquipMatData;
        public DAL.NGLCarrTarEquipMatData NGLCarrTarEquipMatData
        {
            get { if (_NGLCarrTarEquipMatData == null) { _NGLCarrTarEquipMatData = (DAL.NGLCarrTarEquipMatData)NDPBaseClassFactory("NGLCarrTarEquipMatData"); } return _NGLCarrTarEquipMatData; }
            set { _NGLCarrTarEquipMatData = value; }
        }
        private DAL.NGLCarrTarMatBPData _NGLCarrTarMatBPData;
        public DAL.NGLCarrTarMatBPData NGLCarrTarMatBPData
        {
            get { if (_NGLCarrTarMatBPData == null) { _NGLCarrTarMatBPData = (DAL.NGLCarrTarMatBPData)NDPBaseClassFactory("NGLCarrTarMatBPData"); } return _NGLCarrTarMatBPData; }
            set { _NGLCarrTarMatBPData = value; }
        }

        private DAL.NGLBookCarrierData _NGLBookCarrierData;
        public DAL.NGLBookCarrierData NGLBookCarrierData
        {
            get { if (_NGLBookCarrierData == null) { _NGLBookCarrierData = (DAL.NGLBookCarrierData)NDPBaseClassFactory("NGLBookCarrierData"); } return _NGLBookCarrierData; }
            set { _NGLBookCarrierData = value; }
        }

        private DAL.NGLBookFinancialData _NGLBookFinancialData;
        public DAL.NGLBookFinancialData NGLBookFinancialData
        {
            get { if (_NGLBookFinancialData == null) { _NGLBookFinancialData = (DAL.NGLBookFinancialData)NDPBaseClassFactory("NGLBookFinancialData"); } return _NGLBookFinancialData; }
            set { _NGLBookFinancialData = value; }
        }

        private DAL.NGLBookLoadDetailData _NGLBookLoadDetailData;
        public DAL.NGLBookLoadDetailData NGLBookLoadDetailData
        {
            get { if (_NGLBookLoadDetailData == null) { _NGLBookLoadDetailData = (DAL.NGLBookLoadDetailData)NDPBaseClassFactory("NGLBookLoadDetailData"); } return _NGLBookLoadDetailData; }
            set { _NGLBookLoadDetailData = value; }
        }

        private DAL.NGLBookFeeData _NGLBookFeeData;
        public DAL.NGLBookFeeData NGLBookFeeData
        {
            get { if (_NGLBookFeeData == null) { _NGLBookFeeData = (DAL.NGLBookFeeData)NDPBaseClassFactory("NGLBookFeeData"); } return _NGLBookFeeData; }
            set { _NGLBookFeeData = value; }
        }

        private DAL.NGLCarrierData _NGLCarrierData;
        public DAL.NGLCarrierData NGLCarrierData
        {
            get { if (_NGLCarrierData == null) { _NGLCarrierData = (DAL.NGLCarrierData)NDPBaseClassFactory("NGLCarrierData"); } return _NGLCarrierData; }
            set { _NGLCarrierData = value; }
        }

        private DAL.NGLCarrierEquipCodeData _NGLCarrierEquipCodeData;
        public DAL.NGLCarrierEquipCodeData NGLCarrierEquipCodeData
        {
            get { if (_NGLCarrierEquipCodeData == null) { _NGLCarrierEquipCodeData = (DAL.NGLCarrierEquipCodeData)NDPBaseClassFactory("NGLCarrierEquipCodeData"); } return _NGLCarrierEquipCodeData; }
            set { _NGLCarrierEquipCodeData = value; }
        }

        private DAL.NGLCarrierFuelData _NGLCarrierFuelData;
        public DAL.NGLCarrierFuelData NGLCarrierFuelData
        {
            get { if (_NGLCarrierFuelData == null) { _NGLCarrierFuelData = (DAL.NGLCarrierFuelData)NDPBaseClassFactory("NGLCarrierFuelData"); } return _NGLCarrierFuelData; }
            set { _NGLCarrierFuelData = value; }
        }

        private DAL.NGLCarrierFuelAddendumData _NGLCarrierFuelAddendumData;
        public DAL.NGLCarrierFuelAddendumData NGLCarrierFuelAddendumData
        {
            get { if (_NGLCarrierFuelAddendumData == null) { _NGLCarrierFuelAddendumData = (DAL.NGLCarrierFuelAddendumData)NDPBaseClassFactory("NGLCarrierFuelAddendumData"); } return _NGLCarrierFuelAddendumData; }
            set { _NGLCarrierFuelAddendumData = value; }
        }

        private DAL.NGLCarrierFuelAdExData _NGLCarrierFuelAdExData;
        public DAL.NGLCarrierFuelAdExData NGLCarrierFuelAdExData
        {
            get { if (_NGLCarrierFuelAdExData == null) { _NGLCarrierFuelAdExData = (DAL.NGLCarrierFuelAdExData)NDPBaseClassFactory("NGLCarrierFuelAdExData"); } return _NGLCarrierFuelAdExData; }
            set { _NGLCarrierFuelAdExData = value; }
        }

        private DAL.NGLCarrierFuelAdRateData _NGLCarrierFuelAdRateData;
        public DAL.NGLCarrierFuelAdRateData NGLCarrierFuelAdRateData
        {
            get { if (_NGLCarrierFuelAdRateData == null) { _NGLCarrierFuelAdRateData = (DAL.NGLCarrierFuelAdRateData)NDPBaseClassFactory("NGLCarrierFuelAdRateData"); } return _NGLCarrierFuelAdRateData; }
            set { _NGLCarrierFuelAdRateData = value; }
        }

        private DAL.NGLCarrierFuelStateData _NGLCarrierFuelStateData;
        public DAL.NGLCarrierFuelStateData NGLCarrierFuelStateData
        {
            get { if (_NGLCarrierFuelStateData == null) { _NGLCarrierFuelStateData = (DAL.NGLCarrierFuelStateData)NDPBaseClassFactory("NGLCarrierFuelStateData"); } return _NGLCarrierFuelStateData; }
            set { _NGLCarrierFuelStateData = value; }
        }

        private DAL.NGLCarrFeeData _NGLCarrFeeData;
        public DAL.NGLCarrFeeData NGLCarrFeeData
        {
            get { if (_NGLCarrFeeData == null) { _NGLCarrFeeData = (DAL.NGLCarrFeeData)NDPBaseClassFactory("NGLCarrFeeData"); } return _NGLCarrFeeData; }
            set { _NGLCarrFeeData = value; }
        }

        private DAL.NGLCarrTarContractData _NGLCarrTarContractData;
        public DAL.NGLCarrTarContractData NGLCarrTarContractData
        {
            get { if (_NGLCarrTarContractData == null) { _NGLCarrTarContractData = (DAL.NGLCarrTarContractData)NDPBaseClassFactory("NGLCarrTarContractData"); } return _NGLCarrTarContractData; }
            set { _NGLCarrTarContractData = value; }
        }
            
        private DAL.NGLCarrierContData _NGLCarrierContData;
        public DAL.NGLCarrierContData NGLCarrierContData
        {
            get { if (_NGLCarrierContData == null) { _NGLCarrierContData = (DAL.NGLCarrierContData)NDPBaseClassFactory("NGLCarrierContData"); } return _NGLCarrierContData; }
            set { _NGLCarrierContData = value; }
        }
        
        private DAL.NGLCarrierTariffNoDriveDays _NGLCarrierTariffNoDriveDays;
        public DAL.NGLCarrierTariffNoDriveDays NGLCarrierTariffNoDriveDays
        {
            get { if (_NGLCarrierTariffNoDriveDays == null) { _NGLCarrierTariffNoDriveDays = (DAL.NGLCarrierTariffNoDriveDays)NDPBaseClassFactory("NGLCarrierTariffNoDriveDays"); } return _NGLCarrierTariffNoDriveDays; }
            set { _NGLCarrierTariffNoDriveDays = value; }
        }
        private DAL.NGLCarrTarFeeData _NGLCarrTarFeeData;
        public DAL.NGLCarrTarFeeData NGLCarrTarFeeData
        {
            get { if (_NGLCarrTarFeeData == null) { _NGLCarrTarFeeData = (DAL.NGLCarrTarFeeData)NDPBaseClassFactory("NGLCarrTarFeeData"); } return _NGLCarrTarFeeData; }
            set { _NGLCarrTarFeeData = value; }
        }

        private DAL.NGLCarrTarEquipData _NGLCarrTarEquipData;
        public DAL.NGLCarrTarEquipData NGLCarrTarEquipData
        {
            get { if (_NGLCarrTarEquipData == null) { _NGLCarrTarEquipData = (DAL.NGLCarrTarEquipData)NDPBaseClassFactory("NGLCarrTarEquipData"); } return _NGLCarrTarEquipData; }
            set { _NGLCarrTarEquipData = value; }
        }

        private DAL.NGLCompData _NGLCompData;
        public DAL.NGLCompData NGLCompData
        {
            get { if (_NGLCompData == null) { _NGLCompData = (DAL.NGLCompData)NDPBaseClassFactory("NGLCompData"); } return _NGLCompData; }
            set { _NGLCompData = value; }
        }
                           
        private DAL.NGLCompParameterData _NGLCompParameterData;
        public DAL.NGLCompParameterData NGLCompParameterData
        {
            get { if (_NGLCompParameterData == null) { _NGLCompParameterData = (DAL.NGLCompParameterData)NDPBaseClassFactory("NGLCompParameterData"); } return _NGLCompParameterData; }
            set { _NGLCompParameterData = value; }
        }
                   
        private DAL.NGLCompContData _NGLCompContData;
        public DAL.NGLCompContData NGLCompContData
        {
            get { if (_NGLCompContData == null) { _NGLCompContData = (DAL.NGLCompContData)NDPBaseClassFactory("NGLCompContData"); } return _NGLCompContData; }
            set { _NGLCompContData = value; }
        }

        private DAL.NGLBatchProcessDataProvider _NGLBatchProcessData;
        public DAL.NGLBatchProcessDataProvider NGLBatchProcessData
        {
            get { if (_NGLBatchProcessData == null) { _NGLBatchProcessData = (DAL.NGLBatchProcessDataProvider)NDPBaseClassFactory("NGLBatchProcessDataProvider"); } return _NGLBatchProcessData; }
            set { _NGLBatchProcessData = value; }
        }

        private DAL.NGLLaneData _NGLLaneData;
        public DAL.NGLLaneData NGLLaneData
        {
            get { if (_NGLLaneData == null) { _NGLLaneData = (DAL.NGLLaneData)NDPBaseClassFactory("NGLLaneData"); } return _NGLLaneData; }
            set { _NGLLaneData = value; }
        }

        private DAL.NGLLaneFeeData _NGLLaneFeeData;
        public DAL.NGLLaneFeeData NGLLaneFeeData
        {
            get { if (_NGLLaneFeeData == null) { _NGLLaneFeeData = (DAL.NGLLaneFeeData)NDPBaseClassFactory("NGLLaneFeeData"); } return _NGLLaneFeeData; }
            set { _NGLLaneFeeData = value; }
        }

        private DAL.NGLtblAccessorialData _NGLtblAccessorialData;
        public DAL.NGLtblAccessorialData NGLtblAccessorialData
        {
            get { if (_NGLtblAccessorialData == null) { _NGLtblAccessorialData = (DAL.NGLtblAccessorialData)NDPBaseClassFactory("NGLtblAccessorialData"); } return _NGLtblAccessorialData; }
            set { _NGLtblAccessorialData = value; }
        }

        private DAL.NGLFlatFileImport _NGLFlatFileImport;
        public DAL.NGLFlatFileImport NGLFlatFileImport
        {
            get { if (_NGLFlatFileImport == null) { _NGLFlatFileImport = (DAL.NGLFlatFileImport)NDPBaseClassFactory("NGLFlatFileImport"); } return _NGLFlatFileImport; }
            set { _NGLFlatFileImport = value; }
        }

        //Added By LVV on 12/23/16 for v-8.0 Content Management Tables
        private DAL.NGLLoadTenderData _NGLLoadTenderData;
        public DAL.NGLLoadTenderData NGLLoadTenderData
        {
            get { if (_NGLLoadTenderData == null) { _NGLLoadTenderData = (DAL.NGLLoadTenderData)NDPBaseClassFactory("NGLLoadTenderData"); } return _NGLLoadTenderData; }
            set { _NGLLoadTenderData = value; }
        }

        private DAL.NGLLookupDataProvider _NGLLookupData;
        public DAL.NGLLookupDataProvider NGLLookupData
        {
            get
            {
                if (_NGLLookupData == null)
                {
                    if ((Parameters != null)) { _NGLLookupData = new DAL.NGLLookupDataProvider(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLLookupData;
            }
            set { _NGLLookupData = value; }
        }

        private DAL.NGLCompParameterRefSystemData _NGLCompParameterRefSystemData;
        public DAL.NGLCompParameterRefSystemData NGLCompParameterRefSystemData
        {
            get
            {
                if (_NGLCompParameterRefSystemData == null)
                {
                    if ((Parameters != null)) { _NGLCompParameterRefSystemData = new DAL.NGLCompParameterRefSystemData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLCompParameterRefSystemData;
            }
            set { _NGLCompParameterRefSystemData = value; }
        }

        //Added By LVV on 5/2/18 for v-8.1
        private DAL.NGLEmailData _NGLEmailData;
        public DAL.NGLEmailData NGLEmailData
        {
            get { if (_NGLEmailData == null) { _NGLEmailData = (DAL.NGLEmailData)NDPBaseClassFactory("NGLEmailData"); } return _NGLEmailData; }
            set { _NGLEmailData = value; }
        }

        private DAL.NGLBidData _NGLBidData;
        public DAL.NGLBidData NGLBidData
        {
            get
            {
                if (_NGLBidData == null)
                {
                    if ((Parameters != null)) { _NGLBidData = new DAL.NGLBidData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLBidData;
            }
            set { _NGLBidData = value; }
        }

        private DAL.NGLSecurityDataProvider _NGLSecurityData;
        public DAL.NGLSecurityDataProvider NGLSecurityData
        {
            get
            {
                if (_NGLSecurityData == null)
                {
                    if ((Parameters != null)) { _NGLSecurityData = new DAL.NGLSecurityDataProvider(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLSecurityData;
            }
            set { _NGLSecurityData = value; }
        }

        private DAL.NGLLECompAccessorialData _NGLLECompAccessorialData;
        public DAL.NGLLECompAccessorialData NGLLECompAccessorialData
        {
            get
            {
                if (_NGLLECompAccessorialData == null)
                {
                    if ((Parameters != null)) { _NGLLECompAccessorialData = new DAL.NGLLECompAccessorialData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLLECompAccessorialData;
            }
            set { _NGLLECompAccessorialData = value; }
        }

        private DAL.NGLAMSAppointmentData _NGLAMSAppointmentData;
        public DAL.NGLAMSAppointmentData NGLAMSAppointmentData
        {
            get { if (_NGLAMSAppointmentData == null) { _NGLAMSAppointmentData = (DAL.NGLAMSAppointmentData)NDPBaseClassFactory("NGLAMSAppointmentData"); } return _NGLAMSAppointmentData; }
            set { _NGLAMSAppointmentData = value; }
        }

        private DAL.NGLDockSettingData _NGLDockSettingData;
        public DAL.NGLDockSettingData NGLDockSettingData
        {
            get
            {
                if (_NGLDockSettingData == null)
                {
                    if ((Parameters != null)) { _NGLDockSettingData = new DAL.NGLDockSettingData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLDockSettingData;
            }
            set { _NGLDockSettingData = value; }
        }

        private DAL.NGLLegalEntityCarrierData _NGLLegalEntityCarrierData;
        public DAL.NGLLegalEntityCarrierData NGLLegalEntityCarrierData
        {
            get
            {
                if (_NGLLegalEntityCarrierData == null)
                {
                    if ((Parameters != null)) { _NGLLegalEntityCarrierData = new DAL.NGLLegalEntityCarrierData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLLegalEntityCarrierData;
            }
            set { _NGLLegalEntityCarrierData = value; }
        }

        private DAL.NGLLECarrierAccessorialData _NGLLECarrierAccessorialData;
        public DAL.NGLLECarrierAccessorialData NGLLECarrierAccessorialData
        {
            get
            {
                if (_NGLLECarrierAccessorialData == null)
                {
                    if ((Parameters != null)) { _NGLLECarrierAccessorialData = new DAL.NGLLECarrierAccessorialData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLLECarrierAccessorialData;
            }
            set { _NGLLECarrierAccessorialData = value; }
        }

        private DAL.NGLtblSingleSignOnAccountData _NGLtblSingleSignOnAccountData;
        public DAL.NGLtblSingleSignOnAccountData NGLtblSingleSignOnAccountData
        {
            get { if (_NGLtblSingleSignOnAccountData == null) { _NGLtblSingleSignOnAccountData = (DAL.NGLtblSingleSignOnAccountData)NDPBaseClassFactory("NGLtblSingleSignOnAccountData"); } return _NGLtblSingleSignOnAccountData; }
            set { _NGLtblSingleSignOnAccountData = value; }
        }

        private DAL.NGLUserSecurityLegalEntityData _NGLUserSecurityLegalEntityData;
        public DAL.NGLUserSecurityLegalEntityData NGLUserSecurityLegalEntityData
        {
            get
            {
                if (_NGLUserSecurityLegalEntityData == null)
                {
                    if ((Parameters != null)) { _NGLUserSecurityLegalEntityData = new DAL.NGLUserSecurityLegalEntityData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLUserSecurityLegalEntityData;
            }
            set { _NGLUserSecurityLegalEntityData = value; }
        }

        private DAL.NGLPOHdrData _NGLPOHdrData;
        public DAL.NGLPOHdrData NGLPOHdrData
        {
            get { if (_NGLPOHdrData == null) { _NGLPOHdrData = (DAL.NGLPOHdrData)NDPBaseClassFactory("NGLPOHdrData"); } return _NGLPOHdrData; }
            set { _NGLPOHdrData = value; }
        }

        private DAL.NGLPOItemData _NGLPOItemData;
        public DAL.NGLPOItemData NGLPOItemData
        {
            get { if (_NGLPOItemData == null) { _NGLPOItemData = (DAL.NGLPOItemData)NDPBaseClassFactory("NGLPOItemData"); } return _NGLPOItemData; }
            set { _NGLPOItemData = value; }
        }

        private DAL.NGLUserSecurityCarrierData _NGLUserSecurityCarrierData;
        public DAL.NGLUserSecurityCarrierData NGLUserSecurityCarrierData
        {
            get
            {
                if (_NGLUserSecurityCarrierData == null)
                {
                    if ((Parameters != null)) { _NGLUserSecurityCarrierData = new DAL.NGLUserSecurityCarrierData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLUserSecurityCarrierData;
            }
            set { _NGLUserSecurityCarrierData = value; }
        }

        private DAL.NGLUserGroupData _NGLUserGroupData;
        public DAL.NGLUserGroupData NGLUserGroupData
        {
            get
            {
                if (_NGLUserGroupData == null)
                {
                    if ((Parameters != null)) { _NGLUserGroupData = new DAL.NGLUserGroupData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLUserGroupData;
            }
            set { _NGLUserGroupData = value; }
        }

        private DAL.NGLUserAdminData _NGLUserAdminData;
        public DAL.NGLUserAdminData NGLUserAdminData
        {
            get
            {
                if (_NGLUserAdminData == null)
                {
                    if ((Parameters != null)) { _NGLUserAdminData = new DAL.NGLUserAdminData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLUserAdminData;
            }
            set { _NGLUserAdminData = value; }
        }

        private DAL.NGLUserLaneData _NGLUserLaneData;
        /// <summary>
        /// User Lane Data Data Access Object
        /// </summary>
        /// <remarks>
        /// Created by RHR for v-8.4.0.002 on 05/14/2021 -new NGLUserLaneData
        /// </remarks>
        public DAL.NGLUserLaneData NGLUserLaneData
        {
            get
            {
                if (_NGLUserLaneData == null)
                {
                    if ((Parameters != null)) { _NGLUserLaneData = new DAL.NGLUserLaneData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLUserLaneData;
            }
            set { _NGLUserLaneData = value; }
        }

        private DAL.NGLAPMassEntryData _NGLAPMassEntryData;
        public DAL.NGLAPMassEntryData NGLAPMassEntryData
        {
            get
            {
                if (_NGLAPMassEntryData == null)
                {
                    if ((Parameters != null)) { _NGLAPMassEntryData = new DAL.NGLAPMassEntryData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLAPMassEntryData;
            }
            set { _NGLAPMassEntryData = value; }
        }

        private DAL.NGLAPMassEntryMsg _NGLAPMassEntryMsg;
        public DAL.NGLAPMassEntryMsg NGLAPMassEntryMsg
        {
            get
            {
                if (_NGLAPMassEntryMsg == null)
                {
                    if ((Parameters != null)) { _NGLAPMassEntryMsg = new DAL.NGLAPMassEntryMsg(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLAPMassEntryMsg;
            }
            set { _NGLAPMassEntryMsg = value; }
        }

        private DAL.NGLAPMassEntryFees _NGLAPMassEntryFees;
        public DAL.NGLAPMassEntryFees NGLAPMassEntryFees
        {
            get
            {
                if (_NGLAPMassEntryFees == null)
                {
                    if ((Parameters != null)) { _NGLAPMassEntryFees = new DAL.NGLAPMassEntryFees(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLAPMassEntryFees;
            }
            set { _NGLAPMassEntryFees = value; }
        }

        private DAL.NGLLegalEntityAdminData _NGLLegalEntityAdminData;
        public DAL.NGLLegalEntityAdminData NGLLegalEntityAdminData
        {
            get
            {
                if (_NGLLegalEntityAdminData == null)
                {
                    if ((Parameters != null)) { _NGLLegalEntityAdminData = new DAL.NGLLegalEntityAdminData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLLegalEntityAdminData;
            }
            set { _NGLLegalEntityAdminData = value; }
        }

        private DAL.NGLEDIData _NGLEDIData;
        public DAL.NGLEDIData NGLEDIData
        {
            get { if (_NGLEDIData == null) { _NGLEDIData = (DAL.NGLEDIData)NDPBaseClassFactory("NGLEDIData"); } return _NGLEDIData; }
            set { _NGLEDIData = value; }
        }

        private DAL.NGLWhatsNewData _NGLWhatsNewData;
        public DAL.NGLWhatsNewData NGLWhatsNewData
        {
            get
            {
                if (_NGLWhatsNewData == null)
                {
                    if ((Parameters != null)) { _NGLWhatsNewData = new DAL.NGLWhatsNewData(Parameters); } else { throw new System.InvalidOperationException("Parameters Are Required"); }
                }
                return _NGLWhatsNewData;
            }
            set { _NGLWhatsNewData = value; }
        }

        //Added By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes
        private DAL.NGLCompCreditData _NGLCompCreditData;
        public DAL.NGLCompCreditData NGLCompCreditData
        {
            get { if (_NGLCompCreditData == null) { _NGLCompCreditData = (DAL.NGLCompCreditData)NDPBaseClassFactory("NGLCompCreditData"); } return _NGLCompCreditData; }
            set { _NGLCompCreditData = value; }
        }


        #endregion

        #region " shared Rest Services"

        /// <summary>
        /// Overridable Rest service to read the page settings for this specific page based on the sub class Page property
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018
        /// </remarks>
        [HttpGet, ActionName("GetPageSettings")]
        public virtual Models.Response GetPageSettings(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                //remove double quotes generated by JSON
                filter = filter.Replace("\"", "");
                LTS.tblUserPageSetting[] oSettings = this.readPageSettings(filter);
                Models.PageSetting[] records = new Models.PageSetting[] { };
                if (oSettings != null && oSettings.Count() > 0)
                {
                    count = oSettings.Count();
                    List<Models.PageSetting> lPageSettings = new List<Models.PageSetting>();
                    foreach (LTS.tblUserPageSetting s in oSettings)
                    {
                        Models.PageSetting lsetting = new Models.PageSetting() { name = s.UserPSName, value = s.UserPSMetaData };
                        lPageSettings.Add(lsetting);
                    }
                    records = lPageSettings.ToArray();
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

        /// <summary>
        /// Overridable Rest service to save the page setting for this specific page based on the sub class Page property
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018
        /// Modified by RHR for v-8.2.1.006 on 04/15/2020
        ///   Fixed bug where exception is thrown when (int)Page = 0
        /// </remarks>
        [HttpPost, ActionName("PostPageSetting")]
        public virtual Models.Response PostPageSetting([System.Web.Http.FromBody]Models.PageSetting data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            // Modified by RHR for v-8.2.1.006 on 04/15/2020
            if ((int)Page == 0) {
                return response;
            }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblUserPageSetting fSettings = new LTS.tblUserPageSetting { UserPSName = data.name, UserPSPageControl = (int)Page, UserPSUserSecurityControl = Parameters.UserControl, UserPSMetaData = data.value };
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                bool blnRet = sDaL.SaveCurrentUserPageSetting(fSettings);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
                response = new Models.Response(oRecords, 1);
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

        /// <summary>
        /// Get PageSettings for a specific grid and the current User
        /// </summary>
        /// <remarks>
        /// Created by CHA on 07/28/2021
        /// </remarks>
        [HttpGet, ActionName("Filters")]
        public virtual Response Filters(string gridId)
        {
            var response = new Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                LTS.tblUserPageSetting[] oSettings = sDaL.GetPageSettingsByGridForCurrentUser((int)Page, gridId);
                if (oSettings != null && oSettings.Length > 0)
                {
                    var allFilters = oSettings.Select(x => new GridFilters
                    {
                        Name = x.UserPSName.Split('_').First(),
                        FilterId = x.UserPSName,
                        Filters = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<AllFilters>(x.UserPSMetaData).FilterValues.ToArray()
                    }).ToArray();

                    response = new Response(allFilters, oSettings.Length);
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

        /// <summary>
        /// Saves filters
        /// </summary>
        /// <remarks>
        /// Created by CHA on 06/30/2021
        /// </remarks>
        [HttpPost, ActionName("SaveFilter")]
        public virtual Response SaveFilter(string filterName, string filter, string gridId)
        {
            var response = new Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrWhiteSpace(filterName) && !string.IsNullOrWhiteSpace(gridId)) 
                {
                    var filterKeyId = filterName + $"_{Guid.NewGuid()}";
                    savePageFilters(filter: filter, filterKey: filterKeyId, gridId: gridId);
                    response = new Response(new[] { filterKeyId }, 1);
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

        /// <summary>
        /// Delete filter by id
        /// </summary>
        /// <remarks>
        /// Created by CHA on 08/31/2021
        /// </remarks>
        [HttpDelete, ActionName("DeleteFilter")]
        public virtual Response DeleteFilter(string filterId, string gridId)
        {
            var response = new Response();
            if (!authenticateController(ref response) || string.IsNullOrWhiteSpace(filterId) || string.IsNullOrWhiteSpace(gridId)) { return response; }
            try
            {
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                bool result = sDaL.DeleteFilterByNameAndGridForCurrentUser(filterId, (int)Page, gridId);
                response = new Models.Response(new[] { result }, 1);
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


        #endregion

        #region " Methods"

        /// <summary>
        /// post page settings using page id
        /// </summary>
        /// <param name="data"></param>
        /// <param name="UserPSPageControl"></param>
        /// <remarks>
        /// Caller must manage exceptions
        /// </remarks>
        protected void PostPageSetting(Models.PageSetting data, Utilities.PageEnum ePage  )
        {
                LTS.tblUserPageSetting fSettings = new LTS.tblUserPageSetting { UserPSName = data.name, UserPSPageControl = (int)ePage, UserPSUserSecurityControl = Parameters.UserControl, UserPSMetaData = data.value };
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                bool blnRet = sDaL.SaveCurrentUserPageSetting(fSettings);
               
            
        }


        protected DAL.NDPBaseClass NDPBaseClassFactory(string source)
        {
            if ((Parameters != null))
            {
                try
                {
                    string typename = "Ngl.FreightMaster.Data." + source;
                    Type t = typeof(Ngl.FreightMaster.Data.NDPBaseClass).Assembly.GetType(typename);
                    DAL.NDPBaseClass newC = Activator.CreateInstance(t, new object[] { Parameters }) as DAL.NDPBaseClass;
                    if (newC == null)
                    {
                        throw new System.InvalidCastException("The class " + source + " is not a valid NDPBaseClass");
                    }
                    return newC;
                }
                catch (FaultException ex)
                {
                    throw;
                }
                catch (System.NullReferenceException ex)
                {
                    throw new System.InvalidCastException("The class " + source + " is not a valid NDPBaseClass");
                }
                catch (System.ArgumentNullException ex)
                {
                    throw new System.InvalidCastException("The class " + source + " cannot be found or is not a valid NDPBaseClass");
                }
                catch (System.MissingMethodException ex)
                {
                    throw new System.InvalidCastException("The class " + source + " does not support the required constructor.  It may not be a valid NDPBaseClass");
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                throw new System.InvalidOperationException("Parameters Are Required");
            }
        }


        protected void populateSampleParameterData()
        {
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
                NGLSystemData.CreateAppErrorByMessage(Message, Parameters.UserName);
            } catch (Exception ex) {
                //we ignore all errors while saving application error data
            }           
        }


        public void SaveSysError(string Message, string errorProcedure, string record = "", int errorNumber = 0, int errorSeverity = 0, int errorState = 0, int errorLineNber = 0)
        {
            try
            {
                NGLSystemData.CreateSystemErrorByMessage(Message, Parameters.UserName, errorProcedure, record, errorNumber, errorSeverity, errorState, errorLineNber);
            } catch (Exception ex) {
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
        protected bool executeNGLStoredProcedure(string BatchName, string ProcName,DTO.NGLStoredProcedureParameter[] ProcPars)
        {
            try
            {
                return NGLBatchProcessData.executeNGLStoredProcedure(BatchName, ProcName, ProcPars);
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {                    
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",sqlEx.Reason.ToString(),sqlEx.Detail.Message,sqlEx.Detail.Details,sqlEx.ToString());
                SaveSysError(errMsg,sourcePath("executeNGLStoredProcedure"),ProcName); 
            }
            catch (Exception ex)
            {
                throw;
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
            try
            {
                return NGLBatchProcessData.executeNGLStoredProcedure(ProcName, ProcName, ProcPars.ToArray());
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                SaveSysError(errMsg, sourcePath("executeNGLStoredProcedure"), ProcName);
            }
            catch (Exception ex)
            {
                throw;
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
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}",sqlEx.Reason.ToString(),sqlEx.Detail.Message,sqlEx.Detail.Details,sqlEx.ToString());
                SaveSysError(errMsg,sourcePath("executeSQL"),strSQL); 
            }
            catch (Exception ex)
            {
                throw;
            } 
            return false;
        }

        protected int getScalarInteger(string strSQL ) 
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
            catch (Exception ex) { throw; } 
            return intRet;
        }

        protected string getScalarString(string strSQL )
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
            catch (Exception ex) { throw; }
            return strRet;
        }

        protected string sourcePath(string caller)
        {
            return string.Concat(SourceClass,'.',caller);
        }

       protected bool AuthenticateClientCredentials(ref Models.Response response)
        {
            bool blnSuccess = true;
            int uc = 0;
            string sAuthCode = HttpContext.Current.Request.Headers["Authorization"];
            string s64 = sAuthCode.Substring(6);
            byte[] bytes = System.Convert.FromBase64String(s64);
            string sdecoded = System.Text.Encoding.UTF8.GetString(bytes);
            string[] sAuthParts = sdecoded.Split(':');
            response.TokenAuthCode = sAuthParts[0];
            response.TokenSecrectCode = sAuthParts[1];
            //string susc = HttpContext.Current.Request.Headers["USC"];
            //if (string.IsNullOrWhiteSpace(sAuthCode) || string.IsNullOrWhiteSpace(susc))
            //{
            //    response.StatusCode = HttpStatusCode.Unauthorized;
            //    response.Errors = Utilities.getLocalizedMsg("E_NotAuthService");
            //    blnSuccess = false;
            //    return blnSuccess;
            //}
            //string sBasicAuth = sclient_id + ":" + sclient_secret;
            //System.Diagnostics.Debug.WriteLine(sAuthCode);
            return blnSuccess;
        }


        protected string AuthenticateReadAccountFromToken(ref string sAccountNumber )
        {
            string sRet = null;
            int uc = 0;
            string sAuthCode = HttpContext.Current.Request.Headers["Authorization"];
            string s64 = sAuthCode.Substring(6);
            byte[] bytes = System.Convert.FromBase64String(s64);
            string sUserToken = System.Text.Encoding.UTF8.GetString(bytes);
            string sTmpSecret = Guid.NewGuid().ToString().Replace("-", "");
            // Console.WriteLine("Secrect: " + sTmpSecret);
            AuthToken oAuthData = new AuthToken();
            string sTempKey = oAuthData.ComputeContentHash(sTmpSecret);
           // Console.WriteLine("Key: " + sKey);


            AuthenticationService.Managers.IAuthService authService = new AuthenticationService.Managers.JWTService(sTempKey);
            List<System.Security.Claims.Claim> claims = authService.ReadTokenClaims(sUserToken).ToList();
            sAccountNumber = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Authentication)).Value;
           
            return sUserToken;
        }
        /// <summary>
        /// checks the active token and usercontrol number passed in from the client via the Request.Headers; stores the user control number in the UserControl property
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns
        /// <remarks>
        /// Modified by RHR for v-8.0 on 09/12/2017
        ///   added localizaton code for messages
        /// </remarks>
        protected bool authenticateController(ref Models.Response response)
        {
            bool blnSuccess = true;
            int uc = 0;
            string token = HttpContext.Current.Request.Headers["Authorization"];
            string susc = HttpContext.Current.Request.Headers["USC"];
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(susc))
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.Errors = Utilities.getLocalizedMsg("E_NotAuthService");
                blnSuccess = false;
                return blnSuccess;
            }
            int.TryParse(susc, out uc);
            //controller authentication
            if (Utilities.GlobalSSOResultsByUser.ContainsKey(uc))
            {
                DAL.Models.SSOResults res = Utilities.GlobalSSOResultsByUser[uc];
                //modified by RHR for v-8.5.3.007 on 6/14/2023 to fix issue with token expiration
                //if (res == null || res.willTokenExpire(120, token))
                //{
                //    response.StatusCode = HttpStatusCode.NonAuthoritativeInformation;
                //    response.Errors = Utilities.getLocalizedMsg("E_AuthWillExpire");
                //    blnSuccess = false;
                //}
            }
            else
            {
                //refresh the SSOResponse data
                LTS.vSSOResult sResult = new LTS.vSSOResult();
                try
                {
                    DAL.NGLSecurityDataProvider DALSec = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
                    sResult = DALSec.GetUpdatedSSOResults(uc);
                } catch (Exception ex)
                {
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.Errors = Utilities.getLocalizedMsg("E_AccessDenied");
                    return false;
                }
                if (sResult == null || string.IsNullOrWhiteSpace(sResult.USATToken))
                {
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.Errors = Utilities.getLocalizedMsg("E_AccessDenied");
                    return false;
                } else
                {
                    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime dtExpires = sResult.USATExpires.HasValue ? sResult.USATExpires.Value : DateTime.Now.AddHours(1);
                    DAL.Models.SSOResults mResult = new DAL.Models.SSOResults()
                    {
                        CatControl = sResult.CatControl.HasValue ? sResult.CatControl.Value : 0,
                        IsUserCarrier = sResult.IsCarrier == 1 ? true : false,
                        SSOAAuthCode = sResult.SSOAAuthCode,
                        SSOAClientID = sResult.SSOAClientID,
                        SSOAClientSecret = sResult.SSOAClientSecret,
                        SSOAControl = sResult.UserSSOAControl,
                        SSOAExpires = Convert.ToInt32(Math.Floor((dtExpires.ToUniversalTime() - origin).TotalSeconds)),
                        SSOAExpiresMilli = Convert.ToInt64(Math.Floor((dtExpires.ToUniversalTime() - origin).TotalMilliseconds)),
                        SSOALoginURL = sResult.SSOALoginURL,
                        SSOAName = sResult.SSOAName,
                        SSOARedirectURL = sResult.SSOARedirectURL,
                        SSOAUserEmail = sResult.UserEmail,
                        USATToken = sResult.USATToken,
                        USATUserID = sResult.USATUserID,
                        UserCarrierContControl = sResult.CarrierContControl,
                        UserCarrierControl = sResult.CarrierControl,
                        UserFirstName = sResult.UserFirstName,
                        UserFriendlyName = sResult.UserFriendlyName,
                        UserLastName = sResult.UserLastName,
                        UserLEControl = sResult.UserLEControl,
                        UserName = sResult.UserName,
                        UserSecurityControl = uc,
                        UserTheme365 = sResult.UserTheme365,
                        UserTimeZone = sResult.UserTimeZone,
                        UserCultureInfo = sResult.UserCultureInfo,
                    };
                    if (mResult != null)
                    {
                        //modified by RHR for v-8.5.3.007 on 6/14/2023 to fix issue with token expiration
                        //if (mResult.willTokenExpire(120))
                        //{
                        //    response.StatusCode = HttpStatusCode.NonAuthoritativeInformation;
                        //    response.Errors = Utilities.getLocalizedMsg("E_AuthWillExpire");
                        //    return false;
                        //}
                        try
                        {
                            //in multi-threaded another process could have updated the key since we started
                            if (Utilities.GlobalSSOResultsByUser == null) { Utilities.GlobalSSOResultsByUser = new Dictionary<int, DAL.Models.SSOResults>(); }
                            if (Utilities.GlobalSSOResultsByUser.ContainsKey(uc)) { Utilities.GlobalSSOResultsByUser[uc] = mResult; } else { Utilities.GlobalSSOResultsByUser.Add(uc, mResult); }
                        }
                        catch (Exception ex)
                        {
                            //do nothing here
                        }                       
                    }
                }
            }
            UserControl = uc;
            return blnSuccess;
        }

        /// <summary>
        /// Verifies that the user is logged in as a Carrier or SuperUser and has an
        /// associated CarrierControl that is not zero
        /// Can be called from REST methods to make sure only Carriers have access to the data
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <remarks>
        /// Added by LVV on 10/15/2018
        /// </remarks>
        protected bool authenticateCarrier(ref Models.Response response)
        {
            bool blnSuccess = true;
            //If the user is a Carrier or SuperUser with an associated CarrierControl then they are allowed to execute this function, else unauthorized
            if (Parameters.UserCarrierControl == 0 || !(Parameters.CatControl == 2 || Parameters.CatControl == 4))
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.Errors = Utilities.getLocalizedMsg("E_NoAuthCarrier");
                blnSuccess = false;
            }
            return blnSuccess;
        }

        /// <summary>
        /// Verifies that the filters is not null
        /// </summary>
        /// <param name="response"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Added by LVV on 10/15/2018
        /// </remarks>
        protected bool authenticateFilter(ref Models.Response response, string filter)
        {
            bool blnSuccess = true;
            if (string.IsNullOrWhiteSpace(filter))
            {
                FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                response.StatusCode = HttpStatusCode.BadRequest;
                List<string> DetailsList = new List<string>();
                response.Errors = fault.formatMessage("SelectFilter", "E_InvalidFilter", DetailsList);
                blnSuccess = false;
            }
            return blnSuccess;
        }

        /// <summary>
        /// Returns a response with message when the data passed to the controller is null
        /// The message says "The record passed to the controller is null. Please refresh and try again. If the problem persists, please contact technical support."
        /// </summary>
        /// <returns></returns>
        /// <remarks>Added By LVV on 10/19/20 for v-8.3.0.001 Task #Task #20201020161708 - Add Master Carrier Page</remarks>
        protected Models.Response getNullDataMsgResponse()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
            response.StatusCode = HttpStatusCode.BadRequest;
            List<string> DetailsList = new List<string>();
            response.Errors = fault.formatMessage("Null Data", "E_NullControllerData", DetailsList);
            return response;
        }

        #region "AllFilters - Sort"
       
        protected bool isSortEmpty(DAL.Models.AllFilters f)
        {
            bool blnRet = false;
            if (f.SortValues?.Length < 1 || string.IsNullOrWhiteSpace(f.sortDirection)) { blnRet = true; }
            return blnRet;
        }

        /// <summary>
        /// If the user did not do any sorting then apply the default sort
        /// Note: Can only be used to add 1 sort, if more are needed call addToSort()
        /// </summary>
        /// <param name="f">AllFilters object, by reference</param>
        /// <param name="sortField">FieldName to sort by</param>
        /// <param name="blnSortAscending">If true sortDirection = "Asc", else sortDirection = "Desc"</param>
        /// <remarks>Added By LVV on 11/14/19</remarks>
        protected void applyDefaultSort(ref DAL.Models.AllFilters f, string sortField, bool blnSortAscending)
        {
            string sDirection = "Asc";
            if (!blnSortAscending) { sDirection = "Desc"; }
            if (isSortEmpty(f))
            {
                f.SortValues = new DAL.Models.SortDetails[] { new DAL.Models.SortDetails { sortName = sortField, sortDirection = sDirection } };
            }
        }

        /// <summary>
        /// Adds sort details to AllFilters object
        /// </summary>
        /// <param name="f"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <remarks>Added By LVV on 12/30/19</remarks>
        protected void addToSort(ref DAL.Models.AllFilters f, string sortField, bool blnSortAscending)
        {
            string sDirection = "Asc";
            if (!blnSortAscending) { sDirection = "Desc"; }
            if (isSortEmpty(f))
            {
                f.SortValues = new DAL.Models.SortDetails[] { new DAL.Models.SortDetails { sortName = sortField, sortDirection = sDirection } };
            }
            else
            {
                var list = f.SortValues.ToList();
                list.Add(new DAL.Models.SortDetails { sortName = sortField, sortDirection = sDirection });
                f.SortValues = list.ToArray();
            }
        }

        #endregion

        #region "AllFilters - Filter"

        /// <summary>
        /// Adds filters to AllFilters object
        /// </summary>
        /// <param name="f"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <remarks>Added By LVV on 12/30/19</remarks>
        protected void addToFilters(ref DAL.Models.AllFilters f, string fieldName, string fieldValue)
        {
            if (f.FilterValues?.Length < 1 || string.IsNullOrWhiteSpace(f.filterName))
            {
                f.filterName = fieldName;
                f.filterValue = fieldValue;
            }
            else
            {
                var list = f.FilterValues.ToList();
                list.Add(new DAL.Models.FilterDetails { filterName = fieldName, filterValueFrom = fieldValue });
                f.FilterValues = list.ToArray();
            }
        }

        /// <summary>
        /// Adds filters to AllFilters object
        /// </summary>
        /// <param name="f"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <remarks>Added By LVV on 12/30/19</remarks>
        protected void addToFilters(ref DAL.Models.AllFilters f, string fieldName, string fieldValueFrom, string fieldValueTo)
        {
            if (f.FilterValues?.Length < 1 || string.IsNullOrWhiteSpace(f.filterName))
            {
                var list = new List<DAL.Models.FilterDetails>() { new DAL.Models.FilterDetails { filterName = fieldName, filterValueFrom = fieldValueFrom, filterValueTo = fieldValueTo } };
                f.FilterValues = list.ToArray();
            }
            else
            {
                var list = f.FilterValues.ToList();
                list.Add(new DAL.Models.FilterDetails { filterName = fieldName, filterValueFrom = fieldValueFrom, filterValueTo = fieldValueTo });
                f.FilterValues = list.ToArray();
            }
        }

        #endregion

        /// <summary>
        /// Overridable wrapper method to read a specific array of page settings using key value
        /// </summary>
        /// <param name="skey"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018 
        /// </remarks>
        public virtual LTS.tblUserPageSetting[] readPageSettings(string skey)
        {           
            return readPageSettings(skey, this.Parameters, Page);          
        }

        public virtual int readBookControlPageSetting(int iBookControl)
        {
            if (iBookControl == 0)
            {
                LTS.tblUserPageSetting[] lPageSettings = readPageSettings("BookControl", this.Parameters, Page);
                if (lPageSettings != null && lPageSettings.Length > 0)
                {
                    foreach (var lPageSetting in lPageSettings)
                    {
                        if (lPageSetting != null && lPageSetting.UserPSName == "BookControl" && !string.IsNullOrWhiteSpace(lPageSetting.UserPSMetaData))
                        {
                            int.TryParse(lPageSetting.UserPSMetaData, out iBookControl);
                        }
                    }
                }              
            }
            return iBookControl;
        }

        /// <summary>
        /// Overridable wrapper method to read the saved primary key settings for the page 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018 
        /// </remarks>
        public virtual int readPagePrimaryKey()
        {
            return readPagePrimaryKey(this.Parameters, Page);           
        }

        public virtual string readPagePrimaryStringKey()
        {
            return readPagePrimaryStringKey(this.Parameters, Page);
        }

        /// <summary>
        /// Save the specific page filter using the Page property for the current user. 
        /// This overload will use the default page filter key value of AllRecordsFilter will be used to save the data.
        /// This overload does not return any exception details on failure
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018 
        /// </remarks>
        public virtual bool savePageFilters(string filter)
        {
            string retMsg = "";
            return savePageFilters(filter, ref retMsg);           
        }

        /// <summary>
        /// Save the specific page filter using the Page property for the current user.  
        /// The filterKey uses AllRecordsFilter by default, pages with multiple filters will need to provide a unique filter key 
        /// for each section, this value must also be stored in the cmPageDetail.PageDetAPIFilterID field of the data container (grid)
        /// to support content management default filter assignment on page load. 
        /// This overload does not return any exception details on failure
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filterKey"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018 
        /// </remarks>
        public virtual bool savePageFilters(string filter, string filterKey = "AllRecordsFilter")
        {
            string retMsg = "";
            return savePageFilters(filter, ref retMsg, filterKey);
        }

        /// <summary>
        /// Save the specific page filter using the Page property for the current user.  
        /// The filterKey uses AllRecordsFilter by default, pages with multiple filters will need to provide a unique filter key 
        /// for each section, this value must also be stored in the cmPageDetail.PageDetAPIFilterID field of the data container (grid)
        /// to support content management default filter assignment on page load. 
        /// This overload does not return any exception details on failure
        /// You should specifiy for which grid is the filter by passing the gridId
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filterKey"></param>
        /// <param name="gridId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by CHA on 07/27/2021 
        /// </remarks>
        public virtual bool savePageFilters(string filter, string filterKey = "AllRecordsFilter", string gridId = null)
        {
            string retMsg = "";
            return savePageFilters(filter, ref retMsg, filterKey, gridId);
        }

        /// <summary>
        /// Save the specific page filter using the Page property for the current user. 
        /// The filterKey uses AllRecordsFilter by default, pages with multiple filters will need to provide a unique filter key 
        /// for each section, this value must also be stored in the cmPageDetail.PageDetAPIFilterID field of the data container (grid)
        /// to support content management default filter assignment on page load.   
        /// This overload returns exception details in retMsg on failure
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="retMsg"></param>
        /// <param name="filterKey"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018 
        /// </remarks>
        public virtual bool savePageFilters(string filter, ref string retMsg, string filterKey = "AllRecordsFilter", string gridId = null)
        {
            retMsg = "";
            bool blnRet = false;
            try
            { 
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                var trimmedGridId = gridId?.Length > 20 ? gridId.Substring(0, 20) : gridId;
                LTS.tblUserPageSetting fSettings = new LTS.tblUserPageSetting 
                { 
                    UserPSName = filterKey, 
                    UserPSPageControl = (int)Page, 
                    UserPSUserSecurityControl = Parameters.UserControl, 
                    UserPSMetaData = filter,
                    UserPSAPIFilterID = trimmedGridId
                };
                blnRet = sDaL.SaveCurrentUserPageSetting(fSettings);
            } catch (Exception ex)
            {
                retMsg = string.Format("Cannot save filter informaiton for page {0}, Error may not have been shown to user: {1}", Page.ToString(), ex.Message);
                SaveAppError(retMsg);
            }
            return blnRet;
        }

        public virtual bool savePageSetting(string sData, ref string retMsg, string sKey )
        {
            retMsg = "";
            bool blnRet = false;
            try
            {
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                LTS.tblUserPageSetting fSettings = new LTS.tblUserPageSetting { UserPSName = sKey, UserPSPageControl = (int)Page, UserPSUserSecurityControl = Parameters.UserControl, UserPSMetaData = sData };
                blnRet = sDaL.SaveCurrentUserPageSetting(fSettings);
            }
            catch (Exception ex)
            {
                retMsg = string.Format("Cannot save page setting informaiton for page {0}, Error may not have been shown to user: {1}", Page.ToString(), ex.Message);
                SaveAppError(retMsg);
            }
            return blnRet;
        }

        /// <summary>
        /// Sends the email
        /// Returns false if response.Errors was populated, else true.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="Cc"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <returns></returns>
        public virtual bool sendEmail(ref Models.Response response, string From, string To, string Cc, string Subject, string Body)
        {
            DAL.NGLEmailData oMail = new DAL.NGLEmailData(Parameters);
            try
            {
                oMail.GenerateEmail(From, To, Cc, Subject, Body, "", "", "", "");
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                if (ex.GetType() == typeof(FaultException<DAL.SqlFaultInfo>))
                {
                    string sMsg = "";
                    string sReason = "";
                    if (((FaultException<DAL.SqlFaultInfo>)ex).Detail != null)
                    {
                        sMsg = ((FaultException<DAL.SqlFaultInfo>)ex).Detail.Message;
                        sReason = ((FaultException<DAL.SqlFaultInfo>)ex).Reason.ToString();
                    }
                    if ((!string.IsNullOrWhiteSpace(sMsg) && sMsg == "E_SQLExceptionMSG") || (!string.IsNullOrWhiteSpace(sReason) && sReason == "E_SQLExceptionMSG"))
                    {
                        //cannot generate email please try again
                        //SendEmailError                            
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = fault.formatMessage("", "SendEmailError", null);
                        return false;
                    }
                }
                fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return false;
            }
            return true;
        }


        #endregion

        #region " static methods"

        /// <summary>
        /// returns an array of page settings by key; generally one record in the array for a unique key
        /// </summary>
        /// <param name="skey"></param>
        /// <param name="Parameters"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018 
        /// </remarks>
        public static  LTS.tblUserPageSetting[] readPageSettings(string skey, DAL.WCFParameters Parameters, Utilities.PageEnum page)
        {
            DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
            return sDaL.GetPageSettingsForCurrentUser((int)page, skey);
        }

        /// <summary>
        /// Reads the primary key setting (for user) using key value "pk" for the specific page
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018
        /// </remarks>
        public static int readPagePrimaryKey(DAL.WCFParameters Parameters,Utilities.PageEnum page)
        {
            int id = 0;
            try
            {
                LTS.tblUserPageSetting[] keys = readPageSettings("pk", Parameters, page);
                if (keys != null && keys.Count() > 0)
                {
                    int.TryParse(keys[0].UserPSMetaData, out id);
                }
            } catch (Exception ex)
            {
                Utilities.SaveAppError(string.Format("Unable to read primary key for page {0}, Error not shown to user: {1}", page.ToString(), ex.Message));
            }
           
            return id;
        }

        public static String readPagePrimaryStringKey(DAL.WCFParameters Parameters, Utilities.PageEnum page)
        {
            string sid = "";
            try
            {
                LTS.tblUserPageSetting[] keys = readPageSettings("pk", Parameters, page);
                if (keys != null && keys.Count() > 0)
                {
                    sid = keys[0].UserPSMetaData;
                }
            }
            catch (Exception ex)
            {
                Utilities.SaveAppError(string.Format("Unable to read primary key for page {0}, Error not shown to user: {1}", page.ToString(), ex.Message));
            }

            return sid;
        }

        public static bool savePagePrimaryKey(DAL.WCFParameters Parameters, Utilities.PageEnum page,int iPK)
        {
            bool blnbRet = false;
            try
            {              
                LTS.tblUserPageSetting fSettings = new LTS.tblUserPageSetting { UserPSName = "pk", UserPSPageControl = (int)page, UserPSUserSecurityControl = Parameters.UserControl, UserPSMetaData = iPK.ToString() };
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                blnbRet = sDaL.SaveCurrentUserPageSetting(fSettings);
            }
            catch (Exception ex)
            {
                //Do nothing
            }

            return blnbRet;
        }



        /// <summary>
        /// Reads the consolidaton key setting (for user) using key value "cns" for the specific page
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.3.0.003 on 01/30/2021
        /// </remarks>
        public static string readPageCNSKey(DAL.WCFParameters Parameters, Utilities.PageEnum page)
        {
            string sCNS = "";
            try
            {
                LTS.tblUserPageSetting[] keys = readPageSettings("cns", Parameters, page);
                if (keys != null && keys.Count() > 0)
                {
                    sCNS = keys[0].UserPSMetaData;
                }
            }
            catch (Exception ex)
            {
                Utilities.SaveAppError(string.Format("Unable to read CNS key for page {0}, Error not shown to user: {1}", page.ToString(), ex.Message));
            }

            return sCNS;
        }

        public static bool savePageCNSKey(DAL.WCFParameters Parameters, Utilities.PageEnum page, string sCNS)
        {
            bool blnbRet = false;
            try
            {
                LTS.tblUserPageSetting fSettings = new LTS.tblUserPageSetting { UserPSName = "cns", UserPSPageControl = (int)page, UserPSUserSecurityControl = Parameters.UserControl, UserPSMetaData = sCNS };
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                blnbRet = sDaL.SaveCurrentUserPageSetting(fSettings);
            }
            catch (Exception ex)
            {
                //Do nothing
            }

            return blnbRet;
        }



        #endregion
    }
}