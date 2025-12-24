<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TariffServices.aspx.cs" Inherits="DynamicsTMS365.TariffServices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Tariff Services</title>        
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
    <% Response.Write(AuthLoginNotificationHTML); %>   <a href="TariffServices.aspx">TariffServices.aspx</a>
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>   
 
     <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
         var tObj = this;
         var tPage = this;           
        

        <% Response.Write(NGLOAuth2); %>

         var oCarrierEquipList = null;
         
         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc){
             if(btn.id == "btnDeleteService"){
                 if (typeof (wdgtTariffServiceDataEdit) !== 'undefined' && ngl.isObject(wdgtTariffServiceDataEdit) === true) {  
                     wdgtTariffServiceDataEdit.delete();
                 }
                
             } else if(btn.id == "btnOpenContract"){
                 location.href = "Tariff";
                 //1. get CarTarControl from selected grid record
                 //2. update user page setting with selected record data for the tariff page
                 //3. on response from ajax load services page
                 //4. In the Services Page Controller get method read the CarrTarControl user page settings for the tariff page, set this as the parent control number
                 //5. In the tariff controller read summary data read the CarrTarControl user page settings for the tariff page, use this to query the data
                 //6. in the tariff controller when a filter is applied save the filter to the user settings for the page
                 //7. in the tariff controller when a read is triggered with an empty filter read the filter settings from the user settings page
                 //   if no records default to empty filter
             } else  if(btn.id == "btnOpenRates"){
                 location.href = "TariffRates";
             } else if(btn.id == "btnOpenExceptions"){
                 location.href = "TariffExceptions";
             } else if(btn.id == "btnOpenFees"){
                 location.href = "TariffFees";
             } else  if(btn.id == "btnOpenFuel"){
                 location.href = "TariffFuel";
             } else if(btn.id == "btnOpenNoDrive"){
                 location.href = "TariffNoDriveDays";
             } else if(btn.id == "btnOpenHDMs"){
                 location.href = "TariffHDMs";
             } else if(btn.id == "btnOpenBreakPoints"){
                 location.href = "TariffBreakPoints";
             } else if (btn.id == "btnRefresh") {                
                 wdgtvCarrierTariffSummarySummary.read(0);
                 wdgtTariffServiceDataEdit.read(0);
             }
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }
         function NGLEDITOnPageListChanged(e,tList,source){
             //note if we have more than one list on the page
             //we need to process the correct one
             if(!tList || !source) {return;}
             var sID = tList.element[0].id;
             var sName = source.GetFieldName(sID);
             if (!sName) {return;}
             if (sName === 'CarrTarEquipCarrierEquipControl'){
                 oCarrierEquipList = tList;
                 //var sKey = oCarrierEquipList.data("kendoDropDownList").value();
                 //alert(sKey);
                 var item = $("#" + sID)
                 if (!item) {return;}
                 var sKey = item.data("kendoDropDownList").value();
                 //alert(sKey);
                 return updateSelectedServiceEquipment(sKey);
             }
             
         }

         function updateSelectedServiceEquipment(key){           
             var oCRUDCtrl = new nglRESTCRUDCtrl();
             var blnRet = oCRUDCtrl.read("CarrierEquipCode", key, tPage, "readServiceEquipmentSuccessCallback", "readServiceEquipmentAjaxErrorCallback",tPage);
             return true;
         }

         function readServiceEquipmentSuccessCallback(data){
             
             //try {
             //    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
             //} catch (err) {
             //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
             //}
             
             try {
                 if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                     if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {                       
                         ngl.showErrMsg("Read Service Equipment Failure",data.Errors, tPage);
                     }
                     else {                        
                         if (data.Data != null) {                                                       
                             var record = data.Data[0];
                             //update the informtion on the page.
                             $.each(objTariffServiceDataDataFields, function (index, item) {
                                 var blnUpdate = false;
                                 var dataItem = '';
                                 switch (item.fieldName) {
                                     case 'CarrTarEquipName':
                                         dataItem = record['CarrierEquipCode'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipDescription':
                                         dataItem = record['CarrierEquipDescription'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipCasesMinimum':
                                         dataItem = record['CarrierEquipCasesMinimum'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipCasesConsiderFull':
                                         dataItem = record['CarrierEquipCasesConsiderFull'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipCasesMaximum':
                                         dataItem = record['CarrierEquipCasesMaximum'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipWgtMinimum':
                                         dataItem = record['CarrierEquipWgtMinimum'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipWgtConsiderFull':
                                         dataItem = record['CarrierEquipWgtConsiderFull'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipWgtMaximum':
                                         dataItem = record['CarrierEquipWgtMaximum'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipCubesMinimum':
                                         dataItem = record['CarrierEquipCubesMinimum'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipCubesConsiderFull':
                                         dataItem = record['CarrierEquipCubesConsiderFull'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipCubesMaximum':
                                         dataItem = record['CarrierEquipCubesMaximum'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipPltsMinimum':
                                         dataItem = record['CarrierEquipPltsMinimum'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipPltsConsiderFull':
                                         dataItem = record['CarrierEquipPltsConsiderFull'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipPltsMaximum':
                                         dataItem = record['CarrierEquipPltsMaximum'];
                                         blnUpdate = true;
                                         break;
                                     case 'CarrTarEquipTempType':
                                         dataItem = record['CarrierEquipTempType'];
                                         blnUpdate = true;
                                         break;
                                 }
      
                                 if (blnUpdate == true) {                                     
                                     if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                                         if (dataItem !== true) {dataItem = false;}
                                         $("#" + item.fieldTagID).prop('checked', dataItem); 
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                                         if (dataItem !== true) {dataItem = false;}
                                         $("#" + item.fieldTagID).data("kendoSwitch").check(dataItem);
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox){
                                         $("#" + item.fieldTagID).data("kendoNumericTextBox").value(dataItem);
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                                         $("#" + item.fieldTagID).data("kendoDatePicker").value(dataItem);
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                                         $("#" + item.fieldTagID).data("kendoDateTimePicker").value(dataItem);
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                                         $("#" + item.fieldTagID).data("kendoTimePicker").value(dataItem); 
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                                         $("#" + item.fieldTagID).data("kendoEditor").value(dataItem);
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                                         $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: dataItem });
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                                         $("#" + item.fieldTagID).data("kendoDropDownList").value(dataItem);
                                     } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                                         $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(dataItem);
                                     } else {
                                         $("#" + item.fieldTagID).value(dataItem);
                                    }
                                 }
                             }); 
	 
                         }
                         else {                            
                             ngl.showErrMsg("Read Service Equipment Failure", "No Data available", tPage);
                         }
                     }
                 } 
             } catch (err) {
                 ngl.showErrMsg(err.name, err.message, tPage);
             }
         }
         function readServiceEquipmentAjaxErrorCallback(xhr, textStatus, error){
             //try {
             //    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
             //} catch (err) {
             //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
             //}
             ngl.showErrMsg("Read Service Equipment Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tPage);
         }

         //this.deleteServicesSuccessCallback = function (data) {

         //    var oResults = new nglEventParameters();
         //    var tObj = this;
         //    oResults.source = "deleteServicesSuccessCallback";
         //    oResults.widget = tObj;
         //    oResults.msg = 'Failed'; //set default to Failed  
         //    oResults.CRUD = "delete"
         //    var eParent = $('#center-pane');
         //    if (typeof (eParent) !== 'undefined' && ngl.isObject(eParent)) {
         //        kendo.ui.progress(eParent, false);
         //    }
         //    //clear any old return data in rData
         //    this.rData = null;
         //    try {            
         //        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
         //            if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
         //                oResults.error = new Error();
         //                oResults.error.name = "Delete Services Failure";
         //                oResults.error.message = data.Errors;
         //                ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
         //            } else {
         //                if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
         //                    oResults.datatype = "bool";
         //                    oResults.data = data.Data[0];
         //                    oResults.msg = "Success"
         //                    ngl.showSuccessMsg("Success your services record has been deleted", tObj);
         //                }
         //                else {
         //                    oResults.error = new Error();
         //                    oResults.error.name = "Unable to delete your services record";
         //                    oResults.error.message = "The delete procedure returned false, please refresh your data and try again.";
         //                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
         //                }
                    
         //                if (typeof (wdgtTariffServiceDataEdit) !== 'undefined' && ngl.isObject(wdgtTariffServiceDataEdit) === true) {  
         //                    wdgtTariffServiceDataEdit.read(0);
         //                }
         //            }
         //        } else {
         //            oResults.msg = "Success but no data was returned. Please refresh your page and check the results.";
         //            ngl.showSuccessMsg(oResults.msg, null);
         //        }
         //    } catch (err) {
         //        oResults.error = err
         //        ngl.showErrMsg(err.name, err.message, null);
         //    }            

         //}

         //this.deleteServicesAjaxErrorCallback = function (xhr, textStatus, error) {
         //    var oResults = new nglEventParameters();
         //    var tObj = this;
         //    var eParent = $('#center-pane');
         //    if (typeof (eParent) !== 'undefined' && ngl.isObject(eParent)) {
         //        kendo.ui.progress(eParent, false);
         //    }
         //    oResults.source = "deleteServicesAjaxErrorCallback";
         //    oResults.widget = tObj;
         //    oResults.msg = 'Failed'; //set default to Failed
         //    oResults.CRUD = "delete"
         //    oResults.error = new Error();
         //    oResults.error.name = "Delete Services Failure"
         //    oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
         //    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

         //}

         function TariffFeesGridCB(oResults){            

             if (!oResults) { return;}
             if (oResults.source == "readSuccessCallback" ){                
                 try {
                     var blnGetDefault = false;                
                     if(typeof (oResults.data) === "undefined" || ngl.isArray(oResults.data) === false){
                         blnGetDefault = true;
                     } else {
                         if (!oResults.data[0]) {
                             blnGetDefault = true;
                         } else if  (oResults.data[0].CarrTarEquipCarrTarControl === 0){
                             blnGetDefault = true;
                         }
                     }
                     if( blnGetDefault == true){
                         //set the default using the Summary Data
                         var iSummaryCarrTarControlID = wdgtvCarrierTariffSummarySummary.GetFieldID("CarrTarControl");
                         if (!iSummaryCarrTarControlID) {return;}
                         var iCarrTarControl = $("#" + iSummaryCarrTarControlID).val();
                         var iCarrTarEquipCarrTarControlID = wdgtTariffServiceDataEdit.GetFieldID("CarrTarEquipCarrTarControl");
                         if (!iCarrTarEquipCarrTarControlID) {return;}
                         $("#" + iCarrTarEquipCarrTarControlID).val(iCarrTarControl);
                     }           
                 } catch (err) {
                     ngl.showErrMsg(err.name, err.message, tPage);
                 }
             }
         }


         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
                       
           
            if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                //}, 1,this);

            }
            
           var PageReadyJS = <%=PageReadyJS%>;
             //setTimeout(function () {
               menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if  (typeof (divWait) !== 'undefined' ) {
                    divWait.hide();
                }
            //}, 1, this);

        });


    </script>
    <style>

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
       
        .k-grid tbody .k-grid-Edit {
        min-width: 0;
      }

      .k-grid tbody .k-grid-Edit .k-icon {
        margin: 0;
      }
    </style>
    
    </div>
</body>
</html>
