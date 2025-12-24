using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using CM = DynamicsTMS365.ContentManagement;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;
using System.Net;

namespace DynamicsTMS365
{
    public partial class ViewReport : NGLWebUIBaseClass
    {

       public string _sReportPath = "/" +  System.Configuration.ConfigurationManager.AppSettings["ReportsStdPath"] + "/CAR007-Carrier Master Address";
       public string _sReportServerUrl = System.Configuration.ConfigurationManager.AppSettings["ReportsServerUrl"];

        public string sReportPath {
            get { return _sReportPath; }
            set { _sReportPath = value; }
        }

        public string sReportServerUrl
        {
            get { return _sReportServerUrl; }
            set { _sReportServerUrl = value; }
        }
        public string _sServerReport = "< ServerReport ReportPath = \"/FMStdReports/CAR054-Carrier Dropped Loads\" ReportServerUrl=\"http://nglrdp07d/ReportServer/\" />";
        public string sServerReport
        {
            get { return _sServerReport; }
            set { _sServerReport = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //System.Diagnostics.Debug.WriteLine("Page_Load" + " IsPostBack = " + IsPostBack);
                refreshUserControl();
                CM.cmPageBuilder pg = new CM.cmPageBuilder();
                UserGroupCategory = this.SSOR.CatControl;
                pg.UserGroupCategory = UserGroupCategory;
                pg.UserControl = UserControl;
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
            }
        }

        protected void ReportViewer1_Init(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ReportViewer1_Init" + " IsPostBack = " + IsPostBack);
            string reportName = "CAR007-Carrier Master Address";
            string reportControl = "146";
            string refreshReport = "";
            refreshReport = Request.QueryString["refreshReport"];
            if (!string.IsNullOrWhiteSpace(refreshReport) && refreshReport == "true")
            {
                //Make sure we got all the stuff we need to create the new ReportViewer
                reportName = Request.QueryString["reportname"];
                reportControl = Request.QueryString["reportcontrol"];
            } 
                if (string.IsNullOrWhiteSpace(reportName) || string.IsNullOrWhiteSpace(reportControl)) { return; } //if either report name or control is null return because we can't do anything
                int control = 0;
                if (int.TryParse(reportControl, out control) == false) { return; } //if we cannot parse the value of reportControl we can't do anything so return
                if (control == 0) { return; } //if reportControl is 0 we can't do anything so return
                string url = System.Configuration.ConfigurationManager.AppSettings["ReportsServerUrl"];
                string customPath = System.Configuration.ConfigurationManager.AppSettings["ReportsCustomPath"]; //custom reports
                string standardPath = System.Configuration.ConfigurationManager.AppSettings["ReportsStdPath"]; //standard reports
                if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(customPath) || string.IsNullOrWhiteSpace(standardPath)) { return; } //if any of these are null return because we can't do anything

                //System.Diagnostics.Debug.WriteLine(" ** Reset **" + " IsPostBack = " + IsPostBack);
                ReportViewer1.Reset();
                try
                {
                    //Set the processing mode for the ReportViewer to Remote
                    ReportViewer1.ProcessingMode = ProcessingMode.Remote;
                    ReportViewer1.SizeToReportContent = true;

                    //Set the report server URL and report path
                    string path = "";
                    if (control >= 1000) { path = string.Format("/{0}/{1}", customPath, reportName); } else { path = string.Format("/{0}/{1}", standardPath, reportName); }
                    ReportViewer1.ServerReport.ReportServerUrl = new Uri(url);
                    ReportViewer1.ServerReport.ReportPath = path;

                    //Get the credentials if authentication is required
                    DAL.NGLParameterData dalData = new DAL.NGLParameterData(Utilities.DALWCFParameters);                   
                    string userName = dalData.GetParText("ReportServerUser"); //string userName = "NGLServiceAct";
                    string password = dalData.GetParText("ReportServerPass"); //string password = "NGLService2018#";
                    string domain = dalData.GetParText("ReportServerDomain"); //string domain = "NGL";                                                           
                    ReportServerCredentials c = new ReportServerCredentials(userName, password, domain);
                    ReportViewer1.ServerReport.ReportServerCredentials = c;

                    //Set the UserName parameter if the report is a Standard Report
                    string un = Request.QueryString["UN"];
                    ReportParameter userParameter = new ReportParameter("UserName", un, false);
                    if (control < 1000) { ReportViewer1.ServerReport.SetParameters(userParameter); }
                    if (Request.QueryString["PAR1"] != null && Request.QueryString["PAR1Val"] != null)
                    {
                        string par1 = Request.QueryString["PAR1"];
                        string par1val = Request.QueryString["PAR1Val"];
                        ReportParameter oPar1 = new ReportParameter(par1, par1val, false);
                        if (control < 1000) { ReportViewer1.ServerReport.SetParameters(oPar1); }
                    }
                }
                catch (Exception ex)
                {
                    //lblMessage.Display(new MessageLabelOptions("Exception occured :" + ex.Message, MessageLabelStyle.Error, MessageLabelSpeed.Slow, false, true));
                    return;
                }
        }


        #region "Not Currently Used - Except to track event calls in debug"

        //I used these to find out the order in which methods were being called on page load/view report button click

        protected void ReportViewer1_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ReportViewer1_ReportRefresh" + " IsPostBack = " + IsPostBack);
        }

        protected void ReportViewer1_Load(object sender, EventArgs e)
        {
           // System.Diagnostics.Debug.WriteLine("ReportViewer1_Load" + " IsPostBack = " + IsPostBack);
        }

        protected void ReportViewer1_DataBinding(object sender, EventArgs e)
        {
           // System.Diagnostics.Debug.WriteLine("ReportViewer1_DataBinding" + " IsPostBack = " + IsPostBack);
        }

        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ReportViewer1_PreRender" + " IsPostBack = " + IsPostBack);
        }

        protected void ReportViewer1_ReportError(object sender, ReportErrorEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ReportViewer1_ReportError" + " IsPostBack = " + IsPostBack);
        }

        protected void ReportViewer1_SubmittingParameterValues(object sender, ReportParametersEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ReportViewer1_SubmittingParameterValues" + " IsPostBack = " + IsPostBack);
        }

        protected void form2_Load1(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("form2_Load1"+ " IsPostBack = " + IsPostBack);
            //ReportViewer r = (ReportViewer)this.form2.FindControl("ReportViewer1" );
        }

        #endregion

    }



    /// <summary>
    /// Local implementation of IReportServerCredentials
    /// </summary>
    public class ReportServerCredentials : IReportServerCredentials
    {
        private string _userName;
        private string _password;
        private string _domain;

        public ReportServerCredentials(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                // Use default identity.
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                ICredentials c = null;
                // Use default identity.
                if (!string.IsNullOrEmpty(_userName) && _userName.Trim().Length > 3)
                {
                    //pass along credentials specified in config
                    c = new NetworkCredential(_userName, _password, _domain);
                }
                else
                {
                    //no credentials specified in config so use default
                    c = CredentialCache.DefaultCredentials;
                }
                return c;
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            // Do not use forms credentials to authenticate.
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }


}