<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TariffHDMs.aspx.cs" Inherits="DynamicsTMS365.TariffHDMs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head  >
    <title>DTMS Tariff HDMs</title>        
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
         var oTariffHDMsGrid = null;
         var iTariffHDMPK = 0;

        <% Response.Write(NGLOAuth2); %>

        
         <% Response.Write(PageCustomJS); %>
         function savePostPageSettingSuccessCallback(results) {
             //for now do nothing when we save the pk
         }
         function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error) {
             //for now do nothing when we save the pk
         }

         function saveTariffHDMPK() {
             try {
                 TariffHDMsGridSelectedRow = oTariffHDMsGrid.select();
                 if (typeof (TariffHDMsGridSelectedRow) === 'undefined' || TariffHDMsGridSelectedRow == null) { ngl.showValidationMsg("Tarif HDM Record Required", "Please select a record to continue", tPage); return false; }
                 TariffHDMsGridSelectedRowDataItem = oTariffHDMsGrid.dataItem(TariffHDMsGridSelectedRow); //Get the dataItem for the selected row
                 if (typeof (TariffHDMsGridSelectedRowDataItem) === 'undefined' || TariffHDMsGridSelectedRowDataItem == null) { ngl.showValidationMsg("Tariff HDM Record Required", "Please select a record to continue", tPage); return false; }
                 if ("HDMTariffXrefControl" in TariffHDMsGridSelectedRowDataItem) {
                     iTariffHDMPK = TariffHDMsGridSelectedRowDataItem.HDMTariffXrefControl;
                     var setting = { name: 'pk', value: iTariffHDMPK.toString() };
                     var oCRUDCtrl = new nglRESTCRUDCtrl();
                     var blnRet = oCRUDCtrl.update("TariffHDM/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                     //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                     return true;
                 } else { ngl.showValidationMsg("Tariff HDM Record Required", "Invalid Record Identifier, please select a record to continue", tPage); return false; }
             } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }
         }

         function isTariffHDMSelected() {
             if (typeof (iTariffHDMPK) === 'undefined' || iTariffHDMPK === null || iTariffHDMPK === 0) { return saveTariffHDMPK(); }
             return true;
         }

         var blnTariffHDMsGridChangeBound = false;
         //*************  Call Back Functions ****************
         function TariffHDMsGridDataBoundCallBack(e, tGrid) {
             oTariffHDMsGrid = tGrid;
             if (blnTariffHDMsGridChangeBound == false) {
                 oTariffHDMsGrid.bind("change", saveTariffHDMPK);
                 blnTariffHDMsGridChangeBound = true;
             }
             //if iTariffHDMPK is not 0 select that row in the grid
             if (typeof (iTariffHDMPK) !== 'undefined' && iTariffHDMPK !== null && iTariffHDMPK !== 0) {
                 var rows = oTariffHDMsGrid.items();
                 $(rows).each(function (e) {
                     var row = this;
                     var dataItem = oTariffHDMsGrid.dataItem(row);
                     if (dataItem.HDMTariffXrefControl == iTariffHDMPK) { oTariffHDMsGrid.select(row); }
                 });
             }
         }

         function TariffHDMsGridCB(oResults) {
             if (!oResults) { return; }
             if (oResults.source == "showWidgetCallback") {
                 //process any logic needed when the widget is displayed (opened)                 
             }
         }

         function execActionClick(btn, proc){
             if(btn.id == "btnOpenContract" ){ location.href = "Tariff";
             }else if (btn.id == "btnOpenServices" ){ location.href = "TariffServices";
             }else if (btn.id == "btnOpenRates" ){ location.href = "TariffRates";
             }else if (btn.id == "btnOpenExceptions" ){ location.href = "TariffExceptions";
             }else if (btn.id == "btnOpenFees" ){ location.href = "TariffFees";
             }else if (btn.id == "btnOpenFuel" ){ location.href = "TariffFuel";
             }else if (btn.id == "btnOpenNoDrive" ){ location.href = "TariffNoDriveDays";
             }else if (btn.id == "btnAddHDM" ){ openAddNewTariffHDMsGridWindow();
             }else if (btn.id == "btnRefresh" ){  
                 //oTariffHDMsGrid.data("kendoNGLGrid").dataSource.read();
                 oTariffHDMsGrid.dataSource.read();
             }  
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }
        
       
         
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
                        
            if (control != 0){
                setTimeout(function () {
                    //add code here to load screen specific information this is only visible when a user is authenticated
                }, 1,this);

            }
            var PageReadyJS = <%=PageReadyJS%>;
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