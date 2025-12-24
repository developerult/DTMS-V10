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
    public partial class LogOut : System.Web.UI.Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Clear();
        }

    }
}