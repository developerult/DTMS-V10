<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierProNumbers.aspx.cs" Inherits="DynamicsTMS365.CarrierProNumbers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Carrier Pro Numbers</title>        
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
         
          //To DO:
          //1. add CarrierProNumber Controller with support for postpageSetting (See Carrier EDI page and LECarrierMaint execAction function)
          //2. Add logic to use Legal Entity Companies to filter data?  
          //3. Check logic for using a template or importing a default configured by super users
          //4. Check logic to read/get carrier pro; verify that Legal Entity specific data is validated
          //5. Optionally add action logic (Super -- Creates a default) to apply to all legal entities (save as default) on this page, this will set the Legal Entity to zero
          //6. Optionally add action logic to save this (template) to the active legal entity? may need to be the default behavior
          //7. Optionally add action logic to import defaults, use carrier default where Legal Entity is zero.
          
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
         var oCarrierEDIGrid = null;
         var tObj = this;
         var tPage = this;
         var oCarrierEquipList = null;   

        <% Response.Write(NGLOAuth2); %>

         
         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc) {
             if (btn.id == "btnDeleteCarPro") {
                 alert('Coming Soon');
             }
             else if (btn.id == "btnRefresh") {
                 ngl.readDataSource(oCarrierEDIGrid);//oCarrierEDIGrid.dataSource.read();
             }
             else if (btn.id == "btnResetCurrentUserConfig") {
                 resetCurrentUserConfig(PageControl);
             }
             else if (btn.id == "btnOpenCarrier") {
                 location.href = "LECarrierMaint";
             }
             else if (btn == 'btnUPSMod7') { QuickSetup(btn, proc); }
             else if (btn == 'btnYRCMod11') { QuickSetup(btn, proc); }
             else if (btn == 'btnFedExMod7') { QuickSetup(btn, proc); }
             else if (btn == 'btnConWayMod7') { QuickSetup(btn); }
             else if (btn == 'btnOldDomMod10') { QuickSetup(btn, proc); }
         }


         function QuickSetup(type, proc) {
             switch (type) {

                 case 'btnUPSMod7':
                     //debugger;
                     //console.log("tPage[''" + proc + "''].GetFieldID(''" + CarrProChkDigAlgControl + "'')");
                     var id = tPage['' + proc + ''].GetFieldID('CarrProChkDigAlgControl');
                     console.log('id');
                     console.log(id)                     
                     var iDom = $("#" + id).data("kendoDropDownList");
                     console.log('iDom');
                     console.log(iDom.select());

                     if (iDom) {
                         iDom.select(function (dataItem) {
                             return dataItem.Name === "Mod 7";
                         });
                         //iDom.select('Mod 7');//    .CarrProChkDigAlgControl = 2 'Mod 7
                         //iDom.trigger("change");
                     }
                     ////        .CarrProName = "Sample UPS Freight (Mod 7)"  -- skip
                     id = tPage['' + proc + ''].GetFieldID('CarrProDesc');
                     $("#" + id).data("kendoMaskedTextBox").value('Sample Record (Unsaved)'); // .CarrProDesc = "Sample Record (Unsaved)"
                     //                .CarrProPrefix = Nothing
                     //                    .CarrProPrefixSpacer = Nothing
                     //                        .CarrProSuffix = Nothing
                     //                            .CarrProSuffixSpacer = Nothing
                     //                                .CarrProCheckDigitSpacer = Nothing
                     //                                    .CarrProPrintCheckDigitOnSeperateBarCode = False
                     //                                        .CarrProPrintSpacersOnBarCode = False
                     //                                            .CarrProAppendPrefixForCheckDigit = False
                     //                                                .CarrProAppendSuffixForCheckDigit = False
                     //                                                    .CarrProSeedStart = 1234 'sample number
                     //                                                        .CarrProSeedEnd = 1254      'allow 20 numbers
                     //                                                            .CarrProSeedCurrent = 0 'reset current
                     //                                                                .CarrProSeedStepFactor = 1
                     //                                                                    .CarrProSeedWarningSeed = 1244 'warn when 10 are left
                     //                                                                        .CarrProLength = 8
                     //                                                                            .CarrProCheckDigitWeightFactor = Nothing
                     //                                                                                .CarrProCheckDigitSplitWeightFactorDigits = True
                     //                                                                                    .CarrProCheckDigitUseIndexForWeightFactor = False
                     //                                                                                        .CarrProCheckDigitIndexForWeightFactorMin = 2
                     //                                                                                            .CarrProCheckDigitErrorCode = "E"
                     //                                                                                                .CarrProCheckDigit10Code = Nothing
                     //                                                                                                    .CarrProCheckDigitOver10Code = Nothing
                     //                                                                                                        .CarrProCheckDigitZeroCode = "0"
                     //                                                                                                            .CarrProCheckDigitSubtractionFactor = 0
                     //                                                                                                                .CarrProCheckDigitUseSubtractionFactor = False
                     break;
                     //Case QuickSetupOpt.YRC
             //    .CarrProChkDigAlgControl = 4 'Mod 11
             //        .CarrProName = "Sample YRC (Mod 11)"
             //            .CarrProDesc = "Sample Record (Unsaved)"
             //                .CarrProPrefix = "100"
             //                    .CarrProPrefixSpacer = "-"
             //                        .CarrProSuffix = Nothing
             //                            .CarrProSuffixSpacer = Nothing
             //                                .CarrProCheckDigitSpacer = "-"
             //                                    .CarrProPrintCheckDigitOnSeperateBarCode = False
             //                                        .CarrProPrintSpacersOnBarCode = False
             //                                            .CarrProAppendPrefixForCheckDigit = False
             //                                                .CarrProAppendSuffixForCheckDigit = False
             //                                                    .CarrProSeedStart = 340397 'sample number
             //                                                        .CarrProSeedEnd = 340417      'allow 20 numbers
             //                                                            .CarrProSeedCurrent = 0 'reset current
             //                                                                .CarrProSeedStepFactor = 1
             //                                                                    .CarrProSeedWarningSeed = 340407 'warn when 10 are left
             //                                                                        .CarrProLength = 6
             //                                                                            .CarrProCheckDigitWeightFactor = Nothing
             //                                                                                .CarrProCheckDigitSplitWeightFactorDigits = True
             //                                                                                    .CarrProCheckDigitUseIndexForWeightFactor = False
             //                                                                                        .CarrProCheckDigitIndexForWeightFactorMin = 2
             //                                                                                            .CarrProCheckDigitErrorCode = "E"
             //                                                                                                .CarrProCheckDigit10Code = "X"
             //                                                                                                    .CarrProCheckDigitOver10Code = "Y"
             //                                                                                                        .CarrProCheckDigitZeroCode = "0"
             //                                                                                                            .CarrProCheckDigitSubtractionFactor = 11
             //                                                                                                                .CarrProCheckDigitUseSubtractionFactor = True
             //Case QuickSetupOpt.FedExFreight
             //    .CarrProChkDigAlgControl = 2 'Mod 7
             //        .CarrProName = "Sample FedEx Freight (Mod 7)"
             //            .CarrProDesc = "Sample Record (Unsaved)"
             //                .CarrProPrefix = ""
             //                    .CarrProPrefixSpacer = ""
             //                        .CarrProSuffix = ""
             //                            .CarrProSuffixSpacer = ""
             //                                .CarrProCheckDigitSpacer = ""
             //                                    .CarrProPrintCheckDigitOnSeperateBarCode = False
             //                                        .CarrProPrintSpacersOnBarCode = False
             //                                            .CarrProAppendPrefixForCheckDigit = False
             //                                                .CarrProAppendSuffixForCheckDigit = False
             //                                                    .CarrProSeedStart = 1234 'sample number
             //                                                        .CarrProSeedEnd = 1254      'allow 20 numbers
             //                                                            .CarrProSeedCurrent = 0 'reset current
             //                                                                .CarrProSeedStepFactor = 1
             //                                                                    .CarrProSeedWarningSeed = 1244 'warn when 10 are left
             //                                                                        .CarrProLength = 9
             //                                                                            .CarrProCheckDigitWeightFactor = Nothing
             //                                                                                .CarrProCheckDigitSplitWeightFactorDigits = True
             //                                                                                    .CarrProCheckDigitUseIndexForWeightFactor = False
             //                                                                                        .CarrProCheckDigitIndexForWeightFactorMin = 2
             //                                                                                            .CarrProCheckDigitErrorCode = "E"
             //                                                                                                .CarrProCheckDigit10Code = Nothing
             //                                                                                                    .CarrProCheckDigitOver10Code = Nothing
             //                                                                                                        .CarrProCheckDigitZeroCode = Nothing
             //                                                                                                            .CarrProCheckDigitSubtractionFactor = 0
             //                                                                                                                .CarrProCheckDigitUseSubtractionFactor = False
             //Case QuickSetupOpt.ConWay
             //    .CarrProChkDigAlgControl = 2 'Mod 7
             //        .CarrProName = "Sample Conway (Mod 7)"
             //            .CarrProDesc = "Sample Record (Unsaved)"
             //                .CarrProPrefix = "100"
             //                    .CarrProPrefixSpacer = "-"
             //                        .CarrProSuffix = Nothing
             //                            .CarrProSuffixSpacer = Nothing
             //                                .CarrProCheckDigitSpacer = Nothing
             //                                    .CarrProPrintCheckDigitOnSeperateBarCode = False
             //                                        .CarrProPrintSpacersOnBarCode = False
             //                                            .CarrProAppendPrefixForCheckDigit = True
             //                                                .CarrProAppendSuffixForCheckDigit = False
             //                                                    .CarrProSeedStart = 12345 'sample number
             //                                                        .CarrProSeedEnd = 12365      'allow 20 numbers
             //                                                            .CarrProSeedCurrent = 0 'reset current
             //                                                                .CarrProSeedStepFactor = 1
             //                                                                    .CarrProSeedWarningSeed = 12355 'warn when 10 are left
             //                                                                        .CarrProLength = 8
             //                                                                            .CarrProCheckDigitWeightFactor = Nothing
             //                                                                                .CarrProCheckDigitSplitWeightFactorDigits = True
             //                                                                                    .CarrProCheckDigitUseIndexForWeightFactor = False
             //                                                                                        .CarrProCheckDigitIndexForWeightFactorMin = 2
             //                                                                                            .CarrProCheckDigitErrorCode = "E"
             //                                                                                                .CarrProCheckDigit10Code = Nothing
             //                                                                                                    .CarrProCheckDigitOver10Code = Nothing
             //                                                                                                        .CarrProCheckDigitZeroCode = "0"
             //                                                                                                            .CarrProCheckDigitSubtractionFactor = 0
             //                                                                                                                .CarrProCheckDigitUseSubtractionFactor = False
             //Case QuickSetupOpt.OldDom
             //    .CarrProChkDigAlgControl = 3 'Mod 10
             //        .CarrProName = "Sample Old Dominion (Mod 10)"
             //            .CarrProDesc = "Sample Record (Unsaved)"
             //                .CarrProPrefix = Nothing
             //                    .CarrProPrefixSpacer = Nothing
             //                        .CarrProSuffix = Nothing
             //                            .CarrProSuffixSpacer = Nothing
             //                                .CarrProCheckDigitSpacer = Nothing
             //                                    .CarrProPrintCheckDigitOnSeperateBarCode = False
             //                                        .CarrProPrintSpacersOnBarCode = False
             //                                            .CarrProAppendPrefixForCheckDigit = False
             //                                                .CarrProAppendSuffixForCheckDigit = False
             //                                                    .CarrProSeedStart = 1006371953 'sample number
             //                                                        .CarrProSeedEnd = 1006371973      'allow 20 numbers
             //                                                            .CarrProSeedCurrent = 0 'reset current
             //                                                                .CarrProSeedStepFactor = 1
             //                                                                    .CarrProSeedWarningSeed = 1006371963 'warn when 10 are left
             //                                                                        .CarrProLength = 10
             //                                                                            .CarrProCheckDigitWeightFactor = 1212121212
             //                                                                                .CarrProCheckDigitSplitWeightFactorDigits = True
             //                                                                                    .CarrProCheckDigitUseIndexForWeightFactor = False
             //                                                                                        .CarrProCheckDigitIndexForWeightFactorMin = 2
             //                                                                                            .CarrProCheckDigitErrorCode = "E"
             //                                                                                                .CarrProCheckDigit10Code = Nothing
             //                                                                                                    .CarrProCheckDigitOver10Code = Nothing
             //                                                                                                        .CarrProCheckDigitZeroCode = "0"
             //                                                                                                            .CarrProCheckDigitSubtractionFactor = 10
             //        
                 default:
             //        var id = tPage['' + proc + ''].GetFieldID('' + CarrProCheckDigitUseSubtractionFactor + ''); // = True
             //        $("#" + id).data("kendoSwitch").value({ checked: true });
             }

         }

         
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
             $("#txtHint").on("mouseover", function (event) { ngl.showNGLTooltip(event, 'Tool Tip Text',this);});
             //$("#txtHint").on("mousemove", changeNGLTooltipPosition);
             $("#txtHint").on("mouseout", ngl.hideNGLTooltip);
             //eHint.addEventListener("mouseover", "changeNGLTooltipPosition");


           
            if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                //}, 1,this);

            }
            
           var PageReadyJS = <%=PageReadyJS%>;
             //setTimeout(function () {
               menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if  (typeof (divWait) !== 'undefined' ) {
                    divWait.hide();
                }
            //}, 1, this);
             //$("#id147120230104214935660890").kendoTooltip({
             //    content: "Hello!",
             //    show: onShow,
             //    hide: onHide,
             //    position: "top"
             //});


             var showTooltip = function (event) {
                 alert("show tool tip");
                 $("#NGLTooltip").hide();
                 //$('div.NGLTooltip').remove();
                 //$('<div class="NGLTooltip">I\' am tooltips! tooltips! tooltips! :)</div>').appendTo('mytip');
                 changeTooltipPosition(event);
                 $("#NGLTooltip").show();
             };

             var changeTooltipPosition = function (event) {
                 alert("change tool tip");
                 var tooltipX = event.pageX - 8;
                 var tooltipY = event.pageY + 8;
                 $('#NGLTooltip').css({ top: tooltipY, left: tooltipX });
             };

             var hideTooltip = function () {
                 alert("hide tool tip");
                 $("#NGLTooltip").hide();
                 //$('div.NGLTooltip').remove();
             };

             //$("#hint").bind({
             //    mousemove: changeTooltipPosition,
             //    mouseenter: showTooltip,
             //    mouseleave: hideTooltip
             //});

             //$("#username'").bind({
             //    mousemove: changeTooltipPosition,
             //    mouseenter: showTooltip,
             //    mouseleave: hideTooltip
             //});
             

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

      #txtHint{
		cursor:pointer;
	    }

     

    </style>
    
    </div>

         
</body>
</html>