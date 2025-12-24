<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LECarrierMaint.aspx.cs" Inherits="DynamicsTMS365.LECarrierMaint" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS LE Carrier Maintenance</title>
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
                            <div id="pageContent" class="pane-content">
                                <% Response.Write(PageErrorsOrWarnings); %>                                
                                <div id="divTitleLE"></div>
                                <!-- begin Page Content -->
                                <% Response.Write(FastTabsHTML); %>    
                                <!-- End Page Content -->
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
          
          <% Response.WriteFile("~/Views/ChangeLEDialog.html"); %>
          <% Response.WriteFile("~/Views/CarrierSettingsWindow44.html"); %>          
          <% Response.WriteFile("~/Views/AccessorialSettingsWindow44.html"); %>
          
          <%--wndCopyLEConfig--%>
          <div id="wndCopyLEConfig">
              <div id="divCopyToLE"></div>
              <div><strong>Import Configuration From</strong></div>
              <div><input id="ddlLEToCopy" style="width: 200px;" /></div>
              <div style="margin-top: 5px;"><button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" id="btnCopyLEOk" onclick="btnCopyLEOk_Click();">Import</button></div>            
          </div>
          
          <%--wndBillToComp--%>
          <div id="wndBillToComp">
              <div id="lblCarrierName"></div>
              <div><input id="ddlBillToComps" style="width:200px;" /></div>
              <div style="margin-top:5px;"><button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" onclick="btnSetBillToComp_Click();">Set</button></div>            
          </div>
          
          <%--wndCopyLECarConfig--%>
          <div id="wndCopyLECarConfig">
              <div style="margin-bottom:5px;"><strong>Copy From: </strong><strong id="lblCopyFromLECarName"></strong></div>
              <div>
                  <strong>Copy To: </strong>
                  <div id="copyToGrid"></div>
              </div>
              <div style="margin-top:5px;"><button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" id="btnCopyOk" onclick="btnCopyOk_Click();">Copy Config</button></div>                
              <input id="txtCopyFromLECarControl" type="hidden" />           
          </div>
          
          <%--wndInsertDefaultAccessorials--%>
          <div id="wndInsertDefaultAccessorials">
              <div>            
                  <strong>Update the following Carrier(s) with default accessorials: </strong>            
                  <div id="insertToGrid"></div>    
              </div>     
              <div style="margin-top:5px;"><button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" id="btnInsertOk" onclick="btnInsertOk_Click();">Insert Config</button></div>               
          </div>
 
                   
          <% Response.Write(PageTemplates); %>
          <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
          <% Response.Write(AuthLoginNotificationHTML); %>
          <% Response.WriteFile("~/Views/HelpWindow.html"); %>
          <script>
              <% Response.Write(ADALPropertiesjs); %>
              var PageControl = '<%=PageControl%>';
              var oLECarSetGrid = null;
              var tObjPG = this;
              var pgLEControl = 0;
              var pgLEName = "";
              var tObj = this;
              var tPage = this;
              var iLECarPK = 0;
              var selectedRowCarrierName = "";
              var leCarSetGridSelectedRowDataItem; 

        <% Response.Write(NGLOAuth2); %>

              <% Response.Write(PageCustomJS); %>

              var dsAccessorials = kendo.data.DataSource;
              var wndCarSet = kendo.ui.Window;
              var wndAccessorial = kendo.ui.Window;      
              var wndCopyLEConfig = kendo.ui.Window;
              var wndBillToComp = kendo.ui.Window;
              var wndCopyLECarConfig = kendo.ui.Window;
              var wndInsertDefaultAccessorials = kendo.ui.Window;

              //***************** Widgets ******************************
              //~~~~~~~~~~ChangeLEDialogCtrl~~~~~~~~~~~~~~~~~~~
              var wndChangeLEDialog = kendo.ui.Window;
              var oChangeLEDialogCtrl = new ChangeLEDialogCtrl()
              //Widgit call backs
              function oChangeLEDialogSaveCB(results){
                  if (typeof (results) !== 'undefined' && results != null) { 
                      if ('LEAdminControl' in results) { pgLEControl = results.LEAdminControl; }
                      if ('LegalEntity' in results) { pgLEName = results.LegalEntity; }             
                      $("#divTitleLE").html("<h2>" + pgLEName + "</h2>");               
                      //We need to reset the page before refresh to avoid problematic behavior since we are completely changing the data returned (not just filtering or sorting from one retured record set)
                      oLECarSetGrid.dataSource.page(1);
                      refreshLECarSetGrid();
                  }
              }    
              //functions
              function changeLegalEntity() { oChangeLEDialogCtrl.show(); }


              //************* Primary Key Functions ********************
              function saveLECarPK() {
                  try {
                      var row = oLECarSetGrid.select();
                      if (typeof (row) === 'undefined' || row == null) { ngl.showValidationMsg("Carrier Record Required", "Please select a Carrier to continue", tPage); return false; }                   
                      //Get the dataItem for the selected row
                      leCarSetGridSelectedRowDataItem = oLECarSetGrid.dataItem(row);
                      if (typeof (leCarSetGridSelectedRowDataItem) === 'undefined' || leCarSetGridSelectedRowDataItem == null) { ngl.showValidationMsg("Carrier Record Required", "Please select a Carrier to continue", tPage); return false; }  
                      if ("LECarControl" in leCarSetGridSelectedRowDataItem){        
                          //save the name of the selected carrier
                          if("CarrierName" in leCarSetGridSelectedRowDataItem){selectedRowCarrierName = leCarSetGridSelectedRowDataItem.CarrierName;}else{selectedRowCarrierName = "";}
                          iLECarPK = leCarSetGridSelectedRowDataItem.LECarControl;
                          console.log("iLECarPK: " + iLECarPK);
                          var setting = { name: 'pk', value: iLECarPK.toString() };                         
                          var oCRUDCtrl = new nglRESTCRUDCtrl();
                          var blnRet = oCRUDCtrl.update("LegalEntityCarrier/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                          return true;
                      } else { ngl.showValidationMsg("Carrier Record Required", "Invalid Record Identifier, please select a Carrier to continue", tPage); return false; }           
                  } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
              }

              function isLECarSelected() {
                  console.log("isLECarSelected");
                  if (typeof (iLECarPK) === 'undefined' || iLECarPK === null || iLECarPK === 0) { return saveLECarPK(); }
                  return true;
              }

              //************* Action Menu Functions ********************
              function execActionClick(btn, proc) {
                  //alert("action");
                  //console.log(btn);
                  //console.log(proc);
                  if(btn.id == "btnChangeLE"){ changeLegalEntity(); }
                  else if(btn.id == "btnAddCarrier"){ openCarSetAddWindow(); }
                  else if(btn.id == "btnAddCarAccessorial44"){ if (isLECarSelected() === true) {openAccessorialAddWindow();} }           
                  else if(btn.id == "btnCopyLEConfig"){ openCopyLEConfigWnd(); }
                  else if(btn.id == "btnEditBillToComp44"){ if (isLECarSelected() === true) {openEditBillToCompWindow();} }
                  else if(btn.id == "btnCopyLECarFeeConfig"){ if (isLECarSelected() === true) {openCopyLECarFeeWindow();} }
                  else if(btn.id == "btnInsertDefaultAccessorials"){ openInsertDefaultFeeWindow(); }
                  else if(btn.id == "btnCarrierFuel") {location.href="CarrierFuel"}
                  else if (btn.id == "btnMngLECarAssrls" ){ 
                      //Added By LVV on 6/18/20
                      if (isLECarSelected() === true){
                          if (typeof (tPage["wdgtMngLECarAccessorialsWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtMngLECarAccessorialsWndDialog"])){
                              tPage["wdgtMngLECarAccessorialsWndDialog"].show(iLECarPK);                
                          } else{alert("Missing HTML Element (wdgtMngLECarAccessorialsWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                      }
                  }
                  else if(btn.id == "btnRefresh" ){ refresh(); }  
                  else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
                  else if(btn.id=="btnOpenCarEDI"){
                      if (isLECarSelected() === true) {
                          var setting = {name:'pk', value: iLECarPK.toString()};
                          var oCRUDCtrl = new nglRESTCRUDCtrl();
                          var blnRet = oCRUDCtrl.update("CarrierEDI/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);             
                          location.href = "CarrierEDI";
                      }
                  }
                  else if(btn.id=="btnCarrierEquip"){
                      if (isLECarSelected() === true) {
                          var setting = {name:'pk', value: iLECarPK.toString()};
                          var oCRUDCtrl = new nglRESTCRUDCtrl();
                          var blnRet = oCRUDCtrl.update("CarrierEquipment/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);             
                          location.href = "CarrierEquipment";
                      }
                  }
                  else if(btn.id=="btnOpenCarFuel"){location.href="CarrierFuelIndex";}
                  else if(btn.id=="btnOpenNatZones"){location.href="NatFuelZones";}
                  else if (btn.id == "btnOpenMstrCar") { location.href = "MasterCarriers"; } //Added By LVV on 10/19/20 for v-8.3.0.001 Task #Task #20201020161708 - Add Master Carrier Page
                  else if (btn.id == "btnOpenCarProNbr") {
                      if (isLECarSelected() === true) {
                          var setting = { name: 'pk', value: iLECarPK.toString() };
                          var oCRUDCtrl = new nglRESTCRUDCtrl();
                          var blnRet = oCRUDCtrl.update("CarrierProNumber/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                          location.href = "CarrierProNumbers";
                      }
                  }
              }
              

              //************* Call Back Functions **********************        
              var blnLECarSetGridChangeBound = false;
              function LECarSetGridDataBoundCallBack(e,tGrid){           
                  oLECarSetGrid = tGrid;
                  if (blnLECarSetGridChangeBound == false){
                      oLECarSetGrid.bind("change", saveLECarPK);
                      blnLECarSetGridChangeBound = true;
                  }
              }

              //Added By LVV on 6/18/20
              function LECarAssrlGridGetStringData(s){
                  s.ParentControl = iLECarPK;
                  return '';
              }
       
              function LECarSetGridGetStringData(s){ 
                  s.LEAdminControl = pgLEControl;
                  return '';
              }

              function execBeforecontactsInsert(e,fk,w){      
                  //We need to populate the CarrierContCarrierControl in the new CarrierCont record so we get that from the header row
                  var parentRow = $(e.currentTarget).closest("tr.k-detail-row").prev("tr"); // GET PARENT ROW ELEMENT    
                  var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid");            
                  var parentModel = parentGrid.dataItem(parentRow); // GET THE PARENT ROW MODEL           
                  var carrierControl = parentModel.CarrierControl; // ACCESS THE PARENT ROW MODEL ATTRIBUTES
                  if (!carrierControl) { alert("No Carrier Control Field - cannot insert record"); return false;}
                  return w.SetFieldDefault("CarrierContCarrierControl",carrierControl.toString());            
              }

              /* Summary - Validates a custom javascript object by verifying obj is not null and is an object, then tests to see if it is an instance of expectedType. If expectedType is null then it is ignored */
              function validateCustomJSObject(obj, expectedType){
                  var blnReturn = false;
                  if (typeof (obj) !== 'undefined' && obj != null && ngl.isObject(obj)) {
                      if(expectedType != null){
                          if (obj instanceof expectedType){ blnReturn = true; }
                      } else{ blnReturn = true; }  
                  }
                  return blnReturn;
              }

              /* Summary - Validates a javascript array by verifying obj is not null and is an array, then  if index is not null tests to see if the index exists. If index is null then it is ignored */
              function validateJSArray(obj, index){
                  var blnReturn = false;
                  if (typeof (obj) !== 'undefined' && obj != null && ngl.isArray(obj)) { 
                      if(index != null){
                          if(!(!obj[index])){ blnReturn = true; }
                      } else{ blnReturn = true; }                
                  }
                  return blnReturn;
              }
        
        
              function contactsCB(oResults){
                  try {
                      if (validateCustomJSObject(oResults, nglEventParameters)) { 
                          if (oResults.source === 'readSuccessCallback' && oResults.msg === 'Success') {
                              if (validateCustomJSObject(oResults.widget, NGLEditWindCtrl)) {
                                  if (oResults.widget.sNGLCtrlName === 'wdgtcontactsEdit') {                                   
                                      if (!validateJSArray(oResults.data, 0)) { ngl.showErrMsg("Warning - contactsCB()", "There was a problem getting the returnedDataItem", null); return; } 
                                      var returnedDataItem = oResults.data[0]; //the object returned by the read aka the method called to populate edit popup window                                
                                      var carrierControl = 0;
                                      if (validateCustomJSObject(returnedDataItem, null)) { carrierControl = returnedDataItem.CarrierContCarrierControl; } //Get the CarrierControl from the results object (oResults)                              
                                      //Get a reference to the parent grid and make sure it is not null
                                      var grid = $("#" + oResults.widget.ParentIDKey).data("kendoNGLGrid");
                                      if (typeof (grid) === 'undefined' || grid == null || !ngl.isObject(grid)) { return; }                       
                                      //loop through the parent datasource to get a reference to the parent row data item where parent.CarrierControl = detailRecord.CarrierContCarrierControl
                                      var parentDS = grid.dataSource.data();
                                      var headerDataItem = null;                                           
                                      $.each(parentDS, function (index, item) {
                                          if (item.CarrierControl === carrierControl) { headerDataItem = item; return; }
                                      });
                                      //Get the LECarControl from the parent grid header record and add it to the detail item data if detailRecord.CarrierContLECarControl = 0
                                      if (validateCustomJSObject(headerDataItem, null)) { 
                                          if ('LECarControl' in headerDataItem && headerDataItem.LECarControl !== 0) { 
                                              //wdgtcontactsEdit.data = data used in the edit window --> set the CarrierContLECarControl to the header LECarControl
                                              if(wdgtcontactsEdit.data.CarrierContLECarControl === 0){ wdgtcontactsEdit.data.CarrierContLECarControl = headerDataItem.LECarControl; }                               
                                          }      
                                      }            
                                  }
                              } else{ ngl.showErrMsg("Warning - contactsCB()", "oResults.widget is expected to be an instance of NGLEditWindCtrl", null); }
                          }                   
                      } else{ ngl.showErrMsg("Warning - contactsCB()", "oResults are expected to be an instance of nglEventParameters", null); }
                  } catch (err) { ngl.showErrMsg(err.name, err.message, null); }
              }

              function savePostPageSettingSuccessCallback(results){
                  //for now do nothing when we save the pk
              }
              function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
                  //for now do nothing when we save the pk          
              }

              function DeleteLECarrierSuccessCallback(data) {
                  var oResults = new nglEventParameters();
                  oResults.source = "DeleteLECarrierSuccessCallback";
                  oResults.widget = this;
                  oResults.msg = 'Failed'; //set default to Failed     
                  this.rData = null;
                  var tObj = this;
                  try {
                      if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                          if (ngl.stringHasValue(data.Errors)) {
                              oResults.error = new Error();
                              oResults.error.name = "Delete LE Carrier Failure";
                              oResults.error.message = data.Errors;
                              ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                          }else{ oResults.msg = 'Success'; }
                      }
                  } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }            
                  if (ngl.isFunction(refreshLECarSetGrid)) { refreshLECarSetGrid(); }
              }
              function DeleteLECarrierAjaxErrorCallback(xhr, textStatus, error) {
                  var oResults = new nglEventParameters();
                  var tObj = this;
                  oResults.source = "DeleteLECarrierAjaxErrorCallback";
                  oResults.widget = this;
                  oResults.msg = 'Failed'; //set default to Failed  
                  oResults.error = new Error();
                  oResults.error.name = "Delete LE Carrier Failure"
                  oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
                  ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);           
                  if (ngl.isFunction(refreshLECarSetGrid)) { refreshLECarSetGrid(); }
              }

              function PostLECarBillToCompSuccessCallback(data) {
                  var oResults = new nglEventParameters();
                  oResults.source = "PostLECarBillToCompSuccessCallback";
                  oResults.widget = this;
                  oResults.msg = 'Failed'; //set default to Failed     
                  this.rData = null;
                  var tObj = this;
                  try {
                      if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                          if (ngl.stringHasValue(data.Errors)) {
                              oResults.error = new Error();
                              oResults.error.name = "Save Carrier Billing Company Failure";
                              oResults.error.message = data.Errors;
                              ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                          }else{ oResults.msg = 'Success'; wndBillToComp.close(); }
                      }
                  } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }           
                  if (ngl.isFunction(refreshLECarSetGrid)) { refreshLECarSetGrid(); }
              }
              function PostLECarBillToCompAjaxErrorCallback(xhr, textStatus, error) {
                  var oResults = new nglEventParameters();
                  var tObj = this;
                  oResults.source = "PostLECarBillToCompAjaxErrorCallback";
                  oResults.widget = this;
                  oResults.msg = 'Failed'; //set default to Failed  
                  oResults.error = new Error();
                  oResults.error.name = "Save Carrier Billing Company Failure"
                  oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
                  ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);          
                  if (ngl.isFunction(refreshLECarSetGrid)) { refreshLECarSetGrid(); }
              }


              //************* Page Functions **********************
              function refreshLECarSetGrid() { ngl.readDataSource(oLECarSetGrid); }

              function refresh(){
                  ngl.readDataSource(oLECarSetGrid);
                  $('#ddlUnassignedLECarriers').data('kendoDropDownList').dataSource.read();
                  $('#ddlAssignedLECarriers').data('kendoDropDownList').dataSource.read();
              }

              var GetLEAdminNotAsyncCB = function (data) {
                  pgLEControl = data[0].LEAdminControl;
                  pgLEName = data[0].LegalEntity;      
                  $("#divTitleLE").html("<h2>" + pgLEName + "</h2>"); 
              }

              ////////////////////////////////////
              ////////CARRIER SETTINGS///////////
              function openCarSetAddWindow(){            
                  $("#wndCarSet").data("kendoWindow").title("Add New Carrier"); //Change Title of Window     
                  if (pgLEControl == 0) { return; } //Validate LE  
                  //Set LE label
                  var l = "<h3>Legal Entity: " + pgLEName + "</h3>";
                  $("#lblLE").html(l);
                  //Carrier ddl
                  $("#txtIsEdit").val(0);
                  $("#divUnassignedLECarriers").show();
                  $("#divAssignedLECarriers").hide();
                  var dropdownlist = $("#ddlUnassignedLECarriers").data("kendoDropDownList");
                  dropdownlist.readonly(false);
                  dropdownlist.select(0);
                  //DispatchType ddl
                  var ddl = $("#ddlDispatchType").data("kendoDropDownList");
                  ddl.select(function(dataItem) { return dataItem.Control === 1; });
                  ddl.readonly(false);
                  //Reset values
                  $("#chkRateShopOnly").prop('checked', false);
                  $("#chkAPIDispatching").prop('checked', false);
                  $("#chkAPIStatusUpdates").prop('checked', false);
                  $("#chkShowAuditFailReason").prop('checked', false);
                  $("#chkShowPendingFeeFailReason").prop('checked', false);
                  $("#txtBillToCompControl").val(0);
                  $("#txtCarrierAccountRef").data("kendoMaskedTextBox").value("");
                  $("#txtCarrierSCAC").data("kendoMaskedTextBox").value("");
                  //Expired Loads
                  $("#chkLECarUseDefault").prop('checked', true);
                  $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").value(0);
                  $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").value("");
                  $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").value("");  
                  $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").enable(false);
                  $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").enable(false);           
                  $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").enable(false); 
                  //Billing Address
                  $("#txtLECarBillingAddress1").data("kendoMaskedTextBox").value("");
                  $("#txtLECarBillingAddress2").data("kendoMaskedTextBox").value("");
                  $("#txtLECarBillingAddress3").data("kendoMaskedTextBox").value("");
                  $("#txtLECarBillingCity").data("kendoMaskedTextBox").value("");
                  $("#txtLECarBillingState").data("kendoMaskedTextBox").value("");
                  $("#txtLECarBillingZip").data("kendoMaskedTextBox").value("");
                  $("#txtLECarBillingCountry").data("kendoMaskedTextBox").value("");
                  //Modified by RHR for v-8.2.0.117 on 07/17/2019 Added LECarAllowLTLConsolidation
                  $("#chkLECarAllowLTLConsolidation").prop('checked', false);
                  // Begin Modified by RHR for v-8.4.0.002 on 04/27/2021 Added Fields for Token Accept Reject                  
                  $("#chkLECarAllowCarrierAcceptRejectByEmail").prop('checked', false);
                  $("#chkLECarCarrierAuthCarrierAcceptRejectByEmail").prop('checked', false);
                  $("#txtLECarCarrierAuthCarrierAcceptRejectExpMin").data("kendoNumericTextBox").value(0);
                  // End  Modified by RHR for v-8.4.0.002 on 04/27/2021 Added Fields for Token Accept Reject 
                  wndCarSet.center().open();
              }

              function openCarSetEditWindow(e) {
                  var item = this.dataItem($(e.currentTarget).closest("tr")); //Get the item for the row         
                  $("#wndCarSet").data("kendoWindow").title("Edit Carrier Settings"); //Change Title of Window 
                  //Set LE label
                  var l = "<h3>Legal Entity: " + pgLEName + "</h3>";
                  $("#lblLE").html(l);
                  //Carrier ddl
                  $("#txtIsEdit").val(1);
                  $("#divAssignedLECarriers").show();
                  $("#divUnassignedLECarriers").hide();         
                  var dropdownlist = $("#ddlAssignedLECarriers").data("kendoDropDownList");
                  dropdownlist.select(function(dataItem) { return dataItem.Control === item.CarrierControl; });
                  dropdownlist.readonly();
                  //DispatchType ddl
                  var ddl = $("#ddlDispatchType").data("kendoDropDownList");
                  ddl.select(function(dataItem) { return dataItem.Control === item.DispatchTypeControl; });
                  ddl.readonly();
                  //set values from item
                  if (item.RateShopOnly) { $("#chkRateShopOnly").prop('checked', true); } else{ $("#chkRateShopOnly").prop('checked', false); }
                  if (item.APIDispatching) { $("#chkAPIDispatching").prop('checked', true); } else{ $("#chkAPIDispatching").prop('checked', false); }         
                  if (item.APIStatusUpdates) { $("#chkAPIStatusUpdates").prop('checked', true); } else{ $("#chkAPIStatusUpdates").prop('checked', false); }         
                  if (item.ShowAuditFailReason) { $("#chkShowAuditFailReason").prop('checked', true); } else{ $("#chkShowAuditFailReason").prop('checked', false); }        
                  if (item.ShowPendingFeeFailReason) { $("#chkShowPendingFeeFailReason").prop('checked', true); } else{ $("#chkShowPendingFeeFailReason").prop('checked', false); }        
                  $("#txtBillToCompControl").val(item.BillToCompControl);
                  $("#txtCarrierAccountRef").data("kendoMaskedTextBox").value(item.CarrierAccountRef);
                  $("#txtCarrierSCAC").data("kendoMaskedTextBox").value(item.CarrierSCAC);
                  //Expired Loads
                  if (item.LECarUseDefault) { 
                      $("#chkLECarUseDefault").prop('checked', true); 
                      $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").value(0);
                      $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").value("");
                      $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").value("");  
                      $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").enable(false);
                      $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").enable(false);           
                      $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").enable(false);
                  } else{ 
                      $("#chkLECarUseDefault").prop('checked', false); 
                      $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").value(item.LECarCarrierAcceptLoadMins);
                      $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").value(item.LECarExpiredLoadsTo);
                      $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").value(item.LECarExpiredLoadsCc);  
                      $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").enable(true);
                      $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").enable(true);           
                      $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").enable(true);
                  }         
                  //Billing Address
                  $("#txtLECarBillingAddress1").data("kendoMaskedTextBox").value(item.LECarBillingAddress1);
                  $("#txtLECarBillingAddress2").data("kendoMaskedTextBox").value(item.LECarBillingAddress2);
                  $("#txtLECarBillingAddress3").data("kendoMaskedTextBox").value(item.LECarBillingAddress3);
                  $("#txtLECarBillingCity").data("kendoMaskedTextBox").value(item.LECarBillingCity);
                  $("#txtLECarBillingState").data("kendoMaskedTextBox").value(item.LECarBillingState);
                  $("#txtLECarBillingZip").data("kendoMaskedTextBox").value(item.LECarBillingZip);
                  $("#txtLECarBillingCountry").data("kendoMaskedTextBox").value(item.LECarBillingCountry);
                  //Modified by RHR for v-8.2.0.117 on 07/17/2019 Added LECarAllowLTLConsolidation          
                  if (item.LECarAllowLTLConsolidation == true) { $("#chkLECarAllowLTLConsolidation").prop('checked', true); } else{ $("#chkLECarAllowLTLConsolidation").prop('checked', false); }    
                  // Begin Modified by RHR for v-8.4.0.002 on 04/27/2021 Added Fields for Token Accept Reject 
                  if (item.LECarAllowCarrierAcceptRejectByEmail == true) { $("#chkLECarAllowCarrierAcceptRejectByEmail").prop('checked', true); } else{ $("#chkLECarAllowCarrierAcceptRejectByEmail").prop('checked', false); }    
                  if (item.LECarCarrierAuthCarrierAcceptRejectByEmail == true) { $("#chkLECarCarrierAuthCarrierAcceptRejectByEmail").prop('checked', true); } else{ $("#chkLECarCarrierAuthCarrierAcceptRejectByEmail").prop('checked', false); }    
                  //if(item.LECarCarrierAuthCarrierAcceptRejectExpMin == null) { $("#txtLECarCarrierAuthCarrierAcceptRejectExpMin").data("kendoNumericTextBox").value(0);
                  //} else {
                  //    $("#txtLECarCarrierAuthCarrierAcceptRejectExpMin").data("kendoNumericTextBox").value(item.LECarCarrierAuthCarrierAcceptRejectExpMin);
                  //}
                 
                  // End  Modified by RHR for v-8.4.0.002 on 04/27/2021 Added Fields for Token Accept Reject 
                  wndCarSet.center().open();
              }

              function SaveCarrierSetting() {
                 
                  var otmp = $("#focusCancel").focus();
                  var tfRateShopOnly = false;
                  var tfAPIDispatching = false;
                  var tfAPIStatusUpdates = false;
                  var tfShowAuditFailReason = false;
                  var tfShowPendingFeeFailReason = false;
                  var tfLECarUseDefault = false;
                  if ($('#chkRateShopOnly').is(":checked")) { tfRateShopOnly = true; }             
                  if ($('#chkAPIDispatching').is(":checked")) { tfAPIDispatching = true; }  
                  if ($('#chkAPIStatusUpdates').is(":checked")) { tfAPIStatusUpdates = true; }
                  if ($('#chkShowAuditFailReason').is(":checked")) { tfShowAuditFailReason = true; }
                  if ($('#chkShowPendingFeeFailReason').is(":checked")) { tfShowPendingFeeFailReason = true; }
                  if ($('#chkLECarUseDefault').is(":checked")) { tfLECarUseDefault = true; }
            
                  var item = new CarrierDispatchSettings();
                  item.LEAdminControl = pgLEControl;
                  var diACar = $("#ddlAssignedLECarriers").data("kendoDropDownList").dataItem();
                  var diUACar = $("#ddlUnassignedLECarriers").data("kendoDropDownList").dataItem();
                  if($("#txtIsEdit").val() === "1"){ item.CarrierControl = diACar.Control; }else{ item.CarrierControl = diUACar.Control; }           
                  var dataItemDT = $("#ddlDispatchType").data("kendoDropDownList").dataItem();
                  item.DispatchTypeControl = dataItemDT.Control;
                  item.RateShopOnly = tfRateShopOnly;
                  item.APIDispatching = tfAPIDispatching;
                  item.APIStatusUpdates = tfAPIStatusUpdates;
                  item.ShowAuditFailReason = tfShowAuditFailReason;
                  item.ShowPendingFeeFailReason = tfShowPendingFeeFailReason;
                  item.BillToCompControl = $("#txtBillToCompControl").val();
                  item.CarrierAccountRef = $("#txtCarrierAccountRef").data("kendoMaskedTextBox").value();
                  //Expired Loads
                  item.LECarUseDefault = tfLECarUseDefault;
                  item.LECarExpiredLoadsTo = $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").value();
                  item.LECarExpiredLoadsCc = $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").value(); 
                  item.LECarCarrierAcceptLoadMins = $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").value(); 
                  //Billing Address
                  item.LECarBillingAddress1 = $("#txtLECarBillingAddress1").data("kendoMaskedTextBox").value();
                  item.LECarBillingAddress2 = $("#txtLECarBillingAddress2").data("kendoMaskedTextBox").value();
                  item.LECarBillingAddress3 = $("#txtLECarBillingAddress3").data("kendoMaskedTextBox").value();
                  item.LECarBillingCity = $("#txtLECarBillingCity").data("kendoMaskedTextBox").value();
                  item.LECarBillingState = $("#txtLECarBillingState").data("kendoMaskedTextBox").value();
                  item.LECarBillingZip = $("#txtLECarBillingZip").data("kendoMaskedTextBox").value();
                  item.LECarBillingCountry = $("#txtLECarBillingCountry").data("kendoMaskedTextBox").value();
                  //Modified by RHR for v-8.2.0.117 on 07/17/2019 Added LECarAllowLTLConsolidation  
                  if ($('#chkLECarAllowLTLConsolidation').is(":checked")) { item.LECarAllowLTLConsolidation = true; } else { item.LECarAllowLTLConsolidation = false; }
                  // Begin Modified by RHR for v-8.4.0.002 on 04/27/2021 Added Fields for Token Accept Reject 
                  if ($('#chkLECarAllowCarrierAcceptRejectByEmail').is(":checked")) { item.LECarAllowCarrierAcceptRejectByEmail = true; } else { item.LECarAllowCarrierAcceptRejectByEmail = false; }
                  if ($('#chkLECarCarrierAuthCarrierAcceptRejectByEmail').is(":checked")) { item.LECarCarrierAuthCarrierAcceptRejectByEmail = true; } else { item.LECarCarrierAuthCarrierAcceptRejectByEmail = false; }
                  //item.LECarCarrierAuthCarrierAcceptRejectExpMin = $("#txtLECarCarrierAuthCarrierAcceptRejectExpMin").data("kendoNumericTextBox").value(); 
                  // End  Modified by RHR for v-8.4.0.002 on 04/27/2021 Added Fields for Token Accept Reject 
            
                  $.ajax({
                      async: true,
                      type: "POST",
                      url: "api/LegalEntityCarrier/SaveLegalEntityCarrier",
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
                                  if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save Carrier Dispatch Settings Failure", data.Errors, null); }
                                  else {
                                      if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                          if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') { blnSuccess = true; refresh(); wndCarSet.close(); }
                                      }
                                  }
                              }
                              if (blnSuccess === false && blnErrorShown === false) {
                                  if (strValidationMsg.length < 1) { strValidationMsg = "Save Carrier Dispatch Settings Failure"; }
                                  ngl.showErrMsg("Save Carrier Dispatch Settings Failure", strValidationMsg, null);
                              }
                          } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                      },
                      error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Carrier Dispatch Settings Failure", sMsg, null); }
                  });                      
              }

              function ConfirmDeleteLECarrier(iRet){          
                  if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; }           
                  if (typeof (oLECarDeleteItem) !== 'undefined' && ngl.isObject(oLECarDeleteItem)) {
                      var lecControl = 0;
                      if ('LECarControl' in oLECarDeleteItem) { lecControl = oLECarDeleteItem.LECarControl; } 
                      if (lecControl == 0) { ngl.showErrMsg("Delete Error", "Could not get LECarControl from the record", null); return; }
                      var oCRUDCtrl = new nglRESTCRUDCtrl();
                      var blnRet = oCRUDCtrl.delete("LegalEntityCarrier/DeleteLegalEntityCarrier", lecControl, tPage, "DeleteLECarrierSuccessCallback", "DeleteLECarrierAjaxErrorCallback", true);            
                  }
              }
       
              var oLECarDeleteItem = null; 
              function deleteLECarrier(e){
                  oLECarDeleteItem = null; //clear any old values
                  oLECarDeleteItem = this.dataItem($(e.currentTarget).closest("tr")); 
                  if (typeof (oLECarDeleteItem) !== 'undefined' && ngl.isObject(oLECarDeleteItem)) {
                      ngl.OkCancelConfirmation(
                             "Delete Selected Carrier",
                             "This action cannot be undone. Are you sure you want to continue?",
                             400,
                             400,
                             null,
                             "ConfirmDeleteLECarrier");
                  };      
              }

              $('#chkLECarUseDefault').click(function(){
                  if($(this).is(':checked')){
                      //Disable fields and set it to 0
                      $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").value(0);
                      $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").value("");
                      $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").value("");  
                      $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").enable(false);
                      $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").enable(false);           
                      $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").enable(false);                                         
                  } else {
                      $("#txtLECarCarrierAcceptLoadMins").data("kendoNumericTextBox").enable(true);
                      $("#txtLECarExpiredLoadsTo").data("kendoMaskedTextBox").enable(true);           
                      $("#txtLECarExpiredLoadsCc").data("kendoMaskedTextBox").enable(true);
                  }
              });
       
              ////////////////////////////////////
              //////////ACCESSORIALS/////////////
              function DisablePercentTolerances() {
                  //Disable Percent Tolerances and set them to 0
                  $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(false);
                  $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(false);
                  $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(0);
                  $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(0);
              }
              function DisableFlatRateTolerances() {
                  //Disable Flat Rate Tolerances and set them to 0
                  $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false);
                  $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false);
                  $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
                  $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);
              }

              function EnablePercentTolerances(perLow, perHigh) {
                  //Enable Percent Tolerances
                  //If perLow/perHigh is null do not set Percent Tolerances, else if perLow/perHigh is not null then set the Percent Tolerances to value
                  $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(true);
                  $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(true);
                  if (typeof (perLow) !== 'undefined' && perLow != null) { $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(perLow); }
                  if (typeof(perHigh) !== 'undefined' && perHigh != null) { $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(perHigh); }
              }
              function EnableFlatRateTolerances(low, high) {
                  //Enable Flat Rate Tolerances
                  //If low/high is null do not set Flat Rate Tolerances, else if low/high is not null then set the Flat Rate Tolerances to value
                  $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(true);
                  $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(true);
                  if (typeof (low) !== 'undefined' && low != null) { $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(low); }
                  if (typeof (high) !== 'undefined' && high != null) { $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(high); }
              }

              function DisableAverageValue() {
                  //Disable AverageValue and set it to 0
                  $("#txtAverageValue").data("kendoNumericTextBox").enable(false);
                  $("#txtAverageValue").data("kendoNumericTextBox").value(0);
              }       
              function EnableAverageValue(value) {
                  //Enable the AverageValue
                  //If value is null do not set AverageValue, else if value is not null then set the AverageValue to value
                  $("#txtAverageValue").data("kendoNumericTextBox").enable(true);
                  if (typeof (value) !== 'undefined' && value != null) { $("#txtAverageValue").data("kendoNumericTextBox").value(value); }          
              }

              function changeAverageValue(value){
                  if(value > 0){
                      $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(true);
                      $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(true);
                      $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false);
                      $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false);
                      $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
                      $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);
                  }
                  if(value === 0){
                      $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(false);
                      $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(false);
                      $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(true);
                      $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(true);
                      $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(0);
                      $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(0);
                  }
              }

              var blnOnlyPopulateCaption = true; //set to true by default

              function openAccessorialAddWindow(){
                  blnOnlyPopulateCaption = false
                  //Reset the control value
                  $("#txtLECAControl").val(0);
                  $("#txtLECALECarControl").val(0);                
                  $("#txtLECAUpdated").val("");            
                  $("#wndAccessorial").data("kendoWindow").title("Add New Accessorial"); //Set the title of the window
                  //Set the labels for the window
                  var c = "<h3>Carrier: " + selectedRowCarrierName + "</h3>";
                  $("#lblCarr").html(c);
                  var l = "<h3>Legal Entity: " + pgLEName + "</h3>";
                  $("#lblLEAcc").html(l);
                  //Reset values
                  $("#txtLECALECarControl").val(iLECarPK);     
                  var dropdownlist = $("#ddAccessorialCode").data("kendoDropDownList");
                  //dropdownlist.readonly(false);
                  dropdownlist.select(0);
                  $("#chkAutoApprove").prop('checked', false);
                  $("#chkAllowCarrierUpdates").prop('checked', false);
                  $("#chkAccessorialVisible").prop('checked', true);
                  $("#txtCaption").data("kendoMaskedTextBox").value("");
                  $("#txtEDICode").data("kendoMaskedTextBox").value("");           
                  $("#chkDynamicAverageValue").prop('checked', false);
                  EnableAverageValue(0); //Enable the AverageValue and set it to 0            
                  DisablePercentTolerances(); //Disable Percent Tolerances and set them to 0          
                  EnableFlatRateTolerances(0, 0); //Enable Flat Rate Tolerances and set them to 0                             
                  wndAccessorial.center().open();
              }

              function openAccessorialEditWindow(e) {   
                  blnOnlyPopulateCaption = true;
                  //Reset the control value
                  $("#txtLECAControl").val(0);
                  $("#txtLECALECarControl").val(0);
                  $("#txtLECAUpdated").val("");
                  var item = this.dataItem($(e.currentTarget).closest("tr"));             
                  $("#wndAccessorial").data("kendoWindow").title("Edit Accessorial"); //set the title of the window
                  //set the lables for the window
                  var c = "<h3>Carrier: " + selectedRowCarrierName + "</h3>";
                  $("#lblCarr").html(c);
                  var l = "<h3>Legal Entity: " + pgLEName + "</h3>";
                  $("#lblLEAcc").html(l);
                  //values
                  var dropdownlist = $("#ddAccessorialCode").data("kendoDropDownList");
                  dropdownlist.select(function(dataItem) { return dataItem.AccessorialCode === item.AccessorialCode; });
                  //dropdownlist.readonly();
                  if (item.AutoApprove) { $("#chkAutoApprove").prop('checked', true); } else{ $("#chkAutoApprove").prop('checked', false); }
                  if (item.AllowCarrierUpdates) { $("#chkAllowCarrierUpdates").prop('checked', true); } else{ $("#chkAllowCarrierUpdates").prop('checked', false); }
                  if (item.AccessorialVisible) { $("#chkAccessorialVisible").prop('checked', true); } else{ $("#chkAccessorialVisible").prop('checked', false); }
                  $("#txtCaption").data("kendoMaskedTextBox").value(item.Caption);
                  $("#txtEDICode").data("kendoMaskedTextBox").value(item.EDICode); 
                  //save the control so it can be accessed by the save function
                  $("#txtLECAControl").val(item.LECAControl);
                  $("#txtLECALECarControl").val(item.LECALECarControl);
                  $("#txtLECAUpdated").val(item.LECAUpdated); 
                  if (item.DynamicAverageValue) { 
                      $("#chkDynamicAverageValue").prop('checked', true); 
                      DisableAverageValue(); //Disable AverageValue and set it to 0                
                      DisableFlatRateTolerances(); //Disable Flat Rate Tolerances and set them to 0               
                      EnablePercentTolerances(item.ApproveTolerancePerLow, item.ApproveTolerancePerHigh); //Enable Percent Tolerances and set them to the database value               
                  } 
                  else{ 
                      $("#chkDynamicAverageValue").prop('checked', false);
                      $("#txtAverageValue").data("kendoNumericTextBox").value(item.AverageValue);
                      if(item.AverageValue > 0){
                          EnablePercentTolerances(item.ApproveTolerancePerLow, item.ApproveTolerancePerHigh); //Enable Percent Tolerances and set them to the database value                  
                          DisableFlatRateTolerances(); //Disable Flat Rate Tolerances and set them to 0              
                      }else if(item.AverageValue === 0){
                          DisablePercentTolerances(); //Disable Percent Tolerances and set them to 0                    
                          EnableFlatRateTolerances(item.ApproveToleranceLow, item.ApproveToleranceHigh); //Enable Flat Rate Tolerances and set them to the database value                                        
                      }
                  }                  
                  wndAccessorial.center().open();
              }

              function SaveAccessorial() {
                  var otmp = $("#focusCancelAcc").focus();
                  var tfAutoApprove = false;
                  var tfAllowCarrierUpdates = false;
                  var tfAccessorialVisible = false;
                  var tfDynamicAverageValue = false;            
                  if ($('#chkAutoApprove').is(":checked")) { tfAutoApprove = true; }               
                  if ($('#chkAllowCarrierUpdates').is(":checked")) { tfAllowCarrierUpdates = true; }  
                  if ($('#chkAccessorialVisible').is(":checked")) { tfAccessorialVisible = true; }
                  if ($('#chkDynamicAverageValue').is(":checked")) { tfDynamicAverageValue = true; }
                  var item = new LECarrierAccessorial();
                  item.LECAControl = $("#txtLECAControl").val();
                  item.LECALECarControl = $("#txtLECALECarControl").val();
                  var dataItem = $("#ddAccessorialCode").data("kendoDropDownList").dataItem();
                  item.LECAAccessorialCode = dataItem.AccessorialCode;
                  item.LECACaption = $("#txtCaption").data("kendoMaskedTextBox").value();
                  item.LECAEDICode = $("#txtEDICode").data("kendoMaskedTextBox").value();
                  item.LECAAutoApprove = tfAutoApprove;
                  item.LECAAllowCarrierUpdates = tfAllowCarrierUpdates;
                  item.LECAAccessorialVisible = tfAccessorialVisible;
                  item.LECADynamicAverageValue = tfDynamicAverageValue;
                  item.LECAAverageValue = $("#txtAverageValue").data("kendoNumericTextBox").value();
                  item.LECAApproveToleranceLow = $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(); 
                  item.LECAApproveToleranceHigh = $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value();
                  item.LECAApproveTolerancePerLow = $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value();
                  item.LECAApproveTolerancePerHigh = $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value();
                  item.LECAUpdated = $("#txtLECAUpdated").val();                          
                  $.ajax({
                      type: "POST",
                      url: "api/LECarrierAccessorial/Post",
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
                                  if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save LE Carrier Accessorial Failure", data.Errors, null); }                        
                                  else {                              
                                      if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                                                
                                          if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                              blnSuccess = true;
                                              if (ngl.stringHasValue(data.Data[0].ErrMsg)){ ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Save LE Carrier Accessorial Error'), data.Data[0].ErrMsg, null); return; }
                                              if (ngl.stringHasValue(data.Data[0].SuccessMsg)){ var sSuccessMsg = ngl.replaceEmptyString(data.Data[0].SuccessTitle,'', '<br>') + ngl.replaceEmptyString(data.Data[0].SuccessMsg,''); ngl.showSuccessMsg(sSuccessMsg, null); }
                                              if (ngl.stringHasValue(data.Data[0].WarningMsg)){ ngl.showWarningMsg(ngl.replaceEmptyString(data.Data[0].WarningTitle,'Save LE Carrier Accessorial Warning'), data.Data[0].WarningMsg, null); }                                   
                                              refreshLECarSetGrid();
                                              wndAccessorial.close();         
                                              blnOnlyPopulateCaption = true; //reset back to default
                                          }                             
                                      }
                                  }
                              } 
                              if (blnSuccess === false && blnErrorShown === false) {
                                  if (strValidationMsg.length < 1) { strValidationMsg = "Save LE Carrier Accessorial Failure"; }
                                  ngl.showErrMsg("Save LE Carrier Accessorial Failure", strValidationMsg, null);
                              }
                          } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                      },
                      error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save LE Carrier Accessorial Failure", sMsg, null); }
                  });        
              }

              function deleteLECA(e){ //REPLACE
                  var item = this.dataItem($(e.currentTarget).closest("tr"));
                  var lecaControl = 0;
                  if (typeof (item) !== 'undefined' && item != null) { 
                      if ('LECAControl' in item) { lecaControl = item.LECAControl; } //REPLACE
                  } 
                  if (lecaControl == 0) { ngl.showErrMsg("Delete Error", "Could not get LECAControl from the record", null); return; }
                  $.ajax({
                      async: false,
                      url: "api/LECarrierAccessorial/DeleteLECarrierAccessorials/" + lecaControl,
                      headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                      contentType: "application/json; charset=utf-8",
                      dataType: 'json',
                      type: "DELETE",
                      success: function (data) {     
                          try {
                              var blnSuccess = false;
                              var blnErrorShown = false;
                              var strValidationMsg = "";
                              if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                  if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Delete LE Carrier Accessorial Failure", data.Errors, null); }
                              }
                          } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                      },
                      error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure"); ngl.showErrMsg("Delete LE Carrier Accessorial Failure", sMsg, null); }
                  });
                  refreshLECarSetGrid();
              }

              $('#chkDynamicAverageValue').click(function(){
                  if($(this).is(':checked')){
                      DisableAverageValue(); //Disable AverageValue and set it to 0                
                      DisableFlatRateTolerances(); //Disable Flat Rate Tolerances and set them to 0               
                      EnablePercentTolerances(null, null); //Enable Percent Tolerances (do not change the values aka "null")
                  } else {
                      EnableAverageValue(null); //Enable the AverageValue (do not change the values aka "null")               
                      EnablePercentTolerances(null, null); //Enable Percent Tolerances (do not change the values aka "null")              
                      $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false); //Disable Flat Rate Tolerances
                      $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false); //Disable Flat Rate Tolerances            
                  }
              });
       
              ////////////wndBillToComp///////////////////
              function refreshBillToCompsDropDown() { $('#ddlBillToComps').data('kendoDropDownList').dataSource.read(); }

              function openEditBillToCompWindow(){                     
                  refreshBillToCompsDropDown(); //refresh the ddl
                  //set the Carrier label         
                  var c = "<h3>Carrier: " + selectedRowCarrierName + "</h3>";
                  $("#lblCarrierName").html(c);
                  //set the ddl
                  var dropdownlist = $("#ddlBillToComps").data("kendoDropDownList");
                  dropdownlist.select(function(dataItem) { return dataItem.Control === leCarSetGridSelectedRowDataItem.BillToCompControl; });
                  //open window           
                  wndBillToComp.center().open();
              }

              function btnSetBillToComp_Click(){
                  var compControl = $("#ddlBillToComps").data("kendoDropDownList").value();                
                  var gr = new GenericResult();
                  gr.Control = iLECarPK;
                  gr.intField1 = compControl;
                  var oCRUDCtrl = new nglRESTCRUDCtrl();
                  var blnRet = oCRUDCtrl.update("LegalEntityCarrier/SetLECarrierBillToComp", gr, tPage, "PostLECarBillToCompSuccessCallback", "PostLECarBillToCompAjaxErrorCallback", true);             
              }

              ///////////COPY CARRIER FEE CONFIG///////////// 
              function openCopyLECarFeeWindow() {
                  var title = "Copy " + selectedRowCarrierName + " ACCSL Config";
                  $("#wndCopyLECarConfig").data("kendoWindow").title(title);
                  $("#txtCopyFromLECarControl").val(iLECarPK);
                  $("#lblCopyFromLECarName").html(selectedRowCarrierName);
                  $("#copyToGrid").data("kendoGrid").clearSelection();
                  $("#copyToGrid").data("kendoGrid").dataSource.read();
                  wndCopyLECarConfig.center().open();
              }

              function ConfirmCopyOkClick(iRet){          
                  if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
                  //Chose the Ok action
                  if(typeof (copyToLECars) !== 'undefined' && ngl.isArray(copyToLECars) && copyToLECars.length > 0){
                      var s = new GenericResult();  
                      s.intArray = copyToLECars;
                      s.intField1 = $("#txtCopyFromLECarControl").val();                   
                      $.ajax({
                          type: 'POST',
                          url: 'api/LECarrierAccessorial/CopyLECarrierAccessorialConfig',
                          contentType: 'application/json; charset=utf-8',
                          dataType: 'json',
                          data: JSON.stringify(s),
                          headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                          success: function (data) { wndCopyLECarConfig.close(); refreshLECarSetGrid(); },
                          //error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                      });
                  }
              }
       
              var copyToLECars = null; 
              function btnCopyOk_Click(){
                  copyToLECars = null; //clear any old values
                  copyToLECars = $("#copyToGrid").data("kendoGrid").selectedKeyNames();
                  if(typeof (copyToLECars) !== 'undefined' && ngl.isArray(copyToLECars) && copyToLECars.length > 0){
                      var title = "Copy " + selectedRowCarrierName + " Accessorial Configuration";
                      ngl.OkCancelConfirmation(
                          title,
                          "This action will overwrite the existing Accessorial configuration for the selected Carriers. Are you sure that you want to proceed?",
                          400,
                          400,
                          null,
                          "ConfirmCopyOkClick");
                  }else{ ngl.showWarningMsg("Selection Required", "Please select at least one \"Copy To\" Carrier", null); }                          
              }

              ///////////COPY LEGAL ENTITY CONFIG/////////////       
              function refreshLEToCopyDropDown() { $('#ddlLEToCopy').data('kendoDropDownList').dataSource.read(); }

              function openCopyLEConfigWnd(){
                  if (pgLEControl == 0) { ngl.showErrMsg("Legal Entity Required", "", null); wndCopyLEConfig.close(); return; }                    
                  var h = "<h2>" + pgLEName + "</h2>";
                  $("#divCopyToLE").html(h);
                  refreshLEToCopyDropDown();
                  wndCopyLEConfig.center().open();
              }

              function btnCopyLEOk_Click(){
                  if($("#ddlLEToCopy").data("kendoDropDownList").value() == pgLEControl){ ngl.showErrMsg("Cannot Copy Own Config", "Cannot Copy Own Config", null); return;  }           
                  var msgPrompt = "This action will overwrite any existing configurations for " + pgLEName + ". Are you sure that you want to proceed?";               
                  kendo.confirm(msgPrompt).then(function () {                    
                      //kendo.alert("You chose the Ok action.");
                      execCopyLEConfig();               
                  }, function () {                    
                      //kendo.alert("You chose to Cancel action.");
                      wndCopyLEConfig.close();
                      refreshLECarSetGrid();                
                  });          
              }

              function execCopyLEConfig() {
                  var CopyToLE = pgLEControl;
                  var CopyFromLE = $("#ddlLEToCopy").data("kendoDropDownList").value();
                  var name = $("#ddlLEToCopy").data("kendoDropDownList").text();                       
                  var gr = new GenericResult();
                  gr.intField1 = CopyToLE;
                  gr.intField2 = CopyFromLE;
            
                  $.ajax({
                      async: false,
                      type: "POST",
                      url: "api/LegalEntity/CopyLECAMConfig",
                      contentType: "application/json; charset=utf-8",
                      dataType: 'json',
                      data: JSON.stringify(gr),
                      headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                      success: function (data) {     
                          try {
                              var blnSuccess = false;
                              var blnErrorShown = false;
                              var strValidationMsg = "";
                              if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                  if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Copy Legal Entity CAM Config Failure", data.Errors, null); }
                                  else {
                                      if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                          if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') { blnSuccess = true; refreshLECarSetGrid(); }
                                      }
                                  }
                              }
                              if (blnSuccess === false && blnErrorShown === false) {
                                  if (strValidationMsg.length < 1) { strValidationMsg = "Copy Legal Entity CAM Config Failure"; }
                                  ngl.showErrMsg("Copy Legal Entity CAM Config Failure", strValidationMsg, null);
                              }
                          } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                      },
                      error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Copy Legal Entity CAM Config Failure", sMsg, null); }
                  });           
                  wndCopyLEConfig.close();
              }

              ///////////INSERT DEFAULT ACCESSORIALS///////////// 
              function openInsertDefaultFeeWindow() {
                  $("#insertToGrid").data("kendoGrid").clearSelection();
                  $("#insertToGrid").data("kendoGrid").dataSource.read();
                  wndInsertDefaultAccessorials.center().open();
              }

              function ConfirmInsertOkClick(iRet){          
                  if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
                  //Chose the Ok action
                  if(typeof (insertToLECars) !== 'undefined' && ngl.isArray(insertToLECars) && insertToLECars.length > 0){
                      var s = new GenericResult();  
                      s.intArray = insertToLECars;                 
                      $.ajax({
                          type: 'POST',
                          url: 'api/LECarrierAccessorial/CopyDefaultAccessorialsToLECarriers',
                          contentType: 'application/json; charset=utf-8',
                          dataType: 'json',
                          data: JSON.stringify(s),
                          headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                          success: function (data) { wndInsertDefaultAccessorials.close(); refreshLECarSetGrid(); },
                          //error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                      });
                  }
              }
       
              var insertToLECars = null; 
              function btnInsertOk_Click(){
                  insertToLECars = null; //clear any old values
                  insertToLECars = $("#insertToGrid").data("kendoGrid").selectedKeyNames();
                  if(typeof (insertToLECars) !== 'undefined' && ngl.isArray(insertToLECars) && insertToLECars.length > 0){
                      var title = "Insert Default Accessorials";
                      ngl.OkCancelConfirmation(
                          title,
                          "This action will overwrite the existing Accessorial configuration for the selected Carriers. Are you sure that you want to proceed?",
                          400,
                          400,
                          null,
                          "ConfirmInsertOkClick");
                  }else{ ngl.showWarningMsg("Selection Required", "Please select at least one Carrier", null); }                          
              }

              //Fast Tab functions
              function expandExpired() { $("#FastTabDivExpired").show(); $("#ExpandExpiredSpan").hide(); $("#CollapseExpiredSpan").show(); }
              function collapseExpired() { $("#FastTabDivExpired").hide(); $("#ExpandExpiredSpan").show(); $("#CollapseExpiredSpan").hide(); }
              function expandBilling() { $("#FastTabDivBilling").show(); $("#ExpandBillingSpan").hide(); $("#CollapseBillingSpan").show(); }
              function collapseBilling() { $("#FastTabDivBilling").hide(); $("#ExpandBillingSpan").show(); $("#CollapseBillingSpan").hide(); }

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
                  
                  //NOTE: This has to be done before PageReadyJS because the grid is dependant on the LEControl value                  
                  getLEAdminNotAsync(0, GetLEAdminNotAsyncCB); //Get the LE for the user
                  
                  var kmask = new kendoMasks();
                  kmask.loadDefaultMasks();
                  var phoneMask = kmask.getMask("phone_number");
                  console.log("kmask");
                  console.log(kmask);
                  console.log("phoneMask");
                  console.log(phoneMask);
                  if (control != 0){
                      setTimeout(function () {                          
                          ////////////ChangeLEDialogCtrl///////////////////
                          oChangeLEDialogCtrl = new ChangeLEDialogCtrl();                
                          oChangeLEDialogCtrl.loadDefaults(wndChangeLEDialog, oChangeLEDialogSaveCB);
                          
                          //Kendo Definitions
                          $("#txtCarrierSCAC").kendoMaskedTextBox();
                          $("#txtCarrierSCAC").data("kendoMaskedTextBox").readonly();
                          $("#txtCarrierAccountRef").kendoMaskedTextBox();
                          $("#txtCaption").kendoMaskedTextBox();
                          $("#txtEDICode").kendoMaskedTextBox();
                          $("#txtApproveToleranceLow").kendoNumericTextBox({ format: "{0:c2}" });
                          $("#txtApproveToleranceHigh").kendoNumericTextBox({ format: "{0:c2}" });
                          $("#txtApproveTolerancePerLow").kendoNumericTextBox({ format: "p0", factor: 100, min: 0, max: 1, step: 0.01 });
                          $("#txtApproveTolerancePerHigh").kendoNumericTextBox({ format: "p0", factor: 100, min: 0, max: 1, step: 0.01 });                 
                          $("#txtAverageValue").kendoNumericTextBox({
                              change: function() { var value = this.value(); changeAverageValue(value); }
                          });
                          $("#txtLECarCarrierAcceptLoadMins").kendoNumericTextBox({ format: "#########", min: 0 });
                          $("#txtLECarExpiredLoadsTo").kendoMaskedTextBox();
                          $("#txtLECarExpiredLoadsCc").kendoMaskedTextBox();
                          $("#txtLECarBillingAddress1").kendoMaskedTextBox();
                          $("#txtLECarBillingAddress2").kendoMaskedTextBox();
                          $("#txtLECarBillingAddress3").kendoMaskedTextBox();
                          $("#txtLECarBillingCity").kendoMaskedTextBox();
                          $("#txtLECarBillingState").kendoMaskedTextBox();
                          $("#txtLECarBillingZip").kendoMaskedTextBox();
                          $("#txtLECarBillingCountry").kendoMaskedTextBox();
                          $("#txtLECarCarrierAuthCarrierAcceptRejectExpMin").kendoNumericTextBox();
                          ///////////UNASSIGNED LE CARRIERS DDL/////////////
                          $("#ddlUnassignedLECarriers").kendoDropDownList({
                              dataTextField: "Name",
                              dataValueField: "Control",
                              autoWidth: true,
                              filter: "contains",
                              dataSource: {
                                  serverFiltering: false,
                                  transport: {
                                      read: function(options) {
                                          var v = new  vLookupListCriteria();
                                          v.id = nglGlobalDynamicLists.AvailLECarrier;
                                          v.sortKey  = 1;
                                          v.criteria = pgLEControl;
                                          $.ajax({
                                              url: "api/vLookupList/GetGlobalDynamicListFiltered",
                                              contentType: "application/json; charset=utf-8",
                                              dataType: 'json',
                                              data: {filter: JSON.stringify(v)},
                                              headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },                                        
                                              success: function (data) { options.success(data); },
                                              error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Available LECarrier Failure"); ngl.showErrMsg("Get Available LECarrier", sMsg, null); }
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
                                              Description: { type: "string" }
                                          }
                                      }, 
                                      errors: "Errors"
                                  },
                                  error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Carriers JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                              }
                          });
                          
                          ///////////ASSIGNED LE CARRIERS DDL/////////////
                          $("#ddlAssignedLECarriers").kendoDropDownList({
                              dataTextField: "Name",
                              dataValueField: "Control",
                              autoWidth: true,
                              filter: "contains",
                              dataSource: {
                                  serverFiltering: false,
                                  transport: {
                                      read: function(options) {
                                          var v = new  vLookupListCriteria();
                                          v.id = nglGlobalDynamicLists.LECarrier;
                                          v.sortKey  = 1;
                                          v.criteria = pgLEControl;
                                          $.ajax({
                                              url: "api/vLookupList/GetGlobalDynamicListFiltered",
                                              contentType: "application/json; charset=utf-8",
                                              dataType: 'json',
                                              data: {filter: JSON.stringify(v)},
                                              headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },                                        
                                              success: function (data) { options.success(data); },
                                              error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Assigned LECarrier Failure"); ngl.showErrMsg("Get Assigned LECarrier", sMsg, null); }
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
                                              Description: { type: "string" }
                                          }
                                      }, 
                                      errors: "Errors"
                                  },
                                  error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Assigned LECarrier JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                              }
                          });
                          
                          /////////DISPATCH TYPE DDL//////////
                          $("#ddlDispatchType").kendoDropDownList({
                              dataTextField: "Name",
                              dataValueField: "Control",
                              autoWidth: true,
                              filter: "contains",
                              dataSource: {
                                  serverFiltering: false,
                                  transport: {
                                      read: {
                                          url: "api/vLookupList/GetStaticList/62",
                                          headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
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
                                              Description: { type: "string" }
                                          }
                                      }, 
                                      errors: "Errors"
                                  },
                                  error: function (xhr, textStatus, error) { ngl.showErrMsg("Get DispatchType JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                              }
                          });
                          
                          //////////ACCESSORIALS DDL//////////
                          dsAccessorials = new kendo.data.DataSource({
                              transport: {
                                  read: {
                                      url: "api/Accessorials/GetAccessorials",
                                      dataType: 'json',
                                      headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                      type: "GET"
                                  },
                                  parameterMap: function (options, operation) { return options; }
                              },
                              schema: {
                                  data: "Data",
                                  total: "Count",
                                  model: {
                                      id: "AccessorialCode",
                                      fields: {
                                          AccessorialCode: { type: "number" },
                                          AccessorialName: { type: "string" },
                                          AccessorialAutoApprove: { type: "bool" },
                                          AccessorialAllowCarrierUpdates: { type: "bool" },
                                          AccessorialVisible: { type: "bool" },                               
                                          AccessorialCaption: { type: "string" },
                                          AccessorialEDICode: { type: "string" },
                                          AccessorialMinimum: { type: "number" }
                                      }
                                  },
                                  errors: "Errors"
                              },
                              error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Accessorials JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                              serverPaging: false,
                              serverSorting: false,
                              serverFiltering: false
                          });
                          $("#ddAccessorialCode").kendoDropDownList({
                              dataTextField: "AccessorialName",
                              dataValueField: "AccessorialCode",
                              dataSource: dsAccessorials,
                              select: function(e) {
                                  var item = e.dataItem;
                                  if (typeof (item) !== 'undefined' && item != null && ngl.isObject(item)) { 
                                      if ('AccessorialCaption' in item) { $("#txtCaption").val(item.AccessorialCaption); }                            
                                      if (blnOnlyPopulateCaption === false){                               
                                          if ('AccessorialAutoApprove' in item) { if(item.AccessorialAutoApprove){ $("#chkAutoApprove").prop('checked', true); } else{ $("#chkAutoApprove").prop('checked', false); } }
                                          if ('AccessorialAllowCarrierUpdates' in item) { if(item.AccessorialAllowCarrierUpdates){ $("#chkAllowCarrierUpdates").prop('checked', true); } else{ $("#chkAllowCarrierUpdates").prop('checked', false); } }
                                          if ('AccessorialVisible' in item) { if(item.AccessorialVisible){ $("#chkAccessorialVisible").prop('checked', true); }else{ $("#chkAccessorialVisible").prop('checked', false); } }                             
                                          if ('AccessorialEDICode' in item) { $("#txtEDICode").val(item.AccessorialEDICode); }
                                          if ('AccessorialMinimum' in item) { $("#txtAverageValue").data("kendoNumericTextBox").value(item.AccessorialMinimum); changeAverageValue(item.AccessorialMinimum); } 
                                      }
                                  }                 
                              }
                          });
                          
                          ///////BILL TO COMP BY LE DDL/////////
                          dsBillToComps = new kendo.data.DataSource({
                              transport: {
                                  read: {
                                      url: function(options) {
                                          return "api/vLookupList/GetUserDynamicList/21"; //CompActive = 21
                                      },
                                      headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                                  },
                                  parameterMap: function (options, operation) { return options; }
                              },
                              schema: {
                                  data: "Data",
                                  total: "Count",
                                  model: {
                                      id: "Control",
                                      fields: {
                                          Name: { editable: false },
                                          Control: { editable: false }
                                      }
                                  },
                                  errors: "Errors"
                              },
                              error: function (xhr, textStatus, error) { ngl.showErrMsg("GetBillToComp JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                              serverPaging: false,
                              serverSorting: false,
                              serverFiltering: false
                          });               
                          $("#ddlBillToComps").kendoDropDownList({
                              autoBind: true,
                              dataTextField: "Name",
                              dataValueField: "Control",
                              dataSource: dsBillToComps,
                              optionLabel: { Name: "N/A", Control: 0 }
                          });
                          
                          ///////LEGAL ENTITY TO COPY DDL/////////
                          dsLEToCopy = new kendo.data.DataSource({
                              transport: {
                                  read: {
                                      url: function(options) {
                                          return "api/LegalEntity/GetLEWithCAMConfigsToCopy/";
                                      },
                                      headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                                  },
                                  parameterMap: function (options, operation) { return options; }
                              },
                              schema: {
                                  data: "Data",
                                  total: "Count",
                                  model: {
                                      id: "Control",
                                      fields: {
                                          Control: { editable: false },
                                          Name: { editable: false },                
                                          Description: { editable: false }
                                      }
                                  },
                                  errors: "Errors"
                              },
                              error: function (xhr, textStatus, error) { ngl.showErrMsg("GetLEWithCAMConfigsToCopy JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                              serverPaging: false,
                              serverSorting: false,
                              serverFiltering: false
                          });                
                          $("#ddlLEToCopy").kendoDropDownList({
                              autoBind: true,
                              dataTextField: "Name",
                              dataValueField: "Control",
                              dataSource: dsLEToCopy
                          });
                          
                          
                          ////////wndAccessorial//////////////
                          wndAccessorial = $("#wndAccessorial").kendoWindow({
                              title: "Edit/Add",
                              height: 500,
                              width: 500,
                              minWidth: 500,
                              modal: true,
                              visible: false,
                              actions: ["save", "Minimize", "Maximize", "Close"],
                          }).data("kendoWindow");
                          $("#wndAccessorial").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveAccessorial(); }); //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                          
                          //////////wndCarSet/////////////////
                          wndCarSet = $("#wndCarSet").kendoWindow({
                              title: "Edit/Add",
                              height: 485,
                              width: 475,
                              modal: true,
                              visible: false,
                              actions: ["save", "Minimize", "Maximize", "Close"],
                          }).data("kendoWindow");
                          $("#wndCarSet").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveCarrierSetting(); }); //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                          //$("#wndCarSet").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveCarrierSetting(); }); //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.

                          ////////wndBillToComp//////////////
                          wndBillToComp = $("#wndBillToComp").kendoWindow({
                              title: "Set Bill To Company",
                              modal: true,
                              visible: false,
                          }).data("kendoWindow");
                          
                          var queries = {};
                          $.each(document.location.search.substr(1).split('&'),function(c,q){
                              var i = q.split('=');
                              //console.log(i[0]);
                              if(i[0]!=""){
                                  queries[i[0].toString()] = i[1].toString();
                              }
                          });
                         // console.log(queries);
                          if(queries.carrControl==null ||queries=='undefined'){
                              $("#copyToGrid").kendoGrid({
                                  dataSource: {
                                      pageSize: 10,
                                      transport: {                            
                                          read: function(options) { 
                                              var s = new AllFilter();                                    
                                              s.page = options.data.page;
                                              s.skip = options.data.skip;
                                              s.take = options.data.take;
                                              if (typeof (LECarSetGridGetStringData) !== 'undefined' && ngl.isFunction(LECarSetGridGetStringData)) {s.Data = LECarSetGridGetStringData(s); } else { s.Data = '';}; 
                                              $.ajax({ 
                                                  url: 'api/LegalEntityCarrier/GetRecords/', 
                                                  contentType: 'application/json; charset=utf-8', 
                                                  dataType: 'json', 
                                                  data: { filter: JSON.stringify(s) }, 
                                                  headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                                  success: function(data) { 
                                                      options.success(data); 
                                                      console.log(data);
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
                                              id: "LECarControl"
                                          }
                                      }
                                  },
                                  pageable: true,
                                  scrollable: false,
                                  persistSelection: true,
                                  sortable: true,
                                  columns: [
                                      { selectable: true, width: 50 },
                                      { field:"CarrierName", title: "Name" },
                                      { field: "DispatchTypeName", title:"Dispatch Type", hidden: false }
                                  ]
                              });

                              $("#insertToGrid").kendoGrid({
                                  dataSource: {
                                      pageSize: 10,
                                      transport: {                            
                                          read: function(options) { 
                                              var s = new AllFilter();                                    
                                              s.page = options.data.page;
                                              s.skip = options.data.skip;
                                              s.take = options.data.take;
                                              if (typeof (LECarSetGridGetStringData) !== 'undefined' && ngl.isFunction(LECarSetGridGetStringData)) {s.Data = LECarSetGridGetStringData(s); } else { s.Data = '';}; 
                                              $.ajax({ 
                                                  url: 'api/LegalEntityCarrier/GetRecords/', 
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
                                              id: "LECarControl"
                                          }
                                      }
                                  },
                                  pageable: true,
                                  scrollable: false,
                                  persistSelection: true,
                                  sortable: true,
                                  columns: [
                                      { selectable: true, width: 50 },
                                      { field:"CarrierName", title: "Name" },
                                      { field: "DispatchTypeName", title:"Dispatch Type", hidden: false }
                                  ]
                              });
                          }
                          
                          /////////wndCopyLECarConfig//////////////
                          wndCopyLECarConfig = $("#wndCopyLECarConfig").kendoWindow({
                              title: "Copy Carrier Accessorial Config",
                              modal: true,
                              visible: false              
                          }).data("kendoWindow");
                          
                          /////////wndCopyLEConfig//////////////
                          wndCopyLEConfig = $("#wndCopyLEConfig").kendoWindow({
                              title: "Import Legal Entity Config",
                              modal: true,
                              visible: false              
                          }).data("kendoWindow");
                          
                          /////////wndInsertDefaultAccessorials//////////////
                          wndInsertDefaultAccessorials = $("#wndInsertDefaultAccessorials").kendoWindow({
                              title: "Insert Default Accessorials",
                              modal: true,
                              visible: false              
                          }).data("kendoWindow");
                         
                        
                      }, 10,this);                     
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
