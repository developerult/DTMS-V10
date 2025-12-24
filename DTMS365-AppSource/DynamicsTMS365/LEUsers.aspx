<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LEUsers.aspx.cs" Inherits="DynamicsTMS365.LEUsers" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS User Maintenance</title>
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
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;"> 
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
                            <!-- End Page Content -->

                          <div id="divLE"></div>

                          <div id="id47172201712271727264291841"><div class="fast-tab"><span id="ExpandLEUSSpan" style="display:none;"><a onclick="expandFastTab('ExpandLEUSSpan','CollapseLEUSSpan','LEUSHeader','LEUSDetail');"><span style="font - size: small; font - weight:bold;" class="k-icon k-i-chevron-down"></span></a></span><span id="CollapseLEUSSpan" style="display:normal;"><a onclick="collapseFastTab('ExpandLEUSSpan','CollapseLEUSSpan','LEUSHeader',null);"><span style="font - size: small; font - weight:bold;" class="k-icon k-i-chevron-up"></span></a></span> <span style="font-size:small; font-weight:bold" > Legal Entity Users</span></div><div id="LEUSHeader" class="OpenOrders"><div id="LEUSwrapper"><div id="LEUSFilterFastTab"><span id="ExpandLEUSFilterFastTabSpan" style="display:none;"><a onclick="expandFastTab('ExpandLEUSFilterFastTabSpan','CollapseLEUSFilterFastTabSpan','LEUSFilterFastTabHeader',null);"><span style="font - size: small; font - weight:bold;" class="k-icon k-i-chevron-down"></span></a></span><span id="CollapseLEUSFilterFastTabSpan" style="display:normal;"><a onclick="collapseFastTab('ExpandLEUSFilterFastTabSpan','CollapseLEUSFilterFastTabSpan','LEUSFilterFastTabHeader',null);"><span style="font - size: small; font - weight:bold;" class="k-icon k-i-chevron-up"></span></a></span><span style="font-size:small; font-weight:bold" > Filters </span><div id="LEUSFilterFastTabHeader" style="padding: 10px;"><span><label for="ddlLEUSFilters" > Filter by:</ label><input id="ddlLEUSFilters" /><span id="spLEUSfilterText"><input id="txtLEUSFilterVal" /></span><span id="spLEUSfilterDates" ><label for="dpLEUSFilterFrom" > From:</ label><input id="dpLEUSFilterFrom" /><label for="dpLEUSFilterTo" > To:</ label><input id="dpLEUSFilterTo" /></span><span id="spLEUSfilterButtons" ><a id="btnLEUSFilter" ></a><a id="btnLEUSClearFilter" ></a></span></span><input id="txtLEUSSortDirection" type="hidden" /><input id="txtLEUSSortField" type="hidden" /></div></div><div id="LEUsersGrid"></div></div></div></div> 

                          <input id="txtLEControl" type="hidden" />
                          <input id="txtLEName" type="hidden" />
                          <input id="txtLECompControl" type="hidden" />

                        </div>
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>


          <div id="wndUserLE">
              <div id="lblSetLEUser"></div>
              <div>
                  <input id="ddlUserLE" style="min-width:200px; width:100%;" />
              </div>
              <div style="margin-top: 5px;">
                  <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" onclick="btnSetUserLE_Click();">Apply</button>
              </div>
              <input id="txtSelectedUSC" type="hidden" />
          </div>
          
          <div id="wndActivateUser">
              <div id="lblActivateUser"></div>
              <div>
                  <input id="ddlUserGroupsByLE2" style="min-width:200px; width:100%;" />
              </div>
              <div style="margin-top: 5px;">
                  <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" onclick="activateUser();">Activate</button>
              </div>
              <input id="txtActivateUSC" type="hidden" />
          </div>

          <div id="wndAddVisibleComp">
              <div>
                  <div style="padding: 0px 10px 0px 10px;">
                      <div><div id="lblVisibleCompUserName"></div></div>
                      <!--<div><p>Note: Mass Update will only perform create or updates - not deletes</p></div>-->
                      <div>
                          <strong>Apply To: </strong>
                          <div id="unrestrictedCompsGrid"></div>
                      </div>
                  </div>
                  <input id="txtAddVisibleCompUSC" type="hidden" />
              </div>
          </div>

          <div id="wndAddVisibleLane">
              <div>
                  <div style="padding: 0px 10px 0px 10px;">
                      <div><div id="lblVisibleLaneUserName"></div></div>
                      <!--<div><p>Note: Mass Update will only perform create or updates - not deletes</p></div>-->
                      <div>
                          <strong>Apply To: </strong>
                          <div id="unrestrictedLanesGrid"></div>
                      </div>
                  </div>
                  <input id="txtAddVisibleLaneUSC" type="hidden" />
              </div>
          </div>

          <script type="text/x-kendo-template" id="LEUsersDetTemplate">
              <div class="tabstrip">
                <ul>
                    <li class="k-active">Single Sign On Accounts</li>
                    <li>Associated Carriers</li>
                    <li>Visible Companies</li>
                    <li>Visible Lanes</li>
                </ul>              
                <div><div class="ssoAccounts"></div></div>
                <div><div class="associatedCarriers"></div></div>
                <div><div class="companyRestrictions"></div></div>
                <div><div class="laneRestrictions"></div></div>
              </div>
          </script>

    <% Response.WriteFile("~/Views/SingleSignOnEditWindow.html"); %>
    <% Response.WriteFile("~/Views/wndLE.html"); %>
    <% Response.WriteFile("~/Views/UserAddWindow.html"); %>
    <% Response.WriteFile("~/Views/LEUsersAssociatedCarrierEAWnd.html"); %>
    <% Response.WriteFile("~/Views/SSOAMassUpdateWnd.html"); %>
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

        var oLEUsersGrid = null;
        var oSSOAGrid = null;
        var oRoleGrid = null;
        var oChgRoleLEGrid = null;
        var tObjPG = this;
        // user child grids
        var olaneRestGrid = null; //laneRestrictions
        var ocompanyRestrictionsGrid = null;
        var oassociatedCarriersGrid = null;
        var ossoAccountsGrid = null;

        var wndLE = kendo.ui.Window;        
        var wndUserLE = kendo.ui.Window;
        var wndAddUser = kendo.ui.Window;
        var wndActivateUser = kendo.ui.Window; 
        var dsLEUsers = kendo.data.DataSource;
        var dsUserLE = kendo.data.DataSource;
        var dsUserGroups = kendo.data.DataSource;      
        var wndAddVisibleComp = kendo.ui.Window     
        var wndAddVisibleLane = kendo.ui.Window;


        //************* Action Menu Functions ********************
        function execActionClick(btn, proc){
            if(btn.id == "btnChangeLE"){ changeLegalEntity(); }
            else if(btn.id == "btnAddSSOA"){ openSingleSignOnAddWindow(); }
            else if(btn.id == "btnSetUserLE"){ setUserLE(); }
            else if(btn.id == "btnCreateUser"){   
                if($("#txtLEControl").val() == 0) { ngl.showWarningMsg("Cannot Add User to Legal Entity 'None'", "Cannot add user to Legal Entity 'None'. To add a Carrier user click the 'Add New Carrier User' button. Otherwise please select a valid Legal Entity to add a non-carrier user.", null); } else { openAddNewUserWindow(false); } 
            }
            else if(btn.id == "btnCreateCarUser"){ openAddNewUserWindow(true); }
            else if(btn.id === "btnManageRoles"){                                  
                if (typeof (tPage["wdgtMngRoleWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtMngRoleWndDialog"])){
                    tPage["wdgtMngRoleWndDialog"].show();                
                } else{alert("Missing HTML Element (wdgtMngRoleWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
            }
            else if(btn.id === "btnSetRoleLE"){
                if (typeof (tPage["wdgtAssignRoleLEWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtAssignRoleLEWndDialog"])){
                    tPage["wdgtAssignRoleLEWndDialog"].show();                
                } else{alert("Missing HTML Element (wdgtAssignRoleLEWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
            }
            else if(btn.id === "btnManageSSOA"){                                  
                if (typeof (tPage["wdgtMngSSOAWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtMngSSOAWndDialog"])){
                    tPage["wdgtMngSSOAWndDialog"].show();                
                } else{alert("Missing HTML Element (wdgtMngSSOAWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
            }
            else if(btn.id == "btnMassUpdateSSOA"){ openSSOAMassUpdateWindow(); }
            else if (btn.id == "btnRefresh" ){ refresh(); }
            else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        function refresh(){
            ngl.readDataSource(oLEUsersGrid);
            ngl.readDataSource(oSSOAGrid);
            ngl.readDataSource($('#massUpdateUsersGrid').data('kendoGrid'));
            ngl.readDataSource($("#ddlUserGroupsByLE").data("kendoDropDownList"));
        }


        //************* Call Back Functions **********************
        function LEUsersGridDataBoundCallBack(e,tGrid){   
            oLEUsersGrid = tGrid;           
            // get the index of the UserGroupsName column
            var columnIndexUGName = tGrid.wrapper.find(".k-grid-header [data-field=" + "UserGroupsName" + "]").index();
            var ds = tGrid.dataSource.data(); 
            for (var j=0; j < ds.length; j++) {
                if (typeof (ds[j].UserUserGroupsControl) !== 'undefined' && ds[j].UserUserGroupsControl != null) {
                    //INACTIVE USER
                    if (ds[j].UserUserGroupsControl === 10) {
                        //This user is InActive so disable the Edit button and change the "Deactivate User" button to "Activate User"
                        var item = tGrid.dataSource.get(ds[j].UserSecurityControl); //Get by ID or any other preferred method

                        //Hide the Edit and Delete buttons
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditLEUser").hide(); 
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-DeleteLEUser").hide();
                        //Change this button to say "Activate" and change the icon to "unlock"
                        var btn = tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-Deactivate");
                        btn.html("<span class='k-icon k-i-check-circle'></span>Activate");

                        //Change the text to red for the UserGroupsName column
                        var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']");
                        var cell = row.children().eq(columnIndexUGName);
                        cell.addClass("red");
                    }
                    //CARRIER USER
                    if (ds[j].UserUserGroupsControl === 7 || ds[j].UserUserGroupsControl === 8) {
                        //Currently there is nothing to edit if the user is a Carrier User so disable the "Edit" button
                        var item = tGrid.dataSource.get(ds[j].UserSecurityControl); //Get by ID or any other preferred method                      
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditLEUser").prop('disabled', true); //Disable the Edit buttons
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditLEUser").addClass("k-state-disabled"); //Add the diabled class to the css (change button color)
                    }
                    ////if (ds[j].UserUserGroupsControl === 4) {
                    ////    //var columns = tGrid.columns;
                    ////    //columns[0].name = FreeTrialButtons;
                    ////    var columnIndexFTBtn = tGrid.wrapper.find(".k-grid-header [data-name=" + "FreeTrialButtons" + "]").index();
                    ////    tGrid.hideColumn(columnIndexFTBtn);
                    ////}
                }                
            }
        }

        function SSOAGridDataBoundCallBack(e,tGrid){           
            oSSOAGrid = tGrid;       
        }

        function RoleGridDataBoundCallBack(e,tGrid){   
            oRoleGrid = tGrid;           
            var ds = tGrid.dataSource.data(); 
            for (var j=0; j < ds.length; j++) {
                if (typeof (ds[j].UserGroupsControl) !== 'undefined' && ds[j].UserGroupsControl != null) {
                    //INACTIVE USER
                    if (ds[j].UserGroupsControl <= 10) {
                        //This UserGroup is a System Default and cannot be edited or deleted
                        var item = tGrid.dataSource.get(ds[j].UserGroupsControl); //Get by ID or any other preferred method
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditRoleGridCRUDCtrl").prop('disabled', true); //Disable the Edit button
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditRoleGridCRUDCtrl").addClass("k-state-disabled"); //Add the diabled class to the css (change button color)
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-DeleteRoleGridCRUDCtrl").prop('disabled', true); //Disable the Delete button
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-DeleteRoleGridCRUDCtrl").addClass("k-state-disabled"); //Add the diabled class to the css (change button color)
                    }
                }                
            }
        }

        function RoleGridGetStringData(s)
        {                              
            s.ParentControl = $("#txtLEControl").val();
            return '';
        }

        function execBeforeRoleGridInsert(e,fk,w){
            var leCompControl = $("#txtLECompControl").val();
            if (!leCompControl) { alert("No Legal Entity Field - cannot insert record"); return false;}
            return w.SetFieldDefault("UserGroupsLegalEntityCompControl",leCompControl.toString());            
        }
     
        function ChgRoleLEGridDataBoundCallBack(e,tGrid){   
            oChgRoleLEGrid = tGrid;           
            var ds = tGrid.dataSource.data(); 
            for (var j=0; j < ds.length; j++) {
                if (typeof (ds[j].UserGroupsControl) !== 'undefined' && ds[j].UserGroupsControl != null) {
                    //INACTIVE USER
                    if (ds[j].UserGroupsControl <= 10) {
                        //This UserGroup is a System Default and cannot be edited or deleted
                        var item = tGrid.dataSource.get(ds[j].UserGroupsControl); //Get by ID or any other preferred method
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditChgRoleLEGridCRUDCtrl").prop('disabled', true); //Disable the Edit button
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditChgRoleLEGridCRUDCtrl").addClass("k-state-disabled"); //Add the diabled class to the css (change button color)
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-DeleteChgRoleLEGridCRUDCtrl").prop('disabled', true); //Disable the Delete button
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-DeleteChgRoleLEGridCRUDCtrl").addClass("k-state-disabled"); //Add the diabled class to the css (change button color)
                    }
                }                
            }
        }
       
        function execBeforescreensInsert(e,fk,w){
            //We need to populate the UserGroupsControl in the new record so we get that from the header row
            var parentRow = $(e.currentTarget).closest("tr.k-detail-row").prev("tr"); // GET PARENT ROW ELEMENT    
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid");            
            var parentModel = parentGrid.dataItem(parentRow); // GET THE PARENT ROW MODEL           
            var groupControl = parentModel.UserGroupsControl; // ACCESS THE PARENT ROW MODEL ATTRIBUTES
            if (!groupControl) { alert("No Group Control Field - cannot insert record"); return false;}
            return w.SetFieldDefault("UserGroupsControl",groupControl.toString());                
        }

        function execBeforereportsInsert(e,fk,w){
            //We need to populate the UserGroupsControl in the new record so we get that from the header row
            var parentRow = $(e.currentTarget).closest("tr.k-detail-row").prev("tr"); // GET PARENT ROW ELEMENT    
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid");            
            var parentModel = parentGrid.dataItem(parentRow); // GET THE PARENT ROW MODEL           
            var groupControl = parentModel.UserGroupsControl; // ACCESS THE PARENT ROW MODEL ATTRIBUTES
            if (!groupControl) { alert("No Group Control Field - cannot insert record"); return false;}
            return w.SetFieldDefault("UserGroupsControl",groupControl.toString());                
        }

        function execBeforeproceduresInsert(e,fk,w){
            //We need to populate the UserGroupsControl in the new record so we get that from the header row
            var parentRow = $(e.currentTarget).closest("tr.k-detail-row").prev("tr"); // GET PARENT ROW ELEMENT    
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid");            
            var parentModel = parentGrid.dataItem(parentRow); // GET THE PARENT ROW MODEL           
            var groupControl = parentModel.UserGroupsControl; // ACCESS THE PARENT ROW MODEL ATTRIBUTES
            if (!groupControl) { alert("No Group Control Field - cannot insert record"); return false;}
            return w.SetFieldDefault("UserGroupsControl",groupControl.toString());                
        }


        //SHARED CALLBACK METHODS
        function AjaxErrorCallback(xhr, textStatus, error, cbSource, errName) {
            //Basically sets the messages from parameters and calls refreshLEUsersGrid() at the end
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = cbSource; //ex: "DeleteAssociatedCarrierAjaxErrorCallback"
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = errName; //ex: "Delete Associated Carrier Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);        
            if (ngl.isFunction(refreshLEUsersGrid)) { refreshLEUsersGrid(); }
        }
        function SuccessCallback(data, cbSource, errName) {
            //Basically sets the messages from parameters, does some source specific logic, and calls refreshLEUsersGrid() at the end
            var oResults = new nglEventParameters();
            oResults.source = cbSource; //ex: "SaveAssociatedCarrierSuccessCallback"
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
                        oResults.error.name = errName; //ex: "Save Associated Carrier Failure";
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
                                var blnRefreshed = false;
                                if (cbSource === "SaveSSOForUserCallback"){ 
                                    wndSingleSignOn.close();
                                    ossoAccountsGrid.dataSource.read();
                                    blnRefreshed = true;
                                }
                                else if (cbSource === "UpdateCarrierUserUSGSuccessCallback"){ wndAddUser.close(); }
                                else if (cbSource === "CreateUserSuccessCallback"){ wndAddUser.close(); }
                                else if (cbSource === "SaveAssociatedCarrierSuccessCallback"){ 
                                    wndAssociatedCarrierEA.close();
                                    oassociatedCarriersGrid.dataSource.read(); 
                                    blnRefreshed = true;
                                }
                                else if (cbSource === "ReplaceUserSecurityWithGroupCallback"){
                                    wndActivateUser.close();
                                    if($("#txtIsAdd").val() == 0){ wndAddUser.close(); }
                                }
                                if (blnRefreshed == false && ngl.isFunction(refreshLEUsersGrid)) { refreshLEUsersGrid(); }
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = errName; }
                    oResults.error = new Error();
                    oResults.error.name = errName;
                    oResults.error.message = strValidationMsg;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }      
        }          
        function DeleteSuccessCallback(data, cbSource, errName) {
            //Basically sets the messages from parameters and calls refreshLEUsersGrid() at the end
            var oResults = new nglEventParameters();
            oResults.source = cbSource; //ex: "DeleteSSOForUserCallback"
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                        oResults.error = new Error();
                        oResults.error.name = errName; //ex: "Delete Single Sign On For User Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    } else { this.rData = data.Data; }
                }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }
            
            if (cbSource === "DeleteSSOForUserCallback"){ ossoAccountsGrid.dataSource.read();} 
            else if (cbSource === "DeleteAssociatedCarrierSuccessCallback"){ oassociatedCarriersGrid.dataSource.read(); }
            else if (cbSource === "DeleteVisibleCompanySuccessCallback"){ ocompanyRestrictionsGrid.dataSource.read(); }
            else if (cbSource === "DeleteVisibleLaneSuccessCallback"){ olaneRestGrid.dataSource.read();} 
            else {
                if (ngl.isFunction(refreshLEUsersGrid)) { refreshLEUsersGrid(); }
            }                     
            
        }

        

        //SetUserLE
        function SetUserLECallback(data) { SuccessCallback(data, "SetUserLECallback", "Save LEUser Failure"); }
        function SetUserLEAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "SetUserLEAjaxErrorCallback", "Save LEUser Failure"); }
        //DeleteSSOForUser
        function DeleteSSOForUserCallback(data) { DeleteSuccessCallback(data, "DeleteSSOForUserCallback", "Delete Single Sign On For User Failure"); }
        function DeleteSSOForUserAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "DeleteSSOForUserAjaxErrorCallback", "Delete Single Sign On For User Failure"); }       
        //ReplaceUserSecurityWithGroup
        function ReplaceUserSecurityWithGroupCallback(data) { SuccessCallback(data, "ReplaceUserSecurityWithGroupCallback", "ReplaceUserSecurityWithGroup Failure"); }
        function ReplaceUserSecurityWithGroupAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "ReplaceUserSecurityWithGroupAjaxErrorCallback", "ReplaceUserSecurityWithGroup Failure"); }
        //UpdateCarrierUserUSG
        function UpdateCarrierUserUSGSuccessCallback(data) { SuccessCallback(data, "UpdateCarrierUserUSGSuccessCallback", "Update CarrierUser Failure"); }
        function UpdateCarrierUserUSGAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "UpdateCarrierUserUSGAjaxErrorCallback", "Update CarrierUser Failure"); }
        //CreateUser
        function CreateUserSuccessCallback(data) { SuccessCallback(data, "CreateUserSuccessCallback", "Create User Failure"); }
        function CreateUserAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "CreateUserAjaxErrorCallback", "Create User Failure"); }
        //DeleteUser
        function DeleteUserCallback(data) { DeleteSuccessCallback(data, "DeleteUserCallback", "Delete User Failure"); }
        function DeleteUserAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "DeleteUserAjaxErrorCallback", "Delete User Failure"); }
        //SaveAssociatedCarrier
        function SaveAssociatedCarrierSuccessCallback(data) { SuccessCallback(data, "SaveAssociatedCarrierSuccessCallback", "Save Associated Carrier Failure"); }
        function SaveAssociatedCarrierAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "SaveAssociatedCarrierAjaxErrorCallback", "Save Associated Carrier Failure"); }
        //DeleteAssociatedCarrier
        function DeleteAssociatedCarrierSuccessCallback(data) { DeleteSuccessCallback(data, "DeleteAssociatedCarrierSuccessCallback", "Delete Associated Carrier Failure"); }
        function DeleteAssociatedCarrierAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "DeleteAssociatedCarrierAjaxErrorCallback", "Delete Associated Carrier Failure"); }
        //DeleteVisibleCompany
        function DeleteVisibleCompanySuccessCallback(data) { DeleteSuccessCallback(data, "DeleteVisibleCompanySuccessCallback", "Delete Visible Company Failure"); }
        function DeleteVisibleCompanyAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "DeleteVisibleCompanyAjaxErrorCallback", "Delete Visible Company Failure"); }
        //DeleteVisibleCompany
        function DeleteVisibleLaneSuccessCallback(data) { DeleteSuccessCallback(data, "DeleteVisibleLaneSuccessCallback", "Delete Visible Lane Failure"); }
        function DeleteVisibleLaneAjaxErrorCallback(xhr, textStatus, error) { AjaxErrorCallback(xhr, textStatus, error, "DeleteVisibleLaneAjaxErrorCallback", "Delete Visible Lane Failure"); }


        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
           
        }


        //************* Page Functions **********************
        function refreshLEUsersGrid() { $('#LEUsersGrid').data('kendoGrid').dataSource.read();  }

        ////////////wndLE///////////////////
        var resGetLEAdmin = function (data) {
            $("#txtLEControl").val(data.LEAdminControl);
            $("#txtLEName").val(data.LegalEntity);
            $("#txtLECompControl").val(data.LECompControl);
            $("#divLE").html("<h2>" + data.LegalEntity + "</h2>");
            dsUserGroups.read(); //update the UserGroupsByLE ddl (it is based on txtLEControl)
            refreshLEUsersGrid();
        }

        function changeLegalEntity(){ wndLE.center().open(); }

        function btnSetLEA_Click(){
            var control = $("#ddLEA").data("kendoDropDownList").value();
            var name = $("#ddLEA").data("kendoDropDownList").text();
            if (ngl.isNullOrUndefined(control) || control === "") { control = 0; }                       
            $("#txtLEControl").val(control);
            $("#txtLEName").val(name);

            var dropdownlist = $("#ddLEA").data("kendoDropDownList");        
            var dataItem = dropdownlist.dataItem(); // get the dataItem corresponding to the selectedIndex.
            $("#txtLECompControl").val(dataItem.Description);
            
            $("#divLE").html("<h2>" + name + "</h2>");        
            //We need to reset the page before refresh to avoid problematic behavior
            //since we are completely changing the data returned (not just filtering or sorting from one retured record set)
            //$("#LEUsersGrid").data("kendoGrid").dataSource.page(1);
            //oLEUsersGrid.dataSource.page(1);
            refreshLEUsersGrid();
            dsUserGroups.read(); //update the UserGroupsByLE ddl (it is based on txtLEControl)          
            wndLE.close(); 
        }


        ////////wndUserLE///////////////////
        function setUserLE(){
            //Get the selected row from the grid
            var grid = $("#LEUsersGrid").data("kendoGrid");
            var row = grid.select();
            if (typeof (row) === 'undefined' && row == null) { ngl.showErrMsg("User Required", "Please select the header row in the grid for the User", null); return; } 
            //Get the dataItem for the selected row
            var dataItem = grid.dataItem(row);
            if (typeof (dataItem) === 'undefined' || dataItem == null || !ngl.isObject(dataItem)) { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; } //Verify the row item is not null

            //***** CANNOT EDIT AN INACTIVE USER *****
            if (!('UserUserGroupsControl' in dataItem)) { ngl.showErrMsg("UserUserGroupsControl Required", "Row object does not contain property UserUserGroupsControl.", null); return; } //Verify UserUserGroupsControl is a column in the grid              
            if (typeof (dataItem.UserUserGroupsControl) === 'undefined' || dataItem.UserUserGroupsControl == null) { ngl.showErrMsg("UserUserGroupsControl Required", "UserUserGroupsControl cannot be null", null); return; }                   
            if(dataItem.UserUserGroupsControl === 10){ ngl.showWarningMsg("User Inactive", "Cannot edit an inactive user", null); return; }              
            //****************************************

            //Cannot Assing a LE to a Carrier User
            if(dataItem.UserUserGroupsControl === 7 || dataItem.UserUserGroupsControl === 8){ ngl.showWarningMsg("Action Not Allowed", "Cannot assign a Legal Entity to a Carrier User", null); return; } 

            if (!('UserSecurityControl' in dataItem)) { ngl.showErrMsg("UserSecurityControl Required", "Row object does not contain property UserSecurityControl.", null); return; } //Verify UserSecurityControl is a column in the grid                                              
            if (typeof (dataItem.UserSecurityControl) === 'undefined' || dataItem.UserSecurityControl == null || dataItem.UserSecurityControl == 0) { ngl.showErrMsg("UserSecurityControl Required", "UserSecurityControl cannot be 0", null); return; } //Verify that UserSecurityControl is not null or 0                                           
            $("#txtSelectedUSC").val(dataItem.UserSecurityControl); //Save the UserSecurityControl so it can be accessed by the Save function                 
            if ('UserName' in dataItem) { $("#lblSetLEUser").html("<h3>" + dataItem.UserName + "</h3>"); }             
            if ('LEAdminControl' in dataItem) {               
                var dropdownlist = $("#ddlUserLE").data("kendoDropDownList");
                dropdownlist.select(function(d) { return d.LEAdminControl === dataItem.LEAdminControl; });           
            }                 
            wndUserLE.center().open();
        }
        
        function btnSetUserLE_Click(){            
            var g = new GenericResult();
            g.Control = $("#txtSelectedUSC").val();  
            g.intField1 = $("#ddlUserLE").data("kendoDropDownList").value();
            var tObj = tObjPG;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("LEUser/SetUserLE", g, tObj, "SetUserLECallback", "SetUserLEAjaxErrorCallback");   
            wndUserLE.close(); 
        }





        ////////////////////////////////////
        //////////ADD NEW USER/////////////
        function openAddNewUserWindow(blnIsCarrier){
            if(blnIsCarrier === true){ configureWndUIAddCarrierUser(); }else{ configureWndUIAddUser(); }               
            wndAddUser.center().open();
        }

        function openEditUserWindow(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));      
            if (typeof (item) === 'undefined' || item == null || !ngl.isObject(item)) { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; } //Verify the row item is not null                 
            if (!('UserSecurityControl' in item)) { ngl.showErrMsg("UserSecurityControl Required", "Row object does not contain property UserSecurityControl.", null); return; } //Verify UserSecurityControl is a column in the grid                 
            if (typeof (item.UserSecurityControl) === 'undefined' || item.UserSecurityControl == null || item.UserSecurityControl == 0) { ngl.showErrMsg("UserSecurityControl Required", "UserSecurityControl cannot be 0", null); return; } //Verify UserSecurityControl is not null or 0                            
            if (!('UserUserGroupsControl' in item)) { ngl.showErrMsg("UserUserGroupsControl Required", "Row object does not contain property UserUserGroupsControl.", null); return; } //Verify UserUserGroupsControl is a column in the grid                 
            $("#txtActivateUSC").val(item.UserSecurityControl); //Save the UserSecurityControl so it can be accessed by the Save function          
            //check if this is a carrier user
            var blnIsCarrier = false;
            if(item.UserUserGroupsControl === 7 || item.UserUserGroupsControl === 8) { blnIsCarrier = true; }                                
            //set the values
            if ('UserEmail' in item) { $("#txtUserEmail").data("kendoMaskedTextBox").value(item.UserEmail); }
            if ('UserFriendlyName' in item) { $("#txtFriendlyName").data("kendoMaskedTextBox").value(item.UserFriendlyName); }
            if ('UserFirstName' in item) { $("#txtFirstName").data("kendoMaskedTextBox").value(item.UserFirstName); }
            if ('UserLastName' in item) { $("#txtLastName").data("kendoMaskedTextBox").value(item.UserLastName); }
            //set conditional values (Carrier vs regular)   
            if (blnIsCarrier === true){           
                configureWndUIEditCarrierUser();
                if (item.UserUserGroupsControl === 8) { $("#chkCarrierAccounting").prop('checked', true); }
                if (item.UserUserGroupsControl === 7) { $("#chkCarrierAccounting").prop('checked', false); }                
                var dropdownlist = $("#ddlAddCarUserCarriers").data("kendoDropDownList");                   
                dropdownlist.select(function(dataItem) { return dataItem.Control === item.USCCarrierControl; });
            }else{
                configureWndUIEditUser();
                var dropdownlist = $("#ddlUserGroupsByLE").data("kendoDropDownList");                   
                dropdownlist.select(function(dataItem) { return dataItem.Control === item.UserUserGroupsControl; });       
            }                          
            wndAddUser.center().open();            
        }

        function validateRequiredFields(blnIsCarrier){
            var fields = "";
            var strSp = "";                
            //Shared required fields
            if (ngl.isNullOrWhitespace($("#txtUserEmail").data("kendoMaskedTextBox").value())){ fields += (strSp + "Email"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtFirstName").data("kendoMaskedTextBox").value())){ fields += (strSp + "First Name"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtLastName").data("kendoMaskedTextBox").value())){ fields += (strSp + "Last Name"); strSp = ", "; }
            if(blnIsCarrier === true){
                //Carrier required fields
                var dataItemC = $("#ddlAddCarUserCarriers").data("kendoDropDownList").dataItem();
                if ((typeof (dataItemC) === 'undefined' || dataItemC === null) || (typeof (dataItemC.Control) === 'undefined' || dataItemC.Control === null || dataItemC.Control < 1)){ fields += (strSp + "Carrier"); strSp = ", "; }
            }else{
                //User required fields
                var dataItemUG = $("#ddlUserGroupsByLE").data("kendoDropDownList").dataItem();
                if ((typeof (dataItemUG) === 'undefined' || dataItemUG === null) || (typeof (dataItemUG.Control) === 'undefined' || dataItemUG.Control === null || dataItemUG.Control < 1)){ fields += (strSp + "User Group"); strSp = ", "; }
                //NGLAPI required fields
                if ($('#chkAllowNGLAPI').is(":checked")) {
                    if (ngl.isNullOrWhitespace($("#txtAccountGroup").data("kendoMaskedTextBox").value())){ fields += (strSp + "Account Group"); strSp = ", "; }
                }
            }
            //Legacy required fields
            if (!$('#chkLoginWithMicrosoft').is(":checked")) {
                if (ngl.isNullOrWhitespace($("#txtLegacyUserName").data("kendoMaskedTextBox").value())){ fields += (strSp + "User Name"); strSp = ", "; }
                if (!$('#chkAutoGeneratePwd').is(":checked")) {
                    if (ngl.isNullOrWhitespace($("#txtLegacyPwd").data("kendoMaskedTextBox").value())){ fields += (strSp + "Password"); strSp = ", "; }
                }
            }       
            return fields;
        }

        function SaveNewUser(){
            var blnIsCarrier = false;
            if($("#txtIsCarrierUser").val() == 1){ blnIsCarrier = true; }
            var tObj = tObjPG;
            if($("#txtIsAdd").val() == 0){ 
                //***** EDIT *****
                if(blnIsCarrier === false){
                    //NON-CARRIER USER
                    var dataItem = $("#ddlUserGroupsByLE").data("kendoDropDownList").dataItem();
                    var g = new GenericResult();
                    g.Control = $("#txtActivateUSC").val();
                    g.intField1 = dataItem.Control;                
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("UserGroup/ReplaceUserSecurityWithGroup", g, tObj, "ReplaceUserSecurityWithGroupCallback", "ReplaceUserSecurityWithGroupAjaxErrorCallback");
                }else{
                    //CARRIER USER
                    var g = new GenericResult();
                    if ($('#chkCarrierAccounting').is(":checked")) { g.strField = "Y"; } else { g.strField = "N"; }
                    g.Control = $("#txtActivateUSC").val();                
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("UserGroup/UpdateCarrierUserUserSecurityGroup", g, tObj, "UpdateCarrierUserUSGSuccessCallback", "UpdateCarrierUserUSGAjaxErrorCallback");
                }
            }
            else {
                //***** ADD *****                          
                var alertMsg = "";
                alertMsg = validateRequiredFields(blnIsCarrier); //make sure all required fields are populated
                if (!ngl.isNullOrWhitespace(alertMsg)){ ngl.showErrMsg("Required Fields", alertMsg, null); return; }
                var tfLoginWithMicrosoft = false;  
                var tfAllowNGLAPI = false;  
                var tfAutoGeneratePwd = false;  
                var tfSendUserPwd = false;  
                if ($('#chkLoginWithMicrosoft').is(":checked")) { tfLoginWithMicrosoft = true; }       
                if ($('#chkAllowNGLAPI').is(":checked")) { tfAllowNGLAPI = true; } 
                if ($('#chkAutoGeneratePwd').is(":checked")) { tfAutoGeneratePwd = true; } 
                if ($('#chkSendUserPwdEmail').is(":checked")) { tfSendUserPwd = true; } 
                var item = new User();
                //SHARED
                item.UserEmail = $("#txtUserEmail").data("kendoMaskedTextBox").value();
                item.UserFriendlyName = $("#txtFriendlyName").data("kendoMaskedTextBox").value();
                item.UserFirstName = $("#txtFirstName").data("kendoMaskedTextBox").value();
                item.UserLastName = $("#txtLastName").data("kendoMaskedTextBox").value();
                item.UserPhoneWork = $("#txtWorkPhone").data("kendoMaskedTextBox").value();
                item.UserPhoneWorkExt = $("#txtWorkPhoneExt").data("kendoMaskedTextBox").value();
                item.UseMicrosoftAccount = tfLoginWithMicrosoft;
                item.AutoGeneratePwd = tfAutoGeneratePwd;
                item.SendUserPwd = tfSendUserPwd;
                item.Pwd = $("#txtLegacyPwd").data("kendoMaskedTextBox").value();
                item.UserName = $("#txtLegacyUserName").data("kendoMaskedTextBox").value();
                item.UserCultureInfo = $("#cultureDropdown").data("kendoComboBox").value(); 
                item.UserTimeZone = $("#timeZoneDropdown").data("kendoComboBox").value(); 
                if(blnIsCarrier === true){
                    //CARRIER USER
                    item.blnIsCarrierUser = true;
                    item.LEAControl = 0;
                    var c = new vUserSecurityCarrier();
                    c.USCControl = 0; 
                    var dataItem = $("#ddlAddCarUserCarriers").data("kendoDropDownList").dataItem();                    
                    c.USCCarrierControl = dataItem.Control;
                    c.USCCarrierNumber = dataItem.Description;
                    c.USCCarrierContControl = 0;   
                    if ($('#chkCarrierAccounting').is(":checked")) { c.USCCarrierAccounting = "Y"; } else { c.USCCarrierAccounting = "N"; }
                    var tempCarrier = new Array();
                    tempCarrier.push(c);
                    item.AssociatedCarriers = tempCarrier;
                }else{                    
                    //NON-CARRIER USER
                    item.blnIsCarrierUser = false;
                    item.LEAControl = $("#txtLEControl").val();
                    item.AccountGroup = $("#txtAccountGroup").data("kendoMaskedTextBox").value();
                    var dataItem = $("#ddlUserGroupsByLE").data("kendoDropDownList").dataItem();
                    item.UserUserGroupsControl = dataItem.Control;                           
                    item.AllowNGLAPI = tfAllowNGLAPI;
                    item.AssociatedCarriers = UserAddCars;
                }             
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("LEUser/CreateUser", item, tObj, "CreateUserSuccessCallback", "CreateUserAjaxErrorCallback");
            } 
        }
      
        var deleteUserControl = 0;
        function deleteUser(e){
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            deleteUserControl = 0; //clear any old values        
            if (typeof (item) === 'undefined' || item == null || !ngl.isObject(item)) { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; } //Verify the row item is not null       
            if (!('UserSecurityControl' in item)) { ngl.showErrMsg("UserSecurityControl Required", "Row object does not contain property UserSecurityControl.", null); return; } //Verify UserSecurityControl is a column in the grid
            if (typeof (item.UserSecurityControl) === 'undefined' || item.UserSecurityControl == null || item.UserSecurityControl == 0) { ngl.showErrMsg("UserSecurityControl Required", "UserSecurityControl cannot be 0", null); return; } //Verify UserSecurityControl is not null or 0             
            if (!('UserUserGroupsControl' in item)) { ngl.showErrMsg("UserUserGroupsControl Required", "Row object does not contain property UserUserGroupsControl.", null); return; } //Verify UserUserGroupsControl is a column in the grid
            if (typeof (item.UserUserGroupsControl) === 'undefined' || item.UserUserGroupsControl == null) { ngl.showErrMsg("UserUserGroupsControl Required", "UserUserGroupsControl cannot be null", null); return; } //Verify UserUserGroupsControl is not null or 0                                             
            if(item.UserUserGroupsControl == 10){ ngl.showWarningMsg("User Inactive", "Cannot delete an inactive user", null); return; } //CANNOT DELETE AN INACTIVE USER      
            deleteUserControl = item.UserSecurityControl;            
            if (deleteUserControl == 0) { ngl.showErrMsg("Delete Error", "Could not get UserSecurityControl from the record", null); return; }       
            //perform confirmation
            var title = "Delete User Account";
            ngl.OkCancelConfirmation(
                title,
                "Are you sure that you want to delete this User?",
                400,
                400,
                null,
                "ConfirmDeleteUser");
        }

        function ConfirmDeleteUser(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
            //Chose the Ok action
            if(typeof (deleteUserControl) !== 'undefined' && deleteUserControl > 0){   
                var tObj = tObjPG;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.delete("LEUser/DeleteUser", deleteUserControl, tObj, "DeleteUserCallback", "DeleteUserAjaxErrorCallback");           
            }
        }


        ////////////////////////////////////
        /////////DEACTIVATE USER///////////
        function activateUser(){
            var dataItem = $("#ddlUserGroupsByLE2").data("kendoDropDownList").dataItem();
            var g = new GenericResult();
            g.Control = $("#txtActivateUSC").val();
            g.intField1 = dataItem.Control;                  
            var tObj = tObjPG;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("UserGroup/ReplaceUserSecurityWithGroup", g, tObj, "ReplaceUserSecurityWithGroupCallback", "ReplaceUserSecurityWithGroupAjaxErrorCallback");
        }

        function deactivateUser(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            if (typeof (item) === 'undefined' || item == null || !ngl.isObject(item)) { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; } //Verify the row item is not null                 
            if (!('UserSecurityControl' in item)) { ngl.showErrMsg("UserSecurityControl Required", "Row object does not contain property UserSecurityControl.", null); return; } //Verify UserSecurityControl is a column in the grid                      
            if (typeof (item.UserSecurityControl) === 'undefined' || item.UserSecurityControl == null || item.UserSecurityControl == 0) { ngl.showErrMsg("UserSecurityControl Required", "UserSecurityControl cannot be 0", null); return; } //Verify that UserSecurityControl is not null or 0
            //Check if the user is "inactive" -- if yes then show the popup window to choose a new Group -- if no then deactivate
            if (!('UserUserGroupsControl' in item)) { ngl.showErrMsg("UserUserGroupsControl Required", "Row object does not contain property UserUserGroupsControl.", null); return; } //Verify UserUserGroupsControl is a column in the grid                                            
            if (typeof (item.UserUserGroupsControl) === 'undefined' || item.UserUserGroupsControl == null) { ngl.showErrMsg("UserUserGroupsControl Required", "UserUserGroupsControl cannot be null", null); return; }                      
            if(item.UserUserGroupsControl == 10){                   
                //show the popup window to choose a new Group    
                var un = "";                        
                if ('UserFriendlyName' in item) { if (typeof (item.UserFriendlyName) !== 'undefined' && item.UserFriendlyName != null && item.UserFriendlyName.length > 0){ un = " " + item.UserFriendlyName; } }
                $("#wndActivateUser").data("kendoWindow").title("Activate User" + un);                           
                $("#lblActivateUser").html("<strong>Assign User to Group</strong>");
                $("#txtActivateUSC").val(item.UserSecurityControl);
                wndActivateUser.center().open();                  
            }                      
            else{                        
                //deactivate
                var g = new GenericResult();
                g.Control = item.UserSecurityControl;                       
                g.intField1 = 10; //User Group "Inactive"                 
                var tObj = tObjPG;
                var oCRUDCtrl = new nglRESTCRUDCtrl();                        
                var blnRet = oCRUDCtrl.update("UserGroup/ReplaceUserSecurityWithGroup", g, tObj, "ReplaceUserSecurityWithGroupCallback", "ReplaceUserSecurityWithGroupAjaxErrorCallback");                 
            }  
        } 



        ////////////////////////////////////
        /////////FORMS FOR GROUP//////////
        var formData = null; 
        function removeFormRestrictionFromGroup(e){
            formData = null; //clear any old values 
            //Get data from parent grid
            var detailGridWrapper = this.wrapper;           
            var parentRow = detailGridWrapper.closest("tr.k-detail-row").prev("tr"); //GET PARENT ROW ELEMENT           
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid"); //GET PARENT GRID ELEMENT          
            var parentModel = parentGrid.dataItem(parentRow); //GET THE PARENT ROW MODEL	         
            if (!('UserGroupsControl' in parentModel)) { ngl.showErrMsg("UserGroupsControl Required", "Parent Row object does not contain property UserGroupsControl.", null); return; } //Verify UserGroupsControl is a column in the parent record        
            if (typeof (parentModel.UserGroupsControl) === 'undefined' || parentModel.UserGroupsControl == null) { ngl.showErrMsg("UserGroupsControl Required", "UserGroupsControl cannot be null", null); return; }               
            //get data from row
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (item) === 'undefined' || item == null || !ngl.isObject(item)) { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; } //Verify the row item is not null
            if (!('FormControl' in item)) { ngl.showErrMsg("FormControl Required", "Row object does not contain property FormControl.", null); return; } //Verify FormControl is a column in the grid     
            if (typeof (item.FormControl) === 'undefined' || item.FormControl == null || item.FormControl == 0) { ngl.showErrMsg("FormControl Required", "FormControl cannot be 0", null); return; } //Verify that FormControl is not null or 0                                                           
            formData = new GenericResult();
            formData.Control = item.FormControl;
            formData.intField1 = parentModel.UserGroupsControl;
            //perform confirmation
            var title = "Remove Screen Restriction";
            ngl.OkCancelConfirmation(
                title,
                "This action will remove this screen from the Restricted List for this Role. Are you sure you want to proceed?",
                400,
                400,
                null,
                "ConfirmRemoveFormRestrictionFromGroup");                                   
        }

        function ConfirmRemoveFormRestrictionFromGroup(iRet){      
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
            //Chose the Ok action
            if(typeof (formData) !== 'undefined' && formData !== null){   
                $.ajax({
                    type: 'POST',
                    url: 'api/LEUserForm/RemoveFormRestrictionFromGroup',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(formData),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {     
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Remove Form Restriction From Group Failure", data.Errors, null); } 
                                else { ngl.readDataSource(oRoleGrid); }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure"); ngl.showErrMsg("Remove Form Restriction From Group Failure", sMsg, null); }
                });
            }
        }


        ////////////////////////////////////
        /////////REPORTS FOR GROUP//////////
        var reportData = null; 
        function removeReportRestrictionFromGroup(e){
            reportData = null; //clear any old values 
            //Get data from parent grid
            var detailGridWrapper = this.wrapper;           
            var parentRow = detailGridWrapper.closest("tr.k-detail-row").prev("tr"); //GET PARENT ROW ELEMENT           
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid"); //GET PARENT GRID ELEMENT          
            var parentModel = parentGrid.dataItem(parentRow); //GET THE PARENT ROW MODEL	         
            if (!('UserGroupsControl' in parentModel)) { ngl.showErrMsg("UserGroupsControl Required", "Parent Row object does not contain property UserGroupsControl.", null); return; } //Verify UserGroupsControl is a column in the parent record        
            if (typeof (parentModel.UserGroupsControl) === 'undefined' || parentModel.UserGroupsControl == null) { ngl.showErrMsg("UserGroupsControl Required", "UserGroupsControl cannot be null", null); return; }               
            //get data from row
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (item) === 'undefined' || item == null || !ngl.isObject(item)) { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; } //Verify the row item is not null
            if (!('ReportControl' in item)) { ngl.showErrMsg("ReportControl Required", "Row object does not contain property ReportControl.", null); return; } //Verify ReportControl is a column in the grid     
            if (typeof (item.ReportControl) === 'undefined' || item.ReportControl == null || item.ReportControl == 0) { ngl.showErrMsg("ReportControl Required", "ReportControl cannot be 0", null); return; } //Verify that ReportControl is not null or 0                                                           
            reportData = new GenericResult();
            reportData.Control = item.ReportControl;
            reportData.intField1 = parentModel.UserGroupsControl;
            //perform confirmation
            var title = "Remove Report Restriction";
            ngl.OkCancelConfirmation(
                title,
                "This action will remove this report from the Restricted List for this Role. Are you sure you want to proceed?",
                400,
                400,
                null,
                "ConfirmRemoveReportRestrictionFromGroup");                                   
        }

        function ConfirmRemoveReportRestrictionFromGroup(iRet){      
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
            //Chose the Ok action
            if(typeof (reportData) !== 'undefined' && reportData !== null){   
                $.ajax({
                    type: 'POST',
                    url: 'api/LEUserReport/RemoveReportRestrictionFromGroup',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(reportData),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {     
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Remove Report Restriction From Group Failure", data.Errors, null); } 
                                else { ngl.readDataSource(oRoleGrid); }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure"); ngl.showErrMsg("Remove Report Restriction From Group Failure", sMsg, null); }
                });
            }
        }


        ////////////////////////////////////
        /////////PROCEDURES FOR GROUP//////////
        var procedureData = null; 
        function removeProcedureRestrictionFromGroup(e){
            procedureData = null; //clear any old values 
            //Get data from parent grid
            var detailGridWrapper = this.wrapper;           
            var parentRow = detailGridWrapper.closest("tr.k-detail-row").prev("tr"); //GET PARENT ROW ELEMENT           
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid"); //GET PARENT GRID ELEMENT          
            var parentModel = parentGrid.dataItem(parentRow); //GET THE PARENT ROW MODEL	         
            if (!('UserGroupsControl' in parentModel)) { ngl.showErrMsg("UserGroupsControl Required", "Parent Row object does not contain property UserGroupsControl.", null); return; } //Verify UserGroupsControl is a column in the parent record        
            if (typeof (parentModel.UserGroupsControl) === 'undefined' || parentModel.UserGroupsControl == null) { ngl.showErrMsg("UserGroupsControl Required", "UserGroupsControl cannot be null", null); return; }               
            //get data from row
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (item) === 'undefined' || item == null || !ngl.isObject(item)) { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; } //Verify the row item is not null
            if (!('ProcedureControl' in item)) { ngl.showErrMsg("ProcedureControl Required", "Row object does not contain property ProcedureControl.", null); return; } //Verify ProcedureControl is a column in the grid     
            if (typeof (item.ProcedureControl) === 'undefined' || item.ProcedureControl == null || item.ProcedureControl == 0) { ngl.showErrMsg("ProcedureControl Required", "ProcedureControl cannot be 0", null); return; } //Verify that ProcedureControl is not null or 0                                                           
            procedureData = new GenericResult();
            procedureData.Control = item.ProcedureControl;
            procedureData.intField1 = parentModel.UserGroupsControl;
            //perform confirmation
            var title = "Remove Procedure Restriction";
            ngl.OkCancelConfirmation(
                title,
                "This action will remove this procedure from the Restricted List for this Role. Are you sure you want to proceed?",
                400,
                400,
                null,
                "ConfirmRemoveProcedureRestrictionFromGroup");                                   
        }

        function ConfirmRemoveProcedureRestrictionFromGroup(iRet){      
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
            //Chose the Ok action
            if(typeof (procedureData) !== 'undefined' && procedureData !== null){   
                $.ajax({
                    type: 'POST',
                    url: 'api/LEUserProcedure/RemoveProcedureRestrictionFromGroup',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(procedureData),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {     
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Remove Procedure Restriction From Group Failure", data.Errors, null); } 
                                else { ngl.readDataSource(oRoleGrid); }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure"); ngl.showErrMsg("Remove Procedure Restriction From Group Failure", sMsg, null); }
                });
            }
        }



        ////////////////////////////////////
        /////////ASSOCIATED COMPANIES//////////
        var deleteUserAdminControl = 0;
        function deleteVisibleCompany(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            deleteUserAdminControl = 0; //clear any old values
            if (typeof (item) !== 'undefined' && item != null) { if ('UserAdminControl' in item) { deleteUserAdminControl = item.UserAdminControl; } }
            if (deleteUserAdminControl == 0) { ngl.showErrMsg("Delete Visible Company Error", "Could not get the primary key from the record", null); return; }
            //get the title and msg
            var childGrid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
            var total = childGrid.dataSource.total();
            var title = "Delete Visible Company";
            var msg = "Deleting the record will move this company to the restricted list. Are you sure you want to proceed?";
            if(total < 2){ 
                title = "Remove Company Restrictions";
                msg = "Removing the last visible company record means the user will no longer have any company restrictions. All companies will be visible to this user. Are you sure you want to proceed?";
            }
            //perform confirmation
            ngl.OkCancelConfirmation(
                title,
                msg,
                400,
                400,
                null,
                "ConfirmDeleteVisibleCompany");
        }

        function ConfirmDeleteVisibleCompany(iRet) {
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action
            //Chose the Ok action
            if (typeof (deleteUserAdminControl) !== 'undefined' && deleteUserAdminControl > 0) {
                var tObj = tObjPG;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.delete("UserAdmin/DELETE", deleteUserAdminControl, tObj, "DeleteVisibleCompanySuccessCallback", "DeleteVisibleCompanyAjaxErrorCallback")
            }
        }

        function openAddVisibleCompWindow(event, usc, userName) {
            if (typeof (usc) === 'undefined' || usc == null || usc == 0) { ngl.showErrMsg("UserSecurityControl Required", "UserSecurityControl cannot be 0", null); return; } //Verify UserSecurityControl is not null or 0
            $("#txtAddVisibleCompUSC").val(usc); //Save the UserSecurityControl to the window
            if (ngl.stringHasValue(userName)) { var c = "<h3>" + userName + "</h3>"; $("#lblVisibleCompUserName").html(c); }
            var childGrid = $(event.target).closest(".k-grid").data("kendoGrid");
            crDetTotal = childGrid.dataSource.total();
            var grid = $("#unrestrictedCompsGrid").data("kendoGrid");
            if (crDetTotal > 0){                
                grid.showColumn("UnboundFieldR");
                grid.hideColumn("UnboundFieldG");
            }
            else{
                grid.showColumn("UnboundFieldG");
                grid.hideColumn("UnboundFieldR");
            }
            $("#unrestrictedCompsGrid").data("kendoGrid").clearSelection();
            $('#unrestrictedCompsGrid').data('kendoGrid').dataSource.read();
            wndAddVisibleComp.center().open();
        }

        function ConfirmAddVisibleComps(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
            //Chose the Ok action
            if(typeof (addVisibleCompsData) !== 'undefined' && ngl.isArray(addVisibleCompsData.intArray) && addVisibleCompsData.intArray.length > 0){   
                $.ajax({
                    type: 'POST',
                    url: 'api/UserAdmin/AddUserAdmins',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(addVisibleCompsData),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {     
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Add Visible Company Failure", data.Errors, null); } 
                                else { 
                                    wndAddVisibleComp.close(); 
                                    ocompanyRestrictionsGrid.dataSource.read(); }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Add Visible Failure", sMsg, null); }
                });
            }
        }
       
        var addVisibleCompsData = null; 
        var crDetTotal = 0;
        function AddVisibleComps(){
            visibleCompsData = null; //clear any old values
            var msg = "This action will make the selected companies visible to the user. Are you sure you want to proceed?";
            var title = "Unrestrict Companies";
            if(crDetTotal < 1){ 
                title = "Apply Company Restrictions";
                msg = "This action will make the selected companies visible to the user. It will also add all other unselected companies to the restricted list. Are you sure you want to proceed?";
            }
            var companyNumbers = $("#unrestrictedCompsGrid").data("kendoGrid").selectedKeyNames(); //get selected users
            if(typeof (companyNumbers) !== 'undefined' && ngl.isArray(companyNumbers) && companyNumbers.length > 0){              
                addVisibleCompsData = new GenericResult();
                addVisibleCompsData.Control = $("#txtAddVisibleCompUSC").val();
                addVisibleCompsData.intArray = companyNumbers;
                //perform confirmation
                ngl.OkCancelConfirmation(
                    title,
                    msg,
                    400,
                    400,
                    null,
                    "ConfirmAddVisibleComps");
            }else { ngl.showWarningMsg("Selection Required", "Please select at least one Company", null); }    
            crDetTotal = 0; //clear any old values
        }

        //End Company Restrictions

        ////////////////////////////////////
        /////////ASSOCIATED Lanes//////////
        var deleteUserLaneControl = 0;
        function deleteVisibleLane(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            deleteUserLaneControl = 0; //clear any old values
            if (typeof (item) !== 'undefined' && item != null) { if ('USLControl' in item) { deleteUserLaneControl = item.USLControl; } }
            if (deleteUserLaneControl == 0) { ngl.showErrMsg("Delete Visible Lane Error", "Could not get the primary key from the record", null); return; }
            //get the title and msg
            var childGrid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
            var total = childGrid.dataSource.total();
            var title = "Delete Visible Lane";
            var msg = "Deleting the record will move this lane to the restricted list. Are you sure you want to proceed?";
            if(total < 2){ 
                title = "Remove Lane Restrictions";
                msg = "Removing the last visible lane record means the user will no longer have any lane restrictions. All lanes will be visible to this user. Are you sure you want to proceed?";
            }
            //perform confirmation
            ngl.OkCancelConfirmation(
                title,
                msg,
                400,
                400,
                null,
                "ConfirmDeleteVisibleLane");
        }

        function ConfirmDeleteVisibleLane(iRet) {
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action
            //Chose the Ok action
            if (typeof (deleteUserLaneControl) !== 'undefined' && deleteUserLaneControl > 0) {
                var tObj = tObjPG;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.delete("UserLane/DELETE", deleteUserLaneControl, tObj, "DeleteVisibleLaneSuccessCallback", "DeleteVisibleLaneAjaxErrorCallback")
            }
        }

        
        function openAddVisibleLaneWindow(event, usc, userName) {
            if (typeof (usc) === 'undefined' || usc == null || usc == 0) { ngl.showErrMsg("UserSecurityControl Required", "UserSecurityControl cannot be 0", null); return; } //Verify UserSecurityControl is not null or 0
            $("#txtAddVisibleLaneUSC").val(usc); //Save the UserSecurityControl to the window
            if (ngl.stringHasValue(userName)) { var c = "<h3>" + userName + "</h3>"; $("#lblVisibleLaneUserName").html(c); }
            var childGrid = $(event.target).closest(".k-grid").data("kendoGrid");
            lrDetTotal = childGrid.dataSource.total();
            var grid = $("#unrestrictedLanesGrid").data("kendoGrid");
            if (lrDetTotal > 0){                
                grid.showColumn("UnboundFieldR");
                grid.hideColumn("UnboundFieldG");
            }
            else{
                grid.showColumn("UnboundFieldG");
                grid.hideColumn("UnboundFieldR");
            }
            $("#unrestrictedLanesGrid").data("kendoGrid").clearSelection();
            $('#unrestrictedLanesGrid').data('kendoGrid').dataSource.read();
            wndAddVisibleLane.center().open();
        }

        function ConfirmAddVisibleLanes(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
            //Chose the Ok action
            if(typeof (addVisibleLanesData) !== 'undefined' && ngl.isArray(addVisibleLanesData.intArray) && addVisibleLanesData.intArray.length > 0){   
                $.ajax({
                    type: 'POST',
                    url: 'api/UserLane/AddUserLanes',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(addVisibleLanesData),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {     
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Add Visible Lane Failure", data.Errors, null); } 
                                else { 
                                    wndAddVisibleLane.close(); 
                                    olaneRestGrid.dataSource.read();
                                }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Add Visible Failure", sMsg, null); }
                });
            }
        }
       
        var addVisibleLanesData = null; 
        var lrDetTotal = 0;
        function AddVisibleLanes(){
            visibleLanesData = null; //clear any old values
            var msg = "This action will make the selectedlLanes visible to the user. Are you sure you want to proceed?";
            var title = "Unrestrict Lanes";
            if(lrDetTotal < 1){ 
                title = "Apply Lane Restrictions";
                msg = "This action will make the selected lanes visible to the user. It will also add all other unselected lanes to the restricted list. Are you sure you want to proceed?";
            }
            var userLaneControls = $("#unrestrictedLanesGrid").data("kendoGrid").selectedKeyNames(); //get selected users
            if(typeof (userLaneControls) !== 'undefined' && ngl.isArray(userLaneControls) && userLaneControls.length > 0){              
                addVisibleLanesData = new GenericResult();
                addVisibleLanesData.Control = $("#txtAddVisibleLaneUSC").val();
                addVisibleLanesData.intArray = userLaneControls;
                //perform confirmation
                ngl.OkCancelConfirmation(
                    title,
                    msg,
                    400,
                    400,
                    null,
                    "ConfirmAddVisibleLanes");
            }else { ngl.showWarningMsg("Selection Required", "Please select at least one Lane", null); }    
            lrDetTotal = 0; //clear any old values
        }
        

        // End Lane Restrictions
        $(function () {
            //wire focus of all numerictextbox widgets on the page
            $("input[type=number]").bind("focus", function () {
                var input = $(this);
                clearTimeout(input.data("selectTimeId")); //stop started time out if any
                var selectTimeId = setTimeout(function() { input.select(); });
                input.data("selectTimeId", selectTimeId);
            }).blur(function(e) {
                clearTimeout($(this).data("selectTimeId")); //stop started timeout
            });
        })

        $(document).ready(function () {  
            var PageMenuTab = <%=PageMenuTab%>;
            
            
            if (control != 0){
                //Set all values to 0
                $("#txtLEControl").val(0);
                $("#txtLEName").val("");
                $("#txtLECompControl").val(0);
                $("#txtCarrierControl").val(0);
               
                   
                //Get the LE for the user
                getLEAdmin(0, resGetLEAdmin);

                //////LE USERS GRID/////////
                var LEUSfilterData = [ 
                    { text: "", value: "None" },
                    { text: "User Name", value: "UserName" },
                    { text: "Role", value: "UserGroupsName" },
                    { text: "Friendly Name", value: "UserFriendlyName" },
                    { text: "First Name", value: "UserFirstName" },
                    { text: "Last Name", value: "UserLastName" },
                    { text: "Title", value: "UserTitle" },
                    { text: "Theme", value: "UserTheme365" },
                    //{ text: "NEXTrackOnly", value: "NEXTrackOnly" }                  
                ];
                $("#ddlLEUSFilters").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: LEUSfilterData,
                    select: function(e) {
                        var name = e.dataItem.text; 
                        var val = e.dataItem.value; 
                        $("#txtLEUSFilterVal").data("kendoMaskedTextBox").value("");
                        $("#dpLEUSFilterFrom").data("kendoDatePicker").value("");
                        $("#dpLEUSFilterTo").data("kendoDatePicker").value("");
                        switch(val){
                            case "None":
                                $("#spLEUSfilterText").hide();
                                $("#spLEUSfilterDates").hide();
                                $("#spLEUSfilterButtons").hide(); 
                                break; 
                            case "NoDatesAvailable":
                                $("#spLEUSfilterText").hide();
                                $("#spLEUSfilterDates").show();
                                $("#spLEUSfilterButtons").show();
                                break;
                            default:
                                $("#spLEUSfilterText").show();
                                $("#spLEUSfilterDates").hide();
                                $("#spLEUSfilterButtons").show();
                                break;
                        }
                    }
                });
                $("#txtLEUSFilterVal").kendoMaskedTextBox(); 
                $("#dpLEUSFilterFrom").kendoDatePicker(); 
                $("#dpLEUSFilterTo").kendoDatePicker(); 
                $("#btnLEUSFilter").kendoButton({icon: "filter",click: function(e) { var dataItem = $("#ddlLEUSFilters").data("kendoDropDownList").dataItem(); if (1 === 0){ var dtFrom = $("#dpLEUSFilterFrom").data("kendoDatePicker").value(); if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}} $("#LEUsersGrid").data("kendoGrid").dataSource.read();}}); 
                $("#btnLEUSClearFilter").kendoButton({icon: "filter-clear",click: function(e) {var dropdownlist = $("#ddlLEUSFilters").data("kendoDropDownList"); dropdownlist.select(0);dropdownlist.trigger("change");$("#txtLEUSFilterVal").data("kendoMaskedTextBox").value("");$("#dpLEUSFilterFrom").data("kendoDatePicker").value(""); $("#dpLEUSFilterTo").data("kendoDatePicker").value(""); $("#spLEUSfilterText").hide(); $("#spLEUSfilterDates").hide(); $("#spLEUSfilterButtons").hide();$("#LEUsersGrid").data("kendoGrid").dataSource.read();}}); 
                $("#txtLEUSFilterVal").data("kendoMaskedTextBox").value("");
                $("#dpLEUSFilterFrom").data("kendoDatePicker").value("");
                $("#dpLEUSFilterTo").data("kendoDatePicker").value("");
                $("#spLEUSfilterText").hide();
                $("#spLEUSfilterDates").hide();
                $("#spLEUSfilterButtons").hide();

                dsLEUsers = new kendo.data.DataSource({
                    serverSorting: true, 
                    serverPaging: true, 
                    pageSize: 10,
                    transport: { 
                        read: function(options) { 
                            var s = new AllFilter();
                            s.filterName = $("#ddlLEUSFilters").data("kendoDropDownList").value();
                            s.filterValue = $("#txtLEUSFilterVal").data("kendoMaskedTextBox").value();
                            s.filterFrom = $("#dpLEUSFilterFrom").data("kendoDatePicker").value();
                            s.filterTo = $("#dpLEUSFilterTo").data("kendoDatePicker").value();
                            s.sortName = $("#txtLEUSSortField").val();s.sortDirection = $("#txtLEUSSortDirection").val();                           
                            s.page = options.data.page;
                            s.skip = options.data.skip;
                            s.take = options.data.take;
                            s.LEAdminControl = $("#txtLEControl").val();

                            $.ajax({ 
                                url: 'api/LEUser/GetRecords/' + s, 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) { 
                                    options.success(data); 
                                    if (data.Errors != null) { 
                                        if (data.StatusCode === 203){ ngl.showErrMsg("Authorization Timeout", data.Errors, null); } 
                                        else { ngl.showErrMsg("Access Denied", data.Errors, null); } 
                                    } 
                                }, 
                                error: function(result) { options.error(result); } 
                            }); 
                        },           
                        parameterMap: function(options, operation) { return options; } 
                    },  
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "UserSecurityControl",
                            fields: {
                                UserSecurityControl: { type: "number" },
                                UserName: { type: "string" },
                                UserUserGroupsControl: { type: "number" },
                                UserGroupsName: { type: "string" },
                                UserEmail: { type: "string" },
                                UserSSOAControl: { type: "number" },
                                //SSOASecurityXrefUserName: { type: "string" },
                                //SSOASecurityXrefPassword: { type: "string" },
                                //SSOASecurityXrefReferenceID: { type: "string" },
                                LEAdminControl: { type: "number" },
                                LEAdminLegalEntity: { type: "string" },
                                LEAdminCompControl: { type: "number" },
                                CompName: { type: "string" },
                                UserFriendlyName: { type: "string" },
                                UserFirstName: { type: "string" },
                                UserMiddleIn: { type: "string" },
                                UserLastName: { type: "string" },
                                UserTitle: { type: "string" },
                                UserTheme365: { type: "string" },
                                NEXTrackOnly: { type: "boolean" }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function(xhr, textStatus, error) { ngl.showErrMsg("Access dsLEUsers Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                });

                $('#LEUsersGrid').kendoGrid({                   
                    dataSource: dsLEUsers,
                    autoBind:false,
                    selectable: "row",
                    pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                    sortable: { mode: "single", allowUnsort: true },
                    sort: function(e) {
                        if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                        if (!e.sort.field) { e.sort.field == ""; }
                        $("#txtLEUSSortDirection").val(e.sort.dir);
                        $("#txtLEUSSortField").val(e.sort.field);
                    },
                    dataBound: function(e) { 
                        var tObj = this; 
                        if (typeof (LEUsersGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(LEUsersGridDataBoundCallBack)) { LEUsersGridDataBoundCallBack(e,tObj); } 
                    },
                    resizable: true,
                    groupable: true, 
                    columns: [
                        {command: [{ className: "cm-icononly-button", name: "EditLEUser", text: "", iconClass: "k-icon k-i-pencil", click: openEditUserWindow },{ className: "cm-icononly-button", name: "DeleteLEUser", text: "", iconClass: "k-icon k-i-trash", click: deleteUser },{name: "Deactivate", text: "Deactivate", iconClass: "k-icon k-i-minus-circle", click: deactivateUser }], title: " ", width: 250 },
                        {field: "UserSecurityControl", title: "Control", hidden: true },
                        {field: "UserName", title: "User Name"},
                        {field: "UserUserGroupsControl", title: "Group Control", hidden: true },
                        {field: "UserGroupsName", title: "Role"},                       
                        {field: "UserEmail", title: "Email", hidden: false },
                        {field: "UserSSOAControl", title: "SSOA Control", hidden: true },                                                                                     
                        {field: "LEAdminControl", title: "LEAdminControl", hidden: true },
                        {field: "LEAdminLegalEntity", title: "Legal Entity", hidden: true },
                        {field: "LEAdminCompControl", title: "LEAdminCompControl", hidden: true },
                        {field: "CompName", title: "Comp Name", hidden: true },
                        {field: "UserFriendlyName", title: "Friendly Name", hidden: false },
                        {field: "UserFirstName", title: "First Name", hidden: false },
                        {field: "UserMiddleIn", title: "Middle In", hidden: true },    
                        {field: "UserLastName", title: "Last Name", hidden: false },
                        {field: "UserTitle", title: "Title", hidden: false },
                        {field: "UserTheme365", title: "Theme 365", hidden: false },
                        {field: "NEXTrackOnly", title: "NEXTrackOnly", hidden: false } 
                        
                        //{command: [{ name: "ExtendFT", text: "Extend Free Trial", iconClass: "k-icon k-i-clock", click: openEditUserWindow }], name: "FreeTrialButtons", title: " ", width: 250, hidden: true }
                    ],
                    detailTemplate: kendo.template($("#LEUsersDetTemplate").html()),
                    detailInit: function(e) {
                        var detailRow = e.detailRow;
                        detailRow.find(".tabstrip").kendoTabStrip({
                            animation: {
                                open: { effects: "fadeIn" }
                            }
                        });                       
                        //Single Sign On Accounts Tab
                        detailRow.find(".ssoAccounts").kendoGrid({
                            dataSource: {
                                serverSorting: true,
                                serverPaging: true,
                                pageSize: 10,
                                transport: { 
                                    read: {
                                        url: "api/SingleSignOn/GetSingleSignOnForUser/" + e.data.UserSecurityControl,
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        type: "GET",
                                        dataType: 'json',
                                    },
                                    parameterMap: function (options, operation) { return options; }
                                },  
                                schema: { 
                                    data: "Data",  
                                    total: "Count", 
                                    model: { 
                                        id: "SSOAXControl",
                                        fields: {                                       
                                            SSOAXControl: { type: "integer", editable: false },
                                            SSOAXUN: { type: "string" },
                                            SSOAXRefID: { type: "string", editable: false },
                                            SSOAControl: { type: "integer", editable: false },
                                            SSOAName: { type: "string", editable: false },
                                            SSOADesc: { type: "string", editable: false },
                                            USC: { type: "integer", editable: false },
                                            UserName: { type: "string", editable: false },
                                        }
                                    }, 
                                    errors: "Errors" 
                                }, 
                                error: function (xhr, textStatus, error) { ngl.showErrMsg("GetSingleSignOnForUser JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); } 
                            },
                            toolbar:[{ name: "Custom", template: '<a class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md"  onclick="return openSingleSignOnAddWindow(event,' + e.data.UserSecurityControl + ",'" + e.data.UserName.replace(/\\/g, "") + "'," + e.data.UserUserGroupsControl + ')"><span class="k-icon k-i-add"></span>Add</a>' }],
                            pageable: { pageSizes: [5, 10, 50, "all"] },
                            resizable: true, 
                            groupable: false, 
                            dataBound: function(e) {
                                ossoAccountsGrid = this;
                            },
                            columns: [
                                { command: [{ className: "cm-icononly-button", name: "EditSingleSignOn", text: "", iconClass: "k-icon k-i-pencil", click: openSingleSignOnEditWindow },{ className: "cm-icononly-button", name: "DeleteSingleSignOn", text: "", iconClass: "k-icon k-i-trash", click: deleteSingleSignOn }], title: " ", width: 100 },
                                {field: "SSOAXControl", title: "Control", hidden: true},
                                {field: "SSOAControl", title: "SSOAControl", hidden: true},
                                {field: "SSOAName", title: "SSOA Name", hidden: true},
                                {field: "SSOADesc", title: "Account", hidden: false},
                                {field: "SSOAXUN", title: "User Name", hidden: false},
                                {field: "SSOAXRefID", title: "Reference ID", hidden: false}                            
                            ]
                        });
                        //Associated Carriers Tab
                        detailRow.find(".associatedCarriers").kendoGrid({
                            dataSource: {
                                serverSorting: true,
                                serverPaging: true,
                                pageSize: 10,
                                transport: { 
                                    read: {
                                        url: "api/UserSecurityCarrier/GetByParent/" + e.data.UserSecurityControl,
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        type: "GET",
                                        dataType: 'json',
                                    },
                                    parameterMap: function (options, operation) { return options; }
                                },  
                                schema: { 
                                    data: "Data",  
                                    total: "Count", 
                                    model: { 
                                        id: "USCControl",
                                        fields: {                                       
                                            USCControl: { type: "integer", editable: false },
                                            USCUserSecurityControl: { type: "integer", editable: false },
                                            USCCarrierControl: { type: "integer" },
                                            USCCarrierNumber: { type: "integer", editable: false },
                                            CarrierName: { type: "string", editable: false },
                                            CarrierSCAC: { type: "string", editable: false },
                                            USCCarrierContControl: { type: "integer" },
                                            CarrierContName: { type: "string", editable: false },
                                            CarrierContactEMail: { type: "string", editable: false },
                                            CarrierContactPhone: { type: "string", editable: false },
                                            CarrierContPhoneExt: { type: "string", editable: false },
                                            USCCarrierAccounting: { type: "string" },
                                            USCModDate: { type: "date", editable: false },
                                            USCModUser: { type: "string", editable: false },
                                            USCUpdated: { type: "string", editable: false }
                                        }
                                    }, 
                                    errors: "Errors" 
                                }, 
                                error: function (xhr, textStatus, error) { ngl.showErrMsg("GetvUserSecurityCarriers JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); } 
                            },          
                            toolbar:[{ name: "Custom", template: '<a class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md"  onclick=" return openAddAssociatedCarrierWindow(event,' + e.data.UserSecurityControl + ",'" + e.data.UserName.replace(/\\/g, "") + "'," + e.data.UserUserGroupsControl + ')"><span class="k-icon k-i-add"></span>Add</a>' }],
                            pageable: { pageSizes: [5, 10, 50, "all"] },
                            resizable: true, 
                            groupable: false, 
                            dataBound: function(e) {
                                oassociatedCarriersGrid = this;
                            },
                            columns: [
                                { command: [{ className: "cm-icononly-button", name: "EditAssociatedCarrier", text: "", iconClass: "k-icon k-i-pencil", click: openEditAssociatedCarrierWindow },{ className: "cm-icononly-button", name: "DeleteAssociatedCarrier", text: "", iconClass: "k-icon k-i-trash", click: deleteAssociatedCarrier }], title: " ", width: 100 },
                                {field: "USCControl", title: "Control", hidden: true},
                                {field: "USCUserSecurityControl", title: "USCUserSecurityControl", hidden: true},
                                {field: "USCCarrierControl", title: "USCCarrierControl", hidden: true},                              
                                {field: "CarrierName", title: "Name" },
                                {field: "USCCarrierNumber", title: "Number" },
                                {field: "CarrierSCAC", title: "SCAC" },
                                {field: "USCCarrierContControl", title: "USCCarrierContControl", hidden: true},
                                {field: "CarrierContName", title: "Contact Name", hidden: false},
                                {field: "CarrierContactEMail", title: "Contact Email", hidden: false},                                 
                                {field: "CarrierContactPhone", title: "Contact Phone", hidden: false},
                                {field: "CarrierContPhoneExt", title: "Contact Phone Ext", hidden: false},
                                {field: "USCCarrierAccounting", title: "Accounting", hidden: false},
                                {field: "USCModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(USCModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #", hidden: false},
                                {field: "USCModUser", title: "Mod User", hidden: false},
                            ]
                        });
                        //Restricted Companies Tab
                        detailRow.find(".companyRestrictions").kendoGrid({
                            dataSource: {
                                serverSorting: true,
                                serverPaging: true,
                                pageSize: 10,
                                transport: { 
                                    read: {
                                        url: "api/UserAdmin/GetByParent/" + e.data.UserSecurityControl,
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        type: "GET",
                                        dataType: 'json',
                                    },
                                    parameterMap: function (options, operation) { return options; }
                                },  
                                schema: { 
                                    data: "Data",  
                                    total: "Count", 
                                    model: { 
                                        id: "UserAdminControl",
                                        fields: {                                       
                                            UserAdminControl: { type: "integer", editable: false },
                                            UserSecurityControl: { type: "integer", editable: false },
                                            CompNumber: { type: "integer", editable: false },
                                            CompControl: { type: "integer", editable: false },
                                            CompName: { type: "string", editable: false },                                            
                                            UserAdminUpdated: { type: "string", editable: false }
                                        }
                                    }, 
                                    errors: "Errors" 
                                }, 
                                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Restricted Companies JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); } 
                            },          
                            toolbar:[{ name: "Custom", template: '<a class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md"  onclick="return openAddVisibleCompWindow(event,' + e.data.UserSecurityControl + ",'" + e.data.UserName.replace(/\\/g, "") + "'" + ')"><span class="k-icon k-i-add"></span>Add</a>' }],
                            pageable: { pageSizes: [5, 10, 50, "all"] },
                            resizable: true, 
                            groupable: false, 
                            noRecords: {
                                template: "<div style='margin-top:5px; margin-bottom:5px;'>No Restrictions</div>"
                            },
                            dataBound: function(e) {
                                ocompanyRestrictionsGrid = this;
                            },
                            columns: [
                                { command: [ { className: "cm-icononly-button", name: "DeleteVisibleCompany", text: "", iconClass: "k-icon k-i-trash", click: deleteVisibleCompany }], title: " ", width: 100 },
                                {field: "UserAdminControl", title: "Control", hidden: true},
                                {field: "UserSecurityControl", title: "User Control", hidden: true},
                                {field: "CompControl", title: "CompControl", hidden: true},                              
                                {field: "CompNumber", title: "Number" },
                                {field: "CompName", title: "Name" },
                                //{field: "CompanyTemplate", title: "Company", template: "<span>#: CompNumber # #: CompName #</span>" },
                                {field: "UnboundField", title: " ", template: "<img src='../Content/NGL/StatusFlagGreen16.png' alt='green flag'>" }
                            ],
                            //rowTemplate: "<div><span>#: CompNumber # #: CompName # <img src='../Content/NGL/StatusFlagGreen16.png' alt='green flag'> </span></div>"
                        });
                        //Restricted Lanes Tab
                        detailRow.find(".laneRestrictions").kendoGrid({
                            dataSource: {
                                serverSorting: true,
                                serverPaging: true,
                                pageSize: 10,
                                transport: { 
                                    read: {
                                        url: "api/UserLane/GetByParent/" + e.data.UserSecurityControl,
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        type: "GET",
                                        dataType: 'json',
                                    },
                                    parameterMap: function (options, operation) { return options; }
                                },  
                                schema: { 
                                    data: "Data",  
                                    total: "Count", 
                                    model: { 
                                        id: "USLControl",
                                        fields: {                                       
                                            USLControl: { type: "integer", editable: false },
                                            USLUserSecurityControl: { type: "integer", editable: false },
                                            USLLaneControl: { type: "integer", editable: false },
                                            LaneNumber: { type: "string", editable: false },
                                            LaneControl: { type: "integer", editable: false },
                                            LaneName: { type: "string", editable: false },   
                                            USLModDate: { type: "string", editable: false },   
                                            USLModUser: { type: "string", editable: false },                                            
                                            USLUpdated: { type: "string", editable: false }
                                        }
                                    }, 
                                    errors: "Errors" 
                                }, 
                                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Restricted Companies JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); } 
                            },          
                            toolbar:[{ name: "Custom", template: '<a class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md"  onclick="return openAddVisibleLaneWindow(event,' + e.data.UserSecurityControl + ",'" + e.data.UserName.replace(/\\/g, "") + "'" + ')"><span class="k-icon k-i-add"></span>Add</a>' }],
                            pageable: { pageSizes: [5, 10, 50, "all"] },
                            resizable: true, 
                            groupable: false, 
                            noRecords: {
                                template: "<div style='margin-top:5px; margin-bottom:5px;'>No Lane Restrictions</div>"
                            },
                            dataBound: function(e) {                                
                                olaneRestGrid = this;
                            },
                            columns: [
                                { command: [ { className: "cm-icononly-button", name: "DeleteVisibleLane", text: "", iconClass: "k-icon k-i-trash", click: deleteVisibleLane }], title: " ", width: 100 },
                                {field: "USLControl", title: "Control", hidden: true},
                                {field: "USLUserSecurityControl", title: "User Control", hidden: true},
                                {field: "USLLaneControl", title: "USLLaneControl", hidden: true},         
                                {field: "LaneControl", title: "LaneControl", hidden: true},                              
                                {field: "LaneNumber", title: "Number" },
                                {field: "LaneName", title: "Name" },
                                //{field: "CompanyTemplate", title: "Company", template: "<span>#: CompNumber # #: CompName #</span>" },
                                {field: "UnboundField", title: " ", template: "<img src='../Content/NGL/StatusFlagGreen16.png' alt='green flag'>" }
                            ],
                            //rowTemplate: "<div><span>#: CompNumber # #: CompName # <img src='../Content/NGL/StatusFlagGreen16.png' alt='green flag'> </span></div>"
                        });
                    }
                });
              


                ////////////wndUserLE///////////////////
                wndUserLE = $("#wndUserLE").kendoWindow({
                    title: "Set User Legal Entity",
                    modal: true,
                    visible: false
                }).data("kendoWindow");
    
                //////////USER LEGAL ENTITY DDL//////////
                dsUserLE = new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: function (options) { return "api/LegalEntity/GetLegalEntityAdmins/"; },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                        },
                        parameterMap: function (options, operation) { return options; }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "LEAdminControl",
                            fields: {
                                LEAdminControl: { type: "number", editable: false },
                                CompName: { type: "string", editable: false },
                                LegalEntity: { type: "string", editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Legal Entity Admins JSON Response Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failuire"), null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                });

                $("#ddlUserLE").kendoDropDownList({
                    //autoBind: false,
                    dataTextField: "LegalEntity",
                    dataValueField: "LEAdminControl",
                    dataSource: dsUserLE,
                });

                
                //////////USER GROUP DDL//////////
                dsUserGroups = new kendo.data.DataSource({
                    serverSorting: true, 
                    serverPaging: true, 
                    pageSize: 10,
                    transport: {
                        read: {
                            url: "api/UserGroup/GetUserGroupsForLE",   
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            type: "GET",
                            dataType: 'json',
                            data: function() {
                                var control = $("#txtLEControl").val();
                                return { id: control };
                            }                       
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
                                Name: { type: "string" }, 
                                Description: { type: "string" }
                            }
                        }, 
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Get User Groups For LE Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                });

                $("#ddlUserGroupsByLE").kendoDropDownList({
                    //autoBind: false,
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: dsUserGroups
                });

                $("#ddlUserGroupsByLE2").kendoDropDownList({
                    //autoBind: false,
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: dsUserGroups
                });

                ////////////wndActivateUser///////////////
                wndActivateUser = $("#wndActivateUser").kendoWindow({
                    title: "Activate User",
                    modal: true,
                    visible: false,
                    actions: ["Minimize", "Maximize", "Close"],
                    close: function(e) {
                        //reset values
                        $("#txtActivateUSC").val(0);
                    }
                }).data("kendoWindow");
                //$("#wndActivateUser").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { activateUser(); });


                //************************BEGIN VISIBLE COMPS SECTION************************
                $("#unrestrictedCompsGrid").kendoGrid({
                    dataSource: {
                        pageSize: 10,
                        transport: {                            
                            read: function(options) { 
                                ////var s = new AllFilter();                                    
                                ////s.page = options.data.page;
                                ////s.skip = options.data.skip;
                                ////s.take = options.data.take;
                                ////s.LEAdminControl = $("#txtAddVisibleCompUSC").val();
                                $.ajax({ 
                                    url: 'api/UserAdmin/GetUnrestrictedCompsByUser/' + $("#txtAddVisibleCompUSC").val(), 
                                    contentType: 'application/json; charset=utf-8', 
                                    dataType: 'json', 
                                    ////data: { filter: JSON.stringify(s) }, 
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
                                id: "CompNumber",
                                fields: {
                                    CompNumber: { type: "number" },
                                    CompControl: { type: "number" },
                                    CompName: { type: "string" }
                                }
                            }
                        }
                    },
                    autoBind: false,
                    pageable: true,
                    scrollable: false,
                    persistSelection: true,
                    sortable: true,
                    columns: [
                        { selectable: true, width: 50 },
                        {field: "CompNumber", title: "CompNumber", hidden: true },
                        {field: "CompControl", title: "CompControl", hidden: true },
                        {field: "CompName", title: "Company"},
                        {field: "UnboundFieldG", title: " ", template: "<img src='../Content/NGL/StatusFlagGreen16.png' alt='red flag'>", hidden: true },
                        {field: "UnboundFieldR", title: " ", template: "<img src='../Content/NGL/StatusFlagRed16.png' alt='red flag'>" }
                    ]
                });

                ////////////wndAddVisibleComp///////////////////
                wndAddVisibleComp = $("#wndAddVisibleComp").kendoWindow({
                    title: "Company Restrictions",
                    modal: true,
                    visible: false,   
                    height: '80%',
                    minWidth: 300,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                    close: function(e) {                     
                        $("#txtAddVisibleCompUSC").val(0); //reset values
                    }
                }).data("kendoWindow");
                $("#wndAddVisibleComp").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { AddVisibleComps(); });                 
                //*************************END VISIBLE COMPS SECTION*************************

                //************************BEGIN VISIBLE LANES SECTION************************
                $("#unrestrictedLanesGrid").kendoGrid({
                    dataSource: {
                        pageSize: 10,
                        transport: {                            
                            read: function(options) { 
                                ////var s = new AllFilter();                                    
                                ////s.page = options.data.page;
                                ////s.skip = options.data.skip;
                                ////s.take = options.data.take;
                                ////s.LEAdminControl = $("#txtAddVisibleCompUSC").val();
                                $.ajax({ 
                                    url: 'api/UserLane/GetUnrestrictedLanesByUser/' + $("#txtAddVisibleLaneUSC").val(), 
                                    contentType: 'application/json; charset=utf-8', 
                                    dataType: 'json', 
                                    ////data: { filter: JSON.stringify(s) }, 
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
                                id: "LaneControl",
                                fields: {
                                    LaneNumber: { type: "string" },
                                    LaneControl: { type: "number" },
                                    LaneName: { type: "string" }
                                }
                            }
                        }
                    },
                    autoBind: false,
                    pageable: true,
                    scrollable: false,
                    persistSelection: true,
                    sortable: true,
                    columns: [
                        { selectable: true, width: 50 },
                        {field: "LaneNumber", title: "Lane Number", hidden: true },
                        {field: "LaneControl", title: "Lane Control", hidden: true },
                        {field: "LaneName", title: "Lane Name"},
                        {field: "UnboundFieldG", title: " ", template: "<img src='../Content/NGL/StatusFlagGreen16.png' alt='red flag'>", hidden: true },
                        {field: "UnboundFieldR", title: " ", template: "<img src='../Content/NGL/StatusFlagRed16.png' alt='red flag'>" }
                    ]
                });


                ////////////wndAddVisibleLane///////////////////
                wndAddVisibleLane = $("#wndAddVisibleLane").kendoWindow({
                    title: "Lane Restrictions",
                    modal: true,
                    visible: false,   
                    height: '80%',
                    minWidth: 300,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                    close: function(e) {                     
                        $("#txtAddVisibleLaneUSC").val(0); //reset values
                    }
                }).data("kendoWindow");
                $("#wndAddVisibleLane").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { AddVisibleLanes(); });                 
                //*************************END VISIBLE LANES SECTION*************************
            }
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");                
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }
        });
    </script>
       <style>      
           .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
                  
           .k-grid td{
           overflow: hidden;
                text-overflow: ellipsis;
                white-space: nowrap;
        }
           .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }
                   
           .k-grid tbody .k-grid-Edit { min-width: 0; }
         
           .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }   
    </style>
        
      </div> 
     </body>
</html>
