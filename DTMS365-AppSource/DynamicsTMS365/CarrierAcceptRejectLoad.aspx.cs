using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NGL.Core;
using System.Data;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using CM = DynamicsTMS365.ContentManagement;

namespace DynamicsTMS365
{
    public partial class CarrierAcceptRejectLoad : NGLWebUIBaseClass
    {
        public string sToken { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.TokenSupportEmail = "support@nextgeneration.com";
                this.TokenSupportPhone = "847-963-0007";
                HttpRequest request = HttpContext.Current.Request;
                if (request.QueryString.GetValues("token") != null && request.QueryString.GetValues("token").Length != 0)
                {

                    sToken = request.QueryString.GetValues("token")[0];
                }


                PageControl = (int)Utilities.PageEnum.CarrierBookAppt;
                //refreshUserControl();
                CM.cmPageBuilder pg = new CM.cmPageBuilder();
                pg.UserControl = 0;
                pg.PageControl = PageControl;
                this.UserTheme = pg.UserTheme;
                DAL.NGLtblServiceTokenData oTokenDAL = new DAL.NGLtblServiceTokenData(Utilities.DALWCFParameters);
                DAL.Models.CarrierAcceptLoadWithTokenData oTokenData = oTokenDAL.CarrierAcceptLoadWithTokenData(sToken);
                if (oTokenData == null || oTokenData.BookControl == 0)
                {
                    PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>Invalid Token</ h4>";
                    return;
                }
                this.BookConsPrefix = oTokenData.BookConsPrefix;
                this.BookControl = oTokenData.BookControl;
                this.BookDateLoad = oTokenData.BookDateLoad;
                this.BookDateRequired = oTokenData.BookDateRequired;
                this.BookSHID = oTokenData.BookSHID;
                this.CarrierControl = oTokenData.CarrierControl;
                this.CompControl = oTokenData.CompControl;
                this.CompName = oTokenData.CompName;
                this.ExpirationDate = oTokenData.ExpirationDate;
                this.ExpirationMinutes = oTokenData.ExpirationMinutes;
                this.LECarAllowCarrierAcceptRejectByEmail = oTokenData.LECarAllowCarrierAcceptRejectByEmail;
                this.LECarCarrierAuthCarrierAcceptRejectExpMin = oTokenData.LECarCarrierAuthCarrierAcceptRejectExpMin;
                this.LaneControl = oTokenData.LaneControl;
                this.LECarCarrierAuthCarrierAcceptRejectByEmail = oTokenData.LECarCarrierAuthCarrierAcceptRejectByEmail;
                this.TokenSupportEmail = oTokenData.TokenSupportEmail;
                this.TokenSupportPhone = oTokenData.TokenSupportPhone;
                this.OriginNameAddressCSZ = oTokenData.OriginNameAddressCSZ;
                this.DestNameAddressCSZ = oTokenData.DestNameAddressCSZ;
                this.CarrierContControl = oTokenData.CarrierContControl;
                this.CarrierName = oTokenData.CarrierName;

                this.PageFooterHTML = pg.CreatePageFooter(PageControl, UserControl);
                this.AuthLoginNotificationHTML = pg.AuthLoginNotificationHTML;
                PageMenuTab = pg.CreateMenuTabStrip(PageControl, UserControl);

                //The About page is the same for everyone and is always visible
                //We always use usercontrol 0 to render this page content as a result
                //However, if a user is signed in and visits this page the users individual menu tree 
                //is built based on UserControl
                //PageReadyJS = pg.getMenuTree(UserControl);
                //PageReadyJS += pg.getHelpWindow();
                pg.createPageDetails(PageControl, 0);

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

    public string BookConsPrefix { get; set; }
    public int BookControl { get; set; }
    public DateTime BookDateLoad { get; set; }
    public DateTime BookDateRequired { get; set; }
    public string BookSHID { get; set; }
    public int CarrierControl { get; set; }
    public int CompControl { get; set; }
    public string CompName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int ExpirationMinutes { get; set; }
    public int LaneControl { get; set; }
    public bool LECarAllowCarrierAcceptRejectByEmail { get; set; }
    public int LECarCarrierAuthCarrierAcceptRejectExpMin { get; set; }
    public bool LECarCarrierAuthCarrierAcceptRejectByEmail { get; set; } 
    public string TokenSupportEmail { get; set; }
    public string TokenSupportPhone { get; set; }
    public string OriginNameAddressCSZ { get; set; }
    public string DestNameAddressCSZ { get; set; }
    public int CarrierContControl { get; set; }
    public string CarrierName { get; set; }

    }
}