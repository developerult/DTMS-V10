<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateEDITPDocument.aspx.cs" Inherits="DynamicsTMS365.CreateEDITPDocument" %>

<!DOCTYPE html>

<html>
<head>
    <title>Create EDI Document</title>
    <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />
    <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />
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

        .hide-display {
            display: none;
        }

        .breakWord20 {
            word-break: break-all !important;
            word-wrap: break-word !important;
            vertical-align: top;
        }

        .k-grid-header .k-header {
            overflow: visible !important;
            white-space: normal !important;
        }

        #LoopDesc {
            text-overflow: ellipsis !important;
        }
        .ui-container {
            height: 100% !important; width: 100% !important; margin-top: 2px !important;
        }
        .ui-vertical-container {
            height: 98% !important; width: 98% !important; 
        }
        .ui-horizontal-container {
            height: 100% !important; width: 100% !important;
        }
        .ui-id260-container {
            margin-top: 10px; 
        }
        .ui-legend-container {
            color:black;font-weight:bold; 
        }
        .ui-fieldset-container {
            border-color:#BBDCEB; border-width:1px;width:1300px;margin:10px; margin-left:30px;
        }
        .ui-border-container {
            margin-left: 500px; margin-top: 70px; margin-bottom: 250px;
        }
        .ui-margin-Document {
            margin-top: 12px;
        }
        .ui-button-margin {
            width:100px;
        }
        .ui-th-margin {
            width: 150px;
        }
        .ui-td-margin {
            width: 225px;
        }
        .ui-span-container {
            color: red;position: relative;
        }
        .ui-button-container {
            margin-left: 23px; width: 100px;
        }
    </style>

