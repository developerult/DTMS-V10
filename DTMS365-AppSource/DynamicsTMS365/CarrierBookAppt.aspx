<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierBookAppt.aspx.cs" Inherits="DynamicsTMS365.CarrierBookAppt" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Carrier Book Appt</title>
         <%=cssReference%>  
        <style>

    html,

    body
    {
        height:100%;
        margin:0;
        padding:0;
    }

    html
    {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

    </style>
    </head>  
    <body>
                
                           
        <%=jsnosplitterScripts%> 
        <%=sWaitMessage%>    
               
      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">                                                               
           <!-- <div id="CarrierBookApptContent" class="ngl-blueBorder" style=" position:absolute;  width:auto; height:auto;top: 25%;  left: 25%;  ">   -->
                <div id="CarrierBookApptContent" class="ngl-blueBorder" > 
                <br />
                 <% Response.Write(PageErrorsOrWarnings); %>                                                     
                <!-- begin Page Content -->
               
                <div id="ViewCarAvailApptUOTokenData">
                    <div>
                        <fieldset>
                            <legend><b>Order Details</b></legend>
                            <div style="margin:10px;">
                                <table class="tblResponsive" style="width: 100%;">
                                    <tr>
                                        <td class="tblResponsive-top" style="width: 20%;"><label><b>SHID: </b></label><label><b id="txtUOSHID"></b></label></td>
                                        <td class="tblResponsive-top" style="width: 20%;"><label><b>CNS: </b></label><label><b id="txtUOCNS"></b></label></td>
                                        <td class="tblResponsive-top" style="width: 20%;"><label><b>Equip ID: </b></label><label><b id="txtUOEquipID"></b></label></td>
                                    </tr>
                                    <tr>                                        
                                        <td class="tblResponsive-top" style="width: 20%;"><label><b>Order Number: </b></label><label><b id="txtUOOrderNumber"></b></label></td>
                                        <td class="tblResponsive-top" style="width: 20%;"><label><b id="lblUOLRDate"></b></label><label><b id="txtUOLRDate"></b></label></td>
                                        <td class="tblResponsive-top" style="width: 20%;"><label><b>Scheduled: </b></label><label><b id="txtUOScheduled"></b></label></td>
                                    </tr>                           
                                </table>
                            </div>
                        </fieldset>
                    </div>   
                     <div style="display:none; margin-top: 15px;"  border: solid 2px black;" id="divApptResults" >
                        <h4>Schedule Appointments Results</h4>
                        <span id="txtApptResults">Success</span>                
                    </div>
                    <div style="margin-top: 15px;" id="divAvailAppt">
                        <fieldset>
                            <legend><b>Available Appointments</b></legend>
                            <div style="margin-top: 15px;" align="center">
                                <div id="AvailableApptsUOGrid"></div>
                            </div>
                        </fieldset>
                    </div>
                </div>

               
               <p>If you need assistance please send an email to: <% Response.Write(LaneCarrierBookApptviaTokenFailEmail);%></p>               
               <p>Or Call: <%Response.Write(LaneCarrierBookApptviaTokenFailPhone);%></p>
                <!-- End Page Content --> 
           </div> 

        <div id="wndViewCarAvailApptUOGrouped">
            <%--<div>
                <fieldset>
                    <legend><b>Order Details</b></legend>
                    <div style="margin:10px;">
                        <table class="tblResponsive" style="width: 100%;">
                            <tr>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>SHID: </b></label><label><b id="txtUOSHID"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>CNS: </b></label><label><b id="txtUOCNS"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>Equip ID: </b></label><label><b id="txtUOEquipID"></b></label></td>
                            </tr>
                            <tr>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b id="lblUOLRDate"></b></label><label><b id="txtUOLRDate"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>Scheduled: </b></label><label><b id="txtUOScheduled"></b></label></td>
                            </tr>                           
                        </table>
                    </div>
                </fieldset>
            </div>
            <div style="margin-top: 15px;">
                <fieldset>
                    <legend><b>Available Appointments</b></legend>
                    <div style="margin-top: 15px;" align="center">
                        <div id="AvailableApptsUOGrid"></div>
                    </div>
                </fieldset>
            </div>--%>
        </div>
          
        <div id="wndEditEquipIDGrouped">
            <div>
                <fieldset>
                    <legend><b>Edit EquipmentID</b></legend>
                    <div style="margin-top: 15px;" align="center">
                        <h2 style="margin: 0; font-size: 1em;" id="lblEditEquipIDMsg"></h2>
                    </div>
                    <div style="margin-top: 15px;" align="center">
                        <button id="btnEditEquipIDSubmit">Save EquipmentID</button>
                    </div>
                </fieldset>
            </div>
        </div>

        <div id="wndViewCarAvailApptBAGrouped">
            <div>
                <fieldset>
                    <legend><b>Order Details</b></legend>
                    <div style="margin:10px;">
                        <table class="tblResponsive" style="width: 100%;">
                            <tr>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>SHID: </b></label><label><b id="txtBASHID"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>CNS: </b></label><label><b id="txtBACNS"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>Equip ID: </b></label><label><b id="txtBAEquipID"></b></label></td>                                
                            </tr>
                            <tr>                                        
                                <td class="tblResponsive-top" style="width: 20%;"><label><b id="lblBALRDate"></b></label><label><b id="txtBALRDate"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>Scheduled: </b></label><label><b id="txtBAScheduled"></b></label></td>                                                              
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </div>
            <div style="margin-top: 15px;">
                <fieldset>
                    <legend><b>Available Appointments</b></legend>
                    <div style="margin-top: 15px;" align="center">
                        <div id="AvailableApptsBAGrid"></div>
                    </div>
                </fieldset>
            </div>
        </div>

        <div id="wndRequestEmail">
            <div align="center">      
                <h2 style="margin:0; font-size:1em;" id="txtEmailMsg"></h2>  
            </div>  
            <div>        
                <fieldset>          
                    <legend><b>Body</b></legend>           
                    <div align="center">               
                        <h2 style="margin:0; font-size:1em;" id="txtEmailBody"></h2>           
                    </div>      
                </fieldset>   
            </div>  
            <div style="margin-top: 15px;">       
                <fieldset>           
                    <legend><b>Additional Comments</b></legend>          
                    <div style="margin-top: 15px;" align="center">               
                        <textarea id="txtEmailComments" rows="8" cols="20" style="width:300px;"></textarea>            
                    </div>            
                    <div style="margin-top: 15px;" align="center">                
                        <button id="btnSubmitReqEmail">Submit Request</button>          
                    </div>      
                </fieldset>            
            </div>
        </div>

        <script type="text/x-kendo-template" id="AMSCarGroupedDetTemplate">
            <div class="tabstrip">
                <ul><li class="k-active">Orders</li></ul>
                <div><div class="orders"></div></div>
            </div>
        </script>
            <% Response.Write(PageTemplates); %>
           <% Response.Write(AuthLoginNotificationHTML); %>  
          <script>
         <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';            
        
        var tObj = this;
        var tPage = this;  
         var tBookConsPrefix = '<%=BookConsPrefix%>';    
         var tBookControl = '<%=BookControl%>';    
         var tBookDateLoad = '<%=BookDateLoad%>';    
         var tBookDateRequired = '<%=BookDateRequired%>';    
         var tBookSHID = '<%=BookSHID%>';    
         var tCarrierControl = '<%=CarrierControl%>';    
         var tCompControl = '<%=CompControl%>';    
         var tCompName = '<%=CompName%>';    
         var tExpirationDate = '<%=ExpirationDate%>';    
         var tExpirationMinutes = '<%=ExpirationMinutes%>';    
         var tIsPickup = '<%=IsPickup%>';    
         var tLaneAllowCarrierBookApptByEmail = '<%=LaneAllowCarrierBookApptByEmail%>';    
         var tLaneCarrierBookApptviaTokenEmail = '<%=LaneCarrierBookApptviaTokenEmail%>';    
         var tLaneCarrierBookApptviaTokenFailEmail = '<%=LaneCarrierBookApptviaTokenFailEmail%>';    
         var tLaneCarrierBookApptviaTokenFailPhone = '<%=LaneCarrierBookApptviaTokenFailPhone%>';    
         var tLaneControl = '<%=LaneControl%>';    
         var tLaneRequireCarrierAuthBookApptByEmail = '<%=LaneRequireCarrierAuthBookApptByEmail%>';    
              var tLaneUseCarrieContEmailForBookApptByEmail = '<%=LaneUseCarrieContEmailForBookApptByEmail%>';
              var tBookCarrOrderNumber = '<%=BookCarrOrderNumber%>';
              var sToken = '<%=sToken%>';

              <% Response.Write(PageCustomJS); %>

              var fromEmail = 'rramsey@nextgeneration.com';

            var dsUnschedOrderPickUp = kendo.data.DataSource;
            var dsUnschedOrderDelivery = kendo.data.DataSource;
            var dsBookedApptPickup = kendo.data.DataSource;
            var dsBookedApptDelivery = kendo.data.DataSource;
            var dsAvailableApptsUO = kendo.data.DataSource;
            var dsAvailableApptsBA = kendo.data.DataSource;
            var wndViewCarAvailApptUOGrouped = kendo.ui.Window;
            var wndRequestEmail = kendo.ui.Window;
            var wndViewCarAvailApptBAGrouped = kendo.ui.Window;
            var wndEditEquipIDGrouped = kendo.ui.Window;

            function sendReqEmail(eObject) {
                $.ajax({
                    url: "api/Email/GenerateEmail",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: JSON.stringify(eObject),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("Sent Request Email Failure", data.Errors, null);
                                }
                                else {
                                    if (data.StatusCode == 200) {
                                        blnSuccess = true;
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Sent Request Email Failure"; }
                                ngl.showErrMsg("Sent Request Email Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Sent Request Email Failure");
                        ngl.showErrMsg("Sent Request Email Failure", sMsg, null);
                    }
                });
            }

              //************* Email ***************//
              var eObject = new EmailObject();               
              $("#txtEmailComments").kendoMaskedTextBox();
              $("#btnSubmitReqEmail").kendoButton({
                  icon: "email",
                  click: function () {
                      eObject.emailFrom = fromEmail;
                      eObject.emailCc = "";
                      eObject.emailBcc = "";
                      eObject.emailBody = eObject.emailBody + $("#txtEmailComments").val();
                      sendReqEmail(eObject)
                      wndRequestEmail.close();
                  }
              });

              function ViewUOPickAvailAppt(e) {
                                          
                 
                  $("#txtUOEquipID").text(tBookConsPrefix);
                  //This is set to Load Date on Pickup
                  $("#lblUOLRDate").text("Load Date: ");
                  $("#txtUOLRDate").text(kendo.toString(tBookDateLoad, 'M/d/yyyy'));
                  $("#txtUOScheduled").text('');
                  $("#txtUOSHID").text(tBookSHID);
                  $("#txtUOCNS").text(tBookConsPrefix);
                  $("#txtUOOrderNumber").text(tBookCarrOrderNumber);

                  blnUOIsPickup = true;

                  //$("#AvailableApptsUOGrid").data("kendoGrid").dataSource.data([]);
                  $("#AvailableApptsUOGrid").data("kendoGrid").dataSource.read();
              }

              function showApptReport(sText, refreshAppt) {
                  //debugger;
                  if (refreshAppt === true) {
                      $("#AvailableApptsUOGrid").data("kendoGrid").dataSource.read();
                  } else {
                      $("#divAvailAppt").hide();
                  }
                  //console.log('Show this message:');
                  //console.log(sText);
                  $("#txtApptResults").html(sText);
                  //console.log($("#txtApptResults").html());
                  //debugger;                  
                  $("#divApptResults").show();
              }

              function ViewUODelAvailAppt(e) {
                 
                  $("#txtUOEquipID").text(tBookConsPrefix);
                  //This is set to Delivery Date on Delivery
                  $("#lblUOLRDate").text("Delivery Date: ");
                  $("#txtUOLRDate").text(kendo.toString(tBookDateRequired, 'M/d/yyyy'));
                  $("#txtUOScheduled").text(kendo.toString(''));
                  $("#txtUOSHID").text(tBookSHID);
                  $("#txtUOCNS").text(tBookConsPrefix);
                  $("#txtUOOrderNumber").text(tBookCarrOrderNumber);

                  blnUOIsPickup = false;

                  //$("#AvailableApptsUOGrid").data("kendoGrid").dataSource.data([]);
                  $("#AvailableApptsUOGrid").data("kendoGrid").dataSource.read();
              }

              function UpdateBookedAppointment(e) {
                  var tsDataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    
                  var s = new AMSCarrierAvailableSlots();                         
                  s.Docks = tsDataItem.Docks;
                  //Modified by LVV for v-8.2 on 11/1/2019 we now use ngl.convertDateForWindows to avoid non-ascii characters in date string
                  s.StartTime = ngl.convertTimePickerToDateString(tsDataItem.StartTime, ngl.convertDateForWindows(tsDataItem.StartTime, ""), "");                      
                  s.EndTime = tsDataItem.EndTime;
                  s.Warehouse = tsDataItem.Warehouse;
                  s.CarrierControl = tsDataItem.CarrierControl;
                  s.CompControl = tsDataItem.CompControl;
                  s.CarrierNumber = tsDataItem.CarrierNumber;
                  s.CarrierName = tsDataItem.CarrierName;
                  s.Books = tsDataItem.Books;
                  //Modified by LVV for v-8.2 on 11/1/2019 we now use ngl.convertDateForWindows to avoid non-ascii characters in date string
                  s.Date = ngl.convertTimePickerToDateString(tsDataItem.Date, ngl.convertDateForWindows(tsDataItem.Date, ""), "");

                  var w = new AMSCarrierBAWrapper(); 
                  w.AMSCarrierAvailableSlots = s;
                  w.blnIsPickup = blnBAIsPickup; 

                  //Modified by LVV on 11/1/2019
                  $.ajax({
                      url: "api/CarrierSchedulerGrouped/UpdateCarrierBookedAppointmentGrouped",
                      type: "POST",
                      contentType: "application/json; charset=utf-8",
                      dataType: 'json',
                      data: JSON.stringify(w),
                      headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                      success: function (data) {
                          try {
                              var blnSuccess = false;
                              var blnErrorShown = false;
                              var strValidationMsg = "";
                              if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                  if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Update Appointment Failure", data.Errors, null); }
                                  else {
                                      if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                          if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                              blnSuccess = true;
                                              if (data.Data[0] == false) { ngl.showWarningMsg("Update Appointment Failure!", "", null); } 
                                              else { wndViewCarAvailApptBAGrouped.close(); ngl.showSuccessMsg("Update Appointment Success"); refresh(); } //refresh Grids
                                          }
                                      }
                                  }
                              }
                              if (blnSuccess === false && blnErrorShown === false) {
                                  if (strValidationMsg.length < 1) { strValidationMsg = "Update Appointment Failure"; }
                                  ngl.showErrMsg("Update Appointment Failure", strValidationMsg, null);
                              }
                          } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                      },
                      error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update Appointment Failure"); ngl.showErrMsg("Update Appointment Failure", sMsg, null); }
                  });
              }

              function ViewBAPickAvailAppt(e) {
                  BADataItem = this.dataItem($(e.currentTarget).closest("tr"));             

                  $("#txtBAEquipID").text(BADataItem.BookCarrTrailerNo ? BADataItem.BookCarrTrailerNo : "");
                  //This is set to Load Date on Pickup
                  $("#lblBALRDate").text("Load Date: ");
                  $("#txtBALRDate").text(kendo.toString(BADataItem.BookDateLoad, 'M/d/yyyy'));
                  $("#txtBAScheduled").text(kendo.toString(BADataItem.ScheduledDate ? BADataItem.ScheduledDate : "", 'M/d/yyyy') + " " + kendo.toString(BADataItem.ScheduledTime ? BADataItem.ScheduledTime : "", 'HH:mm'));
                  $("#txtBASHID").text(BADataItem.BookSHID);
                  $("#txtBACNS").text(BADataItem.BookConsPrefix);

                  
                  blnBAIsPickup = true;

                  $("#AvailableApptsBAGrid").data("kendoGrid").dataSource.data([]);
                  $("#AvailableApptsBAGrid").data("kendoGrid").dataSource.read();
              };


     $(document).ready(function () {  
            
         var PageReadyJS = <%=PageReadyJS%>;

        
         //************* wndViewCarAvailApptUOGrouped ***************//
         wndViewCarAvailApptUOGrouped = $("#wndViewCarAvailApptUOGrouped").kendoWindow({
             title: "Edit/Add",
             height: 'auto',
             width: 850,
             modal: true,
             visible: false,
             actions: ["Minimize", "Maximize", "Close"]
         }).data("kendoWindow");

         //************* wndRequestEmail ***************//
         wndRequestEmail = $("#wndRequestEmail").kendoWindow({
             title: "Edit/Add",
             height: 'auto',
             width: 440,
             modal: true,
             visible: false,
             actions: ["Minimize", "Maximize", "Close"]
         }).data("kendoWindow");

         //************* wndViewCarAvailApptBAGrouped ***************//
         wndViewCarAvailApptBAGrouped = $("#wndViewCarAvailApptBAGrouped").kendoWindow({
             title: "Edit/Add",
             height: 'auto',
             width: 850,
             modal: true,
             visible: false,
             actions: ["Minimize", "Maximize", "Close"]
         }).data("kendoWindow");

         //************* wndEditEquipIDGrouped ***************//
         wndEditEquipIDGrouped = $("#wndEditEquipIDGrouped").kendoWindow({
             title: "Edit/Add",
             height: 'auto',
             width: 'auto',
             modal: true,
             visible: false,
             actions: ["Minimize", "Maximize", "Close"]
         }).data("kendoWindow");

         //********** AvailableApptsUOGrid **********//
         var UODataItem = {};
         var blnUOIsPickup = true;
         dsAvailableApptsUO = new kendo.data.DataSource({
             pageSize: 10,
             transport: {
                 read: function (options) {
                     var whseControl = 0;
                     var dt = null;
                     if(blnUOIsPickup) { whseControl = tCompControl; dt = tBookDateLoad; }else{ whseControl = tCompControl; dt = tBookDateRequired; }
                                    
                     var gr = new GenericResult();                                   
                     gr.blnField = blnUOIsPickup;
                     gr.strField = tBookSHID;                                   
                     gr.strField2 = tCompName;
                     gr.intField1 = whseControl;
                     gr.intField2 = tCarrierControl;
                     gr.dtField = dt;
                     gr.sToken = sToken;
                     $.ajax({
                         url: 'api/CarrierSchedulerGrouped/GetCarAvailableApptsUOToken',
                         contentType: 'application/json; charset=utf-8',
                         dataType: 'json',
                         data: { filter: JSON.stringify(gr) },
                         headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                         success: function (data) {
                             debugger;
                             options.success(data);
                             console.log(data.Data);
                             try {
                                 debugger;
                                 var blnSuccess = false;
                                 var blnErrorShown = false;
                                 var strValidationMsg = "";
                                 if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                     if (ngl.stringHasValue(data.Errors)) {
                                         blnErrorShown = true;
                                         //ngl.showErrMsg("Get Available Appointments UO Failure", data.Errors, null);
                                     }
                                     else {
                                         if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                             if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                 blnSuccess = true;
                                                 if (data.Data[0].Message != undefined) {
                                                     $("#ViewCarAvailApptUOTokenData").hide();
                                                     eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                     eObject.emailSubject = data.Data[0].Subject;
                                                     eObject.emailBody = data.Data[0].Body;
                                                     $("#txtEmailMsg").text(data.Data[0].Message?data.Data[0].Message:"");
                                                     $("#txtEmailBody").html(data.Data[0].Body);
                                                     $("#txtEmailComments").val("");
                                                     wndRequestEmail.title(data.Data[0].Subject);
                                                     wndRequestEmail.center().open();
                                                 } else {
                                                     $("#ViewCarAvailApptUOTokenData").show();
                                                     //wndViewCarAvailApptUOGrouped.title("Carrier Scheduling - Select Appointment");
                                                     //wndViewCarAvailApptUOGrouped.center().open();
                                                 }
                                             }
                                         }
                                     }
                                 }
                                 if (blnSuccess === false && blnErrorShown === false) {
                                     if (strValidationMsg.length < 1) { strValidationMsg = "Get Available Appointments UO Failure"; }
                                     ngl.showErrMsg("Get Available Appointments UO Failure", strValidationMsg, null);
                                 }
                             } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                         },
                         error: function (result) { options.error(result); }
                     });
                 },
                 parameterMap: function (options, operation) { return options; }
             },
             schema: {
                 data: "Data",
                 total: "Count",
                 model: {
                     id: "CarrierNumber",
                     fields: {
                         CarrierControl: { type: "number" },
                         CarrierNumber: { type: "number" },
                         CarrierName: { type: "string" },
                         CompControl: { type: "number" },
                         Warehouse: { type: "string" },
                         Date: { type: "date" },
                         StartTime: { type: "date" },
                         EndTime: { type: "string" },
                         Docks: { type: "string" },
                         Books: { type: "string" }                                                                             
                     }
                 },
                 errors: "Errors"
             },
             error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Available Appointment UO Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
         });

         $("#AvailableApptsUOGrid").kendoGrid({
             noRecords: { template: "<p>No records available.</p>" },
             autoBind: false,
             height: 200,
             dataSource: dsAvailableApptsUO,
             pageable: true,
             columns: [
                 { command: [{ name: "bookAppt", text: "Book Appt", width: 100, click: BookAppointmentUO }], title: "Action" },
                 { field: "Warehouse", title: "Warehouse", template: "<span title='${Warehouse}'>${Warehouse}</span>" },
                 { field: "Date", title: "Date", format: "{0:M/d/yyyy}" },
                 { field: "StartTime", title: "Start Time", template: "#= kendo.toString(kendo.parseDate(StartTime, 'HH:mm'), 'HH:mm') #" }, //format: "{0:HH:mm}" //Modified by LVV on 11/1/2019
                 { field: "EndTime", title: "End Time", hidden: true },
                 { field: "Docks", title: "Docks", hidden: true },
                 { field: "Books", title: "Books", hidden: true },
                 { field: "CarrierControl", title: "CarrierControl", hidden: true },
                 { field: "CarrierNumber", title: "CarrierNumber", hidden: true },
                 { field: "CarrierName", title: "CarrierName", hidden: true },
                 { field: "CompControl", title: "CompControl", hidden: true },
             ],
         });

         function BookAppointmentUO(e) {
             var BookApptObject = this.dataItem($(e.currentTarget).closest("tr"));                                   
             //Modified By LVV on 8/27/2018 for v-8.3 Scheduler - Added code to fix the problem with the dates being converted from different time zones
             //Modified by RHR for v-8.2 on 09/19/2018 we now use ngl.convertDateForWindows to avoid non-ascii characters in date string
             BookApptObject.EndTime = BookApptObject.EndTime;
             BookApptObject.StartTime = ngl.convertTimePickerToDateString(BookApptObject.StartTime, ngl.convertDateForWindows(BookApptObject.StartTime, ""), "");
             BookApptObject.sToken = sToken;
             //console.log(BookApptObject);
             $.ajax({
                 url: "api/CarrierSchedulerGrouped/CarrierScheduleApptForUOGroupedToken",
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 dataType: 'json',
                 data: JSON.stringify(BookApptObject),
                 headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                 success: function (data) {
                     try {
                         var blnSuccess = false;
                         var blnErrorShown = false;
                         var strValidationMsg = "";
                         if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                             blnSuccess == true
                             //console.log(data);
                             
                             if (ngl.stringHasValue(data.Errors)) {
                                 //debugger;
                                 blnErrorShown = true;
                                 conole.log(data.Errors);
                                 ngl.showErrMsg("Schedule Appointment Failure", data.Errors, null);
                                 showApptReport("Schedule Appointment Failure", true);
                             }
                             else {
                                 if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                     if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                         blnSuccess = true;
                                         //debugger;
                                         //console.log(data.Data[0] == false)
                                         if (data.Data[0] == false) {
                                             showApptReport(data.Messages, true);
                                         } else {
                                             showApptReport(data.Messages, false);
                                         }
                                     }
                                 }
                             }
                         }                       
                         if (blnSuccess === false && blnErrorShown === false) {
                             //debugger;
                             if (strValidationMsg.length < 1) { strValidationMsg = "Schedule Appointment Failure"; }
                             ngl.showErrMsg("Schedule Appointment Failure", strValidationMsg, null);
                         }
                     } catch (err) {
                         //debugger;
                         ngl.showErrMsg(err.name, err.description, null);
                     }
                 },
                 error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Schedule Appointment Failure"); ngl.showErrMsg("Schedule Appointment Failure", sMsg, null); }
             });
         }


         //********** AvailableApptsBAGrid **********//
         var BADataItem  = {};
         var blnBAIsPickup;
         dsAvailableApptsBA = new kendo.data.DataSource({
             pageSize: 10,
             transport: {
                 read: function (options) {
                     var whseControl = 0;
                     var dt = null;
                     if(blnBAIsPickup) { whseControl = BADataItem.BookOrigCompControl; dt = BADataItem.BookDateLoad; }else{ whseControl = BADataItem.BookDestCompControl; dt = BADataItem.BookDateRequired; }
                                    
                     var gr = new GenericResult(); 
                     gr.blnField = blnBAIsPickup;
                     gr.blnField1 = false; //IsDelete
                     gr.strField = BADataItem.BookSHID;
                     gr.strField2 = BADataItem.Warehouse;
                     gr.intField1 = whseControl;
                     gr.intField2 = tCarrierControl;
                     gr.dtField = dt;                                   
                     $.ajax({
                         url: 'api/CarrierSchedulerGrouped/GetModifyOptionCarrierBAGrouped',
                         contentType: 'application/json; charset=utf-8',
                         dataType: 'json',
                         data: { filter: JSON.stringify(gr) },
                         headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                         success: function (data) {
                             options.success(data);
                             //console.log(data.Data);
                             try {
                                 var blnSuccess = false;
                                 var blnErrorShown = false;
                                 var strValidationMsg = "";
                                 if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                     if (ngl.stringHasValue(data.Errors)) {
                                         blnErrorShown = true;
                                         //ngl.showErrMsg("Get Available Appointments BA Failure", data.Errors, null);
                                     }
                                     else {
                                         if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                             if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                 blnSuccess = true;
                                                 if (data.Data[0].blnMustRequestAppt != undefined) {
                                                     if (data.Data[0].blnMustRequestAppt) {
                                                         eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                         eObject.emailSubject = data.Data[0].Subject;
                                                         eObject.emailBody = data.Data[0].Body;
                                                         $("#txtEmailMsg").text(data.Data[0].Message?data.Data[0].Message:"");
                                                         $("#txtEmailBody").html(data.Data[0].Body);
                                                         $("#txtEmailComments").val("");
                                                         wndRequestEmail.title(data.Data[0].Subject);
                                                         wndRequestEmail.center().open();
                                                     } else {
                                                         $("#BookedApptPickGrid").data("kendoGrid").dataSource.read();
                                                         $("#BookedApptDelGrid").data("kendoGrid").dataSource.read();
                                                     }
                                                 } else {
                                                     wndViewCarAvailApptBAGrouped.title("Carrier Scheduling - Select Appointment");
                                                     wndViewCarAvailApptBAGrouped.center().open();
                                                 }
                                             }
                                         }
                                     }
                                 }
                                 if (blnSuccess === false && blnErrorShown === false) {
                                     if (strValidationMsg.length < 1) { strValidationMsg = "Get Available Appointments BA Failure"; }
                                     ngl.showErrMsg("Get Available Appointments BA Failure", strValidationMsg, null);
                                 }
                             } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                         },
                         error: function (result) { options.error(result); }
                     });
                 },
                 parameterMap: function (options, operation) { return options; }
             },
             schema: {
                 data: "Data",
                 total: "Count",
                 model: {
                     id: "CarrierNumber",
                     fields: {
                         CarrierControl: { type: "number" },
                         CarrierNumber: { type: "number" },
                         CarrierName: { type: "string" },
                         CompControl: { type: "number" },
                         Warehouse: { type: "string" },
                         Date: { type: "date" },
                         StartTime: { type: "date" },
                         EndTime: { type: "string" },
                         Docks: { type: "string" },
                         Books: { type: "string" }
                     }
                 },
                 errors: "Errors"
             },
             error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Available Appointment BA Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
         });

         $("#AvailableApptsBAGrid").kendoGrid({
             noRecords: { template: "<p>No records available.</p>" },
             autoBind: false,
             height: 200,
             dataSource: dsAvailableApptsBA,
             pageable: true,
             columns: [
                 { command: [{ name: "bookAppt", text: "Book Appt", width: 100, click: UpdateBookedAppointment }], title: "Action" },
                 { field: "Warehouse", title: "Warehouse", template: "<span title='${Warehouse}'>${Warehouse}</span>" },
                 { field: "Date", title: "Date", format: "{0:M/d/yyyy}" },
                 { field: "StartTime", title: "Start Time", template: "#= kendo.toString(kendo.parseDate(StartTime, 'HH:mm'), 'HH:mm') #" }, //format: "{0:HH:mm}", //Modified by LVV on 11/1/2019
                 { field: "EndTime", title: "End Time", hidden: true },
                 { field: "Docks", title: "Docks", hidden: true },
                 { field: "Books", title: "Books", hidden: true },
                 { field: "CarrierControl", title: "CarrierControl", hidden: true },
                 { field: "CarrierNumber", title: "CarrierNumber", hidden: true },
                 { field: "CarrierName", title: "CarrierName", hidden: true },
                 { field: "CompControl", title: "CompControl", hidden: true },
             ],
         });


         var divWait = $("#h1Wait");
         if (typeof(divWait) !== 'undefined') { divWait.hide(); }
         if (tBookControl != 0){
             ViewUOPickAvailAppt()
         }

        });


          </script>
    <style>


    </style>
        </div>
    </body>
</html>
