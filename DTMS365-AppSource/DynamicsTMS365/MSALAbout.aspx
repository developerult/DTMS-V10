<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MSALAbout.aspx.cs" Inherits="DynamicsTMS365.MSALAbout" %>


<!DOCTYPE html>

<html>
    <head >
        <title>DTMS About</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   
        <style>

html,

body
{
    height:100%;
    margin:0;
    padding:0;
}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

</style>
    </head>
    <body>       
        <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>  
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
<%--        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>--%>
        
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bluebird/3.3.4/bluebird.min.js" class="pre"></script> 
        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/0.1.1/js/msal.min.js"></script>
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
                            <a id='btnRegister' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='register_Click();' href='#' style='margin:6px; vertical-align: top;'>Register</a>
                            <h4 id="WMessage"></h4>
                             <h5 id="UData"></h5> 
                            <div id="AboutContent" class="ngl-blueBorder">
                               <div style="padding: 10px;">
                                   <div id="editableCont"></div>
                               </div>
                            </div>
                            <div id="AboutEditor"><textarea id="ContEditor" style="height: 90%; width: 90%;"></textarea></div>
                            <input id="edPgDet" type="hidden" />

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
        var clientApplication;
          //start ADAL properties
        var opostLogoutRedirectUri = '<% Response.Write(WebBaseURI); %>';
        var oredirectUri = '<% Response.Write(WebBaseURI); %>' + getCurentFileName(); 
        var oidaClient = '<% Response.Write(idaClientId); %>';  //
        var oAuth2instasnce = 'https://login.microsoftonline.com/';
        var oAuth2tenant = 'common';
        //loadAuthContext();
        //NOTE:   validateUser(); must be called in the docuemnt.ready function
        //End ADAL properties
        function authCallback(errorDesc, token, error, tokenType) {
            //This function is called after loginRedirect and acquireTokenRedirect. Not called with loginPopup
            // msal object is bound to the window object after the constructor is called.
            if (token) {
                //debugger;
                var s = tokentype;
            }
            else {
                var divWelcome = document.getElementById('WMessage');
                divWelcome.innerHTML = "error: " + error + ": " + errorDesc;                
            }
        }

        if (!clientApplication) {
            new Msal.Storage('localStorage');
            clientApplication = new Msal.UserAgentApplication('1b4fc9cf-1c01-4694-a115-b9dd263746db', null, authCallback);
            clientApplication.cacheLocation = 'localStorage';
        }

        https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=6731de76-14a6-49ae-97bc-6eba6914391e&response_type=token&redirect_uri=http%3A%2F%2Flocalhost%2Fmyapp%2F&scope=https%3A%2F%2Fgraph.microsoft.com%2Fmail.read&response_mode=fragment&state=12345&nonce=678910&prompt=none&domain_hint={{consumers-or-organizations}}&login_hint={{your-username}}

        var PageControl = '<%=PageControl%>';            
        var control = 0;
        var tObj = this;
        var tPage = this;
        var resGetAboutEditableContent = function (data) {
            //set the value of the editor and the html page content
            $("#ContEditor").data("kendoEditor").value(data.Content);
            $("#editableCont").html(data.Content);
           
            $('#edPgDet').val(data.PageDetControl);
        }

        function getAboutEditableContent() {
            var e = new editorContent();
            e.PageControl = PageControl;
            e.USec = 0;
            e.EditorName = "ContEditor";
            e.Content = "";
            e.PageDetControl = $('#edPgDet').val();

            getEditorContentNoAuth(JSON.stringify(e), resGetAboutEditableContent);
        }


        var resSaveAboutContentEditor = function (data) {          
            //set the html page content
            var c = $("#ContEditor").data("kendoEditor").value();
            $("#editableCont").html(c);
            
            $("#AboutEditor").hide();
            $("#AboutContent").show();           
        }

        function saveAboutContentEditor() {
            var h = new editorContent();
            h.PageControl = PageControl;
            h.USec = localStorage.NGLvar1452;
            h.EditorName = "ContEditor";
            h.Content = $("#ContEditor").data("kendoEditor").value();
            h.PageDetControl = $('#edPgDet').val();

            saveEditorContent(h, resSaveAboutContentEditor);
        }

        function cancelAboutContentEditor() {            
            getAboutEditableContent();
            $("#AboutEditor").hide();
            $("#AboutContent").show();
        }

        function editPageDynamicContent(p){
            $("#AboutContent").hide();
            $("#AboutEditor").show();                
            $("#ContEditor").data("kendoEditor").refresh();
        }

        function execActionClick(btn, proc){
            if(btn.id == "btnTMSContEdit"){
                editPageDynamicContent(proc);
            }
            
        }

        function register_Click(){
            document.location = "NGLLogin.aspx?caller=About.aspx";
        }

        $(document).ready(function () {  
            var PageMenuTab = <%=PageMenuTab%>;
            //debugger;
            //var cToken = clientApplication.getCachedToken('1b4fc9cf-1c01-4694-a115-b9dd263746db','rkrtechnical@gmail.com')
            //if (cToken != null) {
            //    var user = clientApplication.getUser();
            //    var userData = '';
            //    var divWelcome = document.getElementById('WMessage');
            //    var divUserData = document.getElementById('UData');
            //    if (user) {
            //        for (var property in user) {
            //            if (user.hasOwnProperty(property)) {
            //                var sPropName = property;
            //                var sPropvalue = user[property];
            //                userData = userData + sPropName + ':' + sPropvalue;
            //                //var $entry = $template;
            //                //$entry.find(".view-data-claim").html(property);
            //                //$entry.find(".view-data-value").html(user[property]);
            //                //output += $entry.html();
            //            }
            //        }
            //        divWelcome.innerHTML = "Welcome " + user.name;
            //        divUserData.innerHTML = userData

            //    } else {
            //        divWelcome.innerHTML = "Not Authorized ";
               
            //    }
            //}
            //clientApplication.acquireTokenSilent(['1b4fc9cf-1c01-4694-a115-b9dd263746db'])
            //clientApplication.acquireTokenSilent(["user.read"])
            clientApplication.acquireTokenSilent(["user.read"])
            .then(function (token) {
                var user = clientApplication.getUser();
                var userData = '';
                var divWelcome = document.getElementById('WMessage');
                var divUserData = document.getElementById('UData');
                if (user) {
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
            }, function (error) {
                clientApplication.acquireTokenPopup(["user.read"]).then(function (accessToken) { 
                    var user = clientApplication.getUser();
                    var userData = '';
                    var divWelcome = document.getElementById('WMessage');
                    var divUserData = document.getElementById('UData');
                    if (user) {
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
               }, function (error) { 
                   if (error.indexOf("interaction_required" !== -1)) {
                       document.location = "NGLLogin.aspx?caller=About.aspx";
                   } else {
                       var divWelcome = document.getElementById('WMessage');
                       divWelcome.innerHTML = "Auth Error: " + error + " -- " + errorElement
                   }                 
               }); 

                
            });

            //var token = clientApplication.acquireTokenSilent();
            //if (token === null) {
            //    document.location = "NGLLogin.aspx?caller=About.aspx";
            //} else {
            //    var user = clientApplication.getUser();
            //    var userData = '';
            //    var divWelcome = document.getElementById('WMessage');
            //    var divUserData = document.getElementById('UData');
            //    if (user) {
            //        for (var property in user) {
            //            if (user.hasOwnProperty(property)) {
            //                var sPropName = property;
            //                var sPropvalue = user[property];
            //                userData = userData + sPropName + ':' + sPropvalue;
            //                //var $entry = $template;
            //                //$entry.find(".view-data-claim").html(property);
            //                //$entry.find(".view-data-value").html(user[property]);
            //                //output += $entry.html();
            //            }
            //        }
            //        divWelcome.innerHTML = "Welcome " + user.name;
            //        divUserData.innerHTML = userData

            //    } else {
            //        divWelcome.innerHTML = "Not Authorized ";
               
            //    }
            //}
            


            <%--var blnReload = validateUser();
            control = <%=UserControl%>;
            if(blnReload == true && control == 0){
                var uc = localStorage.NGLvar1452;
                document.location = oredirectUri + "?uc=" + uc;
                return;
            }--%>

            var PageReadyJS = <%=PageReadyJS%>; 

            $("#AboutEditor").hide();
            $("#AboutContent").show();         
            $('#edPgDet').val(0);
            getAboutEditableContent();

        });


    </script>
    <style>


    </style>
    </div>


    </body>

</html>