</head>
<body>
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
    <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
    <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/splitter2.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/menuitems.js"></script>
    <!-- added by SRP on 3/8/2018 For Dynamically adding menu items for all pages -->
    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 3/8/2018 For Editing KendoWindow Configuration from Javascript -->

    <div id="example" class="ui-container">
        <div id="vertical" class="ui-vertical-container">
            <%--Action Menu TabStrip--%>
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" class="ui-horizontal-container">
                    <div id="left-pane">
                        <div class="pane-content">
                            <div><span>EDI - MAINTENANCE</span></div>
                            <%--Page Navigation Menu Tree--%>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <!-- Begin Page Content -->

                        <%--Message--%>
                        <!-- Grid Fast Tab -->
                        <div id="id260" class="ui-id260-container">
                            <div id="ParHeader" class="OpenOrders">
                                <div id="Parwrapper">
                                    <%--Filters--%>
                                      <fieldset id="form" class="ui-fieldset-container">
                                        <legend class="ui-legend-container"><span class='k-icon k-i-add'></span>Create TP EDI Document</legend>
                                          <div  class="ui-border-container">
                                              <div>
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top ui-th-margin" >Company Name:</td>
                                                        <td class="tblResponsive-top"><input id="ddlCompanyName" class="ui-td-margin" /><br><br><span id="company-validation" class="hide-display ui-span-container" style="">Please Select Comany</span></td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div>
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top ui-th-margin">Carrier Name:</td>
                                                        <td class="tblResponsive-top"><input id="ddlCarrier" class="ui-td-margin" /></td>
                                                    </tr>
                                          
                                                </table>
                                            </div>
                                             <div class="ui-margin-Document">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top ui-th-margin">EDI Document Type:</td>
                                                        <td class="tblResponsive-top"><input id="ddlEDIDocumentType" class="ui-td-margin" /><br/><br/><span id="document-validation" class="hide-display ui-span-container">Please Select Documnet</span></td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div>
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top ui-th-margin">Transaction Direction:</td>
                                                        <td class="tblResponsive-top"><input id="ddlTransactionDirection" class="ui-td-margin" /><br/><br/></td>
                                                    </tr>
                                                </table>
                                            </div>                                             

                                            <div class="ui-margin-button">
                                                <table class="tblResponsive">
                                                    <tr>
                                                        <td class="tblResponsive-top ui-th-margin"></td>
                                                        <td class="tblResponsive-top"><button id="btnNext" class="ui-button-margin" >Next</button> <button id="btnCancel" class="ui-button-container">Cancel</button></td>
                                                    </tr>
                                                </table>
                                            </div>

                                          </div>
                                  </fieldset>
                                </div>
                            </div>
                        </div>

                        <!-- End Page Content -->
                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block ui-horizontal-container">
                <div class="pane-content">
                    <div>
                        <span>
                            <p>If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></p>
                        </span>
                    </div>
                </div>
            </div>
        </div>


        <%--Added By SRP on 3/8/18 EDILoop for KendoWindow--%>
        <% Response.WriteFile("~/Views/EDILoopAddWindow.html"); %>

        <% Response.Write(AuthLoginNotificationHTML); %>
        <script>
        //************* Page Variables **************************
            var PageControl = '<%=PageControl%>';   
            var tObj = this;
            var tPage = this;
        var oKendoGrid = null;
        var dsListTypeDropDown = kendo.data.DataSource;
        var dsLGTDropDown = kendo.data.DataSource;
        var dsLEADropDown = kendo.data.DataSource;
        var wndMessage = kendo.ui.Window;
        var wndAddDocType = kendo.ui.Window; 

        //************Hard Codeed For Transaction Direction********//
        var ddlTransaction = [{ TrControl:true, TrName:"InBound"},{ TrControl:false, TrName:"OutBound"}];
            
        //define Kendo dropdown widgets
        $("#ddlCompanyName").kendoDropDownList();   
        $("#ddlCarrier").kendoDropDownList();   
        $("#ddlEDIDocumentType").kendoDropDownList();  
        $("#ddlTransactionDirection").kendoDropDownList(); 

        $("#btnNext").kendoButton();  
        $("#btnCancel").kendoButton(); 
        

        //*************  Call Back Functions  *******************
        function EDILoopGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;
            //add databound code here
        }

        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
            if(btn.id == "btnAddLoop"){ 
                openLoopAddWindow();
            }
        }
        //****Button Events for NEXT and CANCEL**********//
        $("#btnNext").click(function(){
            var submit = true;
            var Company = $("#ddlCompanyName").val();
            if (Company =="") {
                $("#company-validation").removeClass("hide-display");
                submit = false;
            }
            var Document = $("#ddlEDIDocumentType").val();
            if (Document =="") {
                $("#document-validation").removeClass("hide-display");
                submit = false;
            }
            var item = new NGLElement();

            var dataItem1 = $("#ddlCarrier").data("kendoDropDownList").dataItem();
            var TPDocCCEDIControl  = dataItem1.CCEDIControl;
           
            var TPDocControl=0;
            var dataItem4 = $("#ddlTransactionDirection").data("kendoDropDownList").dataItem();
            var TPDocEDITControl  = dataItem4.MasterDocControl;
            
            var dataItem6 = $("#ddlCompanyName").data("kendoDropDownList").dataItem();
            var company  = dataItem6.CompName;

            var dataItem7 = $("#ddlCarrier").data("kendoDropDownList").dataItem();
            var CARRIER  = dataItem7.CarrierName;

            var dataItem8 = $("#ddlEDIDocumentType").data("kendoDropDownList").dataItem();
            var EDITNAME  = dataItem8.EDITName;
             
            var dataItem9 = $("#ddlTransactionDirection").data("kendoDropDownList").dataItem();
            var TPDocInbound  = dataItem9.MasterDocInbound;
            if(TPDocInbound=="InBound")
            {
                TPDocInbound=true
            }
            else
            {
                TPDocInbound=false
            }

            $.ajax({
                async: false,
                url: "api/EDITPDocument/GetRecordByCarrier/",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: { TPDocCCEDIControl: TPDocCCEDIControl,TPDocEDITControl:TPDocEDITControl,TPDocInbound:TPDocInbound },
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("EDI MasterDoc Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        submit = false;
                                        ngl.showWarningMsg("A document already exists for selected company and carrier.<br><br>Please check the list of Trading Partner documents.", strValidationMsg);
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                           
                        }
                    } catch (err) {
                        submit = false;
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save EDI MasterDoc Failure");
                    ngl.showErrMsg("EDI MasterDoc Failure", sMsg, null);
                }
            }); 
            var dataJSON = {TPDocControl:TPDocControl, TPDocCCEDIControl: TPDocCCEDIControl,TPDocEDITControl:TPDocEDITControl,TPDocInbound:TPDocInbound,Action:"ADD" };
            
            if(submit == true){
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "api/EDITPDocument/SaveEDITPDocumentMap",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: JSON.stringify(dataJSON),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("Add EDITPDocument Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            blnSuccess = true;
                                            window.location.assign("EDIDocStructLoops?TPDocControl="+ data.Data[0].TPDocControl+"&EDITName="+EDITNAME+"&CompName="+company+"&CarrierName="+CARRIER+"&TPDocInbound="+TPDocInbound+"");
                                        }
                                        else
                                        {
                                            ngl.showWarningMsg("A document already exists for selected company and carrier.<br><br>Please check the list of Trading Partner documents.", strValidationMsg);
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Add MDSElememts Failure"; }
                                ngl.showWarningMsg("A document already exists for selected company and carrier.<br><br>Please check the list of Trading Partner documents.", strValidationMsg);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Add EDITPDocument Failure");
                        ngl.showErrMsg("Add EDITPDocument Failure", sMsg, null);
                    }
                });
            }
           
        });

        //********On change Validation***********//
        $("#ddlCompanyName").on("change input", function () {
            var datatypesval = $(this).val();
            if (datatypesval != 0) {
                $("#company-validation").addClass("hide-display");
            }
            else {
                $("#company-validation").removeClass("hide-display");
            }
        });

        $("#ddlEDIDocumentType").on("change input", function () {
            var datatypesval = $(this).val();
            if (datatypesval != 0) {
                $("#document-validation").addClass("hide-display");
            }
            else {
                $("#document-validation").removeClass("hide-display");
            }
        });

        $("#btnCancel").click(function(){
            //*****Back to TP DOc Page*******//
            window.location.assign("EDITPDocument.aspx");
        });

        $("#ddlCompanyName").on("change Comapny", function () {

            var cName = $(this).data("kendoDropDownList").dataItem().CompName;

            dsCarrier = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDITPDocument/GetCompCarriers",
                        contentType: 'application/json; charset=utf-8',
                        dataType: "json",
                        data: { filter: JSON.stringify({"filterName":"None","filterValue":"","page":1,"skip":0,"take":100}) }, 
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }                      
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: { 
                        id: "CCEDIControl",
                        fields: {
                            CCEDIControl: { type: "number" },
                            CarrierName: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                filter: {
                    logic: "or",
                    filters: [
                        { field: "CCEDIControl", operator: "eq", value: 0 },
                        { field: "CompName", operator: "eq", value: cName }
                    ]
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsCarrier.read();

            $("#ddlCarrier").kendoDropDownList({
                dataSource: dsCarrier,
                dataTextField: "CarrierName",
                dataValueField: "CCEDIControl",
                autoWidth: true,
                filter: "contains",
                change: function(e) {
                    console.log("Hi Change");
                    var element = e.sender.select();
                    var dataItem = e.sender.dataItem(element[0]);
                   
                }
            });
        });

        $("#ddlEDIDocumentType").on("change Comapny", function () {

            var dName = $(this).data("kendoDropDownList").dataItem().EDITName;

            dsDocTypeInOutBound = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDIMasterDocument/GetMasterEDIDocRecords",
                        contentType: 'application/json; charset=utf-8',
                        dataType: "json",
                        async:false,
                        data: { filter: JSON.stringify({"filterName":"None","filterValue":"","page":1,"skip":0,"take":100}) }, 
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }                      
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: { 
                        id: "MasterDocControl",
                        fields: {
                            MasterDocControl: { type: "number" },
                            EDITName: { type: "string" }, 
                            MasterDocInbound: {type: "boolean"}
                        }
                    }, 
                    errors: "Errors"
                },
                filter: {
                    logic: "and",
                    filters: [
                        { field: "MasterDocPublished", operator: "eq", value: true },
                        { field: "EDITName", operator: "eq", value: dName }
                    ]
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsDocTypeInOutBound.read();

            for(var i=0; i< dsDocTypeInOutBound._data.length;i++){
                if(dsDocTypeInOutBound._data[i].MasterDocInbound == true)
                    dsDocTypeInOutBound._data[i].MasterDocInbound = "InBound";
                else
                    dsDocTypeInOutBound._data[i].MasterDocInbound = "OutBound";
                 }

            $("#ddlTransactionDirection").kendoDropDownList({
                dataSource: dsDocTypeInOutBound,
                dataTextField: "MasterDocInbound",
                dataValueField: "MasterDocControl",
                autoWidth: true,
                filter: "contains",
            });
        });

        $(document).ready(function () {
            var PageMenuTab = $('#tab').kendoTabStrip({
                animation: { 
                    open: { 
                        effects: 'fadeIn'
                    } 
                },
                dataTextField: 'text',
                dataContentField: 'content',
                dataSource: [
                    { 
                        text: 'Actions',
                        content: "",
                    }
                ]
            }).data('kendoTabStrip').select(0);
            
            var PageReadyJS = $('#menuTree').kendoTreeView({
                dataUrlField: 'LinksTo',
                dataSource: {
                    data:menuitems.data
                },
                loadOnDemand: false
            }).data('kendoTreeView'), handleTextBox = function (callback) { return function (e) { if (e.type != 'keypress' || kendo.keys.ENTER == e.keyCode) { callback(e); } }; };

            control = <%=UserControl%>;           

            //************DropDown DataSources***********//

            $.ajax({ 
                url: 'api/EDIMasterDocument/GetMasterEDIDocRecords', 
                contentType: 'application/json; charset=utf-8', 
                dataType: 'json', 
                async: false,
                data: { filter: JSON.stringify({"filterName":"None","filterValue":"","page":1,"skip":0,"take":100}) }, 
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                success: function(data) {
                    dsDocTypeDropDown=data.Data;
                } 
            }); 

            function onlyUnique(mArray) {
                var Fresh = [];
                for(var i = 0; i < mArray.length;i++){
                    if(mArray[i].MasterDocPublished == true){
                        Fresh.push(mArray[i]);
                    }
                }
                var varyFresh = [];
                var DummyFresh = [];
                for(var i = 0; i < Fresh.length;i++){
                    if(!DummyFresh.includes(Fresh[i].EDITName)){
                        DummyFresh.push(Fresh[i].EDITName);
                        varyFresh.push(Fresh[i]);
                    }
                }
                return varyFresh;
            }
            dsDocTypeDropDown = onlyUnique(dsDocTypeDropDown);

            $("#ddlEDIDocumentType").kendoDropDownList({
                dataSource: dsDocTypeDropDown,
                dataTextField: "EDITName",
                dataValueField: "MasterDocControl",
                optionLabel:"Select Document",
                autoWidth: true,
                filter: "contains",
            });
            

            $.ajax({ 
                url: 'api/EDITPDocument/GetCompCarriers', 
                contentType: 'application/json; charset=utf-8', 
                dataType: 'json', 
                async: false,
                data: { filter: JSON.stringify({"filterName":"None","filterValue":"","page":1,"skip":0,"take":100}) }, 
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                success: function(data) {
                    dsCompanyAndCarrier=data.Data;
                } 
            }); 

            function onlyUniqueCompany(cArray) {
                var cFresh = [];
                var cDummyFresh = [];
                for(var i = 0; i < cArray.length;i++){
                    if(!cDummyFresh.includes(cArray[i].CompName)){
                        cDummyFresh.push(cArray[i].CompName);
                        cFresh.push(cArray[i]);
                    }
                }
                return cFresh;
            }
            dsCompanyAndCarrierDropDown = onlyUniqueCompany(dsCompanyAndCarrier);

            $("#ddlCompanyName").kendoDropDownList({
                dataSource: dsCompanyAndCarrierDropDown,
                dataTextField: "CompName",
                dataValueField: "CCEDIControl",
                optionLabel:"Select Company",
                autoWidth: true,
                filter: "contains",
            });
           
            ////////////wnd AddEDILoop/////////////////

            kendoWin.height = 425;
            kendoWin.width = 300;
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddDocType = $("#wndAddEDILoop").kendoWindow(kendoWin).data("kendoWindow");
            $("#wndAddEDILoop").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDILoop(); });
        });

        </script>
    </div>
</body>

</html>
