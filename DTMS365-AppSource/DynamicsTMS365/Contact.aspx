<%@ Page Title="Contact Page" Language="C#"  AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="DynamicsTMS365.Contact" %>

<!DOCTYPE html>

<html>
    <head>
        <title>DTMS Contact</title>
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

                            <div id="divContent" class="ngl-blueBorder"; style="display:none;">
                                <div style="padding: 10px;">
                                    
                                    <%-- This is the left panel where the editable CM content appears --%>
                                    <div id="nglCont" style="position:relative; float:left; display:inline-block; width:300px;">                     
                                        <div id="editableCont"></div>                                                                      
                                    </div>

                                    <%-- This is the right panel with the hardcoded Send Email form --%>
                                    <div id="emailForm" style="position:relative; overflow:hidden; display:inline-block; width:calc(100% - 300px);" >
                                        <ul id="fieldlist">
                                            <li>
                                                <label for="ddlEmail" class="required">To</label>
                                                <input id="ddlEmail"/>
                                            </li>
                                            <li>
                                                <label for="txtSubject" class="required">Subject</label>
                                                <input type="text" id="txtSubject" style="width:100%"/>                                        
                                            </li>
                                            <li>
                                                <label for="txtBody" class="required">Body</label>
                                                <textarea id="txtBody" class="txtBox-midLeftExpand" maxlength="500" rows="10" style="resize:vertical; height:auto;"></textarea>                                           
                                            </li>
                                            <li>
                                                <button id="btnSendEmail" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" onclick="sendContactEmail();" >Send</button>                                                                                            
                                            </li>                                        
                                        </ul>                                   
                                    </div>
                                </div>
                            </div>        
                              
                            <%-- This is the hidden editor that the super user can use to create the editable content. Only visible when button clicked --%>                       
                            <div id="divEditor">
                                <textarea id="edContact" style="height: 90%; width: 90%;"></textarea>
                            </div>
                            <input id="edPgDet" type="hidden" />
                                                            
                        </div>
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>


    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>             
    <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';var tObj = this;
        var tPage = this;     

        <% Response.Write(NGLOAuth2); %>

        var tObjPG = this;

        //*************  execActionClick  ****************
        function execActionClick(btn, proc){
            if (btn.id == "btnTMSContEdit"){ editPageDynamicContent(proc); }            
        }
               
        var resGetContactEditableContent = function (data) {
            //set the value of the editor and the html page content
            $("#edContact").data("kendoEditor").value(data.Content);
            $("#editableCont").html(data.Content);           
            $('#edPgDet').val(data.PageDetControl);
        }

        function getContactEditableContent() {
            var e = new editorContent();
            e.PageControl = PageControl;
            e.USec = 0;
            e.EditorName = "edContact";
            e.Content = "";
            e.PageDetControl = $('#edPgDet').val();
            getEditorContentNoAuth(JSON.stringify(e), resGetContactEditableContent);
        }


        var resSaveContactContentEditor = function (data) {          
            //set the html page content
            var c = $("#edContact").data("kendoEditor").value();
            $("#editableCont").html(c);            
            $("#divEditor").hide();
            $("#divContent").show();           
        }

        function saveContactContentEditor() {
            var h = new editorContent();
            h.PageControl = PageControl;
            h.USec = localStorage.NGLvar1452;
            h.EditorName = "edContact";
            h.Content = $("#edContact").data("kendoEditor").value();
            h.PageDetControl = $('#edPgDet').val();
            saveEditorContent(h, resSaveContactContentEditor);
        }

        function cancelContactContentEditor(){
            getContactEditableContent();
            $("#divEditor").hide();
            $("#divContent").show();
        }


        function SendSMTPEmailAjaxErrorCallback(xhr, textStatus, error, cbSource, errName) {
            var msg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Failed");
            ngl.showErrMsg("Send SMTP Email Failure", msg, null);
        }
        function SendSMTPEmailSuccessCallback(data) {
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Send SMTP Email Failure", data.Errors, null); }
                    else { $("#txtSubject").data("kendoMaskedTextBox").value(""); $("#txtBody").data("kendoEditor").value(""); ngl.showSuccessMsg("Success!", null); }
                }                
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        }

        
        function sendSMTPEmail(emailTo, emailCc, emailBcc, subject, body) {
            var smtpFrom = '<%=ConfigurationManager.AppSettings["SmtpFromAddress"]%>';
            var fields = "";
            var strSp = "";
            if (ngl.isNullOrWhitespace(emailTo)) { fields += (strSp + "To"); strSp = ", "; }
            if (ngl.isNullOrWhitespace(smtpFrom)) { fields += (strSp + "WebConfig:SmtpFromAddress"); strSp = ", "; }
            if (ngl.isNullOrWhitespace(subject)) { fields += (strSp + "Subject"); strSp = ", "; }
            if (ngl.isNullOrWhitespace(body)) { fields += (strSp + "Body"); strSp = ", "; }
            if (!ngl.isNullOrWhitespace(fields)) { ngl.showErrMsg("Required Fields", fields, null); return; }           
            var em = new EmailObject();
            em.emailTo = emailTo;
            em.emailFrom = smtpFrom;
            em.emailCc = emailCc;
            em.emailBcc = emailBcc;
            em.emailSubject = subject;
            em.emailBody = body;       
            var tObj = tObjPG;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("Email/GenerateSMTPEmail", em, tObj, "SendSMTPEmailSuccessCallback", "SendSMTPEmailAjaxErrorCallback");  
        }

        function sendContactEmail(){
            var emailTo = $("#ddlEmail").data("kendoDropDownList").value();
            var subject = $("#txtSubject").data("kendoMaskedTextBox").value();
            var body = $("#txtBody").data("kendoEditor").value();
            sendSMTPEmail(emailTo, "", "", subject, body);        
        }


        function editPageDynamicContent(p){
            $("#divContent").hide();
            $("#divEditor").show();    
            $("#edContact").data("kendoEditor").refresh();
        }


        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>; 
           
                                
            setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>; }, 10,this);
            setTimeout(function () {        
                menuTreeHighlightPage(); //must be called after PageReadyJS
                $("#divEditor").hide();                    
                $("#divContent").show();  
                $('#edPgDet').val(0);
                getContactEditableContent();

                $("#txtSubject").kendoMaskedTextBox();
                $("#btnSendEmail").kendoButton({ icon: "email" });

                var ts = '<%=ConfigurationManager.AppSettings["NGLTechSupportEmail"]%>';
                var sale = '<%=ConfigurationManager.AppSettings["NGLSalesEmail"]%>';
            
                var data = [
                    { text: "Logistics", value: sale },
                    { text: "Support", value: ts }              
                ];

                $("#ddlEmail").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: data,
                }); 

                $('#txtBody').kendoEditor({
                    resizable: { content: true, toolbar: true },
                    encoded: false,
                    tools: [ {name: 'email', tooltip: 'Send Email', exec: function(e){sendContactEmail();} }, 'bold','italic','underline','strikethrough','subscript','superscript','foreColor','justifyLeft','justifyCenter','justifyRight','justifyFull','insertUnorderedList','insertOrderedList','indent','outdent','formatting' ]
                });

                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }
            }, 10, this);
        });
    </script>        
            <style>           
                #fieldlist { margin: 0; padding: 0; }                            
                #fieldlist li { list-style: none; padding-bottom: .7em; text-align: left; }                      
                #fieldlist label { display: block; padding-bottom: .3em; text-transform: uppercase; font-weight: bold; font-size: 12px; }       
            </style>  
        </div>
    </body>
</html>