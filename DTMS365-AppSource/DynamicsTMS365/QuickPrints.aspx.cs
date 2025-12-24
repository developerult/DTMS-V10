using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL = Ngl.FreightMaster.Data;
using CM = DynamicsTMS365.ContentManagement;


namespace DynamicsTMS365
{
    public partial class QuickPrints : NGLWebUIBaseClass
    {

        public int BKCntrl { get; set; }
        public int Rpt { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PageControl = (int)Utilities.PageEnum.QuickPrints;
                refreshUserControl();

                HttpRequest request = HttpContext.Current.Request;
                int intBKCntrl = 0, intRpt = 0;
                string strBKCntrl = "", strRpt = "";
                if (request.QueryString.GetValues("BkCtrl") != null && request.QueryString.GetValues("BkCtrl").Length != 0)
                {
                    strBKCntrl = request.QueryString.GetValues("BkCtrl")[0];
                    int.TryParse(strBKCntrl, out intBKCntrl);                                   
                }
                if (request.QueryString.GetValues("Rpt") != null && request.QueryString.GetValues("Rpt").Length != 0)
                {
                    strRpt = request.QueryString.GetValues("Rpt")[0];
                    int.TryParse(strRpt, out intRpt);
                }
                BKCntrl = intBKCntrl;
                Rpt = intRpt;

                CM.cmPageBuilder pg = new CM.cmPageBuilder();
                UserGroupCategory = this.SSOR.CatControl;
                pg.UserGroupCategory = UserGroupCategory;
                pg.UserControl = UserControl;
                pg.PageControl = PageControl;
                this.UserTheme = pg.UserTheme;
                this.PageFooterHTML = pg.PageFooterHTML;
                this.AuthLoginNotificationHTML = pg.AuthLoginNotificationHTML;
                PageMenuTab = pg.CreateMenuTabStrip(PageControl, UserControl);
                PageReadyJS = pg.getMenuTree(UserControl);
                PageReadyJS += pg.getHelpWindow();
                PageReadyJS += pg.menuTreeHover();
                if (UserControl != 0) { pg.createPageDetails(PageControl, UserControl); }
                PageReadyJS += pg.PageReadyJS + "\n\r" + KendoIconFix;;
                FastTabsHTML = pg.FastTabsHTML;
                FastTabsJS = pg.FastTabsJS;
                PageTemplates = pg.PageTemplates;
                PageCustomJS = pg.PageCustomJS;
                PageArrayDataJS = pg.PageArrayDataJS;
                PageErrorsOrWarnings +=  pg.PageErrorsOrWarnings;
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
            }
        }

    }
}