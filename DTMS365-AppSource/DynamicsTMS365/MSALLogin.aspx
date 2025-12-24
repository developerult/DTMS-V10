<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MSALLogin.aspx.cs" Inherits="DynamicsTMS365.MSALLogin" %>


<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Free Trial Registration</title>            
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
        <script src="../Scripts/kendoR32023/jquery.min.js"></script>  
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
        <%--<script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>--%>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bluebird/3.3.4/bluebird.min.js" class="pre"></script> 
        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/0.1.1/js/msal.min.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>         
        <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>  
        <%-- <script src="Scripts/NGL/v-8.5.4.006/app.js"></script>  --%>    
        <%--<script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script> --%>

      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">                          
                
                    
         <div >
                    <br /><br />
                    <div class="ngl-blueBorder">  
                        <span style="margin:6px; vertical-align: middle;">
                                <a href="Default.aspx"><img border="0" alt="Home" src="../Content/NGL/Home32.png" width="32" height="32"></a>
                        </span>
                        <span style="margin:6px; vertical-align: middle;" >
                            <a href="http://www.nextgeneration.com"><img border="0" alt="NGL" src="../Content/NGL/nextracklogo.GIF" ></a>
                        </span>                              
                        <div style="padding: 10px;">                        
                            <h4 id="WelcomeMessage"></h4> 
                            <span id='signInText' style='display:none;'>Sign In</span>
                            <div>
                                <span>Login Using your Windows Live or Azure ID                                 
                                <br />
                                <br />
                                <button id="btnAADSignIn" onclick="signIn()"> Sign In </button></span>
                            </div>
                      
                            <div>
                                <h4 id="NGLLoginMessage">Or Your NGL Account</h4>
                                <span>User Name: </span>&nbsp;<input id="txtNGLUserName" />
                                <br />
                                <span>Password: </span>&nbsp;&nbsp;&nbsp;<input id="txtNGLPass" />
                                <br />
                                <br />
                                <button id="btnNGLSignIn"> Sign In </button> 
                            </div>                                                                                          
                        </div>  
                        <h5 id="UserData"></h5>
                    </div>                              
        </div>

    <%--<script class="pre">
        var userAgentApplication = new Msal.UserAgentApplication(
            "<% Response.Write(idaClientId); %>", null, function (errorDes, token, error, tokenType) {
              // this callback is called after loginRedirect OR acquireTokenRedirect (not used for loginPopup/aquireTokenPopup)
        })
        userAgentApplication.loginPopup(["user.read"]).then( function(token) {
            var user = userAgentApplication.getUser();
            // signin successful
        }, function (error) {
            // handle error
        });
    </script>--%>
            
    <script>
        var clientApplication;


        var opostLogoutRedirectUri = '<% Response.Write(WebBaseURI); %>';
        var oredirectUri = '<% Response.Write(WebBaseURI); %>' + getCurentFileName(); 
        var oidaClient = '<% Response.Write(idaClientId); %>';  //
        var oAuth2instasnce = 'https://login.microsoftonline.com/';
        var oAuth2tenant = 'common';
        var caller = '<%=Caller%>';

        if (!clientApplication) {            
            new Msal.Storage('localStorage');
            clientApplication = new Msal.UserAgentApplication('1b4fc9cf-1c01-4694-a115-b9dd263746db', null, authCallback);
            clientApplication.cacheLocation = 'localStorage';
            //clientApplication.getCachedToken
        }

        <%--var ADAL = new AuthenticationContext({
            instance: 'https://login.microsoftonline.com/',
            tenant: 'common', //COMMON OR YOUR TENANT ID

            clientId: '<% Response.Write(idaClientId); %>', 

            redirectUri: 'http://localhost:44320/NGLLogin', //REPLACE WITH YOUR REDIRECT URL

            callback: userSignedIn,
            popUp: true
        });--%>

        function signIn() {
            clientApplication.loginPopup(["user.read"]).then(onSignin);
            //ADAL.login();
        }

        function onSignin(idToken) {
            // Check Login Status, Update UI
            //debugger;
            var user = clientApplication.getUser();
            var userData = '';
            var divWelcome = document.getElementById('WelcomeMessage');
            var divUserData = document.getElementById('UserData');
            if (user) {
                
                clientApplication.acquireTokenSilent(["user.read"]).then(function (token) {
                    console.log("Success acquiring access token");
                    localStorage.msToken = token;
                    }, function (error) {                     
                        // interaction required 
                        if (error.indexOf("interaction_required" != -1)) { 
                            clientApplication.acquireTokenPopup(["user.read"]).then(function (token) {
                                console.log("Success acquiring access token"); 
                                }, function (error) { 
                                    console.log("Failure acquiring token: " + error); 
                                    }); 
                            } 
                        }); 

                for (var property in user) {
                    if (user.hasOwnProperty(property)) {
                        var sPropName = property;
                        var sPropvalue = user[property];
                        userData = userData + sPropName + ':' + sPropvalue;
                        //var $entry = $template;
                        //$entry.find(".view-data-claim").html(property);
                        //$entry.find(".view-data-value").html(user[property]);
                        //output += $entry.html();
                    }
                }
                divWelcome.innerHTML = "Welcome " + user.name;
                divUserData.innerHTML = userData

            } else {
                divWelcome.innerHTML = "Not Authorized ";
               
            }

        }

        function userSignedIn(err, token) {
            console.log('userSignedIn called');
            if (!err) {
                console.log("token: " + token);
                showWelcomeMessage();
            }
            else {
                //console.error("error: " + err);
                var divWelcome = document.getElementById('WelcomeMessage');
                divWelcome.innerHTML = "error: " + err;
            }
        }

        function showWelcomeMessage() {
            var user = ADAL.getCachedUser();
            var divWelcome = document.getElementById('WelcomeMessage');
            divWelcome.innerHTML = "Welcome " + user.profile.name;
        }

        function authCallback(errorDesc, token, error, tokenType) {
            //This function is called after loginRedirect and acquireTokenRedirect. Not called with loginPopup
            // msal object is bound to the window object after the constructor is called.
            if (token) {
                //debugger;
                var s = tokentype;
            }
            else {
                var divWelcome = document.getElementById('WelcomeMessage');
                divWelcome.innerHTML = "error: " + error + ": " + errorDesc;                
            }
        }

        //loadAuthContext();

       
        $(document).ready(function () {
            //debugger;
            

            //var blnReload = validateUser();
            
            <%--var caller = '<%=Caller%>';
            if (localStorage.SignedIn == "t") {
                var uc = localStorage.NGLvar1452;
                document.location = caller + "?uc=" + uc;
               return;
            }--%>
            //control = <%=UserControl%>;
            //alert(caller);
            //if(blnReload == true && control == 0){
            //    var uc = localStorage.NGLvar1452;
            //    document.location = oredirectUri + "?uc=" + uc;
            //    return;
            //}
            //var treeview = <%=getMenuTree()%>;

            $("#txtNGLUserName").kendoMaskedTextBox();
            $("#txtNGLPass").kendoMaskedTextBox();

            $("#btnNGLSignIn").kendoButton({
                click: function(e) {
                    //debugger;
                    //var sfilter = new NGLClass14();
                    var sfilter = new NGLClass14();
                    sfilter.NGLvar1455 = $("#txtNGLUserName").data("kendoMaskedTextBox").value();
                    sfilter.NGLvar1450 = $("#txtNGLPass").data("kendoMaskedTextBox").value();

                    $.ajax({
                        async: false,
                        type: "GET",
                        url: "api/SSOA",
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: {filter: JSON.stringify(sfilter)},
                        success: function (data) {
                            localStorage.NGLvar1455 = data.Data[0].UserName;
                            localStorage.NGLvar1452 = data.Data[0].UserSecurityControl;                            
                            localStorage.NGLvar1451 = data.Data[0].USATUserID
                            localStorage.NGLvar1474 = ""; //data.Data[0].NGLvar1474 aka JWT Token;
                           // debugger;
                            localStorage.NGLvar1454 = data.Data[0].USATToken; //aka App Token
                                        
                            //alert("localStorage.NGLvar1455: " + localStorage.NGLvar1455)
                            var divWelcome = document.getElementById('WelcomeMessage');

                            divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1455;
                        },
                        error: function (xhr, textStatus, error) {
                            if (error == "Bad Request") {
                                var jsonResponse = JSON.parse(xhr.responseText);
                                alert(jsonResponse.message);
                            }
                            else {
                                alert("Unknown Error: " + JSON.stringify(xhr));
                            }
                        }
                    });
                }

            });

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