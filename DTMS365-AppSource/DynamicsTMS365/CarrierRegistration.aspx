<%@ Page Title="Carrier Registration Page" Language="C#"  AutoEventWireup="true" CodeBehind="CarrierRegistration.aspx.cs" Inherits="DynamicsTMS365.CarrierRegistration" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Carrier Registration</title>
       <%=cssReference%>              
        <style>

html,

body
{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

</style>
                 
    </head>
     <body>       
        <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>  
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/splitter2.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script> 


      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%;"> 
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
                            <div id="pageContent" class="pane-content">


                                <div class="demo-section k-content" style="text-align: center;">

                                    <h4>Show notification:</h4>
                                    <p>
                                        <button id="showValidationNotification" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md">Validation</button><br />
                                        <button id="showErrorNotification" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md">Error</button><br />
                                        <button id="showSuccessNotification" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md">Success</button>
                                        <button id="showWarningNotification" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md">Warning</button><br />
                                        <button id="showInfoNotification" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md">Info</button><br />
                                    </p>
                                    <h4>Hide notification:</h4>
                                    <p>
                                        <button id="hideAllNotifications" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md">Hide All Notifications</button>
                                    </p>

                                </div>


                            </div>                          
                        </div>
  
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>




    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %> 
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>         
    <script>
        
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var tObj = this;
        var tPage = this;  

        <% Response.Write(NGLOAuth2); %>


         <% Response.Write(PageCustomJS); %>
        function execActionClick(btn, proc){
            
        }



        $(document).ready(function () {  
            var PageMenuTab = <%=PageMenuTab%>;
            
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS

            if (control != 0){


                $("#showValidationNotification").click(function(){
                    ngl.showValidationMsg("Required Fields", "Zip Code, State, Country", null);
                });

                $("#showErrorNotification").click(function(){
                    ngl.showErrMsg("Wrong Password", "Please enter your password again.", null);
                });

                $("#showSuccessNotification").click(function(){
                    ngl.showSuccessMsg("Success!", null);
                });

                $("#showWarningNotification").click(function(){
                    ngl.showWarningMsg("Warning", "Free Trial ends in 5 days.", null);
                });

                $("#showInfoNotification").click(function(){
                    ngl.showInfoNotification("Info", "Truck related information.", null);
                });

                $("#hideAllNotifications").click(function(){
                    notification.hide();
                });

                $(document).one("kendo:pageUnload", function(){ if (notification) { notification.hide(); } });
                //** NOTIFICATION CODE END **


/////////////////////////////////////////////////////////////////////////
                
                vSettlementGrid365 = new kendo.data.DataSource({
                    serverSorting: true, 
                    serverPaging: true, 
                    pageSize: 10,
                    transport: { 
                        read: function(options) { 

                            var s = new AllFilter();

                            s.filterName = $("#ddlSettlementGridFilters").data("kendoDropDownList").value();
                            s.filterValue = $("#txtSettlementGridFilterVal").data("kendoMaskedTextBox").value();
                            s.filterFrom = $("#dpSettlementGridFilterFrom").data("kendoDatePicker").value();
                            s.filterTo = $("#dpSettlementGridFilterTo").data("kendoDatePicker").value();
                            s.sortName = $("#txtSettlementGridSortField").val();s.sortDirection = $("#txtSettlementGridSortDirection").val();
                            s.page = options.data.page;
                            s.skip = options.data.skip;
                            s.take = options.data.take;

                            $.ajax({ 

                                url: 'api/Settlement/GetRecords/' + s, 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) { 
                                    options.success(data); 

                                    if (data.Errors != null) { 

                                        if (data.StatusCode === 203){ 
                                            ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                        } 
                                        else 
                                        { 
                                            ngl.showErrMsg("Access Denied", data.Errors, null); 
                                        } 
                                    } 
                                }, 

                                error: function(result) { 
                                    options.error(result); 
                                } 
                            }); 

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
                                CnsNumber: { type: "string" },
                                ProNumber: { type: "string" },
                                OrderNumber: { type: "string" },
                                PickupName: { type: "string" },
                                DestinationName: { type: "string" },
                                Status: { type: "string" },
                                DeliveredDate: { type: "date" },
                                ContractedCost: { type: "number" },
                                InvoiceNumber: { type: "string" },
                                InvoiceAmount: { type: "number" },
                                SHID: { type: "string" },
                                CarrierPro: { type: "string" },
                                BookFinAPActWgt: { type: "number" },
                                BookCarrBLNumber: { type: "string" }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function(xhr, textStatus, error) {
                        ngl.showErrMsg("Access vSettlementGrid365 Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                    }
                });




                $('#SettlementGrid').kendoGrid({
                    theme: "blueopal",
                    dataSource: vSettlementGrid365,
                    pageable: true,
                    sortable: true,
                    resizable: true,
                    groupable: true, 
                    columns: [
                        {field: "Control", title: "Control", hidden: true },
                        {field: "CnsNumber", title: "CNS"},
                        {field: "ProNumber", title: "Pro"},
                        {field: "OrderNumber", title: "Order No"},
                        {field: "PickupName", title: "Pickup Name"},
                        {field: "DestinationName", title: "Destination"},
                        {field: "Status", title: "Status"},
                        {field: "DeliveredDate", title: "Delivered Date"},
                        {field: "ContractedCost", title: "Contracted Cost"},
                        {field: "InvoiceNumber", title: "Invoice Number"},
                        {field: "InvoiceAmount", title: "Invoice Amount"},
                        {field: "SHID", title: "SHID"},
                        {field: "CarrierPro", title: "Carrier Pro"},
                        {field: "BookFinAPActWgt", title: "Billed Weight"},
                        {field: "BookCarrBLNumber", title: "BL Number"}]

                });
/////////////////////////////////////////////////////////////////////////






            
            }

        });


    </script>
          <style>
                .demo-section p {
                    margin: 3px 0 10px;
                    line-height: 50px;
                }
                .demo-section .k-button {
                    width: 250px;
                }

            </style>         
   
    </div>


    </body>

</html>