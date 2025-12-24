<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="DynamicsTMS365.ChangePassword" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Change Password</title>
        <%=cssReference%>
        <style>

html,

body
{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:auto; }

</style>
                 
    </head>
     <body> 
    <%=jssplitter2Scripts%>
        
         
      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">                          
                   
         <div >
             <% Response.Write(PageErrorsOrWarnings); %>
                    <br /><br />
                    <div class="ngl-blueBorderFullPage" style="min-width: 450px;">  
                        <div style="margin: 10px 10px 0px 10px;">                     
                            
                            <span style="margin:6px; vertical-align: middle;" >
                                <a id="aLogoURL" href="../NGLLogin" ><img id="imgTMS" border="0" alt="TMS Login Page" src="../Content/NGL/TMSIcon.png" ></a>
                            </span>  
                            <h4 id="WelcomeMessage">Your adminitrator as requested that you change your password</h4>                         
                            <hr />  
                        </div>                       
                        <div style="padding-left: 10px; padding-right: 10px;">
                            <div style="position:relative; float:left; display:inline-block; width:90%;">                        
                                <h4>Dynamics TMS&trade; Account</h4> 
                                                  
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
                                 <div id="PasswordDiv" style="padding-bottom: 10px;">
                                    <div style="padding: 0px 0px 0px 10px;">
                                        <p>To change your password: Confirm your current password, then enter a new password below and click the "Change Password" button</p>
                                    </div>
                                    <div style="padding: 0px 0px 0px 10px;">
                                        <table style="border-collapse: collapse; border-spacing: 0; border: none;">
                                            <tr>
                                                <td style="text-align: right; border: none;"><strong>Current Password</strong></td>
                                                <td style="border: none;">
                                                    <input id="txtOldPass" type="password" maxlength="100" style="width: 150px;" /></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; border: none;"><strong>Create New Password</strong></td>
                                                <td style="border: none;">
                                                    <input id="txtNewPass" type="password" maxlength="100" style="width: 150px;" /></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; border: none;"><strong>Confirm New Password</strong></td>
                                                <td style="border: none;">
                                                    <input id="txtConfirmNewPass" type="password" maxlength="100" style="width: 150px;" /></td>
                                            </tr>
                                            <tr>       
                                                <td style="text-align: right; border: none;">&nbsp;</td>
                                                <td style="border: none;">
                                                    <button id="btnChangePass" onclick="btnChangePass_Click();" type="button" style="width: 150px;">Change Password</button></td>
                                            </tr>
                                            <tr>
                                                <td style="border: none;"></td>
                                                <td style="border: none;">
                                                    <a id="aGoBack" href="../NGLLogin" ><img id="imgGoBack" border="0" alt="TMS Login Page" src="../Content/NGL/GoBack.gif" ></a>
                                            </tr>
                                        </table>
                                    </div>
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
                    </div>                             
        </div>
           

    
     <% Response.Write(AuthLoginNotificationHTML); %> 
     <script>
        <% Response.Write(ADALPropertiesjs); %>
        var PageControl = '<%=PageControl%>';
        var tObj = this;
        var tPage = this;
        <% Response.Write(NGLOAuth2); %>
        <% Response.Write(PageCustomJS); %>

         function btnChangePass_Click() {
             var sNewP = $("#txtNewPass").val();
             var sCurrentP = $("#txtOldPass").val();
             var sConfirmP = $("#txtConfirmNewPass").val();
             if (sNewP != sConfirmP) {
                 ngl.Alert("Confirm Password Failure", "The new passwords do not match, please try again", 400, 400);
                 return;
             }

            var oPostNew = new postNewPassword();
            oPostNew.loadDefaults(changePassSuccessCallBack);
            oPostNew.postPassword(sNewP, sCurrentP);
        }

              function changePassSuccessCallBack(oEventParameters) {
                  //add additional validation here
                  if (oEventParameters.error != null) {
                      $("#txtOldPass").val("");
                      $("#txtNewPass").val("");
                      $("#txtConfirmNewPass").val("");
                  } else {
                      document.location = "../Default";
                  }
              }


              //validateLogin(caller);
              $(document).ready(function () {
                  //debugger;
                  //
                  control = <%=UserControl%>;           
            

                  $("#txtOldPass").kendoMaskedTextBox();
                  $("#txtNewPass").kendoMaskedTextBox();
                  $("#txtConfirmNewPass").kendoMaskedTextBox();

                  $("#btnChangePass").kendoButton({ imageUrl: "../Content/NGL/Keys16.png" });

            });


     </script>
    
    </div>


    </body>

</html>
