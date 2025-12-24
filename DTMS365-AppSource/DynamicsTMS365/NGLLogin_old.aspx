<%@ Page Title="NGL Login Page" Language="C#"  AutoEventWireup="true" CodeBehind="NGLLogin_old.aspx.cs" Inherits="DynamicsTMS365.NGLLogin_old" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Login</title>                 
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />      
        <style>

html,

body
{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:auto; }

</style>
                 
    </head>
     <body>       
        <script src="../Scripts/kendoR32023/jquery.min.js"></script>  
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>         
        <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>  
        <%-- <script src="Scripts/NGL/v-8.5.4.006/app.js"></script>  --%>    
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script> 
        <script src="Scripts/NGL/Ctrls/dataEntryForm.js"></script> 
        
         
      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">                          
        
                    
         <div >
             <% Response.Write(PageErrorsOrWarnings); %>
                    <br /><br />
                    <div class="ngl-blueBorderFullPage" style="min-width: 450px;">  
                        <div style="margin: 10px 10px 0px 10px;">                     
                            
                            <span style="margin:6px; vertical-align: middle;" >
                                <a id="aLogoURL" href="<% Response.Write(HomeTabHrefURL); %>" target="_blank"><img id="imgLogo" border="0" alt="Public Web" src="<% Response.Write(HomeTabLogo); %>" ></a>
                            </span>  
                            <h4 id="WelcomeMessage">Please sign in using one of the following options</h4>                         
                            <hr />  
                        </div>                       
                        <div style="padding-left: 10px; padding-right: 10px;">
                            <div style="position:relative; float:left; display:inline-block; width:250px;">                        
                                <h4>Dynamics TMS&trade; Account</h4> 
                                <% Response.Write(PageDataTableHTML); %>                               
                                <%--<div id="userDataEntry" style="display:none">
                                <table  style="border: none; width: 249px;" >
                                    <tr style="border: none;" >
                                        <td style="border: none;" >User Name:</td>
                                        <td style="border: none;" ><input id="txtNGLUserName" /></td>
                                    </tr>
                                    <tr style="border: none;" >
                                        <td style="border: none;" >Password:</td>
                                        <td style="border: none;" ><input id="txtNGLPass" /></td>
                                    </tr>
                                </table>
                               </div>--%>
                               <a id='NGLSignIn' class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md"  onclick='NGLSignIn();' href='#' style='margin:6px; vertical-align: top;'>
                                    <span class='k-icon k-i-user' style='vertical-align: middle;'></span>
                                    <span id='NGLsignInText' style='vertical-align: middle;'>Sign In</span>
                                </a>
                            </div>
                            <div style="position:relative; float:left; display:inline-block; width:calc(100% - 250px);  margin-top:10px; " >
                                <p><a id="lnkForgotPassword" href="ForgotPassword.aspx"><img id="imgForgotPassword" alt="Forgot Password"  src="Content/NGL/ForgotPassword.gif" style="border:0;" /></a></p>
                            
                                <p><a id="lnkRequestAccount" href="RequestAnAccount.aspx"><img id="imgRequestAccount" src="Content/NGL/RequestAccount.gif"  style="border:0;" /></a></p>
                                                                                                       
                                <p><a id="lnkBackToPublicSite" href="<% Response.Write(HomeTabHrefURL); %>" target="_blank"><img id="imgBackToPublicSite" src="Content/NGL/BackToPublicSite.gif"  style="border:0;" /></a>   
                            </div>

                            <div id="divWindowsAccount">
                                <h4>Or Your Microsoft Account</h4> 
                                <%--<button id="btnAADSignIn" onclick="callerSignIn()"> Sign In </button>--%>
                                <a id='AADSignIn' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='callerSignIn();' href='#' style='margin:6px; vertical-align: top;'>
                                    <span class='k-icon k-i-user' style='vertical-align: middle;'></span>
                                    <span id='AADsignInText' style='vertical-align: middle;'>Sign In</span>
                                </a>
                                <hr />
                            </div>

                        </div>

                         <div id ="divFreeTrial" style="padding-left: 10px; padding-right: 10px;">
                                <h4>Would you like to know more?</h4> 
                             <%--<button id="btnAADSignIn" onclick="callerSignIn()"> Sign In </button>--%>
                                   <%-- <a id='freeSignIn' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick="window.location.href='/RegisterForFreeTrial.aspx'" style='margin:6px; vertical-align: top;'>
                                        <span class='k-icon k-i-user' style='vertical-align: middle;'></span>
                                        <span id='freesignInText' style='vertical-align: middle;'>Free Trial</span>
                                    </a>  --%>
                             <a id='freeSignIn' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick="window.open('https://dynamicstms.com/', '_blank');" style='margin:6px; vertical-align: top;'>
                                    <span class='k-icon k-i-user' style='vertical-align: middle;'></span>
                                    <span id='freesignInText' style='vertical-align: middle;'>Get More Information</span>
                                </a>  
                                <input id="signInText" type="hidden" />
                            </div> 

                        <div style="position:relative; clear:both; float:none; display:inline-block; margin:10px;" id="bottom-pane"  >
                            <hr />
                            <div style="margin:5px,5px,5px,5px; padding:5px,5px,5px,5px; border:solid  #7bd2f6 2px; background-color: #7bd2f6; border-radius: 10px;" >
                                <a id="aFooterURL" href="<% Response.Write(HomeTabHrefURL); %>" target="_blank"> <% Response.Write(PageFooterHTML); %> </a>
                            </div>
                            <br />
                        </div>                       
                    </div>                              
        </div>

    
     <% Response.Write(AuthLoginNotificationHTML); %>         
    <script>
        //debugger;
       //start ADAL properties
        var opostLogoutRedirectUri = '<% Response.Write(WebBaseURI); %>';
        var oredirectUri = '<% Response.Write(WebBaseURI); %>' + getCurentFileName(); 
        var oidaClient = '<% Response.Write(idaClientId); %>';  //
        var oAuth2instasnce = 'https://login.microsoftonline.com/';
        var oAuth2tenant = 'common';
        var caller = '<%=Caller%>';
        //loadAuthContext();
        loadCallerAuthContext();


        function loadDataEntryCtrl(){
            $('#userDataEntry').load('Views/dataEntrtyForm.html');
            //$errorMessage.empty();
            var ctrl = dataEntryForm;

            if (!ctrl)
                return;
            var dataRows = [];
            var dataRow = new dataEntryRow();
            dataRow.caption = "User Name:"
            dataRow.id = "txtNGLUserName";
            dataRow.controltype =  "0";
            dataRow.metaData = "";
            dataRows.push(dataRow)
            var dataRowP = new dataEntryRow();
            dataRowP.caption = "Password:"
            dataRowP.id = "txtNGLPass";
            dataRowP.controltype =  "0";
            dataRowP.metaData = "";
            dataRows.push(dataRowP)
            var html =  $('#userDataEntry');
            //debugger;
             //   .find(".data-container").empty();
            //    $panel.html($html.html());
            //    ctrl.postProcess(html,dataRows);
            ctrl.postProcess(html,dataRows);
            $('#userDataEntry').show();
            // Load View HTML
            //$.ajax({
            //    type: "GET",
            //    url: "Views/dataEntrtyForm.html",
            //    dataType: "html",
            //}).done(function (html) {

            //    // Show HTML Skeleton (Without Data)
            //    var $html = $(html);
            //    $html.find(".data-container").empty();
            //    $panel.html($html.html());
            //    ctrl.postProcess(html,dataRows);

            //}).fail(function () {
            //    $errorMessage.html('Error loading page.');
            //}).always(function () {

            //});
        }

        // Modified by RHR for v-8.5.4.005 on 01/29/2024 moved security information fro filter into header
        //     filter is no longer used.
        function NGLSignIn(){
            
            //debugger;
            //var sfilter = new NGLClass14();
            var sfilter = new NGLClass14();
            sfilter.NGLvar1455 = $("#txtNGLUserName").data("kendoMaskedTextBox").value();
            sfilter.NGLvar1450 = $("#txtNGLPass").data("kendoMaskedTextBox").value();

            //data: { filter: JSON.stringify(sfilter) },
            $.ajax({
                async: false,
                type: "GET",
                url: "api/SSOA/GetNGLSSOAToken",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: { filter: '' },
                headers: { "NGLClass14": JSON.stringify(sfilter) },
                success: function (data) {
                    try{
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        //debugger;
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null  && data.Errors.length > 0) { 
                                blnErrorShown = true;
                                ngl.showErrMsg("User Login Validation Failure", data.Errors,null);                                   
                            }
                            else {                                    
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray( data.Data)) {
                                    if (data.Data.length > 0  && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])){
                                        blnSuccess = true;
                                        dataitem = data.Data[0];
                                        if ('UserName' in dataitem  && typeof (dataitem.UserName) !== 'undefined' && dataitem.UserName !== null ) { 
                                            //username is required and cannot be null
                                            localStorage.NGLvar1455 = dataitem.UserName;
                                        } else {
                                            blnSuccess = false;
                                            strValidationMsg = "The user name or password is not valid.  Please try again";
                                        }
                                        if (blnSuccess === true && 'UserSecurityControl' in dataitem) { localStorage.NGLvar1452 = dataitem.UserSecurityControl; }
                                        if (blnSuccess === true && 'USATUserID' in dataitem) { localStorage.NGLvar1451 = dataitem.USATUserID; }
                                        if (blnSuccess === true && 'USATToken' in dataitem) { localStorage.NGLvar1454 = dataitem.USATToken; }  
                                        if (blnSuccess === true && 'SSOAControl' in dataitem) { localStorage.NGLvar1472 = dataitem.SSOAControl; } 
                                        if (blnSuccess === true && 'SSOAUserEmail' in dataitem) { localStorage.NGLvar1458 = dataitem.SSOAUserEmail; }                                      
                                        localStorage.NGLvar1474 = ""; //data.Data[0].NGLvar1474 aka JWT Token;
                                        if (blnSuccess === true && 'UserFriendlyName' in dataitem) { localStorage.NGLvar1457 = dataitem.UserFriendlyName; }
                                        //for now we do not do anything with the read method
                                        //this.edit(row.PageControl, row.PageName, row.PageDesc, row.PageCaption, row.PageCaptionLocal, row.PageDataSource, row.PageSortable, row.PagePageable, row.PageGroupable, row.PageEditable, row.PageDataElmtControl, row.PageElmtFieldControl, row.PageAutoRefreshSec, row.PageFormControl)
                                        if (blnSuccess === true ) {
                                            var divWelcome = document.getElementById('WelcomeMessage');
                                            if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                                                divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1455;
                                            }
                                            var uc = localStorage.NGLvar1452;
                                            if ('CatControl' in dataitem  &&  dataitem.CatControl == '2') { 
                                                caller = "Default.aspx";
                                                //document.location = "../Default?uc=" + uc;
                                                //document.location = "../Login?uc=" + uc + "&caller=" + caller;
                                            } 
                                            //else {
                                            //    document.location = caller + "?uc=" + uc;
                                            //}
                                            document.location = "../Login?uc=" + uc + "&caller=" + caller;
                                           
                                            
                                        }
                                    }                                       
                                }
                            }
                        } 
                        if (blnSuccess === false  && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) {strValidationMsg ="Account information not found"; }
                            ngl.showErrMsg("User Login Validation Failure",strValidationMsg,null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name,err.description,null);
                    }                        
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Authentication Failure");
                    ngl.showErrMsg("User Login Validation", sMsg, null);                        
                }
            });
          

        }
        
        //validateLogin(caller);
        $(document).ready(function () {
            //debugger;
            //
            control = <%=UserControl%>;
            //loadDataEntryCtrl();
            //if(blnReload == true ){
            //    var uc = localStorage.NGLvar1452;
            //    document.location = caller + "?uc=" + uc;
            //    return;
            //}

            //Hide the WindowsAccount and FreeTrial buttons if this is not AppSource
            // Modified by RHR for v-8.5.3.007 we only hide the free trial button when is IsAppSource is true
            $("#divWindowsAccount").show();
            var blnIsAppource = '<%=ConfigurationManager.AppSettings["IsAppSource"]%>';
            if(blnIsAppource.toLowerCase() === "true"){
                //$("#divWindowsAccount").show();
                $("#divFreeTrial").show();
            }
            else{
                //$("#divWindowsAccount").hide();
                $("#divFreeTrial").hide();
            }

            //Code to change logo  and logo url based on web config
            document.getElementById('imgLogo').src = '<%=ConfigurationManager.AppSettings["HomeTabLogo"]%>';
            document.getElementById('aLogoURL').setAttribute('href','<%=ConfigurationManager.AppSettings["HomeTabHrefURL"]%>');


            $("#txtNGLUserName").kendoMaskedTextBox();
            $("#txtNGLPass").kendoMaskedTextBox();


            $("#btnAADSignIn").kendoButton({
                click: function(e) {
                    signIn();                    
                }

            });


        });


    </script>
    
    </div>


    </body>

</html>