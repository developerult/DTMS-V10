<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessorialMaint.aspx.cs" Inherits="DynamicsTMS365.AccessorialMaint" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Accessorial Maint</title>         
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

                    
          <div id="wndEditMFXG">
              <div id="lblNACInfo" style="margin-left: 5px; margin-bottom:15px;"></div>           
              <span><input id="ddlAccessorial" style="min-width:200px; width:100%;"/></span>                             
              <input id="txtNACCode" type="hidden" /><input id="txtNACDesc" type="hidden" />   
              <input id="txtNACControl" type="hidden" /> 
          </div>

     <% Response.Write(PageTemplates); %>

    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>       
    <script>
       
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var tPage = this;
        var tObjPG = this;   

        <% Response.Write(NGLOAuth2); %>
        
        var oAccessorialGrid = null;
        var oMapFeeXrefGrid = null;

        <% Response.Write(PageCustomJS); %>

        var wndEditMFXG = kendo.ui.Window;


        //************* Action Menu Functions ***************
        function execActionClick(btn, proc){   
            if(btn.id === "btnMapFeeXref"){                                  
                if (typeof (tPage["wdgtMapFeeXrefWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtMapFeeXrefWndDialog"])){
                    tPage["wdgtMapFeeXrefWndDialog"].show();                
                } else{alert("Missing HTML Element (wdgtMapFeeXrefWndDialog is undefined)");} //Add better error handling here if cm stuff is missing                 
            }
            else if (btn.id == "btnRefresh" ){ refresh(); }  
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        //*************  Call Back Functions ****************
        function AccessorialGridDataBoundCallBack(e,tGrid){ oAccessorialGrid = tGrid; }

        function MapFeeXrefGridDataBoundCallBack(e,tGrid){ oMapFeeXrefGrid = tGrid; }

        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
        }

        function refresh() {    
            //Modified by RHR for v-8.2 on 02/20/2019
            ngl.readDataSource(oAccessorialGrid);
            ngl.readDataSource(oMapFeeXrefGrid);
            ngl.readDataSource($('#ddlAccessorial').data('kendoDropDownList'));          
        }

        function openMapFeeXrefEditWnd(e) {    
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            if (typeof (item) !== 'undefined' && item != null) {                    
                if (('NACControl' in item) && ('NACCode' in item) && ('NACDesc' in item) && ('AccessorialCode' in item)) {    
                    $("#txtNACControl").val(item.NACControl);
                    $("#txtNACCode").val(item.NACCode);
                    $("#txtNACDesc").val(item.NACDesc);
                    var dropdownlist = $("#ddlAccessorial").data("kendoDropDownList");
                    dropdownlist.select(function(dataItem) { return dataItem.Control === item.AccessorialCode; });
                    $("#lblNACInfo").html("<strong>" + item.NACDesc + " (" + item.NACCode + ")</strong>");
                } else { ngl.showErrMsg("", "There was a problem getting the properties from the Row object.", null); return; }               
            } else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; } 
            wndEditMFXG.center().open();
        }

        function SaveMapFeeXref() {    
            var item = new GenericResult();
            var dataItem = $("#ddlAccessorial").data("kendoDropDownList").dataItem();
            item.Control = dataItem.Control;
            item.intField1 = $("#txtNACControl").val();
            ////item.strField1 = $("#txtNACCode").val(); 
            ////item.strField2 = $("#txtNACDesc").val();          
           
            var tObj = tObjPG;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("APIFeesXref/SaveP44NGLFeeMapXref", item, tObj, "SaveP44NGLFeeMapXrefSuccessCallback", "SaveP44NGLFeeMapXrefAjaxErrorCallback");
        }

        function SaveP44NGLFeeMapXrefSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "SaveP44NGLFeeMapXrefSuccessCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        oResults.error = new Error();
                        oResults.error.name = "SaveP44NGLFeeMapXref Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } catch (err) {
                oResults.error = err
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }    
            //Modified by RHR for v-8.2 on 02/20/2019
            ngl.readDataSource(oMapFeeXrefGrid);
            ngl.readDataSource($('#ddlAccessorial').data('kendoDropDownList'));
            wndEditMFXG.close();
        }
        function SaveP44NGLFeeMapXrefAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "SaveP44NGLFeeMapXrefAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "SaveP44NGLFeeMapXref Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            //Modified by RHR for v-8.2 on 02/20/2019
            ngl.readDataSource(oMapFeeXrefGrid);
            ngl.readDataSource($('#ddlAccessorial').data('kendoDropDownList'));
            wndEditMFXG.close();
        }

       


        $(document).ready(function () {          
            var PageMenuTab = <%=PageMenuTab%>;
           
            if (control != 0){

                $("#ddlAccessorial").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: {
                        serverSorting: true, 
                        serverPaging: true, 
                        pageSize: 10,
                        transport: { 
                            read: {
                                url: "api/vLookupList/GetGlobalDynamicList/" + nglGlobalDynamicLists.Accessorials,
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            },           
                            parameterMap: function(options, operation) { return options; } 
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
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Accessorials JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                    },
                    optionLabel: {
                        Name: "None",
                        Control: 0
                    }
                });

                ////////////////wndEditMFXG///////////////
                wndEditMFXG = $("#wndEditMFXG").kendoWindow({
                    title: "Edit",
                    modal: true,
                    visible: false,
                    width: 300,
                    height: 100,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                }).data("kendoWindow");             
                $("#wndEditMFXG").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveMapFeeXref(); });

            }
           
            var PageReadyJS = <%=PageReadyJS%>               
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");               
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }
            
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
