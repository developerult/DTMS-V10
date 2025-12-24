<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterCarriers.aspx.cs" Inherits="DynamicsTMS365.MasterCarriers" %>

<!DOCTYPE html>

<html>
<head>
    <title>DTMS Master Carriers</title>        
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

        
         var oMstrCarGrid = null;
         
         <% Response.Write(PageCustomJS); %>

         //*************  execActionClick  ****************
         function execActionClick(btn, proc){
             if (btn.id == "btnOpenCarrier"){ location.href = "LECarrierMaint"; }
             else if (btn.id == "btnAddMstrCar" ){ 
                 if (typeof (tPage["wdgtAddMstrCarWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtAddMstrCarWndDialog"])){
                     tPage["wdgtAddMstrCarWndDialog"].show();                
                 } else{alert("Missing HTML Element (wdgtAddMstrCarWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
             }
             else if (btn.id == "btnRefresh"){ refresh(); } 
             else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
         }

         function refresh(){ ngl.readDataSource(oMstrCarGrid); }

         //*************  Call Back Functions ****************  
         function MstrCarGridDataBoundCallBack(e,tGrid){ oMstrCarGrid = tGrid; }

         function AddMstrCarWndCB(oResults) {
             if (typeof (oResults) === 'undefined' || ngl.isObject(oResults) === false) { return; }
             if(oResults.widget.sNGLCtrlName === "wdgtAddMstrCarWndDialog" && oResults.source === "saveSuccessCallback"){
                 refresh(); 
                 //execActionClick("Refresh");
                 oResults.widget.executeActions("close");
             } 
         }

         function savePostPageSettingSuccessCallback(results){ return; } //for now do nothing when we save the pk
         function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){ return; } //for now do nothing when we save the pk

         //*************  Page Level Functions ****************


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

        /*This fixes thead icon alignment in the grid buttons. Now the right side doesn't get cut off and it looks more centered. Should probably add this code to common.css */
        .k-button-icontext .k-icon, .k-button-icontext .k-image, .k-button-icontext .k-sprite { margin-right:0; } 
    </style>  
    </div>
</body>
</html>