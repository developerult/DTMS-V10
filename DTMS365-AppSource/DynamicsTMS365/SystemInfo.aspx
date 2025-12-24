<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemInfo.aspx.cs" Inherits="DynamicsTMS365.SystemInfo" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Package Descriptions</title>         
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
                            <!-- begin Page Content -->
                            <% Response.Write(FastTabsHTML); %>   

                            <div class="ngl-blueBorder">
                                <div style="padding: 10px;">
                                    <div style="display: inline-block; float: left;">
                                        <ul>
                                            <li>
                                                <h3>Client Software Version</h3>
                                            </li>
                                            <li class="ftBold">Current Release</li>
                                            <li><input id="txtCurrentClientSoftwareRelease" class="k-input k-textboxlong" /></li>
                                            <li class="ftBold">Last Modified</li>
                                            <li><input id="txtLastClientModified" class="k-input k-textboxlong" /></li>
                                            <li class="ftBold">Auth Number</li>
                                            <li><input id="txtAuthNumber" class="k-input k-textboxlong" /></li>                                            
                                        </ul>
                                    </div>
                                    <div style="display: inline-block; float: left;">
                                        <ul>
                                            <li>
                                                <h3>Server Software Version</h3>
                                            </li>
                                            <li class="ftBold">Current Release</li>
                                            <li><input id="txtCurrentServerSoftwareRelease" class="k-input k-textboxlong" /></li>
                                            <li class="ftBold">Last Modified</li>
                                            <li><input id="txtLastServerSoftwareModified" class="k-input k-textboxlong" /></li>
                                            <%--<li class="ftBold">Database</li>
                                            <li><input id="txtDatabase" class="k-input k-textboxlong" /></li>
                                            <li class="ftBold">Server</li>
                                            <li><input id="txtServer" class="k-input k-textboxlong" /></li>--%>
                                        </ul>
                                        <input type="hidden"  id="txtDatabase" />
                                        <input type="hidden"  id="txtServer"  />
                                    </div>
                                </div>
                            </div>

                            <!-- End Page Content -->

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
        var tPage = this;
        var tObjPG = this;
        var tObj = this;           
        

        <% Response.Write(NGLOAuth2); %>

        

        <% Response.Write(PageCustomJS); %>


        //************* Action Menu Functions ***************
        function execActionClick(btn, proc){            
            if (btn.id == "btnRefresh" ){ refresh(); }  
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
            else if(btn.id == "btnEmailSysInfo"){ emailSystemInfo(); }           
        }

        function refresh() { 
            getSystemInfo();        
        }


        //*************  Call Back Functions ****************       
        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingSuccessCallbackAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
        }

        
        function GetSystemInfoSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "GetSystemInfoSuccessCallback"; 
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "Get System Info Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                this.rData = data.Data;
                                oResults.data = data.Data;
                                oResults.msg = "Success";
                                blnSuccess = true;

                                $("#txtCurrentClientSoftwareRelease").data("kendoMaskedTextBox").value(data.Data[0].CurrentClientSoftwareRelease);
                                $("#txtLastClientModified").data("kendoMaskedTextBox").value(data.Data[0].LastClientModified);
                                $("#txtAuthNumber").data("kendoMaskedTextBox").value(data.Data[0].AuthNumber);
                                $("#txtCurrentServerSoftwareRelease").data("kendoMaskedTextBox").value(data.Data[0].CurrentServerSoftwareRelease);
                                $("#txtLastServerSoftwareModified").data("kendoMaskedTextBox").value(data.Data[0].LastServerSoftwareModified);
                                $("#txtDatabase").val(data.Data[0].Database);
                                $("#txtServer").val(data.Data[0].Server);
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Get System Info Failure"; }
                    oResults.error = new Error();
                    oResults.error.name = "Get System Info Failure";
                    oResults.error.message = strValidationMsg;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
            } catch (err) {
                oResults.error = err
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }      
        } 
        function GetSystemInfoAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "GetSystemInfoAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Get System Info Failure";
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);           
        }

        //************* Page Level Functions ****************
        function getSystemInfo(){
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.read("SystemInfo/GetSystemInfo", 0, tPage, "GetSystemInfoSuccessCallback", "GetSystemInfoAjaxErrorCallback",true);
        }

        function emailSystemInfo(){
            var item = new SystemInfo();
            item.CurrentClientSoftwareRelease = $("#txtCurrentClientSoftwareRelease").data("kendoMaskedTextBox").value();
            item.LastClientModified = $("#txtLastClientModified").data("kendoMaskedTextBox").value();
            item.AuthNumber = $("#txtAuthNumber").data("kendoMaskedTextBox").value();
            item.CurrentServerSoftwareRelease = $("#txtCurrentServerSoftwareRelease").data("kendoMaskedTextBox").value();
            item.LastServerSoftwareModified = $("#txtLastServerSoftwareModified").data("kendoMaskedTextBox").value();
            item.Database = $("#txtDatabase").val();
            item.Server = $("#txtServer").val();
            $.ajax({
                type: "POST",
                url: 'api/SystemInfo/EmailSystemInfo',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(item),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Email System Info Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') { blnSuccess = true; ngl.showSuccessMsg("Email sent successfully", null) }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Email System Info Failure"; }
                            ngl.showErrMsg("Email System Info Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }                         
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Email System Info Failure"); ngl.showErrMsg("Email System Info Failure", sMsg, null); }
            });
        }
 

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            
            if (control != 0){

                $("#txtCurrentClientSoftwareRelease").kendoMaskedTextBox();
                $("#txtLastClientModified").kendoMaskedTextBox();
                $("#txtAuthNumber").kendoMaskedTextBox();
                $("#txtCurrentServerSoftwareRelease").kendoMaskedTextBox();
                $("#txtLastServerSoftwareModified").kendoMaskedTextBox();
                //$("#txtDatabase").kendoMaskedTextBox();
                //$("#txtServer").kendoMaskedTextBox();

                $("#txtCurrentClientSoftwareRelease").data("kendoMaskedTextBox").readonly();
                $("#txtLastClientModified").data("kendoMaskedTextBox").readonly();
                $("#txtAuthNumber").data("kendoMaskedTextBox").readonly();
                $("#txtCurrentServerSoftwareRelease").data("kendoMaskedTextBox").readonly();
                $("#txtLastServerSoftwareModified").data("kendoMaskedTextBox").readonly();
                //$("#txtDatabase").data("kendoMaskedTextBox").readonly();
                //$("#txtServer").data("kendoMaskedTextBox").readonly();

                getSystemInfo();
            }
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }
            
        });

    </script>
          <style>

              ul li{
                  list-style-type: none;
                  margin-bottom:5px;
              }

              .ftBold{
                  list-style-type: none;
                  font-weight:bold;
              }
              
              .k-textboxlong{
                  width: 200px !important;
                  min-width: 20px !important;
              }
              
              .k-grid tbody tr td {
                  overflow: hidden;
                  text-overflow: ellipsis;
                  white-space: nowrap;
              }
              
              .k-tooltip{
                  max-height: 500px;
                  max-width: 450px;
                  overflow-y: auto;
              }
              
              .k-grid tbody .k-grid-Edit { min-width: 0; }
              
              .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }

          </style>
      </div>
    </body>
</html>
