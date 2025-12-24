<%@ Page Title="Settings Page" Language="C#" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="DynamicsTMS365.Settings" %>

<!DOCTYPE html>

<html>
<head>
    <title>DTMS Settings</title>
    <%=cssReference%>
    <style>
        html,
        body {
            height: 100%;
            margin: 0;
            padding: 0;
        }

        html {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
            overflow: hidden;
        }
     
         .subscription-section label {
        margin-bottom: 5px;
        font-weight: bold;
        display: inline-block;
    }

    #Unsubscribed {
        width: 47%;
    }

    #subscriptionList{
        display: grid;
        grid-template-columns: 1fr;
        gap: 20px;
    }
    #subscriptionList .subscription-section {
        max-width: none;
        width: 100%;
    }

    #subscriptionList .k-listbox {
        width:  45%;
        height: 350px;
    }

    #subscriptionList .k-listbox:first-of-type {
        width: 54%;
        margin-right: 1px;
    }
    .subscriptionPane{
            background-color: aliceblue;
    padding: 3em;
    border: 1px solid #003f59;
    border-radius: 5px;
    }
    .k-listbox-toolbar .k-button:first-child{
        margin-top:0.5em;
    }
    .k-listbox-toolbar .k-button{
        width: calc(7.428571em + 10px) !important;
        height: calc(4em + 10px) !important;
        padding: 4px;
    }
    .templateList{
        padding: 2px;
    margin: 5px;
    }
    #subscriptionList .k-list .k-item{
         margin: 5px;
        border: 1px solid #003f59;
    } 

    </style>
