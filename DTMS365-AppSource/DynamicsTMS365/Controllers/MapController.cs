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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class MapController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.MapController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.tblStop selectModelData(LTS.tblStop d)
        {
            Models.tblStop modelRecord = new Models.tblStop();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "StopUpdated" };
                string sMsg = "";
                modelRecord = (Models.tblStop)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.StopUpdated.ToArray());  modelRecord.setAddressString(); }
            }
            return modelRecord;
        }



        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        [HttpGet, ActionName("GetBingMapRoutes")]
        public Models.Response GetBingMapRoutes(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                DModel.MapRouteWayPoints routes = NGLBookTrackData.GetBingMapRoutes(id);
               //DModel.MapWaypoint w = new DModel.MapWaypoint();
                foreach (DModel.MapWaypoint w in routes.MapItWayPoints)
                {
                    if(w.Lattitude == 0 || w.Longitude == 0)
                    {
                        //ByRef res As Models.ResultObject, ByRef dblLat As Double, ByRef dblLong As Double, ByVal Zip As String
                        DAL.Models.ResultObject res = new DAL.Models.ResultObject();
                        double dblLat = 0;
                        double dblLong = 0;
                        bool blnRet = bll.GetLatLongPCMiler(ref res, ref dblLat, ref dblLong, w.Zip);
                        w.Lattitude = dblLat;
                        w.Longitude = dblLong;
                    }
                }
               // double dIncre = 0;
                foreach (DModel.MapWaypoint w in routes.TrackItWayPoints)
                {
                    if (w.Lattitude == 0 || w.Longitude == 0)
                    {
                        //ByRef res As Models.ResultObject, ByRef dblLat As Double, ByRef dblLong As Double, ByVal Zip As String
                        DAL.Models.ResultObject res = new DAL.Models.ResultObject();
                        double dblLat = 0;
                        double dblLong = 0;
                        bool blnRet = bll.GetLatLongPCMiler(ref res, ref dblLat, ref dblLong, w.Zip);
                        w.Lattitude = dblLat;
                        w.Longitude = dblLong;
                        //w.Lattitude = 40.1 + dIncre;
                        //// Lattitude   40.071111111111115  double
                        //w.Longitude = -75.00 + dIncre;
                        ////Longitude - 74.865 double
                        //dIncre = dIncre + 1;
                    }
                }

                DModel.MapRouteWayPoints[] oData = new DModel.MapRouteWayPoints[1] { routes };
                response = new Models.Response(oData, 1);
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

        [HttpGet, ActionName("GetStops")]
        public Models.Response GetStops()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                string ofilter = request["filter[filters][0][value]"];
                //string oOp = request["filter[filters][0][operator]"];
                if (!authenticateFilter(ref response, ofilter)) { return response; } //Verfiy that the filters object is not null
                DAL.NGLtblStopData dalZipData = new DAL.NGLtblStopData(Parameters);
                Models.tblStop[] records = new Models.tblStop[] { };
                int count = 0;
                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 500 : int.Parse(request["take"]);
                LTS.tblStop[] oData = dalZipData.GetStopsFilteredWildCard(ofilter);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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