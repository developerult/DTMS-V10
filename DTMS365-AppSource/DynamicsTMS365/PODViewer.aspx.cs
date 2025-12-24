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
using System.Data.SqlClient;
using System.Data.Linq;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace DynamicsTMS365
{
    public partial class PODViewer : NGLWebUIBaseClass
    {
        public string sCNS { get; set; }
        public int iBookControl { get; set; }
        public int iBookImageControl { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;
                if (request.QueryString.GetValues("BookImageControl") != null && request.QueryString.GetValues("BookImageControl").Length != 0)
                {
                    int ibookkey = 0;
                    int.TryParse(request.QueryString.GetValues("BookImageControl")[0], out ibookkey);
                    iBookImageControl = ibookkey;
                }
                else
                {
                    iBookImageControl = 0;
                }
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

                PageControl = (int)Utilities.PageEnum.PODViewer;
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

                if (iBookImageControl != 0)
                {
                    string sConnection = System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString;
                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sConnection);
                    using (SqlCommand cmdSQL = new SqlCommand(" select i.BookImageTypeDesc as FileName, t.ImageTypeCodeName as ImageType, i.BookImageOLE as FileData from dbo.BookImage as i left outer join [dbo].[tblImageTypeCode] as t on i.BookImageTypeCode = t.ImageTypeCodeControl where BookImageControl = " + iBookImageControl.ToString(), con))
                    {
                        cmdSQL.Connection.Open();
                        DataTable dt = new DataTable();
                        dt.Load(cmdSQL.ExecuteReader());
                        DataRow dr = dt.Rows[0];
                        string sFileName = dr.Field<string>("FileName").Trim();
                        string[] sFileTypes = sFileName.Split('.');
                        string sFileType = sFileTypes.Last();
                        lblFileName.Text = sFileName;
                        lblImageType.Text = dr.Field<string>("ImageType");                        
                        byte[] bytes = (byte[])dr.Field<byte[]>("FileData");
                        string strBase64 = Convert.ToBase64String(bytes);
                        switch (sFileType)
                        {
                            case "png":                                
                                imgFile.ImageUrl = "data:Image/png;base64," + strBase64;
                                break;
                            case "gif":                                
                                imgFile.ImageUrl = "data:Image/gif;base64," + strBase64;
                                break;
                            case "jpg":
                                imgFile.ImageUrl = "data:Image/jpg;base64," + strBase64;
                                break;
                            case "jpeg":
                                imgFile.ImageUrl = "data:Image/jpeg;base64," + strBase64;
                                break;
                            default:
                                lblFileName.Text = "Reading File";
                                lblImageType.Text = "Please Wait";
                                try
                                {
                                    string sURLPath = System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "upload/" + sFileName;
                                    string sFilePath = Server.MapPath("~/upload/") + sFileName;
                                    if (File.Exists(sFilePath)) { File.Delete(sFilePath); }

                                    using (var writer = new BinaryWriter(File.OpenWrite(sFilePath)))
                                    {
                                        writer.Write(bytes);
                                    }
                              
                                    Response.Redirect(sURLPath, false);
                                }
                                catch (Exception ex)
                                {
                                    //do nothing or handle as required
                                }
                                break;

                        }
                        //string strBase64 = Convert.ToBase64String(bytes);
                        //imgFile.ImageUrl = "data:Image/png;base64," + strBase64;
                        //imgFile.ImageUrl = "data:image/pdf;base64," + Convert.ToBase64String(dr.Field<byte[]>("FileData"));

                        //imgFile.ImageUrl = "data:image/gif;base64," + Convert.ToBase64String(dr.Field<byte[]>("FileData"));
                    }
                }
                else
                {
                    lblFileName.Text = "No File Found";
                    lblImageType.Text = "Undefined";
                }
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
            }
        }

        

    }
}