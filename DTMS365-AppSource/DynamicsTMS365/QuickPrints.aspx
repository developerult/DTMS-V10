<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuickPrints.aspx.cs" Inherits="DynamicsTMS365.QuickPrints" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Quick Prints</title>         
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


                            <div class="k-block" style="text-align:center; margin-left:10px; margin-right:10px; margin-top:10px; width:auto; min-width: 450px; padding-left:2px; padding-right:2px;">
                                <h2>Click the button in the Report Menu to reopen the last report</h2>
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

    <% Response.WriteFile("~/Views/DispatchReport.html"); %>
    <% Response.WriteFile("~/Views/BOLReport.html"); %> 
    <% Response.Write(PageTemplates); %>
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>          
    <script>    
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        
        var bkCntrl = <%=BKCntrl%>;
        var rpt = <%=Rpt%>;              
        var tObj = this;
        var tPage = this;           
      

        <% Response.Write(NGLOAuth2); %>

        


        <% Response.Write(PageCustomJS); %>

        // start widgit configuration
        var winBOLReport  = kendo.ui.Window;
        var oBOLReportCtrl = new BOLReportCtrl();
        var winDispatchReport = kendo.ui.Window;
        var oDispatchReportCtrl = new DispatchingReportCtrl();

        //*************  execActionClick  ****************
        function execActionClick(btn, proc){         
            if (btn.id == "btnRefresh" ){ refresh(); }
            else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        //************* Report Mapping **********************
        function execReportClick(btn, proc){
            if(btn.id == "btnCAR120" ){ PrintSelectedBOL(); }
            else if(btn.id == "btnDispatchReport" ){ openDispatchReport(); }
            else if(btn.id == "btnCAR106" ){ openLoadTenderReport(iBookPK); }          
        }



        function openDispatchReport(){
            if (typeof (bkCntrl) !== 'undefined' || bkCntrl !== null || bkCntrl !== 0) {
                if (typeof (oDispatchReportCtrl) !== 'undefined' && ngl.isObject(oDispatchReportCtrl)) {
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.read("Dispatching/GetDispatchReportData", bkCntrl, tPage, "GetDispatchReportDataSuccessCallback", "GetDispatchReportDataAjaxErrorCallback",true);  
                }; 
            }
        }

        function openLoadTenderReport(){
            ngl.showInfoNotification("Feature Unavailable", "Load Tender Report coming soon", null);
        }

        function PrintSelectedBOL(){
            if (typeof (bkCntrl) !== 'undefined' || bkCntrl !== null || bkCntrl !== 0) {
                if (typeof (oBOLReportCtrl) !== 'undefined' && ngl.isObject(oBOLReportCtrl)) {
                    oBOLReportCtrl.readByBookControl(bkCntrl)
                }; 
            }                         
        }


        function oBOLReportSelectCB(results){ return; }
        function oBOLReportSaveCB(results){ return; }
        function oBOLReportCloseCB(results){ return; }
        function oBOLReportReadCB(results){ return; }

        function oDispatchingReportSelectCB(results){ return; }
        function oDispatchingReportSaveCB(results){ return; }
        function oDispatchingReportCloseCB(results){          
            try{
                var blnTryAgain = false;
                if (typeof(results.data) !== 'undefined' && ngl.isObject(results.data) && typeof(results.data.ErrorCode) !== 'undefined' && results.data.ErrorCode !== null ){                
                    if(!ngl.isNullOrWhitespace(results.data.ErrorCode) && !isNaN(results.data.ErrorCode)){
                        ////get the bid type
                        //if (typeof(results.data.DispatchBidType) !== 'undefined' && results.data.DispatchBidType !== null && !ngl.isNullOrWhitespace(results.data.DispatchBidType) && !isNaN(results.data.DispatchBidType)){
                        //    //possible bid types
                        //    //1	NextStop
                        //    //2	NGLTar
                        //    //3	P44
                        //    //4	Spot
                        //    //if (results.data.DispatchBidType.toString() == "2" || results.data.DispatchBidType.toString() == "3"){
                        //    //    ////if we have a loadTenderControl reopen the quote window so user can select a new quote
                        //    //    //if (typeof(results.data.LoadTenderControl) !== 'undefined' && results.data.LoadTenderControl !== null && !ngl.isNullOrWhitespace(results.data.LoadTenderControl) && !isNaN(results.data.LoadTenderControl)){
                        //    //    //    if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])){
                        //    //    //        sBidLoadTenderControlVal = results.data.LoadTenderControl;
                        //    //    //        tPage["wdgtQuotesWndDialog"].read(results.data.LoadTenderControl);
                        //    //    //    } 
                        //    //    //}
                        //    //    return;
                        //    //} else { execActionClick("Refresh"); return; } //just refresh the data and exit
                        //}                       
                    }                  
                }
                //if we get here just show the BOL and refresh the data               
                //if(!blnBtnDispatchReportClicked){ PrintSelectedBOL(); } //not clicked so show BOL aka report shown after dispatching
                //blnBtnDispatchReportClicked = false; //reset the value
                //("Refresh");                
            } catch (err) {
                //do nothing
            }
            return;
        }
        function oDispatchingReportReadCB(results){ return; }

        function GetDispatchReportDataSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "GetDispatchReportDataSuccessCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                        oResults.error = new Error();
                        oResults.error.name = "Get Dispatch Report Data Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else{ 
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                oResults.msg = 'Success';
                                var dArray = [];
                                for (i = 0; i < data.Data.length; i++) {
                                    var d = new Dispatch();
                                    d = data.Data[i];    
                                    dArray.push(d);
                                }
                                oDispatchReportCtrl.data = dArray;
                                oDispatchReportCtrl.show();
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Dispatch Report Data Failure"; }
                    ngl.showErrMsg("Get Dispatch Report Data Failure", strValidationMsg, null);
                }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }           
        }
        function GetDispatchReportDataAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "GetDispatchReportDataAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Get Dispatch Report Data Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        }
      

        //*************  Call Back Functions ****************
        function savePostPageSettingSuccessCallback(results){ return; } //for now do nothing when we save the pk
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){ return; } //for now do nothing when we save the pk


        var QuickPrintReportHTML365 = {
            BOL: 0,
            Dispatch: 1
        }

      

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            

            if (control != 0){

                oBOLReportCtrl = new BOLReportCtrl(); 
                oBOLReportCtrl.loadDefaults(winBOLReport,oBOLReportSelectCB,oBOLReportSaveCB,oBOLReportCloseCB,oBOLReportReadCB);

                oDispatchReportCtrl = new DispatchingReportCtrl();
                oDispatchReportCtrl.loadDefaults(winDispatchReport,oDispatchingReportSelectCB,oDispatchingReportSaveCB,oDispatchingReportCloseCB,oDispatchingReportReadCB);

                switch (rpt) {
                    case QuickPrintReportHTML365.BOL:
                        PrintSelectedBOL();
                        break;
                    case QuickPrintReportHTML365.Dispatch:  
                        openDispatchReport();
                        break;
                    default:
                        break;
                }


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
