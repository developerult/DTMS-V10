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
    public partial class FreeTrialRegistration : System.Web.UI.Page
    {
        public int PageControl { get; set; }

        private string _UserTheme = "blueopal";
        public string UserTheme
        {
            get
            {
                return _UserTheme;
            }
            set
            {
                _UserTheme = value;
            }
        }

        public string UserToken
        {
            get
            {
                string strToken = "abc123"; //sample token
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserToken"] != null)
                {
                    strToken = (string)HttpContext.Current.Session["UserToken"];
                }
                return strToken;
            }

            set
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["UserToken"] = value;
                }
            }
        }

        public string UserName
        {
            get
            {
                string strUserName = "System"; //sample token
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserName"] != null)
                {
                    strUserName = (string)HttpContext.Current.Session["UserName"];
                }
                return strUserName;
            }

            set
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["UserName"] = value;
                }
            }
        }

        public int UserControl
        {
            get
            {
                int intControl = 0;
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserControl"] != null)
                {
                    string suc = HttpContext.Current.Session["UserControl"].ToString();
                    bool blnTry = int.TryParse(suc, out intControl);
                    //bool blnTry = int.TryParse((string)HttpContext.Current.Session["UserControl"], out intControl);
                }
                return intControl;
            }

            set
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["UserControl"] = value;
                }
            }
        }

        public string WebBaseURI
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"];

            }

            set
            {
                return;
            }
        }
        public string RequireAuthenticationOnAllPages
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["RequireAuthenticationOnAllPages"];

            }
        }

        public string idaClientId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["idaClientId"];

            }

            set
            {
                return;
            }
        }

        public string FastTabsHTML { get; set; }
        public string FastTabsJS { get; set; }
        public string PageTemplates { get; set; }
        public string PageCustomJS { get; set; }
        public string PageArrayDataJS { get; set; }
        string _PageReadyJS = "null";
        public string PageReadyJS { get { return _PageReadyJS; } set { _PageReadyJS = value; } }
        public string PageErrorsOrWarnings { get; set; }
        public static List<string> datasources = new List<string>();
        public string PageMenuHTML { get; set; }
        public string PageFooterHTML { get; set; }
        public string AuthLoginNotificationHTML { get; set; }

        string _PageMenuTab = "null";
        public string PageMenuTab { get { return _PageMenuTab; } set { _PageMenuTab = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PageControl = (int)Utilities.PageEnum.FreeTrialRegistration;

                HttpRequest request = HttpContext.Current.Request;
                if (UserControl == 0)
                {

                    var uc = request.QueryString["uc"];
                    int ius = 0;
                    if (uc != null)
                    {
                        var suc = uc.ToString();
                        int.TryParse(suc, out ius);
                    }
                    UserControl = ius;
                }

                CM.cmPageBuilder pg = new CM.cmPageBuilder();
                pg.UserControl = UserControl;
                pg.PageControl = PageControl;
                this.UserTheme = pg.UserTheme;
                this.PageFooterHTML = pg.CreatePageFooter(PageControl, UserControl);
                this.AuthLoginNotificationHTML = pg.AuthLoginNotificationHTML;

                PageMenuTab = pg.CreateMenuTabStrip(PageControl, UserControl);

                PageReadyJS = pg.getMenuTree(UserControl);
                PageReadyJS += pg.getHelpWindow();

                if (UserControl != 0)
                {
                    pg.createPageDetails(PageControl, UserControl);
                }


                PageReadyJS += pg.PageReadyJS; //+ "\n\r" + KendoIconFix;;
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