</head>
<body>
    
    <%=jssplitter2Scripts%>
    <%=sWaitMessage%>
     <script type="text/kendo-x-tmpl" id="templateUnsubscribedList">

    <div class="templateList">
       <p> <span style="float:left"> #:ProcedureName#  #:ProcAlertUserXrefSendEmail#   #:ProcAlertUserXrefShowPopup#  </span >  &nbsp;  <span  style="float:right" > 
         <label>  Show Email 
         
         #  if (ProcAlertUserXrefSendEmail == 1 ) { # 
         <input checked onclick="SubscriptionAlertChange('UnsubscribedAlertList','#:ProcedureControl#')" type="checkbox" id="EmailAlert#:ProcedureControl#" name="EmailAlert" />
         <img id="EmailAlertGreenFlag#:ProcedureControl#" src="../Content/NGL/StatusFlagGreen16.png" alt="red flag"> #
            } 
            else if (ProcAlertUserXrefSendEmail == 0){ # 
         <input #if(ProcAlertUserXrefSendEmail==0 || ProcAlertUserXrefSendEmail==true){'checked=checked'}else{''}# onclick="SubscriptionAlertChange('UnsubscribedAlertList','#:ProcedureControl#')" type="checkbox" id="EmailAlert#:ProcedureControl#" name="EmailAlert" />
             <img id="EmailAlertRedFlag#:ProcedureControl#" src="../Content/NGL/StatusFlagRed16.png" alt="red flag"> 
            #} #
        

          </label>
          <label>  Show Popup 
           #  if (ProcAlertUserXrefShowPopup ==  1 ) { # 
         <input checked type="checkbox" onclick="SubscriptionAlertChange('UnsubscribedAlertList','#:ProcedureControl#')"  id="PopupAlert#:ProcedureControl#" onchange name="PopupAlert" /> 
         <img id="PopupAlertGreenFlag#:ProcedureControl#" src="../Content/NGL/StatusFlagGreen16.png" alt="red flag"> #
            } 
            else if (ProcAlertUserXrefShowPopup == 0){ # 
         <input  type="checkbox" onclick="SubscriptionAlertChange('UnsubscribedAlertList','#:ProcedureControl#')"  id="PopupAlert#:ProcedureControl#" onchange name="PopupAlert" /> 
             <img id="PopupAlertRedFlag#:ProcedureControl#" src="../Content/NGL/StatusFlagRed16.png" alt="red flag">
            #} #
         </label> </span> </p>

       <p>  #:ProcedureDescription# 
      </p>
    </div>
 </script>
     <script type="text/kendo-x-tmpl" id="templateSubscribedList">

    <div class="templateList">
       <p> <span style="float:left"> #:ProcedureName#  #:ProcAlertUserXrefSendEmail#   #:ProcAlertUserXrefShowPopup#   </span >  &nbsp;  <span  style="float:right" > 
         <label>  Show Email  
         #  if (ProcAlertUserXrefSendEmail == 1 ) { # 
         <input checked onclick="SubscriptionAlertChange('SubscribedAlertList', '#:ProcedureControl#')" type="checkbox" id="EmailAlert#:ProcedureControl#" name="EmailAlert" />
         <img id="EmailAlertGreenFlag#:ProcedureControl#" src="../Content/NGL/StatusFlagGreen16.png" alt="red flag"> #
            } 
            else if (ProcAlertUserXrefSendEmail == 0){ # 
         <input  onclick="SubscriptionAlertChange('SubscribedAlertList', '#:ProcedureControl#')" type="checkbox" id="EmailAlert#:ProcedureControl#" name="EmailAlert" />
             <img id="EmailAlertRedFlag#:ProcedureControl#" src="../Content/NGL/StatusFlagRed16.png" alt="red flag"> 
            #} #
        

          </label>
          <label> Show Popup 
           #  if (ProcAlertUserXrefShowPopup ==  1 ) { # 
          <input checked type="checkbox" onclick="SubscriptionAlertChange('SubscribedAlertList','#:ProcedureControl#')"  id="PopupAlert#:ProcedureControl#" onchange name="PopupAlert" /> 
         <img id="PopupAlertGreenFlag#:ProcedureControl#" src="../Content/NGL/StatusFlagGreen16.png" alt="red flag"> #
            } 
            else if (ProcAlertUserXrefShowPopup == 0){ # 
         <input  type="checkbox" onclick="SubscriptionAlertChange('SubscribedAlertList','#:ProcedureControl#')"  id="PopupAlert#:ProcedureControl#" onchange name="PopupAlert" /> 
             <img id="PopupAlertRedFlag#:ProcedureControl#" src="../Content/NGL/StatusFlagRed16.png" alt="red flag">
            #} #
         </label> </span> </p>

       <p>  #:ProcedureDescription# 
      </p>
    </div>
 </script>


    <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
        <div id="vertical" style="height: 98%; width: 98%;">
            <div id="menu-pane" style="height: 100%; width: 100%;">
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

                        <div id="pageContent" class="pane-content">
                            <h1 style="padding-left: 10px;">Manage My Account</h1>
                            <div class="fast-tab">
                                <span id="ExpandInfoSpan" style="display: none;"><a onclick='expandInfo();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                <span id="CollapseInfoSpan" style="display: normal;"><a onclick='collapseInfo();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                <span style="font-size: small; font-weight: bold">Change Account Info</span>&nbsp;&nbsp;<br />
                                <div id="InfoDiv" style="padding-bottom: 10px;">
                                    <div style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                        <div style="width: 95%; height: 95%;">
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Email</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtEmail" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Friendly Name</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtFriendlyName" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">First Name</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtFirstName" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Last Name</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtLastName" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>
                                        <div style="width: 95%; height: 95%;">
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Title</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtTitle" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Department</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtDepartment" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Cell Phone</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtCellPhone" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Home Phone</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtHomePhone" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>
                                        <div style="width: 95%; height: 95%;">
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Work Phone</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtWorkPhone" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Work Phone Ext</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="txtWorkPhoneExt" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div style="float: left;">
                                            <table class="tblResponsive">
                                                <tr>
                                                    <td class="tblResponsive-top">Culture Info</td>
                                                </tr>
                                                <tr>
                                                    <td class="tblResponsive-top">
                                                        <input id="cultureDropdown" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="float: left;">
                                            <table class="tblResponsive">
                                                <tr>
                                                    <td class="tblResponsive-top">Time Zone</td>
                                                </tr>
                                                <tr>
                                                    <td class="tblResponsive-top">
                                                        <input id="timeZoneDropdown" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>
                                        <div style="width: 95%; height: 95%;">
                                            <div style="float: left;">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top">Choose Theme</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <input id="ddThemes" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblResponsive-top">
                                                            <button id="btnSaveInfo" type="button" style="width: 150px;">Save</button></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>                                        
                                        <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>
                                    </div>
                                </div>
                            </div>
                            <div id="PasswordFastTab" class="fast-tab">
                                <span id="ExpandPasswordSpan" style="display: none;"><a onclick='expandPassword();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                <span id="CollapsePasswordSpan" style="display: normal;"><a onclick='collapsePassword();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                <span style="font-size: small; font-weight: bold">Change Password</span>&nbsp;&nbsp;<br />
                                <div id="PasswordDiv" style="padding-bottom: 10px;">
                                    <div style="padding: 0px 0px 0px 10px;">
                                        <p>To reset your password: Confirm your current password, then enter a new password below and click the "Change Password" button</p>
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
                                                <td style="border: none;"></td>
                                                <td style="border: none;">
                                                    <button id="btnChangePass" onclick="btnChangePass_Click();" type="button" style="width: 150px;">Change Password</button></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="MenuTab" class="fast-tab">
                                <span id="ExpandMenuSpan" style="display: none;"><a onclick='expandMenu();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                <span id="CollapseMenuSpan" style="display: normal;"><a onclick='collapseMenu();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                <span style="font-size: small; font-weight: bold">Change Menu</span>&nbsp;&nbsp;<br />
                                <div id="MenuDiv" style="padding: 10px;">
                                    <button id="btnMenuSettings" onclick="menuSettings();" type="button" style="width: 150px;">Edit Menu</button>
                                    <button id="btnMenuSettingsHide" onclick="menuSettingsHide()" type="button" style="width: 150px; display: none">Stop menu editing</button>
                                    <button id="btnMenuReset" onclick="menuReset();" type="button" style="width: 150px;">Reset Menu</button>
                                </div>
                            </div>

                           <div id="SubscriptionTab" class="fast-tab">
                                <span id="ExpandSubscriptionSpan" style="display: none;">
                                    <a onclick='expandSubscription();'>
                                        <span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span>
                                    </a>
                                </span>
                                <span id="CollapseSubscriptionSpan" style="display: normal;">
                                    <a onclick='collapseSubscription();'>
                                        <span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span>
                                    </a>
                                </span>
                                <span style="font-size: small; font-weight: bold">Manage Notification Sunscriptions </span>&nbsp;&nbsp;<br />
                                <div id="SubscriptionDiv" style="padding: 10px;">
                                  <div id="subscriptionList" class="subscription-container" role="application">
                                    <div class="subscription-section wide">
                                        <div class="subscriptionPane">
                                            <label for="optional" id="Unsubscribed">Unsubscribed Notifications (<span id="UnsubscribedCount"> </span> )</label>
                                            <label for="selected">Subscribed Notifications (<span id="SubscribedCount"> </span>)</label>
                                            <br />
                                             <select id="UnsubscribedAlertList"></select>
                                            <select id="SubscribedAlertList"></select>
                                        </div>
                                    </div>
                                      </div>
                                </div>
                            </div>

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
            //modified by RHR for v-8.5.4.006 on 05/17/2024 
            var sTimezone = "Central Standard Time";
            var sCulture = "en-US";

        <% Response.Write(NGLOAuth2); %>



        <% Response.Write(PageCustomJS); %>


            function execActionClick(btn, proc) {

            }

            function expandInfo() { $("#InfoDiv").show(); $("#ExpandInfoSpan").hide(); $("#CollapseInfoSpan").show(); }
            function collapseInfo() { $("#InfoDiv").hide(); $("#ExpandInfoSpan").show(); $("#CollapseInfoSpan").hide(); }

            function expandPassword() { $("#PasswordDiv").show(); $("#ExpandPasswordSpan").hide(); $("#CollapsePasswordSpan").show(); }
            function collapsePassword() { $("#PasswordDiv").hide(); $("#ExpandPasswordSpan").show(); $("#CollapsePasswordSpan").hide(); }

            function expandMenu() { $("#MenuDiv").show(); $("#ExpandMenuSpan").hide(); $("#CollapseMenuSpan").show(); }
            function collapseMenu() { $("#MenuDiv").hide(); $("#ExpandMenuSpan").show(); $("#CollapseMenuSpan").hide(); }

            function expandSubscription() {
                $("#SubscriptionDiv").show();
                $("#ExpandSubscriptionSpan").hide();
                $("#CollapseSubscriptionSpan").show();
            }
            function collapseSubscription() {
                $("#SubscriptionDiv").hide();
                $("#ExpandSubscriptionSpan").show();
                $("#CollapseSubscriptionSpan").hide();
            }

            var resGetUserSettings = function (data) {
                $("#txtEmail").data("kendoMaskedTextBox").value(data.UserEmail);
                $("#txtFriendlyName").data("kendoMaskedTextBox").value(data.UserFriendlyName);
                $("#txtFirstName").data("kendoMaskedTextBox").value(data.UserFirstName);
                $("#txtLastName").data("kendoMaskedTextBox").value(data.UserLastName);
                $("#txtTitle").data("kendoMaskedTextBox").value(data.UserTitle);
                $("#txtDepartment").data("kendoMaskedTextBox").value(data.UserDepartment);
                $("#txtCellPhone").data("kendoMaskedTextBox").value(data.UserPhoneCell);
                $("#txtHomePhone").data("kendoMaskedTextBox").value(data.UserPhoneHome);
                $("#txtWorkPhone").data("kendoMaskedTextBox").value(data.UserPhoneWork);
                $("#txtWorkPhoneExt").data("kendoMaskedTextBox").value(data.UserPhoneWorkExt);
                var dropdownlist = $("#ddThemes").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) { return dataItem.value === data.UserTheme365; });
                sTimezone = data.UserTimeZone;
                sCulture = data.UserCultureInfo; 

                //var oTimezone = $("#timeZoneDropdown").data("kendoComboBox");
                //oTimezone.select(function (dataItem) { return dataItem.value === sTimezone; });
                $("#timeZoneDropdown").data("kendoComboBox").value(data.UserTimeZone);
                //console.log('time z');
                //console.log(sTimezone);
                //console.log($("#timeZoneDropdown").data("kendoComboBox").value())

                //var oculture = $("#cultureDropdown").data("kendoComboBox");
                //oculture.select(function (dataItem) { return dataItem.value === sTimezone; });
                $("#cultureDropdown").data("kendoComboBox").value(data.UserCultureInfo);
                //console.log('culture');
                //console.log(sCulture);
                //console.log($("#cultureDropdown").data("kendoComboBox").value())


            }

            function getUserSettings(resultFunc) {
                var urls = "api/Users/GetUserSettings";
                $.ajax({
                    async: false,
                    type: "GET",
                    url: urls,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get User Settings Failure", data.Errors, null); }
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
                                if (strValidationMsg.length < 1) { strValidationMsg = "User Settings not found"; }
                                ngl.showErrMsg("Get User Settings Failure", strValidationMsg, null);
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"); ngl.showErrMsg("Get User Settings Failure", sMsg, null); }
                });
            }

            function saveUserSettings(resultFunc) {
                var u = new User();
                u.UserSecurityControl = 0;
                u.UserEmail = $("#txtEmail").data("kendoMaskedTextBox").value();
                u.UserName = "";
                u.UserFriendlyName = $("#txtFriendlyName").data("kendoMaskedTextBox").value();
                u.UserFirstName = $("#txtFirstName").data("kendoMaskedTextBox").value();
                u.UserLastName = $("#txtLastName").data("kendoMaskedTextBox").value();
                u.UserTitle = $("#txtTitle").data("kendoMaskedTextBox").value();
                u.UserDepartment = $("#txtDepartment").data("kendoMaskedTextBox").value();
                u.UserPhoneCell = $("#txtCellPhone").data("kendoMaskedTextBox").value();
                u.UserPhoneHome = $("#txtHomePhone").data("kendoMaskedTextBox").value();
                u.UserPhoneWork = $("#txtWorkPhone").data("kendoMaskedTextBox").value();
                u.UserPhoneWorkExt = $("#txtWorkPhoneExt").data("kendoMaskedTextBox").value();
                u.UserTheme365 = $("#ddThemes").data("kendoDropDownList").value();
                u.UserCultureInfo = $("#cultureDropdown").data("kendoComboBox").value();
                u.UserTimeZone = $("#timeZoneDropdown").data("kendoComboBox").value();
                var urls = "api/Users/SaveUserSettings";
                $.ajax({
                    async: false,
                    type: "POST",
                    url: urls,
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
                                if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save User Settings Failure", data.Errors, null); }
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
                                if (strValidationMsg.length < 1) { strValidationMsg = "There was a problem while saving the User Settings"; }
                                ngl.showErrMsg("Save User Settings Failure", strValidationMsg, null);
                            } else { document.location = oredirectUri; }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save User Settings Failure", sMsg, null); }
                });
            }

            function changePassSuccessCallBack(oEventParameters) {
                //add additional validation here
                if (oEventParameters.error != null) {
                    $("#txtOldPass").val("");
                    $("#txtNewPass").val("");
                    $("#txtConfirmNewPass").val("");
                } else {
                    $("#txtOldPass").val($("#txtNewPass").val());
                    $("#txtNewPass").val("");
                    $("#txtConfirmNewPass").val("");
                }
            }

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

            function getMenuCookie() {
                var nameEQ = "menuOpen=";
                var ca = document.cookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                    if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
                }
                return null;
            }

            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>;                

                var kmask = new kendoMasks();
                kmask.loadDefaultMasks();
                var phoneMask = kmask.getMask("phone_number");

                $("#txtEmail").kendoMaskedTextBox();
                $("#txtFriendlyName").kendoMaskedTextBox();
                $("#txtFirstName").kendoMaskedTextBox();
                $("#txtLastName").kendoMaskedTextBox();
                $("#txtTitle").kendoMaskedTextBox();
                $("#txtDepartment").kendoMaskedTextBox();
                $("#txtCellPhone").kendoMaskedTextBox({ mask: phoneMask });
                $("#txtHomePhone").kendoMaskedTextBox({ mask: phoneMask });
                $("#txtWorkPhone").kendoMaskedTextBox({ mask: phoneMask });
                $("#txtWorkPhoneExt").kendoMaskedTextBox();

                $("#btnSaveInfo").kendoButton({
                    icon: "save",
                    click: function (e) { saveUserSettings(resGetUserSettings); }
                });

                $("#btnMenuReset").kendoButton({
                    icon: "k-icon k-i-exclamation-circle"
                });

                $("#btnMenuSettings").kendoButton({
                    icon: "k-icon k-i-gear k-icon-48"
                });

                $("#btnMenuSettingsHide").kendoButton({
                    icon: "k-icon k-i-x-circle"
                });

                $("#txtOldPass").kendoMaskedTextBox();
                $("#txtNewPass").kendoMaskedTextBox();
                $("#txtConfirmNewPass").kendoMaskedTextBox();

                $("#btnChangePass").kendoButton({ imageUrl: "../Content/NGL/Keys16.png" });
                // Themes that do not work with DTMS
                // { text: "Material Aqua", value: "material-aqua-dark" },
                // { text: "Material Dark", value: "material-main-dark" },
                // Modified by RHR for v-8.5.4.001 on 05/22/2023
                // Note:  changes to Theme values must have supportig ngl-theme files
                //        added to the NGL\Content\NGL\Themes folder
                $("#ddThemes").kendoDropDownList({
                    dataSource: [
                        { text: "Black Opal", value: "classic-opal-dark" },
                        { text: "Blue Opal", value: "classic-opal" },
                        { text: "Bootstrap", value: "bootstrap-main" },
                        { text: "Default", value: "classic-main" },
                        { text: "Bootstrap Dark", value: "bootstrap-main-dark" },
                        { text: "Material", value: "material-main" },
                        { text: "Material Arctic", value: "material-arctic" },
                        { text: "Material Eggplant", value: "material-eggplant" },
                        { text: "Material Lime", value: "material-lime" },
                        { text: "Metro", value: "classic-metro" },
                        { text: "Metro Black", value: "classic-metro-dark" },
                        { text: "Moonlight", value: "classic-moonlight" },
                        { text: "Office 365", value: "fluent-main" },
                        { text: "Silver", value: "classic-silver" },
                        { text: "Uniform", value: "classic-uniform" }
                    ],
                    dataTextField: "text",
                    dataValueField: "value"
                });

                $("#cultureDropdown").kendoComboBox({
                    dataTextField: "DisplayName",
                    dataValueField: "CultureId",
                    autoWidth: true,
                    autofill: true,
                    filter: "contains",
                    dataSource: {
                        //serverFiltering: true,
                        transport: {
                            read: {
                                url: "api/Users/GetCultureInfo",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }
                        },
                        schema: {
                            data: "Data",
                            errors: "Errors"
                        }
                    },
                    select: function (e) {
                        var item = e.dataItem;
                        if (item) {
                            // If you need to do anything upon selecting a culture, you can do it here
                        }
                    }
                });
                $("#timeZoneDropdown").kendoComboBox({
                    dataTextField: "value",
                    dataValueField: "value",
                    autoWidth: true,
                    filter: "contains",
                    autofill: true,
                    dataSource: {
                        //serverFiltering: true,
                        transport: {
                            read: {
                                url: "api/Users/GetTimeZoneInfo",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }
                        },                        
                        schema: {
                            data: "Data",
                            errors: "Errors"
                        }
                    },
                    select: function (e) {
                        var item = e.dataItem;
                        if (item) {
                            // If you need to do anything upon selecting a culture, you can do it here
                        }
                    }
                });
                var sSignedIn = localStorage.SignedIn;
                if (typeof (sSignedIn) !== 'undefined' && sSignedIn != null && sSignedIn === "t") {
                    $("#pageContent").show();
                    getUserSettings(resGetUserSettings);
                    var iSSOAControl = localStorage.NGLvar1472;
                    if (typeof (iSSOAControl) === 'undefined' || iSSOAControl == null || iSSOAControl != "1") { $("#PasswordFastTab").hide(); } else { $("#PasswordFastTab").show(); }
                } else { $("#pageContent").hide(); }

                var PageReadyJS = <%=PageReadyJS%>;
                //menuTreeHighlightPage(); //must be called after PageReadyJS

                let menuOpened = getMenuCookie();

                if (menuOpened) {
                    $("#btnMenuSettings").click();
                    $('#btnMenuSettingsHide').show();
                }

                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') { divWait.hide(); }

                // subscription settings

                function onAdd(e) {
                    console.log("onAdd : " + getWidgetName(e));
                    console.log("e.dataItems", e.dataItems);
                    var elementID = getWidgetName(e);
                    var POSTurl = "api/UnsubscribedAlert/Post/";
                    if (elementID == "SubscribedAlertList") {
                         POSTurl = "api/SubscribedAlert/Post/";
                    }
                    debugger;
                    kendo.ui.progress($(document.body), true); 
                    $.each(e.dataItems, function (index, Item) {
                        $.ajax({
                            type: "POST",
                            url: POSTurl,
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            data: JSON.stringify(Item),
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                if (data.Errors != null) {
                                    if (data.StatusCode === 203) {
                                        ngl.showErrMsg("Authorization Timeout", data.Errors, null);
                                    }
                                    else {
                                        ngl.showErrMsg("Access Denied", data.Errors, null);
                                    }
                                } else {
                                    var Listbox = $("#" + elementID).data("kendoListBox");
                                    console.log("ListboxDS", Listbox)
                                    Listbox.refresh();
                                    var UnsubscribedAlertList = $("#UnsubscribedAlertList").data("kendoListBox");
                                    console.log("UnsubscribedAlertList", UnsubscribedAlertList)
                                    if (typeof (UnsubscribedAlertList) !== 'undefined' && ngl.isObject(UnsubscribedAlertList)) {
                                        $("#UnsubscribedCount").text(UnsubscribedAlertList.dataSource.data().length);

                                        //UnsubscribedAlertList.dataSource.read();
                                    }
                                    var SubscribedAlertList = $("#SubscribedAlertList").data("kendoListBox");
                                    console.log("oKendoList1", SubscribedAlertList)
                                    if (typeof (SubscribedAlertList) !== 'undefined' && ngl.isObject(SubscribedAlertList)) {
                                        $("#SubscribedCount").text(SubscribedAlertList.dataSource.data().length);
                                        //SubscribedAlertList.dataSource.read();
                                    }
                                    // add code here similar to get all subscribe and refresh list on the screen with the data.
                                    //document.location.reload();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                //kendo.ui.progress($(document.body), false); 
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Add New Truck Item Failure");
                                ngl.showErrMsg("Add New Truck Item Failure", sMsg, null);
                               // kendo.ui.progress($(document.body), false); 
                            }
                        });
                    });
                    kendo.ui.progress($(document.body), false); 

                }

                function onChange(e) {
                    console.log("change : " + getWidgetName(e));
                }

                function onDataBound(e) {
                    if ("kendoConsole" in window) {
                        console.log("dataBound : " + getWidgetName(e));
                    }
                }

                function onRemove(e) {

                    console.log("onAdd : " + getWidgetName(e));
                    console.log("e.dataItems", e.dataItems);
                    var elementID = getWidgetName(e);
                    var DeleteUrl = "api/UnsubscribedAlert/DELETE";
                    if (elementID == "SubscribedAlertList") {
                         DeleteUrl = "api/SubscribedAlert/DELETE/";

                        
                    }

                    return false;
                    $.ajax({
                        type: "DELETE",
                        url: DeleteUrl,
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: JSON.stringify(e.dataItems[0]),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            if (data.Errors != null) {
                                if (data.StatusCode === 203) {
                                    ngl.showErrMsg("Authorization Timeout", data.Errors, null);
                                }
                                else {
                                    ngl.showErrMsg("Access Denied", data.Errors, null);
                                }
                            } else {
                                // add code here similar to get all subscribe and refresh list on the screen with the data.
                                var UnsubscribedAlertList = $("#UnsubscribedAlertList").data("kendoListBox");
                                console.log("UnsubscribedAlertList", UnsubscribedAlertList)
                                if (typeof (UnsubscribedAlertList) !== 'undefined' && ngl.isObject(UnsubscribedAlertList)) {
                                    $("#UnsubscribedCount").text(UnsubscribedAlertList.dataSource.data().length);

                                    //UnsubscribedAlertList.dataSource.read();
                                }
                                var SubscribedAlertList = $("#SubscribedAlertList").data("kendoListBox");
                                console.log("oKendoList1", SubscribedAlertList)
                                if (typeof (SubscribedAlertList) !== 'undefined' && ngl.isObject(SubscribedAlertList)) {
                                    $("#SubscribedCount").text(SubscribedAlertList.dataSource.data().length);
                                    //SubscribedAlertList.dataSource.read();
                                }
                                //document.location.reload();
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            //kendo.ui.progress($(document.body), false); 
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Add New Truck Item Failure");
                            ngl.showErrMsg("Add New Truck Item Failure", sMsg, null);
                            // kendo.ui.progress($(document.body), false); 
                        }
                    });

                    
                    console.log("remove : " + getWidgetName(e) + " : " + e.dataItems.length + " item(s)");
                };

                function onReorder(e) {
                    console.log("reorder : " + getWidgetName(e));
                }

                function onDragStart(e) {
                    console.log("dragstart : " + getWidgetName(e));
                }

                function onDrag(e) {
                    console.log("drag : " + getWidgetName(e));
                }

                function onDrop(e) {
                    console.log("drop : " + getWidgetName(e));
                }

                function onDragEnd(e) {
                    console.log("dragend : " + getWidgetName(e));
                }

                function getWidgetName(e) {
                    var listBoxId = e.sender.element.attr("id");
                    var widgetName = listBoxId === "UnsubscribedAlertList" ? "UnsubscribedAlertList" : "SubscribedAlertList";
                    return widgetName;
                }

                var  UnsubscribedAlertDS = new kendo.data.DataSource({
                        serverSorting: true,
                        serverPaging: true,
                        pageSize: 10,
                        transport: {
                            read: function (options) {
                                var s = {};
                                $.ajax({
                                    url: 'api/UnsubscribedAlert/GetRecords',
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    data: { filter: JSON.stringify(s), PageStatus: JSON.stringify({}) },
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                    success: function (data) {
                                        options.success(data);
                                        if (data.Errors != null) {
                                            PageStatus = 1;
                                            if (data.StatusCode === 203) {
                                                ngl.showErrMsg("Authorization Timeout", data.Errors, null);
                                            }
                                            else {
                                                ngl.showErrMsg("Access Denied", data.Errors, null);
                                            }
                                        } else {
                                            $("#UnsubscribedCount").text(data.Count);
                                        
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
                                id: "ProcedureControl",
                                fields: {
                                    ProcAlertUserXrefSendEmail: { type: "number" },
                                    ProcAlertUserXrefShowPopup: { type: "number" },
                                    ProcedureControl: { type: "number" },
                                    ProcedureDescription: { type: "string" },
                                    ProcedureHasAlert: { type: "bool" },
                                    ProcedureName: { type: "string" },
                                    ProcedureSecurityGroupXrefControl: { type: "number" },
                                    ProcedureSecurityXrefControl: { type: "number" },
                                    ProcedureUpdated: { type: "number" },
                                    ProcedureUserOverrideGroup: { type: "bool" },
                                    strEmail: { type: "number" },
                                    strPopup: { type: "number" },
                                }
                            },
                            errors: "Errors"
                        },
                        error: function (xhr, textStatus, error) {
                            ngl.showErrMsg("Access UnsubscribedAlert Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                        }
                    });


                var SubscribedAlertDS = new kendo.data.DataSource({
                    serverSorting: true,
                    serverPaging: true,
                    pageSize: 10,
                    transport: {
                        read: function (options) {
                            var s = {};
                            $.ajax({
                                url: 'api/SubscribedAlert/GetRecords',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: { filter: JSON.stringify(s), PageStatus: JSON.stringify({}) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    options.success(data);
                                    if (data.Errors != null) {
                                        PageStatus = 1;
                                        if (data.StatusCode === 203) {
                                            ngl.showErrMsg("Authorization Timeout", data.Errors, null);
                                        }
                                        else {
                                            ngl.showErrMsg("Access Denied", data.Errors, null);
                                        }
                                    } else {
                                        $("#SubscribedCount").text(data.Count);
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
                            id: "ProcedureControl",
                            fields: {
                                ProcAlertUserXrefSendEmail: { type: "number" },
                                ProcAlertUserXrefShowPopup: { type: "number" },
                                ProcedureControl: { type: "number" },
                                ProcedureDescription: { type: "string" },
                                ProcedureHasAlert: { type: "bool" },
                                ProcedureName: { type: "string" },
                                ProcedureSecurityGroupXrefControl: { type: "number" },
                                ProcedureSecurityXrefControl: { type: "number" },
                                ProcedureUpdated: { type: "number" },
                                ProcedureUserOverrideGroup: { type: "bool" },
                                strEmail: { type: "number" },
                                strPopup: { type: "number" },
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) {
                        ngl.showErrMsg("Access SubscribedAlert Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                    }
                });

                $("#UnsubscribedAlertList").kendoListBox({
                    connectWith: "SubscribedAlertList",
                    draggable: true,
                    dropSources: ["SubscribedAlertList"],
                    toolbar: {
                        position: "right",
                        tools: ["transferAllTo",  "transferTo", "transferFrom", "transferAllFrom"]
                    }
                    ,
                    messages: {
                        tools: {
                            transferTo: "Subscribe",
                            transferFrom: "Unsubscribe",
                            transferAllTo: "Subscribe All",
                            transferAllFrom: "Unsubscribe All",
                            remove: "Delete Unsubscription"
                        }
                    },
                    selectable: "multiple",
                    dataSource: UnsubscribedAlertDS,
                    template: kendo.template($("#templateUnsubscribedList").html()),
                    add: onAdd,
                    change: onChange,
                    dataBound: onDataBound,
                    dragstart: onDragStart,
                    drag: onDrag,
                    drop: onDrop,
                    dragend: onDragEnd,
                    remove: onRemove,
                    reorder: onReorder
                });

                $("#SubscribedAlertList").kendoListBox({
                    connectWith: "UnsubscribedAlertList",
                    draggable: {
                        placeholder: function (element) {
                            return element.clone().css({
                                "opacity": 0.3,
                                "border": "1px dashed #000000"
                            });
                        }
                    },
                    dropSources: ["UnsubscribedAlertList"],
                    selectable: "multiple",
                    dataSource: SubscribedAlertDS,
                    template: kendo.template($("#templateSubscribedList").html()),
                    toolbar: {
                        position: "right",
                        tools: []
                    },
                     messages: {
                        tools: {
                            transferTo: "Subscribe",
                            transferFrom: "Unsubscribe",
                            transferAllTo: "Subscribe All",
                            transferAllFrom: "Unsubscribe All",
                            remove: "Delete Subscription"
                        }
                    },
                    add: onAdd,
                    change: onChange,
                    dataBound: onDataBound,
                    dragstart: onDragStart,
                    drag: onDrag,
                    drop: onDrop,
                    dragend: onDragEnd,
                    remove: onRemove,
                    reorder: onReorder
                });

            });
          
            function SubscriptionAlertChange(Listbox, ProcedureControl) {
                console.log("Listbox, ProcedureControl", Listbox, ProcedureControl);
                var ListboxDS = $("#" + Listbox).data("kendoListBox").dataSource;
                var ListboxData = ListboxDS.data();
                var parameter = ListboxData.filter((item) => item.ProcedureControl == ProcedureControl);
                parameter[0].ProcAlertUserXrefSendEmail = $("#EmailAlert" + ProcedureControl).is(":checked") ? 1 : 0;
                parameter[0].ProcAlertUserXrefShowPopup = $("#PopupAlert" + ProcedureControl).is(":checked") ? 1 : 0;

                console.log("parameter", parameter);

                $.ajax({
                    url: 'api/SubscribedAlert/SystemAlertSaveUserSettings',
                  contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    type: "POST",
                    data: JSON.stringify(parameter[0]),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                   success: function (data) {
                        //options.success(data);
                        if (data.Errors != null) {
                            PageStatus = 1;
                            if (data.StatusCode === 203) {
                                ngl.showErrMsg("Authorization Timeout", data.Errors, null);
                            }
                           else {
                                ngl.showErrMsg("Access Denied", data.Errors, null);
                            }
                        } else {
                            $("#SubscribedCount").text(data.Count);
                            var RefreshListbox = $("#" + Listbox).data("kendoListBox");
                            console.log("ListboxDS", RefreshListbox)
                            RefreshListbox.refresh();
                        }
                    },

                    error: function (result) {
                        options.error(result);
                    }
                });
            }

            function EmailAlertSuccessCallback(data) { console.log("EmailAlertSuccessCallback", data) }
            function EmailAlertAjaxErrorCallback(xhr, textStatus, error) { ngl.showErrMsg("Stop Resequence Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tPage); }        
        </script>
    </div>
</body>
</html>
