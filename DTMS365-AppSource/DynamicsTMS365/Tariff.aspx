<%@ Page Title="Tariff Page" Language="C#"  AutoEventWireup="true" CodeBehind="Tariff.aspx.cs" Inherits="DynamicsTMS365.Tariff" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Tariff</title>         
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
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>       
    <script>
       
        <% Response.Write(ADALPropertiesjs); %>
         var PageControl = '<%=PageControl%>'; 
         var oTariffGrid = null;
        var wnd = kendo.ui.Window; 
        var tObj = this;
        var tPage = this;    

        <% Response.Write(NGLOAuth2); %>

        var iContractPK = 0;
        <% Response.Write(PageCustomJS); %>

        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingSuccessCallbackAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
        }


        function saveTariffPK() {
            try {
                var row = oTariffGrid.select();
                if (typeof (row) === 'undefined' || row == null) {              
                    ngl.showValidationMsg("Contract Required", "Please select a Contract to continue", tPage);
                    return false;
                }               
                //Get the dataItem for the selected row
                var dataItem = oTariffGrid.dataItem(row);
                if (typeof (dataItem) === 'undefined' || dataItem == null) {              
                    ngl.showValidationMsg("Contract Required", "Please select a Contract to continue", tPage);
                    return false;
                }  
                if ("CarrTarControl" in dataItem){
                
                    iContractPK = dataItem.CarrTarControl;
                    var setting = {name:'pk', value: iContractPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("Tariff/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingSuccessCallbackAjaxErrorCallback",tPage);
                    return true;
                } else {
                    ngl.showValidationMsg("Contract Required", "Invlaid Record Identifier, please select a Contract to continue", tPage);
                    return false;
                }

            } catch (err) {
                ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen.  If this continues please contact technical support. Error: " + err.message, tPage);
            }
            
        }
        function isContractSelected() {
            if (typeof (iContractPK) === 'undefined' || iContractPK === null || iContractPK === 0) {
                return saveTariffPK();
            }
            return true;
        }
        function execActionClick(btn, proc){            
            if(btn.id == "btnAddContract"){             
                    openAddNewTariffGridWindow();
            } else if(btn.id == "btnOpenServices"){               
                if (isContractSelected() === true) { location.href = "TariffServices"; }
                //1. get CarTarControl from selected grid record
                //2. update user page setting with selected record data for the tariff page
                //3. on response from ajax load services page
                //4. In the Services Page Controller get method read the CarrTarControl user page settings for the tariff page, set this as the parent control number
                //5. In the tariff controller read summary data read the CarrTarControl user page settings for the tariff page, use this to query the data
                //6. in the tariff controller when a filter is applied save the filter to the user settings for the page
                //7. in the tariff controller when a read is triggered with an empty filter read the filter settings from the user settings page
                //   if no records default to empty filter
            } else  if(btn.id == "btnOpenRates"){
                if (isContractSelected() === true) { location.href = "TariffRates";}
            } else if(btn.id == "btnOpenExceptions"){
                if (isContractSelected() === true) { location.href = "TariffExceptions";}
            } else if(btn.id == "btnOpenFees"){
                if (isContractSelected() === true) { location.href = "TariffFees";}
            } else  if(btn.id == "btnOpenFuel"){
                if (isContractSelected() === true) { location.href = "TariffFuel";}
            } else if(btn.id == "btnOpenNoDrive"){
                if (isContractSelected() === true) { location.href = "TariffNoDriveDays";}
            } else if(btn.id == "btnOpenHDMs"){
                if (isContractSelected() === true) { location.href = "TariffHDMs";}
            } else if(btn.id == "btnCopyContract"){
                alert('Copy Contract coming soon');
            } else if(btn.id == "btnImportTariff"){
                alert('Import coming soon');
            } else if(btn.id == "btnExportTariff"){
                //alert('Export coming soon');
                if (isContractSelected() === true) { exportToExcel(); }
            } else if (btn.id == "btnRefresh" ){ 
                oTariffGrid.dataSource.read();
            } else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }
        var blnTariffGridChangeBound = false;
        //*************  Call Back Functions ****************
        function TariffGridDataBoundCallBack(e,tGrid){           
            oTariffGrid = tGrid;
            if (blnTariffGridChangeBound == false){
                oTariffGrid.bind("change", saveTariffPK);
                blnTariffGridChangeBound = true;
            }
           

        }


        //*************  Tariff Export Functions ****************
        function exportToExcel(){

            var ds = new kendo.data.DataSource({
                type: "odata",
                transport: {
                    read: {
                        url: "api/TariffExport/Get/" + iContractPK,
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
                        fields: {
                            Country: { type: "string" },
                            State: { type: "string" },
                            City: { type: "string" },
                            FromZip: { type: "string" },
                            ToZip: { type: "string" },
                            Lane: { type: "number" },
                            TariffClass: { type: "string" },
                            Min: { type: "number" },                               
                            MaxDays: { type: "number" },
                            Val1: { type: "number" },
                            Val2: { type: "number" },
                            Val3: { type: "number" },
                            Val4: { type: "number" },
                            Val5: { type: "number" },
                            Val6: { type: "number" },
                            Val7: { type: "number" },
                            Val8: { type: "number" },
                            Val9: { type: "number" },
                            Val10: { type: "number" }
                        }
                    },
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Tariff Excel Export JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
            });

            var rows = [{
                cells: [
                  { value: "Country" },
                  { value: "State" },
                  { value: "City" },
                  { value: "FromZip" },
                  { value: "ToZip" },
                  { value: "Lane" },
                  { value: "TariffClass" },
                  { value: "Min" },
                  { value: "MaxDays" },
                  { value: "Val1" },
                  { value: "Val2" },
                  { value: "Val3" },
                  { value: "Val4" },
                  { value: "Val5" },
                  { value: "Val6" },
                  { value: "Val7" },
                  { value: "Val8" },
                  { value: "Val9" },
                  { value: "Val10" }
                ]
            }];

            //using fetch, so we can process the data when the request is successfully completed
            ds.fetch(function(){
                var data = this.data();
                for (var i = 0; i < data.length; i++){
                    //push single row for every record
                    rows.push({
                        cells: [
                          { value: data[i].Country },
                          { value: data[i].State },
                          { value: data[i].City },
                          { value: data[i].FromZip },
                          { value: data[i].ToZip },
                          { value: data[i].Lane },
                          { value: data[i].TariffClass },
                          { value: data[i].Min },
                          { value: data[i].MaxDays },
                          { value: data[i].Val1 },
                          { value: data[i].Val2 },
                          { value: data[i].Val3 },
                          { value: data[i].Val4 },
                          { value: data[i].Val5 },
                          { value: data[i].Val6 },
                          { value: data[i].Val7 },
                          { value: data[i].Val8 },
                          { value: data[i].Val9 },
                          { value: data[i].Val10 }
                        ]
                    })
                }
                var workbook = new kendo.ooxml.Workbook({
                    sheets: [
                      {
                          columns: [
                            // Column settings (width)
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true }

                          ],
                          // Title of the sheet
                          title: "Tariffs",
                          // Rows of the sheet
                          rows: rows
                      }
                    ]
                });
                //save the file as Excel file with extension xlsx
                kendo.saveAs({dataURI: workbook.toDataURLAsync(), fileName: "TestTariffExport.xlsx"});
            });

        }

        $(document).ready(function () {
           
            var PageMenuTab = <%=PageMenuTab%>;
            
            if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                    
                //}, 1,this);

            }
            //setTimeout(function () {, 0,this);
           var PageReadyJS = <%=PageReadyJS%>
            //setTimeout(function () {   
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if  (typeof (divWait) !== 'undefined' ) {
                    divWait.hide();
                }
            //}, 0, this);


            
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