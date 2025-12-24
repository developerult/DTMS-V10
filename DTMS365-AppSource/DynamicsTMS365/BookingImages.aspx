<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingImages.aspx.cs" Inherits="DynamicsTMS365.BookingImages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Booking Images</title>        
         <%=cssReference%>             
        <style>
            html,
            body {height:100%; margin:0; padding:0;}

            html {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

        </style>
    </head>
    <body>     
        <%=jssplitter2Scripts%> 
        <%=sWaitMessage%>         
    <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white; ">
                    <div id="tab" class="menuBarTab"></div> 
                </div>
                <div id="top-pane">
                  <div id="horizontal" style="height: 100%; width: 100%; ">
                        <div id="left-pane">
                            <div class="pane-content">
                                <% Response.Write(MenuControl); %>
                                <div id="menuTree"></div>                                                               
                            </div>
                        </div>
                        <div id="center-pane">                            
                            <% Response.Write(PageErrorsOrWarnings); %>                            
                            
                            <!-- begin Page Content -->                           
                            <% Response.Write(FastTabsHTML); %>   
                            <!-- End Page Content -->
                        </div>
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>

          
     <% Response.Write(PageTemplates); %>
        
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>   
 
     <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
          var tObj = this;
         var tPage = this;
         var oBookingImages = null;  

        <% Response.Write(NGLOAuth2); %>
        var iBookImagePK = 0; //Control = BookControl  
        var BookImageSetGridSelectedRowDataItem; 


         function execBeforeBookingImagesGridInsert(){
             // prepare for new record like: add code to update the default values for specific fields
             // see TariffRates.aspx for examples
             return true;
           
         }

         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc) {
             if (btn.id == "btnRefresh") { refresh(); }
             else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }
             else if (btn.id == "btnOpenFile") { openFile(); }
         }
         // modified by RHR for v-8.5.2.006 on 12/22/2022 added refresh method
         function refresh() {
             ngl.readDataSource(oBookingImages);
         }

         //*************  Call Back Functions ****************
             var blnBookingImagesGridChangeBound = false;
         function BookingImagesGridDataBoundCallBack(e, tGrid) {           
             oBookingImages = tGrid;
             if (blnBookingImagesGridChangeBound == false) {
                 oBookingImages.bind("change", saveBookImagePK);
                 blnBookingImagesGridChangeBound = true;
             }
         }
           
         // widget call back         
         function BookingImagesGridCB(oResults){   
             if (!oResults) { return;}
             if (oResults.source == "showWidgetCallback"   ){
                 //process any logic needed when the widget is displayed (opened)                 
             }             
         }
         
         // ************** End Call Back functions Functions ******************
             function saveBookImagePK() {
                 try {
                     var row = oBookingImages.select();
                     if (typeof (row) === 'undefined' || row == null) { ngl.showValidationMsg("Booking Image Record Required", "Please select a Booking Image to continue", tPage); return false; }
                     //Get the dataItem for the selected row
                     BookImageSetGridSelectedRowDataItem = oBookingImages.dataItem(row);
                     if (typeof (BookImageSetGridSelectedRowDataItem) === 'undefined' || BookImageSetGridSelectedRowDataItem == null) { ngl.showValidationMsg("Booking Image Record Required", "Please select a Booking Image to continue", tPage); return false; }
                     if ("BookImageControl" in BookImageSetGridSelectedRowDataItem) {
                         //save the name of the selected carrier
                         iBookImagePK = BookImageSetGridSelectedRowDataItem.BookImageControl;
                         var setting = { name: 'pk', value: iBookImagePK.toString() };
                         var oCRUDCtrl = new nglRESTCRUDCtrl();
                         var blnRet = oCRUDCtrl.update("BookingImage/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                         return true;
                     } else { ngl.showValidationMsg("Booking Image Record Required", "Invalid Record Identifier, please select a Booking Image to continue", tPage); return false; }
                 } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }
             }

             function isBookImageSelected() {
                 if (typeof (iBookImagePK) === 'undefined' || iBookImagePK === null || iBookImagePK === 0) { return saveBookImagePK(); }
                 return true;
             }

             function openFile() {
                 if (typeof (iBookImagePK) !== 'undefined' || iBookImagePK !== null || iBookImagePK !== 0) {
                     window.open("../PODViewer?BookImageControl=" + iBookImagePK, "_blank");
                 }
             }
         
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
            
            
            if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                //}, 1,this);

            }
            var PageReadyJS = <%=PageReadyJS%>
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if  (typeof (divWait) !== 'undefined' ) {
                divWait.hide();
            }
           
           

        });


     </script>
    <style>

        .k-grid tbody tr td {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

         .k-tooltip{
            max-height: 500px;
            max-width: 450px;
            overflow-y: auto;
        }
       
        .k-grid tbody .k-grid-Edit {
        min-width: 0;
      }

      .k-grid tbody .k-grid-Edit .k-icon {
        margin: 0;
      }
    </style>
    
    </div>
</body>
</html>