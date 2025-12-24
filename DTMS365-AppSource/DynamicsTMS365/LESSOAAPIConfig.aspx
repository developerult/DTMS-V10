<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LESSOAAPIConfig.aspx.cs" Inherits="DynamicsTMS365.LESSOAAPIConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
        <title>DTMS Legal Entity SSOA API Config</title>         
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
         var oSSOALEConfigGrid = null;
        var tObj = this;
        var tPage = this;    

        <% Response.Write(NGLOAuth2); %>

        var iSSOALEPK = 0;
        <% Response.Write(PageCustomJS); %>

     function savePostPageSettingSuccessCallback(results) {
         //for now do nothing when we save the pk
     }
     function savePostPageSettingSuccessCallbackAjaxErrorCallback(xhr, textStatus, error) {
         //for now do nothing when we save the pk
     }

     function savetblSSOALEConfigPK() {
         try {
             var row = oSSOALEGrid.select();
             if (typeof (row) === 'undefined' || row == null) {
                 ngl.showValidationMsg("Record Required", "Please select a record to continue", tPage);
                 return false;
             }
             //Get the dataItem for the selected row
             var dataItem = oSSOALEGrid.dataItem(row);
             if (typeof (dataItem) === 'undefined' || dataItem == null) {
                 ngl.showValidationMsg("Record Required", "Please select a record to continue", tPage);
                 return false;
             }
             if ("SSOALEControl" in dataItem) {

                 iSSOALEPK = dataItem.SSOALEControl;
                 var setting = { name: 'pk', value: iSSOALEPK.toString() };
                 var oCRUDCtrl = new nglRESTCRUDCtrl();
                 var blnRet = oCRUDCtrl.update("tblSSOALEConfig/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingSuccessCallbackAjaxErrorCallback", tPage);
                 return true;
             } else {
                 ngl.showValidationMsg("Record Required", "Invlaid Record Identifier, please select a record to continue", tPage);
                 return false;
             }

         } catch (err) {
             ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen.  If this continues please contact technical support. Error: " + err.message, tPage);
         }

     }
     function isRecordSelected() {
         if (typeof (iSSOALEPK) === 'undefined' || iSSOALEPK === null || iSSOALEPK === 0) {
             return savetblSSOALEConfigPK();
         }
         return true;
     }
     function execActionClick(btn, proc) {
         if (btn.id == "btnRefresh") {
             oSSOALEGrid.dataSource.read();
         } else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }
     }
     var blnSSOALEGridChangeBound = false;
     //*************  Call Back Functions ****************
     function SSOALEGridDataBoundCallBack(e, tGrid) {
         oSSOALEGrid = tGrid;
         if (blnSSOALEGridChangeBound == false) {
             oSSOALEGrid.bind("change", savetblSSOALEConfigPK);
             blnSSOALEGridChangeBound = true;
         }


     }


     $(document).ready(function () {

         var PageMenuTab = <%=PageMenuTab%>;

           if (control != 0) {
               //setTimeout(function () {
               //    //add code here to load screen specific information this is only visible when a user is authenticated

               //}, 1,this);

           }
     var PageReadyJS = <%=PageReadyJS%>
         //setTimeout(function () {   
         menuTreeHighlightPage(); //must be called after PageReadyJS
         var divWait = $("#h1Wait");
         if (typeof (divWait) !== 'undefined') {
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
