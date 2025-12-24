<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DAT.aspx.cs" Inherits="DynamicsTMS365.DAT" %>
<%--Added By LVV on 9/30/20 for v-8.3.0.001 Task #20200930125350 - DAT Migration--%>
<!DOCTYPE html>

<html>
<head>
    <title>DTMS DAT</title>        
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

        <% Response.Write(NGLOAuth2); %>


         var oDATPostGrid = null;
         var oDATDeletedGrid = null;
         var oDATExpiredGrid = null;
         var oDATErrorGrid = null;        
         
         <% Response.Write(PageCustomJS); %>

         //*************  execActionClick  ****************
         function execActionClick(btn, proc){
             if (btn.id == "btnOpenLoadBoard"){ location.href = "LoadBoard"; }
             else if (btn.id == "btnRefresh"){ refresh(); } 
             else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
             else if(btn.id == "btnLaunchDATWebsite"){ launchDATWebsite(); }
         }

         function refresh(){ 
             ngl.readDataSource(oDATPostGrid); 
             ngl.readDataSource(oDATDeletedGrid);
             ngl.readDataSource(oDATExpiredGrid);
             ngl.readDataSource(oDATErrorGrid);
         }

         function launchDATWebsite(){
             //var oCRUDCtrl = new nglRESTCRUDCtrl();
             //oCRUDCtrl.filteredRead = ("Parameter/GetGlobalParText", "DATLoadBoardWebsite", tObj, "launchDATWebsiteSuccessCallback", "launchDATWebsiteAjaxErrorCallback", true);  
             
             var oCRUDCtrl = new nglRESTCRUDCtrl();
             var blnRet = oCRUDCtrl.filteredRead("Parameter/GetGlobalParText", "DATLoadBoardWebsite", tObj, "launchDATWebsiteSuccessCallback", "launchDATWebsiteAjaxErrorCallback");  
        

         }

         //*************  Call Back Functions ****************
         function DATPostGridDataBoundCallBack(e,tGrid){ 
             oDATPostGrid = tGrid;
             var columnIndexRefID = oDATPostGrid.wrapper.find(".k-grid-header [data-field=" + "LTDATRefID" + "]").index(); //get the index of the LTDATRefID column
             var columnIndexLTSCDesc = oDATPostGrid.wrapper.find(".k-grid-header [data-field=" + "LTSCDesc" + "]").index(); //get the index of the LTSCDesc column
             var ds = oDATPostGrid.dataSource.data();
             for (var j=0; j < ds.length; j++) {
                 var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']"); //get the row
                 var cellRefID = row.children().eq(columnIndexRefID); //get the RefID cell
                 var cellLTSCDesc = row.children().eq(columnIndexLTSCDesc); //get the LTSCDesc cell
                 if (ngl.stringHasValue(ds[j].LTDATRefID)) { cellRefID.addClass("mediumseagreen"); } //Make text green
                 if (ngl.stringHasValue(ds[j].LTSCDesc)) { cellLTSCDesc.addClass("mediumseagreen"); } //Make text green
             }
         } 

         function DATDeletedGridDataBoundCallBack(e,tGrid){ 
             oDATDeletedGrid = tGrid;            
             var columnIndexRefID = oDATDeletedGrid.wrapper.find(".k-grid-header [data-field=" + "LTDATRefID" + "]").index(); //get the index of the LTDATRefID column
             var columnIndexLTSCDesc = oDATDeletedGrid.wrapper.find(".k-grid-header [data-field=" + "LTSCDesc" + "]").index(); //get the index of the LTSCDesc column
             var ds = oDATDeletedGrid.dataSource.data();
             for (var j=0; j < ds.length; j++) {
                 var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']"); //get the row
                 var cellRefID = row.children().eq(columnIndexRefID); //get the RefID cell
                 var cellLTSCDesc = row.children().eq(columnIndexLTSCDesc); //get the LTSCDesc cell
                 if (ngl.stringHasValue(ds[j].LTDATRefID)) { cellRefID.addClass("blue"); } //Make text blue
                 if (ngl.stringHasValue(ds[j].LTSCDesc)) { cellLTSCDesc.addClass("blue"); } //Make text blue
             }
         }

         function DATExpiredGridDataBoundCallBack(e,tGrid){ 
             oDATExpiredGrid = tGrid;
             var columnIndexRefID = oDATExpiredGrid.wrapper.find(".k-grid-header [data-field=" + "LTDATRefID" + "]").index(); //get the index of the LTDATRefID column
             var columnIndexLTSCDesc = oDATExpiredGrid.wrapper.find(".k-grid-header [data-field=" + "LTSCDesc" + "]").index(); //get the index of the LTSCDesc column
             var ds = oDATExpiredGrid.dataSource.data();
             for (var j=0; j < ds.length; j++) {
                 var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']"); //get the row
                 var cellRefID = row.children().eq(columnIndexRefID); //get the RefID cell
                 var cellLTSCDesc = row.children().eq(columnIndexLTSCDesc); //get the LTSCDesc cell
                 if (ngl.stringHasValue(ds[j].LTDATRefID)) { cellRefID.addClass("red"); } //Make text red
                 if (ngl.stringHasValue(ds[j].LTSCDesc)) { cellLTSCDesc.addClass("red"); } //Make text red
             }
         }

         function DATErrorGridDataBoundCallBack(e,tGrid){ 
             oDATErrorGrid = tGrid;
             var columnIndexRefID = oDATErrorGrid.wrapper.find(".k-grid-header [data-field=" + "LTDATRefID" + "]").index(); //get the index of the LTDATRefID column
             var columnIndexLTSCDesc = oDATErrorGrid.wrapper.find(".k-grid-header [data-field=" + "LTSCDesc" + "]").index(); //get the index of the LTSCDesc column
             var ds = oDATErrorGrid.dataSource.data();
             for (var j=0; j < ds.length; j++) {
                 var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']"); //get the row
                 var cellRefID = row.children().eq(columnIndexRefID); //get the RefID cell
                 var cellLTSCDesc = row.children().eq(columnIndexLTSCDesc); //get the LTSCDesc cell
                 if (ngl.stringHasValue(ds[j].LTDATRefID)) { cellRefID.addClass("orangered"); } //Make text orangered
                 if (ngl.stringHasValue(ds[j].LTSCDesc)) { cellLTSCDesc.addClass("orangered"); } //Make text orangered
             }
         }

         function launchDATWebsiteSuccessCallback(data) {
             try {
                 var blnSuccess = false;
                 var blnErrorShown = false;
                 var strValidationMsg = "";
                 if (typeof (data) != 'undefined' && ngl.isObject(data)) {
                     if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Launch DAT Website Failure", data.Errors, null); }
                     else {
                         if (typeof (data.Data) != 'undefined' && ngl.isArray(data.Data)) {                     
                             if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined') {
                                 blnSuccess = true;
                                 var urlDATWebsite = data.Data[0];
                                 if (ngl.stringHasValue(urlDATWebsite)){ openInNewTab(urlDATWebsite); }
                             }
                         }
                     }
                 }
                 if (blnSuccess === false && blnErrorShown === false) {
                     if (strValidationMsg.length < 1) { strValidationMsg = "Launch DAT Website Failure"; }
                     ngl.showErrMsg("Launch DAT Website Failure", strValidationMsg, null);
                 }
             } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
         }
         function launchDATWebsiteAjaxErrorCallback(xhr, textStatus, error) {
             ngl.showErrMsg("Launch DAT Website Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), null);
         }

             
         //*************  Page Level Functions ****************
         function savePostPageSettingSuccessCallback(results){ return; } //for now do nothing when we save the pk
         function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){ return; } //for now do nothing when we save the pk

         function openInNewTab(url) {
             var win = window.open(url, '_blank');
             win.focus();
         }


         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;            
                                           
             if (control != 0){

             }                     
             var PageReadyJS = <%=PageReadyJS%>;
             menuTreeHighlightPage(); //must be called after PageReadyJS
             var divWait = $("#h1Wait");             
             if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }        
         });
    </script>
    <style>        
        .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }      
        .k-tooltip { max-height: 500px; max-width: 450px; overflow-y: auto; }             
        .k-grid tbody .k-grid-Edit { min-width: 0; }     
        .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }
    </style>  
    </div>
</body>
</html>
