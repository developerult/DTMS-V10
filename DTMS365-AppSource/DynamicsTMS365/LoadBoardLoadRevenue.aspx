<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadBoardLoadRevenue.aspx.cs" Inherits="DynamicsTMS365.LoadBoardLoadRevenue" %>

<!DOCTYPE html>

<html>
<head >
    <title>DTMS Load Board Load Revenue</title>        
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
         //Modified by RHR for v-8.5.4.004 on 12/06/2023 new Key table properties
         //used for query string links
         // not all values are used on all pages
         var BookControlKey = '<%=BookControlKey%>';
         var LaneControlKey = '<%=LaneControlKey%>';
         var CarrierControlKey = '<%=CarrierControlKey%>';
         var CompControlKey = '<%=CompControlKey%>';
         var TariffControlKey = '<%=TariffControlKey%>';

         var tObj = this;
         var tPage = this;
         var oLBRLDFeesGrid = null;
         var oLBRLDChargesGrid = null;
         var oLBRLDCostsGrid = null;    

        <% Response.Write(NGLOAuth2); %>

        
         
         <% Response.Write(PageCustomJS); %>

         function execActionClick(btn, proc){
             if (btn.id == "btnRefresh") { refresh();}
             else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }
             else { ngl.pgNavigation(btn.id, true, BookControlKey); } // modified by RHR forfor v-8.5.4.004 on 12/06/2023 added page navigation menu method
         }
         // modified by RHR for v-8.5.2.006 on 12/22/2022 added refresh method
         function refresh() {

             if (oLBRLDFeesGrid) { oLBRLDFeesGrid.dataSource.read(); }
             if (oLBRLDChargesGrid) { oLBRLDChargesGrid.dataSource.read(); }
             if (oLBRLDCostsGrid) { oLBRLDCostsGrid.dataSource.read(); }
                 //if (wdgtvLoadBoardSummarySummary) { wdgtvLoadBoardSummarySummary.read(0); }
                 //if (wdgtLoadBoardRevDataEdit) { wdgtLoadBoardRevDataEdit.read(0); } 
         }
         //*************  Call Back Functions ****************

         var sVariances = [
             "QvA_Variance", "QvAj_Variance", "QvB_Variance", "QvE_Variance", "QvP_Variance","QvPd_Variance",
             "EvA_Variance", "EvAj_Variance", "EvB_Variance", "EvP_Variance", "EvPd_Variance", 
             "BvA_Variance", "BvAj_Variance", "BvP_Variance","BvPd_Variance",
             "PvA_Variance", "PvAj_Variance", "PvPd_Variance",
             "AjvA_Variance", "AjvPd_Variance",
             "AvPd_Variance"             
         ];



         function addFinancialNumericClassToGridColumns(tGrid,ds,j,row){
             for (var v=0; v < sVariances.length; v++ ){
                 var sField = sVariances[v];
                 var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + sField.toString() + "]").index();
                 var dsColumn = ds[j][sField.toString()]
                 var cell = row.children().eq(columnIndex);               
                 ngl.addFinancialNumericClassToGridColumn(dsColumn, columnIndex, cell);
             }              
         }

         function LBRLDFeesGridDataBoundCallBack(e,tGrid){           
             oLBRLDFeesGrid = tGrid;
             var ds = tGrid.dataSource.data();
             for (var j=0; j < ds.length; j++) {                 
                 var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']");
                 addFinancialNumericClassToGridColumns(tGrid,ds,j,row);
             }
             //Modified by RHR for v-8.5.4.004 on 12/06/2023 new Key table properties
             if (BookControlKey && BookControlKey != 0) {
                 if (wdgtvLBRevLoadSummarySummary) { wdgtvLBRevLoadSummarySummary.read(BookControlKey); }
             }
         }

         function LBRLDChargesGridDataBoundCallBack(e,tGrid){           
             oLBRLDChargesGrid = tGrid;             
             var ds = tGrid.dataSource.data();
             for (var j=0; j < ds.length; j++) {                 
                 var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']");
                 addFinancialNumericClassToGridColumns(tGrid,ds,j,row);
             }
         }

         function LBRLDCostsGridDataBoundCallBack(e,tGrid){           
             oLBRLDCostsGrid = tGrid;
             var ds = tGrid.dataSource.data();
             for (var j=0; j < ds.length; j++) {                 
                 var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']");
                 addFinancialNumericClassToGridColumns(tGrid,ds,j,row);
             }
         }       
       
       
