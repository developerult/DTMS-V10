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
    public partial class RateShoppingQ : NGLWebUIBaseClass
    {

        public int RatingDefaultPltWidth { get; set; }
        public int RatingDefaultPltHeight { get; set; }
        public int RatingDefaultPltLength { get; set; }
        public string RatingDefaultPkgType { get; set; }
        public string RatingDefaultFreightClass { get; set; }
        public double RatingDefaultWeight { get; set; }
        public string RatingDefaultWeightUnit { get; set; }
        public string RatingDefaultLengthUnit { get; set; }
        public string DefaultCountryCode { get; set; }

        protected void readParameterPackageInfo()
        {
            int LEControl = this.SSOR.UserLEControl;
            RatingDefaultPltWidth = Convert.ToInt32(Utilities.GetParValueByLegalEntity("RatingDefaultPltWidth", LEControl));
            RatingDefaultPltHeight = Convert.ToInt32(Utilities.GetParValueByLegalEntity("RatingDefaultPltHeight", LEControl));
            RatingDefaultPltLength = Convert.ToInt32(Utilities.GetParValueByLegalEntity("RatingDefaultPltLength", LEControl));
            RatingDefaultPkgType = Utilities.GetParTextByLegalEntity("RatingDefaultPkgType", LEControl);
            RatingDefaultFreightClass = Utilities.GetParTextByLegalEntity("RatingDefaultFreightClass", LEControl);
            RatingDefaultWeight = Utilities.GetParValueByLegalEntity("RatingDefaultWeight", LEControl);
            RatingDefaultWeightUnit = Utilities.GetParTextByLegalEntity("RatingDefaultWeightUnit", LEControl);
            RatingDefaultLengthUnit = Utilities.GetParTextByLegalEntity("RatingDefaultLengthUnit", LEControl);
            DefaultCountryCode = Utilities.GetParTextByLegalEntity("COMPANYCOUNTRY", LEControl);
        }
        protected void loadDefaultPackageInfo()
        {

            RatingDefaultPltWidth = 40;
            RatingDefaultPltHeight = 48;
            RatingDefaultPltLength = 48;
            RatingDefaultPkgType = "PLT";
            RatingDefaultFreightClass = "70";
            RatingDefaultWeight = 500;
            RatingDefaultWeightUnit = "LB";
            RatingDefaultLengthUnit = "IN";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loadDefaultPackageInfo();
                PageControl = (int)Utilities.PageEnum.RateShoppingQ;
                refreshUserControl();

                CM.cmPageBuilder pg = new CM.cmPageBuilder();
                UserGroupCategory = this.SSOR.CatControl;
                pg.UserGroupCategory = UserGroupCategory;
                pg.UserControl = UserControl;
                pg.PageControl = PageControl;
                this.UserTheme = pg.UserTheme;
                this.PageFooterHTML = pg.CreatePageFooter(PageControl, UserControl);
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
                readParameterPackageInfo();
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
            }

        }
    }
}