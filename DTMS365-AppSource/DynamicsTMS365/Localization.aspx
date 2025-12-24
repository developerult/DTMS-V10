<%@ Page Title="Language Localization Reference" Language="C#" AutoEventWireup="true" CodeBehind="Localization.aspx.cs" Inherits="DynamicsTMS365.Localization" %>

<%--Created By RHR on 01/07/17 for v-8.0 Language Localization Reference--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
        <title>DTMS Language Localization Reference</title>         
       
         <%=cssReference%>              
          <style>
            html,
body{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
        </style>
    </head>
    <body>       
          <%=jssplitter2Scripts%>
        <%=sWaitMessage%>     

      <div id="example" style="height: 99%; width: 99%;  margin-top: 2px;">
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
                            <% Response.Write(FastTabsHTML); %>    

                        </div>
                      </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content" style="margin:10px;">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
             </div>
       
    <script>
         <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var tObj = this;
        var tPage = this;           
       

        <% Response.Write(NGLOAuth2); %>

        
        var UserToken = '<%=UserToken%>'; 
       <% Response.Write(PageCustomJS); %>
        var carrierequipcodeKey = 0;
        var cmPageKey = 31;
        var GetUserDynamicList
        var GroupDataSource = kendo.data.DataSource;
        var CarEquipDataSource = kendo.data.DataSource;
        var cmPageDataSource = kendo.data.DataSource;
        function grouptypechanged(){
            var value = $("#grouptype").val();
            alert(value);
        }
        function carequipchanged() {
            var value = $("#carrierequipcode").val();
            alert(value);
        }

        function cmPagechanged() {
            var value = $("#cmPage").val();
            alert(value);
        }

        $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
            //everntaully this will only say this
            var PageReadyJS = <%=PageReadyJS%>; 
           
            GroupDataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/vLookupList/GetStaticList/"  +  nglStaticLists.cmGroupType,
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
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            })

            CarEquipDataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/vLookupList/GetGlobalDynamicList/" + carrierequipcodeKey,
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
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            })

            
            cmPageDataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/vLookupList/GetUserDynamicList/" + cmPageKey ,
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
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            })
           var cmGroupTypeList = $("#grouptype").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                dataSource: GroupDataSource,
                change: grouptypechanged
           });

           var CarEquipList = $("#carrierequipcode").kendoDropDownList({
               dataTextField: "Name",
               dataValueField: "Control",
               dataSource: CarEquipDataSource,
               change: carequipchanged
           });
           
           var cmPageList = $("#cmPage").kendoDropDownList({
               dataTextField: "Name",
               dataValueField: "Control",
               dataSource: cmPageDataSource,
               change: cmPagechanged
           });

            
           menuTreeHighlightPage(); //must be called after PageReadyJS
           var divWait = $("#h1Wait");
           if  (typeof (divWait) !== 'undefined' ) {
               divWait.hide();
           }


        });


    </script>
    
    </div>


    </body>
</html>
