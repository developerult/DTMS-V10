<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NatFuelZones.aspx.cs" Inherits="DynamicsTMS365.NatFuelZones" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Nat Fuel Zones</title>
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
                var tObj = this;
                var tPage = this; 
                var tObjPG = this;           
       

        <% Response.Write(NGLOAuth2); %>

        
                var ovNatFuelCrossTab = null;
                <% Response.Write(PageCustomJS); %>

                //************* Action Menu Functions ********************
                function execActionClick(btn, proc){
                   
                    if (btn.id == "btnRefresh" ){                 
                        ngl.readDataSource(ovNatFuelCrossTab);
                    }  
                    if(btn.id == "btnResetCurrentUserConfig"){
                        resetCurrentUserConfig(PageControl);
                    }            
                }

               
                //************* Call Back Functions **********************        
                function savePostPageSettingSuccessCallback(results){
                    //for now do nothing when we save the pk
                }
                function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
                    //for now do nothing when we save the pk           
                }      
                //************* Call Back Functions **********************
                function vNatFuelCrossTabGridDataBoundCallBack(e,tGrid){           
                    ovNatFuelCrossTab = tGrid;
                   
                    } 
             

                //************* Page Functions ***************************
                
                function DeleteCarrierFuelMaint(e){
                    //debugger;
                    var item = this.dataItem($(e.currentTarget).closest("tr"));
                   // wdgtLaneEA.delete(item); 
                }
                $(function () {
                    //wire focus of all numerictextbox widgets on the page
                    $("input[type=number]").bind("focus", function () {
                        var input = $(this);
                        clearTimeout(input.data("selectTimeId")); //stop started time out if any
                        var selectTimeId = setTimeout(function(){ input.select(); });
                        input.data("selectTimeId", selectTimeId);
                    }).blur(function(e) {
                        clearTimeout($(this).data("selectTimeId")); //stop started timeout
                    });
                });
                var elem = document.querySelectorAll('.k-icon.k-i-save');
                console.log(elem);
               // elem[1].click();
                //$(".k-icon.k-i-save").click(function (e) {
                //    debugger;
                //    alert("Hi");
                //    var alertMsg = "";
                //    alertMsg = validateRequiredFields();
                //    if (!ngl.isNullOrWhitespace(alertMsg)){
                //        //kendo.ui.progress($(document.body), false);
                //        ngl.showErrMsg("Required Fields", alertMsg, null);
                //        return;
                //    }
                //});
                function validateRequiredFields(){
                    var fields = "";
                    var strSp = "";
                    var intTemp = 0;
                    //An associated company is required
                    if(!document.getElementById('id10612020082555234667768').val()==null && document.getElementById('id10612020082555234667768').val()=='undefined'){
                        fields = "Date req.";
                        return fields;
                    }                  
                    //Legal Entity required fields
                    if (ngl.isNullOrWhitespace($("#id10612020082555234666768").data("kendodatepicker").value())){ fields += (strSp + "National Avg"); strSp = ", "; }
                    //if (ngl.isNullOrWhitespace($("#txtCNSPrefix").data("kendoMaskedTextBox").value())){ fields += (strSp + "CNS Prefix"); strSp = ", "; }
                    intTemp = $("#id10612020082555234666768").data("kendoNumericTextBox").value()
                    if (typeof (intTemp) === 'undefined' || intTemp === null || intTemp < 0 ){ fields += (strSp + "National Avg"); strSp = ", "; }
                    return fields;
                }
                $(document).ready(function () {
                    var PageMenuTab = <%=PageMenuTab%>;
                                    
                    if (control != 0){        
            
                    }
                    var PageReadyJS = <%=PageReadyJS%>;
                    menuTreeHighlightPage(); //must be called after PageReadyJS
                    var divWait = $("#h1Wait");
                    if (typeof(divWait) !== 'undefined') { divWait.hide(); }
                });
            </script>
            <style>
                .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }                
                .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }              
                .k-grid tbody .k-grid-Edit { min-width: 0; }                         
                .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }       
            </style>
        </div>
    </body>
</html>