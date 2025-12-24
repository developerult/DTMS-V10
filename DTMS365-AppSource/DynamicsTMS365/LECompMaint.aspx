<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LECompMaint.aspx.cs" Inherits="DynamicsTMS365.LECompMaint" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Company Maintenance</title>
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

                            <div id="divTitleLE"></div>
                           

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



          <div id="wndCopyCompFeeConfig">
              <div style="margin-bottom:5px;"><strong>Copy From: </strong><strong id="lblCopyFromCompName"></strong></div>
              <div>
                  <strong>Copy To: </strong>
                  <div id="copyToGrid"></div>
              </div>
              <div style="margin-top:5px;">
                  <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" id="btnCopyOk" onclick="btnCopyOk_Click();">Copy Config</button>
              </div>
              <input id="txtCopyFromCompControl" type="hidden" />
          </div>

    <% Response.WriteFile("~/Views/ChangeLEDialog.html"); %>
 <%--   <% Response.WriteFile("~/Views/CompMaintEditWindow.html"); %>--%>
    <% Response.WriteFile("~/Views/CompContEAWindow.html"); %> 
    <% Response.WriteFile("~/Views/CreditLimitAssignWnd.html"); %> <%--Added By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes--%>
    <% Response.Write(PageTemplates); %>    

    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>       
    <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var oLECompGrid = null;
        var wndCopyCompFeeConfig = kendo.ui.Window;
        var tObj = this;
        var tPage = this; 
        var tObjPG = this;
        var pgLEControl = 0;
        var pgLEName = "";                    
        var iCompPK = 0;
        var selectedCompName = "";

        <% Response.Write(NGLOAuth2); %>


        <% Response.Write(PageCustomJS); %>
        //***************** Widgets ******************************

        //~~~~~~~~~~ChangeLEDialogCtrl~~~~~~~~~~~~~~~~~~~ 
        var wndChangeLEDialog = kendo.ui.Window;
        var oChangeLEDialogCtrl = new ChangeLEDialogCtrl()
        //Widgit call backs

        function execBeforecarriersInsert(e,fk,w){
            if (!pgLEControl) { alert("No Legal Entity Field - cannot insert record"); return false;}
            return w.SetFieldDefault("LECompCarLEAControl",pgLEControl.toString());            
        }

        function execBeforeaccessorialsInsert(e,fk,w){
            if (!pgLEControl) { alert("No Legal Entity Field - cannot insert record"); return false;}
            return w.SetFieldDefault("LECompAccessorialLEAControl",pgLEControl.toString());            
        }

        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk           
        }

        function saveCompPK() {
            try {
                var row = oLECompGrid.select();
                if (typeof (row) === 'undefined' || row == null) { ngl.showValidationMsg("Warehouse Record Required", "Please select a Warehouse to continue", tPage); return false; }                                  
                var dataItem = oLECompGrid.dataItem(row); //Get the dataItem for the selected row
                if (typeof (dataItem) === 'undefined' || dataItem == null) { ngl.showValidationMsg("Warehouse Record Required", "Please select a Warehouse to continue", tPage); return false; }  
                if ("CompControl" in dataItem){            
                    if("CompName" in dataItem){selectedCompName = dataItem.CompName;}else{selectedCompName = "";}
                    iCompPK = dataItem.CompControl;
                    var setting = {name:'pk', value: iCompPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("Comp/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                    return true;
                } else { ngl.showValidationMsg("Warehouse Record  Required", "Invalid Record Identifier, please select a Warehouse to continue", tPage); return false; }
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
        }

        function isCompSelected() {
            if (typeof (iCompPK) === 'undefined' || iCompPK === null || iCompPK === 0) { return saveCompPK(); }
            return true;
        }

        function oChangeLEDialogSaveCB(results){
            if (typeof (results) !== 'undefined' && results != null) { 
                if ('LEAdminControl' in results) { pgLEControl = results.LEAdminControl; }
                if ('LegalEntity' in results) { pgLEName = results.LegalEntity; }            
                $("#divTitleLE").html("<h2>" + pgLEName + "</h2>");                
                //We need to reset the page before refresh to avoid problematic behavior since we are completely changing the data returned (not just filtering or sorting from one retured record set)
                oLECompGrid.dataSource.page(1);
                refreshLECompGrid();
            }
        }  
        
        //functions
        function changeLegalEntity() { oChangeLEDialogCtrl.show(); }


        //~~~~~~~~~~CompMaintEAWndCtrl~~~~~~~~~~~~~~~~~~~
        var wndCompMaintEA = kendo.ui.Window;
        var wdgtCompMaintEA = new CompMaintEAWndCtrl()
        //Widgit call backs
        function wdgtCompMaintEASaveCB(results){
            //We need to reset the page before refresh to avoid problematic behavior since we are completely changing the data returned (not just filtering or sorting from one retured record set)
            oLECompGrid.dataSource.page(1);
            refreshLECompGrid();
        }
        //functions
        function openAddNewCompWindow() {
            //clear the data and set the LE to the selected value from the screen for Add New
            wdgtCompMaintEA.data = null;
            wdgtCompMaintEA.screenLEName = pgLEName;
            ////pgLEControl
            wdgtCompMaintEA.show();       
        }


        //function openEditCompWindow(e) {
        //    var item = this.dataItem($(e.currentTarget).closest("tr")); 
        //    //oCompMaintData = new CompMaint(); 
        //    //wdgtCompMaintEA.data = oCompMaintData;
        //    wdgtCompMaintEA.data = item;
        //    wdgtCompMaintEA.show();            
        //}

        //var deleteCompControl = 0;
        //function deleteCompany(e){
        //    var item = this.dataItem($(e.currentTarget).closest("tr"));
        //    deleteCompControl = 0; //clear any old values
        //    if (typeof (item) !== 'undefined' && item != null) {          
        //        if ('CompControl' in item) { 
        //            if (typeof (item.CompControl) === 'undefined' || item.CompControl == null || item.CompControl == 0) { ngl.showErrMsg("CompControl Required", "CompControl cannot be 0", null); return; }              
        //            deleteCompControl = item.CompControl;
        //        } 
        //        else { ngl.showErrMsg("CompControl Required", "Row object does not contain property CompControl.", null); return; }

        //        if (deleteCompControl == 0) { ngl.showErrMsg("Delete Error", "Could not get CompControl from the record", null); return; }

        //        //perform confirmation
        //        var title = "Delete Warehouse";
        //        ngl.OkCancelConfirmation(
        //            title,
        //            "Are you sure that you want to delete this Warehouse?",
        //            400,
        //            400,
        //            null,
        //            "ConfirmDeleteComp"); 
        //    } 
        //    else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; }
        //}
        //function ConfirmDeleteComp(iRet){          
        //    if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
        //    //Chose the Ok action
        //    if(typeof (deleteCompControl) !== 'undefined' && deleteCompControl > 0){   
        //        var tObj = tObjPG;
        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
        //        var blnRet = oCRUDCtrl.delete("Comp/Delete", deleteCompControl, tObj, "DeleteCompCallback", "DeleteCompAjaxErrorCallback");           
        //    }
        //}
        //function DeleteCompCallback(data) {
        //    var oResults = new nglEventParameters();
        //    oResults.source = "DeleteCompCallback";
        //    oResults.widget = this;
        //    oResults.msg = 'Failed'; //set default to Failed     
        //    this.rData = null;
        //    var tObj = this;
        //    try {
        //        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
        //            if (ngl.stringHasValue(data.Errors)) {
        //                oResults.error = new Error();
        //                oResults.error.name = "Delete Warehouse Failure";
        //                oResults.error.message = data.Errors;
        //                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        //            } else{ this.rData = data.Data; }
        //        }
        //    } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }           
        //    if (ngl.isFunction(refreshLECompGrid)) { refreshLECompGrid(); }
        //}
        //function DeleteCompAjaxErrorCallback(xhr, textStatus, error) {
        //    var oResults = new nglEventParameters();
        //    var tObj = this;
        //    oResults.source = "DeleteCompAjaxErrorCallback";
        //    oResults.widget = this;
        //    oResults.msg = 'Failed'; //set default to Failed  
        //    oResults.error = new Error();
        //    oResults.error.name = "Delete Warehouse Failure"
        //    oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        //    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);        
        //    if(ngl.isFunction(refreshLECompGrid)){ refreshLECompGrid(); }
        //}

    

        //************* Action Menu Functions ********************
        function execActionClick(btn, proc){
            if(btn.id == "btnChangeLE"){ changeLegalEntity(); }
            else if(btn.id == "btnAddCompMaint30"){ 
                //openAddNewCompWindow();
                if (typeof (tPage["wdgtAddCompWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtAddCompWndDialog"])){ 
                    tPage["wdgtAddCompWndDialog"].SetFieldDefault("CompLegalEntity",pgLEName); 
                    tPage["wdgtAddCompWndDialog"].show();
                } else{alert("Missing HTML Element (wdgtAddCompWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
            }
            else if(btn.id=="btnOpenCompEDI"){
                if (isCompSelected() === true) {
                    var setting = {name:'pk', value: iCompPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("CompEDI/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);             
                    location.href = "CompEDI";
                }
            }
            else if(btn.id == "btnOpenTolerance"){            
                if (typeof (tPage["wdgtTolerancePopUpDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtTolerancePopUpDialog"])){ 
                    if (isCompSelected() === true) {
                        tPage["wdgtTolerancePopUpDialog"].read(iCompPK);
                    }
                } else{alert("Missing HTML Element (wdgtTolerancePopUpDialog is undefined)");} //Add better error handling here if cm stuff is missing                           
            }           
            else if(btn.id == "btnAddCompCont30"){ openAddCompContWindow(); }
            else if(btn.id == "btnCopyWhseFeeConfig"){ if (isCompSelected() === true) {openCopyLECompFeeWindow();} }
            else if(btn.id == "btnCalcCompLatLong"){ if (isCompSelected() === true) { RecalculateLatLong(); } } //Added By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes
            else if(btn.id == "btnOpenCompFin"){
                //Added By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes
                if (isCompSelected() === true) {
                    var setting = {name:'pk', value: iCompPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("CompFinancial/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);             
                    location.href = "CompanyFinancial";
                }
            }  
            else if(btn.id == "btnRefresh" || btn === "Refresh" ){ refresh(); }
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }       
            else if(btn.id == "btnAssignCreditLimits"){ openCreditLimitAssignWnd(pgLEControl); } //Added By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes
        }

        function refresh() { ngl.readDataSource(oLECompGrid); }
        
       
        //************* Call Back Functions **********************        
        var blnCompGridChangeBound = false;
        function LECompGridDataBoundCallBack(e,tGrid){           
            oLECompGrid = tGrid;
            if (blnCompGridChangeBound == false){
                oLECompGrid.bind("change", saveCompPK);
                blnCompGridChangeBound = true;
            }
        }
       
        function LECompGridGetStringData(s){ 
            s.LEAdminControl = pgLEControl;
            return '';
        }

        var GetLEAdminNotAsyncCB = function (data) {
            pgLEControl = data[0].LEAdminControl;
            pgLEName = data[0].LegalEntity;      
            $("#divTitleLE").html("<h2>" + pgLEName + "</h2>"); 
        }

        function AddCompWndCB(oResults){          
            if(oResults.widget.sNGLCtrlName === "wdgtAddCompWndDialog" && oResults.source === "saveSuccessCallback"){
                refresh();
                oResults.widget.executeActions("close");
            }          
        }
       

        //************* Page Functions ***************************
        function refreshLECompGrid() {
            if (typeof (oLECompGrid) !== 'undefined' && ngl.isObject(oLECompGrid)) {
                oLECompGrid.dataSource.read();
            }
        }

        function openCopyLECompFeeWindow() {
            $("#txtCopyFromCompControl").val(iCompPK);
            $("#lblCopyFromCompName").html(selectedCompName);
            $("#copyToGrid").data("kendoGrid").clearSelection();
            $("#copyToGrid").data("kendoGrid").dataSource.read();
            wndCopyCompFeeConfig.center().open();
        }

        function btnCopyOk_Click(){
            var msgPrompt = "This action will overwrite the existing Accessorial configuration for the selected Warehouses. Are you sure that you want to proceed?";                
            kendo.confirm(msgPrompt).then(function () {
                //kendo.alert("You chose the Ok action.");
                if (!pgLEControl) { alert("There was a problem getting the Legal Entity - cannot copy record"); return false;}
                var copyToComps = $("#copyToGrid").data("kendoGrid").selectedKeyNames();
                if(typeof (copyToComps) !== 'undefined' && ngl.isArray(copyToComps) && copyToComps.length > 0){
                    var s = new GenericResult();  
                    s.intArray = copyToComps;
                    s.intField1 = $("#txtCopyFromCompControl").val();
                    s.Control = pgLEControl;
                    
                    $.ajax({
                        type: 'POST',
                        url: 'api/LECompAccessorial/CopyCompAccessorialConfig',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify(s),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) { wndCopyCompFeeConfig.close(); refreshLECompGrid(); },
                        //error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                    });
                }
            }, function () {
                //kendo.alert("You chose to Cancel action.");
                wndCopyCompFeeConfig.close();
                refreshLECompGrid();
            });          
        }



        /* Summary - Calls the REST method to recalculate the lattitude and longitude */
        function ConfirmRecalculateLatLong(iRet) {
            if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; } //Chose the Cancel action
            //Chose the Ok action        
            kendo.ui.progress(oLECompGrid.element, true);
            $.ajax({
                type: 'GET',
                url: 'api/Comp/RecalculateLatLong/' + iCompPK,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {
                    kendo.ui.progress(oLECompGrid.element, false); 
                    try {
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Recalculate Lat Long Failure", data.Errors, null); }
                            else {                               
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = data.Data[0].Success;
                                        if (data.Data[0].Success === true){ ngl.showSuccessMsg(ngl.replaceEmptyString(data.Data[0].SuccessMsg,'Success!'), null); }
                                        if (ngl.stringHasValue(data.Data[0].ErrMsg)){ ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Recalculate Lat Long Error'), data.Data[0].ErrMsg, null); }                                        
                                        if (ngl.stringHasValue(data.Data[0].WarningMsg)){ ngl.showWarningMsg(ngl.replaceEmptyString(data.Data[0].WarningTitle,'Recalculate Lat Long Warning'), data.Data[0].WarningMsg, null); }                                                                                                                             
                                        refreshLECompGrid();
                                    }                              
                                }                            
                            }
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { kendo.ui.progress(oLECompGrid.element, false); var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Recalculate Lat Long Failure", sMsg, null); }
            });
        }

        /* Summary - Verifies that the user wants to recalculate the lattitude and longitude and if so calls ConfirmRecalculateLatLong() */
        function RecalculateLatLong() {
            //perform confirmation
            var title = "Recalculate Lattitude and Longitude";
            ngl.OkCancelConfirmation(
                title,
                "This action will overwrite any existing Lattitude and Longitude data for the Warehouse. Are you sure you want to proceed?",
                400,
                400,
                null,
                "ConfirmRecalculateLatLong");
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
         

            //NOTE: This has to be done before PageReadyJS because the grid is dependent on the LEControl value                
            //Get the LE for the user
            getLEAdminNotAsync(0, GetLEAdminNotAsyncCB);
 
            if (control != 0){
                setTimeout(function () {  

                    ////////////ChangeLEDialogCtrl///////////////////
                    oChangeLEDialogCtrl = new ChangeLEDialogCtrl();
                    oChangeLEDialogCtrl.loadDefaults(wndChangeLEDialog, oChangeLEDialogSaveCB);

                    /*
                    ////////////CompMaintEAWndCtrl///////////////////
                    wdgtCompMaintEA = new CompMaintEAWndCtrl(); 
                    wdgtCompMaintEA.loadDefaults(wndCompMaintEA, wdgtCompMaintEASaveCB);   
                    */

                    $("#copyToGrid").kendoGrid({
                        dataSource: {
                            pageSize: 10,
                            transport: {                            
                                read: function(options) { 
                                    var s = new AllFilter();                                    
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;
                                    if (typeof (LECompGridGetStringData) !== 'undefined' && ngl.isFunction(LECompGridGetStringData)) {s.Data = LECompGridGetStringData(s); } else { s.Data = '';}; 
                                    $.ajax({ 
                                        url: 'api/Comp/GetRecords/', 
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
                                    id: "CompControl"
                                }
                            }
                        },
                        pageable: true,
                        scrollable: false,
                        persistSelection: true,
                        sortable: true,
                        columns: [
                            { selectable: true, width: "50px" },
                            { field:"CompName", title: "Name" },
                            { field: "CompNumber", title:"Number", hidden: true }
                        ]
                    });

                    /////////wndCopyCompFeeConfig//////////////
                    wndCopyCompFeeConfig = $("#wndCopyCompFeeConfig").kendoWindow({
                        title: "Copy Warehouse Accessorial Config",
                        modal: true,
                        visible: false              
                    }).data("kendoWindow");
            
               }, 10,this);          
            }
            var PageReadyJS = <%=PageReadyJS%>;        
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined') { divWait.hide(); } 

            


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
