<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TariffExceptions.aspx.cs" Inherits="DynamicsTMS365.TariffExceptions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Tariff Exceptions</title>       
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
       

        <% Response.Write(NGLOAuth2); %>

        
         var oTariffClassXrefGrid = null;
         var oTariffDiscountGrid = null;
         var oTariffInterlineGrid = null;
         var oTariffMinChargeGrid = null;
         var oTariffMinWeightGrid = null;
         var oTariffNonServiceGrid = null;
        

         function getCarrTarControl(type){
             var iCarrTarControl = 0;
             try{
                 
                 if (typeof (objvCarrierTariffSummaryDataFields) !== 'undefined' && objvCarrierTariffSummaryDataFields != null) {  
                                        
                     $.each(objvCarrierTariffSummaryDataFields, function (index, item) {
                         if (item.fieldName == "CarrTarControl"){                             
                             var htmlID = item.fieldTagID
                             if (!htmlID) {
                                 return iCarrTarControl
                             } else {
                                 iCarrTarControl = $("#" + htmlID).val();
                                 return iCarrTarControl;   
                            }                               
                         }                        
                     });
                 }
                            
             } catch (err) {
                 ngl.showErrMsg("Create new " + type + " record Failure.  Invalid Carrier Contract.",err, tPage);
             }
             return iCarrTarControl;
         }
        
         function execBeforeTariffClassXrefGridInsert(){
             var blnRet = false;
             var iCarrTarControl = getCarrTarControl("Class Exception");
             if (typeof (iCarrTarControl) !== 'undefined' && iCarrTarControl !== null  && iCarrTarControl !== 0) {
                 try {
                     if (typeof (objTariffClassXrefGridDataFields) !== 'undefined' && objTariffClassXrefGridDataFields != null) {  
                         blnRowSelected = true;                         
                         $.each(objTariffClassXrefGridDataFields, function (index, item) {
                             if (item.fieldName == "CarrTarClassXrefCarrTarControl"){
                                 item.fieldDefaultValue = iCarrTarControl;
                                 blnRet = true;
                                 return blnRet;                                
                             } 
                         });
                     }
          
                 } catch (err) {
                     ngl.showErrMsg("Save new Class Exception record Failure. Contact Technical Support.",err, tPage);
                 }
             }
             return blnRet;
         }
     
         function execBeforeTariffDiscountGridInsert(){
             var blnRet = false;
             var iCarrTarControl = getCarrTarControl("Discount Exception");
             if (typeof (iCarrTarControl) !== 'undefined' && iCarrTarControl !== null  && iCarrTarControl !== 0) {
                 try {
                     if (typeof (objTariffDiscountGridDataFields) !== 'undefined' && objTariffDiscountGridDataFields != null) {  
                         blnRowSelected = true;                         
                         $.each(objTariffDiscountGridDataFields, function (index, item) {
                             if (item.fieldName == "CarrTarDiscountCarrTarControl"){
                                 item.fieldDefaultValue = iCarrTarControl;
                                 blnRet = true;
                                 return blnRet;                                
                             } 
                         });
                     }
          
                 } catch (err) {
                     ngl.showErrMsg("Save new Discount Exception record Failure. Contact Technical Support.",err, tPage);
                 }
             }
             return blnRet;
         }
         
         function execBeforeTariffInterlineGridInsert(){
             var blnRet = false;
             var iCarrTarControl = getCarrTarControl("Interline Exception");
             if (typeof (iCarrTarControl) !== 'undefined' && iCarrTarControl !== null  && iCarrTarControl !== 0) {
                 try {
                     if (typeof (objTariffInterlineGridDataFields) !== 'undefined' && objTariffInterlineGridDataFields != null) {  
                         blnRowSelected = true;                         
                         $.each(objTariffInterlineGridDataFields, function (index, item) {
                             if (item.fieldName == "CarrTarInterlineCarrTarControl"){
                                 item.fieldDefaultValue = iCarrTarControl;
                                 blnRet = true;
                                 return blnRet;                                
                             } 
                         });
                     }
          
                 } catch (err) {
                     ngl.showErrMsg("Save new Interline Exception record Failure. Contact Technical Support.",err, tPage);
                 }
             }
             return blnRet;
         }
        
         function execBeforeTariffMinChargeGridInsert(){
             var blnRet = false;
             var iCarrTarControl = getCarrTarControl("Minimun Charge Exception");
             if (typeof (iCarrTarControl) !== 'undefined' && iCarrTarControl !== null  && iCarrTarControl !== 0) {
                 try {
                     if (typeof (objTariffMinChargeGridDataFields) !== 'undefined' && objTariffMinChargeGridDataFields != null) {  
                         blnRowSelected = true;                         
                         $.each(objTariffMinChargeGridDataFields, function (index, item) {
                             if (item.fieldName == "CarrTarMinChargeCarrTarControl"){
                                 item.fieldDefaultValue = iCarrTarControl;
                                 blnRet = true;
                                 return blnRet;                                
                             } 
                         });
                     }
          
                 } catch (err) {
                     ngl.showErrMsg("Save new Minimun Charge Exception record Failure. Contact Technical Support.",err, tPage);
                 }
             }
             return blnRet;
         }
          
         function execBeforeTariffMinWeightGridInsert(){
             var blnRet = false;
             var iCarrTarControl = getCarrTarControl("Minimun Weight Exception");
             if (typeof (iCarrTarControl) !== 'undefined' && iCarrTarControl !== null  && iCarrTarControl !== 0) {
                 try {
                     if (typeof (objTariffMinWeightGridDataFields) !== 'undefined' && objTariffMinWeightGridDataFields != null) {  
                         blnRowSelected = true;                         
                         $.each(objTariffMinWeightGridDataFields, function (index, item) {
                             if (item.fieldName == "CarrTarMinWeightCarrTarControl"){
                                 item.fieldDefaultValue = iCarrTarControl;
                                 blnRet = true;
                                 return blnRet;                                
                             } 
                         });
                     }
          
                 } catch (err) {
                     ngl.showErrMsg("Save new Minimun Weight Exception record Failure. Contact Technical Support.",err, tPage);
                 }
             }
             return blnRet;
         }
          
         function execBeforeTariffNonServiceGridInsert(){
             var blnRet = false;
             var iCarrTarControl = getCarrTarControl("Non-Service Exception");
             if (typeof (iCarrTarControl) !== 'undefined' && iCarrTarControl !== null  && iCarrTarControl !== 0) {
                 try {
                     if (typeof (objTariffNonServiceGridDataFields) !== 'undefined' && objTariffNonServiceGridDataFields != null) {  
                         blnRowSelected = true;                         
                         $.each(objTariffNonServiceGridDataFields, function (index, item) {
                             if (item.fieldName == "CarrTarNonServiceCarrTarControl"){
                                 item.fieldDefaultValue = iCarrTarControl;
                                 blnRet = true;
                                 return blnRet;                                
                             } 
                         });
                     }
          
                 } catch (err) {
                     ngl.showErrMsg("Save new Minimun Weight Exception record Failure. Contact Technical Support.",err, tPage);
                 }
             }
             return blnRet;
         }

         function toolbar_click(){
             alert('hello');
         }
         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenContract" ){ location.href = "Tariff";
             }else if (btn.id == "btnOpenServices" ){ location.href = "TariffServices";
             }else if (btn.id == "btnOpenRates" ){ location.href = "TariffRates";             
             }else if (btn.id == "btnOpenFees" ){ location.href = "TariffFees";
             }else if (btn.id == "btnOpenFuel" ){ location.href = "TariffFuel";
             }else if (btn.id == "btnOpenNoDrive" ){ location.href = "TariffNoDriveDays";
             }else if (btn.id == "btnOpenHDMs" ){ location.href = "TariffHDMs";
             }else if (btn.id == "btnRefresh" ){
                 if (oTariffClassXrefGrid) { oTariffClassXrefGrid.dataSource.read(); }
                 if (oTariffDiscountGrid) { oTariffDiscountGrid.dataSource.read(); }
                 if (oTariffInterlineGrid) { oTariffInterlineGrid.dataSource.read(); }
                 if (oTariffMinChargeGrid) { oTariffMinChargeGrid.dataSource.read(); }
                 if (oTariffMinWeightGrid) { oTariffMinWeightGrid.dataSource.read(); }
                 if (oTariffNonServiceGrid) { oTariffNonServiceGrid.dataSource.read(); }
             }    
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }

         
         //*************  Call Back Functions ****************
         function TariffClassXrefGridDataBoundCallBack(e,tGrid){           
             oTariffClassXrefGrid = tGrid;
         }
         //*************  Call Back Functions ****************
         function TariffDiscountGridDataBoundCallBack(e,tGrid){           
             oTariffDiscountGrid = tGrid;
         }
         //*************  Call Back Functions ****************
         function TariffInterlineGridDataBoundCallBack(e,tGrid){           
             oTariffInterlineGrid = tGrid;
         }
         //*************  Call Back Functions ****************
         function TariffMinChargeGridDataBoundCallBack(e,tGrid){           
             oTariffMinChargeGrid = tGrid;
         }
         //*************  Call Back Functions ****************
         function TariffMinWeightGridDataBoundCallBack(e,tGrid){           
             oTariffMinWeightGrid = tGrid;
         }
         //*************  Call Back Functions ****************
         function TariffNonServiceGridDataBoundCallBack(e,tGrid){           
             oTariffNonServiceGrid = tGrid;
         }
       
       
         
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
                      
            
            if (control != 0){
                //setTimeout(function () {
                    //add code here to load screen specific information this is only visible when a user is authenticated
                    //var oCRUDCtrl = new nglRESTCRUDCtrl();
                    //var blnRet = oCRUDCtrl.read("Tariff/GetCarrierTariffSummary", 6717, tObj, "readCurrentContractSuccessCallback", "readCurrentContractAjaxErrorCallback");
                //}, 1,this);

            }
            var PageReadyJS = <%=PageReadyJS%>
             //setTimeout(function () {
               menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if  (typeof (divWait) !== 'undefined' ) {
                    divWait.hide();
                }
            //}, 1, this);

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
