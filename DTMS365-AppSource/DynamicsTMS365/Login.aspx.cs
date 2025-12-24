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
    public partial class Login : NGLWebUIBaseClass
    {

        public string sCaller { get; set; }
        /// <summary>
        /// Process User information and update session data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Modified by RHR for v-8.4.x on 03/27/2021
        ///   new rules for validate user 
        ///   we no longer expect to use querystring to pass in usercontrol to each page
        ///   Now this is only possible on this page, Login.aspx, a redirect only page
        ///   the Login.aspx redirect page will do the following:
        ///   1. check for query strings that include usercontrol and caller
        ///   2. if both are provided the page will create the session variable for useer control and return authentication to the caller
        ///   3. if user control is 0 but caller is not let Javascript read local storage and authenticate the user with local data
        ///         SSOA js will redirect back to Login.aspx page after authentication with valid data 
        ///         or it will redirect to NGLLogin page
        ///   4. if both user and caller are not valid redirect to NGLLogin page
        ///   So: all other pages will call refreshUserControl on load
        ///         a) check session value if blank (check query string for legacy calls to page)
        ///             if no data redirect to Login.aspx page with caller and zero for usercontrol
        ///         b) if we have a user control check if user is allowed to access the page
        ///         c) if user does not have permissions redirect to home (Default.aspx)

        /// </remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PageControl = (int)Utilities.PageEnum.Login;
               
                HttpRequest request = HttpContext.Current.Request;
                //   1. check for query strings that include usercontrol and caller
                sCaller = request.QueryString["caller"];
                //we must check if this is an external OAuth 2 authentication method
                checkhOAuthData();
                if (this.blnUseSSR == true)
                {
                    return;
                }
                var sUserControl = request.QueryString["uc"];
                int ius = 0;
                if (sUserControl != null)
                {
                    // if numeric save to session   
                    string suc = sUserControl.ToString();
                    int.TryParse(suc, out ius);
                    //save the session data
                    UserControl = ius;
                }
                if (!string.IsNullOrWhiteSpace(sCaller) && UserControl != 0)
                {
                    ///   2. if both are provided the page will create the session variable for useer control and return authentication to the caller
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + sCaller, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                                   
                } else if (!string.IsNullOrWhiteSpace(sCaller))
                {
                    ///   3. if user control is 0 but caller is not let Javascript read local storage and authenticate the user with local data
                    return;
                } else
                {
                    ///   4. if both user and caller are not valid redirect to NGLLogin page

                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "NGLLogin.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
            }
        }

        private void checkhOAuthData()
        {
            this.blnUseSSR = false;
            this.SSOR = new DAL.Models.SSOResults();
            HttpRequest request = HttpContext.Current.Request;
            string authUN = "";
            string authT = "";
            if (request.QueryString.GetValues("AuthUN") != null && request.QueryString.GetValues("AuthT") != null)
                {
                    if (request.QueryString.GetValues("AuthUN").Length != 0 && request.QueryString.GetValues("AuthT").Length != 0)
                    {
                        authUN = request.QueryString.GetValues("AuthUN")[0];
                        authT = request.QueryString.GetValues("AuthT")[0];
                        DAL.Models.SSOResults ssosr = DynamicsTMS365.Controllers.SSOAController.GetNGLLegacySSOAForUser(authUN, authT);
                        if (ssosr != null && ssosr.UserSecurityControl != 0)
                        {
                            this.blnUseSSR = true;
                            this.UserControl = ssosr.UserSecurityControl;
                            this.SSOR = ssosr;
                            //s.Replace(@"\", @"\\");
                            this.UserName = authUN;
                            this.UserToken = ssosr.USATToken;
                            this.UserTheme = ssosr.UserTheme365;
                            //save to server user dictionary
                            if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(ssosr.UserSecurityControl))
                            {
                                DynamicsTMS365.Utilities.GlobalSSOResultsByUser[ssosr.UserSecurityControl] = ssosr;
                            }
                            else
                            {
                                DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(ssosr.UserSecurityControl, ssosr);
                            }

                        }
                    }
                }                

            }
        }
}