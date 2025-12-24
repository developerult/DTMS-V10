using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using CM = DynamicsTMS365.ContentManagement;
using DynamicsTMS365.Controllers;


namespace DynamicsTMS365
{
    public partial class LoadBoardItems : NGLWebUIBaseClass
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PageControl = (int)Utilities.PageEnum.LoadBoardItems;
                refreshUserControl();

                CM.cmPageBuilder pg = new CM.cmPageBuilder();
                UserGroupCategory = this.SSOR.CatControl;
                pg.UserGroupCategory = UserGroupCategory;
                pg.UserControl = UserControl;
                pg.PageControl = PageControl;
                //Modified by RHR for v-8.5.4.004 on 12/06/2023 added logic to call page from multiple sources using parent id
                HttpRequest request = HttpContext.Current.Request;
                if (request.QueryString.GetValues("parent") != null && request.QueryString.GetValues("parent").Length != 0)
                {
                    int iParent = 0;
                    int.TryParse(request.QueryString.GetValues("parent")[0], out iParent);
                    BookControlKey = iParent;
                    Utilities.SavePageSetting( "BookControl", BookControlKey.ToString(), PageControl,UserControl);

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
                else
                {
                    string sParent = Utilities.ReadPageSetting("BookControl", PageControl, UserControl);
                    if (!string.IsNullOrWhiteSpace(sParent))
                    {
                        RedirectRequired = true;
                        sRedirect = WebBaseURI + "LoadBoardItems?parent=" + sParent;
                    }
                    else
                    {
                        PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + Utilities.getLocalizedMsg("E_InvalidParentOrRecordKeyField") + "</ h4>";

                    }
                }
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
            }
            if (RedirectRequired)
            {
                Response.Redirect(sRedirect, false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }


        }
    }
}