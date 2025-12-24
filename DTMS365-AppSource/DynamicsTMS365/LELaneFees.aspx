<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LELaneFees.aspx.cs" Inherits="DynamicsTMS365.LELaneFees" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Lane Fees</title>        
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
         var oLELaneFeeGrid = null;
         var iLaneFeePK = 0;         
         var tObjPG = this;
         var pgLEControl = 0;
         var pgLEName = "";      

        <% Response.Write(NGLOAuth2); %>

        

         function execBeforeLaneFeesGridInsert(){
             // prepare for new record like: add code to update the default values for specific fields
             // see TariffRates.aspx for examples
             return true;
           
         }
         <% Response.Write(PageCustomJS); %>

         function savePostPageSettingSuccessCallback(results){
             //for now do nothing when we save the pk
         }
         function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
             //for now do nothing when we save the pk
         }

         function saveLELaneFeePK() {            
             try {
                 loadLELaneFeeGridSelectedRow = oLELaneFeeGrid.select();
                 if (typeof (loadLELaneFeeGridSelectedRow) === 'undefined' || loadLELaneFeeGridSelectedRow == null) { ngl.showValidationMsg("LaneFee Record Required", "Please select a LaneFee to continue", tPage); return false; }                             
                 loadLELaneFeeGridSelectedRowDataItem = oLELaneFeeGrid.dataItem(loadLELaneFeeGridSelectedRow); //Get the dataItem for the selected row
                 if (typeof (loadLELaneFeeGridSelectedRowDataItem) === 'undefined' || loadLELaneFeeGridSelectedRowDataItem == null) { ngl.showValidationMsg("LaneFee Record Required", "Please select a LaneFee to continue", tPage); return false; } 
                 if ("LaneFeesControl" in loadLELaneFeeGridSelectedRowDataItem){                
                     iLaneFeePK = loadLELaneFeeGridSelectedRowDataItem.LaneFeesControl;
                     var setting = {name:'pk', value: iLaneFeePK.toString()};
                     var oCRUDCtrl = new nglRESTCRUDCtrl();
                     var blnRet = oCRUDCtrl.update("Lane/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                     //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                     return true;
                 } else { ngl.showValidationMsg("LaneFee Record Required", "Invalid Record Identifier, please select a LaneFee to continue", tPage); return false; }
             } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
         }

         function isLELaneFeeSelected() {
             if (typeof (iLaneFeePK) === 'undefined' || iLaneFeePK === null || iLaneFeePK === 0) { return saveLELaneFeePK(); }
             return true;
         }
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenLane" ){ location.href = "Lane";           
             }else if (btn.id == "btnRefresh" ){                 
                 ngl.readDataSource(oLELaneFeeGrid);
             }  
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }

         var blnLELaneFeeGridChangeBound = false;
         //*************  Call Back Functions ****************
         function LaneFeesGridDataBoundCallBack(e,tGrid){ 
             oLELaneFeeGrid = tGrid;  
             if (blnLELaneFeeGridChangeBound == false){
                 oLELaneFeeGrid.bind("change", saveLELaneFeePK);
                 blnLELaneFeeGridChangeBound = true;
             }        
             //if iLaneFeePK is not 0 select that row in the grid
             if (typeof (iLaneFeePK) !== 'undefined' && iLaneFeePK !== null && iLaneFeePK !== 0) {
                 var rows = oLELaneFeeGrid.items();
                 $(rows).each(function(e) {
                     var row = this;
                     var dataItem = oLELaneFeeGrid.dataItem(row);
                     if (dataItem.LaneFeesControl == iLaneFeePK) { oLELaneFeeGrid.select(row); }
                 });
             }
         }
           
         // widget call back         
         function LaneFeesGridCB(oResults){   
             if (!oResults) { return;}
             if (oResults.source == "showWidgetCallback"   ){
                 //process any logic needed when the widget is displayed (opened)                 
             }             
         }
         
         // ************** End Call Back functions Functions ******************

         //************* Action Menu Functions ********************
         function execActionClick(btn, proc){
             if(btn.id == "btnLELaneMaint"){ location.href = "LELaneMaint"; }  
             else if(btn.id == "btnLaneProfileFees"){ location.href = "LELaneProfileFees"; }  
             else if(btn.id == "btnLanePreferredCarriers"){ location.href = "LELanePreferredCarriers"; }  
             else if (btn.id == "btnLELaneHDMMaint") { location.href = "LELaneHDMMaint"; }
             else if (btn.id == "btnRefresh" || btn === "Refresh") { ngl.readDataSource(oLELaneFeeGrid); }
             else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
         }       
         
         if (typeof (iLaneFeePK) !== 'undefined' && iLaneFeePK !== null && iLaneFeePK !== 0) {
             var rows = oLELaneFeeGrid.items();
             $(rows).each(function(e) {
                 var row = this;
                 var dataItem = oLELaneFeeGrid.dataItem(row);
                 if (dataItem.LaneFeesControl == iLaneFeePK) { oLELaneFeeGrid.select(row); }
             });
         }
         
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
           
           
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
