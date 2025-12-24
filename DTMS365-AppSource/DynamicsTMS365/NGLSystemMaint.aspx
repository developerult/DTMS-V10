<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NGLSystemMaint.aspx.cs" Inherits="DynamicsTMS365.NGLSystemMaint" %>

<!DOCTYPE html>

<html>
<head>
    <title>DTMS NGL System Maint</title>
    <%=cssReference%>
    <style>
        html,
        body { height: 100%; margin: 0; padding: 0; }
        html { font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow: hidden; }
    </style>
</head>
<body>
    <%=jssplitter2Scripts%>
    <%=sWaitMessage%>
    <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
        <div id="vertical" style="height: 98%; width: 98%;">
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%;">
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

        <div id="divModuleWnd"></div>

        <% Response.Write(PageTemplates); %>

        <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
        <% Response.Write(AuthLoginNotificationHTML); %>
        <% Response.WriteFile("~/Views/HelpWindow.html"); %>

        <script>
            <% Response.Write(ADALPropertiesjs); %>;
            var PageControl = '<%=PageControl%>'; 
            var oPgFooterGrid = null;
            //var tObj = this;
            var tPage = this;           
       

        <% Response.Write(NGLOAuth2); %>

        
            var oModuleWndCtrl = new SelectionGridWndCtrl();
         
            <% Response.Write(PageCustomJS); %>

            //*************  execActionClick  ****************
            function execActionClick(btn, proc){                
                if (btn.id == "btnCreateModuleLicenseKey" ){ 
                    if (typeof (oModuleWndCtrl) !== 'undefined' && ngl.isObject(oModuleWndCtrl)) { oModuleWndCtrl.show(); } 
                }
                else if (btn.id == "btnManagePgFooterMsg" ){ 
                    if (typeof (tPage["wdgtPgFooterWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtPgFooterWndDialog"])){
                        tPage["wdgtPgFooterWndDialog"].show();                
                    } else{alert("Missing HTML Element (wdgtPgFooterWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                } 
                else if (btn.id == "btnRefresh" ){ refresh(); } 
                else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
            }

            function refresh(){ 
                ngl.readDataSource(oPgFooterGrid); 
                oModuleWndCtrl.refresh();
            }

            function PgFooterGridDataBoundCallBack(e,tGrid){           
                oPgFooterGrid = tGrid;       
            }


            function savePostPageSettingSuccessCallback(results){
                //for now do nothing when we save the pk
            }
            function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
                //for now do nothing when we save the pk
            }

            function oModuleSaveCB(results){            
                if (typeof (results) !== 'undefined' && results != null && ngl.isObject(results)) {
                    if (typeof (results.data) !== 'undefined' && results.data != null && ngl.isArray(results.data)) {
                        if (typeof (results.data[0]) !== 'undefined' && results.data[0] != null){
                            var msg = "Copy and Paste the following value into the License File for the Module License Key: " + results.data[0].toString();
                            ngl.Alert("Module License Key", msg, 400, 400);
                        }
                    }
                }
                return;
            }

            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>;                    
                                                        
                if (control != 0){

                    oModuleWndCtrl = new SelectionGridWndCtrl(); 
                    oModuleWndCtrl.loadDefaults("divModuleWnd", "Generate Module License Key", "NGLSysModule", oModuleSaveCB);

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
            /*This fixes the icon alignment in the grid buttons. Now the right side doesn't get cut off and it looks more centered. Should probably add this code to common.css */    
            .k-button-icontext .k-icon, .k-button-icontext .k-image, .k-button-icontext .k-sprite { margin-right:0; } 
        </style>
    </div>
</body>
</html>