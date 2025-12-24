<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Parameters.aspx.cs" Inherits="DynamicsTMS365.Parameters" %>

<!DOCTYPE html>
<html>
    <head >
        <title>DTMS Home</title>  
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
                <div id="menu-pane" style="height: 100%; width: 100%;">
                    <div id="tab" class="menuBarTab"></div>
                </div>
                <div id="top-pane">
                  <div id="horizontal" style="height: 100%; width: 100%;">                        
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
                    <div class="pane-content" style="margin:10px;">
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
        var tObj = this;
        var tPage = this;           
        

        <% Response.Write(NGLOAuth2); %>

        
       
        <% Response.Write(PageCustomJS); %>
       
        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            

            //var Parameter = new kendo.data.DataSource();
            
            var PageReadyJS = <%=PageReadyJS%>; 
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }

            //    Parameter = new kendo.data.DataSource({ 
            //        transport: { 
            //            read: function(options) {
            //                var s = new AllFilter();
            //                s.filterName = $("#ddlParGridFilters").data("kendoDropDownList").value();
            //                s.filterValue = $("#txtParGridFilterVal").data("kendoMaskedTextBox").value();
            //                s.filterFrom = $("#dpParGridFilterFrom").data("kendoDatePicker").value();
            //                s.filterTo = $("#dpParGridFilterTo").data("kendoDatePicker").value();
            //                s.sortName = $("#txtParGridSortField").val();
            //                s.sortDirection = $("#txtParGridSortDirection").val();
            //                s.page = options.data.page;
            //                s.skip = options.data.skip;
            //                s.take = options.data.take; 
            //                $.ajax({
            //                    url: "api/Parameter/GetRecords/" + s, 
            //                    contentType: 'application/json; charset=utf-8',
            //                    dataType: 'json',
            //                    data: { filter: JSON.stringify(s) },
            //                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            //                    success: function(data) {
            //                        // notify the data source that the request succeeded
            //                        options.success(data);
            //                        if (data.Errors != null) {
            //                            if (data.StatusCode === 203) {
            //                                showErrorNotification("Authorization Timeout", data.Errors);
            //                            }
            //                            else {
            //                                showErrorNotification("Access Denied", data.Errors);
            //                            }               
            //                        }
            //                    },
            //                    error: function(result) {
            //                        // notify the data source that the request failed
            //                        options.error(result);
            //                    }
            //                });
            //            },                   
            //            parameterMap: function (options, operation) { return options; } 
            //        }, 
            //        schema: {  
            //            data: "Data",  
            //            total: "Count", 
            //            model: {
            //                id: "ParKey",
            //                fields: {
            //                    ParKey: { type: "string" },
            //                    ParText: { type: "string" },
            //                    ParDescription: { type: "string" },
            //                    ParCategoryControl: { type: "number" },
            //                    ParIsGlobal: {type: "bool"} 
            //                }
            //            },
            //            errors: "Errors" 
            //        },
            //        error: function (xhr, textStatus, error) { 
            //            showErrorNotification("Get Parameter Data Failed",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"));  this.cancelChanges(); 
            //        },
            //        pageSize: 10,
            //        serverPaging: true,
            //        sortable: true,
            //        pageable: true,
            //        groupable:  true});                  
            //    $('#ParGrid').kendoGrid({ 
            //        theme: "blueopal", 
            //        dataSource: Parameter, 
            //        height: 400, 
            //        pageable: true, 
            //        sortable: true, 
            //        resizable: true, 
            //        groupable: true, 
            //        columns: [
            //            {field: "ParKey"},
            //            {field: "ParText"},
            //            {field: "ParDescription"},
            //            {field: "ParCategoryControl"},
            //            {field: "ParIsGlobal"},
            //        ]});
                   
        });
    </script>     
      </div>
    </body>
</html>
