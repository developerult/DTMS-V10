<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NGLExpenses.aspx.cs" Inherits="DynamicsTMS365.NGLExpenses" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS NGL Expenses</title>
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
        <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
            <div id="vertical" style="height: 98%; width: 98%;">
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                    <div id="tab" class="menuBarTab"></div>
                </div>
                <div id="top-pane">
                    <div id="horizontal" style="height: 100%; width: 100%;">
                        <div id="left-pane">
                            <div class="pane-content">
                                <% Response.Write(MenuControl); %>
                                <div id="menuTree"></div>
                            </div>
                        </div>
                        <div id="center-pane">
                            <% Response.Write(PageErrorsOrWarnings); %>
                                    
                            <div style="float: left;">                                       
                                <table class="tblResponsive">                                            
                                    <tr><td class="tblResponsive-top">Carrier</td></tr>                                           
                                    <tr><td class="tblResponsive-top"><input id="ddlExCarrier" style="width: 250px;" /></td></tr>                                        
                                </table>                                    
                            </div>                                
                            <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>

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
                var tObj = this;
                var tPage = this; 
                var tObjPG = this;              
       

        <% Response.Write(NGLOAuth2); %>

                         
                var oNGLExpensesGrid = null;                   
                var ddlExCarrier;
                var pgExCarrierFltr = 0;
                <% Response.Write(PageCustomJS); %>

   
                //************* Action Menu Functions ********************
                function execActionClick(btn, proc){            
                    if(btn.id === "btnAddExpense"){                                  
                        if (typeof (tPage["wdgtAddExpenseWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtAddExpenseWndDialog"])){
                            tPage["wdgtAddExpenseWndDialog"].show();                
                        } else{alert("Missing HTML Element (wdgtAddExpenseWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                    }
                    else if(btn.id === "btnAddExpenseCarrier"){                                  
                        if (typeof (tPage["wdgtAddExpenseCarrierWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtAddExpenseCarrierWndDialog"])){
                            tPage["wdgtAddExpenseCarrierWndDialog"].show();                
                        } else{alert("Missing HTML Element (wdgtAddExpenseCarrierWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                    } 
                    else if (btn.id == "btnRefresh" || btn === "Refresh" ){ refresh(); }
                    else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }                      
                }

                function refresh(){ 
                    ngl.readDataSource(oNGLExpensesGrid);
                    ngl.readDataSource($('#ddlExCarrier').data('kendoDropDownList'));
                }
             
                //************* Call Back Functions **********************        
                function savePostPageSettingSuccessCallback(results){ } //for now do nothing when we save the pk
                function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){ }      //for now do nothing when we save the pk

                var blnFirstLoad = true;
                function readGetPageSettingSuccessCallback(data) {
                    var oResults = new nglEventParameters();
                    var tObj = this;
                    oResults.source = "readGetPageSettingSuccessCallback";
                    oResults.msg = 'Failed'; //set default to Failed         
                    oResults.CRUD = "read";
                    oResults.widget = tObj;
                    var dsUserPageSettings = null;                          
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";                        
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                          
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null); }                          
                            else {                               
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = true;
                                        dsUserPageSettings = data.Data[0]; 
                                        oResults.msg = "Success";
                                        if(typeof (dsUserPageSettings) !== 'undefined' && dsUserPageSettings != null && dsUserPageSettings.value != undefined){
                                            var d = JSON.parse(dsUserPageSettings.value);           
                                            var psData = JSON.parse(d.Data);
                                            if(psData.CarrierDDLValue !== 0){
                                                pgExCarrierFltr = psData.CarrierDDLValue;                                                  
                                                ddlExCarrier.select(function(dataItem) { return dataItem.Control === psData.CarrierDDLValue; }); //set the ddl                                                                                                          
                                            }                                                                                                                                      
                                        }                                   
                                    }                              
                                }                            
                            }                        
                        }                      
                        if (blnSuccess === false && blnErrorShown === false) {  
                            if (strValidationMsg.length < 1) { strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to receive this message."; }
                            ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null);                                    
                            ddlExCarrier.select(0);
                            var dataItemExCarrier = ddlExCarrier.dataItem();
                            pgExCarrierFltr = dataItemExCarrier.Control;
                        }                   
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }             
                }
                function readGetPageSettingAjaxErrorCallback(xhr, textStatus, error) {
                    var oResults = new nglEventParameters();
                    var tObj = this;
                    oResults.source = "readGetPageSettingAjaxErrorCallback";
                    oResults.msg = 'Failed'; //set default to Failed        
                    oResults.CRUD = "read"
                    oResults.widget = tObj;
                    oResults.error = new Error();
                    oResults.error.name = "Read Page Settings Failure"
                    oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); 
                }

                
                function NGLExpensesGridGetStringData(s)
                {           
                    blnFirstLoad = false;
                    var f = new AuditFilters(); 
                    f.CarrierDDLValue = pgExCarrierFltr;      
                    s.Data = JSON.stringify(f);
                    return JSON.stringify(f);
                }

                function NGLExpensesGridDataBoundCallBack(e,tGrid){
                    oNGLExpensesGrid = tGrid;
                }  
                

                function AddExpenseWndCB(oResults){          
                    if(oResults.widget.sNGLCtrlName === "wdgtAddExpenseWndDialog" && oResults.source === "saveSuccessCallback"){
                        execActionClick("Refresh");
                        oResults.widget.executeActions("close");
                    }          
                }

                function AddExpenseCarrierWndCB(oResults){          
                    if(oResults.widget.sNGLCtrlName === "wdgtAddExpenseCarrierWndDialog" && oResults.source === "saveSuccessCallback"){
                        execActionClick("Refresh");
                        oResults.widget.executeActions("close");
                    }          
                }



                //************* Page Functions ***************************
                function getDDLVars() {
                    ddlExCarrier = $("#ddlExCarrier").data("kendoDropDownList");
                }



                $(function () {
                    //wire focus of all numerictextbox widgets on the page
                    $("input[type=number]").bind("focus", function () {
                        var input = $(this);
                        clearTimeout(input.data("selectTimeId")); //stop started time out if any
                        var selectTimeId = setTimeout(function(){ input.select(); });
                        input.data("selectTimeId", selectTimeId);
                    }).blur(function(e) {
                        clearTimeout($(this).data("selectTimeId")); //stop started timeout
                    });
                })

                $(document).ready(function () {
                    var PageMenuTab = <%=PageMenuTab%>;
                                  
                    if (control != 0){        
                         
                        $("#ddlExCarrier").kendoDropDownList({
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            filter: "contains",
                            dataSource: {
                                serverFiltering: false,
                                transport: {
                                    read: {
                                        async: false,
                                        url: "api/vLookupList/GetUserDynamicList/" + nglUserDynamicLists.NGLExpenseCarrier,
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                                    }, 
                                },
                                schema: { 
                                    data: "Data",
                                    total: "Count",
                                    model: { 
                                        id: "Control",
                                        fields: {
                                            Control: { type: "number" },
                                            Name: { type: "string" }, 
                                            Description: { type: "string" }
                                        }
                                    }, 
                                    errors: "Errors"
                                },
                                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Expense Carriers JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                            },
                            select: function(e) {
                                var value = e.dataItem.Control;                       
                                if (ngl.isNullOrWhitespace(value)){ value = 0; } //Value must be int so if value is empty string or null set it to 0
                                pgExCarrierFltr = value;
                                if(blnFirstLoad === false){ refresh(); }                       
                            }
                        });

                        getDDLVars();
                        getPageSettings(tPage, "NGLExpenses", "expensesGridFilter", false);        
                    }
                    var PageReadyJS = <%=PageReadyJS%>;
                    menuTreeHighlightPage(); //must be called after PageReadyJS
                    var divWait = $("#h1Wait");
                    if (typeof(divWait) !== 'undefined') { divWait.hide(); }
                });
            </script>
            <style>
                .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
                .k-tooltip { max-height: 500px; max-width: 450px; overflow-y: auto; }
                .k-grid tbody .k-grid-Edit { min-width: 0; }                    
                .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }
            </style>
        </div>
    </body>
</html>
