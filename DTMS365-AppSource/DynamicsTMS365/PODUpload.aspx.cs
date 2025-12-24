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
using System.Security.Policy;

namespace DynamicsTMS365
{
    public partial class PODUpload : NGLWebUIBaseClass
    {
        public string sCNS { get; set; }
        public int iBookControl { get; set; }
        public string sSource { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;
                if (request.QueryString.GetValues("bookcontrol") != null && request.QueryString.GetValues("bookcontrol").Length != 0)
                {
                    int ibookkey = 0;
                    int.TryParse(request.QueryString.GetValues("bookcontrol")[0], out ibookkey);
                    iBookControl = ibookkey;
                }
                else
                {
                    iBookControl = 0;
                }

                PageControl = (int)Utilities.PageEnum.PODUpload;
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

                //The About page is the same for everyone and is always visible
                //We always use usercontrol 0 to render this page content as a result
                //However, if a user is signed in and visits this page the users individual menu tree 
                //is built based on UserControl
                PageReadyJS = pg.getMenuTree(UserControl);
                PageReadyJS += pg.getHelpWindow();
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                // posible file types for future
                //•	Freight invoices
                //•	Lumper receipts
                //•	Certificates of Insurance
                //•	POD’s
                //•	OSD pictures/ documents
                //•	Donation Receipts
                //•	Operating Authority
                this.sSource = null;
            this.btnViewFile.Visible = false;
            if (iBookControl != 0)
            {
                int iImageType = 1;
                string sImageType = hdnImageTypeCode.Value;
                int.TryParse(sImageType, out iImageType);
                string sConnection = System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString;
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sConnection);
                if (FileUpload1.HasFile)
                {
                    string strname = FileUpload1.FileName.ToString();
                    strname = strname.Replace(" " , "");
                    int iSuf = 0;
                    while (System.IO.File.Exists(Server.MapPath("~/upload/") + strname))
                    {
                        strname = iSuf++.ToString() + "-" + strname;

                    }
                    FileUpload1.PostedFile.SaveAs(Server.MapPath("~/upload/") + strname);
                    int length = FileUpload1.PostedFile.ContentLength;
                    //create a byte array to store the binary image data
                    byte[] imgbyte = new byte[length];
                    //store the currently selected file in memeory
                    HttpPostedFile img = FileUpload1.PostedFile;
                    //set the binary data
                    img.InputStream.Read(imgbyte, 0, length);
                    con.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(" If (Exists(Select * From dbo.Book where BookControl = @BookControl)) Begin insert into BookImage(BookImageBookControl,BookImageTypeCode,BookImageTypeDesc,BookImageOLE,BookImageModDate,BookImageModUser) values(@BookControl,@BookImageTypeCode,@BookImageTypeDesc,@BookImageOLE,@BookImageModDate,@BookImageModUser) END", con);
                    cmd.Parameters.Add("@BookControl", System.Data.SqlDbType.Int).Value = this.iBookControl;
                    cmd.Parameters.Add("@BookImageTypeCode", System.Data.SqlDbType.Int).Value = iImageType;
                    cmd.Parameters.Add("@BookImageTypeDesc", System.Data.SqlDbType.NVarChar, 255).Value = strname;
                    cmd.Parameters.Add("@BookImageOLE", System.Data.SqlDbType.Image).Value = imgbyte;
                    cmd.Parameters.Add("@BookImageModDate", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@BookImageModUser", System.Data.SqlDbType.NVarChar,100).Value = this.SSOR.UserName;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    lblMessage.Text = "Success: Click View Image to see results";
                    this.btnViewFile.Visible = true;
                    sSource = System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "upload/" + strname;
                    hdnSourcePath.Value = sSource;
                }
                else
                {
                    lblMessage.Text = "Please select an image to upload!";
                }
            } else
            {
                lblMessage.Text = "Please return to the previous page and select a valid record!";
            }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;

            }
        }


        protected void btnViewFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.sSource = hdnSourcePath.Value;
                if (!string.IsNullOrEmpty(sSource)) {
                    string sFileToView = this.sSource;
                    this.sSource = null;
                    this.btnViewFile.Visible = false;
                    Response.Redirect(sFileToView,false);
                } else
                {
                    lblMessage.Text = "The source file is not available. Please try again or return to the previous page and select a valid record!";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;

            }
        }

       
    }
}