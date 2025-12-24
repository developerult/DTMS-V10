using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    public partial class SettlementCopy03152020 : NGLWebUIBaseClass
    {

        public string EditAccessorialsMessage { get; set; }
        public string ReadOnlyAccessorialsMessage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EditAccessorialsMessage = Utilities.getLocalizedMsg("M_SettlmentEditAccessorial");
                ReadOnlyAccessorialsMessage = Utilities.getLocalizedMsg("M_SettlmentDisplayAccessorial");
                if (string.IsNullOrWhiteSpace(EditAccessorialsMessage)) { EditAccessorialsMessage = "Enter Accessorials Below; click the Cost value to edit. All accessorials entered require approval."; }
                if (string.IsNullOrWhiteSpace(ReadOnlyAccessorialsMessage)) { ReadOnlyAccessorialsMessage = "Accessorial are being evaluated and cannot be modified."; }

                PageControl = (int)Utilities.PageEnum.Settlement;
                refreshUserControl();

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

                if (UserControl != 0)
                {
                    pg.createPageDetails(PageControl, UserControl);
                }

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