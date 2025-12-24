using System.Linq;
using System.Web;
using System.Web.Http;
using LTS = Ngl.FreightMaster.Data.LTS;
using System;
using System.Collections.Generic;
using DAL = Ngl.FreightMaster.Data;

namespace DynamicsTMS365.Controllers
{
    public class CompDashboardController : NGLControllerBase
    {
        #region " Constructors "
        public CompDashboardController()
                : base(Utilities.PageEnum.CompanyMaint)
        {
        }

        #endregion
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CompDashboardController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion
        #region " Data Translation"
        private static Models.CompDashboard selectModelData(IGrouping<string, LTS.vCompDashboard> data, int daysOld)
        {
            var result = new Models.CompDashboard
            {
                CompName = data.Key,
                TotalWgt = data.Where(o => o.DaysOld <= daysOld).Sum(o => o.BookTotalWgt) ?? 0,
                TotalLate = data.Where(o => o.DaysOld <= daysOld).Sum(o => o.LateApptDate),
                TotalOnTime = data.Where(o => o.DaysOld <= daysOld).Sum(o => o.OnTimeApptDate)
            };

            if (result.TotalLate != 0)
            {
                result.OTLPerc = Math.Round((double)result.TotalOnTime / (double)(result.TotalLate + result.TotalOnTime) * 100, 2);
            }
            else
            {
                result.OTLPerc = 100;
            }

            result.TotalOrders = result.TotalOnTime + result.TotalLate;

            return result;
        }

        private static Models.CarrierDashboard selectCarrierModelDataold(IGrouping<int, LTS.vCompDashboard> data)
        {
            var result = new Models.CarrierDashboard
            {
                iMonth = data.Key,
                Month = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(data.Key),
                Loads = data.Count()
            };            

            return result;
        }

        private static Models.CarrierDashboard selectCarrierModelData(IGrouping<int, LTS.vCompDashboard> data)
        {
            var result = new Models.CarrierDashboard
            {
                iMonth = data.Key,
                Month = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(data.Key),
                Loads = data.Count()
            };

            return result;
        }
        private static Models.PlanningDashboard selectPlanningModelData(IGrouping<DateTime, LTS.vPlanningDashboard> data)
        {
            var result = new Models.PlanningDashboard
            {
                LoadDate = data.Key,
                Loads = data.Count()
            };



            return result;
        }

        #endregion
        #region " REST Services"
        [HttpGet, ActionName("GetCompDashboard")]
        public Models.Response GetCompDashboard()
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                var modelCompDashboards = new List<Models.CompDashboard[]>();
                LTS.vCompDashboard[] vcompDashboards = new LTS.vCompDashboard[] { };
                vcompDashboards = NGLCompData.GetCompDashboard(false);
                if (vcompDashboards != null && vcompDashboards.Count() > 0)
                {
                    //var grouped = vcompDashboards.GroupBy(x => x.CompName);
                    // Modified by RHR for v-8.5.2.002 on 06/16/2022  added group by destination comp for on time performance 
                    // by destination
                    var grouped = vcompDashboards.GroupBy(x => x.Delivery);
                    var yearData = grouped
                        .Select(x => selectModelData(x, DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365))
                        .OrderByDescending(x => x.TotalWgt)
                        .Where(x => x.TotalWgt > 0)
                        .Take(10).ToArray();
                    modelCompDashboards.Add(yearData);

                    var monthData = grouped
                        .Select(x => selectModelData(x, 30))
                        .OrderByDescending(x => x.TotalWgt)
                        .Where(x => x.TotalWgt > 0)
                        .Take(10).ToArray();
                    modelCompDashboards.Add(monthData);

                    var weekData = grouped
                        .Select(x => selectModelData(x, 7))
                        .OrderByDescending(x => x.TotalWgt)
                        .Where(x => x.TotalWgt > 0)
                        .Take(10).ToArray();
                    modelCompDashboards.Add(weekData);
                }
                response = new Models.Response(modelCompDashboards.ToArray(), modelCompDashboards.ToArray().Length);
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

