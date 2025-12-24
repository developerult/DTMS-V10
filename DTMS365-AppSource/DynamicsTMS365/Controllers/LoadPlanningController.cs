using Ngl.FreightMaster.Data.Models;
using NGL.FM.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BLL = NGL.FM.BLL;
using DModel = Ngl.FreightMaster.Data.Models;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;
using static NGL.FM.BLL.NGLTMS365BLL;

namespace DynamicsTMS365.Controllers
{
    //Add order to Truck by Passing this class as Parameter
    public class LoadItem
    {
        public string SolDetailBookControl { get; set; }
        public string SolDetailCompControl { get; set; }
        public string solTruckKey { get; set; }
        public string drpIndex { get; set; }
        public string origCompControl { get; set; }
        public string origTruckKey { get; set; }
        public int TruckPageSize { get; set; }
        public bool IsCollapse { get; set; }

        public int TruckSize { get; set; }

    }
    public class LoadPlanningController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LoadPlanningController()
                : base(Utilities.PageEnum.LoadPlanning)
	     {
        }

        #endregion
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadPlanningController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        /// <value>The source class.</value>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }
        /// <summary>
        /// The request
        /// </summary>
        HttpRequest request = HttpContext.Current.Request;
        #endregion

        #region " Data Translation"

        /// <summary>
        public static LTS.tblSolutionDetail selectLTSData(Models.tblSolutionDetail d)
        {
            LTS.tblSolutionDetail ltsRecord = new LTS.tblSolutionDetail();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "SolutionDetailUpdated", "rowguid", "tblSolutionTruck" };
                string sMsg = "";
                ltsRecord = (LTS.tblSolutionDetail)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.SolutionDetailUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 2-23-01-30 added check for null SolutionDetailUpdated
        /// </remarks>
        private Models.tblSolutionDetail selectModelData(DTO.tblSolutionDetail d)
        {
            Models.tblSolutionDetail modelRecord = new Models.tblSolutionDetail();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "SolutionDetailUpdated", "rowguid", "tblSolutionTruck" };
                string sMsg = "";
                modelRecord = (Models.tblSolutionDetail)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);              
                if (modelRecord != null && d.SolutionDetailUpdated != null) { modelRecord.setUpdated(d.SolutionDetailUpdated.ToArray()); }
            }

            return modelRecord;
        }

        private Models.tblSolutionTruck selectModelData(DTO.tblSolutionTruck d)
        {
            Models.tblSolutionTruck modelRecord = new Models.tblSolutionTruck();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "SolutionTruckUpdated", "rowguid","tblSolution","tblSolutionDetail"};
                string sMsg = "";
                modelRecord = (Models.tblSolutionTruck)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null && d.SolutionTruckUpdated != null) { modelRecord.setUpdated(d.SolutionTruckUpdated.ToArray()); }
            }

            return modelRecord;
        }
        private DTO.tblSolutionDetail selectDTOData(Models.tblSolutionDetail d)
        {
            DTO.tblSolutionDetail ltsRecord = new DTO.tblSolutionDetail();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "SolutionDetailUpdated", "rowguid","tblSolutionTruck" };
                string sMsg = "";
                ltsRecord = (DTO.tblSolutionDetail)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.SolutionDetailUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }



        #endregion

        #region "Rest Services"
        /// <summary>
        /// Get new booking data from DAL using LoadPlanningTruckDataFilters
        /// DAL reads from database via vNewBookingsForSolutions
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="PageStatus"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 01/19/2023 added new logic for Missing Load Date filter
        /// </remarks>
        [HttpGet, ActionName("GetRecordsByNewOrdersFilter")]
        public Models.Response GetRecordsByNewOrdersFilter(string filter, string PageStatus)
        {
            var response = new Models.Response();
           if (!authenticateController(ref response)) { return response; }          
           try
            {
                DAL.Models.AllFilters f1 = new DAL.Models.AllFilters();
                int page = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(PageStatus);
                if (page == 0)
                {
                    var ups = readPageSettings("NewLoadsFltr", Parameters, Utilities.PageEnum.LoadPlanning);
                    if (ups?.Count() > 0)
                    {
                        if (ups[0] != null && !string.IsNullOrWhiteSpace(ups[0].UserPSMetaData)) { f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups[0].UserPSMetaData); }

                    }
                    else
                    {
                        savePageFilters(filter, "NewLoadsFltr");
                        f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                    }
                }
                else
                {
                    savePageFilters(filter, "NewLoadsFltr");
                    f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                }
                // Modified by RHR for v-8.5.3.007 on 01/19/2023 
                List<string> sUniqueFilters = new List<string> { "BookConsPrefix", "BookSHID", "BookDateRequired" };
                if (isLoadDateRequired(ref f1, sUniqueFilters))
                {
                    //if unique filters are not provided we must always have a load date so add it if it is missing
                    addMissingDateFilter(ref f1, "BookDateLoad", DateTime.Now, 7); //use todays date with 7 days before and 7 days after if no BookDateLoad is provided
                }
                int count = 0;
                int RecordCount = 0;
                DynamicsTMS365.TMSApp.clsUserTMSLookup UserLookups = Utilities.UserLists.getUserTMSLookup(this.UserControl);
                
                
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };

                //DTO.LoadPlanningTruckDataFilter f = new DTO.LoadPlanningTruckDataFilter();
                //f.StartDateFilter = new DateTime(2022, 10, 10);
                //f.StopDateFilter = new DateTime(2023, 05, 05);
                //f.UseLoadDateFilter = true;
                //f.OrigCityFilter= f1.FilterValues.Where(x => x.filterName == "OrigCity").Select(y => y.filterValueFrom).FirstOrDefault();
                //f.DestCityFilter = f1.FilterValues.Where(x => x.filterName == "DestCity").Select(y => y.filterValueFrom).FirstOrDefault();
                //f.OrigSt1Filter = f1.FilterValues.Where(x => x.filterName == "OrigState").Select(y => y.filterValueFrom).FirstOrDefault();
                //f.DestSt1Filter = f1.FilterValues.Where(x => x.filterName == "DestState").Select(y => y.filterValueFrom).FirstOrDefault();
                //f.BookTranCodeFilter= f1.FilterValues.Where(x => x.filterName == "TranCode").Select(y => y.filterValueFrom).FirstOrDefault();
                //f.BookTransTypeFilter = f1.FilterValues.Where(x => x.filterName == "TransType").Select(y => y.filterValueFrom).FirstOrDefault();
                //f.LaneNumberFilter= f1.FilterValues.Where(x => x.filterName == "LaneNumber").Select(y => y.filterValueFrom).FirstOrDefault();                
                //string sCompControlFilter = f1.FilterValues.Where(x => x.filterName == "CompanyName").Select(y => y.filterValueFrom).FirstOrDefault();
                //if (!string.IsNullOrWhiteSpace(sCompControlFilter))
                //{
                //    oLookup = UserLookups.getUserDynamicvLookupList(DAL.NGLLookupDataProvider.UserDynamicLists.Comp, DAL.NGLLookupDataProvider.ListSortType.Name, null, null, Parameters);
                //    if (oLookup != null && oLookup.Count() > 0)
                //    {
                //        f.CompControlFilter = oLookup.Where(x => x.Name.ToUpper().Contains(sCompControlFilter.ToUpper())).Select(y => y.Control).FirstOrDefault();
                //    }
                //}
                //f.PageSize = Convert.ToInt32(f1.FilterValues.Where(x => x.filterName == "PageSize").Select(y => y.filterValueFrom).FirstOrDefault());
                //f.PageSize = (f.PageSize == 0) ? f1.take : f.PageSize;
                //f.Page = f1.page == 0 ? 1 : f1.page;
                //
                
                Models.tblSolutionDetail[] newOrders = new Models.tblSolutionDetail[] { };
                DAL.NGLNewBookingsForSolutionData orders = new DAL.NGLNewBookingsForSolutionData(Parameters);
                DTO.tblSolutionDetail[] ltsRet = orders.GetNewBookingsFiltered365(f1,ref RecordCount);
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    newOrders = (from e in ltsRet
                                       select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(newOrders, count);
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
        /// Get New Pro Numbers from Order Preiew Screen
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="PageStatus"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 01/19/2023 added new logic for missing Load Date Filter
        /// </remarks>
        [HttpGet, ActionName("GetRecordsByImportQueueFilter")]
        public Models.Response GetRecordsByImportQueueFilter(string filter, string PageStatus)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
           
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f1 = new DAL.Models.AllFilters();
                int page = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(PageStatus);
                if (page == 0)
                {
                    var ups = readPageSettings("NewLoadsFltr", Parameters, Utilities.PageEnum.LoadPlanning);
                    if (ups?.Count() > 0)
                    {
                        if (ups[0] != null && !string.IsNullOrWhiteSpace(ups[0].UserPSMetaData)) { f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups[0].UserPSMetaData); }

                    }
                    else
                    {
                        savePageFilters(filter, "NewLoadsFltr");
                        f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                    }
                }
                else
                {
                    savePageFilters(filter, "NewLoadsFltr");
                    f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                }
                // Modified by RHR for v-8.5.3.007 on 01/19/2023 
                List<string> sUniqueFilters = new List<string> { "BookConsPrefix", "BookSHID", "BookDateRequired" };
                if (isLoadDateRequired(ref f1, sUniqueFilters))
                {
                    //if unique filters are not provided we must always have a load date so add it if it is missing
                    addMissingDateFilter(ref f1, "BookDateLoad", DateTime.Now, 7); //use todays date with 7 days before and 7 days after if no BookDateLoad is provided
                }
                //DTO.LoadPlanningTruckDataFilter f = new DTO.LoadPlanningTruckDataFilter();
                //f.StartDateFilter = new DateTime(2022, 10, 10);
                //f.StopDateFilter = new DateTime(2023, 05, 05);
                //f.PageSize = Convert.ToInt32(f1.FilterValues.Where(x => x.filterName == "PageSize").Select(y => y.filterValueFrom).FirstOrDefault());
                //f.PageSize = (f.PageSize == 0) ? f1.take : f.PageSize;
                //f.Page = f1.page == 0 ? 1 : f1.page;
                Models.tblSolutionDetail[] newOrders = new Models.tblSolutionDetail[] { };
                DAL.NGLNewPOsForSolutionData orders = new DAL.NGLNewPOsForSolutionData(Parameters);
                //debug code removed
                DTO.tblSolutionDetail[] ltsRet = orders.GetNewPOsFiltered365(f1, ref RecordCount);
                //DTO.tblSolutionDetail[] ltsRet = null;
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    newOrders = (from e in ltsRet
                                 select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(newOrders, count);
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

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            var response = new Models.Response();
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="PageStatus"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 01/19/2023 added new logic for missing load date filter
        /// </remarks>
        [HttpGet, ActionName("GetRecordsBySummaryFilter")]
        public Models.Response GetRecordsBySummeryFilter(string filter,string PageStatus)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            //if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {

                DAL.Models.AllFilters f1 = new DAL.Models.AllFilters();
                int page = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(PageStatus);
                if (page == 0)
                {

                    var ups = readPageSettings("SummaryLoadsFltr", Parameters, Utilities.PageEnum.LoadPlanning);
                    if (ups?.Count() > 0)
                    {
                        if (ups[0] != null && !string.IsNullOrWhiteSpace(ups[0].UserPSMetaData)) { f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups[0].UserPSMetaData); }

                    }
                    else
                    {
                        savePageFilters(filter, "SummaryLoadsFltr");
                        f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                    }
                }
                else
                {
                    savePageFilters(filter, "SummaryLoadsFltr");
                    f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                }
                // Modified by Kanna for v-8.5.3.007 on 02/16/2023
                if (f1.FilterValues.Any(x => x.filterName == "BookDateLoad")) {
                    FilterDetails oFilter = f1.FilterValues.Where(x => x.filterName == "BookDateLoad").FirstOrDefault();
                    // we need to validate that string is a date
                    DateTime oDt = DateTime.Now;
                    if (DateTime.TryParse(oFilter.filterValueFrom, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out oDt))
                    {
                        oFilter.filterFrom = oDt;
                        oFilter.filterValueFrom = "";
                    }
                    if (DateTime.TryParse(oFilter.filterValueTo, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out oDt))
                    {
                        oFilter.filterTo = oDt;
                        oFilter.filterValueTo = "";
                    }
                    //if (DateTime.TryParse(oFilter.filterValueFrom,  out oDt))
                    //{
                    //    oFilter.filterFrom = oDt;
                    //}
                    //if (DateTime.TryParse(oFilter.filterValueTo, out oDt))
                    //{
                    //    oFilter.filterTo = oDt;
                    //}

                }
                    // Modified by RHR for v-8.5.3.007 on 01/19/2023 
                    List<string> sUniqueFilters = new List<string> { "BookConsPrefix", "BookSHID", "BookDateRequired" };
                if (isLoadDateRequired(ref f1, sUniqueFilters))
                {
                    //if unique filters are not provided we must always have a load date so add it if it is missing
                    addMissingDateFilter(ref f1, "BookDateLoad", DateTime.Now, 7); //use todays date with 7 days before and 7 days after if no BookDateLoad is provided
                }
                DynamicsTMS365.TMSApp.clsUserTMSLookup UserLookups = Utilities.UserLists.getUserTMSLookup(this.UserControl);
                 DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };

                int count = 0;
                int RecordCount = 0;               
               // DTO.LoadPlanningTruckDataFilter f = new DTO.LoadPlanningTruckDataFilter();
               // f.StartDateFilter = new DateTime(2022, 10, 10);
                //f.StopDateFilter = new DateTime(2023, 05, 05);
                // kanna commented below 2 lines
                // f.StartDateFilter = (f.StartDateFilter == null || f.StartDateFilter == DateTime.MinValue) ? new DateTime(DateTime.Now.AddDays(-365).Ticks) : f.StartDateFilter;
                // f.StopDateFilter = (f.StopDateFilter == null || f.StopDateFilter == DateTime.MinValue) ? new DateTime(DateTime.Now.Ticks) : f.StopDateFilter;
                //f.UseLoadDateFilter = true;                
                //f.BookConsPrefixFilter = f1.FilterValues.Where(x => x.filterName == "BookConsPrefixFilter").Select(y => y.filterValueFrom).FirstOrDefault();
                //string sCarrierNameFilter = f1.FilterValues.Where(x => x.filterName == "CarrierName").Select(y => y.filterValueFrom).FirstOrDefault();
                //if (!string.IsNullOrWhiteSpace(sCarrierNameFilter))
                //{
                //    oLookup = UserLookups.getUserDynamicvLookupList(DAL.NGLLookupDataProvider.UserDynamicLists.CarrierActive, DAL.NGLLookupDataProvider.ListSortType.Name, null, null, Parameters);
                //    if (oLookup != null && oLookup.Count() > 0)
                //    {
                //        f.CarrierControlFilter = oLookup.Where(x => x.Name.ToUpper().Contains(sCarrierNameFilter.ToUpper())).Select(y => y.Control).FirstOrDefault();
                //    }
                //}                
                //f.PageSize = Convert.ToInt32(f1.FilterValues.Where(x => x.filterName == "PageSize").Select(y => y.filterValueFrom).FirstOrDefault());
                //f.PageSize = (f.PageSize == 0) ?  f1.take : f.PageSize;
                //f.Page = f1.page==0?1:f1.page;
                //f.LaneNumberFilter= f1.FilterValues.Where(x => x.filterName == "LaneNumber").Select(y => y.filterValueFrom).FirstOrDefault();                
                //f.BookTransTypeFilter = f1.FilterValues.Where(x => x.filterName == "TransType").Select(y => y.filterValueFrom).FirstOrDefault();
                Models.tblSolutionTruck[] newOrders = new Models.tblSolutionTruck[] { };
                DAL.NGLLoadPlanningTruckData orders = new DAL.NGLLoadPlanningTruckData(Parameters);
                // debug code
                DTO.tblSolutionTruck[] ltsRet = orders.GetLoadPlanningTrucks365Filtered(f1);
                //DTO.tblSolutionTruck[] ltsRet = null;
                //DTO.tblSolutionTruck[] ltsRet = orders.GetLoadPlanningTrucksFiltered(f, ref RecordCount);
                if (ltsRet?.Count() > 0) { count = ltsRet.Length; }
                if (RecordCount > count) { count = RecordCount; }
                response = new Models.Response(ltsRet, count);
                
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


        [HttpPost, ActionName("LoadItemDropToTruck")]
        public Models.Response LoadItemDropToTruck(LoadItem load)
        {
            var response = new Models.Response();
            NGLSolutionDetailBLL soldetbll = new NGLSolutionDetailBLL(Parameters);
            DAL.NGLLoadPlanningTruckData truckdata = new DAL.NGLLoadPlanningTruckData(Parameters);
            DAL.NGLtblSolutionDetailData soldetData = new DAL.NGLtblSolutionDetailData(Parameters);
            DAL.NGLNewBookingsForSolutionData loadData = new DAL.NGLNewBookingsForSolutionData(Parameters);
            DAL.NGLNewPOsForSolutionData IQloadData = new DAL.NGLNewPOsForSolutionData(Parameters);
            int dropIndex = Convert.ToInt32(load.drpIndex);
            //DTO.tblSolutionDetail solDet = soldetData.GettblSolutionDetailFiltered(Convert.ToInt32(load.SolDetailBookControl));
            if (load.SolDetailBookControl.IndexOf('-') != -1)
            {
                DTO.tblSolutionDetail loadDet = IQloadData.GetNewPOFiltered(long.Parse(load.SolDetailBookControl));
                DTO.tblSolutionTruck solTruck = truckdata.GetLoadPlanningTruckFiltered(Convert.ToInt32(load.SolDetailCompControl), load.solTruckKey, false);
                soldetbll.LoadPlanningItemDropped(solTruck, loadDet, dropIndex);
                soldetbll.LoadPlanningRecalculateTruck(solTruck, true, false, true);
            }
            else
            {
                DTO.tblSolutionDetail loadDet = loadData.GetNewBookingFiltered(Convert.ToInt32(load.SolDetailBookControl));
                DTO.tblSolutionTruck solTruck = truckdata.GetLoadPlanningTruckFiltered(Convert.ToInt32(load.SolDetailCompControl), load.solTruckKey, false);
                soldetbll.LoadPlanningItemDropped(solTruck, loadDet, dropIndex);
                soldetbll.LoadPlanningRecalculateTruck(solTruck, true, false, true);
            }
            return response;
        }

        [HttpPost, ActionName("ReassignLoadToTruck")]
        public Models.Response ReassignLoadToTruck(LoadItem load)
        {
            var response = new Models.Response();
            NGLSolutionDetailBLL soldetbll = new NGLSolutionDetailBLL(Parameters);
            DAL.NGLLoadPlanningTruckData truckdata = new DAL.NGLLoadPlanningTruckData(Parameters);
            DAL.NGLtblSolutionDetailData soldetData = new DAL.NGLtblSolutionDetailData(Parameters);
            DAL.NGLNewBookingsForSolutionData loadData = new DAL.NGLNewBookingsForSolutionData(Parameters);
            int dropIndex = Convert.ToInt32(load.drpIndex);
            //DTO.tblSolutionDetail solDet = soldetData.GettblSolutionDetailFiltered(Convert.ToInt32(load.SolDetailBookControl));            
            DTO.tblSolutionDetail loadDet = loadData.GetLoadBookingFiltered(Convert.ToInt32(load.SolDetailBookControl));
            DTO.tblSolutionTruck solTruck = truckdata.GetLoadPlanningTruckFiltered(Convert.ToInt32(load.SolDetailCompControl), load.solTruckKey, false);
            
            //Reassigns the load to the new truck
            soldetbll.LoadPlanningItemDropped(solTruck, loadDet, dropIndex);

            //Recalculates the stops in origing truck after removing a load
            //load.origTruckKey;
            DTO.tblSolutionTruck OrginSolTruck = truckdata.GetLoadPlanningTruckFiltered(Convert.ToInt32(load.origCompControl), load.origTruckKey, false);
            soldetbll.LoadPlanningReSequenceStopNumbers(OrginSolTruck);
            soldetbll.LoadPlanningReSequenceStopNumbers(solTruck);
            soldetbll.LoadPlanningRecalculateTruck(solTruck, true, false, true);

            return response;
        }

        [HttpPost, ActionName("UpdateLoadIndex")]
        public Models.Response UpdateLoadIndex(LoadItem load)
        {
            var response = new Models.Response();
            NGLSolutionDetailBLL soldetbll = new NGLSolutionDetailBLL(Parameters);
            DAL.NGLLoadPlanningTruckData truckdata = new DAL.NGLLoadPlanningTruckData(Parameters);
            DAL.NGLtblSolutionDetailData soldetData = new DAL.NGLtblSolutionDetailData(Parameters);
            DAL.NGLNewBookingsForSolutionData loadData = new DAL.NGLNewBookingsForSolutionData(Parameters);
            int dropIndex = Convert.ToInt32(load.drpIndex);
            //string CompControl = "0";            
            //DTO.tblSolutionDetail solDet = soldetData.GettblSolutionDetailFiltered(Convert.ToInt32(load.SolDetailBookControl));            
            DTO.tblSolutionDetail loadDet = loadData.GetLoadBookingFiltered(Convert.ToInt32(load.SolDetailBookControl));
            DTO.tblSolutionTruck solTruck = truckdata.GetLoadPlanningTruckFiltered(Convert.ToInt32(load.SolDetailCompControl), load.solTruckKey, false);
            soldetbll.LoadPlanningItemDropped(solTruck, loadDet, dropIndex);
            return response;
        }


        [HttpGet, ActionName("GetFilterSettings")]
        public Models.Response GetFilterSettings()
        {
            var response = new Models.Response();
          //  if (!authenticateFilter(ref response,null)) { return response; } //Verfiy that the filters object is not null
            try
            {

                DAL.Models.AllFilters f1 = new DAL.Models.AllFilters();
               // DAL.Models.AllFilters f2 = new DAL.Models.AllFilters();
                int count = 0;
                int RecordCount = 0;
                var ups = readPageSettings("SummaryLoadsFltr", Parameters, Utilities.PageEnum.LoadPlanning);
                var ups1 = readPageSettings("NewLoadsFltr", Parameters, Utilities.PageEnum.LoadPlanning);
                if (ups?.Count() > 0)
                    {
                        if (ups[0] != null && !string.IsNullOrWhiteSpace(ups[0].UserPSMetaData)) { f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups[0].UserPSMetaData); }

                    }
                if (ups1?.Count() > 0)
                {
                    if (ups1[0] != null && !string.IsNullOrWhiteSpace(ups1[0].UserPSMetaData)) { f1 = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups1[0].UserPSMetaData); }

                }
                FilterDetails[] lstRet = f1.FilterValues;           
                if (lstRet?.Count() > 0) { count = lstRet.Count(); }
                if (RecordCount > count) { count = RecordCount; }
                response = new Models.Response(lstRet, count);
                // response = new Models.Response(ltsRet, count);
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

        [HttpGet, ActionName("GetLoadsInTruck")]
        public Models.Response GetLoadsInTruck(string CompControl, string TruckKey)
        {
            var response = new Models.Response();
            try
            {
                int count = 0;
                int RecordCount = 0;
                int compControl = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(CompControl);
                string truckKey = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(TruckKey);
                DAL.NGLLoadPlanningTruckData order = new DAL.NGLLoadPlanningTruckData(Parameters);
                System.Threading.Thread.Sleep(1500);
                DTO.tblSolutionTruck truck = order.GetLoadPlanningTruckFiltered(compControl, truckKey, false);

                if(truck != null)
                {
                    if(truck.SolutionDetails != null)
                    {
                        DTO.tblSolutionDetail[] loads = truck.SolutionDetails.ToArray();
                        if (loads.Count() > 0) { count = loads.Length; }
                        if (RecordCount > count) { count = RecordCount; }
                        response = new Models.Response(loads, count);
                    }
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
        /// Method to Save the Truck Page Details
        /// </summary>
        /// <param name="load">list of solutiontruckkey</param>
        /// <returns></returns>
        [HttpPost, ActionName("SavePageChanges")]
        public Models.Response SavePageChanges(LoadItem load)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool[] oRecords = new bool[1] { false }; string retMsg = string.Empty;
                string pagesettings = load.IsCollapse + "," + load.TruckPageSize + "," + load.TruckSize + "," + load.SolDetailCompControl.Trim(',');//Added for PageSettings
                oRecords[0] = savePageSetting(pagesettings, ref retMsg, "LoadPlanningTrucks");
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
        /// Method to Read the truck pageDetails
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetPageChanges")]
        public Models.Response GetPageChanges()
        {
            var response = new Models.Response();
            try
            {                
                response = GetPageSettings("LoadPlanningTrucks");                
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

        [HttpPost, ActionName("CalculateTruck")]
        public Models.Response CalculateTruck(LoadItem load)
        {
            var response = new Models.Response();
            NGLSolutionDetailBLL soldetbll = new NGLSolutionDetailBLL(Parameters);
            DAL.NGLLoadPlanningTruckData truckdata = new DAL.NGLLoadPlanningTruckData(Parameters);

            DTO.tblSolutionTruck solTruck = truckdata.GetLoadPlanningTruckFiltered(Convert.ToInt32(load.SolDetailCompControl), load.solTruckKey, false);
            soldetbll.LoadPlanningRecalculateTruck(solTruck, true, true, true);
            return response;
        }

        [HttpPost, ActionName("CalculateTruckLineHaul")]
        public Models.Response CalculateTruckLineHaul(LoadItem load)
        {
            var response = new Models.Response();
            NGLSolutionDetailBLL soldetbll = new NGLSolutionDetailBLL(Parameters);
            DAL.NGLLoadPlanningTruckData truckdata = new DAL.NGLLoadPlanningTruckData(Parameters);

            DTO.tblSolutionTruck solTruck = truckdata.GetLoadPlanningTruckFiltered(Convert.ToInt32(load.SolDetailCompControl), load.solTruckKey, false);
            soldetbll.LoadPlanningRecalculateTruck(solTruck, true, false, true);
            return response;
        }

        [System.Web.Http.HttpGet, ActionName("StopResequence")]
        public Models.Response StopResequence(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);

                DModel.ResultObject oData = oBLL.StopResequence(id, false);

                DModel.ResultObject[] oRecords = new DModel.ResultObject[1];

                oRecords[0] = oData;

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

        [HttpGet, ActionName("GetMiles")]
        public Models.Response GetMiles(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);

                DModel.ResultObject oData = oBLL.StopResequence(id, true);

                DModel.ResultObject[] oRecords = new DModel.ResultObject[1];

                oRecords[0] = oData;

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
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 2023-01-30  
        ///     replaced "NO" for "New order" vs IQ for "Import Queue"
        ///     added new logic to test for null loadDet and throw an error
        ///     modified call to GetNewPOFiltered we now use GetNewPOFiltered365
        ///     this supports the ability to show New Lane records in the Import Queue list
        /// </remarks>
        [HttpPost, ActionName("AddNewTruckItem")]
        public Models.Response AddNewTruckItem(LoadItem lst)
        {
            //inewTruckBookPKType(NO/IQ) as solTruckKey ; Active Carrier As SolDetailCompControl ;selected orderID from newpo SolDetailBookControl
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLoadPlanningTruckData dalBookData = new DAL.NGLLoadPlanningTruckData(Parameters);
                DAL.NGLNewBookingsForSolutionData loadData = new DAL.NGLNewBookingsForSolutionData(Parameters);
                DAL.NGLNewPOsForSolutionData orders = new DAL.NGLNewPOsForSolutionData(Parameters);               
                DAL.NGLBookData cns = new DAL.NGLBookData(Parameters);
                NGLTMS365BLL bllbook = new NGLTMS365BLL(Parameters);
                NGLOrderImportBLL iqdal = new NGLOrderImportBLL(Parameters);
                DTO.tblSolutionDetail loadDet = new DTO.tblSolutionDetail();              
                
                
                if (lst.solTruckKey == "NO")
                {
                    //try to get the new booking 
                    loadDet = loadData.GetNewBookingFiltered(Convert.ToInt64(lst.SolDetailBookControl));
                    if (loadDet == null)
                    {
                        //try to get the booking record
                        loadDet = loadData.GetLoadBookingFiltered(Convert.ToInt64(lst.SolDetailBookControl));
                    }
                    
                }
                else
                {
                    loadDet = orders.GetNewPOFiltered365(Convert.ToInt64(lst.SolDetailBookControl));                   
                }
                if (loadDet == null)
                {
                    loadData.throwDataValidationException("The selected record cannot be found. Please refresh the page and try again", DAL.SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails, false, false);
                }
                List<BookingActions> actns=  bllbook.GetAvailableBookingActions(loadDet.SolutionDetailBookControl, 
                loadDet.SolutionDetailTranCode, null, null, null, null, Convert.ToInt32(lst.SolDetailCompControl), loadDet.SolutionDetailLockAllCosts,
                loadDet.SolutionDetailBookSHID, null,Convert.ToInt32(loadDet.SolutionDetailRouteConsFlag));
                loadDet.SolutionDetailConsPrefix=cns.GetNextCNSNumber(loadDet.SolutionDetailCompControl, loadDet.SolutionDetailCompNumber);
                if (actns[0]==BookingActions.None)
                {
                    // BookStopNo,BookCarrTruckControl need to check
                    if (lst.solTruckKey == "NO")
                    {
                        if (!string.IsNullOrEmpty(loadDet.SolutionDetailProNumber))
                            dalBookData.UpdateLoadPlanningCarrier(loadDet.SolutionDetailProNumber, loadDet.SolutionDetailConsPrefix, loadDet.SolutionDetailRouteConsFlag, 1, Convert.ToInt32(lst.SolDetailCompControl), 0, 0,null);
                        else
                            dalBookData.UpdateLoadPlanningCarrier(loadDet.SolutionDetailBookControl, loadDet.SolutionDetailConsPrefix, loadDet.SolutionDetailRouteConsFlag, 1, Convert.ToInt32(lst.SolDetailCompControl), null, null);
 
                    }
                    else
                    {
                        iqdal.UpdateNewPOWithLoadPlanningCarrier(loadDet.SolutionDetailOrderNumber, loadDet.SolutionDetailOrderSequence, loadDet.SolutionDetailCompNumber,
                            loadDet.SolutionDetailConsPrefix, loadDet.SolutionDetailRouteConsFlag, loadDet.SolutionDetailOrigStopNumber,
                            Convert.ToInt32(lst.SolDetailCompControl), Convert.ToInt32(loadDet.SolutionDetailSolutionTruckControl), loadDet.SolutionDetailHoldLoad);
                    }
                }
                bool[] oRecords = new bool[1];
                oRecords[0] = true;
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
        /// Mass Update to Load Planning data from Modify Load Planning Popup Window
        /// </summary>
        /// <param name="lpData"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.006 on 01/10/2023
        /// </remarks>
        [HttpPost, ActionName("MassUpdate")]
        public Models.Response MassUpdate([System.Web.Http.FromBody] Models.LPMassUpdate lpData)
        {
           
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLtblSolutionData oDAL = new DAL.NGLtblSolutionData(Parameters);

                Dictionary<string, string> oData = new Dictionary<string, string>();
                oData.Add("BookShipCarrierProNumberRaw", lpData.BookShipCarrierProNumberRaw);
                oData.Add("BookShipCarrierProControl", lpData.BookShipCarrierProControl);
                oData.Add("BookRouteConsFlag", lpData.BookRouteConsFlag);
                oData.Add("BookRouteTypeCode", lpData.BookRouteTypeCode);
                oData.Add("BookDateLoad", lpData.BookDateLoad);
                oData.Add("BookDateRequired", lpData.BookDateRequired);
                oData.Add("BookCarrActDate", lpData.BookCarrActDate);
                oData.Add("BookShipCarrierProNumber", lpData.BookShipCarrierProNumber);
                oData.Add("BookShipCarrierName", lpData.BookShipCarrierName);
                oData.Add("BookShipCarrierNumber", lpData.BookShipCarrierNumber);
                oData.Add("BookCarrBLNumber", lpData.BookCarrBLNumber);
                oData.Add("BookCustomerApprovalTransmitted", lpData.BookCustomerApprovalTransmitted);
                oData.Add("BookCustomerApprovalRecieved", lpData.BookCustomerApprovalRecieved);
                oData.Add("BookCarrTrailerNo", lpData.BookCarrTrailerNo);
                oData.Add("BookCarrSealNo", lpData.BookCarrSealNo);
                oData.Add("BookCarrDriverNo", lpData.BookCarrDriverNo);
                oData.Add("BookCarrDriverName", lpData.BookCarrDriverName);
                oData.Add("BookCarrTripNo", lpData.BookCarrTripNo);
                oData.Add("BookCarrRouteNo", lpData.BookCarrRouteNo);
                oData.Add("BookTrackContact", lpData.BookTrackContact);
                oData.Add("BookTrackStatus", lpData.BookTrackStatus);
                oData.Add("BookTrackComment", lpData.BookTrackComment);
                
                //Note: Kanna must finish adding data to the dictionary oData for each property of the  Models.LPMassUpdate

                string sRet = oDAL.UpdateBookFromLoadPlanning(lpData.BookProNumber,oData);
                string[] oRecords = new string[1];
                oRecords[0] = sRet;
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

        #region "Private Methods"

        

        /// <summary>
        /// Add a missing date filter to the AllFilters data, typically used when the Load Date is not provided
        /// </summary>
        /// <param name="fUserFilters"></param>
        /// <param name="sFilterName"></param>
        /// <param name="dtDefault"></param>
        /// <param name="iRange"></param>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-01-19
        /// </remarks>
        private void addMissingDateFilter( ref DAL.Models.AllFilters fUserFilters, string sFilterName, DateTime dtDefault, int iRange = 7)
        {
            DAL.Models.FilterDetails oFDetails = new DAL.Models.FilterDetails();
            DateTime dStartDate = dtDefault.AddDays((iRange * -1)); //default if missing
            DateTime dEndDate = dtDefault.AddDays(iRange); //default if missing
            if (fUserFilters.FilterValues.Any(x => x.filterName == sFilterName))
            {
                oFDetails = fUserFilters.FilterValues.Where(x => x.filterName == sFilterName).FirstOrDefault();
                if (!oFDetails.filterFrom.HasValue)
                {
                    oFDetails.filterFrom = dStartDate;
                    oFDetails.filterTo = dEndDate;
                }
            } else
            {
                oFDetails.filterName = sFilterName;
                oFDetails.filterFrom = dStartDate;
                oFDetails.filterTo = dEndDate;
                fUserFilters.addFilterDetail(oFDetails);

            }
           
        }

        /// <summary>
        /// Check for missing unique filter values and add date range if not provided
        /// </summary>
        /// <param name="fUserFilters"></param>
        /// <param name="sUniqueFiltes"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-01-19
        /// </remarks>
        private bool isLoadDateRequired(ref DAL.Models.AllFilters fUserFilters, List<string> sUniqueFiltes)
        {
            bool bRet = true;
            if (fUserFilters == null || fUserFilters.FilterValues == null || fUserFilters.FilterValues.Count() < 1) { return bRet; }            
            if (fUserFilters.FilterValues.Any(x => sUniqueFiltes.Contains( x.filterName))) { bRet = false;}
            return bRet;
        }

        #endregion
    }
}