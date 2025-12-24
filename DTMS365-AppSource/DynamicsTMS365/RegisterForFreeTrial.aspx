<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterForFreeTrial.aspx.cs" Inherits="DynamicsTMS365.RegisterForFreeTrial" %>

<!DOCTYPE html>

<html>
<head>
    <title>DTMS Register For Free Trial</title>
    <%=cssReference%>
    <style>
        html,
        body {
            height: 100%;
            margin: 0;
            padding: 0;
        }

        html {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
            overflow: hidden;
        }
    </style>

</head>
<body>
    <%=jssplitter4Scripts%>
    <div id="h1Wait" style="margin: 10px;">
        <span style="margin: 6px; vertical-align: middle;"><a href="http://www.nextgeneration.com">
            <img border="0" alt="NGL" src="../Content/NGL/nextracklogo.GIF"></a> </span><span>Please Wait Loading
                <img border="0" alt="Home" src="../Content/NGL/loading5.gif">
            </span>
    </div>

    <div id="example" style="height: 100%; width: 100%; margin-top: 2px; display: none;">

        <div id="vertical" style="height: 98%; width: 98%;">
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%;">
                    <div id="center-pane">
                        <% Response.Write(PageErrorsOrWarnings); %>

                        <div class="ngl-blueBorder">

                            <div style="margin: 10px 10px 0px 10px;">
                                <span style="margin: 6px; vertical-align: middle;">
                                    <a href="Default.aspx">
                                        <img border="0" alt="Home" src="../Content/NGL/Home32.png" width="32" height="32"></a>
                                </span>
                                <span style="margin: 6px; vertical-align: middle;">
                                    <a href="http://www.nextgeneration.com">
                                        <img border="0" alt="NGL" src="../Content/NGL/nextracklogo.GIF"></a>
                                </span>
                                <h3>Register For Free Trial</h3>
                                <h4 id="WelcomeMessage">Your username will be your email address</h4>
                                <hr />
                            </div>

                            <div style="padding: 10px;">
                                <div style="margin-left: 15px;">
                                    <div class="k-content">
                                        <form id="registerForm" data-role="validator" novalidate="novalidate">
                                            <ul id="fieldlist">
                                                <%--<li>Your username will be your email address</li>--%>
                                                <li>
                                                    <label for="txtEmail" class="required">Email<span style="color: red;"> *</span></label>
                                                    <input type="email" id="txtEmail" name="Email" class="k-textbox" placeholder="myname@example.net" required data-email-msg="Email format is not valid" maxlength="255" style="width: 219px;" />
                                                </li>

                                                <li>
                                                    <label for="txtFirstName">First Name<span style="color: red;"> *</span></label>
                                                    <input type="text" class="k-textbox" name="First Name" id="txtFirstName" required="required" maxlength="100" style="width: 219px;" />
                                                </li>
                                                <li>
                                                    <label for="txtLastName">Last Name<span style="color: red;"> *</span></label>
                                                    <input type="text" class="k-textbox" name="Last Name" id="txtLastName" required="required" maxlength="100" style="width: 219px;" />
                                                </li>
                                                <li>
                                                    <label for="txtFriendlyName">Friendly Name</label>
                                                    <input type="text" class="k-textbox" name="Friendly Name" id="txtFriendlyName" maxlength="100" style="width: 219px;" />
                                                </li>
                                                <li>
                                                    <label for="txtWorkPhone">Work Phone<span style="color: red;"> *</span></label>
                                                    <input type="text" class="k-textbox" name="Work Phone" id="txtWorkPhone" required="required" />
                                                </li>
                                                <li>
                                                    <label for="txtWorkPhoneExt">Work Phone Ext</label>
                                                    <input type="text" class="k-textbox" name="Work Phone Ext" id="txtWorkPhoneExt" maxlength="20" />
                                                </li>
                                                <li>
                                                    <label for="txtPass1">Password<span style="color: red;"> *</span></label>
                                                    <input id="txtPass1" type="password" class="k-textbox" data-type="text" name="Password" required="required"
                                                        pattern='.{6,25}' validationmessage='Password must be between 6-25 characters' style="width: 219px;" />
                                                    <span data-for='txtPass1' class='k-invalid-msg'></span>
                                                </li>
                                                <li>
                                                    <label for="txtPass2">Confirm Password<span style="color: red;"> *</span></label>
                                                    <input id="txtPass2" type="password" class="k-textbox" data-type="text" name="Confirm Password" required="required"
                                                        data-greaterdate-field="Password" data-greaterdate-msg='Passwords do not match'
                                                        pattern='.{6,25}' validationmessage='Password must be between 6-25 characters' style="width: 219px;" />
                                                    <span data-for='txtPass2' class='k-invalid-msg'></span>
                                                </li>
                                                <li>
                                                    <button type="button" class="k-primary" data-role="button" data-click='save'>Register</button>
                                                </li>
                                            </ul>
                                        </form>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>

                </div>
            </div>
            <div id="bottom-pane" class="k-block" style="height: 50px; width: 100%;">
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
            //var tokenHash = location.hash;
            //if (typeof (tokenHash) != 'undefined' && tokenHash != null){
            //    if (ADAL.isCallback(tokenHash)){
            //        var requestInfo = ADAL.getRequestInfo(tokenHash);
            //        ADAL.saveTokenFromHash(requestInfo);
            //    }
            //}
            //var strLogInMsg = "";
            //var blnLoggedIn = isLoggedIn(strLogInMsg);
        
            function save(e) {
                var validator = $("#registerForm").data("kendoValidator");
                if (validator.validate()) {

                    var results = new SSOResults();
                    results.SSOAControl = 1; // NGL.FreightMaster.Data.Utilities.NGL
                    results.SSOAName = "NGL"; 
                    //results.SSOAClientID = "";
                    //results.SSOALoginURL = "";
                    results.SSOARedirectURL = oredirectUri; //use Web config WebBaseURI for new users; empty when using NGL Authentication
                    results.SSOAClientSecret = $("[name='Password']").val(); 
                    results.SSOAAuthCode = oAuth2tenant; //use tenant use Web config idaTenant for new users;  default = common
                    results.UserSecurityControl = 0; 
                    results.UserName = $("[id='txtEmail']").val();  
                    results.UserLastName = $("[id='txtLastName']").val();
                    results.UserFirstName = $("[id='txtFirstName']").val();
                    results.USATUserID = $("[id='txtEmail']").val();
                    results.SSOAUserEmail = $("[id='txtEmail']").val();
                    results.UserFriendlyName = $("[id='txtFriendlyName']").val();
                    results.UserWorkPhone = $("[id='txtWorkPhone']").val();
                    results.UserWorkPhoneExt = $("[id='txtWorkPhoneExt']").val();

                    //These items will be populatedby the system so default values are fine for now
                    results.UserFriendlyName = $("[id='txtFriendlyName']").val();

                    try {
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: "api/SSOA/PostNGLFreeTrialSignup",
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            data: JSON.stringify(results),
                            success: function (data) {
                                try{
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null  && data.Errors.length > 0) { 
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Create NGL Free Trial User Failure", data.Errors,null);                                   
                                        }
                                        else {                                    
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray( data.Data)) {
                                                if (data.Data.length > 0  && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])){
                                                    blnSuccess = true;
                                                    dataitem = data.Data[0];
                                                    if ('UserName' in dataitem  && typeof (dataitem.UserName) !== 'undefined' && dataitem.UserName !== null ) { 
                                                        //////username is required and cannot be null
                                                        ////localStorage.NGLvar1455 = dataitem.UserName;
                                                    } else {
                                                        blnSuccess = false;
                                                        strValidationMsg = "The user name or password is not valid.  Please try again";
                                                    }
                                                    ////if (blnSuccess === true && 'UserSecurityControl' in dataitem) { localStorage.NGLvar1452 = dataitem.UserSecurityControl; }
                                                    ////if (blnSuccess === true && 'USATUserID' in dataitem) { localStorage.NGLvar1451 = dataitem.USATUserID; }
                                                    ////if (blnSuccess === true && 'USATToken' in dataitem) { localStorage.NGLvar1454 = dataitem.USATToken; }  
                                                    ////if (blnSuccess === true && 'SSOAControl' in dataitem) { localStorage.NGLvar1472 = dataitem.SSOAControl; }
                                                    ////if (blnSuccess === true && 'SSOAUserEmail' in dataitem) { localStorage.NGLvar1458 = dataitem.SSOAUserEmail; }                                      
                                                    ////localStorage.NGLvar1474 = ""; //data.Data[0].NGLvar1474 aka JWT Token;
                                                    ////if (blnSuccess === true && 'UserFriendlyName' in dataitem) { localStorage.NGLvar1457 = dataitem.UserFriendlyName; }
                                                    //////for now we do not do anything with the read method
                                                    //////this.edit(row.PageControl, row.PageName, row.PageDesc, row.PageCaption, row.PageCaptionLocal, row.PageDataSource, row.PageSortable, row.PagePageable, row.PageGroupable, row.PageEditable, row.PageDataElmtControl, row.PageElmtFieldControl, row.PageAutoRefreshSec, row.PageFormControl)
                                                    if (blnSuccess === true ) { 
                                                        ngl.showSuccessMsg("Free Trial Account Requested", null);
                                                        localStorage.clear();
                                                        ////    localStorage.SignedIn = "t";
                                                        ////    var divWelcome = document.getElementById('WelcomeMessage');
                                                        ////    if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                                                        ////        divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1455;
                                                        ////    }                                                   
                                                    }
                                                }                                       
                                            }
                                        }
                                    } 
                                    if (blnSuccess === false  && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) {strValidationMsg ="Could not create a new NGL Free Trial."; }
                                        ngl.showErrMsg("Create NGL Free Trial User Failure",strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name,err.description,null);
                                }                        

                                //redirect to the home page
                                ////document.location = '<% Response.Write(WebBaseURI); %>' + "Default" + "?uc=" + localStorage.NGLvar1452;
                                document.location = '<% Response.Write(WebBaseURI); %>' + "Default";

                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Authentication Failure");
                                ngl.showErrMsg("User Login Validation", sMsg, null);                        
                            }
                        });
                    } catch (err) {
                        ngl.showErrMsg("Post Single Sign On Results Failure", err.message, null);
                    }

                }
            }
              
              
              
            function compare(str1, str2) {
                if (str1 === str2){
                    return true;
                }
                else{
                    return false;
                }
            }


            $(document).ready(function () {   

                var PageReadyJS = <%=PageReadyJS%>;

                var kmask = new kendoMasks();
                kmask.loadDefaultMasks();     
                var phoneMask = kmask.getMask("phone_number");

                $("#txtWorkPhone").kendoMaskedTextBox({
                    mask: phoneMask
                });

                var container = $("#registerForm");
                kendo.init(container);
                container.kendoValidator({
                    rules: {
                        greaterdate: function (input) {
                            if (input.is("[data-greaterdate-msg]") && input.val() != "") {                                    
                                var pass2 = input.val();
                                //alert("[name='" + input.data("greaterdateField") + "']");
                                var pass1 = $("[name='" + input.data("greaterdateField") + "']").val();
                                //alert("Pass1: " + pass1 + " Pass2: " + pass2);
                                return compare(pass1, pass2);
                            }

                            return true;
                        }
                    }
                });


            });


        </script>

        <style>
            #fieldlist {
                margin: 0;
                padding: 0;
            }

                #fieldlist li {
                    list-style: none;
                    /*padding-bottom: .7em;*/
                    text-align: left;
                }

                #fieldlist label {
                    display: block;
                    /*padding-bottom: .3em;*/
                    font-weight: bold;
                    text-transform: uppercase;
                    /*font-size: 12px;*/
                    color: #444;
                }

                #fieldlist li .k-widget:not(.k-tooltip),
                #fieldlist li .k-textbox {
                    margin: 0 5px 5px 0;
                }

            span.k-widget.k-tooltip-validation {
                display;
                inline-block;
                width: 160px;
                text-align: left;
                border: 0;
                padding: 0;
                margin: 0;
                background: none;
                box-shadow: none;
                color: red;
            }

            .k-tooltip-validation .k-warning {
                display: none;
            }
        </style>

    </div>


</body>

</html>