/*
         function LoadBoardRevDataCB(oResults){
             if ( oResults.source == "readSuccessCallback")
             {
                 //var check  wdgtLoadBoardRevDataEdit.GetFieldID
                 var resultData = oResults.data[0];
                 setTimeout(function (oResults) {
                     var BookRevDiscountid = wdgtLoadBoardRevDataEdit.GetFieldID("BookRevDiscount");
                     var BookRevLineHaulid = wdgtLoadBoardRevDataEdit.GetFieldID("BookRevLineHaul");
                     var BookRevCommPercentid = wdgtLoadBoardRevDataEdit.GetFieldID("BookRevCommPercent");
                     var BookRevCommCostid = wdgtLoadBoardRevDataEdit.GetFieldID("BookRevCommCost");
                     var BookRevNegRevenueid = wdgtLoadBoardRevDataEdit.GetFieldID("BookRevNegRevenue");
                     if (oResults.data[0].BookLockAllCosts == true){
                         $("#" + BookRevDiscountid).data("kendoNumericTextBox").readonly(true);
                         replaceSlockClass(BookRevDiscountid,'k-icon k-i-lock');
                         $("#" + BookRevLineHaulid).data("kendoNumericTextBox").readonly(true);
                         replaceSlockClass(BookRevLineHaulid,'k-icon k-i-lock');
                         $("#" + BookRevCommPercentid).data("kendoNumericTextBox").readonly(true);
                         replaceSlockClass(BookRevCommPercentid,'k-icon k-i-lock');
                         $("#" + BookRevCommCostid).data("kendoNumericTextBox").readonly(true);
                         replaceSlockClass(BookRevCommCostid,'k-icon k-i-lock');                         
                         $("#" + BookRevNegRevenueid).data("kendoDropDownList").readonly(true);
                         replaceSlockClass(BookRevNegRevenueid,'k-icon k-i-lock');
                     } else {                     
                         //$(this).parent().siblings('.badge.votes');
                         //'class="k-icon k-i-save"'
                         $("#" + BookRevDiscountid).data("kendoNumericTextBox").readonly(false);
                         replaceSlockClass(BookRevDiscountid);
                         $("#" + BookRevLineHaulid).data("kendoNumericTextBox").readonly(false);
                         replaceSlockClass(BookRevLineHaulid);
                         $("#" + BookRevCommPercentid).data("kendoNumericTextBox").readonly(false);
                         replaceSlockClass(BookRevCommPercentid);
                         $("#" + BookRevCommCostid).data("kendoNumericTextBox").readonly(false);
                         replaceSlockClass(BookRevCommCostid);                         
                         $("#" + BookRevNegRevenueid).data("kendoDropDownList").readonly(false);
                         replaceSlockClass(BookRevNegRevenueid);
                     }
                     var BookRevBilledBFCid = wdgtLoadBoardRevDataEdit.GetFieldID("BookRevBilledBFC");
                     var BookTotalBFCid = wdgtLoadBoardRevDataEdit.GetFieldID("BookTotalBFC");
                     if (oResults.data[0].BookLockBFCCost == true){
                         //alert('BFC Costs are locked');
                         $("#" + BookRevBilledBFCid).data("kendoNumericTextBox").readonly(true);
                         replaceSlockClass(BookRevBilledBFCid,'k-icon k-i-lock');
                         $("#" + BookTotalBFCid).data("kendoNumericTextBox").readonly(true);
                         replaceSlockClass(BookTotalBFCid,'k-icon k-i-lock');
                     } else {                     

                         $("#" + BookRevBilledBFCid).data("kendoNumericTextBox").readonly(false);
                         replaceSlockClass(BookRevBilledBFCid);
                         $("#" + BookTotalBFCid).data("kendoNumericTextBox").readonly(false);
                         replaceSlockClass(BookTotalBFCid);
                     }
                 },20,oResults);
             }

         }

         function replaceSlockClass(idKey,sClass){
             var slockspan =  $("#splock" + idKey);
             if (slockspan) {
                 slockspan.removeClass(); 
                 if (sClass) { slockspan.addClass(sClass);}
             }
        }
  
*/


         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;            
           
             if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                //}, 1,this);
           
             }                     
             var PageReadyJS = <%=PageReadyJS%>;
             menuTreeHighlightPage(); //must be called after PageReadyJS
             var divWait = $("#h1Wait");             
             if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }        
         });
     </script>
    <style>        
        .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
       
        .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }
              
        .k-grid tbody .k-grid-Edit { min-width: 0; }
      
        .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }
    </style>  
    </div>
</body>
</html>