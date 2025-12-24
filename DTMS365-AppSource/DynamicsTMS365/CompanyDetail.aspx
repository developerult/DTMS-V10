<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyDetail.aspx.cs" Inherits="DynamicsTMS365.CompanyDetail" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Company Detail</title>
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
                var tObjPG = this; 

        <% Response.Write(NGLOAuth2); %>

        
                <% Response.Write(PageCustomJS); %>

                //************* Action Menu Functions ********************
                function execActionClick(btn, proc){
                    if(btn.id == "btnRefresh" ){ refresh(); }  
                    else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }  
                    else { ngl.pgNavigation(btn.id, true); }// modified by RHR for v-8.5.2.006 on 12/22/2022 added page navigation menu method

                }

                function refresh(){ 
                    if(wdgtepCompDetailsEdit){ wdgtepCompDetailsEdit.read(0); } 
                }
             
                //************* Call Back Functions **********************        
                function savePostPageSettingSuccessCallback(results){
                    //for now do nothing when we save the pk
                }
                function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
                    //for now do nothing when we save the pk           
                }      

                //************* Page Functions ***************************

                $(function () {
                    //wire focus of all numerictextbox widgets on the page
                    $("input[type=number]").bind("focus", function () {
                        var input = $(this);
                        clearTimeout(input.data("selectTimeId")); //stop started time out if any
                        var selectTimeId = setTimeout(function(){ input.select(); });
                        input.data("selectTimeId", selectTimeId);
                    }).blur(function(e) {
                        clearTimeout($(this).data("selectTimeId")); //stop started timeout
                    });
                })

                $(document).ready(function () {
                    var PageMenuTab = <%=PageMenuTab%>;
              
                    if (control != 0){        
            
                    }
                    var PageReadyJS = <%=PageReadyJS%>;
                    menuTreeHighlightPage(); //must be called after PageReadyJS
                    var divWait = $("#h1Wait");
                    if (typeof(divWait) !== 'undefined') { divWait.hide(); }
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