<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="DynamicsTMS365.SignUp" %>


<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Sign Up Page</title>
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
                          <div id="divSignUpPageWrapper">
                              <div style="padding:10px 10px 0px 10px;">
                                  <div id="divSignUpAdminCont"></div>
                              </div>
                              <div id="centerHTML">
                                  <div id="enterComp" class="pane-content">
                                      <div class="ngl-blueBorder">
                                          <div style="padding: 10px;">
                                              <div style="display: inline-block; float: left;">
                                                  <ul>
                                                      <li><h3>Company Info</h3></li>
                                                      <li>Company Name</li>
                                                      <li class="required"><input id="txtLegalEntity" class="k-input k-textboxlong" /> *</li>
                                                      <li>3 Letter Prefix</li>
                                                      <li class="required"><input id="txtCompAbrev" class="k-input k-textboxlong" /> *</li>
                                                      <li>Warehouse Name</li>
                                                      <li class="required"><input id="txtCompName" class="k-input k-textboxlong" /> *</li>
                                                      <li>Warehouse ID</li>
                                                      <li class="required"><input id="txtCompAlphaCode" class="k-input k-textboxlong" /> *</li>
                                                  </ul>
                                              </div>
                                              <div style="display: inline-block; float: left;">
                                                  <ul>
                                                      <li><h3>Ship From Address</h3></li>
                                                      <li>Address 1</li>
                                                      <li class="required"><input id="txtAddress1" class="k-input k-textboxlong" /> *</li>
                                                      <li>Address 2</li>
                                                      <li class="NotRequired"><input id="txtAddress2" class="k-input k-textboxlong" /></li>
                                                      <li>Address 3</li>
                                                      <li class="NotRequired"><input id="txtAddress3" class="k-input k-textboxlong" /></li>
                                                      <li>City</li>
                                                      <li class="required"><input id="txtCity" class="k-input k-textboxlong" /> *</li>
                                                      <li>State</li>
                                                      <li class="required"><input id="txtState" class="k-input k-textboxlong" /> *</li>
                                                      <li>Postal Code</li>
                                                      <li class="required"><input id="txtZip" class="k-input k-textboxlong" /> *</li>
                                                      <li>Country</li>
                                                      <li class="required"><input id="txtCountry" class="k-input k-textboxlong" /> *</li>
                                                      <li>&nbsp; </li>
                                                      <li><button id="btn" type="button">Submit</button></li>
                                                  </ul>
                                              </div>
                                              <div style="display: inline-block; float: left;">
                                                  <ul>
                                                      <li><h3>Company Contact</h3></li>
                                                      <li>Contact Name</li>
                                                      <li class="required"><input id="txtContName" class="k-input k-textboxlong" /> *</li>
                                                      <li>Title</li>
                                                      <li class="NotRequired"><input id="txtContTitle" class="k-input k-textboxlong" /></li>
                                                      <li>800 Number</li>
                                                      <li class="NotRequired"><input id="txtCont800" class="k-input k-textboxlong" /></li>
                                                      <li>Phone Number</li>
                                                      <li class="required"><input id="txtContPhone" class="k-input k-textboxlong" /> *</li>
                                                      <li>Phone Ext</li>
                                                      <li class="NotRequired"><input id="txtContPhoneExt" class="k-input k-textboxlong" /></li>
                                                      <li>Fax</li>
                                                      <li class="NotRequired"><input id="txtContFax" class="k-input k-textboxlong" /></li>
                                                      <li>Email</li>
                                                      <li class="required"><input id="txtContEmail" class="k-input k-textboxlong" /> *</li>
                                                  </ul>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                                  <div id="compPending"></div>
                              </div>
                          </div>
                          <div id="divSignUpEditWrapper">
                              <textarea id="txtSignUpEditor"></textarea>
                              <input id="edPgDet" type="hidden" />
                          </div>
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
        var alertMSg = "";           
       

        <% Response.Write(NGLOAuth2); %>

        

        <% Response.Write(PageCustomJS); %>
        
        //*************  execActionClick  ****************
        function execActionClick(btn, proc){
            if(btn.id == "btnTMSContEdit"){ editPageDynamicContent(); }         
        }

        function editPageDynamicContent(p){
            $("#divSignUpPageWrapper").hide();
            $("#divSignUpEditWrapper").show();                
            $("#txtSignUpEditor").data("kendoEditor").refresh();
        }

        function displayPageUI(){
            $("#divSignUpPageWrapper").show();
            $("#divSignUpEditWrapper").hide();
        }

        function displayEditor(){
            $("#divSignUpPageWrapper").hide();
            $("#divSignUpEditWrapper").show();
        }

        var resGetSignUpEditableContent = function (data) {
            //set the value of the editor and the html page content
            $("#txtSignUpEditor").data("kendoEditor").value(data.Content);
            $("#divSignUpAdminCont").html(data.Content);         
            $('#edPgDet').val(data.PageDetControl);
        }

        function getSignUpEditableContent() {
            var e = new editorContent();
            e.PageControl = PageControl;
            e.USec = 0;
            e.EditorName = "txtSignUpEditor";
            e.Content = "";
            e.PageDetControl = $('#edPgDet').val();
            getEditorContentNoAuth(JSON.stringify(e), resGetSignUpEditableContent);
        }

        var resSaveSignUpContentEditor = function (data) {          
            //set the html page content
            var c = $("#txtSignUpEditor").data("kendoEditor").value();
            $("#divSignUpAdminCont").html(c);         
            $("#divSignUpEditWrapper").hide();
            $("#divSignUpPageWrapper").show();           
        }

        function saveSignUpContentEditor() {
            var h = new editorContent();
            h.PageControl = PageControl;
            h.USec = localStorage.NGLvar1452;
            h.EditorName = "txtSignUpEditor";
            h.Content = $("#txtSignUpEditor").data("kendoEditor").value();
            h.PageDetControl = $('#edPgDet').val();
            saveEditorContent(h, resSaveSignUpContentEditor);
        }

        function cancelSignUpContentEditor() {            
            getSignUpEditableContent();
            $("#divSignUpEditWrapper").hide();
            $("#divSignUpPageWrapper").show();
        }


        function validateRequiredFields(){
            alertMsg = ""; //clear value
            var ret = true;            
            var fields = "";
            var strSp = "";
            if (ngl.isNullOrWhitespace($("#txtLegalEntity").data("kendoMaskedTextBox").value())){ fields += (strSp + "Company Name"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtCompAbrev").data("kendoMaskedTextBox").value())){ fields += (strSp + "3 Letter Prefix"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtCompName").data("kendoMaskedTextBox").value())){ fields += (strSp + "Warehouse Name"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtCompAlphaCode").data("kendoMaskedTextBox").value())){ fields += (strSp + "Warehouse ID"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtAddress1").data("kendoMaskedTextBox").value())){ fields += (strSp + "Address 1"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtCity").data("kendoMaskedTextBox").value())){ fields += (strSp + "City"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtState").data("kendoMaskedTextBox").value())){ fields += (strSp + "State"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtZip").data("kendoMaskedTextBox").value())){ fields += (strSp + "Postal Code"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtCountry").data("kendoMaskedTextBox").value())){ fields += (strSp + "Country"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtContName").data("kendoMaskedTextBox").value())){ fields += (strSp + "Contact Name"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtContPhone").data("kendoMaskedTextBox").value())){ fields += (strSp + "Phone Number"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtContEmail").data("kendoMaskedTextBox").value())){ fields += (strSp + "Email"); strSp = ", "; }
            if (!ngl.isNullOrWhitespace(fields)){ alertMsg = fields; ret = false; }
            return ret;
        }

        var resSignUpClick = function (data) {
            var emailTo = '<%=ConfigurationManager.AppSettings["NGLSignUpEmailTo"]%>';
            
            var s = "<div class='ngl-blueBorder'><div style='padding: 10px;'>"
                + "<div style='text-align: center;'><h1>Request Pending</h1></div>"
                + "<div style='text-align:center;'><p>The following information has been sent to Next Generation Logistics, Inc. with the request for access to the full version of the software.<br>If you would like to change any of the information please send an email to <a href='mailto: " + emailTo + "'>" + emailTo + "</a></p></div>"
                + "<div style='display:inline-block; float:left;'><ul><li><h2>Company Info</h2></li>"
                + "<li><strong>Company Name:</strong> " + data.CompLegalEntity + "</li>"
                + "<li><strong>3 Letter Prefix:</strong> " + data.CompAbrev + "</li>"
                + "<li><strong>Warehouse Name:</strong> " + data.CompName + "</li>"
                + "<li><strong>Warehouse ID:</strong> " + data.CompAlphaCode + "</li>"
                + "</ul></div>"
                + "<div style='display:inline-block; float:left;'><ul><li><h2>Ship From Address</h2></li><li><strong>Address 1:</strong> " + data.ShipFromAddress1 + "</li>"
                + "<li><strong>Address 2:</strong> " + data.ShipFromAddress2 + "</li>"
                + "<li><strong>Address 3:</strong> " + data.ShipFromAddress3 + "</li>"
                + "<li><strong>City:</strong> " + data.ShipFromCity + "</li>"
                + "<li><strong>State:</strong> " + data.ShipFromState + "</li>"
                + "<li><strong>Postal Code:</strong> " + data.ShipFromZip + "</li>"
                + "<li><strong>Country:</strong> " + data.ShipFromCountry + "</li>"
                + "</ul></div>"
                + "<div style='display:inline-block; float:left;'><ul><li><h2>Company Contact</h2></li><li><strong>Contact Name:</strong> " + data.CompContName + "</li>"
                + "<li><strong>Title:</strong> " + data.CompContTitle + "</li>"
                + "<li><strong>800 Number:</strong> " + data.CompCont800 + "</li>"
                + "<li><strong>Phone Number:</strong> " + data.CompContPhone + "</li>"
                + "<li><strong>Phone Ext:</strong> " + data.CompContPhoneExt + "</li>"
                + "<li><strong>Fax:</strong> " + data.CompContFax + "</li>"
                + "<li><strong>Email:</strong> " + data.CompContEmail + "</li>"
                + "</ul></div>"             
                + "</div></div>";
                    
            $("#compPending").html(s);
            $("#compPending").show();
            $("#enterComp").hide();
        }

        function signUp_Click(resultFunc){
            if (!validateRequiredFields()) { ngl.showErrMsg("Required Fields", alertMsg, null); return; }
            var comp = new freeTrialComp();                      
            comp.CompControl = 0;
            comp.CompLegalEntity = $("#txtLegalEntity").data("kendoMaskedTextBox").value();
            comp.CompName = $("#txtCompName").data("kendoMaskedTextBox").value();
            comp.CompNumber = 0;
            comp.ShipFromAddress1 = $("#txtAddress1").data("kendoMaskedTextBox").value();
            comp.ShipFromAddress2 = $("#txtAddress2").data("kendoMaskedTextBox").value();
            comp.ShipFromAddress3 = $("#txtAddress3").data("kendoMaskedTextBox").value();
            comp.ShipFromCity = $("#txtCity").data("kendoMaskedTextBox").value();
            comp.ShipFromState = $("#txtState").data("kendoMaskedTextBox").value();
            comp.ShipFromZip = $("#txtZip").data("kendoMaskedTextBox").value();
            comp.ShipFromCountry = $("#txtCountry").data("kendoMaskedTextBox").value();
            comp.CompAbrev = $("#txtCompAbrev").data("kendoMaskedTextBox").value();
            comp.CompAlphaCode = $("#txtCompAlphaCode").data("kendoMaskedTextBox").value();
            comp.CompContName = $("#txtContName").data("kendoMaskedTextBox").value(); 
            comp.CompContTitle = $("#txtContTitle").data("kendoMaskedTextBox").value();
            comp.CompCont800 = $("#txtCont800").data("kendoMaskedTextBox").value();
            comp.CompContPhone = $("#txtContPhone").data("kendoMaskedTextBox").value();
            comp.CompContPhoneExt = $("#txtContPhoneExt").data("kendoMaskedTextBox").value();
            comp.CompContFax = $("#txtContFax").data("kendoMaskedTextBox").value();
            comp.CompContEmail = $("#txtContEmail").data("kendoMaskedTextBox").value();
            comp.ValidationMsg = "";
            $.ajax({
                async: false,
                type: "POST",
                url: "api/SubscriptionRequest/CreateSubscriptionRequest",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(comp),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Sign Up - CreateSubscriptionRequest Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = true;
                                        if (ngl.isFunction(resultFunc)) { resultFunc(data.Data[0]); }
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Sign Up - CreateSubscriptionRequest could not be completed."; }
                            ngl.showErrMsg("Sign Up - CreateSubscriptionRequest Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Sign Up - CreateSubscriptionRequest", sMsg, null); }
            });
        }

        function getPendingComp(resultFunc){         
            $.ajax({
                async: false,
                type: "GET",
                url: "api/SubscriptionRequest/GetSubscriptionRequestByUser/" + localStorage.NGLvar1452,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) {
                                if (data.StatusCode === 200) {
                                    //no comp found
                                    blnSuccess = true;
                                    $("#txtContName").data("kendoMaskedTextBox").value(localStorage.NGLvar1457);
                                    $("#txtContEmail").data("kendoMaskedTextBox").value(localStorage.NGLvar1458);
                                    $("#compPending").hide();
                                    $("#enterComp").show();
                                } else{ blnErrorShown = true; ngl.showErrMsg("Get Sign Up Company Info Failure", data.Errors, null); }
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = true;
                                        if (ngl.isFunction(resultFunc)) { resultFunc(data.Data[0]); }
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Sign Up Company Info could not be completed."; }
                            ngl.showErrMsg("Get Sign Up Company Info Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Sign Up Company Info Failure"); ngl.showErrMsg("Get Sign Up Company Info", sMsg, null); }
            });
        }
       
        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
           

            $("#centerHTML").hide();
            $("#divSignUpEditWrapper").hide(); 
            ////$("#divSignUpPageWrapper").hide();

            if (control !=0){ 
                setTimeout(function () {              
                    $("#centerHTML").show();

                    $("#txtLegalEntity").kendoMaskedTextBox();
                    $("#txtCompName").kendoMaskedTextBox();
                    $("#txtAddress1").kendoMaskedTextBox();
                    $("#txtAddress2").kendoMaskedTextBox();
                    $("#txtAddress3").kendoMaskedTextBox();
                    $("#txtCity").kendoMaskedTextBox();
                    $("#txtState").kendoMaskedTextBox();
                    $("#txtZip").kendoMaskedTextBox();
                    $("#txtCountry").kendoMaskedTextBox();
                    $("#txtContName").kendoMaskedTextBox();
                    $("#txtContTitle").kendoMaskedTextBox();
                    //phone_number mask accepts any
                    //$("#txtCont800").kendoMaskedTextBox({ mask: "(000) 000-0000" });
                    //$("#txtContPhone").kendoMaskedTextBox({ mask: "(000) 000-0000" });
                    $("#txtCont800").kendoMaskedTextBox();
                    $("#txtContPhone").kendoMaskedTextBox();
                    $("#txtContPhoneExt").kendoMaskedTextBox();
                    $("#txtContFax").kendoMaskedTextBox();
                    $("#txtContEmail").kendoMaskedTextBox();
                    $("#txtCompAbrev").kendoMaskedTextBox({ mask: "&&&" });
                    $("#txtCompAlphaCode").kendoMaskedTextBox();
                    $("#btn").kendoButton({ click: function(e){ signUp_Click(resSignUpClick); } });

                    getPendingComp(resSignUpClick);

                    displayPageUI();
                    $('#edPgDet').val(0);
                    getSignUpEditableContent();          
                }, 10,this);                      
            }    
            setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>; }, 10,this);
            setTimeout(function () {        
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') { divWait.hide(); }
            }, 10, this);                 
        });
    </script>
          <style>
              ul li{ list-style-type: none; }
              ul li.required{ list-style-type: none; color: red; margin-bottom:5px; }
              ul li.NotRequired{ list-style-type: none; margin-bottom:5px; }
              ul li.txtLabel{ list-style-type: none; font-weight:bold; }             
              .k-textboxlong{ width: 200px !important; min-width: 20px !important; }
          </style>   
      </div>
    </body>
</html>