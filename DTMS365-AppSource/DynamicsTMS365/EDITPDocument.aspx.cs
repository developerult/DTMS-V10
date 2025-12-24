using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsTMS365
{
    public partial class EDITPDocument : System.Web.UI.Page
    {
        /// <summary>
        /// On Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Page Control Property
        /// </summary>
        public int PageControl { get; set; }
        //blueopal
        private string _UserTheme = "blueopal";
        /// <summary>
        /// User Defined Theme
        /// </summary>
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
        /// <summary>
        /// User Token
        /// </summary>
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
        /// <summary>
        /// UserName
        /// </summary>
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
        /// <summary>
        /// UserControl
        /// </summary>
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
        /// <summary>
        /// WebBaseURI
        /// </summary>
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

        /// <summary>
        /// HomeTabHrefURL
        /// </summary>
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
        /// <summary>
        /// HomeTabLogo
        /// </summary>
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
        /// <summary>
        /// ClientId
        /// </summary>
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
        /// <summary>
        /// FastTabsHTML
        /// </summary>
        public string FastTabsHTML { get; set; }
        /// <summary>
        /// FastTabsJS
        /// </summary>
        public string FastTabsJS { get; set; }
        /// <summary>
        /// PageTemplates
        /// </summary>
        public string PageTemplates { get; set; }
        /// <summary>
        /// PageCustomJS
        /// </summary>
        public string PageCustomJS { get; set; }
        /// <summary>
        /// PageArrayDataJS
        /// </summary>
        public string PageArrayDataJS { get; set; }


        string _PageReadyJS = "null";
        /// <summary>
        /// PageReadyJS
        /// </summary>
        public string PageReadyJS { get { return _PageReadyJS; } set { _PageReadyJS = value; } }

        /// <summary>
        /// PageErrorsOrWarnings
        /// </summary>
        public string PageErrorsOrWarnings { get; set; }
        /// <summary>
        /// Data Sources
        /// </summary>
        public static List<string> datasources = new List<string>();
        /// <summary>
        /// Page Menu HTML
        /// </summary>
        public string PageMenuHTML { get; set; }
        /// <summary>
        /// Page Footer HTML
        /// </summary>
        public string PageFooterHTML { get; set; }

        private string _AuthLoginNotificationHTML;
        /// <summary>
        /// AuthLoginNotificationHTML
        /// </summary>
        public string AuthLoginNotificationHTML
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_AuthLoginNotificationHTML))
                {
                    _AuthLoginNotificationHTML = "<span id='notification' style='display:none;'></span><script id='infoTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-info-circle' title='info'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script><script id='errorTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-x-outline' style='color: red;' title='error'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script><script id='successTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-check-outline' style='color: green;' title='success'></span>&nbsp;&nbsp;<div style='margin:.5rem 0;'>#= message #</div></div></script><script id='warningTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-exclamation-circle' style='color: orange;' title='warning'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script><script>var notification = $('#notification').kendoNotification({ position: { pinned: true, top: 30, left: function(e){ var x = ($(window).width() * .5) - 250; return x; } }, autoHideAfter: 0, stacking: 'down', templates: [{type: 'info',template: $('#infoTemplate').html()},{type: 'error',template: $('#errorTemplate').html()},{type: 'success',template: $('#successTemplate').html()},{type: 'warning',template: $('#warningTemplate').html()}]}).data('kendoNotification');</script>";
                }
                return _AuthLoginNotificationHTML;

            }
            set
            {
                _AuthLoginNotificationHTML = value;
            }
        }
        /// <summary>
        /// HelpWindowHTML
        /// </summary>
        public string HelpWindowHTML { get; set; }
        string _PageMenuTab = "null";
        /// <summary>
        /// PageMenuTab
        /// </summary>
        public string PageMenuTab { get { return _PageMenuTab; } set { _PageMenuTab = value; } }
        /// <summary>
        /// PageDataTableHTML
        /// </summary>
        public string PageDataTableHTML { get; set; }
    }
}