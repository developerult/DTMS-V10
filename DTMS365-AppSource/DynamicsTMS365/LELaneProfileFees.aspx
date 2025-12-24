<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LELaneProfileFees.aspx.cs" Inherits="DynamicsTMS365.LELaneProfileFees" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Lane Profile Fees</title>        
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

         function saveLELaneProfileFeesPK() {           
             try {               
                 loadLELaneGridSelectedRow = oLELaneFeeGrid.select();
                 if (typeof (loadLELaneGridSelectedRow) === 'undefined' || loadLELaneGridSelectedRow == null) { ngl.showValidationMsg("LaneFee Record Required", "Please select a LaneFee to continue", tPage); return false; }                             
                 loadLELaneGridSelectedRowDataItem = oLELaneFeeGrid.dataItem(loadLELaneGridSelectedRow); //Get the dataItem for the selected row
                 if (typeof (loadLELaneGridSelectedRowDataItem) === 'undefined' || loadLELaneGridSelectedRowDataItem == null) { ngl.showValidationMsg("LaneFee Record Required", "Please select a LaneFee to continue", tPage); return false; } 
                 if ("LaneControl" in loadLELaneGridSelectedRowDataItem){                
                     iLELaneFeePK = loadLELaneGridSelectedRowDataItem.LaneControl;
                     var setting = {name:'pk', value: iLELaneFeePK.toString()};
                     var oCRUDCtrl = new nglRESTCRUDCtrl();
                     var blnRet = oCRUDCtrl.update("LELaneFee/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                     //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                     return true;
                 } else { ngl.showValidationMsg("LaneFee Record Required", "Invalid Record Identifier, please select a LaneFee to continue", tPage); return false; }
             } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
         }

         function isLELaneFeeSelected() {
             if (typeof (iLELaneFeePK) === 'undefined' || iLELaneFeePK === null || iLELaneFeePK === 0) { return saveLELaneFeePK(); }
             return true;
         }
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenLane" ){ location.href = "Lane";           
             }else if (btn.id == "btnRefresh" ){                 
                 ngl.readDataSource(oLaneFees);
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
                 blnLELaneGridChangeBound = true;
             }        
             //if iLELanePK is not 0 select that row in the grid
             if (typeof (iLELaneFeePK) !== 'undefined' && iLELaneFeePK !== null && iLELaneFeePK !== 0) {
                 var rows = oLELaneFeeGrid.items();
                 $(rows).each(function(e) {
                     var row = this;
                     var dataItem = oLELaneFeeGrid.dataItem(row);
                     if (dataItem.LaneControl == iLELaneFeePK) { oLELaneFeeGrid.select(row); }
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
             else if(btn.id == "btnLaneFee"){ location.href = "LELaneFees"; }  
             else if(btn.id == "btnLanePreferredCarriers"){ location.href = "LELanePreferredCarriers"; } 
             else if(btn.id == "btnRefresh" || btn === "Refresh" ){ ngl.readDataSource(oLaneFees); }
             else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
             //else if(btn.id == "btnLaneFee"){ 
             //    if (isLELaneSelected() === true) {
             //        OpenItem(pgLEControl);
             //    }
             //}
         }
       
         if (typeof (iLELanePK) !== 'undefined' && iLELanePK !== null && iLELanePK !== 0) {
             var rows = oLELaneFeeGrid.items();
             $(rows).each(function(e) {
                 var row = this;
                 var dataItem = oLELaneFeeGrid.dataItem(row);
                 if (dataItem.LaneControl == iLELanePK) { oLELaneFeeGrid.select(row); }
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

