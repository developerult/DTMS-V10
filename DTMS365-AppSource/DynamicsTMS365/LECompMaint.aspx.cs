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
    public partial class LECompMaint : NGLWebUIBaseClass
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PageControl = (int)Utilities.PageEnum.CompanyMaint;
                refreshUserControl();
                CM.cmPageBuilder pg = new CM.cmPageBuilder();
                UserGroupCategory = this.SSOR.CatControl;
                pg.UserGroupCategory = UserGroupCategory;
                pg.UserControl = UserControl;
                pg.PageControl = PageControl;

                HttpRequest request = HttpContext.Current.Request;
                if (request.QueryString.GetValues("CompControl") != null && request.QueryString.GetValues("CompControl").Length != 0)
                {
                    pg.addToDictOverrideSavedFilterArray("AllRecordsFilter", "CompControl", request.QueryString.GetValues("CompControl")[0]);
                }

                this.UserTheme = pg.UserTheme;
                this.PageFooterHTML = pg.PageFooterHTML;
                this.AuthLoginNotificationHTML = pg.AuthLoginNotificationHTML;
                PageMenuTab = pg.CreateMenuTabStrip(PageControl, UserControl);
                PageReadyJS = pg.getMenuTree(UserControl);
                PageReadyJS += pg.getHelpWindow();
                PageReadyJS += pg.menuTreeHover();
                if (UserControl != 0) { pg.createPageDetails(PageControl, UserControl); }
                PageReadyJS += pg.PageReadyJS + "\n\r" + KendoIconFix;
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