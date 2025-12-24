<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cmPages.aspx.cs" Inherits="DynamicsTMS365.cmPages" %>

<%--Created By RHR on 04/18/17 for v-8.0 Content Management Page Configuration--%>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Content Management Page Configuration</title>        
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
                      <div id="horizontal" style="height: 100%; width: 100%; background-color: white;">                        
                        <div id="left-pane">
                            <div class="pane-content">
                                <div><span>Menu</span></div>ss
                                <div id="menuTree"></div>
                            </div>
                        </div>
                        <div id="center-pane">                            
                            <% Response.Write(PageErrorsOrWarnings); %> 
                            <div class="ngl-blueBorder" style="min-width:780px; margin-left:2px; margin-right:0px; padding-right:0px; width:auto;">                                
                                <div style="padding: 8px;">
                                    <span><input id="cmPage" value="1" style="width: 25%;" />&nbsp; Select a Page or &nbsp;
                                        <a id="btnEditPage" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnOpenWinEditPage_Click();" style="width:auto;" href="#">
                                            <span class="k-icon k-i-edit"></span>&nbsp;Edit Page&nbsp;
                                        </a>
                                        &nbsp; or &nbsp;
                                        <a id="btnAddPage" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnOpenWinNewPage_Click();" style="width:auto;" href="#">
                                            <span class="k-icon k-i-plus"></span>&nbsp;Add a New Page&nbsp;
                                        </a>
                                     </span>                                    
                                    <div class="demo-section k-content wide">
                                            <div id="products"></div>
                                    </div>
                                    <br />
                                    <div style="position:relative; float:left; display:inline-block; width:340px;">  
                                        <div id="divPageDetail"  style="width: auto; max-height: 450px;  overflow: auto; position: relative;">      
                                                  <div id="PageDetailGrid"></div>
                                        </div>
                                    </div>
                                    <div style="position:relative; float:left; display:inline-block; width:100px; margin-left:5px; margin-right:5px; vertical-align:top;">
                                        <h4 style="margin:0px; padding: 0px; vertical-align:top;">Actions</h4>  
                                        <p><a id="btnMoveUp" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnMoveUp_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-sort-asc-small"></span>&nbsp;Up&nbsp;</a>
                                        </p> 
                                         <p><a id="btnMoveDown" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnMoveDown_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-sort-desc-sm"></span>&nbsp;Down&nbsp;</a>
                                        </p>  
                                         <p><a id="btnSaveDetails" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnSaveDetails_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-save"></span>&nbsp;Save&nbsp;</a>
                                        </p>  
                                         <p><a id="btnDeleteDetails" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnDeleteDetails_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-trash"></span>&nbsp;Delete&nbsp;</a>
                                        </p>  
                                         <p><a id="btnNewDetails" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnNewDetails_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-plus"></span>&nbsp;Add&nbsp;</a>
                                        </p>    
                                         <p><a id="btnAddDataElements" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnAddDataElements_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-plus"></span>&nbsp;Add Elemts.&nbsp;</a>
                                        </p>  
                                        <p><a id="btnCopyDescriptionToCaption" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnCopyDescriptionToCaption_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-save"></span>&nbsp;Copy Description to Caption&nbsp;</a>
                                        </p>                                         
                                        <p><a id="btnMakeEditFilter" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnMakeEditFilter_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-save"></span>&nbsp;Edit & Filter&nbsp;</a>
                                        </p>  
                                        <p><a id="btnMakeReadOnlyFilter" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnMakeReadOnlyFilter_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-save"></span>&nbsp;Readonly & Filter&nbsp;</a>
                                        </p>  
                                        <p><a id="btnMakeHiddenReadonly" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnMakeHiddenReadonly_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-save"></span>&nbsp;Hide & Readonly&nbsp;</a>
                                        </p>                                          
                                        <p><a id="btnHideAllControls" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="btnHideAllControls_Click();" style="width:100px;" href="#">
                                            <span class="k-icon k-i-save"></span>&nbsp;Hide all Controls&nbsp;</a>
                                        </p>       
                                    </div>
                                    <div style="position:relative; float:left; display:inline-block; width:calc(100% - 450px); margin-top:0px; padding-top:0px; vertical-align:top;" > 
                                        <h4 style="margin:0px; padding:0px; vertical-align:top;">Edit Page Element Details</h4>                                       
                                        <% Response.WriteFile("~/Views/CMPageDetailCtrl.html"); %>                                                                       
                                    </div>                                     
                                </div>
                            </div>
                        </div>
                      </div>
                </div>
                <div id="bottom-pane" style="height: 100%; width: 100%; background-color: #daecf4; ">
                    <div class="pane-content">   
                         <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
             </div>
             <%--<div id="createNewPageWindow"></div>--%>
            <% Response.WriteFile("~/Views/CreateCMPageCtrl.html"); %>  
            <% Response.WriteFile("~/Views/CMNewPageDetailCtrl.html"); %>  
              
    <%--<script type="text/x-kendo-template" id="template">
        <div class="product">     
            <input id="txtPageControl" type="hidden" value="#:PageControl#" /> 
            <input id="txtPageUpdated" type="hidden" value="#:PageUpdated#" />  
            <input id="chkPagePageable" checked="${PagePageable}" type="checkbox" data-role="switch" data-align="right" />                             
            <h2>#:PageName#</h2>
            <h3>#:PageDesc#</h3>
            <p>#:PageCaption#</p>           
            <p>#:PageCaptionLocal#</p>
            <p>#:PageFormControl#</p>
            <p>#:PageDataSource#</p>
            <p>#:PageSortable#</p>
            <p>#:PagePageable#</p>
            <p>#:PageGroupable#</p>
            <p>#:PageEditable#</p>
            <p>#:PageDataElmtControl#</p>
            <p>#:PageElmtFieldControl#</p>
            <p>#:PageAutoRefreshSec#</p>
            <p>#:PageModDate#</p>
            <p>#:PageModUser#</p>
        </div>
    </script>--%>

    <script type="text/x-kendo-tmpl" id="pgtemplate">
        # var sType = ctrlSubTypes.getSubTypeName(PageDetGroupSubTypeControl); #        
        <div class="product-view k-widget">
            <span><b>#:PageDetName#</b>&nbsp;ParentID:&nbsp;#:PageDetParentID#</span>                
            <br>
            <span>Element:&nbsp;#:sType#&nbsp;Sequence:&nbsp;#:PageDetSequenceNo#</span>
        </div>
    </script>
    
    <script type="text/x-kendo-tmpl" id="pgaltTemplate">
        # var sType = ctrlSubTypes.getSubTypeName(PageDetGroupSubTypeControl); #
        <div class="product-view k-widget alt">
            <span><b>#:PageDetName#</b>&nbsp;ParentID:&nbsp;#:PageDetParentID#</span>                
            <br>
            <span>Element:&nbsp;#:sType#&nbsp;Sequence:&nbsp;#:PageDetSequenceNo#</span>
        </div>
    </script>
   <% Response.Write(AuthLoginNotificationHTML); %>  
    <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var tObj = this;
        var tPage = this; 

        <% Response.Write(NGLOAuth2); %>

        
        <% Response.Write(PageCustomJS); %>
        var iEditPageDetControl = 0;
        //Begin local functions and properties
        var UserToken = '<%=UserToken%>'; 
        var cmPageKey = 31;
        var cmFormListKey = 28;
        var cmPagesDataSource = kendo.data.DataSource;
        var cmPageDataSource = kendo.data.DataSource;
        var pgDetailsData = kendo.data.TreeListDataSource;
        var intPageControl = 0;
        var intPageDetControl = 4;
        var ctrlSubTypes = new GroupSubTypes();
        var pgTreeList = kendo.data.kendoTreeList;
        var pgTreeListSelectedIndex = 0;
        var pgTreeListSelecting = false;
        //End local functions and properties
        
        //Begin Create/Edit Page Region
        var winNewPage = kendo.ui.Window;

        var oPage = new cmPage();
        var oCreatePageCtrl = new createCMPageCtrl();

        function pgTreeListSelect(){
            //pgTreeListSelecting = true;
            //debugger;
            var treeList = $("#PageDetailGrid").data("kendoTreeList");
            if (typeof (treeList) !== "undefined" ){                
                var dataItem  = treeList.dataSource.get(iEditPageDetControl);
                if (typeof (dataItem) !== "undefined" ){
                    iEditPageDetControl = 0;  //clear the selected item control so select will update record data
                    treeList.select($("#PageDetailGrid tbody>tr[data-uid=" + dataItem.uid + "]"));
                     var itemScrollTop = treeList.select()[0].offsetTop;
                    $("#PageDetailGrid").animate({ scrollTop: itemScrollTop });
                    //var row = treeList.content.find("tr[data-uid=" + dataItem.uid + "]");
                    //var row = treeList.content.find("tr[data-uid=" + dataItem.uid + "]");
                    //if (typeof (row) !== "undefined" ){ treeList.select(row); }
                }           
            }
            //treeview.findByUid(getitem.uid);
            //var selectitem = treeview.findByUid(getitem.uid);
            //treeview.select(selectitem);
            //treeList.select($("#PageDetailGrid tbody>tr:nth(" + pgTreeListSelectedIndex + ")"));
            //var row = treeList.select();
            //if(typeof (row) !== "undefined" && row.length > 0){
            //      var data = treeList.dataItem(row);
            //      alert(data.PageDetName);
                  //console.log(data.name);
            //    }
            //pgTreeListSelecting = false;
        }
        //call back events
        function crPageSelectCB(results){            
            //alert('Selected Msg: ' + strMsg);
        }

        function crPageSaveCB(results){
            //alert('Save Msg: ' + strMsg);
            $("#cmPage").data("kendoDropDownList").dataSource.read();
        }

        //not used,  we still need to capture kendo window close event and trigger call back
        function crPageCloseCB(results){
            //alert('Close Msg: ' + strMsg);
            $("#cmPage").data("kendoDropDownList").dataSource.read();
        }
        
        //local click events
        function btnOpenWinEditPage_Click(){           
            if (typeof (intPageControl) != 'undefined' && intPageControl !== 0) { oCreatePageCtrl.read(intPageControl); }
        }
        
        function btnOpenWinNewPage_Click() {
            oCreatePageCtrl.show();            
        }
        
        //End Create/Edit Page Region
               
        

        //Begin Manage Page Detail Region

        var winNewPageDetail = kendo.ui.Window;

        var oPageDetail = new cmPageDetail();
        var oCMNewPageDetailCtrl = new CMPageDetailCtrl();
        var oCMEditPageDetailCtrl = new CMPageDetailCtrl();
        //call back events
        function crPageDetailSelectCB(results){
            try{
                var pageDataItems = [];
                if (typeof(results) !== 'undefined' && ngl.isObject(results) && results.source === "ContainerSelected" && typeof(results.data) !== 'undefined' && ngl.isObject(results.data)){
                    if (typeof (results.error) !== 'undefined' && ngl.isObject(results.error)) {
                        ngl.showErrMsg(results.error.name, results.error.message, null);
                    } else {
                        var conainer = ctrlSubTypes.getSubType(results.data.PageDetControl);
                        for (iDetail = 0; iDetail < ctrlSubTypes.arrSubTypes.length; iDetail++){
                            var dataitem = ctrlSubTypes.arrSubTypes[iDetail];
                            if (conainer.canContain(dataitem.GroupSubTypeControl)){ pageDataItems.push(dataitem); }
                        }
                        oCMNewPageDetailCtrl.loadDetailItemList(pageDataItems);
                    }                    
                }
            } catch (err) { ngl.showError(err); }           
        }

        function crPageDetailSaveCB(results){
            //alert('Save Msg: ' + strMsg);
            //$("#cmPage").data("kendoDropDownList").dataSource.read();
            loadPageDetails();
        }

        //not used,  we still need to capture kendo window close event and trigger call back
        function crPageDetailCloseCB(results){
            //alert('Close Msg: ' + strMsg);
            //$("#cmPage").data("kendoDropDownList").dataSource.read();
        }

        //local click events        
        
        function btnNewDetails_Click() {
            //debugger;
            try {
                var containers = [];
                var iDetail = 0;
                var dtData = pgDetailsData.data();                
                //for (iDetail = 0; iDetail < dtData.length; iDetail++){
                //    var dataitem = dtData[iDetail];
                //    if (ctrlSubTypes.isSubTypeAContainer(dataitem.PageDetGroupSubTypeControl)){ containers.push(dataitem); }
                //}         
                oCMNewPageDetailCtrl.loadContainerList(dtData,0,intPageControl);
                //var pageDataItems = [];
                //var conainer = ctrlSubTypes.getSubType( containers[0].PageDetControl);
                //for (iDetail = 0; iDetail < ctrlSubTypes.arrSubTypes.length; iDetail++){
                //    var dataitem = ctrlSubTypes.arrSubTypes[iDetail];
                //    if (conainer.canContain(dataitem.GroupSubTypeControl)){ pageDataItems.push(dataitem); }
                //}
                //oCMNewPageDetailCtrl.loadDetailItemList(pageDataItems)
                oCMNewPageDetailCtrl.show();                 
            } catch (err) { ngl.showError(err); }
        }

        function btnAddDataElements_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                if (oCMEditPageDetailCtrl.addDataElements()) { loadPageDetails(); } //reload the data elements for the page
            } else { ngl.showInfoNotification("Cannot Save Changes", "Please select an element to edit.", null); }
        }

        function btnMoveUp_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                oCMEditPageDetailCtrl.changeSequence(-1);
            } else { ngl.showInfoNotification("Cannot Move Up", "Please select an element to modify.", null); }
        }

        function btnMoveDown_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                oCMEditPageDetailCtrl.changeSequence(1);
            } else { ngl.showInfoNotification("Cannot Move Down", "Please select an element to modify.", null); }
        }

        function btnSaveDetails_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                //debugger;
                oCMEditPageDetailCtrl.save();
            } else { ngl.showInfoNotification("Cannot Save Changes", "Please select an element to edit.", null); }           
            return;
        }

        function btnDeleteDetails_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                oCMEditPageDetailCtrl.delete();
            } else { ngl.showInfoNotification("Cannot Delete Data", "Please select an element to delete.", null); }
            return;
        }

        function btnCopyDescriptionToCaption_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                oCMEditPageDetailCtrl.copyDescriptionToCaption();
            } else { ngl.showInfoNotification("Cannot Save Changes", "Please select an element to edit.", null); }           
            return;
        }

        function btnMakeEditFilter_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                oCMEditPageDetailCtrl.makeEditFilter();
            } else { ngl.showInfoNotification("Cannot Save Changes", "Please select an element to edit.", null); }           
            return;
        }

        function btnMakeReadOnlyFilter_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                oCMEditPageDetailCtrl.makeReadOnlyFilter();
            } else { ngl.showInfoNotification("Cannot Save Changes", "Please select an element to edit.", null); }           
            return;
        }

        function btnMakeHiddenReadonly_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                oCMEditPageDetailCtrl.makeHiddenReadonly();
            } else { ngl.showInfoNotification("Cannot Save Changes", "Please select an element to edit.", null); }           
            return;
        }

        function btnHideAllControls_Click(){
            if (typeof (oCMEditPageDetailCtrl) !== 'undefined' && ngl.isObject(oCMEditPageDetailCtrl)) {
                oCMEditPageDetailCtrl.hideAllControls();
            } else { ngl.showInfoNotification("Cannot Process Requests", "Please select an element and try again.", null); }           
            return;
        }

        //function btnNewDetails_Click(){
        //    return;
        //}

        //function btnNewPage_Click(){

        //}
       
        //var stypename = ctrlSubTypes.getSubTypeName(7);
        //alert(stypename)
        function cmPagechanged() {
            //alert("cmPagechanged")
            //debugger;
            var value = $("#cmPage").val();
            intPageControl = value;
            loadPageDetails();       
        }

        function loadPageDetails(){
            try{
                //iEditPageDetControl = 0;
                //debugger;
                //var treelist = $("#PageDetailGrid").data("kendoTreeList");
                pgTreeList.dataSource.read();
            } catch (err) { ngl.showError(err); }
            //$('#PageDetailGrid').kendoGrid({ 
            //    height: 500,
            //    dataSource: pgDetailsData, 
            //    columns: [
            //        //{field: "ROW_NUMBER", title: "ROW_NUMBER"},
            //        {field: "PageDetPageControl", title: "P-Control"},                    
            //        {field: "PageDetControl", title: "Control"},  
            //        {field: "PageDetGroupSubTypeControl", title: "Type", template: "# ctrlSubTypes.getSubTypeName(PageDetGroupSubTypeControl) #"},
            //        {field: "PageDetParentID", title: "Parent"},
            //        {field: "PageDetSequenceNo", title: "Sequence"},
            //        {field: "PageDetName", title: "Name"}                   
            //    ]
            //});
        }
                               
        //function lvItemChanged(selectedItem){   
        //    debugger;
        //    alert(selectedItem.PageDetNme);
        //}

        function pgDetailSelected() {
            if ( pgTreeListSelecting == true) { return; }
            //debugger;
            //console.log('pgDetailSelected');
            var data = pgDetailsData.view(),
            selectedItem = $.map(this.select(), function (item) {
                return data[$(item).index()];
            });
            //console.log(selectedItem[0].PageDetName);
            if (selectedItem == null || selectedItem.length < 1 ){
                try{
                    //debugger;
                    $("#PageDetailGrid").data("kendoTreeList").select(0);
                    selectedItem = $.map(this.select(), function (item) {
                        return data[$(item).index()];
                    });
                    //console.log(selectedItem[0].PageDetName);
                }
                catch(err) { return; }
            }
            if (selectedItem != null && selectedItem.length > 0 ){
                //debugger;
                pgTreeListSelectedIndex =  this.select().index(); //selectedItem[0].index;
                //check if the current page detail is already selected and exit if it has not changed
                if (iEditPageDetControl != 0 && iEditPageDetControl === selectedItem[0].PageDetControl) { console.log("page detail already selected, exiting"); return; }
                iEditPageDetControl = selectedItem[0].PageDetControl;
                var sType = null;
                if (typeof (ctrlSubTypes) != 'undefined' && ctrlSubTypes != null) { sType = ctrlSubTypes.getSubType(selectedItem[0].PageDetGroupSubTypeControl); }
                oCMEditPageDetailCtrl = new CMPageDetailCtrl();                
                oCMEditPageDetailCtrl.loadDefaults(null,crPageDetailSelectCB,crPageDetailSaveCB,crPageDetailCloseCB,null);
                var dtData = pgDetailsData.data(); 
                console.log('editing selected item');
                oCMEditPageDetailCtrl.edit(selectedItem[0]); 
                //Load the container data using the current ParentID as the default for the first dropdown list
                //if (ngl.intTryParse(selectedItem[0].PageDetParentID,0) == 0) {selectedItem[0].PageDetParentID = intPageControl;}
                oCMEditPageDetailCtrl.loadContainerList(dtData,selectedItem[0].PageDetParentID,intPageControl);                             
            }                   
        }

        //End Manage Page Detail Region
    

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            var sTypes = ctrlSubTypes.loadDefaults();
            oCreatePageCtrl.loadDefaults(winNewPage,crPageSelectCB,crPageSaveCB,crPageCloseCB,null);
            oCMNewPageDetailCtrl.loadDefaults(winNewPageDetail,crPageDetailSelectCB,crPageDetailSaveCB,crPageDetailCloseCB,null);
            //winNewPage = $("#winNewPage").kendoWindow({
            //    title: "Create New Page",
            //    modal: true,
            //    visible: false              
            //}).data("kendoWindow");         
            $("#txtPageDetName").kendoMaskedTextBox();
            $("#txtPageDetName").data("kendoMaskedTextBox").readonly(); //set to readonly
            $("#txtPageDetDesc").kendoMaskedTextBox();
            $("#txtPageDetCaption").kendoMaskedTextBox();
            $("#txtPageDetCaptionLocal").kendoMaskedTextBox();
            $("#txtPageDetSequenceNo").kendoNumericTextBox({format: "0"});
            $("#txtPageDetEditWndSeqNo").kendoNumericTextBox({format: "0"}); //Added by LVV on 7/26/19
            $("#txtPageDetAddWndSeqNo").kendoNumericTextBox({format: "0"}); //Added by LVV on 7/26/19
            $("#txtPageDetWidth").kendoNumericTextBox({format: "0"});            
            $("#txtPageDetMetaData").kendoMaskedTextBox();
            //$("#txtPageDetMetaData").kendoEditor({ 
            //    resizable: {
            //        content: true,
            //        toolbar: true
            //    },
            //    // Empty tools so do not display toolbar
            //    tools: []
            //});
            $("#txtPageDetPageTemplateControl").kendoNumericTextBox();
            $("#txtPageDetFieldFormatOverride").kendoMaskedTextBox();
            $("#txtPageDetCSSClass").kendoMaskedTextBox();
            $("#txtPageDetAttributes").kendoMaskedTextBox();
            $("#txtPageDetAPIReference").kendoMaskedTextBox();
            $("#txtPageDetAPIFilterID").kendoMaskedTextBox();
            $("#txtPageDetAPISortKey").kendoMaskedTextBox();
                     
            var PageReadyJS = <%=PageReadyJS%>;

            cmPagesDataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/vLookupList/GetUserDynamicList/" + cmPageKey,
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
                            Description: { editable: false }
                        }
                    },
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get User List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });

            var cmPageList = $("#cmPage").kendoDropDownList({
                dataTextField: "Name",
                //dataTextField: "Description",
                dataValueField: "Control",
                dataSource: cmPagesDataSource,
                change: cmPagechanged,
                dataBound: cmPagechanged,
            });

            pgDetailsData = new kendo.data.TreeListDataSource({  
                transport: { 
                    //read: {                            
                    //    url: "api/cmPageDetail/" + intPageControl, 
                    //    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    //    type: "GET"
                    //},   
                    read: function(options) {
                        $.ajax({
                            url: 'api/cmPageDetail/' + intPageControl,
                            contentType: 'application/json; charset=utf-8',
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function(data) {
                                options.success(data);
                                if (typeof (data) !== 'undefined' && ngl.isObject(data) && typeof (data.Errors) !== 'undefined' &&  data.Errors != null) { 
                                    ngl.showErrMsg('Access Denied', data.Errors, null); 
                                } else {
                                    pgTreeListSelect();
                                    //if (typeof (pgTreeList) !== "undefined" && pgTreeListSelectedIndex != null && pgTreeListSelectedIndex > 0){                                            
                                    //    pgTreeList.select($("#treeList tbody>tr:nth(" + pgTreeListSelectedIndex + ")"));
                                    //     debugger;
                                    //    var row = pgTreeList.select();
                                    //    if(typeof (row) !== "undefined" && row.length > 0){
                                    //      var data = pgTreeList.dataItem(row);
                                    //      alert(data.PageDetName);
                                    //    }
                                    //}
                                }
                            },
                            error: function(result) { options.error(result); }
                        });
                    },                 
                    parameterMap: function (options, operation) { return options; }
                }, 
                schema: { 
                    data: "Data",  
                    total: "Count", 
                    model: { 
                        id: "PageDetControl",
                        fields: {
                            parentId:{ type: "number" ,  nullable: true},
                            PageDetControl: { type: "number" },
                            PageDetPageControl: { type: "number" },
                            PageDetGroupTypeControl: { type: "number" },
                            PageDetGroupSubTypeControl: { type: "number" },
                            PageDetName: { type: "string" },
                            PageDetDesc: { type: "string" },
                            PageDetCaption: { type: "string" },
                            PageDetCaptionLocal: { type: "string" },  
                            PageDetSequenceNo: { type: "number" },                           
                            PageDetEditWndSeqNo: { type: "number" }, //Added by LVV on 7/26/19
                            PageDetAddWndSeqNo: { type: "number" }, //Added by LVV on 7/26/19
                            PageDetParentID: { type: "number" } ,
                            PageDetOrientation: {type: "number"} ,
                            PageDetWidth: {type: "number"} ,
                            PageDetAllowFilter: {type: "bool"}, 
                            PageDetFilterTypeControl: {type: "number"} ,
                            PageDetAllowSort: {type: "bool"} ,
                            PageDetAllowPaging: {type: "bool"} ,
                            PageDetUserSecurityControl: {type: "number"} ,
                            PageDetVisible: {type: "bool"} ,
                            PageDetEditWndVisibility: {type: "bool"}, //Added by LVV on 7/26/19
                            PageDetAddWndVisibility: {type: "bool"}, //Added by LVV on 7/26/19
                            PageDetReadOnly: {type: "bool"} ,
                            PageDetDataElmtControl: {type: "number"} ,
                            PageDetElmtFieldControl: {type: "number"} ,
                            PageDetPageTemplateControl: {type: "number"} ,
                            PageDetExpanded: {type: "bool"} ,
                            PageDetMetaData: { type: "string" } ,
                            PageDetFKReference: {type: "string"} ,
                            PageDetTagIDReference: { type: "string" } ,
                            PageDetCSSClass: { type: "string" } ,
                            PageDetAttributes: { type: "string" } ,
                            PageDetAPIReference: { type: "string" } ,
                            PageDetAPIFilterID: { type: "string" } ,
                            PageDetAPISortKey: { type: "string" }                       
                        }
                    }, 
                    errors: "Errors" 
                }, 
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Page Detail Failed",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }              
            });

            pgTreeList = $("#PageDetailGrid").kendoTreeList({
                dataSource: pgDetailsData,
                selectable: "single", 
                change: pgDetailSelected, 
                scrollable: false,
                columns: [
                    { field: "PageDetName", title: "Name" },
                    { field: "PageDetSequenceNo", title: "Sort", width: 30 }
                ],
                autoBind: false
            }).data("kendoTreeList");

            loadPageDetails();
                           
            var divWait = $("#h1Wait");               
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }           
       });

    </script>
    
    </div>

     <style>
      .k-button{
        white-space: normal;
      }
    </style>

</body>
</html>
