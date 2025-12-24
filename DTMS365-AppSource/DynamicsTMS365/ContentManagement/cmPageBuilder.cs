using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.ContentManagement
{
    public class cmPageBuilder
    {
        //Modified by RHR on 6/5/2019  added new dictionary property to override the saved grid filter on a page
        //the key must be equal to the PageDetAPIFilterID in the cmPageDetails Table
        //the value must be a JSON Array string in the following format:
        //[{ filterID: 1, filterCaption: "Warehouse", filterName: "CompName" , filterValueFrom: "wa",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false } ]

        public int UserGroupCategory { get; set; }

        private Dictionary<string, string> _DictOverrideSavedFilterArray;
        public Dictionary<string, string> DictOverrideSavedFilterArray
        {
            get
            {
                if (_DictOverrideSavedFilterArray == null)
                {
                    _DictOverrideSavedFilterArray = new Dictionary<string, string>();

                }
                return _DictOverrideSavedFilterArray;
            }
            set { _DictOverrideSavedFilterArray = value; }
        }


        private DAL.WCFParameters _Parameters;
        public DAL.WCFParameters Parameters
        {
            get
            {
                if (_Parameters == null)
                {
                    //Utilities.DALWCFParameters.CloneParameters(ref _Parameters);
                    _Parameters = Utilities.DALWCFParameters.CloneParameters();
                    //Can call method either way
                    //Note: executeFunc(Utilities.DALWCFParameters.CloneParameters()) -- this function requires a copy of the parameters as arg but we don't have to create an instance to execute the function
                    _Parameters.UserControl = UserControl;
                    //Read user info from dictionary and populate other fields
                    if (Utilities.GlobalSSOResultsByUser.ContainsKey(UserControl))
                    {
                        //Modified By LVV on 4/9/20 - bug fix null reference exception
                        DAL.Models.SSOResults ssoa = Utilities.GlobalSSOResultsByUser[UserControl];
                        if (ssoa != null)
                        {
                            _Parameters.UserName = ssoa.UserName;
                            _Parameters.IsUserCarrier = ssoa.IsUserCarrier;
                            _Parameters.UserCarrierControl = ssoa.UserCarrierControl;
                            _Parameters.UserCarrierContControl = ssoa.UserCarrierContControl;
                            _Parameters.UserLEControl = ssoa.UserLEControl;
                            _Parameters.UserEmail = ssoa.SSOAUserEmail;
                            _Parameters.CatControl = ssoa.CatControl;
                        }
                    }
                }
                return _Parameters;
            }
            set { _Parameters = value; }
        }

        public string FastTabsHTML { get; set; }
        public string FastTabsJS { get; set; }
        //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
        public string PageTemplates { get; set; }
        //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
        public LTS.cmPageTemplate[] AllTemplates { get; set; }

        private string _PageCustomeJS = "";
        public string PageCustomJS
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PageCustomeJS))
                {
                    //the first time we read this property include the PaneSettings
                    _PageCustomeJS = createPaneSettingsJSVariables();
                }
                return _PageCustomeJS;
            }
            set { _PageCustomeJS = value; }
        }
        public string PageArrayDataJS { get; set; }
        string _PageReadyJS = "";
        public string PageReadyJS { get { return _PageReadyJS; } set { _PageReadyJS = value; } }
        public string PageErrorsOrWarnings { get; set; }
        public static List<string> datasources = new List<string>();
        public int UserControl { get; set; }
        public object UserCulture { get; set; }

        private string _cmLineSeperator = " ";
        public string cmLineSeperator
        {
            get
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    return "\n\r";
                }
                else
                {
                    return _cmLineSeperator;
                }
            }
            private set
            {
                _cmLineSeperator = value;
            }
        }

        public int PageControl { get; set; }

        private string _PageMenuHTML;
        public string PageMenuHTML
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PageMenuHTML))
                {
                    _PageMenuHTML = "<div class='pane-content'><span style='float:left; display:inline-block;'><span style='margin:6px; vertical-align: middle;'><a href='Default.aspx'><img border='0' alt='Home' src='../Content/NGL/Home32.png' width='32' height='32'></a></span><span style='margin:6px; vertical-align: middle;' ><a href='http://www.nextgeneration.com'><img border='0' alt='NGL' src='../Content/NGL/nextracklogo.GIF' ></a></span></span><span style='float:right; display:inline-block;'><a id='btnSignInOut' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='signInOut();' href='#' style='margin:6px; vertical-align: top;'><span class='k-icon k-i-user' style='vertical-align: middle;'></span><span id='signInText' style='vertical-align: middle;'>Sign In</span></a><span id='WelcomeMessage' style='margin:6px; vertical-align: middle;'></span><span style='margin:6px; vertical-align: middle;'><a href='Settings.aspx'><img border='0' alt='Settings' src='../Content/NGL/Settings32.png' width='32' height='32'></a></span><span style='margin:6px; vertical-align: middle;'' ><a href='#' onclick='openHelpWindow();return false;'><img border='0' alt='Help' src='../Content/NGL/Help32.png' ></a></span></span></div>";

                }
                return _PageMenuHTML;

            }
            set
            {
                _PageMenuHTML = value;
            }
        }

        private string _PageMenuNoAuthHTML;
        public string PageMenuNoAuthHTML
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PageMenuNoAuthHTML))
                {
                    _PageMenuNoAuthHTML = "<div class='pane-content'><span style='float:left; display:inline-block;'><span style='margin:6px; vertical-align: middle;'><a href='Default.aspx'><img border='0' alt='Home' src='../Content/NGL/Home32.png' width='32' height='32'></a></span><span style='margin:6px; vertical-align: middle;' ><a href='http://www.nextgeneration.com'><img border='0' alt='NGL' src='../Content/NGL/nextracklogo.GIF' ></a></span></span><span style='float:right; display:inline-block;'><span style='margin:6px; vertical-align: middle;'><a href='Settings.aspx'><img border='0' alt='Settings' src='../Content/NGL/Settings32.png' width='32' height='32'></a></span><span style='margin:6px; vertical-align: middle;'' ><a href='#' onclick='openHelpWindow();return false;'><img border='0' alt='Help' src='../Content/NGL/Help32.png' ></a></span></span></div>";

                }
                return _PageMenuNoAuthHTML;

            }
            set
            {
                _PageMenuNoAuthHTML = value;
            }
        }

        private string _PageFooterHTML;
        public string PageFooterHTML
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PageFooterHTML))
                {
                    //_PageFooterHTML = "<div><span><p>This secure site exists to provide On-Line Rate Shopping information. If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: nglsupport@nextgeneration.com'>nglsupport@nextgeneration.com</a></p></span></div>";
                    _PageFooterHTML = CreatePageFooter(0, 0);

                }
                return _PageFooterHTML;

            }
            set
            {
                _PageFooterHTML = value;
            }
        }

        private void appendNotifiTemplate(string sLine, string id, string sDivClass, string sImgClass, string sImgTitle, string sColor, ref System.Text.StringBuilder sbt)
        {

            sbt.Append(string.Format("<script id='{1}' type='text/x-kendo-template'>{0}<div class='{2}'>{0}", sLine, id, sDivClass));
            sbt.Append(string.Format("<span class='{1}' style='color: {3};' title='{2}'></span>&nbsp;&nbsp;<strong>#= title #</strong>{0}", sLine, sImgClass, sImgTitle, sColor));
            sbt.Append(string.Format("<p>#= message #</p></div></script>{0}", sLine));

        }


        //new notification style 11/2/2017
        private string _AuthLoginNotificationHTML;
        public string AuthLoginNotificationHTML
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_AuthLoginNotificationHTML))
                {
                    //System.Text.StringBuilder sbFT = new System.Text.StringBuilder();
                    //string sLine = "";
                    //if (System.Diagnostics.Debugger.IsAttached)
                    //{
                    //    sLine = System.Environment.NewLine;
                    //}

                    //sbFT.Append(string.Format(" < span id='notification' style='display:none;'></span>{0}", sLine));
                    //appendNotifiTemplate(sLine, "infoTemplate", "ngl-notification", "k-icon k-i-info-circle", "info", "blue", ref sbFT);
                    //appendNotifiTemplate(sLine, "errorTemplate", "ngl-notification", "k-icon k-i-x-outline", "error", "red", ref sbFT);
                    //appendNotifiTemplate(sLine, "successTemplate", "ngl-notification", "k-icon k-i-check-outline", "success", "green", ref sbFT);
                    //appendNotifiTemplate(sLine, "warningTemplate", "ngl-notification", "k-icon k-i-exclamation-circle", "warning", "orange", ref sbFT);
                    //sbFT.Append(string.Format("<script>var notification = $('#notification').kendoNotification({ position: { pinned: true, top: 30, left: function (e) { var x = ($(window).width() * .5) - 50; return x; } }, autoHideAfter: 0, stacking: 'down', templates: [{ type: 'info', template: $('#infoTemplate').html() }, { type: 'error', template: $('#errorTemplate').html() }, { type: 'success', template: $('#successTemplate').html() }, { type: 'warning', template: $('#warningTemplate').html() }] }).data('kendoNotification');</script>{0}", sLine));
                    //sbFT.Append(string.Format("<div id='nglAlertDialog'></div><div id='nglConfirmDialog'></div><span id='alertNotification' style='display:none;'></span>{0}", sLine));
                    //sbFT.Append(string.Format("<script id='alertTemplate' type='text/x-kendo-template'>div class='k-block k-error-colored ngl-notification'><span class='k-icon k-i-bell' title='alert'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script>{0}", sLine));
                    //sbFT.Append(string.Format("<script>var alertNotification = $('#alertNotification').kendoNotification({ position: { pinned: true, bottom: 30, right: 30 }, autoHideAfter: 10000, stacking: 'up', templates: [{ type: 'alert', template: $('#alertTemplate').html() }] }).data('kendoNotification');</script>{0}", sLine));
                    ////_AuthLoginNotificationHTML = sbFT.ToString();
                    //_AuthLoginNotificationHTML = "<span id='notification' style='display:none;'></span><script id='infoTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-info-circle' title='info'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script><script id='errorTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-x-outline' style='color: red;' title='error'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script><script id='successTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-check-outline' style='color: green;' title='success'></span>&nbsp;&nbsp;<div style='margin:.5rem 0;'>#= message #</div></div></script><script id='warningTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-exclamation-circle' style='color: orange;' title='warning'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script><script>var notification = $('#notification').kendoNotification({ position: { pinned: true, top: 30, left: function(e){ var x = ($(window).width() * .5) - 250; return x; } }, autoHideAfter: 0, stacking: 'down', templates: [{type: 'info',template: $('#infoTemplate').html()},{type: 'error',template: $('#errorTemplate').html()},{type: 'success',template: $('#successTemplate').html()},{type: 'warning',template: $('#warningTemplate').html()}]}).data('kendoNotification');</script><div id='nglAlertDialog'></div><div id='nglConfirmDialog'></div>" + "<span id='alertNotification' style='display:none;'></span><script id='alertTemplate' type='text/x-kendo-template'><div class='k-block k-error-colored ngl-notification'><span class='k-icon k-i-bell' title='alert'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script><script>var alertNotification = $('#alertNotification').kendoNotification({ position: { pinned: true, bottom: 30, right: 30 }, autoHideAfter: 10000, stacking: 'up', templates: [{type: 'alert',template: $('#alertTemplate').html()}]}).data('kendoNotification');</script>";
                    _AuthLoginNotificationHTML = "<span id='notification' style='display:none;'></span><script id='infoTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-info-circle' title='info'></span>&nbsp;&nbsp;<strong>#= title #</strong><p>#= message #</p></div></script><script id='errorTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-x-outline' style='color: red;' title='error'></span>&nbsp;&nbsp;<strong>#= title #</strong><p>#= message #</p></div></script><script id='successTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-check-outline' style='color: green;' title='success'></span>&nbsp;&nbsp;<p>#= message #</p></div></script><script id='warningTemplate' type='text/x-kendo-template'><div class='k-block ngl-notification'><span class='k-icon k-i-exclamation-circle' style='color: orange;' title='warning'></span>&nbsp;&nbsp;<strong>#= title #</strong><p>#= message #</p></div></script><script>var notification = $('#notification').kendoNotification({ position: { pinned: true, top: 30, left: function(e){ var x = ($(window).width() * .5) - 50; return x; } }, autoHideAfter: 0, stacking: 'down', templates: [{type: 'info',template: $('#infoTemplate').html()},{type: 'error',template: $('#errorTemplate').html()},{type: 'success',template: $('#successTemplate').html()},{type: 'warning',template: $('#warningTemplate').html()}]}).data('kendoNotification');</script><div id='nglAlertDialog'></div><div id='nglConfirmDialog'></div>" + "<span id='alertNotification' style='display:none;'></span><script id='alertTemplate' type='text/x-kendo-template'><div class='k-block k-error-colored ngl-notification'><span class='k-icon k-i-bell' title='alert'></span>&nbsp;&nbsp;<strong>#= title #</strong><p>#= message #</p></div></script><script>var alertNotification = $('#alertNotification').kendoNotification({ position: { pinned: true, bottom: 30, right: 30 }, autoHideAfter: 10000, stacking: 'up', templates: [{type: 'alert',template: $('#alertTemplate').html()}]}).data('kendoNotification');</script>";

                    //this.AuthLoginNotificationHTML = "<span id='notification' style='display:none;'></span><script id='infoTemplate' type='text/x-kendo-template'><div class='ngl-notification'><span class='k-icon k-i-info-circle' title='info'></span>&nbsp;&nbsp;<strong>#= title #</strong><p>#= message #</p></div></script><script id='errorTemplate' type='text/x-kendo-template'><div class='ngl-notification'><img src='../Content/NGL/OK16.png' /> <h3>#= title #</h3><p>#= message #</p></div></script><script id='successTemplate' type='text/x-kendo-template'><div class='ngl-notification'><span class='k-icon k-i-check-outline' style='color: green;' title='success'></span>&nbsp;&nbsp;<p>#= message #</p></div></script><script id='warningTemplate' type='text/x-kendo-template'><div class='ngl-notification'><span class='k-icon k-i-exclamation-circle' style='color: orange;' title='warning'></span>&nbsp;&nbsp;<strong>#= title #</strong><div style='margin:.5rem 0;'>#= message #</div></div></script><script>var notification = $('#notification').kendoNotification({ position: { pinned: true, top: 30, left: function (e) { var x = ($(window).width() * .5) - 50; return x; } }, autoHideAfter: 0, stacking: 'down', templates: [{ type: 'info', template: $('#infoTemplate').html() }, { type: 'error', template: $('#errorTemplate').html() }, { type: 'success', template: $('#successTemplate').html() }, { type: 'warning', template: $('#warningTemplate').html() }] }).data('kendoNotification');</script>";

                }
                return _AuthLoginNotificationHTML;

            }
            set
            {
                _AuthLoginNotificationHTML = value;
            }
        }

        //old notification style
        //private string _AuthLoginNotificationHTML;
        //public string AuthLoginNotificationHTML
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(_AuthLoginNotificationHTML))
        //        {
        //            _AuthLoginNotificationHTML = "<span id='notification' style='display:none;'></span><script id='infoTemplate' type='text/x-kendo-template'><div class='ngl-info'><img src='../Content/NGL/Information16.png' /><h3>#= title #</h3><p>#= message #</p></div></script><script id='errorTemplate' type='text/x-kendo-template'><div class='ngl-error'><img src='../Content/NGL/Error16.png' /><h3>#= title #</h3><p>#= message #</p></div></script><script id='successTemplate' type='text/x-kendo-template'><div class='ngl-success'><img src='../Content/NGL/OK16.png' /><h3>#= message #</h3></div></script><script>var notification = $('#notification').kendoNotification({position: {pinned: true,top: 100,right: 200},autoHideAfter: 0,stacking: 'down',templates: [{type: 'info',template: $('#infoTemplate').html()}, {type: 'error',template: $('#errorTemplate').html()}, {type: 'success',template: $('#successTemplate').html()}]}).data('kendoNotification');</script>";

        //        }
        //        return _AuthLoginNotificationHTML;

        //    }
        //    set
        //    {
        //        _AuthLoginNotificationHTML = value;
        //    }
        //}


        private string _UserTheme;
        public string UserTheme
        {
            get
            {
                _UserTheme = Utilities.getUserTheme(UserControl);
                return _UserTheme;
            }
        }

        /// <summary>
        /// Create the page variable used by splitter2 js to assign the users saved pane settings.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/25/2018
        /// </remarks>
        public string createPaneSettingsJSVariables()
        {
            //default
            string sRet = "userBottomPaneSize =  '35px'; userBottomPaneCollapsed = true; userMenuPaneSize = '150px'; userMenuCollapsed = false; userLeftPaneSize = '150px' ; userLeftPaneCollapsed = false; ";
            try
            {
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                LTS.tblUserPageSetting[] oSettings = sDaL.GetPageSettingsForCurrentUser((int)Utilities.PageEnum.Home, "PaneSettings");
                if (oSettings != null && oSettings.Count() > 0)
                {
                    //we read the first record duplicates are ignored
                    string sSetting = oSettings[0].UserPSMetaData;
                    Models.PageSetting[] oPageSettings = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.PageSetting[]>(sSetting);
                    if (oPageSettings != null && oPageSettings.Count() > 0)
                    {
                        sRet = "";
                        foreach (Models.PageSetting s in oPageSettings)
                        {
                            var sVal = s.value.ToLower();
                            var sValue = "";
                            if (sVal == "true")
                            {
                                sValue = "true";
                            }
                            else if (sVal == "false")
                            {
                                sValue = "false";
                            }
                            else
                            {
                                sValue = "'" + s.value + "'";
                            }
                            sRet += string.Format("{0} = {1}; {2}", s.name, sValue, this.cmLineSeperator);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //do nothig just return the default
            }
            return sRet;
        }


        public void addToDictOverrideSavedFilterArray(string sFilterKey, string sfilterName, string sfilterValueFrom = "", string sfilterValueTo = "", string sfilterFrom = "", string sfilterTo = "", bool bfilterIsDate = false, string sfilterCaption = "")
        {
            if (string.IsNullOrWhiteSpace(sFilterKey) || string.IsNullOrWhiteSpace(sfilterName)) { return; }
            int ifilterID = this.DictOverrideSavedFilterArray.Count() + 1;
            if (string.IsNullOrWhiteSpace(sfilterCaption)) { sfilterCaption = sfilterName; }
            string sFilter = string.Format("[{{ filterID: {0}, filterCaption: \"{7}\", filterName: \"{1}\" , filterValueFrom: \"{2}\",filterValueTo: \"{3}\",filterFrom: \"{4}\",filterTo: \"{5}\",filterIsDate: {6} }}]", ifilterID, sfilterName, sfilterValueFrom, sfilterValueTo, sfilterFrom, sfilterTo, bfilterIsDate.ToString().ToLower(), sfilterCaption);

            this.DictOverrideSavedFilterArray.Add(sFilterKey, sFilter);
        }

        bool buildPage(string sPage, string UserToken, ref string Message)
        {
            bool blnRet = false;
            try
            {
                //add code here to read the cm page data and split it out into Fast Tabs, Page Templates, Java Script and Page Data Objects

                blnRet = true;
            }
            catch (FaultException<DAL.SqlFaultInfo> sqlEx)
            {
                string errMsg = string.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString());
                Utilities.SaveSysError(errMsg, sPage, "cmPageBuilder.buildPage");
            }
            catch (Exception)
            {
                throw;
            }

            return blnRet;
        }

        public string createPageDetails(int PageControl, int UserSecControl)
        {
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            LTS.cmPageDetail[] ltsPD = dalSecData.getPageDetailElements(PageControl, UserSecControl);
            if (ltsPD == null || ltsPD.Count() < 1)
            {
                //TODO: add localization code
                //PageErrorsOrWarnings = String.Format("The Page Configuration cannot be found for Page Control # {0}", PageControl);
                return "";
            }
            //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
            //get page level templates using getPageTemplates(PageControl)

            LTS.cmPageTemplate[] oPageTemps = dalSecData.getPageTemplates(PageControl);
            if (oPageTemps != null && oPageTemps.Count() > 0)
            {
                foreach (LTS.cmPageTemplate t in oPageTemps)
                {
                    PageTemplates += t.PageTemplateContent + "\n\r";
                }
            }
            //Get AllTemplates for the elements
            AllTemplates = dalSecData.getAllTemplates();
            //break out the parent objects sorted by sequence number fast tabs
            LTS.cmPageDetail[] PageLevel1 = ltsPD.Where(x => x.PageDetParentID == 0 && x.PageDetVisible == true).OrderBy(y => y.PageDetSequenceNo).ToArray();
            if (PageLevel1 == null || PageLevel1.Count() < 1)
            {
                //TODO: add localization code
                PageErrorsOrWarnings = String.Format("The first level Page Configuration is not valid for Page Control # {0}", PageControl);
                return "";
            }
            string sFieldCRUDTagID = "";
            foreach (LTS.cmPageDetail lvl1 in PageLevel1)
            {
                cmHTMLBuilder[] oHTMLs = processPageDetailsSubTypes(lvl1, ltsPD, UserSecControl, ref sFieldCRUDTagID, lvl1.PageDetTagIDReference);
                //if we needed the CRUD Tag ID we would use it here
                if (oHTMLs != null && oHTMLs.Count() > 0)
                {
                    foreach (cmHTMLBuilder h in oHTMLs)
                    {
                        FastTabsHTML += h.ToString();
                    }
                }



            }
            //// note this code should go into the for each below.  this is just a test
            //System.Text.StringBuilder sbFT = new System.Text.StringBuilder();
            ////sbFT.Append("<div id=\"LocalizationData\">");
            //sbFT.Append(string.Format("<div id=\"{0}Data\">",sKey));
            //sbFT.Append("<div class=\"pane-content\" >");
            //sbFT.Append("<div class=\"fast-tab\" >");
            //sbFT.Append(buildExpandFastTab(sKey));
            //sbFT.Append(buildCollapseFastTab(sKey));
            //sbFT.Append("<span style=\"font-size:small; font-weight:bold\">");
            //sbFT.Append(string.Format("{0}  Data",sKey));
            //sbFT.Append("</span>&nbsp;&nbsp;<br/>");
            //sbFT.Append("<span id=\"");
            //sbFT.Append(string.Format("{0}Header",sKey));
            //sbFT.Append("\"><label>");
            //sbFT.Append(string.Format("{0} Data",sKey));
            //sbFT.Append("</label>");
            //sbFT.Append("</span>");
            //sbFT.Append("</div>");
            //sbFT.Append("<div id=\"");
            //sbFT.Append(string.Format("{0}Detail",sKey));
            //sbFT.Append("\" class=\"OpenOrders\"> ");
            //sbFT.Append("<div id=\"");
            //sbFT.Append(string.Format("{0}Grid", sKey));
            //sbFT.Append("\"></div>");
            //sbFT.Append("</div>");
            //sbFT.Append("</div>");
            //sbFT.Append("</div>");
            //FastTabsHTML = sbFT.ToString();
            //For each PageDetail record returned determine what type of object it is using the GroupSubTypeControl and build it
            //foreach (LTS.cmPageDetail pdh in ltsPD.Where(x => x.PageDetParentID == 0 && x.PageDetVisible == true).OrderBy(x => x.PageDetSequenceNo))
            //{

            //    //first create the parent string
            //    string sRet = "";
            //    if (pdh.PageDetParentID == 0 && pdh.PageDetVisible == true)
            //    {
            //        switch (pdh.PageDetGroupSubTypeControl)
            //        {
            //            case 1:
            //                //kendoGrid
            //                sRet = createkendoGrid(pdh, ltsPD, UserSecControl);
            //                break;
            //            case 2:
            //                //Console.WriteLine("Case 2");
            //                break;
            //            case 9:
            //                //DatePicker
            //                sRet = createkendoDatePicker(pdh.PageDetName);
            //                break;
            //            case 17:
            //                //Button
            //                sRet = createkendoButton(pdh.PageDetName);
            //                break;
            //            default:
            //                //Console.WriteLine("Default case");
            //                break;
            //        }

            //    }
            //    result += sRet;
            //    sRet = "";
            //}
            datasources.Clear();
            return "";
        }

        public string createPageDetailsWSharedFilters(int PageControl, int UserSecControl, int FilterPageControl)
        {
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            LTS.cmPageDetail[] ltsPD = dalSecData.getPageDetailElements(PageControl, UserSecControl);
            string sFieldCRUDTagID = "";
            if (ltsPD == null || ltsPD.Count() < 1)
            {
                FastTabsHTML += createSharedFilters(FilterPageControl, UserSecControl, ref dalSecData, ref sFieldCRUDTagID);
                //TODO: add localization code
                //PageErrorsOrWarnings = String.Format("The Page Configuration cannot be found for Page Control # {0}", PageControl);
                return "";
            }
            //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
            //get page level templates using getPageTemplates(PageControl)

            LTS.cmPageTemplate[] oPageTemps = dalSecData.getPageTemplates(PageControl);
            if (oPageTemps != null && oPageTemps.Count() > 0)
            {
                foreach (LTS.cmPageTemplate t in oPageTemps)
                {
                    PageTemplates += t.PageTemplateContent + "\n\r";
                }
            }
            //Get AllTemplates for the elements
            AllTemplates = dalSecData.getAllTemplates();
            FastTabsHTML += createSharedFilters(FilterPageControl, UserSecControl, ref dalSecData, ref sFieldCRUDTagID);
            //break out the parent objects sorted by sequence number fast tabs
            LTS.cmPageDetail[] PageLevel1 = ltsPD.Where(x => x.PageDetParentID == 0 && x.PageDetVisible == true).OrderBy(y => y.PageDetSequenceNo).ToArray();
            if (PageLevel1 == null || PageLevel1.Count() < 1)
            {
                //TODO: add localization code
                PageErrorsOrWarnings = String.Format("The first level Page Configuration is not valid for Page Control # {0}", PageControl);
                return "";
            }

            foreach (LTS.cmPageDetail lvl1 in PageLevel1)
            {
                cmHTMLBuilder[] oHTMLs = processPageDetailsSubTypes(lvl1, ltsPD, UserSecControl, ref sFieldCRUDTagID, lvl1.PageDetTagIDReference);
                //if we needed the CRUD Tag ID we would use it here
                if (oHTMLs != null && oHTMLs.Count() > 0)
                {
                    foreach (cmHTMLBuilder h in oHTMLs)
                    {
                        FastTabsHTML += h.ToString();
                    }
                }
            }

            datasources.Clear();
            return "";
        }


        public string createSharedFilters(int FilterPageControl, int UserSecControl, ref DAL.NGLSecurityDataProvider dalSecData, ref string sFieldCRUDTagID)
        {
            string sFilterFT = "";
            LTS.cmPageDetail[] ltsFilterPD = dalSecData.getPageDetailElements(FilterPageControl, UserSecControl);
            if (ltsFilterPD != null && ltsFilterPD.Count() > 0)
            {
                // just get the filters
                LTS.cmPageDetail[] FilterPageLevel1 = ltsFilterPD.Where(x => x.PageDetParentID == 0 && x.PageDetVisible == true).OrderBy(y => y.PageDetSequenceNo).ToArray();

                foreach (LTS.cmPageDetail lvl1 in FilterPageLevel1)
                {
                    if (lvl1.PageDetAPIFilterID == "AllRecordsFilter")
                    {
                        var oHTMLs = new List<cmHTMLBuilder>();
                        cmHTMLBuilder oHtml = new cmHTMLBuilder();
                        buildFastTabForKendoGrid(lvl1, ltsFilterPD, UserSecControl, ref oHtml, ref sFieldCRUDTagID);
                        oHTMLs.Add(oHtml);
                        if (oHTMLs != null && oHTMLs.Count() > 0)
                        {
                            foreach (cmHTMLBuilder h in oHTMLs)
                            {
                                sFilterFT += h.ToString();
                            }
                        }
                    }

                }

            }

            return sFilterFT;

        }



        public cmHTMLBuilder[] processPageDetailsSubTypes(LTS.cmPageDetail pdItem, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref string sFieldCRUDTagID, string sParentID = "", bool blnReadOnReady = true)
        {
            cmHTMLBuilder oHtml = new cmHTMLBuilder();
            var oHtmls = new List<cmHTMLBuilder>();
            string sReadyJs = "";
            switch (pdItem.PageDetGroupSubTypeControl)
            {
                case (int)Utilities.GroupSubType.Grid:
                    //kendoGrid
                    //sHtml = "<div id = \"ParGrid\" ></div >";
                    sReadyJs = createkendoGrid(pdItem, ltsPD, UserSecControl, ref oHtml, blnReadOnReady);
                    PageReadyJS += sReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.SpreadSheet:
                    //Console.WriteLine("Case 2");
                    break;
                case (int)Utilities.GroupSubType.DatePicker:
                    //DatePicker TODO: add code to generate HTML
                    sReadyJs = createkendoDatePicker(pdItem.PageDetName);
                    PageReadyJS += sReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.Editor:
                    //Editor TODO: add code to generate HTML
                    sReadyJs = createKendoEditor(pdItem, ltsPD, UserSecControl);
                    PageReadyJS += sReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.Button:
                    //Button TODO: add code to generate HTML
                    sReadyJs = createkendoButton(pdItem.PageDetName);
                    PageReadyJS += sReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.GridFastTab:
                    //Fast Tab For Grid
                    buildFastTabForKendoGrid(pdItem, ltsPD, UserSecControl, ref oHtml, ref sFieldCRUDTagID);
                    //PageReadyJS += sGridReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.FilterSelection:
                    //Fast Tab Filter Selection (New buildKendoGridFastTabFilters method should be used for nested Grid Filters)
                    oHtml.innerHTML = buildFastTabFilterSelection(pdItem, ltsPD, UserSecControl);
                    //PageReadyJS += sGridReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.FormFastTab:
                    //Fast Tab For Form TODO: add code to generate HTML
                    buildFastTabForForm(pdItem, ltsPD, UserSecControl);
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.FastTabAction:
                    //Fast Tab Actions
                    oHtml.innerHTML = buildFastTabActions(pdItem, ltsPD, UserSecControl);
                    //PageReadyJS += sGridReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.EditorCustomTool:
                    //Editor Custom Tool
                    oHtml.innerHTML = createKendoEditorCustomTool(pdItem);
                    //PageReadyJS += sGridReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.EditorStandardTools:
                    //Editor Standard Tools
                    oHtml.innerHTML = createKendoEditorStandardTools(pdItem);
                    //PageReadyJS += sGridReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.NGLEditOnPageCtrl:
                    sReadyJs = this.createNGLEditOnPageWidget(pdItem, ltsPD, UserSecControl, ref oHtml);
                    PageReadyJS += sReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.NGLSummaryDataCtrl:
                    sReadyJs = this.createNGLSummaryDataWidget(pdItem, ltsPD, UserSecControl, ref oHtml);
                    PageReadyJS += sReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.NGLWorkFlowOptionCtrl:
                    sReadyJs = this.createNGLWorkFlowOptionCtrl(pdItem, ltsPD, UserSecControl, ref oHtmls, sParentID);
                    PageReadyJS += sReadyJs;
                    break;
                case (int)Utilities.GroupSubType.NGLWorkFlowSectionCtrl:
                    sReadyJs = this.createNGLWorkFlowSectionCtrl(pdItem, ltsPD, UserSecControl, ref oHtmls, sParentID);
                    PageReadyJS += sReadyJs;
                    break;
                case (int)Utilities.GroupSubType.NGLPopupWindCtrl:
                    sReadyJs = this.createNGLPopupWindCtrl(pdItem, ltsPD, UserSecControl, ref oHtmls, sParentID);
                    PageReadyJS += sReadyJs;
                    oHtmls.Add(oHtml);
                    break;
                case (int)Utilities.GroupSubType.NGLErrWarnMsgLogCtrl:
                    sReadyJs = this.createNGLErrWarnMsgLogCtrl(pdItem, ltsPD, UserSecControl, ref oHtmls, sParentID);
                    PageReadyJS += sReadyJs;
                    oHtmls.Add(oHtml);
                    break;  //createNGLErrWarnMsgLogCtrl
                default:
                    //Console.WriteLine("Default case");
                    break;
            }

            return oHtmls.ToArray();
        }


        #region "Fast Tab"

        public string buildFastTabForKendoGrid(LTS.cmPageDetail pdItem, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref cmHTMLBuilder oHTML, ref string sFieldCRUDTagID)
        {
            string sRet = "";   //contains any Page Ready Java Script needed by this component; the KendoGridFastTab does not require any embended java script in the page ready method;        
            if (ValidatePageItem(pdItem.PageDetName, false) == false) { return sRet; }
            int detailControl = pdItem.PageDetControl;
            string sKey = string.IsNullOrWhiteSpace(pdItem.PageDetTagIDReference) ? pdItem.PageDetName : pdItem.PageDetTagIDReference; // pdItem.PageDetName;           
            string sID = sKey; //sKey + "Data";
            //add the fast tab parent wrapper
            oHTML = new cmHTMLBuilder("div", sID, pdItem.PageDetCSSClass, pdItem.PageDetAttributes, "");
            //add the fast-tab container
            cmHTMLBuilder fasttab = new cmHTMLBuilder("div", "", "fast-tab", "", "");
            //add the inner html for the fast tab
            fasttab.innerHTML = buildExpandFastTab(sKey) + buildCollapseFastTab(sKey, true, false) + " <span style=\"font-size:small; font-weight:bold\" > " + Utilities.GetLocalizedString(pdItem.PageDetCaption, pdItem.PageDetCaptionLocal, null) + "</span>";
            //add the fast tab container to the results
            oHTML.addNestedHTML(fasttab);
            //add the data grid container
            string gridContainerID = sID + "Header"; //must match the id used in the buidFastTab methods above  generally keyHeader
            cmHTMLBuilder gridContainer = new cmHTMLBuilder("div", gridContainerID, "OpenOrders", "", "");
            //get all of the visible children for this Fast Tab
            LTS.cmPageDetail[] lvl2 = ltsPD.Where(x => x.PageDetParentID == detailControl && x.PageDetVisible == true).OrderBy(y => y.PageDetSequenceNo).ToArray();
            if (lvl2 != null || lvl2.Count() > 0)
            {

                foreach (LTS.cmPageDetail detItem in lvl2)
                {

                    if (detItem.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.Grid) { sFieldCRUDTagID = detItem.PageDetTagIDReference; }
                    string sItemFieldCRUDTagID = "";
                    cmHTMLBuilder[] oHTMLs = processPageDetailsSubTypes(detItem, ltsPD, UserSecControl, ref sItemFieldCRUDTagID, pdItem.PageDetTagIDReference);
                    //if we need the CRUDTagID we could use it here
                    if (oHTMLs != null && oHTMLs.Count() > 0)
                    {
                        foreach (cmHTMLBuilder h in oHTMLs)
                        {
                            gridContainer.addNestedHTML(h);
                        }
                    }

                }
            }
            //now add the nested html elements to the parent
            oHTML.addNestedHTML(gridContainer);

            //old code removed to support additional HTML processing the primary caller of processPageDetailsSubTypes must update the 
            //FastTabsHTML property, soon to be renamed  PageHTML
            //System.Text.StringBuilder sbFT = new System.Text.StringBuilder();
            //sbFT.Append(string.Format("<div id=\"{0}Data\">", sKey));
            ////sbFT.Append("\n\r");
            //    sbFT.Append("<div class=\"pane-content\" >");
            //    //sbFT.Append("\n\r");
            //        sbFT.Append("<div class=\"fast-tab\" >");
            //        //sbFT.Append("\n\r");
            //            sbFT.Append(buildExpandFastTab(sKey));
            //            //sbFT.Append("\n\r");
            //            sbFT.Append(buildCollapseFastTab(sKey));
            //            //sbFT.Append("\n\r");
            //            sbFT.Append(string.Format("<span style=\"font-size:small; font-weight:bold\">{0}  Data</span><br/><div id=\"{0}Header\"><label>{0} Filters and Actions</label>", sKey));
            //            //sbFT.Append("\n\r");
            //            sbFT.Append(sFastTabActions);
            //            //sbFT.Append("\n\r");
            //            sbFT.Append(sFilterDetails);
            //            //sbFT.Append("\n\r");
            //            sbFT.Append("</div>"); //close filter details
            //            //sbFT.Append("\n\r");
            //            sbFT.Append(string.Format("<div id =\"{0}Detail\" class=\"OpenOrders\">", sKey));
            //                sbFT.Append(sPageDetails);
            //                sbFT.Append("\n\r");
            //            sbFT.Append("</div>");
            //            //sbFT.Append("\n\r");
            //        sbFT.Append("</div>");
            //        //sbFT.Append("\n\r");
            //    sbFT.Append("</div>");
            //    //sbFT.Append("\n\r");
            //sbFT.Append("</div>");
            ////sbFT.Append("\n\r");
            //FastTabsHTML += sbFT.ToString();
            return sRet;
        }

        public string buildFastTabForForm(LTS.cmPageDetail pdItem, LTS.cmPageDetail[] ltsPD, int UserSecControl)
        {
            //TODO:  Add Code to parse ltsPD and insert the child fields inside of the Form area below
            string sRet = "";
            int detailControl = pdItem.PageDetControl;
            string sKey = pdItem.PageDetName;
            string sFilterDetails = "";
            string sFastTabActions = "";
            string sFastTabForm = "";
            //get all of the children for this page detail
            //Note: need to be modified to work like the grid pages where we use the new cmhtmlbuilder
            LTS.cmPageDetail[] lvl2 = ltsPD.Where(x => x.PageDetParentID == detailControl && x.PageDetVisible == true).OrderBy(y => y.PageDetSequenceNo).ToArray();
            if (lvl2 != null || lvl2.Count() > 0)
            {
                foreach (LTS.cmPageDetail detItem in lvl2)
                {
                    if (detItem.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.FilterSelection)
                    {
                        string sItemFieldCRUDTagID = "";
                        cmHTMLBuilder[] oHTMLs = processPageDetailsSubTypes(detItem, ltsPD, UserSecControl, ref sItemFieldCRUDTagID, pdItem.PageDetTagIDReference);
                        //if we needed the childs CRUD Tag ID we would use it here
                        if (oHTMLs != null && oHTMLs.Count() > 0)
                        {
                            foreach (cmHTMLBuilder h in oHTMLs)
                            {
                                sFilterDetails += h.ToString();
                            }
                        }
                    }
                    else if (detItem.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.FilterSelection)
                    {
                        string sItemFieldCRUDTagID = "";
                        cmHTMLBuilder[] oHTMLs = processPageDetailsSubTypes(detItem, ltsPD, UserSecControl, ref sItemFieldCRUDTagID, pdItem.PageDetTagIDReference);
                        //if we needed the childs CRUD Tag ID we would use it here

                        if (oHTMLs != null && oHTMLs.Count() > 0)
                        {
                            foreach (cmHTMLBuilder h in oHTMLs)
                            {
                                sFastTabActions += h.ToString();
                            }
                        }
                    }
                    else if (detItem.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.FormFastTab)
                    {
                        string sItemFieldCRUDTagID = "";
                        cmHTMLBuilder[] oHTMLs = processPageDetailsSubTypes(detItem, ltsPD, UserSecControl, ref sItemFieldCRUDTagID, pdItem.PageDetTagIDReference);
                        //if we needed the childs CRUD Tag ID we would use it here

                        if (oHTMLs != null && oHTMLs.Count() > 0)
                        {
                            foreach (cmHTMLBuilder h in oHTMLs)
                            {
                                sFastTabForm += h.ToString();
                            }
                        }
                    }
                    else
                    {
                        string sItemFieldCRUDTagID = "";
                        cmHTMLBuilder[] oHTMLs = processPageDetailsSubTypes(detItem, ltsPD, UserSecControl, ref sItemFieldCRUDTagID, pdItem.PageDetTagIDReference);
                        //if we needed the childs CRUD Tag ID we would use it here                      
                    }

                }
            }
            System.Text.StringBuilder sbFT = new System.Text.StringBuilder();
            //sbFT.Append("<div id=\"LocalizationData\">");
            sbFT.Append(string.Format("<div id=\"{0}Data\"><div class=\"pane-content\" ><div class=\"fast-tab\" >", sKey));
            sbFT.Append(buildExpandFastTab(sKey));
            sbFT.Append(buildCollapseFastTab(sKey));
            sbFT.Append(string.Format("<span style=\"font-size:small; font-weight:bold\">{0}  Data</span>&nbsp;&nbsp;<br/><span id=\"{0}Header\"><label>{0} Data</label></span>", sKey));
            sbFT.Append(sFastTabActions);
            sbFT.Append(sFilterDetails);
            sbFT.Append(string.Format("</div><div id=\"{0}Detail\" class=\"OpenOrders\"><div id=\"{0}Form\">", sKey));
            sbFT.Append(sFastTabForm);
            sbFT.Append("</div></div></div></div>");
            FastTabsHTML += sbFT.ToString();
            return sRet;
        }

        public string buildKendoGridFastTabFilters()
        {
            string sRet = "";


            return sRet;
        }

        public string buildFastTabFilterSelection(LTS.cmPageDetail pdItem, LTS.cmPageDetail[] ltsPD, int UserSecControl)
        {
            //ToDo:  add code to loop through all the filter elements and add then to the return string
            // similar to the sample below.  Note each filter selection segment should be encapsulated inside its own fast tab
            //return  string.Format("<span id=\"Expand{0}Span\" style=\"display:none;\">&nbsp;&nbsp;<img id=\"imgExpandAvailableLoads\" onclick=\"expandFastTab('Expand{0}Span','Collapse{0}Span','{0}Header','{0}Detail');\"  border=\"0\" alt=\"Expand\" src=\"../Content/NGL/expand.png\" width=\"12\" height=\"12\"  /></span>", sKey);
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            LTS.cmElementField[] ltsEF;
            ltsEF = dalSecData.getElementFields(pdItem.PageDetControl, UserSecControl);
            //Get the ID field for the datasource model
            var pk = ltsEF.Where(x => x.ElmtFieldPK = true).FirstOrDefault();
            System.Text.StringBuilder sbHTML = new System.Text.StringBuilder();

            //get the page details for group type 2	Editors
            LTS.cmPageDetail[] pdds = ltsPD.Where(x => x.PageDetParentID == pdItem.PageDetControl && x.PageDetVisible == true && x.PageDetGroupTypeControl == 2).OrderBy(x => x.PageDetSequenceNo).ToArray();
            if (pdds != null && pdds.Count() > 0)
            {

                sbHTML.Append(buildExpandFastTab(pdItem.PageDetName));
                sbHTML.Append(buildCollapseFastTab(pdItem.PageDetName));
                sbHTML.Append(String.Format("<span id=\"{0}Header\"><label>{1}</label></span>", pdItem.PageDetName, pdItem.PageDetCaption));
                sbHTML.Append(String.Format("<div id=\"{0}Detail\" ><ul class=\"filterfieldlist\" id=\"{0}fieldlist\">", pdItem.PageDetName));

                //Add the filters to the page 
                foreach (LTS.cmPageDetail pdd in pdds)
                {
                    if (pdd != null)
                    {
                        string sFormat = "";
                        //get the element used for masks and lookup information
                        LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                        if (ef != null)
                        {
                            sFormat = ef.ElmtFieldFormat;
                            //TODO: add logic to look up combo box lookup details when needed
                        }
                        DateTime dtVal = DateTime.Now;
                        DateTime.TryParse(sFormat, out dtVal);
                        //ToDo: add enum to look up sub types
                        //for now we support the following:'
                        // 9 -- DatePicker
                        //10 -- DateTimePicker
                        //13 -- MaskedTextBox
                        //15 -- NumericTextBox
                        switch (pdd.PageDetGroupSubTypeControl)
                        {
                            case 9:
                                //DatePicker
                                sbHTML.Append(insertkendoDatePickerLiHTML(pdd));
                                PageReadyJS += createkendoDatePicker(pdd.PageDetName, dtVal);
                                break;
                            case 10:
                                //DatePicker
                                sbHTML.Append(insertkendoDateTimePickerLiHTML(pdd));
                                PageReadyJS += createkendoDateTimePicker(pdd.PageDetName, dtVal);
                                break;
                            case 13:
                                //DatePicker
                                sbHTML.Append(insertkendoMaskedTextBoxLiHTML(pdd));
                                PageReadyJS += createkendoMaskedTextBox(pdd.PageDetName, sFormat);
                                break;
                            case 15:
                                //NumericTextBox
                                //Todo: add logict for precision like decimal places
                                sbHTML.Append(insertkendoNumericTextBoxLiHTML(pdd));
                                PageReadyJS += createkendoNumericTextBox(pdd.PageDetName, sFormat);
                                break;
                            default:
                                //Console.WriteLine("Default case");
                                break;
                        }
                    }
                }
            }
            sbHTML.Append("</ul></div>");
            return sbHTML.ToString();
        }

        public string buildFastTabActions(LTS.cmPageDetail pdItem, LTS.cmPageDetail[] ltsPD, int UserSecControl)
        {
            //ToDo:  add code to loop through all the action elements and add then to the return string
            // similar to the sample below.  Note each action selection segment should be encapsulated inside its own fast tab
            //eturn  string.Format("<span id=\"Expand{0}Span\" style=\"display:none;\">&nbsp;&nbsp;<img id=\"imgExpandAvailableLoads\" onclick=\"expandFastTab('Expand{0}Span','Collapse{0}Span','{0}Header','{0}Detail');\"  border=\"0\" alt=\"Expand\" src=\"../Content/NGL/expand.png\" width=\"12\" height=\"12\"  /></span>", sKey);
            return "";
        }

        public string buildExpandFastTab(string sKey, bool bIncHeader = true, bool bIncDetail = true)
        {
            string sRet = "";
            string sHeader = "null";
            if (bIncHeader) { sHeader = "'" + sKey + "Header'"; } //single quotes are required
            string sDetail = "null";
            if (bIncDetail) { sDetail = "'" + sKey + "Detail'"; } //single quotes are required

            sRet = string.Format("<span id=\"Expand{0}Span\" style=\"display:none;\"><a onclick=\"expandFastTab('Expand{0}Span','Collapse{0}Span',{1},{2});\"><span style=\"font - size: small; font - weight:bold;\" class=\"k-icon k-i-chevron-down\"></span></a></span>", sKey, sHeader, sDetail);
            return sRet;
        }

        public string buildCollapseFastTab(string sKey, bool bIncHeader = true, bool bIncDetail = true)
        {
            string sRet = "";
            string sHeader = "null";
            if (bIncHeader) { sHeader = "'" + sKey + "Header'"; } //single quotes are required
            string sDetail = "null";
            if (bIncDetail) { sDetail = "'" + sKey + "Detail'"; } //single quotes are required

            sRet = string.Format("<span id=\"Collapse{0}Span\" style=\"display:normal;\"><a onclick=\"collapseFastTab('Expand{0}Span','Collapse{0}Span',{1},{2});\"><span style=\"font - size: small; font - weight:bold;\" class=\"k-icon k-i-chevron-up\"></span></a></span>", sKey, sHeader, sDetail);
            return sRet;
        }

        #endregion

        #region "Original Grid/DataSource Code"
        //This is Rob's original code as of 11/15/2017

        ////public string createkendoGrid(LTS.cmPageDetail pdh, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref cmHTMLBuilder oHtml)
        ////{
        ////    DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
        ////    LTS.cmElementField[] ltsEF;
        ////    string result = "";

        ////    //first create the grid header info
        ////    string sRet = "";
        ////    string sKey = string.IsNullOrWhiteSpace(pdh.PageDetTagIDReference) ? pdh.PageDetName : pdh.PageDetTagIDReference;
        ////    LTS.cmDataElement de = dalSecData.getDataElement(pdh.PageDetDataElmtControl);
        ////    oHtml = new cmHTMLBuilder("div", sKey + "wrapper", "", "", "");
        ////    //oHtml = new cmHTMLBuilder("div", sKey, pdh.PageDetCSSClass, pdh.PageDetAttributes,"");


        ////    sRet = "\n\r$('#" + sKey + "').kendoGrid({" +
        ////            "\n\r theme: \"" + this.UserTheme + "\"," +
        ////            "\n\r dataSource: " + de.DataElmtName + "," +
        ////            "\n\r pageable: " + pdh.PageDetAllowPaging.ToString().ToLower() + "," +
        ////            "\n\r sortable: " + pdh.PageDetAllowSort.ToString().ToLower() + "," +
        ////            "\n\r resizable: true," +
        ////            "\n\r groupable: true";

        ////    //Get the element fields associated with this grid
        ////    ltsEF = dalSecData.getElementFields(pdh.PageDetControl, UserSecControl);
        ////    int iPK = pdh.PageDetElmtFieldControl;
        ////    //Get the ID field for the datasource model
        ////    LTS.cmElementField pk = ltsEF.Where(x => x.ElmtFieldControl == iPK).FirstOrDefault();
        ////    if (pk == null || pk.ElmtFieldControl == 0)
        ////    {
        ////        pk = ltsEF.Where(x => x.ElmtFieldPK == true).FirstOrDefault();
        ////    }




        ////    bool blnHasColumns = false;
        ////    string strColumns = "";
        ////    bool blnShowFilters = false;
        ////    //define the filter object ids
        ////    string sFilterFastTabID = sKey + "FilterFastTab";
        ////    string sFilterFastTabCaption = "Filters"; //default value actual caption comes from pageDetCaption below
        ////    string sFilterDataID = sKey + "filterData";
        ////    //data entry fields (for now we only suppot one text and two date fields
        ////    string sFilterValID = "txt" + sKey + "FilterVal";
        ////    string sdpFilterFromID = "dp" + sKey + "FilterFrom";
        ////    string sdpFilterToID = "dp" + sKey + "FilterTo";
        ////    string sddlFilterListID = "ddl" + sKey + "Filters";
        ////    //html wrapper spans 
        ////    string sTextFilterSpanID = "sp" + sKey + "filterText";
        ////    string sDateFilterSpanID = "sp" + sKey + "filterDates";
        ////    string sButtonFilterSpanID = "sp" + sKey + "filterButtons";
        ////    string sFilterWrapperDivID = sFilterFastTabID + "Header";  //must use the same key used to generate the fast tabs with the Header suffix
        ////    //buttons
        ////    string sbtnFilterID = "btn" + sKey + "Filter";
        ////    string sbtnClearFilterID = "btn" + sKey + "ClearFilter";
        ////    //sorting options
        ////    string stxtSortDirectionID = "txt" + sKey + "SortDirection";
        ////    string stxtSortFieldID = "txt" + sKey + "SortField";
        ////    string sJSDateFilterCaseStatement = "";
        ////    string sJSDateFilterFromToFields = "";
        ////    string sJSDateFilterFromToSplitter = "";  //used for && between filters
        ////    System.Text.StringBuilder sbJSFilterDataArray = new System.Text.StringBuilder("var " + sKey + "filterData = [ ");

        ////    sbJSFilterDataArray.AppendFormat("{{ text: \"{0}\", value: \"{1}\" }}", "", "None"); //always add None as a filter option

        ////    //Create the grid field layout
        ////    foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == pdh.PageDetControl).OrderBy(x => x.PageDetSequenceNo))
        ////    {
        ////        if (pdd != null)
        ////        {
        ////            if (pdd.PageDetGroupSubTypeControl == 20)
        ////            {
        ////                sFilterFastTabCaption = Utilities.GetLocalizedString(pdd.PageDetCaption, pdd.PageDetCaptionLocal, null);
        ////                blnShowFilters = pdd.PageDetVisible;
        ////            }
        ////            else //add the fields to the grid
        ////            {

        ////                LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
        ////                if (ef != null)
        ////                {
        ////                    string sFilterText = "";
        ////                    string sFilterValue = "";
        ////                    blnHasColumns = true;
        ////                    strColumns += "\n\r{";



        ////                    if (!String.IsNullOrWhiteSpace(ef.ElmtFieldName))
        ////                    {
        ////                        sFilterValue = ef.ElmtFieldName;
        ////                        strColumns += "\n\rfield: \"" + sFilterValue + "\"";

        ////                    }
        ////                    if (!String.IsNullOrWhiteSpace(pdd.PageDetCaption))
        ////                    {
        ////                        sFilterText = Utilities.GetLocalizedString(pdd.PageDetCaption, pdd.PageDetCaptionLocal, null);
        ////                        strColumns += ", title: \"" + sFilterText + "\"";
        ////                    }
        ////                    if (pdd.PageDetWidth != 0)
        ////                    {
        ////                        strColumns += ", width: " + pdd.PageDetWidth.ToString();
        ////                    }
        ////                    if (pdd.PageDetVisible == false)
        ////                    {
        ////                        strColumns += ", hidden: true ";
        ////                    }
        ////                    if (!String.IsNullOrWhiteSpace(ef.ElmtFieldFormat))
        ////                    {
        ////                        strColumns += ", template: \"#= " + ef.ElmtFieldFormat + "#\"";
        ////                    }

        ////                    strColumns += "},";

        ////                    if (pdd.PageDetAllowFilter && !string.IsNullOrWhiteSpace(sFilterValue))
        ////                    {
        ////                        sbJSFilterDataArray.AppendFormat(",{{ text: \"{0}\", value: \"{1}\" }}", (string.IsNullOrWhiteSpace(sFilterText) ? sFilterValue : sFilterText), sFilterValue);
        ////                    }

        ////                    switch (ef.ElmtFieldDataTypeControl)
        ////                    {
        ////                        case 5:
        ////                        case 6:
        ////                        case 7:
        ////                        case 8:
        ////                        case 22:
        ////                            //these are all datetime fields so we use the date from and date to filters
        ////                            sJSDateFilterCaseStatement += "\n\r case \"" + sFilterValue + "\":";
        ////                            //sJSDateFilterFromToFields is as the condition inside an if statement to test if the selected field uses the date from and to filters
        ////                            sJSDateFilterFromToFields += sJSDateFilterFromToSplitter + " dataItem.value === \"" + sFilterValue + "\" ";
        ////                            sJSDateFilterFromToSplitter = " || ";
        ////                            break;
        ////                        default:
        ////                            break;
        ////                    }
        ////                }
        ////            }

        ////        }
        ////    }
        ////    sbJSFilterDataArray.Append(" ];"); //close the javascript array 
        ////    if (blnHasColumns == true)
        ////    {
        ////        //remove the last comma
        ////        strColumns = strColumns.Remove(strColumns.Length - 1);

        ////        sRet += "\n\r, columns: [" + strColumns;
        ////        sRet += "]";
        ////    }
        ////    sRet += "\n\r});";
        ////    //Build the filter inner html string
        ////    string sGridFilterHTML = string.Format("<div id=\"{0}\" style=\"padding: 10px;\"><span><label for=\"{1}\" > {2}:</ label><input id=\"{1}\" /><span id=\"{3}\"><input id=\"{4}\" /></span><span id=\"{5}\" ><label for=\"{6}\" > {7}:</ label><input id=\"{6}\" /><label for=\"{8}\" > {9}:</ label><input id=\"{8}\" /></span><span id=\"{10}\" ><a id=\"{11}\" ></a><a id=\"{12}\" ></a></span></span><input id=\"{13}\" type=\"hidden\" /><input id=\"{14}\" type=\"hidden\" /></div>", sFilterWrapperDivID, sddlFilterListID, Utilities.getLocalizedMsg("Filter by"), sTextFilterSpanID, sFilterValID, sDateFilterSpanID, sdpFilterFromID, Utilities.getLocalizedMsg("From"), sdpFilterToID, Utilities.getLocalizedMsg("To"), sButtonFilterSpanID, sbtnFilterID, sbtnClearFilterID, stxtSortDirectionID, stxtSortFieldID);
        ////    string sFastTabLabel = string.Format("<span style=\"font-size:small; font-weight:bold\" > {0} </span>", sFilterFastTabCaption);
        ////    string sGridFastTabFilterInnerHTML = string.Concat(buildExpandFastTab(sFilterFastTabID, true, false), buildCollapseFastTab(sFilterFastTabID, true, false), sFastTabLabel, sGridFilterHTML);
        ////    string sGridFastTabStyle = "";
        ////    if (!blnShowFilters) { sGridFastTabStyle = "style=\"display: none;\""; }

        ////    oHtml.addNestedHTML(new cmHTMLBuilder("div", sFilterFastTabID, "", sGridFastTabStyle, sGridFastTabFilterInnerHTML));

        ////    oHtml.addNestedHTML(new cmHTMLBuilder("div", sKey, pdh.PageDetCSSClass, pdh.PageDetAttributes, ""));


        ////    //if (blnShowFilters) {
        ////    //Build the filter inner html string           

        ////    //sRet = sbJSFilterDataArray.ToString() + sRet;
        ////    //if no dates just set the case to NoDatesAvailable (a field that should not exist in the data)
        ////    if (string.IsNullOrWhiteSpace(sJSDateFilterCaseStatement))
        ////    {
        ////        sJSDateFilterCaseStatement += " case \"NoDatesAvailable\":";
        ////    }
        ////    //add the filter dropdown list                
        ////    result = sbJSFilterDataArray.ToString() + string.Format("$(\"#{0}\").kendoDropDownList({{dataTextField: \"text\",dataValueField: \"value\",dataSource: {1},select: function(e) {{var name = e.dataItem.text; var val = e.dataItem.value; $(\"#{2}\").data(\"kendoMaskedTextBox\").value(\"\");$(\"#{3}\").data(\"kendoDatePicker\").value(\"\");$(\"#{4}\").data(\"kendoDatePicker\").value(\"\");switch(val){{case \"None\":$(\"#{5}\").hide();$(\"#{6}\").hide();$(\"#{7}\").hide(); break;{8}$(\"#{5}\").hide();$(\"#{6}\").show();$(\"#{7}\").show();break;default:$(\"#{5}\").show();$(\"#{6}\").hide();$(\"#{7}\").show();break;}}}}}});", sddlFilterListID, sFilterDataID, sFilterValID, sdpFilterFromID, sdpFilterToID, sTextFilterSpanID, sDateFilterSpanID, sButtonFilterSpanID, sJSDateFilterCaseStatement);
        ////    //build the filter string
        ////    string sFilters = string.Format("\n\r var s = new AllFilter();\n\r s.filterName = $(\"#{0}\").data(\"kendoDropDownList\").value();\n\r s.filterValue = $(\"#{1}\").data(\"kendoMaskedTextBox\").value();\n\r s.filterFrom = $(\"#{2}\").data(\"kendoDatePicker\").value();\n\r s.filterTo = $(\"#{3}\").data(\"kendoDatePicker\").value();\n\r s.sortName = $(\"#{4}\").val();s.sortDirection = $(\"#{5}\").val();\n\r s.page = options.data.page;\n\r s.skip = options.data.skip;s.take = options.data.take;", sddlFilterListID, sFilterValID, sdpFilterFromID, sdpFilterToID, stxtSortFieldID, stxtSortDirectionID);


        ////    //}
        ////    //add the js page ready scripts
        ////    //if no date fields set to false;
        ////    if (string.IsNullOrWhiteSpace(sJSDateFilterFromToFields)) { sJSDateFilterFromToFields = "1 === 0"; } //should return false because we have no date fields
        ////    result += string.Format("$(\"#{0}\").kendoMaskedTextBox(); $(\"#{1}\").kendoDatePicker(); $(\"#{2}\").kendoDatePicker(); $(\"#{3}\").kendoButton({{icon: \"filter\",click: function(e) {{ var dataItem = $(\"#{4}\").data(\"kendoDropDownList\").dataItem(); if ({5}){{ var dtFrom = $(\"#{1}\").data(\"kendoDatePicker\").value(); if (!dtFrom) {{ showErrorNotification(\"Required Fields\", \"Filter From date cannot be null\"); return;}}}} $(\"#{6}\").data(\"kendoGrid\").dataSource.read();}}}}); $(\"#{10}\").kendoButton({{icon: \"filter-clear\",click: function(e) {{var dropdownlist = $(\"#{4}\").data(\"kendoDropDownList\"); dropdownlist.select(0);dropdownlist.trigger(\"change\");$(\"#{0}\").data(\"kendoMaskedTextBox\").value(\"\");$(\"#{1}\").data(\"kendoDatePicker\").value(\"\"); $(\"#{2}\").data(\"kendoDatePicker\").value(\"\"); $(\"#{7}\").hide(); $(\"#{8}\").hide(); $(\"#{9}\").hide();$(\"#{6}\").data(\"kendoGrid\").dataSource.read();}}}}); $(\"#{0}\").data(\"kendoMaskedTextBox\").value(\"\");$(\"#{1}\").data(\"kendoDatePicker\").value(\"\");$(\"#{2}\").data(\"kendoDatePicker\").value(\"\");$(\"#{7}\").hide();$(\"#{8}\").hide();$(\"#{9}\").hide();", sFilterValID, sdpFilterFromID, sdpFilterToID, sbtnFilterID, sddlFilterListID, sJSDateFilterFromToFields, sKey, sTextFilterSpanID, sDateFilterSpanID, sButtonFilterSpanID, sbtnClearFilterID);

        ////    //We only want to create any shared datasources one time so check if it has already been created
        ////    if (!datasources.Contains(de.DataElmtName))
        ////    {
        ////        result += createkendoDataSource(de.DataElmtName, pk.ElmtFieldName, pdh, ltsPD, ltsEF, "", sFilters);
        ////        datasources.Add(de.DataElmtName);
        ////    }

        ////    result += sRet;
        ////    sRet = "";

        ////    return result;

        ////}

        ////public string createkendoDataSource(string dsName, string id, LTS.cmPageDetail pdh, LTS.cmPageDetail[] ltsPD, LTS.cmElementField[] ltsEF, string sKeyFilter = "", string sJSFilter = "")
        ////{
        ////    string result = "";

        ////    //Parameter = new kendo.data.DataSource({
        ////    //    transport: {
        ////    //    read:
        ////    //    {
        ////    //        url: "api/Parameter/Get/", 
        ////    //            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        ////    //            type: "GET"},
        ////    //        parameterMap: function(options, operation) { return options; }
        ////    //}, 
        ////    //    schema:
        ////    //{
        ////    //    data: "Data",  
        ////    //        total: "Count", 
        ////    //        model:
        ////    //    {
        ////    //        id: "ParKey",
        ////    //            fields:
        ////    //        {
        ////    //            ParKey: { type: "string" },
        ////    //                ParText: { type: "string" },
        ////    //                ParDescription: { type: "string" },
        ////    //                ParCategoryControl: { type: "number" },
        ////    //                ParIsGlobal: { type: "bool"}
        ////    //        }
        ////    //    },
        ////    //        errors: "Errors"
        ////    //    },
        ////    //    error: function(xhr, textStatus, error) {
        ////    //    showErrorNotification("Get Parameter Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure")); this.cancelChanges();
        ////    //},
        ////    //    pageSize: 10,
        ////    //    serverPaging: true,
        ////    //    sortable: true,
        ////    //    pageable: true,
        ////    //    groupable: true,});
        ////    if (!string.IsNullOrWhiteSpace(sJSFilter))
        ////    {           //" data: { filter: JSON.stringify(function(options){ " + sJSFilter + " })}," +
        ////                //result = "var sFilter = JSON.stringify(function(options){ " + sJSFilter + " });" +
        ////                //        dsName + " = new kendo.data.DataSource({" +
        ////                //        " transport:{ read:{url: 'api/" + pdh.PageDetAPIReference + "/GetRecords/' + sFilter})\"," +
        ////                //        " contentType: \"application / json; charset = utf - 8\"," +
        ////                //        " dataType: 'json',"  +
        ////                //        " headers: { \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452}," +
        ////                //        " type: \"GET\"}," +
        ////                //        " parameterMap: function(options, operation) { return options; }}," +
        ////                //        " schema: {data: \"Data\"," +
        ////                //        " total: \"Count\"," +
        ////                //        " model: {id: \"" + id + "\"," +
        ////                //        "   fields: {";

        ////        //result = dsName + " = new kendo.data.DataSource({" +
        ////        //       " transport:{ " + 
        ////        //       "    read:{ " + 
        ////        //       "        url: \"api/" + pdh.PageDetAPIReference + "/GetRecords\"," +
        ////        //       "        contentType: \"application / json; charset = utf - 8\"," +
        ////        //       "        dataType: 'json'," +
        ////        //       "        data: { filter: JSON.stringify(function(options){ " + sJSFilter + " })}," + 
        ////        //       "        headers: { \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452}," +
        ////        //       "        type: \"GET\"," +
        ////        //       "        parameterMap: function(options, operation) { return options; }" +
        ////        //       "      }" +
        ////        //       "  }," +
        ////        //       " schema: {data: \"Data\"," +
        ////        //       " total: \"Count\"," +
        ////        //       " model: {id: \"" + id + "\"," +
        ////        //       "   fields: {";
        ////        result = "\n\r" + dsName + " = new kendo.data.DataSource({" +
        ////       "\n\r serverSorting: true, " +
        ////       "\n\r serverPaging: true, " +
        ////       "\n\r pageSize: 10," +
        ////       "\n\r transport: { " +
        ////        "\n\r read: function(options) { " +
        ////        sJSFilter +
        ////        "\n\r $.ajax({ " +
        ////        "\n\r url: 'api/" + pdh.PageDetAPIReference + "/GetRecords/' + s, " +
        ////        "\n\r contentType: 'application/json; charset=utf-8', " +
        ////        "\n\r dataType: 'json', " +
        ////        "\n\r data: { filter: JSON.stringify(s) }, " +
        ////        "\n\rheaders: { \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452 }, " +
        ////        "\n\rsuccess: function(data) { " +
        ////        "\n\r   options.success(data); " +
        ////        "\n\r   if (data.Errors != null) { " +
        ////        "\n\r       if (data.StatusCode === 203){ " +
        ////        "\n\r           showErrorNotification(\"Authorization Timeout\", data.Errors); " +
        ////        "\n\r       } " +
        ////        "\n\r       else " +
        ////        "\n\r       { " +
        ////        "\n\r           showErrorNotification(\"Access Denied\", data.Errors); " +
        ////        "\n\r       } " +
        ////        "\n\r   } " +
        ////        "\n\r}, " +
        ////        "\n\r error: function(result) { " +
        ////        "\n\r   options.error(result); " +
        ////        "\n\r } " +
        ////        "\n\r}); " +
        ////        "\n\r },           " +
        ////        "\n\r parameterMap: function(options, operation) { return options; } " +
        ////        "\n\r},  " +
        ////        "\n\r schema: {" +
        ////        "\n\r   data: \"Data\"," +
        ////        "\n\r   total: \"Count\"," +
        ////        "\n\r   model: {" +
        ////        "\n\r       id: \"" + id + "\"," +
        ////        "\n\r       fields: {";
        ////    }
        ////    else
        ////    {
        ////        result = dsName + " = new kendo.data.DataSource({" +
        ////           " transport:{ read:{url: \"api/" + pdh.PageDetAPIReference + "/Get/" + sKeyFilter + "\"," +
        ////           " contentType: \"application / json; charset = utf - 8\"," +
        ////           " dataType: 'json'," +
        ////           " headers: { \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452}," +
        ////           " type: \"GET\"}," +
        ////           " parameterMap: function(options, operation) { return options; }}," +
        ////           " schema: {data: \"Data\"," +
        ////           " total: \"Count\"," +
        ////           " model: {id: \"" + id + "\"," +
        ////           "   fields: {";
        ////    }
        ////    string sSeperater = "";

        ////    //add the fields
        ////    foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == pdh.PageDetControl).OrderBy(x => x.PageDetSequenceNo))
        ////    {
        ////        if (pdd != null)
        ////        {
        ////            LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
        ////            if (ef != null)
        ////            {
        ////                if (!String.IsNullOrWhiteSpace(ef.ElmtFieldName))
        ////                {
        ////                    result += sSeperater + "\n\r     " + ef.ElmtFieldName + ": { type: \"" + getJavaScriptDataType(ef.ElmtFieldDataTypeControl) + "\" }";
        ////                    sSeperater = ",";
        ////                }
        ////            }
        ////        }
        ////    }


        ////    //add the remaining tags
        ////    result += "\n\r     }" + //close fields
        ////        "\n\r     }," + // close models
        ////        "\n\r    errors: \"Errors\"" +
        ////        "\n\r}," +
        ////        "\n\r   error: function(xhr, textStatus, error) {" +
        ////        "\n\r       showErrorNotification(\"Access " + dsName + " Data Failed\", formatAjaxJSONResponsMsgs(xhr, textStatus, error, \" cannot complete your request\")); this.cancelChanges();" +
        ////        "\n\r   }" +
        ////        "\n\r});\n\r\n\r\n\r";

        ////    return result;
        ////}

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdh"></param>
        /// <param name="ltsPD"></param>
        /// <param name="UserSecControl"></param>
        /// <param name="oHtml"></param>
        /// <param name="blnReadOnReady"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR on 08/12/2020 for v-8.3.0.001 
        ///     added logic to support new method to read saved filters 
        ///     supports common logic for filters and new grid group by logic
        ///     added new Group By logic for grids
        /// </remarks>
        /// <remarks>
        ///  Modified by CHA v-8.5 on 06/22/2021
        ///     added logic to suport drag and drop
        /// <remarks>
        public string createkendoGrid(LTS.cmPageDetail pdh, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref cmHTMLBuilder oHtml, bool blnReadOnReady = true)
        {
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            LTS.cmElementField[] ltsEF;
            string result = "";
            //first create the grid header info
            string sRet = "";
            string sKey = string.IsNullOrWhiteSpace(pdh.PageDetTagIDReference) ? pdh.PageDetName : pdh.PageDetTagIDReference;
            LTS.cmDataElement de = dalSecData.getDataElement(pdh.PageDetDataElmtControl);
            if (de == null)
            {
                string sNoDataMsg = string.Concat("<h4>", "No Data Elements For: ", pdh.PageDetName, "</h4>");
                oHtml = new cmHTMLBuilder("div", sKey + "wrapper", "", "", sNoDataMsg);
                return "";
            }
            oHtml = new cmHTMLBuilder("div", sKey + "wrapper", "", "", "");
            //oHtml = new cmHTMLBuilder("div", sKey, pdh.PageDetCSSClass, pdh.PageDetAttributes,"");
            bool blnHasColumns = false;
            string strColumns = "";
            bool blnShowFilters = false;
            //Modified by RHR for v-8.2 on 06/27/2018 
            //  added new logic for filter widget
            //define the filter object ids
            string sFilterFastTabID = sKey + "FilterFastTab";
            //string sFilterFastTabCaption =  "Filters"; //default value actual caption comes from pageDetCaption below
            string sFilterFastTabCaption = Utilities.GetLocalizedString(pdh.PageDetCaption, pdh.PageDetCaptionLocal, null) + " Filters"; //default value actual caption comes from pageDetCaption below

            //sorting options
            string stxtSortDirectionID = "txt" + sKey + "SortDirection";
            string stxtSortFieldID = "txt" + sKey + "SortField";
            string sJSDateFilterCaseStatement = "";
            string sJSDateFilterFromToFields = "";
            string sJSDateFilterFromToSplitter = "";  //used for && between filters
            string sSortable = "\n\r sortable: " + pdh.PageDetAllowSort.ToString().ToLower() + ",";
            //Modified by RHR on 08/12/2020 for v-8.3.0.001 to support new method to read saved filters
            DAL.Models.AllFilters oSavedFilters = readSavedFilters(pdh);
            //Get the element fields associated with this grid
            ltsEF = dalSecData.getElementFields(pdh.PageDetControl, UserSecControl);

            string sGoupBy = "group: [";
            string sGroupArray = "[";
            string sGroupAgg = "";
            string sGroupAggSep = "";
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == pdh.PageDetControl).OrderBy(x => x.PageDetSequenceNo))
            {
                if (pdd.PageDetVisible == true)
                {
                    if (!string.IsNullOrWhiteSpace(pdd.PageDetMetaData) && pdd.PageDetMetaData.Contains("agg:"))
                    {
                        LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();


                        string[] sAggs = pdd.PageDetMetaData.Split(':');
                        if (sAggs.Count() > 1)
                        {
                            if (string.IsNullOrWhiteSpace(sGroupAgg)) { sGroupAgg = " [ "; }
                            string sAgg = sAggs[1];
                            sGroupAgg += sGroupAggSep + "{" + string.Format("field: '{0}', aggregate: '{1}'", ef.ElmtFieldName, sAgg) + "}";
                            sGroupAggSep = ",";
                        }

                    }
                }
            }
            string sDataAgg = "";
            if (!string.IsNullOrWhiteSpace(sGroupAgg))
            {
                sGroupAgg += " ]";
                sDataAgg = "aggregate: " + sGroupAgg;
                sGroupAgg = "aggregates: " + sGroupAgg;
            }

            if (oSavedFilters != null && oSavedFilters.Groups != null && oSavedFilters.Groups.Count > 0)
            {
                string sGBspacer = "";
                foreach (string s in oSavedFilters.Groups)
                {

                    sGoupBy += sGBspacer + "{ field: '" + s + "'," + sGroupAgg + "}";
                    sGroupArray += sGBspacer + "'" + s + "'";
                    sGBspacer = ",";
                }

            }
            sGoupBy += "]";
            sGroupArray += "]";


            //build sorting function
            if (pdh.PageDetAllowSort)
            {
                sSortable = "\n\r sortable: {" +
                    "\n\r     mode: \"single\"," +
                    "\n\r     allowUnsort: true" +
                    "\n\r }," +
                    "\n\r sort: function(e) {" +
                    "\n\r if (!e.sort.dir) { e.sort.dir == \"\"; e.sort.field == \"\"; }" +
                    "\n\r if (!e.sort.field) { e.sort.field == \"\"; }" +
                    "\n\r     $(\"#" + stxtSortDirectionID + "\").val(e.sort.dir);" +
                    "\n\r     $(\"#" + stxtSortFieldID + "\").val(e.sort.field);" +
                    "\n\r},";
            }

            string sPageable = "\n\r pageable: ";
            if (pdh.PageDetAllowPaging) { sPageable += "{ pageSizes: [5, 10, 15, 20, 25, 50]},"; } else { sPageable += "false,"; }
            string sGroupByArrayID = "arr" + sKey + "GroupBy";
            //sRet = "\n\r$('#" + sKey + "').kendoGrid({" +
            sRet = "\n\r$('#" + sKey + "').kendoNGLGrid({" +            
            "\n\r autoBind: false, theme: \"" + this.UserTheme + "\"," +
                    "\n\r toolbarColumnMenu: true," +
                    "\n\r dataSource: " + de.DataElmtName + "," +
                    "\n\r selectable: \"row\"," + sPageable + sSortable +
                    "\n\r dataBound: function(e) { \n\r var tObj = this; \n\r if (typeof (" + pdh.PageDetName + "DataBoundCallBack) !== 'undefined' && ngl.isFunction(" + pdh.PageDetName + "DataBoundCallBack)) { " + pdh.PageDetName + "DataBoundCallBack(e,tObj,'" + sKey + "'); } \n\r }," +
                    "\n\r resizable: true," +
                    "\n\r reorderable: true," +
                    "\n\r scrollable: true," +
                    "\n\r nglCustomDragAndDrop: " + (string.IsNullOrEmpty(pdh.PageDetFieldFormatOverride) && pdh.PageDetFieldFormatOverride.ToLower().Contains("draganddrop") ? "'false'" : "'true'") + "," + // Add a way to enable or disable the functionality 
                    "\n\r nglDragStart: function(dsO, e) { if (typeof (" + pdh.PageDetName + "DragStartCallBack) !== 'undefined' && ngl.isFunction(" + pdh.PageDetName + "DragStartCallBack)) { " + pdh.PageDetName + "DragStartCallBack(dsO, e); } \n\r }," + // Add a way to add custom callback function, Add this option in case the funciton is existing
                    "\n\r nglDragEnd: function(dsO, ddO, isDraggingUp) { if (typeof (" + pdh.PageDetName + "DropCallBack) !== 'undefined' && ngl.isFunction(" + pdh.PageDetName + "DropCallBack)) { " + pdh.PageDetName + "DropCallBack(dsO, ddO, isDraggingUp); } \n\r }," + // Add a way to add custom callback function, Add this option in case the funciton is existing // Modified by CHA on 09/30/2021 to add drag direction
                    "\n\r columnResize: function(e) { this._captureColumnResize(e); }," +
                    "\n\r columnReorder: function(e) { this._captureColumnReorder(e); }," +
                    "\n\r groupable: true," +
                    "\n\r group: function(e) {" +
                    "\n\r   " + sGroupByArrayID + " = new Array();" +
                    "\n\r   if (e.groups.length){" +
                    "\n\r       for (let i = 0; i < e.groups.length; i++){" +
                    "\n\r           " + sGroupByArrayID + ".push(e.groups[i].field);" +
                    "\n\r       }" +
                    "\n\r   }" +
                    //"\n\r   " + de.DataElmtName + ".read(); \n\r" +
                    "\n\r }";



            int iPK = pdh.PageDetElmtFieldControl;
            //Get the ID field for the datasource model
            LTS.cmElementField pk = ltsEF.Where(x => x.ElmtFieldControl == iPK).FirstOrDefault();
            if (pk == null || pk.ElmtFieldControl == 0)
            {
                pk = ltsEF.Where(x => x.ElmtFieldPK == true).FirstOrDefault();
            }
            if (pk == null)
            {
                throw new System.InvalidOperationException("A primary key is not properly configured for this page, please check the grid content management settings or contact technical support.");
            }
            System.Text.StringBuilder sbJSFilterDataArray = new System.Text.StringBuilder("[ ");

            List<string> toolTipClasses = new List<string>();
            bool blnAutoHide = false;
            bool blnHasTooltip = false;
            string sTabStripData = "";
            //Modified by RHR on 08/06/2018 for v-8.2 to support new CRUD Widget
            string sWidgetCommands = "";
            string sWidgetContainerDivID = "";
            string sWidgetCommandTitleSize = "";
            string sWidgetToolBars = "";
            string sCRUDWidgetReadyJS = createNGLEditWidget(pdh, ltsPD, ltsEF, pk.ElmtFieldName, ref sWidgetCommands, ref sWidgetContainerDivID, ref sWidgetCommandTitleSize, ref sWidgetToolBars, UserSecControl, ref toolTipClasses, ref blnHasTooltip);
            if (!string.IsNullOrWhiteSpace(sCRUDWidgetReadyJS))
            {
                oHtml.addNestedHTML(new cmHTMLBuilder("div", sWidgetContainerDivID, "", "", ""));
            }
            // Begin ToolBar Test
            // Begin Remove

            //if (ltsPD.Any(x => x.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.KendoGridExportToolBar))
            //{
            //    if (!string.IsNullOrWhiteSpace(sWidgetToolBars))
            //    {
            //        sRet += ",\n\r toolbar:[{name: \"excel\", excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\", allPages: true}}, " + sWidgetToolBars + "]";
            //    }
            //    else
            //    {
            //        sRet += ",\n\rtoolbar: [\"excel\"],excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\",allPages: true}";
            //    }
            //    //Modified by RHR for v-8.2 on 08/23/2018 added logic to generate a unique file name for the excel export
            //    //sRet += ",\n\rtoolbar: [\"excel\"],excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\",allPages: true}";
            //    //sRet += ",\n\r toolbar:[ { [\"excel\"],excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\",allPages: true}} { name: \"Custom\",    template: '<a class=\"k-button\" href=\"\\#\" onclick=\"return toolbar_click()\"><span class=\"k-icon k-i-add\"></span>Add</a>' }]";
            //    //sRet += ",\n\r toolbar:[{name: \"excel\", excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\",allPages: true}},{ name: \"Custom\",   template: '<a class=\"k-button\" href=\"\\#\" onclick=\"return toolbar_click()\"><span class=\"k-icon k-i-add\"></span>Add</a>' }]";
            //    //sRet += ",\n\r toolbar:[{name: \"excel\", excel:{fileName: \"vCarrierTariffClass.xlsx\", allPages: true}}, { name: \"Custom\", template: '<a class=\"k-button\"  onclick=\"return toolbar_click()\"><span class=\"k-icon k-i-add\"></span>Add</a>' }]";
            //}
            //else if (!string.IsNullOrWhiteSpace(sWidgetToolBars))
            //{
            //    sRet += ",\n\r toolbar:[" + sWidgetToolBars + "]";
            //}

            // End Remove

            //Create the grid field layout
            //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
            //Modified by RHR on 08/29/2018 for v-8.2 to support saved page filter data
            System.Text.StringBuilder sJSSavedFilterArray = new System.Text.StringBuilder();
            //Modified by RHR on 08/12/2020 for v-8.3.0.001 to support new method to read saved
            int iWidth = 0;
            strColumns = createGridFieldColumns(pdh, ltsPD, ltsEF, ref sFilterFastTabCaption, ref sTabStripData, ref blnHasColumns, ref blnHasTooltip, ref toolTipClasses, ref blnAutoHide, ref blnShowFilters, ref sbJSFilterDataArray, ref sJSDateFilterCaseStatement, ref sJSDateFilterFromToFields, ref sJSDateFilterFromToSplitter, ref sJSSavedFilterArray, ref oHtml, UserControl, ref oSavedFilters, ref sWidgetToolBars,ref iWidth,sWidgetCommands, sWidgetCommandTitleSize);
            //modified by RHR for v-8.1 on 02/22/2018 moved Foreach to createGridFieldColumns function to allow loop to be called from other methods
            sbJSFilterDataArray.Append(" ];"); //close the javascript array 
            if (iWidth < 400) { iWidth = 600; }
            //"\n\r width: " + iWidth.ToString() + "," +
            //"\n\r width: " + iWidth.ToString() + "," +
            //sRet = "\n\r$('#" + sKey + "').kendoNGLGrid({" +
            //       "\n\r autoBind: false, theme: \"" + this.UserTheme + "\"," +
            //       "\n\r toolbarColumnMenu: true," +
            //       "\n\r dataSource: " + de.DataElmtName + "," +
            //       "\n\r selectable: \"row\"," + sPageable + sSortable +
            //       "\n\r dataBound: function(e) { \n\r var tObj = this; \n\r if (typeof (" + pdh.PageDetName + "DataBoundCallBack) !== 'undefined' && ngl.isFunction(" + pdh.PageDetName + "DataBoundCallBack)) { " + pdh.PageDetName + "DataBoundCallBack(e,tObj,'" + sKey + "'); } \n\r }," +
            //       "\n\r resizable: true," +
            //       "\n\r reorderable: true," +
            //       "\n\r scrollable: true," +                   
            //       "\n\r nglCustomDragAndDrop: " + (string.IsNullOrEmpty(pdh.PageDetFieldFormatOverride) && pdh.PageDetFieldFormatOverride.ToLower().Contains("draganddrop") ? "'false'" : "'true'") + "," + // Add a way to enable or disable the functionality 
            //       "\n\r nglDragStart: function(dsO, e) { if (typeof (" + pdh.PageDetName + "DragStartCallBack) !== 'undefined' && ngl.isFunction(" + pdh.PageDetName + "DragStartCallBack)) { " + pdh.PageDetName + "DragStartCallBack(dsO, e); } \n\r }," + // Add a way to add custom callback function, Add this option in case the funciton is existing
            //       "\n\r nglDragEnd: function(dsO, ddO, isDraggingUp) { if (typeof (" + pdh.PageDetName + "DropCallBack) !== 'undefined' && ngl.isFunction(" + pdh.PageDetName + "DropCallBack)) { " + pdh.PageDetName + "DropCallBack(dsO, ddO, isDraggingUp); } \n\r }," + // Add a way to add custom callback function, Add this option in case the funciton is existing // Modified by CHA on 09/30/2021 to add drag direction
            //       "\n\r columnResize: function(e) { this._captureColumnResize(e); }," +
            //       "\n\r columnReorder: function(e) { this._captureColumnReorder(e); }," +
            //       "\n\r groupable: true," +
            //       "\n\r group: function(e) {" +
            //       "\n\r   " + sGroupByArrayID + " = new Array();" +
            //       "\n\r   if (e.groups.length){" +
            //       "\n\r       for (let i = 0; i < e.groups.length; i++){" +
            //       "\n\r           " + sGroupByArrayID + ".push(e.groups[i].field);" +
            //       "\n\r       }" +
            //       "\n\r   }" +
            //       //"\n\r   " + de.DataElmtName + ".read(); \n\r" +
            //       "\n\r }";
            // Add toolbar logic here


            //TODO: fix bug where if one grid on the page has the export button all grids on the page have the export button
            //  the Any statement must identify the parent ID
            if (ltsPD.Any(x => x.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.KendoGridExportToolBar))
            {
                if (!string.IsNullOrWhiteSpace(sWidgetToolBars))
                {
                    sRet += ",\n\r toolbar:[{name: \"excel\", excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\", allPages: true}}, " + sWidgetToolBars + "]";
                }
                else
                {
                    sRet += ",\n\rtoolbar: [\"excel\"],excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\",allPages: true}";
                }
                //Modified by RHR for v-8.2 on 08/23/2018 added logic to generate a unique file name for the excel export
                //sRet += ",\n\rtoolbar: [\"excel\"],excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\",allPages: true}";
                //sRet += ",\n\r toolbar:[ { [\"excel\"],excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\",allPages: true}} { name: \"Custom\",    template: '<a class=\"k-button\" href=\"\\#\" onclick=\"return toolbar_click()\"><span class=\"k-icon k-i-add\"></span>Add</a>' }]";
                //sRet += ",\n\r toolbar:[{name: \"excel\", excel:{fileName: \"" + Utilities.timeStampFileName(de.DataElmtName, ".xlsx") + "\",allPages: true}},{ name: \"Custom\",   template: '<a class=\"k-button\" href=\"\\#\" onclick=\"return toolbar_click()\"><span class=\"k-icon k-i-add\"></span>Add</a>' }]";
                //sRet += ",\n\r toolbar:[{name: \"excel\", excel:{fileName: \"vCarrierTariffClass.xlsx\", allPages: true}}, { name: \"Custom\", template: '<a class=\"k-button\"  onclick=\"return toolbar_click()\"><span class=\"k-icon k-i-add\"></span>Add</a>' }]";
            }
            else if (!string.IsNullOrWhiteSpace(sWidgetToolBars))
            {
                sRet += ",\n\r toolbar:[" + sWidgetToolBars + "]";
            }

            //And move toolbar logic

            //End Toolbar test

            if (blnHasColumns == true)
            {
                //remove the last comma
                strColumns = strColumns.Remove(strColumns.Length - 1);

                sRet += "\n\r, columns: [" + strColumns;
                //sRet += "],columnMenu: {columns: true},"; //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
                sRet += "],"; //turned off for now 
            }
            //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
            sRet += sTabStripData;
            sRet += "\n\r}); \n\r ";
            if (blnReadOnReady)
            {
                sRet += de.DataElmtName + ".read(); \n\r";
            }


            ////***** TODO AT THIS POINT WE NEED TO APPEND JAVASCRIPT CODE TO INSTANTIATE THE TOOLTIP
            if (blnHasTooltip)
            {
                string strTipFilter = "";
                string spSeg = "";
                string strAutoHide = "";
                foreach (string t in toolTipClasses)
                {
                    if (t.Contains("filter:"))
                    {
                        sRet += "\n\r\n\r $('#" + sKey + "').kendoTooltip({" + t + "}).data('kendoTooltip');";

                    }
                    else
                    {
                        strTipFilter += spSeg + ".k-grid-content td." + t;
                        spSeg = ", ";
                    }
                }
                if (!string.IsNullOrEmpty(strTipFilter))
                {
                    if (!blnAutoHide)
                    {
                        strAutoHide = "autoHide: false,";
                    }

                    sRet += "\n\r\n\r $('#" + sKey + "').kendoTooltip({" +
                    "\n\r filter: \"" + strTipFilter + "\"," +
                    "\n\r position: \"right\", \n\r " + strAutoHide +
                    "\n\r content: function(e) { \n\r var tObj = $('#" + sKey + "').data('kendoNGLGrid'); \n\r if (typeof (" + pdh.PageDetName + "ToolTipCallBack) !== 'undefined' && ngl.isFunction(" + pdh.PageDetName + "ToolTipCallBack)) { return " + pdh.PageDetName + "ToolTipCallBack(e,tObj); } \n\r }" +
                    "\n\r }).data(\"kendoTooltip\");";
                }

            }

            string sGridFastTabStyle = "";
            if (!blnShowFilters) { sGridFastTabStyle = "display: none;"; }
            //Modified by RHR for v-8.2 on 06/27/2018 added new logic for filter widget
            sFilterFastTabID = "div" + sKey + "filterContent";
            oHtml.addNestedHTML(new cmHTMLBuilder("div", sFilterFastTabID, "", sGridFastTabStyle, ""));

            oHtml.addNestedHTML(new cmHTMLBuilder("div", sKey, pdh.PageDetCSSClass, pdh.PageDetAttributes, ""));

            //Modified by RHR on 08/29/2018 for v-8.2 to support new saved filter logic
            result = string.Format("{4}{5} var {8} = {9}; {5} obj{0}Filters = new AllFiltersCtrl('{10}', '{11}'); var obj{0}FilterData = {1};  obj{0}Filters.loadDefaults(\"{2}\", \"{0}\", obj{0}FilterData, '{3}',  function(results){{ var oKendoGrid = $('#{0}').data(\"kendoNGLGrid\");  if (typeof(oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid))  {{oKendoGrid.dataSource.read(); }} return;}},'{7}'); obj{0}Filters.show(); obj{0}Filters.addSavedFilters({6}); ", sKey, sbJSFilterDataArray.ToString(), sFilterFastTabID, this.UserTheme, sCRUDWidgetReadyJS, this.cmLineSeperator, sJSSavedFilterArray.ToString(), sFilterFastTabCaption, sGroupByArrayID, sGroupArray, pdh.PageDetFieldFormatOverride.ToLower().Contains("savefilters"), pdh.PageDetAPIReference);
            //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
            string sFilterDataStringFunction = " if (typeof (" + pdh.PageDetName + "GetStringData) !== 'undefined' && ngl.isFunction(" + pdh.PageDetName + "GetStringData)) {s.Data = " + pdh.PageDetName + "GetStringData(s); } else { s.Data = '';}; \n\r";
            //build the filter string            
            //Modified by RHR on 02/27/2018 for v-8.1 to support new content management 
            // Modified by RHR on 08/12/2020 for v-8.3.0.001 added new Group By logic for grids 
            string sFilters = string.Format("\n\r var s = obj{0}Filters.data;\n\r s.Groups = {4}; \n\r s.sortName = $(\"#{1}\").val();s.sortDirection = $(\"#{2}\").val();\n\r s.page = options.data.page;\n\r s.skip = options.data.skip;\n\r s.take = options.data.take;\n\r {3}", sKey, stxtSortFieldID, stxtSortDirectionID, sFilterDataStringFunction, sGroupByArrayID);
            //Modified By LVV on 4/9/20 - bug fix for in case options.data.x is null
            //string sFilters = string.Format("\n\r var s = obj{0}Filters.data;\n\r s.sortName = $(\"#{1}\").val();s.sortDirection = $(\"#{2}\").val();\n\r s.page = ngl.intTryParse(options.data.page,1);\n\r s.skip = ngl.intTryParse(options.data.skip,0);\n\r s.take = options.data.take;\n\r {3}", sKey, stxtSortFieldID, stxtSortDirectionID, sFilterDataStringFunction);

            //We only want to create any shared datasources one time so check if it has already been created
            if (!datasources.Contains(de.DataElmtName))
            {
                result += createkendoDataSource(de.DataElmtName, pk.ElmtFieldName, pdh, ltsPD, ltsEF, "", sFilters, sGoupBy, sDataAgg);
                datasources.Add(de.DataElmtName);
            }

            result += sRet;
            sRet = "";

            return result;

        }

        public DAL.Models.AllFilters readSavedFilters(LTS.cmPageDetail pdh)
        {
            DAL.Models.AllFilters oSavedFilters = null;
            string sFilters = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(pdh.PageDetAPIFilterID))
                {
                    DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                    LTS.tblUserPageSetting[] oSettings = sDaL.GetPageSettingsForCurrentUser(pdh.PageDetPageControl, pdh.PageDetAPIFilterID);
                    if (oSettings != null && oSettings.Count() > 0)
                    {
                        //we read the first record duplicates are ignored
                        sFilters = oSettings[0].UserPSMetaData;
                    }
                }


            }
            catch (Exception ex)
            {
                //do nothing on error we just don't have any saved filters
            }

            if (!string.IsNullOrWhiteSpace(sFilters))
            {
                //convert sFilters to all filters object
                try
                {
                    oSavedFilters = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(sFilters);
                }
                catch (Exception ex)
                {
                    //on error do nothing
                }
            }
            return oSavedFilters;
        }


        /// <summary>
        /// Create Fields and Columns for Kendo Grid
        /// </summary>
        /// <param name="pdh"></param>
        /// <param name="ltsPD"></param>
        /// <param name="ltsEF"></param>
        /// <param name="sFilterFastTabCaption"></param>
        /// <param name="sTabStripData"></param>
        /// <param name="blnHasColumns"></param>
        /// <param name="blnHasTooltip"></param>
        /// <param name="toolTipClasses"></param>
        /// <param name="blnAutoHide"></param>
        /// <param name="blnShowFilters"></param>
        /// <param name="sbJSFilterDataArray"></param>
        /// <param name="sJSDateFilterCaseStatement"></param>
        /// <param name="sJSDateFilterFromToFields"></param>
        /// <param name="sJSDateFilterFromToSplitter"></param>
        /// <param name="sJSSavedFilterArray"></param>
        /// <param name="oHtml"></param>
        /// <param name="UserSecControl"></param>
        /// <param name="oSavedFilters"></param>
        /// <param name="sWidgetCommands"></param>
        /// <param name="sWidgetCommandTitleSize"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
        /// Modified by RHR on 08/12/2020 for v-8.3.0.001 
        ///     added logic to support new method in caller to read saved filters 
        ///     data is now passed by ref as a parameter
        ///     supports common logic for filters and new grid group by logic
        /// </remarks>
        public string createGridFieldColumns(LTS.cmPageDetail pdh, LTS.cmPageDetail[] ltsPD
            , LTS.cmElementField[] ltsEF
            , ref string sFilterFastTabCaption
            , ref string sTabStripData
            , ref bool blnHasColumns
            , ref bool blnHasTooltip
            , ref List<string> toolTipClasses
            , ref bool blnAutoHide
            , ref bool blnShowFilters
            , ref System.Text.StringBuilder sbJSFilterDataArray
            , ref string sJSDateFilterCaseStatement
            , ref string sJSDateFilterFromToFields
            , ref string sJSDateFilterFromToSplitter
            , ref System.Text.StringBuilder sJSSavedFilterArray
            , ref cmHTMLBuilder oHtml
            , int UserSecControl
            , ref DAL.Models.AllFilters oSavedFilters  //Modified by RHR on 08/12/2020 for v-8.3.0.001 to support new method in caller to read saved filters
            , ref string sWidgetToolBars
            , ref int iWidth 
            , string sWidgetCommands = ""
            , string sWidgetCommandTitleSize = "")
        {
            string sParentTagID = pdh.PageDetTagIDReference;
            string strColumns = "";
            string strCommands = "";
            string strCommandsSep = "";
            string strCommandTitleSize = "";
            string strFilterDDLSep = "";
            //get any saved filters for this data container.
            //string sFilters = "";
            bool blnApplySavedFilters = false;
            int iFilterID = 0;
            string strSavedFilterSep = "";
            //Modified by RHR for v-8.5.2.005 on 08/18/2022 added logic to read captions from tblDataEntryField
            // this uses content management NGL Kendo Grid PageDetFieldFormatOverride setting of readcaptions
            // along with the PageDetMetaData setting for file type format looks like defiletype = 1
            DTO.tblDataEntryField[] arrDataEntryFields = new DTO.tblDataEntryField[] { };
            if (!string.IsNullOrEmpty(pdh.PageDetFieldFormatOverride) && pdh.PageDetFieldFormatOverride.ToLower().Contains("readcaptions"))
            {
                int iDEFileType = -1;
                if (int.TryParse(ParseDEFileTypeMetaData(pdh.PageDetMetaData), out iDEFileType))
                {
                    if (iDEFileType != -1)
                    {
                        arrDataEntryFields = GettblDataEntryFieldsFiltered(iDEFileType);
                    }
                }
            }

            if (oSavedFilters != null && oSavedFilters.FilterValues != null && oSavedFilters.FilterValues.Count() > 0)
            {

                if (sJSSavedFilterArray == null) { sJSSavedFilterArray = new System.Text.StringBuilder("[ "); } else { sJSSavedFilterArray.Append("[ "); }
                blnApplySavedFilters = true;
            }
            string sToolParTerminal = string.Empty;
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == pdh.PageDetControl).OrderBy(x => x.PageDetSequenceNo))
            {
                if (pdd != null)
                {

                    //Modified by RHR for v-8.4.0.003 on 08/30/2021 added page specific Read Only settings (primarily for the All Tab)
                    // we use the field format override to determin which fields to hide by group hideForGroups (5, 6, 7, 8, 9)
                    if (!string.IsNullOrWhiteSpace(pdd.PageDetFieldFormatOverride) && pdd.PageDetFieldFormatOverride.Length > 13 && pdd.PageDetFieldFormatOverride.Substring(0, 13).ToLower() == "hideforgroups" && pdd.PageDetFieldFormatOverride.Contains("("))
                    {
                        string[] sSplitGroupSection = pdd.PageDetFieldFormatOverride.Split('(');
                        if (sSplitGroupSection[1] != null && !string.IsNullOrWhiteSpace(sSplitGroupSection[1]))
                        {
                            string[] sGroups = sSplitGroupSection[1].Split(',');
                            if (sGroups.Contains(UserGroupCategory.ToString()))
                            {
                                continue;
                            }
                        }
                    }
                    //if (pdh.PageDetName == "AllGrid" && pdd.PageDetName == "Edit" && UserGroupCategory == 4)
                    //{
                    //    continue;
                    //}
                    if (pdd.PageDetGroupSubTypeControl == 20)
                    {
                        sFilterFastTabCaption = Utilities.GetLocalizedString(pdd.PageDetCaption, pdd.PageDetCaptionLocal, null);
                        //blnShowFilters = pdd.PageDetVisible;
                    }
                    else //add the fields to the grid
                    {
                        //check for a tab strip control
                        if (pdd.PageDetGroupSubTypeControl == 18)
                        {

                            sTabStripData = createkendoTapStripDataSource(pdd, ltsPD, ltsEF, UserSecControl, ref oHtml, sParentTagID);
                        }
                        else
                        {
                            /////////////////////////////////////////////////
                            //this is a tooltip column
                            //other columns can have tooltips, but this is a column whose entire purpose is an icon for a hover over
                            if (pdd.PageDetGroupSubTypeControl == 46)
                            {
                                //the css and metadata cannot be null
                                if (!String.IsNullOrWhiteSpace(pdd.PageDetMetaData) && !String.IsNullOrWhiteSpace(pdd.PageDetCSSClass))
                                {
                                    blnHasColumns = true;
                                    blnHasTooltip = true;

                                    toolTipClasses.Add(pdd.PageDetCSSClass);
                                    if (!string.IsNullOrWhiteSpace((pdd.PageDetDesc)) && (pdd.PageDetDesc.ToLower() == "autohide"))
                                    {
                                        blnAutoHide = true;
                                    }

                                    strColumns += "\n\r{ field: \"\"";
                                    //if (!String.IsNullOrWhiteSpace(pdd.PageDetCaption))
                                    //{
                                    //    var tt = Utilities.GetLocalizedString(pdd.PageDetCaption, pdd.PageDetCaptionLocal, null);
                                    //    strColumns += ", title: \"" + tt + "\"";
                                    //}
                                    strColumns += ", attributes: {\"class\": \"" + pdd.PageDetCSSClass + "\"}";
                                    strColumns += ", template: \"" + (pdd.PageDetMetaData).Replace("\"", "'") + "\""; //There can be no internal " inside template "" so replace all literal \" in metadata string with literal '
                                    if (pdd.PageDetWidth != 0)
                                    {
                                        strColumns += ", width: " + pdd.PageDetWidth.ToString();
                                       
                                    }
                                    strColumns += " },";
                                }
                            }

                            //this is a button
                            if (pdd.PageDetGroupSubTypeControl == 17 && pdd.PageDetVisible == true && ValidatePageItem(pdd.PageDetName, false))
                            {
                                //the onclick (from metadata) and name cannot be null
                                if (!String.IsNullOrWhiteSpace(pdd.PageDetMetaData) && !String.IsNullOrWhiteSpace(pdd.PageDetName))
                                {
                                    if (!String.IsNullOrWhiteSpace(pdd.PageDetFieldFormatOverride) && pdd.PageDetFieldFormatOverride.ToLower() == "toolbar")
                                    {
                                        string sCaption = "";                                        

                                        if (!String.IsNullOrWhiteSpace(pdd.PageDetCaption))
                                        {
                                            sCaption = Utilities.GetLocalizedString(pdd.PageDetCaption, pdd.PageDetCaptionLocal, UserCulture);
                                        }

                                        sWidgetToolBars += string.Format("{4}{{ name: \"Custom\", template: '<a class=\"k-button k-button-solid-base k-button-solid k-button-md k-rounded-md\"  onclick=\"{0}()\"><span class=\"{2}\"></span>{3}</a>' }}", pdd.PageDetMetaData, pdd.PageDetCSSClass, pdd.PageDetAttributes, sCaption, sToolParTerminal);
                                        sToolParTerminal = ",";

                                    }
                                    else
                                    {
                                        blnHasColumns = true;

                                        if (string.IsNullOrWhiteSpace(strCommands))
                                        {
                                            //strCommands = this.cmLineSeperator +  "{ command:[ ";
                                            //strCommandTitleSize += ", title: \"Actions\"";
                                            if (pdd.PageDetWidth != 0)
                                            {
                                                strCommandTitleSize = string.Concat(", title: \"Actions\", width: ", pdd.PageDetWidth.ToString());
                                                iWidth += pdd.PageDetWidth;
                                            }
                                            else
                                            {
                                                strCommandTitleSize = ", title: \"Actions\", width: 160";
                                                iWidth += 160;
                                            }
                                            // Modified by RHR for v-8.5.4.006 on 05/13/2024 added missing PageDetPageControl, PageDetControl, PageDetUserSecurityControl properties
                                            strCommandTitleSize += ", PageDetPageControl: " + pdd.PageDetPageControl.ToString();
                                            strCommandTitleSize += ", PageDetControl: " + pdd.PageDetControl.ToString();
                                            strCommandTitleSize += ", PageDetUserSecurityControl: " + pdd.PageDetUserSecurityControl.ToString();


                                        }
                                        //Modified by RHR for v-8.2 we now call createKendoGridButton so code can be shared across multiple procedures
                                        strCommands += createKendoGridButton(strCommandsSep, pdd.PageDetName, pdd.PageDetCaption, pdd.PageDetCaptionLocal, pdd.PageDetCSSClass, pdd.PageDetAttributes, pdd.PageDetMetaData);
                                        if (!string.IsNullOrWhiteSpace(pdd.PageDetCSSClass))
                                        {
                                            toolTipClasses.Add(AddToolTipFilter(pdd, ref blnHasTooltip));
                                        }
                                        strCommandsSep = ",";
                                    }
                                    
                                }
                            }
                            else
                            {

                                /////////////////////////////////////////////////
                                LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                                //Modified by RHR for v-8.1.1 on 05/15/2018 added support to ignore fields with a sequence number greater than 999
                                if (ef != null && pdd.PageDetSequenceNo < 1000)
                                {

                                    string sFilterText = "";
                                    string sFilterValue = "";
                                    blnHasColumns = true;
                                    strColumns += "\n\r{";
                                    string sFieldName = "";
                                    string sCaptionOverride = "";

                                    if (!String.IsNullOrWhiteSpace(ef.ElmtFieldName))
                                    {
                                        sFilterValue = ef.ElmtFieldName;
                                        sFieldName = sFilterValue;
                                        strColumns += "field: \"" + sFilterValue + "\"";
                                        if (arrDataEntryFields != null && arrDataEntryFields.Count() > 0 && arrDataEntryFields.Any(x => x.DEFieldName == ef.ElmtFieldName))
                                        {
                                            sCaptionOverride = arrDataEntryFields.Where(x => x.DEFieldName == ef.ElmtFieldName).Select(y => y.DEFieldDesc).FirstOrDefault();
                                        }

                                    }

                                    if (!String.IsNullOrWhiteSpace(sCaptionOverride))
                                    {
                                        strColumns += ", title: \"" + sCaptionOverride + "\"";
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrWhiteSpace(pdd.PageDetCaption))
                                        {
                                            sFilterText = Utilities.GetLocalizedString(pdd.PageDetCaption, pdd.PageDetCaptionLocal, null);
                                            strColumns += ", title: \"" + sFilterText + "\"";
                                        }
                                    }

                                    if (pdd.PageDetWidth != 0)
                                    {
                                        strColumns += ", width: " + pdd.PageDetWidth.ToString() ;
                                        if (pdd.PageDetVisible == true)
                                        {
                                            iWidth += pdd.PageDetWidth;
                                        }                                           
                                    }
                                    if (pdd.PageDetVisible == false)
                                    {
                                        strColumns += ", hidden: true ";
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(pdd.PageDetMetaData) && pdd.PageDetMetaData.Contains("agg:"))
                                        {
                                            string[] sAggs = pdd.PageDetMetaData.Split(':');
                                            if (sAggs.Count() > 1)
                                            {
                                                string sAgg = sAggs[1];
                                                if (sAggs.Count() > 2)
                                                {
                                                    sFilterText = sAggs[2];
                                                }
                                                strColumns += string.Format(", aggregates: ['{1}'], footerTemplate: '{0}: #=data.{2}? {1}: 0#', groupFooterTemplate: '{0}: #=data.{2}? {1}: 0#'", sFilterText, sAgg, sFieldName);
                                            }
                                        }
                                    }


                                    //Modified by RHR for v-8.2 on 08/22/2018 added support for kendoNGLGrid showhide attribute for columns with with a sequence number less than 100
                                    if (pdd.PageDetSequenceNo < 100)
                                    {
                                        strColumns += ", showhide: 1 ";
                                    }
                                    else
                                    {
                                        strColumns += ", showhide: 0 ";
                                    }
                                    strColumns += ", PageDetPageControl: " + pdd.PageDetPageControl.ToString();
                                    strColumns += ", PageDetControl: " + pdd.PageDetControl.ToString();
                                    strColumns += ", PageDetUserSecurityControl: " + pdd.PageDetUserSecurityControl.ToString(); //Added By LVV on 10/17/19

                                    if (pdd.PageDetPageTemplateControl != 0 && AllTemplates.Any(x => x.PageTemplateControl == pdd.PageDetPageTemplateControl))
                                    {
                                        //if (AllTemplates.Any(x => x.PageTemplateControl == pdd.PageDetPageTemplateControl))
                                        //{
                                        strColumns += ", " + AllTemplates.Where(x => x.PageTemplateControl == pdd.PageDetPageTemplateControl).Select(x => x.PageTemplateContent).FirstOrDefault();
                                        //}

                                    }
                                    else
                                    {

                                        //Modified by RHR for v-8.2 on 09/10/2018 we now store template and format data in two seperate fields
                                        string stemplate = Utilities.cleanFormatOrTemplateData(pdd.PageDetFieldTemplateOverride, ef.ElmtFieldTemplate);
                                        if (!string.IsNullOrWhiteSpace(stemplate))
                                        {
                                            //add the format to the columns
                                            strColumns += ", template: " + stemplate;
                                        }
                                        else if (pdd.PageDetGroupSubTypeControl == 12 || pdd.PageDetGroupSubTypeControl == 13)
                                        {
                                            strColumns += ", template: \"<span title='${" + sFilterValue + "}' style='white-space: nowrap'>${" + sFilterValue + "}</span>\"";
                                        }
                                        string sformat = Utilities.cleanFormatOrTemplateData(pdd.PageDetFieldFormatOverride, ef.ElmtFieldFormat);
                                        if (!string.IsNullOrWhiteSpace(sformat))
                                        {
                                            //add the format to the columns
                                            strColumns += ", format: " + sformat;
                                        }
                                         
                                        //if (!String.IsNullOrWhiteSpace(ef.ElmtFieldFormat))
                                        //{
                                        //    //I changed this so the entire string comes from the database
                                        //    //because sometimes a value uses format and other use template so this was the
                                        //    //easiest way to distinguish without having to do a bunch of tests/comparisions
                                        //    //If you dont' like this way I can fix it - LVV
                                        //    strColumns += ", " + ef.ElmtFieldFormat;
                                        //    ////if (ef.ElmtFieldDataTypeControl == 16)
                                        //    ////{
                                        //    ////    strColumns += ", format: \"" + ef.ElmtFieldFormat + "\"";
                                        //    ////}
                                        //    ////strColumns += ", template: \"#= " + ef.ElmtFieldFormat + "#\""; -- kendo.toString(kendo.parseDate(LTBookDateRequired, 'yyyy-MM-dd'), 'MM/dd/yyyy')  
                                        //}
                                    }
                                    strColumns += ",attributes: {class: \"nowrap\"}},";
                                    //strColumns += "},";

                                    if (pdd.PageDetAllowFilter && !string.IsNullOrWhiteSpace(sFilterValue))
                                    {
                                        blnShowFilters = true;
                                        string strIsDate = "false";
                                        switch (ef.ElmtFieldDataTypeControl)
                                        {
                                            case 5:
                                            case 6:
                                            case 7:
                                            case 8:
                                            case 22:
                                                //these are all datetime fields so we use the date from and date to filters
                                                strIsDate = "true";
                                                break;
                                            default:
                                                break;
                                        }
                                        sbJSFilterDataArray.AppendFormat("{0}{{ filterCaption: \"{1}\", filterName: \"{2}\" , filterValueFrom: \"\",filterValueTo: \"\",filterFrom: \"\",filterTo: \"\",filterIsDate: {3} }}", strFilterDDLSep, (string.IsNullOrWhiteSpace(sFilterText) ? sFilterValue : sFilterText), sFilterValue, strIsDate);
                                        if (blnApplySavedFilters && oSavedFilters.FilterValues.Any(x => x.filterName == sFilterValue))
                                        {
                                            DAL.Models.FilterDetails[] oFilterDetails = oSavedFilters.FilterValues.Where(x => x.filterName == sFilterValue).ToArray();
                                            if (oFilterDetails != null && oFilterDetails.Count() > 0)
                                            {
                                                foreach (DAL.Models.FilterDetails f in oFilterDetails)
                                                {
                                                    iFilterID++;
                                                    sJSSavedFilterArray.AppendFormat("{0}{{ filterID: {1}, filterCaption: \"{2}\", filterName: \"{3}\" , filterValueFrom: \"{4}\",filterValueTo: \"{5}\",filterFrom: \"{6}\",filterTo: \"{7}\",filterIsDate: {8} }}", strSavedFilterSep, iFilterID, (string.IsNullOrWhiteSpace(sFilterText) ? sFilterValue : sFilterText), sFilterValue, f.filterValueFrom, f.filterValueTo, (f.filterFrom.HasValue ? f.filterFrom.Value.ToString("s") : ""), (f.filterTo.HasValue ? f.filterTo.Value.ToString("s") : ""), strIsDate);
                                                    strSavedFilterSep = ",";
                                                }
                                            }
                                        }
                                        strFilterDDLSep = ",";
                                    }

                                }
                            }

                        }

                    }
                }
            }

            //Modified by RHR for v-8.2 on 08/06/2018
            //added logic to support the automated NGL Widget for CRUD operation buttons            
            if (!string.IsNullOrWhiteSpace(strCommands))
            {
                if (string.IsNullOrWhiteSpace(sWidgetCommands))
                {
                    strColumns = string.Format("{0}{{ command:[{1}]{2}}},{0}{3}", this.cmLineSeperator, strCommands, strCommandTitleSize, strColumns);
                }
                else
                {
                    strColumns = string.Format("{0}{{ command:[{1},{2}]{3}}},{0}{4}", this.cmLineSeperator, strCommands, sWidgetCommands, strCommandTitleSize, strColumns);
                }


                //if (!string.IsNullOrWhiteSpace(sWidgetCommands))
                //{
                //    strCommands = strCommands + ", " + sWidgetCommands
                //}
                //strColumns =  strCommands + " ] " + strCommandTitleSize + " }, " + strColumns;

            }
            else if (!string.IsNullOrWhiteSpace(sWidgetCommands))
            {
                blnHasColumns = true;
                if (string.IsNullOrWhiteSpace(sWidgetCommandTitleSize)) { sWidgetCommandTitleSize = ", title: \"Actions\", width: 160"; }
                //we do not have any other buttons just widget buttons so add the commands
                strColumns = string.Format("{0}{{ command:[{1}]{2}}},{0}{3}", this.cmLineSeperator, sWidgetCommands, sWidgetCommandTitleSize, strColumns);
            }
            if (!string.IsNullOrWhiteSpace(pdh.PageDetAPIFilterID) && DictOverrideSavedFilterArray != null && DictOverrideSavedFilterArray.ContainsKey(pdh.PageDetAPIFilterID))
            {
                sJSSavedFilterArray = new System.Text.StringBuilder(DictOverrideSavedFilterArray[pdh.PageDetAPIFilterID]);
            }
            else
            {
                //strCommands += strCommands;
                if (blnApplySavedFilters)
                {
                    //close the array
                    sJSSavedFilterArray.Append(" ]");
                }
            }
            return strColumns;
        }



        /// <summary>
        /// builds a tooltip filter string for the page detail object
        /// </summary>
        /// <param name="pdd"></param>
        /// <param name="blnHasTooltip"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.4.0.002 on 02/28/2021
        /// </remarks>
        public string AddToolTipFilter(LTS.cmPageDetail pdd, ref bool blnHasTooltip)
        {
            string sRet = "";
            if (!string.IsNullOrWhiteSpace(pdd.PageDetCSSClass))
            {
                string sIonOnlyButtonClass = "cm-icononly-button";
                string sfilter = pdd.PageDetCSSClass;
                if (pdd.PageDetCSSClass.Length >= sIonOnlyButtonClass.Length)
                {
                    if (pdd.PageDetCSSClass.Substring(0, sIonOnlyButtonClass.Length) == sIonOnlyButtonClass)
                    {
                        if (pdd.PageDetCSSClass.Length > sIonOnlyButtonClass.Length)
                        {
                            string sRight = pdd.PageDetCSSClass.Right(1);
                            if (!string.IsNullOrWhiteSpace(sRight))
                            {
                                sfilter = sRight;
                            }
                        }
                    }
                }


                string sTip = pdd.PageDetDesc;
                if (string.IsNullOrWhiteSpace(sTip))
                {
                    sTip = pdd.PageDetName;
                }
                sTip = Utilities.getLocalizedMsg(sTip);

                sRet = "filter: '." + sfilter + "', content: function(e){ return '" + sTip + "'; }";
                blnHasTooltip = true;
            }

            return sRet;
        }

        public string createNGLEditWidget(LTS.cmPageDetail pdh, LTS.cmPageDetail[] ltsPD, LTS.cmElementField[] ltsEF, string spkFieldName, ref string sWidgetCommands, ref string sWidgetContainerDivID, ref string sWidgetCommandTitleSize, ref string sToolBars, int UserSecControl, string sKeyFilter = "", string sParentID = "")
        {
            List<string> toolTipClasses = new List<string>();
            bool blnHasTooltip = false;
            return createNGLEditWidget(pdh, ltsPD, ltsEF, spkFieldName, ref sWidgetCommands, ref sWidgetContainerDivID, ref sWidgetCommandTitleSize, ref sToolBars, UserSecControl, ref toolTipClasses, ref blnHasTooltip, sKeyFilter, sParentID);
        }

        /// <summary>
        /// Add an NGLEditWindCtrl to the page linked to the parent grid
        /// </summary>
        /// <param name="pdh"></param>
        /// <param name="ltsPD"></param>
        /// <param name="ltsEF"></param>
        /// <param name="spkFieldName"></param>
        /// <param name="sWidgetCommands"></param>
        /// <param name="sWidgetContainerDivID"></param>
        /// <param name="sWidgetCommandTitleSize"></param>
        /// <param name="sToolBars"></param>
        /// <param name="UserSecControl"></param>
        /// <param name="toolTipClasses"></param>
        /// <param name="blnHasTooltip"></param>
        /// <param name="sKeyFilter"></param>
        /// <param name="sParentID"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/25/2018
        ///   creates page reference to NGLEditWindCtrl html and js
        ///   adds scripts to PageReadyJS which should be included in $(document).ready function 
        /// </remarks>
        public string createNGLEditWidget(LTS.cmPageDetail pdh, LTS.cmPageDetail[] ltsPD, LTS.cmElementField[] ltsEF, string spkFieldName, ref string sWidgetCommands, ref string sWidgetContainerDivID, ref string sWidgetCommandTitleSize, ref string sToolBars, int UserSecControl, ref List<string> toolTipClasses, ref bool blnHasTooltip, string sKeyFilter = "", string sParentID = "")
        {
            System.Text.StringBuilder sbDataFields = new System.Text.StringBuilder("[ ");
            string strDataFieldsSep = "";
            int iPHControl = pdh.PageDetControl;
            if (!ltsPD.Any(x => x.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLEditWindCtrl && x.PageDetParentID == iPHControl))
            {
                return "";
            }

            if (string.IsNullOrWhiteSpace(sParentID)) { sParentID = pdh.PageDetTagIDReference; }
            string strCommandsSep = "";
            //get the ngleditwidget data only one is allowed per grid so just get the first one
            LTS.cmPageDetail nWdgt = ltsPD.Where(x => x.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLEditWindCtrl && x.PageDetParentID == iPHControl).FirstOrDefault();
            if (nWdgt == null) { return ""; } //nothing to do not configured
            //Modified by RHR for v-8.4.0.003 on 08/30/2021 added page specific Read Only settings (primarily for the All Tab)
            // we use the field format override to determin which fields to hide by group hideForGroups (5, 6, 7, 8, 9)
            if (!string.IsNullOrWhiteSpace(nWdgt.PageDetFieldFormatOverride) && nWdgt.PageDetFieldFormatOverride.Length > 13 && nWdgt.PageDetFieldFormatOverride.Substring(0, 13).ToLower() == "hideforgroups" && nWdgt.PageDetFieldFormatOverride.Contains("("))
            {
                string[] sSplitGroupSection = nWdgt.PageDetFieldFormatOverride.Split('(');
                if (sSplitGroupSection[1] != null && !string.IsNullOrWhiteSpace(sSplitGroupSection[1]))
                {
                    string[] sGroups = sSplitGroupSection[1].Split(',');
                    if (sGroups.Contains(UserGroupCategory.ToString()))
                    {
                        return "";
                    }
                }
            }
            //create the data objects for the widget
            //		var wndCompMaintEA = kendo.ui.Window;
            //var wdgtCompMaintEA = new NGLEditWindCtrl()
            string sCleanPageDetName = System.Text.RegularExpressions.Regex.Replace(pdh.PageDetName, @"\s+", "");
            string sobjClass = string.Concat("NGL", sCleanPageDetName, "Class");
            System.Text.StringBuilder sbFieldClass = new System.Text.StringBuilder();
            sbFieldClass.AppendFormat("{0}function {1}() {{", this.cmLineSeperator, sobjClass);
            sWidgetContainerDivID = "div" + nWdgt.PageDetTagIDReference;
            string sDataSourceName = Utilities.GetLocalizedString(nWdgt.PageDetCaption, nWdgt.PageDetCaptionLocal, this.UserCulture);
            string sWindowObjectName = string.Concat("wnd", sCleanPageDetName, "Edit");
            string sNGLEditWindCtrlName = string.Concat("wdgt", sCleanPageDetName, "Edit");
            this.PageCustomJS += string.Format("{0} var {1} = kendo.ui.Window; {0} var {2} = new NGLEditWindCtrl();", this.cmLineSeperator, sWindowObjectName, sNGLEditWindCtrlName);
            //check for Edit button -- Metadata contains CRUD to determine which options are supported
            if (nWdgt.PageDetMetaData.Contains("C"))
            {
                //create the Add Function                 
                string sFunctionName = "openAddNew" + sCleanPageDetName + "Window";
                string sExecBeforeFunc = "execBefore" + sCleanPageDetName + "Insert";
                string sCheckExecBeforeFunc = string.Format("if ( (typeof({0}) === 'undefined' || ngl.isFunction({0}) === false) || ({0}(e,fk,{1}) === true)){{", sExecBeforeFunc, sNGLEditWindCtrlName);

                if (string.IsNullOrWhiteSpace(sKeyFilter)) { sKeyFilter = "null"; }
                this.PageCustomJS += string.Format("{0} function {1}(e,fk) {{{0} {3} {2}.data = null;{0}{2}.show(fk);}} {0}}}{0}", this.cmLineSeperator, sFunctionName, sNGLEditWindCtrlName, sCheckExecBeforeFunc);
                sToolBars += string.Format("{{ name: \"Custom\", template: '<a class=\"k-button k-button-solid-base k-button-solid k-button-md k-rounded-md\"  onclick=\"return {0}(event,' + {1} + ')\"><span class=\"k-icon k-i-add\"></span>Add</a>' }}", sFunctionName, sKeyFilter);
            }
            //check for Edit button -- Metadata contains CRUD to determine which options are supported
            if (nWdgt.PageDetMetaData.Contains("U"))
            {
                //create the Edit Function                 
                string sFunctionName = "openEdit" + sCleanPageDetName + "Window";
                this.PageCustomJS += string.Format("{0} function {1}(e) {{{0} var data = this.dataItem($(e.currentTarget).closest(\"tr\"));{0} var parentGrid = $('#{4}').data(\"kendoNGLGrid\");{0} if (typeof(parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {{ {0}  kendo.ui.progress(parentGrid.element, true); {0} }} {0}{2}.data = data;{0}{2}.read(data.{3}); {0}}}{0}", this.cmLineSeperator, sFunctionName, sNGLEditWindCtrlName, spkFieldName, sParentID);

                //we support updates so add the update button    
                nWdgt.PageDetCSSClass = "cm-icononly-button U";
                nWdgt.PageDetDesc = "Edit";
                sWidgetCommands += createKendoGridButton(strCommandsSep, "Edit" + nWdgt.PageDetName, "", "", nWdgt.PageDetCSSClass, "k-icon k-i-pencil", sFunctionName);
                if (nWdgt.PageDetWidth != 0)
                {
                    sWidgetCommandTitleSize = string.Concat(", title: \"Actions\", width: ", nWdgt.PageDetWidth.ToString());
                }
                else
                {
                    sWidgetCommandTitleSize = ", title: \"Actions\", width: 160";
                }
                toolTipClasses.Add(AddToolTipFilter(nWdgt, ref blnHasTooltip));
                strCommandsSep = ",";
            }
            //check for Delete button -- Metadata contains CRUD to determine which options are supported
            if (nWdgt.PageDetMetaData.Contains("D"))
            {
                //create the Delete Function                 
                string sFunctionName = "delete" + sCleanPageDetName + "Record";
                string sFunctionConfirmDeleteName = "confirmDelete" + sCleanPageDetName + "Record";
                this.PageCustomJS += string.Format("{0} function {1}(iRet,data) {{{0} if (typeof(iRet) === 'undefined' || iRet === null || iRet === 0) {{ return; }} {0}{2}.delete(data.{3}); {0}}}{0}", this.cmLineSeperator, sFunctionName, sNGLEditWindCtrlName, spkFieldName);
                this.PageCustomJS += string.Format("{0} function {1}(e) {{{0} var item = this.dataItem($(e.currentTarget).closest(\"tr\"));{0}if (typeof(item) === 'undefined' || ngl.isObject(item) == false){{ return; }}{0}ngl.OkCancelConfirmation(\"Delete Selected {2} Record\",\"Warning! This action cannot be undone.  Are you sure?\",400,400,null,\"{3}\",item); {0}}}{0}", this.cmLineSeperator, sFunctionConfirmDeleteName, sDataSourceName, sFunctionName);
                nWdgt.PageDetCSSClass = "cm-icononly-button D";
                nWdgt.PageDetDesc = "Delete";
                //we support deletes so add the Delete data button                
                sWidgetCommands += createKendoGridButton(strCommandsSep, "Delete" + nWdgt.PageDetName, "", "", nWdgt.PageDetCSSClass, "k-icon k-i-trash", sFunctionConfirmDeleteName);
                if (string.IsNullOrWhiteSpace(sWidgetCommandTitleSize))
                {
                    if (nWdgt.PageDetWidth != 0)
                    {
                        sWidgetCommandTitleSize = string.Concat(", title: \"Actions\", width: ", nWdgt.PageDetWidth.ToString());
                    }
                    else
                    {
                        sWidgetCommandTitleSize = ", title: \"Actions\", width: 160";
                    }
                }
                toolTipClasses.Add(AddToolTipFilter(nWdgt, ref blnHasTooltip));
                strCommandsSep = ",";
            }

            string sParentTagID = nWdgt.PageDetTagIDReference;
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == pdh.PageDetControl && x.PageDetSequenceNo < 1000).OrderBy(x => x.PageDetSequenceNo))
            {

                if (pdd != null)
                {
                    if (!Utilities.isGroupSubTypeAllowedOnNGLEditWindCtrl(pdd.PageDetGroupSubTypeControl))
                    {
                        continue;
                    }
                    LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                    string sCleanItemPageDetName = System.Text.RegularExpressions.Regex.Replace(pdd.PageDetName, @"\s+", "");
                    sbFieldClass.AppendFormat("{0}{1}: null; ", this.cmLineSeperator, sCleanItemPageDetName);
                    //we ignore fields with a sequence number greater than 999
                    //if (ef != null && pdd.PageDetSequenceNo < 1000)
                    // Modified by RHR for v-8.2.1 on 10/7/19 we now allow html elements taht do not have an element field
                    if (pdd.PageDetSequenceNo < 1000)
                    {
                        if(pdd.PageDetCSSClass == "NGLToolTip"  && !string.IsNullOrWhiteSpace(pdd.PageDetDesc))
                        {
                            //this.PageCustomJS += string.Format("\n\r $(\"#{0}\").on(\"mouseover\", function (event) {{ ngl.showNGLTooltip(event, '{1}',this);}}); \n\r  $(\"#{0}\").on(\"mouseout\", ngl.hideNGLTooltip);", pdd.PageDetTagIDReference, pdd.PageDetDesc);
                        }
                        
                        addDateFieldItem(ref strDataFieldsSep, pdd, ef, ref sbDataFields, sParentTagID, sCleanItemPageDetName);
                    }
                }
            }
            sbFieldClass.AppendFormat("{0} }} {0}", this.cmLineSeperator);
            this.PageCustomJS += sbFieldClass.ToString();
            sbDataFields.Append(" ];"); //close the js array 
            string sDataFieldObjectName = string.Concat("obj", sCleanPageDetName, "DataFields");
            //create the field array object
            this.PageCustomJS += string.Format("{0} var {1} = {2} ", this.cmLineSeperator, sDataFieldObjectName, sbDataFields.ToString());

            string sEditErrorMsg = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorMsg"), sDataSourceName);
            string sEditErrorTitle = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorTitle"), sDataSourceName);
            string sAddErrorMsg = String.Format(Utilities.getLocalizedMsg("M_AddPopupErrorMsg"), sDataSourceName);
            string sAddErrorTitle = String.Format(Utilities.getLocalizedMsg("M_AddPopupErrorTitle"), sDataSourceName);           
            string sReadyJs = string.Format("{0}{1} = new NGLEditWindCtrl(); {0}{1}.loadDefaults('{2}',{3},'{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{1}');",
                this.cmLineSeperator,
                sNGLEditWindCtrlName,
                sWidgetContainerDivID,
                sWindowObjectName,
                nWdgt.PageDetTagIDReference,
                sParentID,  
                sDataFieldObjectName,
                this.UserTheme,
                sCleanPageDetName + "CB",
                nWdgt.PageDetAPIReference,
                sobjClass,
                sDataSourceName,
                sEditErrorMsg,
                sEditErrorTitle,
                sAddErrorMsg,
                sAddErrorTitle);
            
            return sReadyJs;
        }


        /// <summary>
        /// Add an NGLSummaryDataCtrl to the page
        /// </summary>
        /// <param name="nWdgt"></param>
        /// <param name="ltsPD"></param>
        /// <param name="UserSecControl"></param>
        /// <param name="oHtml"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/25/2018
        ///   creates page reference to NGLSummaryDataCtrl html and js
        ///   adds scripts to PageReadyJS which should be included in $(document).ready(function 
        /// </remarks>
        public string createNGLSummaryDataWidget(LTS.cmPageDetail nWdgt, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref cmHTMLBuilder oHtml)
        {
            if (nWdgt == null) { return ""; } //nothing to do not configured
            System.Text.StringBuilder sbDataFields = new System.Text.StringBuilder("[ ");
            string strDataFieldsSep = "";
            if (!(nWdgt.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLSummaryDataCtrl))
            {
                return "";
            }
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);

            //Get the element fields associated with this widget
            LTS.cmElementField[] ltsEF = dalSecData.getElementFields(nWdgt.PageDetControl, UserSecControl);

            //create the data objects for the widget
            //var wdgtCompMaintEA = new NGLEditWindCtrl()
            string sCleanPageDetName = System.Text.RegularExpressions.Regex.Replace(nWdgt.PageDetName, @"\s+", "");
            string sobjClass = string.Concat("NGL", sCleanPageDetName, "Class");
            System.Text.StringBuilder sbFieldClass = new System.Text.StringBuilder();
            sbFieldClass.AppendFormat("{0}function {1}() {{", this.cmLineSeperator, sobjClass);
            string sWidgetContainerDivID = "div" + nWdgt.PageDetTagIDReference + "wrapper";
            oHtml = new cmHTMLBuilder("div", sWidgetContainerDivID, "", "", "");
            string sDataSourceName = Utilities.GetLocalizedString(nWdgt.PageDetCaption, nWdgt.PageDetCaptionLocal, this.UserCulture);
            string sNGLCtrlName = string.Concat("wdgt", sCleanPageDetName, "Summary");
            this.PageCustomJS += string.Format("{0} var {1} = new NGLSummaryDataCtrl();", this.cmLineSeperator, sNGLCtrlName);
            string sParentTagID = nWdgt.PageDetTagIDReference;
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == nWdgt.PageDetControl && x.PageDetSequenceNo < 1000).OrderBy(x => x.PageDetSequenceNo))
            {
                if (pdd != null)
                {
                    if (!Utilities.isGroupSubTypeAllowedOnNGLSummaryDataCtrl(pdd.PageDetGroupSubTypeControl))
                    {
                        continue;
                    }
                    LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                    string sCleanItemPageDetName = System.Text.RegularExpressions.Regex.Replace(pdd.PageDetName, @"\s+", "");
                    sbFieldClass.AppendFormat("{0}{1}: null; ", this.cmLineSeperator, sCleanItemPageDetName);
                    //we ignore fields with a sequence number greater than 999
                    if (ef != null && pdd.PageDetSequenceNo < 1000)
                    { addDateFieldItem(ref strDataFieldsSep, pdd, ef, ref sbDataFields, sParentTagID, sCleanItemPageDetName); }
                }
            }
            sbFieldClass.AppendFormat("{0} }} {0}", this.cmLineSeperator);
            this.PageCustomJS += sbFieldClass.ToString();
            sbDataFields.Append(" ];"); //close the js array 
            string sDataFieldObjectName = string.Concat("obj", sCleanPageDetName, "DataFields");
            //create the field array object
            this.PageCustomJS += string.Format("{0} var {1} = {2} ", this.cmLineSeperator, sDataFieldObjectName, sbDataFields.ToString());

            string sReadyJs = string.Format("{0}{1} = new NGLSummaryDataCtrl(); {0}{1}.loadDefaults('{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}'); {0} setTimeout(function () {{ {0}{1}.read(0); }}, 0, this);",
                this.cmLineSeperator,
                sNGLCtrlName,
                sWidgetContainerDivID,
                nWdgt.PageDetTagIDReference,
                sDataFieldObjectName,
                this.UserTheme,
                sCleanPageDetName + "CB",
                nWdgt.PageDetAPIReference,
                sobjClass,
                sDataSourceName);
            return sReadyJs;
        }

        /// <summary>
        /// Add an NGLEditOnPageCtrl to the page
        /// </summary>
        /// <param name="nWdgt"></param>
        /// <param name="ltsPD"></param>
        /// <param name="UserSecControl"></param>
        /// <param name="oHtml"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/25/2018
        ///   creates page reference to NGLEditOnPageCtrl html and js
        ///   adds scripts to PageReadyJS which should be included in $(document).ready(function 
        /// </remarks>
        public string createNGLEditOnPageWidget(LTS.cmPageDetail nWdgt, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref cmHTMLBuilder oHtml)
        {
            if (nWdgt == null) { return ""; } //nothing to do not configured
            System.Text.StringBuilder sbDataFields = new System.Text.StringBuilder("[ ");
            string strDataFieldsSep = "";
            if (!(nWdgt.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLEditOnPageCtrl))
            {
                return "";
            }
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);

            //Get the element fields associated with this widget
            LTS.cmElementField[] ltsEF = dalSecData.getElementFields(nWdgt.PageDetControl, UserSecControl);
            int iPK = nWdgt.PageDetElmtFieldControl;
            LTS.cmElementField pk = ltsEF.Where(x => x.ElmtFieldControl == iPK).FirstOrDefault();
            if (pk == null || pk.ElmtFieldControl == 0)
            {
                pk = ltsEF.Where(x => x.ElmtFieldPK == true).FirstOrDefault();
            }
            if (pk == null)
            {
                throw new System.InvalidOperationException("A primary key is not properly configured for this page, please check the grid content management settings or contact technical support.");
            }
            //get the primary key field name           
            string PKName = pk.ElmtFieldName;


            //create the data objects for the widget

            string sCleanPageDetName = System.Text.RegularExpressions.Regex.Replace(nWdgt.PageDetName, @"\s+", "");
            string sobjClass = string.Concat("NGL", sCleanPageDetName, "Class");
            System.Text.StringBuilder sbFieldClass = new System.Text.StringBuilder();
            sbFieldClass.AppendFormat("{0}function {1}() {{", this.cmLineSeperator, sobjClass);
            string sWidgetContainerDivID = "div" + nWdgt.PageDetTagIDReference + "wrapper";
            oHtml = new cmHTMLBuilder("div", sWidgetContainerDivID, "", "", "");
            string sDataSourceName = Utilities.GetLocalizedString(nWdgt.PageDetCaption, nWdgt.PageDetCaptionLocal, this.UserCulture);
            string sNGLCtrlName = string.Concat("wdgt", sCleanPageDetName, "Edit");
            this.PageCustomJS += string.Format("{0} var {1} = new NGLEditOnPageCtrl();", this.cmLineSeperator, sNGLCtrlName);

            //check for Delete button -- Metadata contains CRUD to determine which options are supported
            if (nWdgt.PageDetMetaData.Contains("D"))
            {
                //create the Delete Function                 
                string sFunctionName = "delete" + sCleanPageDetName + "Record";
                string sFunctionConfirmDeleteName = "confirmDelete" + sCleanPageDetName + "Record";
                this.PageCustomJS += string.Format("{0} function {1}(iRet) {{{0} if (typeof(iRet) === 'undefined' || iRet === null || iRet === 0) {{ return; }} {0}{2}.delete(); {0}}}{0}", this.cmLineSeperator, sFunctionName, sNGLCtrlName);
                this.PageCustomJS += string.Format("{0} function {1}() {{{0}ngl.OkCancelConfirmation(\"Delete Selected {2} Record\",\"Warning! This action cannot be undone.  Are you sure?\",400,400,null,\"{3}\"); {0}}}{0}", this.cmLineSeperator, sFunctionConfirmDeleteName, sDataSourceName, sFunctionName);

            }
            string sParentTagID = nWdgt.PageDetTagIDReference;
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == nWdgt.PageDetControl && x.PageDetSequenceNo < 1000).OrderBy(x => x.PageDetSequenceNo))
            {
                if (pdd != null)
                {
                    if (!Utilities.isGroupSubTypeAllowedOnNGLEditOnPageCtrl(pdd.PageDetGroupSubTypeControl))
                    {
                        continue;
                    }
                    LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                    string sCleanItemPageDetName = System.Text.RegularExpressions.Regex.Replace(pdd.PageDetName, @"\s+", "");
                    sbFieldClass.AppendFormat("{0}{1}: null; ", this.cmLineSeperator, sCleanItemPageDetName);
                    //we ignore fields with a sequence number greater than 999
                    if (pdd.PageDetSequenceNo < 1000)
                    { addDateFieldItem(ref strDataFieldsSep, pdd, ef, ref sbDataFields, sParentTagID, sCleanItemPageDetName); }
                }
            }
            sbFieldClass.AppendFormat("{0} }} {0}", this.cmLineSeperator);
            this.PageCustomJS += sbFieldClass.ToString();
            sbDataFields.Append(" ];"); //close the js array 
            string sDataFieldObjectName = string.Concat("obj", sCleanPageDetName, "DataFields");
            //create the field array object
            this.PageCustomJS += string.Format("{0} var {1} = {2} ", this.cmLineSeperator, sDataFieldObjectName, sbDataFields.ToString());

            string sEditErrorMsg = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorMsg"), sDataSourceName);
            string sEditErrorTitle = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorTitle"), sDataSourceName);
            string sReadyJs = string.Format("{0}{1} = new NGLEditOnPageCtrl(); {0}{1}.loadDefaults('{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{1}'); {0} setTimeout(function () {{ {0}{1}.read(0); }}, 0, this); ",
                this.cmLineSeperator,
                sNGLCtrlName,
                sWidgetContainerDivID,
                nWdgt.PageDetTagIDReference,
                sDataFieldObjectName,
                this.UserTheme,
                sCleanPageDetName + "CB",
                nWdgt.PageDetAPIReference,
                sobjClass,
                sDataSourceName,
                sEditErrorMsg,
                sEditErrorTitle,
                PKName);
            return sReadyJs;
        }

        /// <summary>
        /// Add an NGLWorkFlowOptionCtrl to the page linked to the parent NGLPopupWindCtrl 
        /// </summary>
        /// <param name="nWdgt"></param>
        /// <param name="ltsPD"></param>
        /// <param name="UserSecControl"></param>
        /// <param name="oHtml"></param>
        /// <param name="sParentID"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/18/2018
        ///   creates page reference to NGLWorkFlowOptionCtrl html and js
        ///   adds scripts to PageReadyJS which should be included in $(document).ready(function 
        ///   this control drills down into dependent records in cmPageDetail adding them sequentially to the 
        ///   Data fields with dependent parent ID and sequence numbers
        /// </remarks>
        public string createNGLWorkFlowOptionCtrl(LTS.cmPageDetail nWdgt, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref List<cmHTMLBuilder> oHtmls, string sParentID = "")
        {
            if (nWdgt == null) { return ""; } //nothing to do not configured
            System.Text.StringBuilder sbDataFields = new System.Text.StringBuilder("[ ");
            string strDataFieldsSep = "";
            if (!(nWdgt.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLWorkFlowOptionCtrl))
            {
                return "";
            }
            if (string.IsNullOrWhiteSpace(sParentID)) { sParentID = nWdgt.PageDetTagIDReference; }
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            //Get the element fields associated with this widget
            LTS.cmElementField[] ltsEF = dalSecData.getElementFields(nWdgt.PageDetControl, UserSecControl);

            string sCleanPageDetName = System.Text.RegularExpressions.Regex.Replace(nWdgt.PageDetName, @"\s+", "");
            string sobjClass = string.Concat("NGL", sCleanPageDetName, "Class");
            System.Text.StringBuilder sbFieldClass = new System.Text.StringBuilder();
            sbFieldClass.AppendFormat("{0}function {1}() {{", this.cmLineSeperator, sobjClass);
            string sWidgetContainerDivID = "div" + nWdgt.PageDetTagIDReference + "wrapper";
            oHtmls.Add(new cmHTMLBuilder("div", sWidgetContainerDivID, "", "", ""));
            string sDataSourceName = Utilities.GetLocalizedString(nWdgt.PageDetCaption, nWdgt.PageDetCaptionLocal, this.UserCulture);
            string sNGLCtrlName = string.Concat("wdgt", sCleanPageDetName, "Edit");
            this.PageCustomJS += string.Format("{0} var {1} = new NGLWorkFlowOptionCtrl();", this.cmLineSeperator, sNGLCtrlName);

            string sParentTagID = nWdgt.PageDetTagIDReference;
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == nWdgt.PageDetControl && x.PageDetSequenceNo < 1000).OrderBy(x => x.PageDetSequenceNo))
            {

                //we need to get all the items at this level checking for child records then drill down into each list of child records
                //we only allow group headers at the top, but there can be multiple switches, each header or switch can have flow sections
                //but only headers can have more switches
                //so add each PageDetControl to a list add the data to the sbDataFields string  then check for children
                if (pdd != null)
                {
                    if (!Utilities.isGroupSubTypeAllowedOnNGLWorkFlowOptionCtrl(pdd.PageDetGroupSubTypeControl))
                    {
                        continue;
                    }
                    LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                    string sCleanItemPageDetName = System.Text.RegularExpressions.Regex.Replace(pdd.PageDetName, @"\s+", "");
                    sbFieldClass.AppendFormat("{0}{1}: null; ", this.cmLineSeperator, sCleanItemPageDetName);
                    //we ignore fields with a sequence number greater than 999
                    if (pdd.PageDetSequenceNo < 1000)
                    {
                        addDateFieldItem(ref strDataFieldsSep, pdd, ef, ref sbDataFields, sParentTagID, sCleanItemPageDetName);
                        int ipddControl = pdd.PageDetControl;
                        string scParentTagID = pdd.PageDetTagIDReference;
                        // check for children
                        foreach (LTS.cmPageDetail cpdd in ltsPD.Where(x => x.PageDetParentID == ipddControl && x.PageDetSequenceNo < 1000).OrderBy(x => x.PageDetSequenceNo))
                        {
                            if (cpdd != null)
                            {
                                switch (pdd.PageDetGroupSubTypeControl)
                                {
                                    case (int)Utilities.GroupSubType.NGLWorkFlowGroup:
                                        if (!Utilities.isGroupSubTypeAllowedOnNGLWorkFlowGroup(cpdd.PageDetGroupSubTypeControl))
                                        {
                                            continue;
                                        }
                                        break;
                                    case (int)Utilities.GroupSubType.NGLWorkFlowOnOffSwitch:
                                        if (!Utilities.isGroupSubTypeAllowedOnNGLWorkFlowOnOffSwitch(cpdd.PageDetGroupSubTypeControl))
                                        {
                                            continue;
                                        }
                                        break;
                                    case (int)Utilities.GroupSubType.NGLWorkFlowYesNoSwitch:
                                        if (!Utilities.isGroupSubTypeAllowedOnNGLWorkFlowYesNoSwitch(cpdd.PageDetGroupSubTypeControl))
                                        {
                                            continue;
                                        }
                                        break;
                                    default:
                                        continue;
                                }
                                LTS.cmElementField cef = (ltsEF.Where(x => x.ElmtFieldControl == cpdd.PageDetElmtFieldControl)).FirstOrDefault();
                                string scCleanItemPageDetName = System.Text.RegularExpressions.Regex.Replace(cpdd.PageDetName, @"\s+", "");
                                sbFieldClass.AppendFormat("{0}{1}: null; ", this.cmLineSeperator, scCleanItemPageDetName);
                                //we ignore fields with a sequence number greater than 999
                                if (cpdd.PageDetSequenceNo < 1000)
                                {
                                    string sCRUDTagID = "";
                                    //add code here to create any NGLWorkFlowSectionCtrls
                                    if (cpdd.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLWorkFlowSectionCtrl)
                                    {
                                        sCRUDTagID = cpdd.PageDetTagIDReference;
                                        string sItemFieldCRUDTagID = "";
                                        cmHTMLBuilder[] cHTMLs = processPageDetailsSubTypes(cpdd, ltsPD, UserSecControl, ref sItemFieldCRUDTagID, sParentID);
                                        //if we needed the childs CRUD Tag ID we would use it here
                                        if (cHTMLs != null && cHTMLs.Count() > 0)
                                        {
                                            oHtmls.AddRange(cHTMLs);
                                        }
                                    }
                                    else if (
                                          cpdd.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLWorkFlowOnOffSwitch
                                          ||
                                          cpdd.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLWorkFlowYesNoSwitch)
                                    {
                                        //this child can still have more nested children
                                        int icpddControl = cpdd.PageDetControl;
                                        string xParentTagID = cpdd.PageDetTagIDReference;
                                        // check for children
                                        foreach (LTS.cmPageDetail xpdd in ltsPD.Where(x => x.PageDetParentID == icpddControl && x.PageDetSequenceNo < 1000).OrderBy(x => x.PageDetSequenceNo))
                                        {
                                            if (xpdd != null)
                                            {
                                                switch (xpdd.PageDetGroupSubTypeControl)
                                                {
                                                    case (int)Utilities.GroupSubType.NGLWorkFlowOnOffSwitch:
                                                        if (!Utilities.isGroupSubTypeAllowedOnNGLWorkFlowOnOffSwitch(cpdd.PageDetGroupSubTypeControl))
                                                        {
                                                            continue;
                                                        }
                                                        break;
                                                    case (int)Utilities.GroupSubType.NGLWorkFlowYesNoSwitch:
                                                        if (!Utilities.isGroupSubTypeAllowedOnNGLWorkFlowYesNoSwitch(cpdd.PageDetGroupSubTypeControl))
                                                        {
                                                            continue;
                                                        }
                                                        break;
                                                    default:
                                                        continue;
                                                }
                                                LTS.cmElementField xef = (ltsEF.Where(x => x.ElmtFieldControl == xpdd.PageDetElmtFieldControl)).FirstOrDefault();
                                                string sxCleanItemPageDetName = System.Text.RegularExpressions.Regex.Replace(xpdd.PageDetName, @"\s+", "");
                                                //add code here to create any NGLWorkFlowSectionCtrls
                                                sbFieldClass.AppendFormat("{0}{1}: null; ", this.cmLineSeperator, sxCleanItemPageDetName);
                                                //we ignore fields with a sequence number greater than 999
                                                string sxCRUDTagID = "";
                                                if (xpdd.PageDetSequenceNo < 1000)
                                                {
                                                    if (xpdd.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLWorkFlowSectionCtrl)
                                                    {
                                                        sxCRUDTagID = xpdd.PageDetTagIDReference;
                                                        string sxItemFieldCRUDTagID = "";
                                                        cmHTMLBuilder[] cHTMLs = processPageDetailsSubTypes(xpdd, ltsPD, UserSecControl, ref sxItemFieldCRUDTagID, sParentID);
                                                        //if we needed the childs CRUD Tag ID we would use it here
                                                        if (cHTMLs != null && cHTMLs.Count() > 0)
                                                        {
                                                            oHtmls.AddRange(cHTMLs);
                                                        }
                                                    }
                                                    addDateFieldItem(ref strDataFieldsSep, xpdd, xef, ref sbDataFields, xParentTagID, sxCleanItemPageDetName, sxCRUDTagID);
                                                }
                                            }
                                        }
                                    }
                                    addDateFieldItem(ref strDataFieldsSep, cpdd, cef, ref sbDataFields, scParentTagID, scCleanItemPageDetName, sCRUDTagID);
                                }
                            }
                        }
                    }
                }
            }

            sbFieldClass.AppendFormat("{0} }} {0}", this.cmLineSeperator);
            this.PageCustomJS += sbFieldClass.ToString();
            sbDataFields.Append(" ];"); //close the js array 
            string sDataFieldObjectName = string.Concat("obj", sCleanPageDetName, "DataFields");
            //create the field array object
            this.PageCustomJS += string.Format("{0} var {1} = {2} ", this.cmLineSeperator, sDataFieldObjectName, sbDataFields.ToString());

            string sEditErrorMsg = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorMsg"), sDataSourceName);
            string sEditErrorTitle = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorTitle"), sDataSourceName);
            string sReadyJs = string.Format("{0}{1} = new NGLWorkFlowOptionCtrl(); {0}{1}.loadDefaults('{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{1}');",
                this.cmLineSeperator,
                sNGLCtrlName,
                sWidgetContainerDivID,          //Container Div ID on parent
                nWdgt.PageDetTagIDReference,   //control IDKey
                sParentID,                     // Parent Object ID typically the window
                sDataFieldObjectName,          // fieldData
                this.UserTheme,                // user Theme
                sCleanPageDetName + "CB",      // Call back function on page
                nWdgt.PageDetAPIReference,     // API Reference for CRUD (typically just Read and Update for user settings 
                sobjClass,                     // DataType for this control not normally used but available for future class name is defined using widget.pagedetname 
                sDataSourceName,               // Caption of control
                sEditErrorMsg,                 // localized messge for save errors 
                sEditErrorTitle);              // localized message save error title

            return sReadyJs;
        }

        /// <summary>
        /// Add an NGLWorkFlowSectionCtrl to the page linked to the parent NGLPopupWindCtrl part of the NGLWorkFlowOptionCtrl 
        /// </summary>
        /// <param name="nWdgt"></param>
        /// <param name="ltsPD"></param>
        /// <param name="UserSecControl"></param>
        /// <param name="oHtmls"></param>
        /// <param name="sParentID"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/22/2018
        /// </remarks>
        public string createNGLWorkFlowSectionCtrl(LTS.cmPageDetail nWdgt, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref List<cmHTMLBuilder> oHtmls, string sParentID = "")
        {
            System.Text.StringBuilder sbDataFields = new System.Text.StringBuilder("[ ");
            string strDataFieldsSep = "";
            if (!(nWdgt.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLWorkFlowSectionCtrl))
            {
                return "";
            }
            if (string.IsNullOrWhiteSpace(sParentID)) { sParentID = nWdgt.PageDetTagIDReference; }
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);

            //Get the element fields associated with this widget
            LTS.cmElementField[] ltsEF = dalSecData.getElementFields(nWdgt.PageDetControl, UserSecControl);

            //create the data objects for the widget
            //var wdgtCompMaintEA = new NGLEditWindCtrl()
            string sCleanPageDetName = System.Text.RegularExpressions.Regex.Replace(nWdgt.PageDetName, @"\s+", "");
            string sobjClass = string.Concat("NGL", sCleanPageDetName, "Class");
            System.Text.StringBuilder sbFieldClass = new System.Text.StringBuilder();
            sbFieldClass.AppendFormat("{0}function {1}() {{", this.cmLineSeperator, sobjClass);
            string sWidgetContainerDivID = "div" + nWdgt.PageDetTagIDReference + "wrapper";
            oHtmls.Add(new cmHTMLBuilder("div", sWidgetContainerDivID, "", "", ""));
            string sDataSourceName = Utilities.GetLocalizedString(nWdgt.PageDetCaption, nWdgt.PageDetCaptionLocal, this.UserCulture);
            string sNGLCtrlName = string.Concat("wdgt", sCleanPageDetName, "Section");
            this.PageCustomJS += string.Format("{0} var {1} = new NGLWorkFlowSectionCtrl();", this.cmLineSeperator, sNGLCtrlName);
            string sParentTagID = nWdgt.PageDetTagIDReference;
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == nWdgt.PageDetControl && x.PageDetSequenceNo < 1000).OrderBy(x => x.PageDetSequenceNo))
            {
                if (pdd != null)
                {
                    if (!Utilities.isGroupSubTypeAllowedOnNGLWorkFlowSectionCtrl(pdd.PageDetGroupSubTypeControl))
                    {
                        continue;
                    }
                    LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                    string sCleanItemPageDetName = System.Text.RegularExpressions.Regex.Replace(pdd.PageDetName, @"\s+", "");
                    sbFieldClass.AppendFormat("{0}{1}: null; ", this.cmLineSeperator, sCleanItemPageDetName);
                    //we ignore fields with a sequence number greater than 999
                    //if (ef != null && pdd.PageDetSequenceNo < 1000)
                    if (pdd.PageDetSequenceNo < 1000)
                    {
                        addDateFieldItem(ref strDataFieldsSep, pdd, ef, ref sbDataFields, sParentTagID, sCleanItemPageDetName);
                    }
                }
            }
            sbFieldClass.AppendFormat("{0} }} {0}", this.cmLineSeperator);
            this.PageCustomJS += sbFieldClass.ToString();
            sbDataFields.Append(" ];"); //close the js array 
            string sDataFieldObjectName = string.Concat("obj", sCleanPageDetName, "DataFields");
            //create the field array object
            this.PageCustomJS += string.Format("{0} var {1} = {2} ", this.cmLineSeperator, sDataFieldObjectName, sbDataFields.ToString());

            string sEditErrorMsg = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorMsg"), sDataSourceName);
            string sEditErrorTitle = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorTitle"), sDataSourceName);
            string sReadyJs = string.Format("{0}{1} = new NGLWorkFlowSectionCtrl(); {0}{1}.loadDefaults('{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{1}');",
                this.cmLineSeperator,
                sNGLCtrlName,
                 sWidgetContainerDivID,          //Container Div ID on parent
                nWdgt.PageDetTagIDReference,   //control IDKey
                sParentID,                     // Parent Object ID typically the window
                sDataFieldObjectName,          // fieldData
                this.UserTheme,                // user Theme
                sCleanPageDetName + "CB",      // Call back function on page
                nWdgt.PageDetAPIReference,     // API Reference for CRUD (typically just Read and Update for user settings 
                sobjClass,                     // DataType for this control not normally used but available for future class name is defined using widget.pagedetname 
                sDataSourceName,               // Caption of control
                sEditErrorMsg,                 // localized messge for save errors 
                sEditErrorTitle);              // localized message save error title
            return sReadyJs;
        }

        public string createNGLPopupWindCtrl(LTS.cmPageDetail nWdgt, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref List<cmHTMLBuilder> oHtmls, string sParentID = "")
        {
            if (nWdgt == null) { return ""; } //nothing to do not configured
            System.Text.StringBuilder sbDataFields = new System.Text.StringBuilder("[ ");
            string strDataFieldsSep = "";
            if (!(nWdgt.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLPopupWindCtrl))
            {
                return "";
            }
            if (string.IsNullOrWhiteSpace(sParentID)) { sParentID = nWdgt.PageDetTagIDReference; }
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            //Get the element fields associated with this widget
            LTS.cmElementField[] ltsEF = dalSecData.getElementFields(nWdgt.PageDetControl, UserSecControl);

            string sCleanPageDetName = System.Text.RegularExpressions.Regex.Replace(nWdgt.PageDetName, @"\s+", "");
            string sobjClass = string.Concat("NGL", sCleanPageDetName, "Class");
            System.Text.StringBuilder sbFieldClass = new System.Text.StringBuilder();
            sbFieldClass.AppendFormat("{0}function {1}() {{", this.cmLineSeperator, sobjClass);
            string sWidgetContainerDivID = "div" + nWdgt.PageDetTagIDReference;
            string sDataSourceName = Utilities.GetLocalizedString(nWdgt.PageDetCaption, nWdgt.PageDetCaptionLocal, this.UserCulture);
            string sWindowObjectName = string.Concat("wnd", sCleanPageDetName, "Dialog");
            string sNGLCtrlName = string.Concat("wdgt", sCleanPageDetName, "Dialog");
            this.PageCustomJS += string.Format("{0} var {1} = kendo.ui.Window; {0} var {2} = new NGLPopupWindCtrl();", this.cmLineSeperator, sWindowObjectName, sNGLCtrlName);
            cmHTMLBuilder oHtml = new cmHTMLBuilder("div", sWidgetContainerDivID, "", "", "");

            string sParentTagID = nWdgt.PageDetTagIDReference;
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == nWdgt.PageDetControl && x.PageDetSequenceNo < 1000).OrderBy(x => x.PageDetSequenceNo))
            {

                if (pdd != null)
                {
                    if (!Utilities.isGroupSubTypeAllowedOnNGLPopupWindCtrl(pdd.PageDetGroupSubTypeControl))
                    {
                        continue;
                    }
                    LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                    string sCleanItemPageDetName = System.Text.RegularExpressions.Regex.Replace(pdd.PageDetName, @"\s+", "");
                    sbFieldClass.AppendFormat("{0}{1}: null; ", this.cmLineSeperator, sCleanItemPageDetName);
                    //we ignore fields with a sequence number greater than 999
                    if (pdd.PageDetSequenceNo < 1000)
                    {
                        string sFieldCRUDTagID = "";
                        if (Utilities.isGroupSubTypeAContainer(pdd.PageDetGroupSubTypeControl))
                        {

                            //create a temporary wrapper for this control
                            //string sTmpChildContainerDivID = "tmp" + pdd.PageDetTagIDReference + "wrapper";
                            //oHtml.addNestedHTML(new cmHTMLBuilder("div", sTmpChildContainerDivID, "", "display:none;", ""));
                            //cmHTMLBuilder oHTMLTMP = new cmHTMLBuilder("div", sTmpChildContainerDivID, "", "display:none;", "");
                            cmHTMLBuilder[] cHTMLs = processPageDetailsSubTypes(pdd, ltsPD, UserSecControl, ref sFieldCRUDTagID, sParentTagID, false);

                            if (cHTMLs != null && cHTMLs.Count() > 0)
                            {
                                string sTmpChildContainerDivID = "tmp" + pdd.PageDetTagIDReference + "wrapper";
                                cmHTMLBuilder oHTMLTMP = new cmHTMLBuilder("div", sTmpChildContainerDivID, "", "display:none;", "");
                                foreach (cmHTMLBuilder h in cHTMLs)
                                {
                                    oHTMLTMP.addNestedHTML(h);
                                }
                                oHtml.addNestedHTML(oHTMLTMP);
                            }
                        }
                        addDateFieldItem(ref strDataFieldsSep, pdd, ef, ref sbDataFields, sParentTagID, sCleanItemPageDetName, sFieldCRUDTagID);
                    }
                }
            }
            oHtmls.Add(oHtml);
            sbFieldClass.AppendFormat("{0} }} {0}", this.cmLineSeperator);
            this.PageCustomJS += sbFieldClass.ToString();
            sbDataFields.Append(" ];"); //close the js array 
            string sDataFieldObjectName = string.Concat("obj", sCleanPageDetName, "DataFields");
            //create the field array object
            this.PageCustomJS += string.Format("{0} var {1} = {2} ", this.cmLineSeperator, sDataFieldObjectName, sbDataFields.ToString());

            string sEditErrorMsg = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorMsg"), sDataSourceName);
            string sEditErrorTitle = String.Format(Utilities.getLocalizedMsg("M_EditPopupErrorTitle"), sDataSourceName);
            string sAddErrorMsg = String.Format(Utilities.getLocalizedMsg("M_AddPopupErrorMsg"), sDataSourceName);
            string sAddErrorTitle = String.Format(Utilities.getLocalizedMsg("M_AddPopupErrorTitle"), sDataSourceName);
            string sReadyJs = string.Format("{0}{1} = new NGLPopupWindCtrl(); {0}{1}.loadDefaults('{2}',{3},'{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{1}');",
                this.cmLineSeperator,
                sNGLCtrlName,
                sWidgetContainerDivID,
                sWindowObjectName,
                nWdgt.PageDetTagIDReference,
                sParentID,
                sDataFieldObjectName,
                this.UserTheme,
                sCleanPageDetName + "CB",
                nWdgt.PageDetAPIReference,
                sobjClass,
                sDataSourceName,
                sEditErrorMsg,
                sEditErrorTitle,
                sAddErrorMsg,
                sAddErrorTitle);
            return sReadyJs;
        }

        public string createNGLErrWarnMsgLogCtrl(LTS.cmPageDetail nWdgt, LTS.cmPageDetail[] ltsPD, int UserSecControl, ref List<cmHTMLBuilder> oHtmls, string sParentID = "")
        {
            if (nWdgt == null) { return ""; } //nothing to do not configured

            if (!(nWdgt.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.NGLErrWarnMsgLogCtrl))
            {
                return "";
            }
            if (string.IsNullOrWhiteSpace(sParentID)) { sParentID = nWdgt.PageDetTagIDReference; }
            string sCleanPageDetName = System.Text.RegularExpressions.Regex.Replace(nWdgt.PageDetName, @"\s+", "");
            string sWidgetContainerDivID = "div" + nWdgt.PageDetTagIDReference;  //Container will be the popup window control
            string sDataSourceName = Utilities.GetLocalizedString(nWdgt.PageDetCaption, nWdgt.PageDetCaptionLocal, this.UserCulture);
            string sWindowObjectName = string.Concat("wnd", sCleanPageDetName, "Dialog");
            string sNGLCtrlName = string.Concat("wdgt", sCleanPageDetName, "Dialog");
            this.PageCustomJS += string.Format("{0} var {1} = kendo.ui.Window; {0} var {2} = new NGLErrWarnMsgLogCtrl();", this.cmLineSeperator, sWindowObjectName, sNGLCtrlName);

            System.Text.StringBuilder sbWinInnerHTML = new System.Text.StringBuilder("");
            sbWinInnerHTML.AppendFormat("<div id=\"div{0}ErWaMsLgTab\"><ul>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.AppendFormat("<li id=\"li{0}logTab\"><sp id=\"sp{0}logTitle\"></sp></li>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.AppendFormat("<li id=\"li{0}msgTab\"><sp id=\"sp{0}msgTitle\"></sp></li>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.AppendFormat("<li id=\"li{0}warnTab\"><sp id=\"sp{0}warnTitle\"></sp></li>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.AppendFormat("<li id=\"li{0}errTab\"><sp id=\"sp{0}errTitle\"></sp></li></ul>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.AppendFormat("<div><div id=\"div{0}logGrid\"></div></div>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.AppendFormat("<div><div id=\"div{0}msgGrid\"></div></div>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.AppendFormat("<div><div id=\"div{0}warnGrid\"></div></div>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.AppendFormat("<div><div id=\"div{0}errGrid\"></div></div>", nWdgt.PageDetTagIDReference);
            sbWinInnerHTML.Append("</div>");
            cmHTMLBuilder oHtml = new cmHTMLBuilder("div", sWidgetContainerDivID, "", "display: none;", sbWinInnerHTML.ToString());
            this.PageCustomJS = this.PageCustomJS + "\n\r var errWarnData = null;\n\r   function openNGLErrWarnMsgLogCtrlDialog(){ \n\r  if (typeof (errWarnData) !== 'undefined' && ngl.isObject(errWarnData)) {  \n\r if (typeof(wdgtNGLErrWarnMsgLogCtrlDialog) !== 'undefined' && wdgtNGLErrWarnMsgLogCtrlDialog != null)  { \n\r console.log(errWarnData); wdgtNGLErrWarnMsgLogCtrlDialog.show(errWarnData);  \n\r errWarnData = null;  } else {\n\r ngl.showErrMsg('Messages', 'No messages are availble', null);}  } else {\n\r ngl.showErrMsg('Messages', 'No messages are availble', null);}}";
            string sParentTagID = nWdgt.PageDetTagIDReference;
            oHtmls.Add(oHtml);
            string sReadyJs = string.Format("{0}{1} = new NGLErrWarnMsgLogCtrl(); {0}{1}.loadDefaults('{2}',{3},'{4}','{5}','{6}','{1}');",
                this.cmLineSeperator,
                sNGLCtrlName,
                sWidgetContainerDivID,  //Container will be the popup window control
                sWindowObjectName,
                nWdgt.PageDetTagIDReference,
                sParentID,
                this.UserTheme);
            return sReadyJs;
        }


        private void addDateFieldItem(ref string strDataFieldsSep, LTS.cmPageDetail pdd, LTS.cmElementField ef, ref System.Text.StringBuilder sbDataFields, string sParentTagID, string sCleanItemPageDetName, string sFieldCRUDTagID = "")
        {
            sbDataFields.AppendFormat("{0}{1}{{ fieldID: \"{2}\"", this.cmLineSeperator, strDataFieldsSep, pdd.PageDetControl);
            sbDataFields.AppendFormat(",fieldTagID: \"{0}\"", pdd.PageDetTagIDReference); //DOM ID PageDetTagIDReference
            sbDataFields.AppendFormat(",fieldCaption: \"{0}\"", Utilities.GetLocalizedString(pdd.PageDetCaption, pdd.PageDetCaptionLocal, this.UserCulture));  //Dispaly caption cmPageDetail.(localize PageDetCaptionLocal)
            sbDataFields.AppendFormat(",fieldName: \"{0}\"", sCleanItemPageDetName); //name of field cleaned version of cmPageDetail.PageDetName
            sbDataFields.AppendFormat(",fieldDefaultValue: \"{0}\"", pdd.PageDetMetaData); //default value for insert cmPageDetail.PageDetMetaData
            sbDataFields.AppendFormat(",fieldGroupSubType: {0}", pdd.PageDetGroupSubTypeControl); //value to identify the data type associated with the nglGroupSubTypeEnum
            sbDataFields.AppendFormat(",fieldReadOnly: {0}", pdd.PageDetReadOnly.ToString().ToLower()); //do we allow updates PageDetReadOnly
            if (ef != null)
            {
                string sFormat = Utilities.cleanFormatOrTemplateData(pdd.PageDetFieldFormatOverride, ef.ElmtFieldFormat);
                if (string.IsNullOrWhiteSpace(sFormat)) { sFormat = "\"\""; } //we do this because cleanFormatOrTemplateData is used by other procedures and can return an empty string
                sbDataFields.AppendFormat(",fieldFormat: {0}", sFormat); //optional input mask for kendo control cmElementField.ElmtFieldFormat or new cmPageDetail.PageDetFieldFormatOverride
                string sTemplate = Utilities.cleanFormatOrTemplateData(pdd.PageDetFieldTemplateOverride, ef.ElmtFieldTemplate);
                if (string.IsNullOrWhiteSpace(sTemplate))
                {
                    //we do this because cleanFormatOrTemplateData is used by other procedures and can return an empty string
                    sTemplate = "\"\"";
                }
                else if (sTemplate.Length > 3 && sTemplate.Substring(0, 3).ToLower() == "fun")
                {
                    //cleanFormatOrTemplateData cannot encapsulate sTemplate in quotes for functions because 
                    //  they cannot exist for the grid, but quotes are neede for the widget template strings
                    sTemplate = "\"" + sTemplate + "\"";
                }
                sbDataFields.AppendFormat(",fieldTemplate: {0}", sTemplate); //optional template layout for kendo control cmElementField.ElmtFieldTemplate or new cmPageDetail.PageDetFieldTemplateOverride
                sbDataFields.AppendFormat(",fieldAllowNull: {0}", ef.ElmtFieldAllowNull.ToString().ToLower()); //allow blank or null values  cmElementField.ElmtFieldAllowNull
                sbDataFields.AppendFormat(",fieldMaxlength: {0}", calculateFieldMaxLength(ef)); //maximum size of text fields as string  cmElementField.ElmtFieldMaxLength
            }
            else
            {
                string sFormat = Utilities.cleanFormatOrTemplateData(pdd.PageDetFieldFormatOverride, "");
                if (string.IsNullOrWhiteSpace(sFormat)) { sFormat = "\"\""; } //we do this because cleanFormatOrTemplateData is used by other procedures and can return an empty string
                sbDataFields.AppendFormat(",fieldFormat: {0}", sFormat); //optional input mask for kendo control cmElementField.ElmtFieldFormat or new cmPageDetail.PageDetFieldFormatOverride
                string sTemplate = Utilities.cleanFormatOrTemplateData(pdd.PageDetFieldTemplateOverride, "");
                if (string.IsNullOrWhiteSpace(sTemplate))
                {
                    //we do this because cleanFormatOrTemplateData is used by other procedures and can return an empty string
                    sTemplate = "\"\"";
                }
                else if (sTemplate.Length > 3 && sTemplate.Substring(0, 3).ToLower() == "fun")
                {
                    //cleanFormatOrTemplateData cannot encapsulate sTemplate in quotes for functions because 
                    //  they cannot exist for the grid, but quotes are neede for the widget template strings
                    sTemplate = "\"" + sTemplate + "\"";
                }

                sbDataFields.AppendFormat(",fieldTemplate: {0}", sTemplate); //optional template layout for kendo control cmElementField.ElmtFieldTemplate or new cmPageDetail.PageDetFieldTemplateOverride
                sbDataFields.Append(",fieldAllowNull: true");
                sbDataFields.Append(",fieldMaxlength: 50");
            }
            sbDataFields.AppendFormat(",fieldVisible: {0}", pdd.PageDetVisible.ToString().ToLower()); //flag to show or hide field  PageDetVisible
            sbDataFields.AppendFormat(",fieldRequired: {0}", pdd.PageDetRequired.ToString().ToLower()); //flag to require update PageDetRequired
            sbDataFields.AppendFormat(",fieldInsertOnly: {0}", pdd.PageDetInsertOnly.ToString().ToLower()); // allow updates on insert only PageDetInsertOnly
            sbDataFields.AppendFormat(",fieldAPIReference: \"{0}\"", pdd.PageDetAPIReference); // reference to vlookuplist api 
            sbDataFields.AppendFormat(",fieldAPIFilterID: \"{0}\"", pdd.PageDetAPIFilterID); // reference to key enum for vlookuplist
            sbDataFields.AppendFormat(",fieldParentTagID: \"{0}\"", sParentTagID); // reference to pageDetailParentID
            sbDataFields.AppendFormat(",fieldSequenceNo: {0}", pdd.PageDetSequenceNo.ToString()); // reference to pageDetailSequenceNo
            sbDataFields.AppendFormat(",fieldCRUDTagID: \"{0}\"", sFieldCRUDTagID); // Modified by RHR for v-8.2 tag id for the widget used on CRUD operations if different than fieldTagID, and example is when a fast tab contains a grid, the grid's tag id will be used to read the data; typically used on nested grids in popup windows
            sbDataFields.AppendFormat(",fieldWidgetAction: \"{0}\"", pdd.PageDetWidgetAction); // Modified by RHR for v-8.2 action to be executed by the widget's executeAction method 
            sbDataFields.AppendFormat(",fieldWidgetActionKey: \"{0}\"", pdd.PageDetWidgetActionKey); // Modified by RHR for v-8.2  optional key used by the widget's executeAction method to determine when to trigger an action
            // Modified by RHR for v-8.5.4.001 on 05/28/2023 added logic to convert css for k-button
            if (pdd.PageDetCSSClass?.Trim() == "k-button") { 
                pdd.PageDetCSSClass = "k-button k-button-solid-base k-button-solid k-button-md k-rounded-md"; }
            sbDataFields.AppendFormat(",fieldCssClass: \"{0}\"", pdd.PageDetCSSClass); // Modified by RHR for v-8.2  
            sbDataFields.AppendFormat(",fieldStyle: \"{0}\"", pdd.PageDetAttributes); // Modified by RHR for v-8.2 
            sbDataFields.AppendFormat(",fieldEditWndVisibility: {0}", pdd.PageDetEditWndVisibility.ToString()); // Modified by RHR for v-8.2.1.100 on 10/4/2019  
            sbDataFields.AppendFormat(",fieldEditWndSeqNo: {0}", pdd.PageDetEditWndSeqNo.ToString()); // Modified by RHR for v-8.2.1.100 on 10/4/2019  
            sbDataFields.AppendFormat(",fieldAddWndVisibility: {0}", pdd.PageDetAddWndVisibility.ToString()); // Modified by RHR for v-8.2.1.100 on 10/4/2019  
            sbDataFields.AppendFormat(",fieldAddWndSeqNo: {0}", pdd.PageDetAddWndSeqNo.ToString()); // Modified by RHR for v-8.2.1.100 on 10/4/2019  
            sbDataFields.AppendFormat(",fieldNGLToolTip: \"{0}\"", pdd.PageDetDesc); // Modified by RHR for v-8.5.3.007 on 03/14/2023 map Description to tool tip

            sbDataFields.AppendFormat("}} {0}", this.cmLineSeperator);
            strDataFieldsSep = ",";
        }

        private int calculateFieldMaxLength(LTS.cmElementField ef)
        {
            int iRet = 50; //defalut value
            if (ef.ElmtFieldDataTypeControl == 17) //(nchar) maxlength = 1
            {
                if (ef.ElmtFieldMaxLength > 2)
                {
                    iRet = (int)Math.Ceiling((double)ef.ElmtFieldMaxLength / 2);
                }
                else
                {
                    iRet = 1;
                }
            }
            else if (ef.ElmtFieldDataTypeControl == 18) //(ntext)  maxlength = 4000
            {
                iRet = 4000;
            }
            else if (ef.ElmtFieldDataTypeControl == 20) //(nvarchar)  maxlength = Max Length / 2
            {
                if (ef.ElmtFieldMaxLength > 2)
                {
                    iRet = (int)Math.Ceiling((double)ef.ElmtFieldMaxLength / 2);
                }
                else
                {
                    iRet = 1;
                }
            }
            else if (ef.ElmtFieldDataTypeControl == 27) //(text)  maxlength = 4000
            {
                iRet = 4000;
            }
            else if (ef.ElmtFieldDataTypeControl == 33) //(varchar)  maxlength = Max Length
            {
                iRet = ef.ElmtFieldMaxLength;
            }

            return iRet;
        }

        private string createKendoGridButton(string sCommandsSep, string sName, string sCaption, string sCaptionLocal, string sClassName, string sAttributes, string sClick)
        {
            string sRet = "";

            if (!string.IsNullOrWhiteSpace(sClassName))
            {
                sClassName = "className: \"" + sClassName + "\",";
            }

            if (!String.IsNullOrWhiteSpace(sAttributes))
            {
                sAttributes = ", iconClass: \"" + sAttributes + "\"";
            }


            if (!String.IsNullOrWhiteSpace(sCaption))
            {
                sCaption = Utilities.GetLocalizedString(sCaption, sCaptionLocal, UserCulture);
            }

            sRet = string.Format("{0}{1}{{{2} name: \"{3}\" , text: \"{4}\" {5} , click:  {6}  }}", sCommandsSep, cmLineSeperator, sClassName, sName, sCaption, sAttributes, sClick);

            return sRet;
        }

        
        /// <summary>
        /// Build Tab Strip Data
        /// </summary>
        /// <param name="pdd"></param>
        /// <param name="ltsPD"></param>
        /// <param name="ltsEF"></param>
        /// <param name="UserSecControl"></param>
        /// <param name="oHtml"></param>
        /// <param name="sParentID"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR on 08/12/2020 for v-8.3.0.001 
        ///     added logic to support new method to read saved filters 
        ///     supports common logic for filters and new grid group by logic       
        /// </remarks>
        public string createkendoTapStripDataSource(LTS.cmPageDetail pdd, LTS.cmPageDetail[] ltsPD, LTS.cmElementField[] ltsEF, int UserSecControl, ref cmHTMLBuilder oHtml, string sParentID  = "")
        {
            if (pdd.PageDetPageTemplateControl == 0) { return "\n\r // missing template control for tab strip \n\r "; }
            string sTemplateName = AllTemplates.Where(x => x.PageTemplateControl == pdd.PageDetPageTemplateControl).Select(x => x.PageTemplateName).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(sTemplateName)) { return "\n\r // invalid template control for tab strip \n\r "; }
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("\n\r detailTemplate: kendo.template($(\"#{0}\").html()),", sTemplateName);
            sb.Append("\n\r detailInit: function(d) {           // second detail init function");
            sb.Append("\n\r var detailRow = d.detailRow;");
            sb.AppendFormat("\n\r detailRow.find(\".{0}\")", pdd.PageDetCSSClass);
            sb.Append(".kendoTabStrip({");
            sb.Append("\n\r animation:");
            sb.Append("\n\r { open: { effects: \"fadeIn\" }}");
            sb.Append("\n\r });");
            //get the children for this tab strip
            LTS.cmPageDetail[] lvl2 = ltsPD.Where(x => x.PageDetParentID == pdd.PageDetControl && x.PageDetVisible == true).OrderBy(y => y.PageDetSequenceNo).ToArray();
            if (lvl2 != null || lvl2.Count() > 0)
            {
                if (pdd.PageDetElmtFieldControl != 0 && ltsEF.Any(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl))
                {
                    string sKeyFilter = string.Format("d.data.{0}", ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl).Select(y => y.ElmtFieldName).FirstOrDefault());
                    foreach (LTS.cmPageDetail detItem in lvl2)
                    {
                        if (detItem.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.Grid)
                        {
                            LTS.cmElementField[] ltsdetEF = dalSecData.getElementFields(detItem.PageDetControl, UserSecControl);
                            if (detItem.PageDetElmtFieldControl != 0 && ltsdetEF.Any(x => x.ElmtFieldControl == detItem.PageDetElmtFieldControl))
                            {
                                string sFilterFastTabCaption = "Filters";
                                string sTabStripData = "";
                                bool blnHasColumns = false;
                                bool blnHasTooltip = false;
                                //Modified by RHR for v-8.2 on 10/09/2018  added Edit Widget Functionality to Child Grids
                                string sWidgetCommands = "";
                                string sWidgetContainerDivID = "";
                                string sWidgetCommandTitleSize = "";
                                string sWidgetToolBars = "";
                                int iPK = detItem.PageDetElmtFieldControl;
                                LTS.cmElementField pk = ltsdetEF.Where(x => x.ElmtFieldControl == iPK).FirstOrDefault();
                                if (pk == null || pk.ElmtFieldControl == 0)
                                {
                                    pk = ltsdetEF.Where(x => x.ElmtFieldPK == true).FirstOrDefault();
                                }
                                if (pk != null)
                                {
                                    List<string> emptyToolTipClasses = new List<string>();
                                    bool blnEmptyHasTooltip = false;
                                    string sCRUDWidgetReadyJS = createNGLEditWidget(detItem, ltsPD, ltsdetEF, pk.ElmtFieldName, ref sWidgetCommands, ref sWidgetContainerDivID, ref sWidgetCommandTitleSize, ref sWidgetToolBars, UserSecControl,ref emptyToolTipClasses, ref blnEmptyHasTooltip, sKeyFilter, sParentID);
                                    if (!string.IsNullOrWhiteSpace(sCRUDWidgetReadyJS))
                                    {
                                        oHtml.addNestedHTML(new cmHTMLBuilder("div", sWidgetContainerDivID, "", "", ""));
                                        this.PageReadyJS += string.Format(" {0}  {1}  {0} ", this.cmLineSeperator, sCRUDWidgetReadyJS);
                                    }
                                }

                                List<string> toolTipClasses = new List<string>();
                                bool blnAutoHide = false;
                                bool blnShowFilters = false;
                                string sKey = string.IsNullOrWhiteSpace(detItem.PageDetTagIDReference) ? detItem.PageDetName : detItem.PageDetTagIDReference;
                                System.Text.StringBuilder sbJSFilterDataArray = new System.Text.StringBuilder();
                                string sJSDateFilterCaseStatement = "";
                                string sJSDateFilterFromToFields = "";
                                string sJSDateFilterFromToSplitter = "";
                                // ref sJSDateFilterFromToSplitter

                                string sID = ltsdetEF.Where(x => x.ElmtFieldControl == detItem.PageDetElmtFieldControl).Select(y => y.ElmtFieldName).FirstOrDefault();
                                sb.AppendFormat("{0}detailRow.find(\".{1}\").kendoNGLGrid({{{0}dataSource:{0}", this.cmLineSeperator, string.IsNullOrWhiteSpace(detItem.PageDetCSSClass) ? "adjustments" : detItem.PageDetCSSClass);
                                sb.Append(createEmbededfChildkendoDataSource(detItem.PageDetName, sID, detItem, ltsPD, ltsdetEF, sKeyFilter));
                                sb.Append("resizable: true, scrollable: true, sortable: false, pageable: false, selectable: \"row\",");
                                sb.Append("\n\r dataBound: function(e) { \n\r var tObj = this; \n\r if (typeof (" + detItem.PageDetName + "DataBoundCallBack) !== 'undefined' && ngl.isFunction(" + detItem.PageDetName + "DataBoundCallBack)) { " + detItem.PageDetName + "DataBoundCallBack(e,tObj); } \n\r }");

                                //TODO: fix bug where if one grid on the page has the export button all grids on the page have the export button
                                //  the Any statement must identify the parent ID
                                if (ltsPD.Any(x => x.PageDetParentID == detItem.PageDetControl && x.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.KendoGridExportToolBar))
                                {
                                    if (!string.IsNullOrWhiteSpace(sWidgetToolBars))
                                    {
                                        sb.AppendFormat(",{0} toolbar:[{{name: \"excel\", excel:{{fileName: \"{1}\", allPages: true}}}}, {2}]", this.cmLineSeperator, Utilities.timeStampFileName(detItem.PageDetCaption, ".xlsx"), sWidgetToolBars);
                                    }
                                    else
                                    {
                                        sb.AppendFormat(",{0} toolbar: [\"excel\"],excel:{{fileName: \"{1}\",allPages: true}}", this.cmLineSeperator, Utilities.timeStampFileName(detItem.PageDetCaption, ".xlsx"));
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(sWidgetToolBars))
                                {
                                    sb.AppendFormat(",{0} toolbar:[{1}]", this.cmLineSeperator, sWidgetToolBars);
                                }
                                //Modified by RHR on 08/29/2018 for v-8.2 to support saved page filter data
                                System.Text.StringBuilder sJSSavedFilterArray = new System.Text.StringBuilder();
                                //Modified by RHR on 08/12/2020 for v-8.3.0.001 to support new method to read saved filters 
                                DAL.Models.AllFilters oSavedFilters = readSavedFilters(detItem);
                                int iWidth = 0;
                                string strColumns = createGridFieldColumns(detItem, ltsPD, ltsdetEF, ref sFilterFastTabCaption, ref sTabStripData, ref blnHasColumns, ref blnHasTooltip, ref toolTipClasses, ref blnAutoHide, ref blnShowFilters, ref sbJSFilterDataArray, ref sJSDateFilterCaseStatement, ref sJSDateFilterFromToFields, ref sJSDateFilterFromToSplitter, ref sJSSavedFilterArray, ref oHtml, UserControl, ref oSavedFilters, ref sWidgetToolBars, ref iWidth, sWidgetCommands, sWidgetCommandTitleSize);
                                sbJSFilterDataArray.Append(" ];"); //close the javascript array 
                                if (blnHasColumns == true)
                                {
                                    //remove the last comma
                                    strColumns = strColumns.Remove(strColumns.Length - 1);
                                    sb.AppendFormat(",{0} columns: [{0}{1}{0}]", this.cmLineSeperator, strColumns);
                                }
                                //Modified by RHR on 02/27/2018 for v-8.1 to support new content management changers 
                                if (!string.IsNullOrWhiteSpace(sTabStripData))
                                {
                                    sb.AppendFormat(",{0} {1}", this.cmLineSeperator, sTabStripData);
                                }
                                sb.AppendFormat("{0} }});", this.cmLineSeperator);

                            }
                            else
                            {
                                sb.AppendFormat("\n\r // TabStrip element, {1}, has an invalid or missing element control number  \n\r", this.cmLineSeperator, detItem.PageDetName);
                            }
                        }
                        else
                        {
                            sb.AppendFormat("\n\r // TabStrip element, {1}, is not a grid; only grids are supported  \n\r", this.cmLineSeperator, detItem.PageDetName);
                        }
                    }
                }
                else
                {
                    sb.Append("\n\r // TabStrip is missing FK element control  \n\r");
                }
            }
            else
            {
                sb.Append("\n\r // TabStrip has no elements \n\r");
            }

            //    detailRow.find(".adjustments").kendoGrid(
            //createEmbededfChildkendoDataSource()
            //        dataSource:
            //        {
            //            transport:
            //            {
            //                read:
            //                {
            //                    url: "api/tblBidCostAdj/" + d.data.BidControl,
            //                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            //                                    type: "GET"
            //                                },
            //                                parameterMap: function(options, operation) { return options; }
            //            },                          
            //                            schema:
            //            {
            //                data: "Data",
            //                                total: "Count",
            //                                model:
            //                {
            //                    id: "BidCostAdjControl",
            //                                },
            //                                errors: "Errors"
            //                            },
            //                            error: function(e) {
            //                if (e.errors != null)
            //                {
            //                    ngl.showErrMsg("Access Denied", e.errors + "<br\>(Source: LoadHistoryGrid.detailInit.Adjustments)", null);
            //                }
            //                this.cancelChanges();
            //            },
            //                            serverPaging: false, serverSorting: false, serverFiltering: false
            //                        },
            //                        resizable: true, scrollable: true, sortable: false, pageable: false,
            //                        columns: [
            //                            { field: "BidCostAdjControl", title: "Control", hidden: true},
            //                            { field: "BidCostAdjBidControl", title: "Bid Control", hidden: true},
            //                            { field: "BidCostAdjFreightClass", title: "Class", width: 50},
            //                            { field: "BidCostAdjWeight", title: "Wgt", width: 50},
            //                            { field: "BidCostAdjDesc", title: "Desc", width: 200},
            //                            { field: "BidCostAdjDescCode", title: "Code", width: 50},
            //                            { field: "BidCostAdjAmount", title: "Amt", width: 50},
            //                            { field: "BidCostAdjRate", title: "Rate", width: 50},
            //                            { field: "BidCostAdjUOM", title: "UOM", width: 50},                                      
            //                            { field: "BidCostAdjModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(BidCostAdjModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #", hidden: true},
            //                            { field: "BidCostAdjModUser", title: "Mod User", hidden: true}
            //                        ]
            //                    });
            //    detailRow.find(".errors").kendoGrid({
            //        dataSource:
            //        {
            //            transport:
            //            {
            //                read:
            //                {
            //                    url: "api/tblBidSvcErr/" + d.data.BidControl,
            //                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            //                                    type: "GET"
            //                                },
            //                                parameterMap: function(options, operation) { return options; }
            //            },
            //                            schema:
            //            {
            //                data: "Data",
            //                                total: "Count",
            //                                model:
            //                {
            //                    id: "BidSvcErrControl"
            //                                },
            //                                errors: "Errors"},
            //                            error: function(e) {
            //                if (e.errors != null)
            //                {
            //                    ngl.showErrMsg("Access Denied", e.errors + "<br\>(Source: LoadHistoryGrid.detailInit.Errors)", null);
            //                }
            //                this.cancelChanges();
            //            },
            //                            serverPaging: false, serverSorting: false, serverFiltering: false
            //                        },
            //                        resizable: true, scrollable: true, sortable: false, pageable: false,
            //                        columns: [
            //                            { field: "BidSvcErrControl", title: "Control", hidden: true},
            //                            { field: "BidSvcErrBidControl", title: "Bid Control", hidden: true},
            //                            { field: "BidSvcErrErrorMessage", title: "Msg", width: 100},
            //                            { field: "BidSvcErrVendorErrorCode", title: "Vendor Code", width: 50},
            //                            { field: "BidSvcErrVendorErrorMessage", title: "Vendor Msg", width: 50},
            //                            { field: "BidSvcErrCode", title: "Code", width: 50},
            //                            { field: "BidSvcErrFieldName", title: "Field Name", width: 50},
            //                            { field: "BidSvcErrMessage", title: "Details", width: 100},                               
            //                            { field: "BidSvcErrModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(BidSvcErrModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #", hidden: true},
            //                            { field: "BidSvcErrModUser", title: "Mod User", hidden: true}
            //                        ]
            //                    });


            sb.Append("} //end of second detail init function");
            sb.Append("\n\r ");  // we always enter a line return after a comment
            return sb.ToString();

        }
        public string createkendoDataSource(string dsName, string id, LTS.cmPageDetail pdh, LTS.cmPageDetail[] ltsPD, LTS.cmElementField[] ltsEF, string sKeyFilter = "", string sJSFilter = "", string sGoupBy = "", string sDataAgg = "")
        {
            string result = "";

            //Parameter = new kendo.data.DataSource({
            //    transport: {
            //    read:
            //    {
            //        url: "api/Parameter/Get/", 
            //            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            //            type: "GET"},
            //        parameterMap: function(options, operation) { return options; }
            //}, 
            //    schema:
            //{
            //    data: "Data",  
            //        total: "Count", 
            //        model:
            //    {
            //        id: "ParKey",
            //            fields:
            //        {
            //            ParKey: { type: "string" },
            //                ParText: { type: "string" },
            //                ParDescription: { type: "string" },
            //                ParCategoryControl: { type: "number" },
            //                ParIsGlobal: { type: "bool"}
            //        }
            //    },
            //        errors: "Errors"
            //    },
            //    error: function(xhr, textStatus, error) {
            //    ngl.showErrMsg("Get Parameter Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null); this.cancelChanges();
            //},
            //    pageSize: 10,
            //    serverPaging: true,
            //    sortable: true,
            //    pageable: true,
            //    groupable: true,});
            if (!string.IsNullOrWhiteSpace(sJSFilter))
            {           //" data: { filter: JSON.stringify(function(options){ " + sJSFilter + " })}," +
                        //result = "var sFilter = JSON.stringify(function(options){ " + sJSFilter + " });" +
                        //        dsName + " = new kendo.data.DataSource({" +
                        //        " transport:{ read:{url: 'api/" + pdh.PageDetAPIReference + "/GetRecords/' + sFilter})\"," +
                        //        " contentType: \"application / json; charset = utf - 8\"," +
                        //        " dataType: 'json',"  +
                        //        " headers: { \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452}," +
                        //        " type: \"GET\"}," +
                        //        " parameterMap: function(options, operation) { return options; }}," +
                        //        " schema: {data: \"Data\"," +
                        //        " total: \"Count\"," +
                        //        " model: {id: \"" + id + "\"," +
                        //        "   fields: {";

                //result = dsName + " = new kendo.data.DataSource({" +
                //       " transport:{ " + 
                //       "    read:{ " + 
                //       "        url: \"api/" + pdh.PageDetAPIReference + "/GetRecords\"," +
                //       "        contentType: \"application / json; charset = utf - 8\"," +
                //       "        dataType: 'json'," +
                //       "        data: { filter: JSON.stringify(function(options){ " + sJSFilter + " })}," + 
                //       "        headers: { \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452}," +
                //       "        type: \"GET\"," +
                //       "        parameterMap: function(options, operation) { return options; }" +
                //       "      }" +
                //       "  }," +
                //       " schema: {data: \"Data\"," +
                //       " total: \"Count\"," +
                //       " model: {id: \"" + id + "\"," +
                //       "   fields: {";
                //Modified by RHR for v-8.2 on 2/12/2019 we are showing error message twice once in the grid and once in the datasouce so I removed the error message from the datasouce to prevent duplicate errors.
                // old code modified below: result = string.Format("{0} {1} = new kendo.data.DataSource({{{0} serverSorting: true, {0} serverPaging: true, {0} pageSize: 10,{0} transport: {{ {0} read: function(options) {{ {2} {0} $.ajax({{ {0} url: 'api/{3}/GetRecords/' + s, {0}  contentType: 'application/json; charset=utf-8', {0} dataType: 'json', {0}  data: {{ filter: JSON.stringify(s) }}, {0} headers: {{ \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452 }}, {0} success: function(data) {{ {0}  options.success(data); {0} if (typeof (data) !== 'undefined' && ngl.isObject(data) && typeof (data.Errors) !== 'undefined' &&  data.Errors != null) {{ ngl.showErrMsg('Access Denied', data.Errors, null); }} }}, {0} error: function(result) {{{0} options.error(result); {0} }} {0} }}); {0} }}, {0}  parameterMap: function(options, operation) {{ return options; }} {0} }},  {0} schema: {{{0} data: \"Data\",{0} total: \"Count\",{0}  model: {{{0} id: \"{4}\",{0} fields: {{", this.cmLineSeperator, dsName, sJSFilter, pdh.PageDetAPIReference, id);
                result = string.Format("{0} {1} = new kendo.data.DataSource({{{0} serverSorting: true, {0} serverPaging: true, {0} pageSize: 10,{0} transport: {{ {0} read: function(options) {{ {2} {0} $.ajax({{ {0} url: 'api/{3}/GetRecords/' + s, {0}  contentType: 'application/json; charset=utf-8', {0} dataType: 'json', {0}  data: {{ filter: JSON.stringify(s) }}, {0} headers: {{ \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452 }}, {0} success: function(data) {{ {0}  options.success(data); {0} if (typeof (data) !== 'undefined' && ngl.isObject(data) && typeof (data.Errors) !== 'undefined' &&  data.Errors != null) {{  }} }}, {0} error: function(result) {{{0} options.error(result); {0} }} {0} }}); {0} }}, {0}  parameterMap: function(options, operation) {{ return options; }} {0} }},  {0} schema: {{{0} data: \"Data\",{0} total: \"Count\",{0}  model: {{{0} id: \"{4}\",{0} fields: {{", this.cmLineSeperator, dsName, sJSFilter, pdh.PageDetAPIReference, id);
            }
            else
            {
                result = dsName + " = new kendo.data.DataSource({" +
                   " transport:{ read:{url: \"api/" + pdh.PageDetAPIReference + "/Get/" + sKeyFilter + "\"," +
                   " contentType: \"application / json; charset = utf - 8\"," +
                   " dataType: 'json'," +
                   " headers: { \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452}," +
                   " type: \"GET\"}," +
                   " parameterMap: function(options, operation) { return options; }}," +
                   " schema: {data: \"Data\"," +
                   " total: \"Count\"," +
                   " model: {id: \"" + id + "\"," +
                   "   fields: {";
            }
            string sSeperater = "";

            //add the fields
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == pdh.PageDetControl).OrderBy(x => x.PageDetSequenceNo))
            {
                if (pdd != null)
                {
                    LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                    if (ef != null)
                    {
                        if (!String.IsNullOrWhiteSpace(ef.ElmtFieldName))
                        {
                            result += sSeperater + "\n\r     " + ef.ElmtFieldName + ": { type: \"" + getJavaScriptDataType(ef.ElmtFieldDataTypeControl) + "\" }";
                            sSeperater = ",";
                        }
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(sGoupBy)) { sGoupBy = "group: []"; }
            if (string.IsNullOrWhiteSpace(sDataAgg)) { sDataAgg = "aggregate: []"; }

            //add the remaining tags
            result += "\n\r     }" + //close fields
                "\n\r     }," + // close models
                "\n\r    errors: \"Errors\"" +
                "\n\r}," +
                "\n\r    " + sGoupBy + "," +
                "\n\r    " + sDataAgg + "," +
                "\n\r   error: function(xhr, textStatus, error) {" +
                "\n\r       ngl.showErrMsg(\"Access " + dsName + " Data Failed\", formatAjaxJSONResponsMsgs(xhr, textStatus, error, \" cannot complete your request\"), null); this.cancelChanges();" +
                "\n\r   }" +
                "\n\r});\n\r\n\r\n\r";

            return result;
        }
        public string createEmbededfChildkendoDataSource(string dsName, string id, LTS.cmPageDetail pdh, LTS.cmPageDetail[] ltsPD, LTS.cmElementField[] ltsEF, string sKeyFilter = "")
        {
            string result = "";
            result = "{\n\r serverSorting: false, " +
                    "\n\r serverPaging: false, " +
                    "\n\r serverFiltering: false," +
                    "\n\r transport:{ read:{url: \"api/" + pdh.PageDetAPIReference + "/\" +  " + sKeyFilter + "," +
                   "\n\r  headers: { \"Authorization\": localStorage.NGLvar1454, \"USC\": localStorage.NGLvar1452}," +
                   "\n\r  type: \"GET\"}," +
                   "\n\r  parameterMap: function(options, operation) { return options; }}," +
                   "\n\r  schema: {data: \"Data\"," +
                   "\n\r  total: \"Count\"," +
                   "\n\r  model: {id: \"" + id + "\"," +
                   "\n\r  fields: {";


            string sSeperater = "";

            //add the fields
            foreach (LTS.cmPageDetail pdd in ltsPD.Where(x => x.PageDetParentID == pdh.PageDetControl).OrderBy(x => x.PageDetSequenceNo))
            {
                if (pdd != null)
                {
                    LTS.cmElementField ef = (ltsEF.Where(x => x.ElmtFieldControl == pdd.PageDetElmtFieldControl)).FirstOrDefault();
                    if (ef != null)
                    {
                        if (!String.IsNullOrWhiteSpace(ef.ElmtFieldName))
                        {
                            result += sSeperater + "\n\r     " + ef.ElmtFieldName + ": { type: \"" + getJavaScriptDataType(ef.ElmtFieldDataTypeControl) + "\" }";
                            sSeperater = ",";
                        }
                    }
                }
            }


            //add the remaining tags
            result += "\n\r     }" + //close fields
                "\n\r     }," + // close models
                "\n\r    errors: \"Errors\"" +
                "\n\r}," +
                "\n\r   error: function(xhr, textStatus, error) {" +
                "\n\r       ngl.showErrMsg(\"Access " + dsName + " Data Failed\", formatAjaxJSONResponsMsgs(xhr, textStatus, error, \" cannot complete your request\"), null); this.cancelChanges();" +
                "\n\r   }" +
                "\n\r},\n\r";

            return result;
        }


        #region "Auto Complete"

        public string createkendoAutoComplete(LTS.cmPageDetail det)
        {
            string result = "";
            if (det == null || det.PageDetGroupSubTypeControl != 6) { return result; }

            return result;
        }

        #endregion

        #region "Combo Box"

        public string createkendoComboBox(LTS.cmPageDetail det)
        {
            string result = "";
            if (det == null || det.PageDetGroupSubTypeControl != 8) { return result; }
            //sample
            //$("#fabric").kendoComboBox({
            //    dataTextField: "text",
            //            dataValueField: "value",
            //            dataSource: [
            //                { text: "Cotton", value: "1" },
            //                { text: "Polyester", value: "2" },
            //                { text: "Cotton/Polyester", value: "3" },
            //                { text: "Rib Knit", value: "4" }
            //            ],
            //            filter: "contains",
            //            suggest: true,
            //            index: 3
            //        });

            return result;
        }

        #endregion

        #region "Drop Down"

        public string createkendoDropDownList(LTS.cmPageDetail det)
        {
            string result = "";
            if (det == null || det.PageDetGroupSubTypeControl != 11) { return result; }
            //sample
            //var data = [
            //            { text: "Black", value: "1" },
            //            { text: "Orange", value: "2" },
            //            { text: "Grey", value: "3" }
            //        ];

            //        // create DropDownList from input HTML element
            //        $("#color").kendoDropDownList({
            //    dataTextField: "text",
            //            dataValueField: "value",
            //            dataSource: data,
            //            index: 0,
            //            change: onChange
            //        });


            return result;
        }

        #endregion



        #region "Date Picker"

        public string createkendoDatePicker(string name, DateTime? val)
        {
            string result = "";
            result = "$(\"#" + name + "\").kendoDatePicker();";

            if (val.HasValue)
            {
                //note: we use {{ and }} to output a single { or }
                result = string.Format("$(\"#{0}\").kendoDatePicker({{value:\"{1}\"}});", name, val.Value.ToShortDateString());
            }
            else
            {
                result = "$(\"#" + name + "\").kendoDatePicker();";
            }
            return result;
        }

        public string createkendoDatePicker(LTS.cmPageDetail det, DateTime? val = null)
        {
            if (det == null || det.PageDetGroupSubTypeControl != 9) { return ""; }
            if (!val.HasValue) { val = DateTime.Now; }
            return createkendoDatePicker(det.PageDetName, val);
        }

        public string createkendoDatePicker(string name)
        {
            return createkendoDatePicker(name, null);
        }

        #endregion

        #region "Time Picker"

        public string createkendoDateTimePicker(string name, DateTime? val)
        {
            string result = "";
            if (val.HasValue)
            {
                //note: we use {{ and }} to output a single { or }
                result = string.Format("$(\"#{0}\").kendoDateTimePicker({{value:\"{1}\"}});", name, val);
            }
            else
            {
                result = "$(\"#" + name + "\").kendoDateTimePicker();";
            }

            return result;
        }

        public string createkendoDateTimePicker(LTS.cmPageDetail det, DateTime? val = null)
        {
            if (det == null || det.PageDetGroupSubTypeControl != 10) { return ""; }
            if (!val.HasValue) { val = DateTime.Now; }
            return createkendoDateTimePicker(det.PageDetName, val);
        }

        #endregion


        #region "Color Picker"

        public string createkendoColorPicker(LTS.cmPageDetail det)
        {
            string result = "";
            if (det == null || det.PageDetGroupSubTypeControl != 7) { return result; }
            result = "$(\"#" + det.PageDetName + "\").kendoColorPicker({ value: \"#ffffff\",buttons: false,select: " + det.PageDetMetaData + "});";
            return result;
        }

        #endregion


        #region "kendo Button"

        public string createkendoButton(string name)
        {
            string result = "";
            result = "$(\"#" + name + "\").kendoButton();";
            return result;
        }

        public string createkendoButton(LTS.cmPageDetail det)
        {
            if (det == null || det.PageDetGroupSubTypeControl != 15) { return ""; }
            return createkendoButton(det.PageDetName);
        }

        #endregion

        #region "Masked TextBox"

        public string createkendoMaskedTextBox(string name, string mask)
        {
            string result = "";
            //note: we use {{ and }} to output a single { or }
            result = string.Format("$(\"#{0}\").kendoMaskedTextBox({{mask: \"{1}\"}});", name, mask);
            return result;
        }

        /// <summary>
        /// Deprecated: should use createkendoMaskedTextBox(string name, string mask) overload 
        /// with new PageDetFieldFormatOverride or cmElementField.ElmtFieldFormat
        /// </summary>
        /// <param name="det"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 07/10/2018
        ///   we no longer support using Meta data for input mask
        ///   We should use a reference to cmElementField.ElmtFieldFormat
        ///   or the new PageDetFieldFormatOverride
        ///   PageDetMetaData on input data is now used for the default value
        ///   when adding a new record.
        /// </remarks>
        public string createkendoMaskedTextBox(LTS.cmPageDetail det)
        {
            if (det == null || det.PageDetGroupSubTypeControl != 13) { return ""; }
            return createkendoMaskedTextBox(det.PageDetName, det.PageDetMetaData);
        }

        #endregion

        #region "Numeric TextBox"

        public string createkendoNumericTextBox(string name, string mask)
        {
            string result = "";
            //note: we use {{ and }} to output a single { or }
            result = string.Format("$(\"#{0}\").kendoMaskedTextBox({{mask: \"{1}\"}});", name, mask);
            return result;
        }

        public string createkendoNumericTextBox(LTS.cmPageDetail det)
        {
            if (det == null || det.PageDetGroupSubTypeControl != 15) { return ""; }
            return createkendoNumericTextBox(det.PageDetName, det.PageDetMetaData);
        }

        #endregion


        public string insertkendoDatePickerLiHTML(LTS.cmPageDetail pdItem)
        {
            //ToDo:  correct the format of this objects html
            //TODO: add meta data field once project is updated with changes
            //TODO: add look up for localization logic
            return string.Format("<li class=\"filterfieldLabel\"><label for=\"{1}\">{0}:</label></li><li><input id=\"{1}\" value=\"{2}\" /></li>", pdItem.PageDetCaption, pdItem.PageDetName, "Reference Meta Data Here");
        }

        public string insertkendoMaskedTextBoxLiHTML(LTS.cmPageDetail pdItem)
        {
            //TODO: add meta data field once project is updated with changes
            //TODO: add look up for localization logic
            return string.Format("<li class=\"filterfieldLabel\"><label for=\"{1}\">{0}:</label></li><li><input id=\"{1}\" value=\"{2}\" /></li>", pdItem.PageDetCaption, pdItem.PageDetName, "Reference Meta Data Here");

        }

        public string insertkendoDateTimePickerLiHTML(LTS.cmPageDetail pdItem)
        {
            //ToDo:  correct the format of this objects html
            //TODO: add meta data field once project is updated with changes
            //TODO: add look up for localization logic
            return string.Format("<li class=\"filterfieldLabel\"><label for=\"{1}\">{0}:</label></li><li><input id=\"{1}\" value=\"{2}\" /></li>", pdItem.PageDetCaption, pdItem.PageDetName, "Reference Meta Data Here");
        }

        //NumericTextBox
        public string insertkendoNumericTextBoxLiHTML(LTS.cmPageDetail pdItem)
        {
            //ToDo:  correct the format of this objects html
            //TODO: add meta data field once project is updated with changes
            //TODO: add look up for localization logic
            //ToDo: add logic to add min and max and step values to the numeric text box
            return string.Format("<li class=\"filterfieldLabel\"><label for=\"{1}\">{0}:</label></li><li><input id=\"{1}\" value=\"{2}\" /></li>", pdItem.PageDetCaption, pdItem.PageDetName, "Reference Meta Data Here");
        }

        public string insertkendoDatePickerTDHTML(LTS.cmPageDetail pdItem)
        {
            //ToDo:  correct the format of this objects html
            //TODO: add meta data field once project is updated with changes
            //TODO: add look up for localization logic
            return string.Format("<li class=\"filterfieldLabel\"><label for=\"{1}\">{0}:</label></li><li><input id=\"{1}\" value=\"{2}\" /></li>", pdItem.PageDetCaption, pdItem.PageDetName, "Reference Meta Data Here");
        }

        public string insertkendoMaskedTextBoxTDHTML(LTS.cmPageDetail pdItem)
        {
            //TODO: add meta data field once project is updated with changes
            //TODO: add look up for localization logic
            return string.Format("<li class=\"filterfieldLabel\"><label for=\"{1}\">{0}:</label></li><li><input id=\"{1}\" value=\"{2}\" /></li>", pdItem.PageDetCaption, pdItem.PageDetName, "Reference Meta Data Here");

        }

        public string insertkendoDateTimePickerTDHTML(LTS.cmPageDetail pdItem)
        {
            //ToDo:  correct the format of this objects html
            //TODO: add meta data field once project is updated with changes
            //TODO: add look up for localization logic
            return string.Format("<li class=\"filterfieldLabel\"><label for=\"{1}\">{0}:</label></li><li><input id=\"{1}\" value=\"{2}\" /></li>", pdItem.PageDetCaption, pdItem.PageDetName, "Reference Meta Data Here");
        }

        //NumericTextBox
        public string insertkendoNumericTextBoxTDHTML(LTS.cmPageDetail pdItem)
        {
            //ToDo:  correct the format of this objects html
            //TODO: add meta data field once project is updated with changes
            //TODO: add look up for localization logic
            //ToDo: add logic to add min and max and step values to the numeric text box
            return string.Format("<li class=\"filterfieldLabel\"><label for=\"{1}\">{0}:</label></li><li><input id=\"{1}\" value=\"{2}\" /></li>", pdItem.PageDetCaption, pdItem.PageDetName, "Reference Meta Data Here");
        }


        #region "kendo Editor"

        public string createKendoEditorCustomTool(LTS.cmPageDetail detItem)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string name = String.IsNullOrWhiteSpace(detItem.PageDetName) ? "custom" : detItem.PageDetName;
            string tooltip = String.IsNullOrWhiteSpace(detItem.PageDetDesc) ? "" : detItem.PageDetDesc;
            string sClass = String.IsNullOrWhiteSpace(detItem.PageDetCSSClass) ? "check" : detItem.PageDetCSSClass;
            //string sUi = "ui: { type: 'button',icon: '" + sClass + "' }"
            sb.Append("{ ");
            sb.Append(string.Format("name: '{0}',", name));
            sb.Append(string.Format("ui: {{ type: 'button',icon: '{0}'}},", sClass));
            sb.Append(string.Format(" tooltip: '{0}',", tooltip));
            sb.Append(string.Format("exec: function(e) {{ {0} }}", detItem.PageDetMetaData));
            sb.Append("},");

            return sb.ToString();
        }

        public string createKendoEditorStandardTools(LTS.cmPageDetail detItem)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            String[] substrings = (detItem.PageDetMetaData).Split('|');
            foreach (string s in substrings)
            {
                sb.Append(string.Format("'{0}',", s));
            }

            return sb.ToString();
        }

        public string createKendoEditor(LTS.cmPageDetail pdItem, LTS.cmPageDetail[] ltsPD, int UserSecControl)
        {
            /*
            //NOTE LVV: Eventually we probably want to write this code to automatically create the HTML for on screen Editors but
            //it is too big of a change to make today (11/12/19). Don't forget to add parameter "ref cmHTMLBuilder oHtml" if uncomment code
            //Also must change line sb.Append(string.Format("$('#{0}').kendoEditor({{", pdItem.PageDetName)); to sb.Append(string.Format("$('#{0}').kendoEditor({{", sKey));           
            string sKey = string.IsNullOrWhiteSpace(pdItem.PageDetTagIDReference) ? pdItem.PageDetName : pdItem.PageDetTagIDReference;
            if (pdItem.PageDetElmtFieldControl == 0) //because sometimes Update fields etc. are Editors and we don't want to screw this up
            {
                oHtml = new cmHTMLBuilder("textarea", sKey, "", "height: 90%; width: 90%;", "");
            }
            */

            string sRet = "";
            int detailControl = pdItem.PageDetControl;
            //string sKey = pdItem.PageDetName;
            string sTools = "";
            //string sFastTabActions = "";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("$('#{0}').kendoEditor({{", pdItem.PageDetName));
            sb.Append("resizable: { content: true, toolbar: true },");
            sb.Append("encoded: false,"); //Modified By LVV on 2/17/20 - Added so it would send HTML and not encode it - was causing problems with emails
            sb.Append("tools: [");

            //get all of the children for this page detail
            LTS.cmPageDetail[] lvl2 = ltsPD.Where(x => x.PageDetParentID == detailControl && x.PageDetVisible == true).OrderBy(y => y.PageDetSequenceNo).ToArray();
            if (lvl2 != null || lvl2.Count() > 0)
            {
                string sFieldCRUDTagID = "";
                foreach (LTS.cmPageDetail detItem in lvl2)
                {
                    if (detItem.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.EditorCustomTool)
                    {
                        cmHTMLBuilder[] cHTMLs = processPageDetailsSubTypes(detItem, ltsPD, UserSecControl, ref sFieldCRUDTagID);
                        if (cHTMLs != null && cHTMLs.Count() > 0)
                        {
                            foreach (cmHTMLBuilder h in cHTMLs)
                            {
                                sTools += h.ToString();
                            }
                        }
                    }
                    if (detItem.PageDetGroupSubTypeControl == (int)Utilities.GroupSubType.EditorStandardTools)
                    {
                        cmHTMLBuilder[] cHTMLs = processPageDetailsSubTypes(detItem, ltsPD, UserSecControl, ref sFieldCRUDTagID);
                        if (cHTMLs != null && cHTMLs.Count() > 0)
                        {
                            foreach (cmHTMLBuilder h in cHTMLs)
                            {
                                sTools += h.ToString();
                            }
                        }
                    }
                }
                //remove the last comma
                if (!string.IsNullOrWhiteSpace(sTools)) { sTools = sTools.Remove(sTools.Length - 1); }
            }
            else
            {
                //Add the default tools which is all tools
                sTools = "'bold', 'italic', 'underline', 'strikethrough', 'subscript', 'superscript','fontName', 'fontSize', 'foreColor', 'backColor','justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull','insertUnorderedList', 'insertOrderedList', 'indent', 'outdent','createLink', 'unlink', 'insertImage', 'insertFile','createTable', 'addColumnLeft', 'addColumnRight', 'addRowAbove', 'addRowBelow', 'deleteRow', 'deleteColumn','formatting','print','pdf'";
            }
            sb.Append(sTools);
            sb.Append("]");
            sb.Append("});");
            sRet = sb.ToString();
            return sRet;
        }

        #endregion


        #region "Menu Tree"

        /// <summary>
        /// Creates the Main Menu Tree using configuration from the database table cmMenuTree
        /// and returns the string to the caller
        /// </summary>
        /// <param name="UserSecControl"></param>
        /// <returns>string</returns>
        /// <remarks>
        /// Added By LVV on 12/20/16 for v-8.0 Content Management Tables
        ///  add logic to cache the menu tree in a data dictionary with a 30 minute expiration date by user
        ///  add '30 min expiration' to web config as an app setting
        ///  check if it is in cache and if it is not expired -- then use it
        ///  else get from db
        /// Modified by RHR for v-Appsource1 on 04/28/2017  we just use the default App Source Menu
        ///   here by setting the UserSecControl to -1  
        /// Modified By LVV on 11/25/19
        ///  Added id to child nodes - was previously missing. 
        ///  Note: We really should make it so all the nodes have the exact same fields but I can't do that today because this is supposed to be a stable version (bug fix vs major change)
        ///  text: MenuTreeCaption/MenuTreeCaptionLocal (caption), id: MenuTreeLinkPageControl (linkPgCtrl), LinksTo: MenuTreeLinkTo (linkURL), mtc: MenuTreeControl, expanded: MenuTreeExpanded (nodeExpanded)
        /// </remarks>
        public string getMenuTree(int UserSecControl)
        {
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            string sRet = "";
            //if the menu tree for the usc is in the cache and not expired then use it else create from db
            bool blnRefreshTree = true;
            if (Utilities.GlobalMenuTreeByUser.ContainsKey(UserSecControl))
            {
                Models.GenericResult gmt = Utilities.GlobalMenuTreeByUser[UserSecControl];
                if (gmt.dtField > DateTime.Now) { blnRefreshTree = false; sRet = gmt.strField; }
            }
            if (blnRefreshTree)
            {
                LTS.cmMenuTree[] dtoMT = dalSecData.getMenuTreeData(UserSecControl);
                int favoriteId = 0;
                sRet = string.Concat("$('#menuTree').kendoTreeView({",
                   " dataUrlField: 'LinksTo',",
                   " template: \"#=item.text#\",",
                   " dataSource: { ",
                   "     data: [ ");
                //For each menuItem returned grab the control number and create one section of the string below for each menuItem without a ParentID
                //Inside of each menuItem select the child records using the ParentID and populate the child menu tree
                foreach (LTS.cmMenuTree mt in dtoMT.Where(x => x.MenuTreeParentID == 0 && x.MenuTreeVisible == true).OrderBy(x => x.MenuTreeSequenceNo))
                {
                    if (mt.MenuTreeParentID == 0 && mt.MenuTreeVisible == true)
                    {
                        //first create the parent string        
                        string pCaption = Utilities.GetLocalizedString(mt.MenuTreeCaption, mt.MenuTreeCaptionLocal, null);
                        string pLinkPgCtrl = mt.MenuTreeLinkPageControl.ToString();
                        string pLinkURL = mt.MenuTreeLinkTo;
                        string pMenuTreeCtrl = mt.MenuTreeControl.ToString();
                        string pNodeExpanded = mt.MenuTreeExpanded.ToString().ToLower();                        

                        if (mt.MenuTreeName == "Favorites")
                        {
                            favoriteId = mt.MenuTreeControl;
                        }

                        if (!string.IsNullOrWhiteSpace(mt.MenuTreeLinkTo)) { sRet += string.Format("{{text: '<span name=\"itemName\" data-itemId=\"{2}\" data-security={5} data-order={6}>{0}</span>', id: '{1}', mtc: '{2}', expanded: {3}, LinksTo: '{4}'", pCaption, pLinkPgCtrl, pMenuTreeCtrl, pNodeExpanded, pLinkURL, mt.MenuTreeUserSecurityControl, mt.MenuTreeSequenceNo); } else { sRet += string.Format("{{text: '<span name=\"itemName\" data-itemId={2} data-security={4} data-order={5}>{0}</span>', id: '{1}', mtc: '{2}', expanded: {3}", pCaption, pLinkPgCtrl, pMenuTreeCtrl, pNodeExpanded, mt.MenuTreeUserSecurityControl, mt.MenuTreeSequenceNo); }
                        //next create any children
                        bool blnHadItems = false;
                        string strItems = "";
                        System.Text.StringBuilder sbItems = new System.Text.StringBuilder();
                        foreach (LTS.cmMenuTree mtt in dtoMT.Where(x => x.MenuTreeParentID == mt.MenuTreeControl).OrderBy(x => x.MenuTreeSequenceNo))
                        {
                            if (mtt != null)
                            {
                                blnHadItems = true;
                                string cCaption = Utilities.GetLocalizedString(mtt.MenuTreeCaption, mtt.MenuTreeCaptionLocal, null);
                                string cLinkPgCtrl = mtt.MenuTreeLinkPageControl.ToString();
                                string cLinkURL = mtt.MenuTreeLinkTo;
                                
                                if (mt.MenuTreeName == "Favorites")
                                {
                                    if (!string.IsNullOrWhiteSpace(mtt.MenuTreeLinkTo)) { sbItems.Append(string.Format("{{text: '<span name=\"itemName\" data-parentId=\"{3}\" data-itemId=\"{4}\" data-position=\"{5}\">{0}</span> <div class=\"menuItem\" style =\"display: none\"><span title=\"Remove\" name=\"Remove\" class=\"k-icon k-i-apply-format\" onclick=\"removeMenuItemFromFavorites({4}); return false;\"></span></div>', visible: true, id: '{1}', LinksTo: '{2}'}},", cCaption, cLinkPgCtrl, cLinkURL, mtt.MenuTreeParentID, mtt.MenuTreeControl, mtt.MenuTreeSequenceNo)); } else { sbItems.Append(string.Format("{{text: '{0}', id: '{1}'}},", cCaption, cLinkPgCtrl)); }
                                }
                                else
                                {
                                    if (mtt.MenuTreeVisible)
                                    {
                                        if (!string.IsNullOrWhiteSpace(mtt.MenuTreeLinkTo)) { sbItems.Append(string.Format("{{text: '<span name=\"itemName\" data-parentId=\"{3}\" data-itemId=\"{4}\" data-position=\"{5}\">{0}</span> <div class=\"menuItem\" style =\"display: none\"><span title=\"Add to favorites\" onclick=\"addMenuItemToFavorites({6},{4}); return false;\" name=\"Favorite\" class=\"k-icon k-i-star\"></span><span title=\"Remove\" name=\"Remove\" class=\"k-icon k-i-x-circle\" onclick=\"toggleMenuItemVisibility({4}); return false;\"></span></div>', visible: true, id: '{1}', LinksTo: '{2}'}},", cCaption, cLinkPgCtrl, cLinkURL, mtt.MenuTreeParentID, mtt.MenuTreeControl, mtt.MenuTreeSequenceNo, favoriteId)); } else { sbItems.Append(string.Format("{{text: '{0}', id: '{1}'}},", cCaption, cLinkPgCtrl)); }

                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(mtt.MenuTreeLinkTo)) { sbItems.Append(string.Format("{{text: '<span name=\"itemName\" style=\"display: none;\" class=\"nonVisible\"  data-parentId=\"{3}\" data-itemId=\"{4}\" data-position=\"{5}\">{0}</span> <div class=\"menuItem\" style=\"display: none\"><span title=\"Add to favorites\" onclick=\"addMenuItemToFavorites({6},{4}); return false;\" name=\"Favorite\" class=\"k-icon k-i-star\"></span><span title=\"Restore\" name=\"Restore\" class=\"k-icon k-i-reset\" onclick=\"toggleMenuItemVisibility({4}); return false;\"></span></div>', visible: false, id: '{1}', LinksTo: '{2}'}},", cCaption, cLinkPgCtrl, cLinkURL, mtt.MenuTreeParentID, mtt.MenuTreeControl, mtt.MenuTreeSequenceNo, favoriteId)); } else { sbItems.Append(string.Format("{{text: '{0}', id: '{1}'}},", cCaption, cLinkPgCtrl)); }
                                    }
                                }
                            }
                            strItems = sbItems.ToString();
                        }
                        if (blnHadItems == true)
                        {
                            sRet += string.Format(", items: [{0}", strItems);
                            sRet = sRet.Remove(sRet.Length - 1); //remove the last comma
                            sRet += "]},";
                        }
                        else { sRet += "},"; }
                    }
                }
                sRet = sRet.Remove(sRet.Length - 1); //remove the last comma
                sRet += "]}, dataBound: function(e){onMenuTreeDataBound(e);}, dragAndDrop: false, dragend: function(e){onDragEndMenuTree(e);},drop: function(e){dropNodeMenuTree(e);}, drag: function(e){dragNodeMenuTree(e);}, loadOnDemand: false, expand: function(e){saveExpandNode(e);}, collapse: function(e){saveCollapseNode(e);} }).data('kendoTreeView'), handleTextBox = function (callback) { return function (e) { if (e.type != 'keypress' || kendo.keys.ENTER == e.keyCode) {callback(e);} }; };";
                //add menu to cache before return                
                Models.GenericResult gr = new Models.GenericResult();
                double temp = 0;
                string strExpires = System.Configuration.ConfigurationManager.AppSettings["MenuTreeExpiresMinutes"]; //get the refresh minutes from the web config
                bool blnTry = double.TryParse(strExpires, out temp);
                double min = temp == 0 ? 30 : temp;
                gr.strField = sRet;
                gr.dtField = DateTime.Now.AddMinutes(min); //add the refresh minutes to the current time to set the expiration date
                if (Utilities.GlobalMenuTreeByUser.ContainsKey(UserSecControl)) { Utilities.GlobalMenuTreeByUser[UserSecControl] = gr; } else { Utilities.GlobalMenuTreeByUser.Add(UserSecControl, gr); } //add or update the menu tree to the cache
            }
            return sRet;
        }

        #endregion

        #region "Menu Tree Hover"

        public string menuTreeHover()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("$(document).ready(function () {");
            sb.Append("$(function () {");

            sb.Append("$('span[name=\"Remove\"]').hover(function () { ");
            sb.Append("$(this).parents(\"a.k-in\").removeClass(\"k-fab k-fab-error\");");
            sb.Append("});");

            sb.Append("$('span[name=\"Remove\"]').mouseover(function () {");
            sb.Append("$(this).parents(\"a.k-in\").addClass(\"k-fab k-fab-error\");");
            sb.Append("});");

            sb.Append("$('span[name=\"Favorite\"]').hover(function() {");
            sb.Append("$(this).parents(\"a.k-in\").removeClass(\"k-fab k-fab-warning\");");
            sb.Append("});");

            sb.Append("$('span[name=\"Favorite\"]').mouseover(function () {");
            sb.Append("$(this).parents(\"a.k-in\").addClass(\"k-fab k-fab-warning\");");
            sb.Append("});");

            sb.Append("$('span[name=\"Restore\"]').hover(function() {");
            sb.Append("$(this).parents(\"a.k-in\").removeClass(\"k-fab k-fab-success\");");
            sb.Append("});");

            sb.Append("$('span[name=\"Restore\"]').mouseover(function() { ");
            sb.Append("$(this).parents(\"a.k-in\").addClass(\"k-fab k-fab-success\");");
            sb.Append("});");

            sb.Append("});");
            sb.Append("});");

            return sb.ToString();
        }

        #endregion

        #region "Help Window"

        public string getHelpWindow()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("setTimeout(function () { ");
            sb.Append("$('#txtE4PHCont').val(0); $('#txtE3PHCont').val(0); $('#txtE2PHCont').val(0); $('#txtE1PHCont').val(0);");

            sb.Append("$('#editor1').kendoEditor({");
            sb.Append("resizable: { content: true, toolbar: true },");
            sb.Append("tools: [");
            sb.Append("{ name: 'save',   ui: {type:'button',icon: 'save' }, tooltip: 'Save',");
            sb.Append("exec: function(e) {");
            sb.Append("saveHelpEditorL1();");
            sb.Append("}");
            sb.Append("},");
            sb.Append("'bold', 'italic', 'underline', 'strikethrough', 'subscript', 'superscript','fontName', 'fontSize', 'foreColor', 'backColor','justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull','insertUnorderedList', 'insertOrderedList', 'indent', 'outdent','createLink', 'unlink', 'insertImage', 'insertFile','createTable', 'addColumnLeft', 'addColumnRight', 'addRowAbove', 'addRowBelow', 'deleteRow', 'deleteColumn','formatting','print','pdf'");
            sb.Append("]");
            sb.Append("});");

            sb.Append("$('#editor2').kendoEditor({");
            sb.Append("resizable: { content: true, toolbar: true },");
            sb.Append("tools: [");
            sb.Append("{ name: 'save',   ui: {type:'button',icon: 'save' }, tooltip: 'Save',");
            sb.Append("exec: function(e) {");
            sb.Append("saveHelpEditorL2();");
            sb.Append("}");
            sb.Append("},");
            sb.Append("'bold', 'italic', 'underline', 'strikethrough', 'subscript', 'superscript','fontName', 'fontSize', 'foreColor', 'backColor','justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull','insertUnorderedList', 'insertOrderedList', 'indent', 'outdent','createLink', 'unlink', 'insertImage', 'insertFile','createTable', 'addColumnLeft', 'addColumnRight', 'addRowAbove', 'addRowBelow', 'deleteRow', 'deleteColumn','formatting','print','pdf'");
            sb.Append("]");
            sb.Append("});");

            sb.Append("$('#editor3').kendoEditor({");
            sb.Append("resizable: { content: true, toolbar: true },");
            sb.Append("tools: [");
            sb.Append("{ name: 'save',   ui: {type:'button',icon: 'save' }, tooltip: 'Save',");
            sb.Append("exec: function(e) {");
            sb.Append("saveHelpEditorL3();");
            sb.Append("}");
            sb.Append("},");
            sb.Append("'bold', 'italic', 'underline', 'strikethrough', 'subscript', 'superscript','fontName', 'fontSize', 'foreColor', 'backColor','justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull','insertUnorderedList', 'insertOrderedList', 'indent', 'outdent','createLink', 'unlink', 'insertImage', 'insertFile','createTable', 'addColumnLeft', 'addColumnRight', 'addRowAbove', 'addRowBelow', 'deleteRow', 'deleteColumn','formatting','print','pdf'");
            sb.Append("]");
            sb.Append("});");

            sb.Append("$('#editor4').kendoEditor({");
            sb.Append("resizable: { content: true, toolbar: true },");
            sb.Append("tools: [");
            sb.Append("{ name: 'save',   ui: {type:'button',icon: 'save' }, tooltip: 'Save',");
            sb.Append("exec: function(e) {");
            sb.Append("saveHelpEditorL4();");
            sb.Append("}");
            sb.Append("},");
            sb.Append("'bold', 'italic', 'underline', 'strikethrough', 'subscript', 'superscript','fontName', 'fontSize', 'foreColor', 'backColor','justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull','insertUnorderedList', 'insertOrderedList', 'indent', 'outdent','createLink', 'unlink', 'insertImage', 'insertFile','createTable', 'addColumnLeft', 'addColumnRight', 'addRowAbove', 'addRowBelow', 'deleteRow', 'deleteColumn','formatting','print','pdf'");
            sb.Append("]");
            sb.Append("});");


            sb.Append("$('#HelpWindow').kendoWindow({");
            sb.Append("title: \"Help\", modal: false, visible: false,");
            sb.Append("actions: [\"pencil\", \"Minimize\", \"Maximize\", \"Close\"],");
            sb.Append("maxHeight: $(window).height() * 0.8,"); //
            sb.Append("maxWidth: $(window).width() * 0.8,"); //
            sb.Append("minHeight: $(window).height() * 0.2,"); //
            sb.Append("minWidth: $(window).width() * 0.2"); //
            sb.Append("}).data('kendoWindow');");

            sb.Append("$('#HelpWindow').data('kendoWindow').wrapper.find('.k-svg-i-pencil').parent().click(function(e) {");
            sb.Append("getPageHelpNotes(PageControl, getPHNEdit);");
            sb.Append("var ht = $('#HelpWindow').data('kendoWindow').title();");
            sb.Append("$('#HelpWindow').data('kendoWindow').title(ht + \" : Edit Mode\");");
            sb.Append("$('#winCont').hide();");
            sb.Append("$('#EditCont').show();");
            sb.Append("$('#HelpWindow').data('kendoWindow').center();"); //.resize(true);
            sb.Append("});");
            sb.Append(" }, 0, this);");

            return sb.ToString();
        }

        #endregion


        //NOTE: I don't think this code actually works - I am pretty sure it is just a placeholder
        public string getRoleCenterTree(int UserSecControl)
        {
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            string sRet = "";

            LTS.cmMenuTree[] dtoMT = dalSecData.getMenuTreeData(UserSecControl);

            sRet = string.Concat("$('#menuTree').kendoTreeView({",
                " dataUrlField: 'LinksTo',",
                " dataSource: { ",
                "     data: [ ");
            //For each menuItem returned grab the control number and create one section of the string below for each menuItem without a ParentID
            //Inside of each menuItem select the child records using the ParentID and populate the child menu tree
            foreach (LTS.cmMenuTree mt in dtoMT.Where(x => x.MenuTreeParentID == 0 && x.MenuTreeVisible == true).OrderBy(x => x.MenuTreeSequenceNo))
            {
                if (mt.MenuTreeParentID == 0 && mt.MenuTreeVisible == true)
                {
                    //first create the parent string                    
                    sRet += "{text: '" + Utilities.GetLocalizedString(mt.MenuTreeCaption, mt.MenuTreeCaptionLocal, null);

                    if (!String.IsNullOrWhiteSpace(mt.MenuTreeLinkTo))
                    {
                        //sRet += "', LinksTo: '" + mt.MenuTreeLinkTo;
                        sRet += string.Format("', LinksTo: '{0}", mt.MenuTreeLinkTo);
                    }
                    //sRet += "', expanded: " + mt.MenuTreeExpanded.ToString().ToLower();
                    sRet += string.Format("', expanded: {0}", mt.MenuTreeExpanded.ToString().ToLower());

                    bool blnHadItems = false;
                    string strItems = "";
                    System.Text.StringBuilder sbItems = new System.Text.StringBuilder();

                    foreach (LTS.cmMenuTree mtt in dtoMT.Where(x => x.MenuTreeParentID == mt.MenuTreeControl && x.MenuTreeVisible == true).OrderBy(x => x.MenuTreeSequenceNo))
                    {
                        if (mtt != null)
                        {
                            blnHadItems = true;
                            //strItems += "{text: '" + Utilities.GetLocalizedString(mtt.MenuTreeCaption, mtt.MenuTreeCaptionLocal, null);
                            string s = "{text: '" + Utilities.GetLocalizedString(mtt.MenuTreeCaption, mtt.MenuTreeCaptionLocal, null);
                            sbItems.Append(s);

                            if (!String.IsNullOrWhiteSpace(mtt.MenuTreeLinkTo))
                            {
                                //strItems += "', LinksTo: '" + mtt.MenuTreeLinkTo;
                                //strItems += string.Format("', LinksTo: '{0}" , mtt.MenuTreeLinkTo);
                                sbItems.Append(string.Format("', LinksTo: '{0}", mtt.MenuTreeLinkTo));
                            }
                            //strItems += "'},";
                            sbItems.Append("'},");
                            //strItems = sbItems.ToString();
                        }
                        strItems = sbItems.ToString();
                    }
                    if (blnHadItems == true)
                    {
                        sRet += string.Format(", items: [{0}", strItems);
                        //remove the last comma
                        sRet = sRet.Remove(sRet.Length - 1);
                        sRet += "]},";
                    }
                    else
                    {
                        sRet += "},";
                    }
                }
            }
            //remove the last comma
            sRet = sRet.Remove(sRet.Length - 1);
            sRet += "]}, loadOnDemand: false }).data('kendoTreeView'), handleTextBox = function (callback) { return function (e) { if (e.type != 'keypress' || kendo.keys.ENTER == e.keyCode) {callback(e);} }; };";

            //add menu to cache before return
            Models.GenericResult gr = new Models.GenericResult();
            double temp = 0;
            string strExpires = System.Configuration.ConfigurationManager.AppSettings["MenuTreeExpiresMinutes"];
            bool blnTry = double.TryParse(strExpires, out temp);
            double min = temp == 0 ? 30 : temp;

            gr.strField = sRet;
            gr.dtField = DateTime.Now.AddMinutes(min);

            if (Utilities.GlobalMenuTreeByUser.ContainsKey(UserSecControl))
            {
                Utilities.GlobalMenuTreeByUser[UserSecControl] = gr;
            }
            else
            {
                Utilities.GlobalMenuTreeByUser.Add(UserSecControl, gr);
            }


            return sRet;
        }

        #region "Menu Tab"

        /// <summary>
        /// Creates the Navigation Tab for the Action Menu at the top of each page
        /// </summary>
        /// <param name="PageControl"></param>
        /// <param name="UserControl"></param>
        /// <param name="altHelpIcon"></param>
        /// <param name="srcHelpIcon"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo
        ///  Added parameters altHelpIcon and srcHelpIcon and logic to display a different Help Icon if this is a test system
        /// </remarks>
        public string CreateNavigation(int PageControl, int UserControl, string altHelpIcon, string srcHelpIcon)
        {
            LTS.cmPageMenu[] actions = DALcmPageData.GetPageNavigationData(PageControl, UserControl);

            if (actions == null || actions.Count() < 1) { return ""; }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            ////sb.Append("<span style='float:left; display:inline-block;'>");

            foreach (LTS.cmPageMenu a in actions.Where((cmPageMenu x) => x.PageMenuVisible == true).OrderBy(y => y.PageMenuSequenceNo))
            {
                if ((a.PageMenuGroupTypeControl == 10) && (a.PageMenuGroupSubTypeControl == 17))
                {
                    sb.Append("<button id='");
                    sb.Append(a.PageMenuName);
                    sb.Append("'");
                    sb.Append(" class='k-button actionBarButton' type='button'");
                    sb.Append(" onclick='execActionClick(");
                    sb.Append(a.PageMenuName);
                    sb.Append(", ");
                    sb.Append(a.PageMenuProcedureControl);
                    sb.Append(");'>");

                    if (!string.IsNullOrWhiteSpace(a.PageMenuImgSmall))
                    {
                        if (a.PageMenuImgSmall.Trim().StartsWith("../"))
                        {
                            //the image is a url
                            sb.Append("<img class='k-image' alt='' src='");
                            sb.Append(a.PageMenuImgSmall.Trim());
                            sb.Append("' >");
                            if (!string.IsNullOrWhiteSpace(a.PageMenuCaption))
                            {
                                string cap = Utilities.GetLocalizedString(a.PageMenuCaption, a.PageMenuCaptionLocal, null);
                                sb.Append(cap);
                            }
                        }
                        else
                        {
                            //the image is a kendo web font icon
                            sb.Append("<span class='k-icon ");
                            sb.Append(a.PageMenuImgSmall);
                            sb.Append("'></span>");
                            if (!string.IsNullOrWhiteSpace(a.PageMenuCaption))
                            {
                                string cap = Utilities.GetLocalizedString(a.PageMenuCaption, a.PageMenuCaptionLocal, null);
                                sb.Append(cap);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(a.PageMenuCaption))
                        {
                            string cap = Utilities.GetLocalizedString(a.PageMenuCaption, a.PageMenuCaptionLocal, null);
                            sb.Append(cap);
                        }
                    }
                    sb.Append("</button>");
                }
            }
            ////sb.Append("</span>");

            sb.Append("<span style='float:right; display:inline-block;'>");
            sb.Append("<span style='margin:6px; vertical-align: middle;'><a href='Settings.aspx'><img border='0' alt='Settings' src='../Content/NGL/Settings32.png' width='32' height='32'></a></span>");
            sb.Append(string.Format("<span style='margin:6px; vertical-align: middle;'><a href='#' onclick='openHelpWindow();return false;'><img border='0' alt='{0}' src='{1}' ></a></span>", altHelpIcon, srcHelpIcon)); //Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo 
            sb.Append("</span>");

            return sb.ToString();
        }

        /// <summary>
        /// Creates the Reports Tab for the Action Menu at the top of each page
        /// </summary>
        /// <param name="PageControl"></param>
        /// <param name="UserControl"></param>
        /// <param name="altHelpIcon"></param>
        /// <param name="srcHelpIcon"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo
        ///  Added parameters altHelpIcon and srcHelpIcon and logic to display a different Help Icon if this is a test system
        /// </remarks>
        public string CreateReports(int PageControl, int UserControl, string altHelpIcon, string srcHelpIcon)
        {
            LTS.cmPageMenu[] reports = DALcmPageData.GetPageReportData(PageControl, UserControl);
            if (reports == null || reports.Count() < 1) { return ""; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (LTS.cmPageMenu r in reports.OrderBy(y => y.PageMenuSequenceNo))
            {
                if ((r.PageMenuGroupTypeControl == 10) && (r.PageMenuGroupSubTypeControl == 17))
                {
                    sb.Append("<button id='");
                    sb.Append(r.PageMenuName);
                    sb.Append("'");
                    sb.Append(" class='k-button actionBarButton' type='button'");
                    sb.Append(" onclick='execReportClick(");
                    sb.Append(r.PageMenuName);
                    sb.Append(", ");
                    sb.Append(r.PageMenuReportControl);
                    sb.Append(");'>");

                    if (!string.IsNullOrWhiteSpace(r.PageMenuImgSmall))
                    {
                        if (r.PageMenuImgSmall.Trim().StartsWith("../"))
                        {
                            //the image is a url
                            sb.Append("<img class='k-image' alt='' src='");
                            sb.Append(r.PageMenuImgSmall.Trim());
                            sb.Append("' >");
                            if (!string.IsNullOrWhiteSpace(r.PageMenuCaption)) { string cap = Utilities.GetLocalizedString(r.PageMenuCaption, r.PageMenuCaptionLocal, null); sb.Append(cap); }
                        }
                        else
                        {
                            //the image is a kendo web font icon
                            sb.Append("<span class='k-icon ");
                            sb.Append(r.PageMenuImgSmall);
                            sb.Append("'></span>");
                            if (!string.IsNullOrWhiteSpace(r.PageMenuCaption)) { string cap = Utilities.GetLocalizedString(r.PageMenuCaption, r.PageMenuCaptionLocal, null); sb.Append(cap); }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(r.PageMenuCaption)) { string cap = Utilities.GetLocalizedString(r.PageMenuCaption, r.PageMenuCaptionLocal, null); sb.Append(cap); }
                    }
                    sb.Append("</button>");
                }
            }
            sb.Append("<span style='float:right; display:inline-block;'>");
            sb.Append("<span style='margin:6px; vertical-align: middle;'><a href='Settings.aspx'><img border='0' alt='Settings' src='../Content/NGL/Settings32.png' width='32' height='32'></a></span>");
            sb.Append(string.Format("<span style='margin:6px; vertical-align: middle;'><a href='#' onclick='openHelpWindow();return false;'><img border='0' alt='{0}' src='{1}' ></a></span>", altHelpIcon, srcHelpIcon)); //Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo 
            sb.Append("</span>");
            return sb.ToString();
        }

        /// <summary>
        /// Creates the Action Tab for the Action Menu at the top of each page
        /// </summary>
        /// <param name="PageControl"></param>
        /// <param name="UserControl"></param>
        /// <param name="altHelpIcon"></param>
        /// <param name="srcHelpIcon"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo
        ///  Added parameters altHelpIcon and srcHelpIcon and logic to display a different Help Icon if this is a test system
        /// </remarks>
        public string CreateActions(int PageControl, int UserControl, string altHelpIcon, string srcHelpIcon)
        {
            LTS.cmPageMenu[] actions = DALcmPageData.GetPageActionData(PageControl, UserControl);

            if (actions == null || actions.Count() < 1) { return ""; }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            ////sb.Append("<span style='float:left; display:inline-block;'>");

            foreach (LTS.cmPageMenu a in actions.Where((cmPageMenu x) => x.PageMenuVisible == true).OrderBy(y => y.PageMenuSequenceNo))
            {
                if ((a.PageMenuGroupTypeControl == 10) && (a.PageMenuGroupSubTypeControl == 17))
                {
                    sb.Append("<button id='");
                    sb.Append(a.PageMenuName);
                    sb.Append("'");
                    sb.Append(" class='k-button actionBarButton' type='button'");
                    sb.Append(" onclick='execActionClick(");
                    sb.Append(a.PageMenuName);
                    sb.Append(", ");
                    sb.Append(a.PageMenuProcedureControl);
                    sb.Append(");'>");

                    if (!string.IsNullOrWhiteSpace(a.PageMenuImgSmall))
                    {
                        if (a.PageMenuImgSmall.Trim().StartsWith("../"))
                        {
                            //the image is a url
                            sb.Append("<img class='k-image' alt='' src='");
                            sb.Append(a.PageMenuImgSmall.Trim());
                            sb.Append("' >");
                            if (!string.IsNullOrWhiteSpace(a.PageMenuCaption))
                            {
                                string cap = Utilities.GetLocalizedString(a.PageMenuCaption, a.PageMenuCaptionLocal, null);
                                sb.Append(cap);
                            }
                        }
                        else
                        {
                            //the image is a kendo web font icon
                            sb.Append("<span class='k-icon ");
                            sb.Append(a.PageMenuImgSmall);
                            sb.Append("'></span>");
                            if (!string.IsNullOrWhiteSpace(a.PageMenuCaption))
                            {
                                string cap = Utilities.GetLocalizedString(a.PageMenuCaption, a.PageMenuCaptionLocal, null);
                                sb.Append(cap);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(a.PageMenuCaption))
                        {
                            string cap = Utilities.GetLocalizedString(a.PageMenuCaption, a.PageMenuCaptionLocal, null);
                            sb.Append(cap);
                        }
                    }
                    sb.Append("</button>");
                }
            }
            ////sb.Append("</span>");

            sb.Append("<span style='float:right; display:inline-block;'>");
            sb.Append("<span style='margin:6px; vertical-align: middle;'><a href='Settings.aspx'><img border='0' alt='Settings' src='../Content/NGL/Settings32.png' width='32' height='32'></a></span>");
            sb.Append(string.Format("<span style='margin:6px; vertical-align: middle;'><a href='#' onclick='openHelpWindow();return false;'><img border='0' alt='{0}' src='{1}' ></a></span>", altHelpIcon, srcHelpIcon)); //Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo 
            sb.Append("</span>");

            return sb.ToString();
        }

        /// <summary>
        /// Creates the Home Tab for the Action Menu at the top of each page
        /// </summary>
        /// <param name="PageControl"></param>
        /// <param name="UserControl"></param>
        /// <param name="altHelpIcon"></param>
        /// <param name="srcHelpIcon"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo
        ///  Added parameters altHelpIcon and srcHelpIcon and logic to display a different Help Icon if this is a test system
        /// </remarks>
        public string CreateHomeTab(int PageControl, int UserControl, string altHelpIcon, string srcHelpIcon)
        {
            DAL.NGLcmPageData dalPgData = new DAL.NGLcmPageData(Utilities.DALWCFParameters);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            LTS.cmPage pg = dalPgData.GetRecord(PageControl);
            string HomeTabLogo = "";
            string HomeTabHrefURL = "";
            if (!dalPgData.ReadUserPageLogo(PageControl,UserControl,ref HomeTabLogo,ref HomeTabHrefURL)) {
                HomeTabLogo = System.Configuration.ConfigurationManager.AppSettings["HomeTabLogo"];
                 HomeTabHrefURL = System.Configuration.ConfigurationManager.AppSettings["HomeTabHrefURL"];
            }
            
            if (pg == null || string.IsNullOrWhiteSpace(pg.PageCaption))
            {
                sb.Append("<div class='pane-content'>");

                sb.Append("<span style='float:left; display:inline-block;'>");
                sb.Append("<span style='margin:6px; vertical-align: middle;'><a href='Default.aspx'><img border='0' alt='Home' src='../Content/NGL/Home32.png' width='32' height='32'></a></span>");
                //sb.Append("<span style='margin:6px; vertical-align: middle;' ><a href='http://www.nextgeneration.com'><img border='0' alt='NGL' src='../Content/NGL/nextracklogo.GIF' ></a></span>");             
                sb.Append(string.Format("<span style='margin:6px; vertical-align: middle;' ><a href='{0}'><img border='0' alt='NGL' src='{1}' ></a></span>", HomeTabHrefURL, HomeTabLogo));

                sb.Append("</span>");

                sb.Append("<span style='float:right; display:inline-block;'>");
                sb.Append("<span id='WelcomeMessage' style='margin:6px; vertical-align: middle;'></span>");
                sb.Append("<a id='btnSignInOut' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='signInOut();' href='#' style='margin:6px; vertical-align: top;'><span class='k-icon k-i-user' style='vertical-align: middle;'></span><span id='signInText' style='vertical-align: middle;'>Sign In</span></a>");
                sb.Append("<span style='margin:6px; vertical-align: middle;'><a href='Settings.aspx'><img border='0' alt='Settings' src='../Content/NGL/Settings32.png' width='32' height='32'></a></span>");
                sb.Append(string.Format("<span style='margin:6px; vertical-align: middle;'><a href='#' onclick='openHelpWindow();return false;'><img border='0' alt='{0}' src='{1}' ></a></span>", altHelpIcon, srcHelpIcon)); //Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo
                sb.Append("</span>");

                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class='pane-content'><div class='wrap' style='text-align:center'><div style='float: left;'><span style='margin:6px; vertical-align: middle;'><a href='Default.aspx'><img border='0' alt='Home' src='../Content/NGL/Home32.png' width='32' height='32'></a></span>");
                sb.Append(string.Format("<span style='margin:6px; vertical-align: middle;'><a href='{0}'><img border='0' alt='NGL' src='{1}'></a></span></div>", HomeTabHrefURL, HomeTabLogo));
                sb.Append("<div style='float: right;'><span id='WelcomeMessage' style='margin:6px; vertical-align: middle;'></span><a id='btnSignInOut' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='signInOut();' href='#' style='margin:6px; vertical-align: top;'><span class='k-icon k-i-user' style='vertical-align: middle;'></span><span id='signInText' style='vertical-align: middle;'>Sign In</span></a>");
                sb.Append(string.Format("<span style='margin:6px; vertical-align: middle;'><a href='Settings.aspx'><img border='0' alt='Settings' src='../Content/NGL/Settings32.png' width='32' height='32'></a></span><span style='margin:6px; vertical-align: middle;'><a href='#' onclick='openHelpWindow();return false;'><img border='0' alt='{0}' src='{1}'></a></span></div>", altHelpIcon, srcHelpIcon)); //Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo
                sb.Append("<div style='margin:0 auto !important; display:inline-block'><span><h2 style='-webkit-margin-before: 0.50em; -webkit-margin-after: 0.50em;'>");
                sb.Append(pg.PageCaption);
                sb.Append("</h2></span></div></div></div>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates the Action Menu at the top of each page (Home, Action, Reports, and Navigation Tabs)
        /// </summary>
        /// <param name="PageControl"></param>
        /// <param name="UserControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo
        ///  Added logic to display a different Help Icon if this is a test system
        /// </remarks>
        public string CreateMenuTabStrip(int PageControl, int UserControl)
        {
            //Modified By LVV on 6/25/20 for v-8.2.1.008 Task #202006091213 - Add logic to D365 web config for help logo
            string HelpTestIcon = System.Configuration.ConfigurationManager.AppSettings["HelpTestIcon"];
            string strIsTest = System.Configuration.ConfigurationManager.AppSettings["IsTest"];
            string srcHelpIcon = "../Content/NGL/Help32.png";
            string altHelpIcon = "Help";
            if (!string.IsNullOrWhiteSpace(strIsTest) && (strIsTest.ToUpper() == "TRUE" || strIsTest.ToUpper() == "T" || strIsTest == "1"))
            {
                if (!string.IsNullOrWhiteSpace(HelpTestIcon)) { srcHelpIcon = HelpTestIcon; }
                altHelpIcon = "Help (Test)";
            }

            string tabHome = CreateHomeTab(PageControl, UserControl, altHelpIcon, srcHelpIcon);
            string tabActions = CreateActions(PageControl, UserControl, altHelpIcon, srcHelpIcon);
            string tabReports = CreateReports(PageControl, UserControl, altHelpIcon, srcHelpIcon);
            string tabNavigation = CreateNavigation(PageControl, UserControl, altHelpIcon, srcHelpIcon);

            bool blnHasActions = false;
            if (!string.IsNullOrWhiteSpace(tabActions)) { blnHasActions = true; }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append(" setTimeout(function () { ");
            sb.Append("$('#tab').kendoTabStrip({");
            sb.Append("animation: { open: { effects: 'fadeIn'} },");
            sb.Append("dataTextField: 'text',");
            sb.Append("dataContentField: 'content',");
            sb.Append("dataSource: [");
            sb.Append("{ text: 'Home', content: \"");
            sb.Append(tabHome);
            sb.Append("\"}");
            if (!string.IsNullOrWhiteSpace(tabActions))
            {
                sb.Append(",");
                sb.Append("{ text: 'Actions', content: \"");
                sb.Append(tabActions);
                sb.Append("\"}");
            }
            if (!string.IsNullOrWhiteSpace(tabNavigation))
            {
                sb.Append(",");
                sb.Append("{ text: 'Navigation', content: \"");
                sb.Append(tabNavigation);
                sb.Append("\"}");
            }
            if (!string.IsNullOrWhiteSpace(tabReports))
            {
                sb.Append(",");
                sb.Append("{ text: 'Reports', content: \"");
                sb.Append(tabReports);
                sb.Append("\"}");
            }
            sb.Append("]");
            //If Actions Tab exists select that tab to have focus. If Actions Tab does not exist for this page, the Home Tab will have focus
            if (blnHasActions) { sb.Append("}).data('kendoTabStrip').select(1);"); } else { sb.Append("}).data('kendoTabStrip').select(0);"); }
            //sb.Append(" }, 10,this);");  
            sb.Append(" if ( localStorage.SignedIn == 't'){ var divWelcome = document.getElementById('WelcomeMessage'); if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {  divWelcome.innerHTML = 'Welcome ' + localStorage.NGLvar1455; }  var signInMsg = document.getElementById('signInText'); if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) {signInMsg.innerHTML = 'Sign Out';}}");
            return sb.ToString();
        }

        #endregion

        #region "Page Footer"

        /// <summary>
        /// Populate each page’s footer data using the message from cmPage
        /// </summary>
        /// <param name="PageControl"></param>
        /// <param name="UserControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
        ///   Modified the content management logic used to populate each page’s footer data to read the message from cmPage
        /// </remarks>
        public string CreatePageFooter(int PageControl, int UserControl)
        {
            DAL.NGLcmPageData oPg = new DAL.NGLcmPageData(Utilities.DALWCFParameters);
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);

            if (PageControl != this.PageControl) { PageControl = this.PageControl; } //When this method is called from the get of PageFooterHTML the parameters are always going to be set to 0 so we need to get them from the page
            if (UserControl != this.UserControl) { UserControl = this.UserControl; } //When this method is called from the get of PageFooterHTML the parameters are always going to be set to 0 so we need to get them from the page

            string strFooter = ""; //return value
            string strDefault = ""; //default HTML structure   
            string strPgFooterMsg = ""; //message from database for the page
            var ltsPg = oPg.GetRecord(PageControl);
            if (ltsPg != null) { strPgFooterMsg = ltsPg.PageFooterMsg; }
            if (string.IsNullOrWhiteSpace(strPgFooterMsg)) { strPgFooterMsg = ""; } //if ltsPg.PageFooterMsg returns null we want to set it to ""

            string strContent = System.Configuration.ConfigurationManager.AppSettings["FooterContent"];
            if (string.IsNullOrWhiteSpace(strContent)) { strContent = ""; }

            strDefault = string.Format("<div><span><p>{0} {1}</p></span></div>", strContent, strPgFooterMsg);
            strFooter = strDefault;

            //if the user is a Free Trial User we need to change the message to a special case
            if (UserControl != 0)
            {
                DTO.tblUserSecurity usec = dalSecData.GettblUserSecurity(UserControl);
                if (usec != null)
                {
                    if (string.Equals(usec.UserGroupsName, "Free Trial"))
                    {
                        var html = "<div style='text-align:center;'><span><strong style='color: red;'>{0}</strong></br></span>{1}</span></div>";
                        var strMsg = "This secure site exists to provide On-Line Rate Shopping information. If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a>";
                        var strExpired = "Free Trial Expired! Sign Up to continue to use the software";
                        if (usec.UserFreeTrialActive && ((usec.UserEndFreeTrial.HasValue) && (usec.UserEndFreeTrial.Value > DateTime.Now)))
                        {
                            TimeSpan t = usec.UserEndFreeTrial.Value.Subtract(DateTime.Now);
                            var strExpiresIn = "Free Trial Expires in " + t.Days.ToString() + " days";
                            strFooter = string.Format(html, strExpiresIn, strMsg);
                        }
                        else { strFooter = string.Format(html, strExpired, strMsg); } //expired
                    }
                }
            }
            return strFooter;
        }

        //Depreciated By LVV on 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
        //we now use the above version of CreatePageFooter()
        public string CreatePageFooterOld(int PageControl, int UserControl)
        {
            string strFooter = "";
            string strDefault = "";

            string strContent = System.Configuration.ConfigurationManager.AppSettings["FooterContent"];

            if (string.IsNullOrWhiteSpace(strContent))
            {
                strContent = "This secure site exists to provide On-Line Rate Shopping information. If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a>";
            }

            strDefault = string.Format("<div><span><p>{0}</p></span></div>", strContent);

            if (UserControl == 0)
            {
                return strDefault;
            }

            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
            DTO.tblUserSecurity usec = dalSecData.GettblUserSecurity(UserControl);


            if (string.Equals(usec.UserGroupsName, "Free Trial"))
            {
                if (usec.UserFreeTrialActive && ((usec.UserEndFreeTrial.HasValue) && (usec.UserEndFreeTrial.Value > DateTime.Now)))
                {
                    TimeSpan t = usec.UserEndFreeTrial.Value.Subtract(DateTime.Now);
                    //if (t.Days < 14)
                    //{
                    ////string s = "<div><span style='text-align:center;color: red;'><p><strong>Free Trial Expires in " + t.Days.ToString() + " days</strong></p></span></div>";
                    ////strFooter = s + strDefault;
                    //}

                    strFooter = "<div style='text-align:center;'><span><strong style='color: red;'>Free Trial Expires in " + t.Days.ToString() + " days</strong></br></span>This secure site exists to provide On-Line Rate Shopping information. If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></span></div>";

                }
                else
                {
                    //expired
                    ////string s = "<div><span style='text-align:center;color: red;'><p><strong>Free Trial Expired! Sign Up to continue to use the software</strong></p></span></div>";
                    ////strFooter = s + strDefault;
                    strFooter = "<div style='text-align:center;'><span><strong style='color: red;'>Free Trial Expired! Sign Up to continue to use the software</strong></br></span>This secure site exists to provide On-Line Rate Shopping information. If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></span></div>";
                }

            }
            else
            {
                strFooter = strDefault;
            }

            return strFooter;
        }

        public string createFullPageFooter(LTS.cmPageDetail container, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 45) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<div style='position:relative; clear:both; float:none; display:inline-block; margin:10px;' id='{0}' >{1}", container.PageDetWidth, System.Environment.NewLine));
            sb.Append(string.Format("<hr />{0}", System.Environment.NewLine));
            sb.Append(string.Format("<div style='margin:5px,5px,5px,5px; padding:5px,5px,5px,5px; border:solid  #7bd2f6 2px; background-color: #7bd2f6; border-radius: 10px;' >{0}", System.Environment.NewLine));
            sb.Append(CreatePageFooter(this.PageControl, this.UserControl));
            sb.Append(string.Format("</ div>{0}", System.Environment.NewLine));
            sb.Append(string.Format("<br />{0}", System.Environment.NewLine));
            sb.Append(string.Format("</ div>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }

        #endregion


        /// <summary>
        /// Generates the HTML needed to dispay a kendo icon button with an image and button text
        /// </summary>
        /// <param name="container"></param>
        /// <param name="sDetails"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.0 on 7/25/2017 
        ///   on click goes in metadata for container -- 41 - Kendo Icon Button 
        ///   Button text goes in caption for container -- 41 - Kendo Icon Button
        ///   icon image goes in metadata for -- 42 - K-Icon child
        ///   The caption for 42 - K-Icon is not used
        /// </remarks>
        public string createKendoIconButton(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetGroupSubTypeControl != 41 || container.PageDetVisible == false) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<span><a id='{0}' class='k-button' onclick='{1}' href='#'>{2}", container.PageDetName, container.PageDetMetaData, System.Environment.NewLine));
            bool blnIconFound = false;
            if (cmDetails == null || cmDetails.Count() < 1) { return sHTML; }
            List<LTS.cmPageDetail> cmChildren = cmDetails.Where(x => x.PageDetParentID == container.PageDetControl).OrderBy(x => x.PageDetSequenceNo).ToList();
            if (cmChildren != null && cmChildren.Count() > 0)
            {
                foreach (LTS.cmPageDetail det in cmChildren)
                {
                    if (det.PageDetGroupSubTypeControl == 42 && det.PageDetVisible == true)
                    {
                        blnIconFound = true;
                        sb.Append(string.Format("<span class='{0}'></span>{1}</a>{2}", det.PageDetMetaData, Utilities.GetLocalizedString(container.PageDetCaption, container.PageDetCaptionLocal, Culture), System.Environment.NewLine));
                        break;
                    }
                }
            }
            if (!blnIconFound)
            {
                sb.Append(string.Format("<span class='k-icon k-i-window'></span>{0}</a>{1}", Utilities.GetLocalizedString(container.PageDetCaption, container.PageDetCaptionLocal, Culture), System.Environment.NewLine));
            }
            sb.Append(string.Format("</span>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }


        public string createStandardBorder(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 26) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<div class='ngl-blueBorder'>{0}", System.Environment.NewLine));
            //append any children
            sb.Append(appendContainerDetails(container, cmDetails, Culture));
            //close the container
            sb.Append(string.Format("</div>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }

        public string createFullPageBorder(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 27) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<div class='ngl-blueBorderFullPage' style='min-width:{0}px; '>{0}", container.PageDetWidth, System.Environment.NewLine));
            //append any children
            sb.Append(appendContainerDetails(container, cmDetails, Culture));
            //close the container
            sb.Append(string.Format("</div>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRecods"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.0 on 7/25/2017
        ///   generates a multi-row two column table with labels and data entry fields for each 
        /// </remarks>
        public string createDataEntryFormTable(List<LTS.cmPageDetail> sDetails, int width, string metaData = "", object Culture = null)
        {
            string sTable = "";
            if (sDetails == null || sDetails.Count() < 1) { return sTable; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<table style='border:none; width:{0}px; {1}'>{2}", width, metaData, System.Environment.NewLine));

            foreach (LTS.cmPageDetail det in sDetails)
            {

                if (det == null || string.IsNullOrWhiteSpace(det.PageDetName) || det.PageDetVisible == false) { break; }
                bool blnSubTypeSupported = true;
                string sEditorHTML = string.Format("<input id='{0}' {1} />", det.PageDetName, det.PageDetMetaData);
                switch (det.PageDetGroupSubTypeControl)
                {
                    case 0:
                        break;
                    case 6: //AutoComplete
                        PageReadyJS += createkendoAutoComplete(det);
                        break;
                    case 7: //ColorPicker
                        PageReadyJS += createkendoColorPicker(det);
                        break;
                    case 8: //ComboBox
                        PageReadyJS += createkendoComboBox(det);
                        break;
                    case 9: //DatePicker
                        PageReadyJS += createkendoDatePicker(det);
                        break;
                    case 10: //DateTimePicker
                        PageReadyJS += createkendoDateTimePicker(det);
                        break;
                    case 11: //DropDownList
                        PageReadyJS += createkendoDropDownList(det);
                        break;
                    case 13: //MaskedTextBox
                        PageReadyJS += createkendoMaskedTextBox(det);
                        break;
                    case 15: //NumericTextBox
                        PageReadyJS += createkendoNumericTextBox(det);
                        break;
                    default:
                        blnSubTypeSupported = false;
                        break;
                }
                if (blnSubTypeSupported)
                {
                    sb.Append(string.Format("<tr style='border: none;' >{0}", System.Environment.NewLine));
                    string sTDWidth = "";
                    if (det.PageDetWidth > 0) { sTDWidth = string.Format("width='{0}' ", det.PageDetWidth); }
                    sb.Append(string.Format("<td style='border:none;' {2} >{0}:</td>{1}", Utilities.GetLocalizedString(det.PageDetCaption, det.PageDetCaptionLocal, Culture), System.Environment.NewLine, sTDWidth));
                    sb.Append(string.Format("<td style='border:none;' {2} >{0}</td>{1}", sEditorHTML, System.Environment.NewLine, sTDWidth));
                    sb.Append(string.Format("</tr>{0}", System.Environment.NewLine));
                }
            }
            sb.Append("</table>");
            sTable = sb.ToString();
            return sTable;
        }

        public string createDataEntryFormTable(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 29) { return sHTML; }
            if (cmDetails == null || cmDetails.Count() < 1) { return sHTML; }
            List<LTS.cmPageDetail> tblDetails = cmDetails.Where(x => x.PageDetParentID == container.PageDetPageControl).OrderBy(x => x.PageDetSequenceNo).ToList();
            if (tblDetails == null || tblDetails.Count() < 1) { return sHTML; }
            sHTML = createDataEntryFormTable(tblDetails, container.PageDetWidth, container.PageDetMetaData, Culture);
            return sHTML;
        }
        public string createDataEntryFormTableSample(int pgControl)
        {
            string sTable = "";
            List<LTS.cmPageDetail> sRecods = new List<LTS.cmPageDetail>();
            LTS.cmPageDetail uRow = new LTS.cmPageDetail();
            uRow.PageDetPageControl = pgControl;
            uRow.PageDetGroupTypeControl = 2;
            uRow.PageDetGroupSubTypeControl = 13;
            uRow.PageDetCaption = "User Name";
            uRow.PageDetName = "txtNGLUserName";
            uRow.PageDetVisible = true;
            sRecods.Add(uRow);
            LTS.cmPageDetail pRow = new LTS.cmPageDetail();
            pRow.PageDetPageControl = pgControl;
            pRow.PageDetGroupTypeControl = 2;
            pRow.PageDetGroupSubTypeControl = 13;
            pRow.PageDetCaption = "Password";
            pRow.PageDetName = "txtNGLPass";
            pRow.PageDetVisible = true;
            pRow.PageDetMetaData = " type='password' ";
            sRecods.Add(pRow);

            sTable = createDataEntryFormTable(sRecods, 249);

            return sTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="cmDetails"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        /// <remarks>
        /// created
        ///   width is controled in the metadata
        /// </remarks>
        public string createFloatBlockLeft(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 28) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<div style='position:relative; float:left; display:inline-block; {0}' >{1}", container.PageDetMetaData, System.Environment.NewLine));
            //append any children
            sb.Append(appendContainerDetails(container, cmDetails, Culture));
            //close the container
            sb.Append(string.Format("</ div>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }
        public string createFloatBlockLeftSample(int pgControl)
        {
            string sHTML = "";

            return sHTML;
        }

        public string createHeaderTag(string sTag, LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl < 31 || container.PageDetGroupSubTypeControl > 34) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<{0} style='{2}' >{1}", sTag, Utilities.GetLocalizedString(container.PageDetCaption, container.PageDetCaptionLocal, Culture), container.PageDetMetaData));
            //append any children
            sb.Append(appendContainerDetails(container, cmDetails, Culture));
            //close the container
            sb.Append(string.Format("</ {0}>{1}", sTag, System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }

        public string createParagraph(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 37) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<p>{1}{0}{1}", container.PageDetMetaData, System.Environment.NewLine));
            //append any children
            sb.Append(appendContainerDetails(container, cmDetails, Culture));
            //close the container
            sb.Append(string.Format("</ p>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }

        public string createLink(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 39) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<a id='{0}' href='{1}'>{2}", container.PageDetName, container.PageDetMetaData, Utilities.GetLocalizedString(container.PageDetCaption, container.PageDetCaptionLocal, Culture)));
            //append any children
            sb.Append(appendContainerDetails(container, cmDetails, Culture));
            //close the container
            sb.Append(string.Format("</ a>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            //< div style = "position:relative; float:left; display:inline-block; width:calc(100% - 250px);  margin-top:10px; " >

            //                     < p >< a id = "lnkForgotPassword" href = "ForgotPassword.aspx" >< img id = "imgForgotPassword" src = "Content/NGL/ForgotPassword.gif" /></ a ></ p >


            //                                < p >< a id = "lnkRequestAccount" href = "RequestAnAccount.aspx" >< img id = "imgRequestAccount" src = "Content/NGL/RequestAccount.gif" /></ a ></ p >


            //                                           < p >< a id = "lnkBackToPublicSite" href = "http://www.nextgeneration.com" >< img id = "imgBackToPublicSite" src = "Content/NGL/BackToPublicSite.gif" /></ a >

            //                                                  </div>
            return sHTML;
        }


        public string createSpan(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 36) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<span style='{1}' >{0}'", Utilities.GetLocalizedString(container.PageDetCaption, container.PageDetCaptionLocal, Culture), container.PageDetMetaData));
            //append any children
            sb.Append(appendContainerDetails(container, cmDetails, Culture));
            //close the container
            sb.Append(string.Format("</span>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }

        public string createDiv(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false || container.PageDetGroupSubTypeControl != 35) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("<div style='clear:both; {1}' >{2}{0}'", Utilities.GetLocalizedString(container.PageDetCaption, container.PageDetCaptionLocal, Culture), container.PageDetMetaData));
            //append any children
            sb.Append(appendContainerDetails(container, cmDetails, Culture));
            //close the container
            sb.Append(string.Format("</div>{0}", System.Environment.NewLine));
            sHTML = sb.ToString();
            return sHTML;
        }

        public string appendContainerDetails(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetVisible == false) { return sHTML; }
            if (cmDetails == null || cmDetails.Count() < 1) { return sHTML; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<LTS.cmPageDetail> cmChildren = cmDetails.Where(x => x.PageDetParentID == container.PageDetControl).OrderBy(x => x.PageDetSequenceNo).ToList();
            if (cmChildren != null && cmChildren.Count() > 0)
            {
                foreach (LTS.cmPageDetail det in cmChildren)
                {
                    if (det == null || string.IsNullOrWhiteSpace(det.PageDetName) || det.PageDetVisible == false) { break; }
                    string sEditorHTML = string.Format("<input id='{0}' />", det.PageDetName);
                    switch (det.PageDetGroupSubTypeControl)
                    {
                        case 6:
                            sb.Append(sEditorHTML + System.Environment.NewLine);
                            PageReadyJS += createkendoAutoComplete(det);
                            break;
                        case 7:
                            sb.Append(sEditorHTML + System.Environment.NewLine);
                            PageReadyJS += createkendoColorPicker(det);
                            break;
                        case 8:
                            sb.Append(sEditorHTML + System.Environment.NewLine);
                            PageReadyJS += createkendoComboBox(det);
                            break;
                        case 9:
                            sb.Append(sEditorHTML + System.Environment.NewLine);
                            PageReadyJS += createkendoDatePicker(det);
                            break;
                        case 10:
                            sb.Append(sEditorHTML + System.Environment.NewLine);
                            PageReadyJS += createkendoDateTimePicker(det);
                            break;
                        case 11:
                            sb.Append(sEditorHTML + System.Environment.NewLine);
                            PageReadyJS += createkendoDropDownList(det);
                            break;
                        //case 12:  // TODO: Need to add code to support HTML Editor
                        //    sb.Append(sEditorHTML + System.Environment.NewLine);
                        //    PageReadyJS += createkendoDropDownList(det);
                        //    break;
                        case 13:
                            //Note: createkendoMaskedTextBox requires cmElementField.ElmtFieldFormat
                            sb.Append(sEditorHTML + System.Environment.NewLine);
                            PageReadyJS += createkendoMaskedTextBox(det);
                            break;
                        //case 14: // TODO: Need to add code to support MultiSelect list
                        //    sb.Append(sEditorHTML + System.Environment.NewLine);
                        //    PageReadyJS += createkendoMaskedTextBox(det);
                        //    break;
                        case 15:
                            sb.Append(sEditorHTML + System.Environment.NewLine);
                            PageReadyJS += createkendoNumericTextBox(det);
                            break;
                        case 17:
                            sb.Append(string.Format("<button id='{0}' onclick='{1}'> {2} </button>{3}", det.PageDetName, det.PageDetMetaData, Utilities.GetLocalizedString(det.PageDetCaption, det.PageDetCaptionLocal, Culture), System.Environment.NewLine));
                            PageReadyJS += createkendoButton(det);
                            break;
                        // items from 18 to 25 are skipped for now until we have a way to test these nested containers
                        case 26:
                            sb.Append(createStandardBorder(det, cmDetails, Culture));
                            break;
                        case 27:
                            sb.Append(createFullPageBorder(det, cmDetails, Culture));
                            break;
                        case 28: //Float Block Left includes nested objects so this call appendContainerDetails recursively                            
                            sb.Append(createFloatBlockLeft(det, cmDetails, Culture));
                            break;
                        case 29: //DataEntryFormTable                            
                            sb.Append(createDataEntryFormTable(det, cmDetails, Culture));
                            break;
                        case 30:
                            sb.Append("<hr />");
                            break;
                        case 31:
                            sb.Append(createHeaderTag("h1", det, cmDetails, Culture));
                            break;
                        case 32:
                            sb.Append(createHeaderTag("h2", det, cmDetails, Culture));
                            break;
                        case 33:
                            sb.Append(createHeaderTag("h3", det, cmDetails, Culture));
                            break;
                        case 34:
                            sb.Append(createHeaderTag("h4", det, cmDetails, Culture));
                            break;
                        case 35:
                            sb.Append(createDiv(det, cmDetails, Culture));
                            break;
                        case 36:
                            sb.Append(createSpan(det, cmDetails, Culture));
                            break;
                        case 37:
                            sb.Append(createParagraph(det, cmDetails, Culture));
                            break;
                        case 38:
                            sb.Append(string.Format("<img id='{0}' src='{1}' />", det.PageDetName, det.PageDetMetaData));
                            break;
                        case 39:
                            sb.Append(createLink(det, cmDetails, Culture));
                            break;
                        case 41:
                            sb.Append(createKendoIconButton(det, cmDetails, Culture));
                            break;
                        case 43:
                            sb.Append("&nbsp;");
                            break;
                        case 44:
                            sb.Append("<br />");
                            break;
                        case 45:
                            sb.Append(createFullPageFooter(det, Culture));
                            break;
                        default:
                            break;
                    }
                }
            }

            sHTML = sb.ToString();
            return sHTML;

        }
        /// <summary>
        /// Test Method -- Generate HTML if container Group Type is 14 (HTML Content) or retuns an empty string
        /// </summary>
        /// <param name="container"></param>
        /// <param name="cmDetails"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.0 on 7/25/2017
        ///   This method assumes that the container is a cmPageDetail record where the Group Type is 14-HTML Content.
        ///   It generates the HTML markup for itself and all dependent child details and returns the results as a string
        ///   if the Group Type is not 14 an empty string is returned
        /// </remarks>
        public string createHTMLContainer(LTS.cmPageDetail container, List<LTS.cmPageDetail> cmDetails, object Culture = null)
        {
            string sHTML = "";
            if (container == null || container.PageDetGroupTypeControl != 14 || container.PageDetVisible == false) { return sHTML; }
            switch (container.PageDetGroupSubTypeControl)
            {
                case 29: //Two Column Data Table
                    sHTML = createDataEntryFormTable(container, cmDetails, Culture);
                    break;
                case 30: //Left Floating Div with No Border 
                    sHTML = createFloatBlockLeft(container, cmDetails, Culture);
                    break;
                case 31: //H1 container
                    sHTML = createHeaderTag("h1", container, cmDetails, Culture);
                    break;
                case 32: //H2 container
                    sHTML = createHeaderTag("h2", container, cmDetails, Culture);
                    break;
                case 33: //H3 container
                    sHTML = createHeaderTag("h3", container, cmDetails, Culture);
                    break;
                case 34: //H4 container
                    sHTML = createHeaderTag("h4", container, cmDetails, Culture);
                    break;
                case 35: //Standard Div container
                    sHTML = createDiv(container, cmDetails, Culture);
                    break;
                case 36: //Standard Span container
                    sHTML = createSpan(container, cmDetails, Culture);
                    break;
                case 37: //Standard Paragraph container
                    sHTML = createParagraph(container, cmDetails, Culture);
                    break;
                default:
                    //Console.WriteLine("Default case");
                    break;
            }
            return sHTML;
        }

        public string getJavaScriptDataType(int DataTypeControl)
        {
            string sRet = "string";
            switch (DataTypeControl)
            {
                case 1: //   bigint
                case 9: //   decimal
                case 10: //  float
                case 13: //  hierarchyid
                case 15: //  int
                case 16: //  money
                case 19: //  numeric
                case 21: //  real
                case 23: //  smallint
                case 24: //  smallmoney
                case 30: //  tinyint
                    sRet = "number";
                    break;
                case 3: //   bit
                    sRet = "bool";
                    break;
                case 5: //   date
                case 6: //   datetime
                case 7: //   datetime2
                case 8: //   datetimeoffset
                case 22: //  smalldatetime
                case 28: //  time
                    sRet = "date";
                    break;
                default:
                    sRet = "string";
                    break;
            }
            return sRet;
        }

        private DAL.NGLcmPageData _DALcmPageData;
        public DAL.NGLcmPageData DALcmPageData
        {
            get
            {
                if (_DALcmPageData == null)
                {
                    _DALcmPageData = new DAL.NGLcmPageData(Utilities.DALWCFParameters);
                }
                return _DALcmPageData;
            }
            set
            {
                _DALcmPageData = value;
            }
        }

        public bool ValidatePageItem(string sItemName, bool blnAddifMissing)
        {
            return DALcmPageData.isPageItemAllowed(sItemName, UserControl, blnAddifMissing);

        }

        /// <summary>
        /// If the user has a set up for the Bing Maps SSOA then return the url for the javascript map control
        /// including the correct key. Else return an empty string - we just won't write the script to the page
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Added By LVV on 9/24/19 Bing Maps
        /// </remarks>
        public string getBingMapsJS()
        {
            string strRet = "";
            DAL.NGLtblSingleSignOnAccountData oSec = new DAL.NGLtblSingleSignOnAccountData(Parameters);
            DTO.WCFResults[] SSOA = oSec.GetSingleSignOnAccountByUser(UserControl, DAL.Utilities.SSOAAccount.BingMaps);
            if (SSOA?.Length > 0)
            {
                //ignore any errors - if it is not returned then we return ""
                string url = "", key = "";
                if (SSOA[0].KeyFields?.Count > 0)
                {
                    url = SSOA[0].KeyFields["SSOALoginURL"];
                    key = SSOA[0].KeyFields["RefID"];
                    if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(key))
                    {
                        //strRet = string.Format("{0}mapcontrol?callback=GetMap&key={1}", url, key);
                        strRet = string.Format("<script type='text/javascript' src='{0}mapcontrol?callback=GetMap&key={1}' async defer></script>", url, key);
                    }
                }
            }
            return strRet;
        }

        public string getTrimbleAPIKey()
        {
            string strRet = "TrimbleAPIKey = 'NA'; \n";
            DAL.NGLtblSingleSignOnAccountData oSec = new DAL.NGLtblSingleSignOnAccountData(Parameters);
            DTO.WCFResults[] SSOA = oSec.GetSingleSignOnAccountByUser(UserControl, DAL.Utilities.SSOAAccount.Trimble);
            if (SSOA?.Length > 0)
            {
                //ignore any errors - if it is not returned then we return ""
                string url = "", key = "";
                if (SSOA[0].KeyFields?.Count > 0)
                {
                    url = SSOA[0].KeyFields["SSOALoginURL"];
                    key = SSOA[0].KeyFields["RefID"];
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        strRet = "TrimbleAPIKey = '" + key + "'; \n";
                    }
                }
            }
            return strRet;
        }

        public string ParseDEFileTypeMetaData(string sMetaData) 
        {
            string sRetVal = "-1";
            try
            {
                //expected sMetaData = value "defiletype:1";
                sMetaData = sMetaData.ToLower();
                if (!string.IsNullOrEmpty(sMetaData) && sMetaData.Contains("defiletype"))
                {
                    string[] sSplitArray = new string[] { "defiletype" };
                    string[] sValues = sMetaData.Split(sSplitArray, 10, StringSplitOptions.RemoveEmptyEntries);
                    if (sValues != null && sValues.Count() > 0 && !string.IsNullOrEmpty(sValues[0]))
                    {
                        string[] sKeys = sValues[0].Split(':');
                        if (sKeys != null && sKeys.Count() > 0) { sRetVal = sKeys[1]; }
                    }
                }

            }
            catch (Exception ex)
            {
                //do nothig just return the default value -1
            }

            return sRetVal;
        }


        public DTO.tblDataEntryField[] GettblDataEntryFieldsFiltered(int DEFileType ){

            DTO.tblDataEntryField[] arrRet = new DTO.tblDataEntryField[] { };
            try
            {
                DAL.NGLtblDataEntryFieldData oData = new DAL.NGLtblDataEntryFieldData(Parameters);
                arrRet = oData.GettblDataEntryFieldsFiltered(DEFileType);
               
            }
            catch (Exception ex)
            {
                //do nothig just return an empty array the caller will use the default information when blank
            }
            return arrRet;


        }

    }
}