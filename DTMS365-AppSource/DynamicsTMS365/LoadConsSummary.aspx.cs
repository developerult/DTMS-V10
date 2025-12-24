using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using CM = DynamicsTMS365.ContentManagement;

namespace DynamicsTMS365
{
    public partial class LoadConsSummary : NGLWebUIBaseClass
    {
        private string _bingMapsJS = "";
        public string BingMapsJS
        {
            get { return _bingMapsJS; }
            set { _bingMapsJS = value; }
        }

        public int AutoDisplayBOLReportOnDispatch { get; set; }
        public int AutoDisplayDispatchReportOnDispatch { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PageControl = (int)Utilities.PageEnum.ConsolidationSum;
                refreshUserControl();
                int LEControl = this.SSOR.UserLEControl;
                AutoDisplayBOLReportOnDispatch = Convert.ToInt32(Utilities.GetParValueByLegalEntity("AutoDisplayBOLReportOnDispatch", LEControl));
                AutoDisplayDispatchReportOnDispatch = Convert.ToInt32(Utilities.GetParValueByLegalEntity("AutoDisplayDispatchReportOnDispatch", LEControl));
                CM.cmPageBuilder pg = new CM.cmPageBuilder();
                UserGroupCategory = this.SSOR.CatControl;
                pg.UserGroupCategory = UserGroupCategory;
                pg.UserControl = UserControl;
                pg.PageControl = PageControl;
                HttpRequest request = HttpContext.Current.Request;
                if (request.QueryString.GetValues("BookConsPrefix") != null && request.QueryString.GetValues("BookConsPrefix").Length != 0)
                {
                    pg.addToDictOverrideSavedFilterArray("AllRecordsFilter", "BookConsPrefix", request.QueryString.GetValues("BookConsPrefix")[0], request.QueryString.GetValues("BookConsPrefix")[0], "", "", false, "Consolidation Number");

                }
                this.UserTheme = pg.UserTheme;
                this.PageFooterHTML = pg.PageFooterHTML;
                this.AuthLoginNotificationHTML = pg.AuthLoginNotificationHTML;

                PageMenuTab = pg.CreateMenuTabStrip(PageControl, UserControl);

                PageReadyJS = pg.getMenuTree(UserControl);
                PageReadyJS += pg.getHelpWindow();
                PageReadyJS += pg.menuTreeHover();

                if (UserControl != 0)
                {
                    pg.createPageDetails(PageControl, UserControl);
                }

                PageReadyJS += pg.PageReadyJS + "\n\r" + KendoIconFix;;
                FastTabsHTML = pg.FastTabsHTML;
                FastTabsJS = pg.FastTabsJS;
                PageTemplates = pg.PageTemplates;
                PageCustomJS = pg.getTrimbleAPIKey() + pg.PageCustomJS;
                //PageCustomJS = pg.PageCustomJS;
                PageArrayDataJS = pg.PageArrayDataJS;
                PageErrorsOrWarnings += pg.PageErrorsOrWarnings;

                BingMapsJS = pg.getBingMapsJS(); //Added By LVV on 9/24/19 Bing Maps

            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
            }
        }
    }
}