using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using BLL = NGL.FM.BLL;

namespace DynamicsTMS365.Controllers
{
    public class ContactController : NGLControllerBase
    {

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ContactController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " REST Services"

        [HttpGet, ActionName("GetUserLECarrierContacts")]
        public Models.Response GetUserLECarrierContacts(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                int RecordCount = 0;
                int count = 0;

                DAL.NGLCarrierContData oDAL = new DAL.NGLCarrierContData(Parameters);
                DAL.Models.Contact[] oData = new DAL.Models.Contact[] { };
                oData = oDAL.GetUserLECarrierContacts(ref RecordCount, f);
                count = oData.Count();
                if (RecordCount > count) { count = RecordCount; }
                response = new Models.Response(oData, count);
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


        [HttpGet, ActionName("GetContacts")]
        public Models.Response GetContacts(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                Models.SelectContactFilters scf = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.SelectContactFilters>(filter);
                DAL.Models.Contact[] oData = new DAL.Models.Contact[] { };
                int RecordCount = 0;
                int count = 0;

                switch (scf.ContactType)
                {
                    case 0:
                        break;
                    case 1: //Lane
                        break;
                    case 2: //Carrier
                        DAL.Models.AllFilters f = new DAL.Models.AllFilters { CarrierControlFrom = scf.Control };
                        oData = NGLCarrierContData.GetUserLECarrierContacts(ref RecordCount, f);
                        break;
                    case 3: //Comp
                        break;
                    default:
                        break;
                }

                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(oData, count);
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