<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageSchedule.aspx.cs" Inherits="DynamicsTMS365.ManageSchedule" %>

<!DOCTYPE html>

<html>
<head>
    <title>Manage Schedule</title>
    <%=cssReference%>
    <style>
        html,
        body {height: 100%;margin: 0;padding: 0;}

        html {font-size: 12px;font-family: Arial, Helvetica, sans-serif;overflow: hidden;}     
          
    </style>
</head>
<body>
    <%=jssplitter2Scripts%> 
    <%=sWaitMessage%> 

    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 3/8/2018 For Editing KendoWindow Configuration from Javascript -->

    <div id="example" class="ui-container">
        <div id="vertical" class="ui-vertical-container">
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" class="ui-horizontal-container">
                    <div id="left-pane">
                        <div class="pane-content">
                            <% Response.Write(MenuControl); %>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <% Response.Write(PageErrorsOrWarnings); %>
                        <!-- Begin Page Content -->

                        <%--Message--%>
                        <div id="txtScreenMessage"></div>

                        <!-- Grid Fast Tab -->

                        <div class="divMenuWarehouse">
                            <label><b>Select Warehouse :</b></label>
                            <input id="ddlResourceWarehouse" style="width: 300px;" />
                        </div>
                        <%--scheduler--%>
                        <div id="Manageschedule">
                            <div id="tabstrip">
                                <ul>
                                    <li class="k-active"><b>Resources</b></li>
                                    <li><b>Overrides</b>
                                    </li>
                                    <li><b>User Fields</b>
                                    </li>
                                    <li><b>Appt Status Colors</b>
                                    </li>
                                    <li><b>Appt Order Colors</b>
                                    </li>
                                    <li><b>Appt Details Fields</b>
                                    </li>
                                    <li><b>Tracking Fields</b>
                                    </li>
                                    <%--<li><b>Misc</b>
                                    </li>--%>
                                </ul>
                                <div>
                                    <div class="weather">
                                        <div class="tabLable"><b>Resource Management</b></div>
                                        <div id="resourceGrid"></div>
                                        <div class="MTop">
                                            <fieldset>
                                                <legend><b>Resource Details</b></legend>
                                                <div id="ResourceEdit-UI">
                                                    <div style="width: 12%; max-width: 90px;">
                                                        <div><b>Resource ID</b></div>
                                                        <div>
                                                            <input id="txtResourceID" maxlength="50" style="width: 100%" /></div>
                                                    </div>
                                                    <div style="width: 13%">
                                                        <div><b>Resource Name</b></div>
                                                        <div>
                                                            <input id="txtResourceName" maxlength="50" style="width: 100%" /></div>
                                                    </div>
                                                    <div style="width: 15%; max-width: 130px;">
                                                        <div><b>Booking Sequence</b></div>
                                                        <div>
                                                            <input id="numBookingSeq" maxlength="50" style="width: 100%; max-width: 130px;" /></div>
                                                    </div>
                                                    <div style="width: 5%; max-width: 100px;">
                                                        <div><b>Inbound</b></div>
                                                        <div><input id="switchDockInbound" type="checkbox" data-role="switch" /></div>
                                                    </div>
                                                    <div style="width: 10%; max-width: 100px; margin-left: 25px;">
                                                        <div><b>Validation</b></div>
                                                        <div>
                                                            <input id="switchValiOnOff" type="checkbox" data-role="switch" /></div>
                                                    </div>
                                                    <div style="width: 10%; max-width: 110px;">
                                                        <div><b>Override Alert</b></div>
                                                        <div>
                                                            <input id="switchOverAlerts" type="checkbox" data-role="switch" /></div>
                                                    </div>
                                                    <div style="width: 10%; max-width: 120px;"> <%--Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement--%>
                                                        <div><b>Notification Alert</b></div>
                                                        <div>
                                                            <input id="switchNotiAlerts" type="checkbox" data-role="switch" /></div>
                                                    </div>
                                                    <div style="width: 20%; max-width: 230px;">
                                                        <div><b>Resource Notification Email</b></div>
                                                        <div>
                                                            <input id="txtNEA" maxlength="50" style="width: 100%;" /></div>
                                                    </div>
                                                    <button id="SaveDockDoorDetails" style="float: right"><span class='k-icon k-i-save'></span></button>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <div class="MTop">
                                            <div id="ApptSettings" style="float: left; width: 46%;">
                                                <fieldset>
                                                    <legend><b>Appointment Settings</b></legend>
                                                    <br />
                                                    <button id="SaveDockDoorTimeMaxMin" style="float: right"><span class='k-icon k-i-save'></span></button>
                                                    <label class="MBottom"><b>Start and Stop time by Day</b></label>
                                                    <div id="SaveDockDoorTimeMaxMin-UI">
                                                        <div>
                                                            <table class="tblResponsive MTop">
                                                                <thead>
                                                                    <tr>
                                                                        <td>MON</td>
                                                                        <td>TUE</td>
                                                                        <td>WED</td>
                                                                        <td>THU</td>
                                                                        <td>FRI</td>
                                                                        <td>SAT</td>
                                                                        <td>SUN</td>
                                                                    </tr>
                                                                </thead>
                                                                <tr>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeStartMON" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeStartTUE" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeStartWED" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeStartTHU" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeStartFRI" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeStartSAT" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeStartSUN" class="ui-td-margin" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 10px"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeEndMON" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeEndTUE" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeEndWED" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeEndTHU" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeEndFRI" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeEndSAT" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="timeEndSUN" class="ui-td-margin" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 10px"></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <br />
                                                        <label><b>Max Appt by Day</b></label>
                                                        <div>
                                                            <table class="tblResponsive">
                                                                <tr>
                                                                    <td style="height: 10px"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMaxApptMON" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMaxApptTUE" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMaxApptWED" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMaxApptTHU" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMaxApptFRI" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMaxApptSAT" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMaxApptSUN" class="ui-td-margin" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 10px"></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <br />
                                                        <label><b>Appointment Time (In Minutes)</b></label>
                                                        <div>
                                                            <table class="tblResponsive ApptTimeTable">
                                                                <tr>
                                                                    <td style="height: 10px"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>Min</td>
                                                                    <td>Avg</td>
                                                                    <td>Max</td>
                                                                    <td></td>
                                                                    <%-- <td></td>--%>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMinTime" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numAvgTime" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <input id="numMaxTime" class="ui-td-margin" />
                                                                    </td>
                                                                    <td class="tblResponsive-top">
                                                                        <%-- <input id="ddlSetupTime" class="ui-td-margin" />--%>
                                                                    </td>
                                                                    <%-- <td class="tblResponsive-top">
                                                                     <input id="ddlBreakdownTime" class="ui-td-margin" />
                                                                 </td>--%>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 30px"></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>

                                                </fieldset>
                                            </div>
                                            <div style="float: left; width: 54%;" id="OtherSettings">
                                                <fieldset>
                                                    <legend><b>Other Settings</b></legend>
                                                    <div>
                                                        <div>
                                                            <div style="float: left; width: 49%;">
                                                                <label><b>Temperature Setting/Commodity Code</b></label>
                                                                <div id="tempSetGrid" class="MTop MBottom"></div>
                                                            </div>
                                                            <div style="float: left; width: 49%; margin-left: 2%;">
                                                                <label><b>Package Type</b></label>
                                                                <div id="pkgTypGrid" class="MTop MBottom"></div>
                                                            </div>
                                                        </div>
                                                        <div class="MTop">
                                                            <label><b>Appointment Calculation Time Factor Maintenance</b></label>
                                                            <div id="ACTFMGrid" class="MTop"></div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <div class="tabLable">
                                        <label><b>Resource Override Setting for :</b></label>
                                        <input id="ddlROSDockDoor" />
                                    </div>
                                    <div id="ROSGrid"></div>
                                </div>
                                <div>
                                    <div class="tabLable">
                                        <label><b>Manage User Fields</b></label>
                                    </div>
                                    <div id="UserFieldsGrid"></div>
                                </div>
                                <div>
                                    <div class="tabLable">
                                        <label><b>Manage Appointment Status Colors</b></label>
                                    </div>
                                    <div id="ApptStatusColorGrid"></div>
                                </div>
                                <div>
                                    <div class="tabLable">
                                        <label><b>Manage Appointment Order Colors</b></label>
                                    </div>
                                    <div id="ApptOrderColorGrid"></div>
                                </div>
                                <div>
                                    <div class="tabLable MBottom">
                                        <label><b>Manage Appointment Details - Popup/Hover Fields</b></label>
                                        <button id="btnSaveApptDetailFieldForComp" style="float: right"><span class='k-icon k-i-save'></span>Save</button>
                                    </div>
                                    <div id="ApptDPHFGrid"></div>
                                </div>
                                <div>
                                    <div class="tabLable">
                                        <label><b>Manage Tracking Fields</b></label>
                                    </div>
                                    <div id="trakingFieldGrid"></div>
                                </div>
                                <%--<div>
                                    <div class="tabLable">
                                        <div class="tabLable"><b>Miscellaneous</b></div>
                                        <table class="tblResponsive">
                                            <tr>
                                                <td class="tblResponsive-top"><b>Time Zone :</b></td>
                                                <td class="tblResponsive-top">
                                                    <input id="ddlTimeZone" style="width: 250px" /></td>
                                                <td class="tblResponsive-top">
                                                    <button id="btnTimeZone">Save</button></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                        <!-- End Page Content -->
                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block ui-horizontal-container">
                <div class="pane-content">
                    <% Response.Write(PageFooterHTML); %> 
                </div>
            </div>
        </div>

        <%--LVV 2/12--%>
        <div id="wndMngLockOutWnd">
            <div style="padding: 0px 10px 0px 10px;">
                <div id="LockOutGrid"></div>
            </div>
        </div>

        <%--Added By SK for ManageScheduleWindow for KendoWindow--%>
        <% Response.WriteFile("~/Views/ManageScheduleWindow.html"); %>

        <% Response.Write(PageTemplates); %>

        <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
        <% Response.Write(AuthLoginNotificationHTML); %>  
        <% Response.WriteFile("~/Views/HelpWindow.html"); %> 
        <script> 
            <% Response.Write(ADALPropertiesjs); %>

            var PageControl = '<%=PageControl%>';  
            var tObj = this;
            var tPage = this;           
        

        <% Response.Write(NGLOAuth2); %>

       
            <% Response.Write(PageCustomJS); %>

            //************* Page Variables **************************  
            var dsWarehouse = kendo.data.DataSource;
            var dsResourcesData = kendo.data.DataSource;
            var dsTemperature = kendo.data.DataSource;
            var dsPackageGrid = kendo.data.DataSource;
            var dsActfm = kendo.data.DataSource;
            var dsResourceOverrides = kendo.data.DataSource;
            var dsUserFields = kendo.data.DataSource;
            var dsResourceOverride = kendo.data.DataSource;
            var dsApptStatusCol = kendo.data.DataSource;
            var dsApptOrderStatusCol = kendo.data.DataSource;
            var dsApptDetails = kendo.data.DataSource;
            var dsTrackingField = kendo.data.DataSource;
            var CompControl=0;
            var compDockControl=0;
            var dsPackageTypeField = kendo.data.DataSource;
            var bitpositionPTArray = [];
            var bitpositionTSArray = [];
            var bitpositionApptFieldsArray = [];
            var dsApptColorStatus;
            var dsOrderColorStatus;
            var dsTimeFactor =  [{control:1, value :1},{control:2, value :2},{control:3, value :3},{control:4, value :4}];
            var dsCompUserFieldMaps;
            //LVV CHANGE
            //Define page level variables
            var dom_btnChangeSOP = null;
            var dom_SOPExists = false;
            var dom_btnCopyDockSet = null;
            var dom_btnCopyDockSetExists = false;
            var dom_btnResetLockOut = null; //LVV 2/12
            var dom_LockOutExists = false; //LVV 2/12
            var oLockOutGrid = null; //LVV 2/12
            var wndMngLockOutWnd = kendo.ui.Window; //LVV 2/12

            //************* Action Menu Functions ********************
            function execActionClick(btn, proc){
                if(btn.id == "btnCopyResourceSettings"){ copyResourceSettings(); }
                else if(btn.id == "btnOverridePassword"){ changeOverridePassword(); }
                else if(btn.id === "btnResetFailedAttempts"){ 
                    refreshLockOutGrid();
                    wndMngLockOutWnd.center().open();
                    //if (typeof (tPage["wdgtMngLockOutWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtMngLockOutWndDialog"])){
                    //    tPage["wdgtMngLockOutWndDialog"].show();                
                    //} else{alert("Missing HTML Element (wdgtMngLockOutWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                } //LVV 2/12
                else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
            }

            function refreshLockOutGrid(){ ngl.readDataSource(oLockOutGrid); } //LVV 2/12

            function LockOutGridDataBoundCallBack(e,tGrid){           
                oLockOutGrid = tGrid;       
            } //LVV 2/12

            function copyResourceSettings() {
                $("#numWndNewResourceIDCopy-validation").addClass("hide-display");
                $("#txtWndNewResourceNameCopy-validation").addClass("hide-display");

                $("input.newExt:radio:first").prop("checked", true).trigger("click");

                var s = new  AllFilter();
                s.CompControlFrom  =CompControl;
                s.Filtervalue  =compDockControl;
                    
                $.ajax({ 
                    url:  'api/AMSCompDockdoor/GetCopyToExisingDocks', 
                    contentType: 'application/json; charset=utf-8', 
                    dataType: 'json', 
                    async:false,
                    data: { filter: JSON.stringify(s) }, 
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                    success: function(data) {
                        $("#ddlWndExtSelectResourceCopy").data("kendoDropDownList").setDataSource(data.Data);
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("Get CompDockDoor Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                            blnSuccess = true;
                                        }
                                        else{
                                            blnSuccess = true;
                                            strValidationMsg = "No records were found matching your search criteria";
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Get CompDockDoor Failure"; }
                                ngl.showErrMsg("Get CompDockDoor Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                               
                    }, 
                    error: function(result) { 
                        options.error(result);
                                
                    } 
                }); 
                   
                $("#numWndNewResourceIDCopy").val("");
                $("#txtWndNewResourceNameCopy").val("");
                $("#ddlWndExtSelectResourceCopy").data("kendoDropDownList").select(0);

                wndCopyResourceSettings.title("Add Resource & Copy Settings");
                wndCopyResourceSettings.center().open();
            }

            function changeOverridePassword() {
                $("#txtWndNewPassword-validation").addClass("hide-display");
                $("#txtWndNewConPassword-validation").addClass("hide-display");
                $("#txtWndNewPassword").val("");
                $("#txtWndNewConPassword").val("");
                WndSupOverridePassword.title("Overrides - Change Supervisor Override Password");
                WndSupOverridePassword.center().open();
            }


            //************Save Page Settings************//
            function InsertOrUpdateCurrentUserPageSetting(pSettingName) {
                var userSettings = {};
                userSettings.CompanyID = $("#ddlResourceWarehouse").val();


                //Modified by RHR for v-8.2 on 09/15/2018

                var userPageSettings = null;
                var UserPageSettingsData = new PageSettingModel();

                UserPageSettingsData.UserPSUserSecurityControl = '<%=UserControl%>';

                UserPageSettingsData.name = pSettingName;
                UserPageSettingsData.value = JSON.stringify(userSettings);


                $.ajax({
                    url: '/api/ManageSchedule/PostPageSetting/',
                    type: 'Post',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(UserPageSettingsData),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        //debugger;
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("Save User Page Setting Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            blnSuccess = true;
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Save User Page Setting Failure"; }
                                ngl.showErrMsg("Save User Page Setting Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                        ngl.showErrMsg("Save User Page Setting Failure", sMsg, null);
                    }
                });
            }

            //LVV 2/12
            function UnlockAccount(e) {
                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                $.ajax({
                    type: 'POST',
                    url: 'api/ManageScheduleLockout/ResetFailedAttempt',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(dataItem),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Reset Account Failure", data.Errors, null); }
                                else { refreshLockOutGrid(); }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Reset Account Failure", sMsg, null); }
                });
            }


            //******Default State function *******//
            function resourcesTabDefaultStates() {
                $("#resourceGrid").data("kendoGrid").dataSource.read();
                //LVV CHANGE
                if (dom_btnCopyDockSetExists === true) { dom_btnCopyDockSet.disabled = true; }

                $("#txtResourceID").val("");
                $("#txtResourceName").val("");
                $("#numBookingSeq").data("kendoNumericTextBox").value("");
                $("#switchValiOnOff").data("kendoSwitch").check(false);
                $("#switchOverAlerts").data("kendoSwitch").check(false);
                $("#switchNotiAlerts").data("kendoSwitch").check(false);
                $("#switchDockInbound").data("kendoSwitch").check(false); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                $("#txtNEA").val("");

                $("#timeStartMON").data("kendoTimePicker").value("");
                $("#timeStartTUE").data("kendoTimePicker").value("");
                $("#timeStartWED").data("kendoTimePicker").value("");
                $("#timeStartTHU").data("kendoTimePicker").value("");
                $("#timeStartFRI").data("kendoTimePicker").value("");
                $("#timeStartSAT").data("kendoTimePicker").value("");
                $("#timeStartSUN").data("kendoTimePicker").value("");

                $("#timeEndMON").data("kendoTimePicker").value("");
                $("#timeEndTUE").data("kendoTimePicker").value("");
                $("#timeEndWED").data("kendoTimePicker").value("");
                $("#timeEndTHU").data("kendoTimePicker").value("");
                $("#timeEndFRI").data("kendoTimePicker").value("");
                $("#timeEndSAT").data("kendoTimePicker").value("");
                $("#timeEndSUN").data("kendoTimePicker").value("");

                $("#numMaxApptMON").data("kendoNumericTextBox").value("");
                $("#numMaxApptTUE").data("kendoNumericTextBox").value("");
                $("#numMaxApptWED").data("kendoNumericTextBox").value("");
                $("#numMaxApptTHU").data("kendoNumericTextBox").value("");
                $("#numMaxApptFRI").data("kendoNumericTextBox").value("");
                $("#numMaxApptSAT").data("kendoNumericTextBox").value("");
                $("#numMaxApptSUN").data("kendoNumericTextBox").value("");

                $("#numMinTime").data("kendoNumericTextBox").value("");
                $("#numMaxTime").data("kendoNumericTextBox").value("");
                $("#numAvgTime").data("kendoNumericTextBox").value("");

                $("#SaveDockDoorDetails").data("kendoButton").enable(false);
                $("#SaveDockDoorTimeMaxMin").data("kendoButton").enable(false);
                $("#tempSetGrid .k-grid-edittemptype").addClass('k-state-disabled').removeClass("k-grid-edittemptype");
                $("#pkgTypGrid .k-grid-editpackagetype").addClass('k-state-disabled').removeClass("k-grid-editpackagetype");
                $("#ACTFMGrid .k-grid-addactfm").addClass('k-state-disabled').removeClass("k-grid-addactfm");

                $("#ACTFMGrid").data("kendoGrid").dataSource.data([]);
                $("#pkgTypGrid").data("kendoGrid").dataSource.data([]);
                $("#tempSetGrid").data("kendoGrid").dataSource.data([]);
                isSelectedResourceRow = false;
            }


            dsResourcesData = new kendo.data.DataSource({
                serverSorting: true,
                serverPaging: true,
                transport: {
                    read: function (options) {
                        var s = new AllFilter();
                        s.filterName = 'CompControl';
                        s.filterValue = CompControl

                        $.ajax({
                            url: '/api/AMSCompDockdoor/GetRecords/',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            async: false,
                            data: { filter: JSON.stringify(s) },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                options.success(data);
                                try {
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Get Resources Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                }
                                                else {
                                                    blnSuccess = true;
                                                    strValidationMsg = "No records were found matching your search criteria";
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get Resources Failure"; }
                                        ngl.showErrMsg("Get Resources Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }

                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    },
                    destroy: function (options) {
                        $.ajax({
                            url: 'api/AMSCompDockdoor/DeleteCompResource',
                            type: 'POST',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            data: JSON.stringify(options.data),
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                try {
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Delete Resource Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                    blnSuccess = true;
                                                    if (data.Data[0] == false) {
                                                        ngl.showWarningMsg("Delete Resource Failure!", "", null);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Delete Resource Failure"; }
                                        ngl.showErrMsg("Delete Resource Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                //refresh grid and DefaultState
                                resourcesTabDefaultStates();
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Resource Failure");
                                ngl.showErrMsg("Delete Resource Failure", sMsg, null);
                            }
                        });
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "CompDockControl",
                        fields: {
                            CompDockControl: { type: "number" },
                            CompDockDockDoorID: { type: "string" },
                            CompDockDockDoorName: { type: "string" },
                            CompDockDoorModDate: { type: "date", editable: false },
                            CompDockDoorModUser: { type: "string" }

                        }
                    },
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) {
                    ngl.showErrMsg("Access StatusColor Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });



            var editReSourceObject;
            function editResource(e) {
                editReSourceObject = this.dataItem($(e.currentTarget).closest("tr"));
                $("#txtResourceID").val(editReSourceObject.CompDockDockDoorID);
                $("#txtResourceName").val(editReSourceObject.CompDockDockDoorName);
                $("#numBookingSeq").data("kendoNumericTextBox").value(editReSourceObject.CompDockBookingSeq);
                $("#switchValiOnOff").data("kendoSwitch").check(editReSourceObject.CompDockValidation);
                $("#switchOverAlerts").data("kendoSwitch").check(editReSourceObject.CompDockOverrideAlert);
                $("#switchNotiAlerts").data("kendoSwitch").check(editReSourceObject.CompDockNotificationAlert);
                $("#switchDockInbound").data("kendoSwitch").check(editReSourceObject.CompDockInbound); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                $("#txtNEA").val(editReSourceObject.CompDockNotificationEmail);

                //ApptCompDockTimeSetting
                $("#SaveDockDoorDetails").data("kendoButton").enable(true);

                var s = new AllFilter();
                s.filterName = 'dockDoorControl';
                s.filterValue = editReSourceObject.CompDockControl;

                var AMSApptTimeSettings;
                $.ajax({
                    url: '/api/AMSCompDockdoor/GetDockApptTimeSettings/',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    async: false,
                    data: { filter: JSON.stringify(s) },
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        if (data.Data != null) {
                            AMSApptTimeSettings = data.Data[0];
                        }
                    }
                });
                if (AMSApptTimeSettings) {
                    $("#timeStartMON").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.MonStart), "HH:mm"));
                    $("#timeStartTUE").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.TueStart), "HH:mm"));
                    $("#timeStartWED").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.WedStart), "HH:mm"));
                    $("#timeStartTHU").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.ThuStart), "HH:mm"));
                    $("#timeStartFRI").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.FriStart), "HH:mm"));
                    $("#timeStartSAT").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.SatStart), "HH:mm"));
                    $("#timeStartSUN").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.SunStart), "HH:mm"));

                    $("#timeEndMON").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.MonEnd), "HH:mm"));
                    $("#timeEndTUE").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.TueEnd), "HH:mm"));
                    $("#timeEndWED").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.WedEnd), "HH:mm"));
                    $("#timeEndTHU").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.ThuEnd), "HH:mm"));
                    $("#timeEndFRI").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.FridEnd), "HH:mm"));
                    $("#timeEndSAT").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.SatEnd), "HH:mm"));
                    $("#timeEndSUN").data("kendoTimePicker").value(kendo.toString(new Date(AMSApptTimeSettings.SunEnd), "HH:mm"));

                    $("#numMaxApptMON").data("kendoNumericTextBox").value(AMSApptTimeSettings.MonMaxAppt);
                    $("#numMaxApptTUE").data("kendoNumericTextBox").value(AMSApptTimeSettings.TueMaxAppt);
                    $("#numMaxApptWED").data("kendoNumericTextBox").value(AMSApptTimeSettings.WedMaxAppt);
                    $("#numMaxApptTHU").data("kendoNumericTextBox").value(AMSApptTimeSettings.ThuMaxAppt);
                    $("#numMaxApptFRI").data("kendoNumericTextBox").value(AMSApptTimeSettings.FriMaxAppt);
                    $("#numMaxApptSAT").data("kendoNumericTextBox").value(AMSApptTimeSettings.SatMaxAppt);
                    $("#numMaxApptSUN").data("kendoNumericTextBox").value(AMSApptTimeSettings.SunMaxAppt);

                    $("#numMinTime").data("kendoNumericTextBox").value(AMSApptTimeSettings.ApptMinsMin);
                    $("#numMaxTime").data("kendoNumericTextBox").value(AMSApptTimeSettings.ApptMinsMax);
                    $("#numAvgTime").data("kendoNumericTextBox").value(AMSApptTimeSettings.ApptMinsAvg);
                } else {
                    $("#timeStartMON").data("kendoTimePicker").value("");
                    $("#timeStartTUE").data("kendoTimePicker").value("");
                    $("#timeStartWED").data("kendoTimePicker").value("");
                    $("#timeStartTHU").data("kendoTimePicker").value("");
                    $("#timeStartFRI").data("kendoTimePicker").value("");
                    $("#timeStartSAT").data("kendoTimePicker").value("");
                    $("#timeStartSUN").data("kendoTimePicker").value("");

                    $("#timeEndMON").data("kendoTimePicker").value("");
                    $("#timeEndTUE").data("kendoTimePicker").value("");
                    $("#timeEndWED").data("kendoTimePicker").value("");
                    $("#timeEndTHU").data("kendoTimePicker").value("");
                    $("#timeEndFRI").data("kendoTimePicker").value("");
                    $("#timeEndSAT").data("kendoTimePicker").value("");
                    $("#timeEndSUN").data("kendoTimePicker").value("");

                    $("#numMaxApptMON").data("kendoNumericTextBox").value("");
                    $("#numMaxApptTUE").data("kendoNumericTextBox").value("");
                    $("#numMaxApptWED").data("kendoNumericTextBox").value("");
                    $("#numMaxApptTHU").data("kendoNumericTextBox").value("");
                    $("#numMaxApptFRI").data("kendoNumericTextBox").value("");
                    $("#numMaxApptSAT").data("kendoNumericTextBox").value("");
                    $("#numMaxApptSUN").data("kendoNumericTextBox").value("");

                    $("#numMinTime").data("kendoNumericTextBox").value("");
                    $("#numMaxTime").data("kendoNumericTextBox").value("");
                    $("#numAvgTime").data("kendoNumericTextBox").value("");
                }

            }


            //LVV Change
            function SaveDockDoorDetails_Click() {
                var odata = editReSourceObject;
                odata.CompDockDockDoorName = $("#txtResourceName").val();
                odata.CompDockBookingSeq = $("#numBookingSeq").val();
                odata.CompDockValidation = $("#switchValiOnOff").data("kendoSwitch").check();
                odata.CompDockOverrideAlert = $("#switchOverAlerts").data("kendoSwitch").check();
                odata.CompDockNotificationAlert = $("#switchNotiAlerts").data("kendoSwitch").check();
                odata.CompDockInbound = $("#switchDockInbound").data("kendoSwitch").check(); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                odata.CompDockNotificationEmail = $("#txtNEA").val();
                $.ajax({
                    url: "api/AMSCompDockdoor/UpdateCompResourceDetails",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: JSON.stringify(odata),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";                           
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("Save Resource Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                            blnSuccess = true;
                                            //refresh grid and SelectRow  
                                            $("#resourceGrid").data("kendoGrid").dataSource.read();
                                            $("#resourceGrid").data("kendoGrid").dataSource.sync();
                                            var grid = $("#resourceGrid").data("kendoGrid");
                                            $.each(grid.tbody.find('tr'), function () {
                                                var model = grid.dataItem(this);
                                                if (model.CompDockDockDoorID == data.Data[0].CompDockDockDoorID) {
                                                    var row = $("#resourceGrid").find("tbody>tr[data-uid=" + model.uid + "]");
                                                    grid.select(row);
                                                    //$('[data-uid='+model.uid+']').addClass('k-state-selected');
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {                               
                                if (strValidationMsg.length < 1) { strValidationMsg = "Save Resource Failure"; }
                                ngl.showErrMsg("Save Resource Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                        ngl.showErrMsg("Save Resource Failure", sMsg, null);
                    }
                });

                $('#ResourceEdit-UI :input').each(function () {
                    $(this).data('initialValue', $(this).val());
                });
                swtval1 = $("#switchValiOnOff").data("kendoSwitch").check();
                swtval2 = $("#switchOverAlerts").data("kendoSwitch").check();
                swtval3 = $("#switchNotiAlerts").data("kendoSwitch").check();
                swtval4 = $("#switchDockInbound").data("kendoSwitch").check(); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
            }


            //LVV Change
            function SaveDockDoorTimeMaxMin_Click() {
                var tdata = editReSourceObject;
                tdata.DockControl = editReSourceObject.CompDockControl
                tdata.MonStart = $("#timeStartMON").val();
                tdata.TueStart = $("#timeStartTUE").val();
                tdata.WedStart = $("#timeStartWED").val();
                tdata.ThuStart = $("#timeStartTHU").val();
                tdata.FriStart = $("#timeStartFRI").val();
                tdata.SatStart = $("#timeStartSAT").val();
                tdata.SunStart = $("#timeStartSUN").val();
                tdata.MonEnd = $("#timeEndMON").val();
                tdata.TueEnd = $("#timeEndTUE").val();
                tdata.WedEnd = $("#timeEndWED").val();
                tdata.ThuEnd = $("#timeEndTHU").val();
                tdata.FridEnd = $("#timeEndFRI").val();
                tdata.SatEnd = $("#timeEndSAT").val();
                tdata.SunEnd = $("#timeEndSUN").val();
                tdata.MonMaxAppt = $("#numMaxApptMON").val();
                tdata.TueMaxAppt = $("#numMaxApptTUE").val();
                tdata.WedMaxAppt = $("#numMaxApptWED").val();
                tdata.ThuMaxAppt = $("#numMaxApptTHU").val();
                tdata.FriMaxAppt = $("#numMaxApptFRI").val();
                tdata.SatMaxAppt = $("#numMaxApptSAT").val();
                tdata.SunMaxAppt = $("#numMaxApptSUN").val();
                tdata.ApptMinsMin = $("#numMinTime").val();
                tdata.ApptMinsMax = $("#numMaxTime").val();
                tdata.ApptMinsAvg = $("#numAvgTime").val();

                $.ajax({
                    url: "api/AMSCompDockdoor/SaveDockApptTimeSettings",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: JSON.stringify(tdata),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("Save DockApptTimeSettings Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        blnSuccess = true;
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Save DockApptTimeSettings Failure"; }
                                ngl.showErrMsg("Save DockApptTimeSettings Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                        ngl.showErrMsg("Save DockApptTimeSettings Failure", sMsg, null);
                    }
                });

                $('#SaveDockDoorTimeMaxMin-UI :input').each(function () {
                    $(this).data('initialValue', $(this).val());
                });
            }


            dsApptBlockOurRecurrenceType = new kendo.data.DataSource({
                serverSorting: true,
                serverPaging: true,
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: '/api/vLookupList/GetStaticList/',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            async: false,
                            data: { id: nglStaticLists.ApptStatusColorCodeKey },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                options.success(data);
                                try {
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Get Appt Color Status Deatils Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                }
                                                else {
                                                    blnSuccess = true;
                                                    strValidationMsg = "No records were found matching your search criteria";
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get Appt Color Status Failure"; }
                                        ngl.showErrMsg("Get Appt Color Status Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }

                            },
                            error: function (result) {
                                ngl.showErrMsg("Get Appt Color Status Failure", result, null);
                            }
                        });
                    }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "Control",
                        fields: {
                            Control: { type: "number" },
                            Name: { type: "string" },
                        }
                    },
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) {
                    ngl.showErrMsg("Access Appt Color Status Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });


            function ApptBlockout(e) {
                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                $("#txtApptBlockOutTitle-validation").addClass("hide-display");
                $("#timeApptBlockOutFrom-validation").addClass("hide-display");
                $("#timeApptBlockOutTo-validation").addClass("hide-display");
                $("#dateApptBlockOutStart-validation").addClass("hide-display");
                $("#dateApptBlockOutEndOn-validation").addClass("hide-display");
                $("#numApptBlockOutEndAfter-validation").addClass("hide-display");

                $("#lblApptBlockOutID").html(dataItem.CompDockDockDoorID);
                $("#lblApptBlockOutName").html(dataItem.CompDockDockDoorName);

                AddNewBlockOutState();

                DockBlockCompDockContol = dataItem.CompDockControl;
                $("#ApptBlkOutPeriodGrid").data("kendoGrid").dataSource.read();

                wndApptBlockPeriods.title("Resource - Appointment Block Out Periods");
                wndApptBlockPeriods.center().open();
            };

            function AddNewBlockOutState() {
                $("#txtApptBlockOutTitle-validation").addClass("hide-display");
                $("#timeApptBlockOutFrom-validation").addClass("hide-display");
                $("#timeApptBlockOutTo-validation").addClass("hide-display");
                $("#dateApptBlockOutStart-validation").addClass("hide-display");
                $("#dateApptBlockOutEndOn-validation").addClass("hide-display");
                $("#numApptBlockOutEndAfter-validation").addClass("hide-display");

                $("#ddlApptBlockOutRecurrenceType").data("kendoDropDownList").select(0);
                $("#txtApptBlockOutTitle").val("");
                $("#txtApptBlockOutDesc").val("");
                $("#timeApptBlockOutFrom").val("");
                $("#timeApptBlockOutTo").val("");
                $("#dateApptBlockOutStart").val("");
                $("input.ApptBlk:radio:first").prop("checked", true).trigger("click");
                $("#dateApptBlockOutEndOn").val("");
                $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").value(0);

                $("#WeeklyMON").prop("checked", false);
                $("#WeeklyTUE").prop("checked", false);
                $("#WeeklyWED").prop("checked", false);
                $("#WeeklyTHU").prop("checked", false);
                $("#WeeklyFRI").prop("checked", false);
                $("#WeeklySAT").prop("checked", false);
                $("#WeeklySUN").prop("checked", false);
                $("#chkApptBlockOutOn").prop("checked", true);

                $("#btnWndCancleApptblkoutPeriod").hide();
                $("#btnWndAddApptblkoutPeriod").html("Add");
                ApptDockDoorBlockOutPeriod = {};
            }         



            function editapptblkoutperiods(e) {
                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                ApptDockDoorBlockOutPeriod = dataItem;

                $("#txtApptBlockOutTitle-validation").addClass("hide-display");
                $("#timeApptBlockOutFrom-validation").addClass("hide-display");
                $("#timeApptBlockOutTo-validation").addClass("hide-display");
                $("#dateApptBlockOutStart-validation").addClass("hide-display");
                $("#dateApptBlockOutEndOn-validation").addClass("hide-display");
                $("#numApptBlockOutEndAfter-validation").addClass("hide-display");

                $("#ddlApptBlockOutRecurrenceType").data("kendoDropDownList").value(ApptDockDoorBlockOutPeriod.RecurrenceType);
                $("#txtApptBlockOutTitle").val(ApptDockDoorBlockOutPeriod.Title);
                $("#txtApptBlockOutDesc").val(ApptDockDoorBlockOutPeriod.Description);
                $("#timeApptBlockOutFrom").data("kendoTimePicker").value(ApptDockDoorBlockOutPeriod.StartTime);
                $("#timeApptBlockOutTo").data("kendoTimePicker").value(ApptDockDoorBlockOutPeriod.EndTime);
                $("#dateApptBlockOutStart").data("kendoDatePicker").value(ApptDockDoorBlockOutPeriod.StartDate);

                if (ApptDockDoorBlockOutPeriod.Until != null) {
                    $("input.ApptBlk:radio:first").prop("checked", true).trigger("click");
                    $("#dateApptBlockOutEndOn").data("kendoDatePicker").value(ApptDockDoorBlockOutPeriod.Until);
                    $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").value("");
                } else if (ApptDockDoorBlockOutPeriod.Count > 0) {
                    $("input.ApptBlk:radio:eq(1)").prop("checked", true).trigger("click");
                    $("#dateApptBlockOutEndOn").data("kendoDatePicker").value("");
                    $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").value(ApptDockDoorBlockOutPeriod.Count);
                } else {
                    $("input.ApptBlk:radio:last").prop("checked", true).trigger("click");
                    $("#dateApptBlockOutEndOn").data("kendoDatePicker").value("");
                    $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").value("");
                }


                $("#WeeklyMON").prop("checked", ApptDockDoorBlockOutPeriod.blnMon);
                $("#WeeklyTUE").prop("checked", ApptDockDoorBlockOutPeriod.blnTue);
                $("#WeeklyWED").prop("checked", ApptDockDoorBlockOutPeriod.blnWed);
                $("#WeeklyTHU").prop("checked", ApptDockDoorBlockOutPeriod.blnThu);
                $("#WeeklyFRI").prop("checked", ApptDockDoorBlockOutPeriod.blnFri);
                $("#WeeklySAT").prop("checked", ApptDockDoorBlockOutPeriod.blnSat);
                $("#WeeklySUN").prop("checked", ApptDockDoorBlockOutPeriod.blnSun);
                $("#chkApptBlockOutOn").prop("checked", ApptDockDoorBlockOutPeriod.DockBlockOn);

                $("#btnWndAddApptblkoutPeriod").html("Update");
                $("#btnWndCancleApptblkoutPeriod").show();
            }



            //************Temperature Grid details**********//
            dsTemperature = new kendo.data.DataSource({
                serverSorting: true,
                serverPaging: true,
                transport: {
                    read: function (options) {
                        var s = new AllFilter();
                        s.filterName = 'dockDoorControl';
                        s.filterValue = compDockControl;
                        $.ajax({
                            url: '/api/AMSCompDockdoor/GetSupportedTempTypesForDock/',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            data: { filter: JSON.stringify(s) },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                options.success(data);
                                try {
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("GetSupportedTempTypesForDock Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                }
                                                else {
                                                    blnSuccess = true;
                                                    strValidationMsg = "No records were found matching your search criteria";
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "GetSupportedTempTypesForDock Failure"; }
                                        ngl.showErrMsg("GetSupportedTempTypesForDock Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }

                            },
                            error: function (result) {
                                options.error(result);

                            }
                        });
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "Control",
                        fields: {
                            Control: { type: "number" },
                            PTCaption: { type: "string" },
                            PTOn: { type: "boolean" },
                            PTBitPos: { type: "number" }
                        }
                    },
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) {
                    ngl.showErrMsg("AccessTemperature Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });

            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>;   

                if (control != 0){
                    setTimeout(function () {
                        //add code here to load screen specific information this is only visible when a user is authenticated  

                        //ALERTS
                        setTimeout(getAlerts, 60000);

                        //LVV CHANGE
                        //Populate the variables here so we only have to do it one time
                        dom_btnChangeSOP = document.getElementById('btnOverridePassword');
                        if (typeof (dom_btnChangeSOP) !== 'undefined' && ngl.isObject(dom_btnChangeSOP)) { dom_SOPExists = true; }

                        dom_btnCopyDockSet = document.getElementById('btnCopyResourceSettings');
                        if (typeof (dom_btnCopyDockSet) !== 'undefined' && ngl.isObject(dom_btnCopyDockSet)) { dom_btnCopyDockSetExists = true; }

                        dom_btnResetLockOut = document.getElementById('btnResetFailedAttempts'); //LVV 2/12
                        if (typeof (dom_btnResetLockOut) !== 'undefined' && ngl.isObject(dom_btnResetLockOut)) { dom_LockOutExists = true; } //LVV 2/12

                       
                        $("#tabstrip").kendoTabStrip({
                            height: 450,
                            activate: function(){
                                var index = $("#tabstrip").data("kendoTabStrip").select().index();
                                switch (index){
                                    case 0:
                                        //LVV CHANGE
                                        if (dom_btnCopyDockSetExists === true) { $("#btnCopyResourceSettings").show(); }
                                        if (dom_SOPExists === true) { $("#btnOverridePassword").hide(); }  
                                        if (dom_LockOutExists === true) { $("#btnResetFailedAttempts").hide(); } //LVV 2/12
                                        break;
                                    case 1:
                                        //LVV CHANGE
                                        if (dom_btnCopyDockSetExists === true) { $("#btnCopyResourceSettings").hide(); }
                                        if (dom_SOPExists === true) { $("#btnOverridePassword").show(); }
                                        if (dom_LockOutExists === true) { $("#btnResetFailedAttempts").show(); } //LVV 2/12
                                        break;
                                    default:
                                        //LVV CHANGE
                                        if (dom_btnCopyDockSetExists === true) { $("#btnCopyResourceSettings").hide(); }
                                        if (dom_SOPExists === true) { $("#btnOverridePassword").hide(); }
                                        if (dom_LockOutExists === true) { $("#btnResetFailedAttempts").hide(); } //LVV 2/12
                                }
                            },
                            animation:  {
                                open: {
                                    effects: "fadeIn"
                                }
                            }
                        });

                        //LVV CHANGE
                        if (dom_btnCopyDockSetExists === true) { $("#btnCopyResourceSettings").show(); } 
                        if (dom_SOPExists === true) { $("#btnOverridePassword").hide(); }
                        if (dom_LockOutExists === true) { $("#btnResetFailedAttempts").hide(); } //LVV 2/12
             

                        /////set default message/////
                        $("#txtScreenMessage").html('<h3>Manage Schedule</h3>');

                        //*********Kendo Widgets*********//
                        //*********Action tab***********//
                        $("#btnCpyResConfig").kendoButton();

                        //*********1st TAB*********//
                        //##### Add new Resource Popup ######//
                        $("#numWndNewResourceID").kendoMaskedTextBox();
                        $("#txtWndNewResourceName").kendoMaskedTextBox();
                        $("#numWndNewResourceBookSeq").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#redioWndNewResourceValidation").kendoSwitch();
                        $("#redioWndNewResourceOverride").kendoSwitch();
                        $("#redioWndNewResourceNotificationAlert").kendoSwitch();
                        $("#txtWndNewResourceNotificationEmail").kendoMaskedTextBox();
                        $("#chkWndNewResourceInbound").kendoSwitch(); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement

                        //#### Copy Resource Settings Popup ######//
                        $("#numWndNewResourceIDCopy").kendoMaskedTextBox();
                        $("#txtWndNewResourceNameCopy").kendoMaskedTextBox();
                        $("#ddlWndExtSelectResourceCopy").kendoDropDownList({
                            dataSource:null,
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            filter: "contains",
                        });

                        //if($("#rdoNEWResource").is(":checked")){
                        //    $("#NewResourceDiv").show();
                        //    $("#ExistingResourceDiv").hide();                   
                        //    $("#btnWndSaveResourceCopy").html("Add Resource and Copy Settings");
                        //}

                        $(".newExt").click(function () {
                            if($("#rdoNEWResource").is(":checked")){
                                $("#NewResourceDiv").show();
                                $("#ExistingResourceDiv").hide();
                   
                                $("#btnWndSaveResourceCopy").html("Add Resource and Copy Settings");
                            }
                            if($("#rdoEXTResource").is(":checked")){
                                $("#NewResourceDiv").hide();
                                $("#ExistingResourceDiv").show();

                                $("#btnWndSaveResourceCopy").html("Copy Resource Settings");
                            }
                        });

                        //###### Appt Block Out Period Popup ####//
                        $(".ApptBlk").click(function () {
                            if($("#rdoOn").is(":checked")){
                                $("#dateApptBlockOutEndOn").data("kendoDatePicker").enable(true);
                                $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").value("");
                                $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").enable(false);
                                $("#numApptBlockOutEndAfter-validation").addClass("hide-display");
                            }else if($("#rdoAfter").is(":checked")){
                                $("#dateApptBlockOutEndOn").data("kendoDatePicker").value("");
                                $("#dateApptBlockOutEndOn").data("kendoDatePicker").enable(false);
                                $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").enable(true);
                                $("#dateApptBlockOutEndOn-validation").addClass("hide-display");
                            }else if($("#rdoNever").is(":checked")){
                                $("#dateApptBlockOutEndOn").data("kendoDatePicker").value("");
                                $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").value("");
                                $("#dateApptBlockOutEndOn").data("kendoDatePicker").enable(false);
                                $("#numApptBlockOutEndAfter").data("kendoNumericTextBox").enable(false);
                                $("#dateApptBlockOutEndOn-validation").addClass("hide-display");
                                $("#numApptBlockOutEndAfter-validation").addClass("hide-display");
                            }
                        });
                
                        $("#txtApptBlockOutTitle").kendoMaskedTextBox();
                        $("#txtApptBlockOutDesc").kendoMaskedTextBox();
                        $("#timeApptBlockOutFrom").kendoTimePicker({
                            format: "HH:mm"
                        });
                        $("#timeApptBlockOutTo").kendoTimePicker({
                            format: "HH:mm"
                        });
                        $("#dateApptBlockOutStart").kendoDatePicker();
                        $("#dateApptBlockOutEndOn").kendoDatePicker();
                        $("#numApptBlockOutEndAfter").kendoNumericTextBox({ decimals: 0, format: "#", min: 0});

                        //@@@@@@@ Edit resource Details @@@@@@//
                        $("#txtResourceID").kendoMaskedTextBox();
                        $("#txtResourceID").data("kendoMaskedTextBox").enable(false);
                        $("#txtResourceName").kendoMaskedTextBox();
                        $("#numBookingSeq").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#switchValiOnOff").kendoSwitch();
                        $("#switchOverAlerts").kendoSwitch();
                        $("#switchNotiAlerts").kendoSwitch();
                        $("#txtNEA").kendoMaskedTextBox();
                        $("#switchDockInbound").kendoSwitch(); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement

                        $("#timeStartMON").kendoTimePicker({ format: "HH:mm" });
                        $("#timeStartTUE").kendoTimePicker({ format: "HH:mm" });
                        $("#timeStartWED").kendoTimePicker({ format: "HH:mm" });
                        $("#timeStartTHU").kendoTimePicker({ format: "HH:mm" });
                        $("#timeStartFRI").kendoTimePicker({ format: "HH:mm" });
                        $("#timeStartSAT").kendoTimePicker({ format: "HH:mm" });
                        $("#timeStartSUN").kendoTimePicker({ format: "HH:mm" });
  
                        $("#timeEndMON").kendoTimePicker({ format: "HH:mm" });
                        $("#timeEndTUE").kendoTimePicker({ format: "HH:mm" });
                        $("#timeEndWED").kendoTimePicker({ format: "HH:mm" });
                        $("#timeEndTHU").kendoTimePicker({ format: "HH:mm" });
                        $("#timeEndFRI").kendoTimePicker({ format: "HH:mm" });
                        $("#timeEndSAT").kendoTimePicker({ format: "HH:mm" });
                        $("#timeEndSUN").kendoTimePicker({ format: "HH:mm" });

                        $("#numMaxApptMON").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#numMaxApptTUE").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#numMaxApptWED").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#numMaxApptTHU").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#numMaxApptFRI").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#numMaxApptSAT").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#numMaxApptSUN").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });

                        $("#numMinTime").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#numAvgTime").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#numMaxTime").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });

             
                        //*****APC call for Warehouses based on user id*****//
                        $.ajax({ 
                            url: '/api/vLookupList/GetUserDynamicList/', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            async: false,
                            data: { id: nglUserDynamicLists.CompNEXTrack},
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) {
                                dsWarehouse=data.Data;
                                try {
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Get Company Deatils Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                }
                                                else{
                                                    blnSuccess = true;
                                                    strValidationMsg = "No records were found matching your search criteria";
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get Company Deatils Failure"; }
                                        ngl.showErrMsg("Get Company Deatils Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function(result) { 
                                ngl.showErrMsg("Get Company Deatils Failure", result, null); 
                            } 
                        });  

                        /////kendoDropDownList for Comp/Warehouse /////
                        $("#ddlResourceWarehouse").kendoDropDownList({
                            dataSource:dsWarehouse,
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            filter: "contains",
                        });
                       
                        //***********Get User Page Settings*********//
                        //Modified by RHR for v-8.2 on 09/15/2018 
                        //added logic to call SchedulerController's GetPageSettings API
                        var dsUserPageSettings = null;
                        var sKey = "ManageSchedulePage";
                        $.ajax({ 
                            url: '/api/ManageSchedule/GetPageSettings/' + sKey , 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            async: false,
                            data: { filter : sKey},
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) {
                                //debugger;
                                dsUserPageSettings =data.Data[0]; 
                                try {                                
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                }
                                            }                                       
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to receive this message."; }
                                        ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                            } 
                        }); 
                       
                        if(typeof (dsUserPageSettings) !== 'undefined' && dsUserPageSettings != null && dsUserPageSettings.value != undefined){
                            var psData = JSON.parse(dsUserPageSettings.value);
                            CompControl = psData.CompanyID;
                            $("#ddlResourceWarehouse").data("kendoDropDownList").value(CompControl);
                        }
                        /////on Change event for warehouse/////
                        CompControl =  $("#ddlResourceWarehouse").val();
                        if(CompControl > 0 ){
                    
                        }else{
                            CompControl = -1;
                        }
                        $("#ddlResourceWarehouse").on("change warehouse", function () {
                            CompControl = $(this).data("kendoDropDownList").dataItem().Control;
                            resourcesTabDefaultStates();

                            //LVV CHANGE
                            if (dom_SOPExists === true) { dom_btnChangeSOP.disabled = true; }

                            $("#ROSGrid").data("kendoGrid").dataSource.data([]);
                            $("#UserFieldsGrid").data("kendoGrid").dataSource.read();
                            $("#ApptStatusColorGrid").data("kendoGrid").dataSource.read();
                            $("#ApptOrderColorGrid").data("kendoGrid").dataSource.read();
                            $("#ApptDPHFGrid").data("kendoGrid").dataSource.read();
                            $("#trakingFieldGrid").data("kendoGrid").dataSource.read();
                            editReSourceObject = null;
                            $('#ResourceEdit-UI :input').each(function() {
                                $(this).data('initialValue', $(this).val());
                            });
                            swtval1 = $("#switchValiOnOff").data("kendoSwitch").check();
                            swtval2 = $("#switchOverAlerts").data("kendoSwitch").check();
                            swtval3 = $("#switchNotiAlerts").data("kendoSwitch").check();
                            swtval4 = $("#switchDockInbound").data("kendoSwitch").check(); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                            $('#SaveDockDoorTimeMaxMin-UI :input').each(function() {
                                $(this).data('initialValue', $(this).val());
                            });
                            isSelectedResourceRow = false;

                            //***Save UserPageSettings*******//
                            InsertOrUpdateCurrentUserPageSetting("ManageSchedulePage");
                        });

             
                
                        var lastSelection = null;
                        var isDirtyResourceDetails = false;
                        var isDirtyApptSettings = false;
                        var isSelectedResourceRow = false;
                        var swtval1,swtval1,swtval1;
                        //**********Redource Grid*************//

                        // Block on 30-09-2025
                        // Previous  {className: "cm-icononly-button", name: "delete", text: "", iconClass: "k-icon k-i-trash" }
                        // Add new name ''destroy' for delete.
                        $("#resourceGrid").kendoGrid({
                            //height:200,
                            noRecords: true,
                            dataSource: dsResourcesData,
                            selectable: 'row',
                            change: function(e) {
                                if(isSelectedResourceRow == true){
                                    isDirtyResourceDetails = false;
                                    isDirtyApptSettings = false;
                                    $('#ResourceEdit-UI :input').each(function () {
                                        if($(this).data('initialValue') != $(this).val()){
                                            isDirtyResourceDetails = true;
                                        }
                                    });
                                    //console.log('swtval1 = ' + swtval1)
                                    //console.log('swtval2 = ' + swtval2)
                                    //console.log('swtval3 = ' + swtval3)
                                    //console.log('swtval4 = ' + swtval4)
                                    //console.log('switchValiOnOff = ' + $("#switchValiOnOff").data("kendoSwitch").check())
                                    //console.log('switchOverAlerts = ' + $("#switchOverAlerts").data("kendoSwitch").check())
                                    //console.log('switchNotiAlerts = ' + $("#switchNotiAlerts").data("kendoSwitch").check())
                                    //console.log('switchDockInbound = ' + $("#switchDockInbound").data("kendoSwitch").check())

                                    //if(swtval1 != $("#switchValiOnOff").data("kendoSwitch").check() || swtval2 != $("#switchOverAlerts").data("kendoSwitch").check() || swtval3 != $("#switchNotiAlerts").data("kendoSwitch").check() || swtval4 != $("#switchDockInbound").data("kendoSwitch").check()){ //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                                    //    isDirtyResourceDetails = true;
                                    //}
                                    if(isDirtyResourceDetails == true){                                      
                                        var ResourceDetailsChange = confirm("Warning - unsaved changes. Do you want to save changes made to the \"Resource Details\" section for the selected Resource?");
                                        if(ResourceDetailsChange){
                                            $(this.select()).removeClass("k-state-selected");
                                            $(lastSelection).addClass("k-state-selected");
                                            SaveDockDoorDetails_Click(); //LVV fixed - added the code to do the actual save
                                            return;
                                        }
                                    }
                       
                                    $('#SaveDockDoorTimeMaxMin-UI :input').each(function () {
                                        if($(this).data('initialValue') != $(this).val()){
                                            isDirtyApptSettings = true;
                                        }
                                    });
                                    if(isDirtyApptSettings == true){
                                        var ApptSettingsChange = confirm("Warning - unsaved changes. Do you want to save changes made to the \"Appointment Settings\" section for the selected Resource?");
                                        if(ApptSettingsChange){
                                            $(this.select()).removeClass("k-state-selected");
                                            $(lastSelection).addClass("k-state-selected");
                                            SaveDockDoorTimeMaxMin_Click(); //LVV fixed - added the code to do the actual save
                                            return;
                                        }
                                    }
                                }
                                isSelectedResourceRow = true;
                        
                                lastSelection = $("#resourceGrid .k-state-selected");

                                var grid = $("#resourceGrid").data("kendoGrid");
                                var selectedItem = grid.dataItem(grid.select());
                                editReSourceObject = selectedItem;
                                compDockControl=selectedItem.CompDockControl;
                                $("#ACTFMGrid").data("kendoGrid").dataSource.read();
                                $("#pkgTypGrid").data("kendoGrid").dataSource.read();
                                $("#tempSetGrid").data("kendoGrid").dataSource.read();

                                $("#txtResourceID").val(selectedItem.CompDockDockDoorID);
                                $("#txtResourceName").val(selectedItem.CompDockDockDoorName);
                                $("#numBookingSeq").data("kendoNumericTextBox").value(selectedItem.CompDockBookingSeq);
                                $("#switchValiOnOff").data("kendoSwitch").check(selectedItem.CompDockValidation);
                                $("#switchOverAlerts").data("kendoSwitch").check(selectedItem.CompDockOverrideAlert);
                                $("#switchNotiAlerts").data("kendoSwitch").check(selectedItem.CompDockNotificationAlert);
                                $("#switchDockInbound").data("kendoSwitch").check(selectedItem.CompDockInbound); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                                $("#txtNEA").val(selectedItem.CompDockNotificationEmail);

                                $("#lblCopyResourceID").html(selectedItem.CompDockDockDoorID);
                                $("#lblCopyResourceName").html(selectedItem.CompDockDockDoorName);  

                                $("#lblTempSettingRID").html(selectedItem.CompDockDockDoorID);
                                $("#lblTempSettingRNAME").html(selectedItem.CompDockDockDoorName);  

                                $("#lblPackageTypeRID").html(selectedItem.CompDockDockDoorID);
                                $("#lblPackageTypeRNAME").html(selectedItem.CompDockDockDoorName);  

                                $("#lblACTFMRID").html(selectedItem.CompDockDockDoorID);
                                $("#lblACTFMRNAME").html(selectedItem.CompDockDockDoorName);  

                                //AMSApptCompDockTimeSetting
                                var s = new AllFilter();
                                s.filterName = 'dockDoorControl';
                                s.filterValue = selectedItem.CompDockControl;

                                var dsAMSApptTimeSettings;
                                $.ajax({ 
                                    url: '/api/AMSCompDockdoor/GetDockApptTimeSettings/', 
                                    contentType: 'application/json; charset=utf-8', 
                                    dataType: 'json', 
                                    async:false,
                                    data: { filter: JSON.stringify(s) }, 
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                    success: function(data) {
                                        if(data.Data != null){
                                            dsAMSApptTimeSettings= data.Data[0];
                                        }
                                
                                    } 
                                }); 
                                if(dsAMSApptTimeSettings){
                                    $("#timeStartMON").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.MonStart),"HH:mm"));
                                    $("#timeStartTUE").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.TueStart),"HH:mm"));
                                    $("#timeStartWED").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.WedStart),"HH:mm"));
                                    $("#timeStartTHU").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.ThuStart),"HH:mm"));
                                    $("#timeStartFRI").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.FriStart),"HH:mm"));
                                    $("#timeStartSAT").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.SatStart),"HH:mm"));
                                    $("#timeStartSUN").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.SunStart),"HH:mm"));

                                    $("#timeEndMON").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.MonEnd),"HH:mm"));
                                    $("#timeEndTUE").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.TueEnd),"HH:mm"));
                                    $("#timeEndWED").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.WedEnd),"HH:mm"));
                                    $("#timeEndTHU").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.ThuEnd),"HH:mm"));
                                    $("#timeEndFRI").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.FridEnd),"HH:mm"));
                                    $("#timeEndSAT").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.SatEnd),"HH:mm"));
                                    $("#timeEndSUN").data("kendoTimePicker").value(kendo.toString(new Date(dsAMSApptTimeSettings.SunEnd),"HH:mm"));

                                    $("#numMaxApptMON").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.MonMaxAppt);
                                    $("#numMaxApptTUE").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.TueMaxAppt);
                                    $("#numMaxApptWED").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.WedMaxAppt);
                                    $("#numMaxApptTHU").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.ThuMaxAppt);
                                    $("#numMaxApptFRI").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.FriMaxAppt);
                                    $("#numMaxApptSAT").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.SatMaxAppt);
                                    $("#numMaxApptSUN").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.SunMaxAppt);

                                    $("#numMinTime").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.ApptMinsMin);
                                    $("#numMaxTime").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.ApptMinsMax);
                                    $("#numAvgTime").data("kendoNumericTextBox").value(dsAMSApptTimeSettings.ApptMinsAvg);
                                }else{
                                    $("#timeStartMON").data("kendoTimePicker").value("");
                                    $("#timeStartTUE").data("kendoTimePicker").value("");
                                    $("#timeStartWED").data("kendoTimePicker").value("");
                                    $("#timeStartTHU").data("kendoTimePicker").value("");
                                    $("#timeStartFRI").data("kendoTimePicker").value("");
                                    $("#timeStartSAT").data("kendoTimePicker").value("");
                                    $("#timeStartSUN").data("kendoTimePicker").value("");

                                    $("#timeEndMON").data("kendoTimePicker").value("");
                                    $("#timeEndTUE").data("kendoTimePicker").value("");
                                    $("#timeEndWED").data("kendoTimePicker").value("");
                                    $("#timeEndTHU").data("kendoTimePicker").value("");
                                    $("#timeEndFRI").data("kendoTimePicker").value("");
                                    $("#timeEndSAT").data("kendoTimePicker").value("");
                                    $("#timeEndSUN").data("kendoTimePicker").value("");

                                    $("#numMaxApptMON").data("kendoNumericTextBox").value("");
                                    $("#numMaxApptTUE").data("kendoNumericTextBox").value("");
                                    $("#numMaxApptWED").data("kendoNumericTextBox").value("");
                                    $("#numMaxApptTHU").data("kendoNumericTextBox").value("");
                                    $("#numMaxApptFRI").data("kendoNumericTextBox").value("");
                                    $("#numMaxApptSAT").data("kendoNumericTextBox").value("");
                                    $("#numMaxApptSUN").data("kendoNumericTextBox").value("");

                                    $("#numMinTime").data("kendoNumericTextBox").value("");
                                    $("#numMaxTime").data("kendoNumericTextBox").value("");
                                    $("#numAvgTime").data("kendoNumericTextBox").value("");
                                }
                                $("#SaveDockDoorDetails").data("kendoButton").enable(true);
                                $("#SaveDockDoorTimeMaxMin").data("kendoButton").enable(true);

                                $('#ResourceEdit-UI :input').each(function() {
                                    $(this).data('initialValue', $(this).val());
                                });
                                $('#SaveDockDoorTimeMaxMin-UI :input').each(function() {
                                    $(this).data('initialValue', $(this).val());
                                });
                                swtval1 = $("#switchValiOnOff").data("kendoSwitch").check();
                                swtval2 = $("#switchOverAlerts").data("kendoSwitch").check();
                                swtval3 = $("#switchNotiAlerts").data("kendoSwitch").check();
                                swtval4 = $("#switchDockInbound").data("kendoSwitch").check(); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement

                            },
                            toolbar: [{name:"addnewresource", text:"Add new Resource", iconClass: "k-icon k-i-add"}],
                            editable: 'inline',
                            columns: [
                                { command: [{ name: "apptblockout", text: "Appt Block Periods", iconClass: "k-icon k-i-gears", click: ApptBlockout }, { name: "editresource", text: "", iconClass: "k-icon k-i-pencil", click: editResource }, { className: "cm-icononly-button", name: "destroy", text: "", iconClass: "k-icon k-i-trash" }], title: "Action", width:230 },
                               { field: "CompDockDockDoorID" ,title: "Resource ID" },
                               { field: "CompDockDockDoorName" ,title: "Resource Name" },
                               { field: "CompDockDoorModUser" ,title: "Mod User"},
                               { field: "CompDockDoorModDate" ,title: "Mod Date", format: "{0:M/d/yyyy hh:mm tt}" },
                            ],
                        });

                        $("#resourceGrid").on('click', 'td', function(){
                            var grid=$("#resourceGrid").data("kendoGrid");
                            var row =grid.select();
                            var dataitem=grid.dataItem(row);
                            if(typeof(dataitem) == 'undefined' || dataitem == null)
                            {
                                //LVV CHANGE
                                if (dom_btnCopyDockSetExists === true) { dom_btnCopyDockSet.disabled = true; } 

                                $("#tempSetGrid .k-grid-edittemptype").addClass('k-state-disabled').removeClass("k-grid-edittemptype");
                                $("#pkgTypGrid .k-grid-editpackagetype").addClass('k-state-disabled').removeClass("k-grid-editpackagetype");
                                $("#ACTFMGrid .k-grid-addactfm").addClass('k-state-disabled').removeClass("k-grid-addactfm");
                            }
                            else
                            {
                                //LVV CHANGE
                                if (dom_btnCopyDockSetExists === true) { dom_btnCopyDockSet.disabled = false; } 

                                $("#tempSetGrid .k-state-disabled").removeClass('k-state-disabled').addClass("k-grid-edittemptype");
                                $("#pkgTypGrid .k-state-disabled").removeClass('k-state-disabled').addClass("k-grid-editpackagetype");
                                $("#ACTFMGrid .k-state-disabled").removeClass('k-state-disabled').addClass("k-grid-addactfm");
                            }
                   
                        });

                        $("#resourceGrid").on('click', '.k-grid-addnewresource', function(e) {
                            wndAddResourceDockDoor.title("Add New Resource");

                            $("#rdoNEWResource").prop("checked", true);

                            $("#numWndNewResourceID-validation").addClass("hide-display");
                            $("#txtWndNewResourceName-validation").addClass("hide-display");

                            $("#numWndNewResourceID").val("");
                            $("#txtWndNewResourceName").val("");
                            $("#numWndNewResourceBookSeq").data("kendoNumericTextBox").value("");
                            $("#redioWndNewResourceValidation").data("kendoSwitch").check(false);
                            $("#redioWndNewResourceOverride").data("kendoSwitch").check(false);
                            $("#redioWndNewResourceNotificationAlert").data("kendoSwitch").check(false);
                            $("#chkWndNewResourceInbound").data("kendoSwitch").check(false); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement                        
                            $("#txtWndNewResourceNotificationEmail").val("");

                            wndAddResourceDockDoor.center().open();
                        });

                        $("#numWndNewResourceID").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#numWndNewResourceID-validation").addClass("hide-display");
                            }
                            else {
                                $("#numWndNewResourceID-validation").removeClass("hide-display");
                            }
                        });
                        $("#txtWndNewResourceName").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#txtWndNewResourceName-validation").addClass("hide-display");
                            }
                            else {
                                $("#txtWndNewResourceName-validation").removeClass("hide-display");
                            }
                        });

                        $("#btnWndSaveResource").kendoButton({
                            click: function(e) {
                                var submit = true;
                                if ($("#numWndNewResourceID").val() =="") {
                                    $("#numWndNewResourceID-validation").removeClass("hide-display");
                                    submit = false;
                                }
                                if ($("#txtWndNewResourceName").val() =="") {
                                    $("#txtWndNewResourceName-validation").removeClass("hide-display");
                                    submit = false;
                                }

                                if(submit == true){
                                    var data = new AMSAppointments();
                                    data.CompDockCompControl = CompControl;
                                    data.CompDockDockDoorID=$("#numWndNewResourceID").val();
                                    data.CompDockDockDoorName=$("#txtWndNewResourceName").val();
                                    data.CompDockBookingSeq=$("#numWndNewResourceBookSeq").val();
                                    data.CompDockValidation=$("#redioWndNewResourceValidation").data("kendoSwitch").check();
                                    data.CompDockOverrideAlert=$("#redioWndNewResourceOverride").data("kendoSwitch").check();
                                    data.CompDockNotificationAlert=$("#redioWndNewResourceNotificationAlert").data("kendoSwitch").check();
                                    data.CompDockNotificationEmail=$("#txtWndNewResourceNotificationEmail").val();
                                    data.CompDockInbound=$("#chkWndNewResourceInbound").data("kendoSwitch").check(); //Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                                    $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: "/api/AMSCompDockdoor/SaveNewCompResource/",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: 'json',
                                        data: JSON.stringify(data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            wndAddResourceDockDoor.close();
                                                            //refresh grid and DefaultState
                                                            resourcesTabDefaultStates();
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Comp Dock Door Failure"; }
                                                    ngl.showErrMsg("Save Comp Dock Door Failure", strValidationMsg, null);
                                                }

                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Comp Dock Door Failure", sMsg, null);                        
                                        }
                                    });
                                }
                            }
                        });

                           

                        $("#numWndNewResourceIDCopy").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#numWndNewResourceIDCopy-validation").addClass("hide-display");
                            }
                            else {
                                $("#numWndNewResourceIDCopy-validation").removeClass("hide-display");
                            }
                        });
                        $("#txtWndNewResourceNameCopy").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#txtWndNewResourceNameCopy-validation").addClass("hide-display");
                            }
                            else {
                                $("#txtWndNewResourceNameCopy-validation").removeClass("hide-display");
                            }
                        });

                        $("#btnWndSaveResourceCopy").kendoButton({
                            click: function(e) {
                                var submit = true;
                                var odata = new AMSAppointments;

                                if($("#rdoNEWResource").is(":checked")){

                                    if ($("#numWndNewResourceIDCopy").val() == "") {
                                        $("#numWndNewResourceIDCopy-validation").removeClass("hide-display");
                                        submit = false;
                                    }
                                    if ($("#txtWndNewResourceNameCopy").val() =="") {
                                        $("#txtWndNewResourceNameCopy-validation").removeClass("hide-display");
                                        submit = false;
                                    }
                                    odata.CopyFromDockControl = compDockControl
                                    odata.CopyToDockControl ="";
                                    odata.CopyToNew = true;
                                    odata.DockDoorID = $("#numWndNewResourceIDCopy").val(); 
                                    odata.DockDoorName = $("#txtWndNewResourceNameCopy").val();
                                }else{
                                    odata.CopyFromDockControl = compDockControl 
                                    odata.CopyToDockControl = $("#ddlWndExtSelectResourceCopy").val(); 
                                    odata.CopyToNew = false;
                                    odata.DockDoorID = "";
                                    odata.DockDoorName = "";
                                }

                                if(submit == true){
                                    $.ajax({
                                        url: "api/AMSCompDockdoor/CopyResourceConfig",
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: 'json',
                                        data: JSON.stringify(odata),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Save Resource and Copy Settings Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Save Resource and Copy Settings Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Resource and Copy Settings Failure"; }
                                                    ngl.showErrMsg("Save Resource and Copy Settings Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //close popup
                                            wndCopyResourceSettings.close();
                                            //refresh grid and DefaultState
                                            resourcesTabDefaultStates();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Resource and Copy Settings Failure", sMsg, null);
                                        }
                                    });
                                }
                       
                            }
                        });


                        $("#SaveDockDoorDetails").kendoButton({
                            click: function(e) {
                                SaveDockDoorDetails_Click();
                              
                            }
                        });
                        $("#SaveDockDoorDetails").data("kendoButton").enable(false);

                        $("#SaveDockDoorTimeMaxMin").kendoButton({
                            click: function(e) {
                                SaveDockDoorTimeMaxMin_Click();
                               
                            }
                        });
                        $("#SaveDockDoorTimeMaxMin").data("kendoButton").enable(false);

                        $("#ddlApptBlockOutRecurrenceType").kendoDropDownList({
                            dataSource:dsApptBlockOurRecurrenceType,
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            filter: "contains",
                        });


                        $("#txtApptBlockOutTitle").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#txtApptBlockOutTitle-validation").addClass("hide-display");
                            }
                            else {
                                $("#txtApptBlockOutTitle-validation").removeClass("hide-display");
                            }
                        });
                        $("#timeApptBlockOutFrom").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#timeApptBlockOutFrom-validation").addClass("hide-display");
                            }
                            else {
                                $("#timeApptBlockOutFrom-validation").removeClass("hide-display");
                            }
                        });
                        $("#timeApptBlockOutTo").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#timeApptBlockOutTo-validation").addClass("hide-display");
                            }
                            else {
                                $("#timeApptBlockOutTo-validation").removeClass("hide-display");
                            }
                        });
                        $("#dateApptBlockOutStart").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#dateApptBlockOutStart-validation").addClass("hide-display");
                            }
                            else {
                                $("#dateApptBlockOutStart-validation").removeClass("hide-display");
                            }
                        });
                        $("#dateApptBlockOutEndOn").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#dateApptBlockOutEndOn-validation").addClass("hide-display");
                            }
                            else {
                                $("#dateApptBlockOutEndOn-validation").removeClass("hide-display");
                            }
                        });
                        $("#numApptBlockOutEndAfter").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#numApptBlockOutEndAfter-validation").addClass("hide-display");
                            }
                            else {
                                $("#numApptBlockOutEndAfter-validation").removeClass("hide-display");
                            }
                        });


                        //*************Single DAY Checkox Checking********//
                       

                        $("#btnWndCancleApptblkoutPeriod").hide();
                        var ApptDockDoorBlockOutPeriod = {};
                        $("#btnWndAddApptblkoutPeriod").kendoButton({
                            click:function(){
                                var submit = true;
                                if ($("#txtApptBlockOutTitle").val() =="") {
                                    $("#txtApptBlockOutTitle-validation").removeClass("hide-display");
                                    submit = false;
                                }
                                if ($("#timeApptBlockOutFrom").val() =="") {
                                    $("#timeApptBlockOutFrom-validation").removeClass("hide-display");
                                    submit = false;
                                }
                                if ($("#timeApptBlockOutTo").val() =="") {
                                    $("#timeApptBlockOutTo-validation").removeClass("hide-display");
                                    submit = false;
                                }
                                if ($("#dateApptBlockOutStart").val() =="") {
                                    $("#dateApptBlockOutStart-validation").removeClass("hide-display");
                                    submit = false;
                                }

                                ApptDockDoorBlockOutPeriod.DockControl = DockBlockCompDockContol;
                                ApptDockDoorBlockOutPeriod.RecurrenceType = $("#ddlApptBlockOutRecurrenceType").val();
                                ApptDockDoorBlockOutPeriod.Title = $("#txtApptBlockOutTitle").val();
                                ApptDockDoorBlockOutPeriod.Description = $("#txtApptBlockOutDesc").val();
                                ApptDockDoorBlockOutPeriod.StartTime = $("#timeApptBlockOutFrom").val();
                                ApptDockDoorBlockOutPeriod.EndTime = $("#timeApptBlockOutTo").val();

                                ApptDockDoorBlockOutPeriod.StartDate = $("#dateApptBlockOutStart").val();
                                if($("#rdoOn").is(":checked")){
                                    if ($("#dateApptBlockOutEndOn").val() =="") {
                                        $("#dateApptBlockOutEndOn-validation").removeClass("hide-display");
                                        submit = false;
                                    }
                                    ApptDockDoorBlockOutPeriod.UNTIL = $("#dateApptBlockOutEndOn").val();
                                    ApptDockDoorBlockOutPeriod.COUNT = 0;

                                }
                                else if($("#rdoAfter").is(":checked")){
                                    if ($("#numApptBlockOutEndAfter").val() == 0) {
                                        $("#numApptBlockOutEndAfter-validation").removeClass("hide-display");
                                        submit = false;
                                    }
                                    ApptDockDoorBlockOutPeriod.UNTIL = null;
                                    ApptDockDoorBlockOutPeriod.COUNT = $("#numApptBlockOutEndAfter").val();
                                }
                                else if($("#rdoNever").is(":checked")){
                                    ApptDockDoorBlockOutPeriod.UNTIL = 0;
                                    ApptDockDoorBlockOutPeriod.COUNT = null;
                                }
                       
                                ApptDockDoorBlockOutPeriod.blnSun = $("#WeeklySUN").is(":checked");
                                ApptDockDoorBlockOutPeriod.blnMon = $("#WeeklyMON").is(":checked");
                                ApptDockDoorBlockOutPeriod.blnTue = $("#WeeklyTUE").is(":checked");
                                ApptDockDoorBlockOutPeriod.blnWed = $("#WeeklyWED").is(":checked");
                                ApptDockDoorBlockOutPeriod.blnThu = $("#WeeklyTHU").is(":checked");
                                ApptDockDoorBlockOutPeriod.blnFri = $("#WeeklyFRI").is(":checked");
                                ApptDockDoorBlockOutPeriod.blnSat = $("#WeeklySAT").is(":checked");
                                ApptDockDoorBlockOutPeriod.DockBlockOn = $("#chkApptBlockOutOn").is(":checked");

                                if(submit == true){

                                    $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: "/api/AMSCompDockdoor/SaveDockApptBlockOutPeriod/",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: 'json',
                                        data: JSON.stringify(ApptDockDoorBlockOutPeriod),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Save Appt Block Out Period Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Save Appt Block Out Period Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Appt Block Out Period Failure"; }
                                                    ngl.showErrMsg("Save Appt Block Out Period Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //grid refresh
                                            $("#ApptBlkOutPeriodGrid").data("kendoGrid").dataSource.read();
                                            AddNewBlockOutState();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Appt Block Out Period Failure", sMsg, null);                        
                                        }
                                    });
                                }
                            }
                        });

                        $("#btnWndCancleApptblkoutPeriod").kendoButton({
                            click:function(){
                                AddNewBlockOutState();
                            }
                        })

                        var DockBlockCompDockContol = 0;
                        dsApptBlockoutPeriod = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    s.filterName = 'DockBlockCompDockContol';
                                    s.ParentControl = DockBlockCompDockContol;
                                    $.ajax({ 
                                        url: '/api/AMSCompDockDoor/GetDockBlockOutPeriodsByDock/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get Appt Block Out Periods Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appt Block Out Periods Failure"; }
                                                    ngl.showErrMsg("Get Appt Block Out Periods Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },
                                destroy: function(options) {
                                    $.ajax({
                                        url: 'api/AMSCompDockdoor/DeleteDockApptBlockOutPeriod', 
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Delete Appt Block Out Period Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Delete Appt Block Out Period Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete Appt Block Out Period Failure"; }
                                                    ngl.showErrMsg("Delete Appt Block Out Period Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //refresh grid
                                            $("#ApptBlkOutPeriodGrid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Appt Block Out Period Failure");
                                            ngl.showErrMsg("Delete Appt Block Out Period Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "DockBlockControl",
                                    fields: {
                                        DockBlockControl: { type: "number" },
                                        DockControl: { type: "number" },
                                        DockBlockExpired: { type: "bool" },
                                        DockBlockOn: { type: "bool" },
                                        RecurrenceType: { type: "number" },
                                        Title: { type: "string" },
                                        Description: { type: "string" },
                                        StartTime: { type: "date"},
                                        EndTime: { type: "date"},
                                        StartDate: { type: "date"},
                                        Until: { type: "date"},
                                        Count: { type: "number" },
                                        blnSun: { type: "bool" },
                                        blnMon: { type: "bool" },
                                        blnTue: { type: "bool" },
                                        blnWed: { type: "bool" },
                                        blnThu: { type: "bool" },
                                        blnFri: { type: "bool" },
                                        blnSat: { type: "bool" }
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access Appt Block Out Periods Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });

                        //*****Wnd GRID Appointment Block Out Period Grid*****// 
                        $("#ApptBlkOutPeriodGrid").kendoGrid({
                            //height:100,
                            noRecords: true,
                            autoBind: false,
                            dataSource: dsApptBlockoutPeriod,
                            editable: "inline",
                            columns: [
                                { command: [{ className: "cm-icononly-button", name: "editapptblkoutperiods", text: "", iconClass: "k-icon k-i-pencil", click: editapptblkoutperiods},{ className: "cm-icononly-button", name: "delete", text:"", iconClass: "k-icon k-i-trash"}], title: "Action", width:100 },
                                { field: "DockBlockControl", title:"DockBlockControl", hidden:true },
                                { field: "DockControl", title:"DockControl", hidden:true },
                                { field: "Title", title:"Title" },
                                { field: "Description", title:"Desc" },
                                { field: "RecurrenceType", title:"Recurrence Type", template:function(data){var idx1 = dsApptBlockOurRecurrenceType._data.map(function(e) { return e.Control; }).indexOf(data.RecurrenceType); if(idx1 != -1){return dsApptBlockOurRecurrenceType._data[idx1].Name}else{return "None"}}},
                                { field: "StartDate", title:"Start Date", format: "{0:M/d/yyyy}" },
                                { field: "StartTime", title:"Start Time", format: "{0:HH:mm}" },
                                { field: "EndTime", title:"End Time", format: "{0:HH:mm}" },
                                { field: "DockBlockExpired", title:"Expired", template: '<input type="checkbox" #= DockBlockExpired ? "checked=checked" : "" # disabled="disabled" ></input>' },
                                { field: "DockBlockOn", title:"On", template: '<input type="checkbox" #= DockBlockOn ? "checked=checked" : "" # disabled="disabled" ></input>' },
                                { field: "Until", title:"Until", format: "{0:M/d/yyyy}", hidden:true },                  
                                { field: "Count", title:"Count", hidden:true },
                                { field: "blnSun", title:"blnSun", template: '<input type="checkbox" #= blnSun ? "checked=checked" : "" # disabled="disabled" ></input>', hidden:true },
                                { field: "blnMon", title:"blnMon", template: '<input type="checkbox" #= blnMon ? "checked=checked" : "" # disabled="disabled" ></input>', hidden:true },
                                { field: "blnTue", title:"blnTue", template: '<input type="checkbox" #= blnTue ? "checked=checked" : "" # disabled="disabled" ></input>', hidden:true },
                                { field: "blnWed", title:"blnWed", template: '<input type="checkbox" #= blnWed ? "checked=checked" : "" # disabled="disabled" ></input>', hidden:true },
                                { field: "blnThu", title:"blnThu", template: '<input type="checkbox" #= blnThu ? "checked=checked" : "" # disabled="disabled" ></input>', hidden:true },
                                { field: "blnFri", title:"blnFri", template: '<input type="checkbox" #= blnFri ? "checked=checked" : "" # disabled="disabled" ></input>', hidden:true },
                                { field: "blnSat", title:"blnSat", template: '<input type="checkbox" #= blnSat ? "checked=checked" : "" # disabled="disabled" ></input>', hidden:true },
                            ],
                        });
                

                        $("#tempSetGrid").kendoGrid({
                            height:150,
                            noRecords: true,
                            autoBind: false,
                            dataSource: dsTemperature,
                            toolbar: [{name:"edittemptype", text:"Edit", iconClass: "k-icon k-i-pencil"}],
                            columns: [
                               { field:"PTCaption",title: "Temperature Type" },
                               { field: "PTBitPos" ,hidden: true},
                               { field: "PTOn" ,hidden: true},
                            ],
                        });

                        $("#tempSetGrid").on('click', '.k-grid-edittemptype', function (e) {                            
                            $("#tempgrid").data("kendoGrid").dataSource.read();
                            wndtempTypGrid.title("Resource - Edit Temperature Types");
                            wndtempTypGrid.center().open();
                        })

                        //************ Temperature Grid popup***************/
                        dsTempTypeField = new kendo.data.DataSource({
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    s.filterName = 'dockDoorControl';
                                    s.filterValue = compDockControl;
                                    $.ajax({ 
                                        url: '/api/AMSCompDockdoor/GetEditableTempTypesForDock/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            console.log(data);
                                            bitpositionTSArray=[];
                                            for(var i in data.Data)
                                            {
                                                if(data.Data[i].PTOn==true){
                                                    bitpositionTSArray.push(data.Data[i].PTBitPos);
                                                }
                                            }
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("GetSupportedTempTypesForDock Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get UserField Failure"; }
                                                    ngl.showErrMsg("GetSupportedTempTypesForDock Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "Control",
                                    fields: {
                                        Control: { type: "number" },
                                        PTCaption: { type: "string" },
                                        PTOn: { type: "boolean"} ,
                                        PTBitPos: { type: "number"}
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access temperature grid Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });
               
                        $("#tempgrid").kendoGrid({
                            height:400,
                            noRecords: true,
                            autoBind: false,
                            dataSource: dsTempTypeField,
                            columns: [
                               { field: "PTCaption",title: "Package Type"},
                               { field: "PTBitPos",hidden: true},
                               {template:'<input type="checkbox" #= PTOn ? \'checked="checked"\' : "" # class="chkbxTS" />'}
                            ],
                        });

                        $("#tempgrid .k-grid-content").on("change", "input.chkbxTS", function(e) {
                            bitpositionTSArray = [];
                            var grid = $("#tempgrid").data("kendoGrid"),
                            dataItem = grid.dataItem($(e.target).closest("tr"));
                            dataItem.set("PTOn", this.checked);
                            console.log("checkbox: " + this.checked);
                            if(isInArray(dataItem.PTBitPos,bitpositionTSArray) ){
                                if(this.checked==false){
                                    bitpositionTSArray.splice($.inArray(dataItem.PTBitPos, bitpositionTSArray),1);
                                }
                            }else{
                                if(this.checked==true){
                                    bitpositionTSArray.push(dataItem.PTBitPos);
                                }
                            }
                        });

                        $("#btnWndSaveTS").kendoButton({
                            click: function(e) {
                                var csv="";

                                //jQuery.each( bitpositionTSArray, function( i, val ) {
                                //    if(csv==""){
                                //        csv=csv+val;
                                //    }else{
                                //        csv=csv+','+val;
                                //    }
                                //});

                                var grid = $("#tempgrid").data("kendoGrid");

                                var items = grid.dataSource.data();
                                var selected = [];

                                for (var i = 0; i < items.length; i++) {
                                    if (items[i].PTOn === true) {
                                        selected.push(items[i].PTBitPos);
                                    }
                                }
                                var csv = selected.join(',');

                                var s= new AllFilter();
                                s.filterValue=compDockControl;
                                s.data = csv;
                                console.log(JSON.stringify(s));
                                $.ajax({
                                    async: false,
                                    type: "POST",
                                    url: "api/AMSCompDockdoor/SaveDockTempTypes",
                                    // contentType: "application/json; charset=utf-8",
                                    dataType: 'json',
                                    data: {"":JSON.stringify(s)}, 
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                    success: function (data) {
                                        try {
                                            var blnSuccess = false;
                                            var blnErrorShown = false;
                                            var strValidationMsg = "";
                                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                    blnErrorShown = true;
                                                    ngl.showErrMsg("Save TemparatureType Failure", data.Errors, null);
                                                }
                                                else {
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            if (data.Data[0] == false) {
                                                                ngl.showWarningMsg("Save TemparatureType Failure!", "", null);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (blnSuccess === false && blnErrorShown === false) {
                                                if (strValidationMsg.length < 1) { strValidationMsg = "Save TemparatureType Failure"; }
                                                ngl.showErrMsg("Save TemparatureType Failure", strValidationMsg, null);
                                            }
                                        } catch (err) {
                                            ngl.showErrMsg(err.name, err.description, null);
                                        }
                                        //refresh grid
                                        $("#TempTypeAddEdit").data("kendoWindow").close();
                                        $("#tempSetGrid").data("kendoGrid").dataSource.read();
                                    },
                                    error: function (xhr, textStatus, error) {
                                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                        ngl.showErrMsg("Save TemparatureType Failure", sMsg, null);                        
                                    }
                                });
                            }
                        });

                        //************Package grid details************//
                        dsPackageGrid = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    s.filterName = 'dockDoorControl';
                                    s.filterValue = compDockControl;
                                    $.ajax({ 
                                        url: '/api/AMSCompDockdoor/GetSupportedPackageTypesForDock/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("GetSupportedPackageTypesForDock Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "GetSupportedPackageTypesForDock Failure"; }
                                                    ngl.showErrMsg("GetSupportedPackageTypesForDock Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                        } 
                                    }); 
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "Control",
                                    fields: {
                                        Control: { type: "number" },
                                        PTCaption: { type: "string"},
                                        PTOn: { type: "boolean"} ,
                                        PTBitPos: { type: "number"}  
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("GetSupportedPackageTypesForDock Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });

                        $("#pkgTypGrid").kendoGrid({
                            height:150,
                            noRecords: true,
                            autoBind: false,
                            dataSource: dsPackageGrid,
                            toolbar: [{name:"editPackageType", text:"Edit", iconClass: "k-icon k-i-pencil"}],
                            columns: [
                               { field: "PTCaption",title:"Package Type" },
                               { field: "PTBitPos" ,hidden: true},
                               { field: "PTOn" ,hidden: true}
                            ],
                        });

                        $("#pkgTypGrid").on('click', '.k-grid-editpackagetype', function(e) {
                            $("#packagegrid").data("kendoGrid").dataSource.read();
                            wndpkgTypGrid.title("Resource - Edit Package Types");
                            wndpkgTypGrid.center().open();
                        })

                        //***********PackageType grid popup**********//    
                        dsPackageTypeField = new kendo.data.DataSource({
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    s.filterName = 'dockDoorControl';
                                    s.filterValue = compDockControl;
                                    $.ajax({ 
                                        url: '/api/AMSCompDockdoor/GetEditablePackageTypesForDock/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            bitpositionPTArray = [];
                                            for(var i in data.Data)
                                            {
                                                if(data.Data[i].PTOn==true){
                                                    bitpositionPTArray.push(data.Data[i].PTBitPos);
                                                }
                                            }                                            
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("GetSupportedPackageTypesForDock Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "GetSupportedPackageTypesForDock Failure"; }
                                                    ngl.showErrMsg("GetSupportedPackageTypesForDock Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },      
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "Control",
                                    fields: {
                                        Control: { type: "number" },
                                        PTCaption: { type: "string"},
                                        PTOn: { type: "boolean"} ,
                                        PTBitPos: { type: "number"}
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("GetSupportedPackageTypesForDock data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });
               
                        $("#packagegrid").kendoGrid({
                            height:400,
                            noRecords: true,
                            autoBind: false,
                            dataSource: dsPackageTypeField,                            
                            columns: [
                               { field: "PTCaption",title: "Package Type"},
                               { field: "PTBitPos",hidden: true},
                               {template:'<input type="checkbox" #= PTOn ? \'checked="checked"\' : "" # class="chkbxPT" />'}
                            ],
                        });

                        $("#packagegrid .k-grid-content").on("change", "input.chkbxPT", function (e) {
                            
                            var grid = $("#packagegrid").data("kendoGrid"),
                            dataItem = grid.dataItem($(e.target).closest("tr"));
                            dataItem.set("PTOn", this.checked);                           
                            if(isInArray(dataItem.PTBitPos,bitpositionPTArray) ){
                                if(this.checked==false){
                                    bitpositionPTArray.splice($.inArray(dataItem.PTBitPos, bitpositionPTArray),1);
                                }
                            }else{
                                if(this.checked==true){
                                    bitpositionPTArray.push(dataItem.PTBitPos);
                                }
                            }
                        });

                        function isInArray(value, array) {
                            return array.indexOf(value) > -1;
                        }

                        $("#btnWndSavePT").kendoButton({
                            click: function(e) {
                                var csv="";
                                jQuery.each( bitpositionPTArray, function( i, val ) {
                                    if(csv==""){
                                        csv=csv+val;
                                    }else{
                                        csv=csv+','+val;
                                    }
                                });

                                var s= new AllFilter();
                                s.filterValue=compDockControl;
                                s.data=csv;
                                $.ajax({
                                    async: false,
                                    type: "POST",
                                    url: "api/AMSCompDockdoor/SaveDockPackageTypes",
                                    // contentType: "application/json; charset=utf-8",
                                    dataType: 'json',
                                    data: {"":JSON.stringify(s)}, 
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                    success: function (data) {
                                        try {
                                            var blnSuccess = false;
                                            var blnErrorShown = false;
                                            var strValidationMsg = "";
                                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                    blnErrorShown = true;
                                                    ngl.showErrMsg("Save PackageType Failure", data.Errors, null);
                                                }
                                                else {
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            if (data.Data[0] == false) {
                                                                ngl.showWarningMsg("Save PackageType Failure!", "", null);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (blnSuccess === false && blnErrorShown === false) {
                                                if (strValidationMsg.length < 1) { strValidationMsg = "Save PackageType Failure"; }
                                                ngl.showErrMsg("Save PackageType Failure", strValidationMsg, null);
                                            }
                                        } catch (err) {
                                            ngl.showErrMsg(err.name, err.description, null);
                                        }
                                        //refresh grid
                                        $("#PackageTypeAddEdit").data("kendoWindow").close();
                                        $("#pkgTypGrid").data("kendoGrid").dataSource.read();
                                    },
                                    error: function (xhr, textStatus, error) {
                                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                        ngl.showErrMsg("Save PackageType Failure", sMsg, null);                        
                                    }
                                });
                            }
                        });

                        //***** Appointment Calulation Time Factor Maintenance grid*****//
                        dsActfm = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 10,
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    s.ParentControl = compDockControl;
                                    $.ajax({ 
                                        url: '/api/AMSCompDockdoor/GetDockTimeCalcFactors/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {                                    
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Calculation Time Factor Failure"; }
                                                    ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },      
                                destroy: function(options) {
                                    $.ajax({
                                        url: '/api/AMSCompDockdoor/DeleteDockTimeCalcFactor/', 
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Delete ACTFM Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Delete ACTFM Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete ACTFM Failure"; }
                                                    ngl.showErrMsg("Delete ACTFM Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //refresh grid
                                            $("#ACTFMGrid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete ACTFM Failure");
                                            ngl.showErrMsg("Delete ACTFM Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "DockTCFCalcFactorTypeControl",
                                    fields: {
                                        DockTCFCalcFactorTypeControl: { type: "number" },
                                        DockTCFName: { type: "string" },
                                        DockTCFDescription: { type: "string" },
                                        DockTCFAmount: { type: "number" },
                                        DockTCFUOM:{ type: "string" },
                                        DockTCFTimeFactor: { type: "number" },
                                        DockTCFOn:{ type: "boolean" }
                                                                            
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access ACTF Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });

                        // For delete the row we change the text 'delete' to 'destroy'. Added in 27-09-2025
                        $("#ACTFMGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            groupable: true,
                            resizable: true,
                            reorderable: true,
                            height:190,
                            autoBind: false,
                            dataSource: dsActfm,
                            toolbar: [{name:"addactfm", text:"Add", iconClass: "k-icon k-i-add"}],
                            editable: 'inline',
                            columns: [
                                { command: [{ className: "cm-icononly-button", name: "editACTFM", text: "", iconClass: "k-icon k-i-pencil", click: editACTFM }, { className: "cm-icononly-button", name: "destroy", text: "", iconClass: "k-icon k-i-trash" }], title: "Action",width:100 },
                               { field: "DockTCFName",title:"Name" },
                               { field: "DockTCFDescription" ,title:"Desc"},
                               { field: "DockTCFCalcFactorTypeControl",title:"Calc Type",template:function(data){var idx1 = calcTypesDropdaown._data.map(function(e) { return e.Control; }).indexOf(data.DockTCFCalcFactorTypeControl); if(idx1 != -1){return calcTypesDropdaown._data[idx1].Name}else{return "None"}}}, 
                               { field: "DockTCFUOM",title:"UOM"}, //template:function(data){var idx1 = uomDropdaown._data.map(function(e) { return e.Name; }).indexOf(data.DockTCFUOM); if(idx1 != -1){return uomDropdaown._data[idx1].Name}else{return "None"}}},
                               { field: "DockTCFAmount",title:"Amount"},
                               { field: "DockTCFTimeFactor",title:"Time Factor" },
                               { field: "DockTCFOn",title:"On", template: '<input type="checkbox" #= DockTCFOn ? "checked=checked" : "" # disabled="disabled" ></input>', width: "40px" },
                            ],
                        });

                        var ACTFMObject;
                        $("#ACTFMGrid").on('click', '.k-grid-addactfm', function(e) {

                            $("#txtWndName-validation").addClass("hide-display");
                            $("#ddlWndCalcType").data("kendoDropDownList").enable(true);
                            $("#ddlWndUnitsOfMesure").data("kendoDropDownList").enable(true);

                            ACTFMObject = { DockTCFControl : 0 }

                            $("#txtWndName").data("kendoMaskedTextBox").value("");
                            $("#txtWndDescription").data("kendoMaskedTextBox").value("");
                  

                            var dsCaltype;
                            var v = new  vLookupListCriteria();
                            v.id = nglGlobalDynamicLists.CalcFactorTypeForDock;
                            v.sortKey  = 1;
                            v.criteria = compDockControl;
                    
                            $.ajax({ 
                                url:  '/api/vLookupList/GetGlobalDynamicListFiltered/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                async:false,
                                data: { filter: JSON.stringify(v) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) {
                                    $("#ddlWndCalcType").data("kendoDropDownList").setDataSource(data.Data);
                                    dsCaltype = data;
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                    }
                                                    else{
                                                        blnSuccess = true;
                                                        strValidationMsg = "No records were found matching your search criteria";
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Calculation Time Factor Failure"; }
                                            ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                               
                                }, 
                                error: function(result) { 
                                    options.error(result);
                                
                                } 
                            }); 

                            $("#ddlWndCalcType").data("kendoDropDownList").select(0);

                            if($("#ddlWndCalcType").val() != 0){
                                var vv = new  vLookupListCriteria();
                                vv.id = nglGlobalDynamicLists.CalcFactorTypeUOM;
                                vv.sortKey  = 1;
                                vv.criteria = compDockControl+","+$("#ddlWndCalcType").val();

                                $.ajax({ 
                                    url:  '/api/vLookupList/GetGlobalDynamicListFiltered/', 
                                    contentType: 'application/json; charset=utf-8', 
                                    dataType: 'json', 
                                    async:false,
                                    data: { filter: JSON.stringify(vv) }, 
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                    success: function(data) {
                                        $("#ddlWndUnitsOfMesure").data("kendoDropDownList").setDataSource(data.Data);
                                        try {
                                            var blnSuccess = false;
                                            var blnErrorShown = false;
                                            var strValidationMsg = "";
                                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                    blnErrorShown = true;
                                                    ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", data.Errors, null);
                                                }
                                                else {
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                            blnSuccess = true;
                                                        }
                                                        else{
                                                            blnSuccess = true;
                                                            strValidationMsg = "No records were found matching your search criteria";
                                                        }
                                                    }
                                                }
                                            }
                                            if (blnSuccess === false && blnErrorShown === false) {
                                                if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Calculation Time Factor Failure"; }
                                                ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", strValidationMsg, null);
                                            }
                                        } catch (err) {
                                            ngl.showErrMsg(err.name, err.description, null);
                                        }
                               
                                    }, 
                                    error: function(result) { 
                                        options.error(result);
                                
                                    } 
                                }); 
                            }

                   
                            $("#ddlWndUnitsOfMesure").data("kendoDropDownList").select(0);
                            $("#numWndAmount").data("kendoNumericTextBox").value("");
                            $("#numWndTimeFactor").data("kendoNumericTextBox").value("");
                            $("#switchWndOnOff").data("kendoSwitch").check(true);

                            wndAddEditResource.title("Add Appointment Caluclation Time Factor");
                            wndAddEditResource.center().open();
                        });

                        $("#txtWndName").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#txtWndName-validation").addClass("hide-display");
                            }
                            else {
                                $("#txtWndName-validation").removeClass("hide-display");
                            }
                        });

                        function editACTFM(e){
                            var dataItem = this.dataItem($(e.currentTarget).closest("tr")); 

                            var dsCaltype;
                            var v = new  vLookupListCriteria();
                            v.id = nglGlobalDynamicLists.CalcFactorTypeForDock;
                            v.sortKey  = 2;
                            v.criteria = compDockControl;
                    
                            $.ajax({ 
                                url:  '/api/vLookupList/GetGlobalDynamicListFiltered/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                async:false,
                                data: { filter: JSON.stringify(v) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) {
                                    $("#ddlWndCalcType").data("kendoDropDownList").setDataSource(data.Data);
                                    dsCaltype = data;
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                    }
                                                    else{
                                                        blnSuccess = true;
                                                        strValidationMsg = "No records were found matching your search criteria";
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Calculation Time Factor Failure"; }
                                            ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                               
                                }, 
                                error: function(result) { 
                                    options.error(result);
                                
                                } 
                            }); 

                            if(dsCaltype.Data.length > 0){
                                var vv = new  vLookupListCriteria();
                                vv.id = nglGlobalDynamicLists.CalcFactorTypeUOM;
                                vv.sortKey  = 2;
                                vv.criteria = compDockControl+","+dataItem.DockTCFCalcFactorTypeControl;//$("#ddlWndCalcType").val();

                                $.ajax({ 
                                    url:  '/api/vLookupList/GetGlobalDynamicListFiltered/', 
                                    contentType: 'application/json; charset=utf-8', 
                                    dataType: 'json', 
                                    async:false,
                                    data: { filter: JSON.stringify(vv) }, 
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                    success: function(data) {
                                        $("#ddlWndUnitsOfMesure").data("kendoDropDownList").setDataSource(data.Data);
                                        try {
                                            var blnSuccess = false;
                                            var blnErrorShown = false;
                                            var strValidationMsg = "";
                                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                    blnErrorShown = true;
                                                    ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", data.Errors, null);
                                                }
                                                else {
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                            blnSuccess = true;
                                                        }
                                                        else{
                                                            blnSuccess = true;
                                                            strValidationMsg = "No records were found matching your search criteria";
                                                        }
                                                    }
                                                }
                                            }
                                            if (blnSuccess === false && blnErrorShown === false) {
                                                if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Calculation Time Factor Failure"; }
                                                ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", strValidationMsg, null);
                                            }
                                        } catch (err) {
                                            ngl.showErrMsg(err.name, err.description, null);
                                        }
                               
                                    }, 
                                    error: function(result) { 
                                        options.error(result);
                                
                                    } 
                                }); 
                            }

                            $("#txtWndName-validation").addClass("hide-display");

                   
                            ACTFMObject = dataItem;

                            $("#txtWndName").data("kendoMaskedTextBox").value(dataItem.DockTCFName);
                            $("#txtWndDescription").data("kendoMaskedTextBox").value(dataItem.DockTCFDescription);
                            var ddlCalType = $("#ddlWndCalcType").data("kendoDropDownList");
                            ddlCalType.value(parseInt(dataItem.DockTCFCalcFactorTypeControl));
                            ddlCalType.enable(false);
                            var ddlUOM = $("#ddlWndUnitsOfMesure").data("kendoDropDownList");
                            ddlUOM.value(dataItem.DockTCFUOM);
                            ddlUOM.enable(false);
                            $("#numWndAmount").data("kendoNumericTextBox").value(dataItem.DockTCFAmount);
                            $("#numWndTimeFactor").data("kendoNumericTextBox").value(dataItem.DockTCFTimeFactor);
                            $("#switchWndOnOff").data("kendoSwitch").check(dataItem.DockTCFOn);

                            wndAddEditResource.title("Edit Appointment Caluclation Time Factor");
                            wndAddEditResource.center().open();
                        }

                        //*************calc types**********//:
                        calcTypesDropdaown = new kendo.data.DataSource({
                            transport: { 
                                read: function(options) { 

                                    var v = new  vLookupListCriteria();
                                    v.id = nglGlobalDynamicLists.CalcFactorTypeForDock;
                                    v.sortKey  = 0;
                                    v.criteria = compDockControl;

                                    $.ajax({ 
                                        url:  '/api/vLookupList/GetGlobalDynamicListFiltered/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(v) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Calculation Time Factor Failure"; }
                                                    ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },      
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "Control",
                                    fields: {
                                        Control: { type: "number" },
                                        Name: { type: "string" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access ACTF Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });
                        //***********Unit of Measure************// 
                        uomDropdaown = new kendo.data.DataSource({
                            transport: { 
                                read: function(options) { 

                                    var v = new  vLookupListCriteria();
                                    v.id = nglGlobalDynamicLists.CalcFactorTypeUOM;
                                    v.sortKey  = 0;
                                    v.criteria = compDockControl+","+3;

                                    $.ajax({ 
                                        url:  '/api/vLookupList/GetGlobalDynamicListFiltered/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(v) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get Appointment UOM Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment UOM Failure"; }
                                                    ngl.showErrMsg("Get Appointment UOM Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },      
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "Control",
                                    fields: {
                                        Control: { type: "number" },
                                        Name: { type: "string" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access ACTF Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });

                        $("#txtWndName").kendoMaskedTextBox();
                        $("#txtWndDescription").kendoMaskedTextBox();
                        $("#numWndAmount").kendoNumericTextBox({ decimals: 1, format: "#", min: 0});
                        $("#numWndTimeFactor").kendoNumericTextBox({ decimals: 0, format: "#", min: 0});
                        $("#switchWndOnOff").kendoSwitch();

                        $("#ddlWndCalcType").kendoDropDownList({
                            dataSource:calcTypesDropdaown,
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            filter: "contains",
                        });

                        $("#ddlWndCalcType").on("change",function(){
                            // $("#ddlWndUnitsOfMesure").data("kendoDropDownList").dataSource.read();
                            var vv = new  vLookupListCriteria();
                            vv.id = nglGlobalDynamicLists.CalcFactorTypeUOM;
                            vv.sortKey  = 1;
                            vv.criteria = compDockControl+","+$(this).val();

                            $.ajax({ 
                                url:  '/api/vLookupList/GetGlobalDynamicListFiltered/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                async:false,
                                data: { filter: JSON.stringify(vv) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) {
                                    $("#ddlWndUnitsOfMesure").data("kendoDropDownList").setDataSource(data.Data);
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                    }
                                                    else{
                                                        blnSuccess = true;
                                                        strValidationMsg = "No records were found matching your search criteria";
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Calculation Time Factor Failure"; }
                                            ngl.showErrMsg("Get Appointment Calculation Time Factor Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                               
                                }, 
                                error: function(result) { 
                                    options.error(result);
                                } 
                            }); 
                            $("#ddlWndUnitsOfMesure").data("kendoDropDownList").select(0);
                        })
              
                        $("#ddlWndUnitsOfMesure").kendoDropDownList({
                            dataSource:null,
                            dataTextField: "Description",
                            dataValueField: "Name",
                            autoWidth: true,
                            index:0,
                            filter: "contains",
                        });

                        $("#btnWndSaveApptCal").kendoButton({
                            click: function(e) {
                                var submit = true;
                                if ($("#txtWndName").data("kendoMaskedTextBox").value() =="") {
                                    $("#txtWndName-validation").removeClass("hide-display");
                                    submit = false;
                                }

                                if(submit == true){
                                    if(ACTFMObject.DockTCFControl == 0){
                                        var data = {};
                                        data.DockTCFName=$("#txtWndName").val();
                                        data.DockTCFDescription=$("#txtWndDescription").val();
                                        data.DockTCFAmount=$("#numWndAmount").val();
                                        data.DockTCFUOM=$("#ddlWndUnitsOfMesure").val();
                                        data.DockTCFTimeFactor=$("#numWndTimeFactor").val();
                                        data.DockTCFOn = $("#switchWndOnOff").data("kendoSwitch").check();
                                        data.DockTCFCalcFactorTypeControl=$("#ddlWndCalcType").val();
                                        data.DockTCFCompDockContol=compDockControl;

                                        $.ajax({
                                            async: false,
                                            type: "POST",
                                            url: "/api/AMSCompDockdoor/SaveDockTimeCalcFactor/",
                                            contentType: "application/json; charset=utf-8",
                                            dataType: 'json',
                                            data: JSON.stringify(data),
                                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                            success: function (data) {     
                                                try {
                                                    $("#ResourceAddEdit").data("kendoWindow").close();
                                                    $("#ACTFMGrid").data("kendoGrid").dataSource.read();
                                                    var blnSuccess = false;
                                                    var blnErrorShown = false;
                                                    var strValidationMsg = "";
                                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                            }
                                                        }
                                       
                                                    }
                                                    if (blnSuccess === false && blnErrorShown === false) {
                                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save Appointment calculation time faction Failure"; }
                                                        ngl.showErrMsg("Save Appointment calculation time faction Failure", strValidationMsg, null);
                                                    }

                                                } catch (err) {
                                                    ngl.showErrMsg(err.name, err.description, null);
                                                }
                                            },
                                            error: function (xhr, textStatus, error) {
                                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                                ngl.showErrMsg("Save Appointment calculation time faction Failure", sMsg, null);                        
                                            }
                                        });
                                    }
                                    else{
                                        var data = ACTFMObject;
                                        data.DockTCFName=$("#txtWndName").val();
                                        data.DockTCFDescription=$("#txtWndDescription").val();
                                        data.DockTCFAmount=$("#numWndAmount").val();
                                        data.DockTCFUOM=$("#ddlWndUnitsOfMesure").val();
                                        data.DockTCFTimeFactor=$("#numWndTimeFactor").val();
                                        data.DockTCFOn = $("#switchWndOnOff").data("kendoSwitch").check();
                                        data.DockTCFCalcFactorTypeControl=$("#ddlWndCalcType").val();
                                        data.DockTCFCompDockContol=compDockControl;

                                        $.ajax({
                                            async: false,
                                            type: "POST",
                                            url: "/api/AMSCompDockdoor/SaveDockTimeCalcFactor/",
                                            contentType: "application/json; charset=utf-8",
                                            dataType: 'json',
                                            data: JSON.stringify(data),
                                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                            success: function (data) {     
                                                try {
                                                    $("#ResourceAddEdit").data("kendoWindow").close();
                                                    $("#ACTFMGrid").data("kendoGrid").dataSource.read();
                                                    var blnSuccess = false;
                                                    var blnErrorShown = false;
                                                    var strValidationMsg = "";
                                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                            }
                                                        }
                                       
                                                    }
                                                    if (blnSuccess === false && blnErrorShown === false) {
                                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save Appointment calculation time faction Failure"; }
                                                        ngl.showErrMsg("Save Appointment calculation time faction Failure", strValidationMsg, null);
                                                    }

                                                } catch (err) {
                                                    ngl.showErrMsg(err.name, err.description, null);
                                                }
                                            },
                                            error: function (xhr, textStatus, error) {
                                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                                ngl.showErrMsg("Save Appointment calculation time faction Failure", sMsg, null);                        
                                            }
                                        });
                                    }
                                }
                       
                        
                            }
                        });

                        //*********2nd TAB*********//
                
                        //##### Overide Password Popup ######//
                        //LVV CHANGE
                        ////$("#btnOverridePassword").kendoButton();
                        $("#txtWndNewPassword").kendoMaskedTextBox();
                        $("#txtWndNewConPassword").kendoMaskedTextBox();
                        // ddl For DockDoors //
                        $("#ddlROSDockDoor").kendoDropDownList({
                            dataSource:dsResourcesData,
                            dataTextField: "CompDockDockDoorName",
                            dataValueField: "CompDockControl",
                            autoWidth: true,
                            filter: "contains",
                        });

                        //**** Selected data*****//
                        var RODockControl = $("#ddlROSDockDoor").val();
                        if(RODockControl > 0){
                            var sdata = $("#ddlROSDockDoor").data("kendoDropDownList").dataItem();
                            $("#lblSOPassResourceID").html(sdata.CompDockDockDoorID);
                            $("#lblSOPassResourceName").html(sdata.CompDockDockDoorName);
                            //LVV CHANGE
                            if (dom_SOPExists === true) { dom_btnChangeSOP.disabled = false; }
                        }else{
                            RODockControl =0;
                            //LVV CHANGE
                            if (dom_SOPExists === true) { dom_btnChangeSOP.disabled = true; }
                        }
                        //*****On Change event for DockDoor*****//
                        $("#ddlROSDockDoor").on("change warehouse", function () {
                            RODockControl =   $("#ddlROSDockDoor").val();
                            var sdata = $(this).data("kendoDropDownList").dataItem();
                            $("#lblSOPassResourceID").html(sdata.CompDockDockDoorID);
                            $("#lblSOPassResourceName").html(sdata.CompDockDockDoorName);  
                    
                            //LVV CHANGE
                            if (dom_SOPExists === true) { dom_btnChangeSOP.disabled = false; }

                            $("#ROSGrid").data("kendoGrid").dataSource.read();
                        });

                        dsResourceOverrides = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    //s.filterName = "CompControl";
                                    s.filterValue = RODockControl;

                                    $.ajax({ 
                                        url: '/api/AMSCompDockdoor/GetOverrideSettingsForDock/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get Resource Overrides Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Resource Overrides Failure"; }
                                                    ngl.showErrMsg("Get Resource Overrides Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },
                                update: function(options) {
                                    $.ajax({ 
                                        url: 'api/AMSCompDockdoor/UpdateOverrideSettingsForDock', 
                                        type: "POST",
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Save Resource Override Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Save Resource Override Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Resource Override Failure"; }
                                                    ngl.showErrMsg("Save Resource Override Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //refresh the grid
                                            $("#ROSGrid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Resource Override Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "DockSettingControl",
                                    fields: {
                                        DockSettingControl: { type: "number",editable: false },
                                        DockSettingName: { type: "string",editable: false },
                                        DockSettingDescription: { type: "string",editable: false },
                                        DockSettingRequireReasonCode: { type: "boolean" },
                                        DockSettingRequireSupervisorPwd: { type: "boolean" },
                                        DockSettingOn: { type: "boolean" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access Resource Override Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });

                        $("#ROSGrid").kendoGrid({
                            height:500,
                            noRecords: true,
                            dataSource: dsResourceOverrides,
                            editable: "inline",
                            columns: [
                               { command: [{ className: "cm-icononly-button", name: "edit", text:{edit: "", update: "", cancel: ""}, iconClass:{edit: "k-icon k-i-pencil", update: "k-icon k-i-check", cancel: "k-icon k-i-cancel"}}], title: "Action", width:120 },
                               { field: "DockSettingName", title: "Name" },
                               { field: "DockSettingDescription", title: "Description" },
                               { field: "DockSettingRequireReasonCode",  title: "Require Reason Code to Override", template: '<input type="checkbox" #= DockSettingRequireReasonCode ? "checked=checked" : "" # disabled="disabled" ></input>' },
                               { field: "DockSettingRequireSupervisorPwd",  title: "Require Supervisor Password to Override", template: '<input type="checkbox" #= DockSettingRequireSupervisorPwd ? "checked=checked" : "" # disabled="disabled" ></input>'  },
                               { field: "DockSettingOn", title: "On", editor:editDockSettingOn, template: '<input type="checkbox" #= DockSettingOn ? "checked=checked" : "" # disabled="disabled" ></input>', width:70 },
                            ],
                        });

                        function editDockSettingOn(container, options) {
                            $('<input data-bind="value:' + options.field + '"/>')
                                .appendTo(container)
                                .kendoSwitch();
                        }


                        $("#txtWndNewPassword").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#txtWndNewPassword-validation").addClass("hide-display");
                            }
                            else {
                                $("#txtWndNewPassword-validation").removeClass("hide-display");
                            }
                        });
                        $("#txtWndNewConPassword").on("change input", function () {
                            if ($(this).val() != "") {
                                $("#txtWndNewConPassword-validation").addClass("hide-display");
                            }
                            else {
                                $("#txtWndNewConPassword-validation").removeClass("hide-display");
                            }
                        });

                        $("#btnWndChangePassword").kendoButton({
                            click: function(e) {
                                var submit = true;
                                if ($("#txtWndNewPassword").data("kendoMaskedTextBox").value() =="") {
                                    $("#txtWndNewPassword-validation").removeClass("hide-display");
                                    submit = false;
                                }
                                if ($("#txtWndNewConPassword").data("kendoMaskedTextBox").value() =="") {
                                    $("#txtWndNewConPassword-validation").removeClass("hide-display");
                                    submit = false;
                                }
                                //if($("#txtWndNewPassword").val() != $("#txtWndNewConPassword").val()){
                                //    ngl.showWarningMsg("New Password and Confirm New Password should be Same","");
                                //    submit = false;
                                //}
                                var odata = new AMSAppointments;
                                odata.DockControl = RODockControl;
                                odata.newPwd = $("#txtWndNewPassword").val();
                                odata.confirmPwd = $("#txtWndNewConPassword").val();

                                if(submit == true){
                                    $.ajax({
                                        url: "api/AMSCompDockdoor/ChangeOverridesPwdForDock",
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: 'json',
                                        data: JSON.stringify(odata),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Change Supervisor Override Password Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Change Supervisor Override Password Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Change Supervisor Override Password Failure"; }
                                                    ngl.showErrMsg("Change Supervisor Override Password Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //close popup 
                                            WndSupOverridePassword.close();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Change Data Failure");
                                            ngl.showErrMsg("Change Supervisor Override Password Failure", sMsg, null);
                                        }
                                    });
                                }
                            }
                        });

                        //*********3rd TAB*********//
                        dsUserFields = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    //s.filterName = "CompControl";
                                    s.CompControlFrom = CompControl

                                    $.ajax({ 
                                        url: '/api/AMSCompUserField/GetCompUserFieldSettingRecords/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get UserFields Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get UserFields Failure"; }
                                                    ngl.showErrMsg("Get UserFields Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                        } 
                                    }); 
                                },
                                create: function(options) {
                                    options.data.CompAMSUserFieldSettingCompControl = CompControl;
                                    $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: "api/AMSCompUserField/AddUserField",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: 'json',
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            $("#UserFieldsGrid").data("kendoGrid").dataSource.read();
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save UserField Failure"; }
                                                    ngl.showErrMsg("Save UserField Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save UserField Failure", sMsg, null);                        
                                        }
                                    });
                                },
                                update: function(options) {
                                    $.ajax({ 
                                        url: 'api/AMSCompUserField/UpdateUserField', 
                                        type: "POST",
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Save UserField Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                                $("#UserFieldsGrid").data("kendoGrid").dataSource.read();
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save UserField Failure"; }
                                                    ngl.showErrMsg("Save UserField Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save UserField Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                destroy: function(options) {
                                    $.ajax({
                                        url: 'api/AMSCompUserField/DeleteUserField', 
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Delete UserField Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Delete UserField Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete UserField Failure"; }
                                                    ngl.showErrMsg("Delete UserField Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //refresh the grid
                                            $("#UserFieldsGrid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete UserField Failure");
                                            ngl.showErrMsg("Delete UserField Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "CompAMSUserFieldSettingControl",
                                    fields: {
                                        CompAMSUserFieldSettingControl: { type: "number" },
                                        CompAMSUserFieldSettingFieldName: { type: "string",
                                            validation: {
                                                required: {
                                                    message: "Name is required."
                                                }
                                            }
                                        },
                                        CompAMSUserFieldSettingFieldDesc: { type: "string" },
                                        CompAMSUserFieldSettingModDate: {type:"date",editable: false },
                                        CompAMSUserFieldSettingModUser: {type:"string",editable: false },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access UserFields Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });

                        $("#UserFieldsGrid").kendoGrid({
                            height:500,
                            noRecords: true,
                            dataSource: dsUserFields,
                            editable: "inline",
                            toolbar: [{name:"create",text:"Add User field"}],
                            columns: [
                               { command: [{ className: "cm-icononly-button", name: "edit", text:{edit: "", update: "", cancel: ""}},{ className: "cm-icononly-button", name: "delete", text:"", iconClass: "k-icon k-i-trash" }], title: "Action", width:120 },
                               { field: "CompAMSUserFieldSettingFieldName" ,title: "Name" },
                               { field: "CompAMSUserFieldSettingFieldDesc", title: "Map To" , editor: settingFieldMapDropDownEditor, template: 
                                     function(data){
                                         var idx1 = dsCompUserFieldMaps._data.map(
                                                 function(e) { 
                                                     return e.settingFieldMapDesc; 
                                                 }).indexOf(data.CompAMSUserFieldSettingFieldDesc); 
                                         if(idx1 != -1){
                                             return dsCompUserFieldMaps._data[idx1].settingFieldMapName
                                         }else{
                                             return "Custom"
                                         }
                                     }
                               },
                               { field: "CompAMSUserFieldSettingModDate", title: "Modified DateTime", format: "{0:M/d/yyyy hh:mm tt}"},
                               { field: "CompAMSUserFieldSettingModUser", title: "Modified By"},
                            ],
                            });

                        dsCompUserFieldMaps = new kendo.data.DataSource({
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();                               
                                    $.ajax({ 
                                        url: '/api/AMSCompUserField/GetCompUserFieldMaping/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Read User Field Mapping Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Read User Field Mapping Failure"; }
                                                    ngl.showErrMsg("Read User Field Mapping Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }                               
                                        }, 
                                        error: function(result) { 
                                            ngl.showErrMsg("Read User Field Mapping Failure", result, null); 
                                        } 
                                    }); 
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "settingFieldMapDesc",
                                    fields: {
                                        settingFieldMapDesc: { type: "string" },
                                        settingFieldMapName: { type: "string"}                             
                            
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Read User Field Mapping Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });
                        
                        dsCompUserFieldMaps.read();
                        function settingFieldMapDropDownEditor(container, options) {
                                $('<input required data-text-field="settingFieldMapName" data-value-field="settingFieldMapDesc" data-bind="value:' + options.field + '"/>')
                                .appendTo(container)
                                .kendoDropDownList({
                                    autoBind: false,
                                    dataTextField: "settingFieldMapName",
                                    dataValueField:  "settingFieldMapDesc",
                                    autoBind: true,
                                    dataSource:  dsCompUserFieldMaps,
                                    change: function (e) {
                                        // Note: this code updates the data but not the text in the grid
                                        //var MapDesc = this.dataItem(e.item);
                                        //var sCompAMSUserFieldSettingFieldName = options.model.CompAMSUserFieldSettingFieldName;
                                        //if (isEmpty(sCompAMSUserFieldSettingFieldName)){
                                        //    options.model.CompAMSUserFieldSettingFieldName =  MapDesc.settingFieldMapName;
                                        //    //options.model.set("CompAMSUserFieldSettingFieldName", MapDesc.settingFieldMapName);
                                        //}                                       
                                    }
                                });
                        }

                        //*********4th TAB*********//
                        dsApptColorStatus = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    $.ajax({ 
                                        url: '/api/vLookupList/GetStaticList/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        async: false,
                                        data: { id:nglStaticLists.ApptStatusColorCodeKey},
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get Appt Color Status Deatils Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appt Color Status Failure"; }
                                                    ngl.showErrMsg("Get Appt Color Status Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            ngl.showErrMsg("Get Appt Color Status Failure", result, null); 
                                        } 
                                    }); 
                                }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "Control",
                                    fields: {
                                        Control: { type: "number" },
                                        Name: { type: "string"},
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access Appt Color Status Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });
                        dsApptColorStatus.read();
                        dsApptStatusCol = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    s.CompControlFrom = CompControl
                                    s.filterName = 'colorType';
                                    s.filterValue = 0;

                                    $.ajax({ 
                                        url: '/api/AMSCompColorCode/GetCompColorCodeSettingRecords/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get Appt Status Color Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appt Status Color Failure"; }
                                                    ngl.showErrMsg("Get Appt Status Color Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                        } 
                                    }); 
                                },      
                                create: function(options) {
                                    options.data.CompAMSColorCodeSettingCompControl = CompControl; 
                                    options.data.CompAMSColorCodeSettingType = 0;
                                    var color=options.data.CompAMSColorCodeSettingColorCode;
                                    if(color.charAt(0)=='#'){
                                        options.data.CompAMSColorCodeSettingColorCode=options.data.CompAMSColorCodeSettingColorCode.substring(1, options.data.CompAMSColorCodeSettingColorCode.length);
                                    }
                                    $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: "/api/AMSCompColorCode/AddApptColorCodeSetting/",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: 'json',
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            $("#ApptStatusColorGrid").data("kendoGrid").dataSource.read();
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Appt Status Color Failure"; }
                                                    ngl.showErrMsg("Save Appt Status Color Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Appt Status Color Failure", sMsg, null);                        
                                        }
                                    });
                                },
                                update: function(options) {
                                    var color=options.data.CompAMSColorCodeSettingColorCode;
                                    if(color.charAt(0)=='#'){
                                        options.data.CompAMSColorCodeSettingColorCode=options.data.CompAMSColorCodeSettingColorCode.substring(1, options.data.CompAMSColorCodeSettingColorCode.length);
                                    } 
                                    $.ajax({ 
                                        url: '/api/AMSCompColorCode/UpdateApptColorCodeSetting/', 
                                        type: "POST",
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Save Appt Status Color Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                                $("#ApptStatusColorGrid").data("kendoGrid").dataSource.read();
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Appt Status Color Failure"; }
                                                    ngl.showErrMsg("Save Appt Status Color Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Appt Status Color Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                destroy: function(options) {
                                    $.ajax({
                                        url: '/api/AMSCompColorCode/DeleteApptColorCodeSetting/', 
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Delete Appt Status Color Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Delete Appt Status Color Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete Appt Status Color Failure"; }
                                                    ngl.showErrMsg("Delete Appt Status Color Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //refresh the grid
                                            $("#ApptStatusColorGrid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Appt Status Color Failure");
                                            ngl.showErrMsg("Delete Appt Status Color Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "CompAMSColorCodeSettingControl",
                                    fields: {
                                        CompAMSColorCodeSettingControl: { type: "number" },
                                        CompAMSColorCodeSettingFieldName: { type: "string",
                                            validation: {
                                                required: {
                                                    message: "Name is required."
                                                }
                                    
                                            }
                                        },
                                        CompAMSColorCodeSettingFieldDesc: { type: "string" },
                                        CompAMSColorCodeSettingColorCode: { type: "string" },
                                        CompAMSColorCodeSettingKey: { type: "number"},
                                        CompAMSColorCodeSettingModDate: {type:"date", editable: false },
                                        CompAMSColorCodeSettingModUser: {type:"string", editable: false }
                            
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access Appt Status Color Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });
                
                        $("#ApptStatusColorGrid").kendoGrid({
                            height:500,
                            noRecords: true,
                            dataSource: dsApptStatusCol,
                            editable: "inline",
                            toolbar: [{name:"create",text:"Add Appt Status Color"}],
                            columns: [
                               { command: [{ className: "cm-icononly-button", name: "edit", text:{edit: "", update: "", cancel: ""}},{ className: "cm-icononly-button", name: "delete", text:"", iconClass: "k-icon k-i-trash" }], title: "Action", width:120 },
                               { field:"CompAMSColorCodeSettingFieldName",title: "Name" },
                               { field:"CompAMSColorCodeSettingFieldDesc",title: "Description" },
                               {field: "CompAMSColorCodeSettingColorCode", title: "Color", width:65, editor: editApptStatusColor,
                                   template: function(dataItem) {
                                       return "<div style='width: 22px;margin-left:15px;background-color: #" + dataItem.CompAMSColorCodeSettingColorCode + ";'>&nbsp;</div>";
                                   }
                               },
                               { field:"CompAMSColorCodeSettingKey",title: "Status", editor:editApptColorStatusDropDown,template:function(data){var idx1 = dsApptColorStatus._data.map(function(e) { return e.Control; }).indexOf(data.CompAMSColorCodeSettingKey); if(idx1 != -1){return dsApptColorStatus._data[idx1].Name}else{return "None"}}},
                               { field:"CompAMSColorCodeSettingModDate",title: "Modified Date", format: "{0:M/d/yyyy HH:mm}" },
                               { field:"CompAMSColorCodeSettingModUser",title: "ModifiedBy" },
                            ],
                        });

                        function editApptStatusColor(container, options) {
                            if(options.model.CompAMSColorCodeSettingColorCode==""){
                                options.model.CompAMSColorCodeSettingColorCode = 'ffffff';
                            }
                            $("<input required type='color' style='margin-top:-19px; width:25px; margin-left:74px' data-bind='value:" + options.field + "' />")
                            .appendTo(container)
                            .kendoColorPicker({
                                buttons: false
                            });
                        }

                        function editApptColorStatusDropDown(container, options) {
                            if(options.model.CompAMSColorCodeSettingKey == 0){
                                $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                                    .appendTo(container)
                                    .kendoDropDownList({
                                        dataTextField: "Name",
                                        dataValueField: "Control",
                                        dataSource: dsApptColorStatus,
                                        index: 0,
                                        autoBind: true,
                                    });
                            }else{
                                $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                                    .appendTo(container)
                                    .kendoDropDownList({
                                        dataTextField: "Name",
                                        dataValueField: "Control",
                                        autoBind: true,
                                        dataSource: dsApptColorStatus,
                                    });
                            }
                        }

                        //*********5th TAB*********//               
                        dsOrderColorStatus = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    $.ajax({ 
                                        url: '/api/vLookupList/GetStaticList/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        async: false,
                                        data: { id:nglStaticLists.ApptTypeColorCodeKey},
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Order Color Status Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if(data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Order Color Status Failure"; }
                                                    ngl.showErrMsg("Get Order Color Status Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }                               
                                        }, 
                                        error: function(result) { ngl.showErrMsg("Get Order Color Status Failure", result, null); } 
                                    }); 
                                }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "Control",
                                    fields: {
                                        Control: { type: "number" },
                                        Name: { type: "string"},
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) { ngl.showErrMsg("Access Order Color Status Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });
                        dsOrderColorStatus.read();

                        dsApptOrderStatusCol = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    s.CompControlFrom = CompControl
                                    s.filterName = 'colorType';
                                    s.filterValue = 1;

                                    $.ajax({ 
                                        url: '/api/AMSCompColorCode/GetCompColorCodeSettingRecords/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get Appt Order Color Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appt Order Color Failure"; }
                                                    ngl.showErrMsg("Get Appt Order Color Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                               
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },      
                                create: function(options) {
                                    options.data.CompAMSColorCodeSettingCompControl = CompControl;
                                    options.data.CompAMSColorCodeSettingType = 1;
                                    var color=options.data.CompAMSColorCodeSettingColorCode;
                                    if(color.charAt(0)=='#'){
                                        options.data.CompAMSColorCodeSettingColorCode=options.data.CompAMSColorCodeSettingColorCode.substring(1, options.data.CompAMSColorCodeSettingColorCode.length);
                                    }
                                    $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: "/api/AMSCompColorCode/AddApptColorCodeSetting/",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: 'json',
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            $("#ApptOrderColorGrid").data("kendoGrid").dataSource.read();
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Appt Order Color Failure"; }
                                                    ngl.showErrMsg("Save Appt Order Color Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Appt Order Color Failure", sMsg, null);
                                        }
                                    });
                                },
                                update: function(options) {
                                    var color=options.data.CompAMSColorCodeSettingColorCode;
                                    if(color.charAt(0)=='#'){
                                        options.data.CompAMSColorCodeSettingColorCode=options.data.CompAMSColorCodeSettingColorCode.substring(1, options.data.CompAMSColorCodeSettingColorCode.length);
                                    } 
                                    $.ajax({ 
                                        url: '/api/AMSCompColorCode/UpdateApptColorCodeSetting/', 
                                        type: "POST",
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Save Appt Order Color Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                                $("#ApptOrderColorGrid").data("kendoGrid").dataSource.read();
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Appt Order Color Failure"; }
                                                    ngl.showErrMsg("Save Appt Order Color Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Appt Order Color Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                destroy: function(options) {
                                    $.ajax({
                                        url: '/api/AMSCompColorCode/DeleteApptColorCodeSetting/', 
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Delete Appt Order Color Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Delete Appt Order Color Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete Appt Order Color Failure"; }
                                                    ngl.showErrMsg("Delete Appt Order Color Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //refresh the grid
                                            $("#ApptOrderColorGrid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Appt Order Color Failure");
                                            ngl.showErrMsg("Delete Appt Order Color Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "CompAMSColorCodeSettingControl",
                                    fields: {
                                        CompAMSColorCodeSettingControl: { type: "number" },
                                        CompAMSColorCodeSettingFieldName: { type: "string", validation: { required: { message: "Name is required." } } },
                                        CompAMSColorCodeSettingFieldDesc: { type: "string" },
                                        CompAMSColorCodeSettingColorCode: { type: "string" },
                                        CompAMSColorCodeSettingKey: { type: "number"},
                                        CompAMSColorCodeSettingModDate: {type:"date", editable: false },
                                        CompAMSColorCodeSettingModUser: {type:"string", editable: false }                          
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) { ngl.showErrMsg("Access Appt Order Color Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#ApptOrderColorGrid").kendoGrid({
                            height:500,
                            noRecords: true,
                            dataSource: dsApptOrderStatusCol,
                            editable: "inline",
                            toolbar: [{name:"create",text:"Add Appt Order Color"}],
                            columns: [
                               { command: [{ className: "cm-icononly-button", name: "edit", text:{edit: "", update: "", cancel: ""}},{ className: "cm-icononly-button", name: "delete", text:"", iconClass: "k-icon k-i-trash" }], title: "Action", width:120 },
                               { field:"CompAMSColorCodeSettingFieldName",title: "Name" },
                               { field:"CompAMSColorCodeSettingFieldDesc",title: "Description" }, 
                               {field: "CompAMSColorCodeSettingColorCode", title: "Color", width:65, editor: editOrderStatusColor,
                                   template: function(dataItem) {
                                       return "<div style='width: 22px;margin-left:15px;background-color: #" + dataItem.CompAMSColorCodeSettingColorCode + ";'>&nbsp;</div>";}                       
                               },
                               { field:"CompAMSColorCodeSettingKey",title: "Status", editor:editOrderColorStatusDropDown, template:function(data){ var idx1 = dsOrderColorStatus._data.map(function(e) { return e.Control; }).indexOf(data.CompAMSColorCodeSettingKey); if(idx1!= -1){return dsOrderColorStatus._data[idx1].Name}else{return "None"}}},
                               { field:"CompAMSColorCodeSettingModDate",title: "Modified Date", format: "{0:M/d/yyyy HH:mm}" },
                               { field:"CompAMSColorCodeSettingModUser",title: "ModifiedBy" },
                            ],
                        });

                        function editOrderStatusColor (container, options) {
                            if(options.model.CompAMSColorCodeSettingColorCode==""){
                                options.model.CompAMSColorCodeSettingColorCode = 'ffffff';
                            }
                            $("<input required type='color' style='margin-top:-19px; width:25px; margin-left:74px' data-bind='value:" + options.field + "' />")
                            .appendTo(container)
                            .kendoColorPicker({
                                buttons: false
                            });
                        }

                        function editOrderColorStatusDropDown(container, options) {
                            if(options.model.CompAMSColorCodeSettingKey == 0){
                                $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                                .appendTo(container)
                                .kendoDropDownList({
                                    dataTextField: "Name",
                                    dataValueField: "Control",
                                    dataSource: dsOrderColorStatus,
                                    index: 0,
                                    autoBind: true,                        
                                });
                            }else{
                                $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                                .appendTo(container)
                                .kendoDropDownList({
                                    dataTextField: "Name",
                                    dataValueField: "Control",
                                    autoBind: true,
                                    dataSource: dsOrderColorStatus,
                                });
                            }
                        }

                        //*********6th TAB*********//             
                        dsApptDetails = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    //s.filterName = "CompControl";
                                    s.filterValue = CompControl;

                                    $.ajax({ 
                                        url: '/api/AMSCompDockdoor/GetApptDetailFieldsForComp/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            bitpositionApptFieldsArray=[];
                                            for(var i in data.Data)
                                            {
                                                if(data.Data[i].PTOn==true){
                                                    bitpositionApptFieldsArray.push(data.Data[i].PTBitPos);
                                                }
                                            }
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get ApptDetailFieldsForComp Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get ApptDetailFieldsForComp Failure"; }
                                                    ngl.showErrMsg("Get ApptDetailFieldsForComp Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                
                                        } 
                                    }); 
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "PTBitPos",
                                    fields: {
                                        PTBitPos: { type: "number",editable: false },
                                        PTCaption: { type: "string",editable: false },
                                        PTOn: { type: "boolean" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access ApptDetailFieldsForComp Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });

                        $("#ApptDPHFGrid").kendoGrid({
                            height:500,
                            noRecords: true,
                            dataSource: dsApptDetails,
                            editable: "inline",
                            columns: [
                               {template:'<input type="checkbox" #= PTOn ? \'checked="checked"\' : "" # class="chkbxApptFields" />', width:150 },
                               { field: "PTCaption", title:"Description"},
                            ],
                        });

                        $("#ApptDPHFGrid .k-grid-content").on("change", "input.chkbxApptFields", function(e) {
                            var grid = $("#ApptDPHFGrid").data("kendoGrid"),
                            dataItem = grid.dataItem($(e.target).closest("tr"));
                            dataItem.set("PTOn", this.checked);
                            if(isInArray(dataItem.PTBitPos,bitpositionApptFieldsArray) ){
                                if(this.checked==false){
                                    bitpositionApptFieldsArray.splice($.inArray(dataItem.PTBitPos, bitpositionApptFieldsArray),1);
                                }
                            }else{
                                if(this.checked==true){
                                    bitpositionApptFieldsArray.push(dataItem.PTBitPos);
                                }
                            }
                        });

                        $("#btnSaveApptDetailFieldForComp").kendoButton({
                            click:function(){
                                var csv="";

                                jQuery.each( bitpositionApptFieldsArray, function( i, val ) {
                                    if(csv==""){
                                        csv=csv+val;
                                    }else{
                                        csv=csv+','+val;
                                    }
                                });

                                var s= new AllFilter();
                                s.filterValue=CompControl;
                                s.data=csv;
                                $.ajax({ 
                                    url: 'api/AMSCompDockdoor/SaveApptDetailFieldForComp', 
                                    type: "POST",
                                    //contentType: 'application/json; charset=utf-8', 
                                    dataType: 'json', 
                                    data: {"":JSON.stringify(s)},
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                    success: function (data) {
                                        try {
                                            var blnSuccess = false;
                                            var blnErrorShown = false;
                                            var strValidationMsg = "";
                                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                    blnErrorShown = true;
                                                    ngl.showErrMsg("Save ApptDetailFieldForComp Failure", data.Errors, null);
                                                }
                                                else {
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            if (data.Data[0] == false) {
                                                                ngl.showWarningMsg("Save ApptDetailFieldForComp Failure!", "", null);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (blnSuccess === false && blnErrorShown === false) {
                                                if (strValidationMsg.length < 1) { strValidationMsg = "Save ApptDetailFieldForComp Failure"; }
                                                ngl.showErrMsg("Save ApptDetailFieldForComp Failure", strValidationMsg, null);
                                            }
                                        } catch (err) {
                                            ngl.showErrMsg(err.name, err.description, null);
                                        }
                                        //refresh the grid
                                        $("#ApptDPHFGrid").data("kendoGrid").dataSource.read();
                                    },
                                    error: function (xhr, textStatus, error) {
                                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                        ngl.showErrMsg("Save ApptDetailFieldForComp Failure", sMsg, null); 
                                    } 
                                });
                            }
                        })

                        //*********7th TAB*********//
                        dsTrackingField = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            transport: { 
                                read: function(options) { 
                                    var s = new AllFilter();
                                    s.CompControlFrom = CompControl;

                                    $.ajax({ 
                                        url: '/api/AMSCompApptTrackingField/GetCompApptTrackingFieldSettingRecords/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Get UserField Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                            }
                                                            else{
                                                                blnSuccess = true;
                                                                strValidationMsg = "No records were found matching your search criteria";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get UserField Failure"; }
                                                    ngl.showErrMsg("Get UserField Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        }, 
                                        error: function(result) { 
                                            options.error(result);
                                        } 
                                    }); 
                                },      
                                create: function(options) {
                                    options.data.CompAMSApptTrackingSettingCompControl = CompControl;
                                    $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: "/api/AMSCompApptTrackingField/AddApptTrackingField/",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: 'json',
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            $("#trakingFieldGrid").data("kendoGrid").dataSource.read();
                                                        }
                                                    }
                                       
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Tracking Fields Failure"; }
                                                    ngl.showErrMsg("Save Tracking Fields Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Tracking Fields Failure", sMsg, null);
                                        }
                                    });
                                },
                                update: function(options) {
                                    $.ajax({ 
                                        url: '/api/AMSCompApptTrackingField/UpdateApptTrackingField/', 
                                        type: "POST",
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {     
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Save Tracking Field Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                                $("#trakingFieldGrid").data("kendoGrid").dataSource.read();
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Tracking Field Failure"; }
                                                    ngl.showErrMsg("Save Tracking Field Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                            ngl.showErrMsg("Save Tracking Field Failure", sMsg, null); 
                                        } 
                                    });
                                },   
                                destroy: function(options) {
                                    $.ajax({
                                        url: '/api/AMSCompApptTrackingField/DeleteApptTrackingField/', 
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                        blnErrorShown = true;
                                                        ngl.showErrMsg("Delete Tracking Field Failure", data.Errors, null);
                                                    }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) {
                                                                    ngl.showWarningMsg("Delete Tracking Field Failure!", "", null);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete Tracking Field Failure"; }
                                                    ngl.showErrMsg("Delete Tracking Field Failure", strValidationMsg, null);
                                                }
                                            } catch (err) {
                                                ngl.showErrMsg(err.name, err.description, null);
                                            }
                                            //refresh the grid
                                            $("#trakingFieldGrid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) {
                                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Tracking Field Failure");
                                            ngl.showErrMsg("Delete Tracking Field Failure", sMsg, null); 
                                        } 
                                    });
                                },
                                parameterMap: function(options, operation) { return options; } 
                            },  
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "CompAMSApptTrackingSettingControl",
                                    fields: {
                                        CompAMSUserFieldSettingControl: { type: "number" },
                                        CompAMSApptTrackingSettingName: { type: "string",
                                            validation: {
                                                required: {
                                                    message: "Name is required."
                                                }
                                    
                                            }
                                        },
                                        CompAMSApptTrackingSettingDesc: { type: "string" },
                                        CompAMSApptTrackingSettingModDate: {type:"date",editable: false },
                                        CompAMSApptTrackingSettingModUser: {type:"string",editable: false }
                            
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function(xhr, textStatus, error) {
                                ngl.showErrMsg("Access Tracking Fields Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                            }
                        });

                        $("#trakingFieldGrid").kendoGrid({
                            height:500,
                            noRecords: true,
                            dataSource: dsTrackingField,
                            editable: "inline",
                            toolbar: ["create"],
                            columns: [
                               { command: [{ className: "cm-icononly-button", name: "edit", text:{edit: "", update: "", cancel: ""}},{ className: "cm-icononly-button", name: "delete", text:"", iconClass: "k-icon k-i-trash" }], title: "Action", width:120 },
                               { field: "CompAMSApptTrackingSettingName",title: "Name"},
                               { field: "CompAMSApptTrackingSettingDesc",title: "Description" },
                               { field: "CompAMSApptTrackingSettingModDate", title:"Modified Date", format: "{0:M/d/yyyy hh:mm tt}" },
                               { field: "CompAMSApptTrackingSettingModUser",title:"Modified By"},
                            ],
                        });

                        //*********8th TAB*********//
                        $("#ddlTimeZone").kendoDropDownList();
                        $("#btnTimeZone").kendoButton();

                        //********Some Defalts States*************//
                        //LVV CHANGE
                        if (dom_btnCopyDockSetExists === true) { dom_btnCopyDockSet.disabled = true; } 

                        $("#tempSetGrid .k-grid-edittemptype").addClass('k-state-disabled').removeClass("k-grid-edittemptype");
                        $("#pkgTypGrid .k-grid-editpackagetype").addClass('k-state-disabled').removeClass("k-grid-editpackagetype");
                        $("#ACTFMGrid .k-grid-addactfm").addClass('k-state-disabled').removeClass("k-grid-addactfm");

                        WndSupOverridePassword = $("#SupOverridePassword").kendoWindow({                           
                            height: 'auto',
                            width: 'auto'
                        }).data("kendoWindow");

                     
                        wndAddResourceDockDoor = $("#addResourceDockDoor").kendoWindow({
                            height: 'auto',
                            width: 'auto'
                        }).data("kendoWindow");

                wndCopyResourceSettings = $("#CopyResourceSettings").kendoWindow({
                    height: 'auto',
                    width: 'auto'
                }).data("kendoWindow");

                wndAddEditResource = $("#ResourceAddEdit").kendoWindow({
                    height: 'auto',
                    width: 'auto'
                }).data("kendoWindow");

                        //kendoWindow.height = '75%';
                        //kendoWindow.width = '650';
                wndApptBlockPeriods = $("#ApptBlockPeriods").kendoWindow({
                    height: '75%',
                    width: '650px'
                }).data("kendoWindow");

                        //kendoWindow.height = 'auto';
                        //kendoWindow.width = '450';
                wndpkgTypGrid = $("#PackageTypeAddEdit").kendoWindow({
                    height: 'auto',
                    width: '450px'
                }).data("kendoWindow");

                        //kendoWindow.height = 'auto';
                        //kendoWindow.width = '450';
                wndtempTypGrid = $("#TempTypeAddEdit").kendoWindow({
                    height: 'auto',
                    width: '450px'
                }).data("kendoWindow");
                        

                        $("#LockOutGrid").kendoNGLGrid({
                            dataSource: {
                                pageSize: 10,
                                transport: {                            
                                    read: function(options) { 
                                        var s = new  AllFilter();
                                        $.ajax({ 
                                            url: 'api/ManageScheduleLockout/GetAllRecords', 
                                            contentType: 'application/json; charset=utf-8', 
                                            dataType: 'json', 
                                            data: { filter: JSON.stringify(s) }, 
                                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                            success: function(data) { 
                                                options.success(data); 
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data) && typeof (data.Errors) !== 'undefined' &&  data.Errors != null) { ngl.showErrMsg('Access Denied', data.Errors, null); }
                                            }, 
                                            error: function(result) { options.error(result); } 
                                        }); 
                                    },
                                },
                                schema: {
                                    data: "Data",
                                    total: "Count",
                                    model: {
                                        id: "USC",
                                        fields: {
                                            UserName: { type: "string" },
                                            DockControl: { type: "number" },
                                            DockName: { type: "string" },
                                            FailedAttempts: { type: "number" }
                                        }
                                    }
                                }
                            },
                            pageable: true,
                            scrollable: false,
                            sortable: true,
                            dataBound: function(e) { 
                                var tObj = this; 
                                if (typeof (LockOutGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(LockOutGridDataBoundCallBack)) { LockOutGridDataBoundCallBack(e,tObj); } 
                            },
                            columns: [
                                { command: { className: "cm-icononly-button", name: "ResetFailedAttempts", text: "", iconClass: "k-icon k-i-unlock", click: UnlockAccount }, title: "Action" },
                                {field: "USC", title: "USC", hidden: true },
                                {field: "DockControl", title: "DockControl", hidden: true },                             
                                {field: "UserName", title: "User", template: "<span title='${UserName}'>${UserName}</span>"},
                                {field: "CompName", title: "Warehouse", template: "<span title='${CompName}'>${CompName}</span>"},
                                {field: "DockName", title: "Dock", template: "<span title='${DockName}'>${DockName}</span>"},
                                {field: "FailedAttempts", title: "Failed Attempts"}
                            ]
                        }); //LVV 2/12

                        ////////////wndAddVisibleComp///////////////////
                        wndMngLockOutWnd = $("#wndMngLockOutWnd").kendoWindow({
                            title: "Reset Account Lockout",
                            modal: true,
                            visible: false,   
                            //height: '80%',
                            //minWidth: 300,
                            actions: ["Minimize", "Maximize", "Close"],
                        }).data("kendoWindow"); //LVV 2/12

                //  //add code above to load screen specific information this is only visible when a user is authenticated
                  }, 10,this);
                }
                setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>}, 10,this);
                setTimeout(function () {   
                    menuTreeHighlightPage(); //must be called after PageReadyJS
                    var divWait = $("#h1Wait");
                    if (typeof (divWait) !== 'undefined') { divWait.hide(); }
                }, 10, this);

            });

        </script>

        <style>
                                  
            .hide-display { display: none; }

            .k-grid-header .k-header {
                overflow: visible !important;
                white-space: normal !important;
            }

            .divMenuWarehouse { margin: 10px; }

            .tabLable { margin: 10px; margin-left: 0; }

            .ui-container {
                margin:1%;
                height: 100% !important;
                width: 100% !important;
                margin-top: 2px !important;
            }

            .ui-vertical-container {
                height: 98% !important;
                width: 98% !important;
            }

            .ui-horizontal-container {
                height: 100% !important;
                width: 100% !important;
            }

            .ui-span-container {
                font-size: small;
                font-weight: bold;
                color: red;
                position: relative;
                bottom: 5px;
            }
       
            /*h2 { margin: 0; font-size: 1em; }*/

            table { border-collapse: collapse; }

            table, th, td { border: 0.3px solid rgba(0%,0%,0%,.7); color: black; } 

            .ui-th-margin { width: 125px; }

            .ui-td-margin { width: 200px; }

            .ui-td-margin2 { width: 240px; }

            .ui-th-marginExtra { width: 150px; }

            .inputNumeric { width: 90px; }

            .BtnWidth { width: 100px; }

            .MTop { margin-top: 15px; }

            .MBottom { margin-bottom: 15px; }

            .MLeft { margin-left: 5px; }

            .RDdiv { margin: 8px; }

            .MLeftRD { margin-left: 70px; }

            .RDdiv .ui-th-margin{ width:80px !important; }

            .RDdiv .ui-td-margin{
               padding-left:1px;
               width:50%;
               min-width:50px;
            }

            #ApptBlockPeripods .ui-th-margin { width: 75px; }

            #ApptBlockPeripods .ui-td-margin { width: 175px; }

            #ApptSettings td { width: 5% !important; }

            .ApptTimeTable > table > tr > td { width: 25% !important; }

            .ApptTimeTable { margin: 0 auto; width: 70% !important; }

            #ApptSettings .ui-td-margin { width: 100% !important; }

            #btnFindMyAppt { margin-left: 3px; }

            fieldset { border-color: #BBDCEB; }

            .k-colorpicker { vertical-align: initial !important; margin: 20px 0; }

            #ResourceEdit-UI > div { float: left; margin: 5px; }

            #ResourceEdit-UI > div > div:first-child { padding-bottom: 10px; }

             /*Added on 24/07/2018 */
            #ApptBlockOutUI .ui-th-margin { min-width:40px; max-width:40px; }

            #ApptBlockOutUI .ui-td-margin {
               padding-left:1px;
               width:100%;
               min-width:100px;
               max-width:200px;
            }

        /*-----------------------------------------------------------*/

            .k-grid tbody .k-button {
                min-width: 18px;
                width: 32px;
            }

            .k-grid tbody tr td { vertical-align: initial !important; }

            .k-grid-apptblockout { min-width: 140px !important; width: 140px !important; }

            .tblResponsive .tblResponsive-top { vertical-align: initial !important; }

        </style>

    </div>
</body>

</html>
