<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreatePage.aspx.cs" Inherits="DynamicsTMS365.CreatePage" %>

<%--Created By RHR on 02/22/17 for v-8.0 Content Management Create Page--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head >
        <title>DTMS Content Management Create Page</title>          
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />               
        <%--<link href="Content/kendoR32023/kendo.common-office365.min.css" rel="stylesheet" />
        <link href="Content/kendoR32023/kendo.office365.min.css" rel="stylesheet" /> 
        <link href="Content/kendoR32023/kendo.office365.mobile.min.css" rel="stylesheet" />--%>
       <style>
            html,
body{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
       </style>
    </head>
    <body>       
       <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>              
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/splitter3.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script>

      <div id="example" style="height: 99%; width: 99%;  margin-top: 2px;">      
        <div id="vertical" style="height: 98%; width: 98%; " >
            <div id="menu-pane" >
                <div class="pane-content"> 
                    <span style="float:left; display:inline-block;">                      
                        
                        <span style="margin:6px; vertical-align: middle;" >
                            <a id="aLogoURL" href="<% Response.Write(HomeTabHrefURL); %>"><img id="imgLogo" border="0" alt="Public Web" src="<% Response.Write(HomeTabLogo); %>" ></a>
                        </span>                    
                    </span>
                    <span style="float:right; display:inline-block;">
                        <span style="margin:6px; vertical-align: middle;">
                                <a href="Settings.aspx"><img border="0" alt="Settings" src="Content/NGL/Settings32.png" width="32" height="32" /></a>
                            </span>
                    </span>
                </div>
            </div>
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%; background-color: white;">                        
                    <div id="left-pane">
                        <div class="pane-content">
                            <div><span>Menu</span></div>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <div id="PageDetails">
                            <div class="pane-content" >
                                <span id="pwmovePrev" style="display:none;">
                                    &nbsp;&nbsp;
                                    <img id="imgpwmovePrev" onclick="movePrevFastTab(pwData,'pwmovePrev','pwmoveNext');" border="0" alt="Expand" src="../Content/NGL/MovePreviousBlue16.png" width="12" height="12" /></span>
                                 <span id="pwmovePrevblank" style="display:normal;">
                                    &nbsp;&nbsp;
                                    <img id="imgpwmovePrevblank" border="0" alt="Expand" src="../Content/NGL/Blank16.png" width="12" height="12" /></span>
                              
                                <span style="font-size:small; font-weight:bold">Create Page Wizard</span>
                                <span id="pwmoveNext" style="display:normal;">
                                    &nbsp;&nbsp;
                                    <img id="imgpwmoveNext" onclick="moveNextFastTab(pwData,'pwmovePrev','pwmoveNext');" border="0" alt="Collapse" src="../Content/NGL/MoveNextBlue16.png" width="12" height="12" /></span>
                                 <span id="pwmoveNextblank" style="display:none;">
                                    &nbsp;&nbsp;
                                    <img id="imgpwmoveNextblank" border="0" alt="Expand" src="../Content/NGL/Blank16.png" width="12" height="12" /></span>
                              
                                <br />
                                <div id="pgWiz1" style="display:normal;">
                                    <label>Create Page Step 1 Enter Name and Desciption</label>
                                    <ul class="dateentrylist" id="cmPagefieldlist">
                                        <li class="dataentryLabel"><label for="cmLocalKeyFilter">Key Filter:</label></li>
                                        <li><input id="cmLocalKeyFilter" value="Reference Meta Data Here" /></li>
                                        <li class="filterfieldLabel"><label for="cmLocalValueFilter">Default Filter:</label></li>
                                        <li><input id="cmLocalValueFilter" value="Reference Meta Data Here" /></li>
                                        <li class="filterfieldLabel"><label for="cmLocalValueLocalFilter">Localized Filter:</label></li>
                                        <li><input id="cmLocalValueLocalFilter" value="Reference Meta Data Here" /></li>
                                        <li class="filterfieldLabel"><label for="cmLocalModDateFilter">Mod Date Filter:</label></li>
                                        <li><input id="cmLocalModDateFilter" value="Reference Meta Data Here" /></li>
                                        <li class="filterfieldLabel"><label for="cmLocalModUserFilter">Mod User Filter:</label></li>
                                        <li><input id="cmLocalModUserFilter" value="Reference Meta Data Here" /></li>

                                    </ul>
                                </div>
                                <div id="pgWiz2" style="display:none;">
                                    <label>Create Page Step 2</label>
                                </div>
                                <div id="pgWiz3" style="display:none;">
                                    <label>Create Page Step 3</label>
                                </div>
                                <div id="pgWiz4" style="display:none;">
                                    <label>Create Page Step 4</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="right-pane">
                        <div class="pane-content">
                            <div class="pageDetails">
                                

                                    <%--<span id="ExpandLocalizationFiltersSpan" style="display:none;">
                                        &nbsp;&nbsp;
                                        <img id="imgExpandAvailableLoads" onclick="expandFastTab('ExpandLocalizationFiltersSpan','CollapseLocalizationFiltersSpan','LocalizationFiltersHeader','LocalizationFiltersDetail');" border="0" alt="Expand" src="../Content/NGL/expand.png" width="12" height="12" /></span>
                                    <span id="CollapseLocalizationFiltersSpan" style="display:normal;">
                                        &nbsp;&nbsp;
                                        <img id="imgCollapseAvailableLoads" onclick="collapseFastTab('ExpandLocalizationFiltersSpan','CollapseLocalizationFiltersSpan','LocalizationFiltersHeader','LocalizationFiltersDetail');" border="0" alt="Collapse" src="../Content/NGL/collapse.png" width="12" height="12" /></span>
                                    <span id="LocalizationFiltersHeader">
                                        <label>Filters</label></span>
                                    <div id="LocalizationFiltersDetail">
                                        <ul class="filterfieldlist" id="LocalizationFiltersfieldlist">
                                            <li class="filterfieldLabel"><label for="cmLocalKeyFilter">Key Filter:</label></li>
                                            <li><input id="cmLocalKeyFilter" value="Reference Meta Data Here" /></li>
                                            <li class="filterfieldLabel"><label for="cmLocalValueFilter">Default Filter:</label></li>
                                            <li><input id="cmLocalValueFilter" value="Reference Meta Data Here" /></li>
                                            <li class="filterfieldLabel"><label for="cmLocalValueLocalFilter">Localized Filter:</label></li>
                                            <li><input id="cmLocalValueLocalFilter" value="Reference Meta Data Here" /></li>
                                            <li class="filterfieldLabel"><label for="cmLocalModDateFilter">Mod Date Filter:</label></li>
                                            <li><input id="cmLocalModDateFilter" value="Reference Meta Data Here" /></li>
                                            <li class="filterfieldLabel"><label for="cmLocalModUserFilter">Mod User Filter:</label></li>
                                            <li><input id="cmLocalModUserFilter" value="Reference Meta Data Here" /></li>
                                        </ul>
                                    </div>
                                </div>--%>

                            </div>
                        </div>
                    </div>
                </div>
            
            </div>
            <div id="bottom-pane" style="height: 100%; width: 100%; background-color: #daecf4; ">
                <div class="pane-content">
                    <div>
                        <span><p>This secure site exists to provide On-Line Tendering, Shipment Accept/Reject, Shipment Status, Shipment Tracking, Shipment Settlement and Proof of Delivery (POD) information. If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href="mailto:support.nextgeneration.com">support.nextgeneration.com</a></p></span>
                    </div>
                </div>
            </div>
        </div>
         
    <script>
        //start ADAL properties
        var opostLogoutRedirectUri = '<% Response.Write(WebBaseURI); %>';
        var oredirectUri = '<% Response.Write(WebBaseURI); %>' + getCurentFileName(); 
        var oidaClient = '<% Response.Write(idaClientId); %>';  //
        var oAuth2instasnce = 'https://login.microsoftonline.com/';
        var oAuth2tenant = 'common';
        loadAuthContext();
        //NOTE:   validateUser(); must be called in the docuemnt.ready function
        //End ADAL properties

        var PageControl = '<%=PageControl%>'; 
        var control = 0;
        var UserToken = '<%=UserToken%>';
        var tObj = this;
        var tPage = this;
        var groupTypeKey = 56;
        var groupSubTypeKey = 57;
        var formListKey = 28;
        var DataElementKey = 4;
        var ElementFieldKey = 5;
        var pwData; 
        $(document).ready(function () {
                       
           var serverUserControl = <%=UserControl%>;
            control = localStorage.NGLvar1452 
            ngl.UserValidated365(True,control,getCurentFileName(),serverUserControl);

            pwData = new wizardData('pgWiz',4);
            //everntaully this will only say this
            var PageReadyJS = <%=PageReadyJS%>;           

        });


    </script>
    
     
    </div>


    </body>

</html>
