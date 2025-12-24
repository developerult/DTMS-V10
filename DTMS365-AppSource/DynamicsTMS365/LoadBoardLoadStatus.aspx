<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadBoardLoadStatus.aspx.cs" Inherits="DynamicsTMS365.LoadBoardLoadStatus" %>

<!DOCTYPE html>

<html>
<head>
    <title>DTMS Load Board Load Status</title>        
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
                            
                            <div style="padding-bottom:10px; padding-top:10px; padding-left:10px;">                            
                                <div><input type="checkbox" id="chkNotesForSHID" class="k-checkbox"><label class="k-checkbox-label" for="chkNotesForSHID"><strong>Show Notes for SHID</strong></label></div>                                             
                            </div>                         
                            
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
         var oBookTrackGrid = null;    

        <% Response.Write(NGLOAuth2); %>

        
         
         <% Response.Write(PageCustomJS); %>

         //*************  execActionClick  ****************
         function execActionClick(btn, proc) {
             if (btn.id == "btnRefresh") { refresh(); }
             else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }
             else { ngl.pgNavigation(btn.id, true, BookControlKey); }// modified by RHR for v-8.5.4.004 on 12/07/2023 added page navigation menu method with parent key
         }
         // modified by RHR for v-8.5.2.006 on 12/22/2022 added refresh method
         function refresh() {
             ngl.readDataSource(oBookTrackGrid);
         }

         //*************  Call Back Functions ****************
         function BookTrackGridDataBoundCallBack(e,tGrid){           
             oBookTrackGrid = tGrid;
             //Modified by RHR for v-8.5.4.004 on 12/06/2023 new Key table properties
             if (BookControlKey && BookControlKey != 0) {
                 if (wdgtvLoadBoardSummarySummary) { wdgtvLoadBoardSummarySummary.read(BookControlKey); }
             }
         }     

         function BookTrackGridGetStringData(s)
         {                  
             if($('#chkNotesForSHID').is(':checked')){ return 'true'; } else { return ''; }
         }
             
         //*************  Page Level Functions ****************
         $('#chkNotesForSHID').click(function(){
             if (oBookTrackGrid) { oBookTrackGrid.dataSource.read(); }  
             //save the page setting
             var UserPageSettingsData = new PageSettingModel();                          
             UserPageSettingsData.name = "LBStatusPgFltr";      
             if ($('#chkNotesForSHID').is(":checked")) { UserPageSettingsData.value = true; } else{ UserPageSettingsData.value = false; }   
             postPageSetting(tPage, "LoadBoardLoadStatus", UserPageSettingsData, true);
         });

         function savePostPageSettingSuccessCallback(results){ return; } //for now do nothing when we save the pk
         function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){ return; } //for now do nothing when we save the pk

         function readGetPageSettingSuccessCallback(data) {
             var oResults = new nglEventParameters();
             var tObj = this;
             oResults.source = "readGetPageSettingSuccessCallback";
             oResults.msg = 'Failed'; //set default to Failed         
             oResults.CRUD = "read";
             oResults.widget = tObj;
             var dsUserPageSettings = null;                          
             try {
                 var blnSuccess = false;
                 var blnErrorShown = false;
                 var strValidationMsg = "";                        
                 if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                          
                     if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null); }                          
                     else {                               
                         if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                             if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                 blnSuccess = true;
                                 dsUserPageSettings = data.Data[0]; 
                                 oResults.msg = "Success";
                                 if(typeof (dsUserPageSettings) !== 'undefined' && dsUserPageSettings != null && dsUserPageSettings.value != undefined){
                                     var d = JSON.parse(dsUserPageSettings.value);  
                                     if(d === true){ $("#chkNotesForSHID").prop('checked', true); } else{ $("#chkNotesForSHID").prop('checked', false); }                                                                                                                                      
                                 }                                   
                             }                              
                         }                            
                     }                        
                 }                      
                 if (blnSuccess === false && blnErrorShown === false) {  
                     if (strValidationMsg.length < 1) { strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to receive this message."; }
                     ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null); 
                     //save the page setting
                     var UserPageSettingsData = new PageSettingModel();                          
                     UserPageSettingsData.name = "LBStatusPgFltr";      
                     if ($('#chkNotesForSHID').is(":checked")) { UserPageSettingsData.value = true; } else{ UserPageSettingsData.value = false; }   
                     postPageSetting(tPage, "LoadBoardLoadStatus", UserPageSettingsData, true);
                 }                   
             } catch (err) { ngl.showErrMsg(err.name, err.description, null); }             
         }
         function readGetPageSettingAjaxErrorCallback(xhr, textStatus, error) {
             var oResults = new nglEventParameters();
             var tObj = this;
             oResults.source = "readGetPageSettingAjaxErrorCallback";
             oResults.msg = 'Failed'; //set default to Failed        
             oResults.CRUD = "read"
             oResults.widget = tObj;
             oResults.error = new Error();
             oResults.error.name = "Read Page Settings Failure"
             oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
             ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); 
         }


         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;            
             
             if (control != 0){
                 getPageSettings(tPage, "LoadBoardLoadStatus", "LBStatusPgFltr", false);
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