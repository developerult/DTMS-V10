<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDITPDocument.aspx.cs" Inherits="DynamicsTMS365.EDITPDocument" %>

<!DOCTYPE html>

<html>
<head>
    <title>Manage EDI TP Documents</title>         
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
        .ui-container {
            height: 100% !important; width: 100% !important; margin-top: 2px !important;
        }
        .ui-vertical-container {
            height: 98% !important; width: 98% !important; 
        }
        .ui-horizontal-container {
            height: 100% !important; width: 100% !important;
        }
        .ui-padding-container {
            padding: 10px;
        }
        .ui-id260-container {
            margin-top: 10px; 
        }
        .ui-span-container {
            font-size: small; font-weight: bold;
        }
    </style>

</head>
<body>
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
    <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
    <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/splitter2.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/menuitems.js"></script>
    <!-- added by SRP on 14/02/2018 For Dynamically adding menu items for all pages -->
    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 14/02/2018 For Editing KendoWindow Configuration from Javascript -->

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
                        <div id="txtScreenMessage"></div>

                        <!-- Grid Fast Tab -->
                        <div id="id260" class="ui-id260-container">
                            <div class="fast-tab">
                                <span id="ExpandParSpan" style="display: none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span class="k-icon k-i-chevron-down ui-span-container"></span></a></span>
                                <span id="CollapseParSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span class="k-icon k-i-chevron-up ui-span-container"></span></a></span>
                                <span class="ui-span-container">EDI Document</span>
                            </div>
                            <div id="ParHeader" class="OpenOrders">
                                <div id="Parwrapper">
                                    <%--Filters--%>
                                    <div id="ParFilterFastTab">
                                        <span id="ExpandParFilterFastTabSpan" style="display: none;"><a onclick="expandFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span class="k-icon k-i-chevron-down ui-span-container"></span></a></span>
                                        <span id="CollapseParFilterFastTabSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span class="k-icon k-i-chevron-up ui-span-container"></span></a></span>
                                        <span class="ui-span-container">Filters </span>
                                        <div id="ParFilterFastTabHeader" class="ui-padding-container">
                                            <span>
                                                <label for="ddlEDITypeFilters">Filter by:</label>
                                                <input id="ddlEDITypeFilters" />
                                                <span id="spEDITypeFilterText">
                                                    <input id="txtEDITypeFilterVal" /></span>
                                                <span id="spEDITypeFilterDates">
                                                    <label for="dpEDITypeFilterFrom">From:</label>
                                                    <input id="dpEDITypeFilterFrom" />
                                                    <label for="dpEDITypeFilterTo">To:</label>
                                                    <input id="dpEDITypeFilterTo" />
                                                </span>
                                                <span id="spEDITypeFilterButtons"><a id="btnEDITypeFilter"></a><a id="btnEDITypeClearFilter"></a></span>
                                            </span>
                                            <input id="txtEDITypeSortDirection" type="hidden" />
                                            <input id="txtEDITypeSortField" type="hidden" />
                                        </div>
                                    </div>
                                    <%--Grid--%>
                                    <div id="EDITPDocGrid"></div>
                                </div>
                            </div>
                        </div>

                        <!-- End Page Content -->
                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block ui-horizontal-container">
                <div class="pane-content">
                    <div><span>
                        <p>If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></p>
                    </span></div>
                </div>
            </div>
        </div>

        <%--Popup Window HTML--%>
        <div id="wndMessage">
            <div>
                <h2>Enter a Message</h2>
                <input id="txtUserInput" style="width: 250px;" />
            </div>
        </div>

        <%--Added By SRP on 2/14/18 EDIDocumentType for KendoWindow--%>
        <% Response.WriteFile("~/Views/EDIDocumentEditWindow.html"); %>
       
        <%--Added By SK on May 4, 18 EDITPDocumen for Preview Window--%>
        <% Response.WriteFile("~/Views/PreviewDocStructure.html"); %>

        <% Response.Write(AuthLoginNotificationHTML); %>
        <script>
        //************* Page Variables **************************
        var kendoWinPreview = {
            modal: true,
            visible: false,
            actions: ["file-pdf", "Minimize", "Maximize", "Close"],
        };

        var PageControl = '<%=PageControl%>';
        var DocumentPreview = kendo.ui.Window;
        var oKendoGrid = null;
        var wndMessage = kendo.ui.Window;
        var dsDocTypeDropDownnew= kendo.data.DataSource;
        var dsComapanyDropDownnew= kendo.data.DataSource;
        var dsComp= kendo.data.DataSource;
        var dsDocTypeDropDownnew= kendo.data.DataSource;
        var dsCarrier= kendo.data.DataSource;
        var ddlTransaction = [{ TrControl:true, TrName:"InBound"},{ TrControl:false, TrName:"OutBound"}];
        var wndAddDocType = kendo.ui.Window; //Added By SRP on 2/14/18 EDIDocumentType


        //*************  Call Back Functions  *******************
        function EDITPDocGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;
        }
        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
         
            if(btn.id == "btnAddDocumentType"){ 
                window.location.assign("CreateEDITPDocument.aspx");
            }
        }

        function showDetails(e) {  
            e.preventDefault();
            var d = this.dataItem($(e.currentTarget).closest("tr"));
            window.location.assign("EDIDocStructLoops?TPDocControl="+d.TPDocControl+"&EDITName="+d.EDITName+"&CompName="+d.CompName+"&CarrierName="+d.CarrierName+"&TPDocInbound="+d.TPDocInbound+"");
        }

        //**********Export PDF ************//
        function exportPDF(selector) {
            kendo.drawing.drawDOM("#content", {
                forcePageBreak: ".new-page",
                paperSize: "A4",
                margin: "1cm",
                multiPage: true,
            }).then(function(group){
                kendo.drawing.pdf.saveAs(group, DocumentName+".pdf");
            });
        }

        var DocumentName;

        function showPreview(e) {  
            e.preventDefault();
            var rowItems = this.dataItem($(e.currentTarget).closest("tr"));
            $("#show").empty();
            $.ajax({ 
                url: 'api/EDITPDocument/GetTPDocumentPreview', 
                contentType: 'application/json; charset=utf-8', 
                dataType: 'json', 
                async: false,
                data: { TPDocControl: rowItems.TPDocControl},  
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                success: function(data) {
                    DocumnetDetails=data.Data;
                } 
            }); 

                //*****Tital Of Window********//
            $("#DocumentPreview").data("kendoWindow").title("Document Preview");

            //*************Date Format**************//
            var m_names = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');
            var d = new Date();
            var curr_date = d.getDate();
            var curr_month = d.getMonth();
            var curr_year = d.getFullYear();

             if(DocumnetDetails.length>0){
                
            //*************PDF file name********//
            DocumentName=DocumnetDetails[0].EDITName+"-"+DocumnetDetails[0].TPDocType;
            //*********Header of The View*************//
            var DocHead = '<% = System.Configuration.ConfigurationManager.AppSettings["NGLApplicationName"]%>' ;
            var header="";
            header += "<h3 class='Docheader'>"+DocHead+"</h3>";
            header += "<h4 class='DOCDetails'>"+DocumnetDetails[0].EditDescription+"</h4>"
            header += "<h4 class='DOCDetails'>"+DocumnetDetails[0].TPDocType+" "+DocumnetDetails[0].EDITName+"</h4>"
            header += "<h4 class='DOCDetails'>Version "+DocumnetDetails[0].Version+"</h4>"
            header += "<h5 class='DOCDetails'>"+DocumnetDetails[0].CompName+" - "+DocumnetDetails[0].CarrierName+"</h5>"
            header += "<h5 class='cuttentDate'>"+ m_names[curr_month]+" "+ curr_date +", " + curr_year+"</h5>"
            header += "<div class='hederHR'></div>"
           // $("#show").append(header);

                var DSLoopParentLoopID = 0;
                var structhtml = header;
                var count = 0;

                ShowDocument(DocumnetDetails,DSLoopParentLoopID);

                function ParentLoops(lArray,DSLoopParentLoopID){
                    var parentLoopsArray=[];
                    for(var i=0;i<lArray.length;i++){
                        if(lArray[i].DSLoopParentLoopID == DSLoopParentLoopID){
                            parentLoopsArray.push(lArray[i]);
                        }
                    }
                    return parentLoopsArray;
                }
            
                function uniqueParentLoops(ulArray){
                    var uniqueparentLoopsArray=[];
                    var uniqueparentLoopsArrayObject=[];
                    for(var i=0;i<ulArray.length;i++){
                        if(!uniqueparentLoopsArray.includes(ulArray[i].LoopName)){
                            uniqueparentLoopsArray.push(ulArray[i].LoopName);
                            uniqueparentLoopsArrayObject.push(ulArray[i]);
                        }
                    }
                    return uniqueparentLoopsArrayObject;
                }
       
                function ShowDocument(array,ParentLoopID){

                    var parray = ParentLoops(array,ParentLoopID);

                    parray.sort((x,y)=>x.DSLoopSeqIndex - y.DSLoopSeqIndex);

                    var lArray = uniqueParentLoops(parray);

                    var newArray  = [];
                
                    for(var i=0;i<lArray.length;i++){
                        structhtml += "<h2><b>Begin Loop: "+lArray[i].LoopName+"</b></h2>";
                        var segmentsArray = DocumnetDetails.filter(function(x){ return x.DSLoopLoopControl == lArray[i].DSLoopLoopControl })
                        var hSegments=[];
                        var nSegments=[];
                        for(var z=0;z<segmentsArray.length;z++){
                            if(!hSegments.includes(segmentsArray[z].SegmentName)){
                                hSegments.push(segmentsArray[z].SegmentName);
                                nSegments.push(segmentsArray[z]);
                            }
                        }

                        nSegments.sort((x,y)=>x.DSSegSeqIndex - y.DSSegSeqIndex);
                        if(nSegments[0].SegmentName != null){
                            for(var j=0;j<nSegments.length;j++){
                                if(nSegments[j].SegmentName != null){
                                    structhtml +="<h4>"+nSegments[j].SegmentName+" - "+nSegments[j].SegmentDesc+" (Header - "+nSegments[j].DSSegUsage+") Loop id - "+lArray[i].LoopName+"</h4>";
                                    structhtml +="<table>";
                                    structhtml +="<thead><tr><th style='width:150px;'>Element</th><th style='width:300px;'>Description</th><th style='width:150px;'>Usage</th><th style='width:150px;'>Length</th><th style='width:150px;'>Values Used/Notes</th><th style='width:200px;'>Mapping</th></tr></thead>";
                                    ElementsArray = segmentsArray.filter(function(y){ return y.SegmentName == nSegments[j].SegmentName})
                                    structhtml +="<tbody>";
                                    for(var k=0;k<ElementsArray.length;k++){
                                        if(ElementsArray[k].ElementName != null){
                                            structhtml +="<tr><td style='width:150px;' class='td_align'>"+ElementsArray[k].ElementName+"</td><td style='width:300px;' >"+ElementsArray[k].ElementDesc+"</td><td  style='width:150px;' class='td_align'>"+ElementsArray[k].DSElementUsage+"</td><td  style='width:150px;' class='td_align'>"+ElementsArray[k].ElementMinLength+"-"+ElementsArray[k].ElementMaxLength+"</td><td  style='width:150px;' >"+ElementsArray[k].DSAttrNotes+"</td><td style='width:200px;'>"+ElementsArray[k].MappingDetails+"</td></tr>";
                                        }
                                    }
                                    structhtml +="</tbody></table>";
                                }
                            }
                        }else{
                            structhtml += "</br>";
                        }
                        structhtml += "<h2><b>End Loop: "+lArray[i].LoopName+"</b></h2>";
                        for(var a = 0;a <DocumnetDetails.length; a++){
                       
                            if(lArray[i].DSLoopLoopControl == DocumnetDetails[a].DSLoopParentLoopID){
                                newArray.push(DocumnetDetails[a]);
                            }
                        }
                     
                        if(newArray.length > 0){
                            count++;
                            ShowDocument(DocumnetDetails,lArray[i].DSLoopLoopControl);
                        }else{
                            if(count>0){
                                for(var c=0; c<count-1;c++){
                                   // structhtml +="</fieldset>";
                                }
                            }else{
                                //structhtml +="</fieldset>";
                            }
                            count = 0;
                        }
                    }
                }
            }else{
                var structhtml="<div style='text-align:center;'><h2>Document configuration not available</h2></div>"
            }
                $("#show").append(structhtml);
                DocumentPreview.center().open();
        }

        //Added By SRP on 2/14/18 EDITypeAddExample
        function openEDIDocumentEditWindow(e) {
            //Get the record data from the grid
            var item = this.dataItem($(e.currentTarget).closest("tr")); 

            //****Clear validation Error**********//
            var company = $("#company-validation").hasClass("hide-display");
            if (company == false) {
                $("#company-validation").addClass("hide-display");
            }
            var document = $("#document-validation").hasClass("hide-display");
            if (document == false) {
                $("#document-validation").addClass("hide-display");
            }

            //*********Edit Popup Title*********//
            $("#wndAddDocument").data("kendoWindow").title("Edit TP Document Details");

            $("#txtTPDocControl").val(item.TPDocControl);
            $("#txtTPDocCCEDIControl").val(item.TPDocCCEDIControl);
           
            var ddl1 = $("#ddlCompany").data("kendoDropDownList");
            ddl1.value(parseInt(item.CompControl));
            ddl1.enable(false);
            var ddl2 = $("#ddlCarrier").data("kendoDropDownList");
            ddl2.value(parseInt(item.CarrierControl));

            var ddl3 = $("#ddldoctype").data("kendoDropDownList");
            ddl3.value(parseInt(item.EDITControl));
            ddl3.enable(false);

            var ddl4= $("#ddlMasterDocInbound").data("kendoDropDownList");
            ddl4.value(Boolean(item.TPDocInbound));
            ddl4.enable(false);

            var cName = $("#ddlCompany").data("kendoDropDownList").dataItem().CompName;
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
                dataTextField: "CarrierName",
                dataValueField: "CarrierControl",
                dataTypeField: "CCEDIControl", 
                autoWidth: true,
                filter: "contains",
                dataSource: dsCarrier,
            });

            wndAddDocType.center().open();
        }
        //Added By SRP on 2/14/18 EDITypeAddExample

        function SaveEDITPDocument() {
            var otmp = $("#focusCancel").focus();

            var submit = true;

            var tName = $("#ddlCompany").val();
            if (tName == "") {
                $("#company-validation").removeClass("hide-display");
                submit = false;
            }

            var tName = $("#ddldoctype").val();
            if (tName == "") {
                $("#document-validation").removeClass("hide-display");
                submit = false;
            }


            var item = new NGLDocType();

            var TPDocControl = $("#txtTPDocControl").val();

            var dataItem1 = $("#ddlCarrier").data("kendoDropDownList").dataItem();
            var TPDocCCEDIControl  = dataItem1.CCEDIControl;

            var dataItem1 = $("#ddlCompany").data("kendoDropDownList").dataItem();
            var CompControl  = dataItem1.CompControl;

            var dataItem2 = $("#ddlCarrier").data("kendoDropDownList").dataItem();
            var CarrierControl  = dataItem2.CarrierControl;
            
            var dataItem3 = $("#ddldoctype").data("kendoDropDownList").dataItem();
            var TPDocEDITControl  = dataItem3.EDITControl;

            var dataItem4 = $("#ddlMasterDocInbound").data("kendoDropDownList").dataItem();
            var TPDocInbound  = dataItem4.TrControl;
            $.ajax({
                async: false,
                url: "api/EDITPDocument/GetRecordByCarrierID/",
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

            var dataJSON = { TPDocControl:TPDocControl,TPDocCCEDIControl: TPDocCCEDIControl,TPDocEDITControl:TPDocEDITControl,TPDocInbound:TPDocInbound,Action:"EDIT" };
            if (submit == true) {
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
                                    ngl.showErrMsg("Save EDITPDocument Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            blnSuccess = true;
                                            refreshEDITPDocGrid();
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIDocumentType Failure"; }
                                ngl.showErrMsg("Save EDITPDocument Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                        ngl.showErrMsg("Save EDITPDocument Failure", sMsg, null);                        
                    }
                    });
            wndAddDocType.close();         
            }
        }

        $("#ddlCarrier").on("change input", function () {
            var datatypesval = $(this).val();
            var item = new NGLDocType();
            var dataItem1 = $("#ddlCarrier").data("kendoDropDownList").dataItem();
            var TPDocCCEDIControl  = dataItem1.CCEDIControl;

            var dataItem1 = $("#ddlCompany").data("kendoDropDownList").dataItem();
            var CompControl  = dataItem1.CompControl;

            var dataItem2 = $("#ddlCarrier").data("kendoDropDownList").dataItem();
            var CarrierControl  = dataItem2.CarrierControl;
            
            var dataItem3 = $("#ddldoctype").data("kendoDropDownList").dataItem();
            var TPDocEDITControl  = dataItem3.EDITControl;

            var dataItem4 = $("#ddlMasterDocInbound").data("kendoDropDownList").dataItem();
            var TPDocInbound  = dataItem4.TrControl;
            var segment = datatypesval;
            var dataJSON = { TPDocCCEDIControl: TPDocCCEDIControl,TPDocEDITControl:TPDocEDITControl,TPDocInbound:TPDocInbound };
            $.ajax({
                async: false,
                url: "api/EDITPDocument/GetRecordByCarrierID/",
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
                                ngl.showErrMsg("Save EDI MasterDoc Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        ngl.showWarningMsg("A document already exists for selected company and carrier.<br><br>Please check the list of Trading Partner documents.", strValidationMsg);
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                           
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save EDI MasterDoc Failure");
                    ngl.showErrMsg("Save EDI MasterDoc Failure", sMsg, null);
                }
            });         

        });


        //********On change Validation***********//
        $("#ddlCompany").on("change input", function () {
            var datatypesval = $(this).val();
            if (datatypesval != 0) {
                $("#company-validation").addClass("hide-display");
            }
            else {
                $("#company-validation").removeClass("hide-display");
            }
        });

        $("#ddldoctype").on("change input", function () {
            var datatypesval = $(this).val();
            if (datatypesval != 0) {
                $("#document-validation").addClass("hide-display");
            }
            else {
                $("#document-validation").removeClass("hide-display");
            }
        });

      //**********Edit Mode Dropdowns*********//
      
        function DataTypeDropDownEditor(container, options) {
            $('<input required data-text-field="EDITName" data-value-field="EDITControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "EDITName",
                    dataValueField: "EDITControl",
                    autoBind: true,
                    dataSource: dsDocTypeDropDownnew,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("EDITControl", fee.ListTypeControl);
                        options.model.set("EDITName", fee.ListTypeName);
                    }
                });
        }
        function CompDropDownEditor(container, options) {
            $('<input required data-text-field="CompNumber" data-value-field="CompControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "CompNumber",
                    dataValueField: "CompControl",
                    autoBind: true,
                    dataSource: dsComp,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("CompControl", fee.ListTypeControl);
                        options.model.set("CompNumber", fee.ListTypeName);
                    }
                });
        }
        function editTransactionDropDown(container, options) {
            $('<input required data-text-field="TrName" data-value-field="TrControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "TrName",
                    dataValueField: "TrControl",
                    autoBind: true,
                    dataSource: ddlTransaction,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("TrControl", fee.ListTypeControl);
                        options.model.set("TrName", fee.ListTypeName);
                    }
                });
        }
        function refreshEDITPDocGrid() {
            //oKendoGrid gets set during EDITPDocGridDataBoundCallBack()
            if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) {
                oKendoGrid.dataSource.read();
            }
          }

            //*************Datssource For Drop Downs********//
        dsDocTypeDropDownnew = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "api/EDIDocumentType/GetRecords",
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
                    id: "EDITControl",
                    fields: {
                        EDITControl: { type: "number" },
                        EDITName: { type: "string" }, 
                        EDITDescription: { type: "string" }
                    }
                }, 
                errors: "Errors"
            },
            error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
            serverPaging: false,
            serverSorting: false,
            serverFiltering: false
        });
        dsDocTypeDropDownnew.read();

        dsComp = new kendo.data.DataSource({
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
                    id: "CompControl",
                    fields: {
                        CompControl: { type: "number" },
                        CompName: { type: "string" },
                        CarrierControl: { type: "number" },
                        CarrierName: { type: "string" },
                        CCEDIControl: { type: "number" },
                    }
                }, 
                errors: "Errors"
            },
            error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
            serverPaging: false,
            serverSorting: false,
            serverFiltering: false
        });
        dsComp.read();

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
                    id: "CarrierControl",
                    fields: {
                        CompControl: { type: "number" },
                        CompName: { type: "string" },
                        CarrierControl: { type: "number" },
                        CarrierName: { type: "string" },
                        CCEDIControl: { type: "number" },
                    }
                }, 
                errors: "Errors"
            },
            error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
            serverPaging: false,
            serverSorting: false,
            serverFiltering: false
        });
        dsCarrier.read();

        dsDocTypeDropDownnew = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "api/EDIDocumentType/GetRecords",
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
                    id: "EDITControl",
                    fields: {
                        EDITControl: { type: "number" },
                        EDITName: { type: "string" }, 
                        EDITDescription: { type: "string" }
                    }
                }, 
                errors: "Errors"
            },
            error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
            serverPaging: false,
            serverSorting: false,
            serverFiltering: false
        });
        dsDocTypeDropDownnew.read();

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
                        //Modified By SRP on 2/12/18 EDITypeAddExample
                        content: "<button id='btnAddDocumentType' class='k-button actionBarButton' type='button' onclick='execActionClick(btnAddDocumentType, 160);'><span class='k-icon k-i-add'></span>Create TP EDI Document</button>"
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

            //set default message
            var l = "<h3>Manage EDI TP Documents </h3>";
            $("#txtScreenMessage").html(l);

            
            $("#ddlCompany").kendoDropDownList({
                dataTextField: "CompName",
                dataValueField: "CompControl",
                optionLabel:"Select Company",
                dataTypeField: "CCEDIControl", 
                autoWidth: true,
                filter: "contains",
                dataSource: dsComp,
                dataBound: onDataBound

            });
            
            $("#ddlCarrier").kendoDropDownList({
                dataTextField: "CarrierName",
                dataValueField: "CarrierControl",
                dataTypeField: "CCEDIControl", 
                autoWidth: true,
                filter: "contains",
                dataSource: dsCarrier,
                dataBound: onDataBound
            });
            $("#ddldoctype").kendoDropDownList({
                dataTextField: "EDITName",
                dataValueField: "EDITControl",
                optionLabel:"Select Document Type",

                autoWidth: true,
                filter: "contains",
                dataSource: dsDocTypeDropDownnew
                
            });
            function onDataBound(e) {
                $("#ddlCompany").closest(".k-combobox").attr("companyattr", "CCEDIControl");
            };
            function onDataBound(e) {
                $("#ddlCarrier").closest(".k-combobox").attr("carrierattr", "CCEDIControl");
            };
            $("#ddlMasterDocInbound").kendoDropDownList({
                dataSource: ddlTransaction,
                dataTextField: "TrName",
                dataValueField: "TrControl",
                autoWidth: true,
                filter: "contains",
            });
            ////////////Filters///////////////////
            var EDITypeFilterData = [ 
               { text: "", value: "None" },
               { text: "EDITName", value: "EDITName" },
               { text: "CarrierName", value: "CarrierName" },
               { text: "CompName", value: "CompName" },
            ];
            
            $("#ddlEDITypeFilters").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: EDITypeFilterData,
                select: function(e) {
                    var name = e.dataItem.text; 
                    var val = e.dataItem.value; 
                    $("#txtEDITypeFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpEDITypeFilterFrom").data("kendoDatePicker").value("");
                    $("#dpEDITypeFilterTo").data("kendoDatePicker").value("");
                    switch(val){
                        case "None":
                            $("#spEDITypeFilterText").hide();
                            $("#spEDITypeFilterDates").hide();
                            $("#spEDITypeFilterButtons").hide(); 
                            break; 
                        case "NoDatesAvailable":
                            $("#spEDITypeFilterText").hide();
                            $("#spEDITypeFilterDates").show();
                            $("#spEDITypeFilterButtons").show();
                            break;
                        default:
                            $("#spEDITypeFilterText").show();
                            $("#spEDITypeFilterDates").hide();
                            $("#spEDITypeFilterButtons").show();
                            break;
                    }
                }
            });
            
            $("#txtEDITypeFilterVal").kendoMaskedTextBox(); 
            $("#dpEDITypeFilterFrom").kendoDatePicker(); 
            $("#dpEDITypeFilterTo").kendoDatePicker(); 
            $("#btnEDITypeFilter").kendoButton({
                icon: "filter",
                click: function(e) { 
                    var dataItem = $("#ddlEDITypeFilters").data("kendoDropDownList").dataItem(); 
                    
                    if (1 === 0){ 
                        var dtFrom = $("#dpEDITypeFilterFrom").data("kendoDatePicker").value(); 
                        if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}
                    } 
                    $("#EDITPDocGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#btnEDITypeClearFilter").kendoButton({
                icon: "filter-clear",
                click: function(e) {
                    var dropdownlist = $("#ddlEDITypeFilters").data("kendoDropDownList"); 
                    dropdownlist.select(0);
                    dropdownlist.trigger("change");
                    $("#txtEDITypeFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpEDITypeFilterFrom").data("kendoDatePicker").value(""); 
                    $("#dpEDITypeFilterTo").data("kendoDatePicker").value(""); 
                    $("#spEDITypeFilterText").hide(); 
                    $("#spEDITypeFilterDates").hide(); 
                    $("#spEDITypeFilterButtons").hide();
                    $("#EDITPDocGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtEDITypeFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpEDITypeFilterFrom").data("kendoDatePicker").value("");
            $("#dpEDITypeFilterTo").data("kendoDatePicker").value("");
            $("#spEDITypeFilterText").hide();
            $("#spEDITypeFilterDates").hide();
            $("#spEDITypeFilterButtons").hide();

            
            ////////////Grid///////////////////
            EDIType = new kendo.data.DataSource({
                serverSorting: true, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();

                        s.filterName = $("#ddlEDITypeFilters").data("kendoDropDownList").value();
                        s.filterValue = $("#txtEDITypeFilterVal").data("kendoMaskedTextBox").value();

                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;

                        $.ajax({ 
                            url: '/api/EDITPDocument/GetCompRecords/', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s) }, 
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) {
                                
                                options.success(data);
                                // Updated by SRP on 3/22/2018
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Get EDIDocument Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                }
                                                else{
                                                    blnSuccess = true;
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get EDIDocument Failure"; }
                                        ngl.showErrMsg("Get EDIDocument Failure", strValidationMsg, null);
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
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: "api/EDITPDocument/SaveEDIDocument",
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
                                                    refreshEDITPDocGrid();
                                                }
                                            }
                                       
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIDocument Failure"; }
                                        ngl.showErrMsg("Save EDIDocument Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDIDocument Failure", sMsg, null);                        
                            }
                        });
                    },
                    update: function(options) {
                        $.ajax({ 
                            url: 'api/EDITPDocument/PostSave', 
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
                                            ngl.showErrMsg("Save EDIDocument Failure", data.Errors, null);
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
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIDocument Failure"; }
                                        ngl.showErrMsg("Save EDIDocument Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                //refresh the grid
                                if (ngl.isFunction(refreshEDITPDocGrid)) {
                                    refreshEDITPDocGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDIType Failure", sMsg, null); 
                            } 
                        });
                    },
                    destroy: function(options) {
                        $.ajax({
                            url: 'api/EDITPDocument/DeleteTPDocumentRecord', 
                            type: 'DELETE',
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: JSON.stringify(options.data),
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function (data) {     
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (data == 0) {
                                        
                                    }
                                    if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                        }
                                    
                                    }
                                
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                //refresh the grid
                                if (ngl.isFunction(refreshEDITPDocGrid)) {
                                    refreshEDITPDocGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete EDIDocument Failure");
                                ngl.showErrMsg("delete EDIDocument Failure", sMsg, null); 
                            } 
                        });
                    },
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "TPDocControl",
                        fields: {
                            TPDocControl: { type: "number" },
                            EDITControl: { type: "number" },
                            EDITName: { type: "string" },
                            CarrierName: { type: "string" },
                            CompName: { type: "string" },//Updated By SRP on 3/22/2018
                            CompControl: { type: "number" },
                            CarrierControl: { type: "number" },
                            TPDocCCEDIControl: { type: "number" },
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDIDocument Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            dsDocTypeDropDownnew.read();
            var grid = $("#EDITPDocGrid").kendoGrid({
                dataSource: EDIType,
                pageable: true,
                sortable: {
                    mode: "single",
                    allowUnsort: true
                },
                height: 300,
                sort: function(e) {
                    if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                    if (!e.sort.field) { e.sort.field == ""; }
                    $("#txtEDITypeSortDirection").val(e.sort.dir);
                    $("#txtEDITypeSortField").val(e.sort.field);
                },
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (EDITPDocGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(EDITPDocGridDataBoundCallBack)) { EDITPDocGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable:"inline",
                columns: [
                    { field: "TPDocControl", title: "TPDocControl ", hidden: true },
                    { field: "EDITControl", title: "Document Id", hidden: true },
                    { field: "EDITName", title: "Document Name",editor: DataTypeDropDownEditor, hidden: false },// template:data=>dsDocTypeDropDownnew._data[Object.values(dsDocTypeDropDownnew._data).findIndex(x=> x.EDITControl == data.TPDocEDITControl)].EDITName},
                    { field: "CompName", title: "Company Name", hidden: false },
                    { field: "CarrierName", title: "Carrier Name", hidden: false },
                    { field: "CompControl", title: "Company Id", hidden: true },
                    { field: "CarrierControl", title: "Carrier Id", hidden: true },
                    { field: "TPDocInbound", title: "Transaction",hidden: false,editor: editTransactionDropDown, template:data=>ddlTransaction[Object.values(ddlTransaction).findIndex(x=> x.TrControl == data.TPDocInbound)].TrName},
                    { field: "TPDocCCEDIControl", title: "TPDoc Id", hidden: true},// template:data=>dsComp._data[Object.values(dsComp._data).findIndex(x=> x.CompControl == data.TPDocCCEDIControl)].CompName}, 
                    { command: [{ name: "editTPDocument", text: "", iconClass: "k-icon k-i-pencil", click: openEDIDocumentEditWindow},{ name: "ViewConfig", text: "", iconClass: "k-icon k-i-file-wrench", click: showDetails},{name: "ExportDocument", text: "", iconClass: "k-icon k-i-file-pdf",  click: showPreview },{name: "destroy", text: "", imageClass: "k-i-delete", iconClass: "k-icon"  }], title: "Action", width: "150px" }
                ]
            }).data("kendoGrid");
            
            $("#EDITPDocGrid").kendoTooltip({
                filter: ".k-grid-editTPDocument",
                content: "Edit&nbsp;TP&nbsp;Document"
            });

            $("#EDITPDocGrid").kendoTooltip({
                filter: ".k-grid-ViewConfig",
                content: "Edit&nbsp;Document&nbsp;Structure"
            });

            $("#EDITPDocGrid").kendoTooltip({
                filter: ".k-grid-ExportDocument",
                content: "View&nbsp;and&nbsp;Export&nbsp;To&nbsp;PDF"
            });
            
            $("#EDITPDocGrid").kendoTooltip({
                filter: ".k-grid-delete",
                content: "Delete&nbsp;TP&nbsp;Document"
            });

            ////////////wndMessage///////////////////
            wndMessage = $("#wndMessage").kendoWindow({
                title: "Message Window",
                height: 100,
                width: 270,
                modal: true,
                visible: false,
                actions: ["save", "Minimize", "Maximize", "Close"],
            }).data("kendoWindow");

            $("#wndMessage").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveMessage(); }); 

            ////////////wndAddEDIDocumentType/////////////////

            kendoWin.height = 400;// For Updating Kendowindow height config
            kendoWin.width = 350;// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddDocType = $("#wndAddDocument").kendoWindow(kendoWin).data("kendoWindow");

            $("#wndAddDocument").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDITPDocument(); });
            
            ////////////wnd Document Preview/////////////////
            kendoWinPreview.height = '75%';// For Updating Kendowindow height config
            kendoWinPreview.width = '60%';// For Updating Kendowindow width config

            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            DocumentPreview = $("#DocumentPreview").kendoWindow(kendoWinPreview).data("kendoWindow");
          
            $("#DocumentPreview").data("kendoWindow").wrapper.find(".k-svg-i-file-pdf").click(function (e) { exportPDF('#show'); });

            //*****ToolTip Exporte Button*********//
            $(".k-i-file-pdf").kendoTooltip({
                position: "left",
                content: "Export&nbsp;PDF"
            });

        });

        </script>
        <style>
            .k-grid tbody .k-button {
                min-width: 18px;
                width: 28px;
            }

            .k-grid tbody tr td {
                vertical-align: top;
            }
        </style>

    </div>
</body>

</html>
