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
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class TariffBreakPointController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public TariffBreakPointController()
                : base(Utilities.PageEnum.TariffBreakPoints)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.TariffBreakPointController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " Data Translation"

        private Models.vCarrierTariffService selectModelData(LTS.vCarrierTariffService d)
        {
            Models.vCarrierTariffService modelRecord = new Models.vCarrierTariffService();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrTarEquipUpdated" };
                string sMsg = "";
                modelRecord = (Models.vCarrierTariffService)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarEquipUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.vCarrierTariffService selectLTSData(Models.vCarrierTariffService d)
        {
            LTS.vCarrierTariffService ltsRecord = new LTS.vCarrierTariffService();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrTarEquipUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vCarrierTariffService)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrTarEquipUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }

        #endregion

        #region " REST Services"
        //    POST 	/API/objectcontroller{data
        //}  : Create a new object or Update a the current object if the control number exists
        /// 
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"
        /// 

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a CarrTarControl or = zero
            // if zero we look this up using the current user tariff primary key
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {               
                int count = 0;
               
                if (id == 0)
                {
                    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                }
               
               
                DAL.NGLCarrTarMatBPData oDAL = new DAL.NGLCarrTarMatBPData(Parameters);
                LTS.vCarrTarEquipMatBreakPoint[] records = new LTS.vCarrTarEquipMatBreakPoint[1];
                LTS.vCarrTarEquipMatBreakPoint oData = new LTS.vCarrTarEquipMatBreakPoint();
                oData = oDAL.GetCarrTarEquipMatBreakPointForContract(id);
                if (oData != null )
                {
                    count = 1;
                    records[0] = oData;
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

      

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]LTS.vCarrTarEquipMatBreakPoint data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrTarMatBPData oDAL = new DAL.NGLCarrTarMatBPData(Parameters);
                bool blnRet = oDAL.saveCarrTarEquipMatBreakPoint(data);

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


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrTarEquipData oDAL = new DAL.NGLCarrTarEquipData(Parameters);
                bool blnRet = oDAL.DeleteCarrierTariffService(id);
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


        #endregion


        #region " public methods"


        #endregion

    }
}