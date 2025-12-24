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
    public partial class NewLogin : System.Web.UI.Page
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

        public string HomeTabHrefURL
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["HomeTabHrefURL"];

            }

            set
            {
                return;
            }
        }

        public string HomeTabLogo
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["HomeTabLogo"];

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

        public string PageDataTableHTML { get; set; }

        public String Caller
        {
            get
            {
                string strCaller = "Default.aspx"; //sample token
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["Caller"] != null)
                {
                    strCaller = (string)HttpContext.Current.Session["Caller"];
                    if (string.IsNullOrWhiteSpace(strCaller)) { strCaller = "Default.aspx"; }
                }
                return strCaller;

            }

            set
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["Caller"] = value;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PageControl = (int)Utilities.PageEnum.Login;

                //if (UserControl == 0)
                //{
                HttpRequest request = HttpContext.Current.Request;
                var uc = request.QueryString["uc"];
                int ius = 0;
                if (uc != null)
                {
                    var suc = uc.ToString();
                    int.TryParse(suc, out ius);
                }
                //    UserControl = ius;
                //}

                if (!IsPostBack)
                {
                    //HttpRequest request = HttpContext.Current.Request;
                    string sCaller = request.QueryString["caller"];
                    if (!string.IsNullOrWhiteSpace(sCaller) && ius != 0)
                    {
                        try
                        {
                            if (HttpContext.Current.Session != null)
                            {
                                HttpContext.Current.Session["UserControl"] = UserControl.ToString();
                                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + sCaller, false);
                                Context.ApplicationInstance.CompleteRequest();
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            //do nothing
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(sCaller)) { Caller = sCaller; } else { Caller = "Default.aspx"; }

                    CM.cmPageBuilder pg = new CM.cmPageBuilder();
                    pg.PageControl = PageControl;
                    pg.UserControl = UserControl;
                    this.UserTheme = pg.UserTheme;
                    this.PageFooterHTML = pg.CreatePageFooter(PageControl, UserControl);
                    this.AuthLoginNotificationHTML = pg.AuthLoginNotificationHTML;
                    this.PageDataTableHTML = pg.createDataEntryFormTableSample(this.PageControl);
                    PageMenuTab = pg.CreateMenuTabStrip(PageControl, UserControl);

                    //The About page is the same for everyone and is always visible
                    //We always use usercontrol 0 to render this page content as a result
                    //However, if a user is signed in and visits this page the users individual menu tree 
                    //is built based on UserControl
                    //PageReadyJS = pg.getMenuTree(UserControl);
                    //PageReadyJS += pg.getHelpWindow();
                    //pg.createPageDetails(PageControl, 0);

                    PageReadyJS += pg.PageReadyJS; // + "\n\r" + KendoIconFix;;
                    FastTabsHTML = pg.FastTabsHTML;
                    FastTabsJS = pg.FastTabsJS;
                    PageTemplates = pg.PageTemplates;
                    PageCustomJS = pg.PageCustomJS;
                    PageArrayDataJS = pg.PageArrayDataJS;
                    PageErrorsOrWarnings += pg.PageErrorsOrWarnings;
                }

            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
            }


            //string siName = HttpContext.Current.User.Identity.Name;

            //string rName = Request.ServerVariables["logon_user"];

            //string s = "Success";
        }

        public string getLoginTable(CM.cmPageBuilder pg)
        {
            string sTable = "";
            string sScripts = "";
            string sTemplates = "";
            string sDocReady = "";
            List<LTS.cmPageDetail> sRecods = new List<LTS.cmPageDetail>();
            LTS.cmPageDetail uRow = new LTS.cmPageDetail();
            uRow.PageDetPageControl = this.PageControl;
            uRow.PageDetGroupTypeControl = 2;
            uRow.PageDetGroupSubTypeControl = 13;
            uRow.PageDetCaption = "User Name";
            uRow.PageDetName = "txtNGLUserName";
            uRow.PageDetVisible = true;
            sRecods.Add(uRow);
            LTS.cmPageDetail pRow = new LTS.cmPageDetail();
            pRow.PageDetPageControl = this.PageControl;
            pRow.PageDetGroupTypeControl = 2;
            pRow.PageDetGroupSubTypeControl = 13;
            pRow.PageDetCaption = "Password";
            pRow.PageDetName = "txtNGLPass";
            pRow.PageDetVisible = true;
            sRecods.Add(pRow);

            sTable = pg.createDataEntryFormTable(sRecods, 249);

            return sTable;
        }
    }
}