<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestAnAccount.aspx.cs" Inherits="DynamicsTMS365.RequestAnAccount" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Request An Account</title>         
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
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script> 

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
                            <h4 id="WelcomeMessage">If you need an account just fill out the form below.  One of our support representatives will contact you shortly.  Please note that all fields are required.</h4>                         
                            <a id="lnkBackToLogIn" href="NGLLogin.aspx"><img id="imgGoBackToLogIn" src="Content/NGL/GoBackToLogIn.gif" /></a>

                            <hr />   
                            <h4 id="RequestAccountSuccess" style="display:none;"></h4>                                                  
                            <div id="tblRequestAnAccount">   
                                 <table class="tblNB">
                                    <tr>
                                         <th class="tblNB-Bold">First & Last Name:</th>
                                         <td><input type="text" id="txtName" class="txtBox-midLeftExpand" onfocus="this.select();" /></td>
                                     </tr>

                                    <tr>
                                         <th class="tblNB-Bold">Company Name:</th>        
                                        <td><input  type="text" id="txtCompanyName" class="txtBox-midLeftExpand" onfocus="this.select();" /></td>
                                     </tr>
                               
                                    <tr>
                                         <th class="tblNB-Bold">Address:</th>
                                         <td><input  type="text" id="txtAddress" class="txtBox-midLeftExpand" onfocus="this.select();" /></td>        
                                    </tr>
            
                                   <tr>       
                                        <th class="tblNB-Bold">City / State / Zip:</th>      
                                        <td>
                                            <input  type="text" id="txtCity" class="txtBox-midLeft" onfocus="this.select();" style="width:200px;"/>
                                            <input  type="text" id="txtState" class="txtBox-midLeft" onfocus="this.select();" style="width:200px;" />
                                            <input  type="text" id="txtZipCode" class="txtBox-midLeft" onfocus="this.select();" style="width:100px;" />
                                        </td>
                                    </tr>
        
        
                                   <tr>        
                                        <th class="tblNB-Bold">Phone:</th>       
                                        <td><input  type="text" id="txtPhone" class="txtBox-midLeft" onfocus="this.select();" style="width:100px;" /></td>       
                                    </tr>      
        
                                   <tr>        
                                        <th class="tblNB-Bold">Fax:</th>        
                                        <td><input type="text" id="txtFax" class="txtBox-midLeft" onfocus="this.select();" style="width:100px;" /></td>        
                                    </tr>      
        
                                   <tr>       
                                        <th class="tblNB-Bold">Email:</th>      
                                        <td><input  type="text" id="txtEmail" class="txtBox-midLeftExpand" onfocus="this.select();" /></td>        
                                    </tr>
               
                                   <tr>       
                                        <th class="tblNB-Bold">Comments:</th>        
                                        <td><textarea id="txtComments" class="txtBox-midLeftExpand" onfocus="this.select();" style="height:75px;"></textarea></td>
                                    </tr>
                
                                   <tr>
                                        <td><input type="image" id="cmdSendRequest" src="../Content/NGL/SendRequest.gif" onclick="SendRequest_Click();" /></td>
                                    </tr>       
                                    
                                    </table>

                            </div> 
                           
                        </div>   
                        <div style="position:relative; clear:both; float:none; display:inline-block; margin:10px;" id="bottom-pane"  >
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
        function SendRequest_Click(){
            var sname = $("#txtName").data("kendoMaskedTextBox").value();
            var scompanyName = $("#txtCompanyName").data("kendoMaskedTextBox").value();           
            var saddress = $("#txtAddress").data("kendoMaskedTextBox").value();
            var scity = $("#txtCity").data("kendoMaskedTextBox").value();
            var sstate = $("#txtState").data("kendoMaskedTextBox").value();       
            var szipCode = $("#txtZipCode").data("kendoMaskedTextBox").value();
            var sphone = $("#txtPhone").data("kendoMaskedTextBox").value();      
            var sfax = $("#txtFax").data("kendoMaskedTextBox").value();
            var semail = $("#txtEmail").data("kendoMaskedTextBox").value();
            var scomments =  $("#txtComments").data("kendoMaskedTextBox").value();
            var sValidationMessage = "Please enter required fields: ";
            var sSpacer = "";
            var bDataValid = true;
            if (typeof (sname) === 'undefined' || sname === null ||  isEmpty(sname) ) 
            {
                bDataValid = false;
                sValidationMessage += sSpacer + "Name";
                sSpacer = "; "
            }
            if (typeof (scompanyName) === 'undefined' || scompanyName === null ||  isEmpty(scompanyName) ) 
            {
                bDataValid = false;
                sValidationMessage += sSpacer + "Company Name";
                sSpacer = "; "
            }
            if (typeof (saddress) === 'undefined' || saddress === null ||  isEmpty(saddress) ) 
            {
                bDataValid = false;
                sValidationMessage += sSpacer + "Address";
                sSpacer = "; "
            }
            if (typeof (scity) === 'undefined' || scity === null ||  isEmpty(scity) ) 
            {
                bDataValid = false;
                sValidationMessage += sSpacer + "City";
                sSpacer = "; "
            }
            if (typeof (szipCode) === 'undefined' || szipCode === null ||  isEmpty(szipCode) ) 
            {
                bDataValid = false;
                sValidationMessage += sSpacer + "Zip/Postal Code";
                sSpacer = "; "
            }
            if ( typeof (sphone) === 'undefined' || sphone === null || isEmpty(sphone) ) 
            {
                bDataValid = false;
                sValidationMessage += sSpacer + "Phone";
                sSpacer = "; "
            }
            if (typeof (semail) === 'undefined' || semail === null ||  isEmpty(semail) ) 
            {
                bDataValid = false;
                sValidationMessage += sSpacer + "Email";
                sSpacer = "; "
            }
            if (bDataValid == false)
            {
                ngl.showValidationMsg("Cannot Send Request",sValidationMessage,null)
                return;
            }

            var body = "<h4>A user has requested access to NEXTrack.  Here is the information that was submitted:</h4><br/><p>" + 
                "Name: " + sname + "<br/>" + 
                "Company Name: " + scompanyName + "<br/>" + 
                "Address: " + saddress + "<br/>" + 
                "City: " + scity + "<br/>" + 
                "State: " + sstate + "<br/>" + 
                "Zip Code: " + szipCode + "<br/>" + 
                "Phone: " + sphone + "<br/>" + 
                "Fax: " + sfax + "<br/>" + 
                "Email: " + semail + "<br/>" + 
                "Comments: " + scomments + "<br/></p>" +  
                "You can log in to NEXTrack as an administrator to set up this account.";

            var res = sendEmail("<%=AccountRequestSendToEmail %>", "<%=SmtpFromAddress %>", "", "", "<%=AccountRequestEmailSubject %>", body);

            if (res === 1){
                $("#tblRequestAnAccount").hide();
                $("#WelcomeMessage").text("Your request has been sent.  One of our support representatives will contact you shortly.");
                //$("#RequestAccountSuccess").show;
            }
        }
       
       
        $(document).ready(function () {
            control = <%=UserControl%>;

            var kmask = new kendoMasks();
            kmask.loadDefaultMasks();     
            var phoneMask = kmask.getMask("phone_number");

            $("#txtName").kendoMaskedTextBox();
            $("#txtCompanyName").kendoMaskedTextBox();
            $("#txtAddress").kendoMaskedTextBox();
            $("#txtCity").kendoMaskedTextBox();
            $("#txtState").kendoMaskedTextBox();          
            $("#txtZipCode").kendoMaskedTextBox();
            $("#txtPhone").kendoMaskedTextBox({
                mask: phoneMask
            });
            $("#txtFax").kendoMaskedTextBox({
                mask: phoneMask
            });
            $("#txtEmail").kendoMaskedTextBox();
           

            
            $("#txtComments").kendoMaskedTextBox();
           
        });


    </script>
    
    </div>


    </body>

</html>
