using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class ReportsController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public ReportsController()
                : base(Utilities.PageEnum.Reports)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ReportsController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        ////private Models.vCarrierContract selectModelData(LTS.vCarrierContract d)
        ////{
        ////    Models.vCarrierContract modelRecord = new Models.vCarrierContract();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "CarrTarUpdated" };
        ////        string sMsg = "";
        ////        modelRecord = (Models.vCarrierContract)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        ////        if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarUpdated.ToArray()); }
        ////    }

        ////    return modelRecord;
        ////}


        ////private LTS.vCarrierContract selectLTSData(Models.vCarrierContract d)
        ////{
        ////    LTS.vCarrierContract ltsRecord = new LTS.vCarrierContract();
        ////    if (d != null)
        ////    {
        ////        //List<string> skipObjs = new List<string> { "CarrTarUpdated", "CarrierTariffBreakPoints", "CarrierTariffMatrixes", "CarrierTariffDiscounts", "CarrierTariffFees", "CarrierTariffInterlines", "CarrierTariffMinCharges", "CarrierTariffNonServices", "CarrierTariffEquipments", "CarrierTariffMatrixBPs", "CarrierTariffClassXrefs", "CarrierTariffNoDriveDays", "CarrierTariffMinWeights", "CompRefCarrier", "Carrier" };
        ////        List<string> skipObjs = new List<string> { "CarrTarUpdated" };
        ////        string sMsg = "";
        ////        ltsRecord = (LTS.vCarrierContract)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
        ////        if (ltsRecord != null)
        ////        {
        ////            byte[] bupdated = d.getUpdated();
        ////            ltsRecord.CarrTarUpdated = bupdated == null ? new byte[0] : bupdated;

        ////        }
        ////    }

        ////    return ltsRecord;
        ////}

        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"


        [HttpGet, ActionName("GetReportsTreeFlat")]
        public Models.Response GetReportsTreeFlat()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {               
                DAL.NGLtblReportListData oDAL = new DAL.NGLtblReportListData(Parameters);
                DTO.ReportsTree[] records = new DTO.ReportsTree[] { };
                int count = 0;
                DTO.ReportsTree oData = oDAL.GetReportsTreeFlat(Parameters.UserName);        
                if (oData != null)
                {
                    records = new DTO.ReportsTree[1] { oData };
                    count = 1;
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





        #endregion


    }
}