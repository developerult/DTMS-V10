<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDILogs.aspx.cs" Inherits="DynamicsTMS365.EDILogs" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS EDI Logs</title>         
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
                                <div><span>Menu</span></div>
                                <div id="menuTree"></div>                                                               
                            </div>
                        </div>
                        <div id="center-pane">                            
                            <% Response.Write(PageErrorsOrWarnings); %>
                                                       
                            <div class="pane-content">                                   
                                <div class="fast-tab">                                        
                                    <span id="ExpandEnterEDISpan" style="display: none;"><a onclick='expandEnterEDI();'><span style="font-size:small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>                                        
                                    <span id="CollapseEnterEDISpan" style="display: normal;"><a onclick='collapseEnterEDI();'><span style="font-size:small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                                       
                                    <span style="font-size:small;font-weight:bold">Enter EDI</span>&nbsp;&nbsp;<br />                                       
                                    <div id="divEnterEDI" style="padding: 10px; width:calc(100% - 20px); height:100%;">       
                                        <div class="k-block k-info-colored">Multiple EDI files are required to share the same Carrier and Document Type or else Process EDI will not work properly (Carrier EDI Settings lookup is based on the first EDI file only)</div>                                                                               
                                        <textarea id="txtEnterEDIString" rows="5" style="resize:vertical;height:auto;width:100%;"></textarea>                                        
                                    </div>                                    
                                </div>                               
                            </div>                           
                            <div class="pane-content">                                    
                                <div class="fast-tab">                                       
                                    <span id="ExpandFormatEDISpan" style="display: none;"><a onclick='expandFormatEDI();'><span style="font-size:small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>                                        
                                    <span id="CollapseFormatEDISpan" style="display: normal;"><a onclick='collapseFormatEDI();'><span style="font-size:small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                                        
                                    <span style="font-size:small;font-weight:bold">EDI Formatted</span>&nbsp;&nbsp;<br />                                       
                                    <div id="divFormatEDI" style="padding: 10px; width:calc(100% - 20px); height:100%;">                                                                                     
                                        <textarea id="txtFormatEDIString" rows="10" style="resize:vertical;height:auto;width:100%;"></textarea>                                       
                                    </div>                                    
                                </div>                              
                            </div>
                            <div class="pane-content">                                    
                                <div class="fast-tab">                                       
                                    <span id="ExpandLogSpan" style="display: none;"><a onclick='expandLog();'><span style="font-size:small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>                                        
                                    <span id="CollapseLogSpan" style="display: normal;"><a onclick='collapseLog();'><span style="font-size:small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                                        
                                    <span style="font-size:small;font-weight:bold">Logs</span>&nbsp;&nbsp;<br />                                       
                                    <div id="divLog" style="padding: 10px; width:calc(100% - 20px); height:100%;">                                                                                     
                                        <div id="LogGrid" style="min-height: 50px;"></div>                                       
                                    </div>                                    
                                </div>                              
                            </div>
                            <div class="pane-content">                                   
                                <div class="fast-tab">                                        
                                    <span id="Expand997Span" style="display: none;"><a onclick='expand997();'><span style="font-size:small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>                                        
                                    <span id="Collapse997Span" style="display: normal;"><a onclick='collapse997();'><span style="font-size:small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                                       
                                    <span style="font-size:small;font-weight:bold">Generated 997</span>&nbsp;&nbsp;<br />                                       
                                    <div id="div997" style="padding: 10px; width:calc(100% - 20px); height:100%;">                 
                                        <div class="k-block k-info-colored">If no 997 is generated make sure the EDI Settings are configured properly for the Carrier</div>                                                                      
                                        <textarea id="txt997" rows="5" style="resize:vertical;height:auto;width:100%;"></textarea>                                        
                                    </div>                                    
                                </div>                               
                            </div>
                                                       
                            <!-- begin Page Content -->
                            <% Response.Write(FastTabsHTML); %>                                                              
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
        var oEDI214Grid = null;
        var oEDI210InGrid = null;
        var oBookTrackGrid = null;
        var ediSHID = ""; 

        <% Response.Write(NGLOAuth2); %>

        

        <% Response.Write(PageCustomJS); %>

        //************* Action Menu Functions ***************
        function execActionClick(btn, proc){          
            if (btn.id == "btnProcessEDI" ){ processEDI(); } 
            else if(btn.id === "btnViewEDI214"){                                  
                if (typeof (tPage["wdgtViewEDI214WndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtViewEDI214WndDialog"])){
                    tPage["wdgtViewEDI214WndDialog"].show();                
                } else{alert("Missing HTML Element (wdgtViewEDI214WndDialog is undefined)");} //Add better error handling here if cm stuff is missing
            }
            else if(btn.id === "btnViewEDI210In"){                                  
                if (typeof (tPage["wdgtViewEDI210InWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtViewEDI210InWndDialog"])){
                    tPage["wdgtViewEDI210InWndDialog"].show();                
                } else{alert("Missing HTML Element (wdgtViewEDI210InWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
            }
            else if (btn.id == "btnRefresh" ){ refresh(); }  
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }          
        }

        function refresh() {
            $("#txtEnterEDIString").data("kendoMaskedTextBox").value(""); 
            $("#txtFormatEDIString").data("kendoMaskedTextBox").value(""); 
            $("#txt997").data("kendoMaskedTextBox").value("");
            $("#LogGrid").data('kendoNGLGrid').dataSource.data([]); //clear the old logs
            ediSHID = "";
            ngl.readDataSource(oEDI214Grid);
            ngl.readDataSource(oEDI210InGrid);
            ngl.readDataSource(oBookTrackGrid);
        }


        //*************  Call Back Functions ****************       
        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingSuccessCallbackAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
        }

        function EDI214GridDataBoundCallBack(e,tGrid){ oEDI214Grid = tGrid; }
        function EDI210InGridDataBoundCallBack(e,tGrid){ oEDI210InGrid = tGrid; }
        function BookTrackGridDataBoundCallBack(e,tGrid){ oBookTrackGrid = tGrid; }

        function BookTrackGridGetStringData(s){ s.Data = ediSHID; return ediSHID; }

        function ProcessEDISuccessCallback(data) {
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Process EDI Failure", data.Errors, tObj); }
                    else {
                        if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                //set log grid datasource to return value and set 997
                                var logs = data.Data[0].strArray;
                                var s997 = data.Data[0].strField;
                                $("#txt997").data("kendoMaskedTextBox").value(s997);
                                var grid = $("#LogGrid").data("kendoNGLGrid");
                                $.each(logs, function(index, value) {
                                    var strVal = value.trim();
                                    grid.dataSource.add({ msg: strVal });
                                });                                                                
                            }
                        }
                    }
                }
            } catch (err) { ngl.showErrMsg(err.name, err.message, null); }
            kendo.ui.progress($(document.body), false);
            ngl.readDataSource(oBookTrackGrid);
        }
        function ProcessEDIAjaxErrorCallback(xhr, textStatus, error) {
            var tObj = this;
            var oResults = new nglEventParameters();
            oResults.source = "ProcessEDIAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Process EDI Failure";
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            kendo.ui.progress($(document.body), false);
        }


        
        //************* Page Level Functions ****************
        function expandEnterEDI() { $("#divEnterEDI").show(); $("#ExpandEnterEDISpan").hide(); $("#CollapseEnterEDISpan").show(); }
        function collapseEnterEDI() { $("#divEnterEDI").hide(); $("#ExpandEnterEDISpan").show(); $("#CollapseEnterEDISpan").hide(); }
        function expandFormatEDI() { $("#divFormatEDI").show(); $("#ExpandFormatEDISpan").hide(); $("#CollapseFormatEDISpan").show(); }
        function collapseFormatEDI() { $("#divFormatEDI").hide(); $("#ExpandFormatEDISpan").show(); $("#CollapseFormatEDISpan").hide(); }
        function expandLog() { $("#divLog").show(); $("#ExpandLogSpan").hide(); $("#CollapseLogSpan").show(); }
        function collapseLog() { $("#divLog").hide(); $("#ExpandLogSpan").show(); $("#CollapseLogSpan").hide(); }
        function expand997() { $("#div997").show(); $("#Expand997Span").hide(); $("#Collapse997Span").show(); }
        function collapse997() { $("#div997").hide(); $("#Expand997Span").show(); $("#Collapse997Span").hide(); }


        function getFormattedEDI(strInput){
            var strRet = "";
            var strHeader = "";
            /*
            ~ST*214*
            01234567 start = i+4, end = i+7
            */
            var i = strInput.indexOf("~ST*"); //i is the index of the ~ character in ~ST*
            var ediType = strInput.substring(i+4, i+7); //end is up to but not including
            strHeader = "*************** " + ediType + " ***************" + '\n';
            var segs = strInput.split('~')
            $.each(segs, function(index, value) {
                var strVal = value.trim();
                if(ngl.strStartsWith(strVal, "ISA")){
                    strRet += (strHeader + strVal) + '\n';
                } else{
                    if(ngl.strStartsWith(strVal, "IEA")){ strRet += (strVal + '\n' + strHeader) + '\n'; }
                    else{ strRet += strVal + '\n'; }
                }              
            });
            getBookTracks(strInput, ediType);
            return strRet;
        }

        function processEDI() {
            var title = "Process EDI";
            var msg = "This process will import the EDI file into the database. Are you sure you want to continue?";            
            ngl.OkCancelConfirmation(title, msg, 400, 400, null, "ConfirmProcessEDI"); //perform confirmation
        }
        function ConfirmProcessEDI(iRet) {
            if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; } //Chose the Cancel action           
            //Chose the Ok action
            kendo.ui.progress($(document.body), true);
            $("#LogGrid").data('kendoNGLGrid').dataSource.data([]); //clear the old logs
            var item = new GenericResult();                     
            item.strField = $("#txtEnterEDIString").data("kendoMaskedTextBox").value();
            item.blnField = true;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("EDILog/ProcessEDI", item, tObjPG, "ProcessEDISuccessCallback", "ProcessEDIAjaxErrorCallback", true);
        }

        function getBookTracks(strInput, ediType){
            //if(ediType === "214"){
            //    var b = strInput.indexOf("~B10*"); //i is the index of the ~ character in ~B10*
            //    var l = strInput.indexOf("~L11*"); //i is the index of the ~ character in ~L11*
            //    var strB10 = strInput.substring(b+5, l); //end is up to but not including
            //    var segs = strB10.split('*');
            //    ediSHID = segs[1];
            //}
            //if(ediType === "210"){
            //    var b = strInput.indexOf("~B3*"); //i is the index of the ~ character in ~B10*
            //    var l = strInput.indexOf("~N9*"); //i is the index of the ~ character in ~L11*
            //    var strB3 = strInput.substring(b+4, l); //end is up to but not including
            //    var segs = strB3.split('*');
            //    ediSHID = segs[2];
            //}
            ediSHID = strInput;
            ngl.readDataSource(oBookTrackGrid);
        }
       

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
           
            if (control != 0){

                $("#txtEnterEDIString").kendoMaskedTextBox({
                    change: function() {
                        var strEDI = this.value();
                        var strEDIFormatted = getFormattedEDI(strEDI);
                        $("#txtFormatEDIString").data("kendoMaskedTextBox").value(strEDIFormatted); 
                    }
                });

                $("#txtFormatEDIString").kendoMaskedTextBox();
                $("#txtFormatEDIString").data("kendoMaskedTextBox").readonly();

                $("#txt997").kendoMaskedTextBox();
                $("#txt997").data("kendoMaskedTextBox").readonly();

                $("#LogGrid").kendoNGLGrid({
                    dataSource: [],
                    columns: [ {field: "msg", title: "Message", template: "<span title='${msg}'>${msg}</span>"} ]
                });

            }
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }          
        });
    </script>
          <style>              
              .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }             
              .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }             
              .k-grid tbody .k-grid-Edit { min-width: 0; }             
              .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }
          </style>
      </div>
    </body>
</html>
