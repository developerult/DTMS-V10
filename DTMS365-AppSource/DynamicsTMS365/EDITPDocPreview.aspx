    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDITPDocPreview.aspx.cs" Inherits="DynamicsTMS365.EDITPDocPreview" %>

<!DOCTYPE html>

<html>
<head>
    <title>EDI TP Document Preview</title>         
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
        .ui-legend-container {
            color:black;font-weight:bold; 
        }
        .ui-fieldset-container {
            border-color:#BBDCEB; border-width:1px;width:1165px;margin:10px; margin-left:30px;
        }
        .ui-border-container {
            margin:20px; font-size:1.1em;
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
                        <div id="id260" style="margin-top: 10px;">
                            
                            <div id="ParHeader" class="OpenOrders">
                                <div id="Parwrapper">
                                    <%--Filters--%>
                                    <div>
                                        <div class="getPdf size-a4" >
                                           <fieldset id="form" class="ui-fieldset-container">
                                              <legend class="ui-legend-container"><span class='k-icon k-i-file-txt'></span>Document Details</legend>
                                              <div id="show" class="ui-border-container"></div>
                                           </fieldset>
                                        </div>
                                    </div>                                    
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

            //*****Get Doc Control Function************//
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
            var TPDocControl = getParameterByName('TPDocControl');

            var view = viewPreview();

            function viewPreview(){
                $("#show").empty();
                $.ajax({ 
                    url: 'api/EDITPDocument/GetTPDocumentPreview', 
                    contentType: 'application/json; charset=utf-8', 
                    dataType: 'json', 
                    async: false,
                    data: { TPDocControl: TPDocControl},  
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                    success: function(data) {
                        DocumnetDetails=data.Data;
                    } 
                }); 
               

            //*************Date Format**************//
            var m_names = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');
            var d = new Date();
            var curr_date = d.getDate();
            var curr_month = d.getMonth();
            var curr_year = d.getFullYear();
          
            //*********Header of The View*************//
            var DocHead = '<% = System.Configuration.ConfigurationManager.AppSettings["NGLApplicationName"]%>' ;
            var header="";
            header += "<h3 class='Docheader'>"+DocHead+"</h3>";
            header += "<h4 class='DOCDetails'>"+DocumnetDetails[0].EditDescription+"</h4>"
            header += "<h4 class='DOCDetails'>"+DocumnetDetails[0].TPDocType+" "+DocumnetDetails[0].EDITName+"</h4>"
            header += "<h4 class='DOCDetails'>Version "+DocumnetDetails[0].Version+"</h4>"
            header += "<h5 class='cuttentDate'>"+DocumnetDetails[0].CompName+" - "+DocumnetDetails[0].CarrierName+"</h5>"
            header += "<h5 class='cuttentDate'>"+ m_names[curr_month]+" "+ curr_date +", " + curr_year+"</h5>"
            header += "<hr class='hederHR'></hr></br>"
            $("#show").append(header);



                var DSLoopParentLoopID = 0;
                var structhtml = "";
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
                        structhtml += "<h2'><b>Begin Loop: "+lArray[i].LoopName+"</b></h2></br>";
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
                                            structhtml +="<tr><td class='td_align'>"+ElementsArray[k].ElementName+"</td><td >"+ElementsArray[k].ElementDesc+"</td><td class='td_align'>"+ElementsArray[k].DSElementUsage+"</td><td class='td_align'>"+ElementsArray[k].ElementMinLength+"-"+ElementsArray[k].ElementMaxLength+"</td><td>"+ElementsArray[k].DSAttrNotes+"</td><td>"+ElementsArray[k].DataMapFieldTable+"."+ElementsArray[k].DataMapFieldColumn+"</td></tr>";
                                        }
                                    }
                                    structhtml +="</tbody></table>";
                                }
                            }
                        }else{
                            structhtml += "</br>";
                        }
                        structhtml += "<h2'><b>End Loop: "+lArray[i].LoopName+"</b></h2></br></br>";
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
                                    structhtml +="</fieldset>";
                                }
                            }else{
                                //structhtml +="</fieldset>";
                            }
                            count = 0;
                        }
                    }
                }
                $("#show").append(structhtml);
            }

            function getPDF(selector) {
                kendo.drawing.drawDOM($(selector)).then(function(group){
                    kendo.drawing.pdf.saveAs(group, "EDITPDocument.pdf");
                });
            }

            //*************  Action Functions  **********************
            function execActionClick(btn, proc){
                if(btn.id == "exportDocument"){ 
                    getPDF(".getPdf");
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
                            content: "<button id='exportDocument' class='k-button actionBarButton' type='button' onclick='execActionClick(exportDocument, 160);' ><span class='k-icon k-i-hyperlink-open-sm'></span>Export Document to PDF</button>",
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
