<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="DynamicsTMS365.ForgotPassword" %>

<html>
    <head >
        <title>DTMS Forgot Password</title>         
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
    <body> >       
        <%=jssplitter2Scripts%>

      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div >
             <% Response.Write(PageErrorsOrWarnings); %>
                    <br /><br />
                    <div class="ngl-blueBorderFullPage" style="min-width: 450px;">  
                        <div style="margin: 10px;">                    
                            
                            <span style="margin:6px; vertical-align: middle;" >
                                <a id="aLogoURL" href="<% Response.Write(HomeTabHrefURL); %>"><img id="imgLogo" border="0" alt="Public Web" src="<% Response.Write(HomeTabLogo); %>" ></a>
                            </span>     
                            <h3>Dynamics TMS 365 and NGL NEXTrack&trade; Collaboration Portal Login Options</h3>
                            <h4 id="WelcomeMessage">If you forgot your password just enter your user name below.  We'll assign you a temporary password and send it to you.</h4>                         
                            <a id="lnkBackToLogIn" href="NGLLogin.aspx"><img id="imgGoBackToLogIn" src="Content/NGL/GoBackToLogIn.gif" /></a>
                            <hr />                                                             
                            <div id="tblResetPassword">
                                <table  style="border: none;" >
                                    <tr style="border: none;" >
                                        <td style="border: none;" >User Name:</td>
                                        <td style="border: none;" ><input id="txtNGLUserName" /></td>
                                    </tr>
                                    <tr style="border: none;" >
                                        <td style="border: none;" ></td>
                                        <td style="border: none;" ><input type="image" name="SendPassword" id="sendPassword" src="Content/NGL/SendMyPassword.gif" onclick="SendPassword_Click();" /></td>
                                    </tr>
                                </table>
                            </div> 
                             <%--<hr />--%>
                        </div>   
                        <div style="position:relative; clear:both; float:none; display:none; margin:10px;" id="bottom-pane"  >
                            <hr />
                            <div style="margin:5px,5px,5px,5px; padding:5px,5px,5px,5px; border:solid  #7bd2f6 2px; background-color: #7bd2f6; border-radius: 10px;" >
                                <% Response.Write(PageFooterHTML); %> 
                            </div>
                            <br />
                        </div>                       
                    </div>                              
        </div>

    
     <% Response.Write(AuthLoginNotificationHTML); %>      
    <script>
       
        
        var PageControl = '<%=PageControl%>';    
        var tObj = this;
        var tPage = this;
        function SendPassword_Click(){
            //var sfilter =  encodeURIComponent($("#txtNGLUserName").data("kendoMaskedTextBox").value());
            var sfilter =  ngl.encodeNGLReservedCharacters($("#txtNGLUserName").data("kendoMaskedTextBox").value());
            //sfilter = sfilter.replace("\\",String.fromCharCode(200) );
            //sfilter = sfilter.replace("/",String.fromCharCode(201) );
            //alert('filter ' + sfilter);
            //url: "api/SSOA/PostNGLSendPassword/" + sfilter,
            $.ajax({
                async: false,
                type: "POST",
                url: "api/SSOA/PostNGLSendPassword/''" ,
                contentType: "application/json; charset=utf-8",
                headers: { "NGLUserName": sfilter },
                success: function (data) {
                    try{                        
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null  && data.Errors.length > 0) {                                
                                ngl.showErrMsg("User Login Validation Failure", data.Errors,null);                                   
                            }
                            else {    
                                var divWelcome = document.getElementById('WelcomeMessage');
                                if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                                    divWelcome.innerHTML = "A temporary password has been assigned to your account.  Please check your email for your new password, allow up to 15 minutes for your email to arrive before trying again.";
                                }                                
                                $("#tblResetPassword").hide();                                
                            }
                        } 
                    } catch (err) {
                        ngl.showErrMsg(err.name,err.description,null);
                    }                        
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Unable to send new password");
                    ngl.showErrMsg("Send New Password Failure", sMsg, null);                        
                }
            });
          

        }
       
        $(document).ready(function () {
            control = <%=UserControl%>;
            $("#txtNGLUserName").kendoMaskedTextBox();
        });


    </script>
    
    </div>


    </body>

</html>