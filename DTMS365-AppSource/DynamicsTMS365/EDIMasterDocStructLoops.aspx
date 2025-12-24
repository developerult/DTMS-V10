<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDIMasterDocStructLoops.aspx.cs" Inherits="DynamicsTMS365.EDIMasterDocStructLoops" %>

<!DOCTYPE html>

<html>
<head>
    <title>Master DocStruct Loops</title>         
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
        .ui-container {
            height: 100% !important; width: 100% !important; margin-top: 2px !important;
        }
        .ui-vertical-container {
            height: 98% !important; width: 98% !important; 
        }
        .ui-horizontal-container {
            height: 100% !important; width: 100% !important;
        }
        .k-tooltip-validation {
            margin-top: 3px !important;
            display: block;
            position: static;
            padding: 0;
        } 
        .k-callout {
            display: none;
        }
         #editNotes:hover{
            color:red;
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
    <!-- added by SRP on 2/22/2018 For Dynamically adding menu items for all pages -->
    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 2/22/2018 For Editing KendoWindow Configuration from Javascript -->

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
                            <div><span>Menu</span></div>
                            <%--Page Navigation Menu Tree--%>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <!-- Begin Page Content -->

                        <%--Message--%>
                        <div id="txtScreenMessage"></div>
                         <div id="LoopsTreeGrid"></div>
                        <div id="window">
                              <textarea id="editor" rows="10" cols="30" style="height:440px" aria-label="editor" data-bind="value: MDSAttrNotes"></textarea>
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

        <%--Added By SRP on 2/22/2018 EDIElement for KendoWindow--%>
        <% Response.WriteFile("~/Views/MDocStructLoopsAddWindow.html"); %>
        <% Response.WriteFile("~/Views/PreviewDocStructure.html"); %>

        <% Response.WriteFile("~/Views/EDIMasterDocStructLoopAddWindow.html"); %>
        <% Response.Write(AuthLoginNotificationHTML); %>
        <script>

        //************* Page Variables **************************
            var PageControl = '<%=PageControl%>'; 
            var tObj = this;
            var tPage = this;
        var oKendoTreeList=null;
        var oKendoGridLoopSegments=null;
        var oKendoGridelement=null;
        var oKendoGridelementAttribute=null;

        var dsDataMapFieldTable = kendo.data.DataSource;
        var dsDataMapFieldColumn = kendo.data.DataSource;
        var oKendoGridsegelement = null;
        var LoopsData = kendo.data.DataSource;
        var ParentLoopDropDown = kendo.data.DataSource;
        var dsSegmentData = kendo.data.DataSource;
        var dsMDocSegmentsByLoop = kendo.data.DataSource;
        var wndMessage = kendo.ui.Window;
        var wndAddMDocStructLoops = kendo.ui.Window; 
        var ddlUsage = [{ UsControl:'M', UsName:"Mandatory"},{ UsControl:'O', UsName:"Optional"},{ UsControl:'C', UsName:"Conditional"}];
        var ddlTransaction = [{ TrControl:true, TrName:"InBound"},{ TrControl:false, TrName:"OutBound"}];
        var ddlPublish = [{ PbControl:false, PbName:"No"},{ PbControl:true, PbName:"Yes"}];

        var wndAddDocType = kendo.ui.Window; 
        var dsSegmentDropDown = kendo.data.DataSource; 
        var dsDataMapFieldTablenew=kendo.data.DataSource;
        var dsSegmentElementDropDown= kendo.data.DataSource; 
        var SegElementGridDataSource= kendo.data.DataSource;
        var dsDocTypeDropDownnew= kendo.data.DataSource;
        var dsElement= kendo.data.DataSource;
        var dsFormatingDropDown = kendo.data.DataSource;
        var DocumentPreview = kendo.ui.Window;

        //******Kendo Buttons**********//
        $("#preview").kendoButton();
        $("#btnAddElememts").kendoButton();

        //*********Add Input Widges**************//
        $("#txtMDSLoopMinCount").kendoMaskedTextBox();
        $("#txtMDSLoopMaxCount").kendoMaskedTextBox();
        $("#txtMDSLoopSeqIndex").kendoMaskedTextBox();
        // $("#ddlMDSLoopLoopControl").kendoDropDownList();

        //************Document Control based on the URL*******//
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

        //***** Global Varivables*******//
        var doccontrol = getParameterByName('DocControl');
        var loopControl;
        var SegDetailes;
        var currentSegLength = 0;

        var EDITName = getParameterByName('MDocName');
        var MDocInOutbound;
        if(getParameterByName('MDocInOutbound')== "true")
        {
            MDocInOutbound="InBound";
        }
        else
        {
            MDocInOutbound="OutBound";
        }

        //*************  Call Back Functions  *******************
        function LoopsTreeGridDataBoundCallBack(e,LoopsObj){           
            oKendoTreeList = LoopsObj;
        }
               
        function LoopSegmentsGridDataBoundCallBack(e,sObj){           
            oKendoGridLoopSegments = sObj;
        }

        function SegElementsGridDataBoundCallBack(e,tGrid){           
            oKendoGridelement = tGrid;
        }
        function SegElementAttributesGridDataBoundCallBack(e,aObj){           
            oKendoGridelementAttribute = aObj;
        }

      
        //************Refresh Grid Functions********//
        function refreshLoopsTreeList() {
            if (typeof (oKendoTreeList) !== 'undefined' && ngl.isObject(oKendoTreeList)) {
                oKendoTreeList.dataSource.read();
            }
        }
        
        function refreshLoopsSegmentsGrid() {
            if (typeof (oKendoGridLoopSegments) !== 'undefined' && ngl.isObject(oKendoGridLoopSegments)) {
                oKendoGridLoopSegments.dataSource.read();
            }
        }

        function refreshSegElementsGrid() {
            if (typeof (oKendoGridelement) !== 'undefined' && ngl.isObject(oKendoGridelement)) {
                oKendoGridelement.dataSource.read();
            }
        }

        function refreshLoopElementAttributesGrid() {
            if (typeof (oKendoGridelementAttribute) !== 'undefined' && ngl.isObject(oKendoGridelementAttribute)) {
                oKendoGridelementAttribute.dataSource.read();
            }
        }
        
        //*******Document Preview Function********//
        function viewPreview(){
            $("#show").empty();
            $.ajax({ 
                url: 'api/EDIMasterDocument/GetMasterDocPreview', 
                contentType: 'application/json; charset=utf-8', 
                dataType: 'json', 
                async: false,
                data: { MasterDocControl: doccontrol },  
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
                //*********Header of The View*************//
                var DocHead = '<% = System.Configuration.ConfigurationManager.AppSettings["NGLApplicationName"]%>' ;
                var header="";
                header += "<h3 class='Docheader'>"+DocHead+"</h3>";
                header += "<h4 class='DOCDetails'>"+DocumnetDetails[0].EditDescription+"</h4>"
                header += "<h4 class='DOCDetails'>"+DocumnetDetails[0].MasterDocType+" "+DocumnetDetails[0].EDITName+"</h4>"
                header += "<h4 class='DOCDetails'>Version "+DocumnetDetails[0].Version+"</h4>"
                header += "<h5 class='cuttentDate'>"+ m_names[curr_month]+" "+ curr_date +", " + curr_year+"</h5>"
                header += "<div class='hederHR'></div>"
                $("#show").append(header);

                var MDSLoopParentLoopID = 0;
                var structhtml = "";
                var count = 0;

                ShowDocument(DocumnetDetails,MDSLoopParentLoopID);

                function ParentLoops(lArray,MDSLoopParentLoopID){
                    var parentLoopsArray=[];
                    for(var i=0;i<lArray.length;i++){
                        if(lArray[i].MDSLoopParentLoopID == MDSLoopParentLoopID){
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

                    parray.sort((x,y)=>x.MDSLoopSeqIndex - y.MDSLoopSeqIndex);

                    var lArray = uniqueParentLoops(parray);

                    var newArray  = [];
                
                    for(var i=0;i<lArray.length;i++){
                        structhtml += "<h2><b>Begin Loop: "+lArray[i].LoopName+"</b></h2>";
                        var segmentsArray = DocumnetDetails.filter(function(x){ return x.MDSLoopLoopControl == lArray[i].MDSLoopLoopControl })
                        var hSegments=[];
                        var nSegments=[];
                        for(var z=0;z<segmentsArray.length;z++){
                            if(!hSegments.includes(segmentsArray[z].SegmentName)){
                                hSegments.push(segmentsArray[z].SegmentName);
                                nSegments.push(segmentsArray[z]);
                            }
                        }

                        nSegments.sort((x,y)=>x.MDSSegSeqIndex - y.MDSSegSeqIndex);
                        if(nSegments[0].SegmentName != null){
                            for(var j=0;j<nSegments.length;j++){
                                if(nSegments[j].SegmentName != null){
                                    structhtml +="<h4>"+nSegments[j].SegmentName+" - "+nSegments[j].SegmentDesc+" (Header - "+nSegments[j].MDSSegUsage+") Loop id - "+lArray[i].LoopName+"</h4>";
                                    structhtml +="<table>";
                                    structhtml +="<thead><tr><th style='width:150px;'>Element</th><th style='width:300px;'>Description</th><th style='width:150px;'>Usage</th><th style='width:150px;'>Length</th><th style='width:150px;'>Values Used/Notes</th><th style='width:200px;'>Mapping</th></tr></thead>";
                                    ElementsArray = segmentsArray.filter(function(y){ return y.SegmentName == nSegments[j].SegmentName})
                                    structhtml +="<tbody>";
                                    for(var k=0;k<ElementsArray.length;k++){
                                        if(ElementsArray[k].ElementName != null){
                                            structhtml +="<tr><td class='td_align'>"+ElementsArray[k].ElementName+"</td><td >"+ElementsArray[k].ElementDesc+"</td><td class='td_align'>"+ElementsArray[k].MDSElementUsage+"</td><td class='td_align'>"+ElementsArray[k].ElementMinLength+"-"+ElementsArray[k].ElementMaxLength+"</td><td>"+ElementsArray[k].MDSAttrNotes+"</td><td>"+ElementsArray[k].MappingDetails+"</td></tr>";
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
                       
                            if(lArray[i].MDSLoopLoopControl == DocumnetDetails[a].MDSLoopParentLoopID){
                                newArray.push(DocumnetDetails[a]);
                            }
                        }
                     
                        if(newArray.length > 0){
                            count++;
                            ShowDocument(DocumnetDetails,lArray[i].MDSLoopLoopControl);
                        }else{
                            if(count>0){
                                for(var c=0; c<count-1;c++){
                                    structhtml +="</fieldset>";
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

        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
            if (btn.id == "btnAddElement") {
                openMasterDocAddWindow();
            }
        }
      
        //Added By By S on 2/22/2018 EDIElementAddSample
        function openMasterDocAddWindow() {
            //Validation Display
            var Doccheck = $("#name-validation").hasClass("hide-display");
            if (Doccheck == false) {
                $("#name-validation").addClass("hide-display");
            }
            var MinMax = $("#minmax-validation").hasClass("hide-display");
            if (MinMax == false) {
                $("#minmax-validation").addClass("hide-display");
            }
            // title of the window (in case you want to share the same window for Add and Edit- Note: In grids editing does not have to always be inline)
            $("#wndAddMDocStructLoops").data("kendoWindow").title("Add loop to Document Structure");

            //Clear all previous values since this is Add New
            $("#txtMDSLoopControl").val(0);
            var ddlDocument = $("#ddlMDSLoopLoopControl").data("kendoDropDownList");
            ddlDocument.readonly(false);
            ddlDocument.select(0);

            var ddlDocument = $("#ddlMDSLoopParentLoopID").data("kendoDropDownList");
            ddlDocument.readonly(false);
            ddlDocument.select(0);

            var ddlDocument = $("#ddlMDSLoopUsage").data("kendoDropDownList");
            ddlDocument.readonly(false);
            ddlDocument.value('M');

            $("#txtMDSLoopMinCount").data("kendoMaskedTextBox").value(1);
            $("#txtMDSLoopMaxCount").data("kendoMaskedTextBox").value(1);
            $("#txtMDSLoopSeqIndex").data("kendoMaskedTextBox").value(0);

            $("#chkMDSLoopDisabled").prop('checked', false);

            wndAddMDocStructLoops.center().open();
        }
        
        function openMasterDocEditWindow(e) {
            var eitem = this.dataItem($(e.currentTarget).closest("tr")); 
            //Validation Display
            var Doccheck = $("#name-validation").hasClass("hide-display");
            if (Doccheck == false) {
                $("#name-validation").addClass("hide-display");
            }
            var MinMax = $("#minmax-validation").hasClass("hide-display");
            if (MinMax == false) {
                $("#minmax-validation").addClass("hide-display");
            }
            // title of the window (in case you want to share the same window for Add and Edit- Note: In grids editing does not have to always be inline)
            $("#wndAddMDocStructLoops").data("kendoWindow").title("Edit Document Structure loop");

            //Clear all previous values  and Update with edit objects values
            $("#txtMDSLoopControl").val(eitem.MDSLoopControl);

            var ddl= $("#ddlMDSLoopLoopControl").data("kendoDropDownList");
            ddl.value(parseInt(eitem.MDSLoopLoopControl));
            ddl.enable(false);

            $("#ddlMDSLoopParentLoopID").data("kendoDropDownList").value(parseInt(eitem.MDSLoopParentLoopID));
           
            var usage = eitem.MDSLoopUsage
            $("#ddlMDSLoopUsage").data("kendoDropDownList").value(usage);

            $("#txtMDSLoopMinCount").data("kendoMaskedTextBox").value(eitem.MDSLoopMinCount);

            $("#txtMDSLoopMaxCount").data("kendoMaskedTextBox").value(eitem.MDSLoopMaxCount);

            $("#txtMDSLoopSeqIndex").data("kendoMaskedTextBox").value(eitem.MDSLoopSeqIndex);

            $("#txtMDSLoopModDate").val(eitem.MDSLoopModDate);
            $("#txtMDSLoopModUser").val(eitem.MDSLoopModUser);
            $("#txtMDSLoopUpdated").val(eitem.MDSLoopUpdated);
            $("#txtMDSLoopCreateDate").val(eitem.MDSLoopCreateDate);
            $("#txtMDSLoopCreateUser").val(eitem.MDSLoopCreateUser);

            $("#chkMDSLoopDisabled").prop('checked', eitem.MDSLoopDisabled);

            wndAddMDocStructLoops.center().open();
        }
        
        function SaveEDIMasterDoc() {
            var otmp = $("#focusCancel").focus();

            var submit = true;
            var Document = $("#ddlMDSLoopLoopControl").val();

            if (Document =="") {
                $("#name-validation").removeClass("hide-display");
                submit = false;
            }

            var LMinC =  $("#txtMDSLoopMinCount").val();
            var LMaxC =  $("#txtMDSLoopMaxCount").val();
            if(LMinC > LMaxC){
                $("#minmax-validation").removeClass("hide-display");
                submit = false;
            }
           
            var item = new NGLElement();
            item.MDSLoopControl = $("#txtMDSLoopControl").val();
            if (item.MDSLoopControl == 0){
                item.MDSLoopControl = $("#txtMDSLoopControl").val();

                item.MDSLoopMasterDocControl = doccontrol;

                var dataItem1 = $("#ddlMDSLoopLoopControl").data("kendoDropDownList").dataItem();
                item.MDSLoopLoopControl  = dataItem1.LoopControl;

                var dataItem2 = $("#ddlMDSLoopParentLoopID").data("kendoDropDownList").dataItem();
                item.MDSLoopParentLoopID  = dataItem2.LoopControl;

                var dataItem3 = $("#ddlMDSLoopUsage").data("kendoDropDownList").dataItem();
                item.MDSLoopUsage  = dataItem3.UsControl;

                item.MDSLoopMinCount = $("#txtMDSLoopMinCount").data("kendoMaskedTextBox").value();

                item.MDSLoopMaxCount = $("#txtMDSLoopMaxCount").data("kendoMaskedTextBox").value();

                item.MDSLoopSeqIndex = $("#txtMDSLoopSeqIndex").data("kendoMaskedTextBox").value();

                item.MDSLoopDisabled= $("#chkMDSLoopDisabled").is(":checked");

              
                if (submit == true) {

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIMDocStructLoop/SaveMDocStructLoop/",
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
                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                        blnErrorShown = true;
                                        ngl.showErrMsg("Save MDSLoops Failure", data.Errors, null);
                                    }
                                    else {
                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                blnSuccess = true;
                                                refreshLoopsTreeList();
                                            }
                                        }
                                    }
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save MDSLoops Failure"; }
                                    ngl.showErrMsg("Save MDSLoops Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                 ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                           ngl.showErrMsg("Save MDSLoops Failure", sMsg, null);
                        }
                    });
                    wndAddMDocStructLoops.close();
                }
            }
            else{
                item.MDSLoopControl = $("#txtMDSLoopControl").val();

                item.MDSLoopMasterDocControl = doccontrol;

                var dataItem1 = $("#ddlMDSLoopLoopControl").data("kendoDropDownList").dataItem();
                item.MDSLoopLoopControl  = dataItem1.LoopControl;

                var dataItem2 = $("#ddlMDSLoopParentLoopID").data("kendoDropDownList").dataItem();
                item.MDSLoopParentLoopID  = dataItem2.LoopControl;

                var dataItem3 = $("#ddlMDSLoopUsage").data("kendoDropDownList").dataItem();
                item.MDSLoopUsage  = dataItem3.UsControl;

                item.MDSLoopMinCount = $("#txtMDSLoopMinCount").data("kendoMaskedTextBox").value();

                item.MDSLoopMaxCount = $("#txtMDSLoopMaxCount").data("kendoMaskedTextBox").value();

                item.MDSLoopSeqIndex = $("#txtMDSLoopSeqIndex").data("kendoMaskedTextBox").value();

                item.MDSLoopModUser = $("#txtMDSLoopModUser").val();
                item.MDSLoopUpdated = $("#txtMDSLoopUpdated").val();
                item.MDSLoopModDate = $("#txtMDSLoopModDate").val();
                item.MDSLoopCreateDate = $("#txtMDSLoopCreateDate").val();
                item.MDSLoopCreateUser = $("#txtMDSLoopCreateUser").val();

                item.MDSLoopDisabled= $("#chkMDSLoopDisabled").is(":checked");

                if (submit == true) {

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIMDocStructLoop/UpdateMDocStructLoop",
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: JSON.stringify(item),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                var strValidationMsg = "";
                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                    blnSuccess = true;
                                    refreshLoopsTreeList();
                                }
                                else{
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Update MDSLoops Failure"; }
                                       ngl.showErrMsg("Save MDSLoops Element Failure", strValidationMsg, null);
                                    }
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update MDSLoops Failure");
                            ngl.showErrMsg("Update MDSLoops Failure", sMsg, null);
                        }
                    });
                    wndAddMDocStructLoops.close();
                }
            }
        }

        //********OnChange Validation In Loop AddWindow********//
        $("#ddlMDSLoopLoopControl").on("change input", function () {
            var datatypesval = $(this).val();
            if (datatypesval != 0) {
                $("#name-validation").addClass("hide-display");
            }
            else {
                $("#name-validation").removeClass("hide-display");
            }
        });
        //********Save Struct Segment Details ********//
        $("#btnAddElememts").click(function(){
            var submit = true;
          
            var Documentnew = $("#ddlSegmentType").val();
            if (Documentnew =="") {
                $("#segment-validation").removeClass("hide-display");
                submit = false;
            }

            var segmentlength = $("#txtSegmentLength").data("kendoMaskedTextBox").value();  
            if(currentSegLength >= segmentlength){
                $("#SegmentLength-validation").removeClass("hide-display");
                submit = false;
            }
            var MDSSegMDSLoopControl=$("#txtMDSSegMDSLoopControl").val();
            
            var dataItem1 = $("#ddlSegmentType").data("kendoDropDownList").dataItem();
            var SegmentControl  = dataItem1.SegmentControl;
                  
            var dataJSON = { SegmentControl: SegmentControl,segmentlength:segmentlength,MDSSegMDSLoopControl:MDSSegMDSLoopControl };
            if (submit == true) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "api/EDIMasterDocStructSegment/SaveMDocStructLoopSegmentElement",
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
                                    ngl.showErrMsg("Add MDOCSegmentlememts Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            blnSuccess = true;
                                            refreshLoopsSegmentsGrid();
                                            segmentDetailsFill(SegmentControl);
                                            refreshSegElementsGrid();
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Add MDOCSegmentlememts Failure"; }
                                ngl.showErrMsg("Add MDOCSegmentlememts Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Add MDOCSegmentlememts Failure");
                        ngl.showErrMsg("Add MDOCSegmentlememts Failure", sMsg, null);
                    }
                });
            }
           
        });
            //********Validation for Segment Length********//
        $("#txtSegmentLength").on("change input", function () {
            var length = $(this).val();
            if (length != "") {
                $("#SegmentLength-validation").addClass("hide-display");
            }
            else {
                $("#SegmentLength-validation").removeClass("hide-display");
            }
        });
        
        function openMasterDocStructLoopEditWindow(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            //******Validation Error Hide
            var SegmCheck = $("#segment-validation").hasClass("hide-display");
            if (SegmCheck == false) {
                $("#segment-validation").addClass("hide-display");
            }
            var Seglength = $("#SegmentLength-validation").hasClass("hide-display");
            if (Seglength == false) {
                $("#SegmentLength-validation").addClass("hide-display");
            }
            var MinMax = $("#eminmax-validation").hasClass("hide-display");
            if (MinMax == false) {
                $("#eminmax-validation").addClass("hide-display");
            }
            //*****Title of Window
            $("#wndAddMasterDocStructLoop").data("kendoWindow").title("Edit Master Doc Struct Loop Details");

            $("#SegElementsGrid").data('kendoGrid').dataSource.data([]);
           
            var ddlSegment = $("#ddlSegmentType").data("kendoDropDownList");
            ddlSegment.select(function(dataItem) { return dataItem.SegmentControl === item.MDSSegSegmentControl; });

            $("#txtMDSSegControl").val(item.MDSSegControl);

            loopControl = item.MDSLoopControl;

            var ddl= $("#lblMDSLoopLoopControl").data("kendoDropDownList");
            ddl.value(parseInt(item.MDSLoopLoopControl));
            ddl.wrapper.hide();
            var lName = $("#lblMDSLoopLoopControl").data("kendoDropDownList").text();
            $("#LoopName").text(lName);

            $("#txtMDSSegMDSLoopControl").val(item.MDSLoopControl);

            $("#txtSegmentLength").data("kendoMaskedTextBox").value(0);

            var ddlSegUsage = $("#ddlSegUsage").data("kendoDropDownList");
            ddlSegUsage.readonly(false);
            ddlSegUsage.value('M');

            $("#txtSegmentMinCount").data("kendoMaskedTextBox").value(1);
            $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value(1);
            $("#txtSegmentDesc").data("kendoMaskedTextBox").value('');
            $("#txtSegmentSeqIndex").data("kendoMaskedTextBox").value(0);

            //**********SegmentGrid ReCreate with fresh******//
            if($('#listBox').data("kendoGrid")){
                $('#listBox').data("kendoGrid").destroy();
                $("#listBox").empty();
            }

            dsMDocSegmentsByLoop = new kendo.data.DataSource({
                transport: {
                    read: function(options) { 
                        $.ajax({ 
                            url: 'api/EDIMasterDocStructSegment/GetRecordSegmentByMaster', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: {loopControl:loopControl},
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
                                            ngl.showErrMsg("Get Segments Failure", data.Errors, null);
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
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get Segments Failure"; }
                                      ngl.showErrMsg("Get Segments Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                   ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Segments Failure");
                                ngl.showErrMsg("Get Segments Failure", sMsg, null); 
                                
                            } 
                        }); 
                    },      
                    destroy: function(options) {
                        var SegmentControl = options.data.SegmentControl
                        var Segcontrol = { MDSSegControl : options.data.MDSSegControl }
                        $.ajax({
                            url: 'api/EDIMasterDocStructSegment/DeleteRecord', 
                            type: 'POST',
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: JSON.stringify(Segcontrol),
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function (data) {     
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (data == false) {
                                        ngl.showWarningMsg("Segment cannot be deleted!", "", null);
                                    }
                                    if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showWarningMsg("Segment cannot be deleted!", data.Errors, null);
                                        }
                                    }
                                
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                if (ngl.isFunction(refreshLoopsSegmentsGrid)) {
                                    refreshLoopsSegmentsGrid();
                                    refreshSegElementsGrid();
                                    //****refresh Segments Details****//
                                    segmentDetailsFill(SegmentControl);
                                    //****Set defalt Drop Down and Length***//
                                    var ddlSegment = $("#ddlSegmentType").data("kendoDropDownList");
                                    ddlSegment.readonly(false);
                                    ddlSegment.select(0);
                                    $("#txtSegmentLength").data("kendoMaskedTextBox").value(0);
                                   
                                   
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete Segment Failure");
                                ngl.showErrMsg("delete Segment Failure", sMsg, null); 
                            } 
                        });
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: { 
                        id: "MDSSegControl",
                        fields: {
                            MDSSegControl: { type: "number" },
                            SegmentControl: { type: "number" },
                            SegmentName: { type: "string", editable: false  }, 
                        }
                    }, 
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access Segment Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            dsMDocSegmentsByLoop.read();

            $("#listBox").kendoGrid({
                dataSource: dsMDocSegmentsByLoop,
                selectable:"row",
                change: onChange,
                height: 200,
                dataBound: function(e) { 
                    var sObj = this; 
                    if (typeof (LoopSegmentsGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(LoopSegmentsGridDataBoundCallBack)) { LoopSegmentsGridDataBoundCallBack(e,sObj); } 
                },
                editable:"inline",
                columns: [
                    { field: "MDSSegControl", title: "MDSSegControl", hidden: true },
                    { field: "SegmentName", title: "Segments", width: "30px", hidden: false},
                    { command: [{ name: "destroy", text:"",   imageClass: "k-i-delete", iconClass: "k-icon"}], title: "Action", width: "10px" }
                ]
            });
            //**********onChange Listbox Segment  bind Element and A******//
            function onChange(e) {
                var data = this.dataItem(this.select());

                //*******Validation And Select Segment in DropDown******//
                $("#SegmentLength-validation").addClass("hide-display");
                $("#segment-validation").addClass("hide-display");
                var ddl1 = $("#ddlSegmentType").data("kendoDropDownList");
                ddl1.value(parseInt(data.SegmentControl));

                var segment = data.SegmentControl;

                $("#SegElementsGrid").empty();

                //******Elemnts and Attributes Grids Update*********//
                ElemntsAndAttributes(segment);

                //************Get Segment Elements Length and Set to Segment Length  *******//
                reFreshLength();
               
                //************* Segmemt Data *************//
                segmentDetailsFill(segment);
            }

            wndAddDocType.center().open();
        }
        
        function SaveEDIMasterDocSegment() {
            
                var otmp = $("#focusCancel").focus();

                var submit = true;
                var Documentnew = $("#ddlSegmentType").val();
                if (Documentnew =="") {
                    $("#segment-validation").removeClass("hide-display");
                    submit = false;
                }

                var sMinC = $("#txtSegmentMinCount").val();
                var sMaxC = $("#txtSegmentMaxCount").val();
                if (sMinC >sMaxC) {
                    $("#eminmax-validation").removeClass("hide-display");
                    submit = false;

                }
                var item = new NGLElement();

            if(SegDetailes.Data.length > 0){
                item.MDSSegControl  = SegDetailes.Data[0].MDSSegControl;
                item.MDSSegMDSLoopControl  = SegDetailes.Data[0].MDSSegMDSLoopControl;
                item.MDSSegSegmentControl  = SegDetailes.Data[0].MDSSegSegmentControl;

                var dataItem2 = $("#ddlSegUsage").data("kendoDropDownList").dataItem();
                item.MDSSegUsage  = dataItem2.UsControl;

                item.MDSSegMinCount = $("#txtSegmentMinCount").data("kendoMaskedTextBox").value();

                item.MDSSegMaxCount = $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value();

                item.MDSSegDesc= $("#txtSegmentDesc").data("kendoMaskedTextBox").value();

                item.MDSSegSeqIndex =  $("#txtSegmentSeqIndex").data("kendoMaskedTextBox").value();

                item.MDSSegDisabled  = SegDetailes.Data[0].MDSSegDisabled;
                item.MDSSegCreateDate  = SegDetailes.Data[0].MDSSegCreateDate;
                item.MDSSegCreateUser  = SegDetailes.Data[0].MDSSegCreateUser;
                item.MDSSegModDate  = SegDetailes.Data[0].MDSSegModDate;
                item.MDSSegModUser  = SegDetailes.Data[0].MDSSegModUser;
                item.MDSSegUpdated  = SegDetailes.Data[0].MDSSegUpdated;

                
                if (submit == true) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIMasterDocStructSegment/UpdateMDocStructLoopSegment",
                        data: JSON.stringify(item),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                var strValidationMsg = "";
                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                    blnSuccess = true;
                                    ngl.showSuccessMsg("Update Successful", null);
                                    segmentDetailsFill(data.Data[0].MDSSegSegmentControl);
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Update Segment Failure"; }
                                    ngl.showErrMsg("Update Segment Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update Segment Failure");
                            ngl.showErrMsg("Update Segment Failure", sMsg, null);
                        }
                    });
                }
                
            }
            else{
                item.MDSSegControl = $("#txtMDSSegControl").val();
                var segmentControl = $("#txtMDSSegControl").val();

                item.MDSSegMDSLoopControl=$("#txtMDSSegMDSLoopControl").val();
            
                var dataItem1 = $("#ddlSegmentType").data("kendoDropDownList").dataItem();
                item.MDSSegSegmentControl  = dataItem1.SegmentControl;

                var dataItem2 = $("#ddlSegUsage").data("kendoDropDownList").dataItem();
                item.MDSSegUsage  = dataItem2.UsControl;

                item.MDSSegMinCount = $("#txtSegmentMinCount").data("kendoMaskedTextBox").value();

                item.MDSSegMaxCount = $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value();

                item.MDSSegDesc= $("#txtSegmentDesc").data("kendoMaskedTextBox").value();

                item.MDSSegSeqIndex =  $("#txtSegmentSeqIndex").data("kendoMaskedTextBox").value();


                if (submit == true) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIMasterDocStructSegment/SaveMDocStructLoopSegment",
                        data: JSON.stringify(item),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                var strValidationMsg = "";
                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                    blnSuccess = true;
                                    ngl.showSuccessMsg("save Successful", null);
                                    refreshLoopsSegmentsGrid();
                                    segmentDetailsFill(data.Data[0].MDSSegSegmentControl);
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Segment Failure"; }
                                    ngl.showErrMsg("Save Segment Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Segment Failure");
                            ngl.showErrMsg("Save Segment Failure", sMsg, null);
                        }
                    });
                }
                
            }

        }

        //***validation Field for min<max****//
        $("#txtSegmentMaxCount").on("change input", function () {
            var length = $(this).val();
            if (length != "") {
                $("#eminmax-validation").addClass("hide-display");
            }
            else {
                $("#eminmax-validation").removeClass("hide-display");
            }
        });

        $("#txtSegmentMinCount").on("change input", function () {
            var length = $(this).val();
            if (length != "") {
                $("#eminmax-validation").addClass("hide-display");
            }
            else {
                $("#eminmax-validation").removeClass("hide-display");
            }
        });
        
        //**********DorpDown with SegElementsGrid********//
        $("#ddlSegmentType").on("change input", function () {
            var datatypesval = $(this).val();
            if (datatypesval != 0) {
                $("#segment-validation").addClass("hide-display");
            }
            else {
                $("#segment-validation").removeClass("hide-display");
            }

            var Seglength = $("#SegmentLength-validation").hasClass("hide-display");
            if (Seglength == false) {
                $("#SegmentLength-validation").addClass("hide-display");
            }

            var segment = datatypesval;

            $("#SegElementsGrid").empty();

            //******Elemnts and Attributes Grids Update*********//
            ElemntsAndAttributes(segment);

            //************Get Segment Elements Length and Set to Segment Length  *******//
            reFreshLength();
               
            //************* Segmemt Data *************//
            segmentDetailsFill(segment);

        });


        //************Global Function for refresh **********//
        function reFreshLength(){
            var grid = $("#SegElementsGrid").data("kendoGrid");
            var dataSource = grid.dataSource;
            
            SegElementGridDataSource.fetch(function() {
                currentSegLength = dataSource.total();
                $("#txtSegmentLength").data("kendoMaskedTextBox").value(dataSource.total());
            });
        }

        function segmentDetailsFill(segmentControl){
            $.ajax({ 
                url: 'api/EDIMasterDocStructSegment/GetRecordbySegmentLoop', 
                contentType: 'application/json; charset=utf-8', 
                dataType: 'json', 
                async: false,
                data: { segment:segmentControl,loop:loopControl  },  
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                success: function(data) {
                    SegDetailes=data;
                } 
            }); 

            if(SegDetailes.Data.length > 0){
                //***** Fill the Segment Detaile**********//
                var usage = SegDetailes.Data[0].MDSSegUsage
                $("#ddlSegUsage").data("kendoDropDownList").value(usage);

                $("#txtSegmentMinCount").data("kendoMaskedTextBox").value(SegDetailes.Data[0].MDSSegMinCount);
                $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value(SegDetailes.Data[0].MDSSegMaxCount);
                $("#txtSegmentDesc").data("kendoMaskedTextBox").value(SegDetailes.Data[0].MDSSegDesc);
                $("#txtSegmentSeqIndex").data("kendoMaskedTextBox").value(SegDetailes.Data[0].MDSSegSeqIndex);
            }
            else{
                var ddlDocument = $("#ddlSegUsage").data("kendoDropDownList");
                ddlDocument.readonly(false);
                ddlDocument.value('M');

                $("#txtSegmentMinCount").data("kendoMaskedTextBox").value(1);
                $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value(1);
                $("#txtSegmentDesc").data("kendoMaskedTextBox").value('');
                $("#txtSegmentSeqIndex").data("kendoMaskedTextBox").value(0);
            }
        }

        function ElemntsAndAttributes(segment){

            //**********Elements Grid ReCreate with fresh******//
            if($('#SegElementsGrid').data("kendoGrid")){
                $('#SegElementsGrid').data("kendoGrid").destroy();
                $("#SegElementsGrid").empty();
            }
            SegElementGridDataSource = new kendo.data.DataSource({
                pageSize: 10,
                transport: {
                    read: function(options) { 
                        $.ajax({ 
                            async:false,
                            url: 'api/EDIMasterDocStructSegment/GetRecordSegmentLoopElements', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { segment:segment,loop:loopControl},  
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
                                            ngl.showErrMsg("Get Segment Element Failure", data.Errors, null);
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
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get Segment Element Failure"; }
                                        ngl.showErrMsg("Get Segment Element Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Segment Element Failure");
                                ngl.showErrMsg("Get Segment Element Failure", sMsg, null); 
                                
                            } 
                        }); 
                    },      
                    update: function(options) {
                        $.ajax({ 
                            url: 'api/EDIMasterDocStructElement/UpdateMDocSegmentElement', 
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
                                            ngl.showErrMsg("Update Segment Element Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                    refreshSegElementsGrid();
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Update Segment Element Failure"; }
                                        ngl.showErrMsg("Update Segment Element Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update Segment Element Failure");
                                ngl.showErrMsg("Update Segment Element Failure", sMsg, null); 
                            } 
                        });
                    },
                    destroy: function(options) {
                        var EleControl = { MDSElementControl : options.data.MDSElementControl }
                        $.ajax({
                            url: 'api/EDIMasterDocStructElement/DeleteRecord', 
                            type: 'POST',
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: JSON.stringify(EleControl),
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function (data) {     
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (data == false) {
                                        ngl.showWarningMsg("Segment Element cannot be deleted!", "", null);
                                    }
                                    if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showWarningMsg("Segment Element cannot be deleted!", data.Errors, null);
                                        }
                                    }
                                
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                if (ngl.isFunction(refreshSegElementsGrid)) {
                                    refreshSegElementsGrid();
                                    reFreshLength();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete Segment Element Failure");
                                ngl.showErrMsg("delete Segment Element Failure", sMsg, null); 
                            } 
                        });
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "MDSElementControl",
                        fields: {
                            MDSElementControl: { type: "number" },
                            MDSElementName: { type: "string", editable: false},
                            MDSElementDesc: { type: "string" },
                            MDSElementMinCount: { type: "number" },
                            MDSElementMaxCount: { type: "number" },
                            MDSElementDisabled: { type: "boolen" },
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access Segment Elements Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            
            SegElementGridDataSource.read();
            $("#SegElementsGrid").kendoGrid({
                dataSource: SegElementGridDataSource,
                pageable: true,
                sortable: {
                    mode: "single",
                    allowUnsort: true
                },
                pageSize: 10,
                height: 400,
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (SegElementsGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(SegElementsGridDataBoundCallBack)) { SegElementsGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                batch: true,
                detailInit: detailInit,
                detailExpand: function(e){
                    e.sender.tbody.find('.k-detail-row').each(function(idx, item){
                        if(item !== e.detailRow[0]){
                            e.sender.collapseRow($(item).prev());
                            $(item).remove();
                        }
                    })
                },
                columns: [
                    { field: "MDSElementControl", title: "MDSElementControl", hidden: true },
                    { field: "MDSElementName", title: "Element Name", hidden: false },
                    { field: "MDSElementDesc", title: "Element Desc", hidden: false },
                    { field: "MDSElementEDIDataTypeControl", title: "Element Data Type", hidden: false,  editor: editDocType, template:data=>dsDataTypeDropDown._data[Object.values(dsDataTypeDropDown._data).findIndex(x=> x.Control == data.MDSElementEDIDataTypeControl)].Name}, 
                    { field: "MDSElementUsage", title: "Element Usage", hidden: false,  editor: editUsageDropDown, template:data=>ddlUsage[Object.values(ddlUsage).findIndex(x=> x.UsControl == data.MDSElementUsage)].UsName},
                    { field: "MDSElementMinCount", title: "MinLength", hidden: false },
                    { field: "MDSElementMaxCount", title: "MaxLength", hidden: false },
                    { command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}},{ name: "destroy", text:"", imageClass: "k-i-delete", iconClass: "k-icon", visible: function(dataItem){ var elength = SegElementGridDataSource._data.length; return dataItem.MDSElementName == SegElementGridDataSource._data[elength-1].MDSElementName} }], title: "Action", width: "80px" }
                ]
            });
            
            
            //************ Chaild Grid Attributes ******************//
            function detailInit(e) {
                var TAttrID = e.data.MDSElementControl;
                window.AttrID =e.data.MDSElementControl;
               
                AttrGridDataSource = new kendo.data.DataSource({
                    pageSize:4,
                    transport:{
                        read: function(options) { 
                            $.ajax({ 
                                async:false,
                                url: 'api/EDIMasterDocStructElmntAttribute/GetRecordbyElementId', 
                                contentType: 'application/json; charset=utf-8', 
                                datatype: 'json',
                                data: { ElementControl: window.AttrID},
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
                                                ngl.showErrMsg("Get Element Attributes Failure", data.Errors, null);
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
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Element Attributes Failure"; }
                                            ngl.showErrMsg("Get Element Attributes Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                               
                                }, 
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Element Attributes Failure");
                                    ngl.showErrMsg("Get Element Attributes Failure", sMsg, null); 
                                
                                } 
                            }); 
                        },
                        create: function(options) {
                            var modelnew = $("#SegElementsGrid").data("kendoGrid").dataItem($(event.target).closest(".k-detail-row").prev(".k-master-row"));
                            options.data.MDSAttrMDSElementControl=modelnew.MDSElementControl;
                            window.AttrID=modelnew.MDSElementControl;
                            $.ajax({ 
                                url: 'api/EDIMasterDocStructElmntAttribute/SaveMDocSegElementAttributes', 
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
                                                ngl.showErrMsg("Save Element Attribute Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                        //refreshLoopElementAttributesGrid();
                                                        $('#grid'+AttrID+'').data("kendoGrid").dataSource.read();
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Save Element Attribute Failure"; }
                                            ngl.showErrMsg("Save Element Attribute Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                                },
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Element Attribute Failure");
                                    ngl.showErrMsg("Save Element Attribute Failure1", sMsg, null); 
                                }
                            });
                        },
                        update: function(options) {
                            window.AttrID=options.data.MDSAttrMDSElementControl;
                            $.ajax({
                                url: 'api/EDIMasterDocStructElmntAttribute/UpdateMDocSegElementAttributes', 
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
                                                ngl.showErrMsg("Update Element Attribute Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                        // refreshLoopElementAttributesGrid();
                                                        $('#grid'+AttrID+'').data("kendoGrid").dataSource.read();
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Update Element Attribute Failure"; }
                                            ngl.showErrMsg("Update Element Attribute Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                                },
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update Element Attribute Failure");
                                    ngl.showErrMsg("Update Element Attribute Failure1", sMsg, null); 
                                } 
                            });
                        },
                        destroy: function(options) {
                            window.AttrID=options.data.MDSAttrMDSElementControl;
                            var EleAttribControl = { MDSAttrControl : options.data.MDSAttrControl}
                            $.ajax({
                                url: 'api/EDIMasterDocStructElmntAttribute/DeleteRecord', 
                                type: 'POST',
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: JSON.stringify(EleAttribControl),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function (data) {     
                                    try {                                    
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (data == false) {
                                            ngl.showWarningMsg("Element Attribute cannot be deleted!", "", null);
                                        }
                                        if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showWarningMsg("Element Attribute cannot be deleted!", data.Errors, null);
                                            }
                                        }
                                
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                                    if (ngl.isFunction(refreshSegElementsGrid)) {
                                        //refreshLoopElementAttributesGrid();
                                        $('#grid'+AttrID+'').data("kendoGrid").dataSource.read();
                                    }
                                },
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete Element Attribute Failure");
                                    ngl.showErrMsg("delete Element Attribute Failure", sMsg, null); 
                                } 
                            });
                        },
                        parameterMap: function (options, operation) { return options; }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "MDSAttrControl",
                            fields: {
                                MDSAttrControl: { type: "number" },
                                MDSAttrQualifyingElementControl: { type: "number" },
                                MDSAttrQualifyingValue: { type: "string" },
                                MDSAttrDefaultVal:{ type: "string" },
                                MDSAttrNotes:{ type: "string" },
                                MDSAttrTransformationTypeControl: { type: "string" },
                                MDSAttrValidationTypeControl: { type: "string" },
                                MDSAttrFormattingFnControl: { type: "string" },
                                MDSAttrDataMapFieldControl:{ type: "string" },
                                MDSAttrDataMapFieldColumnControl:{ type: "string" },
                                MDSAttrMDSElementControl: { type: "number" },
                            }
                        },
                        errors: "Errors"
                    },
                });

                $('<div id="grid'+e.data.MDSElementControl+'" />').appendTo(e.detailCell).kendoGrid({
                    dataSource:AttrGridDataSource,
                    //dataBound: function(e) { 
                    //    var aObj = this; 
                    //    if (typeof (SegElementAttributesGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(SegElementAttributesGridDataBoundCallBack)) { SegElementAttributesGridDataBoundCallBack(e,aObj); } 
                    //},
                    height: 230,
                    pageable: true,
                    editable: "inline",
                    toolbar: [{name:"create", text:"Add new Attribute"}],
                    columns: [
                        { field: "MDSAttrControl", title:"MDSAttrControl", hidden: true},
                        { field: "MDSAttrMDSElementControl", title:"MDSAttrMDSElementControl", hidden: true},
                        { field: "MDSAttrQualifyingElementControl", title:"Qual Elmt Name",width:100, editor: QalyElmtNameDropDownEditor, hidden: false ,template:data=>{ var idx = Object.values(SegElementGridDataSource._data).findIndex(x => x.MDSElementControl == data.MDSAttrQualifyingElementControl); if(idx!= -1){return SegElementGridDataSource._data[idx].MDSElementName}else{return "None"}}},
                        { field: "MDSAttrQualifyingValue", title:"Qual Elmt Value" },
                        { field: "MDSAttrDefaultVal", title:"Default Value" },
                        { field: "MDSAttrNotes", title:"Notes", width: "50px", editor: editMDSAttrNotes, template:(x)=>'<span  class="k-icon k-i-search"></span>'},
                        { field: "MDSAttrTransformationTypeControl", title:"Transformation", editor: TransformationDropDownEditor, hidden: false ,template:data=>{ var idx1 = Object.values(dsTransformationDropDown._data).findIndex(x => x.Control == data.MDSAttrTransformationTypeControl); if(idx1!= -1){return dsTransformationDropDown._data[idx1].Name}else{return "None"}}},
                        { field: "MDSAttrValidationTypeControl", title:"Validation", editor: ValidationDropDownEditor, hidden: false ,template:data=>{ var idx2 = Object.values(dsValidationDropDown._data).findIndex(x => x.Control == data.MDSAttrValidationTypeControl); if(idx2!= -1){return dsValidationDropDown._data[idx2].Name}else{return "None"}}},
                        { field: "MDSAttrFormattingFnControl", title:"FormatFn", editor: FormatingDropDownEditor, hidden: false ,template:data=>{ var idx3 = Object.values(dsFormatingDropDown._data).findIndex(x => x.Control == data.MDSAttrFormattingFnControl); if(idx3!= -1){return dsFormatingDropDown._data[idx3].Name}else{return "None"}}},
                        { field: "MDSAttrDataMapFieldColumnControl", title: "DataMap Tables", editor: DataMapFieldTableDropDownEditor,width:150, template:data=>{ var idx4 = Object.values(dsDataMapFieldColumn._data).findIndex(x => x.DataMapFieldControl == data.MDSAttrDataMapFieldColumnControl); if(idx4!= -1){return dsDataMapFieldColumn._data[idx4].DataMapFieldTable}else{return "None"}}},
                        { field: "MDSAttrDataMapFieldControl", title:"DataMap  Columns", editor: DataMapFieldColumnDropDownEditor,width:150, template:data=>{ var idx5 = Object.values(dsDataMapFieldColumn._data).findIndex(x => x.DataMapFieldControl == data.MDSAttrDataMapFieldControl); if(idx5!= -1){return dsDataMapFieldColumn._data[idx5].DataMapFieldColumn}else{return "None"}}},
                        { command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}},{ name: "destroy", text:"",   imageClass: "k-i-delete", iconClass: "k-icon", visible: function(e){return AttrGridDataSource._data.length > 1;}}], title: "Action", width: "80px" }
                    ]
                });
               
                $('#grid'+TAttrID+'').kendoTooltip({
                    filter: ".k-i-search",
                    content: function(e){
                        var dataItem = $('#grid'+TAttrID+'').data("kendoGrid").dataItem(e.target.closest("tr"));
                        var content = dataItem.MDSAttrNotes;
                        return content;
                    },
                });
            }
        }

        //*********Popup Editor*********//
        $("#window").kendoWindow({
            title: "Attribute Notes",
            width: "500px",
            height: "400px",
            visible: false,
            modal: true,
            close: function(e) {
                e.sender.element.focus();
            },
            actions: [
                "Maximize",
                "Close"
            ],
        });
        $("#editor").kendoEditor();

            //*********Popup Editor function *********//
        function editMDSAttrNotes(container, options){
            $('<a href="#"><span id="editNotes"  class="k-icon k-i-search" value=' + options.field + '></span></a>').appendTo(container).click(function(){
                var editor = $("#editor").data("kendoEditor");
                var editwindow = $("#window").data("kendoWindow");
                var dataItem = options.model;
               
                kendo.bind(editor.element, dataItem);
                editwindow.open().center();
            });
        }


        //******** in-line Edit for Dropdowns and numbers ********//
      
        function editDocType(container, options) {
            $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoBind: true,
                    dataSource: dsDataTypeDropDown,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("Control", fee.ListTypeControl);
                        options.model.set("Name", fee.ListTypeName);
                    }
                });
        }

        function editUsageDropDown(container, options) {
            $('<input required data-text-field="UsName" data-value-field="UsControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "UsName",
                    dataValueField: "UsControl",
                    autoBind: true,
                    dataSource: ddlUsage,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("UsControl", fee.ListTypeControl);
                        options.model.set("UsName", fee.ListTypeName);
                    }
                });
        }
        
        function FormatingDropDownEditor(container, options) {
            if(options.model.MDSAttrFormattingFnControl==0){
                $('<input id="FormatingDropDownLists" required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Control",                    
                        autoBind: true,                        
                        dataSource: dsFormatingDropDown,
                        index: 0,
                        dataBound: function(){

                            options.model[options.field] = dsFormatingDropDown._data[0].Control;
                        },//end databound
                        change: function (e) {
                            var fee = this.dataItem(e.item);
                            options.model.set("Control", fee.ListTypeControl);
                            options.model.set("Name", fee.ListTypeName);
                        }
                    });
            }
            else
            {
                $('<input id="FormatingDropDownLists" required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Control",                    
                        autoBind: true,                        
                        dataSource: dsFormatingDropDown,
                        change: function (e) {
                            var fee = this.dataItem(e.item);
                            options.model.set("Control", fee.ListTypeControl);
                            options.model.set("Name", fee.ListTypeName);
                        }
                    });
            }
        }

        function DataMapFieldTableDropDownEditor(container, options) {
            if(options.model.MDSAttrDataMapFieldColumnControl==0){
                $('<input id="attriTableDropDownList" required data-text-field="DataMapFieldTable" data-value-field="DataMapFieldControl" />')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "DataMapFieldTable",
                    dataValueField: "DataMapFieldControl",                    
                    autoBind: true,
                    dataSource: dsDataMapFieldTable,
                    dataBound: function(){

                        options.model[options.field] = 1;
                    },//end databound
                    change: function (e) {
                        /*******Onchanging of Table Dropdrown Column Dropdown Bind******/
                        var fee = this.dataItem(e.item);
                        var totalDS = dsDataMapFieldColumn._data;
                        var newDS = [];
                        for (var i= 0;i < totalDS.length ;i++){
                            if(totalDS[i].DataMapFieldTable == fee.DataMapFieldTable){
                                newDS.push(totalDS[i])
                            }
                        }
                        $("#attriColumnDropDownList").data("kendoDropDownList").dataSource.data(newDS);
                    }
                });
            }
            else
            {
                var tableID = options.model.MDSAttrDataMapFieldControl;
                var tablename = columnDataSource.find(x=>x.DataMapFieldControl == tableID).DataMapFieldTable;
                $('<input id="attriTableDropDownList" required data-text-field="DataMapFieldTable" data-value-field="DataMapFieldControl" />')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "DataMapFieldTable",
                        dataValueField: "DataMapFieldControl",                    
                        autoBind: true,
                        index:"",
                        dataSource: dsDataMapFieldTable,
                        change: function (e) {
                            /*******Onchanging of Table Dropdrown Column Dropdown Bind******/
                            var fee = this.dataItem(e.item);
                            var totalDS = dsDataMapFieldColumn._data;
                            var newDS = [];
                            for (var i= 0;i < totalDS.length ;i++){
                                if(totalDS[i].DataMapFieldTable == fee.DataMapFieldTable){
                                    newDS.push(totalDS[i])
                                }
                            }
                            $("#attriColumnDropDownList").data("kendoDropDownList").dataSource.data(newDS);
                        }
                    });
                $("#attriTableDropDownList").data("kendoDropDownList").text(tablename);
            }
            
        }
        var columnDataSource;
        function DataMapFieldColumnDropDownEditor(container, options) {
            
            var tableName  =  $("#attriTableDropDownList").data("kendoDropDownList").text();
            var totalDS = columnDataSource;
            var fnewDS = [];
            for (var i= 0;i < totalDS.length ;i++){
                if(totalDS[i].DataMapFieldTable == tableName){
                    fnewDS.push(totalDS[i])
                }
            }
            $('<input id="attriColumnDropDownList" required data-text-field="DataMapFieldColumn" data-value-field="DataMapFieldControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "DataMapFieldColumn",
                    dataValueField: "DataMapFieldControl",  
                    optionLabel:"Select Column",
                    autoBind: true,
                    dataSource: fnewDS,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                    }
                });
        }

        function ValidationDropDownEditor(container, options) {
            if(options.model.MDSAttrValidationTypeControl==0){
                $('<input id="sssssss" required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Control",
                        autoBind: true,                        
                        dataSource: dsValidationDropDown,
                        index: 0,
                        dataBound: function(){

                            options.model[options.field] = dsValidationDropDown._data[0].Control;
                        },//end databound
                        change: function (e) {
                            var fee = this.dataItem(e.item);
                            options.model.set("Control", fee.ListTypeControl);
                            options.model.set("Name", fee.ListTypeName);
                        
                        }
                    });
            }
            else
            {
                $('<input id="sssssss" required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Control",
                        autoBind: true,                        
                        dataSource: dsValidationDropDown,
                        change: function (e) {
                            var fee = this.dataItem(e.item);
                            options.model.set("Control", fee.ListTypeControl);
                            options.model.set("Name", fee.ListTypeName);
                        
                        }
                    });
            }

            // $("#sssssss").data("kendoDropDownList").value(2);
        }

        function TransformationDropDownEditor(container, options) {
            if(options.model.MDSAttrTransformationTypeControl==0){
                $('<input id="TransformationDropDownLists" required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Control",                    
                        autoBind: true,                        
                        dataSource: dsTransformationDropDown,
                        index: 0,
                        dataBound: function(){

                            options.model[options.field] = dsTransformationDropDown._data[0].Control;
                        },//end databound
                        change: function (e) {
                            var fee = this.dataItem(e.item);
                            options.model.set("Control", fee.ListTypeControl);
                            options.model.set("Name", fee.ListTypeName);
                        }
                    });
            }
            else{
                $('<input id="TransformationDropDownLists" required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Control",                    
                        autoBind: true,                        
                        dataSource: dsTransformationDropDown,
                        change: function (e) {
                            var fee = this.dataItem(e.item);
                            options.model.set("Control", fee.ListTypeControl);
                            options.model.set("Name", fee.ListTypeName);
                        }
                    });
            }
        }

        function QalyElmtNameDropDownEditor(container, options) {
            var parentData = $("#SegElementsGrid").data("kendoGrid").dataItem($(event.target).closest(".k-detail-row").prev(".k-master-row")); 
            if(options.model.MDSAttrMDSElementControl==0){
                var newDS = SegElementGridDataSource._data;
                var VeryNewDS =[{MDSElementControl:'0',MDSElementName:'None'}];
                for(var i=0; i<newDS.length;i++){
                    if(newDS[i].MDSElementControl == parentData.MDSElementControl){

                    }else{
                        VeryNewDS.push(newDS[i])
                    }
                }
                $('<input required data-text-field="MDSElementName" data-value-field="MDSElementControl" data-bind="value:' + options.field + '"/>')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "MDSElementName",
                        dataValueField: "MDSElementControl",
                        autoBind: true,
                        dataSource: VeryNewDS,
                        change: function (e) {
                            var fee = this.dataItem(e.item);
                            options.model.set("MDSElementControl", fee.ListTypeControl);
                            options.model.set("MDSElementName", fee.ListTypeName);
                        }
                    });
            }
            else
            {
                var newDS = SegElementGridDataSource._data;
                var VeryNewDS =[{MDSElementControl:'0',MDSElementName:'None'}];
                for(var i=0; i<newDS.length;i++){
                    if(newDS[i].MDSElementControl == options.model.MDSAttrMDSElementControl){

                    }else{
                        VeryNewDS.push(newDS[i])
                    }
                }
                $('<input required data-text-field="MDSElementName" data-value-field="MDSElementControl" data-bind="value:' + options.field + '"/>')
                    .appendTo(container)
                    .kendoDropDownList({
                        dataTextField: "MDSElementName",
                        dataValueField: "MDSElementControl",
                        autoBind: true,
                        dataSource: VeryNewDS,
                        change: function (e) {
                            var fee = this.dataItem(e.item);
                            options.model.set("MDSElementControl", fee.ListTypeControl);
                            options.model.set("MDSElementName", fee.ListTypeName);
                        }
                    });
            }
            
        }
       
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
                        //Modified By SRP on 2/22/2018 ElementAddExample
                        content: "<button id='btnAddElement' class='k-button actionBarButton' type='button' onclick='execActionClick(btnAddElement, 160);'><span class='k-icon k-i-add'></span>Add loop to Document Structure</button> <button id='btnPreviewDoc' class='k-button actionBarButton' type='button' onclick='viewPreview()'><span class='k-icon k-i-preview'></span>Preview Document Structure</button>"                        
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
              var l = "<h3 style='margin:10px'>"+EDITName+"- "+MDocInOutbound+" Document Configuration </h3>";
            $("#txtScreenMessage").html(l);

            //define Kendo widgets
            $("#txtSegmentMinCount").kendoMaskedTextBox();    //Added By SRP on 3/8/18 EDILoopAddExample
            $("#txtSegmentMaxCount").kendoMaskedTextBox();    //Added By SRP on 3/8/18 EDILoopAddExample
            $("#txtSegmentLength").kendoMaskedTextBox();
            $("#txtSegmentDesc").kendoMaskedTextBox();
            $("#txtSegmentSeqIndex").kendoMaskedTextBox();

            /////////////DropDown Data Source's/////////////
            LoopsData = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "/api/EDILoops/GetRecords/",
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
                        id: "LoopControl",
                        fields: {
                            LoopControl: { type: "number" },
                            LoopName: { type: "string" }, 
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
            LoopsData.read();

            dsFormatingDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/vLookupList/GetStaticList/" + nglStaticLists.tblEDIFormattingFunctions, //Modified By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }                      
                    },
                    parameterMap: function (options, operation) { return options; }
                    
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: { 
                        id: "FormattingFnControl",
                        fields: {
                            FormattingFnControl: { type: "number" },
                            FormattingFnName: { type: "string" }, 
                            FormattingFnDesc: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsFormatingDropDown.read();

            dsDataTypeDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/vLookupList/GetStaticList/" + nglStaticLists.tblEDIDataType, //Modified By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
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
                            Control: { type: "number" },
                            Name: { type: "string" }, 
                            Description: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsDataTypeDropDown.read();

            // SegmentElements Lists     
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
          
            dsSegmentDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDISegment/GetRecords",
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
                        id: "SegmentControl",
                        fields: {
                            SegmentControl: { type: "number" },
                            SegmentName: { type: "string" }, 
                            SegmentDesc: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsSegmentDropDown.read();

            dsDataMapFieldTable = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDIDataMapField/GetTableRecords",
                        //url: "api/EDIDataMapField/GetRecords",
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
                        id: "DataMapFieldControl",
                        fields: {
                            DataMapFieldControl: { type: "number" },
                            DataMapFieldTable: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsDataMapFieldTable.read();
            
            $.ajax({ 
                url: 'api/EDIDataMapField/GetRecords', 
                contentType: 'application/json; charset=utf-8', 
                dataType: 'json', 
                async: false,
                data: { filter: JSON.stringify({"filterName":"None","filterValue":"","page":1,"skip":0,"take":100}) }, 
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                success: function(data) {
                    columnDataSource=data.Data;
                } 
            }); 

           dsDataMapFieldColumn = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDIDataMapField/GetRecords",
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
                        id: "DataMapFieldControl",
                        fields: {
                            DataMapFieldControl: { type: "number" },
                            DataMapFieldColumn: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsDataMapFieldColumn.read();
           
            
            dsSegmentElementDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDIMasterDocStructElement/GetRecords",
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
                        id: "MDSElementControl",
                        fields: {
                            MDSElementName: { type: "string" },
                            MDSElementDesc: { type: "string" },
                            MDSElementEDIDataTypeControl: { type: "number" },
                            MDSElementUsage: { type: "string" },
                            MDSElementMinCount: { type: "number" },
                            MDSElementMaxCount: { type: "number" },
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDIElement Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            dsSegmentElementDropDown.read();

            dsFormatingDropDown = new kendo.data.DataSource({
                transport: {
                    read: {

                        url: "api/vLookupList/GetStaticList/" + nglStaticLists.tblEDIFormattingFunctions, //Modified By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
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
                            Control: { type: "number" },
                            Name: { type: "string" }, 
                            Description: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsFormatingDropDown.read();

            dsValidationDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/vLookupList/GetStaticList/" + nglStaticLists.tblEDIValidationTypes, //Modified By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
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
                            Control: { type: "number" },
                            Name: { type: "string" }, 
                            Description: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsValidationDropDown.read();

            dsTransformationDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/vLookupList/GetStaticList/" + nglStaticLists.tblEDITransformationTypes, //Modified By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
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
                            Control: { type: "number" },
                            Name: { type: "string" }, 
                            Description: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            dsTransformationDropDown.read();
          

            //**********ADD DropDowns**********//
            $("#ddlMDSLoopLoopControl").kendoDropDownList({
                dataSource: LoopsData,
                dataTextField: "LoopName",
                dataValueField: "LoopControl",
                optionLabel:"Select Loop",
                autoWidth: true,
                filter: "contains",
            });

            $("#lblMDSLoopLoopControl").kendoDropDownList({
                dataSource: LoopsData,
                dataTextField: "LoopName",
                dataValueField: "LoopControl",
                optionLabel:"Select Loop",
                autoWidth: true,
                filter: "contains",
            });

            $("#ddlMDSLoopParentLoopID").kendoDropDownList({
                dataSource: LoopsData,
                dataTextField: "LoopName",
                dataValueField: "LoopControl",
                optionLabel:"Select Loop",
                autoWidth: true,
                filter: "contains",
            });

            $("#ddlMDSLoopUsage").kendoDropDownList({
                dataSource: ddlUsage,
                dataTextField: "UsName",
                dataValueField: "UsControl",
                autoWidth: true,
                filter: "contains",
            });

            $("#ddlSegmentType").kendoDropDownList({
                dataSource: dsSegmentDropDown,
                dataTextField: "SegmentName",
                dataValueField: "SegmentControl",
                optionLabel:"Select Segment",
                autoWidth: true,
                filter: "contains",
            });

            $("#ddlSegUsage").kendoDropDownList({
                dataSource: ddlUsage,
                dataTextField: "UsName",
                dataValueField: "UsControl",
                autoWidth: true,
                filter: "contains",
            });

            ////////////Grid///////////////////
            EDIMasterDoc = new kendo.data.TreeListDataSource({
                serverSorting: false, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();

                        s.filterName = "MDSLoopMasterDocControl";
                        s.filterValue = doccontrol;

                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;

                        $.ajax({ 
                            url: '/api/EDIMDocStructLoop/GetRecords/', 
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
                                            //ngl.showErrMsg("Get EDI MasterDoc Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                } else {
                                                    blnSuccess = true;
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get EDI MasterDoc Failure"; }
                                        ngl.showErrMsg("Get EDI MasterDoc Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Data Failure");
                                ngl.showErrMsg("Get EDI MasterDoc Failure", sMsg, null); 
                                
                            } 
                        }); 
                    },
                    destroy: function(options) {
                        var deteteLoop = confirm("Are you sure you want to delete this record?");
                        if(deteteLoop == true){
                            $.ajax({
                                url: 'api/EDIMDocStructLoop/DeleteMParentLoopRecord', 
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
                                        if (data == false) {
                                            //ngl.showWarningMsg("EDIMasterDocStructLoop Not Deleted!, it cannot be deleted!", "", null);
                                        
                                        }
                                        if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                //ngl.showWarningMsg("EDIMasterDocStructLoop already exist with this EDI MasterDocStructLoop, it cannot be deleted!", data.Errors, null);
                                            }
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                                    if (ngl.isFunction(refreshLoopsTreeList)) {
                                        refreshLoopsTreeList();
                                    }
                                },
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete EDIMasterDocStructLoop Failure");
                                    ngl.showErrMsg("delete EDIMasterDocStructLoop Failure", sMsg, null); 
                                } 
                            });
                        }else{
                            refreshLoopsTreeList();
                        }
                    },
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "MDSLoopLoopControl",
                        parentId: "MDSLoopParentLoopID",
                        fields: {
                            MDSLoopLoopControl: { field: "MDSLoopLoopControl", type: "number" },
                            MDSLoopParentLoopID: { field: "MDSLoopParentLoopID", type: "number" },
                            MDSLoopSeqIndex: { field: "MDSLoopSeqIndex", type: "number" }
                        },
                        expanded: true
                    },
                    errors: "Errors"
                },
                sort: [
                    { field: "MDSLoopSeqIndex", dir: "asc"}
                ],
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDI MasterDoc Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            dsSegmentElementDropDown.read()
            $("#LoopsTreeGrid").kendoTreeList({
                dataSource: EDIMasterDoc,
                height: 400,
                dataBound: function(e) { 
                    var LoopsObj = this; 
                    if (typeof (LoopsTreeGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(LoopsTreeGridDataBoundCallBack)) { LoopsTreeGridDataBoundCallBack(e,LoopsObj); } 
                },
                pageable: true,
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                       { field: "MDSLoopControl", title: "MasterDocControl ", hidden: true },
                       { field: "MDSLoopLoopControl", title: "Loop", width:300, hidden: false, template:data=>LoopsData._data[Object.values(LoopsData._data).findIndex(x=> x.LoopControl == data.MDSLoopLoopControl)].LoopName},
                       { field: "MDSLoopUsage", title: "Usage", width:200, hidden: false, template:data=>ddlUsage[Object.values(ddlUsage).findIndex(x=> x.UsControl == data.MDSLoopUsage)].UsName},
                       { field: "MDSLoopMinCount", title: "Minimum Count", width:100, hidden: false},
                       { field: "MDSLoopMaxCount", title: "Maximum Count", width:100, hidden: false},
                        { command: [{ name: "Edit Loop", text:" ",imageClass: "k-i-edit", click: openMasterDocEditWindow},{ name: "Associate Segment", text:" ",imageClass: "k-i-hyperlink", click: openMasterDocStructLoopEditWindow},{name: "destroy", text: " ",imageClass: "k-i-delete"}], title: "Action", width: "100px" },
                ]
            });

              //*****ToolTip*********//
            $("#LoopsTreeGrid").kendoTooltip({
                filter: ".k-i-edit",
                content: "Edit&nbsp;Loop&nbsp;Details"
            });
           
            $("#LoopsTreeGrid").kendoTooltip({
                filter: ".k-i-hyperlink",
                content: "Edit&nbsp;Loop&nbsp;Structure"
            });

            $("#LoopsTreeGrid").kendoTooltip({
                filter: ".k-i-delete",
                content: "Delete&nbsp;Loop"
            });
           
              //************Dummy Grid*******//
            $("#SegElementsGrid").kendoGrid({
                dataSource: SegElementGridDataSource,
                pageable: true,
                pageSize: 10,   
                height: 400,
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (SegElementsGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(SegElementsGridDataBoundCallBack)) { SegElementsGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                    { field: "MDSElementControl", title: "MDSElementControl", hidden: true },
                    { field: "MDSElementName", title: "Element Name", hidden: false },
                    { field: "MDSElementDesc", title: "Element Desc", hidden: false },
                    { field: "MDSElementEDIDataTypeControl", title: "Element Data Type", hidden: false,  editor: editDocType, template:data=>dsDataTypeDropDown._data[Object.values(dsDataTypeDropDown._data).findIndex(x=> x.Control == data.MDSElementEDIDataTypeControl)].Name}, 
                    { field: "MDSElementUsage", title: "Element Usage", hidden: false,  editor: editUsageDropDown, template:data=>ddlUsage[Object.values(ddlUsage).findIndex(x=> x.UsControl == data.MDSElementUsage)].UsName},
                    { field: "MDSElementMinCount", title: "MinLength", hidden: false },
                    { field: "MDSElementMaxCount", title: "MaxLength", hidden: false },
                    { command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}},{ name: "destroy", text:"", imageClass: "k-i-delete", iconClass: "k-icon", visible: function(dataItem){ var elength = SegElementGridDataSource._data.length; return dataItem.MDSElementName == SegElementGridDataSource._data[elength-1].MDSElementName} }], title: "Action", width: "80px" }
                ]
            });


            ////////////wndAddEDIDocumentType/////////////////

            kendoWin.height = '80%';// For Updating Kendowindow height config
            kendoWin.width = '65%';// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddDocType = $("#wndAddMasterDocStructLoop").kendoWindow(kendoWin).data("kendoWindow");
            $("#wndAddMasterDocStructLoop").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDIMasterDocSegment(); });
            
            ////////////wnd Add EDIStruct Ele/////////////////
            kendoWin.height = 500;// For Updating Kendowindow height config
            kendoWin.width = 300;// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddMDocStructLoops = $("#wndAddMDocStructLoops").kendoWindow(kendoWin).data("kendoWindow");

            $("#wndAddMDocStructLoops").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDIMasterDoc(); });


            ////////////wnd Document Preview/////////////////
            kendoWin.height = '75%';// For Updating Kendowindow height config
            kendoWin.width = '60%';// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            DocumentPreview = $("#DocumentPreview").kendoWindow(kendoWin).data("kendoWindow");

            $("#DocumentPreview").data("kendoWindow").wrapper.find('a[aria-label="save"]').remove();
          

        });
        </script>
        <style>
            .k-treelist tbody td .k-button {
                min-width: 18px;
                width: 28px;
            }
            .k-treelist tbody tr td 
            {
                vertical-align: top
            }
             .k-treelist td {
                 border-width: 0;
            }
           .k-grid tbody .k-button {
                min-width: 18px;
                width: 28px;
            }
           .k-grid .k-grid-toolbar .k-button {
                min-width: 200px;
                width: 200px;
                min-height: 18px;
                height: 28px;
            }
            .k-grid tbody tr td {
                vertical-align: top;
            }
        </style>
    </div>
</body>

</html>

