<%@ Page Title="Free Trial Registration Page" Language="C#"  AutoEventWireup="true" CodeBehind="FreeTrialRegistration.aspx.cs" Inherits="DynamicsTMS365.FreeTrialRegistration" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Free Trial</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />                
        <style>

html,

body
{height:100%; margin:0; padding:0;}

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
        <script src="Scripts/NGL/v-8.5.4.006/splitter2.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script> 


      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white; ">
                    <div id="tab" class="menuBarTab"></div>
                </div>
                <div id="top-pane">
                  <div id="horizontal" style="height: 100%; width: 100%; ">
                        <div id="left-pane">
                            <div class="pane-content">
                                <div><span>Menu</span></div>
                                <div id="menuTree"></div>                                                               
                            </div>
                        </div>
                        <div id="center-pane">
                            <% Response.Write(PageErrorsOrWarnings); %>

                           <div class="ngl-blueBorder";>
                               <div style="padding: 10px;">
                                   <div class="pane-content">
                                       <div><h1>Dynamics TMS 365 Free Trial</h1></div> 
                                   </div>   
                                                                   
                                   <div id="FTMsg" class="pane-content" style="position: relative; float: left; display: inline-block; min-width:200px; width: 35%; margin-right:25px;"></div>
                                   <div style="position: relative; float: left; display: inline-block; width: calc(65% - 25px); margin-top: 10px;"> 
                                      <h4><span style="text-decoration:underline;">Looking&nbsp;For LTL Rate Shopping and Dispatching? Here's A Quick Start Guide</span><span style="text-decoration:underline;"></span></h4>
                                       
                                       <p>Step 1:  <a href="../RateShopping">Begin rate shopping Here</a></p>
                                       <p>Step 2:  Enter order information and click calculate</p>
                                       <p>Step 3:  Rates will be displayed using  public carriers and rates</p>
                                       <h4><span style="text-decoration:underline;">Interested In Joining? Next Steps</span></h4>
                                       <p>Step 4:  <a href="../SignUp">Sign Up</a></p>
                                       <p>Step 5:  <a href="../Contact">Register your carriers with Next Generation Logistics</a></p>
                                       <p>Step 6:  <a href="../RateShopping">Continue rate shopping with your negotiated carrier rates and contracts</a></p> 
                                   </div>
                               </div>
                           </div>
                             
                        </div>
  
                    </div>
                </div>
                <div id="bottom-pane" style="height: 100%; width: 100%; background-color: #daecf4; ">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>

          

    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>    
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>     
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
        var tObj = this;
        var tPage = this;
        var tokenHash = location.hash;
        if (typeof (tokenHash) != 'undefined' && tokenHash != null){
            if (ADAL.isCallback(tokenHash)){
                var requestInfo = ADAL.getRequestInfo(tokenHash);
                ADAL.saveTokenFromHash(requestInfo);
            }
        }
        var strLogInMsg = "";
        var blnLoggedIn = isLoggedIn(strLogInMsg);
        
        

        var resGetFreeTrialInfo = function (data) {
            if (data == null){ alert("data null"); return; }
            var s = "<div style='display:inline-block; float:left;'><ul><li style='list-style-type: none;'><h3>User Friendly Name</h3></li><li style='list-style-type: none;'>" + data.strField
                + "</li><li style='list-style-type: none;'><h3>User Email</h3></li><li style='list-style-type: none;'>" + data.strField2
                + "</li><li style='list-style-type: none;'><h3>Free Trial Expires</h3></li><li style='list-style-type: none;'>" + data.strField3
                + "</li></ul></div>";                
                
            $("#FTMsg").html(s);
        }

        function getFreeTrialInfo(resultFunc) {
            var urls = "api/SSOA/GetFreeTrialInfo/0";
            $.ajax({
                async: false,
                type: "GET",
                url: urls,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("Get Free Trial Info Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = true;
                                        if (ngl.isFunction(resultFunc)) {
                                            resultFunc(data.Data[0]);
                                        }                  
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Free Trial Info not found"; }
                            ngl.showErrMsg("Get Free Trial Info Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                    ngl.showErrMsg("Get Free Trial Info Failure", sMsg, null);                        
                }
            });
        }

        function register_Click(){
            signIn();
            //signInOut();
        }

        $(document).ready(function () {   
            
            var PageMenuTab = <%=PageMenuTab%>;            
           
           //var tokenHash = location.hash;
           // if (typeof (tokenHash) != 'undefined' && tokenHash != null){
           //     if (ADAL.isCallback(tokenHash)){
           //         var requestInfo = ADAL.getRequestInfo(tokenHash);
           //         ADAL.saveTokenFromHash(requestInfo);
           //     }
            // }
            var blnReload = true;
            if (blnLoggedIn === true){ blnReload = updateValidUserMsgs();}else {blnReload = validateUser();}            
            control = <%=UserControl%>;           
            if(blnReload == true && control == 0){
                var uc = localStorage.NGLvar1452;
                document.location = oredirectUri + "?uc=" + uc;
                return;
            } else {
                if (strLogInMsg.length > 0) {
                    ngl.showInfoNotification("Login Message",strLogInMsg,null)
                }
            }

          
           
            //debugger;
            
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS

            if(control != 0){
                getFreeTrialInfo(resGetFreeTrialInfo);
            }
            else{
                $("#FTMsg").html("<span><strong>Must have a valid Microsoft Live ID</strong></span></br><p>The first time you sign in with your Microsoft account a Dynamics TMS 365 trial account will be created. The trial period is 30 days from the first log in. </p><div class='pane-content'><a id='btnRegister' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='register_Click();' href='#' style='margin:6px; vertical-align: top;'>Register</a></div>");
            }


        });


    </script>
   
    </div>


    </body>

</html>