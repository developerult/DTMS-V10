<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterUser.aspx.cs" Inherits="DynamicsTMS365.RegisterUser" %>

<!DOCTYPE html>

<html >
    <head >
        <title>DTMS Manage Requests</title>
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
                                                   
                          <div id='pageContent' class='pane-content'>
                              <div class="fast-tab" style="padding-top:10px;">
                                  <span id="ExpandPendingFTSpan" style="display: none;"><a onclick='expandPendingFT();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                  <span id="CollapsePendingFTSpan" style="display: normal;"><a onclick='collapsePendingFT();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                  <span style="font-size: small; font-weight: bold">Pending Free Trial Requests (NGL Legacy)</span>&nbsp;&nbsp;<br />
                                  <div id="PendingFTDiv" style="padding-bottom: 10px; padding-top:10px; padding-right:10px;">
                                      <div id='PendingFTGrid'></div>
                                  </div>
                              </div>
                          </div>

                          <!-- begin Page Content -->
                          <% Response.Write(FastTabsHTML); %>    
                          <!-- End Page Content -->

                          <%--<div id='pageContent' class='pane-content'>
                              <div class="fast-tab" style="padding-top:10px;">
                                  <span id="ExpandPendingFTSpan" style="display: none;"><a onclick='expandPendingFT();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                  <span id="CollapsePendingFTSpan" style="display: normal;"><a onclick='collapsePendingFT();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                  <span style="font-size: small; font-weight: bold">Pending Free Trial Requests (NGL Legacy)</span>&nbsp;&nbsp;<br />
                                  <div id="PendingFTDiv" style="padding-bottom: 10px; padding-top:10px; padding-right:10px;">

                                      <div id='PendingFTGrid'></div>
                                  </div>
                              </div>
                          </div>--%>

                      </div>
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>

          <div id="winExtendFT">
              <div>
                  <div style="float: left;">
                      <div style="float: left;">
                          <table class="tblResponsive">
                              <tr><td class="tblResponsive-top">Select a Free Trial User to Edit</td></tr>
                              <tr><td class="tblResponsive-top"><input id="ddlFTUsers" style="width: 250px;" /></td></tr>
                          </table>
                      </div>
                      <div style="float: left;">
                          <table class="tblResponsive">
                              <tr><td class="tblResponsive-top">Current Free Trial End Date</td></tr>
                              <tr><td class="tblResponsive-top"><input id="dtCurrentFTEnd" /></td></tr>
                          </table>
                      </div>
                      <div style="float: left;">
                          <table class="tblResponsive">
                              <tr><td class="tblResponsive-top">New Free Trial End Date</td></tr>
                              <tr><td class="tblResponsive-top"><input id="dtNewFTEnd" /></td></tr>
                          </table>
                      </div>
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
        var oKendoGrid = null;
        var pendingFT = kendo.data.DataSource;
        var winExtendFT = kendo.ui.Window;
        var tObj = this;
        var tPage = this;
        var tObjPG = this;           
       

        <% Response.Write(NGLOAuth2); %>

        

        <% Response.Write(PageCustomJS); %>
        
        //*************  execActionClick  ****************
        function execActionClick(btn, proc){
            if(btn.id == "btnExtendFT"){ btnExtendFT_Click(); }   
            else if (btn.id == "btnRefresh" || btn === "Refresh" ){ refresh(); }
        }

        function refreshSubReqPDGrid() { ngl.readDataSource(oKendoGrid); }

        function refresh() { ngl.readDataSource(oKendoGrid); }

        //*************  Call Back Functions **********************
        function SubReqPDGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;
        }

        //ApproveSubReq
        function ApproveSubReqCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "ApproveSubReqCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "ApproveSubscriptionRequest Failure";
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
                                //TODO FIGURE OUT A WAY TO PERSIST THE MAIN GRID STATE OF WHICH DETAILS WERE EXPANDED BEFORE SAVE
                                if (ngl.isFunction(refreshSubReqPDGrid)) { refreshSubReqPDGrid(); }
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "ApproveSubscriptionRequest Failure"; }
                    oResults.error = new Error();
                    oResults.error.name = "ApproveSubscriptionRequest Failure";
                    oResults.error.message = strValidationMsg;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }           
        }
        function ApproveSubReqAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "ApproveSubReqAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "ApproveSubscriptionRequest Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);          
            //TODO FIGURE OUT A WAY TO PERSIST THE MAIN GRID STATE OF WHICH DETAILS WERE EXPANDED BEFORE SAVE
            if (ngl.isFunction(refreshSubReqPDGrid)) { refreshSubReqPDGrid(); }
        }


        /////////FREE TRIAL/////////
        function expandPendingFT() { $("#PendingFTDiv").show(); $("#ExpandPendingFTSpan").hide(); $("#CollapsePendingFTSpan").show(); }
        function collapsePendingFT() { $("#PendingFTDiv").hide(); $("#ExpandPendingFTSpan").show(); $("#CollapsePendingFTSpan").hide(); }

        function btnExtendFT_Click(){
            $("#dtNewFTEnd").data("kendoDatePicker").value("");
            refreshFTUsersDropDown();
            winExtendFT.center().open();
        }

        function refreshPendingFTGrid() {
            $('#PendingFTGrid').data('kendoGrid').dataSource.read();
        }
        
        function refreshFTUsersDropDown() {
            $('#ddlFTUsers').data('kendoDropDownList').dataSource.read();
        }

        function acceptPendingUser(e){
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));         
            var u = new User();
            u.UserSecurityControl = dataItem.UserSecurityControl;
            u.UserEmail = dataItem.UserEmail;
            $.ajax({
                async: false,
                type: "POST",
                url: "api/Users/AcceptLegacyFTRequest",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(u),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Approve Free Trial Request Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        ngl.showSuccessMsg("Free Trial Account Request Approved", null);
                                        refreshPendingFTGrid();       
                                        refreshFTUsersDropDown();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "There was a problem while accepting the Free Trial account request."; }
                            ngl.showErrMsg("Approve Free Trial Request Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Post Data Failure"); ngl.showErrMsg("Approve Free Trial Request Failure", sMsg, null); }
            });
        }

        function SaveExtendFT() {
            var item = new User();
            var dataItem = $("#ddlFTUsers").data("kendoDropDownList").dataItem();          
            item.UserSecurityControl = dataItem.UserSecurityControl;
            item.UserEndFreeTrial = $("#dtNewFTEnd").data("kendoDatePicker").value();
            if (ngl.isNullOrWhitespace(item.UserEndFreeTrial)){ ngl.showErrMsg("Required Fields", "Free Trial End Date cannot be null", null); return; }              
            $.ajax({
                async: false,
                type: "POST",
                url: "api/Users/ExtendFreeTrial",
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
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Extend Free Trial Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        ngl.showSuccessMsg("Save Successful", null);
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Extend Free Trial Failure"; }
                            ngl.showErrMsg("Extend Free Trial Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Extend Free Trial Failure", sMsg, null); }
            });
            winExtendFT.close();           
        }
        

        /////////SIGN UP REQUESTS/////////
        function ApproveSubReq(e) {
            //Get the record from the Grid
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            //Validate the row object values 
            if (!('SUBPDControl' in item)) { ngl.showErrMsg("Unexpected Error - Contact IT", "Cannot get SUBPDControl from the grid. Check to make sure all field names match accross all objects.", null); return; }
            if (!('SUBPDCompControl' in item)) { ngl.showErrMsg("Unexpected Error - Contact IT", "Cannot get SUBPDCompControl from the grid. Check to make sure all field names match accross all objects.", null); return; }
            if (!('LEAdminControl' in item)) { ngl.showErrMsg("Unexpected Error - Contact IT", "Cannot get LEAdminControl from the grid. Check to make sure all field names match accross all objects.", null); return; }
            if (!('SUBPDUserSecurityControl' in item)) { ngl.showErrMsg("Unexpected Error - Contact IT", "Cannot get SUBPDUserSecurityControl from the grid. Check to make sure all field names match accross all objects.", null); return; }
            var strUser = "this user";
            if ('UserName' in item) { strUser = item.UserName; }
            var g = new GenericResult();
            g.Control = item.LEAdminControl;
            g.intField1 = item.SUBPDCompControl; //same as LEAdminCompControl in this case
            g.intField2 = item.SUBPDUserSecurityControl;
            g.intField3 = item.SUBPDControl;
            var tObj = tObjPG;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("SubscriptionRequest/ApproveSubscriptionRequest", g, tObj, "ApproveSubReqCallback", "ApproveSubReqAjaxErrorCallback");   
        }


        $(document).ready(function () {   
            var PageMenuTab = <%=PageMenuTab%>;
                                
            if (control != 0){

                $("#dtCurrentFTEnd").kendoDatePicker();
                $("#dtCurrentFTEnd").data("kendoDatePicker").readonly();         

                $("#dtNewFTEnd").kendoDatePicker();
           
                $("#ddlFTUsers").kendoDropDownList({
                    dataTextField: "UserFriendlyName",
                    dataValueField: "UserSecurityControl",
                    autoWidth: true,
                    filter: "startswith",
                    dataSource: {
                        serverFiltering: true,
                        serverPaging: true,
                        transport: {
                            read: {
                                url: "api/Users/GetFreeTrialUsers/0",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                type: "GET"
                            }
                        },
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "UserSecurityControl",
                                fields: {
                                    UserSecurityControl: { type: "number" },
                                    UserName: { type: "string" }, 
                                    UserFriendlyName: { type: "string" },
                                    UserFirstName: { type: "string" },
                                    UserLastName: { type: "string" },
                                    UserStartFreeTrial: { type: "date" },
                                    UserEndFreeTrial: { type: "date" },
                                    UserFreeTrialActive: { type: "bool" }
                                }
                            }, 
                            errors: "Errors"
                        },
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Free Trial Users JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                    },
                    change: function(e) {
                        var item = this.dataItem();                    
                        if (typeof (item) !== 'undefined' && item != null && ngl.isObject(item)) { 
                            if ('UserEndFreeTrial' in item) { 
                                $("#dtCurrentFTEnd").data("kendoDatePicker").value(item.UserEndFreeTrial);                        
                                $("#dtNewFTEnd").data("kendoDatePicker").value("");
                                $("#dtNewFTEnd").data("kendoDatePicker").min(item.UserEndFreeTrial);
                            }
                        }
                    },
                    dataBound: function(e) {
                        this.select(0);
                        this.trigger("change");
                    }
                });


                pendingFT = new kendo.data.DataSource({ 
                    serverSorting: true,
                    serverPaging: true,
                    pageSize: 10,
                    transport: { 
                        read: {
                            url: "api/Users/GetFreeTrialUsers/1",
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            type: "GET"
                        },                   
                        parameterMap: function (options, operation) { return options; } 
                    }, 
                    schema: { 
                        data: "Data",
                        total: "Count",
                        model: { 
                            id: "UserSecurityControl",
                            fields: {
                                UserSecurityControl: { type: "number" },
                                UserName: { type: "string" }, 
                                UserFriendlyName: { type: "string" },
                                UserFirstName: { type: "string" },
                                UserLastName: { type: "string" },
                                UserEmail: { type: "string" },
                                UserPhoneWork: { type: "string" },
                                UserPhoneWorkExt: { type: "string" },
                                UserStartFreeTrial : { type: "date" }
                            }
                        }, 
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Free Trial Users JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failuire"), null);  this.cancelChanges(); }                  
                });
            
                $('#PendingFTGrid').kendoGrid({ 
                    dataSource: pendingFT, 
                    sortable: true,
                    pageable: true, 
                    resizable: true, 
                    groupable: false, 
                    columns: [
                        {field: "UserSecurityControl", title: "Control", hidden: true},
                        {field: "UserFirstName", title: "First Name"},
                        {field: "UserLastName", title: "Last Name"},
                        {field: "UserEmail", title: "Email"},
                        {field: "UserPhoneWork", title: "Work Phone"},
                        {field: "UserPhoneWorkExt", title: "Work Phone Ext"},
                        {field: "UserStartFreeTrial", title: "Requested Date", template: "#= kendo.toString(kendo.parseDate(UserStartFreeTrial, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"},
                        { command: { text: "Approve", iconClass: "k-icon k-i-check", click: acceptPendingUser }, title: " " }
                    ]
                });
         

                winExtendFT = $("#winExtendFT").kendoWindow({
                    title: "Extend Free Trial",
                    width: 275,
                    maxWidth: 800,
                    modal: true,
                    visible: false,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                }).data("kendoWindow");
                //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                $("#winExtendFT").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveExtendFT(); }); 

            }                      
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS 
            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }
        });
    </script>  
      </div>
    </body>
</html>
