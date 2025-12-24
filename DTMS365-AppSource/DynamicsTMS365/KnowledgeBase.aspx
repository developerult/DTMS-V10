<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeBase.aspx.cs" Inherits="DynamicsTMS365.KnowledgeBase" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Knowledge Base</title>         
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
            <div id="vertical" style="height: 98%; width: 98%;">
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
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
                         
                            <div style="margin-left:10px;">
                                <h1>Knowledge Base</h1>
                                <div id="divMainCategories"></div>
                                <div id="divSearchResults"></div>
                            </div>
                            

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

    <% Response.WriteFile("~/Views/AddWhatsNewWnd.html"); %>
    <% Response.Write(PageTemplates); %>
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>          
    <script>    
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';              
        var oWhatsNewGrid = null;        
        var tObj = this;
        var tPage = this;  

        <% Response.Write(NGLOAuth2); %>

           
        <% Response.Write(PageCustomJS); %>


        //*************  execActionClick  ****************
        function execActionClick(btn, proc){                 
            if(btn.id === "btnMngWhatsNew"){                                  
                if (typeof (tPage["wdgtMngWhatsNewDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtMngWhatsNewDialog"])){
                    tPage["wdgtMngWhatsNewDialog"].show();                
                } else{alert("Missing HTML Element (wdgtMngWhatsNewDialog is undefined)");} //Add better error handling here if cm stuff is missing    
            }
            else if (btn.id == "btnRefresh" ){ refresh(); }
            else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }
      
        //*************  Action Menu Functions ****************
        function refresh(){ ngl.readDataSource(oWhatsNewGrid); GetWhatsNewHTML(); }


        //*************  Page Level Functions ****************  
        function execBeforeWhatsNewGridInsert(e,fk,w){
            wndAddWhatsNew.center().open();
            return false; //returning false short circuits the widget Edit function so I can circumvent it and instead call my custom function when the Edit button is clicked            
        }

        function execBeforenotesInsert(e,fk,w){      
            //We need to populate the CarrierContCarrierControl in the new CarrierCont record so we get that from the header row
            var parentRow = $(e.currentTarget).closest("tr.k-detail-row").prev("tr"); // GET PARENT ROW ELEMENT    
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid");            
            var parentModel = parentGrid.dataItem(parentRow); // GET THE PARENT ROW MODEL      
            // ACCESS THE PARENT ROW MODEL ATTRIBUTES
            var pVersion = parentModel.WhatsNewVersion;
            var pControl = parentModel.WhatsNewControl;
            var pFeatureType = parentModel.WhatsNewFeatureTypeControl;
            if (!pVersion) { alert("No Version Field - cannot insert record"); return false;}          
            if (!pControl) { alert("No Parent Control Field - cannot insert record"); return false;}
            if (!pFeatureType) { alert("No Feature Type Field - cannot insert record"); return false;}
            w.SetFieldDefault("WhatsNewVersion",pVersion);
            w.SetFieldDefault("WhatsNewParentID",pControl.toString());
            return w.SetFieldDefault("WhatsNewFeatureTypeControl",pFeatureType.toString());
        }
        
        function GetWhatsNewHTML(){            
            var strVersion = "";
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.filteredRead("WhatsNew/GetWhatsNewReportHTML", strVersion, tObj, "GetWhatsNewHTMLSuccessCallback", "GetWhatsNewHTMLAjaxErrorCallback");  
        }
        
        
        //*************  Call Back Functions ****************
        function WhatsNewGridDataBoundCallBack(e,tGrid){ oWhatsNewGrid = tGrid; } 

        function WhatsNewGridCB(data){ refresh();  }
        function notesCB(data){ refresh();  }

        function GetWhatsNewHTMLAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "GetWhatsNewHTMLAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Get Whats New Report HTML Failure";
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);        
        }
        function GetWhatsNewHTMLSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "GetWhatsNewHTMLSuccessCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "Get Whats New HTML Data Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                this.rData = data.Data;
                                oResults.data = data.Data;
                                oResults.msg = "Success";
                                blnSuccess = true;                               
                                var WNhtml = "";
                                if(ngl.stringHasValue(data.Data[0])){ WNhtml = data.Data[0]; }
                                $("#divWNHTML").html(WNhtml);
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Whats New Report HTML Failure"; }
                    oResults.error = new Error();
                    oResults.error.name = "Get Whats New Report HTML Failure";
                    oResults.error.message = strValidationMsg;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }      
        }


        $(function () {
            //wire focus of all numerictextbox widgets on the page
            $("input[type=number]").bind("focus", function () {
                var input = $(this);
                clearTimeout(input.data("selectTimeId")); //stop started time out if any
                var selectTimeId = setTimeout(function(){ input.select(); });
                input.data("selectTimeId", selectTimeId);
            }).blur(function(e) { clearTimeout($(this).data("selectTimeId")); }); //stop started timeout
        });

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            
            if (control != 0){              
                GetWhatsNewHTML();
            }
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");               
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }      
        });
        </script>
            <style>
                .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
                .k-tooltip { max-height: 500px; max-width: 450px; overflow-y: auto; }
                .k-grid tbody .k-grid-Edit { min-width: 0; }               
                .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }                             
            </style>  
        </div>  
    </body>
</html>