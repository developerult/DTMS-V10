using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsTMS365
{
    public partial class MSALLogin : System.Web.UI.Page
    {
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

        public String Caller
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Caller"];

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
            HttpRequest request = HttpContext.Current.Request;
            string sCaller = request.QueryString["caller"];
            if (!string.IsNullOrWhiteSpace(sCaller)) { Caller = sCaller; }
            if (string.IsNullOrWhiteSpace(Caller)) { Caller = "Default.aspx"; }
            string siName = HttpContext.Current.User.Identity.Name;

            string rName = Request.ServerVariables["logon_user"];

            string s = "Success";
        }

        ////Added By LVV on 12/20/16 for v-8.0 Content Management Tables
        //public string getMenuTree()
        //{
        //    return Utilities.getMenuTree(0);
        //}
    }
}