        [HttpGet, ActionName("GetCarierDashboardOld")]
        public Models.Response GetCarierDashboardOld()
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                var modelCarrierDashboards = new List<Models.CarrierDashboard[]>();
                LTS.vCompDashboard[] vcompDashboards = new LTS.vCompDashboard[] { };
                vcompDashboards = NGLCompData.GetCompDashboard(false);
                if (vcompDashboards != null && vcompDashboards.Count() > 0)
                {
                    //var grouped = vcompDashboards.GroupBy(x => x.CompName);
                    // Modified by RHR for v-8.5.2.002 on 06/16/2022  added group by destination comp for on time performance 
                    // by destination

                    // old order by month logic
                    //var grouped = vcompDashboards.Where(x => x.BookDateLoad.HasValue).GroupBy(x => x.BookDateLoad.Value.Month);
                    //var yearData = grouped
                    //    .Select(x => selectCarrierModelData(x)).OrderBy(x => x.iMonth).ToArray();
                    //modelCarrierDashboards.Add(yearData);

                    //new order by min date logic
                    //var grouped = vcompDashboards.Where(x => x.BookDateLoad.HasValue).GroupBy(x =>  new { x.BookDateLoad.Value.Year, x.BookDateLoad.Value.Month });

                    //var yearData = grouped
                    //    .Select(x => selectCarrierModelData(x)).OrderBy(x => x.iMonth).ToArray();
                    //modelCarrierDashboards.Add(yearData);

                    //var monthData = grouped
                    //    .Select(x => selectModelData(x, 30))
                    //    .OrderByDescending(x => x.TotalWgt)
                    //    .Where(x => x.TotalWgt > 0)
                    //    .Take(10).ToArray();
                    //modelCarrierDashboards.Add(monthData);

                    //var weekData = grouped
                    //    .Select(x => selectModelData(x, 7))
                    //    .OrderByDescending(x => x.TotalWgt)
                    //    .Where(x => x.TotalWgt > 0)
                    //    .Take(10).ToArray();
                    //modelCarrierDashboards.Add(weekData);
                }
                response = new Models.Response(modelCarrierDashboards.ToArray(), modelCarrierDashboards.ToArray().Length);
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

        [HttpGet, ActionName("GetCarierDashboard")]
        public Models.Response GetCarierDashboard()
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                var modelCompDashboards = new List<DAL.Models.LoadsDeliveredSummary>();

                modelCompDashboards = NGLCompData.GetLoadsDeliveredDashboard(false);

                response = new Models.Response(modelCompDashboards.ToArray(), modelCompDashboards.Count());

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

        [HttpGet, ActionName("GetPlanningDashboard")]
        public Models.Response GetPlanningDashboard()
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                var modelPlanningDashboards = new List<Models.PlanningDashboard[]>();
                LTS.vPlanningDashboard[] vPlanningDashboards = new LTS.vPlanningDashboard[] { };
                vPlanningDashboards = NGLCompData.GetPlanningDashboard(false);
                if (vPlanningDashboards != null && vPlanningDashboards.Count() > 0)
                {
                    //var grouped = vcompDashboards.GroupBy(x => x.CompName);
                    // Modified by RHR for v-8.5.2.002 on 06/16/2022  added group by destination comp for on time performance 
                    // by destination
                    var grouped = vPlanningDashboards.Where(x => x.BookDateLoad.HasValue).GroupBy(x => x.BookDateLoad.Value);
                    var vData = grouped
                        .Select(x => selectPlanningModelData(x)).OrderBy(x => x.LoadDate).ToArray();
                    modelPlanningDashboards.Add(vData);

                    //var monthData = grouped
                    //    .Select(x => selectModelData(x, 30))
                    //    .OrderByDescending(x => x.TotalWgt)
                    //    .Where(x => x.TotalWgt > 0)
                    //    .Take(10).ToArray();
                    //modelCarrierDashboards.Add(monthData);

                    //var weekData = grouped
                    //    .Select(x => selectModelData(x, 7))
                    //    .OrderByDescending(x => x.TotalWgt)
                    //    .Where(x => x.TotalWgt > 0)
                    //    .Take(10).ToArray();
                    //modelCarrierDashboards.Add(weekData);
                }
                response = new Models.Response(modelPlanningDashboards.ToArray(), modelPlanningDashboards.ToArray().Length);
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